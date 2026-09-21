package com.mst.repositories;
import com.mst.models.dto.InventoryItemWarehouseAllocationRequest;
import com.mst.security.*;
import com.mst.services.DesktopInventoryItemWarehouseAllocationService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryItemWarehouseAllocationLiveTest {
 @Test void allocationMatchesOriginalRowsAndRollsBack() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemWarehouseAllocationRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryItemWarehouseAllocationService(repo,context,mock(DesktopReportRights.class));
  var items=repo.items(u,0,0);var warehouses=repo.warehouses(u);assertFalse(items.isEmpty());assertFalse(warehouses.isEmpty());assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetItemsForAllocatetoWarehouse'",u.getOrganizationId(),u.getCompanyId())),new HashSet<>(items));assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetActiveWareHouse'",u.getOrganizationId(),u.getCompanyId())),new HashSet<>(warehouses));
  var before=jdbc.queryForList("SELECT * FROM ItemAllocateToWareHouse ORDER BY Id");int item=0,warehouse=0;outer:for(var i:items)for(var w:warehouses){if(jdbc.queryForObject("SELECT COUNT(*) FROM ItemAllocateToWareHouse WHERE ItemId=? AND WarehouseId=? AND OrganizationId=? AND CompanyId=?",Integer.class,i.get("Id"),w.get("Id"),u.getOrganizationId(),u.getCompanyId())==0){item=num(i.get("Id"));warehouse=num(w.get("Id"));break outer;}}assertTrue(item>0&&warehouse>0,"Need an existing eligible item/warehouse pair, without inventing reference data");
  var request=new InventoryItemWarehouseAllocationRequest();request.itemIds=List.of(item);request.warehouseIds=List.of(warehouse);var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{assertEquals(1,service.save(request).get("inserted"));var row=jdbc.queryForMap("SELECT * FROM ItemAllocateToWareHouse WHERE ItemId=? AND WarehouseId=? AND OrganizationId=? AND CompanyId=?",request.itemIds.get(0),request.warehouseIds.get(0),u.getOrganizationId(),u.getCompanyId());assertEquals(u.getId(),row.get("EntryUser"));assertEquals(0,row.get("BrancheId"));assertEquals(0,row.get("ProjectsId"));assertEquals(1,row.get("ActionId"));assertEquals(0,service.save(request).get("inserted"));request.warehouseIds=List.of(-1);assertThrows(IllegalArgumentException.class,()->service.save(request));return null;}finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM ItemAllocateToWareHouse ORDER BY Id"));Files.writeString(Path.of("migration/inventory/evidence/item-warehouse-allocations-rollback.txt"),"ItemAllocateToWarehouse: "+items.size()+" item rows and "+warehouses.size()+" active warehouses match original queries. Selected allocation inserts source ActionId1, BrancheId0, ProjectsId0 and authenticated audit/scope; repeat allocation is a no-op; invalid warehouse denied. Entire ItemAllocateToWareHouse table unchanged after rollback. Rights mocked. Browser/native pending. No source Update/Delete/History handler.\n");
 }
 private static int num(Object value){return ((Number)value).intValue();}
}
