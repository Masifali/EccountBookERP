package com.mst.controllers;

import com.mst.models.dto.StoreDefineLookupsDto;
import com.mst.services.StoreDefineLookupsService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * JSON API of the two desktop lookup dialogs opened by the green "+" buttons of the Store screens:
 * Define_Department (ScreenName "Define_Department") at /api/store/define/department and
 * frmLookUpDefineAsset (ScreenName "frmLookUpDefineAsset") at /api/store/define/asset.
 * No page of their own — countx_store_define_lookups.js builds the dialogs on the host page.
 * Every call carries {@code host} = the ScreenName of the page that opened the dialog (rights).
 * Behaviour notes and deviations: {@link StoreDefineLookupsService}.
 */
@RestController
public class StoreDefineLookupsController {


    private final StoreDefineLookupsService service;
    public StoreDefineLookupsController(StoreDefineLookupsService service) { this.service = service; }

    // ------------------------------------------------------------------ Define_Department

    @GetMapping("/api/store/define/department/list")
    public ResponseEntity<?> departmentList(@RequestParam(required = false) String host) {
        return run(() -> service.departmentList(host), "Could not load the departments.");
    }

    @GetMapping("/api/store/define/department/{id}")
    public ResponseEntity<?> department(@PathVariable int id, @RequestParam(required = false) String host) {
        try {
            Map<String, Object> m = service.department(id, host);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, "Could not open the department.")));
        }
    }

    @PostMapping("/api/store/define/department/save")
    public ResponseEntity<?> departmentSave(@RequestBody StoreDefineLookupsDto.Department body) {
        return run(() -> service.saveDepartment(body, false), "Save failed.");
    }

    @PostMapping("/api/store/define/department/update")
    public ResponseEntity<?> departmentUpdate(@RequestBody StoreDefineLookupsDto.Department body) {
        return run(() -> service.saveDepartment(body, true), "Update failed.");
    }

    // ------------------------------------------------------------------ frmLookUpDefineAsset

    @GetMapping("/api/store/define/asset/lookups")
    public ResponseEntity<?> assetLookups(@RequestParam(required = false) String host) {
        return run(() -> service.assetLookups(host), "Could not load the screen.");
    }

    @GetMapping("/api/store/define/asset/departments")
    public ResponseEntity<?> assetDepartments(@RequestParam(required = false) String host) {
        return run(() -> service.assetDepartments(host), "Could not load the departments.");
    }

    @GetMapping("/api/store/define/asset/history")
    public ResponseEntity<?> assetHistory(@RequestParam(required = false) String host,
                                          @RequestParam(defaultValue = "entry") String dateType,
                                          @RequestParam(required = false) String fromDate,
                                          @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int categoryId) {
        return run(() -> service.assetHistory(host, dateType, fromDate, toDate, categoryId), "History failed.");
    }

    @GetMapping("/api/store/define/asset/{id}")
    public ResponseEntity<?> asset(@PathVariable int id, @RequestParam(required = false) String host) {
        try {
            Map<String, Object> m = service.asset(id, host);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, "Could not open the asset.")));
        }
    }

    @PostMapping("/api/store/define/asset/save")
    public ResponseEntity<?> assetSave(@RequestBody StoreDefineLookupsDto.Asset body) {
        return run(() -> service.saveAsset(body, false), "Save failed.");
    }

    @PostMapping("/api/store/define/asset/update")
    public ResponseEntity<?> assetUpdate(@RequestBody StoreDefineLookupsDto.Asset body) {
        return run(() -> service.saveAsset(body, true), "Update failed.");
    }

    // ------------------------------------------------------------------ plumbing

    @FunctionalInterface
    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
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
