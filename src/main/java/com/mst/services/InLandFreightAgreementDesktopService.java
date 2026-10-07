package com.mst.services;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 8 "InLand Freight Agreement" - Architecture.WinApp.Account_Definition.InLandFreightAgreement (form Name
 * "InLandFreightAgreement"), call for call. BLL/DAL read from recovered_source/projects:
 * Architecture.BLL.Accounts.InvInLandFreightAgreement (0645), Architecture.DAL.Accounts.InvInLandFreightAgreement (0576),
 * models 1174 / 1175 / 0089, GenericProvider.SetProc (0207).
 *
 * <ul>
 * <li>AccountsFill: CommonServices.SupplierCustomerGetAllServiceBind -> SupplierCustomer.Getall ->
 *     Sp_SupplierCustomer_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadByOrganizationCompanyId' (Id, CompanyName).</li>
 * <li>DistrictFill: District.GetAll -> Sp_District_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll' (Id, District).</li>
 * <li>CityFill: CommonServices.CityGetAllService -> City.GetAll -> SP_City_GetAllMethod @OrganizationId, @CompanyId,
 *     @MethodType='GetAll' (Id, CityName).</li>
 * <li>FreightTypeFill: CommonServices.StaticColumnsService("FreightType") -> SpStaticColumnNames @Activity='FreightType' (Id, type).</li>
 * <li>GenerateCode: Sp_InvInLandFreightAgreement_GetAll @Activity='GenerateCode', @OrganizationId, @CompanyId, @DocumentTypeId=161,
 *     @FinancialYearId (only when != 0) -> first row DocNo.</li>
 * <li>HistoryGridFill: FormHistory -> same proc, @Activity='FormHistory', @organizationId, @CompanyId, @DocumentTypeId, @FinancialYearId.</li>
 * <li>ReadById_Update / History "Detail": InvInLandFreightAgreement.GetByID -> DAL.GetAll: @Activity='ReadById' @Id, then per
 *     header with DocumentTypeId 161 @Activity='InvInLandFreightAgreementDetailReadById'; attachments DMSAttachments.GetByID(id,
 *     "InLandFreightAgreement") -> Sp_DMSAttachments_GetAllMethod @ScreenName, @Id, @Activity='ReadById'.</li>
 * <li>Insert(): BLL.Save -> DAL.SetData in ONE transaction: header Sp_InvInLandFreightAgreement_Insert (Id 0) or _Update (every
 *     non-virtual model property is a parameter), Sp_InvInLandFreightAgreementDetail_Insert per grid row (ActionId 1 new, 2 existing,
 *     3 removed), attachments: when the list is not empty Sp_DMSAttachments_GetAllMethod 'DeleteById' then Proc_DMSAttachments_Insert per file.</li>
 * </ul>
 */
@Service
public class InLandFreightAgreementDesktopService {

    public static final String SCREEN_NAME = "InLandFreightAgreement";
    public static final int DOCUMENT_TYPE_ID = 161;

