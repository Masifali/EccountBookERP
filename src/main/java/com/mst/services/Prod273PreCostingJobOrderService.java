package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Prod273PreCostingJobOrderRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.Prod273Support.blank;
import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.day;
import static com.mst.services.Prod273Support.list;
import static com.mst.services.Prod273Support.map;
import static com.mst.services.Prod273Support.ok;
import static com.mst.services.Prod273Support.project;
import static com.mst.services.Prod273Support.stamp;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.truthy;

/**
 * Production hub (module 18) — screen 273 "Production Job Order (Pre Costing)".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.Production.ProductionPreCostingAndJobOrder (.cs, 14493 lines)
 *                  base.Name = "ProductionPreCostingAndJobOrder" (the rights key)
 *   DocumentTypeId 401 (:1013, :1405, :1701, :4688, :4922)
 *   Route          /production/pre-costing-job-order     API /production/pre-costing-job-order/api
 *   Print          CommonServices.PrintJobOrder630 → 630-PreCostProductionJobOrderSlip.rpt (:2035, :4884, :6076)
 *   Sibling        screen 281 frmProductionJobOrderMain → /production/job-order (same header model 0479,
 *                  DAL 0273; this screen fills Input / Output / OverHeads / Packing Material / Plant /
 *                  Lab Standard instead of the rate schedules)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED:
 * <ol>
 *  <li>Insert():1649 order — "OutPut Grid Record Not Found", "Input Grid Record Not Found", FormValidation
 *      (Plan Code, "Manual ReportNo", Production Type, WorkInProcess, WIPItem, WIP Warehouse, Plan Type,
 *      Status), the Lab Standard controls, confirm, Run Calculations, the two "are not equal" confirms, then
 *      the per-row checks in grid order Output → Input → Packing Material → OverHeads → Plant → Lab. The page
 *      runs the pre-confirm part; the server runs everything again before writing (same words).</li>
 *  <li>A Packing Material row is sent only when PackingItemId > 0, an OverHead row only when Amount > 0 and
 *      the account is set (:1892, :1951); the rows that fail that test are silently dropped by the desktop.</li>
 *  <li>Removed rows (lstRemove*) are sent with ActionTypeId 3 only on an Update (:1791, :1879, :1938, :1986);
 *      Reset clears the lists. A row with Id 0 is simply dropped on the desktop and never reaches the list.</li>
 *  <li>Row Ids are kept only when RECID != 0 (:1766 etc.); SaveAs zeroes RECID so every row inserts again.</li>
 *  <li>Lab Standard rows all carry the header's Brand Item, Party and Schedule (FillLabGroupAnalysisStandardsFromGrid :2046).</li>
 *  <li>JobOrderCreatewithoutRates (config) switches every rate / amount check off (:1746, :1842, :1914, :1968).</li>
 *  <li>The PM "ItemRate Required" check also needs the CanRateandAmount right (:1914).</li>
 *  <li>History sends DocumentTypeIds "401" (unlike screen 281, which leaves it unset) — BindHistoryGrid:4688.</li>
 *  <li>GetLastWipAccount / GenerateCode use 401 (:1013, :1405).</li>
 * </ol>
 *
 * DEVIATIONS (web-only, with reason):
 * <ol>
 *  <li>Tenancy: open / update only a 401 header of the signed-in company (the page could otherwise post any id).</li>
 *  <li>Attachments (btnattachment, Attachment dialog) are not ported; the history link lists the stored names.</li>
 *  <li>The Load Invoices loader form (LoadavailableTransactionsForIssuance) source was not recovered; the modal
 *      is rebuilt from the procedures the web port of screen 280 already calls for the same dialog.</li>
 *  <li>FormHelper.ValidateControls / ValidateField texts (Lab Standards) are not in the recovered source; the
 *      page uses "&lt;caption&gt; Field Required" / "&lt;caption&gt; Field Required in Lab Standard Row No: n".</li>
 *  <li>A loaded Packing Material row shows the Brand / Brand UOM resolved from the Output grid; the desktop's
 *      read procedure returns no BrandName column so it shows blanks.</li>
 * </ol>
 */
@Service
public class Prod273PreCostingJobOrderService {

    public static final String SCREEN = "ProductionPreCostingAndJobOrder";
    public static final int DOC_TYPE = 401;
    /** grdInputDetail_DoubleClick:2553 / grdPlannedOutput_DoubleClick:3343. */
    public static final int PRODUCTION_INPUT_DOC_TYPE = 80;
    public static final int PRODUCTION_OUTPUT_DOC_TYPE = 112;

    private static final String[][] PLAN_TYPES = { { "1", "Local" }, { "2", "Export" }, { "3", "Party" } };          // PlanTypeBind:5504
    private static final String[][] STATUSES = { { "1", "In Process" }, { "2", "Complete" }, { "3", "Cancel" } };    // StatusBind:5543
    private static final String[][] ENTRY_TYPES = { { "1", "Input" }, { "2", "Output" } };                             // EntryTypeFill:2233
    private static final String[][] OUTPUT_ENTRY_TYPES = { { "1", "ByProduct" }, { "2", "FinishGoods" } };            // EntryTypeOutPut:2996
    private static final String[][] FILTER_TYPES = { { "1", "SaleOrder" }, { "2", "Export Contract" }, { "3", "Item Type" }, { "4", "Item Category" } }; // :1097
    private static final Set<Integer> OVERHEAD_ACCOUNT_TYPES = new HashSet<>(Arrays.asList(6, 8, 11, 12, 13, 14, 20, 21));   // :3688

    private final Prod273Support db;
    private final Prod273PreCostingJobOrderRepository repo;

    public Prod273PreCostingJobOrderService(Prod273Support db, Prod273PreCostingJobOrderRepository repo) {
        this.db = db;
        this.repo = repo;
    }

    // ====================================================================================== load

