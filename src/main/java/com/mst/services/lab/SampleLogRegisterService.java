package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.repositories.lab.SampleLogRegisterRepository;
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
import java.util.Locale;
import java.util.Map;
import java.util.Objects;
import java.util.Set;

import static com.mst.repositories.lab.SampleLogRegisterRepository.ci;

/**
 * Lab (ModuleId 7) — screen 155 "Sample Log Register".
 *
 * Desktop form   Architecture.WinApp.Lab/InvLabSampleLogRegister.cs   (line refs ":n" below)
 * ScreenName     InvLabSampleLogRegister   (base.Name — rights key :728, DMS ScreenName :527)
 * DocumentTypeId 301                       (:501 / :571)
 * BLL            architecture.bll/0408_Architecture.BLL.Lab.InvLabSampleLogRegister.cs
 * DAL            architecture.dal/0363_Architecture.DAL.Lab.InvLabSampleLogRegister.cs
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE (btnSave_Click :489) / UPDATE (btnUpdate_Click :557) — one transaction here
 * ---------------------------------------------------------------------------------------------
 *   Save:   formvalidation -> SampleCode() again (:500, Sp_InvLabSampleLogRegister_GetAllMethod
 *           'GenerateDocNo') -> Sp_InvLabSampleLogRegister_Insert -> Proc_DMSAttachments_Insert per file.
 *   Update: formvalidation -> DMSAttachments.RemoveById (:594, Sp_DMSAttachments_GetAllMethod
 *           'DeleteById') -> Sp_InvLabSampleLogRegister_Update -> Proc_DMSAttachments_Insert per file.
 * The form has no Delete button and no delete procedure call, so there is no delete here.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP QUIRKS REPRODUCED (see the report for the decisions they need)
 * ---------------------------------------------------------------------------------------------
 *  Q1 Qty is saved through Conversion.ToInt(txtqty.Text) (:511) = Convert.ToInt32(string): digits only.
 *     "12.5" or "1,000" is saved as 0, although CalculateWeight (:1055) reads the same text through
 *     Conversion.ToDouble and does use 12.5 / 1000 for the weight.
 *  Q2 Weight is saved through Conversion.ToInt(txtweight.Text) (:513) and the box is filled with
 *     (qty * pack).ToString("0,0") (:1063) — a text WITH a thousands separator from 1,000 up, which
 *     Convert.ToInt32 refuses, so every weight of 1,000 or more is stored as 0. {@link #savedWeight}.
 *  Q3 The pack factor is Conversion.ToInt(CmbPackSize.Text) (:1060): an Equivalent that is not a whole
 *     number text ("37.5", "50.00") counts as 0, the weight stays "0" and the save is refused with
 *     "weight Should Be Greater Than 0".
 *  Q4 City is not validated (:403) but both read procedures INNER JOIN City, so a record saved without
 *     a city never shows in the Form grid and cannot be reopened.
 *
 * Tenancy (organization, company, user) comes from the session only.
 */
@Service
public class SampleLogRegisterService {

    private static final Logger LOG = LoggerFactory.getLogger(SampleLogRegisterService.class);

    public static final String SCREEN_NAME = "InvLabSampleLogRegister";     // seed_screendef.txt:143 (Id 155)
    public static final int SCREEN_ID = 155;
    public static final int DOCUMENT_TYPE_ID = 301;                         // :501
    private static final int NO_OF_RECORDS = 50;                            // grdHistoryForLoad :642
    private static final String ATTACHMENT_FOLDER_CONFIG = "Attachment Folder Path";   // definition() :826
    /** btnimg1_Click :855 — "(*.jpg; *.jpeg; *.PNG; *.png)". */
    private static final Set<String> PICTURE_EXTENSIONS = Set.of("jpg", "jpeg", "png");

