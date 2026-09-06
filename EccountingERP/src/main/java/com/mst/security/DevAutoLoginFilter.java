package com.mst.security;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

/**
 * TEMPORARY DEV-ONLY LOGIN BYPASS - requested explicitly so the main window/menu can
 * be checked while the real rights model is still being ported (see the note below).
 *
 * When app.security.bypass-login=true, every request that doesn't already have an
 * Authentication gets one auto-injected here, as user "admin" with every authority
 * in SidebarAuthorities.ALL - so hitting /index directly works with no login screen,
 * and every menu item renders (fixed_sidebar.html's hasAuthority(...) checks all see
 * a real, authenticated principal).
 *
 * WHY THIS EXISTS RIGHT NOW: CustomUserDetailsService currently decides "sees every
 * menu item" by a hardcoded check - UserAccount.userGroup.userGroupRole equals
 * "ADMIN" - but the real desktop app does NOT work that way. Searching the real
 * decompiled source confirms UserGroupRole is never read anywhere in
 * Architecture.BLL/DAL - it's an unused column. The real desktop's menu/rights are
 * entirely DB-driven, through a bigger real subsystem: ScreenDefinition,
 * ScreenRights, tblUserRights, SreenRightsUserWise, CompanyRights and AppMenu.cs (the
 * BLL that actually assembles a logged-in user's menu tree from those tables - every
 * user, including whoever administers the system, only sees what a rights row
 * explicitly grants them). Porting that correctly is a real, separate task -
 * this filter is a stand-in so login/rights issues don't block checking on other
 * screens in the meantime.
 *
 * TO TURN THIS OFF: set app.security.bypass-login=false in application.properties
 * (or delete this class) once real login / the real rights model is ready again.
 */
@Component
public class DevAutoLoginFilter extends OncePerRequestFilter {

	@Value("${app.security.bypass-login:false}")
	private boolean bypassLogin;

	@Override
	protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain)
			throws ServletException, IOException {
		if (bypassLogin && SecurityContextHolder.getContext().getAuthentication() == null) {
			List<GrantedAuthority> authorities = new ArrayList<>();
			authorities.add(new SimpleGrantedAuthority("ROLE_ADMIN"));
			for (String code : SidebarAuthorities.ALL) {
				authorities.add(new SimpleGrantedAuthority(code));
			}
			// fixed_sidebar.html calls #authentication.getPrincipal().getUsername() -
			// same as real Spring Security login, the principal must be a UserDetails
			// (or at least something with getUsername()), not a bare String.
			org.springframework.security.core.userdetails.User principal =
					new org.springframework.security.core.userdetails.User("admin", "", authorities);
			UsernamePasswordAuthenticationToken auth =
					new UsernamePasswordAuthenticationToken(principal, null, authorities);
			SecurityContextHolder.getContext().setAuthentication(auth);
		}
		filterChain.doFilter(request, response);
	}
}
