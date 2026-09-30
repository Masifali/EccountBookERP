package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.repositories.imports.ImpBRepository;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.Base64;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Shared BLL plumbing of the four Import screens ported in this package (ImpB*).
 *
 *  - user(screenId) / require(...)      HrmSupport: View on the ScreenDefinition row + Save / Update / Print where the form checks.
 *  - formRights(screenId, screenName)   CommonServices.SetRightsValueInRightsObject(base.Name): the tblUserRights flags the
 *                                       form reads (Save / Update / Print / CanView AllRecord), Admin short-circuit, through
 *                                       DesktopVoucherSupport.rights (Sp_tblUserRights_GetAllMethod 'GetByUserId').
 *  - feature(id) / config(description)  CommonServices.GetERPFeatureById / GetConfigurationByOrgCompandConfigDescription.
 *  - attachments                        the Attachment dialog + DAL SetData's attachment block (DMSAttachments).
 *
 * Tenancy (Organization, Company, Branch, user id, financial year) is always the signed-in user's.
 */
@Component
public class ImpBSupport {

    @Autowired private HrmSupport hrm;
    @Autowired private DesktopVoucherSupport desk;
    @Autowired private DesktopAttachmentStore store;
    @Autowired private ImpBRepository repo;

    public UserAccount user(int screenId) { return hrm.user(screenId); }

    public void require(UserAccount u, int screenId, String action) { hrm.require(u, screenId, action); }

    public boolean can(UserAccount u, int screenId, String action) { return hrm.can(u, screenId, action); }

    public int financialYearId() { return hrm.financialYearId(); }

