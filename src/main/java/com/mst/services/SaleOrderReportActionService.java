package com.mst.services;

import com.mst.models.SaleOrderReportAction;
import com.mst.repositories.SaleOrderReportRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.dao.DataAccessException;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/** Status events from frmSaleOrderHistory; SQL retains the original update and audit rules. */
@Service
public class SaleOrderReportActionService {
    private static final Map<String, String> RIGHTS = Map.of(
            "Complete", "CanChangeOrderStatusToComplete", "Cancel", "CanChangeOrderStatusToCancel",
            "Open", "CanChangeOrderStatusToOpen", "UpdateExpiryDate", "CanChangeOrderExpiryDate");
    private final SaleOrderReportRepository repo;
    private final SaleOrderReportService reports;
    private final CurrentUserContext context;

    public SaleOrderReportActionService(SaleOrderReportRepository repo, SaleOrderReportService reports,
            CurrentUserContext context) { this.repo = repo; this.reports = reports; this.context = context; }

    public Map<String, Object> apply(SaleOrderReportAction request) {
        if (request == null || request.filter() == null || request.action() == null
                || !Set.of("header", "detail").contains(Objects.toString(request.grid(), "")))
            throw new IllegalArgumentException("Select a report row and action");
        boolean auto = "AutoComplete".equals(request.action());
        if (!auto && !RIGHTS.containsKey(request.action())) throw new IllegalArgumentException("Unknown order action");
        var u = context.requireAccountingUser(); var grants = repo.actionRights(u);
        if (auto ? !grants.contains(RIGHTS.get("Complete")) && !grants.contains(RIGHTS.get("Cancel"))
                : !grants.contains(RIGHTS.get(request.action())))
            throw new AccessDeniedException("The user does not have rights for this order action");
        var f = request.filter();
        boolean statusAllowed = auto ? Set.of("Open", "Cancel").contains(Objects.toString(f.status(), ""))
                : "Open".equals(request.action()) ? Set.of("Cancel", "Complete").contains(Objects.toString(f.status(), ""))
                : "Open".equals(f.status());
        if (!"Approve".equals(f.approval()) || !statusAllowed)
            throw new IllegalArgumentException("This action is not available for the selected approval and order status");
        if (!auto && (request.remarks() == null || request.remarks().isEmpty()))
            throw new IllegalArgumentException("Action Remarks Required");
        if ("UpdateExpiryDate".equals(request.action()) && (request.expiryDate() == null || request.bulk()))
            throw new IllegalArgumentException("Select one order and its new expiry date");

        // Re-run the native report with validated tenant, user, branch and cost-center filters.
        // Client-supplied order balances, dispatch quantities and document types are never trusted.
        var report = reports.detail(f);
        var rows = rows(report, auto || "header".equals(request.grid()) ? "headers" : "rows");
        var selected = selectedRows(request, rows);
        if (auto) {
            if (!"header".equals(request.grid()) || request.selections().size() != rows.size()
                    || request.selections().stream().map(SaleOrderReportAction.Selection::rowIndex).distinct().count() != rows.size())
                throw new IllegalArgumentException("The report changed. Press Show before Auto Complete");
            double tolerance = repo.completionTolerance(u);
            var eligible = selected.stream().filter(row -> ((Number) row.get("BalWeight")).doubleValue() <= tolerance).toList();
            if (eligible.isEmpty()) throw new IllegalArgumentException("No record found for auto complete");
            repo.autoComplete(u, eligible);
            // SQL may leave an order open based on SaleOrderValidate and expiry/tolerance configuration.
            return Map.of("message", "Auto Complete processed " + eligible.size() + " orders. Refreshing their status.",
                    "processed", eligible.size());
        }
        if (!request.bulk() && selected.size() != 1) throw new IllegalArgumentException("Select one order");
        // Native single-row Cancel checks DispatchQty; its bulk Cancel handler does not.
        if (!request.bulk() && "Cancel".equals(request.action())
                && ((Number) selected.get(0).get("DispatchQty")).doubleValue() > 0)
            throw new IllegalArgumentException("Record Not Cancel because Dispatch Qty gratter than zero");
        int processed = 0;
        try {
            for (var row : selected) {
                repo.updateStatus(u, ((Number) row.get("Id")).intValue(),
                        "Complete".equals(request.action()) ? "Status" : request.action(), request.remarks(),
                        "UpdateExpiryDate".equals(request.action()) ? request.expiryDate() : null);
                processed++;
            }
        } catch (DataAccessException e) {
            // The native batch is not atomic: never tell the user that earlier writes were rolled back.
            throw new IllegalStateException("The order action failed after " + processed
                    + " completed selections. Refresh the report before trying again.", e);
        }
        return Map.of("message", request.action() + " successful for " + processed + " selections.", "processed", processed);
    }

    private static List<Map<String, Object>> selectedRows(SaleOrderReportAction request, List<Map<String, Object>> rows) {
        if (request.selections() == null || request.selections().isEmpty())
            throw new IllegalArgumentException("There is no record to Update Status");
        List<Map<String, Object>> selected = new ArrayList<>();
        Set<Integer> indexes = new HashSet<>();
        for (var selection : request.selections()) {
            if (selection == null || selection.rowIndex() < 0 || selection.rowIndex() >= rows.size()
                    || !indexes.add(selection.rowIndex())) throw new IllegalArgumentException("Select valid report rows");
            var row = rows.get(selection.rowIndex());
            if (((Number) row.get("Id")).intValue() != selection.id())
                throw new IllegalArgumentException("The report changed. Press Show before updating an order");
            selected.add(row);
        }
        return selected;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Map<String, Object> report, String key) {
        return (List<Map<String, Object>>) report.get(key);
    }
}
