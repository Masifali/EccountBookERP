package com.mst.controllers;

import java.util.HashMap;
import java.util.Map;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.services.SpecialScreenRightsService;

/** SpecialSreenRightsUserWise dialog + SpecialRightsImplement (see SpecialScreenRightsService). */
@RestController
@RequestMapping("/api/special-rights")
public class SpecialScreenRightsController {

    private final SpecialScreenRightsService service;

    public SpecialScreenRightsController(SpecialScreenRightsService service) { this.service = service; }

    @GetMapping("/lookups")
    public ResponseEntity<?> lookups(@RequestParam("screenId") int screenId) {
        return run(() -> service.lookups(screenId));
    }

    @GetMapping("/rights")
    public ResponseEntity<?> rights(@RequestParam("screenId") int screenId) { return run(() -> service.rights(screenId)); }

    @GetMapping("/history")
    public ResponseEntity<?> history(@RequestParam("screenId") int screenId) { return run(() -> service.history(screenId)); }

    @GetMapping("/{id}")
    public ResponseEntity<?> byId(@PathVariable("id") int id) { return run(() -> service.byId(id)); }

    @GetMapping("/mine")
    public ResponseEntity<?> mine(@RequestParam("screenId") int screenId) { return run(() -> service.mine(screenId)); }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> b) {
        return run(() -> {
            Map<String, Object> r = new HashMap<>();
            r.put("message", service.save(i(b.get("id")), i(b.get("screenId")), i(b.get("rightId")), i(b.get("userId")),
                    Boolean.parseBoolean(String.valueOf(b.get("isActive")))));
            return r;
        });
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            Map<String, Object> m = new HashMap<>();
            m.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
            return ResponseEntity.badRequest().body(m);
        }
    }

    private static int i(Object o) {
        if (o == null) return 0;
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0; }
    }
}
