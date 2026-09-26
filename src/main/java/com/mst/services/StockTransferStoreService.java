package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StockTransferStoreDto;
import com.mst.repositories.StockTransferStoreRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.StoreStockTransferRepository;
import com.mst.security.CurrentUserContext;
import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Packing Material (module 54) - screen 505 "Stock Transfer Store".
 *
 * <ul>
 *   <li>ScreenName (base.Name): {@code frmStockTransferStore}; DocumentTypeId 807; TransferType always "MoveOrder".</li>
 *   <li>Desktop form Architecture.WinApp.StoreManagement/frmStockTransferStore.cs (logic :303-3163).</li>
 *   <li>BLL 0559 InvStockTransferHeader (GenerateCode, FormHistory, GetByID, Save/MakeVoucher,
 *       StockTransferPackingMaterialAndStore_SlipandRegister); DAL 0412 SetData - for 807 the FIFO branch never
 *       runs (flag3 is only read when DocumentTypeId != 807) and the 806 row-removal never runs.</li>
 * </ul>
 *
 * DESKTOP BEHAVIOUR REPRODUCED (operator decision):
 *  1. Rows removed from the grid of an opened document are never deleted: Sp_InvStockTransferHeader_Update deletes
 *     only the expenses, and USP_InvStockTransferDetailRowsDeleteByIds runs for 806 only (DAL 0412:333).
 *  2. The Ref Document lists are read with DocumentTypeId 245 (Purchase Invoice Direct PM), not 807 (:697).
 *  3. Expense amounts are shared by QTY (OtherExpensesProportion:1086) but the voucher charges each row's share to
 *     the item's purchase GL against itself (AgainstAccountId = AccountId, BLL 0559:80).
 *  4. btnprint and the print preview check use the Update right, not Print (:314-318).
 */
@Service
public class StockTransferStoreService {

    public static final int DOC = 807;
    public static final String SCREEN = "frmStockTransferStore";
    private static final int FIFO_FEATURE = 5;
    private static final int MULTI_BRANCH_FEATURE = 11;

    private final StockTransferStoreRepository own;
    private final StoreStockTransferRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StockTransferStoreService(StockTransferStoreRepository own, StoreStockTransferRepository repo,
                                     StoreIssuanceRepository shared, StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.repo = repo; this.shared = shared; this.rights = rights; this.ctx = ctx;
    }

    private boolean financialEffect(UserAccount u) {                                              // Load:326-334
        return repo.erpFeature(u, FIFO_FEATURE) || toBool(shared.config(u, "StockTransferFinancialEffectIsActive"));
    }

