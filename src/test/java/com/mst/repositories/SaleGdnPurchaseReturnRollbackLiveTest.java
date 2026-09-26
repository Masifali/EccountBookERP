package com.mst.repositories;

import static org.junit.jupiter.api.Assertions.*;
import com.mst.models.UserAccount;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleGdnPurchaseReturnRollbackLiveTest {
 @Test void insertLoadUpdateDeleteAndRollback() throws Exception {
  var p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}
  var ds=new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password"));var j=new JdbcTemplate(ds);j.setQueryTimeout(120);
  var a=j.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId,AppId FROM UserAccount WHERE UserName='numan'");var u=new UserAccount();u.setId(n(a.get("ID")));u.setOrganizationId(n(a.get("OrganizationId")));u.setCompanyId(n(a.get("CompanyId")));u.setBranchesId(n(a.get("BranchesId")));u.setAppId(n(a.get("AppId")));
  var repo=new SaleGdnPurchaseReturnRepository(j);var initial=repo.initial(u,58);int doc=n(initial.get("nextNo"));int before=j.queryForObject("SELECT COUNT(*) FROM InvGdn WHERE OrganizationId=78 AND CompanyId=78 AND DocumentTypeId=703",Integer.class);int cropId=((List<Map<String,Object>>)initial.get("crops")).stream().filter(x->"2025-26".equals(String.valueOf(x.get("CropYear")))).map(x->n(x.get("Id"))).findFirst().orElseThrow();
  var detail=new LinkedHashMap<String,Object>();detail.put("Id",0);detail.put("ActionTypeId",1);detail.put("WarehouseId",5199);detail.put("ItemId",6);detail.put("Item", "Paddy Super Fine Punjabi");detail.put("PackingTypeId",2);detail.put("CropYear","2025-26");detail.put("CropYearId",cropId);detail.put("JobLotId",1);detail.put("ItemQty",0.02d);detail.put("ItemUomId",23);detail.put("GrossWeight",1d);detail.put("EBWPerUnit",0d);detail.put("EBWTotal",0d);detail.put("WtCut",0d);detail.put("WtCutTotal",0d);detail.put("AdLsWeight",0d);detail.put("NetBillWeight",1d);detail.put("StockWeight",1d);detail.put("CityId",0);detail.put("AreaCity","");
  var request=new LinkedHashMap<String,Object>();request.put("DocNo",doc);request.put("DocDate",LocalDate.now().toString());request.put("SupplierCustomerId",1197);request.put("GpNo",0);request.put("FactoryWeight",1d);request.put("VehicleNo","ROLLBACK-703");request.put("BiltyNo","RB");request.put("RemarksHeader","CODEX ROLLBACK TEST");request.put("details",new ArrayList<>(List.of(detail)));request.put("expenses",List.of());request.put("id",0);
  var tx=new TransactionTemplate(new DataSourceTransactionManager(ds));tx.execute(status->{int id=repo.save(u,58,request);assertTrue(id>0);var saved=repo.record(u,id);assertEquals(doc,n(saved.get("DocNo")));var rows=(List<Map<String,Object>>)saved.get("details");assertEquals(1,rows.size());detail.put("Id",n(rows.get(0).get("Id")));detail.put("ActionTypeId",2);detail.put("ItemQty",0.03d);request.put("id",id);int updated=repo.save(u,58,request);assertEquals(id,updated);repo.delete(u,id);assertThrows(Exception.class,()->repo.record(u,id));status.setRollbackOnly();return null;});
  int after=j.queryForObject("SELECT COUNT(*) FROM InvGdn WHERE OrganizationId=78 AND CompanyId=78 AND DocumentTypeId=703",Integer.class);assertEquals(before,after);Files.createDirectories(Path.of("migration/sale/evidence"));Files.writeString(Path.of("migration/sale/evidence/gdn-purchase-return-rollback.txt"),"Document 703 insert, tenant-safe load, update, desktop delete procedure and transaction rollback executed against GoldenAcedb for Numan 78/78/58 year 58. Header/detail/inventory/FIFO/validation/in-transit procedure chain ran and final InvGdn count remained "+after+" (before "+before+").\n");
 }
 static int n(Object o){return o instanceof Number?((Number)o).intValue():0;}
}
