package com.mst.controllers;

import com.mst.services.desktopvoucher.BillsVoucherService;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import com.mst.services.desktopvoucher.PartyVoucherService;
import com.mst.services.desktopvoucher.PrematureReceiptsService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Pages and APIs for the Accounts Transaction screens rebuilt by recheck group C:
 *   16 Party Payment        GET /accounts/vouchers/party-payment       (frmPartyPaymentVoucher, 35)
 *   17 Party Receipts       GET /accounts/vouchers/party-receipt       (frmPartyReceiptVoucher, 34)
 *   25 Premature Receipts   GET /accounts/vouchers/premature-receipts  (AccountsPrematurePaymentsReceipts, 159)
 *   38 Bills Payable        GET /accounts/vouchers/bills-payables      (frmBillsPayables, 6)
 *   39 Bills Receivables    GET /accounts/vouchers/bills-receivables   (frmBillsReceivables, 7)
 *
 * The literal page paths take precedence over AccountsModuleViewController's
 * "/accounts/vouchers/{voucherType}" pattern, so that shared controller is not edited; its
 * "party-payment" / "party-receipt" cases are no longer reached.
 *
 * Tenancy never comes from a request: every service reads it from CurrentUserContext.
 */
@Controller
public class DesktopVoucherScreensController {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopVoucherScreensController.class);
    private static final String API = "/accounts/api/desktop-vouchers";

    @Autowired private PartyVoucherService party;
    @Autowired private PrematureReceiptsService premature;
    @Autowired private BillsVoucherService bills;
    @Autowired private DesktopVoucherSupport support;

    // =================================================================================== pages

    @GetMapping("/accounts/vouchers/party-payment")
    public String partyPayment(Model m) { return partyPage(m, PartyVoucherService.PPV, "Party Payment Voucher"); }

    @GetMapping("/accounts/vouchers/party-receipt")
    public String partyReceipt(Model m) { return partyPage(m, PartyVoucherService.PRV, "Party Receipt Voucher"); }

    private String partyPage(Model m, int docType, String title) {
        m.addAttribute("activeMenu", "accounts");
        m.addAttribute("moduleTitle", title);
        m.addAttribute("documentTypeId", docType);
        return "accounts/vouchers/party_voucher_desktop";
    }

    @GetMapping("/accounts/vouchers/premature-receipts")
    public String prematureReceipts(Model m) {
        m.addAttribute("activeMenu", "accounts");
        m.addAttribute("moduleTitle", "Accounts Premature Receipts");
        return "accounts/vouchers/premature_receipts";
    }

    @GetMapping("/accounts/vouchers/bills-payables")
    public String billsPayables(Model m) { return billsPage(m, BillsVoucherService.PAYABLES, "Bills Payables"); }

    @GetMapping("/accounts/vouchers/bills-receivables")
    public String billsReceivables(Model m) { return billsPage(m, BillsVoucherService.RECEIVABLES, "Bills Receivables"); }

    private String billsPage(Model m, int docType, String title) {
        m.addAttribute("activeMenu", "accounts");
        m.addAttribute("moduleTitle", title);
        m.addAttribute("documentTypeId", docType);
        return "accounts/vouchers/bills_voucher_desktop";
    }

    // ============================================================================ shared APIs

    /** VoucherHead.ReadByCurrentBalanceByDateAndAccountId - the balance labels. */
    @GetMapping(API + "/balance")
    @ResponseBody
    public ResponseEntity<?> balance(@RequestParam("accountId") int accountId, @RequestParam("date") String date) {
        return run(() -> support.currentBalance(date, accountId));
    }

    /** CheqBookHeader.OutstandingCheqNo - Party Payment's CheqNoFill(). */
    @GetMapping(API + "/cheques")
    @ResponseBody
    public ResponseEntity<?> cheques(@RequestParam("bankId") int bankId,
                                     @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return run(() -> {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : support.outstandingCheques(bankId, recId)) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", r.get("Id")); x.put("CheqNo", r.get("CheqNo"));
                out.add(x);
            }
            return out;
        });
    }

    // ======================================================================= party (16, 17)

    @GetMapping(API + "/party/{docType}/lookups")
    @ResponseBody
    public ResponseEntity<?> partyLookups(@PathVariable("docType") int docType) { return run(() -> party.lookups(docType)); }

    @GetMapping(API + "/party/{docType}/next-code")
    @ResponseBody
    public ResponseEntity<?> partyNextCode(@PathVariable("docType") int docType) {
        return run(() -> one("voucherCode", party.nextCode(docType)));
    }

    @GetMapping(API + "/party/{docType}/history-accounts")
    @ResponseBody
    public ResponseEntity<?> partyHistoryAccounts(@PathVariable("docType") int docType) { return run(() -> party.historyAccounts(docType)); }

    @PostMapping(API + "/party/{docType}/save")
    @ResponseBody
    public ResponseEntity<?> partySave(@PathVariable("docType") int docType, @RequestBody Map<String, Object> body) {
        return run(() -> party.save(docType, body));
    }

    @GetMapping(API + "/party/{docType}/load/{id}")
    @ResponseBody
    public ResponseEntity<?> partyLoad(@PathVariable("docType") int docType, @PathVariable("id") int id) {
        return runOr404(() -> party.load(docType, id));
    }

    @GetMapping(API + "/party/{docType}/history")
    @ResponseBody
    public ResponseEntity<?> partyHistory(@PathVariable("docType") int docType, @RequestParam Map<String, String> q) {
        return run(() -> party.history(docType, q));
    }

    @GetMapping(API + "/party/{docType}/history-detail/{id}")
    @ResponseBody
    public ResponseEntity<?> partyHistoryDetail(@PathVariable("docType") int docType, @PathVariable("id") int id) {
        return run(() -> party.historyDetail(docType, id));
    }

    // ============================================================================ premature (25)

    @GetMapping(API + "/premature/lookups")
    @ResponseBody
    public ResponseEntity<?> prLookups() { return run(() -> premature.lookups()); }

    @GetMapping(API + "/premature/senders")
    @ResponseBody
    public ResponseEntity<?> prSenders(@RequestParam(value = "costCenterId", defaultValue = "0") int cc) {
        return run(() -> premature.senders(cc));
    }

    @GetMapping(API + "/premature/receivers")
    @ResponseBody
    public ResponseEntity<?> prReceivers(@RequestParam("voucherTypeId") int vt) { return run(() -> premature.receivers(vt)); }

    @GetMapping(API + "/premature/doc-no")
    @ResponseBody
    public ResponseEntity<?> prDocNo() { return run(() -> one("docNo", premature.generateCode())); }

    @GetMapping(API + "/premature/pending")
    @ResponseBody
    public ResponseEntity<?> prPending(@RequestParam(value = "costCenterId", defaultValue = "0") int cc) {
        return run(() -> premature.pending(cc));
    }

    @GetMapping(API + "/premature/history-combos")
    @ResponseBody
    public ResponseEntity<?> prHistoryCombos(@RequestParam(value = "costCenterId", defaultValue = "0") int cc) {
        return run(() -> premature.historyCombos(cc));
    }

    @GetMapping(API + "/premature/history")
    @ResponseBody
    public ResponseEntity<?> prHistory(@RequestParam Map<String, String> q) { return run(() -> premature.history(q)); }

    @PostMapping(API + "/premature/read")
    @ResponseBody
    public ResponseEntity<?> prRead(@RequestBody Map<String, Object> q) {
        return run(() -> {
            if (!Boolean.TRUE.equals(support.rights(PrematureReceiptsService.SCREEN).get("update")))
                throw new SecurityException("You do not have the Update right for this screen.");
            return premature.readByMultiParams(q);
        });
    }

    @PostMapping(API + "/premature/save")
    @ResponseBody
    public ResponseEntity<?> prSave(@RequestBody Map<String, Object> body) { return run(() -> premature.save(body)); }

    /**
     * The pending grid's Confirm / Cancel buttons (DocumentTypeId 159) and the toolbar's
     * "Confirm Vouchers" (DocumentTypeId 0, every checked row). Each row is its own
     * UpdateRecord call - its own transaction - exactly as the desktop loops them; a failure
     * stops the loop with the rows before it already committed.
     */
    @PostMapping(API + "/premature/status")
    @ResponseBody
    public ResponseEntity<?> prStatus(@RequestBody Map<String, Object> body) {
        return run(() -> {
            String action = String.valueOf(body.get("action"));
            boolean bulk = Boolean.TRUE.equals(body.get("bulk"));
            if (!"Confirm".equals(action) && !"Cancel".equals(action)) throw new IllegalArgumentException("Unknown action");
            Map<String, Boolean> r = support.rights(PrematureReceiptsService.SCREEN);
            if ("Cancel".equals(action) && !Boolean.TRUE.equals(r.get("canChangeOrderStatusToCancel")))
                throw new SecurityException("You Don't Have Right To Cancel the Voucher");
            if ("Confirm".equals(action) && !Boolean.TRUE.equals(r.get("canChangeOrderStatusToComplete")))
                throw new SecurityException("You Don't Have Right To Confirm the Voucher");
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
            if (rows.isEmpty()) throw new IllegalArgumentException("Please select row first");
            for (Map<String, Object> row : rows) {
                if (DesktopVoucherSupport.toInt(row.get("receiverAcId")) == 0)
                    throw new IllegalArgumentException("Receiver Account not found. Please check!");
                premature.updateRecord(row, action, bulk ? 0 : PrematureReceiptsService.DOC_TYPE);
            }
            return one("message", bulk ? "Records Confirm Successfully" : "Record " + action + " Successfully");
        });
    }

    // ============================================================================ bills (38, 39)

    @GetMapping(API + "/bills/{docType}/lookups")
    @ResponseBody
    public ResponseEntity<?> billsLookups(@PathVariable("docType") int docType) { return run(() -> bills.lookups(docType)); }

    @GetMapping(API + "/bills/{docType}/next-code")
    @ResponseBody
    public ResponseEntity<?> billsNextCode(@PathVariable("docType") int docType) { return run(() -> one("voucherCode", bills.nextCode(docType))); }

    @GetMapping(API + "/bills/{docType}/history-accounts")
    @ResponseBody
    public ResponseEntity<?> billsHistoryAccounts(@PathVariable("docType") int docType) { return run(() -> bills.historyAccounts(docType)); }

    @GetMapping(API + "/bills/custom-accounts")
    @ResponseBody
    public ResponseEntity<?> billsCustomAccounts(@RequestParam("customGroupId") int groupId) {
        return run(() -> bills.customAccounts(groupId));
    }

    @GetMapping(API + "/bills/{docType}/last-rate")
    @ResponseBody
    public ResponseEntity<?> billsLastRate(@PathVariable("docType") int docType, @RequestParam("currencyId") int currencyId) {
        return run(() -> bills.lastExchangeRate(docType, currencyId));
    }

    @GetMapping(API + "/bills/tax-schedule")
    @ResponseBody
    public ResponseEntity<?> billsTaxSchedule(@RequestParam("taxTypeId") int taxTypeId, @RequestParam("date") String date) {
        return run(() -> bills.taxSchedule(taxTypeId, date));
    }

    @PostMapping(API + "/bills/{docType}/save")
    @ResponseBody
    public ResponseEntity<?> billsSave(@PathVariable("docType") int docType, @RequestBody Map<String, Object> body) {
        return run(() -> bills.save(docType, body));
    }

    @GetMapping(API + "/bills/{docType}/load/{id}")
    @ResponseBody
    public ResponseEntity<?> billsLoad(@PathVariable("docType") int docType, @PathVariable("id") int id) {
        return runOr404(() -> bills.load(docType, id));
    }

    @GetMapping(API + "/bills/{docType}/history")
    @ResponseBody
    public ResponseEntity<?> billsHistory(@PathVariable("docType") int docType, @RequestParam Map<String, String> q) {
        return run(() -> bills.history(docType, q));
    }

    @GetMapping(API + "/bills/{docType}/history-detail/{id}")
    @ResponseBody
    public ResponseEntity<?> billsHistoryDetail(@PathVariable("docType") int docType, @PathVariable("id") int id) {
        return run(() -> bills.historyDetail(docType, id));
    }

    // ================================================================================ plumbing

    private static Map<String, Object> one(String k, Object v) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put(k, v); return m;
    }

    private ResponseEntity<?> runOr404(Supplier<Object> body) {
        ResponseEntity<?> r = run(body);
        if (r.getStatusCode() == HttpStatus.OK && r.getBody() == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record not found"));
        }
        return r;
    }

    /**
     * IllegalArgumentException = the desktop's own validation message (400); SecurityException =
     * a missing right (403); anything else - typically a RAISERROR out of a procedure such as
     * USP_VoucherBalanceCheck or Sp_VoucherHead_Update - is passed through as its own text (500).
     */
    private ResponseEntity<?> run(Supplier<Object> body) {
        try {
            return ResponseEntity.ok(body.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (SecurityException | org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("Desktop voucher request failed", e);
            Throwable c = e;
            while (c.getCause() != null && c.getCause() != c) c = c.getCause();
            String m = c.getMessage() != null ? c.getMessage() : e.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(m == null ? "Request failed." : m));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
