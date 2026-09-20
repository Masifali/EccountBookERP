package com.mst.configuration;

import com.mst.security.filter.ReceiptMangerAuthFilter;
import com.mst.security.LegacyUserPasswordEncoder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.method.configuration.EnableGlobalMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.builders.WebSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.web.util.matcher.AntPathRequestMatcher;


import org.springframework.context.annotation.Lazy;

@Configuration
@EnableWebSecurity
@EnableGlobalMethodSecurity(securedEnabled = true, prePostEnabled = true)
@EnableCaching(proxyTargetClass = true)
//@EnableJpaRepositories(repositoryBaseClass = ExtendedRepositoryImpl.class)
@PropertySource(value = "file:/application.properties", ignoreResourceNotFound = true)
public class SecurityConfiguration extends WebSecurityConfigurerAdapter {

    @Autowired
    @Lazy
    ReceiptMangerAuthFilter receiptMangerAuthFilter;
    @Autowired
    private BCryptPasswordEncoder bCryptPasswordEncoder;
    @Autowired
    private LegacyUserPasswordEncoder legacyUserPasswordEncoder;
    @Autowired
    private UserDetailsService userDetailsService;
    @Autowired
    private com.mst.security.DesktopLoginAuthenticationProvider desktopLoginAuthenticationProvider;
    @Autowired
    private com.mst.security.LoginContextSuccessHandler loginContextSuccessHandler;
    @Autowired
    private com.mst.security.LoginContextFilter loginContextFilter;


    /**
     * Authentication goes through the desktop's own stored procedure.
     *
     * It used to be {@code auth.userDetailsService(...).passwordEncoder(...)}, i.e. Spring's
     * DaoAuthenticationProvider: load the row by user name with a JPA SELECT, compare the
     * password in Java. That applies the password rule and nothing else, while the desktop hands
     * both values to Sp_UserAccount_Login and treats "no row" as a failed login - so every other
     * condition inside that procedure was being skipped on the web.
     *
     * DesktopLoginAuthenticationProvider calls the procedure instead, and still resolves the
     * screen authorities through the same UserDetailsService afterwards. The encoder stays a bean
     * because the provider uses it to encrypt the typed password before the call, exactly as
     * Architecture.BLL.UserAccount.Login does.
     */
    @Override
    protected void configure(AuthenticationManagerBuilder auth)
            throws Exception {
        auth.authenticationProvider(desktopLoginAuthenticationProvider);
    }

    @Override

    protected void configure(HttpSecurity http) throws Exception {
        // Replace the default DENY writer only with the scoped report-frame policy.
        http.headers().frameOptions().disable()
                .addHeaderWriter(new com.mst.security.ReportFrameHeaderWriter());
        http.addFilterBefore(receiptMangerAuthFilter, org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter.class);
        /* After Spring Security has restored the session, so the principal is present. */
        http.addFilterAfter(loginContextFilter, org.springframework.security.web.access.intercept.FilterSecurityInterceptor.class);
        http.exceptionHandling().defaultAuthenticationEntryPointFor(
                new org.springframework.security.web.authentication.HttpStatusEntryPoint(org.springframework.http.HttpStatus.UNAUTHORIZED),
                new AntPathRequestMatcher("/api/**"));
        http
                .authorizeRequests()
                .antMatchers("/login", "/css/**", "/js/**", "/images/**", "/vendors/**", "/build/**", "/.well-known/**").permitAll()
                .anyRequest().authenticated()
                .and()
                .formLogin()
                .loginPage("/login")
                /* LoginNew.cs does not finish at the password: it then works out the company,
                   branch, financial year and application, asking only where there is a choice.
                   The success handler reproduces that and sends the operator either straight to
                   the dashboard or to /login/context. defaultSuccessUrl would jump past it. */
                .successHandler(loginContextSuccessHandler)
                .failureUrl("/login?error=true")
                .permitAll()
                .and()
                .csrf().disable();
    }


    @Override
    public void configure(WebSecurity web) throws Exception {
        web
                .ignoring()
                .antMatchers("/resources/**", "/static/**", "/css/**", "/js/**",
                        "/images/**", "/vendors/**", "/Whastsapp/**", "/build/**", "/.well-known/**");

    }

    /**
     * LoginContextFilter is a @Component extending OncePerRequestFilter, so Spring Boot would ALSO
     * register it as a plain servlet filter - running it twice, and the first time OUTSIDE the
     * security chain, before the SecurityContext has been restored from the session. There it
     * would see no authentication on every request and let everything through, which is harmless
     * but pointless, and the ordering would be a trap for whoever reads it next.
     *
     * Disabled as a servlet filter here, exactly as receiptFilterRegistration does for the same
     * reason; it runs once, inside the chain, via addFilterAfter below.
     */
    @Bean
    public org.springframework.boot.web.servlet.FilterRegistrationBean<com.mst.security.LoginContextFilter> loginContextFilterRegistration() {
        var registration = new org.springframework.boot.web.servlet.FilterRegistrationBean<>(loginContextFilter);
        registration.setEnabled(false);
        return registration;
    }

    @Bean
    public org.springframework.boot.web.servlet.FilterRegistrationBean<ReceiptMangerAuthFilter> receiptFilterRegistration() {
        var registration=new org.springframework.boot.web.servlet.FilterRegistrationBean<>(receiptMangerAuthFilter);
        registration.setEnabled(false); // Run once inside Spring Security, after session restoration.
        return registration;
    }

    @Bean
    public BCryptPasswordEncoder passwordEncoder() {
        BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder();
        return bCryptPasswordEncoder;
    }

    /*  @Bean
      public ReceiptMangerAuthFilter receiptMangerAuthFilter() {

          return new ReceiptMangerAuthFilter();
      }*/




/*    @Bean
    public CompanyService companyService() {

        return new CompanyService();
    }*/

    @Bean
    @Override
    public AuthenticationManager authenticationManagerBean() throws Exception {
        return super.authenticationManagerBean();
    }
}
