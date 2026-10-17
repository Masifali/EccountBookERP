package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv42Support.day;
import static com.mst.services.Inv42Support.list;
import static com.mst.services.Inv42Support.str;
import static com.mst.services.Inv42Support.toDouble;
import static com.mst.services.Inv42Support.toInt;
import static com.mst.services.Inv42Support.truthy;

/**
 * Inventory hub, module 42 "Returnable Inventory Management" — screen 923 "Delivery Order",
 * desktop {@code Architecture.WinApp.ReturnableInventoryManagement.DeliveryOrder_ReturnAble}
 * (base.Name = "DeliveryOrder_ReturnAble" = the rights key), DocumentTypeId 1200, DeliveryOrderType "Local".
 * Line refs ":NNN" are DeliveryOrder_ReturnAble.cs.
 *
 * <pre>
 *   InitializeComponentMethod :356      rights, ERP feature 4, DeliveryOrderGenerateCode(1200), ReferenceParties.GetAll,
 *                                       GetDataForDropDownFromDeliveryOrder("1200"), GetItemCategoryForComboServiceBind("5,8"),
 *                                       StaticColumnsService("DeliveryTypeForReturnAble"), VehicleType.GetAll,
 *                                       globalAllSupplierCustomer, globalWarehousesWithBranches, getGlobalAllItems (7/8),
 *                                       AssetRegister.GetAllAssetFromAssetRegister, GetLookupsByTypeIdDt(13)
 *   DocumentNoDbCall :523               Sp_InvDeliveryOrder_GetAllMethod 'GenerateCode' (DocumentTypeId 1200)
 *   bindRateUomAndItemPackUom :799      Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   Insert :1173                        InvDeliveryOrder.Save → Sp_InvDeliveryOrder_Insert|Update, Sp_InvDeliveryOrderDetail_Insert
 *   ReadById :1359                      Sp_InvDeliveryOrder_GetAllMethod 'ReadById' + 'ReadByIdDetailIdForReturnable'
 *   gridhistoryfill :1839               SP_DeliveryOrderFormHistory (@DeliveryOrderType 'Local')
 *   btnprint_Click :2253                InvDeliveryOrderSlip_1200 → 1200-DeliveryOrderSlipReturnable.rpt (existing print route)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED: the detail rows carry the header's Delivery Type / Customer / Reference Party
 * (:1247-1249); a row removed with "X" is sent with ActionTypeId 3 on Update (:1059-1071); the ReturnableDate is
 * only sent for Delivery Type 4 (:1218); DoTotalQty / GrossWeight / NetWeight are the sum of the grid Qty (:1204-1225);
 * btnDelete is never made visible (:2053, :2068), so the page has no Delete.
 *
 * DEVIATIONS: Doc No is taken from the generator (Save) or the stored header (Update), never from the request;
 * tenancy — a document is only opened/updated when it is a 1200 header of the signed-in company; the stored
 * AttachmentsValues/CustomAttachmentsValues are kept on Update (attachments are not ported).
 */
@Service
public class Inv42DeliveryOrderReturnableService {

    public static final String SCREEN = "DeliveryOrder_ReturnAble";
    public static final int DOC_TYPE = 1200;
    public static final String ORDER_TYPE = "Local";

