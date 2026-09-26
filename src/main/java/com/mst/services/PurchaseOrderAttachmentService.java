package com.mst.services;

import com.mst.models.dto.PurchaseOrderAttachmentsDto;
import com.mst.repositories.PurchaseOrderRecordRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import java.sql.Timestamp;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.NOT_FOUND;

/** PurchsaeOrder.cs:3653-3658 / DAL PurchaseOrder.SetData: attachment changes share the order transaction. */
@Service
public class PurchaseOrderAttachmentService {
    private static final String SCREEN = "PurchsaeOrder";
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopAttachmentStore store;
    private final PurchaseOrderRecordRepository records;
    public PurchaseOrderAttachmentService(JdbcTemplate jdbc, CurrentUserContext context,
            DesktopAttachmentStore store, PurchaseOrderRecordRepository records) {
        this.jdbc=jdbc; this.context=context; this.store=store; this.records=records;
    }

    public List<Map<String,Object>> list(int id) {
        records.require(id);
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity=?",
                SCREEN, id, "ReadById").stream().filter(r -> number(r.get("OrganizationId")) == context.currentOrganizationId()
                        && number(r.get("CompanyId")) == context.currentCompanyId()
                        && number(r.get("RefDocumentTypeId")) == 41).toList();
    }

    public record Prepared(List<Map<String,Object>> retained, List<Map<String,Object>> added, Set<Integer> removed) {
        public String names() { return joined("Attachment"); }
        public String storedNames() { return joined("UploadedFileCustomName"); }
        private String joined(String key) { return java.util.stream.Stream.concat(retained.stream(), added.stream())
                .map(r -> Objects.toString(r.get(key), "")).collect(Collectors.joining(",")); }
    }

    public Prepared prepare(int id, PurchaseOrderAttachmentsDto change) {
        var existing = id > 0 ? list(id) : List.<Map<String,Object>>of();
        if (change == null) change = new PurchaseOrderAttachmentsDto();
        if (change.files == null || change.removeAttachmentIds == null || change.files.size() > 10)
            throw new IllegalArgumentException("Select at most ten files at once");
        Set<Integer> removed = new HashSet<>(change.removeAttachmentIds);
        for (Integer attachmentId : removed) {
            if (attachmentId == null || existing.stream().noneMatch(r -> number(r.get("Id")) == attachmentId))
                throw new IllegalArgumentException("Attachment does not belong to this Purchase Order");
        }
        var added = new ArrayList<Map<String,Object>>();
        for (var upload : change.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(upload);
            String stored = store.store(context.requireAccountingUser(), upload.name, bytes);
            added.add(Map.of("Attachment", upload.name, "UploadedFileCustomName", stored,
                    "UploadedFileSizeMb", bytes.length / 1048576d));
        }
        return new Prepared(existing.stream().filter(r -> !removed.contains(number(r.get("Id")))).toList(), added, removed);
    }

    public void persist(int id, int supplierId, Prepared change) {
        for (Integer attachmentId : change.removed()) {
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?,@Activity=?", attachmentId, "AttachmentDeleteById");
        }
        Timestamp now = new Timestamp(System.currentTimeMillis());
        for (var row : change.added()) {
            ProcExec.call(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,"
                    + "@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,"
                    + "@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@DetailWiseAttachment=?,"
                    + "@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?", 0, supplierId, 0, 41, id,
                    row.get("Attachment"), now, context.currentUserId(), now, context.currentUserId(),
                    context.currentOrganizationId(), context.currentCompanyId(), context.currentBranchId(), SCREEN,
                    false, row.get("UploadedFileCustomName"), row.get("UploadedFileSizeMb"), 0);
        }
    }

    public record Download(String name, byte[] bytes) {}
    public Download download(int id, int attachmentId) {
        var row = list(id).stream().filter(r -> number(r.get("Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new ResponseStatusException(NOT_FOUND, "Attachment not found"));
        String original = Objects.toString(row.get("Attachment"), "");
        String stored = Objects.toString(row.get("UploadedFileCustomName"), "");
        return new Download(baseName(original), store.read(context.requireAccountingUser(), baseName(stored.isBlank() ? original : stored)));
    }
    private static int number(Object v) { return v instanceof Number ? ((Number)v).intValue() : 0; }
    private static String baseName(String v) { String name = v.replace('\\','/'); name=name.substring(name.lastIndexOf('/')+1); DesktopAttachmentStore.validateName(name); return name; }
}
