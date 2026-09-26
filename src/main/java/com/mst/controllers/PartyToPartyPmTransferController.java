package com.mst.controllers;

import com.mst.models.dto.PartyToPartyPmTransferDto;
import com.mst.services.PartyToPartyPmTransferService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Management, ModuleId 24 — screen 335 "Packing Material Transfer (Party to Party) (Not Use)",
 * {@code Architecture.WinApp.StoreManagement.PartyToPartyPackingMaterialTransfer}, ScreenName
 * "frmPackingMaterialTransferPartyToParty" (form base.Name "PartyToPartyPackingMaterialTransfer"),
 * DocumentTypeId 125, at /store/party-to-party-pm-transfer. BLL 0251 / DAL 0220 / Model 0195-0196
 * InvPmStockWithPartiesTransfer*.
 *
 * DESKTOP BEHAVIOUR REPRODUCED and DEVIATIONS: see {@link PartyToPartyPmTransferService}.
 */
@Controller
public class PartyToPartyPmTransferController {

    private static final String API = "/api/store/party-to-party-pm-transfer";

    private final PartyToPartyPmTransferService service;
    public PartyToPartyPmTransferController(PartyToPartyPmTransferService service) { this.service = service; }

    @GetMapping("/store/party-to-party-pm-transfer")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/party_to_party_pm_transfer";
    }

    /** Form_Load:222. */
    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    /** btnRefresh_Click:829 — the global lists again. */
    @GetMapping(API + "/globals")
    @ResponseBody
    public ResponseEntity<?> globals() { return run(service::globals, "Refresh failed."); }

    /** Reset:802 — GenerateCode. */
    @GetMapping(API + "/next-doc-no")
    @ResponseBody
    public ResponseEntity<?> nextDocNo() {
        return run(() -> Collections.singletonMap("docNo", service.nextDocNo()), "Could not generate the document number.");
    }

    /** btnshow_Click:928 → FormHistorybind:653. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String mode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo) {
        return run(() -> service.history(mode, fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    /** ReadById:858 / BindHistoryDetailGrid:1005 — GetByID. */
    @GetMapping(API + "/{id:[0-9]+}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(e, "Could not open that document.")));
        }
    }

    /** 219-Print / history Print / Print Preview → P2PPMSlip419 (the Crystal layout is not ported). */
    @GetMapping(API + "/{id:[0-9]+}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        try {
            List<Map<String, Object>> rows = service.slip(id);
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(e, "Print failed.")));
        }
    }

    /** btnSave_Click:628 / btnUpdate_Click:641 → Insert():546. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PartyToPartyPmTransferDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(r, "Save failed.")));
        }
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(e, fallback)));
        }
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
