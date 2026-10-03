package com.mst.services.lab;

import com.mst.repositories.lab.ExportPreShipmentAnalysisRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 168 "Export Pre Shipment Analysis" (Lab, module 7) — desktop
 * Architecture.WinApp.Export/EximPreProductionLab.cs (ScreenName "EximPreProductionLab", form text
 * "FrmPreProductionLotInspection"). Line references (:n) are that file unless a BLL/DAL file is named.
 *
 * THIS DESKTOP FORM IS A LEGACY FORM THAT NO LONGER WORKS AGAINST ITS OWN PROCEDURES. It is ported as it
 * is; nothing was "repaired", because a repair would have to invent the values the form never supplies:
 *
 *  1. SAVE / UPDATE CAN NEVER SUCCEED. Insert() (:167-201) fills only Id, InspectedByLabId, ReportDate,
 *     ReportRefNo, LotRefNo, ItemId, QtyKgs and InspectionRemarks. The model's RequestDate is a
 *     non-nullable DateTime (Model 0866) and stays DateTime.MinValue (0001-01-01). GenericProvider.SetProc
 *     (DAL 0207:300) adds it with AddWithValue, i.e. as SQL "datetime", and ADO.NET refuses to send it:
 *     "SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM." — before
 *     the procedure runs. DAL 0524 rolls back and the form shows that text (:197-200).
 *     Even past that, DAL 0524 SetData does "foreach (... in obj.LotInspectionDetails)" on a list the form
 *     never creates (null) -> NullReferenceException -> rollback. Both steps are reproduced in save().
 *     The form also never sets OrganizationId / CompanyId / FinancialYearId / BranchesId / EntryUserId /
 *     ScreenName, so a header written by it would belong to company 0.
 *  2. THE LAB COMBO ASKS FOR COMPANY 0. LabInspectionNameFill (:109-129) passes a new ExImLookUps with
 *     only ExImLookUptypesId = 3, so the procedure filters OrganizationId = 0 AND CompanyId = 0.
 *  3. THE GRID LISTS ONLY UNAPPROVED ROWS ENTERED BY USER 0. ReadAll (:252-280) passes a ReportsParameters
 *     with only OrganizationId and CompanyId; BLL 0472 Getall then sends @IsApproved = 0,
 *     @CanViewAllRecord = 0 and @EntryUser = 0, and the procedure requires "lab.EntryUserId = @EntryUser".
 *  4. ROW DOUBLE-CLICK READS THE WRONG CELL. grd_DoubleClick (:203-207) does
 *     Convert.ToInt32(grd.CurrentRow.Cells[0].Value); column 0 of today's result set is "ReportCriteria"
 *     (a text), so the desktop throws "Input string was not in a correct format." The web page opens the
 *     row by its Id column instead (the one deliberate deviation — a read, see byId()).
 *
 * RIGHTS. The form never calls CommonServices.SetRightsValueInRightsObject; the only gate is the menu.
 * The server enforces the View right of ScreenName "EximPreProductionLab" on every call.
 */
@Service
public class ExportPreShipmentAnalysisService {

    public static final int SCREEN_ID = 168;
    public static final String SCREEN_NAME = "EximPreProductionLab";

    /** System.Data.SqlTypes.SqlDateTime's own text (SqlTypeException). */
    static final String SQL_DATETIME_OVERFLOW =
            "SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.";
    /** Convert.ToDouble / Convert.ToInt32 on a text that is not a number (FormatException). */
    static final String FORMAT_EXCEPTION = "Input string was not in a correct format.";
    /** List<T>[0] on an empty list (ArgumentOutOfRangeException), as BLL 0472 GetByID rethrows it. */
    static final String INDEX_OUT_OF_RANGE =
            "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index";
    static final String NULL_REFERENCE = "Object reference not set to an instance of an object.";

    /** DateTime.MinValue — the value of a DateTime property nobody assigned. */
    private static final LocalDateTime CLR_MIN_DATE = LocalDateTime.of(1, 1, 1, 0, 0);
    private static final LocalDateTime SQL_MIN_DATE = LocalDateTime.of(1753, 1, 1, 0, 0);

    /** What Convert.ToDouble accepts from a text box: sign, digits with group separators, fraction, exponent. */
    private static final Pattern NUMBER = Pattern.compile("[+-]?(\\d[\\d,]*)?(\\.\\d*)?([eE][+-]?\\d+)?");

    private final ExportPreShipmentAnalysisRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public ExportPreShipmentAnalysisService(ExportPreShipmentAnalysisRepository repo, StoreScreenRights rights,
                                            CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    private Map<String, Boolean> requireView() {
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) {
            throw new AccessDeniedException("You do not have the View right for Export Pre Shipment Analysis.");
        }
        return r;
    }

    /** FrmPreProductionLotInspection_Load (:93-107): LabInspectionNameFill, ItemNameFill, ReadAll. */
    public Map<String, Object> init() {
        Map<String, Boolean> r = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("labs", labRows());
        out.put("items", itemRows());
        out.put("grid", gridData());
        return out;
    }

