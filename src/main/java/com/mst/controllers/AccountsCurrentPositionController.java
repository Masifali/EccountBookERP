package com.mst.controllers;

import com.mst.services.AccountsCurrentPositionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Accounts Current Postition (Dashboard) -
 * Architecture.WinApp.Account_Reports\AcFrmDashboard.cs.
 *
 * One form behind TWO DashBoard entries (Accounts Dashboard and Exective DashBoards); both
 * cards carry the same TargetUrl, so both link here.
 *
 * The path's last segment normalises to "acfrmdashboard", the TargetUrl class name.
 *
 * This route did NOT exist before: a hand-written WEB_ROUTES entry pointed the screen at
 * /accounts/dashboard, which no controller maps at all - so the card looked built and led
 * nowhere. That entry was removed; this is the real one.
 */
@Controller
public class AccountsCurrentPositionController {

    @Autowired
    private AccountsCurrentPositionService service;

    @GetMapping("/dashboard/ac-frm-dashboard")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Accounts Current Postition (Dashboard)");
        return "dashboard/accounts_current_position";
    }

    /** AcFrmDashboard_Load, :492-517. */
    @GetMapping("/api/dashboard/accounts-position/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** cmbperemeter_SelectedIndexChanged, :533-568. */
    @GetMapping("/api/dashboard/accounts-position/parameter")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> parameter(
            @RequestParam(defaultValue = "1") int id,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.parameter(id, toDate));
    }

    /** GenerateCards(), :316-354. */
    @GetMapping("/api/dashboard/accounts-position/cards")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> cards(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String companyIds,
            @RequestParam(required = false) String branchIds) {
        return ResponseEntity.ok(service.accountCards(fromDate, toDate, companyIds, branchIds));
    }

    /** GenerateFcyPayablesReceivablesCards(), :194-265. */
    @GetMapping("/api/dashboard/accounts-position/fcy")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> fcy(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String companyIds,
            @RequestParam(required = false) String branchIds) {
        return ResponseEntity.ok(service.fcyCards(fromDate, toDate, companyIds, branchIds));
    }
}
