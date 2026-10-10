package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.pcc.SalePccInvoiceGdnService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.util.*;

/**
 * 547 SaleInvoiceAgainstGDNConcrete prints. Desktop: CommonServices.SaleInvoiceConcreteCustomerSlip(id) = 1861-SaleInvoice_Slip.rpt,
 * CommonServices.SaleInvoiceConcreteItemSlip(id) = 1861A-SaleInvoice_Slip.rpt (both: InvSaleInvoice.SaleInvoiceSlip rows + a sub report) and
 * CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(id, 1861), 1861) = 103-AcRptPurchaseSalesVoucherSlip.rpt,
 * all with @CompanyName / @CompanyAddress, through the shared Jasper pipeline.
 */
@Controller
@RequestMapping("/sale/pcc/print")
public class SalePccInvoiceGdnPrintController extends ReportPrintSupport {
    private final SalePccInvoiceGdnService service;
    private final ICompanyRepository companies;

    public SalePccInvoiceGdnPrintController(SalePccInvoiceGdnService service, ICompanyRepository companies) {
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
        printReportData(response, item ? "1861A-SaleInvoice_Slip.rpt" : "1861-SaleInvoice_Slip.rpt", out);
    }

    /** btnPartySlip / btnSlip_Click / grdHistory "Customer Slip" -> 1861. */
    @GetMapping("/sale-invoice-against-gdn-slip")
    public void customerSlip(HttpServletResponse response, @RequestParam int id) throws Exception { slip(response, id, false); }

    /** btnItemPrint / btn294APrint_Click / grdHistory "Item Slip" -> 1861A. */
    @GetMapping("/sale-invoice-against-gdn-item-slip")
    public void itemSlip(HttpServletResponse response, @RequestParam int id) throws Exception { slip(response, id, true); }

    /** btnPrint_Click / grdHistory "Voucher" -> 103. */
    @GetMapping("/sale-invoice-against-gdn-voucher")
    public void voucher(HttpServletResponse response, @RequestParam int id) throws Exception {
        printReportData(response, "103-AcRptPurchaseSalesVoucherSlip.rpt", result(service.voucherSlip(id)));
    }
}
