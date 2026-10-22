package com.mst.controllers.lab;
import com.mst.services.lab.PurchaseCutApprovalService;
import com.mst.services.lab.PurchaseCutApprovalService.Cut;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class PurchaseCutApprovalController {
    private final PurchaseCutApprovalService service;
    public PurchaseCutApprovalController(PurchaseCutApprovalService service){this.service=service;}
    @GetMapping("/dashboard/purchase-cut-approval") public String page(){return "dashboard/purchase_cut_approval";}
    @GetMapping("/api/dashboard/purchase-cut-approval") @ResponseBody public Map<String,Object> list(){return service.list();}
    @GetMapping("/api/dashboard/purchase-cut-approval/{id}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping("/api/dashboard/purchase-cut-approval") @ResponseBody public Map<String,Object> approve(@RequestBody Cut cut){return service.approve(cut);}
    @ExceptionHandler({IllegalArgumentException.class,AccessDeniedException.class}) @ResponseBody
    public ResponseEntity<Map<String,String>> error(RuntimeException e){return ResponseEntity.status(e instanceof AccessDeniedException?403:400).body(Map.of("message",e.getMessage()));}
}
