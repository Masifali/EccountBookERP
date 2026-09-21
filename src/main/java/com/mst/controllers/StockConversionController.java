package com.mst.controllers;

import com.mst.models.dto.StockConversionDto;
import com.mst.services.StockConversionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
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
        return ResponseEntity.ok(service.entryTypes());
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

    /**
     * Present so the contract exists and the screen can call it, but it refuses.
     *
     * The desktop's Save posts inventory transactions, an accounting voucher and contractor wages
     * alongside the document. Until those three are traced, saving the header and details alone
     * would create a conversion with no stock movement and no voucher. 501 Not Implemented is the
     * honest status: the request is understood and the server will not pretend it worked.
     */
    @PostMapping("/api/production/stock-conversion/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody StockConversionDto dto) {
        try {
            service.save(dto);
            return ResponseEntity.ok(fail("Saved."));
        } catch (UnsupportedOperationException e) {
            return ResponseEntity.status(HttpStatus.NOT_IMPLEMENTED).body(fail(e.getMessage()));
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Save failed." : e.getMessage()));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
