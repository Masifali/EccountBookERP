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
 * 213 "Export Bill Of Lading" - Architecture.WinApp.Export.EximBillOfLading. Data layer: Architecture.BLL.Export
 * .ExImBillOfLading (BLL 0460 / DAL, model 0841), ExImInvoice (BLL 0467), ExImExportShipingLineBooking (BLL 0463),
 * Bank.GetAll (BLL 0057), SupplierCustomer.ReadByOrganizationCompanyIdForExport (BLL 0600). Procedures:
 *
 *   Sp_ExImInvoice_GetAllMethod 'GetInvoiceNoForBillOfLading'      @OrganizationId @CompanyId @Activity           (the BLL does not send FinancialYearId)
 *   Sp_ExImInvoice_GetAllMethod 'ReadByOrganizationCompanyId'      @OrganizationId @CompanyId @DocumentTypeIds='204,211' @FinancialYearId @Id @Activity
 *   Sp_ExImExportShipingLineBooking_GetAllMethod 'ReadByInvoiceId' @OrganizationId @CompanyId @ExImInvoiceId @Activity
 *   Sp_Bank_GetAllMethod 'ReadAll'                                 @OrganizationId @CompanyId @Activity           (form keeps IsHomeland == 'Home Country')
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport' @OrganizationId @CompanyId @Activity
 *   Sp_ExImBillOfLading_GetAllMethod 'ReadById'                    @Id @Activity
 *   USP_GetDataForDropDownFromBillOfLading                         @OrganizationId @CompanyId                     (rows with Activity 'InvoiceNo')
 *   [dbo].[USP_BillOfLadingExport_FormHistory]                     @OrganizationId @CompanyId @CanViewAllRecord [@EntryUser] [@FromDate @ToDate | @EntryFromDate @EntryToDate | @ModifyFromDate @ModifyToDate] [@ExImInvoiceId]
 *   Sp_ExImBillOfLading_Insert / _Update                           the 32 non-virtual model properties (SetProc)
 *
 * The DAL's attachment steps (Proc_DMSAttachments_Insert / physical copies) run only when the Attachment popup
 * holds files; the web port has no attachment popup, so only the SetProc runs, in its own transaction.
 */
@Repository
public class ExportBillOfLadingRepository {

    private final JdbcTemplate jdbc;

    public ExportBillOfLadingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** ExImInvoice.GetInvoiceNoForBillOfLading. */
    public List<Map<String, Object>> invoicesForBillOfLading(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetInvoiceNoForBillOfLading"));
    }

    /** ExImInvoice.GetData(ReportsParameters{Ids, FinancialYearId, Id}) - @DocumentTypeIds, @FinancialYearId / @Id only when non-zero. */
    public List<Map<String, Object>> invoiceData(UserAccount u, int financialYearId, String documentTypeIds, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (invoiceId != 0) p.put("Id", invoiceId);
        p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImExportShipingLineBooking.ReadByInvoiceId. */
    public List<Map<String, Object>> bookingByInvoiceId(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImExportShipingLineBooking_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ExImInvoiceId", invoiceId, "Activity", "ReadByInvoiceId"));
    }

    /** Bank.GetAll. */
    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForExport. */
    public List<Map<String, Object>> exportParties(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyIdForExport"));
    }

    /** ExImBillOfLading.GetByID. */
    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImBillOfLading_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /** ExImBillOfLading.GetDataForDropDownFromBillOfLading (no Activity). */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromBillOfLading",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImBillOfLading.FormHistory. */
    public List<Map<String, Object>> formHistory(UserAccount u, boolean canViewAll, Map<String, Object> dates, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.putAll(dates);
        if (invoiceId != 0) p.put("ExImInvoiceId", invoiceId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_BillOfLadingExport_FormHistory]", p);
    }

    /** ExImBillOfLading.Save -> DAL SetData: Insert when Id == 0 else Update; 0 back means obj.Id. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> model) {
        int id = model.get("Id") == null ? 0 : ((Number) model.get("Id")).intValue();
        Integer n = DesktopProc.scalar(jdbc, id == 0 ? "Sp_ExImBillOfLading_Insert" : "Sp_ExImBillOfLading_Update", model);
        return n == null || n == 0 ? id : n;
    }
}
