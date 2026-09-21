package com.mst.repositories;

import com.mst.controllers.*;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.security.*;
import com.mst.services.InventoryPosItemService;
import org.junit.jupiter.api.Test;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class InventoryPosItemContractTest {
    @Test void deniedSaveDoesNotAccessOrWriteDatabase(){var repo=mock(InventoryPosItemRepository.class);var writer=mock(InventoryPosItemWriter.class);var context=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var u=new UserAccount();when(context.requireAccountingUser()).thenReturn(u);doThrow(new AccessDeniedException("Denied")).when(rights).require(u,106,"Save");var service=new InventoryPosItemService(repo,writer,mock(InventoryOpeningRepository.class),context,rights,mock(com.mst.services.InventoryPosFileService.class));assertThrows(AccessDeniedException.class,()->service.save(new InventoryPosItemRequest()));verifyNoInteractions(repo,writer);}
    @Test void dedicatedPosRoutesAndDatabaseFailureStatus() throws Exception {var service=mock(InventoryPosItemService.class);var mvc=MockMvcBuilders.standaloneSetup(new InventoryPosItemController(service),new InventoryModuleViewController()).build();for(String path:new String[]{"/inventory/pos-define-item","/inventory/pos_define_item"})mvc.perform(get(path)).andExpect(status().isOk()).andExpect(view().name("inventory/pos_item"));when(service.save(any())).thenThrow(new org.springframework.dao.DataIntegrityViolationException("Duplicate item"));mvc.perform(post("/api/inventory/pos-items/save").contentType("application/json").content("{}")).andExpect(status().isConflict()).andExpect(jsonPath("$.message").value("Duplicate item"));}
    @Test void barcodeValidationAndEncoding(){var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(new UserAccount());var service=new InventoryPosItemService(mock(InventoryPosItemRepository.class),mock(InventoryPosItemWriter.class),mock(InventoryOpeningRepository.class),context,mock(DesktopReportRights.class),mock(com.mst.services.InventoryPosFileService.class));assertThrows(IllegalArgumentException.class,()->service.barcode(""));assertThrows(IllegalArgumentException.class,()->service.barcode("\u0000"));var encoded=service.barcode("1234567890");assertEquals("1234567890",encoded.get("code"));assertFalse(((java.util.List<?>)encoded.get("bars")).isEmpty());}
}
