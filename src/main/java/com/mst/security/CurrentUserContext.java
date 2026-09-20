package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.jdbc.core.JdbcTemplate;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;

@Component
public class CurrentUserContext {

    private static final Logger LOGGER = LoggerFactory.getLogger(CurrentUserContext.class);

	@Autowired
	private IUserAccountRepository userAccountRepository;

	@Autowired
	private JdbcTemplate jdbc;


	/** Mirrors LoginNew: active company allocation, then allocated branch. */
	private void resolveDesktopAllocations(UserAccount account) {
		try {
			if (account.getOrganizationId() == null || account.getId() == null) return;
			if (account.getCompanyId() == null || account.getCompanyId() == 0) {
				java.util.List<java.util.Map<String,Object>> companies = jdbc.queryForList(
					"EXEC dbo.sp_UserAccountAllocation_GetAllMethod @OrganizationId=?, @UserAccountId=?, @IsActive=?, @Activity=?",
					account.getOrganizationId(), account.getId(), 1, "GetCompaniesByUserId");
				if (!companies.isEmpty()) {
					Object value = companies.get(0).get("CompanyId");
					if (value instanceof Number) account.setCompanyId(((Number) value).intValue());
				}
			}
			if (account.getCompanyId() != null && account.getCompanyId() > 0
					&& (account.getBranchesId() == null || account.getBranchesId() == 0)) {
				java.util.List<java.util.Map<String,Object>> branches = jdbc.queryForList(
					"EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?, @CompanyId=?, @UserId=?",
					account.getOrganizationId(), account.getCompanyId(), account.getId());
				if (!branches.isEmpty()) {
					Object value = branches.get(0).get("BranchId");
					if (value instanceof Number) account.setBranchesId(((Number) value).intValue());
				}
			}
		} catch (Exception ignored) {
			// Preserve the Desktop failure semantics: callers requiring accounting
			// context reject unresolved users; no arbitrary tenant is substituted.
		}
	}

