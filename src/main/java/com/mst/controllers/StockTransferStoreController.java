package com.mst.controllers;

import com.mst.models.dto.StockTransferStoreDto;
import com.mst.services.StockTransferStoreService;
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
 * Packing Material, ModuleId 54 — screen 505 "Stock Transfer Store", {@code frmStockTransferStore.cs}
 * (ScreenName "frmStockTransferStore"), DocumentTypeId 807, at /packing-material/stock-transfer-store.
 * All behaviour notes and deviations are on {@link StockTransferStoreService}.
 */
@Controller
public class StockTransferStoreController {

    private static final String API = "/api/packing-material/stock-transfer-store";

    private final StockTransferStoreService service;
    public StockTransferStoreController(StockTransferStoreService service) { this.service = service; }

    @GetMapping("/packing-material/stock-transfer-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/stock_transfer_store";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not number the document."); }

    @GetMapping(API + "/branch-racks")
    @ResponseBody
    public ResponseEntity<?> branchRacks(@RequestParam(defaultValue = "0") int branchId) { return run(() -> service.branchRacks(branchId), "Could not load racks."); }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int conditionId, @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId, @RequestParam(required = false) String docDate) {
        return run(() -> service.stock(recId, itemId, conditionId, warehouseId, rackId, docDate), "Could not read the stock.");
    }

    @GetMapping(API + "/ref-docs")
    @ResponseBody
    public ResponseEntity<?> refDocs(@RequestParam(defaultValue = "0") int refDocumentTypeId, @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.refDocs(refDocumentTypeId, recId), "Could not load reference documents.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record not found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StockTransferStoreDto dto) { return write(() -> service.save(dto), "Save failed."); }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return write(() -> service.delete(id), "Delete failed."); }

    // --------------------------------------------------------------------------- helpers

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), fallback)));
        }
    }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            /* RAISERRORs from the save procedures carry the desktop's own message. */
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(rootCause(e), fallback)));
        }
    }

    private static Throwable rootCause(Throwable t) {
        Throwable r = t;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        return r;
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
