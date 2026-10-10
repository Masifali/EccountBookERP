package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 808 "frmDeliveryOrderEngr" = Architecture.WinApp.Mfg.frmDeliveryOrderEngr (Sale Engr, module 134, document type 1657).
 *
 * Desktop map (Architecture.WinApp.Mfg/frmDeliveryOrderEngr.cs, logic lines before InitializeComponent :2858):
 *   ctor :124, InitializeComponentCustom :338, InitializeComponentMethod :406 (rights, GenerateCode 1657, VehicleType.GetAll,
 *   customers, ReferenceParties.GetAll, GetWarehouses, ItemCategory.Getall, LookUps.GetDataByTypeId(1), history customers),
 *   VehicleTypefill :500, CustomerNameFill :549, BindReferencePartiesCombo :584, GetWarehouses :633, WareHouseFill :651,
 *   ItemCategoryFill :705, CastingTypeFill :737, bindvarientunit :770, bindRateUomAndItemPackUom :811, CmbCategory_Leave :842,
 *   ItemDetailBind :854, FillItemFinishWeight :904, CmbItemName_Leave :920, rdSearchByName_CheckedChanged :935,
 *   FormValidation :958, FormValidationDetail :970, btnplus_Click :1016, grdSettings :1035, grd_ColumnButtonClick :1117,
 *   DeleteDetailrow :1148, btnUpdateDetail_Click :1218, grd_DoubleClick :1258, Insert :1294, btnsave_Click :1422, btnSaveAs_Click :1435,
 *   btnupdate_Click :1448, btnDelete_Click :1464, ReadById :1500, btnRefresh_Click :1542, Reset :1561, ResetDetail :1604,
 *   GetCustomerDataForHistoryDropDown :1675, HistoryCombosFill :1702, gridhistoryfill :1748, HistoryGridSettings :1864,
 *   grdhistory_SelectionChanged :1906, grdhistory_DoubleClick :2034, grdhistory_ColumnButtonClick :2058, CalculateOrderBalanceQtyandWeight :2143,
 *   btnprint_Click :2199, LoadInGridDetail :2211, GetSaleOrderIds :2273, btnLoadSaleOrder_Click :2292, btnLoaderView2_Click :2314,
 *   SaleOrderBind :2363, ItemBindbyOrderId :2410, CmbSupplierCustomer_Leave :2431, CmbOrderNo_Leave :2455, KeyDown :2550.
 * Loader dialog frmLoadSaleOrderForDoEngr.cs: Load :90, PendingSaleOrderLoad :170, GridDetailBind :280, GridSecondViewFill :470,
 *   BranchFill :540, HistoryCombosFill :600, btnLoadOnInvoice_Click_1 :690.
 *
 * Differences from the module-83 DeliveryOrder_Engr (screen 538): document type 1657, Mfg detail row (Reference Party, Modal Description = item
 * variant, Casting Type, Specification / Remarks), delivery type is always "Local" (no delivery-type, requested/approved-by, job lot, asset, transfer
 * warehouse), orders come from sale-order document type 1656, quantity comes from the order balance and is saved as qty and weight alike.
 */
