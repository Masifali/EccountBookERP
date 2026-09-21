package com.mst.security;

import com.mst.security.desktop.DesktopUserAccountBll;
import com.mst.security.desktop.DesktopUserAccountDal;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.AuthenticationServiceException;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.DisabledException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Component;

import java.util.Map;

/**
 * Authentication, done the way the desktop does it: through the ported BLL, which calls the
 * procedure.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT CHANGED, AND WHY IT MATTERS
 * ---------------------------------------------------------------------------------------------
 * Before this class, Spring's DaoAuthenticationProvider loaded the row with
 * {@code findByUserName} - a plain SELECT on dbo.UserAccount - and compared the password in Java.
 * That applies the password rule and nothing else. The desktop hands both values to
 * Sp_UserAccount_Login and treats "no row" as a failed login, so every other condition inside
 * that procedure is part of its answer.
 *
 * {@link DesktopUserAccountBll#login} is the ported Architecture.BLL.UserAccount.Login: it
 * encrypts the typed password, calls the procedure, and returns the row plus the configuration
 * and profile the desktop loads at the same moment. This class is only the Spring Security
 * adapter around it. The UserDetailsService is still used, but for what it is good at - resolving
 * the screen authorities that gate the menu - and only AFTER the BLL has said yes.
 *
 * ---------------------------------------------------------------------------------------------
 * THE PASSWORD NEVER TRAVELS IN CLEAR, AND IS NEVER LOGGED
 * ---------------------------------------------------------------------------------------------
 * The BLL encrypts it with {@link LegacyUserPasswordEncoder} - this port's reproduction of
 * Encryption.UserPasswordEncryptString - and binds it as a parameter rather than concatenating
 * it. Nothing here writes either the raw or the encrypted value to a log.
 *
 * ---------------------------------------------------------------------------------------------
 * A PROCEDURE FAILURE IS NOT A FAILED LOGIN
 * ---------------------------------------------------------------------------------------------
 * "No row" means bad credentials. A SQL error - wrong name, changed signature, database down -
 * means the question was never answered, and quietly falling back to the old in-Java comparison
 * would be a silent downgrade of authentication to the weaker rule. So by default it fails the
 * login with a distinct message and a loud log.
 *
 * If that ever locks everyone out of a running system, {@code app.login.fallback-to-local=true}
 * restores the previous behaviour for exactly as long as it takes to fix the procedure. It is a
 * deliberate, visible switch, off by default, and it logs a warning on every use - not a hidden
 * safety net.
 *
 * ---------------------------------------------------------------------------------------------
 * OTP IS NOT REQUESTED HERE
 * ---------------------------------------------------------------------------------------------
 * The BLL is called with {@code otp = false}, so the two-way-authentication branch is skipped.
 * Nothing in this port can verify a code yet, and generating and mailing one that no screen ever
 * checks would be worse than not sending it. It weakens nothing the procedure enforces. When an
 * OTP screen exists, this becomes {@code otp = true} and the result's authenticationCode and
 * authenticationEnabledForUser drive it.
 */
