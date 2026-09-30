package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.repositories.PartyLimitsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/**
 * Account Reports tile "Party Limits &amp; Balances" (R4, 2026-09-30). The desktop has no report of that name; its only
 * "Party Limits" screen is 738 frmInvSupCustLimits = Architecture.WinApp.SupfrmSupplierLimits ("Supplier / Customer
 * Limits": combo + Credit / Debit / Effected Date, Save / Update / New / Refresh, History grid). The previous page was a
 * static mock-up with a fabricated row; this serves the real screen. The desktop form checks no Save/Update right
 * (opening the screen is the right), so the screen's View right is what is enforced here.
 */
@Controller
public class PartyLimitsController {
    private static final int SCREEN_ID = 738;
    private final PartyLimitsRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public PartyLimitsController(PartyLimitsRepository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository; this.context = context; this.rights = rights;
    }

    @GetMapping({"/accounts/reports/party-limits-balances", "/accounts/reports/party-limits"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Supplier / Customer Limits");
        return "accounts/reports/party_limits_balances";
    }

    private UserAccount user() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }

    @GetMapping("/api/accounts/party-limits/lookups")
    @ResponseBody
    public Map<String, Object> lookups() { return repository.lookups(user()); }

    @GetMapping("/api/accounts/party-limits")
    @ResponseBody
    public List<Map<String, Object>> history() { return repository.history(user()); }

    @GetMapping("/api/accounts/party-limits/{id}")
    @ResponseBody
    public Map<String, Object> read(@PathVariable int id) {
        UserAccount u = user();
        Map<String, Object> row = repository.readById(id);
        ownRow(u, row);
        return row;
    }

    public static class SaveRequest {
        public int id;
        public int partyId;
        public String debit;
        public String credit;
        public String effectedDate;
    }

    /** btnsave_Click (RecId = 0) / update_Click (RecId from the double-clicked row) -> Insert(). */
    @PostMapping(value = "/api/accounts/party-limits", consumes = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody SaveRequest r) {
        UserAccount u = user();
        /* formvalidation(), in the desktop's order and wording. */
        if (r.partyId <= 0) return bad("Please Select Supplier");
        double debit = toDouble(r.debit), credit = toDouble(r.credit);
        if (debit == 0.0) return bad("Please Select Debit Credit");
        if (credit == 0.0) return bad("Please Select  Credit");
        if (r.effectedDate == null || r.effectedDate.isBlank()) return bad("Please Select Effected Date");
        LocalDate effected;
        try { effected = LocalDate.parse(r.effectedDate.trim()); } catch (Exception e) { return bad("Please Select Effected Date"); }
        if (r.id < 0) return bad("Invalid record");
        if (r.id > 0) ownRow(u, repository.readById(r.id));
        int id = repository.save(u, r.id, r.partyId, debit, credit, effected);
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("id", id);
        body.put("message", r.id == 0 ? "Save Successfully" : "Update Successfully");
        return ResponseEntity.ok(body);
    }

    /** The desktop only reaches ids through its organisation/company-filtered grid. */
    private static void ownRow(UserAccount u, Map<String, Object> row) {
        if (!Objects.equals(num(row.get("OrganizationId")), u.getOrganizationId()) || !Objects.equals(num(row.get("CompanyId")), u.getCompanyId()))
            throw new org.springframework.security.access.AccessDeniedException("Record does not belong to the current company");
    }

    private static Integer num(Object o) { return o instanceof Number ? ((Number) o).intValue() : o == null ? null : Integer.valueOf(o.toString().trim()); }

    /** Conversion.ToDouble: blank or unparsable text is 0. */
    private static double toDouble(String s) {
        if (s == null || s.isBlank()) return 0.0;
        try { return Double.parseDouble(s.trim().replace(",", "")); } catch (NumberFormatException e) { return 0.0; }
    }

    private static ResponseEntity<Map<String, Object>> bad(String message) {
        return ResponseEntity.badRequest().body(Map.of("message", message));
    }

    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    @ResponseBody
    public ResponseEntity<Map<String, Object>> database(org.springframework.dao.DataAccessException ex) {
        /* RAISERROR texts of the procedures ("You cannot define party limits in Previous Date...", "You cannot insert 2 limits ..."). */
        return ResponseEntity.status(409).contentType(MediaType.APPLICATION_JSON)
                .body(Map.of("message", Objects.toString(ex.getMostSpecificCause().getMessage(), "Save failed")));
    }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, Object>> invalid(IllegalArgumentException ex) {
        return ResponseEntity.badRequest().body(Map.of("message", Objects.toString(ex.getMessage(), "Invalid request")));
    }
}
