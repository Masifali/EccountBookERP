package com.mst.repositories;

import com.mst.models.dto.InventoryWarehouseRequest;
import com.mst.services.DesktopInventoryWarehouseService;
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
class InventoryWarehouseRackLiveTest {
 @SuppressWarnings("unchecked") @Test void originalRackAndItemAllocationRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryWarehouseRepository(jdbc);
  var before=jdbc.queryForList("SELECT * FROM invWarehouseRack ORDER BY Id");var itemsBefore=jdbc.queryForList("SELECT * FROM ItemAllocationToStoreRack ORDER BY Id");
  var auditsBefore=jdbc.queryForList("SELECT * FROM UserAudit WHERE ScreenName='frmRackDefine' AND OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());
  var warehouses=repo.rackWarehouses(u);assertFalse(warehouses.isEmpty(),"Need an existing branch warehouse; do not create reference data");
  assertEquals(jdbc.queryForList("EXEC dbo.usp_invWarehouseRack_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadAll"),repo.rackHistory(u));
  assertEquals(jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()).stream().filter(row->Boolean.TRUE.equals(row.get("IsActive"))).toList(),warehouses);
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryWarehouseService(repo,context,mock(DesktopReportRights.class));
  var r=new InventoryWarehouseRequest.Rack();r.warehouseId=((Number)warehouses.get(0).get("Id")).intValue();r.name="VERIFY_RACK_"+UUID.randomUUID();r.sortNo=19;
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  final int[] counts={0,0};
  tx.execute(status->{try{
   var row=service.saveRack(r);r.id=((Number)row.get("Id")).intValue();assertEquals(r.name,row.get("rackName"));assertEquals(u.getId(),row.get("EntryUserId"));assertEquals(19,row.get("sortNo"));
   var created=jdbc.queryForMap("SELECT TOP 1 * FROM UserAudit WHERE ScreenName='frmRackDefine' AND ReferenceId=? ORDER BY Id DESC",r.id);assertEquals(u.getId(),created.get("UserId"));assertEquals(u.getOrganizationId(),created.get("OrganizationId"));assertEquals(u.getCompanyId(),created.get("CompanyId"));assertEquals(1,created.get("ActionId"));assertEquals("Created Rack ["+r.name+"] successfully.",created.get("ActivityDescription"));
   var duplicate=new InventoryWarehouseRequest.Rack();duplicate.warehouseId=r.warehouseId;duplicate.name=r.name.toLowerCase();assertThrows(IllegalArgumentException.class,()->service.saveRack(duplicate));
   r.sortNo=20;r.active=false;r.name+=" updated";var updated=service.saveRack(r);assertEquals(r.name,updated.get("rackName"));assertEquals(false,updated.get("isActive"));assertEquals(u.getId(),updated.get("ModifyUserId"));assertEquals(row.get("EntryDate"),updated.get("EntryDate"));
   var edited=jdbc.queryForMap("SELECT TOP 1 * FROM UserAudit WHERE ScreenName='frmRackDefine' AND ReferenceId=? ORDER BY Id DESC",r.id);assertEquals(2,edited.get("ActionId"));assertEquals(u.getId(),edited.get("UserId"));assertEquals("Updated Rack ["+r.name+"] successfully.",edited.get("ActivityDescription"));
   assertTrue(repo.racks(u).stream().noneMatch(value->((Number)value.get("Id")).intValue()==r.id));r.active=true;service.saveRack(r);
   var data=service.rackItems(r.id);var available=(List<Map<String,Object>>)data.get("unallocated");counts[0]=available.size();assertFalse(available.isEmpty(),"Need real eligible items in parent category 7/8");
   assertEquals(jdbc.queryForList("EXEC dbo.usp_GetItemsByRackAllocation @OrganizationId=?, @CompanyId=?, @ActionId=1, @StoreRackId=?",u.getOrganizationId(),u.getCompanyId(),r.id),available);
   var allocation=new InventoryWarehouseRequest.RackItems();allocation.rackId=r.id;allocation.itemIds=List.of(((Number)available.get(0).get("Id")).intValue());allocation.active=true;service.saveRackItems(allocation);
   var saved=jdbc.queryForMap("SELECT * FROM ItemAllocationToStoreRack WHERE storeRackId=? AND itemId=?",r.id,allocation.itemIds.get(0));assertEquals(true,saved.get("isActive"));assertEquals(u.getId(),saved.get("EntryUserId"));
   allocation.active=false;service.saveRackItems(allocation);var changed=jdbc.queryForMap("SELECT * FROM ItemAllocationToStoreRack WHERE storeRackId=? AND itemId=?",r.id,allocation.itemIds.get(0));assertEquals(false,changed.get("isActive"));assertEquals(u.getId(),changed.get("ModifyUserId"));assertEquals(saved.get("Id"),changed.get("Id"));
   data=service.rackItems(r.id);assertEquals(available.size()-1,((List<?>)data.get("unallocated")).size());counts[1]=((List<?>)data.get("allocated")).size();assertEquals(1,counts[1]);
   allocation.active=true;service.saveRackItems(allocation);assertEquals(true,jdbc.queryForObject("SELECT isActive FROM ItemAllocationToStoreRack WHERE storeRackId=? AND itemId=?",Boolean.class,r.id,allocation.itemIds.get(0)));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.rack(other,r.id));assertThrows(IllegalArgumentException.class,()->repo.rackItems(other,r.id));
   return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM invWarehouseRack ORDER BY Id"));assertEquals(itemsBefore,jdbc.queryForList("SELECT * FROM ItemAllocationToStoreRack ORDER BY Id"));
  assertEquals(auditsBefore,jdbc.queryForList("SELECT * FROM UserAudit WHERE ScreenName='frmRackDefine' AND OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));
  Files.writeString(Path.of("migration/inventory/evidence/warehouse-racks-rollback.txt"),"Warehouse116 children: branch warehouse lookup and rack history match original procedures. Rack insert/load/update/inactivate/reactivate, duplicate name rejection, UserAudit creation/update rows, user auditing fields and cross-company denial passed. "+counts[0]+" eligible items match original allocation query. Item allocation/deactivation/reactivation preserves allocation ID and excludes inactive allocations from unallocated grid, matching desktop. All rack, allocation and scoped UserAudit rows unchanged after rollback. Browser/native and saved grid settings still pending.\n");
 }
}
