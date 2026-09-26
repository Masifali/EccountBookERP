package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.GrnStoreDto;
import com.mst.repositories.GrnStoreRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 324 "Grn Store" — Store Purchase (ModuleId 67). Desktop form {@code GrnStore.cs},
 * base.Name / ScreenName {@code "GrnStore"}, DocumentTypeId <b>48</b> (constructor, GrnStore.cs:314;
 * the 42 that also appears in the form is the Purchase Order document type sent to the PO slip
 * behind the register's OrderNo link, :2553). Page /store/grn-store, API /api/store/grn-store.
 *
 * <pre>
 *   BLL 0576 / DAL 0429  Architecture.(BLL|DAL).Inventory.InvGrn   Save, SetData, GetByID/GetDate,
 *                        GenerateInvGrnCode, GetHisoty, GrnRegisterStore, GetDataForDropDownFromGrn,
 *                        GetRecordsById
 *   Model 1026 / 1027    InvGrn / InvGrnDetail (parameter order and CLR defaults)
 *   BLL 0611 / DAL 0464  VehicleType.GetAll
 *   BLL 0557             InvPurchaseDemandHeader.GetPendingPurchaseOrderForGrnStore / GetDataForDropDownFromPurchaseDemand
 *   BLL 0595             PurchaseOrder.GetDataForDropDownFromPurchaseOrder
 *   BLL 0314             PurchasePreBillHeader.PurchaseDemand_PendingDataLoader
 *   BLL 0313             DeliveryChallanHeader.GetDataForDropDownFromDeliveryChallanHeader / _PendingDataLoader
 *   BLL 0131             InvGrnandGdnReports.GrnSlipStore (212-Print)
 *   BLL 0134             PurchaseOrderReports.PurchaseOrderSlipReport203 (register OrderNo link)
 *   BLL 0056             GetAvgRatesAndStockInHand (AvailableStockQty, via StoreIssuanceRepository)
 *   BLL 0581 / DAL 0434  InvPurchaseInvoice.RemoveByID (Delete, via StoreIssuanceRepository)
 *   Loaders: frmPendingPurchaseOrderStoreLoader.cs (DocumentTypeId 42), frmPendingPurchaseDemand.cs (141),
 *            frmPendingDeliveryChallanLoader.cs (148)
 * </pre>
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. History (HistoryGridFill:1456) never sets ReportsParameters.EntryUser, and GetHisoty adds
 *     {@code @EntryUser} whenever CanViewAllRecord is false — so a user without the
 *     "CanView AllRecord" right queries EntryUser = 0 and sees no rows.
 *  2. The Supplier combo is enabled only when BaseDocumentTypeId == 2 (EnabledDisabledFeilds:1896),
 *     yet Insert():1078 requires it. Before anything is loaded (after New/Save) it is disabled, so a
 *     GRN can only be saved after a PO / Demand / Delivery-Challan load. On the very first open
 *     EnabledDisabledFeilds has not run yet and every control is enabled (Form_Load never calls it).
 *  3. Gate Pass No is a required free-text box (:1079) even when no gate pass exists; GpNo is
 *     Conversion.ToInt of that text.
 *  4. EntryDate and ModifyDate are DateTime.Today (midnight), not Now (:1104-1107). ProjectsId is the
 *     BranchesId (:1099). TransporterDocRef is "" and OtherCharges 0.
 *  5. Delivery-Challan load puts PurchaseOrderDetailId into OrderId and PurchaseOrderHeaderId into
 *     OrderDetailId (:2282-2283) — the two ids are swapped and saved that way (JS carries this).
 *  6. The PO loader's SupplierId is assigned after ShowDialog returns (:1928), so its first search
 *     never filters by the GRN's supplier. Its ItemId filter is set but GetPendingPurchaseOrderForGrnStore
 *     never sends it. Its From/To Doc No are sent as @FromDocNo/@ToDocNo, which
 *     Sp_InvPurchaseDemand_GetAllMethod does not declare (it has @DocNoFrom/@DocNoTo), so a Doc No
 *     filter makes SQL Server refuse the call; the error is shown as the desktop shows it.
 *  7. The PO loader reads the "PurchaseOrderBranchWise" configuration only after its first combo
 *     fill, so the combos on open are never branch-filtered; its Refresh is.
 *  8. With ERP feature 4 (SubsidiaryAccountAllownOnVouchers) on, Insert():1125 reads
 *     CmbTransporter.SelectedRow.Cells[3] before the carriage check: no selected transporter is a
 *     NullReferenceException ("Object reference not set to an instance of an object.").
 *  9. The Register's Transporter filter is bound but never sent to USp_InvGrnStore_Register (:2398-2413).
 * 10. The PO loader grid shows no ItemConditionId column; rows loaded from it carry ItemConditionId 0
 *     and the per-row validation then requires the user to pick one.
 * 11. Doc No on a new save: the procedure Sp_InvGrn_Insert computes its own DocNo; the success
 *     message shows the number the form held (web: the generator's number at save time).
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  D1. Doc No is read-only and never trusted: generator on insert, stored header on update.
 *  D2. Open / update / delete / slip only for a header of this company and DocumentTypeId 48.
 *  D3. Attachments (AttachmentsList / DeleteAttachmentsList) are not ported; an update re-sends the
 *      stored AttachmentsValues / CustomAttachmentsValues unchanged, a new save sends "".
 *  D4. Prints return the procedure rows; the Crystal layouts (212, 336, 201) are out of scope.
 *  D5. Dates are returned to the page as "yyyy-MM-dd HH:mm:ss" text so no time-zone shift can move a day.
 *  D6. A transporter row posted with TransporterSupCustId is accepted only when that pair exists in the
 *      transporter list; otherwise the first row with that account id supplies it.
 *
 *  Tenancy checks on Save / Update (web only; the desktop trusts its own grid). All run after the
 *  desktop's own validations, inside the save, before anything is written:
 *  T1. BaseDocumentTypeId must be 1, 2 or 3 (rows exist only after a load). On update the stored
 *      header's BaseDocumentTypeId and InwardGatePassId are used; rows added on update must be of that
 *      same base type and carry that same gate pass.
 *  T2. Every row with Id 0 is looked up in the matching pending loader, re-run server-side for this
 *      organization / company / branch / financial year with no date or number filter
 *      (GetPendingPurchaseOrderForGrnStore, USP_InvPurchaseDemand_PendingDataLoader,
 *      USP_DeliveryChallanHeader_PendingDataLoader). The posted detail id must be pending and its
 *      header id must match; the row's other reference ids, ItemId and UOM id are then taken from
 *      that pending row, never from the request (Delivery Challan keeps the desktop's swapped order
 *      ids, note 5). PO and Delivery Challan rows must all come from one document, as must Demand
 *      rows when a gate pass is attached.
 *  T3. InwardGatePassId is the GatePassId of the loaded document's pending row (PO / Demand: when > 0;
 *      Delivery Challan: always); a posted value that differs is refused.
 *  T4. Supplier: for PO / Delivery Challan it must equal the source document's party
 *      (SupplierCustomerId / BillToPartyId); for Demand it must be in this company's supplier list.
 *      Transporter must be 0 or in this company's transporter list.
 *  T5. WarehouseId / RackId must be a rack of that warehouse holding that item in this company's
 *      branch (usp_getRackswithWarehouseByItemId); ItemConditionId must be in V_ItemCondition minus 4.
 *  T6. Update: every posted row Id > 0 must be a line of the stored document (else refused); its
 *      reference ids, ItemId and UOM id come from the stored line. A stored line referenced by a live
 *      703 Goods Dispatch Note For Purchase Return must be posted with its own Id.
 *  T7. Loader Doc No slips (201 / 454 / 148) run with the session's organization and company.
 */
@Service
public class GrnStoreService {

    private static final Logger LOG = LoggerFactory.getLogger(GrnStoreService.class);

    public static final int DOCUMENT_TYPE_ID = 48;
    public static final String SCREEN_NAME = "GrnStore";
    public static final String REF_DOCUMENT_TYPE_IDS = "1,2,3";      // :1469, :2404
    private static final int PO_DOCUMENT_TYPE_ID = 42;                // frmPendingPurchaseOrderStoreLoader:108
    private static final int DEMAND_DOCUMENT_TYPE_ID = 141;           // frmPendingPurchaseDemand:96
    private static final int CHALLAN_DOCUMENT_TYPE_ID = 148;          // frmPendingDeliveryChallanLoader:98
    private static final Set<Integer> EXCLUDED_TRANSPORTER_TYPES =
            new HashSet<>(Arrays.asList(2, 4, 10, 11, 12, 13, 14, 15, 20, 21, 22));   // TransporterDtFillFromGlobal:622-626 (int[11])

    private final GrnStoreRepository repo;
    private final StoreIssuanceRepository store;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public GrnStoreService(GrnStoreRepository repo, StoreIssuanceRepository store, StoreScreenRights rights,
                           CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.store = store;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ================================================================================ lookups

    /** InitializeComponentMethod:384 — everything Form_Load binds. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        int branch = branch(u);
        boolean subsidiary = repo.erpFeature(u, 4);                               // :388
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));                                 // :389
        out.put("docNo", repo.nextDocNo(u, DOCUMENT_TYPE_ID, fy, branch));         // :390 GetDocumentCode
        out.put("subsidiaryAccountAllownOnVouchers", subsidiary);
        out.put("wareHouseConfigId", toInt(store.config(u, "DefaultWarehouseForStoreFlow")));              // :527
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(store.config(u, "DefaultDaysToLessFromHistoryFromDate"))); // :528
        out.put("freightInwardAc", toInt(store.config(u, "FreightInwardAc")));     // :510
        out.put("financialYearStart", financialYearStart(u, fy));
        out.put("suppliers", suppliers(u));
        out.put("transporters", transporters(u, subsidiary));
        out.put("vehicleTypes", vehicleTypes());
        out.put("itemConditions", store.itemConditions());                         // GridDtFill:685 (Id != 4)
        out.put("racks", store.racksWithWarehouseAndItem(u, branch));
        out.put("historyFilters", historyFilters());
        return out;
    }

    /** HistoryComboBind:461 — BtnRefresh_Click:2367 re-reads it (History and Register Refresh). */
    public Map<String, List<Map<String, Object>>> historyFilters() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        for (String a : new String[] { "Supplier", "Transporter", "Warehouse", "Item" }) out.put(a, new ArrayList<>());
        for (Map<String, Object> r : repo.grnDropDowns(u, String.valueOf(DOCUMENT_TYPE_ID),
                String.valueOf(u.getBranchesId() == null ? 0 : u.getBranchesId()), 0)) {
            List<Map<String, Object>> t = out.get(str(r.get("Activity")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        return out;
    }

    /** SupplierDtFillFromGlobal:562 — Id, CompanyName, PartyCode, GlAccountId, CityId, CityName, MobileNo. */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.suppliers(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CompanyName", str(r.get("CompanyName")));
            o.put("PartyCode", str(r.get("PartyCode")));
            o.put("GlAccountId", toInt(r.get("GlAccountId")));
            o.put("CityId", toInt(r.get("CityId")));
            o.put("CityName", str(r.get("CityName")));
            o.put("MobileNo", str(r.get("MobilePersonal")));
            out.add(o);
        }
        return out;
    }

    /**
     * TransporterDtFillFromGlobal:605. Feature 4 on: every vendor/customer for transporter, value
     * member = GlAccountId, SupplierCustomerId = the party. Off: every account from the custom
     * groups except the eleven excluded AccountTypeIds, distinct by ChartOfAccountId (first wins).
     */
    private List<Map<String, Object>> transporters(UserAccount u, boolean subsidiary) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (Map<String, Object> r : repo.vendorsAndCustomersForTransporter(u)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(r.get("GlAccountId")));
                o.put("AccountTitle", str(r.get("CompanyName")));
                o.put("AccountCode", str(r.get("PartyCode")));
                o.put("SupplierCustomerId", toInt(r.get("Id")));
                out.add(o);
            }
            return out;
        }
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : repo.accountsWithCustomGroup(u)) {
            if (EXCLUDED_TRANSPORTER_TYPES.contains(toInt(r.get("AccountTypeId")))) continue;
            int id = toInt(r.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(r.get("AccountTitle")));
            o.put("AccountCode", str(r.get("AccountCode")));
            o.put("SupplierCustomerId", 0);
            out.add(o);
        }
        return out;
    }

    /** VehicleTypesBind:665 — Id / VehicleDescription. */
    private List<Map<String, Object>> vehicleTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.vehicleTypes()) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("VehicleDescription", str(r.get("VehicleDescription")));
            out.add(o);
        }
        return out;
    }

    /** reset():1350 — GetDocumentCode after New / Save / Delete. */
    public int nextDocNo() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.nextDocNo(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(), branch(u));
    }

    // ================================================================================== stock

    /**
     * RowBalanceStockQtyUpdate:956 — QtyInHand rounded to 2 places, 0 without an item. The
     * document's own id and type are sent only for an opened document (:968).
     */
    public List<Double> availableStock(String docDate, int recId, List<Map<String, Object>> rows) {
        UserAccount u = ctx.requireAccountingUser();
        if (recId > 0 && ownedHeader(u, recId) == null) recId = 0;
        Timestamp d = StoreIssuanceService.formDate(docDate, recId);
        List<Double> out = new ArrayList<>();
        for (Map<String, Object> r : rows == null ? new ArrayList<Map<String, Object>>() : rows) {
            int itemId = toInt(ci(r, "ItemId"));
            if (itemId == 0) { out.add(0d); continue; }
            Map<String, Object> s = store.avgRateAndStock(u, itemId, d, toInt(ci(r, "ItemConditionId")), recId,
                    recId > 0 ? DOCUMENT_TYPE_ID : 0, toInt(ci(r, "WarehouseId")), toInt(ci(r, "RackId")));
            double q = 0d;
            if (s != null && s.get("QtyInHand") != null) {
                q = java.math.BigDecimal.valueOf(toDouble(s.get("QtyInHand"))).setScale(2, java.math.RoundingMode.HALF_EVEN).doubleValue();
            }
            out.add(q);
        }
        return out;
    }

    // ================================================================================ history

    /** HistoryGridFill:1456 → GetHisoty. dateType: doc | entry | modify | approved (the four radios). */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo, int supplierId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        int fy = ctx.currentFinancialYearId();
        int branch = branch(u);
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "CanViewAllRecord", viewAll);
        if (fy != 0) p.put("FinancialYearId", fy);
        if (branch != 0) p.put("BranchesId", branch);
        p.put("BaseDocumentTypeIds", REF_DOCUMENT_TYPE_IDS);
        if (!viewAll) p.put("EntryUser", 0);                   // desktop note 1: EntryUser is never set
        Timestamp f = blank(from) ? null : StoreIssuanceService.pickerDate(from);
        Timestamp t = blank(to) ? null : StoreIssuanceService.pickerDate(to);
        String type = dateType == null ? "doc" : dateType;
        switch (type) {
            case "entry":    put(p, "EntryFromDate", f); put(p, "EntryToDate", t); break;
            case "modify":   put(p, "ModifyFromDate", f); put(p, "ModifyToDate", t); break;
            case "approved": put(p, "ApprovedFromDate", f); put(p, "ApprovedToDate", t); break;
            default:         put(p, "fromDate", f); put(p, "toDate", t); break;
        }
        if (fromDocNo != 0) p.put("GrnNoF", fromDocNo);
        if (toDocNo != 0) p.put("GrnNoT", toDocNo);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(p)) {                           // :1545-1548
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("BaseDocumentTypeId", toInt(r.get("BaseDocumentTypeId")));
            o.put("BaseDocumentType", str(r.get("BaseDocumentType")));
            o.put("DocDate", text(r.get("DocDate")));
            o.put("DocNo", r.get("DocNo"));
            o.put("SupplierName", str(r.get("SupplierName")));
            o.put("GpNo", r.get("GpNo"));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("BiltyNo", str(r.get("BiltyNo")));
            o.put("Transporter", str(r.get("Transporter")));
            o.put("FreightAmount", toDouble(r.get("CarriageAmount")));
            o.put("RemarksHeader", str(r.get("RemarksHeader")));
            o.put("EntryUser", str(r.get("EntryUser")));
            o.put("EntryDate", text(r.get("EntryDate")));
            o.put("ModifyUser", str(r.get("ModifyUser")));
            o.put("ModifyDate", text(r.get("ModifyDate")));
            o.put("NoOfAttachments", r.get("NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    /**
     * RegisterGridFill:2391 → GrnRegisterStore. Returns the grid rows (dtRegisterGrid, :2420-2447)
     * and the procedure's own rows (dtRegisterFromDb), which the 336-Print prints.
     */
    public Map<String, Object> register(String from, String to, int fromDocNo, int toDocNo, int supplierId,
                                        int warehouseId, int itemId, boolean onlyPending) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("BaseDocumentTypeIds", REF_DOCUMENT_TYPE_IDS);
        p.put("GrnDateF", StoreIssuanceService.pickerDate(from));
        p.put("GrnDateT", StoreIssuanceService.pickerDate(to));
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (fromDocNo != 0) p.put("GrnNoF", fromDocNo);
        if (toDocNo != 0) p.put("GrnNoT", toDocNo);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (onlyPending) p.put("OnlyPending", 1);
        List<Map<String, Object>> raw = repo.register(p);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("BaseDocumentType", str(r.get("BaseDocumentType")));
            o.put("DocDate", text(r.get("DocDate")));
            o.put("DocNo", r.get("DocNo"));
            o.put("OrderId", toInt(r.get("OrderId")));
            o.put("OrderNo", r.get("PoNo"));
            o.put("OrderDate", text(r.get("PoDate")));
            o.put("SupplierCustomerId", toInt(r.get("SupplierCustomerId")));
            o.put("PartyName", str(r.get("CompanyName")));
            o.put("ReferenceNo", str(r.get("ReferenceDocNo")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("BiltyNo", str(r.get("BiltyNo")));
            o.put("TransporterId", toInt(r.get("TransporterId")));
            o.put("TransporterName", str(r.get("TransporterName")));
            o.put("FreightAmount", toDouble(r.get("FreightAmount")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemUOM", str(r.get("UOMCode")));
            o.put("ItemQty", toDouble(r.get("ItemQty")));
            o.put("WareHouseName", str(r.get("WareHouseName")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            o.put("DocumentTypeId", toInt(r.get("DocumentTypeId")));
            grid.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", grid);
        out.put("raw", plain(raw));
        return out;
    }

    // =================================================================================== read

    /** ReadById:1180 — header and FillDetailFromListCommonForReadById:1227 rows. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.storeDetails(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(d.get("Id")));
            o.put("OrderId", toInt(d.get("PurchaseOrderId")));
            o.put("OrderDetailId", toInt(d.get("PurchaseOrderDetailId")));
            o.put("OrderNo", toInt(d.get("PurchaseOrder")));
            o.put("DemandId", toInt(d.get("InvPurchasedemondId")));
            o.put("DemandDetailId", toInt(d.get("PurchaseDemondDetailId")));
            o.put("DemandNo", toInt(d.get("PurchaseDemandNo")));
            o.put("DeliveryChallanId", toInt(d.get("DeliveryChallanId")));
            o.put("DeliveryChallanDetailId", toInt(d.get("DeliveryChallanDetailId")));
            o.put("DeliveryChallanNo", toInt(d.get("DeliveryChallanNo")));
            o.put("PurchasePreBillId", toInt(d.get("PurchasePreBillId")));
            o.put("PurchasePreBillDetailId", toInt(d.get("PurchasePreBillDetailId")));
            o.put("PurchasePreBillNo", toInt(d.get("PurchasePreBillNo")));
            o.put("PurchasePreBillQty", toDouble(d.get("PurchasePreBillQty")));
            o.put("WarehouseId", toInt(d.get("WarehouseId")));
            o.put("WarehouseName", str(d.get("WareHouseCode")));                  // :1251 WareHouseCode
            o.put("ItemCondition", str(d.get("ConditionStatus")));                // :1254 (history detail)
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("ItemName", str(d.get("Item")));
            o.put("UOMId", toInt(d.get("ItemUomId")));
            o.put("UOM", str(d.get("UOMCode")));
            o.put("UomEquivalent", toDouble(d.get("UOM")));
            o.put("TotalOrderQty", toDouble(d.get("PurchaseOrderQty")));
            o.put("UsedOrderQty", toDouble(d.get("TotalUsedOrderQty")));
            o.put("BalOrderQty", toDouble(d.get("BalOrderQty")));
            o.put("TotalDemandQty", toDouble(d.get("PurchaseDemandQty")));
            o.put("UsedDemandQty", toDouble(d.get("TotalUsedDemandQty")));
            o.put("BalDemandQty", toDouble(d.get("BalDemandQty")));
            o.put("TotalDeliveryChallanQty", toDouble(d.get("DeliveryChallanQty")));
            o.put("UsedDeliveryChallanQty", toDouble(d.get("UsedDeliveryChallanQtyGrn")));
            o.put("BalDeliveryChallanQty", toDouble(d.get("BalDeliveryChallanQty")));
            o.put("ThisQty", toDouble(d.get("ItemQty")));
            o.put("Remarks", str(d.get("CommentsDetail")));
            o.put("ItemConditionId", toInt(d.get("ItemConditionId")));
            o.put("RackId", toInt(d.get("RackId")));
            o.put("RackName", str(d.get("rackName")));
            o.put("AvailableStockQty", 0d);
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("DocDate", text(h.get("DocDate")));
        out.put("SupplierCustomerId", toInt(h.get("SupplierCustomerId")));
        out.put("ReferenceDocNo", str(h.get("ReferenceDocNo")));
        out.put("InwardGatePassId", toInt(h.get("InwardGatePassId")));
        out.put("GpNo", toInt(h.get("GpNo")));
        out.put("VehicleType", str(h.get("VehicleType")));
        out.put("VehicleNo", str(h.get("VehicleNo")));
        out.put("BiltyNo", str(h.get("BiltyNo")));
        out.put("TransporterId", toInt(h.get("TransporterId")));
        out.put("CarriageAmount", toDouble(h.get("CarriageAmount")));
        out.put("RemarksHeader", str(h.get("RemarksHeader")));
        out.put("BaseDocumentTypeId", toInt(h.get("BaseDocumentTypeId")));
        out.put("rows", rows);
        return out;
    }

    // ================================================================================= prints

    /** ShowPrint:1880 → GrnSlipReport212:14438 — Id 0 is "No Record Found For Display". */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        if (ownedHeader(u, id) == null) return null;
        return plain(repo.slip(u, DOCUMENT_TYPE_ID, id));
    }

    /**
     * GrdiRegister_LinkClicked:2538 — OrderNo opens the PO slip when the user's View right on screen
     * "PurchsaeOrder" is true; "You dont have right" otherwise.
     */
    public List<Map<String, Object>> purchaseOrderSlip(int orderId) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has("PurchsaeOrder", "view")) throw new IllegalStateException("You dont have right");
        List<Map<String, Object>> r = repo.purchaseOrderSlip(u, orderId);
        if (r == null || r.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");
        return plain(r);
    }

    // ================================================================================ loaders

    /**
     * frmPendingPurchaseOrderStoreLoader.ComboDbCall:156 → CombosFill:180 ("Supplier" / "Item").
     * refresh=false is the open (BranchImplemented still false, desktop note 7); true is its Refresh.
     */
    public Map<String, Object> purchaseOrderLoaderLookups(boolean refresh) {
        UserAccount u = ctx.requireAccountingUser();
        boolean branchWise = refresh && StoreIssuanceService.toBool(store.config(u, "PurchaseOrderBranchWise"));
        List<Map<String, Object>> rows = repo.purchaseOrderDropDowns(u, String.valueOf(PO_DOCUMENT_TYPE_ID),
                branchWise ? String.valueOf(branch(u)) : "");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", activity(rows, "Supplier"));
        out.put("items", activity(rows, "Item"));
        return out;
    }

    /** PendingDataDbCall:225 → GetPendingPurchaseOrderForGrnStore. */
    public List<Map<String, Object>> pendingPurchaseOrders(String from, String to, int fromDocNo, int toDocNo, int supplierId) {
        UserAccount u = ctx.requireAccountingUser();
        return plain(repo.pendingPurchaseOrders(u, PO_DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(), branch(u), supplierId,
                StoreIssuanceService.pickerDate(from), StoreIssuanceService.pickerDate(to), fromDocNo, toDocNo));
    }

    /** frmPendingPurchaseDemand.ComboDbCall:142 — (org, comp, "141", "Item"); every row goes to the Item combo. */
    public Map<String, Object> purchaseDemandLoaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.purchaseDemandDropDowns(u, String.valueOf(DEMAND_DOCUMENT_TYPE_ID), "Item")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            items.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items);
        return out;
    }

    /** frmPendingPurchaseDemand.PendingDataDbCall:197 → PurchaseDemand_PendingDataLoader. */
    public List<Map<String, Object>> pendingPurchaseDemands(String from, String to, int fromDocNo, int toDocNo, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        return plain(repo.pendingPurchaseDemands(u, branch(u), ctx.currentFinancialYearId(), DEMAND_DOCUMENT_TYPE_ID,
                StoreIssuanceService.pickerDate(from), StoreIssuanceService.pickerDate(to), fromDocNo, toDocNo, itemId));
    }

    /** frmPendingDeliveryChallanLoader.ComboDbCall:144 → CombosFill:166 ("BillToPartyName" / "Item"). */
    public Map<String, Object> deliveryChallanLoaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = repo.deliveryChallanDropDowns(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("billToParties", activity(rows, "BillToPartyName"));
        out.put("items", activity(rows, "Item"));
        return out;
    }

    /** frmPendingDeliveryChallanLoader.PendingDataDbCall:211 → DeliveryChallanHeader_PendingDataLoader. */
    public List<Map<String, Object>> pendingDeliveryChallans(String from, String to, int fromDocNo, int toDocNo,
                                                             int itemId, int billToPartyId) {
        UserAccount u = ctx.requireAccountingUser();
        return plain(repo.pendingDeliveryChallans(u, branch(u), ctx.currentFinancialYearId(), CHALLAN_DOCUMENT_TYPE_ID,
                StoreIssuanceService.pickerDate(from), StoreIssuanceService.pickerDate(to), fromDocNo, toDocNo, itemId, billToPartyId));
    }

    // =================================================================================== save

    /**
     * btnSave_Click:1003 / btnUpdate_Click:1016 → Insert():1064 → BLL InvGrn.Save:60 → DAL SetData:53.
     * One transaction, in DAL order: header, details, InventoryTransactions, the stock evaluation
     * when a gate pass is attached, usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn.
     */
    @Transactional
    public Map<String, Object> save(GrnStoreDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> stored = recId != 0 ? ownedHeader(u, recId) : null;
        if (recId != 0 && stored == null) throw new IllegalArgumentException("No Record Found");

        List<GrnStoreDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");                  // :1072
        int supplierId = i(dto.SupplierCustomerId);
        if (supplierId == 0) throw new IllegalArgumentException("Supplier field is required");               // :1078
        if (blank(dto.GpNo)) throw new IllegalArgumentException("Gate Pass No field is required");         // :1079
        int baseType = i(dto.BaseDocumentTypeId);
        int gpId = i(dto.InwardGatePassId);
        if (baseType == 3 && gpId == 0) throw new IllegalArgumentException("No Record found against gp no");   // :1084

        int fy = ctx.currentFinancialYearId();
        int branch = branch(u);
        /* D1 — txtdocno is ReadOnly: the stored number on update, the generator's on insert. */
        int docNo = stored != null ? toInt(ci(stored, "DocNo")) : repo.nextDocNo(u, DOCUMENT_TYPE_ID, fy, branch);
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);                               // :1109
        Timestamp today = Timestamp.valueOf(LocalDate.now().atStartOfDay());                                  // :1104 / :1106

        /* :1119-1133 — the transporter, then the carriage check. */
        boolean subsidiary = repo.erpFeature(u, 4);
        int transporterId = i(dto.TransporterId);
        int transporterSupCustId = 0;
        if (subsidiary) {
            if (transporterId == 0) throw new IllegalArgumentException("Object reference not set to an instance of an object.");  // note 8
            transporterSupCustId = transporterParty(u, transporterId, i(dto.TransporterSupCustId));
        }
        double carriage = toDouble(dto.CarriageAmount == null ? null : dto.CarriageAmount.trim());
        if (carriage > 0d && transporterId == 0) throw new IllegalArgumentException("Transporter Account field required");

        /* InvGrn — non-virtual properties in model order (Model 1026). */
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("IsApproved", false);
        head.put("AddWages", false);
        head.put("ScaleShortWeightApply", false);
        head.put("SupplierShortWeightApply", false);
        head.put("DocDate", docDate);
        head.put("EntryDate", today);
        head.put("ModifyDate", today);
        head.put("PostDate", null);
        head.put("CarriageAmount", carriage);
        head.put("FreightDeduction", 0d);
        head.put("BiltyFreight", 0d);
        head.put("AdvanceByFactoryFreight", 0d);
        head.put("AdvanceByPartyFreight", 0d);
        head.put("AccessWeight", 0d);
        head.put("FactoryWeight", 0d);
        head.put("OtherCharges", 0d);                                                                         // :1134
        head.put("PartyWeight", 0d);
        head.put("ScaleKart", 0d);
        head.put("BranchesId", branch);
        head.put("CompanyId", u.getCompanyId());
        head.put("FinancialYearId", fy);
        head.put("DocNo", docNo);
        head.put("SupplierDispatchId", 0);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("BaseDocumentTypeId", baseType);
        head.put("EntryUser", u.getId());
        head.put("GpNo", toInt(dto.GpNo));                                                                    // :1113
        head.put("Id", recId);
        head.put("InwardGatePassId", gpId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PostUser", 0);
        head.put("ProjectsId", branch);                                                                       // :1099
        head.put("ReferencePartyId", 0);
        head.put("StockPartyId", 0);
        head.put("SupplierCustomerId", supplierId);
        head.put("TransporterId", transporterId);
        head.put("TransporterSupCustId", transporterSupCustId);
        head.put("GhallaMandiId", 0);
        head.put("BillCalculateTypeId", 0);
        head.put("GrnTypeId", 0);
        head.put("ReqestedById", 0);
        head.put("ReturnableDate", null);
        head.put("BiltyNo", trim(dto.BiltyNo));
        head.put("ReferenceDocNo", trim(dto.ReferenceDocNo));
        head.put("RemarksHeader", trim(dto.RemarksHeader));
        head.put("SupplierReference", null);
        head.put("TransporterDocRef", "");                                                                     // :1117
        head.put("VehicleNo", trim(dto.VehicleNo));
        head.put("ActionId", recId == 0 ? 1 : 2);                                                             // BLL Save:66 / :74
        head.put("VehicleType", dto.VehicleType == null ? "" : dto.VehicleType);
        head.put("DeliveryTerm", null);
        head.put("Status", null);
        head.put("TransType", null);
        head.put("ScreenName", SCREEN_NAME);                                                                  // :1136
        head.put("AttachmentsValues", stored != null ? nz(ci(stored, "AttachmentsValues")) : "");             // D3
        head.put("CustomAttachmentsValues", stored != null ? nz(ci(stored, "CustomAttachmentsValues")) : "");

        /* :1138-1151 — each row filled, then validated in the desktop's order. */
        List<Map<String, Object>> details = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {
            GrnStoreDto.Row r = rows.get(idx);
            double qty = d(r.ThisQty);
            Map<String, Object> pd = new LinkedHashMap<>();                     // InvGrnDetail, Model 1027 order
            pd.put("AdLsWeight", 0d);
            pd.put("EBWPerUnit", 0d);
            pd.put("EBWTotal", 0d);
            pd.put("EbPurAgainstWeight", 0d);
            pd.put("GrossWeight", qty);                                         // :1312
            pd.put("SupplierQty", 0d);
            pd.put("ItemQty", qty);
            pd.put("NetBillWeight", qty);                                       // :1313
            pd.put("StockWeight", qty);                                         // :1314
            pd.put("WtCut", 0d);
            pd.put("WtCutTotal", 0d);
            pd.put("ScaleKart", 0d);
            pd.put("ScaleShortWeight", 0d);
            pd.put("SupplierShortWeight", 0d);
            pd.put("FreightAmount", 0d);
            pd.put("StockEbUnit", 0d);
            pd.put("StockEbTotal", 0d);
            pd.put("Id", i(r.Id));                                              // :1282 (overrides :1142)
            pd.put("InvGrnId", 0);
            pd.put("ItemId", i(r.ItemId));
            pd.put("ItemUomId", i(r.UOMId));
            pd.put("JobLotId", 0);
            pd.put("PackingTypeId", 0);
            pd.put("RefDocumentTypeId", 0);
            pd.put("RefDocNoId", 0);
            pd.put("RefDocSubIdNo", 0);
            pd.put("PurchaseOrderId", i(r.OrderId));
            pd.put("PurchaseOrderDetailId", i(r.OrderDetailId));
            pd.put("PurchaserOrderNo", 0);                                      // the form sets PurchaseOrder (virtual)
            pd.put("SupplySchedulId", 0);
            pd.put("GatePassInwarDetailId", 0);
            pd.put("WarehouseId", i(r.WarehouseId));
            pd.put("WareHouseFromId", 0);
            pd.put("InvPurchasedemondId", i(r.DemandId));
            pd.put("PurchaseDemondDetailId", i(r.DemandDetailId));
            pd.put("AreaCity", null);
            pd.put("CommentsDetail", r.Remarks == null ? "" : r.Remarks);
            pd.put("PackingDate", null);
            pd.put("ExpiryDate", null);
            pd.put("CropYearId", 0);
            pd.put("CropYear", null);
            pd.put("LabReportRef", null);
            pd.put("ContractorId", 0);
            pd.put("CityId", 0);
            pd.put("WbTicketId", 0);
            pd.put("LineId", 0);
            pd.put("WeightCutOnId", 0);
            pd.put("LabId", 0);
            pd.put("QtyForWtCut", 0d);
            pd.put("GdnId", 0);
            pd.put("GdnDetailId", 0);
            pd.put("GdnDocumentTypeId", 0);
            pd.put("AssetId", 0);
            pd.put("ConditionId", 0);
            pd.put("DeliveryChallanId", i(r.DeliveryChallanId));
            pd.put("DeliveryChallanDetailId", i(r.DeliveryChallanDetailId));
            pd.put("ItemConditionId", i(r.ItemConditionId));
            pd.put("RackId", i(r.RackId));

            validate(pd.get("WarehouseId"), "WareHouseName", idx);             // :1144-1149
            validate(pd.get("ItemId"), "Item", idx);
            validate(pd.get("ItemUomId"), "Uom", idx);
            validate(pd.get("ItemConditionId"), "ItemCondition", idx);
            validate(pd.get("RackId"), "Rack Name", idx);
            validate(pd.get("ItemQty"), "This Qty", idx);
            details.add(pd);
        }

        /* BLL Save:60 */
        if (recId == 0) {
            for (Map<String, Object> pd : details) {
                if (toInt(pd.get("Id")) > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            }
        } else {
            repo.recordDate(recId);                                             // GetRecordsById → PreviousDate (virtual)
        }

        /* T1-T6 — web tenancy checks; rewrites head's base type / gate pass and the rows' reference ids. */
        enforceTenancy(u, recId, stored, dto, rows, details, head, subsidiary);
        int gatePass = toInt(head.get("InwardGatePassId"));

        /* DAL 0429 SetData. */
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");
        int num = repo.setProc(recId == 0 ? "Sp_InvGrn_Insert" : "Sp_InvGrn_Update", head);
        if (num > 0) head.put("Id", num); else num = recId;
        if (num <= 0) throw new IllegalStateException("Save returned no document id.");
        int line = 1;
        for (Map<String, Object> pd : details) {
            pd.put("LineId", line++);
            pd.put("InvGrnId", num);
            pd.put("Id", repo.setProc("Sp_InvGrnDetail_Insert", pd));
        }
        repo.inventoryTransactions(u, DOCUMENT_TYPE_ID, num);
        if (gatePass > 0) repo.stockEvaluation(u, DOCUMENT_TYPE_ID, num);             // DAL :215
        repo.stockInTransitAndVoucher(u, DOCUMENT_TYPE_ID, num);

        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", (recId == 0 ? "Record Save Successfully [" : "Record Update Successfully [") + docNo + "]");
        m.put("id", num);
        return m;
    }

    // ============================================================================== tenancy

    /** T1-T6 (see the class Javadoc). Throws IllegalArgumentException (400) on the first failure. */
    private void enforceTenancy(UserAccount u, int recId, Map<String, Object> stored, GrnStoreDto dto,
                                List<GrnStoreDto.Row> rows, List<Map<String, Object>> details,
                                Map<String, Object> head, boolean subsidiary) {
        int fy = ctx.currentFinancialYearId();
        int branch = branch(u);
        int postedBase = i(dto.BaseDocumentTypeId);
        int postedGp = i(dto.InwardGatePassId);

        /* T1 */
        int base = postedBase;
        int storedGp = 0;
        if (stored != null) {
            int sb = toInt(ci(stored, "BaseDocumentTypeId"));
            if (sb != 0) {
                if (postedBase != sb) throw new IllegalArgumentException("The source document type of this GRN cannot change on update.");
                base = sb;
            }
            storedGp = toInt(ci(stored, "InwardGatePassId"));
        }
        if (base < 1 || base > 3) throw new IllegalArgumentException("Rows can only come from a Purchase Order, Purchase Demand or Delivery Challan load.");

        Map<Integer, Map<String, Object>> storedLines = new HashMap<>();
        if (stored != null) for (Map<String, Object> d : repo.storeDetails(recId)) storedLines.put(toInt(d.get("Id")), d);

        List<Map<String, Object>> racks = store.racksWithWarehouseAndItem(u, branch);
        Set<Integer> conditions = new HashSet<>();
        for (Map<String, Object> c : store.itemConditions()) conditions.add(toInt(c.get("Id")));

        List<Map<String, Object>> pending = null;
        Set<Integer> postedLineIds = new HashSet<>();
        Set<Integer> seenKeys = new HashSet<>();
        Set<Integer> newGatePasses = new HashSet<>();
        Integer docHeader = null;          // PO / Delivery Challan: the single source document
        Integer newParty = null;           // PO / Delivery Challan: its party
        for (Map<String, Object> s : storedLines.values()) {
            if (base == 1) docHeader = toInt(s.get("PurchaseOrderId"));
            if (base == 3) docHeader = toInt(s.get("DeliveryChallanId"));
            if (base == 1 || base == 3) seenKeys.add(toInt(s.get(base == 1 ? "PurchaseOrderDetailId" : "DeliveryChallanDetailId")));
            if (base == 2) seenKeys.add(toInt(s.get("PurchaseDemondDetailId")));
        }

        for (int idx = 0; idx < rows.size(); idx++) {
            GrnStoreDto.Row r = rows.get(idx);
            Map<String, Object> pd = details.get(idx);
            int rowNo = idx + 1;
            int rowId = i(r.Id);
            if (rowId > 0 && stored != null) {                                    /* T6 */
                Map<String, Object> s = storedLines.get(rowId);
                if (s == null) throw new IllegalArgumentException("Row " + rowNo + " is not a line of this document.");
                postedLineIds.add(rowId);
                for (String k : new String[] { "PurchaseOrderId", "PurchaseOrderDetailId", "InvPurchasedemondId",
                        "PurchaseDemondDetailId", "DeliveryChallanId", "DeliveryChallanDetailId", "ItemId", "ItemUomId" }) {
                    pd.put(k, toInt(s.get(k)));
                }
            } else if (rowId == 0) {                                              /* T2 */
                if (pending == null) pending = pendingFor(u, base, fy, branch);
                String keyCol = base == 1 ? "OrderDetailId" : "DetailId";
                int key = base == 1 ? i(r.OrderDetailId) : base == 2 ? i(r.DemandDetailId) : i(r.DeliveryChallanDetailId);
                Map<String, Object> src = null;
                for (Map<String, Object> p : pending) if (key != 0 && toInt(p.get(keyCol)) == key) { src = p; break; }
                if (src == null) throw new IllegalArgumentException("Row " + rowNo + ": the referenced document line is not pending for this company.");
                int header = base == 3 ? toInt(src.get("DeliveryChallanHeaderId")) : toInt(src.get("Id"));
                int postedHeader = base == 1 ? i(r.OrderId) : base == 2 ? i(r.DemandId) : i(r.DeliveryChallanId);
                if (header != postedHeader) throw new IllegalArgumentException("Row " + rowNo + ": the line does not belong to the posted document.");
                if (!seenKeys.add(key)) throw new IllegalArgumentException("Row " + rowNo + ": the same document line is loaded twice.");
                if (base == 1 || base == 3) {
                    if (docHeader != null && docHeader != 0 && docHeader != header)
                        throw new IllegalArgumentException("Row " + rowNo + ": rows must come from one " + (base == 1 ? "Purchase Order." : "Delivery Challan."));
                    docHeader = header;
                    newParty = toInt(src.get(base == 1 ? "SupplierCustomerId" : "BillToPartyId"));
                }
                int srcGp = toInt(src.get("GatePassId"));
                if (srcGp > 0) newGatePasses.add(srcGp);
                if (base == 1) {
                    pd.put("PurchaseOrderId", header);
                    pd.put("PurchaseOrderDetailId", key);
                    pd.put("InvPurchasedemondId", 0);
                    pd.put("PurchaseDemondDetailId", 0);
                    pd.put("DeliveryChallanId", 0);
                    pd.put("DeliveryChallanDetailId", 0);
                    pd.put("ItemUomId", toInt(src.get("ItemUOMId")));
                } else if (base == 2) {
                    pd.put("PurchaseOrderId", 0);
                    pd.put("PurchaseOrderDetailId", 0);
                    pd.put("InvPurchasedemondId", header);
                    pd.put("PurchaseDemondDetailId", key);
                    pd.put("DeliveryChallanId", 0);
                    pd.put("DeliveryChallanDetailId", 0);
                    pd.put("ItemUomId", toInt(src.get("ItemSchuomId")));
                } else {
                    pd.put("PurchaseOrderId", toInt(src.get("PurchaseOrderDetailId")));      // note 5 — swapped as the desktop does
                    pd.put("PurchaseOrderDetailId", toInt(src.get("PurchaseOrderHeaderId")));
                    pd.put("InvPurchasedemondId", toInt(src.get("PurchaseDemandHeaderId")));
                    pd.put("PurchaseDemondDetailId", toInt(src.get("PurchaseDemandDetailId")));
                    pd.put("DeliveryChallanId", header);
                    pd.put("DeliveryChallanDetailId", key);
                    pd.put("ItemUomId", toInt(src.get("UomId")));
                }
                pd.put("ItemId", toInt(src.get("ItemId")));
            }
            /* T5 */
            int itemId = toInt(pd.get("ItemId")), whId = toInt(pd.get("WarehouseId")), rackId = toInt(pd.get("RackId"));
            boolean rackOk = false;
            for (Map<String, Object> k : racks) {
                if (toInt(k.get("ItemId")) == itemId && toInt(k.get("WarehouseId")) == whId && toInt(k.get("Id")) == rackId) { rackOk = true; break; }
            }
            if (!rackOk) throw new IllegalArgumentException("Row " + rowNo + ": the warehouse and rack are not valid for this item.");
            if (!conditions.contains(toInt(pd.get("ItemConditionId")))) throw new IllegalArgumentException("Row " + rowNo + ": the item condition is not valid.");
        }
        if (base == 2 && postedGp > 0) {
            /* Demand with a gate pass: one demand only (frmPendingPurchaseDemand :523). */
            Integer demand = null;
            for (Map<String, Object> pd : details) {
                int dId = toInt(pd.get("InvPurchasedemondId"));
                if (demand != null && demand != dId) throw new IllegalArgumentException("Rows with a gate pass must come from one Purchase Demand.");
                demand = dId;
            }
        }

        /* T3 */
        int gp;
        if (stored == null) {
            if (postedGp != 0 && !newGatePasses.contains(postedGp)) throw new IllegalArgumentException("The gate pass does not belong to the loaded document.");
            gp = postedGp;
        } else if (postedGp == storedGp) {
            gp = storedGp;
            if (storedGp != 0) for (Integer g : newGatePasses) if (g != storedGp) throw new IllegalArgumentException("The gate pass does not belong to the loaded document.");
        } else if (storedGp == 0 && newGatePasses.contains(postedGp)) {
            gp = postedGp;
        } else {
            throw new IllegalArgumentException("The gate pass does not belong to the loaded document.");
        }
        if (base == 3 && gp == 0) throw new IllegalArgumentException("No Record found against gp no");

        /* T4 */
        int supplierId = toInt(head.get("SupplierCustomerId"));
        if (base == 2) {
            boolean found = false;
            for (Map<String, Object> s : repo.suppliers(u)) if (toInt(s.get("Id")) == supplierId) { found = true; break; }
            if (!found) throw new IllegalArgumentException("Supplier is not valid for this company.");
        } else {
            int expected = newParty != null ? newParty : (stored != null ? toInt(ci(stored, "SupplierCustomerId")) : 0);
            if (expected != supplierId) throw new IllegalArgumentException("Supplier must be the party of the loaded document.");
        }
        int transporterId = toInt(head.get("TransporterId"));
        if (transporterId != 0) {
            boolean found = false;
            for (Map<String, Object> t : transporters(u, subsidiary)) if (toInt(t.get("Id")) == transporterId) { found = true; break; }
            if (!found) throw new IllegalArgumentException("Transporter is not valid for this company.");
        }

        /* T6 — a stored line in a live 703 dispatch note must be re-posted with its own Id. */
        if (stored != null) {
            for (Integer line : repo.grnLinesInPurchaseReturn(u, recId)) {
                if (line != null && storedLines.containsKey(line) && !postedLineIds.contains(line)) {
                    throw new IllegalArgumentException("Can not Update this Grn,Because Record Against This Grn already exist in [Goods Dispatched Notes For Purchase Return]");
                }
            }
        }

        head.put("BaseDocumentTypeId", base);
        head.put("InwardGatePassId", gp);
    }

    /** The matching pending loader, unfiltered by date / number / party / item. */
    private List<Map<String, Object>> pendingFor(UserAccount u, int base, int fy, int branch) {
        switch (base) {
            case 1:  return repo.pendingPurchaseOrders(u, PO_DOCUMENT_TYPE_ID, fy, branch, 0, null, null, 0, 0);
            case 2:  return repo.pendingPurchaseDemands(u, branch, fy, DEMAND_DOCUMENT_TYPE_ID, null, null, 0, 0, 0);
            default: return repo.pendingDeliveryChallans(u, branch, fy, CHALLAN_DOCUMENT_TYPE_ID, null, null, 0, 0, 0, 0);
        }
    }

    // ========================================================================== loader slips

    /** frmPendingPurchaseOrderStoreLoader.grd_LinkClicked:339 → PurchaseOrderSlipReport201. */
    public List<Map<String, Object>> loaderPurchaseOrderSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        return plain(repo.purchaseOrderGeneralSlip(u, id));
    }

    /** frmPendingPurchaseDemand.grd_LinkClicked:324 → PurchaseDemandSlip454. */
    public List<Map<String, Object>> loaderPurchaseDemandSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        return plain(repo.purchaseDemandSlip(u, id));
    }

    /** frmPendingDeliveryChallanLoader.grd_LinkClicked:334 → StoreDeliveryChallanSlip148. */
    public List<Map<String, Object>> loaderDeliveryChallanSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        return plain(repo.deliveryChallanSlip(u, branch(u), ctx.currentFinancialYearId(), id));
    }

    // ================================================================================= delete

    /** btnDelete_Click:1028 → InvPurchaseInvoice.RemoveByID. */
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0 || ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
        store.removeInvoiceVoucherAndStock(u, DOCUMENT_TYPE_ID, id, u.getId());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Delete Record Successfully");
        return m;
    }

    // ================================================================================ helpers

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    /** SelectedRow.Cells[3] — the posted pair when it exists, else the first row carrying that account. */
    private int transporterParty(UserAccount u, int glAccountId, int postedParty) {
        Integer first = null;
        for (Map<String, Object> r : repo.vendorsAndCustomersForTransporter(u)) {
            if (toInt(r.get("GlAccountId")) != glAccountId) continue;
            int party = toInt(r.get("Id"));
            if (party == postedParty) return party;
            if (first == null) first = party;
        }
        return first == null ? 0 : first;
    }

    private static List<Map<String, Object>> activity(List<Map<String, Object>> rows, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!name.equals(str(r.get("Activity")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            out.add(o);
        }
        return out;
    }

    private static final DateTimeFormatter TS = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    /** D5 — dates as local text. */
    static Object text(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(TS);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(TS);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().format(TS);
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).format(TS);
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay().format(TS);
        if (v instanceof java.time.OffsetDateTime) return ((java.time.OffsetDateTime) v).toLocalDateTime().format(TS);
        return v;
    }

    /** Procedure rows passed through to the page, keys kept case-insensitive, dates as text. */
    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
            for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), text(e.getValue()));
            out.add(o);
        }
        return out;
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == yearId) {
                    Object v = ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(text(v)).substring(0, 10);
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the financial year start", e);
        }
        return null;
    }

    /** FormHelper.ValidateField:503 — 0, non-positive and blank are "required". */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    private static void put(Map<String, Object> p, String k, Object v) { if (v != null) p.put(k, v); }
    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }
    private static String trim(String s) { return s == null ? "" : s.trim(); }
    private static Object nz(Object v) { return v == null ? null : String.valueOf(v); }
}