    private final SampleLogRegisterRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public SampleLogRegisterService(SampleLogRegisterRepository repo, StoreScreenRights rights,
                                    CurrentUserContext ctx, DesktopAttachmentStore store) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.store = store;
    }

    /** What the page posts on Save / Update. Ids and the visible texts of the form's controls only. */
    public static class SaveRequest {
        public int id;                       // RecId — 0 = Save, otherwise Update
        public String sampleNo;              // txtsampleno.Text (display; regenerated / re-read here)
        public String sampleDate;            // txtsampled.Value, yyyy-MM-dd
        public int supplierCustomerId;       // cmbsup.Value
        public int referencePartyId;         // cmbreferenceparty.Value
        public int cityId;                   // cmbcity.Value
        public int itemId;                   // cmbitem.Value
        public String crop;                  // cmbCropYear.Text
        public int jobLotId;                 // cmbJobLot.Value
        public String lotDesc;               // cmbJobLot.Text
        public String qty;                   // txtqty.Text
        public int itemUomId;                // CmbPackSize.Value
        public String packSize;              // CmbPackSize.Text (the Equivalent)
        public String otherRemarks;          // txtremarks.Text
        public InventoryPosItemRequest.Upload picture;          // a picture chosen with Browse
        public boolean keepPicture;                             // Update: the loaded picture is still shown
        public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();   // attachments added
        public List<Integer> keepAttachmentIds = new ArrayList<>();               // Update: attachments left in the list
    }

    // ======================================================================== form load

    /** InvLabSampleLogRegister_Load (:724) — rights, number, the five fills and the grid. */
    public Map<String, Object> lookups() {
        UserAccount u = requireView();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);                                                    // :728-730
        out.put("screenId", SCREEN_ID);
        out.put("screenName", SCREEN_NAME);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("financialYearStart", financialYearStart(u));                     // FromDate :731
        out.put("today", LocalDate.now().toString());                             // ToDate :732
        /* Each fill is guarded on its own, exactly as on the desktop (SampleCode :208, ItemFill :232,
           CityFill :368, CropYearFill :315, JobLotFill :329, SupplierFillGetAll :275, grdHistoryForLoad :630
           each have their own try / catch): one failing procedure must not empty the City combo or any other. */
        List<String> errors = new ArrayList<>();
        int sampleNo = 0;
        try { sampleNo = repo.generateCode(org, comp); }                           // SampleCode :742
        catch (org.springframework.dao.DataAccessException e) { safe("SampleCode", errors, () -> { throw e; }); }
        out.put("sampleNo", sampleNo);
        out.put("items", pick(safe("ItemFill", errors, () -> repo.items(org, comp)),             // ItemFill :743
                "Id", "ItemName", "ItemCategory", "ItemCode", "InventoryParentCategoriesId"));
        out.put("cities", pick(safe("CityFill", errors, () -> repo.cities(org, comp)), "Id", "CityName"));        // CityFill :744
        out.put("cropYears", pick(safe("CropYearFill", errors, () -> repo.cropYears(org, comp)), "Id", "CropYear"));  // CropYearFill :745
        out.put("jobLots", pick(safe("JobLotFill", errors, () -> repo.jobLots(org, comp)), "Id", "JobLotDescription"));   // JobLotFill :746
        out.put("suppliers", pick(safe("SupplierFillGetAll", errors, () -> repo.suppliers(org, comp)), "Id", "CompanyName"));     // SupplierFillGetAll :747
        final boolean viewAll = Boolean.TRUE.equals(r.get("viewAll"));
        out.put("grid", safe("grdHistoryForLoad", errors, () -> gridRows(u, viewAll)));      // grdHistoryForLoad :748
        out.put("errors", errors);
        /* definition() (:822): the Browse button needs the "Attachment Folder Path" configuration. */
        out.put("attachmentFolderConfigured", !isBlank(store.configuration(u, ATTACHMENT_FOLDER_CONFIG)));
        return out;
    }


    /**
     * One list of the form load. On the desktop every fill (ItemFill, CityFill, GroupFill, BindPlantName ...)
     * has its own try / catch -> MessageBox.Show(ex.Message): a fill that fails leaves ITS combo empty and
     * the other fills still run. Reading all of them in one unguarded call made one failing procedure
     * empty every dropdown of the page. Here a failing list comes back empty and its message is added to
     * "errors", which the page shows one by one.
     */
    private static List<Map<String, Object>> safe(String fill, List<String> errors,
                                                  java.util.function.Supplier<List<Map<String, Object>>> read) {
        try {
            List<Map<String, Object>> rows = read.get();
            return rows == null ? new ArrayList<>() : rows;
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
            LOG.warn("{} failed: {}", fill, text);
            errors.add(fill + ": " + text);
            return new ArrayList<>();
        }
    }

    /** refresh() (:444) — SampleCode + grdHistoryForLoad. */
    public Map<String, Object> refresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("sampleNo", repo.generateCode(u.getOrganizationId(), u.getCompanyId()));
        out.put("grid", gridRows(u, rights.has(SCREEN_NAME, "viewAll")));
        return out;
    }

    /** bindPackSizeInput (:247) — the item's UOM schedule; value Id, display Equivalent. */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = requireView();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u.getOrganizationId(), u.getCompanyId(), itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("Equivalent", netString(ci(r, "Equivalent")));          // the combo's text column
            out.add(m);
        }
        return out;
    }

    /**
     * grdHistoryForLoad (:628): the last 50 records (own records only without "CanView AllRecord"),
     * mapped to the grid table of :648-667.
     */
    private List<Map<String, Object>> gridRows(UserAccount u, boolean viewAll) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.readAll(u.getOrganizationId(), u.getCompanyId(), NO_OF_RECORDS, viewAll, u.getId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("LogNo", ci(r, "SampleNo"));
            m.put("LogDate", iso(ci(r, "SampleDate")));
            m.put("Supplier", ci(r, "CompanyNameSp"));
            m.put("ReferenceParty", ci(r, "CompanyNameRp"));
            m.put("City", ci(r, "DescriptionCity"));
            m.put("ItemNames", ci(r, "ItemName"));
            m.put("Crop", ci(r, "Crop"));
            m.put("Lot", ci(r, "LotDesc"));
            m.put("Qty", toDouble(ci(r, "Qty")));
            m.put("PackSize", ci(r, "PackSize"));
            m.put("EntryUser", ci(r, "UserName"));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("Remarks", ci(r, "OtherRemarks"));
            m.put("Attachments", ci(r, "NoOfAttachments"));
            out.add(m);
        }
        return out;
    }

    /** btnshow_Click (:1120) — the History tab grid, mapped to the table of :1150-1169. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int supplierId, int referencePartyId,
                                             int itemId, int cityId, String cropYear) {
        UserAccount u = requireView();
        Date from = Date.valueOf(day(fromDate));
        Date to = Date.valueOf(day(toDate));
        String crop = cropYear == null ? "" : cropYear;                       // Conversion.ToString(CmbCropCriteria.Text)
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.register(u.getOrganizationId(), u.getCompanyId(), from, to,
                supplierId, referencePartyId, cityId, itemId, crop)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("LogNo", ci(r, "SampleNo"));
            m.put("LogDate", iso(ci(r, "SampleDate")));
            m.put("Supplier", ci(r, "SupplierName"));
            m.put("ReferenceParty", ci(r, "RefPartyName"));
            m.put("City", ci(r, "CityName"));
            m.put("ItemNames", ci(r, "ItemName"));
            m.put("Crop", ci(r, "Crop"));
            m.put("Lot", ci(r, "LotDescriptions"));
            m.put("Qty", toDouble(ci(r, "Qty")));
            m.put("PackSize", ci(r, "PackUom"));
            m.put("EntryUser", ci(r, "EntryUser"));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("Remarks", ci(r, "OtherRemarks"));
            out.add(m);
        }
        return out;
    }

    /**
     * ReadById (:752). An empty result (unknown id, another company's record, or a record whose city is
     * missing — quirk Q4) leaves the form untouched on the desktop (:759), so it is {found:false} here.
     */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> r = own(u, id);
        out.put("found", r != null);
        if (r == null) return out;
        int itemId = toInt(ci(r, "ItemId"));
        out.put("Id", toInt(ci(r, "Id")));
        out.put("SampleNo", netString(ci(r, "SampleNo")));
        out.put("SampleDate", iso(ci(r, "SampleDate")));
        out.put("SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")));
        out.put("ReferencePartyId", toInt(ci(r, "ReferencePartyId")));
        out.put("CityId", toInt(ci(r, "CityId")));
        out.put("ItemId", itemId);
        out.put("uoms", uoms(itemId));                                         // bindPackSizeInput :770
        out.put("ItemUomId", toInt(ci(r, "ItemUomId")));
        out.put("Crop", str(ci(r, "Crop")));
        out.put("JobLotId", netString(ci(r, "JobLotId")));                     // Conversion.ToString :773
        out.put("Qty", netString(ci(r, "Qty")));
        out.put("Weight", netString(ci(r, "Weight")));
        out.put("OtherRemarks", str(ci(r, "OtherRemarks")));
        /* :777-796 — the picture is shown only when the stored file can be read; otherwise the path is dropped. */
        out.put("hasPicture", readPicture(u, str(ci(r, "SamplePic"))) != null);
        out.put("attachments", attachmentRows(u, id));                          // DMSAttachments.GetByID :800
        return out;
    }

    // ======================================================================== save / update

    /** btnSave_Click (:489) when id == 0, btnUpdate_Click (:557) otherwise. */
    @Transactional
    public Map<String, Object> save(SaveRequest dto) {
        UserAccount u = ctx.requireAccountingUser();
        if (dto == null) throw new IllegalArgumentException("Please Insert sample no");
        boolean update = dto.id != 0;
        /* btnSave.Enabled = DoHaveSaveRight, btnUpdate.Enabled = DoHaveUpdateRights (:729-730). */
        if (!update && !rights.has(SCREEN_NAME, "save"))
            throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (update && !rights.has(SCREEN_NAME, "update"))
            throw new AccessDeniedException("You do not have the Update right for this screen.");
        int org = u.getOrganizationId(), comp = u.getCompanyId(), userId = u.getId();

        /* Update works on the stored record of this company only; its number is the stored one. */
        Map<String, Object> stored = null;
        if (update) {
            stored = own(u, dto.id);
            if (stored == null) throw new IllegalArgumentException("The record was not found.");
        }

        // ---- formvalidation (:403), desktop order and texts
        String weightText = calculateWeight(dto.qty, dto.packSize);              // CalculateWeight :1055
        if (isBlank(dto.sampleNo)) throw new IllegalArgumentException("Please Insert sample no");
        if (dto.supplierCustomerId == 0) throw new IllegalArgumentException("Please Select Supplier ");
        if (dto.itemId == 0) throw new IllegalArgumentException("Please Select Item");
        if (isBlank(dto.qty)) throw new IllegalArgumentException("Please Insert Quantity");
        if (dto.itemUomId == 0 || isBlank(dto.packSize)) throw new IllegalArgumentException("Please Select Pack Size");
        if (weightText.isEmpty() || weightText.equals("0")) throw new IllegalArgumentException("weight Should Be Greater Than 0");

        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));       // BLL Save :19-20
        SampleLogRegisterRepository.Header h = new SampleLogRegisterRepository.Header();
        h.id = update ? dto.id : 0;
        h.documentTypeId = DOCUMENT_TYPE_ID;                                       // :501 / :571
        /* Save re-reads the number just before saving (:500); Update keeps the record's number (:572). */
        h.sampleNo = update ? toInt(ci(stored, "SampleNo")) : repo.generateCode(org, comp);
        h.sampleDate = Date.valueOf(day(dto.sampleDate));                          // :503
        h.supplierCustomerId = dto.supplierCustomerId;                             // :504
        h.referencePartyId = dto.referencePartyId;                                 // :505
        h.cityId = dto.cityId;                                                     // :506
        h.itemId = dto.itemId;                                                     // :507
        h.crop = dto.crop == null ? "" : dto.crop;                                 // :508 Conversion.ToString
        h.lotDesc = dto.lotDesc == null ? "" : dto.lotDesc;                        // :509
        h.jobLotId = dto.jobLotId;                                                 // :510
        h.qty = netToInt(dto.qty);                                                 // :511 (quirk Q1)
        h.itemUomId = dto.itemUomId;                                               // :512
        h.weight = savedWeight(weightText);                                        // :513 (quirk Q2)
        h.otherRemarks = dto.otherRemarks == null ? "" : dto.otherRemarks;         // :514
        h.sampleCookingPic = null;                                                 // :516
        h.isAttachment = false;                                                    // :517
        h.organizationId = org;
        h.companyId = comp;
        h.entryUser = userId;
        h.modifyUser = userId;
        h.entryDate = now;
        h.modifyDate = now;

        /* SamplePic (:515 / :585) = fileSavePath: the newly browsed picture, else (Update) the loaded
           record's path while its picture is still shown, else "". */
        if (dto.picture != null && !isBlank(dto.picture.base64)) {
            h.samplePic = storePicture(u, dto.picture);
        } else if (update && dto.keepPicture) {
            h.samplePic = str(ci(stored, "SamplePic"));
        } else {
            h.samplePic = "";
        }

        if (dto.files != null && dto.files.size() > 10)
            throw new IllegalArgumentException("At most ten attachments may be uploaded at once");

        int id;
        List<Map<String, Object>> kept = new ArrayList<>();
        if (update) {
            /* The attachment list of the dialog = the stored rows the user left in it (:800-808) + new files. */
            Set<Integer> keep = new LinkedHashSet<>(dto.keepAttachmentIds == null ? List.<Integer>of() : dto.keepAttachmentIds);
            for (Map<String, Object> a : attachmentRowsRaw(u, dto.id)) {
                if (keep.contains(toInt(ci(a, "Id")))) kept.add(a);
            }
            repo.removeAttachments(dto.id, SCREEN_NAME);                           // :594 DMSAttachments.RemoveById
            repo.update(h);                                                        // :602
            id = dto.id;
        } else {
            id = repo.insert(h);                                                   // :530
            if (id <= 0) throw new IllegalStateException("The record could not be saved.");
        }

        /* DAL SetDate :28-41 — RefDocumentNo = Id, RefAccountId = SupplierCustomerId, RefDocumentTypeId 301. */
        for (Map<String, Object> a : kept) {
            repo.insertAttachment(id, h.supplierCustomerId, DOCUMENT_TYPE_ID, str(ci(a, "Attachment")),
                    now, userId, now, userId, org, comp, SCREEN_NAME,
                    emptyToNull(str(ci(a, "UploadedFileCustomName"))), toDouble(ci(a, "UploadedFileSizeMb")));
        }
        if (dto.files != null) {
            for (InventoryPosItemRequest.Upload f : dto.files) {
                byte[] bytes = DesktopInventoryItemFileService.decode(f);
                String storedName = store.store(u, f.name, bytes);
                repo.insertAttachment(id, h.supplierCustomerId, DOCUMENT_TYPE_ID, f.name,
                        now, userId, now, userId, org, comp, SCREEN_NAME, storedName, bytes.length / 1048576d);
            }
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("sampleNo", h.sampleNo);
        out.put("message", update ? "Update Successfully" : "Save Successfully");  // :605 / :533
        return out;
    }

    /**
     * btnimg1_Click (:847) + File.Copy (:539): the picture goes to the "Attachment Folder Path" folder and
     * SamplePic is the full path in it (Path.Combine(saveDirectory, fileName), :863). The file name is the
     * store's generated name; the desktop's is "name + yyyy_MM_dd_HH_mm_ss + ext".
     */
    private String storePicture(UserAccount u, InventoryPosItemRequest.Upload file) {
        String folder = store.configuration(u, ATTACHMENT_FOLDER_CONFIG);
        if (isBlank(folder)) throw new IllegalArgumentException("Please Map the Path in Configration");   // :838
        String name = file.name == null ? "" : file.name;
        String ext = name.contains(".") ? name.substring(name.lastIndexOf('.') + 1).toLowerCase(Locale.ROOT) : "";
        if (!PICTURE_EXTENSIONS.contains(ext)) throw new IllegalArgumentException("Select a .jpg, .jpeg or .png picture");
        byte[] bytes = DesktopInventoryItemFileService.decode(file);
        DesktopInventoryItemFileService.imageType(bytes);                       // refuses a file that is not an image
        String storedName = store.store(u, name, bytes);
        String dir = folder.trim();
        boolean sep = dir.endsWith("\\") || dir.endsWith("/");
        return dir + (sep ? "" : "\\") + storedName;
    }

    // ======================================================================== picture / attachments

    public DesktopInventoryItemFileService.Download picture(int id) {
        UserAccount u = requireView();
        Map<String, Object> r = own(u, id);
        byte[] bytes = r == null ? null : readPicture(u, str(ci(r, "SamplePic")));
        if (bytes == null) return null;
        String type;
        try { type = DesktopInventoryItemFileService.imageType(bytes); } catch (RuntimeException e) { type = "application/octet-stream"; }
        return new DesktopInventoryItemFileService.Download(baseName(str(ci(r, "SamplePic"))), bytes, type);
    }

    /** File.Exists(fileSavePath) (:780) against the attachment storage; null when it cannot be read. */
    private byte[] readPicture(UserAccount u, String samplePic) {
        if (isBlank(samplePic)) return null;
        try {
            return store.read(u, baseName(samplePic));
        } catch (RuntimeException e) {
            return null;
        }
    }

    /** grd_LinkClicked (:974) / ReadById (:800) — DMSAttachments.GetByID(Id, "InvLabSampleLogRegister"). */
    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = requireView();
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
            throw new AccessDeniedException("You do not have the View right for Sample Log Register.");
        return u;
    }

    /** The record, only when it belongs to the session's organization and company. */
    private Map<String, Object> own(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (toInt(ci(r, "OrganizationId")) != u.getOrganizationId() || toInt(ci(r, "CompanyId")) != u.getCompanyId()) return null;
        return r;
    }

    private String financialYearStart(UserAccount u) {
        try {
            int yearId = ctx.currentFinancialYearId();
            for (Map<String, Object> r : repo.activeFinancialYears(u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == yearId) {
                    Object v = ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(v).substring(0, 10);
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the financial year start", e);
        }
        return null;
    }

    /** The named columns only, in the given order (the form's combo tables). */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... columns) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : columns) m.put(c, ci(r, c));
            out.add(m);
        }
        return out;
    }

    /**
     * CalculateWeight (:1055): qty = Conversion.ToDouble(txtqty.Text), pack = Conversion.ToInt(CmbPackSize.Text);
     * both positive -> (qty * pack).ToString("0,0"), otherwise "0".
     */
    static String calculateWeight(String qtyText, String packText) {
        double qty = netToDouble(qtyText);
        double pack = netToInt(packText);
        if (qty > 0.0 && pack > 0.0) return format00(qty * pack);
        return "0";
    }

    /** Custom numeric format "0,0": rounded to a whole number, grouped, at least two digits ("05", "1,250"). */
    static String format00(double v) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return "0";
        BigDecimal r = new BigDecimal(v).setScale(0, java.math.RoundingMode.HALF_UP);
        String s = String.format(Locale.US, "%,d", r.toBigInteger());
        return s.length() < 2 ? "0" + s : s;
    }

    /**
     * lab.Weight = Conversion.ToInt(txtweight.Text) (:513) — quirk Q2: the text of the weight box carries
     * a thousands separator from 1,000 up and Convert.ToInt32 refuses it, so such a weight is saved as 0.
     * This is the single place to change if that desktop behaviour is to be corrected on the web.
     */
    static double savedWeight(String weightText) {
        return netToInt(weightText);
    }

    /** Conversion.ToInt(string) = Convert.ToInt32(string): optional white space and sign, digits only; else 0. */
    static int netToInt(String text) {
        if (text == null || text.isEmpty()) return 0;
        String s = text.trim();
        if (!s.matches("[+-]?\\d+")) return 0;
        try {
            return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s);
        } catch (NumberFormatException e) {
            return 0;                                                   // OverflowException -> 0
        }
    }

    /** Conversion.ToDouble(string) = Convert.ToDouble(string): float with thousands separators; else 0. */
    static double netToDouble(String text) {
        if (text == null || text.isEmpty()) return 0d;
        String s = text.trim();
        if (!s.matches("[+-]?(\\d[\\d,]*\\.?\\d*|\\.\\d+)([eE][+-]?\\d+)?")) return 0d;
        try {
            double d = Double.parseDouble(s.replace(",", ""));
            return Double.isInfinite(d) || Double.isNaN(d) ? 0d : d;
        } catch (NumberFormatException e) {
            return 0d;
        }
    }

    /** value.ToString() of a database value as .NET prints it (int, float, decimal). */
    static String netString(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) {
            double d = ((Number) v).doubleValue();
            if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
            return String.valueOf(d);
        }
        return String.valueOf(v);
    }

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
