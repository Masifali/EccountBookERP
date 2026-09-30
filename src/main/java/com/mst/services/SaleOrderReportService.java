package com.mst.services;

import com.mst.models.*;
import com.mst.repositories.SaleOrderReportRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class SaleOrderReportService {
    private final SaleOrderReportRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public SaleOrderReportService(SaleOrderReportRepository repo, CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.context = context; this.rights = rights;
    }
    private UserAccount user() {
        var u = context.requireAccountingUser(); rights.require(u, repo.screenId(), "View"); return u;
    }
    public Map<String, Object> initial() {
        var u = user(); var branches = repo.branches(u); var costs = repo.costCenters(u);
        boolean currentAllowed = branches.stream().anyMatch(r -> Objects.equals(((Number) r.get("BranchId")).intValue(), u.getBranchesId()));
        return Map.of("branches", branches, "branchId", currentAllowed ? u.getBranchesId() : 0,
                "costCenters", costs, "costCenterLocked", Objects.equals(u.getAppId(), 5),
                "activities", SaleOrderReportColumns.ACTIVITIES,
                "actionRights", repo.actionRights(u),
                "packUoms", List.of(1, 5, 10, 20, 25, 40, 50, 60, 65, 80, 100)); // native ItemUOMFill, not DB values
    }
    public List<Map<String, Object>> lookups(List<Integer> branchIds, int costCenterId) {
        var u = user(); validateCostCenter(u, costCenterId, false);
        return repo.lookups(u, branchFilter(u, branchIds), costCenterId);
    }
    public Map<String, Object> detail(SaleOrderReportFilter f) {
        var u = user(); validate(u, f);
        var raw = repo.detail(u, f, branchFilter(u, f.branchIds()));
        var detail = raw.stream().map(r -> SaleOrderReportColumns.project(r, SaleOrderReportColumns.DETAIL)).toList();
        Map<Integer, List<Map<String, Object>>> grouped = new LinkedHashMap<>();
        for (var row : raw) grouped.computeIfAbsent(((Number) row.get("Id")).intValue(), ignored -> new ArrayList<>()).add(row);
        List<Map<String, Object>> headers = new ArrayList<>();
        Map<Integer, List<Map<String, Object>>> lines = new LinkedHashMap<>();
        for (var entry : grouped.entrySet()) {
            var row = SaleOrderReportColumns.project(entry.getValue().get(0), SaleOrderReportColumns.HEADER);
            // LINQ Sum<double> is sequential; retain native first-row DispatchQty on the header.
            double amount = 0, balance = 0;
            for (var line : entry.getValue()) {
                amount += ((Number) line.get("Amount")).doubleValue();
                balance += ((Number) line.get("BalWeight")).doubleValue();
            }
            row.put("OrderAmount", amount); row.put("BalWeight", balance); headers.add(row);
            lines.put(entry.getKey(), entry.getValue().stream().map(r -> SaleOrderReportColumns.project(r, SaleOrderReportColumns.LINES)).toList());
        }
        return Map.of("rows", detail, "columns", SaleOrderReportColumns.names(SaleOrderReportColumns.DETAIL),
                "headers", headers, "headerColumns", SaleOrderReportColumns.names(SaleOrderReportColumns.HEADER),
                "lines", lines, "lineColumns", SaleOrderReportColumns.names(SaleOrderReportColumns.LINES));
    }
    public Map<String, Object> summary(SaleOrderReportFilter f) {
        var u = user(); validate(u, f); String spec = SaleOrderReportColumns.summary(f.activity());
        var rows = repo.summary(u, f, branchFilter(u, f.branchIds()));
        return Map.of("rows", rows.stream().map(r -> SaleOrderReportColumns.project(r, spec)).toList(),
                "columns", SaleOrderReportColumns.names(spec));
    }
    private void validate(UserAccount u, SaleOrderReportFilter f) {
        if (f == null || f.toDate() == null) throw new IllegalArgumentException("To Date is required");
        if (f.status() != null && !Set.of("", "Open", "Cancel", "Complete").contains(f.status()))
            throw new IllegalArgumentException("Select a valid order status");
        if (f.approval() == null || !Set.of("UnApprove", "Approve", "All").contains(f.approval()))
            throw new IllegalArgumentException("Select an approval status");
        validateCostCenter(u, f.costCenterId(), true);
    }
    private void validateCostCenter(UserAccount u, int id, boolean required) {
        if (required && Objects.equals(u.getAppId(), 5) && id == 0) throw new IllegalArgumentException("Cost Center Not Found");
        if (id != 0 && repo.costCenters(u).stream().noneMatch(r -> ((Number) r.get("Id")).intValue() == id))
            throw new AccessDeniedException("The selected cost center is not available to this user");
    }
    private String branchFilter(UserAccount u, List<Integer> ids) {
        if (ids == null || ids.isEmpty()) throw new IllegalArgumentException("Select Branch First");
        Set<Integer> allowed = repo.branches(u).stream().map(r -> ((Number) r.get("BranchId")).intValue()).collect(Collectors.toSet());
        if (ids.stream().anyMatch(id -> id == null || !allowed.contains(id)))
            throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(",", ",", ""));
    }
}
