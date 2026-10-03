package com.mst.repositories.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 159 "Sample Analysis" — Architecture.WinApp.Lab/InvLabSampleAnalysis.cs (ScreenName
 * "InvLabSampleAnalysis", module 7), DocumentTypeId 302 (InvLabSampleAnalysis.cs:345, :1081).
 *
 * Every procedure, @Activity and parameter below was read from the decompiled desktop source and
 * checked against procdure.utf8.sql:
 *
 *   BLL 0407 / DAL 0362  Architecture.*.Lab.InvLabSampleAnalysisHeader
 *        GenerateCode / GetAll / GetByID (+ ReadDetailByHeaderId, ReadLabSampleSubParamsDetailByHeaderId) /
 *        GetPrintSlipAndReport / GetSampleLogForSampleAnalysis / GetDataForDropDownFromInvLabSampleAnalysisHeader
 *        Save (SetData) -> Sp_InvLabSampleAnalysisHeader_Insert | _Update, Sp_InvLabSampleAnalysisDetail_Insert,
 *                          Sp_InvLabSampleAnalysiSubParamsDetail_Insert
 *   BLL 0400  InvLabAnalysisGroup.AnalysisType / GetAllOrById / GetAllItemsFromByAnalysisGroup /
 *             GetAllItemsFromByAnalysisGroupandPurchaseOrder
 *   BLL 0394  LabAnalysisStandardSchedule.ComboFill / GetParametersFromGroupStandardSchedule
 *   BLL 0406  InvLabGroupAnalysisStandards.GetParametersFromGroupStandards
 *   BLL 0595  PurchaseOrder.GetForComboPoBySupplierandItemId
 *   BLL 0600  SupplierCustomer.GetforComboBinding ; BLL 0583 Item.GetAllbyCombobind ;
 *   BLL 0571  InvCropYear.Getall ; BLL 0594 jobLot.GetAll ; BLL 0069 DMSAttachments.ReadAttachmentsbyRefDocumentTypeId
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET omits a CLR null). Nothing here catches: a
 * failing procedure is a failing request.
 */
@Repository
public class SampleAnalysisRepository {

    public static final String P_GETALL = "Sp_InvLabSampleAnalysisHeader_GetAllMethod";

    private final JdbcTemplate jdbc;

    public SampleAnalysisRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ======================================================================= configuration

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(description) — ConfigKey or "". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // =========================================================================== numbering

    /** BLL 0407 GenerateCode (:33) — @OrganizationId, @CompanyId, @Activity='GenerateDocNo'; first row's DocNo. */
    public int generateDocNo(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GenerateDocNo"));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    // ================================================================================ DDL

    /** BLL 0400 AnalysisType (:~) — the desktop's own text command: SELECT * FROM [dbo].[vAnalysisType]. */
    public List<Map<String, Object>> analysisTypes() {
        return jdbc.queryForList("SELECT * FROM [dbo].[vAnalysisType]");
    }

    /** CommonServices.ItemGetForComboServiceBind() -> BLL 0583 Item.GetAllbyCombobind; ItemCategoryId 0 => @InventoryParentCategoriesId not sent. */
    public List<Map<String, Object>> items(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadAllForComboTwoColumns"));
    }

    /** CommonServices.SupplierCustomerGetforComboServiceBind() -> BLL 0600 SupplierCustomer.GetforComboBinding. */
    public List<Map<String, Object>> suppliers(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationIdCompanyIdForBinding"));
    }

    /** CommonServices.CropYearGetAllService() -> BLL 0571 InvCropYear.Getall. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** CommonServices.JobLotGetAllService() -> BLL 0594 jobLot.GetAll. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** BLL 0400 GetAllOrById — obj.Id 0 and ParentCategoryId 0, so neither is sent (AnalysisGroupdtFill :558). */
    public List<Map<String, Object>> analysisGroups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** BLL 0394 ComboFill — @OrganizationId, @CompanyId, @FromDate (the Doc Date), @Activity='ComboFill' (AnalysisGroupdtFill :567). */
    public List<Map<String, Object>> scheduleGroups(UserAccount u, Timestamp docDate) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_LabAnalysisStandardSchedule_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FromDate", docDate, "Activity", "ComboFill"));
    }

    /** BLL 0406 GetParametersFromGroupStandards — @Id (group), @OrganizationId, @CompanyId (GetDtForDetailGrid :855). */
    public List<Map<String, Object>> parametersFromGroupStandards(UserAccount u, int groupId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabGroupAnalysisStandards_GetAllMethod", params(
                "Id", groupId, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetParametersFromGroupStandards"));
    }

