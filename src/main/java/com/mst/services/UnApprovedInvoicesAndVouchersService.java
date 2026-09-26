package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Undo Approval (Dashboard) - "UnApproved Vouchers".
 *
 * Ported from Architecture.WinApp.ApprovalDashboard\UnApprovedInvoicesAndVouchers.cs (788 lines).
 *
 * Two procedures, both from BLL.Dashboard, both taking the same six parameters:
 *
 *   Sp_Accounts_ApprovedVoucherDashboardForUnPost
 *   Sp_Inventory_ReadDashboardApprovedVouchersForUnPost
 *       @OrganizationId, @CompanyId, @FinancialYearId, @UserId    always
 *       @FromDate                                                 only when the From box is ticked
 *       @ToDate                                                   only when the To box is ticked
 *
 * Both From and To are DateTimePickers with ShowCheckBox (designer :700, :716), and
 * DynamicallyGenerateCards only copies each value when its box is ticked (:120-127). Ticking is
 * therefore part of the contract, not decoration, and the web form carries the same two boxes.
 *
 * The rows are split into the form's three panels (:129-143):
 *
 *   Accounts UnApproval Dashboard     the accounts rows,   card title = VoucherType
 *   Inventory UnApproval Dashboard    inventory, ActionId != 2, card title = Description
 *   CommissionAgent Dashboard         inventory, ActionId == 2, card title = Description
 *                                     - the panel is hidden entirely when that set is empty (:143)
 *
 * Every card carries TypeID as its Id and Count as its number.
 *
 * Read-only: this screen only opens other screens. It has no save, update or delete of its own,
 * and the un-approval itself happens on the form the card opens - none of which are ported yet,
 * so nothing here un-approves anything.
 */
@Service
public class UnApprovedInvoicesAndVouchersService {

    private static final Logger LOG =
            LoggerFactory.getLogger(UnApprovedInvoicesAndVouchersService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /** frmSaleTaxSummaryRpt_Load :85-99 - From is a week ago, To is today. */
    public Map<String, Object> defaults() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", LocalDate.now().minusDays(7).toString());
        out.put("toDate", LocalDate.now().toString());
        return out;
    }

    /** DynamicallyGenerateCards(), form :101-161. */
    public Map<String, Object> cards(String fromDate, String toDate) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> accounts = new ArrayList<>();
        List<Map<String, Object>> inventory = new ArrayList<>();
        List<Map<String, Object>> commission = new ArrayList<>();
        try {
            for (Map<String, Object> r : run("Sp_Accounts_ApprovedVoucherDashboardForUnPost",
                                             fromDate, toDate)) {
                accounts.add(card(r, "VoucherType"));
            }
        } catch (Exception e) {
            LOG.error("Accounts un-approval dashboard failed", e);
            out.put("accountsError", e.getMessage());
        }
        try {
            for (Map<String, Object> r : run("Sp_Inventory_ReadDashboardApprovedVouchersForUnPost",
                                             fromDate, toDate)) {
                /* :136-142 - ActionId 2 is the commission-agent panel, everything else inventory. */
                if (asInt(col(r, "ActionId")) == 2) commission.add(card(r, "Description"));
                else                                inventory.add(card(r, "Description"));
            }
        } catch (Exception e) {
            LOG.error("Inventory un-approval dashboard failed", e);
            out.put("inventoryError", e.getMessage());
        }
        out.put("accounts", accounts);
        out.put("inventory", inventory);
        out.put("commission", commission);
        /* PanelExportAndCommissionAgent.Visible = commissionRows.Any()  (:143) */
        out.put("showCommission", !commission.isEmpty());
        return out;
    }

    private Map<String, Object> card(Map<String, Object> r, String titleColumn) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("id", asInt(col(r, "TypeID")));
        c.put("title", str(col(r, titleColumn)));
        c.put("count", asInt(col(r, "Count")));
        return c;
    }

    private List<Map<String, Object>> run(String proc, String fromDate, String toDate) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId");  args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");       args.add(currentUserContext.currentCompanyId());
        names.add("@FinancialYearId"); args.add(currentUserContext.currentFinancialYearId());
        names.add("@UserId");          args.add(currentUserContext.currentUserId());

        Object from = date(fromDate);
        if (from != null) { names.add("@FromDate"); args.add(from); }
        Object to = date(toDate);
        if (to != null)   { names.add("@ToDate");   args.add(to); }

        StringBuilder sql = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
