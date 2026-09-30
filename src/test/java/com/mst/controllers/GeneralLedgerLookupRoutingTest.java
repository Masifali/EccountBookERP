package com.mst.controllers;
import com.mst.services.AccountsReportService;
import org.junit.jupiter.api.Test;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.ui.ExtendedModelMap;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
class GeneralLedgerLookupRoutingTest {
 @Test void ledgerAliasesLoadOnlyTheFiltersTheyRender() {
  for(String route: new String[]{"general-ledger","general_ledger"}) {
   var service=mock(AccountsReportService.class);
   var c=new AccountsModuleViewController();
   ReflectionTestUtils.setField(c,"accountsReportService",service);
   var model=new ExtendedModelMap();
   assertEquals("accounts/reports/general_ledger",c.getReportModule(route,20661,"2026-09-01","2026-09-26",model));
   assertEquals(20661,model.get("selectedAccountId"));
   assertEquals("2026-09-01",model.get("selectedFromDate"));
   verify(service).getAllDetailAccounts();verify(service).getDateTypes();
   verifyNoMoreInteractions(service);
  }
 }
}
