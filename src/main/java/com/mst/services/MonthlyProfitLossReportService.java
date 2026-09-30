package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.*;

import static com.mst.services.AccountReportsHSupport.params;

/**
 * Screen 958 "Monthly Profit Loss" (ScreenName/TargetUrl frmProfitLossMonthWiseComparison).
 *
 *   Months   InitializeComponentMethod -> GlobalVariables_Helper.GetMonthListByAllFinancialYEarFromGlobal:
 *            every month from clsGlobalVariables.FirstFinancialStartDate to RunningFinancialEndDate,
 *            which DashboardNew.SetFinancialDates fills from FinancialYear.getFinancialStartAndEndDate
 *            -> usp_getFinancialStartAndEndDate @CompanyId. Id and text are both "MMM-yy".
 *   Show     GridFill -> VoucherReports.GetMonthWiseProfitLoss (BLL 0141) -> usp_getMonthwiseProfitLoss
 *            @CompanyId, @FromDate (1st of From Month), @ToDate (last day of To Month),
 *            @ActionId = 1 only when "Document Type Wise" is ticked (0 is not sent).
 *            Tables[0] is returned with its columns in order; Tables[1] (the logo) is not needed.
 */
@Service
public class MonthlyProfitLossReportService {

    @Autowired
    private AccountReportsHSupport h;

    private static final DateTimeFormatter MMM_YY = DateTimeFormatter.ofPattern("MMM-yy", Locale.ENGLISH);

    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> months = new ArrayList<>();
        List<Map<String, Object>> r = h.rows("usp_getFinancialStartAndEndDate", params("CompanyId", h.comp()));
        if (!r.isEmpty()) {
            Object s = AccountReportsHSupport.col(r.get(0), "Start_Period");
            Object e = AccountReportsHSupport.col(r.get(0), "End_Period");
            if (s != null && e != null) {
                LocalDate d = LocalDate.parse(String.valueOf(s).substring(0, 10));
                LocalDate end = LocalDate.parse(String.valueOf(e).substring(0, 10));
                while (!d.isAfter(end)) { months.add(d.format(MMM_YY)); d = d.plusMonths(1); }
            }
        }
        out.put("months", months);
        out.put("amountDecimals", h.amountDecimals());
        out.put("financialYearStart", h.financialYearStart());
        return out;
    }

    public Map<String, Object> show(Map<String, Object> body) {
        String fromMonth = body.get("fromMonth") == null ? "" : String.valueOf(body.get("fromMonth")).trim();
        String toMonth = body.get("toMonth") == null ? "" : String.valueOf(body.get("toMonth")).trim();
        if (fromMonth.isEmpty()) throw new AccountReportsHSupport.Refusal("From Month Required");
        if (toMonth.isEmpty()) throw new AccountReportsHSupport.Refusal("To Month Required");
        LocalDate from = parseMonth(fromMonth);
        LocalDate to = parseMonth(toMonth).plusMonths(1).minusDays(1);
        boolean docWise = Boolean.TRUE.equals(body.get("docWise"));
        AccountReportsHSupport.Table t = h.rowsWithColumns("usp_getMonthwiseProfitLoss", params(
                "CompanyId", h.comp(),
                "FromDate", java.sql.Date.valueOf(from),
                "ToDate", java.sql.Date.valueOf(to),
                "ActionId", docWise ? 1 : null));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("columns", t.columns);
        out.put("rows", t.rows);
        out.put("fromDate", from.toString());
        out.put("toDate", to.toString());
        return out;
    }

    /** DateTime.ParseExact(value, "MMM-yy", InvariantCulture) - day 1. */
    private static LocalDate parseMonth(String s) {
        try {
            String[] p = s.split("-");
            int m = Arrays.asList("jan","feb","mar","apr","may","jun","jul","aug","sep","oct","nov","dec").indexOf(p[0].toLowerCase(Locale.ROOT)) + 1;
            int yy = Integer.parseInt(p[1]);
            /* .NET two-digit year: TwoDigitYearMax 2049 */
            int y = yy <= 49 ? 2000 + yy : 1900 + yy;
            if (m < 1 || p[1].length() != 2) throw new IllegalArgumentException();
            return LocalDate.of(y, m, 1);
        } catch (Exception e) {
            throw new AccountReportsHSupport.Refusal("String was not recognized as a valid DateTime.");
        }
    }
}
