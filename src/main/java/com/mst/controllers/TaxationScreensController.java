package com.mst.controllers;

import com.mst.services.TaxationScreensService;
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
 * Three more Taxation screens (dbo.ScreenDefinition ModuleId 9):
 *
 *   /taxation/items-allocate-to-tax-item   179  RegularItemsAllocateToTaxItem.cs  "Items Allocate To Tax Item"
 *   /taxation/sale-tax-summary             175  frmSaleTaxSummaryRpt.cs           "Sale Tax Summary"
 *   /taxation/wht-challan-deposit          174  frmWhtTaxChallanDeposit.cs        "Wht Challan Deposit"
 *
 * Linked from the hub through DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID. No parameter carries tenancy
 * or a user id; the service derives them from the session.
 */
@Controller
public class TaxationScreensController {

    private static final String API = "/api/taxation";

    @Autowired private TaxationScreensService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/taxation/items-allocate-to-tax-item")
    public String allocationPage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/items_allocate_to_tax_item";
    }

    @GetMapping("/taxation/sale-tax-summary")
    public String saleTaxSummaryPage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/sale_tax_summary";
    }

    @GetMapping("/taxation/wht-challan-deposit")
    public String whtChallanPage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/wht_challan_deposit";
    }

    // ------------------------------------------------------------------ 179

    @GetMapping(API + "/allocation/setup") @ResponseBody
    public ResponseEntity<?> allocationSetup() { return call(() -> service.allocationSetup(), "Load failed."); }

    @GetMapping(API + "/allocation/lists") @ResponseBody
    public ResponseEntity<?> allocationLists(@RequestParam("taxItemId") int taxItemId,
                                             @RequestParam(value = "categoryId", defaultValue = "0") int categoryId,
                                             @RequestParam(value = "typeId", defaultValue = "0") int typeId) {
        return call(() -> service.allocationLists(taxItemId, categoryId, typeId), "Load failed.");
    }

    @PostMapping(API + "/allocation/allocate") @ResponseBody
    public ResponseEntity<?> allocate(@RequestBody Map<String, Object> body) { return call(() -> service.allocate(body), "Save failed."); }

    @PostMapping(API + "/allocation/deallocate") @ResponseBody
    public ResponseEntity<?> deallocate(@RequestBody Map<String, Object> body) { return call(() -> service.deallocate(body), "Save failed."); }

    // ------------------------------------------------------------------ 175

    @GetMapping(API + "/sale-tax-summary") @ResponseBody
    public ResponseEntity<?> saleTaxSummary(@RequestParam("from") String from, @RequestParam("to") String to) {
        return call(() -> service.saleTaxSummary(from, to), "Load failed.");
    }

    // ------------------------------------------------------------------ 174

    @GetMapping(API + "/wht-challan/setup") @ResponseBody
    public ResponseEntity<?> whtChallanSetup() { return call(() -> service.whtChallanSetup(), "Load failed."); }

    @GetMapping(API + "/wht-challan/doc-no") @ResponseBody
    public ResponseEntity<?> whtChallanDocNo() { return call(() -> service.whtChallanDocNo(), "Load failed."); }

    @GetMapping(API + "/wht-challan/pending") @ResponseBody
    public ResponseEntity<?> whtChallanPending(@RequestParam("partyAccountId") int partyAccountId,
                                               @RequestParam(value = "branchId", defaultValue = "0") int branchId,
                                               @RequestParam(value = "projectId", defaultValue = "0") int projectId) {
        return call(() -> service.whtChallanPending(partyAccountId, branchId, projectId), "Load failed.");
    }

    @GetMapping(API + "/wht-challan/history") @ResponseBody
    public ResponseEntity<?> whtChallanHistory() { return call(() -> service.whtChallanHistory(), "Load failed."); }

    @GetMapping(API + "/wht-challan/voucher") @ResponseBody
    public ResponseEntity<?> whtChallanVoucher(@RequestParam("id") int id) { return call(() -> service.whtChallanVoucher(id), "Load failed."); }

    @PostMapping(API + "/wht-challan/save") @ResponseBody
    public ResponseEntity<?> saveWhtChallan(@RequestBody Map<String, Object> body) { return call(() -> service.saveWhtChallan(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

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
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
