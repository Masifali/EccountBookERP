package com.mst.controllers;

import com.mst.services.LabourWagesService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * Labour Wages (Contractor Wages Bill, DocumentTypeId 101).
 * Ported from Architecture.WinApp.Contractor_Wages\frmwagesBillHeader.cs.
 *
 * Save and Update are one endpoint because the desktop builds one payload and lets Id decide
 * (BLL Save(), :358-370). There is no delete endpoint: the form has no delete path - its only
 * row-level removal is inside the unsaved grid.
 */
@RestController
@RequestMapping("/api/contractor-wages/labour-wages")
public class LabourWagesRestController {

    @Autowired
    private LabourWagesService labourWagesService;

    /** DocumentTypeForManualWages(), BLL :533-567 */
    @GetMapping("/document-types")
    public ResponseEntity<List<Map<String, Object>>> getDocumentTypes() {
        return ResponseEntity.ok(labourWagesService.getDocumentTypes());
    }

    /** GenerateCode(), BLL :376-418 */
    @GetMapping("/next-doc-no")
    public ResponseEntity<Map<String, Object>> getNextDocNo(@RequestParam int refDocumentTypeId) {
        return ResponseEntity.ok(Map.of("docNo", labourWagesService.generateDocNo(refDocumentTypeId)));
    }

    /** GetByID(), BLL :421-443 - header plus its rows. */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable int id) {
        return ResponseEntity.ok(labourWagesService.getById(id));
    }

    /** GetContractorWagesBillHeaderIdByDocNo(), BLL :445-488 - what a clickable doc code resolves to. */
    @GetMapping("/by-doc-no")
    public ResponseEntity<Map<String, Object>> getIdByDocNo(@RequestParam int refDocumentTypeId,
                                                            @RequestParam int docNo) {
        Integer id = labourWagesService.getIdByDocNo(refDocumentTypeId, docNo);
        return ResponseEntity.ok(id == null ? Map.of() : Map.of("id", id));
    }

    /** FormHistory(), BLL :569-611 - the footer History button. */
    @GetMapping("/form-history")
    public ResponseEntity<List<Map<String, Object>>> getFormHistory(
            @RequestParam(required = false) Integer noOfRecords) {
        return ResponseEntity.ok(labourWagesService.getFormHistory(noOfRecords));
    }

    /** GetHistory(), BLL :618+ - the History tab's own filters. */
    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer fromDocNo,
            @RequestParam(required = false) Integer toDocNo) {
        return ResponseEntity.ok(labourWagesService.getHistory(fromDate, toDate, fromDocNo, toDocNo));
    }

    /** Grid dropdowns - contractor list and the activities allowed for this reference doc type. */
    @GetMapping("/grid-lookups")
    public ResponseEntity<Map<String, Object>> getGridLookups(@RequestParam int refDocumentTypeId) {
        return ResponseEntity.ok(labourWagesService.getGridLookups(refDocumentTypeId));
    }

    /** CommonServices.GetWagesRate (form :977). */
    @GetMapping("/wages-rate")
    public ResponseEntity<Map<String, Object>> getWagesRate(
            @RequestParam(required = false) String docDate,
            @RequestParam(defaultValue = "0") double packSize,
            @RequestParam(defaultValue = "0") int wagesAccountId,
            @RequestParam(defaultValue = "0") int contractorId) {
        return ResponseEntity.ok(labourWagesService.getWagesRate(docDate, packSize, wagesAccountId, contractorId));
    }


    /** GlobalVariables_Helper config values the form's calculations depend on. */
    @GetMapping("/config-flags")
    public ResponseEntity<Map<String, Object>> getConfigFlags() {
        return ResponseEntity.ok(labourWagesService.getConfigFlags());
    }

    /** "Pending Data For Load" - PendingTicket() / PendingGrnAndGdn() on the form. */
    @GetMapping("/pending")
    public ResponseEntity<List<Map<String, Object>>> getPending(
            @RequestParam(defaultValue = "0") int refDocumentTypeId,
            @RequestParam(defaultValue = "0") int refDocId,
            @RequestParam(required = false) String reqType) {
        return ResponseEntity.ok(labourWagesService.getPending(refDocumentTypeId, refDocId, reqType));
    }

    /** Already billed?  GetIdByRefDocTypeIdAndRefDocId - a hit switches the form to Update. */
    @GetMapping("/by-ref-doc")
    public ResponseEntity<Map<String, Object>> getIdByRefDoc(
            @RequestParam int refDocumentTypeId,
            @RequestParam int refDocNoId,
            @RequestParam(required = false) String reqType) {
        Integer id = labourWagesService.getIdByRefDoc(refDocumentTypeId, refDocNoId, reqType);
        return ResponseEntity.ok(id == null ? Map.of() : Map.of("id", id));
    }

    /** LoadDataForWages() - the chosen document's rows, each flagged free-of-cost or not. */
    @GetMapping("/load-ref-doc")
    public ResponseEntity<Map<String, Object>> loadRefDoc(
            @RequestParam int refDocumentTypeId,
            @RequestParam int refDocId,
            @RequestParam(required = false) String reqType) {
        return ResponseEntity.ok(labourWagesService.loadRefDoc(refDocumentTypeId, refDocId, reqType));
    }

    /** USP_CheckItemsFreeofcostforWages - re-asked when a row's wages activity changes (:885). */
    @GetMapping("/free-of-cost")
    public ResponseEntity<Map<String, Object>> checkFreeOfCost(
            @RequestParam(required = false) String docDate,
            @RequestParam int refDocumentTypeId,
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(defaultValue = "0") int wagesAccountId) {
        return ResponseEntity.ok(Map.of("isFreeOfCost",
                labourWagesService.checkFreeOfCost(docDate, refDocumentTypeId, itemId, wagesAccountId)));
    }

    /** Insert()/Update() on the form. Posts a GL voucher - see LabourWagesService.save. */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> body) {
        return ResponseEntity.ok(labourWagesService.save(body));
    }
}
