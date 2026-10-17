package com.mst.controllers;

import com.mst.services.W6BFeedManualService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Production hub screen 929 "Production Manual" (Architecture.WinApp.FeedMill.Production.frmProductionManualFeed, DocumentTypeId 1122).
 * Page /production/manual-feed, JSON under /production/manual-feed/api. Notes and gaps: {@link W6BFeedManualService}.
 * The slip print is the existing GET /reports/print/1122-production-slip?branchesId=&documentTypeId=1122&id= (ConcretePrintController); the voucher print is CrystalPrint 'acc-103'.
 */
@Controller
@RequestMapping("/production/manual-feed")
public class W6BFeedManualController {

    private final W6BFeedManualService service;
    public W6BFeedManualController(W6BFeedManualService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Manual");
        return "production/w6b_feed_manual";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/boms-by-item") @ResponseBody
    public ResponseEntity<?> bomsByItem(@RequestParam int itemId) { return W3BApi.run(() -> service.bomsByItem(itemId), "Could not load the bills of material."); }

    @PostMapping("/api/bom-inputs") @ResponseBody
    public ResponseEntity<?> bomInputs(@RequestBody Map<String, Object> body) {
        return W3BApi.run(() -> {
            List<Integer> ids = new ArrayList<>();
            Object o = body.get("bomIds");
            if (o instanceof List) for (Object x : (List<?>) o) ids.add(x == null ? 0 : Integer.parseInt(String.valueOf(x).trim().isEmpty() ? "0" : String.valueOf(x).trim()));
            return service.inputsFromBom(ids);
        }, "Could not load the BOM inputs.");
    }

    @PostMapping("/api/item-rates") @ResponseBody
    public ResponseEntity<?> itemRates(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.itemRates(body), "Could not read the stock and rate."); }

    @GetMapping("/api/history-lookups") @ResponseBody
    public ResponseEntity<?> historyLookups() { return W3BApi.run(service::historyLookups, "Could not load the history lists."); }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int productionHeaderId, @RequestParam(defaultValue = "0") int productTypeId) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, productionHeaderId, productTypeId), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.read(id), "Could not open that record."); }

    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam int id) { return W3BApi.run(() -> service.voucherHead(id), "Could not find the voucher."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
