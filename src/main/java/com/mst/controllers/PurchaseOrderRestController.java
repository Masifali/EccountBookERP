package com.mst.controllers;

import com.mst.models.dto.PurchaseOrderFullDto;
import com.mst.services.PurchaseOrderFullService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/purchase-order")
public class PurchaseOrderRestController {

    @Autowired
    private PurchaseOrderFullService purchaseOrderService;

    @GetMapping("/next-doc-no")
    public ResponseEntity<Map<String, Object>> getNextDocNo(
            /* 41 = this module's Purchase Order (po.DocumentTypeId = 41, PurchsaeOrder.cs :3295).
               The default used to be 1052, which is Commission Trading's Purchase Order - a
               different document in a different schema - so any caller that omitted docType
               was numbered against the wrong series. */
            @RequestParam(defaultValue = "41") int docType) {
        /* ------------------------------------------------------------------------------------
           companyId USED TO BE A REQUEST PARAMETER, @RequestParam(defaultValue = "1").

           The page calls this as /api/purchase-order/next-doc-no?docType=41 and sends no
           companyId, so it defaulted to 1 - and the service took it, because its rule was
           "companyId > 0 ? companyId : session". This company is 78, so
           Sp_PurchaseOrder_GetAllMethod ran with @CompanyId = 1, matched no rows, and returned
           MAX(DocNo)+1 = 1. That is the whole of "the desktop shows PO-493, the web shows PO-1".

           The desktop passes clsGlobalVariables.UserAccount.CompanyId and offers no way to
           override it. Taking a tenancy id from the query string is also exactly what the
           standing rule forbids: a caller could number a document against another company's
           series simply by appending ?companyId=. So the parameter is gone, not merely
           defaulted differently.
           ------------------------------------------------------------------------------------ */
        int docNo = purchaseOrderService.generateNextDocNo(docType);
        String formattedCode = String.format("PO-%d", docNo);
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", docNo);
        res.put("nextCode", formattedCode);
        res.put("displayCode", formattedCode);
        /* "branchNo" used to echo docNo. BranchSrNo is a different number from a different
           activity (GeneratePurchaseOrderBranchCodeByDocId, scoped by branch); it has its own
           endpoint now, so this no longer pretends the two are the same. */
        return ResponseEntity.ok(res);
    }

    @GetMapping("/suppliers")
    public ResponseEntity<List<Map<String, Object>>> getSuppliers(
            @RequestParam(required = false) String query,
            @RequestParam(defaultValue = "name") String mode) {
        return ResponseEntity.ok(purchaseOrderService.searchSuppliers(query, mode));
    }

    @GetMapping("/brokers")
    public ResponseEntity<List<Map<String, Object>>> getBrokers(
            @RequestParam(required = false) String query) {
        return ResponseEntity.ok(purchaseOrderService.searchBrokers(query));
    }

    @GetMapping("/commission-agents")
    public ResponseEntity<List<Map<String, Object>>> getCommissionAgents(
            @RequestParam(required = false) String query) {
        return ResponseEntity.ok(purchaseOrderService.searchCommissionAgents(query));
    }

    @GetMapping("/items")
    public ResponseEntity<List<Map<String, Object>>> getItems(
            @RequestParam(required = false) String query,
            @RequestParam(defaultValue = "name") String mode,
            @RequestParam(required = false) Integer parentCategoryId) {
        return ResponseEntity.ok(purchaseOrderService.searchItems(query, mode, parentCategoryId));
    }

    @GetMapping("/parent-categories")
    public ResponseEntity<List<Map<String, Object>>> getParentCategories() {
        return ResponseEntity.ok(purchaseOrderService.getParentCategories());
    }

    @GetMapping("/cities")
    public ResponseEntity<List<Map<String, Object>>> getCities(@RequestParam(required = false) String query) {
        return ResponseEntity.ok(purchaseOrderService.searchCities(query));
    }

    @PostMapping("/save-city")
    public ResponseEntity<Map<String, Object>> saveCity(@RequestBody Map<String, String> body) {
        String cityName = body != null ? body.get("cityName") : null;
        return ResponseEntity.ok(purchaseOrderService.saveCity(cityName));
    }

