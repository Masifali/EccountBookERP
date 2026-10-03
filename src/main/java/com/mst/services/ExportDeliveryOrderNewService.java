package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportDeliveryOrderNewRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportInvoiceTransferSupport.*;

/**
 * BLL side of 201 "Export Delivery Order (New)" - Architecture.WinApp.Export.ExportDeliveryOrderNew (caption
 * "Delivery Order Export", DocumentTypeId 84, DeliveryOrderType "Export", ScreenName saved "ExportDeliveryOrderNew";
 * rights by the screen name "ExportDeliveryOrder" = ScreenDefinition 201).
 *
 * Desktop behaviour kept:
 *  - FormValidation texts and order ("Brnach field is required" - the desktop's spelling - only when the branch combo is
 *    visible, i.e. more than one branch), DocNo, Item Weight, Packing Weight, Total Gross Weight, Other Remarks when
 *    Other Weight > 0; then "Grid Record Not Found".
 *  - OtherWeightProportion before saving: row OtherWeight = header Other Weight / SUM(Weight) * row Weight (the hidden
 *    "Weight" column, not LoadWeight), 0 when the header Other Weight is 0.
 *  - InnerQty is assigned from DoQty BEFORE DoQty is set, so it is always saved as 0; InnerUomId = PackUomId;
 *    InnerEbUnit = PackingWeight; InnerEbTotal = OuterEbTotal = TotalPackingWeight.
 *  - Detail ActionTypeId: new document -> 1 for every row; update -> 2 for saved rows, 1 for new rows; deleted saved rows
 *    are sent first with ActionTypeId 3. The removed-rows list is never cleared by Reset on the desktop, so it is sent
 *    with the next save too (a new document then fails with the BLL's "Record cannot be inserted because ActionTypeId
 *    not equal to 1") - kept, the page keeps the list the same way.
 *  - "Container No Required in Row No" + n when ContainerNoCompulsoryOnDeliveryOrderExport is on.
 *  - Header EximInvoiceId = the invoice combo's current value; TransporterId = the transporter combo, which this form never
 *    fills (always 0); ModifyUser only on update; BranchesId = branch combo when more than one branch, else the user's.
 *  - Attachments (DMS) are not part of this web screen: AttachmentsValues / CustomAttachmentsValues of a loaded record
 *    are written back unchanged, "" for a new record.
 * Tenancy, user and financial year from the session only.
 */
@Service
public class ExportDeliveryOrderNewService {

    public static final int SCREEN_ID = 201;
    public static final int DOCUMENT_TYPE_ID = 84;
    public static final String FORM_NAME = "ExportDeliveryOrderNew";

    @Autowired private ExportDeliveryOrderNewRepository repo;
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

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("DefaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("ContainerNoCompulsoryOnDeliveryOrderExport", asBool(repo.config(u, "ContainerNoCompulsoryOnDeliveryOrderExport")));
        c.put("DefaultJobLotId", asInt(repo.config(u, "Job/Lot")));
        c.put("DefaultWarehouseId", asInt(repo.config(u, "Warehouse")));
        return c;
    }

