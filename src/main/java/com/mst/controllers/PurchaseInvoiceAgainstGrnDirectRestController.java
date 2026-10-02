package com.mst.controllers;

import com.mst.services.PurchaseInvoiceAgainstGrnDirectService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.sql.SQLException;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Purchase Invoice Against GRN Direct (screen 131, DocumentTypeId 138). Organization, company, branch,
 * financial year and user always come from the session inside the service; rights come from tblUserRights
 * for ScreenName "PurchaseInvoiceAgainstGrnDirect".
 *
 * Error reporting: the page shows whatever {@code message} the server returns. The handlers at the bottom
 * make sure a refused Save/Update carries the real reason - the innermost SQL Server message with its error
 * number, the procedure and line that raised it, and the procedure the application was executing - instead of
 * a generic text, and log the full chain under "138 save failed" so the app log explains every failure.
 */
@RestController
@RequestMapping("/api/purchase-invoice-against-grn-direct")
public class PurchaseInvoiceAgainstGrnDirectRestController {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseInvoiceAgainstGrnDirectRestController.class);
    private static final Pattern PROC = Pattern.compile("(?i)(?:EXEC|call)\\s+(?:\\[?dbo\\]?\\.)?\\[?([A-Za-z0-9_]+)\\]?");

    private final PurchaseInvoiceAgainstGrnDirectService service;

    public PurchaseInvoiceAgainstGrnDirectRestController(PurchaseInvoiceAgainstGrnDirectService service) { this.service = service; }

    /** Form Load: rights, lookups, configuration and the next document number. */
    @GetMapping("/init")
    public Map<String, Object> init() { return service.init(); }

    /** Toolbar Refresh (btnFrmRefresh_Click). */
    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-code")
    public Map<String, Object> nextCode() { return Map.of("docNo", service.nextDocNo()); }

    @GetMapping("/last-exchange-rate")
    public Map<String, Object> lastExchangeRate(@RequestParam int currencyId) { return service.lastExchangeRate(currencyId); }

    @GetMapping("/history-suppliers")
    public List<Map<String, Object>> historySuppliers() { return service.historySuppliers(); }

    /** Load GRN -> LoadInGridDetail. */
    @PostMapping("/load-grns")
    public Map<String, Object> loadGrns(@RequestBody Map<String, Object> body) { return service.loadGrns(body); }

    @PostMapping("/history")
    public List<Map<String, Object>> history(@RequestBody(required = false) Map<String, Object> body) {
        return service.history(body == null ? Map.of() : body);
    }

    @GetMapping("/{id}/detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable int id) { return service.getById(id); }

    /** Save (id 0) and Update (id &gt; 0). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @PostMapping("/delete/{id}")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

    // ------------------------------------------------------------------------------------------ error reporting

    /**
     * A database refusal. SQL Server's own text (a RAISERROR / THROW in a procedure or trigger, a missing
     * parameter, a conversion error, a constraint) is the real cause; it is returned verbatim together with
     * the error number, the raising procedure/line and the procedure the application was executing.
     * 409 for RAISERROR 50000/50001 (a business rule in the database), 500 otherwise.
     */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException error) {
        Throwable cause = error.getMostSpecificCause();
        int number = 0;
        StringBuilder text = new StringBuilder();
        if (cause instanceof SQLException sql) {
            number = sql.getErrorCode();
            LinkedHashSet<String> messages = new LinkedHashSet<>();
            for (SQLException x = sql; x != null && messages.size() < 5; x = x.getNextException()) {
                if (x.getMessage() != null && !x.getMessage().isBlank()) messages.add(x.getMessage().trim());
            }
            text.append(String.join(" | ", messages));
            text.append(" [SQL error ").append(number);
            String origin = serverOrigin(sql);
            if (!origin.isEmpty()) text.append(", raised in ").append(origin);
            String executing = executing(error.getMessage());
            if (!executing.isEmpty()) text.append(", while executing ").append(executing);
            text.append(']');
        } else {
            text.append(cause == null ? error.getMessage() : cause.getClass().getSimpleName() + ": " + cause.getMessage());
        }
        boolean businessRule = number == 50000 || number == 50001;
        if (businessRule) LOG.warn("138 save refused by the database: {}", text);
        else LOG.error("138 database operation failed: {}", text, error);
        return ResponseEntity.status(businessRule ? HttpStatus.CONFLICT : HttpStatus.INTERNAL_SERVER_ERROR)
                .body(body(text.toString() + (businessRule ? "" : " No partial invoice changes were saved.")));
    }

    /** Every other failure: the ported refusals keep their status, anything unexpected says what it was. */
    @ExceptionHandler(RuntimeException.class)
    public ResponseEntity<Map<String, Object>> failed(RuntimeException error) {
        if (error instanceof DataAccessException dae) return database(dae);
        if (error instanceof ResponseStatusException rse)
            return ResponseEntity.status(rse.getRawStatusCode()).body(body(rse.getReason() == null ? "This action is unavailable" : rse.getReason()));
        if (error instanceof IllegalArgumentException) {
            LOG.info("138 refused: {}", error.getMessage());
            return ResponseEntity.badRequest().body(body(blank(error.getMessage()) ? "Request refused." : error.getMessage()));
        }
        if (error instanceof IllegalStateException) {
            LOG.warn("138 could not complete: {}", error.getMessage());
            return ResponseEntity.status(HttpStatus.CONFLICT).body(body(blank(error.getMessage()) ? "The operation could not be completed." : error.getMessage()));
        }
        if (error instanceof AccessDeniedException)
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(body(blank(error.getMessage()) ? "You do not have permission for this action." : error.getMessage()));
        // an unexpected fault: say which one, with the innermost cause, instead of a bare 500
        Throwable root = error;
        while (root.getCause() != null && root.getCause() != root) root = root.getCause();
        String text = root.getClass().getSimpleName() + (blank(root.getMessage()) ? "" : ": " + root.getMessage());
        LOG.error("138 request failed: {}", text, error);
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(body(text + " No partial invoice changes were saved."));
    }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    private static Map<String, Object> body(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", message);
        m.put("error", message);
        return m;
    }

    /** "EXEC dbo.Sp_X @a=?..." -> "Sp_X" from the Spring message, so the failing step is named. */
    private static String executing(String springMessage) {
        if (springMessage == null) return "";
        Matcher m = PROC.matcher(springMessage);
        return m.find() ? m.group(1) : "";
    }

    /**
     * SQL Server JDBC: SQLServerException.getSQLServerError() -> procedure name and line number of the
     * statement that raised the error. Reached by reflection so this class does not depend on the driver.
     */
    private static String serverOrigin(SQLException sql) {
        try {
            Object err = sql.getClass().getMethod("getSQLServerError").invoke(sql);
            if (err == null) return "";
            Object proc = err.getClass().getMethod("getProcedureName").invoke(err);
            Object line = err.getClass().getMethod("getLineNumber").invoke(err);
            Object server = err.getClass().getMethod("getServerName").invoke(err);
            StringBuilder sb = new StringBuilder();
            if (proc != null && !proc.toString().isBlank()) sb.append("procedure ").append(proc);
            else sb.append("an ad-hoc statement or trigger");
            if (line != null && !"0".equals(line.toString())) sb.append(" line ").append(line);
            if (server != null && !server.toString().isBlank()) sb.append(" on ").append(server);
            return sb.toString();
        } catch (ReflectiveOperationException | RuntimeException ignored) {
            return "";
        }
    }
}