    private final Inv42Support db;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public Inv42DeliveryOrderReturnableService(Inv42Support db, StoreScreenRights rights, CurrentUserContext ctx) {
        this.db = db;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================== load

    /** InitializeComponentMethod :356-429. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("itemSearchByCode", truthy(db.config(u, "ItemSearchByCode")));                                 // :390
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(db.config(u, "DefaultDaysToLessFromHistoryFromDate"))); // :391
        out.put("docNo", docNo(u));
        out.putAll(refresh(u));
        out.put("historyCombos", historyCombos(u));
        out.put("financialYearStart", db.financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    /** btnRefresh_Click :1415 — every combo again (the globals are re-read). */
    public Map<String, Object> refresh() { return refresh(ctx.requireAccountingUser()); }

    private Map<String, Object> refresh(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("deliveryTypes", db.staticColumns("DeliveryTypeForReturnAble"));        // :366 / :1421
        out.put("vehicleTypes", db.vehicleTypes());                                      // :622
        out.put("customers", db.customers(u));                                           // :551
        out.put("referenceParties", db.referenceParties(u));                             // :590
        out.put("warehouses", db.warehouses(u));                                         // :638
        out.put("itemCategories", itemCategories(u));                                    // :718
        out.put("items", items(u));                                                      // :751
        out.put("assetItems", assetItems(u));                                            // :772
        out.put("lookups13", db.lookups13(u));                                           // :670
        return out;
    }

    /** DocumentNoDbCall :523 — CommonServices.DeliveryOrderGenerateCode(1200). */
    public int docNo() { return docNo(ctx.requireAccountingUser()); }

    private int docNo(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Activity", "GenerateCode");
        if (ctx.currentFinancialYearId() != 0) p.put("FinancialYearId", ctx.currentFinancialYearId());
        if (toInt(u.getBranchesId()) != 0) p.put("BranchesId", u.getBranchesId());
        List<Map<String, Object>> r = db.rows("Sp_InvDeliveryOrder_GetAllMethod", p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** ItemCategoryDbCall :718 — GetItemCategoryForComboServiceBind("5,8") → Sp_ItemCategory_GetAllMethod @Ids='5,8'. */
    private List<Map<String, Object>> itemCategories(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_ItemCategory_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Ids", "5,8", "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();                         // dtCategory :734
            o.put("Id", toInt(r.get("Id")));
            o.put("CategoryDescription", str(r.get("CategoryDescription")));
            o.put("ParentCategoryId", toInt(r.get("InventoryParentCategoriesId")));
            out.add(o);
        }
        return out;
    }

    /** ItemDtsFillFromGlobal :751 — getGlobalAllItems with InventoryParentCategoriesId 7 or 8 (the page filters by category). */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.allItems(u)) {
            int parent = toInt(r.get("InventoryParentCategoriesId"));
            if (parent != 7 && parent != 8) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            o.put("ItemCategoryId", toInt(r.get("ItemCategoryId")));
            out.add(o);
        }
        return out;
    }

