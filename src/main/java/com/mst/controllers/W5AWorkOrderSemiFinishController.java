package com.mst.controllers;

import com.mst.services.W5AWorkOrderSemiFinishService;
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

import java.util.Map;

/**
 * Mfg Production hub (module 103) - screen 781 "Work Order For Semi Finish" (Architecture.WinApp.Mfg.frmWorkOrderForSemiFinish, DocumentTypeId 1621).
 * Page /production/mfg/work-order-semi-finish, JSON under /production/mfg/work-order-semi-finish/api. Notes: {@link W5AWorkOrderSemiFinishService}.
 */
@Controller
@RequestMapping("/production/mfg/work-order-semi-finish")
public class W5AWorkOrderSemiFinishController {

    private final W5AWorkOrderSemiFinishService service;
    public W5AWorkOrderSemiFinishController(W5AWorkOrderSemiFinishService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Work Order For Semi Finish");
        return "production/mfg/w5a_work_order_semi_finish";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/item-info") @ResponseBody
    public ResponseEntity<?> itemInfo(@RequestParam int itemId) { return W3BApi.run(() -> service.itemInfo(itemId), "Could not load the item."); }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    @GetMapping("/api/history-detail/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> historyDetail(@PathVariable int id) { return W3BApi.run(() -> service.historyDetail(id), "Could not load the detail."); }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.load(id), "Could not open that work order."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
