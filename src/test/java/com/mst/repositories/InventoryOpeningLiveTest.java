package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="inventory.live",matches="true")
class InventoryOpeningLiveTest {
    static JdbcTemplate jdbc() throws Exception {Properties p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}JdbcTemplate jdbc=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));jdbc.setQueryTimeout(45);return jdbc;}
    static UserAccount user(JdbcTemplate jdbc){var a=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId,AppId FROM dbo.UserAccount WHERE UserName=?","numan");UserAccount u=new UserAccount();u.setId(((Number)a.get("ID")).intValue());u.setOrganizationId(((Number)a.get("OrganizationId")).intValue());u.setCompanyId(((Number)a.get("CompanyId")).intValue());u.setBranchesId(((Number)a.get("BranchesId")).intValue());u.setAppId(((Number)a.get("AppId")).intValue());return u;}
    @Test void lookupsHistoryAndLoadMatchDesktopProcedures() throws Exception {
        var jdbc=jdbc();var u=user(jdbc);var repo=new InventoryOpeningRepository(jdbc);var lookups=repo.lookups(u);var years=repo.years(u);assertFalse(years.isEmpty());int year=((Number)years.get(0).get("Id")).intValue();
        var rows=repo.history(u,year,new InventoryOpeningRequest.History());
        var direct=jdbc.queryForList("EXEC dbo.Sp_InvStockOpeningBalanceHeader_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),40,year,"ReadAll");
        assertEquals(new HashSet<>(direct),new HashSet<>(rows));assertEquals(direct.size(),rows.size());
        StringBuilder evidence=new StringBuilder("Opening stock: live read-only procedure comparison\nUser="+u.getId()+" Organization="+u.getOrganizationId()+" Company="+u.getCompanyId()+" Branch="+u.getBranchesId()+" FinancialYear="+year+"\n");
        for(var e:lookups.entrySet())evidence.append(e.getKey()).append(" rows=").append(((List<?>)e.getValue()).size()).append('\n');
        evidence.append("History rows=").append(rows.size()).append("; every column matches desktop procedure.\n");
        if(!rows.isEmpty()){var first=rows.get(0);assertEquals(first,repo.record(u,((Number)first.get("Id")).intValue()));var units=repo.uoms(u,((Number)first.get("ItemId")).intValue());assertFalse(units.isEmpty());evidence.append("Record load and dependent UOM executed successfully.\n");}
        assertTrue(repo.nextCode(u,year)>0);
        evidence.append("No business writes. Native desktop and browser interaction not yet verified.\n");Files.createDirectories(Path.of("migration/inventory/evidence"));Files.writeString(Path.of("migration/inventory/evidence/opening-stock-live-read.txt"),evidence);
    }
}
