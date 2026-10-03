package com.mst.controllers;

import com.mst.models.dto.SaleDriverBioRequest;
import com.mst.services.DriverBioInwardService;
import java.util.List;
import java.util.Map;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/**
 * frmDriverBio with RefDocumentTypeId = 51 (ScreenName "frmDriverBioForInWard"), opened from Inward Gate Pass
 * (BtnDriverForm_Click, and after Save with the saved gate pass pre-selected: ?gatePassId=).
 * /sale/driver-bio stays the outward (91) page.
 */
@Controller
@RequestMapping("/purchase/driver-bio")
public class PurchaseDriverBioController {
    private final DriverBioInwardService service;
    public PurchaseDriverBioController(DriverBioInwardService service) { this.service = service; }

    @GetMapping({"", "/"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Driver Bio (Inward)");
        return "purchase/driver_bio_inward";
    }

    @GetMapping("/api/initial") @ResponseBody
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/api/refresh") @ResponseBody
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/api/gate-pass") @ResponseBody
    public List<Map<String, Object>> gatePass(@RequestParam int gpSrNo) { return service.gatePassHeader(gpSrNo); }

    @GetMapping("/api/history") @ResponseBody
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "docDate") String dateField,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromDoc, @RequestParam(defaultValue = "0") int toDoc) {
        return service.history(dateField, fromDate, toDate, fromDoc, toDoc);
    }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    /** Body: the SaleDriverBioRequest fields plus gpDate (the picker value, ISO date-time). */
    @PostMapping("/api/save") @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        SaleDriverBioRequest r = new SaleDriverBioRequest();
        r.id = intOf(body.get("id"));
        r.gatePassOutwardId = intOf(body.get("gatePassOutwardId"));
        r.forwarderName = str(body.get("forwarderName"));
        r.driverName = str(body.get("driverName"));
        r.fatherName = str(body.get("fatherName"));
        r.cnicNo = str(body.get("cnicNo"));
        r.driverCellNo = str(body.get("driverCellNo"));
        r.alternateCellNo = str(body.get("alternateCellNo"));
        r.remarksHeader = str(body.get("remarksHeader"));
        return service.save(r, str(body.get("gpDate")));
    }

    private static int intOf(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        String s = v == null ? "" : v.toString().trim();
        return s.matches("-?\\d+") ? Integer.parseInt(s) : 0;
    }
    private static String str(Object v) { return v == null ? null : v.toString(); }
}
