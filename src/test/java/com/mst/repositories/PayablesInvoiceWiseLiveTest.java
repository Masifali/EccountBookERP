package com.mst.repositories;
import com.mst.models.UserAccount;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="accounts.live",matches="true")
class PayablesInvoiceWiseLiveTest {
 @Test void resultRowsMatchDesktopProcedure() throws Exception {
  Properties p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}
  JdbcTemplate jdbc=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url")+";loginTimeout=10;socketTimeout=60000",p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));
  jdbc.setQueryTimeout(60);
  var account=jdbc.queryForMap("SELECT OrganizationId,CompanyId FROM dbo.UserAccount WHERE UserName='numan'");
  UserAccount u=new UserAccount();u.setOrganizationId(((Number)account.get("OrganizationId")).intValue());u.setCompanyId(((Number)account.get("CompanyId")).intValue());
  var repo=new PayablesReportRepository(jdbc);
  for(int action=0;action<=2;action++) {
   String suffix=action==0?"":", @ActionId="+action;
   var desktop=jdbc.queryForList("EXEC dbo.usp_PayablesReportInvoiceWise @OrganizationId=?,@CompanyId=?,@FromDate='2026-09-01',@ToDate='2026-09-26'"+suffix,u.getOrganizationId(),u.getCompanyId());
   var javaRows=repo.invoiceWise(u,LocalDate.of(2026,9,1),LocalDate.of(2026,9,26),0,0,0,0,action).get(0);
   assertEquals(desktop.size(),javaRows.size());
   for(int i=0;i<desktop.size();i++) for(String field:List.of("Id","DocumentTypeId","DocNo","BillAmount","PaidAmount","UnPaidAmount","SupplierName")) assertEquals(desktop.get(i).get(field),javaRows.get(i).get(field),field+" row "+i);
   System.out.println("Invoice-wise action="+action+" rows="+javaRows.size()+" matched desktop procedure");
  }
 }
}

