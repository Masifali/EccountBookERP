package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.SaleGdnStockRequest;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import static org.junit.jupiter.api.Assertions.*;

class SaleGdnReadContractTest {
 static class Db extends JdbcTemplate {
  List<String> sql=new ArrayList<>();List<Object[]> args=new ArrayList<>();
  List<Map<String,Object>> rows=List.of();
  @Override public List<Map<String,Object>> queryForList(String q,Object...a){sql.add(q);args.add(a);return rows;}
 }
 UserAccount user(){var u=new UserAccount();u.setId(42);u.setOrganizationId(78);u.setCompanyId(78);u.setBranchesId(58);return u;}
 @Test void allGdnHistoryVariantsUseTheActualScreenGrant(){
  var db=new Db();
  var repos=List.of(new SaleGdnRepository(db),new SaleGdnDirectRepository(db),new SaleGdnPurchaseReturnRepository(db));
  for(var r:repos){
   assertFalse(r.canViewAllRecords(user(),"User"));
   assertArrayEquals(new Object[]{42,r.screenName(),"User",78},db.args.get(db.args.size()-1));
   r.history(user(),58,false);assertArrayEquals(new Object[]{78,78,r.documentTypeId(),58,58,0,42},db.args.get(db.args.size()-1));
   db.rows=List.of(Map.of("RightName","CanView AllRecord","Value",true));
   assertTrue(r.canViewAllRecords(user(),"User"));
   r.history(user(),58,true);assertArrayEquals(new Object[]{78,78,r.documentTypeId(),58,58,1,null},db.args.get(db.args.size()-1));
   db.rows=List.of();assertTrue(r.canViewAllRecords(user(),"Admin"));
  }
 }
 @Test void billingAndDispatchedWeightComeFromTheTwoOriginalActivities(){
  var db=new Db();db.rows=List.of(Map.of("TotalDoWeight",2000d,"ShortExcessWeight",-100d,"SaleTypeId",2,"GrossWeight",800d));
  var result=new SaleGdnRepository(db).gatePassBilling(user(),58,323);
  assertEquals(-100d,result.get("ShortExcessWeight"));assertEquals(800d,result.get("DispatchedGrossWeight"));
  assertTrue(db.sql.get(0).contains("ReadByGpNoForGdnDeliveryOrder"));assertArrayEquals(new Object[]{78,78,58,323},db.args.get(0));
  assertTrue(db.sql.get(1).contains("GetGdnGrossWeightForValidation"));assertArrayEquals(new Object[]{78,78,323},db.args.get(1));
 }
 @Test void fifoOmitsDocumentIdentityOnInsertAndIncludesItOnUpdate(){
  var db=new Db();var repo=new SaleGdnRepository(db);
  var line=Map.<String,Object>of("ItemId",51,"ItemUomId",407,"WarehouseId",5200,"JobLotId",1,"PackingTypeId",1,"CropYear","2026-27");
  repo.fifoStocks(user(),400,false,Map.of("DocDate","2026-09-26"),line);
  assertFalse(db.sql.get(0).contains("@DocumentTypeId"));assertFalse(db.sql.get(0).contains("@Id="));
  repo.fifoStocks(user(),400,true,Map.of("DocDate","2026-09-26"),line);
  assertTrue(db.sql.get(1).endsWith("@DocumentTypeId=?,@Id=?"));
  var args=db.args.get(1);assertEquals(86,args[args.length-2]);assertEquals(400,args[args.length-1]);
 }
 @Test void generateStockUsesTheNativeFeatureSpecificParametersAndBalanceColumn(){
  var db=new Db();var repo=new SaleGdnRepository(db);
  db.rows=List.of(Map.of("BalWeight",12.25d,"NetBalWeight",9d),Map.of("BalWeight",2.5d,"NetBalWeight",1d));
  var line=new SaleGdnStockRequest.Line(51,5200,"2026-27",1,2,407);var date=LocalDate.of(2026,9,26);
  assertEquals(14.75d,repo.availableStock(user(),date,line,true));
  assertTrue(db.sql.get(0).contains("USP_GetStockByFifoMethod"));
  assertFalse(db.sql.get(0).contains("@DocumentTypeId"));assertFalse(db.sql.get(0).contains("@Id="));
  assertArrayEquals(new Object[]{78,78,51,"2026-09-26",5200,1,"2026-27",2,407},db.args.get(0));
  assertEquals(12.25d,repo.availableStock(user(),date,line,false));
  assertTrue(db.sql.get(1).contains("usp_getAvailableStock"));assertTrue(db.sql.get(1).endsWith("@ActionId=1"));
  assertFalse(db.sql.get(1).contains("@PackUomId"),"desktop sets stockUOM but non-FIFO BLL tests ItemUomId");
  assertArrayEquals(new Object[]{78,78,51,"2026-09-26",5200,1,"2026-27",2},db.args.get(1));
 }
 @Test void referencedStockReplacesClientRateAndRejectsMissingOriginalStock(){
  var db=new Db();var repo=new SaleGdnRepository(db);
  var line=new LinkedHashMap<String,Object>(Map.of("RefDocumentTypeId",57,"RefDocIdNo",900,"RefDocSubIdNo",901,"StockWeight",100d,"ItemRate",1d));
  assertThrows(IllegalStateException.class,()->repo.applyReferencedStockRate(user(),line));
  db.rows=List.of(Map.of("ItemRate",2200d,"RateUomId",407,"REquivalent",40d));
  repo.applyReferencedStockRate(user(),line);
  assertEquals(2200d,line.get("ItemRate"));assertEquals(407,line.get("RateUomId"));assertEquals(5500d,line.get("ItemAmount"));
  assertArrayEquals(new Object[]{78,78,57,900,901},db.args.get(1));
  db.sql.clear();line.put("RefDocIdNo",0);repo.applyReferencedStockRate(user(),line);assertTrue(db.sql.isEmpty());
 }
}
