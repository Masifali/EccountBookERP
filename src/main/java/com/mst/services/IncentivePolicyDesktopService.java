package com.mst.services;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 7 "Incentive Policy" - Architecture.WinApp.WholeSale.frmIncentivePolicy, call for call
 * (BLL/DAL read from recovered_source/projects: Architecture.BLL.Pos.WsRmIncentivePolicyHeader / DrmLookUps,
 * Architecture.DAL.Pos.WsRmIncentivePolicyHeader, Architecture.BLL.Inventory.Item).
 *
 * <ul>
 * <li>crAdvoiceDurationFilll / TargetBaseOnFilll / IncentiveTypeFilll / IncentiveCalcOnFilll:
 *     DrmLookUps.GetByLookUpsTypeId(10 / 7 / 8 / 9) -> Sp_drmLookUpsGetAllMethods @Id, @Activity='GetByLookUpsTypeId'
 *     (rows Id, LookUpName).</li>
 * <li>ItemFilll: Item.GetAllbyCombobind -> Sp_Item_GetAllMethod @OrganizationId, @CompanyId,
 *     @Activity='ReadAllForComboTwoColumns' (@InventoryParentCategoriesId only when ItemCategoryId != 0: not here).</li>
 * <li>HistoryFill: WsRmIncentivePolicyHeader.FormHistoryOnlyHeader -> Sp_WsRmIncentivePolicyHeader_GetAllMetod
 *     @OrganizationId, @CompanyId, @Activity='FormHistoryOnlyHeader'.</li>
 * <li>ReadById / History Detail: WsRmIncentivePolicyHeader.GetByID -> DAL.GetData: Sp_WsRmIncentivePolicyHeader_GetAllMetod
 *     @Id, @Activity='ReadById', then per header row @Id, @Activity='ReadDetailByHeaderId'.</li>
 * <li>btnsave_Click: BLL.Save stamps EntryDate / ModifyDate = Now; Id == 0 -> Sp_WsRmIncentivePolicyHeader_Insert else
 *     Sp_WsRmIncentivePolicyHeader_Update; DAL.SetData runs in ONE transaction: header SetProc (every non-virtual model
 *     property is a parameter), then Sp_WsRmIncentivePolicyDetail_Insert per grid row with
 *     WsRmIncentivePolicyHeaderId = the saved id; rollback on any error.</li>
 * </ul>
 * The desktop reads the Save / Print / Update / CanView AllRecord rights in Load but never uses them: the form has no gate.
 */
@Service
public class IncentivePolicyDesktopService {

    private static final DateTimeFormatter DT = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss", Locale.ENGLISH);
    private static final DateTimeFormatter D_MMM = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public IncentivePolicyDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    /** DrmLookUps.GetByLookUpsTypeId(typeId). */
    public List<Map<String, Object>> lookUps(int typeId) {
        return DesktopProc.rows(jdbc, "Sp_drmLookUpsGetAllMethods", DesktopProc.params(
                "Id", typeId, "Activity", "GetByLookUpsTypeId"));
    }

    /** Item.GetAllbyCombobind. */
    public List<Map<String, Object>> items() {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadAllForComboTwoColumns"));
    }

