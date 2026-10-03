package com.mst.services.banking;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.mst.reports.prints.ReportPdfService;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 700 "Cheque Printing" - desktop form Architecture.WinApp.ChequePrinting.ChequePrinting
 * (base.Name "ChequePrinting"), BLL Architecture.BLL.ChequePrinting.{ChequeCount, ChequePrintListCards,
 * ChequePrintList}, print in CommonServices.PrintSelectedCheque / UpdateChequePrintStatus.
 *
 * <p>The form does not enter cheques. The cheques are rows of dbo.ChequePrintList that the voucher saves
 * write (dbo.Sp_VoucherDetail_Insert and bank.USP_pdcTransaction_InsertAndUpdate insert/update the row,
 * with AmountInWords = dbo.fnNumberToWords(amount) and the date split into D1 D2 M1 M2 Y1..Y4). This
 * screen lists the unprinted ones (PrintStatus = 0) per bank, prints the selected ones with the bank's
 * cheque .rpt (Bank.ChequeTemplete + ".rpt") and marks them printed (PrintStatus = 1).</p>
 *
 * <ul>
 * <li>ChequePrinting_Load: BankFill() (cmbBanks - Visible=false, never shown) and ChequeCountGridFill():
 *     ChequeCount.ReadAll -> [dbo].[sp_ChequePrinting_GetChequeCounts] @OrganizationId, @CompanyId
 *     (BranchName, Id, Counts of unprinted cheques per bank). Grid columns Id (hidden), BranchName (250), Counts.
 *     When the proc returns no row the grid is NOT refreshed (desktop: "if (dtChequeCount.Rows.Count &gt; 0)").</li>
 * <li>grdChequeCount_DoubleClick -> GetDetailData(BankId, 0): ChequePrintListCards.getChequePrintListCards ->
 *     [dbo].[sp_ChequePrintList] @BankId, @CompanyId, @OrganizationId. The first row's ChequeTemplete picks
 *     the card user control (UBL, BankAlFalah, ABL, AskariBank, BankAlHabib, FaysalBank, HabibMetro, HBL, MCB,
 *     MeezanBank, NBP, NIB, SaadiqStandardCharteredBank, SilkBank, SoneriBank), otherwise "Template Not Available".</li>
 * <li>btn294APrint "Print Selected" / card "Print" -> PrintCheque(): ids = string.Join(",", selected);
 *     PrintSelectedCheque(ids, ChequeTemplate): ("Id or Template Not Found" when either is empty)
 *     ChequePrintList.DataForPrnt -> [dbo].[sp_ChequePrintList_Print] @Ids; no row -> "Not Record Found For Print"
 *     (no exception, so the status update still runs); else Reporting.ReportDirectPrint(dt, Template + ".rpt").
 *     Then UpdateChequePrintStatus(ids) -> [dbo].[sp_ChequePrintList_UpdateStatus] @Ids (own transaction).</li>
 * </ul>
 * The desktop checks no user right on this form. The static VoucherId (Shown -> getChequePrintListCardsByVoucherId)
 * is never assigned by any caller (the payment vouchers open the form with "new ChequePrinting(UserAccount)"),
 * so that path is not reachable and is not exposed here.
 */
@Service
public class ChequePrintingDesktopService {

    public static final String SCREEN_NAME = "ChequePrinting";

    /** The templates GetDetailData knows (one card user control each). */
    public static final List<String> TEMPLATES = Collections.unmodifiableList(Arrays.asList(
            "UBL", "BankAlFalah", "ABL", "AskariBank", "BankAlHabib", "FaysalBank", "HabibMetro", "HBL", "MCB",
            "MeezanBank", "NBP", "NIB", "SaadiqStandardCharteredBank", "SilkBank", "SoneriBank"));

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final ReportPdfService reportPdf;

