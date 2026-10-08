package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 538 "DeliveryOrder_Engr" = Architecture.WinApp.SaleTrading.DeliveryOrder_Engr (Sale Engr, module 83, document type 1606).
 *
 * Desktop map (DeliveryOrder_Engr.cs, logic lines before InitializeComponent, 3148 lines):
 *   PurchsaeOrder_Load :191, HistoryComboFill :285, SupplierNameFill :346, vehicleTypefill :390, AssetFill :405, WareHouseFill :437,
 *   JobLotFill :481, DeliveryTypeFill :496, RequestedByAndApprovedByFill :512, DocumentNoFill :565, ItemDetailFillWithoutOrderId :580,
 *   bindRateUomAndItemPackUom :634, CmbItemName_Leave :668, FormValidation :685, FormValidationDetail :725, btnplus_Click :789,
 *   grdSettings :813, grd_ColumnButtonClick :893, btnUpdateDetail_Click :986, Insert :1052, btnSaveAs_Click :1270, ReadById :1307,
 *   Reset :1392, WareHouseToShowHide :1513, KeyDown :1562, gridhistoryfill :1852, HistoryGridSettings :1970, LoadInGridDetail :2310,
 *   btnLoaderView2_Click :2373, btnLoadSaleOrder_Click :2411, SaleOrderBind :2455, ItemBindbyOrderId :2491, CmbDeliveryType_TextChanged :2618,
 *   CmbDeliveryType_Leave :2660, GetOutStandingParties :2692, GetOutStandingOrderAndParties :2738, btnDelete_Click :2821.
 *
 * Differences from the base DeliveryOrder (Sale module): no branch / packing / weight / crop-year / sale-type / transporter / stock-reserved /
 * expense grids; the detail row carries WareHouse, JobLot, Machine/Asset and a quantity only; quantity doubles as weight; the document type is
 * 1606 and the delivery types come from SpStaticColumnNames 'DeliveryType' (1 Sale Order, 2 .., 3 Machine/Asset, 5 Transfer).
 */
