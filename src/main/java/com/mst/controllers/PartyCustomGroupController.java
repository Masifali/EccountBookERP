package com.mst.controllers;

import java.util.ArrayList;
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
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.services.PartyCustomGroupDesktopService;

/**
 * Screen 786 "Party Custom Group" - Architecture.WinApp.Account_Definition.frmPartyCustomGroup.
 * A separate page from /accounts/custom_group (frmAccountCustomGroup, a different class). Every database call
 * goes through PartyCustomGroupDesktopService, which makes the desktop's procedure calls.
 */
@Controller
@RequestMapping("/accounts/party-custom-group")
public class PartyCustomGroupController {

    private final PartyCustomGroupDesktopService svc;

    public PartyCustomGroupController(PartyCustomGroupDesktopService svc) {
        this.svc = svc;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/party_custom_group";
    }

    /** frmPartyCustomGroup_Load: GridBind, AccountTypeCombo, LookUpBind. */
    @GetMapping("/load")
    @ResponseBody
    public Map<String, Object> load() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("gridGroups", svc.gridGroups());
            res.put("partyGroups", svc.partyGroups());
            res.put("comboGroups", svc.comboGroups());
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** BtnRefresh_Click: AccountTypeCombo + LookUpBind. */
    @GetMapping("/refresh")
    @ResponseBody
    public Map<String, Object> refresh() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("partyGroups", svc.partyGroups());
            res.put("comboGroups", svc.comboGroups());
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** GridBind only (Reset() / btnClear_Click). */
    @GetMapping("/grid")
    @ResponseBody
    public Map<String, Object> grid() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("gridGroups", svc.gridGroups());
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** grdLookUp_DoubleClick: AcLookUps.GetById. */
    @GetMapping("/group/{id}")
    @ResponseBody
    public Map<String, Object> groupById(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("group", svc.groupById(id));
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** btnAdd_Click: InsertLookUp / UpdateLookUp. */
    @PostMapping("/save")
    @ResponseBody
    public Map<String, Object> save(@RequestParam(value = "id", required = false, defaultValue = "0") int id,
                                    @RequestParam(value = "update", required = false, defaultValue = "false") boolean update,
                                    @RequestParam(value = "name", required = false) String name) {
        Map<String, Object> res = new HashMap<>();
        try {
            String msg = svc.saveGroup(id, update, name);
            res.put("success", true);
            res.put("saved", msg != null);
            res.put("message", msg);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** BtnShow_Click. */
    @GetMapping("/show")
    @ResponseBody
    public Map<String, Object> show(@RequestParam(value = "groupId", required = false, defaultValue = "0") int groupId,
                                    @RequestParam(value = "partyGroupId", required = false, defaultValue = "0") int partyGroupId) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.show(groupId, partyGroupId));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** btnAddAc_Click -> SaveAccountCustomGroup. Body: {"groupId":n,"ids":[SupplierCustomerId,...]}. */
    @PostMapping("/allocate")
    @ResponseBody
    public Map<String, Object> allocate(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            boolean ok = svc.allocate(toInt(body.get("groupId")), intList(body.get("ids")));
            res.put("success", true);
            res.put("message", ok ? "Saved Successfully" : null);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** btnDelete_Click. Body: {"rows":[{"customGroupId":n,"supplierCustomerId":n},...]}. */
    @PostMapping("/unallocate")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public Map<String, Object> unAllocate(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            Object rows = body.get("rows");
            svc.unAllocate(rows instanceof List ? (List<Map<String, Object>>) rows : null);
            res.put("success", true);
            res.put("message", "Record Remove successfully");
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

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static List<Integer> intList(Object o) {
        List<Integer> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<?>) o) out.add(toInt(x));
        return out;
    }
}
