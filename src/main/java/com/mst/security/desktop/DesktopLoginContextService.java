package com.mst.security.desktop;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

/**
 * The four lists LoginNew.cs asks for after the password is accepted, each from the procedure the
 * desktop calls for it.
 *
 * <pre>
 *   Company          UserAccountAllocation.GetAllCompaniesByUserId            (BLL 0086)
 *                    sp_UserAccountAllocation_GetAllMethod
 *                    @OrganizationId, @UserAccountId, @IsActive (only when non-zero),
 *                    @Activity='GetCompaniesByUserId'          -> CompanyId, CompName, IsHeadOffice
 *
 *   Branch           BranchesAllocationToUser.GetBranchsAllocatedToUser       (BLL 0017)
 *                    [dbo].[USP_GetBranchsAllocatedToUser]
 *                    @OrganizationId, @CompanyId, @UserId      -> BranchId, BranchName, IsHeadOffice
 *
 *   Financial year   FinancialYear.GetFinancialYearlist                       (BLL 0071)
 *                    Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId
 *                    @OrganizationId, @CompanyId               -> Id, FinancialYearCode
 *
 *   Application      ApplicationsAllocateToUser.ApplicationsAllocateToUser_ByUser (BLL 0004)
 *                    [dbo].[USP_ApplicationsAllocateToUser_ByUser]
 *                    @CompanyId, @UserId, @MasterAppId = 1     -> AppId, AppName
 * </pre>
 *
 * ---------------------------------------------------------------------------------------------
 * A FINDING WHILE PORTING THIS
 * ---------------------------------------------------------------------------------------------
 * {@code CurrentUserContext.currentAppId()} calls a DIFFERENT procedure -
 * {@code USP_ApplicationsAllocateToUser_AllocatedData @CompanyId, @UserId} - which is not the one
 * the login screen uses and takes no @MasterAppId. Left alone rather than changed underneath the
 * screens that already depend on it; the login picker uses the desktop's own procedure, and the
 * difference is recorded for a decision.
 *
 * ---------------------------------------------------------------------------------------------
 * EVERY CHOICE IS RE-VALIDATED SERVER SIDE
 * ---------------------------------------------------------------------------------------------
 * {@link #isAllocatedCompany}, {@link #isAllocatedBranch}, {@link #isActiveFinancialYear} and
 * {@link #isAllocatedApplication} re-run the same allocation read and check the submitted id
 * against it. A posted company id is a request, not a fact: without this, anyone could type
 * another company's id into the form and read its documents for the rest of the session.
 */
@Service
public class DesktopLoginContextService {

    @Autowired private DesktopUserAccountDal dal;

    // ------------------------------------------------------------------------------ lists

    /** LoginNew.cs:194 - IsActive is always 1 there. */
    public List<Map<String, Object>> companies(int organizationId, int userAccountId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@UserAccountId", userAccountId);
        p.put("@IsActive", 1);
        p.put("@Activity", "GetCompaniesByUserId");
        return dal.exec("sp_UserAccountAllocation_GetAllMethod", p);
    }

    /** LoginNew.cs:225 */
    public List<Map<String, Object>> branches(int organizationId, int companyId, int userId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@UserId", userId);
        return dal.exec("[dbo].[USP_GetBranchsAllocatedToUser]", p);
    }

    /** LoginNew.cs:246 */
    public List<Map<String, Object>> financialYears(int organizationId, int companyId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        return dal.exec("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
    }

    /** LoginNew.cs:274 - MasterAppId is the literal 1 the desktop passes. */
    public List<Map<String, Object>> applications(int companyId, int userId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@CompanyId", companyId);
        p.put("@UserId", userId);
        p.put("@MasterAppId", 1);
        return dal.exec("[dbo].[USP_ApplicationsAllocateToUser_ByUser]", p);
    }

    // -------------------------------------------------------------------------- validation

    public boolean isAllocatedCompany(int organizationId, int userAccountId, int companyId) {
        return contains(companies(organizationId, userAccountId), "CompanyId", companyId);
    }

    public boolean isAllocatedBranch(int organizationId, int companyId, int userId, int branchId) {
        return contains(branches(organizationId, companyId, userId), "BranchId", branchId);
    }

    public boolean isActiveFinancialYear(int organizationId, int companyId, int financialYearId) {
        return contains(financialYears(organizationId, companyId), "Id", financialYearId);
    }

    public boolean isAllocatedApplication(int companyId, int userId, int appId) {
        return contains(applications(companyId, userId), "AppId", appId);
    }

    private static boolean contains(List<Map<String, Object>> rows, String column, int value) {
        if (value <= 0) return false;
        for (Map<String, Object> r : rows) {
            if (DesktopUserAccountDal.intOf(r, column) == value) return true;
        }
        return false;
    }

    /**
     * The desktop's "only ask when there is a choice" rule.
     *
     * LoginNew binds each combo with {@code ZeroIndex: true}, which puts a blank row in front of
     * the data, then tests {@code if (dt.Rows.Count != 2)} - two rows being blank plus one real
     * one. So: exactly one allocation proceeds silently, more than one stops and asks. Expressed
     * here against the real row count, which is the same test without the blank row.
     */
    public static boolean mustAsk(List<Map<String, Object>> rows) {
        return rows != null && rows.size() > 1;
    }

    /** The single row's id when there is exactly one, else 0. */
    public static int onlyId(List<Map<String, Object>> rows, String column) {
        if (rows == null || rows.size() != 1) return 0;
        return DesktopUserAccountDal.intOf(rows.get(0), column);
    }
}
