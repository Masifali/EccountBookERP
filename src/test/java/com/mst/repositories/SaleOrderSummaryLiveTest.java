package com.mst.repositories;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.models.*;
import com.mst.security.*;
import com.mst.services.SaleOrderReportService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import java.util.concurrent.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

/** Opt-in database verification. Only reads business data and writes connection-local temporary tables. */
@EnabledIfSystemProperty(named="sale.report.live",matches="true")
class SaleOrderSummaryLiveTest {
    SaleOrderReportFilter filter(int branch,String activity,int docNo,boolean skipZero){
        return new SaleOrderReportFilter(LocalDate.of(2026,9,1),LocalDate.of(2026,10,4),docNo,docNo,
                0,0,0,0,0,0,0,0,0,"Open","Approve",false,List.of(branch),0,null,0,0,0,activity,skipZero);
    }
    @SuppressWarnings("unchecked") List<Map<String,Object>> rows(Map<String,Object> result){return (List<Map<String,Object>>)result.get("rows");}
    double value(Map<String,Object> row,String field){return row.get(field) instanceof Number number?number.doubleValue():0;}
    double total(List<Map<String,Object>> rows,String field){return rows.stream().mapToDouble(row->value(row,field)).sum();}
    @Test void nativeSummaryCalculationsAndConcurrentReportsStayIsolated() throws Exception {
        var config=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application-local.properties"))){config.load(in);}
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(config.getProperty("spring.datasource.url"),config.getProperty("spring.datasource.username"),config.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        var account=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId,AppId FROM dbo.UserAccount WHERE UserName=?","numan");
        var user=new UserAccount();user.setId(((Number)account.get("ID")).intValue());user.setOrganizationId(((Number)account.get("OrganizationId")).intValue());user.setCompanyId(((Number)account.get("CompanyId")).intValue());user.setBranchesId(((Number)account.get("BranchesId")).intValue());user.setAppId(((Number)account.get("AppId")).intValue());
        var repository=new SaleOrderReportRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(user);
        var service=new SaleOrderReportService(repository,context,mock(DesktopReportRights.class));
        var before=jdbc.queryForMap("SELECT COUNT_BIG(*) AS Rows, CHECKSUM_AGG(BINARY_CHECKSUM(*)) AS Checksum FROM dbo.SaleOrderRegisterTemp");
        var path=Path.of("target/sale-order-verification");Files.createDirectories(path);var mapper=new ObjectMapper().findAndRegisterModules();
        var manifest=new LinkedHashMap<String,String>();var counts=new LinkedHashMap<String,Integer>();List<Map<String,Object>> register=List.of();
        for(int index=0;index<SaleOrderReportColumns.ACTIVITIES.size();index++){
            String activity=SaleOrderReportColumns.ACTIVITIES.get(index);var result=service.summary(filter(user.getBranchesId(),activity,0,false));var rows=rows(result);
            if(index==0){assertFalse(rows.isEmpty());register=rows;}
            assertEquals(SaleOrderReportColumns.summaryPrintLabels().get(activity),result.get("printLabel"));
            for(var row:rows){
                assertEquals(value(row,"OrderQty")-value(row,"DispatchedQty"),value(row,"BalQty"),.01,activity);
                assertEquals(value(row,"OrderWeight")-value(row,"DispatchedWeight"),value(row,"BalWeight"),.01,activity);
                assertEquals(value(row,"OrderAmount")-value(row,"DispatchedAmount"),value(row,"BalAmount"),.01,activity);
            }
            // Reference-party-only activities intentionally use the native inner join (unassigned parties excluded).
            if(!activity.startsWith("Order Summary By ReferenceParty"))for(String field:List.of("OrderQty","DispatchedQty","BalQty","OrderWeight","DispatchedWeight","BalWeight","OrderAmount","DispatchedAmount","BalAmount"))
                assertEquals(total(register,field),total(rows,field),.1,activity+" "+field);
            String file="summary-"+index+".json";manifest.put(activity,file);counts.put(activity,rows.size());Files.writeString(path.resolve(file),mapper.writeValueAsString(result));
        }
        var detail=repository.detail(user,filter(user.getBranchesId(),"Order Register",0,false),","+user.getBranchesId());
        assertEquals(detail.size(),register.size());assertEquals(total(detail,"OrderItemQty"),total(register,"OrderQty"),.01);assertEquals(total(detail,"NetWeight"),total(register,"OrderWeight"),.01);assertEquals(total(detail,"Amount"),total(register,"OrderAmount"),.1);
        var skip=rows(service.summary(filter(user.getBranchesId(),"Order Register",0,true)));assertEquals(register.stream().filter(row->value(row,"BalWeight")>0).count(),skip.size());
        int[] docs=register.stream().mapToInt(row->((Number)row.get("DocNo")).intValue()).distinct().limit(2).toArray();assertEquals(2,docs.length);
        var executor=Executors.newFixedThreadPool(2);
        try {
            var first=executor.submit(()->rows(service.summary(filter(user.getBranchesId(),"Order Register",docs[0],false))));
            var second=executor.submit(()->rows(service.summary(filter(user.getBranchesId(),"Order Register",docs[1],false))));
            for(int index=0;index<2;index++){int doc=docs[index];var actual=(index==0?first:second).get(90,TimeUnit.SECONDS);assertFalse(actual.isEmpty());assertTrue(actual.stream().allMatch(row->value(row,"DocNo")==doc));assertEquals(register.stream().filter(row->value(row,"DocNo")==doc).count(),actual.size());}
        }finally{executor.shutdownNow();}
        assertEquals(before,jdbc.queryForMap("SELECT COUNT_BIG(*) AS Rows, CHECKSUM_AGG(BINARY_CHECKSUM(*)) AS Checksum FROM dbo.SaleOrderRegisterTemp"),"Shared desktop scratch data must be unchanged");
        Files.writeString(path.resolve("summary-manifest.json"),mapper.writeValueAsString(manifest));Files.writeString(path.resolve("initial.json"),mapper.writeValueAsString(service.initial()));
        Files.writeString(path.resolve("summary-result.txt"),mapper.writerWithDefaultPrettyPrinter().writeValueAsString(counts)+"\nAll quantity, weight and amount totals checked; simultaneous document filters isolated; desktop scratch table unchanged.\n");
    }
}
