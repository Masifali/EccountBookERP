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
    private com.mst.services.cmagt.PurchaseOrderCmagtSaveService saveService;

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

    /**
     * Now on the complete contract: [cmagt].[USP_purchaseOrderMaster_InsertAndUpdate] with all
     * 42 parameters plus the seven child collections, DocumentTypeId 1052.
     *
     * The old path called the same master procedure but sent 24 parameters and wrote only the
     * detail rows, so eighteen values took the procedure's defaults and six child collections
     * were never written. It is kept, disabled, at /save-legacy-donotuse.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto dto) {
        return ResponseEntity.ok(saveService.saveOrUpdate(dto));
    }

    @PostMapping("/save-legacy-donotuse")
    public ResponseEntity<Map<String, Object>> saveLegacy(@RequestBody PurchaseOrderCmagtDto dto) {
        throw new UnsupportedOperationException(
                "Disabled: sent 24 of the procedure's 42 parameters and wrote only the detail "
              + "rows. Use POST /api/commission/purchase-order/save.");
    }

    /** FormHistory — filtered by CanViewAllRecord, as the desktop does. */
    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(saveService.formHistory(fromDate, toDate));
    }

    /** ReadById plus all seven child collections, same family Save writes to. */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(saveService.getById(id));
    }

    /**
     * DocumentNoFill -> BLL GenerateCode, @DocumentTypeId fixed at this form's own 1052.
     *
     * The page has called this since it was written; the endpoint did not exist, so every new
     * document opened with an empty Doc No. The number the procedure allocates at save time was
     * always the real one, so nothing was mis-numbered - the box was simply blank.
     */
    @GetMapping("/generate-no")
    public ResponseEntity<Map<String, Object>> generateNextNo() {
        Map<String, Object> r = new java.util.LinkedHashMap<>();
        r.put("docNo", saveService.generateNextDocNo());
        return ResponseEntity.ok(r);
    }

    /**
     * BLL DeleteByID - @EntryUserId, @Id, @Activity='DeleteById'. Not a JPA delete.
     *
     * The Delete button on this screen called DELETE /{id}, which was not mapped: every click
     * returned 405 and the page reported "Delete was refused." The desktop's own delete is a
     * procedure call that cascades to the child tables, so this maps to that and nothing else.
     */
    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> deleteRecord(@PathVariable Integer id) {
        Map<String, Object> r = saveService.deleteById(id);
        r.put("success", !"ERROR".equals(String.valueOf(r.get("status"))));
        return ResponseEntity.ok(r);
    }
}
