package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Admin Panel -> "Account Movement" (DashboardNew btnAccountMovement_Click -> Architecture.WinApp.AccountToAccountTransfer,
 * Text "Account Transfer", 537 x 156).
 *
 * Desktop data sources, copied:
 * <ul>
 *   <li>{@link #accounts()} - clsGlobalVariables.AllAccountsWithCustomGroupId, filled by DatatableHelper
 *       .GlobalServicesDbCall("AccountsWithCustomGroupId") -> GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup
 *       (orgId, compId, 0, 0, "") -> [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId, @CompanyId
 *       (PageSize/PageNumber 0 and Keyword "" are not sent). The form keeps the first row per ChartOfAccountId
 *       (HashSet seenIds) with columns Id, AccountTitle, AccountCode, AccountClass (= AccountClassName),
 *       AccountType, ParentAccountId (= ParentCodeId), ParentAccountTitle, AccountClassId (= AccountClass),
 *       AccountTypeId.</li>
 *   <li>{@link #thirdLevelAccounts()} - ThirdLevelAccountsFill -> BLL 0639 ChartofAccount.ReadAllAccountgroup ->
 *       Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId, @CompanyId, @FinancialYearId (ActiveYr.Id),
 *       @Account_Level=3, @CoaType='ReadAllAccountGroup' (LanguageId/AccountTypeId/AccountClassId 0, ids "" - not sent).
 *       Columns Id, AccountTitle, AccountCode, AccountType, AccountClass (= ClassName), ParentAccount (= ParentAccountTitle).</li>
 *   <li>{@link #update} - btnUpdate_Click -> BLL 0654 VoucherHead.ChartOfAccountParentUpdate -> one SqlTransaction,
 *       [dbo].[usp_AccountMoveToAnotherParentGroup] @OrganizationId, @CompanyId, @UserId, @ChartOfAccountId, @ActionId,
 *       then @ParentAccountId (ActionId 1) or @ToChartOfAccountId (ActionId 2). ExecuteNonQuery.</li>
 * </ul>
 * The form checks no screen rights: it is reachable only from Admin Panel, which DashboardNew_Load shows only
 * when RoleName == "Admin" (DashboardNew.cs:1191) - the same gate is applied to every call here.
 */
@Service
public class AccountMovementService {

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext currentUserContext;

    public UserAccount admin() {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!"Admin".equals(currentUserContext.currentRoleName()))
            throw new AccessDeniedException("Admin Panel - Account Movement is shown on the desktop only when RoleName is \"Admin\".");
        return u;
    }

    /** AcfrmAcAllocation_Load: ThirdLevelAccountsFill + AccountFillFromGlobal - both lists in one call. */
    public Map<String, Object> load() {
        admin();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("thirdLevel", thirdLevelAccounts());
        out.put("accounts", accounts());
        return out;
    }

    public List<Map<String, Object>> accounts() {
        UserAccount u = admin();
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            int id = toInt(a.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(a.get("AccountTitle")));
            o.put("AccountCode", str(a.get("AccountCode")));
            o.put("AccountClass", str(a.get("AccountClassName")));
            o.put("AccountType", str(a.get("AccountType")));
            o.put("ParentAccountId", toInt(a.get("ParentCodeId")));
            o.put("ParentAccountTitle", str(a.get("ParentAccountTitle")));
            o.put("AccountClassId", toInt(a.get("AccountClass")));
            o.put("AccountTypeId", toInt(a.get("AccountTypeId")));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> thirdLevelAccounts() {
        UserAccount u = admin();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", currentUserContext.currentFinancialYearId(),
                "Account_Level", 3,
                "CoaType", "ReadAllAccountGroup"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("AccountTitle", str(r.get("AccountTitle")));
            o.put("AccountCode", str(r.get("AccountCode")));
            o.put("AccountType", str(r.get("AccountType")));
            o.put("AccountClass", str(r.get("ClassName")));
            o.put("ParentAccount", str(r.get("ParentAccountTitle")));
            out.add(o);
        }
        return out;
    }

    /**
     * btnUpdate_Click, the same checks in the same order with the same texts, then the procedure in one transaction.
     * The class/type rules read the From/To rows of the same account list the combos were filled from.
     */
    @Transactional
    public Map<String, Object> update(Map<String, Object> body) {
        UserAccount u = admin();
        int detailId = toInt(body.get("detailAccountId"));
        boolean merge = truthy(body.get("merge"));
        int thirdLevelId = toInt(body.get("thirdLevelAccountId"));
        int toId = toInt(body.get("toAccountId"));

        if (detailId == 0) throw new IllegalArgumentException("Detail Account field is required");
        if (merge) {
            if (toId == 0) throw new IllegalArgumentException("To Account Title field is required");
            Map<String, Object> from = null, to = null;
            for (Map<String, Object> a : accounts()) {
                int id = toInt(a.get("Id"));
                if (id == detailId) from = a;
                if (id == toId) to = a;
            }
            int fromClass = from == null ? 0 : toInt(from.get("AccountClassId"));
            int toClass = to == null ? 0 : toInt(to.get("AccountClassId"));
            int fromType = from == null ? 0 : toInt(from.get("AccountTypeId"));
            int toType = to == null ? 0 : toInt(to.get("AccountTypeId"));
            if (fromClass == 4 && toClass != 4)
                throw new IllegalArgumentException("Expense Class Account Transactions can be transfer to only Expense Class Accounts");
            if (fromClass == 5 && toClass != 5)
                throw new IllegalArgumentException("Revenue Class Account Transactions can be transfer to only Revenue Class Accounts");
            if ((fromType == 2 || fromType == 15) && toType != 2 && toType != 15)
                throw new IllegalArgumentException((fromType == 2 ? "Cash Equivalent" : "Bank Equivalent")
                        + " account transactions can only be transferred to Cash Equivalent or Bank Equivalent accounts.");
        } else if (thirdLevelId == 0) {
            throw new IllegalArgumentException("Third Level Account field is required");
        }

        int actionId = merge ? 2 : 1;
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "UserId", u.getId(),
                "ChartOfAccountId", detailId,
                "ActionId", actionId);
        if (actionId == 1) p.put("ParentAccountId", thirdLevelId);
        else p.put("ToChartOfAccountId", toId);
        DesktopProc.scalar(jdbc, "[dbo].[usp_AccountMoveToAnotherParentGroup]", p);

        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", "Parent Account Change Successfully");
        return r;
    }

    // ------------------------------------------------------------------ helpers (Conversion.ToInt / ToString)

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o == null) return 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s.replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    private static boolean truthy(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o == null) return false;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }
}
