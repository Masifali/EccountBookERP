package com.mst.repositories;

import com.mst.models.dto.InventoryItemTaxScheduleRequest;
import com.mst.security.*;
import com.mst.services.DesktopInventoryItemTaxScheduleService;
import java.nio.file.*;
import java.time.LocalDateTime;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryItemTaxScheduleLiveTest {
 @Test void originalQueriesAndWritesMatchWithinRollback() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemTaxScheduleRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var rights=mock(DesktopReportRights.class);var service=new DesktopInventoryItemTaxScheduleService(repo,context,rights);
  var before=jdbc.queryForList("SELECT * FROM ItemTaxSchedule ORDER BY Id");var rows=service.history(0,0,0,0,null,null);assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadByOrganizationCompanyId'",u.getOrganizationId(),u.getCompanyId())),new HashSet<>(rows));
  var items=repo.items(u);var taxes=repo.taxes(u);assertFalse(items.isEmpty());assertFalse(taxes.isEmpty());assertEquals(jdbc.queryForList("EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=2, @Activity='ReadByCombo'",u.getOrganizationId(),u.getCompanyId()),taxes);
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(120);var updated=new boolean[]{false};
  tx.execute(status->{try{
   var r=new InventoryItemTaxScheduleRequest();r.itemId=num(items.get(0).get("Id"));r.taxTypeId=num(taxes.get(0).get("Id"));
   // Prefer a real tax which the original Update procedure permits; no reference data is created.
   for(var tax:taxes){int t=num(tax.get("Id"));int references=jdbc.queryForObject("SELECT (SELECT COUNT(*) FROM VoucherHead h JOIN VoucherDetail d ON h.Id=d.VoucherHeadId WHERE h.OrganizationId=? AND h.CompanyId=? AND d.TaxTypeId=?) + (SELECT COUNT(*) FROM PurchaseOrder h JOIN PurchaseOrderDetail d ON h.Id=d.PurchaseOrderId WHERE h.OrganizationId=? AND h.CompanyId=? AND d.TaxNameId=?) + (SELECT COUNT(*) FROM InvPurchaseInvoice h JOIN InvPurchaseInvoiceDetail d ON h.Id=d.InvPurchaseInvoiceId WHERE h.OrganizationId=? AND h.CompanyId=? AND d.TaxNameId=?)",Integer.class,u.getOrganizationId(),u.getCompanyId(),t,u.getOrganizationId(),u.getCompanyId(),t,u.getOrganizationId(),u.getCompanyId(),t);if(references==0){r.taxTypeId=t;updated[0]=true;break;}}
   var maximum=jdbc.queryForObject("SELECT MAX(d) FROM (SELECT MAX(EffectedDate) d FROM ItemTaxSchedule WHERE OrganizationId=? AND CompanyId=? UNION ALL SELECT MAX(EffectedDate) FROM TaxScheduleMain WHERE OrganizationId=? AND CompanyId=?) dates",java.sql.Timestamp.class,u.getOrganizationId(),u.getCompanyId(),u.getOrganizationId(),u.getCompanyId());
   r.effectedDate=(maximum==null?LocalDateTime.now():maximum.toLocalDateTime()).plusDays(1).withHour(12).withMinute(15).withSecond(0).withNano(0);r.active=true;
   var saved=service.save(r);r.id=num(saved.get("Id"));assertEquals(r.itemId,saved.get("ItemId"));assertEquals(r.taxTypeId,saved.get("TaxTypeId"));assertEquals(u.getId(),saved.get("EntryUser"));assertEquals(u.getOrganizationId(),saved.get("OrganizationId"));assertEquals(u.getCompanyId(),saved.get("CompanyId"));assertEquals(true,saved.get("IsActive"));assertEquals(r.effectedDate,((java.sql.Timestamp)saved.get("EffectedDate")).toLocalDateTime());
   var originalEntry=saved.get("EntryDate");r.active=false;r.effectedDate=r.effectedDate.plusDays(1);
   if(updated[0]){var changed=service.save(r);assertEquals(false,changed.get("IsActive"));assertEquals(u.getId(),changed.get("ModifyUser"));assertEquals(originalEntry,changed.get("EntryDate"));assertEquals(1,service.history(0,0,r.itemId,r.taxTypeId,r.effectedDate.toLocalDate(),false).size());}
   else{assertThrows(org.springframework.dao.DataAccessException.class,()->service.save(r));assertEquals(true,repo.record(u,r.id).get("IsActive"));}
   var bulk=new InventoryItemTaxScheduleRequest();bulk.sourceId=r.id;bulk.itemId=r.itemId;bulk.taxTypeId=r.taxTypeId;bulk.effectedDate=r.effectedDate.plusDays(1);bulk.active=true;
   assertEquals(1,service.allocate(List.of(bulk)).get("inserted"));var snapshot=jdbc.queryForList("SELECT * FROM ItemTaxSchedule ORDER BY Id");assertThrows(org.springframework.dao.DataAccessException.class,()->service.allocate(List.of(bulk)));assertEquals(snapshot,jdbc.queryForList("SELECT * FROM ItemTaxSchedule ORDER BY Id"));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.id));
   bulk.sourceId=-1;assertThrows(IllegalArgumentException.class,()->service.allocate(List.of(bulk)));bulk.taxTypeId=-1;assertThrows(IllegalArgumentException.class,()->service.save(bulk));verify(rights,atLeastOnce()).require(u,176,"Save");verify(rights,atLeastOnce()).require(u,176,"Update");return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM ItemTaxSchedule ORDER BY Id"));
  Files.writeString(Path.of("migration/inventory/evidence/item-tax-schedules-rollback.txt"),"ItemTaxSchedule screen176: original history "+rows.size()+" rows and tax dropdown match; insert/load, scope/audit/effected datetime, "+(updated[0]?"update/inactivate":"original procedure update rejection (all existing taxes are referenced)")+", selected-row allocation and duplicate rejection verified. Cross-company and invalid reference denied. Original ItemTaxSchedule rows unchanged after rollback. Rights mocked for write tests. Browser/native/saved layouts remain pending.\n");
 }
 private static int num(Object value){return ((Number)value).intValue();}
}
