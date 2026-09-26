package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import com.mst.services.cmagt.TradeBillAgainstGdnCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/trade-bill-against-gdn")
public class TradeBillAgainstGdnCmagtRestController {

    @Autowired
    private TradeBillAgainstGdnCmagtService service;

    /* Company and organization are NEVER taken from the request. They used to arrive as
       @RequestParam(defaultValue = "1"), which meant two things at once: a caller could read
       another company's data by appending ?companyId=, and a caller that omitted it silently
       queried company 1 - which in this database does not exist, so the screen showed nothing and
       said nothing. The desktop reads UserAccount.OrganizationId / .CompanyId and offers no
       override; this is that, enforced server-side. */
    @org.springframework.beans.factory.annotation.Autowired
    private com.mst.security.CurrentUserContext currentUserContext;

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody TradeBillAgainstGdnCmagtDto dto) {
        /* The repository refuses (no voucher engine yet). Surface that refusal as the page's
           own {status, message} shape instead of a bare 500 the page reports as "Server error". */
        try {
            return ResponseEntity.ok(service.saveOrUpdate(dto));
        } catch (UnsupportedOperationException e) {
            Map<String, Object> r = new java.util.LinkedHashMap<>();
            r.put("status", "ERROR");
            r.put("message", e.getMessage());
            return ResponseEntity.ok(r);
        }
    }

    /** HistoryGridFill -> BLL FormHistory. Tenancy, year, branch and rights from the session. */
    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer docNoFrom,
            @RequestParam(required = false) Integer docNoTo,
            @RequestParam(required = false) Integer tradingAccountId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer customerId) {
        return ResponseEntity.ok(service.getHistory(fromDate, toDate, docNoFrom, docNoTo,
                tradingAccountId, supplierId, customerId));
    }

    /** DocumentNoDbCall / BranchSrNoDbCall - the next Doc No and Branch Sr No. */
    @GetMapping("/generate-no")
    public ResponseEntity<Map<String, Object>> generateNo() {
        return ResponseEntity.ok(service.generateCodes());
    }

    /** BtnDelete_Click -> BLL DeleteByID(UserAccount.ID, RecId). */
    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> delete(@PathVariable Integer id) {
        try {
            return ResponseEntity.ok(service.deleteById(id));
        } catch (org.springframework.dao.DataAccessException e) {
            /* e.g. the procedure's RAISERROR 'Record cannot be deleted because record has
               approved' - shown to the operator as the desktop's MessageBox shows ex.Message. */
            Throwable root = e.getMostSpecificCause();
            Map<String, Object> r = new java.util.LinkedHashMap<>();
            r.put("status", "ERROR");
            r.put("message", root != null ? root.getMessage() : e.getMessage());
            return ResponseEntity.ok(r);
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(service.getById(id));
    }
}
