package com.mst.security;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import com.mst.constants.RightType;
import com.mst.models.RealCompanyRight;
import com.mst.models.RealScreenRight;
import com.mst.models.RealUserRight;
import com.mst.models.Screen;
import com.mst.models.UserAccount;
import com.mst.models.UserRight;
import com.mst.repositories.IRealCompanyRightRepository;
import com.mst.repositories.IRealScreenRightRepository;
import com.mst.repositories.IRealUserRightRepository;
import com.mst.repositories.IScreenRepository;
import com.mst.repositories.IUserAccountRepository;
import com.mst.repositories.IUserRightRepository;

/**
 * Bridges com.mst.models.UserAccount (this port's ditto of the desktop's login-user
 * model) into Spring Security, and is also where the menu/rights actually take
 * effect: fixed_sidebar.html gates every menu item with hasAuthority('SOME_CODE'),
 * so a signed-in user's menu is exactly the set of authorities loaded here.
 *
 * IMPORTANT, per investigation of the real decompiled desktop source: there is NO
 * "admin sees everything" shortcut anywhere in Architecture.BLL/DAL -
 * UserGroup.UserGroupRole (this port's earlier "ADMIN" bypass column) is declared on
 * the model but never actually read by the real app. Every real user, including
 * whoever administers the system, only sees what real dbo.tblUserRights (the
 * per-user grant grid) plus real dbo.CompanyRights (the tenant-level "is this screen
 * even enabled for this company" gate) actually grant them, resolved through real
 * dbo.ScreenRights' "View" right - see resolveScreenAuthorities() below, and the
 * Javadoc on RealUserRight/RealCompanyRight/RealScreenRight for how those three real,
 * read-only tables fit together.
 *
 * Screens this port has linked to a confirmed real dbo.ScreenDefinition.Id (see
 * Screen.realScreenDefinitionId) are gated that way, exactly like the desktop.
 * Screens not yet linked keep using this port's own additively-new MstUserRight grid
 * as a fallback (see UserRight.java) until they're mapped too.
 *
 * Password: read directly from the real dbo.UserAccount.Password column, which is
 * Rijndael/AES-encrypted, Base64-encoded - see {@link LegacyUserPasswordEncoder}.
 */
@Service
public class CustomUserDetailsService implements UserDetailsService {

	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private IUserRightRepository userRightRepository;
	@Autowired
	private IScreenRepository screenRepository;
	@Autowired
	private IRealScreenRightRepository realScreenRightRepository;
	@Autowired
	private IRealUserRightRepository realUserRightRepository;
	@Autowired
	private IRealCompanyRightRepository realCompanyRightRepository;
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

	private List<GrantedAuthority> resolveScreenAuthorities(UserAccount account) {
		List<GrantedAuthority> authorities = new ArrayList<>();
		List<Screen> allScreens = screenRepository.findAll();

		List<Screen> realGated = allScreens.stream()
				.filter(s -> s.getRealScreenDefinitionId() != null)
				.collect(Collectors.toList());

		// --- Screens linked to a real dbo.ScreenDefinition.Id: gate exactly like the
		// desktop does, through real CompanyRights + tblUserRights + ScreenRights. ---
		if (!realGated.isEmpty()) {
			List<Integer> realScreenIds = realGated.stream()
					.map(Screen::getRealScreenDefinitionId)
					.collect(Collectors.toList());

			Set<Integer> companyEnabledScreenIds = realCompanyRightRepository
					.findByCompanyIdAndScreenIdInAndIsActiveTrue(account.getCompanyId(), realScreenIds)
					.stream().map(RealCompanyRight::getScreenId).collect(Collectors.toSet());

			Map<Integer, Integer> viewRightIdByScreenId = new HashMap<>();
			for (RealScreenRight sr : realScreenRightRepository.findByScreenIdInAndRightName(realScreenIds, "View")) {
				viewRightIdByScreenId.put(sr.getScreenId(), sr.getId());
			}

			List<Integer> viewRightIds = new ArrayList<>(viewRightIdByScreenId.values());
			Set<Integer> grantedRightIds = viewRightIds.isEmpty()
					? new HashSet<>()
					: realUserRightRepository
							.findByUserIdAndCompanyIdAndRightIdInAndValueTrue(account.getId(), account.getCompanyId(), viewRightIds)
							.stream().map(RealUserRight::getRightId).collect(Collectors.toSet());

			for (Screen screen : realGated) {
				Integer realId = screen.getRealScreenDefinitionId();
				if (!companyEnabledScreenIds.contains(realId)) {
					continue;
				}
				Integer viewRightId = viewRightIdByScreenId.get(realId);
				if (viewRightId == null || !grantedRightIds.contains(viewRightId)) {
					continue;
				}
				addScreenAuthorities(authorities, screen);
			}
		}

		// --- Screens not yet mapped to a real Id: this port's own MstUserRight grid. ---
		for (UserRight right : userRightRepository.findByUserId(account.getId())) {
			if (right.getRightType() != RightType.VIEW || !Boolean.TRUE.equals(right.getValue())) {
				continue;
			}
			Screen screen = right.getScreen();
			if (screen.getRealScreenDefinitionId() != null) {
				continue; // already handled by the real-rights block above
			}
			addScreenAuthorities(authorities, screen);
		}

		return authorities;
	}

	private void addScreenAuthorities(List<GrantedAuthority> authorities, Screen screen) {
		if (screen.getAuthorityCode() != null) {
			authorities.add(new SimpleGrantedAuthority(screen.getAuthorityCode()));
		}
		if (screen.getSectionAuthorityCode() != null) {
			authorities.add(new SimpleGrantedAuthority(screen.getSectionAuthorityCode()));
		}
	}
}
