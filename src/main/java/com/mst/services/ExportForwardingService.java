package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportForwardingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ExportForwardingRepository.ci;
import static com.mst.repositories.ExportForwardingRepository.str;
import static com.mst.repositories.ExportForwardingRepository.toBool;
import static com.mst.repositories.ExportForwardingRepository.toDouble;
import static com.mst.repositories.ExportForwardingRepository.toInt;

/**
 * BLL side of 212 "Export Forwarding" - Architecture.WinApp.Export.EximForwarding (ScreenName "EximForwarding",
 * DocumentTypeId 205, App 8 / Module 11). Rights: CompanyRights + ScreenRights + tblUserRights of ScreenDefinition
 * 212 (View to open, Save / Update / Print / Delete as formRights.DoHave...). Organisation, company, branch, user and
 * financial year come from the session, never from the request.
 *
 * The form's validations are repeated here with the desktop's texts and in the desktop's order, so a procedure is
 * never reached with what the form would have refused. The page repeats them first for the same messages.
 */
@Service
public class ExportForwardingService {

    public static final int SCREEN_ID = 212;
    public static final int DOCUMENT_TYPE_ID = 205;

    @Autowired private ExportForwardingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int fy() { return ctx.currentFinancialYearId(); }

    // ============================================================================ load

    /** ExpfrmForwarding_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        perm.put("Delete", allowed(u, "Delete"));
        out.put("permissions", perm);
        out.put("config", config(u));
        put(out, "docNo", () -> generateCode(u));
        put(out, "gatePasses", () -> gatePassCombo(u));
        put(out, "gatePassGrid", () -> repo.gatePassGrid(u, fy()));
        put(out, "packingTypes", () -> repo.packingTypes());
        put(out, "jobLots", () -> jobLots(u));
        put(out, "warehouses", () -> repo.activeWarehouses(u));
        put(out, "cropYears", () -> repo.cropYears(u));
        put(out, "ports", () -> ports(u));
        put(out, "parties", () -> parties(u));
        put(out, "items", () -> repo.exportItems(u));
        put(out, "historyCombos", () -> historyCombos(u));
        return out;
    }

    /** Configuration values the form reads in Load / btnRefresh. */
    public Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("WagesCompulsoryOnForwarding", toBool(repo.config(u, "WagesCompulsoryOnForwarding")));
        c.put("DefaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("StockLoaderVisibleInForwardingDetail", toBool(repo.config(u, "StockLoaderVisibleInForwardingDetail")));
        c.put("NetWeightAndStockWeightEqualOnForwarding", toBool(repo.config(u, "NetWeightAndStockWeightEqualOnForwarding")));
        c.put("SaleCostingJobOrderWise", toBool(repo.config(u, "SaleCostingJobOrderWise")));
        c.put("WagesActiveOrInActive", repo.wagesActive(DOCUMENT_TYPE_ID));
        c.put("ErpFeatureFifo", repo.erpFeature(u, 5));
        return c;
    }

