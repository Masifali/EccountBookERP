package com.mst.controllers;

import com.mst.services.PayablesScheduleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Payables And Payment Schedule -
 * Architecture.WinApp.Account_Reports\PayablesAndPaymentSchedule.cs.
 *
 * The last path segment normalises to "payablesandpaymentschedule", the TargetUrl class name,
 * so the DashBoard card resolves here through ScreenRouteIndex without a hand-written entry.
 */
@Controller
public class PayablesScheduleController {

    @Autowired
    private PayablesScheduleService service;

    @GetMapping("/dashboard/payables-and-payment-schedule")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Payables And Payment Schedule");
        return "dashboard/payables_payment_schedule";
    }

    /** PayablesAndPaymentSchedule_Load, :146-179. */
    @GetMapping("/api/dashboard/payables-schedule/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** btnshow_Click -> gridHistory(), :182-355 - detail rows plus the aging summary. */
    @GetMapping("/api/dashboard/payables-schedule/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String dueFrom,
            @RequestParam(required = false) String dueUpto,
            @RequestParam(required = false) String purchaseFrom,
            @RequestParam(required = false) String purchaseTo,
            @RequestParam(defaultValue = "false") boolean chkFrom,
            @RequestParam(defaultValue = "false") boolean chkTo,
            @RequestParam(defaultValue = "false") boolean chkDueFrom,
            @RequestParam(defaultValue = "false") boolean chkDueUpto,
            @RequestParam(defaultValue = "false") boolean chkPurchaseFrom,
            @RequestParam(defaultValue = "false") boolean chkPurchaseTo,
            @RequestParam(defaultValue = "0") int parentCategoryId,
            @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "0") int customerGroupId,
            @RequestParam(defaultValue = "0") int agingDays,
            @RequestParam(required = false) String controlAccountIds,
            @RequestParam(required = false) String branchIds) {

        PayablesScheduleService.Filters f = new PayablesScheduleService.Filters();
        f.fromDate = fromDate;
        f.toDate = toDate;
        f.dueFrom = dueFrom;
        f.dueUpto = dueUpto;
        f.purchaseFrom = purchaseFrom;
        f.purchaseTo = purchaseTo;
        f.chkFrom = chkFrom;
        f.chkTo = chkTo;
        f.chkDueFrom = chkDueFrom;
        f.chkDueUpto = chkDueUpto;
        f.chkPurchaseFrom = chkPurchaseFrom;
        f.chkPurchaseTo = chkPurchaseTo;
        f.parentCategoryId = parentCategoryId;
        f.customGroupId = customGroupId;
        f.customerGroupId = customerGroupId;
        f.agingDays = agingDays;
        f.controlAccountIds = controlAccountIds == null ? "" : controlAccountIds;
        f.branchIds = branchIds == null ? "" : branchIds;

        return ResponseEntity.ok(service.report(f));
    }
}
