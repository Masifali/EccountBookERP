package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 348 GoodsDispatchNotes_Store = Architecture.WinApp.StoreManagement.GoodsDispatchNotes_Store (frmGSIssuance, Store Management module 10,
 * document type 144). Rights / attachment screen name = the form name (base.Name) "GoodsDispatchNotes_Store".
 *
 * Desktop map (GoodsDispatchNotes_Store.cs, 4899 lines, logic before InitializeComponent :2645):
 *   ctor :300 (DocumentTypeId 144, table/columns :335), InitializeComponentMethod :364, frmGSIssuance_Load :419, HistoryComboDbCall :434, HistoryComboBind :458,
 *   GetConfigurationsFromGlobal :483, DocumentNoDbCall :508, SupplierFillFromPurchaseInvoiceStore :534, TransporterFill :562, CmbCustomer_Leave :626,
 *   ItemdtFromPurchaseInvoiceStoreAgainstPartyId :651, PackUomFromGlobalBind :684, WareHouseBindFromGlobalRacksByItemId :698, RackBindFromGlobalRacksByItemId :733,
 *   JobLot/ItemCondition/City binds :780-:823, cmbItem_Leave :823, Add_Click :872, FillDetailRow :910, grdDetail_DoubleClick :940, DetailGridSettings :988,
 *   OtherItemsBind :1252, grdInvExpSettings :1282, PendingGatePassDbCall :1418, grdGp_ColumnButtonClick :1513, FormValidation :1549, FormValidationDetail :1572,
 *   Insert :1601, FillDetailListCommonForInsertAndDelete :1749, ReadById :1797, formReset :1890, btnRefresh_Click :1931, Print_Click :2002, HistoryFill :2019,
 *   DataGridHistory_SelectionChanged :2221, frmGSIssuance_KeyDown :2472.
 *
 * BLL/DAL chain (Architecture.BLL.Inventory.InvGdn.Save -> DAL.Inventory.InvGdn.SetData, one transaction): Sp_InvGdn_Insert / Sp_InvGdn_Update (EntryUser 0 on update,
 * ModifyUser 0 on insert), Sp_InvGdnDetail_Insert per row (LineId 1..n), Sp_InvGdnExpense_Insert per expense row, DMS attachments, Sp_InventoryTransactions_GetALLMethod,
 * then - because 144 is not a "valid" FIFO document type (IsValidDocType) and no row carries a stock reference - Sp_InventoryStockEvalautionDetail_Update,
 * USP_InventoryValidation per row, usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding. No order auto-complete (144 is not in IsValidForOrderComplete).
 */
@Service
public class StoreGoodsDispatchNotesService {
    public static final String SCREEN = "GoodsDispatchNotes_Store";
    public static final int DOC_TYPE = 144;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final Map<String, List<Object[]>> paramCache = new java.util.concurrent.ConcurrentHashMap<>();

