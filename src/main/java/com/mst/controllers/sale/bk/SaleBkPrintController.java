package com.mst.controllers.sale.bk;

import com.mst.controllers.ERPPrint.ReportPrintSupport;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.bk.SaleBkBookingRegisterService;
import com.mst.services.sale.bk.SaleBkSaleRegisterService;
import org.springframework.beans.factory.annotation.Value;
import java.io.File;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.util.*;

/**
 * Prints of the Booking Office / Customer Portal screens (key B of the Sale hub port). Like the desktop (Reporting.ShowReportWithDataTable) the
 * Crystal template gets the procedure's rows plus @CompanyName / @CompanyAddress; the web re-runs the same procedure with the same arguments
 * and renders the template through the shared Jasper pipeline (ReportPrintSupport.printReportData).
 *   765 btnPrint            -> 274_1-PreBookinRegister.rpt     over the last Show's rows
 *   765 row Print / 761     -> 274_PreBookingOrder_Slip.rpt    (CommonServices.PreBookingSlip(Id))
 *   586 Print drop-down     -> a *.rpt of the Sales_Salt folder over the last Show's rows of the detail tab
 *   586 summary Print       -> the 1809_* template of the activity over the last Show's rows of the summary tab
 */
@Controller
@RequestMapping("/sale/bk/print")
public class SaleBkPrintController extends ReportPrintSupport {
    private final ICompanyRepository companies;
    private final SaleBkBookingRegisterService booking;
    private final SaleBkSaleRegisterService saleRegister;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    private static final Set<String> SUMMARY_TEMPLATES = new LinkedHashSet<>(Arrays.asList(
            "1809_01_SalesRegisterSummary", "1809_02-SalesRegisterSummaryByCustomer", "1809_03-SalesRegisterSummaryByItem", "1809_04-SalesRegisterSummaryByCustomer&Item",
            "1809_05-SalesRegisterSummaryByItemWithoutPacking", "1809_06-SalesRegisterSummaryByWarehouse", "1809_15-SalesSummaryByItem&City", "1809_14-SalesSummaryByItemPackSize&City",
            "1809_07-SalesSummaryByCustomer&City", "1809_09-SalesSummaryByCustomerItem&City", "1809_10-SalesSummaryByCustomer&PackSize", "1809_11-SalesSummaryByParentCategory",
            "1809_12-SalesSummaryByParentCategory&Item", "1809_13-SalesSummaryByParentCategory&Customer"));

    public SaleBkPrintController(ICompanyRepository companies, SaleBkBookingRegisterService booking, SaleBkSaleRegisterService saleRegister) {
        this.companies = companies; this.booking = booking; this.saleRegister = saleRegister;
    }

    private Map<String, Object> result(List<Map<String, Object>> rows) {
        String name = "", address = "";
        try {
            com.mst.models.Company c = companies.findById(currentUserContext.currentCompanyId()).orElse(null);
            if (c != null) { name = c.getCompName() == null ? "" : c.getCompName(); address = c.getCompAddress() == null ? "" : c.getCompAddress(); }
        } catch (Exception ignored) { /* a missing header is cosmetic */ }
        Map<String, Object> params = new LinkedHashMap<>();
        params.put("@CompanyName", name);
        params.put("@CompanyAddress", address);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("reportParameters", params);
        return out;
    }

    @GetMapping("/prebooking-register")
    public void bookingRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "274_1-PreBookinRegister.rpt", result(booking.raw(q)));
    }

    @GetMapping("/prebooking-slip")
    public void bookingSlip(HttpServletResponse response, @RequestParam int id) throws Exception {
        // Id 0 -> "No Record Found For Display" (the pipeline answers 404 with that text for an empty row list)
        printReportData(response, "274_PreBookingOrder_Slip.rpt", result(id == 0 ? new ArrayList<>() : booking.slip(id)));
    }

    /** tsDropDown_DropDownItemClicked: the clicked file of the Sales_Salt folder prints the last Show's table (no rows -> "Record Not Found For Display"). */
    @GetMapping("/sale-register-salt")
    public void saleRegisterSalt(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String name = q.get("template");
        if (name == null || name.trim().isEmpty() || name.matches(".*[\\\\/:*?\"<>|].*") || name.contains("..")) throw new IllegalArgumentException("Invalid report name.");
        String found = null;
        if (reportRoot != null && !reportRoot.trim().isEmpty()) {
            File[] files = new File(reportRoot, "Sales_Salt").listFiles();
            if (files != null) for (File f : files) {
                String n = f.getName();
                if (!f.isFile() || !n.toLowerCase(Locale.ROOT).endsWith("rpt")) continue;
                int dot = n.lastIndexOf('.');
                if ((dot > 0 ? n.substring(0, dot) : n).equals(name)) found = name + ".rpt";
            }
        }
        if (found == null) throw new IllegalArgumentException("Report " + name + " is not in the Sales_Salt folder");
        printReportData(response, found, result(saleRegister.raw(q)));
    }

    /** btnPrint_Click of the summary tab: the template named by the Print button text over the last Show's dtSummary. */
    @GetMapping("/sale-summary-salt")
    public void saleSummarySalt(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String t = q.get("template");
        if (t == null || !SUMMARY_TEMPLATES.contains(t)) throw new IllegalArgumentException("Invalid report name.");
        printReportData(response, t + ".rpt", result(saleRegister.summaryRaw(q)));
    }
}
