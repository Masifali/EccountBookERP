package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Approvals - vouchers (Architecture.WinApp.ApprovalDashboard\PendingApprovalVouchersHistory.cs, 1,777 lines).
 *
 * Opened by ApprovalDashboard.UserControl_Click (:244-259) for card TypeID 0-9, 18, 34, 35 and by
 * UnApprovedInvoicesAndVouchers.UserControl_Click (:162-176) for 0-9, 34, 35 with flagAproved = true.
 * The card's TypeID is the form's static Id; Load (:183-197) maps it to a DocumentTypeId.
 *
 * Procedures, read from BLL Architecture.BLL.Accounts.VoucherHead (0654):
 *
 *   HistoryFill -> VoucherForApprovalDashboard (:985)            USp_PendingVoucherForApprovalDashboard
 *       @OrganizationId @CompanyId @FinancialYearId @DocumentTypeId always; @FromDate @ToDate (always set by
 *       the form); @DocNoFrom / @DocNoTo only when non-zero. @UserId, @PendingForViewId and @IsApproved are
 *       NOT sent: the form leaves UserId 0 and sets both filters to "All".
 *   PendingForViewVouchers -> PendingVoucherReconcilationForApprovalDashboard (:1090)
 *       Sp_Vouchers_GetMethods @OrganizationId @CompanyId @DocumentTypeName(=DocumentTypeId as text)
 *       @FinancialYearId (non-zero) @Activity
 *   PendingVoucherForUnApprovalDashboard (:1147)
 *       Sp_Vouchers_GetMethods @OrganizationId @CompanyId @DocumentTypeId @FinancialYearId @FromDate @ToDate
 *       @DocNoFrom/@DocNoTo (non-zero) @Activity
 *   GridDetailByVoucherHeadId -> GetDetailByVoucherHeadId (:1421)  Sp_Vouchers_GetMethods @Id @Activity
 *   Approve / UnApprove / PendingForView -> UpdateStatusandIsapprovedByVoucherId (:929), one EXEC per row:
 *       Sp_Vouchers_GetMethods @Activity @OrganizationId @CompanyId @Id @PostDate @EntryUser @ActionTypeId @ReqType
 *         Approve          ActionTypeId false, ReqType 'AP'
 *         PendingForView   ActionTypeId true,  ReqType 'AP'
 *         UnApprove        ActionTypeId false, ReqType 'UP'
 *   NoOfAttachments link -> CommonServices.GetNoofAttachmentsByRefDocumentTypeID
 *       -> DMSAttachments.ReadAttachmentsbyRefDocumentTypeId
 *       Sp_DMSAttachments_GetAllMethod @RefDocumentTypeId @Id @Activity='ReadAttachmentsbyRefDocumentTypeId'
 *
 * Organisation, company, user and financial year always come from CurrentUserContext.
 */
@Service
public class PendingApprovalVouchersHistoryService {

    private static final Logger LOG = LoggerFactory.getLogger(PendingApprovalVouchersHistoryService.class);

    /** PendingApprovalVouchersHistory_Load :183-197 - card TypeID -> DocumentTypeId, 0 when unmapped. */
    private static final Map<Integer, Integer> DOCUMENT_TYPE_MAP = new LinkedHashMap<>();
    static {
        DOCUMENT_TYPE_MAP.put(0, 1);
        DOCUMENT_TYPE_MAP.put(1, 2);
        DOCUMENT_TYPE_MAP.put(2, 3);
        DOCUMENT_TYPE_MAP.put(3, 4);
        DOCUMENT_TYPE_MAP.put(4, 6);
        DOCUMENT_TYPE_MAP.put(5, 7);
        DOCUMENT_TYPE_MAP.put(6, 10);
        DOCUMENT_TYPE_MAP.put(7, 26);
        DOCUMENT_TYPE_MAP.put(8, 5);
        DOCUMENT_TYPE_MAP.put(9, 27);
        DOCUMENT_TYPE_MAP.put(18, 203);
        DOCUMENT_TYPE_MAP.put(34, 34);
        DOCUMENT_TYPE_MAP.put(35, 35);
    }

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private AccountReportsDesktopService accountReports;

    public static int documentTypeFor(int cardId) {
        Integer t = DOCUMENT_TYPE_MAP.get(cardId);
        return t == null ? 0 : t;
    }

