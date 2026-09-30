package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsAClearingAgentBill;
import com.mst.models.logistics.LgsAClearingAgentBillDetail;
import com.mst.models.logistics.LgsAVoucherDetail;
import com.mst.models.logistics.LgsAVoucherHead;
import com.mst.repositories.logistics.LgsALookupRepository;
import com.mst.repositories.logistics.LgsAServiceBillRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of 233 "Service Bill Direct" - Architecture.WinApp.Service/ExImClearingAgentBillDirect.cs
 * (ScreenName "ExImClearingAgentBillDirect", DocumentTypeId 155). Validation order and texts are the
 * form's; FormHelper.ValidateControls texts: combo / string box -> "&lt;name&gt; field is required",
 * numeric box -> "&lt;name&gt; must be a non-zero number".
 */
@Service
public class LgsAServiceBillService {

    public static final int SCREEN = 233;
    public static final int DOC_TYPE = 155;

    @Autowired private LgsAServiceBillRepository repo;
    @Autowired private LgsALookupRepository look;
    @Autowired private HrmSupport hrm;

    // ================================================================== load / lists

    private Map<String, Object> rights(UserAccount u) {
        Map<String, Object> r = hrm.rights(u, SCREEN);
        r.put("canViewAllRecord", hrm.can(u, SCREEN, "CanView AllRecord"));
        return r;
    }

    /** GetConfigurationsFromGlobal. */
    private Map<String, Object> configs(UserAccount u) {
        return map("DefaultDaysToLessFromHistoryFromDate", toInt(look.config(u, "DefaultDaysToLessFromHistoryFromDate")),
                "DebitAccountConfig", toBool(look.config(u, "ServicesBillDebitAmountChargetoSelectedAc")),
                "ItemSearchWithNameOrCode", toBool(look.config(u, "ItemSearchWithNameOrCode")));
    }

