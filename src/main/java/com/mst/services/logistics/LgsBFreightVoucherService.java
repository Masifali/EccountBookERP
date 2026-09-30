package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsBFreightExpenseDetail;
import com.mst.models.logistics.LgsBFreightPaymentDetail;
import com.mst.models.logistics.LgsBFreightVoucherOutward;
import com.mst.models.logistics.LgsBVoucherDetail;
import com.mst.models.logistics.LgsBVoucherHead;
import com.mst.repositories.logistics.LgsBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.logistics.LgsBPurchaseOrderService.combo;
import static com.mst.services.logistics.LgsBPurchaseOrderService.field;
import static com.mst.services.logistics.LgsBPurchaseOrderService.nonZeroDouble;
import static com.mst.services.logistics.LgsBPurchaseOrderService.nonZeroInt;
import static com.mst.services.logistics.LgsBPurchaseOrderService.text;
import static com.mst.services.logistics.LgsBSupport.*;

/**
 * 965 Freight Voucher (Export)                 frmFreightVoucherExport (+ loader frmLoadPendingGpForFreightVoucher)
 * 967 Freight Voucher Export (Multi Vehicles)  frmFreightVoucherExportMultiVehicles
 *
 * Both are DocumentTypeId 1305 over BLL lgstcm.FreightVoucherOutward (0387) / DAL (0344). Save = BLL Save / SaveMulti:
 * MakeVoucher per voucher, then DAL SetData in one transaction per call: [lgstcm].[USP_FreightVoucherOutward_InsertAndUpdate],
 * [lgstcm].[USP_FreightVoucherOutwardPaymentDetail_Insert] (LineId 1..n, CheqDate = now when unset),
 * [lgstcm].[USP_FreightVoucherOutwardExpenseDetail_Insert], then the voucher with USP_VoucherBalanceCheck (LgsBVoucherPoster).
 */
@Service
public class LgsBFreightVoucherService {

    public static final int SCREEN_SINGLE = 965;
    public static final int SCREEN_MULTI = 967;
    public static final String NAME_SINGLE = "frmFreightVoucherExport";
    public static final String NAME_MULTI = "frmFreightVoucherExportMultiVehicles";
    public static final int DOC_TYPE = 1305;

    @Autowired private LgsBSupport sup;
    @Autowired private LgsBRepository repo;
    @Autowired private LgsBVoucherPoster poster;

    private static int screen(boolean multi) { return multi ? SCREEN_MULTI : SCREEN_SINGLE; }

    // ================================================================== load

    /**
     * InitializeComponentMethod of either form: rights, the next DocNo (single) / MasterDocNo (multi), LookUpAllServices
     * (RateBaseUom with its Equivalent, BillWeightBase), transporters (group 10), cities, the Charges To (Dr) account
     * (ChargesToAccountForFreightVoucherOutward), configuration; the single form adds the instrument types, the pending
     * gate passes / delivery orders, the other-charges items and the Cash / Bank / Other account lists; the multi form the
     * loader's customer combo.
     */
    public Map<String, Object> setup(boolean multi, int recId) {
        UserAccount u = sup.user(screen(multi));
        Map<String, Object> out = lists(u, multi, recId);
        out.put("rights", sup.rights(u, screen(multi), multi ? NAME_MULTI : NAME_SINGLE));
        out.put("docNo", multi ? repo.fvGenerateMasterDocNo(u, sup.fy(), DOC_TYPE) : repo.fvGenerateCode(u, sup.fy(), DOC_TYPE));
        out.put("yearStart", sup.yearStart(u));
        return out;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> refresh(boolean multi, int recId) { return lists(sup.user(screen(multi)), multi, recId); }

    private Map<String, Object> lists(UserAccount u, boolean multi, int recId) {
        Map<String, Object> c = sup.formats(u);
        int chargesTo = toInt(sup.config(u, "ChargesToAccountForFreightVoucherOutward"));
        c.put("chargesToAccount", chargesTo);
        c.put("defaultDays", toInt(sup.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("chequeBookEnabled", toBool(sup.config(u, "CheqBook Enabled")));
        List<Map<String, Object>> accounts = sup.accounts(u);
        /* DrAccountBindFromGlobal: AllAccountsWithCustomGroupId where ChartOfAccountId == the configured account. */
        List<Map<String, Object>> dr = new ArrayList<>();
        for (Map<String, Object> a : accounts) {
            if (toInt(a.get("ChartOfAccountId")) == chargesTo) {
                dr.add(map("Id", chargesTo, "AccountTitle", str(a.get("AccountTitle")), "AccountCode", str(a.get("AccountCode"))));
            }
        }
        Map<String, Object> out = map("config", c, "lookups", sup.lookUps(u), "suppliers", sup.suppliers(u, true), "cities", sup.cities(u),
                "chargesAccounts", dr);
        if (multi) {
            out.put("customers", repo.gpoCustomers(u));
        } else {
            out.put("instrumentTypes", repo.instrumentTypes());
            out.put("pendingGp", repo.gpPending(u, sup.fy(), recId, null, null, 0, 0, 0, 0, 1, false));
            out.put("otherCharges", repo.otherChargesItems(u));
            /* BillToCrAccountBindFromGlobal: 1 Cash = AccountTypeId 2, 2 Bank = 15, 3 Other = 3 / 6 / 8. */
            out.put("cashAccounts", sup.accountsOfTypes(accounts, 2));
            out.put("bankAccounts", sup.accountsOfTypes(accounts, 15));
            out.put("otherAccounts", sup.accountsOfTypes(accounts, 3, 6, 8));
        }
        return out;
    }

    /** CmbTransporter_Leave: PurchaseOrderHeader_PendingForFreightVoucherOutward (multi sends HeaderId 1 = @IsMasterDoc). */
    public List<Map<String, Object>> pendingPo(boolean multi, int partyId, int recId) {
        UserAccount u = sup.user(screen(multi));
        return repo.poPendingForFreight(u, sup.fy(), partyId, recId, multi ? 1 : 0);
    }

    /** GpAndDoBind(PendingGpAndDoDbCall()): ActionId 1, RecId. */
    public List<Map<String, Object>> pendingGp(int recId) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        return repo.gpPending(u, sup.fy(), recId, null, null, 0, 0, 0, 0, 1, false);
    }

    /** AccountCurrentBalance: VoucherHead.ReadByCurrentBalanceByDateAndAccountId (VoucherDate = now). */
    public Map<String, Object> balance(int accountId) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        List<Map<String, Object>> r = repo.currentBalance(u, sup.fy(), accountId, LocalDateTime.now());
        return r.isEmpty() ? map("found", false) : map("found", true, "Balance", toDouble(r.get(0).get("Balance")));
    }

    /** CheqNoFill: CheqBookHeader.OutstandingCheqNo(BankId, Id = VoucherHeadIdGet(RecId, 1305)). */
    public List<Map<String, Object>> cheques(int bankId, int recId) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        int vh = recId > 0 && ownFreight(u, recId) ? repo.voucherHeadId(u, DOC_TYPE, recId) : 0;
        return repo.outstandingCheques(u, bankId, vh);
    }

