package com.mst.repositories;

import com.mst.controllers.*;
import com.mst.services.*;
import org.junit.jupiter.api.Test;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import static org.mockito.Mockito.mock;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class InventoryReportRouteTest {
    @Test void migratedReportsHaveDistinctRoutesIncludingOldBookmarks() throws Exception {
        var mvc=MockMvcBuilders.standaloneSetup(new MainModulesController(),new InventoryItemLedgerController(mock(InventoryItemLedgerService.class)),new InventoryVehicleTransactionsController(mock(InventoryVehicleTransactionsService.class)),new InventoryTransactionsController(mock(InventoryTransactionsService.class))).build();
        for(String route:new String[]{"/stocks/item_ledger","/stocks/item-ledger"})mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("stocks/item_ledger"));
        for(String route:new String[]{"/stocks/transaction-vehicle-wise","/stocks/stock_movement","/stocks/stock-movement"})mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("stocks/transaction_vehicle_wise"));
        for(String route:new String[]{"/stocks/stock_register","/stocks/stock-register"})mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("stocks/stock_register"));
        for(String route:new String[]{"/stocks/stock_as_on_date","/stocks/stock-as-on-date"})mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("stocks/stock_as_on_date"));
        mvc.perform(get("/inventory/stock-transactions")).andExpect(status().isOk()).andExpect(view().name("inventory/stock_transactions"));
    }
}
