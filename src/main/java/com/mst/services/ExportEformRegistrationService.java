package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportEformRegistrationRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of two desktop forms that write the same ExImEFormRegistration document:
 *
 *   217 ExpfrmEformRegistration   "E-Form Registration"   - header + Payment Terms + Commodity + Advance Payment Utilization
 *   218 ExpfrmFinancialInsturment "Financial Instruments" - header only (panel5 / panel6 holding the three grids are
 *                                                           Visible = false on the desktop), DocumentTypeId 207, attachments
 *
 * Rights (tblUserRights by ScreenName on the desktop; here DesktopReportRights by ScreenDefinition.Id): View to load,
 * Save for btnSave, Update for btnUpdate (both enabled from the rights), "CanView AllRecord" for the history filter.
 *
 * Desktop quirks reproduced (each also noted where it happens):
 *  - Commodity rows are added as (Commodity, NetWeight, FcAmount) but saved with FcAmount = the NetWeight cell and
 *    Weight = the FcAmount cell, and ReadById loads (CommodityDescription, FcAmount, Weight) into those same columns -
 *    the swap is symmetric and kept.
 *  - Value FC / Net Weight Kg are compared through Conversion.ToInt(text): a value with decimals reads as 0.
 *  - 217 save: an unbound Payment Terms grid (nothing added, no record opened) makes GetTotal throw
 *    "Object reference not set to an instance of an object."; the same happens for an unbound utilisation grid when
 *    payment rows exist. The page sends whether each grid was bound and the message is returned unchanged.
 *  - 217 save: the "Your Commodity Grid Fc Total Amount Is Greater Than Value Fc" message is shown for a payment-term
 *    total that differs from Value FC (the second identical check with the "PaymentTerm Detail" text is unreachable).
 *  - AdvanceUtilizationGrid is a form field the desktop never resets; the page keeps it and sends it.
 *  - 218 save: commodity rows are saved only when their FcAmount total equals Value FC; otherwise they are silently
 *    dropped (grid hidden on the desktop anyway).
 *  - FcUtilizedAmount is saved through Conversion.ToInt (rounded half to even).
 *
 * Deviations (documented, not business logic):
 *  - 217 passes new SupplierCustomer() / new SeaPorts() / new Country() without organisation and company (the desktop
 *    binds empty combos for a real company); here tenancy always comes from the session (rule 11).
 *  - 217 never sets ExpiryDate, so the desktop sends DateTime.MinValue and SQL Server raises "SqlDateTime overflow"
 *    on every save; here the parameter is omitted (the procedure default NULL is used).
 *  - 218 attachments (Attachment popup, DMS file copy) are not ported: the save sends no attachment list, so the DAL
 *    attachment branch is not reached, exactly as a desktop save without attachments.
 */
@Service
public class ExportEformRegistrationService {

    public static final int SCREEN_EFORM = 217;
    public static final int SCREEN_FI = 218;
    public static final int FI_DOCUMENT_TYPE_ID = 207;

