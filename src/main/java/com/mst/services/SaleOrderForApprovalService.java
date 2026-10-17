package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseOrderForApprovalRepository;
import com.mst.repositories.SaleOrderForApprovalRepository;
import com.mst.security.CurrentUserContext;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SaleOrderForApprovalService {
    public record Filter(int documentTypeId, int branchId, LocalDate fromDate, LocalDate toDate,
                         int customerId, int itemId, String approved, String status) { }
    public record Update(Filter filter, List<Integer> ids, String action) { }
    private final SaleOrderForApprovalRepository repo;
    private final PurchaseOrderForApprovalRepository common;
    private final CurrentUserContext ctx;
    private final AccountReportsDesktopService config;

    public SaleOrderForApprovalService(SaleOrderForApprovalRepository repo, PurchaseOrderForApprovalRepository common,
            CurrentUserContext ctx, AccountReportsDesktopService config) {
        this.repo = repo; this.common = common; this.ctx = ctx; this.config = config;
    }

    private UserAccount user(int type) {
        if (!Set.of(81, 90, 127).contains(type)) throw new IllegalArgumentException("Unsupported sales order type");
        var u = ctx.requireAccountingUser();
        if (!repo.canApprove(u, type)) throw new AccessDeniedException("You do not have approval rights for this order type");
        return u;
    }

    private int branch(UserAccount u, int requested) {
        if (!config.configBool("SaleOrderBranchWise")) return 0;
        int id = requested > 0 ? requested : Objects.requireNonNullElse(u.getBranchesId(), 0);
        if (id <= 0 || !repo.branchAllowed(u, id)) throw new AccessDeniedException("The selected branch is not allocated to you");
        return id;
    }

    private static void validate(Filter f) {
        if (f == null || f.toDate() == null) throw new IllegalArgumentException("To Date is required");
        if (f.fromDate() != null && f.fromDate().isAfter(f.toDate())) throw new IllegalArgumentException("From Date must not be after To Date");
        if (!Set.of("Not Approved", "Approved", "All").contains(Objects.toString(f.approved(), "")))
            throw new IllegalArgumentException("Select an approval status");
        if (!Set.of("Open", "Cancel", "Complete").contains(Objects.toString(f.status(), "")))
            throw new IllegalArgumentException("Select an order status");
        if (f.branchId() < 0 || f.customerId() < 0 || f.itemId() < 0) throw new IllegalArgumentException("Invalid filter selection");
    }

    public Map<String,Object> setup(Filter f) {
        validate(f);
        var u = user(f.documentTypeId());
        int branch = branch(u, f.branchId());
        var customers = new ArrayList<Map<String,Object>>();
        var items = new ArrayList<Map<String,Object>>();
        for (var r : repo.combos(u, f.documentTypeId())) {
            var target = "Customer".equals(r.get("Activity")) ? customers : "Item".equals(r.get("Activity")) ? items : null;
            if (target != null) target.add(Map.of("id", val(r, "Id"), "name", val(r, "ReferenceName")));
        }
        LocalDate from = f.fromDate();
        if (from == null) {
            for (var r : common.activeYears(u.getOrganizationId(), u.getCompanyId())) {
                if (integer(r.get("Id")) == ctx.currentFinancialYearId() && r.get("Start_Period") != null) {
                    from = LocalDate.parse(r.get("Start_Period").toString().substring(0,10)); break;
                }
            }
        }
        var out = new LinkedHashMap<String,Object>();
        out.put("title", f.documentTypeId() == 127 ? "Booking Order Approval" : "Sales Order Approval");
        out.put("branchId", branch); out.put("customers", customers); out.put("items", items);
        out.put("fromDate", from == null ? "" : from.toString()); out.put("toDate", f.toDate().toString());
        out.put("rights", rights(u));
        return out;
    }

    public Map<String,Object> history(Filter f) {
        validate(f);
        var u = user(f.documentTypeId());
        var raw = repo.history(u, f, branch(u, f.branchId()));
        var headers = new LinkedHashMap<Integer,Map<String,Object>>();
        var lines = new LinkedHashMap<Integer,List<Map<String,Object>>>();
        for (var r : raw) {
            int id = integer(r.get("Id"));
            var h = headers.computeIfAbsent(id, key -> {
                var row = project(r, "Id,DocNo,DocDate,BranchSrNo,BranchName,SupplierName,CompanyNameSpCAgent,DeliveryTerm,OrderExpiryDate,EntryDate,UserNameEusr,ModifyDate,UserNameMusr,NoOfAttachments");
                row.put("Amount", BigDecimal.ZERO); return row;
            });
            h.put("Amount", ((BigDecimal)h.get("Amount")).add(decimal(r.get("Amount"))));
            lines.computeIfAbsent(id, key -> new ArrayList<>()).add(project(r,
                    "PoDId,ItemName,Crop,PackTypeDesc,UOMCodeItm,OrderItemQty,NetWeight,OrderItemRate,UOMCodeRate,Amount,BagPrice,BagWeight,JobLotCode,CityArea,IsCanceled"));
        }
        return Map.of("rows", new ArrayList<>(headers.values()), "lines", lines, "rights", rights(u));
    }

    @Transactional(rollbackFor = Exception.class)
    public Map<String,Object> update(Update request) {
        if (request == null) throw new IllegalArgumentException("Select an order action");
        Filter f = request.filter(); validate(f);
        var u = user(f.documentTypeId());
        int branch = branch(u, f.branchId());
        String action = request.action();
        if (!Set.of("Approve", "ApprovedAndCancel", "Status", "Cancel").contains(Objects.toString(action,"")))
            throw new IllegalArgumentException("Unknown order action");
        boolean approving = "Approve".equals(action) || "ApprovedAndCancel".equals(action);
        if (!"Open".equals(f.status()) || !(approving ? "Not Approved" : "Approved").equals(f.approved()))
            throw new IllegalArgumentException("This action is not available for the selected filters");
        if (!approving && !Boolean.TRUE.equals(rights(u).get("Status".equals(action) ? "complete" : "cancel")))
            throw new AccessDeniedException("You do not have rights for this order action");
        if (request.ids() == null || request.ids().isEmpty() || request.ids().size() > 500
                || request.ids().stream().anyMatch(id -> id == null || id <= 0)
                || new HashSet<>(request.ids()).size() != request.ids().size())
            throw new IllegalArgumentException("Select valid orders first");
        // Re-read the authorized result: browser-supplied ids and approval flags are never sufficient.
        var raw = repo.history(u, f, branch);
        var visible = new HashSet<Integer>();
        for (var row : raw) visible.add(integer(row.get("Id")));
        if (!visible.containsAll(request.ids())) throw new AccessDeniedException("An order is no longer in this approval list. Press Show and try again");
        var ids = request.ids().stream().sorted().toList();
        for (int id : ids) {
            var locked = repo.lockOrder(u, f.documentTypeId(), id);
            if (locked.size() != 1) throw new AccessDeniedException("Order not found in your company");
            var row = locked.get(0);
            if ((branch > 0 && integer(row.get("BranchesId")) != branch)
                    || !"Open".equals(row.get("OrderStatus")) || bool(row.get("IsApproved")) == approving)
                throw new IllegalArgumentException("The order has changed. Press Show and try again");
            if ("Cancel".equals(action) && raw.stream().filter(r -> integer(r.get("Id")) == id)
                    .anyMatch(r -> decimal(r.get("DispatchQty")).signum() > 0))
                throw new IllegalArgumentException("An order with dispatched quantity cannot be cancelled");
        }
        for (int id : ids) repo.update(u, id, action);
        return Map.of("message", "Updated " + ids.size() + " order(s) successfully", "count", ids.size());
    }

    private Map<String,Boolean> rights(UserAccount u) {
        boolean complete = "Admin".equals(ctx.currentRoleName()), cancel = complete;
        if (!complete) for (var r : common.rightsByUser(u.getId(), "frmSaleOrderForApproval", ctx.currentRoleName(), u.getCompanyId())) {
            if ("CanChangeOrderStatusToComplete".equals(r.get("RightName"))) complete = bool(r.get("Value"));
            if ("CanChangeOrderStatusToCancel".equals(r.get("RightName"))) cancel = bool(r.get("Value"));
        }
        return Map.of("complete", complete, "cancel", cancel);
    }
    private static LinkedHashMap<String,Object> project(Map<String,Object> r, String columns) {
        var out = new LinkedHashMap<String,Object>();
        for (String key : columns.split(",")) out.put(key, val(r,key));
        return out;
    }
    private static Object val(Map<String,Object> r, String key) { return Objects.requireNonNullElse(r.get(key), ""); }
    private static int integer(Object o) { return o instanceof Number n ? n.intValue() : 0; }
    private static boolean bool(Object o) { return Boolean.TRUE.equals(o) || "1".equals(String.valueOf(o)); }
    private static BigDecimal decimal(Object o) { return o == null || "".equals(o) ? BigDecimal.ZERO : new BigDecimal(o.toString()); }
}
