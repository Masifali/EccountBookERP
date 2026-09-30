package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpBGrnGdnStoreService;
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
 * Party Processing "GRN / GDN Store" (frmGrnGdnStorePartyProcessing.cs): 679 ?mode=grn (DocumentTypeId 44),
 * 680 ?mode=gdn (45). API /api/party-processing/grn-gdn-store/*?mode=.
 */
@Controller
public class PpBGrnGdnStoreController {

    private static final String API = "/api/party-processing/grn-gdn-store";

    @Autowired private PpBGrnGdnStoreService service;

    @GetMapping("/party-processing/grn-gdn-store")
    public String page(@RequestParam(value = "mode", required = false) String mode, Model model) {
        boolean gdn = "gdn".equalsIgnoreCase(mode);
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppMode", gdn ? "gdn" : "grn");
        model.addAttribute("ppScreen", gdn ? PpBGrnGdnStoreService.SCREEN_GDN : PpBGrnGdnStoreService.SCREEN_GRN);
        model.addAttribute("ppTitle", gdn ? "GOODS DISPATCH NOTES PARTY PROCESSING" : "GOODS RECEIVING NOTES PARTY PROCESSING");
        return "partyprocessing/grn-gdn-store";
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
    public ResponseEntity<?> slip(@RequestParam("mode") String mode, @RequestParam("id") int id,
                                  @RequestParam(value = "grid", defaultValue = "false") boolean grid) {
        return HrmApi.run(() -> service.slip(mode, id, grid));
    }
}
