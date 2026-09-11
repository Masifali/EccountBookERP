package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.InwardGatePassService;
import com.mst.services.PurchaseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.List;
import java.util.Map;

@Controller
@RequestMapping("/purchase")
public class PurchaseModuleViewController {

    @Autowired
    private PurchaseService purchaseService;

    @Autowired
    private InwardGatePassService inwardGatePassService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping({"", "/", "/dashboard"})
    public String purchaseDashboard(Model model) {
        model.addAttribute("activeMenu", "purchase");
        return "purchase/purchase_dashboard";
    }

    @GetMapping("/supplier")
    public String supplierPurchases(Model model) {
        return "redirect:/accounts/supplier";
    }

    @GetMapping("/reports")
    public String purchaseReports(Model model) {
        return "redirect:/accounts/reports/payables-report";
    }

    @GetMapping("/purchase-order")
    public String purchaseOrder(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Order");
        model.addAttribute("documentTypeId", 16);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(16));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/purchase_order";
    }

    @GetMapping("/inward-gate-pass")
    public String inwardGatePass(Model model) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();

        Map<String, Object> dropdowns = inwardGatePassService.getDropdowns(orgId, compId);
        Map<String, Object> nextNums = inwardGatePassService.generateNextNumbers(orgId, compId, branchId, yearId, 51, "Paddy");
        List<Map<String, Object>> historyList = inwardGatePassService.getHistory(orgId, compId, branchId, yearId, 51, null, null, null, null, null);

        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Inward Gate Pass");
        model.addAttribute("documentTypeId", 51);
        model.addAttribute("nextDocNo", nextNums.get("gpSrNo"));
        model.addAttribute("nextTypeNo", nextNums.get("gpTypeSrNo"));

        model.addAttribute("suppliers", dropdowns.get("suppliers"));
        model.addAttribute("cities", dropdowns.get("cities"));
        model.addAttribute("items", dropdowns.get("items"));
        model.addAttribute("vehicleTypes", dropdowns.get("vehicleTypes"));
        model.addAttribute("gatePassTypes", dropdowns.get("gatePassTypes"));
        model.addAttribute("weighBridges", dropdowns.get("weighBridges"));
        model.addAttribute("packingTypes", dropdowns.get("packingTypes"));
        model.addAttribute("historyList", historyList);

        return "purchase/inward_gate_pass";
    }

    @GetMapping("/goods-receipt-notes")
    public String goodsReceiptNotes(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Goods Receipt Notes (GRN)");
        model.addAttribute("documentTypeId", 36);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(36));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/goods_receipt_notes";
    }

    @GetMapping("/grn-sale-return")
    public String grnSaleReturn(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "GRN (Sale Return)");
        model.addAttribute("documentTypeId", 37);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(37));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/grn_sale_return";
    }

    @GetMapping("/purchase-invoice")
    public String purchaseInvoice(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice");
        model.addAttribute("documentTypeId", 18);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(18));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/purchase_invoice";
    }

    @GetMapping("/purchase-invoice-again-grn-direct")
    public String purchaseInvoiceAgainGrnDirect(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Against GRN Direct");
        model.addAttribute("documentTypeId", 18);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(18));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/purchase_invoice_again_grn_direct";
    }

    @GetMapping("/purchase-invoice-direct")
    public String purchaseInvoiceDirect(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Direct");
        model.addAttribute("documentTypeId", 18);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(18));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/purchase_invoice_direct";
    }

    @GetMapping("/purchase-invoice-return")
    public String purchaseInvoiceReturn(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Return");
        model.addAttribute("documentTypeId", 19);
        model.addAttribute("nextDocNo", purchaseService.generateNextDocNo(19));
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        return "purchase/purchase_invoice_return";
    }
}
