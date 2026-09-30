package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.PayrollAdjustmentDto;
import com.mst.models.hrm.dto.PayrollAllowanceDto;
import com.mst.models.hrm.dto.PayrollLoanDeductionDto;
import com.mst.models.hrm.dto.PayrollPostingDto;
import com.mst.services.hrm.HrmPayrollService;
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
 * HRM "Payroll" (AppModules 2027) pages and API.
 *
 *   670 /hrm/payroll-posting            frmPayrollPosting.cs          API /api/hrm/payroll/payroll-posting/...
 *   671 /hrm/employee-allowance         frmEmployeeAllowance.cs       API /api/hrm/payroll/employee-allowance/...
 *   672 /hrm/employee-late-adjustment   frmEmployeeLateAdjustment.cs  API /api/hrm/payroll/employee-late-adjustment/...
 *   673 /hrm/employee-loan-deduction    EmployeeLoanDeduction.cs      API /api/hrm/payroll/employee-loan-deduction/...
 *   674 /hrm/employee-short-adjustment  EmployeeShortAdjustment.cs    API /api/hrm/payroll/employee-short-adjustment/...
 */
@Controller
public class HrmPayrollController {

    private static final String API = "/api/hrm/payroll";

    @Autowired private HrmPayrollService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/hrm/payroll-posting")
    public String postingPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/payroll/payroll-posting"; }

    @GetMapping("/hrm/employee-allowance")
    public String allowancePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/payroll/employee-allowance"; }

    @GetMapping("/hrm/employee-late-adjustment")
    public String latePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/payroll/employee-late-adjustment"; }

    @GetMapping("/hrm/employee-loan-deduction")
    public String loanPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/payroll/employee-loan-deduction"; }

    @GetMapping("/hrm/employee-short-adjustment")
    public String shortPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/payroll/employee-short-adjustment"; }

    // ------------------------------------------------------------------ 670 Payroll Posting

    @GetMapping(API + "/payroll-posting/setup") @ResponseBody
    public ResponseEntity<?> postingSetup() { return HrmApi.run(() -> service.postingSetup()); }

    @GetMapping(API + "/payroll-posting/refresh") @ResponseBody
    public ResponseEntity<?> postingRefresh() { return HrmApi.run(() -> service.postingRefresh()); }

    @GetMapping(API + "/payroll-posting/code") @ResponseBody
    public ResponseEntity<?> postingCode() { return HrmApi.run(() -> service.postingCode()); }

    @GetMapping(API + "/payroll-posting/show") @ResponseBody
    public ResponseEntity<?> postingShow(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId,
                                         @RequestParam(value = "categoryId", defaultValue = "0") int categoryId,
                                         @RequestParam(value = "month", defaultValue = "") String month,
                                         @RequestParam(value = "year", defaultValue = "0") int year,
                                         @RequestParam(value = "monthValue", defaultValue = "0") int monthValue,
                                         @RequestParam(value = "debitAccountId", defaultValue = "0") int debitAccountId,
                                         @RequestParam(value = "employeeId", defaultValue = "0") int employeeId) {
        return HrmApi.run(() -> service.postingShow(departmentId, categoryId, month, year, monthValue, debitAccountId, employeeId));
    }

    @GetMapping(API + "/payroll-posting/by-id") @ResponseBody
    public ResponseEntity<?> postingById(@RequestParam("id") int id) { return HrmApi.run(() -> service.posting(id)); }

    @GetMapping(API + "/payroll-posting/history") @ResponseBody
    public ResponseEntity<?> postingHistory() { return HrmApi.run(() -> service.postingHistory()); }

    @GetMapping(API + "/payroll-posting/voucher-head-id") @ResponseBody
    public ResponseEntity<?> postingVoucher(@RequestParam("id") int id) { return HrmApi.run(() -> service.postingVoucherHeadId(id)); }

    @GetMapping(API + "/payroll-posting/print-check") @ResponseBody
    public ResponseEntity<?> postingPrintCheck(@RequestParam(value = "payrollId", defaultValue = "0") int payrollId,
                                               @RequestParam(value = "month", defaultValue = "") String month,
                                               @RequestParam(value = "year", defaultValue = "0") int year,
                                               @RequestParam(value = "monthValue", defaultValue = "0") int monthValue) {
        return HrmApi.run(() -> service.postingPrintCheck(payrollId, month, year, monthValue));
    }

