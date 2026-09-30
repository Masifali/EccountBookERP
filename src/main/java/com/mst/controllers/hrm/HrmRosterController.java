package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.RosterHistoryDto;
import com.mst.models.hrm.dto.RosterLoadDto;
import com.mst.models.hrm.dto.RosterSaveDto;
import com.mst.services.hrm.HrmRosterService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import static com.mst.services.hrm.HrmRosterService.SCREEN_DUTY_ROASTER;
import static com.mst.services.hrm.HrmRosterService.SCREEN_EMPLOYEE_ROASTER;

/**
 * HRM duty-roster pages and API (AppModules 2022 "Attendance Management").
 *
 *   656 /hrm/duty-roaster      frmDutyRoasterNew.cs   API /api/hrm/roster/duty-roaster/{setup|lists|employees|load|save|history|history-detail|by-id}
 *   657 /hrm/employee-roaster  frmEmployeeRoaster.cs  API /api/hrm/roster/employee-roaster/{setup|lists|load|save|history|history-detail|by-id}
 */
@Controller
public class HrmRosterController {

    private static final String DUTY = "/api/hrm/roster/duty-roaster";
    private static final String EMP = "/api/hrm/roster/employee-roaster";

    @Autowired private HrmRosterService service;

    // ------------------------------------------------------------------ 656 Duty Roaster

    @GetMapping("/hrm/duty-roaster")
    public String dutyRoasterPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/roster/duty-roaster"; }

    @GetMapping(DUTY + "/setup") @ResponseBody
    public ResponseEntity<?> dutySetup() { return HrmApi.run(() -> service.dutySetup()); }

    @GetMapping(DUTY + "/lists") @ResponseBody
    public ResponseEntity<?> dutyLists() { return HrmApi.run(() -> service.dutyLists()); }

    @GetMapping(DUTY + "/employees") @ResponseBody
    public ResponseEntity<?> dutyEmployees(@RequestParam(value = "sectionId", defaultValue = "0") int sectionId,
                                           @RequestParam(value = "locationId", defaultValue = "0") int locationId,
                                           @RequestParam(value = "departmentIds", required = false) String departmentIds) {
        return HrmApi.run(() -> service.dutyEmployees(sectionId, locationId, departmentIds));
    }

    @PostMapping(DUTY + "/load") @ResponseBody
    public ResponseEntity<?> dutyLoad(@RequestBody RosterLoadDto body) { return HrmApi.run(() -> service.load(SCREEN_DUTY_ROASTER, body)); }

    @PostMapping(DUTY + "/save") @ResponseBody
    public ResponseEntity<?> dutySave(@RequestBody RosterSaveDto body) { return HrmApi.run(() -> service.save(SCREEN_DUTY_ROASTER, body)); }

    @PostMapping(DUTY + "/history") @ResponseBody
    public ResponseEntity<?> dutyHistory(@RequestBody RosterHistoryDto body) { return HrmApi.run(() -> service.dutyHistory(body)); }

    @GetMapping(DUTY + "/history-detail") @ResponseBody
    public ResponseEntity<?> dutyHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> service.historyDetail(SCREEN_DUTY_ROASTER, id)); }

    @GetMapping(DUTY + "/by-id") @ResponseBody
    public ResponseEntity<?> dutyById(@RequestParam("id") long id) { return HrmApi.run(() -> service.dutyById(id)); }

    // ------------------------------------------------------------------ 657 Employee Roaster

    @GetMapping("/hrm/employee-roaster")
    public String employeeRoasterPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/roster/employee-roaster"; }

    @GetMapping(EMP + "/setup") @ResponseBody
    public ResponseEntity<?> employeeSetup() { return HrmApi.run(() -> service.employeeSetup()); }

    @GetMapping(EMP + "/lists") @ResponseBody
    public ResponseEntity<?> employeeLists() { return HrmApi.run(() -> service.employeeLists()); }

    @PostMapping(EMP + "/load") @ResponseBody
    public ResponseEntity<?> employeeLoad(@RequestBody RosterLoadDto body) { return HrmApi.run(() -> service.load(SCREEN_EMPLOYEE_ROASTER, body)); }

    @PostMapping(EMP + "/save") @ResponseBody
    public ResponseEntity<?> employeeSave(@RequestBody RosterSaveDto body) { return HrmApi.run(() -> service.save(SCREEN_EMPLOYEE_ROASTER, body)); }

    @GetMapping(EMP + "/history") @ResponseBody
    public ResponseEntity<?> employeeHistory() { return HrmApi.run(() -> service.employeeHistory()); }

    @GetMapping(EMP + "/history-detail") @ResponseBody
    public ResponseEntity<?> employeeHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> service.historyDetail(SCREEN_EMPLOYEE_ROASTER, id)); }

    @GetMapping(EMP + "/by-id") @ResponseBody
    public ResponseEntity<?> employeeById(@RequestParam("id") long id) { return HrmApi.run(() -> service.employeeById(id)); }
}
