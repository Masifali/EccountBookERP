package com.mst.controllers;

import com.mst.models.dto.DepartmentRequestDto;
import com.mst.services.DepartmentRequestService;
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
 * Store Management, ModuleId 24 — screen 338 "Department Request", {@code frmDepartmentRequest.cs}
 * (ScreenName "frmDepartmentRequest"), DocumentTypeId 450, at /store/department-request.
 * See {@link DepartmentRequestService} for the desktop-behaviour notes and deviations.
 */
@Controller
public class DepartmentRequestController {

    private static final String API = "/api/store/department-request";

    private final DepartmentRequestService service;
    public DepartmentRequestController(DepartmentRequestService service) { this.service = service; }

    @GetMapping("/store/department-request")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/department_request";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() {
        return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not generate the Doc No.");
    }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int itemConditionId,
                                   @RequestParam(required = false) String docDate) {
        return run(() -> service.stock(itemId, itemConditionId, docDate), "Could not read the stock.");
    }

    @GetMapping(API + "/barcode")
    @ResponseBody
    public ResponseEntity<?> barcode(@RequestParam(required = false) String barcode) {
        return run(() -> Collections.singletonMap("itemId", service.itemIdByBarcode(barcode)), "Could not read the barcode.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateField,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom,
                                     @RequestParam(defaultValue = "0") int docNoTo) {
        return run(() -> service.history(dateField, fromDate, toDate, docNoFrom, docNoTo), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        try {
            List<Map<String, Object>> rows = service.slip(id);
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody DepartmentRequestDto dto) {
        return write(() -> service.save(dto), "Save failed.");
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return write(() -> service.delete(id), "Delete failed.");
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.badRequest().body(fail(msg(r, fallback)));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
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
