package com.mst.security;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.authentication.SimpleUrlAuthenticationSuccessHandler;
import org.springframework.stereotype.Component;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;

/**
 * What LoginNew.cs does the moment the password is accepted: work out the company, branch,
 * financial year and application, asking only where there is a choice.
 *
 * <pre>
 *   everything resolves to one option  ->  straight to /dashboard, no extra screen
 *   anything has more than one option  ->  /login/context, the desktop's "Continue" step
 *   no company / no branch at all      ->  refused, with the desktop's own message
 * </pre>
 *
 * The third case matters: the desktop throws "Company record not found" and leaves the operator on
 * the login form with nothing signed in. Reproduced here by dropping the session - a user with no
 * allocation must not end up half-authenticated with no tenancy, which is the state that makes
 * every later screen fail in a confusing way.
 */
@Component
public class LoginContextSuccessHandler extends SimpleUrlAuthenticationSuccessHandler {

    private static final Logger LOG = LoggerFactory.getLogger(LoginContextSuccessHandler.class);

    @Autowired private LoginContextResolver resolver;
    @Autowired private IUserAccountRepository userAccountRepository;

    @Override
    public void onAuthenticationSuccess(HttpServletRequest request, HttpServletResponse response,
                                        Authentication authentication)
            throws IOException, ServletException {

        UserAccount user = userAccountRepository.findByUserName(authentication.getName());
        if (user == null) {
            /* The provider only reaches here after the procedure accepted the credentials, so this
               is a data problem rather than a wrong password - but it still must not sign anyone
               in with no identity behind them. */
            LOG.warn("Authenticated {} has no UserAccount row", authentication.getName());
            fail(request, response, "Invalid UserId or Password");
            return;
        }

        LoginContext context;
        try {
            context = resolver.autoResolve(user);
        } catch (IllegalStateException e) {
            /* "Company record not found" / "Branch record not found", verbatim. */
            fail(request, response, e.getMessage());
            return;
        } catch (Exception e) {
            LOG.error("Could not resolve the login context for {}", authentication.getName(), e);
            fail(request, response, "Could not load your company and branch. Please try again.");
            return;
        }

        LoginContext.store(request, context);

        if (!context.isComplete()) {
            getRedirectStrategy().sendRedirect(request, response, "/login/context");
            return;
        }

        try {
            resolver.licenseCheck(user, context, request);
        } catch (RuntimeException e) {
            /* The desktop refuses the login on an expired or missing licence, with its own text. */
            fail(request, response, e.getMessage());
            return;
        }

        getRedirectStrategy().sendRedirect(request, response, "/modules");
    }

    /** Back to the login form with the desktop's message, and nothing left signed in. */
    private void fail(HttpServletRequest request, HttpServletResponse response, String message)
            throws IOException {
        SecurityContextHolder.clearContext();
        HttpSession session = request.getSession(false);
        if (session != null) session.invalidate();
        String encoded = URLEncoder.encode(message == null ? "" : message, StandardCharsets.UTF_8.name());
        getRedirectStrategy().sendRedirect(request, response, "/login?error=true&message=" + encoded);
    }
}
