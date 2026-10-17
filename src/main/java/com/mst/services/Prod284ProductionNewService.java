package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionInputRepository;
import com.mst.repositories.ProductionOutputRepository;
import com.mst.repositories.ProductionOverheadRepository;
import com.mst.repositories.ProductionPackingMaterialRepository;
import com.mst.repositories.ProductionSettlementRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionInputRepository.ci;
import static com.mst.repositories.ProductionInputRepository.toDouble;
import static com.mst.repositories.ProductionInputRepository.toInt;

/**
 * Screen 284 "ProductionNew Against (Pre Costing Job Order)" -
 * Architecture.WinApp.Production/FoodProductionWithValuesNew.cs (base.Name FoodProductionWithValuesNew).
 *
 * The READ side: everything frmFoodProduction_Load:893 binds, the per-tab reads (ReadByIdInput:1881,
 * ReadByIdOutPut:2791, ReadByIdPackingMaterial:3677, ReadByIdOverHeadJobOrderWise:4432), the
 * histories (InputGridHistory:2234, OutputGridHistory:3073, BindHistoryPMGrid:3937,
 * BindGridHistoryOverHead:4736, GridHistory:5118), the four "Load ... From Job Order" dialogs,
 * LoadOutPutPendingforRates, the Summery tab (:5353-5611), the Settlement reads (:5768-6017) and the
 * report data sources. Writes are in {@link Prod284ProductionNewPostingService}.
 *
 * Every procedure is the one the desktop BLL/DAL calls; the parameter guards follow the BLL as the
 * sibling screen 280 port established them (ProductionInput/Output/Overhead/PackingMaterial/Settlement
 * repositories, which are reused here where the BLL method is the same one).
 */
@Service
public class Prod284ProductionNewService {

    public static final int SCREEN_ID = 284;
    /** base.Name - the rights row key (SetRightsValueInRightsObject(base.Name), :920). */
    public static final String SCREEN_NAME = "FoodProductionWithValuesNew";

    public static final int DOC_INPUT = 80;        // InsertInput:1716
    public static final int DOC_OUTPUT = 112;      // OutPutInsert:2924
    public static final int DOC_PM = 111;          // PackingMaterialInsert:3576
    public static final int DOC_OH = 110;          // OverHeadInsert:4320
    public static final int DOC_SETTLEMENT = 142;  // SettlementFinancials:6614
    /** JobOrderNoFill:1194 - the Pre Costing job orders (DocumentTypeId 401). */
    public static final int JOB_ORDER_DOC_TYPE = 401;

    @Autowired private ProductionInputRepository inRepo;
    @Autowired private ProductionOutputRepository outRepo;
    @Autowired private ProductionOverheadRepository ohRepo;
    @Autowired private ProductionPackingMaterialRepository pmRepo;
    @Autowired private ProductionSettlementRepository stRepo;
    @Autowired private CurrentUserContext ctx;

    UserAccount user() { return ctx.requireAccountingUser(); }
    int org() { return ctx.currentOrganizationId(); }
    int comp() { return ctx.currentCompanyId(); }
    int branch() { return ctx.currentBranchId(); }
    int fy() { return ctx.currentFinancialYearId(); }

    /** A validation the desktop answers with MessageBox.Show(...) and stops. */
    public static class Prod284Message extends RuntimeException {
        public Prod284Message(String m) { super(m); }
    }

    // ================================================================================= rights

