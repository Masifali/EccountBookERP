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
 * W5A - Mfg Production hub, screen 781 "Work Order For Semi Finish - Melting Heat".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.Mfg.frmWorkOrderForSemiFinish (.cs, 3868 lines), base.Name "frmWorkOrderForSemiFinish" (rights key)
 *   DocumentTypeId 1621 (:351, :1253, :1573)
 *   Route          /production/mfg/work-order-semi-finish      API /production/mfg/work-order-semi-finish/api
 *   Print          CommonServices.WorkOrderSemiFinish_Slip1621(id) -> 1621_WorkOrderSemiFinish_Slip.rpt (:1525), existing
 *                  /reports/print/1621-work-order-semi-finish-slip
 *   BLL / DAL      recovered_source/projects/architecture.bll/0368 + architecture.dal/0325 (Mfg.WorkOrder), model 0541 (workOrderItemDetail),
 *                  via {@link W3BWorkOrderRepository} (the same [Mfg].[USP_WorkOrder_GetAllMethod] family as screen 593)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 * <ol>
 *  <li>Load:264 - rights, DocumentNoFill:335 (WorkOrder.GenerateCode, 1621), GetItemCategory:362 (Sp_ItemCategory_GetAllMethod 'ReadByOrganizationCompanyId'),
 *      SampleModaldBCall:403 ([item].[USP_ItemModel_WithItemId]), ItemdtFill:474 (Sp_Item_GetAllMethod 'AllItemsBindForEngr'), CastingType:555
 *      ([Mfg].[USP_LookUps_GetAllMethod] 'GetDataByTypeId' type 1).</li>
 *  <li>Combo chain on the page: Category / Sample Modal / Item filtering (ItemDetailBind:494, ItemModalBind:423, CmbCategory_Leave:607, cmbItem_Leave:620,
 *      CmbSampleModal_Leave:777), UOM (ItemUomBind:640 = Sp_UOMSchedule_GetAllMethod 'ReadByItemID'), Variant (bindvarientunit:676 =
 *      USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo'), item finish weight (FillItemFinishWeight:589, casting type 1 = Sand weight, else Dye weight).</li>
 *  <li>Detail grid (:943): Add / Update / Cancel (FormValidationOperationDetail:823 messages), Edit, Delete (DeleteOperationDetailRow:1023: a saved row is queued
 *      with ActionTypeId 3 after a confirm; "Please Reset the Detail first.." while a row is being edited).</li>
 *  <li>Insert:1218 - FormValidation:794 ("doc No Field Required", "WorkOrder No Field Required", date rule messages), "Detail Grid Record Not Found...",
 *      confirm, header (DocumentTypeId 1621, IsApproved false, ProjectsId = BranchesId), removed rows first (only in update mode), then every grid row
 *      (ActionTypeId 1 new / 2 existing; Qty = 0 -> "Qty Field Required in Grid Detail Row No: n"), WorkOrder.Save ([Mfg].[USP_WorkOrder_Insert] / _Update and
 *      [Mfg].[USP_workOrderItemDetail_Insert] per row, one transaction), "Data Save Successfully....  DocNo" / "Data Update Successfully....  DocNo", FromReset,
 *      and the Preview tick prints the 1621 slip when the user has the Print right.</li>
 *  <li>ReadById:1375, Delete:1428 (WorkOrder.DeleteByID), SaveAs:1452 / DataGridHistory_SaveAs:1762, History:1549-1880 (columns :1580-:1596, Print / Edit / SaveAs
 *      buttons, detail grid of the selected row), Edit needs the Update right ("you don't have update rights...").</li>
 * </ol>
 *
 * DEVIATIONS / NOTES
 * <ol>
 *  <li>CmbOrderNoDetail (Order No) is never bound to data in the desktop code, so every new row carries OrderId 0 / OrderNo 0 and an Update resets them to 0;
 *      the control is therefore not drawn here (saved rows keep the OrderNo the database returns, always 0 from the 'ReadByHeaderId_workOrderItemDetail' query).</li>
 *  <li>The history procedure lists document types 1614 AND 1621. Opening a 1614 order here would re-save it as 1621 (the update procedure writes DocumentTypeId),
 *      so the history list, Read and Save are limited to 1621 documents.</li>
 *  <li>History EntryUserId is always sent (the desktop leaves it unset), so a user without CanViewAllRecord sees his own orders instead of none.</li>
 *  <li>Attachments (Attachment dialog / DMS copy) and the "Define Variant" / "Define Item" pop-up buttons are not ported.</li>
 *  <li>Database behaviour kept: USP_WorkOrder_Insert also creates the 1620 "Work Order In Progress" row (screen 594), and DeleteById refuses a work order that has an
 *      in-progress row, so Delete reports that procedure message for such orders. The Lead Time box is not saved by the desktop (Insert never reads it).</li>
 *  <li>Header ids the form never sets (BomHeaderId, ItemId, ItemUomId, ItemQty) are sent as 0, as the desktop's model does; the procedure's readers use LEFT JOINs for them.</li>
 * </ol>
 */
@Service
public class W5AWorkOrderSemiFinishService {

    public static final String SCREEN = "frmWorkOrderForSemiFinish";
    public static final int DOC_TYPE = 1621;

    private final Prod273Support db;
    private final W3BWorkOrderRepository repo;

    public W5AWorkOrderSemiFinishService(Prod273Support db, W3BWorkOrderRepository repo) {
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

    /** frmBillOfMaterial_Manufacturing_Load:264. */
    public Map<String, Object> lookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        UserAccount u = db.user();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("docNo", docNo());
        out.putAll(refresh());
        return out;
    }

    /** btnRefresh_Click:1129 - GetItemCategory, SampleModaldBCall, ItemdtFill, CastingType. */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cats = new ArrayList<>();
        for (Map<String, Object> r : repo.rows("Sp_ItemCategory_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"))) {
            cats.add(pair(toInt(ci(r, "Id")), str(ci(r, "CategoryDescription"))));
        }
        out.put("categories", cats);
        List<Map<String, Object>> modals = new ArrayList<>();
        for (Map<String, Object> r : repo.rows("[item].[USP_ItemModel_WithItemId]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Map<String, Object> o = pair(toInt(ci(r, "itemModalId")), str(ci(r, "ItemSampleNo")));
            o.put("ItemModal", str(ci(r, "ItemModal")));
            o.put("ItemCategoryId", toInt(ci(r, "ItemCategoryId")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            modals.add(o);
        }
        out.put("modals", modals);
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.rows("Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "AllItemsBindForEngr"))) {
            Map<String, Object> o = pair(toInt(ci(r, "Id")), str(ci(r, "ItemName")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCodeNew")));
            o.put("ItemCategoryId", toInt(ci(r, "ItemCategoryId")));
            o.put("ItemModalId", toInt(ci(r, "itemModalId")));
            o.put("SandWeight", toDouble(ci(r, "SandWeight")));
            o.put("DyeWeight", toDouble(ci(r, "DyeWeight")));
            items.add(o);
        }
        out.put("items", items);
        List<Map<String, Object>> cast = new ArrayList<>();
        for (Map<String, Object> r : repo.rows("[Mfg].[USP_LookUps_GetAllMethod]", params("Id", 1, "Activity", "GetDataByTypeId"))) {
            cast.add(pair(toInt(ci(r, "Id")), str(ci(r, "LookupName"))));
        }
        out.put("castingTypes", cast);
        return out;
    }

    public int docNo() {                                                                              // DocumentNoFill:335
        UserAccount u = db.user();
        return repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
    }

    /** ItemUomBind:640 + bindvarientunit:676 for one item. */
    public Map<String, Object> itemInfo(int itemId) {
        UserAccount u = db.user();
        List<Map<String, Object>> uoms = new ArrayList<>(), variants = new ArrayList<>();
        if (itemId > 0) {
            for (Map<String, Object> r : db.uomsByItem(u, itemId)) uoms.add(pair(toInt(ci(r, "Id")), str(ci(r, "UOMCode"))));
            for (Map<String, Object> r : repo.rows("[dbo].[USP_ItemAttributeVarient_GetAllMethod]", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "Activity", "GetAllForCombo"))) {
                variants.add(pair(toInt(ci(r, "ItemAttributeVarientId")), str(ci(r, "VarientDescription"))));
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("uoms", uoms);
        out.put("variants", variants);
        return out;
    }

    // ====================================================================================== read / history

    private void requireMine(UserAccount u, Map<String, Object> h) {
        if (h == null || toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())
                || toInt(ci(h, "DocumentTypeId")) != DOC_TYPE) {
            throw new IllegalArgumentException("Record Not Found");
        }
    }

    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.woItemDetails(id)) {
            int casting = toInt(ci(r, "CastingTypeId"));
            double weight = casting != 1 ? toDouble(ci(r, "DyeWeight")) : toDouble(ci(r, "SandWeight"));          // :1393
            double qty = toDouble(ci(r, "Qty"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "workOrderItemDetailId")));
            o.put("OrderId", toInt(ci(r, "OrderId")));
            o.put("OrderNo", toInt(ci(r, "OrderNo")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("UomId", toInt(ci(r, "UomId")));
            o.put("Uom", str(ci(r, "Uom")));
            o.put("VariantId", toInt(ci(r, "ItemVariantId")));
            o.put("VariantDescription", str(ci(r, "VariantDescription")));
            o.put("CastingTypeId", casting);
            o.put("CastingType", str(ci(r, "CastingType")));
            o.put("Qty", qty);
            o.put("WeightPerQty", weight);
            o.put("CastingWeight", weight * qty);
            rows.add(o);
        }
        return rows;
    }

    /** ReadById:1375. */
    public Map<String, Object> load(int id) {
        UserAccount u = db.user();
        Map<String, Object> h = repo.woHeader(id);
        requireMine(u, h);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(ci(h, "DocNo")));
        out.put("docDate", day(ci(h, "DocDate")));
        out.put("workOrderNo", str(ci(h, "ManualDocCode")));
        out.put("startDate", day(ci(h, "StartDate")));
        out.put("endDate", day(ci(h, "EndDate")));
        out.put("deadLine", day(ci(h, "DeadLine")));
        out.put("remarksHeader", str(ci(h, "RemarksHeader")));
        out.put("batchDesc", str(ci(h, "BatchDesc")));
        out.put("details", detailRows(id));
        return out;
    }

    /** HistoryGridFill:1561 (columns :1580-:1596). */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.woHistory(u, db.branchId(u), db.financialYearId(), DOC_TYPE, fromDocNo, toDocNo,
                ts(fromDate), ts(toDate), 0, 0, viewAll, toInt(u.getId()))) {
            if (toInt(ci(r, "DocumentTypeId")) != DOC_TYPE) continue;                       // the procedure also lists 1614 (see class notes)
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RecordNo", toInt(ci(r, "RecordNo")));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocNo", str(ci(r, "DocNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("ManualDocCode", str(ci(r, "ManualDocCode")));
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

    /** DatagridHistoryDetailbind:1796 - the item rows of one order. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = db.user();
        requireMine(u, repo.woHeader(id));
        return detailRows(id);
    }

    // ====================================================================================== save / delete

    /**
     * Insert:1218. Body: id, docNo, docDate, workOrderNo, startDate, endDate, deadLine, batchDesc, remarksHeader, rows [{Id, OrderId, ItemId, UomId, VariantId,
     * CastingTypeId, Qty}], removed [{Id, OrderId, ItemId, UomId, VariantId, CastingTypeId, Qty}] (update mode only).
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("id"));
        boolean update = recId > 0;
        if (update ? !db.has(SCREEN, "update") : !db.has(SCREEN, "save")) {
            throw new IllegalStateException(update ? "You do not have the Update right on Work Order For Semi Finish." : "You do not have the Save right on Work Order For Semi Finish.");
        }
        String docNoText = str(b.get("docNo")).trim();
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw new IllegalArgumentException("doc No Field Required");                     // :796
        String workOrderNo = str(b.get("workOrderNo"));
        if (workOrderNo.trim().isEmpty() || "0".equals(workOrderNo)) throw new IllegalArgumentException("WorkOrder No Field Required");    // :802
        Timestamp start = ts(b.get("startDate")), end = ts(b.get("endDate")), dead = ts(b.get("deadLine"));
        if (start != null && end != null && start.toLocalDateTime().toLocalDate().isAfter(end.toLocalDateTime().toLocalDate())) {
            throw new IllegalArgumentException("Start date cannot be greater than End Date");                                              // :808
        }
        if (dead != null && end != null && dead.toLocalDateTime().toLocalDate().isBefore(end.toLocalDateTime().toLocalDate())) {
            throw new IllegalArgumentException("Dead Line date cannot be less than End Date");                                             // :814
        }
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Grid Record Not Found...\nGrid Should have atleast one record");   // :1228
        List<Integer> owned = new ArrayList<>();
        if (update) {
            requireMine(u, repo.woHeader(recId));
            for (Map<String, Object> d : repo.woItemDetails(recId)) owned.add(toInt(ci(d, "workOrderItemDetailId")));
        }
        int n = 0;
        for (Map<String, Object> r : rows) {
            n++;
            if (toInt(r.get("ItemId")) == 0) continue;
            if (toDouble(r.get("Qty")) == 0d) throw new IllegalArgumentException("Qty Field Required in Grid Detail Row No: " + n);       // :1297
            int did = update ? toInt(r.get("Id")) : 0;
            if (did > 0 && !owned.contains(did)) throw new IllegalArgumentException("Record Not Found");
        }
        List<Map<String, Object>> removed = update ? list(b.get("removed")) : new ArrayList<>();
        for (Map<String, Object> d : removed) {
            if (!owned.contains(toInt(d.get("Id")))) throw new IllegalArgumentException("Record Not Found");
        }

        Timestamp stamp = now();
        int branch = db.branchId(u);
        Map<String, Object> h = params("WorkOrderId", update ? recId : null, "DocNo", toInt(docNoText), "DocDate", pickerValue(b.get("docDate")),
                "DocumentTypeId", DOC_TYPE, "ManualDocCode", workOrderNo, "BomHeaderId", 0, "ItemId", 0, "ItemUomId", 0, "ItemQty", 0,
                "StartDate", pickerValue(b.get("startDate")), "EndDate", pickerValue(b.get("endDate")), "DeadLine", pickerValue(b.get("deadLine")),
                "BatchDesc", str(b.get("batchDesc")), "RemarksHeader", str(b.get("remarksHeader")),
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branch, "ProjectsId", branch,
                "FinancialYearId", db.financialYearId(), "EntryUserId", u.getId(), "EntryDate", stamp, "ModifyUserId", u.getId(), "ModifyDate", stamp,
                "IsApproved", false, "ApprovedUserId", u.getId(), "ApprovedDate", stamp, "ActionId", update ? 2 : 1);
        int woId = repo.woSaveHeader(!update, h);
        if (update) woId = recId;

        for (Map<String, Object> d : removed) {                                                                                          // :1262
            repo.woSaveItemDetail(params("workOrderItemDetailId", toInt(d.get("Id")), "WorkOrderId", woId, "OrderId", toInt(d.get("OrderId")),
                    "ItemId", toInt(d.get("ItemId")), "UomId", toInt(d.get("UomId")), "ItemVariantId", toInt(d.get("VariantId")),
                    "CastingTypeId", toInt(d.get("CastingTypeId")), "Qty", toDouble(d.get("Qty")), "sortNo", 0, "ActionTypeId", 3));
        }
        for (Map<String, Object> r : rows) {
            if (toInt(r.get("ItemId")) == 0) continue;                                                                                   // :1273
            int did = update ? toInt(r.get("Id")) : 0;                                                                                    // :1311
            repo.woSaveItemDetail(params("workOrderItemDetailId", did > 0 ? did : null, "WorkOrderId", woId, "OrderId", toInt(r.get("OrderId")),
                    "ItemId", toInt(r.get("ItemId")), "UomId", toInt(r.get("UomId")), "ItemVariantId", toInt(r.get("VariantId")),
                    "CastingTypeId", toInt(r.get("CastingTypeId")), "Qty", toDouble(r.get("Qty")), "sortNo", 0, "ActionTypeId", did <= 0 ? 1 : 2));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(docNoText));         // :1334 / :1339
        out.put("id", woId);
        out.put("docNo", toInt(docNoText));
        return out;
    }

    /** btnDelete_Click:1428 - WorkOrder.DeleteByID(user, id). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = db.user();
        if (!db.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right on Work Order For Semi Finish.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        requireMine(u, repo.woHeader(id));
        repo.woDelete(toInt(u.getId()), id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Delete Record Successfully");
        return out;
    }
}
