package com.mst.repositories;
import java.util.*;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import static org.junit.jupiter.api.Assertions.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryGeneralItemReadLiveTest {
 private static void sameRows(List<Map<String,Object>> expected,Object actual){var rows=(List<?>)actual;assertEquals(expected.size(),rows.size());for(var row:expected)assertEquals(Collections.frequency(expected,row),Collections.frequency(rows,row));}
 @SuppressWarnings("unchecked") @Test void originalGeneralItemLookupsAndHistory() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemRepository(jdbc,new InventoryPosItemRepository(jdbc));var choices=repo.lookups(u);
  sameRows(jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Ids=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"1,2,3,4,6,10,11","ReadByOrganizationCompanyId"),choices.get("categories"));
  sameRows(jdbc.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId"),choices.get("suppliers"));
  var rows=repo.history(u,true,50,0,0,0);assertEquals(jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='FormHistory', @CanViewAllRecord=1, @IsTaxable=0, @ScreenName='InvDefrmAddItem', @NoOfRecords=50, @ParentIds='1,2,3,4,6,10,11'",u.getOrganizationId(),u.getCompanyId()),rows);
  var restricted=repo.history(u,false,50,0,0,0);assertEquals(jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='FormHistory', @CanViewAllRecord=0, @EntryUser=?, @IsTaxable=0, @ScreenName='InvDefrmAddItem', @NoOfRecords=50, @ParentIds='1,2,3,4,6,10,11'",u.getOrganizationId(),u.getCompanyId(),u.getId()),restricted);
  assertFalse(rows.isEmpty());int id=((Number)rows.get(0).get("Id")).intValue();var details=repo.details(u,id);var item=(Map<String,Object>)details.get("item");assertEquals(u.getCompanyId(),item.get("CompanyId"));repo.defaults(u,((Number)item.get("ItemCategoryId")).intValue(),((Number)item.get("ItemTypeId")).intValue());
  var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,id));
  var counts=new TreeMap<String,Integer>();choices.forEach((key,value)->{if(value instanceof List<?> list)counts.put(key,list.size());});
  Files.writeString(Path.of("migration/inventory/evidence/general-item-read.txt"),"Define Item111 read contract: original categories/suppliers and all/restricted history match. History "+rows.size()+", restricted "+restricted.size()+". Record/load, code/default query and company denial passed. Lookups "+counts+". No writes in this test; main save/update/images/children and UI/runtime remain in progress.\n");
 }
}

