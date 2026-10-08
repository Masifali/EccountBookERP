package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.repositories.support.ProcExec;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.DesktopInventoryItemFileService;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;
import org.springframework.web.server.ResponseStatusException;

import java.sql.Timestamp;
import java.util.*;

/**
 * DMS attachments of the Sale Engr documents (the desktop's Attachment.AT dialog + the DAL SetData tail):
 *   Sp_DMSAttachments_GetAllMethod @ScreenName @Id @Activity='ReadById'       list for a document
 *   Proc_DMSAttachments_Insert per new file (RefAccountId = party, RefDocumentTypeId = document type, RefDocumentNo = document id)
 *   Sp_DMSAttachments_GetAllMethod @Id @Activity='AttachmentDeleteById'        a file the user removed
 * Files are kept by DesktopAttachmentStore (the configured attachment folder / VPS), the same store the Sale Order page uses.
 * Call apply() from inside the screen's own transaction, after the document row has been saved.
 */
@Component
public class SaleEngrAttachments {
    private final JdbcTemplate jdbc;
    private final SaleEngrSupport sup;
    private final DesktopAttachmentStore store;

    public SaleEngrAttachments(JdbcTemplate jdbc, SaleEngrSupport sup, DesktopAttachmentStore store) {
        this.jdbc = jdbc; this.sup = sup; this.store = store;
    }

    public static class Change {
        public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
        public List<Integer> removeAttachmentIds = new ArrayList<>();
    }

    public record Download(String name, byte[] bytes) { }

    public List<Map<String, Object>> list(String screen, int id) {
        UserAccount u = sup.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity=?", screen, id, "ReadById")) {
            if (SaleEngrSupport.toInt(r.get("OrganizationId")) == u.getOrganizationId() && SaleEngrSupport.toInt(r.get("CompanyId")) == u.getCompanyId()) out.add(r);
        }
        return out;
    }

    public void apply(String screen, int documentTypeId, int id, int refAccountId, Change change) {
        if (change == null) return;
        UserAccount u = sup.user();
        List<Integer> remove = change.removeAttachmentIds == null ? List.of() : change.removeAttachmentIds;
        List<InventoryPosItemRequest.Upload> files = change.files == null ? List.of() : change.files;
        if (files.size() > 10 || remove.size() > 100) throw new IllegalArgumentException("Select at most ten files at once");
        if (!remove.isEmpty()) {
            List<Map<String, Object>> existing = list(screen, id);
            for (Integer a : new HashSet<>(remove)) {
                if (a == null || existing.stream().noneMatch(r -> SaleEngrSupport.toInt(r.get("Id")) == a))
                    throw new IllegalArgumentException("Attachment does not belong to this document");
                ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?,@Activity=?", a, "AttachmentDeleteById");
            }
        }
        for (InventoryPosItemRequest.Upload upload : files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(upload);
            String stored = store.store(u, upload.name, bytes);
            Timestamp now = SaleEngrSupport.now();
            Object[] values = {0, refAccountId > 0 ? refAccountId : null, 0, documentTypeId, id, upload.name, now, u.getId(), now, u.getId(),
                    u.getOrganizationId(), u.getCompanyId(), u.getBranchesId(), screen, false, stored, bytes.length / 1048576d, 0};
            jdbc.execute("EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,@RefDocumentTypeId=?,@RefDocumentNo=?,"
                            + "@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,"
                            + "@ScreenName=?,@DetailWiseAttachment=?,@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?",
                    (org.springframework.jdbc.core.PreparedStatementCallback<Void>) st -> {
                        for (int i = 0; i < values.length; i++) st.setObject(i + 1, values[i]);
                        boolean result = st.execute();
                        while (result || st.getUpdateCount() != -1) {
                            if (result) try (var rs = st.getResultSet()) { while (rs.next()) { /* drain */ } }
                            result = st.getMoreResults();
                        }
                        return null;
                    });
        }
    }

    public Download download(String screen, int id, int attachmentId) {
        UserAccount u = sup.user();
        Map<String, Object> row = list(screen, id).stream().filter(r -> SaleEngrSupport.toInt(r.get("Id")) == attachmentId)
                .findFirst().orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(row.get("UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(row.get("Attachment"), "");
        String name = baseName(Objects.toString(row.get("Attachment"), stored));
        return new Download(name, store.read(u, baseName(stored)));
    }

    private static String baseName(String value) {
        String name = value.replace('\\', '/');
        name = name.substring(name.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name);
        return name;
    }
}
