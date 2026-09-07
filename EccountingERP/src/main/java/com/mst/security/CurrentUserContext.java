package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;

@Component
public class CurrentUserContext {

	@Autowired
	private IUserAccountRepository userAccountRepository;

	private UserAccount resolve() {
		try {
			Authentication auth = SecurityContextHolder.getContext().getAuthentication();
			if (auth != null && auth.isAuthenticated() && auth.getName() != null) {
				UserAccount account = userAccountRepository.findByUserName(auth.getName());
				if (account != null) {
					return account;
				}
			}
		} catch (Exception ignored) {
		}
		return null;
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

	public int currentFinancialYearId() {
		return 1;
	}
}
