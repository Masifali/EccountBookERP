package com.mst.controllers;

import java.util.LinkedHashMap;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import org.springframework.web.server.ResponseStatusException;

/** Shows the desktop's own message text (the MessageBox of the form) for the Paddy Purchase screens. */
@RestControllerAdvice(assignableTypes = PaddyPurchaseApi.class)
public class PaddyPurchaseAdvice {
    private static ResponseEntity<Map<String, Object>> body(int status, String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false); m.put("message", message == null ? "Request failed" : message);
        return ResponseEntity.status(status).body(m);
    }
    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class})
    public ResponseEntity<Map<String, Object>> refused(RuntimeException error) { return body(400, error.getMessage()); }
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public ResponseEntity<Map<String, Object>> denied(org.springframework.security.access.AccessDeniedException error) { return body(403, error.getMessage()); }
    @ExceptionHandler(ResponseStatusException.class)
    public ResponseEntity<Map<String, Object>> status(ResponseStatusException error) { return body(error.getRawStatusCode(), error.getReason()); }
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(org.springframework.dao.DataAccessException error) {
        Throwable cause = error.getMostSpecificCause();
        if (cause instanceof java.sql.SQLException && ((java.sql.SQLException) cause).getErrorCode() >= 50000) return body(409, cause.getMessage());
        org.slf4j.LoggerFactory.getLogger(PaddyPurchaseAdvice.class).error("Paddy purchase database operation failed", error);
        return body(500, "The database request failed. Nothing was saved.");
    }
}
