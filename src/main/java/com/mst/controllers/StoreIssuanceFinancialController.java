package com.mst.controllers;

import com.mst.services.StoreIssuanceFinancialService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.List;
import java.util.Map;

/**
 * 33 StoreIssuanceFinancial "Store Issuance (Financials)" - Architecture.WinApp.StoreManagement.StoreIssuanceFinancial,
 * DocumentTypeId 451. No tenant, user, year or document type is accepted from the request.
 */
@Controller
public class StoreIssuanceFinancialController {

    private static final String API = "/api/store/store-issuance-financial";
    private final StoreIssuanceFinancialService service;
    public StoreIssuanceFinancialController(StoreIssuanceFinancialService service) { this.service = service; }

    @GetMapping({"/store/store-issuance-financial", "/store/issuance-financial"})
    public String page(Model model) {
        service.user();
        model.addAttribute("activeMenu", "store");
        return "store/store_issuance_financial";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public Map<String, Object> setup() { return service.setup(); }

    @GetMapping(API + "/doc-no") @ResponseBody
    public Map<String, Object> docNo() { return service.docNo(); }

    @GetMapping(API + "/history") @ResponseBody
    public List<Map<String, Object>> history() { return service.history(); }

    @GetMapping(API + "/load") @ResponseBody
    public Map<String, Object> load(@RequestParam int id) { return service.load(id); }

    @PostMapping(API + "/update") @ResponseBody
    public Map<String, Object> update(@RequestBody Map<String, Object> body) { return service.update(body); }
}
