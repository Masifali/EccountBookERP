package com.mst.repositories;

import com.mst.models.Brand;
import com.mst.security.*;
import com.mst.services.BrandService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryBrandLiveTest {
    @Test void exactHistoryAndDisposableCrudAlwaysRollback() throws Exception {
        var jdbc=InventoryOpeningLiveTest.jdbc();var user=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopBrandRepository(jdbc);var ctx=mock(CurrentUserContext.class);when(ctx.requireAccountingUser()).thenReturn(user);var service=new BrandService(repo,ctx,mock(DesktopReportRights.class));
        var history=repo.history(user,null);var direct=jdbc.queryForList("EXEC dbo.USP_Brand_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity=?",user.getOrganizationId(),user.getCompanyId(),"FormHistory");assertEquals(direct.size(),history.size());
        for(var r:direct){var b=history.stream().filter(x->x.getId().equals(r.get("Id"))).findFirst().orElseThrow();assertEquals(r.get("BrandName"),b.getBrandName());assertEquals(r.get("BrandCode"),b.getBrandCode());assertEquals(r.get("EntryUserName"),b.getEntryUserName());assertEquals(r.get("ModifyUserName"),b.getModifyUserName());assertEquals(r.get("EntryDate")==null?null:((java.sql.Timestamp)r.get("EntryDate")).toLocalDateTime(),b.getEntryDate());assertEquals(r.get("ModifyDate")==null?null:((java.sql.Timestamp)r.get("ModifyDate")).toLocalDateTime(),b.getModifyDate());}
        var before=jdbc.queryForList("SELECT * FROM Brand WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",user.getOrganizationId(),user.getCompanyId());var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);String suffix=UUID.randomUUID().toString().substring(0,12);String code="CODEX_ROLLBACK_"+suffix;
        tx.execute(status->{try{Brand b=new Brand();b.setBrandCode(code);b.setBrandName("Test rollback "+suffix);var saved=service.addOrUpdate(b);assertEquals(user.getId(),saved.getEntryUserId());assertNotNull(saved.getEntryDate());assertEquals(user.getCompanyId(),saved.getCompanyId());saved.setBrandName("Updated rollback "+suffix);var updated=service.addOrUpdate(saved);assertEquals(saved.getBrandName(),updated.getBrandName());assertEquals(user.getId(),updated.getModifyUserId());assertNotNull(updated.getModifyDate());Brand duplicate=new Brand();duplicate.setBrandCode(code);duplicate.setBrandName("Duplicate "+suffix);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->service.addOrUpdate(duplicate));service.delete(saved.getId());assertTrue(repo.history(user,saved.getId()).isEmpty());return null;}finally{status.setRollbackOnly();}});
        assertEquals(before,jdbc.queryForList("SELECT * FROM Brand WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",user.getOrganizationId(),user.getCompanyId()));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM Brand WHERE BrandCode=?",Integer.class,code));
        Files.writeString(Path.of("migration/inventory/evidence/brand-live.txt"),"GoldenAcedb: Numan "+user.getId()+", org/company "+user.getOrganizationId()+"/"+user.getCompanyId()+"\nHistory="+history.size()+". Every displayed field (code/name/user/date) matches desktop procedure. Insert/load/update/duplicate rejection/delete of disposable test row passed; audit IDs/dates verified. Always-rolled-back transaction; all original company Brand rows unchanged. Rights mocked for DB test; separate permission/ownership tests cover service enforcement. Native comparison pending.\n");
    }
}
