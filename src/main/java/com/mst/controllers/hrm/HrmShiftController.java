package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.ShiftHolidaySaveDto;
import com.mst.models.hrm.dto.ShiftLocationSaveDto;
import com.mst.models.hrm.dto.ShiftSaveDto;
import com.mst.models.hrm.dto.ShiftTimingSaveDto;
import com.mst.services.hrm.HrmShiftService;
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
 * HRM shift screens (AppModules 2022) pages and API.
 *
 *   652 /hrm/define-shift     frmGenShift.cs         API /api/hrm/shift/define-shift/{setup|reload|list|by-id|save}
 *   653 /hrm/shift-location   frmShiftLocation.cs    API /api/hrm/shift/shift-location/{setup|list|by-id|save}
 *   654 /hrm/shift-timing     frmGenShiftTiming.cs   API /api/hrm/shift/shift-timing/{setup|reload|locations|history|by-id|save}
 *   655 /hrm/holiday          frmHoliday.cs          API /api/hrm/shift/holiday/{setup|by-month|save}
 */
@Controller
public class HrmShiftController {

    private static final String API = "/api/hrm/shift";

    @Autowired private HrmShiftService service;

    // ------------------------------------------------------------------ 652
    @GetMapping("/hrm/define-shift")
    public String defineShiftPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/shift/define-shift"; }

    @GetMapping(API + "/define-shift/setup") @ResponseBody
    public ResponseEntity<?> shiftSetup() { return HrmApi.run(() -> service.shiftSetup()); }

    @GetMapping(API + "/define-shift/reload") @ResponseBody
    public ResponseEntity<?> shiftReload() { return HrmApi.run(() -> service.shiftReload()); }

    @GetMapping(API + "/define-shift/list") @ResponseBody
    public ResponseEntity<?> shiftList() { return HrmApi.run(() -> service.shifts()); }

    @GetMapping(API + "/define-shift/by-id") @ResponseBody
    public ResponseEntity<?> shiftById(@RequestParam("id") int id) { return HrmApi.run(() -> service.shift(id)); }

    @PostMapping(API + "/define-shift/save") @ResponseBody
    public ResponseEntity<?> shiftSave(@RequestBody ShiftSaveDto body) { return HrmApi.run(() -> service.saveShift(body)); }

    // ------------------------------------------------------------------ 653
    @GetMapping("/hrm/shift-location")
    public String shiftLocationPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/shift/shift-location"; }

    @GetMapping(API + "/shift-location/setup") @ResponseBody
    public ResponseEntity<?> shiftLocationSetup() { return HrmApi.run(() -> service.shiftLocationSetup()); }

    @GetMapping(API + "/shift-location/list") @ResponseBody
    public ResponseEntity<?> shiftLocationList() { return HrmApi.run(() -> service.shiftLocations()); }

    @GetMapping(API + "/shift-location/by-id") @ResponseBody
    public ResponseEntity<?> shiftLocationById(@RequestParam("id") int id) { return HrmApi.run(() -> service.shiftLocation(id)); }

    @PostMapping(API + "/shift-location/save") @ResponseBody
    public ResponseEntity<?> shiftLocationSave(@RequestBody ShiftLocationSaveDto body) { return HrmApi.run(() -> service.saveShiftLocation(body)); }

    // ------------------------------------------------------------------ 654
    @GetMapping("/hrm/shift-timing")
    public String shiftTimingPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/shift/shift-timing"; }

    @GetMapping(API + "/shift-timing/setup") @ResponseBody
    public ResponseEntity<?> shiftTimingSetup() { return HrmApi.run(() -> service.shiftTimingSetup()); }

    @GetMapping(API + "/shift-timing/reload") @ResponseBody
    public ResponseEntity<?> shiftTimingReload() { return HrmApi.run(() -> service.shiftTimingReload()); }

    @GetMapping(API + "/shift-timing/locations") @ResponseBody
    public ResponseEntity<?> shiftTimingLocations(@RequestParam(value = "shiftId", defaultValue = "0") int shiftId) { return HrmApi.run(() -> service.shiftTimingLocations(shiftId)); }

    @GetMapping(API + "/shift-timing/history") @ResponseBody
    public ResponseEntity<?> shiftTimingHistory() { return HrmApi.run(() -> service.shiftTimings()); }

    @GetMapping(API + "/shift-timing/by-id") @ResponseBody
    public ResponseEntity<?> shiftTimingById(@RequestParam("id") int id) { return HrmApi.run(() -> service.shiftTiming(id)); }

    @PostMapping(API + "/shift-timing/save") @ResponseBody
    public ResponseEntity<?> shiftTimingSave(@RequestBody ShiftTimingSaveDto body) { return HrmApi.run(() -> service.saveShiftTiming(body)); }

    // ------------------------------------------------------------------ 655
    @GetMapping("/hrm/holiday")
    public String holidayPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/shift/holiday"; }

    @GetMapping(API + "/holiday/setup") @ResponseBody
    public ResponseEntity<?> holidaySetup() { return HrmApi.run(() -> service.holidaySetup()); }

    @GetMapping(API + "/holiday/by-month") @ResponseBody
    public ResponseEntity<?> holidayByMonth(@RequestParam("month") int month, @RequestParam("year") int year) { return HrmApi.run(() -> service.holidaysByMonth(month, year)); }

    @PostMapping(API + "/holiday/save") @ResponseBody
    public ResponseEntity<?> holidaySave(@RequestBody ShiftHolidaySaveDto body) { return HrmApi.run(() -> service.saveHolidays(body)); }
}
