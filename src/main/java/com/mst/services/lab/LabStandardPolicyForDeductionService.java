package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.lab.LabStandardPolicyForDeductionRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.lab.LabStandardPolicyForDeductionRepository.ci;

/**
 * Lab (ModuleId 7) — screen 166 "Lab Standard Policy For Deduction (Not Use)".
 *
 * Desktop form   Architecture.WinApp.Lab/InvLabStandardPolicyForDeduction.cs   (line refs ":n" below)
 * ScreenName     InvLabStandardPolicyForDeduction   (base.Name :1804)
 * BLL            architecture.bll/0398_Architecture.BLL.Lab.InvLabAnalysisStandardDeductionPolicyHeader.cs
 * DAL            architecture.dal/0355_Architecture.DAL.Lab.InvLabAnalysisStandardDeductionPolicyHeader.cs
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE (btnSave_Click :586) / UPDATE (btnUpdate_Click :500) -> Insert() (:516) — one transaction
 * ---------------------------------------------------------------------------------------------
 *   Save:   "Grid Record Not Found" -> FormValidation (:436) ->
 *           Sp_InvLabAnalysisStandardDeductionPolicyHeader_Insert (returns the new Id) ->
 *           Sp_InvLabAnalysisStandardDeductionPolicyDetail_Insert per grid row.
 *   Update: the same checks -> Sp_InvLabAnalysisStandardDeductionPolicyHeader_Update (it deletes the
 *           header's detail rows itself) -> Sp_InvLabAnalysisStandardDeductionPolicyDetail_Insert per row.
 * The DAL (SetData :11) runs header + details in one SqlTransaction and rolls back on any error; here
 * that is the @Transactional of {@link #save}.
 * The form has no Delete button and the BLL no delete method, so there is no delete here.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP FACTS REPRODUCED
 * ---------------------------------------------------------------------------------------------
 *  F1 The form reads no screen rights at all (Lab_Load :163 never calls a rights service), so Save and
 *     Update are not gated by the Save / Update rights; only opening the screen (the menu's View right)
 *     is checked here.
 *  F2 Insert() validates only "at least one grid row", Apply On and Item. The detail rows are validated
 *     when they are ADDED (FormValidationDetail :453) — a row changed through the detail "Update"
 *     button (:372) is not validated at all, and is saved as it stands.
 *  F3 chkIsaccepted ("Is Active", checked) is never read and nothing of it is saved; IsApproved is
 *     always false; PolicyName is never sent; BranchId / ProjectId / ApprovedUserId are 0; SortNo is 0.
 *  F4 WeightKg is Conversion.ToDouble of the Weight cell's text (:559) — the 'GetUom' list is numeric
 *     ("1", "5", "10", "40", "50", "60", "80", "100").
 *
 * Tenancy (organization, company, user) comes from the session only.
 */
@Service
public class LabStandardPolicyForDeductionService {

    public static final String SCREEN_NAME = "InvLabStandardPolicyForDeduction";   // ScreenDefinition Id 166
    public static final int SCREEN_ID = 166;

    private final LabStandardPolicyForDeductionRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public LabStandardPolicyForDeductionService(LabStandardPolicyForDeductionRepository repo,
                                                StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    /** One grid row as the form's DataTable holds it (:177-183) — cell texts. */
    public static class DetailRow {
        public String perameterId;          // cmbparameter.Value
        public String analysisPerameter;    // cmbparameter.Text (display only)
        public String rangeFrom;            // double column
        public String rangeTo;              // double column
        public String deductionOn;          // cmbdeduction.Text
        public String deductionValue;       // double column
        public String weight;               // cmbweight.Text
    }

    /** What the page posts on Save / Update. */
    public static class SaveRequest {
        public int id;                      // RecId — 0 = Save, otherwise Update
        public String policyApplyOn;        // cmbApplyOn.Text
        public int itemId;                  // cmbitem.Value
        public String effectiveFrom;        // txtAffectiveDate.Value, yyyy-MM-dd
        public String effectiveTo;          // TxtAffectiveDateTo.Value, yyyy-MM-dd
        public String remarksHeader;        // txtremarks.Text
        public List<DetailRow> rows = new ArrayList<>();
    }

    // ======================================================================== form load

    /** Lab_Load (:163) — ApplyOnFill, ItemBind, LabItemPerameterFill, DeductionTypeFill, UOM. */
    public Map<String, Object> lookups() {
        UserAccount u = requireView();
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenId", SCREEN_ID);
        out.put("screenName", SCREEN_NAME);
        out.put("today", LocalDate.now().toString());                                       // DateTimePicker default
        out.put("applyOn", pick(repo.staticColumns("LabApplyOn"), "Id", "type"));           // :235
        out.put("items", pick(repo.items(org, comp),                                        // :192
                "Id", "ItemName", "ItemCategory", "ItemCode", "InventoryParentCategoriesId"));
        out.put("parameters", pick(repo.analysisParameters(org, comp),                      // :209
                "Id", "AnalysisParameterDescription"));
        out.put("deductionOn", pick(repo.staticColumns("LabDeductionOn"), "Id", "type"));   // :256
        out.put("uom", pick(repo.staticColumns("GetUom"), "Id", "type"));                   // :276
        return out;
    }

    /** gridhistoryfill (:712) — every policy of the organization + company; the Id column is hidden (:728). */
    public List<Map<String, Object>> history() {
        UserAccount u = requireView();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.readAll(u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("PolicyApplyOn", ci(r, "PolicyApplyOn"));
            m.put("ItemName", ci(r, "ItemName"));
            m.put("EffectiveFrom", iso(ci(r, "EffectiveFrom")));
            m.put("EfffectiveTo", iso(ci(r, "EfffectiveTo")));
            m.put("RemarksHeader", ci(r, "RemarksHeader"));
            out.add(m);
        }
        return out;
    }

    /**
     * getUpdate (:600). BLL GetById (:61) takes element [0] of the result, so an unknown id is the
     * ArgumentOutOfRangeException text on the desktop; the same text is the refusal here (also for a
     * record of another organization / company, which the desktop's history grid never lists).
     */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> h = own(u, id);
        if (h == null)
            throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("PolicyApplyOn", str(ci(h, "PolicyApplyOn")));                   // :611
        out.put("ItemId", toInt(ci(h, "ItemId")));                               // :612
        out.put("ItemName", str(ci(h, "ItemName")));
        out.put("EffectiveFrom", iso(ci(h, "EffectiveFrom")));                   // :613
        out.put("EfffectiveTo", iso(ci(h, "EfffectiveTo")));                     // :614
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));                   // :615
        /* :617-620 — table.Rows.Add(AnalysisParameterId, AnalysisParameterDescription, RangeFrom, RangeTo,
           DeductFrom, DedValue, WeightKg): the three numeric(18,3) values land in double columns, WeightKg
           (float) in the untyped (string) "Weight" column. */
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.readDetail(toInt(ci(h, "Id")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("PerameterId", netString(toInt(ci(d, "AnalysisParameterId"))));
            m.put("AnalysisPerameter", str(ci(d, "AnalysisParameterDescription")));
            m.put("RangeFrom", toDouble(ci(d, "RangeFrom")));
            m.put("RangeTo", toDouble(ci(d, "RangeTo")));
            m.put("DeductionOn", str(ci(d, "DeductFrom")));
            m.put("DeductionValue", toDouble(ci(d, "DedValue")));
            m.put("Weight", netString(toDouble(ci(d, "WeightKg"))));
            rows.add(m);
        }
        out.put("rows", rows);
        return out;
    }

    // ======================================================================== save / update

    /** Insert() (:516) — btnSave_Click (:586) when id == 0, btnUpdate_Click (:500) otherwise. */
    @Transactional
    public Map<String, Object> save(SaveRequest dto) {
        UserAccount u = requireView();
        if (dto == null) throw new IllegalArgumentException("Grid Record Not Found");
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        boolean update = dto.id != 0;

        /* :520 */
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* FormValidation (:436): cmbApplyOn.ActiveRow / cmbitem.ActiveRow — a row of the combo's own list. */
        String applyOn = dto.policyApplyOn == null ? "" : dto.policyApplyOn;
        if (!hasText(repo.staticColumns("LabApplyOn"), "type", applyOn))
            throw new IllegalArgumentException("ApplyOn field is required");
        if (dto.itemId == 0 || !hasId(repo.items(org, comp), dto.itemId))
            throw new IllegalArgumentException("ItemName Field is Required");

        /* Update works on a stored record of this organization + company only. */
        if (update && own(u, dto.id) == null)
            throw new IllegalArgumentException("Recod cannot be updated because RecId not found");          // :506

        /* The parameter of every row must be one of this company's analysis parameters (the combo's list). */
        Set<Integer> parameterIds = new HashSet<>();
        for (Map<String, Object> p : repo.analysisParameters(org, comp)) parameterIds.add(toInt(ci(p, "Id")));
        for (DetailRow r : dto.rows) {
            if (r == null || !parameterIds.contains(netToInt(r.perameterId)))
                throw new IllegalArgumentException("Parameter Field is Required");
        }

        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        LabStandardPolicyForDeductionRepository.Header h = new LabStandardPolicyForDeductionRepository.Header();
        h.id = update ? dto.id : 0;                                               // :530
        h.policyApplyOn = applyOn;                                                // :536
        h.itemId = dto.itemId;                                                    // :537
        h.effectiveFrom = Timestamp.valueOf(day(dto.effectiveFrom).atStartOfDay());   // :538
        h.efffectiveTo = Timestamp.valueOf(day(dto.effectiveTo).atStartOfDay());      // :539
        h.remarksHeader = dto.remarksHeader == null ? "" : dto.remarksHeader;     // :540 Conversion.ToString
        h.companyId = comp;                                                       // :541
        h.organizationId = org;                                                   // :542
        h.entryDate = now;                                                        // :543 / BLL :21
        h.entryUserId = u.getId();                                                // :544
        h.modifyDate = now;                                                       // :545 / BLL :24
        h.approvedDate = now;                                                     // :546
        h.isApproved = false;

        int id;
        if (update) {
            repo.updateHeader(h);                                                 // also deletes the detail rows
            id = dto.id;
        } else {
            id = repo.insertHeader(h);
            if (id <= 0) throw new IllegalStateException("The record could not be saved.");
        }

        /* :548-560 + DAL :28-32 */
        for (DetailRow r : dto.rows) {
            repo.insertDetail(id,
                    netToInt(r.perameterId),                                      // Conversion.ToInt   :552
                    netToDecimal(r.rangeFrom),                                    // Conversion.ToDecimal :553
                    netToDecimal(r.rangeTo),                                      // :554
                    r.deductionOn == null ? "" : r.deductionOn,                   // Conversion.ToString :555
                    netToDecimal(r.deductionValue),                               // :556
                    netToDouble(r.weight));                                       // Conversion.ToDouble :557
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", update ? "Update SuccessFully" : "Save SuccessFully");  // :563 / :567
        return out;
    }

    // ======================================================================== helpers

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view"))
            throw new AccessDeniedException("You do not have the View right for Lab Standard Policy For Deduction.");
        return u;
    }

    /** The header row, only when it belongs to the session's organization and company. */
    private Map<String, Object> own(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (toInt(ci(r, "OrganizationId")) != u.getOrganizationId() || toInt(ci(r, "CompanyId")) != u.getCompanyId()) return null;
        return r;
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

    private static boolean hasText(List<Map<String, Object>> rows, String column, String text) {
        if (text == null || text.trim().isEmpty()) return false;
        for (Map<String, Object> r : rows) {
            if (str(ci(r, column)).trim().equalsIgnoreCase(text.trim())) return true;
        }
        return false;
    }

    private static boolean hasId(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) {
            if (toInt(ci(r, "Id")) == id) return true;
        }
        return false;
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

    /**
     * Conversion.ToDecimal of a grid cell (a double value or its text): the cell's number as a decimal,
     * anything that is not a number 0. The procedure's parameter is NUMERIC(18,3).
     */
    static BigDecimal netToDecimal(String text) {
        double d = netToDouble(text);
        try {
            return BigDecimal.valueOf(d);
        } catch (NumberFormatException e) {
            return BigDecimal.ZERO;
        }
    }

    /** value.ToString() of a number as .NET prints it (int, double). */
    static String netString(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).stripTrailingZeros().toPlainString();
        if (v instanceof Double || v instanceof Float) {
            double d = ((Number) v).doubleValue();
            if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
            return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
        }
        return String.valueOf(v);
    }

    private static LocalDate day(String value) {
        if (value == null || value.trim().isEmpty()) return LocalDate.now();
        try {
            String s = value.trim();
            return LocalDate.parse(s.substring(0, Math.min(10, s.length())));
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid date");
        }
    }

    /** yyyy-MM-dd of a database date / datetime; "" for null. */
    private static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

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
}
