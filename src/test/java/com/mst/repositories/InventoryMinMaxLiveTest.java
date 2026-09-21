package com.mst.repositories;
import com.mst.models.dto.InventoryMinMaxRequest;
import com.mst.services.DesktopInventoryMinMaxService;
import com.mst.security.*;
import java.util.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryMinMaxLiveTest {
 @SuppressWarnings("unchecked") @Test void originalScheduleAndItemSideEffectsRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryMinMaxRepository(jdbc);var choices=repo.lookups(u);var filter=new InventoryMinMaxRequest.Filter();var history=repo.query(u,filter,true);
  assertEquals(jdbc.queryForList("EXEC dbo.USP_GetItemsFromMinAndMaxRateSchedule_FormHistory @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()),history);
  var parents=(List<Map<String,Object>>)choices.get("parents");assertFalse(parents.isEmpty());var items=repo.items(u,((Number)parents.get(0).get("Id")).intValue());assertFalse(items.isEmpty());int item=((Number)items.get(0).get("Id")).intValue();var units=repo.uoms(u,item);assertFalse(units.isEmpty());var unit=units.stream().filter(v->new BigDecimal(v.get("Equivalent").toString()).signum()>0).findFirst().orElseThrow();
  var before=jdbc.queryForList("SELECT * FROM ItemMinMaxRateSchedule WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());var itemBefore=jdbc.queryForMap("SELECT MinRate,MaxRate,EffectedDate FROM Item WHERE Id=?",item);
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryMinMaxService(repo,context,mock(DesktopReportRights.class));var request=new InventoryMinMaxRequest();request.effectedDate=LocalDateTime.of(2099,12,29,10,11,12);while(jdbc.queryForObject("SELECT COUNT(*) FROM ItemMinMaxRateSchedule WHERE ItemId=? AND CAST(EffectedDate AS DATE)=?",Integer.class,item,java.sql.Date.valueOf(request.effectedDate.toLocalDate()))>0)request.effectedDate=request.effectedDate.plusDays(1);var row=new InventoryMinMaxRequest.Row();row.itemId=item;row.rateUomId=((Number)unit.get("Id")).intValue();row.rateUom=new BigDecimal(unit.get("Equivalent").toString());row.minRate=new BigDecimal("123.125");row.maxRate=new BigDecimal("456.875");request.rows=List.of(row);
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{
   var result=service.save(request);int id=((List<Integer>)result.get("ids")).get(0);var saved=repo.record(u,id);assertEquals(u.getId(),saved.get("EntryUser"));assertEquals(u.getBranchesId(),saved.get("BranchesId"));assertEquals(repo.year(u),saved.get("FinancialYearId"));assertEquals(row.rateUomId,saved.get("RateUomId"));assertEquals(row.minRate.doubleValue(),((Number)saved.get("MinRate")).doubleValue());assertEquals(row.maxRate.doubleValue(),jdbc.queryForObject("SELECT MaxRate FROM Item WHERE Id=?",Double.class,item));
   assertThrows(org.springframework.dao.DataAccessException.class,()->service.save(request));var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,id));
   var deletion=new InventoryMinMaxRequest.Delete();deletion.ids=List.of(id);service.delete(deletion);assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM ItemMinMaxRateSchedule WHERE Id=?",Integer.class,id));assertEquals(row.maxRate.doubleValue(),jdbc.queryForObject("SELECT MaxRate FROM Item WHERE Id=?",Double.class,item));
   request.effectedDate=request.effectedDate.plusDays(1);row.minRate=new BigDecimal("124.125");int next=((List<Integer>)service.save(request).get("ids")).get(0);assertEquals(124.125,((Number)repo.record(u,next).get("MinRate")).doubleValue());return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM ItemMinMaxRateSchedule WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));assertEquals(itemBefore,jdbc.queryForMap("SELECT MinRate,MaxRate,EffectedDate FROM Item WHERE Id=?",item));
  Files.writeString(Path.of("migration/inventory/evidence/min-max-rollback.txt"),"Inventory104: "+history.size()+" history rows match original query. Original schedule insert/load, UOM equivalent resolution, Item current-rate side effect, branch/year/user audit, duplicate-date rejection, company guard, history delete and subsequent dated rate verified inside rollback. Delete deliberately leaves Item current rates unchanged, matching usp_MinMaxDeletionById. Original schedule and Item values unchanged afterward. Browser/native/Crystal326 and326_01 pending.\n");
 }
}