    /** BtnOtherChargesItem_Click / btnRefresh: LogiticOtherChargesItems.FormHistory. */
    public List<Map<String, Object>> otherCharges() { return repo.otherChargesItems(sup.user(SCREEN_SINGLE)); }

    // ================================================================== Gate Pass Loader (frmLoadPendingGpForFreightVoucher)

    /**
     * InitializeComponentMethod: customers (GatePassOutward.GetDataForDropDownFromGPO "Customer") and transporters
     * (GetDataForDropDownFromFreightVoucherOutward "Transporter").
     */
    public Map<String, Object> gpLoaderCombos() {
        UserAccount u = sup.user(SCREEN_SINGLE);
        return map("customers", repo.gpoCustomers(u), "transporters", repo.fvDropDown(u, null, "Transporter"));
    }

    /** PendingDataDbCall: GatePassOutward_PendingForFreightVoucherOutward (All 0 / Pending 1 / Complete 2, no RecId). */
    public List<Map<String, Object>> gpLoader(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        return repo.gpPending(u, sup.fy(), 0, toDate(f.get("fromDate")), toDate(f.get("toDate")), i(f, "fromDocNo"), i(f, "toDocNo"),
                i(f, "customerId"), i(f, "transporterId"), i(f, "actionId"), false);
    }

