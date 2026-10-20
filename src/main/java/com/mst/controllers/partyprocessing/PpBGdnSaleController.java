package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpBGdnSaleService;
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

/** Party Processing "GDN For Sale To Party Processing" (GDNForSaleToPartyProcessing.cs), ScreenId 615, DocumentTypeId 221. */
@Controller
public class PpBGdnSaleController {


    @Autowired private PpBGdnSaleService service;

    @GetMapping("/party-processing/gdn-sale")
    public String page(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppScreen", PpBGdnSaleService.SCREEN);
        return "partyprocessing/gdn-sale";
    }

    @GetMapping("/api/party-processing/gdn-sale/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(service::setup); }

    @GetMapping("/api/party-processing/gdn-sale/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> service.refresh(recId)); }

    @GetMapping("/api/party-processing/gdn-sale/code") @ResponseBody
    public ResponseEntity<?> code() { return HrmApi.run(service::code); }

    @GetMapping("/api/party-processing/gdn-sale/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(itemId)); }

    @PostMapping("/api/party-processing/gdn-sale/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.stock(body)); }

    @GetMapping("/api/party-processing/gdn-sale/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return HrmApi.run(service::historyCustomers); }

    @PostMapping("/api/party-processing/gdn-sale/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.history(body)); }

    @GetMapping("/api/party-processing/gdn-sale/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    @PostMapping("/api/party-processing/gdn-sale/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.save(body)); }

    @PostMapping("/api/party-processing/gdn-sale/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam("id") int id) { return HrmApi.run(() -> service.delete(id)); }

    @GetMapping("/api/party-processing/gdn-sale/slip") @ResponseBody
    public ResponseEntity<?> slip(@RequestParam("id") int id) { return HrmApi.run(() -> service.slip(id)); }
}
