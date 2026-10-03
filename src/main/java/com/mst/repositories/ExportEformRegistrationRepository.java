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
 * Data layer of two desktop forms that share one BLL / DAL / table (ExImEFormRegistration):
 *
 *   217 ExpfrmEformRegistration   "EFrom Registration (Not Use)"      (DocumentTypeId never set -> 0)
 *   218 ExpfrmFinancialInsturment "Financial Insturment (Not Use)"    (DocumentTypeId 207)
 *
 * Every call is the desktop's own BLL call traced to its procedure in procdure.utf8.sql (01-Oct-2026):
 *
 *   Sp_SupplierCustomer_GetAllMethod 'GetCustomerIdByLcOrderAndFcReceipt' @OrganizationId @CompanyId      SupplierCustomer.GetCustomerIdByLcOrderAndFcReceipt (customer combo: Id, CompanyName)
 *   Sp_ExImFCBankReceipts_GetAllMethod 'ReadByFCBankReceiptBySupplierCustomerId'
 *                                    @OrganizationId @CompanyId [@SupplierCustomerId] [@Id]                ExImFCBankReceipts.GetDataBySuppliercustomerId (bank reference / currency)
 *   Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll' @OrganizationId @CompanyId                                 ExImLcPaymentTerm.Getall (Id, LcOrderTerm)
 *   Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll'                                                             ExImDeliveryTerm.Getall (Id, Description)
 *   Sp_MultiCurrency_GetAllMethod 'ReadAll' @OrganizationId @CompanyId                                     MultiCurrency.GetAll (Id, CurrencyCode)
 *   SP_Country_ReadMethod @OrganizationId @CompanyId @MethodType='GetAll'                                  country.GetAll (Id, Description)
 *   Sp_Bank_GetAllMethod 'ReadAll' @OrganizationId @CompanyId                                              Bank.GetAll (the form filters IsHomeland)
 *   Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId' @OrganizationId @CompanyId                     SeaPorts.Getall (Id, PortName)
 *   Sp_ExImEFormRegistration_GetAllMethod 'ReadById' @Id                                                    ExImEFormRegistration.GetByID header
 *     'ReadEFormRegistrationAdvancePaymentUtilizationDetailbyEformHeaderId' | 'ReadEFormRegistrationComodityDetailbyEformHeaderId'
 *     | 'ReadExImEFormRegistrationPaymentTermsbyEformHeaderId' @Id                                          DAL GetDate children
 *   Sp_ExImEFormRegistration_GetAllMethod 'FormHistory' @OrganizationId @CompanyId @CanViewAllRecord [@EntryUser] [@NoOfRecords]
 *   Sp_ExImEFormRegistration_Insert / _Update  the 32 non-virtual model properties                          DAL SetDate header (Update deletes the three child tables itself)
 *   Sp_ExImEFormRegistrationAdvancePaymentUtilizationDetail_Insert @Id @ExImEFormRegistrationId @FcUtilizedAmount @ExImFCBankReceiptsId @FcCurrencyId @Remarks
 *   Sp_ExImEFormRegistrationComodityDetail_Insert @FcAmount @Weight @ExImEFormRegistrationId @Id @CommodityDescription
 *   Sp_ExImEFormRegistrationPaymentTerms_Insert @FcAmount @PaymentPercent @DaDays @ExImEFormRegistrationId @ExmLcPaymentTermId @Id
 *   Sp_DMSAttachments_GetAllMethod 'DeleteById' @ScreenName @Id + Proc_DMSAttachments_Insert                (218 attachments - see the service; file upload itself not ported)
 *
 * The DAL runs the header and every child insert in one transaction, rolled back on any error. No table,
 * column or procedure is created or changed.
 */
@Repository
public class ExportEformRegistrationRepository {

    private final JdbcTemplate jdbc;

    public ExportEformRegistrationRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ combos

    public List<Map<String, Object>> customers(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "GetCustomerIdByLcOrderAndFcReceipt");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** ExImFCBankReceipts.GetDataBySuppliercustomerId - @SupplierCustomerId / @Id only when non-zero. */
    public List<Map<String, Object>> fcBankReceipts(UserAccount u, int supplierCustomerId, int id) {
        Map<String, Object> p = tenant(u);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (id != 0) p.put("Id", id);
        p.put("Activity", "ReadByFCBankReceiptBySupplierCustomerId");
        return DesktopProc.rows(jdbc, "Sp_ExImFCBankReceipts_GetAllMethod", p);
    }

    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll"));
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p);
    }

    public List<Map<String, Object>> countries(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll");
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod", p);
    }

    public List<Map<String, Object>> banks(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod", p);
    }

    public List<Map<String, Object>> seaPorts(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadByCompanyNOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ read / history

    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImEFormRegistration_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> child(int id, String activity) {
        return DesktopProc.rows(jdbc, "Sp_ExImEFormRegistration_GetAllMethod", params("Id", id, "Activity", activity));
    }

    /** ExImEFormRegistration.FormHistory: @EntryUser only without CanViewAllRecord; @NoOfRecords = 50 (the form's value). */
    public List<Map<String, Object>> formHistory(UserAccount u, boolean canViewAll, int noOfRecords) {
        Map<String, Object> p = tenant(u);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "Sp_ExImEFormRegistration_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ save

    /**
     * DAL ExImEFormRegistration.SetDate: header (Insert when Id == 0 else Update), then the advance payment
     * utilisation rows, the commodity rows and the payment-term rows, each with ExImEFormRegistrationId = the
     * header id; then, when attachments are listed, the DMS delete + inserts. Returns the header id (the
     * Update proc selects nothing -> 0 -> obj.Id, as the DAL does).
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> utilization, List<Map<String, Object>> commodities,
                    List<Map<String, Object>> paymentTerms, List<Map<String, Object>> attachments, String screenName) {
        int id = ((Number) header.get("Id")).intValue();
        Integer r = DesktopProc.scalar(jdbc, id == 0 ? "Sp_ExImEFormRegistration_Insert" : "Sp_ExImEFormRegistration_Update", header);
        int num = r == null ? 0 : r;
        if (num > 0) id = num; else num = id;
        for (Map<String, Object> d : utilization) { d.put("ExImEFormRegistrationId", id); DesktopProc.scalar(jdbc, "Sp_ExImEFormRegistrationAdvancePaymentUtilizationDetail_Insert", d); }
        for (Map<String, Object> d : commodities) { d.put("ExImEFormRegistrationId", id); DesktopProc.scalar(jdbc, "Sp_ExImEFormRegistrationComodityDetail_Insert", d); }
        for (Map<String, Object> d : paymentTerms) { d.put("ExImEFormRegistrationId", id); DesktopProc.scalar(jdbc, "Sp_ExImEFormRegistrationPaymentTerms_Insert", d); }
        if (attachments != null && !attachments.isEmpty()) {
            DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params("ScreenName", screenName, "Id", num, "Activity", "DeleteById"));
            for (Map<String, Object> a : attachments) DesktopProc.scalar(jdbc, "Proc_DMSAttachments_Insert", a);
        }
        return num;
    }
}
