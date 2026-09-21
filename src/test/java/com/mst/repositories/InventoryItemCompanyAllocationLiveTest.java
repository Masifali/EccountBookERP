package com.mst.repositories;
import com.mst.models.dto.InventoryItemCompanyAllocationRequest;
import com.mst.security.*;
import com.mst.services.DesktopInventoryItemCompanyAllocationService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryItemCompanyAllocationLiveTest {
 @Test void companyAllocationAndStatusRoundtripRollsBack() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemCompanyAllocationRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryItemCompanyAllocationService(repo,context,mock(DesktopReportRights.class));
  var directCompanies=jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity='ReadByOrganizationId'",u.getOrganizationId());var actualCompanies=repo.companies(u);assertEquals(directCompanies.size(),actualCompanies.size());for(int index=0;index<directCompanies.size();index++)for(var entry:directCompanies.get(index).entrySet()){var actual=actualCompanies.get(index).get(entry.getKey());if(entry.getValue() instanceof byte[] bytes)assertArrayEquals(bytes,(byte[])actual,entry.getKey());else assertEquals(entry.getValue(),actual,entry.getKey());}
  var pending=repo.allocations(u,u.getCompanyId(),false);var allocated=repo.allocations(u,u.getCompanyId(),true);assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.Sp_ItemAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ActionId=1, @Activity='ItemsAllocatedOrUnAllocatedToCompany'",u.getOrganizationId(),u.getCompanyId())),new HashSet<>(pending));assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.Sp_ItemAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ActionId=2, @Activity='ItemsAllocatedOrUnAllocatedToCompany'",u.getOrganizationId(),u.getCompanyId())),new HashSet<>(allocated));
  assertThrows(IllegalArgumentException.class,()->service.rows(-1,false));
  var before=new LinkedHashMap<String,List<Map<String,Object>>>();for(String table:List.of("ItemAllocation","UOMSchedule","fed.ItemPricingSchedule"))before.put(table,jdbc.queryForList("SELECT * FROM "+table+" ORDER BY Id"));
  var fixture=jdbc.queryForMap("SELECT TOP 1 i.Id FROM Item i JOIN ItemAllocation a ON a.ItemId=i.Id WHERE i.OrganizationId=? AND i.CompanyId=? AND a.OrganizationId=i.OrganizationId AND a.CompanyId=i.CompanyId AND NOT EXISTS(SELECT 1 FROM InventoryStockEvalautionDetail d WHERE d.OrganizationId=i.OrganizationId AND d.CompanyId=i.CompanyId AND d.ItemId=i.Id) ORDER BY i.Id",u.getOrganizationId(),u.getCompanyId());
  var request=new InventoryItemCompanyAllocationRequest();request.companyId=u.getCompanyId();request.itemIds=List.of(((Number)fixture.get("Id")).intValue());
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  tx.execute(status->{try{
   assertEquals(1,service.change("deallocate",request).get("changed"));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM ItemAllocation WHERE OrganizationId=? AND CompanyId=? AND ItemId=?",Integer.class,u.getOrganizationId(),u.getCompanyId(),request.itemIds.get(0)));
   assertEquals(1,service.change("allocate",request).get("changed"));var allocation=jdbc.queryForMap("SELECT * FROM ItemAllocation WHERE OrganizationId=? AND CompanyId=? AND ItemId=?",u.getOrganizationId(),u.getCompanyId(),request.itemIds.get(0));assertEquals(u.getBranchesId(),allocation.get("BranchId"));assertEquals(true,allocation.get("IsActive"));
   assertEquals(1,service.change("deactivate",request).get("changed"));assertTrue(repo.statusRows(u,u.getCompanyId(),false).stream().anyMatch(r->Objects.equals(r.get("Id"),request.itemIds.get(0))));
   assertEquals(1,service.change("activate",request).get("changed"));assertTrue(repo.statusRows(u,u.getCompanyId(),true).stream().anyMatch(r->Objects.equals(r.get("Id"),request.itemIds.get(0))));
   request.itemIds=List.of(-1);assertThrows(IllegalArgumentException.class,()->service.change("allocate",request));return null;
  }finally{status.setRollbackOnly();}});
  for(var entry:before.entrySet())assertEquals(entry.getValue(),jdbc.queryForList("SELECT * FROM "+entry.getKey()+" ORDER BY Id"));
  Files.writeString(Path.of("migration/inventory/evidence/item-company-allocations-rollback.txt"),"ItemsAllocationToCompany: original company lookup and pending/allocated procedure rows match ("+pending.size()+"/"+allocated.size()+"). Unallocate/reallocate using a real unreferenced item, authenticated BranchId and active/inactive/reactivate passed. Invalid company/item rejected. ItemAllocation, UOMSchedule and fed.ItemPricingSchedule unchanged after rollback. Active/InActive read is a documented Java repair: desktop BLL references an activity and @Status missing in installed Sp_Item_GetAllMethod. Cross-company status correction implemented but not exercised by this single-company fixture. Rights mocked. Browser/native pending.\n");
 }
}

