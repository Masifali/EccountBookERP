package com.mst.repositories;

import com.mst.controllers.*;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest;
import com.mst.security.*;
import com.mst.services.InventoryOpeningService;
import java.math.BigDecimal;
import org.junit.jupiter.api.Test;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class InventoryOpeningContractTest {
    @Test void requiredControlsHaveDesktopValidationOrder(){var r=new InventoryOpeningRequest();assertEquals("Project Field Required",assertThrows(IllegalArgumentException.class,()->InventoryOpeningService.validate(r)).getMessage());r.projectsId=1;assertEquals("WareHouseName Field Required",assertThrows(IllegalArgumentException.class,()->InventoryOpeningService.validate(r)).getMessage());r.warehouseId=r.itemId=r.itemUomSch=r.jobLotId=r.stockCrGLAcId=r.rateUomSch=r.packingTypeId=1;r.cropYear="fixture";r.qty=r.weightKgs=r.itemRate=r.itemAmount=BigDecimal.ONE;r.transactionType="Stocks";assertDoesNotThrow(()->InventoryOpeningService.validate(r));r.itemAmount=BigDecimal.ZERO;assertEquals("Amount Field Required",assertThrows(IllegalArgumentException.class,()->InventoryOpeningService.validate(r)).getMessage());}
    @Test void deniedUpdateNeverTouchesTheDatabase(){var repo=mock(InventoryOpeningRepository.class);var writer=mock(InventoryOpeningWriter.class);var ctx=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var u=new UserAccount();when(ctx.requireAccountingUser()).thenReturn(u);doThrow(new AccessDeniedException("Denied")).when(rights).require(u,92,"Update");var service=new InventoryOpeningService(repo,writer,ctx,rights,mock(com.mst.services.InventoryOpeningAttachmentService.class));var request=new InventoryOpeningRequest();request.id=42;assertThrows(AccessDeniedException.class,()->service.save(request));verifyNoInteractions(repo,writer);}
    @Test void openingRoutesUseDedicatedFormAndValidationIsJson() throws Exception {var service=mock(InventoryOpeningService.class);var mvc=MockMvcBuilders.standaloneSetup(new MainModulesController(),new InventoryOpeningController(service)).build();for(String route:new String[]{"/stocks/stock_opening_form","/stocks/stock-opening-form"})mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("stocks/opening_stock"));when(service.save(any())).thenThrow(new IllegalArgumentException("Project Field Required"));mvc.perform(post("/api/inventory/opening-stock").contentType("application/json").content("{}" )).andExpect(status().isBadRequest()).andExpect(jsonPath("$.message").value("Project Field Required"));}
}
