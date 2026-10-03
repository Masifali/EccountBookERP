package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * DAL of the Admin Panel item "User Rights" - Architecture.WinApp.Configurations.frmUserRights (the user MASTER:
 * tabs "User Define" / "User History", inner tab "ApplicationsAllocateToUser") and the form its toolbar opens,
 * Architecture.WinApp.Lookups.frmBranchesAllocationToUser.
 *
 *   BLL 0087 UserAccount      ReadAll / GetByID / ReadAllRole / Save          DAL 0078 SetData (one SqlTransaction)
 *   BLL 0086 UserAccountAllocation.GetAllCompaniesByUserId
 *   BLL 0004 ApplicationsAllocateToUser   _AllocatedData / _UnAllocatedData / Save (DAL 0004, one transaction)
 *   BLL 0017 BranchesAllocationToUser     BranchesAllocatedOrUnAllocatedToUser / Save (DAL 0011) / DeleteById
 *   BLL 0062 Company.GetAlldt  (CommonServices.CompanyServiceBind - the Location grid)
 *   BLL 0054 CompanyFeatures   USP_GetERPFeaturesByCompanyId (GetERPFeatureById(16) = Device Dependency)
 *
 * GenericProvider.SetProc sends EVERY non-virtual property of the model as "@"+name and ADO.NET drops the null
 * ones, so each write below sends exactly the model's non-virtual properties - checked against procdure.utf8.sql
 * (23/09/2026 script) and the Architecture.Model classes 0099 UserAccount, 0104 UserProfile, 0071
 * UserAccountAllocation, 0065 ScreenRightstoUser, 0003 ApplicationsAllocateToUser, 0016 BranchesAllocationToUser.
 * No table, column or procedure is created or changed.
 */
@Repository
public class UserDefineRepository {

    private final JdbcTemplate jdbc;

    public UserDefineRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ reads

