package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.repositories.lab.QcDeductionPolicyRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.StoreScreenRights;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Set;

import static com.mst.repositories.lab.QcDeductionPolicyRepository.ci;

/**
 * Lab (ModuleId 7) — screen 164 "Lab Deduction Policy For Purchase".
 *
 * Desktop form   Architecture.WinApp.QCL/frmQcDeductionPolicy.cs   (line refs ":n" below)
 * ScreenName     frmQcDeductionPolicy   (base.Name — rights key :232, DMS ScreenName :877)
 * BLL            architecture.bll/0294_Architecture.BLL.QCL.QcDeductionPolicyHeader.cs
 * DAL            architecture.dal/0252_Architecture.DAL.QCL.QcDeductionPolicyHeader.cs
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE (btnSave_Click :896) / UPDATE (btnUpdate_Click :772) -> Insert() (:788) — one transaction
 * ---------------------------------------------------------------------------------------------
 *   1. [qcl].[USP_QcDeductionPolicyHeader_InsertAndUpdate]   ActionId 1 (new) / 2 (update)
 *   2. [qcl].[USP_QcDeductionPolicyDetail_Insert] per grid row (ActionTypeId 1 new row, 2 stored row),
 *      then per removed stored row (ActionTypeId 3) — Update only (:868-874)
 *   3. attachments: Sp_DMSAttachments_GetAllMethod 'DeleteById' + Proc_DMSAttachments_Insert per file
 * DELETE (btnDelete_Click :948): [qcl].[USP_QcDeductionPolicy_GetAllMethod] 'DeleteById'.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP / DATABASE QUIRKS THAT ARE REPRODUCED BECAUSE THE SAME OBJECTS ARE CALLED
 * ---------------------------------------------------------------------------------------------
 *  Q1 'ReadByHeaderId' (proc :563928) filters d.QcDeductionPolicyDetailId = @Id although the DAL passes
 *     the HEADER id, so a loaded policy shows only the detail whose own id equals the header id (usually
 *     none, or a detail of another policy). See {@link #detailRows}.
 *  Q2 The header procedure refuses an approved record only when @IsApproved = 1 is SENT; the form never
 *     sets IsApproved, so an approved policy can be updated. Delete is refused by the procedure.
 *  Q3 On update the procedure writes PendingForView = @PendingForView, which the model does not carry:
 *     it becomes NULL.
 *  Q4 The 'FormHistory' attachment count is joined on InvLabGroupAnalysisStandardsId instead of the
 *     header id, so NoOfAttachments is the procedure's value, not a recount.
 *
 * Tenancy (organization, company, user) comes from the session only.
 */
@Service
public class QcDeductionPolicyService {

    private static final Logger LOG = LoggerFactory.getLogger(QcDeductionPolicyService.class);

    public static final String SCREEN_NAME = "frmQcDeductionPolicy";
    public static final int SCREEN_ID = 164;
    /** clsGlobalVariables.DecimalRateFormate — the "Rate" columns of the grids (ConfigureNumericalColumn :522). */
    private static final String RATE_DECIMALS_CONFIG = "Default NoofDecimal Points For Rate";

