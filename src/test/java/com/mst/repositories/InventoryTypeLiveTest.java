package com.mst.repositories;
import com.mst.models.dto.InventoryTypeRequest;
import com.mst.services.DesktopInventoryTypeService;
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
class InventoryTypeLiveTest {
 @SuppressWarnings("unchecked") private static int first(Map<String,Object> choices,String key){return ((Number)((List<Map<String,Object>>)choices.get(key)).get(0).get("Id")).intValue();}
 @Test void originalQueriesAndWritesAlwaysRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryTypeRepository(jdbc);var choices=repo.lookups(u);var history=repo.history(u);
  assertEquals(jdbc.queryForList("EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @ParentCategoryIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"1,2,4,6,10,11","ReadByOrganizationCompanyId").stream().filter(r->Set.of(15,16,18).contains(r.get("Type"))).toList(),history);
  var before=jdbc.queryForList("SELECT * FROM ItemType WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryTypeService(repo,context,mock(DesktopReportRights.class));var r=new InventoryTypeRequest();r.type=first(choices,"itemTypes");r.parentCategoryId=first(choices,"parentCategories");r.typeCode=repo.code(u);r.typeDescription="CODEX_ROLLBACK_TYPE_"+UUID.randomUUID().toString().substring(0,8);r.isMother=true;
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(60);
  tx.execute(status->{try{var row=service.save(r);r.id=((Number)row.get("Id")).intValue();assertEquals(u.getId(),row.get("EntryUser"));assertNotNull(row.get("EntryDate"));assertEquals(true,row.get("IsMother"));assertEquals(false,row.get("PostState"));assertEquals(0,row.get("PostUser"));assertNull(row.get("PostDate"));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.id));r.typeCode=row.get("TypeCode").toString();r.typeDescription+=" updated";r.isMother=false;var changed=service.save(r);assertEquals(false,changed.get("IsMother"));assertEquals(u.getId(),changed.get("ModifyUser"));assertEquals(row.get("EntryDate"),changed.get("EntryDate"));assertEquals(r.id,changed.get("Id"));return null;
  }finally{status.setRollbackOnly();}});assertEquals(before,jdbc.queryForList("SELECT * FROM ItemType WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));
  Files.writeString(Path.of("migration/inventory/evidence/item-type-rollback.txt"),"Inventory113 FormTypeId0: "+history.size()+" history rows match original procedure plus desktop type filter. Database parent/type lookup restrictions used. Insert/load/update Mother flag, generated ID, audit dates/users, default posting state and cross-company guard verified. All original rows identical after rollback. Browser/native and multi-language child pending.\n");
 }
}
