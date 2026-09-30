package com.mst.controllers.sale;

import com.mst.repositories.SaleInvoiceReturnRepository;
import com.mst.services.SaleInvoiceReturnService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.*;

import static com.mst.repositories.SaleInvoiceReturnRepository.*;

/** Sale Invoice Return (InvfrmSaleInvoiceReturn, 98): page + JSON API under /sale/sale-invoice-return/api. */
@Controller
@RequestMapping("/sale/sale-invoice-return")
public class SaleInvoiceReturnController {

    private final SaleInvoiceReturnService service;
    private final SaleInvoiceReturnRepository repo;

    public SaleInvoiceReturnController(SaleInvoiceReturnService service, SaleInvoiceReturnRepository repo) {
        this.service = service; this.repo = repo;
    }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Return");
        return "sale/sale_invoice_return";
    }

    /** Load event: rights, config flags, every lookup, doc numbers, history branches. */
    @GetMapping("/api/init") @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping("/api/numbers") @ResponseBody
    public Map<String, Object> numbers() { return repo.numbers(); }

    /** comsupplier_Leave -> ItemsBindByParty. */
    @GetMapping("/api/items-by-party") @ResponseBody
    public List<Map<String, Object>> itemsByParty(@RequestParam("partyId") int partyId) { return repo.itemsByParty(partyId); }

    /** btnRefresh -> item(): all items. */
    @GetMapping("/api/all-items") @ResponseBody
    public List<Map<String, Object>> allItems() { return repo.allItems(); }

    /** comItem_Leave -> PackUOM(). */
    @GetMapping("/api/uoms") @ResponseBody
    public List<Map<String, Object>> uoms(@RequestParam("itemId") int itemId) { return repo.uoms(itemId); }

    /** HistoryComboFill: customers of the ticked branches (leading-comma list, as the desktop sends). */
    @GetMapping("/api/history-customers") @ResponseBody
    public List<Map<String, Object>> historyCustomers(@RequestParam(value = "branchesIds", required = false) String branchesIds) {
        return repo.historyCustomers(branchesIds == null ? "" : branchesIds);
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(value = "dateMode", required = false) String dateMode,
                                     @RequestParam(value = "fromDate", required = false) String fromDate,
                                     @RequestParam(value = "toDate", required = false) String toDate,
                                     @RequestParam(value = "fromDocNo", required = false, defaultValue = "0") int fromDocNo,
                                     @RequestParam(value = "toDocNo", required = false, defaultValue = "0") int toDocNo,
                                     @RequestParam(value = "customerId", required = false, defaultValue = "0") int customerId,
                                     @RequestParam(value = "branchesIds", required = false) String branchesIds) {
        try {
            if (branchesIds == null || branchesIds.isBlank()) return fail("Select branch first");
            return ResponseEntity.ok(repo.history(dateMode, fromDate, toDate, fromDocNo, toDocNo, customerId, branchesIds, repo.hasRight("CanView AllRecord")));
        } catch (RuntimeException e) { return fail(msg(e)); }
    }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable("id") int id) {
        try { return ResponseEntity.ok(service.read(id)); } catch (RuntimeException e) { return fail(msg(e)); }
    }

    /** History detail grid (GetDetailGrdByHeadId :3619) - the same GetByID. */
    @GetMapping("/api/{id:[0-9]+}/details") @ResponseBody
    public ResponseEntity<?> details(@PathVariable("id") int id) {
        try { return ResponseEntity.ok(service.read(id).get("details")); } catch (RuntimeException e) { return fail(msg(e)); }
    }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) {
        try { return ResponseEntity.ok(service.save(payload)); } catch (RuntimeException e) { return fail(msg(e)); }
    }

    @DeleteMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
        try { service.delete(id); return ResponseEntity.ok(Map.of("success", true, "message", "Delete Record Successfully")); }
        catch (RuntimeException e) { return fail(msg(e)); }
    }

    /** DeleteDetailrow :5221 - a saved line is validated before it is removed from the grid. */
    @PostMapping("/api/{id:[0-9]+}/detail/{detailId:[0-9]+}/check-delete") @ResponseBody
    public ResponseEntity<?> checkDelete(@PathVariable("id") int id, @PathVariable("detailId") int detailId) {
        try { repo.validateDetailDelete(id, detailId); return ResponseEntity.ok(Map.of("success", true)); }
        catch (RuntimeException e) { return fail(msg(e)); }
    }

    // ------------------------------------------------------------------ Load Sale Invoice dialog
    @GetMapping("/api/loader/init") @ResponseBody
    public Map<String, Object> loaderInit() { return service.loaderInit(); }

    @GetMapping("/api/loader/pending") @ResponseBody
    public ResponseEntity<?> pending(@RequestParam("fromDate") String fromDate, @RequestParam("toDate") String toDate,
                                     @RequestParam(value = "branchesIds", required = false) String branchesIds) {
        try { return ResponseEntity.ok(service.pending(fromDate, toDate, branchesIds == null ? "" : branchesIds)); }
        catch (RuntimeException e) { return fail(msg(e)); }
    }

    @PostMapping("/api/loader/load") @ResponseBody
    public ResponseEntity<?> load(@RequestBody Map<String, Object> body) {
        try {
            List<Integer> ids = new ArrayList<>();
            if (body.get("invoiceIds") instanceof List<?> l) for (Object o : l) ids.add(num(o));
            return ResponseEntity.ok(service.loadSaleInvoices(ids, str(body.get("fromDate")), str(body.get("toDate")), str(body.get("branchesIds")),
                    truthy(repo.config("SaleInvoiceReturnBranchWise"))));
        } catch (RuntimeException e) { return fail(msg(e)); }
    }

    private static ResponseEntity<Map<String, Object>> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message);
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(m);
    }
    private static String msg(Exception e) {
        Throwable t = e; while (t.getCause() != null && (t.getMessage() == null || t.getMessage().isBlank())) t = t.getCause();
        String m = t.getMessage(); return m == null || m.isBlank() ? "Request failed." : m;
    }
}
