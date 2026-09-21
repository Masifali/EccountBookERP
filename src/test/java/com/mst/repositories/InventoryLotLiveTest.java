package com.mst.repositories;
import com.mst.models.dto.InventoryLotRequest;
import com.mst.services.DesktopInventoryLotService;
import com.mst.security.*;
import java.util.*;
import java.time.LocalDateTime;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryLotLiveTest {
 @SuppressWarnings("unchecked") @Test void originalJobLotCrudAndAllocationRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryLotRepository(jdbc);var choices=repo.lookups(u);var history=repo.history(u);
  assertEquals(jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"GetAll"),history);
  var before=jdbc.queryForList("SELECT * FROM JobLot WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());
  var allocationsBefore=jdbc.queryForList("SELECT * FROM JobLotsAllocationToBranch ORDER BY Id");
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryLotService(repo,context,mock(DesktopReportRights.class));
  var r=new InventoryLotRequest();r.code="VERIFY_"+UUID.randomUUID().toString().substring(0,8);r.description="Inventory JobLot rollback verification";r.startDate=LocalDateTime.of(2026,9,1,10,11,12);r.endDate=LocalDateTime.of(2026,9,30,10,11,12);r.status="InComplete";r.jobTypeId=((Number)((List<Map<String,Object>>)choices.get("jobTypes")).get(0).get("Id")).intValue();
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{
   var row=service.save(r);r.id=((Number)row.get("Id")).intValue();assertEquals(r.code,row.get("JobLotCode"));assertEquals(u.getId(),row.get("EntryUser"));assertEquals(r.startDate,((java.sql.Timestamp)row.get("StartDate")).toLocalDateTime());
   assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM JobLotsAllocationToBranch a LEFT JOIN Branches b ON b.Id=a.BranchId WHERE a.JobLotId=? AND (b.Id IS NULL OR b.OrganizationId<>a.OrganizationId OR b.CompanyId<>a.CompanyId)",Integer.class,r.id));
   r.description+=" updated";r.status="Complete";var changed=service.save(r);assertEquals(r.description,changed.get("JobLotDescription"));assertEquals("Complete",changed.get("JobStatus"));assertEquals(0,changed.get("ModifyUser"));assertEquals(row.get("EntryDate"),changed.get("EntryDate"));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.id));
   var branchRows=(List<Map<String,Object>>)choices.get("branches");assertFalse(branchRows.isEmpty());int branch=((Number)branchRows.get(0).get("Id")).intValue();
   // Remove only this test lot's auto-allocations inside the rollback transaction, then exercise the original child procedures.
   jdbc.update("DELETE FROM JobLotsAllocationToBranch WHERE JobLotId=?",r.id);
   var a=new InventoryLotRequest.Allocation();a.branchId=branch;a.lotIds=List.of(r.id);a.allocate=true;service.allocate(a);
   var allocation=jdbc.queryForMap("SELECT * FROM JobLotsAllocationToBranch WHERE JobLotId=? AND BranchId=?",r.id,branch);assertEquals(u.getId(),allocation.get("EntryUserId"));assertEquals(u.getCompanyId(),allocation.get("CompanyId"));assertEquals(0,allocation.get("FinancialYearId"));
   a.allocate=false;service.allocate(a);assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM JobLotsAllocationToBranch WHERE JobLotId=?",Integer.class,r.id));
   r.endDate=r.startDate.minusDays(1);assertThrows(IllegalArgumentException.class,()->service.save(r));return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM JobLot WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));assertEquals(allocationsBefore,jdbc.queryForList("SELECT * FROM JobLotsAllocationToBranch ORDER BY Id"));
  Files.writeString(Path.of("migration/inventory/evidence/job-lot-rollback.txt"),"Inventory110: "+history.size()+" history rows match SP_JobLot_ReadMethod. Original insert/load/update, dates, audit, company ownership, branch allocation/unallocation and date rejection verified inside rollback. All original JobLot and all-company allocation rows unchanged afterward. Corrected cross-company auto-allocation for the new lot only; database schema/procedures unchanged. Browser/native and reference-document navigation pending.\n");
 }
}