    /**
     * PendingApprovalVouchersHistory_Load :177-226.
     * fromdate = ActiveYr.Start_Period; when BOTH RequestedFromDate and RequestedToDate are set (the dashboard
     * sets From only when its tick box is on, To always) both pickers take them, otherwise To keeps its
     * designer default (today).
     */
    public Map<String, Object> setup(int cardId, String requestedFrom, String requestedTo, boolean unApprove) {
        Map<String, Object> out = new LinkedHashMap<>();
        int documentTypeId = documentTypeFor(cardId);
        out.put("id", cardId);
        out.put("documentTypeId", documentTypeId);
        out.put("unApprove", unApprove);
        boolean allowEdit = false;
        try { allowEdit = accountReports.configBool("AllowEditOnVoucherNoinReports"); }
        catch (Exception e) { LOG.error("AllowEditOnVoucherNoinReports lookup failed", e); }
        out.put("allowEditOnVoucherNoinReports", allowEdit);

        String yearStart = "";
        try { yearStart = financialYearStart(); }
        catch (Exception e) { LOG.error("Financial year start lookup failed", e); }
        out.put("financialYearStart", yearStart);

        String from = iso(requestedFrom);
        String to = iso(requestedTo);
        if (from != null && to != null) {
            out.put("fromDate", from);
            out.put("toDate", to);
        } else {
            out.put("fromDate", yearStart);
            out.put("toDate", LocalDate.now().toString());
        }
        out.put("title", unApprove ? "Vouchers For UnApproval" : "Vouchers For Approval");
        out.put("buttonText", unApprove ? "UnApprove" : "Approve");
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the active year's row of Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    private String financialYearStart() {
        int year = ctx.currentFinancialYearId();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()))) {
            if (asInt(r.get("Id")) == year) {
                Object s = r.get("Start_Period");
                return s == null ? "" : iso(String.valueOf(s)) == null ? "" : iso(String.valueOf(s));
            }
        }
        return "";
    }

    /** HistoryFill :228-275 -> USp_PendingVoucherForApprovalDashboard. */
    public List<Map<String, Object>> history(int cardId, String fromDate, String toDate, int docNoFrom, int docNoTo) {
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "DocumentTypeId", documentTypeFor(cardId),
                "FromDate", sqlDate(fromDate),
                "ToDate", sqlDate(toDate),
                "DocNoFrom", docNoFrom != 0 ? docNoFrom : null,
                "DocNoTo", docNoTo != 0 ? docNoTo : null);
        return clean(DesktopProc.rows(jdbc, "USp_PendingVoucherForApprovalDashboard", p));
    }

    /** PendingVoucherForUnApprovalDashboard :277-322 (flagAproved = true). */
    public List<Map<String, Object>> forUnApproval(int cardId, String fromDate, String toDate, int docNoFrom, int docNoTo) {
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", documentTypeFor(cardId),
                "FinancialYearId", fy != 0 ? fy : null,
                "FromDate", sqlDate(fromDate),
                "ToDate", sqlDate(toDate),
                "DocNoFrom", docNoFrom != 0 ? docNoFrom : null,
                "DocNoTo", docNoTo != 0 ? docNoTo : null,
                "Activity", "PendingVoucherForUnApprovalDashboard");
        return clean(DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", p));
    }

    /** PendingForViewVouchers :324-383. */
    public List<Map<String, Object>> pendingForView(int cardId) {
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeName", String.valueOf(documentTypeFor(cardId)),
                "FinancialYearId", fy != 0 ? fy : null,
                "Activity", "PendingVoucherReconcilationForApprovalDashboard");
        return clean(DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", p));
    }

    /** GridDetailByVoucherHeadId :717-741. */
    public List<Map<String, Object>> detail(int voucherHeadId) {
        requireOwnVoucher(voucherHeadId);
        return clean(DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", voucherHeadId, "Activity", "GetDetailByVoucherHeadId")));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID -> AttachmentName / CustomName / EntryDate. */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "RefDocumentTypeId", documentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (r.containsKey("OrganizationId") && asInt(r.get("OrganizationId")) != org) continue;
            if (r.containsKey("CompanyId") && asInt(r.get("CompanyId")) != comp) continue;
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("AttachmentName", r.get("Attachment"));
            a.put("CustomName", r.get("UploadedFileCustomName"));
            a.put("EntryDate", value(r.get("EntryDate")));
            out.add(a);
        }
        return out;
    }

    /**
     * VoucherHead.UpdateStatusandIsapprovedByVoucherId - one EXEC per row, in grid order, all in one transaction.
     * reqType 'AP' / 'UP'; actionTypeId false = approve / un-approve, true = move to the Pending-For-View grid.
     */
    @Transactional
    public int updateStatus(List<Integer> ids, boolean actionTypeId, String reqType) {
        if (ids == null || ids.isEmpty()) throw new IllegalArgumentException("Please select check box first");
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        int user = ctx.currentUserId();
        for (Integer id : ids) requireOwnVoucher(id == null ? 0 : id);
        for (Integer id : ids) {
            DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                    "Activity", "UpdateStatusandIsapprovedByVoucherId",
                    "OrganizationId", org,
                    "CompanyId", comp,
                    "Id", id,
                    "PostDate", new java.sql.Timestamp(System.currentTimeMillis()),
                    "EntryUser", user,
                    "ActionTypeId", actionTypeId,
                    "ReqType", reqType));
        }
        return ids.size();
    }

    /** Not on the desktop: the procedures take a bare voucher id, so the web checks it is this company's voucher. */
    private void requireOwnVoucher(int voucherHeadId) {
        Integer n = jdbc.queryForObject(
                "SELECT COUNT(1) FROM VoucherHead WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, voucherHeadId, ctx.currentOrganizationId(), ctx.currentCompanyId());
        if (n == null || n == 0) throw new IllegalArgumentException("VoucherId Not Found");
    }

    // ---------------------------------------------------------------- helpers

    private static List<Map<String, Object>> clean(List<Map<String, Object>> rows) {
        if (rows == null) return Collections.emptyList();
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) m.put(e.getKey(), value(e.getValue()));
            out.add(m);
        }
        return out;
    }

    /** Dates go out as ISO text so the page formats them itself. */
    private static Object value(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toString();
        if (v instanceof java.time.temporal.TemporalAccessor) return v.toString();
        if (v instanceof java.math.BigDecimal) return ((java.math.BigDecimal) v).doubleValue();
        return v;
    }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static String iso(String s) {
        if (s == null) return null;
        String t = s.trim();
        if (t.length() < 10) return null;
        try { return LocalDate.parse(t.substring(0, 10)).toString(); } catch (Exception e) { return null; }
    }

    private static Object sqlDate(String s) {
        String t = iso(s);
        return t == null ? null : java.sql.Date.valueOf(LocalDate.parse(t));
    }
}
