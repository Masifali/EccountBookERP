package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 539 "GoodsDispatchNotes_Engr" = Architecture.WinApp.SaleTrading.GoodsDispatchNotes_Engr (frmGSIssuance, Sale Engr, module 83, document type 1612).
 *
 * Desktop map (GoodsDispatchNotes_Engr.cs, logic before InitializeComponent, 3020 lines):
 *   frmGSIssuance_Load :208, GenerateDocNo :330, TypeFill :344, EmployeeFill :360, CustomerFilll :381, TransporterFill :411, FixedAssest :459,
 *   HistoryCombosBind :488, OrderNoBind :520, GetOutStandingParties :552, GetOutStandingOrderAndParties :583, ItemDetailFillWithoutOrderId :600,
 *   ItemBindbyOrderId :640, ItemUomFill :686, JobLotFill :719, WarehouseFill :738, DeliveryTerms :769, defaultConfiquration :787,
 *   FormValidation :838, FormValidationDetail :880, Add_Click :946, DetailGridSettings :973, grdDetail_DoubleClick :1060, btnUpdateDetail_Click :1090,
 *   grdDetail_ColumnButtonClick :1137, OtherItemsBind :1170, grdInvExpSettings :1200, GetPendingGatePass :1260, grdGp_ColumnButtonClick :1330,
 *   formReset :1432, CmbType_Leave :1498, WareHouseToShowHide :1555, Insert :1625, ReadById :1975, btnDelete_Click :2092, Print_Click :2123,
 *   HistoryFill :2200, gridsetting :2272, DataGridHistory_SelectionChanged :2370, LoadInGridDetail :2629, BtnDeliveryOrder_Click :2669,
 *   frmGSIssuance_KeyDown :2705, grdDetail_KeyDown :2816, GridExpense_KeyDown :2955.
 * Dialog frmLoadDeliveryOrderOnGdn_Engr (pending Delivery Orders of type Returnable / Returned) is served by the /loader endpoints.
 *
 * BLL/DAL chain (Architecture.BLL.Inventory.InvGdn.Save -> DAL.Inventory.InvGdn.SetData, one transaction):
 *   Sp_InvGdn_Insert / Sp_InvGdn_Update  (EntryUser=0 on update, ModifyUser=0 on insert, ActionId 2 / 1)
 *   Sp_InvGdnDetail_Insert per row (LineId = 1..n) ; Sp_InvGdnExpense_Insert per expense row
 *   DMS attachments ; Sp_InventoryTransactions_GetALLMethod ; (document type 1612 is not "valid" for FIFO costing and carries no stock reference,
 *   so the stock evaluation branch is Sp_InventoryStockEvalautionDetail_Update) ; USP_InventoryValidation per row ;
 *   usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding ; (no order auto-complete and no reserved posting for 1612).
 */
@Service
public class SaleGdnEngrService {
    public static final String SCREEN = "GoodsDispatchNotes_Engr";
    public static final int DOC_TYPE = 1612;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final Map<String, List<Object[]>> paramCache = new java.util.concurrent.ConcurrentHashMap<>();

