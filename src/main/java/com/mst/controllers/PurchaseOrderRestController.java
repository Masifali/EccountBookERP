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
            @RequestParam(defaultValue = "1052") int docType,
            @RequestParam(defaultValue = "1") int companyId) {
        int docNo = purchaseOrderService.generateNextDocNo(docType, companyId);
        String formattedCode = String.format("PO-%d", docNo);
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", docNo);
        res.put("branchNo", docNo);
        res.put("nextCode", formattedCode);
        res.put("displayCode", formattedCode);
        return ResponseEntity.ok(res);
    }

    @GetMapping("/suppliers")
    public ResponseEntity<List<Map<String, Object>>> getSuppliers(
            @RequestParam(required = false) String query,
            @RequestParam(defaultValue = "name") String mode) {
        return ResponseEntity.ok(purchaseOrderService.searchSuppliers(query, mode));
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

    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(purchaseOrderService.getHistory(fromDate, toDate));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> deletePurchaseOrder(@PathVariable Integer id) {
        boolean ok = purchaseOrderService.deletePurchaseOrder(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", ok);
        res.put("message", ok ? "Purchase Order deleted successfully." : "Failed to delete Purchase Order.");
        return ResponseEntity.ok(res);
    }
}
