package com.mst.repositories;

import com.mst.models.dto.SaleOutwardGatePassHistoryFilter;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleOutwardHistoryLiveTest {
 @Test void historyMatchesOriginalDesktopProcedureRows() throws Exception {
  var jdbc=SaleOutwardGatePassLiveTest.jdbc();jdbc.setQueryTimeout(30);
  var user=SaleOutwardGatePassLiveTest.user(jdbc);var repo=new SaleOutwardGatePassRepository(jdbc);
  var actual=repo.history(user,58,true,null);
  var expected=jdbc.queryForList("EXEC dbo.Sp_GatePassOutward_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=91,@FinancialYearId=58,@BranchesId=?,@CanViewAllRecord=1,@Activity='ReadByGatePassHistory'",user.getOrganizationId(),user.getCompanyId(),user.getBranchesId());
  assertEquals(expected,actual);
  if(!actual.isEmpty())assertTrue(actual.get(0).keySet().containsAll(List.of("CompanyName","ItemQty","Description","UserName","ModifyUserName","Container","Container1","NoOfAttachments")));
  var own=repo.history(user,58,false,null);
  assertEquals(jdbc.queryForList("EXEC dbo.Sp_GatePassOutward_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=91,@FinancialYearId=58,@BranchesId=?,@CanViewAllRecord=0,@EntryUser=?,@Activity='ReadByGatePassHistory'",user.getOrganizationId(),user.getCompanyId(),user.getBranchesId(),user.getId()),own);
  var from=LocalDate.of(2026,9,1);var to=LocalDate.of(2026,9,26);
  for(String mode:List.of("document","entry","modify")){
   var filter=new SaleOutwardGatePassHistoryFilter(from,to,mode,0,0,0);
   String prefix=mode.equals("document")?"Date":mode.equals("entry")?"Entry":"Modify";
   String start=prefix+(mode.equals("document")?"From":"FromDate"),end=prefix+(mode.equals("document")?"To":"ToDate");
   assertEquals(jdbc.queryForList("EXEC dbo.Sp_GatePassOutward_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=91,@FinancialYearId=58,@BranchesId=?,@CanViewAllRecord=1,@"+start+"=?,@"+end+"=?,@Activity='ReadByGatePassHistory'",user.getOrganizationId(),user.getCompanyId(),user.getBranchesId(),from,to),repo.history(user,58,true,filter));
  }
  Files.createDirectories(Path.of("migration/sale/evidence"));
  Files.writeString(Path.of("migration/sale/evidence/outward-history-read-parity.txt"),"Read-only GoldenAcedb comparison: Java repository rows equal original desktop Sp_GatePassOutward_GetAllMethod for org/company "+user.getCompanyId()+", branch "+user.getBranchesId()+", FY 58, document 91. All-record rows="+actual.size()+", own-record rows="+own.size()+". Document/entry/modify date parameter comparisons passed. History column names confirmed. No business data written. Native desktop GUI comparison still pending.\n");
 }
}
