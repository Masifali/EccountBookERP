package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderCmagtDto;
import com.mst.services.cmagt.PurchaseOrderCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/purchase-order")
public class PurchaseOrderCmagtRestController {

    @Autowired
    private com.mst.services.cmagt.PurchaseOrderCmagtSaveService saveService;

    @Autowired
    private PurchaseOrderCmagtService service;

    /* Company and organization are NEVER taken from the request. They used to arrive as
       @RequestParam(defaultValue = "1"), which meant two things at once: a caller could read
       another company's data by appending ?companyId=, and a caller that omitted it silently
       queried company 1 - which in this database does not exist, so the screen showed nothing and
       said nothing. The desktop reads UserAccount.OrganizationId / .CompanyId and offers no
       override; this is that, enforced server-side. */
    @org.springframework.beans.factory.annotation.Autowired
    private com.mst.security.CurrentUserContext currentUserContext;

    /**
     * Now on the complete contract: [cmagt].[USP_purchaseOrderMaster_InsertAndUpdate] with all
     * 42 parameters plus the seven child collections, DocumentTypeId 1052.
     *
     * The old path called the same master procedure but sent 24 parameters and wrote only the
     * detail rows, so eighteen values took the procedure's defaults and six child collections
     * were never written. It is kept, disabled, at /save-legacy-donotuse.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto dto) {
        return ResponseEntity.ok(saveService.saveOrUpdate(dto));
    }

    @PostMapping("/save-legacy-donotuse")
    public ResponseEntity<Map<String, Object>> saveLegacy(@RequestBody PurchaseOrderCmagtDto dto) {
        throw new UnsupportedOperationException(
                "Disabled: sent 24 of the procedure's 42 parameters and wrote only the detail "
              + "rows. Use POST /api/commission/purchase-order/save.");
    }

    /** FormHistory — filtered by CanViewAllRecord, as the desktop does. */
    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String dateType,
            @RequestParam(required = false) String validityFrom,
            @RequestParam(required = false) String validityTo,
            @RequestParam(required = false) Integer commissionAgentId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer itemId,
            @RequestParam(required = false) String parentItemIds,
            @RequestParam(required = false) Integer deliveryToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        /* HistoryFill's optional filters (frmPurchaseOrderCmagt.cs:3931-3993). All optional, so
           the existing ?fromDate=&toDate= call is unchanged. */
        Map<String, Object> extra = new java.util.LinkedHashMap<>();
        extra.put("dateType", dateType);
        extra.put("ValidityDateFrom", validityFrom);
        extra.put("ValidityDateTo", validityTo);
        extra.put("CommissionAgentId", commissionAgentId);
        extra.put("SupplierId", supplierId);
        extra.put("ItemId", itemId);
        extra.put("ParentItemIds", parentItemIds);
        extra.put("DeliveryToPartyId", deliveryToPartyId);
        extra.put("ShipToAddress", shipToAddress);
        return ResponseEntity.ok(saveService.formHistory(fromDate, toDate, extra));
    }

    /**
     * The procedures refuse with RAISERROR - approved record, referred in GRN Loading, document
     * date outside the active financial year, order weight below the weight already used. The
     * desktop shows ex.Message. Without this the refusal surfaced as a bare 500 and the page
     * could only say "Internal Server Error". Scoped to this controller only.
     */
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<Map<String, Object>> procedureRefused(org.springframework.dao.DataAccessException e) {
        Throwable t = e.getMostSpecificCause() != null ? e.getMostSpecificCause() : e;
        String msg = t.getMessage() == null ? "The database refused the operation." : t.getMessage().trim();
        Map<String, Object> body = new java.util.LinkedHashMap<>();
        body.put("success", false);
        body.put("status", "ERROR");
        body.put("message", msg);
        body.put("error", msg);
        return ResponseEntity.badRequest().body(body);
    }

    /** ReadById plus all seven child collections, same family Save writes to. */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(saveService.getById(id));
    }

    /**
     * DocumentNoFill -> BLL GenerateCode, @DocumentTypeId fixed at this form's own 1052.
     *
     * The page has called this since it was written; the endpoint did not exist, so every new
     * document opened with an empty Doc No. The number the procedure allocates at save time was
     * always the real one, so nothing was mis-numbered - the box was simply blank.
     */
    @GetMapping("/generate-no")
    public ResponseEntity<Map<String, Object>> generateNextNo() {
        Map<String, Object> r = new java.util.LinkedHashMap<>();
        r.put("docNo", saveService.generateNextDocNo());
        return ResponseEntity.ok(r);
    }

    /**
     * BLL DeleteByID - @EntryUserId, @Id, @Activity='DeleteById'. Not a JPA delete.
     *
     * The Delete button on this screen called DELETE /{id}, which was not mapped: every click
     * returned 405 and the page reported "Delete was refused." The desktop's own delete is a
     * procedure call that cascades to the child tables, so this maps to that and nothing else.
     */
    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> deleteRecord(@PathVariable Integer id) {
        Map<String, Object> r = saveService.deleteById(id);
        r.put("success", !"ERROR".equals(String.valueOf(r.get("status"))));
        return ResponseEntity.ok(r);
    }

    /* ===================================================================== Load So picker */

    @Autowired
    private com.mst.repositories.cmagt.PurchaseOrderCmagtRepository poRepository;

    /** frmLoadSaleIrderForPO.ComboDbCall - rows split on Activity by the page (CombosFill). */
    @GetMapping("/load-so/combos")
    public ResponseEntity<List<Map<String, Object>>> loadSoCombos() {
        return ResponseEntity.ok(poRepository.loadSoCombos(
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId()));
    }

    /** frmLoadSaleIrderForPO.PendingDataDbCall. Tenancy, branch and year from the session. */
    @GetMapping("/load-so/pending")
    public ResponseEntity<List<Map<String, Object>>> loadSoPending(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer fromDocNo,
            @RequestParam(required = false) Integer toDocNo,
            @RequestParam(required = false) Integer commissionAgentId,
            @RequestParam(required = false) Integer buyerId,
            @RequestParam(required = false) Integer itemId,
            @RequestParam(required = false) Integer deliveryToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        return ResponseEntity.ok(poRepository.loadSoPending(
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(), currentUserContext.currentFinancialYearId(),
                fromDate, toDate, fromDocNo, toDocNo, commissionAgentId, buyerId, itemId,
                deliveryToPartyId, shipToAddress));
    }

    @Autowired
    private com.mst.repositories.cmagt.CmagtReportRepository cmagtReportRepository;

    /** frmLoadSaleIrderForPO.btnReset_Click (:417-422): FromDate = clsGlobalVariables.ActiveYr.Start_Period
        of the signed-in session's financial year (same lookup the reports use). */
    @GetMapping("/financial-year-start")
    public ResponseEntity<Map<String, Object>> financialYearStart() {
        Object start = cmagtReportRepository.financialYearStart(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), currentUserContext.currentFinancialYearId());
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        String s = start == null ? null : String.valueOf(start);
        out.put("financialYearStart", s == null ? null : s.substring(0, Math.min(10, s.length())));
        return ResponseEntity.ok(out);
    }

    /** HistoryComboDbCall - the history filter combos (HistoryComboBind splits on Activity). */
    @GetMapping("/history-combos")
    public ResponseEntity<List<Map<String, Object>>> historyCombos() {
        return ResponseEntity.ok(poRepository.historyCombos(
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId()));
    }

    /* ============================================= Ship-to "+" (SupfrmShipToAddress) */

    @GetMapping("/ship-to/countries")
    public ResponseEntity<List<Map<String, Object>>> shipToCountries() {
        return ResponseEntity.ok(poRepository.countries());
    }

    @GetMapping("/ship-to/cities")
    public ResponseEntity<List<Map<String, Object>>> shipToCities() {
        return ResponseEntity.ok(poRepository.cities(
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId()));
    }

    @GetMapping("/ship-to/history")
    public ResponseEntity<List<Map<String, Object>>> shipToHistory() {
        return ResponseEntity.ok(poRepository.shipToHistory(
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId()));
    }

    @GetMapping("/ship-to/{id}")
    public ResponseEntity<Map<String, Object>> shipToById(@PathVariable Integer id) {
        List<Map<String, Object>> rows = poRepository.shipToById(id);
        return ResponseEntity.ok(rows.isEmpty() ? new java.util.LinkedHashMap<>() : rows.get(0));
    }

    /** Insert(): FormValidation messages, then BLL Save. Org/company/user from the session. */
    @PostMapping("/ship-to/save")
    public ResponseEntity<Map<String, Object>> shipToSave(@RequestBody Map<String, Object> body) {
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        String err = null;
        if (isBlank(body.get("SupplierCustomerId")) || "0".equals(String.valueOf(body.get("SupplierCustomerId")))) err = "Please Select Supplier";
        else if (isBlank(body.get("AddressLine1"))) err = "Please Enter Address";
        else if (isBlank(body.get("AddressTitle"))) err = "Please Enter Address Title";
        else if (isBlank(body.get("CountryId")) || "0".equals(String.valueOf(body.get("CountryId")))) err = "Please Select Country";
        else if (isBlank(body.get("CityId")) || "0".equals(String.valueOf(body.get("CityId")))) err = "Please Select City";
        if (err != null) {
            out.put("success", false); out.put("message", err);
            return ResponseEntity.ok(out);
        }
        Map<String, Object> m = new java.util.LinkedHashMap<>(body);
        for (String k : new String[] { "AddressLine1", "AddressTitle", "PhoneNo", "MobileNo", "WhatsAppNo", "ContactPerson" }) {
            m.put(k, body.get(k) == null ? "" : String.valueOf(body.get(k)).trim());
        }
        m.put("OrganizationId", currentUserContext.currentOrganizationId());
        m.put("CompanyId", currentUserContext.currentCompanyId());
        m.put("EntryUser", currentUserContext.currentUserId());
        m.put("ModifyUser", currentUserContext.currentUserId());
        boolean isNew = isBlank(body.get("Id")) || "0".equals(String.valueOf(body.get("Id")));
        Object id = poRepository.shipToSave(m);
        out.put("success", true);
        out.put("id", id);
        out.put("message", isNew ? "Save Successfully" : "Update Successfully");
        return ResponseEntity.ok(out);
    }

    private static boolean isBlank(Object v) { return v == null || String.valueOf(v).trim().isEmpty(); }
}
