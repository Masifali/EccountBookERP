package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** Group E2: the Sale Engr report pages. Each is a new template; no existing route or template is touched. */
@Controller
public class SaEngrRptPageController {
    /** 555 frmGDNHistory_Engr = Architecture.WinApp.Inventory_Reports.frmGDNHistory_Engr ("Good Dispatch Notes" history). */
    @GetMapping("/sale/reports/engr/gdn-history")
    public String gdnHistory() { return "sale/reports/saengr_gdn_history"; }

    /** 554 frmSaleOrderHistory_Engr = Architecture.WinApp.Inventory_Reports.frmSaleOrderHistory_Engr ("Sale Order Report"). */
    @GetMapping("/sale/reports/engr/sale-order-history")
    public String saleOrderHistory() { return "sale/reports/saengr_sale_order_history"; }

    /** 556 SaleReportWithActivities_Engr = Architecture.WinApp.Inventory_Reports.SaleReportWithActivities_Engr ("Sales Register"). */
    @GetMapping("/sale/reports/engr/sale-report-with-activities")
    public String saleReportWithActivities() { return "sale/reports/saengr_sale_report_activities"; }

    /** 848 frmSaleOrderHistory_Engr = Architecture.WinApp.Mfg.Reports.frmSaleOrderHistory_Engr (module 135, "Sale Order Report" for document type 1656). */
    @GetMapping("/sale/reports/engr/mfg-sale-order-history")
    public String mfgSaleOrderHistory() { return "sale/reports/saengr_mfg_sale_order_history"; }
}
