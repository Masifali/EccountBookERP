package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpBShipmentBooking;
import com.mst.repositories.imports.ImpBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpBSupport.*;

/**
 * 785 Shipment Booking - Architecture.WinApp.Import.Transactions.frmShipmentBooking (ScreenName frmShipmentBooking),
 * BLL Architecture.BLL.Import.Transaction.ShipmentBooking, DAL ...Import.Transactions.ShipmentBooking.SetData.
 * Each method names the form method it reproduces; messages are the form's MessageBox texts.
 */
@Service
public class ImpBShipmentBookingService {

    public static final int SCREEN = 785;
    public static final String SCREEN_NAME = "frmShipmentBooking";
    /** GetInvoiceNoForBookingInfo(..., 902, ...) - the Import Invoice document type. */
    public static final int INVOICE_DOC_TYPE = 902;

    @Autowired private ImpBRepository repo;
    @Autowired private ImpBSupport sup;

    // ------------------------------------------------------------------ combos

    /** InvoiceNoBind(): {Id, InvoiceNo, loadingPortId, destinationPortId, fclTotal} (columns 2-4 hidden in the combo). */
    private List<Map<String, Object>> invoices(UserAccount u, long recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bookingInvoices(u, sup.financialYearId(), recId)) {
            out.add(map("Id", r.get("invoiceMasterId"), "InvoiceNo", str(r.get("invoiceMasterNo")), "loadingPortId", toInt(r.get("loadingPortId")),
                    "destinationPortId", toInt(r.get("destinationPortId")), "fclTotal", toDouble(r.get("fclTotal"))));
        }
        return out;
    }

    /** ShippingLine(): CommonServices.GetSupplierustomerByCustomerGroupId("10") -> {Id, CompanyName}. */
    private List<Map<String, Object>> shippingLines(UserAccount u) { return pick(repo.suppliersByGroup(u, "10", 0), "Id", "CompanyName"); }

    /** DestinationPort(): ImportRelated rows with Activity "SeaAirport" -> {Id, Port} (both port combos). */
    private List<Map<String, Object>> ports(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : activity(repo.importRelated(u), "SeaAirport")) out.add(map("Id", r.get("Id"), "Port", r.get("name")));
        return out;
    }

    /** CurrencyFill(): MultiCurrency.GetAll -> BindDDLNew(Id, CurrencyCode). */
    private List<Map<String, Object>> currencies() { return pick(sup.currencies(), "Id", "CurrencyCode"); }

    /** HistoryCombosFill(): USP_GetDataForDropDownFrom_ShipmentBooking split by Activity. */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> dt = repo.bookingHistoryCombos(u);
        return map("invoices", activity(dt, "invoiceMasterNo"), "ports", activity(dt, "DestinationPort"),
                "lines", activity(dt, "shippingLIne"), "bookingNos", activity(dt, "bookingNo"));
    }

    /** frmShippingBookingInfo_Load: ConfigureRights + ConfigureControls + BindData. */
    public Map<String, Object> setup() {
        UserAccount u = sup.user(SCREEN);
        return map("rights", sup.formRights(u, SCREEN, SCREEN_NAME), "historyDays", sup.historyDays(),
                "invoices", invoices(u, 0), "containerTypes", List.of(map("Id", 1, "Type", "Fcl")),
                "currencies", currencies(), "lines", shippingLines(u), "ports", ports(u), "history", historyCombos(u));
    }

    /** btnRefresh_Click: InvoiceNoBind, CurrencyFill, ShippingLine, DestinationPort, HistoryCombosFill. */
    public Map<String, Object> refresh(long recId) {
        UserAccount u = sup.user(SCREEN);
        return map("invoices", invoices(u, Math.max(0, recId)), "currencies", currencies(), "lines", shippingLines(u),
                "ports", ports(u), "history", historyCombos(u));
    }

    /** InvoiceNoBind() alone (Reset / ReadById). */
    public List<Map<String, Object>> invoiceList(long recId) { return invoices(sup.user(SCREEN), Math.max(0, recId)); }

    // ------------------------------------------------------------------ ReadById

    private Map<String, Object> own(UserAccount u, long id) {
        List<Map<String, Object>> r = repo.booking(id);
        if (r.isEmpty()) return null;
        Map<String, Object> m = r.get(0);
        if (toInt(m.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(m.get("CompanyId")) != toInt(u.getCompanyId()))
            throw invalid("Record not found.");
        return m;
    }

    /**
     * ReadById(Id): ShipmentBooking.GetByID, InvoiceNoBind() for this RecId, the record, and DMSAttachments.GetByID.
     * The invoice list is the desktop's call (@RecId = the booking id). That procedure only keeps the booked invoice
     * when @RecId equals the invoice id, so the booked invoice is re-read with @RecId = its own id and added when it
     * is missing (otherwise the record could never be updated - see the report).
     */
    public Map<String, Object> byId(long id) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> b = own(u, id);
        if (b == null) throw invalid("Record not found.");
        List<Map<String, Object>> inv = invoices(u, id);
        long invoiceId = toLong(b.get("invoiceMasterId"));
        if (invoiceId > 0 && !has(inv, "Id", invoiceId)) {
            for (Map<String, Object> r : invoices(u, invoiceId)) if (toLong(r.get("Id")) == invoiceId) inv.add(r);
        }
        return map("record", map(
                "ShipmentBookingId", b.get("ShipmentBookingId"), "bookingDate", b.get("bookingDate"), "bookingNo", str(b.get("bookingNo")),
                "invoiceMasterId", invoiceId, "containerTypeId", toInt(b.get("containerTypeId")), "totalContainer", toInt(b.get("totalContainer")),
                "dischargePortId", toInt(b.get("dischargePortId")), "loadingPortId", toInt(b.get("loadingPortId")),
                "currencyId", toInt(b.get("currencyId")), "exchangeRate", toDec(b.get("exchangeRate")),
                "vessel", str(b.get("vessel")), "voyage", str(b.get("voyage")), "etaLoadingport", b.get("etaLoadingport"),
                "etdLoadingPort", b.get("etdLoadingPort"), "transitDays", toInt(b.get("transitDays")), "etaDestinationPort", b.get("etaDestinationPort"),
                "shippingLIneId", toInt(b.get("shippingLIneId")), "containerCollectionLocation", str(b.get("containerCollectionLocation")),
                "address", str(b.get("address")), "freeDays", toInt(b.get("freeDays"))),
                "invoices", inv, "attachments", sup.attachments(u, SCREEN_NAME, id));
    }

    // ------------------------------------------------------------------ History (GridBind)

    /**
     * GridBind() -> ShipmentBooking.History: @OrganizationId, @CompanyId, @CanViewAllRecord always; the date pair of the
     * chosen radio when its box is ticked; @ShipmentBookingId (Booking No combo), @ShippingLineId, @dischargePortId when
     * != 0; @createdUserId when the user cannot view all records; @Activity SEARCH. The form puts the Invoice No filter
     * in ReportsParameters.ImInvoiceId but the BLL reads InvoiceId, so @invoiceMasterId is never sent (desktop defect,
     * reproduced). LoadingPortId is never set by the form.
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        boolean all = sup.canViewAllRecord(SCREEN_NAME);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "CanViewAllRecord", all);
        long rec = toLong(f.get("bookingNoId"));
        if (rec != 0) p.put("ShipmentBookingId", rec);
        String kind = str(f.get("dateType"));
        LocalDateTime from = toBool(f.get("fromChecked")) ? toDay(f.get("fromDate")) : null;
        LocalDateTime to = toBool(f.get("toChecked")) ? toDay(f.get("toDate")) : null;
        String fk = "entry".equals(kind) ? "EntryFromDate" : "modify".equals(kind) ? "ModifyFromDate" : "FromDate";
        String tk = "entry".equals(kind) ? "EntryToDate" : "modify".equals(kind) ? "ModifyToDate" : "ToDate";
        if (from != null) p.put(fk, ts(from));
        if (to != null) p.put(tk, ts(to));
        int line = toInt(f.get("shippingLineId"));
        if (line != 0) p.put("ShippingLineId", line);
        int port = toInt(f.get("destinationPortId"));
        if (port != 0) p.put("dischargePortId", port);
        if (!all) p.put("createdUserId", u.getId());
        p.put("Activity", "SEARCH");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bookingHistory(p)) {
            out.add(map("Id", r.get("ShipmentBookingId"), "BookingDate", r.get("bookingDate"), "BookingNo", r.get("bookingNo"),
                    "InvoiceNo", r.get("invoiceMasterNo"), "ContainerType", r.get("ContainerType"), "NoOfContainers", r.get("totalContainer"),
                    "LoadingPort", r.get("LoadingPort"), "DestinationPort", r.get("DestinationPort"), "Currency", r.get("CurrencyName"),
                    "ExchangeRate", r.get("exchangeRate"), "Vessel", r.get("vessel"), "Voyage", r.get("voyage"), "ETADate", r.get("etaLoadingport"),
                    "ETDDate", r.get("etdLoadingPort"), "TransitDays", r.get("transitDays"), "ETAFinalDate", r.get("etaDestinationPort"),
                    "ShippingLine", r.get("ShippingLineName"), "ContainerLocation", r.get("containerCollectionLocation"), "Address", r.get("address"),
                    "FreeDays", r.get("freeDays"), "EntryUser", r.get("CreatedUserName"), "EntryDate", r.get("createdOn"),
                    "ModifyUser", r.get("LastModifiedUserName"), "ModifyDate", r.get("lastModifiedOn"), "NoOfAttachments", r.get("NoOfAttachments")));
        }
        return out;
    }

    public List<Map<String, Object>> attachments(long id) {
        UserAccount u = sup.user(SCREEN);
        if (own(u, id) == null) throw invalid("Record not found.");
        return sup.attachments(u, SCREEN_NAME, id);
    }

    public ImpBSupport.DesktopAttachmentStoreFile attachmentFile(long id, int attachmentId) {
        UserAccount u = sup.user(SCREEN);
        if (own(u, id) == null) throw invalid("Record not found.");
        return sup.file(u, SCREEN_NAME, id, attachmentId);
    }

    // ------------------------------------------------------------------ Insert()

    /**
     * btnsave_Click (RecId = 0) / btnupdate_Click -> Insert(): formValidation() in the form's order and wording, the model
     * exactly as the form fills it, ShipmentBooking.Save (appActionId 1 / 2, @Activity INSERT / UPDATE) and the DAL's
     * attachment block in the same transaction; "Record Saved Successfully" / "Record Update Successfully".
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN);
        long id = Math.max(0, toLong(b.get("id")));
        sup.require(u, SCREEN, id > 0 ? "Update" : "Save");
        Map<String, Object> old = id > 0 ? own(u, id) : null;
        if (id > 0 && old == null) throw invalid("Record not found.");

        String bookingNo = str(b.get("bookingNo"));
        if (bookingNo.trim().isEmpty() || bookingNo.trim().equals("0")) throw invalid("Booking No Is Required");
        long invoiceId = toLong(b.get("invoiceMasterId"));
        boolean invoiceOk = invoiceId > 0 && (has(invoices(u, id), "Id", invoiceId) || (old != null && toLong(old.get("invoiceMasterId")) == invoiceId));
        if (!invoiceOk) throw invalid("Invoice No Is Required");
        String qty = str(b.get("totalContainer"));
        if (qty.trim().isEmpty() || toInt(qty) <= 0) throw invalid("No of Containers Must Be Greater Than 0 : Thank You");
        if (toInt(b.get("loadingPortId")) <= 0) throw invalid("Loading Port Is Required");
        if (toInt(b.get("dischargePortId")) <= 0) throw invalid("Destination Port Is Required");
        if (toInt(b.get("shippingLIneId")) <= 0) throw invalid("Shipping Line Is Required");
        String free = str(b.get("freeDays"));
        if (free.trim().isEmpty() || toInt(free) <= 0) throw invalid("Free Days Required");

        LocalDateTime now = LocalDateTime.now();
        ImpBShipmentBooking m = new ImpBShipmentBooking();
        m.ShipmentBookingId = id;
        m.bookingDate = nvl(toDate(b.get("bookingDate")), now);
        m.bookingNo = bookingNo;
        m.invoiceMasterId = invoiceId;
        m.containerTypeId = toInt(b.get("containerTypeId"));
        m.totalContainer = toInt(qty);
        m.loadingPortId = toInt(b.get("loadingPortId"));
        m.dischargePortId = toInt(b.get("dischargePortId"));
        m.currencyId = toInt(b.get("currencyId"));
        m.exchangeRate = toDec(b.get("exchangeRate"));
        m.vessel = str(b.get("vessel")).trim();
        m.voyage = str(b.get("voyage")).trim();
        m.etaLoadingport = nvl(toDate(b.get("etaLoadingport")), now);
        m.etdLoadingPort = nvl(toDate(b.get("etdLoadingPort")), now);
        m.transitDays = toInt(b.get("transitDays"));
        m.etaDestinationPort = nvl(toDate(b.get("etaDestinationPort")), now);
        m.shippingLIneId = toInt(b.get("shippingLIneId"));
        m.containerCollectionLocation = str(b.get("containerCollectionLocation"));
        m.address = str(b.get("address"));
        m.freeDays = toInt(free);
        m.RowVersionLong = old == null ? 0 : toLong(old.get("RowVersionLong"));
        m.createdOn = now; m.lastModifiedOn = now; m.approvedOn = now; m.DeletedOn = now;
        m.docDate = m.bookingDate;
        m.createdUserId = u.getId(); m.lastModifiedUserId = u.getId(); m.approvedUserId = u.getId(); m.DeletedUserId = u.getId();
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchesId = branchId(u);
        m.ProjectsId = branchId(u);
        m.FinancialYearId = sup.financialYearId();
        m.AttachmentsValues = old == null ? "" : str(old.get("AttachmentsValues"));
        m.CustomAttachmentsValues = old == null ? "" : str(old.get("CustomAttachmentsValues"));
        m.appActionId = id <= 0 ? 1 : 2;                                   // BLL Save
        String activity = id > 0 ? "UPDATE" : "INSERT";

        final ImpBSupport.AttachmentPlan[] plan = new ImpBSupport.AttachmentPlan[1];
        long saved = repo.tx(() -> {
            plan[0] = sup.prepare(u, SCREEN_NAME, id, b);
            if (plan[0].changed && !plan[0].finalList.isEmpty()) { m.AttachmentsValues = plan[0].values(); m.CustomAttachmentsValues = plan[0].customValues(); }
            long num = repo.set(ImpBRepository.IMEX + "[usp_Set_ShipmentBooking]", m, activity);
            if (num <= 0) num = m.ShipmentBookingId;
            sup.apply(plan[0], u, SCREEN_NAME, num, (int) m.invoiceMasterId, 0);
            return num;
        });
        if (id > 0) sup.afterUpdate(plan[0], id, SCREEN_NAME, 0);
        return saved((int) saved, id > 0 ? "Record Update Successfully" : "Record Saved Successfully");
    }

    private static LocalDateTime nvl(LocalDateTime a, LocalDateTime b) { return a == null ? b : a; }

    /** btnPrint_Click / grid Print -> CommonServices.ImBookingInfoSlip(id): see the report - not traceable here. */
    public Map<String, Object> print(long id) {
        UserAccount u = sup.user(SCREEN);
        if (id <= 0) throw invalid("No Record found");
        sup.require(u, SCREEN, "Print");
        throw invalid("903-Print (CommonServices.ImBookingInfoSlip) is not available on the web: the desktop's CommonServices source is not "
                + "in the workspace, so its .rpt and procedure cannot be traced.");
    }

    static BigDecimal zero() { return BigDecimal.ZERO; }
}
