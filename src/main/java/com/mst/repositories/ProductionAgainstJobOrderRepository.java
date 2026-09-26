package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 280 "Production Against (Job Order)" — the shell's own data contracts.
 *
 * Desktop form: Architecture.WinApp.Production/FoodProductionWithValues.cs (6,497 lines).
 * ScreenName `FoodProductionWithValues`, ScreenAlias "Production Against (Job Order)",
 * ModuleId 18 — confirmed in migration/user-rights/reconciliation-input.json.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THIS SCREEN IS
 * ---------------------------------------------------------------------------------------------
 * It is NOT one form. It is a shell with a tab strip, and six of its eight tabs host a SEPARATE
 * desktop form constructed at runtime and dropped into `PanelOtherForms`
 * (tabControl1_SelectedIndexChanged:839). See claude/SCREEN-280-IS-A-CONTAINER-OF-SEVEN-FORMS.md.
 *
 * This repository covers the SHELL only: the seven switches that decide what the screen may show,
 * and the pickers every tab shares. Nothing here writes.
 *
 * ---------------------------------------------------------------------------------------------
 * EVERY SWITCH IS READ FROM THE DATABASE
 * ---------------------------------------------------------------------------------------------
 * frmFoodProduction_Load:452 reads seven flags, and their effects are structural rather than
 * cosmetic — `IsManualEntryOnInputNotAllowed` hides a whole panel, `FIFOCGSFlag` disables the
 * Rate box and hides a button, `DoHaveCanSettlementtab` REMOVES two tabs. None of them may be
 * hardcoded, so each is traced to its procedure below.
 */
@Repository
public class ProductionAgainstJobOrderRepository {

    /** frmFoodProduction_Load:490 — CommonServices.SetRightsValueInRightsObject(base.Name). */
    public static final String DESKTOP_SCREEN_NAME = "FoodProductionWithValues";

    @Autowired private JdbcTemplate jdbc;

    /* ------------------------------------------------------------------ the seven switches */

    /**
     * clsGlobalVariables.WagesRefDocumentsStatusList.
     *
     * DashboardNew.cs:1380 fills it once at start-up from
     * InvContractorWagesBillHeader.GetRefDocumentsForWages(0), which is
     * [dbo].[USP_GetRefDocumentsForWages] with @RefDocumentTypeId GUARDED — the BLL adds the
     * parameter only when it is non-zero, and the desktop passes 0, so the parameter is OMITTED
     * and every row comes back. Sending 0 explicitly would filter on RefDocumentTypeId = 0 and
     * return nothing.
     *
     * The shell then picks two rows out of that list by id (:459, :461):
     *   112 -> WagesActiveOrInActiveForOutPut
     *   181 -> WagesActiveOrInActiveForConsumption
     */
    public List<Map<String, Object>> wagesRefDocumentStatuses() {
        return jdbc.queryForList("EXEC [dbo].[USP_GetRefDocumentsForWages]");
    }

