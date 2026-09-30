package com.mst.controllers;

import java.time.LocalDate;
import java.util.HashMap;
import java.util.Map;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.services.CheqBookDesktopService;

/**
 * Screen 42 - Cheque Book Registration (desktop form AcfrmChequebookRegistration).
 *
 * Split out of AccountDefinitionModulesController on 2026-09-30 so this screen has its own controller
 * and its own page (accounts/cheque_book_registration.html). Every database call goes through
 * CheqBookDesktopService, i.e. the desktop's procedures:
 *   cmbBnkac                      -> GetAccountTitleByAccountTypeIds (type 15)
 *   CheqbookRegistratioinGridFill -> SP_CheqBookHeader_GetAllMethod
 *   Save_Click                    -> SP_CheqBookHeader_Insert + SP_CheqBookDetail_Insert per serial (one transaction)
 * The leaves grid Cancel / Re-Open buttons are the desktop's separate CheqbookStatus screen
 * (SP_CheqBookDetail_Update, rights of screen "CheqbookStatus").
 */
@Controller
@RequestMapping("/accounts/cheque_book")
public class ChequeBookRegistrationController {

    private final CheqBookDesktopService cheqBookDesktop;

    public ChequeBookRegistrationController(CheqBookDesktopService cheqBookDesktop) {
        this.cheqBookDesktop = cheqBookDesktop;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("rights", cheqBookDesktop.rights());
        model.addAttribute("statusRights", cheqBookDesktop.statusRights());
        return "accounts/cheque_book_registration";
    }

    /** Bank account combo + registration history grid. */
    @GetMapping("/data")
    @ResponseBody
    public Map<String, Object> data() {
        Map<String, Object> res = new HashMap<>();
        res.put("banks", cheqBookDesktop.bankAccounts());
        res.put("history", cheqBookDesktop.history());
        return res;
    }

    /** cmbBnkac only - loads fast, so the combo fills while the history is still coming. */
    @GetMapping("/banks")
    @ResponseBody
    public Object banks() {
        return cheqBookDesktop.bankAccounts();
    }

    /** CheqbookRegistratioinGridFill only (SP_CheqBookHeader_GetAllMethod 'GetAll', one row per leaf). */
    @GetMapping("/history")
    @ResponseBody
    public Object history() {
        return cheqBookDesktop.history();
    }

    @PostMapping("/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            Object d = body.get("docDate");
            LocalDate docDate = (d == null || String.valueOf(d).isEmpty()) ? null : LocalDate.parse(String.valueOf(d));
            Object bank = body.get("bankId");
            int bankId = bank instanceof Number ? ((Number) bank).intValue()
                    : (bank == null || String.valueOf(bank).isEmpty() ? 0 : Integer.parseInt(String.valueOf(bank)));
            res.put("message", cheqBookDesktop.save(bankId, docDate,
                    str(body.get("docNo")), str(body.get("prefix")),
                    str(body.get("serialFrom")), str(body.get("serialTo")), str(body.get("remarks"))));
            res.put("success", true);
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            res.put("success", false);
            res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
        }
        return res;
    }

    /** Cheque leaves of one registered book (right-hand grid). */
    @GetMapping("/details/{headerId}")
    @ResponseBody
    public Map<String, Object> details(@PathVariable("headerId") int headerId) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("success", true);
            res.put("leaves", cheqBookDesktop.leaves(headerId));
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    /** Cancel / Re-Open a leaf - the desktop's CheqbookStatus screen (Save = Cancel, Update = Open). */
    @PostMapping("/status")
    @ResponseBody
    public Map<String, Object> status(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            Object id = body.get("detailId");
            int detailId = id instanceof Number ? ((Number) id).intValue() : Integer.parseInt(String.valueOf(id));
            boolean cancel = Boolean.parseBoolean(String.valueOf(body.get("cancel")));
            res.put("message", cheqBookDesktop.changeStatus(detailId, cancel, str(body.get("remarks"))));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    private static String rootMessage(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() != null ? t.getMessage() : e.getMessage();
    }

    private static String str(Object o) {
        return o == null ? "" : String.valueOf(o);
    }
}
