package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpBGatePassService;
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
 * Party Processing "Gate Pass" (GatePassInwardPartyProcessing.cs): 681 ?mode=inward (DocumentTypeId 54),
 * 682 ?mode=outward (55). API /api/party-processing/gate-pass/*?mode=; every call checks View on the mode's screen.
 */
@Controller
public class PpBGatePassController {

    private static final String API = "/api/party-processing/gate-pass";

    @Autowired private PpBGatePassService service;

    @GetMapping("/party-processing/gate-pass")
    public String page(@RequestParam(value = "mode", required = false) String mode, Model model) {
        boolean out = "outward".equalsIgnoreCase(mode);
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppMode", out ? "outward" : "inward");
        model.addAttribute("ppScreen", out ? PpBGatePassService.SCREEN_OUTWARD : PpBGatePassService.SCREEN_INWARD);
        model.addAttribute("ppTitle", out ? "GatePass Outward" : "GatePass Inward");
        return "partyprocessing/gate-pass";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.setup(mode)); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.refresh(mode)); }

    @GetMapping(API + "/code") @ResponseBody
    public ResponseEntity<?> code(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.code(mode)); }

    @GetMapping(API + "/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam("mode") String mode, @RequestParam(value = "gpType", required = false) String gpType) {
        return HrmApi.run(() -> service.itemsForType(mode, gpType));
    }

    @GetMapping(API + "/advance-dos") @ResponseBody
    public ResponseEntity<?> advanceDos(@RequestParam("mode") String mode, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return HrmApi.run(() -> service.advanceDos(mode, recId));
    }

    @GetMapping(API + "/ado-data") @ResponseBody
    public ResponseEntity<?> adoData(@RequestParam("mode") String mode, @RequestParam("id") int id) { return HrmApi.run(() -> service.adoData(mode, id)); }

    @GetMapping(API + "/open") @ResponseBody
    public ResponseEntity<?> open(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.open(mode)); }

    @GetMapping(API + "/history-parties") @ResponseBody
    public ResponseEntity<?> historyParties(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.historyParties(mode)); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam("mode") String mode, @RequestBody Map<String, Object> body) {
        return HrmApi.run(() -> service.history(mode, body));
    }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("mode") String mode, @RequestParam("id") int id) { return HrmApi.run(() -> service.byId(mode, id)); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestParam("mode") String mode, @RequestBody Map<String, Object> body) {
        return HrmApi.run(() -> service.save(mode, body));
    }

    @PostMapping(API + "/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam("mode") String mode, @RequestParam("id") int id) { return HrmApi.run(() -> service.delete(mode, id)); }

    @GetMapping(API + "/slip") @ResponseBody
    public ResponseEntity<?> slip(@RequestParam("mode") String mode, @RequestParam("id") int id) { return HrmApi.run(() -> service.slip(mode, id)); }
}
