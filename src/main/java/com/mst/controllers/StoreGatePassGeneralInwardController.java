package com.mst.controllers;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.StoreGatePassGeneralInwardService;
import com.mst.services.sale.engr.SaleEngrAttachments;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.server.ResponseStatusException;

import java.nio.charset.StandardCharsets;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Purchase, ModuleId 67 - screen 342 "Inward Gate Pass General", Architecture.WinApp.Inventory_Definition.GatePassGeneral
 * (DocumentTypeId 52). Page /store/inward-gate-pass-general, JSON under /api/store/inward-gate-pass-general.
 */
@Controller
public class StoreGatePassGeneralInwardController extends SaleEngrControllerBase {
    private final StoreGatePassGeneralInwardService service;

    public StoreGatePassGeneralInwardController(StoreGatePassGeneralInwardService service) { this.service = service; }

    @GetMapping("/store/inward-gate-pass-general")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/inward_gate_pass_general";
    }

    @GetMapping("/api/store/inward-gate-pass-general/initial") @ResponseBody
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/api/store/inward-gate-pass-general/next-no") @ResponseBody
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/api/store/inward-gate-pass-general/warehouses") @ResponseBody
    public List<Map<String, Object>> warehouses() { return service.warehouses(); }

    @GetMapping("/api/store/inward-gate-pass-general/uoms") @ResponseBody
    public List<Map<String, Object>> uoms(@RequestParam(defaultValue = "0") int itemId) { return service.uoms(itemId); }

    @GetMapping("/api/store/inward-gate-pass-general/ref-docs") @ResponseBody
    public List<Map<String, Object>> refDocs(@RequestParam int typeId, @RequestParam(defaultValue = "0") int recId) { return service.refDocs(typeId, recId); }

    @GetMapping("/api/store/inward-gate-pass-general/history-parties") @ResponseBody
    public List<Map<String, Object>> historyParties() { return service.historyParties(); }

    @GetMapping("/api/store/inward-gate-pass-general/history") @ResponseBody
    public List<Map<String, Object>> history(@RequestParam(required = false) String status,
                                             @RequestParam(defaultValue = "doc") String dateMode,
                                             @RequestParam(defaultValue = "true") boolean useFrom,
                                             @RequestParam(defaultValue = "true") boolean useTo,
                                             @RequestParam(required = false) String fromDate,
                                             @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromDocNo,
                                             @RequestParam(defaultValue = "0") int toDocNo,
                                             @RequestParam(defaultValue = "0") int partyId) {
        StoreGatePassGeneralInwardService.HistoryFilter f = new StoreGatePassGeneralInwardService.HistoryFilter();
        f.status = status; f.dateMode = dateMode; f.useFrom = useFrom; f.useTo = useTo; f.fromDate = fromDate; f.toDate = toDate;
        f.fromDocNo = fromDocNo; f.toDocNo = toDocNo; f.partyId = partyId;
        return service.history(f);
    }

    @GetMapping("/api/store/inward-gate-pass-general/{id:[0-9]+}") @ResponseBody
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/api/store/inward-gate-pass-general/{id:[0-9]+}/details") @ResponseBody
    public List<Map<String, Object>> details(@PathVariable int id) { return service.details(id); }

    @PostMapping("/api/store/inward-gate-pass-general/save") @ResponseBody
    public Map<String, Object> save(@RequestBody StoreGatePassGeneralInwardService.Request r) { return service.save(r); }

    @GetMapping("/api/store/inward-gate-pass-general/{id:[0-9]+}/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachmentList(id); }

    @GetMapping("/api/store/inward-gate-pass-general/{id:[0-9]+}/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> download(@PathVariable int id, @PathVariable int attachmentId) {
        SaleEngrAttachments.Download d = service.download(id, attachmentId);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name(), StandardCharsets.UTF_8).replace("+", "%20"))
                .contentType(MediaType.APPLICATION_OCTET_STREAM).body(d.bytes());
    }

    // The desktop shows ex.Message in a MessageBox; the page does the same with {message}.
    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<Map<String, Object>> badRequest(IllegalArgumentException e) { return fail(HttpStatus.BAD_REQUEST, e.getMessage()); }

    @ExceptionHandler(IllegalStateException.class)
    public ResponseEntity<Map<String, Object>> conflict(IllegalStateException e) { return fail(HttpStatus.CONFLICT, e.getMessage()); }

    @ExceptionHandler(ResponseStatusException.class)
    public ResponseEntity<Map<String, Object>> status(ResponseStatusException e) { return fail(e.getStatus(), e.getReason()); }

    private static ResponseEntity<Map<String, Object>> fail(HttpStatus s, String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message == null || message.isBlank() ? "Request failed." : message);
        return ResponseEntity.status(s).body(m);
    }
}
