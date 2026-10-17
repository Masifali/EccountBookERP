package com.mst.services;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 199 "Exchange Rate Forward Booking" - desktop form Architecture.WinApp.Account_Definition.ExchangeRateForwardBooking
 * (2,668 lines; BLL Accounts.fcyExRateForwardBooking, DAL Accounts.fcyExRateForwardBooking.SetData).
 *
 * Desktop to web, by line of ExchangeRateForwardBooking.cs:
 *  - DefineCity_Load :198-224   rights (SetRightsValueInRightsObject), GenerateDocNo, BankComboFill, BranchFill, MultiCurrencyfill,
 *                                HistoryCombosFill, History From = Now - DefaultDaysToLessFromHistoryFromDate (3 when 0), To = Now.
 *  - GenerateDocNo :226         USP_FcyExRateForwardBooking_GetAllMethod @OrganizationId, @CompanyId, @FinancialYearId, @Activity='GenerateCode'
 *  - BankComboFill :246         Sp_Bank_GetAllMethod @Activity='ReadAllBankDT', only IsHomeland = "Home Country" (Id, BranchName)
 *  - BranchFill :307            Sp_Branches_GetAllMethod @Activity='GetAll' (Id, BranchName) - caption "Client"
 *  - MultiCurrencyfill :323     Sp_MultiCurrency_GetAllMethod @Activity='ReadAll' (Id, CurrencyCode) - Fcy Code and Hcy Code
 *  - HistoryCombosFill :369     USP_GetDataForDropDownFromFcyExRateForwardBooking, rows ActivityType "Bank" / "Branches"
 *  - FormValidation :494-569    the twelve messages, in the desktop's order
 *  - Insert :584-654            USP_FcyExRateForwardBooking_InsertAndUpdate (ActionId 1 / 2), attachments in the same transaction
 *  - ReadById :672              USP_FcyExRateForwardBooking_GetAllMethod @Id, @Activity='ReadById' + DMSAttachments.GetByID
 *  - GridFill :719-814          USP_FcyExRateForwardBooking_GetAllMethod @Activity='FormHistory'
 *  - calculateHcyAmount :1146, CalculateTotalDays :1270 are repeated on the server from the posted inputs.
 * The form has no delete. Print-01 runs the existing /reports/print/01-fcy-ex-rate-forwward-booking-slip.
 *
 * DELIBERATE DEPARTURE FROM THE DESKTOP (ReadById :693-704): the desktop assigns CmbHcyCode.Value = obj.FcyId,
 * txtHcyAmount = obj.FcyAmount, datStartOfOptionalPeriod = datFinalMaturity = obj.FirstMaturityDate, so an opened record
 * shows the Fcy code / amount in the Hcy fields and the first maturity as the final maturity, and Update then writes that
 * back. Here the stored HcyId, HcyAmount, OptionalPeriod and FinalMaturityDate are loaded instead, so an Update does not
 * overwrite good data. Every other field loads exactly as the desktop does.
 */
@Service
public class W4ExchangeRateForwardBookingService {

    public static final String SCREEN_NAME = "ExchangeRateForwardBooking";
    private static final String OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.";
    private static final DateTimeFormatter SHORT_DATE = DateTimeFormatter.ofPattern("dd/MM/yyyy", Locale.US);
    private static final DateTimeFormatter ENTRY_DATE = DateTimeFormatter.ofPattern("dd-MMM-yyyy hh:mm a", Locale.US);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final W4ExchangeRateForwardBookingAttachmentService attachments;

