package com.mst.controllers;

import com.mst.services.UnApprovedInvoicesAndVouchersService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Undo Approval (Dashboard) -
 * Architecture.WinApp.ApprovalDashboard\UnApprovedInvoicesAndVouchers.cs.
 *
 * The path's last segment normalises to "unapprovedinvoicesandvouchers", the TargetUrl class
 * name, so the DashBoard card links here on its own.
 */
@Controller
public class UnApprovedInvoicesAndVouchersController {

    @Autowired
    private UnApprovedInvoicesAndVouchersService service;

    @GetMapping("/dashboard/un-approved-invoices-and-vouchers")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "UnApproved Vouchers");
        return "dashboard/un_approved_invoices_and_vouchers";
    }

    /** frmSaleTaxSummaryRpt_Load :85-99 - From a week ago, To today. */
    @GetMapping("/api/dashboard/un-approved/defaults")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> defaults() {
        return ResponseEntity.ok(service.defaults());
    }

    /**
     * DynamicallyGenerateCards(), form :101-161. fromDate / toDate are left blank when their
     * tick box is clear, and the parameter is then omitted entirely.
     */
    @GetMapping("/api/dashboard/un-approved/cards")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> cards(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.cards(fromDate, toDate));
    }
}
