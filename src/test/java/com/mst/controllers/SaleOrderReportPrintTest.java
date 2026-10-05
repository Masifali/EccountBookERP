package com.mst.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.ERPPrint.SaleOrderReportPrintController;
import com.mst.models.*;
import com.mst.repositories.ICompanyRepository;
import com.mst.reports.jasper.ReportTemplateService;
import com.mst.security.CurrentUserContext;
import com.mst.services.SaleOrderReportService;
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

class SaleOrderReportPrintTest {
    @TempDir Path templates;
    final SaleOrderReportService service=mock(SaleOrderReportService.class);
    MockMvc mvc;
    final String body="{\"fromDate\":\"2026-09-01\",\"toDate\":\"2026-10-04\",\"branchIds\":[58],\"status\":\"Open\",\"approval\":\"Approve\",\"categoryId\":3}";
    @BeforeEach void setup() throws Exception {
        var context=mock(CurrentUserContext.class);when(context.currentUserId()).thenReturn(78);when(context.currentCompanyId()).thenReturn(58);
        var companies=mock(ICompanyRepository.class);var company=new Company();company.setCompName("Sample Company");when(companies.findById(58)).thenReturn(Optional.of(company));
        var controller=new SaleOrderReportPrintController(service,companies);ReflectionTestUtils.setField(controller,"currentUserContext",context);ReflectionTestUtils.setField(controller,"reportTemplates",new ReportTemplateService(templates.toString()));
        mvc=MockMvcBuilders.standaloneSetup(controller).build();
        var row=new LinkedHashMap<String,Object>();row.put("DocNo",137);row.put("SupplierName","Sample Customer");row.put("ItemName","Sample Item");row.put("OrderItemQty",16500.0);row.put("DispatchQty",10872.0);row.put("BalQty",5628.0);row.put("Amount",76951875.0);
        when(service.detailRows(any())).thenReturn(List.of(row));
        when(service.summaryRows(any())).thenReturn(List.of(row));
    }
    @Test void allRegisterPrintsProducePdfAndPassTheDisplayedFilter() throws Exception {
        Files.createDirectories(Path.of("target/sale-order-verification"));
        for(String variant:List.of("270","271","271A")){
            var result=mvc.perform(post("/sale/reports/sale-order-report/api/print/"+variant).contentType("application/json").content(body)).andExpect(status().isOk()).andExpect(content().contentType("application/pdf")).andReturn().getResponse();
            assertTrue(new String(result.getContentAsByteArray(),0,4).equals("%PDF"));assertTrue(result.getHeader("Content-Disposition").contains(variant));Files.write(Path.of("target/sale-order-verification/print-"+variant+"-sample.pdf"),result.getContentAsByteArray());
        }
        var filter=org.mockito.ArgumentCaptor.forClass(SaleOrderReportFilter.class);verify(service,times(3)).detailRows(filter.capture());for(var f:filter.getAllValues()){assertEquals(3,f.categoryId());assertEquals(List.of(58),f.branchIds());assertEquals("Open",f.status());}
    }
    @Test void invalidTemplateAndUnauthorizedBranchCannotPrint() throws Exception {
        mvc.perform(post("/sale/reports/sale-order-report/api/print/999").contentType("application/json").content(body)).andExpect(status().isBadRequest());verifyNoInteractions(service);
        when(service.detailRows(any())).thenThrow(new AccessDeniedException("Branch not allocated"));mvc.perform(post("/sale/reports/sale-order-report/api/print/270").contentType("application/json").content(body)).andExpect(status().isForbidden());
    }
    @Test void everySummaryActivityUsesItsDesktopPrintTemplate() throws Exception {
        var mapper=new ObjectMapper();
        for(String activity:SaleOrderReportColumns.ACTIVITIES){
            var filter=mapper.readTree(body);((com.fasterxml.jackson.databind.node.ObjectNode)filter).put("activity",activity);
            var result=mvc.perform(post("/sale/reports/sale-order-report/api/print/summary").contentType("application/json").content(mapper.writeValueAsBytes(filter)))
                    .andExpect(status().isOk()).andExpect(content().contentType("application/pdf")).andReturn().getResponse();
            assertEquals("%PDF",new String(result.getContentAsByteArray(),0,4));
            String fileName=com.mst.reports.jasper.CrystalJasperPrinter.stem(SaleOrderReportColumns.summaryPrintTemplate(activity))+".pdf";
            assertEquals("inline; filename=\""+fileName+"\"",result.getHeader("Content-Disposition"));
        }
        var capture=org.mockito.ArgumentCaptor.forClass(SaleOrderReportFilter.class);verify(service,times(15)).summaryRows(capture.capture());
        assertEquals(SaleOrderReportColumns.ACTIVITIES,capture.getAllValues().stream().map(SaleOrderReportFilter::activity).toList());
        verify(service,never()).detailRows(any());
    }
    @Test void invalidOrUnauthorizedSummaryCannotPrint() throws Exception {
        mvc.perform(post("/sale/reports/sale-order-report/api/print/summary").contentType("application/json").content(body)).andExpect(status().isBadRequest());verifyNoInteractions(service);
        when(service.summaryRows(any())).thenThrow(new AccessDeniedException("Branch not allocated"));
        mvc.perform(post("/sale/reports/sale-order-report/api/print/summary").contentType("application/json").content(body.replace("\"categoryId\":3","\"categoryId\":3,\"activity\":\"Order Register\""))).andExpect(status().isForbidden());
    }
}
