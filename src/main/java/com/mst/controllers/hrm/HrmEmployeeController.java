package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.EmployeeRegistrationDto;
import com.mst.services.hrm.HrmEmployeeService;
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
 * HRM "Employee Management" (AppModules 2020) - screen 648 Employee Registration.
 *
 *   648 /hrm/employee-registration (?id=EmployeeId opens that employee)   frmEmployeeRegistration.cs
 *   API /api/hrm/employee/employee-registration/{setup|refresh|reset|salary-breakup|department-accounts|by-id|by-no|
 *                                                history|print-check|save}
 */
@Controller
public class HrmEmployeeController {

    private static final String API = "/api/hrm/employee/employee-registration";

    @Autowired private HrmEmployeeService service;

    @GetMapping("/hrm/employee-registration")
    public String employeeRegistrationPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/employee/employee_registration"; }

    /** frmEmployeeRegistration_Load. */
    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(() -> service.setup()); }

    /** btnRefresh_Click (combos). */
    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return HrmApi.run(() -> service.refresh()); }

    /** btnnew_Click (employee no, benefits, week days). */
    @GetMapping(API + "/reset") @ResponseBody
    public ResponseEntity<?> reset() { return HrmApi.run(() -> service.reset()); }

    /** cmbLocation_ValueChanged -> GridBindEmployeeSalary. */
    @GetMapping(API + "/salary-breakup") @ResponseBody
    public ResponseEntity<?> salaryBreakup(@RequestParam("locationId") int locationId) { return HrmApi.run(() -> service.salaryBreakup(locationId)); }

    /** cmbDepartment_Leave -> GetCaoByDepartmentId. */
    @GetMapping(API + "/department-accounts") @ResponseBody
    public ResponseEntity<?> departmentAccounts(@RequestParam("departmentId") int departmentId) { return HrmApi.run(() -> service.departmentAccounts(departmentId)); }

    /** RetrivedDataEmployeeRegistration(Id). */
    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.employee(id)); }

    /** txtEmployeeNoForUpdate_Leave -> GetEmployeeIdByEmployeeNo. */
    @GetMapping(API + "/by-no") @ResponseBody
    public ResponseEntity<?> byNo(@RequestParam("employeeNo") String employeeNo) { return HrmApi.run(() -> service.idByEmployeeNo(employeeNo)); }

    /** tabPage2 History -> GridBindEmployeeRegistration. */
    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history() { return HrmApi.run(() -> service.history()); }

    /** btnPrint / grid Print / btnPrintRegister: rows of Sp_genEmployee_SlipandRegister before the viewer opens. */
    @GetMapping(API + "/print-check") @ResponseBody
    public ResponseEntity<?> printCheck(@RequestParam(value = "employeeId", defaultValue = "0") int employeeId) { return HrmApi.run(() -> service.printCheck(employeeId)); }

    /** btnsave_Click / btnupdate_Click. */
    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody EmployeeRegistrationDto body) { return HrmApi.run(() -> service.saveEmployee(body)); }
}
