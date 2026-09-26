package com.mst.controllers.sale;

import com.mst.services.SaleGdnPurchaseReturnService;
import java.util.*;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller @RequestMapping("/sale/gdn-purchase-return/api")
public class SaleGdnPurchaseReturnController {
    private final SaleGdnPurchaseReturnService service;
    public SaleGdnPurchaseReturnController(SaleGdnPurchaseReturnService service){this.service=service;}
    @GetMapping("/initial") @ResponseBody public Map<String,Object> initial(){return service.initial();}
    @GetMapping("/items") @ResponseBody public List<Map<String,Object>> items(@RequestParam int supplierId){return service.items(supplierId);}
    @GetMapping("/uoms") @ResponseBody public List<Map<String,Object>> uoms(@RequestParam int itemId){return service.uoms(itemId);}
    @GetMapping("/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @GetMapping("/history") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
    @PostMapping @ResponseBody public Map<String,Object> save(@RequestBody Map<String,Object> request){request.put("id",0);return service.save(request);}
    @PutMapping("/{id:[0-9]+}") @ResponseBody public Map<String,Object> update(@PathVariable int id,@RequestBody Map<String,Object> request){request.put("id",id);return service.save(request);}
    @DeleteMapping("/{id:[0-9]+}") public ResponseEntity<Void> delete(@PathVariable int id){service.delete(id);return ResponseEntity.noContent().build();}
}
