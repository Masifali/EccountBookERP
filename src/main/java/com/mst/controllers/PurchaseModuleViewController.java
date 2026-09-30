package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.InwardGatePassService;
import com.mst.services.MarketGrnService;
import com.mst.services.PurchaseOrderFullService;
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

    /* Supplies the GRN screen's database-backed lists - vehicle types, packing types (restricted
       to ids {1,2,5}), UOMs and crop years - through the same procedures InvFrmGRN.cs uses. */
    @Autowired
    private MarketGrnService marketGrnService;

    /* getPaymentTerms() reads Sp_InvDueTerms_GetAllMethod - the same InvDueTerms list
       InvfrmPurchaseInvoice.cs:1066 binds with value member Id and display member
       TermsDescription. */
    @Autowired
    private PurchaseOrderFullService purchaseOrderFullService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @Autowired
    private com.mst.repositories.PurchaseInvoiceNumberingRepository invoiceNumbering;

    @Autowired private com.mst.repositories.GrnNumberingRepository grnNumbering;

    private int grnNumber(int type) {
        return grnNumbering.next(currentUserContext.currentOrganizationId(),currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),currentUserContext.currentFinancialYearId(),type);
    }

    private int invoiceNumber(int type) {
        return invoiceNumbering.next(currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                currentUserContext.currentFinancialYearId(), type);
    }

    /* dbo.App 3 "Purchase" through the generic App/Module/Screen renderer (AppMenuController), like the
       Accounts, Production, HRM and Taxation hubs: module cards (5 Supplier Purchases, 52 Purchase Reports)
       and their counts come from the user's CompanyRights + ScreenRights rows, as the desktop frmMenue
       builds them. The old purchase_dashboard.html / supplier_purchases.html / purchase_reports_dashboard.html
       had typed counts (8 / 4), invented tile titles and no tiles for 129, 868 and 869. ?module=N is kept. */
    @GetMapping({"", "/", "/dashboard"})
    public String purchaseDashboard() {
        return "forward:/app/Purchase";
    }

    @GetMapping("/supplier")
    public String supplierPurchases() {
        return "forward:/app/Purchase?module=5";
    }

    @GetMapping("/reports")
    public String purchaseReports() {
        return "forward:/app/Purchase?module=52";
    }

    @GetMapping("/reports/purchase-order-register")
    public String purchaseOrderRegisterReport(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Order Register");
        return "purchase/reports/purchase_order_report";
    }

    @GetMapping("/reports/inward-gate-pass-register")
    public String inwardGatePassRegisterReport(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Inward Gate Pass Register");
        return "purchase/reports/inward_gate_pass_report";
    }

    @GetMapping("/reports/grn-register")
    public String grnRegisterReport(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "GRN Register");
        return "purchase/reports/grn_report";
    }

    @GetMapping("/reports/purchase-invoice-register")
    public String purchaseInvoiceRegisterReport(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Register");
        return "purchase/reports/purchase_invoice_report";
    }

    /* 869 SupplierPortal.Reports.frmSupplierDispatchPreBillReport "Stock In Transit Report" (module 52);
       data from PurchaseReportRestController /purchase/api/reports/stock-in-transit. */
    @GetMapping("/reports/stock-in-transit")
    public String stockInTransitReport(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Stock In Transit Report");
        return "purchase/reports/stock_in_transit_report";
    }

    @GetMapping("/purchase-order")
    public String purchaseOrder(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Order");
        model.addAttribute("documentTypeId", 41);
        model.addAttribute("nextDocNo", purchaseOrderFullService.generateNextDocNo(41));
        /* suppliers / items / warehouses / jobLots from PurchaseService were put here but
           purchase_order.html reads none of them (it has no th: expressions) and they are not
           the desktop's lists (PurchsaeOrder.cs has no warehouse at all); the page fills every
           combo from /api/purchase-order/*. Removed - four unused queries per page load. */
        return "purchase/purchase_order";
    }

    @GetMapping("/inward-gate-pass")
    public String inwardGatePass(Model model) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();

        Map<String, Object> dropdowns = inwardGatePassService.getDropdowns(orgId, compId);
        // InwardGatePass_Load: gpnofill() then cmbgptype_Leave_1 with the row gatepasstype() activates (Rows[2] after the
        // blank row = second returned type). The open-vehicle grid (grdfrmfill) is loaded by the page from /open-records.
        List<?> gpTypes = (List<?>) dropdowns.get("gatePassTypes");
        String defaultGpType = gpTypes != null && gpTypes.size() > 1 ? String.valueOf(((Map<?, ?>) gpTypes.get(1)).get("name")) : "";
        Map<String, Object> nextNums = inwardGatePassService.generateNextNumbers(orgId, compId, branchId, yearId, 51, defaultGpType);

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
        model.addAttribute("orderTypes", dropdowns.get("orderTypes"));
        model.addAttribute("transitVehicles", dropdowns.get("transitVehicles"));
        model.addAttribute("weighBridges", dropdowns.get("weighBridges"));
        model.addAttribute("packingTypes", dropdowns.get("packingTypes"));
        model.addAttribute("documentTypes", dropdowns.get("documentTypes"));
        return "purchase/inward_gate_pass";
    }

    @GetMapping("/goods-receipt-notes")
    public String goodsReceiptNotes(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Goods Receipt Notes (GRN)");
        model.addAttribute("documentTypeId", 46);
        model.addAttribute("nextDocNo", grnNumber(46));
        model.addAllAttributes(marketGrnService.getDropdowns(currentUserContext.currentOrganizationId(),currentUserContext.currentCompanyId()));
        return "purchase/goods_receipt_notes";
    }

    @org.springframework.beans.factory.annotation.Autowired private com.mst.services.SaleReturnGrnService saleReturnGrnService;

    @GetMapping("/grn-sale-return")
    public String grnSaleReturn(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "GRN (Sale Return)");
        model.addAttribute("documentTypeId", 143);
        model.addAttribute("nextDocNo", grnNumber(143));
        model.addAllAttributes(saleReturnGrnService.getDropdowns(currentUserContext.currentOrganizationId(),currentUserContext.currentCompanyId()));
        return "purchase/grn_sale_return";
    }

    @GetMapping("/purchase-invoice")
    public String purchaseInvoice(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice");
        model.addAttribute("documentTypeId", 56);
        // The dedicated type-56 API supplies original scoped lists after showing the loader.
        return "purchase/purchase_invoice";
    }

    @GetMapping("/purchase-invoice-again-grn-direct")
    public String purchaseInvoiceAgainGrnDirect(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Against GRN Direct");
        /* Was 18. The desktop form declares DocumentTypeId = 138 (frmPurchaseInvoiceAgaintGrnDirect.cs:1474/:1879/:2315).
           Three different Purchase Invoice screens all carried 18, which collapsed three
           distinct document types into one: one shared numbering sequence, and history or
           search on any of them returning all three. */
        model.addAttribute("documentTypeId", 138);
        /* The page reads rights, its original scoped lookups and the next number from
           /api/purchase-invoice-against-grn-direct/init (the form has no item/warehouse/job-lot
           combos - every detail row comes from a GRN Direct). */
        return "purchase/purchase_invoice_again_grn_direct";
    }

    @GetMapping("/purchase-invoice-direct")
    public String purchaseInvoiceDirect(Model model) {
        // This form loads original scoped desktop lookups and rights through its dedicated API.
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Direct");
        return "purchase/purchase_invoice_direct";
    }
    @GetMapping("/purchase-invoice-return")
    public String purchaseInvoiceReturn(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Return");
        /* Was 19. The desktop form declares DocumentTypeId = 59 (InvfrmInvPurchaseInvoiceReturn.cs:1261/:2488/:3086).
           Three different Purchase Invoice screens all carried 19, which collapsed three
           distinct document types into one: one shared numbering sequence, and history or
           search on any of them returning all three. */
        model.addAttribute("documentTypeId", 59);
        // Every lookup, the numbers and the rights come from /api/purchase/purchase-invoice-return/init (the desktop's own sources).
        return "purchase/purchase_invoice_return";
    }

    @GetMapping({"/purchase-invoice-store-management", "/purchase-invoice-store"})
    public String purchaseInvoiceStoreManagement(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Store Management");
        model.addAttribute("documentTypeId", 61);
        model.addAttribute("nextDocNo", invoiceNumber(61));
        model.addAttribute("nextBranchSrNo", invoiceNumbering.nextBranch(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), currentUserContext.currentFinancialYearId(), currentUserContext.currentBranchId(), 61));
        model.addAttribute("nextTaxNo", 1);
        /* Was a single hard-coded "Cash" option whose VALUE was the string "Cash"; the desktop
           stores an InvDueTerms Id. Same list as the Purchase Invoice screen. */
        model.addAttribute("paymentTerms", purchaseOrderFullService.getPaymentTerms());
        model.addAttribute("suppliers", purchaseService.getSuppliers(""));
        model.addAttribute("items", purchaseService.getItems(""));
        model.addAttribute("warehouses", purchaseService.getWarehouses());
        model.addAttribute("racks", purchaseService.getRacks());
        model.addAttribute("jobLots", purchaseService.getJobLots());
        model.addAttribute("taxAccounts", purchaseService.getTaxAccounts());
        model.addAttribute("discountAccounts", purchaseService.getDiscountAccounts());
        return "purchase/purchase_invoice_store_management";
    }
}


