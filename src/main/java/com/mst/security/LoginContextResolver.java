package com.mst.security;

import com.mst.models.UserAccount;
import com.mst.security.desktop.DesktopLoginContextService;
import com.mst.security.desktop.DesktopUserAccountBll;
import com.mst.security.desktop.DesktopUserAccountDal;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpServletRequest;
import java.util.List;
import java.util.Map;

/**
 * LoginNew.cs steps 6-10, ported: choose the company, the branch, the financial year and the
 * application, then run the licence check.
 *
 * ---------------------------------------------------------------------------------------------
 * THE DESKTOP ONLY ASKS WHEN THERE IS A CHOICE
 * ---------------------------------------------------------------------------------------------
 * Each combo is bound with {@code ZeroIndex: true} (a blank row in front) and then tested with
 * {@code if (dt.Rows.Count != 2)}. Two rows means blank plus one real one, so:
 *
 *   exactly one allocation  ->  selected silently, the login continues
 *   more than one           ->  the button becomes "Continue" and the operator picks
 *   none                    ->  "Company record not found" / "Branch record not found"
 *
 * {@link #autoResolve} reproduces that, and it is also what decides whether the web shows its
 * picker page at all - a user with one company and one branch never sees it, exactly as on the
 * desktop.
 *
 * Financial year and application are gentler in the C#: both are bound only
 * {@code if (Count > 0)} and neither throws when empty, so neither blocks a login here.
 *
 * ---------------------------------------------------------------------------------------------
 * LICENCE CHECK IS PORTED BUT NOT CALLED BY DEFAULT
 * ---------------------------------------------------------------------------------------------
 * LoginNew.cs:435 calls {@code LicenseCheck(DeviceId)}, which passes a Windows device id, machine
 * name, IP and MAC address through to {@code GetMaxDate} - a procedure that WRITES a row. A
 * browser has only the IP. Rather than send three nulls into a write on a live database, it is
 * behind {@code app.login.license-check}, off by default, and the three missing values are left
 * null when it is switched on. That decision is yours to make, not mine to assume.
 */
@Service
public class LoginContextResolver {

    private static final Logger LOG = LoggerFactory.getLogger(LoginContextResolver.class);

    /** LoginNew.cs:205 and :230 - the desktop's own two messages. */
    public static final String NO_COMPANY = "Company record not found";
    public static final String NO_BRANCH  = "Branch record not found";

    @Autowired private DesktopLoginContextService lists;
    @Autowired private DesktopUserAccountBll userAccountBll;

    @Value("${app.login.license-check:false}")
    private boolean licenseCheckEnabled;

    /**
     * Builds the context as far as it can go without asking. Anything with exactly one option is
     * chosen; the first thing with more than one stops the walk and leaves the context incomplete,
     * which is what sends the operator to the picker page.
     */
    public LoginContext autoResolve(UserAccount user) {
        LoginContext context = new LoginContext(user.getUserName());
        int organizationId = user.getOrganizationId() == null ? 0 : user.getOrganizationId();
        int userId = user.getId() == null ? 0 : user.getId();

        List<Map<String, Object>> companies = lists.companies(organizationId, userId);
        if (companies.isEmpty()) throw new IllegalStateException(NO_COMPANY);
        if (DesktopLoginContextService.mustAsk(companies)) return context;   // ask
        applyCompany(context, companies.get(0));

        List<Map<String, Object>> branches =
                lists.branches(organizationId, context.getCompanyId(), userId);
        if (branches.isEmpty()) throw new IllegalStateException(NO_BRANCH);
        if (DesktopLoginContextService.mustAsk(branches)) return context;    // ask
        applyBranch(context, branches.get(0));

        List<Map<String, Object>> years = lists.financialYears(organizationId, context.getCompanyId());
        if (DesktopLoginContextService.mustAsk(years)) return context;       // ask
        if (years.size() == 1) applyFinancialYear(context, years.get(0));

        List<Map<String, Object>> apps = lists.applications(context.getCompanyId(), userId);
        if (DesktopLoginContextService.mustAsk(apps)) return context;        // ask
        if (apps.size() == 1) applyApplication(context, apps.get(0));

        context.setComplete(true);
        return context;
    }

