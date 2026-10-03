package com.mst.repositories.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 161 "Sale Analysis" — Architecture.WinApp.Lab/InvLabSaleAnalysis.cs (ScreenName
 * "InvLabSaleAnalysis", module 7), DocumentTypeId 304 (InvLabSaleAnalysis.cs:615).
 *
 * Every procedure, @Activity and parameter below was read from the decompiled desktop source and
 * checked against procdure.utf8.sql (line numbers ":n" are that file):
 *
 *   BLL 0405 / DAL 0360  Architecture.*.Lab.InvLabAnalysisSaleHeader
 *        GenerateCode / GetAll / GetById / SaleAnalysisPrint
 *        Save (DAL SetData) -> Sp_InvLabAnalysisSaleHeader_Insert (:166915) | _Update (:167044),
 *                              Sp_InvLabAnalysisSaleDetail_Insert (:166685), Proc_DMSAttachments_Insert (:36546)
 *   BLL 0568  GatePassOutward.GetGPNoForSaleLabAnalaysis / GetByGpNo   (Sp_GatePassOutward_GetAllMethod :95728)
 *   BLL 0583  Item.GetItemsFromDoAndSoByGpId                           (USP_GetItemsFromDoAndSoByGpId :354353)
 *   BLL 0400  InvLabAnalysisGroup.GetAllOrById                         (Sp_InvLabAnalysisGroup_GetAllMethod :163915)
 *   BLL 0407  InvLabSampleAnalysisHeader.ReadByAnalysisGroupId         (Sp_InvLabSampleAnalysisHeader_GetAllMethod :168919)
 *   BLL 0571  InvCropYear.Getall                                       (Sp_InvCropYear_GetAllMethod :124054)
 *   BLL 0069  DMSAttachments.RemoveById / GetByID                      (Sp_DMSAttachments_GetAllMethod :64034)
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET omits a CLR null). Nothing here catches: a
 * failing procedure is a failing request.
 */
@Repository
public class SaleAnalysisRepository {

    public static final String P_GETALL = "Sp_InvLabAnalysisSaleHeader_GetAllMethod";

    private final JdbcTemplate jdbc;

    public SaleAnalysisRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =========================================================================== numbering