    /** BLL 0394 GetParametersFromGroupStandardSchedule — @Id, @OrganizationId, @CompanyId, @FromDate (GetDtForDetailGrid :857). */
    public List<Map<String, Object>> parametersFromGroupStandardSchedule(UserAccount u, int groupId, Timestamp docDate) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_LabAnalysisStandardSchedule_GetAllMethod]", params(
                "Id", groupId, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FromDate", docDate, "Activity", "GetParametersFromGroupStandardSchedule"));
    }

    /** BLL 0400 GetAllItemsFromByAnalysisGroup — @Id is the group's GroupTypeId (BindItemsByAnalysisGroup :761). */
    public List<Map<String, Object>> itemsByAnalysisGroup(UserAccount u, int groupTypeId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", groupTypeId, "Activity", "GetAllItemsFromByAnalysisGroup"));
    }

    /** BLL 0400 GetAllItemsFromByAnalysisGroupandPurchaseOrder — @Id GroupTypeId, @OrderId (BindItemsByAnalysisGroupandOrderId :740). */
    public List<Map<String, Object>> itemsByAnalysisGroupAndOrder(UserAccount u, int groupTypeId, int orderId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", groupTypeId, "OrderId", orderId, "Activity", "GetAllItemsFromByAnalysisGroupandPurchaseOrder"));
    }

    /** BLL 0595 PurchaseOrder.GetForComboPoBySupplierandItemId (PurchaseOrderFill :496). */
    public List<Map<String, Object>> purchaseOrders(UserAccount u, int supplierId, int itemId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "SupplierCustomerId", supplierId, "Activity", "GetForComboPoBySupplierandItemId"));
    }

    /** BLL 0407 GetSampleLogForSampleAnalysis (SampleLogNumberFill :447). */
    public List<Map<String, Object>> sampleLogs(UserAccount u, int supplierId, int itemId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabSampleLogRegister_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "SupplierCustomerId", supplierId, "ItemId", itemId, "Activity", "GetDataForSampleAnalysis"));
    }

    /**
     * BLL 0407 GetDataForDropDownFromInvLabSampleAnalysisHeader (HistoryComboFill :341) — only
     * @OrganizationId, @CompanyId and @DocumentTypeIds='302' are set on the ReportsParameters.
     */
    public List<Map<String, Object>> historyDropDowns(UserAccount u, String documentTypeIds) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromInvLabSampleAnalysisHeader]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", documentTypeIds));
    }

    // ============================================================================ history

    /**
     * BLL 0407 GetAll (:68). The BLL never adds @FromDocNo / @ToDocNo (the form sets them on the
     * ReportsParameters, :1495-1496, but GetAll does not read them), so they are not sent here.
     * The Entry/Modify date ranges and NoOfRecords are never set by the form.
     */
    public List<Map<String, Object>> history(UserAccount u, boolean canViewAll, int entryUser,
                                             Timestamp fromDate, Timestamp toDate, int supplierId, int itemId,
                                             int jobLotId, int analysisGroupId, String cropYear, String status) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CanViewAllRecord", canViewAll,
                "EntryUser", canViewAll ? null : (Object) entryUser,
                "FromDate", fromDate, "ToDate", toDate,
                "SupplierCustomerId", supplierId != 0 ? (Object) supplierId : null,
                "ItemId", itemId != 0 ? (Object) itemId : null,
                "JobLotId", jobLotId != 0 ? (Object) jobLotId : null,
                "AnalysisGroupId", analysisGroupId != 0 ? (Object) analysisGroupId : null,
                "CropYear", cropYear != null && !cropYear.isEmpty() ? cropYear : null,
                "Status", status != null && !status.isEmpty() ? status : null,
                "Activity", "ReadAll"));
    }

    // =============================================================================== read

    /** BLL 0407 GetByID (:222) — @Id, @Activity='ReadById' (the procedure filters by Id alone). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0362 GetData (:138) — @Id, @Activity='ReadDetailByHeaderId'. */
    public List<Map<String, Object>> details(int headerId) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", headerId, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL 0362 GetData (:148) — @Id, @Activity='ReadLabSampleSubParamsDetailByHeaderId'. */
    public List<Map<String, Object>> subDetails(int headerId) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", headerId, "Activity", "ReadLabSampleSubParamsDetailByHeaderId"));
    }

    /** BLL 0069 DMSAttachments.ReadAttachmentsbyRefDocumentTypeId — @RefDocumentTypeId, @Id (grdhistory_LinkClicked :1654). */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params(
                "RefDocumentTypeId", documentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    /**
     * BLL 0407 GetPrintSlipAndReport (Print657 :1908) — @OrganizationId, @CompanyId and, only when
     * non-zero, @Id. The other guarded parameters are never set by this form.
     */
    public List<Map<String, Object>> printRows(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabSampleAnalysisHeader_RiceSlipAndRegister_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id != 0 ? (Object) id : null));
    }

    // =============================================================================== save

    /**
     * GenericProvider.SetProc(header, "Sp_InvLabSampleAnalysisHeader_Insert" | "_Update") — every
     * non-virtual property of Model 0626 as "@Name"; the caller builds the map (nulls are omitted).
     * Insert ends with SELECT @Id; Update returns no row (0) and the DAL then keeps obj.Id (DAL 0362 :44-51).
     */
    public int setHeader(String proc, Map<String, Object> p) { return DesktopProc.setProc(jdbc, proc, p); }

    /** SetProc(detail, "Sp_InvLabSampleAnalysisDetail_Insert") — Model 0625 non-virtual: Id, InvLabAnalysisItemsId, InvLabSampleAnalysisHeaderId, ResultValue, RemarksDetail. */
    public int insertDetail(int id, int analysisItemsId, int headerId, double resultValue, String remarksDetail) {
        return DesktopProc.setProc(jdbc, "Sp_InvLabSampleAnalysisDetail_Insert", params(
                "Id", id, "InvLabAnalysisItemsId", analysisItemsId, "InvLabSampleAnalysisHeaderId", headerId,
                "ResultValue", resultValue, "RemarksDetail", remarksDetail));
    }

    /** SetProc(sub, "Sp_InvLabSampleAnalysiSubParamsDetail_Insert") — Model 0611 non-virtual properties. */
    public int insertSubDetail(int id, int detailId, int headerId, int parentParameterId, int subParameterId, double resultValue) {
        return DesktopProc.setProc(jdbc, "Sp_InvLabSampleAnalysiSubParamsDetail_Insert", params(
                "Id", id, "InvLabSampleAnalysisDetailId", detailId, "InvLabSampleAnalysisHeaderId", headerId,
                "InvParentParameterId", parentParameterId, "SubParameterId", subParameterId, "ResultValue", resultValue));
    }
}
