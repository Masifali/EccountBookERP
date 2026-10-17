package com.mst.services;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Architecture.WinApp.Account_Definition.frmFinancialYearClose (ScreenDefinition 399, module 43, System Admin).
 *
 * What the desktop screen does (it does NOT write opening balances itself): it runs USP_FinancialYearClose over
 * [active year start .. ClosingDate] for the chosen branch, which returns every income/expense account
 * (AccountClass 4,5) with its net balance flipped into Debit / Credit; Save posts those rows as ONE voucher of
 * DocumentTypeId 30 through VoucherHead.Save (DAL 0586 SetData, here DesktopVoucherWriter) with a balancing line
 * on the configured UnAppropriatedProfitAndLoss account.
 *
 * GATE: the Admin Panel is a gear item drawn only for RoleName "Admin"; AdmPanelService.admin() is the same test.
 * The browser sends only ids/dates/the document no; the rows are re-read from the same procedure on the server
 * (new) or from the stored voucher (update) - exactly the rows the desktop grid holds - never taken from the page.
 */
@Service
public class FinancialYearCloseService {

    private static final int DOC_TYPE = 30;                                    // frmFinancialYearClose :133 / :224

    @Autowired private JdbcTemplate jdbc;
    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext ctx;
    @Autowired private AdmPanelService adm;

    // ------------------------------------------------------------------ load (InitializeComponentMethod)

    public Map<String, Object> init() {
        adm.admin();
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId(), user = ctx.currentUserId();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("docNo", generateCode(org, comp, ctx.currentFinancialYearId()));
        // BranchesAllocationToUser.GetBranchsAllocatedToUser -> [dbo].[USP_GetBranchsAllocatedToUser]
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?", org, comp, user);
        List<Map<String, Object>> branches = new ArrayList<>();
        for (Map<String, Object> b : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("BranchId", asInt(ci(b, "BranchId")));
            m.put("BranchName", str(ci(b, "BranchName")));
            branches.add(m);
        }
        r.put("branches", branches);
        int own = 0;
        try { own = ctx.currentBranchId(); } catch (RuntimeException ignore) { /* no branch on the account */ }
        r.put("defaultBranchId", own);                                          // cmbBranchName.Text = UserAccount.BranchName
        r.put("unAppropriatedConfigured", unAppropriatedAccount(org, comp) != 0);
        return r;
    }