    /**
     * CommonServices.GetMultipleConfigurationsByConfigDescriptions (:465) — ONE call returning
     * four configuration rows:
     *
     *   WagesCompulsoryOnProduction    -> WagesStatus
     *   IssuanceByLoader               -> LoaderStatus
     *   IsManualEntryOnInputNotAllowed -> IsManualEntryOnInputNotAllowed
     *   ByProductRateEditableIsAllow   -> ByProductRateEditAbleOrNot
     *
     * BLL 0621:131 — Sp_ConfigrationsAllocation_GetAllMethod with
     * @Activity='GetMultipleConfigurationsByConfigDescriptions' and the comma-separated list in
     * @ConfigDescription. The desktop's spelling of each name is reproduced exactly; note
     * "ByProductRateEditableIsAllow" in the request versus the field it sets,
     * `ByProductRateEditAbleOrNot` — they differ, and the request string is the one that matters.
     */
    public List<Map<String, Object>> multipleConfigurations(int organizationId, int companyId,
                                                            String commaSeparatedDescriptions) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                organizationId, companyId, commaSeparatedDescriptions,
                "GetMultipleConfigurationsByConfigDescriptions");
    }

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name) (:463, :464) — the single-value form,
     * used for SaleCostingJobOrderWise and OutputItemsByJobOrderRateSchedule.
     */
    public String configValue(int organizationId, int companyId, String configDescription) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                organizationId, companyId, configDescription,
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return "";
        Object v = col(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /**
     * CommonServices.GetERPFeatureById(5) -> FIFOCGSFlag (:516).
     *
     * BLL 0269:372 — USP_GetERPFeaturesByCompanyId returns the company's feature rows and the
     * flag is "is feature 5 among them".
     */
    public boolean erpFeature(int organizationId, int companyId, int featureId) {
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                organizationId, companyId)) {
            Object id = col(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /** The grant grid for this user on this screen — Sp_tblUserRights_GetAllMethod. */
    public List<Map<String, Object>> userRightsForScreen(int userId, String screenName,
                                                         String roleName, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
              + "@CompanyId=?, @Activity=?",
                userId, screenName, roleName == null ? "" : roleName, companyId, "GetByUserId");
    }

    /* ------------------------------------------------------------------- the shared pickers */

    /**
     * Production Department — CommonServices.GetActiveWareHouseByWareHouseType(2).
     *
     * BLL 0582:141 — Sp_InvWareHouse_GetAllMethod, @Activity='GetActiveWareHouseByWareHouseType'.
     * @WarehouseType carries the ActivityId, and 2 is the Production warehouse type on this
     * screen. @WarehouseTypeIds and @BranchesIds are guarded in the BLL and are not sent here
     * because the shell does not set them.
     */
    public List<Map<String, Object>> productionDepartments(int organizationId, int companyId,
                                                           int warehouseType) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@WarehouseType=?, @Activity=?",
                organizationId, companyId, warehouseType, "GetActiveWareHouseByWareHouseType");
    }

    /**
     * Job Order picker — InvProductionJobOrder.JobOrdersForProduction (BLL 0335:1582)
     * -> usp_getJobOrdersForProduction, three parameters, all unconditional.
     */
    public List<Map<String, Object>> jobOrdersForProduction(int organizationId, int companyId,
                                                            int branchesId) {
        return jdbc.queryForList(
                "EXEC dbo.usp_getJobOrdersForProduction @OrganizationId=?, @CompanyId=?, @BranchesId=?",
                organizationId, companyId, branchesId);
    }

    /**
     * Plant picker — InvProductionJobOrder.PlantsForProductionByJobOrderId (BLL 0335:1654)
     * -> usp_getPlantsForProductionByJobOrderId, five parameters in the BLL's own order.
     */
    public List<Map<String, Object>> plantsForJobOrder(int organizationId, int companyId,
                                                        int financialYearId, int branchesId,
                                                        int jobOrderId) {
        return jdbc.queryForList(
                "EXEC dbo.usp_getPlantsForProductionByJobOrderId @OrganizationId=?, @CompanyId=?, "
              + "@FinancialYearId=?, @BranchesId=?, @JobOrderId=?",
                organizationId, companyId, financialYearId, branchesId, jobOrderId);
    }

    /**
     * cmbJobOrderConsumption_Leave:1433 -> InvProductionJobOrder.GetGlAccountsByJobOrderId:
     * Sp_InvProductionJobOrder_GetAllMethod @OrganizationId, @CompanyId, @Id,
     * @Activity='GetGlAccountsByJobOrderId' (all four unconditional in the IL).
     * Columns used: WorkInProccessAcId / WorkInProcessAc, WipItemId / ItemName.
     */
    public List<Map<String, Object>> glAccountsByJobOrderId(int organizationId, int companyId, int jobOrderId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvProductionJobOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                organizationId, companyId, jobOrderId, "GetGlAccountsByJobOrderId");
    }

    /**
     * UOMSchedule.Getall (UOMFill on this form) - the list the shell caches in `dtuomlst` and
     * every tab reads.
     *
     * BLL Inventory.UOMSchedule::Getall is Sp_UOMSchedule_GetAllMethod with @OrganizationId,
     * @CompanyId and @Activity='ReadByOrganizationCompanyId' (IL, Architecture.BLL.Inventory).
     *
     * Pass 2 correction: this used usp_getAllUomsByCompanyId, which is a different list. That
     * procedure filters on CompanyId, INNER JOINs Item and sorts by BaseRateUom. The desktop's
     * activity filters on OrganizationId only (its CompanyId line is commented out in the
     * procedure), LEFT JOINs both Item and ItemPartyProcessing, and returns ItemCode,
     * ScheduleUnitId and UOMDescription as well. Anything later reading "the first base-rate
     * row" out of this list would pick differently from the desktop under the old order.
     */
    public List<Map<String, Object>> uomSchedules(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadByOrganizationCompanyId");
    }

    /**
     * CommonServices.VoucherHeadIdGet(Id, DocumentTypeId) (CommonServices.cs:11040) — the voucher
     * behind a saved document.
     *
     * It delegates to VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId (BLL 0654), which
     * is `Sp_Vouchers_GetMethods` with
     * @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId' — note that the activity
     * name does NOT match the BLL method name, and the document id travels as @DocumentTypeSrNo,
     * not @RefDocNoId. Both are easy to get wrong from the method name alone.
     */
    public Integer voucherHeadId(int organizationId, int companyId,
                                 int documentTypeId, int documentTypeSrNo) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                organizationId, companyId, documentTypeId, documentTypeSrNo);
        if (rows.isEmpty()) return 0;
        Object v = col(rows.get(0), "Id");
        if (v == null) v = rows.get(0).values().iterator().next();
        return (v instanceof Number) ? ((Number) v).intValue() : 0;
    }

    /* ------------------------------------------------------------------------------ helpers */

    /** SQL Server result keys are case-insensitive to the desktop; JdbcTemplate's are not. */
    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    public static List<Map<String, Object>> project(List<Map<String, Object>> rows,
                                                    String idCol, String textCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", col(r, idCol));
            m.put("name", col(r, textCol));
            out.add(m);
        }
        return out;
    }
}