    /** Real per-item UOM schedule list (Pack UOM + Rate UOM dropdowns on the Purchase Order Detail
     *  tab), ditto desktop's UOMFill()/bindRateUomAndItemPackUom() - see PurchaseOrderFullService
     *  for the verified Sp_UOMSchedule_GetAllMethod-equivalent query. */
    @GetMapping("/uom-schedules")
    public ResponseEntity<List<Map<String, Object>>> getUomSchedules() {
        return ResponseEntity.ok(purchaseOrderService.getUomSchedulesForPurchaseOrder());
    }

    @GetMapping("/job-lots")
    public ResponseEntity<List<Map<String, Object>>> getJobLots() {
        return ResponseEntity.ok(purchaseOrderService.getJobLots());
    }

    @GetMapping("/payment-terms")
    public ResponseEntity<List<Map<String, Object>>> getPaymentTerms() {
        return ResponseEntity.ok(purchaseOrderService.getPaymentTerms());
    }

    @GetMapping("/delivery-terms")
    public ResponseEntity<List<Map<String, Object>>> getDeliveryTerms() {
        return ResponseEntity.ok(purchaseOrderService.getDeliveryTerms());
    }

    /**
     * The History tab's Supplier Name and Booking Person pickers - ONE call, two lists.
     *
     * HistorySupplierComboFill (PurchsaeOrder.cs:4640-4695) fills both from a single
     * USP_GetDataForDropDownFromPurchaseOrder result split on its Activity column, so they are
     * the parties that actually appear on Purchase Orders, not the party master. Keeping it as
     * one endpoint keeps that relationship visible and matches the desktop's single round trip.
     *
     * @param branchesIds CSV of BranchId. The desktop passes the branch the History tab has
     *                    selected and refuses to run without one; omitted here means the BLL
     *                    omits the parameter, which is its own documented behaviour.
     */
    @GetMapping("/history-parties")
    public ResponseEntity<Map<String, Object>> getHistoryParties(
            @RequestParam(required = false) String branchesIds) {
        return ResponseEntity.ok(purchaseOrderService.getHistoryParties(branchesIds));
    }

    @GetMapping("/booking-persons")
    public ResponseEntity<List<Map<String, Object>>> getBookingPersons() {
        return ResponseEntity.ok(purchaseOrderService.getBookingPersons());
    }

    @GetMapping("/lookup-party-types")
    public ResponseEntity<List<Map<String, Object>>> getLookupPartyTypes() {
        return ResponseEntity.ok(purchaseOrderService.getLookupPartyTypes());
    }

    @GetMapping("/lookup-parties")
    public ResponseEntity<List<Map<String, Object>>> getLookupParties() {
        return ResponseEntity.ok(purchaseOrderService.getLookupParties());
    }

    @PostMapping("/save-lookup-party")
    public ResponseEntity<Map<String, Object>> saveLookupParty(@RequestBody Map<String, Object> payload) {
        return ResponseEntity.ok(purchaseOrderService.saveLookupParty(payload));
    }

    @GetMapping("/accounts")
    public ResponseEntity<List<Map<String, Object>>> getAccounts(@RequestParam(required = false) String query) {
        return ResponseEntity.ok(purchaseOrderService.getAccounts(query));
    }

    // ==========================================================================================
    // Packing Material (Empty Bags) - real dropdown / default-row / standalone-persist endpoints.
    // See PurchaseOrderFullService for the verified stored procedures each of these calls.
    // ==========================================================================================

    /** Commission / Brokery Rate UOM, from the same StaticColumnNames procedure the
     *  desktop's CommissionUOMFill() uses (PurchsaeOrder.cs :1166). */
    @GetMapping("/commission-uoms")
    public ResponseEntity<List<Map<String, Object>>> getCommissionUoms() {
        return ResponseEntity.ok(purchaseOrderService.getCommissionUoms());
    }

    @GetMapping("/empty-bags/types")
    public ResponseEntity<List<Map<String, Object>>> getEmptyBagTypes() {
        return ResponseEntity.ok(purchaseOrderService.getEmptyBagTypes());
    }

