package com.mst.controllers.sale;
import java.nio.file.*;
import java.util.regex.*;
import org.junit.jupiter.api.Test;
import org.springframework.ui.ExtendedModelMap;
import static org.junit.jupiter.api.Assertions.*;
class SaleMenuRoutingTest {
 @Test void desktopCardsOpenTheirSubmenus() {
  var c=new SaleModuleViewController();
  assertEquals("sale/customer_sales_menu",c.customerSales(new ExtendedModelMap()));
  assertEquals("sale/sales_reports_menu",c.saleReports(new ExtendedModelMap()));
  assertEquals("sale/customer_sales",c.customerDirectory(new ExtendedModelMap()));
 }
 @Test void outwardGatePassUsesItsDedicatedInitialApi() {
  var model=new ExtendedModelMap();
  assertEquals("sale/outward_gate_pass",new SaleModuleViewController().outwardGatePass(model));
  assertEquals(91,model.get("documentTypeId"));
  assertFalse(model.containsAttribute("suppliers"));
  assertFalse(model.containsAttribute("items"));
  assertFalse(model.containsAttribute("nextDocNo"));
 }
 @Test void submenusHaveElevenTransactionsAndFiveReports() throws Exception {
  for(var entry:java.util.Map.of("customer_sales_menu",11,"sales_reports_menu",5).entrySet()){
   String html=Files.readString(Path.of("src/main/resources/templates/sale/"+entry.getKey()+".html"));
   Matcher links=Pattern.compile("class=\"screen\" href=\"([^\"]+)\"").matcher(html);
   java.util.Set<String> urls=new java.util.HashSet<>();while(links.find())assertTrue(urls.add(links.group(1)),"Duplicate destination");
   assertEquals(entry.getValue(),urls.size());
  }
 }
}
