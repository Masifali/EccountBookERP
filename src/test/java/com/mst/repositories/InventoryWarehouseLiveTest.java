package com.mst.repositories;
import com.mst.models.dto.InventoryWarehouseRequest;
import com.mst.services.DesktopInventoryWarehouseService;
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
class InventoryWarehouseLiveTest {
 @SuppressWarnings("unchecked") @Test void originalWarehouseCrudAndAllocationRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryWarehouseRepository(jdbc);var choices=Map.of("types",repo.types(),"branches",repo.branches(u));var history=repo.history(u);
  assertEquals(jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId"),history);
  var before=jdbc.queryForList("SELECT * FROM InvWareHouse WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());
  var allocationsBefore=jdbc.queryForList("SELECT * FROM WarehousesAllocationToBranch ORDER BY Id");
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryWarehouseService(repo,context,mock(DesktopReportRights.class));
  var r=new InventoryWarehouseRequest();r.code="VERIFY_"+UUID.randomUUID().toString().substring(0,8);r.name="Inventory warehouse rollback verification";r.warehouseType=((Number)((List<Map<String,Object>>)choices.get("types")).get(0).get("Id")).intValue();
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{
   var row=service.save(r);r.id=((Number)row.get("Id")).intValue();assertEquals(r.code,row.get("WareHouseCode"));assertEquals(u.getId(),row.get("EntryUser"));assertNull(row.get("BranchesId"));
   assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM WarehousesAllocationToBranch a LEFT JOIN Branches b ON b.Id=a.BranchId WHERE a.WarehouseId=? AND (b.Id IS NULL OR b.OrganizationId<>a.OrganizationId OR b.CompanyId<>a.CompanyId)",Integer.class,r.id));
   r.name+=" updated";r.active=false;var changed=service.save(r);assertEquals(r.name,changed.get("WareHouseName"));assertEquals(false,changed.get("IsActive"));assertEquals(u.getId(),changed.get("ModifyUser"));assertEquals(row.get("EntryDate"),changed.get("EntryDate"));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.id));
   var branchRows=(List<Map<String,Object>>)choices.get("branches");assertFalse(branchRows.isEmpty());int branch=((Number)branchRows.get(0).get("Id")).intValue();
   // Remove only this test warehouse's auto-allocations inside the rollback transaction, then exercise the original child procedures.
   jdbc.update("DELETE FROM WarehousesAllocationToBranch WHERE WarehouseId=?",r.id);
   var a=new InventoryWarehouseRequest.Allocation();a.branchId=branch;a.warehouseIds=List.of(r.id);a.allocate=true;service.allocate(a);
   var allocation=jdbc.queryForMap("SELECT * FROM WarehousesAllocationToBranch WHERE WarehouseId=? AND BranchId=?",r.id,branch);assertEquals(u.getId(),allocation.get("EntryUserId"));assertEquals(u.getCompanyId(),allocation.get("CompanyId"));assertEquals(0,allocation.get("FinancialYearId"));
   a.allocate=false;service.allocate(a);assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM WarehousesAllocationToBranch WHERE WarehouseId=?",Integer.class,r.id));
   r.code="";assertThrows(IllegalArgumentException.class,()->service.save(r));return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM InvWareHouse WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));assertEquals(allocationsBefore,jdbc.queryForList("SELECT * FROM WarehousesAllocationToBranch ORDER BY Id"));
  Files.writeString(Path.of("migration/inventory/evidence/warehouse-rollback.txt"),"Inventory116: "+history.size()+" history rows match Sp_InvWareHouse_GetAllMethod. Original insert/load/update, active state, audit, company ownership, branch allocation/unallocation and required-code rejection verified inside rollback. All original InvWareHouse and all-company allocation rows unchanged afterward. Corrected cross-company auto-allocation for the new warehouse only; database schema/procedures unchanged. Rack child/browser/native verification pending.\n");
 }
}

