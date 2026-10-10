package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.pcc.SalePccInvoiceCalc.Fmt;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Detail;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Head;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Invoice;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Journal;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 553 "SaleInvoiceReturnConcrete" = Architecture.WinApp.pcc.Sale.SaleInvoiceReturnConcrete (Sale Pcc, module 85, document type 1862).
 *
 * Desktop map (SaleInvoiceReturnConcrete.cs, 4944 lines; method : line):
 *   Load :293, DocumentNo :463, Project :485, braches :501, suppliercustomer :517, VisitedByFill :571, PaymentTerms :609, DeliveryTerm :638,
 *   RefSalesManFill :655, CommissionTypeFill :693, CommissionUOMFill :710, CommissionDebitAccountFill :729, bindWareHouse :764, item :793,
 *   bindvarientunit :847, JobLotBind :889, DiscountTypefill :918, BinCity :946, VoucherHeadIdGet :975, dtForGridComboFill :986, ConfigurationDefault :998,
 *   comItem_Leave :1051, AvailableStockGetByItem :1075, CurrencyFill :1188, MultiCurrencyFeature :1221, cmbCurrency_Leave :1254, txtExchangeRate_TextChanged :1290,
 *   grdGLedger_* :1338-1507, btnAdd_Click :1508, grd_DoubleClick :1543, btnUpdateDetail_Click :1585, grdSettings :1663, grd_CellUpdated :1802,
 *   grd_ColumnButtonClick :1854, FormValidation :1950, FormValidationDetail :2038, Reset :2126, ResetDetail :2192, btnRefresh_Click :2225, Insert :2262,
 *   ReadById :2619, btnDelete_Click :2714, btnUpdate_Click :2742, HistoryComboBind :2757, FillHistory :2914, grdHistory_* :3067-3130, GetDetailGrdByHeadId :3130,
 *   btnPrint_Click :3297, btnSlip_Click :3308, CalculateDetailAmount :3374, CalculateDiscountAndTotalAmount :3413, BillAmount :3485, CalculateRowNetAmount :3613,
 *   NetRateCalculation :3738, TotalCommissionAmount :3766, KeyDown :4059, grdGLedger_KeyDown :4460, UpdateDiscountTypeAndRateInGrid :4652,
 *   CalculateWithManualOrderAmountTotal :4752, PropotionateDiscountByManualOrderAmountInGrid :4808.
 * BLL 0347 pcc.InvSaleInvoice.Save (Id 0: ModifyUser 0 + pcc.USP_InvSaleInvoice_Insert; else EntryUser 0 + _Update) + SaleInvoiceFinancial (0358) +
 * DAL 0285 SetData = SalePccInvoiceFinancial + SalePccInvoicePersist, one transaction.
 *
 * The page keeps the amounts live (the same formulas as the desktop text boxes); Save re-runs CalculateDetailAmount / BillAmount on the posted
 * entry values (SalePccInvoiceCalc) so the saved amounts never depend on a browser-side total. Row discount amounts come from the entry panel,
 * "Update Same Discount" or the manual-total proportioning exactly as the desktop persists the grid cells; the server only bounds them.
 */
@Service
public class SalePccInvoiceReturnService {
    public static final String SCREEN = "SaleInvoiceReturnConcrete";
    public static final int SCREEN_ID = 553;
    public static final int DOC_TYPE = 1862;
    private static final String GET = "[pcc].[USP_InvSaleInvoice_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SalePccLookups lk;
    private final SalePccAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SalePccInvoiceFinancial financial;
    private final SalePccInvoicePersist persist;

    public SalePccInvoiceReturnService(SaleEngrSupport sup, SalePccLookups lk, SalePccAttachments attachments, SaleInvoiceRepository repo,
                                       SalePccInvoiceFinancial financial, SalePccInvoicePersist persist) {
        this.sup = sup; this.lk = lk; this.attachments = attachments; this.repo = repo; this.financial = financial; this.persist = persist;
    }

    // ------------------------------------------------------------------ configuration

