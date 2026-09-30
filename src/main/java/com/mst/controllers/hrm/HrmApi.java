package com.mst.controllers.hrm;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.core.AuthenticationException;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * Response plumbing shared by every HRM controller.
 *
 *   return HrmApi.run(() -> service.something(...));
 *
 * IllegalArgumentException / IllegalStateException -> 400 {success:false, message} - the desktop's
 * MessageBox text (validation, "Record not found.", a RAISERROR from the procedure);
 * AccessDeniedException -> 403; not signed in -> 401; anything else -> 500 with the root cause's message
 * (the desktop shows ex.Message too).
 */
public final class HrmApi {

    private HrmApi() { }

    public static ResponseEntity<?> run(Callable<?> work) {
        try {
            return ResponseEntity.ok(work.call());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, "Request failed.")));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        } catch (AuthenticationException e) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(fail(root(e, "Sign in again.")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, "Request failed.")));
        }
    }

    public static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    public static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
