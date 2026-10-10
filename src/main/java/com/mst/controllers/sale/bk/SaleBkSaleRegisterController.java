package com.mst.controllers.sale.bk;

import com.mst.services.sale.bk.SaleBkSaleRegisterService;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.util.*;

/** Screen 586 SaleInvoice_Register (Architecture.WinApp.Salt.Reports.SaleInvoice_Register), module 95 Sale Salt. */
@Controller
public class SaleBkSaleRegisterController {
    private final SaleBkSaleRegisterService service;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    public SaleBkSaleRegisterController(SaleBkSaleRegisterService service) { this.service = service; }

    @GetMapping("/sale/salt/salt-sale-invoice-register")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Register");
        return "sale/bk/sale_register_salt";
    }

    @GetMapping("/api/sale/bk/sale-register/lookups")
    @ResponseBody
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/api/sale/bk/sale-register/combos")
    @ResponseBody
    public Map<String, Object> combos() { return service.combos(); }

    @GetMapping("/api/sale/bk/sale-register/summary-combos")
    @ResponseBody
    public Map<String, Object> summaryCombos() { return service.summaryCombos(); }

    @GetMapping("/api/sale/bk/sale-register/history")
    @ResponseBody
    public List<Map<String, Object>> history(@RequestParam Map<String, String> q) { return service.history(q); }

    @GetMapping("/api/sale/bk/sale-register/summary")
    @ResponseBody
    public Map<String, Object> summary(@RequestParam Map<String, String> q) { return service.summary(q); }

    /** CommonServices.DynamicReportsLoad("Sales_Salt"): the *rpt files of the folder, shown without the extension. */
    @GetMapping("/api/sale/bk/sale-register/dynamic-reports")
    @ResponseBody
    public List<String> dynamicReports() {
        List<String> out = new ArrayList<>();
        if (reportRoot != null && !reportRoot.trim().isEmpty()) {
            File[] files = new File(reportRoot, "Sales_Salt").listFiles();
            if (files != null) {
                Arrays.sort(files, Comparator.comparing(f -> f.getName().toLowerCase(Locale.ROOT)));
                for (File f : files) {
                    String n = f.getName();
                    if (!f.isFile() || !n.toLowerCase(Locale.ROOT).endsWith("rpt")) continue;
                    int dot = n.lastIndexOf('.');
                    out.add(dot > 0 ? n.substring(0, dot) : n);
                }
            }
        }
        return out;
    }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, String>> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", String.valueOf(e.getMessage())));
    }
}