    private Fmt fmt() {
        return new Fmt(toInt(sup.config("Default NoofDecimal Points For Amount")), toInt(sup.config("Default NoofDecimal Points For Rate")),
                toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount")));
    }

    private static Map<String, Object> fmtMap(Fmt f) {
        return row("amountRound", f.amtRound, "amount", f.amtPlaces, "rateRound", f.rateRound, "rate", f.ratePlaces, "fcy", f.fcyPlaces);
    }

    // ------------------------------------------------------------------ Load / Refresh

    public Map<String, Object> initial() {
        lk.requireView(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                                       // DocumentNo
        m.putAll(lists(LocalDate.now().toString()));
        m.put("settings", row("itemByCode", sup.configBool("ItemSearchByCode"), "partyByCode", sup.configBool("SupplierCustomerDefaultFilterByPartyCode"),
                "multiCurrency", sup.erpFeature(6), "subsidiary", sup.erpFeature(4)));  // ItemSearchWithCode, PartySearchWithCode, HasMultiCurrencyFeature, SubsidiaryAccountAllownOnVouchers
        m.put("defaults", defaults());
        m.put("fmt", fmtMap(fmt()));
        m.put("history", historyCombos());
        m.put("dateTypes", SalePccLookups.dateTypes());
        m.put("fyStart", lk.fyStart());
        return m;
    }

    /** btnRefresh_Click: CurrencyFill, suppliercustomer, VisitedByFill, PaymentTerms, RefSalesManFill, CommissionDebitAccountFill, bindWareHouse, item, JobLotBind, BinCity, dtForGridComboFill, ConfigurationDefault. */
    public Map<String, Object> lists(String docDate) {
        UserAccount u = sup.user();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("currencies", mapRows(repo.currencies(u), "Id", "CurrencyCode"));                       // CurrencyFill (MultiCurrency.GetAll)
        m.put("customers", lk.customers());                                                          // suppliercustomer
        m.put("visitedBy", refParties(2));                                                           // VisitedByFill
        m.put("terms", mapRows(repo.paymentTerms(u), "Id", "TermsDescription"));                      // PaymentTerms (GetDueTermServiceBind)
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Factory Loading"), row("Id", 2, "DeliveryTerm", "Delivery")));      // DeliveryTerm
        m.put("refSalesMan", refParties(3));                                                         // RefSalesManFill
        m.put("commTypes", List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent")));                  // CommissionTypeFill
        m.put("commUoms", List.of(row("Id", 1, "UOM", "40"), row("Id", 2, "UOM", "50"), row("Id", 3, "UOM", "60"), row("Id", 4, "UOM", "100")));   // CommissionUOMFill
        m.put("commDebit", mapRows(repo.accountTitlesByTypes(u, "10,11,12,20,21", null), "Id", "AccountTitle"));                      // CommissionDebitAccountFill
        m.put("warehouses", lk.warehouses());                                                        // bindWareHouse
        m.put("items", items(docDate));                                                              // item
        m.put("jobLots", lk.jobLots());                                                              // JobLotBind
        m.put("discTypes", List.of(row("Id", 1, "DiscountType", "Flat"), row("Id", 2, "DiscountType", "Percent")));                       // DiscountTypefill
        m.put("cities", lk.cities());                                                                // BinCity
        m.put("projects", projects());                                                               // Project
        m.put("branches", branches());                                                               // braches
        return m;
    }

    private static List<Map<String, Object>> mapRows(List<Map<String, Object>> src, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String c : cols) o.put(c, ci(r, c));
            out.add(o);
        }
        return out;
    }

    /** ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId (2 = Visited By, 3 = Ref Sales Man). */
    private List<Map<String, Object>> refParties(int typeId) {
        return mapRows(sup.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ReferencePartyTypeId", typeId,
                "Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId"), "Id", "ReferencePartyName");
    }

