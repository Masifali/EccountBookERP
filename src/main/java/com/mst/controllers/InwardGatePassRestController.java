package com.mst.controllers;

import com.mst.models.InwardGatePass;
import com.mst.security.CurrentUserContext;
import com.mst.services.InwardGatePassService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/inward-gate-pass")
public class InwardGatePassRestController {

    @Autowired
    private InwardGatePassService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/dropdowns")
    public Map<String, Object> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getDropdowns(orgId, compId);
    }

    @GetMapping("/open-records")
    public List<Map<String,Object>> getOpenGatePasses() { return service.getOpenGatePasses(); }

    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNumbers(
            @RequestParam(defaultValue = "51") Integer docTypeId,
            @RequestParam(defaultValue = "Paddy") String gatepassType) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.generateNextNumbers(orgId, compId, branchId, yearId, docTypeId, gatepassType);
    }

    @GetMapping("/{id:[0-9]+}")
    public Map<String, Object> getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @GetMapping("/order-party-items")
    public List<Map<String,Object>> getOrderPartyItems(@RequestParam(defaultValue="41") int documentTypeId,@RequestParam int number,@RequestParam(required=false) String date,@RequestParam(defaultValue="0") int gatePassId) {
        return service.getOrderPartyItems(documentTypeId,number,date,gatePassId);
    }

    @GetMapping("/lab-data/{id}")
    public Map<String,Object> getLabData(@PathVariable int id) {
        Map<String,Object> row=service.getLabData(id);
        return row==null?Map.of():row;
    }

    /** NoOfAttachments link: history=false -> grd (by RefDocumentTypeId 51), history=true -> grdhistory (by ScreenName). */
    @GetMapping("/{id:[0-9]+}/attachments")
    public List<Map<String,Object>> getAttachments(@PathVariable int id,@RequestParam(defaultValue="false") boolean history) {
        return service.getAttachments(id,history);
    }

    @GetMapping("/{id:[0-9]+}/attachments/{attachmentId:[0-9]+}")
    public org.springframework.http.ResponseEntity<byte[]> getAttachment(@PathVariable int id,@PathVariable int attachmentId,@RequestParam(defaultValue="false") boolean history) {
        var file=service.getAttachmentFile(id,attachmentId,history);
        return org.springframework.http.ResponseEntity.ok().contentType(org.springframework.http.MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options","nosniff")
                .header(org.springframework.http.HttpHeaders.CONTENT_DISPOSITION,org.springframework.http.ContentDisposition.attachment().filename(file.name(),java.nio.charset.StandardCharsets.UTF_8).build().toString())
                .cacheControl(org.springframework.http.CacheControl.noStore()).body(file.bytes());
    }

    @GetMapping("/history-suppliers")
    public List<Map<String,Object>> getHistorySuppliers() { return service.getHistorySuppliers(); }

    @GetMapping("/po-info-combos")
    public Map<String,Object> getPoInfoCombos(@RequestParam(defaultValue="41") int documentTypeId) { return service.getPoInfoCombos(documentTypeId); }

    @GetMapping("/transit-vehicles")
    public List<Map<String,Object>> getTransitVehicles(@RequestParam(defaultValue="0") int supplierId,@RequestParam(defaultValue="0") int orderId,@RequestParam(defaultValue="0") int gatePassId) {
        return service.getTransitVehicles(supplierId,orderId,gatePassId);
    }

    @Autowired
    private com.fasterxml.jackson.databind.ObjectMapper objectMapper;

    /** Body = the gate pass fields plus `confirmed` (confirmation keys the operator accepted) and `isApprovedChecked`. */
    @PostMapping("/save")
    public Map<String, Object> saveRecord(@RequestBody Map<String,Object> body) {
        Set<String> confirmed=new HashSet<>();
        if (body.get("confirmed") instanceof Collection) for (Object key:(Collection<?>)body.get("confirmed")) confirmed.add(String.valueOf(key));
        boolean approved=Boolean.TRUE.equals(body.get("isApprovedChecked"));
        InwardGatePass obj=objectMapper.copy().configure(com.fasterxml.jackson.databind.DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES,false).convertValue(body,InwardGatePass.class);
        return service.saveRecord(obj,confirmed,approved);
    }

    @PostMapping("/delete/{id}")
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.deleteRecord(id, orgId, compId);
    }

    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public List<Map<String, Object>> getHistory(
            @RequestBody(required = false) Map<String, Object> payload,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Double fromDocNo,
            @RequestParam(required = false) Double toDocNo,
            @RequestParam(required = false) Integer supplierId) {
        if (payload != null) {
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null) toDate = payload.get("toDate").toString();
            if (payload.get("fromDocNo") != null && !payload.get("fromDocNo").toString().isEmpty()) fromDocNo = Double.parseDouble(payload.get("fromDocNo").toString());
            if (payload.get("toDocNo") != null && !payload.get("toDocNo").toString().isEmpty()) toDocNo = Double.parseDouble(payload.get("toDocNo").toString());
            if (payload.get("supplierId") != null && !payload.get("supplierId").toString().isEmpty()) supplierId = Integer.parseInt(payload.get("supplierId").toString());
        }

        String dateField=payload!=null?Objects.toString(payload.get("dateField"),"docDate"):"docDate";
        return service.getHistory(fromDate, toDate, fromDocNo, toDocNo, supplierId,dateField);
    }

    @GetMapping("/driver-bio/cnic")
    public Map<String, Object> findDriverBioByCnic(@RequestParam String cnic) {
        return service.findDriverBioByCnic(cnic);
    }

    @GetMapping("/driver-bio/cell")
    public Map<String, Object> findDriverBioByCell(@RequestParam String cell) {
        return service.findDriverBioByCell(cell);
    }

    @RequestMapping(value = "/po-info", method = {RequestMethod.GET, RequestMethod.POST})
    public List<Map<String, Object>> getPoInfo(
            @RequestBody(required = false) Map<String, Object> payload,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Double fromDocNo,
            @RequestParam(required = false) Double toDocNo,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer documentTypeId,
            @RequestParam(required = false) Integer expiryDays,
            @RequestParam(required = false) String dateField) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        if (payload != null) {
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null) toDate = payload.get("toDate").toString();
            if (payload.get("fromDocNo") != null && !payload.get("fromDocNo").toString().isEmpty()) fromDocNo = Double.parseDouble(payload.get("fromDocNo").toString());
            if (payload.get("toDocNo") != null && !payload.get("toDocNo").toString().isEmpty()) toDocNo = Double.parseDouble(payload.get("toDocNo").toString());
            if (payload.get("supplierId") != null && !payload.get("supplierId").toString().isEmpty()) supplierId = Integer.parseInt(payload.get("supplierId").toString());
            if (payload.get("documentTypeId") != null && !payload.get("documentTypeId").toString().isEmpty()) documentTypeId = Integer.parseInt(payload.get("documentTypeId").toString());
            if (payload.get("expiryDays") != null && !payload.get("expiryDays").toString().isEmpty()) expiryDays = Integer.parseInt(payload.get("expiryDays").toString());
            if (payload.get("dateField") != null) dateField = payload.get("dateField").toString();
        }
        if (dateField == null || dateField.isBlank()) dateField = "docDate";
        return service.getPoInfoGrid(orgId, compId, fromDate, toDate, fromDocNo, toDocNo, supplierId, documentTypeId, expiryDays, dateField);
    }
}
