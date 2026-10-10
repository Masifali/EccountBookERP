package com.mst.controllers.sale.bk;

import com.mst.services.sale.bk.SaleBkPriceListService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/** Screen 764 PriceList (Architecture.WinApp.WholeSale.frmItemPricingSchedule), module 2042 Booking Office / Customer Portal Report. */
@Controller
public class SaleBkPriceListController {
    private final SaleBkPriceListService service;

    public SaleBkPriceListController(SaleBkPriceListService service) { this.service = service; }

    @GetMapping("/sale/reports/price-list")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Price List");
        return "sale/bk/price_list";
    }

    @GetMapping("/api/sale/bk/price-list/init")
    @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping("/api/sale/bk/price-list/previous")
    @ResponseBody
    public List<Map<String, Object>> previous(@RequestParam(value = "ids", required = false, defaultValue = "") String ids,
                                              @RequestParam(value = "categoryId", defaultValue = "0") int categoryId,
                                              @RequestParam(value = "typeId", defaultValue = "0") int typeId) {
        return service.previousPrices(ids, categoryId, typeId);
    }

    @GetMapping("/api/sale/bk/price-list/history")
    @ResponseBody
    public List<Map<String, Object>> history() { return service.history(); }

    @GetMapping("/api/sale/bk/price-list/edit")
    @ResponseBody
    public Map<String, Object> edit(@RequestParam("itemId") String itemId) { return service.editInfo(itemId); }

    @PostMapping("/api/sale/bk/price-list/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, String>> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", String.valueOf(e.getMessage())));
    }
}
