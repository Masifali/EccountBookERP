package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.OvertimeEmployeeDto;
import com.mst.models.hrm.dto.OvertimeRequestDto;
import com.mst.services.hrm.HrmOvertimeService;
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
 * HRM "Over Time Management" (AppModules 2025) pages and API.
 *
 *   665 /hrm/over-time-request    OverTimeRequest.cs (+ EmpOverTimeLoadForRequest.cs as a modal)
 *       API /api/hrm/overtime/over-time-request/{setup|combos|by-id|save|history|history-detail|slip-check|loader-setup|loader-show}
 *   666 /hrm/employee-over-time   frmEmployeeOverTime.cs
 *       API /api/hrm/overtime/employee-over-time/{setup|history-combos|employees|detail|save|history|slip-check}
 */
@Controller
public class HrmOvertimeController {

    private static final String API = "/api/hrm/overtime";
    private static final String REQ = API + "/over-time-request";
    private static final String EMP = API + "/employee-over-time";

    @Autowired private HrmOvertimeService service;

    // ------------------------------------------------------------------ 665 Over Time Request

    @GetMapping("/hrm/over-time-request")
    public String overTimeRequestPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/overtime/over-time-request"; }

    @GetMapping(REQ + "/setup") @ResponseBody
    public ResponseEntity<?> requestSetup() { return HrmApi.run(() -> service.requestSetup()); }

    @GetMapping(REQ + "/combos") @ResponseBody
    public ResponseEntity<?> requestCombos() { return HrmApi.run(() -> service.requestCombos()); }

    @GetMapping(REQ + "/by-id") @ResponseBody
    public ResponseEntity<?> requestById(@RequestParam("id") long id) { return HrmApi.run(() -> service.request(id)); }

    @PostMapping(REQ + "/save") @ResponseBody
    public ResponseEntity<?> requestSave(@RequestBody OvertimeRequestDto body) { return HrmApi.run(() -> service.saveRequest(body)); }

    @GetMapping(REQ + "/history") @ResponseBody
    public ResponseEntity<?> requestHistory(@RequestParam(value = "mode", required = false) String mode,
                                            @RequestParam(value = "from", required = false) String from,
                                            @RequestParam(value = "to", required = false) String to,
                                            @RequestParam(value = "sectionId", defaultValue = "0") int sectionId,
                                            @RequestParam(value = "departmentId", defaultValue = "0") int departmentId,
                                            @RequestParam(value = "requestedById", defaultValue = "0") int requestedById) {
        return HrmApi.run(() -> service.requestHistory(mode, from, to, sectionId, departmentId, requestedById));
    }

    @GetMapping(REQ + "/history-detail") @ResponseBody
    public ResponseEntity<?> requestHistoryDetail(@RequestParam("id") long id) { return HrmApi.run(() -> service.requestHistoryDetail(id)); }

    @GetMapping(REQ + "/slip-check") @ResponseBody
    public ResponseEntity<?> requestSlipCheck(@RequestParam(value = "id", defaultValue = "0") long id) { return HrmApi.run(() -> service.requestSlipCheck(id)); }

    @GetMapping(REQ + "/loader-setup") @ResponseBody
    public ResponseEntity<?> loaderSetup(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId) {
        return HrmApi.run(() -> service.loaderSetup(departmentId));
    }

    @GetMapping(REQ + "/loader-show") @ResponseBody
    public ResponseEntity<?> loaderShow(@RequestParam(value = "fromDate", required = false) String fromDate,
                                        @RequestParam(value = "toDate", required = false) String toDate,
                                        @RequestParam(value = "employeeId", defaultValue = "0") long employeeId,
                                        @RequestParam(value = "departmentId", defaultValue = "0") long departmentId) {
        return HrmApi.run(() -> service.loaderRows(fromDate, toDate, employeeId, departmentId));
    }

    // ------------------------------------------------------------------ 666 Employee Over Time

    @GetMapping("/hrm/employee-over-time")
    public String employeeOverTimePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/overtime/employee-over-time"; }

    @GetMapping(EMP + "/setup") @ResponseBody
    public ResponseEntity<?> employeeSetup() { return HrmApi.run(() -> service.employeeSetup()); }

    @GetMapping(EMP + "/history-combos") @ResponseBody
    public ResponseEntity<?> employeeHistoryCombos() { return HrmApi.run(() -> service.employeeHistoryDropDowns()); }

    @GetMapping(EMP + "/employees") @ResponseBody
    public ResponseEntity<?> employees(@RequestParam(value = "month", defaultValue = "0") int month,
                                       @RequestParam(value = "year", defaultValue = "0") int year) {
        return HrmApi.run(() -> service.otEmployees(month, year));
    }

    @GetMapping(EMP + "/detail") @ResponseBody
    public ResponseEntity<?> employeeDetail(@RequestParam("employeeId") long employeeId,
                                            @RequestParam(value = "month", defaultValue = "0") int month,
                                            @RequestParam(value = "year", defaultValue = "0") int year) {
        return HrmApi.run(() -> service.employeeDetail(employeeId, month, year));
    }

    @PostMapping(EMP + "/save") @ResponseBody
    public ResponseEntity<?> employeeSave(@RequestBody OvertimeEmployeeDto body) { return HrmApi.run(() -> service.saveEmployeeOverTime(body)); }

    @GetMapping(EMP + "/history") @ResponseBody
    public ResponseEntity<?> employeeHistory(@RequestParam(value = "month", defaultValue = "0") int month,
                                             @RequestParam(value = "year", defaultValue = "0") int year,
                                             @RequestParam(value = "employeeId", defaultValue = "0") long employeeId) {
        return HrmApi.run(() -> service.employeeHistory(month, year, employeeId));
    }

    @GetMapping(EMP + "/slip-check") @ResponseBody
    public ResponseEntity<?> employeeSlipCheck(@RequestParam(value = "month", defaultValue = "0") int month,
                                               @RequestParam(value = "year", defaultValue = "0") int year) {
        return HrmApi.run(() -> service.employeeSlipCheck(month, year));
    }
}
