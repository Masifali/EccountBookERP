package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.LinkedCaseInsensitiveMap;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 280, tab "Output" - Architecture.WinApp.Production/frmProductionOutput.cs (5,981 lines),
 * DocumentTypeId 112, plus the two dialogs it opens: LoadOutPutPendingforRates and
 * frmPendingMoveOrderDocuments.
 *
 * Every procedure below is the one the desktop's BLL calls, with the parameters the BLL builds.
 * A parameter the BLL wraps in a test (if (x != 0), !CheckDateTimeNull, !IsNullOrEmpty) is
 * GUARDED: it is omitted when unset, never sent as NULL or 0. Each method names its BLL.
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE = BLL InvFoodProduction.Save -> MakeVoucher -> DAL InvFoodProduction.SetData, one transaction
 * ---------------------------------------------------------------------------------------------
 * GenericProvider.SetProc reflects over the model and sends every NON-VIRTUAL property as
 * "@" + name (DAL.Common :5773). A property left at its C# default therefore travels as 0/false,
 * while a reference-type property left null reaches ADO.NET as a null-valued parameter, which
 * SqlClient treats as "not supplied" - the procedure default. So: value-type defaults are sent,
 * nulls are omitted. The parameter sets below are exactly the procedure's declared parameters
 * that are also model properties (the procedure must declare every non-virtual property, or
 * SetProc would fail on the desktop); @BranchSrNo / @UserLogId and the detail approval columns
 * are procedure-only and are therefore never sent.
 */
@Repository
public class ProductionOutputRepository {

    public static final int DOC_TYPE_OUTPUT = 112;
    public static final int DOC_TYPE_INPUT = 80;

    @Autowired private JdbcTemplate jdbc;

    // =============================================================================== plumbing

    public static Map<String, Object> p() { return new LinkedHashMap<>(); }

