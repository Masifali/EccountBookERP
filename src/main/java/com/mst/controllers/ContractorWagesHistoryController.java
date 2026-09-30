package com.mst.controllers;

import com.mst.services.ContractorWagesHistoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.sql.SQLException;
import java.util.LinkedHashMap;
import java.util.Map;

/** API behind /accounts/reports/wages-report (frmStockContractorWagesHistory.cs). */
@RestController
@RequestMapping("/api/accounts/reports/contractor-wages-history")
public class ContractorWagesHistoryController {

    @Autowired private ContractorWagesHistoryService service;

    @GetMapping("/load")
    public ResponseEntity<?> load() {
        try { return ResponseEntity.ok(service.load()); } catch (Exception e) { return error(e); }
    }

    @GetMapping("/branches")
    public ResponseEntity<?> branches() {
        try { return ResponseEntity.ok(service.branches()); } catch (Exception e) { return error(e); }
    }

    @PostMapping("/combos")
    public ResponseEntity<?> combos(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.combos(body.get("branchText") == null ? "" : String.valueOf(body.get("branchText")))); }
        catch (Exception e) { return error(e); }
    }

    @PostMapping("/grid")
    public ResponseEntity<?> grid(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.grid(body)); } catch (Exception e) { return error(e); }
    }

    private static ResponseEntity<?> error(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        HttpStatus status = HttpStatus.BAD_REQUEST;
        if (e instanceof DataAccessException) {
            Throwable t = e;
            while (t.getCause() != null && !(t instanceof SQLException)) t = t.getCause();
            msg = t.getMessage();
        } else if (!(e instanceof IllegalArgumentException) && !(e instanceof IllegalStateException)) {
            status = HttpStatus.INTERNAL_SERVER_ERROR;
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return ResponseEntity.status(status).body(m);
    }
}
