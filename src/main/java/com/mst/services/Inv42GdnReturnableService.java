package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv42Support.day;
import static com.mst.services.Inv42Support.dateTime;
import static com.mst.services.Inv42Support.list;
import static com.mst.services.Inv42Support.str;
import static com.mst.services.Inv42Support.toDouble;
import static com.mst.services.Inv42Support.toInt;
import static com.mst.services.Inv42Support.truthy;

/**
 * Inventory hub, module 42 — screen 927 "Issuance Gdn", desktop
 * {@code Architecture.WinApp.ReturnableInventoryManagement.GdnReturnable} (base.Name "GdnReturnable"), DocumentTypeId 1202.
 * Line refs ":NNN" are GdnReturnable.cs.
 *
 * <pre>
 *   InitializeComponentMethod :330   rights, feature 4, InvGdnGenerateCode(1202), GetDataForDropDownFromGdn("1202"),
 *                                    StaticColumnsService("DeliveryTypeForReturnAble"), ReferenceParties.GetAll, VehicleType.GetAll,
 *                                    GetLookupsByTypeIdDt(13), OutwardGatePass_OutstandingForReturnableGdn, globals
 *   RowbindFromPendingGpGrid :823    USP_DeliveryOrder_PendingDataForGdn_Engr (DocumentTypeId 1200, Id = DO, ActionId 1)
 *   ShowDeliveryOrderDialog :1858    frmLoadDeliveryOrderOnGdnReturnable (source not recovered) → the same procedure without Id
 *   Insert :983                      InvGdn.Save → Sp_InvGdn_Insert|Update, Sp_InvGdnDetail_Insert, stock posting
 *   ReadById :1154                   Sp_InvGdn_GetAllMethod 'GetById' + 'GetGDNDetailByGdnId'
 *   btnDelete_Click :1228            InvPurchaseInvoice.RemoveByID → USP_RecoredRemoveByOrgCompDocAndByID
 *   HistoryFill :1340                InvGdn.FormHistoryForStoreIssuanceGdn → USP_IssuanceGDNFormHistory
 *   Print_Click :1264                GdnReturnable_Slip_1203 → 1203_GdnReturnable_Slip.rpt (existing print route)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED: ProjectsId = BranchesId (:1003); IsApproved false even on Update (:1014); every detail's
 * GpDate is "now" and LineId the row position (:1056); GrossWeight / NetBillWeight / StockWeight = ItemQty (:1093);
 * the transporter is only sent with a Freight amount (:1029); the detail validation messages, the second one with the
 * literal "{r.RowIndex + 1}" (:1079, the desktop string is not interpolated).
 *
 * DEVIATIONS: Doc No from the generator / stored header, never the request; tenancy on open/update/delete; the DAL's
 * FIFO (feature 5) stock-evaluation branch is not ported (Sp_InventoryStockEvalautionDetail_Update is run when feature 5
 * is off, nothing when on — see REPORT.md); attachments not ported (stored AttachmentsValues kept).
 */
@Service
public class Inv42GdnReturnableService {

    public static final String SCREEN = "GdnReturnable";
    public static final int DOC_TYPE = 1202;
    public static final int DO_DOC_TYPE = 1200;