    /** btnRefresh_Click: configuration, gate passes, parties, packing types, ports, job lots, warehouses. */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        put(out, "gatePasses", () -> gatePassCombo(u));
        put(out, "parties", () -> parties(u));
        put(out, "packingTypes", () -> repo.packingTypes());
        put(out, "ports", () -> ports(u));
        put(out, "jobLots", () -> jobLots(u));
        put(out, "warehouses", () -> repo.activeWarehouses(u));
        return out;
    }

    public int generateCode(UserAccount u) { return repo.generateCode(u, fy(), DOCUMENT_TYPE_ID); }

    /** FormReset pieces: doc no, gate passes and the Gate Pass information grid. */
    public Map<String, Object> reset() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "docNo", () -> generateCode(u));
        put(out, "gatePasses", () -> gatePassCombo(u));
        put(out, "gatePassGrid", () -> repo.gatePassGrid(u, fy()));
        return out;
    }

    private List<Map<String, Object>> gatePassCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingGatePasses(u, fy())) out.add(row("Id", toInt(ci(r, "Id")), "GpSrNo", str(ci(r, "GpSrNo"))));
        return out;
    }

    /** JobLotFill: SaleCostingJobOrderWise -> distinct job lots of usp_JobLot_GetWithJobOrderAndItem, else JobLotGetAllService. */
    private List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (toBool(repo.config(u, "SaleCostingJobOrderWise"))) {
            Set<Integer> seen = new LinkedHashSet<>();
            for (Map<String, Object> r : repo.jobLotsWithJobOrderAndItem(u)) {
                int id = toInt(ci(r, "Id"));
                if (seen.add(id)) out.add(row("Id", id, "JobLotDescription", str(ci(r, "JobLotDescription"))));
            }
        } else {
            for (Map<String, Object> r : repo.jobLots(u)) out.add(row("Id", toInt(ci(r, "Id")), "JobLotDescription", str(ci(r, "JobLotDescription"))));
        }
        return out;
    }

    /** bindSeaPort: PortType "Loading" / "Destination". */
    private Map<String, Object> ports(UserAccount u) {
        List<Map<String, Object>> load = new ArrayList<>(), dest = new ArrayList<>();
        for (Map<String, Object> r : repo.seaPorts(u)) {
            String t = str(ci(r, "PortType"));
            Map<String, Object> m = row("Id", toInt(ci(r, "Id")), "PortName", str(ci(r, "PortName")));
            if ("Loading".equals(t)) load.add(m);
            else if ("Destination".equals(t)) dest.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("loading", load);
        out.put("destination", dest);
        return out;
    }

    /** ShippingAgentFill: GetSupplierustomerByCustomerGroupId("10") binds Shipping Line, Shipping Agent and Transporter. */
    private List<Map<String, Object>> parties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.suppliersByGroup(u, "10")) out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName"))));
        return out;
    }

    /** HistoryCombosFill: Activity "Customer" / "GpNo" / "Invoice". */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> c = new ArrayList<>(), g = new ArrayList<>(), i = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u)) {
            String a = str(ci(r, "Activity"));
            Map<String, Object> m = row("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "ReferenceName")));
            if ("Customer".equals(a)) c.add(m); else if ("GpNo".equals(a)) g.add(m); else if ("Invoice".equals(a)) i.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", c);
        out.put("gatePasses", g);
        out.put("invoices", i);
        return out;
    }

    public Map<String, Object> historyCombosRefresh() { return historyCombos(user("View")); }

    // ============================================================================ gate pass / invoice events

    /**
     * cmbGpNo_Leave / cmbGpNo_TextChanged: GatePassDetailForForwardingByGpId (first row), the invoices of the gate pass
     * (GetInvoiceNoForExportForwarding) and GetContainersForForwarding (only with both ids).
     */
    public Map<String, Object> gatePass(int gpId, int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> gp = repo.gatePassDetail(u, gpId);
        out.put("gatePass", gp.isEmpty() ? null : gp.get(0));
        List<Map<String, Object>> inv = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicesForGatePass(u, gpId, fy())) inv.add(row("Id", toInt(ci(r, "Id")), "InvoiceNo", str(ci(r, "InvoiceNo"))));
        out.put("invoices", inv);
        out.put("containers", containers(gpId, invoiceId));
        return out;
    }

    private List<Map<String, Object>> containers(int gpId, int invoiceId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (invoiceId > 0 && gpId > 0) {
            for (Map<String, Object> r : repo.containers(gpId, invoiceId))
                out.add(row("Id", str(ci(r, "ContainerId")), "Container", str(ci(r, "Container")), "SealNo", str(ci(r, "SealNo"))));
        }
        return out;
    }

    /**
     * cmbInvoiceNo_Leave -> GetDeliveryOrderbyGatepassId + GetContainersForForwarding: GetInformationFromInvoice
     * (ExImInvoice.GetData Ids "204,209"), DeliveryOrderDetailForExportGdnForwarding, the invoice's other items
     * (ExImInvoice.GetByID) and the containers. The page builds the grid with the desktop's formulas.
     */
    public Map<String, Object> invoice(int gpId, int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> info = null;
        if (invoiceId > 0) {
            List<Map<String, Object>> all = repo.invoiceData(u, "204,209", fy(), 0);
            if (!all.isEmpty()) {
                Map<String, Object> found = null;
                for (Map<String, Object> r : all) if (toInt(ci(r, "Id")) == invoiceId) { found = r; break; }
                /* count stays 0 when the invoice is not in the list: the ports come from the FIRST row (desktop quirk). */
                Map<String, Object> dr = found != null ? found : all.get(0);
                info = new LinkedHashMap<>();
                info.put("found", found != null);
                info.put("SupplierCustomerId", found == null ? 0 : toInt(ci(found, "SupplierCustomerId")));
                info.put("Customer", found == null ? "" : str(ci(found, "Customer")));
                info.put("LoadingPortId", toInt(ci(dr, "LoadingPortId")));
                info.put("DestinationPortId", toInt(ci(dr, "DestinationPortId")));
                info.put("ShippingLineId", toInt(ci(dr, "ShippingLineId")));
                info.put("ShippingAgentId", toInt(ci(dr, "ShippingAgentId")));
                info.put("TransporterId", toInt(ci(dr, "TransporterId")));
            }
        }
        out.put("invoiceInfo", info);
        out.put("deliveryOrder", repo.deliveryOrderDetail(u, invoiceId, gpId));
        List<Map<String, Object>> others = new ArrayList<>();
        if (invoiceId > 0) {
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
        }
        out.put("otherItems", others);
        out.put("containers", containers(gpId, invoiceId));
        return out;
    }

    /** BindPackUomAndRateUom: CommonServices.dtUomFromGloablUomScheduleByItemId. */
    public List<Map<String, Object>> uoms(int itemId) { return repo.globalUomByItem(user("View"), itemId); }

    // ============================================================================ read / history

    /** ReadById: header, detail rows, other items, NoOfInvoices = GetInvoiceNoForExportForwarding(gp).Rows.Count + 1, gate pass data. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = ownedHeader(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        if (h == null) { out.put("header", null); return out; }
        out.put("header", h);
        out.put("details", repo.details(id));
        out.put("otherItems", repo.otherItems(id));
        int gpId = toInt(ci(h, "GatePassOutwardId"));
        int invoiceId = toInt(ci(h, "ExImInvoiceId"));
        out.put("noOfInvoices", repo.invoicesForGatePass(u, gpId, fy()).size() + 1);
        List<Map<String, Object>> gp = repo.gatePassDetail(u, gpId);
        out.put("gatePass", gp.isEmpty() ? null : gp.get(0));
        out.put("containers", containers(gpId, invoiceId));
        return out;
    }

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())) return null;
        return h;
    }

    /** GridBind: CanViewAllRecord true, date filter by the chosen radio, doc no range, gate pass, invoice, customer. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        Timestamp from = toBool(f.get("fromChecked")) ? day(f.get("fromDate"), false) : null;
        Timestamp to = toBool(f.get("toChecked")) ? day(f.get("toDate"), true) : null;
        String mode = str(f.get("dateMode"));
        List<Map<String, Object>> rows = repo.formHistory(u, fy(), true,
                "doc".equals(mode) ? from : null, "doc".equals(mode) ? to : null,
                "entry".equals(mode) ? from : null, "entry".equals(mode) ? to : null,
                "modify".equals(mode) ? from : null, "modify".equals(mode) ? to : null,
                "approved".equals(mode) ? from : null, "approved".equals(mode) ? to : null,
                toInt(f.get("customerId")), toInt(f.get("fromDocNo")), toInt(f.get("toDocNo")),
                toInt(f.get("gpId")), toInt(f.get("invoiceId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("DocNo", toInt(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("GpNo", toInt(ci(r, "GpNo")));
            m.put("GPDate", iso(ci(r, "GpDate")));
            m.put("InvoiceNo", str(ci(r, "InvoiceNo")));
            m.put("ExportVoucherNo", str(ci(r, "ExportVoucherNo")));
            m.put("CustomerName", str(ci(r, "CustomerName")));
            m.put("ShippingLine", str(ci(r, "ShippingLine")));
            m.put("DriverName", str(ci(r, "DriverName")));
            m.put("DriverCellNo", str(ci(r, "DriverCellNo")));
            m.put("DriverCnicNo", str(ci(r, "DriverCnicNo")));
            m.put("Transporter", str(ci(r, "TransporterName")));
            m.put("VehicleNo", str(ci(r, "VehicleNo")));
            m.put("BiltyNo", str(ci(r, "BiltyNo")));
            m.put("Freight", toDouble(ci(r, "FreightAmt")));
            m.put("EntryUser", str(ci(r, "EntryUserName")));
            m.put("EntryDate", isoTime(ci(r, "EntryDate")));
            m.put("ModifyUser", str(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoTime(ci(r, "ModifyDate")));
            m.put("ApprovedStatus", str(ci(r, "ApprovedStatus")));
            m.put("ApprovedUser", str(ci(r, "ApprovedUserName")));
            m.put("ApprovedDate", isoTime(ci(r, "ApprovedDate")));
            m.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GetDetailByHeaderId (DataGridHistory_SelectionChanged). */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = user("View");
        if (ownedHeader(u, id) == null) return new ArrayList<>();
        return repo.details(id);
    }

    // ============================================================================ save

    /**
     * Insert() (btnSave / btnUpdate) and AutoUpdateRecord(UpdateRecID) (the hidden "Auto Update Records" button):
     * the desktop's checks in its order, then ExImForwarding.Save (Id 0 -> ActionId 1, ModifyUser 0, Sp_ExImForwarding_Insert;
     * else ActionId 2, EntryUser 0, Sp_ExImForwarding_Update) -> DAL 0517 SetDate.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = toInt(b.get("recId"));
        boolean auto = toBool(b.get("autoUpdate"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        if (recId > 0 && ownedHeader(u, recId) == null) throw new IllegalArgumentException("Record not update because Id not found");
        if (!auto && recId > 0 && toBool(b.get("referredInExportVoucher")))
            throw new IllegalStateException("This Document is referred in Export Voucher. So you can't update this record.");
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        List<Map<String, Object>> others = list(b.get("others"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Please Check Detail Grid");
        formValidation(b);

        boolean netEqStock = toBool(repo.config(u, "NetWeightAndStockWeightEqualOnForwarding"));
        boolean jobOrderWise = toBool(repo.config(u, "SaleCostingJobOrderWise"));
        double doInvoiceWt = toDouble(b.get("invoiceNetWeight"));
        double factoryWeightBal = toDouble(b.get("balFcWeight"));
        double sumNet = 0, sumStock = 0;
        for (Map<String, Object> r : rows) { sumNet += toDouble(r.get("NetWeight")); sumStock += toDouble(r.get("StockWeight")); }
        if (auto) {
            if (sumNet != doInvoiceWt) throw new IllegalStateException("SumOfGrid NetWeight and DoNetWeight not equal please check");
        } else if (Math.abs(sumNet - doInvoiceWt) > 1d) {
            throw new IllegalStateException("SumOfGrid NetWeight and DoNetWeight not equal please check");
        }
        /* btnSave.Visible && btnSave.Enabled: only in insert mode with the Save right. One invoice: no check
           (the desktop's "StockWeight == FactoryWeight" branch is empty). */
        if (recId == 0 && allowed(u, "Save")) {
            if (toInt(b.get("noOfInvoices")) != 1 && sumStock > factoryWeightBal)
                throw new IllegalStateException("StockWeight cannot be greater than FactoryWeight please check");
        }
        if (!auto) validateLoaderRows(rows);

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> f = new LinkedHashMap<>();       // Model 0850 ExImForwarding, declaration order
        f.put("IsApproved", false);
        f.put("ApprovedDate", null);
        f.put("DocDate", at(b.get("docDate")));
        f.put("EntryDate", now);
        f.put("GPDate", at(b.get("gpDate")));
        f.put("LotCompletedDate", at(b.get("completedDate")));  // set even when "Date Active" is not ticked (desktop)
        f.put("ModifyDate", now);
        f.put("FreightAmt", toDouble(b.get("freight")));
        f.put("OtherCharges", toDouble(b.get("factoryWeight")));  // OtherCharges = Factory Weight (desktop)
        f.put("FactoryWeight", toDouble(b.get("factoryWeight")));
        f.put("ApprovedUser", 0);
        f.put("BranchesId", auto ? 0 : toInt(u.getBranchesId()));    // AutoUpdateRecord never sets BranchesId
        f.put("CompanyId", u.getCompanyId());
        f.put("DestinationPortId", toInt(b.get("destinationPortId")));
        f.put("DocNo", toInt(b.get("docNo")));
        f.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        f.put("EntryUser", recId == 0 ? toInt(u.getId()) : 0);       // BLL Save: EntryUser 0 on update
        f.put("ExImInvoiceId", toInt(b.get("invoiceId")));
        f.put("ExportSoNoId", 0);
        f.put("GatePassOutwardId", toInt(b.get("gpId")));
        f.put("Id", recId);
        f.put("LCOrderId", 0);
        f.put("LoadingPortId", toInt(b.get("loadingPortId")));
        f.put("ModifyUser", recId == 0 ? 0 : toInt(u.getId()));      // BLL Save: ModifyUser 0 on insert
        f.put("NoOfContainer", toInt(b.get("noOfContainer")));
        f.put("OrganizationId", u.getOrganizationId());
        f.put("ProjectsId", 0);
        f.put("ShippedContainer", toInt(b.get("shippedContainer")));
        f.put("ShippingAgentId", toInt(b.get("shippingAgentId")));
        f.put("AutoUpdateId", auto ? 1 : 0);
        f.put("ShippingLineId", toInt(b.get("shippingLineId")));
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
        f.put("OtherRemarks", null);
        f.put("Status", null);
        f.put("VehicleNo", str(b.get("vehicleNo")));
        f.put("ReferenceNo", null);

        List<Map<String, Object>> details = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) details.add(removedModel(r));
        String c1 = str(b.get("container1")), c2 = str(b.get("container2"));
        double grossWeight = 0;
        int invoiceId = toInt(b.get("invoiceId"));
        List<Map<String, Object>> jobItems = jobOrderWise ? repo.jobLotsWithJobOrderAndItem(u) : null;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int n = i + 1;
            Map<String, Object> d = detailModel();
            d.put("Id", toInt(r.get("Id")));
            d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("RefDocIdNo", toInt(r.get("RefDocIdNo")));
            d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            d.put("ExImLcOrderId", toInt(r.get("ContractId")));
            if (toInt(d.get("ExImLcOrderId")) == 0) throw new IllegalStateException("ContractId Not found in row#" + n);
            d.put("ContractDetailId", toInt(r.get("ContractDetailId")));
            if (toInt(d.get("ContractDetailId")) == 0) throw new IllegalStateException("Contract DetailId Not found in row#" + n);
            d.put("InvoiceId", invoiceId);
            d.put("InvoiceDetailId", toInt(r.get("InvoiceDetailId")));
            if (toInt(d.get("InvoiceDetailId")) == 0) throw new IllegalStateException("Invoice Detail Id Not found in row#" + n);
            if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required in row#" + n);
            d.put("ItemId", toInt(r.get("ItemId")));
            String itemName = str(r.get("ItemName")).trim();
            d.put("ItemDescription", str(r.get("ItemDescription")).trim());
            if (toInt(r.get("PackTypeId")) == 0) throw new IllegalArgumentException("Packing Type Required in row#" + n);
            d.put("PackingMaterialId", toInt(r.get("PackTypeId")));
            if (toInt(r.get("MasterUOMId")) == 0) throw new IllegalArgumentException("Master UOM Required In Grid");
            d.put("UOMScheduleIdOuter", toInt(r.get("MasterUOMId")));
            d.put("EbUnit", toDouble(r.get("EbUnitMaster")));
            d.put("EbTotal", toDouble(r.get("EbTotalMaster")));
            if (toDouble(r.get("MasterQTY")) == 0) throw new IllegalArgumentException("Master QTY Required in row#" + n);
            d.put("OuterQty", toDouble(r.get("MasterQTY")));
            d.put("InnerQty", toDouble(r.get("InnerQTY")));
            d.put("UOMScheduleIdInner", toInt(r.get("InnerUOMId")));
            d.put("InnerEbUnit", toDouble(r.get("EbUnitInner")));
            d.put("InnerEbTotal", toDouble(r.get("EbTotalInner")));
            d.put("TotalPackingWeight", toDouble(d.get("InnerEbTotal")) + toDouble(d.get("EbTotal")));
            d.put("AdLsWeight", toDouble(r.get("AddLess")));
            if (toDouble(r.get("NetWeight")) == 0) throw new IllegalArgumentException("NetWeight Required in row#" + n);
            d.put("NetWeight", toDouble(r.get("NetWeight")));
            if (toDouble(r.get("GrossWeight")) == 0) throw new IllegalArgumentException("Gross Weight Required in row#" + n);
            d.put("GrossWeight", toDouble(r.get("GrossWeight")));
            grossWeight += toDouble(r.get("GrossWeight"));
            d.put("StockWeight", netEqStock ? toDouble(r.get("NetWeight")) : toDouble(r.get("StockWeight")));
            d.put("CropYearId", toInt(r.get("CropYearId")));
            d.put("_CropYear", str(r.get("CropYear")));
            if (toInt(d.get("CropYearId")) == 0) throw new IllegalArgumentException("CropYear Required in row#" + n);
            if (toInt(r.get("WarehouseId")) == 0) throw new IllegalArgumentException("Warehouse Required in row#" + n);
            d.put("WarehouseId", toInt(r.get("WarehouseId")));
            if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("Job Lot Required in row#" + n);
            d.put("LotJobId", toInt(r.get("JobLotId")));
            if (!auto && jobItems != null) itemIdExistsInJobLot(jobItems, toInt(r.get("ItemId")), itemName, toInt(r.get("JobLotId")), str(r.get("JobLot")));
            String container = str(r.get("Container#"));
            /* Insert(): either container box filled -> container required; AutoUpdateRecord(): both filled. */
            boolean need = auto ? (!c1.isEmpty() && !c2.isEmpty()) : (!c1.isEmpty() || !c2.isEmpty());
            if (need && container.isEmpty()) throw new IllegalArgumentException("Container Required in row#" + n);
            d.put("ContainerNo", container.trim());
            if (!auto) {
                d.put("ContainerId", toInt(r.get("ContainerId")));
                d.put("DoDetailId", toInt(r.get("DoDetailId")));
            }
            if (str(r.get("Seal#")).isEmpty()) throw new IllegalArgumentException("Seal No Required in row#" + n);
            d.put("SealNo", str(r.get("Seal#")).trim());
            d.put("ActionTypeId", toInt(d.get("Id")) <= 0 ? 1 : 2);
            details.add(d);
        }
        List<Map<String, Object>> otherModels = new ArrayList<>();
        for (Map<String, Object> o : others) {
            Map<String, Object> m = otherModel();
            m.put("otherItemId", toInt(o.get("ItemId")));
            m.put("oItemQty", toDouble(o.get("Qty")));
            m.put("oItemRate", toDouble(o.get("Rate")));
            m.put("oItemAmount", toDouble(o.get("Amount")));
            m.put("OtherItemRemarks", str(o.get("Remarks")));
            m.put("ContainerNo", str(o.get("ContainerNo")));
            otherModels.add(m);
        }
        int success = repo.saveForwarding(u, recId == 0 ? "Sp_ExImForwarding_Insert" : "Sp_ExImForwarding_Update", f, details, otherModels);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", success);
        out.put("message", auto ? "" : (recId > 0 ? "Record Update SuccessFully" : "Record Save SuccessFully"));
        boolean wages = toBool(repo.config(u, "WagesCompulsoryOnForwarding")) && repo.wagesActive(DOCUMENT_TYPE_ID);
        out.put("openWages", !auto && wages);
        out.put("grossWeight", grossWeight);
        return out;
    }

    /** formValidation - the header checks in the desktop's order. */
    private void formValidation(Map<String, Object> b) {
        String docNo = str(b.get("docNo"));
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("Please Check Doc No");
        if (str(b.get("invoiceText")).trim().isEmpty() || toInt(b.get("invoiceId")) == 0) throw new IllegalArgumentException("Please Select Invoice No");
        if (str(b.get("customerText")).trim().isEmpty() || toInt(b.get("customerId")) == 0) throw new IllegalArgumentException("Please Select Customer");
        if (str(b.get("gpNo")).trim().isEmpty() || toInt(b.get("gpId")) == 0) throw new IllegalArgumentException("Please Select GatePass No");
        if (str(b.get("vehicleNo")).trim().isEmpty()) throw new IllegalArgumentException("Please Check Vehicle No");
        if (toInt(b.get("loadingPortId")) == 0) throw new IllegalArgumentException("Please Select Loading Port");
        if (toInt(b.get("destinationPortId")) == 0) throw new IllegalArgumentException("Please Select Destination Port");
        if (str(b.get("noOfContainer")).trim().isEmpty() || toInt(b.get("noOfContainer")) == 0) throw new IllegalArgumentException("Please Check No Of Container");
        if (str(b.get("factoryWeight")).trim().isEmpty() || toDouble(b.get("factoryWeight")) == 0) throw new IllegalArgumentException("Please Check Factory Weight");
        if (str(b.get("doGrossWeight")).trim().isEmpty() || toDouble(b.get("doGrossWeight")) == 0) throw new IllegalArgumentException("Please Check Do GrossWeight");
        if (str(b.get("doNetWeight")).trim().isEmpty() || toDouble(b.get("doNetWeight")) == 0) throw new IllegalArgumentException("Please Check Do NetWeight");
    }

    /** ValidateDetailRecordsWithorWithoutLoader. */
    private static void validateLoaderRows(List<Map<String, Object>> rows) {
        int idx = -1;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (toInt(r.get("RefDocumentTypeId")) > 0 || toInt(r.get("RefDocIdNo")) > 0 || toInt(r.get("RefDocSubIdNo")) > 0) { idx = i; break; }
        }
        if (idx < 0) return;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (toInt(r.get("RefDocumentTypeId")) == 0 || toInt(r.get("RefDocIdNo")) == 0 || toInt(r.get("RefDocSubIdNo")) == 0)
                throw new IllegalStateException("All rows in the detail grid must either be from the loader or none should be from the loader.\n"
                        + "Row No: " + (i + 1) + " does not have loader values, while Row No: " + (idx + 1) + " has loader values.");
        }
    }

    /** ItemIdExistsInJobLot (only with SaleCostingJobOrderWise). */
    static void itemIdExistsInJobLot(List<Map<String, Object>> jobItems, int itemId, String itemName, int jobLotId, String jobLotName) {
        for (Map<String, Object> r : jobItems) if (toInt(ci(r, "Id")) == jobLotId && toInt(ci(r, "ItemId")) == itemId) return;
        throw new IllegalStateException("The selected item '" + itemName + "' does not exist against the job lot '" + jobLotName + "'.");
    }

    /** grdDetail_ColumnButtonClick "Delete" on a saved row - the fields the desktop copies, ActionTypeId 3. */
    private static Map<String, Object> removedModel(Map<String, Object> r) {
        Map<String, Object> d = detailModel();
        d.put("Id", toInt(r.get("Id")));
        d.put("ExImLcOrderId", toInt(r.get("ContractId")));
        d.put("ContractDetailId", toInt(r.get("ContractDetailId")));
        d.put("InvoiceDetailId", toInt(r.get("InvoiceDetailId")));
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("PackingMaterialId", toInt(r.get("PackTypeId")));
        d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
        d.put("RefDocIdNo", toInt(r.get("RefDocIdNo")));
        d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
        d.put("CropYearId", toInt(r.get("CropYearId")));
        d.put("UOMScheduleIdOuter", toInt(r.get("MasterUOMId")));
        d.put("EbUnit", toDouble(r.get("EbUnitMaster")));
        d.put("EbTotal", toDouble(r.get("EbTotalMaster")));
        d.put("InnerQty", toDouble(r.get("InnerQTY")));
        d.put("UOMScheduleIdInner", toInt(r.get("InnerUOMId")));
        d.put("InnerEbUnit", toDouble(r.get("EbUnitInner")));
        d.put("InnerEbTotal", toDouble(r.get("EbTotalInner")));
        d.put("TotalPackingWeight", toDouble(d.get("InnerEbTotal")) + toDouble(d.get("EbTotal")));
        d.put("AdLsWeight", toDouble(r.get("AddLess")));
        d.put("NetWeight", toDouble(r.get("NetWeight")));
        d.put("GrossWeight", toDouble(r.get("GrossWeight")));
        d.put("StockWeight", toDouble(r.get("StockWeight")));
        d.put("WarehouseId", toInt(r.get("WarehouseId")));
        d.put("ContainerNo", str(r.get("Container#")).trim());
        d.put("ContainerId", toInt(r.get("ContainerId")));
        d.put("DoDetailId", toInt(r.get("DoDetailId")));
        d.put("SealNo", str(r.get("Seal#")).trim());
        d.put("LotJobId", toInt(r.get("JobLotId")));
        d.put("ItemDescription", str(r.get("ItemDescription")).trim());
        d.put("ActionTypeId", 3);
        return d;
    }

    /** Model 0851 ExImForwardingPackingDetail - every non-virtual property, declaration order, at its default. */
    static Map<String, Object> detailModel() {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[]{"AdLsWeight", "GrossWeight", "InnerQty", "NetWeight", "StockWeight", "OuterQty", "EbUnit", "EbTotal",
                "InnerEbUnit", "InnerEbTotal", "TotalPackingWeight"}) d.put(k, 0d);
        for (String k : new String[]{"ExImForwardingId", "ExImLcOrderId", "DoDetailId", "Id", "ItemId", "LotJobId", "CropYearId",
                "PackingMaterialId", "UOMScheduleIdInner", "UOMScheduleIdOuter", "WarehouseId", "RefDocumentTypeId", "RefDocIdNo",
                "RefDocSubIdNo", "WarehouseToId", "ContractDetailId", "InvoiceId", "InvoiceDetailId", "LineId", "ActionTypeId", "ContainerId"}) d.put(k, 0);
        for (String k : new String[]{"ContainerNo", "ItemDescription", "LabReportNo", "OtherInfo", "OuterPackDescription", "SealNo",
                "ProductionNo", "PackingExpiryDate"}) d.put(k, null);
        return d;
    }

    /** Model 0829 ExImForwardingOtherItems - every non-virtual property at its default. */
    static Map<String, Object> otherModel() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("oItemAmount", 0d); m.put("oItemQty", 0d); m.put("oItemWeightKgs", 0d); m.put("oItemRate", 0d);
        m.put("ExImForwardingId", 0); m.put("Id", 0); m.put("otherItemId", 0); m.put("InvoiceOtherItemsDetailId", 0);
        m.put("ExImInvoiceId", 0); m.put("WareHouseFromId", 0); m.put("WarehouseToId", 0);
        m.put("ContainerNo", null); m.put("OtherItemRemarks", null);
        return m;
    }

    // ============================================================================ delete / auto update / stock

    /** btnDelete_Click: referred in an export voucher -> refused; else InvPurchaseInvoice.RemoveByID (DocumentTypeId 205). */
    public Map<String, Object> delete(Map<String, Object> b) {
        UserAccount u = user("Delete");
        int recId = toInt(b.get("recId"));
        if (recId > 0 && toBool(b.get("referredInExportVoucher")))
            throw new IllegalStateException("This Document is referred in Export Voucher. So you can't delete this record.");
        if (recId <= 0 || ownedHeader(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        repo.removeById(u, DOCUMENT_TYPE_ID, recId);
        return row("success", true, "message", "Delete Record Successfully");
    }

    /** btnRecordsUpdate_Click: the ids to re-save; the page loads and re-saves each one as the desktop does. */
    public List<Integer> autoUpdateIds() {
        UserAccount u = user("Update");
        List<Integer> out = new ArrayList<>();
        for (Map<String, Object> r : repo.autoUpdateIds(u, DOCUMENT_TYPE_ID)) out.add(toInt(ci(r, "Id")));
        if (out.isEmpty()) throw new IllegalStateException("Ids Not Found For Update");
        return out;
    }

    /**
     * btnGenerateAvailableStock_Click: ERP feature 5 -> sum of BalWeight of GetStockByFifoMethod, else BalWeight of the
     * first row of GetAvailableStock (ActionId 1); one value per grid row, in row order.
     */
    public List<Double> availableStock(Map<String, Object> b) {
        UserAccount u = user("View");
        boolean fifo = repo.erpFeature(u, 5);
        Timestamp docDate = at(b.get("docDate"));
        List<Double> out = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("rows"))) {
            int itemId = toInt(r.get("ItemId")), wh = toInt(r.get("WarehouseId")), jl = toInt(r.get("JobLotId")), pt = toInt(r.get("PackTypeId"));
            String crop = str(r.get("CropYear"));
            double w = 0;
            if (fifo) {
                for (Map<String, Object> s : repo.stockByFifo(u, itemId, docDate, wh, crop, jl, pt, toInt(r.get("MasterUOMId"))))
                    w += toDouble(ci(s, "BalWeight"));
            } else {
                List<Map<String, Object>> s = repo.availableStock(u, itemId, docDate, wh, jl, crop, pt, 1);
                if (!s.isEmpty()) w = toDouble(ci(s.get(0), "BalWeight"));
            }
            out.add(w);
        }
        return out;
    }

    // ============================================================================ helpers

    interface Loader { Object load() throws Exception; }

    static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", root(e)); }
    }

    static String root(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e) : t.getMessage();
    }

    static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add(new LinkedHashMap<>((Map<String, Object>) o));
        return out;
    }

    /** A DateTimePicker value: the chosen date at the current time of day (the picker keeps DateTime.Now's time). */
    static Timestamp at(Object v) {
        LocalDate d = date(v);
        if (d == null) d = LocalDate.now();
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    /** History filter date: the picker value (DateTime.Now's time, as the desktop sends it). */
    static Timestamp day(Object v, boolean end) {
        LocalDate d = date(v);
        if (d == null) return null;
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    static LocalDate date(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception e) { return null; }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    static String isoTime(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        return String.valueOf(v);
    }
}
