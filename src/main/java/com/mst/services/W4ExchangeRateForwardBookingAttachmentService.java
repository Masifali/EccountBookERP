package com.mst.services;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Set;

import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;

/**
 * Attachments of Account_Definition.ExchangeRateForwardBooking (screen name "ExchangeRateForwardBooking").
 *
 * Desktop chain:
 *  - ReadById (:672-): DMSAttachments.GetByID(RecId, base.Name) -> Sp_DMSAttachments_GetAllMethod
 *    @ScreenName, @Id, @Activity='ReadById'. The grid's NoOfAttachments link (CommonServices.GetNoofAttachmentsByScreenName,
 *    :898-912) reads the same list.
 *  - Save (Insert :584-654): FormHelper.UpdateAttachmentsForObject, then DAL fcyExRateForwardBooking.SetData runs
 *    USP_FcyExRateForwardBooking_InsertAndUpdate and, in the same transaction, Proc_DMSAttachments_Insert per attachment
 *    with RefAccountId = BankId, RefDocumentTypeId = 0, RefDocumentNo = the saved Id. The update branch of the procedure
 *    itself runs DELETE FROM DMSAttachments for this ScreenName and RefDocumentNo first, so the kept attachments are
 *    inserted again together with the new ones.
 *  - Update with every attachment removed: DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 0, 0) (:633-636).
 * File bytes go through DesktopAttachmentStore, like the other screens.
 */
@Service
public class W4ExchangeRateForwardBookingAttachmentService {

    public static final String SCREEN = "ExchangeRateForwardBooking";
    public static final int DOC_TYPE = 0;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public W4ExchangeRateForwardBookingAttachmentService(JdbcTemplate jdbc, CurrentUserContext ctx, DesktopAttachmentStore store) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.store = store;
    }

    /** What the Attachment form holds after the user's changes: rows kept, rows added, ids removed. */
    public static final class Prepared {
        public final List<Map<String, Object>> retained = new ArrayList<>();
        public final List<Map<String, Object>> added = new ArrayList<>();
        public final Set<Integer> removed = new HashSet<>();
        public int total() { return retained.size() + added.size(); }
    }

    /** DMSAttachments.GetByID(id, "ExchangeRateForwardBooking"), restricted to the signed-in company. */
    public List<Map<String, Object>> list(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "ScreenName", SCREEN, "Id", id, "Activity", "ReadById"));
        List<Map<String, Object>> out = new ArrayList<>();
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId();
        for (Map<String, Object> r : rows) {
            if (num(r.get("OrganizationId")) != org || num(r.get("CompanyId")) != comp) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", num(r.get("Id")));
            m.put("Attachment", str(r.get("Attachment")));
            m.put("FileName", str(r.get("UploadedFileCustomName")));
            Object size = r.get("UploadedFileSizeMb");
            m.put("SizeMb", size instanceof Number ? ((Number) size).doubleValue() : 0d);
            m.put("EntryDate", r.get("EntryDate") == null ? "" : String.valueOf(r.get("EntryDate")));
            m.put("EntryUserName", str(r.get("EntryUserName")));
            out.add(m);
        }
        return out;
    }

    /** Builds the list the desktop's AT.lst would hold at Save time; new files are written to the store here. */
    public Prepared prepare(int id, Object filesObj, Object removeObj) {
        List<?> files = filesObj instanceof List ? (List<?>) filesObj : List.of();
        List<?> removeIds = removeObj instanceof List ? (List<?>) removeObj : List.of();
        if (files.size() > 10) throw new IllegalArgumentException("Select at most ten files at once");
        List<Map<String, Object>> existing = id > 0 ? list(id) : List.of();
        Prepared p = new Prepared();
        for (Object o : removeIds) {
            int rid = num(o);
            if (rid <= 0 || existing.stream().noneMatch(e -> num(e.get("Id")) == rid))
                throw new IllegalArgumentException("Attachment does not belong to this Exchange Rate Forward Booking");
            p.removed.add(rid);
        }
        for (Map<String, Object> e : existing) if (!p.removed.contains(num(e.get("Id")))) p.retained.add(e);
        for (Object o : files) {
            if (!(o instanceof Map)) throw new IllegalArgumentException("Invalid attachment");
            Map<?, ?> f = (Map<?, ?>) o;
            String name = f.get("name") == null ? "" : String.valueOf(f.get("name"));
            DesktopAttachmentStore.validateName(name);
            String b64 = f.get("base64") == null ? "" : String.valueOf(f.get("base64"));
            if (b64.length() > 7 * 1024 * 1024) throw new IllegalArgumentException("Attachment exceeds 5 MB");
            byte[] bytes;
            try { bytes = Base64.getDecoder().decode(b64); }
            catch (IllegalArgumentException ex) { throw new IllegalArgumentException("Invalid attachment content"); }
            if (bytes.length == 0 || bytes.length > DesktopAttachmentStore.MAX_BYTES)
                throw new IllegalArgumentException("Attachment must be between 1 byte and 5 MB");
            String saved = store.store(ctx.requireAccountingUser(), name, bytes);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Attachment", name);
            m.put("FileName", saved);
            m.put("SizeMb", bytes.length / 1048576d);
            p.added.add(m);
        }
        return p;
    }

    /**
     * The attachment part of the DAL SetData (after the header row) plus RemoveByIdAndNames. The update branch of the
     * procedure has already deleted the old rows, so only the insert is done here.
     */
    public void persist(int headerId, int bankId, boolean update, Prepared p) {
        if (p.total() > 0) {
            List<Map<String, Object>> all = new ArrayList<>(p.retained);
            all.addAll(p.added);
            for (Map<String, Object> r : all) {
                Timestamp now = new Timestamp(System.currentTimeMillis());
                ProcExec.call(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,"
                        + "@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,"
                        + "@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@DetailWiseAttachment=?,"
                        + "@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?",
                        0, bankId, 0, DOC_TYPE, headerId, str(r.get("Attachment")), now, ctx.currentUserId(), now,
                        ctx.currentUserId(), ctx.currentOrganizationId(), ctx.currentCompanyId(), 0, SCREEN, false,
                        str(r.get("FileName")), ((Number) r.get("SizeMb")).doubleValue(), 0);
            }
        } else if (update && !p.removed.isEmpty()) {
            /* if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0) RemoveByIdAndNames(RecId, Name, 0, 0) */
            DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                    "Id", headerId, "ScreenName", SCREEN, "RefDocumentTypeId", DOC_TYPE, "Activity", "RemoveByIdAndName"));
        }
    }

    public static final class Download {
        private final String name;
        private final byte[] bytes;
        public Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
        public String name() { return name; }
        public byte[] bytes() { return bytes; }
    }

    /** Opens a stored attachment of the signed-in company (the Attachment form's click-to-open). */
    public Download download(int headerId, int attachmentId) {
        Map<String, Object> row = list(headerId).stream().filter(r -> num(r.get("Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Attachment not found"));
        String original = str(row.get("Attachment"));
        String stored = str(row.get("FileName"));
        String name = base(original);
        return new Download(name, store.read(ctx.requireAccountingUser(), base(stored.isBlank() ? original : stored)));
    }

    private static String base(String v) {
        String n = v.replace('\\', '/');
        n = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(n);
        return n;
    }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static String str(Object o) { return o == null ? "" : Objects.toString(o, ""); }
}
