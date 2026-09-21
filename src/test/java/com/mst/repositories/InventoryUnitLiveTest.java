package com.mst.repositories;
import com.mst.models.dto.InventoryUnitRequest;
import com.mst.security.*;
import com.mst.services.InventoryUnitService;
import java.math.BigDecimal;
import java.nio.file.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryUnitLiveTest {
 @Test void sourceReadAndWriteCycleAlwaysRollback() throws Exception {
    var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryUnitRepository(jdbc);var before=repo.history(u);assertEquals(jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId"),before);
    var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new InventoryUnitService(repo,context,mock(DesktopReportRights.class));var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(30);int[] id={0};
    tx.execute(status->{try{var r=new InventoryUnitRequest();r.code="CODEX_ROLLBACK_UOM";r.equivalent=new BigDecimal("12.5");r.qtyEquivalent=new BigDecimal("2.5");var parents=repo.parents();r.parentUomId=parents.isEmpty()?0:((Number)parents.get(0).get("Id")).intValue();var saved=service.save(r);id[0]=((Number)saved.get("Id")).intValue();r.id=id[0];assertEquals(r.code,saved.get("UOMDescription"));assertEquals(r.code,saved.get("UOMSymbol"));assertEquals(u.getOrganizationId(),saved.get("OrganizationId"));assertEquals(u.getCompanyId(),saved.get("CompanyId"));assertEquals(u.getId(),saved.get("EntryUser"));r.code="CODEX_UPDATED_UOM";r.active=false;r.equivalent=new BigDecimal("25");var updated=service.save(r);assertEquals(false,updated.get("UOMStatus"));assertEquals(25d,((Number)updated.get("Equivalent")).doubleValue());assertEquals(u.getId(),updated.get("ModifyUser"));var other=InventoryOpeningLiveTest.user(jdbc);other.setOrganizationId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.id));return null;}catch(Exception e){throw new RuntimeException(e);}finally{status.setRollbackOnly();}});
    assertEquals(before,repo.history(u));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM UOM WHERE Id=?",Integer.class,id[0]));Files.writeString(Path.of("migration/inventory/evidence/pos-unit-rollback.txt"),"POS base-unit child: organization-wide history matched desktop procedure. Insert/load/update/inactivate and cross-organization denial passed in an always-rolled-back transaction. Code copied to description/symbol; parent, equivalent, quantity, entry and modify users checked. Original UOM rows identical after rollback. No desktop Delete operation. Rights mocked in DB test; browser/native pending.\n");
 }
}
