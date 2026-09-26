package com.mst.repositories;

import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 280 "Production Against (Job Order)" - the Overhead tab.
 *
 * Desktop form: Architecture.WinApp.Production/frmProductionOverhead.cs (3,390 lines), hosted by
 * FoodProductionWithValues in PanelOtherForms (:880). Header DocumentTypeId 110.
 *
 * Every call below names the desktop line that makes it and the BLL/DAL that shapes it. Each
 * parameter is one the procedure declares (checked against procdure.utf8.sql) and each guarded
 * parameter is omitted exactly when the BLL omits it. Nothing here creates or alters a table.
 *
 * The form does NOT open LoadOverHeadsDataByJobOrder.cs - nothing in src280 constructs it. Its
 * two popups are GrdPopUpToReturndt (FOH Expenses) and FormulasPopUp (Formulas).
 */
@Repository
public class ProductionOverheadRepository {

    /** frmProductionOverhead.OverHeadInsert:823 - OverHeaddetail.DocumentTypeId = 110. */
    public static final int DOC_TYPE_ID = 110;

    /** frmFoodProduction_Load:223-230. */
    public static final String SHELL_SCREEN_NAME = "FoodProductionWithValues";
    public static final String OWN_SCREEN_NAME   = "frmProductionOverhead";

    private static final String P_OH       = "Sp_InvFoodProductionOverHeads_GetAllMethod";
    private static final String P_FOOD     = "Sp_InvFoodProduction_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ProductionOverheadRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =============================================================================== plumbing

    public static Map<String, Object> params() { return new LinkedHashMap<>(); }

