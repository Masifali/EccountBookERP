package com.mst.controllers;

import com.mst.services.PurchaseAnalyticsDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Purchase Analytics Dashboard -
 * Architecture.WinApp.AnalyticDashboard\PurchaseAnalytiicsDashBoard.cs.
 *
 * ONE desktop form, more than one DashBoard entry, because its Form.Tag decides which parent
 * categories it shows. The desktop sets that tag from the rights row's ScreenName
 * (DashboardNew.cs:1572), and the two it recognises are frmPaddyPurchaseAnalytics (parent
 * category 1) and frmRicePurchaseAnalytics (parent categories 2 and 4). Any other tag yields no
 * parent category at all, which is the desktop's own behaviour at :375-384.
 *
 * Routes registered here:
 *
 *   /dashboard/paddy-purchase-analytics        <- ScreenName frmPaddyPurchaseAnalytics
 *   /dashboard/rice-purchase-analytics         <- ScreenName frmRicePurchaseAnalytics
 *   /dashboard/purchase-analytiics-dash-board  <- the TargetUrl type name; the page offers the
 *                                                 Paddy/Rice choice, since no tag came with it
 *
 * ScreenRouteIndex normalises a last path segment to letters and digits and strips a leading
 * "frm" when it matches, so the TargetUrl resolves to its route on its own.
 *
 * NOT registered here: /dashboard/analytics-dashboard. "frmAnalyticsDashboard" is a ScreenName,
 * not a TargetUrl, and both AnalyticDashboard forms carry it as their Load-handler name and
 * window caption - so it does not identify this form. See the note above the class-name route.
 */
@Controller
public class PurchaseAnalyticsDashboardController {

    @Autowired
    private PurchaseAnalyticsDashboardService service;

    @GetMapping("/dashboard/paddy-purchase-analytics")
    public String paddy(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Paddy Purchase Analytics");
        model.addAttribute("screenTag", PurchaseAnalyticsDashboardService.TAG_PADDY);
        model.addAttribute("tagFixed", true);
        return "dashboard/purchase_analytics_dashboard";
    }

    @GetMapping("/dashboard/rice-purchase-analytics")
    public String rice(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Rice Purchase Analytics");
        model.addAttribute("screenTag", PurchaseAnalyticsDashboardService.TAG_RICE);
        model.addAttribute("tagFixed", true);
        return "dashboard/purchase_analytics_dashboard";
    }

    /*
     * "/dashboard/analytics-dashboard" was registered here and served the PURCHASE form. That was
     * wrong. "frmAnalyticsDashboard" is not a TargetUrl at all - it is a ScreenName, the value the
     * desktop puts in Form.Tag (DashboardNew.cs:1571-1572) - and BOTH AnalyticDashboard forms carry
     * it as their Load-handler name and window caption. Confirmed against the shipped
     * ECCOUNTBOOKERP.exe: no type of that name exists, while SalesAnalyticsDashBoard and
     * PurchaseAnalytiicsDashBoard are both present as real type names.
     *
     * A card labelled "Sales Analytics" therefore resolves to SalesAnalyticsDashBoard, which is
     * served by SalesAnalyticsDashboardController at /dashboard/sales-analytics-dashboard. Only the
     * class-name route below stays here, because that one really does name this form.
     */
    @GetMapping("/dashboard/purchase-analytiics-dash-board")
    public String className(Model model) {
        return neutral(model);
    }

    private String neutral(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Purchase Analytics Dashboard");
        /* The desktop decides this by Form.Tag before the form opens; with no tag to go on the
           page offers the choice rather than guessing one. */
        model.addAttribute("screenTag", PurchaseAnalyticsDashboardService.TAG_PADDY);
        model.addAttribute("tagFixed", false);
        return "dashboard/purchase_analytics_dashboard";
    }

