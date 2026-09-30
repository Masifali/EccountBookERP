package com.mst.services;

import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 14 "User Chart Of Account Management" - desktop form
 * Architecture.WinApp.Account_Definition.UserChartOfAccountManagement, BLL/DAL Accounts.CoaCustomGroupForUsers,
 * CoaCustomGroupAlocToUsers, CoaAlocToCustomGroupForUsers and UserAccount. Four tabs:
 * User Allocate For Accounts Status, Custom Group Definition (+ custom groups to a user),
 * Coa Accounts Allocation To Custom Group, History By User. Every call below is the desktop's.
 * The form checks no rights.
 */
@Service
public class UserCoaManagementDesktopService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public UserCoaManagementDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    private int org() { return ctx.currentOrganizationId(); }
    private int company() { return ctx.currentCompanyId(); }

    /** frmAccountCustomGroup_Load (the form's Load handler keeps that name) / btnrefresh_Click. */
    public Map<String, Object> lookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("gridGroups", gridGroups());
        /* AllUsers: UserAccount.GetUsersFromAccountsShowHideStatus */
        out.put("users", DesktopProc.rows(jdbc, "USP_GetUsersFromAccountsShowHideStatus", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company())));
        /* CutomGroupFill: CoaCustomGroupForUsers.BindCustomGroupForAlocToCoa (UserId 0 -> not sent) */
        out.put("customGroups", DesktopProc.rows(jdbc, "[dbo].[USP_BindCustomGroupForAlocToCoa]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company())));
        /* ThirdLevelAccountsFill: ReadAllAccountgroup Account_Level = 3 (the 2nd-level combo is Visible = false) */
        out.put("thirdLevel", DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "FinancialYearId", ctx.currentFinancialYearId(),
                "Account_Level", 3, "CoaType", "ReadAllAccountGroup")));
        /* AccountTypeCombo: AccountTypes.GetAll(new AccountTypes()) - both ids 0 */
        out.put("accountTypes", DesktopProc.rows(jdbc, "Proc_AccountTypes_ReadAll", DesktopProc.params(
                "OrganizationId", 0, "CompanyId", 0)));
        /* UsersBindForHistory: UserAccount.ReadAll */
        out.put("historyUsers", DesktopProc.rows(jdbc, "Sp_UserAccount_GetAllMethod", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "Activity", "ReadAll")));
        /* CoacustomgroupCombo: CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll TypeId 1 */
        out.put("coaCustomGroups", DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "AcLookUpTypesId", 1, "Activity", "ReadAll")));
        out.put("statusUsers", statusUsers());
        return out;
    }

    /** GridBind: CoaCustomGroupForUsers.GetAll with ApprovedFilter "All" (no @IsActive). */
    public List<Map<String, Object>> gridGroups() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_CoaCustomGroupForUsers_ReadAll]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company()));
    }

    /** AllUsersForAllocationAccountsStatus */
    public List<Map<String, Object>> statusUsers() {
        return DesktopProc.rows(jdbc, "USP_GetUsersForAccountsStatusAllocation", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company()));
    }

    /** InsertCustomGroup (the Save / Update confirmation is asked on the page). */
    @Transactional
    public String saveGroup(int recId, String groupName, boolean active) {
        String text = groupName == null ? "" : groupName;
        if (text.trim().isEmpty()) throw new IllegalArgumentException("Group Name Required");
        String proc = recId == 0 ? "[dbo].[USP_CoaCustomGroupForUsers_Insert]" : "[dbo].[USP_CoaCustomGroupForUsers_Update]";
        int num = DesktopProc.setProc(jdbc, proc, DesktopProc.params(
                "BranchId", ctx.currentBranchId(), "CompanyId", company(), "Id", recId,
                "IsActive", active ? 1 : 0, "OrganizationId", org(), "CoaCustomGroupNameForUsers", text));
        if (num <= 0) num = recId;
        return recId > 0 ? "Custom Group Update Successfully....  " + num : "Custom Group Save Successfully....  " + num;
    }

    /** CmbUsers_Leave: AllocatedCustomGroupToUser + DeAllocatedCustomGroupToUser. */
    public Map<String, Object> userGroups(int userId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("allocated", DesktopProc.rows(jdbc, "[dbo].[USP-GetAllCoaCustomGroupAlocToUser]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "UserId", userId)));
        /* CoaCustomGroupForUsers.GetAll: UserId (> 0), ApprovedFilter unset -> @IsActive = IsApproved (true) */
        out.put("unallocated", DesktopProc.rows(jdbc, "[dbo].[USP_CoaCustomGroupForUsers_ReadAll]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "UserId", userId > 0 ? userId : null, "IsActive", true)));
        return out;
    }

    /** AllocateCustomGroupToUser: returns the message, or null when the last insert returned 0. */
    @Transactional
    public String allocateGroupsToUser(int userId, List<Integer> groupIds) {
        if (groupIds == null || groupIds.isEmpty()) throw new IllegalArgumentException("Please Check Rows First To Allocate");
        int result = 0;
        for (Integer g : groupIds) {
            result = DesktopProc.setProc(jdbc, "[dbo].[USP_CoaCustomGroupAlocToUsers_Insert]", DesktopProc.params(
                    "CoaAlocToCustomGroupForUsersId", g == null ? 0 : g, "Id", 0, "UsersId", userId));
        }
        DesktopProc.rows(jdbc, "USP_AccountsStatusUserWiseInsertIntoTmpTable", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company()));
        return result > 0 ? "Custom Group's Allocated To User Successfully" : null;
    }

    /** DeAllocateCustomGroupToUser (confirmation asked on the page). */
    public String deallocateGroupsFromUser(int userId, List<Integer> groupIds) {
        if (groupIds == null || groupIds.isEmpty()) throw new IllegalArgumentException("Please Check Rows First To Allocate");
        DesktopProc.rows(jdbc, "[dbo].[USp_CoaCustomGroupAlocToUserDeleteById]", DesktopProc.params(
                "CustomGroupIds", csv(groupIds), "UserId", userId, "OrganizationId", org(), "CompanyId", company()));
        return "Record's UnAllocated Successfully";
    }

    /** btnShow_Click: UnAllocated + Allocated chart of accounts for the custom group. */
    public Map<String, Object> coaShow(int customGroupId, int thirdLevelAccountId, int accountTypeId, int coaCustomGroupId) {
        int branch = ctx.currentBranchId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("unallocated", DesktopProc.rows(jdbc, "[dbo].[USP-GetAllCoaNotAlocToCustomGroupToUsers]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "CustomGroupId", customGroupId,
                "BranchesId", branch > 0 ? branch : null,
                "ParentAccountId", thirdLevelAccountId > 0 ? thirdLevelAccountId : null,
                "AccountTypeId", accountTypeId > 0 ? accountTypeId : null,
                "CustomCoaGroupId", coaCustomGroupId > 0 ? coaCustomGroupId : null)));
        out.put("allocated", DesktopProc.rows(jdbc, "[dbo].[USP-GetAllCoaAlocToCustomGroupToUsers]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "CustomGroupId", customGroupId,
                "BranchesId", branch > 0 ? branch : null)));
        return out;
    }

    /** SaveChartOfAccountToCustomGroup: returns "Allocated Successfully" or null when the last insert returned 0. */
    @Transactional
    public String allocateCoa(int customGroupId, List<Integer> coaIds) {
        if (coaIds == null || coaIds.isEmpty()) throw new IllegalArgumentException("Checked row first");
        int result = 0;
        for (Integer id : coaIds) {
            result = DesktopProc.setProc(jdbc, "[dbo].[USP_CoaAlocToCustomGroupForUsers_Insert]", DesktopProc.params(
                    "BranchId", ctx.currentBranchId(), "ChartOfAccountId", id == null ? 0 : id,
                    "CoaCustomGroupForUsersId", customGroupId, "CompanyId", company(), "Id", 0, "OrganizationId", org()));
        }
        DesktopProc.rows(jdbc, "USP_AccountsStatusUserWiseInsertIntoTmpTable", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company()));
        return result > 0 ? "Allocated Successfully" : null;
    }

    /** btnDelete_Click (confirmation asked on the page). */
    public String deallocateCoa(int customGroupId, List<Integer> coaIds) {
        if (coaIds == null || coaIds.isEmpty()) throw new IllegalArgumentException("Checked row first");
        DesktopProc.rows(jdbc, "[dbo].[USp_CoaAlocToCustomGroupForUserDeleteById]", DesktopProc.params(
                "CoaIds", csv(coaIds), "CustomGroupId", customGroupId, "OrganizationId", org(), "CompanyId", company()));
        return "Record's UnAllocated Successfully";
    }

    /** BindHistorybyUser */
    public List<Map<String, Object>> history(int userId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP-CoaAlocToCustomGroupHistorybyUser]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "UserId", userId > 0 ? userId : null));
    }

    /** button1_Click (confirmation asked on the page): rows [{id, userId, accountStatus, isActive}]. */
    @Transactional
    public void saveAccountStatus(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Checked row first");
        LocalDateTime now = LocalDateTime.now();
        int user = ctx.currentUserId();
        for (Map<String, Object> r : rows) {
            if (toInt(r.get("accountStatus")) == 0) throw new IllegalArgumentException("AccountStatus Field is required");
        }
        for (Map<String, Object> r : rows) {
            DesktopProc.setProc(jdbc, "[dbo].[USP_AccountsShowAndHideStatusUserWise_Insert]", DesktopProc.params(
                    "IsActive", toBool(r.get("isActive")), "EntryDate", now, "AccountStatus", toInt(r.get("accountStatus")),
                    "CompanyId", company(), "EntryUserId", user, "Id", toInt(r.get("id")), "ModifyDate", now,
                    "ModifyUserId", user, "OrganizationId", org(), "UserId", toInt(r.get("userId"))));
        }
    }

    private static String csv(List<Integer> ids) {
        StringBuilder sb = new StringBuilder();
        for (Integer i : ids) sb.append(i == null ? "" : String.valueOf(i)).append(',');
        return sb.toString();
    }

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = o == null ? "" : String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
