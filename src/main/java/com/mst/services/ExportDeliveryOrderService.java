package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportDeliveryOrderRepository;
import com.mst.repositories.ExportForwardingRepository;
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

import static com.mst.repositories.ExportForwardingRepository.ci;
import static com.mst.repositories.ExportForwardingRepository.str;
import static com.mst.repositories.ExportForwardingRepository.toBool;
import static com.mst.repositories.ExportForwardingRepository.toDouble;
import static com.mst.repositories.ExportForwardingRepository.toInt;
import static com.mst.services.ExportForwardingService.at;
import static com.mst.services.ExportForwardingService.day;
import static com.mst.services.ExportForwardingService.iso;
import static com.mst.services.ExportForwardingService.isoTime;
import static com.mst.services.ExportForwardingService.list;
import static com.mst.services.ExportForwardingService.put;
import static com.mst.services.ExportForwardingService.row;

/**
 * BLL side of 208 "Export Delivery Order" - Architecture.WinApp.Export.ExportDeliveryOrderB (rights by its own
 * ScreenName "ExportDeliveryOrderB" = ScreenDefinition 208; DocumentTypeId 84, DeliveryOrderType "Export").
 * The form's validations are repeated here with the desktop's texts; session tenancy only.
 */
@Service
public class ExportDeliveryOrderService {

    public static final int SCREEN_ID = 208;
    public static final int DOCUMENT_TYPE_ID = 84;
    public static final String SCREEN_NAME = "ExportDeliveryOrderB";

