package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StockAdjustmentPmDto;
import com.mst.repositories.StockAdjustmentPmRepository;
import com.mst.repositories.StockAdjustmentRepository;
import com.mst.repositories.StockTransferStoreRepository;
import com.mst.repositories.StoreDefineAssetsExtraRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StockAdjustmentPmRepository.DOC;
import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Packing Material (ModuleId 54) - screen 492 "Stock Adjustment For PM".
 *
 *   Desktop form : Architecture.WinApp.StoreManagement.StockAdjustmentForPM (ScreenName "StockAdjustmentForPM")
 *   DocumentTypeId 215, route /packing-material/stock-adjustment, API /api/packing-material/stock-adjustment
 *   BLL 0546 / DAL 0401 InvStockAdjustment (the 320 engine, see {@link StockAdjustmentService}),
 *   CommonServices.StockAdjutmentPackingMaterial215 (215_StockAdjustmentPMSlip.rpt), VoucherReport_118.
 *
 * With DocumentTypeId 215 the BLL never reads feature 5 (MakeVoucher :52) and the DAL skips the FIFO
 * branch (:104), so the save is: header Insert/Update, one Sp_InvStockAdjustmentDetail_Insert per row,
 * evaluation update, transactions recalc, USP_InventoryValidation per row (Loss), then the voucher.
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *  1. FIFOCgs is never assigned, so the Loss rate is always the average rate
 *     (GetAvgRateQtyAndStockInHand AvgRate, rounded to 3), never FIFO.
 *  2. Rows removed from an opened document are only removed from the grid: Sp_InvStockAdjustment_Update
 *     and the detail insert run for the rows that remain (the proc's own delete/re-insert rule applies).
 *  3. The voucher lines carry Comments "" (the BLL builds a text and never uses it).
 *  4. On insert the voucher head's ModifyUser is 0, on update its EntryUser is the signed-in user
 *     (MakeVoucher runs before the BLL zeroes the other user id).
 *  5. Financial effects on and an item without a GL row → the line is skipped; no line at all →
 *     "VoucherDetail List Not Found" and the save rolls back.
 *  6. History Doc No boxes are sent but the procedure never filters on them.
 *  7. ProjectsId is the user's branch (Insert :966).
 *
 * DEVIATIONS (web-only)
 *  A. DocNo is generated at save for a new document (the insert proc recomputes it anyway); an update
 *     keeps the stored number.
 *  B. A document is opened / updated / printed only when it is DocumentTypeId 215 of the user's
 *     organization and company.
 *  C. Attachments, grid layouts and the shortcut-key popup are not ported.
 */
@Service
public class StockAdjustmentPmService {

    public static final String SCREEN = "StockAdjustmentForPM";

    private final StockAdjustmentPmRepository pm;
    private final StockAdjustmentRepository repo;
    private final StockTransferStoreRepository store;
    private final StoreDefineAssetsExtraRepository accountsRepo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StockAdjustmentPmService(StockAdjustmentPmRepository pm, StockAdjustmentRepository repo,
                                    StockTransferStoreRepository store, StoreDefineAssetsExtraRepository accountsRepo,
                                    StoreIssuanceRepository shared, StoreScreenRights rights, CurrentUserContext ctx) {
        this.pm = pm; this.repo = repo; this.store = store; this.accountsRepo = accountsRepo;
        this.shared = shared; this.rights = rights; this.ctx = ctx;
    }

    // ================================================================================= load

    /** frmStockAdjustment_Load:233. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("docNo", pm.nextDocNo(u, ctx.currentFinancialYearId()));
        List<Map<String, Object>> items = new ArrayList<>();                     // ItemDtsFillFromGlobal: type 14
        for (Map<String, Object> r : store.items(u)) if (toInt(r.get("ItemTypeOfTypeId")) == 14) items.add(r);
        out.put("items", items);
        out.put("racks", store.racks(u, branch(u)));                               // racksWithWarehouseAndItems
        out.put("conditions", store.conditions());
        out.put("entryTypes", StockAdjustmentRepository.project(pm.entryTypes(), "Id", "Type"));
        out.put("packingMaterialDefaultWarehouse", toInt(shared.config(u, "PackingMaterialDefaultWarehouse")));
        return out;
    }

    public int docNo() { return pm.nextDocNo(ctx.requireAccountingUser(), ctx.currentFinancialYearId()); }

    /** CmbEntryType_Leave:428 - 1 → types 9,10; anything else → 9,11,12,13,14,20,21. */
    public List<Map<String, Object>> accounts(int entryTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        return entryTypeId == 1 ? accountsRepo.accountsByTypes(u, 9, 10)
                : accountsRepo.accountsByTypes(u, 9, 11, 12, 13, 14, 20, 21);
    }

    // ============================================================================ stock / rate

    /**
     * AvailableStock:1614 (QtyInHand of item/condition/warehouse/rack, rounded to 2; label hidden when no row)
     * and BindAvgRateForLossCase:1550 (Loss only: AvgRate without warehouse/rack, rounded to 3).
     */
    public Map<String, Object> stock(int recId, int itemId, int conditionId, int warehouseId, int rackId,
                                     int entryTypeId, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        if (recId > 0 && owned(u, recId) == null) recId = 0;
        Timestamp d = StoreIssuanceService.formDate(docDate, recId);
        int type = recId > 0 ? DOC : 0;
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> s = store.avgRateQtyAndStock(u, itemId, d, conditionId, recId, type, warehouseId, rackId);
        out.put("visible", s != null);
        out.put("stock", s == null ? 0d : round(toDouble(ci(s, "QtyInHand")), 2));
        if (entryTypeId == 2) {
            Map<String, Object> r = store.avgRateQtyAndStock(u, itemId, d, conditionId, recId, type, 0, 0);
            out.put("rate", r == null ? 0d : round(toDouble(ci(r, "AvgRate")), 3));
        }
        return out;
    }

    // ================================================================================= history

    /** gridhistoryfill:1142. */
    public List<Map<String, Object>> history(String from, String to, int fromDocNo, int toDocNo, int adjustmentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pm.formHistory(u, ctx.currentFinancialYearId(), viewAll, u.getId(),
                isBlank(from) ? null : StoreIssuanceService.pickerDate(from),
                isBlank(to) ? null : StoreIssuanceService.pickerDate(to), fromDocNo, toDocNo, adjustmentTypeId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("VoucherHeadId", toInt(ci(r, "VoucherHeadId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", ci(r, "DocDate"));
            o.put("Entrytype", ci(r, "EntryType"));
            o.put("EntryUser", ci(r, "EntryUser"));
            o.put("EntryDate", ci(r, "EnteryDate"));
            o.put("ModifyUser", ci(r, "ModifyUser"));
            o.put("ModifyDate", ci(r, "ModifyDate"));
            o.put("Remarks", ci(r, "RemarksHeader"));
            o.put("NoOfAttachments", ci(r, "NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById:1060 (and grdhistory_SelectionChanged:1305). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        int type = toInt(ci(h, "AdjustmentTypeId"));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("Item", str(ci(d, "ItemName")));
            o.put("WareHouseId", toInt(ci(d, "WarehouseId")));
            o.put("WareHouse", str(ci(d, "WareHouseName")));
            o.put("RackId", toInt(ci(d, "RackId")));
            o.put("RackName", str(ci(d, "rackName")));
            o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
            o.put("ItemCondition", str(ci(d, "ItemCondition")));
            o.put("ItemQty", toDouble(ci(d, "Qty")));
            o.put("ItemRate", toDouble(ci(d, "ItemRate")));
            o.put("ItemAmount", toDouble(ci(d, "Amount")));
            o.put("AccountId", type != 2 ? toInt(ci(d, "CreditAccountId")) : toInt(ci(d, "DebitAccountId")));
            o.put("AccountTitle", str(ci(d, "AccountTitle")));
            o.put("Comments", str(ci(d, "RemarksDetail")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("AdjustmentTypeId", type);
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("VoucherHeadId", shared.voucherHeadId(u, DOC, id));
        out.put("rows", rows);
        return out;
    }

    public boolean owns(int id) { return owned(ctx.requireAccountingUser(), id) != null; }

    // ==================================================================================== save

    /** btnsave_Click:1031 / btnupdate_Click:1044 → Insert():933 → BLL 0546 Save → DAL 0401 SetData. */
    @Transactional
    public Map<String, Object> save(StockAdjustmentPmDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Update because RecId Not Found");
        }
        List<StockAdjustmentPmDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");                 // :941
        int fy = ctx.currentFinancialYearId();
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : pm.nextDocNo(u, fy);
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");                   // :704
        int type = dto.AdjustmentTypeId;
        boolean typeInList = false;
        for (Map<String, Object> t : pm.entryTypes()) if (toInt(t.get("Id")) == type) typeInList = true;
        if (type == 0 || !typeInList) throw new IllegalArgumentException("EntryType Field is Required"); // :710
        if (existing != null && type != toInt(ci(existing, "AdjustmentTypeId")))                          // combo disabled on read
            throw new IllegalArgumentException("EntryType Field is Required");

        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> h = new LinkedHashMap<>();                            // Model 0977, declaration order
        h.put("IsApproved", false);
        h.put("ApprovedDate", now);
        h.put("DocDate", docDate);
        h.put("EnteryDate", now);
        h.put("ModifyDate", now);
        h.put("AdjustmentTypeId", type);
        h.put("ApprovalUserId", 0);
        h.put("BranchesId", branch(u));
        h.put("CompanyId", u.getCompanyId());
        h.put("DocNo", docNo);
        h.put("DocumentTypeId", DOC);
        h.put("EnteryUserId", u.getId());
        h.put("FinancialYearId", fy);
        h.put("Id", recId);
        h.put("ModifyUserId", recId > 0 ? u.getId() : 0);                        // :957 only on update
        h.put("OrganizationId", u.getOrganizationId());
        h.put("ProjectsId", branch(u));                                           // :966 (D7)
        h.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader);

        List<Map<String, Object>> details = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {                             // :979
            StockAdjustmentPmDto.Row r = rows.get(idx);
            int n = idx + 1;
            /* FormValidationDetail:719 - the grid can only hold rows that passed it. */
            if (r.ItemId == 0) throw new IllegalArgumentException("ItemName field is required (row " + n + ")");
            if (r.WareHouseId == 0) throw new IllegalArgumentException("WareHouseName field is required (row " + n + ")");
            if (r.RackId == 0) throw new IllegalArgumentException("Rack field is required (row " + n + ")");
            if (r.ItemConditionId == 0) throw new IllegalArgumentException("Item Condition field is required (row " + n + ")");
            if (!Double.isFinite(r.ItemQty) || r.ItemQty == 0d) throw new IllegalArgumentException("ItemQty field is required (row " + n + ")");
            if (!Double.isFinite(r.ItemRate) || r.ItemRate == 0d) throw new IllegalArgumentException("ItemRate field is required (row " + n + ")");
            if (r.AccountId == 0) throw new IllegalArgumentException("Account Title field is required (row " + n + ")");
            double amount = r.ItemQty * r.ItemRate;                               // CalculateAmount:1502
            if (amount == 0d) throw new IllegalArgumentException("ItemAmount field is required (row " + n + ")");
            Map<String, Object> m = new LinkedHashMap<>();                        // Model 0978, declaration order
            m.put("ItemRate", dec(r.ItemRate));
            m.put("NetWeight", dec(r.ItemQty));
            m.put("Qty", dec(r.ItemQty));
            m.put("Amount", amount);
            m.put("CreditAccountId", type == 2 ? 0 : r.AccountId);                // :996
            m.put("CropYearId", 0);
            m.put("DebitAccountId", type == 2 ? r.AccountId : 0);
            m.put("Id", r.Id);
            m.put("RefDocumentTypeId", 0);
            m.put("RefDocNoId", 0);
            m.put("RefDocSubIdNo", 0);
            m.put("InvStockAdjustmentId", 0);
            m.put("ItemId", r.ItemId);
            m.put("JobLotId", 0);
            m.put("PackingTypeId", 0);
            m.put("PackUomId", 0);
            m.put("RateUomId", 0);
            m.put("WarehouseId", r.WareHouseId);
            m.put("LineId", n);
            m.put("ItemConditionId", r.ItemConditionId);
            m.put("RackId", r.RackId);
            m.put("RemarksDetail", r.Comments == null ? "" : r.Comments);
            details.add(m);
        }
        List<Integer> accountIds = new ArrayList<>();
        for (Map<String, Object> a : accounts(type)) accountIds.add(toInt(a.get("Id")));
        for (StockAdjustmentPmDto.Row r : rows)
            if (!accountIds.contains(r.AccountId)) throw new IllegalArgumentException("Account Title field is required");

        boolean inactive = StoreIssuanceService.toBool(shared.config(u, "InventoryFinancialsEffectsInActive"));
        Voucher voucher = inactive ? null : makeVoucher(u, h, details);
        if (recId == 0) h.put("ModifyUserId", 0); else h.put("EnteryUserId", 0);
        int id = setData(u, h, details, recId == 0 ? "Sp_InvStockAdjustment_Insert" : "Sp_InvStockAdjustment_Update",
                inactive, voucher);
        Map<String, Object> ok = new LinkedHashMap<>();
        ok.put("success", true);
        ok.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo);
        ok.put("id", id);
        return ok;
    }

    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
    }

    /** BLL 0546 MakeVoucher with DocumentTypeId 215 (flag = false: no FIFO). */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details) {
        Voucher v = new Voucher();
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.DocumentTypeId = DOC;
        vh.DocumentTypeSrNo = toInt(h.get("Id"));
        vh.RefDocNoId = toInt(h.get("Id"));
        vh.VoucherCode = toInt(h.get("DocNo"));
        vh.VoucherDate = String.valueOf(h.get("DocDate"));
        vh.Remarks = str(h.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = toInt(h.get("BranchesId"));
        vh.ProjectId = toInt(h.get("ProjectsId"));
        vh.ManualBillNo = String.valueOf(toInt(h.get("DocNo")));
        vh.DueDate = now;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(h.get("FinancialYearId"));
        vh.EntryUser = toInt(h.get("EnteryUserId"));
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = toInt(h.get("ModifyUserId"));
        v.head = vh;
        List<Map<String, Object>> gl = shared.itemGlAccounts(u);
        if (details.isEmpty()) return v;
        int type = toInt(h.get("AdjustmentTypeId"));
        int branch = toInt(h.get("BranchesId"));
        double sum = 0d;
        int line = 1;
        for (Map<String, Object> item : details) {
            if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            Map<String, Object> g = find(gl, toInt(item.get("ItemId")));
            if (g == null) continue;
            int purchase = toInt(g.get("PurchaseGLAC"));
            int itemId = toInt(item.get("ItemId"));
            double amount = toDouble(item.get("Amount"));
            double qty = toDouble(item.get("Qty")), wt = toDouble(item.get("NetWeight")), rate = toDouble(item.get("ItemRate"));
            if (type == 1) {
                int cr = toInt(item.get("CreditAccountId"));
                ContraVoucherDto.Detail a = line(line, purchase, cr, branch, itemId, rate, amount);
                a.DebitAmount = amount; a.QtyIn = qty; a.WeightIn = wt;
                ContraVoucherDto.Detail b = line(line, cr, purchase, branch, itemId, rate, amount);
                b.CreditAmount = amount; b.QtyIn = qty; b.WeightIn = wt;
                v.lines.add(a); v.lines.add(b);
                line++;
                sum += amount;
            }
            if (type == 2) {
                int dr = toInt(item.get("DebitAccountId"));
                ContraVoucherDto.Detail a = line(line, dr, purchase, branch, itemId, rate, amount);
                a.DebitAmount = amount; a.QtyOut = qty; a.WeightOut = wt;
                ContraVoucherDto.Detail b = line(line, purchase, dr, branch, itemId, rate, amount);
                b.CreditAmount = amount; b.QtyOut = qty; b.WeightOut = wt;
                v.lines.add(a); v.lines.add(b);
                sum += amount;
                line++;
            }
        }
        vh.VoucherAmount = sum;
        vh.BillAmount = sum;
        return v;
    }

    private static ContraVoucherDto.Detail line(int lineId, int account, int against, int branch, int itemId,
                                                double rate, double amount) {
        ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
        l.LineId = lineId;
        l.AccountId = account;
        l.AgainstAccountId = against;
        l.Comments = "";
        l.ItemId = itemId;
        l.ItemRate = rate;
        l.ItemAmount = amount;
        l.BranchesId = branch;
        return l;
    }

    /** DAL 0401 SetData, DocumentTypeId 215 (the FIFO branch at :104 is never taken). */
    private int setData(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details, String proc,
                        boolean inactive, Voucher voucher) {
        int recId = toInt(h.get("Id"));
        int num3 = repo.setProc(proc, h);
        if (num3 > 0) h.put("Id", num3); else num3 = recId;
        if (num3 <= 0) throw new IllegalStateException("Save returned no document id.");
        int id = num3;
        int line = 1;
        for (Map<String, Object> d : details) {
            d.put("InvStockAdjustmentId", id);
            d.put("LineId", line);
            d.put("Id", repo.setProc("Sp_InvStockAdjustmentDetail_Insert", d));
            line++;
        }
        int type = toInt(h.get("AdjustmentTypeId"));
        Timestamp docDate = (Timestamp) h.get("DocDate");
        pm.evaluationUpdate(u, id);
        pm.transactionsRecalc(u, id);
        if (type == 2) for (Map<String, Object> d : details) pm.inventoryValidation(u, docDate, d);
        if (!inactive) writeVoucher(u, id, h, details, voucher, type);
        return id;
    }

    /** DAL 0401 :258-300 - voucher head insert/update, details, H mirrors. */
    private void writeVoucher(UserAccount u, int id, Map<String, Object> h, List<Map<String, Object>> details,
                              Voucher v, int type) {
        ContraVoucherDto.Head vh = v.head;
        int existing = shared.voucherHeadId(u, DOC, id);
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        int num2 = shared.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num2 > 0) vh.Id = num2;
        if (v.lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail l : v.lines) {
            l.VoucherHeadId = vh.Id;
            l.BranchesId = toInt(h.get("BranchesId"));
            for (Map<String, Object> d : details) {
                int ln = toInt(d.get("LineId"));
                if (ln == i(l.LineId) && ln > 0) { l.RefDocSubIdNo = toInt(d.get("Id")); break; }
            }
            l.IsCGS = type == 2 ? 1 : 0;
            shared.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        int ref = shared.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : v.lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = ref;
            shared.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
    }

    // ================================================================================= helpers

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOC) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static Map<String, Object> find(List<Map<String, Object>> gl, int itemId) {
        for (Map<String, Object> x : gl) if (toInt(x.get("Id")) == itemId) return x;
        return null;
    }

    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                String n = f.getName();
                if (v != null && (n.equals("VoucherDate") || n.equals("ChequeDate") || n.equals("DueDate")
                        || n.equals("EntryDate") || n.equals("ModifyDate") || n.equals("PostDate")
                        || n.equals("DCheqDate") || n.equals("GpDate"))) {
                    String s = String.valueOf(v);
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(n, v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    private static BigDecimal dec(double v) {
        return v == 0d ? BigDecimal.ZERO : new BigDecimal(v).round(new MathContext(15));
    }

    private static double round(double v, int p) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0d;
        return new BigDecimal(v).setScale(p, RoundingMode.HALF_EVEN).doubleValue();
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
}
