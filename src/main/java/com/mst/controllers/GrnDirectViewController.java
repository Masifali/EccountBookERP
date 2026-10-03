package com.mst.controllers;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.GrnDirectService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.Map;

/**
 * Goods Receipt Notes Direct - Architecture.WinApp.Purchase.InvFrmGRNDirect (ScreenDefinition 129 "frmGrnDirect",
 * DocumentTypeId 137, Purchase module 5). The page and its API; all business rules live in GrnDirectService.
 * Tenancy is the session's everywhere - no request parameter carries organization / company / branch / year / user.
 */
@Controller
public class GrnDirectViewController {

    private static final String API = "/api/purchase/grn-direct";

    private final GrnDirectService service;

    public GrnDirectViewController(GrnDirectService service) { this.service = service; }

    @GetMapping("/purchase/grn-direct")
    public String page(Model model) {
        service.requireView();
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Goods Receipt Notes Direct");
        model.addAttribute("screenId", 129);
        model.addAttribute("documentTypeId", 137);
        return "purchase/grn_direct";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(service::setup); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return HrmApi.run(service::refresh); }

    @GetMapping(API + "/code") @ResponseBody
    public ResponseEntity<?> code() { return HrmApi.run(service::code); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(itemId)); }

    @GetMapping(API + "/deduction-policy") @ResponseBody
    public ResponseEntity<?> deductionPolicy(@RequestParam(value = "date", required = false) String date,
                                             @RequestParam(value = "difference", defaultValue = "0") double difference) {
        return HrmApi.run(() -> service.deductionPolicy(date, difference));
    }

    @GetMapping(API + "/history-suppliers") @ResponseBody
    public ResponseEntity<?> historySuppliers() { return HrmApi.run(service::historySuppliers); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.history(body)); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    /** BtnLoadOrder_Click :4027 - the LoadPurchaseOrder list, then the picked orders' lines (LoadInGridDetail :4055). */
    @GetMapping(API + "/order-loader") @ResponseBody
    public ResponseEntity<?> orderLoader(@RequestParam(value = "supplierId", defaultValue = "0") int supplierId,
                                         @RequestParam(value = "fromDate", required = false) String fromDate,
                                         @RequestParam(value = "toDate", required = false) String toDate,
                                         @RequestParam(value = "fromDocNo", defaultValue = "0") int fromDocNo,
                                         @RequestParam(value = "toDocNo", defaultValue = "0") int toDocNo) {
        return HrmApi.run(() -> service.orderLoader(supplierId, fromDate, toDate, fromDocNo, toDocNo));
    }

    @GetMapping(API + "/order-lines") @ResponseBody
    public ResponseEntity<?> orderLines(@RequestParam(value = "ids", defaultValue = "") String ids) {
        return HrmApi.run(() -> service.orderLines(ids));
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.save(body)); }

    @PostMapping(API + "/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam("id") int id) { return HrmApi.run(() -> service.delete(id)); }
}
