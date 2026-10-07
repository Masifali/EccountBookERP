package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.AdvanceAdjustmentService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 4 "Advance Adjustment" (Architecture.WinApp.Account_Definition.frmAdvanceAdjustment).
 *   GET  /accounts/vouchers/advance-adjustment-desktop          the form (the older /accounts/vouchers/advance-adjustment page is a static mock-up and is left alone)
 *   GET  /api/accounts/advance-adjustment/load                  frmAdvanceAdjustment_Load
 *   GET  /api/accounts/advance-adjustment/doc-no                GenerateDocNo
 *   GET  /api/accounts/advance-adjustment/supplier/{id}         cmbSupplier_Leave
 *   POST /api/accounts/advance-adjustment/save                  btnSave_Click
 * A desktop MessageBox warning comes back as {success:false, warning:true, message}.
 * The desktop checks no Save right (opening the screen is the right), so the screen's View right (4) is enforced.
 */
@Controller
public class AdvanceAdjustmentController {
    private static final int SCREEN_ID = 4;
    private static final String API = "/api/accounts/advance-adjustment";

    @Autowired private AdvanceAdjustmentService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
    }

    @GetMapping("/accounts/vouchers/advance-adjustment-desktop")
    public String page(Model model) {
        gate();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Advance Adjustment");
        return "accounts/vouchers/advance_adjustment_desktop";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(() -> { gate(); return service.load(); }); }

    @GetMapping(API + "/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> { gate(); return service.generateDocNo(); }); }

    @GetMapping(API + "/supplier/{id}") @ResponseBody
    public ResponseEntity<?> supplier(@PathVariable int id) { return run(() -> { gate(); return service.supplierGrids(id); }); }

    @PostMapping(API + "/save") @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> r) {
        return run(() -> {
            gate();
            Object s = r.get("supplierId");
            Integer supplier = null;
            try { if (s != null && !s.toString().isBlank()) supplier = (int) Double.parseDouble(s.toString().trim()); } catch (NumberFormatException e) { supplier = null; }
            int id = service.save(str(r.get("docDate")), str(r.get("docNo")), str(r.get("adjustedAmount")), str(r.get("remarks")), supplier,
                    (List<Map<String, Object>>) r.get("invoices"), (List<Map<String, Object>>) r.get("advances"));
            Map<String, Object> out = new LinkedHashMap<>();
            out.put("id", id);
            out.put("message", "SAVED SECCESSFULLY");
            return out;
        });
    }

    private static String str(Object o) { return o == null ? null : o.toString(); }

    private ResponseEntity<?> run(Supplier<Object> work) {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            m.put("success", true);
            m.put("data", work.get());
            return ResponseEntity.ok(m);
        } catch (AdvanceAdjustmentService.Warning e) {
            m.clear(); m.put("success", false); m.put("warning", true); m.put("message", e.getMessage());
            return ResponseEntity.ok(m);
        } catch (AccessDeniedException e) {
            m.clear(); m.put("success", false); m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(m);
        } catch (Exception e) {
            Throwable c = e;
            while (c.getCause() != null && c.getCause() != c) c = c.getCause();
            m.clear(); m.put("success", false); m.put("message", c.getMessage() == null ? e.getClass().getSimpleName() : c.getMessage());
            boolean bad = e instanceof IllegalArgumentException || e instanceof IllegalStateException;
            return ResponseEntity.status(bad ? HttpStatus.OK : HttpStatus.INTERNAL_SERVER_ERROR).body(m);
        }
    }
}
