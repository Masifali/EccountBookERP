package com.mst.repositories;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 280, tab "PackingMaterial" - Architecture.WinApp.Production.frmProductionPackingMaterial
 * (src280/frmProductionPackingMaterial.cs, 4,213 lines). DocumentTypeId 111.
 *
 * Every procedure below is called with the parameters the desktop BLL sends, under the same
 * guards (a BLL that adds a parameter only when it is non-zero is reproduced by omitting it).
 * Each one was checked against the procedure's declaration in procdure.utf8.sql.
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE = DAL Architecture.DAL.Production.InvFoodProductionPackingMaterial::SetData
 * ---------------------------------------------------------------------------------------------
 * Read in IL (extracted_disasm/Architecture.dal/Architecture.DAL.Production.txt :5525-6806).
 * One SqlTransaction; for EVERY grid row, in order:
 *
 *   1. Sp_InvFoodProductionPackingMaterial_Insert (Id = 0) or _Update, through GenericProvider
 *      .SetProc, so the parameters are the model's 29 non-virtual properties. Insert returns the
 *      new Id -> ModifyUser := 0, Id := new id. Update returns nothing -> EntryUser := 0 and the
 *      row's own Id is kept. (Both assignments are the DAL's; they feed the voucher below.)
 *   2. CommonServices.GetItemGlIdsandItemName  -> Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'
 *   3. Sp_InvProductionJobOrder_GetAllMethod 'GetGlAccountsByJobOrderId' -> WorkInProccessAcId,
 *      else "Work in process Account not found against JobOrder".
 *   4. The FIFO block (FIFOImplemention, USP_InventoryQtyReverseAndDeleteByReferenceId,
 *      USP_InventoryStockEvalautionDetail_Insert) is guarded by a local that the DAL sets to 0
 *      and never changes (IL_0052 ldc.i4.0 / stloc.s 8). It is dead code and is NOT ported.
 *   5. Sp_InventoryStockEvalautionDetail_Update  (SetProc, Org/Company/RefDocumentTypeId/RefDocIdNo)
 *   6. Sp_InventoryTransactions_GetALLMethod     (SetProc, same four values)
 *   7. USP_InventoryValidation @Activity='PM' - once for EVERY row of the list, inside the loop
 *      over the rows (so an N-row save validates N x N times, exactly as the desktop does).
 *   8. When the row id > 0 and config InventoryFinancialsEffectsInActive is off: the voucher.
 *      If ModifyUser > 0 the existing voucher is looked up (Sp_Vouchers_GetMethods
 *      'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'); two lines are ADDED to the
 *      voucher's detail list (Dr WIP / Cr item stock); Sp_VoucherHead_Insert or _Update;
 *      Sp_VoucherDetail_Insert for every line in the list; USP_VoucherBalanceCheck.
 * After the loop: USP_InvFoodProductionPmRowsDeleteByIds when PmDetailRowsRemoveIds is set (the
 * form never sets it, so it never runs from this screen - reproduced as "not sent").
 *
 * DESKTOP QUIRK REPRODUCED: the VoucherHead object and its voucherDetailList are created ONCE,
 * before the loop, and the list is never cleared. Saving N new rows therefore creates N vouchers
 * and the k-th voucher carries the lines of rows 1..k (2k lines). Each voucher still balances.
 *
 * The SetProc models (InventoryStockEvalautionDetail, InventoryTransactions, VoucherHead,
 * VoucherDetail) are sent with the same CLR-default key sets already verified for
 * InventoryOpeningWriter (every key checked here again against the four procedures: 0 missing).
 */
