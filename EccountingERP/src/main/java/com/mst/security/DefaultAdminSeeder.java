package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.stereotype.Component;

import com.mst.models.UserAccount;
import com.mst.models.UserGroup;
import com.mst.repositories.IUserAccountRepository;
import com.mst.repositories.IUserGroupRepository;

/**
 * First-run bootstrap only: seeds one ADMIN role (UserGroup) and one login user
 * (admin / admin123) so there's a way to sign in and start using User Registration /
 * Roles / Rights to create real accounts. Not part of the desktop app - the desktop
 * ships with its own seeded SQL Server database, so this port needs an equivalent
 * bootstrap step for a brand-new database.
 *
 * Guarded per-row (existsByUserName("admin") / findByUserGroupRole("ADMIN")) rather
 * than by a whole-table row count, since this now runs against the real, already-
 * populated GoldenAcedb: it must never touch/duplicate anything if "admin" already
 * exists there, and must not skip seeding just because other real users already do.
 *
 * Password: written straight to UserAccount.password (the real dbo.UserAccount.Password
 * column), encrypted with {@link LegacyUserPasswordEncoder} - the same scheme the
 * real desktop app uses - per the user's explicit instruction to use dbo.UserAccount
 * for login credentials rather than a separate table.
 */
@Component
public class DefaultAdminSeeder implements CommandLineRunner {

	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private IUserGroupRepository userGroupRepository;
	@Autowired
	private LegacyUserPasswordEncoder legacyUserPasswordEncoder;

	@Override
	public void run(String... args) {
		if (userAccountRepository.existsByUserName("admin")) {
			return;
		}

		UserGroup adminGroup = userGroupRepository.findByUserGroupRole("ADMIN");
		if (adminGroup == null) {
			adminGroup = new UserGroup();
			adminGroup.setUserGroupName("Administrator");
			adminGroup.setUserGroupRole("ADMIN");
			adminGroup.setStatus(true);
			adminGroup = userGroupRepository.save(adminGroup);
		}

		UserAccount admin = new UserAccount();
		admin.setId(userAccountRepository.findMaxId() + 1);
		admin.setUserName("admin");
		admin.setFirstName("System");
		admin.setLastName("Administrator");
		admin.setUserGroup(adminGroup);
		admin.setIsActive(true);
		admin.setPassword(legacyUserPasswordEncoder.encode("admin123"));
		userAccountRepository.save(admin);

		System.out.println("[DefaultAdminSeeder] Seeded first-run login: admin / admin123 - change this immediately after first login.");
	}
}
