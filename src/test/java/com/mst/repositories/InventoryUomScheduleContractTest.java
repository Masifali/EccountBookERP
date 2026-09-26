package com.mst.repositories;

import com.mst.controllers.*;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryUomScheduleRequest;
import com.mst.security.*;
import com.mst.services.InventoryUomScheduleService;
import java.math.BigDecimal;
import org.junit.jupiter.api.Test;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class InventoryUomScheduleContractTest {
    @Test void validationMatchesDesktop(){var r=new InventoryUomScheduleRequest();assertEquals("Please Select Item",assertThrows(IllegalArgumentException.class,()->InventoryUomScheduleService.validate(r)).getMessage());r.itemId=1;assertEquals("Please Select Schedule Unit",assertThrows(IllegalArgumentException.class,()->InventoryUomScheduleService.validate(r)).getMessage());r.scheduleUnitId=1;r.equivalent=BigDecimal.ONE;assertEquals("Please Insert QtyEquivalent",assertThrows(IllegalArgumentException.class,()->InventoryUomScheduleService.validate(r)).getMessage());r.qtyEquivalent=BigDecimal.ONE;assertDoesNotThrow(()->InventoryUomScheduleService.validate(r));}
    @Test void deniedUpdatesNeverTouchDatabase(){var repo=mock(InventoryUomScheduleRepository.class);var ctx=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var user=new UserAccount();when(ctx.requireAccountingUser()).thenReturn(user);doThrow(new AccessDeniedException("Denied")).when(rights).require(user,115,"View");var service=new InventoryUomScheduleService(repo,ctx,rights);var r=new InventoryUomScheduleRequest();r.id=1;assertThrows(AccessDeniedException.class,()->service.save(r));verifyNoInteractions(repo);}
    @Test void viewGrantEnablesDesktopActions(){var repo=mock(InventoryUomScheduleRepository.class);var ctx=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var user=new UserAccount();when(ctx.requireAccountingUser()).thenReturn(user);var service=new InventoryUomScheduleService(repo,ctx,rights);assertEquals(java.util.Map.of("Save",true,"Update",true),service.lookups().get("permissions"));verify(rights).require(user,115,"View");verifyNoMoreInteractions(rights);}
    @Test void distinctRoutesAndActualFailureStatus() throws Exception {var service=mock(InventoryUomScheduleService.class);var mvc=MockMvcBuilders.standaloneSetup(new InventoryUomScheduleController(service),new InventoryManagementRestController(),new InventoryModuleViewController()).build();for(String path:new String[]{"/inventory/item_uom_schedule","/inventory/item-uom-schedule"})mvc.perform(get(path)).andExpect(status().isOk()).andExpect(view().name("inventory/item_uom_schedule"));when(service.save(any())).thenThrow(new IllegalArgumentException("Please Select Item"));mvc.perform(post("/api/inventory/uom-schedules/save").contentType("application/json").content("{}")).andExpect(status().isBadRequest()).andExpect(jsonPath("$.message").value("Please Select Item"));}
}