    private int amountDigits(UserAccount u) {
        int n = toInt(shared.config(u, "Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    // ================================================================================= load

    /** PurchsaeOrder_Load:303. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        boolean fifo = repo.erpFeature(u, FIFO_FEATURE);
        boolean cgsEntryAllow = toBool(shared.config(u, "CGSEntryAllow"));
        boolean multi = repo.erpFeature(u, MULTI_BRANCH_FEATURE);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("financialEffect", financialEffect(u));
        out.put("rateEditable", !fifo && !cgsEntryAllow);                                         // :347
        out.put("cgsEntryAllow", cgsEntryAllow);
        out.put("multiBranch", multi);
        out.put("branches", multi ? own.branchesAllocatedToUser(u) : List.of());
        out.put("userBranchId", branch(u));
        out.put("racks", own.racks(u, branch(u)));                                                // racksWithWarehouseAndItems
        out.put("items", own.items(u));
        out.put("uoms", own.uoms(u, 0));
        out.put("conditions", own.conditions());
        out.put("refDocumentTypes", own.refDocumentTypes());
        out.put("accounts", accounts(u));
        out.put("amountDecimals", amountDigits(u));
        out.put("packingMaterialDefaultWarehouse", toInt(shared.config(u, "PackingMaterialDefaultWarehouse")));
        out.put("defaultWarehouseForStoreFlow", toInt(shared.config(u, "DefaultWarehouseForStoreFlow")));
        int days = toInt(shared.config(u, "DefaultDaysToLessFromHistoryFromDate"));
        out.put("historyDays", days > 0 ? days : 3);
        out.put("docNo", own.generateCode(u, DOC, fy, branch(u)));
        return out;
    }

    public int docNo() {
        UserAccount u = ctx.requireAccountingUser();
        return own.generateCode(u, DOC, ctx.currentFinancialYearId(), branch(u));
    }

    /** CmbFromBranch_Leave / CmbToBranch_Leave - the racks of that branch. */
    public List<Map<String, Object>> branchRacks(int branchId) {
        UserAccount u = ctx.requireAccountingUser();
        if (own.branchesAllocatedToUser(u).stream().noneMatch(b -> toInt(b.get("Id")) == branchId)) return List.of();
        return own.racks(u, branchId);
    }

    /** AccountsFill:1016 - AccountTypeId not in 2, 11, 12, 15. */
    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.coaAllocation(u)) {
            int t = toInt(r.get("AccountTypeId"));
            if (t == 2 || t == 11 || t == 12 || t == 15) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("AccountTitle", str(r.get("AccountTitle")));
            out.add(o);
        }
        return out;
    }

    /** AvailableStockGetByItem:2367 and GetAvgRate:2393. */
    public Map<String, Object> stock(int recId, int itemId, int conditionId, int warehouseId, int rackId, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        if (recId > 0 && owned(u, recId) == null) recId = 0;
        Timestamp d = StoreIssuanceService.formDate(docDate, recId);
        int type = recId > 0 ? DOC : 0;
        Map<String, Object> s = own.avgRateQtyAndStock(u, itemId, d, conditionId, recId, type, warehouseId, rackId);
        Map<String, Object> r = own.avgRateQtyAndStock(u, itemId, d, conditionId, recId, type, 0, 0);
        double stock = s == null ? 0d : BigDecimal.valueOf(toDouble(ci(s, "QtyInHand"))).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
        double rate = r == null ? 0d : BigDecimal.valueOf(toDouble(ci(r, "AvgRate"))).setScale(3, RoundingMode.HALF_EVEN).doubleValue();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("availableStock", stock > 0 ? stock : 0d);
        out.put("avgRate", rate > 0 ? rate : 0d);
        return out;
    }

    private double avgRate(UserAccount u, int itemId, int conditionId, Timestamp d, int recId) {
        Map<String, Object> r = own.avgRateQtyAndStock(u, itemId, d, conditionId, recId, recId > 0 ? DOC : 0, 0, 0);
        double rate = r == null ? 0d : BigDecimal.valueOf(toDouble(ci(r, "AvgRate"))).setScale(3, RoundingMode.HALF_EVEN).doubleValue();
        return rate > 0 ? rate : 0d;
    }

    public List<Map<String, Object>> refDocs(int refDocumentTypeId, int recId) {
        UserAccount u = ctx.requireAccountingUser();
        if (recId > 0 && owned(u, recId) == null) recId = 0;
        return own.refDocs(u, refDocumentTypeId, recId);
    }

    // ================================================================================= history

    /** gridhistoryfill:1994 - BranchesId = the user's branch. */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Timestamp f = isBlank(from) ? null : StoreIssuanceService.pickerDate(from);
        Timestamp t = isBlank(to) ? null : StoreIssuanceService.pickerDate(to);
        String k = dateType == null ? "doc" : dateType;
        List<Map<String, Object>> rows = repo.formHistory(u, DOC, viewAll, ctx.currentFinancialYearId(), branch(u), u.getId(),
                "doc".equals(k) ? f : null, "doc".equals(k) ? t : null,
                "entry".equals(k) ? f : null, "entry".equals(k) ? t : null,
                "modify".equals(k) ? f : null, "modify".equals(k) ? t : null,
                "approved".equals(k) ? f : null, "approved".equals(k) ? t : null,
                fromDocNo, toDocNo, "");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocDate", ci(r, "DocDate"));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("TransType", ci(r, "TransferType"));
            o.put("FromBranchName", ci(r, "FromBranchName"));
            o.put("ToBranchName", ci(r, "ToBranchName"));
            o.put("Remarks", ci(r, "RemarksHeader"));
            o.put("EntryDate", ci(r, "EntryDate"));
            o.put("EntryUser", ci(r, "EntryUserName"));
            o.put("ModifyDate", ci(r, "ModifyDate"));
            o.put("ModifyUser", ci(r, "ModifyUserName"));
            o.put("NoOfAttachments", ci(r, "NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= read

    /** ReadById:1743 (GetDetailByHeaderId:2213 uses the same rows). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        List<Map<String, Object>> details = repo.details(id);
        if (details.isEmpty()) return null;                                                          // "Record not found"
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("FromBranchId", toInt(ci(h, "FromBranchId")));
        out.put("ToBranchId", toInt(ci(h, "ToBranchId")));
        out.put("IsApproved", toBool(str(ci(h, "IsApproved"))));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("WareHouseFromId", toInt(ci(d, "FromWarehouseId")));
            o.put("WareHouseFrom", str(ci(d, "FromWareHouseName")));
            o.put("RackFromId", toInt(ci(d, "RackId")));
            o.put("RackFrom", str(ci(d, "rackName")));
            o.put("WareHouseToId", toInt(ci(d, "ToWarehouseId")));
            o.put("WareHouseTo", str(ci(d, "ToWareHouseName")));
            o.put("RackToId", toInt(ci(d, "ToRackId")));
            o.put("RackTo", str(ci(d, "TorackName")));
            o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
            o.put("ItemCondition", str(ci(d, "ItemCondition")));
            o.put("PackUOMId", toInt(ci(d, "PackUomId")));
            o.put("PackUOM", str(ci(d, "PackUom")));
            o.put("QTY", toDouble(ci(d, "Qty")));
            o.put("ItemRate", toDouble(ci(d, "ItemRate")));
            o.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
            o.put("Expense", toDouble(ci(d, "ExpenseAmount")));
            o.put("RefDocumentTypeId", toInt(ci(d, "RefDocumentTypeId")));
            o.put("RefDocumentType", str(ci(d, "RefDocumentType")));
            o.put("RefDocId", toInt(ci(d, "RefDocNoId")));
            o.put("RefDocNo", str(ci(d, "RefDocNo")));
            o.put("RefDocInvoiceId", toInt(ci(d, "RefDocInvoiceId")));
            o.put("RefDocInvoiceNo", str(ci(d, "RefDocInvoiceNo")));
            o.put("Remarks", str(ci(d, "RemarksDetail")));
            rows.add(o);
        }
        out.put("rows", rows);
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : repo.expenses(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Account", toInt(ci(e, "AccountId")));
            o.put("Percentage", toDouble(ci(e, "Percentage")));
            o.put("Qty", toDouble(ci(e, "Qty")));
            o.put("Rate", toDouble(ci(e, "Rate")));
            o.put("Amount", toDouble(ci(e, "Amount")));
            o.put("Remarks", str(ci(e, "Remarks")));
            exp.add(o);
        }
        out.put("expenses", exp);
        return out;
    }

    // ================================================================================= delete

    /** btnDelete_Click:3062 -> InvPurchaseInvoice.RemoveByID (807). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0 || owned(u, id) == null) throw new IllegalArgumentException("RecordId Not Found.....");
        shared.removeInvoiceVoucherAndStock(u, DOC, id, u.getId());
        return ok("Delete Record Successfully", id);
    }

    // ================================================================================= save

    /** btnsave_Click / btnupdate_Click -> Insert():1509 -> BLL 0559 Save -> DAL 0412 SetData. */
    @Transactional
    public Map<String, Object> save(StockTransferStoreDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record not found");
            if (toBool(str(ci(existing, "IsApproved")))) throw new IllegalArgumentException("Approved Record Can't Update");  // :1728
        }
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");       // :1513
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : own.generateCode(u, DOC, fy, branch(u));
        boolean multi = repo.erpFeature(u, MULTI_BRANCH_FEATURE);
        boolean fin = financialEffect(u);
        boolean rateEditable = !repo.erpFeature(u, FIFO_FEATURE) && !toBool(shared.config(u, "CGSEntryAllow"));
        int dp = amountDigits(u);

        /* FormValidation:909 */
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        List<Map<String, Object>> branches = multi ? own.branchesAllocatedToUser(u) : List.of();
        int fromBranch = 0, toBranch = 0;
        if (multi) {
            fromBranch = dto.FromBranchId == null ? 0 : dto.FromBranchId;
            toBranch = dto.ToBranchId == null ? 0 : dto.ToBranchId;
            final int fb = fromBranch, tb = toBranch;
            if (fb == 0 || branches.stream().noneMatch(b -> toInt(b.get("Id")) == fb)) throw new IllegalArgumentException("From Branch Field is Required");
            if (tb == 0 || branches.stream().noneMatch(b -> toInt(b.get("Id")) == tb)) throw new IllegalArgumentException("To Branch Field is Required");
            if (fb == tb) throw new IllegalArgumentException("From Branch And To Branch can't be same");
        }

        /* :1540 other charges */
        List<StockTransferStoreDto.Expense> exps = dto.expenses == null ? new ArrayList<>() : dto.expenses;
        Set<Integer> accountIds = new HashSet<>();
        for (Map<String, Object> a : accounts(u)) accountIds.add(toInt(a.get("Id")));
        double creditForExpense = 0;
        for (StockTransferStoreDto.Expense r : exps) {
            if (r == null) throw new IllegalArgumentException("Invalid other-charges row");
            if (d(r.Amount) < 0) throw new IllegalArgumentException("Other charges amount cannot be negative");
            creditForExpense += d(r.Amount);
            if (i(r.Account) != 0 && !accountIds.contains(i(r.Account))) throw new IllegalArgumentException("Select an account from the list");
            if (!(d(r.Amount) > 0.0)) continue;
            if (i(r.Account) == 0) throw new IllegalArgumentException("Please Select an Account Against OtherCharges Amount First");
            if (r.Remarks == null || r.Remarks.isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }

        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> sh = new LinkedHashMap<>();                                             // Model 1009 order
        sh.put("Id", recId);
        sh.put("DocNo", docNo);
        sh.put("DocDate", docDate);
        sh.put("GatePassId", 0);
        sh.put("WbTicketId", 0);
        sh.put("WbNetWeight", 0d);
        sh.put("OtherWeight", 0d);
        sh.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader);
        sh.put("TransferType", "MoveOrder");
        sh.put("EntryDate", now);
        sh.put("EntryUser", u.getId());
        sh.put("ModifyDate", now);
        sh.put("ModifyUser", u.getId());
        sh.put("IsApproved", false);
        sh.put("ApprovedUserId", u.getId());
        sh.put("ApprovedDate", now);
        sh.put("TransitWarehouseId", 0);
        sh.put("RefDocumentTypeId", 0);
        sh.put("OrganizationId", u.getOrganizationId());
        sh.put("CompanyId", u.getCompanyId());
        sh.put("BranchesId", branch(u));
        sh.put("ProjectsId", branch(u));
        sh.put("DocumentTypeId", DOC);
        sh.put("FinancialYearId", fy);
        sh.put("NoOfBranches", multi ? branches.size() : 0);
        sh.put("FromBranchId", fromBranch);
        sh.put("ToBranchId", toBranch);

        Set<Integer> ownDetailIds = new HashSet<>();
        if (recId != 0) for (Map<String, Object> d0 : repo.details(recId)) ownDetailIds.add(toInt(ci(d0, "Id")));
        List<Map<String, Object>> fromRacks = own.racks(u, multi ? fromBranch : branch(u));
        List<Map<String, Object>> toRacks = multi ? own.racks(u, toBranch) : fromRacks;
        Set<Integer> conditions = new HashSet<>();
        for (Map<String, Object> c : own.conditions()) conditions.add(toInt(c.get("Id")));
        Set<Integer> items = new HashSet<>();
        for (Map<String, Object> it : own.items(u)) items.add(toInt(it.get("Id")));
        List<Map<String, Object>> uoms = own.uoms(u, 0);
        Set<Integer> refTypes = new HashSet<>();
        for (Map<String, Object> t : own.refDocumentTypes()) refTypes.add(toInt(ci(t, "Id")));

        double totalQty = 0;
        for (StockTransferStoreDto.Row r : dto.rows) if (r != null) totalQty += d(r.QTY);

        /* Insert:1604 detail loop */
        List<Map<String, Object>> details = new ArrayList<>();
        Map<Integer, List<Map<String, Object>>> refCache = new HashMap<>();
        int n = 0;
        for (StockTransferStoreDto.Row r : dto.rows) {
            n++;
            if (r == null) throw new IllegalArgumentException("Grid Record Not Found");
            int detailId = recId != 0 ? i(r.Id) : 0;
            if (detailId != 0 && !ownDetailIds.contains(detailId)) throw new IllegalArgumentException("Detail row " + detailId + " does not belong to this document.");
            if (i(r.WareHouseFromId) == 0) throw new IllegalArgumentException("FromWarehouse Field is Required.in Detail Grid Row No : " + n);
            if (i(r.WareHouseToId) == 0) throw new IllegalArgumentException("WarehouseTo Field is Required.in Detail Grid Row No : " + n);
            if (i(r.RackFromId) == 0) throw new IllegalArgumentException("Rack From Field is Required.in Detail Grid Row No : " + n);
            if (i(r.RackToId) == 0) throw new IllegalArgumentException("Rack To Field is Required.in Detail Grid Row No : " + n);
            if (i(r.ItemId) == 0) throw new IllegalArgumentException("ItemId not found");
            if (i(r.PackUOMId) == 0) throw new IllegalArgumentException("Pack Uom  not Found in Detail Grid Row No : " + n);
            if (i(r.ItemConditionId) == 0) throw new IllegalArgumentException("Item Condition  not Found in Detail Grid Row No : " + n);
            if ((int) Math.rint(d(r.QTY)) == 0) throw new IllegalArgumentException("To QTY not Found in Detail Grid Row No : " + n);
            if (d(r.QTY) < 0) throw new IllegalArgumentException("Qty cannot be negative in Detail Grid Row No : " + n);
            // the lists the combos were built from (BindWarehouses / BindRacks / ItemConditionBindFromGlobal / PackUOM)
            if (!items.contains(i(r.ItemId))) throw new IllegalArgumentException("Select an item from the list in Detail Grid Row No : " + n);
            if (!rackOf(fromRacks, i(r.ItemId), i(r.WareHouseFromId), i(r.RackFromId))) throw new IllegalArgumentException("Rack From does not belong to the item and warehouse in Detail Grid Row No : " + n);
            if (!rackOf(toRacks, i(r.ItemId), i(r.WareHouseToId), i(r.RackToId))) throw new IllegalArgumentException("Rack To does not belong to the item and warehouse in Detail Grid Row No : " + n);
            if (i(r.RackFromId) == i(r.RackToId)) throw new IllegalArgumentException("Rack From and Rack To cannot be the same. Please select a different rack.");
            if (!conditions.contains(i(r.ItemConditionId))) throw new IllegalArgumentException("Select an item condition from the list in Detail Grid Row No : " + n);
            final int itemId = i(r.ItemId), uomId = i(r.PackUOMId);
            if (uoms.stream().noneMatch(m -> toInt(m.get("Id")) == uomId && toInt(m.get("ItemId")) == itemId)) throw new IllegalArgumentException("PackUOM field is required");
            // GetAvgRate / AvgAmountCalculation: rate is the average unless the box is editable
            double rate = rateEditable ? d(r.ItemRate) : avgRate(u, itemId, i(r.ItemConditionId), docDate, recId);
            if (rate < 0) throw new IllegalArgumentException("AvgRate cannot be negative");
            double amount = d(r.QTY) > 0 && rate > 0 ? BigDecimal.valueOf(d(r.QTY) * rate).setScale(dp, RoundingMode.HALF_UP).doubleValue() : 0d;
            if (fin) {
                if (rate <= 0.0) throw new IllegalArgumentException("AvgRate field Required.in Detail Grid Row No : " + n);
                if (amount <= 0.0) throw new IllegalArgumentException("Amount field Required.in Detail Grid Row No : " + n);
            }
            int rdt = i(r.RefDocumentTypeId), rdoc = rdt > 0 ? i(r.RefDocId) : 0, rinv = rdt > 0 ? i(r.RefDocInvoiceId) : 0;
            if (rdt > 0) {
                if (!refTypes.contains(rdt)) throw new IllegalArgumentException("Select a document type from the list");
                if (rdoc == 0 && rinv == 0) throw new IllegalArgumentException("RefDocNo or RefDocInvoiceNo field Required in Detail grid Row No : " + n);
                final int rd = rdoc, ri = rinv;
                var refs = refCache.computeIfAbsent(rdt, t -> own.refDocs(u, t, recId));
                if (rd > 0 && refs.stream().noneMatch(x -> toInt(ci(x, "RefDocId")) == rd)) throw new IllegalArgumentException("Select a reference document from the list");
                if (ri > 0 && refs.stream().noneMatch(x -> toInt(ci(x, "RefDocInvoiceId")) == ri)) throw new IllegalArgumentException("Select a reference invoice from the list");
            }
            Map<String, Object> vd = detailModel();
            vd.put("Id", detailId);
            vd.put("ItemId", itemId);
            vd.put("FromWarehouseId", i(r.WareHouseFromId));
            vd.put("RackId", i(r.RackFromId));
            vd.put("ToWarehouseId", i(r.WareHouseToId));
            vd.put("ToRackId", i(r.RackToId));
            vd.put("ItemConditionId", i(r.ItemConditionId));
            vd.put("PackUomId", uomId);
            vd.put("Qty", d(r.QTY));
            vd.put("GrossWeight", d(r.QTY));
            vd.put("NetWeight", d(r.QTY));
            vd.put("ItemRate", rate);
            vd.put("ItemAmount", amount);
            vd.put("ExpenseAmount", creditForExpense > 0 ? creditForExpense / totalQty * d(r.QTY) : 0d);   // OtherExpensesProportion:1086
            vd.put("RefDocumentTypeId", rdt);
            vd.put("RefDocNoId", rdoc);
            vd.put("RefDocInvoiceId", rinv);
            vd.put("RemarksDetail", r.Remarks == null ? "" : r.Remarks);
            details.add(vd);
        }
        List<Map<String, Object>> expenses = new ArrayList<>();
        for (StockTransferStoreDto.Expense r : exps) {
            if (i(r.Account) == 0) continue;
            Map<String, Object> pf = new LinkedHashMap<>();
            pf.put("Amount", d(r.Amount));
            pf.put("Percentage", d(r.Percentage));
            pf.put("Qty", d(r.Qty));
            pf.put("Rate", d(r.Rate));
            pf.put("AccountId", i(r.Account));
            pf.put("Id", 0);
            pf.put("InvStockTransferHeaderId", 0);
            pf.put("Remarks", r.Remarks == null ? "" : r.Remarks);
            expenses.add(pf);
        }

        Voucher voucher = makeVoucher(u, sh, details, expenses);                                    // BLL Save:167
        String proc;
        if (recId == 0) { sh.put("ModifyUser", 0); proc = "Sp_InvStockTransferHeader_Insert"; }
        else { sh.put("EntryUser", 0); proc = "Sp_InvStockTransferHeader_Update"; }
        int id = setData(u, sh, details, expenses, voucher, proc);
        return ok(recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo, id);
    }

    private static boolean rackOf(List<Map<String, Object>> racks, int itemId, int warehouseId, int rackId) {
        for (Map<String, Object> r : racks) {
            if (toInt(r.get("Id")) == rackId && toInt(r.get("ItemId")) == itemId && toInt(r.get("WarehouseId")) == warehouseId) return true;
        }
        return false;
    }

    /** BLL 0559 MakeVoucher:80. */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details, List<Map<String, Object>> expenses) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOC;
        vh.DocumentTypeSrNo = toInt(obj.get("Id"));
        vh.RefDocNoId = toInt(obj.get("Id"));
        vh.VoucherCode = toInt(obj.get("DocNo"));
        vh.VoucherDate = String.valueOf(obj.get("DocDate"));
        vh.Remarks = str(obj.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = toInt(obj.get("BranchesId"));
        vh.ProjectId = toInt(obj.get("ProjectsId"));
        vh.ManualBillNo = "0";                                                                     // WorkingReportNo never set
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.DueDate = now;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(obj.get("FinancialYearId"));
        vh.EntryUser = toInt(obj.get("EntryUser"));
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = toInt(obj.get("ModifyUser"));
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        List<Map<String, Object>> gl = shared.itemGlAccounts(u);
        if (!details.isEmpty() && !expenses.isEmpty()) {
            double total = 0.0;
            for (Map<String, Object> item : details) {
                if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
                Map<String, Object> g = null;
                for (Map<String, Object> x : gl) if (toInt(ci(x, "Id")) == toInt(item.get("ItemId"))) { g = x; break; }
                if (g == null) continue;
                String c = "Item: " + str(ci(g, "ItemName")) + ",   Qty: " + clr(toDouble(item.get("Qty")))
                        + ",   Weight: " + clr(toDouble(item.get("NetWeight"))) + ",   Rate:" + clr(toDouble(item.get("ItemRate")))
                        + "  Item Amount: " + clr(toDouble(item.get("ItemAmount"))) + "  ExpenseAmount: " + clr(toDouble(item.get("ExpenseAmount")));
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = toInt(ci(g, "PurchaseGLAC"));
                l.AgainstAccountId = l.AccountId;
                l.Comments = c;
                l.DebitAmount = toDouble(item.get("ExpenseAmount"));
                l.ItemId = toInt(item.get("ItemId"));
                l.QtyIn = toDouble(item.get("Qty"));
                l.ItemRate = toDouble(item.get("ItemRate"));
                l.WeightIn = toDouble(item.get("NetWeight"));
                l.ItemAmount = toDouble(item.get("ItemAmount"));
                l.BranchesId = toInt(obj.get("BranchesId"));
                lines.add(l);
            }
            for (Map<String, Object> e : expenses) {
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = toInt(e.get("AccountId"));
                l.AgainstAccountId = l.AccountId;
                l.Comments = str(e.get("Remarks"));
                l.QtyIn = toDouble(e.get("Qty"));
                l.ItemRate = toDouble(e.get("Rate"));
                l.CreditAmount = toDouble(e.get("Amount"));
                l.BranchesId = toInt(obj.get("BranchesId"));
                total += toDouble(e.get("Amount"));
                lines.add(l);
            }
            vh.VoucherAmount = total;
            vh.BillAmount = total;
        }
        Voucher v = new Voucher();
        v.head = vh;
        v.lines = lines;
        return v;
    }

    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines;
    }

