package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.pcc.SalePccDeliveryOrderService;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * 549 DeliveryOrderConcrete prints. Desktop: CommonServices.DeliveryOrderConcreteSlip(id) = 1853-DeliveryOrderSlip.rpt and
 * DeliveryOrderConcreteSlipCustomerWise(id) = 1853A-DeliveryOrderSlip.rpt, both fed by InvDeliveryOrder.DeliveryOrder_SlipConcrete
 * with @CompanyName / @CompanyAddress. Same data, same templates, through the shared Jasper pipeline.
 */
@Controller
@RequestMapping("/sale/pcc/print")
public class SalePccDeliveryOrderPrintController extends ReportPrintSupport {
    private final SalePccDeliveryOrderService service;
    private final ICompanyRepository companies;

    public SalePccDeliveryOrderPrintController(SalePccDeliveryOrderService service, ICompanyRepository companies) {
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

    /** btnprint_Click / grdhistory "Print" -> 1853 ; btnPrintCustomerWise_Click / grdhistory "PrintII" -> 1853A (kind=customer). */
    @GetMapping("/delivery-order-slip")
    public void slip(HttpServletResponse response, @RequestParam int id, @RequestParam(required = false) String kind) throws Exception {
        List<Map<String, Object>> rows = service.slipRows(id);
        printReportData(response, "customer".equals(kind) ? "1853A-DeliveryOrderSlip.rpt" : "1853-DeliveryOrderSlip.rpt", result(rows));
    }
}
