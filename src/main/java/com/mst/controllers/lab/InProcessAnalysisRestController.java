package com.mst.controllers.lab;

import com.mst.services.lab.InProcessAnalysisService;
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
 * JSON for screen 162 "In-Process Analysis" (InvLabAnalysisInProcess.cs, DocumentTypeId 306).
 * No parameter carries tenancy: organization, company, financial year and user come from the session
 * inside {@link InProcessAnalysisService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice); rights
 * are AccessDeniedException (403); the desktop's "Are you sure to Save?" / "Are you sure to Update?" is a
 * 409 {confirm:true, message} the page answers by re-posting with confirm = true. A procedure's own error
 * is what the desktop shows in its MessageBox (Insert() catch, :1050), so it is answered as a 400 with
 * that text.
 */
@RestController
@RequestMapping("/api/lab/inprocess-analysis")
public class InProcessAnalysisRestController {

    private static final Logger LOG = LoggerFactory.getLogger(InProcessAnalysisRestController.class);

    private final InProcessAnalysisService service;

    public InProcessAnalysisRestController(InProcessAnalysisService service) { this.service = service; }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/new")
    public Map<String, Object> formNew() { return service.formNew(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh(@RequestParam(defaultValue = "0") int recId) { return service.formRefresh(recId); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/plant-steps")
    public Map<String, Object> plantSteps(@RequestParam(defaultValue = "0") int plantId,
                                          @RequestParam(defaultValue = "0") int jobOrderId) {
        return service.plantLeave(plantId, jobOrderId);
    }

    @GetMapping("/job-order")
    public Map<String, Object> jobOrder(@RequestParam(defaultValue = "0") int jobOrderId,
                                        @RequestParam(defaultValue = "0") int plantId) {
        return service.jobOrderLeave(jobOrderId, plantId);
    }

    @GetMapping("/group-parameters")
    public List<Map<String, Object>> groupParameters(@RequestParam(defaultValue = "0") int analysisGroupId) {
        return service.analysisGroupLeave(analysisGroupId);
    }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "true") boolean fromChecked,
                                             @RequestParam(required = false) String fromDate,
                                             @RequestParam(defaultValue = "true") boolean toChecked,
                                             @RequestParam(required = false) String toDate,
                                             @RequestParam(required = false) String fromDocNo,
                                             @RequestParam(required = false) String toDocNo,
                                             @RequestParam(defaultValue = "0") int jobOrderId,
                                             @RequestParam(defaultValue = "0") int plantId,
                                             @RequestParam(defaultValue = "0") int itemId,
                                             @RequestParam(required = false) String analyst) {
        return service.history(fromChecked, fromDate, toChecked, toDate, fromDocNo, toDocNo, jobOrderId, plantId, itemId, analyst);
    }

    @GetMapping("/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    @GetMapping("/{id}/history-detail")
    public Map<String, Object> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @GetMapping("/{id}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachments(id); }

    @GetMapping("/{id}/print-check")
    public Map<String, Object> printCheck(@PathVariable int id) { return service.checkPrint(id); }

    /** which = "analysis" (sampleanalysispicture) | "cooking" (cookingpic). */
    @GetMapping("/{id}/picture/{which}")
    public ResponseEntity<byte[]> picture(@PathVariable int id, @PathVariable String which) {
        if (!"analysis".equals(which) && !"cooking".equals(which)) return ResponseEntity.notFound().build();
        String[] name = new String[1];
        byte[] bytes = service.picture(id, which, name);
        String n = name[0] == null ? "" : name[0].toLowerCase(Locale.ROOT);
        MediaType type = n.endsWith(".png") ? MediaType.IMAGE_PNG
                : n.endsWith(".gif") ? MediaType.IMAGE_GIF
                : (n.endsWith(".jpg") || n.endsWith(".jpeg")) ? MediaType.IMAGE_JPEG
                : MediaType.APPLICATION_OCTET_STREAM;
        return ResponseEntity.ok().contentType(type).cacheControl(CacheControl.noStore()).body(bytes);
    }

    /**
     * Save (id = 0) or Update (id = the loaded record).
     *   200 {success, id, message}
     *   409 {success:false, confirm:true, message} — the desktop's Yes/No; re-post with confirm = true.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> body) {
        try {
            return ResponseEntity.ok(service.save(body));
        } catch (InProcessAnalysisService.ConfirmationRequiredException e) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", false);
            m.put("confirm", true);
            m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.CONFLICT).body(m);
        }
    }

    /** A procedure's error text — the message the desktop's MessageBox shows. */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("In-Process Analysis database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
