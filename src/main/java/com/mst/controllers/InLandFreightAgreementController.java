package com.mst.controllers;

import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

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

import com.mst.services.InLandFreightAgreementDesktopService;
import com.mst.services.InLandFreightAgreementDesktopService.Download;
import com.mst.services.InLandFreightAgreementDesktopService.SaveRequest;

/**
 * Screen 8 "InLand Freight Agreement" - Architecture.WinApp.Account_Definition.InLandFreightAgreement
 * (form Name "InLandFreightAgreement"). Every database call goes through InLandFreightAgreementDesktopService, which makes
 * the desktop's procedure calls. The desktop form reads no screen right (the Rightsobjects field is never used), so the
 * page has no right gate beyond the route's own (menu) right.
 */
@Controller
@RequestMapping("/accounts/inland-freight-agreement")
public class InLandFreightAgreementController {

    private final InLandFreightAgreementDesktopService svc;

    public InLandFreightAgreementController(InLandFreightAgreementDesktopService svc) {
        this.svc = svc;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/inland_freight_agreement";
    }

    /** InLandFreightAgreement_Load / btnRefresh_Click: the four combo fills and GenerateCode. */
    @GetMapping("/load")
    @ResponseBody
    public Map<String, Object> load() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.load());
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** Reset(): GenerateCode only. */
    @GetMapping("/code")
    @ResponseBody
    public Map<String, Object> code() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("docNo", svc.generateCode());
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** tabControl1_SelectedIndexChanged (History): HistoryGridFill. */
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

    /** ReadById_Update / History "Detail". */
    @GetMapping("/{id:\\d+}")
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

    /** GrdHistory_LinkClicked (NoOfAttachments): the attachments of one agreement. */
    @GetMapping("/{id:\\d+}/attachments")
    @ResponseBody
    public Map<String, Object> attachments(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            svc.getById(id);                                    // the agreement must belong to the signed-in company
            List<Map<String, Object>> rows = svc.attachments(id);
            res.put("rows", rows);
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** Opening one attached file. */
    @GetMapping("/{id:\\d+}/attachments/{attId:\\d+}")
    public ResponseEntity<byte[]> download(@PathVariable("id") int id, @PathVariable("attId") int attId) {
        try {
            Download d = svc.download(id, attId);
            String ascii = d.name.replaceAll("[^A-Za-z0-9._ -]", "_");
            return ResponseEntity.ok()
                    .header(HttpHeaders.CONTENT_DISPOSITION, "inline; filename=\"" + ascii + "\"; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name, java.nio.charset.StandardCharsets.UTF_8).replace("+", "%20"))
                    .header("X-Content-Type-Options", "nosniff")
                    .contentType(MediaType.APPLICATION_OCTET_STREAM)
                    .body(d.bytes);
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            return ResponseEntity.status(404).contentType(MediaType.TEXT_PLAIN).body(String.valueOf(t.getMessage()).getBytes(java.nio.charset.StandardCharsets.UTF_8));
        }
    }

    /** btnsave_Click / btnUpdate_Click (after the browser's Yes/No confirm): Insert(). */
    @PostMapping("/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody SaveRequest body) {
        Map<String, Object> res = new HashMap<>();
        try {
            Map<String, Object> saved = svc.save(body);
            res.putAll(saved);
            res.put("success", true);
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

}
