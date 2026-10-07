package com.mst.controllers;

import java.util.HashMap;
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
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.services.ReceiptByContractDesktopService;
import com.mst.services.ReceiptByContractDesktopService.Download;
import com.mst.services.ReceiptByContractDesktopService.SaveRequest;

/**
 * Screen 18 "Receipt By Contract" - Architecture.WinApp.Account_Definition.ReceiptByContract (form Name "ReceiptByContract").
 * Every database call goes through ReceiptByContractDesktopService, which makes the desktop's procedure calls. The 118 voucher
 * slip is opened by the page through the existing /reports/print/by-key/acc-118 contract once the voucher head id is known.
 */
@Controller
@RequestMapping("/accounts/receipt-by-contract")
public class ReceiptByContractController {

    private final ReceiptByContractDesktopService svc;

    public ReceiptByContractController(ReceiptByContractDesktopService svc) {
        this.svc = svc;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/receipt_by_contract";
    }

    /** ReceiptByInvoice_Load / btnRefresh_Click. */
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

    /** VoucherNofill. */
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

    /** CmbTranType_Leave: the credit accounts of one account type (2 Cash, 15 Bank, 3 Party). */
    @GetMapping("/accounts")
    @ResponseBody
    public Map<String, Object> accounts(@RequestParam("type") int type) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.accountsByType(type));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** AccountCurrentBalance. */
    @GetMapping("/balance/account")
    @ResponseBody
    public Map<String, Object> accountBalance(@RequestParam("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.accountBalance(id));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** CustomerCurrentBalance. */
    @GetMapping("/balance/customer")
    @ResponseBody
    public Map<String, Object> customerBalance(@RequestParam("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.customerBalance(id));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** AdvanceAccountCurrentBalance. */
    @GetMapping("/balance/advance")
    @ResponseBody
    public Map<String, Object> advanceBalance(@RequestParam("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.putAll(svc.advanceBalance(id));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** ChkBoxWthHolding_CheckedChanged: ReadTaxSchedule. */
    @GetMapping("/tax-schedule")
    @ResponseBody
    public Map<String, Object> taxSchedule(@RequestParam("taxTypeId") int taxTypeId, @RequestParam("date") String date) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.taxSchedule(taxTypeId, date));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** GridContractFill. */
    @GetMapping("/pending")
    @ResponseBody
    public Map<String, Object> pending(@RequestParam("supplierId") int supplierId) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.pending(supplierId));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** btnsearchhistory_Click / HistoryGridFill. */
    @GetMapping("/history")
    @ResponseBody
    public Map<String, Object> history(@RequestParam(value = "supplierId", defaultValue = "0") int supplierId,
            @RequestParam(value = "from", required = false) String from, @RequestParam(value = "to", required = false) String to,
            @RequestParam(value = "fromDocNo", defaultValue = "0") int fromDocNo, @RequestParam(value = "toDocNo", defaultValue = "0") int toDocNo) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.history(supplierId, from, to, fromDocNo, toDocNo));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** ReadById. */
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

    /** print_Click: the voucher head id of a receipt (VoucherHeadIdGet), proved to be this company's first. */
    @GetMapping("/{id:\\d+}/voucher")
    @ResponseBody
    public Map<String, Object> voucher(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            svc.getById(id);
            res.put("voucherHeadId", svc.voucherHeadId(id));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** Gridhistory_LinkClicked (NoOfAttachemtns): GetNoofAttachmentsByRefDocumentTypeID(id, 29). */
    @GetMapping("/{id:\\d+}/attachments")
    @ResponseBody
    public Map<String, Object> attachments(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            svc.getById(id);
            res.put("rows", svc.attachmentsByRef(id));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

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
            res.putAll(svc.save(body));
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