    public StoreGoodsDispatchNotesService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (InitializeComponentMethod)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        boolean subsidiary = sup.erpFeature(4);                                          // SubsidiaryAccountAllownOnVouchers (GetConfigurationsFromGlobal)
        m.put("rights", sup.rights(SCREEN));
        m.put("subsidiary", subsidiary);
        m.put("nextNo", nextNo());                                                       // DocumentNoDbCall
        m.put("customers", customers());                                                 // SupplierFillFromPurchaseInvoiceStore
        m.put("transporters", transporters());                                           // TransporterFill
        m.put("vehicleTypes", vehicleTypes());                                           // VehicleType.GetAll
        m.put("jobLots", jobLots());                                                     // JobLotDtFillFromGlobalAndBind
        m.put("conditions", conditions());                                               // ItemConditionBindFromGlobal
        m.put("cities", cities());                                                       // CityDtFillFromGlobalAndBind
        m.put("racks", racks());                                                         // clsGlobalVariables.racksWithWarehouseAndItems
        m.put("uoms", uoms());                                                           // clsGlobalVariables.globalUomSchedule
        m.put("otherItems", otherItems());                                               // OtherItemsBind
        m.put("gatePasses", pendingGatePasses());                                        // PendingGatePassDbCall
        m.put("history", historyCombos());                                               // HistoryComboDbCall
        m.putAll(config());
        return m;
    }

    /** GetConfigurationsFromGlobal. */
    public Map<String, Object> config() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("wagesStatus", sup.configBool("WagesCompulsoryOnGdnStoreAndPm"));
        m.put("wagesActive", wagesActive());
        m.put("historyDays", sup.configInt("DefaultDaysToLessFromHistoryFromDate"));
        m.put("freightOutwardAc", sup.configInt("FreightOutwardAc"));
        m.put("defaultWarehouse", sup.configInt("Warehouse"));
        return m;
    }

    /** GlobalVariables_Helper.GetWagesRefDocumentsStatusById(144) -> USP_GetRefDocumentsForWages. */
    private boolean wagesActive() {
        for (Map<String, Object> r : sup.rows("USP_GetRefDocumentsForWages"))
            if (toInt(ci(r, "RefDocumentTypeId")) == DOC_TYPE) return toBool(ci(r, "IsActive"));
        return false;
    }

    /** btnRefresh_Click: the global lists are reloaded, then the combos are re-bound. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", customers());
        m.put("transporters", transporters());
        m.put("vehicleTypes", vehicleTypes());
        m.put("jobLots", jobLots());
        m.put("conditions", conditions());
        m.put("cities", cities());
        m.put("racks", racks());
        m.put("uoms", uoms());
        m.put("otherItems", otherItems());
        m.put("gatePasses", pendingGatePasses());
        m.putAll(config());
        return m;
    }

    /** formReset tail: DocumentNoDbCall + PendingGatePassGridBind(PendingGatePassDbCall()). */
    public Map<String, Object> resetData() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("nextNo", nextNo());
        m.put("gatePasses", pendingGatePasses());
        return m;
    }

    public int nextNo() {
        // InvGdn.GenerateInvGdnCode -> Sp_InvGdn_GetAllMethod 'GenerateInvGdnCode'
        List<Map<String, Object>> r = sup.rows("Sp_InvGdn_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerateInvGdnCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** InvPurchaseInvoice.GetPartiesFromPurchaseInvoiceStoreWithGlAccount. */
    public List<Map<String, Object>> customers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetPartiesFromPurchaseInvoiceStoreWithGlAccount", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", ci(r, "Id"), "CompanyName", ci(r, "CompanyName"), "GlAccountId", ci(r, "GlAccountId")));
        return out;
    }

    /** CmbCustomer_Leave -> InvPurchaseInvoice.GetItemsFromPurchaseInvoiceStoreAgainstPartyId (no DocumentTypeIds: the procedure falls back to 58,61,63,64,131). */
    public List<Map<String, Object>> items(int customerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetItemsFromPurchaseInvoiceStoreAgainstPartyId", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "SupplierCustomerId", customerId))
            out.add(row("Id", ci(r, "Id"), "ItemName", ci(r, "ItemName"), "ItemCode", ci(r, "ItemCodeNew"), "ItemCodeNew", ci(r, "ItemCodeNew")));
        return out;
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

    public List<Map<String, Object>> vehicleTypes() { return sup.rows("Sp_VehicleType_GetAllMethod"); }

    /** clsGlobalVariables.globalJobLot (USP_GetJobLotsAllocatedToBranch) -> Id, Description. */
    public List<Map<String, Object>> jobLots() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetJobLotsAllocatedToBranch", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", ci(r, "Id"), "Description", ci(r, "JobLotDescription")));
        return out;
    }

    /** clsGlobalVariables.globalItemConditions (V_ItemCondition) -> Id, Description. */
    public List<Map<String, Object>> conditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.jdbc().queryForList("SELECT * FROM dbo.V_ItemCondition"))
            out.add(row("Id", ci(r, "Id"), "Description", ci(r, "ConditionStatus")));
        return out;
    }

    /** clsGlobalVariables.globalAllCities (USP_City_GetAllWithCountryAndTehsil) -> Id, Description. */
    public List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_City_GetAllWithCountryAndTehsil", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", ci(r, "Id"), "Description", ci(r, "CityName")));
        return out;
    }

    /** clsGlobalVariables.racksWithWarehouseAndItems (usp_getRackswithWarehouseByItemId for the user's branch). */
    public List<Map<String, Object>> racks() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("usp_getRackswithWarehouseByItemId", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchId", idOrNull(sup.branch())))
            out.add(row("Id", toInt(ci(r, "Id")), "RackName", str(ci(r, "RackName")), "WarehouseId", toInt(ci(r, "invWarehouseId")),
                    "WareHouseName", str(ci(r, "WareHouseName")), "ItemId", toInt(ci(r, "ItemId")), "BaseRackId", toInt(ci(r, "BaseRackId"))));
        return out;
    }

    /** clsGlobalVariables.globalUomSchedule (usp_getAllUomsByCompanyId, Active 1); CommonServices.dtUomFromGloablUomScheduleByItemId filters it per item. */
    public List<Map<String, Object>> uoms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("usp_getAllUomsByCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Active", 1))
            out.add(row("Id", toInt(ci(r, "Id")), "ItemId", toInt(ci(r, "ItemId")), "UOMCode", str(ci(r, "UOMCode")), "Equivalent", toDouble(ci(r, "Equivalent")),
                    "BaseRateUom", toBool(ci(r, "BaseRateUom")), "BasePackUom", toBool(ci(r, "BasePackUom"))));
        return out;
    }

    /** InventoryItemsOther.GetAll -> Sp_InventoryItemsOther_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InventoryItemsOther_GetAllMethod", "Activity", "ReadAll", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", r.get("Id"), "OtherItemName", r.get("OtherItemName")));
        return out;
    }

    /** GatePassOutward.GetOutstandingGatePassForGdn_Store (grdGp). */
    public List<Map<String, Object>> pendingGatePasses() {
        return sup.rows("USP_GetOutstandingGatePassForGdn_Store", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "FinancialYearId", sup.fy(), "BranchesId", sup.branch());
    }

    /** HistoryComboBind: InvGdn.GetDataForDropDownFromGdn (org, company, DocumentTypeIds 144, BranchesIds) keeping Activity = Supplier. */
    public List<Map<String, Object>> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromGdn", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "BranchesIds", String.valueOf(sup.branch()), "DocumentTypeIds", String.valueOf(DOC_TYPE)))
            if ("Supplier".equals(str(r.get("Activity")))) cust.add(row("Id", r.get("Id"), "Name", r.get("ReferenceName")));
        return cust;
    }

    // ------------------------------------------------------------------ History (HistoryFill / DataGridHistory_SelectionChanged)

    public static class HistoryFilter {
        public String fromDate, toDate;        // empty when the box is not checked
        public int fromDocNo, toDocNo, customerId;
    }

    public List<Map<String, Object>> history(HistoryFilter f) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate from = parseDate(f.fromDate), to = parseDate(f.toDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("FinancialYearId", sup.fy()); p.put("BranchesId", sup.branch());
        p.put("DocumentTypeId", DOC_TYPE);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (f.fromDocNo != 0) p.put("FromDocNo", (double) f.fromDocNo);
        if (f.toDocNo != 0) p.put("ToDocNo", (double) f.toDocNo);
        if (f.customerId != 0) p.put("SupplierCustomerId", f.customerId);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", sup.userId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "USP_IssuanceGDNFormHistory", p))
            out.add(row("Id", toInt(ci(r, "Id")), "DocNo", toInt(ci(r, "DocNo")), "DocDate", ci(r, "DocDate"), "Customer", str(ci(r, "CustomerName")),
                    "ManualNo", str(ci(r, "ReferenceDocNo")), "EntryDate", ci(r, "EntryDate"), "EntryUser", str(ci(r, "UserName")),
                    "Remarks", str(ci(r, "RemarksHeader")), "NoOfAttachments", toInt(ci(r, "NoOfAttachments"))));
        return out;
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
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert)

    public static class Line {
        public int id;
        public int itemId;
        public int warehouseId;
        public int rackId;
        public int itemConditionId;
        public int jobLotId;
        public int packUomId;
        public double itemQty;
        public int cityId;
        public String remarks;
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
        public int customerId;
        public String manualNo;
        public int transporterId;
        public String freight;
        public String remarks;
        public int gatePassId;
        public String gpNo;
        public int vehicleTypeId;
        public String vehicleType;
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
        List<Line> rows = r.lines == null ? List.of() : r.lines;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        formValidation(r);

        Map<String, Object> old = null;
        if (r.id > 0) old = header(r.id);

        double freight = toDouble(r.freight);
        int transporterId = 0, transporterSupCustId = 0;
        double carriage = 0.0;
        if (freight > 0.0) {
            if (r.transporterId <= 0) throw new IllegalArgumentException("Transporter Account field required");
            carriage = freight;
            if (!subsidiary) {
                transporterId = r.transporterId;
            } else {
                // SelectedRow.Cells[3] of the vendor list (the SupplierCustomerId column) is the TransporterId, the combo value the TransporterSupCustId
                Map<String, Object> t = null;
                for (Map<String, Object> x : transporters()) if (toInt(x.get("Id")) == r.transporterId) t = x;
                if (t == null) throw new IllegalArgumentException("Transporter Account field required");
                transporterId = toInt(t.get("SupplierCustomerId"));
                transporterSupCustId = r.transporterId;
            }
        }

        List<Line> details = new ArrayList<>();
        double totalQty = 0.0;
        int idx = 0;
        for (Line l : rows) {
            int rowIndex = idx++;
            if (!((int) Math.rint(l.itemQty) > 0)) continue;      // Conversion.ToInt(ItemQty) > 0 (Convert.ToInt32 rounds to even)
            required(l.itemId, "Item", rowIndex);
            required(l.warehouseId, "Warehouse", rowIndex);
            required(l.rackId, "Rack Name", rowIndex);
            required(l.itemConditionId, "Item Condition", rowIndex);
            required(l.packUomId, "Pack Uom", rowIndex);
            required(l.jobLotId, "JobLot", rowIndex);
            required(l.itemQty, "Qty", rowIndex);
            totalQty += l.itemQty;
            details.add(l);
        }
        // BLL.Save: a new document may not carry a detail Id
        if (r.id == 0) for (Line l : details) if (l.id > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");   // DAL.InvGdn.SetData

        List<Expense> exps = new ArrayList<>();
        for (Expense e : (r.expenses == null ? List.<Expense>of() : r.expenses)) if (e.itemId > 0 && e.qty > 0.0) exps.add(e);

        Timestamp now = now();
        java.sql.Date docDate = java.sql.Date.valueOf(parseDate(r.docDate) == null ? LocalDate.now() : parseDate(r.docDate));
        Map<String, Object> h = new LinkedHashMap<>();
        for (String k : List.of("IsApproved", "AddWages", "ShortWeightDeductionApply", "IsStockReserved")) h.put(k, false);
        h.put("DocDate", docDate);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("CarriageAmount", carriage);
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
        for (String k : List.of("PostUser", "ProjectsId", "ReferencePartyId", "StockPartyId", "BillCalculateTypeId", "VehicleTypeId", "OrderId", "OrderTypeId",
                "AdvanceDeliveryOrderId", "RequestedByLookUpId")) h.put(k, 0);
        h.put("SupplierCustomerId", r.customerId);
        h.put("TransporterId", transporterId);
        h.put("TransporterSupCustId", transporterSupCustId);
        if (r.vehicleTypeId > 0) h.put("VehicleType", text(r.vehicleType));        // only when the vehicle-type combo has a value
        h.put("BiltyNo", text(r.biltyNo));
        h.put("ReferenceDocNo", text(r.manualNo));
        h.put("RemarksHeader", r.remarks == null ? "" : r.remarks);
        h.put("ScreenName", SCREEN);
        h.put("VehicleNo", r.vehicleNo == null ? "" : r.vehicleNo);
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
            // FillDetailListCommonForInsertAndDelete: StockWeight = GrossWeight = ItemQty = NetBillWeight
            d.put("GrossWeight", l.itemQty); d.put("NetBillWeight", l.itemQty); d.put("StockWeight", l.itemQty); d.put("ItemQty", l.itemQty);
            d.put("IsAssetItem", false);
            for (String k : List.of("DeliveryScheduleId", "ItemVariantId", "CastingTypeId", "ProductionStageId", "PackingTypeId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo",
                    "RefDocSubIdNo", "GrnId", "GrnDetailId", "GrnDocumentTypeId", "DepartmentId", "WbTicketId", "GpNo", "RateUomId", "CropYearId",
                    "OrderTypeId", "BrandItemId", "SecondaryUomId", "SaleOrderId", "SaleOrderDetailId", "WareHouseToId", "DeliveryTypeId", "AssetId",
                    "InvDeliveryOrderId", "InvDeliveryOrderDetailId")) d.put(k, 0);
            d.put("SecondaryUomQty", java.math.BigDecimal.ZERO);
            d.put("Id", l.id);
            d.put("ActionTypeId", l.id <= 0 ? 1 : 2);
            d.put("InvGdnId", id);
            d.put("LineId", lineId);
            d.put("ItemId", l.itemId);
            d.put("ItemUomId", l.packUomId);
            d.put("JobLotId", l.jobLotId);
            d.put("WarehouseId", l.warehouseId);
            d.put("RackId", l.rackId);
            d.put("ItemConditionId", l.itemConditionId);
            d.put("CityId", l.cityId);
            d.put("CommentsDetail", l.remarks == null ? "" : l.remarks);
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
        // stock evaluation branch of DAL.InvGdn.SetData for 144 (IsValidDocType is false and no row has a stock reference)
        sup.setProcMap("Sp_InventoryStockEvalautionDetail_Update", full("Sp_InventoryStockEvalautionDetail_Update",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        for (Line l : details) {
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("OrganizationId", u.getOrganizationId()); v.put("CompanyId", u.getCompanyId()); v.put("DocumentTypeId", DOC_TYPE); v.put("DocDate", docDate);
            v.put("ItemId", l.itemId); v.put("WarehouseId", l.warehouseId); v.put("JobLotId", l.jobLotId); v.put("InvPackingTypeId", 0); v.put("PackUomId", l.packUomId);
            v.put("NetWeight", l.itemQty); v.put("RefDocumentTypeId", 0); v.put("RefDocNoId", 0); v.put("RefDocSubIdNo", 0); v.put("ItemConditionId", l.itemConditionId);
            DesktopProc.scalar(sup.jdbc(), "USP_InventoryValidation", v);
        }
        DesktopProc.scalar(sup.jdbc(), "usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "Id", id));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Record Update SuccessFully" : "Record Save SuccessFully") + toInt(r.docNo));
        // frmwagesBillHeader (RefDocTypeId 144, RefDocId = id, GrossWeightTotal = total quantity) opens after the message when both wage switches are on
        boolean wages = sup.configBool("WagesCompulsoryOnGdnStoreAndPm") && wagesActive();
        out.put("wages", wages);
        out.put("wagesRefDocTypeId", DOC_TYPE);
        out.put("wagesRefDocId", id);
        out.put("wagesGrossWeightTotal", totalQty);
        return out;
    }

    /** FormValidation(): the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || toInt(no) == 0) throw new Warning("DocNo Field Required");
        double f = toDouble(r.freight);
        if (r.transporterId > 0 && !(f > 0.0)) throw new Warning("Freight Amount Field is Required");
        if (f > 0.0 && r.transporterId <= 0) throw new Warning("Transporter Field is Required");
    }

    /** FormHelper.ValidateField:503 - "{field} is required in Detail Grid at row No: {n}". */
    private static void required(int v, String name, int rowIndex) {
        if (v == 0) throw new IllegalArgumentException(name + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    private static void required(double v, String name, int rowIndex) {
        if (v <= 0.0) throw new IllegalArgumentException(name + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    // ------------------------------------------------------------------ helpers

    /**
     * GenericProvider.SetProc of a model with many properties: every procedure parameter that the caller did not give receives the
     * property default (0 / false; an unset string or date is left out).
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