    /** PurchsaeOrder_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.put("config", config(u));
        out.put("userBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        out.put("invoices", invoiceCombo(u, 0));
        out.put("packingTypes", pick(repo.packingTypes(), "Id", "PackTypeDesc"));
        int code = repo.generateCode(u, fy());
        out.put("docNo", code > 0 ? String.valueOf(code) : "");
        out.put("warehouses", pick(repo.activeWarehouses(u), "Id", "WareHouseName"));
        out.put("jobLots", pick(repo.jobLots(u), "Id", "JobLotDescription"));
        out.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        out.put("historyCombos", historyCombos(u));
        return out;
    }

    /** btnRefresh_Click: branches, packing types, warehouses, job lots, invoices (RecId). */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        out.put("packingTypes", pick(repo.packingTypes(), "Id", "PackTypeDesc"));
        out.put("warehouses", pick(repo.activeWarehouses(u), "Id", "WareHouseName"));
        out.put("jobLots", pick(repo.jobLots(u), "Id", "JobLotDescription"));
        out.put("invoices", invoiceCombo(u, recId));
        return out;
    }

    /** Reset(): DocumentNoFill + CmbInvoiceNoFill (Id 0) + defaultConfiquration. */
    public Map<String, Object> reset() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        int code = repo.generateCode(u, fy());
        out.put("docNo", code > 0 ? String.valueOf(code) : "");
        out.put("invoices", invoiceCombo(u, 0));
        out.put("config", config(u));
        return out;
    }

    private List<Map<String, Object>> invoiceCombo(UserAccount u, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.outstandingInvoices(u, recId)) out.add(row("Id", asInt(ci(r, "Id")), "InvoiceNo", text(ci(r, "InvoiceNo"))));
        return out;
    }

    /** CmbInvoiceNoFill (obj.Id = the form's Id). */
    public List<Map<String, Object>> invoices(int recId) { return invoiceCombo(user("View"), recId); }

    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> c = new ArrayList<>(), i = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u, fy())) {
            String a = text(ci(r, "Activity"));
            if ("Customer".equals(a)) c.add(row("Id", asInt(ci(r, "Id")), "Customer", text(ci(r, "ReferenceName"))));
            else if ("InvoiceNo".equals(a)) i.add(row("Id", asInt(ci(r, "Id")), "InvoiceNo", text(ci(r, "ReferenceName"))));
        }
        return row("customers", c, "invoices", i);
    }

    /** btnRefreshHistory_Click -> HistoryCombosFill. */
    public Map<String, Object> historyCombosRefresh() { return historyCombos(user("View")); }

    // ============================================================================ detail events

    /**
     * cmbInvoiceNo_Leave: GetDatabyInvoiceId (contracts -> first row as Sale Contract No; header data -> invoice date and
     * customer), GetInvoicewiseDoWeightandBalanceWeight, and GetContainerNoFromCROByInvoiceId when the invoice is set.
     */
    public Map<String, Object> invoice(int invoiceId, int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> contracts = repo.contractsByInvoice(u, invoiceId);
        List<Map<String, Object>> orders = new ArrayList<>();
        if (!contracts.isEmpty()) orders.add(row("OrderId", asInt(ci(contracts.get(0), "ContractId")), "OrderNo", text(ci(contracts.get(0), "ContractNo"))));
        out.put("orders", orders);
        List<Map<String, Object>> hdr = repo.invoiceHeader(u, invoiceId);
        out.put("invoiceDate", hdr.isEmpty() ? "" : iso(ci(hdr.get(0), "DocDate")));
        out.put("customers", pick(hdr, "Id", "CustomerName"));
        out.put("weights", weights(u, invoiceId));
        out.put("containers", invoiceId != 0 ? containerRows(u, invoiceId, recId) : null);
        return out;
    }

    private Map<String, Object> weights(UserAccount u, int invoiceId) {
        List<Map<String, Object>> r = repo.invoiceWeights(u, invoiceId);
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : new String[] { "InvoiceWeight", "DoWeight", "GpRejectedWeight", "WeightAvailableForDo" })
            m.put(k, r.isEmpty() ? "0" : text(ci(r.get(0), k)));
        return m;
    }

    /** cmbInvoiceNo_TextChanged: GetInvoicewiseDoWeightandBalanceWeight. */
    public Map<String, Object> invoiceWeights(int invoiceId) { return weights(user("View"), invoiceId); }

    private List<Map<String, Object>> containerRows(UserAccount u, int invoiceId, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.containers(u, invoiceId, recId)) out.add(row("ContainerId", asInt(ci(r, "ContainerId")), "ContainerNo", text(ci(r, "Container"))));
        return out;
    }

    /** CmbOrderNo_TextChanged: ExportInvoiceDetailByHeaderId when invoice and order are both > 0. */
    public List<Map<String, Object>> contractItems(int invoiceId, int orderId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        if (invoiceId > 0 && orderId > 0) {
            for (Map<String, Object> r : repo.invoiceDetailByHeader(u, invoiceId, orderId)) {
                out.add(row("ItemId", asInt(ci(r, "ItemId")), "ItemName", text(ci(r, "ItemName")), "InvoiceDetailId", asInt(ci(r, "InvoiceDetailId")),
                        "PackUomId", asInt(ci(r, "PackUomId")), "PackUom", text(ci(r, "PackUom")),
                        "CropYearId", asInt(ci(r, "CropYearId")), "CropYear", text(ci(r, "CropYear"))));
            }
        }
        return out;
    }

    /** CmbItemName Leave -> BindPackUomAndPackingType(InvoiceDetailId). */
    public Map<String, Object> itemDetail(int invoiceDetailId) {
        user("View");
        List<Map<String, Object>> dt = repo.invoiceDetailForDetailId(invoiceDetailId);
        Map<String, Object> out = new LinkedHashMap<>();
        if (dt.isEmpty()) { out.put("found", false); return out; }
        out.put("found", true);
        out.put("TotalPackingWeight", text(ci(dt.get(0), "TotalPackingWeight")));
        out.put("PackingWeight", text(ci(dt.get(0), "PackingWeight")));
        out.put("JobLotId", asInt(ci(dt.get(0), "JobLotId")));
        List<Map<String, Object>> uoms = new ArrayList<>();
        for (Map<String, Object> r : dt) uoms.add(row("PackUomId", asInt(ci(r, "PackUomId")), "PackUom", text(ci(r, "PackUom")), "Equivalent", asDouble(ci(r, "Equivalent"))));
        out.put("uoms", uoms);
        return out;
    }

    public List<Map<String, Object>> containers(int invoiceId, int recId) {
        UserAccount u = user("View");
        return containerRows(u, invoiceId, recId);
    }

    // ============================================================================ read / history

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (asInt(ci(h, "OrganizationId")) != u.getOrganizationId() || asInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        return h;
    }

    private static boolean exportDo(Map<String, Object> h) {
        return asInt(ci(h, "DocumentTypeId")) == DOCUMENT_TYPE_ID && "Export".equals(text(ci(h, "DeliveryOrderType")));
    }

    /** grid row of the desktop's "table" from an InvDeliveryOrderdetail (ReadByIdDetailIdExport). */
    private static Map<String, Object> tableRow(Map<String, Object> d) {
        double tpw = asDouble(ci(d, "TotalPackingWeight"));
        double pw = asDouble(ci(d, "PackingWeight"));
        /* ReadById: TotalPackingWeight 0 -> TotalPackingWeight = PackingWeight, and when > 0 PackingWeight = TPW / LoadingQty. */
        if (tpw == 0) {
            tpw = pw;
            if (tpw > 0) pw = tpw / asDouble(ci(d, "LoadingQty"));
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("InvoiceDetailId", asInt(ci(d, "InvoiceDetailId")));
        m.put("InvoiceId", asInt(ci(d, "ExImInvoiceId")));
        m.put("InvoiceNo", text(ci(d, "InvoiceNo")));
        m.put("SupplierCustomerId", asInt(ci(d, "SupplierCustomerId")));
        m.put("SupplierCustomer", text(ci(d, "SupplierCustomer")));
        m.put("OrderId", asInt(ci(d, "SaleOrderId")));
        m.put("OrderNo", text(ci(d, "OrderNo")));
        m.put("WareHouseId", asInt(ci(d, "WarehouseId")));
        m.put("WareHouse", text(ci(d, "WareHouseName")));
        m.put("JobLotId", asInt(ci(d, "JobLotId")));
        m.put("JobLot", text(ci(d, "JobLotDescription")));
        m.put("CropYearId", asInt(ci(d, "CropYearId")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("Item", text(ci(d, "ItemName")));
        m.put("ItemUOM", text(ci(d, "PackUOM")));
        m.put("ItemUOMId", asInt(ci(d, "PackUomId")));
        m.put("PackingTypeId", asInt(ci(d, "InvPackingTypeId")));
        m.put("PackingType", text(ci(d, "PackTypeDesc")));
        m.put("QTY", asDouble(ci(d, "DoQty")));
        m.put("Weight", asDouble(ci(d, "DoWeight")));
        m.put("LoadQty", asDouble(ci(d, "LoadingQty")));
        m.put("LoadWeight", asDouble(ci(d, "LoadingWeight")));
        m.put("PackingWeight", pw);
        m.put("TotalPackingWeight", tpw);
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("OtherWeight", asDouble(ci(d, "OtherWeight")));
        m.put("ContainerId", asInt(ci(d, "ContainerId")));
        m.put("ContainerNo", text(ci(d, "Container")));
        m.put("ContainerRemarks", text(ci(d, "ContainerRemarks")));
        m.put("InspectionRemarks", text(ci(d, "InspectionRemarks")));
        m.put("Remarks", text(ci(d, "LoadingRemarks")));
        return m;
    }

    /** ReadById: InvDeliveryOrder.GetByID. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = owned(u, id);
        if (h == null) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> hd = new LinkedHashMap<>();
        hd.put("Id", asInt(ci(h, "Id")));
        hd.put("DocDate", iso(ci(h, "DocDate")));
        hd.put("DocNo", text(ci(h, "DocNo")));
        hd.put("LoadingInstructions", text(ci(h, "LoadingInstructions")));
        hd.put("EximInvoiceId", asInt(ci(h, "EximInvoiceId")));
        hd.put("VehicleNo", text(ci(h, "VehicleNo")));
        hd.put("BranchesId", asInt(ci(h, "BranchesId")));
        hd.put("NetWeight", asDouble(ci(h, "NetWeight")));
        hd.put("PackingWeight", asDouble(ci(h, "PackingWeight")));
        hd.put("OtherWeight", asDouble(ci(h, "OtherWeight")));
        hd.put("GrossWeight", asDouble(ci(h, "GrossWeight")));
        hd.put("OtherWeightRemarks", text(ci(h, "OtherWeightRemarks")));
        hd.put("TransporterId", asInt(ci(h, "TransporterId")));
        hd.put("IsApproved", asBool(ci(h, "IsApproved")));
        List<Map<String, Object>> rows = new ArrayList<>();
        /* DAL GetData only reads the export detail list for DocumentTypeId 84 + "Export"; otherwise the list stays null and
           the desktop's loop throws. */
        if (!exportDo(h)) throw new IllegalStateException("Object reference not set to an instance of an object.");
        for (Map<String, Object> d : repo.details(id)) rows.add(tableRow(d));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        out.put("rows", rows);
        return out;
    }

    /** gridhistoryfill: ExportFormHistoryNew, one row per Id. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        boolean all = allowed(u, "CanView AllRecord");
        Timestamp from = asBool(f.get("fromChecked")) ? pickerDate(f.get("fromDate")) : null;
        Timestamp to = asBool(f.get("toChecked")) ? pickerDate(f.get("toDate")) : null;
        String mode = text(f.get("dateMode"));
        Map<String, Timestamp> dates = new LinkedHashMap<>();
        if ("doc".equals(mode)) { dates.put("FromDate", from); dates.put("ToDate", to); }
        else if ("entry".equals(mode)) { dates.put("EntryFromDate", from); dates.put("EntryToDate", to); }
        else if ("modify".equals(mode)) { dates.put("ModifyFromDate", from); dates.put("ModifyToDate", to); }
        else if ("approved".equals(mode)) { dates.put("ApprovedFromDate", from); dates.put("ApprovedToDate", to); }
        List<Map<String, Object>> rows = repo.formHistory(u, fy(), all, dates, asInt(f.get("fromDocNo")), asInt(f.get("toDocNo")),
                asInt(f.get("customerId")), asInt(f.get("invoiceId")));
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = asInt(ci(r, "Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("DoDate", iso(ci(r, "DocDate")));
            m.put("DoNo", asInt(ci(r, "DocNo")));
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceDate", plainDate(ci(r, "InvoiceDate")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceNetWeight", asDouble(ci(r, "InvoiceNetWeight")));
            m.put("InvoiceGrossWeight", asDouble(ci(r, "InvoiceGrossWeight")));
            m.put("CustomerName", text(ci(r, "SupplierCustomer")));
            m.put("OrderNo", text(ci(r, "OrderNo")));
            m.put("GatePassId", asInt(ci(r, "GatePassId")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("Transporter", text(ci(r, "Transporter")));
            m.put("GpDate", plainDate(ci(r, "GpDate")));
            m.put("GpSrNo", ci(r, "GpSrNo") == null ? "" : String.valueOf(asInt(ci(r, "GpSrNo"))));
            m.put("GpStatus", text(ci(r, "GpStatus")));
            m.put("ForwardingNo", ci(r, "ForwardingNo") == null ? "" : String.valueOf(asInt(ci(r, "ForwardingNo"))));
            m.put("FactoryWeight", asDouble(ci(r, "FactoryWeight")));
            m.put("EntryDate", plainDate(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", plainDate(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("ApprovedDate", plainDate(ci(r, "ApprovedDate")));
            m.put("ApprovedUser", text(ci(r, "ApprovedUser")));
            m.put("BranchName", text(ci(r, "BranchName")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** A DateTime column as Janus shows it (M/d/yyyy h:mm:ss tt); "" for DBNull. */
    private static String plainDate(Object o) {
        if (o == null) return "";
        if (o instanceof Timestamp) {
            java.time.LocalDateTime t = ((Timestamp) o).toLocalDateTime();
            return t.format(java.time.format.DateTimeFormatter.ofPattern("M/d/yyyy h:mm:ss a", java.util.Locale.ENGLISH));
        }
        return text(o);
    }

    /** GetDetailByHeaderId (grdhistory_SelectionChanged): the desktop's detail columns. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = owned(u, id);
        List<Map<String, Object>> out = new ArrayList<>();
        if (h == null || !exportDo(h)) return out;
        for (Map<String, Object> d : repo.details(id)) {
            out.add(row("Id", asInt(ci(d, "Id")), "InvocieNo", text(ci(d, "InvoiceNo")), "CustomerName", text(ci(d, "SupplierCustomer")),
                    "OrderNo", text(ci(d, "OrderNo")), "WareHouse", text(ci(d, "WareHouseName")), "JobLot", text(ci(d, "JobLotDescription")),
                    "Item", text(ci(d, "ItemName")), "ItemUOM", text(ci(d, "PackUOM")), "PackingType", text(ci(d, "PackTypeDesc")),
                    "QTY", asDouble(ci(d, "DoQty")), "Weight", asDouble(ci(d, "DoWeight")), "PackingWeight", asDouble(ci(d, "PackingWeight")),
                    "TotalPackingWeight", asDouble(ci(d, "TotalPackingWeight")), "GrossWeight", asDouble(ci(d, "GrossWeight")),
                    "OtherWeight", asDouble(ci(d, "OtherWeight")), "LoadQty", asDouble(ci(d, "LoadingQty")), "LoadWeight", asDouble(ci(d, "LoadingWeight")),
                    "ContainerNo", text(ci(d, "Container")), "ContainerRemarks", text(ci(d, "ContainerRemarks")),
                    "InspectionRemarks", text(ci(d, "InspectionRemarks")), "Remarks", text(ci(d, "LoadingRemarks"))));
        }
        return out;
    }

    // ============================================================================ save

    /** Insert() (btnsave_Click sets Id = 0 first; btnupdate_Click keeps the loaded Id). */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        Map<String, Object> existing = null;
        if (recId > 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        List<Map<String, Object>> branchList = repo.branches(u);
        boolean branchVisible = branchList.size() > 1;
        /* FormValidation */
        if (branchVisible && asInt(b.get("branchId")) == 0) throw new IllegalArgumentException("Brnach field is required");
        String docNo = text(b.get("docNo")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("DocNo Field is Required");
        if (asDouble(b.get("itemWeight")) == 0) throw new IllegalArgumentException("Item Weight Field is Required");
        if (asDouble(b.get("packingWeight")) == 0) throw new IllegalArgumentException("Packing Weight Field is Required");
        if (asDouble(b.get("totalGrossWeight")) == 0) throw new IllegalArgumentException("Total Gross Weight Field is Required");
        if (asDouble(b.get("otherWeight")) > 0 && text(b.get("otherRemarks")).isEmpty()) throw new IllegalArgumentException("Other Remarks Field is Required");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        boolean containerCompulsory = asBool(repo.config(u, "ContainerNoCompulsoryOnDeliveryOrderExport"));

        /* OtherWeightProportion */
        double netWeight = 0;
        for (Map<String, Object> r : rows) netWeight += asDouble(r.get("Weight"));
        double otherHeader = asDouble(b.get("otherWeight"));

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("removed"))) details.add(removedModel(r));
        double totalQty = 0;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            Map<String, Object> d = detailModel();
            if (recId == 0) d.put("ActionTypeId", 1);
            else {
                d.put("Id", asInt(r.get("Id")));
                d.put("ActionTypeId", asInt(d.get("Id")) > 0 ? 2 : 1);
            }
            d.put("ExImInvoiceId", asInt(r.get("InvoiceId")));
            d.put("InvoiceDetailId", asInt(r.get("InvoiceDetailId")));
            d.put("SupplierCustomerId", asInt(r.get("SupplierCustomerId")));
            d.put("SaleOrderId", asInt(r.get("OrderId")));
            d.put("WarehouseId", asInt(r.get("WareHouseId")));
            d.put("JobLotId", asInt(r.get("JobLotId")));
            d.put("CropYearId", asInt(r.get("CropYearId")));
            d.put("ItemId", asInt(r.get("ItemId")));
            d.put("PackUomId", asInt(r.get("ItemUOMId")));
            d.put("InvPackingTypeId", asInt(r.get("PackingTypeId")));
            d.put("InnerQty", asDouble(d.get("DoQty")));                 /* DoQty is still 0 here (desktop order) */
            d.put("InnerUomId", (double) asInt(d.get("PackUomId")));
            d.put("InnerEbUnit", asDouble(r.get("PackingWeight")));
            d.put("InnerEbTotal", asDouble(r.get("TotalPackingWeight")));
            d.put("OuterEbTotal", asDouble(r.get("TotalPackingWeight")));
            double doQty = asDouble(r.get("LoadQty"));
            d.put("DoQty", doQty);
            d.put("DoWeight", asDouble(r.get("LoadWeight")));
            d.put("LoadingQty", doQty);
            d.put("LoadingWeight", asDouble(r.get("LoadWeight")));
            d.put("PackingWeight", asDouble(r.get("PackingWeight")));
            d.put("TotalPackingWeight", asDouble(r.get("TotalPackingWeight")));
            d.put("GrossWeight", asDouble(r.get("GrossWeight")));
            d.put("OtherWeight", otherHeader > 0 ? otherHeader / netWeight * asDouble(r.get("Weight")) : 0d);
            totalQty += doQty;
            if (containerCompulsory && asInt(r.get("ContainerId")) == 0) throw new IllegalArgumentException("Container No Required in Row No" + (i + 1));
            d.put("ContainerId", asInt(r.get("ContainerId")));
            d.put("ContainerRemarks", text(r.get("ContainerRemarks")));
            d.put("InspectionRemarks", text(r.get("InspectionRemarks")));
            d.put("LoadingRemarks", text(r.get("Remarks")));
            details.add(d);
        }
        /* BLL InvDeliveryOrder.Save */
        if (recId == 0) for (Map<String, Object> d : details)
            if (asInt(d.get("ActionTypeId")) != 1) throw new IllegalStateException("Record cannot be inserted because ActionTypeId not equal to 1");

        Timestamp now = now();
        Map<String, Object> po = new LinkedHashMap<>();       // Model 1006 InvDeliveryOrder, declaration order
        po.put("IsApproved", false);
        po.put("IsStockReserved", false);
        po.put("ApprovedDate", null);
        po.put("ExpiryDate", null);
        po.put("ReturnableDate", null);
        po.put("DocDate", pickerDate(b.get("docDate")));
        po.put("EntryDate", now);
        po.put("ModifyDate", now);
        po.put("DoTotalQty", BigDecimal.valueOf(totalQty));
        po.put("ApprovedUser", 0);
        po.put("TransporterId", asInt(b.get("transporterId")));
        po.put("BranchesId", branchVisible ? asInt(b.get("branchId")) : (u.getBranchesId() == null ? 0 : u.getBranchesId()));
        po.put("ToBranchId", 0);
        po.put("FromBranchId", 0);
        po.put("CompanyId", u.getCompanyId());
        po.put("DocNo", asInt(b.get("docNo")));
        po.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        po.put("EntryUser", u.getId());
        po.put("Id", recId);
        po.put("ModifyUser", recId > 0 ? u.getId() : 0);
        po.put("OrganizationId", u.getOrganizationId());
        po.put("ProjectsId", 0);
        po.put("EximInvoiceId", asInt(b.get("invoiceId")));
        po.put("FinancialYearId", fy());
        po.put("ActionId", recId == 0 ? 1 : 2);
        po.put("LoadingPortId", 0);
        po.put("SaleTypeId", 0);
        po.put("DepartmentFromId", 0);
        po.put("DepartmentToId", 0);
        po.put("RequestedByLookUpId", 0);
        po.put("ApprovedByLookUpId", 0);
        po.put("LoadingInstructions", text(b.get("remarks")));
        po.put("DeliveryOrderType", "Export");
        po.put("OtherWeightRemarks", text(b.get("otherRemarks")));
        po.put("AccountRemarks", null);
        po.put("VehicleNo", text(b.get("vehicleNo")).trim());
        po.put("ScreenName", FORM_NAME);
        po.put("VehicleType", null);
        po.put("GrossWeight", asDouble(b.get("totalGrossWeight")));
        po.put("NetWeight", asDouble(b.get("itemWeight")));
        po.put("PackingWeight", asDouble(b.get("packingWeight")));
        po.put("OtherWeight", asDouble(b.get("otherWeight")));
        po.put("AttachmentsValues", existing == null ? "" : text(ci(existing, "AttachmentsValues")));
        po.put("CustomAttachmentsValues", existing == null ? "" : text(ci(existing, "CustomAttachmentsValues")));

        repo.save(recId == 0 ? "Sp_InvDeliveryOrder_Insert" : "Sp_InvDeliveryOrder_Update", po, details);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + asInt(b.get("docNo")));
        return out;
    }

    /** grd_ColumnButtonClick "Delete" on a saved row: the fields the desktop copies, ActionTypeId 3. */
    private static Map<String, Object> removedModel(Map<String, Object> r) {
        Map<String, Object> d = detailModel();
        d.put("Id", asInt(r.get("Id")));
        d.put("ExImInvoiceId", asInt(r.get("InvoiceId")));
        d.put("SupplierCustomerId", asInt(r.get("SupplierCustomerId")));
        d.put("SaleOrderId", asInt(r.get("OrderId")));
        d.put("WarehouseId", asInt(r.get("WareHouseId")));
        d.put("JobLotId", asInt(r.get("JobLotId")));
        d.put("ItemId", asInt(r.get("ItemId")));
        d.put("CropYearId", asInt(r.get("CropYearId")));
        d.put("PackUomId", asInt(r.get("ItemUOMId")));
        d.put("InvPackingTypeId", asInt(r.get("PackingTypeId")));
        d.put("DoQty", asDouble(r.get("LoadQty")));
        d.put("DoWeight", asDouble(r.get("LoadWeight")));
        d.put("LoadingQty", asDouble(r.get("LoadQty")));
        d.put("LoadingWeight", asDouble(r.get("LoadWeight")));
        d.put("PackingWeight", asDouble(r.get("PackingWeight")));
        d.put("TotalPackingWeight", asDouble(r.get("TotalPackingWeight")));
        d.put("GrossWeight", asDouble(r.get("GrossWeight")));
        d.put("LoadingRemarks", text(r.get("Remarks")));
        d.put("InspectionRemarks", text(r.get("InspectionRemarks")));
        d.put("ContainerRemarks", text(r.get("ContainerRemarks")));
        d.put("ActionTypeId", 3);
        return d;
    }

    /** Model 1000 InvDeliveryOrderdetail - every non-virtual property, declaration order, default values. */
    static Map<String, Object> detailModel() {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[] { "DoQty", "DoWeight", "LoadingQty", "LoadingWeight", "PackingWeight", "TotalPackingWeight", "GrossWeight",
                "StockWeight", "OtherWeight", "InnerQty", "InnerUomId", "InnerEbUnit", "InnerEbTotal", "AccessWtSet", "OuterEbTotal" }) d.put(k, 0d);
        for (String k : new String[] { "Id", "InvDeliveryOrderId", "InvPackingTypeId", "ItemId", "PackUomId", "CastingTypeId", "ItemVariantId",
                "SaleOrderId", "SaleOrderDetailId", "InvoiceDetailId", "SupplierCustomerId", "WarehouseId", "WareHouseToId", "JobLotId",
                "ToJobLotId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "CropYearId", "ExImInvoiceId", "ActionTypeId",
                "BagTypeId", "ContainerId", "DeliveryTypeId", "AssetId" }) d.put(k, 0);
        for (String k : new String[] { "LoadingRemarks", "ContainerRemarks", "InspectionRemarks", "ItemDiscription" }) d.put(k, null);
        d.put("DeliveryScheduleId", 0);
        d.put("DeliveryScheduleDetailId", 0);
        d.put("ThirdPartyAnalysisSubId", 0);
        d.put("ThirdPartyAnalysisId", 0);
        d.put("IsAssetItem", false);
        return d;
    }
}
