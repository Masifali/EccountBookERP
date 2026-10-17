package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository;
import com.mst.repositories.ProductionSettlementRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.Prod871FoodProductionSaver.FormMessage;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_CONSUMPTION;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_INPUT;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_OUTPUT;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_OVERHEAD;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_PACKING_MATERIAL;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_PRE_COSTING_JOB_ORDER;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_SETTLEMENT;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.bool;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.col;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.str;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.toDouble;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.toInt;

/**
 * Screen 871 "Production Against (Pre Costing Job Order)" - Production hub, module 18.
 * Architecture.WinApp.Production/ProductionAgainstPreCostingJobOrder.cs (20,162 lines; designer from :11399).
 *
 * One form, eight tab pages (tabControl1: Input, OutPut, Consumption, Packing Material, Over Head,
 * Transaction History, Summary, Settlement), each transaction tab with a bottom "Form" / "History"
 * tab strip. Every grid is owned by the form itself (no hosted child forms, unlike screen 280).
 *
 * What frmFoodProduction_Load:1227 decides is served by {@link #state()}: the four configurations,
 * the wages flags for 80/112/181, the rights object (Save/Update/Print/Rate/ProductionSettlement), the
 * FIFO feature (shows Difference Account, hides both "Pending For Rates" buttons, disables the
 * consumption Rate box), and - when the ProductionSettlement right is missing - the removal of the
 * Settlement and Over Head tabs (:1307).
 *
 * Form-level validation (FormValidation* and the row checks of Insert*) is repeated here with the
 * desktop's own messages, so a direct call cannot skip what the page checks first.
 *
 * Deviations (web-only, with reason): doc numbers are regenerated server side on Save (the desktop's
 * read-only text box); job order ids must be rows of usp_getJobOrdersForProduction for the session
 * branch; the loader forms (LoadInputDataByJobOrder, LoadOutPutDataByJobOrder,
 * LoadPackingMaterialDataByJobOrder, LoadOverHeadsDataByJobOrder, LoadOutPutPendingforRates,
 * LoadConsumptionPendingforRates, LoadavailableTransactionsForIssuanceOnConsumption) are not in the
 * recovered tree - they are in-page modals over the procedures their names map to, with the columns
 * the form reads from their dtIssuance (:2896, :4506, :5315, :6234, :10459).
 */
@Service
public class Prod871ProductionAgainstPreCostingJobOrderService {

    public static final int SCREEN_ID = 871;
    private static final int ERP_FEATURE_FIFO_CGS = 5;
    private static final String CONFIG_REQUEST =
            "WagesCompulsoryOnProduction,IssuanceByLoader,IsManualEntryOnInputNotAllowed,ByProductRateEditableIsAllow";

    private final Prod871ProductionAgainstPreCostingJobOrderRepository repo;
    private final Prod871FoodProductionSaver saver;
    private final CurrentUserContext ctx;

    public Prod871ProductionAgainstPreCostingJobOrderService(Prod871ProductionAgainstPreCostingJobOrderRepository repo,
                                                             Prod871FoodProductionSaver saver, CurrentUserContext ctx) {
        this.repo = repo;
        this.saver = saver;
        this.ctx = ctx;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }
    private int org() { return ctx.currentOrganizationId(); }
    private int comp() { return ctx.currentCompanyId(); }
    private int branch() { return ctx.currentBranchId(); }
    private int year() { return ctx.currentFinancialYearId(); }
    private int uid() { return ctx.currentUserId(); }

    // ================================================================================= rights

