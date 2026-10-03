package com.mst.controllers.lab;

import com.mst.services.lab.SampleAnalysisRequest;
import com.mst.services.lab.SampleAnalysisService;
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
 * JSON for screen 159 "Sample Analysis" (InvLabSampleAnalysis.cs, DocumentTypeId 302).
 * No parameter carries tenancy: organization, company and user come from the session inside
 * {@link SampleAnalysisService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice);
 * rights are AccessDeniedException (403). The desktop's "Are you sure to Save?" / "Are you sure to
 * Update?" is answered as 409 {confirm:true,message}; the page asks and posts again with confirm=true.
 * A procedure's own RAISERROR ("The record cannot be inserted because another sample is referred to
 * against this order and item.", "Record cannot be updated because record has referred in
 * PurchaseOrder") is what the desktop shows in its message box, so it is answered as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/sample-analysis")
public class SampleAnalysisRestController {

    private static final Logger LOG = LoggerFactory.getLogger(SampleAnalysisRestController.class);

    private final SampleAnalysisService service;

    public SampleAnalysisRestController(SampleAnalysisService service) { this.service = service; }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.formRefresh(); }

    @GetMapping("/numbers")
    public Map<String, Object> numbers() { return service.numbers(); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/analysis-groups")
    public List<Map<String, Object>> analysisGroups(@RequestParam(defaultValue = "0") int typeId,
                                                    @RequestParam(required = false) String docDate) {
        return service.analysisGroups(typeId, docDate);
    }

    @GetMapping("/parameters")
    public List<Map<String, Object>> parameters(@RequestParam(defaultValue = "0") int typeId,
                                                @RequestParam(defaultValue = "0") int groupId,
                                                @RequestParam(required = false) String docDate) {
        return service.parameters(typeId, groupId, docDate);
    }

    @GetMapping("/items")
    public Map<String, Object> items(@RequestParam(defaultValue = "0") int groupId,
                                     @RequestParam(defaultValue = "0") int groupTypeId,
                                     @RequestParam(defaultValue = "0") int orderId) {
        return service.itemsFor(groupId, groupTypeId, orderId);
    }

    @GetMapping("/by-party-item")
    public Map<String, Object> byPartyItem(@RequestParam(defaultValue = "0") int supplierId,
                                           @RequestParam(defaultValue = "0") int itemId) {
        return service.byPartyItem(supplierId, itemId);
    }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "true") boolean fromChecked,
                                             @RequestParam(required = false) String fromDate,
                                             @RequestParam(defaultValue = "true") boolean toChecked,
                                             @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int supplierId,
                                             @RequestParam(defaultValue = "0") int itemId,
                                             @RequestParam(defaultValue = "0") int jobLotId,
                                             @RequestParam(defaultValue = "0") int analysisGroupId,
                                             @RequestParam(required = false) String cropYear,
                                             @RequestParam(required = false) String status) {
        return service.history(fromChecked, fromDate, toChecked, toDate, supplierId, itemId, jobLotId,
                analysisGroupId, cropYear, status);
    }

    @GetMapping("/{id:[0-9]+}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    @GetMapping("/{id:[0-9]+}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachments(id); }

    @GetMapping("/{id:[0-9]+}/print-check")
    public Map<String, Object> printCheck(@PathVariable int id) { return service.checkPrint(id); }

    /** The stored Analysis Pic ("analysis") or Cooking Pic ("cooking"); 404 when the record has no file. */
    @GetMapping("/{id:[0-9]+}/picture/{which:analysis|cooking}")
    public ResponseEntity<byte[]> picture(@PathVariable int id, @PathVariable String which) {
        SampleAnalysisService.PictureFile f = service.picture(id, which);
        if (f == null) return ResponseEntity.notFound().build();
        String n = f.name.toLowerCase(Locale.ROOT);
        MediaType type = n.endsWith(".png") ? MediaType.IMAGE_PNG
                : n.endsWith(".gif") ? MediaType.IMAGE_GIF
                : n.endsWith(".jpg") || n.endsWith(".jpeg") ? MediaType.IMAGE_JPEG
                : MediaType.APPLICATION_OCTET_STREAM;
        return ResponseEntity.ok().contentType(type).header("X-Content-Type-Options", "nosniff")
                .cacheControl(CacheControl.noStore()).body(f.bytes);
    }

    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody SampleAnalysisRequest request) { return service.save(request); }

    /** Desktop Yes/No prompt: 409 {confirm:true,message}; the page resends with confirm=true. */
    @ExceptionHandler(SampleAnalysisService.ConfirmRequired.class)
    public ResponseEntity<Map<String, Object>> confirm(SampleAnalysisService.ConfirmRequired e) {
        Map<String, Object> m = new LinkedHashMap<>();
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
        LOG.warn("Sample Analysis database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