    @Autowired private ExportEformRegistrationRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screen, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen, action);
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private static int screen(boolean fi) { return fi ? SCREEN_FI : SCREEN_EFORM; }

    // ================================================================= load

    /** Form_Load: rights, suppliercustomer, PaymentTerm, currency, Country, issueBank, bindcmbLoadingPort, DeliveryTerm. */
    public Map<String, Object> setup(boolean fi) {
        UserAccount u = user(screen(fi), "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, screen(fi), "Save"));
        perm.put("Update", allowed(u, screen(fi), "Update"));
        perm.put("Print", allowed(u, screen(fi), "Print"));
        out.put("permissions", perm);
        out.putAll(combos(u, fi));
        return out;
    }

    /** The combo reloads FormReset / BtnNew_Click repeat. */
    public Map<String, Object> combos(boolean fi) { return combos(user(screen(fi), "View"), fi); }

    private Map<String, Object> combos(UserAccount u, boolean fi) {
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "customers", () -> idName(repo.customers(u), "Id", "CompanyName"));
        put(out, "paymentTerms", () -> idName(repo.paymentTerms(u), "Id", "LcOrderTerm"));
        put(out, "currencies", () -> idName(repo.currencies(u), "Id", "CurrencyCode"));
        put(out, "countries", () -> idName(repo.countries(u), "Id", "Description"));
        /* issueBank: 217 keeps IsHomeland == "Foreign Country", 218 keeps "Home Country" */
        String home = fi ? "Home Country" : "Foreign Country";
        put(out, "banks", () -> {
            List<Map<String, Object>> b = new ArrayList<>();
            for (Map<String, Object> r : repo.banks(u)) {
                if (!home.equals(text(ci(r, "IsHomeland")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "BranchName")));
                b.add(m);
            }
            return b;
        });
        put(out, "ports", () -> idName(repo.seaPorts(u), "Id", "PortName"));
        put(out, "deliveryTerms", () -> idName(repo.deliveryTerms(), "Id", "Description"));
        return out;
    }

    /** cmbCustomer_Leave: GetDataBySuppliercustomerId(SupplierCustomerId) -> bank reference + currency combos. */
    public List<Map<String, Object>> receiptsByCustomer(boolean fi, int supplierCustomerId) {
        return receiptRows(repo.fcBankReceipts(user(screen(fi), "View"), supplierCustomerId, 0));
    }

    /** cmbBankReerence_Leave: GetDataBySuppliercustomerId(Id) - FcNetAmount, FcUtilizedAmount, currency. */
    public List<Map<String, Object>> receiptById(boolean fi, int id) {
        return receiptRows(repo.fcBankReceipts(user(screen(fi), "View"), 0, id));
    }

    private static List<Map<String, Object>> receiptRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BankFbpNo", text(ci(r, "BankFbpNo")));
            m.put("MultiCurrencyId", asInt(ci(r, "MultiCurrencyId")));
            m.put("CurrencyCode", text(ci(r, "CurrencyCode")));
            m.put("FcNetAmount", asDouble(ci(r, "FcNetAmount")));
            m.put("FcUtilizedAmount", asDouble(ci(r, "FcUtilizedAmount")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= read / history

    /**
     * ReadById: GetByID(Id)[0] (an Id with no row throws "Index was out of range..." on the desktop) plus the three
     * child lists the DAL reads.
     */
    public Map<String, Object> readById(boolean fi, int id) {
        user(screen(fi), "View");
        List<Map<String, Object>> h = repo.header(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> r = h.get(0);
        Map<String, Object> hd = new LinkedHashMap<>();
        hd.put("Id", asInt(ci(r, "Id")));
        hd.put("ConsigneeId", asInt(ci(r, "ConsigneeId")));
        hd.put("IssuingBankId", asInt(ci(r, "IssuingBankId")));
        hd.put("EFormNo", ci(r, "EFormNo") == null ? null : text(ci(r, "EFormNo")));
        hd.put("EFormDate", iso(ci(r, "EFormDate")));
        hd.put("DeliveryTermId", asInt(ci(r, "DeliveryTermId")));
        hd.put("PaymenttermId", asInt(ci(r, "PaymenttermId")));
        hd.put("TradeType", ci(r, "TradeType") == null ? null : text(ci(r, "TradeType")));
        hd.put("FcurrencyId", asInt(ci(r, "FcurrencyId")));
        hd.put("EFormValueTotal", asDouble(ci(r, "EFormValueTotal")));
        hd.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
        hd.put("AmountRs", asDouble(ci(r, "AmountRs")));
        hd.put("ConsigneeIban", ci(r, "ConsigneeIban") == null ? null : text(ci(r, "ConsigneeIban")));
        hd.put("IbanNo", ci(r, "IbanNo") == null ? null : text(ci(r, "IbanNo")));
        hd.put("NetWeightTotal", asDouble(ci(r, "NetWeightTotal")));
        hd.put("CountryId", asInt(ci(r, "CountryId")));
        hd.put("LoadingPortId", asInt(ci(r, "LoadingPortId")));
        hd.put("DestinationPortId", asInt(ci(r, "DestinationPortId")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        List<Map<String, Object>> com = new ArrayList<>();
        for (Map<String, Object> c : repo.child(id, "ReadEFormRegistrationComodityDetailbyEformHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            /* dtCommodity.Rows.Add(CommodityDescription, FcAmount, Weight) into (Commodity, NetWeight, FcAmount) */
            m.put("Commodity", text(ci(c, "CommodityDescription")));
            m.put("NetWeight", asDouble(ci(c, "FcAmount")));
            m.put("FcAmount", asDouble(ci(c, "Weight")));
            com.add(m);
        }
        out.put("commodities", com);
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> c : repo.child(id, "ReadExImEFormRegistrationPaymentTermsbyEformHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("PaymentTermId", asInt(ci(c, "ExmLcPaymentTermId")));
            m.put("PaymentTerm", text(ci(c, "PaymentTermName")));
            m.put("PaymentPercent", asDouble(ci(c, "PaymentPercent")));
            m.put("FcAmount", asDouble(ci(c, "FcAmount")));
            m.put("DueDays", String.valueOf(asInt(ci(c, "DaDays"))));
            pay.add(m);
        }
        out.put("paymentTerms", pay);
        List<Map<String, Object>> util = new ArrayList<>();
        for (Map<String, Object> c : repo.child(id, "ReadEFormRegistrationAdvancePaymentUtilizationDetailbyEformHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("BankReferenceId", asInt(ci(c, "ExImFCBankReceiptsId")));
            m.put("BankReference", text(ci(c, "BankFbpNo")));
            m.put("CurrencyCodeId", asInt(ci(c, "FcCurrencyId")));
            m.put("CurrencyCode", text(ci(c, "CurrencyCode")));
            m.put("ThisUtilize", asDouble(ci(c, "FcUtilizedAmount")));
            m.put("Remarks", text(ci(c, "Remarks")));
            util.add(m);
        }
        out.put("utilization", util);
        return out;
    }

    /** HistoryFill (tabControl1_SelectedIndexChanged to History): NoOfRecords = 50, own records without CanView AllRecord. */
    public List<Map<String, Object>> history(boolean fi) {
        UserAccount u = user(screen(fi), "View");
        boolean all = allowed(u, screen(fi), "CanView AllRecord");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, all, 50)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof java.util.Date || v instanceof java.time.temporal.Temporal) v = isoTime(v);
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    // ================================================================= save

    /**
     * btnSave_Click / btnUpdate_Click (which calls btnSave_Click): FormValidation, the form's grid checks in the
     * desktop order, then ExImEFormRegistration.Save (BLL stamps EntryDate, ModifyDate, PostDate = now).
     */
    public Map<String, Object> save(boolean fi, Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        boolean updateMode = asBool(b.get("updateMode"));
        UserAccount u = user(screen(fi), updateMode ? "Update" : "Save");
        Map<String, Object> h = map(b.get("header"));
        List<Map<String, Object>> com = list(b.get("commodities"));
        List<Map<String, Object>> pay = list(b.get("paymentTerms"));
        List<Map<String, Object>> util = list(b.get("utilization"));
        boolean payBound = asBool(b.get("paymentGridBound"));
        boolean utilBound = asBool(b.get("utilizationGridBound"));
        int advanceUtilizationGrid = asInt(b.get("advanceUtilizationGrid"));

        /* FormValidation - combo / box TEXT must not be blank */
        if (text(h.get("customerText")).isEmpty()) throw new IllegalArgumentException("Customer Field Required");
        if (text(h.get("bankText")).isEmpty()) throw new IllegalArgumentException("Issue Bank Field Required");
        if (text(h.get("deliveryTermText")).isEmpty()) throw new IllegalArgumentException("Delivery Term Field Required");
        if (text(h.get("valueFc")).isEmpty()) throw new IllegalArgumentException("ValueFc Field Required");

        int id = recId > 0 && updateMode ? recId : 0;
        Map<String, Object> hdr = new LinkedHashMap<>();
        Timestamp now = new Timestamp(System.currentTimeMillis());

        if (!fi) {
            /* 217: totalpercent from grdPaymentTerm - an unbound grid throws */
            if (!payBound) throw new IllegalStateException("Object reference not set to an instance of an object.");
            double totalPercent = 0;
            for (Map<String, Object> r : pay) totalPercent += asDouble(r.get("PaymentPercent"));
            if (totalPercent > 100.0) throw new IllegalArgumentException("Payment Term Detail Payment % Is Greater Than 100:Please Check");
        }

        double valueFcInt = convToIntText(h.get("valueFc"));
        List<Map<String, Object>> comItems = new ArrayList<>();
        if (!com.isEmpty()) {
            double num = 0, netWeight = 0;
            for (Map<String, Object> r : com) { num += asDouble(r.get("FcAmount")); netWeight += asDouble(r.get("NetWeight")); }
            double netWeightKg = convToIntText(h.get("netWeightKg"));
            boolean take = true;
            if (!fi) {
                if (num != valueFcInt || netWeightKg != netWeight)
                    throw new IllegalArgumentException("Commodity Detail Fc-Amount & Weight Not Equal :Please Check");
            } else {
                take = num == valueFcInt;   /* 218: rows are dropped when the totals differ */
            }
            if (take) {
                for (Map<String, Object> r : com) {
                    Map<String, Object> m = new LinkedHashMap<>();
                    /* model order: FcAmount, Weight, ExImEFormRegistrationId, Id, CommodityDescription (FcAmount <- NetWeight cell, Weight <- FcAmount cell) */
                    m.put("FcAmount", asDouble(r.get("NetWeight")));
                    m.put("Weight", asDouble(r.get("FcAmount")));
                    m.put("ExImEFormRegistrationId", 0);
                    m.put("Id", 0);
                    m.put("CommodityDescription", text(r.get("Commodity")));
                    comItems.add(m);
                }
            }
        }

        List<Map<String, Object>> payItems = new ArrayList<>();
        if (!pay.isEmpty()) {
            double fcCheck = 0;
            for (Map<String, Object> r : pay) fcCheck += asDouble(r.get("FcAmount"));
            double utilized = 0;
            if (!fi) {
                if (!utilBound) throw new IllegalStateException("Object reference not set to an instance of an object.");
                for (Map<String, Object> r : util) utilized += asDouble(r.get("ThisUtilize"));
                if (fcCheck != valueFcInt) throw new IllegalArgumentException("Your Commodity Grid Fc Total Amount Is Greater Than Value Fc: Please Check");
            }
            for (Map<String, Object> r : pay) {
                Map<String, Object> m = new LinkedHashMap<>();
                /* model order: FcAmount, PaymentPercent, DaDays, ExImEFormRegistrationId, ExmLcPaymentTermId, Id */
                m.put("FcAmount", asDouble(r.get("FcAmount")));
                m.put("PaymentPercent", asDouble(r.get("PaymentPercent")));
                m.put("DaDays", convToIntText(r.get("DueDays")).intValue());
                m.put("ExImEFormRegistrationId", 0);
                m.put("ExmLcPaymentTermId", asInt(r.get("PaymentTermId")));
                m.put("Id", 0);
                payItems.add(m);
                if (asInt(r.get("PaymentTermId")) == 1) {
                    advanceUtilizationGrid = 1;
                    if (!fi && fcCheck != utilized) throw new IllegalArgumentException("Your Payment Term Is Advance and Your Amount is not Equal");
                }
            }
        }

        List<Map<String, Object>> utilItems = new ArrayList<>();
        if (!util.isEmpty()) {
            for (Map<String, Object> r : util) {
                Map<String, Object> m = new LinkedHashMap<>();
                /* model order: FcUtilizedAmount, ExImEFormRegistrationId, ExImFCBankReceiptsId, FcCurrencyId, Id, Remarks */
                m.put("FcUtilizedAmount", (double) Math.rint(asDouble(r.get("ThisUtilize"))));
                m.put("ExImEFormRegistrationId", 0);
                m.put("ExImFCBankReceiptsId", asInt(r.get("BankReferenceId")));
                m.put("FcCurrencyId", asInt(r.get("CurrencyCodeId")));
                m.put("Id", 0);
                m.put("Remarks", r.get("Remarks") == null ? "" : String.valueOf(r.get("Remarks")));
                utilItems.add(m);
            }
        } else if (advanceUtilizationGrid == 1) {
            /* the page sets its own AdvanceUtilizationGrid = 1 when a payment row with PaymentTermId 1 reached this point */
            throw new IllegalArgumentException("Please Utilize Advance Amount");
        }

        /* the 32 non-virtual ExImEFormRegistration properties in model order */
        hdr.put("PostState", false);
        hdr.put("EFormDate", ts(h.get("eFormDate")));
        hdr.put("EntryDate", now);
        hdr.put("ModifyDate", now);
        hdr.put("PostDate", now);
        hdr.put("AmountRs", fi ? 0d : (double) (float) asDouble(h.get("amountRs")));
        hdr.put("EFormValueTotal", asDouble(h.get("valueFc")));
        hdr.put("ExchangeRate", fi ? 0d : (double) (float) asDouble(h.get("exchangeRate")));
        hdr.put("NetWeightTotal", fi ? 0d : asDouble(h.get("netWeightKg")));
        hdr.put("CompanyId", u.getCompanyId());
        hdr.put("ConsigneeId", asInt(h.get("consigneeId")));
        hdr.put("CountryId", asInt(h.get("countryId")));
        hdr.put("DeliveryTermId", asInt(h.get("deliveryTermId")));
        hdr.put("DestinationPortId", asInt(h.get("destinationPortId")));
        hdr.put("EntryUser", u.getId());
        hdr.put("FcurrencyId", asInt(h.get("currencyId")));
        hdr.put("Id", id);
        hdr.put("IssuingBankId", asInt(h.get("bankId")));
        hdr.put("DocumentTypeId", fi ? FI_DOCUMENT_TYPE_ID : 0);
        hdr.put("LoadingPortId", fi ? 0 : asInt(h.get("loadingPortId")));
        hdr.put("ModifyUser", u.getId());
        hdr.put("OrganizationId", u.getOrganizationId());
        hdr.put("PostUser", 0);
        hdr.put("EFormNo", h.get("eFormNo") == null ? "" : String.valueOf(h.get("eFormNo")));
        if (fi) {
            hdr.put("IbanNo", str(h.get("ibanNo")));
            hdr.put("TradeType", str(h.get("tradeTypeText")));
            hdr.put("ConsigneeIban", str(h.get("consigneeIban")));
            hdr.put("ConsigneeAddress", str(h.get("consigneeAddress")));
            hdr.put("ExpiryDate", ts(h.get("expiryDate")));
            hdr.put("Status", asBool(h.get("status")));
            hdr.put("BalanceAmount", asDouble(h.get("balanceAmount")));
            hdr.put("PaymenttermId", asInt(h.get("paymentModeId")));
        } else {
            /* IbanNo, TradeType, ConsigneeIban, ConsigneeAddress: CLR null -> not sent; ExpiryDate: see the class note */
            hdr.put("Status", false);
            hdr.put("BalanceAmount", 0d);
            hdr.put("PaymenttermId", 0);
        }

        int result = repo.save(hdr, utilItems, comItems, payItems, new ArrayList<>(), fi ? "ExpfrmFinancialInsturment" : "ExpfrmEformRegistration");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", result > 0);
        out.put("id", result);
        out.put("advanceUtilizationGrid", advanceUtilizationGrid);
        out.put("message", result > 0 ? (updateMode ? "Update SuccessFully" : "Save SuccessFully") : "");
        return out;
    }

    // ================================================================= helpers

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, id)));
            m.put("name", text(ci(r, name)));
            out.add(m);
        }
        return out;
    }

    /** Conversion.ToInt(text) = Convert.ToInt32(string) with failures 0 - "12.5" is 0. */
    private static Double convToIntText(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return (double) Math.rint(((Number) v).doubleValue());
        try { return (double) Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0d; }
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> map(Object v) { return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>(); }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Conversion.ToString(text) of a TextBox - never null. */
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0; }
    }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    private static Timestamp ts(Object v) {
        if (v == null) return new Timestamp(System.currentTimeMillis());
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(LocalDate.parse(s.substring(0, 10)).atStartOfDay()); }
            catch (Exception e2) { return new Timestamp(System.currentTimeMillis()); }
        }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static String isoTime(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toString();
        return String.valueOf(v);
    }
}