    private final QcDeductionPolicyRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public QcDeductionPolicyService(QcDeductionPolicyRepository repo, StoreScreenRights rights,
                                    CurrentUserContext ctx, DesktopAttachmentStore store) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.store = store;
    }

    /** One grid row (table of :238-246). */
    public static class RowRequest {
        public int id;                       // "Id" — QcDeductionPolicyDetailId, 0 for a row added on the page
        public int analysisParameterId;      // "AnalysisParameterId"
        public double rangeFrom;             // "RangeFrom"
        public double rangeTo;               // "RangeTo"
        public double deductionWeight;       // "DeductionWeight"
        public int uom;                      // "Uom"
        public double deductionQty;          // "DeductionQty"
        public double deductionRate;         // "DeductionRate"
    }

    /** What the page posts on Save / Update. */
    public static class SaveRequest {
        public int id;                       // RecId — 0 = Save, otherwise Update
        public String policyName;            // txtPolicyName.Text
        public int analysisGroupId;          // CmbAnalysisGroup.Value
        public String effectiveFrom;         // txtAffectiveDate.Value, yyyy-MM-dd
        public String effectiveTo;           // TxtAffectiveDateTo.Value, yyyy-MM-dd
        public String remarks;               // txtremarks.Text
        public List<RowRequest> rows = new ArrayList<>();          // grd.GetRows()
        public List<Integer> removedDetailIds = new ArrayList<>(); // lstRemoveRecord (:561-573)
        public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();   // AT.lst rows added
        public List<Integer> keepAttachmentIds = new ArrayList<>();               // AT.lst rows of the record still listed
    }

    // ======================================================================== form load

    /** frmQcDeductionPolicy_Load (:224) — rights, AnalysisGroup(), UOM(), the History dates. */
    public Map<String, Object> lookups() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));                                 // :232-235
        out.put("screenId", SCREEN_ID);
        out.put("screenName", SCREEN_NAME);
        out.put("analysisGroups", analysisGroups(u));                              // :236
        out.put("uoms", uoms());                                                   // :237
        out.put("today", LocalDate.now().toString());                              // ToDateHistory :252
        /* DefaultDaysToLessFromHistoryFromDate is never assigned in the form, so it is always -3 days (:251). */
        out.put("historyFrom", LocalDate.now().minusDays(3).toString());
        int rate = 0;
        try {
            rate = toInt(Objects.toString(store.configuration(u, RATE_DECIMALS_CONFIG), "").trim());
        } catch (RuntimeException e) {
            LOG.warn("Could not read '{}'", RATE_DECIMALS_CONFIG, e);
        }
        out.put("rateDecimals", rate >= 1 && rate <= 4 ? rate : 2);
        return out;
    }

    /** btnRefresh_Click (:988) — AnalysisGroup() + UOM(). */
    public Map<String, Object> refresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("analysisGroups", analysisGroups(u));
        out.put("uoms", uoms());
        return out;
    }

    /** AnalysisGroup() (:260): dt(Id, AnalysisGroupDescription, GroupTypeId, GroupType) built row by row from the procedure's table. */
    private List<Map<String, Object>> analysisGroups(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.analysisGroups(u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("AnalysisGroupDescription", ci(r, "AnalysisGroupDescription"));
            m.put("GroupTypeId", ci(r, "GroupType"));
            m.put("GroupType", ci(r, "InvParentCateDescription"));
            out.add(m);
        }
        return out;
    }

    /** UOM() (:367): value "Id", display "type". */
    private List<Map<String, Object>> uoms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uoms()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("type", str(ci(r, "type")));
            out.add(m);
        }
        return out;
    }

    /**
     * AnalysisParamerterByGroup (:314): dt(AnalysisParameterId, AnalysisParameter) — one row per row the
     * procedure returns (InvLabAnalysisItemsId, AnalysisParameterDescription), duplicates included.
     */
    public List<Map<String, Object>> parameters(int groupId) {
        UserAccount u = requireView();
        return parameterRows(u, groupId);
    }

    private List<Map<String, Object>> parameterRows(UserAccount u, int groupId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (groupId <= 0) return out;                                              // CmbAnalysisGroup_Leave :405
        for (Map<String, Object> r : repo.parametersByGroup(u.getOrganizationId(), u.getCompanyId(), groupId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AnalysisParameterId", ci(r, "InvLabAnalysisItemsId"));
            m.put("AnalysisParameter", ci(r, "AnalysisParameterDescription"));
            out.add(m);
        }
        return out;
    }

    // ======================================================================== history

    /**
     * gridhistoryfill (:1159). Without "CanView AllRecord" only the user's own records are read and NO
     * date is applied (the dates are in the else-branch, :1169 onward); otherwise the checked dates go to
     * the Entry / Modify / Approved pair chosen by the radio buttons. Mapped to the table of :1210-1224.
     */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate) {
        UserAccount u = requireView();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Date from = null, to = null;
        if (viewAll) {
            from = isBlank(fromDate) ? null : Date.valueOf(day(fromDate));         // FromDateHistory.Checked
            to = isBlank(toDate) ? null : Date.valueOf(day(toDate));               // ToDateHistory.Checked
        }
        String type = dateType == null ? "entry" : dateType.trim().toLowerCase();
        boolean modify = "modify".equals(type), approved = "approved".equals(type), entry = !modify && !approved;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u.getOrganizationId(), u.getCompanyId(), viewAll, u.getId(),
                entry ? from : null, entry ? to : null, modify ? from : null, modify ? to : null,
                approved ? from : null, approved ? to : null)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "QcDeductionPolicyHeaderId"));
            m.put("InvLabGroupAnalysisStandardsId", ci(r, "InvLabGroupAnalysisStandardsId"));
            m.put("AnalysisGroup", ci(r, "InvLabAnalysisGroup"));
            m.put("PolicyName", ci(r, "PolicyName"));
            m.put("EffectiveFrom", iso(ci(r, "EffectiveFrom")));
            m.put("EffectiveTo", iso(ci(r, "EfffectiveTo")));
            m.put("RemarksHeader", ci(r, "RemarksHeader"));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("EntryUser", ci(r, "EntryUser"));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));                         // "" when null (:1228)
            m.put("ModifyUser", ci(r, "ModifyUser"));
            m.put("IsApproved", toBool(ci(r, "IsApproved")) ? "Approved" : "Not Approved");
            m.put("ApprovedDate", iso(ci(r, "ApprovedDate")));
            m.put("ApprovedUser", ci(r, "ApprovedUser"));
            m.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GridDetailBind (:1372) — the detail grid under the selected History row. */
    public List<Map<String, Object>> details(int id) {
        UserAccount u = requireView();
        if (own(u, id) == null) return new ArrayList<>();
        return detailRows(id);
    }

    // ======================================================================== read

    /** ReadById (:910). */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> r = own(u, id);
        /* GetByID returns list[0]; an empty list is an exception shown in a MessageBox. */
        if (r == null) throw new IllegalArgumentException("Record Not Found");
        int groupId = toInt(ci(r, "InvLabGroupAnalysisStandardsId"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(r, "QcDeductionPolicyHeaderId")));
        out.put("PolicyName", str(ci(r, "PolicyName")));
        out.put("InvLabGroupAnalysisStandardsId", groupId);
        out.put("RemarksHeader", str(ci(r, "RemarksHeader")));
        out.put("EffectiveFrom", iso(ci(r, "EffectiveFrom")));
        out.put("EfffectiveTo", iso(ci(r, "EfffectiveTo")));
        out.put("parameters", parameterRows(u, groupId));                          // CmbAnalysisGroup_Leave :930
        out.put("rows", detailRows(id));                                           // :931-935
        out.put("attachments", attachmentRows(u, id));                             // LoadAttachmentsForObject :939
        return out;
    }

    /**
     * QcDeductionPolicyDetailList of DAL GetData (:120-125), mapped to the grid table (:238-246 / :934).
     * Quirk Q1: the procedure returns the detail whose own id equals the header id.
     */
    private List<Map<String, Object>> detailRows(int headerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.readDetails(headerId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(d, "QcDeductionPolicyDetailId")));
            m.put("AnalysisParameterId", toInt(ci(d, "AnalysisParameterId")));
            m.put("AnalysisParameter", str(ci(d, "AnalysisParameter")));
            m.put("RangeFrom", toDouble(ci(d, "RangeFrom")));
            m.put("RangeTo", toDouble(ci(d, "RangeTo")));
            m.put("DeductionWeight", toDouble(ci(d, "dedWeight")));
            m.put("Uom", toInt(ci(d, "dedWtUom")));
            m.put("DeductionQty", toDouble(ci(d, "dedOnQtyKg")));
            m.put("DeductionRate", toDouble(ci(d, "dedOnRateRs")));
            out.add(m);
        }
        return out;
    }

    // ======================================================================== save / update

    /** btnSave_Click (:896) when id == 0, btnUpdate_Click (:772) otherwise -> Insert() (:788). */
    @Transactional
    public Map<String, Object> save(SaveRequest dto) {
        UserAccount u = ctx.requireAccountingUser();
        if (dto == null) throw new IllegalArgumentException("Grid Record Not Found");
        boolean update = dto.id != 0;
        /* btnSave.Enabled = DoHaveSaveRight, btnUpdate.Enabled = DoHaveUpdateRights (:233-234). */
        if (!rights.has(SCREEN_NAME, "view"))
            throw new AccessDeniedException("You do not have the View right for Lab Deduction Policy For Purchase.");
        if (!update && !rights.has(SCREEN_NAME, "save"))
            throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (update && !rights.has(SCREEN_NAME, "update"))
            throw new AccessDeniedException("You do not have the Update right for this screen.");
        int org = u.getOrganizationId(), comp = u.getCompanyId(), userId = u.getId();

        /* Update works on a stored record of this company only. */
        if (update && own(u, dto.id) == null)
            throw new IllegalArgumentException("Record cannot be updated because RecId not found");   // :778

        List<RowRequest> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");           // :798
        // ---- FormValidation (:702)
        String policyName = dto.policyName == null ? "" : dto.policyName;
        if (policyName.isEmpty() || policyName.trim().equals("0"))
            throw new IllegalArgumentException("PolicyName field is required");
        if (dto.analysisGroupId == 0) throw new IllegalArgumentException("AnalysisGroup field is required");

        /* The stored details the page may address: exactly what ReadById gave it (quirk Q1). */
        Set<Integer> storedDetailIds = new LinkedHashSet<>();
        if (update) {
            for (Map<String, Object> d : repo.readDetails(dto.id)) storedDetailIds.add(toInt(ci(d, "QcDeductionPolicyDetailId")));
        }

        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));                         // :824-829, BLL :19-20
        QcDeductionPolicyRepository.Header h = new QcDeductionPolicyRepository.Header();
        h.qcDeductionPolicyHeaderId = update ? dto.id : 0;                                          // :811
        h.actionId = update ? 2 : 1;                                                                // BLL :21-28
        h.policyName = policyName;                                                                  // :817
        h.invLabGroupAnalysisStandardsId = dto.analysisGroupId;                                     // :818
        h.effectiveFrom = Timestamp.valueOf(day(dto.effectiveFrom).atStartOfDay());                 // :819
        h.efffectiveTo = Timestamp.valueOf(day(dto.effectiveTo).atStartOfDay());                    // :820
        h.remarksHeader = dto.remarks == null ? "" : dto.remarks;                                   // :821
        h.companyId = comp;
        h.organizationId = org;
        h.entryDate = now;
        h.entryUserId = userId;
        h.modifyUserId = userId;
        h.approvedUserId = userId;
        h.modifyDate = now;
        h.approvedDate = now;
        h.isApproved = false;                                                                        // never set (quirk Q2)

        // ---- the grid rows (:831-863)
        List<QcDeductionPolicyRepository.Detail> details = new ArrayList<>();
        Set<Integer> used = new LinkedHashSet<>();
        for (int i = 0; i < rows.size(); i++) {
            RowRequest r = rows.get(i);
            QcDeductionPolicyRepository.Detail d = new QcDeductionPolicyRepository.Detail();
            if (!update) {
                d.actionTypeId = 1;                                                                  // :844
            } else {
                d.qcDeductionPolicyDetailId = r == null ? 0 : r.id;                                  // :848
                if (d.qcDeductionPolicyDetailId > 0) {
                    if (!storedDetailIds.contains(d.qcDeductionPolicyDetailId) || !used.add(d.qcDeductionPolicyDetailId))
                        throw new IllegalArgumentException("The detail row " + (i + 1) + " does not belong to this record. Reload the record.");
                    d.actionTypeId = 2;                                                              // :844
                } else {
                    d.qcDeductionPolicyDetailId = 0;
                    d.actionTypeId = 1;                                                              // :848
                }
            }
            if (r == null || r.analysisParameterId == 0)
                throw new IllegalArgumentException("AnalysisParameter Field Required in Grid Row no : " + (i + 1));   // :853
            d.analysisParameterId = r.analysisParameterId;
            d.rangeFrom = dec(r.rangeFrom);
            d.rangeTo = dec(r.rangeTo);
            d.dedWeight = dec(r.deductionWeight);
            d.dedWtUom = r.uom;
            d.dedOnQtyKg = finite(r.deductionQty);
            d.dedOnRateRs = dec(r.deductionRate);
            details.add(d);
        }
        if (details.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows");   // :866
        /* lstRemoveRecord (:868-874): the stored rows deleted on the page, ActionTypeId 3 — Update only. */
        if (update && dto.removedDetailIds != null) {
            for (Integer rid : dto.removedDetailIds) {
                if (rid == null || rid <= 0) continue;
                if (!storedDetailIds.contains(rid) || !used.add(rid))
                    throw new IllegalArgumentException("A removed detail row does not belong to this record. Reload the record.");
                QcDeductionPolicyRepository.Detail d = new QcDeductionPolicyRepository.Detail();
                d.qcDeductionPolicyDetailId = rid;
                d.actionTypeId = 3;
                details.add(d);
            }
        }

        if (dto.files != null && dto.files.size() > 10)
            throw new IllegalArgumentException("At most ten attachments may be uploaded at once");

        /* AT.lst of an Update = the stored attachments still listed + the new files. */
        List<Map<String, Object>> stored = update ? attachmentRowsRaw(u, dto.id) : new ArrayList<>();
        List<Map<String, Object>> kept = new ArrayList<>();
        Set<Integer> keep = new LinkedHashSet<>(dto.keepAttachmentIds == null ? List.<Integer>of() : dto.keepAttachmentIds);
        for (Map<String, Object> a : stored) if (keep.contains(toInt(ci(a, "Id")))) kept.add(a);
        int newFiles = dto.files == null ? 0 : dto.files.size();

        // ---- DAL SetData (0252 :16)
        int id = repo.saveHeader(h);                                                                 // DAL :44
        if (id <= 0) throw new IllegalStateException("The record could not be saved.");
        for (QcDeductionPolicyRepository.Detail d : details) {                                       // DAL :53-57
            d.qcDeductionPolicyHeaderId = id;
            repo.saveDetail(d);
        }
        if (kept.size() + newFiles > 0) {                                                            // DAL :58
            repo.removeAttachments(id, SCREEN_NAME);                                                 // DAL :60-76
            for (Map<String, Object> a : kept) {
                repo.insertAttachment(id, str(ci(a, "Attachment")), now, userId, now, userId, org, comp, SCREEN_NAME,
                        emptyToNull(str(ci(a, "UploadedFileCustomName"))), toDouble(ci(a, "UploadedFileSizeMb")));
            }
            if (dto.files != null) {
                for (InventoryPosItemRequest.Upload f : dto.files) {
                    byte[] bytes = DesktopInventoryItemFileService.decode(f);
                    String storedName = store.store(u, f.name, bytes);
                    repo.insertAttachment(id, f.name, now, userId, now, userId, org, comp, SCREEN_NAME,
                            storedName, bytes.length / 1048576d);
                }
            }
        } else if (update && !stored.isEmpty()) {
            /* FormHelper.DeleteAllAttachmentsFromDb (:882 / FormHelper :629): every attachment was removed
               in the dialog and none is left -> the record's attachment rows are removed. */
            repo.removeAttachments(id, SCREEN_NAME);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + policyName);   // :881 / :886
        return out;
    }

    // ======================================================================== delete

    /** btnDelete_Click (:948) — the procedure refuses an approved record with its own message. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view"))
            throw new AccessDeniedException("You do not have the View right for Lab Deduction Policy For Purchase.");
        if (!rights.has(SCREEN_NAME, "delete"))                                                      // btnDelete.Enabled :235
            throw new AccessDeniedException("You do not have the Delete right for this screen.");
        if (id <= 0 || own(u, id) == null) throw new IllegalArgumentException("Record Not Found");   // :967
        repo.deleteById(id, u.getId());                                                              // :960
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Delete Record Successfully");                                            // :961
        return out;
    }

    // ======================================================================== attachments

    /** grdhistory_LinkClicked (:1319) / ReadById (:939) — the record's attachments. */
    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = requireView();
        if (own(u, id) == null) return new ArrayList<>();
        return attachmentRows(u, id);
    }

    private List<Map<String, Object>> attachmentRowsRaw(UserAccount u, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachments(id, SCREEN_NAME)) {
            if (toInt(ci(r, "OrganizationId")) == u.getOrganizationId() && toInt(ci(r, "CompanyId")) == u.getCompanyId()) out.add(r);
        }
        return out;
    }

    private List<Map<String, Object>> attachmentRows(UserAccount u, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : attachmentRowsRaw(u, id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("Attachment", safeBaseName(str(ci(r, "Attachment"))));
            m.put("EntryUserName", ci(r, "EntryUserName"));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("UploadedFileSizeMb", ci(r, "UploadedFileSizeMb"));
            out.add(m);
        }
        return out;
    }

    public DesktopInventoryItemFileService.Download attachment(int id, int attachmentId) {
        UserAccount u = requireView();
        if (own(u, id) == null) return null;
        for (Map<String, Object> r : attachmentRowsRaw(u, id)) {
            if (toInt(ci(r, "Id")) != attachmentId) continue;
            String storedName = str(ci(r, "UploadedFileCustomName"));
            if (storedName.isBlank()) storedName = str(ci(r, "Attachment"));
            String shown = str(ci(r, "Attachment"));
            return new DesktopInventoryItemFileService.Download(baseName(shown.isBlank() ? storedName : shown),
                    store.read(u, baseName(storedName)), "application/octet-stream");
        }
        return null;
    }

    // ======================================================================== helpers

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view"))
            throw new AccessDeniedException("You do not have the View right for Lab Deduction Policy For Purchase.");
        return u;
    }

    /** The header ('ReadById'), only when it belongs to the session's organization and company. */
    private Map<String, Object> own(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (toInt(ci(r, "OrganizationId")) != u.getOrganizationId() || toInt(ci(r, "CompanyId")) != u.getCompanyId()) return null;
        return r;
    }

    /** Conversion.ToDecimal(double cell value). */
    private static BigDecimal dec(double v) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return BigDecimal.ZERO;
        return BigDecimal.valueOf(v);
    }

    private static double finite(double v) { return Double.isNaN(v) || Double.isInfinite(v) ? 0d : v; }

    private static LocalDate day(String value) {
        if (isBlank(value)) return LocalDate.now();
        try {
            return LocalDate.parse(value.trim().substring(0, Math.min(10, value.trim().length())));
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid date");
        }
    }

    private static String iso(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.sql.Date) return v.toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v);
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static double toDouble(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0d;
        try { return Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && "true".equalsIgnoreCase(String.valueOf(v).trim());
    }

    private static String str(Object v) { return Objects.toString(v, ""); }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static String emptyToNull(String s) { return s == null || s.isEmpty() ? null : s; }

    /** Path.GetFileName of a Windows or Unix path, validated as a plain file name. */
    private static String baseName(String path) {
        String p = path.replace('\\', '/');
        String name = p.substring(p.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name);
        return name;
    }

    private static String safeBaseName(String path) {
        String p = path.replace('\\', '/');
        return p.substring(p.lastIndexOf('/') + 1);
    }
}
