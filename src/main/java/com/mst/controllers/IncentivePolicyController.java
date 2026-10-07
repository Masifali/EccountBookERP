package com.mst.controllers;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.services.IncentivePolicyDesktopService;
import com.mst.services.StoreScreenRights;

/**
 * Screen 7 "Incentive Policy" - Architecture.WinApp.WholeSale.frmIncentivePolicy (Name "frmIncentivePolicy").
 * Every database call goes through IncentivePolicyDesktopService (the desktop's procedures and parameter sets).
 * The desktop reads the Save / Print / Update / CanView AllRecord rights and never uses them; the page shows them
 * as data attributes only.
 */
@Controller
@RequestMapping("/accounts/incentive-policy")
public class IncentivePolicyController {

    private static final String SCREEN_NAME = "frmIncentivePolicy";

    private final IncentivePolicyDesktopService svc;
    private final StoreScreenRights rights;

    public IncentivePolicyController(IncentivePolicyDesktopService svc, StoreScreenRights rights) {
        this.svc = svc;
        this.rights = rights;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("rights", rights.of(SCREEN_NAME));
        return "accounts/incentive_policy";
    }

    /** FrmExportSalesContract_Load: the five combo fills. */
    @GetMapping("/load")
    @ResponseBody
    public Map<String, Object> load() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.combos());
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** tabControl1_SelectedIndexChanged (History): HistoryFill. */
    @GetMapping("/history")
    @ResponseBody
    public Map<String, Object> history() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.history());
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** ReadById / History "Detail". */
    @GetMapping("/{id}")
    @ResponseBody
    public Map<String, Object> byId(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.getById(id));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** btnsave_Click (after the browser's Yes/No confirm). */
    @PostMapping("/save")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            Object d = body.get("detail");
            int id = svc.save(toInt(body.get("id")), str(body.get("dateFrom")), str(body.get("dateTo")), str(body.get("description")),
                    body.get("crAdvoiceTerms"), str(body.get("remarks")), d instanceof List ? (List<Map<String, Object>>) d : null);
            res.put("success", true);
            res.put("id", id);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    private static void fail(Map<String, Object> res, Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        res.put("success", false);
        res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
    }

    private static String str(Object o) { return o == null ? null : String.valueOf(o); }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
