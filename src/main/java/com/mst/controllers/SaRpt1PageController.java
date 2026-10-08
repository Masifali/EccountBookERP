package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** Group R1: the ten Sales Reports pages (module 53). Each is a new template; no existing route or template is touched. */
@Controller
public class SaRpt1PageController {
    /** 832 GdnRegister = Architecture.WinApp.pcc.Reports.GdnRegister ("GDN Register"). */
    @GetMapping("/sale/reports/gdn-register")
    public String gdnRegister() { return "sale/reports/sarpt1_gdn_register"; }

    /** 840 SaleInvoiceRegister = Architecture.WinApp.pcc.Reports.SaleInvoiceRegister ("Sale Invoice Report"). */
    @GetMapping("/sale/reports/sale-invoice-register")
    public String saleInvoiceRegister() { return "sale/reports/sarpt1_sale_invoice_register"; }

    /** 908 SaleInvoiceStockRateUpdate = Architecture.WinApp.FIFO.SaleInvoiceStockRateUpdate ("Sale & Export (CGS Rate Update)"). */
    @GetMapping("/sale/reports/sale-invoice-stock-rate-update")
    public String saleInvoiceStockRateUpdate() { return "sale/reports/sarpt1_sale_invoice_stock_rate_update"; }

    /** 490 OrdersWithLedgerBalance = Architecture.WinApp.Inventory_Reports.OrdersWithLedgerBalance ("Sale Order Register With Ledger Balance"). */
    @GetMapping("/sale/reports/orders-with-ledger-balance")
    public String ordersWithLedgerBalance() { return "sale/reports/sarpt1_orders_with_ledger_balance"; }

    /** 485 frmSaleInvoiceHistory = Architecture.WinApp.Inventory_Reports.frmSaleInvoiceHistory ("Sale Invoice Report"). */
    @GetMapping("/sale/reports/sale-invoice-history")
    public String saleInvoiceHistory() { return "sale/reports/sarpt1_sale_invoice_history"; }

    /** 487 frmSaleInvoiceDirectRegister = Architecture.WinApp.Inventory_Reports.frmSaleInvoiceDirectRegister ("Sale Invoice Direct Report"). */
    @GetMapping("/sale/reports/sale-invoice-direct-register")
    public String saleInvoiceDirectRegister() { return "sale/reports/sarpt1_sale_invoice_direct_register"; }

    /** 491 SalePriceListWithDiscount = Architecture.WinApp.WholeSale.SalePriceListWithDiscount ("Sale Price List With Discount"). */
    @GetMapping("/sale/reports/sale-price-list-with-discount")
    public String salePriceListWithDiscount() { return "sale/reports/sarpt1_sale_price_list_with_discount"; }

    /** 486 frmSaleInvoiceReturnRegister = Architecture.WinApp.Inventory_Reports.frmSaleInvoiceReturnRegister ("Sale Invoice Return Report"). */
    @GetMapping("/sale/reports/sale-invoice-return-register")
    public String saleInvoiceReturnRegister() { return "sale/reports/sarpt1_sale_invoice_return_register"; }

    /** 488 frmPurchaseAndSaleDetailByJobLot = Architecture.WinApp.Inventory_Reports.frmPurchaseAndSaleDetailByJobLot ("JobLot Detail (Purchase & Sale)"). */
    @GetMapping("/sale/reports/purchase-and-sale-detail-by-joblot")
    public String purchaseAndSaleDetailByJobLot() { return "sale/reports/sarpt1_purchase_and_sale_by_joblot"; }

    /** 489 DeliveryOrderHistory = Architecture.WinApp.Inventory_Reports.DeliveryOrderHistory ("5015 Delivery Order Report"). */
    @GetMapping("/sale/reports/delivery-order-history")
    public String deliveryOrderHistory() { return "sale/reports/sarpt1_delivery_order_history"; }
}
