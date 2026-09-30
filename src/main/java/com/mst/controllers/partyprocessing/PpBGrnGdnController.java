package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpBGrnGdnService;
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
 * Party Processing "Goods Receiving / Dispatch Notes" (PartyProGrnAndGdn.cs): 687 ?mode=grn (DocumentTypeId 49),
 * 688 ?mode=gdn (89). API /api/party-processing/grn-gdn/*?mode=.
 */
@Controller
public class PpBGrnGdnController {

    private static final String API = "/api/party-processing/grn-gdn";

    @Autowired private PpBGrnGdnService service;

    @GetMapping("/party-processing/grn-gdn")
    public String page(@RequestParam(value = "mode", required = false) String mode, Model model) {
        boolean gdn = "gdn".equalsIgnoreCase(mode);
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppMode", gdn ? "gdn" : "grn");
        model.addAttribute("ppScreen", gdn ? PpBGrnGdnService.SCREEN_GDN : PpBGrnGdnService.SCREEN_GRN);
        model.addAttribute("ppTitle", gdn ? "Goods Dispatch Notes" : "Goods Receiving Notes");
        model.addAttribute("ppHistoryTitle", gdn ? "Goods Dispatch Notes Party Processing History" : "Goods Receiving Notes Party Processing History");
        return "partyprocessing/grn-gdn";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.setup(mode)); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.refresh(mode)); }

    @GetMapping(API + "/code") @ResponseBody
    public ResponseEntity<?> code(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.code(mode)); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam("mode") String mode, @RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(mode, itemId)); }

    @GetMapping(API + "/pending") @ResponseBody
    public ResponseEntity<?> pending(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.pending(mode)); }

    @GetMapping(API + "/gate-pass") @ResponseBody
    public ResponseEntity<?> gatePass(@RequestParam("mode") String mode, @RequestParam("gpNo") int gpNo,
                                      @RequestParam(value = "gpId", defaultValue = "0") int gpId,
                                      @RequestParam(value = "stockPartyId", defaultValue = "0") int stockPartyId) {
        return HrmApi.run(() -> service.gatePass(mode, gpNo, gpId, stockPartyId));
    }

    @GetMapping(API + "/loader-setup") @ResponseBody
    public ResponseEntity<?> loaderSetup(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.loaderSetup(mode)); }

    @PostMapping(API + "/loader") @ResponseBody
    public ResponseEntity<?> loader(@RequestParam("mode") String mode, @RequestBody Map<String, Object> body) {
        return HrmApi.run(() -> service.loader(mode, body));
    }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos(@RequestParam("mode") String mode) { return HrmApi.run(() -> service.historyCombos(mode)); }

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
