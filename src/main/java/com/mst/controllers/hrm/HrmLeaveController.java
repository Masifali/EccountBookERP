package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.LeaveCplRequestSaveDto;
import com.mst.models.hrm.dto.LeaveCplSaveDto;
import com.mst.models.hrm.dto.LeaveOpeningSaveDto;
import com.mst.models.hrm.dto.LeaveRequestSaveDto;
import com.mst.services.hrm.HrmLeaveService;
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
 * HRM "Leave Management" pages and API.
 *
 *   660     /hrm/leave-opening       frmEmployeeLeaveOpening.cs      API /api/hrm/leave/leave-opening/{setup|combos|load|save}
 *   661     /hrm/employee-leave      frmEmployeeLeaveRequest.cs      API /api/hrm/leave/employee-leave/{setup|employee|balance|history|history-detail|by-id|save}
 *   662/463 /hrm/cpl-leave-opening   frmEmployeeCPLLeaveOpening.cs   API /api/hrm/leave/cpl-leave-opening/{setup|employees|history|by-id|save}
 *   461     /hrm/cpl-attendance      frmEmployeeCPLAttendance.cs     API /api/hrm/leave/cpl-attendance/{setup|load|history|by-id|save}
 *   462     /hrm/cpl-request         frmEmployeeCPLRequest.cs        API /api/hrm/leave/cpl-request/{setup|employee|cpl|history|by-id|save}
 */
@Controller
public class HrmLeaveController {

    private static final String API = "/api/hrm/leave";

    @Autowired private HrmLeaveService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/hrm/leave-opening")
    public String leaveOpeningPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/leave/leave-opening"; }

    @GetMapping("/hrm/employee-leave")
    public String employeeLeavePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/leave/employee-leave"; }

    @GetMapping("/hrm/cpl-leave-opening")
    public String cplLeaveOpeningPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/leave/cpl-leave-opening"; }

    @GetMapping("/hrm/cpl-attendance")
    public String cplAttendancePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/leave/cpl-attendance"; }

    @GetMapping("/hrm/cpl-request")
    public String cplRequestPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/leave/cpl-request"; }

    // ------------------------------------------------------------------ 660 Leave Opening

    @GetMapping(API + "/leave-opening/setup") @ResponseBody
    public ResponseEntity<?> openingSetup() { return HrmApi.run(() -> service.openingSetup()); }

    @GetMapping(API + "/leave-opening/combos") @ResponseBody
    public ResponseEntity<?> openingCombos() { return HrmApi.run(() -> service.openingCombos()); }

    @GetMapping(API + "/leave-opening/load") @ResponseBody
    public ResponseEntity<?> openingLoad(@RequestParam(value = "leaveQuotaId", defaultValue = "0") int leaveQuotaId,
                                         @RequestParam(value = "leaveTypeProfileId", defaultValue = "0") int leaveTypeProfileId,
                                         @RequestParam(value = "employeeId", defaultValue = "0") long employeeId) {
        return HrmApi.run(() -> service.openingLoad(leaveQuotaId, leaveTypeProfileId, employeeId));
    }

    @PostMapping(API + "/leave-opening/save") @ResponseBody
    public ResponseEntity<?> openingSave(@RequestBody LeaveOpeningSaveDto body) { return HrmApi.run(() -> service.openingSave(body)); }

    // ------------------------------------------------------------------ 661 Employee Leave

    @GetMapping(API + "/employee-leave/setup") @ResponseBody
    public ResponseEntity<?> leaveSetup() { return HrmApi.run(() -> service.leaveSetup()); }

    @GetMapping(API + "/employee-leave/employee") @ResponseBody
    public ResponseEntity<?> leaveEmployee(@RequestParam(value = "employeeId", defaultValue = "0") long employeeId) {
        return HrmApi.run(() -> service.leaveEmployee(employeeId));
    }

    @GetMapping(API + "/employee-leave/balance") @ResponseBody
    public ResponseEntity<?> leaveBalance(@RequestParam(value = "employeeId", defaultValue = "0") long employeeId,
                                          @RequestParam(value = "leaveQuotaId", defaultValue = "0") int leaveQuotaId,
                                          @RequestParam(value = "leaveTypeProfileId", defaultValue = "0") int leaveTypeProfileId) {
        return HrmApi.run(() -> service.leaveBalance(employeeId, leaveQuotaId, leaveTypeProfileId));
    }

    @GetMapping(API + "/employee-leave/history") @ResponseBody
    public ResponseEntity<?> leaveHistory() { return HrmApi.run(() -> service.leaveHistory()); }

