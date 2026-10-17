package com.mst.controllers;

import com.mst.services.W3CProductionFohService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.sql.SQLException;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * 906 "Production FOH" - Architecture.WinApp.Production.Regular/frmProductionFohReportCreate.cs
 * (DocumentTypeId 922, ScreenName "frmProductionFohReportCreate"). Page /production/production-foh,
 * JSON under /api/production/production-foh. No parameter carries tenancy, branch, financial year or user.
 */
@Controller
public class W3CProductionFohController {

    private static final String API = "/api/production/production-foh";

    @Autowired private W3CProductionFohService service;

    @GetMapping("/production/production-foh")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/w3c_production_foh";
    }

    @GetMapping(API + "/init")
    @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (Exception e) { return error(e); }
    }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() {
        try { return ResponseEntity.ok(service.refresh()); } catch (Exception e) { return error(e); }
    }

    /** BindGrid:339. */
    @GetMapping(API + "/allocation")
    @ResponseBody
    public ResponseEntity<?> allocation(@RequestParam String monthDate) {
        try { return ResponseEntity.ok(service.allocation(monthDate)); } catch (Exception e) { return error(e); }
    }

    /** btnPrint_Click:480 - rows behind the 666_01 print. */
    @GetMapping(API + "/allocation-print-rows")
    @ResponseBody
    public ResponseEntity<?> allocationPrintRows(@RequestParam String monthDate) {
        try { return ResponseEntity.ok(service.allocationPrintRows(monthDate)); } catch (Exception e) { return error(e); }
    }

    /** AccountsBindByMonthDateDbCall:516. */
    @GetMapping(API + "/accounts")
    @ResponseBody
    public ResponseEntity<?> accounts(@RequestParam String month) {
        try { return ResponseEntity.ok(service.accounts(month)); } catch (Exception e) { return error(e); }
    }

    /** FillForFohExpenseData:663. */
    @GetMapping(API + "/expense-hours")
    @ResponseBody
    public ResponseEntity<?> expenseHours(@RequestParam String month) {
        try { return ResponseEntity.ok(service.expenseHours(month)); } catch (Exception e) { return error(e); }
    }

    /** ReadByIdFohExpense:1044 (withAccounts) / GetDetailGrdByHeadId:1367 (without). */
    @GetMapping(API + "/read")
    @ResponseBody
    public ResponseEntity<?> read(@RequestParam(defaultValue = "0") int id,
                                  @RequestParam(defaultValue = "true") boolean withAccounts) {
        try { return ResponseEntity.ok(service.read(id, withAccounts)); } catch (Exception e) { return error(e); }
    }

    /** HistoryFill:1188. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "entry") String basis,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String month) {
        try { return ResponseEntity.ok(service.history(basis, fromDate, toDate, month)); } catch (Exception e) { return error(e); }
    }

    /** ProdcutionFohAllocateToJobOrderPrint922 - rows behind the 922 print. */
    @GetMapping(API + "/print-rows")
    @ResponseBody
    public ResponseEntity<?> printRows(@RequestParam(defaultValue = "0") int id) {
        try { return ResponseEntity.ok(service.printRows922(id)); } catch (Exception e) { return error(e); }
    }

    /** InsertFohExpense:860 - {result:true, id, updated} | 400 {message}. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.save(body)); } catch (Exception e) { return error(e); }
    }

    /** BtnDeleteFohExpense_Click:1011. */
    @PostMapping(API + "/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@RequestParam(defaultValue = "0") int id) {
        try { return ResponseEntity.ok(service.delete(id)); } catch (Exception e) { return error(e); }
    }

    private static ResponseEntity<?> error(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        HttpStatus status = HttpStatus.BAD_REQUEST;
        if (e instanceof DataAccessException) {
            Throwable t = e;
            while (t.getCause() != null && !(t instanceof SQLException)) t = t.getCause();
            msg = t.getMessage();
        } else if (!(e instanceof IllegalArgumentException) && !(e instanceof IllegalStateException)) {
            status = HttpStatus.INTERNAL_SERVER_ERROR;
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return ResponseEntity.status(status).body(m);
    }
}