    /** BLL 0405 GenerateCode — @OrganizationId, @CompanyId, @Activity='GenerateDocNo'; first row's DocNo. */
    public int generateDocNo(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GenerateDocNo"));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    // ================================================================================ DDL

    /**
     * GatepassNofill (:270) -> BLL 0568 GetGPNoForSaleLabAnalaysis (:1522): @Activity, @organizationId,
     * @CompanyId only (no @BranchesId). The procedure returns the company's outward gate passes with
     * Status = 'Open', newest GpSrNo first; the form keeps Id, GpSrNo, VehicleNo.
     */
    public List<Map<String, Object>> gatePasses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_GetAllMethod", params(
                "Activity", "GetGPNoForSaleLabAnalaysis",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /**
     * txtgatepassno_Leave (:305) -> CommonServices.GetByGpNo(91, gpId) (CommonServices.cs:14746) -> BLL 0568
     * GetByGpNo (:212): @OrganizationId, @CompanyId, @DocumentTypeId, @Id, @FinancialYearId, @Activity='ReadByGpNo'.
     * The procedure (:95913) answers only for a gate pass of that document type and financial year that no
     * GDN refers to yet.
     */
    public List<Map<String, Object>> gatePassByGpNo(UserAccount u, int documentTypeId, int gpId, int financialYearId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Id", gpId, "FinancialYearId", financialYearId,
                "Activity", "ReadByGpNo"));
    }

    /** ItemFill (:451) -> BLL 0583 Item.GetItemsFromDoAndSoByGpId (:2179): @GpId only. Columns Id, ItemName. */
    public List<Map<String, Object>> itemsByGatePass(int gpId) {
        return DesktopProc.rows(jdbc, "USP_GetItemsFromDoAndSoByGpId", params("GpId", gpId));
    }

    /** AnalysisGroup (:379) -> BLL 0400 GetAllOrById with Id 0 / ParentCategoryId 0: neither is sent; Activity 'ReadAll'. */
    public List<Map<String, Object>> analysisGroups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** CropYearFill (:470) -> BLL 0571 InvCropYear.Getall: @OrganizationId, @CompanyId, @Activity='ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /**
     * cmbanalysisgroup_Leave (:412) -> BLL 0407 ReadByAnalysisGroupId (:242): @OrganizationId, @CompanyId,
     * @Id (the group), @Activity='GetBySampleGroupNo' (:168919). Columns InvLabAnalysisGroup,
     * AnalysisParameterDescription, MinValue, MaxValue, labgroupstandardId.
     */
    public List<Map<String, Object>> parametersByGroup(UserAccount u, int groupId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabSampleAnalysisHeader_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", groupId, "Activity", "GetBySampleGroupNo"));
    }

    // ============================================================================ history

    /**
     * historygridfill (:860) -> BLL 0405 GetAll (:64): @OrganizationId, @CompanyId, @NoOfRecords when
     * non-zero (the form sends 50), @CanViewAllRecord always, @EntryUser only when the user may NOT
     * view all records, @Activity='ReadAll'.
     */
    public List<Map<String, Object>> history(UserAccount u, int noOfRecords, boolean canViewAll, int userId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "NoOfRecords", noOfRecords != 0 ? (Object) noOfRecords : null,
                "CanViewAllRecord", canViewAll,
                "EntryUser", canViewAll ? null : (Object) userId,
                "Activity", "ReadAll"));
    }

    // =============================================================================== read

    /**
     * BLL 0405 GetById (:113) — @Id, @Activity='ReadById'. ONE ROW PER DETAIL ROW (header INNER JOIN
     * detail, :166754): header columns are prefixed SAH…, detail columns are RemarksDetail,
     * InAnalysisResult, InvLabGroupAnalysisStandardsId, AnalysisParameterDescription, MinValue, MaxValue.
     * A header without detail rows returns nothing.
     */
    public List<Map<String, Object>> readById(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
    }

    /**
     * GenerateReport (:1133) -> BLL 0405 SaleAnalysisPrint (:135): @OrganizationId, @CompanyId and, only
     * when non-zero, @Id. The dates / supplier / order / item guards are never met from this form.
     */
    public List<Map<String, Object>> printRows(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "SP_InvLabAnalysisSale_Slip_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id != 0 ? (Object) id : null));
    }

    // =============================================================================== save

    /**
     * GenericProvider.SetProc(header, "Sp_InvLabAnalysisSaleHeader_Insert" | "_Update") — every
     * non-virtual property of Model 0623 as "@Name"; the caller builds the map (nulls are omitted).
     * Insert ends with SELECT @Id; Update returns no row (0) and the DAL then keeps obj.Id (DAL 0360 :19-27).
     */
    public int setHeader(String proc, Map<String, Object> p) { return DesktopProc.setProc(jdbc, proc, p); }

    /**
     * SetProc(detail, "Sp_InvLabAnalysisSaleDetail_Insert") — Model 0622 non-virtual: InAnalysisResult, Id,
     * InvLabAnalysisSaleHeaderId, InvLabGroupAnalysisStandardsId, RemarksDetail (DAL 0360 :28-32).
     */
    public int insertDetail(int headerId, int groupStandardsId, double inAnalysisResult, String remarksDetail) {
        return DesktopProc.setProc(jdbc, "Sp_InvLabAnalysisSaleDetail_Insert", params(
                "InAnalysisResult", inAnalysisResult, "Id", 0, "InvLabAnalysisSaleHeaderId", headerId,
                "InvLabGroupAnalysisStandardsId", groupStandardsId, "RemarksDetail", remarksDetail));
    }

    // ======================================================================== attachments

    /** DMSAttachments.GetByID (BLL 0069 :50): @ScreenName, @Id, @Activity='ReadById' (:64059). */
    public List<Map<String, Object>> attachments(int id, String screenName) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params(
                "ScreenName", screenName, "Id", id, "Activity", "ReadById"));
    }

    /**
     * Insert() (:641-644) -> DMSAttachments.RemoveById (BLL 0069 :216): @ScreenName, @Id,
     * @Activity='DeleteById' (:64166 — delete from DMSAttachments where RefDocumentNo = @Id and ScreenName = @ScreenName).
     */
    public void removeAttachments(int id, String screenName) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?",
                screenName, id, "DeleteById");
    }

    /**
     * DAL 0360 SetData (:33-47): one Proc_DMSAttachments_Insert per attachment through SetProc — the
     * non-virtual properties of Architecture.Model.DMSAttachments. RefAccountId = header.SupplierCustomerId
     * (never set by this form: 0), RefDocumentTypeId = 304, EntryUser = header.EntryUser (never set: 0),
     * DMSFoldersLabelsId 0 and BranchId 0 are the DAL's own values.
     */
    public void insertAttachment(int refDocumentNo, int refAccountId, int refDocumentTypeId, String attachment,
                                 Timestamp entryDate, int entryUser, Timestamp modifyDate, int modifyUser,
                                 int organizationId, int companyId, String screenName,
                                 String uploadedFileCustomName, double uploadedFileSizeMb) {
        DesktopProc.setProc(jdbc, "Proc_DMSAttachments_Insert", params(
                "OrganizationId", organizationId, "RefDocumentNo", refDocumentNo, "CompanyId", companyId,
                "ModifyUser", modifyUser, "Attachment", attachment, "BranchId", 0, "Id", 0,
                "EntryDate", entryDate, "RefDocumentTypeId", refDocumentTypeId, "RefAccountId", refAccountId,
                "DMSFoldersLabelsId", 0, "ModifyDate", modifyDate, "EntryUser", entryUser,
                "ScreenName", screenName, "UploadedFileCustomName", uploadedFileCustomName,
                "UploadedFileSizeMb", uploadedFileSizeMb, "DetailWiseAttachment", false, "LineId", 0));
    }
}
