package com.mst.security;
import com.mst.configuration.SecurityConfiguration;
import com.mst.security.filter.ReceiptMangerAuthFilter;
import com.mst.repositories.IUserAccountRepository;
import javax.servlet.Filter;
import org.junit.jupiter.api.Test;
import org.springframework.context.annotation.*;
import org.springframework.web.context.support.AnnotationConfigWebApplicationContext;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;
import org.springframework.web.bind.annotation.*;
import org.springframework.mock.web.*;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextImpl;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;
class ApiAuthenticationBoundaryTest {
 @Configuration @EnableWebMvc @Import(SecurityConfiguration.class) static class Config {
  @Bean ReceiptMangerAuthFilter receiptMangerAuthFilter(){return new ReceiptMangerAuthFilter();}
  @Bean LegacyUserPasswordEncoder legacyUserPasswordEncoder(){return new LegacyUserPasswordEncoder();}
  @Bean CustomUserDetailsService customUserDetailsService(){return mock(CustomUserDetailsService.class);}
  @Bean(name="IUserAccountRepository") IUserAccountRepository userRepository(){return mock(IUserAccountRepository.class);}
  // MstUserRight / MstScreen are gone - rights come from the desktop's own tables now, so the
  // two repositories that used to be mocked here no longer exist. What CustomUserDetailsService
  // needs instead is DesktopScreenRightsService, and SecurityConfiguration needs the three login
  // beans below. Each mock is still post-processed by Spring, so its own @Autowired fields have
  // to be satisfiable too - hence the small chain of mocks after it.
  @Bean com.mst.security.DesktopScreenRightsService desktopScreenRightsService(){return mock(com.mst.security.DesktopScreenRightsService.class);}
  @Bean com.mst.repositories.IRealScreenDefinitionRepository realScreenDefinitionRepository(){return mock(com.mst.repositories.IRealScreenDefinitionRepository.class);}

  // ${...} in @Value is only resolved when this is present; the new login beans carry defaults
  // (app.login.fallback-to-local, app.login.license-check) and would otherwise fail to create.
  @Bean static org.springframework.context.support.PropertySourcesPlaceholderConfigurer placeholders(){
   return new org.springframework.context.support.PropertySourcesPlaceholderConfigurer();
  }

  @Bean org.springframework.jdbc.core.JdbcTemplate jdbcTemplate(){return mock(org.springframework.jdbc.core.JdbcTemplate.class);}
  @Bean com.mst.security.desktop.DesktopUserAccountDal desktopUserAccountDal(){return mock(com.mst.security.desktop.DesktopUserAccountDal.class);}
  @Bean com.mst.security.desktop.DesktopUserAccountBll desktopUserAccountBll(){return mock(com.mst.security.desktop.DesktopUserAccountBll.class);}
  @Bean com.mst.security.desktop.DesktopLoginContextService desktopLoginContextService(){return mock(com.mst.security.desktop.DesktopLoginContextService.class);}
  @Bean com.mst.security.LoginContextResolver loginContextResolver(){return mock(com.mst.security.LoginContextResolver.class);}
  @Bean com.mst.security.DesktopLoginAuthenticationProvider desktopLoginAuthenticationProvider(){return mock(com.mst.security.DesktopLoginAuthenticationProvider.class);}
  @Bean com.mst.security.LoginContextSuccessHandler loginContextSuccessHandler(){return mock(com.mst.security.LoginContextSuccessHandler.class);}
  // A REAL filter, not a mock: a mocked doFilter would never call the chain and every request in
  // this test would come back empty.
  @Bean com.mst.security.LoginContextFilter loginContextFilter(){return new com.mst.security.LoginContextFilter();}
  @Bean com.mst.repositories.IRealScreenRightRepository realScreenRightRepository(){return mock(com.mst.repositories.IRealScreenRightRepository.class);}
  @Bean com.mst.repositories.IRealUserRightRepository realUserRightRepository(){return mock(com.mst.repositories.IRealUserRightRepository.class);}
  @Bean com.mst.repositories.IRealCompanyRightRepository realCompanyRightRepository(){return mock(com.mst.repositories.IRealCompanyRightRepository.class);}
  @Bean JwtUtils jwtUtils(){return mock(JwtUtils.class);}
  @Bean org.springframework.cache.CacheManager cacheManager(){return new org.springframework.cache.concurrent.ConcurrentMapCacheManager();}
  @Bean Probe probe(){return new Probe();}
 }
 @RestController static class Probe {@RequestMapping({"/api/permission-probe","/modules/permission-probe"}) String probe(){return "handler-reached";}}
 @Test void everyApiVerbRequiresAuthenticationAndSessionStillWorks()throws Exception{
  try(var context=new AnnotationConfigWebApplicationContext()){
   context.setServletContext(new MockServletContext());context.register(Config.class);context.refresh();
   var mvc=MockMvcBuilders.webAppContextSetup(context).addFilters(context.getBean("springSecurityFilterChain",Filter.class)).build();
   for(var method:org.springframework.http.HttpMethod.values())if(method!=org.springframework.http.HttpMethod.TRACE) mvc.perform(request(method,"/api/permission-probe")).andExpect(status().isUnauthorized());
   mvc.perform(get("/modules/permission-probe").accept(org.springframework.http.MediaType.TEXT_HTML)).andExpect(status().is3xxRedirection());
   var session=new MockHttpSession();var security=new SecurityContextImpl(new UsernamePasswordAuthenticationToken("test-user","",java.util.List.of()));session.setAttribute("SPRING_SECURITY_CONTEXT",security);
   mvc.perform(get("/api/permission-probe").session(session)).andExpect(status().isOk()).andExpect(content().string("handler-reached"));
   var jwt=context.getBean(JwtUtils.class);when(jwt.getUserNameFromJwtToken("unit-test-token")).thenReturn("token-user");when(jwt.validateToken("unit-test-token","token-user")).thenReturn(true);
   when(context.getBean(CustomUserDetailsService.class).loadUserByUsername("token-user")).thenReturn(org.springframework.security.core.userdetails.User.withUsername("token-user").password("").authorities("ROLE_USER").build());
   mvc.perform(get("/api/permission-probe").servletPath("/api/permission-probe").header("Authorization","Bearer unit-test-token")).andExpect(status().isOk());
   mvc.perform(get("/api/permission-probe").servletPath("/api/permission-probe").header("Authorization","Bearer invalid-test-token")).andExpect(status().isUnauthorized());
  }
 }
}
