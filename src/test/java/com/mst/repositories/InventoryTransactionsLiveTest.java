package com.mst.repositories;
import com.mst.models.dto.InventoryTransactionsRequest;
import java.nio.file.*;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import static org.junit.jupiter.api.Assertions.*;
@EnabledIfSystemProperty(named="inventory.live",matches="true")
class InventoryTransactionsLiveTest {
 @Test void reportAndLookupsMatchOriginalDesktopQueries()throws Exception{
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryTransactionsRepository(jdbc);var lookups=repo.lookups(u);
  equalRows(jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()),(List<Map<String,Object>>)lookups.get("choices"));
  equalRows(jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @Activity='DocumentTypeGetFromInventoryTransactions'",u.getOrganizationId(),u.getCompanyId()),(List<Map<String,Object>>)lookups.get("documents"));
  var last=jdbc.queryForObject("SELECT CONVERT(date,MAX(DocDate)) FROM InventoryTransactions WHERE OrganizationId=? AND CompanyId=?",java.sql.Date.class,u.getOrganizationId(),u.getCompanyId());assertNotNull(last);LocalDate to=last.toLocalDate(),from=to.withDayOfMonth(1);var r=new InventoryTransactionsRequest();r.setFromDate(from);r.setToDate(to);
  var expected=ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC pcc.[USP-InventoryStockTransactionsReport] @OrganizationId=?, @CompanyId=?, @DateFrom=?, @DateTo=?",u.getOrganizationId(),u.getCompanyId(),java.sql.Date.valueOf(from),last));var rows=repo.load(u,r);equalRows(expected,rows);assertFalse(rows.isEmpty());
  int item=((Number)rows.get(0).get("ItemId")).intValue();r.setItemId(item);equalRows(ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC pcc.[USP-InventoryStockTransactionsReport] @OrganizationId=?, @CompanyId=?, @DateFrom=?, @DateTo=?, @ItemId=?",u.getOrganizationId(),u.getCompanyId(),java.sql.Date.valueOf(from),last,item)),repo.load(u,r));
  var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertTrue(repo.load(other,r).isEmpty());
  Files.writeString(Path.of("migration/inventory/evidence/inventory-transactions-live.txt"),"Screen580 Inventory Stock Transactions: original dropdown/document queries and all report columns match for "+from+" to "+to+"; "+rows.size()+" rows. Item filter and foreign-company empty result verified. Organization/company taken from Numan's DB context. Original report has no branch/user-row parameter. No database writes. User580 rights/browser/native/417 output and document links remain pending.\n");
 }
 private static void equalRows(List<Map<String,Object>> expected,List<Map<String,Object>> actual){assertEquals(expected.size(),actual.size());for(int i=0;i<expected.size();i++){assertEquals(expected.get(i).keySet(),actual.get(i).keySet());for(String key:expected.get(i).keySet())assertTrue(Objects.deepEquals(expected.get(i).get(key),actual.get(i).get(key)),"Row "+i+" column "+key+" differs");}}
}