@Service
public class SaleDeliveryOrderMfgEngrService {
    public static final String SCREEN = "frmDeliveryOrderEngr";
    public static final int DOC_TYPE = 1657;
    public static final int ORDER_DOC_TYPE = 1656;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public SaleDeliveryOrderMfgEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (InitializeComponentMethod)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                    // GenerateCode -> DeliveryOrderGenerateCode(1657)
        m.put("vehicleTypes", vehicleTypes());                        // BindVehicleTypeCombo
        m.put("customers", customers());                              // BindCustomerCombo
        m.put("refParties", refParties());                            // BindReferencePartiesCombo
        m.put("warehouses", warehouses());                            // GetWarehouses
        m.put("categories", categories());                            // GetItemCategory
        m.put("castingTypes", castingTypes());                        // LookUps.GetDataByTypeId(1)
        m.put("historyCustomers", historyCustomers());                // GetCustomerDataForHistoryDropDown
        int days = sup.configInt("DefaultDaysToLessFromHistoryFromDate");
        m.put("historyDays", days > 0 ? days : 3);                    // FromDateHistory = Now - days (3 when the configuration is 0)
        return m;
    }

    /** Refresh (btnRefresh_Click) re-reads these lists only. */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("vehicleTypes", vehicleTypes());
        m.put("customers", customers());
        m.put("refParties", refParties());
        m.put("warehouses", warehouses());
        m.put("categories", categories());
        m.put("castingTypes", castingTypes());
        return m;
    }

    /** CommonServices.DeliveryOrderGenerateCode(1657) -> InvDeliveryOrder.GenerateCode (Sp_InvDeliveryOrder_GetAllMethod 'GenerateCode'). */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** VehicleType.GetAll: the desktop builds the Activity parameter but never passes it. */
    public List<Map<String, Object>> vehicleTypes() { return sup.rows("Sp_VehicleType_GetAllMethod"); }

    /** BindCustomerCombo: GetERPFeatureById(4) ? GetVendorsAndCustomers(2) : SupplierCustomerGetforComboServiceBind. */
    public List<Map<String, Object>> customers() {
        if (sup.erpFeature(4))
            return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company(), "PartyTypeId", 2);
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** ReferenceParties.GetAll -> Sp_ReferenceParties_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> refParties() {
        return sup.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll");
    }

    /** GetWarehouses: WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId (BranchesId only when the branch feature (11) is on). */
    public List<Map<String, Object>> warehouses() {
        int branch = sup.erpFeature(11) ? sup.branch() : 0;
        return sup.rows("USP_GetWarehousesAllocatedToBranch", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchId", idOrNull(branch));
    }

    /** ItemCategory.Getall -> Sp_ItemCategory_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> categories() {
        return sup.rows("Sp_ItemCategory_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId");
    }

    /** Mfg.LookUps.GetDataByTypeId(1) -> [Mfg].[USP_LookUps_GetAllMethod] @Id = 1, 'GetDataByTypeId'. */
    public List<Map<String, Object>> castingTypes() {
        return sup.rows("[Mfg].[USP_LookUps_GetAllMethod]", "Id", 1, "Activity", "GetDataByTypeId");
    }

    /** GetCustomerDataForHistoryDropDown: InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (Tag is never set so no DeliveryOrderType). */
    public List<Map<String, Object>> historyCustomers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromDeliveryOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "FinancialYearId", idOrNull(sup.fy()), "BranchesIds", String.valueOf(sup.branch()), "Activity", "Customer"))
            out.add(row("Id", r.get("Id"), "Customer", r.get("ReferenceName")));            // HistoryCombosFill
        return out;
    }

    // ------------------------------------------------------------------ detail entry lookups

    /** SaleOrderBind: SaleOrder.SaleOrderIdandNoGetForSupplierCustomerId (DocumentTypeId 1656, RecId = the open record). */
    public List<Map<String, Object>> orders(int customerId, int recId) {
        return sup.rows("Sp_SaleOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "OrderSupCustId", customerId,
                "DocumentTypeId", ORDER_DOC_TYPE, "RecId", idOrNull(recId), "Activity", "SaleOrderIdandNoGetForSupplierCustomerId");
    }

    /** ItemBindbyOrderId: SaleOrder.SaleOrder_GetDatabyOrderId -> dtitem (Id, ItemName, ItemCode, ItemCategoryId, FinishWeight, OrderDetailId). */
    public List<Map<String, Object>> orderItems(int orderId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_SaleOrder_GetAllMethod", "Id", orderId, "Activity", "ReadBySaleOrderHeaderId"))
            out.add(row("Id", r.get("OrderItemId"), "ItemName", r.get("ItemName"), "ItemCode", r.get("ItemCodeNew"), "ItemCategoryId", r.get("ItemCategoryId"),
                    "FinishWeight", r.get("WeightFinishGoods"), "OrderDetailId", r.get("Id")));
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'). */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", r.get("Id"), "UOMCode", r.get("UOMCode"), "Equivalent", r.get("Equivalent"), "QtyEquivalent", r.get("QtyEquivalent"), "BaseRateUom", r.get("BaseRateUom")));
        return out;
    }

    /** bindvarientunit: ItemAttributeVarient.GetAllForCombo -> VariantId / VariantDescription. */
    public List<Map<String, Object>> variants(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_ItemAttributeVarient_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "GetAllForCombo"))
            out.add(row("VariantId", r.get("ItemAttributeVarientId"), "VariantDescription", r.get("VarientDescription")));
        return out;
    }

    /** CalculateOrderBalanceQtyandWeight: InvDeliveryOrder.OrderBalanceWeight ('GetDoBalWeightByOrder'; Id = order, SaleOrderDetailId = the item's order detail). */
    public Map<String, Object> balance(int orderId, int orderDetailId, int itemId) {
        double bal = 0.0;
        List<Map<String, Object>> r = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", orderId,
                "SaleOrderDetailId", orderDetailId, "ItemId", itemId, "Activity", "GetDoBalWeightByOrder");
        if (!r.isEmpty()) bal = toDouble(r.get(0).get("BalWeight"));
        return row("balWeight", bal);
    }

    // ------------------------------------------------------------------ History (gridhistoryfill)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        LocalDate from = parseDate(fromDate), to = parseDate(toDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE);
        p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", idOrNull(sup.fy()));
        if (!all) p.put("EntryUser", sup.userId());
        p.put("DeliveryOrderType", "Local");                       // reports.Activity = "Local"
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

    /** InvDeliveryOrder DAL.GetData: document types 1606 / 1657 read the detail with 'ReadByIdDetailIdForEngr'. */
    private List<Map<String, Object>> details(int id) {
        return sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "Id", id, "Activity", "ReadByIdDetailIdForEngr");
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Loader dialog (frmLoadSaleOrderForDoEngr)

    /** Load :90 / BranchFill :540: branch list, the active year start, the BranchFeature / SaleOrderBranchWise switches. */
    public Map<String, Object> loaderInit() {
        UserAccount u = sup.user();
        boolean feature = sup.erpFeature(11);
        boolean implemented = sup.configBool("SaleOrderBranchWise");
        List<Map<String, Object>> branches = new ArrayList<>();
        if (feature && implemented) {
            String name = "";
            for (Map<String, Object> r : sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"))
                if (toInt(r.get("Id")) == sup.branch()) { name = str(r.get("BranchName")); break; }
            branches.add(row("Id", sup.branch(), "BranchName", name));
        } else {
            for (Map<String, Object> r : sup.rows("USP_GetBranchsAllocatedToUserFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "UserId", u.getId(), "DocumentTypeId", ORDER_DOC_TYPE))
                branches.add(row("Id", r.get("BranchId"), "BranchName", r.get("BranchName")));
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", branches);
        m.put("branchFeature", feature);
        m.put("branchImplemented", implemented);
        m.put("fromDate", fyStart());
        return m;
    }

    /** HistoryCombosFill (loader): SaleOrder.GetDataForDropDownFromSaleOrder, DocumentTypeIds 1656, BranchesIds when the branch feature is on. */
    public Map<String, Object> loaderCombos(String branchIds) {
        UserAccount u = sup.user();
        boolean feature = sup.erpFeature(11);
        boolean implemented = sup.configBool("SaleOrderBranchWise");
        String br = feature ? (implemented ? String.valueOf(sup.branch()) : text(branchIds)) : null;
        List<Map<String, Object>> cust = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(ORDER_DOC_TYPE),
                "BranchesIds", br == null || br.isEmpty() ? null : br)) {
            String a = str(r.get("Activity"));
            if ("Customer".equals(a)) cust.add(row("Id", r.get("Id"), "Customer", r.get("ReferenceName")));
            else if ("Item".equals(a)) items.add(row("Id", r.get("Id"), "ItemName", r.get("ReferenceName")));
        }
        return row("customers", cust, "items", items);
    }

    /** PendingSaleOrderLoad: USP_GetPendingSaleOrderForDoAndGdn_Engr (FinancialYearId, customer, item, dates, BranchesIds). */
    public List<Map<String, Object>> loaderMain(String fromDate, String toDate, int customerId, int itemId, String branchIds) {
        if (text(branchIds).isEmpty()) throw new IllegalArgumentException("Select branch first");
        return sup.rows("USP_GetPendingSaleOrderForDoAndGdn_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(), "FinancialYearId", idOrNull(sup.fy()),
                "SupplierCustomerId", idOrNull(customerId), "ItemId", idOrNull(itemId), "DocDateFrom", parseDate(fromDate), "DocDateTo", parseDate(toDate),
                "BranchesIds", branchIds);
    }

    /** GridDetailBind: USP_GetPendingSaleOrderDetailForDoAndGdn_Engr (Ids = checked orders, OrderDetailIds = details already in the grid are excluded). */
    public List<Map<String, Object>> loaderDetail(String ids, String orderDetailIds) {
        if (text(ids).isEmpty()) return List.of();
        return sup.rows("USP_GetPendingSaleOrderDetailForDoAndGdn_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Ids", ids, "OrderDetailIds", text(orderDetailIds).isEmpty() ? null : orderDetailIds);
    }

    /** GridSecondViewFill: the same detail procedure with customer, item, dates and branches (no Ids, no exclusions, no financial year). */
    public List<Map<String, Object>> loaderSecond(String fromDate, String toDate, int customerId, int itemId, String branchIds) {
        if (text(branchIds).isEmpty()) throw new IllegalArgumentException("Select branch first");
        return sup.rows("USP_GetPendingSaleOrderDetailForDoAndGdn_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "SupplierCustomerId", idOrNull(customerId), "ItemId", idOrNull(itemId), "FromDate", parseDate(fromDate), "ToDate", parseDate(toDate), "BranchesIds", branchIds);
    }

    /** LoadInGridDetail: SaleOrder.LoadSaleOrderForDOInDetail_Engr(org, company, "id1,id2,..."). */
    public List<Map<String, Object>> loaderRows(String orderDetailIds) {
        return sup.rows("USP_LoadSaleOrderForDOInDetail_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(), "OrderDetailIds", orderDetailIds);
    }

    /** Start of the active financial year (FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    // ------------------------------------------------------------------ Save (Insert :1294)

    public static class Line {
        public int id;
        public int supplierCustomerId;
        public int refPartyId;
        public int orderId;
        public int orderDetailId;
        public int warehouseId;
        public int itemId;
        public int packUomId;
        public int variantId;
        public int castingTypeId;
        public String remarks;
        public double loadQty;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public String vehicleType;
        public String vehicleNo;
        public String remarks;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new IllegalArgumentException("DocNo Field is Required");     // FormValidation :958

        Map<String, Object> old = null;
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            old = header(r.id);
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(d.get("Id")));
        }
        // per-row checks in the desktop's order (raised while the detail list is built)
        int rowNo = 0;
        for (Line l : lines) {
            rowNo++;
            if (l.supplierCustomerId == 0) throw new IllegalArgumentException("Customer Field is Required in detail Row No : " + rowNo);
            if (l.warehouseId == 0) throw new IllegalArgumentException("Warehouse Field is Required in detail Row No : " + rowNo);
            if (l.orderId == 0) throw new IllegalArgumentException("OrderNo Field is Required in detail Row No : " + rowNo);
            if (l.itemId == 0) throw new IllegalArgumentException("Item Field is Required in detail Row No : " + rowNo);
            if (l.packUomId == 0) throw new IllegalArgumentException("Uom Field is Required in detail Row No : " + rowNo);
            if (r.id > 0 && l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this delivery order");
        }
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        for (Line l : removed)
            if (r.id <= 0 || l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this delivery order");

        Timestamp now = now();
        boolean update = r.id > 0;
        String procHead = update ? "Sp_InvDeliveryOrder_Update" : "Sp_InvDeliveryOrder_Insert";
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(r.docNo));
        h.put("DocDate", parseDate(r.docDate));
        h.put("DoTotalQty", BigDecimal.ZERO);
        h.put("LoadingInstructions", r.remarks == null ? "" : r.remarks);
        h.put("IsApproved", old != null && toBool(old.get("IsApproved")));
        h.put("EntryDate", now);                                   // InvDeliveryOrder.Save stamps EntryDate / ModifyDate with DateTime.Now
        h.put("EntryUser", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUser", u.getId());
        h.put("ApprovedDate", now);
        h.put("ApprovedUser", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", 0);
        h.put("GrossWeight", 0.0);
        h.put("NetWeight", 0.0);
        h.put("DeliveryOrderType", "Local");
        h.put("EximInvoiceId", 0);
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("VehicleType", text(r.vehicleType));
        h.put("FinancialYearId", sup.fy());
        h.put("ActionId", update ? 2 : 1);                         // InvDeliveryOrder.Save: ActionId 1 insert / 2 update
        h.put("ToBranchId", 0);
        h.put("FromBranchId", 0);
        h.put("PackingWeight", 0.0);
        h.put("OtherWeight", 0.0);
        h.put("LoadingPortId", 0);
        h.put("DepartmentFromId", 0);
        h.put("DepartmentToId", 0);
        h.put("RequestedByLookUpId", 0);
        h.put("ApprovedByLookUpId", 0);
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
        } else {
            h.put("AttachmentsValues", old == null || old.get("AttachmentsValues") == null ? "" : str(old.get("AttachmentsValues")));
            h.put("CustomAttachmentsValues", old == null || old.get("CustomAttachmentsValues") == null ? "" : str(old.get("CustomAttachmentsValues")));
        }
        int num = sup.setProcMap(procHead, h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The delivery order could not be saved.");

        for (Line l : removed) sup.setProcMap("Sp_InvDeliveryOrderDetail_Insert", detail(id, l, 3));      // lstRemoveRecord first, ActionTypeId 3
        for (Line l : lines) {
            Line x = l;
            if (!update) { x = copy(l); x.id = 0; }
            int action = x.id <= 0 ? 1 : 2;
            sup.setProcMap("Sp_InvDeliveryOrderDetail_Insert", detail(id, x, action));
        }
        attachments.apply(SCREEN, DOC_TYPE, id, 0, ch);
        sup.setProcMap("[DAW].[USp_DocumentApprovalDetail_Insert]", row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Id", id, "LimitAmount", BigDecimal.ZERO));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", update);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(r.docNo));
        return out;
    }

    private static Line copy(Line l) {
        Line c = new Line();
        c.id = l.id; c.supplierCustomerId = l.supplierCustomerId; c.refPartyId = l.refPartyId; c.orderId = l.orderId; c.orderDetailId = l.orderDetailId;
        c.warehouseId = l.warehouseId; c.itemId = l.itemId; c.packUomId = l.packUomId; c.variantId = l.variantId; c.castingTypeId = l.castingTypeId;
        c.remarks = l.remarks; c.loadQty = l.loadQty;
        return c;
    }

    /** GenericProvider.SetProc(InvDeliveryOrderdetail): DoQty = DoWeight = LoadingQty = LoadingWeight = GrossWeight = the grid's LoadQty (desktop :1342). */
    private Map<String, Object> detail(int headId, Line l, int action) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("PackingWeight", "TotalPackingWeight", "StockWeight", "OtherWeight", "InnerQty", "InnerUomId", "InnerEbUnit", "InnerEbTotal",
                "AccessWtSet", "OuterEbTotal")) d.put(k, 0.0);
        d.put("DoQty", l.loadQty); d.put("DoWeight", l.loadQty); d.put("LoadingQty", l.loadQty); d.put("LoadingWeight", l.loadQty); d.put("GrossWeight", l.loadQty);
        for (String k : List.of("InvPackingTypeId", "InvoiceDetailId", "ToJobLotId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "CropYearId", "ExImInvoiceId",
                "BagTypeId", "ContainerId", "DeliveryScheduleId", "DeliveryScheduleDetailId", "ThirdPartyAnalysisSubId", "ThirdPartyAnalysisId", "JobLotId",
                "DeliveryTypeId", "AssetId", "WareHouseToId")) d.put(k, 0);
        d.put("Id", l.id);
        d.put("InvDeliveryOrderId", headId);
        d.put("SupplierCustomerId", l.supplierCustomerId);
        d.put("SaleOrderId", l.orderId);
        d.put("SaleOrderDetailId", l.orderDetailId);
        d.put("RefPartyId", l.refPartyId);
        d.put("WarehouseId", l.warehouseId);
        d.put("ItemId", l.itemId);
        d.put("PackUomId", l.packUomId);
        d.put("ItemVariantId", l.variantId);
        d.put("CastingTypeId", l.castingTypeId);
        d.put("LoadingRemarks", l.remarks == null ? "" : l.remarks);
        d.put("ActionTypeId", action);
        d.put("IsAssetItem", false);
        return d;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click :1464)

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
