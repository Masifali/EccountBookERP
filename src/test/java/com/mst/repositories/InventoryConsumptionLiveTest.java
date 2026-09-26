package com.mst.repositories;
import com.mst.models.dto.InventoryConsumptionRequest;
import com.mst.services.DesktopInventoryConsumptionService;
import com.mst.security.*;
import java.util.*;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryConsumptionLiveTest {
 @Test void originalAllocationAndDeactivationRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryConsumptionRepository(jdbc);assertNotNull(repo.lookups(u).get("items"));var history=repo.history(u);var available=repo.available(u,0,0);
  assertEquals(jdbc.queryForList("EXEC dbo.USP_ConsumptionItems_FormHistory @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()),history);assertEquals(jdbc.queryForList("EXEC dbo.USP_GetItemsAllocationForConsumption @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()),available);assertFalse(available.isEmpty());
  var before=jdbc.queryForList("SELECT * FROM ConsumptionItems WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryConsumptionService(repo,context,mock(DesktopReportRights.class));var request=new InventoryConsumptionRequest();var row=new InventoryConsumptionRequest.Row();row.itemId=((Number)available.get(0).get("Id")).intValue();row.remarks="ROLLBACK consumption allocation";request.rows=List.of(row);
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{
   var result=service.save(request,false);row.id=((Number)((List<?>)result.get("ids")).get(0)).intValue();var saved=repo.record(u,row.id);assertEquals(true,saved.get("IsActive"));assertEquals(u.getBranchesId(),saved.get("BranchesId"));assertEquals(u.getId(),saved.get("EntryUserId"));assertFalse(repo.available(u,0,0).stream().anyMatch(r->Objects.equals(r.get("Id"),row.itemId)));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,row.id));row.active=false;row.remarks="ROLLBACK updated remarks";service.save(request,true);var changed=repo.record(u,row.id);assertEquals(false,changed.get("IsActive"));assertEquals(row.remarks,changed.get("Remarks"));assertEquals(u.getId(),changed.get("ModifyUserId"));assertEquals(saved.get("EntryDate"),changed.get("EntryDate"));assertFalse(repo.available(u,0,0).stream().anyMatch(r->Objects.equals(r.get("Id"),row.itemId)));row.active=true;service.save(request,true);assertEquals(true,repo.record(u,row.id).get("IsActive"));return null;
  }finally{status.setRollbackOnly();}});assertEquals(before,jdbc.queryForList("SELECT * FROM ConsumptionItems WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));
  Files.writeString(Path.of("migration/inventory/evidence/consumption-rollback.txt"),"Inventory105: "+available.size()+" available rows and "+history.size()+" history rows match original procedures. Insert/load/update remarks/deactivate/reactivate, branch/user audit and cross-company denial passed in rollback. Inactive allocations remain excluded from available items, matching original SQL. All original ConsumptionItems rows unchanged afterward. No delete exists in the desktop workflow. Browser/native/grid settings pending.\n");
 }
}
