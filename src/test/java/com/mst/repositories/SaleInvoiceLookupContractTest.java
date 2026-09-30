package com.mst.repositories;
import com.mst.models.UserAccount;
import org.junit.jupiter.api.Test;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
class SaleInvoiceLookupContractTest {
 static class Repo extends SaleInvoiceRepository {
  List<Map<String,Object>> rows=List.of(); String sql,showBoth="0"; Object[] args;
  Repo(){super(null);}
  @Override public List<Map<String,Object>> q(String s,Object... a){sql=s;args=a;return rows;}
  @Override public String config(UserAccount u,String key){assertEquals("ShowBothVendorAndCustomerOnSalesPurchase",key);return showBoth;}
 }
 UserAccount user(){var u=new UserAccount();u.setOrganizationId(78);u.setCompanyId(78);return u;}
 Map<String,Object> party(int id,int group,int type){return Map.of("Id",id,"CustomerGroupId",group,"PartyTypeId",type,"CompanyName","Party "+id,"CityName","City "+id,"MobilePersonal","123","GlAccountId",100+id);}
 @Test void customerFilterMatchesDesktopGlobalListAndSubsidiaryFeature(){
  var r=new Repo();r.rows=List.of(party(1,1,2),party(2,7,2),party(3,9,2),party(4,10,2),party(5,8,2),party(6,13,2),party(7,1,1));
  assertEquals(List.of(1,5,6,7),r.customers(user(),false).stream().map(x->x.get("Id")).toList());
  r.showBoth="1";var rows=r.customers(user(),true);
  assertEquals(1,rows.size());assertEquals("City 1",rows.get(0).get("CityName"));assertEquals("123",rows.get(0).get("MobileNo"));
  assertEquals("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?",r.sql);assertArrayEquals(new Object[]{78,78},r.args);
 }
 @Test void currencyRateUsesOriginalProcedureAndDocumentScope(){
  var r=new Repo();r.rows=List.of(Map.of("LastExchRate",280.5));
  for(int doc:List.of(95,171)){
   assertEquals(280.5,r.lastExchangeRate(user(),3,doc));
   assertTrue(r.sql.contains("Sp_Vouchers_GetMethods"));assertArrayEquals(new Object[]{78,78,String.valueOf(doc),"3","GetMultiCurrencyAndLastRate"},r.args);
  }
 }
 @Test void historyDropdownUsesOnlySupplierRowsFromGdn(){
  var r=new Repo();r.rows=List.of(Map.of("Activity","Supplier","Id",4,"ReferenceName","Customer A"),Map.of("Activity","Warehouse","Id",9,"ReferenceName","Warehouse A"));
  assertEquals(List.of(Map.of("Id",4,"Customer","Customer A")),r.customersFromGdn(user()));
  assertTrue(r.sql.contains("USP_GetDataForDropDownFromGdn"));assertArrayEquals(new Object[]{78,78},r.args);
 }
}