@Repository
public class ProductionPackingMaterialRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionPackingMaterialRepository.class);

    /** PackingMaterialDetail.DocumentTypeId = 111 (form :1139). */
    public static final int DOC_TYPE_ID = 111;

    private static final String P_PM = "Sp_InvFoodProductionPackingMaterial_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ProductionPackingMaterialRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================== plumbing

    private static Map<String, Object> params() { return new LinkedHashMap<>(); }

    /** EXEC dbo.proc @A=?, @B=? ... in insertion order; returns the first result set. */
    public List<Map<String, Object>> exec(String proc, Map<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(qualify(proc));
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append(at(e.getKey())).append("=?");
            values.add(e.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    /**
     * ExecuteScalar: first column of the first result set, or null when the procedure returns
     * none. A procedure here may or may not produce a result set (the Update procedures do not),
     * so neither queryForList nor update() is safe on its own.
     */
    public Object scalar(String proc, Map<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(qualify(proc));
        boolean first = true;
        for (String k : p.keySet()) {
            sql.append(first ? " " : ", ").append(at(k)).append("=?");
            first = false;
        }
        final List<Object> values = new ArrayList<>(p.values());
        return jdbc.execute(sql.toString(), (PreparedStatementCallback<Object>) ps -> {
            for (int i = 0; i < values.size(); i++) ps.setObject(i + 1, values.get(i));
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        return rs.next() ? rs.getObject(1) : null;
                    }
                }
                if (ps.getUpdateCount() == -1) return null;
                isRs = ps.getMoreResults();
            }
        });
    }

    private static String at(String k) { return k.startsWith("@") ? k : "@" + k; }

    private static String qualify(String proc) {
        return proc.startsWith("[") || proc.contains(".") ? proc : "dbo." + proc;
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    // ================================================================================= reads

    /** GenerateCode (BLL): @OrganizationId, @CompanyId, @FinancialYearId, @BranchesId (model's
     *  BranchId), @Activity='ReadSerialNumber' - all unconditional. Column DocNo. */
    public int generateCode(int org, int comp, int fy, int branch) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@FinancialYearId", fy);
        p.put("@BranchesId", branch);
        p.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> rows = exec(P_PM, p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocNo"));
    }

    /** GetByID (BLL): @Id, @Activity='ReadById'. */
    public Map<String, Object> readById(int id) {
        Map<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> rows = exec(P_PM, p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /**
     * GetAll (BLL, 'ReadAll'). Guards as in the IL: FinancialYearId, BranchesId, ActionId,
     * JobOrderId, PlantId, ItemId when non-zero; every date when not CheckDateTimeNull; DocNoFrom /
     * DocNoTo when != 0. The approved pair travels as @ApprovedFromDate / @ApprovedToDate, names
     * the procedure does NOT declare - the desktop fails there too, and so does this call.
     */
    public List<Map<String, Object>> history(int org, int comp, int fy, int branch, int actionId,
                                             Timestamp fromDate, Timestamp toDate,
                                             Timestamp entryFrom, Timestamp entryTo,
                                             Timestamp modifyFrom, Timestamp modifyTo,
                                             Timestamp approvedFrom, Timestamp approvedTo,
                                             double docNoFrom, double docNoTo, int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (fy != 0) p.put("@FinancialYearId", fy);
        if (branch != 0) p.put("@BranchesId", branch);
        if (actionId != 0) p.put("@ActionId", actionId);
        if (fromDate != null) p.put("@FromDate", fromDate);
        if (toDate != null) p.put("@ToDate", toDate);
        if (entryFrom != null) p.put("@EntryFromDate", entryFrom);
        if (entryTo != null) p.put("@EntryToDate", entryTo);
        if (modifyFrom != null) p.put("@ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("@ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("@ApprovedFromDate", approvedFrom);
        if (approvedTo != null) p.put("@ApprovedToDate", approvedTo);
        if (docNoFrom != 0.0) p.put("@DocNoFrom", docNoFrom);
        if (docNoTo != 0.0) p.put("@DocNoTo", docNoTo);
        if (jobOrderId != 0) p.put("@JobOrderId", jobOrderId);
        p.put("@Activity", "ReadAll");
        return exec(P_PM, p);
    }

    /** GetDataForDropDownFromInvFoodProductionPackingMaterial (BLL): Org, Company, FY when != 0,
     *  @DocumentTypeIds when non-empty (not set here), @Activity when non-empty. */
    public List<Map<String, Object>> historyDropDown(int org, int comp, int fy, String activity) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (fy != 0) p.put("@FinancialYearId", fy);
        if (activity != null && !activity.isEmpty()) p.put("@Activity", activity);
        return exec("USP_GetDataForDropDownFromInvFoodProductionPackingMaterial", p);
    }

    /** InvProductionJobOrder.JobOrdersForProduction - usp_getJobOrdersForProduction, three
     *  unconditional parameters (BLL 0335:1582). Columns Id, PlanCode, DocumentTypeId. */
    public List<Map<String, Object>> jobOrders(int org, int comp, int branch) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@BranchesId", branch);
        return exec("usp_getJobOrdersForProduction", p);
    }

    /** InvProductionJobOrder.PlantsForProductionByJobOrderId (BLL 0335:1654). */
    public List<Map<String, Object>> plants(int org, int comp, int fy, int branch, int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@FinancialYearId", fy);
        p.put("@BranchesId", branch);
        p.put("@JobOrderId", jobOrderId);
        return exec("usp_getPlantsForProductionByJobOrderId", p);
    }

    /** InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId (BLL): @InvJobOrderId always;
     *  @ItemId, @PlantId, @ItemUomId when non-zero; @EntryType when Activity is non-empty (the
     *  form leaves Activity unset); @Activity literal. */
    public List<Map<String, Object>> totals(int org, int comp, int jobOrderId, int plantId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@InvJobOrderId", jobOrderId);
        if (plantId != 0) p.put("@PlantId", plantId);
        p.put("@Activity", "GetInPutTotalQtyandWeightByJobOrderId");
        return exec("Sp_InvFoodProduction_GetAllMethod", p);
    }

    /** InvFoodProduction.GetBrandItemsAndUomForPackMaterialAgainstPmItem (BLL): Org, Company,
     *  @InvProductionJobOrderId, @ItemId always; @PMDocId only when Id != 0. */
    public List<Map<String, Object>> brandItems(int org, int comp, int jobOrderId, int itemId, int pmDocId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@InvProductionJobOrderId", jobOrderId);
        p.put("@ItemId", itemId);
        if (pmDocId != 0) p.put("@PMDocId", pmDocId);
        return exec("USP_GetBrandItemsForPackMaterialAgainstPmItem", p);
    }

    /** ExportContractSchedule.ExportContractSchedule_GetPending (BLL): Org, Company; @ActionId and
     *  @RecId only when non-zero. The form sets RecId = RecpackingMaterial and never ActionId. */
    public List<Map<String, Object>> pendingSchedules(int org, int comp, int recId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (recId != 0) p.put("@RecId", recId);
        return exec("[dbo].[usp_ExportContractSchedule_GetPending]", p);
    }

    /** GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand (BLL): Org, Company, @ItemId,
     *  @DocDate always; DocumentTypeId, RecId, ItemConditionId, WarehouseId, RackId (StoreRackId)
     *  only when non-zero. This form never passes a warehouse or rack. */
    public List<Map<String, Object>> avgRate(int org, int comp, int itemId, Timestamp docDate,
                                             int itemConditionId, int recId, int documentTypeId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("@RecId", recId);
        if (itemConditionId != 0) p.put("@ItemConditionId", itemConditionId);
        p.put("@Activity", "GetAvgRateQtyAndStockInHand");
        return exec("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
    }

    /** clsGlobalVariables.getGlobalAllItems - GlobalServicesMethods.AllItemsWithModal:
     *  [dbo].[USP_Item_AllItemsWithModal] @OrganizationId, @CompanyId (paging/keyword unset). */
    public List<Map<String, Object>> allItems(int org, int comp) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        return exec("[dbo].[USP_Item_AllItemsWithModal]", p);
    }

    /** clsGlobalVariables.racksWithWarehouseAndItems - GlobalServicesMethods
     *  .GetRacksWithWarehouseByItemId: usp_getRackswithWarehouseByItemId @OrganizationId,
     *  @CompanyId, @BranchId (no @ItemId: the global list is every item's racks). */
    public List<Map<String, Object>> racksWithWarehouseAndItems(int org, int comp, int branch) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@BranchId", branch);
        return exec("usp_getRackswithWarehouseByItemId", p);
    }

    /** clsGlobalVariables.globalItemConditions - GlobalServicesMethods.GetItemCondtions ->
     *  PackingMaterialItemsAllocateToTransactionFlow.ItemCondition: the desktop's own literal
     *  "SELECT * FROM dbo.V_ItemCondition" (columns Id, ConditionStatus). */
    public List<Map<String, Object>> itemConditions() {
        return jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition");
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(name) - the ConfigKey of one row of the
     *  configuration allocation, or null when the company has no such row. */
    public String config(int org, int comp, String name) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ConfigDescription", name);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> rows = exec("Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (rows.isEmpty()) return null;
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    /** CommonServices.VoucherHeadIdGet(Id, 111) -> VoucherHead.GetVoucherHeadIdByDocumentTypeIdand
     *  RefDocNoId: Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    public int voucherHeadId(int org, int comp, int documentTypeId, int id) {
        Map<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", id);
        return toInt(scalar("Sp_Vouchers_GetMethods", p));
    }

    /** InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder (BLL): Org,
     *  Company, @ReportType always; @JobOrderId and @Id only when non-zero. Used only to answer
     *  "Record Not Found For DisPlay" before the viewer opens, as the form does. */
    public List<Map<String, Object>> report606(int org, int comp, int jobOrderId, String reportType) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ReportType", reportType);
        if (jobOrderId != 0) p.put("@JobOrderId", jobOrderId);
        return exec("Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder", p);
    }

    // ================================================================================ write

    /** One grid row, already shaped as the desktop's InvFoodProductionPackingMaterial model. */
    public static class PmRow {
        public int id;
        public LocalDateTime docDate;
        public int docNo;
        public int invProductionJobOrderId;
        public int itemId;
        public int brandId;
        public int brandUomId;
        public double qty;
        public double rate;
        public double amount;
        public String pmRemarks;
        public int documentTypeId;
        public int organizationId;
        public int companyId;
        public int entryUser;
        public LocalDateTime entryDate;
        public int modifyUser;
        public LocalDateTime modifyDate;
        public int financialYearId;
        public int warehouseId;
        public int plantId;
        public int branchesId;
        public int itemConditionId;
        public int rackId;
        public int contractScheduleId;
        public int exImInvoiceId;
        public int baseDocumentTypeId;
    }

    private static Timestamp ts(LocalDateTime d) { return d == null ? null : Timestamp.valueOf(d); }

    /**
     * GenericProvider.SetProc over InvFoodProductionPackingMaterial: the 29 non-virtual
     * properties, all declared by both procedures. itemUomSchId and JobOrderPackingMaterialId are
     * never assigned by the form, so they go as the CLR default 0. BranchSrNo, UserLogId and
     * AutoUpdate are not model properties and are left to their procedure defaults.
     */
    private static Map<String, Object> pmParams(PmRow r) {
        Map<String, Object> p = params();
        p.put("@Id", r.id);
        p.put("@DocDate", ts(r.docDate));
        p.put("@DocNo", r.docNo);
        p.put("@InvProductionJobOrderId", r.invProductionJobOrderId);
        p.put("@ItemId", r.itemId);
        p.put("@itemUomSchId", 0);
        p.put("@BrandId", r.brandId);
        p.put("@BrandUomId", r.brandUomId);
        p.put("@Qty", r.qty);
        p.put("@Rate", r.rate);
        p.put("@Amount", r.amount);
        p.put("@pmRemarks", r.pmRemarks);
        p.put("@DocumentTypeId", r.documentTypeId);
        p.put("@OrganizationId", r.organizationId);
        p.put("@CompanyId", r.companyId);
        p.put("@EntryUser", r.entryUser);
        p.put("@EntryDate", ts(r.entryDate));
        p.put("@ModifyUser", r.modifyUser);
        p.put("@ModifyDate", ts(r.modifyDate));
        p.put("@FinancialYearId", r.financialYearId);
        p.put("@WarehouseId", r.warehouseId);
        p.put("@PlantId", r.plantId);
        p.put("@BranchesId", r.branchesId);
        p.put("@JobOrderPackingMaterialId", 0);
        p.put("@ItemConditionId", r.itemConditionId);
        p.put("@ContractScheduleId", r.contractScheduleId);
        p.put("@ExImInvoiceId", r.exImInvoiceId);
        p.put("@BaseDocumentTypeId", r.baseDocumentTypeId);
        p.put("@RackId", r.rackId);
        return p;
    }

    private static final String[] EVALUATION_INT = {
            "AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate", "ExpenseAmountIn",
            "ItemRate", "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut", "CgsAmount", "BranchesId", "CompanyId",
            "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId", "ItemId", "ItemUom", "JobLotId", "OrderNo",
            "OrganizationId", "PrdJobOrderNo", "ProjectsId", "RateUom", "RefDocIdNo", "RefDocSubIdNo",
            "RefDocumentTypeId", "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId", "SupplierCustomerId",
            "WarehouseId", "RefWarehouseId", "CityId", "LineId", "RefRefDocumentTypeId", "RefRefDocIdNo",
            "RefRefDocSubIdNo", "EntryUser", "ModifyUser", "InvoiceId", "InvoiceDetailId", "VarientId", "ItemConditionId" };

    private static final String[] TRANSACTIONS_INT = {
            "AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "ExpenseAmountIn", "ItemRate",
            "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut", "BranchesId", "CompanyId", "DocCodeNo",
            "InvPackingTypeId", "ItemId", "ItemUom", "JobLotId", "OrganizationId", "ProjectsId", "RateUom", "RefDocIdNo",
            "RefDocSubIdNo", "RefDocumentTypeId", "SupplierCustomerId", "WarehouseId", "Id" };

    private static final String[] VOUCHER_BOOL = { "IncludeWHT", "IsApproved", "PostState", "CustomAccounts", "IsUploaded" };
    private static final String[] VOUCHER_INT = {
            "BillAmount", "ExchangeCurrencyRate", "FcAmount", "VoucherAmount",
            "CostCenterAmount", "AgainstAccountId", "BranchId", "CheqId", "ChequePrintId", "CompanyId", "DocumentTypeId",
            "DocumentTypeSrNo", "DueDays", "EntryUser", "FinancialYearId", "Id", "ModifyUser", "MultiCurrencyId",
            "OrganizationId", "PostUser", "ProjectId", "RefAccountId", "RefDocNoId", "VoucherCode", "ActionId",
            "RefDocumentTypeId", "FixedAssetEntryTypeId", "BaseDocumentTypeId", "AdvanceTaxAccountId",
            "AdvanceTaxAmount", "OtherChargesAccountId", "OtherChargesAmount" };

    private static final String[] DETAIL_INT = {
            "Adjustment", "AdvanceAmount", "Commission", "CreditAmount", "DCurrencyAmount", "DebitAmount", "TaxAmount",
            "DExchangeCurrencyRate", "Expenses", "ExTax", "Freight", "ItemAmount", "ItemRate", "Journal", "QtyIn",
            "QtyOut", "RateCut", "RateCutAmount", "SaleTax", "TaxesTotalAmount", "TaxPrcnt", "WeightIn", "WeightOut",
            "WhtHolding", "ItemCgsRate", "TotalDebitAmount", "TotalCreditAmount", "ThirdCurrencyFcyExchangeRate",
            "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount", "ThirdCurrencyReceiverExchangeRate",
            "ThirdCurrencyReceiverFcyAmount", "ThirdCurrencyId", "AccountId", "AgainstAccountId", "DMultiCurrencyId",
            "DocumentTypeIdRef", "GpNo", "Id", "InvoiceNoRefId", "ItemId", "JobLotId", "OrderNo", "SupplierCustomerId",
            "EmployeeId", "SubsidiaryTypeId", "SubsidiaryAccountId", "SubsidiaryAgainstTypeId",
            "SubsidiaryAgainstAccountId", "TaxTypeId", "ActionId", "VoucherHeadId", "RefDocumentTypeId", "RefDocNoId",
            "RefDocNoDetailId", "RefDocSubIdNo", "LineId", "InstrumentTypeId", "SubNo", "SortNo", "IsCGS",
            "PaymentTypeId", "ChequeTypeId", "BranchesId", "CostCenterId", "SBRTaxAmount", "DiscountPercent",
            "DiscountAmount", "ReferenceAccountId", "LocationTypeId", "BaseFcyId", "BaseFcyExchangeRate", "BaseFcyAmount" };

    private static Map<String, Object> defaults(String[] ints, String... bools) {
        Map<String, Object> m = params();
        for (String b : bools) m.put(b, Boolean.FALSE);
        for (String k : ints) m.put(k, 0);
        return m;
    }

    private static Map<String, Object> evaluationDefaults() {
        Map<String, Object> m = params();
        m.put("IsApproved", Boolean.FALSE);
        for (String k : EVALUATION_INT) m.put(k, 0);
        return m;
    }

    private static Map<String, Object> transactionDefaults() {
        Map<String, Object> m = params();
        m.put("IsApproved", Boolean.FALSE);
        for (String k : TRANSACTIONS_INT) m.put(k, 0);
        return m;
    }

    /**
     * DAL SetData, ported step for step (see the class comment). Everything runs on one
     * connection in one transaction; any RAISERROR rolls the whole save back, as the desktop's
     * SqlTransaction does.
     *
     * @param rows          invFoodProductionPackingMaterialList, in grid order
     * @param finEffectsOff config InventoryFinancialsEffectsInActive (read BEFORE the loop, :0041)
     */
    @Transactional(rollbackFor = Exception.class)
    public void save(List<PmRow> rows, boolean finEffectsOff) {
        /* loc3: ONE VoucherHead for the whole call, and one detail list that is never cleared. */
        Map<String, Object> voucher = defaults(VOUCHER_INT, VOUCHER_BOOL);
        List<Map<String, Object>> voucherDetails = new ArrayList<>();

        for (PmRow pm : rows) {
            int loc0, loc1 = 0, loc2;

            /* 1. header row */
            String proc = pm.id != 0 ? "Sp_InvFoodProductionPackingMaterial_Update"
                                     : "Sp_InvFoodProductionPackingMaterial_Insert";
            loc0 = toInt(scalar(proc, pmParams(pm)));
            if (loc0 > 0) {
                pm.modifyUser = 0;
                pm.id = loc0;
            } else {
                pm.entryUser = 0;
                loc0 = pm.id;
            }

            /* 2. CommonServices.GetItemGlIdsandItemName(Org, Company) */
            Map<String, Object> gp = params();
            gp.put("@OrganizationId", pm.organizationId);
            gp.put("@CompanyId", pm.companyId);
            gp.put("@Activity", "GetItemGlIdsandItemName");
            List<Map<String, Object>> itemGl = exec("Sp_Item_GetAllMethod", gp);

            /* 3. WIP account of the job order */
            Map<String, Object> jp = params();
            jp.put("@OrganizationId", pm.organizationId);
            jp.put("@CompanyId", pm.companyId);
            jp.put("@Id", pm.invProductionJobOrderId);
            jp.put("@Activity", "GetGlAccountsByJobOrderId");
            List<Map<String, Object>> wip = exec("Sp_InvProductionJobOrder_GetAllMethod", jp);
            if (wip == null || wip.isEmpty()) {
                throw new IllegalStateException("Work in process Account not found against JobOrder");
            }
            int wipAccountId = toInt(ci(wip.get(0), "WorkInProccessAcId"));

            /* 4. FIFO block: dead (loc8 is the constant 0). */

            /* 5. stock evaluation */
            Map<String, Object> ev = evaluationDefaults();
            ev.put("OrganizationId", pm.organizationId);
            ev.put("CompanyId", pm.companyId);
            ev.put("RefDocumentTypeId", pm.documentTypeId);
            ev.put("RefDocIdNo", loc0);
            scalar("Sp_InventoryStockEvalautionDetail_Update", ev);

            /* 6. stock movements - the procedure rebuilds them from the saved row */
            Map<String, Object> tr = transactionDefaults();
            tr.put("OrganizationId", pm.organizationId);
            tr.put("CompanyId", pm.companyId);
            tr.put("RefDocumentTypeId", pm.documentTypeId);
            tr.put("RefDocIdNo", loc0);
            scalar("Sp_InventoryTransactions_GetALLMethod", tr);

            /* 7. USP_InventoryValidation for every row of the list, @Activity='PM' */
            for (PmRow v : rows) {
                Map<String, Object> vp = params();
                vp.put("@OrganizationId", v.organizationId);
                vp.put("@CompanyId", v.companyId);
                vp.put("@DocumentTypeId", v.documentTypeId);
                vp.put("@DocDate", ts(v.docDate));
                vp.put("@ItemId", v.itemId);
                vp.put("@WarehouseId", v.warehouseId);
                vp.put("@NetWeight", v.qty);
                vp.put("@ItemConditionId", v.itemConditionId);
                vp.put("@RackId", v.rackId);
                vp.put("@Activity", "PM");
                scalar("USP_InventoryValidation", vp);
            }

            /* 8. voucher */
            if (loc0 <= 0 || finEffectsOff) continue;

            if (pm.modifyUser > 0) {
                loc1 = voucherHeadId(pm.organizationId, pm.companyId, pm.documentTypeId, pm.id);
                voucher.put("Id", loc1);
            }
            voucher.put("DocumentTypeSrNo", loc0);
            voucher.put("VoucherDate", ts(pm.docDate));
            voucher.put("VoucherCode", pm.docNo);
            voucher.put("DocumentTypeId", pm.documentTypeId);
            voucher.put("BaseDocumentTypeId", pm.baseDocumentTypeId);
            voucher.put("RefAccountId", wipAccountId);
            voucher.put("AgainstAccountId", wipAccountId);
            voucher.put("EntryDate", Timestamp.valueOf(LocalDateTime.now()));
            voucher.put("ModifyDate", Timestamp.valueOf(LocalDateTime.now()));
            voucher.put("VoucherAmount", pm.amount);
            voucher.put("EntryUser", pm.entryUser);
            voucher.put("ModifyUser", pm.modifyUser);
            voucher.put("OrganizationId", pm.organizationId);
            voucher.put("CompanyId", pm.companyId);
            voucher.put("FinancialYearId", pm.financialYearId);
            voucher.put("BranchId", pm.branchesId);

            if (itemGl == null || itemGl.isEmpty()) {
                throw new IllegalStateException("Item list Not Found");
            }
            Map<String, Object> gl = null;
            for (Map<String, Object> g : itemGl) {
                if (toInt(ci(g, "Id")) == pm.itemId) { gl = g; break; }
            }
            if (gl == null) {
                throw new IllegalStateException("Item GlAccountId Not Found");
            }
            int purchaseGlac = toInt(ci(gl, "PurchaseGLAC"));

            /* Dr WIP / Cr item stock */
            Map<String, Object> d1 = defaults(DETAIL_INT);
            d1.put("AccountId", wipAccountId);
            d1.put("AgainstAccountId", purchaseGlac);
            d1.put("Comments", pm.pmRemarks == null ? "" : pm.pmRemarks);
            d1.put("DebitAmount", pm.amount);
            d1.put("ItemId", pm.itemId);
            d1.put("ItemAmount", pm.amount);
            d1.put("OrderNo", pm.invProductionJobOrderId);
            d1.put("BranchesId", pm.branchesId);
            voucherDetails.add(d1);

            Map<String, Object> d2 = defaults(DETAIL_INT);
            d2.put("AccountId", purchaseGlac);
            d2.put("AgainstAccountId", wipAccountId);
            d2.put("Comments", pm.pmRemarks == null ? "" : pm.pmRemarks);
            d2.put("CreditAmount", pm.amount);
            d2.put("ItemId", pm.itemId);
            d2.put("ItemAmount", pm.amount);
            d2.put("QtyOut", pm.qty);
            d2.put("WeightOut", pm.qty);
            d2.put("OrderNo", pm.invProductionJobOrderId);
            d2.put("BranchesId", pm.branchesId);
            voucherDetails.add(d2);

            loc2 = toInt(scalar(loc1 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucher));
            if (loc2 > 0) voucher.put("Id", loc2);
            int headId = toInt(voucher.get("Id"));

            for (Map<String, Object> d : voucherDetails) {
                d.put("VoucherHeadId", headId);
                scalar("Sp_VoucherDetail_Insert", d);
            }

            if (headId > 0) {
                Map<String, Object> bp = params();
                bp.put("@OrganizationId", pm.organizationId);
                bp.put("@CompanyId", pm.companyId);
                bp.put("@Id", headId);
                scalar("USP_VoucherBalanceCheck", bp);
            }
        }
        /* PmDetailRowsRemoveIds is never set by this form -> USP_InvFoodProductionPmRowsDeleteByIds
           is not called. */
        LOG.debug("Packing material save posted {} row(s)", rows.size());
    }
}
