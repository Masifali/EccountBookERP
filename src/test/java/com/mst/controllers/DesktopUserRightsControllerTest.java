package com.mst.controllers;
import com.mst.services.DesktopUserRightsService;
import com.mst.models.dto.DesktopUserRightsChange;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpSession;
import org.springframework.security.access.AccessDeniedException;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
class DesktopUserRightsControllerTest {
 @Test void pageHttpRequestsRejectNonAdministrator() throws Exception{var service=mock(DesktopUserRightsService.class);when(service.admin()).thenThrow(new AccessDeniedException("Admin required"));var mvc=org.springframework.test.web.servlet.setup.MockMvcBuilders.standaloneSetup(new DesktopUserRightsController(service)).build();for(String path:List.of("/user-management/rights","/configurations/user-rights"))mvc.perform(org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get(path)).andExpect(org.springframework.test.web.servlet.result.MockMvcResultMatchers.status().isForbidden());}
 @Test void pageRejectsNonAdministrator(){var service=mock(DesktopUserRightsService.class);when(service.admin()).thenThrow(new AccessDeniedException("Admin required"));var controller=new DesktopUserRightsController(service);assertThrows(AccessDeniedException.class,controller::page);}
 @Test void administratorCanOpenPage(){var service=mock(DesktopUserRightsService.class);var controller=new DesktopUserRightsController(service);assertEquals("userAccounts/desktop_rights",controller.page());verify(service).admin();}
 @Test void nonAdminCannotReadOrWriteRights(){var context=mock(com.mst.security.CurrentUserContext.class);var repo=mock(com.mst.repositories.DesktopUserRightsRepository.class);var actor=new com.mst.models.UserAccount();when(context.requireAccountingUser()).thenReturn(actor);when(repo.admin(actor)).thenReturn(false);var service=new DesktopUserRightsService(context,repo);assertFalse(service.canManage());assertThrows(AccessDeniedException.class,service::lookups);assertThrows(AccessDeniedException.class,()->service.allocation(new DesktopUserRightsChange(),true,false));verify(repo,never()).companies(any());verify(repo,never()).allocate(any(),any());}
 @Test void writesRequireSessionBoundToken(){var service=mock(DesktopUserRightsService.class);var controller=new DesktopUserRightsController(service);var session=new MockHttpSession();var change=new DesktopUserRightsChange();assertThrows(AccessDeniedException.class,()->controller.write("allocate",change,"forged",session));verifyNoInteractions(service);when(service.lookups()).thenReturn(Map.of("companyId",78));String token=(String)controller.lookups(session).get("token");assertNotNull(token);assertEquals(token,controller.lookups(session).get("token"));assertThrows(AccessDeniedException.class,()->controller.write("allocate",change,token,new MockHttpSession()));verify(service,never()).allocation(any(),anyBoolean(),anyBoolean());controller.write("allocate",change,token,session);verify(service).allocation(change,true,false);}
}
