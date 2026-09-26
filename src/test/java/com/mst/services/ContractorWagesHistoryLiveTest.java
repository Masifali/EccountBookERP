package com.mst.services;

import com.mst.security.CurrentUserContext;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import org.springframework.test.util.ReflectionTestUtils;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="purchase.live", matches="true")
class ContractorWagesHistoryLiveTest {
    @Test void blankAndSelectedFiltersMatchDesktop() throws Exception {
        var props=new Properties();
        try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))) { props.load(in); }
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(props.getProperty("spring.datasource.url"),props.getProperty("spring.datasource.username"),props.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        var user=jdbc.queryForMap("SELECT OrganizationId,CompanyId FROM UserAccount WHERE UserName='numan'");
        int org=((Number)user.get("OrganizationId")).intValue(), company=((Number)user.get("CompanyId")).intValue();
        var ctx=mock(CurrentUserContext.class);
        when(ctx.currentOrganizationId()).thenReturn(org); when(ctx.currentCompanyId()).thenReturn(company);
        var service=new ContractorWagesScheduleService();
        ReflectionTestUtils.setField(service,"jdbcTemplate",jdbc); ReflectionTestUtils.setField(service,"currentUserContext",ctx);
        String sql="EXEC dbo.Sp_InvContractorWagesSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ActionId=1,@Activity='ReadAll'";
        var expected=jdbc.queryForList(sql,org,company);
        var actual=service.getContractorWiseHistory(0,0,null,null);
        assertEquals(ids(expected),ids(actual));
        var dated=service.getContractorWiseHistory(0,0,"2026-01-16","2026-09-23");
        assertEquals(ids(jdbc.queryForList(sql+",@EffectedDate=?,@EffectedDateTo=?",org,company,java.sql.Date.valueOf("2026-01-16"),java.sql.Date.valueOf("2026-09-23"))),ids(dated));
        if(!expected.isEmpty()) {
            int contractor=((Number)expected.get(0).get("ContractorId")).intValue();
            assertEquals(ids(jdbc.queryForList(sql+",@ContractorId=?",org,company,contractor)), ids(service.getContractorWiseHistory(0,contractor,null,null)));
        }
        Files.writeString(Path.of("migration/purchase/evidence/contractor-wages-history.txt"),"Read-only GoldenAcedb comparison passed. Blank account/contractor, date-only and available contractor-only filters match desktop procedure row IDs. Unfiltered rows="+actual.size()+", date-filtered rows="+dated.size()+". No database writes. Browser runtime pending.\n");
    }
    private static Set<Object> ids(List<Map<String,Object>> rows) {
        Set<Object> result=new HashSet<>(); for(var row:rows) result.add(row.get("Id")); return result;
    }
}
