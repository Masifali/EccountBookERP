package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseReportsRepository;
import com.mst.repositories.lab.LabRegisterReportsRepository;
import com.mst.repositories.lab.LabRegisterReportsRepository.Table;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * Lab Report (module 1011) - three report screens, two desktop forms.
 *
 *   629 "Lab Sample Analysis Report"   ScreenName InvLabSampleRegister          -> Architecture.WinApp.Lab.InvLabSampleRegister
 *   632 "Sample Analysis Register"     ScreenName SampleAnalysisRegister        -> the SAME form (seed_screendef.txt:583 TargetUrl)
 *   631 "In-Process Analysis Report"   ScreenName InProcessLabAnalysisRegister  -> Architecture.WinApp.Lab.InProcessLabAnalysisRegister
 *
 * ":NNN" = line in the form's .cs (InvLabSampleRegister.cs or InProcessLabAnalysisRegister.cs).
 *
 * RIGHTS. Neither form reads a right: there is no SetRightsValueInRightsObject call, no Tag read and no
 * button is hidden (InvLabSampleRegister.cs:93-112, InProcessLabAnalysisRegister.cs:89-115). The desktop
 * gate is the menu: CommonServices.OpenDynamicallyScreen (CommonServices.cs:348) instantiates the
 * TargetUrl for a ScreenDefinition row the user can see and only stores the ScreenName in form.Tag.
 * So 629 and 632 differ in nothing but the rights row they are opened through; the web requires the
 * View grant of the screen's own ScreenName (or the Admin role) on every call and hides nothing else.
 *
 * Tenancy (organization / company / financial year) always comes from the session.
 */
@Service
public class LabRegisterReportsService {

    /** ScreenDefinition.ScreenName by screen id (seed_screendef.txt:580, :583, :582). */
    public static final int SAMPLE_REPORT_SCREEN = 629, SAMPLE_REGISTER_SCREEN = 632, IN_PROCESS_SCREEN = 631;
    public static final String SAMPLE_REPORT_NAME = "InvLabSampleRegister";
    public static final String SAMPLE_REGISTER_NAME = "SampleAnalysisRegister";
    public static final String IN_PROCESS_NAME = "InProcessLabAnalysisRegister";

    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");
    /** DateTime.ToShortTimeString() - "HH:mm", the en-GB pattern that goes with the dd/MM/yyyy short date the other ported reports assume. */
    private static final DateTimeFormatter SHORT_TIME = DateTimeFormatter.ofPattern("HH:mm", Locale.ENGLISH);
    /** Conversion.ToDateTime's "no date" (0005_Architecture.Common.Conversion.cs:196). */
    private static final LocalDate NO_DATE = LocalDate.of(1900, 1, 1);

    private final LabRegisterReportsRepository repo;
    private final PurchaseReportsRepository financialYears;
    private final CurrentUserContext context;
    private final StoreScreenRights rights;

    public LabRegisterReportsService(LabRegisterReportsRepository repo, PurchaseReportsRepository financialYears,
                                     CurrentUserContext context, StoreScreenRights rights) {
        this.repo = repo; this.financialYears = financialYears; this.context = context; this.rights = rights;
    }

    /** The screen's own View grant (tblUserRights by ScreenName), or the Admin role. */
    private void requireView(String screenName) {
        context.requireAccountingUser();
        if (rights.has(screenName, "view") || "Admin".equals(context.currentRoleName())) return;
        throw new AccessDeniedException("You do not have the View right for this screen.");
    }

    /** 629 or 632 - the only two rights rows the sample register form is opened through. */
    private static String sampleScreenName(int screen) {
        if (screen == SAMPLE_REPORT_SCREEN) return SAMPLE_REPORT_NAME;
        if (screen == SAMPLE_REGISTER_SCREEN) return SAMPLE_REGISTER_NAME;
        throw new IllegalArgumentException("Unknown screen " + screen);
    }

    // =====================================================================================
    // 629 / 632 InvLabSampleRegister
    // =====================================================================================

