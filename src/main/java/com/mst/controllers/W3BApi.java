package com.mst.controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Shared controller plumbing of the W3B (Mfg Production hub, module 103) screens.
 * Errors: validation -> 400 {message}; missing right -> 403 {message}; anything else -> 500 {message}.
 */
final class W3BApi {

    private W3BApi() { }

    interface Call { Object get() throws Exception; }

    static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), fallback)));
        }
    }

    static ResponseEntity<?> notFound(String message) {
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(message));
    }

    static Throwable root(Throwable e) {
        Throwable r = e;
        int guard = 0;
        while (r.getCause() != null && r.getCause() != r && guard++ < 20) r = r.getCause();
        return r;
    }

    static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
