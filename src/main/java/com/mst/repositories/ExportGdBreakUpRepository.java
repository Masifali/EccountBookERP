package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 881 "Gd Break Up By Invoice" - Architecture.WinApp.Export.frmGdBreakUpByInvoice (App 8 "Export",
 * ModuleId 11). The desktop form's data layer is Architecture.BLL.Export.ExImInvoice (BLL 0467) and
 * Architecture.DAL.Export.ExImInvoice (DAL 0519), plus Bank.GetAll (BLL 0057) and
 * ExImEFormRegistration.GetFINoForInvoice (BLL 0462). Every call below is one of the desktop's own
 * procedures with the desktop's own parameters, read from procdure.utf8.sql on 29-Sep-2026:
 *
 *   USP_GetBankInvoicesForGdBreakUpNew        @OrganizationId @CompanyId [@Id when != 0]      Invoice combo (Id, InvoiceNo, BankInvoiceAmount)
 *   Sp_Bank_GetAllMethod 'ReadAll'            @OrganizationId @CompanyId @Activity            Bank combo, the form keeps IsHomeland == 'Home Country'
 *   USP_ExImInvoiceBankGDBreakUp_ReadById     @Id (= the invoice id)                          ReadyByIdGdBreak
 *   USP_ExImInvoiceBankGDBreakUp_History      @OrganizationId @CompanyId [@FromDate] [@ToDate] [@BankId]   History grid
 *   USP_GetDataForDropDownFromExportGDBreakUps @OrganizationId @CompanyId @Activity='Bank'     History bank combo (Id, name)
 *   USP_ExImInvoiceBankGDBreakUp_Insert       the 29 non-virtual model properties               SetDataGdBreakUp, one call per row
 *   USP_ExportGDBreakUpValidation             @Id @DocumentTypeId                              SetDataGdBreakUp, once after the rows
 *   USP_GetInvoicesForAdvanceUtilize          @OrganizationId @CompanyId                       Advance tab invoice combo (Id, InvoiceNo, DocumentTypeId)
 *   usp_GetFINoForInvoice                     @OrganizationId @CompanyId                       FI combo (Id, EFormNo, PaymenttermId, DocumentTypeId, BalFIAmount)
 *   USP_GetGDsAgainstAdvancePaymentUtilizeInInvoice @OrganizationId @CompanyId [@Id]           GD combo (Id, GDNO, GDValue, UtilizeAmount, GDBalance, BankInvoiceNo, DocumentTypeId, ExImInvoiceId, GdStepStatus)
 *   USP_AdvanceUtilizeagainstGDs_ReadById     @Id [@RefDocumentTypeId when > 0]               grid on invoice Leave
 *   USP_AdvanceUtilizeagainstGDs_Insert       the 16 non-virtual model properties               SetDataAdvanceUtilize, one call per row
 *   USP_AdvanceUtilizeagainstGDs_DeleteById   @Ids                                            SetDataAdvanceUtilize, when rows were removed
 *   USP_ExportAdvanceFinancialInstumentUtilizeValidation @InvoiceId @FIId @GdId @RefDocumentTypeId @FIDocumentTypeId   once per row after the inserts
 *   usp_AdvanceUtilizeagainstGDs_FormHistory  @OrganizationId @CompanyId                       Advance history grid
 *
 * GenericProvider.SetProc sends EVERY non-virtual property of the model, set or not, so the two save
 * methods take the full property map the service builds (unset ints are 0, unset strings are omitted -
 * AddWithValue with a CLR null leaves the parameter out). Both saves run in one transaction and roll
 * back on any error, as the DAL does.
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportGdBreakUpRepository {

    private final JdbcTemplate jdbc;

    public ExportGdBreakUpRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ shared

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(name) - the allocation row's ConfigKey, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    // ------------------------------------------------------------------ GD BreakUp tab

    /** ExImInvoice.GetBankInvoicesForGdBreakUpNew(org, comp, Id) - @Id only when non-zero. */
    public List<Map<String, Object>> bankInvoicesForGdBreakUp(UserAccount u, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (recId != 0) p.put("Id", recId);
        return DesktopProc.rows(jdbc, "USP_GetBankInvoicesForGdBreakUpNew", p);
    }

    /** Bank.GetAll - Sp_Bank_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** ExImInvoice.GetGdBreakUpByInvoiceId - USP_ExImInvoiceBankGDBreakUp_ReadById @Id = InvoiceId. */
    public List<Map<String, Object>> gdBreakUpByInvoiceId(int invoiceId) {
        return DesktopProc.rows(jdbc, "USP_ExImInvoiceBankGDBreakUp_ReadById", params("Id", invoiceId));
    }

    /** ExImInvoice.GetDataForDropDownFromExportGdBreakup(org, comp, "Bank"). */
    public List<Map<String, Object>> historyBanks(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportGDBreakUps]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Bank"));
    }

    /**
     * ExImInvoice.ExImInvoiceBankGDBreakUp_History(ReportsParameters): @FromDate / @ToDate only when the
     * pickers are ticked, @BankId only when a bank is chosen (AccountId != 0). The BLL never sets Id or
     * DocumentTypeId on this call.
     */
    public List<Map<String, Object>> gdBreakUpHistory(UserAccount u, java.sql.Date from, java.sql.Date to, int bankId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (bankId != 0) p.put("BankId", bankId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoiceBankGDBreakUp_History]", p);
    }

    /**
     * DAL ExImInvoice.SetDataGdBreakUp: every list item (removed rows first, then the grid rows, as
     * Insert() builds the list) through USP_ExImInvoiceBankGDBreakUp_Insert, then
     * USP_ExportGDBreakUpValidation with the LAST item's ExImInvoiceId and FormDocumentTypeId; one
     * transaction, rolled back on any error. Returns the last ExecuteScalar (the new Id on an insert,
     * 0 on an update, as Convert.ToInt32(null)).
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveGdBreakUp(List<Map<String, Object>> items) {
        int result = 0;
        Object lastInvoiceId = null, lastFormDocTypeId = null;
        for (Map<String, Object> item : items) {
            lastInvoiceId = item.get("ExImInvoiceId");
            lastFormDocTypeId = item.get("FormDocumentTypeId");
            result = scalarOrZero("USP_ExImInvoiceBankGDBreakUp_Insert", item);
        }
        if (!items.isEmpty()) {
            DesktopProc.rows(jdbc, "USP_ExportGDBreakUpValidation",
                    params("Id", lastInvoiceId, "DocumentTypeId", lastFormDocTypeId));
        }
        return result;
    }

    // ------------------------------------------------------------------ Advance Utilized By GD tab

    /** ExImInvoice.GetInvoicesForAdvanceUtilize. */
    public List<Map<String, Object>> invoicesForAdvanceUtilize(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetInvoicesForAdvanceUtilize",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImEFormRegistration.GetFINoForInvoice - the form passes only the organisation and company. */
    public List<Map<String, Object>> fiNosForInvoice(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_GetFINoForInvoice",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImInvoice.GetGDsAgainstAdvancePaymentUtilizeInInvoice(org, comp, InvoiceId, 0, 0) - @Id only when > 0. */
    public List<Map<String, Object>> gdsForAdvanceUtilize(UserAccount u, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (invoiceId > 0) p.put("Id", invoiceId);
        return DesktopProc.rows(jdbc, "USP_GetGDsAgainstAdvancePaymentUtilizeInInvoice", p);
    }

    /** ExImInvoice.GetAdvanceUtilizeByInvoiceId(InvoiceId, RefDocumentTypeId) - @RefDocumentTypeId only when > 0. */
    public List<Map<String, Object>> advanceUtilizeByInvoiceId(int invoiceId, int refDocumentTypeId) {
        Map<String, Object> p = params("Id", invoiceId);
        if (refDocumentTypeId > 0) p.put("RefDocumentTypeId", refDocumentTypeId);
        return DesktopProc.rows(jdbc, "USP_AdvanceUtilizeagainstGDs_ReadById", p);
    }

    /** ExImInvoice.AdvanceUtilizeagainstGDs_FormHistory. */
    public List<Map<String, Object>> advanceUtilizeHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_AdvanceUtilizeagainstGDs_FormHistory",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /**
     * DAL ExImInvoice.SetDataAdvanceUtilize: each row through USP_AdvanceUtilizeagainstGDs_Insert, then
     * USP_AdvanceUtilizeagainstGDs_DeleteById @Ids when the form removed saved rows, then
     * USP_ExportAdvanceFinancialInstumentUtilizeValidation once per row; one transaction.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveAdvanceUtilize(List<Map<String, Object>> items, String removedIds) {
        int result = 0;
        for (Map<String, Object> item : items) result = scalarOrZero("USP_AdvanceUtilizeagainstGDs_Insert", item);
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

    // ------------------------------------------------------------------ plumbing

    /**
     * Convert.ToInt32(ExecuteScalar()): the Insert branches SELECT the new Id, the Update branches select
     * nothing (null -> 0). Every later result set is still drained so a RAISERROR after the SELECT
     * surfaces and rolls the transaction back.
     */
    private int scalarOrZero(String proc, Map<String, Object> p) {
        Integer v = DesktopProc.scalar(jdbc, proc, p);
        return v == null ? 0 : v;
    }

    /** Case-insensitive column read for callers that project rows. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static List<Map<String, Object>> copy(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(new LinkedHashMap<>(r));
        return out;
    }
}
