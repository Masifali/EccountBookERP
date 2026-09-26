package com.mst.security;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;

/**
 * Bridges com.mst.models.UserAccount into Spring Security, and is where the menu takes effect:
 * fixed_sidebar.html gates every item with {@code hasAuthority('SOME_CODE')}, so a signed-in
 * user's menu is exactly the set of authorities loaded here.
 *
 * ---------------------------------------------------------------------------------------------
 * THE RIGHTS NOW COME ENTIRELY FROM THE DESKTOP'S OWN TABLES
 * ---------------------------------------------------------------------------------------------
 * This used to gate screens two different ways: the ones linked to a confirmed real
 * dbo.ScreenDefinition.Id went through the real chain, and everything else fell back to
 * MstUserRight - a table this port invented, joined to MstScreen, another one. Two tables the
 * desktop does not have, and two answers to the same question.
 *
 * Now there is one answer. {@link SidebarScreenCatalog} lists the screens this port has built (in
 * code, not in a table) and {@link DesktopScreenRightsService} asks the real chain about each:
 *
 *     dbo.CompanyRights   is the screen enabled for this user's company
 *     dbo.ScreenRights    the screen's "View" right
 *     dbo.tblUserRights   has THIS user been granted it
 *
 * MstScreen and MstUserRight are no longer read by anything.
 *
 * ---------------------------------------------------------------------------------------------
 * NO "ADMIN SEES EVERYTHING" FOR View - THAT IS THE DESKTOP'S RULE, NOT AN OVERSIGHT
 * ---------------------------------------------------------------------------------------------
 * CommonServices.SetRightsValueInRightsObject grants an Admin a blanket Save, Update, Delete,
 * Print and CanView AllRecord - but NOT View. View is read from the grant grid for every user,
 * administrator included. So an admin with no View grant on a screen does not see it on the
 * desktop, and does not see it here.
 *
 * The ROLE_ authority below is not used by any rights check; it is kept because templates and
 * older code refer to it.
 *
 * Password: read directly from the real dbo.UserAccount.Password column, which is
 * Rijndael/AES-encrypted and Base64-encoded - see {@link LegacyUserPasswordEncoder}. The password
 * is not verified here: DesktopLoginAuthenticationProvider does that through
 * Sp_UserAccount_Login, the way the desktop does.
 */
@Service
public class CustomUserDetailsService implements UserDetailsService {

	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private DesktopScreenRightsService screenRights;
	@Autowired
	private LegacyUserPasswordEncoder legacyUserPasswordEncoder;

	@Override
	public UserDetails loadUserByUsername(String userName) throws UsernameNotFoundException {
		UserAccount account = userAccountRepository.findByUserName(userName);
		if (account == null) {
			throw new UsernameNotFoundException("No such user: " + userName);
		}

		String role = account.getUserGroup() != null && account.getUserGroup().getUserGroupRole() != null
				? account.getUserGroup().getUserGroupRole().toUpperCase()
				: "USER";

		List<GrantedAuthority> authorities = new ArrayList<>();
		authorities.add(new SimpleGrantedAuthority("ROLE_" + role));
		authorities.addAll(resolveScreenAuthorities(account));

		String encryptedPassword = account.getPassword() != null
				? account.getPassword()
				: legacyUserPasswordEncoder.encode(java.util.UUID.randomUUID().toString());

		return org.springframework.security.core.userdetails.User
				.withUsername(account.getUserName())
				.password(encryptedPassword)
				.disabled(!Boolean.TRUE.equals(account.getIsActive()))
				.authorities(authorities)
				.build();
	}

	/**
	 * One authority per built screen the real chain grants, plus its section so the menu heading
	 * appears. A screen whose real ScreenDefinition row is not yet confirmed is not granted - see
	 * DesktopScreenRightsService for why hiding is the only safe default, and for the log line
	 * that names each one.
	 */
	private List<GrantedAuthority> resolveScreenAuthorities(UserAccount account) {
		List<GrantedAuthority> authorities = new ArrayList<>();
		if (account.getId() == null) return authorities;

		for (SidebarScreenCatalog.Entry entry :
				screenRights.viewableScreens(account.getId(), account.getCompanyId())) {
			if (entry.authorityCode != null && !entry.authorityCode.isEmpty()) {
				authorities.add(new SimpleGrantedAuthority(entry.authorityCode));
			}
			if (entry.sectionAuthorityCode != null && !entry.sectionAuthorityCode.isEmpty()) {
				authorities.add(new SimpleGrantedAuthority(entry.sectionAuthorityCode));
			}
		}
		return authorities;
	}
}
