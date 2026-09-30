package com.mst.controllers.logistics;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.logistics.LgsAAgreementService;
import com.mst.services.logistics.LgsARateNegotiationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * lgstcm rate negotiation and agreement pages and API:
 *
 *   937 /logistics/rate-negotiation              frmLogisticRateNegotiation.cs            API /api/logistics/rate-negotiation/...
 *   941 /logistics/rate-negotiation-transporter  frmLogisticRateNegotiationTransporter.cs API /api/logistics/rate-negotiation-transporter/...
 *   939 /logistics/agreement                     frmLogisticAgreement.cs                  API /api/logistics/agreement/...
 *
 * 937 and 941 share one template / script / service (the transporter form is the rate-negotiation form with
 * cities in place of ports and without the vessel fields); the page tells them apart by its route.
 */
@Controller
public class LgsARateAgreementController {

    private static final String RN = "/api/logistics/{page:rate-negotiation|rate-negotiation-transporter}";
    private static final String AG = "/api/logistics/agreement";

    @Autowired private LgsARateNegotiationService rn;
    @Autowired private LgsAAgreementService ag;

    private static boolean tr(String page) { return "rate-negotiation-transporter".equals(page); }

    // ------------------------------------------------------------------ 937 / 941

    @GetMapping("/logistics/rate-negotiation")
    public String rateNegotiationPage(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("transporter", false);
        return "logistics/rate-negotiation";
    }

    @GetMapping("/logistics/rate-negotiation-transporter")
    public String rateNegotiationTransporterPage(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("transporter", true);
        return "logistics/rate-negotiation";
    }

    @GetMapping(RN + "/setup") @ResponseBody
    public ResponseEntity<?> rnSetup(@PathVariable("page") String page) { return HrmApi.run(() -> rn.setup(tr(page))); }

    @GetMapping(RN + "/refresh") @ResponseBody
    public ResponseEntity<?> rnRefresh(@PathVariable("page") String page) { return HrmApi.run(() -> rn.refresh(tr(page))); }

    @GetMapping(RN + "/history-combos") @ResponseBody
    public ResponseEntity<?> rnHistoryCombos(@PathVariable("page") String page) { return HrmApi.run(() -> rn.refreshHistory(tr(page))); }

    @GetMapping(RN + "/doc-no") @ResponseBody
    public ResponseEntity<?> rnDocNo(@PathVariable("page") String page) { return HrmApi.run(() -> rn.docNo(tr(page))); }

    @GetMapping(RN + "/history") @ResponseBody
    public ResponseEntity<?> rnHistory(@PathVariable("page") String page, @RequestParam Map<String, String> q) {
        return HrmApi.run(() -> rn.history(tr(page), q));
    }

    @GetMapping(RN + "/history-detail") @ResponseBody
    public ResponseEntity<?> rnHistoryDetail(@PathVariable("page") String page, @RequestParam("id") int id) {
        return HrmApi.run(() -> rn.historyDetail(tr(page), id));
    }

    @GetMapping(RN + "/by-id") @ResponseBody
    public ResponseEntity<?> rnById(@PathVariable("page") String page, @RequestParam("id") int id,
                                    @RequestParam(value = "edit", defaultValue = "true") boolean edit) {
        return HrmApi.run(() -> rn.read(tr(page), id, edit));
    }

    @PostMapping(RN + "/save") @ResponseBody
    public ResponseEntity<?> rnSave(@PathVariable("page") String page, @RequestBody Map<String, Object> body) {
        return HrmApi.run(() -> rn.save(tr(page), body));
    }

    @PostMapping(RN + "/delete") @ResponseBody
    public ResponseEntity<?> rnDelete(@PathVariable("page") String page, @RequestParam("id") int id) {
        return HrmApi.run(() -> rn.delete(tr(page), id));
    }

    @GetMapping(RN + "/slip-check") @ResponseBody
    public ResponseEntity<?> rnSlipCheck(@PathVariable("page") String page, @RequestParam("id") int id) {
        return HrmApi.run(() -> rn.slipCheck(tr(page), id));
    }

    // ------------------------------------------------------------------ 939

    @GetMapping("/logistics/agreement")
    public String agreementPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/agreement"; }

    @GetMapping(AG + "/setup") @ResponseBody
    public ResponseEntity<?> agSetup() { return HrmApi.run(() -> ag.setup()); }

    @GetMapping(AG + "/refresh") @ResponseBody
    public ResponseEntity<?> agRefresh() { return HrmApi.run(() -> ag.refresh()); }

    @GetMapping(AG + "/history-combos") @ResponseBody
    public ResponseEntity<?> agHistoryCombos() { return HrmApi.run(() -> ag.refreshHistory()); }

    @GetMapping(AG + "/doc-no") @ResponseBody
    public ResponseEntity<?> agDocNo() { return HrmApi.run(() -> ag.docNo()); }

    @GetMapping(AG + "/history") @ResponseBody
    public ResponseEntity<?> agHistory(@RequestParam Map<String, String> q) { return HrmApi.run(() -> ag.history(q)); }

    @GetMapping(AG + "/history-detail") @ResponseBody
    public ResponseEntity<?> agHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> ag.historyDetail(id)); }

    @GetMapping(AG + "/by-id") @ResponseBody
    public ResponseEntity<?> agById(@RequestParam("id") int id, @RequestParam(value = "edit", defaultValue = "true") boolean edit) {
        return HrmApi.run(() -> ag.read(id, edit));
    }

    @PostMapping(AG + "/save") @ResponseBody
    public ResponseEntity<?> agSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> ag.save(body)); }

    @PostMapping(AG + "/delete") @ResponseBody
    public ResponseEntity<?> agDelete(@RequestParam("id") int id) { return HrmApi.run(() -> ag.delete(id)); }

    @GetMapping(AG + "/slip-check") @ResponseBody
    public ResponseEntity<?> agSlipCheck(@RequestParam("id") int id) { return HrmApi.run(() -> ag.slipCheck(id)); }
}
