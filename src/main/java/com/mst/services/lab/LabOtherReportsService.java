package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseReportsRepository;
import com.mst.repositories.lab.LabOtherReportsRepository;
import com.mst.repositories.lab.LabOtherReportsRepository.Table;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
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
 * Lab Report (module 1011) - two report screens, two desktop forms.
 *
 *   627 "Lab Purchase Analysis Report (Not Use)"  ScreenName InvLabPurchaseReport -> Architecture.WinApp.Lab.InvLabPurchaseReport ("P:nnn")
 *   628 "Lab Sale Analysis Report"                ScreenName InvLabSaleRegister   -> Architecture.WinApp.Lab.InvLabSaleRegister   ("S:nnn")
 *
 * Both are read-only (no Save / Update / Delete on the desktop, none here).
 *
 * RIGHTS. Neither form reads a right (no SetRightsValueInRightsObject call, no Tag read, nothing hidden);
 * the desktop gate is the menu, i.e. the screen's View grant. Every call here requires the View grant of the
 * screen's own ScreenName (or the Admin role), as the other Lab Report ports do.
 *
 * Tenancy (organization / company / financial year) always comes from the session.
 */
@Service
public class LabOtherReportsService {

    public static final int PURCHASE_REPORT_SCREEN = 627, SALE_REGISTER_SCREEN = 628;
    public static final String PURCHASE_REPORT_NAME = "InvLabPurchaseReport";
    public static final String SALE_REGISTER_NAME = "InvLabSaleRegister";

    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");
    /** Conversion.ToDateTime's "no date" (0005_Architecture.Common.Conversion.cs:196). */
    private static final LocalDate NO_DATE = LocalDate.of(1900, 1, 1);

    private final LabOtherReportsRepository repo;
    private final PurchaseReportsRepository financialYears;
    private final CurrentUserContext context;
    private final StoreScreenRights rights;
    private final DesktopAttachmentStore attachments;

    public LabOtherReportsService(LabOtherReportsRepository repo, PurchaseReportsRepository financialYears,
                                  CurrentUserContext context, StoreScreenRights rights, DesktopAttachmentStore attachments) {
        this.repo = repo; this.financialYears = financialYears; this.context = context; this.rights = rights;
        this.attachments = attachments;
    }

    /** The screen's own View grant (tblUserRights by ScreenName), or the Admin role. */
    private UserAccount requireView(String screenName) {
        UserAccount u = context.requireAccountingUser();
        if (rights.has(screenName, "view") || "Admin".equals(context.currentRoleName())) return u;
        throw new AccessDeniedException("You do not have the View right for this screen.");
    }

    // =====================================================================================
    // 627 InvLabPurchaseReport - "Purchase Analysis Report"
    // =====================================================================================

    /**
     * InvLabPurchaseReport_Load P:396 - cmbsupplierfill (P:117), txtdatef = ActiveYr.Start_Period (P:402),
     * historygridfill. txtdatet keeps DateTime.Now ("now").
     */
    public Map<String, Object> purchaseInit() {
        UserAccount u = requireView(PURCHASE_REPORT_NAME);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", plain(repo.suppliers(context.currentOrganizationId(), context.currentCompanyId())));
        out.put("now", STAMP.format(LocalDateTime.now()));
        out.put("yearStart", financialYears.yearStart(u, context.currentFinancialYearId()));
        return out;
    }

    /**
     * The grid table of historygridfill (P:160-172): {row key, procedure column}. The desktop column names with a
     * space or "#" ("Analysis Item", "GatePass#", "Vehicle No", "Crop Year") are the page's captions; the keys are plain.
     */
    private static final String[][] PURCHASE_COLUMNS = {
            {"Id", "Id"}, {"DocumentNo", "DocNo"}, {"SupplierName", "SupplierName"}, {"AnalysisItem", "ItemName"},
            {"GatePassNo", "GpSrNo"}, {"BuiltyNo", "BiltyNo"}, {"VehicleNo", "VehicleNo"}, {"CropYear", "Crop"},
            {"Remarks", "RemarksHeader"}, {"AnalysisPicture", "Pic1Pathe"}, {"CookingPicture", "CookingPic"},
            {"AnalystName", "AnalystName"}};

