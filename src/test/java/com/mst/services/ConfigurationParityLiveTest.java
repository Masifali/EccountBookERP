package com.mst.services;
import com.mst.security.CurrentUserContext;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.*;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="configuration.live", matches="true")
class ConfigurationParityLiveTest {
 @Test void lookupsReadUpdateAndRollback() throws Exception {
  var p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}
  var jdbc=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));jdbc.setQueryTimeout(60);
  var u=jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM UserAccount WHERE UserName='numan'");
  int org=((Number)u.get("OrganizationId")).intValue(),comp=((Number)u.get("CompanyId")).intValue();
  var ctx=mock(CurrentUserContext.class);when(ctx.currentOrganizationId()).thenReturn(org);when(ctx.currentCompanyId()).thenReturn(comp);when(ctx.currentUserId()).thenReturn(((Number)u.get("Id")).intValue());when(ctx.currentBranchId()).thenReturn(((Number)u.get("BranchesId")).intValue());
  var service=new ConfigurationServiceImpl();ReflectionTestUtils.setField(service,"jdbcTemplate",jdbc);ReflectionTestUtils.setField(service,"currentUserContext",ctx);
  assertEquals(jdbc.queryForList("EXEC dbo.Sp_MultiCurrency_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",org,comp),service.getCurrencies());
  assertEquals(jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?,@CompanyId=?,@Activity='GetJobLotGlIdsandName'",org,comp),service.getJobLots());
  assertEquals(jdbc.queryForList("EXEC dbo.USP_City_GetAllWithCountryAndTehsil @OrganizationId=?,@CompanyId=?",org,comp),service.getCities());
  var history=jdbc.queryForList("EXEC dbo.Proc_ConfigrationsAllocation_History @OrganizationId=?,@CompanyId=?",org,comp);
  assertEquals(history.size(),service.getHistory().size());
  String key="DefaultDaysToLessFromHistoryFromDate";
  var before=service.getByKey(key);String old=before==null?null:before.getConfigKey();
  new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource())).execute(status->{status.setRollbackOnly();
   var result=service.saveControl(key,"7");assertTrue(result.success,result.message);assertEquals("7",service.getByKey(key).getConfigKey());
   result=service.saveControl(key,"8");assertTrue(result.success,result.message);assertEquals("8",service.getByKey(key).getConfigKey());
   assertFalse(service.saveControl("NONEXISTENT-CONFIG-TEST-KEY","1").success);
   return null;
  });
  assertEquals(old,service.getByKey(key)==null?null:service.getByKey(key).getConfigKey());
  when(ctx.currentCompanyId()).thenReturn(-1);assertTrue(service.getCurrencies().isEmpty());assertTrue(service.getJobLots().isEmpty());assertTrue(service.getGlobalAccounts(null,null,null).isEmpty());
  Files.writeString(Path.of("migration/configuration/database-evidence.txt"),"Original currency, job lot and city rows match. History count="+history.size()+". Save/load/update and invalid-key rejection passed; all writes rolled back. Empty-company lookups do not invent data or fall back to unscoped accounts. Other controls and runtime layout remain unverified.\n");
 }
}
