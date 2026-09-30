package com.mst.controllers.pbi;

import com.mst.controllers.hrm.HrmApi;
import com.mst.models.pbi.PbiVoucherSaveRequest;
import com.mst.services.pbi.PbiVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Map;

/**
 * API of screen 41 "Payment By Invoice Voucher New" (page /accounts/vouchers/payment-by-invoice, rendered by
 * AccountsModuleViewController). Every call checks View on ScreenDefinition 41; Save / Update / Print where the
 * desktop form checks them. The print itself is POST /reports/print/132-payment-by-invoice-slip-new-report.
 */
@RestController
@RequestMapping("/api/accounts/payment-by-invoice")
public class PbiVoucherController {

    @Autowired private PbiVoucherService service;

    @GetMapping("/setup")
    public ResponseEntity<?> setup() { return HrmApi.run(() -> service.setup()); }

    @GetMapping("/refresh")
    public ResponseEntity<?> refresh(@RequestParam(value = "documentTypeId", defaultValue = "1") int documentTypeId) {
        return HrmApi.run(() -> service.refresh(documentTypeId));
    }

    @GetMapping("/voucher-type")
    public ResponseEntity<?> voucherType(@RequestParam("documentTypeId") int documentTypeId,
                                         @RequestParam(value = "newVoucher", defaultValue = "true") boolean newVoucher) {
        return HrmApi.run(() -> service.voucherType(documentTypeId, newVoucher));
    }

    @GetMapping("/credit-account")
    public ResponseEntity<?> creditAccount(@RequestParam("accountId") int accountId,
                                           @RequestParam(value = "voucherDate", required = false) String voucherDate,
                                           @RequestParam(value = "documentTypeId", defaultValue = "1") int documentTypeId) {
        return HrmApi.run(() -> service.creditAccount(accountId, voucherDate, documentTypeId));
    }

    @GetMapping("/detail-account")
    public ResponseEntity<?> detailAccount(@RequestParam("accountId") int accountId,
                                           @RequestParam(value = "voucherDate", required = false) String voucherDate,
                                           @RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return HrmApi.run(() -> service.detailAccount(accountId, voucherDate, invoiceId));
    }

    @GetMapping("/invoice-balance")
    public ResponseEntity<?> invoiceBalance(@RequestParam("accountId") int accountId, @RequestParam("invoiceId") int invoiceId) {
        return HrmApi.run(() -> service.invoiceBalance(accountId, invoiceId));
    }

    @GetMapping("/tax-schedule")
    public ResponseEntity<?> taxSchedule(@RequestParam(value = "taxTypeId", defaultValue = "0") int taxTypeId,
                                         @RequestParam(value = "voucherDate", required = false) String voucherDate) {
        return HrmApi.run(() -> service.taxSchedule(taxTypeId, voucherDate));
    }

    @GetMapping("/loader/setup")
    public ResponseEntity<?> loaderSetup() { return HrmApi.run(() -> service.loaderSetup()); }

    @GetMapping("/loader/search")
    public ResponseEntity<?> loaderSearch(@RequestParam(value = "accountId", defaultValue = "0") int accountId,
                                          @RequestParam(value = "fromDate", required = false) String fromDate,
                                          @RequestParam(value = "toDate", required = false) String toDate) {
        return HrmApi.run(() -> service.loaderSearch(accountId, fromDate, toDate));
    }

    @PostMapping("/loader/load")
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> loaderLoad(@RequestBody Map<String, Object> body) {
        return HrmApi.run(() -> {
            Object ids = body == null ? null : body.get("ids");
            List<Integer> list = new java.util.ArrayList<>();
            if (ids instanceof List) for (Object o : (List<Object>) ids) list.add(com.mst.services.hrm.HrmSupport.toInt(o));
            int acId = body == null ? 0 : com.mst.services.hrm.HrmSupport.toInt(body.get("accountId"));
            return service.loadOnVoucher(list, acId);
        });
    }

    @GetMapping("/history")
    public ResponseEntity<?> history(@RequestParam(value = "noOfRecords", defaultValue = "0") int noOfRecords) {
        return HrmApi.run(() -> service.history(noOfRecords));
    }

    @GetMapping("/by-id")
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    @GetMapping("/attachments")
    public ResponseEntity<?> attachments(@RequestParam("id") int id) { return HrmApi.run(() -> service.attachments(id)); }

    @GetMapping("/print-check")
    public ResponseEntity<?> printCheck(@RequestParam("id") int id) { return HrmApi.run(() -> service.printCheck(id)); }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody PbiVoucherSaveRequest body) { return HrmApi.run(() -> service.save(body)); }
}
