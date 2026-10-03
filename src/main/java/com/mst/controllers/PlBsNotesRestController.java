package com.mst.controllers;

import com.mst.services.PlBsNotesService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpSession;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/** BsPlSettingForm data: GridLoad / btnUpdate (tab 1) and GridNoteTitleFill / btnUpdateNote (tab 2). */
@RestController
@RequestMapping("/api/admin/pl-bs-notes")
public class PlBsNotesRestController {

    private final PlBsNotesService service;
    public PlBsNotesRestController(PlBsNotesService service) { this.service = service; }

    /** GridLoad(): note = PL | BS, classId = 2 (Assets) | 3 (Liabilities) | 0. */
    @GetMapping("/accounts")
    public ResponseEntity<Map<String, Object>> accounts(@RequestParam(defaultValue = "PL") String note,
                                                        @RequestParam(defaultValue = "0") int classId) {
        return run(() -> service.accounts(note, classId));
    }

    /** btnUpdate_Click. Body: token, mode (radio at update time), loadNote, loadClassId, rows[{ChartofAccountId, NoteTitle}]. */
    @SuppressWarnings("unchecked")
    @PostMapping("/accounts/update")
    public ResponseEntity<Map<String, Object>> updateAccounts(@RequestBody Map<String, Object> body, HttpSession session) {
        if (!tokenOk(body, session)) return expired();
        return run(() -> service.updateAccounts(
                String.valueOf(body.get("mode")),
                String.valueOf(body.get("loadNote")),
                toInt(body.get("loadClassId")),
                (List<Map<String, Object>>) body.get("rows")));
    }

    /** GridNoteTitleFill(): note = '' (All) | PL | BS. */
    @GetMapping("/notes")
    public ResponseEntity<Map<String, Object>> notes(@RequestParam(defaultValue = "") String note) {
        return run(() -> service.notes(note));
    }

    /** btnUpdateNote_Click. Body: token, loadNote, rows[{Id, NoteTitle}]. */
    @SuppressWarnings("unchecked")
    @PostMapping("/notes/update")
    public ResponseEntity<Map<String, Object>> updateNotes(@RequestBody Map<String, Object> body, HttpSession session) {
        if (!tokenOk(body, session)) return expired();
        return run(() -> service.updateNotes(
                body.get("loadNote") == null ? "" : String.valueOf(body.get("loadNote")),
                (List<Map<String, Object>>) body.get("rows")));
    }

    /* ------------------------------------------------------------------ helpers */

    private static boolean tokenOk(Map<String, Object> body, HttpSession session) {
        Object expected = session.getAttribute(PlBsNotesViewController.TOKEN);
        return expected != null && body != null && expected.equals(body.get("token"));
    }

    private static ResponseEntity<Map<String, Object>> expired() {
        return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail("This page has expired. Refresh it and try again."));
    }

    private static ResponseEntity<Map<String, Object>> run(Supplier<Map<String, Object>> call) {
        try {
            return ResponseEntity.ok(call.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        try { return v == null ? 0 : Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    private static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? "Request failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
