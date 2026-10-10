package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.pcc.SalePccInvoiceDirectService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.util.*;

/**
 * 546 SaleInvoiceDirectConcrete prints. Desktop: CommonServices.SaleInvoiceDirectConcreteCustomerSlip(id) = 1856-SaleInvoiceDirect_Slip.rpt,
 * CommonServices.SaleInvoiceDirectConcreteItemSlip(id) = 1856A-SaleInvoiceDirect_Slip.rpt (both: InvSaleInvoice.SaleInvoiceSlip rows + a sub report) and
 * CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(id, 1856), 1856) = 103-AcRptPurchaseSalesVoucherSlip.rpt,
 * all with @CompanyName / @CompanyAddress, through the shared Jasper pipeline.
 */
@Controller
@RequestMapping("/sale/pcc/print")
public class SalePccInvoiceDirectPrintController extends ReportPrintSupport {
    private final SalePccInvoiceDirectService service;
    private final ICompanyRepository companies;

    public SalePccInvoiceDirectPrintController(SalePccInvoiceDirectService service, ICompanyRepository companies) {
        this.service = service;
        this.companies = companies;
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

    private void slip(HttpServletResponse response, int id, boolean item) throws Exception {
        Map<String, Object> s = service.slip(id, item);
        @SuppressWarnings("unchecked") List<Map<String, Object>> rows = (List<Map<String, Object>>) s.get("rows");
        @SuppressWarnings("unchecked") List<Map<String, Object>> sub = (List<Map<String, Object>>) s.get("sub");
        Map<String, Object> out = result(rows);
        out.put("subReports", List.of(Map.of("template", item ? "SaleInvoice_ItemExpenseSubReport.rpt" : "SaleInvoice_SubReport.rpt", "rows", sub)));
        printReportData(response, item ? "1856A-SaleInvoiceDirect_Slip.rpt" : "1856-SaleInvoiceDirect_Slip.rpt", out);
    }

    /** btnPartySlip / btnSlip_Click / grdHistory "Customer Slip" -> 1856. */
    @GetMapping("/sale-invoice-direct-slip")
    public void customerSlip(HttpServletResponse response, @RequestParam int id) throws Exception { slip(response, id, false); }

    /** btnItemPrint / btn294APrint_Click / grdHistory "Item Slip" -> 1856A. */
    @GetMapping("/sale-invoice-direct-item-slip")
    public void itemSlip(HttpServletResponse response, @RequestParam int id) throws Exception { slip(response, id, true); }

    /** btnPrint_Click / grdHistory "Voucher" -> 103. */
    @GetMapping("/sale-invoice-direct-voucher")
    public void voucher(HttpServletResponse response, @RequestParam int id) throws Exception {
        printReportData(response, "103-AcRptPurchaseSalesVoucherSlip.rpt", result(service.voucherSlip(id)));
    }
}