    /** DAL 0412 SetData for 807 - no FIFO branch (flag3 stays false), no 806 row removal. */
    private int setData(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details,
                        List<Map<String, Object>> expenses, Voucher v, String procName) {
        ContraVoucherDto.Head voucher = v.head;
        int recId = toInt(obj.get("Id"));
        int num = repo.setProc(procName, obj);
        if (num > 0) obj.put("Id", num); else num = recId;
        if (num <= 0) throw new IllegalStateException("Save returned no document id.");
        int line = 0;
        for (Map<String, Object> d : details) d.put("LineId", ++line);
        for (Map<String, Object> item2 : details) {                                                // :144
            item2.put("InvStockTransferHeaderId", num);
            item2.put("Id", repo.setProc("Sp_InvStockTransferDetail_Insert", item2));
        }
        for (Map<String, Object> e : expenses) {                                                   // :178
            e.put("InvStockTransferHeaderId", num);
            repo.setProc("Sp_InvStockTransferExpense_Insert", e);
        }
        repo.evaluationUpdate(u, DOC, num);                                                        // :266
        if (!expenses.isEmpty() && !toBool(shared.config(u, "InventoryFinancialsEffectsInActive"))) {  // :273
            int existing = shared.voucherHeadId(u, DOC, num);
            if (existing > 0) voucher.Id = existing;
            voucher.DocumentTypeSrNo = num;
            voucher.RefDocNoId = num;
            int num3 = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(voucher));
            if (num3 > 0) voucher.Id = num3;
            List<ContraVoucherDto.Detail> lines = v.lines;
            if (lines == null || lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
            for (ContraVoucherDto.Detail l : lines) { l.VoucherHeadId = voucher.Id; repo.setProc("Sp_VoucherDetail_Insert", model(l)); }
            repo.exec("USP_VoucherBalanceCheck", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", num));
            int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(voucher));
            for (ContraVoucherDto.Detail l : lines) {
                l.VoucherHeadId = voucher.Id;
                l.DocumentTypeIdRef = documentTypeIdRef;
                repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
            }
        }
        repo.inventoryTransactions(u, DOC, num);                                                   // :327
        for (Map<String, Object> d : details) {                                                    // :347
            repo.exec("USP_InventoryValidation", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC,
                    "DocDate", obj.get("DocDate"), "ItemId", toInt(d.get("ItemId")), "WarehouseId", toInt(d.get("FromWarehouseId")),
                    "JobLotId", toInt(d.get("JobLotId")), "CropYear", d.get("CropYear"), "InvPackingTypeId", toInt(d.get("InvPackingTypeId")),
                    "PackUomId", toInt(d.get("PackUomId")), "RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")),
                    "RefDocNoId", toInt(d.get("RefDocNoId")), "RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")),
                    "NetWeight", toDouble(d.get("NetWeight")), "ItemConditionId", toInt(d.get("ItemConditionId"))));
        }
        return num;
    }

    /** Model 1008 InvStockTransferDetail - non-virtual properties, declaration order, CLR defaults. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", 0); p.put("InvStockTransferHeaderId", 0); p.put("ItemId", 0); p.put("InvPackingTypeId", 0);
        p.put("CropYearId", 0); p.put("CropYear", null); p.put("JobLotId", 0); p.put("JobLotIdTo", 0); p.put("PackUomId", 0);
        p.put("Qty", 0d); p.put("GrossWeight", 0d); p.put("AdLsWeight", 0d); p.put("NetWeight", 0d); p.put("EbUnit", 0d);
        p.put("EbTotal", 0d); p.put("ItemRate", 0d); p.put("RateUomId", 0); p.put("ItemAmount", 0d); p.put("ExpenseAmount", 0d);
        p.put("FromWarehouseId", 0); p.put("ToWarehouseId", 0); p.put("RefDocumentTypeId", 0); p.put("RefDocNoId", 0);
        p.put("RefDocSubIdNo", 0); p.put("TransferDocumentTypeId", 0); p.put("TransferDetailId", 0); p.put("TransferId", 0);
        p.put("RefDocInvoiceId", 0); p.put("LineId", 0); p.put("ItemConditionId", 0); p.put("RackId", 0); p.put("ToRackId", 0);
        p.put("RemarksDetail", null);
        return p;
    }

    /** Public fields in declaration order (as StockTransferManualService.model). */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v != null && ("VoucherDate".equals(f.getName()) || "ChequeDate".equals(f.getName())
                        || "DueDate".equals(f.getName()) || "EntryDate".equals(f.getName())
                        || "ModifyDate".equals(f.getName()) || "PostDate".equals(f.getName())
                        || "DCheqDate".equals(f.getName()) || "GpDate".equals(f.getName()))) {
                    v = Timestamp.valueOf(String.valueOf(v).length() == 10 ? v + " 00:00:00" : String.valueOf(v));
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
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

    public boolean owns(int id) { return owned(ctx.requireAccountingUser(), id) != null; }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { if (v == null) return 0d; if (!Double.isFinite(v)) throw new IllegalArgumentException("Invalid number"); return v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static boolean toBool(String v) {
        if (v == null) return false;
        String s = v.trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
    }
    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }
}
