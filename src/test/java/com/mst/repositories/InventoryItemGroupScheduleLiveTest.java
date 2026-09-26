package com.mst.repositories;
import com.mst.models.dto.InventoryItemGroupScheduleRequest;
import com.mst.security.*;
import com.mst.services.DesktopInventoryItemGroupScheduleService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryItemGroupScheduleLiveTest {
 @Test void schedulesAndAssignmentMatchOriginalQueriesAndRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemGroupScheduleRepository(jdbc,new DesktopInventoryItemLookupRepository(jdbc));var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryItemGroupScheduleService(repo,context,mock(DesktopReportRights.class));
  var schedulesBefore=jdbc.queryForList("SELECT * FROM ItemGroup_UOMSchedule ORDER BY Id");var unitsBefore=jdbc.queryForList("SELECT * FROM UOMSchedule ORDER BY Id");var items=repo.allItems(u);assertFalse(items.isEmpty());var direct=jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());assertEquals(new HashSet<>(direct),new HashSet<>(items));assertEquals(direct.size(),items.size());
  var fixture=jdbc.queryForMap("SELECT TOP 1 g.GroupId,u.Id UnitId FROM ItemGroup g CROSS JOIN UOM u WHERE g.OrganizationId=? AND g.CompanyId=? AND u.OrganizationId=g.OrganizationId AND u.CompanyId=g.CompanyId AND NOT EXISTS(SELECT 1 FROM ItemGroup_UOMSchedule s WHERE s.ItemGroupId=g.GroupId AND s.UOMId=u.Id AND s.OrganizationId=g.OrganizationId AND s.CompanyId=g.CompanyId) ORDER BY g.GroupId,u.Id",u.getOrganizationId(),u.getCompanyId());
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{
   var r=new InventoryItemGroupScheduleRequest();r.groupId=num(fixture.get("GroupId"));r.unitId=num(fixture.get("UnitId"));r.equivalent="40";var saved=service.save(r);r.id=num(saved.get("Id"));assertEquals(r.groupId,saved.get("ItemGroupId"));assertEquals(r.unitId,saved.get("UOMId"));assertEquals(40d,Double.parseDouble(saved.get("Equivalent").toString()));
   r.equivalent="1000";assertEquals(1000d,Double.parseDouble(service.save(r).get("Equivalent").toString()));assertTrue(repo.history(u).stream().anyMatch(row->num(row.get("Id"))==r.id));
   int item=items.stream().map(row->num(row.get("Id"))).filter(id->jdbc.queryForObject("SELECT COUNT(*) FROM UOMSchedule WHERE ItemId=? AND ScheduleUnitId=?",Integer.class,id,r.unitId)==0).findFirst().orElseThrow();
   var expected=jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @ItemGroupId=?, @Activity='GetUomForAssigntoItem'",u.getOrganizationId(),u.getCompanyId(),item,r.groupId);assertFalse(expected.isEmpty());
   var request=new InventoryItemGroupScheduleRequest.Assignment();request.groupId=r.groupId;request.itemIds=List.of(item);assertEquals(expected.size(),service.assign(request).get("assigned"));
   for(var schedule:expected){var row=jdbc.queryForMap("SELECT * FROM UOMSchedule WHERE OrganizationId=? AND CompanyId=? AND ItemId=? AND ScheduleUnitId=?",u.getOrganizationId(),u.getCompanyId(),item,schedule.get("UOMId"));assertEquals(Double.parseDouble(schedule.get("Equivalent").toString()),((Number)row.get("Equivalent")).doubleValue());assertEquals(schedule.get("QtyEquivalent"),row.get("QtyEquivalent"));assertEquals(u.getId(),row.get("EntryUser"));assertEquals(true,row.get("Active"));}
   assertEquals(0,service.assign(request).get("assigned"));var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.id));return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(schedulesBefore,jdbc.queryForList("SELECT * FROM ItemGroup_UOMSchedule ORDER BY Id"));assertEquals(unitsBefore,jdbc.queryForList("SELECT * FROM UOMSchedule ORDER BY Id"));
  Files.writeString(Path.of("migration/inventory/evidence/item-group-schedules-rollback.txt"),"DefineItemGroupUOMSchedule and frmAssignGroupToItem: existing group/unit lookups, schedule insert/load/update and company denial verified. Global item query matched "+items.size()+" rows. Assignment eligibility, created UOM equivalent/quantity/audit/active values and repeated assignment no-op matched original procedures. All group/UOM rows, including base-rate flags, unchanged after rollback. Rights mocked; browser/native and saved grid layout remain pending.\n");
 }
 private static int num(Object value){return ((Number)value).intValue();}
}
