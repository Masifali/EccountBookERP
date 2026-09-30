package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.DevicePullJobDto;
import com.mst.models.hrm.dto.DeviceSaveDto;
import com.mst.services.hrm.HrmDeviceService;
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
 * HRM "Device Management" (AppModules 2021) pages and API.
 *
 *   649      /hrm/device-configuration        frmDeviceManagement.cs       API /api/hrm/device/device-configuration/{setup|list|by-id|save}
 *   650/651  /hrm/pull-attendance-by-machine  PullAttendanceByMachine.cs   API /api/hrm/device/pull-attendance-by-machine/{setup|reload|job}
 */
@Controller
public class HrmDeviceController {

    private static final String API = "/api/hrm/device";

    @Autowired private HrmDeviceService service;

    @GetMapping("/hrm/device-configuration")
    public String deviceConfigurationPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/device/device-configuration"; }

    @GetMapping(API + "/device-configuration/setup") @ResponseBody
    public ResponseEntity<?> deviceSetup() { return HrmApi.run(() -> service.deviceSetup()); }

    @GetMapping(API + "/device-configuration/list") @ResponseBody
    public ResponseEntity<?> deviceList() { return HrmApi.run(() -> service.devices()); }

    @GetMapping(API + "/device-configuration/by-id") @ResponseBody
    public ResponseEntity<?> deviceById(@RequestParam("id") int id) { return HrmApi.run(() -> service.device(id)); }

    @PostMapping(API + "/device-configuration/save") @ResponseBody
    public ResponseEntity<?> deviceSave(@RequestBody DeviceSaveDto body) { return HrmApi.run(() -> service.saveDevice(body)); }

    @GetMapping("/hrm/pull-attendance-by-machine")
    public String pullAttendancePage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/device/pull-attendance-by-machine"; }

    @GetMapping(API + "/pull-attendance-by-machine/setup") @ResponseBody
    public ResponseEntity<?> pullSetup() { return HrmApi.run(() -> service.pullSetup()); }

    @GetMapping(API + "/pull-attendance-by-machine/reload") @ResponseBody
    public ResponseEntity<?> pullReload() { return HrmApi.run(() -> service.pullReload()); }

    @PostMapping(API + "/pull-attendance-by-machine/job") @ResponseBody
    public ResponseEntity<?> pullJob(@RequestBody DevicePullJobDto body) { return HrmApi.run(() -> service.queueJob(body)); }
}