    /** GetAssetItems :772 — [asset].[USP_AssetRegister_GetAllMethod] 'GetAllAssetFromAssetRegister' (CategoryId 0 = all). */
    private List<Map<String, Object>> assetItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("[asset].[USP_AssetRegister_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAllAssetFromAssetRegister"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("assetRegisterId", toInt(r.get("assetRegisterId")));
            o.put("assetName", str(r.get("assetName")));
            o.put("assetCode", str(r.get("assetCode")));
            o.put("ItemCategoryId", toInt(r.get("ItemCategoryId")));
            o.put("UomId", toInt(r.get("UomId")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** HistoryComboDbCall :446 — USP_GetDataForDropDownFromDeliveryOrder, DocumentTypeIds "1200", split by Activity (:472). */
    public Map<String, List<Map<String, Object>>> historyCombos() { return historyCombos(ctx.requireAccountingUser()); }

    private Map<String, List<Map<String, Object>>> historyCombos(UserAccount u) {
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        for (String a : new String[] { "Customer", "DeliveryTypeDetail", "RequestedBy", "ApprovedBy" }) by.put(a, new ArrayList<>());
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", String.valueOf(DOC_TYPE));
        if (toInt(u.getBranchesId()) != 0) p.put("BranchesIds", String.valueOf(u.getBranchesId()));
        if (ctx.currentFinancialYearId() != 0) p.put("FinancialYearId", ctx.currentFinancialYearId());
        for (Map<String, Object> r : db.rows("USP_GetDataForDropDownFromDeliveryOrder", p)) {
            List<Map<String, Object>> t = by.get(str(r.get("Activity")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        return by;
    }

    /** bindRateUomAndItemPackUom :799 — dtUomFromGloablUomScheduleByItemId → Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        return Inv42Support.project(db.rows("Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID")), "Id", "UOMCode");
    }

    // ================================================================================= history

    /**
     * gridhistoryfill :1839 — InvDeliveryOrder.FormHistoryNew → SP_DeliveryOrderFormHistory with DocumentTypeId 1200,
     * Activity/DeliveryOrderType "Local", CanViewAllRecord (EntryUser when not), the checked date pair, doc numbers,
     * ActionId (= Delivery Type) → @DeliveryTypeId, Customer, RequestedBy, ApprovedBy. Zero / blank filters are not sent.
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "DeliveryOrderType", ORDER_TYPE);
        if (ctx.currentFinancialYearId() != 0) p.put("FinancialYearId", ctx.currentFinancialYearId());
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", u.getId());
        String dateType = str(f.get("dateType"));
        Timestamp from = Inv42Support.ts(f.get("fromDate")), to = Inv42Support.ts(f.get("toDate"));
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        if (toInt(f.get("fromDocNo")) != 0) p.put("DocNoFrom", toInt(f.get("fromDocNo")));
        if (toInt(f.get("toDocNo")) != 0) p.put("DocNoTo", toInt(f.get("toDocNo")));
        if (toInt(f.get("deliveryTypeId")) != 0) p.put("DeliveryTypeId", toInt(f.get("deliveryTypeId")));
        if (toInt(f.get("supplierCustomerId")) != 0) p.put("SupplierCustomerId", toInt(f.get("supplierCustomerId")));
        if (toInt(f.get("requestedById")) != 0) p.put("RequestedById", toInt(f.get("requestedById")));
        if (toInt(f.get("approvedById")) != 0) p.put("ApprovedById", toInt(f.get("approvedById")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("SP_DeliveryOrderFormHistory", p)) {
            Map<String, Object> o = new LinkedHashMap<>();        // dt columns :1910-1929 + the detail cells :2136
            o.put("Id", toInt(r.get("Id")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DeliveryType", str(r.get("DeliveryType")));
            o.put("Customer", str(r.get("CustomerName")));
            o.put("ReferenceParty", str(r.get("ReferencePartyName")));
            o.put("VehicleType", str(r.get("VehicleType")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("RequestedBy", str(r.get("RequestedBy")));
            o.put("ApprovedBy", str(r.get("ApprovedBy")));
            o.put("ApprovalStatus", str(r.get("ApprovalStatus")));
            o.put("EntryUser", str(r.get("EntryUser")));
            o.put("EntryDate", Inv42Support.dateTime(r.get("EntryDate")));
            o.put("ModifyUser", str(r.get("ModifyUser")));
            o.put("ModifyDate", Inv42Support.dateTime(r.get("ModifyDate")));
            o.put("ApprovedUser", str(r.get("ApprovedUser")));
            o.put("ApprovedDate", Inv42Support.dateTime(r.get("ApprovedDate")));
            o.put("Remarks", str(r.get("HeaderRemarks")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            o.put("WareHouseName", str(r.get("WareHouseName")));
            o.put("ItemCategory", str(r.get("ItemCategory")));
            o.put("ItemCode", str(r.get("ItemCode")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("PackUOM", str(r.get("PackUOM")));
            o.put("ItemDiscription", str(r.get("ItemDiscription")));
            o.put("LoadingQty", toDouble(r.get("LoadingQty")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById :1359 — InvDeliveryOrder.GetByID: 'ReadById' header + 'ReadByIdDetailIdForReturnable' rows. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();          // table.Rows.Add order :1388
            o.put("Id", toInt(d.get("Id")));
            o.put("WareHouseId", toInt(d.get("WarehouseId")));
            o.put("ItemCategoryId", toInt(d.get("ItemCategoryId")));
            o.put("ItemCategory", str(d.get("ItemCategory")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("ItemName", str(d.get("ItemName")));
            o.put("PackUomId", toInt(d.get("PackUomId")));
            o.put("PackUom", str(d.get("PackUOM")));
            o.put("ItemDescription", str(d.get("ItemDiscription")));
            o.put("ItemQty", toDouble(d.get("DoQty")));
            o.put("IsAssetItem", truthy(d.get("IsAssetItem")));
            o.put("DeliveryTypeId", toInt(d.get("DeliveryTypeId")));
            o.put("SupplierCustomerId", toInt(d.get("SupplierCustomerId")));
            o.put("RefPartyId", toInt(d.get("RefPartyId")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(h.get("Id")));
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("DocDate", day(h.get("DocDate")));
        out.put("VehicleType", str(h.get("VehicleType")));
        out.put("VehicleNo", str(h.get("VehicleNo")));
        out.put("RequestedByLookUpId", toInt(h.get("RequestedByLookUpId")));
        out.put("ApprovedByLookUpId", toInt(h.get("ApprovedByLookUpId")));
        out.put("LoadingInstructions", str(h.get("LoadingInstructions")));
        out.put("ReturnableDate", day(h.get("ReturnableDate")));
        out.put("IsApproved", truthy(h.get("IsApproved")));
        out.put("rows", rows);
        return out;
    }

    private List<Map<String, Object>> details(int id) {
        return db.rows("Sp_InvDeliveryOrder_GetAllMethod", params("Id", id, "Activity", "ReadByIdDetailIdForReturnable"));
    }

    /** The header must exist (ActionId <> 3), be DocumentTypeId 1200 and belong to the user's company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> r = db.rows("Sp_InvDeliveryOrder_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (r.size() != 1) return null;
        Map<String, Object> h = r.get(0);
        if (toInt(h.get("DocumentTypeId")) != DOC_TYPE) return null;
        if (toInt(h.get("CompanyId")) != toInt(u.getCompanyId())) return null;
        if (toInt(h.get("OrganizationId")) != toInt(u.getOrganizationId())) return null;
        return h;
    }

    // ==================================================================================== save

    /**
     * Insert :1173 — "Grid Record Not Found", FormValidation (:852), the confirmation, then one InvDeliveryOrder with
     * its InvDeliveryOrderdetail list (lstRemoveRecord first on Update, :1234) saved through InvDeliveryOrder.Save.
     * btnsave_Click sets RecId = 0 (:1311); btnSaveAs_Click also zeroes every row Id (:1331).
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = toInt(b.get("Id"));
        boolean saveAs = truthy(b.get("saveAs"));
        if (saveAs) recId = 0;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");                 // :1183
        Map<String, Object> existing = null;
        if (recId > 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        int docNo = existing != null ? toInt(existing.get("DocNo")) : docNo(u);
        formValidation(b, docNo);

        int deliveryTypeId = toInt(b.get("DeliveryTypeId"));
        int customerId = toInt(b.get("SupplierCustomerId"));
        int refPartyId = toInt(b.get("RefPartyId"));
        Timestamp docDate = Inv42Support.docDate(b.get("DocDate"));
        double doQty = 0d;
        for (Map<String, Object> r : rows) doQty += toDouble(r.get("ItemQty"));                       // :1206

        Map<String, Object> h = new LinkedHashMap<>();                                                 // :1189-1231
        h.put("Id", recId);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DeliveryOrderType", ORDER_TYPE);
        h.put("DocDate", docDate);
        h.put("DocNo", docNo);
        h.put("VehicleType", str(b.get("VehicleType")));
        h.put("VehicleNo", str(b.get("VehicleNo")).trim());
        h.put("RequestedByLookUpId", toInt(b.get("RequestedByLookUpId")));
        h.put("ApprovedByLookUpId", toInt(b.get("ApprovedByLookUpId")));
        if (deliveryTypeId == 4) h.put("ReturnableDate", Inv42Support.ts(b.get("ReturnableDate")));  // :1218
        h.put("LoadingInstructions", str(b.get("LoadingInstructions")));
        h.put("DoTotalQty", doQty);
        h.put("GrossWeight", doQty);
        h.put("NetWeight", doQty);
        h.put("BranchesId", u.getBranchesId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("EntryUser", u.getId());
        h.put("FinancialYearId", ctx.currentFinancialYearId());
        h.put("ScreenName", SCREEN);
        h.put("EntryDate", Inv42Support.now());
        h.put("ModifyDate", Inv42Support.now());
        h.put("ActionId", recId > 0 ? 2 : 1);
        if (recId > 0) {
            h.put("ModifyUser", u.getId());                                                            // :1197
            h.put("IsApproved", truthy(existing.get("IsApproved")));                                   // :1198
            h.put("AttachmentsValues", existing.get("AttachmentsValues"));
            h.put("CustomAttachmentsValues", existing.get("CustomAttachmentsValues"));
        } else {
            h.put("IsApproved", false);
        }
        for (String z : new String[] { "ApprovedUser", "ProjectsId", "EximInvoiceId", "ToBranchId", "PackingWeight", "OtherWeight",
                "LoadingPortId", "DepartmentFromId", "DepartmentToId", "SaleTypeId", "FromBranchId", "TransporterId" }) h.put(z, 0);
        h.put("IsStockReserved", false);

        List<Map<String, Object>> details = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removed"))) {                                     // :1234-1240
                if (toInt(r.get("Id")) <= 0) continue;
                if (!detailOfDocument(recId, toInt(r.get("Id")))) throw new IllegalArgumentException("Record Not Found");
                Map<String, Object> d = detail(r, 3, deliveryTypeId, customerId, refPartyId);
                details.add(d);
            }
        }
        Set<Integer> seen = new HashSet<>();
        for (int i = 0; i < rows.size(); i++) {                                                        // :1241-1281
            Map<String, Object> r = rows.get(i);
            int rowNo = i + 1;
            int id = recId != 0 ? toInt(r.get("Id")) : 0;
            if (id > 0 && (!detailOfDocument(recId, id) || !seen.add(id))) throw new IllegalArgumentException("Record Not Found");
            if (customerId <= 0) throw new IllegalArgumentException("Customer Field Required in Row# " + rowNo);
            if (toInt(r.get("WareHouseId")) <= 0) throw new IllegalArgumentException("WareHouseName Field Required in Row#" + rowNo);
            if (toInt(r.get("ItemId")) <= 0) throw new IllegalArgumentException("Item Name Field Required in Row#" + rowNo);
            if (toInt(r.get("PackUomId")) <= 0) throw new IllegalArgumentException("PackUOM Field Required in Row#" + rowNo);
            if (!(toDouble(r.get("ItemQty")) > 0d)) throw new IllegalArgumentException("ItemQty Field Required in Row#" + rowNo);
            Map<String, Object> d = detail(r, id > 0 ? 2 : 1, deliveryTypeId, customerId, refPartyId);
            d.put("Id", id);
            details.add(d);
        }

        int num = db.setProc(recId == 0 ? "Sp_InvDeliveryOrder_Insert" : "Sp_InvDeliveryOrder_Update", h);
        if (num <= 0) num = recId;
        if (num <= 0) throw new RuntimeException("Save returned no document id.");
        for (Map<String, Object> d : details) {
            d.put("InvDeliveryOrderId", num);
            db.setProc("Sp_InvDeliveryOrderDetail_Insert", d);
        }
        /* DAL InvDeliveryOrder.SetData — approval-flow row, as the web port of the 84 form does. */
        db.scalar("[DAW].[USp_DocumentApprovalDetail_Insert]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "Id", num, "LimitAmount", 0d));
        String msg = (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo;  // :1288 / :1293
        return Inv42Support.ok(msg, num, docNo);
    }

    /** FormValidation :852 (txtdocno, Delivery Type, Customer, Vehicle Type, Vehicle No, Returnable date, Requested/Approved By). */
    private void formValidation(Map<String, Object> b, int docNo) {
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (toInt(b.get("DeliveryTypeId")) <= 0) throw new IllegalArgumentException("Please Select Delivery Type field First");
        if (toInt(b.get("SupplierCustomerId")) <= 0) throw new IllegalArgumentException("Please Select Customer Field ");
        if (str(b.get("VehicleType")).trim().isEmpty()) throw new IllegalArgumentException("Vehicle Type Field is Required");
        if (str(b.get("VehicleNo")).isEmpty()) throw new IllegalArgumentException("Vehicle No Field is Required");
        if (toInt(b.get("DeliveryTypeId")) == 4) {
            Timestamp doc = Inv42Support.ts(b.get("DocDate")), ret = Inv42Support.ts(b.get("ReturnableDate"));
            if (doc != null && ret != null && doc.toLocalDateTime().toLocalDate().isAfter(ret.toLocalDateTime().toLocalDate())) {
                throw new IllegalArgumentException("Returnable date can not less than doc date");
            }
        }
        if (toInt(b.get("RequestedByLookUpId")) <= 0) throw new IllegalArgumentException("Requested By Field is Required");
        if (toInt(b.get("ApprovedByLookUpId")) <= 0) throw new IllegalArgumentException("Approved By Field is Required");
    }

    /** One InvDeliveryOrderdetail (:1244-1280 / lstRemoveRecord :1059-1071) for Sp_InvDeliveryOrderDetail_Insert. */
    private static Map<String, Object> detail(Map<String, Object> r, int actionTypeId, int deliveryTypeId, int customerId, int refPartyId) {
        Map<String, Object> d = new LinkedHashMap<>();
        double qty = toDouble(r.get("ItemQty"));
        d.put("Id", toInt(r.get("Id")));
        d.put("ActionTypeId", actionTypeId);
        d.put("DeliveryTypeId", actionTypeId == 3 ? 0 : deliveryTypeId);
        d.put("SupplierCustomerId", actionTypeId == 3 ? 0 : customerId);
        d.put("RefPartyId", actionTypeId == 3 ? 0 : refPartyId);
        d.put("WarehouseId", toInt(r.get("WareHouseId")));
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("ItemDiscription", str(r.get("ItemDescription")));
        d.put("PackUomId", toInt(r.get("PackUomId")));
        d.put("DoQty", qty);
        d.put("LoadingQty", qty);
        d.put("DoWeight", qty);
        d.put("LoadingWeight", qty);
        d.put("LoadingRemarks", actionTypeId == 3 ? "" : str(r.get("ItemDescription")));
        d.put("IsAssetItem", truthy(r.get("IsAssetItem")));
        for (String z : new String[] { "SaleOrderId", "InvPackingTypeId", "JobLotId", "SaleOrderDetailId", "RefDocumentTypeId",
                "RefDocIdNo", "RefDocSubIdNo", "CropYearId", "ExImInvoiceId", "BagTypeId", "InvoiceDetailId", "ContainerId",
                "InnerUomId", "AssetId", "WareHouseToId", "ToJobLotId", "ItemVariantId", "CastingTypeId", "DeliveryScheduleId",
                "DeliveryScheduleDetailId", "ThirdPartyAnalysisId", "ThirdPartyAnalysisSubId", "RateUomId" }) d.put(z, 0);
        for (String z : new String[] { "GrossWeight", "PackingWeight", "TotalPackingWeight", "ItemRate", "ItemAmount", "InnerQty",
                "InnerEbUnit", "InnerEbTotal", "OuterEbTotal", "AccessWtSet", "OtherWeight", "StockWeight" }) d.put(z, 0d);
        return d;
    }

    private boolean detailOfDocument(int headerId, int detailId) {
        for (Map<String, Object> d : details(headerId)) if (toInt(d.get("Id")) == detailId) return true;
        return false;
    }
}
