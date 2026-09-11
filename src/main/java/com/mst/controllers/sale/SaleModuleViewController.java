package com.mst.controllers.sale;

import com.mst.services.PurchaseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/sale")
public class SaleModuleViewController {

    @Autowired
    private PurchaseService purchaseService; // Uses existing shared master data services

    @GetMapping({"", "/", "/dashboard"})
    public String saleDashboard(Model model) {
        model.addAttribute("activeMenu", "sale");
        return "sale/sale_dashboard";
    }

    @GetMapping("/customer")
    public String customerSales(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Customer Sales");
        return "sale/customer_sales";
    }

    @GetMapping("/reports")
    public String saleReports(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sales Reports");
        return "sale/sales_reports";
    }

    @GetMapping("/driver-bio")
    public String driverBio(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Driver Bio");
        return "sale/driver_bio";
    }

    @GetMapping("/delivery-order")
    public String deliveryOrder(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Delivery Order");
        model.addAttribute("documentTypeId", 37);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(37));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/delivery_order";
    }

    @GetMapping("/outward-gate-pass")
    public String outwardGatePass(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Outward Gate Pass");
        model.addAttribute("documentTypeId", 35);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(35));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/outward_gate_pass";
    }

    @GetMapping("/gdn")
    public String gdn(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Goods Dispatch Note");
        model.addAttribute("documentTypeId", 36);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(36));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/gdn";
    }

    @GetMapping("/gdn-direct")
    public String gdnDirect(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "GDN Direct");
        model.addAttribute("documentTypeId", 36);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(36));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/gdn_direct";
    }

    @GetMapping("/gdn-purchase-return")
    public String gdnPurchaseReturn(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "GDN for Purchase Return");
        model.addAttribute("documentTypeId", 36);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(36));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/gdn_purchase_return";
    }

    @GetMapping("/delivery-challan")
    public String deliveryChallan(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Delivery Challan");
        model.addAttribute("documentTypeId", 38);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(38));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/delivery_challan";
    }

    @GetMapping("/sale-invoice-gdn-no-wb")
    public String saleInvoiceGdnNoWb(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Against GDN Without WB");
        model.addAttribute("documentTypeId", 20);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(20));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/sale_invoice_gdn_no_wb";
    }

    @GetMapping("/sale-invoice")
    public String saleInvoice(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice");
        model.addAttribute("documentTypeId", 20);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(20));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/sale_invoice";
    }

    @GetMapping("/sale-invoice-direct")
    public String saleInvoiceDirect(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Direct");
        model.addAttribute("documentTypeId", 20);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(20));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/sale_invoice_direct";
    }

    @GetMapping("/sale-invoice-return")
    public String saleInvoiceReturn(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Return");
        model.addAttribute("documentTypeId", 21);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(21));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "sale/sale_invoice_return";
    }
}
