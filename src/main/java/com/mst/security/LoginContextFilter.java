package com.mst.security;

import org.springframework.core.annotation.Order;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

/**
 * Keeps a half-finished login from wandering into the app.
 *
 * On the desktop the login form simply does not go away until a company and a branch are chosen -
 * there is nothing else on screen to click. A browser has an address bar, so the same rule has to
 * be enforced rather than implied: while the session's {@link LoginContext} is missing or
 * incomplete, every page bounces back to /login/context.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT IS DELIBERATELY NOT BLOCKED
 * ---------------------------------------------------------------------------------------------
 *   - anything not authenticated - Spring Security already handles that
 *   - a principal this port's form login did not create, i.e. not a {@link DesktopUserPrincipal}.
 *     The receipt-manager bearer filter sets the SecurityContext directly and has no browser
 *     session to hold a context in; bouncing it to an HTML page would break those clients.
 *   - /login, /logout, /login/context itself and static assets, or the redirect would loop
 *   - /api/** - an API caller gets 401/403 from the existing chain rather than an HTML redirect
 *     it cannot follow
 */
@Component
@Order(org.springframework.core.Ordered.LOWEST_PRECEDENCE - 100)
public class LoginContextFilter extends OncePerRequestFilter {

    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response,
                                    FilterChain chain) throws ServletException, IOException {

        if (isExempt(request)) { chain.doFilter(request, response); return; }

        Authentication auth = SecurityContextHolder.getContext().getAuthentication();
        if (auth == null || !auth.isAuthenticated()
                || auth instanceof org.springframework.security.authentication.AnonymousAuthenticationToken
                || !(auth.getPrincipal() instanceof DesktopUserPrincipal)) {
            chain.doFilter(request, response);
            return;
        }

        LoginContext context = LoginContext.of(request, auth.getName());
        if (context != null && context.isComplete()) {
            chain.doFilter(request, response);
            return;
        }
        response.sendRedirect(request.getContextPath() + "/login/context");
    }

    private static boolean isExempt(HttpServletRequest request) {
        String path = request.getServletPath();
        if (path == null) return true;
        return path.startsWith("/login")
            || path.startsWith("/logout")
            || path.startsWith("/api/")
            || path.startsWith("/css/")   || path.startsWith("/js/")
            || path.startsWith("/images/")|| path.startsWith("/vendors/")
            || path.startsWith("/build/") || path.startsWith("/.well-known/")
            || path.startsWith("/static/")|| path.startsWith("/resources/");
    }
}