    public static int branchId(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    /**
     * The form's Rightsobjects: save / update / print from the ScreenDefinition rights (HrmSupport, the same flags the
     * API enforces) and canViewAllRecord from tblUserRights "CanView AllRecord" of the form name (the desktop's
     * DoHaveCanViewAllRecordRights).
     */
    public Map<String, Object> formRights(UserAccount u, int screenId, String screenName) {
        Map<String, Object> r = hrm.rights(u, screenId);
        r.put("canViewAllRecord", canViewAllRecord(screenName));
        return r;
    }

    public boolean canViewAllRecord(String screenName) {
        return Boolean.TRUE.equals(desk.rights(screenName).get("canViewAllRecord"));
    }

    public boolean feature(int id) { return desk.feature(id); }

    public String config(String description) { String v = desk.config(description); return v == null ? "" : v; }

    /** DefaultDaysToLessFromHistoryFromDate (Conversion.ToInt of the ConfigKey). */
    public int historyDays() { return toInt(config("DefaultDaysToLessFromHistoryFromDate")); }

    /** clsGlobalVariables.globalBranchesAllocateToUser (USP_GetBranchsAllocatedToUser). */
    public List<Map<String, Object>> branchesOfUser() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : desk.branchesAllocatedToUser()) out.add(map("BranchId", r.get("BranchId"), "BranchName", str(r.get("BranchName"))));
        return out;
    }

    /** CommonServices.JobLotGetAllService -> jobLot.GetAll: SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLots() { return desk.jobLots(); }

    /** MultiCurrency.GetAll: Sp_MultiCurrency_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> currencies() { return desk.currencies(); }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(types): Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'. */
    public List<Map<String, Object>> accountsByTypes(String typeIds) { return desk.coaAccountTitleByAccountTypeIds(typeIds, 0); }

    // ------------------------------------------------------------------ small helpers

    /** Rows of a "(Activity, Id, name)" drop-down procedure whose Activity equals one value, as {Id, name}. */
    public static List<Map<String, Object>> activity(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) if (activity.equals(str(r.get("Activity")))) out.add(map("Id", r.get("Id"), "name", str(r.get("name"))));
        return out;
    }

    public static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, r.get(c));
            out.add(m);
        }
        return out;
    }

    public static boolean has(List<Map<String, Object>> rows, String key, long id) {
        if (id == 0 || rows == null) return false;
        for (Map<String, Object> r : rows) if (toLong(r.get(key)) == id) return true;
        return false;
    }

    public static long toLong(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).longValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Long.parseLong(s); } catch (NumberFormatException e) {
            try { return new java.math.BigDecimal(s).longValue(); } catch (NumberFormatException e2) { return 0; }
        }
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> obj(Object v) { return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>(); }

    // ------------------------------------------------------------------ attachments

    /** The attachments of a record (DMSAttachments.GetByID(RecId, base.Name)), this company's rows only. */
    public List<Map<String, Object>> attachments(UserAccount u, String screenName, long id) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (id <= 0) return out;
        for (Map<String, Object> r : repo.attachments(screenName, id)) {
            if (toInt(r.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(r.get("CompanyId")) != toInt(u.getCompanyId())) continue;
            out.add(map("Id", r.get("Id"), "Attachment", str(r.get("Attachment")), "UploadedFileCustomName", str(r.get("UploadedFileCustomName")),
                    "UploadedFileSizeMb", r.get("UploadedFileSizeMb"), "EntryUserName", str(r.get("EntryUserName")), "EntryDate", r.get("EntryDate")));
        }
        return out;
    }

    /** Download of one attachment of a record: the stored file named by Attachment (the desktop's physical file name). */
    public DesktopAttachmentStoreFile file(UserAccount u, String screenName, long id, int attachmentId) {
        for (Map<String, Object> r : attachments(u, screenName, id)) {
            if (toInt(r.get("Id")) != attachmentId) continue;
            String stored = baseName(str(r.get("Attachment")));
            String shown = str(r.get("UploadedFileCustomName"));
            if (shown.isEmpty()) shown = stored;
            return new DesktopAttachmentStoreFile(baseName(shown), store.read(u, stored));
        }
        throw invalid("Attachment not found");
    }

    public static final class DesktopAttachmentStoreFile {
        public final String name; public final byte[] bytes;
        DesktopAttachmentStoreFile(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    private static String baseName(String v) {
        String s = v.replace('\\', '/');
        return s.substring(s.lastIndexOf('/') + 1);
    }

    /**
     * What the Attachment dialog hands the save: the kept existing rows (by Id) plus new files ({name, base64}).
     * prepare() must run inside the save transaction - new files are written through DesktopAttachmentStore and are
     * removed again when that transaction rolls back (DAL: DeleteAttachmentsPhysicallyInException).
     */
    public static final class AttachmentPlan {
        public final List<Map<String, Object>> finalList = new ArrayList<>();   // {Attachment, UploadedFileCustomName, UploadedFileSizeMb}
        public int removed;
        public boolean changed;
        public String values() { return join("Attachment"); }
        public String customValues() { return join("UploadedFileCustomName"); }
        private String join(String k) {
            StringBuilder sb = new StringBuilder();
            for (Map<String, Object> m : finalList) { if (sb.length() > 0) sb.append(','); sb.append(str(m.get(k))); }
            return sb.toString();
        }
    }

    public AttachmentPlan prepare(UserAccount u, String screenName, long id, Map<String, Object> body) {
        AttachmentPlan plan = new AttachmentPlan();
        Map<String, Object> a = obj(body.get("attachments"));
        List<Map<String, Object>> existing = attachments(u, screenName, id);
        Set<Integer> keep = new LinkedHashSet<>();
        Object k = a.get("keep");
        boolean keepGiven = k instanceof List;
        if (keepGiven) for (Object o : (List<?>) k) keep.add(toInt(o));
        for (Map<String, Object> r : existing) {
            if (!keepGiven || keep.contains(toInt(r.get("Id")))) {
                plan.finalList.add(map("Attachment", r.get("Attachment"), "UploadedFileCustomName", r.get("UploadedFileCustomName"),
                        "UploadedFileSizeMb", toDouble(r.get("UploadedFileSizeMb"))));
            } else plan.removed++;
        }
        List<Map<String, Object>> files = list(a.get("files"));
        if (files.size() > 10) throw invalid("At most ten attachments may be uploaded at once");
        for (Map<String, Object> f : files) {
            String name = str(f.get("name")).trim();
            DesktopAttachmentStore.validateName(name);
            byte[] bytes;
            try { bytes = Base64.getDecoder().decode(str(f.get("base64"))); } catch (IllegalArgumentException e) { throw invalid("Invalid attachment content"); }
            String stored = store.store(u, name, bytes);
            plan.finalList.add(map("Attachment", stored, "UploadedFileCustomName", name, "UploadedFileSizeMb", bytes.length / 1048576d));
        }
        plan.changed = plan.removed > 0 || !files.isEmpty();
        return plan;
    }

    /** DAL SetData attachment block: DeleteById then Proc_DMSAttachments_Insert per row (only when the list is not empty). */
    public void apply(AttachmentPlan plan, UserAccount u, String screenName, long id, int refAccountId, int refDocTypeId) {
        if (!plan.changed || plan.finalList.isEmpty()) return;
        repo.deleteAttachments(screenName, id);
        for (Map<String, Object> m : plan.finalList) {
            repo.insertAttachment(u, screenName, id, refAccountId, refDocTypeId, str(m.get("Attachment")),
                    str(m.get("UploadedFileCustomName")), toDouble(m.get("UploadedFileSizeMb")), u.getId(), u.getId());
        }
    }

    /** Form, after an update: removed attachments and none left -> DMSAttachments.RemoveByIdAndNames(RecId, name, docType, 0). */
    public void afterUpdate(AttachmentPlan plan, long id, String screenName, int refDocTypeId) {
        if (plan.removed > 0 && plan.finalList.isEmpty()) repo.removeAttachmentsByIdAndName(id, screenName, refDocTypeId);
    }

    public static boolean eq(Object a, Object b) { return Objects.equals(str(a), str(b)); }
}