    /** EXEC dbo.Proc @A=?, @B=? - parameters in the order the BLL adds them. */
    private List<Map<String, Object>> exec(String proc, Map<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(qualify(proc));
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    private static String qualify(String proc) {
        return proc.startsWith("[") || proc.contains(".") ? proc : "dbo." + proc;
    }

    /**
     * SqlCommand.ExecuteScalar as the DAL uses it inside its transaction: the first column of the
     * first row of the first result set, or null. Every later result is drained so that a
     * RAISERROR raised after the first SELECT (USP_UserAudit_Insert runs after "SELECT @Id")
     * still surfaces and rolls the transaction back, as SqlDataReader.Close does on the desktop.
     *
     * A null value is left out of the call rather than sent: ADO.NET's AddWithValue(name, null)
     * does not transmit the parameter at all, so the procedure's own default applies.
     */
    public Object scalar(String proc, Map<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(qualify(proc));
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (e.getValue() == null) continue;
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        final String text = sql.toString();
        return jdbc.execute((ConnectionCallback<Object>) con -> {
            try (PreparedStatement ps = con.prepareStatement(text)) {
                for (int i = 0; i < values.size(); i++) ps.setObject(i + 1, values.get(i));
                boolean isResult = ps.execute();
                Object value = null;
                boolean captured = false;
                while (true) {
                    if (isResult) {
                        try (ResultSet rs = ps.getResultSet()) {
                            if (!captured) {
                                captured = true;
                                if (rs.next()) value = rs.getObject(1);
                            }
                            while (rs.next()) { /* drain */ }
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    isResult = ps.getMoreResults();
                }
                return value;
            }
        });
    }

    // ================================================================= rights and switches

    /**
     * clsGlobalVariables.ScreenViewReights - filled at login from USP_GetUserRightsForViewbyUserId
     * (BLL :276-313, @AppId / @AppModuleId appended only when non-zero). Load:223 only asks whether
     * any row carries ScreenName "FoodProductionWithValues".
     *
     * The same call DashboardModuleService makes. Note the procedure also stamps
     * LoginUserAndAppInfo (who is in which app) - the desktop does that once at login.
     */
    public List<Map<String, Object>> screenViewRights(int userId, int companyId, int appId) {
        Map<String, Object> p = params();
        p.put("@UserId", userId);
        p.put("@CompanyId", companyId);
        if (appId != 0) p.put("@AppId", appId);
        return exec("USP_GetUserRightsForViewbyUserId", p);
    }

    /** CommonServices.SetRightsValueInRightsObject -> tblUserRights.GetByUserId (BLL): all four unconditional. */
    public List<Map<String, Object>> userRights(int userId, String screenName, String roleName, int companyId) {
        Map<String, Object> p = params();
        p.put("@UserId", userId);
        p.put("@ScreenName", screenName);
        p.put("@RightName", roleName == null ? "" : roleName);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "GetByUserId");
        return exec("Sp_tblUserRights_GetAllMethod", p);
    }

    /**
     * DAL CommonServices.GetConfigurationFromAllocation(org, comp, name) - SetData reads
     * "InventoryFinancialsEffectsInActive" through it; ConfigKey of the first row, else "".
     */
    public String configKey(int organizationId, int companyId, String description) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@ConfigDescription", description);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> rows = exec("Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (rows.isEmpty()) return "";
        Object v = col(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // ================================================================================ lookups

    /**
     * GenerateCodeofOverHead:546 -> InvFoodProductionOverHeads.GenerateCode (BLL): four
     * unconditional parameters, @BranchesId carrying the model's BranchId, activity ReadSerialNumber,
     * column DocNo.
     */
    public int generateCode(int organizationId, int companyId, int financialYearId, int branchesId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@FinancialYearId", financialYearId);
        p.put("@BranchesId", branchesId);
        p.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> rows = exec(P_OH, p);
        return rows.isEmpty() ? 0 : toInt(col(rows.get(0), "DocNo"));
    }

    /** JobOrderNoFill:352 -> InvProductionJobOrder.JobOrdersForProduction: three unconditional. */
    public List<Map<String, Object>> jobOrders(int organizationId, int companyId, int branchesId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@BranchesId", branchesId);
        return exec("usp_getJobOrdersForProduction", p);
    }

    /** GetPlantFeeder:397 -> InvProductionJobOrder.PlantsForProductionByJobOrderId: five unconditional. */
    public List<Map<String, Object>> plants(int organizationId, int companyId, int financialYearId,
                                            int branchesId, int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@FinancialYearId", financialYearId);
        p.put("@BranchesId", branchesId);
        p.put("@JobOrderId", jobOrderId);
        return exec("usp_getPlantsForProductionByJobOrderId", p);
    }

    /**
     * ChartOfAccountOverHead:588 -> CommonServices.CoaAllocationGetAllServiceBind (:778) ->
     * COAAllocation.GetAll (BLL): @OrganizationId, @CompanyId, @UserId only when non-zero,
     * @Activity='COAAllocationSearch'. The account-type filter is the form's, applied by the caller.
     */
    public List<Map<String, Object>> coaAllocation(int organizationId, int companyId, int userId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        if (userId != 0) p.put("@UserId", userId);
        p.put("@Activity", "COAAllocationSearch");
        return exec("Sp_COAAllocation_GetAllMethod", p);
    }

    /** UOMOverHead:621 -> UOM.UOMStaticAll (BLL): [dbo].[USP_UOMStatic_GetAllMethod], no parameters. */
    public List<Map<String, Object>> uomStatic() {
        return jdbc.queryForList("EXEC [dbo].[USP_UOMStatic_GetAllMethod]");
    }

    /**
     * GetBrandItemsForPackingAndOverHeads:281 (BLL): @OrganizationId, @CompanyId, @InvJobOrderId
     * always; @ActionId and @DocumentTypeId only when non-zero. The form never passes an ActionId,
     * so @ActionId is never sent from this screen.
     */
    public List<Map<String, Object>> brandItems(int organizationId, int companyId, int jobOrderId,
                                                int actionId, int documentTypeId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@InvJobOrderId", jobOrderId);
        if (actionId != 0) p.put("@ActionId", actionId);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        p.put("@Activity", "GetBrandItemsForPackingAndOverHeads");
        return exec(P_FOOD, p);
    }

    /** GetItemUomByItemIdFromFinishGoods:311 (BLL): four unconditional, @ItemId carrying WIPItemId. */
    public List<Map<String, Object>> brandUoms(int organizationId, int companyId, int jobOrderId, int itemId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@InvJobOrderId", jobOrderId);
        p.put("@ItemId", itemId);
        p.put("@Activity", "GetItemUomByItemIdFromFinishGoods");
        return exec(P_FOOD, p);
    }

    /**
     * GetTotlInPutOut:667 -> InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId (BLL).
     * @ItemId, @PlantId, @ItemUomId are guarded on non-zero and @EntryType on a non-empty
     * Activity; the form sets none of them, so only the three ids and the activity travel.
     */
    public List<Map<String, Object>> inputOutputTotals(int organizationId, int companyId, int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@InvJobOrderId", jobOrderId);
        p.put("@Activity", "GetInPutTotalQtyandWeightByJobOrderId");
        return exec(P_FOOD, p);
    }

    /**
     * GetOutPutQtyByJobOrderandItemIdOverHeads:748 -> InvFoodProduction.GetOutPutQtyWeightFoodProductionForPmOh
     * (BLL): @ItemId, @ItemUomId, @DocumentTypeId each only when non-zero.
     */
    public List<Map<String, Object>> outputQty(int organizationId, int companyId, int jobOrderId,
                                               int brandId, int brandUomId, int documentTypeId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@InvJobOrderId", jobOrderId);
        if (brandId != 0) p.put("@ItemId", brandId);
        if (brandUomId != 0) p.put("@ItemUomId", brandUomId);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        return exec("[dbo].[USP-GetOutPutQtyWeightFoodProductionForPmOh]", p);
    }

    /**
     * BtnLoadFohOverHeads_Click:1779 -> ProdcutionFohAllocateToJobOrder_DataForOverHeads (BLL):
     * @BranchesId only when non-zero, everything else unconditional.
     */
    public List<Map<String, Object>> fohExpenses(int organizationId, int companyId, int financialYearId,
                                                 int branchesId, int plantId, int jobOrderId,
                                                 java.time.LocalDate docDate) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        p.put("@PlantId", plantId);
        p.put("@JobOrderId", jobOrderId);
        p.put("@DocDate", java.sql.Date.valueOf(docDate));
        return exec("[dbo].[USP_ProdcutionFohAllocateToJobOrder_DataForOverHeads]", p);
    }

    // =================================================================================== reads

    /**
     * InvFoodProductionOverHeads.GetByJobOrderID (BLL): ONLY @Activity='ReadByjobProductionId'
     * and @Id - no organisation or company. Used by ReadByIdOverHeadJobOrderWise:950 and by the
     * history detail grid (grdOverHeadHistory_SelectionChanged:1478).
     */
    public List<Map<String, Object>> byJobOrder(int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@Activity", "ReadByjobProductionId");
        p.put("@Id", jobOrderId);
        return exec(P_OH, p);
    }

    /**
     * BindGridHistoryOverHead:1338 -> InvFoodProductionOverHeads.GetAll (BLL): @FinancialYearId
     * and @BranchesId only when non-zero, activity ReadAll.
     */
    public List<Map<String, Object>> history(int organizationId, int companyId, int financialYearId, int branchesId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        p.put("@Activity", "ReadAll");
        return exec(P_OH, p);
    }

    /**
     * CommonServices.VoucherHeadIdGet(Id, DocumentTypeId) (:9446) -> VoucherHead
     * .GetVoucherHeadIdByDocumentTypeIdandRefDocNoId (BLL): Sp_Vouchers_GetMethods with the
     * activity GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId; column Id of the first row.
     */
    public int voucherHeadId(int organizationId, int companyId, int documentTypeId, int documentTypeSrNo) {
        Map<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", documentTypeSrNo);
        List<Map<String, Object>> rows = exec("Sp_Vouchers_GetMethods", p);
        return rows.isEmpty() ? 0 : toInt(col(rows.get(0), "Id"));
    }

    /**
     * btnPrintOverHead_Click:1583 -> InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder
     * (BLL): @ReportType always, @JobOrderId and @Id only when non-zero.
     */
    public List<Map<String, Object>> report606(int organizationId, int companyId, int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@ReportType", "OverHead");
        if (jobOrderId != 0) p.put("@JobOrderId", jobOrderId);
        return exec("Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder", p);
    }

    // ============================================================= DAL SetData (the write path)

    /**
     * GenericProvider.SetProc(tran, row, "Sp_InvFoodProductionOverHeads_Insert"/"_Update", "")
     * - every non-virtual property of Architecture.Model.Production.InvFoodProductionOverHeads
     * becomes @PropertyName. The model has 26 of them (OHDetailRowsRemoveIds and the list are
     * virtual); all 26 are declared by both procedures, which also declare SupplierCustomerId,
     * BranchSrNo and UserLogId with defaults the model never supplies.
     */
    public int saveOverheadRow(boolean insert, Map<String, Object> modelParams) {
        Object r = scalar(insert ? "Sp_InvFoodProductionOverHeads_Insert" : "Sp_InvFoodProductionOverHeads_Update",
                          modelParams);
        return toInt(r);
    }

    /** SetData IL_01ce: the existing voucher of an updated row, by ExecuteScalar in the transaction. */
    public int voucherHeadIdInTransaction(int organizationId, int companyId, int documentTypeId, int rowId) {
        Map<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", rowId);
        return toInt(scalar("Sp_Vouchers_GetMethods", p));
    }

    /**
     * SetData IL_0292: the job order's WIP account - Sp_InvProductionJobOrder_GetAllMethod
     * @OrganizationId, @CompanyId, @Id (the row's InvProductionJobOrderId),
     * @Activity='GetGlAccountsByJobOrderId'. Column WorkInProccessAcId of the first row.
     */
    public List<Map<String, Object>> glAccountsByJobOrderId(int organizationId, int companyId, int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@Id", jobOrderId);
        p.put("@Activity", "GetGlAccountsByJobOrderId");
        return exec("Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /** SetData IL_0560: Sp_VoucherHead_Insert when no voucher was found, else _Update (SetProc). */
    public int saveVoucherHead(boolean insert, Map<String, Object> voucherHead) {
        return toInt(scalar(insert ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucherHead));
    }

    /** SetData IL_05d8: Sp_VoucherDetail_Insert per line (SetProc). */
    public void insertVoucherDetail(Map<String, Object> voucherDetail) {
        scalar("Sp_VoucherDetail_Insert", voucherDetail);
    }

    /** SetData IL_0630: USP_VoucherBalanceCheck @OrganizationId, @CompanyId, @Id - ExecuteScalar. */
    public void voucherBalanceCheck(int organizationId, int companyId, int voucherHeadId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@Id", voucherHeadId);
        scalar("USP_VoucherBalanceCheck", p);
    }

    /**
     * The complete non-null parameter maps for Architecture.Model.Accounts.VoucherHead and
     * VoucherDetail - every C# value-type property at its CLR default - already established for
     * the other voucher writers in this package. Reused so the writers do not drift apart.
     * Keys carry no "@"; the caller prefixes them.
     */
    public static Map<String, Object> voucherHeadDefaults()   { return InventoryOpeningDefaults.voucher(); }
    public static Map<String, Object> voucherDetailDefaults() { return InventoryOpeningDefaults.detail(); }

    // ================================================================================ helpers

    /** SQL Server column names are case-insensitive to the desktop; JdbcTemplate's map is not. */
    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt: null and unparsable text become 0. */
    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Math.rint(Double.parseDouble(s.replace(",", ""))); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble: null and unparsable text become 0. */
    public static double toDouble(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }
}
