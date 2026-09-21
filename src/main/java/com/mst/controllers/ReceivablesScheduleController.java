package com.mst.controllers;

import com.mst.services.ReceivablesScheduleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Receivables And Receipt Schedule -
 * Architecture.WinApp.Account_Reports\ReceivablesAndReceiptSchedule.cs.
 *
 * The last path segment normalises to "receivablesandreceiptschedule", which is the TargetUrl
 * class name, so the DashBoard card resolves to this route through ScreenRouteIndex without a
 * hand-written entry.
 */
@Controller
public class ReceivablesScheduleController {

    @Autowired
    private ReceivablesScheduleService service;

    @GetMapping("/dashboard/receivables-and-receipt-schedule")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Receivables And Receipt Schedule");
        return "dashboard/receivables_receipt_schedule";
    }

    /** PayablesAndPaymentSchedule_Load, :132-163 - features, the six dates, the five dropdowns. */
    @GetMapping("/api/dashboard/receivables-schedule/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /**
     * btnshow_Click -> gridHistory(), :165-293.
     *
     * Each date carries its own tick box because the desktop sends a date only when its box is
     * ticked, and omitting a parameter is not the same as sending NULL.
     */
    @GetMapping("/api/dashboard/receivables-schedule/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String dueFrom,
            @RequestParam(required = false) String dueUpto,
            @RequestParam(required = false) String saleFrom,
            @RequestParam(required = false) String saleTo,
            @RequestParam(defaultValue = "false") boolean chkFrom,
            @RequestParam(defaultValue = "false") boolean chkTo,
            @RequestParam(defaultValue = "false") boolean chkDueFrom,
            @RequestParam(defaultValue = "false") boolean chkDueUpto,
            @RequestParam(defaultValue = "false") boolean chkSaleFrom,
            @RequestParam(defaultValue = "false") boolean chkSaleTo,
            @RequestParam(defaultValue = "0") int parentCategoryId,
            @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "0") int customerGroupId,
            @RequestParam(required = false) String controlAccountIds,
            @RequestParam(required = false) String branchIds) {

        ReceivablesScheduleService.Filters f = new ReceivablesScheduleService.Filters();
        f.fromDate = fromDate;
        f.toDate = toDate;
        f.dueFrom = dueFrom;
        f.dueUpto = dueUpto;
        f.saleFrom = saleFrom;
        f.saleTo = saleTo;
        f.chkFrom = chkFrom;
        f.chkTo = chkTo;
        f.chkDueFrom = chkDueFrom;
        f.chkDueUpto = chkDueUpto;
        f.chkSaleFrom = chkSaleFrom;
        f.chkSaleTo = chkSaleTo;
        f.parentCategoryId = parentCategoryId;
        f.customGroupId = customGroupId;
        f.customerGroupId = customerGroupId;
        f.controlAccountIds = controlAccountIds == null ? "" : controlAccountIds;
        f.branchIds = branchIds == null ? "" : branchIds;

        return ResponseEntity.ok(service.report(f));
    }
}
