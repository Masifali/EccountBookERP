package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.SupplierOfferCmagtDto;
import com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto;
import com.mst.services.cmagt.SupplierOfferCmagtSaveService;
import com.mst.security.CurrentUserContext;
import com.mst.services.cmagt.SupplierOfferCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/commission/supplier-offer")
public class SupplierOfferCmagtRestController {

    @org.springframework.beans.factory.annotation.Autowired
    private SupplierOfferCmagtSaveService saveService;


    @Autowired
    private SupplierOfferCmagtService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** DocumentNoFill -> BLL GenerateCode, @DocumentTypeId fixed at this form's own 1051. */
    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNo() {
        Map<String, Object> r = new java.util.LinkedHashMap<>();
        r.put("docNo", saveService.generateNextDocNo());
        return r;
    }

    /**
     * ReadById plus all seven child collections, through
     * [cmagt].[USP_purchaseOrderMaster_GetAllMethod] - the same family Save now writes to.
     * It previously read [cmagt].[USP_SupplierOfferMaster_GetAllMethod], so the screen would
     * have saved to one table family and read from another.
     */
    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable Integer id) {
        return saveService.getById(id);
    }

    /**
     * Supplier Offer save — now on the SAME contract the desktop uses.
     *
     * frmSupplierOfferCmagt:3357 calls purchaseOrderMaster.Save(obj) with DocumentTypeId 1051.
     * This endpoint previously posted a SupplierOfferCmagtDto into
     * [cmagt].[USP_SupplierOfferMaster_InsertAndUpdate], a procedure family belonging to a BLL
     * and model that NO desktop form constructs — so a web-saved offer landed in a different
     * table family from the one the desktop reads.
     *
     * The old endpoint is kept, disabled, at /save-legacy-donotuse so nothing silently keeps
     * writing to the orphaned family.
     */
    @PostMapping("/save")
    public Map<String, Object> saveRecord(@RequestBody PurchaseOrderMasterCmagtDto dto) {
        return saveService.saveOrUpdate(dto);
    }

    @PostMapping("/save-legacy-donotuse")
    public Map<String, Object> saveRecordLegacy(@RequestBody SupplierOfferCmagtDto dto) {
        throw new UnsupportedOperationException(
                "Disabled: [cmagt].[USP_SupplierOfferMaster_InsertAndUpdate] belongs to a BLL/model "
              + "no desktop form constructs. Use POST /api/commission/supplier-offer/save, which "
              + "writes through [cmagt].[USP_purchaseOrderMaster_InsertAndUpdate] with "
              + "DocumentTypeId 1051, as frmSupplierOfferCmagt does.");
    }

    /** BLL DeleteByID - @EntryUserId, @Id, @Activity='DeleteById'. Not a JPA delete. */
    @RequestMapping(value = {"/delete/{id}", "/{id}"},
                    method = {RequestMethod.POST, RequestMethod.DELETE})
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        Map<String, Object> r = saveService.deleteById(id);
        r.put("success", !"ERROR".equals(String.valueOf(r.get("status"))));
        return r;
    }

    /**
     * FormHistory — [cmagt].[USP_purchaseOrderMaster_GetAllMethod], DocumentTypeId 1051.
     *
     * Six parameters are always sent, including @CanViewAllRecord; @EntryUserId, @FromDate and
     * @ToDate are guarded in the BLL and omitted when unset. When the right is absent the
     * desktop pins EntryUser to the current user, so the operator sees only their own
     * documents — reproduced in the service.
     *
     * The old body read the orphaned SupplierOffer family and took its own tenancy arguments.
     */
    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public List<Map<String, Object>> getHistory(
            @RequestBody(required = false) Map<String, Object> payload,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        if (payload != null) {
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null)   toDate   = payload.get("toDate").toString();
        }
        return saveService.formHistory(fromDate, toDate);
    }

}