    /** frmProductionJobOrder_Load:769 — rights, the two flags, every picker, GetLastSaveAccount, GeneratePlanCode. */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        int branch = db.branchId(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("jobOrderCreatewithoutRates", db.configFlag(u, "JobOrderCreatewithoutRates"));                  // :825
        out.put("checkStockAndGiveWarningMessageOnJobOrder", db.configFlag(u, "CheckStockAndGiveWarningMessageOnJobOrder")); // :826
        out.put("branchId", branch);
        out.put("rateDecimals", db.rateDecimals(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.put("parties", parties(u));                                                                        // :839-841
        out.put("allUoms", db.allUoms(u));                                                                     // :842
        out.put("wipItems", project(repo.wipItems(u), "Id", "ItemName"));                                     // :843
        out.put("jobLots", project(repo.jobLotsAllocatedToBranch(u, branch), "Id", "JobLotDescription"));      // :844
        out.put("accounts", project(repo.accountsByType(u, appId(), "4"), "Id", "AccountTitle"));              // :845
        out.put("planTypes", Prod273Support.literal(PLAN_TYPES, "Type"));                                      // :846
        out.put("planCode", repo.nextCode(u, DOC_TYPE, db.financialYearId()));                                 // :847
        out.put("plants", project(repo.plantsAllocatedToBranch(u), "PlantId", "PlantName"));                  // :848
        out.put("productionTypes", project(repo.productionTypes(), "Id", "ProductionTypeDescription"));        // :849
        out.put("statuses", Prod273Support.literal(STATUSES, "Status"));                                      // :850
        out.put("entryTypes", Prod273Support.literal(ENTRY_TYPES, "name"));                                   // :851
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));                                   // :852
        out.put("warehouses", warehouses(u, branch));                                                          // :853
        out.put("itemsReadAll", items(db.itemsReadAll(u)));                                                   // :854 ItemDetailFill
        out.put("itemsGetAll", items(db.itemsGetAll(u)));                                                     // :1154 ItemGetAllServiceBind
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));                          // :855
        out.put("filterTypes", Prod273Support.literal(FILTER_TYPES, "name"));                                 // :856
        out.put("packingMaterialItems", db.itemsByType(u, "14"));                                             // :858
        out.put("outputEntryTypes", Prod273Support.literal(OUTPUT_ENTRY_TYPES, "name"));                      // :859
        out.put("overheadAccounts", overheadAccounts(u));                                                      // :860
        Map<String, Object> last = repo.lastWipAccount(u, DOC_TYPE);                                           // :861
        out.put("lastWipAccountId", last == null ? 0 : toInt(ci(last, "AccountId")));
        out.put("lastWipItemId", last == null ? 0 : toInt(ci(last, "WipItemId")));
        out.put("lastWipWareHouseId", last == null ? 0 : toInt(ci(last, "WipWareHouseId")));
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));                 // :874
        out.put("jobOrderNos", jobOrderNos(u));                                                                // :875
        return out;
    }

    /** btnRefresh_Click:1520 — the lists it rebinds (no Reset). */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        int branch = db.branchId(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("checkStockAndGiveWarningMessageOnJobOrder", db.configFlag(u, "CheckStockAndGiveWarningMessageOnJobOrder"));
        out.put("wipItems", project(repo.wipItems(u), "Id", "ItemName"));
        out.put("jobLots", project(repo.jobLotsAllocatedToBranch(u, branch), "Id", "JobLotDescription"));
        out.put("accounts", project(repo.accountsByType(u, appId(), "4"), "Id", "AccountTitle"));
        out.put("planTypes", Prod273Support.literal(PLAN_TYPES, "Type"));
        out.put("productionTypes", project(repo.productionTypes(), "Id", "ProductionTypeDescription"));
        out.put("plants", project(repo.plantsAllocatedToBranch(u), "PlantId", "PlantName"));
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));
        out.put("warehouses", warehouses(u, branch));
        out.put("itemsReadAll", items(db.itemsReadAll(u)));
        out.put("itemsGetAll", items(db.itemsGetAll(u)));
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
        out.put("filterTypes", Prod273Support.literal(FILTER_TYPES, "name"));
        out.put("allUoms", db.allUoms(u));
        out.put("parties", parties(u));
        return out;
    }

    /** Reset:1477/1496 — GeneratePlanCode + GetLastSaveAccount. */
    public Map<String, Object> resetData() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("planCode", repo.nextCode(u, DOC_TYPE, db.financialYearId()));
        Map<String, Object> last = repo.lastWipAccount(u, DOC_TYPE);
        out.put("lastWipAccountId", last == null ? 0 : toInt(ci(last, "AccountId")));
        out.put("lastWipItemId", last == null ? 0 : toInt(ci(last, "WipItemId")));
        out.put("lastWipWareHouseId", last == null ? 0 : toInt(ci(last, "WipWareHouseId")));
        return out;
    }

    private int appId() {
        try { return db.user().getAppId() == null ? 0 : db.user().getAppId(); } catch (RuntimeException e) { return 0; }
    }

    private List<Map<String, Object>> warehouses(UserAccount u, int branch) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.warehousesAllocatedToBranch(u, branch)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "WareHouseName")));
            out.add(o);
        }
        return out;
    }

    /** Both item lists keep ItemCategoryId / ItemTypeId — cmbDocNo_Leave:1164/1185 filters them in memory. */
    private static List<Map<String, Object>> items(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ItemName")));
            o.put("ItemCategoryId", toInt(ci(r, "ItemCategoryId")));
            o.put("ItemTypeId", toInt(ci(r, "ItemTypeId")));
            out.add(o);
        }
        return out;
    }

    /** ChartOfAccountOverHead:3674 — CoaAllocationGetAllServiceBind filtered on AccountTypeId 6,8,11,12,13,14,20,21. */
    private List<Map<String, Object>> overheadAccounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.coaAllocation(u)) {
            if (!OVERHEAD_ACCOUNT_TYPES.contains(toInt(ci(r, "AccountTypeId")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "AccountTitle")));
            out.add(o);
        }
        return out;
    }

    /** SupplierDtFillFromStandard:6858 — the raw rows; the page builds "Parent / Child" names per radio. */
    private List<Map<String, Object>> parties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.partiesFromStandard(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CompanyName", str(ci(r, "CompanyName")));
            o.put("NickName", str(ci(r, "NickName")));
            o.put("PartyCode", str(ci(r, "PartyCode")));
            o.put("GlAccountId", toInt(ci(r, "GlAccountId")));
            o.put("CityName", str(ci(r, "CityName")));
            o.put("MobileNo", str(ci(r, "MobilePersonal")));
            o.put("IsSubSupCust", truthy(ci(r, "IsSubSupCust")));
            o.put("ParentsSupCustId", toInt(ci(r, "ParentsSupCustId")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> jobOrderNos(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.jobOrderNoDropDown(u)) {
            if (!"JobOrderNo".equals(str(ci(r, "Activity")).trim())) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            out.add(o);
        }
        return out;
    }

    // ============================================================================ cascades

    /** cmbFiltersType_Leave:1109 — the Doc No / Other list for the chosen filter type. */
    public List<Map<String, Object>> docNoList(int filterType) {
        UserAccount u = db.user();
        switch (filterType) {
            case 1: return project(repo.saleOrderNumbers(u), "Id", "DocNo");
            case 2: return project(repo.lcOrderNumbers(u), "Id", "LcOrderNo");
            case 3: return project(repo.itemTypes(u), "Id", "TypeDescription");
            case 4: return project(repo.itemCategories(u), "Id", "CategoryDescription");
            default: return new ArrayList<>();
        }
    }

    /** cmbDocNo_Leave:1194/1198/1259/1263 — items of a Sale Order (OrderItemId) or Export Contract (ExImItemId). */
    public List<Map<String, Object>> itemsByDoc(int filterType, int docId) {
        UserAccount u = db.user();
        if (filterType == 1) return project(repo.itemsBySaleOrder(docId), "OrderItemId", "ItemName");
        if (filterType == 2) return project(repo.itemsByExportContract(u, docId), "ExImItemId", "ItemName");
        return new ArrayList<>();
    }

    /** cmbItemInput_Leave:2275 — lblStockWeight. */
    public double stockWeight(int itemId, String planDate) {
        return repo.availableStockWeight(db.user(), itemId, Prod273Support.pickerValue(planDate));
    }

    /** InPutRateBind:2874 / BtnGenerateInPutRates_Click:2935 — AvgRateOnlyForCGS(item, PlanDate, 401, RECID, JobLot, CropYearId, CropYear, 0, Branch). */
    public double avgRate(int itemId, String planDate, int recId, int jobLotId, int cropYearId, String cropYear) {
        UserAccount u = db.user();
        if (itemId <= 0) return 0d;
        return db.avgRateOnlyForCgs(u, itemId, Prod273Support.pickerValue(planDate), DOC_TYPE, recId, jobLotId, cropYearId, cropYear, 0, db.branchId(u));
    }

    /** CmbBrandNameLabStandard_Leave / CmbBuyerName_Leave:6936 — ComboFill. */
    public List<Map<String, Object>> analysisGroups(int brandItemId, int supplierCustomerId) {
        return project(repo.analysisGroups(db.user(), brandItemId, supplierCustomerId), "MainId", "ScheduleName");
    }

    /** GridFillByAnalysisGroup:7019 — ReadByMainId, Active rows only. */
    public List<Map<String, Object>> analysisGroupRows(int mainId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (mainId <= 0) return out;
        for (Map<String, Object> r : repo.analysisGroupRows(db.user(), mainId)) {
            if (!truthy(ci(r, "Active"))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("AnalysisGroupId", toInt(ci(r, "LabAnalysisGroupId")));
            o.put("AnalysisGroup", str(ci(r, "LabAnalysisGroupDescription")));
            o.put("AnalysisItemId", toInt(ci(r, "LabAnalysisItemId")));
            o.put("AnalysisItem", str(ci(r, "LabAnalysisItemName")));
            o.put("MinValue", toDouble(ci(r, "MinValue")));
            o.put("MaxValue", toDouble(ci(r, "MaxValue")));
            o.put("Active", true);
            o.put("MainId", mainId);
            out.add(o);
        }
        return out;
    }

    /** grdInputDetail_DoubleClick:2549 (80) / grdPlannedOutput_DoubleClick:3339 (112). */
    public Map<String, Object> referenced(int jobOrderId, int productionDocType) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", repo.referencedByProduction(db.user(), jobOrderId, productionDocType));
        return out;
    }

    /** grdPlantdetail_ColumnButtonClick:6652. */
    public Map<String, Object> plantDeletable(int jobOrderId, int plantId, String plantName) {
        boolean used = repo.plantUsedInProduction(db.user(), jobOrderId, plantId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("deletable", !used);
        if (used) out.put("message", "You cannot be delete " + plantName + " because " + plantName + " Has been referred to production");
        return out;
    }

    // ============================================================================ loader (Load Invoices)

    public Map<String, Object> loaderLookups() {
        UserAccount u = db.user();
        Map<String, List<Map<String, Object>>> lists = new LinkedHashMap<>();
        for (String k : new String[] { "ParentCategories", "ItemCategories", "ItemTypes", "JobLot", "CropYear", "Warehouse", "DocumentType", "Supplier_Customer", "Items" }) {
            lists.put(k, new ArrayList<>());
        }
        for (Map<String, Object> r : repo.issuanceDropDowns(u, db.branchId(u))) {
            List<Map<String, Object>> l = lists.get(str(ci(r, "ActivityType")));
            if (l == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("Name", str(ci(r, "name")));
            l.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("lists", lists);
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        return out;
    }

    public List<Map<String, Object>> loaderSearch(String from, String to, int warehouseId, int itemId, int jobLotId, String cropYear,
                                                  int refDocumentTypeId, int supplierCustomerId, int parentCategory, int itemCategoryId, int itemTypeId) {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.availableTransactionsForIssuance(u, db.branchId(u), Prod273Support.ts(from), Prod273Support.ts(to),
                warehouseId, itemId, jobLotId, cropYear, refDocumentTypeId, supplierCustomerId, parentCategory, itemCategoryId, itemTypeId)) {
            /* dtIssuance columns LoadDataDetailfromPurchaseInvoivce:6139 reads. */
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
            o.put("RefDocIdNo", toInt(ci(r, "RefDocIdNo")));
            o.put("RefDocSubIdNo", toInt(ci(r, "RefDocSubIdNo")));
            o.put("RefDocumentType", str(ci(r, "RefDocumentType")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocNo", toInt(ci(r, "DocCodeNo")));
            o.put("ManualNo", str(ci(r, "ManualNo")));
            o.put("SupplierCustomerName", str(ci(r, "SupplierCustomerName")));
            o.put("WareHouseId", toInt(ci(r, "WarehouseId")));
            o.put("WareHouse", str(ci(r, "WareHouseCode")));
            o.put("JobLotId", toInt(ci(r, "JobLotId")));
            o.put("JobLotCode", str(ci(r, "JobLotCode")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            o.put("CropYearId", toInt(ci(r, "CropYearId")));
            o.put("CropYear", str(ci(r, "CropBatch")));
            o.put("PackingTypeId", toInt(ci(r, "InvPackingTypeId")));
            o.put("PackTypeCode", str(ci(r, "PackingType")));
            o.put("ItemUOMId", toInt(ci(r, "ItemUom")));
            o.put("PackUom", str(ci(r, "PackUom")));
            o.put("ItemUOM", toDouble(ci(r, "PackSize")));
            o.put("QtyIn", toDouble(ci(r, "QtyIn")));
            o.put("QtyOut", toDouble(ci(r, "QtyOut")));
            o.put("QtyBalance", toDouble(ci(r, "QtyBalance")));
            o.put("WeightIn", toDouble(ci(r, "WeightIn")));
            o.put("WeightOut", toDouble(ci(r, "WeightOut")));
            o.put("WeightBalance", toDouble(ci(r, "WeightBalance")));
            o.put("ItemRate", toDouble(ci(r, "AVgRate")));
            o.put("RateUOMId", toInt(ci(r, "RateUomId")));
            o.put("RateUOM", str(ci(r, "RateUom")));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            o.put("ItemAmount", toDouble(ci(r, "ItemAmount")));
            out.add(o);
        }
        return out;
    }

    // ============================================================================ history

    /** BindHistoryGrid:4679 — the 21 dt columns. */
    public List<Map<String, Object>> history(String dateMode, String from, String to, int docNoFrom, int docNoTo, int jobOrderId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, db.financialYearId(), viewAll, u.getId(), dateMode,
                Prod273Support.pickerValue(from), Prod273Support.pickerValue(to), docNoFrom, docNoTo, jobOrderId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocDate", day(ci(r, "PlanDate")));
            o.put("DocNo", toInt(ci(r, "PlanCode")));
            o.put("PlanType", str(ci(r, "PlanType")));
            o.put("JobOrderNo", str(ci(r, "RefInvoiceNo")));
            o.put("ProductionType", str(ci(r, "ProductionType")));
            o.put("StartDate", day(ci(r, "StartDate")));
            o.put("EndDate", day(ci(r, "EndDate")));
            o.put("PlanStatus", str(ci(r, "PlanStatus")));
            o.put("PackingInstruction", str(ci(r, "PackingInstructions")));
            o.put("DeliveryInstruction", str(ci(r, "DeliveryInstructions")));
            o.put("ProductionInstruction", str(ci(r, "ProductionInsturction")));
            o.put("OtherInstruction", str(ci(r, "OtherInstructions")));
            o.put("EntryUser", str(ci(r, "EntryUserName")));
            o.put("EntryDate", stamp(ci(r, "EntryDate")));
            o.put("ModifyUser", str(ci(r, "ModifyUserName")));
            o.put("ModifyDate", stamp(ci(r, "ModifyDate")));
            o.put("ApprovedStatus", str(ci(r, "ApprovalStatus")));
            o.put("ApprovedUser", str(ci(r, "ApprovedUserName")));
            o.put("ApprovedDate", stamp(ci(r, "PostDate")));                      // :4766 PostDate → ApprovedDate
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> attachments(int id) {
        db.user();
        return db.attachments(SCREEN, id);
    }

    // ============================================================================ read

    /** ReadById:2088 / DataGridHistory_SelectionChanged:4950 — GetByID → header + the six grids in dt column names. */
    public Map<String, Object> load(int id) {
        UserAccount u = db.user();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("Id", toInt(ci(h, "Id")));
        head.put("PlanDate", day(ci(h, "PlanDate")));
        head.put("PlanCode", toInt(ci(h, "PlanCode")));
        head.put("InvProductionPlantId", toInt(ci(h, "InvProductionPlantId")));
        head.put("WorkInProccessAcId", toInt(ci(h, "WorkInProccessAcId")));
        head.put("WipWareHouseId", toInt(ci(h, "WipWareHouseId")));
        head.put("JobLotId", toInt(ci(h, "JobLotId")));
        head.put("WipItemId", toInt(ci(h, "WipItemId")));
        head.put("PlanType", str(ci(h, "PlanType")));
        head.put("RefInvoiceNo", str(ci(h, "RefInvoiceNo")));
        head.put("ProductionType", str(ci(h, "ProductionType")));
        head.put("StartDate", day(ci(h, "StartDate")));
        head.put("EndDate", day(ci(h, "EndDate")));
        head.put("PlanStatus", str(ci(h, "PlanStatus")));
        head.put("ProductionInsturction", str(ci(h, "ProductionInsturction")));
        head.put("DeliveryInstructions", str(ci(h, "DeliveryInstructions")));
        head.put("PackingInstructions", str(ci(h, "PackingInstructions")));
        head.put("OtherInstructions", str(ci(h, "OtherInstructions")));
        head.put("IsApproved", truthy(ci(h, "IsApproved")));
        out.put("header", head);

        List<Map<String, Object>> inputs = new ArrayList<>();
        for (Map<String, Object> r : repo.inputRows(id)) {                       // dtInputDetail.Rows.Add order :2127
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
            o.put("RefDocIdNo", toInt(ci(r, "RefDocIdNo")));
            o.put("RefDocSubIdNo", toInt(ci(r, "RefDocSubIdNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("WareHouseId", toInt(ci(r, "WarehouseId")));
            o.put("WareHouse", str(ci(r, "WareHouseName")));
            o.put("JobLotId", toInt(ci(r, "JobLotId")));
            o.put("JobLot", str(ci(r, "JobLotCode")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("CropYearId", toInt(ci(r, "CropYearId")));
            o.put("CropYear", str(ci(r, "BatchCrop")));
            o.put("PackTypeId", toInt(ci(r, "PackingTypeId")));
            o.put("PackType", str(ci(r, "PackingType")));
            o.put("BalanceQty", toDouble(ci(r, "Qty")));
            o.put("BalanceWeight", toDouble(ci(r, "NetWeight")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            o.put("PackSizeId", toInt(ci(r, "UomSchIdQty")));
            o.put("PackSize", str(ci(r, "UOMCode")));
            o.put("PackSizeEquivalent", toDouble(ci(r, "UOMCodeEquivalent")));
            o.put("Weight", toDouble(ci(r, "NetWeight")));
            o.put("ItemRate", toDouble(ci(r, "ItemRate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", str(ci(r, "RateUom")));
            o.put("RateUomEquivalent", toDouble(ci(r, "RateUomEquivalent")));
            o.put("ItemAmount", toDouble(ci(r, "ItemAmount")));
            o.put("Remarks", str(ci(r, "RemarksInputDt")));
            inputs.add(o);
        }
        out.put("inputs", inputs);

        List<Map<String, Object>> outputs = new ArrayList<>();
        for (Map<String, Object> r : repo.outputRows(id)) {                      // dtPlannedOutput.Rows.Add order :2134
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("EntryTypeId", toInt(ci(r, "EntryTypeId")));
            o.put("EntryType", str(ci(r, "EntryType")));
            o.put("WareHouseId", toInt(ci(r, "WarehouseId")));
            o.put("WareHouseName", str(ci(r, "WareHouseName")));
            o.put("JobLotId", toInt(ci(r, "JobLotId")));
            o.put("JobLotDescription", str(ci(r, "JobLotDescription")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("CropYearId", toInt(ci(r, "CropYearId")));
            o.put("CropYear", str(ci(r, "CropYear")));
            o.put("PackingTypeId", toInt(ci(r, "InvPackingTypeId")));
            o.put("PackingTypeName", str(ci(r, "PackTypeDesc")));
            o.put("QtyOuter", toDouble(ci(r, "QtyOuter")));
            o.put("OuterPackSizeId", toInt(ci(r, "UomSchIdOuter")));
            o.put("OuterPackSizeName", str(ci(r, "outerUOMCode")));
            o.put("EquivalentPack", toDouble(ci(r, "outerUOMCodeEquivalent")));
            o.put("Weight", toDouble(ci(r, "NetWeight")));
            o.put("ItemRate", toDouble(ci(r, "ItemRate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", str(ci(r, "RateUom")));
            o.put("EquivalentRate", toDouble(ci(r, "Equivalent")));
            o.put("ItemAmount", toDouble(ci(r, "ItemAmount")));
            o.put("OHExpense", toDouble(ci(r, "OHExpense")));
            o.put("PMExpense", toDouble(ci(r, "PMExpense")));
            o.put("OHGeneralExpense", toDouble(ci(r, "OHGeneralExpense")));
            o.put("PMGeneralExpense", toDouble(ci(r, "PMGeneralExpense")));
            o.put("NetRate", toDouble(ci(r, "NetRate")));
            o.put("NetAmount", toDouble(ci(r, "NetAmount")));
            o.put("Remarks", str(ci(r, "PackRemarks")));
            outputs.add(o);
        }
        out.put("outputs", outputs);

        List<Map<String, Object>> overheads = new ArrayList<>();
        for (Map<String, Object> r : repo.overheadRows(u, id)) {                  // dtOH.Rows.Add order :2141
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ChartOfAccountId", toInt(ci(r, "CharOfAccountId")));
            o.put("ChartOfAccount", str(ci(r, "AccountTitle")));
            o.put("BrandId", toInt(ci(r, "BrandId")));
            o.put("Brand", str(ci(r, "BrandName")));
            o.put("BrandUomId", toInt(ci(r, "BrandUomId")));
            o.put("BrandUom", outputUomName(outputs, toInt(ci(r, "BrandId")), toInt(ci(r, "BrandUomId")), str(ci(r, "BrandUom"))));
            o.put("ItemQty", toDouble(ci(r, "ItemQty")));
            o.put("ItemRate", toDouble(ci(r, "ItemRate")));
            o.put("Amount", toDouble(ci(r, "Amount")));
            o.put("Remarks", str(ci(r, "ohRemarks")));
            overheads.add(o);
        }
        out.put("overheads", overheads);

        List<Map<String, Object>> packing = new ArrayList<>();
        for (Map<String, Object> r : repo.packingRows(u, id)) {                   // dtPM.Rows.Add order :2148
            Map<String, Object> o = new LinkedHashMap<>();
            int brandId = toInt(ci(r, "BrandId")), brandUomId = toInt(ci(r, "BrandUomId"));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("WarehouseId", toInt(ci(r, "WarehouseId")));
            o.put("Warehouse", str(ci(r, "WareHouseName")));
            o.put("PackingItemId", toInt(ci(r, "ItemId")));
            o.put("PackingItem", str(ci(r, "ItemName")));
            o.put("BrandId", brandId);
            o.put("Brand", outputItemName(outputs, brandId, str(ci(r, "BrandName"))));            // DEVIATION 5
            o.put("BrandUomId", brandUomId);
            o.put("BrandUom", outputUomName(outputs, brandId, brandUomId, str(ci(r, "BrandUom"))));
            o.put("ItemQty", toDouble(ci(r, "Qty")));
            o.put("ItemRate", toDouble(ci(r, "Rate")));
            o.put("Amount", toDouble(ci(r, "Amount")));
            o.put("Remarks", str(ci(r, "pmRemarks")));
            packing.add(o);
        }
        out.put("packing", packing);

        List<Map<String, Object>> plants = new ArrayList<>();
        for (Map<String, Object> r : repo.plantRows(id)) {                       // dtPlantdetail.Rows.Add :2155
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("PlantId", toInt(ci(r, "PlantId")));
            o.put("PlantName", str(ci(r, "PlantName")));
            o.put("AssumedPlantCapacity", toDouble(ci(r, "AssumedPlantCapacity")));
            o.put("RemarksDetail", str(ci(r, "RemarksDetail")));
            plants.add(o);
        }
        out.put("plants", plants);

        Map<String, Object> lab = new LinkedHashMap<>();
        List<Map<String, Object>> labRows = new ArrayList<>();
        for (Map<String, Object> r : repo.labRows(id)) {                         // FillDetailFromList:2182
            if (lab.isEmpty()) {                                                  // :2160 — the first row's header values
                lab.put("BrandItemId", toInt(ci(r, "BrandItemId")));
                lab.put("SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")));
                lab.put("LabStandardMainId", toInt(ci(r, "LabStandardMainId")));
            }
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "InvProductionJobOrderLabStandardDetailId")));
            o.put("AnalysisGroupId", toInt(ci(r, "LabAnalysisGroupId")));
            o.put("AnalysisGroup", str(ci(r, "LabAnalysisGroupDescription")));
            o.put("AnalysisItemId", toInt(ci(r, "LabAnalysisItemId")));
            o.put("AnalysisItem", str(ci(r, "LabAnalysisItemName")));
            o.put("MinValue", toDouble(ci(r, "MinValue")));
            o.put("MaxValue", toDouble(ci(r, "MaxValue")));
            o.put("Active", truthy(ci(r, "Active")));
            o.put("MainId", toInt(ci(r, "LabStandardMainId")));
            labRows.add(o);
        }
        lab.put("rows", labRows);
        out.put("lab", lab);
        return out;
    }

    private static String outputItemName(List<Map<String, Object>> outputs, int itemId, String fallback) {
        if (!fallback.isEmpty() || itemId == 0) return fallback;
        for (Map<String, Object> o : outputs) if (toInt(o.get("ItemId")) == itemId) return str(o.get("ItemName"));
        return fallback;
    }

    private static String outputUomName(List<Map<String, Object>> outputs, int itemId, int uomId, String fallback) {
        if (uomId == 0) return "";
        for (Map<String, Object> o : outputs) {
            if (toInt(o.get("ItemId")) == itemId && toInt(o.get("OuterPackSizeId")) == uomId) return str(o.get("OuterPackSizeName"));
        }
        return fallback;
    }

    // ============================================================================ save

    /** Insert():1631 — everything before the confirm box, so the page gets the desktop's first message. */
    public Map<String, Object> precheck(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("Id"));
        requireRight(recId);
        if (recId != 0 && ownedHeader(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(b);
        return ok("", 0, 0);
    }

    private void requireRight(int recId) {
        if (recId == 0 && !db.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !db.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
    }

    private void preConfirm(Map<String, Object> b) {
        if (list(b.get("outputs")).isEmpty()) throw new IllegalArgumentException("OutPut Grid Record Not Found");   // :1651
        if (list(b.get("inputs")).isEmpty()) throw new IllegalArgumentException("Input Grid Record Not Found");     // :1655
        /* FormValidation:1578 */
        if (toInt(b.get("PlanCode")) == 0) throw new IllegalArgumentException("Plan Code Field Required");
        String ref = str(b.get("RefInvoiceNo"));
        if (ref.isEmpty() || "0".equals(ref)) throw new IllegalArgumentException("Manual ReportNo Field Required");
        if (toInt(b.get("ProductionTypeId")) == 0) throw new IllegalArgumentException("Production Type Field Required");
        if (toInt(b.get("WorkInProccessAcId")) == 0) throw new IllegalArgumentException("WorkInProcess Field Required");
        if (toInt(b.get("WipItemId")) == 0) throw new IllegalArgumentException("WIPItem Field Required");
        if (toInt(b.get("WipWareHouseId")) == 0) throw new IllegalArgumentException("WIP Warehouse Field Required");
        if (toInt(b.get("PlanTypeId")) == 0) throw new IllegalArgumentException("Plan Type Field Required");
        if (toInt(b.get("PlanStatusId")) == 0) throw new IllegalArgumentException("Status Field Required");
        /* :1661 — the Lab Standard controls when that grid has rows (DEVIATION 4 for the wording). */
        Map<String, Object> lab = map(b.get("lab"));
        if (!list(lab.get("rows")).isEmpty()) {
            if (toInt(lab.get("LabStandardMainId")) == 0) throw new IllegalArgumentException("Schedule Name Field Required");
            if (toInt(lab.get("BrandItemId")) == 0) throw new IllegalArgumentException("Brand Item Field Required");
        }
    }

    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("Id"));
        requireRight(recId);
        if (recId != 0 && ownedHeader(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(b);
        boolean withoutRates = db.configFlag(u, "JobOrderCreatewithoutRates");
        boolean canRateAndAmount = db.has(SCREEN, "rateAndAmount");

        Timestamp now = Prod273Support.now();
        Map<String, Object> head = new LinkedHashMap<>();                       // Insert():1696 — model 0479 property order
        head.put("IsApproved", Boolean.FALSE);
        head.put("EndDate", Prod273Support.pickerValue(b.get("EndDate")));
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("PlanDate", Prod273Support.pickerValue(b.get("PlanDate")));
        head.put("StartDate", Prod273Support.pickerValue(b.get("StartDate")));
        head.put("PendingForView", Boolean.FALSE);
        head.put("ActionId", recId == 0 ? 1 : 2);                                // BLL 0335 Save
        head.put("DocumentTypeId", DOC_TYPE);
        head.put("BranchesId", db.branchId(u));
        head.put("CompanyId", u.getCompanyId());
        head.put("EntryUser", u.getId());
        head.put("Id", recId);
        head.put("InvProductionPlantId", toInt(b.get("InvProductionPlantId")));
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PlanCode", toInt(b.get("PlanCode")));
        head.put("PlanTypeSrNo", 0);
        head.put("ProjectsId", 0);
        head.put("RefSalesOrderId", 0);
        head.put("SupplierCustomerId", 0);                                       // :1720
        head.put("DeliveryInstructions", str(b.get("DeliveryInstructions")));
        head.put("LotReference", null);                                          // never set by this form → omitted
        head.put("OtherInstructions", str(b.get("OtherInstructions")));
        head.put("PackingInstructions", str(b.get("PackingInstructions")));
        head.put("PlanStatus", str(b.get("PlanStatus")));
        head.put("PlanType", str(b.get("PlanType")));
        head.put("ProductionInsturction", str(b.get("ProductionInsturction")));
        head.put("ProductionType", str(b.get("ProductionType")));
        head.put("RefInvoiceNo", str(b.get("RefInvoiceNo")));
        head.put("WorkInProccessAcId", toInt(b.get("WorkInProccessAcId")));
        head.put("FinishGoodsAcId", 0);
        head.put("ByProductacId", 0);
        head.put("JobLotId", toInt(b.get("JobLotId")));
        head.put("WipItemId", toInt(b.get("WipItemId")));
        head.put("WipWareHouseId", toInt(b.get("WipWareHouseId")));
        head.put("FinancialYearId", db.financialYearId());
        head.put("FinishGoodRate", 0d);

        /* ---- Output rows :1727 ---- */
        List<Map<String, Object>> outputs = new ArrayList<>();
        int idx = 0;
        for (Map<String, Object> r : list(b.get("outputs"))) {
            idx++;
            /* :1731-1745 test the cell TEXT for "" — an unset (null) id; the entry form never adds one. */
            if (blank(r.get("CropYearId"))) throw new IllegalArgumentException("CropYear Field Required in Planned Output Detail Row No: " + idx);
            if (blank(r.get("WareHouseId"))) throw new IllegalArgumentException("WareHouse Field Required in Planned Output Detail Row No: " + idx);
            if (blank(r.get("JobLotId"))) throw new IllegalArgumentException("JobLot Field Required in Planned Output Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("ItemRate")) == 0d) throw new IllegalArgumentException("ItemRate Field is Required in Planned Output Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("ItemAmount")) == 0d) throw new IllegalArgumentException("ItemAmount Field is Required in Planned Output Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("NetRate")) == 0d) throw new IllegalArgumentException("NetRate Field is Required in Planned Output Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("NetAmount")) == 0d) throw new IllegalArgumentException("NetAmount Field is Required in Planned Output Detail Row No: " + idx);
            int id = recId != 0 ? toInt(r.get("Id")) : 0;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", id);
            d.put("InvProductionJobOrderId", 0);
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("InvPackingTypeId", toInt(r.get("PackingTypeId")));
            d.put("QtyInner", toDouble(r.get("QtyOuter")));
            d.put("UomSchIdInner", toInt(r.get("OuterPackSizeId")));
            d.put("QtyOuter", toDouble(r.get("QtyOuter")));
            d.put("UomSchIdOuter", toInt(r.get("OuterPackSizeId")));
            d.put("NetWeight", toDouble(r.get("Weight")));
            d.put("PackRemarks", str(r.get("Remarks")));
            d.put("CropYearId", toInt(r.get("CropYearId")));
            d.put("WarehouseId", toInt(r.get("WareHouseId")));
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("ItemRate", toDouble(r.get("ItemRate")));
            d.put("ItemAmount", toDouble(r.get("ItemAmount")));
            d.put("WagesRate", 0d);
            d.put("WagesAmount", 0d);
            d.put("PMRate", 0d);
            d.put("PMAmount", 0d);
            d.put("OHExpense", toDouble(r.get("OHExpense")));
            d.put("PMExpense", toDouble(r.get("PMExpense")));
            d.put("OHGeneralExpense", toDouble(r.get("OHGeneralExpense")));
            d.put("PMGeneralExpense", toDouble(r.get("PMGeneralExpense")));
            d.put("NetRate", toDouble(r.get("NetRate")));
            d.put("NetAmount", toDouble(r.get("NetAmount")));
            d.put("RateUomId", toInt(r.get("RateUomId")));
            d.put("WagesAccountId", 0);
            d.put("PMItemId", 0);
            d.put("EntryTypeId", toInt(r.get("EntryTypeId")));
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            outputs.add(d);
        }
        if (recId > 0) for (Map<String, Object> r : list(b.get("removedOutputs"))) outputs.add(removedOutput(u, r));   // :1791

        /* ---- Input rows :1798 ---- */
        List<Map<String, Object>> inputs = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("inputs"))) {
            idx++;
            if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field Require in Planned Input Detail Row No: " + idx);
            if (blank(r.get("CropYear"))) throw new IllegalArgumentException("CropYear Field Require in Planned Input Detail Row No: " + idx);
            if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field Required in Planned Input Detail Row No: " + idx);
            if (toDouble(r.get("Qty")) == 0d) throw new IllegalArgumentException("Quantity Field Require in Planned Input Detail Row No: " + idx);
            if (toInt(r.get("PackSizeId")) == 0) throw new IllegalArgumentException("PackSize Field Required in Planned Input Detail Row No: " + idx);
            if (toDouble(r.get("Weight")) == 0d) throw new IllegalArgumentException("Weight Field Required in Planned Input Detail Row No: " + idx);
            if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Require in Planned Input Detail Row No: " + idx);
            if (toInt(r.get("PackTypeId")) == 0) throw new IllegalArgumentException("Packing Type Field Require in Planned Input Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("ItemRate")) == 0d) throw new IllegalArgumentException("ItemRate Field is Required in Planned Input Detail Row No: " + idx);
            if (!withoutRates && toInt(r.get("RateUomId")) == 0) throw new IllegalArgumentException("RateUom Field is Required in Planned Input Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("ItemAmount")) == 0d) throw new IllegalArgumentException("ItemAmount Field is Required in Planned Input Detail Row No: " + idx);
            int id = recId != 0 ? toInt(r.get("Id")) : 0;
            inputs.add(inputRow(r, id, id <= 0 ? 1 : 2));
        }
        if (recId > 0) for (Map<String, Object> r : list(b.get("removedInputs"))) inputs.add(inputRow(r, toInt(r.get("Id")), 3));   // :1879

        /* ---- Packing Material rows :1886 — only rows with a packing item ---- */
        List<Map<String, Object>> packing = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("packing"))) {
            idx++;
            if (toInt(r.get("PackingItemId")) <= 0) continue;
            if (toInt(r.get("BrandId")) == 0 && toInt(r.get("BrandUomId")) != 0) throw new IllegalArgumentException("OutPut Item Field Required in PM Detail Row No: " + idx);
            if (toInt(r.get("BrandUomId")) == 0 && toInt(r.get("BrandId")) != 0) throw new IllegalArgumentException("OutPut UomField Required in PM Detail Row No: " + idx);
            if (toDouble(r.get("ItemQty")) == 0d) throw new IllegalArgumentException("ItemQty Required in PM Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("ItemRate")) == 0d && toDouble(r.get("ItemQty")) != 0d && canRateAndAmount)
                throw new IllegalArgumentException("ItemRate Required in PM Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("Amount")) == 0d) throw new IllegalArgumentException("Amount Required in PM Detail Row No: " + idx);
            int id = recId != 0 ? toInt(r.get("Id")) : 0;
            packing.add(packingRow(r, id, id <= 0 ? 1 : 2));
        }
        if (recId > 0) for (Map<String, Object> r : list(b.get("removedPacking"))) packing.add(packingRow(r, toInt(r.get("Id")), 3));   // :1938

        /* ---- OverHead rows :1945 — only rows with an amount and an account ---- */
        List<Map<String, Object>> overheads = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("overheads"))) {
            idx++;
            if (!(toDouble(r.get("Amount")) > 0d && toInt(r.get("ChartOfAccountId")) > 0)) continue;
            if (toInt(r.get("BrandId")) == 0 && toInt(r.get("BrandUomId")) != 0) throw new IllegalArgumentException("OutPut Item Field Required in OH Detail Row No: " + idx);
            if (toInt(r.get("BrandUomId")) == 0 && toInt(r.get("BrandId")) != 0) throw new IllegalArgumentException("OutPut UomField Require in OH Detail Row No: " + idx);
            if (!withoutRates && toDouble(r.get("Amount")) == 0d) throw new IllegalArgumentException("Amount Required in OH Detail Row No: " + idx);
            int id = recId != 0 ? toInt(r.get("Id")) : 0;
            overheads.add(overheadRow(r, id, id <= 0 ? 1 : 2));
        }
        if (recId > 0) for (Map<String, Object> r : list(b.get("removedOverheads"))) overheads.add(overheadRow(r, toInt(r.get("Id")), 3));   // :1986

        /* ---- Plant rows :1993 ---- */
        List<Map<String, Object>> plantRows = list(b.get("plants"));
        if (plantRows.isEmpty()) throw new IllegalArgumentException("At Least One Detail Record Is Required In Plant Detail Grid");
        List<Map<String, Object>> plants = new ArrayList<>();
        for (Map<String, Object> r : plantRows) {
            Map<String, Object> d = new LinkedHashMap<>();                       // model 0461 order
            d.put("Id", recId != 0 ? toInt(r.get("Id")) : 0);
            d.put("InvProductionJobOrderId", 0);
            d.put("PlantId", toInt(r.get("PlantId")));
            d.put("AssumedPlantCapacity", toDouble(r.get("AssumedPlantCapacity")));
            d.put("RemarksDetail", str(r.get("RemarksDetail")));
            plants.add(d);
        }

        /* ---- Lab Standard rows :2007 ---- */
        Map<String, Object> lab = map(b.get("lab"));
        List<Map<String, Object>> labRows = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(lab.get("rows"))) {
            idx++;
            int id = recId != 0 ? toInt(r.get("Id")) : 0;
            if (toInt(r.get("AnalysisItemId")) == 0) throw new IllegalArgumentException("Analysis Item Field Required in Lab Standard Row No: " + idx);
            if (toDouble(r.get("MaxValue")) == 0d) throw new IllegalArgumentException("MaxValue Field Required in Lab Standard Row No: " + idx);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("InvProductionJobOrderLabStandardDetailId", id);
            d.put("InvProductionJobOrderId", 0);
            d.put("LabStandardMainId", toInt(lab.get("LabStandardMainId")));
            d.put("SupplierCustomerId", toInt(lab.get("SupplierCustomerId")));
            d.put("BrandItemId", toInt(lab.get("BrandItemId")));
            d.put("LabAnalysisItemId", toInt(r.get("AnalysisItemId")));
            d.put("MinValue", toDouble(r.get("MinValue")));
            d.put("MaxValue", toDouble(r.get("MaxValue")));
            d.put("Active", Boolean.TRUE);                                        // :2056
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            labRows.add(d);
        }

        /* ---- DAL 0273 SetData — one transaction ---- */
        int num = repo.saveHeader(recId == 0, head);
        if (num <= 0) num = recId;
        if (num <= 0) throw new RuntimeException("Job Order save returned no Id");
        for (Map<String, Object> d : inputs)    { d.put("InvProductionJobOrderId", num); repo.insertInput(d); }
        for (Map<String, Object> d : outputs)   { d.put("InvProductionJobOrderId", num); repo.insertOutput(d); }
        for (Map<String, Object> d : plants)    { d.put("InvProductionJobOrderId", num); repo.insertPlant(d); }
        for (Map<String, Object> d : overheads) { d.put("InvProductionJobOrderId", num); repo.insertOverhead(d); }
        for (Map<String, Object> d : packing)   { d.put("InvProductionJobOrderId", num); repo.insertPacking(d); }
        for (Map<String, Object> d : labRows)   { d.put("InvProductionJobOrderId", num); repo.insertLab(d); }
        if (recId == 0 && db.configFlag(u, "AutoJobLotCreateOnJobOrder")) insertAutoJobLot(u, head, num);        // DAL 0273:138

        int planCode = toInt(head.get("PlanCode"));
        return ok((recId == 0 ? "Save Successfully" : "Update Successfully") + planCode, num, planCode);           // :2024 / :2028
    }

    /** DAL 0273:138-157 — Proc_JobLot_Insert with the DAL's own values. */
    private void insertAutoJobLot(UserAccount u, Map<String, Object> head, int jobOrderId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved", Boolean.FALSE);
        p.put("PostState", Boolean.FALSE);
        p.put("IsCompany", Boolean.FALSE);
        p.put("IsThirdParty", Boolean.FALSE);
        p.put("EndDate", head.get("EndDate"));
        p.put("EntryDate", Prod273Support.now());
        p.put("ModifyDate", Prod273Support.now());
        p.put("StartDate", head.get("StartDate"));
        p.put("CompanyId", u.getCompanyId());
        p.put("EntryUser", u.getId());
        p.put("Id", 0);
        p.put("ModifyUser", u.getId());
        p.put("OrganizationId", u.getOrganizationId());
        p.put("PostUser", 0);
        p.put("JobLotCode", head.get("RefInvoiceNo"));
        p.put("JobLotDescription", head.get("RefInvoiceNo"));
        p.put("JobStatus", "InComplete");
        p.put("JobTypeId", 22);
        p.put("AccountId", 0);
        p.put("RefDocumentTypeId", DOC_TYPE);
        p.put("RefDocNoId", jobOrderId);
        repo.insertJobLot(p);
    }

    /** InvProductionJobOrderInput model (0480) order — Insert():1857 / DeleteInputdetailRow:2733. */
    private static Map<String, Object> inputRow(Map<String, Object> r, int id, int actionTypeId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", id);
        d.put("InvProductionJobOrderId", 0);
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("CropYearId", toInt(r.get("CropYearId")));
        d.put("BatchCrop", str(r.get("CropYear")));
        d.put("JobLotId", toInt(r.get("JobLotId")));
        d.put("Qty", toDouble(r.get("Qty")));
        d.put("UomSchIdQty", toInt(r.get("PackSizeId")));
        d.put("NetWeight", toDouble(r.get("Weight")));
        d.put("WarehouseId", toInt(r.get("WareHouseId")));
        d.put("PackingTypeId", toInt(r.get("PackTypeId")));
        d.put("RemarksInputDt", str(r.get("Remarks")));
        d.put("ItemRate", toDouble(r.get("ItemRate")));
        d.put("ItemAmount", toDouble(r.get("ItemAmount")));
        d.put("WagesRate", 0d);
        d.put("WagesAmount", 0d);
        d.put("RateUomId", toInt(r.get("RateUomId")));
        d.put("WagesAccountId", 0);
        d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
        d.put("RefDocIdNo", toInt(r.get("RefDocIdNo")));
        d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
        Timestamp docDate = Prod273Support.ts(r.get("DocDate"));
        d.put("DocDate", docDate == null ? Prod273Support.now() : docDate);
        d.put("ActionTypeId", actionTypeId);
        d.put("DocNo", toInt(r.get("DocNo")));
        return d;
    }

    /** OutputDetailRowDelete:3550 — the removed row with ActionTypeId 3 (tenancy checked). */
    private Map<String, Object> removedOutput(UserAccount u, Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", toInt(r.get("Id")));
        d.put("InvProductionJobOrderId", 0);
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("InvPackingTypeId", toInt(r.get("PackingTypeId")));
        d.put("QtyInner", toDouble(r.get("QtyOuter")));
        d.put("UomSchIdInner", toInt(r.get("OuterPackSizeId")));
        d.put("QtyOuter", toDouble(r.get("QtyOuter")));
        d.put("UomSchIdOuter", toInt(r.get("OuterPackSizeId")));
        d.put("NetWeight", toDouble(r.get("Weight")));
        d.put("PackRemarks", str(r.get("Remarks")));
        d.put("CropYearId", toInt(r.get("CropYearId")));
        d.put("WarehouseId", toInt(r.get("WareHouseId")));
        d.put("JobLotId", toInt(r.get("JobLotId")));
        d.put("ItemRate", toDouble(r.get("ItemRate")));
        d.put("ItemAmount", toDouble(r.get("ItemAmount")));
        d.put("WagesRate", 0d);
        d.put("WagesAmount", 0d);
        d.put("PMRate", 0d);
        d.put("PMAmount", 0d);
        d.put("OHExpense", toDouble(r.get("OHExpense")));
        d.put("PMExpense", toDouble(r.get("PMExpense")));
        d.put("OHGeneralExpense", toDouble(r.get("OHGeneralExpense")));
        d.put("PMGeneralExpense", toDouble(r.get("PMGeneralExpense")));
        d.put("NetRate", toDouble(r.get("NetRate")));
        d.put("NetAmount", toDouble(r.get("NetAmount")));
        d.put("RateUomId", toInt(r.get("RateUomId")));
        d.put("WagesAccountId", 0);
        d.put("PMItemId", 0);
        d.put("EntryTypeId", toInt(r.get("EntryTypeId")));
        d.put("ActionTypeId", 3);
        return d;
    }

    /** InvProductionJobOrderPackingMaterial — Insert():1924 / PackingMaterialDetailrowDelete:4418. */
    private static Map<String, Object> packingRow(Map<String, Object> r, int id, int actionTypeId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", id);
        d.put("InvProductionJobOrderId", 0);
        d.put("ItemId", toInt(r.get("PackingItemId")));
        d.put("ItemUomSchId", 0);
        d.put("BrandId", toInt(r.get("BrandId")));
        d.put("BrandUomId", toInt(r.get("BrandUomId")));
        d.put("Qty", toDouble(r.get("ItemQty")));
        d.put("Rate", toDouble(r.get("ItemRate")));
        d.put("Amount", toDouble(r.get("Amount")));
        d.put("pmRemarks", str(r.get("Remarks")));
        d.put("WarehouseId", toInt(r.get("WarehouseId")));
        d.put("ActionTypeId", actionTypeId);
        return d;
    }

    /** InvProductionJobOrderOverHeads — Insert():1973 / OverHeadDetailRowDelete:4012. */
    private static Map<String, Object> overheadRow(Map<String, Object> r, int id, int actionTypeId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", id);
        d.put("InvProductionJobOrderId", 0);
        d.put("CharOfAccountId", toInt(r.get("ChartOfAccountId")));
        d.put("BrandId", toInt(r.get("BrandId")));
        d.put("BrandUomId", toInt(r.get("BrandUomId")));
        d.put("Amount", toDouble(r.get("Amount")));
        d.put("ohRemarks", str(r.get("Remarks")));
        d.put("ItemQty", toDouble(r.get("ItemQty")));
        d.put("ItemRate", toDouble(r.get("ItemRate")));
        d.put("ActionTypeId", actionTypeId);
        return d;
    }

    // ============================================================================ helpers

    /** The header must exist, be DocumentTypeId 401 and belong to the signed-in company (DEVIATION 1). */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOC_TYPE) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }
}
