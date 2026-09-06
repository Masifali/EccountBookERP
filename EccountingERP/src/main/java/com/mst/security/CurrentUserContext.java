package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;

/**
 * Resolves the signed-in UserAccount (see CustomUserDetailsService) so services can
 * stamp EntryUser/ModifyUser/OrganizationId/CompanyId - real desktop audit columns
 * present on essentially every master table (Item, Brand, ItemCategory, ItemType,
 * ProductType, InvWareHouse, ...) - without each service re-implementing the lookup.
 * Falls back to id/org/company 1 when there's no authenticated user (e.g. a
 * CommandLineRunner seeder running before login), matching this project's existing
 * single-organization/single-company assumption elsewhere.
 */
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
			// fall through to defaults below
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
}
