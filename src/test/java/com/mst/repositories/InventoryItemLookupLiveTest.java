package com.mst.repositories;
import com.mst.models.dto.InventoryItemLookupRequest;
import com.mst.repositories.DesktopInventoryItemLookupRepository.Kind;
import com.mst.security.*;
import com.mst.services.DesktopInventoryItemLookupService;
import java.util.*;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryItemLookupLiveTest {
 @Test void cropAndGroupOriginalCrudAlwaysRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemLookupRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new DesktopInventoryItemLookupService(repo,context,mock(DesktopReportRights.class));
  var cropBefore=jdbc.queryForList("SELECT * FROM InvCropYear ORDER BY Id");var groupBefore=jdbc.queryForList("SELECT * FROM ItemGroup ORDER BY GroupId");
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
  for(var kind:Kind.values()){
   String procedure=kind==Kind.CROP_YEAR?"Sp_InvCropYear_GetAllMethod":"Sp_ItemGroup_GetAllMethod";var history=repo.history(u,kind);var direct=jdbc.queryForList("EXEC dbo."+procedure+" @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId());assertEquals(new HashSet<>(direct),new HashSet<>(history));assertEquals(direct.size(),history.size());
   tx.execute(status->{try{
    var r=new InventoryItemLookupRequest();r.name="VERIFY_"+UUID.randomUUID().toString().substring(0,12);r.defaultKey=true;var saved=service.save(kind,r);r.id=((Number)saved.get(kind==Kind.CROP_YEAR?"Id":"GroupId")).intValue();assertEquals(u.getOrganizationId(),saved.get("OrganizationId"));assertEquals(u.getCompanyId(),saved.get("CompanyId"));assertEquals(r.name,saved.get(kind==Kind.CROP_YEAR?"CropYear":"ItemGroupName"));
    if(kind==Kind.CROP_YEAR){assertEquals(u.getId(),saved.get("EntryUserId"));assertEquals(true,saved.get("DefaultKey"));}
    r.name+=" edited";r.defaultKey=false;var changed=service.save(kind,r);assertEquals(r.name,changed.get(kind==Kind.CROP_YEAR?"CropYear":"ItemGroupName"));
    if(kind==Kind.CROP_YEAR){assertEquals(false,changed.get("DefaultKey"));assertEquals(u.getId(),changed.get("ModifyUserId"));assertEquals(saved.get("EntryDate"),changed.get("EntryDate"));}
    var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,kind,r.id));r.name=" ";assertThrows(IllegalArgumentException.class,()->service.save(kind,r));return null;
   }finally{status.setRollbackOnly();}});
  }
  assertEquals(cropBefore,jdbc.queryForList("SELECT * FROM InvCropYear ORDER BY Id"));assertEquals(groupBefore,jdbc.queryForList("SELECT * FROM ItemGroup ORDER BY GroupId"));
  Files.writeString(Path.of("migration/inventory/evidence/item-lookups-rollback.txt"),"DefineCropYear and DefineItemGroup: original history rows, insert/load/update, crop default checkbox and entry/modify audit, company ownership and blank validation verified in rollback-only transactions. Original table rows unchanged after rollback. No Delete operation in either desktop form. Child rights use parent111 View; mocked in this test. Native/browser/grid-layout verification remains pending.\n");
 }
}