    private static final DateTimeFormatter DT = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss", Locale.ENGLISH);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public InLandFreightAgreementDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, DesktopAttachmentStore store) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.store = store;
    }

    /** Refusal that the page shows as a plain MessageBox text (a desktop validation or database message). */
    public static class Refusal extends RuntimeException {
        private static final long serialVersionUID = 1L;
        public Refusal(String m) { super(m); }
    }

    // ------------------------------------------------------------------------------------------------- DDL

    public List<Map<String, Object>> parties() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("CompanyName", r.get("CompanyName"));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> districts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_District_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("District", r.get("District"));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SP_City_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "MethodType", "GetAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("CityName", r.get("CityName"));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> freightTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SpStaticColumnNames", DesktopProc.params("Activity", "FreightType"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("Type", r.get("type"));
            out.add(o);
        }
        return out;
    }

    /** GenerateCode(): the first row's DocNo, 0 when the procedure returns none. */
    public int generateCode() {
        Map<String, Object> p = DesktopProc.params(
                "Activity", "GenerateCode",
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID);
        int fy = ctx.currentFinancialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_InvInLandFreightAgreement_GetAll", p);
        return rows.isEmpty() ? 0 : netInt(rows.get(0).get("DocNo"));
    }

    /** InLandFreightAgreement_Load / btnRefresh_Click: AccountsFill, DistrictFill, CityFill, FreightTypeFill, GenerateCode. */
    public Map<String, Object> load() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("parties", parties());
        out.put("districts", districts());
        out.put("cities", cities());
        out.put("freightTypes", freightTypes());
        out.put("docNo", generateCode());
        return out;
    }

    // ------------------------------------------------------------------------------------------------- history / read

    /** HistoryGridFill: FormHistory rows (dates as yyyy-MM-dd HH:mm:ss). */
    public List<Map<String, Object>> history() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_InvInLandFreightAgreement_GetAll", DesktopProc.params(
                "Activity", "FormHistory",
                "organizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "FinancialYearId", ctx.currentFinancialYearId()))) {
            out.add(plain(r));
        }
        return out;
    }

    /** GetByID(Id)[0] (throws as List[0] does when there is none) - the desktop reaches ids only through its own history grid, so the row must belong to the signed-in company. */
    private Map<String, Object> header(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_InvInLandFreightAgreement_GetAll", DesktopProc.params(
                "Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) throw new Refusal("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> h = rows.get(0);
        if (netInt(h.get("OrganizationId")) != ctx.currentOrganizationId() || netInt(h.get("CompanyId")) != ctx.currentCompanyId())
            throw new org.springframework.security.access.AccessDeniedException("Record does not belong to the current company");
        return h;
    }

    private List<Map<String, Object>> details(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : DesktopProc.rows(jdbc, "Sp_InvInLandFreightAgreement_GetAll", DesktopProc.params(
                "Id", id, "Activity", "InvInLandFreightAgreementDetailReadById"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "LoadingDistrictId", "DistrictName", "LoadingLocationId", "LoadingLocationName", "DestinationLocationId",
                    "UnLoadingLocationName", "EffectiveDate", "FreightTypeId", "FreightType", "FreightRate", "FreightUom", "PackSizeTo",
                    "FregihtAmount", "MinAmount", "RemarksDetail" }) o.put(k, norm(d.get(k)));
            out.add(o);
        }
        return out;
    }

    /** DMSAttachments.GetByID(id, base.Name). */
    public List<Map<String, Object>> attachments(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "ScreenName", SCREEN_NAME, "Id", id, "Activity", "ReadById"))) {
            if (netInt(a.get("OrganizationId")) != ctx.currentOrganizationId() || netInt(a.get("CompanyId")) != ctx.currentCompanyId()) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", a.get("Id"));
            o.put("Attachment", a.get("Attachment"));
            o.put("UploadedFileCustomName", a.get("UploadedFileCustomName"));
            o.put("UploadedFileSizeMb", norm(a.get("UploadedFileSizeMb")));
            o.put("EntryDate", norm(a.get("EntryDate")));
            o.put("EntryUserName", a.get("EntryUserName"));
            out.add(o);
        }
        return out;
    }

    /** ReadById_Update / History Detail: header, grid rows, attachments. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "DocNo", "AgreementDate", "AgreementCode", "SupplierCustomerId", "TransporterId", "DocumentTypeId" })
            head.put(k, norm(h.get(k)));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", head);
        /* DAL.GetAll reads the details only for DocumentTypeId == 161 */
        out.put("details", netInt(h.get("DocumentTypeId")) == DOCUMENT_TYPE_ID ? details(id) : new ArrayList<Map<String, Object>>());
        out.put("attachments", attachments(id));
        return out;
    }

    // ------------------------------------------------------------------------------------------------- save

    public static class Upload {
        public String name;
        public String base64;
    }

    public static class SaveRequest {
        public int id;
        public String docDate;
        public String agreementCode;
        public String docNo;
        public Object partyId;
        public Object transporterId;
        public List<Map<String, Object>> detail;
        public List<Map<String, Object>> removed;
        /** ids of the already saved attachments the list still holds (Attachment.lst, ReadById_Update) */
        public List<Integer> keepAttachments;
        public List<Upload> addAttachments;
    }

    /**
     * Insert() after the confirm. FormValidation / "Grid Record Not Found" are repeated here with the desktop's texts.
     * Returns {id, docNo}.
     */
    @Transactional
    public Map<String, Object> save(SaveRequest r) {
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId(), user = ctx.currentUserId();
        int fy = ctx.currentFinancialYearId();
        String docNoText = r.docNo == null ? "" : r.docNo.trim();
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new Refusal("DocNo Field is Required");
        int partyId = netInt(r.partyId), transporterId = netInt(r.transporterId);
        if (r.partyId == null || String.valueOf(r.partyId).trim().isEmpty()) throw new Refusal("Customer Field is Required");
        if (r.transporterId == null || String.valueOf(r.transporterId).trim().isEmpty()) throw new Refusal("Transporter Field is Required");
        if (r.detail == null || r.detail.isEmpty()) throw new Refusal("Grid Record Not Found");

        Set<Integer> partyIds = ids(parties());
        if (!partyIds.contains(partyId)) throw new Refusal("Customer Field is Required");
        if (!partyIds.contains(transporterId)) throw new Refusal("Transporter Field is Required");
        Set<Integer> districtIds = ids(districts()), cityIds = ids(cities());

        int recId = r.id;
        if (recId < 0) throw new Refusal("Invalid record");
        Set<Integer> ownDetailIds = new HashSet<>();
        if (recId > 0) {
            header(recId);                                               // belongs to the signed-in company
            for (Map<String, Object> d : jdbc.queryForList(
                    "SELECT Id FROM dbo.InvInLandFreightAgreementDetail WHERE InvInLandFreightAgreement = ?", recId))
                ownDetailIds.add(netInt(d.get("Id")));
        }

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        String code = r.agreementCode == null ? "" : r.agreementCode.trim();
        /* obj fields in the model's property order; BLL.Save picks Insert for Id == 0, Update otherwise */
        int num = DesktopProc.setProc(jdbc, recId == 0 ? "Sp_InvInLandFreightAgreement_Insert" : "Sp_InvInLandFreightAgreement_Update", DesktopProc.params(
                "IsApproved", Boolean.FALSE,
                "AgreementDate", ts(r.docDate),
                "ApprovedDate", now,
                "EntryDate", now,
                "ModifyDate", now,
                "ApprovedUser", user,
                "BranchId", 0,
                "CompanyId", company,
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "EntryUser", user,
                "FinancialYearId", fy,
                "Id", recId,
                "ModifyUser", user,
                "OrganizationId", org,
                "ProjectId", 0,
                "DocNo", netInt(docNoText),
                "SupplierCustomerId", partyId,
                "TransporterId", transporterId,
                "AgreementCode", code));
        int hid = num > 0 ? num : recId;                                  // DAL: num > 0 ? obj.Id = num : num = obj.Id

        /* obj.InvInLandFreightAgreementDetailList: the grid rows first, then (RecId > 0) the removed rows */
        List<Map<String, Object>> all = new ArrayList<>();
        for (Map<String, Object> row : r.detail) {
            int did = netInt(row.get("Id"));
            if (did > 0 && !ownDetailIds.contains(did)) throw new Refusal("Record does not belong to this agreement");
            if (!districtIds.contains(netInt(row.get("DistrictId")))) throw new Refusal("District Field is Required");
            if (!cityIds.contains(netInt(row.get("LoadingLocationId")))) throw new Refusal("Loading Location Field is Required");
            if (!cityIds.contains(netInt(row.get("UnLoadingLocationId")))) throw new Refusal("Unloading Location Field is Required");
            Map<String, Object> d = new LinkedHashMap<>(row);
            d.put("ActionId", did > 0 ? 2 : 1);
            all.add(d);
        }
        if (recId > 0 && r.removed != null) {
            for (Map<String, Object> row : r.removed) {
                int did = netInt(row.get("Id"));
                if (did > 0 && !ownDetailIds.contains(did)) continue;    // a stale entry of another document is not this agreement's row
                Map<String, Object> d = new LinkedHashMap<>(row);
                d.put("ActionId", 3);
                all.add(d);
            }
        }
        for (Map<String, Object> d : all) {
            int action = netInt(d.get("ActionId"));
            /* The model's DateTime fields the form does not set are Now for a grid row. A removed row only carries ActionId 3 (the
               procedure just flags the row); its dates are sent as Now instead of DateTime.MinValue, which the desktop's SqlDateTime
               parameter cannot carry. */
            DesktopProc.setProc(jdbc, "Sp_InvInLandFreightAgreementDetail_Insert", DesktopProc.params(
                    "IsActive", Boolean.FALSE,
                    "IsApproved", Boolean.FALSE,
                    "ApprovedDate", now,
                    "EffectiveDate", ts(str(d.get("EffectiveDate"))),
                    "EnteryDate", now,
                    "ModifyDate", now,
                    "FregihtAmount", dec(d.get("FreightAmount")),
                    "FreightRate", dec(d.get("FreightRate")),
                    "MinAmount", dec(d.get("MinAmount")),
                    "ActionId", action,
                    "ApprovedUserId", user,
                    "DestinationLocationId", netInt(d.get("UnLoadingLocationId")),
                    "EnteryUserId", user,
                    "FreightTypeId", netInt(d.get("FreightTypeId")),
                    "FreightUom", netInt(d.get("FreightUom")),
                    "Id", netInt(d.get("Id")),
                    "InvInLandFreightAgreement", hid,
                    "LoadingDistrictId", netInt(d.get("DistrictId")),
                    "LoadingLocationId", netInt(d.get("LoadingLocationId")),
                    "ModifyUserId", user,
                    "PackSizeTo", netInt(d.get("PackSizeTo")),
                    "RemarksDetail", str(d.get("RemarksDetail")),
                    "SubAgreementCode", code));
        }

        saveAttachments(r, hid, org, company, user, now);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", hid);
        out.put("docNo", netInt(docNoText));
        return out;
    }

    /** DAL: only when AttachmentsList is not empty - DeleteById, then one Proc_DMSAttachments_Insert per file. */
    private void saveAttachments(SaveRequest r, int hid, int org, int company, int user, Timestamp now) {
        List<Map<String, Object>> keep = new ArrayList<>();
        if (r.id > 0 && r.keepAttachments != null && !r.keepAttachments.isEmpty()) {
            Set<Integer> want = new HashSet<>(r.keepAttachments);
            for (Map<String, Object> a : attachments(hid)) if (want.contains(netInt(a.get("Id")))) keep.add(a);
        }
        List<Upload> add = r.addAttachments == null ? new ArrayList<Upload>() : r.addAttachments;
        if (keep.isEmpty() && add.isEmpty()) return;
        if (add.size() > 10) throw new Refusal("Select at most ten files at once");

        DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "ScreenName", SCREEN_NAME, "Id", hid, "Activity", "DeleteById"));
        for (Map<String, Object> a : keep) insertAttachment(hid, org, company, user, now, str(a.get("Attachment")),
                str(a.get("UploadedFileCustomName")), netDouble(a.get("UploadedFileSizeMb")));
        for (Upload up : add) {
            byte[] bytes = decode(up);
            String stored = store.store(ctx.requireAccountingUser(), up.name, bytes);
            insertAttachment(hid, org, company, user, now, up.name, stored, bytes.length / 1048576d);
        }
    }

    private void insertAttachment(int hid, int org, int company, int user, Timestamp now, String name, String stored, double mb) {
        Map<String, Object> p = DesktopProc.params(
                "Id", 0,
                "RefAccountId", 0,
                "DMSFoldersLabelsId", 0,
                "RefDocumentTypeId", DOCUMENT_TYPE_ID,
                "RefDocumentNo", hid,
                "Attachment", name,
                "EntryDate", now,
                "EntryUser", user,
                "ModifyDate", now,
                "ModifyUser", user,
                "OrganizationId", org,
                "CompanyId", company,
                "BranchId", 0,
                "ScreenName", SCREEN_NAME,
                "DetailWiseAttachment", Boolean.FALSE,
                "UploadedFileCustomName", stored == null || stored.isEmpty() ? null : stored,
                "UploadedFileSizeMb", mb,
                "LineId", 0);
        DesktopProc.setProc(jdbc, "Proc_DMSAttachments_Insert", p);
    }

    private static byte[] decode(Upload up) {
        if (up == null || up.name == null) throw new Refusal("Invalid attachment");
        DesktopAttachmentStore.validateName(up.name);
        if (up.base64 == null || up.base64.length() > 7 * 1024 * 1024) throw new Refusal("Attachment exceeds 5 MB");
        byte[] b;
        try {
            b = Base64.getDecoder().decode(up.base64);
        } catch (IllegalArgumentException e) {
            throw new Refusal("Invalid attachment content");
        }
        if (b.length == 0 || b.length > DesktopAttachmentStore.MAX_BYTES) throw new Refusal("Attachment must be between 1 byte and 5 MB");
        return b;
    }

    public static class Download {
        public final String name;
        public final byte[] bytes;
        public Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    /** GrdHistory_LinkClicked -> AttachmentView / Attachment card click: opening one listed file. */
    public Download download(int docId, int attachmentId) {
        header(docId);
        for (Map<String, Object> a : attachments(docId)) {
            if (netInt(a.get("Id")) != attachmentId) continue;
            String original = base(str(a.get("Attachment")));
            String stored = str(a.get("UploadedFileCustomName"));
            return new Download(original, store.read(ctx.requireAccountingUser(), base(stored.isEmpty() ? original : stored)));
        }
        throw new Refusal("Attachment not found");
    }

    private static String base(String v) {
        String n = v.replace('\\', '/');
        n = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(n);
        return n;
    }

    // ------------------------------------------------------------------------------------------------- print

    /** VoucherReports.InlandFreightAggreement: DocumentTypeId 161, FinancialYearId, and @Id only when != 0. */
    public List<Map<String, Object>> printRows(int id) {
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID);
        if (id != 0) p.put("Id", id);
        return DesktopProc.rows(jdbc, "SP_InLandFreightAgreementSlipAndRegister", p);
    }

    // ------------------------------------------------------------------------------------------------- helpers

    private static Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(netInt(r.get("Id")));
        return s;
    }

    /** Conversion.ToInt: null / "" / a non-integer string -> 0. */
    static int netInt(Object v) {
        if (v == null) return 0;
        try {
            if (v instanceof Number) {
                double d = ((Number) v).doubleValue();
                if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
                double x = Math.rint(d);
                if (x > Integer.MAX_VALUE || x < Integer.MIN_VALUE) return 0;
                return (int) x;
            }
            String s = String.valueOf(v).trim();
            if (!s.matches("[+-]?\\d+")) return 0;
            return Integer.parseInt(s);
        } catch (RuntimeException e) {
            return 0;
        }
    }

    static double netDouble(Object v) {
        if (v == null) return 0.0;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(String.valueOf(v).trim());
            return Double.isNaN(d) || Double.isInfinite(d) ? 0.0 : d;
        } catch (RuntimeException e) {
            return 0.0;
        }
    }

    /** Conversion.ToDecimal. */
    private static BigDecimal dec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        try {
            if (v instanceof BigDecimal) return (BigDecimal) v;
            String s = String.valueOf(v).trim();
            return s.isEmpty() ? BigDecimal.ZERO : new BigDecimal(s);
        } catch (RuntimeException e) {
            return BigDecimal.ZERO;
        }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** 'yyyy-MM-dd' or 'yyyy-MM-dd HH:mm:ss' / 'T' form -> Timestamp (a date alone gets 00:00:00). */
    private static Timestamp ts(String s) {
        if (s == null || s.trim().isEmpty()) throw new Refusal("Invalid date");
        try {
            String t = s.trim().replace('T', ' ');
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atTime(LocalTime.MIDNIGHT));
            if (t.length() == 16) t = t + ":00";
            return Timestamp.valueOf(LocalDateTime.parse(t.substring(0, 19), DT));
        } catch (RuntimeException e) {
            throw new Refusal("Invalid date");
        }
    }

    /** JSON friendly value: Timestamp -> yyyy-MM-dd HH:mm:ss, BigDecimal -> plain number string. */
    private static Object norm(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(DT);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(DT);
        if (v instanceof BigDecimal) return ((BigDecimal) v).stripTrailingZeros().toPlainString();
        return v;
    }

    private static Map<String, Object> plain(Map<String, Object> row) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : row.entrySet()) o.put(e.getKey(), norm(e.getValue()));
        return o;
    }
}
