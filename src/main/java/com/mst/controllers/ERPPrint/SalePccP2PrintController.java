package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.pcc.SalePccGdnConcreteService;
import com.mst.services.sale.pcc.SalePccGdnDirectConcreteService;
import com.mst.services.sale.pcc.SalePccSaleOrderService;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/** 548 SaleOrderConcrete print: 1852-InvRptSaleOrderSlip.rpt fed by [pcc].[USP_SaleOrderSlipAndRegister], @CompanyName / @CompanyAddress. */
@Controller
@RequestMapping("/sale/pcc/print")
public class SalePccP2PrintController extends ReportPrintSupport {
    private final SalePccSaleOrderService saleOrder;
    private final SalePccGdnConcreteService gdn;
    private final SalePccGdnDirectConcreteService gdnDirect;
    private final ICompanyRepository companies;

    public SalePccP2PrintController(SalePccSaleOrderService saleOrder, SalePccGdnConcreteService gdn, SalePccGdnDirectConcreteService gdnDirect, ICompanyRepository companies) {
        this.saleOrder = saleOrder;
        this.gdn = gdn;
        this.gdnDirect = gdnDirect;
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

    @GetMapping("/sale-order-slip")
    public void saleOrderSlip(HttpServletResponse response, @RequestParam int id) throws Exception {
        printReportData(response, "1852-InvRptSaleOrderSlip.rpt", result(saleOrder.slipRows(id)));
    }

    /** 551 GenerateCustomerSlip: 1855-InvGdn_Slip.rpt (USP_InvGdn_Slip) with the InvGdn_SubReport.rpt sub report (USP_InvGdnWagesDetail_SubReport). */
    @GetMapping("/gdn-concrete-slip")
    public void gdnConcreteSlip(HttpServletResponse response, @RequestParam int id) throws Exception {
        Map<String, Object> report = result(gdn.slipRows(id));
        report.put("subReports", List.of(Map.of("template", "InvGdn_SubReport.rpt", "rows", gdn.slipWages(id))));
        printReportData(response, "1855-InvGdn_Slip.rpt", report);
    }

    /** 552 GenerateCustomerSlip: 1866-InvGdnDirect_Slip.rpt (USP_InvGdn_Slip) with the InvGdn_SubReport.rpt sub report (USP_InvGdnWagesDetail_SubReport). */
    @GetMapping("/gdn-direct-slip")
    public void gdnDirectSlip(HttpServletResponse response, @RequestParam int id) throws Exception {
        Map<String, Object> report = result(gdnDirect.slipRows(id));
        report.put("subReports", List.of(Map.of("template", "InvGdn_SubReport.rpt", "rows", gdnDirect.slipWages(id))));
        printReportData(response, "1866-InvGdnDirect_Slip.rpt", report);
    }
}
