package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.stereotype.Component;

import com.mst.models.UserAccount;
import com.mst.models.UserGroup;
import com.mst.repositories.IUserAccountRepository;
import com.mst.repositories.IUserGroupRepository;
import lombok.extern.slf4j.Slf4j;

@Slf4j
// Not registered: legacy ERP reference data must not be seeded at startup.
public class DefaultAdminSeeder implements CommandLineRunner {

	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private IUserGroupRepository userGroupRepository;
	@Autowired
	private LegacyUserPasswordEncoder legacyUserPasswordEncoder;

	@Override
	public void run(String... args) {
		try {
			if (userAccountRepository.existsByUserName("admin")) {
				return;
			}

			UserGroup adminGroup = null;
			try {
				adminGroup = userGroupRepository.findByUserGroupRole("ADMIN");
			} catch (Throwable ignored) {
			}

			if (adminGroup == null) {
				try {
					adminGroup = new UserGroup();
					adminGroup.setUserGroupName("Administrator");
					adminGroup.setUserGroupRole("ADMIN");
					adminGroup.setStatus(true);
					adminGroup = userGroupRepository.save(adminGroup);
				} catch (Throwable ignored) {
				}
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

			log.info("[DefaultAdminSeeder] Seeded first-run login: admin / admin123");
		} catch (Throwable t) {
			log.warn("DefaultAdminSeeder skipped: {}", t.getMessage());
		}
	}
}
