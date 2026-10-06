package com.mst.controllers;

import com.mst.services.PendingApprovalVouchersHistoryService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Approvals - vouchers. Architecture.WinApp.ApprovalDashboard\PendingApprovalVouchersHistory.cs.
 *
 * Opened from the Approval Dashboard (cards 0-9, 18, 34, 35) and from Undo Approval (cards 0-9, 34, 35,
 * unApprove=1 = flagAproved). The card's TypeID travels as "id"; the DocumentTypeId is derived from it on the
 * server, never taken from the browser.
 */
@Controller
public class PendingApprovalVouchersHistoryController {

    private static final Logger LOG = LoggerFactory.getLogger(PendingApprovalVouchersHistoryController.class);

    @Autowired
    private PendingApprovalVouchersHistoryService service;

    @GetMapping("/dashboard/pending-approval-vouchers-history")
    public String page(Model model,
                       @RequestParam(name = "popup", defaultValue = "0") int popup) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Approvals");
        model.addAttribute("popup", popup == 1);
        return "dashboard/pending_approval_vouchers_history";
    }

    /** PendingApprovalVouchersHistory_Load :177-226. */
    @GetMapping("/api/dashboard/pending-approval-vouchers/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup(
            @RequestParam(name = "id", defaultValue = "0") int id,
            @RequestParam(name = "fromDate", required = false) String fromDate,
            @RequestParam(name = "toDate", required = false) String toDate,
            @RequestParam(name = "unApprove", defaultValue = "0") int unApprove) {
        try {
            return ResponseEntity.ok(service.setup(id, fromDate, toDate, unApprove == 1));
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** HistoryFill (approve mode) or PendingVoucherForUnApprovalDashboard (unApprove mode) - grid grd. */
    @GetMapping("/api/dashboard/pending-approval-vouchers/history")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> history(
            @RequestParam(name = "id", defaultValue = "0") int id,
            @RequestParam(name = "fromDate", required = false) String fromDate,
            @RequestParam(name = "toDate", required = false) String toDate,
            @RequestParam(name = "docNoFrom", defaultValue = "0") int docNoFrom,
            @RequestParam(name = "docNoTo", defaultValue = "0") int docNoTo,
            @RequestParam(name = "unApprove", defaultValue = "0") int unApprove) {
        try {
            Map<String, Object> out = new LinkedHashMap<>();
            out.put("rows", unApprove == 1
                    ? service.forUnApproval(id, fromDate, toDate, docNoFrom, docNoTo)
                    : service.history(id, fromDate, toDate, docNoFrom, docNoTo));
            return ResponseEntity.ok(out);
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** PendingForViewVouchers - grid grdPendingViewVoucher. */
    @GetMapping("/api/dashboard/pending-approval-vouchers/pending-for-view")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> pendingForView(
            @RequestParam(name = "id", defaultValue = "0") int id) {
        try {
            Map<String, Object> out = new LinkedHashMap<>();
            out.put("rows", service.pendingForView(id));
            return ResponseEntity.ok(out);
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** GridDetailByVoucherHeadId - grid grdDetail. */
    @GetMapping("/api/dashboard/pending-approval-vouchers/detail")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> detail(
            @RequestParam(name = "voucherHeadId", defaultValue = "0") int voucherHeadId) {
        try {
            Map<String, Object> out = new LinkedHashMap<>();
            out.put("rows", service.detail(voucherHeadId));
            return ResponseEntity.ok(out);
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** NoOfAttachments link - CommonServices.GetNoofAttachmentsByRefDocumentTypeID. */
    @GetMapping("/api/dashboard/pending-approval-vouchers/attachments")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> attachments(
            @RequestParam(name = "refId", defaultValue = "0") int refId,
            @RequestParam(name = "documentTypeId", defaultValue = "0") int documentTypeId) {
        try {
            Map<String, Object> out = new LinkedHashMap<>();
            out.put("rows", service.attachments(refId, documentTypeId));
            return ResponseEntity.ok(out);
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** ApproveVoucher / ApproveVoucherForPendingGrid / btnApproved_Click: ActionTypeId false, ReqType 'AP'. */
    @PostMapping("/api/dashboard/pending-approval-vouchers/approve")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> approve(@RequestBody Map<String, Object> body) {
        return update(body, false, "AP", "Record Approved Successfully");
    }

    /** btnPendingForView_Click: ActionTypeId true, ReqType 'AP'. */
    @PostMapping("/api/dashboard/pending-approval-vouchers/move-to-pending")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> moveToPending(@RequestBody Map<String, Object> body) {
        return update(body, true, "AP", "Record Move on PendingGrid Successfully");
    }

    /** UnApproveVoucher / btnApproved_Click with flagAproved: ActionTypeId false, ReqType 'UP'. */
    @PostMapping("/api/dashboard/pending-approval-vouchers/unapprove")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> unApprove(@RequestBody Map<String, Object> body) {
        return update(body, false, "UP", "Record UnApproved Successfully");
    }

    private ResponseEntity<Map<String, Object>> update(Map<String, Object> body, boolean actionTypeId,
                                                       String reqType, String okMessage) {
        try {
            List<Integer> ids = new ArrayList<>();
            Object raw = body == null ? null : body.get("ids");
            if (raw instanceof List) {
                for (Object o : (List<?>) raw) {
                    int v = toInt(o);
                    if (v != 0) ids.add(v);
                }
            }
            int n = service.updateStatus(ids, actionTypeId, reqType);
            Map<String, Object> out = new LinkedHashMap<>();
            out.put("ok", true);
            out.put("count", n);
            out.put("message", okMessage);
            return ResponseEntity.ok(out);
        } catch (Exception e) {
            return fail(e);
        }
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static ResponseEntity<Map<String, Object>> fail(Exception e) {
        LOG.error("PendingApprovalVouchersHistory failed", e);
        Map<String, Object> out = new LinkedHashMap<>();
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() != null ? t.getMessage() : e.getMessage();
        out.put("message", m == null ? e.getClass().getSimpleName() : m);
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(out);
    }
}
