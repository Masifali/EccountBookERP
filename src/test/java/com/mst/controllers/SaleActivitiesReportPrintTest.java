package com.mst.controllers;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.ERPPrint.SaleActivitiesReportPrintController;
import com.mst.models.*;
import com.mst.repositories.ICompanyRepository;
import com.mst.reports.jasper.*;
import com.mst.security.CurrentUserContext;
import com.mst.services.SaleActivitiesReportService;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.io.TempDir;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import java.nio.file.*;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class SaleActivitiesReportPrintTest {
    @TempDir Path templates;MockMvc mvc;final SaleActivitiesReportService service=mock(SaleActivitiesReportService.class);
    @BeforeEach void setup() throws Exception {
        var context=mock(CurrentUserContext.class);when(context.currentUserId()).thenReturn(78);when(context.currentCompanyId()).thenReturn(58);
        var companies=mock(ICompanyRepository.class);var company=new Company();company.setCompName("Test Company");when(companies.findById(58)).thenReturn(Optional.of(company));
        var controller=new SaleActivitiesReportPrintController(service,companies);ReflectionTestUtils.setField(controller,"currentUserContext",context);ReflectionTestUtils.setField(controller,"reportTemplates",new ReportTemplateService(templates.toString()));mvc=MockMvcBuilders.standaloneSetup(controller).build();
        var row=new LinkedHashMap<String,Object>();row.put("ItemName","Sample Item");row.put("QtyOut",100.0);row.put("BillWeightOut",5000.0);row.put("AmountOut",550000.0);row.put("Customer","Sample Customer");row.put("ItemRate",5500.0);row.put("RateCut",0.0);when(service.reportRows(any())).thenReturn(List.of(row));
    }
    String body(String activity) throws Exception{return new ObjectMapper().writeValueAsString(Map.of("activity",activity,"fromDate","2026-09-01","toDate","2026-10-04","branchIds",List.of(58),"itemId",624));}
    @Test void eachDesktopActivityProducesItsPdfWithTheDisplayedFilter() throws Exception {
        for(String activity:SaleActivitiesReportColumns.ACTIVITIES){var response=mvc.perform(post("/sale/reports/sale-invoice-report-with-activities/api/print").contentType("application/json").content(body(activity))).andExpect(status().isOk()).andExpect(content().contentType("application/pdf")).andReturn().getResponse();assertEquals("%PDF",new String(response.getContentAsByteArray(),0,4));assertEquals("inline; filename=\""+CrystalJasperPrinter.stem(SaleActivitiesReportColumns.printTemplate(activity))+".pdf\"",response.getHeader("Content-Disposition"));}
        var capture=org.mockito.ArgumentCaptor.forClass(SaleActivitiesReportFilter.class);verify(service,times(23)).reportRows(capture.capture());assertEquals(SaleActivitiesReportColumns.ACTIVITIES,capture.getAllValues().stream().map(SaleActivitiesReportFilter::activity).toList());assertTrue(capture.getAllValues().stream().allMatch(f->f.itemId()==624&&f.branchIds().equals(List.of(58))));
    }
    @Test void unauthorizedAndEmptyReportsDoNotPrint() throws Exception {
        when(service.reportRows(any())).thenThrow(new AccessDeniedException("Branch not allocated"));mvc.perform(post("/sale/reports/sale-invoice-report-with-activities/api/print").contentType("application/json").content(body("Sales Register"))).andExpect(status().isForbidden());
        reset(service);when(service.reportRows(any())).thenReturn(List.of());mvc.perform(post("/sale/reports/sale-invoice-report-with-activities/api/print").contentType("application/json").content(body("Sales Register"))).andExpect(status().isNotFound());
    }
}
