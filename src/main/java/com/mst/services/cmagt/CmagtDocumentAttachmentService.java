package com.mst.services.cmagt;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.DesktopInventoryItemFileService;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

/** DMS attachment control used by the Commission Trading GRN and GDN forms. */
@Service
public class CmagtDocumentAttachmentService {
    private static final String SCREEN_GRN = GrnLoadingChallanCmagtService.DESKTOP_SCREEN_NAME;
    private static final String SCREEN_GDN = GoodsDispatchingNoteCmagtService.DESKTOP_SCREEN_NAME;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopAttachmentStore store;
    private final GrnLoadingChallanCmagtService grn;
    private final GoodsDispatchingNoteCmagtService gdn;

    public CmagtDocumentAttachmentService(JdbcTemplate jdbc, CurrentUserContext context,
            DesktopAttachmentStore store, GrnLoadingChallanCmagtService grn,
            GoodsDispatchingNoteCmagtService gdn) {
        this.jdbc = jdbc;
        this.context = context;
        this.store = store;
        this.grn = grn;
        this.gdn = gdn;
    }

    public static class Request {
        public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
        public List<Integer> removeAttachmentIds = new ArrayList<>();
    }

    public record Download(String name, byte[] bytes) {}
    private record Document(String screen, int typeId, int accountId) {}

    public List<Map<String, Object>> list(String screenKey, int id) {
        Document document = document(screenKey, id);
        UserAccount user = context.requireAccountingUser();
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity=?",
                        document.screen(), id, "ReadById").stream()
                .filter(row -> number(value(row, "OrganizationId")) == user.getOrganizationId()
                        && number(value(row, "CompanyId")) == user.getCompanyId()
                        && number(value(row, "RefDocumentTypeId")) == document.typeId())
                .toList();
    }

    @Transactional
    public List<Map<String, Object>> save(String screenKey, int id, Request request) {
        Document document = document(screenKey, id);
        UserAccount user = context.requireAccountingUser();
        if (request == null || request.files == null || request.removeAttachmentIds == null
                || request.files.size() > 10 || request.removeAttachmentIds.size() > 100) {
            throw new IllegalArgumentException("Select at most ten files at once");
        }

        List<Map<String, Object>> existing = list(screenKey, id);
        Set<Integer> removals = new HashSet<>(request.removeAttachmentIds);
        for (Integer attachmentId : removals) {
            if (attachmentId == null || existing.stream().noneMatch(row -> number(value(row, "Id")) == attachmentId)) {
                throw new IllegalArgumentException("Attachment does not belong to this document");
            }
        }
        for (Integer attachmentId : removals) {
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?,@Activity=?",
                    attachmentId, "AttachmentDeleteById");
        }

        Timestamp now = new Timestamp(System.currentTimeMillis());
        for (InventoryPosItemRequest.Upload upload : request.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(upload);
            String stored = store.store(user, upload.name, bytes);
            Object[] values = {0, document.accountId(), 0, document.typeId(), id, upload.name,
                    now, user.getId(), now, user.getId(), user.getOrganizationId(), user.getCompanyId(),
                    user.getBranchesId(), document.screen(), false, stored, bytes.length / 1048576d, 0};
            jdbc.execute("EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,"
                    + "@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,"
                    + "@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@DetailWiseAttachment=?,"
                    + "@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?",
                    (org.springframework.jdbc.core.PreparedStatementCallback<Void>) statement -> {
                        for (int i = 0; i < values.length; i++) statement.setObject(i + 1, values[i]);
                        boolean result = statement.execute();
                        while (result || statement.getUpdateCount() != -1) {
                            if (result) try (var rs = statement.getResultSet()) { while (rs.next()) { } }
                            result = statement.getMoreResults();
                        }
                        return null;
                    });
        }
        return list(screenKey, id);
    }

    public Download download(String screenKey, int id, int attachmentId) {
        UserAccount user = context.requireAccountingUser();
        Map<String, Object> row = list(screenKey, id).stream()
                .filter(candidate -> number(value(candidate, "Id")) == attachmentId)
                .findFirst().orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Attachment not found"));
        String original = Objects.toString(value(row, "Attachment"), "");
        String stored = Objects.toString(value(row, "UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = original;
        return new Download(baseName(original), store.read(user, baseName(stored)));
    }

    private Document document(String screenKey, int id) {
        if (id <= 0) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Save this document before opening attachments");
        Map<String, Object> record;
        String screen;
        int type;
        if ("grn".equalsIgnoreCase(screenKey)) {
            record = data(grn.getById(id));
            screen = SCREEN_GRN;
            type = GrnLoadingChallanCmagtService.DOCUMENT_TYPE_ID;
        } else if ("gdn".equalsIgnoreCase(screenKey)) {
            record = data(gdn.getById(id));
            screen = SCREEN_GDN;
            type = GoodsDispatchingNoteCmagtService.DOCUMENT_TYPE_ID;
        } else {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Unknown Commission Trading document");
        }
        String accountColumn = "gdn".equalsIgnoreCase(screenKey) ? "buyerId" : "supplierId";
        return new Document(screen, type, number(value(record, accountColumn)));
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> data(Map<String, Object> response) {
        Object status = value(response, "status");
        Object data = value(response, "data");
        if (!"SUCCESS".equalsIgnoreCase(Objects.toString(status, "")) || !(data instanceof Map<?, ?>)) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Commission Trading document not found");
        }
        return (Map<String, Object>) data;
    }

    private static Object value(Map<String, Object> map, String key) {
        if (map == null) return null;
        for (Map.Entry<String, Object> entry : map.entrySet())
            if (entry.getKey().equalsIgnoreCase(key)) return entry.getValue();
        return null;
    }

    private static int number(Object value) {
        if (value instanceof Number n) return n.intValue();
        try { return value == null ? 0 : (int) Double.parseDouble(value.toString()); }
        catch (NumberFormatException ignored) { return 0; }
    }

    private static String baseName(String value) {
        String name = value.replace('\\', '/');
        name = name.substring(name.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name);
        return name;
    }
}
