package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportPreShipmentInspectionRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportFormSupport.*;

/**
 * BLL side of 188 PreShipmentInspection "Pre-Shipment Inspection" (Architecture.WinApp.Export), ScreenName
 * "PreShipmentInspection", AutoScrollMinSize 1050 x 800, maximised. tabControl1 "Form" | "History".
 * Rights: ScreenDefinition 188 - View, Save, Update (btnUpdate, history Edit / double-click), Print (513 slip),
 * CanViewAllRecord (history EntryUser filter). Tenancy, branch and the active financial year come from the session.
 *
 * Quirks kept: RequestDate = ReportDate, RequestRefNo = DocNo, LotRefNo = ExporterLotRefNo = the job lot text,
 * ProductSpecification = the item text, ProjectsId = BranchesId; detail rows removed from the grid are NOT
 * deleted on Update (the form never passes RemoveDetailIds); the Report Status list is fixed (Accepted /
 * Rejected / Waiting, Waiting active) and the detail Status list is fixed (In-Process / Complete, In-Process active).
 */
@Service
public class ExportPreShipmentInspectionService {

    public static final int SCREEN_ID = 188;
    public static final String SCREEN_NAME = "PreShipmentInspection";

    @Autowired private ExportPreShipmentInspectionRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** InitializeComponentMethod. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); } catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        put(out, "docNo", () -> docNo(u));
        out.putAll(refresh());
        put(out, "historyItems", () -> pick(repo.historyItems(u), "Id", "name"));
        return out;
    }

    /** DocNoDbCall: GenerateCode -> "0" when nothing comes back. */
    public String docNo(UserAccount u) {
        List<Map<String, Object>> r = repo.generateCode(u);
        if (r.isEmpty()) return "0";
        Object v = r.get(0).values().iterator().next();
        return text(v);
    }

    public Map<String, Object> docNo() { Map<String, Object> m = new LinkedHashMap<>(); m.put("docNo", docNo(user("View"))); return m; }

    /** btnRefresh_Click: inspection statuses, job lots, items + customers from the export contracts. */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "inspectionStatuses", () -> pick(repo.inspectionStatuses(u), "Id", "LookUpName"));
        put(out, "jobLots", () -> pick(repo.jobLots(u), "Id", "JobLotDescription"));
        List<Map<String, Object>> items = new ArrayList<>(), customers = new ArrayList<>();
        try {
            for (Map<String, Object> r : repo.contractDropDowns(u, 0, null)) {
                String a = text(ci(r, "ActivityType"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("Name", text(ci(r, "name")));
                if ("Items".equals(a)) items.add(m); else if ("Customer".equals(a)) customers.add(m);
            }
        } catch (Exception e) { out.put("itemsError", msg(e)); }
        out.put("items", items);
        out.put("customers", customers);
        return out;
    }

    public List<Map<String, Object>> historyItems() { return pick(repo.historyItems(user("View")), "Id", "name"); }

    /** CmbCustomer_Leave -> ContractNoDbCall(CustomerId): empty when no customer. */
    public List<Map<String, Object>> contracts(int customerId) {
        UserAccount u = user("View");
        if (customerId == 0) return new ArrayList<>();
        return pick(repo.contractDropDowns(u, customerId, "ContractNo"), "Id", "name");
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("Name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    /** ReadById(ID): header + LotInspectionDetails (dtdetail columns). */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> rows = repo.byId(id);
        if (rows.isEmpty() || asInt(ci(rows.get(0), "Id")) == 0) throw new IllegalStateException("Record Not Found...");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("ReportDocNo", asInt(ci(r, "ReportDocNo")));
        m.put("ReportDate", iso(ci(r, "ReportDate")));
        m.put("InspectedByLabId", asInt(ci(r, "InspectedByLabId")));
        m.put("JobLotId", asInt(ci(r, "JobLotId")));
        m.put("ItemId", asInt(ci(r, "ItemId")));
        m.put("QtyKgs", asDouble(ci(r, "QtyKgs")));
        m.put("ReportStatus", text(ci(r, "ReportStatus")));
        m.put("ConfirmationDate", iso(ci(r, "ConfirmationDate")));
        m.put("InspectionRemarks", text(ci(r, "InspectionRemarks")));
        m.put("RequiredAnalysis", text(ci(r, "RequiredAnalysis")));
        m.put("AttachmentsValues", text(ci(r, "AttachmentsValues")));
        m.put("CustomAttachmentsValues", text(ci(r, "CustomAttachmentsValues")));
        m.put("details", details(id));
        return m;
    }

    /** DataGridHistory_SelectionChanged -> GetByID(Id).LotInspectionDetails. */
    public List<Map<String, Object>> details(int headerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.detailsByHeaderId(headerId)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", asInt(ci(d, "Id")));
            x.put("CustomerId", asInt(ci(d, "SupplierCustomerId")));
            x.put("Customer", text(ci(d, "SupplierCustomer")));
            x.put("ContractId", asInt(ci(d, "ContractId")));
            x.put("ContractNo", text(ci(d, "ContractNo")));
            x.put("MTons", asDouble(ci(d, "Weight")));
            x.put("Status", text(ci(d, "Status")));
            x.put("SubLot", text(ci(d, "SubLot")));
            x.put("Remarks", text(ci(d, "SubRemarks")));
            out.add(x);
        }
        return out;
    }

    public List<Map<String, Object>> detailsOf(int headerId) { user("View"); return details(headerId); }

    /** HistoryFill: FormHistory with the ticked radio's date pair, GP range, item; grid columns in dtcol order. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("ScreenName", SCREEN_NAME);
        f.put("FinancialYearId", currentUserContext.currentFinancialYearId());
        boolean canViewAll = allowed(u, "CanViewAllRecord");
        f.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) f.put("EntryUser", u.getId());
        String by = text(b.get("dateBy"));
        java.sql.Date from = sqlDate(asDate(b.get("fromDate"))), to = sqlDate(asDate(b.get("toDate")));
        String fk = "entry".equals(by) ? "EntryFromDate" : "modify".equals(by) ? "ModifyFromDate" : "FromDate";
        String tk = "entry".equals(by) ? "EntryToDate" : "modify".equals(by) ? "ModifyToDate" : "ToDate";
        if (flag(b.get("fromChecked")) && from != null) f.put(fk, from);
        if (flag(b.get("toChecked")) && to != null) f.put(tk, to);
        if (asInt(b.get("itemId")) != 0) f.put("ItemId", asInt(b.get("itemId")));
        if (asInt(b.get("fromDocNo")) != 0) f.put("FromDocNo", asInt(b.get("fromDocNo")));
        if (asInt(b.get("toDocNo")) != 0) f.put("ToDocNo", asInt(b.get("toDocNo")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, f)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ReportNo", asInt(ci(r, "ReportDocNo")));
            m.put("ReportDate", iso(ci(r, "ReportDate")));
            m.put("InspectedByName", text(ci(r, "InspectedByName")));
            m.put("JobLot", text(ci(r, "LotRefNo")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("MTons", asDouble(ci(r, "QtyKgs")));
            m.put("ConfirmationDate", iso(ci(r, "ConfirmationDate")));
            m.put("ReportStatus", text(ci(r, "ReportStatus")));
            m.put("InspectionRemarks", text(ci(r, "InspectionRemarks")));
            m.put("RequiredAnalysis", text(ci(r, "RequiredAnalysis")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** Insert(): FormValidation, the detail rows with FormHelper.ValidateField, Save. */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recId")));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        String docNo = text(b.get("docNo"));
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("Doc No field is required");
        if (asInt(b.get("inspectionStatusId")) == 0) throw new IllegalArgumentException("Inspection Status field is required");
        if (asInt(b.get("jobLotId")) == 0) throw new IllegalArgumentException("JobLot field is required");
        if (asInt(b.get("itemId")) == 0) throw new IllegalArgumentException("Item Name field is required");
        if (asInt(b.get("reportStatusId")) == 0) throw new IllegalArgumentException("Report Status field is required");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Timestamp reportDate = ts(b.get("reportDate"));
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("IsApproved", false);
        h.put("ApprovedDate", now);
        h.put("ConfirmationDate", ts(b.get("confirmationDate")));
        h.put("DateOfInspection", reportDate);
        h.put("ResultDate", null);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("ReportDate", reportDate);
        h.put("RequestDate", reportDate);
        h.put("SampleATADestinationDate", null);
        h.put("SampleDispatchedDate", null);
        h.put("SampleETADestinationDate", null);
        h.put("SampleHandedOverDate", null);
        h.put("SampleTakenDate", null);
        h.put("StockReservedDate", null);
        h.put("StockRSealedDate", null);
        h.put("QtyKgs", BigDecimal.valueOf(asDouble(b.get("headerMtons"))));
        h.put("ActionId", recId == 0 ? 1 : 2);
        h.put("ApprovedUserId", u.getId());
        h.put("BranchesId", branch);
        h.put("CompanyId", u.getCompanyId());
        h.put("CountryOfInspectionId", 0);
        h.put("PlaceOfInspectionId", 0);
        h.put("CountryOfOriginId", 0);
        h.put("EntryUserId", u.getId());
        h.put("FinancialYearId", currentUserContext.currentFinancialYearId());
        h.put("Id", recId);
        h.put("InspectedByLabId", asInt(b.get("inspectionStatusId")));
        h.put("InspectionAgencyId", 0);
        h.put("ItemId", asInt(b.get("itemId")));
        h.put("JobLotId", asInt(b.get("jobLotId")));
        h.put("LotCurrentStageId", 0);
        h.put("ModifyUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("ProjectsId", branch);
        h.put("ReportDocNo", asInt(docNo));
        h.put("ReportResultRemarksId", 0);
        h.put("RequestedById", 0);
        h.put("RequestMediumId", 0);
        h.put("RequiredAnalysisId", 0);
        h.put("RevisionNo", 0);
        h.put("SampleHandedOverId", 0);
        h.put("SamplingResponsibilityId", 0);
        h.put("TransitDays", 0);
        h.put("ExImFarmingNTradeId", 0);
        h.put("ExImFarmingTypeId", 0);
        h.put("ExImTradeTypeId", 0);
        h.put("CourierTrackingNo", null);
        h.put("ExporterLotRefNo", str(b.get("jobLotText")));
        h.put("InspectionRemarks", text(b.get("inspectionRemarks")));
        h.put("InspectionReportNo", null);
        h.put("LabRemarks", null);
        h.put("LotRefNo", str(b.get("jobLotText")));
        h.put("PlaceOfInspection", null);
        h.put("ProductSpecification", text(b.get("itemText")));
        h.put("ReportRefNo", null);
        h.put("ReportStatus", str(b.get("reportStatusText")));
        h.put("RequestedBy", null);
        h.put("RequestMedium", null);
        h.put("RequestRefNo", str(b.get("docNo")));
        h.put("RequiredAnalysis", text(b.get("requiredAnalysis")));
        h.put("LotInstructionsOrRemarks", null);
        h.put("ScreenName", SCREEN_NAME);
        h.put("AttachmentsValues", str(b.get("attachmentsValues")));
        h.put("CustomAttachmentsValues", str(b.get("customAttachmentsValues")));

        List<Map<String, Object>> details = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : list(b.get("details"))) {
            Map<String, Object> d = new LinkedHashMap<>();
            BigDecimal w = dec(r.get("MTons"));
            d.put("Weight", w);
            d.put("ContractId", asInt(r.get("ContractId")));
            d.put("ContractScheduleId", 0);
            d.put("InvoiceId", 0);
            d.put("Id", asInt(r.get("Id")));
            d.put("InvLabPreProductionExportLotInspectionHeaderId", 0);
            d.put("SupplierCustomerId", asInt(r.get("CustomerId")));
            d.put("SealNoBuyer", null);
            d.put("SealNoShipper", null);
            d.put("Status", str(r.get("Status")));
            d.put("SubLot", text(r.get("SubLot")));
            d.put("SubRemarks", str(r.get("Remarks")));
            validateField(asInt(r.get("CustomerId")), "Customer Name", i);
            validateField(asInt(r.get("ContractId")), "Contract", i);
            validateField(w, "M.Tons", i);
            validateField(str(r.get("Status")), "Status", i);
            validateField(text(r.get("SubLot")), "SubLot", i);
            details.add(d);
            i++;
        }
        int n = repo.save(h, details);
        return saved(n, recId == 0 ? "Record Saved Successfully" : "Record Update Successfully");
    }

    /** FormHelper.ValidateField: 0 / <= 0 / blank -> "{field} is required in Detail Grid at row No: {n}". */
    private static void validateField(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
