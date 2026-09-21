package com.mst.security;

import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.Collection;

/**
 * The signed-in principal, carrying the one thing the desktop's login returns that this port had
 * no home for: <b>RoleName</b>.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS
 * ---------------------------------------------------------------------------------------------
 * CommonServices.SetRightsValueInRightsObject (:17565-17645) decides the blanket Admin grant from
 * {@code clsGlobalVariables.UserAccount.RoleName}, a column Sp_UserAccount_Login returns. It is a
 * <i>virtual</i> property on the desktop model - it is not a column of dbo.UserAccount, so a JPA
 * entity mapped to that table can never carry it, and reloading the user by name cannot recover
 * it. It exists only in the login result.
 *
 * Until now the port substituted {@code UserGroup.UserGroupRole} for it. That substitution may
 * well be right - the login procedure probably joins UserGroup - but it was never verified, and
 * it drives whether a user is treated as Admin for Save, Update, Delete, Print and
 * CanView AllRecord. Carrying the real value through the session removes the guess.
 *
 * The principal is the right place for it: it is established once, at authentication, from the
 * procedure's own answer, and it cannot be re-derived later from the database.
 */
public class DesktopUserPrincipal extends User {

    private static final long serialVersionUID = 1L;

    /** RoleName exactly as Sp_UserAccount_Login returned it. May be null if it selects no such column. */
    private final String roleName;

    /** dbo.UserAccount.Id as the procedure returned it - the identity the procedure authenticated. */
    private final Integer userId;

    public DesktopUserPrincipal(String username, String password, boolean enabled,
                                Collection<? extends GrantedAuthority> authorities,
                                String roleName, Integer userId) {
        super(username, password, enabled, true, true, true, authorities);
        this.roleName = roleName;
        this.userId = userId;
    }

    public String getRoleName() { return roleName; }
    public Integer getUserId()  { return userId; }

    /** Copies an existing UserDetails, adding the two login-only values. */
    public static DesktopUserPrincipal from(UserDetails base, String roleName, Integer userId) {
        return new DesktopUserPrincipal(base.getUsername(), base.getPassword(), base.isEnabled(),
                base.getAuthorities(), roleName, userId);
    }
}