    /**
     * CommonServices.SetRightsValueInRightsObject("FoodProductionWithValuesNew") (:920) - the same
     * rule set the 280 shell reads (CommonServices.cs:14738): the role spelled exactly "Admin" starts
     * with Save/Update/Delete/Print; grant rows add Save/Update/Print, a "Delete" row overrides;
     * "Rate" -> DoHaveCanRateandAmount (:939) and "ProductionSettlement" -> DoHaveCanSettlementtab
     * (:945) come ONLY from grant rows. The grid-bar rights (:929-938) ride along for the gear menus.
     */
    public Map<String, Object> rights() {
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("save", admin);
        r.put("update", admin);
        r.put("delete", admin);
        r.put("print", admin);
        r.put("rateAndAmount", false);
        r.put("settlementTab", false);
        r.put("saveLayout", admin);
        r.put("gridPrint", admin);
        r.put("gridExport", admin);
        try {
            for (Map<String, Object> row : inRepo.userRights(ctx.currentUserId(), SCREEN_NAME, role, comp())) {
                String name = str(ci(row, "RightName"));
                boolean v = asBool(ci(row, "Value"));
                switch (name) {
                    case "Save":   r.put("save", admin || v); break;
                    case "Update": r.put("update", admin || v); break;
                    case "Print":  r.put("print", admin || v); break;
                    case "Delete": r.put("delete", v); break;
                    case "Rate":   r.put("rateAndAmount", v); break;
                    case "ProductionSettlement": r.put("settlementTab", v); break;
                    case "SaveLayout": r.put("saveLayout", admin || v); break;
                    case "GridPrint": r.put("gridPrint", admin || v); break;
                    case "GridExport": r.put("gridExport", admin || v); break;
                    default: break;
                }
            }
        } catch (Exception ignored) { /* the defaults above stay */ }
        return r;
    }

    // =================================================================================== Load

    /**
     * frmFoodProduction_Load:893 - the three configurations (:901), the rights (:920), and every
     * list the Load binds: stockAccoutnFill:1097, combojoblotfill:1115, CropYear:1132,
     * Warehouse:1085, bagType:1144, JobOrderNoFill:1180, JobOrderNoFillForSummery:1223,
     * UOMFill:1252, EntryTypeFill:1269, BindProductionDepartment:1163, GetStaticUOM (:900),
     * GenerateDocNumberInput:1595.
     */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenId", SCREEN_ID);
        out.put("screenName", SCREEN_NAME);
        out.put("rights", rights());