    /**
     * CommonServices.SetRightsValueInRightsObject("ProductionAgainstPreCostingJobOrder") (:1267):
     * the role spelled "Admin" starts with Save/Update/Delete/Print; the grant rows then set Save, Update,
     * Print, Delete, "Rate" (DoHaveCanRateandAmount) and "ProductionSettlement" (DoHaveCanSettlementtab).
     */
    public Map<String, Boolean> rights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        r.put("save", admin); r.put("update", admin); r.put("delete", admin); r.put("print", admin);
        r.put("rateAndAmount", false); r.put("settlementTab", false);
        r.put("saveLayout", true); r.put("gridPrint", true); r.put("gridExport", true);
        r.put("groupCollapse", true); r.put("groupExpand", true);
        try {
            for (Map<String, Object> row : repo.userRights(uid(), Prod871ProductionAgainstPreCostingJobOrderRepository.DESKTOP_SCREEN_NAME, role, comp())) {
                String name = str(col(row, "RightName")).trim();
                boolean v = bool(col(row, "Value"));
                switch (name) {
                    case "Save": r.put("save", admin || v); break;
                    case "Update": r.put("update", admin || v); break;
                    case "Print": r.put("print", admin || v); break;
                    case "Delete": r.put("delete", v); break;
                    case "Rate": r.put("rateAndAmount", v); break;
                    case "ProductionSettlement": r.put("settlementTab", v); break;
                    case "SaveLayout": r.put("saveLayout", v); break;
                    case "GridPrint": r.put("gridPrint", v); break;
                    case "GridExport": r.put("gridExport", v); break;
                    case "GridGroupCollapse": r.put("groupCollapse", v); break;
                    case "GridGroupExpand": r.put("groupExpand", v); break;
                    default: break;
                }
            }
        } catch (RuntimeException ignored) { /* the defaults above stay */ }
        return r;
    }

    // ================================================================================== state

    /** frmFoodProduction_Load:1227 - switches, rights, the combos every tab shares, the history pickers. */
    public Map<String, Object> state() {
        int org = org(), comp = comp(), branch = branch(), fy = year();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenId", SCREEN_ID);
        out.put("title", "Production Against (Pre Costing Job Order)");

        boolean wIn = false, wOut = false, wCons = false;
        try {
            for (Map<String, Object> r : repo.wagesRefDocumentStatuses()) {
                int id = toInt(col(r, "RefDocumentTypeId"));
                boolean a = bool(col(r, "IsActive"));
                if (id == DOC_INPUT) wIn = a;
                if (id == DOC_OUTPUT) wOut = a;
                if (id == DOC_CONSUMPTION) wCons = a;
            }
        } catch (RuntimeException ignored) { }
        out.put("wagesActiveForInput", wIn);
        out.put("wagesActiveForOutput", wOut);
        out.put("wagesActiveForConsumption", wCons);

        boolean wagesStatus = false, loaderStatus = false, manualNotAllowed = false, byProductRateEditable = false;
        try {
            for (Map<String, Object> r : repo.multipleConfigurations(org, comp, CONFIG_REQUEST)) {
                String n = str(col(r, "ConfigDescription")).trim();
                boolean v = bool(col(r, "ConfigKey"));
                if ("WagesCompulsoryOnProduction".equals(n)) wagesStatus = v;
                if ("IssuanceByLoader".equals(n)) loaderStatus = v;
                if ("IsManualEntryOnInputNotAllowed".equals(n)) manualNotAllowed = v;
                if ("ByProductRateEditableIsAllow".equals(n)) byProductRateEditable = v;
            }
        } catch (RuntimeException ignored) { }
        out.put("wagesCompulsoryOnProduction", wagesStatus);
        out.put("issuanceByLoader", loaderStatus);
        out.put("isManualEntryOnInputNotAllowed", manualNotAllowed);
        out.put("byProductRateEditable", byProductRateEditable);
        boolean fifo = false;
        try { fifo = repo.erpFeature(org, comp, ERP_FEATURE_FIFO_CGS); } catch (RuntimeException ignored) { }
        out.put("fifoCgs", fifo);
        Map<String, Boolean> rights = rights();
        out.put("rights", rights);

        out.put("today", LocalDate.now().toString());
        out.put("historyFrom", LocalDate.now().minusDays(3).toString());      /* DateTime.Now.AddDays(-3.0) :1495 */
        Object start = null;
        try { start = repo.financialYearStart(org, comp, fy); } catch (RuntimeException ignored) { }
        out.put("financialYearStart", day(start));

        /* CommondtForCombosFill:1539 */
        out.put("jobLots", project(repo.jobLotsForBranch(org, comp, branch), "Id", "JobLotDescription"));
        out.put("warehousesOutput", project(repo.warehousesForBranch(org, comp, branch), "Id", "WareHouseName"));
        out.put("warehousesPm", project(repo.warehousesForBranchPm(org, comp, branch), "Id", "WareHouseName"));
        out.put("stockAccounts", project(repo.accountTitlesByAccountTypeIds(org, comp, ctx.currentAppId(), uid(), "4"), "Id", "AccountTitle"));
        out.put("differenceAccounts", project(repo.accountTitlesByAccountTypeIds(org, comp, ctx.currentAppId(), uid(), "4,12"), "Id", "AccountTitle"));
        out.put("cropYears", project(repo.cropYears(org, comp), "Id", "CropYear"));
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
        List<Map<String, Object>> entryTypes = new ArrayList<>();
        entryTypes.add(pair("Id", 1, "Name", "ByProduct"));
        entryTypes.add(pair("Id", 2, "Name", "FinishGoods"));
        out.put("entryTypes", entryTypes);
        List<Map<String, Object>> branches = project(repo.branchesForUser(org, comp, uid()), "BranchId", "BranchName");
        out.put("branches", branches);
        /* BranchesFill:6497 - cmbBranchName.Text = UserAccount.BranchName (the session branch is pre-ticked) */
        String userBranch = "";
        for (Map<String, Object> b : branches) if (toInt(b.get("BranchId")) == branch) userBranch = str(b.get("BranchName"));
        out.put("userBranchName", userBranch);
        out.put("userBranchId", branch);
        out.put("departments", project(repo.productionDepartments(org, comp), "Id", "WareHouseName"));
        out.put("uomSchedules", project(repo.uomSchedules(org, comp), "Id", "ItemId", "UOMCode", "Equivalent"));
        out.put("uomStatic", project(repo.uomStatic(), "Id", "UOM", "Equivalent"));
        out.put("jobOrders", jobOrders());
        out.put("summaryJobOrders", summaryJobOrders(String.valueOf(branch)));
        out.put("settlementJobOrders", settlementJobOrders(1));
        out.put("inputHistoryJobOrders", historyJobOrders(String.valueOf(DOC_INPUT)));
        out.put("outputHistoryJobOrders", historyJobOrders(String.valueOf(DOC_OUTPUT)));
        out.put("consumptionHistoryJobOrders", historyJobOrders(String.valueOf(DOC_CONSUMPTION)));
        out.put("transactionHistoryCombos", transactionHistoryCombos());
        List<Map<String, Object>> chargesTypes = new ArrayList<>();
        chargesTypes.add(pair("Id", 1, "Type", "Processing Charges"));
        chargesTypes.add(pair("Id", 2, "Type", "Other Charges"));
        out.put("chargesTypes", chargesTypes);
        boolean canSave = Boolean.TRUE.equals(rights.get("save"));
        out.put("docNoInput", canSave ? serial(DOC_INPUT) : 0);
        out.put("docNoOutput", canSave ? serial(DOC_OUTPUT) : 0);
        out.put("docNoConsumption", canSave ? serial(DOC_CONSUMPTION) : 0);
        out.put("docNoPm", canSave ? repo.packingMaterialCode(org, comp, branch, fy) : 0);
        out.put("docNoOh", canSave ? repo.overheadCode(org, comp, branch, fy) : 0);
        return out;
    }

    /** JobOrderNoFill:1585 - Id, PlanCode, DocumentTypeId (hidden). */
    public List<Map<String, Object>> jobOrders() {
        return project(repo.jobOrdersForProduction(org(), comp(), branch()), "Id", "PlanCode", "DocumentTypeId");
    }

    /** JobOrderNoFillForSummery:1669 - the ticked branches as ",1,2". */
    public List<Map<String, Object>> summaryJobOrders(String branchIds) {
        return project(repo.jobOrdersForSummary(org(), comp(), year(), branchIds), "Id", "PlanCode");
    }

    /** JobOrderNoFillForSettlement(ActionId):1708 - DocumentTypeId 401; SettlementDate travels with the row (:9099). */
    public List<Map<String, Object>> settlementJobOrders(int actionId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.jobOrdersAll(org(), comp(), year(), actionId, DOC_PRE_COSTING_JOB_ORDER)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(col(r, "Id")));
            m.put("PlanCode", str(col(r, "PlanCode")));
            m.put("ApprovalStatus", str(col(r, "ApprovalStatus")));
            m.put("SettledStatus", str(col(r, "SettledStatus")));
            m.put("SettlementDate", day(col(r, "SettlementDate")));
            out.add(m);
        }
        return out;
    }

    /** jobOrderInputHCombobind / OutputH / ConsumptionH (:2934 / :4042 / :10823) - Activity "JobOrder". */
    public List<Map<String, Object>> historyJobOrders(String documentTypeIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDownFromFoodProduction(org(), comp(), year(), documentTypeIds, String.valueOf(branch()), "JobOrder")) {
            out.add(pair("Id", toInt(col(r, "Id")), "JobOrder", str(col(r, "ReferenceName"))));
        }
        return out;
    }

    /** jobOrderTransactionHistoryCombobind:4080 - DocumentTypeIds "80,112", rows split by Activity. */
    public Map<String, Object> transactionHistoryCombos() {
        List<Map<String, Object>> jobs = new ArrayList<>(), plants = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDownFromFoodProduction(org(), comp(), year(), "80,112", String.valueOf(branch()), null)) {
            String a = str(col(r, "Activity"));
            if ("JobOrder".equals(a)) jobs.add(pair("Id", toInt(col(r, "Id")), "JobOrder", str(col(r, "ReferenceName"))));
            else if ("Plant".equals(a)) plants.add(pair("Id", toInt(col(r, "Id")), "Plant", str(col(r, "ReferenceName"))));
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("jobOrders", jobs);
        m.put("plants", plants);
        return m;
    }

    /** GenerateDocNumberInput / OutPut / Consumption (:2161, :3308, :9195). */
    public int serial(int documentTypeId) {
        return repo.serialNumber(org(), comp(), branch(), documentTypeId, year());
    }

    public int packingMaterialCode() { return repo.packingMaterialCode(org(), comp(), branch(), year()); }
    public int overheadCode() { return repo.overheadCode(org(), comp(), branch(), year()); }

    // ========================================================================= job order cascade

    /** CmbJobOrderNo_Leave:2191 - WIP account / WIP item / FinishGoodRate, then GetPlantFeeder. */
    public Map<String, Object> jobOrderCascade(int jobOrderId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> gl = jobOrderId > 0 ? repo.glAccountsByJobOrderId(org(), comp(), jobOrderId) : new ArrayList<>();
        out.put("wipAccounts", project(gl, "WorkInProccessAcId", "WorkInProcessAc"));
        out.put("wipItems", project(gl, "WipItemId", "ItemName"));
        out.put("finishGoodRate", gl.isEmpty() ? 0d : toDouble(col(gl.get(0), "FinishGoodRate")));
        out.put("plants", plants(jobOrderId));
        return out;
    }

    /** GetPlantFeeder:1755 - PlantsForProductionByJobOrderId. */
    public List<Map<String, Object>> plants(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return project(repo.plantsForJobOrder(org(), comp(), year(), branch(), jobOrderId), "PlantId", "PlantName");
    }

    /** OutPutPlantFillFromProductionInput:1831. */
    public List<Map<String, Object>> plantsFromInput(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return project(repo.plantsFromProductionInput(jobOrderId, branch()), "PlantId", "PlantName");
    }

    /** cmbsummeryJobOrderNo_TextChanged:6527 - GetPlantByJobOrderId. */
    public List<Map<String, Object>> plantsForSummary(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return project(repo.plantByJobOrderId(jobOrderId, branch()), "PlantId", "PlantName");
    }

    /** GetInPutTotalQtyandWeightByJobOrderId rows (EntryType, EntryTypeOutPut, ItemQty, StockWeight) - the page sums them (:3368, :4612, :5395). */
    public List<Map<String, Object>> inputTotals(int jobOrderId, int plantId) {
        return project(repo.inputTotals(org(), comp(), jobOrderId, plantId), "EntryType", "EntryTypeOutPut", "ItemQty", "StockWeight");
    }

    // ================================================================================= loaders

    public List<Map<String, Object>> loadInput(int jobOrderId) { return plain(repo.loadInputByJobOrder(org(), comp(), jobOrderId)); }
    public List<Map<String, Object>> loadOutput(int jobOrderId) { return plain(repo.loadOutputByJobOrder(org(), comp(), jobOrderId)); }
    public List<Map<String, Object>> loadPackingMaterial(int jobOrderId) { return plain(repo.loadPackingMaterialByJobOrder(org(), comp(), jobOrderId)); }
    public List<Map<String, Object>> loadOverheads(int jobOrderId) { return plain(repo.loadOverheadsByJobOrder(org(), comp(), jobOrderId)); }

    public List<Map<String, Object>> pendingRateJobOrders() {
        return project(repo.pendingRateJobOrders(org(), comp(), year()), "Id", "PlanCode");
    }

    /** LoadOutPutPendingforRates.EntryType "Issue" (Input) / "ByProduct" (Output). */
    public List<Map<String, Object>> pendingForRates(String from, String to, int jobOrderId, String entryType) {
        return plain(repo.pendingForRates(org(), comp(), branch(), jobOrderId, dateOrNull(from), dateOrNull(to), entryType));
    }

    public List<Map<String, Object>> consumptionPendingForRates(String from, String to, int jobOrderId) {
        return plain(repo.consumptionPendingForRates(org(), comp(), branch(), jobOrderId, dateOrNull(from), dateOrNull(to)));
    }

    public List<Map<String, Object>> issuanceDropDowns() { return plain(repo.issuanceDropDowns(org(), comp(), String.valueOf(branch()))); }

    public List<Map<String, Object>> issuanceSearch(String from, String to, int parentCategoryId, int itemCategoryId, int itemTypeId,
                                                    int jobLotId, String cropYear, int warehouseId, int refDocumentTypeId,
                                                    int supplierCustomerId, int itemId) {
        return plain(repo.availableTransactionsForIssuance(org(), comp(), dateOrNull(from), dateOrNull(to), supplierCustomerId,
                warehouseId, itemId, refDocumentTypeId, jobLotId, parentCategoryId, itemCategoryId, itemTypeId, cropYear));
    }

    // ===================================================================== consumption entry

    /** ConsumptionDetailComboBind:9225 - rows split by ActivityType (Warehouse / Items / CropYear / JobLot / PackingType). */
    public List<Map<String, Object>> consumptionDetailCombos() {
        return project(repo.dropDownForConsumptionForm(org(), comp(), String.valueOf(branch())), "ActivityType", "Id", "name", "Code");
    }

    /** GetAvailableStockForConsumption:10493. */
    public Map<String, Object> stock(int warehouseId, int itemId, int jobLotId, String cropYear, String toDate) {
        List<Map<String, Object>> r = repo.weightCurrStockByItem(org(), comp(), itemId, dateTime(toDate), warehouseId, jobLotId, cropYear);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("availableQty", r.isEmpty() ? 0d : toDouble(col(r.get(0), "AvailableQty")));
        m.put("availableStock", r.isEmpty() ? 0d : toDouble(col(r.get(0), "AvailableStock")));
        return m;
    }

    /** CommonServices.AvgRateOnlyForCGS (grid Quantity/Weight edits :2589, Generate InPut Rates :2696, consumption :10579). */
    public double avgRateCgs(int itemId, String docDate, int documentTypeId, int recId, int jobLotId, int cropYearId,
                             String cropYear, int warehouseId) {
        return repo.avgRateOnlyForCgs(org(), comp(), itemId, dateTime(docDate), documentTypeId, recId, jobLotId, cropYearId, cropYear, warehouseId);
    }

    /** CommonServices.GetAvgRateFromFIFOMethod - the AvgRate of the first FIFO layer (0 when none). */
    public double fifoRate(int itemId, int packUomId, String docDate, int warehouseId, int jobLotId, int packingTypeId, String cropYear) {
        List<Map<String, Object>> r = repo.stockByFifo(org(), comp(), itemId, packUomId, dateTime(docDate), warehouseId, jobLotId, packingTypeId, cropYear);
        return r.isEmpty() ? 0d : toDouble(col(r.get(0), "AvgRate"));
    }

    /** CommonServices.GetUomScheduleByItemId - Id / UOMCode / Equivalent. */
    public List<Map<String, Object>> uomByItem(int itemId) {
        return project(repo.uomScheduleByItemId(org(), comp(), itemId), "Id", "UOMCode", "Equivalent");
    }

    // ============================================================================ read one

    /** InvFoodProduction.GetByID - header + invFoodProductionDetails + InvFoodProductionConsumptionDetailslist. */
    public Map<String, Object> production(int id) {
        Map<String, Object> h = repo.productionHeader(id);
        if (h == null) throw new FormMessage("Record Not Found");
        if (toInt(col(h, "OrganizationId")) != org() || toInt(col(h, "CompanyId")) != comp()) throw new FormMessage("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>(plainRow(h));
        out.put("details", plain(repo.productionDetails(id)));
        out.put("consumptionDetails", plain(repo.consumptionDetails(id)));
        out.put("voucherHeadId", repo.voucherHeadId(org(), comp(), toInt(col(h, "DocumentTypeId")), id));
        return out;
    }

    /** ReadByIdPackingMaterial:4841 - one PM row with its names. */
    public Map<String, Object> packingMaterial(int id) {
        Map<String, Object> r = repo.packingMaterialById(id);
        if (r == null) throw new FormMessage("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>(plainRow(r));
        out.put("voucherHeadId", repo.voucherHeadId(org(), comp(), DOC_PACKING_MATERIAL, id));
        return out;
    }

    /** ReadByIdOverHeadJobOrderWise:5589 / grdOverHeadHistory_SelectionChanged:6016. */
    public Map<String, Object> overheadsByJobOrder(int jobOrderId) {
        List<Map<String, Object>> rows = plain(repo.overheadsByJobOrder(jobOrderId));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        int lastId = rows.isEmpty() ? 0 : toInt(rows.get(rows.size() - 1).get("Id"));
        out.put("recOverHeadId", lastId);
        out.put("voucherHeadId", lastId > 0 ? repo.voucherHeadId(org(), comp(), DOC_OVERHEAD, lastId) : 0);
        return out;
    }

    /** CommonServices.VoucherHeadIdGet(Id, DocumentTypeId). */
    public int voucherHeadId(int id, int documentTypeId) { return repo.voucherHeadId(org(), comp(), documentTypeId, id); }

    // ============================================================================== histories

    /** InputGridHistory:3043 / OutputGridHistory:4208 / GrdHistoryConsumptionMain:10896 / GridHistory:6248 (RefDocumentTypeId 401). */
    public List<Map<String, Object>> formHistory(String from, String to, int docNoFrom, int docNoTo, int jobOrderId, int plantId, int documentTypeId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(org(), comp(), year(), branch(), dateOrNull(from), dateOrNull(to), docNoFrom, docNoTo,
                jobOrderId, plantId, DOC_PRE_COSTING_JOB_ORDER)) {
            if (documentTypeId != 0 && toInt(col(r, "DocumentTypeId")) != documentTypeId) continue;   /* the form filters the DataTable (:3089, :4256, :10949) */
            out.add(plainRow(r));
        }
        return out;
    }

    /** BindHistoryPMGrid:4883 (ActionId 1 = chkPendingForRates). */
    public List<Map<String, Object>> packingMaterialHistory(boolean pendingForRates) {
        return plain(repo.packingMaterialHistory(org(), comp(), year(), branch(), pendingForRates ? 1 : 0));
    }

    /** BindGridHistoryOverHead:5877 - grouped per job order (sum qty / sum amount / avg rate). */
    public List<Map<String, Object>> overheadHistory() {
        Map<Integer, Map<String, Object>> g = new LinkedHashMap<>();
        Map<Integer, double[]> acc = new LinkedHashMap<>();
        for (Map<String, Object> r : repo.overheadHistory(org(), comp(), year(), branch())) {
            int jo = toInt(col(r, "InvProductionJobOrderId"));
            Map<String, Object> m = g.get(jo);
            if (m == null) {
                m = new LinkedHashMap<>();
                m.put("InvProductionJobOrderId", jo);
                m.put("ProductionJobOrder", str(col(r, "PlanCode")));
                m.put("IsApproved", str(col(r, "JobOrderStatus")));
                m.put("JobPlanStatus", str(col(r, "JobPlanStatus")));
                g.put(jo, m);
                acc.put(jo, new double[]{0, 0, 0, 0});
            }
            double[] a = acc.get(jo);
            a[0] += toDouble(col(r, "ItemQty"));
            a[1] += toDouble(col(r, "Amount"));
            a[2] += toDouble(col(r, "ItemRate"));
            a[3] += 1;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map.Entry<Integer, Map<String, Object>> e : g.entrySet()) {
            double[] a = acc.get(e.getKey());
            Map<String, Object> m = e.getValue();
            m.put("TotalItemQty", a[0]);
            m.put("TotalExpense", a[1]);
            m.put("AverageItemRate", a[3] > 0 ? a[2] / a[3] : 0d);
            out.add(m);
        }
        return out;
    }

    // ================================================================================ summary

    /** btnShow_Click:6557 - GrdSummaryValuesFill + GrdSummaryReportFill + grdDocWiseSummeryFill. */
    public Map<String, Object> summary(int jobOrderId, int plantId, String from, String to, String branchIds) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("values", plain(repo.summaryValues(org(), comp(), jobOrderId, plantId, dateOrNull(from), dateOrNull(to), branchIds, null)));
        out.put("report", plain(repo.recoverySummaryReport(org(), comp(), jobOrderId, plantId, dateOrNull(from), dateOrNull(to), branchIds, 0)));
        out.put("docWise", plain(repo.docWiseSummaryReport(org(), comp(), jobOrderId, plantId, dateOrNull(from), dateOrNull(to), branchIds)));
        return out;
    }

    // ============================================================================= settlement

    /** btnGeneralSettlement_Click:7079 - the three InvFoodProductionSettlement activities and the summary values. */
    public Map<String, Object> settlementGenerate(int jobOrderId) {
        int org = org(), comp = comp();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> inOut = plain(repo.settlementData(org, comp, jobOrderId, "InPutOutPut"));
        out.put("inputOutput", inOut);
        out.put("overheads", plain(repo.settlementData(org, comp, jobOrderId, "OverHeads")));
        out.put("packingMaterial", plain(repo.settlementData(org, comp, jobOrderId, "PackingMaterial")));
        double netWeight = 0d;
        for (Map<String, Object> r : inOut) {
            if ("Output".equals(str(r.get("EntryType"))) && "FinishGoods".equals(str(r.get("EntryTypeDetail")))) netWeight += toDouble(r.get("Weight"));
        }
        out.put("summaryValues", plain(repo.summaryValues(org, comp, jobOrderId, 0, null, null, null, netWeight)));
        return out;
    }

    /** txtFGWeight_TextChanged:8636 - GetSummeryValues with NetWeight (the hidden FG panel of the Output tab). */
    public List<Map<String, Object>> summaryValuesFg(int jobOrderId, double fgWeight) {
        return plain(repo.summaryValues(org(), comp(), jobOrderId, 0, null, null, null, fgWeight));
    }

    /**
     * btnUpdateSettlement_Click:8338. FIFO: when a FinishGoods row with Status "NotUsed" exists -> UpdateSettlement then
     * SettlementFinancials, else SettlementFinancials only when the difference is non-zero. Non-FIFO: UpdateSettlement.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> settlementUpdate(Map<String, Object> body) {
        int org = org(), comp = comp();
        int jobOrderId = toInt(body.get("jobOrderId"));
        boolean fifo = repo.erpFeature(org, comp, ERP_FEATURE_FIFO_CGS);
        List<Map<String, Object>> rows = body.get("outputRows") instanceof List ? (List<Map<String, Object>>) body.get("outputRows") : new ArrayList<>();
        double difference = toDouble(body.get("differenceAmount"));
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> messages = new ArrayList<>();
        if (fifo) {
            boolean notUsed = false;
            for (Map<String, Object> r : rows) if ("FinishGoods".equals(str(r.get("EntryType"))) && "NotUsed".equals(str(r.get("Status")))) notUsed = true;
            if (notUsed) {
                updateSettlement(jobOrderId, rows);
                messages.add(settlementFinancials(body));
            } else if (difference != 0d) {
                messages.add(settlementFinancials(body));
            }
        } else {
            updateSettlement(jobOrderId, rows);
            out.put("reset", true);
        }
        out.put("messages", messages);
        return out;
    }

    /** UpdateSettlement:8139 - the grdOutPutSettlement rows with Quantity > 0 and Weight > 0. */
    private void updateSettlement(int jobOrderId, List<Map<String, Object>> rows) {
        List<LinkedHashMap<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!(toDouble(r.get("Quantity")) > 0d && toDouble(r.get("Weight")) > 0d)) continue;
            LinkedHashMap<String, Object> d = ProductionSettlementRepository.detailParams();
            d.put("@Id", toInt(r.get("DetailId")));
            d.put("@InvFoodProductionId", toInt(r.get("Id")));
            String entryType = str(r.get("EntryType"));
            d.put("@EntryType", entryType);
            d.put("@RefDocNoId", 0);
            if (str(r.get("WareHouseId")).isEmpty() || toInt(r.get("WareHouseId")) == 0) throw new FormMessage("WareHouse Field Require in Output Detail");
            d.put("@WarehouseId", toInt(r.get("WareHouseId")));
            if (str(r.get("ItemId")).isEmpty() || toInt(r.get("ItemId")) == 0) throw new FormMessage("Item Field Require in Output Detail");
            d.put("@ItemId", toInt(r.get("ItemId")));
            d.put("@ItemUomId", toInt(r.get("ItemUOMId")));
            d.put("@CropBatch", str(r.get("CropYear")));
            String lot = str(r.get("JobLotId"));
            if (lot.isEmpty() || "0".equals(lot)) throw new FormMessage("JobLot Field Require in Output Detail");
            d.put("@JobLotId", toInt(r.get("JobLotId")));
            d.put("@PackingtypeId", toInt(r.get("PackingTypeId")));
            d.put("@Qty", toDouble(r.get("Quantity")));
            double weight = toDouble(r.get("Weight"));
            d.put("@Weight", weight);
            if (weight == 0d) throw new FormMessage("Weight Field Required");
            double rate = toDouble(r.get("Rate"));
            d.put("@Rate", rate);
            if (rate == 0d) throw new FormMessage("Rate Field Required");
            double netRate = ("FinishGoods".equals(entryType) && "Used".equals(str(r.get("Status")))) ? rate : toDouble(r.get("NetRate"));
            d.put("@NetRate", netRate);
            if (netRate == 0d) throw new FormMessage("NetRate Field Required");
            d.put("@RateUOMId", toInt(r.get("RateUOMId")));
            if (toInt(r.get("RateUOMId")) == 0) throw new FormMessage("RateUom Field Required");
            d.put("@InvProductionJobOrderId", jobOrderId);
            double amount = toDouble(r.get("ItemAmount"));
            d.put("@Amount", amount);
            d.put("@ItemPmCost", toDouble(r.get("ItemPmCost")));
            d.put("@GeneralPmCost", toDouble(r.get("GeneralPmCost")));
            d.put("@ItemOhCost", toDouble(r.get("ItemOhCost")));
            d.put("@GeneralOhCost", toDouble(r.get("GeneralOhCost")));
            if (amount == 0d) throw new FormMessage("ItemAmount must be greater than zero Please Check");
            d.put("@TotalAmount", toDouble(r.get("TotalAmount")));
            d.put("@VoucherHeadId", toInt(r.get("WIPAccountId")));
            d.put("@Remarks", str(r.get("Remarks")));
            details.add(d);
        }
        saver.updateSettlement(org(), comp(), jobOrderId, details);
    }

    /** SettlementFinancials:8228 - the DocumentTypeId 142 voucher Dr/Cr difference account against WIP. */
    private String settlementFinancials(Map<String, Object> body) {
        int org = org(), comp = comp();
        int jobOrderId = toInt(body.get("jobOrderId"));
        int diffAccountId = toInt(body.get("differenceAccountId"));
        if (jobOrderId == 0) throw new FormMessage("JobOrder Settlement Field is Required");
        if (diffAccountId == 0) throw new FormMessage("Difference Account Field is Required");
        List<Map<String, Object>> gl = repo.glAccountsByJobOrderId(org, comp, jobOrderId);
        if (gl.isEmpty()) throw new FormMessage("WIP Account Not Found");
        int wip = toInt(col(gl.get(0), "WorkInProccessAcId"));
        if (wip == diffAccountId) throw new FormMessage("WIP Account and Difference Account are the same. Please verify.");
        double difference = toDouble(body.get("differenceAmount"));
        String jobOrderText = str(body.get("jobOrderText"));
        int voucherId = repo.voucherHeadId(org, comp, DOC_SETTLEMENT, jobOrderId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        LinkedHashMap<String, Object> head = ProductionSettlementRepository.voucherHeadDefaults();
        head.put("@Id", voucherId);
        head.put("@DocumentTypeSrNo", jobOrderId);
        head.put("@ManualBillNo", jobOrderText);
        head.put("@VoucherDate", Timestamp.valueOf(dateTime(str(body.get("settlementDate")))));
        head.put("@DocumentTypeId", DOC_SETTLEMENT);
        head.put("@RefAccountId", wip);
        head.put("@AgainstAccountId", diffAccountId);
        head.put("@EntryDate", now); head.put("@ModifyDate", now);
        head.put("@EntryUser", uid()); head.put("@ModifyUser", uid());
        head.put("@OrganizationId", org); head.put("@CompanyId", comp);
        head.put("@FinancialYearId", year()); head.put("@BranchId", branch());
        String remarks = "Finish Goods ActualAmount " + str(body.get("actualFinishGoodsText")) + " Finish Goods Amount against JobOrder "
                + jobOrderText.trim() + "  " + str(body.get("finishAmountAgainstJobOrderText")) + " Difference Amount " + str(body.get("differenceAmountText"));
        head.put("@VoucherAmount", Math.abs(difference));
        List<LinkedHashMap<String, Object>> lines = new ArrayList<>();
        LinkedHashMap<String, Object> vd = ProductionSettlementRepository.voucherDetailDefaults();
        vd.put("@AccountId", diffAccountId); vd.put("@AgainstAccountId", wip); vd.put("@Comments", remarks);
        if (difference > 0d) vd.put("@DebitAmount", difference); else vd.put("@CreditAmount", difference);
        vd.put("@OrderNo", jobOrderId); vd.put("@BranchesId", branch());
        lines.add(vd);
        LinkedHashMap<String, Object> vd2 = ProductionSettlementRepository.voucherDetailDefaults();
        vd2.put("@AccountId", wip); vd2.put("@AgainstAccountId", diffAccountId); vd2.put("@Comments", remarks);
        if (difference > 0d) vd2.put("@CreditAmount", difference); else vd2.put("@DebitAmount", difference);
        vd2.put("@OrderNo", jobOrderId); vd2.put("@BranchesId", branch());
        lines.add(vd2);
        int r = saver.settlementFinancials(head, lines);
        return r > 0 ? "Record Save Successfully" : "Record Update Successfully";
    }

    /** btnApproveSettlement_Click:8546. */
    public void approveSettlement(int jobOrderId) {
        if (jobOrderId == 0) throw new FormMessage("JobOrder Settlement Field is Required");
        saver.approveJobOrder(org(), comp(), jobOrderId, uid());
    }

    // ================================================================================= prints

    /** The "Record Not Found For DisPlay" check each print button makes before the viewer opens. */
    public int reportRowCount(String report, int jobOrderId, int actionId, int plantId, String from, String to, String branchIds) {
        int org = org(), comp = comp();
        LinkedHashMap<String, Object> m = Prod871ProductionAgainstPreCostingJobOrderRepository.p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        switch (report) {
            case "606-pm":
                m.put("@JobOrderId", jobOrderId); m.put("@ReportType", "PackingMaterial");
                return repo.reportRowCount("dbo.Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder", m);
            case "606-oh":
                m.put("@JobOrderId", jobOrderId); m.put("@ReportType", "OverHead");
                return repo.reportRowCount("dbo.Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder", m);
            case "613":
                m.put("@JobOrderId", jobOrderId); if (actionId != 0) m.put("@ActionId", actionId);
                return repo.reportRowCount("dbo.Sp_InvFoodProduction_Summery2_Rpt", m);
            case "613A":
                m.put("@JobOrderId", jobOrderId); if (actionId != 0) m.put("@ActionId", actionId);
                return repo.reportRowCount("dbo.usp_ProductionBeforSettlement_Summery2_Rpt", m);
            case "615":
                m.put("@JobOrderId", jobOrderId); if (actionId != 0) m.put("@ActionId", actionId);
                return repo.reportRowCount("dbo.Sp_InvFoodProduction_Summery3_Rpt", m);
            case "615A":
                m.put("@JobOrderId", jobOrderId); if (actionId != 0) m.put("@ActionId", actionId);
                return repo.reportRowCount("dbo.usp_ProductionBeforSettlement_Summery3_Rpt", m);
            case "602A":
                return repo.recoverySummaryReport(org, comp, jobOrderId, 0, null, null, null, actionId).size();
            case "607":
                m.put("@InvJobOrderId", jobOrderId);
                if (dateOrNull(from) != null) m.put("@FromDate", dateOrNull(from));
                if (dateOrNull(to) != null) m.put("@ToDate", dateOrNull(to));
                if (branchIds != null && !branchIds.isEmpty()) m.put("@BranchesIds", branchIds);
                return repo.reportRowCount("dbo.Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt", m);
            default:
                throw new FormMessage("Unknown report " + report);
        }
    }

    // ================================================================================== saves

    /** InsertInput:2257 (Save: RecId 0 / Update: RecId = header id). */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveInput(Map<String, Object> body) {
        Map<String, Boolean> rights = rights();
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));
        int recId = toInt(body.get("id"));
        requireSaveRight(rights, recId);
        /* FormValidation:2222 */
        String docNo = str(body.get("docNumber")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new FormMessage("document Number Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        Map<String, Object> job = requireJobOrder(jobOrderId, "Job Order Number Field Required");
        if (toInt(body.get("plantId")) == 0) throw new FormMessage("Plant Name Field Required");
        if (toInt(body.get("wipAccountId")) == 0) throw new FormMessage("WorkInProcess Field Required");
        if (toInt(body.get("wipItemId")) == 0) throw new FormMessage("WIP Item Field Required");
        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw new FormMessage("Enter Detail First ...");

        int org = org(), comp = comp();
        LocalDateTime now = LocalDateTime.now().withNano(0);
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocumentTypeId", DOC_INPUT);
        h.put("DocCode", recId > 0 ? toInt(docNo) : serial(DOC_INPUT));
        h.put("DocDate", dateTime(str(body.get("docDate"))));
        h.put("MainRemarks", str(body.get("remarks")));
        h.put("EntryType", "Input");
        h.put("InvFoodProductionPlanId", 0);
        h.put("WIPAccountId", toInt(body.get("wipAccountId")));
        h.put("WIPItemId", toInt(body.get("wipItemId")));
        h.put("EBDepartmentId", toInt(body.get("departmentId")));
        h.put("InvJobOrderId", jobOrderId);
        h.put("InvJobOrderNo", str(col(job, "PlanCode")).trim());
        h.put("BaseDocumentTypeId", 1);                                  /* :2293 - always 1 on this form */
        h.put("PlantId", toInt(body.get("plantId")));
        h.put("OrganizationId", org); h.put("CompanyId", comp); h.put("BranchId", branch());
        h.put("EntryDate", now); h.put("ModifyDate", now);
        h.put("EntryUser", uid()); h.put("ModifyUser", uid());
        h.put("FinancialYearId", year());
        h.put("IsApproved", false); h.put("ProjectId", 0); h.put("ContractScheduleId", 0);
        h.put("WipWareHouseId", 0); h.put("StockHoldForLabApproval", false);

        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0d;
        int i = 0;
        for (Map<String, Object> r : rows) {
            i++;
            String wh = str(r.get("WareHouseId")).trim();
            if (wh.isEmpty() || "0".equals(wh)) throw new FormMessage("WareHouse Field Required in Input Detail Row No : " + i);
            String item = str(r.get("ItemId")).trim();
            if (item.isEmpty() || "0".equals(item)) throw new FormMessage("Item Field Required in Input Detail Row No : " + i);
            String crop = str(r.get("CropYear")).trim();
            if (crop.isEmpty() || "0".equals(crop)) throw new FormMessage("CropYear Field Required in Input Detail Row No : " + i);
            if (str(r.get("JobLotId")).trim().isEmpty()) throw new FormMessage("JobLot Field Required in Input Detail Row No : " + i);
            if (toDouble(r.get("Quantity")) <= 0d) throw new FormMessage("ItemQty Field Required in Input Detail Row No : " + i);
            if (toDouble(r.get("Weight")) <= 0d) throw new FormMessage("ItemWeight Field Required in Input Detail Row No : " + i);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("EntryType", "Issue");
            d.put("JobOrderInPutId", toInt(r.get("JobOrderInPutId")));
            d.put("InvProductionJobOrderId", jobOrderId);
            d.put("RefDocNoId", toInt(r.get("RefDocIdNo")));
            d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            d.put("WarehouseId", toInt(wh));
            d.put("ItemId", toInt(item));
            d.put("ItemUomId", toInt(r.get("ItemUOMId")));
            d.put("CropBatch", crop);
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
            d.put("PackUnit", toInt(r.get("ItemUOM")));                 /* :2357 Conversion.ToInt of the pack UOM text */
            double qty = toDouble(r.get("Quantity")), weight = toDouble(r.get("Weight"));
            d.put("Qty", qty); d.put("Weight", weight);
            grossWeight += weight;
            double rate = toDouble(r.get("Rate"));
            if (!rateRight && toInt(d.get("RefDocumentTypeId")) == 0 && toInt(d.get("RefDocNoId")) == 0) {
                d.put("Rate", 0d); d.put("NetRate", 0d); d.put("Amount", 0d); d.put("TotalAmount", 0d);
            } else {
                d.put("Rate", rate); d.put("NetRate", rate);
                double amount = (double) halfEven(toDouble(r.get("ItemAmount")));   /* :2374 Conversion.ToInt(ItemAmount) */
                d.put("Amount", amount); d.put("TotalAmount", amount);
            }
            d.put("RateUOMId", toInt(r.get("RateUOMId")));
            double rateEq = toDouble(r.get("RateUOMEquivalent"));
            if (rateEq != 0d) {
                List<Map<String, Object>> uoms = repo.uomScheduleByItemId(org, comp, toInt(item));
                if (!uoms.isEmpty()) {
                    int want = halfEven(rateEq);
                    Map<String, Object> hit = null;
                    for (Map<String, Object> x : uoms) if (toDouble(col(x, "Equivalent")) == want) { hit = x; break; }
                    if (hit == null) throw new FormMessage("RateUOM On Row No " + i + " is not define please check");
                    d.put("RateUOMId", toInt(col(hit, "Id")));
                }
            }
            d.put("VoucherHeadId", 0);
            d.put("Remarks", str(r.get("Remarks")));
            d.put("MoveOrderDocumentTypeId", 0); d.put("MoveOrderDocId", 0);
            d.put("EbUnit", 0d); d.put("EbTotal", 0d); d.put("GrossWeight", 0d);
            d.put("ItemPmCost", 0d); d.put("GeneralPmCost", 0d); d.put("ItemOhCost", 0d); d.put("GeneralOhCost", 0d);
            d.put("StockAcId", 0); d.put("OutPutIdFromJobOrder", 0); d.put("DeleteFlag", 0); d.put("JobOrderScheduleId", 0);
            details.add(d);
        }
        if (details.isEmpty()) throw new FormMessage("Grid Record Not Found");
        String removeIds = str(body.get("removeIds"));
        int id = saver.saveInputOrConsumption(h, details, removeIds);
        return afterSave(recId, toInt(h.get("DocCode")), id, DOC_INPUT, grossWeight, "wagesActiveForInput");
    }

    /** OutPutInsert:3825. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveOutput(Map<String, Object> body) {
        Map<String, Boolean> rights = rights();
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));
        int recId = toInt(body.get("id"));
        requireSaveRight(rights, recId);
        boolean fifo = repo.erpFeature(org(), comp(), ERP_FEATURE_FIFO_CGS);
        /* FormValidationOutPut:3273 */
        String docNo = str(body.get("docNumber")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new FormMessage("DocNo Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        Map<String, Object> job = requireJobOrder(jobOrderId, "Job Order Number Field Required");
        if (toInt(body.get("plantId")) == 0) throw new FormMessage("Plant Name Field Is Required");
        if (toInt(body.get("wipAccountId")) == 0) throw new FormMessage("WorkInProcess Field Required");
        if (toInt(body.get("wipItemId")) == 0) throw new FormMessage("WIP Item Field Required");
        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw new FormMessage("Enter Detail First ...");

        LocalDateTime now = LocalDateTime.now().withNano(0);
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("@Id", recId);
        h.put("@DocDate", dateTime(str(body.get("docDate"))));
        h.put("@DocCode", recId > 0 ? toInt(docNo) : serial(DOC_OUTPUT));
        h.put("@DocumentTypeId", DOC_OUTPUT);
        h.put("@WIPAccountId", toInt(body.get("wipAccountId")));
        h.put("@WIPItemId", toInt(body.get("wipItemId")));
        h.put("@MainRemarks", str(body.get("remarks")));
        h.put("@IsApproved", false);
        h.put("@EntryDate", now); h.put("@EntryUser", uid());
        h.put("@ModifyDate", now); h.put("@ModifyUser", uid());
        h.put("@OrganizationId", org()); h.put("@CompanyId", comp());
        h.put("@InvFoodProductionPlanId", 0);
        h.put("@InvJobOrderId", jobOrderId);
        h.put("@InvJobOrderNo", str(col(job, "PlanCode")).trim());
        h.put("@EntryType", "Output");
        h.put("@BranchId", branch()); h.put("@ProjectId", 0); h.put("@ActionId", 0);
        h.put("@FinancialYearId", year());
        h.put("@PlantId", toInt(body.get("plantId")));
        h.put("@EBDepartmentId", 0); h.put("@ContractScheduleId", 0); h.put("@WipWareHouseId", 0);
        h.put("@BaseDocumentTypeId", 1);                                 /* :3860 */
        h.put("@StockHoldForLabApproval", false);

        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0d;
        int i = 0;
        for (Map<String, Object> r : rows) {
            i++;
            if (str(r.get("EntryType")).trim().isEmpty()) throw new FormMessage("EntryType Field Required in Output Detail Row No : " + i);
            if (toInt(r.get("WareHouseId")) == 0) throw new FormMessage("WareHouse Field Required in Output Detail Row No : " + i);
            if (toInt(r.get("ItemId")) == 0) throw new FormMessage("Item Field Required in Output Detail Row No : " + i);
            String lot = str(r.get("JobLotId")).trim();
            if (lot.isEmpty() || "0".equals(lot)) throw new FormMessage("JobLot Field Required in Output Detail Row No : " + i);
            if (toDouble(r.get("Quantity")) <= 0d) throw new FormMessage("Quantity Field Required in Output Detail Row No : " + i);
            if (toDouble(r.get("Weight")) <= 0d) throw new FormMessage("Weight Field Required in Output Detail Row No : " + i);
            double rate = toDouble(r.get("Rate"));
            if (rateRight && fifo && rate == 0d) throw new FormMessage("Rate Required in Output Detail Row No : " + i);
            double rateEq = toDouble(r.get("RateEquivalent"));
            if (rateRight && rateEq == 0d) throw new FormMessage("Rate Equivalent Required in Output Detail Row No : " + i);
            double weight = toDouble(r.get("Weight"));
            double amount = weight / rateEq * rate;            /* :3940 - Infinity/NaN when RateEquivalent is 0 without the Rate right */
            if (rateRight && fifo && amount == 0d) throw new FormMessage("Amount Required in Output Detail Row No : " + i);
            if (Double.isNaN(amount) || Double.isInfinite(amount)) amount = 0d;
            grossWeight += weight;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("@Id", toInt(r.get("Id")));
            d.put("@InvFoodProductionId", 0);
            d.put("@RefDocumentTypeId", 0); d.put("@RefDocNoId", 0);
            d.put("@InvProductionJobOrderId", jobOrderId); d.put("@InvProductionJobOrderNo", 0);
            d.put("@EntryType", str(r.get("EntryType")).trim());
            d.put("@WarehouseId", toInt(r.get("WareHouseId")));
            d.put("@ItemId", toInt(r.get("ItemId")));
            d.put("@ItemUomId", toInt(r.get("ItemUOMId")));
            d.put("@CropBatch", str(r.get("CropYear")));
            d.put("@JobLotId", toInt(r.get("JobLotId")));
            d.put("@PackingtypeId", toInt(r.get("PackingTypeId")));
            d.put("@Qty", toDouble(r.get("Quantity")));
            d.put("@PackUnit", 0);
            d.put("@Weight", weight);
            d.put("@Rate", rate);
            d.put("@RateUOMId", toInt(r.get("RateUOMId")));
            d.put("@Amount", amount);
            d.put("@ItemPmCost", 0d); d.put("@GeneralPmCost", 0d); d.put("@ItemOhCost", 0d); d.put("@GeneralOhCost", 0d);
            d.put("@NetRate", rate); d.put("@TotalAmount", amount);
            d.put("@Remarks", str(r.get("Remarks")));
            d.put("@VoucherHeadId", 0);
            d.put("@StockAcId", toInt(r.get("StockAcId")));
            d.put("@RefDocSubIdNo", 0);
            d.put("@JobOrderInPutId", 0);
            d.put("@OutPutIdFromJobOrder", toInt(r.get("OutPutIdFromJobOrder")));
            d.put("@DeleteFlag", 0); d.put("@LineId", 0); d.put("@JobOrderScheduleId", 0);
            d.put("@GrossWeight", toDouble(r.get("GrossWeight")));
            d.put("@EbUnit", toDouble(r.get("EbUnit"))); d.put("@EbTotal", toDouble(r.get("EbTotal")));
            d.put("@MoveOrderDocumentTypeId", 0); d.put("@MoveOrderDocId", 0);
            details.add(d);
        }
        if (details.isEmpty()) throw new FormMessage("grid Record not found please check!");
        int id = saver.saveOutput(h, details, str(body.get("removeIds")));
        return afterSave(recId, toInt(h.get("@DocCode")), id, DOC_OUTPUT, grossWeight, "wagesActiveForOutput");
    }

    /** InsertConsumption:10116. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveConsumption(Map<String, Object> body) {
        Map<String, Boolean> rights = rights();
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));
        int recId = toInt(body.get("id"));
        requireSaveRight(rights, recId);
        /* FormValidationConsumption:9562 */
        String docNo = str(body.get("docNumber")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new FormMessage("document Number Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        Map<String, Object> job = requireJobOrder(jobOrderId, "Job Order Number Field Required");
        if (toInt(body.get("plantId")) == 0) throw new FormMessage("Plant Name Field Required");
        if (toInt(body.get("wipAccountId")) == 0) throw new FormMessage("WorkInProcess Ac Field Required");
        if (toInt(body.get("wipItemId")) == 0) throw new FormMessage("WIP Item Field Required");
        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw new FormMessage("Enter Detail First ...");

        int org = org(), comp = comp();
        LocalDateTime now = LocalDateTime.now().withNano(0);
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocumentTypeId", DOC_CONSUMPTION);
        h.put("DocCode", recId > 0 ? toInt(docNo) : serial(DOC_CONSUMPTION));
        h.put("DocDate", dateTime(str(body.get("docDate"))));
        h.put("MainRemarks", str(body.get("remarks")));
        h.put("EntryType", "Consumption");
        h.put("InvFoodProductionPlanId", toInt(body.get("plantId")));      /* :10146 - the plant id, as written */
        h.put("WIPAccountId", toInt(body.get("wipAccountId")));
        h.put("WIPItemId", toInt(body.get("wipItemId")));
        h.put("EBDepartmentId", toInt(body.get("departmentId")));
        h.put("InvJobOrderId", jobOrderId);
        h.put("InvJobOrderNo", str(col(job, "PlanCode")).trim());
        h.put("BaseDocumentTypeId", 1);
        h.put("PlantId", toInt(body.get("plantId")));
        h.put("OrganizationId", org); h.put("CompanyId", comp); h.put("BranchId", branch());
        h.put("EntryDate", now); h.put("ModifyDate", now);
        h.put("EntryUser", uid()); h.put("ModifyUser", uid());
        h.put("FinancialYearId", year());
        h.put("IsApproved", false); h.put("ProjectId", 0); h.put("ContractScheduleId", 0);
        h.put("WipWareHouseId", 0); h.put("StockHoldForLabApproval", false);

        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0d;
        int i = 0;
        for (Map<String, Object> r : rows) {
            i++;
            /* :10174 - Conversion.ToDouble of the "#,##0.##" cell TEXT */
            if (!(round2(toDouble(r.get("Quantity"))) > 0d) || !(round2(toDouble(r.get("Weight"))) > 0d)) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("EntryType", "Consumption");
            d.put("Id", toInt(r.get("Id")));
            d.put("RefDocNoId", toInt(r.get("RefDocIdNo")));
            d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            String wh = str(r.get("WareHouseId")).trim();
            if (wh.isEmpty() || "0".equals(wh)) throw new FormMessage("WareHouse Field Require in Consumption Detail");
            d.put("WarehouseId", toInt(wh));
            String item = str(r.get("ItemId")).trim();
            if (item.isEmpty() || "0".equals(item)) throw new FormMessage("Item Field Require in Consumption Detail");
            d.put("ItemId", toInt(item));
            d.put("ItemUomId", toInt(r.get("ItemUOMId")));
            String crop = str(r.get("CropYear")).trim();
            if (crop.isEmpty() || "0".equals(crop)) throw new FormMessage("CropYear Field Require in Consumption Detail");
            d.put("CropBatch", crop);
            if (str(r.get("JobLotId")).trim().isEmpty()) throw new FormMessage("JobLot Field Require in Consumption Detail");
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
            double qty = toDouble(r.get("Quantity"));
            d.put("Qty", qty);
            if (qty == 0d) throw new FormMessage("ItemQty Field Required");
            double weight = toDouble(r.get("Weight"));
            d.put("Weight", weight);
            grossWeight += weight;
            if (weight == 0d) throw new FormMessage("ItemWeight Field Required");
            d.put("InvProductionJobOrderId", jobOrderId);
            if (!rateRight && toInt(d.get("RefDocumentTypeId")) == 0 && toInt(d.get("RefDocNoId")) == 0) {
                d.put("Rate", 0d); d.put("Amount", 0d);
            } else {
                d.put("Rate", toDouble(r.get("Rate")));
                d.put("Amount", (double) halfEven(toDouble(r.get("ItemAmount"))));
            }
            d.put("RateUOMId", toInt(r.get("RateUOMId")));
            double rateUom = toDouble(r.get("RateUOM"));
            if (rateUom != 0d) {
                List<Map<String, Object>> uoms = repo.uomScheduleByItemId(org, comp, toInt(item));
                if (!uoms.isEmpty()) {
                    int want = halfEven(rateUom);
                    Map<String, Object> hit = null;
                    for (Map<String, Object> x : uoms) if (toDouble(col(x, "Equivalent")) == want) { hit = x; break; }
                    if (hit == null) throw new FormMessage("RateUOM On Row No " + i + " is not define please check");
                    d.put("RateUOMId", toInt(col(hit, "Id")));
                }
            }
            d.put("Remarks", str(r.get("Remarks")));
            details.add(d);
        }
        if (details.isEmpty()) throw new FormMessage("Grid Record Not Found");
        int id = saver.saveInputOrConsumption(h, details, str(body.get("removeIds")));
        return afterSave(recId, toInt(h.get("DocCode")), id, DOC_CONSUMPTION, grossWeight, "wagesActiveForConsumption");
    }

    /** PackingMaterialInsert:4702. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> savePackingMaterial(Map<String, Object> body) {
        Map<String, Boolean> rights = rights();
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));
        int recId = toInt(body.get("id"));
        requireSaveRight(rights, recId);
        /* FormValidationOfPackingMaterial:4520 */
        if (str(body.get("docNumber")).trim().isEmpty()) throw new FormMessage("Doc No. Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        requireJobOrder(jobOrderId, "Job Order No. Field Required");
        int plantId = toInt(body.get("plantId"));
        if (plantId == 0) throw new FormMessage("Plant Name Field Required");
        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        LocalDateTime now = LocalDateTime.now().withNano(0);
        LocalDateTime docDate = dateTime(str(body.get("docDate")));
        int docNo = recId > 0 ? toInt(body.get("docNumber")) : packingMaterialCode();
        List<Map<String, Object>> list = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            i++;
            if (toInt(r.get("PackingItemId")) == 0) throw new FormMessage("PM Item Field Required in Grid Row No: " + i);
            int brand = toInt(r.get("BrandId")), brandUom = toInt(r.get("BrandUomId"));
            if (brand == 0 && brandUom != 0) throw new FormMessage("OutPut Item Field Required in Grid Row No: " + i);
            if (brandUom == 0 && brand != 0) throw new FormMessage("OutPut UomField Required in Grid Row No: " + i);
            double qty = toDouble(r.get("ItemQty")), rate = toDouble(r.get("ItemRate")), amount = toDouble(r.get("Amount"));
            if (qty <= 0d && (str(r.get("ItemRate")).trim().length() > 0 || rate != 0d)) throw new FormMessage("ItemQty Required in Grid Row No: " + i);
            if (rate == 0d && (str(r.get("ItemQty")).trim().length() > 0 || qty != 0d) && rateRight) throw new FormMessage("ItemRate Required in Grid Row No: " + i);
            if (amount <= 0d && rateRight) throw new FormMessage("Amount Required in Grid Row No: " + i);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("DocDate", docDate);
            m.put("DocNo", recId > 0 ? toInt(r.get("DocNo")) : docNo);
            m.put("InvProductionJobOrderId", jobOrderId);
            m.put("ItemId", toInt(r.get("PackingItemId")));
            m.put("itemUomSchId", 0);
            m.put("BrandId", brand); m.put("BrandUomId", brandUom);
            m.put("Qty", qty); m.put("Rate", rate); m.put("Amount", amount);
            m.put("pmRemarks", str(r.get("Remarks")));
            m.put("DocumentTypeId", DOC_PACKING_MATERIAL);
            m.put("OrganizationId", org()); m.put("CompanyId", comp());
            m.put("EntryUser", uid()); m.put("EntryDate", now);
            m.put("ModifyUser", uid()); m.put("ModifyDate", now);
            m.put("FinancialYearId", year());
            m.put("WarehouseId", toInt(r.get("WarehouseId")));
            m.put("PlantId", plantId);
            m.put("BranchesId", branch());
            m.put("JobOrderPackingMaterialId", toInt(r.get("JobOrderPackingMaterialId")));
            m.put("ItemConditionId", 0); m.put("ContractScheduleId", 0); m.put("ExImInvoiceId", 0);
            m.put("BaseDocumentTypeId", 1);                                 /* :4779 */
            m.put("RackId", 0);
            list.add(m);
        }
        if (list.isEmpty()) throw new FormMessage("Grid Record not found please check!");
        saver.savePackingMaterial(list, str(body.get("removeIds")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        return out;
    }

    /** OverHeadInsert:5436. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveOverheads(Map<String, Object> body) {
        Map<String, Boolean> rights = rights();
        int recId = toInt(body.get("id"));
        requireSaveRight(rights, recId);
        /* FormValidationOfOverHead:6089 */
        if (str(body.get("docNumber")).trim().isEmpty()) throw new FormMessage("Doc No. Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        requireJobOrder(jobOrderId, "Job Order No. Field Required");
        int plantId = toInt(body.get("plantId"));
        if (plantId == 0) throw new FormMessage("Plant Field Required");
        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        LocalDateTime now = LocalDateTime.now().withNano(0);
        LocalDateTime docDate = dateTime(str(body.get("docDate")));
        int docNo = recId > 0 ? toInt(body.get("docNumber")) : overheadCode();
        List<Map<String, Object>> list = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            i++;
            double qty = toDouble(r.get("ItemQty")), rate = toDouble(r.get("ItemRate")), amount = toDouble(r.get("Amount"));
            if (!(halfEven(qty) > 0 || rate > 0d || amount > 0d)) continue;          /* :5472 */
            int charges = toInt(r.get("ChargesTypeId")), coa = toInt(r.get("ChartOfAccountId"));
            int brand = toInt(r.get("BrandId")), brandUom = toInt(r.get("BrandUomId")), rateUom = toInt(r.get("RateUomId"));
            /* :5487-5522 - the desktop texts are NOT interpolated ("{r.RowIndex + 1}" is literal); reproduced verbatim */
            if (charges == 0) throw new FormMessage("ChargesType Field Required in Overhead Detail Row No : {r.RowIndex + 1}");
            if (coa == 0) throw new FormMessage("OverHead Account Field Required in Overhead Detail Row No : {r.RowIndex + 1}");
            if (brand == 0 && brandUom != 0) throw new FormMessage("OutPut Item Field Required in Overhead Detail Row No : {r.RowIndex + 1}");
            if (brandUom == 0 && brand != 0) throw new FormMessage("OutPut UomField Require in Overhead Detail Row No : {r.RowIndex + 1}");
            if (qty == 0d && rate != 0d) throw new FormMessage("ItemQty Required in Overhead Detail Row No : {r.RowIndex + 1}");
            if (rate == 0d && qty != 0d) throw new FormMessage("ItemRate Required in Overhead Detail Row No : {r.RowIndex + 1}");
            if (rateUom == 0 && brand == 0 && brandUom == 0 && charges == 1) throw new FormMessage("RateUom Required in Overhead Detail Row No : " + i);
            if (amount == 0d) throw new FormMessage("Amount Required in Overhead Detail Row No : {r.RowIndex + 1}");
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("DocDate", docDate);
            m.put("DocNo", recId > 0 && toInt(r.get("DocNo")) > 0 ? toInt(r.get("DocNo")) : docNo);
            m.put("InvProductionJobOrderId", jobOrderId);
            m.put("CharOfAccountId", coa);
            m.put("BrandId", brand); m.put("BrandUomId", brandUom);
            m.put("Amount", amount);
            m.put("ohRemarks", str(r.get("Remarks")));
            m.put("DocumentTypeId", DOC_OVERHEAD);
            m.put("ItemQty", qty); m.put("ItemRate", rate);
            m.put("OrganizationId", org()); m.put("CompanyId", comp());
            m.put("EntryUser", uid()); m.put("EntryDate", now);
            m.put("ModifyUser", uid()); m.put("ModifyDate", now);
            m.put("FinancialYearId", year());
            m.put("PlantId", plantId);
            m.put("BranchesId", branch());
            m.put("ChargesTypeId", charges);
            m.put("RateUomId", rateUom);
            m.put("JobOrderOverHeadId", toInt(r.get("JobOrderOverHeadId")));
            m.put("DetailDocumentTypeId", 0);
            m.put("BaseDocumentTypeId", 1);                                 /* :5527 */
            list.add(m);
        }
        if (list.isEmpty()) throw new FormMessage("Grid Record not found please check!");
        saver.saveOverheads(list, str(body.get("removeIds")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        return out;
    }

    /** The success message, then WagesStatus && WagesActive -> WagesDeleteByReferenceIds + the frmwagesBillHeader dialog (:2409, :3966, :10264). */
    private Map<String, Object> afterSave(int recId, int docCode, int id, int documentTypeId, double grossWeight, String wagesFlag) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + docCode);
        boolean compulsory = false, active = false;
        try {
            for (Map<String, Object> r : repo.multipleConfigurations(org(), comp(), "WagesCompulsoryOnProduction")) {
                if ("WagesCompulsoryOnProduction".equals(str(col(r, "ConfigDescription")).trim())) compulsory = bool(col(r, "ConfigKey"));
            }
            for (Map<String, Object> r : repo.wagesRefDocumentStatuses()) {
                if (toInt(col(r, "RefDocumentTypeId")) == documentTypeId) active = bool(col(r, "IsActive"));
            }
        } catch (RuntimeException ignored) { }
        boolean wages = compulsory && active;
        out.put("openWages", wages);
        if (wages) {
            repo.wagesDeleteByReferenceIds(org(), comp(), documentTypeId, id);
            out.put("wagesRefDocTypeId", documentTypeId);
            out.put("wagesRefDocId", id);
            out.put("grossWeightTotal", grossWeight);
        }
        return out;
    }

    private void requireSaveRight(Map<String, Boolean> rights, int recId) {
        if (recId > 0 ? !Boolean.TRUE.equals(rights.get("update")) : !Boolean.TRUE.equals(rights.get("save"))) {
            throw new FormMessage(recId > 0 ? "You do not have the Update right on this screen." : "You do not have the Save right on this screen.");
        }
    }

    private Map<String, Object> requireJobOrder(int jobOrderId, String message) {
        if (jobOrderId == 0) throw new FormMessage(message);
        for (Map<String, Object> r : repo.jobOrdersForProduction(org(), comp(), branch())) {
            if (toInt(col(r, "Id")) == jobOrderId) return r;
        }
        throw new FormMessage(message);
    }

    // ================================================================================= helpers

    private static Map<String, Object> pair(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k1, v1); m.put(k2, v2);
        return m;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, plainValue(col(r, c)));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(plainRow(r));
        return out;
    }

    private static Map<String, Object> plainRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : r.entrySet()) m.put(e.getKey(), plainValue(e.getValue()));
        return m;
    }

    /** Dates leave as local text (a Timestamp through Jackson is UTC and moves a midnight date east of Greenwich). */
    private static Object plainValue(Object o) {
        if (o instanceof java.sql.Timestamp) return ((java.sql.Timestamp) o).toLocalDateTime().withNano(0).toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) o).getTime()).toLocalDateTime().withNano(0).toString();
        if (o instanceof BigDecimal) return ((BigDecimal) o).doubleValue();
        return o;
    }

    private static String day(Object o) {
        Object p = plainValue(o);
        if (p == null) return null;
        String s = String.valueOf(p);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** A DateTimePicker value: "yyyy-MM-dd" (takes the current time of day, as an untouched picker does) or an ISO date-time. */
    static LocalDateTime dateTime(String s) {
        LocalDateTime v = dateOrNull(s);
        return v == null ? LocalDateTime.now().withNano(0) : v;
    }

    static LocalDateTime dateOrNull(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        String t = s.trim();
        try {
            if (t.length() <= 10) return LocalDate.parse(t).atTime(LocalTime.now().withNano(0));
            return LocalDateTime.parse(t.length() == 16 ? t + ":00" : t.substring(0, Math.min(19, t.length())));
        } catch (RuntimeException e) {
            throw new FormMessage("Invalid date: " + s);
        }
    }

    /** Conversion.ToInt of a double / decimal: Convert.ToInt32, half to even. */
    private static int halfEven(double v) {
        return new BigDecimal(v).setScale(0, RoundingMode.HALF_EVEN).intValue();
    }

    private static double round2(double v) {
        return new BigDecimal(v).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
    }
}
