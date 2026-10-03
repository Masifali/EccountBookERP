package com.mst.controllers.lab;

import com.mst.services.lab.SaleAnalysisRequest;
import com.mst.services.lab.SaleAnalysisService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.CacheControl;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/**
 * JSON for screen 161 "Sale Analysis" (InvLabSaleAnalysis.cs, DocumentTypeId 304).
 * No parameter carries tenancy: organization, company and user come from the session inside
 * {@link SaleAnalysisService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice);
 * rights are AccessDeniedException (403). The desktop's "Are you sure to Save?" / "Are you sure to
 * Update?" is answered as 409 {confirm:true,message}; the page asks and posts again with confirm=true.
 * A procedure's own RAISERROR is what the desktop shows in its message box (catch -> MessageBox.Show
 * (ex.Message)), so it is answered as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/sale-analysis")
public class SaleAnalysisRestController {

    private static final Logger LOG = LoggerFactory.getLogger(SaleAnalysisRestController.class);

    private final SaleAnalysisService service;

    public SaleAnalysisRestController(SaleAnalysisService service) { this.service = service; }

    /** InvLabPurchaseAnalysis_Load (:221). */
    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    /** Refresh1 (:554). */
    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.formRefresh(); }

    /** New (:518). */
    @GetMapping("/new")
    public Map<String, Object> formNew() { return service.formNew(); }

    /** txtgatepassno_Leave (:305). */
    @GetMapping("/gate-pass")
    public Map<String, Object> gatePass(@RequestParam(defaultValue = "0") int id) { return service.gatePass(id); }

    /** cmbanalysisgroup_Leave (:412). */
    @GetMapping("/parameters")
    public List<Map<String, Object>> parameters(@RequestParam(defaultValue = "0") int groupId) { return service.parameters(groupId); }

    /** historygridfill (:860). */
    @GetMapping("/history")
    public List<Map<String, Object>> history() { return service.history(); }

    /** ReadById (:714) and the history "Detail" button (:820). */
    @GetMapping("/{id:[0-9]+}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    @GetMapping("/{id:[0-9]+}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachments(id); }

    /** GenerateReport (:1133): the rights / ownership / "Not Record Found For Display" check before the PDF is opened. */
    @GetMapping("/{id:[0-9]+}/print-check")
    public Map<String, Object> printCheck(@PathVariable int id, @RequestParam(defaultValue = "false") boolean toolbar) {
        return service.checkPrint(id, toolbar);
    }

    /** The stored Analysis Pic ("analysis") or Cooking Pic ("cooking"); 404 when the record has no file. */
    @GetMapping("/{id:[0-9]+}/picture/{which:analysis|cooking}")
    public ResponseEntity<byte[]> picture(@PathVariable int id, @PathVariable String which) {
        Object[] p = service.picture(id, which);
        if (p == null) return ResponseEntity.notFound().build();
        String n = String.valueOf(p[0]).toLowerCase(Locale.ROOT);
        MediaType type = n.endsWith(".png") ? MediaType.IMAGE_PNG
                : n.endsWith(".jpg") || n.endsWith(".jpeg") ? MediaType.IMAGE_JPEG
                : MediaType.APPLICATION_OCTET_STREAM;
        return ResponseEntity.ok().contentType(type).header("X-Content-Type-Options", "nosniff")
                .cacheControl(CacheControl.noStore()).body((byte[]) p[1]);
    }

    /** btnSave_Click (:573) / btnUpdate_Click (:700) -> Insert() (:584). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody SaleAnalysisRequest request) { return service.save(request); }

    /** Desktop Yes/No prompt: 409 {confirm:true,message}; the page resends with confirm=true. */
    @ExceptionHandler(SaleAnalysisService.ConfirmRequired.class)
    public ResponseEntity<Map<String, Object>> confirm(SaleAnalysisService.ConfirmRequired e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("confirm", true);
        m.put("message", e.getMessage());
        return ResponseEntity.status(HttpStatus.CONFLICT).body(m);
    }

    /** A procedure's RAISERROR text — the message the desktop's message box shows. */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Sale Analysis database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
