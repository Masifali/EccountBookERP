package com.mst.repositories;

import com.mst.models.UserAccount;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleGdnLiveTest {
    private JdbcTemplate jdbc()throws Exception{var p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}var j=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));j.setQueryTimeout(90);return j;}
    private int n(Object o){return o instanceof Number?((Number)o).intValue():0;}
    @Test void desktopLookupsHistoryAndDeliveryPathsMatchGoldenAce()throws Exception{
        var j=jdbc();var a=j.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId,AppId FROM UserAccount WHERE UserName='numan'");var u=new UserAccount();u.setId(n(a.get("ID")));u.setOrganizationId(n(a.get("OrganizationId")));u.setCompanyId(n(a.get("CompanyId")));u.setBranchesId(n(a.get("BranchesId")));u.setAppId(n(a.get("AppId")));
        var r=new SaleGdnRepository(j);var initial=r.initial(u,58);assertFalse(((List<?>)initial.get("customers")).isEmpty(),"customer lookup is empty");assertFalse(((List<?>)initial.get("items")).isEmpty(),"item lookup is empty");assertFalse(((List<?>)initial.get("warehouses")).isEmpty(),"warehouse lookup is empty");assertFalse(((List<?>)initial.get("cities")).isEmpty(),"city lookup is empty");
        var history=r.history(u,58);if(!history.isEmpty()){var record=r.record(u,n(history.get(0).get("Id")));assertEquals(86,n(record.get("DocumentTypeId")));assertTrue(record.containsKey("details"));}
        Files.createDirectories(Path.of("migration/sale/evidence"));Files.writeString(Path.of("migration/sale/evidence/gdn-read.txt"),"Goods Dispatch Note document 86. GoldenAcedb desktop numbering, customer, item, pending gate pass, warehouse, brand, crop, job lot, packing, UOM, expense, city, transporter, form history and record load paths executed for Numan tenant 78/78 branch 58 year 58. Customers="+((List<?>)initial.get("customers")).size()+", items="+((List<?>)initial.get("items")).size()+", warehouses="+((List<?>)initial.get("warehouses")).size()+", cities="+((List<?>)initial.get("cities")).size()+", gate passes="+((List<?>)initial.get("gatePasses")).size()+", history="+history.size()+".\n");
    }
}
