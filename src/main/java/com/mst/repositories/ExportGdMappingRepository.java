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
 * 952 "Goods Declaration (GD) / Bank Invoice & GD Mapping" - Architecture.WinApp.Export.
 * frmGdBreakUpByInvoiceNew (the newer variant of 881; FormDocumentTypeId 214, ScreenName
 * "frmGdBreakUpByInvoice"). Data layer: Architecture.BLL.Export.ExImInvoice (BLL 0467 / DAL 0519)
 * and Bank.GetAll (BLL 0057). Procedures, read from procdure.utf8.sql on 30-Sep-2026:
 *
 *   USP_GetBankInvoicesForGdBreakUpNew                 @OrganizationId @CompanyId [@Id != 0]         Custom Invoice combo (Id, InvoiceNo, BankInvoiceAmount)
 *   Sp_Bank_GetAllMethod 'ReadAll'                     @OrganizationId @CompanyId @Activity           Bank combo (Home Country)
 *   USP_ExImInvoiceBankGDBreakUp_ReadById              @Id (= the invoice id)                         ReadyByIdGdBreak
 *   USP_ExImInvoiceBankGDBreakUp_History               @OrganizationId @CompanyId [@FromDate] [@ToDate] [@BankId]
 *   USP_GetDataForDropDownFromExportGDBreakUps         @OrganizationId @CompanyId @Activity='Bank'    history bank combo
 *   USP_ExImInvoiceBankGDBreakUp_Insert                the 29 non-virtual model properties             SetDataGdBreakUp, one call per row
 *   USP_ExportGDBreakUpValidation                      @Id @DocumentTypeId                            once after the rows
 *   usp_GetCustomInvoicesForGdMapping                  @OrganizationId @CompanyId [@Id > 0]           dtInvoices (Id, InvoiceNo, DocumentTypeId, GdId, GDNO, GDValue, GdDocTypeId)
 *   usp_getPaymentTermDetailByInvoiceId                @InvoiceId                                     dtPaymentTerm (RecId 0 -> @FcyReceiptId not sent)
 *   USP_GetDataForDropDownFromAdvanceUtilizeagainstGDs @OrganizationId @CompanyId @Activity='Invoice' advance history invoice combo
 *   usp_AdvanceUtilizeagainstCustomInvoicesAndGDs_FormHistory @OrganizationId @CompanyId @DocumentTypeId=214 [@InvoiceId] [@FromDate] [@ToDate]
 *   USP_AdvanceUtilizeagainstGDs_Insert                the 16 non-virtual model properties             SetDataAdvanceUtilize, one call per row
 *   USP_AdvanceUtilizeagainstGDs_DeleteById            @Ids                                           when rows were removed
 *   USP_ExportAdvanceFinancialInstumentUtilizeValidation @InvoiceId @FIId @GdId @RefDocumentTypeId @FIDocumentTypeId   once per row
 *
 * GenericProvider.SetProc sends EVERY non-virtual property (a CLR null is omitted). Both saves run
 * in one transaction and roll back on any error, as the DAL does.
 */
@Repository
public class ExportGdMappingRepository {

    private final JdbcTemplate jdbc;

    public ExportGdMappingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(name). */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    // ------------------------------------------------------------------ GD BreakUp tab

    public List<Map<String, Object>> bankInvoicesForGdBreakUp(UserAccount u, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (recId != 0) p.put("Id", recId);
        return DesktopProc.rows(jdbc, "USP_GetBankInvoicesForGdBreakUpNew", p);
    }

    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> gdBreakUpByInvoiceId(int invoiceId) {
        return DesktopProc.rows(jdbc, "USP_ExImInvoiceBankGDBreakUp_ReadById", params("Id", invoiceId));
    }

    public List<Map<String, Object>> historyBanks(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportGDBreakUps]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Bank"));
    }

    public List<Map<String, Object>> gdBreakUpHistory(UserAccount u, java.sql.Date from, java.sql.Date to, int bankId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (bankId != 0) p.put("BankId", bankId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoiceBankGDBreakUp_History]", p);
    }

    /** DAL ExImInvoice.SetDataGdBreakUp. */
    @Transactional(rollbackFor = Exception.class)
    public int saveGdBreakUp(List<Map<String, Object>> items) {
        int result = 0;
        Object lastInvoiceId = null, lastFormDocTypeId = null;
        for (Map<String, Object> item : items) {
            lastInvoiceId = item.get("ExImInvoiceId");
            lastFormDocTypeId = item.get("FormDocumentTypeId");
            Integer v = DesktopProc.scalar(jdbc, "USP_ExImInvoiceBankGDBreakUp_Insert", item);
            result = v == null ? 0 : v;
        }
        if (!items.isEmpty()) {
            DesktopProc.rows(jdbc, "USP_ExportGDBreakUpValidation", params("Id", lastInvoiceId, "DocumentTypeId", lastFormDocTypeId));
        }
        return result;
    }

    // ------------------------------------------------------------------ Bank Invoice Payment Schedule & GD Mapping tab

    /** ExImInvoice.GetCustomInvoicesForGdMapping(org, comp, Id) - @Id only when > 0. */
    public List<Map<String, Object>> customInvoicesForGdMapping(UserAccount u, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (invoiceId > 0) p.put("Id", invoiceId);
        return DesktopProc.rows(jdbc, "usp_GetCustomInvoicesForGdMapping", p);
    }

    /** ExImInvoice.getPaymentTermDetailByInvoiceId(InvoiceId, 0). */
    public List<Map<String, Object>> paymentTermDetail(int invoiceId) {
        return DesktopProc.rows(jdbc, "usp_getPaymentTermDetailByInvoiceId", params("InvoiceId", invoiceId));
    }

    /** ExImInvoice.GetDataForDropDownFromExportAdvanceUtilizeAgainstGd(org, comp, "Invoice"). */
    public List<Map<String, Object>> advanceHistoryInvoices(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromAdvanceUtilizeagainstGDs]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Invoice"));
    }

    /** ExImInvoice.AdvanceUtilizeagainstCustomInvoicesAndGDs_FormHistory(ReportsParameters) - DocumentTypeId 214 always set. */
    public List<Map<String, Object>> advanceHistory(UserAccount u, int invoiceId, java.sql.Date from, java.sql.Date to) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 214);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        return DesktopProc.rows(jdbc, "usp_AdvanceUtilizeagainstCustomInvoicesAndGDs_FormHistory", p);
    }

    /** DAL ExImInvoice.SetDataAdvanceUtilize. */
    @Transactional(rollbackFor = Exception.class)
    public int saveAdvanceUtilize(List<Map<String, Object>> items, String removedIds) {
        int result = 0;
        for (Map<String, Object> item : items) {
            Integer v = DesktopProc.scalar(jdbc, "USP_AdvanceUtilizeagainstGDs_Insert", item);
            result = v == null ? 0 : v;
        }
        if (removedIds != null && !removedIds.isEmpty()) {
            DesktopProc.rows(jdbc, "USP_AdvanceUtilizeagainstGDs_DeleteById", params("Ids", removedIds));
        }
        for (Map<String, Object> item : items) {
            DesktopProc.rows(jdbc, "USP_ExportAdvanceFinancialInstumentUtilizeValidation", params(
                    "InvoiceId", item.get("EximInvoiceId"),
                    "FIId", item.get("FIId"),
                    "GdId", item.get("GDId"),
                    "RefDocumentTypeId", item.get("RefDocumentTypeId"),
                    "FIDocumentTypeId", item.get("DocumentTypeId")));
        }
        return result;
    }
}
