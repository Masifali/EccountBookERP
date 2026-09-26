package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StoreSendReceiptRequest;
import com.mst.repositories.GrnPmRepository;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.StoreSendReceiptRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;
import static com.mst.repositories.StoreSendReceiptRepository.*;

/**
 * Screen 504 "Store Send Receipt" - Packing Material module 54. Desktop
 * {@code Architecture.WinApp.PackingMaterial_Store.StoreSendReceipt}, DocumentTypeId 177.
 *
 * Save = {@code Insert()} :590 -> BLL InvStoreSendReceipt.Save -> DAL 0395 SetData:
 * header (Insert / Update, the update procedure deletes the old details) -> each detail ->
 * attachments -> Sp_InventoryStockEvalautionDetail_Update -> Sp_InventoryTransactions_GetALLMethod.
 * No voucher.
 */
@Service
public class StoreSendReceiptService {

    public static final int SCREEN_ID = 504;

    private final StoreSendReceiptRepository repo;
    private final GrnPmRepository grn;
    private final InventoryOpeningRepository shared;
    private final DesktopAttachmentStore store;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public StoreSendReceiptService(StoreSendReceiptRepository repo, GrnPmRepository grn, InventoryOpeningRepository shared,
                                   DesktopAttachmentStore store, JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.grn = grn; this.shared = shared; this.store = store; this.jdbc = jdbc; this.context = context; this.rights = rights;
    }

    private UserAccount user(String action) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int financialYear(UserAccount u) {
        var years = shared.years(u);
        if (years.isEmpty()) throw new IllegalArgumentException("No active financial year allocated to this company");
        return intOf(years.get(0).get("Id"));
    }

    // ================================================================== load

