package com.mst.services.sale;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.DesktopInventoryItemFileService;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.NOT_FOUND;

@Service
public class SaleOrderAttachmentService {
    private static final String SCREEN = "SaleOrder";
    private static final int DOCUMENT_TYPE_ID = 81;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopAttachmentStore store;

    public SaleOrderAttachmentService(JdbcTemplate jdbc, CurrentUserContext context, DesktopAttachmentStore store) {
        this.jdbc = jdbc; this.context = context; this.store = store;
    }

    public static class Request {
        public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
        public List<Integer> removeAttachmentIds = new ArrayList<>();
    }
    public record Download(String name, byte[] bytes) {}

    private UserAccount record(int id) {
        UserAccount u = context.requireAccountingUser();
        Integer count = jdbc.queryForObject(
                "SELECT COUNT(*) FROM dbo.SaleOrder WHERE Id=? AND OrganizationId=? AND CompanyId=? AND ISNULL(ActionId,1)<>3",
                Integer.class, id, u.getOrganizationId(), u.getCompanyId());
        if (count == null || count == 0) throw new ResponseStatusException(NOT_FOUND, "Sale Order not found");
        return u;
    }

    public List<Map<String, Object>> list(int id) {
        UserAccount u = record(id);
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity=?",
                        SCREEN, id, "ReadById").stream()
                .filter(r -> number(r.get("OrganizationId")) == u.getOrganizationId()
                        && number(r.get("CompanyId")) == u.getCompanyId()).toList();
    }

    @Transactional
    public List<Map<String, Object>> save(int id, Request request) {
        UserAccount u = record(id);
        if (request == null || request.files == null || request.removeAttachmentIds == null
                || request.files.size() > 10 || request.removeAttachmentIds.size() > 100)
            throw new IllegalArgumentException("Select at most ten files at once");
        List<Map<String, Object>> existing = list(id);
        for (Integer attachmentId : new HashSet<>(request.removeAttachmentIds)) {
            if (attachmentId == null || existing.stream().noneMatch(r -> number(r.get("Id")) == attachmentId))
                throw new IllegalArgumentException("Attachment does not belong to this Sale Order");
            jdbc.update("EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?,@Activity=?", attachmentId, "AttachmentDeleteById");
        }
        Map<String, Object> order = jdbc.queryForMap(
                "SELECT OrderSupCustId FROM dbo.SaleOrder WHERE Id=? AND OrganizationId=? AND CompanyId=?",
                id, u.getOrganizationId(), u.getCompanyId());
        for (InventoryPosItemRequest.Upload upload : request.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(upload);
            String stored = store.store(u, upload.name, bytes);
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Object[] values = {0, order.get("OrderSupCustId"), 0, DOCUMENT_TYPE_ID, id, upload.name,
                    now, u.getId(), now, u.getId(), u.getOrganizationId(), u.getCompanyId(),
                    u.getBranchesId(), SCREEN, false, stored, bytes.length / 1048576d, 0};
            jdbc.execute("EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?," +
                    "@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?," +
                    "@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@DetailWiseAttachment=?," +
                    "@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?",
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
        return list(id);
    }

    public Download download(int id, int attachmentId) {
        UserAccount u = record(id);
        Map<String, Object> row = list(id).stream().filter(r -> number(r.get("Id")) == attachmentId)
                .findFirst().orElseThrow(() -> new ResponseStatusException(NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(row.get("UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(row.get("Attachment"), "");
        String name = baseName(Objects.toString(row.get("Attachment"), stored));
        return new Download(name, store.read(u, baseName(stored)));
    }

    private static int number(Object value) { return value instanceof Number ? ((Number) value).intValue() : 0; }
    private static String baseName(String value) {
        String name = value.replace('\\', '/'); name = name.substring(name.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name); return name;
    }
}
