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

    /**
     * btnsave_Click / btnUpdate_Click -> Insert() -> BLL Save -> DAL SetData (16 procedures, one
     * transaction, voucher included). header.RecId > 0 updates. Every refusal - the form's own,
     * MakeVoucher's, or a procedure's RAISERROR (financial year, approved record, voucher
     * balance) - comes back as {status: ERROR, message} with the desktop's text, and nothing is
     * committed.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody TradeBillAgainstGdnCmagtDto dto,
                                                    @RequestParam(defaultValue = "1056") int documentTypeId) {
        try {
            return ResponseEntity.ok(service.save(dto,documentTypeId));
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable root = e.getMostSpecificCause();
            return ResponseEntity.ok(err(root != null ? root.getMessage() : e.getMessage()));
        } catch (RuntimeException e) {
            return ResponseEntity.ok(err(e.getMessage()));
        }
    }

    private static Map<String, Object> err(String m) {
        Map<String, Object> r = new java.util.LinkedHashMap<>();
        r.put("status", "ERROR");
        r.put("message", m);
        return r;
    }

    // ------------------------------------------------------------------ GDN loader (frmPendingGdnLoader)

    /** ComboDbCall: flat {Activity, Id, ReferenceName} rows, split by the page as CombosFill does. */
    @GetMapping("/loader/dropdowns")
    public ResponseEntity<List<Map<String, Object>>> loaderDropdowns() {
        return ResponseEntity.ok(service.loaderDropdowns());
    }

    /** PendingDataDbCall: all nine result sets of the loader procedure, in order. */
    @GetMapping("/loader/pending")
    public ResponseEntity<Object> loaderPending(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "0") int fromDocNo,
            @RequestParam(defaultValue = "0") int toDocNo,
            @RequestParam(defaultValue = "0") int commissionAgentId,
            @RequestParam(defaultValue = "0") int supplierId,
            @RequestParam(defaultValue = "0") int buyerId,
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(defaultValue = "0") int deliverToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        try {
            return ResponseEntity.ok(service.pendingGdn(fromDate, toDate, fromDocNo, toDocNo, commissionAgentId,
                    supplierId, buyerId, itemId, deliverToPartyId, shipToAddress));
        } catch (RuntimeException e) {
            return ResponseEntity.ok(err(e.getMessage()));
        }
    }

    // --------------------------------------------------------------------------- lookups

    /** Every account combo on the form, filtered as DatatableHelper.GetAccountsFromGlobalByTypeIds. */
    @GetMapping("/lookups/accounts")
    public ResponseEntity<Map<String, List<Map<String, Object>>>> accounts() {
        return ResponseEntity.ok(service.accountLists());
    }

    @GetMapping("/lookups/tax-types")
    public ResponseEntity<List<Map<String, Object>>> taxTypes() {
        return ResponseEntity.ok(service.taxTypes());
    }

    @GetMapping("/lookups/wht-percent")
    public ResponseEntity<Map<String, Object>> whtPercent(@RequestParam(defaultValue = "0") int taxNameId,
                                                          @RequestParam(required = false) String docDate) {
        return ResponseEntity.ok(service.whtPercent(taxNameId, docDate));
    }

    @GetMapping("/lookups/sale-tax-types")
    public ResponseEntity<List<Map<String, Object>>> saleTaxTypes(@RequestParam(defaultValue = "0") int customerId,
                                                                  @RequestParam(required = false) String docDate) {
        return ResponseEntity.ok(service.saleTaxTypes(customerId, docDate));
    }

    @GetMapping("/lookups/configs")
    public ResponseEntity<Map<String, Object>> configs() {
        return ResponseEntity.ok(service.configs());
    }

    /** HistoryComboBind: Trading Account / Supplier / Customer from the saved bills. */
    @GetMapping("/lookups/history-combos")
    public ResponseEntity<List<Map<String, Object>>> historyCombos() {
        return ResponseEntity.ok(service.historyCombos());
    }

    /** SetRightsValueInRightsObject for this screen - the page enables Save/Update/Delete from it. */
    @GetMapping("/rights")
    public ResponseEntity<Map<String, Boolean>> rights(@RequestParam(defaultValue = "1056") int documentTypeId) {
        return ResponseEntity.ok(service.formRights(documentTypeId));
    }

    @GetMapping("/lookups/year-start")
    public ResponseEntity<Map<String, Object>> yearStart() {
        return ResponseEntity.ok(service.yearStart());
    }

    /** HistoryGridFill -> BLL FormHistory. Tenancy, year, branch and rights from the session. */
    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(defaultValue = "doc") String dateKind,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer docNoFrom,
            @RequestParam(required = false) Integer docNoTo,
            @RequestParam(required = false) Integer tradingAccountId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer customerId,
            @RequestParam(defaultValue = "1056") int documentTypeId) {
        return ResponseEntity.ok(service.getHistory(dateKind, fromDate, toDate, docNoFrom, docNoTo,
                tradingAccountId, supplierId, customerId,documentTypeId));
    }

    /** DocumentNoDbCall / BranchSrNoDbCall - the next Doc No and Branch Sr No. */
    @GetMapping("/generate-no")
    public ResponseEntity<Map<String, Object>> generateNo(@RequestParam(defaultValue = "1056") int documentTypeId) {
        return ResponseEntity.ok(service.generateCodes(documentTypeId));
    }

    /** BtnDelete_Click -> BLL DeleteByID(UserAccount.ID, RecId). */
    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> delete(@PathVariable Integer id,
                                                       @RequestParam(defaultValue = "1056") int documentTypeId) {
        try {
            return ResponseEntity.ok(service.deleteById(id,documentTypeId));
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
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id,
                                                        @RequestParam(defaultValue = "1056") int documentTypeId) {
        return ResponseEntity.ok(service.getById(id,documentTypeId));
    }
}
