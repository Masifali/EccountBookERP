package com.mst.controllers;

import com.mst.models.dto.VoucherValidationFilterDto;
import com.mst.models.dto.VoucherValidationReportDto;
import com.mst.services.VoucherValidationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/accounts/api/voucher-validation")
public class VoucherValidationRestController {

    @Autowired
    private VoucherValidationService voucherValidationService;

    @PostMapping("/report")
    public ResponseEntity<?> getReportData(@RequestBody VoucherValidationFilterDto filter) {
        try {
            List<VoucherValidationReportDto> data = voucherValidationService.getVoucherValidationReport(filter);
            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("data", data);
            response.put("totalRecords", data.size());
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            Map<String, Object> error = new HashMap<>();
            error.put("success", false);
            error.put("message", "Unable to load Voucher Validation Report: " + e.getMessage());
            return ResponseEntity.status(500).body(error);
        }
    }

    @GetMapping("/accounts")
    public ResponseEntity<?> getAccounts() {
        return ResponseEntity.ok(voucherValidationService.getAllDetailAccounts());
    }

    @GetMapping("/doc-types")
    public ResponseEntity<?> getDocTypes() {
        return ResponseEntity.ok(voucherValidationService.getDocumentTypes());
    }

    @GetMapping("/custom-groups")
    public ResponseEntity<?> getCustomGroups() {
        return ResponseEntity.ok(voucherValidationService.getCustomGroups());
    }
}
