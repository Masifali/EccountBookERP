package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpAReportsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Party Processing report pages of group PpA and their API (/api/party-processing/reports/<page>/...).
 *
 *   690     /party-processing/reports/grn-info                 frmPartyProcessingGrnInfo.cs
 *   691     /party-processing/reports/gate-pass                frmGatePassPartyProcessing.cs
 *   692/693 /party-processing/reports/grn-register?mode=grn|gdn  frmGRNGDNPartyProcessing.cs (Tag -> DocumentTypeId 49 / 89)
 *   694     /party-processing/reports/stock-report             frmStockReportPartyProcessing.cs
 */
@Controller
public class PpAReportsController {

    private static final String API = "/api/party-processing/reports";

    @Autowired private PpAReportsService service;

    private static String page(Model model, String view) { model.addAttribute("activeMenu", "apps"); return view; }

    // ------------------------------------------------------------------ 690 GRN Info

    @GetMapping("/party-processing/reports/grn-info")
    public String grnInfoPage(Model model) { return page(model, "partyprocessing/reports/grn-info"); }

    @GetMapping(API + "/grn-info/setup") @ResponseBody
    public ResponseEntity<?> grnInfoSetup() { return HrmApi.run(() -> service.grnInfoSetup()); }

    @GetMapping(API + "/grn-info/combos") @ResponseBody
    public ResponseEntity<?> grnInfoCombos() { return HrmApi.run(() -> service.grnInfoCombos()); }

    @PostMapping(API + "/grn-info/search") @ResponseBody
    public ResponseEntity<?> grnInfo(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> service.grnInfo(f)); }

    @GetMapping(API + "/grn-info/slip-check") @ResponseBody
    public ResponseEntity<?> grnSlipCheck(@RequestParam("id") int id) { return HrmApi.run(() -> service.grnSlipCheck(id)); }

    // ------------------------------------------------------------------ 691 Gate Pass

    @GetMapping("/party-processing/reports/gate-pass")
    public String gatePassPage(Model model) { return page(model, "partyprocessing/reports/gate-pass"); }

    @GetMapping(API + "/gate-pass/setup") @ResponseBody
    public ResponseEntity<?> gatePassSetup() { return HrmApi.run(() -> service.gatePassSetup()); }

    @GetMapping(API + "/gate-pass/combos") @ResponseBody
    public ResponseEntity<?> gatePassCombos() { return HrmApi.run(() -> service.gatePassCombos()); }

    @PostMapping(API + "/gate-pass/search") @ResponseBody
    public ResponseEntity<?> gatePass(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> service.gatePass(f)); }

    // ------------------------------------------------------------------ 692 / 693 GRN / GDN register

    @GetMapping("/party-processing/reports/grn-register")
    public String grnGdnPage(@RequestParam(value = "mode", defaultValue = "grn") String mode, Model model) {
        model.addAttribute("ppMode", "gdn".equalsIgnoreCase(mode) ? "gdn" : "grn");
        return page(model, "partyprocessing/reports/grn-register");
    }

    @GetMapping(API + "/grn-register/setup") @ResponseBody
    public ResponseEntity<?> grnGdnSetup(@RequestParam(value = "mode", defaultValue = "grn") String mode) { return HrmApi.run(() -> service.grnGdnSetup(mode)); }

    @GetMapping(API + "/grn-register/combos") @ResponseBody
    public ResponseEntity<?> grnGdnCombos(@RequestParam(value = "mode", defaultValue = "grn") String mode) { return HrmApi.run(() -> service.grnGdnCombos(mode)); }

    @PostMapping(API + "/grn-register/search") @ResponseBody
    public ResponseEntity<?> grnGdn(@RequestParam(value = "mode", defaultValue = "grn") String mode, @RequestBody Map<String, Object> f) {
        return HrmApi.run(() -> service.grnGdn(mode, f));
    }

    // ------------------------------------------------------------------ 694 Stock Report

    @GetMapping("/party-processing/reports/stock-report")
    public String stockPage(Model model) { return page(model, "partyprocessing/reports/stock-report"); }

    @GetMapping(API + "/stock-report/setup") @ResponseBody
    public ResponseEntity<?> stockSetup() { return HrmApi.run(() -> service.stockSetup()); }

    @GetMapping(API + "/stock-report/combos") @ResponseBody
    public ResponseEntity<?> stockCombos() { return HrmApi.run(() -> service.stockCombos()); }

    @PostMapping(API + "/stock-report/search") @ResponseBody
    public ResponseEntity<?> stock(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> service.stock(f)); }
}
