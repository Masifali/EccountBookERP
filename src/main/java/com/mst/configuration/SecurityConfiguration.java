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


    @Override
    protected void configure(AuthenticationManagerBuilder auth)
            throws Exception {
        auth
                .userDetailsService(userDetailsService)
                .passwordEncoder(legacyUserPasswordEncoder);


    }

    @Override

    protected void configure(HttpSecurity http) throws Exception {
        http
                .authorizeRequests()
                .antMatchers("/login", "/css/**", "/js/**", "/images/**", "/vendors/**", "/build/**", "/.well-known/**").permitAll()
                .anyRequest().authenticated()
                .and()
                .formLogin()
                .loginPage("/login")
                .defaultSuccessUrl("/accounts/dashboard", false)
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
                        "/images/**", "/vendors/**", "/Whastsapp/**", "/build/**", "/modules/**", "/.well-known/**")
                .requestMatchers(new org.springframework.security.web.util.matcher.AndRequestMatcher(
                        new AntPathRequestMatcher("/api/**"),
                        new org.springframework.security.web.util.matcher.NegatedRequestMatcher(
                                new AntPathRequestMatcher("/api/accounts/**"))));
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
