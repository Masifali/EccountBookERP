package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionInputRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

/**
 * 599 Production Release (Production Pcc) - Architecture.WinApp.pcc.Production/ProductionRelease.cs (base.Name "ProductionRelease"),
 * DocumentTypeId 1857 (the release), released documents are the Concrete Production 1850 (frmConcreteProduction).
 * Line refs ":NNN" are ProductionRelease.cs unless noted.
 *
 * Data (decompiled BLL 0349 BLL.pcc.ProductionRelease / 0350 BLL.pcc.ProductionHeader, DAL 0287 DAL.pcc.ProductionRelease):
 *   combos        pcc.USP_GetDataForDropDownFromProductionHeader          (AllCombobind :99)
 *   pending list  pcc.USP_PendingProductionsForApproval                   (HistoryGridFill :318)
 *   detail grids  pcc.USP_ProductionHeader_GetAllMethod ReadById + ReadByHeaderId_ProductionInputDetail / _ProductionOutPutDetail (grdDetailbind :460)
 *   release list  pcc.USP_ProductionRelease_GetAllMethod @Activity=FormHistory (HistoryBind :203)
 *   delete        pcc.USP_ProductionRelease_GetAllMethod @Activity=DeleteById (grdHistory_ColumnButtonClick)
 *   save          DAL ProductionRelease.SetData, ONE transaction: pcc.USP_ProductionRelease_Insert per ticked row, then
 *                 Sp_InventoryTransactions_GetALLMethod (RefDocumentTypeId 1857), Sp_InventoryStockEvalautionDetail_Update,
 *                 MakeVoucher -> Sp_VoucherHead_Insert / Sp_VoucherDetail_Insert / USP_VoucherBalanceCheck,
 *                 and finally pcc.Usp_ProductionReleaseValidation (@RefDocIds = ",id,id") which sets IsApproved on the productions.
 *
 * Deviations (web only):
 *  1. The release list uses the History tab's own From/To dates (the desktop's HistoryBind reads the filter tab's pickers,
 *     txtFromdateHistory/txtToDateHistory, although it sets FromDate/ToDate on the tab switch).
 *  2. The Production No filter sends the chosen production number text as @ProductionNo; the desktop sends the combo's Id
 *     (a ROW_NUMBER from pcc.USP_GetDataForDropDownFromProductionHeader), which can never equal a production number.
 *  3. Rights are enforced on the server: Save for release, "Delete" for the delete button (the desktop shows the X button
 *     to everybody), and ticked productions must belong to the user's organization / company, be a not yet released
 *     Concrete Production (1850) of the user's branch.
 */
@Service
public class W3AProductionReleaseService {

    public static final String SCREEN = "ProductionRelease";
    public static final int SCREEN_ID = 599;
    private static final int DOC_RELEASE = 1857, DOC_PRODUCTION = 1850;

    private final ProductionInputRepository repo;
    private final CurrentUserContext ctx;
    private final DesktopReportRights screenRights;
    private final SteelProductionSupport rights;

    public W3AProductionReleaseService(ProductionInputRepository repo, CurrentUserContext ctx, DesktopReportRights screenRights, SteelProductionSupport rights) {
        this.repo = repo; this.ctx = ctx; this.screenRights = screenRights; this.rights = rights;
    }

    // ----------------------------------------------------------------- helpers
    private static int toInt(Object o) { return ProductionInputRepository.toInt(o); }
    private static Object ci(Map<String, Object> r, String k) { return ProductionInputRepository.ci(r, k); }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static java.sql.Date date(Object iso) {
        String s = str(iso);
        if (s.length() < 10) throw new IllegalArgumentException("Invalid date.");
        return java.sql.Date.valueOf(LocalDate.parse(s.substring(0, 10)));
    }
    private UserAccount user() {
        UserAccount u = ctx.requireAccountingUser();
        screenRights.require(u, SCREEN_ID, "View");
        return u;
    }
    private static String dateOnly(Object o) { String s = str(o); return s.length() >= 10 ? s.substring(0, 10) : s; }

