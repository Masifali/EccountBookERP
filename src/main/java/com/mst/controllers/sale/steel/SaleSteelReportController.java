package com.mst.controllers.sale.steel;

import com.mst.services.sale.steel.SaleSteelReportService;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.util.*;

/**
 * Module 87 Sales Steel Reports: 557 / 558 / 559 / 560.
 *   GET /{report}/lookups   the form's Load lists
 *   GET /{report}/rows      the Show button (query = the form's filters)
 *   GET /sale-invoice-register-with-activities/combos   btnNew_Click (AllComboBind)
 *   GET /sale-order-slip-register/attachments           GetNoofAttachmentsByRefDocumentTypeID
 *   GET /dynamic-reports?folder=Sales_Steel|SalesDirect_Steel   CommonServices.DynamicReportsLoad
 */
@RestController
@RequestMapping("/api/sale/steel-reports")
public class SaleSteelReportController {
    private final SaleSteelReportService service;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    public SaleSteelReportController(SaleSteelReportService service) { this.service = service; }

    @GetMapping("/{report}/lookups")
    public Map<String, Object> lookups(@PathVariable("report") String report) { return service.lookups(report); }

    @GetMapping("/sale-invoice-register/rows")
    public List<Map<String, Object>> invoiceRows(@RequestParam Map<String, String> q) { return service.invoiceRegister(q); }

    @GetMapping("/sale-invoice-register-with-activities/combos")
    public Map<String, Object> activityCombos() { return service.activitiesCombos(); }

    @GetMapping("/sale-invoice-register-with-activities/rows")
    public Map<String, Object> activityRows(@RequestParam Map<String, String> q) { return service.activitiesGrid(q); }

    @GetMapping("/gp-outward-register/rows")
    public List<Map<String, Object>> gpRows(@RequestParam Map<String, String> q) { return service.gpRegister(q); }

    @GetMapping("/sale-order-slip-register/rows")
    public List<Map<String, Object>> orderRows(@RequestParam Map<String, String> q) { return service.orderRegister(q); }

    @GetMapping("/sale-order-slip-register/attachments")
    public List<Map<String, Object>> orderAttachments(@RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments("sale-order-slip-register", id, documentTypeId);
    }

    /** CommonServices.DynamicReportsLoad(folder): the *rpt files of <report root>/<folder>, shown without the extension. */
    @GetMapping("/dynamic-reports")
    public List<String> dynamicReports(@RequestParam("folder") String folder) {
        service.currentUser("sale-invoice-register");
        String f = folder == null ? "" : folder.trim();
        if (!SaleSteelReportService.FOLDERS.contains(f)) throw new IllegalArgumentException("Unknown report folder " + folder);
        List<String> out = new ArrayList<>();
        if (reportRoot != null && !reportRoot.trim().isEmpty()) {
            File[] files = new File(reportRoot, f).listFiles();
            if (files != null) {
                Arrays.sort(files, Comparator.comparing(x -> x.getName().toLowerCase(Locale.ROOT)));
                for (File x : files) {
                    String n = x.getName();
                    if (!x.isFile() || !n.toLowerCase(Locale.ROOT).endsWith("rpt")) continue;
                    int dot = n.lastIndexOf('.');
                    out.add(dot > 0 ? n.substring(0, dot) : n);
                }
            }
        }
        return out;
    }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public ResponseEntity<String> denied(org.springframework.security.access.AccessDeniedException e) {
        return ResponseEntity.status(403).contentType(MediaType.TEXT_PLAIN).body(e.getMessage());
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, org.springframework.dao.DataAccessException.class})
    public ResponseEntity<String> failed(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return ResponseEntity.badRequest().contentType(MediaType.TEXT_PLAIN).body(m);
    }
}
