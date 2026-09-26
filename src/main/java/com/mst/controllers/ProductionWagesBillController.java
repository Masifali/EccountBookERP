package com.mst.controllers;

import com.mst.services.ProductionWagesBillService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
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

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Contractor Labour Wages bill - frmwagesBillHeader.cs (DocumentTypeId 101).
 *
 * GET /production/wages-bill?refDocTypeId=&refDocId=&grossWeightTotal= - all optional. Without
 * them the page is the standalone form; with them it is the dialog the Input (80), Output (112),
 * Consumption (181) and Stock Conversion (66) saves open (obj.RefDocTypeId / RefDocId /
 * GrossWeightTotal, then ShowDialog). Inside an iframe the page calls
 * window.parent.P280WagesClosed({saved}) when it closes.
 *
 * No parameter here carries tenancy, the financial year, the branch, the role or a user id.
 */
@Controller
public class ProductionWagesBillController {

    private static final String API = "/api/production/wages-bill";

    @Autowired
    private ProductionWagesBillService service;

    @GetMapping("/production/wages-bill")
    public String page(Model model,
                       @RequestParam(defaultValue = "0") int refDocTypeId,
                       @RequestParam(defaultValue = "0") int refDocId,
                       @RequestParam(defaultValue = "0") double grossWeightTotal) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("refDocTypeId", refDocTypeId);
        model.addAttribute("refDocId", refDocId);
        model.addAttribute("grossWeightTotal", grossWeightTotal);
        return "production/wages_bill";
    }

    /** frmwagesBillHeader_Load:321. */
    @GetMapping(API + "/state")
    @ResponseBody
    public ResponseEntity<?> state(@RequestParam(defaultValue = "0") int refDocTypeId,
                                   @RequestParam(defaultValue = "0") int refDocId) {
        return run(() -> service.state(refDocTypeId, refDocId));
    }

    /** btnRefresh_Click:2531 - rights and contractors again (configurations come with /state). */
    @GetMapping(API + "/contractors")
    @ResponseBody
    public ResponseEntity<?> contractors() { return run(service::contractors); }

    /** CommonServices.GetWagesAccount(ids, 0, 1). */
    @GetMapping(API + "/wages-accounts")
    @ResponseBody
    public ResponseEntity<?> wagesAccounts(@RequestParam(required = false) String ids) {
        return run(() -> service.wagesAccounts(ids));
    }

    /** GenerateDocNo():465. */
    @GetMapping(API + "/next-doc-no")
    @ResponseBody
    public ResponseEntity<?> nextDocNo() {
        return run(() -> one("docNo", service.nextDocNo()));
    }

    /** GetIdByRefDocTypeIdAndRefDocId (LoadDataForWages:2186, with RefDocument). */
    @GetMapping(API + "/id-by-reference")
    @ResponseBody
    public ResponseEntity<?> idByReference(@RequestParam(defaultValue = "0") int refDocumentTypeId,
                                           @RequestParam(defaultValue = "0") int refDocNoId,
                                           @RequestParam(required = false) String refDocument) {
        return run(() -> one("id", service.idByReference(refDocumentTypeId, refDocNoId, refDocument)));
    }

    /** PendingGrnAndGdn():1974 / PendingTicket():2040. */
    @GetMapping(API + "/pending")
    @ResponseBody
    public ResponseEntity<?> pending(@RequestParam(defaultValue = "0") int refDocTypeId,
                                     @RequestParam(defaultValue = "0") int refDocId) {
        return run(() -> service.pending(refDocTypeId, refDocId));
    }

    /** LoadDataForWages:2209 - InvGrn.GetGrnDetialForContractorWages. */
    @GetMapping(API + "/reference-detail")
    @ResponseBody
    public ResponseEntity<?> referenceDetail(@RequestParam(defaultValue = "0") int id,
                                             @RequestParam(defaultValue = "0") int documentTypeId,
                                             @RequestParam(required = false) String reqType) {
        return run(() -> service.referenceDetail(id, documentTypeId, reqType));
    }

    /** CommonServices.CheckItemsFreeofcostforWages, one answer per row sent. */
    @PostMapping(API + "/free-of-cost")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> freeOfCost(@RequestBody Map<String, Object> body) {
        return run(() -> service.freeOfCost((List<Map<String, Object>>) body.getOrDefault("rows", new ArrayList<>())));
    }

    /** CommonServices.GetWagesRate, one answer per row sent. */
    @PostMapping(API + "/wages-rate")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> wagesRate(@RequestBody Map<String, Object> body) {
        return run(() -> service.wagesRates((List<Map<String, Object>>) body.getOrDefault("rows", new ArrayList<>())));
    }

    /** InvContractorWagesBillHeader.GetByID. */
    @GetMapping(API + "/bill/{id}")
    @ResponseBody
    public ResponseEntity<?> bill(@PathVariable int id) { return run(() -> service.bill(id)); }

    /** RetreivalDetailGridDeletedData():2332. */
    @GetMapping(API + "/previous-records")
    @ResponseBody
    public ResponseEntity<?> previousRecords(@RequestParam(defaultValue = "0") int refDocTypeId,
                                             @RequestParam(defaultValue = "0") int refDocId) {
        return run(() -> service.previousRecords(refDocTypeId, refDocId));
    }

    /** ValidationOnformClose():2597 - WagesDeleteByRefDocTypeAndId after the operator said Yes. */
    @PostMapping(API + "/delete-by-reference")
    @ResponseBody
    public ResponseEntity<?> deleteByReference(@RequestBody Map<String, Object> body) {
        return run(() -> {
            service.wagesDeleteByRefDocTypeAndId(num(body.get("refDocTypeId")), num(body.get("refDocId")));
            return one("success", true);
        });
    }

    /** DocumentTypeFillForCombo():569 / btnRefreshHistory_Click:3556. */
    @GetMapping(API + "/history-document-types")
    @ResponseBody
    public ResponseEntity<?> historyDocumentTypes() { return run(service::historyDocumentTypes); }

    /** bindHistory():3247. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String from,
                                     @RequestParam(required = false) String to,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo,
                                     @RequestParam(defaultValue = "0") int refDocumentTypeId) {
        return run(() -> service.history(from, to, fromDocNo, toDocNo, refDocumentTypeId));
    }

    /** History "Voucher" button:3420 - VoucherHeadIdGet(Id, 101). */
    @GetMapping(API + "/voucher-head-id")
    @ResponseBody
    public ResponseEntity<?> voucherHeadId(@RequestParam(defaultValue = "0") int id) {
        return run(() -> one("voucherHeadId", service.voucherHeadId(id)));
    }

    /** BtnCancelPendingRecords_Click:4037. */
    @PostMapping(API + "/cancel-pending")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> cancelPending(@RequestBody Map<String, Object> body) {
        return write(() -> one("message",
                service.cancelPending((List<Map<String, Object>>) body.getOrDefault("rows", new ArrayList<>()))));
    }

    /** btnsave_Click:3029 / btnUpdate_Click:3042 -> Insert():2702. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        return write(() -> service.save(body));
    }

    // ------------------------------------------------------------------------------ helpers

    private static int num(Object v) {
        return com.mst.repositories.ProductionWagesBillRepository.toInt(v);
    }

    private static Map<String, Object> one(String k, Object v) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k, v);
        return m;
    }

    private static ResponseEntity<?> run(Supplier<Object> s) {
        try {
            return ResponseEntity.ok(s.get());
        } catch (ProductionWagesBillService.WagesValidationException e) {
            return ResponseEntity.badRequest().body(one("message", e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    private static ResponseEntity<?> write(Supplier<Object> s) {
        try {
            Object r = s.get();
            if (r instanceof Map) {
                @SuppressWarnings("unchecked")
                Map<String, Object> m = new LinkedHashMap<>((Map<String, Object>) r);
                m.put("success", true);
                return ResponseEntity.ok(m);
            }
            return ResponseEntity.ok(r);
        } catch (ProductionWagesBillService.WagesValidationException e) {
            Map<String, Object> m = one("success", false);
            m.put("message", e.getMessage());
            return ResponseEntity.badRequest().body(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /**
     * The desktop shows ex.Message. For a SQL error that is the RAISERROR text, which Spring wraps;
     * the most specific cause carries it unchanged.
     */
    private static Map<String, Object> fail(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        if (e instanceof DataAccessException && ((DataAccessException) e).getMostSpecificCause() != null) {
            msg = ((DataAccessException) e).getMostSpecificCause().getMessage();
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return m;
    }
}
