package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.pcc.SalePccInvoiceCalc.Fmt;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Detail;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Expense;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Head;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Invoice;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Journal;
import com.mst.services.sale.pcc.SalePccInvoiceModels.Wages;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 547 "SaleInvoiceAgainstGDNConcrete" = Architecture.WinApp.pcc.Sale.SaleInvoiceAgainstGDNConcrete (Sale Pcc, module 85, document type 1861,
 * alias "Sale Invoice Against GDN").
 *
 * Desktop map (SaleInvoiceAgainstGDNConcrete.cs, 6128 lines; method : line):
 *   Load :305, DocumentNo :536, Project :558, BuildingHeightFill :574, BuildingStoreysFill :607, ReferencePartyFill :640, braches :678, suppliercustomer :694,
 *   VisitedByFill :748, TransporterAcFill :906, OtherWagesAcFill :934, SettlementDiscAccountAndCommissionDebitAccountFill :962, dtForGridComboFill :1078,
 *   ConfigurationDefault :1100, CurrencyFill :1129, MultiCurrencyFeature :1162, txtExchangeRate_TextChanged :1231, AddRowInvExpGrid :1274, grdInvExpSettings :1279,
 *   grdInvExp_CellUpdated :1362, grdSettings :1582, grd_CellUpdated :1739, AddRowInContractorWagesGrid :1984, WagesItemFillFromDetail :1987, grdContractorWagesGridSetting :2022,
 *   grdContractorWages_CellUpdated :2043, grdContractorWages_ColumnButtonClick :2111, WagesValidationWithDetail :2167, FormValidation :2635, EnableDiableCommissionFields :2723,
 *   Reset :2775, Insert :2891, BtnSave_Click :3383, ReadById :3395, btnDelete_Click :3545, btnUpdate_Click :3568, HistoryComboBind :3587, FillHistory :3740,
 *   HistoryGridSettings :3820, grdHistory_* :3902-3958, GetDetailGrdByHeadId :3969, btnPrint/btnSlip/btn294APrint :4151-4170, BillAmount :4246,
 *   GridItemNetAmountCalculate :4435, OtherItemsProportion :4478, WagesProportion :4498, SettlementDiscountProportion :4525, FreightHeaderProportion :4559,
 *   TotalCommissionAmount :4605, btnLoadGdn_Click :4920, LoadInGridDetail :4934, LoadExpData :5029, grdInvExp_KeyDown :5400, AutoUpdatedRecords :5551 (hidden admin tool, not ported).
 * Loader dialog LoadPendingGDNConcrete.cs (762 lines): StockComboFill :91, PendingGDNForInvoice :214, DetailGridBind :332, btnLoadOnInvoice_Click_1 :568.
 * BLL 0347 pcc.InvSaleInvoice.Save + SaleInvoiceFinancial (0358) + DAL 0285 SetData = SalePccInvoiceFinancial + SalePccInvoicePersist (one transaction).
 *
 * The page keeps the amounts live like the desktop text boxes; Save re-runs the proportions, GridItemNetAmountCalculate, txtExchangeRate_TextChanged and
 * BillAmount on the posted values (SalePccInvoiceGdnCalc) so the saved header and row totals never depend on a browser-side total. A row's ItemAmount /
 * Discount / ItemAmountWithDisc are the grid cells the desktop persists as they are; the server only bounds them.
 */
@Service
public class SalePccInvoiceGdnService {
    public static final String SCREEN = "SaleInvoiceAgainstGDNConcrete";
    public static final int SCREEN_ID = 547;
    public static final int DOC_TYPE = 1861;
    private static final String GET = "[pcc].[USP_InvSaleInvoice_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SalePccLookups lk;
    private final SalePccAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SalePccInvoiceFinancial financial;
    private final SalePccInvoicePersist persist;

    public SalePccInvoiceGdnService(SaleEngrSupport sup, SalePccLookups lk, SalePccAttachments attachments, SaleInvoiceRepository repo,
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

    private static Map<String, Object> orderedParams(Object... kv) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) p.put(String.valueOf(kv[i]), kv[i + 1]);
        return p;
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

