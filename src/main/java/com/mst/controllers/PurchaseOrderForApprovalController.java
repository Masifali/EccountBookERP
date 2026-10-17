package com.mst.controllers;

import com.mst.services.PurchaseOrderForApprovalService;
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
 * Approvals - Purchase Order. Architecture.WinApp.ApprovalDashboard\PurchaseOrderForApproval.cs (variant "po",
 * dashboard TypeID 20) and Architecture.WinApp.FeedMill.Approval\PurchaseOrderForApproval.cs (variant "fm", TypeID 1100).
 * Opened from the Approval Dashboard card only; the desktop screen has no screen id.
 */
@Controller
public class PurchaseOrderForApprovalController {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseOrderForApprovalController.class);

    @Autowired
    private PurchaseOrderForApprovalService service;

    @GetMapping("/dashboard/purchase-order-for-approval")
    public String page(Model model,
                       @RequestParam(name = "variant", defaultValue = "po") String variant,
                       @RequestParam(name = "popup", defaultValue = "0") int popup) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Approvals");
        model.addAttribute("popup", popup == 1);
        model.addAttribute("variant", "fm".equals(variant) ? "fm" : "po");
        return "dashboard/purchase_order_for_approval";
    }

    @GetMapping("/api/dashboard/purchase-order-approval/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup(
            @RequestParam(name = "variant", defaultValue = "po") String variant,
            @RequestParam(name = "branchId", defaultValue = "0") int branchId,
            @RequestParam(name = "fromDate", required = false) String fromDate,
            @RequestParam(name = "toDate", required = false) String toDate) {
        try {
            return ResponseEntity.ok(service.setup(variant, branchId, fromDate, toDate));
        } catch (Exception e) {
            return fail(e);
        }
    }

    @GetMapping("/api/dashboard/purchase-order-approval/combos")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> combos(
            @RequestParam(name = "variant", defaultValue = "po") String variant,
            @RequestParam(name = "branchId", defaultValue = "0") int branchId) {
        try {
            return ResponseEntity.ok(service.refreshCombos(variant, branchId));
        } catch (Exception e) {
            return fail(e);
        }
    }

    @GetMapping("/api/dashboard/purchase-order-approval/history")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> history(
            @RequestParam(name = "variant", defaultValue = "po") String variant,
            @RequestParam(name = "branchId", defaultValue = "0") int branchId,
            @RequestParam(name = "fromDate", required = false) String fromDate,
            @RequestParam(name = "toDate", required = false) String toDate,
            @RequestParam(name = "fromNo", required = false) String fromNo,
            @RequestParam(name = "toNo", required = false) String toNo,
            @RequestParam(name = "supplierId", defaultValue = "0") int supplierId,
            @RequestParam(name = "itemId", defaultValue = "0") int itemId,
            @RequestParam(name = "status", required = false) String status,
            @RequestParam(name = "approved", required = false) String approved) {
        try {
            return ResponseEntity.ok(service.history(filter(variant, branchId, fromDate, toDate, fromNo, toNo,
                    supplierId, itemId, status, approved)));
        } catch (Exception e) {
            return fail(e);
        }
    }

    @GetMapping("/api/dashboard/purchase-order-approval/pending")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> pending(
            @RequestParam(name = "variant", defaultValue = "po") String variant,
            @RequestParam(name = "branchId", defaultValue = "0") int branchId,
            @RequestParam(name = "fromDate", required = false) String fromDate,
            @RequestParam(name = "toDate", required = false) String toDate,
            @RequestParam(name = "fromNo", required = false) String fromNo,
            @RequestParam(name = "toNo", required = false) String toNo,
            @RequestParam(name = "supplierId", defaultValue = "0") int supplierId,
            @RequestParam(name = "itemId", defaultValue = "0") int itemId,
            @RequestParam(name = "status", required = false) String status,
            @RequestParam(name = "approved", required = false) String approved) {
        try {
            return ResponseEntity.ok(service.pending(filter(variant, branchId, fromDate, toDate, fromNo, toNo,
                    supplierId, itemId, status, approved)));
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** body: variant, source (row|approve|cancel|pending), reqType, ids[], approved, status. */
    @PostMapping("/api/dashboard/purchase-order-approval/update")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> update(@RequestBody Map<String, Object> body) {
        try {
            List<Integer> ids = new ArrayList<>();
            Object raw = body == null ? null : body.get("ids");
            if (raw instanceof List) {
                for (Object o : (List<?>) raw) {
                    int v = toInt(o);
                    if (v != 0) ids.add(v);
                }
            }
            return ResponseEntity.ok(service.update(str(body, "variant"), str(body, "source"), str(body, "reqType"),
                    ids, str(body, "approved"), str(body, "status")));
        } catch (Exception e) {
            return fail(e);
        }
    }

    @GetMapping("/api/dashboard/purchase-order-approval/gl-account")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> glAccount(
            @RequestParam(name = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        try {
            return ResponseEntity.ok(service.glAccount(supplierCustomerId));
        } catch (Exception e) {
            return fail(e);
        }
    }

    private static PurchaseOrderForApprovalService.Filter filter(String variant, int branchId, String fromDate,
            String toDate, String fromNo, String toNo, int supplierId, int itemId, String status, String approved) {
        PurchaseOrderForApprovalService.Filter f = new PurchaseOrderForApprovalService.Filter();
        f.variant = variant;
        f.branchId = branchId;
        f.fromDate = fromDate;
        f.toDate = toDate;
        f.fromNo = fromNo;
        f.toNo = toNo;
        f.supplierId = supplierId;
        f.itemId = itemId;
        f.status = status;
        f.approved = approved;
        return f;
    }

    private static String str(Map<String, Object> m, String k) {
        Object o = m == null ? null : m.get(k);
        return o == null ? null : String.valueOf(o);
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static ResponseEntity<Map<String, Object>> fail(Exception e) {
        LOG.error("PurchaseOrderForApproval failed", e);
        Map<String, Object> out = new LinkedHashMap<>();
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() != null ? t.getMessage() : e.getMessage();
        out.put("message", m == null ? e.getClass().getSimpleName() : m);
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(out);
    }
}
