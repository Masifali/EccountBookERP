package com.mst.repositories;

import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 871 "Production Against (Pre Costing Job Order)" -
 * Architecture.WinApp.Production/ProductionAgainstPreCostingJobOrder.cs (base.Name = the rights key).
 *
 * Every read the desktop form makes through the BLL/DAL, mapped to the stored procedure the DAL
 * executes (the BLL source is not in the recovered tree; the procedure names were traced from the
 * DAL method names, the procedure index and the sibling 280 port that already runs the same calls).
 * Parameters a BLL adds only when set (the `if (x != 0)` guards of ReportsParameters) are added here
 * under the same guards; a null value is omitted (AddWithValue(name, null) -> the procedure default).
 *
 * The write paths (InvFoodProduction.Save for 80/112/181, InvFoodProductionPackingMaterial.Save,
 * InvFoodProductionOverHeads.Save, UpdateSettlement, SetDataForSettlementFinancials) live in
 * {@code Prod871FoodProductionSaver} and reuse the generic DAL plumbing of the 280 repositories.
 */
@Repository
public class Prod871ProductionAgainstPreCostingJobOrderRepository {

    public static final String DESKTOP_SCREEN_NAME = "ProductionAgainstPreCostingJobOrder";

    public static final int DOC_INPUT = 80;
    public static final int DOC_OUTPUT = 112;
    public static final int DOC_OVERHEAD = 110;
    public static final int DOC_PACKING_MATERIAL = 111;
    public static final int DOC_CONSUMPTION = 181;
    public static final int DOC_SETTLEMENT = 142;
    /** The pre-costing job order document type (JobOrderNoFillForSettlement:1725, history RefDocumentTypeId:3065). */
    public static final int DOC_PRE_COSTING_JOB_ORDER = 401;

    private final JdbcTemplate jdbc;

    public Prod871ProductionAgainstPreCostingJobOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =================================================================================== plumbing

    public static LinkedHashMap<String, Object> p() { return new LinkedHashMap<>(); }

