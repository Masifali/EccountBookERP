package com.mst.controllers;

import com.mst.services.ExportDefinesService;
import com.mst.services.ExportGdFiPopupsService;
import com.mst.services.ExportStockReservedTpiService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Export (dbo.App 8) definition / pop-up forms, batch Q:
 *
 *   /export/define-documents             207  ExImfrmDefineDocuments                     "Define Documents"
 *   /export/define-export-charges        -    DefineExportCharges                        ?id= opens a charge
 *   /export/define-third-party-type      -    DefineThirdPartyType                       ?id= opens a type
 *   /export/generate-contract-nos        -    frmGenerateExportContractNos (+DefineExportPrefixType)  ?mainId= selects a batch
 *   /export/gd-container-break-up        -    GdContainerBreakUp                         ?gdId= (GdIdFromBreakUp)
 *   /export/stock-reserved-against-tpi   -    frmStockReservedAgainstThirdPartyInspection ?trackingId= preselects the analysis
 *   /export/fi-opening                   -    frmFIOpening                               ?id= opens an opening
 *
 * (204 "Export Pre Shipment Analysis" = EximPreProductionLab is already ported at
 * /quality/export-pre-shipment-analysis and is not duplicated here.)
 * No parameter carries tenancy or a user id; the services derive them from the session.
 */
@Controller
public class ExportPopupDefinesController {

    private static final String DOC = "/api/export/define-documents";
    private static final String CHG = "/api/export/define-export-charges";
    private static final String TPT = "/api/export/define-third-party-type";
    private static final String GEN = "/api/export/generate-contract-nos";
    private static final String GDC = "/api/export/gd-container-break-up";
    private static final String SRV = "/api/export/stock-reserved-against-tpi";
    private static final String FIO = "/api/export/fi-opening";

    @Autowired private ExportDefinesService defines;
    @Autowired private ExportGdFiPopupsService gdfi;
    @Autowired private ExportStockReservedTpiService reserved;

    // ------------------------------------------------------------------ pages

    private static String page(Model model, String view) {
        model.addAttribute("activeMenu", "export");
        return view;
    }

    @GetMapping("/export/define-documents")
    public String defineDocumentsPage(Model model) { return page(model, "export/define_documents"); }

    @GetMapping("/export/define-export-charges")
    public String defineExportChargesPage(Model model) { return page(model, "export/define_export_charges"); }

    @GetMapping("/export/define-third-party-type")
    public String defineThirdPartyTypePage(Model model) { return page(model, "export/define_third_party_type"); }

    @GetMapping("/export/generate-contract-nos")
    public String generateContractNosPage(Model model) { return page(model, "export/generate_contract_nos"); }

    @GetMapping("/export/gd-container-break-up")
    public String gdContainerBreakUpPage(Model model) { return page(model, "export/gd_container_break_up"); }

    @GetMapping("/export/stock-reserved-against-tpi")
    public String stockReservedPage(Model model) { return page(model, "export/stock_reserved_against_tpi"); }

    @GetMapping("/export/fi-opening")
    public String fiOpeningPage(Model model) { return page(model, "export/fi_opening"); }

    // ------------------------------------------------------------------ 207 Define Documents

    @GetMapping(DOC + "/setup") @ResponseBody
    public ResponseEntity<?> docSetup() { return call(() -> defines.documentsSetup(), "Load failed."); }

    @GetMapping(DOC + "/documents") @ResponseBody
    public ResponseEntity<?> docDocuments() { return call(() -> defines.documentsList(), "Load failed."); }

    @GetMapping(DOC + "/groups") @ResponseBody
    public ResponseEntity<?> docGroups() { return call(() -> defines.groupsList(), "Load failed."); }

    @GetMapping(DOC + "/assign") @ResponseBody
    public ResponseEntity<?> docAssign() { return call(() -> defines.assignList(), "Load failed."); }

    @GetMapping(DOC + "/document") @ResponseBody
    public ResponseEntity<?> docRead(@RequestParam("id") int id) { return call(() -> defines.readDocument(id), "Load failed."); }

    @GetMapping(DOC + "/group") @ResponseBody
    public ResponseEntity<?> docGroupRead(@RequestParam("id") int id) { return call(() -> defines.readGroup(id), "Load failed."); }

    @GetMapping(DOC + "/schedule") @ResponseBody
    public ResponseEntity<?> docScheduleRead(@RequestParam("id") int id) { return call(() -> defines.readSchedule(id), "Load failed."); }

    @PostMapping(DOC + "/save-document") @ResponseBody
    public ResponseEntity<?> docSave(@RequestBody Map<String, Object> body) { return call(() -> defines.saveDocument(body), "Save failed."); }

    @PostMapping(DOC + "/save-group") @ResponseBody
    public ResponseEntity<?> docGroupSave(@RequestBody Map<String, Object> body) { return call(() -> defines.saveGroup(body), "Save failed."); }

