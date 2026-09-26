package com.mst.repositories;
import com.mst.models.dto.InventoryStoreLedgerRequest;
import com.mst.security.*;
import com.mst.services.*;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryStoreLedgerLiveTest {
 @Test void ledgerMatchesOriginalProcedureAndRejectsUnallocatedBranch()throws Exception{
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var stockRepo=new InventoryStoreStockRepository(jdbc);var repo=new InventoryStoreLedgerRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var rights=mock(DesktopReportRights.class);var stock=new InventoryStoreStockService(stockRepo,context,rights);var service=new InventoryStoreLedgerService(repo,stock,context,rights);
  var r=new InventoryStoreLedgerRequest();r.branchIds=stockRepo.branches(u).stream().map(row->((Number)row.get("BranchId")).intValue()).toList();r.parentIds=List.of(7,8);String branches=String.join(",",r.branchIds.stream().map(String::valueOf).toList());
  var dates=jdbc.queryForMap("SELECT CONVERT(date,MIN(s.DocDate)) FirstDate,CONVERT(date,MAX(s.DocDate)) LastDate FROM InventoryStockEvalautionDetail s INNER JOIN Item i ON i.Id=s.ItemId INNER JOIN ItemCategory c ON c.Id=i.ItemCategoryId WHERE s.OrganizationId=? AND s.CompanyId=? AND c.InventoryParentCategoriesId IN (7,8)",u.getOrganizationId(),u.getCompanyId());r.fromDate=((java.sql.Date)dates.get("FirstDate")).toLocalDate();r.toDate=((java.sql.Date)dates.get("LastDate")).toLocalDate();
  var expected=ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC dbo.USP_InventoryEvaluationLedgerStore @OrganizationId=?,@CompanyId=?,@DateFrom=?,@DateTo=?,@BranchesIds=?,@Ids='7,8'",u.getOrganizationId(),u.getCompanyId(),java.sql.Date.valueOf(r.fromDate),java.sql.Date.valueOf(r.toDate),branches));var actual=service.load(r);sameRows(expected,actual);assertFalse(actual.isEmpty());int count=actual.size();
  var row=actual.stream().filter(v->v.get("ItemId") instanceof Number).findFirst().orElseThrow();r.itemId=((Number)row.get("ItemId")).intValue();r.documentTypeId=((Number)row.get("DocumentTypeId")).intValue();r.saleValue=true;
  var filtered=service.load(r);sameRows(ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC dbo.USP_InventoryEvaluationLedgerStore @OrganizationId=?,@CompanyId=?,@DateFrom=?,@DateTo=?,@BranchesIds=?,@Ids='7,8',@ItemId=?,@DocumentTypeId=?,@SaleValue=1",u.getOrganizationId(),u.getCompanyId(),java.sql.Date.valueOf(r.fromDate),java.sql.Date.valueOf(r.toDate),branches,r.itemId,r.documentTypeId)),filtered);
  var foreign=InventoryOpeningLiveTest.user(jdbc);foreign.setCompanyId(-1);assertTrue(repo.load(foreign,r).isEmpty());r.branchIds=List.of(-1);assertThrows(IllegalArgumentException.class,()->service.load(r));verify(rights,atLeastOnce()).require(u,288,"View");
  Files.writeString(Path.of("migration/inventory/evidence/store-ledger-live.txt"),"Store Item Ledger: "+count+" full rows match the original USP_InventoryEvaluationLedgerStore; item/document-type/SaleValue filter "+filtered.size()+" rows match. Foreign company returned no rows; unallocated branch rejected. Read-only test, no database writes. Rights mocked. Browser/native, voucher record navigation, grid layout and Crystal422 output remain unverified.\n");
 }
 private static void sameRows(List<Map<String,Object>> a,List<Map<String,Object>> b){assertEquals(a.size(),b.size());assertTrue(normalize(a).equals(normalize(b)),"Returned row values differ");}
 private static Map<String,Integer> normalize(List<Map<String,Object>> rows){var result=new HashMap<String,Integer>();for(var row:rows){var values=new TreeMap<String,String>();row.forEach((k,v)->values.put(k,v instanceof byte[] bytes?Base64.getEncoder().encodeToString(bytes):Objects.toString(v,"<null>")));result.merge(values.toString(),1,Integer::sum);}return result;}
}
