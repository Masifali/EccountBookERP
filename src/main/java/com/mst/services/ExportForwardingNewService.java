package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportForwardingRepository;
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

import static com.mst.repositories.ExportForwardingRepository.ci;
import static com.mst.repositories.ExportForwardingRepository.str;
import static com.mst.repositories.ExportForwardingRepository.toBool;
import static com.mst.repositories.ExportForwardingRepository.toDouble;
import static com.mst.repositories.ExportForwardingRepository.toInt;
import static com.mst.services.ExportForwardingService.at;
import static com.mst.services.ExportForwardingService.day;
import static com.mst.services.ExportForwardingService.iso;
import static com.mst.services.ExportForwardingService.list;
import static com.mst.services.ExportForwardingService.put;
import static com.mst.services.ExportForwardingService.row;

/**
 * BLL side of 793 "Export Forwarding (New)" - Architecture.WinApp.Export.frmForwardingNew (forwarding / transfer,
 * DocumentTypeId 210, App 8 / Module 100).
 *
 * Rights: the screen opens with ScreenDefinition 793's View right; the form itself reads its button rights with
 * CommonServices.SetRightsValueInRightsObject("EximForwarding"), i.e. from screen 212 - Save, Update, Delete and
 * CanView AllRecord are therefore checked against 212 here too (desktop behaviour kept). The 507-Print buttons are
 * not rights-gated on this form.
 */
@Service
public class ExportForwardingNewService {

    public static final int SCREEN_ID = 793;
    public static final int RIGHTS_SCREEN_ID = ExportForwardingService.SCREEN_ID;   // "EximForwarding"
    public static final int DOCUMENT_TYPE_ID = 210;

    @Autowired private ExportForwardingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount view() {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }

