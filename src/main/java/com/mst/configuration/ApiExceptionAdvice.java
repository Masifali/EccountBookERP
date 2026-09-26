package com.mst.configuration;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

import javax.servlet.http.HttpServletRequest;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Turns a refused save into the reason the desktop would have shown.
 *
 * ---------------------------------------------------------------------------------------------
 * THE DEFECT THIS FIXES
 * ---------------------------------------------------------------------------------------------
 * Since Spring Boot 2.3 the default for `server.error.include-message` is `never`, and this
 * application sets no override and had no @ControllerAdvice. So an uncaught exception produced:
 *
 *     {"timestamp":"...","status":500,"error":"Internal Server Error","path":"/api/..."}
 *
 * with NO message field at all.
 *
 * That silently defeated every desktop validation string ported into the CMAGT save paths -
 * "Please Select Commission Agent / Broker", "Empty Bags Information Required", "Payment Detail
 * Total% not near to 100", "Mapped weight (x) cannot be greater than Offer weight (y)" and the
 * rest. The pages read `d.message || d.error` from the body, so the operator saw
 * "Internal Server Error" and had no way to know which field was wrong. The refusal worked; the
 * explanation never arrived.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY AN ADVICE RATHER THAN include-message=always
 * ---------------------------------------------------------------------------------------------
 * `include-message=always` would expose the text of EVERY exception, including SQL Server
 * messages carrying table and column names, to any caller. This exposes only the exceptions the
 * code raises deliberately as business refusals, and keeps genuine faults as a 500 whose detail
 * stays in the log.
 *
 * The distinction is the status code as much as the text:
 *   IllegalArgumentException -> 400, a refusal the operator can act on; logged at INFO
 *   IllegalStateException    -> 409, the write could not proceed; logged at WARN
 *   AccessDeniedException    -> 403, tenancy or rights
 *   anything else            -> left alone, so the existing 500 behaviour is unchanged
 *
 * Handlers that already catch their own exceptions and return {success,message} are unaffected -
 * an advice only sees what escapes.
 */
@RestControllerAdvice
public class ApiExceptionAdvice {

    private static final Logger LOG = LoggerFactory.getLogger(ApiExceptionAdvice.class);

    /**
     * A business refusal: the request was understood and declined for a stated reason. This is
     * what every ported desktop validation raises.
     *
     * 400 rather than 500 because nothing went wrong on the server - the document was invalid,
     * exactly as the desktop's MessageBox would have said.
     */
    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<Map<String, Object>> refused(IllegalArgumentException e,
                                                       HttpServletRequest req) {
        LOG.info("Refused {} : {}", req.getRequestURI(), e.getMessage());
        return ResponseEntity.badRequest().body(body(e.getMessage(), "Request refused."));
    }

    /**
     * The write could not proceed - for example a procedure that returned no id, so nothing was
     * written. 409 because the request was well-formed but the current state would not allow it.
     */
    @ExceptionHandler(IllegalStateException.class)
    public ResponseEntity<Map<String, Object>> conflict(IllegalStateException e,
                                                        HttpServletRequest req) {
        LOG.warn("Could not complete {} : {}", req.getRequestURI(), e.getMessage());
        return ResponseEntity.status(HttpStatus.CONFLICT)
                .body(body(e.getMessage(), "The operation could not be completed."));
    }

    /** Tenancy and rights refusals - the reason is safe to show and useful to the operator. */
    @ExceptionHandler(AccessDeniedException.class)
    public ResponseEntity<Map<String, Object>> denied(AccessDeniedException e,
                                                      HttpServletRequest req) {
        LOG.info("Denied {} : {}", req.getRequestURI(), e.getMessage());
        return ResponseEntity.status(HttpStatus.FORBIDDEN)
                .body(body(e.getMessage(), "You do not have permission for this action."));
    }

    /**
     * `message` and `error` are both populated because the pages in this application read one or
     * the other - `d.message || d.error` in the CMAGT scripts, `res.message` elsewhere - and a
     * body that satisfies only one of them leaves the other page showing nothing.
     */
    private static Map<String, Object> body(String message, String fallback) {
        String text = (message == null || message.trim().isEmpty()) ? fallback : message.trim();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return m;
    }
}