    /** EXEC proc @A=?, @B=? ... - every row of the FIRST result set; the rest drained (RAISERROR surfaces). */
    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, params, false));
    }

    /** ExecuteScalar: first column of the first row of the first result set, or null. */
    public Object scalar(String proc, Map<String, Object> params) {
        List<Map<String, Object>> r = jdbc.execute(
                (ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, params, true));
        if (r == null || r.isEmpty() || r.get(0).isEmpty()) return null;
        return r.get(0).values().iterator().next();
    }

    private static List<Map<String, Object>> run(Connection con, String proc, Map<String, Object> params,
                                                 boolean firstRowOnly) throws SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) {
                Object v = values.get(i);
                if (v instanceof LocalDateTime) v = Timestamp.valueOf((LocalDateTime) v);
                if (v instanceof LocalDate) v = java.sql.Date.valueOf((LocalDate) v);
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
                    while (true) {
                        boolean more = ps.getMoreResults();
                        if (!more && ps.getUpdateCount() == -1) break;
                    }
                    return out;
                }
                if (ps.getUpdateCount() == -1) break;
                isRs = ps.getMoreResults();
            }
        }
        return out;
    }

    /** SQL Server column names are case-insensitive to the desktop's DataRow[] indexer. */
    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o instanceof Number) {
            double d = ((Number) o).doubleValue();
            return Double.isNaN(d) || Double.isInfinite(d) ? 0 : (int) Math.rint(d);
        }
        String s = o.toString().trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); }
        catch (NumberFormatException e) {
            try { return (int) Math.rint(Double.parseDouble(s.replace(",", ""))); } catch (NumberFormatException x) { return 0; }
        }
    }

    public static double toDouble(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) { double d = ((Number) o).doubleValue(); return Double.isInfinite(d) ? 0 : d; }
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        String s = o.toString().trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { double d = Double.parseDouble(s); return Double.isInfinite(d) ? 0 : d; } catch (NumberFormatException e) { return 0d; }
    }

    public static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    public static boolean bool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o).trim().toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }

    // ====================================================================== Load - switches / rights

    /** CommonServices.GetMultipleConfigurationsByConfigDescriptions (frmFoodProduction_Load:1240). */
    public List<Map<String, Object>> multipleConfigurations(int org, int comp, String csv) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ConfigDescription", csv);
        m.put("@Activity", "GetMultipleConfigurationsByConfigDescriptions");
        return rows("dbo.Sp_ConfigrationsAllocation_GetAllMethod", m);
    }

    /** CommonServices.GetConfigurationByConfigDescription - one ConfigKey (used by BLL Save). */
    public String configValue(int org, int comp, String description) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ConfigDescription", description);
        m.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("dbo.Sp_ConfigrationsAllocation_GetAllMethod", m);
        return r.isEmpty() ? "" : str(col(r.get(0), "ConfigKey")).trim();
    }

    /** CommonServices.GetERPFeatureById(5) -> FIFOCGSFlag (:1312). */
    public boolean erpFeature(int org, int comp, int featureId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        for (Map<String, Object> r : rows("dbo.USP_GetERPFeaturesByCompanyId", m)) {
            if (toInt(col(r, "Id")) == featureId) return true;
        }
        return false;
    }

    /** clsGlobalVariables.WagesRefDocumentsStatusList (:1234-1239). */
    public List<Map<String, Object>> wagesRefDocumentStatuses() {
        return rows("[dbo].[USP_GetRefDocumentsForWages]", p());
    }

    /** CommonServices.SetRightsValueInRightsObject(base.Name) (:1267) - the grant rows. */
    public List<Map<String, Object>> userRights(int userId, String screenName, String roleName, int comp) {
        LinkedHashMap<String, Object> m = p();
        m.put("@UserId", userId); m.put("@ScreenName", screenName);
        m.put("@RightName", roleName == null ? "" : roleName);
        m.put("@CompanyId", comp); m.put("@Activity", "GetByUserId");
        return rows("dbo.Sp_tblUserRights_GetAllMethod", m);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (ResetInputhistory:3010). */
    public Object financialYearStart(int org, int comp, int financialYearId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        List<Map<String, Object>> years = rows("dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", m);
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (toInt(col(r, "Id")) == financialYearId) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row == null ? null : col(row, "Start_Period");
    }

    // ============================================================================ Load - combos

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4" / "4,12") (:1526, :1552). */
    public List<Map<String, Object>> accountTitlesByAccountTypeIds(int org, int comp, int appId, int userId, String ids) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp); m.put("@AppId", appId);
        if (ids != null && !ids.isEmpty()) m.put("@AccountTypeIds", ids);
        if (userId != 0) m.put("@UserId", userId);
        m.put("@Activity", "GetAccountTitleByAccountTypeIds");
        return rows("dbo.Sp_COAAllocation_GetAllMethod", m);
    }

    /** JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId (:1549). */
    public List<Map<String, Object>> jobLotsForBranch(int org, int comp, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (branchId != 0) m.put("@BranchId", branchId);
        return rows("[dbo].[USP_GetJobLotsAllocatedToBranch]", m);
    }

    /** WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId (:1550) - grdByProduct WareHouseId list. */
    public List<Map<String, Object>> warehousesForBranch(int org, int comp, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (branchId != 0) m.put("@BranchId", branchId);
        return rows("[dbo].[USP_GetWarehousesAllocatedToBranch]", m);
    }

    /** WarehousesAllocationToBranch.GetWarehousesFromTransactionAndAllocatedToBranchPM (:1551) - grddetailPackingMaterial WarehouseId list. */
    public List<Map<String, Object>> warehousesForBranchPm(int org, int comp, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (branchId != 0) m.put("@BranchId", branchId);
        return rows("[dbo].[USP_GetWarehousesFromTransactionAndAllocatedToBranchPM]", m);
    }

    /** CommonServices.CropYearGetAllService (:1553). */
    public List<Map<String, Object>> cropYears(int org, int comp) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp); m.put("@Activity", "ReadAll");
        return rows("dbo.Sp_InvCropYear_GetAllMethod", m);
    }

    /** CommonServices.InvPackingTypeGetAllService (:1554). */
    public List<Map<String, Object>> packingTypes() {
        LinkedHashMap<String, Object> m = p();
        m.put("@Activity", "ReadAll");
        return rows("dbo.Sp_InvPackingType_GetAllMethod", m);
    }

    /** CommonServices.GetActiveWareHouseByWareHouseType(2) (BindProductionDepartment:1570). */
    public List<Map<String, Object>> productionDepartments(int org, int comp) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@WarehouseType", 2);
        m.put("@Activity", "GetActiveWareHouseByWareHouseType");
        return rows("dbo.Sp_InvWareHouse_GetAllMethod", m);
    }

    /** InvProductionJobOrder.JobOrdersForProduction (JobOrderNoFill:1599) - Id, PlanCode, DocumentTypeId. */
    public List<Map<String, Object>> jobOrdersForProduction(int org, int comp, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp); m.put("@BranchesId", branchId);
        return rows("dbo.usp_getJobOrdersForProduction", m);
    }

    /** InvProductionJobOrder.GetJobOrderForSummary (JobOrderNoFillForSummery:1693). */
    public List<Map<String, Object>> jobOrdersForSummary(int org, int comp, int financialYearId, String branchesIds) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchesIds != null && !branchesIds.isEmpty()) m.put("@BranchesIds", branchesIds);
        m.put("@Activity", "GetJobOrderForSummary");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /** InvProductionJobOrder.GetJobOrderAll(ActionId, DocumentTypeId 401) (JobOrderNoFillForSettlement:1719). */
    public List<Map<String, Object>> jobOrdersAll(int org, int comp, int financialYearId, int actionId, int documentTypeId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (actionId != 0) m.put("@ActionId", actionId);
        if (documentTypeId != 0) m.put("@DocumentTypeId", documentTypeId);
        m.put("@Activity", "GetJobOrderNoAll");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /** UOMSchedule.Getall (UOMFill:1747) - every schedule row with ItemId / UOMCode / Equivalent. */
    public List<Map<String, Object>> uomSchedules(int org, int comp) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("dbo.Sp_UOMSchedule_GetAllMethod", m);
    }

    /** CommonServices.GetUomScheduleByItemId (:2380, :3684, :8021, :10236). */
    public List<Map<String, Object>> uomScheduleByItemId(int org, int comp, int itemId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ItemId", itemId); m.put("@Activity", "ReadByItemID");
        return rows("dbo.Sp_UOMSchedule_GetAllMethod", m);
    }

    /** CommonServices.GetStaticUOM (:1514) - grdDetailOverhead RateUomId list. */
    public List<Map<String, Object>> uomStatic() {
        return rows("[dbo].[USP_UOMStatic_GetAllMethod]", p());
    }

    /** BranchesAllocationToUser.GetBranchsAllocatedToUser (BranchesFill:6482). */
    public List<Map<String, Object>> branchesForUser(int org, int comp, int userId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp); m.put("@UserId", userId);
        return rows("[dbo].[USP_GetBranchsAllocatedToUser]", m);
    }

    /** InvProductionJobOrder.PlantsForProductionByJobOrderId (GetPlantFeeder:1770). */
    public List<Map<String, Object>> plantsForJobOrder(int org, int comp, int financialYearId, int branchId, int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@FinancialYearId", financialYearId); m.put("@BranchesId", branchId);
        m.put("@JobOrderId", jobOrderId);
        return rows("dbo.usp_getPlantsForProductionByJobOrderId", m);
    }

    /** InvProductionJobOrder.GetPlantByJobOrderIdFromProductionInput (OutPutPlantFillFromProductionInput:1835). */
    public List<Map<String, Object>> plantsFromProductionInput(int jobOrderId, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@InvJobOrderId", jobOrderId); m.put("@BranchesId", branchId);
        return rows("[dbo].[USP_GetPlantByJobOrderIdFromProductionInput]", m);
    }

    /** InvProductionJobOrder.GetPlantByJobOrderId (cmbsummeryJobOrderNo_TextChanged:6527). */
    public List<Map<String, Object>> plantByJobOrderId(int jobOrderId, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", jobOrderId); m.put("@BranchesId", branchId);
        m.put("@Activity", "GetPlantByJobOrderId");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /** InvProductionJobOrder.GetGlAccountsByJobOrderId (CmbJobOrderNo_Leave:2201 etc.) - WIP account / item / FinishGoodRate. */
    public List<Map<String, Object>> glAccountsByJobOrderId(int org, int comp, int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@Id", jobOrderId); m.put("@Activity", "GetGlAccountsByJobOrderId");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /** InvFoodProduction.GetSerialNumber(DocumentTypeId) (:2172, :3319, :9206). */
    public int serialNumber(int org, int comp, int branchId, int documentTypeId, int financialYearId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@DocumentTypeId", documentTypeId);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchId != 0) m.put("@BranchesId", branchId);
        m.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> r = rows("dbo.Sp_InvFoodProduction_GetAllMethod", m);
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocCode"));
    }

    /** InvFoodProductionPackingMaterial.GenerateCode (:4553). */
    public int packingMaterialCode(int org, int comp, int branchId, int financialYearId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchId != 0) m.put("@BranchesId", branchId);
        m.put("@Activity", "GenerateCode");
        return toInt(scalar("dbo.Sp_InvFoodProductionPackingMaterial_GetAllMethod", m));
    }

    /** InvFoodProductionOverHeads.GenerateCode (:5341). */
    public int overheadCode(int org, int comp, int branchId, int financialYearId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchId != 0) m.put("@BranchesId", branchId);
        m.put("@Activity", "GenerateCode");
        return toInt(scalar("dbo.Sp_InvFoodProductionOverHeads_GetAllMethod", m));
    }

    /** InvFoodProduction.GetDataForDropDownFromFoodProduction (history pickers :2934, :4042, :4091, :10823). */
    public List<Map<String, Object>> dropDownFromFoodProduction(int org, int comp, int financialYearId,
                                                                String documentTypeIds, String branchesIds, String activity) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (activity != null && !activity.isEmpty()) m.put("@Activity", activity);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) m.put("@DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) m.put("@BranchesIds", branchesIds);
        return rows("dbo.USP_GetDataForDropDownFromFoodProduction", m);
    }

    // ============================================================================ job order cascade

    /** InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId (:3353, :4597, :5381, :9131). */
    public List<Map<String, Object>> inputTotals(int org, int comp, int jobOrderId, int plantId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@InvJobOrderId", jobOrderId);
        if (plantId != 0) m.put("@PlantId", plantId);
        m.put("@Activity", "GetInPutTotalQtyandWeightByJobOrderId");
        return rows("dbo.Sp_InvFoodProduction_GetAllMethod", m);
    }

    /** InvFoodProduction.GetProductionInPutDataByJobAndPlantId (getProductionInputForReturnToGodown:3447). */
    public List<Map<String, Object>> productionInputByJobAndPlant(int org, int comp, int documentTypeId, int jobOrderId, int plantId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@DocumentTypeId", documentTypeId);
        if (jobOrderId != 0) m.put("@JobOrderId", jobOrderId);
        if (plantId != 0) m.put("@PlantId", plantId);
        return rows("dbo.usp_GetProductionInPutDataByJobAndPlantId", m);
    }

    // ================================================================================== loaders

    /** LoadInputDataByJobOrder (btnLoadInvoices_Click:2858) - USP_LoadInputDataByInvProductionJobOrder. */
    public List<Map<String, Object>> loadInputByJobOrder(int org, int comp, int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", jobOrderId); m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ActionId", 1);
        return rows("dbo.USP_LoadInputDataByInvProductionJobOrder", m);
    }

    /** LoadOutPutDataByJobOrder (btnLoadOutPutFromjobOrder_Click:4468) - USP_LoadOuPutDataByInvProductionJobOrder. */
    public List<Map<String, Object>> loadOutputByJobOrder(int org, int comp, int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", jobOrderId); m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ActionId", 1);
        return rows("dbo.USP_LoadOuPutDataByInvProductionJobOrder", m);
    }

    /** LoadPackingMaterialDataByJobOrder (btnLoadPMFromJobOrder_Click:5279). */
    public List<Map<String, Object>> loadPackingMaterialByJobOrder(int org, int comp, int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", jobOrderId); m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ActionId", 1);
        return rows("dbo.USP_LoadPackingMaterialDataByInvProductionJobOrder", m);
    }

    /** LoadOverHeadsDataByJobOrder (btnLoadOverHeadsFromJobOrder_Click:6196). */
    public List<Map<String, Object>> loadOverheadsByJobOrder(int org, int comp, int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", jobOrderId); m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        return rows("dbo.USP_LoadOverHeadsDataByInvProductionJobOrder", m);
    }

    /** LoadOutPutPendingforRates (btnPenForRateEntries_Click:4425 / btnPendingForRateInput_Click:8923). */
    public List<Map<String, Object>> pendingForRates(int org, int comp, int branchId, int jobOrderId,
                                                     LocalDateTime from, LocalDateTime to, String entryType) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp); m.put("@BranchId", branchId);
        if (jobOrderId != 0) m.put("@InvProductionJobOrderId", jobOrderId);
        if (from != null) m.put("@FromDate", from);
        if (to != null) m.put("@ToDate", to);
        if (entryType != null && !entryType.isEmpty()) m.put("@EntryType", entryType);
        return rows("dbo.USP_GetProductionOutPutDataForRatesInput", m);
    }

    /** LoadConsumptionPendingforRates (BtnPendingForRatesFormConsumption_Click:10401). */
    public List<Map<String, Object>> consumptionPendingForRates(int org, int comp, int branchId, int jobOrderId,
                                                                LocalDateTime from, LocalDateTime to) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp); m.put("@BranchId", branchId);
        if (jobOrderId != 0) m.put("@InvProductionJobOrderId", jobOrderId);
        if (from != null) m.put("@FromDate", from);
        if (to != null) m.put("@ToDate", to);
        return rows("[dbo].[USP_GetProductionConsumptionDataForPendingRates]", m);
    }

    /** The pending-rates loaders' job order picker - GetJobOrderNoForInvFoodProduction (DocumentTypeId 403). */
    public List<Map<String, Object>> pendingRateJobOrders(int org, int comp, int financialYearId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@DocumentTypeId", 403);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        m.put("@Activity", "GetJobOrderNoForInvFoodProduction");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /** LoadavailableTransactionsForIssuanceOnConsumption - the filter combos (StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists). */
    public List<Map<String, Object>> issuanceDropDowns(int org, int comp, String branchesIds) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (branchesIds != null && !branchesIds.isEmpty()) m.put("@BranchesIds", branchesIds);
        return rows("dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists", m);
    }

    /** LoadavailableTransactionsForIssuanceOnConsumption - the grid. */
    public List<Map<String, Object>> availableTransactionsForIssuance(int org, int comp, LocalDateTime from, LocalDateTime to,
                                                                      int supplierCustomerId, int warehouseId, int itemId,
                                                                      int refDocumentTypeId, int jobLotId, int parentCategoryId,
                                                                      int itemCategoryId, int itemTypeId, String cropYear) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (from != null) m.put("@DateFrom", from);
        if (to != null) m.put("@DateTo", to);
        if (supplierCustomerId != 0) m.put("@SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) m.put("@WarehouseId", warehouseId);
        if (itemId != 0) m.put("@ItemId", itemId);
        if (refDocumentTypeId != 0) m.put("@ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) m.put("@JobLotId", jobLotId);
        if (parentCategoryId != 0) m.put("@InventoryParentCategories", parentCategoryId);
        if (itemCategoryId != 0) m.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) m.put("@ItemTypeId", itemTypeId);
        if (cropYear != null && !cropYear.isEmpty()) m.put("@CropYear", cropYear);
        return rows("[dbo].[USP_GetAvailableTransactionsForIssuanceOnProductionCosumption]", m);
    }

    // ===================================================================== consumption detail entry

    /** InvFoodProduction.DropDownForConsumptionForm (ConsumptionDetailComboBind:9229). */
    public List<Map<String, Object>> dropDownForConsumptionForm(int org, int comp, String branchesIds) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (branchesIds != null && !branchesIds.isEmpty()) m.put("@BranchesIds", branchesIds);
        return rows("[dbo].[USP_DropDownForConsumptionForm]", m);
    }

    /** CommonServices.GetWeightCurrStockByItem (GetAvailableStockForConsumption:10504). */
    public List<Map<String, Object>> weightCurrStockByItem(int org, int comp, int itemId, LocalDateTime toDate,
                                                           int warehouseId, int jobLotId, String cropYear) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ItemId", itemId); m.put("@ToDate", toDate);
        if (warehouseId != 0) m.put("@WarehouseId", warehouseId);
        if (jobLotId != 0) m.put("@JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) m.put("@CropYear", cropYear);
        m.put("@Activity", "GetWeightCurrStockByItem");
        return rows("[Sp_InvSaleInvoice_GetAllMethod]", m);
    }

    /** CommonServices.AvgRateOnlyForCGS (:2589, :2696, :10579). */
    public double avgRateOnlyForCgs(int org, int comp, int itemId, LocalDateTime docDate, int documentTypeId, int recId,
                                    int jobLotId, int cropYearId, String cropYear, int warehouseId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ItemId", itemId); m.put("@DocDate", docDate);
        if (documentTypeId != 0) m.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) m.put("@RecId", recId);
        if (jobLotId != 0) m.put("@JobLotId", jobLotId);
        if (cropYearId != 0) m.put("@CropYearId", cropYearId);
        if (cropYear != null && !cropYear.isEmpty()) m.put("@CropYear", cropYear);
        if (warehouseId != 0) m.put("@WarehouseId", warehouseId);
        m.put("@Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> r = rows("dbo.Sp_GetAvgRatesAndStockInHand_GetAllMethod", m);
        return r.isEmpty() ? 0d : toDouble(col(r.get(0), "AvgRate"));
    }

    /** CommonServices.GetAvgRateFromFIFOMethod (:10042, :10579) - the AvgRate of the first FIFO layer. */
    public List<Map<String, Object>> stockByFifo(int org, int comp, int itemId, int packUomId, LocalDateTime docDate,
                                                 int warehouseId, int jobLotId, int packingTypeId, String cropYear) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@ItemId", itemId);
        if (packUomId != 0) m.put("@PackUomId", packUomId);
        m.put("@DocDate", docDate);
        if (warehouseId != 0) m.put("@WarehouseId", warehouseId);
        if (jobLotId != 0) m.put("@JobLotId", jobLotId);
        if (packingTypeId != 0) m.put("@PackingTypeId", packingTypeId);
        if (cropYear != null && !cropYear.isEmpty()) m.put("@CropYear", cropYear);
        return rows("dbo.USP_GetStockByFifoMethod", m);
    }

    // ======================================================================= read one / history

    /** InvFoodProduction.GetByID - the header (:2443, :3731, :10298, :6387, :8699, :8764, :11043). */
    public Map<String, Object> productionHeader(int id) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", id); m.put("@Activity", "GetById");
        List<Map<String, Object>> r = rows("dbo.Sp_InvFoodProduction_GetAllMethod", m);
        return r.isEmpty() ? null : r.get(0);
    }

    /** GetByID - invFoodProductionDetails. */
    public List<Map<String, Object>> productionDetails(int id) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", id); m.put("@Activity", "InvFoodProductionId");
        return rows("dbo.Sp_InvFoodProductionDetail_GetALlMethod", m);
    }

    /** GetByID - InvFoodProductionConsumptionDetailslist. */
    public List<Map<String, Object>> consumptionDetails(int id) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", id); m.put("@Activity", "InvFoodProductionId");
        return rows("dbo.USP_InvFoodProductionConsumptionDetail_GetALlMethod", m);
    }

    /**
     * InvFoodProduction.ProductionFormHistory with RefDocumentTypeId = 401 (:3065, :4230, :6271, :10923).
     * ReadAll's @DocumentTypeId is the JOB ORDER document type ("--Job Order DocumentTypeId" in the
     * procedure), which is where the BLL puts ReportsParameters.RefDocumentTypeId.
     */
    public List<Map<String, Object>> formHistory(int org, int comp, int financialYearId, int branchId,
                                                 LocalDateTime from, LocalDateTime to, int docNoFrom, int docNoTo,
                                                 int jobOrderId, int plantId, int jobOrderDocumentTypeId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchId != 0) m.put("@BranchesId", branchId);
        if (from != null) m.put("@FromDate", from);
        if (to != null) m.put("@ToDate", to);
        if (docNoFrom != 0) m.put("@DocNoFrom", docNoFrom);
        if (docNoTo != 0) m.put("@DocNoTo", docNoTo);
        if (jobOrderId != 0) m.put("@InvJobOrderId", jobOrderId);
        if (plantId != 0) m.put("@PlantId", plantId);
        if (jobOrderDocumentTypeId != 0) m.put("@DocumentTypeId", jobOrderDocumentTypeId);
        m.put("@Activity", "ReadAll");
        return rows("dbo.Sp_InvFoodProduction_GetAllMethod", m);
    }

    /** InvFoodProductionPackingMaterial.GetByID (ReadByIdPackingMaterial:4852). */
    public Map<String, Object> packingMaterialById(int id) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", id); m.put("@Activity", "ReadById");
        List<Map<String, Object>> r = rows("dbo.Sp_InvFoodProductionPackingMaterial_GetAllMethod", m);
        return r.isEmpty() ? null : r.get(0);
    }

    /** InvFoodProductionPackingMaterial.GetAll (BindHistoryPMGrid:4898) - ActionId 1 = "Show Only Zero-Rated Entry". */
    public List<Map<String, Object>> packingMaterialHistory(int org, int comp, int financialYearId, int branchId, int actionId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchId != 0) m.put("@BranchesId", branchId);
        if (actionId != 0) m.put("@ActionId", actionId);
        m.put("@Activity", "ReadAll");
        return rows("dbo.Sp_InvFoodProductionPackingMaterial_GetAllMethod", m);
    }

    /** InvFoodProductionOverHeads.GetAll (BindGridHistoryOverHead:5889). */
    public List<Map<String, Object>> overheadHistory(int org, int comp, int financialYearId, int branchId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (branchId != 0) m.put("@BranchesId", branchId);
        m.put("@Activity", "ReadAll");
        return rows("dbo.Sp_InvFoodProductionOverHeads_GetAllMethod", m);
    }

    /** InvFoodProductionOverHeads.GetByJobOrderID (ReadByIdOverHeadJobOrderWise:5596, grdOverHeadHistory_SelectionChanged:6026). */
    public List<Map<String, Object>> overheadsByJobOrder(int jobOrderId) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Id", jobOrderId); m.put("@Activity", "ReadByjobProductionId");
        return rows("dbo.Sp_InvFoodProductionOverHeads_GetAllMethod", m);
    }

    /** CommonServices.VoucherHeadIdGet(Id, DocumentTypeId) (GetVoucherHeadId:1859, :8270, :8868, :10377). */
    public int voucherHeadId(int org, int comp, int documentTypeId, int documentTypeSrNo) {
        LinkedHashMap<String, Object> m = p();
        m.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@DocumentTypeId", documentTypeId); m.put("@DocumentTypeSrNo", documentTypeSrNo);
        List<Map<String, Object>> r = rows("dbo.Sp_Vouchers_GetMethods", m);
        if (r.isEmpty()) return 0;
        Object v = col(r.get(0), "Id");
        if (v == null && !r.get(0).isEmpty()) v = r.get(0).values().iterator().next();
        return toInt(v);
    }

    // ====================================================================== summary / settlement

    private LinkedHashMap<String, Object> summaryParams(int org, int comp, int jobOrderId, int plantId,
                                                        LocalDateTime from, LocalDateTime to, String branchesIds) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@JobOrderId", jobOrderId);
        if (plantId != 0) m.put("@PlantId", plantId);
        if (from != null) m.put("@FromDate", from);
        if (to != null) m.put("@ToDate", to);
        /* the C# guard (BranchesIds != null) is true for every value the form builds - always sent */
        m.put("@BranchesIds", branchesIds == null ? "" : branchesIds);
        return m;
    }

    /** InvFoodProduction.GetSummeryValues (GrdSummaryValuesFill:6725, Settlement:7291, txtFGWeight:8646). */
    public List<Map<String, Object>> summaryValues(int org, int comp, int jobOrderId, int plantId,
                                                   LocalDateTime from, LocalDateTime to, String branchesIds, Double fgNetWeight) {
        LinkedHashMap<String, Object> m = summaryParams(org, comp, jobOrderId, plantId, from, to, branchesIds);
        if (fgNetWeight != null && fgNetWeight != 0d) m.put("@FgNetWeight", fgNetWeight);
        return rows("dbo.Sp_InvFoodProduction_GetSummeryValues", m);
    }

    /** InvFoodProductionReports.InvFoodProductionRecoverySummeryReport (GrdSummaryReportFill:6604, btnRecoveryReport:8527). */
    public List<Map<String, Object>> recoverySummaryReport(int org, int comp, int jobOrderId, int plantId,
                                                           LocalDateTime from, LocalDateTime to, String branchesIds, int actionId) {
        LinkedHashMap<String, Object> m = summaryParams(org, comp, jobOrderId, plantId, from, to, branchesIds);
        if (actionId != 0) m.put("@ActionId", actionId);
        return rows("dbo.Sp_InvFoodProduction_Summery_Rpt", m);
    }

    /** InvFoodProductionReports.FoodProduction_DocWiseSummeryReport (grdDocWiseSummeryFill:6833). */
    public List<Map<String, Object>> docWiseSummaryReport(int org, int comp, int jobOrderId, int plantId,
                                                          LocalDateTime from, LocalDateTime to, String branchesIds) {
        return rows("[dbo].[USP_FoodProduction_DocWiseSummeryReport]",
                summaryParams(org, comp, jobOrderId, plantId, from, to, branchesIds));
    }

    /** InvFoodProductionReports.InvFoodProductionSettlement(Activity) (btnGeneralSettlement_Click:7107/7206/7242). */
    public List<Map<String, Object>> settlementData(int org, int comp, int jobOrderId, String activity) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@JobOrderId", jobOrderId); m.put("@Activity", activity);
        return rows("dbo.Sp_InvFoodProductionSettlement", m);
    }

    /** The "Record Not Found For DisPlay" row counts of the report buttons (606 PM/OH, 607, 613, 613A, 615, 615A, 602A). */
    public int reportRowCount(String proc, LinkedHashMap<String, Object> params) {
        return rows(proc, params).size();
    }

    // ====================================================================== writes PM / OH (DAL)

    /** CommonServices.GetItemGlIdsandItemName - the item -> PurchaseGLAC list the vouchers use. */
    public List<Map<String, Object>> itemGlAccounts(int org, int comp) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@Activity", "GetItemGlIdsandItemName");
        return rows("dbo.Sp_Item_GetAllMethod", m);
    }

    /** DAL InvFoodProductionPackingMaterial.SetData / InvFoodProductionOverHeads.SetData - row deletes by ids. */
    public void deleteRowsByIds(String proc, int org, int comp, int documentTypeId, String detailIds) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@DocumentTypeId", documentTypeId); m.put("@DetailIds", detailIds);
        scalar(proc, m);
    }

    /** CommonServices.WagesDeleteByReferenceIds (:2411, :3968, :10266). */
    public void wagesDeleteByReferenceIds(int org, int comp, int documentTypeId, int id) {
        LinkedHashMap<String, Object> m = p();
        m.put("@OrganizationId", org); m.put("@CompanyId", comp);
        m.put("@DocumentTypeId", documentTypeId); m.put("@Id", id);
        scalar("dbo.usp_ContractorWagesRemovebyReferenceIds", m);
    }
}
