package com.mst.controllers;

import java.util.HashMap;
import java.util.Map;

import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.services.AccountBudgetAttachmentService;
import com.mst.services.AccountBudgetDesktopService;

/** Screen 3 - Account Budget (desktop form Account_Definition.AccountBudget). */
@Controller
@RequestMapping("/accounts/account-budget")
public class AccountBudgetController {

    private final AccountBudgetDesktopService service;
    private final AccountBudgetAttachmentService attachments;

    public AccountBudgetController(AccountBudgetDesktopService service, AccountBudgetAttachmentService attachments) {
        this.service = service;
        this.attachments = attachments;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/account_budget";
    }

    /** AccountBudget_Load. */
    @GetMapping("/init")
    @ResponseBody
    public Map<String, Object> init() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(service.init());
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** btnRefresh_Click: ProjectFill, COABind, MonthFill. */
    @GetMapping("/lookups")
    @ResponseBody
    public Map<String, Object> lookups() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(service.lookups());
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** GenerateCode(). */
    @GetMapping("/docno")
    @ResponseBody
    public Map<String, Object> docNo() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("docNo", service.generateCode());
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** ReadById(Id). */
    @GetMapping("/{id:\\d+}")
    @ResponseBody
    public Map<String, Object> read(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(service.record(id));
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** grdHistoryDetailbind(Id). */
    @GetMapping("/{id:\\d+}/details")
    @ResponseBody
    public Map<String, Object> details(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", service.historyDetail(id));
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** HistoryGridBind(). */
    @PostMapping("/history")
    @ResponseBody
    public Map<String, Object> history(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", service.history(body));
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** Insert() (Save and Update). */
    @PostMapping("/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(service.save(body));
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** Attachment form content of one record (DMSAttachments.GetByID(id, "AccountBudget")). */
    @GetMapping("/{id:\\d+}/attachments")
    @ResponseBody
    public Map<String, Object> attachmentList(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", attachments.list(id));
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    /** History NoOfAttachments link: GetNoofAttachmentsByRefDocumentTypeID(id, 11). */
    @GetMapping("/{id:\\d+}/attachments/by-document-type")
    @ResponseBody
    public Map<String, Object> attachmentsByType(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", attachments.listByDocumentType(id));
            res.put("success", true);
        } catch (Exception e) { fail(res, e); }
        return res;
    }

    @GetMapping("/{id:\\d+}/attachments/{aid:\\d+}")
    public ResponseEntity<byte[]> download(@PathVariable("id") int id, @PathVariable("aid") int aid) {
        AccountBudgetAttachmentService.Download d = attachments.download(id, aid);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(d.name()).build().toString())
                .contentType(MediaType.APPLICATION_OCTET_STREAM)
                .body(d.bytes());
    }

    private static void fail(Map<String, Object> res, Exception e) {
        res.put("success", false);
        res.put("message", rootMessage(e));
    }

    private static String rootMessage(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() != null ? t.getMessage() : e.getMessage();
    }
}
