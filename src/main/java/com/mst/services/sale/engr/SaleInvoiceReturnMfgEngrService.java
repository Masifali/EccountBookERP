package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.SaleInvoiceFinancialDirect;
import com.mst.services.SaleInvoiceMfgEngrPersist;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 849 "frmSaleInvoiceReturnEngr" = Architecture.WinApp.Mfg.Sale.frmSaleInvoiceReturnEngr (module 134 Sale Engr, "Sale Invoice Return",
 * document type 1662, returns of document types 1660 / 1661). Loader dialog: frmLoadSaleInvoiceForReturnEngr ([Mfg].[USP_SaleInvoice_PendingForReturnEngr]).
 *
 * Desktop map (frmSaleInvoiceReturnEngr.cs, method : line):
 *   SupplierDbCall :935, ItemDtFillByParty :1375, ItemDetailBind :1393, BindPackUomAndRateUom :1447, bindvarientunit :1503, CastingTypeDtFillFromGlobal :1543,
 *   BindDiscountType :1596, AllTaxTypeDBCall :1615, BindTaxType :1627, FillItemFinishWeight :1831, CmbItem_Leave :1860, grd_CellUpdated :3294, Insert :3690,
 *   ReadById :4088, LoadInGridDetail (:4830 ff), toolStripButton3_Click_1 (:4925 ff), btnAdd_Click / btnUpdateDetail_Click / CalculateAmount / CalculateTaxAmount (page script).
 * The header, expense, journal and payment logic is the same code as 847 (identical methods in both desktop forms); the lines differ: they are typed in the
 * item entry panel or loaded from earlier sale invoices (Ref* columns), there is no GDN and no production stage.
 * BLL 0580 InvSaleInvoice.Save + DAL 0433 SetData (SaleInvoiceMfgEngrPersist) and the financial voucher (SaleInvoiceFinancialDirect) run in one transaction.
 *
 * The page runs the desktop's grid arithmetic by calling {@link #calc}; {@link #save} runs the same code again: loaded rows take their identity
 * from the database, typed rows are re-priced from qty, rate, rate equivalent (UOM schedule), add/less, discount and tax, so what is saved never
 * depends on what the browser computed.
 */
@Service
public class SaleInvoiceReturnMfgEngrService {
    public static final String SCREEN = "frmSaleInvoiceReturnEngr";
    public static final int DOC_TYPE = 1662;
    public static final String SRC_TYPES = "1660,1661";

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceMfgEngrPersist persist;

    public SaleInvoiceReturnMfgEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments, SaleInvoiceRepository repo, SaleInvoiceMfgEngrPersist persist) {
        this.sup = sup; this.attachments = attachments; this.repo = repo; this.persist = persist;
    }

    // ------------------------------------------------------------------ configuration (cached for a few seconds per company)

    private final Map<String, Object[]> cache = new ConcurrentHashMap<>();

    @SuppressWarnings("unchecked")
    private <T> T cached(String key, java.util.function.Supplier<T> s) {
        String k = sup.org() + "|" + sup.company() + "|" + key;
        Object[] e = cache.get(k);
        long now = System.currentTimeMillis();
        if (e != null && (long) e[0] > now) return (T) e[1];
        T v = s.get();
        cache.put(k, new Object[]{now + 15000L, v});
        if (cache.size() > 500) cache.clear();
        return v;
    }

    private static final class Cfg {
        boolean sub, multi, branchFeature, freightToExp, taxEditable, branchImplemented;
        int defaultDays, outwardFreightAc;
        int amtRound, amtPlaces, ratePlaces, fcyPlaces;
    }

    /** InitializeComponentMethod's feature / configuration reads. */
    private Cfg cfg() {
        return cached("cfg", () -> {
            Cfg c = new Cfg();
            c.sub = sup.erpFeature(4);                                                  // SubsidiaryAccountAllownOnVouchers
            c.branchFeature = sup.erpFeature(11);                                       // BranchFeature
            c.multi = sup.erpFeature(6);                                                // HasMultiCurrencyFeature
            Map<String, String> m = new HashMap<>();
            for (Map<String, Object> r : sup.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "ConfigDescriptions", "TaxPercentEditable,DebitAmountChargetoExpenseAcFreightGrid,SaleInvoiceDirectBranchWise,DefaultDaysToLessFromHistoryFromDate,FreightOutwardAc",
                    "Activity", "GetMultipleConfigurationsByConfigDescriptions"))
                m.put(str(ci(r, "ConfigDescription")), str(ci(r, "ConfigKey")));
            c.taxEditable = toBool(m.get("TaxPercentEditable"));
            c.freightToExp = toBool(m.get("DebitAmountChargetoExpenseAcFreightGrid"));
            c.branchImplemented = toBool(m.get("SaleInvoiceDirectBranchWise"));
            c.defaultDays = toInt(m.get("DefaultDaysToLessFromHistoryFromDate"));
            c.outwardFreightAc = toInt(m.get("FreightOutwardAc"));
            int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
            int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
            int fcy = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
            c.amtRound = Math.max(0, Math.min(10, amt));
            c.amtPlaces = amt >= 1 && amt <= 4 ? amt : 0;
            c.ratePlaces = rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0);
            c.fcyPlaces = fcy >= 1 && fcy <= 4 ? fcy : 0;
            return c;
        });
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                         // DocumentNoDBCall
        m.put("branchSrNo", nextBranchSrNo());                             // BranchSrNoDBCall
        m.put("lastDiscountAcId", lastDiscountAcId());                     // PreviousDiscountAcIdWithDbCall
        m.put("lastSalesTaxAcId", lastSalesTaxAcId());                     // PreviousSalesTaxAcWithDbCall
        m.put("branchId", sup.branch());
        m.put("branchName", repo.branchName(sup.user()));
        m.put("showBranchSrNo", c.branchFeature && c.branchImplemented);
        m.put("outwardFreightAc", c.outwardFreightAc);
        m.put("defaultDays", c.defaultDays);
        m.put("historyBranches", historyBranches());                       // HistoryCombosBranchFill
        m.put("fyStart", fyStart());
        return m;
    }

    /** btnFrmRefresh_Click: SupplierDBCall, FreightAccount, TaxAccount, PaymentTerm, DiscountAccount, Currency, LookUps, AllTaxType, City, OtherItems. */
    public Map<String, Object> refresh() {
        cache.clear();
        return new LinkedHashMap<>(lists());
    }

    private Map<String, Object> lists() {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", customers(c));
        m.put("terms", terms());
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")));
        m.put("currencies", sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll"));
        m.put("otherItems", otherItems());
        m.put("freightAccounts", freightAccounts(c));
        m.put("taxAccounts", accountTitles("3,6,8,16,17,18,19", null));
        m.put("glAccounts", accountTitles("3,6,8,16,17,18,19", null));      // AccountlstDBCall: the same tax-type accounts feed grdGLedger
        m.put("discountAccounts", discountAccounts());
        m.put("taxTypes", taxTypes());
        m.put("cities", cities());
        m.put("warehouses", warehouses());
        m.put("categories", categories());
        m.put("castingTypes", castingTypes());
        m.put("multiCurrency", c.multi);
        m.put("subsidiary", c.sub);
        m.put("freightDebitToExpenses", c.freightToExp);
        m.put("taxEditable", c.taxEditable);
        m.put("defaults", row("baseCurrency", sup.configInt("Base Currency"), "baseRate", toDouble(sup.config("BaseCurrencyRate"))));
        m.put("fmt", row("amountRound", c.amtRound, "amount", c.amtPlaces, "rate", c.ratePlaces, "fcy", c.fcyPlaces));
        return m;
    }

    /** SupplierDbCall: InvSaleInvoice.GetPartiesFromSaleInvoiceWithGlAccount(.., "1660,1661"). */
    private List<Map<String, Object>> customers(Cfg c) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetPartiesFromSaleInvoiceWithGlAccount", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", SRC_TYPES)) {
            Object mobile = ci(r, "MobileNo") != null ? ci(r, "MobileNo") : ci(r, "MobilePersonal");
            out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName")), "PartyCode", str(ci(r, "PartyCode")), "GlAccountId", toInt(ci(r, "GlAccountId")),
                    "CityId", toInt(ci(r, "CityId")), "CityName", str(ci(r, "CityName")), "MobileNo", str(mobile)));
        }
        return out;
    }

    /** CommonServices.GetDueTermServiceBind. */
    public List<Map<String, Object>> terms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTerms(sup.user())) out.add(row("Id", toInt(ci(r, "Id")), "TermsDescription", str(ci(r, "TermsDescription"))));
        return out;
    }

    /** InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(sup.user())) out.add(row("Id", toInt(ci(r, "Id")), "OtherItemName", str(ci(r, "OtherItemName"))));
        return out;
    }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids, idsNot). */
    private List<Map<String, Object>> accountTitles(String ids, String not) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.accountTitlesByTypes(sup.user(), ids, not)) out.add(row("Id", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "AccountTitle"))));
        return out;
    }

    /** FreightAccountDebitAndCreditDBCall: dtTransporter {Id (GL), AccountTitle, AccountCode, SupplierCustomerId}. */
    private List<Map<String, Object>> freightAccounts(Cfg c) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (c.sub) {
            for (Map<String, Object> r : sup.rows("USP_GetVendorsAndCustomersForTransporter", "OrganizationId", sup.org(), "CompanyId", sup.company()))
                out.add(row("Id", toInt(ci(r, "GlAccountId")), "SupplierCustomerId", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "CompanyName"))));
        } else {
            // the desktop's string, double comma included
            for (Map<String, Object> r : accountTitles(null, c.freightToExp ? "2,15,22" : "2,11,12,,13,14,15,20,21,22"))
                out.add(row("Id", r.get("Id"), "SupplierCustomerId", 0, "AccountTitle", r.get("AccountTitle")));
        }
        return out;
    }

    /** CommonServices.GetAccountTitleByAccountClassIds("4"). */
    private List<Map<String, Object>> discountAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "AccountClassIds", "4",
                "UserId", idOrNull(sup.userId()), "Activity", "GetAccountTitleByAccountClassIds"))
            out.add(row("Id", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "AccountTitle"))));
        return out;
    }

    /** AllTaxTypeDBCall: ItemTaxSchedule.GetAllTaxSchedule(org, company, DocDate, 2). */
    private List<Map<String, Object>> taxTypes() { return taxTypes(null); }

    /** AllTaxTypeDBCall: ItemTaxSchedule.GetAllTaxSchedule(.., DocDate.Value, 2) - the tax types effective on the document date. */
    public List<Map<String, Object>> taxTypes(String docDate) {
        LocalDate eff = parseDate(docDate);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "EffectedDate", eff == null ? LocalDate.now() : eff,
                "TaxTypeId", 2, "Activity", "GetAllTaxSchedule"))
            out.add(row("TaxNameId", toInt(ci(r, "TaxNameId")), "TaxName", str(ci(r, "TaxName")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
        return out;
    }

    /** CityDBCall: CommonServices.CityGetAllService = SP_City_GetAllMethod 'GetAll' (Id, CityName). */
    private List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll")) {
            Object name = ci(r, "CityName") != null ? ci(r, "CityName") : (ci(r, "Description") != null ? ci(r, "Description") : ci(r, "City"));
            out.add(row("Id", toInt(ci(r, "Id")), "CityName", str(name)));
        }
        return out;
    }

    /** DocumentNoDBCall: InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1662). */
    public int nextNo() { return repo.nextDocNo(sup.user(), sup.fy(), DOC_TYPE); }

    /** BranchSrNoDBCall: InvSaleInvoice.GenerateInvSaleInvoiceCodeBranch ('GenerateBranchCode', reading BranchSrNo). */
    public int nextBranchSrNo() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", idOrNull(sup.branch()),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "Activity", "GenerateBranchCode");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "BranchSrNo"));
    }

    /** PreviousDiscountAcIdWithDbCall: InvSaleInvoice.GetLastDiscountAcId. */
    public int lastDiscountAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastDiscountAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DiscountAccountId"));
    }

    /** PreviousSalesTaxAcWithDbCall: InvSaleInvoice.GetLastSaleTaxAcId. */
    public int lastSalesTaxAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastSaleTaxAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "SaleTaxAcId"));
    }

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (document type 1662). */
    public List<Map<String, Object>> lastRate(int currencyId) {
        return sup.rows("Sp_Vouchers_GetMethods", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", String.valueOf(DOC_TYPE),
                "DMultiCurrencyIds", String.valueOf(currencyId), "Activity", "GetMultiCurrencyAndLastRate");
    }

    /** cmbsuppliername_ValueChanged: ledger balance as of the document date (feature 4: subsidiary ledger). */
    public Map<String, Object> ledgerBalance(int customerId, String docDate) {
        Cfg c = cfg();
        int gl = 0;
        for (Map<String, Object> x : customers(c)) if (toInt(x.get("Id")) == customerId) gl = toInt(x.get("GlAccountId"));
        LocalDate d = parseDate(docDate);
        if (customerId <= 0 || d == null) return row("balance", 0, "glAccountId", gl);
        LocalDate start = parseDate(fyStart());
        double bal = repo.ledgerBalance(sup.user(), c.sub, customerId, gl, (start == null ? d : start).atStartOfDay(), d.atStartOfDay());
        return row("balance", bal, "glAccountId", gl);
    }

    /** DocDate_Leave: CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate). */
    public List<Map<String, Object>> taxByDate(String itemIds, String docDate) {
        LocalDate d = parseDate(docDate);
        List<Map<String, Object>> out = new ArrayList<>();
        if (d == null || itemIds == null || itemIds.isBlank()) return out;
        for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemIds", itemIds,
                "EffectedDate", d, "Activity", "GetTaxScheduleDetailbyItemIds"))
            out.add(row("ItemId", toInt(ci(r, "ItemId")), "TaxNameId", toInt(ci(r, "TaxNameId")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
        return out;
    }

    /** Start of the active financial year (clsGlobalVariables.ActiveYr.Start_Period). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    // ------------------------------------------------------------------ Load Invoice dialog (frmLoadSaleInvoiceForReturnEngr)

    /** frmLoadGRN_Load + BranchesFill: the user's own branch (BranchFeature && SaleInvoiceReturnBranchWise), else GetBranchsAllocatedToUserFromSaleInvoice(.., 0). */
    public Map<String, Object> loaderInit() {
        Cfg c = cfg();
        boolean branchWise = sup.configBool("SaleInvoiceReturnBranchWise");
        List<Map<String, Object>> branches = new ArrayList<>();
        if (c.branchFeature && branchWise) branches.add(row("Id", sup.branch(), "BranchName", repo.branchName(sup.user())));
        else for (Map<String, Object> r : sup.rows("USP_GetBranchsAllocatedToUserFromSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "UserId", sup.user().getId(), "DocumentTypeId", 0))
            branches.add(row("Id", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        return row("branches", branches, "fyStart", fyStart(), "branchFeature", c.branchFeature, "branchImplemented", branchWise);
    }

    private static String joinIds(List<Map<String, Object>> rows, String key) {
        StringBuilder b = new StringBuilder();
        for (Map<String, Object> r : rows) b.append(',').append(toInt(r.get(key)));
        return b.toString();
    }

    /** InvSaleInvoice.SaleInvoice_PendingForReturnEngr -> [Mfg].[USP_SaleInvoice_PendingForReturnEngr] (document types 1660 and 1661, BalQty > 0). */
    private List<Map<String, Object>> pending(String branchIds, LocalDate from, LocalDate to, int customerId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("FinancialYearId", sup.fy());
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[Mfg].[USP_SaleInvoice_PendingForReturnEngr]", p);
    }

    /** PendingGdnLoad: the dialog's grid (desktop column names, one row per pending invoice line). */
    public List<Map<String, Object>> loaderRows(String branchIds, String fromDate, String toDate) {
        if (branchIds == null || branchIds.replace(",", "").trim().isEmpty()) throw new Warning("Select branch first");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pending(branchIds, parseDate(fromDate), parseDate(toDate), 0)) {
            out.add(row("BranchId", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName")), "DocumentTypeId", toInt(ci(r, "DocumentTypeId")),
                    "DocumentType", str(ci(r, "DocumentType")), "Id", toInt(ci(r, "Id")), "DocNo", toInt(ci(r, "DocNo")), "DocDate", ci(r, "DocDate"),
                    "SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")), "CustomerName", str(ci(r, "CustomerName")), "PaymentTerm", str(ci(r, "TermsDescription")),
                    "BillAmount", toDouble(ci(r, "BillAmount")), "WareHouseName", str(ci(r, "WareHouseName")), "ItemId", toInt(ci(r, "ItemId")), "ItemCode", str(ci(r, "ItemCode")),
                    "ItemName", str(ci(r, "ItemName")), "ModelDescription", str(ci(r, "VarientDescription")), "CastingType", str(ci(r, "CastingType")),
                    "PackUom", str(ci(r, "PackUomCode")), "ItemQty", toDouble(ci(r, "ItemQty")), "ItemRate", toDouble(ci(r, "ItemRate")), "RateUom", str(ci(r, "RateUomCode")),
                    "ItemAmount", toDouble(ci(r, "ItemAmount")), "UsedQty", toDouble(ci(r, "UsedQty")), "UsedAmount", toDouble(ci(r, "UsedAmount")),
                    "BalQty", toDouble(ci(r, "BalQty")), "BalAmount", toDouble(ci(r, "BalAmount")), "DiscountAmount", toDouble(ci(r, "DiscountAmount")),
                    "TaxAmount", toDouble(ci(r, "TaxAmount")), "TotalItemAmount", toDouble(ci(r, "TotalItemAmount")), "EntryDate", ci(r, "EntryDate"),
                    "EntryUser", str(ci(r, "EntryUserName")), "ModifyDate", ci(r, "ModifyDate"), "ModifyUser", str(ci(r, "ModifyUserName")),
                    "ApprovalStatus", toBool(ci(r, "IsApproved")) ? "Approved" : "Not Approved", "ApprovedDate", ci(r, "ApprovedDate"), "ApprovedUser", str(ci(r, "ApprovedUserName")),
                    "GpNo", toInt(ci(r, "GpNo")), "VehicleNo", str(ci(r, "VehicleNo")), "NoOfAttachments", toInt(ci(r, "NoOfAttachments"))));
        }
        return out;
    }

    /** The pending rows of the checked invoices (BtnLoad: dtGrid rows whose Id is in the checked set). */
    private List<Map<String, Object>> pendingRowsOf(Collection<Integer> invoiceIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pending(null, null, null, 0)) if (invoiceIds.contains(toInt(ci(r, "Id")))) out.add(r);
        return out;
    }

    private static List<Integer> idList(String ids) {
        List<Integer> out = new ArrayList<>();
        if (ids != null) for (String s : ids.split(",")) { int n = toInt(s.trim()); if (n > 0 && !out.contains(n)) out.add(n); }
        return out;
    }

    /**
     * BtnLoad + LoadInGridDetail: the header fields of the first row and one grid row per pending line of the checked invoices.
     * The same-customer / same-document-type / same-branch checks of the dialog and the two "already loaded" checks keep the desktop's wording.
     */
    public Map<String, Object> loadInvoices(String invoiceIds, int customerId, int loadedDocTypeId) {
        Cfg c = cfg();
        List<Integer> ids = idList(invoiceIds);
        if (ids.isEmpty()) throw new Warning("Check the row first");
        List<Map<String, Object>> g = pendingRowsOf(ids);
        Map<String, Object> out = new LinkedHashMap<>();
        if (g.isEmpty()) { out.put("lines", List.of()); return out; }
        boolean branchWise = sup.configBool("SaleInvoiceReturnBranchWise");
        int sid = 0, doc = 0, br = 0;
        for (Map<String, Object> r : g) {
            int s = toInt(ci(r, "SupplierCustomerId")), dt = toInt(ci(r, "DocumentTypeId")), b = toInt(ci(r, "BranchId"));
            if (s != 0) { if (sid == 0) sid = s; if (sid != s) throw new Warning("Sorry!. The Selected Invoices are not of same Customer"); }
            if (dt != 0) { if (doc == 0) doc = dt; if (doc != dt) throw new Warning("Sorry!. The Selected Invoices are not of same DocumentType"); }
            if (c.branchFeature && branchWise) { if (b != 0) { if (br == 0) br = b; if (br != b) throw new Warning("Sorry!. The Selected Invoices are not of same Branch"); } }
        }
        Map<String, Object> first = g.get(0);
        if (loadedDocTypeId > 0 && loadedDocTypeId != toInt(ci(first, "DocumentTypeId")))
            throw new Warning("The already loaded rows have a different Document Type (" + docTypeName(loadedDocTypeId) + "). You cannot load rows with a different Document Type.");
        if (customerId > 0 && customerId != toInt(ci(first, "SupplierCustomerId")))
            throw new Warning("The already loaded rows are associated with a different Customer. You cannot load rows for a different Customer.");
        out.put("head", row("SupplierCustomerId", toInt(ci(first, "SupplierCustomerId")), "SupplierReferenceNo", str(ci(first, "SupplierReferenceNo")),
                "TaxAccountId", toInt(ci(first, "TaxAccountId")), "DiscountAccountId", toInt(ci(first, "DiscountAccountIdHeader")),
                "DiscountAmount", toDouble(ci(first, "DiscountAmountHeader")), "CurrencyId", toInt(ci(first, "CurrencyId")),
                "TermsDescription", str(ci(first, "TermsDescription")), "DueDays", toInt(ci(first, "DueDays")), "DeliveryTerm", str(ci(first, "DeliveryTerm")),
                "RemarksHeader", str(ci(first, "RemarksHeader"))));
        List<Map<String, Object>> lines = new ArrayList<>();
        for (Map<String, Object> r : g) lines.add(returnLine(r));
        out.put("lines", lines);
        return out;
    }

    private final Map<Integer, String> docTypeNames = new ConcurrentHashMap<>();

    private String docTypeName(int id) {
        if (id <= 0) return "";
        return docTypeNames.computeIfAbsent(id, k -> {
            try {
                List<String> l = sup.jdbc().queryForList("SELECT DocumentTypeDescription FROM DocumentType WHERE Id = ?", String.class, k);
                return l.isEmpty() || l.get(0) == null ? String.valueOf(k) : l.get(0);
            } catch (RuntimeException e) { return String.valueOf(k); }
        });
    }

    /** dtGrid.Rows.Add(...) of LoadInGridDetail from a USP_SaleInvoice_PendingForReturnEngr row (the amounts are the invoice line's own, as the desktop loads them). */
    private Map<String, Object> returnLine(Map<String, Object> g) {
        Map<String, Object> m = new LinkedHashMap<>();
        double rate = toDouble(ci(g, "ItemRate"));
        m.put("Id", 0); m.put("RefDocumentTypeId", toInt(ci(g, "DocumentTypeId"))); m.put("RefDocumentType", str(ci(g, "DocumentType")));
        m.put("RefDocId", toInt(ci(g, "Id"))); m.put("RefDocNo", toInt(ci(g, "DocNo"))); m.put("RefDocSubId", toInt(ci(g, "DetailId")));
        m.put("RefInvoiceQty", toDouble(ci(g, "BalQty")));
        m.put("WarehouseId", toInt(ci(g, "WarehouseId"))); m.put("WarehouseName", str(ci(g, "WareHouseName")));
        m.put("ItemId", toInt(ci(g, "ItemId"))); m.put("ItemCode", str(ci(g, "ItemCode"))); m.put("ItemName", str(ci(g, "ItemName")));
        m.put("PackUomId", toInt(ci(g, "ItemUOMId"))); m.put("PackUom", str(ci(g, "PackUomCode"))); m.put("PackEquivalent", toDouble(ci(g, "PackUomEquivalent")));
        m.put("VariantId", toInt(ci(g, "ItemVariantId"))); m.put("VariantDescription", str(ci(g, "VarientDescription")));
        m.put("CastingTypeId", toInt(ci(g, "CastingTypeId"))); m.put("CastingType", str(ci(g, "CastingType")));
        m.put("Remarks", "");
        m.put("ItemQty", toDouble(ci(g, "BalQty"))); m.put("NetWeight", toDouble(ci(g, "WeightFinishGoods")));
        m.put("ItemRate", rate); m.put("RateUomId", toInt(ci(g, "RateUomId"))); m.put("RateUom", str(ci(g, "RateUomCode")));
        m.put("RateEquivalent", toDouble(ci(g, "RateUomEquivalent"))); m.put("AddLessRate", toDouble(ci(g, "RateCut"))); m.put("NetRate", rate);
        m.put("DiscountType", toInt(ci(g, "DiscountTypeId"))); m.put("DiscPct", toDouble(ci(g, "DiscountPercent"))); m.put("DiscountAmount", toDouble(ci(g, "DiscountAmount")));
        m.put("ItemAmount", toDouble(ci(g, "ItemAmount"))); m.put("FcyAmount", toDouble(ci(g, "ItemAmount")));
        m.put("TaxTypeId", toInt(ci(g, "TaxNameId"))); m.put("TaxPct", toDouble(ci(g, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(g, "TaxAmount")));
        m.put("Expense", 0.0); m.put("Freights", 0.0); m.put("BillAmount", toDouble(ci(g, "TotalItemAmount")));
        m.put("GpDate", ci(g, "GpDate")); m.put("GpNo", str(ci(g, "GpNo"))); m.put("VehicleNo", str(ci(g, "VehicleNo")));
        m.put("CityId", toInt(ci(g, "CityId")));
        return m;
    }

    /** ReadById's dtGrid.Rows.Add(...) from an InvSaleInvoiceDetail row. */
    private Map<String, Object> storedLine(Map<String, Object> d) {
        double rate = toDouble(ci(d, "ItemRate")), cut = toDouble(ci(d, "RateCut"));
        int refType = toInt(ci(d, "RefRefDocumentTypeId"));
        String refName = str(ci(d, "RefRefDocumentType"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(ci(d, "Id"))); m.put("RefDocumentTypeId", refType); m.put("RefDocumentType", refName.isEmpty() ? docTypeName(refType) : refName);
        m.put("RefDocId", toInt(ci(d, "RefRefDocIdNo"))); m.put("RefDocNo", toInt(ci(d, "RefDocNo"))); m.put("RefDocSubId", toInt(ci(d, "RefDocSubId")));
        m.put("RefInvoiceQty", toDouble(ci(d, "RefInvoiceQty")));
        m.put("WarehouseId", toInt(ci(d, "WarehouseId"))); m.put("WarehouseName", str(ci(d, "WareHouseName")));
        m.put("ItemId", toInt(ci(d, "ItemId"))); m.put("ItemCode", str(ci(d, "ItemCode"))); m.put("ItemName", str(ci(d, "ItemName")));
        m.put("PackUomId", toInt(ci(d, "ItemUOMId"))); m.put("PackUom", str(ci(d, "UOMCodeItem"))); m.put("PackEquivalent", toDouble(ci(d, "PackEquivalent")));
        m.put("VariantId", toInt(ci(d, "ItemVariantId"))); m.put("VariantDescription", str(ci(d, "VariantDescription")));
        m.put("CastingTypeId", toInt(ci(d, "CastingTypeId"))); m.put("CastingType", str(ci(d, "CastingType")));
        m.put("Remarks", str(ci(d, "RemarksDetail")));
        m.put("ItemQty", toDouble(ci(d, "ItemQty"))); m.put("NetWeight", toDouble(ci(d, "WeightFinishGoods")));
        m.put("ItemRate", rate - cut); m.put("RateUomId", toInt(ci(d, "UomScheduleIdRate"))); m.put("RateUom", str(ci(d, "UOMCodeRate")));
        m.put("RateEquivalent", toDouble(ci(d, "RateUOM"))); m.put("AddLessRate", cut); m.put("NetRate", rate);
        m.put("DiscountType", toInt(ci(d, "DiscountTypeId"))); m.put("DiscPct", toDouble(ci(d, "ItemDiscount"))); m.put("DiscountAmount", toDouble(ci(d, "ItemDiscountAmount")));
        m.put("ItemAmount", toDouble(ci(d, "ItemAmount"))); m.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
        m.put("TaxTypeId", toInt(ci(d, "TaxNameId"))); m.put("TaxPct", toDouble(ci(d, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
        m.put("Expense", toDouble(ci(d, "ExpenseAmount"))); m.put("Freights", toDouble(ci(d, "FreightAmount"))); m.put("BillAmount", toDouble(ci(d, "BillAmount")));
        m.put("GpDate", shortDate(ci(d, "GpDate"))); m.put("GpNo", str(ci(d, "GpNo"))); m.put("VehicleNo", str(ci(d, "VehicleNo")));
        m.put("CityId", toInt(ci(d, "CityId")));
        return m;
    }

    // ------------------------------------------------------------------ item entry panel (DetailPanel)

    /** ItemDtFillByParty: InvSaleInvoice.GetItemsFromSaleInvoiceAgainstPartyId(.., PartyId, "1660,1661"). */
    public List<Map<String, Object>> items(int customerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (customerId <= 0) return out;
        for (Map<String, Object> r : sup.rows("USP_GetItemsFromSaleInvoiceAgainstPartyId", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "SupplierCustomerId", customerId, "DocumentTypeIds", SRC_TYPES)) {
            Object code = ci(r, "ItemCode") != null ? ci(r, "ItemCode") : ci(r, "ItemCodeNew");
            out.add(row("Id", toInt(ci(r, "Id")), "ItemName", str(ci(r, "ItemName")), "ItemCode", str(code), "ItemCategoryId", toInt(ci(r, "ItemCategoryId")),
                    "FinishWeight", toDouble(ci(r, "FinishWeight"))));
        }
        return out;
    }

    /** BindPackUomAndRateUom: CommonServices.dtUomFromGloablUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", toInt(ci(r, "Id")), "UOMCode", str(ci(r, "UOMCode")), "Equivalent", toDouble(ci(r, "Equivalent"))));
        return out;
    }

    /** bindvarientunit: ItemAttributeVarient.GetAllForCombo -> USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo'. */
    public List<Map<String, Object>> variants(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : sup.rows("USP_ItemAttributeVarient_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "GetAllForCombo"))
            out.add(row("VariantId", toInt(ci(r, "ItemAttributeVarientId")), "VariantDescription", str(ci(r, "VarientDescription"))));
        return out;
    }

    private List<Map<String, Object>> warehouses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse"))
            out.add(row("Id", toInt(ci(r, "Id")), "WareHouseName", str(ci(r, "WareHouseName"))));
        return out;
    }

    private List<Map<String, Object>> categories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_ItemCategory_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId"))
            out.add(row("Id", toInt(ci(r, "Id")), "CategoryDescription", str(ci(r, "CategoryDescription"))));
        return out;
    }

    /** CastingTypeDtFillFromGlobal: the Engr lookups of type 1. */
    private List<Map<String, Object>> castingTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[Mfg].[USP_Lookups_GetAllMethod]", "Activity", "GetDataByTypeId", "Id", 1))
            out.add(row("Id", toInt(ci(r, "Id")), "LookupName", str(ci(r, "LookupName"))));
        return out;
    }

    private static String shortDate(Object o) {
        LocalDate d = parseDate(o);
        return d == null ? "" : d.toString();
    }

    // ------------------------------------------------------------------ History

    /** HistoryCombosBranchFill: only the user's branch when SaleInvoiceDirectBranchWise, else USP_GetBranchsAllocatedToUserFromSaleInvoice(…, 1660). */
    public List<Map<String, Object>> historyBranches() {
        Cfg c = cfg();
        UserAccount u = sup.user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (c.branchImplemented) { out.add(row("Id", sup.branch(), "BranchName", repo.branchName(u))); return out; }
        for (Map<String, Object> r : sup.rows("USP_GetBranchsAllocatedToUserFromSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(), "UserId", u.getId(), "DocumentTypeId", DOC_TYPE))
            out.add(row("Id", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        return out;
    }

    /** HistoryComboFill: Usp_AllComboAgainstSaleInvoice, the 'Supplier' and 'PaymentTerm' rows. */
    public Map<String, Object> historyCombos(String branchIds) {
        UserAccount u = sup.user();
        String ids = branchIds == null ? "" : branchIds.trim();
        if (ids.isEmpty()) throw new Warning("Select branch first");
        List<Map<String, Object>> cust = new ArrayList<>(), pay = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Usp_AllComboAgainstSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(DOC_TYPE), "BranchesIds", ids)) {
            String a = str(ci(r, "Activity"));
            if ("Supplier".equals(a)) cust.add(row("Id", toInt(ci(r, "Id")), "Customer", str(ci(r, "ReferenceName"))));
            else if ("PaymentTerm".equals(a)) pay.add(row("Id", toInt(ci(r, "Id")), "PaymentTerm", str(ci(r, "ReferenceName"))));
        }
        return row("customers", cust, "paymentTerms", pay);
    }

    /** GetAll: InvSaleInvoice.FormHistory. */
    public List<Map<String, Object>> history(String branchIds, String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId, int paymentTermId) {
        if (branchIds == null || branchIds.trim().isEmpty()) throw new Warning("Select branch first");
        UserAccount u = sup.user();
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        String mode = dateType == null ? "document" : dateType;
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(mode)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(mode)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(mode)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE); p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", sup.fy()); p.put("EntryUser", u.getId());
        if (f != null) p.put(fk, f);
        if (t != null) p.put(tk, t);
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (paymentTermId != 0) p.put("PaymentTermId", paymentTermId);
        p.put("BranchesIds", branchIds);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "Sp_InvSaleInvoice_GetAllMethod", p))
            out.add(row("Id", toInt(ci(r, "Id")), "VoucherHeadId", toInt(ci(r, "VoucherHeadId")), "DocNo", toInt(ci(r, "DocNo")), "DocDate", shortDate(ci(r, "DocDate")),
                    "DueDate", shortDate(ci(r, "DueDate")), "ManualBillNo", str(ci(r, "ManualBillNo")), "SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")),
                    "CustomerName", str(ci(r, "CustomerName")), "BillAmount", toDouble(ci(r, "BillAmount")), "EntryUser", str(ci(r, "UserName")), "EntryDate", ci(r, "EntryDate"),
                    "ModifyUser", str(ci(r, "ModifyUserName")), "ModifyDate", ci(r, "ModifyDate"), "ApprovedUser", str(ci(r, "ApprovedUserName")), "ApprovedDate", ci(r, "ApprovedDate"),
                    "NoOfAttachments", toInt(ci(r, "NoOfAttachments")), "Remarks", str(ci(r, "RemarksHeader"))));
        return out;
    }

    // ------------------------------------------------------------------ ReadById (InvSaleInvoice.GetByID)

    private Map<String, Object> invoice(int id) {
        Map<String, Object> inv = repo.readById(id);
        UserAccount u = sup.user();
        if (inv == null || (inv.containsKey("OrganizationId") && toInt(ci(inv, "OrganizationId")) != u.getOrganizationId())
                || (inv.containsKey("CompanyId") && toInt(ci(inv, "CompanyId")) != u.getCompanyId())
                || toInt(ci(inv, "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice not found in this company");
        return inv;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> children(Map<String, Object> inv, String key) {
        Object o = inv.get(key);
        return o instanceof List ? (List<Map<String, Object>>) o : List.of();
    }

    public Map<String, Object> record(int id) {
        Cfg c = cfg();
        Map<String, Object> inv = invoice(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : List.of("DocNo", "BranchSrNo", "DocDate", "SupplierCustomerId", "SupplierReferenceNo", "ManualBillNo", "TaxAccountId", "TransporterId",
                "TransporterCreditPartyId", "TransporterDebitGLId", "TransporterDebitPartyId", "FreightAmount", "RemarksHeader", "DeliveryTerm", "PaymentTermId", "DueDays",
                "DueDate", "BillAmount", "CurrencyId", "ExchangeRate", "DiscountAmount", "DiscountAccountId", "FcyAmount", "IsApproved", "FreightRemark"))
            head.put(k, ci(inv, k));
        head.put("Id", id);
        List<Map<String, Object>> lines = new ArrayList<>();
        int saleOrderId = 0;
        for (Map<String, Object> d : children(inv, "details")) { if (lines.isEmpty()) saleOrderId = toInt(ci(d, "SaleOrderId")); lines.add(storedLine(d)); }
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : children(inv, "expenses"))
            exp.add(row("ItemId", toInt(ci(e, "InvRevExpItemId")), "Qty", toDouble(ci(e, "Qty")), "Rate", toDouble(ci(e, "Rate")), "Amount", toDouble(ci(e, "Amount")), "Remarks", str(ci(e, "Remarks"))));
        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> j : children(inv, "journals"))
            jl.add(row("AccountId", toInt(ci(j, "ChartofAccountId")), "GlAccountId", toInt(ci(j, "ChartofAccountId")),
                    "Remarks", str(ci(j, "JvRemarks")), "Percentage", toDouble(ci(j, "JvPrcnt")), "Qty", toDouble(ci(j, "JvQty")), "Rate", toDouble(ci(j, "JvRate")),
                    "Debit", toDouble(ci(j, "JvDebit")), "Credit", toDouble(ci(j, "JvCredit"))));
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> p : children(inv, "paymentTerms"))
            pay.add(row("PaymentTermId", toInt(ci(p, "PaymentTermId")), "%ofTotal", toDouble(ci(p, "PrcntOfTotal")), "Amount", toDouble(ci(p, "Amount")),
                    "DueDays", toInt(ci(p, "DueDays")), "DueDate", ci(p, "DueDate"), "Remarks", str(ci(p, "PaymentRemarks"))));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", head);
        out.put("lines", lines);
        out.put("expenses", exp);
        out.put("journals", jl);
        out.put("payments", pay);
        out.put("saleOrderId", saleOrderId);
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** BindDetailOfHeaderId: the rows of the highlighted history invoice. */
    public List<Map<String, Object>> historyDetail(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : children(invoice(id), "details")) {
            double cut = toDouble(ci(d, "RateCut"));
            int refType = toInt(ci(d, "RefRefDocumentTypeId"));
            String refName = str(ci(d, "RefRefDocumentType"));
            out.add(row("RefDocumentType", refName.isEmpty() ? docTypeName(refType) : refName, "RefDocNo", toInt(ci(d, "RefDocNo")),
                    "Warehouse", str(ci(d, "WareHouseName")), "ItemCode", str(ci(d, "ItemCode")), "ItemName", str(ci(d, "ItemName")),
                    "PackUom", str(ci(d, "UOMCodeItem")), "VariantDescription", str(ci(d, "VariantDescription")), "CastingType", str(ci(d, "CastingType")),
                    "Remarks", str(ci(d, "RemarksDetail")), "ItemQty", toDouble(ci(d, "ItemQty")),
                    "NetWeight", toDouble(ci(d, "WeightFinishGoods")), "ItemRate", toDouble(ci(d, "ItemRate")) - cut, "RateUom", str(ci(d, "UOMCodeRate")),
                    "AddLessRate", cut, "NetRate", toDouble(ci(d, "ItemRate")), "DiscountType", str(ci(d, "DiscountType")), "DiscPct", toDouble(ci(d, "ItemDiscount")),
                    "DiscountAmount", toDouble(ci(d, "ItemDiscountAmount")), "ItemAmount", toDouble(ci(d, "ItemAmount")), "FcyAmount", toDouble(ci(d, "FcyAmount")),
                    "TaxType", str(ci(d, "TaxName")), "TaxPct", toDouble(ci(d, "TaxPercent")), "TaxAmount", toDouble(ci(d, "TaxAmount")), "Expense", toDouble(ci(d, "ExpenseAmount")),
                    "BillAmount", toDouble(ci(d, "BillAmount")), "GpDate", shortDate(ci(d, "GpDate")), "GpNo", str(ci(d, "GpNo")), "VehicleNo", str(ci(d, "VehicleNo")),
                    "CityName", str(ci(d, "CityName"))));
        }
        return out;
    }

    // ------------------------------------------------------------------ the request (common to calc and save)

    public static class Req {
        public int id;
        public String docNo;
        public String docDate;
        public int customerId;
        public String deliveryTerm;
        public int paymentTermId;
        public String dueDays;
        public String dueDate;
        public int currencyId;
        public double exchangeRate;
        public int taxAccountId;
        public int freightCrId;
        public int freightDrId;
        public double freightAmount;
        public String freightRemarks;
        public int discountAccountId;
        public double discountAmount;
        public String manualBillNo;
        public String refNo;
        public String remarks;
        public boolean taxRefresh;               // DocDate_Leave ran on this (new) invoice: re-read the tax schedule by item and date
        public List<Map<String, Object>> lines = new ArrayList<>();
        public List<Map<String, Object>> expenses = new ArrayList<>();
        public List<Map<String, Object>> journals = new ArrayList<>();
        public List<Map<String, Object>> payments = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static final class Totals {
        double bill, billText, fcy, itemAmount, tax, exp, qty;
    }

    private static double d(Map<String, Object> m, String k) { return toDouble(m.get(k)); }

    /** Math.Round(v, dec, MidpointRounding.AwayFromZero) */
    private static double round(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_UP).doubleValue();
    }

    /** Math.Round(v, dec): the default, to-even rounding. */
    private static double roundEven(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_EVEN).doubleValue();
    }

    private static String fmtMax(double v, int max) {
        java.text.DecimalFormat f = new java.text.DecimalFormat("#,##0." + "#".repeat(max));
        return f.format(BigDecimal.valueOf(v));
    }

    private static LocalDate parseDate(Object o) {
        if (o == null) return null;
        String s = String.valueOf(o).trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (RuntimeException e) { return null; }
    }

    private static List<Map<String, Object>> copy(List<Map<String, Object>> in) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (in != null) for (Map<String, Object> m : in) out.add(m == null ? new LinkedHashMap<>() : new LinkedHashMap<>(m));
        return out;
    }

    // ------------------------------------------------------------------ the grid arithmetic (grd_CellUpdated, BillAmount, *Proportion)

    /** The page's preview: the same numbers Insert() would carry, for the rows as they stand. */
    public Map<String, Object> calc(Req r) {
        Cfg c = cfg();
        List<Map<String, Object>> lines = copy(r.lines), pay = copy(r.payments);
        int customerGl = 0;
        for (Map<String, Object> x : customers(c)) if (toInt(x.get("Id")) == r.customerId) customerGl = toInt(x.get("GlAccountId"));
        Totals t = compute(c, r, lines, customerGl);
        paymentFromPercent(pay, t.billText);
        return row("lines", lines, "payments", pay, "billAmount", t.billText, "fcyAmount", t.fcy, "itemAmount", t.itemAmount, "taxAmount", t.tax, "expAmount", t.exp);
    }

    /** PaymentTermAmountCalculateFromPercent: Amount = round(bill * % / 100, 2, away from zero) when % > 0, else 0 (only when the bill is positive and rows exist). */
    private void paymentFromPercent(List<Map<String, Object>> pay, double bill) {
        if (!(bill > 0.0) || pay.isEmpty()) return;
        for (Map<String, Object> p : pay) {
            double pct = d(p, "%ofTotal");
            p.put("Amount", pct > 0.0 ? round(bill * pct / 100.0, 2) : 0.0);
        }
    }

    /**
     * Runs, in the order Insert() leaves them: every edited row's cell formulas (grd_CellUpdated), txtExchangeRate_TextChanged,
     * ExpProportion, FreightProportion, BillAmount (+ BillProportion).
     */
    private Totals compute(Cfg c, Req r, List<Map<String, Object>> lines, int customerGl) {
        Totals t = new Totals();
        double rate = r.exchangeRate;
        for (Map<String, Object> m : lines) {
            String calc = str(m.get("calc"));
            double qty = d(m, "ItemQty");
            if (!calc.isEmpty()) {
                if (calc.contains("rate")) {                                // ItemQty / AddLessRate / DiscountType / Discount% edited
                    double base = d(m, "ItemRate"), addLess = d(m, "AddLessRate");
                    double net = addLess != 0.0 ? base + addLess : base;
                    m.put("NetRate", net);
                    double eq = d(m, "RateEquivalent");
                    double amount = eq == 0.0 ? 0.0 : qty / eq * net;
                    int dt = toInt(m.get("DiscountType"));
                    double pct = d(m, "DiscPct"), da;
                    if (dt == 1) da = pct > 0.0 ? pct : 0.0;
                    else if (dt == 2) {
                        if (pct > 100.0) { pct = 100.0; m.put("DiscPct", 100.0); }
                        da = pct > 0.0 ? amount * pct / 100.0 : 0.0;
                    } else { da = 0.0; m.put("DiscPct", 0.0); }
                    m.put("DiscountAmount", da);
                    m.put("ItemAmount", amount - da);
                }
                int taxType = toInt(m.get("TaxTypeId"));
                double pct = d(m, "TaxPct"), itemAmount = d(m, "ItemAmount");
                if (pct > 100.0) pct = 100.0;                               // "Tax % Can't be greater than 100..."
                if (itemAmount > 0.0 && pct > 0.0 && taxType > 0) { m.put("TaxPct", pct); m.put("TaxAmount", round(itemAmount * pct / 100.0, c.amtRound)); }
                else { m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
            }
            m.put("FcyAmount", rate > 0.0 ? d(m, "ItemAmount") / rate : 0.0);                    // txtExchangeRate_TextChanged
            t.qty += qty; t.itemAmount += d(m, "ItemAmount"); t.tax += d(m, "TaxAmount");
        }
        for (Map<String, Object> e : r.expenses) t.exp += d(e, "Amount");
        // ExpProportion
        for (Map<String, Object> m : lines) m.put("Expense", t.exp > 0.0 && t.qty != 0.0 ? t.exp / t.qty * d(m, "ItemQty") : 0.0);
        // FreightProportion
        if (!lines.isEmpty()) {
            if (r.freightAmount > 0.0 && r.freightCrId > 0 && r.freightDrId <= 0) {
                for (Map<String, Object> m : lines) m.put("Freights", t.qty == 0.0 ? 0.0 : round(r.freightAmount / t.qty * d(m, "ItemQty"), 4));
            } else if (!(r.freightAmount > 0.0) || r.freightDrId > 0) {
                for (Map<String, Object> m : lines) m.put("Freights", 0.0);
            }
        }
        // BillAmount
        double jCredit = 0, jDebit = 0;
        for (Map<String, Object> j : r.journals) if (toInt(j.get("AccountId")) > 0) { jCredit += d(j, "Credit"); jDebit += d(j, "Debit"); }
        if (!lines.isEmpty()) {
            double bill = 0.0;
            if (!c.sub) {
                if (r.freightCrId > 0 && customerGl == r.freightCrId) bill -= r.freightAmount;
                if (r.freightDrId > 0 && customerGl == r.freightDrId) bill += r.freightAmount;
            } else {
                if (r.freightCrId > 0 && r.customerId == r.freightCrId) bill -= r.freightAmount;
                if (r.freightDrId > 0 && r.customerId == r.freightDrId) bill += r.freightAmount;
            }
            bill = bill + t.itemAmount + t.tax + t.exp;
            double diff = jCredit - jDebit;
            bill = diff < 0.0 ? bill - Math.abs(diff) : bill + diff;
            bill = bill - r.discountAmount;
            t.bill = bill;
            t.billText = round(bill, c.amtPlaces);                         // txtBillAmount.Text = BillAmount.ToString(stringFormatsingle)
            t.fcy = rate > 0.0 ? round(bill / rate, 4) : 0.0;              // ToString("#,##0.####")
        }
        // BillProportion
        for (Map<String, Object> m : lines) m.put("BillAmount", d(m, "ItemAmount") + d(m, "TaxAmount") + d(m, "Expense") + d(m, "Freights"));
        return t;
    }

    // ------------------------------------------------------------------ Save (Insert / Update)

    private static boolean has(Collection<Map<String, Object>> rows, String key, int id) {
        for (Map<String, Object> m : rows) if (toInt(ci(m, key)) == id) return true;
        return false;
    }

    @Transactional
    public Map<String, Object> save(Req r) {
        UserAccount u = sup.user();
        Cfg c = cfg();
        Map<String, Boolean> rights = sup.rights(SCREEN);
        if (!Boolean.TRUE.equals(rights.get(r.id > 0 ? "update" : "save")))
            throw new Warning("You do not have the " + (r.id > 0 ? "Update" : "Save") + " right for this screen.");
        List<Map<String, Object>> lines = copy(r.lines), expenses = copy(r.expenses), journals = copy(r.journals), payments = copy(r.payments);

        // ---- lookups that the combos of the desktop are filled from (tenant scope)
        List<Map<String, Object>> customers = customers(c), terms = terms(), currencies = sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(),
                "CompanyId", sup.company(), "Activity", "ReadAll");
        List<Map<String, Object>> freightAcs = freightAccounts(c), taxAcs = accountTitles("3,6,8,16,17,18,19", null), discAcs = discountAccounts();
        List<Map<String, Object>> others = otherItems(), cities = cities(), taxTypes = taxTypes(r.docDate);
        Map<String, Object> customer = null;
        for (Map<String, Object> x : customers) if (toInt(x.get("Id")) == r.customerId) customer = x;
        int customerGl = customer == null ? 0 : toInt(customer.get("GlAccountId"));
        String fKey = c.sub ? "SupplierCustomerId" : "Id";
        if (!has(freightAcs, fKey, r.freightCrId)) r.freightCrId = 0;
        if (!has(freightAcs, fKey, r.freightDrId)) r.freightDrId = 0;
        if (!has(taxAcs, "Id", r.taxAccountId)) r.taxAccountId = 0;
        if (!has(discAcs, "Id", r.discountAccountId)) r.discountAccountId = 0;

        Map<String, Object> stored = null;
        Map<Integer, Map<String, Object>> storedLines = new HashMap<>();
        if (r.id > 0) {
            stored = invoice(r.id);
            for (Map<String, Object> d : children(stored, "details")) storedLines.put(toInt(ci(d, "Id")), storedLine(d));
        }

        // ---- loaded rows take their identity from the database (pending invoice rows / the invoice's own rows); typed rows are re-priced here
        Set<Integer> needSrc = new LinkedHashSet<>();
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            if (id > 0 && !storedLines.containsKey(id)) throw new Warning("Detail row not found");
            if (id == 0 && toInt(m.get("RefDocId")) > 0) needSrc.add(toInt(m.get("RefDocId")));
        }
        Map<Integer, Map<String, Object>> srcByDetail = new HashMap<>();
        if (!needSrc.isEmpty()) for (Map<String, Object> g : pendingRowsOf(needSrc)) srcByDetail.putIfAbsent(toInt(ci(g, "DetailId")), g);
        Map<Integer, Map<Integer, double[]>> uomCache = new HashMap<>();
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            if (!(toInt(m.get("RefDocId")) > 0)) { typedLine(c, m, taxTypes, uomCache); continue; }
            Map<String, Object> src;
            if (id > 0) {
                src = storedLines.get(id);
                if (toInt(src.get("RefDocSubId")) != toInt(m.get("RefDocSubId"))) throw new Warning("Detail row not found");
            } else {
                Map<String, Object> g = srcByDetail.get(toInt(m.get("RefDocSubId")));
                if (g == null || toInt(ci(g, "Id")) != toInt(m.get("RefDocId"))) throw new Warning("Sale invoice row not found, or it is already fully returned");
                if (toInt(ci(g, "SupplierCustomerId")) != r.customerId) throw new Warning("The already loaded rows are associated with a different Customer. You cannot load rows for a different Customer.");
                src = returnLine(g);
            }
            mergeSource(c, m, src);
        }
        if (r.taxRefresh && r.id == 0 && !lines.isEmpty()) applyTaxRefresh(lines, r.docDate);

        Totals t = compute(c, r, lines, customerGl);

        // ---- FormValidation(), in the desktop's order
        String docNoText = r.docNo == null ? "" : r.docNo.trim();
        if (toInt(docNoText) == 0) throw new Warning("Doc No Name is Required");
        if (customer == null) throw new Warning("Customer Name is Required");
        if (!has(terms, "Id", r.paymentTermId)) throw new Warning("Payment Term is Required");
        int dueDays = toInt(r.dueDays);
        if (r.paymentTermId == 2 && dueDays == 0) throw new Warning("Due Days Field is Required");
        String deliveryTerm = r.deliveryTerm == null ? "" : r.deliveryTerm;
        if (!deliveryTerm.equals("Load") && !deliveryTerm.equals("Ponch")) throw new Warning("Delivery Term is Required");
        boolean currencyOk = has(currencies, "Id", r.currencyId);
        if (c.multi) {
            if (!currencyOk) throw new Warning("Fcy Code Field is Required");
            if (!(r.exchangeRate > 0)) throw new Warning("Exchange Rate Field is Required");
            if (t.fcy == 0.0) throw new Warning("Fcy Amount Rate Field is Required");
        } else {
            if (!currencyOk) throw new Warning("Please Configure Your Base Currency In configurations");
            if (!(r.exchangeRate > 0)) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
        if (!(r.freightAmount > 0.0) && (r.freightCrId > 0 || r.freightDrId > 0)) throw new Warning("Freight Amount Field is Required");
        if (r.freightAmount > 0.0 && r.freightCrId <= 0) throw new Warning("Freight/Ac Cr Field is Required");
        if (r.freightAmount > 0.0 && r.freightDrId <= 0) throw new Warning("Freight/Ac Dr Field is Required");
        if (r.freightAmount > 0.0 && (r.freightRemarks == null || r.freightRemarks.trim().isEmpty())) throw new Warning("Freight Remarks Field is Required");

        // ---- Insert(): journal rows, tax account, discount
        for (Map<String, Object> j : journals)
            if ((d(j, "Credit") > 0.0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new Warning("Please Select an Account Against JL First");
        for (Map<String, Object> m : lines)
            if (d(m, "TaxAmount") > 0.0 && r.taxAccountId == 0) throw new Warning("Tax Account is Required");
        if (r.discountAccountId > 0 && r.discountAmount == 0.0) throw new Warning("Discount Amount Required When Discount Account Is Present...");
        if (r.discountAmount != 0.0 && r.discountAccountId == 0) throw new Warning("Discount Ac Required When Discount Amount Is Present...");
        if (r.discountAmount >= t.billText) throw new Warning("Discount Amount Can not be Greater than or Equal to Bill Amount");
        paymentFromPercent(payments, t.billText);                           // PaymentTermAmountCalculateFromPercent
        if (t.billText == 0.0) throw new Warning("Bill Amount Required");

        // ---- header
        SaleInvoiceModels.Head h = new SaleInvoiceModels.Head();
        LocalDateTime now = LocalDateTime.now();
        LocalDate docDate = parseDate(r.docDate);
        if (docDate == null) throw new Warning("Doc No Name is Required");
        h.TaxAccountId = r.taxAccountId;
        h.BranchesId = u.getBranchesId();
        h.DocumentTypeId = DOC_TYPE;
        h.SupplierInvoiceDate = now;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ScreenName = SCREEN;
        h.ModifyUser = u.getId();
        h.FinancialYearId = sup.fy();
        h.Id = r.id;
        h.DocNo = r.id > 0 ? toInt(ci(stored, "DocNo")) : nextNo();
        if (h.DocNo == 0) throw new Warning("Doc No Name is Required");
        h.SupplierInvoiceNo = h.DocNo;
        h.BranchSrNo = r.id > 0 ? toInt(ci(stored, "BranchSrNo")) : nextBranchSrNo();
        h.DocDate = docDate.atStartOfDay();
        h.SupplierCustomerId = r.customerId;
        h.SupplierReferenceNo = str(r.refNo);
        h.ManualBillNo = text(r.manualBillNo);
        h.PaymentTermId = r.paymentTermId;
        h.DueDays = dueDays;
        LocalDate dd = parseDate(r.dueDate);
        h.DueDate = (dd != null ? dd : docDate.plusDays(dueDays)).atStartOfDay();
        h.DeliveryTerm = deliveryTerm;
        h.BillAmount = t.billText;
        h.RemarksHeader = text(r.remarks);
        h.DiscountAmount = BigDecimal.valueOf(r.discountAmount);
        h.DiscountAccountId = r.discountAccountId;
        h.ExchangeRate = BigDecimal.valueOf(r.exchangeRate);
        h.CurrencyId = r.currencyId;
        h.InvoiceQty = BigDecimal.valueOf(t.qty);
        h.InvoiceWeight = BigDecimal.valueOf(t.qty);
        h.FcyAmount = BigDecimal.valueOf(t.fcy);
        if (!c.sub) { h.TransporterId = r.freightCrId; h.TransporterCreditPartyId = 0; h.TransporterDebitGLId = r.freightDrId; h.TransporterDebitPartyId = 0; }
        else {
            for (Map<String, Object> a : freightAcs) {
                if (r.freightCrId > 0 && toInt(a.get("SupplierCustomerId")) == r.freightCrId) { h.TransporterId = toInt(a.get("Id")); h.TransporterCreditPartyId = toInt(a.get("SupplierCustomerId")); }
                if (r.freightDrId > 0 && toInt(a.get("SupplierCustomerId")) == r.freightDrId) { h.TransporterDebitGLId = toInt(a.get("Id")); h.TransporterDebitPartyId = toInt(a.get("SupplierCustomerId")); }
            }
        }
        h.FreightAmount = r.freightAmount;
        h.FreightRemark = text(r.freightRemarks);
        double taxTotal = 0.0;
        for (Map<String, Object> m : lines) if (d(m, "TaxAmount") > 0.0) taxTotal += d(m, "TaxAmount");
        h.IsTaxable = r.taxAccountId > 0 && taxTotal > 0.0;
        h.UomScheduleIdCmRate = "";
        h.AttachmentsValues = r.id > 0 && stored != null && ci(stored, "AttachmentsValues") != null ? str(ci(stored, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = r.id > 0 && stored != null && ci(stored, "CustomAttachmentsValues") != null ? str(ci(stored, "CustomAttachmentsValues")) : "";

        // ---- detail rows (per row checks, in the desktop's order)
        if (lines.isEmpty()) throw new Warning("Grid Record Not Found");
        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        BigDecimal exRate = BigDecimal.valueOf(r.exchangeRate);
        int rowNo = 0;
        for (Map<String, Object> m : lines) {
            rowNo++;
            String tail = rowNo + " In Detail Grid";
            SaleInvoiceModels.Detail pd = new SaleInvoiceModels.Detail();
            pd.Id = toInt(m.get("Id"));
            pd.RefRefDocumentTypeId = toInt(m.get("RefDocumentTypeId"));
            pd.RefRefDocIdNo = toInt(m.get("RefDocId"));
            pd.RefDocSubId = toInt(m.get("RefDocSubId"));
            pd.WarehouseId = toInt(m.get("WarehouseId"));
            pd.ItemId = toInt(m.get("ItemId"));
            pd.ItemUOMId = toInt(m.get("PackUomId"));
            pd.ItemVariantId = toInt(m.get("VariantId"));
            pd.CastingTypeId = toInt(m.get("CastingTypeId"));
            pd.ItemQty = d(m, "ItemQty");
            pd.RemarksDetail = str(m.get("Remarks"));
            pd.GrossWeight = pd.ItemQty; pd.AdLsWeight = 0.0; pd.NetBillWeight = pd.ItemQty; pd.NetStockWeight = pd.ItemQty;
            pd.UomScheduleIdRate = toInt(m.get("RateUomId"));
            pd.RateCut = toInt(m.get("AddLessRate"));                       // Conversion.ToInt(AddLessRate): a whole number
            pd.ItemRate = d(m, "NetRate");
            pd.DiscountTypeId = toInt(m.get("DiscountType"));
            pd.ItemDiscount = d(m, "DiscPct");
            pd.ItemDiscountAmount = d(m, "DiscountAmount");
            pd.ItemAmount = d(m, "ItemAmount");
            pd.ExchangeRate = exRate;
            pd.CurrencyId = r.currencyId;
            pd.FcyAmount = BigDecimal.valueOf(d(m, "FcyAmount"));
            pd.TaxNameId = toInt(m.get("TaxTypeId"));
            pd.TaxPercent = d(m, "TaxPct");
            pd.TaxAmount = roundEven(d(m, "TaxAmount"), 2);                  // Math.Round(TaxAmount, 2)
            pd.IsTaxable = pd.TaxNameId > 0 && pd.TaxPercent > 0.0 && pd.TaxAmount > 0.0 ? "true" : "false";
            pd.FreightAmount = d(m, "Freights");
            pd.JournalAmount = 0.0;
            pd.ExpenseAmount = d(m, "Expense");
            pd.BillAmount = d(m, "BillAmount");
            LocalDate gp = parseDate(m.get("GpDate"));
            pd.GpDate = gp == null ? now : gp.atStartOfDay();
            pd.DueDate = h.DueDate;
            pd.GpNo = toInt(m.get("GpNo"));
            pd.VehicleNo = str(m.get("VehicleNo"));
            pd.CityId = toInt(m.get("CityId"));
            pd.LineId = rowNo;
            if (pd.WarehouseId == 0) throw new Warning("Warehouse Required In Row#" + tail);
            if (pd.ItemId == 0) throw new Warning("Item Required In Row #" + tail);
            if (pd.ItemUOMId == 0) throw new Warning("Pack Uom Required In Row#" + tail);
            if (pd.CastingTypeId == 0) throw new Warning("CastingType Required In Row#" + tail);
            if (pd.ItemQty == 0.0) throw new Warning("Item Qty Required In Row#" + tail);
            if (pd.ItemRate == 0.0) throw new Warning("Item Rate Required In Row#" + tail);
            if (pd.UomScheduleIdRate == 0) throw new Warning("Rate Uom Required In Row#" + tail);
            if (pd.DiscountTypeId > 0 && pd.ItemDiscount <= 0.0 && pd.ItemDiscountAmount <= 0.0) pd.DiscountTypeId = 0;
            if (pd.DiscountTypeId > 0 && pd.ItemDiscount > 0.0 && pd.ItemDiscountAmount <= 0.0) throw new Warning("Discount Amount Required In Row#" + tail);
            if (pd.ItemAmount == 0.0) throw new Warning("Item Amount Required In Row#" + tail);
            if (pd.BillAmount == 0.0) throw new Warning("Bill Amount Required In Row#" + tail);
            if (pd.CityId == 0) throw new Warning("City Name Required In Row#" + tail);
            if (!has(cities, "Id", pd.CityId)) throw new Warning("City Name Required In Row#" + tail);
            inv.details.add(pd);
        }

        // ---- journals (rows whose account is chosen)
        Map<Integer, String> glIds = new HashMap<>();
        for (Map<String, Object> a : taxAcs) glIds.put(toInt(a.get("Id")), str(a.get("AccountTitle")));
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("AccountId"));
            if (acc == 0) continue;
            if (!glIds.containsKey(acc)) throw new Warning("Please Select an Account Against JL First");
            if (!c.sub && acc == customerGl) throw new Warning("Selected Account Can not be Same As Supplier Account");
            SaleInvoiceModels.Journal pj = new SaleInvoiceModels.Journal();
            pj.ChartofAccountId = acc;
            pj.TransporterSupCustId = 0;
            pj.JvRemarks = str(j.get("Remarks"));
            pj.JvPrcnt = d(j, "Percentage"); pj.JvQty = d(j, "Qty"); pj.JvRate = d(j, "Rate"); pj.JvDebit = d(j, "Debit"); pj.JvCredit = d(j, "Credit");
            inv.journals.add(pj);
        }

        // ---- payment terms
        if (payments.isEmpty()) throw new Warning("Payment Detail Record Not Found");
        BigDecimal payAmount = BigDecimal.ZERO, payPct = BigDecimal.ZERO;
        double sumPay = 0.0;
        for (Map<String, Object> p : payments) sumPay += d(p, "Amount");
        if (sumPay > 0.0) {
            int pr = 0;
            for (Map<String, Object> p : payments) {
                pr++;
                if (toDecimal(p.get("Amount")).compareTo(BigDecimal.ZERO) > 0) {
                    SaleInvoiceModels.PaymentTerm o = new SaleInvoiceModels.PaymentTerm();
                    o.PaymentTermId = toInt(p.get("PaymentTermId"));
                    o.PrcntOfTotal = toDecimal(p.get("%ofTotal"));
                    payPct = payPct.add(o.PrcntOfTotal);
                    o.Amount = toDecimal(p.get("Amount"));
                    payAmount = payAmount.add(o.Amount);
                    o.DueDays = toInt(p.get("DueDays"));
                    LocalDate pdd = parseDate(p.get("DueDate"));
                    o.DueDate = (pdd == null ? LocalDate.of(1900, 1, 1) : pdd).atStartOfDay();
                    o.PaymentRemarks = str(p.get("Remarks"));
                    if (o.PaymentTermId <= 0) throw new Warning("Payment Term Required in row#" + pr);
                    if (!has(terms, "Id", o.PaymentTermId)) throw new Warning("Payment Term Required in row#" + pr);
                    if (o.PaymentTermId == 2 && o.DueDays <= 0) throw new Warning("Due Days Required In case Of Credit row in row#" + pr);
                    inv.paymentTerms.add(o);
                }
            }
        } else {
            SaleInvoiceModels.PaymentTerm o = new SaleInvoiceModels.PaymentTerm();
            o.PaymentTermId = r.paymentTermId;
            o.PrcntOfTotal = new BigDecimal("100");
            payPct = new BigDecimal("100");
            o.Amount = BigDecimal.valueOf(100.0 * h.BillAmount / 100.0);
            payAmount = o.Amount.setScale(4, RoundingMode.HALF_EVEN);
            o.DueDays = dueDays;
            o.DueDate = h.DueDate;
            o.PaymentRemarks = "";
            inv.paymentTerms.add(o);
        }
        if (payPct.compareTo(new BigDecimal("100")) != 0) throw new Warning("Payment Detail Total% not equal to 100");
        if (payAmount.compareTo(BigDecimal.valueOf(h.BillAmount)) != 0)
            throw new Warning("Payment Detail Amount " + fmtMax(payAmount.doubleValue(), 4) + " Not Equal to Total Invoice Amount " + fmtMax(h.BillAmount, 4));

        // ---- expenses
        for (Map<String, Object> e : expenses) {
            if (toInt(e.get("ItemId")) != 0 && d(e, "Amount") != 0.0) {
                if (!has(others, "Id", toInt(e.get("ItemId")))) throw new Warning("Please Select an Item Against Expense First");
                SaleInvoiceModels.Expense pe = new SaleInvoiceModels.Expense();
                pe.InvRevExpItemId = toInt(e.get("ItemId"));
                pe.Qty = (int) Math.rint(d(e, "Qty"));                        // Conversion.ToInt(Qty): a whole number
                pe.Rate = d(e, "Rate");
                pe.Amount = d(e, "Amount");
                pe.Remarks = str(e.get("Remarks"));
                inv.expenses.add(pe);
            }
        }

        // ---- BLL 0580 Save + DAL 0433 SetData (one transaction); InvSaleInvoiceCommission is an empty list on this screen
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        attachments.apply(SCREEN, DOC_TYPE, saved, r.customerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("customerId", r.customerId);
        out.put("docDate", docDate.toString());
        out.put("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOC_TYPE, saved));
        out.put("message", (r.id == 0 ? "Record Save Successfully [" : "Record Update Successfully [") + h.DocNo + "]");
        return out;
    }

    /**
     * Puts the database's version of the identity / source columns on the row the page sent (loaded rows). Rows the user did not edit (calc empty)
     * take the source's amounts as they are; edited rows keep the editable inputs (grd: ItemQty, AddLessRate, DiscountType, Discount%, Remarks, GpDate,
     * GpNo, VehicleNo, CityId and, when TaxPercentEditable, the tax type / %).
     */
    private void mergeSource(Cfg c, Map<String, Object> m, Map<String, Object> src) {
        String calc = str(m.get("calc"));
        Map<String, Object> mine = new LinkedHashMap<>(m);
        m.clear();
        m.putAll(src);
        m.put("calc", calc);
        m.put("Remarks", str(mine.get("Remarks")));
        m.put("GpDate", mine.get("GpDate") != null && !str(mine.get("GpDate")).isEmpty() ? mine.get("GpDate") : src.get("GpDate"));
        m.put("GpNo", str(mine.get("GpNo")));
        m.put("VehicleNo", str(mine.get("VehicleNo")));
        m.put("CityId", toInt(mine.get("CityId")) > 0 ? toInt(mine.get("CityId")) : toInt(src.get("CityId")));
        int wh = toInt(mine.get("WarehouseId"));
        if (toInt(mine.get("CastingTypeId")) > 0) m.put("CastingTypeId", toInt(mine.get("CastingTypeId")));   // the grid cell is a drop-down
        if (wh > 0) m.put("WarehouseId", wh);                               // BtnUpdateInGridDetail_Click sets the warehouse of every row
        if (calc.isEmpty()) return;
        m.put("AddLessRate", d(mine, "AddLessRate"));
        if (calc.contains("rate")) {
            m.put("ItemQty", d(mine, "ItemQty"));
            m.put("DiscountType", toInt(mine.get("DiscountType")));
            m.put("DiscPct", d(mine, "DiscPct"));
        }
        if (c.taxEditable) { m.put("TaxTypeId", toInt(mine.get("TaxTypeId"))); m.put("TaxPct", d(mine, "TaxPct")); }
    }

    /**
     * A row typed in the item entry panel (no Ref*): ids come from the page, every amount is recomputed (btnAdd_Click / CalculateAmount /
     * CalculateTaxAmount) from the UOM schedule's equivalents and the tax schedule.
     */
    private void typedLine(Cfg c, Map<String, Object> m, List<Map<String, Object>> taxTypes, Map<Integer, Map<Integer, double[]>> uomCache) {
        Map<String, Object> mine = new LinkedHashMap<>(m);
        m.clear();
        int item = toInt(mine.get("ItemId"));
        Map<Integer, double[]> eq = uomCache.computeIfAbsent(item, k -> {
            Map<Integer, double[]> x = new HashMap<>();
            for (Map<String, Object> u : uoms(k)) x.put(toInt(u.get("Id")), new double[]{toDouble(u.get("Equivalent"))});
            return x;
        });
        int pack = toInt(mine.get("PackUomId")), rateUom = toInt(mine.get("RateUomId"));
        if (!eq.containsKey(pack)) pack = 0;
        if (!eq.containsKey(rateUom)) rateUom = 0;
        int taxType = toInt(mine.get("TaxTypeId"));
        double sched = 0.0;
        for (Map<String, Object> t : taxTypes) if (toInt(t.get("TaxNameId")) == taxType) sched = toDouble(t.get("TaxPercent"));
        double pct = d(mine, "TaxPct");
        pct = c.taxEditable && pct > 0.0 ? pct : sched;                      // txtTaxPercent is editable only when TaxPercentEditable
        m.put("Id", toInt(mine.get("Id"))); m.put("RefDocumentTypeId", 0); m.put("RefDocumentType", ""); m.put("RefDocId", 0); m.put("RefDocNo", 0); m.put("RefDocSubId", 0); m.put("RefInvoiceQty", 0.0);
        m.put("WarehouseId", toInt(mine.get("WarehouseId"))); m.put("ItemId", item);
        m.put("PackUomId", pack); m.put("PackEquivalent", pack > 0 ? eq.get(pack)[0] : 0.0);
        m.put("VariantId", toInt(mine.get("VariantId"))); m.put("CastingTypeId", toInt(mine.get("CastingTypeId")));
        m.put("Remarks", str(mine.get("Remarks"))); m.put("ItemQty", d(mine, "ItemQty")); m.put("NetWeight", d(mine, "NetWeight"));
        m.put("ItemRate", d(mine, "ItemRate")); m.put("RateUomId", rateUom); m.put("RateEquivalent", rateUom > 0 ? eq.get(rateUom)[0] : 0.0);
        m.put("AddLessRate", d(mine, "AddLessRate")); m.put("DiscountType", toInt(mine.get("DiscountType"))); m.put("DiscPct", d(mine, "DiscPct"));
        m.put("TaxTypeId", taxType); m.put("TaxPct", pct);
        m.put("GpDate", mine.get("GpDate")); m.put("GpNo", str(mine.get("GpNo"))); m.put("VehicleNo", str(mine.get("VehicleNo")));
        m.put("CityId", toInt(mine.get("CityId")));
        String calc = str(mine.get("calc"));
        m.put("calc", calc);
        if (!calc.isEmpty()) return;                                         // edited in the grid afterwards: compute() runs grd_CellUpdated's formulas
        // btnAdd_Click / btnUpdateDetail_Click: the amounts are the entry panel's CalculateAmount / CalculateTaxAmount results
        double qty = d(m, "ItemQty"), rate = d(m, "ItemRate"), addLess = d(m, "AddLessRate"), eqv = d(m, "RateEquivalent");
        double net = 0.0, itemAmount = 0.0, da = 0.0, dpct = d(m, "DiscPct");
        if (qty > 0.0 && rate > 0.0 && eqv > 0.0) {
            net = addLess != 0.0 ? rate + addLess : rate;
            double amount = qty / eqv * net;
            int dt = toInt(m.get("DiscountType"));
            if (dt == 1) da = dpct > 0.0 ? dpct : 0.0;
            else if (dt == 2) { if (dpct > 100.0) dpct = 100.0; da = dpct > 0.0 ? amount * dpct / 100.0 : 0.0; }
            else { da = 0.0; dpct = 0.0; }
            da = round(da, 3);                                               // txtDiscountAmount.Text is "#,##0.###"
            itemAmount = round(amount - da, c.amtRound);                     // txtAmount: Math.Round(Amount, amountDecimals, AwayFromZero)
        } else { dpct = 0.0; }
        m.put("NetRate", net); m.put("DiscPct", dpct); m.put("DiscountAmount", da); m.put("ItemAmount", itemAmount);
        double tp = d(m, "TaxPct"), ta = 0.0;
        if (taxType > 0) { if (tp > 100.0) tp = 100.0; ta = round(itemAmount * tp / 100.0, c.amtRound); }
        else tp = 0.0;
        m.put("TaxPct", tp); m.put("TaxAmount", ta);
    }

    /** DocDate_Leave: the tax schedule of the items on the document date decides the tax type / % / amount of every row (new invoice only). */
    private void applyTaxRefresh(List<Map<String, Object>> lines, String docDate) {
        StringBuilder ids = new StringBuilder();
        for (Map<String, Object> m : lines) ids.append(',').append(toInt(m.get("ItemId")));
        List<Map<String, Object>> dt = taxByDate(ids.toString(), docDate);
        for (Map<String, Object> m : lines) {
            boolean flag = false;
            for (Map<String, Object> t : dt) {
                if (toInt(t.get("ItemId")) == toInt(m.get("ItemId"))) {
                    flag = true;
                    double pct = d(t, "TaxPercent"), ia = d(m, "ItemAmount");
                    m.put("TaxTypeId", toInt(t.get("TaxNameId"))); m.put("TaxPct", pct);
                    if (ia > 0.0 && pct > 0.0) m.put("TaxAmount", ia * pct / 100.0);
                }
            }
            if (!flag) { m.put("TaxTypeId", 0); m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
        }
    }

    // ------------------------------------------------------------------ Delete

    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = sup.user();
        if (!Boolean.TRUE.equals(sup.rights(SCREEN).get("delete"))) throw new Warning("You do not have the Delete right for this screen.");
        Map<String, Object> inv = invoice(id);
        if (toBool(ci(inv, "IsApproved"))) throw new Warning("Record cannot be  Delete beacause Record has approved");
        repo.delete(u, id, DOC_TYPE);                                      // InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete
        return row("message", "Delete Record Successfully");
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { invoice(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { invoice(id); return attachments.download(SCREEN, id, attachmentId); }
}
