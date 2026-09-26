package com.mst.repositories;

import com.mst.models.UserAccount;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screen 280 "Production Against (Job Order)" - the INPUT tab.
 * Desktop form: Architecture.WinApp.Production/frmProductionInput.cs (5,790 lines), DocumentTypeId 80,
 * plus the three dialogs it opens: LoadavailableTransactionsForIssuance, LoadOutPutPendingforRates
 * (EntryType "Issue") and frmPendingMoveOrderDocuments.
 *
 * Every call below was traced three ways: the form line that builds the ReportsParameters/model,
 * the BLL IL that decides which of those values become parameters (and under which guard), and
 * the procedure declaration in procdure.utf8.sql. A parameter the BLL guards is OMITTED when it is
 * unset - never sent as NULL - because a procedure that tests "@X IS NULL" answers differently.
 *
 * WRITES reproduce Architecture.DAL.Production.InvFoodProduction.SetData for DocumentTypeId 80.
 * GenericProvider.SetProc sends one @Property per non-virtual model property; the parameter maps
 * below are therefore "the model's value-type defaults, overlaid with what the form/BLL set,
 * filtered to what the procedure declares". A null is not sent at all, which is what ADO.NET does
 * with AddWithValue(name, null) - the procedure's default applies.
 */
@Repository
public class ProductionInputRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionInputRepository.class);

    public static final int DOC_TYPE_INPUT = 80;

    private final JdbcTemplate jdbc;

    public ProductionInputRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =================================================================================== plumbing

    public static LinkedHashMap<String, Object> params() { return new LinkedHashMap<>(); }

    /** EXEC proc @A=?, @B=? ... and every row of the FIRST result set. Nulls are omitted. */
    public List<Map<String, Object>> query(String proc, Map<String, Object> p) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, p, false));
    }

    /** EXEC and the first column of the first row of the first result set (ExecuteScalar), or null. */
    public Object scalar(String proc, Map<String, Object> p) {
        List<Map<String, Object>> rows = jdbc.execute(
                (ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, p, true));
        if (rows == null || rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        return r.isEmpty() ? null : r.values().iterator().next();
    }

    private static List<Map<String, Object>> run(Connection con, String proc, Map<String, Object> p,
                                                 boolean firstRowOnly) throws java.sql.SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (e.getValue() == null) continue;           // AddWithValue(null) -> the proc default
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) {
                Object v = values.get(i);
                if (v instanceof LocalDateTime) v = Timestamp.valueOf((LocalDateTime) v);
                ps.setObject(i + 1, v);
            }
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(md.getColumnLabel(c), rs.getObject(c));
                            out.add(row);
                            if (firstRowOnly) break;
                        }
                    }
                    /* The desktop reads the first table only (GetDataTableProc / ExecuteScalar),
                       but the remaining results are drained so a RAISERROR raised after the first
                       SELECT still surfaces as an exception, as it does in ADO.NET. */
                    drain(ps);
                    return out;
                }
                if (ps.getUpdateCount() == -1) break;
                isRs = ps.getMoreResults();
            }
        }
        return out;
    }

    private static void drain(PreparedStatement ps) throws java.sql.SQLException {
        while (true) {
            boolean more = ps.getMoreResults();
            if (!more && ps.getUpdateCount() == -1) return;
        }
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private List<Map<String, Object>> safe(String label, String proc, Map<String, Object> p) {
        try {
            return query(proc, p);
        } catch (RuntimeException e) {
            LOG.warn("Production Input lookup '{}' failed ({})", label, proc, e);
            throw e;
        }
    }

    private static boolean nz(Integer v) { return v != null && v != 0; }
    private static boolean ne(String v) { return v != null && !v.isEmpty(); }

    // ============================================================ configuration, features, rights

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name) - one configuration row by description.
     * Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationByOrgCompandConfigDescription'
     * (the DAL's CommonServices.GetConfigurationFromAllocation uses the same activity inside Save).
     * Returns null when the company has no such row.
     */
    public String config(UserAccount u, String description) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ConfigDescription", description);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> rows = query("dbo.Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (rows.isEmpty()) return null;
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** CommonServices.GetERPFeatureById(id) / DAL GetERPFeaturesByCompanyId - USP_GetERPFeaturesByCompanyId. */
    public boolean erpFeature(UserAccount u, int featureId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        for (Map<String, Object> r : query("dbo.USP_GetERPFeaturesByCompanyId", p)) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /**
     * clsGlobalVariables.WagesRefDocumentsStatusList - USP_GetRefDocumentsForWages with
     * @RefDocumentTypeId omitted (the desktop passes 0 and the BLL guards it). Load:358 picks row 80.
     */
    public List<Map<String, Object>> wagesRefDocuments() {
        return query("[dbo].[USP_GetRefDocumentsForWages]", params());
    }

    /** tblUserRights.GetByUserId - Sp_tblUserRights_GetAllMethod @Activity='GetByUserId'. */
    public List<Map<String, Object>> userRights(int userId, String screenName, String roleName, int companyId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@UserId", userId);
        p.put("@ScreenName", screenName);
        p.put("@RightName", roleName == null ? "" : roleName);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "GetByUserId");
        return query("dbo.Sp_tblUserRights_GetAllMethod", p);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public Object financialYearStart(UserAccount u, int financialYearId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        List<Map<String, Object>> years = query("dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) { row = r; break; }
        }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row == null ? null : ci(row, "Start_Period");
    }

    // ================================================================================ Load pickers

    /**
     * GenerateDocNumberInput:1045 -> InvFoodProduction.GetSerialNumber (BLL :5199):
     * @OrganizationId, @CompanyId, @DocumentTypeId=80 always; @FinancialYearId and @BranchesId only
     * when non-zero; @Activity='ReadSerialNumber'. Returns DocCode of the first row, else 0.
     */
    public int serialNumber(UserAccount u, int financialYearId, int branchId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", DOC_TYPE_INPUT);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (branchId != 0) p.put("@BranchesId", branchId);
        p.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> rows = query("dbo.Sp_InvFoodProduction_GetAllMethod", p);
        if (rows.isEmpty()) return 0;
        return toInt(ci(rows.get(0), "DocCode"));
    }

    /** CommondtForCombosFillInput:500 -> JobLotsAllocationToBranch (@BranchId only when != 0). */
    public List<Map<String, Object>> jobLotsForBranch(UserAccount u, int branchId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("@BranchId", branchId);
        return safe("jobLots", "[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
    }

    /** CommondtForCombosFillInput:514 -> CommonServices.CropYearGetAllService -> InvCropYear.Getall. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadAll");
        return safe("cropYears", "dbo.Sp_InvCropYear_GetAllMethod", p);
    }

    /** bagType:595 -> InvPackingType.Getall(): @Activity='ReadAll' and nothing else. */
    public List<Map<String, Object>> packingTypes() {
        LinkedHashMap<String, Object> p = params();
        p.put("@Activity", "ReadAll");
        return safe("packingTypes", "dbo.Sp_InvPackingType_GetAllMethod", p);
    }

    /**
     * BindProductionDepartment:617 -> CommonServices.GetActiveWareHouseByWareHouseType(2) ->
     * InvWareHouse.GetActiveWareHouseByWareHouseType: @WarehouseType when ActivityId != 0;
     * @WarehouseTypeIds and @BranchesIds are unset by this caller and so omitted.
     */
    public List<Map<String, Object>> productionDepartments(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@WarehouseType", 2);
        p.put("@Activity", "GetActiveWareHouseByWareHouseType");
        return safe("departments", "dbo.Sp_InvWareHouse_GetAllMethod", p);
    }

    /** WarehouseInputFill:694 -> WarehousesAllocationToBranch (@BranchId only when != 0). */
    public List<Map<String, Object>> warehousesForBranch(UserAccount u, int branchId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("@BranchId", branchId);
        return safe("warehouses", "[dbo].[USP_GetWarehousesAllocatedToBranch]", p);
    }

    /** JobOrderNoFill:634 -> InvProductionJobOrder.JobOrdersForProduction (three params, unconditional). */
    public List<Map<String, Object>> jobOrdersForProduction(UserAccount u, int branchId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", branchId);
        return safe("jobOrders", "dbo.usp_getJobOrdersForProduction", p);
    }

    /** UOMFill:677 -> UOMSchedule.Getall: @Activity='ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> uomSchedules(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return safe("uomSchedules", "dbo.Sp_UOMSchedule_GetAllMethod", p);
    }

    /**
     * jobOrderInputHCombobind:2180 -> InvFoodProduction.GetDataForDropDownFromFoodProduction
     * (BLL :6821): Org/Company always; @FinancialYearId when != 0; @Activity, @DocumentTypeIds and
     * @BranchesIds when not empty; @ActionId when > 0 (unset here). Column ReferenceName.
     */
    public List<Map<String, Object>> historyJobOrders(UserAccount u, int financialYearId, String branchesIds) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        p.put("@Activity", "JobOrder");
        p.put("@DocumentTypeIds", String.valueOf(DOC_TYPE_INPUT));
        if (ne(branchesIds)) p.put("@BranchesIds", branchesIds);
        return safe("historyJobOrders", "dbo.USP_GetDataForDropDownFromFoodProduction", p);
    }

    // ============================================================================ job-order cascade

    /** CmbJobOrderNo_Leave:1140 -> InvProductionJobOrder.GetGlAccountsByJobOrderId (four unconditional). */
    public List<Map<String, Object>> glAccountsByJobOrder(UserAccount u, int jobOrderId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Id", jobOrderId);
        p.put("@Activity", "GetGlAccountsByJobOrderId");
        return safe("glAccounts", "dbo.Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /** GetPlantFeeder:749 -> InvProductionJobOrder.PlantsForProductionByJobOrderId (five, unconditional). */
    public List<Map<String, Object>> plantsForJobOrder(UserAccount u, int financialYearId, int branchId, int jobOrderId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@FinancialYearId", financialYearId);
        p.put("@BranchesId", branchId);
        p.put("@JobOrderId", jobOrderId);
        return safe("plants", "dbo.usp_getPlantsForProductionByJobOrderId", p);
    }

    /** getJobOrderItems:806 -> InvProductionJobOrder.getJobOrderItemsById(id, 1): @Id; @EntryTypeId when != 0. */
    public List<Map<String, Object>> jobOrderItems(int jobOrderId, int entryTypeId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", jobOrderId);
        if (entryTypeId != 0) p.put("@EntryTypeId", entryTypeId);
        return safe("jobOrderItems", "dbo.usp_getJobOrderItemsById", p);
    }

    // ============================================================================ detail entry reads

    /** GetItemByWareHouseId:720 -> Item.GetItemByWarehouseIdfromInventoryTransactions (all unconditional). */
    public List<Map<String, Object>> itemsByWarehouse(UserAccount u, int warehouseId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@WarehouseId", warehouseId);
        p.put("@Activity", "GetItemByWarehouseIdfromInventoryTransactions");
        return safe("itemsByWarehouse", "dbo.Sp_Item_GetAllMethod", p);
    }

    /**
     * GetAvailableStockForInput:899 -> CommonServices.GetWeightCurrStockByItem ->
     * InvSaleInvoice.GetWeightCurrStockByItem (BLL): @ItemId and @ToDate always; @WarehouseId and
     * @JobLotId when != 0; @CropYear when != "".
     */
    public List<Map<String, Object>> weightCurrStock(UserAccount u, int itemId, LocalDateTime toDate,
                                                     int warehouseId, int jobLotId, String cropYear) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@ToDate", toDate);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (cropYear != null && !"".equals(cropYear)) p.put("@CropYear", cropYear);
        p.put("@Activity", "GetWeightCurrStockByItem");
        return query("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /**
     * CommonServices.AvgRateOnlyForCGS -> GetAvgRatesAndStockInHand.AvgRateOnlyForCGS (BLL):
     * @ItemId, @DocDate always; @DocumentTypeId, @RecId, @JobLotId, @CropYearId, @WarehouseId,
     * @BranchesId when != 0; @CropYear when not empty. Returns AvgRate of row 0, else 0.
     */
    public double avgRateOnlyForCgs(UserAccount u, int itemId, LocalDateTime docDate, int documentTypeId,
                                    int recId, int jobLotId, int cropYearId, String cropYear,
                                    int warehouseId, int branchesId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("@RecId", recId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (cropYearId != 0) p.put("@CropYearId", cropYearId);
        if (ne(cropYear)) p.put("@CropYear", cropYear);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        p.put("@Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> rows = query("dbo.Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return rows.isEmpty() ? 0 : toDouble(ci(rows.get(0), "AvgRate"));
    }

    /**
     * GetAvgRatesAndStockInHand.GetStockByFifoMethod (BLL): @ItemId, @DocDate always; @PackUomId
     * (stockUOM), @WarehouseId, @JobLotId, @PackingTypeId, @CropYearId, @DocumentTypeId, @Id when
     * != 0; @CropYear when neither null nor "".
     */
    public List<Map<String, Object>> stockByFifo(UserAccount u, int itemId, int stockUom, LocalDateTime docDate,
                                                 int warehouseId, int jobLotId, int packingTypeId, int cropYearId,
                                                 String cropYear, int documentTypeId, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        if (stockUom != 0) p.put("@PackUomId", stockUom);
        p.put("@DocDate", docDate);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("@PackingTypeId", packingTypeId);
        if (cropYearId != 0) p.put("@CropYearId", cropYearId);
        if (cropYear != null && !"".equals(cropYear)) p.put("@CropYear", cropYear);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (id != 0) p.put("@Id", id);
        return query("dbo.USP_GetStockByFifoMethod", p);
    }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject (@Activity='ReadByItemID'). */
    public List<Map<String, Object>> uomsForItem(UserAccount u, int itemId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@Activity", "ReadByItemID");
        return query("dbo.Sp_UOMSchedule_GetAllMethod", p);
    }

    // ======================================================================= read one / history

    /** InvFoodProduction.GetByID (BLL :5161) -> DAL GetDate: header by @Id, @Activity='GetById'. */
    public Map<String, Object> header(int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", "GetById");
        List<Map<String, Object>> rows = query("dbo.Sp_InvFoodProduction_GetAllMethod", p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** DAL GetDate: Sp_InvFoodProductionDetail_GetALlMethod @Id, @Activity='InvFoodProductionId'. */
    public List<Map<String, Object>> details(int headerId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", headerId);
        p.put("@Activity", "InvFoodProductionId");
        return query("dbo.Sp_InvFoodProductionDetail_GetALlMethod", p);
    }

    /**
     * InputGridHistory:2301 -> InvFoodProduction.ProductionFormHistory (BLL :5452): Org/Company
     * always; @FinancialYearId, @BranchesId when != 0; @FromDate/@ToDate when not
     * CheckDateTimeNull; @DocNoFrom/@DocNoTo when != 0.0; @InvJobOrderId, @PlantId and
     * @DocumentTypeId (from RefDocumentTypeId) when != 0; @Activity='ReadAll'.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, int branchId,
                                             LocalDateTime fromDate, LocalDateTime toDate,
                                             int docNoFrom, int docNoTo, int jobOrderId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (branchId != 0) p.put("@BranchesId", branchId);
        if (fromDate != null) p.put("@FromDate", fromDate);
        if (toDate != null) p.put("@ToDate", toDate);
        if (docNoFrom != 0) p.put("@DocNoFrom", (double) docNoFrom);
        if (docNoTo != 0) p.put("@DocNoTo", (double) docNoTo);
        if (jobOrderId != 0) p.put("@InvJobOrderId", jobOrderId);
        p.put("@Activity", "ReadAll");
        return query("dbo.Sp_InvFoodProduction_GetAllMethod", p);
    }

    /**
     * CommonServices.VoucherHeadIdGet(Id, 80) -> VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId
     * and (inside Save) DAL CommonServices.GetVoucherHeadId - both Sp_Vouchers_GetMethods with
     * @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId' and the id as
     * @DocumentTypeSrNo.
     */
    public int voucherHeadId(UserAccount u, int documentTypeId, int documentId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", documentId);
        List<Map<String, Object>> rows = query("dbo.Sp_Vouchers_GetMethods", p);
        if (rows.isEmpty()) return 0;
        Object v = ci(rows.get(0), "Id");
        if (v == null && !rows.get(0).isEmpty()) v = rows.get(0).values().iterator().next();
        return toInt(v);
    }

    // =========================================================== LoadavailableTransactionsForIssuance

    /**
     * StockComboFill:197 -> StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists:
     * @BranchesIds = Conversion.ToString(BranchesId) (never "", so always sent); @DocumentTypeIds,
     * @ActivityType and @InventoryParentCategory are unset and omitted.
     */
    public List<Map<String, Object>> issuanceDropDowns(UserAccount u, String branchesIds) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (branchesIds != null && !"".equals(branchesIds)) p.put("@BranchesIds", branchesIds);
        return safe("issuanceDropDowns", "dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    /**
     * PendingInventoryTransactionsForIssuanceLoad:319 -> InventoryStockEvalautionDetail
     * .GetAvailableTransactionsForIssuance (BLL): every filter guarded (int != 0, dates by
     * CheckDateTimeNull, strings by IsNullOrEmpty). RefDocumentTypeId travels as
     * @ReferenceDocumentTypeId.
     */
    public List<Map<String, Object>> availableTransactionsForIssuance(
            UserAccount u, int branchesId, LocalDateTime fromDate, LocalDateTime toDate,
            int supplierCustomerId, int warehouseId, int itemId, int refDocumentTypeId, int jobLotId,
            int inventoryParentCategories, int itemCategoryId, int itemTypeId, String cropYear,
            String itemIds) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        if (fromDate != null) p.put("@DateFrom", fromDate);
        if (toDate != null) p.put("@DateTo", toDate);
        if (supplierCustomerId != 0) p.put("@SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("@ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (inventoryParentCategories != 0) p.put("@InventoryParentCategories", inventoryParentCategories);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        if (ne(cropYear)) p.put("@CropYear", cropYear);
        if (ne(itemIds)) p.put("@ItemIds", itemIds);
        return query("dbo.SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance", p);
    }

    // ================================================================== LoadOutPutPendingforRates

    /**
     * JobOrderFill:125 -> InvProductionJobOrder.GetJobOrderNoForInvFoodProduction: @DocumentTypeId
     * (403) and @FinancialYearId when != 0; @PlanStatus unset; @Activity='GetJobOrderNoForInvFoodProduction'.
     */
    public List<Map<String, Object>> pendingRateJobOrders(UserAccount u, int financialYearId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", 403);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        p.put("@Activity", "GetJobOrderNoForInvFoodProduction");
        return safe("pendingRateJobOrders", "dbo.Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /**
     * OutputGridHistory:190 / grd_SelectionChanged -> InvFoodProduction.GetProductionOutPutDataForRatesInput
     * (BLL :6122): @BranchId always (from BranchesId); @InvProductionJobOrderId when != 0; dates by
     * CheckDateTimeNull; @FromDocNo/@ToDocNo never set by this dialog; @EntryType (the BLL literal is
     * "@EntryType " with a trailing space) when ReportType != "" - a null ReportType passes that
     * test and adds a null, which ADO.NET does not send, so null here means "omit".
     */
    public List<Map<String, Object>> pendingRatesData(UserAccount u, int branchesId, LocalDateTime fromDate,
                                                     LocalDateTime toDate, int jobOrderId, String entryType) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchId", branchesId);
        if (jobOrderId != 0) p.put("@InvProductionJobOrderId", jobOrderId);
        if (fromDate != null) p.put("@FromDate", fromDate);
        if (toDate != null) p.put("@ToDate", toDate);
        if (entryType != null && !"".equals(entryType)) p.put("@EntryType", entryType);
        return query("dbo.USP_GetProductionOutPutDataForRatesInput", p);
    }

    // ============================================================ frmPendingMoveOrderDocuments

    /**
     * PendingDataDbCall -> InvStockTransferHeader.MoveOrder_PendingData (BLL): Org, Company,
     * @FinancialYearId, @BranchesIds always; dates by CheckDateTimeNull; @FromDocNo/@ToDocNo when
     * != 0; @VehicleNo/@BiltyNo when not empty.
     */
    public List<Map<String, Object>> moveOrderPending(UserAccount u, int financialYearId, String branchesIds,
                                                      LocalDateTime fromDate, LocalDateTime toDate,
                                                      int fromDocNo, int toDocNo, String vehicleNo, String biltyNo) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@FinancialYearId", financialYearId);
        p.put("@BranchesIds", branchesIds);
        if (fromDate != null) p.put("@FromDate", fromDate);
        if (toDate != null) p.put("@ToDate", toDate);
        if (fromDocNo != 0) p.put("@FromDocNo", (double) fromDocNo);
        if (toDocNo != 0) p.put("@ToDocNo", (double) toDocNo);
        if (ne(vehicleNo)) p.put("@VehicleNo", vehicleNo);
        if (ne(biltyNo)) p.put("@BiltyNo", biltyNo);
        return query("[dbo].[USP_MoveOrder_PendingData]", p);
    }

    // ================================================================================== wages

    /**
     * CommonServices.WagesDeleteByReferenceIds(80, id) -> InvContractorWagesBillHeader
     * .ContractorWagesRemovebyReferenceIds: usp_ContractorWagesRemovebyReferenceIds, four params.
     * Runs AFTER the save committed, on its own - exactly as the desktop calls it after Save returns.
     */
    public void wagesRemoveByReference(UserAccount u, int documentTypeId, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@Id", id);
        scalar("dbo.usp_ContractorWagesRemovebyReferenceIds", p);
    }

    // ================================================================= Save - account lookups

    /** CommonServies.GetItemListForFinancialEffects / DAL GetItemGlIdsandItemName - Sp_Item_GetAllMethod. */
    public List<Map<String, Object>> itemGlAccounts(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "GetItemGlIdsandItemName");
        return query("dbo.Sp_Item_GetAllMethod", p);
    }

    // ============================================= Save - DAL FIFO branch (DocumentTypeId 80)

    /** DAL CommonServices.GetJobLotGlIdsandName: [dbo].[SP_JobLot_ReadMethod] @OrganizationId, @CompanyId, @Activity. */
    public List<Map<String, Object>> jobLotGlIdsAndName(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "GetJobLotGlIdsandName");
        return query("[dbo].[SP_JobLot_ReadMethod]", p);
    }

    /**
     * DAL CommonServices.GetEqvilentByItemIdAndUomScheduleId: Sp_Item_GetAllMethod @OrganizationId,
     * @CompanyId, @ItemId, @ScheduleId (= ReportsParameters.Id), @Activity -> first row's
     * Equivalent (Conversion.ToDouble), 0 when there is no row.
     */
    public double equivalentByItemAndSchedule(UserAccount u, int itemId, int scheduleId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@ScheduleId", scheduleId);
        p.put("@Activity", "GetEqvilentByItemIdAndUomScheduleId");
        List<Map<String, Object>> r = query("dbo.Sp_Item_GetAllMethod", p);
        return r.isEmpty() ? 0d : toDouble(ci(r.get(0), "Equivalent"));
    }

    /**
     * DAL CommonServices.FIFOImplemention (Architecture.DAL.Common :1759) - USP_GetStockByFifoMethod:
     * @OrganizationId, @CompanyId, @ItemId, @DocDate always; @PackUomId (stockUOM), @WarehouseId,
     * @JobLotId, @PackingTypeId, @DocumentTypeId, @Id when != 0 (@CropYearId is never set by
     * SetData, so never sent); @CropYear when neither null nor ""; @FIFOXML only when earlier rows
     * of the same save already reserved layers.
     */
    public List<Map<String, Object>> stockByFifoForSave(UserAccount u, int itemId, int stockUom, LocalDateTime docDate,
                                                        int warehouseId, int jobLotId, int packingTypeId,
                                                        String cropYear, int documentTypeId, int id, String fifoXml) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        if (stockUom != 0) p.put("@PackUomId", stockUom);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("@PackingTypeId", packingTypeId);
        if (cropYear != null && !"".equals(cropYear)) p.put("@CropYear", cropYear);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (id != 0) p.put("@Id", id);
        if (fifoXml != null) p.put("@FIFOXML", fifoXml);
        return query("dbo.USP_GetStockByFifoMethod", p);
    }

    // ===================================================================== Save - the writes

    /* Procedure-declared parameter names (procdure.utf8.sql), used to filter the model maps. */
    public static final Set<String> P_HEADER = set(
            "Id","DocDate","DocCode","DocumentTypeId","WIPAccountId","WIPItemId","MainRemarks","IsApproved",
            "EntryDate","EntryUser","ModifyDate","ModifyUser","OrganizationId","CompanyId",
            "InvFoodProductionPlanId","InvJobOrderId","InvJobOrderNo","EntryType","BranchId","ProjectId",
            "ActionId","FinancialYearId","PlantId","EBDepartmentId","BranchSrNo","ContractScheduleId",
            "WipWareHouseId","BaseDocumentTypeId","StockHoldForLabApproval","UserLogId");
    public static final Set<String> P_DETAIL = set(
            "Id","InvFoodProductionId","RefDocumentTypeId","RefDocNoId","InvProductionJobOrderId",
            "InvProductionJobOrderNo","EntryType","WarehouseId","ItemId","ItemUomId","CropBatch","JobLotId",
            "PackingtypeId","Qty","PackUnit","Weight","Rate","RateUOMId","Amount","ItemPmCost","GeneralPmCost",
            "ItemOhCost","GeneralOhCost","NetRate","TotalAmount","Remarks","VoucherHeadId","StockAcId",
            "RefDocSubIdNo","JobOrderInPutId","OutPutIdFromJobOrder","DeleteFlag","LineId","JobOrderScheduleId",
            "GrossWeight","EbUnit","EbTotal","MoveOrderDocumentTypeId","MoveOrderDocId","IsApproved",
            "ApprovedUserId","ApprovedDate","ApprovedComments","UnApprovedUserId","UnApprovedDate",
            "UnApprovedComments");
    public static final Set<String> P_VOUCHER_HEAD = set(
            "Id","DocumentTypeId","DocumentTypeSrNo","RefDocNoId","VoucherCode","VoucherDate","Remarks",
            "RemarksOtherLingo","VoucherAmount","FinancialYearId","RefAccountId","AgainstAccountId",
            "MultiCurrencyId","ConversionFormula","ExchangeCurrencyRate","FcAmount","CheqId","ChequeNo",
            "ChequeDate","PayTitle","BankBranch","ChequePrintId","Source","DrCrNoteType","IsApproved",
            "EntryDate","EntryUser","ModifyDate","ModifyUser","PostDate","PostUser","PostState",
            "OrganizationId","CompanyId","IncludeWHT","BranchId","ProjectId","ManualBillNo","BillAmount",
            "DueDate","DueDays","ActionId","CostCenterAmount","AttachmentsValues","RefDocumentTypeId",
            "CustomAttachmentsValues","CustomAccounts","BaseDocumentTypeId","AdvanceTaxAccountId",
            "AdvanceTaxAmount","OtherChargesAccountId","OtherChargesAmount","IsUploaded","UploadedDate",
            "UploadedById","InclusiveTax","FixedAssetEntryTypeId","UserLogId");
    public static final Set<String> P_VOUCHER_DETAIL = set(
            "Id","VoucherHeadId","AccountId","AgainstAccountId","Comments","CommentsOtherLingo","DebitAmount",
            "CreditAmount","JobLotId","RefInvoiceNo","TaxesTotalAmount","TaxesRemarks","IsTaxable","TaxTypeId",
            "TaxPrcnt","DCheqDate","CheqNoDetail","DocumentTypeIdRef","InvoiceNoRefId","ItemId","OrderNo","GpNo",
            "VehicleNo","GpDate","QtyIn","QtyOut","WeightIn","WeightOut","SupplierCustomerId","ItemRate",
            "RateCut","RateCutAmount","ItemAmount","Expenses","Freight","Journal","Commission","DMultiCurrencyId",
            "DConversionFormula","DExchangeCurrencyRate","DCurrencyAmount","PaymentType","AdvanceAmount",
            "WhtHolding","SaleTax","ExTax","Adjustment","ActionId","LineId","RefDocumentTypeId","RefDocNoId",
            "RefDocNoDetailId","RefDocSubIdNo","ItemCgsRate","TotalCreditAmount","TotalDebitAmount","SubNo",
            "EntryDate","ChequeStatus","PayeeTitle","SubsidiaryTypeId","EmployeeId","SubsidiaryAccountId",
            "TotalPaidAmount","InvoiceAmount","IsCGS","SubsidiaryAgainstTypeId","SubsidiaryAgainstAccountId",
            "ThirdCurrencyId","ThirdCurrencyFcyExchangeRate","ThirdCurrencyHcyExchangeRate","ThirdCurrencyAmount",
            "ThirdCurrencyReceiverExchangeRate","ThirdCurrencyReceiverFcyAmount","SortNo","PayeeOnly",
            "InstrumentTypeId","ChequeTypeId","BranchesId","CostCenterId","SBRTaxAmount","DiscountPercent",
            "DiscountAmount","ReferenceAccountId","LocationTypeId","PaymentTypeId","TaxAmount","BaseFcyId",
            "BaseFcyExchangeRate","BaseFcyAmount","InventoryId");
    public static final Set<String> P_INV_TRANSACTIONS = set(
            "Id","OrganizationId","CompanyId","BranchesId","ProjectsId","RefDocumentTypeId","RefDocIdNo",
            "DocCodeNo","DocDate","SupplierCustomerId","IsApproved","RefDocSubIdNo","WarehouseId","JobLotId",
            "ItemId","ItemUom","CropBatch","InvPackingTypeId","QtyIn","QtyOut","BillWeightIn","BillWeightOut",
            "StockWeightIn","StockWeightOut","CalcType","ItemRate","RateUom","AmountIn","ExpenseAmountIn",
            "AmountOut","TranRemarks","EntryDate","EntryUserId","ModifyDate","ModifyUserId","GrossWeight",
            "EbUnit","EbTotal","WtCut","WtCutTotal","AddLess","GpNo","VehicleNo","ItemConditionId",
            "SecondaryUomId","SecondaryUomQty","SecondaryUomItemRate");
    public static final Set<String> P_STOCK_EVALUATION_INSERT = set(
            "Id","RefDocumentTypeId","RefDocIdNo","RefRefDocumentTypeId","RefRefDocIdNo","DocCodeNo","DocDate",
            "SupplierCustomerId","RefRefDocSubIdNo","RefDocSubIdNo","OrderNo","WarehouseId","PrdJobOrderNo",
            "VehicleNo","GpNoDcNo","TranRemarks","ItemId","ItemUom","CropBatch","JobLotId","InvPackingTypeId",
            "QtyIn","QtyOut","BillWeightIn","BillWeightOut","StockWeightIn","StockWeightOut","CalcType","ItemRate",
            "RateUom","AmountIn","ExpenseAmountIn","AmountOut","CgsRate","CgsAmount","IsApproved","OrganizationId",
            "CompanyId","BranchesId","ProjectsId","EntryDate","EntryUser","ModifyDate","ModifyUser","LineId",
            "CityId","OtherDocumentTypeId","OtherDocNoId","OtherSubDocNoId","GrossWeight","EbTotal",
            "AddLessWeight","WagesAmount","Freight","Commission","OtherExpense","InvoiceId","InvoiceDetailId",
            "VarientId","RefWarehouseId","BiltyNo","ValidForSale","ItemConditionId","RackId");
    public static final Set<String> P_STOCK_EVALUATION_UPDATE = set(
            "hId","Id","RefDocumentTypeId","RefDocIdNo","DocCodeNo","DocDate","SupplierCustomerId",
            "RefDocSubIdNo","OrderNo","WarehouseId","PrdJobOrderNo","VehicleNo","GpNoDcNo","TranRemarks",
            "ItemId","ItemUom","CropBatch","JobLotId","InvPackingTypeId","QtyIn","QtyOut","BillWeightIn",
            "BillWeightOut","StockWeightIn","StockWeightOut","CalcType","ItemRate","RateUom","AmountIn",
            "ExpenseAmountIn","AmountOut","CgsRate","IsApproved","OrganizationId","CompanyId","BranchesId",
            "ProjectsId","CityId","LineId","RefRefDocumentTypeId","RefRefDocIdNo","RefRefDocSubIdNo",
            "EntryUser","ModifyUser","CgsAmount","OtherDocumentTypeId","OtherDocNoId","OtherSubDocNoId",
            "InvoiceId","InvoiceDetailId","VarientId","RefWarehouseId","EbPurAgainstWeight","FreightDeduction",
            "BiltyNo","BatchNo","WorkOrderId","ProductionStageId","WorkStationId","CastingTypeId","GrnGdnId",
            "GrnGdnNo","GrnGdnDate","GrnGdnDetailId","GPType","GpTypeSrNo","ItemConditionId","SecondaryUomId",
            "SecondaryUomQty","SecondaryUomItemRate");

    private static Set<String> set(String... a) { return Collections.unmodifiableSet(new LinkedHashSet<>(Arrays.asList(a))); }

    /** Model map (property -> value) filtered to the declared parameters, as "@Property". */
    private static LinkedHashMap<String, Object> setProc(Map<String, Object> model, Set<String> declared) {
        LinkedHashMap<String, Object> p = params();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            if (declared.contains(e.getKey())) p.put("@" + e.getKey(), e.getValue());
        }
        return p;
    }

    /** GenericProvider.SetProc(tx, model, proc) - ExecuteScalar, its result converted with Convert.ToInt32. */
    public int setProc(String proc, Map<String, Object> model, Set<String> declared) {
        return toInt(scalar("dbo." + proc, setProc(model, declared)));
    }

    /** A SqlCommand the DAL builds by hand (AddWithValue each, ExecuteNonQuery / ExecuteScalar). */
    public void command(String proc, LinkedHashMap<String, Object> p) {
        scalar(proc, p);
    }

    /* The value-type properties of the four models GenericProvider reflects over, at their CLR
       defaults (0 / false). Reference-type and Nullable properties are null by default and are
       therefore not sent. Same sets InventoryOpeningDefaults carries for the inventory ports. */

    public static LinkedHashMap<String, Object> voucherHeadDefaults() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        for (String k : new String[]{"IncludeWHT", "IsApproved", "PostState", "CustomAccounts", "IsUploaded"}) m.put(k, false);
        for (String k : new String[]{"BillAmount", "ExchangeCurrencyRate", "FcAmount", "VoucherAmount",
                "CostCenterAmount", "AdvanceTaxAmount", "OtherChargesAmount"}) m.put(k, 0d);
        for (String k : new String[]{"AgainstAccountId", "BranchId", "CheqId", "ChequePrintId", "CompanyId",
                "DocumentTypeId", "DocumentTypeSrNo", "DueDays", "EntryUser", "FinancialYearId", "Id", "ModifyUser",
                "MultiCurrencyId", "OrganizationId", "PostUser", "ProjectId", "RefAccountId", "RefDocNoId",
                "VoucherCode", "ActionId", "RefDocumentTypeId", "FixedAssetEntryTypeId", "BaseDocumentTypeId",
                "AdvanceTaxAccountId", "OtherChargesAccountId"}) m.put(k, 0);
        return m;
    }

    public static LinkedHashMap<String, Object> voucherDetailDefaults() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        for (String k : new String[]{"Adjustment", "AdvanceAmount", "Commission", "CreditAmount", "DCurrencyAmount",
                "DebitAmount", "TaxAmount", "DExchangeCurrencyRate", "Expenses", "ExTax", "Freight", "ItemAmount",
                "ItemRate", "Journal", "QtyIn", "QtyOut", "RateCut", "RateCutAmount", "SaleTax", "TaxesTotalAmount",
                "TaxPrcnt", "WeightIn", "WeightOut", "WhtHolding", "ItemCgsRate", "TotalDebitAmount",
                "TotalCreditAmount", "ThirdCurrencyFcyExchangeRate", "ThirdCurrencyHcyExchangeRate",
                "ThirdCurrencyAmount", "ThirdCurrencyReceiverExchangeRate", "ThirdCurrencyReceiverFcyAmount",
                "SBRTaxAmount", "DiscountPercent", "DiscountAmount", "BaseFcyExchangeRate", "BaseFcyAmount"}) m.put(k, 0d);
        for (String k : new String[]{"ThirdCurrencyId", "AccountId", "AgainstAccountId", "DMultiCurrencyId",
                "DocumentTypeIdRef", "GpNo", "Id", "InvoiceNoRefId", "ItemId", "JobLotId", "OrderNo",
                "SupplierCustomerId", "EmployeeId", "SubsidiaryTypeId", "SubsidiaryAccountId",
                "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId", "TaxTypeId", "ActionId", "VoucherHeadId",
                "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId", "RefDocSubIdNo", "LineId",
                "InstrumentTypeId", "SubNo", "SortNo", "PaymentTypeId", "ChequeTypeId", "BranchesId",
                "CostCenterId", "ReferenceAccountId", "LocationTypeId", "BaseFcyId"}) m.put(k, 0);
        m.put("IsCGS", false);
        return m;
    }

    public static LinkedHashMap<String, Object> inventoryTransactionsDefaults() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        for (String k : new String[]{"AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "ExpenseAmountIn",
                "ItemRate", "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut"}) m.put(k, 0d);
        for (String k : new String[]{"BranchesId", "CompanyId", "DocCodeNo", "InvPackingTypeId", "ItemId",
                "ItemUom", "JobLotId", "OrganizationId", "ProjectsId", "RateUom", "RefDocIdNo", "RefDocSubIdNo",
                "RefDocumentTypeId", "SupplierCustomerId", "WarehouseId", "Id"}) m.put(k, 0);
        return m;
    }

    public static LinkedHashMap<String, Object> stockEvaluationDefaults() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        for (String k : new String[]{"AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate",
                "ExpenseAmountIn", "ItemRate", "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut",
                "CgsAmount"}) m.put(k, 0d);
        for (String k : new String[]{"BranchesId", "CompanyId", "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId",
                "ItemId", "ItemUom", "JobLotId", "OrderNo", "OrganizationId", "PrdJobOrderNo", "ProjectsId",
                "RateUom", "RefDocIdNo", "RefDocSubIdNo", "RefDocumentTypeId", "OtherDocumentTypeId",
                "OtherDocNoId", "OtherSubDocNoId", "SupplierCustomerId", "WarehouseId", "RefWarehouseId",
                "CityId", "LineId", "RefRefDocumentTypeId", "RefRefDocIdNo", "RefRefDocSubIdNo", "EntryUser",
                "ModifyUser", "InvoiceId", "InvoiceDetailId", "VarientId", "ItemConditionId"}) m.put(k, 0);
        return m;
    }

    // ================================================================================== helpers

    /** Conversion.ToInt: Convert.ToInt32 - numbers round half-to-even, strings must be integral, else 0. */
    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o instanceof Number) {
            double d = ((Number) o).doubleValue();
            if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
            return (int) Math.rint(d);
        }
        String s = o.toString().trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); }
        catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble: Convert.ToDouble (thousands separators accepted), Infinity -> 0, bad -> 0. */
    public static double toDouble(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) {
            double d = ((Number) o).doubleValue();
            return Double.isInfinite(d) ? 0 : d;
        }
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        String s = o.toString().trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try {
            double d = Double.parseDouble(s);
            return Double.isInfinite(d) ? 0 : d;
        } catch (NumberFormatException e) { return 0; }
    }
}
