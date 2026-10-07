package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * 957 "Vouchers (Upload)" (Architecture.WinApp.DataSyncing.frmPendingVouchersForUpload) - every call the form makes
 * against the LOCAL database, parameter for parameter. A null value is omitted from the EXEC exactly as the BLL omits a
 * guarded parameter (DesktopProc).
 *
 *   ComboDBCall :162  VoucherReports.LedgerByJobLotDropDownAndLists (BLL 0141:3834)  Sp_Vouchers_LedgerByJobLot_DropDownAndLists
 *   VoucherGridFill :229  VoucherHead.Voucher_PendingForUpload (BLL 0654:2829)       USP_Voucher_PendingForUpload
 *   VoucherDetailByHeaderId :456  VoucherHead.GetByID (BLL 0654:377 -> DAL 0586 GetData:237)
 *                                                                                    Sp_Vouchers_GetMethods ReadByID / VoucherDetail_ReadByVoucherHeadID
 *   CommonServices.GetERPFeatureById(4 / 6)                                          USP_GetERPFeaturesByCompanyId
 *   VoucherMapping.MapToTaxProjectVoucher :55  InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany  USP_GetTaxCompanySetupByOrgAndCompany
 *   CommonServices.GetNoofAttachmentsByRefDocumentTypeID                             Sp_DMSAttachments_GetAllMethod
 *
 * The upload's own write (BLL.TaxProject.Accounts.VoucherHead.Save) runs on the TAX SERVER database
 * (ConnectionObject.SQLForServer) and is not here - see com.mst.services.datasync.TaxServerUploadTarget.
 */
@Repository
public class PendingVouchersForUploadRepository {

    /** ComboDBCall :168 - the document types the form (and USP_Voucher_PendingForUpload's own default) lists. */
    public static final String DOCUMENT_TYPE_IDS = "1,2,3,4,5,6,7,9,10,26,34,35";

    private final JdbcTemplate jdbc;

    public PendingVouchersForUploadRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /**
     * ComboDBCall - OrganizationId = UserAccount.OrganizationId, CompanyId = UserAccount.OrganizationId (the form passes
     * the organization id there, as ComboBindForHistory does on the voucher forms - reproduced), AppId and UserId are
     * never set so they are sent as 0.
     */
    public List<Map<String, Object>> combos(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_LedgerByJobLot_DropDownAndLists", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getOrganizationId(),
                "AppId", 0, "UserId", 0, "DocumentTypeIds", DOCUMENT_TYPE_IDS));
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in financial year ("" when not found). */
    public String yearStart(UserAccount u, int financialYearId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (Map<String, Object> r : rows) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId && r.get("Start_Period") != null) {
                return String.valueOf(r.get("Start_Period")).substring(0, 10);
            }
        }
        return "";
    }

    /** CommonServices.GetERPFeatureById(id): the id is listed by USP_GetERPFeaturesByCompanyId. */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /**
     * BLL 0654:2829. OrganizationId, CompanyId, FinancialYearId and UserId always; every other parameter only when the
     * BLL's guard lets it through (null = omitted by DesktopProc).
     */
    public List<Map<String, Object>> pending(UserAccount u, int financialYearId, Map<String, Object> filters) {
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "UserId", u.getId());
        for (String k : new String[] { "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate",
                "ApprovedFromDate", "ApprovedToDate", "DocNoFrom", "DocNoTo", "AccountId", "ActionId", "IsApproved", "DocumentTypeIds" }) {
            if (filters.get(k) != null) p.put(k, filters.get(k));
        }
        return DesktopProc.rows(jdbc, "[dbo].[USP_Voucher_PendingForUpload]", p);
    }

    /** VoucherHead.GetByID header - Sp_Vouchers_GetMethods @Id, @Activity 'ReadByID'. */
    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params("Id", id, "Activity", "ReadByID"));
    }

    /** DAL 0586 GetData :254 - voucherDetailList. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params("Id", id, "Activity", "VoucherDetail_ReadByVoucherHeadID"));
    }

    /** VoucherMapping :55 - InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(source.OrganizationId, source.CompanyId). */
    public List<Map<String, Object>> taxCompanySetup(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "USP_GetTaxCompanySetupByOrgAndCompany",
                DesktopProc.params("OrganizationId", organizationId, "CompanyId", companyId));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID - Sp_DMSAttachments_GetAllMethod ReadAttachmentsbyRefDocumentTypeId. */
    public List<Map<String, Object>> attachments(int id, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "RefDocumentTypeId", refDocumentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    public static String str(Object v) { return Objects.toString(v, ""); }
}
