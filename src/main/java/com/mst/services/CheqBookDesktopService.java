package com.mst.services;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 42 "Cheque Book Registration" - desktop form
 * Architecture.WinApp.Account_Definition.AcfrmChequebookRegistration, BLL/DAL Accounts.CheqBookHeader.
 *
 * <ul>
 * <li>cmbBnkac: CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15") ->
 *     Sp_COAAllocation_GetAllMethod @OrganizationId, @CompanyId, @AppId, @AccountTypeIds='15',
 *     @UserId (when != 0), @Activity='GetAccountTitleByAccountTypeIds'.</li>
 * <li>CheqbookRegistratioinGridFill: SP_CheqBookHeader_GetAllMethod @OrganizationId, @CompanyId,
 *     @MethodType='GetAll' (one row per cheque leaf; the history grid keeps the first row per Id,
 *     the BankName link shows the leaves of that Id).</li>
 * <li>Save_Click -> CheqBookHeader.Save -> DAL SetDate: SP_CheqBookHeader_Insert then one
 *     SP_CheqBookDetail_Insert per serial, one transaction.</li>
 * </ul>
 * Update ("&amp;Edit") is Visible=false with an empty handler; there is no delete.
 */
@Service
public class CheqBookDesktopService {

    public static final String SCREEN_NAME = "AcfrmChequebookRegistration";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public CheqBookDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
    }

    public Map<String, Boolean> rights() {
        return rights.of(SCREEN_NAME);
    }

    public List<Map<String, Object>> bankAccounts() {
        int user = ctx.currentUserId();
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AppId", ctx.currentAppId(),
                "AccountTypeIds", "15",
                "UserId", user != 0 ? user : null,
                "Activity", "GetAccountTitleByAccountTypeIds"));
    }

    public List<Map<String, Object>> history() {
        return DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "MethodType", "GetAll"));
    }

    /**
     * Save_Click. The quantity confirmation (100 to 499) is asked on the page before posting;
     * 500 and more is refused here as on the desktop.
     */
    @Transactional
    public String save(int bankId, LocalDate docDate, String docNoText, String prefix, String serialFrom,
                       String serialTo, String remarks) {
        Boolean canSave = rights.of(SCREEN_NAME).get("save");
        if (canSave == null || !canSave) throw new IllegalArgumentException("You do not have the Save right for this screen.");
        String fromText = serialFrom == null ? "" : serialFrom;
        String toText = serialTo == null ? "" : serialTo;
        long from = toLong(fromText), to = toLong(toText);
        if (from > to) throw new IllegalArgumentException("Serial From Can't Be Greater Than Serial To! Please Check.");
        if (from == to) throw new IllegalArgumentException("Serial From and Serial To Can't Be Same! Please Check.");
        long diff = to - from;
        if (diff >= 500) throw new IllegalArgumentException("You are not Allowed to Save Cheques? Cheques Quantity Greater Than 500");

        LocalDateTime now = LocalDateTime.now();
        String remarksText = remarks == null ? "" : remarks;
        int headerId = DesktopProc.setProc(jdbc, "SP_CheqBookHeader_Insert", DesktopProc.params(
                "PostState", false,
                "DocDate", docDate != null ? docDate.atTime(now.toLocalTime()) : now,
                "EntryDate", now, "ModifyDate", now, "PostDate", now,
                "BankId", bankId, "ChartOfAccountId", bankId,
                "CompanyId", ctx.currentCompanyId(),
                "DocNo", (int) toLong(docNoText == null ? "" : docNoText.trim()),
                "EntryUser", ctx.currentUserId(), "Id", 0, "ModifyUser", 0,
                "OrganizationId", ctx.currentOrganizationId(), "PostUser", 0,
                "CbPrefix", prefix == null ? "" : prefix.trim(),
                "CbSrFrom", fromText.trim(), "CbSrTo", toText.trim(),
                "Remarks", remarksText.trim(),
                "ActionId", 1));
        /* for (double i = ToDouble(from); i <= ToDouble(to); i++) CheqNo = Conversion.ToString(i) */
        double dFrom = toDouble(fromText), dTo = toDouble(toText);
        for (double i = dFrom; i <= dTo; i++) {
            DesktopProc.setProc(jdbc, "SP_CheqBookDetail_Insert", DesktopProc.params(
                    "CheqBookHeaderId", headerId, "Id", 0,
                    "CheqNo", clrDoubleToString(i),
                    "CheqStatus", "Blank",
                    "OtherRemarks", remarksText,
                    "CheqCancelStatus", false,
                    "StatusChangeUserId", 0));
        }
        return "Save Successfully";
    }

    /* ---------------------------------------------------------------------------------------------
       Cheque leaves of one registered book, and Cancel / Re-Open of a leaf.
       The leaf actions are the desktop's separate CheqbookStatus screen (btnsave_Click = Cancel,
       btnUpdate_Click = Open), shown on this page's right-hand grid:
         leaves      -> SP_CheqBookHeader_GetAllMethod 'ReadByHeaderId' (@RecId = header id)
         Blank/Used  -> 'OutstandingCheqNo' (@OrganizationId, @CompanyId, @BankId) - a leaf that is not
                        outstanding and not cancelled has been used by a voucher / PDC
         Remarks     -> 'CancelChequeHistory' (OtherRemarks of cancelled leaves)
         status save -> SP_CheqBookDetail_Update (Id, OtherRemarks, CheqCancelStatus, ChequeStatusDate,
                        StatusChangeUserId) - CheqBookHeader.CheqBookDetailSave / DAL SetDataDetail
       --------------------------------------------------------------------------------------------- */
    public static final String STATUS_SCREEN_NAME = "CheqbookStatus";

    public Map<String, Boolean> statusRights() {
        return rights.of(STATUS_SCREEN_NAME);
    }

    private List<Map<String, Object>> headerRows(int headerId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                "RecId", headerId, "MethodType", "ReadByHeaderId"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Cheque book not found.");
        Map<String, Object> h = rows.get(0);
        if (toInt(ci(h, "OrganizationId")) != ctx.currentOrganizationId() || toInt(ci(h, "CompanyId")) != ctx.currentCompanyId())
            throw new IllegalArgumentException("Cheque book not found.");
        return rows;
    }

    public List<Map<String, Object>> leaves(int headerId) {
        List<Map<String, Object>> rows = headerRows(headerId);
        int bankCoa = toInt(ci(rows.get(0), "ChartOfAccountId"));
        java.util.Set<Integer> outstanding = new java.util.HashSet<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "BankId", bankCoa, "MethodType", "OutstandingCheqNo")))
            outstanding.add(toInt(ci(r, "Id")));
        Map<Integer, Object> remarks = new java.util.HashMap<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "MethodType", "CancelChequeHistory")))
            remarks.put(toInt(ci(r, "Id")), ci(r, "OtherRemarks"));
        List<Map<String, Object>> out = new java.util.ArrayList<>();
        for (Map<String, Object> r : rows) {
            int detailId = toInt(ci(r, "DetailId"));
            if (detailId == 0) continue;
            boolean cancelled = truthy(ci(r, "CheqStatus"));
            Map<String, Object> m = new java.util.LinkedHashMap<>();
            m.put("id", detailId);
            m.put("cheqNo", ci(r, "CheqNo"));
            m.put("status", cancelled ? "Cancelled" : (outstanding.contains(detailId) ? "Blank" : "Used"));
            m.put("remarks", remarks.getOrDefault(detailId, ""));
            out.add(m);
        }
        out.sort((a, b) -> Long.compare(toLong(String.valueOf(a.get("cheqNo"))), toLong(String.valueOf(b.get("cheqNo")))));
        return out;
    }

    /** cancel=true: CheqbookStatus.btnsave_Click; cancel=false: btnUpdate_Click. */
    @Transactional
    public String changeStatus(int detailId, boolean cancel, String remarks) {
        Map<String, Boolean> r = rights.of(STATUS_SCREEN_NAME);
        Boolean allowed = cancel ? r.get("save") : r.get("update");
        if (allowed == null || !allowed)
            throw new IllegalArgumentException("You do not have the " + (cancel ? "Save" : "Update") + " right for Cheque Book Status.");
        List<Map<String, Object>> d = DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                "RecId", detailId, "MethodType", "ReadByDetailId"));
        if (d.isEmpty()) throw new IllegalArgumentException("CheqBook Field Required");
        Map<String, Object> leaf = d.get(0);
        headerRows(toInt(ci(leaf, "CheqBookHeaderId")));            // tenancy check
        boolean isCancelled = truthy(ci(leaf, "CheqCancelStatus"));
        if (cancel && isCancelled) throw new IllegalArgumentException("Cheq No:" + ci(leaf, "CheqNo") + " is already cancelled.");
        if (!cancel && !isCancelled) throw new IllegalArgumentException("Cheq No:" + ci(leaf, "CheqNo") + " is not cancelled.");
        if (cancel) {
            /* the desktop offers only outstanding leaves (cmbbank_Leave -> OutstandingCheqNo) */
            boolean outstanding = false;
            for (Map<String, Object> o : DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                    "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                    "BankId", toInt(ci(leaf, "BankId")), "MethodType", "OutstandingCheqNo")))
                if (toInt(ci(o, "Id")) == detailId) { outstanding = true; break; }
            if (!outstanding) throw new IllegalArgumentException("Cheq No:" + ci(leaf, "CheqNo") + " is already used and cannot be cancelled.");
        }
        /* ExecuteScalar via GenericProvider.SetProc; the proc's user-audit call may return rows, so read leniently */
        DesktopProc.scalar(jdbc, "SP_CheqBookDetail_Update", DesktopProc.params(
                "Id", detailId, "OtherRemarks", remarks == null ? "" : remarks.trim(),
                "CheqCancelStatus", cancel, "ChequeStatusDate", LocalDateTime.now(),
                "StatusChangeUserId", ctx.currentUserId()));
        return "Cheq No:" + ci(leaf, "CheqNo") + (cancel ? " Cancel Successfully" : " Open Successfully");
    }

    private static Object ci(Map<String, Object> r, String k) {
        if (r.containsKey(k)) return r.get(k);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(k)) return e.getValue();
        return null;
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        return (int) toLong(String.valueOf(o));
    }

    private static boolean truthy(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return s.equals("1") || s.equalsIgnoreCase("true");
    }

    /** Conversion.ToString(double) for a whole number: no decimals, no grouping. */
    static String clrDoubleToString(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return String.valueOf(d);
    }

    /** Conversion.ToInt64: non-numeric text is 0. */
    static long toLong(String s) {
        try { return Long.parseLong(s.trim()); } catch (Exception e) {
            try { return (long) Double.parseDouble(s.trim()); } catch (Exception e2) { return 0L; }
        }
    }

    static double toDouble(String s) {
        try { return Double.parseDouble(s.trim()); } catch (Exception e) { return 0.0; }
    }
}
