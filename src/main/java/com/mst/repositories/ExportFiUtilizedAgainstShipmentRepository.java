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
 * "Financial Instruments Utilized Against Shipment" - Architecture.WinApp.Export.FinancialInstrumentsUtilizedAgainstShipment.
 * Data layer: ExImInvoice (BLL 0467 / DAL 0519, model ExImInvoicePaymentTermsDetail), ExImEFormRegistration (BLL 0462),
 * ExImLcPaymentTerm, ExImLcOrder.GetDataForDropDownFromExportInvoice. Procedures (procdure.utf8.sql, 03-Oct-2026):
 *
 *   usp_GetExportInvoicesForFinancialInstruments  @OrganizationId @CompanyId [@ExImInvoiceId = RecId when != 0]   InvoiceNoFill
 *   usp_GetFINoForInvoice                         @OrganizationId @CompanyId                                     BindFinancialInstrument
 *   Sp_ExImEFormRegistration_GetAllMethod         @OrganizationId @CompanyId @DocumentTypeId @Id @Activity='GetFinancialInstrumentsBalance'
 *   Sp_ExImLcPaymentTerm_GetAllMethod             @Activity='ReadAll' @OrganizationId @CompanyId                  PaymentTermsFill
 *   Sp_ExImInvoice_GetAllMethod                   @Id @Activity='ReadById' + 'ReadExImInvoicePaymentTermsDetailByHeaderId'   ExImInvoice.GetByID
 *   Sp_ExImInvoice_GetAllMethod                   @OrganizationId @CompanyId [@EFormRegistrationId] [@DocumentTypeId] [@ExImInvoiceId]
 *                                                 @Activity='FormHistory_PaymentTermsDetail'                       HistoryGridFill
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId @DocumentTypeIds='211'              HistoryCombosFill
 *   usp_GetFinancialInstrumentsForDropDown        @OrganizationId @CompanyId                                     FINoHistoryFill
 *   Sp_ConfigrationsAllocation_GetAllMethod       'GetConfigurationByOrgCompandConfigDescription'                  config
 *   Sp_ExImInvoicePaymentTermsDetail_Insert       the 14 non-virtual detail properties per row, one transaction   SaveForPaymentTermsDetail
 */
@Repository
public class ExportFiUtilizedAgainstShipmentRepository {

    private final JdbcTemplate jdbc;

    public ExportFiUtilizedAgainstShipmentRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> oc(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    public List<Map<String, Object>> invoices(UserAccount u, int recId) {
        Map<String, Object> p = oc(u);
        if (recId != 0) p.put("ExImInvoiceId", recId);
        return DesktopProc.rows(jdbc, "usp_GetExportInvoicesForFinancialInstruments", p);
    }

    /** ExImEFormRegistration.GetFINoForInvoice with only org / company (SupplierCustomerId and RecId stay 0). */
    public List<Map<String, Object>> fiNos(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_GetFINoForInvoice", oc(u));
    }

    /** GetFinancialInstrumentsBalance: Conversion.ToDecimal(Rows[0][0]), 0 when no row. */
    public List<Map<String, Object>> fiBalance(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Id", id);
        p.put("Activity", "GetFinancialInstrumentsBalance");
        return DesktopProc.rows(jdbc, "Sp_ExImEFormRegistration_GetAllMethod", p);
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

    public List<Map<String, Object>> history(UserAccount u, int eFormRegistrationId, int documentTypeId, int invoiceId) {
        Map<String, Object> p = oc(u);
        if (eFormRegistrationId != 0) p.put("EFormRegistrationId", eFormRegistrationId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (invoiceId != 0) p.put("ExImInvoiceId", invoiceId);
        p.put("Activity", "FormHistory_PaymentTermsDetail");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice(DocumentTypeIds "211") - Activity not set, so not sent. */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeIds", "211");
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]", p);
    }

    public List<Map<String, Object>> historyFis(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_GetFinancialInstrumentsForDropDown", oc(u));
    }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** DAL SetDataForPaymentTermsDetail: each row through Sp_ExImInvoicePaymentTermsDetail_Insert, one transaction. */
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
