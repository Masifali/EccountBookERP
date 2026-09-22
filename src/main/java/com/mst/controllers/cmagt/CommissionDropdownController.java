package com.mst.controllers.cmagt;

import com.mst.services.cmagt.CommissionDropdownService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Map;

/**
 * /api/commission/dropdowns/* - the lookup surface ten CMAGT screens depend on.
 *
 * Until now nothing mapped this prefix, so every dropdown on all ten was empty at runtime.
 *
 * /buyers, /suppliers and /commission-agents deliberately return the SAME rows: the desktop's
 * CommonBindings.SupplierBind binds one table to all of those combos and only changes the display
 * member (CommonBindings.cs:160-198). They are kept as three paths so each screen keeps calling
 * the URL it already calls, and so the desktop's own naming survives in the code.
 */
@RestController
@RequestMapping("/api/commission/dropdowns")
public class CommissionDropdownController {

    @Autowired
    private CommissionDropdownService service;

    /**
     * @param useBusinessName GDN and GRN Loading Challan bind with useBusinessName: true
     *                        (frmGoodsDispatchingNoteCmagt:564, frmGrnLoadingChallanCmagt:603).
     *                        1050 and Trade Bill drive it from their RadBusinessName radio, so the
     *                        caller can pass false to get NickName instead.
     */
    @GetMapping("/commission-agents")
    public List<Map<String, Object>> commissionAgents(
            @RequestParam(defaultValue = "true") boolean useBusinessName) {
        return service.parties(useBusinessName);
    }

    @GetMapping("/buyers")
    public List<Map<String, Object>> buyers(
            @RequestParam(defaultValue = "true") boolean useBusinessName) {
        return service.parties(useBusinessName);
    }

    @GetMapping("/suppliers")
    public List<Map<String, Object>> suppliers(
            @RequestParam(defaultValue = "true") boolean useBusinessName) {
        return service.parties(useBusinessName);
    }

    /** Deliver To Party and Transporter are the same list again (SupplierBind, :696-697). */
    @GetMapping("/delivery-parties")
    public List<Map<String, Object>> deliveryParties(
            @RequestParam(defaultValue = "true") boolean useBusinessName) {
        return service.parties(useBusinessName);
    }

    @GetMapping("/items")
    public List<Map<String, Object>> items(@RequestParam(defaultValue = "0") int parentCategoryId) {
        return service.items(parentCategoryId);
    }

    @GetMapping("/parent-categories")
    public List<Map<String, Object>> parentCategories() {
        return service.parentCategories();
    }

    @GetMapping("/companies")
    public List<Map<String, Object>> companies() {
        return service.companies();
    }

    /** ?supplierCustomerId= drives the buyer -> ship-to cascade (CommonBindings:296). */
    @GetMapping("/ship-to-addresses")
    public List<Map<String, Object>> shipToAddresses(
            @RequestParam(defaultValue = "0") int supplierCustomerId) {
        return service.shipToAddresses(supplierCustomerId);
    }

    @GetMapping("/payment-terms")
    public List<Map<String, Object>> paymentTerms() { return service.paymentTerms(); }

    @GetMapping("/delivery-terms")
    public List<Map<String, Object>> deliveryTerms() { return service.deliveryTerms(); }

    @GetMapping("/packing-types")
    public List<Map<String, Object>> packingTypes() { return service.packingTypes(); }

    @GetMapping("/crop-years")
    public List<Map<String, Object>> cropYears() { return service.cropYears(); }

    /** ?itemId= - Equivalent is returned as stored and is never defaulted to 1. */
    @GetMapping("/item-uoms")
    public List<Map<String, Object>> itemUoms(@RequestParam(defaultValue = "0") int itemId) {
        return service.itemUoms(itemId);
    }

    // ========================================================== 1052 lookups

    /** CmbBranch. The user id comes from the session, never from the caller. */
    /** vehicleTypefill() - Sp_VehicleType_GetAllMethod @Activity='ReadAll'. */
    @GetMapping("/vehicle-types")
    public List<Map<String, Object>> vehicleTypes() {
        return service.vehicleTypes();
    }

    /** CityBindFromGlobal() - Loading City and Un-Loading City share this one list. */
    @GetMapping("/cities")
    public List<Map<String, Object>> cities() {
        return service.cities();
    }

    @GetMapping("/branches")
    public List<Map<String, Object>> branches() { return service.branches(); }

    /**
     * CmbTaxName. Item-driven and date-driven, not a list of all taxes: empty until an item is
     * chosen, and a blank docDate omits the date parameter rather than defaulting to today.
     */
    @GetMapping("/taxes")
    public List<Map<String, Object>> taxes(
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(required = false) String docDate) {
        return service.taxes(itemId, docDate);
    }

    /**
     * CmbCommissionAc / CmbBrokeryAc. The desktop binds these to the SAME party table as the
     * other combos (frmPurchaseOrderCmagt:904-905), so this returns parties, not accounts.
     */
    @GetMapping("/accounts")
    public List<Map<String, Object>> accounts(
            @RequestParam(defaultValue = "true") boolean useBusinessName) {
        return service.accounts(useBusinessName);
    }

    /**
     * grdEmptyBagsPm EmptyBagItem value list.
     *
     * transactionFlowId defaults to 1, which is what 1052, GRN Loading Challan and Supplier Offer
     * pass; the GDN passes 2. It is a parameter precisely because it is not the same everywhere.
     */
    @GetMapping("/empty-bag-items")
    public List<Map<String, Object>> emptyBagItems(
            @RequestParam(defaultValue = "1") int transactionFlowId,
            @RequestParam(defaultValue = "0") int itemTypeId,
            @RequestParam(defaultValue = "0") int itemCategoryId) {
        return service.emptyBagItems(transactionFlowId, itemTypeId, itemCategoryId);
    }

    /** grdInvExp ItemId value list. */
    @GetMapping("/other-items")
    public List<Map<String, Object>> otherItems() { return service.otherItems(); }

    /**
     * One call feeding four combos - Commission Type, Commission Rate UOM, Payment Base Date and
     * Allocated Packing Type. The rows come back flat with their Activity column, the shape of the
     * desktop's single DataTable, and the screen splits them as BindViewCombos does. The desktop
     * never sends @Activity, so leaving the parameter off returns every activity.
     */
    @GetMapping("/view-combos")
    public List<Map<String, Object>> viewCombos(
            @RequestParam(required = false) String activity) {
        return service.viewCombos(activity);
    }

    /** CmbAnalysisGroup. parentCategoryId 0 omits the filter and returns every group. */
    @GetMapping("/analysis-groups")
    public List<Map<String, Object>> analysisGroups(
            @RequestParam(defaultValue = "0") int parentCategoryId) {
        return service.analysisGroups(parentCategoryId);
    }

    /** grdParameters rows for the chosen analysis group. */
    @GetMapping("/analysis-group-parameters")
    public List<Map<String, Object>> analysisGroupParameters(
            @RequestParam(defaultValue = "0") int analysisGroupId) {
        return service.analysisGroupParameters(analysisGroupId);
    }

    /**
     * GetCommissionAgentConfigurationsFromGlobalandBind — the seven Commission Agent Portal
     * configuration ids a NEW document pre-selects, plus the two static desktop seeds.
     * Values of 0 mean "not configured": the page must leave that control alone rather than
     * substitute anything.
     */
    @GetMapping("/config-defaults")
    public Map<String, Object> configDefaults() {
        return service.commissionPortalDefaults();
    }
}