    /**
     * InitializeComponentMethod: rights, GetDocumentCode, PortsDbCall, GetConfigurationsFromGlobal,
     * getExportInvoicePendingAndAll, then the binds: invoices, ports (four combos), parties (customer group 10),
     * Delivered At (1 = the company's name, 2 = "Other"), services items, currencies, and the debit accounts
     * (types 6, 8, 10) only when "ServicesBillDebitAmountChargetoSelectedAc" is on.
     */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN);
        Map<String, Object> out = lists(u);
        out.put("rights", rights(u));
        out.put("docNo", repo.generateCode(u, DOC_TYPE, hrm.financialYearId()));
        return out;
    }

    /** btnRefresh_Click: the globals re-read and every list re-bound. */
    public Map<String, Object> refresh() { return lists(hrm.user(SCREEN)); }

    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> cfg = configs(u);
        String compName = "";
        for (Map<String, Object> c : look.companies(u)) if (toInt(c.get("Id")) == u.getCompanyId()) compName = str(c.get("CompName"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", cfg);
        out.put("invoices", look.exportInvoices(u));
        out.put("ports", look.ports(u));
        out.put("parties", look.partiesGroup10(u));
        out.put("deliveredAt", List.of(map("Id", 1, "Name", compName), map("Id", 2, "Name", "Other")));
        out.put("items", look.servicesItems(u));
        out.put("currencies", look.currencies(u));
        out.put("accounts", toBool(cfg.get("DebitAccountConfig")) ? look.accountsByTypes(u, 6, 8, 10) : new ArrayList<>());
        return out;
    }

    /** reset() -> UpdateDocumentNoUI(GetDocumentCode()). */
    public Map<String, Object> docNo() {
        return map("docNo", repo.generateCode(hrm.user(SCREEN), DOC_TYPE, hrm.financialYearId()));
    }

    /** cmbitem_Leave -> ItemUomFromGlobalBind(ItemId). */
    public List<Map<String, Object>> uoms(int itemId) { return look.itemUoms(hrm.user(SCREEN), itemId); }

    /**
     * cmbeximinvoiceno_Leave -> DataByInvoiceDbCall -> BindInvoiceData: the invoice's own values, in the form's
     * formats (M.Ton = NetWeight / 1000 "#,##0.###", CRO rate "#,##0.###"), contracts (LcOrderNoId, LcOrderNo, row 0 active).
     */
    public Map<String, Object> invoiceData(int invoiceId) {
        return invoiceInfo(hrm.user(SCREEN), invoiceId);
    }

    private Map<String, Object> invoiceInfo(UserAccount u, int invoiceId) {
        List<Map<String, Object>> dt = invoiceId > 0 ? repo.invoiceData(u, invoiceId) : new ArrayList<>();
        if (dt.isEmpty()) return map("found", false);
        Map<String, Object> r0 = dt.get(0);
        List<Map<String, Object>> contracts = new ArrayList<>();
        for (Map<String, Object> r : dt) contracts.add(map("LcOrderNoId", toInt(r.get("LcOrderNoId")), "LcOrderNo", str(r.get("LcOrderNo"))));
        double mton = toDouble(r0.get("NetWeight")) / 1000.0;
        return map("found", true, "CustomerName", str(r0.get("CustomerName")), "ForwarderName", str(r0.get("ForwarderName")),
                "contracts", contracts, "BLNumber", str(r0.get("BLNumber")), "GdNo", str(r0.get("GdNo")),
                "LoadingPortId", toInt(r0.get("LoadingPortId")), "DestinationPortId", toInt(r0.get("DestinationPortId")),
                "ContainerDispatchedAtPortId", toInt(r0.get("ContainerDispatchedAtPortId")),
                "ContainerReceivedFromPortId", toInt(r0.get("ContainerReceivedFromPortId")), "DeliveredAtId", toInt(r0.get("DeliveredAtId")),
                "VesselName", str(r0.get("VesselName")), "DeliveryTerm", str(r0.get("DeliveryTerm")),
                "NoOfContainers", str(r0.get("NoOfContainers") instanceof BigDecimal ? ((BigDecimal) r0.get("NoOfContainers")).toPlainString() : r0.get("NoOfContainers")),
                "MTon", fmt(mton, 3, true), "BookingRate", fmt(toDouble(r0.get("BookingRate")), 3, true),
                "CreditAccountId", toInt(r0.get("CreditAccountId")));
    }

    // ================================================================== history

    /**
     * HistoryGridFill: FormHistoryDirect with the ticked date kind (drdocdate / rdentrydate / rdmodifydate), each date
     * only when its box is ticked; doc-no range when != 0.
     */
    public List<Map<String, Object>> history(String dateKind, String fromDate, String toDate, String docFrom, String docTo) {
        UserAccount u = hrm.user(SCREEN);
        Map<String, Object> dates = new LinkedHashMap<>();
        LocalDateTime f = toDate(fromDate), t = toDate(toDate);
        String fk = "entry".equals(dateKind) ? "EntryFromDate" : "modify".equals(dateKind) ? "ModifyFromDate" : "FromDate";
        String tk = "entry".equals(dateKind) ? "EntryToDate" : "modify".equals(dateKind) ? "ModifyToDate" : "ToDate";
        if (f != null) dates.put(fk, ts(f));
        if (t != null) dates.put(tk, ts(t));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, DOC_TYPE, hrm.can(u, SCREEN, "CanView AllRecord"), hrm.financialYearId(), dates,
                toInt(docFrom), toInt(docTo))) {
            out.add(map("Id", toInt(r.get("Id")), "VoucherHeadId", toInt(r.get("VoucherHeadId")), "DocNo", toInt(r.get("DocNo")),
                    "DocDate", r.get("DocDate"), "PartyName", str(r.get("PartyName")), "InvoiceNo", str(r.get("InvoiceNo")),
                    "ContractNo", str(r.get("ContractNo")), "LoadingPort", str(r.get("LoadingPort")), "DestinationPort", str(r.get("DestinationPort")),
                    "NoOfContainer", r.get("NoOfContainer"), "NetBillWeight", r.get("NetBillWeight"), "TotalAmount", r.get("TotalAmount"),
                    "EntryUser", str(r.get("EntryUserName")), "EntryDate", r.get("EntryDate"), "ModifyUser", str(r.get("ModifyUserName")),
                    "ModifyDate", r.get("ModifyDate"), "NoOfAttachments", toInt(r.get("NoOfAttachments"))));
        }
        return out;
    }

    // ================================================================== read

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        List<Map<String, Object>> h = id > 0 ? repo.header(id) : new ArrayList<>();
        if (h.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = h.get(0);
        if (toInt(r.get("OrganizationId")) != u.getOrganizationId() || toInt(r.get("CompanyId")) != u.getCompanyId()
                || toInt(r.get("DocTypeId")) != DOC_TYPE) throw invalid("Record not found");
        return r;
    }

    /** FillDetailFromListCommonForReadById: the grid's 18 columns from the stored detail. */
    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id)) {
            out.add(map("Id", toInt(d.get("Id")), "ItemId", toInt(d.get("ItemId")), "ItemCode", str(d.get("ItemCode")), "ItemName", str(d.get("ItemName")),
                    "Qty", toDouble(d.get("Qty")), "UomId", toInt(d.get("UomId")), "Uom", str(d.get("UomName")), "ChargesRate", toDouble(d.get("ChargesRate")),
                    "CurrencyId", toInt(d.get("CurrencyId")), "Currency", str(d.get("CurrencyName")), "FcyAmount", toDouble(d.get("FcyAmount")),
                    "ExchangeRate", toDouble(d.get("ExhangeRate")), "Amount", toDouble(d.get("Amount")),
                    "OtherChargesAmount", toDouble(d.get("OtherChargesAmount")), "TotalLcyAmount", toDouble(d.get("TotalAmount")),
                    "Description", str(d.get("Description")), "DebitAcId", toInt(d.get("DebitAccountId")), "DebitAc", str(d.get("AccountTitle"))));
        }
        return out;
    }

    /** ReadById(ID): ExImClearingAgentBill.GetByID - header, invoice data (cmbeximinvoiceno_Leave) and the detail grid. */
    public Map<String, Object> read(int id) {
        UserAccount u = hrm.user(SCREEN);
        Map<String, Object> h = ownedHeader(u, id);
        return map("Id", toInt(h.get("Id")), "DocNo", toInt(h.get("DocNo")), "DocDate", h.get("DocDate"), "DueDays", toInt(h.get("DueDays")),
                "DueDate", h.get("DueDate"), "RefBillNo", str(h.get("RefBillNo")), "ExportInvoiceId", toInt(h.get("ExportInvoiceId")),
                "SupCustId", toInt(h.get("SupCustId")), "invoice", invoiceInfo(u, toInt(h.get("ExportInvoiceId"))), "rows", detailRows(id));
    }

    /** grdhistory_SelectionChanged -> GetDetailGrdByHeadId. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = hrm.user(SCREEN);
        ownedHeader(u, id);
        return detailRows(id);
    }

    // ================================================================== save

    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = hrm.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        hrm.require(u, SCREEN, recId == 0 ? "Save" : "Update");
        Map<String, Object> stored = recId > 0 ? ownedHeader(u, recId) : null;
        List<Map<String, Object>> rows = b.get("rows") instanceof List ? (List<Map<String, Object>>) b.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw invalid("Detail Record Not Found");                                           // Insert :1087

        int yearId = hrm.financialYearId();
        int docNo = stored != null ? toInt(stored.get("DocNo")) : repo.generateCode(u, DOC_TYPE, yearId);     // txtdocno is read-only
        if (docNo == 0) throw invalid("Doc No field is required");
        int invoiceId = toInt(b.get("invoiceId"));
        if (invoiceId == 0 || !owns(look.exportInvoices(u), "Id", invoiceId)) throw invalid("Invoice No field is required");
        int partyId = toInt(b.get("partyId"));
        if (partyId == 0 || !owns(look.partiesGroup10(u), "Id", partyId)) throw invalid("Party Name field is required");
        /* The Invoice Information group is disabled: its values are the invoice's own (BindInvoiceData). */
        Map<String, Object> inv = invoiceInfo(u, invoiceId);
        boolean found = toBool(inv.get("found"));
        int contractId = found ? toInt(((List<Map<String, Object>>) inv.get("contracts")).get(0).get("LcOrderNoId")) : 0;
        if (contractId == 0) throw invalid("Contract No field is required");
        int loading = found ? toInt(inv.get("LoadingPortId")) : 0;
        if (loading == 0) throw invalid("Loading Place field is required");
        int destination = found ? toInt(inv.get("DestinationPortId")) : 0;
        if (destination == 0) throw invalid("Destination Place field is required");
        if (toDouble(inv.get("NoOfContainers")) == 0.0) throw invalid("No. of Containers must be a non-zero number");
        if (toDouble(inv.get("MTon")) == 0.0) throw invalid("M.Tons must be a non-zero number");

        boolean debitCfg = toBool(look.config(u, "ServicesBillDebitAmountChargetoSelectedAc"));
        Set<Integer> items = ids(look.servicesItems(u));
        Set<Integer> currencies = ids(look.currencies(u));
        Set<Integer> accounts = debitCfg ? ids(look.accountsByTypes(u, 6, 8, 10)) : new HashSet<>();

        LocalDateTime now = LocalDateTime.now();
        LocalDateTime docDate = pickerValue(b.get("docDate"), stored == null ? null : toDate(stored.get("DocDate")));
        int dueDays = toInt(trim(b.get("dueDays")));
        LocalDateTime dueDate = dueDays > 0 ? docDate.plusDays(dueDays) : docDate;                                   // DueDaysCalculate

        LgsAClearingAgentBill obj = new LgsAClearingAgentBill();
        obj.OrganizationId = u.getOrganizationId();
        obj.CompanyId = u.getCompanyId();
        obj.BranchesId = toInt(u.getBranchesId());
        obj.ProjectId = toInt(u.getBranchesId());
        obj.FinancialYearId = yearId;
        obj.DocTypeId = DOC_TYPE;
        obj.EntryDate = now; obj.ModifyDate = now;                                                                  // (BLL Save sets them again)
        obj.EntryUser = u.getId(); obj.ModifyUser = u.getId();
        obj.IsApproved = false;
        obj.Id = recId;
        obj.DocNo = docNo;
        obj.DocDate = docDate;
        obj.DueDays = dueDays;
        obj.DueDate = dueDate;
        obj.RefBillNo = str(b.get("refBillNo"));
        obj.ExportInvoiceId = invoiceId;
        obj.LcContractId = contractId;
        obj.NoOfContainer = toInt(inv.get("NoOfContainers"));
        obj.NetBillWeight = toDouble(inv.get("MTon"));
        obj.LoadingPortId = loading;
        obj.DestinationPortId = destination;
        obj.BookingRate = toDec(inv.get("BookingRate"));
        obj.USDRate = 0.0;
        obj.SupCustId = partyId;
        obj.AttachmentsValues = stored == null ? "" : str(stored.get("AttachmentsValues"));
        obj.CustomAttachmentsValues = stored == null ? "" : str(stored.get("CustomAttachmentsValues"));

        List<LgsAClearingAgentBillDetail> details = new ArrayList<>();
        double total = 0.0;
        for (Map<String, Object> r : rows) {
            /* FormValidationDetail - every row was validated when it was added; a row that could not have been
               added is refused with the same text. */
            int itemId = toInt(r.get("ItemId"));
            if (itemId == 0 || !items.contains(itemId)) throw invalid("Item Name field is required");
            double qty = toDouble(r.get("Qty"));
            if (qty == 0.0) throw invalid("Quantity must be a non-zero number");
            int uomId = toInt(r.get("UomId"));
            if (uomId == 0 || !owns(look.itemUoms(u, itemId), "Id", uomId)) throw invalid("Rate UOM field is required");
            double rate = toDouble(r.get("ChargesRate"));
            if (rate == 0.0) throw invalid("Charges Rate must be a non-zero number");
            int cur = toInt(r.get("CurrencyId"));
            if (cur == 0 || !currencies.contains(cur)) throw invalid("Currency field is required");
            double ex = toDouble(r.get("ExchangeRate"));
            double other = toDouble(r.get("OtherChargesAmount"));
            /* totalamount(): FcyAmount = rate * qty, Lcy = ex * fcy, Total = lcy + other - each shown "#,##0.####"
               and read back by Conversion.ToDouble. */
            double fcy = rate * qty, lcy = ex * fcy, tot = lcy + other;
            double fcyR = round4(fcy), lcyR = round4(lcy), totR = round4(tot);
            if (fcyR == 0.0) throw invalid("Fcy Amount must be a non-zero number");
            if (lcyR == 0.0) throw invalid("Lcy Amount must be a non-zero number");
            if (totR == 0.0) throw invalid("Total Lcy Amount must be a non-zero number");
            String desc = str(r.get("Description"));
            if (desc.trim().isEmpty()) throw invalid("Description field is required");
            int debit = toInt(r.get("DebitAcId"));
            if (debitCfg && (debit == 0 || !accounts.contains(debit))) throw invalid("Debit Account field is required");
            if (!debitCfg && debit != 0 && !ids(look.accountsByTypes(u, 6, 8, 10)).contains(debit)) debit = 0;

            LgsAClearingAgentBillDetail d = new LgsAClearingAgentBillDetail();
            d.Id = recId != 0 ? toInt(r.get("Id")) : 0;
            d.ItemId = itemId;
            d.Qty = qty;
            d.UomId = uomId;
            d.ChargesRate = rate;
            d.ItemRate = rate;
            d.FcyAmount = fcyR;
            d.CurrencyId = cur;
            d.ExhangeRate = ex;
            d.Amount = lcyR;
            d.OtherChargesAmount = other;
            d.TotalAmount = totR;
            d.Description = desc;
            d.DebitAccountId = debit;
            total += d.TotalAmount;
            d.DocDate = now; d.EffectedDateD = now; d.ValidUpTo = now;
            details.add(d);
        }
        obj.TotalAmount = total;
        obj.GrandTotal = total;

        /* BLL Save -> MakeVoucher(obj) */
        LgsAVoucherHead vh = new LgsAVoucherHead();
        List<LgsAVoucherDetail> vds = makeVoucher(u, obj, details, vh);
        int id = repo.save(obj, details, vh, vds);
        return map("success", true, "id", id, "message", recId == 0 ? "Save Successfully" : "Update Successfully");
    }

    /** BLL ExImClearingAgentBill.MakeVoucher (0114 :19-135). */
    private List<LgsAVoucherDetail> makeVoucher(UserAccount u, LgsAClearingAgentBill obj, List<LgsAClearingAgentBillDetail> details, LgsAVoucherHead vh) {
        vh.DocumentTypeId = obj.DocTypeId;
        vh.DocumentTypeSrNo = obj.Id;
        vh.RefDocNoId = obj.Id;
        vh.VoucherCode = obj.DocNo;
        vh.VoucherDate = obj.DocDate;
        vh.Remarks = "";
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = obj.BranchesId;
        vh.ProjectId = obj.ProjectId;
        vh.ManualBillNo = str(obj.RefBillNo);
        vh.DueDate = obj.DueDate;
        vh.OrganizationId = obj.OrganizationId;
        vh.CompanyId = obj.CompanyId;
        vh.FinancialYearId = obj.FinancialYearId;
        vh.EntryUser = obj.EntryUser;
        vh.EntryDate = LocalDateTime.now();
        vh.ModifyDate = LocalDateTime.now();
        vh.ModifyUser = obj.ModifyUser;
        List<Map<String, Object>> itemList = repo.itemGlAccounts(u);
        List<Map<String, Object>> parties = repo.partyGlAccounts(u);
        Map<String, Object> party = null;
        for (Map<String, Object> p : parties) if (toInt(p.get("Id")) == obj.SupCustId) { party = p; break; }
        if (party == null) throw invalid("PartyName GlAccountId nof found");
        vh.RefAccountId = toInt(party.get("GlAccountId"));
        Map<Integer, Integer> itemGl = new HashMap<>();
        for (Map<String, Object> i : itemList) itemGl.putIfAbsent(toInt(i.get("Id")), toInt(i.get("PurchaseGLAC")));
        List<LgsAVoucherDetail> out = new ArrayList<>();
        double num = 0.0;
        for (LgsAClearingAgentBillDetail item : details) {
            int account;
            if (item.DebitAccountId > 0) account = item.DebitAccountId;
            else if (!itemList.isEmpty()) {
                if (!itemGl.containsKey(item.ItemId)) continue;           // an item not in the list is left out, as the BLL does
                account = itemGl.get(item.ItemId);
            } else throw invalid("Item Record Not found");
            out.add(vd(account, vh.RefAccountId, item, true));
            out.add(vd(vh.RefAccountId, account, item, false));
            num += item.TotalAmount;
        }
        vh.VoucherAmount = num;
        vh.BillAmount = num;
        return out;
    }

    private static LgsAVoucherDetail vd(int account, int against, LgsAClearingAgentBillDetail item, boolean debit) {
        LgsAVoucherDetail d = new LgsAVoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = str(item.Description);
        if (debit) d.DebitAmount = item.TotalAmount; else d.CreditAmount = item.TotalAmount;
        d.ItemId = item.ItemId;
        d.DMultiCurrencyId = item.CurrencyId;
        d.DExchangeCurrencyRate = item.ExhangeRate;
        d.ItemRate = item.ItemRate;
        d.ItemAmount = item.TotalAmount;
        return d;
    }

    // ================================================================== delete / prints

    /** btnDelete_Click: RecId > 0 else "No record found to Delete"; DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> delete(int id) {
        UserAccount u = hrm.user(SCREEN);
        hrm.require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        ownedHeader(u, id);
        repo.delete(u.getId(), id);
        return map("success", true, "message", "Delete Record Successfully");
    }

    /** Slip420(ReportId) -> CommonServices.ServicesBillSlip540: Print right and a bill of this company. */
    public Map<String, Object> slipCheck(int id) {
        UserAccount u = hrm.user(SCREEN);
        hrm.require(u, SCREEN, "Print");
        if (id <= 0) throw invalid("No Record Found For Display");
        ownedHeader(u, id);
        return map("id", id);
    }

    /** btnVoucher_Click / the history Voucher button: CommonServices.VoucherHeadIdGet(RecId, 155) -> VoucherReport_118. */
    public Map<String, Object> voucherId(int id) {
        UserAccount u = hrm.user(SCREEN);
        hrm.require(u, SCREEN, "Print");
        if (id <= 0) throw invalid("Record Not Found For Display");
        ownedHeader(u, id);
        int vh = look.voucherHeadId(u, DOC_TYPE, id);
        if (vh == 0) throw invalid("No Record Found For Display");
        return map("voucherHeadId", vh, "documentTypeId", DOC_TYPE);
    }

    // ================================================================== helpers

    private static Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(r.get("Id")));
        return s;
    }

    /** double.ToString("#,##0.####") read back by Conversion.ToDouble: 4 places, MidpointRounding.AwayFromZero. */
    static double round4(double v) { return BigDecimal.valueOf(v).setScale(4, RoundingMode.HALF_UP).doubleValue(); }

    /** "#,##0.###" / "#,##0.####" text. */
    static String fmt(double v, int places, boolean group) {
        BigDecimal x = BigDecimal.valueOf(v).setScale(places, RoundingMode.HALF_UP).stripTrailingZeros();
        if (x.scale() < 0) x = x.setScale(0);
        String s = x.toPlainString();
        if (!group) return s;
        boolean neg = s.startsWith("-");
        if (neg) s = s.substring(1);
        String ip = s.contains(".") ? s.substring(0, s.indexOf('.')) : s, fp = s.contains(".") ? s.substring(s.indexOf('.')) : "";
        StringBuilder g = new StringBuilder();
        for (int i = 0; i < ip.length(); i++) { if (i > 0 && (ip.length() - i) % 3 == 0) g.append(','); g.append(ip.charAt(i)); }
        return (neg ? "-" : "") + g + fp;
    }

    /** HrmSupport.toDate plus a JSON epoch-millis number (a java.sql.Timestamp the page got back unchanged). */
    static LocalDateTime anyDate(Object v) {
        if (v instanceof Number) return LocalDateTime.ofInstant(java.time.Instant.ofEpochMilli(((Number) v).longValue()), java.time.ZoneId.systemDefault());
        if (v instanceof String && ((String) v).trim().matches("-?\\d{9,}")) return anyDate(Long.parseLong(((String) v).trim()));
        return toDate(v);
    }

    /**
     * DateTimePicker.Value: the picked day with the time of day the picker holds - the current time on a new
     * document (reset sets Value = DateTime.Now), the stored time on a document opened for edit.
     */
    static LocalDateTime pickerValue(Object picked, LocalDateTime stored) {
        LocalDateTime d = anyDate(picked);
        if (d != null) d = d.toLocalDate().atStartOfDay();
        LocalTime time = stored != null ? stored.toLocalTime() : LocalTime.now().withNano(0);
        if (d == null) return stored != null ? stored : LocalDateTime.now().withNano(0);
        return d.toLocalDate().atTime(time);
    }
}
