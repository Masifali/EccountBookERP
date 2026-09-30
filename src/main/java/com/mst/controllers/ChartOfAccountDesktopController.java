package com.mst.controllers;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.security.CurrentUserContext;
import com.mst.services.ChartOfAccountDesktopService;

/**
 * JSON endpoints of screen 9 (AcfrmDefCoa) used by countx_coa_definition_desktop.js. Tenancy, user,
 * branch and financial year come from CurrentUserContext inside the service, never from the request.
 */
@RestController
@RequestMapping("/accounts/chart_of_accounts/api")
public class ChartOfAccountDesktopController {

    private final ChartOfAccountDesktopService service;
    private final CurrentUserContext ctx;

    public ChartOfAccountDesktopController(ChartOfAccountDesktopService service, CurrentUserContext ctx) {
        this.service = service;
        this.ctx = ctx;
    }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() {
        return service.lookups();
    }

    @GetMapping("/parent-accounts")
    public List<Map<String, Object>> parentAccounts() {
        return service.parentAccounts();
    }

    @GetMapping("/account-levels")
    public List<Map<String, Object>> accountLevels() {
        return service.accountLevels();
    }

    @GetMapping("/new-code")
    public Map<String, Object> newCode(@RequestParam(value = "parentCode", required = false, defaultValue = "") String parentCode) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("row", service.newCodeByParent(parentCode));
        } catch (Exception ex) {
            res.put("success", false);
            res.put("message", ex.getMessage());
        }
        return res;
    }

    @GetMapping("/parent-code-by-account/{id}")
    public Map<String, Object> parentCodeByAccount(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        res.put("code", service.parentCodeByAccountId(id));
        return res;
    }

    @GetMapping("/third-level-by-type/{typeId}")
    public List<Map<String, Object>> thirdLevelByType(@PathVariable("typeId") int typeId) {
        return service.thirdLevelByType(typeId);
    }

    @GetMapping("/account/{id}")
    public Map<String, Object> readById(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        res.put("account", service.readById(id));
        return res;
    }

    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        return run(() -> service.save(body));
    }

    @PostMapping("/update")
    public Map<String, Object> update(@RequestBody Map<String, Object> body) {
        return run(() -> service.update(body));
    }

    @GetMapping("/history")
    public Map<String, Object> history(@RequestParam(value = "levels", required = false) String levels) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("rows", service.history(levels));
        } catch (Exception ex) {
            res.put("success", false);
            res.put("message", ex.getMessage());
        }
        return res;
    }

    /** Body: [{id, accountTitle, otherErpCode, isActive, currencyId}] - the rows to send, in order. */
    @PostMapping("/history/update")
    public Map<String, Object> historyUpdate(@RequestBody List<Map<String, Object>> rows) {
        return run(() -> {
            int org = ctx.currentOrganizationId();
            int company = ctx.currentCompanyId();
            service.historyUpdate(rows, service.erpFeatureForPage(org, company, 6));
            return "OK";
        });
    }

    @GetMapping("/print")
    public Map<String, Object> print() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("rows", service.printRows());
        } catch (Exception ex) {
            res.put("success", false);
            res.put("message", ex.getMessage());
        }
        return res;
    }

    private interface Action { String call(); }

    private static Map<String, Object> run(Action a) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("message", a.call());
            res.put("success", true);
        } catch (Exception ex) {
            res.put("success", false);
            res.put("message", rootMessage(ex));
        }
        return res;
    }

    private static String rootMessage(Throwable ex) {
        Throwable t = ex;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m != null ? m : ex.getMessage();
    }
}