    /** StatusBind :214 - the fixed two rows. */
    private static List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        out.add(row("Id", 0, "Status", "Rejected"));
        out.add(row("Id", 1, "Status", "Accepted"));
        return out;
    }

    /**
     * VoucherValidation_Load :103 / reset :142 - ItemNameFill, ParentCategoryBind, StatusBind, cmbsupplierfill
     * (PurchaseOrderBind :210 is empty). "now" is what the two DateTimePickers hold before Load assigns
     * txtdatef = clsGlobalVariables.ActiveYr.Start_Period (:112, AFTER the first BindGrid).
     */
    public Map<String, Object> sampleInit(int screen) {
        requireView(sampleScreenName(screen));
        UserAccount u = context.requireAccountingUser();
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", plain(repo.sampleItems(org, company)));                         // :152
        out.put("parentCategories", plain(repo.sampleParentCategories(org, company)));   // :195
        out.put("statuses", statuses());                                                 // :214
        out.put("suppliers", plain(repo.sampleSuppliers(org, company)));                 // :180
        out.put("now", STAMP.format(LocalDateTime.now()));
        out.put("yearStart", financialYears.yearStart(u, context.currentFinancialYearId()));
        return out;
    }

    /** dtG's fixed columns, in order (BindGrid :266-275). */
    private static final String[] SAMPLE_FIXED = { "Id", "DocDate", "DocNo", "ItemId", "ItemName", "PartyName", "PartyLotRefNo", "ItemQty", "OrderNo", "Status" };

    /**
     * BindGrid :239. Parameters as the form fills ReportsParameters (:246-260) and the BLL guards them
     * (0407:685): dates always (a real DateTimePicker value), ItemId / SupplierCustomerId /
     * InventoryParentCategories when != 0, @IsAccepted unless the Status combo's TEXT is empty ("All").
     *
     * The grid table dtG (:264-301): ten fixed columns, then one column per non-empty Parms_NN of the
     * FIRST row (result columns 41..), filled POSITIONALLY from result column 26 + j (Value_01..).
     */
    public Map<String, Object> sampleRows(Map<String, Object> b) {
        requireView(sampleScreenName(num(b.get("screen"))));
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        String status = str(b.get("status")).trim();
        Boolean isAccepted = status.isEmpty() ? null : Boolean.valueOf("1".equals(status));   // Conversion.ToBool("0"/"1") (:257)
        Table t = repo.sampleRegister(org, company, date(b.get("fromDate")), date(b.get("toDate")),
                nz(num(b.get("itemId"))), nz(num(b.get("supplierId"))), isAccepted, nz(num(b.get("parentCategoryId"))));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", t.rows.size());                  // dtGroupAnalysis.Rows.Count - what 664-Print checks (:121)
        List<Map<String, Object>> columns = new ArrayList<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        if (!t.rows.isEmpty()) {
            Set<String> names = fixedNames(SAMPLE_FIXED);
            Object[] first = t.rows.get(0);
            for (int i = 0; i < t.columns.size() - 41; i++) {                             // :263, :276-282
                String name = text(first[i + 41]);
                if (name.isEmpty()) continue;
                addColumn(names, name);
                columns.add(row("key", "v" + columns.size(), "caption", name));
            }
            int id = t.index("Id"), docDate = t.index("DocDate"), docNo = t.index("DocNo"), itemId = t.index("ItemId"),
                itemName = t.index("ItemName"), partyName = t.index("PartyName"), lot = t.index("PartyLotRefNo"),
                qty = t.index("ItemQty"), orderNo = t.index("OrderNo"), st = t.index("Status");
            for (Object[] r : t.rows) {                                                   // :285-302
                Map<String, Object> g = new LinkedHashMap<>();
                g.put("Id", text(r[id]));
                g.put("DocDate", isoDate(r[docDate]));        // ToShortDateString (:289) - formatted by the page
                g.put("DocNo", text(r[docNo]));
                g.put("ItemId", text(r[itemId]));
                g.put("ItemName", text(r[itemName]));
                g.put("PartyName", text(r[partyName]));
                g.put("PartyLotRefNo", text(r[lot]));
                g.put("ItemQty", text(r[qty]));
                g.put("OrderNo", text(r[orderNo]));
                g.put("Status", text(r[st]));
                for (int j = 0; j < columns.size(); j++) g.put("v" + j, text(r[26 + j]));  // :299
                rows.add(g);
            }
        }
        out.put("columns", columns);
        out.put("rows", rows);
        return out;
    }

    // =====================================================================================
    // 631 InProcessLabAnalysisRegister
    // =====================================================================================

    /** HistoryComboDbCall :118 - DocumentTypeIds = "306" on every call. */
    private static final String IN_PROCESS_DOCUMENT_TYPES = "306";

    /** VoucherValidation_Load :101 - PlantComboFill(HistoryComboDbCall("Plant")); the other combos stay empty. */
    public Map<String, Object> inProcessInit() {
        requireView(IN_PROCESS_NAME);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("plants", plain(repo.inProcessDropDown(context.currentOrganizationId(), context.currentCompanyId(),
                context.currentFinancialYearId(), "Plant", IN_PROCESS_DOCUMENT_TYPES)));
        return out;
    }

    /**
     * cmbPlantName_Leave :207 - OtherComboFill(HistoryComboDbCall(null, PlantId)). HistoryComboDbCall
     * (:118-135) never copies its PlantId argument into ReportsParameters and the BLL has no @PlantId, so
     * the call is the unfiltered one without @Activity; OtherComboFill (:150) keeps the rows whose
     * Activity is "Item" and "JobOrder".
     */
    public Map<String, Object> inProcessLookups() {
        requireView(IN_PROCESS_NAME);
        List<Map<String, Object>> all = repo.inProcessDropDown(context.currentOrganizationId(), context.currentCompanyId(),
                context.currentFinancialYearId(), null, IN_PROCESS_DOCUMENT_TYPES);
        List<Map<String, Object>> items = new ArrayList<>(), jobOrders = new ArrayList<>();
        for (Map<String, Object> r : all) {
            String activity = text(r.get("Activity"));
            Map<String, Object> o = row("Id", r.get("Id"), "name", text(r.get("ReferenceName")));
            if ("Item".equals(activity)) items.add(o);
            else if ("JobOrder".equals(activity)) jobOrders.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items);
        out.put("jobOrders", jobOrders);
        return out;
    }

    /** DateBindDbCall :177 - Id = row number (1-based), DocDate = ToShortDateString of each returned date. */
    public List<Map<String, Object>> inProcessDates(int plantId, int jobOrderId, int itemId) {
        requireView(IN_PROCESS_NAME);
        List<Map<String, Object>> rows = repo.inProcessDates(context.currentOrganizationId(), context.currentCompanyId(),
                nz(itemId), nz(plantId), nz(jobOrderId));
        List<Map<String, Object>> out = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) out.add(row("Id", i + 1, "DocDate", isoDate(rows.get(i).get("DocDate"))));
        return out;
    }

    private static final String[] STEP_FIXED = { "ScheduleId", "PlantId", "StepId", "ParameterId", "StepName", "Parameter" };
    private static final String[] RECOVERY_FIXED = { "ParameterId", "PlantName", "ItemName", "Parameter", "MinValue", "MaxValue" };

    /**
     * btnshow_Click :257. Validations in desktop order with the desktop text (:261-272), then both
     * procedures with the same parameters (:273-281): PlantId / JobOrderId / ItemId when != 0 and
     * DocDate = Conversion.ToDateTime(cmbDocDate.Text) unless that is the 1900 "no date" (0401:206, :237).
     *
     * Step grid (:284-322): six fixed columns, then one column per non-null dtm_NN of the FIRST row
     * (result columns 33..) captioned with its short time, filled positionally from column 21 + j.
     * Recovery grid (:329-368): six fixed columns, dtm_NN from column 30.., values from column 18 + j.
     */
    public Map<String, Object> inProcessRows(Map<String, Object> b) {
        requireView(IN_PROCESS_NAME);
        int plantId = num(b.get("plantId")), jobOrderId = num(b.get("jobOrderId")), itemId = num(b.get("itemId"));
        if (plantId == 0) throw new IllegalArgumentException("Plant Name Feild Required...");    // :263 (sic)
        if (itemId == 0) throw new IllegalArgumentException("Item Name Feild Required...");      // :269 (sic)
        Timestamp docDate = date(b.get("docDate"));
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        Table main = repo.inProcessStepRegister(org, company, nz(plantId), nz(jobOrderId), nz(itemId), docDate);
        Table recovery = repo.inProcessRecovery(org, company, nz(plantId), nz(jobOrderId), nz(itemId), docDate);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", main.rows.size());               // dtMainFromDb.Rows.Count - what 660-Print checks (:437)
        out.put("step", pivot(main, STEP_FIXED,
                new String[] { "ScheduleId", "PlantId", "LabInProcessAnalysisStepId", "AnalysisParameterId", "ProcessStepName", "AnalysisParameterDescription" }, 33, 21));
        out.put("recovery", pivot(recovery, RECOVERY_FIXED,
                new String[] { "AnalysisParameterId", "PlantName", "ItemName", "AnalysisParameterDescription", "MinValue", "MaxValue" }, 30, 18));
        return out;
    }

    /** The two DataTable builds of btnshow_Click: fixed columns by name, time columns from the first row, values by position. */
    private static Map<String, Object> pivot(Table t, String[] fixed, String[] source, int timeStart, int valueStart) {
        List<Map<String, Object>> columns = new ArrayList<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        if (!t.rows.isEmpty()) {
            Set<String> names = fixedNames(fixed);
            Object[] first = t.rows.get(0);
            for (int i = 0; i < t.columns.size() - timeStart; i++) {
                LocalDateTime time = dateTime(first[i + timeStart]);
                if (time == null || time.toLocalDate().equals(NO_DATE)) continue;         // Conversion.CheckDateTimeNull
                String name = SHORT_TIME.format(time);
                addColumn(names, name);
                columns.add(row("key", "v" + columns.size(), "caption", name));
            }
            int[] idx = new int[source.length];
            for (int k = 0; k < source.length; k++) idx[k] = t.index(source[k]);
            for (Object[] r : t.rows) {
                Map<String, Object> g = new LinkedHashMap<>();
                for (int k = 0; k < fixed.length; k++) g.put(fixed[k], text(r[idx[k]]));
                for (int j = 0; j < columns.size(); j++) g.put("v" + j, text(r[valueStart + j]));
                rows.add(g);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("columns", columns);
        out.put("rows", rows);
        return out;
    }

    // =====================================================================================
    // helpers
    // =====================================================================================

    private static Set<String> fixedNames(String[] fixed) {
        Set<String> names = new HashSet<>();
        for (String f : fixed) names.add(f.toLowerCase(Locale.ROOT));
        return names;
    }

    /**
     * DataColumnCollection.Add(name): a second column of the same name (names compare case-insensitively)
     * throws DuplicateNameException, which the form shows in its MessageBox (catch :323 / :398).
     */
    private static void addColumn(Set<String> names, String name) {
        if (!names.add(name.toLowerCase(Locale.ROOT)))
            throw new IllegalArgumentException("A column named '" + name + "' already belongs to this DataTable.");
    }

    private static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** Lookup rows for JSON: insertion-ordered copies with dates as ISO text. */
    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;
                o.put(e.getKey(), v instanceof java.util.Date || v instanceof LocalDateTime || v instanceof LocalDate ? isoDate(v) : v);
            }
            out.add(o);
        }
        return out;
    }

    /** What a string-typed DataColumn stores: value.ToString(), "" for DBNull. */
    private static String text(Object v) {
        if (v == null) return "";
        if (v instanceof Boolean) return ((Boolean) v) ? "True" : "False";
        if (v instanceof Double || v instanceof Float) {
            double d = ((Number) v).doubleValue();
            if (Double.isNaN(d) || Double.isInfinite(d)) return String.valueOf(d);
            return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
        }
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof byte[]) return "";
        if (v instanceof java.util.Date || v instanceof LocalDateTime || v instanceof LocalDate) return isoDate(v);
        return String.valueOf(v);
    }

    private static LocalDateTime dateTime(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        if (v instanceof LocalDateTime) return (LocalDateTime) v;
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay();
        try { return parse(String.valueOf(v)); } catch (RuntimeException e) { return null; }
    }

    private static String isoDate(Object v) {
        LocalDateTime d = dateTime(v);
        return d == null ? "" : STAMP.format(d);
    }

    private static LocalDateTime parse(String s) {
        s = s.trim().replace(' ', 'T');
        if (s.length() == 10) return LocalDate.parse(s).atStartOfDay();
        if (s.length() == 16) s = s + ":00";
        return LocalDateTime.parse(s.length() > 19 ? s.substring(0, 19) : s);
    }

    /**
     * A date sent by the page ("yyyy-MM-dd" or "yyyy-MM-ddTHH:mm:ss"). Null - the parameter is left out -
     * for an empty value and for the dates Conversion.CheckDateTimeNull treats as "no date" (1900-01-01, 0001-01-01).
     */
    private static Timestamp date(Object v) {
        String s = str(v).trim();
        if (s.isEmpty()) return null;
        LocalDateTime d;
        try { d = parse(s); } catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date '" + s + "'"); }
        if (d.toLocalDate().equals(NO_DATE) || d.getYear() == 1) return null;
        return Timestamp.valueOf(d);
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** Conversion.ToInt - 0 for anything that is not a number. */
    private static int num(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        try { return v == null ? 0 : Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** The BLL's "!= 0" guard: 0 is not sent. */
    private static Integer nz(int v) { return v == 0 ? null : v; }
}