    /** UserAccount.ReadAll - Sp_UserAccount_GetAllMethod @OrganizationId @CompanyId @Activity='ReadAll'. */
    public List<Map<String, Object>> readAllUsers(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "Sp_UserAccount_GetAllMethod",
                params("OrganizationId", organizationId, "CompanyId", companyId, "Activity", "ReadAll"));
    }

    /** UserAccount.GetByID - @Id, 'ReadById' (the DAL then adds the allocations and the profile). */
    public List<Map<String, Object>> readUserById(int id) {
        return DesktopProc.rows(jdbc, "Sp_UserAccount_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL 0078 GetData, second read - 'GetUserAllocationByUserId' (UR.*, CompName). */
    public List<Map<String, Object>> allocationsOfUser(int id) {
        return DesktopProc.rows(jdbc, "Sp_UserAccount_GetAllMethod", params("Id", id, "Activity", "GetUserAllocationByUserId"));
    }

    /** DAL 0078 GetData, third read - SP_UserProfile_GetAllMethod @UserId 'GetUserProfileByUserId'. */
    public List<Map<String, Object>> profileOfUser(int userId) {
        return DesktopProc.rows(jdbc, "SP_UserProfile_GetAllMethod", params("UserId", userId, "Activity", "GetUserProfileByUserId"));
    }

    /** UserAccount.ReadAllRole - Sp_AppRoles_GetAllMethod @OrganizationId @CompanyId (Id, RoleDescription). */
    public List<Map<String, Object>> roles(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "Sp_AppRoles_GetAllMethod",
                params("OrganizationId", organizationId, "CompanyId", companyId));
    }

    /** CommonServices.CompanyServiceBind -> Company.GetAlldt: @OrgCompanyTypeId = the organization, 'ReadByOrganizationId'. */
    public List<Map<String, Object>> companiesOfOrganization(int organizationId) {
        return DesktopProc.rows(jdbc, "Sp_Company_GetAllMethod",
                params("OrgCompanyTypeId", organizationId, "Activity", "ReadByOrganizationId"));
    }

    /** CompanyFeatures.GetERPFeaturesByCompanyId - the login-time list GetERPFeatureById reads (Id, ERPFeatures). */
    public List<Map<String, Object>> erpFeatures(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", organizationId, "CompanyId", companyId));
    }

    /** ApplicationsAllocateToUser.GetApplicationsAllocateToCompany - no parameters (the login-time global list). */
    public List<Map<String, Object>> applicationsAllocatedToCompanies() {
        return DesktopProc.rows(jdbc, "USP_GetApplicationsAllocateToCompany", params());
    }

    /** UserAccountAllocation.GetAllCompaniesByUserId: @OrganizationId @UserAccountId [@IsActive when != 0] 'GetCompaniesByUserId'. */
    public List<Map<String, Object>> companiesOfUser(int organizationId, int userId, int isActive) {
        Map<String, Object> p = params("OrganizationId", organizationId, "UserAccountId", userId);
        if (isActive != 0) p.put("IsActive", isActive);
        p.put("Activity", "GetCompaniesByUserId");
        return DesktopProc.rows(jdbc, "sp_UserAccountAllocation_GetAllMethod", p);
    }

    public List<Map<String, Object>> appsAllocated(int companyId, int userId) {
        return DesktopProc.rows(jdbc, "USP_ApplicationsAllocateToUser_AllocatedData", params("CompanyId", companyId, "UserId", userId));
    }

    public List<Map<String, Object>> appsUnAllocated(int companyId, int userId) {
        return DesktopProc.rows(jdbc, "USP_ApplicationsAllocateToUser_UnAllocatedData", params("CompanyId", companyId, "UserId", userId));
    }

    /** BranchesAllocatedOrUnAllocatedToUser - ActionId 1 = not allocated, 2 = allocated (UserId, UserName, BranchId, BranchName). */
    public List<Map<String, Object>> branchesOfUser(int organizationId, int companyId, int userId, int actionId) {
        return DesktopProc.rows(jdbc, "USP_BranchsAllocatedOrUnAllocatedToUser",
                params("OrganizationId", organizationId, "CompanyId", companyId, "UserId", userId, "ActionId", actionId));
    }

    // ------------------------------------------------------------------ writes

    /**
     * DAL 0078 UserAccount.SetData - ONE transaction, in the desktop's order:
     *   1. Sp_UserAccount_Insert (ID == 0) / Sp_UserAccount_Update; the scalar is the new Id (Update returns none -> keep ID);
     *   2. Sp_UserProfile_Insert (profile Id == 0) / Sp_UserProfile_Update with UserId = that Id;
     *   3. per Location row: Sp_tblUserRights_GetAllMethod 'ScreenRightstoUser' (@UserId = UserGroupId, @NewUserid,
     *      @CompanyId), then Sp_UserAccountAllocation_Insert (which itself updates when the row's Id > 0).
     * Any RAISERROR rolls the whole thing back, as the desktop's catch -> Rollback does.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveUser(Map<String, Object> account, Map<String, Object> profile, List<Map<String, Object>> allocations,
                        int userGroupId, ProfileImageWriter imageWriter) {
        int id = asInt(account.get("Id"));
        int returned = DesktopProc.setProc(jdbc, id == 0 ? "Sp_UserAccount_Insert" : "Sp_UserAccount_Update", account);
        int num = returned > 0 ? returned : id;

        if (profile != null) {
            profile.put("UserId", num);
            if (imageWriter != null) profile.put("ProfileImageFileName", imageWriter.write());
            DesktopProc.setProc(jdbc, asInt(profile.get("Id")) == 0 ? "Sp_UserProfile_Insert" : "Sp_UserProfile_Update", profile);
        }

        for (Map<String, Object> a : allocations) {
            DesktopProc.setProc(jdbc, "Sp_tblUserRights_GetAllMethod",
                    params("UserId", userGroupId, "NewUserid", num, "CompanyId", a.get("CompanyId"), "Activity", "ScreenRightstoUser"));
            a.put("UserAccountId", num);
            a.put("ModifyUserId", num);
            a.put("EntryUserId", num);
            DesktopProc.setProc(jdbc, "Sp_UserAccountAllocation_Insert", a);
        }
        return num;
    }

    /** Stores the uploaded picture inside the save transaction (so a rollback removes it) and returns the stored name. */
    public interface ProfileImageWriter { String write(); }

    /** DAL 0004 ApplicationsAllocateToUser.SetData - one USP_ApplicationsAllocateToUser_InsertAndUpdate per checked row, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public long saveAppAllocations(List<Map<String, Object>> rows) {
        long result = 0;
        for (Map<String, Object> r : rows) result = DesktopProc.setProc(jdbc, "USP_ApplicationsAllocateToUser_InsertAndUpdate", r);
        return result;
    }

    /** DAL 0011 BranchesAllocationToUser.SetData - one USP_BranchesAllocationToUser_Insert per checked row, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int saveBranchAllocations(List<Map<String, Object>> rows) {
        int result = 0;
        for (Map<String, Object> r : rows) result = DesktopProc.setProc(jdbc, "USP_BranchesAllocationToUser_Insert", r);
        return result;
    }

    /** BranchesAllocationToUser.DeleteById - @UserId, @BranchIds = "id,id," exactly as the form builds it. */
    public void deleteBranchAllocations(int userId, String branchIds) {
        DesktopProc.setProc(jdbc, "USP_BranchesAllocationToUserDeleteById", params("UserId", userId, "BranchIds", branchIds));
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
