package com.mst.controllers;

import com.mst.models.dto.PartyPaymentVoucherDto;
import com.mst.services.PartyPaymentVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequestMapping("/accounts/api/party-payment")
public class PartyPaymentRestController {

    @Autowired
    private PartyPaymentVoucherService partyPaymentVoucherService;

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextVoucherCode() {
        Map<String, Object> res = new HashMap<>();
        int code = partyPaymentVoucherService.generateNextVoucherCode();
        res.put("voucherCode", code);
        res.put("voucherCodeDisplay", String.format("PPV-2026-%04d", code));
        return ResponseEntity.ok(res);
    }

    @GetMapping("/dr-accounts")
    public ResponseEntity<?> searchDrAccounts(@RequestParam(value = "q", required = false) String query) {
        return ResponseEntity.ok(partyPaymentVoucherService.getDrAccounts(query));
    }

    @GetMapping("/cr-accounts")
    public ResponseEntity<?> searchCrAccounts(@RequestParam(value = "q", required = false) String query) {
        return ResponseEntity.ok(partyPaymentVoucherService.getCrAccounts(query));
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getVoucherById(@PathVariable("id") Integer id) {
        Map<String, Object> voucher = partyPaymentVoucherService.getVoucherById(id);
        if (voucher != null) {
            return ResponseEntity.ok(voucher);
        } else {
            return ResponseEntity.notFound().build();
        }
    }

    @PostMapping("/save")
    public ResponseEntity<?> saveVoucher(@RequestBody PartyPaymentVoucherDto dto) {
        Map<String, Object> res = partyPaymentVoucherService.saveVoucher(dto);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }
}
