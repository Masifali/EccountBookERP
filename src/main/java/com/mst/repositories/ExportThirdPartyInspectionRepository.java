package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 857 "Third Party Inspection" (Architecture.WinApp.Export.frmThirdPartyInspection) and
 * 858 "Lab Against Third Party Inspection" (frmLabAgainstThirdPartyInspection). Both forms edit the
 * same record (InvLabPreProductionExportLotInspectionHeader + Detail + ParametersDetail +
 * ThirdPartyInspectionLotSamplingDetail) through BLL 0472 / DAL 0524
 * InvLabPreProductionExportLotInspectionHeader. Every call is the desktop's own procedure with the
 * desktop's own parameters (procdure.utf8.sql, 23-Sep-2026):
 *
 *   Sp_ConfigrationsAllocation_GetAllMethod                 @OrganizationId @CompanyId @ConfigDescription @Activity   clsGlobalVariables.configrationsAllocation
 *   Sp_ExImLookUps_GetAllMethod                             @Activity='ReadByOrganizationCompanyIdNExImLookUpTypeId' @OrganizationId @CompanyId   ExImLookUps.GetByLookTypeId (types 8/11/12/13/14/15)
 *   USP_GetDataForDropDownFromExportContract                @OrganizationId @CompanyId @Activity='Customer'           857 CmbCustomer
 *   SP_Country_ReadMethod                                   @OrganizationId @CompanyId @MethodType='GetAll'           country.GetAll
 *   USP_GetDataForDropDownFrom_LabPreExportLotInspection    @OrganizationId @CompanyId                                history combos (Items / TrackingNo)
 *   Sp_InvLabAnalysisGroup_GetAllMethod                     @OrganizationId @CompanyId @Activity='ReadAll'            InvLabAnalysisGroup.GetAllOrById
 *   usp_getFarmingNTrade                                    (none)                                                    ExImLcOrder.GetFarmingNTrade
 *   USP_InvLabPreProductionExportLotInspectionHeader_MostUsedIds @OrganizationId @CompanyId @FinancialYearId
 *   USP_Item_AllItemsWithModal                              @OrganizationId @CompanyId                                clsGlobalVariables.getGlobalAllItems
 *   SP_JobLot_ReadMethod                                    @OrganizationId @CompanyId @Activity='GetJobLotGlIdsandName'  clsGlobalVariables.globalJobLot
 *   usp_InvLabPreProductionExportLotInspectionHeader_FormHistory  guarded, see formHistory()             tracking-no combo and History grid
 *   USP_ExImLcOrder_PendingDataLoaderForThirdPartyLab       @OrganizationId @CompanyId [@SupplierCustomerId] [@RecId] 857 cmbContractNo
 *   USP_GetInvoicesDataForShipmentAnalysis                  @OrganizationId @CompanyId [@SupplierCustomerId] [@ContractId]
 *   USP_GetContractSchedulesDataForShipmentAnalysis         @OrganizationId @CompanyId [@SupplierCustomerId] [@ContractId]
 *   usp_ExportContract_BalanceByIds                         @OrganizationId @CompanyId @ContractIds                   857 grdContractInfo
 *   Sp_InvLabGroupAnalysisStandards_GetAllMethod            @Id @OrganizationId @CompanyId @Activity='GetParametersFromGroupStandards'
 *   USP_ThirdPartyType_GetAllMethod                         @OrganizationId @CompanyId [@IsActive] @Activity='FormHistory'
 *   Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod  @Id @Activity='ReadById' / @InvLabPreProductionExportLotInspectionHeaderId
 *                                                           @Activity='ReadByHeaderId' | 'ReadByHeaderId_ParameterDetail' | 'ReadByHeaderId_ThirdPartyInspectionLotSamplingDetail'
 *   USP_InvLabPreProductionExportLotInspectionHeader_InsertAndUpdate   every non-virtual header model property (SetProc)
 *   usp_InvLabPreProductionExportLotInspectionDetailDeletebyIds        @Id @DetailIds         (857 only, RemoveDetailIds)
 *   Sp_InvLabPreProductionExportLotInspectionDetail_Insert              detail model (SetProc)
 *   USP_InvLabPreProductionExportLotInspectionParametersDetail_Insert   parameter model (SetProc)
 *   [dbo].[USP_ThirdPartyInspectionLotSamplingDetail_Insert]           sampling model (SetProc)
 *   Sp_DMSAttachments_GetAllMethod                          @ScreenName @Id @Activity='ReadById'                      NoOfAttachments link (DMSAttachments.GetByID)
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportThirdPartyInspectionRepository {

    private final JdbcTemplate jdbc;

    public ExportThirdPartyInspectionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** ExImLookUps.GetByLookTypeId(org, comp) - ExImLookUptypesId 0 so it is not sent. */
    public List<Map<String, Object>> lookups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", params(
                "Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImLcOrder.GetDataForDropDownFromExportContract(Activity "Customer"). */
    public List<Map<String, Object>> contractCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportContract]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Customer"));
    }

    /** country.GetAll. */
    public List<Map<String, Object>> countries(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** GetDataForDropDownFrom_LabPreExportLotInspection(Activity null - not sent). */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFrom_LabPreExportLotInspection]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** InvLabAnalysisGroup.GetAllOrById - Id / ParentCategoryId 0 so not sent. */
    public List<Map<String, Object>> analysisGroups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> farmingNTrade() {
        return DesktopProc.rows(jdbc, "usp_getFarmingNTrade", params());
    }

    public List<Map<String, Object>> mostUsed(UserAccount u, int financialYearId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvLabPreProductionExportLotInspectionHeader_MostUsedIds]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId));
    }

    public List<Map<String, Object>> allItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[SP_JobLot_ReadMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetJobLotGlIdsandName"));
    }

    /**
     * InvLabPreProductionExportLotInspectionHeader.FormHistory(ReportsParameters): @OrganizationId
     * @CompanyId @ScreenName @FinancialYearId @CanViewAllRecord always; @EntryUser when not
     * CanViewAllRecord; the date pairs / @Id / @ItemId / @ActionId only when set (the caller passes
     * only the guarded ones). ApprovedFilter "All" - @IsApproved never sent.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, String screenName,
                                                 boolean canViewAll, Map<String, Object> guarded) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ScreenName", screenName, "FinancialYearId", financialYearId, "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.putAll(guarded);
        return DesktopProc.rows(jdbc, "[dbo].[usp_InvLabPreProductionExportLotInspectionHeader_FormHistory]", p);
    }

    /** ExImLcOrder.PendingDataLoaderForThirdPartyLab - @SupplierCustomerId / @RecId only when non-zero. */
    public List<Map<String, Object>> pendingContracts(UserAccount u, int customerId, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (recId != 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImLcOrder_PendingDataLoaderForThirdPartyLab]", p);
    }

    /** ExImInvoice.GetInvoicesDataForShipmentAnalysis - @SupplierCustomerId / @ContractId only when non-zero. */
    public List<Map<String, Object>> invoicesForShipment(UserAccount u, int supplierCustomerId, int contractId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (contractId != 0) p.put("ContractId", contractId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetInvoicesDataForShipmentAnalysis]", p);
    }

    /** ExImInvoice.GetContractSchedulesDataForShipmentAnalysis - same guards. */
    public List<Map<String, Object>> schedulesForShipment(UserAccount u, int supplierCustomerId, int contractId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (contractId != 0) p.put("ContractId", contractId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetContractSchedulesDataForShipmentAnalysis]", p);
    }

    /** ExImLcOrder.ExportContractBalanceByIds. */
    public List<Map<String, Object>> contractBalance(UserAccount u, String contractIds) {
        return DesktopProc.rows(jdbc, "usp_ExportContract_BalanceByIds", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ContractIds", contractIds));
    }

    /** InvLabGroupAnalysisStandards.GetParametersFromGroupStandards. */
    public List<Map<String, Object>> groupParameters(UserAccount u, int groupId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabGroupAnalysisStandards_GetAllMethod", params(
                "Id", groupId, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetParametersFromGroupStandards"));
    }

    /** ThirdPartyType.FormHistory(org, comp, 0, ActiveOnly) - @IsActive only when ActiveOnly. */
    public List<Map<String, Object>> thirdPartyTypes(UserAccount u, boolean activeOnly) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (activeOnly) p.put("IsActive", true);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "USP_ThirdPartyType_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ GetByID (DAL GetAll)

    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod",
                params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod",
                params("InvLabPreProductionExportLotInspectionHeaderId", id, "Activity", "ReadByHeaderId"));
    }

    public List<Map<String, Object>> parameterDetails(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod",
                params("InvLabPreProductionExportLotInspectionHeaderId", id, "Activity", "ReadByHeaderId_ParameterDetail"));
    }

    public List<Map<String, Object>> samplingDetails(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod",
                params("InvLabPreProductionExportLotInspectionHeaderId", id, "Activity", "ReadByHeaderId_ThirdPartyInspectionLotSamplingDetail"));
    }

    /** DMSAttachments.GetByID(Id, ScreenName). */
    public List<Map<String, Object>> attachments(int id, String screenName) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod",
                params("ScreenName", screenName, "Id", id, "Activity", "ReadById"));
    }

    // ------------------------------------------------------------------ SetData (one transaction)

    /**
     * DAL InvLabPreProductionExportLotInspectionHeader.SetData: the header through
     * USP_..._InsertAndUpdate (the returned id, or obj.Id when 0 comes back), then - only when
     * RemoveDetailIds is not empty - usp_..._DetailDeletebyIds(@Id, @DetailIds), then every detail
     * row, every checked parameter (RequiredAnalysisGroupId = header RequiredAnalysisId) and every
     * sampling row with the header id. The attachment branch is skipped because the web form sends
     * no attachment list (AttachmentsList empty on the desktop too when nothing was attached).
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, String removeDetailIds, List<Map<String, Object>> details,
                    List<Map<String, Object>> parameters, List<Map<String, Object>> samplings) {
        int num = DesktopProc.setProc(jdbc, "USP_InvLabPreProductionExportLotInspectionHeader_InsertAndUpdate", header);
        int id;
        if (num > 0) id = num;
        else { Object o = header.get("Id"); id = o instanceof Number ? ((Number) o).intValue() : 0; num = id; }
        if (removeDetailIds != null && !removeDetailIds.isEmpty()) {
            DesktopProc.rows(jdbc, "usp_InvLabPreProductionExportLotInspectionDetailDeletebyIds",
                    params("Id", id, "DetailIds", removeDetailIds));
        }
        for (Map<String, Object> d : details) {
            d.put("InvLabPreProductionExportLotInspectionHeaderId", id);
            DesktopProc.setProc(jdbc, "Sp_InvLabPreProductionExportLotInspectionDetail_Insert", d);
        }
        Object reqAnalysis = header.get("RequiredAnalysisId");
        for (Map<String, Object> p : parameters) {
            p.put("InvLabPreProductionExportLotInspectionHeaderId", id);
            p.put("RequiredAnalysisGroupId", reqAnalysis);
            DesktopProc.setProc(jdbc, "USP_InvLabPreProductionExportLotInspectionParametersDetail_Insert", p);
        }
        for (Map<String, Object> s : samplings) {
            s.put("InvLabPreProductionExportLotInspectionHeaderId", id);
            DesktopProc.setProc(jdbc, "[dbo].[USP_ThirdPartyInspectionLotSamplingDetail_Insert]", s);
        }
        return num;
    }

    public static List<Map<String, Object>> safe(List<Map<String, Object>> l) { return l == null ? new ArrayList<>() : l; }
}
