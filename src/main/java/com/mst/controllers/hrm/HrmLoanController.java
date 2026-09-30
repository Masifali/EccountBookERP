package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.HrmAdvanceDto;
import com.mst.models.hrm.dto.HrmLoanDto;
import com.mst.services.hrm.HrmLoanService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 * HRM "Loan Management" (AppModules 2024) pages and API.
 *
 *   663 /hrm/employee-loan      frmEmployeeLoan.cs     API /api/hrm/loan/employee-loan/{setup|employees|list|by-id|save|voucher-id}
 *   664 /hrm/employee-advance   frmEmployeeAdvance.cs  API /api/hrm/loan/employee-advance/{setup|code|employees|list|by-id|previous|save|slip-check}
 * Both pages load a record from ?id= (opened from Loan Approval / Advance Approval).
 */
@Controller
public class HrmLoanController {

    private static final String API = "/api/hrm/loan";

    @Autowired private HrmLoanService service;

    // ------------------------------------------------------------------ 663 Employee Loan

    @GetMapping("/hrm/employee-loan")
    public String employeeLoanPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/loan/employee-loan"; }

    @GetMapping(API + "/employee-loan/setup") @ResponseBody
    public ResponseEntity<?> loanSetup() { return HrmApi.run(() -> service.loanSetup()); }

    @GetMapping(API + "/employee-loan/employees") @ResponseBody
    public ResponseEntity<?> loanEmployees() { return HrmApi.run(() -> service.loanEmployees()); }

    @GetMapping(API + "/employee-loan/list") @ResponseBody
    public ResponseEntity<?> loanList() { return HrmApi.run(() -> service.loanHistory()); }

    @GetMapping(API + "/employee-loan/by-id") @ResponseBody
    public ResponseEntity<?> loanById(@RequestParam("id") int id) { return HrmApi.run(() -> service.loan(id)); }

    @PostMapping(API + "/employee-loan/save") @ResponseBody
    public ResponseEntity<?> loanSave(@RequestBody HrmLoanDto body) { return HrmApi.run(() -> service.saveLoan(body)); }

    @GetMapping(API + "/employee-loan/voucher-id") @ResponseBody
    public ResponseEntity<?> loanVoucherId(@RequestParam("id") int id) { return HrmApi.run(() -> service.loanVoucherId(id)); }

    // ------------------------------------------------------------------ 664 Employee Advance

    @GetMapping("/hrm/employee-advance")
    public String employeeAdvancePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/loan/employee-advance"; }

    @GetMapping(API + "/employee-advance/setup") @ResponseBody
    public ResponseEntity<?> advanceSetup() { return HrmApi.run(() -> service.advanceSetup()); }

    @GetMapping(API + "/employee-advance/code") @ResponseBody
    public ResponseEntity<?> advanceCode() { return HrmApi.run(() -> service.advanceCode()); }

    @GetMapping(API + "/employee-advance/employees") @ResponseBody
    public ResponseEntity<?> advanceEmployees() { return HrmApi.run(() -> service.advanceEmployees()); }

    @GetMapping(API + "/employee-advance/list") @ResponseBody
    public ResponseEntity<?> advanceList() { return HrmApi.run(() -> service.advanceHistory()); }

    @GetMapping(API + "/employee-advance/by-id") @ResponseBody
    public ResponseEntity<?> advanceById(@RequestParam("id") int id) { return HrmApi.run(() -> service.advance(id)); }

    @GetMapping(API + "/employee-advance/previous") @ResponseBody
    public ResponseEntity<?> advancePrevious(@RequestParam("employeeId") int employeeId, @RequestParam(value = "appliedOn", required = false) String appliedOn) {
        return HrmApi.run(() -> service.previousAdvance(employeeId, appliedOn));
    }

    @PostMapping(API + "/employee-advance/save") @ResponseBody
    public ResponseEntity<?> advanceSave(@RequestBody HrmAdvanceDto body) { return HrmApi.run(() -> service.saveAdvance(body)); }

    @GetMapping(API + "/employee-advance/slip-check") @ResponseBody
    public ResponseEntity<?> advanceSlipCheck(@RequestParam("id") int id) { return HrmApi.run(() -> service.advanceSlipCheck(id)); }
}
