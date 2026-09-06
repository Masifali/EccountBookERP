package com.mst.configuration;

import com.mst.security.filter.ReceiptMangerAuthFilter;
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
    private UserDetailsService userDetailsService;


    @Override
    protected void configure(AuthenticationManagerBuilder auth)
            throws Exception {
        auth
                .userDetailsService(userDetailsService)
                .passwordEncoder(bCryptPasswordEncoder);


    }

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http
                .csrf().disable() // 👈 DISABLE CSRF
                .authorizeRequests()
                .antMatchers("/item/**").permitAll()
                .antMatchers("/bar/**").permitAll()
                .antMatchers("/sp/**").permitAll()
                .antMatchers("/dashbord/**").permitAll()
                .antMatchers("/monshi/**").permitAll()
                .antMatchers("/thekedar/**").permitAll()
                .antMatchers("/activity/**").permitAll()
                .antMatchers("/travel/**").permitAll()
                .antMatchers("/location/**").permitAll()
                .antMatchers("/vouchers/**").permitAll()
                .antMatchers("/payables/**").permitAll()
                .antMatchers("/receivables/**").permitAll()
                .antMatchers("/reports/**").permitAll()
                .antMatchers("/user/**").permitAll()
                .antMatchers("/user-image/**").permitAll()
                .antMatchers("/app/**").permitAll()
                .antMatchers("/auth/**").permitAll()
                .antMatchers("/accounts/**").permitAll()
                .antMatchers("/bankAccounts/**").permitAll()
                .antMatchers("/bank/**").permitAll()
                .antMatchers("/add_or_update_bank_account/**").permitAll()
                .antMatchers("/app/getProfitability").permitAll()
                .antMatchers("auth/getToken").permitAll()
                .antMatchers("/stock/**").permitAll()
                .antMatchers("app/isTokenValid").permitAll()
                .antMatchers("/app/productionVoucherCodes").permitAll()
                .antMatchers("/utilities/**").permitAll()
                .antMatchers("/loading/**").permitAll()
                .antMatchers("/order/**").permitAll()
                .antMatchers("/login").permitAll()
                .antMatchers("/kanta/**").permitAll()
                .antMatchers("/combine-voucher-list").permitAll()
                .antMatchers("/stock/mill-rates").permitAll()
                .anyRequest().authenticated()
                .and()
                .formLogin()
                .loginPage("/login").failureUrl("/login?error=true")
                .defaultSuccessUrl("/index")
                .usernameParameter("username")
                .passwordParameter("password")
                .and()
                .logout()
                .logoutRequestMatcher(new AntPathRequestMatcher("/logout"));
    }


    @Override
    public void configure(WebSecurity web) throws Exception {
        web
                .ignoring()
                .antMatchers("/resources/**", "/static/**", "/css/**", "/js/**", "/images/**", "/vendors/**", "/Whastsapp/**", "/build/**");
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