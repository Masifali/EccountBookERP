package com.mst.controllers.lab;

import com.mst.services.lab.PurchaseAnalysisDto;
import com.mst.services.lab.PurchaseAnalysisService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
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
 * JSON for screen 160 "Purchase Analysis" (InvLabPurchaseAnalysis.cs, DocumentTypeId 303) and its loader
 * dialog (PendingGPForPurchaseLab.cs). No parameter carries tenancy: organization, company, branch,
 * financial year and user come from the session inside {@link PurchaseAnalysisService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice); rights
 * are AccessDeniedException (403). The desktop's "Are you sure to Save?" / "Are you sure to Update?" prompt
 * is answered as 409 {confirm:true,message}; the page asks and posts again with confirm:true. A
 * procedure's own RAISERROR (Sp_InvLabAnalysisPurchaseHeader_Insert / _Update, the cuts procedure) is what
 * the desktop shows in its message box, so it is answered here as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/purchase-analysis")
public class PurchaseAnalysisRestController {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseAnalysisRestController.class);

    private final PurchaseAnalysisService service;

    public PurchaseAnalysisRestController(PurchaseAnalysisService service) { this.service = service; }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.formRefresh(); }

    @GetMapping("/new")
    public Map<String, Object> formNew() { return service.formNew(); }

    @GetMapping("/history-refresh")
    public Map<String, Object> historyRefresh() { return service.historyRefresh(); }

    @GetMapping("/gate-pass/{id}")
    public Map<String, Object> gatePass(@PathVariable int id) { return service.gatePass(id); }

    @GetMapping("/moisture")
    public Map<String, Object> moisture(@RequestParam(defaultValue = "0") int itemId,
                                        @RequestParam(defaultValue = "0") int purchaseOrderId) {
        return service.moisture(itemId, purchaseOrderId);
    }

    @GetMapping("/rate-uoms")
    public Map<String, Object> rateUoms(@RequestParam(defaultValue = "0") int itemId) { return service.rateUoms(itemId); }

    @GetMapping("/item-rate")
    public Map<String, Object> itemRate(@RequestParam(defaultValue = "0") int itemId,
                                        @RequestParam(defaultValue = "0") int rateUomId) {
        return service.itemRate(itemId, rateUomId);
    }

    @GetMapping("/analysis-groups")
    public List<Map<String, Object>> analysisGroups(@RequestParam(defaultValue = "0") int parentCategoryId) {
        return service.analysisGroups(parentCategoryId);
    }

    @GetMapping("/parameters")
    public List<Map<String, Object>> parameters(@RequestParam(defaultValue = "0") int groupId) { return service.parameters(groupId); }

    @GetMapping("/sample/{id}")
    public Map<String, Object> sample(@PathVariable int id) { return service.sample(id); }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() { return service.loaderCombos(); }

    @GetMapping("/loader")
    public List<Map<String, Object>> loader(@RequestParam(defaultValue = "0") int supplierId,
                                            @RequestParam(defaultValue = "0") int orderTypeId) {
        return service.loader(supplierId, orderTypeId);
    }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "true") boolean fromChecked,
                                             @RequestParam(required = false) String fromDate,
                                             @RequestParam(defaultValue = "true") boolean toChecked,
                                             @RequestParam(required = false) String toDate,
                                             @RequestParam(required = false) String fromDocNo,
                                             @RequestParam(required = false) String toDocNo,
                                             @RequestParam(defaultValue = "0") int supplierId,
                                             @RequestParam(defaultValue = "0") int itemId,
                                             @RequestParam(defaultValue = "0") int gpId,
                                             @RequestParam(required = false) String approvedStatus,
                                             @RequestParam(required = false) String analystStatus,
                                             @RequestParam(required = false) String vehicleNo) {
        return service.history(fromChecked, fromDate, toChecked, toDate, fromDocNo, toDocNo, supplierId, itemId, gpId,
                approvedStatus, analystStatus, vehicleNo);
    }

    @GetMapping("/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    @GetMapping("/{id}/print-check")
    public Map<String, Object> printCheck(@PathVariable int id, @RequestParam(defaultValue = "false") boolean toolbar) {
        return service.checkPrint(id, toolbar);
    }

    /** The stored Analysis Pic ("analysis") or Cooking Pic ("cooking") of a record. */
    @GetMapping("/{id}/picture/{which}")
    public ResponseEntity<byte[]> picture(@PathVariable int id, @PathVariable String which) {
        Object[] p = service.picture(id, which);
        if (p == null) return ResponseEntity.notFound().build();
        String name = String.valueOf(p[0]).toLowerCase(Locale.ROOT);
        MediaType type = name.endsWith(".png") ? MediaType.IMAGE_PNG
                : name.endsWith(".bmp") ? MediaType.parseMediaType("image/bmp")
                : name.endsWith(".gif") ? MediaType.IMAGE_GIF : MediaType.IMAGE_JPEG;
        return ResponseEntity.ok().contentType(type).header("Cache-Control", "no-store").body((byte[]) p[1]);
    }

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody PurchaseAnalysisDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (PurchaseAnalysisService.ConfirmRequired e) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", false);
            m.put("confirm", true);
            m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.CONFLICT).body(m);
        }
    }

    @PostMapping("/{id}/cuts")
    public Map<String, Object> cuts(@PathVariable int id, @RequestBody PurchaseAnalysisDto.Cuts dto) { return service.updateCuts(id, dto); }

    @PostMapping("/{id}/parameter-results")
    public Map<String, Object> parameterResults(@PathVariable int id, @RequestBody List<PurchaseAnalysisDto.ParamResult> rows) {
        return service.updateParameterResults(id, rows);
    }

    /** A procedure's RAISERROR text — the message the desktop's message box shows (catch ex -> MessageBox.Show(ex.Message)). */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Purchase Analysis database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
