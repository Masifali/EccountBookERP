package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryUomScheduleRequest;
import com.mst.security.*;
import com.mst.services.InventoryUomScheduleService;
import java.math.BigDecimal;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryUomScheduleLiveTest {
    private static int number(Object x){return ((Number)x).intValue();}
    @Test void desktopReadsAndIsolatedWriteCycleAlwaysRollback() throws Exception {
        var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryUomScheduleRepository(jdbc);
        var items=repo.items(u);var units=repo.units(u);var history=repo.history(u,null);assertFalse(items.isEmpty());assertFalse(units.isEmpty());
        var desktop=jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());assertEquals(desktop.size(),history.size());assertEquals(new HashSet<>(desktop),new HashSet<>(history));
        // Choose an existing item/unit relationship that has never been assigned. No reference data is created.
        var fixture=jdbc.queryForMap("SELECT TOP 1 i.Id ItemId,u.Id ScheduleUnitId FROM Item i CROSS JOIN UOM u WHERE i.OrganizationId=? AND u.OrganizationId=? AND EXISTS(SELECT 1 FROM vItemAllocation a WHERE a.Id=i.Id AND a.CompanyId=? AND a.IsActive=1) AND NOT EXISTS(SELECT 1 FROM UOMSchedule s WHERE s.OrganizationId=? AND s.CompanyId=? AND s.ItemId=i.Id AND s.ScheduleUnitId=u.Id) ORDER BY i.Id,u.Id",u.getOrganizationId(),u.getOrganizationId(),u.getCompanyId(),u.getOrganizationId(),u.getCompanyId());
        int item=number(fixture.get("ItemId"));assertTrue(items.stream().anyMatch(x->number(x.get("Id"))==item));
        var before=jdbc.queryForList("SELECT * FROM UOMSchedule WHERE OrganizationId=? AND CompanyId=? AND ItemId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId(),item);
        var ctx=mock(CurrentUserContext.class);when(ctx.requireAccountingUser()).thenReturn(u);var service=new InventoryUomScheduleService(repo,ctx,mock(DesktopReportRights.class));
        var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(60);
        int[] created={0};tx.execute(status->{try{
            var r=new InventoryUomScheduleRequest();r.itemId=item;r.scheduleUnitId=number(fixture.get("ScheduleUnitId"));r.equivalent=new BigDecimal("12.3456");r.qtyEquivalent=new BigDecimal("2.5");r.active=true;r.baseRateUom=r.basePackUom=r.baseSecondaryUom=true;
            var saved=service.save(r);created[0]=number(saved.get("Id"));r.id=created[0];assertEquals(u.getId(),number(saved.get("EntryUser")));assertEquals(u.getCompanyId(),number(saved.get("CompanyId")));assertEquals(r.equivalent.doubleValue(),((Number)saved.get("Equivalent")).doubleValue(),0.000001);
            for(String flag:List.of("BaseRateUom","BasePackUom","BaseSecondaryUom"))assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM UOMSchedule WHERE CompanyId=? AND ItemId=? AND "+flag+"=1",Integer.class,u.getCompanyId(),item));
            r.equivalent=new BigDecimal("24.5678");r.active=false;var changed=service.save(r);assertEquals(false,changed.get("Active"));assertEquals(u.getId(),number(changed.get("ModifyUser")));assertEquals(r.equivalent.doubleValue(),((Number)changed.get("Equivalent")).doubleValue(),0.000001);
            // Desktop Insert reactivates an inactive duplicate and retains its prior equivalents.
            r.id=0;r.active=true;r.equivalent=BigDecimal.ONE;var reactivated=service.save(r);assertEquals(created[0],number(reactivated.get("Id")));assertEquals(true,reactivated.get("Active"));assertEquals(24.5678,((Number)reactivated.get("Equivalent")).doubleValue(),0.000001);
            assertThrows(org.springframework.dao.DataAccessException.class,()->service.save(r));
            var other=new UserAccount();other.setOrganizationId(u.getOrganizationId());other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,created[0]));
            return null;
        }finally{status.setRollbackOnly();}});
        assertEquals(before,jdbc.queryForList("SELECT * FROM UOMSchedule WHERE OrganizationId=? AND CompanyId=? AND ItemId=? ORDER BY Id",u.getOrganizationId(),u.getCompanyId(),item));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM UOMSchedule WHERE Id=?",Integer.class,created[0]));
        Files.writeString(Path.of("migration/inventory/evidence/uom-schedule-live.txt"),"GoldenAcedb: user="+u.getId()+" org="+u.getOrganizationId()+" company="+u.getCompanyId()+"\nItems="+items.size()+" Units="+units.size()+" History="+history.size()+"\nHistory all columns match desktop procedure. Insert/load/update/deactivate/reactivate/duplicate error and base-flag uniqueness verified inside always-rolled-back transaction. Original rows and base flags identical afterward. Cross-company record load denied. Rights mocked in this DB test; separate denial contract test covers service checks. No desktop Delete operation. Native and browser comparison pending.\n");
    }
}
