package com.mst.controllers.sale.steel;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Pages of module 87 "Sales Steel Reports" (Architecture.WinApp.Steel.Reports.SalesReports.*). */
@Controller
public class SaleSteelReportViewController {

    private String page(Model model, String title, String template) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", title);
        return template;
    }

    /** 557 SaleInvoiceRegisterSteel */
    @GetMapping("/sale/reports/steel/sale-invoice-register")
    public String saleInvoiceRegister(Model model) { return page(model, "Sale Invoice Register (Steel)", "sale/steel/rpt_sale_invoice_register_st"); }

    /** 558 SaleInvoiceRegisterWithActivities */
    @GetMapping("/sale/reports/steel/sale-invoice-register-with-activities")
    public String saleInvoiceRegisterWithActivities(Model model) { return page(model, "Sales Register", "sale/steel/rpt_sale_invoice_register_activities_st"); }

    /** 559 frmGPOutwardRegister */
    @GetMapping("/sale/reports/steel/gp-outward-register")
    public String gpOutwardRegister(Model model) { return page(model, "GatePass Outward History", "sale/steel/rpt_gp_outward_register_st"); }

    /** 560 frmSaleOrderSlipAndRegister */
    @GetMapping("/sale/reports/steel/sale-order-slip-register")
    public String saleOrderSlipRegister(Model model) { return page(model, "Sale Order Slip and Register", "sale/steel/rpt_sale_order_slip_register_st"); }
}