    private final Inv42Support db;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public Inv42GdnReturnableService(Inv42Support db, StoreScreenRights rights, CurrentUserContext ctx) {
        this.db = db;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================== load

    /** InitializeComponentMethod :330-395. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("feature4", db.feature(u, 4));                                                                   // :335
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(db.config(u, "DefaultDaysToLessFromHistoryFromDate"))); // :369
        out.put("docNo", docNo(u));
        out.putAll(refresh(u));
        out.put("warehouses", db.warehouses(u));                                                                // :665 grid combo
        out.put("pendingGatePasses", pendingGatePasses(u));                                                     // :756
        out.put("historyCombos", historyCombos(u));                                                             // :398
        out.put("financialYearStart", db.financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    /** btnRefresh_Click :951. */
    public Map<String, Object> refresh() { return refresh(ctx.requireAccountingUser()); }

    private Map<String, Object> refresh(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("deliveryTypes", db.staticColumns("DeliveryTypeForReturnAble"));   // :455
        out.put("customers", db.customers(u));                                       // :471
        out.put("referenceParties", db.referenceParties(u));                         // :530
        out.put("requestedBy", db.lookups13(u));                                     // :554
        out.put("transporters", db.transporters(u, db.feature(u, 4)));               // :566
        out.put("vehicleTypes", db.vehicleTypes());                                  // :542
        return out;
    }

    /** CommonServices.InvGdnGenerateCode(1202) → Sp_InvGdn_GetAllMethod 'GenerateInvGdnCode'. */
    public int docNo() { return docNo(ctx.requireAccountingUser()); }

    private int docNo(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Activity", "GenerateInvGdnCode");
        if (ctx.currentFinancialYearId() != 0) p.put("FinancialYearId", ctx.currentFinancialYearId());
        if (toInt(u.getBranchesId()) != 0) p.put("BranchesId", u.getBranchesId());
        List<Map<String, Object>> r = db.rows("Sp_InvGdn_GetAllMethod", p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** PendingGatePassDbCall :756 — GatePassOutward.OutwardGatePass_OutstandingForReturnableGdn; dtGp columns :719-735. */
    public List<Map<String, Object>> pendingGatePasses() { return pendingGatePasses(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> pendingGatePasses(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_OutwardGatePass_OutstandingForReturnableGdn", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId()))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("OrderType", str(r.get("OrderType")));
            o.put("GatepassType", str(r.get("GatepassType")));
            o.put("OutwardGatePassId", toInt(r.get("OutwardGatePassId")));
            o.put("DeliveryTypeId", toInt(r.get("DeliveryTypeId")));
            o.put("DeliveryType", str(r.get("DeliveryType")));
            o.put("DeliverOrderId", toInt(r.get("DeliverOrderId")));
            o.put("SupplierCustomerId", toInt(r.get("SupplierCustomerId")));
            o.put("CustomerName", str(r.get("CustomerName")));
            o.put("OrderNo", str(r.get("OrderNo")));
            o.put("GpDate", day(r.get("GpDate")));
            o.put("GpSrNo", str(r.get("GpSrNo")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("BiltyNo", str(r.get("BiltyNo")));
            o.put("Qty", toDouble(r.get("Qty")));
            o.put("Freight", toDouble(r.get("Freight")));
            o.put("Status", str(r.get("Status")));
            o.put("RequestedBy", str(r.get("RequestedBy")));
            o.put("ApprovedBy", str(r.get("ApprovedBy")));
            out.add(o);
        }
        return out;
    }

    /** HistoryComboDbCall :398 — USP_GetDataForDropDownFromGdn ("1202") split into Supplier / RequestedBy (:424). */
    public Map<String, List<Map<String, Object>>> historyCombos() { return historyCombos(ctx.requireAccountingUser()); }

    private Map<String, List<Map<String, Object>>> historyCombos(UserAccount u) {
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        by.put("Supplier", new ArrayList<>());
        by.put("RequestedBy", new ArrayList<>());
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", String.valueOf(DOC_TYPE));
        if (toInt(u.getBranchesId()) != 0) p.put("BranchesIds", String.valueOf(u.getBranchesId()));
        for (Map<String, Object> r : db.rows("USP_GetDataForDropDownFromGdn", p)) {
            List<Map<String, Object>> t = by.get(str(r.get("Activity")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        return by;
    }

    /**
     * RowbindFromPendingGpGrid :874 (deliveryOrderId > 0, ActionId 1 = include those already on a gate pass) and the
     * Load Delivery Order dialog (no Id, ActionId omitted = only orders without an outward gate pass) →
     * USP_DeliveryOrder_PendingDataForGdn_Engr with DocumentTypeId 1200.
     */
    public List<Map<String, Object>> pendingDeliveryOrderRows(int customerId, int deliveryTypeId, int deliveryOrderId, boolean fromGatePass) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId(), "FinancialYearId", ctx.currentFinancialYearId(), "DocumentTypeId", DO_DOC_TYPE);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (deliveryTypeId != 0) p.put("DeliveryTypeId", deliveryTypeId);
        if (deliveryOrderId != 0) p.put("Id", deliveryOrderId);
        if (fromGatePass) p.put("ActionId", 1);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_DeliveryOrder_PendingDataForGdn_Engr", p)) {
            Map<String, Object> o = new LinkedHashMap<>();       // the columns LoadInGridDetail :1820 reads
            o.put("Id", toInt(r.get("Id")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("DeliveryTypeId", toInt(r.get("DeliveryTypeId")));
            o.put("DeliveryOrderType", str(r.get("DeliveryOrderType")));
            o.put("SupplierCustomerId", toInt(r.get("SupplierCustomerId")));
            o.put("DetailId", toInt(r.get("DetailId")));
            o.put("WarehouseId", toInt(r.get("WarehouseId")));
            o.put("WareHouseName", str(r.get("WareHouseName")));
            o.put("ItemCategoryId", toInt(r.get("ItemCategoryId")));
            o.put("ItemCategory", str(r.get("ItemCategory")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("ItemCode", str(r.get("ItemCode")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("PackUomId", toInt(r.get("PackUomId")));
            o.put("PackUomCode", str(r.get("PackUomCode")));
            o.put("ItemDescription", str(r.get("ItemDescription")));
            o.put("Qty", toDouble(r.get("Qty")));
            o.put("IsAssetItem", truthy(r.get("IsAssetItem")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    /** HistoryFill :1340 — USP_IssuanceGDNFormHistory; the checked date pair, doc numbers, customer, requested by, IsRefered. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId(), "DocumentTypeId", DOC_TYPE);
        String dateType = str(f.get("dateType"));
        Timestamp from = Inv42Support.ts(f.get("fromDate")), to = Inv42Support.ts(f.get("toDate"));
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        if (toInt(f.get("fromDocNo")) != 0) p.put("FromDocNo", toInt(f.get("fromDocNo")));
        if (toInt(f.get("toDocNo")) != 0) p.put("ToDocNo", toInt(f.get("toDocNo")));
        if (toInt(f.get("supplierCustomerId")) != 0) p.put("SupplierCustomerId", toInt(f.get("supplierCustomerId")));
        if (toInt(f.get("requestedById")) != 0) p.put("RequestedById", toInt(f.get("requestedById")));
        p.put("CanViewAllRecord", viewAll);
        p.put("EntryUserId", u.getId());                                                 // :1401 always set
        if (toInt(f.get("isRefered")) != 0) p.put("IsRefered", toInt(f.get("isRefered")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_IssuanceGDNFormHistory", p)) {
            Map<String, Object> o = new LinkedHashMap<>();      // dtcol :1414-1431
            o.put("Id", toInt(r.get("Id")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("Type", str(r.get("DeliveryType")));
            o.put("Customer", str(r.get("CustomerName")));
            o.put("OrderNo", toInt(r.get("SaleOrderNo")));
            o.put("InvoiceNo", toInt(r.get("InvoiceNo")));
            o.put("RequestedBy", str(r.get("RequestedBy")));
            o.put("ManualNo", str(r.get("ReferenceDocNo")));
            o.put("EntryUser", str(r.get("UserName")));
            o.put("EntryDate", dateTime(r.get("EntryDate")));
            o.put("ModifyUser", str(r.get("ModifyUserName")));
            o.put("ModifyDate", dateTime(r.get("ModifyDate")));
            o.put("ApprovedUser", str(r.get("ApprovedUserName")));
            o.put("ApprovedDate", dateTime(r.get("ApprovedDate")));
            o.put("Remarks", str(r.get("RemarksHeader")));
            o.put("GdnStatus", str(r.get("GdnStatus")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById :1154 / DataGridHistory_SelectionChanged :1543 — InvGdn.GetByID. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();        // table.Rows.Add :1215 + the history detail cells :1572
            o.put("Id", toInt(d.get("Id")));
            o.put("DeliveryOrderId", toInt(d.get("InvDeliveryOrderId")));
            o.put("DeliveryOrderDetailId", toInt(d.get("InvDeliveryOrderDetailId")));
            o.put("DeliveryOrderNo", toInt(d.get("DeliveryScheduleNo")));
            o.put("WareHouseId", toInt(d.get("WarehouseId")));
            o.put("WareHouse", str(d.get("WareHouseCode")));
            o.put("ItemCategoryId", toInt(d.get("ItemCategoryId")));
            o.put("ItemCategory", str(d.get("ItemCategory")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("ItemName", str(d.get("Item")));
            o.put("PackUomId", toInt(d.get("ItemUomId")));
            o.put("PackUom", str(d.get("UOMCode")));
            o.put("ItemDescription", str(d.get("ItemDescription")));
            o.put("ItemQty", toDouble(d.get("ItemQty")));
            o.put("IsAssetItem", truthy(d.get("IsAssetItem")));
            o.put("DeliveryTypeId", toInt(d.get("DeliveryTypeId")));
            o.put("CommentsDetail", str(d.get("CommentsDetail")));
            o.put("JobLot", str(d.get("JobLot")));
            o.put("AssetName", str(d.get("AssetName")));
            o.put("WarehouseTo", str(d.get("WareHouseToCode")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(h.get("Id")));
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("DocDate", day(h.get("DocDate")));
        out.put("SupplierCustomerId", toInt(h.get("SupplierCustomerId")));
        out.put("ReferencePartyId", toInt(h.get("ReferencePartyId")));
        out.put("VehicleTypeId", toInt(h.get("VehicleTypeId")));
        out.put("VehicleNo", str(h.get("VehicleNo")));
        out.put("BiltyNo", str(h.get("BiltyNo")));
        out.put("RemarksHeader", str(h.get("RemarksHeader")));
        out.put("ReturnableDate", day(h.get("ReturnableDate")));
        out.put("ReferenceDocNo", str(h.get("ReferenceDocNo")));
        out.put("GpNo", toInt(h.get("GpNo")));
        out.put("OutwardGatePassId", toInt(h.get("OutwardGatePassId")));
        out.put("RequestedByLookUpId", toInt(h.get("RequestedByLookUpId")));
        out.put("TransporterId", toInt(h.get("TransporterId")));
        out.put("TransporterSupCustId", toInt(h.get("TransporterSupCustId")));
        out.put("CarriageAmount", toDouble(h.get("CarriageAmount")));
        out.put("rows", rows);
        return out;
    }

    private List<Map<String, Object>> details(int id) {
        return db.rows("Sp_InvGdn_GetAllMethod", params("InvGdnMainId", id, "Activity", "GetGDNDetailByGdnId"));
    }

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> r = db.rows("Sp_InvGdn_GetAllMethod", params("Id", id, "Activity", "GetById"));
        if (r.size() != 1) return null;
        Map<String, Object> h = r.get(0);
        if (toInt(h.get("DocumentTypeId")) != DOC_TYPE) return null;
        if (toInt(h.get("CompanyId")) != toInt(u.getCompanyId())) return null;
        if (toInt(h.get("OrganizationId")) != toInt(u.getOrganizationId())) return null;
        return h;
    }

    // ==================================================================================== save

    /** Insert :983 — "Grid record not found", FormValidation :613, the InvGdn object, InvGdn.Save. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = toInt(b.get("Id"));
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");                   // :993
        Map<String, Object> existing = null;
        if (recId > 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        int docNo = existing != null ? toInt(existing.get("DocNo")) : docNo(u);
        formValidation(b, docNo);
        boolean feature4 = db.feature(u, 4);
        double freight = toDouble(b.get("FreightAmount"));

        Map<String, Object> h = new LinkedHashMap<>();                                                     // :999-1028
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", u.getBranchesId());
        h.put("FinancialYearId", ctx.currentFinancialYearId());
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("ScreenName", SCREEN);
        h.put("Id", recId);
        h.put("EntryDate", Inv42Support.now());
        h.put("EntryUser", u.getId());
        h.put("ModifyDate", Inv42Support.now());
        h.put("ModifyUser", u.getId());
        h.put("PostDate", Inv42Support.now());
        h.put("PostUser", u.getId());
        h.put("IsApproved", false);
        h.put("DocNo", docNo);
        Timestamp docDate = Inv42Support.docDate(b.get("DocDate"));
        h.put("DocDate", docDate);
        h.put("SupplierCustomerId", toInt(b.get("SupplierCustomerId")));
        h.put("ReferencePartyId", toInt(b.get("ReferencePartyId")));
        h.put("VehicleTypeId", toInt(b.get("VehicleTypeId")));
        h.put("VehicleType", str(b.get("VehicleType")));
        h.put("VehicleNo", str(b.get("VehicleNo")));
        h.put("BiltyNo", str(b.get("BiltyNo")));
        h.put("RemarksHeader", str(b.get("RemarksHeader")));
        Timestamp ret = Inv42Support.ts(b.get("ReturnableDate"));
        if (ret != null) h.put("ReturnableDate", ret);
        h.put("ReferenceDocNo", str(b.get("ReferenceDocNo")).trim());
        h.put("OutwardGatePassId", toInt(b.get("OutwardGatePassId")));
        h.put("GpNo", toInt(b.get("GpNo")));
        h.put("RequestedByLookUpId", toInt(b.get("RequestedByLookUpId")));
        h.put("TransporterId", 0);
        h.put("TransporterSupCustId", 0);
        h.put("CarriageAmount", 0d);
        if (freight > 0d) {                                                                               // :1029-1049
            if (toInt(b.get("TransporterId")) <= 0) throw new IllegalArgumentException("Transporter Account field required");
            h.put("CarriageAmount", freight);
            if (!feature4) {
                h.put("TransporterId", toInt(b.get("TransporterId")));
            } else {
                h.put("TransporterId", toInt(b.get("TransporterId")));                  // the GL account behind the party
                h.put("TransporterSupCustId", toInt(b.get("TransporterSupCustId")));
            }
        }
        h.put("ActionId", recId > 0 ? 2 : 1);
        h.put("AutoUpdateId", 0);
        h.put("IsStockReserved", false);
        h.put("AttachmentsValues", existing == null ? "" : str(existing.get("AttachmentsValues")));
        h.put("CustomAttachmentsValues", existing == null ? "" : str(existing.get("CustomAttachmentsValues")));
        for (String z : new String[] { "StockPartyId", "TransporterDocRef", "SupplierReference", "ContainerNo", "ContainerNo1", "DriverName", "DriverCNIC", "DriverCellNo" }) h.put(z, "");
        for (String z : new String[] { "PartyWeight", "FactoryWeight", "OtherCharges" }) h.put(z, 0d);
        for (String z : new String[] { "BillCalculateTypeId", "AddWages", "ShortWeightDeductionApply", "OrderTypeId", "OrderId", "AdvanceDeliveryOrderId" }) h.put(z, 0);
        h.put("DeliveryTerm", "");
        h.put("GdnStatus", "");
        h.put("StockPartyId", 0);

        int deliveryTypeId = toInt(b.get("DeliveryTypeId"));
        List<Map<String, Object>> details = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {                                                            // :1052-1097
            Map<String, Object> r = rows.get(i);
            int rowNo = i + 1;
            Map<String, Object> d = new LinkedHashMap<>();
            int id = recId > 0 ? toInt(r.get("Id")) : 0;
            d.put("Id", id);
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            d.put("GpDate", Inv42Support.now());
            d.put("LineId", rowNo);
            d.put("DeliveryTypeId", deliveryTypeId);
            d.put("InvDeliveryOrderId", toInt(r.get("DeliveryOrderId")));
            d.put("InvDeliveryOrderDetailId", toInt(r.get("DeliveryOrderDetailId")));
            d.put("DeliveryScheduleId", 0);
            d.put("WarehouseId", toInt(r.get("WareHouseId")));
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("ItemUomId", toInt(r.get("PackUomId")));
            d.put("ItemDescription", str(r.get("ItemDescription")));
            double qty = toDouble(r.get("ItemQty"));
            d.put("ItemQty", qty);
            d.put("IsAssetItem", truthy(r.get("IsAssetItem")));
            if (toInt(r.get("DeliveryOrderId")) == 0) throw new IllegalArgumentException("DeliveryOrderNo Field Required in Detail Grid Row no : " + rowNo);
            if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Required in Detail Grid Row no : {r.RowIndex + 1}");
            if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("ItemName Field Required in Detail Grid Row no : " + rowNo);
            if (toInt(r.get("PackUomId")) == 0) throw new IllegalArgumentException("PackUom Field Required in Detail Grid Row no : " + rowNo);
            if (qty == 0d) throw new IllegalArgumentException("ItemQty Field Required in Detail Grid Row no : " + rowNo);
            d.put("GrossWeight", qty);
            d.put("NetBillWeight", qty);
            d.put("StockWeight", qty);
            d.put("GpNo", toInt(b.get("GpNo")));
            d.put("VehicleNo", str(b.get("VehicleNo")));
            d.put("CommentsDetail", str(r.get("CommentsDetail")));
            for (String z : new String[] { "SaleOrderId", "PackingTypeId", "JobLotId", "SaleOrderDetailId", "RefPartyId", "RefDocumentTypeId",
                    "RefDocIdNo", "RefDocSubIdNo", "CityId", "DepartmentId", "AssetId", "WareHouseToId", "WbTicketId", "RateUomId",
                    "CostCenterId", "ItemVariantId", "CastingTypeId", "OrderTypeId", "ProductionStageId", "CropYearId", "GrnId",
                    "GrnDetailId", "GrnDocumentTypeId", "JobOrderId", "JobOrderDocumentTypeId", "BrandItemId", "ItemConditionId",
                    "RackId", "SecondaryUomId" }) d.put(z, 0);
            for (String z : new String[] { "EBWPerUnit", "EBWTotal", "WtCut", "WtCutTotal", "AdLsWeight", "ShortWeight", "ItemRate",
                    "ItemAmount", "EbUnitStock", "EbTotalStock", "SecondaryUomQty" }) d.put(z, 0d);
            d.put("CropYear", "");
            details.add(d);
        }

        int num = db.setProc(recId == 0 ? "Sp_InvGdn_Insert" : "Sp_InvGdn_Update", h);
        if (num <= 0) num = recId;
        if (num <= 0) throw new RuntimeException("Save returned no document id.");
        for (Map<String, Object> d : details) {
            d.put("InvGdnId", num);
            db.setProc("Sp_InvGdnDetail_Insert", d);
        }
        /* DAL InvGdn.SetData — stock posting, as the web port of the other GDN forms does. */
        db.scalar("Sp_InventoryTransactions_GetALLMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", num));
        if (!db.feature(u, 5)) {
            db.scalar("Sp_InventoryStockEvalautionDetail_Update", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", num));
        }
        for (Map<String, Object> d : details) {
            db.scalar("USP_InventoryValidation", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "DocumentTypeId", DOC_TYPE, "DocDate", docDate, "ItemId", d.get("ItemId"), "ItemConditionId", 0,
                    "WarehouseId", d.get("WarehouseId"), "NetWeight", d.get("StockWeight")));
        }
        String msg = (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo;  // :1106 / :1111
        return Inv42Support.ok(msg, num, docNo);
    }

    /** FormValidation :613. */
    private void formValidation(Map<String, Object> b, int docNo) {
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field Required");
        if (toInt(b.get("DeliveryTypeId")) <= 0) throw new IllegalArgumentException("Delivery Type Field Required");
        if (toInt(b.get("SupplierCustomerId")) <= 0) throw new IllegalArgumentException("Customer Field Required");
        if (toInt(b.get("RequestedByLookUpId")) <= 0) throw new IllegalArgumentException("RequestedBy Field Required");
        double freight = toDouble(b.get("FreightAmount"));
        if (toInt(b.get("TransporterId")) > 0 && !(freight > 0d)) throw new IllegalArgumentException("Freight Amount Field is Required");
        if (freight > 0d && toInt(b.get("TransporterId")) <= 0) throw new IllegalArgumentException("Transporter Field is Required");
    }

    /** btnDelete_Click :1228 — InvPurchaseInvoice.RemoveByID(org, comp, 1202, id, user) → USP_RecoredRemoveByOrgCompDocAndByID. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
        db.scalar("USP_RecoredRemoveByOrgCompDocAndByID", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Id", id, "UserId", u.getId()));
        return Inv42Support.ok("Delete Record Seccessfully", id, null);
    }
}
