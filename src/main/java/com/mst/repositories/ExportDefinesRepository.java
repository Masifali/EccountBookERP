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
 * Data layer of four Export definition forms (Architecture.WinApp.Export), each call one of the desktop's own
 * procedures with the desktop's own parameters (read from procdure.utf8.sql, 03-Oct-2026):
 *
 * 207 ExImfrmDefineDocuments (BLL 0478/0479/0480, DAL 0530/0531/0532)
 *   Sp_ExImShipmentDocuments_GetAllMethod      @Activity='ReadAll' | 'ReadById' @Id        Getall / GetByID
 *   Sp_ExImShipmentDocuments_Insert/_Update    @Id @exImDocCode @exImDocName (model order) Save (Id == 0 -> Insert)
 *   Sp_ExImShipmentDocGroup_GetAllMethod       @Activity='ReadByCompanyOrganizationId' @OrganizationId @CompanyId | 'ReadById' @Id
 *   Sp_ExImShipmentDocGroup_Insert/_Update     @CompanyId @Id @OrganizationId @ExImDocGroupCode @ExImDocGroupName
 *   Sp_ExImShipmentDocGroupSchedule_GetAllMethod 'ReadByCompanyOrganizationId' @OrganizationId @CompanyId | 'ReadById' @Id
 *   Sp_ExImShipmentDocGroupSchedule_Insert/_Update @CompanyId @Copies @ExImShipmentDocGroupId @ExImShipmentDocuments @Id @OrganizationId @Original
 *
 * DefineExportCharges (BLL 0447, DAL 0496)
 *   USP_ExportCharges_GetAllMethod             @OrganizationId @CompanyId [@Id] @Activity='FormHistory'
 *   USP_ExportCharges_InsertAndUpdate          the 10 model properties (SetProc)
 *   USP_GETAllAccountsFromCustomGroups         @OrganizationId @CompanyId    (clsGlobalVariables.AllAccountsWithCustomGroupId)
 *
 * DefineThirdPartyType (BLL 0446, DAL 0499)
 *   USP_ThirdPartyType_GetAllMethod            @OrganizationId @CompanyId [@Id] [@IsActive] @Activity='FormHistory'
 *   USP_ThirdPartyType_InsertAndUpdate         the 10 model properties (SetProc)
 *
 * frmGenerateExportContractNos (BLL 0039 GenerateExportInvoiceNos, DAL 0038; BLL 0018 ExportPrefixType, DAL 0014)
 *   USP_ExportInvoiceNos_GetAllMethod          @OrganizationId @CompanyId [@DocumentTypeId] @Activity='FormHistory'
 *   GetMainIdForGenratingExportInvoiceNos      @OrganizationId @CompanyId
 *   USP_ExportInvoiceNosValidateAndDelete      @OrganizationId @CompanyId @MainId    then, per number,
 *   USP_ExportInvoiceNos_Insert                the 15 model properties (SetProc), one transaction
 *   usp_ExportInvoiceNosPrefix_History         (no parameters)
 *   usp_ExportInvoiceNosPrefix_Insert          @Id @PrefixDescription  (DefineExportPrefixType popup)
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportDefinesRepository {

    private final JdbcTemplate jdbc;

    public ExportDefinesRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private int scalarOrZero(String proc, Map<String, Object> p) {
        Integer v = DesktopProc.scalar(jdbc, proc, p);
        return v == null ? 0 : v;
    }

    // ================================================================== 207 Define Documents

    public List<Map<String, Object>> shipmentDocuments() {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocuments_GetAllMethod", params("Activity", "ReadAll"));
    }

    public List<Map<String, Object>> shipmentDocumentById(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocuments_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL ExImShipmentDocuments.SetData: SetProc in its own transaction; 0 back -> obj.Id. */
    @Transactional(rollbackFor = Exception.class)
    public int saveShipmentDocument(Map<String, Object> model) {
        int id = (Integer) model.get("Id");
        int r = scalarOrZero(id == 0 ? "Sp_ExImShipmentDocuments_Insert" : "Sp_ExImShipmentDocuments_Update", model);
        return r > 0 ? r : id;
    }

    public List<Map<String, Object>> docGroups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocGroup_GetAllMethod", params(
                "Activity", "ReadByCompanyOrganizationId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> docGroupById(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocGroup_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveDocGroup(Map<String, Object> model) {
        int id = (Integer) model.get("Id");
        int r = scalarOrZero(id == 0 ? "Sp_ExImShipmentDocGroup_Insert" : "Sp_ExImShipmentDocGroup_Update", model);
        return r > 0 ? r : id;
    }

    public List<Map<String, Object>> docGroupSchedules(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocGroupSchedule_GetAllMethod", params(
                "Activity", "ReadByCompanyOrganizationId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> docGroupScheduleById(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocGroupSchedule_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveDocGroupSchedule(Map<String, Object> model) {
        int id = (Integer) model.get("Id");
        int r = scalarOrZero(id == 0 ? "Sp_ExImShipmentDocGroupSchedule_Insert" : "Sp_ExImShipmentDocGroupSchedule_Update", model);
        return r > 0 ? r : id;
    }

    // ================================================================== Define Export Charges

    public List<Map<String, Object>> exportChargesHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_ExportCharges_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }

    /** DAL ExportCharges.SetData: the returned Id, or ExportChargesID when the proc returns 0. */
    @Transactional(rollbackFor = Exception.class)
    public int saveExportCharges(Map<String, Object> model) {
        int r = scalarOrZero("USP_ExportCharges_InsertAndUpdate", model);
        return r > 0 ? r : (Integer) model.get("ExportChargesID");
    }

    /** GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup(org, comp, 0, 0, "") - paging/keyword omitted. */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    // ================================================================== Define Third Party Type

    /** ThirdPartyType.FormHistory(org, comp, 0, false): @Id and @IsActive are not sent. */
    public List<Map<String, Object>> thirdPartyTypeHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_ThirdPartyType_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveThirdPartyType(Map<String, Object> model) {
        int r = scalarOrZero("USP_ThirdPartyType_InsertAndUpdate", model);
        return r > 0 ? r : (Integer) model.get("ThirdPartyTypeID");
    }

    // ================================================================== Generate Export Contract Nos

    public List<Map<String, Object>> exportPrefixTypes() {
        return DesktopProc.rows(jdbc, "usp_ExportInvoiceNosPrefix_History", params());
    }

    /** ExportPrefixType.Save -> usp_ExportInvoiceNosPrefix_Insert @Id @PrefixDescription (model order). */
    @Transactional(rollbackFor = Exception.class)
    public int saveExportPrefixType(int id, String description) {
        return scalarOrZero("usp_ExportInvoiceNosPrefix_Insert", params("Id", id, "PrefixDescription", description));
    }

    /** GenerateExportInvoiceNos.GetAll: @DocumentTypeId only when != 0. */
    public List<Map<String, Object>> exportInvoiceNosHistory(UserAccount u, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
    }

    /** GenerateExportInvoiceNos.GetMainId - Rows[0]["MainId"]. */
    public int mainId(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[GetMainIdForGenratingExportInvoiceNos]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        Object v = r.get(0).get("MainId");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /**
     * DAL GenerateExportInvoiceNos.SetData: USP_ExportInvoiceNosValidateAndDelete with the HEADER's
     * OrganizationId / CompanyId / MainId (0 on a new save - the form never sets obj.MainId there), then every
     * number through USP_ExportInvoiceNos_Insert; one transaction. Returns the last Id.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveExportInvoiceNos(int orgId, int companyId, int headerMainId, List<Map<String, Object>> details) {
        DesktopProc.rows(jdbc, "USP_ExportInvoiceNosValidateAndDelete",
                params("OrganizationId", orgId, "CompanyId", companyId, "MainId", headerMainId));
        int num = 0;
        for (Map<String, Object> d : details) num = scalarOrZero("USP_ExportInvoiceNos_Insert", d);
        return num;
    }

    // ================================================================== shared

    /** clsGlobalVariables.ActiveYr - the session's financial year row (else the first active one). */
    public Map<String, Object> financialYear(UserAccount u, int financialYearId) {
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (Map<String, Object> r : years) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) return r;
        }
        return years.isEmpty() ? null : years.get(0);
    }
}
