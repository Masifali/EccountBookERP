package com.mst.controllers;

import com.mst.services.SaRpt1Service;
import java.io.File;
import java.time.LocalDate;
import java.util.*;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.*;

/**
 * Group R1 data endpoints for the ten Sales Reports pages (the pages are in SaRpt1PageController, the prints in SaRpt1PrintController).
 *   GET  /api/sale/sarpt1/{report}/lookups      the form's Load lists (branches, date config, fixed combos)
 *   GET  /api/sale/sarpt1/{report}/combos       the branch dependent ComboFill lists (485 / 487 / 486 / 488), ?branchesIds=,1,2
 *   GET  /api/sale/sarpt1/{report}/rows         the Show button (query = the form's filters)
 *   GET  /api/sale/sarpt1/dynamic-reports       CommonServices.DynamicReportsLoad("Sales" | "SaleDirectRegister")
 * {report} = gdn-register, sale-invoice-register, sale-invoice-stock-rate-update, orders-with-ledger-balance, sale-invoice-history,
 * sale-invoice-direct-register, sale-price-list-with-discount, sale-invoice-return-register, purchase-and-sale-detail-by-joblot,
 * delivery-order-history.
 */
@RestController
@RequestMapping("/api/sale/sarpt1")
public class SaRpt1Controller {
    private static final Set<String> FOLDERS = new LinkedHashSet<>(Arrays.asList("Sales", "SaleDirectRegister"));

    private final SaRpt1Service service;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    public SaRpt1Controller(SaRpt1Service service) { this.service = service; }

    @GetMapping("/{report}/lookups")
    public Map<String, Object> lookups(@PathVariable("report") String report) { return service.lookups(report); }

    @GetMapping("/{report}/combos")
    public Map<String, Object> combos(@PathVariable("report") String report, @RequestParam(defaultValue = "") String branchesIds) {
        return service.combos(report, branchesIds);
    }

    @GetMapping("/{report}/rows")
    public List<Map<String, Object>> rows(@PathVariable("report") String report, @RequestParam Map<String, String> q) {
        return service.rows(report, q);
    }

    /** 487 btnRefresh_Click. */
    @GetMapping("/sale-invoice-direct-register/refresh")
    public Map<String, Object> directRegisterRefresh() { return service.directRegisterRefresh(); }

    /** 908 btnshow: Table0 = invoices, Table1 = details. */
    @GetMapping("/sale-invoice-stock-rate-update/data")
    public Map<String, Object> cgsData(@RequestParam(defaultValue = "true") boolean sale, @RequestParam(defaultValue = "false") boolean settled,
            @RequestParam String fromDate, @RequestParam String toDate) {
        return service.cgsData(sale, settled, LocalDate.parse(fromDate.substring(0, 10)), LocalDate.parse(toDate.substring(0, 10)));
    }

    /** 908 btnInsertVoucherAndStock_Click. Body: {sale, settled, fromDate, toDate, ids:[invoice ids]}. */
    @PostMapping("/sale-invoice-stock-rate-update/update")
    public Map<String, Object> cgsUpdate(@RequestBody Map<String, Object> body) {
        List<Integer> ids = new ArrayList<>();
        Object raw = body.get("ids");
        if (raw instanceof List) for (Object o : (List<?>) raw) if (o instanceof Number) ids.add(((Number) o).intValue());
        boolean sale = !Boolean.FALSE.equals(body.get("sale"));
        boolean settled = Boolean.TRUE.equals(body.get("settled"));
        LocalDate from = body.get("fromDate") == null ? null : LocalDate.parse(body.get("fromDate").toString().substring(0, 10));
        LocalDate to = body.get("toDate") == null ? null : LocalDate.parse(body.get("toDate").toString().substring(0, 10));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("message", service.stockRateUpdate(sale, settled, from, to, ids));
        return out;
    }

    /** CommonServices.VoucherHeadIdGet (485 "Voucher" button). */
    @GetMapping("/{report}/voucher-head-id")
    public Map<String, Object> voucherHeadId(@PathVariable("report") String report, @RequestParam int id, @RequestParam int documentTypeId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("voucherHeadId", service.voucherHeadId(report, id, documentTypeId));
        return out;
    }

    /** CommonServices.GetGlAccountIdBySupplierCustomerId (840 CustomerName link, 490 uses the row's CustomerGlId). */
    @GetMapping("/{report}/gl-account")
    public Map<String, Object> glAccount(@PathVariable("report") String report, @RequestParam int supplierCustomerId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("glAccountId", service.glAccountOfParty(report, supplierCustomerId));
        return out;
    }

    /**
     * CommonServices.DynamicReportsLoad(FolderName): the .rpt files of <report root>/<FolderName> (Directory.GetFiles(path, "*rpt")),
     * shown by file name without extension as the items of the Print drop-down. Only the two folders these screens use are accepted;
     * a missing folder lists nothing (the desktop creates it and lists nothing).
     */
    @GetMapping("/dynamic-reports")
    public List<String> dynamicReports(@RequestParam("folder") String folder, @RequestParam(value = "report", defaultValue = "sale-invoice-history") String report) {
        service.currentUser(report);
        if (!FOLDERS.contains(folder)) throw new IllegalArgumentException("Unknown report folder " + folder);
        List<String> out = new ArrayList<>();
        if (reportRoot != null && !reportRoot.trim().isEmpty()) {
            File[] files = new File(reportRoot, folder).listFiles();
            if (files != null) {
                Arrays.sort(files, Comparator.comparing(f -> f.getName().toLowerCase(Locale.ROOT)));
                for (File f : files) {
                    String n = f.getName();
                    if (!f.isFile() || !n.toLowerCase(Locale.ROOT).endsWith("rpt")) continue;
                    int dot = n.lastIndexOf('.');
                    out.add(dot > 0 ? n.substring(0, dot) : n);       // Path.GetFileNameWithoutExtension
                }
            }
        }
        return out;
    }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public org.springframework.http.ResponseEntity<String> denied(org.springframework.security.access.AccessDeniedException e) {
        return org.springframework.http.ResponseEntity.status(403).contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(e.getMessage());
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, org.springframework.dao.DataAccessException.class})
    public org.springframework.http.ResponseEntity<String> failed(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return org.springframework.http.ResponseEntity.badRequest().contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(m);
    }
}