    /** item(): ItemPricingSchedule.GetItemWithRatesForItemPricingSchedule (EffectedDate = DocDate). dtitem {Id, ItemName, ItemCode, ItemWeight, ItemRate}; ScheduleId is never filled. */
    public List<Map<String, Object>> items(String docDate) {
        LocalDate d = SalePccLookups.parseDate(docDate);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "[pcc].[USP_ItemPricingSchedule_GetAllMethod]", orderedParams(
                "OrganizationId", sup.org(), "CompanyId", sup.company(), "EffectiveDate", d == null ? LocalDate.now() : d, "Activity", "GetItemWithRatesForItemPricingSchedule")))
            out.add(row("Id", toInt(ci(r, "ItemId")), "ItemName", str(ci(r, "ItemName")), "ItemCode", str(ci(r, "ItemCodeNew")),
                    "ItemWeight", toDouble(ci(r, "WeightKgs")), "ItemRate", toDouble(ci(r, "PreviousRate")), "ScheduleId", 0));
        return out;
    }

    private static Map<String, Object> orderedParams(Object... kv) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) p.put(String.valueOf(kv[i]), kv[i + 1]);
        return p;
    }

    /** CommonServices.BrancheServiceBind. */
    public List<Map<String, Object>> branches() {
        return mapRows(sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"), "Id", "BranchName");
    }

    /** CommonServices.ProjectServiceBind. */
    public List<Map<String, Object>> projects() {
        return mapRows(sup.rows("Sp_Projects_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"), "Id", "ProjectName");
    }

    /** ConfigurationDefault: Warehouse, City Area, Job/Lot, Base Currency, BaseCurrencyRate (the ConfigKey of the configuration allocation). */
    public Map<String, Object> defaults() {
        return row("warehouseId", toInt(sup.config("Warehouse")), "cityId", toInt(sup.config("City Area")), "jobLotId", toInt(sup.config("Job/Lot")),
                "baseCurrency", toInt(sup.config("Base Currency")), "baseRate", toDecimalOrZero(sup.config("BaseCurrencyRate")));
    }

    private static BigDecimal toDecimalOrZero(String s) {
        try { return s == null || s.trim().isEmpty() ? BigDecimal.ZERO : new BigDecimal(s.trim().replace(",", "")); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    /** DocumentNo: InvSaleInvoice.GenerateInvSaleInvoiceCode (DocumentTypeId 1862, branch + financial year). */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", sup.fy(), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    public List<Map<String, Object>> varients(int itemId) { return lk.varients(itemId); }

    /** AvailableStockGetByItem: QtyInHand of GetStockInHandAndAvgRateFromEvaluationConcrete (0 without an item). */
    public Map<String, Object> stock(int itemId, String docDate, int warehouseId, int jobLotId, int varientId) {
        LocalDate d = SalePccLookups.parseDate(docDate);
        double q = itemId > 0 ? lk.stockInHand(itemId, d == null ? LocalDate.now() : d, warehouseId, jobLotId, varientId) : 0.0;
        return row("qty", q);
    }

    /** cmbCurrency_Leave: the last exchange rate used on a 1862 voucher for the currency (0 when none). */
    public Map<String, Object> lastRate(int currencyId) {
        Double d = currencyId > 0 ? repo.lastExchangeRate(sup.user(), currencyId, DOC_TYPE) : null;
        return row("rate", d == null ? 0.0 : d);
    }

    /** grdGLedger_KeyDown F1: the account chooser (feature 4: GetVendorsAndCustomers() -> Id / CompanyName / GlAccountId; otherwise dtAccountlst = COAAllocation search). */
    public Map<String, Object> accounts() {
        UserAccount u = sup.user();
        boolean sub = sup.erpFeature(4);
        List<Map<String, Object>> out = new ArrayList<>();
        if (sub) {
            for (Map<String, Object> r : repo.vendorsAndCustomers(u, 0))
                out.add(row("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "CompanyName")), "GlAccountId", toInt(ci(r, "GlAccountId"))));
        } else {
            Map<String, Object> p = orderedParams("OrganizationId", sup.org(), "CompanyId", sup.company());
            if (u.getId() != 0) p.put("UserId", u.getId());
            p.put("Activity", "COAAllocationSearch");
            for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "Sp_COAAllocation_GetAllMethod", p))
                out.add(row("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "AccountTitle")), "GlAccountId", toInt(ci(r, "Id"))));
        }
        return row("subsidiary", sub, "rows", out);
    }

    // ------------------------------------------------------------------ History

    /** HistoryComboBind: InvSaleInvoice.DropDownFillFromInvSaleInvoice (DocumentTypeIds "1862"), split by Activity. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), agents = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[pcc].[USP_DropDownFillFromInvSaleInvoice]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", "1862")) {
            String a = str(ci(r, "Activity"));
            if ("Customer".equals(a)) cust.add(row("Id", toInt(ci(r, "Id")), "PartyName", str(ci(r, "ReferenceName")), "PartyCode", str(ci(r, "OtherDescription"))));
            else if ("CommissionAgent".equals(a)) agents.add(row("Id", toInt(ci(r, "Id")), "CommissionAgent", str(ci(r, "ReferenceName"))));
        }
        return row("customers", cust, "agents", agents);
    }

    /** FillHistory: InvSaleInvoice.FormHistory (zero filters are not sent; EntryUserId only without the CanView AllRecord right). */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromNo, int toNo, int customerId, int agentId) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch());
        p.put("FinancialYearId", sup.fy()); p.put("DocumentTypeId", DOC_TYPE);
        LocalDate f = SalePccLookups.parseDate(fromDate), t = SalePccLookups.parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        if (fromNo != 0) p.put("DocNoFrom", (double) fromNo);
        if (toNo != 0) p.put("DocNoTo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (agentId != 0) p.put("CommissionAgentId", agentId);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUserId", sup.userId());
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(sup.jdbc(), GET, p);
    }

    // ------------------------------------------------------------------ ReadById

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(GET, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(ci(r.get(0), "OrganizationId")) != u.getOrganizationId() || toInt(ci(r.get(0), "CompanyId")) != u.getCompanyId()
                || toInt(ci(r.get(0), "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice return not found in this company");
        return r.get(0);
    }

    private List<Map<String, Object>> details(int id) { return sup.rows(GET, "Id", id, "Activity", "ReadByHeaderId"); }

    /** InvSaleInvoice.GetByID (header, detail list, journal list) + DMSAttachments.GetByID + VoucherHeadIdGet. */
    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("journals", sup.rows(GET, "Id", id, "Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
        out.put("attachments", attachments.list(SCREEN, id));
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert)

    public static class Row {
        public int id, warehouseId, itemId, scheduleId, attributeVarientId, jobLotId, gpNo, cityId, discountTypeId;
        public BigDecimal varientUnit = BigDecimal.ZERO, qty = BigDecimal.ZERO, itemWeight = BigDecimal.ZERO, netWeight = BigDecimal.ZERO, itemPrice = BigDecimal.ZERO,
                addLessRate = BigDecimal.ZERO, rate = BigDecimal.ZERO, discRate = BigDecimal.ZERO, discAmount = BigDecimal.ZERO;
        public String gpDate, vehicleNo;
        public boolean isFoc;
    }

    public static class Jv {
        public int accountId, glAccountId;
        public String remarks;
        public BigDecimal percentage = BigDecimal.ZERO, qty = BigDecimal.ZERO, rate = BigDecimal.ZERO, debit = BigDecimal.ZERO, credit = BigDecimal.ZERO;
    }

    public static class Request {
        public int id;
        public String docDate, docNo, referenceNo, manualBillNo, remarks, dueDays, refPartyName, refPartyAddress, refPartyCellNo;
        public int supplierCustomerId, visitedById, paymentTermId, deliveryTermId, commissionAgentId, refSalesManId, commissionDebitAccountId, currencyId;
        public String commissionType, commissionUom, commissionRemarks;
        public BigDecimal commissionRate = BigDecimal.ZERO, commissionAmount = BigDecimal.ZERO, exchangeRate = BigDecimal.ZERO;
        public List<Row> rows = new ArrayList<>();
        public List<Row> removed = new ArrayList<>();
        public List<Jv> journals = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static BigDecimal nz(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        Map<String, Boolean> rt = sup.rights(SCREEN);
        lk.need(rt, r.id > 0 ? "update" : "save", r.id > 0 ? "Update" : "Save");
        Fmt f = fmt();
        boolean multi = sup.erpFeature(6);                                                         // HasMultiCurrencyFeature
        List<Row> rows = r.rows == null ? List.of() : r.rows;
        List<Jv> jvs = r.journals == null ? List.of() : r.journals;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");           // Insert: grd.GetRows().Count() == 0

        boolean approved = false;
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            approved = toBool(ci(header(r.id), "IsApproved"));
            if (approved) throw new IllegalArgumentException("Record Not Update beacause Record has approved");   // btnUpdate_Click
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(ci(d, "Id")));
        }

        // BillAmount() over the posted entry values
        BigDecimal exch = nz(r.exchangeRate);
        List<SalePccInvoiceCalc.Line> lines = new ArrayList<>();
        for (Row w : rows) {
            SalePccInvoiceCalc.Line l = new SalePccInvoiceCalc.Line();
            l.qty = nz(w.qty); l.unit = nz(w.varientUnit); l.weight = nz(w.itemWeight); l.netWeight = nz(w.netWeight);
            l.price = nz(w.itemPrice); l.addLess = nz(w.addLessRate); l.rate = nz(w.rate);
            l.discTypeId = w.discountTypeId; l.discRate = nz(w.discRate); l.discAmount = nz(w.discAmount); l.foc = w.isFoc;
            SalePccInvoiceCalc.row(l, f, exch);
            lines.add(l);
        }
        List<SalePccInvoiceCalc.Jv> jcalc = new ArrayList<>();
        for (Jv j : jvs) {
            SalePccInvoiceCalc.Jv c = new SalePccInvoiceCalc.Jv();
            c.accountId = j.accountId; c.glAccountId = j.glAccountId; c.debit = nz(j.debit); c.credit = nz(j.credit);
            jcalc.add(c);
        }
        BigDecimal commAmount = SalePccInvoiceCalc.amt(nz(r.commissionAmount), f);
        SalePccInvoiceCalc.Totals t = SalePccInvoiceCalc.bill(lines, jcalc, commAmount, r.supplierCustomerId, r.commissionAgentId, exch, f);

        // FormValidation()
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new IllegalArgumentException("DocNo Field is Required");
        if (r.supplierCustomerId == 0) throw new IllegalArgumentException("CustomerName Field is Required");
        String termText = "";
        for (Map<String, Object> p : repo.paymentTerms(u)) if (toInt(ci(p, "Id")) == r.paymentTermId) termText = str(ci(p, "TermsDescription"));
        if (r.paymentTermId == 0) throw new IllegalArgumentException("Payment Term Field is Required");
        if (r.deliveryTermId != 1 && r.deliveryTermId != 2) throw new IllegalArgumentException("Delivery Term Field is Required");
        if (text(r.refPartyName).isEmpty()) throw new IllegalArgumentException("Ref Party Name Field is Required");
        if (text(r.refPartyAddress).isEmpty()) throw new IllegalArgumentException("Ref Party Address Field is Required");
        if (text(r.refPartyCellNo).isEmpty()) throw new IllegalArgumentException("Ref Party CellNo Field is Required");
        String exchText = exch.signum() == 0 ? "0" : exch.stripTrailingZeros().toPlainString();
        if (multi) {
            if (r.currencyId == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (exchText.equals("0")) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (t.fcy.signum() == 0 && f.fcyPlaces == 0) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");   // txtFcyAmount.Text == "0"
        } else {
            if (r.currencyId == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exchText.equals("0")) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        if ("Credit".equals(termText) && blank(r.dueDays)) throw new IllegalArgumentException("Due Days Field is Required");

        // commission checks
        if (commAmount.signum() > 0) {
            if (r.commissionAgentId == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
            if (r.refSalesManId == 0) throw new IllegalArgumentException("Please Select Ref Sales Man First");
            if (r.commissionDebitAccountId == 0) throw new IllegalArgumentException("CommissionCreditAccount Account field Required");
            List<Map<String, Object>> info = sup.rows("Sp_SupplierCustomer_GetAllMethod", "GlAccountId", r.commissionDebitAccountId, "Id", 0, "Activity", "GetSupplierCustomerInfoByGLAccountId");
            if (!info.isEmpty() && !info.get(0).isEmpty() && toInt(info.get(0).values().iterator().next()) == r.commissionAgentId)
                throw new IllegalArgumentException("CommissionCreditAccount can not be same as Commission agent Please check");
        }
        for (int i = 0; i < jvs.size(); i++) {
            Jv j = jvs.get(i);
            if ((nz(j.credit).signum() > 0 || nz(j.debit).signum() > 0) && j.accountId == 0 && j.glAccountId == 0)
                throw new IllegalArgumentException("Please Select an Account Party Addless Grid in Row no: " + (i + 1));
        }
        if (t.itemAmount.signum() == 0 || t.itemNet.signum() == 0)
            throw new IllegalArgumentException("ItemAmount and ItemNetAmount can not equal to zero Please check");

        LocalDate docDate = SalePccLookups.parseDate(r.docDate);
        if (docDate == null) throw new IllegalArgumentException("DocDate Field is Required");
        int dueDays = toInt(text(r.dueDays));

        // the invoice (Insert)
        LocalDateTime now = LocalDateTime.now();
        Invoice inv = new Invoice();
        Head h = inv.h;
        h.OrganizationId = u.getOrganizationId(); h.CompanyId = u.getCompanyId(); h.BranchesId = u.getBranchesId(); h.ProjectsId = u.getBranchesId();
        h.FinancialYearId = sup.fy(); h.DocumentTypeId = DOC_TYPE;
        h.EntryDate = now; h.ModifyDate = now; h.ApprovedDate = now; h.DeliveryStartDate = now; h.ExpiryDate = now;
        h.EntryUserId = u.getId(); h.ModifyUserId = u.getId(); h.IsApproved = false;
        h.DocDate = docDate.atStartOfDay(); h.DocNo = toInt(no);
        h.ManualBillNo = text(r.manualBillNo); h.RefrenenceNo = text(r.referenceNo);
        h.SupplierCustomerId = r.supplierCustomerId; h.VisitedById = r.visitedById; h.RemarksHeader = text(r.remarks);
        h.PaymentTermsId = r.paymentTermId;
        h.DueDays = dueDays;
        h.DueDate = (blank(r.dueDays) ? docDate : docDate.plusDays(dueDays)).atStartOfDay();       // DueDateGenerate
        h.DeliveryTerm = r.deliveryTermId == 1 ? "Factory Loading" : "Delivery";
        h.ReferencPartyName = text(r.refPartyName); h.ReferencPartyAddress = text(r.refPartyAddress); h.ReferencPartyCellNo = text(r.refPartyCellNo);
        h.CommissionAgentId = r.commissionAgentId; h.RefSalesManId = r.refSalesManId;
        h.CommissionType = text(r.commissionType);
        h.CommissionRate = nz(r.commissionRate);
        h.CommissionUom = toDecimalOrZero(r.commissionUom);
        h.CommissionAmount = commAmount;
        h.CommissionRemarks = text(r.commissionRemarks);
        h.InvoiceQty = t.qty; h.InvoiceWeight = t.weight;
        h.ExchangeRate = exch; h.CurrencyId = r.currencyId; h.FcyAmount = t.fcy;
        h.CommissionDebitAcId = r.commissionDebitAccountId;
        h.ItemAmountHeader = t.itemAmount; h.DiscountAmountHeader = t.discount; h.ItemNetAmountHeader = t.itemNet;
        h.BillAmountWithoutDiscount = t.billWithoutCommission; h.BillAmount = t.bill;

        for (int i = 0; i < rows.size(); i++) {
            Row w = rows.get(i);
            SalePccInvoiceCalc.Line l = lines.get(i);
            int n = i + 1;
            if (w.id > 0 && !savedIds.contains(w.id)) throw new IllegalArgumentException("Invalid detail row for this sale invoice return");
            Detail d = new Detail();
            d.Id = w.id;
            d.ActionTypeId = w.id > 0 ? 2 : 1;
            d.LineId = n;
            d.InvGdnDetailId = 0; d.InvGdnId = 0; d.SaleOrderId = 0; d.SaleOrderDetailId = 0;
            if (w.warehouseId == 0) throw new IllegalArgumentException("Warehouse Required in Detail Grid And row No: " + n);
            d.WarehouseId = w.warehouseId;
            if (w.itemId == 0) throw new IllegalArgumentException("Item Required in Detail Grid And row No: " + n);
            d.ItemId = w.itemId;
            if (w.attributeVarientId == 0) throw new IllegalArgumentException("AttributeVarient Required in Detail Grid And row No: " + n);
            d.ItemAttributeVarientId = w.attributeVarientId;
            d.ScheduleId = w.scheduleId;
            if (l.unit.signum() == 0) throw new IllegalArgumentException("VarientUnit Required in Detail Grid And row No: " + n);
            d.VarientEquivalent = l.unit;
            if (w.jobLotId == 0) throw new IllegalArgumentException("JobLot Required in Detail Grid And row No: " + n);
            d.JobLotId = w.jobLotId;
            if (l.qty.signum() == 0) throw new IllegalArgumentException("ItemQty Required in Detail Grid And row No: " + n);
            d.ItemQty = l.qty;
            if (l.weight.signum() == 0) throw new IllegalArgumentException("ItemWeight Required in Detail Grid And row No: " + n);
            d.ItemWeight = l.weight;
            if (l.netWeight.signum() == 0) throw new IllegalArgumentException("NetWeight Required in Detail Grid And row No: " + n);
            d.ItemNetWeight = l.netWeight;
            if (l.price.signum() == 0) throw new IllegalArgumentException("ItemPrice Required in Detail Grid And row No: " + n);
            d.ItemRateWithOutAddLess = l.price;
            if (l.rate.signum() <= 0) throw new IllegalArgumentException("ItemRate Required in Detail Grid And row No: " + n + "\nItemRate Can not Less than Equal to 0");
            d.ItemRate = l.rate;
            d.RateAddLess = l.addLess;
            d.RateCut = d.RateAddLess;
            if (l.itemAmount.signum() == 0) throw new IllegalArgumentException("ItemAmount Required in Detail Grid And row No: " + n);
            d.ItemAmount = SalePccInvoiceCalc.amt(l.itemAmount, f);
            if ((l.discRate.signum() > 0 || l.discAmount.signum() > 0) && l.discTypeId == 0)
                throw new IllegalArgumentException("DiscountType Required in Detail Grid And row No: " + n);
            d.ItemDiscountTypeId = l.discTypeId;
            if ((l.discRate.signum() == 0 || l.discAmount.signum() == 0) && l.discTypeId > 0)
                throw new IllegalArgumentException("Discount Required in Detail Grid And row No: " + n);
            if (l.discTypeId == 2 && l.discRate.compareTo(BigDecimal.valueOf(99)) > 0)
                throw new IllegalArgumentException("Discount Percentage Cannot greater than 99");
            if (l.discTypeId > 0 && l.discAmount.compareTo(l.itemAmount) >= 0)
                throw new IllegalArgumentException("Discount Amount Cannot be greater than Item Amount in Detail Grid And row No: " + n);
            d.ItemDiscountRate = l.discRate;
            d.ItemDiscountAmount = l.discTypeId == 0 ? BigDecimal.ZERO : l.discAmount;
            if (l.withDisc.signum() == 0) throw new IllegalArgumentException("ItemAmountWithDisc Required in Detail Grid And row No: " + n);
            d.ItemAmountWithDisc = SalePccInvoiceCalc.amt(l.withDisc, f);
            d.ExchangeRate = exch; d.CurrencyId = r.currencyId;
            d.FcyAmount = l.fcy;
            LocalDate gp = SalePccLookups.parseDate(w.gpDate);
            d.GpDate = (gp == null ? LocalDate.of(1900, 1, 1) : gp).atStartOfDay();               // Conversion.ToDateTime of a blank = 1900-01-01
            d.GpNo = w.gpNo;
            d.VehicleNo = w.vehicleNo == null ? "" : w.vehicleNo;
            d.CityId = w.cityId;
            d.CommissionAmount = l.commission;
            d.ItemNetAmount = l.netAmount;
            d.RemarksDetail = "";
            d.IsFOC = w.isFoc;
            inv.details.add(d);
        }
        if (inv.details.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows of detail ");

        // rows removed from a saved invoice (lstRemoveDetailRecord, ActionTypeId 3) - re-read from the stored detail so the numbers cannot be forged
        List<Row> removed = r.removed == null ? List.of() : r.removed;
        if (!removed.isEmpty()) {
            Map<Integer, Map<String, Object>> stored = new HashMap<>();
            for (Map<String, Object> d : details(r.id)) stored.put(toInt(ci(d, "Id")), d);
            for (Row w : removed) {
                Map<String, Object> s = stored.get(w.id);
                if (w.id <= 0 || s == null) throw new IllegalArgumentException("Deleted detail row does not belong to this sale invoice return");
                Detail d = new Detail();
                d.Id = w.id;
                d.InvGdnDetailId = 0; d.InvGdnId = 0; d.SaleOrderId = 0; d.SaleOrderDetailId = 0;
                d.WarehouseId = toInt(ci(s, "WarehouseId")); d.ItemId = toInt(ci(s, "ItemId")); d.ScheduleId = toInt(ci(s, "ScheduleId"));
                d.ItemAttributeVarientId = toInt(ci(s, "ItemAttributeVarientId")); d.VarientEquivalent = toDecimal(ci(s, "VarientEquivalent"));
                d.JobLotId = toInt(ci(s, "JobLotId")); d.ItemQty = toDecimal(ci(s, "ItemQty")); d.ItemWeight = toDecimal(ci(s, "ItemWeight"));
                d.ItemNetWeight = toDecimal(ci(s, "ItemNetWeight")); d.ItemRateWithOutAddLess = toDecimal(ci(s, "ItemRateWithOutAddLess"));
                d.RateAddLess = toDecimal(ci(s, "RateAddLess")); d.ItemRate = toDecimal(ci(s, "ItemRate")); d.RateCut = d.RateAddLess;
                d.ItemAmount = toDecimal(ci(s, "ItemAmount")); d.ItemDiscountTypeId = toInt(ci(s, "ItemDiscountTypeId"));
                d.ItemDiscountRate = toDecimal(ci(s, "ItemDiscountRate")); d.ItemDiscountAmount = toDecimal(ci(s, "ItemDiscountAmount"));
                d.ItemAmountWithDisc = toDecimal(ci(s, "ItemAmountWithDisc")); d.ExchangeRate = exch; d.CurrencyId = r.currencyId;
                d.FcyAmount = toDecimal(ci(s, "FcyAmount"));
                LocalDate gp = SalePccLookups.parseDate(str(ci(s, "GpDate")));
                d.GpDate = (gp == null ? LocalDate.of(1900, 1, 1) : gp).atStartOfDay();
                d.GpNo = toInt(ci(s, "GpNo")); d.VehicleNo = str(ci(s, "VehicleNo")); d.CityId = toInt(ci(s, "CityId"));
                d.ExpenseAmount = toDecimal(ci(s, "ExpenseAmount")); d.CommissionAmount = toDecimal(ci(s, "CommissionAmount"));
                d.FreightAmount = toDecimal(ci(s, "FreightAmount")); d.JournalAmount = toDecimal(ci(s, "JournalAmount")); d.WagesAmount = toDecimal(ci(s, "WagesAmount"));
                d.ItemNetAmount = toDecimal(ci(s, "ItemNetAmount"));
                d.RemarksDetail = "";
                d.ActionTypeId = 3;
                inv.details.add(d);
            }
        }

        // Party Add / Less rows
        for (int i = 0; i < jvs.size(); i++) {
            Jv j = jvs.get(i);
            int n = i + 1;
            boolean used = j.accountId != 0 || j.glAccountId != 0 || nz(j.credit).signum() != 0 || nz(j.debit).signum() != 0;
            if (!used) continue;
            if (j.accountId == 0 && j.glAccountId == 0) throw new IllegalArgumentException("Account Required in Party AddLess Grid And row No: " + n);
            Journal pj = new Journal();
            pj.ChartofAccountId = j.glAccountId; pj.TransporterSupCustId = j.accountId; pj.JvRemarks = j.remarks == null ? "" : j.remarks;
            pj.JvPrcnt = nz(j.percentage); pj.JvQty = nz(j.qty); pj.JvRate = nz(j.rate); pj.JvDebit = nz(j.debit); pj.JvCredit = nz(j.credit);
            if (pj.JvDebit.signum() == 0 && pj.JvCredit.signum() == 0) throw new IllegalArgumentException("Credit or Debit is Required in Party AddLess Grid And row No: " + n);
            if ((pj.JvDebit.signum() > 0 || pj.JvCredit.signum() > 0) && pj.JvRemarks.isEmpty())
                throw new IllegalArgumentException("Remarks Field is Required in Party AddLess Grid And row No: " + n);
            inv.journals.add(pj);
        }

        // InvSaleInvoice.Save (0347)
        String proc;
        if (r.id == 0) { h.ModifyUserId = 0; proc = "pcc.USP_InvSaleInvoice_Insert"; }
        else { h.Id = r.id; h.EntryUserId = 0; proc = "pcc.USP_InvSaleInvoice_Update"; }
        List<Map<String, Object>> attBefore = attachments.remember(SCREEN, r.id);
        financial.makeVoucher(inv);
        int id = persist.persist(inv, proc);
        if (id <= 0) id = h.Id;
        if (id <= 0) throw new IllegalStateException("The sale invoice return could not be saved.");
        attachments.write(SCREEN, DOC_TYPE, id, h.SupplierCustomerId, attBefore, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", h.DocNo);
        out.put("updated", r.id > 0);
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("message", (r.id > 0 ? "Record Update Successfully [" : "Record Saved Successfully [") + h.DocNo + "] ");
        return out;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click)

    @Transactional
    public Map<String, Object> delete(int id) {
        lk.need(sup.rights(SCREEN), "delete", "Delete");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> h = header(id);
        if (toBool(ci(h, "IsApproved"))) throw new IllegalArgumentException("Record Not Delete beacause Record has approved");
        sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE, "EntryUserId", sup.userId(), "Id", id, "Activity", "DeleteById");
        return row("message", "Delete Voucher Successfully");
    }

    // ------------------------------------------------------------------ Prints

    /** CommonServices.SaleInvoiceReturnConcreteCustomerSlip: SaleInvoiceDirectSlip rows + SalesCustomerBillSubReport (SaleInvoice_SubReport.rpt). */
    public Map<String, Object> slip(int id) {
        if (id <= 0) throw new IllegalArgumentException("No Record Found For Display");
        lk.need(sup.rights(SCREEN), "print", "Print");
        header(id);
        List<Map<String, Object>> rows = sup.rows("pcc.USP_InvSaleInvoice_DirectSlip", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> sub = sup.rows("pcc.USP_InvSaleInvoice_SubReport", "InvSaleInvoiceId", id);
        return row("rows", rows, "sub", sub);
    }

    /** CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(id, 1862), 1862): VoucherSlipForInventoryReport (ApprovedFilter "All"). */
    public List<Map<String, Object>> voucherSlip(int id) {
        lk.need(sup.rights(SCREEN), "print", "Print");
        header(id);
        int vh = repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id);
        if (vh == 0) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = DesktopProc.rows(sup.jdbc(), "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt",
                orderedParams("OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", vh, "DocumentTypeId", DOC_TYPE));
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return rows;
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