    /** GenerateCodeDbCall: Sp_Vouchers_GetMethods 'GenerateVoucherCodeByDocumentTypeId' (BranchesId = 0, the model default). */
    private String generateCode(int org, int comp, int yearId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, "
              + "@BranchesId=?, @Activity=?", org, comp, DOC_TYPE, yearId, 0, "GenerateVoucherCodeByDocumentTypeId");
        return rows.isEmpty() ? "" : str(ci(rows.get(0), "VoucherCode"));
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal("UnAppropriatedProfitAndLoss") -> ToInt. */
    private int unAppropriatedAccount(int org, int comp) {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@ConfigDescription=?, @DefinitionIds=?, @Activity=?",
                    org, comp, "UnAppropriatedProfitAndLoss", null, "GetConfigurationByOrgCompandConfigDescription");
            return rows.isEmpty() ? 0 : asInt(ci(rows.get(0), "ConfigKey"));
        } catch (RuntimeException e) {
            return 0;
        }
    }

    // ------------------------------------------------------------------ btnShow_Click

    public Map<String, Object> show(Map<String, Object> body) {
        adm.admin();
        List<Map<String, Object>> rows = closeRows(str(body.get("closingDate")), asInt(body.get("branchId")));
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("rows", rows);
        r.put("plAmount", rows.isEmpty() ? 0d : plAmount(rows));                // empty: txtPlAmount.Text = "0"
        return r;
    }

    /** FromDate = clsGlobalVariables.ActiveYr.Start_Period; ToDate = ClosingDate; @BranchesId only when <> 0. */
    private List<Map<String, Object>> closeRows(String closingDate, int branchId) {
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId();
        java.sql.Date to = toSqlDate(closingDate);
        java.sql.Date from = activeYearStart();
        List<Map<String, Object>> raw = branchId != 0
                ? jdbc.queryForList("EXEC [dbo].[USP_FinancialYearClose] @OrganizationId=?, @CompanyId=?, @FromDate=?, @ToDate=?, @BranchesId=?",
                        org, comp, from, to, branchId)
                : jdbc.queryForList("EXEC [dbo].[USP_FinancialYearClose] @OrganizationId=?, @CompanyId=?, @FromDate=?, @ToDate=?",
                        org, comp, from, to);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId", asInt(ci(x, "AccountId")));
            m.put("AccountCode", str(ci(x, "AccountCode")));
            m.put("AccountTitle", str(ci(x, "AccountTitle")));
            m.put("AccountType", str(ci(x, "AccountType")));
            m.put("NoteTitle", str(ci(x, "NoteTitle")));
            m.put("Debit", asDouble(ci(x, "Debit")));
            m.put("Credit", asDouble(ci(x, "Credit")));
            out.add(m);
        }
        return out;
    }

    private java.sql.Date activeYearStart() {
        List<Map<String, Object>> y = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id = ?", ctx.currentFinancialYearId());
        Object v = y.isEmpty() ? null : ci(y.get(0), "Start_Period");
        if (v instanceof java.util.Date) return new java.sql.Date(((java.util.Date) v).getTime());
        throw new IllegalStateException("Active financial year not found");
    }

    /** grdSettings: (sum Debit - sum Credit).ToString("##0,##.##") - two decimals, half away from zero - read back by ToDouble. */
    private static double plAmount(List<Map<String, Object>> rows) {
        double d = 0, c = 0;
        for (Map<String, Object> r : rows) { d += asDouble(r.get("Debit")); c += asDouble(r.get("Credit")); }
        double v = BigDecimal.valueOf(d - c).setScale(2, RoundingMode.HALF_UP).doubleValue();
        return v == 0d ? 0d : v;
    }

    // ------------------------------------------------------------------ btnSave_Click / btnUpdate_Click -> Insert()

    /**
     * Insert(). The two Yes/No confirmations ("Are you sure to Save?" / "Update?") are asked by the page after the two
     * input checks and before this runs; {@code confirmed} must be true here so nothing posts without it.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        adm.admin();
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId(), user = ctx.currentUserId();
        int recId = asInt(body.get("id"));
        String docNo = body.get("docNo") == null ? "" : String.valueOf(body.get("docNo"));
        if (docNo.isEmpty()) throw new IllegalArgumentException("Document No Is Required");
        int unApp = unAppropriatedAccount(org, comp);
        if (unApp == 0) throw new IllegalArgumentException("Un-Appropriated Profit And Loss Account Configuration Not Found");
        if (!Boolean.TRUE.equals(body.get("confirmed")))
            throw new IllegalArgumentException(recId > 0 ? "Are you sure to Update?" : "Are you sure to Save?");

        String closingDate = str(body.get("closingDate"));
        int branchId = asInt(body.get("branchId"));
        int yearId = ctx.currentFinancialYearId();

        // the grid: new = USP_FinancialYearClose rows; update = the stored voucher's rows (GetDataById)
        List<Map<String, Object>> grid;
        if (recId > 0) {
            loadVoucher(recId);                                                 // GetByID: "No Record Found"
            grid = storedRows(recId);
        } else {
            grid = closeRows(closingDate, branchId);
        }
        double voucherAmount = grid.isEmpty() ? 0d : plAmount(grid);            // Conversion.ToDouble(txtPlAmount.Text)

        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id = recId > 0 ? recId : 0;
        head.DocumentTypeId = DOC_TYPE;
        head.VoucherDate = closingDate;
        head.OrganizationId = org;
        head.CompanyId = comp;
        head.BranchId = branchId;
        head.FinancialYearId = yearId;
        head.EntryUser = user;
        head.ModifyUser = user;
        head.EntryDate = now;
        head.ModifyDate = now;
        head.VoucherCode = asInt(docNo);                                        // Conversion.ToInt(txtdocno.Text)
        head.VoucherAmount = voucherAmount;
        head.Remarks = "";                                                      // as the other ported vouchers (desktop leaves it unset)
        head.ChequeNo = "";

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        if (!grid.isEmpty()) {
            if (voucherAmount != 0d) {
                ContraVoucherDto.Detail pl = new ContraVoucherDto.Detail();
                pl.AccountId = unApp;
                pl.BranchesId = branchId;
                pl.Comments = "Financial Year Close Run Auto";
                if (voucherAmount > 0d) pl.CreditAmount = voucherAmount; else pl.DebitAmount = voucherAmount;
                details.add(pl);
            }
            for (Map<String, Object> r : grid) {
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = asInt(r.get("AccountId"));
                d.AgainstAccountId = unApp;
                d.Comments = "Financial Year Close Run Auto";
                d.DebitAmount = asDouble(r.get("Debit"));
                d.CreditAmount = asDouble(r.get("Credit"));
                d.BranchesId = branchId;
                details.add(d);
            }
        } else {
            throw new IllegalArgumentException("Grid Record Not Found");
        }

        writer.save(head, details, new ArrayList<>());                          // VoucherHead.Save -> SetData chain, one transaction

        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + head.VoucherCode);
        r.put("docNo", generateCode(org, comp, yearId));                        // Reset()
        return r;
    }

    // ------------------------------------------------------------------ history tab / Edit

    public Map<String, Object> history() {
        adm.admin();
        List<Map<String, Object>> raw = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?",
                "ReadAll", ctx.currentOrganizationId(), ctx.currentCompanyId(), DOC_TYPE);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(x, "Id")));
            m.put("VoucherCode", str(ci(x, "VoucherCode")));
            Object d = ci(x, "VoucherDate");
            m.put("VoucherDate", d instanceof java.util.Date ? new SimpleDateFormat("dd/MM/yyyy").format((java.util.Date) d) : str(d));
            m.put("VoucherAmount", str(ci(x, "VoucherAmount")));
            m.put("NoOfAttachments", asInt(ci(x, "NoOfAttachments")));
            out.add(m);
        }
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("rows", out);
        return r;
    }

    /** GetDataById: VoucherHead.GetByID then the detail rows shown as AccountId/AccountTitle/Debit/Credit. */
    public Map<String, Object> byId(int id) {
        adm.admin();
        Map<String, Object> h = loadVoucher(id);
        List<Map<String, Object>> rows = storedRows(id);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("id", id);
        r.put("docNo", str(ci(h, "VoucherCode")));
        r.put("closingDate", isoDay(ci(h, "VoucherDate")));
        r.put("rows", rows);
        r.put("plAmount", plAmount(rows));
        return r;
    }

    private Map<String, Object> loadVoucher(int id) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "ReadByID");
        if (rows.isEmpty()) throw new IllegalStateException("No Record Found");
        Map<String, Object> h = rows.get(0);
        // the web has to refuse another company's / another document type's id; the desktop never sees one
        if (asInt(ci(h, "DocumentTypeId")) != DOC_TYPE
                || asInt(ci(h, "OrganizationId")) != ctx.currentOrganizationId()
                || asInt(ci(h, "CompanyId")) != ctx.currentCompanyId())
            throw new IllegalStateException("No Record Found");
        return h;
    }

    private List<Map<String, Object>> storedRows(int id) {
        List<Map<String, Object>> raw = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "VoucherDetail_ReadByVoucherHeadID");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId", asInt(ci(x, "AccountId")));
            m.put("AccountTitle", str(ci(x, "AccountTitle")));
            m.put("Debit", asDouble(ci(x, "DebitAmount")));
            m.put("Credit", asDouble(ci(x, "CreditAmount")));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------ helpers

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }
    private static int asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
    private static double asDouble(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0d;
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }
    private static java.sql.Date toSqlDate(String iso) {
        try { return java.sql.Date.valueOf(iso == null ? "" : iso.trim().substring(0, 10)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Closing Date is invalid"); }
    }
    private static String isoDay(Object d) {
        if (d instanceof java.util.Date) return new SimpleDateFormat("yyyy-MM-dd").format((java.util.Date) d);
        String s = str(d);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
