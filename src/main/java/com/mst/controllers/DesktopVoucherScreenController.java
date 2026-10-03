package com.mst.controllers;

import com.mst.models.dto.DesktopVoucherDtos;
import com.mst.services.ContraVoucherService;
import com.mst.services.ContraVoucherTaxService;
import com.mst.services.DesktopVoucherScreenService;
import com.mst.services.DesktopVoucherScreenService.Screen;
import com.mst.services.ExpenseVoucherPlainService;
import com.mst.services.JournalVoucherEntryService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * The API of the three plain voucher screens 19 (voucher-entry), 22 (contra) and 46 (expense).
 * Read side for all three; save for 19 and 46 (Contra's save stays at /accounts/api/contra/save).
 * Every tenancy value comes from the signed-in user inside the services - no request parameter
 * names an organisation, company, branch, year or user.
 */
@RestController
@RequestMapping("/accounts/api/desktop-voucher")
public class DesktopVoucherScreenController {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopVoucherScreenController.class);

    @Autowired private DesktopVoucherScreenService service;
    @Autowired private JournalVoucherEntryService journal;
    @Autowired private ExpenseVoucherPlainService expense;
    @Autowired private ContraVoucherTaxService contraTax;

    @GetMapping("/{screen}/context")
    public ResponseEntity<?> context(@PathVariable String screen) {
        return run(() -> service.context(Screen.of(screen)));
    }

    @GetMapping("/{screen}/next-code")
    public ResponseEntity<?> nextCode(@PathVariable String screen) {
        return run(() -> {
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("voucherCode", service.nextVoucherCode(Screen.of(screen)));
            return r;
        });
    }

    @GetMapping("/{screen}/projects")
    public ResponseEntity<?> projects(@PathVariable String screen) { return run(() -> { Screen.of(screen); return service.projects(); }); }

    @GetMapping("/{screen}/cost-centers")
    public ResponseEntity<?> costCenters(@PathVariable String screen) { return run(() -> { Screen.of(screen); return service.costCenters(); }); }

    @GetMapping("/{screen}/job-lots")
    public ResponseEntity<?> jobLots(@PathVariable String screen) { return run(() -> { Screen.of(screen); return service.jobLots(); }); }

    @GetMapping("/{screen}/branches")
    public ResponseEntity<?> branches(@PathVariable String screen) { return run(() -> { Screen.of(screen); return service.branches(); }); }

    @GetMapping("/{screen}/currencies")
    public ResponseEntity<?> currencies(@PathVariable String screen) { return run(() -> { Screen.of(screen); return service.currencies(); }); }

    /** The detail/debit account combo of each form. */
    @GetMapping("/{screen}/accounts")
    public ResponseEntity<?> accounts(@PathVariable String screen) {
        return run(() -> {
            switch (Screen.of(screen)) {
                case VOUCHER_ENTRY: return service.voucherEntryAccounts();
                case CONTRA:
                case CONTRA_TAX: return service.contraAccounts();
                default: return service.expenseDetailAccounts();
            }
        });
    }

    /** The header "Credit Account" combo (Contra, Expense). */
    @GetMapping("/{screen}/credit-accounts")
    public ResponseEntity<?> creditAccounts(@PathVariable String screen) {
        return run(() -> {
            switch (Screen.of(screen)) {
                case CONTRA:
                case CONTRA_TAX: return service.contraAccounts();
                case EXPENSE: return service.expenseCreditAccounts();
                default: throw new IllegalArgumentException("This screen has no credit-account combo.");
            }
        });
    }

    /** Screen 855 only: reference accounts, currencies with their last rate, location types. */
    @GetMapping("/{screen}/reference-accounts")
    public ResponseEntity<?> referenceAccounts(@PathVariable String screen) {
        return run(() -> { Screen.of(screen); return service.referenceAccounts(); });
    }

    @GetMapping("/{screen}/currencies-with-rate")
    public ResponseEntity<?> currenciesWithRate(@PathVariable String screen) {
        return run(() -> { Screen.of(screen); return service.currenciesWithRate(); });
    }

    @GetMapping("/{screen}/location-types")
    public ResponseEntity<?> locationTypes(@PathVariable String screen) {
        return run(() -> { Screen.of(screen); return service.locationTypes(); });
    }

    @GetMapping("/{screen}/subsidiary-accounts")
    public ResponseEntity<?> subsidiaryAccounts(@PathVariable String screen,
                                                @RequestParam(defaultValue = "0") int accountId,
                                                @RequestParam(required = false) String typeIds) {
        return run(() -> { Screen.of(screen); return service.subsidiaryAccounts(accountId, typeIds); });
    }

    @GetMapping("/{screen}/balance")
    public ResponseEntity<?> balance(@PathVariable String screen, @RequestParam int accountId,
                                     @RequestParam String date) {
        return run(() -> {
            Screen.of(screen);
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("hasRow", service.accountBalanceHasRow(accountId, date));
            r.put("balance", service.accountBalance(accountId, date));
            return r;
        });
    }

    @GetMapping("/{screen}/subsidiary-balance")
    public ResponseEntity<?> subsidiaryBalance(@PathVariable String screen, @RequestParam int accountId,
                                               @RequestParam int subsidiaryId, @RequestParam String date) {
        return run(() -> {
            Screen.of(screen);
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("balance", service.subsidiaryBalance(accountId, subsidiaryId, date));
            return r;
        });
    }

    @GetMapping("/{screen}/last-exchange-rate")
    public ResponseEntity<?> lastExchangeRate(@PathVariable String screen, @RequestParam int currencyId) {
        return run(() -> service.lastExchangeRate(Screen.of(screen), currencyId));
    }

    @GetMapping("/{screen}/cheques")
    public ResponseEntity<?> cheques(@PathVariable String screen, @RequestParam int bankId,
                                     @RequestParam(defaultValue = "0") int recId) {
        return run(() -> { Screen.of(screen); return service.outstandingCheques(bankId, recId); });
    }

    @GetMapping("/{screen}/history-accounts")
    public ResponseEntity<?> historyAccounts(@PathVariable String screen) {
        return run(() -> service.historyAccounts(Screen.of(screen)));
    }

    @GetMapping("/{screen}/history")
    public ResponseEntity<?> history(@PathVariable String screen,
                                     @RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) Integer fromDocNo,
                                     @RequestParam(required = false) Integer toDocNo,
                                     @RequestParam(required = false) Integer accountId,
                                     @RequestParam(defaultValue = "notapproved") String approvedStatus) {
        return run(() -> service.history(Screen.of(screen), dateType, fromDate, toDate, fromDocNo, toDocNo,
                accountId, approvedStatus));
    }

    @GetMapping("/{screen}/read/{id}")
    public ResponseEntity<?> read(@PathVariable String screen, @PathVariable int id) {
        try {
            Map<String, Object> r = service.read(Screen.of(screen), id);
            if (r == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Voucher not found"));
            return ResponseEntity.ok(r);
        } catch (Exception e) {
            LOG.warn("read {} {} failed", screen, id, e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    @PostMapping("/voucher-entry/save")
    public ResponseEntity<Map<String, Object>> saveJournal(@RequestBody DesktopVoucherDtos.JournalEntry dto) {
        return save(() -> journal.save(dto));
    }

    @PostMapping("/expense/save")
    public ResponseEntity<Map<String, Object>> saveExpense(@RequestBody DesktopVoucherDtos.Expense dto) {
        return save(() -> expense.save(dto));
    }

    @PostMapping("/contra-tax/save")
    public ResponseEntity<Map<String, Object>> saveContraTax(@RequestBody DesktopVoucherDtos.ContraTax dto) {
        return save(() -> contraTax.save(dto));
    }

    // ================================================================================== plumbing

    private ResponseEntity<Map<String, Object>> save(Supplier<Map<String, Object>> action) {
        try {
            return ResponseEntity.ok(action.get());
        } catch (ContraVoucherService.ConfirmationRequiredException e) {
            Map<String, Object> body = fail(e.getMessage());
            body.put("confirm", e.kind);
            if (e instanceof JournalVoucherEntryService.DuplicateConfirmation) {
                body.put("confirmKey", ((JournalVoucherEntryService.DuplicateConfirmation) e).accountId);
            }
            return ResponseEntity.status(HttpStatus.CONFLICT).body(body);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            /* A RAISERROR out of USP_VoucherBalanceCheck or Sp_VoucherHead_Update ("Record cannot be
               Updated because record has approved") is the desktop's own text - passed through. */
            LOG.warn("voucher save failed", e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    private ResponseEntity<?> run(Supplier<?> action) {
        try {
            return ResponseEntity.ok(action.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("desktop-voucher read failed", e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? "Request failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
