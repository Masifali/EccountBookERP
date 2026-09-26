package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleOrderHistoryLookupsRepository;
import com.mst.security.CurrentUserContext;
import java.sql.Date;
import java.time.LocalDate;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.web.bind.annotation.*;

/**
 * Sale Order (81) calls that SaleOrder.cs makes and controllers/sale/SaleOrderController does not serve.
 * See SaleOrderHistoryLookupsRepository for the desktop source of each one. Tenancy, user, branch and financial
 * year always come from the signed-in session, never from the request.
 */
@RestController
@RequestMapping("/sale/sale-order/api")
public class SaleOrderHistoryLookupsController {
    private final SaleOrderHistoryLookupsRepository repository;
    private final CurrentUserContext context;

    public SaleOrderHistoryLookupsController(SaleOrderHistoryLookupsRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    /** cmbBranchName on the History tab. */
    @GetMapping("/history-branches")
    public List<Map<String, Object>> historyBranches() {
        return repository.historyBranches(context.requireAccountingUser());
    }

    /** CmbCustomerHistory / CmbBookingPersonHistory for the ticked branches (the user's own branch when none are given). */
    @GetMapping("/history-lookups")
    public Map<String, List<Map<String, Object>>> historyLookups(@RequestParam(name = "branchIds", required = false) String branchIds) {
        UserAccount u = context.requireAccountingUser();
        String ids = branchIds == null || branchIds.isBlank() ? repository.userBranchIds(u) : repository.allowedBranchIds(u, branchIds);
        return repository.historyLookups(u, ids);
    }

    /** gridhistoryfill with the desktop's full filter set and the user's CanView AllRecord right. */
    @GetMapping("/history-search")
    public List<Map<String, Object>> historySearch(@RequestParam(name = "dateMode", required = false) String dateMode,
                                                   @RequestParam(name = "fromDate", required = false) String fromDate,
                                                   @RequestParam(name = "toDate", required = false) String toDate,
                                                   @RequestParam(name = "fromDocNo", required = false) Integer fromDocNo,
                                                   @RequestParam(name = "toDocNo", required = false) Integer toDocNo,
                                                   @RequestParam(name = "customerId", required = false) Integer customerId,
                                                   @RequestParam(name = "bookingPersonId", required = false) Integer bookingPersonId,
                                                   @RequestParam(name = "branchIds", required = false) String branchIds) {
        UserAccount u = context.requireAccountingUser();
        String ids = repository.allowedBranchIds(u, branchIds);
        boolean canViewAll = repository.canViewAllRecords(u.getId(), u.getCompanyId(), context.currentRoleName());
        return repository.history(u, context.currentFinancialYearId(), canViewAll, dateMode,
                date(fromDate), date(toDate), n(fromDocNo), n(toDocNo), n(customerId), n(bookingPersonId), ids);
    }

    /** txtcatsr - next category serial for the chosen Order Category. */
    @GetMapping("/category-sr-no")
    public Map<String, Object> categorySrNo(@RequestParam("categoryId") int categoryId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("catagorySrNo", categoryId > 0
                ? repository.nextCategorySrNo(context.requireAccountingUser(), context.currentFinancialYearId(), categoryId) : 0);
        return out;
    }

    /** chkCommissionOnSale and lblStockWeight for the chosen item. */
    @GetMapping("/item-info/{itemId}")
    public Map<String, Object> itemInfo(@PathVariable("itemId") int itemId,
                                        @RequestParam(name = "docDate", required = false) String docDate) {
        UserAccount u = context.requireAccountingUser();
        Date d = date(docDate);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("commissionOnSale", repository.commissionOnSale(u, itemId));
        out.put("stockWeight", repository.availableStockWeight(u, itemId, d != null ? d : Date.valueOf(LocalDate.now())));
        return out;
    }

    /** CheckOrderExist - the duplicate-order warning the desktop asks the user to confirm before saving. */
    @PostMapping("/order-exists-warning")
    public Map<String, Object> orderExistsWarning(@RequestBody Map<String, Object> body) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("warning", repository.orderExistsWarning(context.requireAccountingUser(),
                n(body.get("id")), date(body.get("docDate") == null ? null : String.valueOf(body.get("docDate"))),
                n(body.get("customerId")), n(body.get("itemId")),
                d(body.get("qty")), d(body.get("rate")), d(body.get("amount"))));
        return out;
    }

    /** cmbCurrency (ERP feature 6 only). */
    @GetMapping("/currencies")
    public List<Map<String, Object>> currencies() {
        return repository.currencies(context.requireAccountingUser());
    }

    /** Configured defaults and feature flags the desktop form reads when it opens. */
    @GetMapping("/config-defaults")
    public Map<String, Object> configDefaults() {
        return repository.configDefaults(context.requireAccountingUser());
    }

    private static Date date(String s) {
        if (s == null || s.isBlank()) return null;
        return Date.valueOf(LocalDate.parse(s.trim().substring(0, 10)));
    }
    private static int n(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o)); } catch (NumberFormatException e) { return 0; }
    }
    private static Double d(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null || String.valueOf(o).isBlank()) return null;
        try { return Double.parseDouble(String.valueOf(o)); } catch (NumberFormatException e) { return null; }
    }
}
