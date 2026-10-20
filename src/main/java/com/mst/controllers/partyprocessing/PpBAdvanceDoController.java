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


    @Autowired private PpBAdvanceDoService service;

    @GetMapping("/party-processing/advance-delivery-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppScreen", PpBAdvanceDoService.SCREEN);
        return "partyprocessing/advance-delivery-order";
    }

    @GetMapping("/api/party-processing/advance-delivery-order/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(service::setup); }

    @GetMapping("/api/party-processing/advance-delivery-order/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return HrmApi.run(service::refresh); }

    @GetMapping("/api/party-processing/advance-delivery-order/code") @ResponseBody
    public ResponseEntity<?> code() { return HrmApi.run(service::code); }

    @GetMapping("/api/party-processing/advance-delivery-order/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(itemId)); }

    @PostMapping("/api/party-processing/advance-delivery-order/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.stock(body)); }

    @PostMapping("/api/party-processing/advance-delivery-order/row-stock") @ResponseBody
    public ResponseEntity<?> rowStock(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.rowStock(body)); }

    @GetMapping("/api/party-processing/advance-delivery-order/invoice-setup") @ResponseBody
    public ResponseEntity<?> invoiceSetup() { return HrmApi.run(service::invoiceSetup); }

    @PostMapping("/api/party-processing/advance-delivery-order/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.invoices(body)); }

    @GetMapping("/api/party-processing/advance-delivery-order/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return HrmApi.run(service::historyCustomers); }

    @PostMapping("/api/party-processing/advance-delivery-order/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.history(body)); }

    @GetMapping("/api/party-processing/advance-delivery-order/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    @PostMapping("/api/party-processing/advance-delivery-order/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.save(body)); }

    @PostMapping("/api/party-processing/advance-delivery-order/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam("id") int id) { return HrmApi.run(() -> service.delete(id)); }

    @GetMapping("/api/party-processing/advance-delivery-order/slip") @ResponseBody
    public ResponseEntity<?> slip(@RequestParam("id") int id) { return HrmApi.run(() -> service.slip(id)); }
}
