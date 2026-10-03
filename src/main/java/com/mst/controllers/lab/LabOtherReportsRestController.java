package com.mst.controllers.lab;

import com.mst.services.lab.LabOtherReportsService;
import java.util.Locale;
import java.util.Map;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * Lab Report (module 1011) API. Desktop MessageBox texts are IllegalArgumentException (HTTP 400,
 * ApiExceptionAdvice); a missing View grant is AccessDeniedException (403).
 *
 *   /lab-purchase-report/*   627  InvLabPurchaseReport  "Lab Purchase Analysis Report (Not Use)"
 *   /lab-sale-register/*     628  InvLabSaleRegister    "Lab Sale Analysis Report"
 */
@RestController
@RequestMapping("/api/lab/reports")
public class LabOtherReportsRestController {

    private final LabOtherReportsService service;

    public LabOtherReportsRestController(LabOtherReportsService service) { this.service = service; }

    // ---------------------------------------------------------------- 627
    @GetMapping("/lab-purchase-report/init")
    public Map<String, Object> purchaseInit() { return service.purchaseInit(); }

    @PostMapping("/lab-purchase-report/rows")
    public Map<String, Object> purchaseRows(@RequestBody Map<String, Object> body) { return service.purchaseRows(body); }

    @GetMapping("/lab-purchase-report/{id:[0-9]+}/detail")
    public Map<String, Object> purchaseDetail(@PathVariable int id) { return service.purchaseDetail(id); }

    /** The stored Analysis Pic ("analysis") or Cooking Pic ("cooking") of a record; 404 when there is none. */
    @GetMapping("/lab-purchase-report/{id:[0-9]+}/picture/{which:analysis|cooking}")
    public ResponseEntity<byte[]> purchasePicture(@PathVariable int id, @PathVariable String which) {
        Object[] p = service.purchasePicture(id, which);
        if (p == null) return ResponseEntity.notFound().build();
        String name = String.valueOf(p[0]).toLowerCase(Locale.ROOT);
        MediaType type = name.endsWith(".png") ? MediaType.IMAGE_PNG
                : name.endsWith(".bmp") ? MediaType.parseMediaType("image/bmp")
                : name.endsWith(".gif") ? MediaType.IMAGE_GIF : MediaType.IMAGE_JPEG;
        return ResponseEntity.ok().contentType(type).header("Cache-Control", "no-store").body((byte[]) p[1]);
    }

    // ---------------------------------------------------------------- 628
    @GetMapping("/lab-sale-register/init")
    public Map<String, Object> saleInit() { return service.saleInit(); }

    @PostMapping("/lab-sale-register/rows")
    public Map<String, Object> saleRows(@RequestBody Map<String, Object> body) { return service.saleRows(body); }
}
