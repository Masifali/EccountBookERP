package com.mst.repositories;
import com.mst.models.dto.InventoryItemLanguageRequest;
import com.mst.security.*;
import com.mst.services.DesktopInventoryItemLanguageService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryItemLanguageLiveTest {
 @Test void sourceLookupsAndTranslationContractUseRealReferences() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemLanguageRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryItemLanguageService(repo,context,mock(DesktopReportRights.class));var items=repo.items(u);var languages=repo.languages(u);assertFalse(items.isEmpty());assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAllItemsIncludedPM'",u.getOrganizationId(),u.getCompanyId())),new HashSet<>(items));assertEquals(jdbc.queryForList("EXEC dbo.Sp_MultiLanguages_GetAll @MethodType='ReadAll', @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()),languages);
  var owned=new HashSet<Integer>(jdbc.queryForList("SELECT i.Id FROM Item i WHERE i.OrganizationId=? AND EXISTS(SELECT 1 FROM ItemAllocation a WHERE a.ItemId=i.Id AND a.OrganizationId=? AND a.CompanyId=?)",Integer.class,u.getOrganizationId(),u.getOrganizationId(),u.getCompanyId()));var expected=jdbc.queryForList("EXEC dbo.USP_ItemMultiLingo_ReadAll").stream().filter(r->owned.contains(((Number)r.get("ItemId")).intValue())).toList();var history=service.history();assertEquals(expected.size(),history.size());for(var row:expected)assertTrue(history.stream().anyMatch(actual->row.entrySet().stream().allMatch(e->Objects.equals(e.getValue(),actual.get(e.getKey())))));
  var before=jdbc.queryForList("SELECT * FROM ItemMultiLingo ORDER BY Id");var r=new InventoryItemLanguageRequest();r.itemId=((Number)items.get(0).get("Id")).intValue();r.languageId=-1;r.title="Inventory rollback translation";assertThrows(IllegalArgumentException.class,()->service.save(r));
  if(!languages.isEmpty()) {r.languageId=((Number)languages.get(0).get("Id")).intValue();var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.execute(status->{try{var saved=service.save(r);r.id=((Number)saved.get("Id")).intValue();assertEquals(r.title,saved.get("OtherLanguageTitle"));r.title="Inventory rollback translation updated";assertEquals(r.title,service.save(r).get("OtherLanguageTitle"));assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemMultiLingo WHERE ItemId=? AND MultiLanguagesId=?",Integer.class,r.itemId,r.languageId));return null;}finally{status.setRollbackOnly();}});}
  assertEquals(before,jdbc.queryForList("SELECT * FROM ItemMultiLingo ORDER BY Id"));Files.writeString(Path.of("migration/inventory/evidence/item-languages.txt"),"ItemMultiLanguage: original item lookup "+items.size()+" rows, language lookup "+languages.size()+" rows and scoped history "+history.size()+" match. Invalid language rejected. "+(languages.isEmpty()?"Insert/update verification blocked: original language catalog is empty; no reference rows invented.":"Insert/load/update and replacement-per-item-language passed in rollback.")+" Original ItemMultiLingo unchanged. Rights mocked; browser/native pending.\n");
 }
}