    @GetMapping("/empty-bags/items")
    public ResponseEntity<List<Map<String, Object>>> getEmptyBagItems() {
        return ResponseEntity.ok(purchaseOrderService.getEmptyBagItems());
    }

    @GetMapping("/empty-bags/packing-types")
    public ResponseEntity<List<Map<String, Object>>> getPackingTypesForEmptyBags() {
        return ResponseEntity.ok(purchaseOrderService.getPackingTypesForEmptyBags());
    }

    @GetMapping("/empty-bags/defaults")
    public ResponseEntity<List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto>> getDefaultEmptyBagRows() {
        return ResponseEntity.ok(purchaseOrderService.getDefaultEmptyBagRows());
    }

    /** Standalone persist for an EXISTING, already-saved Purchase Order Id - independent of the
     *  header Save/Update flow below, so Empty Bags can be tested/updated directly against a real Id. */
    @PutMapping("/{id}/empty-bags")
    public ResponseEntity<Map<String, Object>> saveEmptyBags(
            @PathVariable Integer id,
            @RequestBody List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> rows) {
        return ResponseEntity.ok(purchaseOrderService.saveEmptyBags(id, rows));
    }

    // ==========================================================================================
    // Supplier Expense / Account Credit _Charge to Product / Payment Detail - real dropdown and
    // standalone-persist endpoints, same conventions as the Packing Material (Empty Bags) ones above.
    // ==========================================================================================

    @GetMapping("/supplier-expense/other-items")
    public ResponseEntity<List<Map<String, Object>>> getOtherItemsForSupplierExpense() {
        return ResponseEntity.ok(purchaseOrderService.getOtherItemsForSupplierExpense());
    }

    @PutMapping("/{id}/supplier-expense")
    public ResponseEntity<Map<String, Object>> saveSupplierExpense(
            @PathVariable Integer id,
            @RequestBody List<PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto> rows) {
        return ResponseEntity.ok(purchaseOrderService.saveSupplierExpense(id, rows));
    }

    @GetMapping("/charge-to-product/accounts")
    public ResponseEntity<List<Map<String, Object>>> getAccountsForChargeToProduct() {
        return ResponseEntity.ok(purchaseOrderService.getAccountsForChargeToProduct());
    }

    @PutMapping("/{id}/charge-to-product")
    public ResponseEntity<Map<String, Object>> saveChargeToProduct(
            @PathVariable Integer id,
            @RequestBody List<PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto> rows) {
        return ResponseEntity.ok(purchaseOrderService.saveChargeToProduct(id, rows));
    }

    @PutMapping("/{id}/payment-terms-detail")
    public ResponseEntity<Map<String, Object>> savePaymentTermsDetail(
            @PathVariable Integer id,
            @RequestBody List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows) {
        return ResponseEntity.ok(purchaseOrderService.savePaymentTermsDetail(id, rows));
    }