    /** StoreSendReceipt_Load :170. */
    public Map<String, Object> lookups() {
        UserAccount u = user("View");
        int fy = financialYear(u);
        var wh = repo.warehouses(u);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("senderWarehouses", wh.stream().filter(w -> intOf(w.get("WareHouseTypeId")) == 2).toList());
        r.put("receiverWarehouses", wh.stream().filter(w -> Set.of(2, 3).contains(intOf(w.get("WareHouseTypeId")))).toList());
        r.put("items", repo.items(u));
        r.put("conditions", grn.itemConditions());
        List<Map<String, Object>> senders = new ArrayList<>(), receivers = new ArrayList<>();
        for (var c : repo.historyCombos(u, fy)) {
            String act = Objects.toString(col(c, "Activity"), "");
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(c, "Id")));
            m.put("Description", col(c, "ReferenceName"));
            if ("SenderWarehouse".equals(act)) senders.add(m); else if ("ReceiverWarehouse".equals(act)) receivers.add(m);
        }
        r.put("historySenders", senders);
        r.put("historyReceivers", receivers);
        Map<String, Boolean> p = new LinkedHashMap<>();
        for (String a : List.of("Save", "Update", "Print")) p.put(a, allowed(u, a));
        r.put("permissions", p);
        r.put("financialYearStart", yearStart(u));
        r.put("docNo", number(u, fy));
        return r;
    }

    private int number(UserAccount u, int fy) {
        int code = repo.nextDocNo(u, fy);
        if (code <= 0) throw new IllegalArgumentException("DocNo Not Found");                          // GenerateCode :212
        return code;
    }

    public Map<String, Object> numbers() {
        UserAccount u = user("View");
        return Map.of("docNo", number(u, financialYear(u)));
    }

    /** Loader grid. */
    public List<Map<String, Object>> pending() {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : repo.pending(u, financialYear(u))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RefDocumentTypeId", intOf(col(r, "DocumentTypeId")));
            m.put("RefDocIdNo", intOf(col(r, "PrdHeaderId")));
            m.put("RefDocSubIdNo", intOf(col(r, "PrdDetailId")));
            m.put("DocDate", col(r, "DocDate"));
            m.put("DocNo", col(r, "DocCode"));
            m.put("SenderWareHouseId", intOf(col(r, "SenderWareHouseId")));
            m.put("SenderWareHouse", col(r, "SenderWareHouse"));
            m.put("PackingTypeId", intOf(col(r, "PackingtypeId")));
            m.put("PackingType", col(r, "PackTypeCode"));
            m.put("PackUom", col(r, "UOMCode"));
            m.put("IssueQty", dbl(col(r, "IssueQty")));
            m.put("ReceivedQty", dbl(col(r, "ReceiptQty")));
            m.put("BalQty", dbl(col(r, "BalanceQty")));
            out.add(m);
        }
        return out;
    }

    /** Grd_CellUpdated :830 - Math.Round(AvgRate, 3). */
    public Map<String, Object> avgRate(int itemId, int conditionId, String docDate, int recId) {
        UserAccount u = user("View");
        if (recId > 0) repo.header(u, recId);
        return Map.of("rate", rate(u, itemId, conditionId, date(docDate), recId));
    }

    private double rate(UserAccount u, int itemId, int conditionId, Date docDate, int recId) {
        Double r = repo.avgRate(u, itemId, docDate, conditionId, recId);
        return r == null ? 0d : BigDecimal.valueOf(r).setScale(3, RoundingMode.HALF_EVEN).doubleValue();
    }

    // ================================================================== history / record

    public List<Map<String, Object>> history(String from, String to, int fromDocNo, int toDocNo, int senderId, int receiverId) {
        UserAccount u = user("View");
        HistoryFilter f = new HistoryFilter();
        f.financialYearId = financialYear(u);
        f.from = blank(from) ? null : date(from);
        f.to = blank(to) ? null : date(to);
        f.fromDocNo = fromDocNo; f.toDocNo = toDocNo; f.senderId = senderId; f.receiverId = receiverId;
        return repo.history(u, f);
    }

    /** ReadById :946. */
    public Map<String, Object> record(int id) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", repo.header(u, id));
        List<Map<String, Object>> lines = new ArrayList<>();
        Map<Integer, Integer> recordNo = new HashMap<>();
        var details = repo.details(id);
        for (var d : details) recordNo.putIfAbsent(intOf(col(d, "RefDocDetailId")), recordNo.size() + 1);
        for (var d : details) {
            Map<String, Object> l = new LinkedHashMap<>();
            l.put("Id", intOf(col(d, "InvStoreSendReceiptDetailId")));
            l.put("RefDocNoId", intOf(col(d, "RefDocNoId")));
            l.put("RefDocDetailId", intOf(col(d, "RefDocDetailId")));
            l.put("RefDocumentTypeId", intOf(col(d, "RefDocumentTypeId")));
            l.put("RecordNo", recordNo.get(intOf(col(d, "RefDocDetailId"))));
            l.put("ItemId", intOf(col(d, "ItemId")));
            l.put("ItemName", col(d, "ItemName"));
            l.put("ItemConditionId", intOf(col(d, "ItemConditionId")));
            l.put("ItemCondition", col(d, "ItemCondition"));
            l.put("PackingTypeId", intOf(col(d, "InvPackingTypeId")));
            l.put("PackingType", col(d, "PackingType"));
            l.put("ItemQty", dbl(col(d, "QtyIn")));
            l.put("BalQty", dbl(col(d, "QtyIn")));
            l.put("AvgRate", dbl(col(d, "ItemRate")));
            l.put("Amount", dbl(col(d, "ItemAmount")));
            lines.add(l);
        }
        out.put("lines", lines);
        out.put("attachments", repo.attachments(u, id));
        return out;
    }

    // ================================================================== save

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(StoreSendReceiptRequest r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Record Not Found");
        boolean insert = r.Id == 0;
        UserAccount u = user(insert ? "Save" : "Update");
        int fy = financialYear(u);
        Map<String, Object> old = insert ? null : repo.header(u, r.Id);

        // FormValidation :542
        int docNo = insert ? number(u, fy) : intOf(col(old, "DocNo"));
        if (docNo == 0) throw new IllegalArgumentException("Doc No is Required");
        var wh = repo.warehouses(u);
        if (r.SenderWarehouseId == 0 || wh.stream().noneMatch(w -> intOf(w.get("Id")) == r.SenderWarehouseId && intOf(w.get("WareHouseTypeId")) == 2)) {
            throw new IllegalArgumentException("Sender Warehouse is Required");
        }
        if (r.ReceiverWarehouseId == 0 || wh.stream().noneMatch(w -> intOf(w.get("Id")) == r.ReceiverWarehouseId && Set.of(2, 3).contains(intOf(w.get("WareHouseTypeId"))))) {
            throw new IllegalArgumentException("Receiver Warehouse is Required");
        }
        List<StoreSendReceiptRequest.Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        Date docDate = date(r.DocDate);

        // the rows must come from the loader (or, on update, from the saved document)
        Map<String, Map<String, Object>> pending = new HashMap<>();
        for (var p : repo.pending(u, fy)) pending.put(key(intOf(col(p, "DocumentTypeId")), intOf(col(p, "PrdHeaderId")), intOf(col(p, "PrdDetailId"))), p);
        Map<String, Double> savedQty = new HashMap<>();
        Map<String, Integer> savedPacking = new HashMap<>();
        Set<Integer> savedIds = new HashSet<>();
        if (!insert) for (var d : repo.details(r.Id)) {
            String k = key(intOf(col(d, "RefDocumentTypeId")), intOf(col(d, "RefDocNoId")), intOf(col(d, "RefDocDetailId")));
            savedQty.merge(k, dbl(col(d, "QtyIn")), Double::sum);
            savedPacking.put(k, intOf(col(d, "InvPackingTypeId")));
            savedIds.add(intOf(col(d, "InvStoreSendReceiptDetailId")));
        }
        Map<Integer, Object> items = new HashMap<>();
        for (var i : repo.items(u)) items.put(intOf(i.get("ItemId")), i.get("ItemName"));
        Set<Integer> conditions = new HashSet<>();
        for (var c : grn.itemConditions()) conditions.add(intOf(c.get("Id")));

        Map<String, Double> used = new LinkedHashMap<>();
        Map<String, Integer> recordOf = new HashMap<>();
        List<Map<String, Object>> details = new ArrayList<>();
        Map<Integer, Double> rateCache = new HashMap<>();
        for (int i = 0; i < lines.size(); i++) {
            var l = lines.get(i);
            if (l == null) throw new IllegalArgumentException("Grid Record Not Found");
            int rowNo = i + 1;
            String k = key(l.RefDocumentTypeId, l.RefDocNoId, l.RefDocDetailId);
            var p = pending.get(k);
            if (p == null && !savedQty.containsKey(k)) throw new IllegalArgumentException("Row#" + rowNo + " is not a pending filled-to-empty issue");
            if (l.Id != 0 && (insert || !savedIds.contains(l.Id))) throw new IllegalArgumentException("Detail row does not belong to this document");
            int packing = p != null ? intOf(col(p, "PackingtypeId")) : savedPacking.get(k);
            if (l.ItemId == 0) throw new IllegalArgumentException("Item Required In Row#" + rowNo + " In Detail Grid");
            if (!items.containsKey(l.ItemId)) throw new IllegalArgumentException("Select an item from the list in Row#" + rowNo);
            if (l.ItemConditionId == 0) throw new IllegalArgumentException("Item Condition Required In Row#" + rowNo + " In Detail Grid");
            if (!conditions.contains(l.ItemConditionId)) throw new IllegalArgumentException("Select an item condition from the list in Row#" + rowNo);
            if (packing == 0) throw new IllegalArgumentException("Packing Type Required In Row#" + rowNo + " In Detail Grid");
            BigDecimal qty = BigDecimal.valueOf(l.ItemQty);
            if (qty.signum() == 0) throw new IllegalArgumentException("Item Qty Required In Row#" + rowNo + " In Detail Grid");
            if (qty.signum() < 0) throw new IllegalArgumentException("Item Qty cannot be negative In Row#" + rowNo);
            int condition = l.ItemConditionId;
            double rate = rateCache.computeIfAbsent(l.ItemId * 1000 + condition, x -> rate(u, l.ItemId, condition, docDate, insert ? 0 : r.Id));
            if (rate == 0) throw new IllegalArgumentException("Avg Rate Required In Row#" + rowNo + " In Detail Grid");
            BigDecimal amount = qty.multiply(BigDecimal.valueOf(rate));
            if (amount.signum() == 0) throw new IllegalArgumentException("Amount Required In Row#" + rowNo + " In Detail Grid");
            used.merge(k, l.ItemQty, Double::sum);
            recordOf.putIfAbsent(k, l.RecordNo);
            // UOM with Equivalent 1 :561
            int uomId = 0;
            var uoms = repo.uomsByItem(u, l.ItemId);
            if (!uoms.isEmpty()) {
                var eq = uoms.stream().filter(m -> dbl(col(m, "Equivalent")) == 1d).findFirst().orElse(null);
                if (eq == null) throw new IllegalArgumentException("RateUom With Equivalent [1] is not define For Item : " + Objects.toString(items.get(l.ItemId), ""));
                uomId = intOf(col(eq, "Id"));
            }
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("InvStoreSendReceiptDetailId", l.Id);
            d.put("RefDocNoId", l.RefDocNoId);
            d.put("RefDocDetailId", l.RefDocDetailId);
            d.put("RefDocumentTypeId", l.RefDocumentTypeId);
            d.put("ItemId", l.ItemId);
            d.put("ItemConditionId", l.ItemConditionId);
            d.put("InvPackingTypeId", packing);
            d.put("QtyIn", qty);
            d.put("QtyOut", BigDecimal.ZERO);
            d.put("ItemRate", BigDecimal.valueOf(rate));
            d.put("ItemAmount", amount);
            d.put("SortNo", rowNo);
            d.put("ItemUomId", uomId);
            details.add(d);
        }
        // Insert :551 / Grd_CellUpdated :724 - a record's rows together cannot exceed its balance
        for (var e : used.entrySet()) {
            var p = pending.get(e.getKey());
            double bal = (p == null ? 0d : dbl(col(p, "BalanceQty"))) + savedQty.getOrDefault(e.getKey(), 0d);
            if (e.getValue() > bal + 1e-9) {
                throw new IllegalArgumentException("Total Qty of Record#" + recordOf.get(e.getKey()) + " Can't Be Greater Than Balance Qty Which Is " + cs(bal));
            }
        }

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("InvStoreSendReceiptId", insert ? 0 : r.Id);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchId", u.getBranchesId());
        h.put("FinancialYearId", fy);
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("ModifyUserId", u.getId());
        h.put("ApprovedDate", now);
        h.put("EntryUserId", u.getId());
        h.put("ApprovedUserId", u.getId());
        h.put("DocNo", docNo);
        h.put("DocDate", new Timestamp(docDate.getTime()));
        h.put("SenderWarehouseId", r.SenderWarehouseId);
        h.put("ReceiverWarehouseId", r.ReceiverWarehouseId);
        h.put("ScreenName", SCREEN);

        List<Map<String, Object>> newFiles = storeFiles(u, r);
        int id;
        try {
            int num = repo.execute(insert ? "dbo.USP_InvStoreSendReceipt_Insert" : "dbo.USP_InvStoreSendReceipt_Update", h);
            id = num > 0 ? num : r.Id;
            if (id <= 0) throw new IllegalArgumentException("Record could not be saved");
            for (var d : details) { d.put("InvStoreSendReceiptId", id); repo.execute("dbo.USP_InvStoreSendReceiptDetail_Insert", d); }
            attachments(u, id, r.SenderWarehouseId, r, newFiles);
            ProcExec.run(jdbc, "EXEC dbo.Sp_InventoryStockEvalautionDetail_Update @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?",
                    u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id);
            grn.inventoryTransactions(u, DOCUMENT_TYPE_ID, id);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", docNo);
        out.put("message", (insert ? "Record Save Successfully [" : "Record Update Successfully [") + docNo + "]");
        return out;
    }

    // ================================================================== files

    private List<Map<String, Object>> storeFiles(UserAccount u, StoreSendReceiptRequest r) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (r.files == null) return out;
        if (r.files.size() > 10) throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        for (var file : r.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(file);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("name", file.name);
            m.put("stored", store.store(u, file.name, bytes));
            m.put("size", bytes.length / 1048576d);
            out.add(m);
        }
        return out;
    }

    private void attachments(UserAccount u, int id, int senderId, StoreSendReceiptRequest r, List<Map<String, Object>> newFiles) {
        var existing = repo.attachments(u, id);
        for (Integer removed : new LinkedHashSet<>(r.removeAttachmentIds == null ? List.<Integer>of() : r.removeAttachmentIds)) {
            if (removed == null || existing.stream().noneMatch(a -> intOf(col(a, "Id")) == removed)) {
                throw new IllegalArgumentException("Attachment does not belong to this document");
            }
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?, @Activity=?", removed, "AttachmentDeleteById");
        }
        for (var f : newFiles) {
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Object[] v = {0, senderId, 0, DOCUMENT_TYPE_ID, id, f.get("name"), now, u.getId(), now, u.getId(),
                    u.getOrganizationId(), u.getCompanyId(), 0, SCREEN, false, f.get("stored"), f.get("size"), 0};
            ProcExec.run(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?, @RefAccountId=?, @DMSFoldersLabelsId=?, @RefDocumentTypeId=?, @RefDocumentNo=?, @Attachment=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @ScreenName=?, @DetailWiseAttachment=?, @UploadedFileCustomName=?, @UploadedFileSizeMb=?, @LineId=?", v);
        }
    }

    public DesktopInventoryItemFileService.Download attachment(int id, int attachmentId) {
        UserAccount u = user("View");
        repo.header(u, id);
        var row = repo.attachments(u, id).stream().filter(a -> intOf(col(a, "Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(col(row, "UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(col(row, "Attachment"), "");
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(col(row, "Attachment"), stored)),
                store.read(u, basename(stored)), "application/octet-stream");
    }

    // ================================================================== helpers

    private static String key(int type, int doc, int detail) { return type + ":" + doc + ":" + detail; }

    private String yearStart(UserAccount u) {
        var years = shared.years(u);
        if (years.isEmpty()) return "";
        Object s = col(years.get(0), "Start_Period");
        return s == null ? "" : String.valueOf(s).substring(0, Math.min(10, String.valueOf(s).length()));
    }

    static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return Long.toString((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    private static boolean blank(String v) { return v == null || v.isBlank(); }

    private static Date date(String v) {
        if (blank(v)) throw new IllegalArgumentException("Doc Date is not a valid date");
        try { return Date.valueOf(LocalDate.parse(v.trim().substring(0, 10))); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid date " + v); }
    }

    private static String basename(String name) {
        String n = name.replace('\\', '/');
        String r = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(r);
        return r;
    }
}