    /** Rightsobjects (CommonServices.SetRightsValueInRightsObject for "ProductionRelease"). */
    public Map<String, Object> rights() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("canSave", rights.hasRight(SCREEN, "Save"));
        m.put("canViewAllRecord", rights.hasRight(SCREEN, "CanView AllRecord"));
        m.put("canDelete", rights.hasRight(SCREEN, "Delete"));
        return m;
    }

    // ----------------------------------------------------------------- load
    /** Load :105 - rights, Datetypefill, AllCombobind (:130). */
    public Map<String, Object> init() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.putAll(rights());
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        List<Map<String, Object>> rows = repo.query("[pcc].[USP_GetDataForDropDownFromProductionHeader]", p);
        out.put("plants", bucket(rows, "Plant"));
        out.put("productionTypes", bucket(rows, "ProductionType"));
        out.put("productionNos", bucket(rows, "ProductionNo"));
        out.put("boms", bucket(rows, "BOMName"));
        out.put("items", bucket(rows, "ItemHeader"));
        Object fy = repo.financialYearStart(u, ctx.currentFinancialYearId());
        out.put("fyStart", fy == null ? LocalDate.now().toString() : dateOnly(fy));
        out.put("userBranchId", ctx.currentBranchId());
        return out;
    }

    private static List<Map<String, Object>> bucket(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!str(ci(r, "Activity")).equals(activity)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id")); m.put("Name", ci(r, "ReferenceName"));
            out.add(m);
        }
        return out;
    }

    // ----------------------------------------------------------------- pending productions (tab 1 of the form)
    /** HistoryGridFill :318 + the reshaping into dtcol :346-367. */
    public List<Map<String, Object>> pending(Map<String, Object> b) {
        UserAccount u = user();
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", ctx.currentBranchId());
        p.put("@DocumentTypeId", DOC_PRODUCTION);
        p.put("@FinancialYearId", ctx.currentFinancialYearId());
        p.put("@FromDate", date(b.get("fromDate")));
        p.put("@ToDate", date(b.get("toDate")));
        if (toInt(b.get("fromDocNo")) != 0) p.put("@FromDocNo", toInt(b.get("fromDocNo")));
        if (toInt(b.get("toDocNo")) != 0) p.put("@ToDocNo", toInt(b.get("toDocNo")));
        if (toInt(b.get("plantId")) != 0) p.put("@PlantId", toInt(b.get("plantId")));
        if (toInt(b.get("itemId")) != 0) p.put("@ItemId", toInt(b.get("itemId")));
        if (toInt(b.get("productionTypeId")) != 0) p.put("@ProductionTypeId", toInt(b.get("productionTypeId")));
        if (toInt(b.get("bomId")) != 0) p.put("@BomHeaderId", toInt(b.get("bomId")));
        if (!str(b.get("productionNo")).isEmpty()) p.put("@ProductionNo", str(b.get("productionNo")));   /* deviation 2 */
        List<Map<String, Object>> src = repo.query("[pcc].[USP_PendingProductionsForApproval]", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("DocDate", LocalDate.now().toString());                 /* release date: DateTime.Now.ToShortDateString() */
            m.put("Id", ci(r, "Id")); m.put("DocumentTypeId", ci(r, "DocumentTypeId"));
            m.put("RefDoNo", ci(r, "DocNo")); m.put("ProductionDate", dateOnly(ci(r, "DocDate")));
            for (String k : new String[]{"ProductionNo", "ProductionType", "PlantName", "BomHeaderNo", "ItemName", "ItemCode", "UOM", "ItemQty"}) m.put(k, ci(r, k));
            m.put("WipWareHouseName", ci(r, "WareHouseName"));
            for (String k : new String[]{"WIPAccountTitle", "RemarksHeader", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName", "NoOfAttachments"}) m.put(k, ci(r, k));
            out.add(m);
        }
        return out;
    }

    /** grdDetailbind :460 - ProductionHeader.ReadById then the input / output detail lists. */
    public Map<String, Object> detail(int id) {
        UserAccount u = user();
        header(u, id);   /* ownership check + ActionId <> 3 */
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> in = new ArrayList<>(), op = new ArrayList<>();
        for (Map<String, Object> r : details(id, "ReadByHeaderId_ProductionInputDetail")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id")); m.put("WarehouseName", ci(r, "WareHouseName")); m.put("JobLot", ci(r, "JobLotDescription")); m.put("ItemId", ci(r, "ItemId"));
            m.put("ItemName", ci(r, "ItemName")); m.put("Uom", ci(r, "ItemUomCode")); m.put("ItemQty", ci(r, "ItemQty")); m.put("NetWeight", ci(r, "NetWeight"));
            m.put("Rate", ci(r, "Rate")); m.put("RateUom", ci(r, "RateUomCode")); m.put("Amount", ci(r, "Amount"));
            in.add(m);
        }
        for (Map<String, Object> r : details(id, "ReadByHeaderId_ProductionOutPutDetail")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id")); m.put("EntryType", ci(r, "EntryType")); m.put("SaleOrderId", ci(r, "SaleOrderId")); m.put("SaleOrder", ci(r, "SaleOrderNo"));
            m.put("WarehouseName", ci(r, "WareHouseName")); m.put("JobLot", ci(r, "JobLotDescription")); m.put("ItemId", ci(r, "ItemId")); m.put("ItemName", ci(r, "ItemName"));
            m.put("AttributeVarient", ci(r, "ItemAttribute")); m.put("VarientUnit", ci(r, "VarientEquivalent")); m.put("Uom", ci(r, "ItemUomCode"));
            m.put("ItemQty", ci(r, "ItemQty")); m.put("VarientQty", ci(r, "VarientQty")); m.put("NetWeight", ci(r, "NetWeight")); m.put("Rate", ci(r, "Rate"));
            m.put("RateUom", ci(r, "RateUomCode")); m.put("Amount", ci(r, "Amount"));
            op.add(m);
        }
        out.put("input", in); out.put("output", op);
        return out;
    }

    private Map<String, Object> header(UserAccount u, int id) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> rows = repo.query("[pcc].[USP_ProductionHeader_GetAllMethod]", p);
        if (rows.isEmpty()) throw new IllegalArgumentException("Production output object Not found");
        Map<String, Object> h = rows.get(0);
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId())
            throw new IllegalArgumentException("Production output object Not found");
        return h;
    }

    private List<Map<String, Object>> details(int id, String activity) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Id", id);
        p.put("@Activity", activity);
        return repo.query("[pcc].[USP_ProductionHeader_GetAllMethod]", p);
    }

    // ----------------------------------------------------------------- release history (tab 2)
    /** HistoryBind :203 - ProductionRelease.FormHistory. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user();
        boolean all = rights.hasRight(SCREEN, "CanView AllRecord");
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", ctx.currentBranchId());
        p.put("@DocumentTypeId", DOC_RELEASE);
        p.put("@FinancialYearId", ctx.currentFinancialYearId());
        p.put("@FromDate", date(b.get("fromDate")));
        p.put("@ToDate", date(b.get("toDate")));
        p.put("@CanViewAllRecord", all ? 1 : 0);
        if (!all) p.put("@EntryUserId", ctx.currentUserId());
        p.put("@Activity", "FormHistory");
        List<Map<String, Object>> src = repo.query("[pcc].[USP_ProductionRelease_GetAllMethod]", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id")); m.put("DocNo", ci(r, "DocNo")); m.put("DocDate", dateOnly(ci(r, "DocDate")));
            m.put("RefDocId", ci(r, "RefDocId")); m.put("RefDocNo", ci(r, "RefDocNo")); m.put("RefDocDate", dateOnly(ci(r, "RefDocDate")));
            for (String k : new String[]{"ProductionNo", "ProductionType", "PlantName", "BomHeaderNo", "ItemName", "ItemCode", "UOM", "ItemQty"}) m.put(k, ci(r, k));
            m.put("WipWareHouseName", ci(r, "WareHouseName"));
            for (String k : new String[]{"WIPAccountTitle", "RemarksHeader", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName"}) m.put(k, ci(r, k));
            out.add(m);
        }
        return out;
    }

    /** grdHistory "Delete" button - ProductionRelease.DeleteByID(UserAccount.ID, Id). */
    @Transactional(rollbackFor = Exception.class)
    public void delete(int id) {
        UserAccount u = user();
        if (!rights.hasRight(SCREEN, "Delete")) throw new IllegalStateException("Delete is not allowed for this user.");   /* deviation 3 */
        LinkedHashMap<String, Object> r = ProductionInputRepository.params();
        r.put("@Id", id);
        r.put("@OrganizationId", u.getOrganizationId());
        r.put("@CompanyId", u.getCompanyId());
        r.put("@DocumentTypeId", DOC_RELEASE);
        r.put("@BranchesId", ctx.currentBranchId());
        r.put("@FinancialYearId", ctx.currentFinancialYearId());
        r.put("@CanViewAllRecord", 1);
        r.put("@Activity", "ReadById");
        List<Map<String, Object>> own = repo.query("[pcc].[USP_ProductionRelease_GetAllMethod]", r);
        if (own.isEmpty() || toInt(ci(own.get(0), "OrganizationId")) != u.getOrganizationId() || toInt(ci(own.get(0), "CompanyId")) != u.getCompanyId()
                || toInt(ci(own.get(0), "ActionId")) == 3)
            throw new IllegalArgumentException("Record not found.");
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@EntryUserId", ctx.currentUserId());
        p.put("@Id", id);
        p.put("@Activity", "DeleteById");
        repo.query("[pcc].[USP_ProductionRelease_GetAllMethod]", p);
    }

    /** VoucherReprot :930 - CommonServices.VoucherHeadIdGet(Id, 1857). */
    public int voucherHeadId(int releaseId) {
        UserAccount u = user();
        return repo.voucherHeadId(u, DOC_RELEASE, releaseId);
    }

    // ----------------------------------------------------------------- save
    /** btnSave_Click :960 + DAL ProductionRelease.SetData. body: { rows: [ { id, docDate } ] } (the ticked rows). */
    @Transactional(rollbackFor = Exception.class)
    @SuppressWarnings("unchecked")
    public int save(Map<String, Object> body) {
        UserAccount u = user();
        if (!rights.hasRight(SCREEN, "Save")) throw new IllegalStateException("Save is not allowed for this user.");
        Object ro = body.get("rows");
        List<Map<String, Object>> rows = ro instanceof List ? (List<Map<String, Object>>) ro : new ArrayList<>();
        if (rows.isEmpty()) throw new IllegalArgumentException("Please select check box first");
        final int org = u.getOrganizationId(), comp = u.getCompanyId(), branch = ctx.currentBranchId(), fy = ctx.currentFinancialYearId(), user = ctx.currentUserId();
        List<Map<String, Object>> itemGl = null;
        StringBuilder refIds = new StringBuilder();
        int last = 0;
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> row : rows) {
            int refId = toInt(row.get("id"));
            if (!seen.add(refId)) continue;
            Map<String, Object> h = header(u, refId);
            if (toInt(ci(h, "DocumentTypeId")) != DOC_PRODUCTION || toInt(ci(h, "BranchesId")) != branch)
                throw new IllegalArgumentException("Production output object Not found");
            if (alreadyReleased(u, refId)) throw new IllegalArgumentException("Production is already released.");   /* deviation 3 */
            refIds.append(',').append(refId);
            if (itemGl == null) itemGl = repo.itemGlAccounts(u);
            java.sql.Date docDate = date(row.get("docDate"));

            // 1. USP_ProductionRelease_Insert (item.Id = 0, DocNo = 0, ActionId = 1; DocumentTypeId forced to 1857 by the DAL)
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@Id", 0);
            p.put("@DocumentTypeId", DOC_RELEASE);
            p.put("@DocDate", docDate);
            p.put("@DocNo", 0);
            p.put("@RefDocumentTypeId", toInt(ci(h, "DocumentTypeId")));
            p.put("@RefDocId", refId);
            p.put("@EntryDate", LocalDateTime.now());
            p.put("@EntryUserId", user);
            p.put("@ModifyDate", LocalDateTime.now());
            p.put("@ModifyUserId", user);
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@FinancialYearId", fy);
            p.put("@BranchesId", branch);
            p.put("@ActionId", 1);
            int id = toInt(repo.scalar("[pcc].[USP_ProductionRelease_Insert]", p));
            last = id;

            // 2. inventory movements, 3. stock evaluation (DAL :63-70)
            Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
            it.put("OrganizationId", org); it.put("CompanyId", comp); it.put("RefDocumentTypeId", DOC_RELEASE); it.put("RefDocIdNo", id);
            repo.setProc("Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);
            Map<String, Object> ev = ProductionInputRepository.stockEvaluationDefaults();
            ev.put("OrganizationId", org); ev.put("CompanyId", comp); ev.put("RefDocumentTypeId", DOC_RELEASE); ev.put("RefDocIdNo", id);
            repo.setProc("Sp_InventoryStockEvalautionDetail_Update", ev, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);

            // 4. MakeVoucher (DAL :133) - the release's output rows against the production's WIP account
            if (toInt(ci(h, "WIPGLAccountID")) == 0) throw new IllegalArgumentException("WipAccountId not found");
            int wip = toInt(ci(h, "WIPGLAccountID"));
            Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
            vh.put("DocumentTypeSrNo", id);
            vh.put("VoucherDate", docDate);
            vh.put("VoucherCode", 0);          /* obj.DocNo is never refreshed after the insert (GenericProvider.SetProc returns only the id) */
            vh.put("DocumentTypeId", DOC_RELEASE);
            vh.put("EntryDate", LocalDateTime.now());
            vh.put("ModifyDate", LocalDateTime.now());
            vh.put("EntryUser", user);
            vh.put("ModifyUser", user);
            vh.put("OrganizationId", org);
            vh.put("CompanyId", comp);
            vh.put("FinancialYearId", fy);
            vh.put("BranchId", branch);
            vh.put("IsApproved", true);
            List<Map<String, Object>> lines = new ArrayList<>();
            double total = 0.0;
            int line = 1;
            for (Map<String, Object> d : details(refId, "ReadByHeaderId_ProductionOutPutDetail")) {
                if (toInt(ci(d, "ActionTypeId")) == 3) continue;
                if (itemGl.isEmpty()) throw new IllegalArgumentException("Item GL Account Not Found");
                Map<String, Object> gl = null;
                for (Map<String, Object> g : itemGl) if (toInt(ci(g, "Id")) == toInt(ci(d, "ItemId"))) { gl = g; break; }
                if (gl == null) continue;
                String remarks = str(ci(d, "Remarks"));
                String base = "ItemName: " + str(ci(gl, "ItemName")) + " Qty  " + str(ci(d, "ItemQty")) + " Rate  " + str(ci(d, "Rate"));
                String text = remarks.isEmpty() ? base : remarks + " " + base;
                double amount = ProductionInputRepository.toDouble(ci(d, "Amount"));
                Map<String, Object> dr = ProductionInputRepository.voucherDetailDefaults();
                dr.put("LineId", line);
                dr.put("AccountId", toInt(ci(gl, "PurchaseGLAC")));
                dr.put("AgainstAccountId", wip);
                dr.put("Comments", text);
                dr.put("DebitAmount", amount);
                dr.put("QtyIn", ProductionInputRepository.toDouble(ci(d, "ItemQty")));
                dr.put("WeightIn", ProductionInputRepository.toDouble(ci(d, "NetWeight")));
                dr.put("ItemRate", ProductionInputRepository.toDouble(ci(d, "Rate")));
                dr.put("ItemAmount", amount);
                dr.put("OrderNo", toInt(ci(d, "SaleOrderId")));
                dr.put("JobLotId", toInt(ci(d, "JobLotId")));
                total += amount;
                lines.add(dr);
                Map<String, Object> cr = ProductionInputRepository.voucherDetailDefaults();
                cr.put("LineId", line);
                cr.put("AccountId", wip);
                cr.put("AgainstAccountId", toInt(ci(gl, "PurchaseGLAC")));
                cr.put("Comments", text);
                cr.put("CreditAmount", amount);
                cr.put("QtyIn", ProductionInputRepository.toDouble(ci(d, "ItemQty")));
                cr.put("WeightIn", ProductionInputRepository.toDouble(ci(d, "NetWeight")));
                cr.put("ItemRate", ProductionInputRepository.toDouble(ci(d, "Rate")));
                cr.put("ItemAmount", amount);
                cr.put("OrderNo", toInt(ci(d, "SaleOrderId")));
                cr.put("JobLotId", toInt(ci(d, "JobLotId")));
                lines.add(cr);
                line++;
            }
            vh.put("VoucherAmount", total);

            // 5. voucher head (always an insert: the DAL only looks the head up when ActionId == 2), details, balance check
            int vid = repo.setProc("Sp_VoucherHead_Insert", vh, ProductionInputRepository.P_VOUCHER_HEAD);
            if (vid <= 0) vid = toInt(vh.get("Id"));
            if (lines.isEmpty()) continue;   /* DAL :152 - no detail rows, no balance check */
            for (Map<String, Object> l : lines) {
                l.put("VoucherHeadId", vid);
                repo.setProc("Sp_VoucherDetail_Insert", l, ProductionInputRepository.P_VOUCHER_DETAIL);
            }
            if (vid > 0) {
                LinkedHashMap<String, Object> bp = ProductionInputRepository.params();
                bp.put("@OrganizationId", org);
                bp.put("@CompanyId", comp);
                bp.put("@Id", vid);
                repo.command("dbo.USP_VoucherBalanceCheck", bp);
            }
        }
        // 6. Usp_ProductionReleaseValidation - marks the productions approved
        LinkedHashMap<String, Object> v = ProductionInputRepository.params();
        v.put("@OrganizationId", org);
        v.put("@CompanyId", comp);
        v.put("@RefDocIds", refIds.toString());
        repo.command("[pcc].[Usp_ProductionReleaseValidation]", v);
        return last;
    }

    private boolean alreadyReleased(UserAccount u, int refDocId) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", ctx.currentBranchId());
        p.put("@DocumentTypeId", DOC_RELEASE);
        p.put("@FinancialYearId", ctx.currentFinancialYearId());
        p.put("@CanViewAllRecord", 1);
        p.put("@Activity", "FormHistory");
        for (Map<String, Object> r : repo.query("[pcc].[USP_ProductionRelease_GetAllMethod]", p)) {
            if (toInt(ci(r, "RefDocId")) == refDocId) return true;
        }
        return false;
    }
}