    /**
     * EXEC with named parameters, walking every result the way ExecuteScalar / Fill do: the FIRST
     * result set is returned, everything after it is drained so nothing is left pending on a
     * pooled connection. A null value is not bound at all (see the class comment).
     *
     * Runs on the Spring-managed connection, so inside {@link #save} it is part of the one
     * transaction, exactly as every SetData call shares the desktop's SqlTransaction.
     */
    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        final String text = sql.toString();
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            try (PreparedStatement ps = con.prepareStatement(text)) {
                for (int i = 0; i < values.size(); i++) bind(ps, i + 1, values.get(i));
                boolean isRs = ps.execute();
                List<Map<String, Object>> out = null;
                while (true) {
                    if (isRs) {
                        try (ResultSet rs = ps.getResultSet()) {
                            if (out == null) out = read(rs);
                            else while (rs.next()) { /* drain */ }
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    isRs = ps.getMoreResults();
                }
                return out == null ? new ArrayList<>() : out;
            }
        });
    }

    /** ExecuteScalar: first column of the first row of the first result set, or null. */
    public Object scalar(String proc, Map<String, Object> params) {
        List<Map<String, Object>> r = rows(proc, params);
        if (r.isEmpty() || r.get(0).isEmpty()) return null;
        return r.get(0).values().iterator().next();
    }

    private static void bind(PreparedStatement ps, int i, Object v) throws SQLException {
        if (v instanceof java.util.Date && !(v instanceof Timestamp)) {
            ps.setTimestamp(i, new Timestamp(((java.util.Date) v).getTime()));
        } else if (v instanceof java.time.LocalDateTime) {
            ps.setTimestamp(i, Timestamp.valueOf((java.time.LocalDateTime) v));
        } else if (v instanceof java.time.LocalDate) {
            ps.setTimestamp(i, Timestamp.valueOf(((java.time.LocalDate) v).atStartOfDay()));
        } else {
            ps.setObject(i, v);
        }
    }

    private static List<Map<String, Object>> read(ResultSet rs) throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> row = new LinkedCaseInsensitiveMap<>(n);
            for (int c = 1; c <= n; c++) {
                String name = md.getColumnLabel(c);
                if (name == null || name.isEmpty()) name = md.getColumnName(c);
                if (name == null || name.isEmpty()) name = "Column" + c;
                Object v = rs.getObject(c);
                if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
                row.put(name, v);
            }
            out.add(row);
        }
        return out;
    }

    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static int i(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return new BigDecimal(String.valueOf(o).trim()).intValue(); } catch (Exception e) { return 0; }
    }

    public static double d(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (Exception e) { return 0; }
    }

    public static boolean b(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = o == null ? "" : String.valueOf(o).trim().toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }

    public static String s(Object o) { return o == null ? "" : String.valueOf(o); }

    // ====================================================================== configuration / rights

    /** GlobalVariables_Helper.GetConfigValueFromGlobal / DAL CommonServices.GetConfigurationFromAllocation. */
    public String configValue(int org, int comp, String description) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@ConfigDescription", description);
        q.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", q);
        return r.isEmpty() ? "" : s(col(r.get(0), "ConfigKey")).trim();
    }

    /** CommonServices.GetMultipleConfigurationsByConfigDescriptions (frmFoodProduction_Load:413). */
    public List<Map<String, Object>> multipleConfigurations(int org, int comp, String csv) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@ConfigDescription", csv);
        q.put("@Activity", "GetMultipleConfigurationsByConfigDescriptions");
        return rows("Sp_ConfigrationsAllocation_GetAllMethod", q);
    }

    /** CommonServices.GetERPFeatureById(5) -> FIFOCGSFlag (Load:451). */
    public boolean erpFeature(int org, int comp, int featureId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", q)) {
            if (i(col(r, "Id")) == featureId) return true;
        }
        return false;
    }

    /**
     * clsGlobalVariables.WagesRefDocumentsStatusList (Load:406) - USP_GetRefDocumentsForWages with
     * @RefDocumentTypeId GUARDED in the BLL; the list is loaded with 0, so it is omitted.
     */
    public List<Map<String, Object>> wagesRefDocumentStatuses() {
        return rows("[dbo].[USP_GetRefDocumentsForWages]", p());
    }

    /** Architecture.BLL.tblUserRights.GetByUserId - the grant rows SetRightsValueInRightsObject walks. */
    public List<Map<String, Object>> userRightsForScreen(int userId, String screenName, String roleName, int comp) {
        Map<String, Object> q = p();
        q.put("@UserId", userId);
        q.put("@ScreenName", screenName);
        q.put("@RightName", roleName == null ? "" : roleName);
        q.put("@CompanyId", comp);
        q.put("@Activity", "GetByUserId");
        return rows("Sp_tblUserRights_GetAllMethod", q);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public String financialYearStart(int org, int comp, int yearId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        List<Map<String, Object>> years = rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", q);
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (i(col(r, "Id")) == yearId) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        Object v = row == null ? null : col(row, "Start_Period");
        return v == null ? null : s(v);
    }

    // ============================================================================ the pickers

    /** WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId - @BranchId guarded. */
    public List<Map<String, Object>> warehouses(int org, int comp, int branchId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        if (branchId != 0) q.put("@BranchId", branchId);
        return rows("[dbo].[USP_GetWarehousesAllocatedToBranch]", q);
    }

    /** JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId - @BranchId guarded. */
    public List<Map<String, Object>> jobLots(int org, int comp, int branchId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        if (branchId != 0) q.put("@BranchId", branchId);
        return rows("[dbo].[USP_GetJobLotsAllocatedToBranch]", q);
    }

    /** CommonServices.CropYearGetAllService -> InvCropYear.Getall, @Activity='ReadAll'. */
    public List<Map<String, Object>> cropYears(int org, int comp) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@Activity", "ReadAll");
        return rows("Sp_InvCropYear_GetAllMethod", q);
    }

    /** InvPackingType.Getall - @Activity='ReadAll' only (no tenancy parameter in the BLL). */
    public List<Map<String, Object>> packingTypes() {
        Map<String, Object> q = p();
        q.put("@Activity", "ReadAll");
        return rows("Sp_InvPackingType_GetAllMethod", q);
    }

    /** UOMSchedule.Getall - @Activity='ReadByOrganizationCompanyId' (UOMFill:650). */
    public List<Map<String, Object>> uomSchedules(int org, int comp) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_UOMSchedule_GetAllMethod", q);
    }

    /** InvProductionJobOrder.JobOrdersForProduction - three parameters, all unconditional. */
    public List<Map<String, Object>> jobOrdersForProduction(int org, int comp, int branchId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@BranchesId", branchId);
        return rows("usp_getJobOrdersForProduction", q);
    }

    /** Item.ReadAllItems - @Activity='ReadAllItems' (AllItemForOutPut:1209). */
    public List<Map<String, Object>> allItems(int org, int comp) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@Activity", "ReadAllItems");
        return rows("Sp_Item_GetAllMethod", q);
    }

    /** InvProductionJobOrder.GetByProductAndFinishGoodItemsByJobOrderId (AllItemForOutPutAgainstJobOrder:1227). */
    public List<Map<String, Object>> itemsByJobOrder(int org, int comp, int jobOrderId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@JobOrderId", jobOrderId);
        return rows("USP_GetByProductAndFinishGoodItemsByJobOrderId", q);
    }

    /** InvProductionJobOrder.GetPlantByJobOrderIdFromProductionInput (OutPutPlantFillFromProductionInput:693). */
    public List<Map<String, Object>> plantsFromProductionInput(int jobOrderId, int branchId) {
        Map<String, Object> q = p();
        q.put("@InvJobOrderId", jobOrderId);
        q.put("@BranchesId", branchId);
        return rows("[dbo].[USP_GetPlantByJobOrderIdFromProductionInput]", q);
    }

    /**
     * InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId (CmbJobOrderNoOutput_Leave:1457).
     * @InvJobOrderId always; @ItemId, @PlantId, @ItemUomId guarded != 0; @EntryType (from
     * obj.Activity) guarded non-empty and never set by this form; @Activity fixed.
     */
    public List<Map<String, Object>> inputTotals(int org, int comp, int jobOrderId, int plantId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@InvJobOrderId", jobOrderId);
        if (plantId != 0) q.put("@PlantId", plantId);
        q.put("@Activity", "GetInPutTotalQtyandWeightByJobOrderId");
        return rows("Sp_InvFoodProduction_GetAllMethod", q);
    }

    /** InvProductionJobOrder.GetLastItemRateByJobOrder - all six unconditional (:843). */
    public List<Map<String, Object>> lastItemRate(int org, int comp, int jobOrderId, int itemId, int typeId,
                                                  java.time.LocalDateTime effective) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@JobOrderId", jobOrderId);
        q.put("@ItemId", itemId);
        q.put("@TransTypeId", typeId);
        q.put("@EffectiveDate", effective);
        return rows("USP_GetLastItemRateByJobOrder", q);
    }

    /** InvProductionJobOrder.AvgRateForReturnToGodown_Production - all seven unconditional (:907). */
    public List<Map<String, Object>> avgRateForReturnToGodown(int org, int comp, int jobOrderId, int plantId,
                                                              int itemId, String cropYear) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@DocumentTypeId", DOC_TYPE_INPUT);
        q.put("@JobOrderId", jobOrderId);
        q.put("@PlantId", plantId);
        q.put("@ItemId", itemId);
        /* Conversion.ToString(CmbCropYearOutPut.Text) - never null, so always sent. */
        q.put("@CropBatch", cropYear == null ? "" : cropYear);
        return rows("usp_getAvgRateForReturnToGodown_Production", q);
    }

    /**
     * InvFoodProduction.GetProductionInPutDataByJobAndPlantId(org, comp, 80, job, plant, null)
     * (:1569). The fifth argument, activity, is null here and the BLL omits a null/empty one.
     */
    public List<Map<String, Object>> productionInputByJobAndPlant(int org, int comp, int jobOrderId, int plantId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@DocumentTypeId", DOC_TYPE_INPUT);
        q.put("@JobOrderId", jobOrderId);
        q.put("@PlantId", plantId);
        return rows("usp_GetProductionInPutDataByJobAndPlantId", q);
    }

    /** InvProductionJobOrder.ExportContract_GetForProduction - @RecId guarded != 0 (:1115). */
    public List<Map<String, Object>> exportContracts(int org, int comp, int yearId, int jobOrderId, int recId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@FinancialYearId", yearId);
        q.put("@JobOrderId", jobOrderId);
        if (recId != 0) q.put("@RecId", recId);
        return rows("[dbo].[USP_ExportContract_GetForProduction]", q);
    }

    /** InvFoodProduction.GetSerialNumber - @FinancialYearId / @BranchesId guarded; DocCode of row 0. */
    public int serialNumber(int org, int comp, int yearId, int branchId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@DocumentTypeId", DOC_TYPE_OUTPUT);
        if (yearId != 0) q.put("@FinancialYearId", yearId);
        if (branchId != 0) q.put("@BranchesId", branchId);
        q.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> r = rows("Sp_InvFoodProduction_GetAllMethod", q);
        return r.isEmpty() ? 0 : i(col(r.get(0), "DocCode"));
    }

    /** InvFoodProduction.GetByID - header (@Id, @Activity='GetById'). */
    public Map<String, Object> header(int id) {
        Map<String, Object> q = p();
        q.put("@Id", id);
        q.put("@Activity", "GetById");
        List<Map<String, Object>> r = rows("Sp_InvFoodProduction_GetAllMethod", q);
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL InvFoodProduction.GetDate - details: Sp_InvFoodProductionDetail_GetALlMethod 'InvFoodProductionId'. */
    public List<Map<String, Object>> details(int id) {
        Map<String, Object> q = p();
        q.put("@Id", id);
        q.put("@Activity", "InvFoodProductionId");
        return rows("Sp_InvFoodProductionDetail_GetALlMethod", q);
    }

    /** CommonServices.VoucherHeadIdGet -> Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    public List<Map<String, Object>> voucherHeadRows(int org, int comp, int docTypeId, int docTypeSrNo) {
        Map<String, Object> q = p();
        q.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@DocumentTypeId", docTypeId);
        q.put("@DocumentTypeSrNo", docTypeSrNo);
        return rows("Sp_Vouchers_GetMethods", q);
    }

    /**
     * InvFoodProduction.GetDataForDropDownFromFoodProduction (jobOrderOutputHCombobind:2466):
     * BranchesIds = branch.ToString(), FinancialYearId guarded, DocumentTypeIds "112",
     * Activity "JobOrder"; ActionId guarded > 0 and never set.
     */
    public List<Map<String, Object>> historyJobOrders(int org, int comp, int yearId, int branchId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        if (yearId != 0) q.put("@FinancialYearId", yearId);
        q.put("@Activity", "JobOrder");
        q.put("@DocumentTypeIds", String.valueOf(DOC_TYPE_OUTPUT));
        q.put("@BranchesIds", String.valueOf(branchId));
        return rows("USP_GetDataForDropDownFromFoodProduction", q);
    }

    /**
     * InvFoodProduction.ProductionFormHistory (OutputGridHistory:2590) - @Activity='ReadAll'.
     * FinancialYearId, BranchesId, JobOrderId guarded != 0; both dates always set by the form;
     * DocNoFrom / DocNoTo guarded != 0; PlantId and DocumentTypeId never set.
     */
    public List<Map<String, Object>> history(int org, int comp, int yearId, int branchId,
                                             java.time.LocalDateTime from, java.time.LocalDateTime to,
                                             double docFrom, double docTo, int jobOrderId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        if (yearId != 0) q.put("@FinancialYearId", yearId);
        if (branchId != 0) q.put("@BranchesId", branchId);
        if (from != null) q.put("@FromDate", from);
        if (to != null) q.put("@ToDate", to);
        if (docFrom != 0) q.put("@DocNoFrom", docFrom);
        if (docTo != 0) q.put("@DocNoTo", docTo);
        if (jobOrderId != 0) q.put("@InvJobOrderId", jobOrderId);
        q.put("@Activity", "ReadAll");
        return rows("Sp_InvFoodProduction_GetAllMethod", q);
    }

    /**
     * InvFoodProduction.GetSummeryValues (txtFGWeight_TextChanged:2896). PlantId never set;
     * @FgNetWeight guarded != 0; dates never set; @BranchesIds null (a null-valued SqlParameter =
     * not supplied).
     */
    public List<Map<String, Object>> summaryValues(int org, int comp, int jobOrderId, double fgWeight) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@JobOrderId", jobOrderId);
        if (fgWeight != 0) q.put("@FgNetWeight", fgWeight);
        return rows("Sp_InvFoodProduction_GetSummeryValues", q);
    }

    // ================================================================= LoadOutPutPendingforRates

    /**
     * InvProductionJobOrder.GetJobOrderNoForInvFoodProduction (JobOrderFill) - DocumentTypeId 403
     * and FinancialYearId guarded != 0; Status never set so @PlanStatus is omitted.
     */
    public List<Map<String, Object>> pendingRatesJobOrders(int org, int comp, int yearId) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@DocumentTypeId", 403);
        if (yearId != 0) q.put("@FinancialYearId", yearId);
        q.put("@Activity", "GetJobOrderNoForInvFoodProduction");
        return rows("Sp_InvProductionJobOrder_GetAllMethod", q);
    }

    /**
     * InvFoodProduction.GetProductionOutPutDataForRatesInput. @BranchId always; JobOrderId
     * guarded != 0; both dates set by the dialog; FromDocNo/ToDocNo never set by the dialog (its
     * Doc No boxes are not read); "@EntryType " (the BLL's own trailing space) from ReportType,
     * guarded != "" - a null ReportType, as on the selection-changed call, reaches SqlClient as a
     * null-valued parameter, which is "not supplied".
     */
    public List<Map<String, Object>> pendingRates(int org, int comp, int branchId,
                                                  java.time.LocalDateTime from, java.time.LocalDateTime to,
                                                  int jobOrderId, String entryType) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@BranchId", branchId);
        if (jobOrderId != 0) q.put("@InvProductionJobOrderId", jobOrderId);
        if (from != null) q.put("@FromDate", from);
        if (to != null) q.put("@ToDate", to);
        if (entryType != null && !entryType.isEmpty()) q.put("@EntryType", entryType);
        return rows("USP_GetProductionOutPutDataForRatesInput", q);
    }

    // ============================================================= frmPendingMoveOrderDocuments

    /**
     * InvStockTransferHeader.MoveOrder_PendingData (PendingDataDbCall). FinancialYearId and
     * BranchesIds always; dates guarded by CheckDateTimeNull (always set); FromDocNo/ToDocNo
     * guarded != 0; VehicleNo/BiltyNo guarded !IsNullOrWhiteSpace.
     */
    public List<Map<String, Object>> moveOrdersPending(int org, int comp, int yearId, int branchId,
                                                       java.time.LocalDateTime from, java.time.LocalDateTime to,
                                                       double docFrom, double docTo, String vehicleNo, String biltyNo) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@FinancialYearId", yearId);
        q.put("@BranchesIds", String.valueOf(branchId));
        if (from != null) q.put("@FromDate", from);
        if (to != null) q.put("@ToDate", to);
        if (docFrom != 0) q.put("@FromDocNo", docFrom);
        if (docTo != 0) q.put("@ToDocNo", docTo);
        if (vehicleNo != null && !vehicleNo.trim().isEmpty()) q.put("@VehicleNo", vehicleNo);
        if (biltyNo != null && !biltyNo.trim().isEmpty()) q.put("@BiltyNo", biltyNo);
        return rows("[dbo].[USP_MoveOrder_PendingData]", q);
    }

    // ========================================================================== after-save wages

    /** CommonServices.WagesDeleteByReferenceIds(112, id) -> usp_ContractorWagesRemovebyReferenceIds. */
    public void wagesRemoveByReference(int org, int comp, int docTypeId, int id) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@DocumentTypeId", docTypeId);
        q.put("@Id", id);
        rows("usp_ContractorWagesRemovebyReferenceIds", q);
    }

    /** Sp_Item_GetAllMethod 'GetItemGlIdsandItemName' - BLL CommonServies.GetItemListForFinancialEffects. */
    public List<Map<String, Object>> itemGlAccounts(int org, int comp) {
        Map<String, Object> q = p();
        q.put("@OrganizationId", org);
        q.put("@CompanyId", comp);
        q.put("@Activity", "GetItemGlIdsandItemName");
        return rows("Sp_Item_GetAllMethod", q);
    }

    // ===================================================================================== SAVE

    /**
     * DAL InvFoodProduction.SetData for DocumentTypeId 112, step for step, in one transaction.
     *
     * @param header   Sp_InvFoodProduction_Insert/_Update parameters (the model's non-virtual properties)
     * @param details  Sp_InvFoodProductionDetail_Insert parameters per row; LineId/InvFoodProductionId are set here
     * @param voucher  MakeVoucher's result, or null when BLL Save set RefDocumentTypeId = 1
     * @param voucherLines its detail lines (each carrying "LineId")
     * @param skipInventoryTransactions local 7 of SetData: JobOrderCreatewithoutRates && doc 112
     * @param removeIds OutputDetailRowsRemoveIds (",12,15") or null
     * @return the record Id (SetData's num)
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details,
                    Map<String, Object> voucher, List<Map<String, Object>> voucherLines,
                    boolean skipInventoryTransactions, String removeIds) {
        boolean insert = i(header.get("@Id")) == 0;
        int org = i(header.get("@OrganizationId"));
        int comp = i(header.get("@CompanyId"));
        int jobOrderId = i(header.get("@InvJobOrderId"));

        /* :0056 - SetProc; num > 0 ? obj.Id = num : num = obj.Id. */
        int id = i(scalar(insert ? "Sp_InvFoodProduction_Insert" : "Sp_InvFoodProduction_Update", header));
        if (id <= 0) id = i(header.get("@Id"));

        /* :0089 - every detail row, LineId = 1..n, parent Id stamped, new Id read back. */
        int line = 0;
        for (Map<String, Object> det : details) {
            line++;
            det.put("@LineId", line);
            det.put("@InvFoodProductionId", id);
            int detId = i(scalar("Sp_InvFoodProductionDetail_Insert", det));
            det.put("@Id", detId);
        }

        /* :1516 - unless (JobOrderCreatewithoutRates && 112): Sp_InventoryTransactions_GetALLMethod
           with an InventoryTransactions model carrying Org, Company, RefDocumentTypeId and
           RefDocIdNo; every other value-type property at its default. */
        if (!skipInventoryTransactions) {
            Map<String, Object> t = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : InventoryOpeningDefaults.transactions().entrySet()) {
                t.put(e.getKey().startsWith("@") ? e.getKey() : "@" + e.getKey(), e.getValue());
            }
            t.put("@OrganizationId", org);
            t.put("@CompanyId", comp);
            t.put("@RefDocumentTypeId", DOC_TYPE_OUTPUT);
            t.put("@RefDocIdNo", id);
            scalar("Sp_InventoryTransactions_GetALLMethod", t);
        }

        /* :1865 - USP_ProductionInPutAndOutPutWeightValidation for 112. */
        Map<String, Object> w = p();
        w.put("@OrganizationId", org);
        w.put("@CompanyId", comp);
        w.put("@DocumentTypeId", DOC_TYPE_OUTPUT);
        w.put("@InvProductionJobOrderId", jobOrderId);
        w.put("@PlantId", i(header.get("@PlantId")));
        scalar("USP_ProductionInPutAndOutPutWeightValidation", w);

        /* :1b2c - removed rows, only when the list is non-empty. */
        if (removeIds != null && !removeIds.isEmpty()) {
            Map<String, Object> r = p();
            r.put("@OrganizationId", org);
            r.put("@CompanyId", comp);
            r.put("@DocumentTypeId", DOC_TYPE_OUTPUT);
            r.put("@Id", id);
            r.put("@DetailIds", removeIds);
            scalar("USP_InvFoodProductionDetailRowsDeleteByIds", r);
        }

        /* :1c1a - per row; @RefId is OutPutIdFromJobOrder for 112 (never set by this form -> 0). */
        for (Map<String, Object> det : details) {
            Map<String, Object> c = p();
            c.put("@DocumentTypeId", DOC_TYPE_OUTPUT);
            c.put("@JobOrderId", jobOrderId);
            c.put("@RefId", i(det.get("@OutPutIdFromJobOrder")));
            c.put("@NetWeight", d(det.get("@Weight")));
            scalar("usp_ProductionWeightCompareFromJobOrderWeight", c);
        }

        /* :1d1a - the voucher, unless RefDocumentTypeId == 1. */
        if (voucher != null) {
            int vhId = 0;
            List<Map<String, Object>> vh = voucherHeadRows(org, comp, DOC_TYPE_OUTPUT, id);
            if (!vh.isEmpty()) vhId = i(col(vh.get(0), "Id"));
            voucher.put("@Id", vhId);
            voucher.put("@DocumentTypeSrNo", id);
            int ret = i(scalar(vhId == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucher));
            if (ret > 0) vhId = ret;
            if (voucherLines != null && !voucherLines.isEmpty()) {
                for (Map<String, Object> vd : voucherLines) {
                    vd.put("@VoucherHeadId", vhId);
                    vd.put("@BranchesId", header.get("@BranchId"));
                    /* b__6: the detail whose LineId equals this line's LineId gives RefDocSubIdNo. */
                    int ln = i(vd.get("@LineId"));
                    for (Map<String, Object> det : details) {
                        if (i(det.get("@LineId")) == ln) { vd.put("@RefDocSubIdNo", i(det.get("@Id"))); break; }
                    }
                    scalar("Sp_VoucherDetail_Insert", vd);
                }
                if (vhId > 0) {
                    Map<String, Object> bc = p();
                    bc.put("@OrganizationId", org);
                    bc.put("@CompanyId", comp);
                    bc.put("@Id", vhId);
                    scalar("USP_VoucherBalanceCheck", bc);
                }
            }
        }
        return id;
    }

    /** VoucherHead defaults - the shared, complete non-null parameter map (InventoryOpeningDefaults). */
    public static Map<String, Object> voucherHeadDefaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : InventoryOpeningDefaults.voucher().entrySet()) {
            m.put(e.getKey().startsWith("@") ? e.getKey() : "@" + e.getKey(), e.getValue());
        }
        return m;
    }

    /** VoucherDetail defaults - the shared, complete non-null parameter map (InventoryOpeningDefaults). */
    public static Map<String, Object> voucherDetailDefaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : InventoryOpeningDefaults.detail().entrySet()) {
            m.put(e.getKey().startsWith("@") ? e.getKey() : "@" + e.getKey(), e.getValue());
        }
        return m;
    }

    /** C# double.ToString() on .NET Framework: "G" with 15 significant digits. */
    public static String csDouble(double v) {
        if (Double.isNaN(v)) return "NaN";
        if (Double.isInfinite(v)) return v > 0 ? "Infinity" : "-Infinity";
        if (v == 0) return "0";
        BigDecimal bd = new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros();
        return bd.toPlainString();
    }
}
