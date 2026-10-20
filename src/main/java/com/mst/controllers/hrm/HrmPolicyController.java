package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.PolicyEobiDto;
import com.mst.models.hrm.dto.PolicyGeneralDto;
import com.mst.models.hrm.dto.PolicyLeaveQuotaDto;
import com.mst.models.hrm.dto.PolicySalaryBreakupDto;
import com.mst.models.hrm.dto.PolicySocialSecurityDto;
import com.mst.services.hrm.HrmPolicyService;
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
 * HRM "Policy Management" (AppModules 2019) pages and API.
 *
 *   643 /hrm/social-security          SocialSecurity.cs        API /api/hrm/policy/social-security/{setup|factors|history|by-id|save}
 *   644 /hrm/general-policy           GeneralPolicy.cs         API /api/hrm/policy/general-policy/{setup|headers|by-id|save}
 *   645 /hrm/salary-breakup-policy    SalaryBreakupPolicy.cs   API /api/hrm/policy/salary-breakup-policy/{setup|locations|grid|by-location|history|save}
 *   646 /hrm/leave-quota-policy       LeaveQuotaPolicy.cs      API /api/hrm/policy/leave-quota-policy/{setup|grids|history|by-id|save}
 *   647 /hrm/eobi-policy              EOBIPolicy.cs            API /api/hrm/policy/eobi-policy/{setup|employees|history|by-id|save}
 */
@Controller
public class HrmPolicyController {

    private static final String API = "/api/hrm/policy";

    @Autowired private HrmPolicyService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/hrm/social-security")
    public String socialSecurityPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/policy/social-security"; }

    @GetMapping("/hrm/general-policy")
    public String generalPolicyPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/policy/general-policy"; }

    @GetMapping("/hrm/salary-breakup-policy")
    public String salaryBreakupPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/policy/salary-breakup-policy"; }

    @GetMapping("/hrm/leave-quota-policy")
    public String leaveQuotaPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/policy/leave-quota-policy"; }

    @GetMapping("/hrm/eobi-policy")
    public String eobiPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/policy/eobi-policy"; }

    // ------------------------------------------------------------------ 643 Social Security

    @GetMapping("/api/hrm/policy/social-security/setup") @ResponseBody
    public ResponseEntity<?> socialSetup() { return HrmApi.run(() -> service.socialSetup()); }

    @GetMapping("/api/hrm/policy/social-security/factors") @ResponseBody
    public ResponseEntity<?> socialFactors() { return HrmApi.run(() -> service.socialFactors()); }

    @GetMapping("/api/hrm/policy/social-security/history") @ResponseBody
    public ResponseEntity<?> socialHistory() { return HrmApi.run(() -> service.socialHistory()); }

    @GetMapping("/api/hrm/policy/social-security/by-id") @ResponseBody
    public ResponseEntity<?> socialById(@RequestParam("id") int id) { return HrmApi.run(() -> service.socialPolicy(id)); }

    @PostMapping("/api/hrm/policy/social-security/save") @ResponseBody
    public ResponseEntity<?> socialSave(@RequestBody PolicySocialSecurityDto body) { return HrmApi.run(() -> service.saveSocial(body)); }

    // ------------------------------------------------------------------ 644 General Policy

    @GetMapping("/api/hrm/policy/general-policy/setup") @ResponseBody
    public ResponseEntity<?> generalSetup() { return HrmApi.run(() -> service.generalSetup()); }

    @GetMapping("/api/hrm/policy/general-policy/headers") @ResponseBody
    public ResponseEntity<?> generalHeaders() { return HrmApi.run(() -> service.generalHeaders()); }

    @GetMapping("/api/hrm/policy/general-policy/by-id") @ResponseBody
    public ResponseEntity<?> generalById(@RequestParam("id") int id) { return HrmApi.run(() -> service.generalDetail(id)); }

    @PostMapping("/api/hrm/policy/general-policy/save") @ResponseBody
    public ResponseEntity<?> generalSave(@RequestBody PolicyGeneralDto body) { return HrmApi.run(() -> service.saveGeneral(body)); }