    public W4ExchangeRateForwardBookingService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights,
                                               W4ExchangeRateForwardBookingAttachmentService attachments) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
        this.attachments = attachments;
    }

    public Map<String, Boolean> rights() { return rights.of(SCREEN_NAME); }

    // ------------------------------------------------------------------------------------ Load / Refresh

    /** DefineCity_Load. */
    public Map<String, Object> init() {
        Map<String, Object> m = lookups();
        m.put("rights", rights());
        m.put("docNo", generateCode());
        m.putAll(formats());
        int days = cfgInt("DefaultDaysToLessFromHistoryFromDate");
        m.put("historyDaysBack", days > 0 ? days : 3);
        return m;
    }

    /** btnRefresh_Click :479-492 - BankComboFill, BranchFill, MultiCurrencyfill, HistoryCombosFill. */
    public Map<String, Object> lookups() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("banks", banks());
        m.put("branches", branches());
        m.put("currencies", currencies());
        m.putAll(historyCombos());
        return m;
    }

    public List<Map<String, Object>> banks() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAllBankDT"))) {
            if (!"Home Country".equals(str(r.get("IsHomeland")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("BranchName", str(r.get("BranchName")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> branches() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "GetAll"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("BranchName", str(r.get("BranchName")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> currencies() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAll"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("CurrencyCode", str(r.get("CurrencyCode")));
            out.add(m);
        }
        return out;
    }

    /** HistoryCombosFill: @Activity is not sent (the desktop's null Activity is skipped by DesktopProc). */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> bank = new ArrayList<>(), branch = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromFcyExRateForwardBooking", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("name", str(r.get("name")));
            String t = str(r.get("ActivityType"));
            if ("Bank".equals(t)) bank.add(m);
            else if ("Branches".equals(t)) branch.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyBanks", bank);
        out.put("historyBranches", branch);
        return out;
    }

    /** CommonServices.GetDecimalConfiguration (:5376-5470) - the three display formats this form uses. */
    private Map<String, Object> formats() {
        Map<String, Object> m = new LinkedHashMap<>();
        int amt = cfgInt("Default NoofDecimal Points For Amount");
        int rate = cfgInt("Default NoofDecimal Points For Rate");
        int fcy = cfgInt("DefaultNoOfDecimalPointsForFcyAmount");
        m.put("amountDecimals", (amt >= 1 && amt <= 4) ? amt : 0);          // stringFormatsingle
        m.put("rateDecimals", rate == 0 ? 2 : ((rate >= 1 && rate <= 4) ? rate : 0));   // DecimalRateFormate ("#,#0." + ...)
        m.put("fcyAmountDecimals", (fcy >= 1 && fcy <= 4) ? fcy : 0);      // stringFormatsingleForFcy
        return m;
    }

    private int cfgInt(String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return r.isEmpty() ? 0 : toInt(str(r.get(0).get("ConfigKey")).trim());
    }

    /** GenerateDocNo(): fcyExRateForwardBooking.GenerateCode. */
    public int generateCode() {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "USP_FcyExRateForwardBooking_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ------------------------------------------------------------------------------------ ReadById

    /** fcyExRateForwardBooking.GetById; the procedure has no tenancy filter, so it is checked here. */
    private Map<String, Object> header(int id) {
        List<Map<String, Object>> h = DesktopProc.rows(jdbc, "USP_FcyExRateForwardBooking_GetAllMethod", DesktopProc.params(
                "Id", id, "Activity", "ReadById"));
        if (h.isEmpty()) throw new IllegalArgumentException(OUT_OF_RANGE);
        Map<String, Object> row = h.get(0);
        if (toInt(row.get("OrganizationId")) != ctx.currentOrganizationId() || toInt(row.get("CompanyId")) != ctx.currentCompanyId())
            throw new IllegalArgumentException(OUT_OF_RANGE);
        return row;
    }

    /** ReadById(Id). See the class comment for the one departure. */
    public Map<String, Object> record(int id) {
        Map<String, Object> hd = header(id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("id", id);
        m.put("docNo", toInt(hd.get("DocNo")));
        m.put("bankId", toInt(hd.get("BankId")));
        m.put("contractRef", str(hd.get("BankContractNo")));
        m.put("docDate", isoDate(hd.get("DocDate")));
        m.put("branchId", toInt(hd.get("BranchId")));
        m.put("fcyId", toInt(hd.get("FcyId")));
        m.put("fcyAmount", plain(hd.get("FcyAmount")));
        m.put("bookingRate", plain(hd.get("BookingRate")));
        m.put("hcyId", toInt(hd.get("HcyId")));
        m.put("hcyAmount", plain(hd.get("HcyAmount")));
        m.put("fixedDays", toInt(hd.get("FixedDays")));
        m.put("fixedMaturity", isoDate(hd.get("FirstMaturityDate")));
        m.put("optionalDays", toInt(hd.get("OptionalDays")));
        m.put("startOptional", isoDate(hd.get("OptionalPeriod")));
        m.put("finalMaturity", isoDate(hd.get("FinalMaturityDate")));
        m.put("totalDays", toInt(hd.get("TotalDays")));
        m.put("attachments", attachments.list(id));
        return m;
    }

    // ------------------------------------------------------------------------------------ History

    /** GridFill :719-814. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        Map<String, Boolean> rt = rights();
        boolean viewAll = Boolean.TRUE.equals(rt.get("viewAll"));
        String by = str(b.get("dateBy"));                       // docdate | entrydate | modifydate (the three radio buttons)
        java.sql.Date from = sqlDate(b.get("fromDate")), to = sqlDate(b.get("toDate"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", ctx.currentOrganizationId());
        p.put("CompanyId", ctx.currentCompanyId());
        p.put("FinancialYearId", ctx.currentFinancialYearId());
        String fromKey = "FromDate", toKey = "ToDate";
        if ("entrydate".equals(by)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modifydate".equals(by)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        if (from != null) p.put(fromKey, from);
        if (to != null) p.put(toKey, to);
        int fromNo = toInt(str(b.get("fromDocNo")).trim()), toNo = toInt(str(b.get("toDocNo")).trim());
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        int bankId = toInt(b.get("bankId")), branchId = toInt(b.get("branchId"));
        if (bankId != 0) p.put("BankId", bankId);
        if (branchId != 0) p.put("BranchesId", branchId);
        if (!viewAll) p.put("EntryUserId", ctx.currentUserId());
        p.put("CanViewAllRecord", viewAll);
        p.put("Activity", "FormHistory");

        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_FcyExRateForwardBooking_GetAllMethod", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("DocNo", toInt(r.get("DocNo")));
            m.put("BankName", str(r.get("BankName")));
            m.put("ContractRefNo", str(r.get("BankContractNo")));
            m.put("DealDate", shortDate(r.get("DocDate")));
            m.put("Client", str(r.get("BranchName")));
            m.put("FcyCode", str(r.get("FcyCode")));
            m.put("FcyAmount", dbl(r.get("FcyAmount")));
            m.put("BookingRate", dbl(r.get("BookingRate")));
            m.put("HcyCode", str(r.get("HcyCode")));
            m.put("HcyAmount", dbl(r.get("HcyAmount")));
            m.put("FixedDays", toInt(r.get("FixedDays")));
            m.put("FixedMaturity", shortDate(r.get("FirstMaturityDate")));
            m.put("OptionalDays", toInt(r.get("OptionalDays")));
            m.put("StartOfOptianlPeriod", shortDate(r.get("OptionalPeriod")));    // the desktop's own spelling of the column
            m.put("FinalMaturity", shortDate(r.get("FinalMaturityDate")));
            m.put("TotalDays", toInt(r.get("TotalDays")));
            m.put("EntryDate", entryDate(r.get("EntryDate")));
            m.put("EntryUser", str(r.get("EntryUserName")));
            m.put("ModifyDate", entryDate(r.get("ModifyDate")));
            m.put("ModifyUser", str(r.get("ModifyUserName")));
            m.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------ Save / Update

    /** Insert() :584-654 (the Yes/No prompt is answered on the page first). */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = toInt(b.get("id"));
        Map<String, Boolean> rt = rights();
        if (recId > 0) {
            if (!Boolean.TRUE.equals(rt.get("update"))) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        } else if (!Boolean.TRUE.equals(rt.get("save"))) {
            throw new IllegalArgumentException("You do not have the Save right for this screen.");
        }

        /* calculateHcyAmount :1146 and CalculateTotalDays :1270 */
        int dec = ((Number) formats().get("amountDecimals")).intValue();
        double fcyAmountD = toDouble(b.get("fcyAmount"));
        double rateD = toDouble(b.get("bookingRate"));
        BigDecimal hcyAmount = (fcyAmountD > 0.0 && rateD > 0.0)
                ? BigDecimal.valueOf(fcyAmountD * rateD).setScale(dec, RoundingMode.HALF_UP) : BigDecimal.ZERO;
        int fixedDays = toInt(str(b.get("fixedDays")).trim()), optionalDays = toInt(str(b.get("optionalDays")).trim());
        int totalDays = (fixedDays > 0 || optionalDays > 0) ? fixedDays + optionalDays : 0;

        /* FormValidation :494-569 */
        String docNoText = str(b.get("docNo")).trim();
        if (docNoText.isEmpty() || toInt(docNoText) == 0) throw new IllegalArgumentException("DocNo Field Required");
        int bankId = toInt(b.get("bankId"));
        if (bankId == 0) throw new IllegalArgumentException("Bank Field Required");
        String contractRef = str(b.get("contractRef"));
        if (contractRef.trim().isEmpty()) throw new IllegalArgumentException("Contract Ref No Field Required");
        int branchId = toInt(b.get("branchId"));
        if (branchId == 0) throw new IllegalArgumentException("Client Field Required");
        int fcyId = toInt(b.get("fcyId"));
        if (fcyId == 0) throw new IllegalArgumentException("Fcy Code Field Required");
        if (fcyAmountD == 0.0) throw new IllegalArgumentException("Fcy Amount Field Required");
        if (rateD == 0.0) throw new IllegalArgumentException("Booking Rate Field Required");
        int hcyId = toInt(b.get("hcyId"));
        if (hcyId == 0) throw new IllegalArgumentException("Hcy Code Field Required");
        if (hcyAmount.signum() == 0) throw new IllegalArgumentException("Hcy Amount Field Required");
        if (fixedDays == 0) throw new IllegalArgumentException("Tenor Fixed Days Field Required");
        if (optionalDays == 0) throw new IllegalArgumentException("Tenor Optional Days Field Required");
        if (totalDays == 0) throw new IllegalArgumentException("Tenor Total Days Field Required");

        /* The ids must belong to the signed-in company's own combos. */
        if (banks().stream().noneMatch(x -> toInt(x.get("Id")) == bankId)) throw new IllegalArgumentException("Bank Field Required");
        if (branches().stream().noneMatch(x -> toInt(x.get("Id")) == branchId)) throw new IllegalArgumentException("Client Field Required");
        List<Map<String, Object>> cur = currencies();
        if (cur.stream().noneMatch(x -> toInt(x.get("Id")) == fcyId)) throw new IllegalArgumentException("Fcy Code Field Required");
        if (cur.stream().noneMatch(x -> toInt(x.get("Id")) == hcyId)) throw new IllegalArgumentException("Hcy Code Field Required");
        if (recId > 0) header(recId);

        LocalDate docDate = parseIso(str(b.get("docDate")));
        LocalDate firstMaturity = parseIso(str(b.get("fixedMaturity")));
        LocalDate startOptional = parseIso(str(b.get("startOptional")));
        LocalDate finalMaturity = parseIso(str(b.get("finalMaturity")));
        Timestamp now = new Timestamp(System.currentTimeMillis());
        int user = ctx.currentUserId();

        /* fcyExRateForwardBooking model: every non-virtual property is a parameter (GenericProvider.SetProc). */
        Map<String, Object> hp = new LinkedHashMap<>();
        hp.put("IsApproved", false);
        hp.put("DocDate", java.sql.Date.valueOf(docDate));
        hp.put("EntryDate", now);
        hp.put("ModifyDate", now);
        hp.put("FinalMaturityDate", Timestamp.valueOf(finalMaturity.atStartOfDay()));
        hp.put("FirstMaturityDate", Timestamp.valueOf(firstMaturity.atStartOfDay()));
        hp.put("OptionalPeriod", Timestamp.valueOf(startOptional.atStartOfDay()));
        hp.put("DateOfPurchase", Timestamp.valueOf(docDate.atStartOfDay()));
        hp.put("BookingRate", toDecimal(str(b.get("bookingRate"))));
        hp.put("FcyAmount", toDecimal(str(b.get("fcyAmount"))));
        hp.put("HcyAmount", hcyAmount);
        hp.put("ActionId", recId > 0 ? 2 : 1);                     // BLL Save: Id == 0 -> 1, else 2
        hp.put("ApprovedById", 0);
        hp.put("BankId", bankId);
        hp.put("BranchId", branchId);
        hp.put("CompanyId", ctx.currentCompanyId());
        hp.put("CustomerId", 0);                                    // never set by the form
        hp.put("DocNo", toInt(docNoText));
        hp.put("EntryUserId", user);
        hp.put("FcyId", fcyId);
        hp.put("FinancialYearId", ctx.currentFinancialYearId());
        hp.put("FixedDays", fixedDays);
        hp.put("HcyId", hcyId);
        hp.put("Id", recId);
        hp.put("ModifyUserId", user);
        hp.put("OptionalDays", optionalDays);
        hp.put("OrganizationId", ctx.currentOrganizationId());
        hp.put("TotalDays", totalDays);
        hp.put("BankContractNo", contractRef);
        hp.put("ScreenName", SCREEN_NAME);

        Object atts = b.get("attachments");
        Object files = null, removeIds = null;
        if (atts instanceof Map) { files = ((Map<?, ?>) atts).get("files"); removeIds = ((Map<?, ?>) atts).get("removeIds"); }
        W4ExchangeRateForwardBookingAttachmentService.Prepared prepared = attachments.prepare(recId, files, removeIds);

        int num = DesktopProc.setProc(jdbc, "[dbo].[USP_FcyExRateForwardBooking_InsertAndUpdate]", hp);
        if (num <= 0) num = recId;
        attachments.persist(num, bankId, recId > 0, prepared);

        int savedDocNo = toInt(docNoText);
        if (recId == 0) {
            /* the insert branch of the procedure takes DocNo = Max + 1 itself; read back what was stored */
            try { savedDocNo = toInt(header(num).get("DocNo")); } catch (Exception ignored) { /* keep the posted number */ }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", num);
        out.put("docNo", savedDocNo);
        out.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + savedDocNo);
        return out;
    }

    // ------------------------------------------------------------------------------------ helpers

    private static LocalDate parseIso(String s) {
        try { return LocalDate.parse(s.trim().substring(0, 10)); }
        catch (Exception e) { return LocalDate.now(); }
    }

    private static java.sql.Date sqlDate(Object o) {
        if (o == null) return null;
        try {
            LocalDate d = LocalDate.parse(String.valueOf(o).trim().substring(0, 10));
            if (d.getYear() <= 1900) return null;                    /* Conversion.CheckDateTimeNull */
            return java.sql.Date.valueOf(d);
        } catch (Exception e) { return null; }
    }

    private static LocalDateTime toDateTime(Object o) {
        if (o == null) return null;
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().atStartOfDay();
        if (o instanceof java.util.Date) return new Timestamp(((java.util.Date) o).getTime()).toLocalDateTime();
        if (o instanceof LocalDateTime) return (LocalDateTime) o;
        if (o instanceof LocalDate) return ((LocalDate) o).atStartOfDay();
        try { return LocalDate.parse(String.valueOf(o).substring(0, 10)).atStartOfDay(); } catch (Exception e) { return null; }
    }

    private static String isoDate(Object o) {
        LocalDateTime d = toDateTime(o);
        return d == null ? "" : d.toLocalDate().toString();
    }

    private static String shortDate(Object o) {
        LocalDateTime d = toDateTime(o);
        return d == null ? LocalDate.of(1, 1, 1).format(SHORT_DATE) : d.format(SHORT_DATE);
    }

    private static String entryDate(Object o) {
        LocalDateTime d = toDateTime(o);
        return d == null ? "" : d.format(ENTRY_DATE);
    }

    private static String plain(Object o) {
        if (o instanceof BigDecimal) return ((BigDecimal) o).stripTrailingZeros().toPlainString();
        return str(o);
    }

    private static BigDecimal toDecimal(String s) {
        try { return new BigDecimal(s.trim().replace(",", "")); } catch (Exception e) { return BigDecimal.ZERO; }
    }

    private static double toDouble(Object o) {
        if (o instanceof Number) { double d = ((Number) o).doubleValue(); return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d; }
        if (o == null) return 0.0;
        try {
            double d = Double.parseDouble(String.valueOf(o).trim().replace(",", ""));
            return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d;
        } catch (NumberFormatException e) { return 0.0; }
    }

    private static double dbl(Object o) { return o instanceof Number ? ((Number) o).doubleValue() : 0.0; }

    /** Conversion.ToString */
    private static String str(Object o) {
        if (o == null) return "";
        if (o instanceof Boolean) return ((Boolean) o) ? "True" : "False";
        return String.valueOf(o);
    }

    /** Conversion.ToInt */
    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
