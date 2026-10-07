package com.mst.controllers;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;

import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
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

import com.mst.controllers.ERPPrint.ReportPrintSupport;
import com.mst.models.Company;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.PdcReceiptDesktopService;
import com.mst.services.PdcReceiptDesktopService.Download;
import com.mst.services.PdcReceiptDesktopService.ReportFilter;
import com.mst.services.PdcReceiptDesktopService.SaveRequest;

/**
 * Screen 45 "Pdc Receipts" - Architecture.WinApp.Account_Definition.AcfrmDefPdcManagment (form Name "AcfrmDefPdcManagment").
 * Every database call goes through PdcReceiptDesktopService. The report buttons hand the same table to the same .rpt files the
 * desktop opens: 123-AcRptPdcInventorySlip, 125-PdcInventoryPending, 126-PdcInventoryPendingSummery, 127-PdcInventoryPendingGuriWise
 * (the shared Jasper pipeline); the 118 voucher slip is opened through the existing /reports/print/by-key/acc-118 contract.
 */
@Controller
@RequestMapping("/accounts/pdc-receipts")
public class PdcReceiptController extends ReportPrintSupport {

    @Autowired private PdcReceiptDesktopService svc;
    @Autowired private ICompanyRepository companyRepository;

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/pdc_receipts";
    }

    /** AcfrmDefPdcManagment_Load / toolStripButton1_Click: bankfill, AccountsFill, CreditAccountFill, DocumentNoFill and the rights. */
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

    /** Reset(): DocumentNoFill only. */
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

    /** tabControl1_SelectedIndexChanged HistoryFill(50) / LoadAll HistoryFill(). */
    @GetMapping("/history")
    @ResponseBody
    public Map<String, Object> history(@RequestParam(value = "n", defaultValue = "0") int n) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.history(n));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** txtdocno leave: GetIdByDocNo. */
    @GetMapping("/lookup")
    @ResponseBody
    public Map<String, Object> lookup(@RequestParam("docNo") int docNo) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("id", svc.idByDocNo(docNo));
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

    /** 118-Voucher: the voucher head id of a receipt (VoucherHeadIdGet), proved to be this company's first. */
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

    @GetMapping("/{id:\\d+}/attachments")
    @ResponseBody
    public Map<String, Object> attachments(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            svc.getById(id);
            res.put("rows", svc.attachments(id));
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

    /** btnDelete_Click. */
    @PostMapping("/{id:\\d+}/delete")
    @ResponseBody
    public Map<String, Object> delete(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            svc.delete(id);
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** grdfrm_ColumnButtonClick (not in save mode): PdcCheqDeleteByPdcInventoryId. */
    @PostMapping("/cheque/{id:\\d+}/delete")
    @ResponseBody
    public Map<String, Object> deleteCheque(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            svc.deleteCheque(id);
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    /** btnshow_Click: GeneralReprots.PdcReceiptsRegister into grdReprot. */
    @PostMapping("/report")
    @ResponseBody
    public Map<String, Object> report(@RequestBody ReportFilter f) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", svc.registerGrid(f));
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    // ------------------------------------------------------------------------------------------------- prints

    private List<Map<String, Object>> printRows(String kind, int id, ReportFilter f) {
        switch (kind) {
            case "slip": return svc.slip(id);
            case "register": case "guri": return svc.register(f);
            case "summary": return svc.summary(f);
            default: throw new IllegalArgumentException("Unknown report");
        }
    }

    private static String template(String kind) {
        switch (kind) {
            case "slip": return "123-AcRptPdcInventorySlip.rpt";
            case "register": return "125-PdcInventoryPending.rpt";
            case "summary": return "126-PdcInventoryPendingSummery.rpt";
            default: return "127-PdcInventoryPendingGuriWise.rpt";
        }
    }

    private static ReportFilter filter(String accountId, String debitId, String fromDate, String toDate, String cheqFrom, String cheqTo, String status) {
        ReportFilter f = new ReportFilter();
        f.accountId = accountId; f.debitId = debitId; f.fromDate = fromDate; f.toDate = toDate; f.cheqFrom = cheqFrom; f.cheqTo = cheqTo; f.status = status;
        return f;
    }

    /** "Record Not Found For Display" check made before the PDF is opened (the desktop shows it as a MessageBox). */
    @GetMapping("/print/{kind}/count")
    @ResponseBody
    public Map<String, Object> count(@PathVariable("kind") String kind, @RequestParam(value = "id", defaultValue = "0") int id,
            @RequestParam(value = "accountId", required = false) String accountId, @RequestParam(value = "debitId", required = false) String debitId,
            @RequestParam(value = "fromDate", required = false) String fromDate, @RequestParam(value = "toDate", required = false) String toDate,
            @RequestParam(value = "cheqFrom", required = false) String cheqFrom, @RequestParam(value = "cheqTo", required = false) String cheqTo,
            @RequestParam(value = "status", required = false) String status) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("count", printRows(kind, id, filter(accountId, debitId, fromDate, toDate, cheqFrom, cheqTo, status)).size());
            res.put("success", true);
        } catch (Exception e) {
            fail(res, e);
        }
        return res;
    }

    @GetMapping("/print/{kind}")
    public void print(HttpServletResponse response, @PathVariable("kind") String kind, @RequestParam(value = "id", defaultValue = "0") int id,
            @RequestParam(value = "accountId", required = false) String accountId, @RequestParam(value = "debitId", required = false) String debitId,
            @RequestParam(value = "fromDate", required = false) String fromDate, @RequestParam(value = "toDate", required = false) String toDate,
            @RequestParam(value = "cheqFrom", required = false) String cheqFrom, @RequestParam(value = "cheqTo", required = false) String cheqTo,
            @RequestParam(value = "status", required = false) String status) throws Exception {
        if (!"slip".equals(kind) && !"register".equals(kind) && !"summary".equals(kind) && !"guri".equals(kind)) { response.sendError(404, "Unknown report"); return; }
        currentUserContext.currentUserId();
        List<Map<String, Object>> rows = printRows(kind, id, filter(accountId, debitId, fromDate, toDate, cheqFrom, cheqTo, status));
        Company company = companyRepository.findById(currentUserContext.currentCompanyId()).orElse(null);
        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CompanyAddress", company == null ? "" : Objects.toString(company.getCompAddress(), ""));
        parameters.put("CompanyName", company == null ? "" : Objects.toString(company.getCompName(), ""));
        Map<String, Object> result = new HashMap<>();
        result.put("rows", rows);
        result.put("reportParameters", parameters);
        printReportData(response, template(kind), result);
    }

    private static void fail(Map<String, Object> res, Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        res.put("success", false);
        res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
    }
}