        /* :901 GetMultipleConfigurationsByConfigDescriptions - three names, one call */
        boolean wages = false, loader = false, manualNotAllowed = false;
        try {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org());
            p.put("@CompanyId", comp());
            p.put("@ConfigDescription", "ContractorWagesCompulsoryBeforeInvoices,IssuanceByLoader,IsManualEntryOnInputNotAllowed");
            p.put("@Activity", "GetMultipleConfigurationsByConfigDescriptions");
            for (Map<String, Object> r : inRepo.query("dbo.Sp_ConfigrationsAllocation_GetAllMethod", p)) {
                String n = str(ci(r, "ConfigDescription"));
                boolean v = asBool(ci(r, "ConfigKey"));
                if ("ContractorWagesCompulsoryBeforeInvoices".equals(n)) wages = v;
                if ("IssuanceByLoader".equals(n)) loader = v;
                if ("IsManualEntryOnInputNotAllowed".equals(n)) manualNotAllowed = v;
            }
        } catch (Exception ignored) { }
        out.put("wagesStatus", wages);
        out.put("loaderStatus", loader);
        out.put("isManualEntryOnInputNotAllowed", manualNotAllowed);

        List<String> errors = new ArrayList<>();
        out.put("differenceAccounts", safe(errors, "Difference Account", () -> differenceAccounts()));
        out.put("jobLots", safe(errors, "Job Lot", () -> project(jobLots(), "Id", "JobLotDescription")));
        out.put("cropYears", safe(errors, "Crop Year", () -> project(inRepo.cropYears(user()), "Id", "CropYear")));
        out.put("warehouses", safe(errors, "WareHouse", () -> project(activeWarehouses(), "Id", "WareHouseName")));
        out.put("packingTypes", safe(errors, "Packing Type", () -> project(inRepo.packingTypes(), "Id", "PackTypeDesc")));
        out.put("jobOrders", safe(errors, "Job Order", () -> jobOrders()));
        out.put("jobOrdersAll", safe(errors, "Job Order (Summery)", () -> jobOrdersAll()));
        out.put("uomSchedules", safe(errors, "UOM Schedule", () -> inRepo.uomSchedules(user())));
        out.put("uomStatic", safe(errors, "Static UOM", () -> ohRepo.uomStatic()));
        out.put("departments", safe(errors, "Production Department", () -> project(inRepo.productionDepartments(user()), "Id", "WareHouseName")));
        /* clsGlobalVariables.stringFormatsingle / DecimalRateFormate (CommonServices.GetDecimalConfiguration):
           "#,##0." + N zeros; a rate setting of 0 or none means two decimals, an amount setting of 0 means none. */
        int amountDec = 0, rateDec = 2;
        try { amountDec = toInt(inRepo.config(user(), "Default NoofDecimal Points For Amount")); } catch (Exception ignored) { }
        try { int r = toInt(inRepo.config(user(), "Default NoofDecimal Points For Rate")); rateDec = r > 0 ? r : 2; } catch (Exception ignored) { }
        out.put("amountDecimals", amountDec > 4 ? 0 : amountDec);
        out.put("rateDecimals", rateDec > 4 ? 2 : rateDec);
        out.put("entryTypes", entryTypes());
        out.put("chargesTypes", chargesTypes());
        try { out.put("docNoInput", serial(DOC_INPUT)); } catch (Exception e) { out.put("docNoInput", 0); errors.add("Doc No: " + rootMessage(e)); }
        out.put("today", LocalDate.now().toString());
        out.put("errors", errors);
        return out;
    }

    /** EntryTypeFill:1269 - 1 ByProduct, 2 FinishGoods (a static table). */
    public static List<Map<String, Object>> entryTypes() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(pair("Id", 1, "Name", "ByProduct"));
        l.add(pair("Id", 2, "Name", "FinishGoods"));
        return l;
    }

    /** grdDetailOverheadSettings:4571 - 1 Processing Charges, 2 Other Charges (a static table). */
    public static List<Map<String, Object>> chargesTypes() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(pair("Id", 1, "Type", "Processing Charges"));
        l.add(pair("Id", 2, "Type", "Other Charges"));
        return l;
    }

    /** stockAccoutnFill:1102 - CoaAllocationAccountTitleByAccountTypeIds("4,12"). */
    public List<Map<String, Object>> differenceAccounts() {
        return project(stRepo.accountTitlesByAccountTypeIds(org(), comp(), ctx.currentAppId(), ctx.currentUserId(), "4,12"),
                "Id", "AccountTitle");
    }

    /** combojoblotfill:1119 - CommonServices.JobLotGetAllService -> SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLots() {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@Activity", "GetAll");
        return inRepo.query("dbo.SP_JobLot_ReadMethod", p);
    }

    /** Warehouse:1089 - CommonServices.getActiveWareHouse -> Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'. */
    public List<Map<String, Object>> activeWarehouses() {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@Activity", "GetActiveWareHouse");
        return inRepo.query("dbo.Sp_InvWareHouse_GetAllMethod", p);
    }

    /**
     * gridsettingsOutPut:2622 / grdDetailPackingMaterialSettings:3859 - CommonServices.WareHouseGetAllService
     * -> Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId' (the grid drop-down list).
     */
    public List<Map<String, Object>> allWarehouses() {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return project(inRepo.query("dbo.Sp_InvWareHouse_GetAllMethod", p), "Id", "WareHouseName");
    }

    /**
     * JobOrderNoFill:1198 - InvProductionJobOrder.GetJobOrderNoForInvFoodProduction(DocumentTypeId 401,
     * FinancialYearId) -> Sp_InvProductionJobOrder_GetAllMethod 'GetJobOrderNoForInvFoodProduction'
     * (Id, PlanCode = RefInvoiceNo). Bound to cmbJobOrderPackingMaterial, cmbJobOrderOverHead,
     * CmbJobOrderNo, CmbJobOrderNoOutput.
     */
    public List<Map<String, Object>> jobOrders() {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@DocumentTypeId", JOB_ORDER_DOC_TYPE);
        if (fy() != 0) p.put("@FinancialYearId", fy());
        p.put("@Activity", "GetJobOrderNoForInvFoodProduction");
        return project(inRepo.query("dbo.Sp_InvProductionJobOrder_GetAllMethod", p), "Id", "PlanCode");
    }

    /** JobOrderNoFillForSummery:1239 - InvProductionJobOrder.GetJobOrderAll -> 'GetJobOrderNoAll' (no ActionId). */
    public List<Map<String, Object>> jobOrdersAll() {
        return project(stRepo.jobOrdersAll(org(), comp(), fy(), 0), "Id", "PlanCode", "ApprovalStatus", "SettledStatus");
    }

    /** GetPlantFeeder:1331 - InvProductionJobOrder.GetPlantByJobOrderId(Id, BranchesId) -> 'GetPlantByJobOrderId'. */
    public List<Map<String, Object>> plants(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", jobOrderId);
        p.put("@BranchesId", branch());
        p.put("@Activity", "GetPlantByJobOrderId");
        return project(inRepo.query("dbo.Sp_InvProductionJobOrder_GetAllMethod", p), "PlantId", "PlantName");
    }

    /** CmbJobOrderNo_Leave:1635 / CmbJobOrderNoOutput_Leave:2525 - GetGlAccountsByJobOrderId. */
    public List<Map<String, Object>> glAccounts(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return inRepo.glAccountsByJobOrder(user(), jobOrderId);
    }

    /**
     * GenerateDocNumberInput:1606 (80) / GenerateDocNumberOutPut:2449 (112) - InvFoodProduction.GetSerialNumber:
     * Sp_InvFoodProduction_GetAllMethod 'ReadSerialNumber', column DocCode.
     */
    public int serial(int documentTypeId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@DocumentTypeId", documentTypeId);
        if (fy() != 0) p.put("@FinancialYearId", fy());
        if (branch() != 0) p.put("@BranchesId", branch());
        p.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> rows = inRepo.query("dbo.Sp_InvFoodProduction_GetAllMethod", p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocCode"));
    }

    /** GenerateCodeOfPackingMaterial:3381 - InvFoodProductionPackingMaterial.GenerateCode. */
    public int pmNextCode() { return pmRepo.generateCode(org(), comp(), fy(), branch()); }

    /** GenerateCodeofOverHead:4180 - InvFoodProductionOverHeads.GenerateCode. */
    public int ohNextCode() { return ohRepo.generateCode(org(), comp(), fy(), branch()); }

    /**
     * CmbJobOrderNoOutput_Leave:2482 / GetTotalInPutOutOnPM:3424 / GetTotlInPutOut:4220 -
     * InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId (no PlantId on this form). Rows
     * EntryType / EntryTypeOutPut / ItemQty / StockWeight; the page sums them as the desktop does.
     */
    public List<Map<String, Object>> totals(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return outRepo.inputTotals(org(), comp(), jobOrderId, 0);
    }

    /** GetVoucherHeadId:1373 - CommonServices.VoucherHeadIdGet(Id, DocumentTypeId). */
    public int voucherHeadId(int id, int documentTypeId) {
        return id <= 0 ? 0 : inRepo.voucherHeadId(user(), documentTypeId, id);
    }

    /** grdByProduct_KeyDown:2764 F1 on ItemUOM / ProporationFinishGoodsSettlement:6468 - GetUomScheduleByItemId. */
    public List<Map<String, Object>> uomsForItem(int itemId) {
        return inRepo.uomsForItem(user(), itemId);
    }

    // ============================================================================ read one

    /**
     * ReadByIdInput:1891 / ReadByIdOutPut:2810 / DatagridHistoryDetail:5235 / InputDetailByHeaderId:6959 -
     * InvFoodProduction.GetByID: the header ('GetById') and its details ('InvFoodProductionId').
     */
    public Map<String, Object> production(int id) {
        Map<String, Object> h = inRepo.header(id);
        if (h == null) throw new Prod284Message("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>(h);
        out.put("details", inRepo.details(id));
        out.put("voucherHeadId", inRepo.voucherHeadId(user(), toInt(ci(h, "DocumentTypeId")), id));
        return out;
    }

    /**
     * InputGridHistory:2244 / OutputGridHistory:3083 / GridHistory:5128 - InvFoodProduction.GetAll
     * (Org, Company, Branch, FinancialYear; 'ReadAll'). The desktop filters DocumentTypeId 80 / 112 in
     * memory for the two tab histories and shows everything on Transaction History; the same filter
     * is applied here. The Transaction History search boxes (:1023-1054) have no handler on the
     * desktop (btnSearch has no Click), so the grid always shows the unfiltered GetAll.
     */
    public List<Map<String, Object>> history(int documentTypeId) {
        List<Map<String, Object>> rows = inRepo.history(user(), fy(), branch(), null, null, 0, 0, 0);
        if (documentTypeId == 0) return rows;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) if (toInt(ci(r, "DocumentTypeId")) == documentTypeId) out.add(r);
        return out;
    }

    /** ReadByIdPackingMaterial:3686 - InvFoodProductionPackingMaterial.GetByID ('ReadById'). */
    public Map<String, Object> pm(int id) {
        Map<String, Object> r = pmRepo.readById(id);
        if (r == null) throw new Prod284Message("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>(r);
        out.put("voucherHeadId", pmRepo.voucherHeadId(org(), comp(), DOC_PM, id));
        return out;
    }

    /** BindHistoryPMGrid:3952 - InvFoodProductionPackingMaterial.GetAll; chkPendingForRates -> ActionId 1. */
    public List<Map<String, Object>> pmHistory(boolean pendingForRates) {
        return pmRepo.history(org(), comp(), fy(), branch(), pendingForRates ? 1 : 0,
                null, null, null, null, null, null, null, null, 0, 0, 0);
    }

    /** ReadByIdOverHeadJobOrderWise:4438 / grdOverHeadHistory_SelectionChanged:4866 - InvFoodProductionOverHeads.GetByJobOrderID. */
    public Map<String, Object> ohByJobOrder(int jobOrderId) {
        List<Map<String, Object>> rows = ohRepo.byJobOrder(jobOrderId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        /* GetVoucherHeadId(RecOverHeadId, 110) - RecOverHeadId is the JOB ORDER id on this form (:4844) */
        out.put("voucherHeadId", ohRepo.voucherHeadId(org(), comp(), DOC_OH, jobOrderId));
        return out;
    }

    /**
     * BindGridHistoryOverHead:4748 - InvFoodProductionOverHeads.GetAll, then the LINQ grouping by
     * InvProductionJobOrderId (:4759): TotalItemQty = Sum(ItemQty), TotalExpense = Sum(Amount),
     * AverageItemRate = Average(ItemRate), ProductionJobOrder = first PlanCode, IsApproved = first JobOrderStatus.
     */
    public List<Map<String, Object>> ohHistory() {
        List<Map<String, Object>> rows = ohRepo.history(org(), comp(), fy(), branch());
        LinkedHashMap<Integer, Map<String, Object>> groups = new LinkedHashMap<>();
        LinkedHashMap<Integer, double[]> acc = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            int jo = toInt(ci(r, "InvProductionJobOrderId"));
            Map<String, Object> g = groups.get(jo);
            if (g == null) {
                g = new LinkedHashMap<>();
                g.put("InvProductionJobOrderId", jo);
                g.put("ProductionJobOrder", ci(r, "PlanCode"));
                g.put("IsApproved", ci(r, "JobOrderStatus"));
                groups.put(jo, g);
                acc.put(jo, new double[]{0, 0, 0, 0});
            }
            double[] a = acc.get(jo);
            a[0] += toDouble(ci(r, "ItemQty"));
            a[1] += toDouble(ci(r, "Amount"));
            a[2] += toDouble(ci(r, "ItemRate"));
            a[3] += 1;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map.Entry<Integer, Map<String, Object>> e : groups.entrySet()) {
            double[] a = acc.get(e.getKey());
            Map<String, Object> g = e.getValue();
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("InvProductionJobOrderId", g.get("InvProductionJobOrderId"));
            m.put("ProductionJobOrder", g.get("ProductionJobOrder"));
            m.put("TotalItemQty", a[0]);
            m.put("TotalExpense", a[1]);
            m.put("AverageItemRate", a[3] > 0 ? a[2] / a[3] : 0);
            m.put("IsApproved", g.get("IsApproved"));
            out.add(m);
        }
        return out;
    }

    // ================================================================= the four loader dialogs

    /** LoadInputDataByJobOrder (btnLoadInvoices_Click:2136) - USP_LoadInputDataByInvProductionJobOrder @Id. */
    public List<Map<String, Object>> loadInput(int jobOrderId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", jobOrderId);
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        return inRepo.query("dbo.USP_LoadInputDataByInvProductionJobOrder", p);
    }

    /** LoadOutPutDataByJobOrder (btnLoadOutPutFromjobOrder_Click:3276) - USP_LoadOuPutDataByInvProductionJobOrder. */
    public List<Map<String, Object>> loadOutput(int jobOrderId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", jobOrderId);
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        return inRepo.query("dbo.USP_LoadOuPutDataByInvProductionJobOrder", p);
    }

    /** LoadPackingMaterialDataByJobOrder (btnLoadPMFromJobOrder_Click:3758) - USP_LoadPackingMaterialDataByInvProductionJobOrder. */
    public List<Map<String, Object>> loadPm(int jobOrderId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", jobOrderId);
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        return inRepo.query("dbo.USP_LoadPackingMaterialDataByInvProductionJobOrder", p);
    }

    /** LoadOverHeadsDataByJobOrder (btnLoadOverHeadsFromJobOrder_Click:5066) - USP_LoadOverHeadsDataByInvProductionJobOrder. */
    public List<Map<String, Object>> loadOh(int jobOrderId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", jobOrderId);
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        return inRepo.query("dbo.USP_LoadOverHeadsDataByInvProductionJobOrder", p);
    }

    /** LoadOutPutPendingforRates (btnPenForRateEntries_Click:3259) - JobOrderFill (DocumentTypeId 403). */
    public List<Map<String, Object>> pendingRateJobOrders() {
        return project(outRepo.pendingRatesJobOrders(org(), comp(), fy()), "Id", "PlanCode");
    }

    /** LoadOutPutPendingforRates - OutputGridHistory: USP_GetProductionOutPutDataForRatesInput. */
    public List<Map<String, Object>> pendingRates(String from, String to, int jobOrderId) {
        return outRepo.pendingRates(org(), comp(), branch(), dateOnly(from), dateOnly(to), jobOrderId, null);
    }

    // ======================================================================= Summery tab

    /** GrdSummaryValuesFill:5455 / txtFGWeight_TextChanged:6906 / settlement :5927 - InvFoodProduction.GetSummeryValues. */
    public List<Map<String, Object>> summaryValues(int jobOrderId, double netWeight) {
        return outRepo.summaryValues(org(), comp(), jobOrderId, netWeight);
    }

    /**
     * GrdSummaryReportFill:5378 / btnRecoveryReport_Click:6777 - InvFoodProductionReports
     * .InvFoodProductionRecoverySummeryReport -> Sp_InvFoodProduction_Summery_Rpt (@JobOrderId; @PlantId
     * and @ActionId guarded != 0).
     */
    public List<Map<String, Object>> summaryReport(int jobOrderId, int plantId, int actionId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@JobOrderId", jobOrderId);
        if (plantId != 0) p.put("@PlantId", plantId);
        if (actionId != 0) p.put("@ActionId", actionId);
        return inRepo.query("dbo.Sp_InvFoodProduction_Summery_Rpt", p);
    }

    /** grdDocWiseSummeryFill:5520 - FoodProduction_DocWiseSummeryReport -> USP_FoodProduction_DocWiseSummeryReport. */
    public List<Map<String, Object>> docWiseSummary(int jobOrderId, int plantId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@JobOrderId", jobOrderId);
        if (plantId != 0) p.put("@PlantId", plantId);
        return inRepo.query("dbo.USP_FoodProduction_DocWiseSummeryReport", p);
    }

    // ======================================================================= Settlement tab

    /** btnGeneralSettlement_Click:5796 - InvFoodProductionSettlement with 'InPutOutPut' / 'OverHeads' / 'PackingMaterial'. */
    public List<Map<String, Object>> settlement(int jobOrderId, String activity) {
        if (jobOrderId <= 0) throw new Prod284Message("JobOrder Settlement Field is Required");
        return stRepo.settlementData(org(), comp(), jobOrderId, activity);
    }

    // ============================================================================== prints

    /**
     * Print_Click:2202 / btnPrintOutPut_Click:3219 - InvFoodProductionReports.InvFoodProductionSlip
     * (Id, ReqType "Input"/"Output", JobOrderId) -> Sp_InvFoodProduction_Rpt into 601-InvFoodProductionSlip.rpt.
     */
    public List<Map<String, Object>> print601(int id, String entryType, int jobOrderId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        if (id != 0) p.put("@Id", id);
        if (entryType != null && !entryType.isEmpty()) p.put("@EntryType", entryType);
        if (jobOrderId != 0) p.put("@JobOrderId", jobOrderId);
        return inRepo.query("dbo.Sp_InvFoodProduction_Rpt", p);
    }

    /** btnPackingMaterialSlip_Click:4108 ("PackingMaterial") / btnPrintOverHead_Click:5019 ("OverHead") -> 606. */
    public List<Map<String, Object>> print606(int jobOrderId, String reportType) {
        return pmRepo.report606(org(), comp(), jobOrderId, reportType);
    }

    /** btn607ProductionIssuanceGrnWiseReport_Click:5738 - FoodProductionIssuanceGrnWiseByJobOrderId_607. */
    public List<Map<String, Object>> print607(int jobOrderId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        if (jobOrderId != 0) p.put("@InvJobOrderId", jobOrderId);
        return inRepo.query("dbo.Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt", p);
    }

    /** btn613SummaryReport_Click:6711 (Summery2) / btn615Report_Click:6744 (Summery3); ActionId 1 = Include Wages. */
    public List<Map<String, Object>> printSummary(String rpt, int jobOrderId, int actionId) {
        String proc;
        if ("613".equals(rpt)) proc = "dbo.Sp_InvFoodProduction_Summery2_Rpt";
        else if ("615".equals(rpt)) proc = "dbo.Sp_InvFoodProduction_Summery3_Rpt";
        else if ("602A".equals(rpt)) return summaryReport(jobOrderId, 0, actionId);
        else throw new Prod284Message("Unknown report " + rpt);
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", org());
        p.put("@CompanyId", comp());
        p.put("@JobOrderId", jobOrderId);
        if (actionId != 0) p.put("@ActionId", actionId);
        return inRepo.query(proc, p);
    }

    // ============================================================================== helpers

    interface Q<T> { T get() throws Exception; }

    private static <T> T safe(List<String> errors, String what, Q<T> q) {
        try { return q.get(); } catch (Exception e) { errors.add(what + ": " + rootMessage(e)); return null; }
    }

    public static List<Map<String, Object>> project(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, ci(r, c));
            out.add(m);
        }
        return out;
    }

    private static Map<String, Object> pair(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k1, v1);
        m.put(k2, v2);
        return m;
    }

    static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    /** Conversion.ToBool - true/false, 1/0 and "1"/"0" alike. */
    static boolean asBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o).toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }

    static LocalDateTime dateOnly(String v) {
        if (v == null || v.trim().isEmpty()) return null;
        try { return LocalDate.parse(v.trim().substring(0, 10)).atStartOfDay(); } catch (Exception e) { return null; }
    }

    /** A DateTimePicker value: the chosen day with the current time of day (as an untouched picker carries it). */
    static LocalDateTime docDateTime(String v) {
        LocalDateTime d = dateOnly(v);
        if (d == null) return LocalDateTime.now().withNano(0);
        if (v.trim().length() > 10) {
            try { return LocalDateTime.parse(v.trim().length() == 16 ? v.trim() + ":00" : v.trim().substring(0, 19)); } catch (Exception ignored) { }
        }
        return LocalDateTime.of(d.toLocalDate(), LocalTime.now().withNano(0));
    }

    public static String rootMessage(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.isEmpty()) m = e.getMessage();
        return m == null ? e.getClass().getSimpleName() : m;
    }
}
