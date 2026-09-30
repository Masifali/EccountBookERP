package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.AttendanceFilterDto;
import com.mst.models.hrm.dto.AttendanceSaveDto;
import com.mst.services.hrm.HrmAttendanceService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 * HRM "Attendance Management" (AppModules 2022) pages and API.
 *
 *   658 /hrm/daily-attendance    frmDailyAttendance.cs   API /api/hrm/attendance/daily-attendance/{setup|combos|employees|load|save|print-check}
 *   659 /hrm/manual-attendance   hrmManualAttendance.cs  API /api/hrm/attendance/manual-attendance/{setup|combos|employees|load|update}
 */
@Controller
public class HrmAttendanceController {

    private static final String API = "/api/hrm/attendance";

    @Autowired private HrmAttendanceService service;

    // ------------------------------------------------------------------ 658 Daily Attendance

    @GetMapping("/hrm/daily-attendance")
    public String dailyAttendancePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/attendance/daily-attendance"; }

    @GetMapping(API + "/daily-attendance/setup") @ResponseBody
    public ResponseEntity<?> dailySetup() { return HrmApi.run(() -> service.dailySetup()); }

    @PostMapping(API + "/daily-attendance/combos") @ResponseBody
    public ResponseEntity<?> dailyCombos(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.dailyCombos(f)); }

    @PostMapping(API + "/daily-attendance/employees") @ResponseBody
    public ResponseEntity<?> dailyEmployees(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.dailyEmployees(f)); }

    @PostMapping(API + "/daily-attendance/load") @ResponseBody
    public ResponseEntity<?> dailyLoad(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.dailyLoad(f)); }

    @PostMapping(API + "/daily-attendance/save") @ResponseBody
    public ResponseEntity<?> dailySave(@RequestBody AttendanceSaveDto b) { return HrmApi.run(() -> service.dailySave(b)); }

    @PostMapping(API + "/daily-attendance/print-check") @ResponseBody
    public ResponseEntity<?> dailyPrintCheck(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.dailyPrintCheck(f)); }

    // ------------------------------------------------------------------ 659 Manual Attendance

    @GetMapping("/hrm/manual-attendance")
    public String manualAttendancePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/attendance/manual-attendance"; }

    @GetMapping(API + "/manual-attendance/setup") @ResponseBody
    public ResponseEntity<?> manualSetup() { return HrmApi.run(() -> service.manualSetup()); }

    @PostMapping(API + "/manual-attendance/combos") @ResponseBody
    public ResponseEntity<?> manualCombos(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.manualCombos(f)); }

    @PostMapping(API + "/manual-attendance/employees") @ResponseBody
    public ResponseEntity<?> manualEmployees(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.manualEmployees(f)); }

    @PostMapping(API + "/manual-attendance/load") @ResponseBody
    public ResponseEntity<?> manualLoad(@RequestBody AttendanceFilterDto f) { return HrmApi.run(() -> service.manualLoad(f)); }

    @PostMapping(API + "/manual-attendance/update") @ResponseBody
    public ResponseEntity<?> manualUpdate(@RequestBody AttendanceSaveDto b) { return HrmApi.run(() -> service.manualUpdate(b)); }
}