    @Autowired private ExportDeliveryOrderRepository repo;
    @Autowired private ExportForwardingRepository common;
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
        out.put("userBranchId", toInt(u.getBranchesId()));
        put(out, "invoices", () -> invoiceCombo(u, 0));
        put(out, "packingTypes", () -> common.packingTypes());
        put(out, "docNo", () -> repo.generateCode(u, fy()));
        put(out, "warehouses", () -> common.activeWarehouses(u));
        put(out, "loadingPorts", () -> loadingPorts(u));
        put(out, "transporters", () -> transporters(u));
        put(out, "jobLots", () -> common.jobLots(u));
        put(out, "otherItems", () -> repo.itemsByType(u, "14"));
        put(out, "branches", () -> repo.branches(u));
        put(out, "historyCombos", () -> historyCombos(u));
        return out;
    }

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("SaleCostingJobOrderWise", toBool(common.config(u, "SaleCostingJobOrderWise")));
        c.put("IsInspectionLotMappedOnDeliveryOrderByInvoice", toBool(common.config(u, "IsInspectionLotMappedOnDeliveryOrderByInvoice")));
        c.put("IsMandatoryInspectionLotMappedOnDeliveryOrder", toBool(common.config(u, "IsMandatoryInspectionLotMappedOnDeliveryOrder")));
        c.put("DefaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("ContainerNoCompulsoryOnDeliveryOrderExport", toBool(common.config(u, "ContainerNoCompulsoryOnDeliveryOrderExport")));
        c.put("RestrictOneInvoiceOnDo", toBool(common.config(u, "RestrictOneInvoiceOnDo")));
        c.put("DefaultJobLotId", toInt(common.config(u, "Job/Lot")));
        c.put("DefaultWarehouseId", toInt(common.config(u, "Warehouse")));
        return c;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        put(out, "branches", () -> repo.branches(u));
        put(out, "packingTypes", () -> common.packingTypes());
        put(out, "warehouses", () -> common.activeWarehouses(u));
        put(out, "jobLots", () -> common.jobLots(u));
        put(out, "invoices", () -> invoiceCombo(u, recId));
        put(out, "loadingPorts", () -> loadingPorts(u));
        put(out, "transporters", () -> transporters(u));
        put(out, "otherItems", () -> repo.itemsByType(u, "14"));
        return out;
    }

    /** Reset(): DocumentNoFill + CmbInvoiceNoFill (RecId 0) + defaultConfiquration. */
    public Map<String, Object> reset() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "docNo", () -> repo.generateCode(u, fy()));
        put(out, "invoices", () -> invoiceCombo(u, 0));
        out.put("config", config(u));
        return out;
    }

    /** CmbInvoiceNoFill: getOutstandingExportInvoicesForDeliveryOrder (@DoId = RecId when > 0). */
    public List<Map<String, Object>> invoiceCombo(UserAccount u, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.outstandingInvoices(u, recId > 0 ? recId : 0)) out.add(row("Id", toInt(ci(r, "Id")), "InvoiceNo", str(ci(r, "InvoiceNo"))));
        return out;
    }

    public List<Map<String, Object>> invoices(int recId) { return invoiceCombo(user("View"), recId); }

    /** CmbLoadingPortFill: CommonServices.GetSeaPortForComboServiceBind, PortTypeId 2 only. */
    private List<Map<String, Object>> loadingPorts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : common.seaPorts(u)) if (toInt(ci(r, "PortTypeId")) == 2) out.add(row("Id", toInt(ci(r, "Id")), "PortName", str(ci(r, "PortName"))));
        return out;
    }

    private List<Map<String, Object>> transporters(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : common.suppliersByGroup(u, "10")) out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName"))));
        return out;
    }

    /** HistoryCombosFill: Activity "Customer" / "InvoiceNo". */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> c = new ArrayList<>(), i = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u, fy())) {
            String a = str(ci(r, "Activity"));
            Map<String, Object> m = row("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "ReferenceName")));
            if ("Customer".equals(a)) c.add(m); else if ("InvoiceNo".equals(a)) i.add(m);
        }
        return row("customers", c, "invoices", i);
    }

    public Map<String, Object> historyCombosRefresh() { return historyCombos(user("View")); }

    // ============================================================================ detail events

    /**
     * cmbInvoiceNo_Leave: GetDatabyInvoiceId (header data -> invoice date and customer, contracts -> Sale Contract No),
     * GetInvoicewiseDoWeightandBalanceWeight, GetContainerNoFromCROByInvoiceId (only for an invoice), ThirdPartyNoBind.
     */
    public Map<String, Object> invoice(int invoiceId, int recId, int itemId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", repo.invoiceHeader(u, invoiceId));
        List<Map<String, Object>> contracts = new ArrayList<>();
        for (Map<String, Object> r : repo.contractsByInvoice(u, invoiceId)) contracts.add(row("OrderId", toInt(ci(r, "ContractId")), "OrderNo", str(ci(r, "ContractNo"))));
        out.put("contracts", contracts);
        out.put("weights", weights(u, invoiceId));
        out.put("containers", invoiceId != 0 ? containerRows(u, invoiceId, recId) : null);
        out.put("inspections", inspectionRows(u, itemId, invoiceId));
        return out;
    }

    private Map<String, Object> weights(UserAccount u, int invoiceId) {
        List<Map<String, Object>> r = repo.invoiceWeights(u, invoiceId);
        Map<String, Object> m = new LinkedHashMap<>();
        if (r.isEmpty()) {
            for (String k : new String[]{"InvoiceWeight", "DoWeight", "GpRejectedWeight", "WeightAvailableForDo", "ReturnWeight"}) m.put(k, "0");
        } else {
            for (String k : new String[]{"InvoiceWeight", "DoWeight", "GpRejectedWeight", "WeightAvailableForDo", "ReturnWeight"}) m.put(k, str(ci(r.get(0), k)));
        }
        return m;
    }

    /** cmbInvoiceNo_TextChanged: GetInvoicewiseDoWeightandBalanceWeight. */
    public Map<String, Object> invoiceWeights(int invoiceId) { return weights(user("View"), invoiceId); }

    private List<Map<String, Object>> containerRows(UserAccount u, int invoiceId, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.containers(u, invoiceId, recId)) out.add(row("ContainerId", toInt(ci(r, "ContainerId")), "ContainerNo", str(ci(r, "Container"))));
        return out;
    }

    /** grd_KeyDown F1 on ContainerNo - the same list (GetContainerNoFromShipingLineBookingDetailByInvoiceId). */
    public List<Map<String, Object>> containers(int invoiceId, int recId) {
        UserAccount u = user("View");
        if (invoiceId <= 0) throw new IllegalArgumentException("InvoiceId not found");
        return containerRows(u, invoiceId, recId);
    }

    private List<Map<String, Object>> inspectionRows(UserAccount u, int itemId, int invoiceId) {
        if (itemId > 0 && invoiceId > 0) return repo.inspections(u, fy(), itemId, invoiceId);
        return new ArrayList<>();
    }

    /** CmbOrderNo_TextChanged: ExportInvoiceDetailByHeaderIdNew (items, crop years) when both ids are set. */
    public List<Map<String, Object>> contractItems(int invoiceId, int orderId) {
        UserAccount u = user("View");
        if (invoiceId > 0 && orderId > 0) return repo.invoiceDetailByHeader(u, orderId, invoiceId);
        return new ArrayList<>();
    }

    /**
     * CmbItemName_TextChanged: BindPackUomAndPackingType(InvoiceDetailId) (USP_GetInvoiceDetailForDetailId), the
     * item's UOM schedule (BindInnerPackUom when no inner list came back) and ThirdPartyNoBind.
     */
    public Map<String, Object> item(int invoiceDetailId, int itemId, int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("detail", repo.invoiceDetailForDetailId(invoiceDetailId));
        out.put("uoms", itemId > 0 ? common.uomScheduleByItem(u, itemId) : new ArrayList<>());
        out.put("inspections", inspectionRows(u, itemId, invoiceId));
        return out;
    }

    /** CmbInspectionNoPackListDetail_Leave -> ThirdPartySubLotNoBind. */
    public List<Map<String, Object>> subLots(int analysisId, int invoiceId) {
        UserAccount u = user("View");
        if (analysisId > 0) return repo.subLots(u, analysisId, invoiceId);
        return new ArrayList<>();
    }

    /** grd_KeyDown F1 on InnerUOM - CommonServices.GetUomScheduleByItemId ("ItemId not found" when empty). */
    public List<Map<String, Object>> itemUoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> r = common.uomScheduleByItem(u, itemId);
        if (r.isEmpty()) throw new IllegalArgumentException("ItemId not found");
        return r;
    }

    /** btnCroForm_Click: GetBookingCROIdByInvoiceId. */
    public int croId(int invoiceId) { return repo.croIdByInvoice(user("View"), invoiceId); }

    // ============================================================================ Load Invoices popup

    /** LoadCommercialInvoiceForDO: RestrictOneInvoiceOnDo, combos (ActivityType Invoice / ContractNo / Customer / Items / JobLot / Crop). */
    public Map<String, Object> loaderSetup() {
        UserAccount u = user("View");
        Map<String, List<Map<String, Object>>> map = new LinkedHashMap<>();
        for (String k : new String[]{"Invoice", "ContractNo", "Customer", "Items", "JobLot", "Crop"}) map.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.loaderCombos(u)) {
            String t = str(ci(r, "ActivityType"));
            for (Map.Entry<String, List<Map<String, Object>>> e : map.entrySet())
                if (e.getKey().equalsIgnoreCase(t)) e.getValue().add(row("Id", toInt(ci(r, "Id")), "name", str(ci(r, "name"))));
        }
        Map<String, Object> out = new LinkedHashMap<>(map);
        out.put("RestrictOneInvoiceOnDo", toBool(common.config(u, "RestrictOneInvoiceOnDo")));
        out.put("fromDate", iso(repo.financialYearStart(fy())));
        return out;
    }

    /** GridRecordsDBCall: LoadExportInvoiceDataForDeliveryOrder (the page groups by invoice, as GridRecordsFill does). */
    public List<Map<String, Object>> loaderData(Map<String, Object> f) {
        UserAccount u = user("View");
        return repo.loaderData(u, day(f.get("fromDate"), false), day(f.get("toDate"), true), toInt(f.get("invoiceId")),
                toInt(f.get("contractId")), toInt(f.get("customerId")), toInt(f.get("itemId")), toInt(f.get("jobLotId")), toInt(f.get("cropYearId")));
    }

    /** LoadOtherItemsData(InvoiceIds) - ExImInvoice.InvoiceOtherItemsByInvoiceIds (",id,id"). */
    public List<Map<String, Object>> loaderOtherItems(String ids) { user("View"); return repo.otherItemsByInvoiceIds(ids); }

    // ============================================================================ read / history

    /** ReadById: InvDeliveryOrder.GetByID (+ CmbInvoiceNoFill with the RecId). */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = owned(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        if (h == null) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        out.put("header", h);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            Map<String, Object> m = new LinkedHashMap<>(d);
            /* ReadById: TotalPackingWeight 0 -> TotalPackingWeight = PackingWeight and, when > 0,
               PackingWeight = TotalPackingWeight / LoadingQty. */
            double tpw = toDouble(ci(d, "TotalPackingWeight"));
            if (tpw == 0) {
                double pw = toDouble(ci(d, "PackingWeight"));
                if (pw > 0) m.put("PackingWeight", pw / toDouble(ci(d, "LoadingQty")));
            }
            det.add(m);
        }
        out.put("details", det);
        out.put("expenses", repo.expenses(id));
        out.put("invoices", invoiceCombo(u, id));
        return out;
    }

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID || !"Export".equals(str(ci(h, "DeliveryOrderType")))) return null;
        return h;
    }

    /** gridhistoryfill: ExportFormHistoryNew, one row per Id (the desktop skips repeated Ids). */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        boolean all = allowed(u, "CanView AllRecord");
        Timestamp from = toBool(f.get("fromChecked")) ? day(f.get("fromDate"), false) : null;
        Timestamp to = toBool(f.get("toChecked")) ? day(f.get("toDate"), true) : null;
        String mode = str(f.get("dateMode"));
        List<Map<String, Object>> rows = repo.formHistory(u, fy(), all,
                "doc".equals(mode) ? from : null, "doc".equals(mode) ? to : null,
                "entry".equals(mode) ? from : null, "entry".equals(mode) ? to : null,
                "modify".equals(mode) ? from : null, "modify".equals(mode) ? to : null,
                "approved".equals(mode) ? from : null, "approved".equals(mode) ? to : null,
                toInt(f.get("fromDocNo")), toInt(f.get("toDocNo")), toInt(f.get("customerId")), toInt(f.get("invoiceId")));
        List<Map<String, Object>> out = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(ci(r, "Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("DoDate", iso(ci(r, "DocDate")));
            m.put("DoNo", toInt(ci(r, "DocNo")));
            m.put("InvoiceId", toInt(ci(r, "InvoiceId")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("InvoiceNo", str(ci(r, "InvoiceNo")));
            m.put("InvoiceNetWeight", toDouble(ci(r, "InvoiceNetWeight")));
            m.put("InvoiceGrossWeight", toDouble(ci(r, "InvoiceGrossWeight")));
            m.put("CustomerName", str(ci(r, "SupplierCustomer")));
            m.put("OrderNo", str(ci(r, "OrderNo")));
            m.put("GatePassId", toInt(ci(r, "GatePassId")));
            m.put("VehicleNo", str(ci(r, "VehicleNo")));
            m.put("Transporter", str(ci(r, "Transporter")));
            m.put("LoadingPort", str(ci(r, "LoadingPort")));
            m.put("GpDate", iso(ci(r, "GpDate")));
            m.put("GpSrNo", ci(r, "GpSrNo") == null ? "" : String.valueOf(toInt(ci(r, "GpSrNo"))));
            m.put("GpStatus", str(ci(r, "GpStatus")));
            m.put("ForwardingNo", ci(r, "ForwardingNo") == null ? "" : String.valueOf(toInt(ci(r, "ForwardingNo"))));
            m.put("FactoryWeight", toDouble(ci(r, "FactoryWeight")));
            m.put("EntryDate", isoTime(ci(r, "EntryDate")));
            m.put("EntryUser", str(ci(r, "EntryUser")));
            m.put("ModifyDate", isoTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", str(ci(r, "ModifyUser")));
            m.put("ApprovedDate", isoTime(ci(r, "ApprovedDate")));
            m.put("ApprovedUser", str(ci(r, "ApprovedUser")));
            m.put("BranchName", str(ci(r, "BranchName")));
            m.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GetDetailByHeaderId (grdhistory_SelectionChanged). */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = user("View");
        if (owned(u, id) == null) return new ArrayList<>();
        return repo.details(id);
    }

    // ============================================================================ save

    /**
     * Insert(): FormValidation, "Grid Record Not Found", then the detail rows' checks in the desktop's order,
     * InvDeliveryOrder.Save (Id 0: every detail must carry ActionTypeId 1 - "Record cannot be inserted because
     * ActionTypeId not equal to 1"; Sp_InvDeliveryOrder_Insert, else Sp_InvDeliveryOrder_Update) -> DAL 0411 SetData.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = toInt(b.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        Map<String, Object> existing = null;
        if (recId > 0) { existing = owned(u, recId); if (existing == null) throw new IllegalArgumentException("Record Not Found"); }
        boolean branchVisible = toBool(b.get("branchVisible"));
        if (branchVisible && toInt(b.get("branchId")) == 0) throw new IllegalArgumentException("Branch field is required");
        String docNo = str(b.get("docNo")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("DocNo Field is Required");
        if (toDouble(b.get("itemWeight")) == 0) throw new IllegalArgumentException("Item Weight Field is Required");
        if (toDouble(b.get("totalGrossWeight")) == 0) throw new IllegalArgumentException("Total Gross Weight Field is Required");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        boolean jobOrderWise = toBool(common.config(u, "SaleCostingJobOrderWise"));
        boolean byInvoice = toBool(common.config(u, "IsInspectionLotMappedOnDeliveryOrderByInvoice"));
        boolean mandatory = toBool(common.config(u, "IsMandatoryInspectionLotMappedOnDeliveryOrder"));
        boolean containerCompulsory = toBool(common.config(u, "ContainerNoCompulsoryOnDeliveryOrderExport"));
        boolean oneInvoice = toBool(common.config(u, "RestrictOneInvoiceOnDo"));

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("removed"))) details.add(removedModel(r));
        double totalQty = 0;
        int invoiceId = 0;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int n = i + 1;
            String sfx = " in Detail Grid Row no : " + n;
            if (toInt(r.get("InvoiceId")) == 0) throw new IllegalArgumentException("InvoiceNo field is required" + sfx);
            if (toInt(r.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer field is required" + sfx);
            if (toInt(r.get("OrderId")) == 0) throw new IllegalArgumentException("OrderNo field is required" + sfx);
            if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("Warehouse field is required" + sfx);
            if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("ItemName field is required" + sfx);
            if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot field is required" + sfx);
            if (toInt(r.get("CropYearId")) == 0) throw new IllegalArgumentException("CropYear field is required" + sfx);
            if (toInt(r.get("PackingTypeId")) == 0) throw new IllegalArgumentException("Packing type field is required" + sfx);
            if (toDouble(r.get("MasterQTY")) == 0) throw new IllegalArgumentException("Master Qty field is required" + sfx);
            if (toInt(r.get("MasterUOMId")) == 0) throw new IllegalArgumentException("Master PackUOM field is required" + sfx);
            if (toDouble(r.get("NetWeight")) == 0) throw new IllegalArgumentException("Net Weight field is required" + sfx);
            Map<String, Object> d = detailModel();
            if (recId == 0) d.put("ActionTypeId", 1);
            else {
                d.put("Id", toInt(r.get("Id")));
                d.put("ActionTypeId", toInt(d.get("Id")) > 0 ? 2 : 1);
            }
            d.put("ExImInvoiceId", toInt(r.get("InvoiceId")));
            if (invoiceId == 0) invoiceId = toInt(d.get("ExImInvoiceId"));
            else if (oneInvoice && invoiceId != toInt(d.get("ExImInvoiceId"))) throw new IllegalArgumentException("All Detail Rows Must Have Same Invoice No");
            d.put("InvoiceDetailId", toInt(r.get("InvoiceDetailId")));
            d.put("SupplierCustomerId", toInt(r.get("SupplierCustomerId")));
            d.put("SaleOrderId", toInt(r.get("OrderId")));
            d.put("WarehouseId", toInt(r.get("WareHouseId")));
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("CropYearId", toInt(r.get("CropYearId")));
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("InvPackingTypeId", toInt(r.get("PackingTypeId")));
            d.put("PackUomId", toInt(r.get("MasterUOMId")));
            d.put("PackingWeight", toDouble(r.get("EbUnitMaster")));
            d.put("OuterEbTotal", toDouble(r.get("EbTotalMaster")));
            double doQty = toDouble(r.get("LoadQty"));
            d.put("DoQty", doQty);
            d.put("InnerQty", toDouble(r.get("InnerQTY")) <= 0 ? doQty : toDouble(r.get("InnerQTY")));
            d.put("InnerUomId", toInt(r.get("InnerUOMId")) <= 0 ? (double) toInt(r.get("MasterUOMId")) : (double) toInt(r.get("InnerUOMId")));
            d.put("InnerEbUnit", toDouble(r.get("EbUnitInner")));
            d.put("InnerEbTotal", toDouble(r.get("EbTotalInner")));
            d.put("AccessWtSet", toDouble(r.get("AccessWtSet")));
            d.put("DoWeight", toDouble(r.get("NetWeight")));
            d.put("LoadingQty", doQty);
            d.put("LoadingWeight", toDouble(r.get("NetWeight")));
            d.put("TotalPackingWeight", toDouble(d.get("InnerEbTotal")) + toDouble(d.get("OuterEbTotal")));
            d.put("GrossWeight", toDouble(r.get("GrossWeight")));
            int analysis = toInt(r.get("ThirdPartyAnalysisId"));
            d.put("ThirdPartyAnalysisId", analysis);
            if (jobOrderWise && analysis == 0) throw new IllegalArgumentException("Inspection No field is required" + sfx);
            int sub = toInt(r.get("ThirdPartyAnalysisSubId"));
            d.put("ThirdPartyAnalysisSubId", sub);
            if (mandatory) {
                if (analysis == 0) throw new IllegalArgumentException("Inspection No field is required" + sfx);
                if (byInvoice && sub == 0) throw new IllegalArgumentException("Inspection Sub Lot No field is required" + sfx);
            }
            d.put("OtherWeight", toDouble(r.get("OtherWeight")));
            totalQty += doQty;
            if (containerCompulsory && toInt(r.get("ContainerId")) == 0) throw new IllegalArgumentException("Container No Required in Row No" + n);
            d.put("ContainerId", toInt(r.get("ContainerId")));
            d.put("ContainerRemarks", str(r.get("ContainerRemarks")));
            d.put("InspectionRemarks", str(r.get("InspectionRemarks")));
            d.put("LoadingRemarks", str(r.get("Remarks")));
            details.add(d);
        }
        List<Map<String, Object>> expenses = new ArrayList<>();
        int k = 0;
        for (Map<String, Object> o : list(b.get("others"))) {
            k++;
            if (toInt(o.get("ItemId")) <= 0) continue;
            if (toDouble(o.get("Qty")) == 0) throw new IllegalArgumentException("Qty Required In Other Item Grid Row No : " + k);
            Map<String, Object> e = new LinkedHashMap<>();       // Model 0968 InvDeliveryOrderExpense
            e.put("Id", toInt(o.get("Id")));
            e.put("SaleOrderId", 0);
            e.put("SaleOrderCustomerExpId", 0);
            e.put("ExImInvoiceId", toInt(o.get("ExImInvoiceId")));
            e.put("InvoiceOtherItemDetailId", toInt(o.get("InvoiceDetailId")));
            e.put("InvDeliveryOrderId", 0);
            e.put("ItemId", toInt(o.get("ItemId")));
            e.put("Qty", toDouble(o.get("Qty")));
            e.put("WeightPerQty", toDouble(o.get("WeightPerQty")));
            e.put("NetWeight", toDouble(o.get("NetWeight")));
            e.put("Remarks", str(o.get("Remarks")));
            expenses.add(e);
        }
        if (recId == 0) for (Map<String, Object> d : details)
            if (toInt(d.get("ActionTypeId")) != 1) throw new IllegalStateException("Record cannot be inserted because ActionTypeId not equal to 1");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> po = new LinkedHashMap<>();       // Model 1006 InvDeliveryOrder, declaration order
        po.put("IsApproved", false);
        po.put("IsStockReserved", false);
        po.put("ApprovedDate", null);
        po.put("ExpiryDate", null);
        po.put("ReturnableDate", null);
        po.put("DocDate", at(b.get("docDate")));
        po.put("EntryDate", now);
        po.put("ModifyDate", now);
        po.put("DoTotalQty", BigDecimal.valueOf(totalQty));
        po.put("ApprovedUser", 0);
        po.put("TransporterId", toInt(b.get("transporterId")));
        int branches = repo.branches(u).size();
        po.put("BranchesId", branches > 1 ? toInt(b.get("branchId")) : toInt(u.getBranchesId()));
        po.put("ToBranchId", 0);
        po.put("FromBranchId", 0);
        po.put("CompanyId", u.getCompanyId());
        po.put("DocNo", toInt(b.get("docNo")));
        po.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        po.put("EntryUser", toInt(u.getId()));
        po.put("Id", recId);
        po.put("ModifyUser", recId > 0 ? toInt(u.getId()) : 0);
        po.put("OrganizationId", u.getOrganizationId());
        po.put("ProjectsId", 0);
        po.put("EximInvoiceId", toInt(b.get("invoiceId")));
        po.put("FinancialYearId", fy());
        po.put("ActionId", recId == 0 ? 1 : 2);
        po.put("LoadingPortId", toInt(b.get("loadingPortId")));
        po.put("SaleTypeId", 0);
        po.put("DepartmentFromId", 0);
        po.put("DepartmentToId", 0);
        po.put("RequestedByLookUpId", 0);
        po.put("ApprovedByLookUpId", 0);
        po.put("LoadingInstructions", str(b.get("remarks")));
        po.put("DeliveryOrderType", "Export");
        po.put("OtherWeightRemarks", str(b.get("otherRemarks")));
        po.put("AccountRemarks", null);
        po.put("VehicleNo", str(b.get("vehicleNo")).trim());
        po.put("ScreenName", SCREEN_NAME);
        po.put("VehicleType", null);
        po.put("GrossWeight", toDouble(b.get("totalGrossWeight")));
        po.put("NetWeight", toDouble(b.get("itemWeight")));
        po.put("PackingWeight", toDouble(b.get("packingWeight")));
        po.put("OtherWeight", toDouble(b.get("otherWeight")));
        po.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        po.put("CustomAttachmentsValues", existing == null ? "" : str(ci(existing, "CustomAttachmentsValues")));

        int success = repo.save(recId == 0 ? "Sp_InvDeliveryOrder_Insert" : "Sp_InvDeliveryOrder_Update", po, details, expenses);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", success);
        out.put("docNo", toInt(b.get("docNo")));
        out.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(b.get("docNo")));
        return out;
    }

    /** grd_ColumnButtonClick "Delete" on a saved row: the fields the desktop copies, ActionTypeId 3. */
    private static Map<String, Object> removedModel(Map<String, Object> r) {
        Map<String, Object> d = detailModel();
        d.put("Id", toInt(r.get("Id")));
        d.put("ExImInvoiceId", toInt(r.get("InvoiceId")));
        d.put("SupplierCustomerId", toInt(r.get("SupplierCustomerId")));
        d.put("SaleOrderId", toInt(r.get("OrderId")));
        d.put("WarehouseId", toInt(r.get("WareHouseId")));
        d.put("JobLotId", toInt(r.get("JobLotId")));
        d.put("CropYearId", toInt(r.get("CropYearId")));
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("InvPackingTypeId", toInt(r.get("PackingTypeId")));
        d.put("PackUomId", toInt(r.get("MasterUOMId")));
        d.put("PackingWeight", toDouble(r.get("EbUnitMaster")));
        d.put("OuterEbTotal", toDouble(r.get("EbTotalMaster")));
        d.put("InnerQty", toDouble(r.get("InnerQTY")));
        d.put("InnerUomId", (double) toInt(r.get("InnerUOMId")));
        d.put("InnerEbUnit", toDouble(r.get("EbUnitInner")));
        d.put("InnerEbTotal", toDouble(r.get("EbTotalInner")));
        d.put("AccessWtSet", toDouble(r.get("AccessWtSet")));
        d.put("DoQty", toDouble(r.get("LoadQty")));
        d.put("DoWeight", toDouble(r.get("LoadWeight")));
        d.put("LoadingQty", toDouble(r.get("LoadQty")));
        d.put("LoadingWeight", toDouble(r.get("LoadWeight")));
        d.put("TotalPackingWeight", toDouble(d.get("InnerEbTotal")) + toDouble(d.get("OuterEbTotal")));
        d.put("GrossWeight", toDouble(r.get("GrossWeight")));
        d.put("ThirdPartyAnalysisId", toInt(r.get("ThirdPartyAnalysisId")));
        d.put("ThirdPartyAnalysisSubId", toInt(r.get("ThirdPartyAnalysisSubId")));
        d.put("LoadingRemarks", str(r.get("Remarks")));
        d.put("InspectionRemarks", str(r.get("InspectionRemarks")));
        d.put("ContainerRemarks", str(r.get("ContainerRemarks")));
        d.put("ActionTypeId", 3);
        return d;
    }

    /** Model 1000 InvDeliveryOrderdetail - every non-virtual property, declaration order, default values. */
    static Map<String, Object> detailModel() {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[]{"DoQty", "DoWeight", "LoadingQty", "LoadingWeight", "PackingWeight", "TotalPackingWeight", "GrossWeight",
                "StockWeight", "OtherWeight", "InnerQty", "InnerUomId", "InnerEbUnit", "InnerEbTotal", "AccessWtSet", "OuterEbTotal"}) d.put(k, 0d);
        for (String k : new String[]{"Id", "InvDeliveryOrderId", "InvPackingTypeId", "ItemId", "PackUomId", "CastingTypeId", "ItemVariantId",
                "SaleOrderId", "SaleOrderDetailId", "InvoiceDetailId", "SupplierCustomerId", "WarehouseId", "WareHouseToId", "JobLotId",
                "ToJobLotId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "CropYearId", "ExImInvoiceId", "ActionTypeId",
                "BagTypeId", "ContainerId", "DeliveryTypeId", "AssetId"}) d.put(k, 0);
        for (String k : new String[]{"LoadingRemarks", "ContainerRemarks", "InspectionRemarks", "ItemDiscription"}) d.put(k, null);
        d.put("DeliveryScheduleId", 0);
        d.put("DeliveryScheduleDetailId", 0);
        d.put("ThirdPartyAnalysisSubId", 0);
        d.put("ThirdPartyAnalysisId", 0);
        d.put("IsAssetItem", false);
        return d;
    }
}
