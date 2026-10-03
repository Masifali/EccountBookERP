package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 206 "Export Parties Define" - Architecture.WinApp.Export.ExportPartyDefines (App 8 "Export", ModuleId 11).
 * Data layer of the desktop form: Architecture.BLL.Inventory.SupplierCustomer (BLL 0600 / DAL 0453),
 * CommonServices.CoaAllocationAccountTitleByAccountTypeIds (COAAllocation BLL), country (BLL 0064),
 * StateProvince (BLL 0083), City (BLL 0060) and clsGlobalVariables.globalAllSupplierCustomer
 * (GlobalServicesMethods.getGlobalSupplierCustomer, BLL 0379). Procedures, read from procdure.utf8.sql:
 *
 *   Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'  @OrganizationId @CompanyId @AppId @AccountTypeIds='22' @UserId @NotReferred=1 [@RecId]
 *   USP_GetVendorsAndCustomersWithCityName                           @OrganizationId @CompanyId      (globalAllSupplierCustomer; the form keeps CustomerGroupId == 7)
 *   SP_Country_ReadMethod 'GetAll'                                   @OrganizationId @CompanyId @MethodType
 *   Proc_StateProvince_GetAllMethod 'ReadAll'                        @Activity @OrganizationId @CompanyId
 *   SP_City_GetAllMethod 'GetAll'                                    @OrganizationId @CompanyId @MethodType
 *   Sp_SupplierCustomer_GetAllMethod 'FormHistory'                   @OrganizationId @CompanyId @CustomerGroupId=7 @Activity
 *   Sp_SupplierCustomer_GetAllMethod 'ReadById'                      @Id @Activity
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId'   @OrganizationId @CompanyId @Activity   (cmbglac_Leave lookup)
 *   Sp_SupplierCustomer_Insert / Sp_SupplierCustomer_Update          the 58 non-virtual model properties (SetProc)
 *   [dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]          @OrganizationId @CompanyId   (DAL SetData, same transaction)
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportPartyDefinesRepository {

    private final JdbcTemplate jdbc;

    public ExportPartyDefinesRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds("22", null, 0, "", "", 1, GlrecId). */
    public List<Map<String, Object>> glAccounts(UserAccount u, int recId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", u.getAppId(),
                "AccountTypeIds", "22", "UserId", u.getId(), "NotReferred", 1);
        if (recId != 0) p.put("RecId", recId);
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /** clsGlobalVariables.globalAllSupplierCustomer - DatatableHelper.GlobalSupplierCustomerListsFillDbCall(org, comp). */
    public List<Map<String, Object>> globalAllSupplierCustomer(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** country.GetAll(country) with the user's organisation / company. */
    public List<Map<String, Object>> countries(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** StateProvince.GetAll - Proc_StateProvince_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> provinces(UserAccount u) {
        return DesktopProc.rows(jdbc, "Proc_StateProvince_GetAllMethod",
                params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** City.GetAll - SP_City_GetAllMethod 'GetAll'. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_City_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** SupplierCustomer.FormHistory(org, comp, CustomerGroupId 7) - the party and the consignee grids both read it. */
    public List<Map<String, Object>> formHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CustomerGroupId", 7, "Activity", "FormHistory"));
    }

    /** SupplierCustomer.GetByID - the BLL takes [0], so an unknown id throws as on the desktop. */
    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /** CommonServices.SupplierCustomerGetAllServiceBind - SupplierCustomer.Getall (excludes customer groups 7, 9, 10 in the procedure). */
    public List<Map<String, Object>> getAll(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /**
     * DAL SupplierCustomer.SetData: SetProc (Insert when Id == 0, else Update; 0 back from the Update branch means
     * obj.Id), then USP_SupplierCustomerTaxSchedule_SyncFromMapping, one transaction rolled back on any error.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> model) {
        int id = model.get("Id") == null ? 0 : ((Number) model.get("Id")).intValue();
        int n = DesktopProc.setProc(jdbc, id == 0 ? "Sp_SupplierCustomer_Insert" : "Sp_SupplierCustomer_Update", model);
        if (n == 0) n = id;
        DesktopProc.rows(jdbc, "[dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]",
                params("OrganizationId", model.get("OrganizationId"), "CompanyId", model.get("CompanyId")));
        return n;
    }
}
