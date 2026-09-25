package com.mst.controllers.cmagt;

import com.mst.services.cmagt.CmagtReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.Collections;
import java.util.Map;

/**
 * Commission Trading reports. The URLs are unchanged; each now accepts the desktop form's full
 * filter set as query parameters (all optional):
 *
 *   fromDate, toDate (yyyy-MM-dd), dateType (doc | entry | modify), fromDocNo, toDocNo,
 *   reportType (the desktop "Report Type" text, sent as @Activity), approved (Approved |
 *   UnApproved | All), parentItemIds (csv), itemCategoryId, itemId, agentId, buyerId, supplierId,
 *   deliverToPartyId, shipToAddress, statusId, and for the trade-bill register parentCategoryId,
 *   itemTypeId, brokerId, glAccountId, customerId, vehicleNo, deliveryTerm.
 *
 * Which of them reach the procedure is decided per report in CmagtReportService, exactly as the
 * desktop BLL decides it. Organization / company / branch / financial year are NEVER taken from
 * the request - CmagtReportService reads them from the signed-in session.
 */
@RestController
@RequestMapping("/api/commission/reports")
public class CmagtReportRestController {

    @Autowired
    private CmagtReportService service;

    @GetMapping("/sale-order")
    public ResponseEntity<?> getSaleOrderReport(@RequestParam Map<String, String> q) {
        return run(CmagtReportService.SALE_ORDER, q);
    }

    @GetMapping("/purchase-order")
    public ResponseEntity<?> getPurchaseOrderReport(@RequestParam Map<String, String> q) {
        return run(CmagtReportService.PURCHASE_ORDER, q);
    }

    @GetMapping("/grn-supplier-loading")
    public ResponseEntity<?> getGrnSupplierLoadingReport(@RequestParam Map<String, String> q) {
        return run(CmagtReportService.GRN_SUPPLIER_LOADING, q);
    }

    @GetMapping("/gdn-buyer-dispatch")
    public ResponseEntity<?> getGdnBuyerDispatchReport(@RequestParam Map<String, String> q) {
        return run(CmagtReportService.GDN_BUYER_DISPATCH, q);
    }

    @GetMapping("/agent-trade-bill-register")
    public ResponseEntity<?> getAgentTradeBillRegister(@RequestParam Map<String, String> q) {
        return run(CmagtReportService.AGENT_TRADE_BILL_REGISTER, q);
    }

    /** The report's own filter-combo rows (ComboDbCall / ComboFill) plus the From-date default. */
    @GetMapping("/{report}/filters")
    public ResponseEntity<?> filters(@PathVariable String report) {
        try {
            return ResponseEntity.ok(service.filters(report));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(Collections.singletonMap("message", e.getMessage()));
        }
    }

    private ResponseEntity<?> run(String report, Map<String, String> q) {
        try {
            return ResponseEntity.ok(service.report(report, q));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(Collections.singletonMap("message", e.getMessage()));
        }
    }
}