    @GetMapping(API + "/employee-leave/history-detail") @ResponseBody
    public ResponseEntity<?> leaveHistoryDetail(@RequestParam("id") long id) { return HrmApi.run(() -> service.leaveHistoryDetail(id)); }

    @GetMapping(API + "/employee-leave/by-id") @ResponseBody
    public ResponseEntity<?> leaveById(@RequestParam("id") long id) { return HrmApi.run(() -> service.leaveById(id)); }

    @PostMapping(API + "/employee-leave/save") @ResponseBody
    public ResponseEntity<?> leaveSave(@RequestBody LeaveRequestSaveDto body) { return HrmApi.run(() -> service.leaveSave(body)); }

    // ------------------------------------------------------------------ 662 / 463 CPL Leave Opening

    @GetMapping(API + "/cpl-leave-opening/setup") @ResponseBody
    public ResponseEntity<?> cplOpeningSetup() { return HrmApi.run(() -> service.cplOpeningSetup()); }

    @GetMapping(API + "/cpl-leave-opening/employees") @ResponseBody
    public ResponseEntity<?> cplOpeningEmployees() { return HrmApi.run(() -> service.cplOpeningEmployees()); }

    @GetMapping(API + "/cpl-leave-opening/history") @ResponseBody
    public ResponseEntity<?> cplOpeningHistory() { return HrmApi.run(() -> service.cplOpeningHistory()); }

    @GetMapping(API + "/cpl-leave-opening/by-id") @ResponseBody
    public ResponseEntity<?> cplOpeningById(@RequestParam("id") long id) { return HrmApi.run(() -> service.cplOpeningById(id)); }

    @PostMapping(API + "/cpl-leave-opening/save") @ResponseBody
    public ResponseEntity<?> cplOpeningSave(@RequestBody LeaveCplSaveDto body) { return HrmApi.run(() -> service.cplOpeningSave(body)); }

    // ------------------------------------------------------------------ 461 CPL Attendance

    @GetMapping(API + "/cpl-attendance/setup") @ResponseBody
    public ResponseEntity<?> cplAttendanceSetup() { return HrmApi.run(() -> service.cplAttendanceSetup()); }

    @GetMapping(API + "/cpl-attendance/load") @ResponseBody
    public ResponseEntity<?> cplAttendanceLoad(@RequestParam(value = "employeeId", defaultValue = "0") long employeeId,
                                               @RequestParam(value = "month", defaultValue = "0") int month,
                                               @RequestParam(value = "year", defaultValue = "0") int year) {
        return HrmApi.run(() -> service.cplAttendanceLoad(employeeId, month, year));
    }

    @GetMapping(API + "/cpl-attendance/history") @ResponseBody
    public ResponseEntity<?> cplAttendanceHistory() { return HrmApi.run(() -> service.cplAttendanceHistory()); }

    @GetMapping(API + "/cpl-attendance/by-id") @ResponseBody
    public ResponseEntity<?> cplAttendanceById(@RequestParam("id") long id) { return HrmApi.run(() -> service.cplAttendanceById(id)); }

    @PostMapping(API + "/cpl-attendance/save") @ResponseBody
    public ResponseEntity<?> cplAttendanceSave(@RequestBody LeaveCplSaveDto body) { return HrmApi.run(() -> service.cplAttendanceSave(body)); }

    // ------------------------------------------------------------------ 462 CPL Request

    @GetMapping(API + "/cpl-request/setup") @ResponseBody
    public ResponseEntity<?> cplRequestSetup() { return HrmApi.run(() -> service.cplRequestSetup()); }

    @GetMapping(API + "/cpl-request/employee") @ResponseBody
    public ResponseEntity<?> cplRequestEmployee(@RequestParam(value = "employeeId", defaultValue = "0") long employeeId) {
        return HrmApi.run(() -> service.cplRequestEmployee(employeeId));
    }

    @GetMapping(API + "/cpl-request/cpl") @ResponseBody
    public ResponseEntity<?> cplRequestCpl(@RequestParam(value = "employeeId", defaultValue = "0") long employeeId) {
        return HrmApi.run(() -> service.cplRequestCpl(employeeId));
    }

    @GetMapping(API + "/cpl-request/history") @ResponseBody
    public ResponseEntity<?> cplRequestHistory() { return HrmApi.run(() -> service.cplRequestHistory()); }

    @GetMapping(API + "/cpl-request/by-id") @ResponseBody
    public ResponseEntity<?> cplRequestById(@RequestParam("id") long id) { return HrmApi.run(() -> service.cplRequestById(id)); }

    @PostMapping(API + "/cpl-request/save") @ResponseBody
    public ResponseEntity<?> cplRequestSave(@RequestBody LeaveCplRequestSaveDto body) { return HrmApi.run(() -> service.cplRequestSave(body)); }
}
