package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpBGrnPurchaseService;
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

/** Party Processing "GRN For Purchase From Party Processing" (GRNForPurchaseFromPartyProcessing.cs), ScreenId 613, DocumentTypeId 217. */
@Controller
public class PpBGrnPurchaseController {


    @Autowired private PpBGrnPurchaseService service;

    @GetMapping("/party-processing/grn-purchase")
    public String page(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppScreen", PpBGrnPurchaseService.SCREEN);
        return "partyprocessing/grn-purchase";
    }

    @GetMapping("/api/party-processing/grn-purchase/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(service::setup); }

    @GetMapping("/api/party-processing/grn-purchase/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> service.refresh(recId)); }

    @GetMapping("/api/party-processing/grn-purchase/code") @ResponseBody
    public ResponseEntity<?> code() { return HrmApi.run(service::code); }

    @GetMapping("/api/party-processing/grn-purchase/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(itemId)); }

    @GetMapping("/api/party-processing/grn-purchase/loader-setup") @ResponseBody
    public ResponseEntity<?> loaderSetup() { return HrmApi.run(service::loaderSetup); }

    @PostMapping("/api/party-processing/grn-purchase/loader") @ResponseBody
    public ResponseEntity<?> loader(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.loader(body)); }

    @GetMapping("/api/party-processing/grn-purchase/history-suppliers") @ResponseBody
    public ResponseEntity<?> historySuppliers() { return HrmApi.run(service::historySuppliers); }

    @PostMapping("/api/party-processing/grn-purchase/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.history(body)); }

    @GetMapping("/api/party-processing/grn-purchase/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    @PostMapping("/api/party-processing/grn-purchase/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.save(body)); }

    @PostMapping("/api/party-processing/grn-purchase/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam("id") int id) { return HrmApi.run(() -> service.delete(id)); }

    @GetMapping("/api/party-processing/grn-purchase/slip") @ResponseBody
    public ResponseEntity<?> slip(@RequestParam("id") int id) { return HrmApi.run(() -> service.slip(id)); }
}
