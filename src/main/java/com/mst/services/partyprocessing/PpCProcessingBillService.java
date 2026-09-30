package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.ProcessingBill;
import com.mst.models.partyprocessing.PpCModels.ProcessingBillOh;
import com.mst.models.partyprocessing.PpCModels.ProcessingBillOutput;
import com.mst.models.partyprocessing.PpCModels.ProcessingBillPm;
import com.mst.models.partyprocessing.PpCModels.VoucherDetail;
import com.mst.models.partyprocessing.PpCModels.VoucherHead;
import com.mst.repositories.partyprocessing.PpCRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpCSupport.*;

/**
 * 675 Processing Bill (Party Processing) - Architecture.WinApp.PartyProcessing.ProductionPartyProcessingBill.cs,
 * BLL 0302 / DAL 0305 InvProductionProcessingBill (DocumentTypeId 123) with its voucher (MakeVoucher).
 *
 * Load: rights (btnsave Save, btnUpdate Update, BtnPrint + "Print" check Print, btnDelete CanDelete), AccountTitleFill
 * (account types 2,15,11,12,13,14,20,21,22), GenerateCode, RateUomBind (UOMStaticAll: Id / Equivalent), JobOrderNoFill
 * (GetJobOrderNoForInvFoodProduction, DocumentTypeId 120), DefaultDaysToLessFromHistoryFromDate, HistoryCombosFill.
 * Job order leave: GetGlAccountsByJobOrderId (stock party), GetInPutForProcessingBill (117) and the by-product grid
 * (GetOutPutForProcessingBillByJobOrder, 118, EntryType "ByProduct"). Grid / bill amount calculations run in the page.
 * Attachments (DMS) are not ported: the save sends no attachment rows, as the desktop does when none are added.
 */
@Service
public class PpCProcessingBillService {

    public static final int SCREEN = 675;
    public static final int DOC_TYPE = 123;

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lookups(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("billNo", repo.billCode(u, pp.year()));
        out.put("rateUoms", project(repo.uomStatic(), "Id", "RateUom=Equivalent"));
        out.put("debitAccounts", project(repo.accountsByTypeIds(u, "4,6"), "Id", "AccountTitle"));
        out.put("pmItems", project(repo.itemsForPartyProcessingPm(u), "Id", "ItemName"));
        out.put("ohAccounts", project(repo.accountsByTypeIds(u, "2,15,22"), "Id", "AccountTitle"));
        out.put("defaultDays", pp.defaultDays(u));
        out.put("historyJobOrders", historyJobOrders(u));
        return out;
    }

    /** AccountTitleFill + JobOrderNoFill (btnRefresh only re-runs JobOrderNoFill). */
    private Map<String, Object> lookups(UserAccount u) {
        return m("creditAccounts", project(repo.accountsByTypeIds(u, "2,15,11,12,13,14,20,21,22"), "Id", "AccountTitle"),
                "jobOrders", project(repo.jobOrdersForProduction(u, pp.year()), "Id", "PlanCode"));
    }

    public Map<String, Object> refresh() {
        UserAccount u = pp.user(SCREEN);
        return m("jobOrders", project(repo.jobOrdersForProduction(u, pp.year()), "Id", "PlanCode"));
    }

    public Map<String, Object> code() { UserAccount u = pp.user(SCREEN); return m("billNo", repo.billCode(u, pp.year())); }