    @PostMapping(API + "/payroll-posting/save") @ResponseBody
    public ResponseEntity<?> postingSave(@RequestBody PayrollPostingDto body) { return HrmApi.run(() -> service.savePosting(body)); }

    // ------------------------------------------------------------------ 671 Employee Allowance

    @GetMapping(API + "/employee-allowance/setup") @ResponseBody
    public ResponseEntity<?> allowanceSetup() { return HrmApi.run(() -> service.allowanceSetup()); }

    @GetMapping(API + "/employee-allowance/employees") @ResponseBody
    public ResponseEntity<?> allowanceEmployees() { return HrmApi.run(() -> service.allowanceEmployees()); }

    @GetMapping(API + "/employee-allowance/benefits") @ResponseBody
    public ResponseEntity<?> allowanceBenefits(@RequestParam("employeeId") int employeeId) { return HrmApi.run(() -> service.allowanceBenefits(employeeId)); }

    @GetMapping(API + "/employee-allowance/by-employee") @ResponseBody
    public ResponseEntity<?> allowanceByEmployee(@RequestParam("employeeId") int employeeId) { return HrmApi.run(() -> service.allowancesOf(employeeId)); }

    @PostMapping(API + "/employee-allowance/save") @ResponseBody
    public ResponseEntity<?> allowanceSave(@RequestBody PayrollAllowanceDto body) { return HrmApi.run(() -> service.saveAllowance(body)); }

    // ------------------------------------------------------------------ 672 Employee Late Adjustment

    @GetMapping(API + "/employee-late-adjustment/setup") @ResponseBody
    public ResponseEntity<?> lateSetup() { return HrmApi.run(() -> service.lateSetup()); }

    @GetMapping(API + "/employee-late-adjustment/list") @ResponseBody
    public ResponseEntity<?> lateList() { return HrmApi.run(() -> service.lateList()); }

    @GetMapping(API + "/employee-late-adjustment/employees") @ResponseBody
    public ResponseEntity<?> lateEmployees() { return HrmApi.run(() -> service.lateEmployees()); }

    @GetMapping(API + "/employee-late-adjustment/by-id") @ResponseBody
    public ResponseEntity<?> lateById(@RequestParam("id") int id) { return HrmApi.run(() -> service.late(id)); }

    @PostMapping(API + "/employee-late-adjustment/save") @ResponseBody
    public ResponseEntity<?> lateSave(@RequestBody PayrollAdjustmentDto body) { return HrmApi.run(() -> service.saveLate(body)); }

    // ------------------------------------------------------------------ 673 Employee Loan Deduction

    @GetMapping(API + "/employee-loan-deduction/setup") @ResponseBody
    public ResponseEntity<?> loanSetup() { return HrmApi.run(() -> service.loanSetup()); }

    @GetMapping(API + "/employee-loan-deduction/pending") @ResponseBody
    public ResponseEntity<?> loanPending() { return HrmApi.run(() -> service.loanPending()); }

    @GetMapping(API + "/employee-loan-deduction/history") @ResponseBody
    public ResponseEntity<?> loanHistory() { return HrmApi.run(() -> service.loanHistory()); }

    @PostMapping(API + "/employee-loan-deduction/save") @ResponseBody
    public ResponseEntity<?> loanSave(@RequestBody PayrollLoanDeductionDto body) { return HrmApi.run(() -> service.saveLoan(body)); }

    // ------------------------------------------------------------------ 674 Employee Short Adjustment

    @GetMapping(API + "/employee-short-adjustment/setup") @ResponseBody
    public ResponseEntity<?> shortSetup() { return HrmApi.run(() -> service.shortSetup()); }

    @GetMapping(API + "/employee-short-adjustment/list") @ResponseBody
    public ResponseEntity<?> shortList() { return HrmApi.run(() -> service.shortList()); }

    @GetMapping(API + "/employee-short-adjustment/employees") @ResponseBody
    public ResponseEntity<?> shortEmployees() { return HrmApi.run(() -> service.shortEmployees()); }

    @GetMapping(API + "/employee-short-adjustment/by-id") @ResponseBody
    public ResponseEntity<?> shortById(@RequestParam("id") int id) { return HrmApi.run(() -> service.shortAdjustment(id)); }

    @PostMapping(API + "/employee-short-adjustment/save") @ResponseBody
    public ResponseEntity<?> shortSave(@RequestBody PayrollAdjustmentDto body) { return HrmApi.run(() -> service.saveShort(body)); }
}