    @PostMapping(DOC + "/save-schedule") @ResponseBody
    public ResponseEntity<?> docScheduleSave(@RequestBody Map<String, Object> body) { return call(() -> defines.saveSchedule(body), "Save failed."); }

    // ------------------------------------------------------------------ Define Export Charges

    @GetMapping(CHG + "/setup") @ResponseBody
    public ResponseEntity<?> chgSetup() { return call(() -> defines.chargesSetup(), "Error occurred during database call."); }

    @GetMapping(CHG + "/history") @ResponseBody
    public ResponseEntity<?> chgHistory() { return call(() -> defines.chargesHistory(), "Load failed."); }

    @GetMapping(CHG + "/accounts") @ResponseBody
    public ResponseEntity<?> chgAccounts() { return call(() -> defines.chargesAccounts(), "Load failed."); }

    @PostMapping(CHG + "/save") @ResponseBody
    public ResponseEntity<?> chgSave(@RequestBody Map<String, Object> body) { return call(() -> defines.saveCharges(body), "Save failed."); }

    // ------------------------------------------------------------------ Define Third Party Type

    @GetMapping(TPT + "/setup") @ResponseBody
    public ResponseEntity<?> tptSetup() { return call(() -> defines.thirdPartyTypeSetup(), "Error occurred during database call."); }

    @PostMapping(TPT + "/save") @ResponseBody
    public ResponseEntity<?> tptSave(@RequestBody Map<String, Object> body) { return call(() -> defines.saveThirdPartyType(body), "Save failed."); }

    // ------------------------------------------------------------------ Generate Export Contract Nos

    @GetMapping(GEN + "/setup") @ResponseBody
    public ResponseEntity<?> genSetup() { return call(() -> defines.contractNosSetup(), "Load failed."); }

    @GetMapping(GEN + "/history") @ResponseBody
    public ResponseEntity<?> genHistory() { return call(() -> defines.contractNosHistory(), "Load failed."); }

    @PostMapping(GEN + "/save") @ResponseBody
    public ResponseEntity<?> genSave(@RequestBody Map<String, Object> body) { return call(() -> defines.saveContractNos(body), "Save failed."); }

    @GetMapping(GEN + "/prefix-types") @ResponseBody
    public ResponseEntity<?> genPrefixTypes() { return call(() -> defines.prefixTypes(), "Load failed."); }

    @PostMapping(GEN + "/save-prefix-type") @ResponseBody
    public ResponseEntity<?> genSavePrefix(@RequestBody Map<String, Object> body) { return call(() -> defines.savePrefixType(body), "Save failed."); }

    // ------------------------------------------------------------------ GD Container Break Up

    @GetMapping(GDC + "/gds") @ResponseBody
    public ResponseEntity<?> gdcGds(@RequestParam(value = "gdId", defaultValue = "0") int gdId) { return call(() -> gdfi.gds(gdId), "Load failed."); }

    @GetMapping(GDC + "/rows") @ResponseBody
    public ResponseEntity<?> gdcRows(@RequestParam(value = "gdId", defaultValue = "0") int gdId) { return call(() -> gdfi.containerRows(gdId), "Load failed."); }

    @PostMapping(GDC + "/save") @ResponseBody
    public ResponseEntity<?> gdcSave(@RequestBody Map<String, Object> body) { return call(() -> gdfi.saveContainers(body), "Save failed."); }

    // ------------------------------------------------------------------ FI Opening

    @GetMapping(FIO + "/setup") @ResponseBody
    public ResponseEntity<?> fioSetup() { return call(() -> gdfi.fiSetup(), "Error occurred during database call."); }

    @GetMapping(FIO + "/combos") @ResponseBody
    public ResponseEntity<?> fioCombos() { return call(() -> gdfi.fiCombos(), "Load failed."); }

    @GetMapping(FIO + "/history") @ResponseBody
    public ResponseEntity<?> fioHistory() { return call(() -> gdfi.fiHistory(), "Load failed."); }

    @PostMapping(FIO + "/save") @ResponseBody
    public ResponseEntity<?> fioSave(@RequestBody Map<String, Object> body) { return call(() -> gdfi.saveFi(body), "Save failed."); }

    // ------------------------------------------------------------------ Stock Reserved Against TPI

    @GetMapping(SRV + "/setup") @ResponseBody
    public ResponseEntity<?> srvSetup() { return call(() -> reserved.setup(), "Error occurred during database call."); }

    @GetMapping(SRV + "/refresh") @ResponseBody
    public ResponseEntity<?> srvRefresh() { return call(() -> reserved.refresh(), "Load failed."); }

    @PostMapping(SRV + "/search") @ResponseBody
    public ResponseEntity<?> srvSearch(@RequestBody Map<String, Object> body) { return call(() -> reserved.search(body), "Load failed."); }

    @PostMapping(SRV + "/save") @ResponseBody
    public ResponseEntity<?> srvSave(@RequestBody Map<String, Object> body) { return call(() -> reserved.save(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    /** 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message otherwise. */
    private static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
