package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.W3BWorkOrderRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.day;
import static com.mst.services.Prod273Support.list;
import static com.mst.services.Prod273Support.now;
import static com.mst.services.Prod273Support.pickerValue;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.ts;
import static com.mst.services.Prod273Support.truthy;

/**
 * W3B - Mfg Production hub (module 103) - screen 593 "Work Order".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.Mfg.frmWorkOrder (.cs, 5813 lines), base.Name "frmWorkOrder" (the rights key)
 *   DocumentTypeId 1614 (:434, :1932, :2449)
 *   Route          /production/mfg/work-order      API /production/mfg/work-order/api
 *   Print          CommonServices.WorkOrder_Slip1614(id) -> 1614_WorkOrder_Slip.rpt (:2308)
 *   BLL / DAL      WorkOrder (Mfg schema), BomHeader, SaleOrder - see {@link com.mst.repositories.W3BWorkOrderRepository}
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 * <ol>
 *  <li>Load:353 - rights, ItemFill:445, uomgetAll:604, AllWorkStationOpertaionFill:539, DocumentNoFill:418 (GenerateCode), the three
 *      detail grids (Operations, Items, Customers). History tab: Datetypefill:2396 / AllCombobind:2320.</li>
 *  <li>cmbItem_Leave:621 - item UOM list (ItemUomBind:639), BOM names of the item (BomNoFill:501), customers with a sale order for the item
 *      (CustomerFill:679), customer orders (BindCustomerOrders:747: one row per customer + item, first OrderId / OrderNo, summed qty).</li>
 *  <li>cmbBOMNo_Leave:1016 / GridFillByBomOperationDetail:1028 - the BOM's operations are added to the grid (hours = the hour of OperationTime,
 *      days = hours / OpHoursPerDay, deadline = now).</li>
 *  <li>FormValidation:918 ("doc No Field Required", "BOM Field Required", "Item Feild Required", "IteUom Field Required"),
 *      detail validations :947 / :970 / :993, duplicate checks and Update/Cancel/Delete row handling are on the page.</li>
 *  <li>Insert():1883 - "Operation Grid Record Not Found...", "Item Grid Record Not Found...", confirm, UpdateBOMInDetailGrid:1300 (every operation row
 *      takes the selected BOM), header (IsApproved false, ApprovedUserId = user), removed rows first (ActionTypeId 3, only in update mode), then
 *      Operations (row messages :1971-1982), Materials (:2020-2031), Customers (:2056-2067); ActionTypeId 1 for a new row / 2 for an existing one;
 *      "Data Save Successfully....  DocNo" / "Data Update Successfully....  DocNo".</li>
 *  <li>ReadById:2125 (header + Operation / Material / Customer lists; BOM combo disabled when the order has a BOM), btnDelete_Click:2223
 *      (DeleteByID: the procedure refuses when production or an in-progress order exists), SaveAs:2641 (loads, then saves as a new order),
 *      HistoryGridFill:2437 (columns :2460-2481; the procedure lists document types 1614 and 1621), DatagridHistory_SelectionChanged:2658
 *      (operations of the selected order), Edit needs the Update right ("you don't have update rights...").</li>
 * </ol>
 *
 * DEVIATIONS / NOTES
 * <ol>
 *  <li>The BLL assembly is not in the recovered source: each BLL call is mapped to the [Mfg] procedure of the same name. Header ActionId is 1 (insert) / 2
 *      (update). A zero BomHeaderId / BOMHeaderId / BOMOperationDetailId / ItemId / ItemUomId is left out of the call (NULL) instead of being sent as 0,
 *      so a manually added operation row cannot trip a foreign key.</li>
 *  <li>SaleOrder.ItemExistsAgainstSaleOrderNo (:1925) has no procedure in the database dump; its result is discarded by the desktop and the call is not made.</li>
 *  <li>Attachments (Attachment dialog / DMS copy: AT, FormHelper) are not ported; AttachmentsValues stay empty. Known gap.</li>
 *  <li>History EntryUserId is always sent (the desktop leaves it unset), so a user without CanViewAllRecord sees his own orders instead of none.</li>
 * </ol>
 */
@Service
public class W3BWorkOrderService {

    public static final String SCREEN = "frmWorkOrder";
    public static final int DOC_TYPE = 1614;

    private final Prod273Support db;
    private final W3BWorkOrderRepository repo;

    public W3BWorkOrderService(Prod273Support db, W3BWorkOrderRepository repo) {
        this.db = db;
        this.repo = repo;
    }

