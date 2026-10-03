package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.ExportPmIssuanceForShipmentRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportShipmentFormsSupport.*;

/**
 * Architecture.WinApp.Export.PackingMaterialIssuanceForShipment - "Packing Material / Consumable Store Consumption For
 * Shipment" (ClientSize 1227 x 688, form Text "frmGSIssuance"), an InvGsStoreIssuanceHeader with DocumentTypeId 213.
 * Rights by the form name "PackingMaterialIssuanceForShipment" (no ScreenDefinition row, see ExportShipmentFormsSupport):
 * btnsave = Save, btnUpdate / history Edit = Update, 475-Print / 118-Voucher / history Print = Print,
 * history = CanView AllRecord. "Check Stock" needs the View right of "RptGenrateStocksStore" (ScreenDefinition 300).
 *
 * Save (Insert() :604) -> BLL 0252 Save -> DAL 0221 SetData, branch "DocumentTypeId == 452 || == 213":
 *   BLL  DateLock refusal; StoreIssuanceFinancialEffect && ActionForVoucherId == 1 -> MakeVoucher (213 branch:
 *        RefAccountId 0, Dr = DrAccountId or the item's COGS GL, Cr = the item's Purchase GL, Conversion.ToSingle amounts)
 *   DAL  Sp_InvGsStoreIssuanceHeader_Insert | _Update -> id; Sp_InvGsStoreIssuanceDetail_Insert per row (LineId 1..n,
 *        header id); Sp_InventoryTransactions_GetALLMethod + Sp_InventoryStockEvalautionDetail_Update; USP_InventoryValidation
 *        per row (@WarehouseId = Warehouseid); [voucher] Sp_VoucherHead_Insert|_Update, Sp_VoucherDetail_Insert
 *        (RefDocSubIdNo by LineId), Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert. One transaction.
 *
 * Desktop behaviour reproduced, not corrected:
 *  Q1  Only grid rows with ItemQty > 0 are saved; each detail goes with Id 0 and ActionTypeId 0 (the procedure forces 1
 *      for type 213), also on update.
 *  Q2  BranchSrNo, ProjectsId, ManualNo are never set by this form (0 / 0 / omitted).
 *  Q3  getAvgRate rounds the average rate to 2 places, AvgRateUpdateOnDocDateChange to 3.
 *  Q4  txtDocNo is read only; the number shown (StoreIssuanceGenerateCode(213) at reset, the stored DocNo after an Edit)
 *      is the number saved.
 *  Q5  ReadById of a missing id fails with the List indexer's own message (GetAll(...)[0]).
 * Not ported: Attachment (DMS) - the attachment dialog / physical file store is not available in the web port; the
 * header goes with AttachmentsValues unset exactly as a document without attachments does.
 */
@Service
public class ExportPmIssuanceForShipmentService {

    public static final String SCREEN_NAME = "PackingMaterialIssuanceForShipment";
    public static final int DOCUMENT_TYPE_ID = 213;

    @Autowired private ExportPmIssuanceForShipmentRepository repo;
    @Autowired private StoreIssuanceRepository store;
    @Autowired private SaleInvoiceRepository rightsRepo;
    @Autowired private CurrentUserContext ctx;

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    // ================================================================= load