    /** btnRefresh_Click (:301-305): ItemNameFill() then LabInspectionNameFill(). */
    public Map<String, Object> combos() {
        requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", itemRows());
        out.put("labs", labRows());
        return out;
    }

    /**
     * LabInspectionNameFill (:109-129). OrganizationId and CompanyId are 0 on the desktop (point 2 of the
     * class comment) and are sent as 0 here. Bound "Id" / "LookUpName".
     */
    private List<Map<String, Object>> labRows() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.labs(0, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("LookUpName", str(ci(r, "LookUpName")));
            out.add(m);
        }
        return out;
    }

    /** ItemNameFill (:131-145): clsGlobalVariables.UserAccount's organization and company. Bound "Id" / "ItemName". */
    private List<Map<String, Object>> itemRows() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.items(ctx.currentOrganizationId(), ctx.currentCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("ItemName", str(ci(r, "ItemName")));
            out.add(m);
        }
        return out;
    }

    /** ReadAll (:252-280). */
    public Map<String, Object> grid() {
        requireView();
        return gridData();
    }

    /**
     * The grid does RetrieveStructure() on the table (:266): its columns ARE the result set's columns, in
     * the result set's order. So the column list is taken from the rows and nothing is named here.
     * Dates travel as ISO text, bits as booleans (Janus draws a bit column as a check box).
     */
    private Map<String, Object> gridData() {
        List<Map<String, Object>> rows = repo.readAll(ctx.currentOrganizationId(), ctx.currentCompanyId());
        List<String> columns = new ArrayList<>();
        List<Map<String, Object>> data = new ArrayList<>();
        if (!rows.isEmpty()) columns.addAll(rows.get(0).keySet());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) m.put(e.getKey(), json(e.getValue()));
            data.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("columns", columns);
        out.put("rows", data);
        return out;
    }

    /**
     * ReadById (:224-250) -> BLL 0472 GetByID(@Id, 'ReadById')[0].
     *
     * DAL 0524 GetAll also reads the three detail lists of the header (ReadByHeaderId,
     * ReadByHeaderId_ParameterDetail, ReadByHeaderId_ThirdPartyInspectionLotSamplingDetail); the form uses
     * none of them, so those three reads are not issued.
     *
     * The procedure filters on Id alone. On the desktop the id can only come from the company-filtered
     * grid, so the row is proven to belong to the session's organization and company before it is returned.
     */
    public Map<String, Object> byId(int id) {
        requireView();
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) throw new IllegalArgumentException(INDEX_OUT_OF_RANGE);
        Map<String, Object> r = rows.get(0);
        if (toInt(ci(r, "OrganizationId")) != ctx.currentOrganizationId() || toInt(ci(r, "CompanyId")) != ctx.currentCompanyId()) {
            throw new AccessDeniedException("This record does not belong to the current company.");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(r, "Id")));
        out.put("InspectedByLabId", toInt(ci(r, "InspectedByLabId")));       // :236
        out.put("ReportDate", json(ci(r, "ReportDate")));                    // :237
        out.put("ReportRefNo", str(ci(r, "ReportRefNo")));                   // :238
        out.put("LotRefNo", str(ci(r, "LotRefNo")));                         // :239
        out.put("ItemId", toInt(ci(r, "ItemId")));                           // :240
        out.put("QtyKgs", number(ci(r, "QtyKgs")));                          // :241 obj.QtyKgs.ToString()
        out.put("InspectionRemarks", str(ci(r, "InspectionRemarks")));       // :242
        return out;
    }

    /**
     * Insert() (:167-201) for both btnsave_Click (:214) and btnUpdate_Click (:219); RecId > 0 = update.
     * The form has NO validation of its own. In desktop order:
     *
     *   :177  Convert.ToInt32(cmbInspectionLabName.Value)   — nothing selected = 0, no error
     *   :178  datDate.Value
     *   :182  Convert.ToDouble(txtQty.Text.Trim())          — "" or text -> "Input string was not in a correct format."
     *   :184  BLL 0472 Save: EntryDate = ModifyDate = ApprovedDate = Now, ActionId = Id == 0 ? 1 : 2
     *         DAL 0524 SetData, one transaction: SetProc(USP_..._InsertAndUpdate) -> SqlDateTime overflow
     *         on RequestDate (class comment, point 1); then the loop over the null LotInspectionDetails.
     *   :185-192  "Record Save Successfully" / "Record Update Successfully", Reset(), ReadAll()
     *
     * DAL 0524 first reads two configuration values ("IsVpsAttachmentsServiceOn", "Attachment Folder
     * Path") that only matter when the header carries attachments; this form has none, so they are not read.
     */
    @Transactional
    public Map<String, Object> save(Integer id, Integer inspectedByLabId, String reportDate, String reportRefNo,
                                    String lotRefNo, Integer itemId, String qtyKgs, String inspectionRemarks) {
        requireView();
        int recId = id == null ? 0 : Math.max(id, 0);
        double qty = toDouble(qtyKgs);                                        // :182
        Timestamp now = new Timestamp(System.currentTimeMillis());
        LocalDateTime requestDate = CLR_MIN_DATE;                             // never assigned by the form

        /* Model 0866 in property order — what SetProc enumerates. Null = property left null = not sent. */
        Map<String, Object> model = params(
                "IsApproved", false,
                "ApprovedDate", now,
                "ConfirmationDate", null, "DateOfInspection", null, "ResultDate", null,
                "EntryDate", now, "ModifyDate", now,
                "ReportDate", reportDate(reportDate),
                "RequestDate", sqlDateTime(requestDate),
                "SampleATADestinationDate", null, "SampleDispatchedDate", null, "SampleETADestinationDate", null,
                "SampleHandedOverDate", null, "SampleTakenDate", null, "StockReservedDate", null, "StockRSealedDate", null,
                "QtyKgs", qty,
                "ActionId", recId == 0 ? 1 : 2,
                "ApprovedUserId", 0, "BranchesId", 0, "CompanyId", 0, "CountryOfInspectionId", 0,
                "PlaceOfInspectionId", 0, "CountryOfOriginId", 0, "EntryUserId", 0, "FinancialYearId", 0,
                "Id", recId,
                "InspectedByLabId", inspectedByLabId == null ? 0 : inspectedByLabId,
                "InspectionAgencyId", 0,
                "ItemId", itemId == null ? 0 : itemId,
                "JobLotId", 0, "LotCurrentStageId", 0, "ModifyUserId", 0, "OrganizationId", 0, "ProjectsId", 0,
                "ReportDocNo", 0, "ReportResultRemarksId", 0, "RequestedById", 0, "RequestMediumId", 0,
                "RequiredAnalysisId", 0, "RevisionNo", 0, "SampleHandedOverId", 0, "SamplingResponsibilityId", 0,
                "TransitDays", 0, "ExImFarmingNTradeId", 0, "ExImFarmingTypeId", 0, "ExImTradeTypeId", 0,
                "CourierTrackingNo", null, "ExporterLotRefNo", null,
                "InspectionRemarks", trim(inspectionRemarks),
                "InspectionReportNo", null, "LabRemarks", null,
                "LotRefNo", trim(lotRefNo),
                "PlaceOfInspection", null, "ProductSpecification", null,
                "ReportRefNo", trim(reportRefNo),
                "ReportStatus", null, "RequestedBy", null, "RequestMedium", null, "RequestRefNo", null,
                "RequiredAnalysis", null, "LotInstructionsOrRemarks", null, "ScreenName", null,
                "AttachmentsValues", null, "CustomAttachmentsValues", null);

        repo.insertAndUpdate(model);
        /* DAL 0524: "foreach (... in obj.LotInspectionDetails)" — the form never creates the list. The
           exception leaves the transaction rolled back, as the DAL's catch does. */
        throw new IllegalArgumentException(NULL_REFERENCE);
    }

    /**
     * What SqlClient does with a DateTime it has to send as SQL "datetime": below 1753-01-01 it throws
     * SqlTypeException before anything reaches the server.
     */
    private static Timestamp sqlDateTime(LocalDateTime v) {
        if (v.isBefore(SQL_MIN_DATE)) throw new IllegalArgumentException(SQL_DATETIME_OVERFLOW);
        return Timestamp.valueOf(v);
    }

    /** datDate.Value — the picked day with the time of day the picker carries (the current time). */
    private static Timestamp reportDate(String iso) {
        try {
            LocalDate d = iso == null || iso.trim().isEmpty() ? LocalDate.now() : LocalDate.parse(iso.trim().substring(0, Math.min(10, iso.trim().length())));
            return sqlDateTime(LocalDateTime.of(d, LocalTime.now().withNano(0)));
        } catch (DateTimeParseException e) {
            throw new IllegalArgumentException("String was not recognized as a valid DateTime.");
        }
    }

    /** Convert.ToDouble(txtQty.Text.Trim()). */
    private static double toDouble(String text) {
        String t = text == null ? "" : text.trim();
        if (t.isEmpty() || !NUMBER.matcher(t).matches() || !t.matches(".*\\d.*")) throw new IllegalArgumentException(FORMAT_EXCEPTION);
        try {
            return Double.parseDouble(t.replace(",", ""));
        } catch (NumberFormatException e) {
            throw new IllegalArgumentException(FORMAT_EXCEPTION);
        }
    }

    private static String trim(String v) { return v == null ? "" : v.trim(); }

    private static Object ci(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** double.ToString() — no trailing ".0". */
    private static String number(Object v) {
        if (v == null) return "0";
        if (v instanceof Number) {
            double d = ((Number) v).doubleValue();
            if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
            return String.valueOf(d);
        }
        return String.valueOf(v);
    }

    private static Object json(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toString();
        if (v instanceof java.time.temporal.Temporal) return v.toString();
        if (v instanceof byte[]) return null;
        return v;
    }

    /** Conversion.ToInt — anything unparsable is 0. */
    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
