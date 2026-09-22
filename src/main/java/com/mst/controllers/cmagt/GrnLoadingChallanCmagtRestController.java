package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.GrnLoadingChallanCmagtDto;
import com.mst.services.cmagt.GrnLoadingChallanCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/grn-loading-challan")
public class GrnLoadingChallanCmagtRestController {

    @Autowired
    private GrnLoadingChallanCmagtService service;

    /* Company and organization are NEVER taken from the request. They used to arrive as
       @RequestParam(defaultValue = "1"), which meant two things at once: a caller could read
       another company's data by appending ?companyId=, and a caller that omitted it silently
       queried company 1 - which in this database does not exist, so the screen showed nothing and
       said nothing. The desktop reads UserAccount.OrganizationId / .CompanyId and offers no
       override; this is that, enforced server-side. */
    @org.springframework.beans.factory.annotation.Autowired
    private com.mst.security.CurrentUserContext currentUserContext;

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody GrnLoadingChallanCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto));
    }

    /** The "Load Purchase Order" picker - pending POs available to this GRN. */
    @GetMapping("/pending-purchase-orders")
    public ResponseEntity<List<Map<String, Object>>> pendingPurchaseOrders(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer fromDocNo,
            @RequestParam(required = false) Integer toDocNo,
            @RequestParam(required = false) Integer recId,
            @RequestParam(required = false) Integer commissionAgentId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer itemId,
            @RequestParam(required = false) Integer deliveryToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        return ResponseEntity.ok(service.pendingPurchaseOrders(fromDate, toDate, fromDocNo,
                toDocNo, recId, commissionAgentId, supplierId, itemId, deliveryToPartyId,
                shipToAddress));
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

    /**
     * The complete "Load Purchase Order" payload: all five result sets, under the desktop's
     * own names. /pending-purchase-orders returns only the first and is kept for callers that
     * want just the order list.
     */
    @GetMapping("/pending-purchase-order-tables")
    public ResponseEntity<Map<String, List<Map<String, Object>>>> pendingPurchaseOrderTables(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer fromDocNo,
            @RequestParam(required = false) Integer toDocNo,
            @RequestParam(required = false) Integer recId,
            @RequestParam(required = false) Integer commissionAgentId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer itemId,
            @RequestParam(required = false) Integer deliveryToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        return ResponseEntity.ok(service.pendingPurchaseOrderTables(fromDate, toDate, fromDocNo,
                toDocNo, recId, commissionAgentId, supplierId, itemId, deliveryToPartyId,
                shipToAddress));
    }
}
