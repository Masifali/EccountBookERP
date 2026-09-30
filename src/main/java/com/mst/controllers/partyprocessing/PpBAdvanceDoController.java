package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpBAdvanceDoService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/** Party Processing "Advance Delivery Order" (AdvanceDeliveryOrderPP.cs), ScreenId 875, DocumentTypeId 910. */
@Controller
public class PpBAdvanceDoController {

    private static final String API = "/api/party-processing/advance-delivery-order";

    @Autowired private PpBAdvanceDoService service;

    @GetMapping("/party-processing/advance-delivery-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppScreen", PpBAdvanceDoService.SCREEN);
        return "partyprocessing/advance-delivery-order";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(service::setup); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return HrmApi.run(service::refresh); }

    @GetMapping(API + "/code") @ResponseBody
    public ResponseEntity<?> code() { return HrmApi.run(service::code); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(itemId)); }

    @PostMapping(API + "/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.stock(body)); }

    @PostMapping(API + "/row-stock") @ResponseBody
    public ResponseEntity<?> rowStock(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.rowStock(body)); }

    @GetMapping(API + "/invoice-setup") @ResponseBody
    public ResponseEntity<?> invoiceSetup() { return HrmApi.run(service::invoiceSetup); }

    @PostMapping(API + "/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.invoices(body)); }

    @GetMapping(API + "/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return HrmApi.run(service::historyCustomers); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.history(body)); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.save(body)); }

    @PostMapping(API + "/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam("id") int id) { return HrmApi.run(() -> service.delete(id)); }

    @GetMapping(API + "/slip") @ResponseBody
    public ResponseEntity<?> slip(@RequestParam("id") int id) { return HrmApi.run(() -> service.slip(id)); }
}