    private static Map<String, Object> pair(Object id, Object name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    // ====================================================================================== load

    /** frmBillOfMaterial_Manufacturing_Load:353. */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.putAll(refresh());
        out.put("docNo", docNo());
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));       // Datetypefill:2396
        out.putAll(historyCombos());
        return out;
    }

    /** btnRefresh_Click:1809 - ItemFill, uomgetAll, AllWorkStationOpertaionFill (BOM / customers follow the selected item). */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", Prod273Support.project(db.itemsGetAll(u), "Id", "ItemName"));              // ItemFill:445
        out.put("allUoms", db.allUoms(u));                                                           // uomgetAll:604
        out.put("workStationOperations", wsOps(u));                                                  // AllWorkStationOpertaionFill:539
        return out;
    }

    public int docNo() {                                                                              // DocumentNoFill:418
        UserAccount u = db.user();
        return repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
    }

    private List<Map<String, Object>> wsOps(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : new com.mst.repositories.W3BMfgRepository(db.jdbc()).wsoByWsoId(u, 0)) {
            Map<String, Object> o = pair(toInt(ci(r, "Id")), str(ci(r, "Description")));
            o.put("WorkStationName", str(ci(r, "WorkStationName")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("OpHoursPerDay", toDouble(ci(r, "OpHoursPerDay")));
            out.add(o);
        }
        return out;
    }

    /** AllCombobind:2320 - Activity 'BOMName' (Id = WorkOrderId, ReferenceName = ManualDocCode) and 'ItemHeader'. */
    public Map<String, Object> historyCombos() {
        UserAccount u = db.user();
        List<Map<String, Object>> wo = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.woDropDowns(u)) {
            String a = str(ci(r, "Activity"));
            if ("BOMName".equals(a)) wo.add(pair(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
            if ("ItemHeader".equals(a)) items.add(pair(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bomNames", wo);
        out.put("headerItems", items);
        return out;
    }

    /** BomNoFill:501 - BomHeader.GetBomNoByItemId -> Id / BomName. */
    public List<Map<String, Object>> bomNames(int itemId) {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : repo.bomNamesByItem(u, itemId)) out.add(pair(toInt(ci(r, "Id")), str(ci(r, "BomName"))));
        return out;
    }

    /** CustomerFill:679 + BindCustomerOrders:747. */
    public Map<String, Object> saleOrders(int itemId) {
        UserAccount u = db.user();
        List<Map<String, Object>> customers = new ArrayList<>(), orders = new ArrayList<>();
        Map<Integer, Map<String, Object>> byCustomer = new LinkedHashMap<>();
        if (itemId > 0) {
            for (Map<String, Object> r : repo.saleOrderInfoByItem(u, itemId)) {
                int cust = toInt(ci(r, "CustomerId"));
                if (!byCustomer.containsKey(cust)) {
                    customers.add(pair(cust, str(ci(r, "CustomerName"))));
                    Map<String, Object> o = new LinkedHashMap<>();           // group by (CustomerId, ItemId): first OrderId / OrderNo, summed ItemQty
                    o.put("CustomerId", cust);
                    o.put("OrderId", toInt(ci(r, "OrderId")));
                    o.put("OrderNo", toInt(ci(r, "OrderNo")));
                    o.put("OrderQty", 0d);
                    byCustomer.put(cust, o);
                }
                Map<String, Object> o = byCustomer.get(cust);
                o.put("OrderQty", toDouble(o.get("OrderQty")) + toDouble(ci(r, "ItemQty")));
            }
            for (Map<String, Object> o : byCustomer.values()) if (toInt(o.get("OrderId")) > 0) orders.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", customers);
        out.put("orders", orders);
        return out;
    }

    /** GridFillByBomOperationDetail:1028 - the BOM's operation rows as grid rows. */
    public List<Map<String, Object>> bomOperations(int bomId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bomOperationDetail(bomId)) {
            double hour = hourOf(ci(r, "operationTime"));
            double perDay = toDouble(ci(r, "OpHoursPerDay"));
            if (perDay == 0d) throw new IllegalArgumentException("Attempted to divide by zero.");
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("BOMHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BOMOperationDetailId", toInt(ci(r, "BomOperationDetailId")));
            o.put("WorkStationOperationId", toInt(ci(r, "WorkStationOperationId")));
            o.put("WorkStationName", str(ci(r, "WorkStationName")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("OpHours", hour);
            o.put("OpHoursPerDay", perDay);
            o.put("OpDays", (double) (long) (hour / perDay));       // Conversion.ToInt(hour / OpHoursPerDay)
            o.put("DeadLine", day(now()));
            out.add(o);
        }
        return out;
    }

    /** DateTime.Hour of the BOM operation time (a time / datetime column or its text). */
    static double hourOf(Object v) {
        if (v == null) return 0d;
        if (v instanceof java.sql.Time) return ((java.sql.Time) v).toLocalTime().getHour();
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().getHour();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().getHour();
        String s = String.valueOf(v).trim();
        int sp = s.indexOf(' ');
        if (sp >= 0) s = s.substring(sp + 1);
        int c = s.indexOf(':');
        return c > 0 ? toInt(s.substring(0, c)) : 0d;
    }

    // ====================================================================================== read / history

    private void requireMine(UserAccount u, Map<String, Object> h) {
        if (h == null || toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())
                || toInt(ci(h, "DocumentTypeId")) != DOC_TYPE) {
            throw new IllegalArgumentException("Record Not Found");
        }
    }

    /** ReadById:2125. */
    public Map<String, Object> load(int id) {
        UserAccount u = db.user();
        Map<String, Object> h = repo.woHeader(id);
        requireMine(u, h);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(ci(h, "DocNo")));
        out.put("docDate", day(ci(h, "DocDate")));
        out.put("bomHeaderId", toInt(ci(h, "BomHeaderId")));
        out.put("manualDocCode", str(ci(h, "ManualDocCode")));
        out.put("startDate", day(ci(h, "StartDate")));
        out.put("endDate", day(ci(h, "EndDate")));
        out.put("deadLine", day(ci(h, "DeadLine")));
        out.put("remarksHeader", str(ci(h, "RemarksHeader")));
        out.put("itemId", toInt(ci(h, "ItemId")));
        out.put("itemUomId", toInt(ci(h, "ItemUomId")));
        out.put("itemQty", toDouble(ci(h, "ItemQty")));
        out.put("batchDesc", str(ci(h, "BatchDesc")));
        out.put("operations", operationRows(repo.woOperations(id)));
        out.put("items", materialRows(repo.woMaterials(id)));
        out.put("customers", customerRows(repo.woCustomers(id)));
        return out;
    }

    public List<Map<String, Object>> operationRows(List<Map<String, Object>> src) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "WorkOrderOperationId")));
            o.put("BOMHeaderId", toInt(ci(r, "BOMHeaderId")));
            o.put("BOMOperationDetailId", toInt(ci(r, "BOMOperationDetailId")));
            o.put("WorkStationOperationId", toInt(ci(r, "WorkStationOperationId")));
            o.put("WorkStationName", str(ci(r, "WorkStationName")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("OpHours", toDouble(ci(r, "OperationHours")));
            o.put("OpHoursPerDay", toDouble(ci(r, "OpHoursPerDay")));
            o.put("OpDays", toDouble(ci(r, "OperationDays")));
            o.put("DeadLine", day(ci(r, "DeadLine")));
            rows.add(o);
        }
        return rows;
    }

    public List<Map<String, Object>> materialRows(List<Map<String, Object>> src) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "WorkOrderMaterialId")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("UOMId", toInt(ci(r, "uomId")));
            o.put("UOM", str(ci(r, "PackUom")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            rows.add(o);
        }
        return rows;
    }

    public List<Map<String, Object>> customerRows(List<Map<String, Object>> src) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "WorkOrderCustomerId")));
            o.put("CustomerId", toInt(ci(r, "CustomerId")));
            o.put("CustomerName", str(ci(r, "CustomerName")));
            o.put("SaleOrderId", toInt(ci(r, "SaleOrderId")));
            o.put("SaleOrder", toInt(ci(r, "SaleOrderNo")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            rows.add(o);
        }
        return rows;
    }

    /** HistoryGridFill:2437 (columns :2460-2481). */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo, int workOrderId, int itemId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.woHistory(u, db.branchId(u), db.financialYearId(), DOC_TYPE, fromDocNo, toDocNo,
                ts(fromDate), ts(toDate), workOrderId, itemId, viewAll, toInt(u.getId()))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RecordNo", toInt(ci(r, "RecordNo")));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", str(ci(r, "DocNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("ManualDocCode", str(ci(r, "ManualDocCode")));
            o.put("BomNo", str(ci(r, "BomHeaderNo")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("Uom", str(ci(r, "Uom")));
            o.put("ItemQty", toDouble(ci(r, "ItemQty")));
            o.put("StartDate", day(ci(r, "StartDate")));
            o.put("EndDate", day(ci(r, "EndDate")));
            o.put("DeadLine", day(ci(r, "DeadLine")));
            o.put("BatchDesc", str(ci(r, "BatchDesc")));
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            o.put("EntryUserName", str(ci(r, "EntryUserName")));
            o.put("EntryDate", Prod273Support.stamp(ci(r, "EntryDate")));
            o.put("ModifyUserName", str(ci(r, "ModifyUserName")));
            o.put("ModifyDate", Prod273Support.stamp(ci(r, "ModifyDate")));
            o.put("IsApproved", truthy(ci(r, "IsApproved")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== save / delete

    private static Map<String, Object> put(Map<String, Object> p, String key, int v) { if (v != 0) p.put(key, v); return p; }

    /**
     * Insert():1883. Body: id, docNo, docDate, manualDocCode, bomHeaderId, itemId, itemUomId, itemQty, startDate, endDate, deadLine, batchDesc,
     * remarksHeader, operations [..], items [..], customers [..], removedOperations / removedItems / removedCustomers (update mode only).
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("id"));
        boolean update = recId > 0;
        if (update ? !db.has(SCREEN, "update") : !db.has(SCREEN, "save")) {
            throw new IllegalStateException(update ? "You do not have the Update right on Work Order." : "You do not have the Save right on Work Order.");
        }
        String docNoText = str(b.get("docNo")).trim();
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw new IllegalArgumentException("doc No Field Required");
        String manual = str(b.get("manualDocCode"));
        if (manual.trim().isEmpty() || "0".equals(manual)) throw new IllegalArgumentException("BOM Field Required");
        int itemId = toInt(b.get("itemId"));
        if (itemId == 0) throw new IllegalArgumentException("Item Feild Required");
        int uomId = toInt(b.get("itemUomId"));
        if (uomId == 0) throw new IllegalArgumentException("IteUom Field Required");
        List<Map<String, Object>> ops = list(b.get("operations")), items = list(b.get("items")), custs = list(b.get("customers"));
        if (ops.isEmpty()) throw new IllegalArgumentException("Operation Grid Record Not Found...\nGrid Should have atleast one record");
        if (items.isEmpty()) throw new IllegalArgumentException("Item Grid Record Not Found...\nGrid Should have atleast one record");
        if (update) requireMine(u, repo.woHeader(recId));

        int bomId = toInt(b.get("bomHeaderId"));
        Timestamp stamp = now();
        int branch = db.branchId(u);
        Map<String, Object> h = params("WorkOrderId", update ? recId : null, "DocNo", toInt(docNoText), "DocDate", pickerValue(b.get("docDate")),
                "DocumentTypeId", DOC_TYPE, "ManualDocCode", manual, "ItemId", itemId, "ItemUomId", uomId, "ItemQty", toDouble(b.get("itemQty")),
                "StartDate", pickerValue(b.get("startDate")), "EndDate", pickerValue(b.get("endDate")), "DeadLine", pickerValue(b.get("deadLine")),
                "BatchDesc", str(b.get("batchDesc")), "RemarksHeader", str(b.get("remarksHeader")),
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branch, "ProjectsId", branch,
                "FinancialYearId", db.financialYearId(), "EntryUserId", u.getId(), "EntryDate", stamp, "ModifyUserId", u.getId(), "ModifyDate", stamp,
                "IsApproved", false, "ApprovedUserId", u.getId(), "ApprovedDate", stamp, "ActionId", update ? 2 : 1);
        put(h, "BomHeaderId", bomId);

        // validate every detail row before any write (the desktop returns at the first bad row, before WorkOrder.Save)
        int n = 0;
        for (Map<String, Object> r : ops) {
            n++;
            if (toInt(r.get("WorkStationOperationId")) == 0) continue;
            if (str(r.get("OpDays")).isEmpty()) throw new IllegalArgumentException("OpDays  Field Required in Grid Operations Row No: " + n);
            if (str(r.get("OpHoursPerDay")).isEmpty()) throw new IllegalArgumentException("OpHoursPerDay  Field Required in Grid Operations Row No: " + n);
        }
        n = 0;
        for (Map<String, Object> r : items) {
            n++;
            if (toInt(r.get("ItemId")) == 0) continue;
            if (toInt(r.get("UOMId")) == 0) throw new IllegalArgumentException("Uom Field Required in Item_Grid Row No: " + n);
            if (toDouble(r.get("Qty")) == 0d) throw new IllegalArgumentException("Qty Field Required in Item_Grid Row No: " + n);
        }
        n = 0;
        for (Map<String, Object> r : custs) {
            n++;
            if (toInt(r.get("CustomerId")) <= 0) continue;
            if (toInt(r.get("SaleOrderId")) == 0) throw new IllegalArgumentException("SaleOrder Field Required in Customer_Grid Row No: " + n);
            if (toDouble(r.get("Qty")) == 0d) throw new IllegalArgumentException("Qty Field Required in Customer_Grid Row No: " + n);
        }

        int woId = repo.woSaveHeader(!update, h);
        if (update) woId = recId;

        if (update) for (Map<String, Object> d : list(b.get("removedOperations"))) {          // :1956
            Map<String, Object> p = params("WorkOrderId", woId, "WorkOrderOperationId", toInt(d.get("Id")), "WorkStationOperationId", toInt(d.get("WorkStationOperationId")),
                    "OperationHours", toDouble(d.get("OpHours")), "OpHoursPerDay", toDouble(d.get("OpHoursPerDay")), "OperationDays", toDouble(d.get("OpDays")),
                    "DeadLine", pickerValue(d.get("DeadLine")), "Remarks", str(d.get("Remarks")), "ActionTypeId", 3);
            put(p, "BOMHeaderId", toInt(d.get("BOMHeaderId"))); put(p, "BOMOperationDetailId", toInt(d.get("BOMOperationDetailId")));
            repo.woSaveOperation(p);
        }
        for (Map<String, Object> r : ops) {
            if (toInt(r.get("WorkStationOperationId")) == 0) continue;                           // :1966
            int id = update ? toInt(r.get("Id")) : 0;
            Map<String, Object> p = params("WorkOrderId", woId, "WorkOrderOperationId", id > 0 ? id : null, "WorkStationOperationId", toInt(r.get("WorkStationOperationId")),
                    "OperationHours", toDouble(r.get("OpHours")), "OpHoursPerDay", toDouble(r.get("OpHoursPerDay")), "OperationDays", toDouble(r.get("OpDays")),
                    "Qty", 0, "DeadLine", pickerValue(r.get("DeadLine")), "ActionTypeId", id <= 0 ? 1 : 2);
            put(p, "BOMHeaderId", bomId); put(p, "BOMOperationDetailId", toInt(r.get("BOMOperationDetailId")));   // UpdateBOMInDetailGrid:1300
            repo.woSaveOperation(p);
        }
        if (update) for (Map<String, Object> d : list(b.get("removedItems"))) {                // :1999
            repo.woSaveMaterial(params("WorkOrderId", woId, "WorkOrderMaterialId", toInt(d.get("Id")), "ItemId", toInt(d.get("ItemId")),
                    "uomId", toInt(d.get("UOMId")), "Qty", toDouble(d.get("Qty")), "ActionTypeId", 3));
        }
        for (Map<String, Object> r : items) {
            if (toInt(r.get("ItemId")) == 0) continue;                                           // :2009
            int id = update ? toInt(r.get("Id")) : 0;
            repo.woSaveMaterial(params("WorkOrderId", woId, "WorkOrderMaterialId", id > 0 ? id : null, "ItemId", toInt(r.get("ItemId")),
                    "uomId", toInt(r.get("UOMId")), "Qty", toDouble(r.get("Qty")), "price", 0, "amount", 0, "sortNo", 0, "ActionTypeId", id <= 0 ? 1 : 2));
        }
        if (update) for (Map<String, Object> d : list(b.get("removedCustomers"))) {            // :2036
            repo.woSaveCustomer(params("WorkOrderId", woId, "WorkOrderCustomerId", toInt(d.get("Id")), "CustomerId", toInt(d.get("CustomerId")),
                    "SaleOrderId", toInt(d.get("SaleOrderId")), "Qty", toDouble(d.get("Qty")), "ActionTypeId", 3));
        }
        for (Map<String, Object> r : custs) {
            if (toInt(r.get("CustomerId")) <= 0) continue;                                       // :2046
            int id = update ? toInt(r.get("Id")) : 0;
            repo.woSaveCustomer(params("WorkOrderId", woId, "WorkOrderCustomerId", id > 0 ? id : null, "CustomerId", toInt(r.get("CustomerId")),
                    "SaleOrderId", toInt(r.get("SaleOrderId")), "Qty", toDouble(r.get("Qty")), "ActionTypeId", id <= 0 ? 1 : 2));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(docNoText));   // :2081 / :2086
        out.put("id", woId);
        out.put("docNo", toInt(docNoText));
        return out;
    }

    /** btnDelete_Click:2223 - WorkOrder.DeleteByID(user, id). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = db.user();
        if (!db.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right on Work Order.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        requireMine(u, repo.woHeader(id));
        repo.woDelete(toInt(u.getId()), id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Delete Record Successfully");
        return out;
    }
}
