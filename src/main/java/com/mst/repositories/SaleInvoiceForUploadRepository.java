package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * 956 "Sale Invoice (Upload)" (Architecture.WinApp.DataSyncing.frmPendingSaleInvoiceForUpload) - every call the form makes
 * against the LOCAL database (ConnectionObject.SQL()), parameter for parameter. A null value is omitted from the EXEC exactly
 * as the BLL omits a guarded parameter (DesktopProc).
 *
 *   BLL 0580 InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice :2052  USP_GetBranchsAllocatedToUserFromSaleInvoice
 *   BLL 0580 InvSaleInvoice.AllComboBindAgainstSaleInvoice            :1995  Usp_AllComboAgainstSaleInvoice  (@AppId, @UserId too)
 *   BLL 0580 InvSaleInvoice.SaleInvoicePendingForUpload               :2914  USP_SaleInvoicePendingForUpload
 *   BLL 0580 InvSaleInvoice.GetByID                                   :88    Sp_InvSaleInvoice_GetAllMethod ReadById (+ DAL 0433 GetData children)
 *   BLL 0580 InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany         :23    USP_GetTaxCompanySetupByOrgAndCompany
 *   BLL 0580 InvSaleInvoice.UpdateInvoiceUploadStatus                 :46    USP_InvSaleInvoice_UpdateInvoiceUploadStatus
 *
 * The upload's own write (BLL.TaxProject InvSaleInvoice.Save and TaxScheduleMain_GetLatestSchedule) runs on the TAX SERVER
 * database and is not here - see com.mst.services.datasync.TaxServerUploadTarget.
 */
@Repository
public class SaleInvoiceForUploadRepository {

    /** ComboDBCall :238 (the proc itself filters 95, 99). */
    public static final String DOCUMENT_TYPE_IDS = "95,99";

    private final JdbcTemplate jdbc;

    public SaleInvoiceForUploadRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** BranchesDbCall :172 - GetBranchsAllocatedToUserFromSaleInvoice(org, comp, user, 0): @DocumentTypeId not sent. */
    public List<Map<String, Object>> branchesAllocated(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromSaleInvoice]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()));
    }

    /** UserAccount.BranchName of the signed-in branch (BranchesDbCall :169 when SaleInvoiceBranchWise). */
    public String branchName(UserAccount u, int branchId) {
        List<String> names = jdbc.queryForList("SELECT BranchName FROM dbo.Branches WHERE Id=? AND OrganizationId=? AND CompanyId=?",
                String.class, branchId, u.getOrganizationId(), u.getCompanyId());
        return names.isEmpty() ? "" : Objects.toString(names.get(0), "");
    }

    /**
     * ComboDBCall :229 - Usp_AllComboAgainstSaleInvoice with OrganizationId, CompanyId, AppId, UserId, DocumentTypeIds "95,99",
     * Activity "Supplier" (no branch parameter, no CostCenterId because it is 0).
     */
    public List<Map<String, Object>> customers(UserAccount u) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", u.getAppId(), "UserId", u.getId(), "DocumentTypeIds", DOCUMENT_TYPE_IDS, "Activity", "Supplier");
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstSaleInvoice]", p);
    }

    /** Sp_ConfigrationsAllocation_GetAllMethod GetConfigurationByOrgCompandConfigDescription - the ConfigKey text. */
    public String configuration(UserAccount u, String description) {
        var rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return rows.isEmpty() ? "" : Objects.toString(rows.get(0).get("ConfigKey"), "");
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in financial year. */
    public String yearStart(UserAccount u, int financialYearId) {
        var rows = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (var r : rows) {
            Object id = r.get("Id");
            if (id instanceof Number n && n.intValue() == financialYearId && r.get("Start_Period") != null)
                return String.valueOf(r.get("Start_Period")).substring(0, 10);
        }
        return "";
    }

    /**
     * PendingDataDbCall :329 -> BLL 0580:2914. Guards as the BLL: FinancialYearId / DocNoFrom / DocNoTo /
     * SupplierCustomerId / ActionId only when != 0, dates when set, BranchesIds when non-empty.
     */
    public List<Map<String, Object>> pending(UserAccount u, Integer financialYearId, java.sql.Date fromDate, java.sql.Date toDate,
                                             Integer docNoFrom, Integer docNoTo, Integer supplierCustomerId, Integer actionId,
                                             String branchIds) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "FromDate", fromDate, "ToDate", toDate,
                "DocNoFrom", docNoFrom, "DocNoTo", docNoTo, "SupplierCustomerId", supplierCustomerId,
                "ActionId", actionId, "BranchesIds", branchIds == null || branchIds.isEmpty() ? null : branchIds);
        return DesktopProc.rows(jdbc, "USP_SaleInvoicePendingForUpload", p);
    }

    /** InvSaleInvoice.GetByID(Id) header - Sp_InvSaleInvoice_GetAllMethod @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvSaleInvoice_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadById"));
    }

    /** DAL 0433 GetData :439-450 - the detail list: trading read for type 96, else SaleDetailReadByInvSaleInvoiceId. */
    public List<Map<String, Object>> details(int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_InvSaleInvoice_GetAllMethod", DesktopProc.params("Id", id, "Activity",
                documentTypeId == 96 ? "SaleDetailTradingReadByInvSaleInvoiceId" : "SaleDetailReadByInvSaleInvoiceId"));
    }

    /** DAL 0433 GetData :465-522 - the five child lists, by activity. */
    public List<Map<String, Object>> children(int id, String activity) {
        return DesktopProc.rows(jdbc, "Sp_InvSaleInvoice_GetAllMethod", DesktopProc.params("Id", id, "Activity", activity));
    }

    /** SaleInvoiceMapping :39 - InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(Obj.OrganizationId, Obj.CompanyId). */
    public List<Map<String, Object>> taxCompanySetup(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "USP_GetTaxCompanySetupByOrgAndCompany",
                DesktopProc.params("OrganizationId", organizationId, "CompanyId", companyId));
    }

    /** BLL 0580:46 - USP_InvSaleInvoice_UpdateInvoiceUploadStatus @UploadedById, @Id, @IsUploaded (its own transaction). */
    public void updateUploadStatus(int uploadedById, int id, boolean isUploaded) {
        DesktopProc.rows(jdbc, "[dbo].[USP_InvSaleInvoice_UpdateInvoiceUploadStatus]",
                DesktopProc.params("UploadedById", uploadedById, "Id", id, "IsUploaded", isUploaded));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID - Sp_DMSAttachments_GetAllMethod ReadAttachmentsbyRefDocumentTypeId. */
    public List<Map<String, Object>> attachments(int id, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "RefDocumentTypeId", refDocumentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }
}
