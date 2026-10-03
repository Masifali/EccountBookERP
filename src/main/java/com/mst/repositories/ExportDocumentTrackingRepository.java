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
 * Module 130 "Export document Tracking" (Architecture.WinApp.SDT / SDT_Reports) - one method per BLL call
 * of the seven forms, each the desktop's own procedure with the desktop's own parameters, read from
 * procdure.utf8.sql on 30-Sep-2026. The BLL classes are shared between the forms (ChartOfDocument serves
 * 619, 620, 623; CustomGroup serves 621, 622, 623; ShipmentDocumentSchedule serves 624, 625), so the
 * module keeps one repository.
 *
 *   BLL 0287 AlertLevelColor.FormHistory                      [sdt].[USP_AlertLevelColor_FormHistory]                 (no parameters)
 *   BLL 0289 ChartOfDocument.FormHistory                      [sdt].[USP_ChartOfDocument_GetAllMethod]  @OrganizationId @CompanyId @Activity='FormHistory'
 *            ChartOfDocument.ReadById                         same proc  @Activity='ReadById' @chartOfDocumentId
 *            ChartOfDocument.RequiredLevelForCombo            same proc  @Activity='RequiredLevelForCombo'
 *            ChartOfDocument.DocumentProviderTypeForCombo     same proc  @Activity='DocumentProviderForCombo'
 *            ChartOfDocument.CriteriaDateTypeForCombo         same proc  @Activity='CriteriaDateTypeForCombo'
 *            ChartOfDocument.GetChartOfDocumentByCustomGroupId [sdt].[USP_GetChartOfDocumentByCustomGroupId] @OrganizationId @CompanyId @CustomGroupId
 *            ChartOfDocument.save                             [sdt].[USP_ChartOfDocument_InsertAndUpdate] (the 14 model properties)
 *   BLL 0291 DocDueAlertColorSchedule.DropDownFillFrom...     [sdt].[USP_DropDownFillFromDocDueAlertColorSchedule] @OrganizationId @CompanyId @Activity
 *            DocDueAlertColorSchedule.FormHistory             [sdt].[USP_DocDueAlertColorSchedule_GetAllMethod] @OrganizationId @CompanyId @CanViewAllRecord [@EntryUserId] [@EntryFromDate] [@EntryToDate] [@ModifyFromDate] [@ModifyToDate] [@chartOfDocumentId] @Activity='FormHistory'
 *            DocDueAlertColorSchedule.Save                    [sdt].[USP_DocDueAlertColorSchedule_InsertAndUpdate] (the 14 model properties), one call per list item, one transaction
 *   BLL 0290 CustomGroup.FormHistory / ReadById / GetCustomGroupForCombo   [sdt].[USP_CustomGroup_GetAllMethod]
 *            CustomGroup.save                                 [sdt].[USP_CustomGroup_InsertAndUpdate] (the 10 model properties)
 *   BLL 0288 ClientCustomGroup.ClientCustomGroupUnAllocatedToCustomGroup [sdt].[USP_ClientCustomGroupUnAllocatedToCustomGroup] @OrganizationId @CompanyId @CustomGroupId
 *            ClientCustomGroup.ClientCustomGroupAllocatedToCustomGroup   [sdt].[USP_ClientCustomGroupAllocatedToCustomGroup]   @OrganizationId @CompanyId @CustomGroupId
 *            ClientCustomGroup.GetCustomGroupAllocatedToCustomerBySupplierCustomerId [sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId] @OrganizationId @CompanyId @SupplierCustomerId
 *            ClientCustomGroup.SaveAndUpdate                  [sdt].[USP_ClientCustomGroup_InsertAndUpdate] (the 10 model properties), one call per list item, one transaction
 *   BLL 0293 DocumentOfGroup.GetDataForDropDownFromDocumentOfGroup [sdt].[USP_DropDownFillFromDocumentOfGroup] @OrganizationId @CompanyId
 *            DocumentOfGroup.LastRecordBycustomGroupId        [sdt].[USP_DocumentOfGroup_GetAllMethod] @OrganizationId @CompanyId @CustomGroupId @Activity='LastRecordBycustomGroupId'
 *            DocumentOfGroup.ReadByCustomGroupId              same proc  @CustomGroupId @Activity='ReadByCustomGroupId'
 *            DocumentOfGroup.FormHistory                      same proc  @OrganizationId @CompanyId @DocumentTypeId @CanViewAllRecord [@EntryUserId] [@CustomGroupId] [@EntryFromDate] [@EntryToDate] [@ModifyFromDate] [@ModifyToDate] [@ApprovedFromDate] [@ApprovedToDate] [@DocumentProviderId] [@RequiredLevelId] [@CriteriaDateTypeId] [@chartOfDocumentId] @Activity='FormHistory'
 *            DocumentOfGroup.Save                             [sdt].[USP_DocumentOfGroup_InsertAndUpdate] (the 23 non-virtual model properties), one call per list item, one transaction
 *   BLL 0469 ExImLcOrder.GetDataForDropDownFromExportContract [dbo].[USP_GetDataForDropDownFromExportContract] @OrganizationId @CompanyId
 *   BLL 0457 ExImLcOrderShipmentSchedule.GetContactScheduleNoExistsInShipmentDocumentSchedule  USp_GetContactScheduleNoExistsInShipmentDocumentSchedule @OrganizationId @CompanyId @ReferedInSDS [@SupplierCustomerId] [@SalesPersonId]
 *   BLL 0136 GeneralReprots.StaticColumnNames                 SpStaticColumnNames @Activity='ShipmentDocumentScheduleStatus'
 *   BLL 0292 ShipmentDocumentSchedule.ReadByRefDocument       [sdt].[USP_ShipmentDocumentSchedule_GetAllMethod] @RefDocId @RefDocumentTypeId @CustomGroupId @Activity='ReadByRefDocument'
 *            ShipmentDocumentSchedule.GetChartOfDocumentForShipmentScheduleByRefDocument [sdt].[USP_GetChartOfDocumentForShipmentScheduleByRefDocument] @RefDocId @RefDocumentTypeId @CustomGroupId
 *            ShipmentDocumentSchedule.AutoSaveByRefDocumentType [sdt].[USP_ShipmentDocumentSchedule_AutoInsert] @RefDocumentTypeId @RefDocId @CustomGroupId [@ChartOfDocumentIds]
 *            ShipmentDocumentSchedule.Save (DAL SetData)       [sdt].[USP_ShipmentDocumentSchedule_UpdateRefDocument] @RefDocumentTypeId @RefDocId @RefDocCustomGroupId @RefDocLockStatus, then [sdt].[USP_ShipmentDocumentSchedule_Insert] per row (25 model properties)
 *            ShipmentDocumentSchedule.DeleteByIds             [sdt].[USP_ShipmentDocumentSchedule_GetAllMethod] @Activity='DeleteByIds' @Ids @CODIds @RefDocumentTypeId @EntryUserId
 *            ShipmentDocumentSchedule.UpdateDocStatusAndReadyDate same proc @Activity='UpdateDocStatusAndReadyDate' @Id @DocStatusId @ReadyDate @EntryUserId, per row, one transaction
 *            ShipmentDocumentSchedule.GetDataForDropDownFromShipmentDocumentSchedule [sdt].[USP_DropDownFillFromShipmentDocumentSchedule] @OrganizationId @CompanyId
 *            ShipmentDocumentSchedule.ExportDocumentTrackingReport / ...GroupWiseI / ...GroupWiseII   SDT.USP_ExportDocumentTrackingReport / ...ReportI / ...ReportII  @OrganizationId @CompanyId [@DueFrom] [@DueTo] [@ReadyDateFrom] [@ReadyDateTo] [@DocumentProviderId] [@chartOfDocumentId] [@SupplierCustomerId] [@SalesPersonId] [@ExImInvoiceId] [@ReadyDocsOnly] [@PendingInvoice]
 *   BLL 0069 DMSAttachments.GetByID                           Sp_DMSAttachments_GetAllMethod @ScreenName @Id @Activity='ReadById'
 *
 * A null value is never bound (ADO.NET AddWithValue with a CLR null leaves the parameter out). No table,
 * column or procedure is created or changed.
 */
