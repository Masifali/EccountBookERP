package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.SaleOrderCmagtDto;
import com.mst.services.cmagt.SaleOrderCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * Commission Trading - "Sale Order / Deal With Buyer".
 * Desktop form Architecture.WinApp.Cmagt/frmSaleOrderCmagt.cs, DocumentTypeId = 1053.
 *
 * This route renders its own template (cmagt/sale_order_cmagt) and is entirely separate from
 * /sale/sale-order, which is the ordinary inventory Sale Order (DocumentTypeId 81, different
 * tables, different procedures). The two screens must never share a page.
 */
@Controller
@RequestMapping("/commission/sale-order")
public class SaleOrderCmagtController {

    @Autowired
    private SaleOrderCmagtService service;

    @GetMapping
    public String showPage(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Sale Order / Deal With Buyer");
        model.addAttribute("documentTypeId", SaleOrderCmagtService.DOCUMENT_TYPE_ID);
        return "cmagt/sale_order_cmagt";
    }

    /**
     * All dropdown data for the screen in one call. The page disables its action buttons
     * until this resolves, then re-enables them - so the user can never submit against
     * half-loaded lookups.
     */
    @GetMapping("/api/master-lookups")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> masterLookups() {
        return ResponseEntity.ok(service.getMasterLookups());
    }

    @GetMapping("/api/next-doc-no")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> nextDocNo() {
        return ResponseEntity.ok(Map.of("docNo", service.generateNextDocNo()));
    }

    /** Sp_UOMSchedule_GetAllMethod - fills both Pack Uom and Rate Uom for the chosen item. */
    @GetMapping("/api/item-uoms/{itemId}")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> itemUoms(@PathVariable("itemId") int itemId) {
        return ResponseEntity.ok(service.getItemUomSchedule(itemId));
    }

    /** Sp_ItemTaxSchedule_GetAllMethod - Tax Name / Tax% for the chosen item on the doc date. */
    @GetMapping("/api/item-tax/{itemId}")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> itemTax(
            @PathVariable("itemId") int itemId,
            @RequestParam(value = "docDate", required = false) String docDate) {
        return ResponseEntity.ok(service.getItemTaxSchedule(itemId, docDate));
    }

    /**
     * History tab. dateMode selects which of the three mutually exclusive desktop date
     * filters the From/To pair applies to: docdate (default), entrydate, modifydate.
     */
    @GetMapping("/api/history")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> history(
            @RequestParam(value = "dateMode", required = false, defaultValue = "docdate") String dateMode,
            @RequestParam(value = "fromDate", required = false) String fromDate,
            @RequestParam(value = "toDate", required = false) String toDate,
            @RequestParam(value = "validityFrom", required = false) String validityFrom,
            @RequestParam(value = "validityTo", required = false) String validityTo,
            @RequestParam(value = "commissionAgentId", required = false) Integer commissionAgentId,
            @RequestParam(value = "buyerId", required = false) Integer buyerId,
            @RequestParam(value = "itemId", required = false) Integer itemId,
            @RequestParam(value = "parentItemIds", required = false) String parentItemIds,
            @RequestParam(value = "deliveryToPartyId", required = false) Integer deliveryToPartyId,
            @RequestParam(value = "shipToAddress", required = false) String shipToAddress) {
        return ResponseEntity.ok(service.getHistory(dateMode, fromDate, toDate, validityFrom, validityTo,
                commissionAgentId, buyerId, itemId, parentItemIds, deliveryToPartyId, shipToAddress));
    }

    /**
     * Loads one order: header plus all five child collections, as the desktop DAL does.
     *
     * The service enforces tenant and ownership scope. "Not found" and "not permitted" are
     * reported distinctly (404 vs 403) so a denial is never mistaken for a missing record - but
     * neither response reveals anything about a document the caller may not see.
     */
    @GetMapping("/api/{id}")
    @ResponseBody
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        try {
            Map<String, Object> data = service.getSaleOrderById(id);
            if (data == null) return ResponseEntity.notFound().build();
            return ResponseEntity.ok(data);
        } catch (org.springframework.security.access.AccessDeniedException denied) {
            return ResponseEntity.status(org.springframework.http.HttpStatus.FORBIDDEN)
                    .body(Map.of("success", false, "message", denied.getMessage()));
        }
    }

    /**
     * What the signed-in user is allowed to do on this screen, resolved server-side from the
     * real grant grid. The page uses it only to decide what to show; it is never the control,
     * and the server re-checks on every read and write.
     */
    @GetMapping("/api/permissions")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> permissions() {
        return ResponseEntity.ok(Map.of(
                "screenName", SaleOrderCmagtService.DESKTOP_SCREEN_NAME,
                "canViewAllRecords", service.canViewAllRecords()));
    }

    @PostMapping("/api/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody SaleOrderCmagtDto dto) {
        return ResponseEntity.ok(service.saveSaleOrder(dto));
    }

    @PostMapping("/api/delete/{id}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> delete(@PathVariable("id") int id) {
        return ResponseEntity.ok(service.deleteSaleOrder(id));
    }
}