    public SaleGdnEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (frmGSIssuance_Load)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        boolean subsidiary = sup.erpFeature(4);                                          // SubsidiaryAccountAllownOnVouchers
        m.put("rights", sup.rights(SCREEN));
        m.put("subsidiary", subsidiary);
        m.put("nextNo", nextNo());                                                       // GenerateDocNo
        m.put("deliveryTypes", deliveryTypes());                                         // TypeFill
        m.put("customers", customers());                                                 // CustomerFilll
        m.put("transporters", transporters());                                           // TransporterFill
        m.put("lookups", lookups());                                                     // EmployeeFill
        m.put("items", itemsWithoutOrder());                                             // ItemDetailFillWithoutOrderId
        m.put("warehouses", warehouses());                                               // WarehouseFill
        m.put("jobLots", jobLots());                                                     // JobLotFill
        m.put("assets", assets());                                                       // FixedAssest
        m.put("otherItems", otherItems());                                               // OtherItemsBind
        m.put("outstanding", outstanding(0));                                            // GetOutStandingOrderAndParties
        m.put("gatePasses", pendingGatePasses());                                        // GetPendingGatePass
        m.put("history", historyCombos());                                               // HistoryCombosBind
        m.put("defaultJobLot", sup.configInt("Job/Lot"));                                // defaultConfiquration
        m.put("defaultWarehouse", sup.configInt("Warehouse"));
        return m;
    }

    /** btnRefresh_Click: WarehouseFill, FixedAssest, EmployeeFill, TransporterFill, CustomerFilll. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("warehouses", warehouses());
        m.put("assets", assets());
        m.put("lookups", lookups());
        m.put("transporters", transporters());
        m.put("customers", customers());
        return m;
    }

    /** formReset tail: GenerateDocNo + GetPendingGatePass. */
    public Map<String, Object> resetData() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("nextNo", nextNo());
        m.put("gatePasses", pendingGatePasses());
        return m;
    }

    public int nextNo() {
        // CommonServices.InvGdnGenerateCode(1612) -> InvGdn.GenerateInvGdnCode
        List<Map<String, Object>> r = sup.rows("Sp_InvGdn_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerateInvGdnCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> deliveryTypes() { return sup.rows("SpStaticColumnNames", "Activity", "DeliveryType"); }

    public List<Map<String, Object>> customers() {
        if (sup.erpFeature(4)) return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company());
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** TransporterFill: vendors of party type 1 under sub-ledger accounting, otherwise the COA allocation list without account types 2, 15 and 11. */
    public List<Map<String, Object>> transporters() {
        if (sup.erpFeature(4))
            return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company(), "PartyTypeId", 1);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "UserId", idOrNull(sup.userId()), "Activity", "COAAllocationSearch")) {
            int t = toInt(r.get("AccountTypeId"));
            if (t != 2 && t != 15 && t != 11) out.add(row("Id", r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        }
        return out;
    }

    public List<Map<String, Object>> lookups() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvLookup_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "InvLookupTypeId", 13, "Activity", "ReadByInvlookTypeId"))
            out.add(row("Id", r.get("Id"), "LookupName", r.get("LookupName")));
        return out;
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

    /** InventoryItemsOther.GetAll -> Sp_InventoryItemsOther_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InventoryItemsOther_GetAllMethod", "Activity", "ReadAll", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", r.get("Id"), "OtherItemName", r.get("OtherItemName")));
        return out;
    }

    /** SaleOrder.GetOutstandOrdersAndPartiesForIssuanceGdn (@RecId only when not zero). */
    public List<Map<String, Object>> outstanding(int recId) {
        return sup.rows("USP_GetOutstandOrdersAndPartiesForIssuanceGdn", "OrganizationId", sup.org(), "CompanyId", sup.company(), "RecId", idOrNull(recId));
    }

    /** GatePassOutward.GetOutstandingGatePassForGdn_Engr (grdGp). */
    public List<Map<String, Object>> pendingGatePasses() {
        return sup.rows("USP_GetOutstandingGatePassForGdn_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "FinancialYearId", sup.fy(), "BranchesId", sup.branch());
    }

    /** CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", r.get("Id"), "UOMCode", r.get("UOMCode"), "Equivalent", r.get("Equivalent")));
        return out;
    }

    /** HistoryCombosBind: InvGdn.GetDataForDropDownFromGdn (org, company, DocumentTypeIds 1612) split by Activity. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), req = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromGdn", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", "1612")) {
            String a = str(r.get("Activity"));
            if ("Supplier".equals(a)) cust.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
            else if ("RequestedBy".equals(a)) req.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
        }
        return row("customers", cust, "requestedBy", req);
    }

    // ------------------------------------------------------------------ gate pass "Load" (grdGp_ColumnButtonClick)

    /** InvDeliveryOrder.DeliveryOrderDataForGdn_Engr -> USP_DeliveryOrderDataForGdn (every parameter is sent, zeros included). */
    public List<Map<String, Object>> deliveryOrderData(int customerId, int deliveryOrderId, int deliveryTypeId) {
        return sup.rows("USP_DeliveryOrderDataForGdn", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", 1606,
                "SupplierCustomerId", customerId, "Id", deliveryOrderId, "DeliveryTypeId", deliveryTypeId);
    }

    // ------------------------------------------------------------------ Delivery Order loader (frmLoadDeliveryOrderOnGdn_Engr)

    public Map<String, Object> loaderCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromDeliveryOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "FinancialYearId", idOrNull(sup.fy()), "DocumentTypeIds", "1606", "BranchesIds", String.valueOf(sup.branch()))) {
            String a = str(r.get("Activity"));
            if ("Customer".equals(a)) cust.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
            if ("Item".equals(a)) items.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
        }
        return row("customers", cust, "items", items);
    }

    /**
     * InvDeliveryOrder.DeliveryOrder_PendingDataForGdn_Engr. The desktop sends only the customer, the delivery type and the item
     * (the dialog's date and document-number boxes are not part of the call), so the same is done here.
     */
    public List<Map<String, Object>> loaderRows(int customerId, int deliveryTypeId, int itemId) {
        return sup.rows("USP_DeliveryOrder_PendingDataForGdn_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(),
                "FinancialYearId", sup.fy(), "DocumentTypeId", 1606, "SupplierCustomerId", idOrNull(customerId), "DeliveryTypeId", idOrNull(deliveryTypeId),
                "ItemId", idOrNull(itemId));
    }

    /** Start of the active financial year (btnReset_Click: FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    // ------------------------------------------------------------------ History (HistoryFill / DataGridHistory_SelectionChanged)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId, int requestedById, String referred) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate from = parseDate(fromDate), to = parseDate(toDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("FinancialYearId", sup.fy()); p.put("BranchesId", sup.branch());
        p.put("DocumentTypeId", DOC_TYPE);
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
        if (fromNo != 0) p.put("FromDocNo", (double) fromNo);
        if (toNo != 0) p.put("ToDocNo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (requestedById != 0) p.put("RequestedById", requestedById);
        if ("referred".equals(referred)) p.put("IsRefered", 1);
        else if ("notReferred".equals(referred)) p.put("IsRefered", 2);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", sup.userId());
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "USP_IssuanceGDNFormHistory", p);
    }

    // ------------------------------------------------------------------ ReadById (InvGdn.GetByID)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows("Sp_InvGdn_GetAllMethod", "Id", id, "Activity", "GetById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Goods dispatch note not found in this company");
        return r.get(0);
    }

    public List<Map<String, Object>> details(int id) {
        return sup.rows("Sp_InvGdn_GetAllMethod", "InvGdnMainId", id, "Activity", "GetGDNDetailByGdnId");
    }

    private List<Map<String, Object>> expenses(int id) {
        return sup.rows("Sp_InvGdn_GetAllMethod", "Id", id, "Activity", "GetInvGdnExpensesByHeaderId");
    }

    /** History selection: the detail of the highlighted row (GetByID of the row). */
    public List<Map<String, Object>> detailOf(int id) { header(id); return details(id); }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("expenses", expenses(id));
        out.put("outstanding", outstanding(id));                  // ReadById: GetOutStandingOrderAndParties(RecId)
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert)

    public static class Line {
        public int deliveryOrderId;
        public int deliveryOrderDetailId;
        public int deliveryOrderNo;
        public int orderId;
        public int orderDetailId;
        public int wareHouseId;
        public int itemId;
        public String itemDescription;
        public int packUomId;
        public String remarks;
        public int jobLotId;
        public double itemQty;
        public int assetId;
        public int wareHouseToId;
    }

    public static class Expense {
        public int itemId;
        public double qty;
        public String remarks;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public int deliveryTermId;
        public int deliveryTypeId;
        public int customerId;
        public int requestedById;
        public String manualNo;
        public int transporterId;
        public String freight;
        public String returnableDate;
        public String remarks;
        public int gatePassId;
        public String gpNo;
        public String vehicleNo;
        public String biltyNo;
        public List<Line> lines = new ArrayList<>();
        public List<Expense> expenses = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        boolean subsidiary = sup.erpFeature(4);
        formValidation(r);

        Map<String, Object> old = null;
        if (r.id > 0) old = header(r.id);

        // Insert(): transporter account, then the grid
        double freight = toDouble(r.freight);
        int transporterId = 0, transporterSupCustId = 0;
        if (freight > 0.0) {
            if (r.transporterId <= 0) throw new IllegalArgumentException("Transporter Account field required");
            if (!subsidiary) {
                transporterId = r.transporterId;
                transporterSupCustId = 0;
            } else {
                // SelectedRow.Cells[3] of the vendor list (resolved by name) is the TransporterId, the combo value the TransporterSupCustId
                Map<String, Object> t = null;
                for (Map<String, Object> x : transporters()) if (toInt(x.get("Id")) == r.transporterId) t = x;
                if (t == null) throw new IllegalArgumentException("Transporter Account field required");
                transporterId = toInt(t.get("SupplierCustomerId"));
                transporterSupCustId = r.transporterId;
            }
        }
        List<Line> rows = r.lines == null ? List.of() : r.lines;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");

        int type = r.deliveryTypeId;
        List<Line> details = new ArrayList<>();
        int rowNo = 0;
        for (Line l : rows) {
            rowNo++;                                              // r.RowIndex + 1
            if (!((int) Math.rint(l.itemQty) > 0)) continue;      // Conversion.ToInt(ItemQty) > 0 (Convert.ToInt32 rounds to even)
            if (type == 1 && !((double) l.orderId > 0.0)) throw new IllegalArgumentException("OrderId Field Required when DeliveryType Is 'Sale Order' in Row#" + rowNo);
            if (l.wareHouseId == 0) throw new IllegalArgumentException("WareHouse Field required");
            if (type == 5 && l.wareHouseToId == 0) throw new IllegalArgumentException("WareHouseToId Field required");
            if (l.itemId == 0) throw new IllegalArgumentException("ItemName Field Required");
            if (l.jobLotId == 0) throw new IllegalArgumentException("JobLot Field required");
            if (l.itemQty == 0.0) throw new IllegalArgumentException("ItemQty Field Required");
            if (type == 3 && !((double) l.assetId > 0.0)) throw new IllegalArgumentException("Asset Field Required in Row#" + rowNo);
            details.add(l);
        }
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");   // DAL.InvGdn.SetData

        List<Expense> exps = new ArrayList<>();
        for (Expense e : (r.expenses == null ? List.<Expense>of() : r.expenses)) if (e.itemId > 0 && e.qty > 0.0) exps.add(e);

        Timestamp now = now();
        java.sql.Date docDate = java.sql.Date.valueOf(parseDate(r.docDate) == null ? LocalDate.now() : parseDate(r.docDate));
        String term = r.deliveryTermId == 1 ? "Load" : r.deliveryTermId == 2 ? "Ponch" : "";
        Map<String, Object> h = new LinkedHashMap<>();
        for (String k : List.of("IsApproved", "AddWages", "ShortWeightDeductionApply", "IsStockReserved")) h.put(k, false);
        h.put("DocDate", docDate);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        LocalDate rd = parseDate(r.returnableDate);
        h.put("ReturnableDate", java.sql.Date.valueOf(rd == null ? LocalDate.now() : rd));
        h.put("CarriageAmount", freight);
        for (String k : List.of("FactoryWeight", "OtherCharges", "PartyWeight")) h.put(k, 0.0);
        h.put("BranchesId", u.getBranchesId());
        h.put("CompanyId", u.getCompanyId());
        h.put("DocNo", toInt(r.docNo));
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("EntryUser", r.id > 0 ? 0 : u.getId());          // BLL.Save: EntryUser = 0 on update
        h.put("GpNo", toInt(r.gpNo));
        h.put("Id", r.id);
        h.put("AutoUpdateId", 0);
        h.put("ModifyUser", r.id > 0 ? u.getId() : 0);         // BLL.Save: ModifyUser = 0 on insert
        h.put("OrganizationId", u.getOrganizationId());
        h.put("OutwardGatePassId", r.gatePassId);
        for (String k : List.of("PostUser", "ProjectsId", "ReferencePartyId", "StockPartyId", "BillCalculateTypeId", "VehicleTypeId", "OrderId", "OrderTypeId", "AdvanceDeliveryOrderId")) h.put(k, 0);
        h.put("RequestedByLookUpId", r.requestedById);
        h.put("SupplierCustomerId", r.customerId);
        h.put("TransporterId", transporterId);
        h.put("TransporterSupCustId", transporterSupCustId);
        h.put("BiltyNo", text(r.biltyNo));
        h.put("ReferenceDocNo", text(r.manualNo));
        h.put("RemarksHeader", r.remarks == null ? "" : r.remarks);
        h.put("ScreenName", SCREEN);
        h.put("VehicleNo", r.vehicleNo == null ? "" : r.vehicleNo);
        h.put("DeliveryTerm", term);
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("FinancialYearId", sup.fy());
        // AttachmentsValues / CustomAttachmentsValues: names of the files that stay attached after this save
        SaleEngrAttachments.Change ch = r.attachments;
        List<String> names = new ArrayList<>();
        if (r.id > 0) {
            Set<Integer> gone = new HashSet<>(ch != null && ch.removeAttachmentIds != null ? ch.removeAttachmentIds : List.of());
            for (Map<String, Object> a : attachments.list(SCREEN, r.id)) if (!gone.contains(toInt(a.get("Id")))) names.add(str(a.get("Attachment")));
        }
        if (ch != null && ch.files != null) for (var f : ch.files) names.add(f.name);
        h.put("AttachmentsValues", String.join(",", names));
        h.put("CustomAttachmentsValues", String.join(",", names));

        int num = sup.setProcMap(r.id > 0 ? "Sp_InvGdn_Update" : "Sp_InvGdn_Insert", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The goods dispatch note could not be saved.");

        int lineId = 0;
        for (Line l : details) {
            lineId++;
            Map<String, Object> d = new LinkedHashMap<>();
            for (String k : List.of("AdLsWeight", "EBWPerUnit", "EBWTotal", "EbUnitStock", "EbTotalStock", "WtCut", "WtCutTotal", "ShortWeight", "ItemRate", "ItemAmount")) d.put(k, 0.0);
            d.put("GrossWeight", l.itemQty); d.put("NetBillWeight", l.itemQty); d.put("StockWeight", l.itemQty); d.put("ItemQty", l.itemQty);
            d.put("IsAssetItem", false);
            for (String k : List.of("DeliveryScheduleId", "ItemVariantId", "CastingTypeId", "ProductionStageId", "PackingTypeId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo",
                    "RefDocSubIdNo", "ItemConditionId", "GrnId", "GrnDetailId", "GrnDocumentTypeId", "CityId", "DepartmentId", "WbTicketId", "GpNo", "RateUomId", "CropYearId",
                    "OrderTypeId", "BrandItemId", "RackId", "SecondaryUomId", "Id")) d.put(k, 0);
            d.put("SecondaryUomQty", java.math.BigDecimal.ZERO);
            d.put("InvGdnId", id);
            d.put("LineId", lineId);
            d.put("ItemId", l.itemId);
            d.put("ItemUomId", l.packUomId);
            d.put("JobLotId", l.jobLotId);
            d.put("SaleOrderId", l.orderId);
            d.put("SaleOrderDetailId", l.orderDetailId);
            d.put("WarehouseId", l.wareHouseId);
            d.put("WareHouseToId", type == 5 ? l.wareHouseToId : 0);
            d.put("DeliveryTypeId", type);
            d.put("AssetId", l.assetId);
            d.put("InvDeliveryOrderId", l.deliveryOrderId);
            d.put("InvDeliveryOrderDetailId", l.deliveryOrderDetailId);
            d.put("ActionTypeId", 0);
            d.put("CommentsDetail", l.remarks == null ? "" : l.remarks);
            d.put("ItemDescription", l.itemDescription == null ? "" : l.itemDescription);
            d.put("GpDate", now);
            sup.setProcMap("Sp_InvGdnDetail_Insert", d);
        }
        for (Expense e : exps) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Qty", e.qty); x.put("Id", 0); x.put("InvGdnId", id); x.put("ItemId", e.itemId); x.put("Remarks", e.remarks == null ? "" : e.remarks);
            sup.setProcMap("Sp_InvGdnExpense_Insert", x);
        }
        attachments.apply(SCREEN, DOC_TYPE, id, r.customerId, ch);

        // InventoryTransactions model through Sp_InventoryTransactions_GetALLMethod
        sup.setProcMap("Sp_InventoryTransactions_GetALLMethod", full("Sp_InventoryTransactions_GetALLMethod",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        // stock evaluation branch of DAL.InvGdn.SetData for 1612 (no stock references on the rows)
        sup.setProcMap("Sp_InventoryStockEvalautionDetail_Update", full("Sp_InventoryStockEvalautionDetail_Update",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        for (Line l : details) {
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("OrganizationId", u.getOrganizationId()); v.put("CompanyId", u.getCompanyId()); v.put("DocumentTypeId", DOC_TYPE); v.put("DocDate", docDate);
            v.put("ItemId", l.itemId); v.put("WarehouseId", l.wareHouseId); v.put("JobLotId", l.jobLotId); v.put("InvPackingTypeId", 0); v.put("PackUomId", l.packUomId);
            v.put("NetWeight", l.itemQty); v.put("RefDocumentTypeId", 0); v.put("RefDocNoId", 0); v.put("RefDocSubIdNo", 0); v.put("ItemConditionId", 0);
            com.mst.repositories.support.DesktopProc.scalar(sup.jdbc(), "USP_InventoryValidation", v);
        }
        com.mst.repositories.support.DesktopProc.scalar(sup.jdbc(), "usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "Id", id));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Record Update SuccessFully" : "Record Save SuccessFully") + toInt(r.docNo));
        return out;
    }

    /** FormValidation(): the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || toInt(no) == 0) throw new Warning("DocNo Field Required");
        if (r.deliveryTermId <= 0) throw new Warning("Delivery Term Field Required");
        if (r.deliveryTypeId <= 0) throw new Warning("Type Field Required");
        if (r.customerId <= 0) throw new Warning("Customer Field Required");
        if (r.requestedById <= 0) throw new Warning("RequestedBy Field Required");
        double f = toDouble(r.freight);
        if (r.transporterId > 0 && !(f > 0.0)) throw new Warning("Freight Amount Field is Required");
        if (f > 0.0 && r.transporterId <= 0) throw new Warning("Transporter Field is Required");
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click)

    @Transactional
    public Map<String, Object> delete(int id) {
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        header(id);
        // InvPurchaseInvoice.RemoveByID -> DAL.AccountandInventoryRemoveById -> Sp_InvoicesVouchersandStocksDelete
        sup.setProc("Sp_InvoicesVouchersandStocksDelete", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", id, "DocumentTypeId", DOC_TYPE, "UserId", sup.userId());
        return row("message", "Delete Record Seccessfully");
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    // ------------------------------------------------------------------ helpers

    /**
     * GenericProvider.SetProc of a model with many properties: every procedure parameter that the caller did not give receives the
     * property default (0 / false / unset string or date is left out).
     */
    private Map<String, Object> full(String proc, Map<String, Object> given) {
        List<Object[]> params = paramCache.computeIfAbsent(proc, p -> {
            List<Object[]> l = new ArrayList<>();
            for (Map<String, Object> r : sup.jdbc().queryForList("SELECT p.name, t.name AS tname FROM sys.parameters p JOIN sys.types t ON p.user_type_id = t.user_type_id "
                    + "WHERE p.object_id = OBJECT_ID(?) ORDER BY p.parameter_id", "dbo." + p))
                l.add(new Object[]{String.valueOf(r.get("name")).replace("@", ""), String.valueOf(r.get("tname")).toLowerCase()});
            return l;
        });
        Map<String, Object> out = new LinkedHashMap<>(given);
        for (Object[] p : params) {
            String n = (String) p[0], t = (String) p[1];
            boolean has = false;
            for (String k : out.keySet()) if (k.equalsIgnoreCase(n)) { has = true; break; }
            if (has) continue;
            switch (t) {
                case "bit" -> out.put(n, false);
                case "tinyint", "smallint", "int", "bigint" -> out.put(n, 0);
                case "float", "real", "decimal", "numeric", "money", "smallmoney" -> out.put(n, 0.0);
                default -> { }
            }
        }
        return out;
    }

    private static LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