    /** frmAnalyticsDashboard_Load, :209-252. */
    @GetMapping("/api/dashboard/purchase-analytics/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup(
            @RequestParam(defaultValue = PurchaseAnalyticsDashboardService.TAG_PADDY) String tag) {
        return ResponseEntity.ok(service.setup(tag));
    }

    /** AllComboBind, :402-466 - the cascade off Parent Category. */
    @GetMapping("/api/dashboard/purchase-analytics/combos")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> combos(
            @RequestParam(required = false) String parentCategoryId) {
        return ResponseEntity.ok(service.combos(parentCategoryId));
    }

    /** rdSeason_Click -> GetSeasonScheduleDates, :320-347. */
    @GetMapping("/api/dashboard/purchase-analytics/season")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> season() {
        return ResponseEntity.ok(service.seasonDates());
    }

    /** btnshow_Click -> GetComparisonData, :496-538. */
    @GetMapping("/api/dashboard/purchase-analytics/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(FilterParams p) {
        return ResponseEntity.ok(service.report(p.toFilters()));
    }

    /** GetBreakupDataAgainstActivityBreakUpName, :540-573. */
    @GetMapping("/api/dashboard/purchase-analytics/breakup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> breakup(
            FilterParams p,
            @RequestParam String reportType,
            @RequestParam(defaultValue = "0") int groupId,
            @RequestParam(required = false) String rowBranchId) {
        return ResponseEntity.ok(
                service.breakup(p.toFilters(), reportType, groupId, rowBranchId));
    }

    /** grdItemWise_ColumnButtonClick "Date Wise", :888-910. */
    @GetMapping("/api/dashboard/purchase-analytics/avg-rate-by-item")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> avgRateByItem(
            FilterParams p,
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(required = false) String rowBranchId) {
        return ResponseEntity.ok(service.avgRateByItem(p.toFilters(), itemId, rowBranchId));
    }

    /** grdItemWise_LinkClicked and grdCustomerWise_LinkClicked, :930-957 and :1065-1098. */
    @GetMapping("/api/dashboard/purchase-analytics/avg-rate-by-supplier")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> avgRateBySupplier(
            FilterParams p,
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(defaultValue = "0") int partyId,
            @RequestParam(required = false) String rowBranchId) {
        return ResponseEntity.ok(
                service.avgRateBySupplier(p.toFilters(), itemId, partyId, rowBranchId));
    }

    /** The filter bar, bound once and reused by every endpoint above. */
    public static class FilterParams {
        public String fromDate;
        public String toDate;
        public String cropYear;
        public String branchIds;
        public int parentCategoryId;
        public int itemCategoryId;
        public int itemTypeId;
        public int jobLotId;
        public int partyId;
        public int itemId;

        public void setFromDate(String v)         { this.fromDate = v; }
        public void setToDate(String v)           { this.toDate = v; }
        public void setCropYear(String v)         { this.cropYear = v; }
        public void setBranchIds(String v)        { this.branchIds = v; }
        public void setParentCategoryId(int v)    { this.parentCategoryId = v; }
        public void setItemCategoryId(int v)      { this.itemCategoryId = v; }
        public void setItemTypeId(int v)          { this.itemTypeId = v; }
        public void setJobLotId(int v)            { this.jobLotId = v; }
        public void setPartyId(int v)             { this.partyId = v; }
        public void setItemId(int v)              { this.itemId = v; }

        public PurchaseAnalyticsDashboardService.Filters toFilters() {
            PurchaseAnalyticsDashboardService.Filters f =
                    new PurchaseAnalyticsDashboardService.Filters();
            f.fromDate = fromDate;
            f.toDate = toDate;
            f.cropYear = cropYear == null ? "" : cropYear;
            f.branchIds = branchIds == null ? "" : branchIds;
            f.parentCategoryId = parentCategoryId;
            f.itemCategoryId = itemCategoryId;
            f.itemTypeId = itemTypeId;
            f.jobLotId = jobLotId;
            f.partyId = partyId;
            f.itemId = itemId;
            return f;
        }
    }
}