    /**
     * Applies what the operator submitted, re-reading each allocation and refusing an id that is
     * not in it. A posted id is a request, not a fact: without this check anyone could type
     * another company's id into the form and read its documents for the rest of the session.
     */
    public LoginContext applyChoice(UserAccount user, Integer companyId, Integer branchId,
                                    Integer financialYearId, Integer appId) {
        LoginContext context = new LoginContext(user.getUserName());
        int organizationId = user.getOrganizationId() == null ? 0 : user.getOrganizationId();
        int userId = user.getId() == null ? 0 : user.getId();

        List<Map<String, Object>> companies = lists.companies(organizationId, userId);
        if (companies.isEmpty()) throw new IllegalStateException(NO_COMPANY);
        Map<String, Object> company = pick(companies, "CompanyId", companyId, companies.size() == 1);
        if (company == null) throw new IllegalArgumentException("Select a company allocated to you");
        applyCompany(context, company);

        List<Map<String, Object>> branches =
                lists.branches(organizationId, context.getCompanyId(), userId);
        if (branches.isEmpty()) throw new IllegalStateException(NO_BRANCH);
        Map<String, Object> branch = pick(branches, "BranchId", branchId, branches.size() == 1);
        if (branch == null) throw new IllegalArgumentException("Select a branch allocated to you");
        applyBranch(context, branch);

        List<Map<String, Object>> years = lists.financialYears(organizationId, context.getCompanyId());
        Map<String, Object> year = pick(years, "Id", financialYearId, years.size() == 1);
        if (year != null) applyFinancialYear(context, year);
        else if (!years.isEmpty() && years.size() > 1) {
            throw new IllegalArgumentException("Select a financial year");
        }

        List<Map<String, Object>> apps = lists.applications(context.getCompanyId(), userId);
        Map<String, Object> app = pick(apps, "AppId", appId, apps.size() == 1);
        if (app != null) applyApplication(context, app);
        else if (apps.size() > 1) {
            throw new IllegalArgumentException("Select an application allocated to you");
        }

        context.setComplete(true);
        return context;
    }

    /** LoginNew.cs:435 - run at the very end, after the context is settled. */
    public void licenseCheck(UserAccount user, LoginContext context, HttpServletRequest request) {
        if (!licenseCheckEnabled) return;
        try {
            userAccountBll.licenseCheck(
                    user.getOrganizationId() == null ? 0 : user.getOrganizationId(),
                    context.getCompanyId(),
                    user.getId() == null ? 0 : user.getId(),
                    null,                                  // DeviceId  - no browser equivalent
                    null,                                  // MachineName
                    request == null ? null : request.getRemoteAddr(),
                    null);                                 // LoginMacAddress
        } catch (Exception e) {
            /* The desktop refuses the login on an expired or missing licence. Reproduced: the
               message is the desktop's own and reaches the operator unchanged. */
            LOG.warn("Licence check failed for user {} company {}",
                     user.getId(), context.getCompanyId(), e);
            throw e instanceof RuntimeException ? (RuntimeException) e : new IllegalStateException(e);
        }
    }

    // ------------------------------------------------------------------------------ helpers

    /** The submitted row, or the single row when there is only one and nothing was submitted. */
    private static Map<String, Object> pick(List<Map<String, Object>> rows, String column,
                                            Integer submitted, boolean single) {
        if (rows == null || rows.isEmpty()) return null;
        if (submitted != null && submitted > 0) {
            for (Map<String, Object> r : rows) {
                if (DesktopUserAccountDal.intOf(r, column) == submitted) return r;
            }
            return null;                       // submitted something that is not allocated
        }
        return single ? rows.get(0) : null;
    }

    private static void applyCompany(LoginContext c, Map<String, Object> row) {
        c.setCompany(DesktopUserAccountDal.intOf(row, "CompanyId"),
                     DesktopUserAccountDal.stringOf(row, "CompName"),
                     DesktopUserAccountDal.boolOf(row, "IsHeadOffice"));
    }

    private static void applyBranch(LoginContext c, Map<String, Object> row) {
        c.setBranch(DesktopUserAccountDal.intOf(row, "BranchId"),
                    DesktopUserAccountDal.stringOf(row, "BranchName"));
    }

    private static void applyFinancialYear(LoginContext c, Map<String, Object> row) {
        c.setFinancialYear(DesktopUserAccountDal.intOf(row, "Id"),
                           DesktopUserAccountDal.stringOf(row, "FinancialYearCode"));
    }

    private static void applyApplication(LoginContext c, Map<String, Object> row) {
        c.setApplication(DesktopUserAccountDal.intOf(row, "AppId"),
                         DesktopUserAccountDal.stringOf(row, "AppName"));
    }
}
