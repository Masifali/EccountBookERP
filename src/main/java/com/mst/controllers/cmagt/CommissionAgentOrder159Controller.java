package com.mst.controllers.cmagt;

import com.mst.services.cmagt.CommissionAgentOrder159Service;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.Collections;
import java.util.Map;

/** Browser route for the Commission Agent Trade order screen (desktop document 159). */
@Controller
public class CommissionAgentOrder159Controller {
    private final CommissionAgentOrder159Service service;
    public CommissionAgentOrder159Controller(CommissionAgentOrder159Service service){this.service=service;}

    @GetMapping("/commission/agent-trade/order")
    public String page(Model model){model.addAttribute("activeMenu","commission");model.addAttribute("moduleTitle","Commission Agent Order");return "cmagt/agent_trade_order_159";}

    @GetMapping("/commission/agent-trade/api/order/init") @ResponseBody
    public ResponseEntity<?> init(){return run(service::initialize);}
    @GetMapping("/commission/agent-trade/api/order/code") @ResponseBody
    public ResponseEntity<?> code(){return run(()->Map.of("docNo",service.generateCode()));}
    @GetMapping("/commission/agent-trade/api/order/history") @ResponseBody
    public ResponseEntity<?> history(){return run(service::history);}
    @GetMapping("/commission/agent-trade/api/order/item-uoms") @ResponseBody
    public ResponseEntity<?> itemUoms(@RequestParam int itemId){return run(()->service.itemUoms(itemId));}
    @GetMapping("/commission/agent-trade/api/order/{id}") @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id){return run(()->service.record(id));}
    @PostMapping("/commission/agent-trade/api/order/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String,Object> body){return run(()->service.save(body));}

    private static ResponseEntity<?> run(Call c){try{return ResponseEntity.ok(c.get());}
        catch(IllegalArgumentException e){return ResponseEntity.badRequest().body(Collections.singletonMap("message",e.getMessage()));}
        catch(org.springframework.security.access.AccessDeniedException e){return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Collections.singletonMap("message",e.getMessage()));}
        catch(Exception e){String m=e.getMessage()==null||e.getMessage().isBlank()?"Unable to process this commission agent order.":e.getMessage();return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(Collections.singletonMap("message",m));}}
    @FunctionalInterface private interface Call{Object get()throws Exception;}
}