    private UserAccount right(String action) {
        UserAccount u = view();
        rights.require(u, RIGHTS_SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, RIGHTS_SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int fy() { return ctx.currentFinancialYearId(); }

    // ============================================================================ load

    /** ExpfrmForwarding_Load. */
    public Map<String, Object> setup() {
        UserAccount u = view();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        out.put("permissions", perm);
        out.put("config", config(u));
        put(out, "gatePasses", () -> gatePassCombo(u));
        put(out, "gatePassGrid", () -> repo.gatePassGrid(u, fy()));
        put(out, "docNo", () -> repo.generateCode(u, fy(), DOCUMENT_TYPE_ID));
        put(out, "packingTypes", () -> repo.packingTypes());
        put(out, "jobLots", () -> repo.jobLots(u));
        put(out, "warehouses", () -> repo.activeWarehouses(u));
        put(out, "cropYears", () -> repo.cropYears(u));
        /* TransporterBind runs before any invoice is chosen: ReadByInvoiceId with ExImInvoiceId 0. */
        put(out, "transporterId", () -> transporterOf(u, 0));
        put(out, "transporters", () -> parties(u));
        put(out, "ports", () -> repo.seaPorts(u));
        put(out, "items", () -> repo.exportItems(u));
        put(out, "lastWarehouseToId", () -> lastWarehouse(u));
        put(out, "historyCustomers", () -> historyCustomers(u));
        return out;
    }

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("NetWeightAndStockWeightEqualOnForwarding", toBool(repo.config(u, "NetWeightAndStockWeightEqualOnForwarding")));
        c.put("WagesCompulsoryOnForwarding", toBool(repo.config(u, "WagesCompulsoryOnForwarding")));
        c.put("WagesActiveOrInActive", repo.wagesActive(DOCUMENT_TYPE_ID));
        c.put("DefaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        /* clsGlobalVariables.stringFormatsingleForFcy (CommonServices.GetDecimalConfiguration): "#,##0." + N, N = 1..4 */
        int fcy = toInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        c.put("FcyDecimals", fcy >= 1 && fcy <= 4 ? fcy : 0);
        return c;
    }

    /** btnRefresh_Click: wages status, gate passes, transporters, packing types, ports, job lots, warehouses. */
    public Map<String, Object> refresh() {
        UserAccount u = view();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        put(out, "gatePasses", () -> gatePassCombo(u));
        put(out, "transporters", () -> parties(u));
        put(out, "packingTypes", () -> repo.packingTypes());
        put(out, "ports", () -> repo.seaPorts(u));
        put(out, "jobLots", () -> repo.jobLots(u));
        put(out, "warehouses", () -> repo.activeWarehouses(u));
        return out;
    }

    /** FormReset: generateCode, gatepassGetAll, GridGatepassFill. */
    public Map<String, Object> reset() {
        UserAccount u = view();
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "docNo", () -> repo.generateCode(u, fy(), DOCUMENT_TYPE_ID));
        put(out, "gatePasses", () -> gatePassCombo(u));
        put(out, "gatePassGrid", () -> repo.gatePassGrid(u, fy()));
        return out;
    }

    private List<Map<String, Object>> gatePassCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingGatePasses(u, fy())) out.add(row("Id", toInt(ci(r, "Id")), "GpSrNo", str(ci(r, "GpSrNo"))));
        return out;
    }

    private List<Map<String, Object>> parties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.suppliersByGroup(u, "10")) out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName"))));
        return out;
    }

    /** TransporterBind - ExImExportShipingLineBooking.ReadByInvoiceId: the first row's TransporterId (0 when none). */
    private int transporterOf(UserAccount u, int invoiceId) {
        List<Map<String, Object>> r = repo.shippingBookingByInvoice(u, invoiceId);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "TransporterId"));
    }

    /** GetLastWareHouse: cmbWareHouseTo.Value = the first row's third column. */
    private int lastWarehouse(UserAccount u) {
        List<Map<String, Object>> r = repo.lastWarehouse(u, DOCUMENT_TYPE_ID);
        if (r.isEmpty()) return 0;
        List<Object> vals = new ArrayList<>(r.get(0).values());
        return vals.size() > 2 ? toInt(vals.get(2)) : 0;
    }

    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> c = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u))
            if ("Customer".equals(str(ci(r, "Activity")))) c.add(row("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "ReferenceName"))));
        return c;
    }

    public List<Map<String, Object>> historyCombosRefresh() { return historyCustomers(view()); }

    // ============================================================================ events

    /** cmbGpNo_Leave: GetGatePassDataForForwarding + bindInvoiceNo (GetInvoiceNoForExportForwarding). */
    public Map<String, Object> gatePass(int gpId) {
        UserAccount u = view();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> gp = repo.gatePassDetail(u, gpId);
        out.put("gatePass", gp.isEmpty() ? null : gp.get(0));
        out.put("invoices", repo.invoicesForGatePass(u, gpId, fy()));
        return out;
    }

    /**
     * cmbInvoiceNo_Leave: GetDeliveryOrderbyGatepassId (DeliveryOrderDetailForExportGdnForwarding, the invoice's other
     * items, BindOtherItems) then ContractORDERNo (ExImInvoice.GetData Ids "209", Id = invoice).
     */
    public Map<String, Object> invoice(int gpId, int invoiceId) {
        UserAccount u = view();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("deliveryOrder", repo.deliveryOrderDetail(u, invoiceId, gpId));
        List<Map<String, Object>> others = new ArrayList<>();
        for (Map<String, Object> r : repo.invoiceOtherItems(invoiceId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", toInt(ci(r, "otherItemId")));
            m.put("ItemName", str(ci(r, "ItemName")));
            m.put("Qty", toDouble(ci(r, "oItemQty")));
            m.put("Rate", toDouble(ci(r, "oItemRate")));
            m.put("Amount", toDouble(ci(r, "oItemAmount")));
            m.put("Remarks", str(ci(r, "OtherItemRemarks")));
            others.add(m);
        }
        out.put("otherItems", others);
        out.put("otherItemCombo", repo.invoiceOtherItemCombo(u, invoiceId));
        List<Map<String, Object>> contract = new ArrayList<>();
        if (invoiceId > 0) {
            for (Map<String, Object> r : repo.invoiceData(u, "209", fy(), invoiceId)) {
                if (toInt(ci(r, "Id")) != invoiceId) continue;
                contract.add(row("SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")), "Customer", str(ci(r, "Customer")),
                        "DestinationPortId", toInt(ci(r, "DestinationPortId")), "DestinationPort", str(ci(r, "DestinationPort")),
                        "LoadingPortId", toInt(ci(r, "LoadingPortId")), "LoadingPort", str(ci(r, "LoadingPort"))));
            }
        }
        out.put("contract", contract);
        out.put("contractQueried", invoiceId > 0);
        return out;
    }

    /** cmbItem_Leave: CommonServices.GetUomScheduleByItemId. */
    public List<Map<String, Object>> uoms(int itemId) { return repo.uomScheduleByItem(view(), itemId); }

    // ============================================================================ read / history

    public Map<String, Object> readById(int id) {
        UserAccount u = view();
        Map<String, Object> h = owned(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        if (h == null) { out.put("header", null); return out; }
        out.put("header", h);
        out.put("details", repo.details(id));
        out.put("otherItems", repo.otherItems(id));
        int gpId = toInt(ci(h, "GatePassOutwardId"));
        out.put("noOfInvoices", repo.invoicesForGatePass(u, gpId, fy()).size() + 1);
        out.put("otherItemCombo", repo.invoiceOtherItemCombo(u, toInt(ci(h, "ExImInvoiceId"))));
        List<Map<String, Object>> gp = repo.gatePassDetail(u, gpId);
        out.put("gatePass", gp.isEmpty() ? null : gp.get(0));
        return out;
    }

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())) return null;
        return h;
    }

    /** GridBind: CanViewAllRecord = the EximForwarding right; no gate pass / invoice filter on this form. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = view();
        boolean all = allowed(u, "CanView AllRecord");
        Timestamp from = toBool(f.get("fromChecked")) ? day(f.get("fromDate"), false) : null;
        Timestamp to = toBool(f.get("toChecked")) ? day(f.get("toDate"), true) : null;
        String mode = str(f.get("dateMode"));
        List<Map<String, Object>> rows = repo.formHistory(u, fy(), all,
                "doc".equals(mode) ? from : null, "doc".equals(mode) ? to : null,
                "entry".equals(mode) ? from : null, "entry".equals(mode) ? to : null,
                "modify".equals(mode) ? from : null, "modify".equals(mode) ? to : null,
                "approved".equals(mode) ? from : null, "approved".equals(mode) ? to : null,
                toInt(f.get("customerId")), toInt(f.get("fromDocNo")), toInt(f.get("toDocNo")), 0, 0);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("DocNo", toInt(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("GPNo", toInt(ci(r, "GpNo")));
            m.put("GPDate", iso(ci(r, "GPDate")));
            m.put("PreInvoice#", str(ci(r, "InvoiceNo")));
            m.put("Customer/Importer", str(ci(r, "CustomerName")));
            m.put("TransporterName", str(ci(r, "TransporterName")));
            m.put("DriverName", str(ci(r, "DriverName")));
            m.put("DriverCellNo", str(ci(r, "DriverCellNo")));
            m.put("DriverCNIC", str(ci(r, "DriverCnicNo")));
            m.put("FreightAmount", toDouble(ci(r, "FreightAmt")));
            m.put("VehicleNo", str(ci(r, "VehicleNo")));
            m.put("BiltyNo", str(ci(r, "BiltyNo")));
            m.put("Remarks", str(ci(r, "OtherRemarks")));
            m.put("EntryUser", str(ci(r, "EntryUserName")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));          // ToShortDateString on the desktop
            m.put("ModifyUser", str(ci(r, "ModifyUserName")));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));
            m.put("ApprovedStatus", str(ci(r, "ApprovedStatus")));
            m.put("ApprovedUser", str(ci(r, "ApprovedUserName")));
            m.put("ApprovedDate", iso(ci(r, "ApprovedDate")));
            m.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = view();
        if (owned(u, id) == null) return new ArrayList<>();
        return repo.details(id);
    }

    // ============================================================================ save

    /**
     * Insert() / AutoUpdateRecord(). Desktop quirks kept: the removed saved rows (LstRemoveRecordDetail) are never
     * added to the list, so a deleted saved row is not deleted by an update; NetWeight must equal the DO weight
     * exactly; the PackingExpiryDate / ProductionNo messages carry the 0-based row index; other item Amount is
     * Conversion.ToInt (truncated); FactoryWeight is never set (OtherCharges carries the factory weight).
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = toInt(b.get("recId"));
        boolean auto = toBool(b.get("autoUpdate"));
        UserAccount u = right(recId > 0 ? "Update" : "Save");
        if (recId > 0 && owned(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> others = list(b.get("others"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Please Check Detail Grid");
        formValidation(b);
        double doInvoiceWt = toDouble(b.get("invoiceNetWeight"));
        double factoryWeightBal = toDouble(b.get("balFcWeight"));
        double sumNet = 0, sumStock = 0;
        for (Map<String, Object> r : rows) { sumNet += toDouble(r.get("NetWeight")); sumStock += toDouble(r.get("StockWeight")); }
        if (Math.abs(sumNet - doInvoiceWt) > 1e-9) throw new IllegalStateException("SumOfGrid NetWeight and DoNetWeight not equal please check");
        if (!auto && recId == 0 && allowed(u, "Save")) {
            if (toInt(b.get("noOfInvoices")) != 1 && sumStock > factoryWeightBal)
                throw new IllegalStateException("StockWeight cannot be greater than FactoryWeight please check");
        }

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("IsApproved", false);
        f.put("ApprovedDate", null);
        f.put("DocDate", at(b.get("docDate")));
        f.put("EntryDate", now);
        f.put("GPDate", at(b.get("gpDate")));
        f.put("LotCompletedDate", null);
        f.put("ModifyDate", now);
        f.put("FreightAmt", toDouble(b.get("freight")));
        f.put("OtherCharges", toDouble(b.get("factoryWeight")));
        f.put("FactoryWeight", 0d);
        f.put("ApprovedUser", 0);
        f.put("BranchesId", toInt(u.getBranchesId()));
        f.put("CompanyId", u.getCompanyId());
        f.put("DestinationPortId", toInt(b.get("destinationPortId")));
        f.put("DocNo", toInt(b.get("docNo")));
        f.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        f.put("EntryUser", recId == 0 ? toInt(u.getId()) : 0);
        f.put("ExImInvoiceId", toInt(b.get("invoiceId")));
        f.put("ExportSoNoId", 0);
        f.put("GatePassOutwardId", toInt(b.get("gpId")));
        f.put("Id", recId);
        f.put("LCOrderId", 0);
        f.put("LoadingPortId", toInt(b.get("loadingPortId")));
        f.put("ModifyUser", recId == 0 ? 0 : toInt(u.getId()));
        f.put("NoOfContainer", toInt(b.get("noOfContainer")));
        f.put("OrganizationId", u.getOrganizationId());
        f.put("ProjectsId", toInt(u.getBranchesId()));
        f.put("ShippedContainer", 0);
        f.put("ShippingAgentId", 0);
        f.put("AutoUpdateId", auto ? 1 : 0);
        f.put("ShippingLineId", 0);
        f.put("SupplierCustomerId", toInt(b.get("customerId")));
        f.put("TransporterId", toInt(b.get("transporterId")));
        f.put("CarrierTypeId", 0);
        f.put("FinancialYearId", fy());
        f.put("ActionId", recId == 0 ? 1 : 2);
        f.put("BiltyNo", str(b.get("biltyNo")));
        f.put("Container", str(b.get("container1")));
        f.put("Container1", str(b.get("container2")));
        f.put("DocAttachment", null);
        f.put("DriverCellNo", str(b.get("driverCellNo")));
        f.put("DriverCnicNo", str(b.get("cnicNo")));
        f.put("DriverName", str(b.get("driverName")));
        f.put("GpNo", str(b.get("gpNo")).trim());
        f.put("LotStatus", null);
        f.put("OtherRemarks", str(b.get("remarks")).trim());
        f.put("Status", null);
        f.put("VehicleNo", str(b.get("vehicleNo")));
        f.put("ReferenceNo", null);

        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            Map<String, Object> d = ExportForwardingService.detailModel();
            d.put("Id", toInt(r.get("Id")));
            d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("RefDocIdNo", toInt(r.get("RefDocIdNo")));
            d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            d.put("ContractDetailId", toInt(r.get("ProformaDetailId")));
            d.put("ExImLcOrderId", toInt(r.get("ProformaId")));
            d.put("InvoiceDetailId", toInt(r.get("PreInvoiceDetailId")));
            d.put("InvoiceId", toInt(r.get("PreInvoiceId")));
            if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required In Grid");
            d.put("ItemId", toInt(r.get("ItemId")));
            if (toInt(r.get("PackTypeId")) == 0) throw new IllegalArgumentException("Packing Type Required In Grid");
            d.put("PackingMaterialId", toInt(r.get("PackTypeId")));
            d.put("CropYearId", toInt(r.get("CropYearId")));
            d.put("_CropYear", str(r.get("CropYear")));
            if (toInt(r.get("CropYearId")) == 0) throw new IllegalArgumentException("CropYear Required In Grid");
            if (toDouble(r.get("NoOfBags")) == 0) throw new IllegalArgumentException("No Of Bags Required In Grid");
            d.put("OuterQty", toDouble(r.get("NoOfBags")));
            d.put("InnerQty", toDouble(r.get("NoOfBags")));
            if (toInt(r.get("OuterUOMId")) == 0) throw new IllegalArgumentException("Pack UOM Required In Grid");
            d.put("UOMScheduleIdOuter", toInt(r.get("OuterUOMId")));
            d.put("UOMScheduleIdInner", toInt(r.get("OuterUOMId")));
            if (toDouble(r.get("NetWeight")) == 0) throw new IllegalArgumentException("NetWeight Required In Grid");
            d.put("NetWeight", toDouble(r.get("NetWeight")));
            d.put("EbUnit", toDouble(r.get("EbUnit")));
            d.put("EbTotal", toDouble(r.get("EbTotal")));
            d.put("AdLsWeight", toDouble(r.get("AddLess")));
            if (toDouble(r.get("GrossWeight")) == 0) throw new IllegalArgumentException("Gross Weight Required In Grid");
            d.put("GrossWeight", toDouble(r.get("GrossWeight")));
            grossWeight += toDouble(r.get("GrossWeight"));
            d.put("StockWeight", toDouble(r.get("StockWeight")));
            if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("Job Lot Required In Grid");
            d.put("LotJobId", toInt(r.get("JobLotId")));
            if (toInt(r.get("WarehouseId")) == 0) throw new IllegalArgumentException("Warehouse From Required In Grid");
            d.put("WarehouseId", toInt(r.get("WarehouseId")));
            if (toInt(r.get("WarehouseToId")) == 0) throw new IllegalArgumentException("Warehouse To Required In Grid");
            d.put("WarehouseToId", toInt(r.get("WarehouseToId")));
            if (toInt(d.get("WarehouseId")) == toInt(d.get("WarehouseToId")))
                throw new IllegalArgumentException("Warehouse From and Warehouse To Cannot be Same In Grid");
            d.put("ContainerNo", str(r.get("Container#")).trim());
            d.put("SealNo", str(r.get("Seal#")).trim());
            d.put("PackingExpiryDate", str(r.get("PackingExpiryDate")).trim());
            d.put("ProductionNo", str(r.get("ProductionNo")).trim());
            if (str(d.get("PackingExpiryDate")).isEmpty()) throw new IllegalArgumentException("Packing Expiry Date Required in Detail Grid At Row#" + i);
            if (str(d.get("ProductionNo")).isEmpty()) throw new IllegalArgumentException("Production No Required in Detail Grid At Row#" + i);
            d.put("ActionTypeId", toInt(d.get("Id")) <= 0 ? 1 : 2);
            details.add(d);
        }
        List<Map<String, Object>> otherModels = new ArrayList<>();
        for (Map<String, Object> o : others) {
            Map<String, Object> m = ExportForwardingService.otherModel();
            if (toInt(o.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required In Other Items Grid");
            m.put("otherItemId", toInt(o.get("ItemId")));
            if (toInt(o.get("WarehouseFromId")) == 0) throw new IllegalArgumentException("WarehouseFrom To Required In Other Items Grid");
            m.put("WareHouseFromId", toInt(o.get("WarehouseFromId")));
            if (toInt(o.get("WarehouseToId")) == 0) throw new IllegalArgumentException("WarehouseTo To Required In Other Items Grid");
            m.put("WarehouseToId", toInt(o.get("WarehouseToId")));
            if (toInt(m.get("WareHouseFromId")) == toInt(m.get("WarehouseToId")))
                throw new IllegalArgumentException("Warehouse From and Warehouse To Cannot be Same In Other Items grid");
            m.put("oItemQty", toDouble(o.get("Qty")));
            m.put("oItemRate", toDouble(o.get("Rate")));
            int amount = toInt(o.get("Amount"));
            if (amount == 0) throw new IllegalArgumentException("Amount Required In Other Items Grid");
            m.put("oItemAmount", (double) amount);
            m.put("OtherItemRemarks", str(o.get("Remarks")));
            m.put("ContainerNo", str(o.get("ContainerNo")));
            otherModels.add(m);
        }
        int success = repo.saveForwarding(u, recId == 0 ? "Sp_ExImForwarding_Insert" : "Sp_ExImForwarding_Update", f, details, otherModels);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", success);
        out.put("message", auto ? "" : (recId > 0 ? "Record Update SuccessFully" : "Record Save SuccessFully"));
        out.put("openWages", !auto && toBool(repo.config(u, "WagesCompulsoryOnForwarding")) && repo.wagesActive(DOCUMENT_TYPE_ID));
        out.put("grossWeight", grossWeight);
        return out;
    }

    /** formValidation, in the desktop's order. */
    private void formValidation(Map<String, Object> b) {
        String docNo = str(b.get("docNo"));
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("Please Check Doc No");
        if (str(b.get("gpNo")).trim().isEmpty() || toInt(b.get("gpId")) == 0) throw new IllegalArgumentException("Please Select GatePass No");
        if (str(b.get("invoiceText")).trim().isEmpty() || toInt(b.get("invoiceId")) == 0) throw new IllegalArgumentException("Please Select Pre Invoice No");
        if (str(b.get("customerText")).trim().isEmpty() || toInt(b.get("customerId")) == 0) throw new IllegalArgumentException("Please Select Customer/Importer");
        if (str(b.get("driverName")).trim().isEmpty()) throw new IllegalArgumentException("Please Enter DriverName");
        if (str(b.get("driverCellNo")).trim().isEmpty()) throw new IllegalArgumentException("Please Enter Driver Cell No");
        if (str(b.get("cnicNo")).trim().isEmpty()) throw new IllegalArgumentException("Please Enter Driver CNIC No");
        if (str(b.get("vehicleNo")).trim().isEmpty()) throw new IllegalArgumentException("Please Check Vehicle No");
        if (str(b.get("factoryWeight")).trim().isEmpty() || toDouble(b.get("factoryWeight")) == 0) throw new IllegalArgumentException("Please Check Factory Weight");
        if (str(b.get("doGrossWeight")).trim().isEmpty() || toDouble(b.get("doGrossWeight")) == 0) throw new IllegalArgumentException("Please Check Do GrossWeight");
        if (str(b.get("doNetWeight")).trim().isEmpty() || toDouble(b.get("doNetWeight")) == 0) throw new IllegalArgumentException("Please Check Do NetWeight");
    }

    /** btnDelete_Click: InvPurchaseInvoice.RemoveByID with DocumentTypeId 210. */
    public Map<String, Object> delete(Map<String, Object> b) {
        UserAccount u = right("Delete");
        int recId = toInt(b.get("recId"));
        if (recId <= 0 || owned(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        repo.removeById(u, DOCUMENT_TYPE_ID, recId);
        return row("success", true, "message", "Delete Record Successfully");
    }

    public List<Integer> autoUpdateIds() {
        UserAccount u = right("Update");
        List<Integer> out = new ArrayList<>();
        for (Map<String, Object> r : repo.autoUpdateIds(u, DOCUMENT_TYPE_ID)) out.add(toInt(ci(r, "Id")));
        if (out.isEmpty()) throw new IllegalStateException("Ids Not Found For Update");
        return out;
    }
}