    /** frmGSIssuance_Load: rights, GenerateDocNo, InvoiceNoFill, items / conditions / debit accounts / ports. */
    public Map<String, Object> setup() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights(rightsRepo, ctx, u, SCREEN_NAME));
        out.put("stockReportView", Boolean.TRUE.equals(rights(rightsRepo, ctx, u, "RptGenrateStocksStore").get("View")));
        put(out, "docNo", () -> store.nextDocNo(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID));
        put(out, "invoices", () -> invoiceRows(u, 0));
        lookups(u, out);
        return out;
    }

    /** btnRefresh_Click: the global services re-read, then InvoiceNoFill, items, conditions, debit accounts, ports. */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> invoiceRows(u, recId));
        lookups(u, out);
        return out;
    }

    private void lookups(UserAccount u, Map<String, Object> out) {
        put(out, "items", () -> items(u));
        put(out, "itemConditions", () -> store.itemConditions());
        put(out, "debitAccounts", () -> accounts(u));
        put(out, "ports", () -> ports(u));
        put(out, "racks", () -> store.racksWithWarehouseAndItem(u, branch(u)));
        put(out, "packingMaterialDefaultWarehouse", () -> asInt(store.config(u, "PackingMaterialDefaultWarehouse")));
    }

    /** GenerateDocNo (formReset): CommonServices.StoreIssuanceGenerateCode(213). */
    public Map<String, Object> docNo() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", store.nextDocNo(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID));
        return out;
    }

    /** InvoiceNoFill: Id / InvoiceNo (+ DocumentTypeId, NoOfContainers, MTon shown in the combo). */
    public List<Map<String, Object>> invoices(int recId) { return invoiceRows(ctx.requireAccountingUser(), recId); }

    private List<Map<String, Object>> invoiceRows(UserAccount u, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicesPending(u, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("MTon", asDouble(ci(r, "MTon")));
            out.add(m);
        }
        return out;
    }

    /** ItemDtsFillFromGlobal: ItemTypeOfTypeId 14 or 17; Id, ItemName, ItemCode, ItemCategory, LeadTimeDay, WeightCapacity (ParentCategoryId hidden). */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            int t = asInt(ci(r, "ItemTypeOfTypeId"));
            if (t != 14 && t != 17) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("ItemCategory", text(ci(r, "ItemCategory")));
            m.put("LeadTimeDay", asInt(ci(r, "LeadTimeDay")));
            m.put("WeightCapacity", text(ci(r, "WeightCapacity")));
            m.put("ParentCategoryId", asInt(ci(r, "InventoryParentCategoriesId")));
            out.add(m);
        }
        return out;
    }

    /** DatatableHelper.GetAccountsFromGlobalByTypeIds() - no filter, distinct ChartOfAccountId (first wins). */
    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : repo.allAccounts(u)) {
            int id = asInt(ci(a, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", text(ci(a, "AccountTitle")));
            m.put("AccountCode", text(ci(a, "AccountCode")));
            m.put("ParentAccountTitle", text(ci(a, "ParentAccountTitle")));
            m.put("AccountClass", text(ci(a, "AccountClassName")));
            out.add(m);
        }
        return out;
    }

    private List<Map<String, Object>> ports(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.seaPorts(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("PortName", text(ci(r, "PortName")));
            out.add(m);
        }
        return out;
    }

    /** InvoiceInfo (CmbInvoiceNo_Leave): the rows of GetInvoiceInformationByInvoiceIdForServices. */
    public List<Map<String, Object>> invoiceInfo(int invoiceId) {
        return plain(repo.invoiceInformation(ctx.requireAccountingUser(), invoiceId));
    }

    /**
     * BalanceStockQtyandAvgRate + getAvgRate: QtyInHand WITH warehouse / rack (Math.Round 2), AvgRate WITHOUT them
     * (Math.Round 2); DocumentTypeId 213 only when RECID > 0.
     */
    public Map<String, Object> stock(int recId, int itemId, String docDate, int itemConditionId, int warehouseId, int rackId) {
        UserAccount u = ctx.requireAccountingUser();
        Timestamp d = docDate(docDate, recId);
        int docType = recId > 0 ? DOCUMENT_TYPE_ID : 0;
        Map<String, Object> q = store.avgRateAndStock(u, itemId, d, itemConditionId, recId, docType, warehouseId, rackId);
        Map<String, Object> r = store.avgRateAndStock(u, itemId, d, itemConditionId, recId, docType, 0, 0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("qtyInHand", q == null ? 0d : round(asDouble(ci(q, "QtyInHand")), 2));
        out.put("avgRate", r == null ? 0d : round(asDouble(ci(r, "AvgRate")), 2));
        return out;
    }

    /** AvgRateUpdateOnDocDateChange: per grid row, AvgRate by item + condition (no warehouse / rack), Math.Round 3. */
    public List<Double> ratesOnDate(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = asInt(body.get("recId"));
        Timestamp d = docDate(text(body.get("docDate")), recId);
        List<Double> out = new ArrayList<>();
        for (Map<String, Object> row : list(body.get("rows"))) {
            Map<String, Object> r = store.avgRateAndStock(u, asInt(row.get("ItemId")), d, asInt(row.get("ItemConditionId")),
                    recId, recId > 0 ? DOCUMENT_TYPE_ID : 0, 0, 0);
            out.add(r == null ? 0d : round(asDouble(ci(r, "AvgRate")), 3));
        }
        return out;
    }

    /** txtDocdate.Value: the time of day the form was opened for a new document, midnight after ReadById. */
    private static Timestamp docDate(String iso, int recId) { return recId > 0 ? midnight(iso) : pickerDate(iso); }

    // ================================================================= history / read

    /** HistoryFill: FormHistory with DocumentTypeId 213, FinancialYearId, BranchesId, CanViewAllRecord, checked dates, doc nos. */
    public List<Map<String, Object>> history(String from, String to, int fromDocNo, int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = Boolean.TRUE.equals(rights(rightsRepo, ctx, u, SCREEN_NAME).get("CanViewAllRecord"));
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : store.formHistory(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(), 0, branch(u), viewAll, u.getId(),
                asDate(from) == null ? null : pickerDate(from), asDate(to) == null ? null : pickerDate(to), fromDocNo, toDocNo)) {
            int id = asInt(ci(r, "Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("DocNo", asInt(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("InvoiceNo", raw(ci(r, "InvoiceNo")));
            m.put("EntryUser", raw(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", raw(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("Remarks", raw(ci(r, "Remarks")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** ReadById / DataGridHistory_SelectionChanged: header, detail rows and VoucherHeadIdGet(RECID, 213). */
    public Map<String, Object> readById(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = store.header(id);
        if (h == null) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");   // Q5
        if (asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
            throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", asInt(ci(h, "Id")));
        out.put("DocNo", asInt(ci(h, "DocNo")));
        out.put("DocDate", iso(ci(h, "DocDate")));
        out.put("ExImInvoiceId", asInt(ci(h, "ExImInvoiceId")));
        out.put("Remarks", raw(ci(h, "Remarks")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : store.details(h)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", raw(ci(d, "ItemName")));
            m.put("WareHouseId", asInt(ci(d, "Warehouseid")));
            m.put("WareHouseName", raw(ci(d, "WareHouseName")));
            m.put("RackId", asInt(ci(d, "RackId")));
            m.put("RackName", raw(ci(d, "rackName")));
            m.put("ItemConditionId", asInt(ci(d, "ItemConditionId")));
            m.put("ItemCondition", raw(ci(d, "ItemCondition")));
            m.put("ItemQty", asDouble(ci(d, "IssueQty")));
            m.put("ItemRate", asDouble(ci(d, "ItemRate")));
            m.put("ItemAmount", asDouble(ci(d, "ItemAmount")));
            m.put("Remarks", raw(ci(d, "ReamarksDetail")));
            m.put("DrAcId", asInt(ci(d, "DrAccountId")));
            m.put("DebitAc", raw(ci(d, "DebitAc")));
            rows.add(m);
        }
        out.put("rows", rows);
        out.put("voucherHeadId", store.voucherHeadId(u, DOCUMENT_TYPE_ID, id));
        return out;
    }

    /** Print_Click / history Print: Print right; StoreIssuanceHeader_Slip475 refuses a document with no rows. */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = ctx.requireAccountingUser();
        require(rights(rightsRepo, ctx, u, SCREEN_NAME), "Print", "You don't have right to print");
        if (id == 0) throw new IllegalArgumentException("No record found");
        if (repo.slip475(u, id).isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return ok("", id);
    }

    // ================================================================= save

    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = asInt(body.get("recId"));
        Map<String, Boolean> r = rights(rightsRepo, ctx, u, SCREEN_NAME);
        if (recId > 0) require(r, "Update", "You do not have the Update right for this screen.");
        else require(r, "Save", "You do not have the Save right for this screen.");

        /* FormValidation */
        String docNoText = text(body.get("docNo"));
        if (docNoText.isEmpty() || asInt(docNoText) == 0) throw new IllegalArgumentException("DocNo Field Required");
        int invoiceId = asInt(body.get("invoiceId"));
        if (invoiceId == 0) throw new IllegalArgumentException("Invoice No Field Required");
        List<Map<String, Object>> grid = list(body.get("rows"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        if (recId > 0) {
            Map<String, Object> h = store.header(recId);
            if (h == null || asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
                throw new IllegalArgumentException("Record id not found...");
        }

        Timestamp docDate = docDate(text(body.get("docDate")), recId);
        Timestamp nowTs = now();
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("IsApproved", false);
        head.put("ApprovedDate", null);
        head.put("DocDate", docDate);
        head.put("EntryDate", nowTs);
        head.put("ModifyDate", nowTs);
        head.put("ApprovedUserId", 0);
        head.put("BranchesId", branch(u));
        head.put("BranchSrNo", 0);                                      // Q2
        head.put("CompanyId", u.getCompanyId());
        head.put("DocNo", asInt(docNoText));                            // Q4
        head.put("EntryUser", u.getId());
        head.put("Id", recId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", 0);
        head.put("RefDocNoId", 0);
        head.put("RefDocumentTypeId", 0);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("FinancialYearId", ctx.currentFinancialYearId());
        head.put("ExImInvoiceId", invoiceId);
        head.put("Remarks", raw(body.get("remarks")));
        head.put("ManualNo", null);
        head.put("AttachmentsValues", null);
        head.put("CustomAttachmentsValues", null);
        head.put("ActionId", recId == 0 ? 1 : 2);
        head.put("TypeId", 0);
        head.put("BaseDocumentTypeId", 0);
        head.put("IsUploaded", false);

        List<Map<String, Object>> details = new ArrayList<>();
        for (int i = 0; i < grid.size(); i++) {
            Map<String, Object> g = grid.get(i);
            if (!(asDouble(g.get("ItemQty")) > 0)) continue;                                   // Q1
            Map<String, Object> d = detailModel();
            d.put("LineId", i + 1);
            d.put("ItemId", asInt(g.get("ItemId")));
            d.put("Warehouseid", asInt(g.get("WareHouseId")));
            d.put("RackId", asInt(g.get("RackId")));
            d.put("ItemConditionId", asInt(g.get("ItemConditionId")));
            d.put("IssueQty", asDouble(g.get("ItemQty")));
            d.put("ItemRate", asDouble(g.get("ItemRate")));
            d.put("ItemAmount", asDouble(g.get("ItemAmount")));
            d.put("ReamarksDetail", raw(g.get("Remarks")));
            d.put("DrAccountId", asInt(g.get("DrAcId")));
            validate(d.get("ItemId"), "Item Name", i);
            validate(d.get("Warehouseid"), "Warehouse", i);
            validate(d.get("RackId"), "Rack Name", i);
            validate(d.get("ItemConditionId"), "Item Condition", i);
            validate(d.get("IssueQty"), "Item Qty", i);
            validate(d.get("ItemRate"), "Item Rate", i);
            validate(d.get("ItemAmount"), "Item Amount", i);
            details.add(d);
        }

        /* BLL Save: lock date first. */
        Timestamp lock = store.dateLock(u);
        if (lock != null && !docDate.after(lock)) throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        Map<String, Object> voucher = null;
        if (asBool(store.config(u, "StoreIssuanceFinancialEffect"))) voucher = makeVoucher(u, head, details);

        String proc = recId == 0 ? "Sp_InvGsStoreIssuanceHeader_Insert" : "Sp_InvGsStoreIssuanceHeader_Update";
        int num = store.setProc(proc, head);
        if (num > 0) head.put("Id", num); else num = recId;
        int line = 1;
        for (Map<String, Object> d : details) {
            d.put("LineId", line++);
            d.put("InvGsStoreIssuanceHeaderId", num);
            d.put("Id", store.setProc("Sp_InvGsStoreIssuanceDetail_Insert", d));
        }
        store.postStock(u, DOCUMENT_TYPE_ID, num);
        for (Map<String, Object> d : details) {
            store.inventoryValidation(u, DOCUMENT_TYPE_ID, docDate, asInt(d.get("ItemId")), asInt(d.get("ItemConditionId")),
                    asInt(d.get("Warehouseid")), asDouble(d.get("IssueQty")));
        }
        if (voucher != null) writeVoucher(u, num, voucher, details);
        return ok(recId == 0 ? "Record Save SuccessFully" + asInt(docNoText) : "Record Update SuccessFully" + asInt(docNoText), num);
    }

    /** Architecture.Model.PurchaseTrading.InvGsStoreIssuanceDetail - every non-virtual property with its default. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IssueQty", 0d);
        p.put("AssetsId", 0);
        p.put("IssuanceTypeId", 0);
        p.put("ExImInvoiceId", 0);
        p.put("DepartmentId", 0);
        p.put("Id", 0);
        p.put("LineId", 0);
        p.put("InvGsStoreIssuanceHeaderId", 0);
        p.put("ItemId", 0);
        p.put("ItemUomSchId", 0);
        p.put("ItemConditionId", 0);
        p.put("Warehouseid", 0);
        p.put("SupplierCustomerId", 0);
        p.put("EmployeeId", 0);
        p.put("DrAccountId", 0);
        p.put("BagTypeId", 0);
        p.put("ItemRate", 0d);
        p.put("ItemAmount", 0d);
        p.put("ReamarksDetail", null);
        p.put("RefDocumentTypeId", 0);
        p.put("RefDocId", 0);
        p.put("RefDocDetailId", 0);
        p.put("ActionTypeId", 0);
        p.put("DepartmentToId", 0);
        p.put("DepartRequestDetailId", 0);
        p.put("DepartRequestId", 0);
        p.put("WorkOrderIdId", 0);
        p.put("WorkStationFromId", 0);
        p.put("WorkStationToId", 0);
        p.put("ContractScheduleId", 0);
        p.put("RackId", 0);
        return p;
    }

    /** FormHelper.ValidateField. */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    // ================================================================= voucher (BLL 0252 MakeVoucher, 213 branch)

    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        String nowText = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = asInt(head.get("Id"));
        vh.RefAccountId = 0;
        vh.VoucherCode = asInt(head.get("DocNo"));
        vh.VoucherDate = String.valueOf(head.get("DocDate"));
        vh.Remarks = raw(head.get("Remarks"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = asInt(head.get("BranchesId"));
        vh.ProjectId = asInt(head.get("ProjectsId"));
        vh.DueDate = nowText;
        vh.DueDays = 0;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = asInt(head.get("FinancialYearId"));
        vh.EntryUser = u.getId();
        vh.EntryDate = nowText;
        vh.ModifyDate = nowText;
        vh.ModifyUser = u.getId();
        List<Map<String, Object>> gl = store.itemGlAccounts(u);
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        for (Map<String, Object> item : details) {
            if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            int itemId = asInt(item.get("ItemId"));
            Map<String, Object> g = null;
            for (Map<String, Object> x : gl) if (asInt(ci(x, "Id")) == itemId) { g = x; break; }
            if (g == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            double qty = asDouble(item.get("IssueQty")), rate = asDouble(item.get("ItemRate")), amount = asDouble(item.get("ItemAmount"));
            int drAc = asInt(item.get("DrAccountId")), cogs = asInt(ci(g, "COGSGLAC")), purchase = asInt(ci(g, "PurchaseGLAC"));
            String comments = raw(head.get("Remarks")) + "  Qty: " + StoreIssuanceRepository.clr(qty) + "   " + raw(ci(g, "ItemName"))
                    + "   Rate:" + StoreIssuanceRepository.clr(rate);
            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.LineId = asInt(item.get("LineId"));
            dr.AccountId = drAc > 0 ? drAc : cogs;
            dr.AgainstAccountId = purchase;
            dr.Comments = comments;
            dr.DebitAmount = (double) (float) amount;
            dr.CreditAmount = 0d;
            dr.ItemId = itemId; dr.QtyOut = qty; dr.ItemRate = rate; dr.ItemAmount = amount;
            dr.BranchesId = asInt(head.get("BranchesId"));
            lines.add(dr);
            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.LineId = asInt(item.get("LineId"));
            cr.AccountId = purchase;
            cr.AgainstAccountId = drAc > 0 ? drAc : cogs;
            cr.Comments = comments;
            cr.CreditAmount = (double) (float) amount;
            cr.DebitAmount = 0d;
            cr.ItemId = itemId; cr.QtyOut = qty; cr.ItemRate = rate; cr.ItemAmount = amount;
            cr.BranchesId = asInt(head.get("BranchesId"));
            lines.add(cr);
        }
        Map<String, Object> v = new LinkedHashMap<>();
        v.put("head", vh);
        v.put("lines", lines);
        return v;
    }

    /** DAL 0221 :171-218 - head Insert|Update by GetVoucherHeadId, details, H mirrors. */
    @SuppressWarnings("unchecked")
    private void writeVoucher(UserAccount u, int docId, Map<String, Object> voucher, List<Map<String, Object>> details) {
        ContraVoucherDto.Head vh = (ContraVoucherDto.Head) voucher.get("head");
        List<ContraVoucherDto.Detail> lines = (List<ContraVoucherDto.Detail>) voucher.get("lines");
        int existingId = store.voucherHeadId(u, DOCUMENT_TYPE_ID, docId);
        vh.Id = existingId;
        vh.DocumentTypeSrNo = docId;
        int num2 = store.setProc(existingId == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num2 > 0) vh.Id = num2;
        if (lines == null || lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail l : lines) {
            for (Map<String, Object> d : details) {
                if (asInt(d.get("LineId")) == (l.LineId == null ? 0 : l.LineId)) { l.RefDocSubIdNo = asInt(d.get("Id")); break; }
            }
            l.VoucherHeadId = vh.Id;
            store.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        int documentTypeIdRef = store.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            store.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
    }

    /** Public fields in declaration order (the DTOs mirror the desktop VoucherHead / VoucherDetail models). */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                String n = f.getName();
                if (v != null && ("VoucherDate".equals(n) || "ChequeDate".equals(n) || "DueDate".equals(n) || "EntryDate".equals(n)
                        || "ModifyDate".equals(n) || "PostDate".equals(n) || "DCheqDate".equals(n) || "GpDate".equals(n))) {
                    String s = String.valueOf(v);
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(n, v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }
}
