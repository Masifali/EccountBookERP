package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.StoreReportsCRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management Reports (module 46), group C.
 *
 * <h3>458 "Store Issuance Report"</h3>
 * ScreenName {@code StoreIssuenceHistory} (spelling as in dbo.ScreenDefinition), TargetUrl
 * Architecture.WinApp.Inventory_Reports.IssuanceHistory, desktop form {@code IssuanceHistory.cs}
 * (base.Name "IssuanceHistory"). No DocumentTypeId of its own — the register lists every store
 * issuance document type (the BLL never sends @DocumentTypeId). Route /store/reports/store-issuance-history.
 * BLL 0252 InvGsStoreIssuanceHeader (GetDataForDropDownFromInvGsStoreIssuanceHeader,
 * StoreIssuanceHistory), BLL 0141 VoucherReports.VoucherValidationReport (via
 * CommonServices.VoucherReport_118).
 *
 * <h3>460 "General Gate Pass  Report"</h3>
 * ScreenName {@code frmGatePassGeneral}, TargetUrl Architecture.WinApp.Inventory_Reports.frmGatePassGeneral,
 * desktop form {@code frmGatePassGeneral.cs}. Despite the name it is a pure register (no save,
 * update or delete anywhere in the form) over GatePassGeneral, DocumentTypeId 52 (GatePass General
 * Inward) or 92 (GatePass General Outward) chosen in a combo. Route /store/reports/general-gate-pass.
 * BLL 0123 GatePassGeneralReports.GatePassGeneralRegister, BLL 0133
 * GatePassInwardReports.GatePassGeneralInward254, BLL 0124 GatePassOutwardReports.GatePassOutwardSlipandRegister.
 *
 * <h3>DESKTOP BEHAVIOUR REPRODUCED (not corrected)</h3>
 * <ol>
 *   <li>458 Reset (IssuanceHistory.cs:200) clears Warehouse, Asset, Item, Department and Account
 *       but NOT Item Condition; it resets From to the financial year start and To to today.</li>
 *   <li>458 grid Qty column is typed int (IssuanceHistory.cs:272): the decimal IssueQty is stored
 *       through Convert.ToInt32(decimal) — rounded half-to-even — and the Qty total sums those
 *       rounded values. The page does the same.</li>
 *   <li>458 an empty search clears the grid and says "Record Not found For Display" (:296).</li>
 *   <li>458 the Print dropdown and the row Print/Voucher buttons do not check any right; neither
 *       form reads ScreenRights at all. The API therefore does not gate them either (the page is
 *       reached through the menu, which is where the desktop's gate is).</li>
 *   <li>460 Form_Load fills the Date Type combo (row 2 = "This Week", after the blank ZeroIndex
 *       row) BEFORE it activates the Document Type row, so Date Type's ValueChanged runs the first
 *       search with DocumentTypeId 0 — the procedure's {@code g.DocumentTypeId = @DocumentTypeId}
 *       then returns nothing. Only afterwards is "GatePass General Inward" selected. The page does
 *       the same, so it opens empty until Show is pressed.</li>
 *   <li>460 New (reset(), :420) blanks Document Type and then re-fills Date Type, whose
 *       ValueChanged searches again with DocumentTypeId 0 — an empty grid.</li>
 *   <li>460 "This Day" / "This Week" / "Financial Year" move From only; To is left as it was.
 *       "This Month" takes the first day of the month of DateTime.UtcNow, not local now.</li>
 *   <li>460 the Status combo ("Open"/"Accepted"/"Rejected") is sent as @Status, which the
 *       procedure compares with g.DocAttachment; Weight Status is sent as @WeighableStatus and
 *       compared with g.Status. Both are sent as the combo TEXT, and omitted when blank.</li>
 *   <li>460 a mouse click on Slip prints 254-RptInwardGatePassSlipGeneral from
 *       Sp_GatePassGeneralInward_rpt for either document type (Outward too).</li>
 *   <li>460 KNOWN DESKTOP DEFECT — Ctrl+Space on the Slip cell (DataGridHistory_KeyDown :639) prints
 *       290-InvRptOutwardGatePassSlip from Sp_GatePassOutward_SlipAndRegister_Rpt with the
 *       GatePassGeneral row's Id. That procedure filters dbo.GatePassOutward by Id (R.Id = @Id), i.e.
 *       the WRONG TABLE: it prints an unrelated outward gate pass that happens to share the Id, or
 *       "Record Not Found For Display". Reproduced as-is (not corrected); both Slip paths are kept.</li>
 *   <li>460 when opened by another form with FromDatePublic/ToDatePublic set, From is set to the
 *       financial year start (not FromDatePublic) and To to ToDatePublic, then Show runs.</li>
 *   <li>460 GpDate goes through ToShortDateString() into a DateTime column (time dropped);
 *       DeliveryOrderNo goes through Conversion.ToInt into an untyped column, so a missing one shows "0".</li>
 *   <li>460 Status "Record Not Found For DisPlay" (sic) for an empty 254 slip.</li>
 * </ol>
 *
 * <h3>DEVIATIONS (web-only)</h3>
 * <ol>
 *   <li>Dates and date-times in returned rows are rendered as ISO local strings here, so the page
 *       never depends on the JSON mapper's time zone.</li>
 *   <li>Binary columns (CompLogoImage) are dropped from the rows sent to the browser; the grid
 *       never shows them and they would be repeated on every row.</li>
 *   <li>460 Status / Weight Status / Document Type are accepted only as values of the desktop's
 *       own fixed lists (LimitToList on the desktop); anything else is treated as blank.</li>
 *   <li>Layout: both pages reproduce the designer geometry 1:1 in px (458 ClientSize 896 x 566,
 *       groupBox1 (3,2) 693 x 126; 460 ClientSize 851 x 561, groupBox1 (4,3) 662 x 131) with the
 *       history grid below the filters on the same screen (no tabs, as on the desktop) and a footer
 *       History button that scrolls to it. Native date inputs show the browser's date format, not
 *       the pickers' "dd-MMM-yy"; the grid's ctrlGrdBar (layouts / print / export) is shown disabled;
 *       the record navigator shows the row count.</li>
 *   <li>Doc No / GP No filters: the page applies Conversion.ToInt semantics (a value that does not
 *       fit an Int32 becomes 0 = no filter) before sending, so the API never gets an out-of-range int.</li>
 *   <li>Crystal layout: prints try the project's Crystal endpoint (/api/reports/{key}/print.pdf)
 *       and fall back to printing the procedure's rows. The 458 Print dropdown lists .rpt files
 *       from the desktop's local Store folder (CommonServices.DynamicReportsLoad), which the web
 *       server cannot enumerate: it prints the last search's raw rows instead.</li>
 * </ol>
 *
 * NOT PORTED: saved grid layouts (ctrlGrdBar / GetGridLayout), the HistoryStack bookkeeping on
 * close, Ctrl+E / Escape closing the form (the page has an Exit link instead). The ShortCut Keys
 * button / Ctrl+Alt show the same key list the desktop's ShortCutKeyPopUp shows.
 *
 * SOURCE: ported from recovered_source\ECCOUNTBOOKERP\Architecture.WinApp.Inventory_Reports
 * (IssuanceHistory.cs 89,287 bytes; frmGatePassGeneral.cs 71,622 bytes — the same sizes on the
 * user's device on 2026-09-25). The newer recovery\resolved-source folder no longer exists on the
 * device, so no diff against it was possible; see the review report.
 */
@Service
public class StoreReportsCService {

    private static final Logger LOG = LoggerFactory.getLogger(StoreReportsCService.class);

    public static final String SCREEN_458 = "StoreIssuenceHistory";
    public static final String SCREEN_460 = "frmGatePassGeneral";

    private final StoreReportsCRepository repo;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public StoreReportsCService(StoreReportsCRepository repo, CurrentUserContext ctx, StoreScreenRights rights) {
        this.repo = repo;
        this.ctx = ctx;
        this.rights = rights;
    }

    // ================================================================= 458 Store Issuance Report

    /** Activity → the combo it fills (IssuanceHistory.cs:131). */
    private static final List<String> ISSUANCE_ACTIVITIES = Arrays.asList(
            "DepartmentFrom", "Item", "Asset", "Warehouse", "DebitAccount", "ItemCondition");

    /**
     * frmGatePassReport_Load (IssuanceHistory.cs:222) + FillAllDropDowns (:114). Only activities
     * that came back are returned: a group missing from the result leaves its combo as it was
     * (the foreach only binds the groups present), and an empty result binds nothing at all (:127).
     */
    public Map<String, Object> issuanceLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("combos", issuanceCombos(u));
        out.put("financialYearStart", financialYearStart(u));
        out.put("rights", rights.of(SCREEN_458));
        return out;
    }

    /** btnRefresh_Click (:396) → FillAllDropDowns only. */
    public Map<String, Object> issuanceRefresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("combos", issuanceCombos(u));
        return out;
    }

    private Map<String, List<Map<String, Object>>> issuanceCombos(UserAccount u) {
        Map<String, List<Map<String, Object>>> combos = new LinkedHashMap<>();
        for (Map<String, Object> r : repo.issuanceDropDownSource(u)) {
            String activity = str(ci(r, "Activity"));
            if (!ISSUANCE_ACTIVITIES.contains(activity)) continue;          // :161 TryGetValue
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));                                  // :170 filtered.Rows.Add(Id, ReferenceName)
            o.put("Name", str(ci(r, "ReferenceName")));
            combos.computeIfAbsent(activity, k -> new ArrayList<>()).add(o);
        }
        return combos;
    }

    /** GridFill (IssuanceHistory.cs:238). */
    public List<Map<String, Object>> issuanceSearch(String fromDate, String toDate, int departmentId, int itemId,
                                                    int assetId, int warehouseId, int accountId,
                                                    int itemConditionId, int fromDocNo, int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = repo.issuanceHistory(u, 0,
                departmentId,                                                  // :250 RefDocumentTypeId ← CmbDepartmentName → @DepartmentId
                itemId, assetId, day(fromDate), day(toDate), warehouseId,
                fromDocNo, toDocNo, accountId, itemConditionId);
        return clean(rows);
    }

    /** grdfrm_ColumnButtonClick "Print" (:426) — StoreIssuanceHistory with @Id only. */
    public List<Map<String, Object>> issuanceSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        /* A real grid row always carries its Id; 0 would drop @Id and list the whole company. */
        if (id <= 0) throw new IllegalArgumentException("Record Not Found For Display");
        List<Map<String, Object>> rows = repo.issuanceHistory(u, id, 0, 0, 0, null, null, 0, 0, 0, 0, 0);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");   // :436
        return clean(rows);
    }

    /** grdfrm_ColumnButtonClick "Voucher" (:420) → CommonServices.VoucherReport_118 (:5647). */
    public List<Map<String, Object>> voucher118(int voucherHeadId, int documentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        if (voucherHeadId == 0) throw new IllegalArgumentException("VoucherId Not Found");            // :5656
        if (voucherHeadId < 0) {
            throw new IllegalArgumentException("Record Not Found For Display because VoucherHeadId not found"); // :5684
        }
        List<Map<String, Object>> rows = repo.voucher118(u, voucherHeadId, documentTypeId);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");       // :5679
        return clean(rows);
    }

    // =============================================================== 460 General Gate Pass Report

    /** StatusFill (frmGatePassGeneral.cs:183). */
    private static final String[][] STATUSES = { { "1", "Open" }, { "2", "Accepted" }, { "3", "Rejected" } };
    /** WeightStatus (:165). */
    private static final String[][] WEIGHT_STATUSES = { { "1", "Weighable" }, { "2", "Non-Weighable" } };
    /** DocumentTypeFill (:202). */
    private static final String[][] DOCUMENT_TYPES = { { "52", "GatePass General Inward" }, { "92", "GatePass General Outward" } };
    /** CommonServices.DateType (CommonServices.cs:15850), bound with ZeroIndex true (:155). */
    private static final String[][] DATE_TYPES = { { "1", "This Day" }, { "2", "This Week" }, { "3", "This Month" },
            { "4", "This Year" }, { "5", "Financial Year" } };

    /** frmGPOutward_Load (:116) — the four combos as the form builds them, plus ActiveYr.Start_Period. */
    public Map<String, Object> gatePassLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("statuses", list(STATUSES, "Status"));
        out.put("weightStatuses", list(WEIGHT_STATUSES, "WeightStatus"));
        out.put("documentTypes", list(DOCUMENT_TYPES, "DocumentType"));
        out.put("dateTypes", list(DATE_TYPES, "Parameters"));
        out.put("financialYearStart", financialYearStart(u));
        out.put("rights", rights.of(SCREEN_460));
        return out;
    }

    /** gridHisory (:220). */
    public List<Map<String, Object>> gatePassSearch(String fromDate, String toDate, int gpNoFrom, int gpNoTo,
                                                    String status, int documentTypeId, String weightStatus,
                                                    boolean onlyPending) {
        UserAccount u = ctx.requireAccountingUser();
        String st = inList(status, STATUSES);                                  // :233 Status ← cmbStatus.Text
        String ws = inList(weightStatus, WEIGHT_STATUSES);                     // :235 ReqType ← cmbWeightStatus.Text
        int dt = 0;                                                            // :234 DocumentTypeId ← cmbDocumentType.Value
        for (String[] d : DOCUMENT_TYPES) if (Integer.parseInt(d[0]) == documentTypeId) dt = documentTypeId;
        List<Map<String, Object>> rows = repo.gatePassGeneralRegister(u, dt, day(fromDate), day(toDate),
                ws, st, gpNoFrom, gpNoTo, onlyPending ? 1 : 0);               // :236 PendingForView
        return clean(rows);
    }

    /** DataGridHistory_ColumnButtonClick (:359) — 254-RptInwardGatePassSlipGeneral. */
    public List<Map<String, Object>> gatePassSlip254(int id, int documentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = repo.gatePassGeneralInward254(u, id, documentTypeId);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For DisPlay");     // :381
        return clean(rows);
    }

    /** DataGridHistory_KeyDown Ctrl+Space on "Slip" (:639) — 290-InvRptOutwardGatePassSlip. */
    public List<Map<String, Object>> gatePassSlip290(int id) {
        UserAccount u = ctx.requireAccountingUser();
        /* Id 0 would drop @Id (BLL guard) and list every outward gate pass. */
        if (id <= 0) throw new IllegalArgumentException("Record Not Found For Display");
        List<Map<String, Object>> rows = repo.gatePassOutwardSlip290(u, id);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");     // :649
        return clean(rows);
    }

    // ==================================================================================== helpers

    private static List<Map<String, Object>> list(String[][] src, String textKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] s : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", Integer.parseInt(s[0]));
            m.put(textKey, s[1]);
            out.add(m);
        }
        return out;
    }

    /** The text if it is one of the list's own entries, else "" (LimitToList). */
    private static String inList(String text, String[][] src) {
        if (text == null) return "";
        for (String[] s : src) if (s[1].equals(text)) return s[1];
        return "";
    }

    /** The procedures' @FromDate/@ToDate are {@code date}: the picker's time of day is dropped by SQL anyway. */
    private static Date day(String yyyyMMdd) {
        if (yyyyMMdd == null || yyyyMMdd.trim().length() < 10) return null;
        return Date.valueOf(LocalDate.parse(yyyyMMdd.trim().substring(0, 10)));
    }

    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    /** Deviation 1 and 2: ISO local date strings, binary columns dropped; column order kept. */
    private static List<Map<String, Object>> clean(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;
                if (v instanceof java.sql.Timestamp) v = ((java.sql.Timestamp) v).toLocalDateTime().format(ISO);
                else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
                else if (v instanceof java.time.LocalDateTime) v = ((java.time.LocalDateTime) v).format(ISO);
                else if (v instanceof java.time.LocalDate) v = v.toString();
                else if (v instanceof java.time.OffsetDateTime) v = ((java.time.OffsetDateTime) v).toLocalDateTime().format(ISO);
                else if (v instanceof java.util.Date) v = new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().format(ISO);
                o.put(e.getKey(), v);
            }
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period — the session's financial year among the active ones. */
    private String financialYearStart(UserAccount u) {
        try {
            int yearId = ctx.currentFinancialYearId();
            for (Map<String, Object> r : repo.activeFinancialYears(u)) {
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
}