    /**
     * Header/Detail save - real columns only (fixed: previously targeted a fabricated schema -
     * PurchaseOrder.SupplierCustomerId/IsApproved were coincidentally real, live-verified columns,
     * but PurchaseOrderDetail.ItemQty/GrossWeight/ItemRate/ItemAmount/CommentsDetail were not; the
     * real columns are OrderItemId/OrderItemQty/NetWeight/OrderItemRate/OrderItemRateUOMId/Amount,
     * per GoldenAceDb(0509)t.sql and Sp_PurchaseOrderDetail_Insert). See
     * PurchaseOrderFullService.persistPurchaseOrderDetail() for the per-row Insert/Update/Delete
     * logic that replaced the old blanket delete-then-reinsert (which is what could wipe an existing
     * Purchase Order's real line items on Update). userId is passed as null here so the service falls
     * back to CurrentUserContext.currentUserId() (the authenticated user), matching every other
     * endpoint in this controller rather than hardcoding a user id in the controller layer.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> savePurchaseOrder(@RequestBody PurchaseOrderFullDto dto) {
        return ResponseEntity.ok(purchaseOrderService.savePurchaseOrder(dto, null));
    }

    @PutMapping("/update")
    public ResponseEntity<Map<String, Object>> updatePurchaseOrder(@RequestBody PurchaseOrderFullDto dto) {
        return ResponseEntity.ok(purchaseOrderService.savePurchaseOrder(dto, null));
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getPurchaseOrderById(@PathVariable Integer id) {
        Map<String, Object> po = purchaseOrderService.getPurchaseOrderById(id);
        if (po != null) {
            return ResponseEntity.ok(po);
        }
        return ResponseEntity.notFound().build();
    }

    @GetMapping("/branches")
    public ResponseEntity<List<Map<String, Object>>> getBranches() {
        return ResponseEntity.ok(purchaseOrderService.getBranches());
    }

    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer fromDocNo,
            @RequestParam(required = false) Integer toDocNo,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer bookingPersonId,
            @RequestParam(required = false) Integer branchId,
            @RequestParam(required = false, defaultValue = "DocDate") String dateType) {
        return ResponseEntity.ok(purchaseOrderService.getHistory(fromDate, toDate, fromDocNo, toDocNo, supplierId, bookingPersonId, branchId, dateType));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> deletePurchaseOrder(@PathVariable Integer id) {
        boolean ok = purchaseOrderService.deletePurchaseOrder(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", ok);
        res.put("message", ok ? "Purchase Order deleted successfully." : "Failed to delete Purchase Order.");
        return ResponseEntity.ok(res);
    }

    /**
     * BranchSrNoFill() - the "Branch #" box beside Doc No. It was blank on the web because
     * nothing generated it; the desktop fills it as soon as the form opens.
     */
    @org.springframework.web.bind.annotation.GetMapping("/next-branch-sr-no")
    @org.springframework.web.bind.annotation.ResponseBody
    public java.util.Map<String, Object> nextBranchSrNo(
            @org.springframework.web.bind.annotation.RequestParam(defaultValue = "41") int docType) {
        java.util.Map<String, Object> r = new java.util.LinkedHashMap<>();
        try {
            r.put("branchSrNo", purchaseOrderService.generateNextBranchSrNo(docType));
        } catch (Exception e) {
            r.put("branchSrNo", 0);
            r.put("message", e.getMessage());
        }
        return r;
    }

    /**
     * combordercat_Leave - the "Cat No" box. A DIFFERENT procedure from the two above
     * (Sp_InvOrderCategory_GetAllMethod), and it fires when the Category combo changes, not on New.
     */
    @org.springframework.web.bind.annotation.GetMapping("/next-category-sr-no")
    @org.springframework.web.bind.annotation.ResponseBody
    public java.util.Map<String, Object> nextCategorySrNo(
            @org.springframework.web.bind.annotation.RequestParam int categoryId) {
        java.util.Map<String, Object> r = new java.util.LinkedHashMap<>();
        try {
            r.put("categorySrNo", purchaseOrderService.generateNextCategorySrNo(categoryId));
        } catch (Exception e) {
            r.put("categorySrNo", 0);
            r.put("message", e.getMessage());
        }
        return r;
    }

    /** The configuration-backed defaults a new Purchase Order starts with. */
    @org.springframework.web.bind.annotation.GetMapping("/screen-defaults")
    @org.springframework.web.bind.annotation.ResponseBody
    public java.util.Map<String, Object> screenDefaults() {
        try {
            return purchaseOrderService.screenDefaults();
        } catch (Exception e) {
            java.util.Map<String, Object> r = new java.util.LinkedHashMap<>();
            r.put("message", e.getMessage());
            return r;
        }
    }

    /**
     * Diagnostic: the four values the document-number procedure filters on. Read-only.
     * Use when the generated number does not match the desktop's.
     */
    @org.springframework.web.bind.annotation.GetMapping("/doc-no-context")
    @org.springframework.web.bind.annotation.ResponseBody
    public java.util.Map<String, Object> docNoContext(
            @org.springframework.web.bind.annotation.RequestParam(defaultValue = "41") int docType) {
        try {
            return purchaseOrderService.docNoContext(docType);
        } catch (Exception e) {
            java.util.Map<String, Object> r = new java.util.LinkedHashMap<>();
            r.put("message", e.getMessage());
            return r;
        }
    }
}
