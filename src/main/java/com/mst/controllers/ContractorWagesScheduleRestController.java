package com.mst.controllers;

import com.mst.services.ContractorWagesScheduleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * Contractor Wages - Wages Rate Schedule.
 * Ported from Architecture.WinApp.Contractor_Wages\frmContractWagesSchedule.cs.
 *
 * The desktop form has no delete (no delete button and no delete path in the BLL), so none is
 * exposed here. Save and Update are one endpoint because the desktop builds one payload and lets
 * Id decide: btnsave_Click sets RecId = 0 first (:568-578), btnUpdate_Click does not (:580-590).
 */
@RestController
@RequestMapping("/api/contractor-wages/schedule")
public class ContractorWagesScheduleRestController {

    @Autowired
    private ContractorWagesScheduleService scheduleService;

    /** accountName(), form :229-268 */
    @GetMapping("/wages-accounts")
    public ResponseEntity<List<Map<String, Object>>> getWagesAccounts() {
        return ResponseEntity.ok(scheduleService.getWagesAccounts());
    }

    /** ComboBindFromWagesSchedule(), form :291-360 - Contractor + WagesAccount in one call */
    @GetMapping("/history-dropdowns")
    public ResponseEntity<Map<String, Object>> getHistoryDropdowns() {
        return ResponseEntity.ok(scheduleService.getHistoryDropdowns());
    }

    /** BindgrdWagesSchedule(WagesAccountId), form :643-704 */
    @GetMapping("/by-account/{wagesAccountId}")
    public ResponseEntity<List<Map<String, Object>>> getByAccount(@PathVariable int wagesAccountId) {
        return ResponseEntity.ok(scheduleService.getSchedulesByAccount(wagesAccountId));
    }

    /** ReadbyId(Id), form :585-614 */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable int id) {
        return ResponseEntity.ok(scheduleService.getScheduleById(id));
    }

    /** Insert(), form :518-566 */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> body) {
        return ResponseEntity.ok(scheduleService.saveSchedule(body));
    }

    // ---------------- Contractor Wise (frmContractWiseWagesSchedule.cs) ----------------

    /** ContractorFill(), form :221-256 */
    @GetMapping("/contractors")
    public ResponseEntity<List<Map<String, Object>>> getContractors() {
        return ResponseEntity.ok(scheduleService.getContractors());
    }

    /**
     * BindgrdWagesSchedule(WagesAccountId, ContractorId), form :570-632.
     * Either filter may be 0; the desktop loads as soon as EITHER combo has a value.
     */
    @GetMapping("/contractor-wise")
    public ResponseEntity<List<Map<String, Object>>> getContractorWise(
            @RequestParam(defaultValue = "0") int wagesAccountId,
            @RequestParam(defaultValue = "0") int contractorId) {
        return ResponseEntity.ok(scheduleService.getSchedulesContractorWise(wagesAccountId, contractorId));
    }

    /** CommonServices.GetWagesRate, used to auto-fill Company Rate (form :379-400) */
    @GetMapping("/wages-rate")
    public ResponseEntity<Map<String, Object>> getWagesRate(
            @RequestParam String effectedDate,
            @RequestParam(defaultValue = "0") double packUomFrom,
            @RequestParam(defaultValue = "0") int wagesAccountId,
            @RequestParam(defaultValue = "0") int contractorId) {
        return ResponseEntity.ok(scheduleService.getWagesRate(effectedDate, packUomFrom,
                                                              wagesAccountId, contractorId));
    }

    /** Insert(), frmContractWiseWagesSchedule.cs :748-800 */
    @PostMapping("/contractor-wise/save")
    public ResponseEntity<Map<String, Object>> saveContractorWise(@RequestBody Map<String, Object> body) {
        return ResponseEntity.ok(scheduleService.saveContractorWiseSchedule(body));
    }

    /**
     * grdWagesSchedule_ColumnButtonClick, form :631-634. The row's own EntryUserId is what the
     * desktop passes as @EntryUserId, so it is read from the grid row, not from the session.
     */
    @PostMapping("/{id}/approve")
    public ResponseEntity<Map<String, Object>> approve(@PathVariable int id,
                                                       @RequestBody Map<String, Object> body) {
        int rowEntryUserId = 0;
        Object v = body.get("entryUserId");
        if (v != null) {
            try { rowEntryUserId = (int) Double.parseDouble(String.valueOf(v).trim()); }
            catch (Exception ignored) { }
        }
        return ResponseEntity.ok(scheduleService.approve(id, rowEntryUserId));
    }
}
