package com.mst.services;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.StringUtils;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;
import com.mst.security.LegacyUserPasswordEncoder;
import com.mst.serviceInterface.IUserAccountService;

/**
 * Ditto of the desktop's UserAccount BLL - registration/login-user CRUD.
 *
 * ID GENERATION - the real dbo.UserAccount.ID has no IDENTITY clause (app-assigned),
 * so addOrUpdate() computes the next id itself (max(id)+1) on insert, same convention
 * as every other non-identity master table in this port.
 *
 * PASSWORD - stored directly on UserAccount.password (the real dbo.UserAccount.Password
 * column), encrypted with {@link LegacyUserPasswordEncoder} - the same
 * Rijndael/AES scheme the real desktop app uses. See UserAccount's Javadoc for the
 * confirmation of that scheme.
 *
 * The User Registration form's "password" input has the same name as this entity's
 * own "password" property, so Spring's @ModelAttribute binding pours the *raw
 * plaintext* the user typed straight into userAccount.password before this method
 * ever runs. addOrUpdate() always overwrites that field itself (never trusts the
 * incoming value) with either a freshly-encrypted new password or the previously-
 * stored encrypted value, so plaintext is never persisted.
 */
@Service
public class UserAccountService implements IUserAccountService {

	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private LegacyUserPasswordEncoder legacyUserPasswordEncoder;

	@Override
	public List<UserAccount> getAll() {
		return userAccountRepository.findAllByOrderByUserName();
	}

	@Override
	public UserAccount getById(Integer id) {
		return id == null ? null : userAccountRepository.findById(id).orElse(null);
	}

	@Override
	public UserAccount getByUserName(String userName) {
		return userAccountRepository.findByUserName(userName);
	}

	@Override
	public UserAccount addOrUpdate(UserAccount userAccount, String plainTextPassword) {
		LocalDateTime now = LocalDateTime.now();
		boolean isNew = userAccount.getId() == null;

		String existingEncryptedPassword = null;
		if (!isNew) {
			UserAccount existing = userAccountRepository.findById(userAccount.getId()).orElse(null);
			existingEncryptedPassword = existing != null ? existing.getPassword() : null;
		}

		if (StringUtils.hasText(plainTextPassword)) {
			// A new password was entered on the form - encrypt it with the same
			// scheme the real desktop app uses and store it on UserAccount.password,
			// exactly like the desktop does.
			userAccount.setPassword(legacyUserPasswordEncoder.encode(plainTextPassword));
		} else {
			// Password field left blank on the form - keep whatever was already
			// stored (never overwrite it with the raw plaintext that Spring's
			// @ModelAttribute binding may have poured into this same-named field).
			userAccount.setPassword(existingEncryptedPassword);
		}

		if (isNew) {
			userAccount.setId(userAccountRepository.findMaxId() + 1);
			userAccount.setEntryDate(now);
			userAccount.setIsActive(userAccount.getIsActive() == null || userAccount.getIsActive());
		}
		userAccount.setModifyDate(now);

		return userAccountRepository.save(userAccount);
	}

	@Override
	public void delete(Integer id) {
		userAccountRepository.deleteById(id);
	}

	@Override
	public boolean userNameExists(String userName) {
		return userAccountRepository.existsByUserName(userName);
	}
}
