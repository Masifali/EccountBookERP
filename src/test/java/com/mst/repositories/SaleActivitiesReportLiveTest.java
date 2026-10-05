package com.mst.repositories;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.models.*;
import com.mst.security.*;
import com.mst.services.SaleActivitiesReportService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

/** Opt-in: reads business data and only writes connection-local temporary tables. */
@EnabledIfSystemProperty(named="sale.activities.live",matches="true")
class SaleActivitiesReportLiveTest {
    @Test void allDesktopActivitiesLoadWithoutChangingSharedData() throws Exception {
        var config=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application-local.properties"))){config.load(in);}
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(config.getProperty("spring.datasource.url"),config.getProperty("spring.datasource.username"),config.getProperty("spring.datasource.password")));jdbc.setQueryTimeout(90);
        var account=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId,AppId FROM dbo.UserAccount WHERE UserName=?","numan");
        var user=new UserAccount();user.setId(n(account.get("ID")));user.setOrganizationId(n(account.get("OrganizationId")));user.setCompanyId(n(account.get("CompanyId")));user.setBranchesId(n(account.get("BranchesId")));user.setAppId(n(account.get("AppId")));
        var repository=new SaleActivitiesReportRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(user);
        var service=new SaleActivitiesReportService(repository,context,mock(DesktopReportRights.class));var mapper=new ObjectMapper().findAndRegisterModules();var path=Path.of("target/sale-activities-verification");Files.createDirectories(path);
        var before=jdbc.queryForMap("SELECT COUNT_BIG(*) AS Rows,CHECKSUM_AGG(BINARY_CHECKSUM(*)) AS Checksum FROM dbo.InvSalesRegisterTemp");
        Files.writeString(path.resolve("initial.json"),mapper.writeValueAsString(service.initial()));
        Files.writeString(path.resolve("lookups.json"),mapper.writeValueAsString(service.lookups(List.of(user.getBranchesId()))));
        var errors=new ArrayList<String>();var counts=new LinkedHashMap<String,Integer>();var manifest=new LinkedHashMap<String,String>();
        for(int index=0;index<SaleActivitiesReportColumns.ACTIVITIES.size();index++){
            String activity=SaleActivitiesReportColumns.ACTIVITIES.get(index);
            try {
                var filter=filter(user.getBranchesId(),activity,LocalDate.of(2026,9,1));var result=service.rows(filter);
                @SuppressWarnings("unchecked") var rows=(List<Map<String,Object>>)result.get("rows");counts.put(activity,rows.size());String file="activity-"+index+".json";manifest.put(activity,file);Files.writeString(path.resolve(file),mapper.writeValueAsString(result));
                if(index==0){assertFalse(rows.isEmpty());for(var row:rows)assertEquals(d(row,"ItemRate")-d(row,"RateCut"),d(row,"NetRate"),.01);Files.writeString(path.resolve("register-raw.json"),mapper.writeValueAsString(repository.rows(user,filter,","+user.getBranchesId(),"")));}
            } catch(Exception error){Throwable cause=error;while(cause.getCause()!=null)cause=cause.getCause();errors.add(activity+": "+cause.getMessage());}
        }
        try {var current=service.rows(filter(user.getBranchesId(),"Sales Register",LocalDate.of(2026,9,27)));Files.writeString(path.resolve("this-week.json"),mapper.writeValueAsString(current));}catch(Exception error){Throwable cause=error;while(cause.getCause()!=null)cause=cause.getCause();errors.add("Screenshot date range: "+cause.getMessage());}
        assertEquals(before,jdbc.queryForMap("SELECT COUNT_BIG(*) AS Rows,CHECKSUM_AGG(BINARY_CHECKSUM(*)) AS Checksum FROM dbo.InvSalesRegisterTemp"));
        Files.writeString(path.resolve("manifest.json"),mapper.writeValueAsString(manifest));Files.writeString(path.resolve("result.txt"),mapper.writerWithDefaultPrettyPrinter().writeValueAsString(counts)+"\n"+String.join("\n",errors));
        assertTrue(errors.isEmpty(),String.join("\n",errors));
    }
    static SaleActivitiesReportFilter filter(int branch,String activity,LocalDate from){return new SaleActivitiesReportFilter(from,LocalDate.of(2026,10,4),0,0,0,0,0,0,0,null,0,0,0,0,0,0,0,0,0,0,0,activity,List.of(branch),List.of());}
    static int n(Object value){return ((Number)value).intValue();}
    static double d(Map<String,Object> row,String key){return row.get(key) instanceof Number number?number.doubleValue():0;}
}
