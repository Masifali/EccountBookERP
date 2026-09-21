package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderCmagtDto;
import com.mst.services.cmagt.PurchaseOrderCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/purchase-order")
public class PurchaseOrderCmagtRestController {

    @Autowired
    private PurchaseOrderCmagtService service;

    /* Company and organization are NEVER taken from the request. They used to arrive as
       @RequestParam(defaultValue = "1"), which meant two things at once: a caller could read
       another company's data by appending ?companyId=, and a caller that omitted it silently
       queried company 1 - which in this database does not exist, so the screen showed nothing and
       said nothing. The desktop reads UserAccount.OrganizationId / .CompanyId and offers no
       override; this is that, enforced server-side. */
    @org.springframework.beans.factory.annotation.Autowired
    private com.mst.security.CurrentUserContext currentUserContext;

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody PurchaseOrderCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto));
    }

    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.getHistory(currentUserContext.currentCompanyId(), currentUserContext.currentOrganizationId(), fromDate, toDate));
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(service.getById(id));
    }
}
