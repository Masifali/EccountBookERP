package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportShippingBookingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportFormSupport.*;

/**
 * BLL side of 214 EximShippingBookingInfo "Export Shipping Booking Info" (Architecture.WinApp.Export),
 * AutoScrollMinSize 1250 x 800. tabControl1 "Form" | "History". Rights: ScreenDefinition 214 - View, Save, Update
 * (btnupdate, history Edit / double-click), Print (560-ExBooking Info(CRO).rpt). Tenancy and the active financial
 * year come from the session. The Attachment popup and the ExImLookups popup (Lookup button, LookupId 4) are not
 * part of this port; the container type list is the fixed SpStaticColumnNames('ExportContainerTypeForCRO') table.
 */
@Service
public class ExportShippingBookingService {

    public static final int SCREEN_ID = 214;

    @Autowired private ExportShippingBookingRepository repo;
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

    /** InitializeComponentMethod: rights, configuration, container types, ports, invoices, carrier mediums, history combos, suppliers (group 10). */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        put(out, "companyName", () -> repo.companyName(u));
        out.putAll(refresh());
        put(out, "historyCombos", () -> historyCombos(u));
        return out;
    }

    /** btnRefresh_Click: global party list, configuration, invoices, suppliers, carrier mediums, ports, delivered-at, container types. */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); } catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        put(out, "invoices", () -> pick(repo.invoicesForBookingInfo(u), "Id", "InvoiceNo"));
        put(out, "suppliers", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.globalAllSupplierCustomer(u)) {
                if (asInt(ci(r, "CustomerGroupId")) != 10) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("CompanyName", text(ci(r, "CompanyName")));
                m.put("PartyCode", text(ci(r, "PartyCode")));
                m.put("CityName", text(ci(r, "CityName")));
                m.put("MobileNo", text(ci(r, "MobilePersonal")));
                rows.add(m);
            }
            return rows;
        });
        put(out, "carrierMediums", () -> pick(repo.carrierMediums(u), "Id", "LookUpName"));
        put(out, "ports", () -> pick(repo.ports(u), "Id", "PortName"));
        put(out, "containerTypes", () -> pick(repo.containerTypes(), "Id", "type"));
        return out;
    }

    /** HistoryComboBind: GetDataForDropDownFromShippingBookingInfo split by Activity (Customer, InvoiceNo, DestinationPort, ShippingLine, ShippingAgent, Forwarder, BookingCroNo). */
    public Map<String, Object> historyCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        String[] keys = { "Customer", "InvoiceNo", "DestinationPort", "ShippingLine", "ShippingAgent", "Forwarder", "BookingCroNo" };
        for (String k : keys) out.put(k, new ArrayList<Map<String, Object>>());
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            String a = text(ci(r, "Activity"));
            if (!out.containsKey(a)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "ReferenceName")));
            @SuppressWarnings("unchecked") List<Map<String, Object>> l = (List<Map<String, Object>>) out.get(a);
            l.add(m);
        }
        return out;
    }

    public Map<String, Object> historyCombos() { return historyCombos(user("View")); }

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

    /** cmbExportInvoice_Leave: ExImInvoice.GetData(Ids "204,1816", Id) -> NoOfContainers, DestinationPortId. */
    public Map<String, Object> invoiceLeave(int invoiceId) {
        UserAccount u = user("View");
        List<Map<String, Object>> rows = repo.invoiceData(u, currentUserContext.currentFinancialYearId(), "204,1816", invoiceId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", !rows.isEmpty());
        if (!rows.isEmpty()) {
            out.put("NoOfContainers", text(ci(rows.get(0), "NoOfContainers")));
            out.put("DestinationPortId", asInt(ci(rows.get(0), "DestinationPortId")));
        }
        return out;
    }

    /** ReadById(Id): header + ReadDetailByHeaderId rows (Id, Container, SealNo, ContainerTypeId). */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> rows = repo.byId(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was outside the bounds of the array.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("BookingDate", iso(ci(r, "BookingDate")));
        m.put("BookingCroNo", text(ci(r, "BookingCroNo")));
        m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
        m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
        m.put("ShippingLineId", asInt(ci(r, "ShippingLineId")));
        m.put("TransporterId", asInt(ci(r, "TransporterId")));
        m.put("ShippingAgentId", asInt(ci(r, "ShippingAgentId")));
        m.put("ContrsQty", asInt(ci(r, "ContrsQty")));
        m.put("CarierMediumId", asInt(ci(r, "CarierMediumId")));
        m.put("VesselName", text(ci(r, "VesselName")));
        m.put("VoyageNo", text(ci(r, "VoyageNo")));
        m.put("ETADate", iso(ci(r, "ETADate")));
        m.put("ETDDate", iso(ci(r, "ETDDate")));
        m.put("CuttOFFDate", iso(ci(r, "CuttOFFDate")));
        m.put("TransitDays", asInt(ci(r, "TransitDays")));
        m.put("PortId", asInt(ci(r, "PortId")));
        m.put("FreeDaysAtDestinations", asInt(ci(r, "FreeDaysAtDestinations")));
        m.put("BookingRate", asDouble(ci(r, "BookingRate")));
        m.put("ContainerDispatchedAtPortId", asInt(ci(r, "ContainerDispatchedAtPortId")));
        m.put("ContainerReceivedFromPortId", asInt(ci(r, "ContainerReceivedFromPortId")));
        m.put("DeliveredAtId", asInt(ci(r, "DeliveredAtId")));
        m.put("RemarksHeader", text(ci(r, "RemarksHeader")));
        m.put("AttachmentsValues", text(ci(r, "AttachmentsValues")));
        m.put("CustomAttachmentsValues", text(ci(r, "CustomAttachmentsValues")));
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : repo.detailsByHeaderId(id)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", asInt(ci(d, "Id")));
            x.put("ContainerNo", text(ci(d, "Container")));
            x.put("SealNo", text(ci(d, "SealNo")));
            x.put("ContainerTypeId", asInt(ci(d, "ContainerTypeId")));
            det.add(x);
        }
        m.put("details", det);
        return m;
    }

    /** GridBind: FormHistory with the ticked date pair and the seven history combos; grid columns in dtGridHistory order. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        Map<String, Object> f = new LinkedHashMap<>();
        String by = text(b.get("dateBy"));
        java.sql.Date from = sqlDate(asDate(b.get("fromDate"))), to = sqlDate(asDate(b.get("toDate")));
        String fk = "entry".equals(by) ? "EntryFromDate" : "modify".equals(by) ? "ModifyFromDate" : "FromDate";
        String tk = "entry".equals(by) ? "EntryToDate" : "modify".equals(by) ? "ModifyToDate" : "ToDate";
        if (flag(b.get("fromChecked")) && from != null) f.put(fk, from);
        if (flag(b.get("toChecked")) && to != null) f.put(tk, to);
        if (asInt(b.get("bookingId")) != 0) f.put("Id", asInt(b.get("bookingId")));
        if (asInt(b.get("customerId")) != 0) f.put("SupplierCustomerId", asInt(b.get("customerId")));
        if (asInt(b.get("shippingAgentId")) != 0) f.put("ShippingAgentId", asInt(b.get("shippingAgentId")));
        if (asInt(b.get("shippingLineId")) != 0) f.put("ShippingLineId", asInt(b.get("shippingLineId")));
        if (asInt(b.get("forwarderId")) != 0) f.put("TransporterId", asInt(b.get("forwarderId")));
        if (asInt(b.get("destinationPortId")) != 0) f.put("DestinationPortId", asInt(b.get("destinationPortId")));
        if (asInt(b.get("invoiceId")) != 0) f.put("ExImInvoiceId", asInt(b.get("invoiceId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, f)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BookingDate", iso(ci(r, "BookingDate")));
            m.put("BookingCroNo", text(ci(r, "BookingCroNo")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("ShippingLine", text(ci(r, "ShippingLineName")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("ForwarderName", text(ci(r, "ForwarderName")));
            m.put("VesselName", text(ci(r, "VesselName")));
            m.put("VoyageNo", text(ci(r, "VoyageNo")));
            m.put("FreeDays", asInt(ci(r, "FreeDaysAtDestinations")));
            m.put("NoOfContainers", asInt(ci(r, "ContrsQty")));
            m.put("ClearingAgent", text(ci(r, "ClearingAgentName")));
            m.put("ETADate", iso(ci(r, "ETADate")));
            m.put("ETDDate", iso(ci(r, "ETDDate")));
            m.put("NotReferredContainerNos", text(ci(r, "ContainerNos")));
            m.put("ReferredContainerNos", text(ci(r, "ContainerReferred")));
            m.put("Remarks", text(ci(r, "RemarksHeader")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(): FormHelper.ValidateControls (BookingCRO string, Invoice No / Carrier Medium / Shipping Line combos,
     * No of Containers / Free Days ints), the container rows with a non-empty ContainerNo, "Container Rows can not
     * greater than No. of Containers " when the grid has more rows than ContrsQty, then ExImExportShipingLineBooking.Save.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recId")));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        if (text(b.get("bookingCro")).isEmpty()) throw new IllegalArgumentException("BookingCRO field is required");
        if (asInt(b.get("invoiceId")) == 0) throw new IllegalArgumentException("Invoice No field is required");
        if (asInt(b.get("carierMediumId")) == 0) throw new IllegalArgumentException("Carrier Medium field is required");
        if (asInt(b.get("shippingLineId")) == 0) throw new IllegalArgumentException("Shipping Line field is required");
        if (asInt(b.get("contrsQty")) == 0) throw new IllegalArgumentException("No of Containers must be a non-zero number");
        if (asInt(b.get("freeDays")) == 0) throw new IllegalArgumentException("Free Days must be a non-zero number");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("BookingDate", ts(b.get("bookingDate")));
        h.put("ETADate", ts(b.get("eta")));
        h.put("ETDDate", ts(b.get("etd")));
        h.put("CuttOFFDate", ts(b.get("cutOffDate")));
        h.put("ContrsQty", asInt(b.get("contrsQty")));
        h.put("TransitDays", asInt(b.get("transitDays")));
        h.put("ExImInvoiceId", asInt(b.get("invoiceId")));
        h.put("Id", recId);
        h.put("ShippingAgentId", asInt(b.get("clearingAgentId")));
        h.put("TransporterId", asInt(b.get("forwarderId")));
        h.put("ShippingLineId", asInt(b.get("shippingLineId")));
        h.put("CarierMediumId", asInt(b.get("carierMediumId")));
        h.put("BookingCroNo", text(b.get("bookingCro")));
        h.put("AttachmentsValues", nullIfEmpty(text(b.get("attachmentsValues"))));
        h.put("CustomAttachmentsValues", nullIfEmpty(text(b.get("customAttachmentsValues"))));
        h.put("FreeDaysAtDestinations", asInt(b.get("freeDays")));
        h.put("RemarksHeader", b.get("remarks") == null ? "" : String.valueOf(b.get("remarks")));
        h.put("VesselName", text(b.get("vesselName")));
        h.put("VoyageNo", text(b.get("voyageNo")));
        h.put("PortId", asInt(b.get("destinationPortId")));
        h.put("ContainerDispatchedAtPortId", asInt(b.get("containerDispatchedAtPortId")));
        h.put("ContainerReceivedFromPortId", asInt(b.get("containerReceivedFromPortId")));
        h.put("DeliveredAtId", asInt(b.get("deliveredAtId")));
        h.put("CompanyId", u.getCompanyId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("EntryUser", u.getId());
        h.put("BookingRate", dec(b.get("bookingRate")));
        h.put("EntryDate", now);
        h.put("ModifyUser", u.getId());
        h.put("ModifyDate", now);

        List<Map<String, Object>> grid = list(b.get("details"));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : grid) {
            if (text(r.get("ContainerNo")).isEmpty()) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("ExImExportShipingLineBookingId", 0);
            d.put("Id", asInt(r.get("Id")));
            d.put("ContainerTypeId", asInt(r.get("ContainerTypeId")));
            d.put("ContainerType", text(r.get("ContainerType")));
            d.put("Container", String.valueOf(r.get("ContainerNo")));
            d.put("ContainerSize", null);
            d.put("SealNo", text(r.get("SealNo")));
            details.add(d);
        }
        if (asInt(b.get("contrsQty")) < grid.size()) throw new IllegalArgumentException("Container Rows can not greater than No. of Containers ");
        int n = repo.save(h, details);
        return saved(n, recId > 0 ? "Record Update Successfully" : "Record Saved Successfully");
    }

    private static String nullIfEmpty(String s) { return s == null || s.isEmpty() ? null : s; }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
