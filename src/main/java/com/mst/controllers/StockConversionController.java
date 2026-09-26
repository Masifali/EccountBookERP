package com.mst.controllers;

import com.mst.services.StockConversionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Stock Conversion — invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * The route is the wiring: ScreenRouteIndex matches a desktop screen to whichever GET route Spring
 * has actually registered, on exact normalised equality, and drops a key two routes both claim.
 * /production/stock-conversion is distinct from every other registered route.
 *
 * No parameter here carries tenancy, the financial year, a document type or a user id. All of
 * them are server-derived inside the service; accepting any would let a caller read another
 * company's conversions.
 */
@Controller
public class StockConversionController {

    @Autowired
    private StockConversionService service;

    @GetMapping("/production/stock-conversion")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/stock_conversion";
    }

    @GetMapping("/api/production/stock-conversion/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        Map<String, Object> sc = service.load(id);
        if (sc == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(sc);
    }

    /** btnVoucher_Click:6207 - the voucher behind a saved conversion (DocumentTypeId 66). */
    @GetMapping("/api/production/stock-conversion/voucher-head")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> voucherHead(@RequestParam int id) {
        Map<String, Object> r = new LinkedHashMap<>();
        try {
            r.put("voucherHeadId", service.voucherHeadId(id));
            return ResponseEntity.ok(r);
        } catch (Exception e) {
            r.put("success", false);
            r.put("message", e.getMessage() == null ? "Voucher lookup failed." : e.getMessage());
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(r);
        }
    }

    @GetMapping("/api/production/stock-conversion/next-code")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> nextCode() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("docSrNo", service.nextCode());
        return ResponseEntity.ok(r);
    }

    @GetMapping("/api/production/stock-conversion/id-by-doc-no")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> idByDocNo(@RequestParam int docSrNo) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("id", service.idByDocNo(docSrNo));
        return ResponseEntity.ok(r);
    }

    /** Every filter optional; an unset one is omitted from the call, not sent as null. */
    @GetMapping("/api/production/stock-conversion/history")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> history(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String entryFromDate,
            @RequestParam(required = false) String entryToDate,
            @RequestParam(required = false) String modifyFromDate,
            @RequestParam(required = false) String modifyToDate,
            @RequestParam(required = false) String approvedDateFrom,
            @RequestParam(required = false) String approvedDateTo,
            @RequestParam(required = false) Integer docNoFrom,
            @RequestParam(required = false) Integer docNoTo) {
        try {
            return ResponseEntity.ok(service.history(fromDate, toDate, entryFromDate, entryToDate,
                    modifyFromDate, modifyToDate, approvedDateFrom, approvedDateTo,
                    docNoFrom, docNoTo));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "History failed." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/stock-conversion/entry-types")
    @ResponseBody
    public ResponseEntity<?> entryTypes() {
        try {
            return ResponseEntity.ok(service.entryTypes());
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.CONFLICT).body(fail(e.getMessage()));
        }
    }

    /** The thirteen dropdowns Load fills (:749-760), in one read. Nothing in it is request-driven. */
    @GetMapping("/api/production/stock-conversion/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() {
        try {
            return ResponseEntity.ok(service.lookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Lookups failed." : e.getMessage()));
        }
    }

    /** cmbUOM / cmbRateUom for the chosen item (bindRateUomAndItemPackUom:1041). */
    @GetMapping("/api/production/stock-conversion/uoms")
    @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(defaultValue = "0") int itemId) {
        try {
            return ResponseEntity.ok(service.uoms(itemId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "UOM list failed." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/stock-conversion/stock-filter")
    @ResponseBody
    public ResponseEntity<?> stockFilter(@RequestParam String activity,
                                         @RequestParam(required = false) Integer itemCategoryId,
                                         @RequestParam(required = false) String docDateTo,
                                         @RequestParam(required = false) Integer warehouseId,
                                         @RequestParam(required = false) String cropYear,
                                         @RequestParam(required = false) Integer itemId,
                                         @RequestParam(required = false) Integer jobLotId) {
        try {
            return ResponseEntity.ok(service.stockFilter(activity, itemCategoryId, docDateTo,
                    warehouseId, cropYear, itemId, jobLotId));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Stock filter failed." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/stock-conversion/available-stock")
    @ResponseBody
    public ResponseEntity<?> availableStock(@RequestParam(required = false) Integer itemCategoryId,
                                            @RequestParam(required = false) String dateTo,
                                            @RequestParam(required = false) Integer warehouseId,
                                            @RequestParam(required = false) Integer itemId,
                                            @RequestParam(required = false) Integer jobLotId) {
        try {
            return ResponseEntity.ok(service.availableStock(itemCategoryId, dateTo,
                    warehouseId, itemId, jobLotId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Available stock failed." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/stock-conversion/store-pm-items")
    @ResponseBody
    public ResponseEntity<?> storeAndPmItems() {
        return ResponseEntity.ok(service.storeAndPmItems());
    }

    // ============================================================================== edit side

    /** Load's switches and the grid value lists that do not depend on the Conversion Type. */
    @GetMapping("/api/production/stock-conversion/edit-setup")
    @ResponseBody
    public ResponseEntity<?> editSetup() {
        try { return ResponseEntity.ok(service.editSetup()); }
        catch (Exception e) { return error(e, "Setup failed."); }
    }

    /** GridPmDropdownBind - PM items by Conversion Type. */
    @GetMapping("/api/production/stock-conversion/pm-items")
    @ResponseBody
    public ResponseEntity<?> pmItems(@RequestParam(defaultValue = "0") int conversionTypeId) {
        try { return ResponseEntity.ok(service.pmItems(conversionTypeId)); }
        catch (Exception e) { return error(e, "PM items failed."); }
    }

    /** ScheduleNoDbCall - pending export schedules for the record. */
    @GetMapping("/api/production/stock-conversion/schedules")
    @ResponseBody
    public ResponseEntity<?> schedules(@RequestParam(defaultValue = "0") int recId) {
        try { return ResponseEntity.ok(service.schedules(recId)); }
        catch (Exception e) { return error(e, "Schedules failed."); }
    }

    /** GetAvgRate - AvgRateOnlyForCGS for an Issue line. */
    @GetMapping("/api/production/stock-conversion/avg-rate")
    @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                     @RequestParam(defaultValue = "0") int recId,
                                     @RequestParam(defaultValue = "0") int jobLotId,
                                     @RequestParam(defaultValue = "0") int cropYearId,
                                     @RequestParam(defaultValue = "0") int warehouseId) {
        try {
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("rate", service.avgRate(itemId, docDate, recId, jobLotId, cropYearId, warehouseId));
            return ResponseEntity.ok(r);
        } catch (Exception e) { return error(e, "Average rate failed."); }
    }

    /** GetAvgRateQtyAndStockInHand - the PM grid's rate. */
    @GetMapping("/api/production/stock-conversion/pm-rate")
    @ResponseBody
    public ResponseEntity<?> pmRate(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                    @RequestParam(defaultValue = "0") int itemConditionId,
                                    @RequestParam(defaultValue = "0") int recId) {
        try {
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("rate", service.pmRate(itemId, docDate, itemConditionId, recId));
            return ResponseEntity.ok(r);
        } catch (Exception e) { return error(e, "PM rate failed."); }
    }

    /** CommonServices.GetWagesRate. */
    @GetMapping("/api/production/stock-conversion/wages-rate")
    @ResponseBody
    public ResponseEntity<?> wagesRate(@RequestParam(required = false) String docDate,
                                       @RequestParam(defaultValue = "0") double packSize,
                                       @RequestParam(defaultValue = "0") int wagesId,
                                       @RequestParam(defaultValue = "0") int contractorId) {
        try {
            Map<String, Object> r = service.wagesRate(docDate, packSize, wagesId, contractorId);
            return ResponseEntity.ok(r == null ? new LinkedHashMap<String, Object>() : r);
        } catch (Exception e) { return error(e, "Wages rate failed."); }
    }

    /** CommonServices.CheckItemsFreeofcostforWages for a batch of {itemId, wagesId}. */
    @PostMapping("/api/production/stock-conversion/wages-free")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> wagesFree(@RequestBody Map<String, Object> body) {
        try {
            Object rows = body.get("rows");
            return ResponseEntity.ok(service.wagesFree(body.get("docDate") == null ? null : String.valueOf(body.get("docDate")),
                    rows instanceof List ? (List<Map<String, Object>>) rows : null));
        } catch (Exception e) { return error(e, "Wages check failed."); }
    }

    /** WagesDetailReadbyId. */
    @GetMapping("/api/production/stock-conversion/wages-detail")
    @ResponseBody
    public ResponseEntity<?> wagesDetail(@RequestParam int id) {
        try { return ResponseEntity.ok(service.wagesDetail(id)); }
        catch (Exception e) { return error(e, "Error loading Wages details for ID " + id); }
    }

    /** LoadavailableTransactionsForIssuance - Load. */
    @GetMapping("/api/production/stock-conversion/loader/setup")
    @ResponseBody
    public ResponseEntity<?> loaderSetup() {
        try { return ResponseEntity.ok(service.loaderSetup()); }
        catch (Exception e) { return error(e, "Loader failed."); }
    }

    /** LoadavailableTransactionsForIssuance - Search (btngrnlod). */
    @GetMapping("/api/production/stock-conversion/loader/search")
    @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(required = false) String fromDate,
                                          @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int parentCategoryId,
                                          @RequestParam(defaultValue = "0") int itemCategoryId,
                                          @RequestParam(defaultValue = "0") int itemTypeId,
                                          @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(required = false) String cropYear,
                                          @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int refDocumentTypeId,
                                          @RequestParam(defaultValue = "0") int supplierCustomerId,
                                          @RequestParam(defaultValue = "0") int itemId) {
        try {
            return ResponseEntity.ok(service.loaderSearch(fromDate, toDate, parentCategoryId, itemCategoryId,
                    itemTypeId, jobLotId, cropYear, warehouseId, refDocumentTypeId, supplierCustomerId, itemId));
        } catch (Exception e) { return error(e, "Search failed."); }
    }

    /** LoadavailableTransactionsForStockReleaseFromFumigation - Search (btngrnlod). Load is /loader/setup. */
    @GetMapping("/api/production/stock-conversion/fumigation/search")
    @ResponseBody
    public ResponseEntity<?> fumigationSearch(@RequestParam(required = false) String fromDate,
                                              @RequestParam(required = false) String toDate,
                                              @RequestParam(defaultValue = "0") int parentCategoryId,
                                              @RequestParam(defaultValue = "0") int itemCategoryId,
                                              @RequestParam(defaultValue = "0") int itemTypeId,
                                              @RequestParam(defaultValue = "0") int jobLotId,
                                              @RequestParam(required = false) String cropYear,
                                              @RequestParam(defaultValue = "0") int warehouseId,
                                              @RequestParam(defaultValue = "0") int refDocumentTypeId,
                                              @RequestParam(defaultValue = "0") int supplierCustomerId,
                                              @RequestParam(defaultValue = "0") int itemId) {
        try {
            return ResponseEntity.ok(service.fumigationSearch(fromDate, toDate, parentCategoryId, itemCategoryId,
                    itemTypeId, jobLotId, cropYear, warehouseId, refDocumentTypeId, supplierCustomerId, itemId));
        } catch (Exception e) { return error(e, "Search failed."); }
    }

    /** frmLoadStockShortFallForSales - StockComboFill (Load and Refresh). */
    @GetMapping("/api/production/stock-conversion/shortfall/setup")
    @ResponseBody
    public ResponseEntity<?> shortfallSetup() {
        try { return ResponseEntity.ok(service.shortfallSetup()); }
        catch (Exception e) { return error(e, "Loader failed."); }
    }

    /** frmLoadStockShortFallForSales - PendingOrderLoad (Load, Search, New). */
    @GetMapping("/api/production/stock-conversion/shortfall/search")
    @ResponseBody
    public ResponseEntity<?> shortfallSearch(@RequestParam(required = false) String docDate,
                                             @RequestParam(defaultValue = "0") int parentCategoryId,
                                             @RequestParam(defaultValue = "0") int itemId,
                                             @RequestParam(defaultValue = "0") int warehouseId,
                                             @RequestParam(defaultValue = "0") int jobLotId,
                                             @RequestParam(required = false) String cropYear) {
        try {
            return ResponseEntity.ok(service.shortfallSearch(docDate, parentCategoryId, itemId, warehouseId,
                    jobLotId, cropYear));
        } catch (Exception e) { return error(e, "Search failed."); }
    }

    /**
     * Save (btnsave, RecId = 0) and Update (btnUpdate, RecId > 0): DAL InvStockConversion.SetData in one
     * transaction. A RAISERROR from any guard procedure (usp_StockConversionValidation,
     * USP_InventoryValidation, USP_VoucherBalanceCheck ...) rolls everything back and its message is
     * returned as the message, as the desktop's MessageBox shows ex.Message.
     */
    @PostMapping("/api/production/stock-conversion/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> body) {
        try {
            return ResponseEntity.ok(service.save(body));
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(rootMessage(e, "Save failed.")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e, "Save failed.")));
        }
    }

    /** btnDelete_Click - InvPurchaseInvoice.RemoveByID with DocumentTypeId 66. */
    @PostMapping("/api/production/stock-conversion/delete")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> delete(@RequestBody Map<String, Object> body) {
        try {
            Object id = body.get("id");
            int rec = id instanceof Number ? ((Number) id).intValue() : (id == null ? 0 : Integer.parseInt(String.valueOf(id).trim()));
            return ResponseEntity.ok(service.delete(rec));
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(rootMessage(e, "Delete failed.")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e, "Delete failed.")));
        }
    }

    /** The innermost message - the procedure's RAISERROR text, not Spring's wrapper around it. */
    private static String rootMessage(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
