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

import com.mst.services.W4ExchangeRateForwardBookingAttachmentService;
import com.mst.services.W4ExchangeRateForwardBookingService;

/** Screen 199 - Exchange Rate Forward Booking (desktop form Account_Definition.ExchangeRateForwardBooking). */
@Controller
@RequestMapping("/export/exchange-rate-forward-booking")
public class W4ExchangeRateForwardBookingController {

    private final W4ExchangeRateForwardBookingService service;
    private final W4ExchangeRateForwardBookingAttachmentService attachments;

    public W4ExchangeRateForwardBookingController(W4ExchangeRateForwardBookingService service,
                                                  W4ExchangeRateForwardBookingAttachmentService attachments) {
        this.service = service;
        this.attachments = attachments;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/w4_exchange_rate_forward_booking";
    }

    @GetMapping("/init")
    @ResponseBody
    public Map<String, Object> init() {
        Map<String, Object> res = new HashMap<>();
        try { res.putAll(service.init()); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @GetMapping("/lookups")
    @ResponseBody
    public Map<String, Object> lookups() {
        Map<String, Object> res = new HashMap<>();
        try { res.putAll(service.lookups()); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @GetMapping("/docno")
    @ResponseBody
    public Map<String, Object> docNo() {
        Map<String, Object> res = new HashMap<>();
        try { res.put("docNo", service.generateCode()); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @GetMapping("/{id:\\d+}")
    @ResponseBody
    public Map<String, Object> read(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try { res.putAll(service.record(id)); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @PostMapping("/history")
    @ResponseBody
    public Map<String, Object> history(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try { res.put("rows", service.history(body)); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @PostMapping("/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try { res.putAll(service.save(body)); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @GetMapping("/{id:\\d+}/attachments")
    @ResponseBody
    public Map<String, Object> attachmentList(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try { res.put("rows", attachments.list(id)); res.put("success", true); } catch (Exception e) { fail(res, e); }
        return res;
    }

    @GetMapping("/{id:\\d+}/attachments/{aid:\\d+}")
    public ResponseEntity<byte[]> download(@PathVariable("id") int id, @PathVariable("aid") int aid) {
        W4ExchangeRateForwardBookingAttachmentService.Download d = attachments.download(id, aid);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(d.name()).build().toString())
                .contentType(MediaType.APPLICATION_OCTET_STREAM)
                .body(d.bytes());
    }

    private static void fail(Map<String, Object> res, Exception e) {
        res.put("success", false);
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
    }
}
