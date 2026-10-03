package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * 910 "Purchase Invoice For Upload" (Architecture.WinApp.DataSyncing.frmPendingPurchaseInvoiceForUpload) - every
 * call the form makes against the LOCAL database (ConnectionObject.SQL()), parameter for parameter. A null value is
 * omitted from the EXEC exactly as the BLL omits a guarded parameter (DesktopProc).
 *
 *   BLL 0581 InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice :3121  USP_GetBranchsAllocatedToUserFromPurchaseInvoice
 *   BLL 0581 InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice          :2172  Usp_AllComboAgainstPurchaseInvoice
 *   BLL 0581 InvPurchaseInvoice.PurchaseInvoicePendingForUpload             :3312  USP_PurchaseInvoicePendingForUpload
 *   BLL 0581 InvPurchaseInvoice.GetByID                                     :42    Sp_InvPurchaseInvoice_GetAllMethod ReadById
 *            (DAL 0434 GetDate :580 - the detail list by DocumentTypeId, and the empty bags)
 *   BLL      InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany                        USP_GetTaxCompanySetupByOrgAndCompany
 *   BLL 0581 InvPurchaseInvoice.UpdateInvoiceUploadStatus                  :3399  USP_InvPurchaseInvoice_UpdateUploadStatus
 *
 * The upload's own write (BLL.TaxProject.Inventory.InvPurchaseInvoice.Save) runs on the TAX SERVER database
 * (ConnectionObject.SQLForServer) and is not here - see com.mst.services.datasync.TaxServerUploadTarget.
 */
@Repository
public class PurchaseInvoiceForUploadRepository {

    /** ComboDBCall :238 / USP_PurchaseInvoicePendingForUpload's own filter. */
    public static final String DOCUMENT_TYPE_IDS = "56,57,138,245,702";

    private final JdbcTemplate jdbc;

    public PurchaseInvoiceForUploadRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------------------ lookups

    /** BranchesDbCall :172 - GetBranchsAllocatedToUserFromPurchaseInvoice(org, comp, user, 0): @DocumentTypeId not sent. */
    public List<Map<String, Object>> branchesAllocated(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()));
    }

    /** UserAccount.BranchName of the signed-in branch (BranchesDbCall :169 when PurchaseInvoiceBranchWise). */
    public String branchName(UserAccount u, int branchId) {
        List<String> names = jdbc.queryForList("SELECT BranchName FROM dbo.Branches WHERE Id=? AND OrganizationId=? AND CompanyId=?",
                String.class, branchId, u.getOrganizationId(), u.getCompanyId());
        return names.isEmpty() ? "" : Objects.toString(names.get(0), "");
    }

    /** ComboDBCall :229 - Usp_AllComboAgainstPurchaseInvoice, Activity "Supplier"; @BranchesIds only when non-empty. */
    public List<Map<String, Object>> suppliers(UserAccount u, String branchIds) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", DOCUMENT_TYPE_IDS, "Activity", "Supplier");
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstPurchaseInvoice]", p);
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

    // ------------------------------------------------------------------------------ list

    /**
     * PendingDataDbCall :329 -> BLL 0581:3312. Guards as the BLL: FinancialYearId / DocNoFrom / DocNoTo /
     * SupplierCustomerId / ActionId only when != 0, dates when set, BranchesIds when non-empty (the map's null values
     * are omitted by DesktopProc).
     */
    public List<Map<String, Object>> pending(UserAccount u, Integer financialYearId, java.sql.Date fromDate, java.sql.Date toDate,
                                             Integer docNoFrom, Integer docNoTo, Integer supplierCustomerId, Integer actionId,
                                             String branchIds) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "FromDate", fromDate, "ToDate", toDate,
                "DocNoFrom", docNoFrom, "DocNoTo", docNoTo, "SupplierCustomerId", supplierCustomerId,
                "ActionId", actionId, "BranchesIds", branchIds == null || branchIds.isEmpty() ? null : branchIds);
        return DesktopProc.rows(jdbc, "USP_PurchaseInvoicePendingForUpload", p);
    }

    // ------------------------------------------------------------------------------ upload (local reads / write-back)

    /** InvPurchaseInvoice.GetByID(Id) header - Sp_InvPurchaseInvoice_GetAllMethod @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadById"));
    }

    /**
     * DAL 0434 GetDate :597-637 - the invPurchaseInvoiceDetailList read chosen by DocumentTypeId. Returns null for a
     * type GetDate loads no detail for (the list then stays null on the desktop).
     */
    public List<Map<String, Object>> details(int id, int documentTypeId) {
        String activity = switch (documentTypeId) {
            case 63, 104, 1604 -> "TradingPurchaseDetailReadByInvPurchaseInvoiceId";
            case 56, 166, 168, 172 -> "PurchaseDetailReadByInvPurchaseInvoiceId";
            case 58, 1603 -> "PurchaseTradingDetailReadByInvPurchaseInvoiceId";
            case 57, 98, 59, 138, 1804, 1653, 1654 -> "DirectPurchaseDetailReadByInvPurchaseInvoiceId";
            case 702 -> "PurchaseDetailPackingMaterialReadByHeaderId";
            default -> null;
        };
        if (activity != null)
            return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod", DesktopProc.params("Id", id, "Activity", activity));
        if (documentTypeId == 61 || documentTypeId == 64 || documentTypeId == 245)
            return DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseInvoiceDetail_ReadById]", DesktopProc.params("Id", id));
        return null;
    }

    /** DAL 0434 GetDate :690 - InvPurchaseInvoiceEmptyBagslist. */
    public List<Map<String, Object>> emptyBags(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod",
                DesktopProc.params("Id", id, "Activity", "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID"));
    }

    /** PurchaseInvoiceMapping :79 - InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(Obj.OrganizationId, Obj.CompanyId). */
    public List<Map<String, Object>> taxCompanySetup(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "USP_GetTaxCompanySetupByOrgAndCompany",
                DesktopProc.params("OrganizationId", organizationId, "CompanyId", companyId));
    }

    /** BLL 0581:3399 - USP_InvPurchaseInvoice_UpdateUploadStatus @UploadedById, @Id, @IsUploaded (its own transaction). */
    public void updateUploadStatus(int uploadedById, int id, boolean isUploaded) {
        DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseInvoice_UpdateUploadStatus]",
                DesktopProc.params("UploadedById", uploadedById, "Id", id, "IsUploaded", isUploaded));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID - Sp_DMSAttachments_GetAllMethod ReadAttachmentsbyRefDocumentTypeId. */
    public List<Map<String, Object>> attachments(int id, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "RefDocumentTypeId", refDocumentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }
}
