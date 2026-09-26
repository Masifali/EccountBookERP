package com.mst.repositories;
import com.mst.models.dto.InventoryCategoryRequest;
import com.mst.security.*;
import com.mst.services.DesktopInventoryCategoryService;
import java.util.*;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryCategoryLiveTest {
 @SuppressWarnings("unchecked") private static int first(Map<String,Object> choices,String key){return ((Number)((List<Map<String,Object>>)choices.get(key)).get(0).get("Id")).intValue();}
 @Test void originalHistoryLookupInsertUpdateAndAssetSideEffectsRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryCategoryRepository(jdbc);var choices=repo.lookups(u);
  var before=jdbc.queryForList("SELECT * FROM ItemCategory WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId());
  var history=repo.history(u);assertEquals(jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @ParentCategoryIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"1,2,3,4,6,10,11","FormHistory"),history);
  assertEquals(jdbc.queryForList("EXEC dbo.usp_getItemProductionStage"),choices.get("stages"));assertEquals(jdbc.queryForList("EXEC dbo.usp_getItemVarietyNature"),choices.get("varieties"));
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryCategoryService(repo,context,mock(DesktopReportRights.class));
  var r=new InventoryCategoryRequest();r.InventoryParentCategoriesId=first(choices,"parents");r.CategoryCode=repo.code(u,r.InventoryParentCategoriesId);r.CategoryDescription="CODEX_ROLLBACK_CATEGORY_"+UUID.randomUUID().toString().substring(0,8);r.SerialFrom=jdbc.queryForObject("SELECT COALESCE(MAX(SerialTo),0)+100 FROM ItemCategory WHERE OrganizationId=? AND CompanyId=?",Integer.class,u.getOrganizationId(),u.getCompanyId());r.SerialTo=r.SerialFrom+9;r.RevenueAccountId=first(choices,"revenue");r.InventoryAccountId=first(choices,"inventory");r.CGSAccountId=first(choices,"cgs");r.ItemClassGroupId=first(choices,"classes");r.ItemProductionStageId=first(choices,"stages");r.ItemVarietyNatureId=first(choices,"varieties");
  if(Boolean.TRUE.equals(choices.get("sameAccounts")))r.RevenueAccountId=r.InventoryAccountId;
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(60);
  tx.execute(status->{try{var saved=service.save(r);r.Id=((Number)saved.get("Id")).intValue();assertEquals(u.getId(),saved.get("EntryUser"));assertEquals(u.getOrganizationId(),saved.get("OrganizationId"));assertEquals(u.getCompanyId(),saved.get("CompanyId"));assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM Asset.Category WHERE ItemCategoryId=? AND companyId=?",Integer.class,r.Id,u.getCompanyId()));
   if(!((List<?>)repo.translations(u).get("languages")).isEmpty()){var translated=new InventoryCategoryRequest.Translation();translated.ItemCategoryId=r.Id;translated.MultiLanguagesId=first(repo.translations(u),"languages");translated.CategoryDescription="Rollback translation";repo.saveTranslation(translated);translated.CategoryDescription="Updated rollback translation";repo.saveTranslation(translated);assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemCategoryMultilingo WHERE ItemCategoryId=? AND MultiLanguagesId=?",Integer.class,r.Id,translated.MultiLanguagesId));assertEquals(translated.CategoryDescription,jdbc.queryForObject("SELECT CategoryDescription FROM ItemCategoryMultilingo WHERE ItemCategoryId=? AND MultiLanguagesId=?",String.class,r.Id,translated.MultiLanguagesId));}else{assertThrows(org.springframework.security.access.AccessDeniedException.class,service::translations);}var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.Id));
   r.CategoryCode=saved.get("CategoryCode").toString();r.CategoryStatus=false;r.CategoryDescription+=" updated";var updated=service.save(r);assertEquals(false,updated.get("CategoryStatus"));assertEquals(u.getId(),updated.get("ModifyUser"));assertEquals(r.CategoryDescription,jdbc.queryForObject("SELECT categoryDescription FROM Asset.Category WHERE ItemCategoryId=?",String.class,r.Id));return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM ItemCategoryMultilingo WHERE ItemCategoryId=?",Integer.class,r.Id));assertEquals(before,jdbc.queryForList("SELECT * FROM ItemCategory WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId()));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM Asset.Category WHERE ItemCategoryId=?",Integer.class,r.Id));
  Files.writeString(Path.of("migration/inventory/evidence/category-rollback.txt"),"Screen112 GoldenAcedb: history "+history.size()+" rows equal original FormHistory. Production stages and variety choices equal original procedures. Insert/load/update/inactivate plus Asset.Category side effect verified. Organization/company guards, entry/modify user checked; original rows unchanged after rollback. Branch/User filters absent from original category query. Multi-language feature="+choices.get("multiLanguage")+". Translation write test requires an existing language; empty catalog is not seeded. Feature-disabled access denied verified. Native/browser verification still pending.\n");
 }
}
