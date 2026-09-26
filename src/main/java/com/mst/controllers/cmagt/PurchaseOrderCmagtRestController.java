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
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String dateType,
            @RequestParam(required = false) String validityFrom,
            @RequestParam(required = false) String validityTo,
            @RequestParam(required = false) Integer commissionAgentId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer itemId,
            @RequestParam(required = false) String parentItemIds,
            @RequestParam(required = false) Integer deliveryToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        /* HistoryFill's optional filters (frmPurchaseOrderCmagt.cs:3931-3993). All optional, so
           the existing ?fromDate=&toDate= call is unchanged. */
        Map<String, Object> extra = new java.util.LinkedHashMap<>();
        extra.put("dateType", dateType);
        extra.put("ValidityDateFrom", validityFrom);
        extra.put("ValidityDateTo", validityTo);
        extra.put("CommissionAgentId", commissionAgentId);
        extra.put("SupplierId", supplierId);
        extra.put("ItemId", itemId);
        extra.put("ParentItemIds", parentItemIds);
        extra.put("DeliveryToPartyId", deliveryToPartyId);
        extra.put("ShipToAddress", shipToAddress);
        return ResponseEntity.ok(saveService.formHistory(fromDate, toDate, extra));
    }

    /**
     * The procedures refuse with RAISERROR - approved record, referred in GRN Loading, document
     * date outside the active financial year, order weight below the weight already used. The
     * desktop shows ex.Message. Without this the refusal surfaced as a bare 500 and the page
     * could only say "Internal Server Error". Scoped to this controller only.
     */
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<Map<String, Object>> procedureRefused(org.springframework.dao.DataAccessException e) {
        Throwable t = e.getMostSpecificCause() != null ? e.getMostSpecificCause() : e;
        String msg = t.getMessage() == null ? "The database refused the operation." : t.getMessage().trim();
        Map<String, Object> body = new java.util.LinkedHashMap<>();
        body.put("success", false);
        body.put("status", "ERROR");
        body.put("message", msg);
        body.put("error", msg);
        return ResponseEntity.badRequest().body(body);
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