    /** This migration path must never substitute an arbitrary tenant. */
    public UserAccount requireAccountingUser() {
        Authentication auth = SecurityContextHolder.getContext().getAuthentication();
        if (auth == null || !auth.isAuthenticated()
                || auth instanceof org.springframework.security.authentication.AnonymousAuthenticationToken) {
            throw new org.springframework.security.authentication.AuthenticationCredentialsNotFoundException(
                    "Sign in before loading account codes");
        }
        /* Memoized FOR THIS REQUEST ONLY.
         *
         * currentUserId(), currentOrganizationId(), currentCompanyId() and currentBranchId() each
         * call this method, so a service that reads all four - saveVoucher reads five - issued
         * that many identical findByUserName queries, and resolveDesktopAllocations fired up to
         * two extra stored procedures on each of them. With spring.jpa.open-in-view=true the
         * request's JPA session holds its pooled connection for the whole request, so that extra
         * work is exactly what pushes a request past hikari's leak-detection-threshold (20s) and
         * produces the "Apparent connection leak" stack with findByUserName at its head.
         *
         * The cache lives in the request attributes, NOT in a field: this is a @Component, i.e. a
         * singleton, and a field would serve one user's identity to the next request. It is keyed
         * by the authenticated name so it cannot outlive the principal it was resolved for. With
         * no request bound (a scheduled job, a test) it simply resolves as before. */
        final String cacheKey = "com.mst.CurrentUserContext.user";
        org.springframework.web.context.request.RequestAttributes attrs =
                org.springframework.web.context.request.RequestContextHolder.getRequestAttributes();
        if (attrs != null) {
            Object cached = attrs.getAttribute(cacheKey,
                    org.springframework.web.context.request.RequestAttributes.SCOPE_REQUEST);
            if (cached instanceof Object[]) {
                Object[] pair = (Object[]) cached;
                if (auth.getName() != null && auth.getName().equals(pair[0])) {
                    return (UserAccount) pair[1];
                }
            }
        }

        UserAccount user = userAccountRepository.findByUserName(auth.getName());
        if (user != null) applyLoginContext(user, auth.getName());
        if (user != null) resolveDesktopAllocations(user);
        if (user == null || !Boolean.TRUE.equals(user.getIsActive()) || user.getOrganizationId() == null || user.getOrganizationId() <= 0 || user.getCompanyId() == null || user.getCompanyId() <= 0) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "The signed-in user has no accounting company context");
        }
        /* Only a fully validated user is cached - an unusable one must keep throwing. */
        if (attrs != null) {
            attrs.setAttribute(cacheKey, new Object[] { auth.getName(), user },
                    org.springframework.web.context.request.RequestAttributes.SCOPE_REQUEST);
        }
        return user;
    }

    /**
     * The signed-in user's RoleName - the value the desktop's Admin checks are made against.
     *
     * CommonServices.SetRightsValueInRightsObject (:17571) reads
     * {@code clsGlobalVariables.UserAccount.RoleName}, a column Sp_UserAccount_Login returns. It
     * is a VIRTUAL property on the desktop model - not a column of dbo.UserAccount - so it cannot
     * be recovered by reloading the user; it exists only in the login result, and
     * DesktopUserPrincipal carries it from there.
     *
     * UserGroup.UserGroupRole is the fallback, which is what this port used everywhere before the
     * login went through the procedure. It is still needed for a principal this provider did not
     * create (the receipt-manager bearer filter) and for a procedure that selects no RoleName
     * column.
     */
    public String currentRoleName() {
        try {
            org.springframework.security.core.Authentication auth =
                    SecurityContextHolder.getContext().getAuthentication();
            if (auth != null && auth.getPrincipal() instanceof com.mst.security.DesktopUserPrincipal) {
                String fromLogin = ((com.mst.security.DesktopUserPrincipal) auth.getPrincipal()).getRoleName();
                if (fromLogin != null && !fromLogin.trim().isEmpty()) return fromLogin.trim();
            }
            UserAccount u = requireAccountingUser();
            if (u.getUserGroup() == null || u.getUserGroup().getUserGroupRole() == null) return "";
            return u.getUserGroup().getUserGroupRole().trim();
        } catch (Exception e) {
            /* An unresolvable role is the RESTRICTIVE answer everywhere it is used - it simply
               fails the "is this Admin" test - so an empty string is safe here. */
            return "";
        }
    }

    /**
     * The company, branch and application the operator chose at login win over the columns on
     * their UserAccount row.
     *
     * dbo.UserAccount carries ONE CompanyId and ONE BranchesId, but a user can be allocated to
     * several of each - which is why LoginNew.cs asks. Before the login pickers existed this
     * class had no choice but to use the row; now, when a session-held LoginContext is present
     * and belongs to this principal, it is the answer.
     *
     * Nothing is written back to the row: that would change the company for every other session
     * the user has open, and for the desktop client too.
     *
     * With no context - an API caller on a bearer token, a scheduled job, a session from before
     * this change - the row is used exactly as before, so nothing that works today stops working.
     */
    private void applyLoginContext(UserAccount user, String userName) {
        try {
            org.springframework.web.context.request.RequestAttributes attrs =
                    org.springframework.web.context.request.RequestContextHolder.getRequestAttributes();
            if (!(attrs instanceof org.springframework.web.context.request.ServletRequestAttributes)) return;
            javax.servlet.http.HttpServletRequest request =
                    ((org.springframework.web.context.request.ServletRequestAttributes) attrs).getRequest();
            com.mst.security.LoginContext context = com.mst.security.LoginContext.of(request, userName);
            if (context == null || !context.isComplete()) return;
            if (context.getCompanyId() > 0)  user.setCompanyId(context.getCompanyId());
            if (context.getBranchId() > 0)   user.setBranchesId(context.getBranchId());
            if (context.getAppId() > 0)      user.setAppId(context.getAppId());
        } catch (Exception e) {
            /* A context that cannot be read must not take the request down - the row is still a
               valid answer, and it is the one this class used before the pickers existed. */
            LOGGER.debug("Could not apply the session login context", e);
        }
    }

    /** The financial year held in the session's LoginContext, or 0. */
    private int chosenFinancialYearId() {
        try {
            org.springframework.security.core.Authentication auth =
                    SecurityContextHolder.getContext().getAuthentication();
            if (auth == null || auth.getName() == null) return 0;
            org.springframework.web.context.request.RequestAttributes attrs =
                    org.springframework.web.context.request.RequestContextHolder.getRequestAttributes();
            if (!(attrs instanceof org.springframework.web.context.request.ServletRequestAttributes)) return 0;
            javax.servlet.http.HttpServletRequest request =
                    ((org.springframework.web.context.request.ServletRequestAttributes) attrs).getRequest();
            com.mst.security.LoginContext context =
                    com.mst.security.LoginContext.of(request, auth.getName());
            return (context != null && context.isComplete()) ? context.getFinancialYearId() : 0;
        } catch (Exception e) {
            return 0;
        }
    }

    private int requireId(Integer value,String field) {
        if(value==null||value<=0)throw new org.springframework.security.access.AccessDeniedException("The signed-in user has no active "+field+" context");
        return value;
    }
    public int currentUserId(){return requireId(requireAccountingUser().getId(),"user");}
    public int currentOrganizationId(){return requireId(requireAccountingUser().getOrganizationId(),"organization");}
    public int currentCompanyId(){return requireId(requireAccountingUser().getCompanyId(),"company");}
    public int currentBranchId(){return requireId(requireAccountingUser().getBranchesId(),"branch");}
    public int currentAppId(){
        UserAccount account=requireAccountingUser();
        var apps=jdbc.queryForList("EXEC dbo.USP_ApplicationsAllocateToUser_AllocatedData @CompanyId=?,@UserId=?",account.getCompanyId(),account.getId());
        if(account.getAppId()!=null&&account.getAppId()>0&&apps.stream().anyMatch(a->((Number)a.get("AppId")).intValue()==account.getAppId()))return account.getAppId();
        if(apps.size()==1)return requireId(((Number)apps.get(0).get("AppId")).intValue(),"application");
        throw new org.springframework.security.access.AccessDeniedException("Select an application allocated to the signed-in user");
    }

	/**
	 * Ditto of LoginNew.cs's Financial-Year selection: desktop calls
	 * Architecture.BLL.FinancialYear.GetFinancialYearlist(...) -&gt; real proc
	 * Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId, which filters
	 * FinancialYear WHERE OrganizationId=@OrganizationId AND CompanyId=@CompanyId
	 * AND LoginStatus=1, ORDER BY DefaultFinYear DESC. Desktop then auto-selects
	 * Finlst[0] when only one row comes back, or lets the user pick from
	 * CmbFinancialYear when there is more than one; since this web session has
	 * no such picker yet, the first row (DefaultFinYear DESC puts the real
	 * default year first) is used - never a hardcoded id.
	 */
	public int currentFinancialYearId() {
		UserAccount account = requireAccountingUser();
		/* The year chosen at login wins, the same way clsGlobalVariables.ActiveYr does on the
		   desktop. Without a chosen one this falls through to the procedure below, which is what
		   this method did before the picker existed. */
		int chosen = chosenFinancialYearId();
		if (chosen > 0) return chosen;
		if (account == null || account.getOrganizationId() == null || account.getCompanyId() == null) {
			throw new org.springframework.security.access.AccessDeniedException("The signed-in user has no active financial year context");
		}
		try {
			java.util.List<java.util.Map<String, Object>> years = jdbc.queryForList(
					"EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
					account.getOrganizationId(), account.getCompanyId());
			if (!years.isEmpty()) {
				Object value = years.get(0).get("Id");
				if (value instanceof Number) {
					return ((Number) value).intValue();
				}
			}
		} catch (Exception ignored) {
		}
		throw new org.springframework.security.access.AccessDeniedException("The signed-in user has no active financial year context");
	}
}
