package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Attachments of the Sale Pcc documents. The pcc header procedures (USP_*_InsertAndUpdate) DELETE every DMSAttachments row of the document on an
 * update and the desktop's DAL then writes the whole AttachmentsList again (the files that are still listed plus the new ones). The web keeps the
 * list on the server, so an update is: remember(rows before the header proc) -> header proc -> restore(rows the user did not remove) -> new files.
 */
@Component
public class SalePccAttachments {
    private final SaleEngrAttachments base;
    private final SaleEngrSupport sup;
    private final JdbcTemplate jdbc;

    public SalePccAttachments(SaleEngrAttachments base, SaleEngrSupport sup, JdbcTemplate jdbc) { this.base = base; this.sup = sup; this.jdbc = jdbc; }

    public List<Map<String, Object>> list(String screen, int id) { return base.list(screen, id); }

    public SaleEngrAttachments.Download download(String screen, int id, int attachmentId) { return base.download(screen, id, attachmentId); }

    /** The rows that exist before an update (empty for a new document). */
    public List<Map<String, Object>> remember(String screen, int id) { return id > 0 ? new ArrayList<>(base.list(screen, id)) : new ArrayList<>(); }

    /**
     * After the header procedure ran: re-insert the remembered rows the user did not remove, then store the new files.
     * New document (before.isEmpty() and id was 0): only the new files.
     */
    public void write(String screen, int documentTypeId, int id, int refAccountId, List<Map<String, Object>> before, SaleEngrAttachments.Change change) {
        UserAccount u = sup.user();
        Set<Integer> remove = new HashSet<>(change == null || change.removeAttachmentIds == null ? List.<Integer>of() : change.removeAttachmentIds);
        for (Integer a : remove)
            if (before.stream().noneMatch(r -> toInt(r.get("Id")) == a)) throw new IllegalArgumentException("Attachment does not belong to this document");
        for (Map<String, Object> r : before) {
            if (remove.contains(toInt(r.get("Id")))) continue;
            Timestamp now = now();
            Object[] values = {0, refAccountId > 0 ? refAccountId : null, 0, documentTypeId, id, r.get("Attachment"), now, u.getId(), now, u.getId(),
                    u.getOrganizationId(), u.getCompanyId(), u.getBranchesId(), screen, false, r.get("UploadedFileCustomName"), r.get("UploadedFileSizeMb"), 0};
            exec(values);
        }
        if (change != null && change.files != null && !change.files.isEmpty()) {
            SaleEngrAttachments.Change onlyNew = new SaleEngrAttachments.Change();
            onlyNew.files = change.files;
            base.apply(screen, documentTypeId, id, refAccountId, onlyNew);
        }
    }

    private void exec(Object[] values) {
        jdbc.execute("EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,@RefDocumentTypeId=?,@RefDocumentNo=?,"
                        + "@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,"
                        + "@ScreenName=?,@DetailWiseAttachment=?,@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?",
                (PreparedStatementCallback<Void>) st -> {
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
