package com.mst.repositories;

import com.mst.controllers.DesktopReceivablesController;
import com.mst.services.DesktopReceivablesService;
import org.junit.jupiter.api.Test;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import static org.mockito.Mockito.mock;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class DesktopReportRouteTest {
    @Controller static class GenericReport {
        @GetMapping("/accounts/reports/{report}") public String generic(){return "wrong-report";}
    }
    @Test void desktopReportsTakePrecedenceOverGenericReportRoute() throws Exception {
        var mvc=MockMvcBuilders.standaloneSetup(new DesktopReceivablesController(mock(DesktopReceivablesService.class)),new GenericReport()).build();
        for(String report:new String[]{"receivables-report","payables-report","receivables-by-due-dates","receivables-receipt-schedule"})
            mvc.perform(get("/accounts/reports/"+report)).andExpect(status().isOk()).andExpect(view().name("accounts/reports/desktop_receivables")).andExpect(model().attribute("reportKind",report));
    }
}