    /**
     * historygridfill P:133 (btnsearch_Click P:362, btnAllRecord_Click P:439, Load). ReportsParameters as the form
     * fills it (P:146-153): FromDate / ToDate = the pickers, SupplierCustomerId = Conversion.ToInt(cmbsupplier.Value),
     * GpSrNoF / GpSrNoT = Conversion.ToInt(text). The BLL leaves a 0 out.
     */
    public Map<String, Object> purchaseRows(Map<String, Object> b) {
        requireView(PURCHASE_REPORT_NAME);
        Table t = repo.purchaseReport(context.currentOrganizationId(), context.currentCompanyId(),
                date(b.get("fromDate")), date(b.get("toDate")), nz(num(b.get("supplierId"))),
                nz(num(b.get("gpNoFrom"))), nz(num(b.get("gpNoTo"))));
        List<Map<String, Object>> rows = new ArrayList<>();
        if (!t.rows.isEmpty()) {
            int[] at = new int[PURCHASE_COLUMNS.length];
            for (int c = 0; c < at.length; c++) at[c] = t.index(PURCHASE_COLUMNS[c][1]);
            for (Object[] r : t.rows) {                                                   // P:176-179 - string columns
                Map<String, Object> g = new LinkedHashMap<>();
                for (int c = 0; c < at.length; c++) g.put(PURCHASE_COLUMNS[c][0], text(r[at[c]]));
                rows.add(g);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    /** The header row of a record of the signed-in company, or null. */
    private List<Map<String, Object>> ownedPurchase(int id) {
        if (id <= 0) return new ArrayList<>();
        List<Map<String, Object>> dt = repo.purchaseById(id);
        if (dt.isEmpty()) return dt;
        Map<String, Object> h = dt.get(0);
        if (num(ci(h, "OrganizationId")) != context.currentOrganizationId() || num(ci(h, "CompanyId")) != context.currentCompanyId())
            return new ArrayList<>();
        return dt;
    }

    /**
     * grdhistory_ColumnButtonClick, Column "Detail" (P:259-340): GetById(RecId); when it has rows the detail grid
     * is "Perameter Description" (sic) / "Min Value" / "Max Value" / "Analysis Result" / "Remarks Detail" from
     * AnalysisParameterDescription, MinValue, MaxValue, InAnalysisResult, RemarksDetail of every row, and the two
     * pictures come from Rows[0] Pic1Pathe / CookingPic; with no rows the detail grid is cleared.
     *
     * The form expects ONE ROW PER ANALYSIS PARAMETER from 'ReadById'. The procedure in procdure.utf8.sql now
     * returns the header only (one row, no AnalysisParameterDescription / MinValue / MaxValue / InAnalysisResult /
     * RemarksDetail), so the desktop button ends in "Column 'AnalysisParameterDescription' does not belong to
     * table ." - the form is the "(Not Use)" one. When 'ReadById' still has those columns its rows are used exactly
     * as the form does; otherwise the same five values are read from 'ReadDetailByHeaderId' of the same procedure.
     */
    public Map<String, Object> purchaseDetail(int id) {
        requireView(PURCHASE_REPORT_NAME);
        List<Map<String, Object>> dt = ownedPurchase(id);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        out.put("found", !dt.isEmpty());
        out.put("rows", rows);
        if (dt.isEmpty()) return out;                                                     // griddetail.ClearStructure() P:336
        List<Map<String, Object>> source = has(dt.get(0), "AnalysisParameterDescription") ? dt : repo.purchaseDetailByHeaderId(id);
        for (Map<String, Object> r : source) {                                            // P:287-290 - string columns
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("ParameterDescription", text(ci(r, "AnalysisParameterDescription")));
            g.put("MinValue", text(ci(r, "MinValue")));
            g.put("MaxValue", text(ci(r, "MaxValue")));
            g.put("AnalysisResult", text(ci(r, "InAnalysisResult")));
            g.put("RemarksDetail", text(ci(r, "RemarksDetail")));
            rows.add(g);
        }
        out.put("analysisPic", !text(ci(dt.get(0), "Pic1Pathe")).trim().isEmpty());       // P:291
        out.put("cookingPic", !text(ci(dt.get(0), "CookingPic")).trim().isEmpty());       // P:292
        return out;
    }

    /**
     * sampleanalysispicture / cookingpic (P:291-329): Image.FromFile(Pic1Pathe | CookingPic) when the file exists,
     * otherwise no picture. {name, bytes} or null. which = "analysis" | "cooking".
     */
    public Object[] purchasePicture(int id, String which) {
        UserAccount u = requireView(PURCHASE_REPORT_NAME);
        List<Map<String, Object>> dt = ownedPurchase(id);
        if (dt.isEmpty()) return null;
        String name = text(ci(dt.get(0), "cooking".equals(which) ? "CookingPic" : "Pic1Pathe")).trim();
        if (name.isEmpty()) return null;
        byte[] bytes;
        try { bytes = attachments.read(u, name); } catch (RuntimeException e) { return null; }   // !File.Exists -> no picture
        return bytes == null || bytes.length == 0 ? null : new Object[] { name, bytes };
    }

    // =====================================================================================
    // 628 InvLabSaleRegister - "Sale Analysis Register"
    // =====================================================================================

    /** StatusBind S:231 - the fixed two rows the form itself builds. */
    private static List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        out.add(row("Id", 0, "Status", "Rejected"));
        out.add(row("Id", 1, "Status", "Accepted"));
        return out;
    }

    /**
     * VoucherValidation_Load S:112 / reset S:151 - ItemNameFill, ParentCategoryBind, StatusBind, PurchaseOrderBind,
     * cmbsupplierfill. "now" is what the two DateTimePickers hold before Load assigns
     * txtdatef = ActiveYr.Start_Period (S:121, AFTER the first BindGrid).
     */
    public Map<String, Object> saleInit() {
        UserAccount u = requireView(SALE_REGISTER_NAME);
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", plain(repo.saleItems(org, company)));                            // S:166
        out.put("parentCategories", plain(repo.saleParentCategories(org, company)));      // S:198
        out.put("statuses", statuses());                                                  // S:231
        out.put("orders", plain(repo.saleOrders(org, company)));                          // S:215
        out.put("customers", plain(repo.saleCustomers(org, company)));                    // S:182
        out.put("now", STAMP.format(LocalDateTime.now()));
        out.put("yearStart", financialYears.yearStart(u, context.currentFinancialYearId()));
        return out;
    }

    /** dtG's fixed columns (BindGrid S:289-299): {grid column, procedure column}. */
    private static final String[][] SALE_FIXED = {
            {"Id", "Id"}, {"DocDate", "DocDate"}, {"DocNo", "DocNo"}, {"ItemId", "ItemId"}, {"ItemName", "ItemName"},
            {"GatePassOutwardId", "GatePassOutwardId"}, {"GpSrNo", "GpSrNo"}, {"PartyName", "PartyName"},
            {"OrderId", "SaleOrderId"}, {"OrderNo", "OrderNo"}, {"Status", "Status"}};

    /**
     * BindGrid S:256. Parameters as the form fills ReportsParameters (S:260-281) and the BLL guards them
     * (0405:317): dates always (a real DateTimePicker value), GpSrNoF / GpSrNoT / ItemId / SupplierCustomerId /
     * OrderId / InventoryParentCategories when != 0, @IsAccepted unless the Status combo's TEXT is empty ("All").
     *
     * The grid table dtG (S:285-330): eleven fixed columns, then one column per non-empty Parms_NN of the FIRST
     * row (result columns 35..), filled POSITIONALLY from result column 23 + j (Value_01..).
     *
     * OrderNo: the form reads dtGroupAnalysis.Rows[i]["OrderNo"] (S:322), but USP_LabSaleAnalysis_Register in
     * procdure.utf8.sql has that column commented out ("--gp.SupplierContractCode OrderNo"), so the desktop
     * shows "Column 'OrderNo' does not belong to table ." and an empty grid whenever the procedure returns rows.
     * Here the cell is empty when the column is absent ("orderNoMissing" tells the page) and filled when a
     * database's procedure does return it.
     */
    public Map<String, Object> saleRows(Map<String, Object> b) {
        requireView(SALE_REGISTER_NAME);
        String status = str(b.get("status")).trim();
        Boolean isAccepted = status.isEmpty() ? null : Boolean.valueOf("1".equals(status));   // Conversion.ToBool("0"/"1") S:276
        Table t = repo.saleRegister(context.currentOrganizationId(), context.currentCompanyId(),
                date(b.get("fromDate")), date(b.get("toDate")), nz(num(b.get("gpNoFrom"))), nz(num(b.get("gpNoTo"))),
                nz(num(b.get("itemId"))), nz(num(b.get("customerId"))), nz(num(b.get("orderId"))), isAccepted,
                nz(num(b.get("parentCategoryId"))));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", t.rows.size());                  // dtGroupAnalysis.Rows.Count - what 662-Print checks (S:131)
        List<Map<String, Object>> columns = new ArrayList<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        List<Map<String, Object>> raw = new ArrayList<>();
        boolean orderNoMissing = false;
        if (!t.rows.isEmpty()) {
            Set<String> names = new HashSet<>();
            for (String[] c : SALE_FIXED) names.add(c[0].toLowerCase(Locale.ROOT));
            Object[] first = t.rows.get(0);
            for (int i = 0; i < t.columns.size() - 35; i++) {                             // S:287, S:300-306
                String name = text(first[i + 35]);
                if (name.isEmpty()) continue;
                // DataColumnCollection.Add of a name already present
                if (!names.add(name.toLowerCase(Locale.ROOT)))
                    throw new IllegalArgumentException("A column named '" + name + "' already belongs to this DataTable.");
                columns.add(row("key", "v" + columns.size(), "caption", name));
            }
            int[] at = new int[SALE_FIXED.length];
            for (int c = 0; c < at.length; c++) {
                if ("OrderNo".equals(SALE_FIXED[c][0])) { at[c] = t.find("OrderNo"); orderNoMissing = at[c] < 0; }
                else at[c] = t.index(SALE_FIXED[c][1]);
            }
            for (Object[] r : t.rows) {                                                   // S:310-329
                Map<String, Object> g = new LinkedHashMap<>();
                for (int c = 0; c < at.length; c++) {
                    Object v = at[c] < 0 ? null : r[at[c]];
                    // S:314 Conversion.ToDateTime(DocDate).ToShortDateString() - formatted by the page
                    g.put(SALE_FIXED[c][0], "DocDate".equals(SALE_FIXED[c][0]) ? isoDate(v) : text(v));
                }
                for (int j = 0; j < columns.size(); j++) {                                // S:324-327
                    if (23 + j >= r.length) throw new IllegalArgumentException("Cannot find column " + (23 + j) + ".");
                    g.put("v" + j, text(r[23 + j]));
                }
                rows.add(g);
                // dtGroupAnalysis itself - what print_Click_1 hands to 662-LabSaleAnalysisRegitser.rpt (S:124-141).
                // Binary cells (CompLogoImage) are not carried to the page; the print layer adds the logo itself.
                Map<String, Object> full = new LinkedHashMap<>();
                for (int c = 0; c < t.columns.size(); c++) {
                    Object v = r[c];
                    if (v instanceof byte[]) continue;
                    full.put(t.columns.get(c), v instanceof java.util.Date || v instanceof LocalDateTime || v instanceof LocalDate ? isoDate(v) : v);
                }
                raw.add(full);
            }
        }
        out.put("columns", columns);
        out.put("rows", rows);
        out.put("raw", raw);
        out.put("orderNoMissing", orderNoMissing);
        return out;
    }

    // =====================================================================================
    // helpers
    // =====================================================================================

    private static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** DataRow["name"] - case-insensitive. */
    private static Object ci(Map<String, Object> r, String name) {
        if (r.containsKey(name)) return r.get(name);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean has(Map<String, Object> r, String name) {
        for (String k : r.keySet()) if (k.equalsIgnoreCase(name)) return true;
        return false;
    }

    /** Lookup rows for JSON: insertion-ordered copies with dates as ISO text, no binary cells. */
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
