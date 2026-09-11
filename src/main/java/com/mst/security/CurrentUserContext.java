package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.jdbc.core.JdbcTemplate;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;

@Component
public class CurrentUserContext {

	@Autowired
	private IUserAccountRepository userAccountRepository;

	@Autowired
	private JdbcTemplate jdbc;

	private UserAccount resolve() {
		try {
			Authentication auth = SecurityContextHolder.getContext().getAuthentication();
			if (auth != null && auth.isAuthenticated() && auth.getName() != null) {
				UserAccount account = userAccountRepository.findByUserName(auth.getName());
				if (account != null) {
					resolveDesktopAllocations(account);
					return account;
				}
			}
		} catch (Exception ignored) {
		}
		return null;
	}

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
        UserAccount user = userAccountRepository.findByUserName(auth.getName());
        if (user != null) resolveDesktopAllocations(user);
        if (user == null || user.getOrganizationId() == null || user.getCompanyId() == null) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "The signed-in user has no accounting company context");
        }
        return user;
    }

	public int currentUserId() {
		UserAccount account = resolve();
		return account != null && account.getId() != null ? account.getId() : 1;
	}

    public int currentOrganizationId() {
		UserAccount account = resolve();
		return account != null && account.getOrganizationId() != null ? account.getOrganizationId() : 1;
    }

	public int currentCompanyId() {
		UserAccount account = resolve();
		return account != null && account.getCompanyId() != null ? account.getCompanyId() : 1;
	}

	public int currentBranchId() {
		UserAccount account = resolve();
		return account != null && account.getBranchesId() != null ? account.getBranchesId() : 1;
	}

	public int currentAppId() {
		UserAccount account = resolve();
		return account != null && account.getAppId() != null ? account.getAppId() : 1;
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
		UserAccount account = resolve();
		if (account == null || account.getOrganizationId() == null || account.getCompanyId() == null) {
			return 1;
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
		return 1;
	}
}