    // ------------------------------------------------------------------ 645 Salary Breakup Policy

    @GetMapping("/api/hrm/policy/salary-breakup-policy/setup") @ResponseBody
    public ResponseEntity<?> breakupSetup() { return HrmApi.run(() -> service.breakupSetup()); }

    @GetMapping("/api/hrm/policy/salary-breakup-policy/locations") @ResponseBody
    public ResponseEntity<?> breakupLocations() { return HrmApi.run(() -> service.breakupLocations()); }

    @GetMapping("/api/hrm/policy/salary-breakup-policy/grid") @ResponseBody
    public ResponseEntity<?> breakupGrid() { return HrmApi.run(() -> service.breakupGridFill()); }

    @GetMapping("/api/hrm/policy/salary-breakup-policy/by-location") @ResponseBody
    public ResponseEntity<?> breakupByLocation(@RequestParam("locationId") int locationId) { return HrmApi.run(() -> service.breakupByLocation(locationId)); }

    @GetMapping("/api/hrm/policy/salary-breakup-policy/history") @ResponseBody
    public ResponseEntity<?> breakupHistory() { return HrmApi.run(() -> service.breakupHistory()); }

    @PostMapping("/api/hrm/policy/salary-breakup-policy/save") @ResponseBody
    public ResponseEntity<?> breakupSave(@RequestBody PolicySalaryBreakupDto body) { return HrmApi.run(() -> service.saveBreakup(body)); }

    // ------------------------------------------------------------------ 646 Leave Quota Policy

    @GetMapping("/api/hrm/policy/leave-quota-policy/setup") @ResponseBody
    public ResponseEntity<?> leaveQuotaSetup() { return HrmApi.run(() -> service.leaveQuotaSetup()); }

    @GetMapping("/api/hrm/policy/leave-quota-policy/grids") @ResponseBody
    public ResponseEntity<?> leaveQuotaGrids() { return HrmApi.run(() -> service.leaveQuotaGrids()); }

    @GetMapping("/api/hrm/policy/leave-quota-policy/history") @ResponseBody
    public ResponseEntity<?> leaveQuotaHistory() { return HrmApi.run(() -> service.leaveQuotaHistory()); }

    @GetMapping("/api/hrm/policy/leave-quota-policy/by-id") @ResponseBody
    public ResponseEntity<?> leaveQuotaById(@RequestParam("id") int id) { return HrmApi.run(() -> service.leaveQuota(id)); }

    @PostMapping("/api/hrm/policy/leave-quota-policy/save") @ResponseBody
    public ResponseEntity<?> leaveQuotaSave(@RequestBody PolicyLeaveQuotaDto body) { return HrmApi.run(() -> service.saveLeaveQuota(body)); }

    // ------------------------------------------------------------------ 647 E.O.B.I Policy

    @GetMapping("/api/hrm/policy/eobi-policy/setup") @ResponseBody
    public ResponseEntity<?> eobiSetup() { return HrmApi.run(() -> service.eobiSetup()); }

    @GetMapping("/api/hrm/policy/eobi-policy/employees") @ResponseBody
    public ResponseEntity<?> eobiEmployees() { return HrmApi.run(() -> service.eobiEmployees()); }

    @GetMapping("/api/hrm/policy/eobi-policy/history") @ResponseBody
    public ResponseEntity<?> eobiHistory() { return HrmApi.run(() -> service.eobiHistory()); }

    @GetMapping("/api/hrm/policy/eobi-policy/by-id") @ResponseBody
    public ResponseEntity<?> eobiById(@RequestParam("id") int id) { return HrmApi.run(() -> service.eobiPolicy(id)); }

    @PostMapping("/api/hrm/policy/eobi-policy/save") @ResponseBody
    public ResponseEntity<?> eobiSave(@RequestBody PolicyEobiDto body) { return HrmApi.run(() -> service.saveEobi(body)); }
}
