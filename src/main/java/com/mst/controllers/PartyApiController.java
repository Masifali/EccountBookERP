package com.mst.controllers;

import com.mst.models.SupCustBankDetail;
import com.mst.models.SupplierCustomer;
import com.mst.models.SupplierCustomerMultiLingo;
import com.mst.models.SupplierCustomerShipToAddress;
import com.mst.serviceInterface.ISupplierCustomerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/accounts/api/parties")
public class PartyApiController {

    @Autowired
    private ISupplierCustomerService supplierCustomerService;

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextCode(@RequestParam(value = "customerTypeId", required = false) Integer customerTypeId) {
        String code = supplierCustomerService.generateNextPartyCode(customerTypeId);
        Map<String, String> res = new HashMap<>();
        res.put("code", code);
        return ResponseEntity.ok(res);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getPartyDetails(@PathVariable("id") Integer id) {
        SupplierCustomer party = supplierCustomerService.getById(id);
        if (party == null) {
            return ResponseEntity.notFound().build();
        }
        Map<String, Object> res = new HashMap<>();
        res.put("party", party);
        res.put("bankDetails", supplierCustomerService.getBankDetailsByPartyId(id));
        res.put("shipToAddresses", supplierCustomerService.getShipToAddressesByPartyId(id));
        res.put("multiLingoNames", supplierCustomerService.getMultiLanguageNamesByPartyId(id));
        return ResponseEntity.ok(res);
    }

    @PostMapping("/{id}/bank-details")
    public ResponseEntity<?> saveBankDetails(@PathVariable("id") Integer id, @RequestBody List<SupCustBankDetail> bankDetails) {
        List<SupCustBankDetail> saved = supplierCustomerService.saveBankDetails(id, bankDetails);
        return ResponseEntity.ok(saved);
    }

    @PostMapping("/{id}/ship-to")
    public ResponseEntity<?> saveShipToAddresses(@PathVariable("id") Integer id, @RequestBody List<SupplierCustomerShipToAddress> addresses) {
        List<SupplierCustomerShipToAddress> saved = supplierCustomerService.saveShipToAddresses(id, addresses);
        return ResponseEntity.ok(saved);
    }

    @PostMapping("/{id}/multi-lingo")
    public ResponseEntity<?> saveMultiLingoNames(@PathVariable("id") Integer id, @RequestBody List<SupplierCustomerMultiLingo> names) {
        List<SupplierCustomerMultiLingo> saved = supplierCustomerService.saveMultiLanguageNames(id, names);
        return ResponseEntity.ok(saved);
    }
}
