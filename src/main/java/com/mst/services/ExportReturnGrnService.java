package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.ExportReturnRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportMSupport.*;

/**
 * BLL side of 190 ExportReturn_Grn "Export Return GRN" (Architecture.WinApp.Export.ExportReturn_Grn, ScreenName
 * "ExportReturn_Grn", DocumentTypeId 241) and its popup frmLoadExportReurnInvoiceForGrn (reverse flow).
 *
 * Normal flow: Inward Gp # (pending inward gate passes, DocumentTypeId 51) + Outward Gp # (pending export forwardings)
 * -> the forwarding's packing rows fill the detail grid; rows are edited through the detail panel (double-click / Edit),
 * duplicated with "+", removed with "X". Reverse flow (EnableExportReturnReverseFlow): "Load Invoices" opens the
 * return-invoice loader, Bags / Gross / EbUnit / AddLess edit in the grid and the save posts a cost voucher.
 * Rights from ScreenDefinition.Id 190; tenancy and user from the session only.
 */
@Service
public class ExportReturnGrnService {

    public static final int SCREEN_ID = 190;
    public static final int DOCUMENT_TYPE_ID = 241;

    @Autowired private ExportReturnRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount viewer() {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }
    private int fy() { return ctx.currentFinancialYearId(); }

    // ================================================================== load