    /** All five combos, in the desktop's Load order. */
    public Map<String, Object> combos() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("advoiceDuration", lookUps(10));
        out.put("targetBaseOn", lookUps(7));
        out.put("items", items());
        out.put("incentiveType", lookUps(8));
        out.put("incentiveCalcOn", lookUps(9));
        return out;
    }

    /** HistoryFill(): header rows with the dates already formatted dd-MMM-yyyy. */
    public List<Map<String, Object>> history() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_WsRmIncentivePolicyHeader_GetAllMetod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "FormHistoryOnlyHeader"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("PolicyDateFrom", dMmm(r.get("SchemeDateFrom")));
            o.put("PolicyDateTo", dMmm(r.get("SchemeDateTo")));
            o.put("PolicyDescription", r.get("SchemeDescription"));
            o.put("CrAdvoiceDuration", r.get("CrAdvoiceTermsName"));
            o.put("Remarks", r.get("RemarksHeader"));
            out.add(o);
        }
        return out;
    }

    /** WsRmIncentivePolicyHeader.GetByID: [0] header (throws when none, as GetData(...)[0] does), details. */
    public Map<String, Object> getById(int id) {
        List<Map<String, Object>> heads = DesktopProc.rows(jdbc, "Sp_WsRmIncentivePolicyHeader_GetAllMetod", DesktopProc.params(
                "Id", id, "Activity", "ReadById"));
        if (heads.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> h = heads.get(0);
        Integer hid = h.get("Id") instanceof Number ? ((Number) h.get("Id")).intValue() : id;
        List<Map<String, Object>> det = DesktopProc.rows(jdbc, "Sp_WsRmIncentivePolicyHeader_GetAllMetod", DesktopProc.params(
                "Id", hid, "Activity", "ReadDetailByHeaderId"));
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> header = new LinkedHashMap<>();
        header.put("Id", h.get("Id"));
        header.put("SchemeDescription", h.get("SchemeDescription"));
        header.put("SchemeDateFrom", dt(h.get("SchemeDateFrom")));
        header.put("SchemeDateTo", dt(h.get("SchemeDateTo")));
        header.put("CrAdvoiceTerms", h.get("CrAdvoiceTerms"));
        header.put("RemarksHeader", h.get("RemarksHeader"));
        out.put("header", header);
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> d : det) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "TargetBaseTypeId", "TargetBaseType", "ItemId", "ItemName", "IncentiveTypeId", "IncentiveType",
                    "TargetRangeFrom", "TargetRangeTo", "IncentiveRate", "IncentiveCalcTypeId", "RemarksDetail" }) o.put(k, d.get(k));
            details.add(o);
        }
        out.put("details", details);
        return out;
    }

    /**
     * btnsave_Click after the confirm: the header + the grid rows. {@code detail} rows carry the grid's cells
     * (TargetBaseTypeId, ItemId, IncentiveTypeId, TargetRangeFrom, TargetRangeTo, IncentiveRate, IncentiveCalcOn, RemarksDetail).
     * Returns the saved header id.
     */
    @Transactional
    public int save(int recId, String dateFrom, String dateTo, String description, Object crAdvoiceTerms, String remarks,
                    List<Map<String, Object>> detail) {
        /* FormValidation() */
        if (description == null || description.isEmpty()) throw new IllegalArgumentException("Policy Description Required");
        if (crAdvoiceTerms == null || String.valueOf(crAdvoiceTerms).trim().isEmpty()) throw new IllegalArgumentException("Cr Advoice Duration Required");
        int user = ctx.currentUserId();
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        if (detail == null || detail.isEmpty()) throw new IllegalStateException("Grid record not found");
        String proc = recId == 0 ? "Sp_WsRmIncentivePolicyHeader_Insert" : "Sp_WsRmIncentivePolicyHeader_Update";
        int num = DesktopProc.setProc(jdbc, proc, DesktopProc.params(
                "IsApproved", Boolean.FALSE,
                "ApprovedDate", now,
                "EntryDate", now,
                "ModifyDate", now,
                "SchemeDateFrom", ts(dateFrom),
                "SchemeDateTo", ts(dateTo),
                "ApprovedUserId", user,
                "BranchesId", 0,
                "CompanyId", company,
                "CrAdvoiceTerms", netInt(crAdvoiceTerms),
                "EntryUser", user,
                "Id", recId,
                "ModifyUser", user,
                "OrganizationId", org,
                "ProjectsId", 0,
                "RemarksHeader", remarks == null ? "" : remarks,
                "SchemeDescription", description));
        int hid = num > 0 ? num : recId;                       // DAL: num > 0 ? obj.Id = num : num = obj.Id
        for (Map<String, Object> r : detail) {
            DesktopProc.setProc(jdbc, "Sp_WsRmIncentivePolicyDetail_Insert", DesktopProc.params(
                    "IncentiveRate", netDouble(r.get("IncentiveRate")),
                    "TargetRangeFrom", (double) netInt(r.get("TargetRangeFrom")),     // Conversion.ToInt(...) assigned to a double
                    "TargetRangeTo", (double) netInt(r.get("TargetRangeTo")),
                    "Id", 0,
                    "IncentiveTypeId", netInt(r.get("IncentiveTypeId")),
                    "ItemId", netInt(r.get("ItemId")),
                    "TargetBaseTypeId", netInt(r.get("TargetBaseTypeId")),
                    "WsRmIncentivePolicyHeaderId", hid,
                    "IncentiveCalcTypeId", netInt(r.get("IncentiveCalcOn")),
                    "RemarksDetail", str(r.get("RemarksDetail"))));
        }
        return hid;
    }

    // ------------------------------------------------------------------ Conversion.* (Architecture.Common.Conversion)

    /** Conversion.ToInt: null / "" -> 0; Convert.ToInt32 (doubles round half-to-even; a non-integer string throws -> 0). */
    static int netInt(Object v) {
        if (v == null) return 0;
        try {
            if (v instanceof Number) {
                double d = ((Number) v).doubleValue();
                if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
                double r = Math.rint(d);
                if (r > Integer.MAX_VALUE || r < Integer.MIN_VALUE) return 0;
                return (int) r;
            }
            String s = String.valueOf(v).trim();
            if (s.isEmpty()) return 0;
            if (!s.matches("[+-]?\\d+")) return 0;
            return Integer.parseInt(s);
        } catch (RuntimeException e) {
            return 0;
        }
    }

    /** Conversion.ToDouble: null / "" / unparsable / infinite -> 0. */
    static double netDouble(Object v) {
        if (v == null) return 0.0;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(String.valueOf(v).trim());
            return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d;
        } catch (RuntimeException e) {
            return 0.0;
        }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** 'yyyy-MM-dd' or 'yyyy-MM-dd HH:mm:ss' / 'T' form -> Timestamp (a date alone gets 00:00:00). */
    private static Timestamp ts(String s) {
        if (s == null || s.trim().isEmpty()) throw new IllegalArgumentException("Invalid date");
        String t = s.trim().replace('T', ' ');
        if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atTime(LocalTime.MIDNIGHT));
        if (t.length() == 16) t = t + ":00";
        return Timestamp.valueOf(LocalDateTime.parse(t.substring(0, 19), DT));
    }

    private static String dt(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(DT);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(DT);
        return String.valueOf(v);
    }

    private static String dMmm(Object v) {
        if (v == null) return "01-Jan-0001";                    // Conversion.ToDateTime(null) -> DateTime.MinValue
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(D_MMM);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().format(D_MMM);
        return String.valueOf(v);
    }
}