@Service
public class SaleDeliveryOrderEngrService {
    public static final String SCREEN = "DeliveryOrder_Engr";
    public static final int DOC_TYPE = 1606;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public SaleDeliveryOrderEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                                       // DocumentNoFill
        m.put("deliveryTypes", sup.rows("SpStaticColumnNames", "Activity", "DeliveryType"));   // DeliveryTypeFill
        m.put("customers", customers());                                                 // SupplierNameFill
        m.put("items", itemsWithoutOrder());                                             // ItemDetailFillWithoutOrderId
        m.put("vehicleTypes", sup.rows("Sp_VehicleType_GetAllMethod"));                  // VehicleType.GetAll()
        m.put("warehouses", warehouses());                                               // WareHouseFill
        m.put("jobLots", jobLots());                                                     // JobLotFill
        m.put("assets", assets());                                                       // AssetFill
        m.put("lookups", lookups());                                                     // RequestedByAndApprovedByFill
        m.put("outstanding", outstanding(0));                                            // GetOutStandingOrderAndParties
        m.put("history", historyCombos());                                               // HistoryComboFill
        return m;
    }

    public int nextNo() {
        // CommonServices.DeliveryOrderGenerateCode(1606) -> InvDeliveryOrder.GenerateCode
        List<Map<String, Object>> r = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> customers() {
        if (sup.erpFeature(4)) return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company());
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** CommonServices.ItemGetForComboServiceBindForEngr -> dtitem rows (Id, ItemName, ItemCode = ItemCodeNew, SaleOrderDetailId 0, BalQty 0). */
    public List<Map<String, Object>> itemsWithoutOrder() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForEngr"))
            out.add(row("Id", r.get("Id"), "ItemName", r.get("ItemName"), "ItemCode", r.get("ItemCodeNew"), "SaleOrderDetailId", 0, "BalQty", 0));
        return out;
    }

    public List<Map<String, Object>> warehouses() {
        return sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse");
    }

    public List<Map<String, Object>> jobLots() {
        return sup.rows("SP_JobLot_ReadMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    public List<Map<String, Object>> assets() {
        return sup.rows("Sp_FixedAssetsRegister_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll");
    }

    public List<Map<String, Object>> lookups() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvLookup_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "InvLookupTypeId", 13, "Activity", "ReadByInvlookTypeId"))
            out.add(row("Id", r.get("Id"), "Type", r.get("LookupName")));
        return out;
    }

    /** HistoryComboFill: InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (org + company only), split by the Activity column. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), types = new ArrayList<>(), req = new ArrayList<>(), app = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromDeliveryOrder", "OrganizationId", sup.org(), "CompanyId", sup.company())) {
            String a = str(r.get("Activity"));
            Map<String, Object> o = row("Id", r.get("Id"), "Name", r.get("ReferenceName"));
            switch (a) {
                case "Customer" -> cust.add(o);
                case "DeliveryTypeDetail" -> types.add(o);
                case "RequestedBy" -> req.add(o);
                case "ApprovedBy" -> app.add(o);
                default -> { }
            }
        }
        return row("customers", cust, "deliveryTypes", types, "requestedBy", req, "approvedBy", app);
    }

    /** SaleOrder.GetOutstandOrdersAndPartiesForIssuanceGdn (@RecId only when the record is open). */
    public List<Map<String, Object>> outstanding(int recId) {
        return sup.rows("USP_GetOutstandOrdersAndPartiesForIssuanceGdn", "OrganizationId", sup.org(), "CompanyId", sup.company(), "RecId", idOrNull(recId));
    }

    /** CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", r.get("Id"), "UOMCode", r.get("UOMCode"), "Equivalent", r.get("Equivalent"), "QtyEquivalent", r.get("QtyEquivalent"), "BaseRateUom", r.get("BaseRateUom")));
        return out;
    }

    // ------------------------------------------------------------------ History (gridhistoryfill)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo,
                                             int deliveryTypeId, int customerId, int requestedById, int approvedById) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        LocalDate from = parseDate(fromDate), to = parseDate(toDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE);
        p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", idOrNull(sup.fy()));
        if (!all) p.put("EntryUser", sup.userId());
        p.put("DeliveryOrderType", "Local");                       // base.Tag == "DeliveryOrder_Engr"
        String d = dateType == null ? "document" : dateType;
        String fk, tk;
        switch (d) {
            case "entry" -> { fk = "EntryFromDate"; tk = "EntryToDate"; }
            case "modify" -> { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
            case "approved" -> { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
            default -> { fk = "FromDate"; tk = "ToDate"; }
        }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        p.put("DocNoFrom", fromNo == 0 ? null : (double) fromNo);
        p.put("DocNoTo", toNo == 0 ? null : (double) toNo);
        p.put("SupplierCustomerId", idOrNull(customerId));
        p.put("DeliveryTypeId", idOrNull(deliveryTypeId));
        p.put("RequestedById", idOrNull(requestedById));
        p.put("ApprovedById", idOrNull(approvedById));
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "SP_DeliveryOrderFormHistory", p);
    }

    // ------------------------------------------------------------------ ReadById

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Delivery order not found in this company");
        return r.get(0);
    }

    private List<Map<String, Object>> details(int id) {
        return sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "Id", id, "Activity", "ReadByIdDetailIdForEngr");
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("outstanding", outstanding(id));                  // ReadById: GetOutStandingOrderAndParties with RecId
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Loader (SaleOrderLoadForDo)

    public Map<String, Object> loaderCombos() {
        UserAccount u = sup.user();
        List<Map<String, Object>> cust = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId())) {
            String a = str(r.get("Activity"));
            if ("Customer".equals(a)) cust.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
            if ("Item".equals(a)) items.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
        }
        return row("customers", cust, "items", items);
    }

    /** SaleOrder.SaleOrderLoadForDO_Engr with the loader's filters (document type 1605, approved only, zero balances skipped). */
    public List<Map<String, Object>> loaderOrders(String fromDate, String toDate, int customerId, int itemId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch()); p.put("FinancialYearId", sup.fy());
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        p.put("SupplierCustomerId", idOrNull(customerId));
        p.put("ItemId", idOrNull(itemId));
        p.put("DocumentTypeIds", "1605");
        p.put("IsApproved", true);
        p.put("SkipZero", 1);
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "USP_SaleOrderLoadForDO_Engr", p);
    }

    /** Start of the active financial year (FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    // ------------------------------------------------------------------ Save (Insert / btnupdate_Click)

    public static class Line {
        public int id;
        public int deliveryTypeId;
        public int orderId;
        public int saleOrderDetailId;
        public int supCustId;
        public int wareHouseId;
        public int itemId;
        public String itemDescription;
        public int packUomId;
        public String remarks;
        public int jobLotId;
        public double itemQty;
        public int machineAssetId;
        public int wareHouseToId;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public int deliveryTypeId;
        public int customerId;
        public String vehicleType;
        public String vehicleNo;
        public int requestedById;
        public int approvedById;
        public String remarks;
        public String accountRemarks;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        formValidation(r);

        Map<String, Object> old = null;
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            old = header(r.id);
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(d.get("Id")));
        }

        double doQty = 0.0;
        for (Line l : lines) doQty += l.itemQty;

        // detail validation (Insert, per row, in the desktop's order)
        int rowNo = 0;
        for (Line l : lines) {
            rowNo++;
            if (r.deliveryTypeId != l.deliveryTypeId)
                throw new IllegalArgumentException("DeliveryType in Header Does not Match With Delivery type in Detail  Field Required in Row# " + rowNo);
            if (l.supCustId <= 0) throw new IllegalArgumentException("Customer Field Required in Row# " + rowNo);
            if (l.deliveryTypeId == 1) {
                if (l.orderId <= 0) throw new IllegalArgumentException("OrderNo Field Required in Row# " + rowNo);
                if (l.saleOrderDetailId <= 0) throw new IllegalArgumentException("OrderDetailId Field Required in Row#" + rowNo);
            }
            if (l.wareHouseId <= 0) throw new IllegalArgumentException("WareHouseName Field Required in Row#" + rowNo);
            if (r.deliveryTypeId == 5 && l.wareHouseToId == 0) throw new IllegalArgumentException("WareHouseToId Field required");
            if (l.itemId <= 0) throw new IllegalArgumentException("Item Name Field Required in Row#" + rowNo);
            if (l.jobLotId <= 0) throw new IllegalArgumentException("JobLoat Field Required in Row#" + rowNo);
            if (l.packUomId <= 0) throw new IllegalArgumentException("PackUOM Field Required in Row#" + rowNo);
            if (!(l.itemQty > 0.0)) throw new IllegalArgumentException("ItemQty Field Required in Row#" + rowNo);
            if (l.deliveryTypeId == 3 && !(l.machineAssetId > 0)) throw new IllegalArgumentException("Asset Field Required in Row#" + rowNo);
            if (l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this delivery order");
        }
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this delivery order");

        Timestamp now = now();
        String procHead = r.id > 0 ? "Sp_InvDeliveryOrder_Update" : "Sp_InvDeliveryOrder_Insert";
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(r.docNo));
        h.put("DocDate", parseDate(r.docDate));
        h.put("DoTotalQty", new BigDecimal(doQty, new MathContext(15, RoundingMode.HALF_EVEN)));
        h.put("LoadingInstructions", text(r.remarks));
        h.put("IsApproved", old != null && toBool(old.get("IsApproved")));
        h.put("EntryDate", now);                               // the desktop leaves EntryDate/ModifyDate unset (0001-01-01); the web stamps the save time
        h.put("EntryUser", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUser", r.id > 0 ? u.getId() : 0);
        h.put("ApprovedUser", 0);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", 0);
        h.put("GrossWeight", doQty);
        h.put("NetWeight", doQty);
        h.put("DeliveryOrderType", "Local");
        h.put("EximInvoiceId", 0);
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("VehicleType", text(r.vehicleType));
        h.put("FinancialYearId", sup.fy());
        h.put("ActionId", 0);
        h.put("ToBranchId", 0);
        h.put("FromBranchId", 0);
        h.put("PackingWeight", 0.0);
        h.put("OtherWeight", 0.0);
        h.put("LoadingPortId", 0);
        h.put("DepartmentFromId", 0);
        h.put("DepartmentToId", 0);
        h.put("RequestedByLookUpId", r.requestedById);
        h.put("ApprovedByLookUpId", r.approvedById);
        h.put("AccountRemarks", text(r.accountRemarks));
        h.put("ScreenName", SCREEN);
        h.put("SaleTypeId", 0);
        h.put("TransporterId", 0);
        h.put("IsStockReserved", false);
        SaleEngrAttachments.Change ch = r.attachments;
        if (ch != null && ch.files != null && !ch.files.isEmpty()) {
            StringJoiner names = new StringJoiner(",");
            for (var f : ch.files) names.add(f.name);
            h.put("AttachmentsValues", names.toString());
            h.put("CustomAttachmentsValues", names.toString());
        }
        int num = sup.setProcMap(procHead, h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The delivery order could not be saved.");

        for (Line l : removed) sup.setProcMap("Sp_InvDeliveryOrderDetail_Insert", detail(id, l, 3, true));
        for (Line l : lines) {
            int action = (r.id != 0 ? l.id : 0) <= 0 ? 1 : 2;
            Line x = l;
            if (r.id == 0) { x = copy(l); x.id = 0; }
            sup.setProcMap("Sp_InvDeliveryOrderDetail_Insert", detail(id, x, action, false));
        }
        attachments.apply(SCREEN, DOC_TYPE, id, 0, ch);
        sup.setProcMap("[DAW].[USp_DocumentApprovalDetail_Insert]", row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Id", id, "LimitAmount", BigDecimal.ZERO));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(r.docNo));
        return out;
    }

    private static Line copy(Line l) {
        Line c = new Line();
        c.id = l.id; c.deliveryTypeId = l.deliveryTypeId; c.orderId = l.orderId; c.saleOrderDetailId = l.saleOrderDetailId; c.supCustId = l.supCustId;
        c.wareHouseId = l.wareHouseId; c.itemId = l.itemId; c.itemDescription = l.itemDescription; c.packUomId = l.packUomId; c.remarks = l.remarks;
        c.jobLotId = l.jobLotId; c.itemQty = l.itemQty; c.machineAssetId = l.machineAssetId; c.wareHouseToId = l.wareHouseToId;
        return c;
    }

    /** GenericProvider.SetProc(InvDeliveryOrderdetail): every non-virtual property, null (string) properties are not sent. */
    private Map<String, Object> detail(int headId, Line l, int action, boolean removedRow) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("DoQty", "DoWeight", "LoadingQty", "LoadingWeight", "PackingWeight", "TotalPackingWeight", "GrossWeight", "StockWeight",
                "OtherWeight", "InnerQty", "InnerUomId", "InnerEbUnit", "InnerEbTotal", "AccessWtSet", "OuterEbTotal")) d.put(k, 0.0);
        d.put("DoQty", l.itemQty); d.put("LoadingQty", l.itemQty); d.put("DoWeight", l.itemQty); d.put("LoadingWeight", l.itemQty);
        for (String k : List.of("InvPackingTypeId", "CastingTypeId", "ItemVariantId", "InvoiceDetailId", "ToJobLotId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo",
                "RefDocSubIdNo", "CropYearId", "ExImInvoiceId", "BagTypeId", "ContainerId", "DeliveryScheduleId", "DeliveryScheduleDetailId",
                "ThirdPartyAnalysisSubId", "ThirdPartyAnalysisId")) d.put(k, 0);
        d.put("Id", l.id);
        d.put("InvDeliveryOrderId", headId);
        d.put("ItemId", l.itemId);
        d.put("PackUomId", l.packUomId);
        d.put("SaleOrderId", l.orderId);
        d.put("SaleOrderDetailId", l.saleOrderDetailId);
        d.put("SupplierCustomerId", l.supCustId);
        d.put("WarehouseId", l.wareHouseId);
        d.put("WareHouseToId", removedRow ? 0 : l.wareHouseToId);
        d.put("JobLotId", l.jobLotId);
        d.put("ActionTypeId", action);
        d.put("DeliveryTypeId", l.deliveryTypeId);
        d.put("AssetId", l.machineAssetId);
        d.put("LoadingRemarks", text(l.remarks));
        if (!removedRow) d.put("ItemDiscription", text(l.itemDescription));
        d.put("IsAssetItem", false);
        return d;
    }

    /** FormValidation(): the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        if (r.customerId <= 0) throw new Warning("Customer Field Required");
        boolean veh = false;
        for (Map<String, Object> v : sup.rows("Sp_VehicleType_GetAllMethod")) if (text(r.vehicleType).equals(str(v.get("VehicleDescription")).trim()) && !text(r.vehicleType).isEmpty()) veh = true;
        if (!veh) throw new Warning("Vehicle Type Field is Required");
        if (str(r.vehicleNo).isEmpty()) throw new Warning("Vehicle No Field is Required");
        if (r.requestedById <= 0) throw new Warning("Requested By Field is Required");
        if (r.approvedById <= 0) throw new Warning("Approved By Field is Required");
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click)

    @Transactional
    public Map<String, Object> delete(int id) {
        if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
        Map<String, Object> h = header(id);
        if (toBool(h.get("IsApproved"))) throw new IllegalArgumentException("Record has been approved");
        sup.setProc("USP_RecoredRemoveByOrgCompDocAndByID", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE, "Id", id, "UserId", sup.userId());
        return row("message", "Record Deleted Successfully");
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    private static LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
