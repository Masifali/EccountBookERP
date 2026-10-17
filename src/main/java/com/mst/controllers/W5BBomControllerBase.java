package com.mst.controllers;

import com.mst.services.W5BBomProfile;
import com.mst.services.W5BBomService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * W5B - shared handlers of the three Bill Of Material screens (774 / 845 / 598). The concrete subclasses carry the class-level
 * {@code @Controller @RequestMapping(<route>)} and pass their {@link W5BBomProfile}; every handler here is inherited, so each route
 * serves /api/lookups, /api/refresh, /api/doc-no, /api/history-combos, /api/history, /api/routing, /api/wages-activities,
 * /api/items-by-type, /api/{id}, POST /api/precheck, /api/save and /api/delete.
 *
 * Errors: validation -> 400 {message}; missing right -> 403 {message}; anything else -> 500 {message}.
 */
public abstract class W5BBomControllerBase {

    private final W5BBomService service;
    private final W5BBomProfile profile;

    protected W5BBomControllerBase(W5BBomService service, W5BBomProfile profile) {
        this.service = service;
        this.profile = profile;
    }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", profile.pageTitle);
        model.addAttribute("w5bVariant", profile.key);
        model.addAttribute("w5bRoute", profile.route);
        model.addAttribute("w5bTitle", profile.title);
        model.addAttribute("w5bHistoryTitle", profile.historyTitle);
        model.addAttribute("w5bScreen", profile.screen);
        model.addAttribute("w5bRpt", profile.rpt);
        return "production/w5b_bom";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(() -> service.lookups(profile), "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(() -> service.refresh(profile), "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo(profile)), "Could not generate the Doc No."); }

    @GetMapping("/api/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return run(() -> service.historyCombos(profile), "Could not load the history filters."); }

    @GetMapping("/api/items-by-type") @ResponseBody
    public ResponseEntity<?> itemsByType(@RequestParam(defaultValue = "0") int typeId) {
        return run(() -> service.itemsByProductionType(typeId), "Could not load the items.");
    }

    @GetMapping("/api/routing") @ResponseBody
    public ResponseEntity<?> routing(@RequestParam(defaultValue = "0") int routingNameId) {
        return run(() -> service.routing(profile, routingNameId), "Could not load the routing.");
    }

    @GetMapping("/api/wages-activities") @ResponseBody
    public ResponseEntity<?> wagesActivities(@RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String docDate) {
        return run(() -> service.wagesActivities(profile, itemId, docDate), "Could not load the service activities.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int bomId, @RequestParam(required = false) String bomType,
                                     @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.history(profile, fromDate, toDate, fromDocNo, toDocNo, bomId, bomType, itemId), "History failed.");
    }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(profile, id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that record.")));
        }
    }

    @PostMapping("/api/precheck") @ResponseBody
    public ResponseEntity<?> precheck(@RequestBody Map<String, Object> body) { return write(() -> service.precheck(profile, body)); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return write(() -> service.save(profile, body)); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return write(() -> service.delete(profile, id)); }

    // ------------------------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> write(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(r, "Save failed.")));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
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