    /** InitializeComponentMethod. */
    public Map<String, Object> setup() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", right(ctx, rights, u, SCREEN_ID, "Save"));
        perm.put("Update", right(ctx, rights, u, SCREEN_ID, "Update"));
        perm.put("Print", right(ctx, rights, u, SCREEN_ID, "Print"));
        perm.put("Delete", right(ctx, rights, u, SCREEN_ID, "Delete"));
        out.put("permissions", perm);
        Map<String, Object> cfg = config(u);
        out.put("config", cfg);
        out.put("docNo", repo.grnGenerateCode(u, fy(), DOCUMENT_TYPE_ID));
        out.put("gatePasses", gatePassCombo(repo.pendingInwardGatePasses(u, fy(), 0, 0)));
        if (!asBool(cfg.get("enableExportReturnReverseFlow"))) out.put("forwardings", forwardingCombo(repo.pendingForwardings(u, fy(), 0)));
        out.putAll(globals(u));
        out.put("otherItemList", otherItemList(u, 0));                    // dtOtherItemFillDBCall(cmbForwardingNo.Value = 0)
        out.put("history", historyCombos(u));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        return out;
    }

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("allItemsBindonExportContract", asBool(repo.config(u, "AllItemsBindonExportContract")));
        c.put("enableExportReturnReverseFlow", asBool(repo.config(u, "EnableExportReturnReverseFlow")));
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("decAmount", digits(repo.config(u, "Default NoofDecimal Points For Amount")));
        return c;
    }

    /** SupplierDtFillFromGlobal (customers: group 7 not sub; salesmen: group 10), PortsDbCall, the global detail combos. */
    private Map<String, Object> globals(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> customers = new ArrayList<>(), salesmen = new ArrayList<>();
        for (Map<String, Object> r : repo.globalSupplierCustomers(u)) {
            int g = asInt(ci(r, "CustomerGroupId"));
            boolean isSupplier = g == 7 && !asBool(ci(r, "IsSubSupCust"));
            boolean isSalesman = g == 10;
            if (!isSupplier && !isSalesman) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("CompanyName", text(ci(r, "CompanyName")));
            m.put("PartyCode", text(ci(r, "PartyCode")));
            m.put("CityName", text(ci(r, "CityName")));
            m.put("MobileNo", text(ci(r, "MobilePersonal")));
            if (isSupplier) customers.add(m);
            if (isSalesman) salesmen.add(m);
        }
        out.put("customers", customers);
        out.put("salesmen", salesmen);
        List<Map<String, Object>> ports = new ArrayList<>();
        for (Map<String, Object> r : repo.seaPorts(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("PortName", text(ci(r, "PortName")));
            ports.add(m);
        }
        out.put("ports", ports);
        out.put("warehouses", pairs(repo.warehouses(u), "Id", "WarehouseName", "WareHouseName"));
        out.put("cropYears", pairs(repo.cropYears(u), "Id", "CropYear", "CropYear"));
        out.put("jobLots", pairs(repo.jobLotsGlobal(u), "Id", "JobLotDescription", "JobLotDescription"));
        out.put("packTypes", pairs(repo.packingTypes(), "Id", "PackTypeDesc", "PackTypeDesc"));
        return out;
    }

    private static List<Map<String, Object>> pairs(List<Map<String, Object>> rows, String idCol, String nameCol, String outName) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put(outName, text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    /** InwardGatePassNoBind: Id, GpSrNo, OrderType. */
    private static List<Map<String, Object>> gatePassCombo(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("GpSrNo", text(ci(r, "GpSrNo")));
            m.put("OrderType", text(ci(r, "OrderType")));
            out.add(m);
        }
        return out;
    }

    /** ForwardingNoBind: GatePassOutwardId, GpNo, GpDate, ForwardingId (hidden), ForwardingNo, ForwardingDate. */
    private static List<Map<String, Object>> forwardingCombo(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GatePassOutwardId", asInt(ci(r, "GatePassOutwardId")));
            m.put("GpNo", text(ci(r, "GpNo")));
            m.put("GpDate", iso(ci(r, "GpDate")));
            m.put("ForwardingId", asInt(ci(r, "ForwardingId")));
            m.put("ForwardingNo", text(ci(r, "ForwardingNo")));
            m.put("ForwardingDate", iso(ci(r, "ForwardingDate")));
            out.add(m);
        }
        return out;
    }

    /** dtOtherItemFillDBCall(id): GetOtherItemByExImInvoiceId -> ItemId, ItemName, ItemCode. */
    private List<Map<String, Object>> otherItemList(UserAccount u, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItemsByInvoice(u, id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "ItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            out.add(m);
        }
        return out;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> cfg = config(u);
        out.put("config", cfg);
        out.put("gatePasses", gatePassCombo(repo.pendingInwardGatePasses(u, fy(), 0, recId)));
        if (!asBool(cfg.get("enableExportReturnReverseFlow"))) out.put("forwardings", forwardingCombo(repo.pendingForwardings(u, fy(), recId)));
        out.putAll(globals(u));
        return out;
    }

    /** FormReset: GetDocumentCode, ForwardingNoBind(ForwardingNoDBCall()), InwardGatePassNoBind(.., RecId = 0). */
    public Map<String, Object> reset() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.grnGenerateCode(u, fy(), DOCUMENT_TYPE_ID));
        if (!asBool(repo.config(u, "EnableExportReturnReverseFlow"))) out.put("forwardings", forwardingCombo(repo.pendingForwardings(u, fy(), 0)));
        out.put("gatePasses", gatePassCombo(repo.pendingInwardGatePasses(u, fy(), 0, 0)));
        return out;
    }

    /** GatePassDataForExportReturnGrn(GpId). */
    public Map<String, Object> gatePass(int gpId, int recId) {
        UserAccount u = viewer();
        List<Map<String, Object>> r = repo.pendingInwardGatePasses(u, fy(), gpId, recId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", !r.isEmpty());
        if (!r.isEmpty()) {
            Map<String, Object> g = r.get(0);
            out.put("VehicleNo", text(ci(g, "VehicleNo")));
            out.put("BiltyNo", text(ci(g, "BiltyNo")));
            out.put("GpDate", iso(ci(g, "GpDate")));
            out.put("NetPaid", text(ci(g, "NetPaid")));
            out.put("FactoryWeight", text(ci(g, "FactoryWeight")));
            out.put("Container", text(ci(g, "Container")));
            out.put("Container1", text(ci(g, "Container1")));
        }
        return out;
    }

    /**
     * FillDetailFromExportForwarding: ExImForwarding.GetByID (header + ReadByForwardingHeaderId rows + other items),
     * projected to dtDetail's columns; other items only when the forwarding has some, with the other-item list
     * re-read by dtOtherItemFillDBCall(cmbForwardingNo.Value) - the forwarding id passed as the invoice id (desktop quirk).
     */
    public Map<String, Object> forwarding(int forwardingId) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> hs = repo.forwardingHeader(forwardingId);
        List<Map<String, Object>> ds = hs.isEmpty() ? new ArrayList<>() : repo.forwardingDetail(forwardingId);
        out.put("found", !hs.isEmpty() && !ds.isEmpty());
        if (hs.isEmpty() || ds.isEmpty()) return out;
        Map<String, Object> h = hs.get(0);
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("Id", asInt(ci(h, "Id")));
        head.put("SupplierCustomerId", asInt(ci(h, "SupplierCustomerId")));
        head.put("Customer", text(ci(h, "Customer")));
        head.put("DestinationPortId", asInt(ci(h, "DestinationPortId")));
        head.put("DestinationPort", text(ci(h, "DestinationPort")));
        head.put("LoadingPortId", asInt(ci(h, "LoadingPortId")));
        head.put("LoadingPort", text(ci(h, "LoadingPort")));
        out.put("header", head);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : ds) {
            Map<String, Object> m = blankRow();
            m.put("Id", 0);
            m.put("ContractId", asInt(ci(d, "ExImLcOrderId")));
            m.put("ContractDetailId", asInt(ci(d, "ContractDetailId")));
            m.put("ContractNo", text(ci(d, "LcOrderNo")));
            m.put("ContractDate", iso(ci(d, "ContractDate")));
            m.put("InvoiceId", asInt(ci(d, "InvoiceId")));
            m.put("InvoiceDetailId", asInt(ci(d, "InvoiceDetailId")));
            m.put("InvoiceNo", text(ci(d, "InvoiceNo")));
            m.put("ExImForwardingDetailId", asInt(ci(d, "Id")));
            m.put("WarehouseId", asInt(ci(d, "WarehouseId")));
            m.put("Warehouse", text(ci(d, "WareHouseName")));
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemCode", text(ci(d, "ItemCode")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("JobLotId", asInt(ci(d, "LotJobId")));
            m.put("JobLot", text(ci(d, "JobLotName")));
            m.put("PackingTypeId", asInt(ci(d, "PackingMaterialId")));
            m.put("PackingType", text(ci(d, "Packtype")));
            m.put("PackUomId", asInt(ci(d, "UOMScheduleIdOuter")));
            m.put("PackUomCode", text(ci(d, "OuterUOM")));
            m.put("PackUomEquivalent", asDouble(ci(d, "OuterEquivalent")));
            m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            m.put("EbUnit", asDouble(ci(d, "EbUnit")));
            m.put("EbTotal", asDouble(ci(d, "EbTotal")));
            m.put("AddLess", asDouble(ci(d, "AdLsWeight")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("StockWeight", asDouble(ci(d, "StockWeight")));
            m.put("ItemDescription", text(ci(d, "ItemDescription")));
            m.put("Container#", text(ci(d, "ContainerNo")));
            m.put("Seal#", text(ci(d, "SealNo")));
            m.put("CostRate", 0d);
            rows.add(m);
        }
        out.put("rows", rows);
        List<Map<String, Object>> others = new ArrayList<>();
        for (Map<String, Object> o : repo.forwardingOtherItems(forwardingId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("ItemId", asInt(ci(o, "otherItemId")));
            /* dtOtherItemdetail.Rows.Add(0, otherItemId, ItemName, ItemCode, ...) into columns (Id, ItemId, ItemCode, ItemName, ..):
               the name lands in ItemCode and the code in ItemName (desktop quirk, kept). */
            m.put("ItemCode", text(ci(o, "ItemName")));
            m.put("ItemName", text(ci(o, "ItemCode")));
            m.put("Qty", asDouble(ci(o, "oItemQty")));
            m.put("Rate", asDouble(ci(o, "oItemRate")));
            m.put("Amount", asDouble(ci(o, "oItemAmount")));
            m.put("ContainerNo", null);                                     // txtconainer1.Text - filled by the page
            m.put("Remarks", text(ci(o, "OtherItemRemarks")));
            others.add(m);
        }
        out.put("otherItems", others);
        if (!others.isEmpty()) out.put("otherItemList", otherItemList(u, forwardingId));
        return out;
    }

    private static Map<String, Object> blankRow() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : new String[]{"Id", "ContractId", "ContractDetailId", "ContractNo", "ContractDate", "InvoiceId", "InvoiceDetailId", "InvoiceNo",
                "ExImForwardingDetailId", "ReturnInvoiceId", "ReturnInvoiceDetailId", "ReturnInvoiceNo", "WarehouseId", "Warehouse", "ItemId",
                "ItemCode", "ItemName", "CropYearId", "CropYear", "JobLotId", "JobLot", "PackingTypeId", "PackingType", "PackUomId", "PackUomCode",
                "PackUomEquivalent", "NoOfBags", "GrossWeight", "EbUnit", "EbTotal", "AddLess", "NetWeight", "StockWeight", "ItemDescription",
                "Container#", "Seal#", "CostRate"}) m.put(k, 0);
        return m;
    }

    /** CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId): Id, UOMCode, Equivalent, BaseRateUom, BasePackUom. */
    public List<Map<String, Object>> itemUoms(int itemId) {
        UserAccount u = viewer();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allUoms(u)) {
            if (asInt(ci(r, "ItemId")) != itemId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("UOMCode", text(ci(r, "UOMCode")));
            m.put("Equivalent", asDouble(ci(r, "Equivalent")));
            m.put("BaseRateUom", asBool(ci(r, "BaseRateUom")));
            m.put("BasePackUom", asBool(ci(r, "BasePackUom")));
            out.add(m);
        }
        return out;
    }

    /** ExImForwarding.ExportForwardingDataByInvoiceId (reverse flow: shipping agent / line / transporter and the F1 container / seal lists). */
    public List<Map<String, Object>> forwardingData(int exImInvoiceId) {
        UserAccount u = viewer();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.forwardingDataByInvoice(u, fy(), exImInvoiceId)) out.add(ExportReturnInvoiceService.plain(r));
        return out;
    }

    // ================================================================== history

    public Map<String, Object> historyCombos() { return historyCombos(viewer()); }

    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> cust = new ArrayList<>(), inv = new ArrayList<>();
        for (Map<String, Object> r : repo.grnDropDown(u)) {
            String a = text(ci(r, "Activity")).trim();
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "ReferenceName")));
            if ("Customer".equals(a)) cust.add(m);
            else if ("InvoiceNo".equals(a)) inv.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", cust);
        out.put("invoices", inv);
        return out;
    }

    /** Gridhistoryfill - the 27 columns of the desktop's history DataTable. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = viewer();
        boolean canViewAll = isAdmin(ctx) || repo.canViewAllRecordRight(u, SCREEN_ID);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("FinancialYearId", fy());
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("CanViewAllRecord", canViewAll);
        /* BLL quirk: @EntryUser (the procedure declares @EntryUserId), never set - refused for a user without CanView AllRecord. */
        if (!canViewAll) p.put("EntryUser", 0);
        java.sql.Date from = asBool(b.get("fromChecked")) ? sqlDate(b.get("fromDate")) : null;
        java.sql.Date to = asBool(b.get("toChecked")) ? sqlDate(b.get("toDate")) : null;
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        int fromNo = asInt(b.get("fromDocNo")), toNo = asInt(b.get("toDocNo"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        int cust = asInt(b.get("supplierCustomerId")), inv = asInt(b.get("exImInvoiceId"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (inv != 0) p.put("ExImInvoiceId", inv);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.grnHistory(p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocNo", text(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("ExImForwardingId", asInt(ci(r, "ExImForwardingId")));
            m.put("ForwardingNo", text(ci(r, "ForwardingNo")));
            m.put("GpId", asInt(ci(r, "GpId")));
            m.put("GpNo", text(ci(r, "GpNo")));
            m.put("GPDate", iso(ci(r, "GPDate")));
            m.put("Transporter", text(ci(r, "Transporter")));
            m.put("FreightAmount", asDouble(ci(r, "FreightAmount")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("BiltyNo", text(ci(r, "BiltyNo")));
            m.put("DriverName", text(ci(r, "DriverName")));
            m.put("DriverCellNo", text(ci(r, "DriverCellNo")));
            m.put("DriverCnicNo", text(ci(r, "DriverCnicNo")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("NoOfContainer", asInt(ci(r, "NoOfContainer")));
            m.put("RemarksHeader", text(ci(r, "RemarksHeader")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================== read

    /** ReadById / GetDetailGrdByHeadId. */
    public Map<String, Object> byId(int id) {
        UserAccount u = viewer();
        List<Map<String, Object>> hs = repo.grnHeader(id);
        if (hs.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> h = hs.get(0);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : new String[]{"Id", "DocNo", "GPId", "ExImForwardingId", "GatePassOutwardId", "SupplierCustomerId", "DestinationPortId",
                "LoadingPortId", "ShippingLineId", "ShippingAgentId", "TransporterId"}) head.put(k, asInt(ci(h, k)));
        head.put("DocDate", iso(ci(h, "DocDate")));
        head.put("GPDate", iso(ci(h, "GPDate")));
        head.put("GpNo", text(ci(h, "GpNo")));
        for (String k : new String[]{"RemarksHeader", "VehicleNo", "BiltyNo", "DriverCellNo", "DriverName", "DriverCnicNo", "Container", "Container1"})
            head.put(k, text(ci(h, k)));
        head.put("FreightAmount", asDouble(ci(h, "FreightAmount")));
        head.put("GpFactoryWeight", asDouble(ci(h, "GpFactoryWeight")));
        head.put("NoOfContainer", asDouble(ci(h, "NoOfContainer")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", head);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.grnDetails(id)) {
            Map<String, Object> m = blankRow();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("ContractId", asInt(ci(d, "ContractId")));
            m.put("ContractDetailId", asInt(ci(d, "ContractDetailId")));
            m.put("ContractNo", text(ci(d, "ContractNo")));
            m.put("ContractDate", iso(ci(d, "ContractDate")));
            m.put("InvoiceId", asInt(ci(d, "ExImInvoiceId")));
            m.put("InvoiceDetailId", asInt(ci(d, "InvoiceDetailId")));
            m.put("InvoiceNo", text(ci(d, "InvoiceNo")));
            m.put("ExImForwardingDetailId", asInt(ci(d, "ExImForwardingDetailId")));
            m.put("ReturnInvoiceId", asInt(ci(d, "ReturnInvoiceId")));
            m.put("ReturnInvoiceDetailId", asInt(ci(d, "ReturnInvoiceDetailId")));
            m.put("ReturnInvoiceNo", asInt(ci(d, "ReturnInvoiceNo")));
            m.put("WarehouseId", asInt(ci(d, "WarehouseId")));
            m.put("Warehouse", text(ci(d, "WareHouseName")));
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemCode", text(ci(d, "ItemCode")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("JobLotId", asInt(ci(d, "JobLotId")));
            m.put("JobLot", text(ci(d, "JobLot")));
            m.put("PackingTypeId", asInt(ci(d, "PackingTypeId")));
            m.put("PackingType", text(ci(d, "PackingType")));
            m.put("PackUomId", asInt(ci(d, "PackUomId")));
            m.put("PackUomCode", text(ci(d, "PackUomCode")));
            m.put("PackUomEquivalent", asDouble(ci(d, "PackUomEquivalent")));
            m.put("NoOfBags", asDouble(ci(d, "ItemQty")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            m.put("EbUnit", asDouble(ci(d, "EbUnit")));
            m.put("EbTotal", asDouble(ci(d, "EbTotal")));
            m.put("AddLess", asDouble(ci(d, "AdLsWeight")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("StockWeight", asDouble(ci(d, "StockWeight")));
            m.put("ItemDescription", text(ci(d, "ItemDescription")));
            m.put("Container#", text(ci(d, "ContainerNo")));
            m.put("Seal#", text(ci(d, "SealNo")));
            m.put("CostRate", asDouble(ci(d, "CostRate")));
            rows.add(m);
        }
        out.put("rows", rows);
        List<Map<String, Object>> others = new ArrayList<>();
        for (Map<String, Object> o : repo.grnOtherItems(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(o, "Id")));
            m.put("ItemId", asInt(ci(o, "OtherItemId")));
            /* Rows.Add(Id, OtherItemId, OtherItemName, OtherItemCode, ..) into (Id, ItemId, ItemCode, ItemName, ..) - swapped as on the desktop. */
            m.put("ItemCode", text(ci(o, "OtherItemName")));
            m.put("ItemName", text(ci(o, "OtherItemCode")));
            m.put("Qty", asDouble(ci(o, "Qty")));
            m.put("Rate", asDouble(ci(o, "Rate")));
            m.put("Amount", asDouble(ci(o, "Amount")));
            m.put("ContainerNo", text(ci(o, "ContainerNo")));
            m.put("Remarks", text(ci(o, "OtherItemRemarks")));
            others.add(m);
        }
        out.put("otherItems", others);
        return out;
    }

    /** ReadById side data: the gate passes with RecId, the forwardings / other-item list, the reverse-flow forwarding data. */
    public Map<String, Object> editData(int id, int forwardingId, int firstInvoiceId) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("gatePasses", gatePassCombo(repo.pendingInwardGatePasses(u, fy(), 0, id)));
        boolean reverse = asBool(repo.config(u, "EnableExportReturnReverseFlow"));
        if (!reverse) out.put("forwardings", forwardingCombo(repo.pendingForwardings(u, fy(), id)));
        else out.put("forwardingData", forwardingData(firstInvoiceId));
        out.put("otherItemList", otherItemList(u, forwardingId));
        return out;
    }

    // ================================================================== delete

    public Map<String, Object> delete(int recId) {
        UserAccount u = viewer();
        if (!right(ctx, rights, u, SCREEN_ID, "Delete")) throw new AccessDeniedException("The user does not have Delete rights for this screen");
        if (recId == 0) throw new IllegalArgumentException("Record Not Delete because RecId No Found");
        repo.grnDelete(u, DOCUMENT_TYPE_ID, recId);
        return ok("Deleted Successfully");
    }

    // ================================================================== save

    /** Insert(): validations (desktop texts and order), BLL ExportReturnGrn.Save -> DAL SetData. */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = viewer();
        int recId = asInt(b.get("recId"));
        if (!right(ctx, rights, u, SCREEN_ID, recId > 0 ? "Update" : "Save"))
            throw new AccessDeniedException("The user does not have " + (recId > 0 ? "Update" : "Save") + " rights for this screen");
        boolean reverse = asBool(repo.config(u, "EnableExportReturnReverseFlow"));
        Map<String, Object> h = map(b.get("header"));
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        List<Map<String, Object>> others = list(b.get("otherItems"));

        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        if (trim(h.get("DocNo")).isEmpty()) throw new IllegalArgumentException("Doc No field is required");
        if (asInt(h.get("GPId")) == 0) throw new IllegalArgumentException("GatePass No field is required");
        if (!reverse && asInt(h.get("ExImForwardingId")) == 0) throw new IllegalArgumentException("Forwarding No field is required");
        if (asInt(h.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer field is required");
        if (trim(h.get("VehicleNo")).isEmpty()) throw new IllegalArgumentException("Vehicle No field is required");
        if (asInt(h.get("LoadingPortId")) == 0) throw new IllegalArgumentException("Loading Port field is required");
        if (asInt(h.get("DestinationPortId")) == 0) throw new IllegalArgumentException("Destination Port field is required");
        if (!nonZeroNumber(h.get("NoOfContainer"))) throw new IllegalArgumentException("No Of Container must be a non-zero number");
        if (!nonZeroNumber(h.get("FactoryWeight"))) throw new IllegalArgumentException("Factory Weight must be a non-zero number");
        int transporter = asInt(h.get("TransporterId"));
        BigDecimal freight = dec(h.get("FreightAmount"));
        if (transporter == 0 && freight.signum() > 0) throw new IllegalArgumentException("Transporter is Required when Freight is greater than Zero");
        if (transporter != 0 && freight.signum() == 0) throw new IllegalArgumentException("Freight is Required when Transporter is Selected");
        double gross = 0;
        for (Map<String, Object> r : rows) gross += asDouble(r.get("GrossWeight"));
        if (asDouble(h.get("FactoryWeight")) != gross) throw new IllegalArgumentException("Factory Weight and Total Gross Weight In Grid Should Be Equal");

        Timestamp now = now();
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        head.put("DocDate", pickerValue(h.get("DocDate")));
        head.put("EntryDate", now);
        head.put("GPDate", pickerValue(h.get("GPDate")));
        head.put("ModifyDate", now);
        head.put("FreightAmount", freight);
        head.put("NoOfContainer", dec(h.get("NoOfContainer")));
        head.put("ActionId", recId == 0 ? 1 : 2);
        head.put("ApprovedUserId", u.getId());
        head.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        head.put("CompanyId", u.getCompanyId());
        head.put("DestinationPortId", asInt(h.get("DestinationPortId")));
        head.put("DocNo", asInt(h.get("DocNo")));
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUserId", u.getId());
        head.put("ExImInvoiceId", 0);
        head.put("FinancialYearId", fy());
        head.put("GPId", asInt(h.get("GPId")));
        head.put("ExImForwardingId", asInt(h.get("ExImForwardingId")));
        head.put("GatePassOutwardId", asInt(h.get("GatePassOutwardId")));
        head.put("Id", recId);
        head.put("LCOrderId", 0);
        head.put("LoadingPortId", asInt(h.get("LoadingPortId")));
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PendingForView", 0);
        head.put("ProjectsId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        head.put("ShippingAgentId", asInt(h.get("ShippingAgentId")));
        head.put("ShippingLineId", asInt(h.get("ShippingLineId")));
        head.put("SupplierCustomerId", asInt(h.get("SupplierCustomerId")));
        head.put("TransporterId", transporter);
        head.put("BiltyNo", text(h.get("BiltyNo")));
        head.put("Container", text(h.get("Container")));
        head.put("Container1", text(h.get("Container1")));
        head.put("DriverCellNo", text(h.get("DriverCellNo")));
        head.put("DriverCnicNo", text(h.get("DriverCnicNo")));
        head.put("DriverName", text(h.get("DriverName")));
        head.put("GpNo", trim(h.get("GpNo")));
        head.put("RemarksHeader", text(h.get("RemarksHeader")));
        head.put("VehicleNo", text(h.get("VehicleNo")));

        List<Map<String, Object>> details = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int rowId = recId != 0 ? asInt(r.get("Id")) : 0;
            Map<String, Object> vd = detailModel(r, rowId <= 0 ? 1 : 2, rowId);
            validateField(vd.get("ContractId"), "ContractId", i, "Detail Grid");
            validateField(vd.get("InvoiceDetailId"), "InvoiceDetailId", i, "Detail Grid");
            if (!reverse) validateField(vd.get("ExImForwardingDetailId"), "ExImForwardingDetailId", i, "Detail Grid");
            validateField(vd.get("WarehouseId"), "Warehouse", i, "Detail Grid");
            validateField(vd.get("ItemId"), "Item", i, "Detail Grid");
            validateField(vd.get("CropYearId"), "CropYear", i, "Detail Grid");
            validateField(vd.get("JobLotId"), "Job Lot", i, "Detail Grid");
            validateField(vd.get("PackingTypeId"), "Packing Type", i, "Detail Grid");
            validateField(vd.get("PackUomId"), "Pack UOM", i, "Detail Grid");
            validateField(vd.get("ItemQty"), "No Of Bags", i, "Detail Grid");
            validateField(vd.get("GrossWeight"), "Gross Weight", i, "Detail Grid");
            validateField(vd.get("NetWeight"), "Net Weight", i, "Detail Grid");
            validateField(vd.get("StockWeight"), "Stock Weight", i, "Detail Grid");
            details.add(vd);
        }
        if (recId > 0) for (Map<String, Object> r : removed) details.add(detailModel(r, 3, asInt(r.get("Id"))));

        List<Map<String, Object>> otherItems = new ArrayList<>();
        for (Map<String, Object> r : others) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Amount", dec(r.get("Amount")));
            o.put("Qty", dec(r.get("Qty")));
            o.put("Rate", dec(r.get("Rate")));
            o.put("WeightKgs", BigDecimal.ZERO);
            o.put("ExportReturnGrnId", 0);
            o.put("Id", asInt(r.get("Id")));
            o.put("OtherItemId", asInt(r.get("ItemId")));
            o.put("ContainerNo", text(r.get("ContainerNo")));
            o.put("OtherItemRemarks", text(r.get("Remarks")));
            otherItems.add(o);
        }

        /* BLL Save: MakeVoucher only when a row refers to a return invoice, before the ActionId / user reset. */
        SaleInvoiceModels.VoucherHead vh = null;
        List<SaleInvoiceModels.VoucherDetail> vds = new ArrayList<>();
        boolean anyReturn = false;
        for (Map<String, Object> d : details) if (asInt(d.get("ReturnInvoiceId")) > 0) { anyReturn = true; break; }
        if (anyReturn) { vh = new SaleInvoiceModels.VoucherHead(); vds = makeVoucher(u, head, details, vh); }
        if (recId == 0) head.put("ModifyUserId", 0); else head.put("EntryUserId", 0);

        int id = repo.grnSave(head, details, otherItems, vh, vds);
        Map<String, Object> out = ok(recId > 0 ? "Record Update SuccessFully" : "Record Save SuccessFully");
        out.put("id", id);
        return out;
    }

    /** FillDetailListCommonForInsertAndDelete - ExportReturnGrnDetail non-virtual properties (RemarksDetail stays null). */
    private static Map<String, Object> detailModel(Map<String, Object> r, int actionTypeId, int id) {
        Map<String, Object> vd = new LinkedHashMap<>();
        vd.put("IsApproved", false);
        vd.put("AdLsWeight", dec(r.get("AddLess")));
        vd.put("EbTotal", dec(r.get("EbTotal")));
        vd.put("EbUnit", dec(r.get("EbUnit")));
        vd.put("GrossWeight", dec(r.get("GrossWeight")));
        vd.put("ItemQty", dec(r.get("NoOfBags")));
        vd.put("NetWeight", dec(r.get("NetWeight")));
        vd.put("StockWeight", dec(r.get("StockWeight")));
        vd.put("CostRate", dec(r.get("CostRate")));
        vd.put("ActionTypeId", actionTypeId);
        vd.put("CropYearId", asInt(r.get("CropYearId")));
        vd.put("ExportReturnGrnId", 0);
        vd.put("Id", id);
        vd.put("ContractId", asInt(r.get("ContractId")));
        vd.put("ContractDetailId", asInt(r.get("ContractDetailId")));
        vd.put("InvoiceDetailId", asInt(r.get("InvoiceDetailId")));
        vd.put("ExImInvoiceId", asInt(r.get("InvoiceId")));
        vd.put("ExImForwardingDetailId", asInt(r.get("ExImForwardingDetailId")));
        vd.put("ItemId", asInt(r.get("ItemId")));
        vd.put("JobLotId", asInt(r.get("JobLotId")));
        vd.put("LineId", 0);
        vd.put("PackingTypeId", asInt(r.get("PackingTypeId")));
        vd.put("PackUomId", asInt(r.get("PackUomId")));
        vd.put("WarehouseId", asInt(r.get("WarehouseId")));
        vd.put("ContainerNo", trim(r.get("Container#")));
        vd.put("ItemDescription", trim(r.get("ItemDescription")));
        vd.put("SealNo", trim(r.get("Seal#")));
        vd.put("ReturnInvoiceId", asInt(r.get("ReturnInvoiceId")));
        vd.put("ReturnInvoiceDetailId", asInt(r.get("ReturnInvoiceDetailId")));
        return vd;
    }

    /**
     * BLL ExportReturnGrn.MakeVoucher (0450:19): ExportReturnStockInTransitAccount must be configured; per row
     * (removed rows included, as the BLL iterates the whole list) a PurchaseGLAC / transit pair at CostRate x StockWeight;
     * a row whose item has no GL or whose CostRate is not positive throws "Item Record not found".
     */
    private List<SaleInvoiceModels.VoucherDetail> makeVoucher(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details,
                                                              SaleInvoiceModels.VoucherHead vh) {
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = asInt(obj.get("Id"));
        vh.RefDocNoId = asInt(obj.get("Id"));
        vh.VoucherCode = asInt(obj.get("DocNo"));
        LocalDateTime docDate = ((Timestamp) obj.get("DocDate")).toLocalDateTime();
        vh.VoucherDate = docDate;
        vh.Remarks = text(obj.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = asInt(obj.get("BranchesId"));
        vh.ProjectId = asInt(obj.get("ProjectsId"));
        vh.BillAmount = 0;
        vh.ManualBillNo = "";                                           // Conversion.ToString(obj.InvoiceNo) - a virtual property never set
        vh.DueDate = docDate;
        vh.DueDays = 0;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = asInt(obj.get("FinancialYearId"));
        vh.EntryUser = asInt(obj.get("EntryUserId"));
        vh.EntryDate = LocalDateTime.now().withNano(0);
        vh.ModifyDate = LocalDateTime.now().withNano(0);
        vh.ModifyUser = asInt(obj.get("ModifyUserId"));
        List<Map<String, Object>> itemGl = repo.itemGl(u);
        int transit = asInt(repo.config(u, "ExportReturnStockInTransitAccount"));
        if (transit <= 0) throw new IllegalStateException("Export Return Stock InTransit Account not map in configuration please check...");
        List<SaleInvoiceModels.VoucherDetail> out = new ArrayList<>();
        int line = 1;
        for (Map<String, Object> item : details) {
            Map<String, Object> g = null;
            for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == asInt(item.get("ItemId"))) { g = r; break; }
            BigDecimal costRate = dec(item.get("CostRate"));
            if (g == null || costRate.signum() <= 0) throw new IllegalStateException("Item Record not found");
            BigDecimal amount = costRate.multiply(dec(item.get("StockWeight")));
            String text = " Item: " + text(ci(g, "ItemName")) + " Qty: " + num(item.get("ItemQty")) + " Weight: " + num(item.get("NetWeight")) + " CostRate " + num(costRate);
            int purchase = asInt(ci(g, "PurchaseGLAC"));
            out.add(costLine(purchase, transit, text, amount.doubleValue(), 0, costRate.doubleValue(), asDouble(item.get("ItemQty")), asDouble(item.get("StockWeight")), line));
            out.add(costLine(transit, purchase, text, 0, amount.doubleValue(), costRate.doubleValue(), asDouble(item.get("ItemQty")), asDouble(item.get("StockWeight")), line));
            line++;
        }
        return out;
    }

    private static SaleInvoiceModels.VoucherDetail costLine(int account, int against, String comments, double debit, double credit,
                                                           double rate, double qty, double weight, int line) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemRate = rate;
        d.QtyIn = qty;
        d.WeightIn = weight;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.LineId = line;
        return d;
    }

    // ================================================================== frmLoadExportReurnInvoiceForGrn

    /** InitializeComponentCustom (From = ActiveYr.Start_Period) + GetDataForDropDownFromExportReturnInvoice combos. */
    public Map<String, Object> loaderSetup() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", iso(repo.financialYearStart(fy())));
        Map<String, List<Map<String, Object>>> combos = new LinkedHashMap<>();
        /* tableMap keys: InvoiceNo, Customer, Item, JobLot, Crop - the procedure's crop activity is 'CropYear', so the
           Crop Year combo stays empty (desktop quirk, kept). */
        for (String k : new String[]{"InvoiceNo", "Customer", "Item", "JobLot", "Crop"}) combos.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.riDropDown(u)) {
            String a = text(ci(r, "Activity"));
            for (String k : combos.keySet()) {
                if (k.equalsIgnoreCase(a)) {
                    Map<String, Object> m = new LinkedHashMap<>();
                    m.put("Id", asInt(ci(r, "Id")));
                    m.put("name", text(ci(r, "ReferenceName")));
                    combos.get(k).add(m);
                }
            }
        }
        out.put("combos", combos);
        return out;
    }

    /** GridRecordsDBCall: ExportRetunrInvoice_GetDataForGrnReturn (LcOrderId never set, so @ContractId is never sent). */
    public List<Map<String, Object>> loaderRows(Map<String, Object> b) {
        UserAccount u = viewer();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        java.sql.Date from = sqlDate(b.get("fromDate")), to = sqlDate(b.get("toDate"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("Todate", to);
        int inv = asInt(b.get("exImInvoiceId")), cust = asInt(b.get("supplierCustomerId"));
        int item = asInt(b.get("itemId")), job = asInt(b.get("jobLotId")), crop = asInt(b.get("cropYearId"));
        if (inv != 0) p.put("ExImInvoiceId", inv);
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (item != 0) p.put("ItemId", item);
        if (job != 0) p.put("JobLotId", job);
        if (crop != 0) p.put("CropYearId", crop);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.returnInvoiceDataForGrn(p)) out.add(ExportReturnInvoiceService.plain(r));
        return out;
    }
}
