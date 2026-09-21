package com.mst.controllers.sale;

import com.mst.models.dto.SaleDriverBioRequest;
import com.mst.services.SaleDriverBioService;
import java.util.Map;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/sale/driver-bio/api")
public class SaleDriverBioController {
    private final SaleDriverBioService service;
    public SaleDriverBioController(SaleDriverBioService service){this.service=service;}
    @GetMapping("/initial") public Object initial(){return service.initial();}
    @GetMapping("/history") public Object history(){return service.history();}
    @GetMapping("/{id}") public Object record(@PathVariable int id){return service.record(id);}
    @PostMapping public Map<String,Object> save(@RequestBody SaleDriverBioRequest request){return service.save(request);}
}