@Component
public class DesktopLoginAuthenticationProvider implements AuthenticationProvider {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopLoginAuthenticationProvider.class);

    @Autowired private DesktopUserAccountBll userAccountBll;
    @Autowired private LegacyUserPasswordEncoder passwordEncoder;
    @Autowired private UserDetailsService userDetailsService;

    /** Off by default. See the class note - this is an emergency switch, not a fallback. */
    @Value("${app.login.fallback-to-local:false}")
    private boolean fallbackToLocal;

    @Override
    public Authentication authenticate(Authentication authentication) throws AuthenticationException {
        String userName = authentication.getName();
        Object credentials = authentication.getCredentials();
        String rawPassword = credentials == null ? null : credentials.toString();

        if (userName == null || userName.trim().isEmpty() || rawPassword == null || rawPassword.isEmpty()) {
            throw new BadCredentialsException(DesktopUserAccountBll.INVALID_LOGIN);
        }
        userName = userName.trim();

        DesktopUserAccountBll.LoginResult result;
        try {
            /* appId, validateVersion, appVersion and deviceId are all omitted - a browser has
               none of them, and the BLL omits each parameter when its value is absent, exactly
               as the C# does. Device-bound and version-gated accounts are therefore not enforced
               on the web; that is unchanged from before and is recorded, not hidden. */
            result = userAccountBll.login(userName, rawPassword, 0, 0, 0, null, false);
        } catch (IllegalArgumentException badCredentials) {
            /* The BLL's own "Invalid UserId or Password" - a real answer, not a failure. */
            throw new BadCredentialsException(badCredentials.getMessage());
        } catch (Exception e) {
            LOG.error("Sp_UserAccount_Login failed - login cannot be verified. "
                    + "Set app.login.fallback-to-local=true only as a temporary measure.", e);
            if (!fallbackToLocal) {
                throw new AuthenticationServiceException(
                        "Login verification is unavailable. Please contact your administrator.");
            }
            LOG.warn("app.login.fallback-to-local is ON - authenticating {} against the "
                    + "UserAccount row instead of the procedure. This applies the password rule "
                    + "and nothing else.", userName);
            return localFallback(userName, passwordEncoder.encode(rawPassword));
        }

        /* The procedure has authenticated the user. The authorities still come from the screen
           rights chain, which is unchanged. */
        UserDetails details = loadDetails(userName);
        if (!details.isEnabled()) {
            /* The desktop's IsActive check. Kept even though the procedure very likely applies it
               too - two apps refusing an inactive user is not a conflict. */
            throw new DisabledException("This user is not active");
        }

        Map<String, Object> row = result.user;
        String roleName = DesktopUserAccountDal.stringOf(row, "RoleName");
        int userId = DesktopUserAccountDal.intOf(row, "Id");
        if (roleName == null || roleName.isEmpty()) {
            /* Not fatal: CurrentUserContext falls back to UserGroup.UserGroupRole, which is what
               the port used before. Logged so the gap is visible rather than guessed at. */
            LOG.info("Sp_UserAccount_Login returned no RoleName column; "
                    + "falling back to UserGroup.UserGroupRole for the Admin checks.");
        }

        DesktopUserPrincipal principal =
                DesktopUserPrincipal.from(details, roleName, userId > 0 ? userId : null);
        return new UsernamePasswordAuthenticationToken(principal, null, principal.getAuthorities());
    }

    /** Only reachable with app.login.fallback-to-local=true. The pre-procedure behaviour. */
    private Authentication localFallback(String userName, String encrypted) {
        UserDetails details = loadDetails(userName);
        if (!details.isEnabled()) throw new DisabledException("This user is not active");
        if (details.getPassword() == null || !details.getPassword().equals(encrypted)) {
            throw new BadCredentialsException(DesktopUserAccountBll.INVALID_LOGIN);
        }
        DesktopUserPrincipal principal = DesktopUserPrincipal.from(details, null, null);
        return new UsernamePasswordAuthenticationToken(principal, null, principal.getAuthorities());
    }

    private UserDetails loadDetails(String userName) {
        try {
            return userDetailsService.loadUserByUsername(userName);
        } catch (UsernameNotFoundException e) {
            /* The procedure said the credentials are good but the row is not loadable here. That
               is a data problem, not a wrong password - but it must still not sign anyone in, and
               it must not tell the caller which of the two halves was wrong. */
            LOG.warn("Sp_UserAccount_Login accepted {} but the account could not be loaded", userName, e);
            throw new BadCredentialsException(DesktopUserAccountBll.INVALID_LOGIN);
        }
    }

    @Override
    public boolean supports(Class<?> authentication) {
        return UsernamePasswordAuthenticationToken.class.isAssignableFrom(authentication);
    }
}
