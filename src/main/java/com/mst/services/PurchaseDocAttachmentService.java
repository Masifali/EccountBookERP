package com.mst.services;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.models.dto.PurchaseOrderAttachmentsDto;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import java.sql.Timestamp;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.NOT_FOUND;

/**
 * DMS attachments ("Attachment" toolbar button, AT.Show()) of the GRN family and Stock In Transit:
 *   46  InvFrmGRN                    (InvFrmGRN.cs :3905-3914, DAL 0429 InvGrn.SetData :59-85 / :143-177 / :345)
 *   143 SaleReturnGrn                (SaleReturnGrn.cs :1300-1301 / :1462-1471, same DAL)
 *   137 InvFrmGRNDirect              (InvFrmGRNDirect.cs :2046-2055, same DAL)
 *   251 frmSupplierDispatchPreBill   (frmSupplierDispatchPreBill.cs :2267-2278, DAL 0383 SupplierDispatch.SetData :74-106)
 * The DMS rows are keyed by ScreenName (the form's Name) + record Id (Sp_DMSAttachments_GetAllMethod 'ReadById').
 * Storage is DesktopAttachmentStore (configuration "IsVpsAttachmentsServiceOn" / "Attachment Folder Path"), the
 * same as PurchaseOrderAttachmentService; changes are written inside the document's own save transaction.
 *
 * Record access (tenancy, CanView AllRecord) is checked by the caller before list()/download().
 */
@Service
public class PurchaseDocAttachmentService {
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopAttachmentStore store;

    public PurchaseDocAttachmentService(JdbcTemplate jdbc, CurrentUserContext context, DesktopAttachmentStore store) {
        this.jdbc = jdbc; this.context = context; this.store = store;
    }

    /** base.Name of the desktop form (the DMS ScreenName). */
    public static String screen(int type) {
        return switch (type) {
            case 46 -> "InvFrmGRN";
            case 143 -> "SaleReturnGrn";
            case 137 -> "InvFrmGRNDirect";
            case 251 -> "frmSupplierDispatchPreBill";
            default -> throw new IllegalArgumentException("Unsupported attachment form");
        };
    }

    public List<Map<String, Object>> list(int id, int type) {
        String name = screen(type);
        if (id <= 0) return List.of();
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity=?", name, id, "ReadById")
                .stream().filter(r -> number(r.get("OrganizationId")) == context.currentOrganizationId()
                        && number(r.get("CompanyId")) == context.currentCompanyId()
                        && number(r.get("RefDocumentTypeId")) == type).toList();
    }

    public record Prepared(List<Map<String, Object>> retained, List<Map<String, Object>> added, Set<Integer> removed) {
        public String names() { return joined("Attachment"); }
        public String storedNames() { return joined("UploadedFileCustomName"); }
        public boolean changed() { return !added.isEmpty() || !removed.isEmpty(); }
        private String joined(String key) {
            return java.util.stream.Stream.concat(retained.stream(), added.stream())
                    .map(r -> Objects.toString(r.get(key), "")).collect(Collectors.joining(","));
        }
    }

    /**
     * The page's staged change ({files:[{name,base64}], removeAttachmentIds:[...]}), or null when the operator did
     * not open/change the attachments (then nothing is touched and the stored AttachmentsValues stay).
     * New files are stored now; DesktopAttachmentStore removes them again if the transaction rolls back.
     */
    public Prepared prepare(int id, int type, Object supplied) {
        screen(type);
        if (supplied == null) return null;
        PurchaseOrderAttachmentsDto change = new ObjectMapper().convertValue(supplied, PurchaseOrderAttachmentsDto.class);
        if (change == null) return null;
        if (change.files == null || change.removeAttachmentIds == null || change.files.size() > 10)
            throw new IllegalArgumentException("Select at most ten files at once");
        var existing = id > 0 ? list(id, type) : List.<Map<String, Object>>of();
        Set<Integer> removed = new HashSet<>(change.removeAttachmentIds);
        for (Integer attachmentId : removed) {
            if (attachmentId == null || existing.stream().noneMatch(r -> number(r.get("Id")) == attachmentId))
                throw new IllegalArgumentException("Attachment does not belong to this record");
        }
        var added = new ArrayList<Map<String, Object>>();
        for (var upload : change.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(upload);
            String stored = store.store(context.requireAccountingUser(), upload.name, bytes);
            added.add(Map.of("Attachment", upload.name, "UploadedFileCustomName", stored, "UploadedFileSizeMb", bytes.length / 1048576d));
        }
        return new Prepared(existing.stream().filter(r -> !removed.contains(number(r.get("Id")))).toList(), added, removed);
    }

    /** Removed rows go by Id ('AttachmentDeleteById'), new rows through Proc_DMSAttachments_Insert. */
    public void persist(int id, int type, int refAccountId, Prepared change) {
        if (change == null || !change.changed()) return;
        String name = screen(type);
        for (Integer attachmentId : change.removed())
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?,@Activity=?", attachmentId, "AttachmentDeleteById");
        Timestamp now = new Timestamp(System.currentTimeMillis());
        for (var row : change.added()) {
            ProcExec.call(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,"
                    + "@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,"
                    + "@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@DetailWiseAttachment=?,"
                    + "@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?", 0, refAccountId, 0, type, id,
                    row.get("Attachment"), now, context.currentUserId(), now, context.currentUserId(),
                    context.currentOrganizationId(), context.currentCompanyId(), context.currentBranchId(), name,
                    false, row.get("UploadedFileCustomName"), row.get("UploadedFileSizeMb"), 0);
        }
    }

    public record Download(String name, byte[] bytes) {}

    public Download download(int id, int type, int attachmentId) {
        var row = list(id, type).stream().filter(r -> number(r.get("Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new ResponseStatusException(NOT_FOUND, "Attachment not found"));
        String original = Objects.toString(row.get("Attachment"), "");
        String stored = Objects.toString(row.get("UploadedFileCustomName"), "");
        return new Download(baseName(original), store.read(context.requireAccountingUser(), baseName(stored.isBlank() ? original : stored)));
    }

    private static int number(Object v) {
        if (v instanceof Number n) return n.intValue();
        try { return v == null ? 0 : Integer.parseInt(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static String baseName(String v) {
        String name = v.replace('\\', '/');
        name = name.substring(name.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name);
        return name;
    }
}