    /** HistoryCombosFill: rows with Activity "InvJobOrder". */
    private List<Map<String, Object>> historyJobOrders(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.billDropDown(u)) if ("InvJobOrder".equals(str(r.get("Activity")))) out.add(m("Id", r.get("Id"), "name", r.get("ReferenceName")));
        return out;
    }

    public Map<String, Object> historyCombos() { return m("historyJobOrders", historyJobOrders(pp.user(SCREEN))); }

    /** CmbJobOrderNo_Leave: stock party of the job order, the input grid and the by-product grid. */
    public Map<String, Object> jobOrder(int jobOrderId) {
        UserAccount u = pp.user(SCREEN);
        return m("stockParties", project(repo.jobOrderGl(u, pp.year(), jobOrderId), "StockPartyId", "StockParty"),
                "inputs", inputs(u, jobOrderId), "outputs", outputs(u, jobOrderId));
    }

    /** GetInPutForProcessingBill -> dtTarget (ItemId, ItemName, ItemCode, Uom, Qty, StockWeight). */
    private List<Map<String, Object>> inputs(UserAccount u, int jobOrderId) {
        return project(repo.billInputs(u, pp.year(), jobOrderId), "ItemId", "ItemName", "ItemCode", "Uom=UomEquivalent", "Qty", "StockWeight");
    }

    /** ByProductGrdFill: dtByProduct rows (Id 0, PackSize = UomCode, purchase columns 0, RateUom 40). */
    private List<Map<String, Object>> outputs(UserAccount u, int jobOrderId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.billOutputs(u, pp.year(), jobOrderId, "ByProduct")) {
            out.add(m("Id", 0, "ItemId", r.get("ItemId"), "ItemCode", r.get("ItemCode"), "ItemName", r.get("ItemName"), "PackSize", dbl(r.get("UomCode")),
                    "Qty", dbl(r.get("Qty")), "StockWeight", dbl(r.get("StockWeight")), "PurchaseQty", 0, "PurchaseWeight", 0, "PurchaseRate", 0,
                    "RateUom", 40, "Amount", 0, "DebitAccountId", 0, "Remarks", ""));
        }
        return out;
    }

    private Map<String, Object> header(UserAccount u, int id) {
        if (id == 0) throw invalid("Record Id Not Found");
        List<Map<String, Object>> r = repo.bill(id);
        if (r.isEmpty() || !PpCScreens.owns(u, r.get(0))) throw invalid("Record Not Found");
        return r.get(0);
    }

    /**
     * ReadById: header, CmbJobOrderNo_Leave for its job order, then the saved by-product values laid over the rows of
     * the same item, and the PM / OH grids. The saved output row Id is carried into the row (see save()).
     */
    public Map<String, Object> read(int id) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> h = header(u, id);
        int jobOrderId = toInt(h.get("InvProductionJobOrderId"));
        List<Map<String, Object>> saved = repo.billChild(id, "ReadBillOutPutByHeaderId");
        List<Map<String, Object>> outs = outputs(u, jobOrderId);
        for (Map<String, Object> row : outs) {
            for (Map<String, Object> s : saved) {
                if (toInt(s.get("ItemId")) == toInt(row.get("ItemId"))) {
                    row.put("Id", toInt(s.get("Id")));
                    row.put("PurchaseQty", dbl(s.get("Qty")));
                    row.put("PurchaseWeight", dbl(s.get("Weight")));
                    row.put("PurchaseRate", dbl(s.get("Rate")));
                    row.put("Amount", dbl(s.get("Amount")));
                    row.put("DebitAccountId", toInt(s.get("DebitAccountId")));
                    row.put("Remarks", str(s.get("Remarks")));
                }
            }
        }
        return m("Id", id, "BillNo", toInt(h.get("BillNo")), "BillDate", h.get("BillDate"), "InvProductionJobOrderId", jobOrderId,
                "StockPartyId", toInt(h.get("StockPartyId")), "CreditAccountId", toInt(h.get("CreditAccountId")), "Weight", dbl(h.get("Weight")),
                "Rate", dbl(h.get("Rate")), "RateUomId", toInt(h.get("RateUomId")), "TotalBPPurchaseAmt", dbl(h.get("TotalBPPurchaseAmt")),
                "Amount", dbl(h.get("Amount")), "RemarksHeader", str(h.get("RemarksHeader")),
                "stockParties", project(repo.jobOrderGl(u, pp.year(), jobOrderId), "StockPartyId", "StockParty"),
                "inputs", inputs(u, jobOrderId), "outputs", outs,
                "packing", project(repo.billChild(id, "ReadBillPMById"), "ItemId", "Qty", "Rate", "Amount", "Remarks"),
                "overheads", project(repo.billChild(id, "ReadBillOhById"), "AccountId", "Qty", "Rate", "Amount", "Remarks=OHRemarks"));
    }

    /** BindHistoryDetail: input grid of the row's job order, and the saved by-product / PM / OH rows. */
    public Map<String, Object> historyDetail(int id, int jobOrderId) {
        UserAccount u = pp.user(SCREEN);
        header(u, id);
        return m("inputs", inputs(u, jobOrderId),
                "outputs", project(repo.billChild(id, "ReadBillOutPutByHeaderId"), "ItemCode", "ItemName", "Qty", "UOM=ItemUomId", "Weight", "Rate", "Amount",
                        "Remarks", "DebitAccount"),
                "packing", project(repo.billChild(id, "ReadBillPMById"), "ItemCode", "ItemName", "Qty", "Rate", "Amount", "Remarks"),
                "overheads", project(repo.billChild(id, "ReadBillOhById"), "Account=AccountTitle", "Qty", "Rate", "Amount", "Remarks=OHRemarks"));
    }

    private static BigDecimal dec(Object v) { return BigDecimal.valueOf(toDouble(v)); }

    /** Insert(): FormValidation, the model as the form fills it, the grid checks, BLL Save (MakeVoucher + DAL SetData). */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        String billNo = trim(b.get("billNo"));
        if (billNo.isEmpty() || billNo.equals("0")) throw invalid("BillNo Field Required");
        String rate = str(b.get("rate"));
        if (rate.isEmpty() || rate.equals("0")) throw invalid("Rate Field Required");
        if (toInt(b.get("stockPartyId")) == 0) throw invalid("StockParty Field Required");
        if (toInt(b.get("creditAccountId")) == 0) throw invalid("CreditAccount Field Required");
        if (toInt(b.get("rateUomId")) == 0) throw invalid("RateUom Field Required");
        if (toInt(b.get("jobOrderId")) == 0) throw invalid("JobOrder Field Required");
        if (recId > 0) header(u, recId);

        LocalDateTime now = LocalDateTime.now();
        ProcessingBill o = new ProcessingBill();
        o.Id = recId;
        o.OrganizationId = u.getOrganizationId();
        o.CompanyId = u.getCompanyId();
        o.FinancialYearId = pp.year();
        o.BranchesId = branch(u);
        o.ProjectsId = branch(u);
        o.DocumentTypeId = DOC_TYPE;
        o.ScreenName = "ProductionPartyProcessingBill";
        o.EntryUserId = uid(u);
        o.ModifyUserId = uid(u);
        o.ApprovedUserId = uid(u);
        o.EntryDate = now;
        o.ModifyDate = now;
        o.ApprovedDate = now;
        o.IsApproved = false;
        o.BillNo = toInt(billNo);
        o.BillDate = orNow(picker(b.get("billDate")));
        o.InvProductionJobOrderId = toInt(b.get("jobOrderId"));
        o.StockPartyId = toInt(b.get("stockPartyId"));
        o.CreditAccountId = toInt(b.get("creditAccountId"));
        /* Desktop: RateUomId = Conversion.ToInt(CmbRateUom.Text) - the combo shows the Equivalent, so the equivalent is stored. */
        o.RateUomId = toInt(b.get("rateUomText"));
        o.Rate = dec(b.get("rate"));
        o.Qty = dec(b.get("noOfBags"));
        o.Weight = dec(b.get("noOfBags"));
        o.Amount = dec(b.get("processingAmount"));
        o.TotalBPPurchaseAmt = dec(b.get("bpTotal"));
        o.RemarksHeader = str(b.get("remarks"));

        int idx = 0;
        for (Map<String, Object> r : list(b.get("outputs"))) {
            idx++;
            if (!(toDouble(r.get("Amount")) > 0 && toInt(r.get("DebitAccountId")) > 0)) continue;
            String at = " In OutPut By Product Grid on row no :" + idx;
            if (toInt(r.get("ItemId")) == 0) throw invalid("Please Select an Item" + at);
            if (toDouble(r.get("PackSize")) == 0) throw invalid("Please Enter PackSize" + at);
            if (toDouble(r.get("PurchaseQty")) == 0) throw invalid("Please Enter Purchase Qty" + at);
            if (toDouble(r.get("PurchaseWeight")) == 0) throw invalid("Please Enter Purchase Weight" + at);
            if (toDouble(r.get("PurchaseRate")) == 0) throw invalid("Rate Is Required" + at);
            if (toDouble(r.get("RateUom")) == 0) throw invalid("RateUom Is Required" + at);
            if (toDouble(r.get("Amount")) == 0) throw invalid("Amount Is Required" + at);
            ProcessingBillOutput d = output(r);
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            d.SortNo = idx;
            o.outputs.add(d);
        }
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removedOutputs"))) {
                if (toInt(r.get("Id")) <= 0) continue;
                ProcessingBillOutput d = output(r);
                d.ActionTypeId = 3;
                o.outputs.add(d);
            }
        }
        idx = 0;
        for (Map<String, Object> r : list(b.get("overheads"))) {
            idx++;
            if (toDouble(r.get("Amount")) > 0 && toInt(r.get("AccountId")) == 0) throw invalid("Please Select an Account Against OverHead First");
            if (toInt(r.get("AccountId")) > 0) {
                ProcessingBillOh d = new ProcessingBillOh();
                d.AccountId = toInt(r.get("AccountId"));
                d.Qty = dec(r.get("Qty"));
                d.Rate = dec(r.get("Rate"));
                d.Amount = dec(r.get("Amount"));
                d.OHRemarks = str(r.get("Remarks"));
                d.SortNo = idx;
                o.overheads.add(d);
            }
        }
        idx = 0;
        for (Map<String, Object> r : list(b.get("packing"))) {
            idx++;
            if (!(toDouble(r.get("Amount")) > 0)) continue;
            if (toInt(r.get("ItemId")) == 0) throw invalid("Please Select an Item Against PM Grid on row no :" + idx);
            /* Conversion.ToInt(cell) == 0: a fractional Qty / Rate below 1 fails the check, as on the desktop. */
            if ((int) toDouble(r.get("Qty")) == 0) throw invalid("Please Select an Qty Against PM Grid on row no :" + idx);
            if ((int) toDouble(r.get("Rate")) == 0) throw invalid("Please Select an Rate Against PM Grid on row no :" + idx);
            ProcessingBillPm d = new ProcessingBillPm();
            d.ItemId = toInt(r.get("ItemId"));
            d.Qty = dec(r.get("Qty"));
            d.Rate = dec(r.get("Rate"));
            d.Amount = dec(r.get("Amount"));
            d.Remarks = str(r.get("Remarks"));
            d.SortNo = idx;
            o.packing.add(d);
        }

        VoucherHead v = makeVoucher(u, o);
        String proc = o.Id == 0 ? "USP_InvProductionProcessingBill_Insert" : "USP_InvProductionProcessingBill_Update";
        int id = repo.tx(() -> {
            int n = repo.set(proc, o);
            if (n > 0) o.Id = n; else n = o.Id;
            for (ProcessingBillOh d : o.overheads) { d.InvProductionProcessingBillId = o.Id; repo.set("USP_InvProductionProcessingBillOH_Insert", d); }
            for (ProcessingBillPm d : o.packing) { d.InvProductionProcessingBillId = o.Id; repo.set("USP_InvProductionProcessingBillPM_Insert", d); }
            for (ProcessingBillOutput d : o.outputs) { d.InvProductionProcessingBillId = o.Id; repo.exec("USP_InvProductionProcessingBillOutPut_Insert", d.toParams()); }
            pp.writeVoucher(u, v, DOC_TYPE, o.Id, true, false, true);
            return n;
        });
        return m("ok", true, "id", id, "jobOrderId", o.InvProductionJobOrderId,
                "message", (recId == 0 ? "Record Save Successfully" : "Record Update Successfully") + o.BillNo);
    }

    private static ProcessingBillOutput output(Map<String, Object> r) {
        ProcessingBillOutput d = new ProcessingBillOutput();
        d.Id = toInt(r.get("Id"));
        d.ItemId = toInt(r.get("ItemId"));
        /* Desktop: ItemUomId = Conversion.ToInt(PackSize) - the pack size (100 from the procedure), not a UOM id. */
        d.ItemUomId = (int) toDouble(r.get("PackSize"));
        d.Qty = dec(r.get("PurchaseQty"));
        d.Weight = dec(r.get("PurchaseWeight"));
        d.Rate = dec(r.get("PurchaseRate"));
        d.Amount = dec(r.get("Amount"));
        d.DebitAccountId = toInt(r.get("DebitAccountId"));
        d.Remarks = str(r.get("Remarks"));
        return d;
    }

    /** .NET decimal.ToString() of a value that came from a double cell (no trailing zeros). */
    private static String net(BigDecimal v) { return clr(v.doubleValue()); }

    /** BLL InvProductionProcessingBill.MakeVoucher (DocumentTypeId 123). */
    private VoucherHead makeVoucher(UserAccount u, ProcessingBill o) {
        VoucherHead v = new VoucherHead();
        LocalDateTime now = LocalDateTime.now();
        v.DocumentTypeId = o.DocumentTypeId;
        v.DocumentTypeSrNo = o.Id;
        v.RefDocNoId = o.Id;
        v.VoucherCode = o.BillNo;
        v.VoucherDate = o.BillDate;
        v.Remarks = str(o.RemarksHeader);
        v.RemarksOtherLingo = "";
        v.ChequeDate = LocalDate.now().atStartOfDay();
        v.IncludeWHT = false;
        v.BranchId = o.BranchesId;
        v.ProjectId = o.ProjectsId;
        v.DueDate = now;
        v.OrganizationId = o.OrganizationId;
        v.CompanyId = o.CompanyId;
        v.FinancialYearId = o.FinancialYearId;
        v.EntryUser = o.EntryUserId;
        v.EntryDate = now;
        v.ModifyDate = now;
        v.ModifyUser = o.ModifyUserId;
        List<Map<String, Object>> parties = repo.supplierCustomerGl(u);
        if (!parties.isEmpty()) {
            Map<String, Object> p = find(parties, "Id", o.StockPartyId);
            if (p == null) throw invalid("StockParty GLAccountId not Found");
            v.RefAccountId = toInt(p.get("GlAccountId"));
        }
        String hr = str(o.RemarksHeader);
        v.voucherDetailList.add(line(v.RefAccountId, o.CreditAccountId, hr, dbl(o.Amount), 0, dbl(o.Qty), dbl(o.Rate), dbl(o.Weight), o.StockPartyId));
        v.voucherDetailList.add(line(o.CreditAccountId, v.RefAccountId, hr, 0, dbl(o.Amount), dbl(o.Qty), dbl(o.Rate), 0, o.StockPartyId));
        for (ProcessingBillOh d : o.overheads) {
            v.voucherDetailList.add(line(v.RefAccountId, d.AccountId, str(d.OHRemarks), dbl(d.Amount), 0, dbl(d.Qty), dbl(d.Rate), 0, 0));
            v.voucherDetailList.add(line(d.AccountId, v.RefAccountId, str(d.OHRemarks), 0, dbl(d.Amount), dbl(d.Qty), dbl(d.Rate), 0, 0));
        }
        if (!o.packing.isEmpty()) {
            List<Map<String, Object>> items = repo.itemGl(u);
            for (ProcessingBillPm d : o.packing) {
                Map<String, Object> it = items.isEmpty() ? null : find(items, "Id", d.ItemId);
                if (it == null) throw invalid("Item PurchaseGLAccountId not Found");
                int gl = toInt(it.get("PurchaseGLAC"));
                v.voucherDetailList.add(line(v.RefAccountId, gl, str(d.Remarks), dbl(d.Amount), 0, dbl(d.Qty), dbl(d.Rate), 0, 0));
                v.voucherDetailList.add(line(gl, v.RefAccountId, str(d.Remarks), 0, dbl(d.Amount), dbl(d.Qty), dbl(d.Rate), 0, 0));
            }
        }
        if (!o.outputs.isEmpty()) {
            List<Map<String, Object>> items = repo.partyProcessingItemGl(u);
            for (ProcessingBillOutput d : o.outputs) {
                /* Removed rows (ActionTypeId 3) never reach here on the desktop (its rows never carry a saved Id); they are kept out of the voucher. */
                if (d.ActionTypeId == 3) continue;
                if (!(d.ItemId > 0 && d.Amount.signum() > 0)) throw invalid("By Product ItemId or Item Amount not found");
                Map<String, Object> it = find(items, "Id", d.ItemId);
                if (it == null) throw invalid("By Product ItemId or Item Amount not found");
                String tail = str(it.get("ItemName")) + " Qty " + net(d.Qty) + " Weight " + net(d.Weight) + " Rate " + net(d.Rate) + " Amount " + net(d.Amount);
                String text = str(d.Remarks).isEmpty() ? tail : d.Remarks + " " + tail;
                v.voucherDetailList.add(line(d.DebitAccountId, v.RefAccountId, text, dbl(d.Amount), 0, dbl(d.Qty), dbl(d.Rate), 0, 0));
                v.voucherDetailList.add(line(v.RefAccountId, d.DebitAccountId, text, 0, dbl(d.Amount), dbl(d.Qty), dbl(d.Rate), 0, 0));
            }
        }
        return v;
    }

    private static VoucherDetail line(int account, int against, String comments, double debit, double credit, double qty, double rate,
                                      double weightIn, int supplierCustomerId) {
        VoucherDetail d = new VoucherDetail();
        d.LineId = 0;
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.QtyIn = qty;
        d.ItemRate = rate;
        d.WeightIn = weightIn;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.SupplierCustomerId = supplierCustomerId;
        return d;
    }

    /** btnDelete_Click: DeleteByID(UserAccount.ID, RECID) (button enabled by DoHaveCanDelete). */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.require(u, SCREEN, "Delete");
        header(u, id);
        repo.billDelete(uid(u), id);
        return saved(id, "Delete Record Successfully");
    }

    /** BindHistoryGrid: [dbo].[USP_InvProductionProcessingBill_FormHistory]. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = pp.history(u, SCREEN, b, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.year(), "DocumentTypeId", DOC_TYPE, "BranchesId", branch(u) == 0 ? null : branch(u)},
                "FromDate", "ToDate", "DocNoFrom", "DocNoTo");
        int jo = toInt(b.get("jobOrderId"));
        if (jo != 0) p.put("InvJobOrderId", jo);
        return project(repo.billHistory(p), "Id", "BillDate", "BillNo", "DocumentTypeId", "InvProductionJobOrderId", "JobOrderNo", "STockPartyId", "StockParty",
                "CreditAccountId", "CreditAccount", "Qty", "Weight", "Rate", "RateUom=RateUomCode", "Amount", "TotalBPPurchaseAmt", "RemarksHeader",
                "ApprovalStatus", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName", "ApprovedDate", "ApprovedUserName", "NoOfAttachments");
    }

    /** History "Voucher" button: CommonServices.VoucherHeadIdGet(Id, 123) -> VoucherReport_118. */
    public Map<String, Object> voucher(int id) {
        UserAccount u = pp.user(SCREEN);
        header(u, id);
        return m("voucherHeadId", repo.voucherHeadId(u, DOC_TYPE, id));
    }

    /**
     * Print (BtnPrint, Print right) and the history row's Print (not rights-gated on the desktop):
     * ProductionPartyProcessingBillReport_611(JobOrderId, Id).
     */
    public Map<String, Object> printCheck(int id, boolean fromHistory) {
        UserAccount u = pp.user(SCREEN);
        if (!fromHistory) pp.require(u, SCREEN, "Print");
        Map<String, Object> h = header(u, id);
        return m("id", id, "jobOrderId", toInt(h.get("InvProductionJobOrderId")));
    }
}