    /** BtnShowLoader_Click (multi): the same procedure with DocumentTypeId 1303 (not sent), ActionId 1, RecId, HeaderId 1. */
    public List<Map<String, Object>> multiLoader(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN_MULTI);
        int recId = i(f, "recId");
        if (recId > 0 && fvMaster(u, recId).isEmpty()) recId = 0;
        LocalDateTime from = toDate(f.get("fromDate")), to = toDate(f.get("toDate"));
        return repo.gpPending(u, sup.fy(), recId, from == null ? LocalDateTime.now() : from, to == null ? LocalDateTime.now() : to,
                i(f, "fromDocNo"), i(f, "toDocNo"), i(f, "customerId"), 0, 1, true);
    }

    // ================================================================== read

    private boolean ownFreight(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.fvById(id);
        return !r.isEmpty() && ownRow(u, r.get(0));
    }

    private List<Map<String, Object>> fvMaster(UserAccount u, int masterId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.fvByMaster(masterId)) if (ownRow(u, r)) out.add(r);
        return out;
    }

    /** ReadById(Id) (single): header, payment detail (credit rows and the debit record apart), expense detail. */
    public Map<String, Object> read(int id) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        List<Map<String, Object>> r = repo.fvById(id);
        if (r.isEmpty() || !ownRow(u, r.get(0))) throw invalid("No Record Found");
        List<Map<String, Object>> pay = new ArrayList<>(), dr = new ArrayList<>();
        for (Map<String, Object> p : repo.fvPaymentDetail(id)) {
            Map<String, Object> row = paymentRow(p);
            if (toDouble(p.get("DebitAmount")) > 0) dr.add(row); else pay.add(row);
        }
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : repo.fvExpenseDetail(id)) {
            exp.add(map("Id", toInt(e.get("FreightVoucherOutwardExpenseDetailId")), "ItemId", toInt(e.get("LogiticOtherChargesItemsId")),
                    "Qty", toDouble(e.get("Qty")), "Rate", toDouble(e.get("Rate")), "AddAmount", toDouble(e.get("AddAmount")),
                    "LessAmount", toDouble(e.get("LessAmount")), "Remarks", str(e.get("Remarks"))));
        }
        return map("header", r.get(0), "payments", pay, "drRecord", dr, "expenses", exp);
    }

    /**
     * FreightVoucherOutward_Helper.FillPaymentDetailFromList: Id, TransactionTypeId, TransactionType, InstrumentTypeId,
     * InstrumentType, AccountTitleId, AccountTitle, ChequeId, ChequeNo, ChequeDate, Amount, PayeeTitle, Remarks
     * (Amount = CreditAmount, or DebitAmount for the auto debit record).
     */
    private static Map<String, Object> paymentRow(Map<String, Object> p) {
        double amount = toDouble(p.get("DebitAmount")) > 0 ? toDouble(p.get("DebitAmount")) : toDouble(p.get("CreditAmount"));
        return map("Id", toInt(p.get("FreightVoucherOutwardPaymentDetailId")), "TransactionTypeId", toInt(p.get("TransTypeId")),
                "TransactionType", str(p.get("TransactionType")), "InstrumentTypeId", toInt(p.get("InstrumentTypeId")),
                "InstrumentType", str(p.get("InstrumentType")), "AccountTitleId", toInt(p.get("AccountId")), "AccountTitle", str(p.get("AccountTitle")),
                "ChequeId", toInt(p.get("CheqId")), "ChequeNo", str(p.get("CheqNo")), "ChequeDate", p.get("CheqDate"), "Amount", amount,
                "PayeeTitle", str(p.get("PayeeTitle")), "Remarks", str(p.get("Remarks")));
    }

    /** ReadById(Id) (multi): FreightVoucherOutward.ReadByMasterId -> FillMasterVoucherDetailFromList. */
    public Map<String, Object> readMulti(int masterId) {
        UserAccount u = sup.user(SCREEN_MULTI);
        List<Map<String, Object>> list = fvMaster(u, masterId);
        if (list.isEmpty()) throw invalid("No Record Found");
        return map("header", list.get(0), "rows", multiRows(list));
    }

    static List<Map<String, Object>> multiRows(List<Map<String, Object>> list) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : list) {
            out.add(map("Id", toInt(r.get("FreightVoucherOutwardId")), "DocNo", toInt(r.get("DocNo")), "DocDate", r.get("DocDate"),
                    "GPId", toInt(r.get("GatePassOutwardId")), "GpNo", toInt(r.get("GpSrNo")), "GpDate", r.get("GpDate"),
                    "DoId", toInt(r.get("InvDeliveryOrderId")), "DoNo", toInt(r.get("DeliveryOrderNo")), "DoDate", r.get("DeliveryOrderDate"),
                    "LoadingCityId", toInt(r.get("loadingFromCityId")), "LoadingCity", str(r.get("LoadingFromCityName")),
                    "UnLoadingCityId", toInt(r.get("unloadingToCityId")), "UnLoadingCity", str(r.get("UnloadingToCityName")),
                    "VehicleNo", str(r.get("vehicleNo")), "BiltyNo", str(r.get("biltyNo")), "BiltyDate", r.get("biltyDate"),
                    "PoId", toInt(r.get("PurchaseOrderHeaderId")), "PoNo", toInt(r.get("PurchaseOrderNo")),
                    "NoOfBages", toDouble(r.get("NoOfBags")), "NetWeightDo", toDouble(r.get("NetWeightDo")), "NetWeightWB", toDouble(r.get("NetWeightWB")),
                    "BillWeightBaseId", toInt(r.get("BillWeightBaseId")), "BillWeightBase", str(r.get("BillWeightBase")),
                    "BillWeight", toDouble(r.get("netWeight")), "FreightRate", toDouble(r.get("freightRate")),
                    "RateUomId", toInt(r.get("rateUomId")), "RateUom", str(r.get("RateUomCode")), "QtyForRate", toDouble(r.get("QtyforRate")),
                    "BiltyFreight", toDouble(r.get("biltyFreight")), "OtherCharges", toDouble(r.get("otherCharges")),
                    "TotalBiltyFreight", toDouble(r.get("totalbiltyfreight")), "AdvanceOrCashFreight", toDouble(r.get("AdvanceOrCashFreight")),
                    "NetCreditToTransporter", toDouble(r.get("totalFreight")), "Remarks", str(r.get("RemarksHeader"))));
        }
        return out;
    }

    // ================================================================== save (single)

    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        int recId = Math.max(0, i(b, "recId"));
        sup.require(u, SCREEN_SINGLE, recId > 0 ? "Update" : "Save");
        if (recId > 0 && !ownFreight(u, recId)) throw invalid("No Record Found");
        Map<String, Object> fmt = sup.formats(u);
        int amountDecimals = toInt(fmt.get("amountDecimals"));
        /* Insert(): FormHelper.ValidateControls in the form's order. */
        nonZeroInt(s(b, "docNo"), "Doc No");
        combo(i(b, "gpId"), "Gate Pass No");
        combo(i(b, "doId"), "Delivery Order No");
        combo(i(b, "transporterId"), "Transporter");
        combo(i(b, "loadingCityId"), "Loading City");
        combo(i(b, "unloadingCityId"), "Un-Loading City");
        text(s(b, "vehicleNo"), "Vehicle No");
        text(s(b, "biltyNo"), "Bilty No");
        nonZeroDouble(s(b, "noOfBags"), "No Of Bags");
        combo(i(b, "billWeightBaseId"), "Bill Weight Base");
        nonZeroDouble(s(b, "billWeight"), "Bill Weight");
        nonZeroDouble(s(b, "freightRate"), "Freight Rate");
        combo(i(b, "rateUomId"), "Rate Uom");
        nonZeroDouble(s(b, "qtyForRate"), "Qty For Rate");
        nonZeroDouble(s(b, "biltyFreight"), "Bilty Freight");

        List<Map<String, Object>> payments = list(b, "payments");
        List<Map<String, Object>> removed = list(b, "removed");
        List<Map<String, Object>> expenses = list(b, "expenses");
        List<Map<String, Object>> drRecord = list(b, "drRecord");
        /* CalculateNetPaid(): Other Charges = SUM(AddAmount) - SUM(LessAmount); Advance = SUM(grid Amount); each text in stringFormatsingle. */
        double bilty = toDouble(s(b, "biltyFreight"));
        double add = 0, less = 0, advance = 0;
        for (Map<String, Object> e : expenses) { add += d(e, "AddAmount"); less += d(e, "LessAmount"); }
        for (Map<String, Object> p : payments) advance += d(p, "Amount");
        BigDecimal other = fmt(add - less, amountDecimals), total = fmt(bilty + (add - less), amountDecimals);
        BigDecimal adv = fmt(advance, amountDecimals), net = fmt(bilty + (add - less) - advance, amountDecimals);
        if (toDouble(s(b, "netPayable")) < 0.0 || net.signum() < 0) throw invalid("Net Freight Payable cannot be lesser than zero.");
        if (i(b, "loadingCityId") == i(b, "unloadingCityId")) throw invalid("Loading City and Unloading City cannot be the same.");
        if (i(b, "chargesToAccountId") == 0) {
            throw invalid("Please select a Charges To Account (Dr).\n\nIf the list is empty, go to Configuration, set the default Charges Account's Custom Group, and then click Refresh and then Select an Account.");
        }
        Map<String, Object> transporter = transporter(u, i(b, "transporterId"));

        LocalDateTime now = LocalDateTime.now();
        LgsBFreightVoucherOutward o = new LgsBFreightVoucherOutward();
        o.FreightVoucherOutwardId = recId;
        o.CompanyId = u.getCompanyId();
        o.BranchesId = toInt(u.getBranchesId());
        o.ProjectsId = toInt(u.getBranchesId());
        o.OrganizationId = u.getOrganizationId();
        o.FinancialYearId = sup.fy();
        o.documentTypeId = DOC_TYPE;
        o.DocDate = orNow(toDate(b.get("docDate")));
        o.DocNo = toInt(s(b, "docNo"));
        o.GatePassOutwardId = i(b, "gpId");
        o.InvDeliveryOrderId = i(b, "doId");
        o.transporterId = i(b, "transporterId");
        o.TransporterSupCustId = toInt(transporter.get("GlAccountId"));
        o.loadingFromCityId = i(b, "loadingCityId");
        o.unloadingToCityId = i(b, "unloadingCityId");
        o.RemarksHeader = s(b, "remarks");
        o.vehicleNo = s(b, "vehicleNo");
        o.biltyNo = s(b, "biltyNo");
        o.biltyDate = orNow(toDate(b.get("biltyDate")));
        o.PurchaseOrderHeaderId = i(b, "poId");
        o.NoOfBags = dec(b.get("noOfBags"));
        o.NetWeightDo = dec(b.get("netWeightDo"));
        o.NetWeightWB = dec(b.get("netWeightWb"));
        o.BillWeightBaseId = i(b, "billWeightBaseId");
        o.netWeight = dec(b.get("billWeight"));
        o.freightRate = dec(b.get("freightRate"));
        o.rateUomId = i(b, "rateUomId");
        o.QtyforRate = dec(b.get("qtyForRate"));
        o.biltyFreight = dec(b.get("biltyFreight"));
        o.otherCharges = other;
        o.totalbiltyfreight = total;
        o.AdvanceOrCashFreight = adv;
        o.totalFreight = net;
        o.chargeToDrAccountId = i(b, "chargesToAccountId");
        o.EntryUserId = u.getId();
        o.EntryDate = now;
        o.ModifyDate = now;
        o.ModifyUserId = u.getId();
        o.ApprovedUserId = u.getId();
        o.ApprovedDate = now;
        o.IsApproved = false;
        o.ActionId = recId == 0 ? 1 : 2;
        Map<String, Object> existing = recId > 0 ? repo.fvById(recId).get(0) : null;
        o.AttachmentsValues = existing == null ? "" : str(existing.get("AttachmentsValues"));
        o.CustomAttachmentsValues = existing == null ? "" : str(existing.get("CustomAttachmentsValues"));
        /* Virtual properties MakeVoucher reads: GpSrNo = CmbGpNo.Value (the gate pass Id, as the form sets it),
           DeliveryOrderNo = the DO combo's text, RateUomCode = the Rate Uom combo's text. */
        Remark rm = new Remark(i(b, "gpId"), toInt(s(b, "doNo")), s(b, "rateUomCode"));

        List<Integer> ownPay = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> p : repo.fvPaymentDetail(recId)) ownPay.add(toInt(p.get("FreightVoucherOutwardPaymentDetailId")));
        List<LgsBFreightPaymentDetail> pays = new ArrayList<>();
        List<String> payTypes = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : removed) {
                int pid = i(r, "Id");
                if (pid <= 0 || !ownPay.contains(pid)) continue;
                LgsBFreightPaymentDetail d = payment(r);
                d.FreightVoucherOutwardPaymentDetailId = pid;
                d.ActionTypeId = 3;
                pays.add(d);
            }
        }
        for (int n = 0; n < payments.size(); n++) {
            Map<String, Object> r = payments.get(n);
            LgsBFreightPaymentDetail vd = payment(r);
            vd.FreightVoucherOutwardPaymentDetailId = recId != 0 ? i(r, "Id") : 0;
            if (vd.FreightVoucherOutwardPaymentDetailId > 0 && !ownPay.contains(vd.FreightVoucherOutwardPaymentDetailId)) throw invalid("No Record Found");
            vd.ActionTypeId = vd.FreightVoucherOutwardPaymentDetailId <= 0 ? 1 : 2;
            if (o.TransporterSupCustId == vd.AccountId) throw invalid("Transporter account cannot be add in grid please check....");
            field(vd.TransTypeId, "Transaction Type", n);
            if (vd.TransTypeId == 2) field(vd.InstrumentTypeId, "Instrument Type", n);
            field(vd.AccountId, "Account", n);
            field(vd.CreditAmount, "Amount", n);
            if (o.RemarksHeader == null || o.RemarksHeader.trim().isEmpty()) o.RemarksHeader = vd.Remarks;
            pays.add(vd);
        }
        BigDecimal debitAmount = net;
        if (drRecord.isEmpty() && debitAmount.signum() > 0) {
            LgsBFreightPaymentDetail vd2 = new LgsBFreightPaymentDetail();
            vd2.FreightVoucherOutwardPaymentDetailId = 0;
            vd2.ActionTypeId = 1;
            vd2.TransTypeId = 3;
            vd2.InstrumentTypeId = 3;
            vd2.AccountId = o.chargeToDrAccountId;
            vd2.CheqId = 0;
            vd2.CheqNo = "";
            vd2.CheqDate = now;
            vd2.DebitAmount = debitAmount;
            vd2.PayeeTitle = "";
            vd2.Remarks = "";
            pays.add(vd2);
        } else if (!drRecord.isEmpty()) {
            Map<String, Object> dr = drRecord.get(0);
            LgsBFreightPaymentDetail vd3 = new LgsBFreightPaymentDetail();
            vd3.FreightVoucherOutwardPaymentDetailId = recId != 0 ? i(dr, "Id") : 0;
            if (vd3.FreightVoucherOutwardPaymentDetailId > 0 && !ownPay.contains(vd3.FreightVoucherOutwardPaymentDetailId)) throw invalid("No Record Found");
            vd3.ActionTypeId = vd3.FreightVoucherOutwardPaymentDetailId == 0 ? 1 : (debitAmount.signum() > 0 ? 2 : 3);
            vd3.TransTypeId = i(dr, "TransactionTypeId");
            vd3.InstrumentTypeId = i(dr, "InstrumentTypeId");
            vd3.AccountId = i(dr, "AccountTitleId");
            vd3.CheqId = i(dr, "ChequeId");
            vd3.CheqNo = s(dr, "ChequeNo");
            vd3.CheqDate = toDate(dr.get("ChequeDate"));
            /* The desktop keeps the debit record's stored amount here (dtPaymentDetailDrRecord), not the new Net Freight. */
            vd3.DebitAmount = dec(dr.get("Amount"));
            vd3.PayeeTitle = s(dr, "PayeeTitle");
            vd3.Remarks = s(dr, "Remarks");
            pays.add(vd3);
        }
        List<LgsBFreightExpenseDetail> exps = new ArrayList<>();
        for (int n = 0; n < expenses.size(); n++) {
            Map<String, Object> r = expenses.get(n);
            if (i(r, "ItemId") == 0) continue;
            if (!(d(r, "Qty") > 0 || d(r, "Rate") > 0 || d(r, "AddAmount") > 0 || d(r, "LessAmount") > 0)) continue;
            if (d(r, "AddAmount") == 0.0 && d(r, "LessAmount") == 0.0)
                throw invalid("AddAmount or LessAmount Field Required in Expense Grid Row no : " + (n + 1));
            LgsBFreightExpenseDetail pe = new LgsBFreightExpenseDetail();
            pe.FreightVoucherOutwardExpenseDetailId = i(r, "Id");
            pe.LogiticOtherChargesItemsId = i(r, "ItemId");
            pe.Qty = toInt(r.get("Qty"));
            pe.Rate = d(r, "Rate");
            pe.AddAmount = d(r, "AddAmount");
            pe.LessAmount = d(r, "LessAmount");
            pe.Remarks = s(r, "Remarks");
            exps.add(pe);
        }
        o.FreightVoucherOutwardId = recId;
        LgsBVoucherPoster.Voucher v = makeVoucher(o, rm, pays);
        int id = repo.tx(() -> setData(u, o, pays, exps, v));
        return map("success", true, "id", id, "message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + o.DocNo,
                "docNo", repo.fvGenerateCode(u, sup.fy(), DOC_TYPE));
    }

    private static LocalDateTime orNow(LocalDateTime d) { return d == null ? LocalDateTime.now() : d; }

    /** x.ToString(stringFormatsingle) read back with Conversion.ToDecimal: custom-format rounding is away from zero. */
    private static BigDecimal fmt(double v, int decimals) {
        return new BigDecimal(Double.toString(v)).setScale(decimals, RoundingMode.HALF_UP);
    }

    /** The transporter row of dtSupplier (group 10) - Cells[3] is its GlAccountId. */
    private Map<String, Object> transporter(UserAccount u, int id) {
        for (Map<String, Object> s : sup.suppliers(u, true)) if (toInt(s.get("Id")) == id) return s;
        throw invalid("Transporter field is required");
    }

    /** FillDetailListCommonForInsertAndDelete (payment grid row). */
    private static LgsBFreightPaymentDetail payment(Map<String, Object> r) {
        LgsBFreightPaymentDetail vd = new LgsBFreightPaymentDetail();
        vd.TransTypeId = i(r, "TransactionTypeId");
        vd.InstrumentTypeId = i(r, "InstrumentTypeId");
        vd.AccountId = i(r, "AccountTitleId");
        vd.CheqId = i(r, "ChequeId");
        vd.CheqNo = s(r, "ChequeNo");
        vd.CheqDate = toDate(r.get("ChequeDate"));
        vd.CreditAmount = dec(r.get("Amount"));
        vd.PayeeTitle = s(r, "PayeeTitle");
        vd.Remarks = s(r, "Remarks");
        return vd;
    }

    /** The virtual properties MakeVoucher reads from the model. */
    private static final class Remark {
        final int gpSrNo; final int deliveryOrderNo; final String rateUomCode;
        Remark(int gpSrNo, int deliveryOrderNo, String rateUomCode) { this.gpSrNo = gpSrNo; this.deliveryOrderNo = deliveryOrderNo; this.rateUomCode = rateUomCode; }
    }

    /**
     * FreightVoucherOutward.MakeVoucher: "Charge To Debit Account field is required." when unset; RefAccountId = Charges To (Dr);
     * a Dr/Cr pair per payment row (ActionTypeId != 3, CreditAmount > 0) with LineId = its position (1-based, every row
     * counted); when totalFreight > 0 a Dr (charges account) / Cr (transporter) pair with LineId = rows + 1;
     * VoucherAmount = BillAmount = totalbiltyfreight.
     */
    private static LgsBVoucherPoster.Voucher makeVoucher(LgsBFreightVoucherOutward o, Remark rm, List<LgsBFreightPaymentDetail> pays) {
        LocalDateTime now = LocalDateTime.now();
        LgsBVoucherHead vh = new LgsBVoucherHead();
        vh.DocumentTypeId = o.documentTypeId;
        vh.DocumentTypeSrNo = o.FreightVoucherOutwardId;
        vh.RefDocNoId = o.FreightVoucherOutwardId;
        vh.VoucherCode = o.DocNo;
        vh.VoucherDate = o.DocDate;
        vh.BranchId = o.BranchesId;
        vh.ProjectId = o.ProjectsId;
        vh.OrganizationId = o.OrganizationId;
        vh.CompanyId = o.CompanyId;
        vh.FinancialYearId = o.FinancialYearId;
        vh.EntryUser = o.EntryUserId;
        vh.ModifyUser = o.ModifyUserId;
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.IncludeWHT = false;
        vh.Remarks = str(o.RemarksHeader);
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = now.toLocalDate().atStartOfDay();
        if (o.chargeToDrAccountId == 0) throw invalid("Charge To Debit Account field is required.");
        vh.RefAccountId = o.chargeToDrAccountId;
        List<LgsBVoucherDetail> lines = new ArrayList<>();
        int num = 1;
        for (LgsBFreightPaymentDetail p : pays) {
            if (p.ActionTypeId != 3 && p.CreditAmount.signum() > 0) {
                List<String> parts = common(o, rm);
                if (p.InstrumentTypeId == 1 && !str(p.CheqNo).trim().isEmpty()) parts.add("CheqNo: " + p.CheqNo);
                if (p.InstrumentTypeId == 1) parts.add("CheqDate: " + netDateTime(p.CheqDate));
                if (!(p.InstrumentTypeId != 1 && str(p.PayeeTitle).trim().isEmpty())) parts.add("PayeeTitle: " + str(p.PayeeTitle));
                String auto = String.join(" ", nonBlank(parts));
                String text = str(p.Remarks).trim().isEmpty() ? auto : p.Remarks + " " + auto;
                lines.add(payLine(vh.RefAccountId, p.AccountId, p, true, text, num));
                lines.add(payLine(p.AccountId, vh.RefAccountId, p, false, text, num));
                lines.get(lines.size() - 1).RefDocSubIdNo = p.FreightVoucherOutwardPaymentDetailId;
                lines.get(lines.size() - 2).RefDocSubIdNo = p.FreightVoucherOutwardPaymentDetailId;
            }
            num++;
        }
        if (o.totalFreight.signum() > 0) {
            String auto = String.join(" ", nonBlank(common(o, rm)));
            String text = str(o.RemarksHeader).trim().isEmpty() ? auto : o.RemarksHeader + " " + auto;
            String comments = text.trim().isEmpty() ? str(o.RemarksHeader) : text;
            LgsBVoucherDetail d1 = new LgsBVoucherDetail();
            d1.AccountId = vh.RefAccountId;
            d1.AgainstAccountId = o.TransporterSupCustId;
            d1.Comments = comments;
            d1.DebitAmount = o.totalFreight.doubleValue();
            d1.CreditAmount = 0.0;
            d1.LineId = num;
            lines.add(d1);
            LgsBVoucherDetail d2 = new LgsBVoucherDetail();
            d2.AccountId = o.TransporterSupCustId;
            d2.AgainstAccountId = vh.RefAccountId;
            d2.Comments = comments;
            d2.CreditAmount = o.totalFreight.doubleValue();
            d2.DebitAmount = 0.0;
            d2.LineId = num;
            lines.add(d2);
        }
        vh.VoucherAmount = o.totalbiltyfreight.doubleValue();
        vh.BillAmount = o.totalbiltyfreight.doubleValue();
        return new LgsBVoucherPoster.Voucher(vh, lines);
    }

    /** The 13 remark parts common to both line kinds ($"...{decimal}" prints the decimal with its own scale). */
    private static List<String> common(LgsBFreightVoucherOutward o, Remark rm) {
        List<String> p = new ArrayList<>();
        if (rm.gpSrNo != 0) p.add("Gp No: " + rm.gpSrNo);
        if (rm.deliveryOrderNo != 0) p.add("Delivery Order No: " + rm.deliveryOrderNo);
        if (!str(o.vehicleNo).trim().isEmpty()) p.add("Vehicle No: " + o.vehicleNo);
        if (!str(o.biltyNo).trim().isEmpty()) p.add("Bilty No: " + o.biltyNo);
        if (o.NoOfBags.signum() != 0) p.add("NoOfBags: " + netDecimal(o.NoOfBags));
        if (o.netWeight.signum() != 0) p.add("Weight: " + netDecimal(o.netWeight));
        if (o.freightRate.signum() != 0) p.add("Freight Rate: " + netDecimal(o.freightRate));
        if (!str(rm.rateUomCode).trim().isEmpty()) p.add("Rate Uom: " + rm.rateUomCode);
        if (o.biltyFreight.signum() != 0) p.add("Bilty Freight: " + netDecimal(o.biltyFreight));
        if (o.otherCharges.signum() != 0) p.add("Add Other Charges: " + netDecimal(o.otherCharges));
        if (o.totalbiltyfreight.signum() != 0) p.add("Total Bilty Freight: " + netDecimal(o.totalbiltyfreight));
        if (o.AdvanceOrCashFreight.signum() != 0) p.add("Less Advance / Cash: " + netDecimal(o.AdvanceOrCashFreight));
        if (o.totalFreight.signum() != 0) p.add("Net Credit To Transporter: " + netDecimal(o.totalFreight));
        return p;
    }

    private static List<String> nonBlank(List<String> in) {
        List<String> out = new ArrayList<>();
        for (String s : in) if (s != null && !s.trim().isEmpty()) out.add(s);
        return out;
    }

    /** FreightVoucherOutward.AddVoucherDetail. */
    private static LgsBVoucherDetail payLine(int account, int against, LgsBFreightPaymentDetail p, boolean debit, String auto, int lineId) {
        BigDecimal amount = p.DebitAmount.signum() > 0 ? p.DebitAmount : p.CreditAmount;
        LgsBVoucherDetail l = new LgsBVoucherDetail();
        l.AccountId = account;
        l.AgainstAccountId = against;
        l.Comments = auto == null || auto.trim().isEmpty() ? str(p.Remarks) : auto;
        l.DebitAmount = debit ? amount.doubleValue() : 0.0;
        l.CreditAmount = debit ? 0.0 : amount.doubleValue();
        l.InstrumentTypeId = p.InstrumentTypeId;
        l.InvoiceNoRefId = p.CheqId;
        l.CheqNoDetail = str(p.CheqNo);
        l.DCheqDate = p.CheqDate == null ? LocalDateTime.now() : p.CheqDate;
        l.PayeeTitle = str(p.PayeeTitle);
        l.LineId = lineId;
        return l;
    }

    /** DAL FreightVoucherOutward.SetData (inside the caller's transaction). */
    private int setData(UserAccount u, LgsBFreightVoucherOutward o, List<LgsBFreightPaymentDetail> pays, List<LgsBFreightExpenseDetail> exps,
                        LgsBVoucherPoster.Voucher v) {
        int n = repo.set("[lgstcm].[USP_FreightVoucherOutward_InsertAndUpdate]", o);
        o.FreightVoucherOutwardId = n > 0 ? n : o.FreightVoucherOutwardId;
        int line = 1;
        for (LgsBFreightPaymentDetail p : pays) {
            p.LineId = line;
            p.FreightVoucherOutwardId = o.FreightVoucherOutwardId;
            if (p.CheqDate == null) p.CheqDate = LocalDateTime.now();
            repo.exec("[lgstcm].[USP_FreightVoucherOutwardPaymentDetail_Insert]", p);
            line++;
        }
        for (LgsBFreightExpenseDetail e : exps) {
            e.FreightVoucherOutwardId = o.FreightVoucherOutwardId;
            repo.exec("[lgstcm].[USP_FreightVoucherOutwardExpenseDetail_Insert]", e);
        }
        poster.post(u, v.head, v.lines, o.FreightVoucherOutwardId, true);
        return o.FreightVoucherOutwardId;
    }

    // ================================================================== save (multi)

    public Map<String, Object> saveMulti(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN_MULTI);
        int recId = Math.max(0, i(b, "recId"));
        sup.require(u, SCREEN_MULTI, recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> rows = list(b, "rows");
        List<Map<String, Object>> removed = list(b, "removed");
        if (rows.isEmpty()) throw invalid("Please add at least one record in the grid.");
        nonZeroInt(s(b, "docNo"), "Doc No");
        combo(i(b, "transporterId"), "Transporter");
        if (i(b, "chargesToAccountId") == 0) {
            throw invalid("Please select a Charges To Account (Dr).\n\nIf the list is empty, go to Configuration, set the default Charges Account, and then click Refresh and then Select an Account.");
        }
        List<Integer> own = new ArrayList<>();
        if (recId > 0) {
            List<Map<String, Object>> m = fvMaster(u, recId);
            if (m.isEmpty()) throw invalid("No Record Found");
            for (Map<String, Object> r : m) own.add(toInt(r.get("FreightVoucherOutwardId")));
        }
        Map<String, Object> transporter = transporter(u, i(b, "transporterId"));
        List<LgsBFreightVoucherOutward> list = new ArrayList<>();
        List<Remark> remarks = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : removed) {
                int fid = i(r, "Id");
                if (fid <= 0 || !own.contains(fid)) continue;
                LgsBFreightVoucherOutward d = multiRow(u, b, r, recId, transporter);
                d.FreightVoucherOutwardId = fid;
                d.ActionId = 3;
                list.add(d);
                remarks.add(new Remark(i(r, "GpNo"), i(r, "DoNo"), s(r, "RateUom")));
            }
        }
        String masterRemarks = s(b, "remarks");
        int hdrRateUom = i(b, "rateUomId");
        BigDecimal hdrRate = dec(b.get("freightRate"));
        for (int n = 0; n < rows.size(); n++) {
            Map<String, Object> r = rows.get(n);
            LgsBFreightVoucherOutward vd = multiRow(u, b, r, recId, transporter);
            vd.FreightVoucherOutwardId = recId != 0 ? i(r, "Id") : 0;
            if (vd.FreightVoucherOutwardId > 0 && !own.contains(vd.FreightVoucherOutwardId)) throw invalid("No Record Found");
            vd.ActionId = vd.FreightVoucherOutwardId <= 0 ? 1 : 2;
            if (vd.PurchaseOrderHeaderId > 0) {
                if (vd.rateUomId != hdrRateUom)
                    throw invalid("Order RateUom and RateUom in Detail Grid at row No: " + (n + 1) + " are not same. Please press Apply button if you have changed the Purchase Order.");
                if (vd.freightRate.compareTo(hdrRate) != 0)
                    throw invalid("Order Rate and Rate in Detail Grid at row No: " + (n + 1) + " are not same. Please press Apply button if you have changed the Purchase Order.");
            }
            field(vd.GatePassOutwardId, "GP No", n);
            field(vd.InvDeliveryOrderId, "Do No", n);
            field(vd.loadingFromCityId, "Loading City", n);
            field(vd.unloadingToCityId, "Un-Loading City", n);
            if (vd.loadingFromCityId == vd.unloadingToCityId)
                throw invalid("Loading City and Unloading City cannot be the same in Detail Grid at row No: " + (n + 1));
            field(vd.vehicleNo, "Vehicle No", n);
            field(vd.biltyNo, "Bilty No", n);
            field(vd.NoOfBags, "No Of Bags", n);
            field(vd.BillWeightBaseId, "BillWeightBaseId", n);
            field(vd.netWeight, "BillWeight", n);
            field(vd.freightRate, "FreightRate", n);
            field(vd.rateUomId, "Rate Uom", n);
            field(vd.QtyforRate, "QtyForRate", n);
            field(vd.biltyFreight, "BiltyFreight", n);
            field(vd.totalbiltyfreight, "TotalBiltyFreight", n);
            if (masterRemarks.trim().isEmpty()) masterRemarks = vd.RemarksHeader;
            vd.AttachmentsValues = "";
            vd.CustomAttachmentsValues = "";
            list.add(vd);
            remarks.add(new Remark(i(r, "GpNo"), i(r, "DoNo"), s(r, "RateUom")));
        }
        int last = repo.tx(() -> {
            int result = 0;
            for (int k = 0; k < list.size(); k++) {
                LgsBFreightVoucherOutward o = list.get(k);
                if (o.ActionId == 3) {
                    /* A removed vehicle: the procedure deletes the voucher row and its accounting voucher. The desktop then
                       re-posts a voucher for it (MakeVoucher + Sp_VoucherHead_Insert); that orphan posting is not repeated. */
                    int n = repo.set("[lgstcm].[USP_FreightVoucherOutward_InsertAndUpdate]", o);
                    result = n > 0 ? n : o.FreightVoucherOutwardId;
                    continue;
                }
                LgsBVoucherPoster.Voucher v = makeVoucher(o, remarks.get(k), new ArrayList<>());
                result = setData(u, o, new ArrayList<>(), new ArrayList<>(), v);
            }
            return result;
        });
        return map("success", true, "id", last, "message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(s(b, "docNo")),
                "docNo", repo.fvGenerateMasterDocNo(u, sup.fy(), DOC_TYPE));
    }

    /** FillDetailListCommonForInsertAndDelete (multi): the header fields plus one grid row. */
    private LgsBFreightVoucherOutward multiRow(UserAccount u, Map<String, Object> b, Map<String, Object> r, int recId, Map<String, Object> transporter) {
        LocalDateTime now = LocalDateTime.now();
        LgsBFreightVoucherOutward vd = new LgsBFreightVoucherOutward();
        vd.MasterDocId = recId;
        vd.CompanyId = u.getCompanyId();
        vd.BranchesId = toInt(u.getBranchesId());
        vd.ProjectsId = toInt(u.getBranchesId());
        vd.OrganizationId = u.getOrganizationId();
        vd.FinancialYearId = sup.fy();
        vd.documentTypeId = DOC_TYPE;
        vd.EntryUserId = u.getId();
        vd.EntryDate = now;
        vd.ModifyDate = now;
        vd.ModifyUserId = u.getId();
        vd.ApprovedUserId = u.getId();
        vd.ApprovedDate = now;
        vd.IsApproved = false;
        vd.MasterDocDate = orNow(toDate(b.get("docDate")));
        vd.MasterDocNo = toInt(s(b, "docNo"));
        vd.MasterDocRemarks = s(b, "remarks");
        vd.transporterId = i(b, "transporterId");
        vd.TransporterSupCustId = toInt(transporter.get("GlAccountId"));
        vd.DocNo = i(r, "DocNo");
        vd.DocDate = orNow(toDate(r.get("DocDate")));
        vd.GatePassOutwardId = i(r, "GPId");
        vd.InvDeliveryOrderId = i(r, "DoId");
        vd.loadingFromCityId = i(r, "LoadingCityId");
        vd.unloadingToCityId = i(r, "UnLoadingCityId");
        vd.vehicleNo = s(r, "VehicleNo");
        vd.biltyNo = s(r, "BiltyNo");
        vd.biltyDate = orNow(toDate(r.get("BiltyDate")));
        vd.PurchaseOrderHeaderId = i(r, "PoId");
        vd.NoOfBags = dec(r.get("NoOfBages"));
        vd.NetWeightDo = dec(r.get("NetWeightDo"));
        vd.NetWeightWB = dec(r.get("NetWeightWB"));
        vd.BillWeightBaseId = i(r, "BillWeightBaseId");
        vd.netWeight = dec(r.get("BillWeight"));
        vd.freightRate = dec(r.get("FreightRate"));
        vd.rateUomId = i(r, "RateUomId");
        vd.QtyforRate = dec(r.get("QtyForRate"));
        vd.biltyFreight = dec(r.get("BiltyFreight"));
        vd.otherCharges = dec(r.get("OtherCharges"));
        vd.totalbiltyfreight = dec(r.get("TotalBiltyFreight"));
        vd.AdvanceOrCashFreight = dec(r.get("AdvanceOrCashFreight"));
        vd.totalFreight = dec(r.get("NetCreditToTransporter"));
        vd.RemarksHeader = s(r, "Remarks");
        vd.chargeToDrAccountId = i(b, "chargesToAccountId");
        return vd;
    }

    // ================================================================== delete / history / prints

    public Map<String, Object> delete(boolean multi, int id) {
        UserAccount u = sup.user(screen(multi));
        sup.require(u, screen(multi), "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        if (multi) {
            if (fvMaster(u, id).isEmpty()) throw invalid("No Record Found");
            repo.tx(() -> { repo.fvDeleteByMaster(u.getId(), id); return 0; });
        } else {
            if (!ownFreight(u, id)) throw invalid("No Record Found");
            repo.tx(() -> { repo.fvDelete(u.getId(), id); return 0; });
        }
        return map("success", true, "message", "Delete Record Successfully",
                "docNo", multi ? repo.fvGenerateMasterDocNo(u, sup.fy(), DOC_TYPE) : repo.fvGenerateCode(u, sup.fy(), DOC_TYPE));
    }

    /** HistoryGridFill: FreightVoucherOutward.FormHistory (the multi form de-duplicates by MasterDocId on the page). */
    public List<Map<String, Object>> history(boolean multi, Map<String, Object> f) {
        UserAccount u = sup.user(screen(multi));
        Map<String, Object> p = sup.historyParams(u, DOC_TYPE, sup.canViewAll(u, multi ? NAME_MULTI : NAME_SINGLE), f);
        return repo.formHistory("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p);
    }

    /** GetDetailGrdByHeadId: the payment detail of one voucher (single) / the vouchers of one master document (multi). */
    public List<Map<String, Object>> historyDetail(boolean multi, int id) {
        UserAccount u = sup.user(screen(multi));
        if (multi) {
            List<Map<String, Object>> m = fvMaster(u, id);
            return multiRows(m);
        }
        if (!ownFreight(u, id)) throw invalid("No Record Found");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> p : repo.fvPaymentDetail(id)) out.add(paymentRow(p));
        return out;
    }

    public Map<String, Object> printCheck(boolean multi, int id) {
        UserAccount u = sup.user(screen(multi));
        sup.require(u, screen(multi), "Print");
        return map("id", id);
    }

    /** btnPrintVoucher_Click (single): VoucherHeadIdGet(RecId, 1305) for the 102 slip; "Record Not Found For Display" when none. */
    public Map<String, Object> voucherId(int id) {
        UserAccount u = sup.user(SCREEN_SINGLE);
        sup.require(u, SCREEN_SINGLE, "Print");
        if (id <= 0 || !ownFreight(u, id)) throw invalid("Record Not Found For Display");
        return map("voucherHeadId", repo.voucherHeadId(u, DOC_TYPE, id));
    }

    /**
     * PrintVoucher(PrintId) (multi): FreightVoucherOutward_GetVoucherHeadIds(MasterId) -> "id1,id2,..." for the 102 slip
     * (ANewAcRptPaymentReceiptsVoucherSlip_102(0, HeaderIds)). The desktop reads RecId instead of PrintId (see the report);
     * the id asked for is used here.
     */
    public Map<String, Object> voucherIds(int masterId) {
        UserAccount u = sup.user(SCREEN_MULTI);
        sup.require(u, SCREEN_MULTI, "Print");
        if (masterId <= 0 || fvMaster(u, masterId).isEmpty()) throw invalid("Record Not Found For Display");
        List<String> ids = new ArrayList<>();
        for (Map<String, Object> r : repo.fvVoucherHeadIds(u, sup.fy(), DOC_TYPE, 0, masterId)) ids.add(str(r.get("VoucherHeadId")));
        return map("ids", String.join(",", ids));
    }
}
