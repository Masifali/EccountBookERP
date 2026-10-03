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
 * 197 "Gd Bank Request" - Architecture.WinApp.Export.GdBankRequest. Data layer:
 * Architecture.BLL.Export.GDBreakUpBankRequestHeader (BLL 0454) / DAL 0506 and
 * ExImInvoice.GetDataForDropDownFromExportGdBreakup / GetGdBalanceFromBankRequest (BLL 0467).
 * Procedures, read from procdure.utf8.sql on 30-Sep-2026:
 *
 *   USP_GDBreakUpBankRequestHeader_GetAllMethod 'GenerateCode'   @OrganizationId @CompanyId @Activity           GenerateCode -> DocNo
 *   USP_GetDataForDropDownFromExportGDBreakUps                    @OrganizationId @CompanyId @Activity='Bank'    BankFill (Id, name)
 *   USP_GetDataForDropDownFromGDBreakUpBankRequest                @OrganizationId @CompanyId @Activity='Bank'    HistoryBankFill (Id, name)
 *   USP_GetGdBalanceFromBankRequest                               @OrganizationId @CompanyId @BankId [@RecId>0]  GetGdsAgainstBank (Id, GDNO, GDValue, FobValue, UtilizeAmount, GDBalance, BankInvoiceNo, GdStatus, DocumentTypeId)
 *   USP_GDBreakUpBankRequestHeader_GetAllMethod 'ReadById'       @Id @Activity                                  GetByID header
 *   USP_GDBreakUpBankRequestHeader_GetAllMethod 'ReadByHeaderId_GDBreakUpBankRequestDetail' @Id @Activity       GetByID detail list
 *   USP_GDBreakUpBankRequestHeader_GetAllMethod 'FormHistory'    @OrganizationId @CompanyId [@FromDate @ToDate | @EntryFromDate @EntryToDate |
 *                                                                @ModifyFromDate @ModifyToDate] [@FromDocNo] [@ToDocNo] [@BankId] @Activity
 *   USP_GDBreakUpBankRequestHeader_Insert / _Update              the 12 non-virtual header properties           Save (ActionId 1 / 2)
 *   USP_GDBreakUpBankRequestDetail_Insert                        the 11 non-virtual detail properties            one call per detail (ActionTypeId 1/2/3)
 *
 * The "Approved Date" radio sets ReportsParameters.ApprovedDateFrom/To, which FormHistory never
 * sends (no such parameters) - the filter then behaves as "no date filter", as on the desktop.
 * Attachments (DMSAttachments) are not part of the web port.
 */
@Repository
public class ExportGdBankRequestRepository {

    private static final String PROC = "USP_GDBreakUpBankRequestHeader_GetAllMethod";
    private final JdbcTemplate jdbc;

    public ExportGdBankRequestRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** GlobalVariables_Helper / CommonServices.GetConfigurationByOrgCompandConfigDescription(name). */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** GDBreakUpBankRequestHeader.GenerateCode. */
    public List<Map<String, Object>> generateCode(UserAccount u) {
        return DesktopProc.rows(jdbc, PROC, params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GenerateCode"));
    }

    /** ExImInvoice.GetDataForDropDownFromExportGdBreakup(org, comp, "Bank"). */
    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportGDBreakUps]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Bank"));
    }

    /** GDBreakUpBankRequestHeader.GetDataForDropDownFromGDBreakUpBankRequest(org, comp, "Bank"). */
    public List<Map<String, Object>> historyBanks(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromGDBreakUpBankRequest",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Bank"));
    }

    /** ExImInvoice.GetGdBalanceFromBankRequest(org, comp, AccountId = BankId, Id = RecId) - @RecId only when > 0. */
    public List<Map<String, Object>> gdsAgainstBank(UserAccount u, int bankId, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BankId", bankId);
        if (recId > 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "USP_GetGdBalanceFromBankRequest", p);
    }

    public List<Map<String, Object>> headerById(int id) {
        return DesktopProc.rows(jdbc, PROC, params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> detailByHeaderId(int id) {
        return DesktopProc.rows(jdbc, PROC, params("Id", id, "Activity", "ReadByHeaderId_GDBreakUpBankRequestDetail"));
    }

    /** GDBreakUpBankRequestHeader.FormHistory(ReportsParameters) - the GUARDED parameters as the BLL adds them. */
    public List<Map<String, Object>> formHistory(UserAccount u, String dateKind, java.sql.Date from, java.sql.Date to,
                                                 int fromDocNo, int toDocNo, int bankId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if ("doc".equals(dateKind)) { if (from != null) p.put("FromDate", from); if (to != null) p.put("ToDate", to); }
        else if ("entry".equals(dateKind)) { if (from != null) p.put("EntryFromDate", from); if (to != null) p.put("EntryToDate", to); }
        else if ("modify".equals(dateKind)) { if (from != null) p.put("ModifyFromDate", from); if (to != null) p.put("ModifyToDate", to); }
        /* "approved": ApprovedDateFrom/To are never sent by the BLL. */
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (bankId != 0) p.put("BankId", bankId);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, PROC, p);
    }

    /**
     * DAL GDBreakUpBankRequestHeader.SetData: header SetProc (Insert or Update), then every detail
     * through USP_GDBreakUpBankRequestDetail_Insert with GDBreakUpBankRequestHeaderId = the header
     * id; one transaction, rolled back on any error. Returns the header id.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        boolean insert = ((Number) header.get("Id")).intValue() == 0;
        Integer v = DesktopProc.scalar(jdbc, insert ? "USP_GDBreakUpBankRequestHeader_Insert" : "USP_GDBreakUpBankRequestHeader_Update", header);
        int num = v == null ? 0 : v;
        if (num <= 0) num = ((Number) header.get("Id")).intValue();
        for (Map<String, Object> d : details) {
            d.put("GDBreakUpBankRequestHeaderId", num);
            DesktopProc.scalar(jdbc, "USP_GDBreakUpBankRequestDetail_Insert", d);
        }
        return num;
    }
}
