package com.mst.security.filter;


import com.mst.repositories.IUserRepository;
import com.mst.security.CustomUserDetailsService;
import com.mst.security.JwtUtils;
import com.mst.services.UserService;
import io.jsonwebtoken.ExpiredJwtException;
import io.jsonwebtoken.SignatureException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Lazy;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.stereotype.Component;
import org.springframework.util.StringUtils;
import org.springframework.web.filter.OncePerRequestFilter;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

@Component
public class ReceiptMangerAuthFilter extends OncePerRequestFilter {
    /*   ReceiptMangerAuthFilter(HttpServletRequest request,
                               HttpServletResponse response,
                               FilterChain filterChain) {

       }*/

    @Autowired
    @Lazy
    AuthenticationManager authenticationManager;
    @Autowired
    @Lazy
    CustomUserDetailsService customUserDetailsService;
    @Autowired
    private IUserRepository userRepository;
    @Autowired
    private JwtUtils jwtUtils;

    @Override
    protected void doFilterInternal(HttpServletRequest request,
                                    HttpServletResponse response,
                                    FilterChain filterChain) throws ServletException, IOException {

        String username = null;
        String authToken = null;

        try {

            String headerAuth = request.getHeader("Authorization");

            if (headerAuth != null && StringUtils.hasText(headerAuth) && headerAuth.startsWith("Bearer ")) {
                authToken = headerAuth.substring(7);
                if (authToken.equalsIgnoreCase("null")) {
                    filterChain.doFilter(request, response);
                    return;
                }
                try {
                    username = jwtUtils.getUserNameFromJwtToken(authToken);
                } catch (IllegalArgumentException e) {
                    logger.error("an error occured during getting username from token", e);
                } catch (ExpiredJwtException e) {
                    logger.warn("the token is expired and not valid anymore", e);
                } catch (SignatureException e) {
                    logger.error("Authentication Failed. Username or Password not valid.");
                }
            } else {
                logger.info("couldn't find bearer string, will ignore the header");
            }
            // if (username != null && SecurityContextHolder.getContext().getAuthentication() == null) {
            if (username != null) {

                if (jwtUtils.validateToken(authToken, username)) {
                    UserDetails userDetails = customUserDetailsService.loadUserByUsername(username);
                    UsernamePasswordAuthenticationToken authentication = new UsernamePasswordAuthenticationToken(username, userDetails.getPassword(), userDetails.getAuthorities());
                    authentication.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
                    logger.info("authenticated user " + username + ", setting security context");
                    SecurityContextHolder.getContext().setAuthentication(authentication);
                }
            }//03014471796   , 0304-4810169
        } catch (RuntimeException e) {
            filterChain.doFilter(request, response);
            return;
              /*  Map<String, Object> errorDetails = new HashMap<>();
                errorDetails.put("error", ErrorCode.INVALID_TOKEN.getMessage());

                response.setStatus(HttpStatus.UNAUTHORIZED.value());
                response.setContentType(MediaType.APPLICATION_JSON_VALUE);
                ObjectMapper mapper = new ObjectMapper();
                mapper.writeValue(response.getWriter(), errorDetails);
                SecurityContextHolder.getContext().setAuthentication(authentication);*/
        }
        //throw new SmartSalemException(ErrorCode.INVALID_TOKEN);

        filterChain.doFilter(request, response);
    }


    @Override
    protected boolean shouldNotFilter(HttpServletRequest request) {

        return !request.getServletPath().contains("/") || request.getServletPath().contains("/token");
             /*   request.getServletPath().contains("/login") || request.getServletPath().contains("/") || request.getServletPath().contains("/getAll")
                || request.getServletPath().contains("/isTokenValid") || request.getServletPath().contains("/queue/skip")
                || request.getServletPath().contains("/verify-otp") || request.getServletPath().contains("/paymentCapture");*/
    }

    public void getTokenFromRequest(HttpServletRequest request, FilterChain filterChain, HttpServletResponse response) throws ServletException, IOException {

        Authentication authentication = null;
        String jwt = parseJwt(request);

        String username = jwtUtils.getUserNameFromJwtToken(jwt);
        UserDetails userDetails = customUserDetailsService.loadUserByUsername(username);
        filterChain.doFilter(request, response);
       /* authentication = authenticationManager.authenticate(new ReceiptAuthToken(username, userDetails.getPassword()));
        filterChain.doFilter(request, response);
        SecurityContextHolder.getContext().setAuthentication(authentication);*/
        //  SecurityContextHolder.getContext().getAuthentication();

    }

    public String parseJwt(HttpServletRequest request) {


        return null;
    }


}
