package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.Inact1CreditNotesService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 27 "Customer Incentive Credit Notes" (Architecture.WinApp.WholeSale.frmCreditNotes).
 *   GET  /accounts/credit-notes                         the form
 *   GET  /api/accounts/credit-notes/load                FrmExportSalesContract_Load (DocNo + Debit Account combo)
 *   GET  /api/accounts/credit-notes/generate-code       GenerateCode
 *   GET  /api/accounts/credit-notes/show-detail         btnShowDetail_Click
 *   GET  /api/accounts/credit-notes/history             HistoryFill
 *   GET  /api/accounts/credit-notes/history-detail      DataGridHistory_ColumnButtonClick (Detail)
 *   POST /api/accounts/credit-notes/save                btnsave_Click
 * The form reads the user's rights but never uses them (no button is gated), so the View right of screen 27 is enforced.
 */
@Controller
public class Inact1CreditNotesController {
    private static final int SCREEN_ID = 27;
    private static final String API = "/api/accounts/credit-notes";

    @Autowired private Inact1CreditNotesService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
    }

    @GetMapping("/accounts/credit-notes")
    public String page(Model model) {
        gate();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Customer Incentive Credit Notes");
        return "accounts/inact1_credit_notes";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(() -> { gate(); return service.load(); }); }

    @GetMapping(API + "/generate-code") @ResponseBody
    public ResponseEntity<?> generateCode() { return run(() -> { gate(); return service.generateCode(); }); }

    @GetMapping(API + "/show-detail") @ResponseBody
    public ResponseEntity<?> showDetail(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate) {
        return run(() -> { gate(); return service.showDetail(fromDate, toDate); });
    }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history() { return run(() -> { gate(); return service.history(); }); }

    @GetMapping(API + "/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@RequestParam int id) { return run(() -> { gate(); return service.historyDetail(id); }); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> { gate(); return service.save(body); }); }

    private ResponseEntity<?> run(Supplier<Object> work) {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            m.put("success", true);
            m.put("data", work.get());
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