@Repository
public class ExportDocumentTrackingRepository {

    private final JdbcTemplate jdbc;

    public ExportDocumentTrackingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ shared

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription(name) -> ConfigKey, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the session year's row of the active-year list. */
    public String financialYearStart(UserAccount u, int yearId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Object id = r.get("Id");
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(yearId))) {
                return r.get("Start_Period") == null ? "" : String.valueOf(r.get("Start_Period"));
            }
        }
        return "";
    }

    // ------------------------------------------------------------------ AlertLevelColor / ChartOfDocument (BLL 0287, 0289)

    public List<Map<String, Object>> alertLevelColors() {
        return DesktopProc.rows(jdbc, "[sdt].[USP_AlertLevelColor_FormHistory]", params());
    }

    public List<Map<String, Object>> chartOfDocumentHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ChartOfDocument_GetAllMethod]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }

    public List<Map<String, Object>> chartOfDocumentById(int id) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ChartOfDocument_GetAllMethod]", params("Activity", "ReadById", "chartOfDocumentId", id));
    }

    public List<Map<String, Object>> requiredLevels() {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ChartOfDocument_GetAllMethod]", params("Activity", "RequiredLevelForCombo"));
    }

    public List<Map<String, Object>> documentProviders() {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ChartOfDocument_GetAllMethod]", params("Activity", "DocumentProviderForCombo"));
    }

    public List<Map<String, Object>> criteriaDateTypes() {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ChartOfDocument_GetAllMethod]", params("Activity", "CriteriaDateTypeForCombo"));
    }

    public List<Map<String, Object>> chartOfDocumentByCustomGroupId(UserAccount u, int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_GetChartOfDocumentByCustomGroupId]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "CustomGroupId", customGroupId));
    }

    /** DAL ChartOfDocument.SetData - one SetProc in a transaction; Convert.ToInt32(ExecuteScalar()). */
    @Transactional(rollbackFor = Exception.class)
    public int saveChartOfDocument(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, "[sdt].[USP_ChartOfDocument_InsertAndUpdate]", model);
    }

    // ------------------------------------------------------------------ DocDueAlertColorSchedule (BLL 0291)

    public List<Map<String, Object>> docDueDropDown(UserAccount u, String activity) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_DropDownFillFromDocDueAlertColorSchedule]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", activity));
    }

    /** The BLL's parameter list in its order; the service passes only the parameters the BLL would add. */
    public List<Map<String, Object>> docDueHistory(Map<String, Object> p) {
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "[sdt].[USP_DocDueAlertColorSchedule_GetAllMethod]", p);
    }

    /** DAL DocDueAlertColorSchedule.SetData: every list item through the proc, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int saveDocDueSchedule(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> item : items) result = scalarOrZero("[sdt].[USP_DocDueAlertColorSchedule_InsertAndUpdate]", item);
        return result;
    }

    // ------------------------------------------------------------------ CustomGroup (BLL 0290)

    public List<Map<String, Object>> customGroupHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_CustomGroup_GetAllMethod]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }

    public List<Map<String, Object>> customGroupById(int id) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_CustomGroup_GetAllMethod]", params("Activity", "ReadById", "customGroupId", id));
    }

    public List<Map<String, Object>> customGroupsForCombo(UserAccount u) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_CustomGroup_GetAllMethod]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetCustomGroupForCombo"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveCustomGroup(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, "[sdt].[USP_CustomGroup_InsertAndUpdate]", model);
    }

    // ------------------------------------------------------------------ ClientCustomGroup (BLL 0288)

    public List<Map<String, Object>> clientUnallocated(UserAccount u, int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ClientCustomGroupUnAllocatedToCustomGroup]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "CustomGroupId", customGroupId));
    }

    public List<Map<String, Object>> clientAllocated(UserAccount u, int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ClientCustomGroupAllocatedToCustomGroup]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "CustomGroupId", customGroupId));
    }

    public List<Map<String, Object>> customGroupsForCustomer(UserAccount u, int supplierCustomerId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "SupplierCustomerId", supplierCustomerId));
    }

    /** DAL ClientCustomGroup.SetData: every list item through the proc, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int saveClientCustomGroup(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> item : items) result = scalarOrZero("[sdt].[USP_ClientCustomGroup_InsertAndUpdate]", item);
        return result;
    }

    // ------------------------------------------------------------------ DocumentOfGroup (BLL 0293)

    public List<Map<String, Object>> docOfGroupDropDown(UserAccount u) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_DropDownFillFromDocumentOfGroup]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> docOfGroupLastRecord(UserAccount u, int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_DocumentOfGroup_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CustomGroupId", customGroupId, "Activity", "LastRecordBycustomGroupId"));
    }

    public List<Map<String, Object>> docOfGroupByCustomGroup(int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_DocumentOfGroup_GetAllMethod]",
                params("CustomGroupId", customGroupId, "Activity", "ReadByCustomGroupId"));
    }

    public List<Map<String, Object>> docOfGroupHistory(Map<String, Object> p) {
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "[sdt].[USP_DocumentOfGroup_GetAllMethod]", p);
    }

    /** DAL DocumentOfGroup.SetData: every list item (removed rows first) through the proc, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int saveDocumentOfGroup(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> item : items) result = scalarOrZero("[sdt].[USP_DocumentOfGroup_InsertAndUpdate]", item);
        return result;
    }

    // ------------------------------------------------------------------ ShipmentDocumentSchedule (BLL 0292) + its combos

    /** ExImLcOrder.GetDataForDropDownFromExportContract(org, comp) - Id / name / ActivityType. */
    public List<Map<String, Object>> exportContractDropDown(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportContract]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImLcOrderShipmentSchedule.GetContactScheduleNoExistsInShipmentDocumentSchedule. */
    public List<Map<String, Object>> contractScheduleNos(UserAccount u, int supplierCustomerId, int salesPersonId, boolean referedInSDS) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ReferedInSDS", referedInSDS);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (salesPersonId != 0) p.put("SalesPersonId", salesPersonId);
        return DesktopProc.rows(jdbc, "USp_GetContactScheduleNoExistsInShipmentDocumentSchedule", p);
    }

    /** CommonServices.StaticColumnsService("ShipmentDocumentScheduleStatus") - Id / type. */
    public List<Map<String, Object>> scheduleStatuses() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "ShipmentDocumentScheduleStatus"));
    }

    public List<Map<String, Object>> sdsReadByRefDocument(int refDocId, int refDocumentTypeId, int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]", params(
                "RefDocId", refDocId, "RefDocumentTypeId", refDocumentTypeId, "CustomGroupId", customGroupId, "Activity", "ReadByRefDocument"));
    }

    public List<Map<String, Object>> sdsChartOfDocumentsForRefDocument(int refDocId, int refDocumentTypeId, int customGroupId) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_GetChartOfDocumentForShipmentScheduleByRefDocument]",
                params("RefDocId", refDocId, "RefDocumentTypeId", refDocumentTypeId, "CustomGroupId", customGroupId));
    }

    /** DMSAttachments.GetByID(RefDocId, "frmShipmentDocumentSchedule") - the detail-wise attachment rows. */
    public List<Map<String, Object>> dmsAttachmentsById(int id, String screenName) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params("ScreenName", screenName, "Id", id, "Activity", "ReadById"));
    }

    /** DAL ShipmentDocumentSchedule.AutoSaveByRefDocumentType - only when both ids are > 0, as the DAL does. */
    @Transactional(rollbackFor = Exception.class)
    public void sdsAutoInsert(int refDocumentTypeId, int refDocId, int customGroupId, String chartOfDocumentIds) {
        if (refDocumentTypeId > 0 && refDocId > 0) {
            Map<String, Object> p = params("RefDocumentTypeId", refDocumentTypeId, "RefDocId", refDocId, "CustomGroupId", customGroupId);
            if (chartOfDocumentIds != null && !chartOfDocumentIds.isEmpty()) p.put("ChartOfDocumentIds", chartOfDocumentIds);
            DesktopProc.rows(jdbc, "[sdt].[USP_ShipmentDocumentSchedule_AutoInsert]", p);
        }
    }

    /**
     * DAL ShipmentDocumentSchedule.SetData without the attachment branch: UpdateRefDocument with the first
     * row's reference / custom group / lock status, then every row through USP_ShipmentDocumentSchedule_Insert;
     * one transaction, rolled back on any error.
     */
    @Transactional(rollbackFor = Exception.class)
    public int sdsSave(List<Map<String, Object>> items, int refDocumentTypeId, int refDocId, int refDocCustomGroupId, boolean refDocLockStatus) {
        int result = 0;
        if (!items.isEmpty()) {
            DesktopProc.rows(jdbc, "[sdt].[USP_ShipmentDocumentSchedule_UpdateRefDocument]", params(
                    "RefDocumentTypeId", refDocumentTypeId, "RefDocId", refDocId,
                    "RefDocCustomGroupId", refDocCustomGroupId, "RefDocLockStatus", refDocLockStatus));
        }
        for (Map<String, Object> item : items) result = scalarOrZero("[sdt].[USP_ShipmentDocumentSchedule_Insert]", item);
        return result;
    }

    @Transactional(rollbackFor = Exception.class)
    public void sdsDeleteByIds(String ids, String codIds, int refDocumentTypeId, int entryUserId) {
        if (ids == null || ids.isEmpty()) return;
        DesktopProc.rows(jdbc, "[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]", params(
                "Activity", "DeleteByIds", "Ids", ids, "CODIds", codIds, "RefDocumentTypeId", refDocumentTypeId, "EntryUserId", entryUserId));
    }

    /** BLL ShipmentDocumentSchedule.UpdateDocStatusAndReadyDate - rows with an id, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public void sdsUpdateDocStatusAndReadyDate(List<Map<String, Object>> items) {
        for (Map<String, Object> it : items) {
            DesktopProc.rows(jdbc, "[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]", params(
                    "Activity", "UpdateDocStatusAndReadyDate",
                    "Id", it.get("Id"), "DocStatusId", it.get("DocStatus"), "ReadyDate", it.get("ReadyDate"), "EntryUserId", it.get("ModifyUserId")));
        }
    }

    public List<Map<String, Object>> sdsDropDown(UserAccount u) {
        return DesktopProc.rows(jdbc, "[sdt].[USP_DropDownFillFromShipmentDocumentSchedule]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** which: "" (detail), "I" (group wise I), "II" (group wise II). */
    public List<Map<String, Object>> trackingReport(String which, Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "SDT.USP_ExportDocumentTrackingReport" + which, p);
    }

    // ------------------------------------------------------------------ plumbing

    /** Convert.ToInt32(ExecuteScalar()): the insert branches SELECT the new id, the update branches select nothing (0). */
    private int scalarOrZero(String proc, Map<String, Object> p) {
        Integer v = DesktopProc.scalar(jdbc, proc, p);
        return v == null ? 0 : v;
    }
}
