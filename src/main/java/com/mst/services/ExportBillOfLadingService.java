package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportBillOfLadingRepository;
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
 * BLL side of 213 EximBillOfLading "Export Bill Of Lading" (Architecture.WinApp.Export), ClientSize 964 x 401.
 * tabControl1 "Form" | "History". Rights: ScreenDefinition 213 - View, Save (btnsave), Update (btnUpdate, the
 * history Edit button), Print (505-Print, history Print button), CanViewAllRecord (history filter by EntryUser
 * when missing). Tenancy and the active financial year come from the session.
 *
 * Quirks kept: the history grid's "FrieghtType" column is filled with VoyageNo (BindGrid copies the wrong cell);
 * the "Other" group (E-Form, contract, currency, weights, containers) is disabled and only shows what the invoice
 * carries - it is never saved; FreightType / CarierType are saved as the combo TEXT; BranchesId / ProjectsId are
 * never set by the form (0). The Attachment popup (DMS) is not part of this port.
 */
@Service
public class ExportBillOfLadingService {

    public static final int SCREEN_ID = 213;

    @Autowired private ExportBillOfLadingRepository repo;
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
        catch (RuntimeException ex) { return false; }
    }

    // ================================================================= load

    /** EximShipmentInfo_Load: rights, invoices, suppliers (Notify party / Importer), banks (To The Order), history invoice combo. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.putAll(refresh());
        put(out, "historyInvoices", () -> historyInvoices(u));
        return out;
    }

    /** BtnRefresh_Click: bindInvoiceNo + bindSupplier (banks re-read too, harmless). */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> pick(repo.invoicesForBillOfLading(u), "Id", "InvoiceNo"));
        put(out, "suppliers", () -> pick(repo.exportParties(u), "Id", "CompanyName"));
        put(out, "banks", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> b : repo.banks(u)) {
                if (!"Home Country".equals(text(ci(b, "IsHomeland")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(b, "Id")));
                m.put("Name", text(ci(b, "BranchName")));
                rows.add(m);
            }
            return rows;
        });
        return out;
    }

    /** HistoryCombosFill: GetDataForDropDownFromBillOfLading rows whose Activity == "InvoiceNo" (Id, ReferenceName). */
    public List<Map<String, Object>> historyInvoices(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            if (!"InvoiceNo".equals(text(ci(r, "Activity")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "ReferenceName")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> historyInvoices() { return historyInvoices(user("View")); }

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

    /**
     * cmbInvoiceNo_Leave: cmbinvoiceLeaveComboFills (ExImInvoice.GetData Ids "204,211", Id) and
     * ReadBookingCroByInvoiceId. The invoice rows are returned as the form reads them: row 0's DeliveryTerm,
     * GrossWeight, NetWeight, NoOfContainers, SupplierCustomerId, EFormNo, EFormDate, NotifyParty1/2, and the
     * LcOrderNo / Currency pairs of the rows whose Id == InvoiceId (first one activated).
     */
    public Map<String, Object> invoiceLeave(int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        if (invoiceId <= 0) return out;
        List<Map<String, Object>> rows = repo.invoiceData(u, currentUserContext.currentFinancialYearId(), "204,211", invoiceId);
        out.put("found", !rows.isEmpty());
        if (!rows.isEmpty()) {
            Map<String, Object> r0 = rows.get(0);
            out.put("DeliveryTerm", text(ci(r0, "DeliveryTerm")));
            out.put("GrossWeight", text(ci(r0, "GrossWeight")));
            out.put("NetWeight", text(ci(r0, "NetWeight")));
            out.put("NoOfContainers", text(ci(r0, "NoOfContainers")));
            out.put("SupplierCustomerId", asInt(ci(r0, "SupplierCustomerId")));
            out.put("EFormNo", text(ci(r0, "EFormNo")));
            out.put("EFormDate", iso(ci(r0, "EFormDate")));
            int np1 = asInt(ci(r0, "NotifyParty1"));
            out.put("NotifyPartyId", np1 <= 0 ? asInt(ci(r0, "NotifyParty2")) : np1);
            List<Map<String, Object>> lcs = new ArrayList<>(), ccy = new ArrayList<>();
            for (Map<String, Object> r : rows) {
                if (asInt(ci(r, "Id")) != invoiceId) continue;
                Map<String, Object> l = new LinkedHashMap<>();
                l.put("Id", asInt(ci(r, "LcOrderNoId"))); l.put("Name", text(ci(r, "LcOrderNo"))); lcs.add(l);
                Map<String, Object> c = new LinkedHashMap<>();
                c.put("Id", asInt(ci(r, "FcurrencyId"))); c.put("Name", text(ci(r, "CurrencyCode"))); ccy.add(c);
            }
            out.put("lcOrders", lcs);
            out.put("currencies", ccy);
        }
        List<Map<String, Object>> bk = repo.bookingByInvoiceId(u, invoiceId);
        Map<String, Object> b = new LinkedHashMap<>();
        b.put("found", !bk.isEmpty());
        if (!bk.isEmpty()) {
            b.put("BookingCroNo", text(ci(bk.get(0), "BookingCroNo")));
            b.put("BookingDate", iso(ci(bk.get(0), "BookingDate")));
            b.put("VesselName", text(ci(bk.get(0), "VesselName")));
            b.put("VoyageNo", text(ci(bk.get(0), "VoyageNo")));
        }
        out.put("booking", b);
        return out;
    }

    /** ReadById(Id): ExImBillOfLading.GetByID. */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> rows = repo.byId(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was outside the bounds of the array.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
        m.put("ExImInvoiceNo", text(ci(r, "ExImInvoiceNo")));
        m.put("OrderOfId", asInt(ci(r, "OrderOfId")));
        m.put("ToTheOrderOfName", text(ci(r, "ToTheOrderOfName")));
        m.put("NotifyPartyId", asInt(ci(r, "NotifyPartyId")));
        m.put("NotifyPartyName", text(ci(r, "NotifyPartyName")));
        m.put("ImporterId", asInt(ci(r, "ImporterId")));
        m.put("ImporterName", text(ci(r, "ImporterName")));
        m.put("BLNumber", text(ci(r, "BLNumber")));
        m.put("BLDate", iso(ci(r, "BLDate")));
        m.put("VesselNo", text(ci(r, "VesselNo")));
        m.put("VoyageNo", text(ci(r, "VoyageNo")));
        m.put("FreightType", text(ci(r, "FreightType")));
        m.put("CarierType", text(ci(r, "CarierType")));
        m.put("BookingNo", text(ci(r, "BookingNo")));
        m.put("BookingDate", iso(ci(r, "BookingDate")));
        m.put("FobValue", asDouble(ci(r, "FobValue")));
        m.put("ItemsGoodsDesc", text(ci(r, "ItemsGoodsDesc")));
        m.put("Remarks", text(ci(r, "Remarks")));
        m.put("CompanyId", asInt(ci(r, "CompanyId")));
        m.put("ProjectsId", asInt(ci(r, "ProjectsId")));
        m.put("BranchesId", asInt(ci(r, "BranchesId")));
        return m;
    }

    // ================================================================= history

    /**
     * BindGrid: date pair by the ticked radio (Doc / Entry / Modify), only ticked pickers, @ExImInvoiceId when chosen;
     * EntryUser filter unless the user has the CanViewAllRecord right. Grid columns in dt order; "FrieghtType"
     * carries VoyageNo as on the desktop.
     */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        Map<String, Object> dates = new LinkedHashMap<>();
        String by = text(b.get("dateBy"));
        java.sql.Date from = sqlDate(asDate(b.get("fromDate"))), to = sqlDate(asDate(b.get("toDate")));
        boolean fromOn = flag(b.get("fromChecked")), toOn = flag(b.get("toChecked"));
        String f = "entry".equals(by) ? "EntryFromDate" : "modify".equals(by) ? "ModifyFromDate" : "FromDate";
        String t = "entry".equals(by) ? "EntryToDate" : "modify".equals(by) ? "ModifyToDate" : "ToDate";
        if (fromOn && from != null) dates.put(f, from);
        if (toOn && to != null) dates.put(t, to);
        boolean canViewAll = allowed(u, "CanViewAllRecord");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, canViewAll, dates, asInt(b.get("invoiceId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("EximInvoiceId", asInt(ci(r, "EximInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("BookingNo", text(ci(r, "BookingNo")));
            m.put("BookingDate", iso(ci(r, "BookingDate")));
            m.put("BLNumber", text(ci(r, "BLNumber")));
            m.put("BLDate", iso(ci(r, "BLDate")));
            m.put("VesselNo", text(ci(r, "VesselNo")));
            m.put("VoyageNo", text(ci(r, "VoyageNo")));
            m.put("FrieghtType", text(ci(r, "VoyageNo")));      // desktop: dtGrid.Rows[i]["VoyageNo"] in the FrieghtType column
            m.put("CarierType", text(ci(r, "CarierType")));
            m.put("FobValue", text(ci(r, "FobValue")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= save

    /** Insert(): FormValidation in the desktop's order, then ExImBillOfLading.Save with the fields the form sets. */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recId")));
        boolean updateMode = flag(b.get("updateMode"));
        UserAccount u = user(updateMode && recId > 0 ? "Update" : "Save");
        if (asInt(b.get("invoiceId")) == 0) throw new IllegalArgumentException("Invoice  Field Required");
        if (text(b.get("bookingNo")).isEmpty()) throw new IllegalArgumentException("Booking No  Field Required");
        if (text(b.get("blNumber")).isEmpty()) throw new IllegalArgumentException("Bl Number  Field Required");
        if (text(b.get("voyageNo")).isEmpty()) throw new IllegalArgumentException("Voyage No  Field Required");
        if (asInt(b.get("freightId")) == 0 || text(b.get("freightText")).isEmpty()) throw new IllegalArgumentException("Freight Type Field Required");
        if (asInt(b.get("carierId")) == 0 || text(b.get("carierText")).isEmpty()) throw new IllegalArgumentException("Carrier Type  Field Required");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("ApprovedDate", now);
        m.put("BLDate", ts(b.get("blDate")));
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("ShipingCertDate", now);
        m.put("FobValue", BigDecimal.valueOf(asDouble(b.get("fobValue"))));
        m.put("ApprovedUser", u.getId());
        m.put("BranchesId", 0);
        m.put("CompanyId", u.getCompanyId());
        m.put("EntryUser", u.getId());
        m.put("ExImInvoiceId", asInt(b.get("invoiceId")));
        m.put("Id", (recId > 0 && updateMode) ? recId : 0);
        m.put("ImporterId", asInt(b.get("importerId")));
        m.put("ModifyUser", u.getId());
        m.put("NotifyPartyId", asInt(b.get("notifyPartyId")));
        m.put("OrderOfId", asInt(b.get("orderOfId")));
        m.put("OrganizationId", u.getOrganizationId());
        m.put("ProjectsId", 0);
        m.put("ShipingCertNo", 0);
        m.put("BLNumber", str(b.get("blNumber")));
        m.put("BookingDate", ts(b.get("bookingDate")));
        m.put("BookingNo", str(b.get("bookingNo")));
        m.put("CarierType", str(b.get("carierText")));
        m.put("FreightType", str(b.get("freightText")));
        m.put("NotifyPartyName", str(b.get("notifyPartyName")));
        m.put("Remarks", str(b.get("remarks")));
        m.put("ToTheOrderOfName", str(b.get("toTheOrderOfName")));
        m.put("VesselNo", str(b.get("vesselName")));
        m.put("VoyageNo", str(b.get("voyageNo")));
        m.put("ImporterName", str(b.get("importerName")));
        m.put("ItemsGoodsDesc", str(b.get("goodsDesc")));
        int n = repo.save(m);
        return saved(n, updateMode ? "Update SuccessFully" : "Save SuccessFully");
    }

    /** TextBox.Text as the desktop sends it (untrimmed; null -> ""). */
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
