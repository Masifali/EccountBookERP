package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.pcc.SalePccInvoiceReturnService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.util.*;

/**
 * 553 SaleInvoiceReturnConcrete prints. Desktop: CommonServices.SaleInvoiceReturnConcreteCustomerSlip(id) = 1862-SaleInvoiceRetrurn_Slip.rpt
 * (InvSaleInvoice.SaleInvoiceDirectSlip + SaleInvoice_SubReport.rpt from SalesCustomerBillSubReport) and
 * CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(id, 1862), 1862) = 103-AcRptPurchaseSalesVoucherSlip.rpt,
 * both with @CompanyName / @CompanyAddress, through the shared Jasper pipeline.
 */
@Controller
@RequestMapping("/sale/pcc/print")
public class SalePccInvoiceReturnPrintController extends ReportPrintSupport {
    private final SalePccInvoiceReturnService service;
    private final ICompanyRepository companies;

    public SalePccInvoiceReturnPrintController(SalePccInvoiceReturnService service, ICompanyRepository companies) {
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

    /** btnSlip_Click / grdHistory "Print" (Customer Slip) -> 1862. */
    @GetMapping("/sale-invoice-return-slip")
    public void slip(HttpServletResponse response, @RequestParam int id) throws Exception {
        Map<String, Object> s = service.slip(id);
        @SuppressWarnings("unchecked") List<Map<String, Object>> rows = (List<Map<String, Object>>) s.get("rows");
        @SuppressWarnings("unchecked") List<Map<String, Object>> sub = (List<Map<String, Object>>) s.get("sub");
        Map<String, Object> out = result(rows);
        out.put("subReports", List.of(Map.of("template", "SaleInvoice_SubReport.rpt", "rows", sub)));
        printReportData(response, "1862-SaleInvoiceRetrurn_Slip.rpt", out);
    }

    /** btnPrint_Click / grdHistory "Voucher" -> 103. */
    @GetMapping("/sale-invoice-return-voucher")
    public void voucher(HttpServletResponse response, @RequestParam int id) throws Exception {
        printReportData(response, "103-AcRptPurchaseSalesVoucherSlip.rpt", result(service.voucherSlip(id)));
    }
}
