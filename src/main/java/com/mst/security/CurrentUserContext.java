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
        if (user == null || !Boolean.TRUE.equals(user.getIsActive()) || user.getOrganizationId() == null || user.getOrganizationId() <= 0 || user.getCompanyId() == null || user.getCompanyId() <= 0) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "The signed-in user has no accounting company context");
        }
        return user;
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
