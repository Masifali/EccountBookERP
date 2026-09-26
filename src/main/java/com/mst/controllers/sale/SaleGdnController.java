package com.mst.controllers.sale;

import com.mst.services.SaleGdnService;
import java.util.*;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller @RequestMapping("/sale/gdn/api")
public class SaleGdnController {
    private final SaleGdnService service;
    public SaleGdnController(SaleGdnService service){this.service=service;}
    @GetMapping("/initial") @ResponseBody public Map<String,Object> initial(){return service.initial();}
    @GetMapping("/uoms") @ResponseBody public List<Map<String,Object>> uoms(@RequestParam int itemId){return service.uoms(itemId);}
    @GetMapping("/advance-orders") @ResponseBody public List<Map<String,Object>> orders(@RequestParam int customerId,@RequestParam(defaultValue="0") int gdnId){return service.advanceOrders(customerId,gdnId);}
    @GetMapping("/delivery-order/{id}") @ResponseBody public List<Map<String,Object>> order(@PathVariable int id,@RequestParam int customerId){return service.deliveryOrder(customerId,id);}
    @GetMapping("/gate-pass/{id}/customers") @ResponseBody public List<Map<String,Object>> customers(@PathVariable int id){return service.gatePassCustomers(id);}
    @GetMapping("/expenses") @ResponseBody public List<Map<String,Object>> expenses(@RequestParam String deliveryOrderIds){return service.expenses(deliveryOrderIds);}
    @GetMapping("/history") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
    @GetMapping("/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping @ResponseBody public Map<String,Object> save(@RequestBody Map<String,Object> r){r.put("id",0);return service.save(r);}
    @PutMapping("/{id:[0-9]+}") @ResponseBody public Map<String,Object> update(@PathVariable int id,@RequestBody Map<String,Object> r){r.put("id",id);return service.save(r);}
    @DeleteMapping("/{id:[0-9]+}") public ResponseEntity<Void> delete(@PathVariable int id){service.delete(id);return ResponseEntity.noContent().build();}
}
