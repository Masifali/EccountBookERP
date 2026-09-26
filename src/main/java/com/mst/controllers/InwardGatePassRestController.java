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

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @GetMapping("/order-party-items")
    public List<Map<String,Object>> getOrderPartyItems(@RequestParam int number,@RequestParam String date,@RequestParam(defaultValue="0") int gatePassId) {
        return service.getOrderPartyItems(number,date,gatePassId);
    }

    @GetMapping("/transit-vehicles")
    public List<Map<String,Object>> getTransitVehicles(@RequestParam(defaultValue="0") int supplierId,@RequestParam(defaultValue="0") int orderId,@RequestParam(defaultValue="0") int gatePassId) {
        return service.getTransitVehicles(supplierId,orderId,gatePassId);
    }

    @PostMapping("/save")
    public Map<String, Object> saveRecord(@RequestBody InwardGatePass obj) {
        return service.saveRecord(obj);
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
