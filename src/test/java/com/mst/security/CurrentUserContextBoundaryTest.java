package com.mst.security;
import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;
import org.junit.jupiter.api.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.test.util.ReflectionTestUtils;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
class CurrentUserContextBoundaryTest {
 @AfterEach void clear(){SecurityContextHolder.clearContext();}
 private CurrentUserContext context(IUserAccountRepository repo,JdbcTemplate jdbc){var c=new CurrentUserContext();ReflectionTestUtils.setField(c,"userAccountRepository",repo);ReflectionTestUtils.setField(c,"jdbc",jdbc);return c;}
 @Test void missingSessionNeverFallsBackToTenantOne(){var repo=mock(IUserAccountRepository.class);var jdbc=mock(JdbcTemplate.class);var c=context(repo,jdbc);for(java.util.function.IntSupplier getter:java.util.List.<java.util.function.IntSupplier>of(c::currentUserId,c::currentOrganizationId,c::currentCompanyId,c::currentBranchId,c::currentAppId,c::currentFinancialYearId))assertThrows(org.springframework.security.core.AuthenticationException.class,getter::getAsInt);verifyNoInteractions(repo,jdbc);}
 @Test void validContextRetainsRealIds(){var repo=mock(IUserAccountRepository.class);var jdbc=mock(JdbcTemplate.class);var c=context(repo,jdbc);var u=new UserAccount();u.setId(85);u.setOrganizationId(7);u.setCompanyId(78);u.setBranchesId(9);u.setIsActive(true);when(repo.findByUserName("context-test")).thenReturn(u);SecurityContextHolder.getContext().setAuthentication(new UsernamePasswordAuthenticationToken("context-test","unused",java.util.List.of()));assertEquals(85,c.currentUserId());assertEquals(7,c.currentOrganizationId());assertEquals(78,c.currentCompanyId());assertEquals(9,c.currentBranchId());u.setIsActive(false);assertThrows(org.springframework.security.access.AccessDeniedException.class,c::currentCompanyId);}
}