    private static BigDecimal nz(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    private static BigDecimal toDecimalOrZero(String s) {
        try { return s == null || s.trim().isEmpty() ? BigDecimal.ZERO : new BigDecimal(s.trim().replace(",", "")); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    // ------------------------------------------------------------------ Load / Refresh

    public Map<String, Object> initial() {
        lk.requireView(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());
        m.putAll(lists());
        int freightAc = toInt(sup.config("FreightOutwardAc"));
        m.put("settings", row("partyByCode", sup.configBool("SupplierCustomerDefaultFilterByPartyCode"), "multiCurrency", sup.erpFeature(6), "subsidiary", sup.erpFeature(4),
                "freightAc", freightAc > 0, "creditInSaleGl", creditInSaleGl() == null ? "" : creditInSaleGl()));                                                   // Load: FreightOutwardAc > 0 shows CmbTransporterAc + txtFreightAmountHeader
        m.put("defaults", defaults());
        m.put("fmt", fmtMap(fmt()));
        m.put("history", historyCombos());
        m.put("dateTypes", SalePccLookups.dateTypes());
        m.put("fyStart", lk.fyStart());
        return m;
    }

    /** CreditAmountInItemSaleGL configuration: null when the configuration row does not exist (GridItemNetAmountCalculate reads it). */
    private Boolean creditInSaleGl() {
        String credit = sup.config("CreditAmountInItemSaleGL");
        return blank(credit) ? null : Boolean.valueOf(SaleInvoiceRepository.truthy(credit));
    }

    /** btnRefresh_Click: the combo fills of Load. */
    public Map<String, Object> lists() {
        UserAccount u = sup.user();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("currencies", mapRows(repo.currencies(u), "Id", "CurrencyCode"));                       // CurrencyFill (MultiCurrency.GetAll)
        m.put("customers", lk.customers());                                                          // suppliercustomer
        m.put("visitedBy", refParties(2));                                                           // VisitedByFill
        m.put("terms", mapRows(repo.paymentTerms(u), "Id", "TermsDescription"));                      // PaymentTerms
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Factory Loading"), row("Id", 2, "DeliveryTerm", "Delivery")));
        m.put("refSalesMan", refParties(3));                                                         // RefSalesManFill
        m.put("commTypes", List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent")));
        m.put("commUoms", List.of(row("Id", 1, "UOM", "40"), row("Id", 2, "UOM", "50"), row("Id", 3, "UOM", "60"), row("Id", 4, "UOM", "100")));
        m.put("commDebit", mapRows(repo.accountTitlesByTypes(u, "10,11,12,20,21", null), "Id", "AccountTitle"));   // SettlementDiscAccountAndCommissionDebitAccountFill
        m.put("labourAccounts", mapRows(repo.accountTitlesByTypes(u, "10,3", null), "Id", "AccountTitle"));          // TransporterAcFill / OtherWagesAcFill
        m.put("refParties", mapRows(lk.referenceParties(), "Id", "ReferencePartyName"));             // ReferencePartyFill (type 1)
        m.put("heights", mapRows(lk.buildingLookups(1), "Id", "LookupName"));                        // BuildingHeightFill
        m.put("storeys", mapRows(lk.buildingLookups(2), "Id", "LookupName"));                        // BuildingStoreysFill
        m.put("otherItems", mapRows(repo.otherItems(u), "Id", "OtherItemName"));                     // dtForGridComboFill: InventoryItemsOther.GetAll
        m.put("contractors", sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Activity", "ReadByOrganizationCompanyIdForContractorWages"));                       // dtContr
        m.put("projects", mapRows(sup.rows("Sp_Projects_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"), "Id", "ProjectName"));
        m.put("branches", mapRows(sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"), "Id", "BranchName"));
        return m;
    }

    /** ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId (2 = Visited By, 3 = Ref Sales Man). */
    private List<Map<String, Object>> refParties(int typeId) {
        return mapRows(sup.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ReferencePartyTypeId", typeId,
                "Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId"), "Id", "ReferencePartyName");
    }

    /** ConfigurationDefault: Base Currency, BaseCurrencyRate. */
    public Map<String, Object> defaults() {
        return row("baseCurrency", toInt(sup.config("Base Currency")), "baseRate", toDecimalOrZero(sup.config("BaseCurrencyRate")));
    }

    /** DocumentNo: InvSaleInvoice.GenerateInvSaleInvoiceCode (DocumentTypeId 1861, branch + financial year). */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", sup.fy(), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    /** cmbCurrency_Leave: the last exchange rate used on a 1861 voucher for the currency (0 when none). */
    public Map<String, Object> lastRate(int currencyId) {
        Double d = currencyId > 0 ? repo.lastExchangeRate(sup.user(), currencyId, DOC_TYPE) : null;
        return row("rate", d == null ? 0.0 : d);
    }

    /** grdGLedger_KeyDown F1: the account chooser (feature 4: GetVendorsAndCustomers() ; otherwise dtAccountlst = COAAllocation search). */
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

    // ------------------------------------------------------------------ Load GDN dialog (LoadPendingGDNConcrete)

    /** StockComboFill: InvGdn.GetDataForDropDownFromGdn rows split by Activity. */
    public Map<String, Object> loaderCombos() {
        List<Map<String, Object>> type = new ArrayList<>(), cat = new ArrayList<>(), item = new ArrayList<>(), cust = new ArrayList<>(), par = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("pcc.USP_GetDataForDropDownFromGdn", "OrganizationId", sup.org(), "CompanyId", sup.company())) {
            Map<String, Object> o = row("Id", ci(r, "Id"), "name", ci(r, "name"));
            switch (str(ci(r, "Activity"))) {
                case "ItemTypes" -> type.add(o);
                case "ItemCategories" -> cat.add(o);
                case "Items" -> item.add(o);
                case "Supplier_Customer" -> cust.add(o);
                case "ParentCategories" -> par.add(o);
                default -> { }
            }
        }
        return row("itemTypes", type, "categories", cat, "items", item, "customers", cust, "parentCategories", par);
    }

    /** PendingGDNForInvoice: InvGdn.PendingGdnForInvoice (DocumentTypeId 1855; a zero / empty filter is not sent). All detail rows are returned; the page groups them by GDN. */
    public List<Map<String, Object>> pending(String fromDate, String toDate, int fromNo, int toNo, int customerId, int categoryId, int itemId, int typeId, int parentId) {
        lk.requireView(SCREEN_ID);
        Map<String, Object> p = orderedParams("OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(), "FinancialYearId", sup.fy(), "DocumentTypeId", 1855);
        LocalDate f = SalePccLookups.parseDate(fromDate), t = SalePccLookups.parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("Todate", t);
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (categoryId != 0) p.put("ItemCategoryId", categoryId);
        if (typeId != 0) p.put("ItemTypeId", typeId);
        if (parentId != 0) p.put("InventoryParentCategoriesId", parentId);
        return DesktopProc.rows(sup.jdbc(), "[pcc].[USP_PendingGdnForInvoice]", p);
    }

    /** LoadExpData: InvGdn.GdnWagesDetailByGdnIds (the ids are sent as the desktop builds them: ",12,13"). */
    public List<Map<String, Object>> gdnWages(String ids) {
        lk.requireView(SCREEN_ID);
        String s = ids == null ? "" : ids.trim();
        if (!s.matches("[0-9,]*")) throw new IllegalArgumentException("Invalid GDN ids");
        if (s.isEmpty()) return List.of();
        return sup.rows("[pcc].[USP_InvGdn_GetAllMethod]", "InvGdnIds", s, "Activity", "GdnWagesDetailByGdnIds");
    }

    /** UpdateWagesRateForAllRows (DocDate_ValueChanged): ContractorWagesRateSchedule.GetServiceActivityByItemId(2, ItemId, DocDate, 0, ContractorId). */
    public List<Map<String, Object>> wagesRates(int itemId, String docDate, int contractorId) {
        lk.requireView(SCREEN_ID);
        LocalDate d = SalePccLookups.parseDate(docDate);
        Map<String, Object> p = new LinkedHashMap<>();
        if (contractorId != 0) p.put("ContractorId", contractorId);
        p.put("ItemId", itemId); p.put("RefDocumentTypeId", 2); p.put("EffectedDATE", d == null ? LocalDate.now() : d); p.put("Activity", "GetServiceActivityByItemId");
        return DesktopProc.rows(sup.jdbc(), "[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
    }

    // ------------------------------------------------------------------ History

    /** HistoryComboBind: InvSaleInvoice.DropDownFillFromInvSaleInvoice (DocumentTypeIds "1861"), split by Activity. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), agents = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[pcc].[USP_DropDownFillFromInvSaleInvoice]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", "1861")) {
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
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice against GDN not found in this company");
        return r.get(0);
    }

    private List<Map<String, Object>> details(int id) { return sup.rows(GET, "Id", id, "Activity", "ReadByHeaderId"); }

    /** InvSaleInvoice.GetByID: header, detail, expense, journal and wages lists (WagesTypeId 2 = extra wages, left out like ReadById does) + DMSAttachments.GetByID + VoucherHeadIdGet. */
    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("expenses", sup.rows(GET, "Id", id, "Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
        out.put("journals", sup.rows(GET, "Id", id, "Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
        List<Map<String, Object>> w = new ArrayList<>();
        for (Map<String, Object> r : sup.rows(GET, "Id", id, "Activity", "InvSaleInvoiceWagesDetail_ReadBySaleInvoiceID"))
            if (toInt(ci(r, "WagesTypeId")) != 2) w.add(r);
        out.put("wages", w);
        out.put("attachments", attachments.list(SCREEN, id));
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert)

    public static class Row {
        public int id, orderId, orderDetailId, orderNo, gdnId, gdnDetailId, warehouseId, itemId, scheduleId, attributeVarientId, jobLotId, gpNo, cityId, discountTypeId;
        public BigDecimal varientUnit = BigDecimal.ZERO, qty = BigDecimal.ZERO, itemWeight = BigDecimal.ZERO, netWeight = BigDecimal.ZERO, itemPrice = BigDecimal.ZERO,
                addLessRate = BigDecimal.ZERO, rate = BigDecimal.ZERO, itemAmount = BigDecimal.ZERO, discRate = BigDecimal.ZERO, discAmount = BigDecimal.ZERO,
                itemAmountWithDisc = BigDecimal.ZERO;
        public String gpDate, vehicleNo;
        public boolean isFoc;
    }

    public static class Exp {
        public int id, itemId;
        public BigDecimal qty = BigDecimal.ZERO, rate = BigDecimal.ZERO, amount = BigDecimal.ZERO;
        public String remarks;
    }

    public static class Jv {
        public int accountId, glAccountId;
        public String remarks;
        public BigDecimal percentage = BigDecimal.ZERO, qty = BigDecimal.ZERO, rate = BigDecimal.ZERO, debit = BigDecimal.ZERO, credit = BigDecimal.ZERO;
    }

    public static class Wg {
        public int id, orderId, orderWagesId, itemId, attributeVarientId, contractorId, scheduleId, parentUomId;
        public String serviceActivity, remarks, itemName;
        public BigDecimal varientUnit = BigDecimal.ZERO, qty = BigDecimal.ZERO, itemNetWeight = BigDecimal.ZERO, rate = BigDecimal.ZERO, amount = BigDecimal.ZERO,
                addLess = BigDecimal.ZERO, netAmount = BigDecimal.ZERO;
    }

    public static class Request {
        public int id;
        public String docDate, docNo, referenceNo, manualBillNo, remarks, dueDays, dueDate, distance, deliveryStartDate, deliveryDays, deliveryTerm, buildingArea;
        public String refPartyName, refPartyAddress, refPartyCellNo;
        public int supplierCustomerId, visitedById, paymentTermId, commissionAgentId, refSalesManId, commissionDebitAccountId, currencyId;
        public int buildingHeightId, buildingStoreyId, referencePartyId, transporterId, otherWagesAccountId, settlementDiscountAccountId;
        public String commissionType, commissionUom, commissionRemarks;
        public BigDecimal commissionRate = BigDecimal.ZERO, commissionAmount = BigDecimal.ZERO, exchangeRate = BigDecimal.ZERO;
        public BigDecimal freightAmount = BigDecimal.ZERO, otherWages = BigDecimal.ZERO, settlementDiscount = BigDecimal.ZERO;
        public List<Row> rows = new ArrayList<>();
        public List<Exp> expenses = new ArrayList<>();
        public List<Jv> journals = new ArrayList<>();
        public List<Wg> wages = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    /** GetSupplierCustomerInfoByGLAccountId: the SupplierCustomer id of a GL account (first column of the first row), 0 when none. */
    private int partyOfGl(int glAccountId) {
        if (glAccountId <= 0) return -1;
        List<Map<String, Object>> info = sup.rows("Sp_SupplierCustomer_GetAllMethod", "GlAccountId", glAccountId, "Id", 0, "Activity", "GetSupplierCustomerInfoByGLAccountId");
        return info.isEmpty() || info.get(0).isEmpty() ? 0 : toInt(info.get(0).values().iterator().next());
    }

    private static String stop(String m) { throw new IllegalArgumentException(m); }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        Map<String, Boolean> rt = sup.rights(SCREEN);
        lk.need(rt, r.id > 0 ? "update" : "save", r.id > 0 ? "Update" : "Save");
        Fmt f = fmt();
        boolean multi = sup.erpFeature(6);                                                         // HasMultiCurrencyFeature
        List<Row> rows = r.rows == null ? List.of() : r.rows;
        List<Exp> exps = r.expenses == null ? List.of() : r.expenses;
        List<Jv> jvs = r.journals == null ? List.of() : r.journals;
        List<Wg> wgs = r.wages == null ? List.of() : r.wages;
        if (rows.isEmpty()) stop("Grid Record Not Found");                                         // Insert: grd.GetRows().Count() == 0

        Set<Integer> savedIds = new HashSet<>(), savedExp = new HashSet<>(), savedWages = new HashSet<>();
        if (r.id > 0) {
            if (toBool(ci(header(r.id), "IsApproved"))) stop("Record Not Update beacause Record has approved");     // btnUpdate_Click
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(ci(d, "Id")));
            for (Map<String, Object> d : sup.rows(GET, "Id", r.id, "Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID")) savedExp.add(toInt(ci(d, "Id")));
            for (Map<String, Object> d : sup.rows(GET, "Id", r.id, "Activity", "InvSaleInvoiceWagesDetail_ReadBySaleInvoiceID")) savedWages.add(toInt(ci(d, "Id")));
        }

        // ---- the header chain on the posted values: proportions, GridItemNetAmountCalculate, txtExchangeRate_TextChanged, BillAmount, TotalCommissionAmount
        BigDecimal exch = nz(r.exchangeRate);
        List<SalePccInvoiceGdnCalc.Row> calc = new ArrayList<>();
        for (Row w : rows) {
            SalePccInvoiceGdnCalc.Row c = new SalePccInvoiceGdnCalc.Row();
            c.qty = nz(w.qty); c.netWeight = nz(w.netWeight); c.foc = w.isFoc;
            c.itemAmount = SalePccInvoiceCalc.amt(nz(w.itemAmount), f);
            c.discAmount = w.discountTypeId == 0 ? BigDecimal.ZERO : nz(w.discAmount);
            c.withDisc = SalePccInvoiceCalc.amt(nz(w.itemAmountWithDisc), f);
            calc.add(c);
        }
        BigDecimal expTotal = BigDecimal.ZERO, wagesTotal = BigDecimal.ZERO, jd = BigDecimal.ZERO, jc = BigDecimal.ZERO;
        for (Exp e : exps) expTotal = expTotal.add(nz(e.amount));
        for (Wg w : wgs) wagesTotal = wagesTotal.add(nz(w.netAmount));
        for (Jv j : jvs) if (j.accountId > 0 || j.glAccountId > 0) { jd = jd.add(nz(j.debit)); jc = jc.add(nz(j.credit)); }
        SalePccInvoiceGdnCalc.Head ch = new SalePccInvoiceGdnCalc.Head();
        ch.expenseTotal = expTotal; ch.wagesTotal = wagesTotal;
        ch.freight = nz(r.freightAmount); ch.otherWages = nz(r.otherWages); ch.settle = nz(r.settlementDiscount);
        ch.exchange = exch; ch.settleAccountSet = r.settlementDiscountAccountId > 0;
        String credit = sup.config("CreditAmountInItemSaleGL");
        ch.creditAmountInItemSaleGl = blank(credit) ? null : Boolean.valueOf(SaleInvoiceRepository.truthy(credit));
        ch.supplierId = r.supplierCustomerId; ch.agentId = r.commissionAgentId;
        ch.transporterParty = partyOfGl(r.transporterId); ch.wagesParty = partyOfGl(r.otherWagesAccountId); ch.settleParty = partyOfGl(r.settlementDiscountAccountId);
        BigDecimal commAmount = SalePccInvoiceCalc.amt(nz(r.commissionAmount), f);
        ch.commission = commAmount;
        SalePccInvoiceGdnCalc.chain(calc, ch, f);
        SalePccInvoiceGdnCalc.Totals t = SalePccInvoiceGdnCalc.bill(calc, jd, jc, ch, f);
        BigDecimal cm = SalePccInvoiceCalc.commission(text(r.commissionType), nz(r.commissionRate), t.billWithoutCommission, f);        // TotalCommissionAmount
        if (cm != null && (nz(r.commissionRate).signum() > 0 || commAmount.signum() == 0)) {
            commAmount = cm; ch.commission = cm;
            t = SalePccInvoiceGdnCalc.bill(calc, jd, jc, ch, f);
        }

        // ---- FormValidation()
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) stop("DocNo Field is Required");
        if (r.supplierCustomerId == 0) stop("CustomerName Field is Required");
        String termText = "";
        for (Map<String, Object> p : repo.paymentTerms(u)) if (toInt(ci(p, "Id")) == r.paymentTermId) termText = str(ci(p, "TermsDescription"));
        if (r.paymentTermId == 0) stop("Payment Term Field is Required");
        String dterm = text(r.deliveryTerm);
        if (!"Factory Loading".equals(dterm) && !"Delivery".equals(dterm)) stop("Delivery Term Field is Required");
        if (text(r.refPartyName).isEmpty()) stop("Ref Party Name Field is Required");
        if (text(r.refPartyAddress).isEmpty()) stop("Ref Party Address Field is Required");
        if (text(r.refPartyCellNo).isEmpty()) stop("Ref Party CellNo Field is Required");
        String exchText = exch.signum() == 0 ? "0" : exch.stripTrailingZeros().toPlainString();
        if (multi) {
            if (r.currencyId == 0) stop("Fcy Code Field is Required");
            if (exchText.equals("0")) stop("Exchange Rate Field is Required");
            if (t.fcy.signum() == 0 && f.fcyPlaces == 0) stop("Fcy Amount Rate Field is Required");
        } else {
            if (r.currencyId == 0) stop("Please Configure Your Base Currency In configurations");
            if (exchText.equals("0")) stop("Please Configure Your Base Currency Rate In configurations");
        }
        if ("Credit".equals(termText) && blank(r.dueDays)) stop("Due Days Field is Required");

        // ---- Insert(): freight / labour / commission account checks
        int supplierGl = repo.glAccountIdOfParty(u, r.supplierCustomerId);                          // txtSupplierGLId
        if (ch.freight.signum() > 0) {
            if (r.transporterId == 0) stop("Transporter Account field Required");
            if (r.transporterId == supplierGl) stop("Transporter Account can not be same as Customer Please check");
        }
        if (ch.otherWages.signum() > 0) {
            if (r.otherWagesAccountId == 0) stop("Labour Account field Required");
            if (r.otherWagesAccountId == supplierGl) stop("Labour Account can not be same as Customer Please check");
        }
        if (commAmount.signum() > 0) {
            if (r.commissionAgentId == 0) stop("Please Select Commission Agent Account First");
            if (r.refSalesManId == 0) stop("Please Select Ref Sales Man First");
            if (r.commissionDebitAccountId == 0) stop("CommissionDebitAccount Account field Required");
            List<Map<String, Object>> info = sup.rows("Sp_SupplierCustomer_GetAllMethod", "GlAccountId", r.commissionDebitAccountId, "Id", 0, "Activity", "GetSupplierCustomerInfoByGLAccountId");
            if (!info.isEmpty() && !info.get(0).isEmpty() && toInt(info.get(0).values().iterator().next()) == r.commissionAgentId)
                stop("CommissionDebit Account can not be same as Commission agent Please check");
        }
        for (int i = 0; i < jvs.size(); i++) {
            Jv j = jvs.get(i);
            if ((nz(j.credit).signum() > 0 || nz(j.debit).signum() > 0) && j.accountId == 0 && j.glAccountId == 0)
                stop("Please Select an Account Party Addless Grid in Row no: " + (i + 1));
        }
        for (Exp e : exps) if (nz(e.amount).signum() > 0 && e.itemId == 0) stop("Please Select an Item Against Expense First");

        // WagesValidationWithDetail
        if (!rows.isEmpty() && !wgs.isEmpty()) {
            for (Wg w : wgs) {
                if (w.itemId != 0) {
                    boolean found = false;
                    for (Row d : rows) if (d.itemId == w.itemId) { found = true; break; }
                    if (!found) stop("Item " + text(w.itemName) + " in Contractor Wages Grid  does not exist in Detail Grid ");
                }
            }
        }
        if (t.itemAmount.signum() == 0 || t.itemNet.signum() == 0) stop("ItemAmount and ItemNetAmount can not equal to zero Please check");

        LocalDate docDate = SalePccLookups.parseDate(r.docDate);
        if (docDate == null) stop("DocDate Field is Required");
        int dueDays = toInt(text(r.dueDays));

        // ---- the invoice (Insert)
        LocalDateTime now = LocalDateTime.now();
        Invoice inv = new Invoice();
        Head h = inv.h;
        h.OrganizationId = u.getOrganizationId(); h.CompanyId = u.getCompanyId(); h.BranchesId = u.getBranchesId(); h.ProjectsId = u.getBranchesId();
        h.FinancialYearId = sup.fy(); h.DocumentTypeId = DOC_TYPE;
        h.EntryDate = now; h.ModifyDate = now; h.ApprovedDate = now;
        h.EntryUserId = u.getId(); h.ModifyUserId = u.getId(); h.IsApproved = false;
        h.DocDate = docDate.atStartOfDay(); h.DocNo = toInt(no);
        h.ManualBillNo = text(r.manualBillNo); h.RefrenenceNo = text(r.referenceNo);
        h.SupplierCustomerId = r.supplierCustomerId; h.VisitedById = r.visitedById; h.RemarksHeader = text(r.remarks);
        h.PaymentTermsId = r.paymentTermId;
        h.DueDays = dueDays;
        LocalDate due = SalePccLookups.parseDate(r.dueDate);
        h.DueDate = (due != null ? due : docDate.plusDays(dueDays)).atStartOfDay();                 // obj.DueDate = duedate.Value
        h.DeliveryTerm = dterm;
        h.Distance = text(r.distance);
        LocalDate ds = SalePccLookups.parseDate(r.deliveryStartDate);
        int dDays = toInt(text(r.deliveryDays));
        h.DeliveryStartDate = (ds == null ? LocalDate.now() : ds).atStartOfDay();
        h.DeliveryDays = dDays;
        h.ExpiryDate = h.DeliveryStartDate.plusDays(dDays);
        h.BuildingHeightId = r.buildingHeightId; h.BuildingStoreyId = r.buildingStoreyId; h.BuildingArea = text(r.buildingArea);
        h.ReferencePartyId = r.referencePartyId; h.ReferencPartyName = text(r.refPartyName);
        h.ReferencPartyAddress = text(r.refPartyAddress); h.ReferencPartyCellNo = text(r.refPartyCellNo);
        h.CommissionAgentId = r.commissionAgentId; h.RefSalesManId = r.refSalesManId;
        h.CommissionType = text(r.commissionType);
        h.CommissionRate = nz(r.commissionRate);
        h.CommissionUom = toDecimalOrZero(r.commissionUom);
        h.CommissionAmount = commAmount;
        h.CommissionRemarks = text(r.commissionRemarks);
        h.InvoiceQty = t.qty; h.InvoiceWeight = t.weight;
        h.ExchangeRate = exch; h.CurrencyId = r.currencyId; h.FcyAmount = t.fcy;
        h.TransporterId = r.transporterId; h.OtherWagesAccountId = r.otherWagesAccountId;
        h.SettlementDiscountAccountId = r.settlementDiscountAccountId; h.CommissionDebitAcId = r.commissionDebitAccountId;
        h.ItemAmountHeader = t.itemAmount; h.DiscountAmountHeader = t.discount; h.ItemNetAmountHeader = t.itemNet;
        h.OtherWagesHeader = ch.otherWages; h.FrieghtAmountHeader = ch.freight; h.SettlementDiscountHeader = ch.settle;
        h.BillAmountWithoutDiscount = t.billWithoutCommission; h.BillAmount = t.bill;

        boolean focExist = false;
        for (int i = 0; i < rows.size(); i++) {
            Row w = rows.get(i);
            SalePccInvoiceGdnCalc.Row c = calc.get(i);
            int n = i + 1;
            if (w.id > 0 && !savedIds.contains(w.id)) stop("Invalid detail row for this sale invoice against GDN");
            Detail d = new Detail();
            d.Id = w.id;
            d.ActionTypeId = w.id > 0 ? 2 : 1;
            d.LineId = n;
            d.InvGdnDetailId = w.gdnDetailId; d.InvGdnId = w.gdnId; d.SaleOrderId = w.orderId; d.SaleOrderDetailId = w.orderDetailId; d.SaleOrderNo = w.orderNo;
            if (!focExist) focExist = !w.isFoc;
            if (w.warehouseId == 0) stop("Warehouse Required in Detail Grid And row No: " + n);
            d.WarehouseId = w.warehouseId;
            if (w.itemId == 0) stop("Item Required in Detail Grid And row No: " + n);
            d.ItemId = w.itemId;
            if (w.attributeVarientId == 0) stop("AttributeVarient Required in Detail Grid And row No: " + n);
            d.ItemAttributeVarientId = w.attributeVarientId;
            d.ScheduleId = w.scheduleId;
            if (nz(w.varientUnit).signum() == 0) stop("VarientUnit Required in Detail Grid And row No: " + n);
            d.VarientEquivalent = nz(w.varientUnit);
            if (w.jobLotId == 0) stop("JobLot Required in Detail Grid And row No: " + n);
            d.JobLotId = w.jobLotId;
            if (nz(w.qty).signum() == 0) stop("ItemQty Required in Detail Grid And row No: " + n);
            d.ItemQty = nz(w.qty);
            if (nz(w.itemWeight).signum() == 0) stop("ItemWeight Required in Detail Grid And row No: " + n);
            d.ItemWeight = nz(w.itemWeight);
            if (nz(w.netWeight).signum() == 0) stop("NetWeight Required in Detail Grid And row No: " + n);
            d.ItemNetWeight = nz(w.netWeight);
            if (nz(w.itemPrice).signum() == 0) stop("ItemPrice Required in Detail Grid And row No: " + n);
            d.ItemRateWithOutAddLess = nz(w.itemPrice);
            if (nz(w.rate).signum() <= 0) stop("ItemRate Required in Detail Grid And row No: " + n + "\nItemRate Can not Less than Equal to 0");
            d.ItemRate = nz(w.rate);
            d.RateAddLess = nz(w.addLessRate);
            d.RateCut = d.RateAddLess;
            if (nz(w.itemAmount).signum() == 0) stop("ItemAmount Required in Detail Grid And row No: " + n);
            d.ItemAmount = SalePccInvoiceCalc.rnd(nz(w.itemAmount), f.amtRound);
            if ((nz(w.discRate).signum() > 0 || nz(w.discAmount).signum() > 0) && w.discountTypeId == 0) stop("DiscountType Required in Detail Grid And row No: " + n);
            d.ItemDiscountTypeId = w.discountTypeId;
            if ((nz(w.discRate).signum() == 0 || nz(w.discAmount).signum() == 0) && w.discountTypeId > 0) stop("Discount Required in Detail Grid And row No: " + n);
            d.ItemDiscountRate = nz(w.discRate);
            d.ItemDiscountAmount = nz(w.discAmount);
            if (nz(w.itemAmountWithDisc).signum() == 0) stop("ItemAmountWithDisc Required in Detail Grid And row No: " + n);
            if (nz(w.itemAmountWithDisc).compareTo(nz(w.itemAmount)) > 0 || nz(w.discAmount).signum() < 0)
                stop("Discount Amount Cannot be greater than Item Amount in Detail Grid And row No: " + n);
            d.ItemAmountWithDisc = SalePccInvoiceCalc.rnd(nz(w.itemAmountWithDisc), f.amtRound);
            d.ExchangeRate = exch; d.CurrencyId = r.currencyId;
            d.FcyAmount = c.fcy;
            LocalDate gp = SalePccLookups.parseDate(w.gpDate);
            d.GpDate = (gp == null ? LocalDate.of(1900, 1, 1) : gp).atStartOfDay();
            d.GpNo = w.gpNo;
            d.VehicleNo = w.vehicleNo == null ? "" : w.vehicleNo;
            d.CityId = w.cityId;
            d.ExpenseAmount = c.expense; d.CommissionAmount = BigDecimal.ZERO; d.FreightAmount = c.freight; d.JournalAmount = c.settle; d.WagesAmount = c.wages;
            d.ItemNetAmount = SalePccInvoiceCalc.rnd(c.netAmount, f.amtRound);
            d.RemarksDetail = "";
            d.IsFOC = w.isFoc;
            inv.details.add(d);
        }
        if (inv.details.isEmpty()) stop("At least enter value/quantity in one of the rows of detail ");
        if (!focExist) stop("Not all records can be FreeOfCost in Detail Grid: ");

        // expenses (grdChargeToProduct)
        for (int i = 0; i < exps.size(); i++) {
            Exp e = exps.get(i);
            if (e.itemId == 0 && nz(e.amount).signum() == 0) continue;
            if (e.itemId == 0) stop("Item Required in Charge to Product Grid And row No: " + (i + 1));
            if (nz(e.amount).signum() == 0) stop("Amount Required in Charge to Product Grid And row No: " + (i + 1));
            Expense pe = new Expense();
            pe.Id = savedExp.contains(e.id) ? e.id : 0;
            pe.InvOtherItemId = e.itemId; pe.Qty = nz(e.qty).setScale(0, RoundingMode.DOWN); pe.Rate = nz(e.rate); pe.Amount = nz(e.amount); pe.Remarks = e.remarks == null ? "" : e.remarks;
            inv.expenses.add(pe);
        }

        // Party Add / Less rows
        for (int i = 0; i < jvs.size(); i++) {
            Jv j = jvs.get(i);
            int n = i + 1;
            boolean used = j.accountId != 0 || j.glAccountId != 0 || nz(j.credit).signum() != 0 || nz(j.debit).signum() != 0;
            if (!used) continue;
            if (j.accountId == 0 && j.glAccountId == 0) stop("Account Required in Party AddLess Grid And row No: " + n);
            Journal pj = new Journal();
            pj.ChartofAccountId = j.glAccountId; pj.TransporterSupCustId = j.accountId; pj.JvRemarks = j.remarks == null ? "" : j.remarks;
            pj.JvPrcnt = nz(j.percentage); pj.JvQty = nz(j.qty); pj.JvRate = nz(j.rate); pj.JvDebit = nz(j.debit); pj.JvCredit = nz(j.credit);
            if (pj.JvDebit.signum() == 0 && pj.JvCredit.signum() == 0) stop("Credit or Debit is Required in Party AddLess Grid And row No: " + n);
            if ((pj.JvDebit.signum() > 0 || pj.JvCredit.signum() > 0) && pj.JvRemarks.isEmpty()) stop("Remarks Field is Required in Party AddLess Grid And row No: " + n);
            inv.journals.add(pj);
        }

        // contractor wages (grdContractorWages): only rows with a schedule / contractor / item / varient
        for (int i = 0; i < wgs.size(); i++) {
            Wg w = wgs.get(i);
            if (!(w.scheduleId > 0 || w.contractorId > 0 || w.itemId > 0 || w.attributeVarientId > 0)) continue;
            int n = i + 1;
            Wages d = new Wages();
            d.WagesTypeId = 1;
            d.Id = savedWages.contains(w.id) ? w.id : 0;
            d.SaleOrderId = w.orderId; d.SaleOrderWagesId = w.orderWagesId;                           // the desktop never sets InvGdnId / InvGdnWagesId here
            if (w.itemId == 0) stop("Item  Field Required in Grid Contractor Wages Row No: " + n);
            d.ItemId = w.itemId;
            if (w.attributeVarientId == 0) stop("AttributeVarient  Field Required in Grid Contractor Wages Row No: " + n);
            d.ItemAttributeVarientId = w.attributeVarientId;
            d.VarientEquivalent = nz(w.varientUnit);
            if (w.contractorId == 0) stop("Contractor  Field Required in Grid Contractor Wages Row No: " + n);
            d.ContractorId = w.contractorId;
            if (w.scheduleId == 0) stop("Service Activity Field Required in Grid Contractor Wages Row No: " + n);
            d.ContractorWagesRateScheduleId = w.scheduleId;
            if (nz(w.amount).signum() == 0) stop("Amount Field Required in Grid Contractor Wages Row No: " + n);
            d.ParentUomId = w.parentUomId; d.Qty = nz(w.qty); d.Rate = nz(w.rate); d.ItemNetWeight = nz(w.itemNetWeight);
            d.AddLess = nz(w.addLess); d.NetAmount = nz(w.netAmount); d.Amount = nz(w.amount);
            d.Remarks = !blank(w.remarks) ? w.remarks
                    : "Activity: " + text(w.serviceActivity) + " Rate: " + d.Rate.toPlainString() + " Amount: " + d.Amount.toPlainString() + " AddLess: " + d.AddLess.toPlainString()
                    + " NetAmount: " + d.NetAmount.toPlainString();
            inv.wages.add(d);
        }

        // InvSaleInvoice.Save (0347)
        String proc;
        if (r.id == 0) { h.ModifyUserId = 0; proc = "pcc.USP_InvSaleInvoice_Insert"; }
        else { h.Id = r.id; h.EntryUserId = 0; proc = "pcc.USP_InvSaleInvoice_Update"; }
        List<Map<String, Object>> attBefore = attachments.remember(SCREEN, r.id);
        financial.makeVoucher(inv);
        int id = persist.persist(inv, proc);
        if (id <= 0) id = h.Id;
        if (id <= 0) throw new IllegalStateException("The sale invoice could not be saved.");
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

    /** CommonServices.SaleInvoiceConcreteCustomerSlip (1861-SaleInvoice_Slip) / SaleInvoiceConcreteItemSlip (1861A-SaleInvoice_Slip): InvSaleInvoice.SaleInvoiceSlip + a sub report. */
    public Map<String, Object> slip(int id, boolean itemSlip) {
        if (id <= 0) throw new IllegalArgumentException("No Record Found For Display");
        lk.need(sup.rights(SCREEN), "print", "Print");
        header(id);
        List<Map<String, Object>> rows = sup.rows("[pcc].[USP_InvSaleInvoice_Slip]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> sub = itemSlip ? sup.rows("[pcc].[USP_InvSaleInvoiceItemExpense_SubReport]", "InvSaleInvoiceId", id)
                : sup.rows("pcc.USP_InvSaleInvoice_SubReport", "InvSaleInvoiceId", id);
        return row("rows", rows, "sub", sub);
    }

    /** CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(id, 1861), 1861). */
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
