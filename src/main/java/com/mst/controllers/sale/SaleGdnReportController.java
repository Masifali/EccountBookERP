package com.mst.controllers.sale;

import com.mst.models.SaleGdnReportFilter;
import com.mst.services.SaleGdnReportService;
import java.util.*;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/sale/reports/gdn-report/api")
public class SaleGdnReportController {
    private final SaleGdnReportService service;
    public SaleGdnReportController(SaleGdnReportService service){this.service=service;}
    @GetMapping("/initial") public Map<String,Object> initial(){return service.initial();}
    @GetMapping("/lookups") public List<Map<String,Object>> lookups(@RequestParam List<Integer> branchIds){return service.lookups(branchIds);}
    @PostMapping("/rows") public List<Map<String,Object>> rows(@RequestBody SaleGdnReportFilter filter){return service.history(filter);}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(IllegalArgumentException e){return ResponseEntity.badRequest().body(Map.of("message",e.getMessage()));}
    @ExceptionHandler(AccessDeniedException.class) public ResponseEntity<?> denied(AccessDeniedException e){return ResponseEntity.status(403).body(Map.of("message",e.getMessage()));}
}
