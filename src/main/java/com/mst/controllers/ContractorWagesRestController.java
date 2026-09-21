package com.mst.controllers;

import com.mst.services.ContractorWagesService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * Contractor Wages - Wages Account.
 * Ported from Architecture.WinApp.Contractor_Wages\frmContractorWagesAccount.cs.
 *
 * The desktop screen has no delete (grdWagesAccount.AllowDelete = False, form :437), so no delete
 * endpoint is exposed here.
 */
@RestController
@RequestMapping("/api/contractor-wages")
public class ContractorWagesRestController {

    @Autowired
    private ContractorWagesService contractorWagesService;

    /** GLAcoountBind(), form :126 */
    @GetMapping("/gl-accounts")
    public ResponseEntity<List<Map<String, Object>>> getGlAccounts() {
        return ResponseEntity.ok(contractorWagesService.getGlAccounts());
    }

    /** GetLookupsByTypeIdDt(10), form :142 */
    @GetMapping("/wages-types")
    public ResponseEntity<List<Map<String, Object>>> getWagesTypes() {
        return ResponseEntity.ok(contractorWagesService.getWagesTypes());
    }

    /** GetActivityNature(), form :158 */
    @GetMapping("/activity-natures")
    public ResponseEntity<List<Map<String, Object>>> getActivityNatures() {
        return ResponseEntity.ok(contractorWagesService.getActivityNatures());
    }

    /** FillGrid(), form :270 */
    @GetMapping("/accounts")
    public ResponseEntity<List<Map<String, Object>>> getWagesAccounts() {
        return ResponseEntity.ok(contractorWagesService.getWagesAccounts());
    }

    /** grdWagesAccount_DoubleClick loads from the already-fetched grid rows (form :362); this is
     *  the equivalent single-row read for a direct link to one record. */
    @GetMapping("/accounts/{id}")
    public ResponseEntity<Map<String, Object>> getWagesAccount(@PathVariable int id) {
        return ResponseEntity.ok(contractorWagesService.getWagesAccountById(id));
    }

    /** btnsave_Click (form :205) / UpdateWagesAccount (form :304) — one endpoint, because the
     *  desktop builds one payload and lets Id decide Insert or Update. */
    @PostMapping("/accounts/save")
    public ResponseEntity<Map<String, Object>> saveWagesAccount(@RequestBody Map<String, Object> body) {
        return ResponseEntity.ok(contractorWagesService.saveWagesAccount(body));
    }
}