    public ChequePrintingDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, ReportPdfService reportPdf) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.reportPdf = reportPdf;
    }

    /** ChequeCountGridFill - grdChequeCount rows (Id, BranchName, Counts). */
    public List<Map<String, Object>> chequeCounts() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "[dbo].[sp_ChequePrinting_GetChequeCounts]", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId()));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("BranchName", r.get("BranchName"));
            m.put("Counts", r.get("Counts"));
            out.add(m);
        }
        return out;
    }

    /** GetDetailData(BankId, 0) - the cards of one bank, with the template the first row names. */
    public Map<String, Object> cards(int bankId) {
        List<Map<String, Object>> rows = listRows(bankId);
        Map<String, Object> res = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> c = new LinkedHashMap<>();
            c.put("Id", toInt(r.get("Id")));                                   // Conversion.ToInt(Module["Id"].ToString())
            c.put("ChequeTypeId", toInt(r.get("ChequeTypeId")));
            c.put("ChequeNumber", str(r.get("ChequeNumber")));
            c.put("CheqType", str(r.get("CheqType")));
            c.put("PayeeTitle", str(r.get("PayeeTitle")));
            c.put("AmountInWords", str(r.get("AmountInWords")));
            c.put("Amount", clrAmount(r.get("Amount")));                       // ToDouble(..).ToString("##,#.####")
            for (String k : new String[] { "D1", "D2", "M1", "M2", "Y1", "Y2", "Y3", "Y4" }) c.put(k, str(r.get(k)));
            cards.add(c);
        }
        res.put("chequeTemplate", rows.isEmpty() ? null : str(rows.get(0).get("ChequeTemplete")));
        res.put("cards", cards);
        return res;
    }

    /** Result of PrintCheque(): a PDF, or the desktop's message when no row came back for the ids. */
    public static final class PrintResult {
        public final byte[] pdf;
        public final String message;
        PrintResult(byte[] pdf, String message) { this.pdf = pdf; this.message = message; }
    }

    /**
     * PrintCheque() for the cheques of one bank: PrintSelectedCheque(ids, ChequeTemplate) then
     * UpdateChequePrintStatus(ids). The ids must be cheques listed for that bank in the current company
     * (sp_ChequePrintList), and the template is the first listed row's ChequeTemplete, as on the desktop.
     */
    public PrintResult print(int bankId, List<Integer> selected) throws Exception {
        if (selected == null || selected.isEmpty()) throw new IllegalArgumentException("Please Select Cheque to print");
        List<Map<String, Object>> listed = listRows(bankId);
        Set<Integer> allowed = new LinkedHashSet<>();
        for (Map<String, Object> r : listed) allowed.add(toInt(r.get("Id")));
        List<String> parts = new ArrayList<>();
        for (Integer id : selected) {
            if (id == null || !allowed.contains(id)) throw new IllegalArgumentException("Id or Template Not Found");
            parts.add(String.valueOf(id));
        }
        String ids = String.join(",", parts);                                   // string.Join(",", SelectedChequesList)
        String template = listed.isEmpty() ? "" : str(listed.get(0).get("ChequeTemplete"));
        if (ids.isEmpty() || template.isEmpty()) throw new IllegalArgumentException("Id or Template Not Found");

        List<Map<String, Object>> dt = DesktopProc.rows(jdbc, "[dbo].[sp_ChequePrintList_Print]", DesktopProc.params("Ids", ids));
        PrintResult result;
        if (dt.isEmpty()) {
            result = new PrintResult(null, "Not Record Found For Print");
        } else {
            /* Reporting.ReportDirectPrint(dt, Template + ".rpt", null, null) - through the project's Jasper layer. */
            result = new PrintResult(reportPdf.rowsPdf(template + ".rpt", dt, null), null);
        }
        /* CommonServices.UpdateChequePrintStatus(ids) -> DAL: ExecuteScalar in its own transaction. */
        DesktopProc.scalar(jdbc, "[dbo].[sp_ChequePrintList_UpdateStatus]", DesktopProc.params("Ids", ids));
        return result;
    }

    private List<Map<String, Object>> listRows(int bankId) {
        return DesktopProc.rows(jdbc, "[dbo].[sp_ChequePrintList]", DesktopProc.params(
                "BankId", bankId,
                "CompanyId", ctx.currentCompanyId(),
                "OrganizationId", ctx.currentOrganizationId()));
    }

    /* ------------------------------------------------------------------------------------------ helpers */

    private static String str(Object v) {
        return v == null ? "" : String.valueOf(v);
    }

    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /**
     * .NET {@code double.ToString("##,#.####")}: group separators, at most four decimals (rounded away from
     * zero), no trailing zeros, no mandatory digit - so 0 is "" and 0.5 is ".5".
     */
    static String clrAmount(Object v) {
        BigDecimal d;
        try { d = v == null ? BigDecimal.ZERO : new BigDecimal(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { d = BigDecimal.ZERO; }
        d = d.setScale(4, RoundingMode.HALF_UP).stripTrailingZeros();
        if (d.signum() == 0) return "";
        boolean neg = d.signum() < 0;
        d = d.abs();
        String plain = d.toPlainString();
        int dot = plain.indexOf('.');
        String intPart = dot < 0 ? plain : plain.substring(0, dot);
        String frac = dot < 0 ? "" : plain.substring(dot + 1);
        StringBuilder g = new StringBuilder();
        if (!"0".equals(intPart)) {
            int n = intPart.length();
            for (int i = 0; i < n; i++) {
                g.append(intPart.charAt(i));
                int left = n - 1 - i;
                if (left > 0 && left % 3 == 0) g.append(',');
            }
        }
        if (!frac.isEmpty()) g.append('.').append(frac);
        return (neg ? "-" : "") + g;
    }
}
