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
 * 915 "Advance Utilize Against Invoice" - Architecture.WinApp.Export.FIOrAdvanceUtilizedAgainstInvoice
 * ("Financial Instruments Utilized Against Shipment"). Data layer: Architecture.BLL.Export.ExImInvoice (BLL 0467 / DAL 0519,
 * model 0856 ExImInvoicePaymentTermsDetail), ExImEFormRegistration (BLL 0462), ExImLcPaymentTerm (BLL 0470), ExImLcOrder (BLL 0469).
 *
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   DefaultDaysToLessFromHistoryFromDate (read, unused)
 *   usp_GetExportInvoicesForFinancialInstruments   @OrganizationId @CompanyId [@ExImInvoiceId = RecId]   (Id, InvoiceNo, BankAmount, DocumentTypeId, SupplierCustomerId, TotalAmount)
 *   usp_getPaymentTermDetailByInvoiceId            @InvoiceId                                           (grid on invoice Leave)
 *   usp_GetFINoForInvoice                          @OrganizationId @CompanyId @SupplierCustomerId        (Id, EFormNo, PaymenttermId, DocumentTypeId, BalFIAmount ...)
 *   Sp_ExImEFormRegistration_GetAllMethod 'GetFinancialInstrumentsBalance'  @OrganizationId @CompanyId @DocumentTypeId @Id @Activity
 *   Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll'    @Activity @OrganizationId @CompanyId                  (Id, LcOrderTerm)
 *   Sp_ExImInvoice_GetAllMethod 'ReadById' / 'ReadExImInvoicePaymentTermsDetailByHeaderId' / 'FormHistory_PaymentTermsDetail'
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId @DocumentTypeIds='211,204'  (history Invoice combo)
 *   usp_GetFinancialInstrumentsForDropDown         @OrganizationId @CompanyId                            (history FI combo)
 *   Sp_ExImInvoicePaymentTermsDetail_Insert        the 14 non-virtual detail properties, one call per row (SetProc), one transaction
 */
@Repository
public class ExportAdvanceUtilizeInvoiceRepository {

    private final JdbcTemplate jdbc;

    public ExportAdvanceUtilizeInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> invoices(UserAccount u, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (recId != 0) p.put("ExImInvoiceId", recId);
        return DesktopProc.rows(jdbc, "usp_GetExportInvoicesForFinancialInstruments", p);
    }

    public List<Map<String, Object>> paymentTermDetail(int invoiceId) {
        return DesktopProc.rows(jdbc, "usp_getPaymentTermDetailByInvoiceId", params("InvoiceId", invoiceId));
    }

    public List<Map<String, Object>> fiNos(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return DesktopProc.rows(jdbc, "usp_GetFINoForInvoice", p);
    }

    public List<Map<String, Object>> fiBalance(UserAccount u, int documentTypeId, int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImEFormRegistration_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId, "Id", id,
                "Activity", "GetFinancialInstrumentsBalance"));
    }

    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod",
                params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> invoiceById(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> paymentTermsByHeaderId(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", id, "Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"));
    }

    public List<Map<String, Object>> historyInvoices(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeIds", "211,204"));
    }

    public List<Map<String, Object>> historyFis(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_GetFinancialInstrumentsForDropDown",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> history(UserAccount u, int eFormRegistrationId, int documentTypeId, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (eFormRegistrationId != 0) p.put("EFormRegistrationId", eFormRegistrationId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (invoiceId != 0) p.put("ExImInvoiceId", invoiceId);
        p.put("Activity", "FormHistory_PaymentTermsDetail");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** DAL SetDataForPaymentTermsDetail: every row through Sp_ExImInvoicePaymentTermsDetail_Insert; one transaction; the last scalar returned. */
    @Transactional(rollbackFor = Exception.class)
    public int save(List<Map<String, Object>> rows) {
        int result = 0;
        for (Map<String, Object> r : rows) {
            Integer n = DesktopProc.scalar(jdbc, "Sp_ExImInvoicePaymentTermsDetail_Insert", r);
            result = n == null ? 0 : n;
        }
        return result;
    }
}
