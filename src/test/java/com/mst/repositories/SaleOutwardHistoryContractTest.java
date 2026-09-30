package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleOutwardGatePassHistoryFilter;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import static org.junit.jupiter.api.Assertions.*;

class SaleOutwardHistoryContractTest {
 static class Db extends JdbcTemplate {
  String sql; Object[] args; List<Map<String,Object>> rows=List.of();
  @Override public List<Map<String,Object>> queryForList(String sql,Object... args){this.sql=sql;this.args=args;return rows;}
 }
 private UserAccount user(){var u=new UserAccount();u.setId(42);u.setOrganizationId(78);u.setCompanyId(78);u.setBranchesId(58);return u;}
 @Test void historyUsesDesktopScopeAndOwnRecordsWhenViewAllIsAbsent(){
  var db=new Db();var repo=new SaleOutwardGatePassRepository(db);
  repo.history(user(),58,false,null);
  assertTrue(db.sql.contains("@CanViewAllRecord=?"));assertTrue(db.sql.contains("@EntryUser=?"));
  assertTrue(db.sql.endsWith("@Activity='ReadByGatePassHistory'"));
  assertArrayEquals(new Object[]{78,78,91,58,58,0,42},db.args);
 }
 @Test void viewAllIsReadForThisScreenAndCompany(){
  var db=new Db();var repo=new SaleOutwardGatePassRepository(db);
  assertFalse(repo.canViewAllRecords(user(),"User"));
  assertArrayEquals(new Object[]{42,"OutwardGatePass","User",78},db.args);
  db.rows=List.of(Map.of("RightName","CanView AllRecord","Value",true));
  assertTrue(repo.canViewAllRecords(user(),"User"));
  db.rows=List.of(Map.of("RightName","CanView AllRecord","Value",false));
  assertFalse(repo.canViewAllRecords(user(),"User"));assertTrue(repo.canViewAllRecords(user(),"Admin"));
 }
 @Test void allThreeDateModesUseOriginalProcedureParameters(){
  var db=new Db();var repo=new SaleOutwardGatePassRepository(db);var from=LocalDate.of(2026,9,1);var to=from.plusDays(10);
  for(var type:List.of("document","entry","modify")){
   repo.history(user(),58,true,new SaleOutwardGatePassHistoryFilter(from,to,type,5,10,7));
   String prefix=type.equals("document")?"Date":type.equals("entry")?"Entry":"Modify";
   assertTrue(db.sql.contains("@"+prefix+(type.equals("document")?"From":"FromDate")+"=?"));
   assertFalse(db.sql.contains("@EntryUser="));
   assertArrayEquals(new Object[]{78,78,91,58,58,1,from,to,5,10,7},db.args);
  }
 }
}
