package com.mst.controllers;

import com.mst.services.PaddyGrnGhallaService;
import java.util.List;
import java.util.Map;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/** Screen 469 frmGrnForGhallaMandi (Paddy Purchase, module 48): page /purchase/grn-for-ghalla-mandi, API /api/paddy-purchase/grn-ghalla/*. */
@Controller
public class PaddyGrnGhallaController {
    @GetMapping("/purchase/grn-for-ghalla-mandi")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Goods Receiving Notes For Ghalla Mandi");
        return "purchase/paddy_grn_ghalla";
    }

    @RestController
    @RequestMapping("/api/paddy-purchase/grn-ghalla")
    public static class Api implements PaddyPurchaseApi {
        @Autowired private PaddyGrnGhallaService service;

        @GetMapping("/init") public Map<String, Object> init() { return service.init(); }
        @GetMapping("/lookups") public Map<String, Object> lookups() { return service.lookups(); }
        @GetMapping("/config") public Map<String, Object> config() { return service.config(); }
        @GetMapping("/doc-no") public Map<String, Object> docNo() { return Map.of("docNo", service.nextDocNo()); }
        @GetMapping("/gate-passes") public List<Map<String, Object>> gatePasses() { return service.pendingGatePasses(); }
        @GetMapping("/uoms") public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }
        @GetMapping("/history") public List<Map<String, Object>> history(@RequestParam(defaultValue = "0") int noOfRecords) { return service.history(noOfRecords); }
        @GetMapping("/history-detail/{id:[0-9]+}") public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }
        @GetMapping("/{id:[0-9]+}") public Map<String, Object> byId(@PathVariable int id) { return service.getById(id); }
        @PostMapping("/save") public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }
        @GetMapping("/{id:[0-9]+}/attachments") public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachments(id); }

        @GetMapping("/{id:[0-9]+}/attachments/{attachmentId:[0-9]+}")
        public ResponseEntity<byte[]> attachment(@PathVariable int id, @PathVariable int attachmentId) {
            var file = service.attachmentFile(id, attachmentId);
            return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                    .header("X-Content-Type-Options", "nosniff")
                    .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), java.nio.charset.StandardCharsets.UTF_8).build().toString())
                    .cacheControl(CacheControl.noStore()).body(file.bytes());
        }
    }
}
