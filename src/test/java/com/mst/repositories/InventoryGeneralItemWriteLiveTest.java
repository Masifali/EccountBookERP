package com.mst.repositories;

import com.mst.models.dto.*;
import com.mst.security.*;
import com.mst.services.*;
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
class InventoryGeneralItemWriteLiveTest {
 @SuppressWarnings("unchecked") @org.junit.jupiter.params.ParameterizedTest @org.junit.jupiter.params.provider.ValueSource(booleans={false,true}) void originalSaveUpdateChildrenAndBulkNamesRollback(boolean groupSchedule) throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopInventoryItemRepository(jdbc,new InventoryPosItemRepository(jdbc));var choices=repo.lookups(u);
  var fixture=jdbc.queryForMap("SELECT TOP 1 c.Id CategoryId,i.ItemTypeId,i.ItemClassId,i.BaseUnitId,i.PurchaseGLAC,i.SaleGLAC,i.COGSGLAC FROM ItemCategory c JOIN ChartofAccount s ON s.Id=c.InventoryAccountId AND s.Account_Level=3 AND s.AccountGroup='Group' JOIN ChartofAccount a ON a.Id=c.RevenueAccountId AND a.Account_Level=3 AND a.AccountGroup='Group' JOIN Item i ON i.ItemCategoryId=c.Id AND i.CompanyId=c.CompanyId JOIN ChartofAccount g ON g.Id=i.COGSGLAC AND g.AccountGroup='Detail' WHERE c.OrganizationId=? AND c.CompanyId=? AND c.InventoryParentCategoriesId IN(1,2,3,4,6,10,11) AND i.ItemClassId IN(1,6,7,8) ORDER BY i.Id",u.getOrganizationId(),u.getCompanyId());
  var r=new InventoryGeneralItemRequest();r.ItemCategoryId=num(fixture.get("CategoryId"));r.ItemTypeId=num(fixture.get("ItemTypeId"));r.ItemClassId=num(fixture.get("ItemClassId"));r.BaseUnitId=num(fixture.get("BaseUnitId"));r.PurchaseGLAC=num(fixture.get("PurchaseGLAC"));r.SaleGLAC=num(fixture.get("SaleGLAC"));r.COGSGLAC=num(fixture.get("COGSGLAC"));
  r.MotherItemId=num(((List<Map<String,Object>>)choices.get("mothers")).get(0).get("Id"));r.ItemName="VERIFY_GENERAL_"+UUID.randomUUID().toString().substring(0,8);r.ItemCode="1";r.ItemCodeNew=r.ItemName;r.ItemStatus=true;r.IsCompany=true;r.PackQty=BigDecimal.ONE;r.WholeSalePrice=new BigDecimal("12.5");r.companies=List.of(u.getCompanyId());r.CommOnPurchase=true;r.CommOnSale=true;
  if(groupSchedule)r.ItemGroupId=jdbc.queryForObject("SELECT TOP 1 g.GroupId FROM ItemGroup g JOIN ItemGroup_UOMSchedule s ON s.ItemGroupId=g.GroupId WHERE g.OrganizationId=? AND g.CompanyId=? ORDER BY g.GroupId",Integer.class,u.getOrganizationId(),u.getCompanyId());
  var expectedGroup=groupSchedule?jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @ItemGroupId=?, @Activity='ReadByItemGroupId'",r.ItemGroupId):List.<Map<String,Object>>of();
  var taxes=(List<Map<String,Object>>)choices.get("taxes");assertFalse(taxes.isEmpty(),"Use existing tax reference data");r.ApplyGST=true;r.TaxTypeId=num(taxes.get(0).get("Id"));
  var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var storage=mock(DesktopAttachmentStore.class);String stored="verify_"+UUID.randomUUID()+".png";when(storage.store(any(),anyString(),any(byte[].class))).thenReturn(stored);
  var png=new java.io.ByteArrayOutputStream();javax.imageio.ImageIO.write(new java.awt.image.BufferedImage(2,2,java.awt.image.BufferedImage.TYPE_INT_RGB),"png",png);var image=new InventoryGeneralItemRequest.Image();image.sortNo=1;image.upload=new InventoryPosItemRequest.Upload();image.upload.name="verify-image.png";image.upload.base64=Base64.getEncoder().encodeToString(png.toByteArray());r.images=List.of(image);r.imageDate=java.time.LocalDateTime.now().withNano(0);
  var upload=new InventoryPosItemRequest.Upload();upload.name="verify.txt";upload.base64=Base64.getEncoder().encodeToString("rollback fixture".getBytes(java.nio.charset.StandardCharsets.UTF_8));r.files=List.of(upload);
  var service=new DesktopInventoryItemService(repo,new DesktopInventoryItemWriter(jdbc),new DesktopInventoryItemFileService(storage,jdbc,repo),new InventoryOpeningRepository(jdbc),context,mock(DesktopReportRights.class));
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(180);
  var accountIds=new ArrayList<Integer>();
  tx.execute(status->{try{
   var saved=(Map<String,Object>)service.save(r).get("item");r.Id=num(saved.get("Id"));assertEquals(u.getBranchesId(),saved.get("BranchesId"));assertEquals(u.getId(),saved.get("EntryUser"));assertEquals(u.getOrganizationId(),saved.get("OrganizationId"));assertEquals(u.getCompanyId(),saved.get("CompanyId"));assertEquals(12.5d,((Number)saved.get("WholeSalePrice")).doubleValue());assertEquals(0d,((Number)saved.get("PurchasePrice")).doubleValue());
   for(String key:List.of("PurchaseGLAC","SaleGLAC")){int id=num(saved.get(key));accountIds.add(id);assertEquals(u.getBranchesId(),jdbc.queryForObject("SELECT BranchId FROM ChartofAccount WHERE Id=?",Integer.class,id));assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM COAAllocation WHERE ChartofAccountId=? AND CompanyId=? AND BranchId=?",Integer.class,id,u.getCompanyId(),u.getBranchesId()));}
   assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemAllocation WHERE ItemId=? AND CompanyId=? AND BranchId=?",Integer.class,r.Id,u.getCompanyId(),u.getBranchesId()));
   var expectedUnits=new HashSet<Integer>(List.of(r.BaseUnitId,0));for(var unit:expectedGroup){int unitId=num(unit.get("UOMId"));expectedUnits.add(unitId);assertEquals(Double.parseDouble(unit.get("Equivalent").toString()),jdbc.queryForObject("SELECT Equivalent FROM UOMSchedule WHERE ItemId=? AND ScheduleUnitId=?",Double.class,r.Id,unitId));}assertEquals(expectedUnits.size(),jdbc.queryForObject("SELECT COUNT(*) FROM UOMSchedule WHERE ItemId=?",Integer.class,r.Id));assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM UOMSchedule WHERE ItemId=? AND ScheduleUnitId=?",Integer.class,r.Id,r.BaseUnitId));assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemTaxSchedule WHERE ItemId=? AND TaxTypeId=?",Integer.class,r.Id,r.TaxTypeId));
   var price=jdbc.queryForMap("SELECT * FROM ItemPricingSchedule WHERE ItemId=?",r.Id);assertEquals(6,price.get("PriceTypeId"));assertEquals(u.getBranchesId(),price.get("BranchesId"));assertEquals(0d,jdbc.queryForObject("SELECT Equivalent FROM UOMSchedule WHERE Id=?",Double.class,price.get("RateUomId")));var commission=jdbc.queryForMap("SELECT * FROM ItemWiseCommissionSchedule WHERE ItemId=?",r.Id);assertEquals(u.getBranchesId(),commission.get("BranchesId"));assertEquals(u.getId(),commission.get("EntryUserId"));assertEquals(true,commission.get("OnSale"));assertEquals(true,commission.get("OnPurchase"));
   var gallery=jdbc.queryForMap("SELECT * FROM ItemImage WHERE ItemId=?",r.Id);assertEquals(stored,gallery.get("ImagePath"));assertEquals("verify-image.png",gallery.get("FileName"));assertEquals(false,gallery.get("IsActive"));assertEquals(true,gallery.get("IsProfileImage"));
   var attachment=jdbc.queryForMap("SELECT * FROM DMSAttachments WHERE ScreenName='InvDefrmAddItem' AND RefDocumentNo=?",r.Id);assertEquals(u.getId(),attachment.get("EntryUser"));
   r.ItemCode=Objects.toString(saved.get("ItemCode"),"");r.ItemCodeNew=Objects.toString(saved.get("ItemCodeNew"),"");r.PurchaseGLAC=num(saved.get("PurchaseGLAC"));r.SaleGLAC=num(saved.get("SaleGLAC"));r.ItemName+=" updated";r.CostPrice=new BigDecimal("20");r.PurchasePrice=new BigDecimal("21");r.WholeSalePrice=new BigDecimal("22");r.ItemNameOtherLingo="updated other";r.files=List.of();r.removeAttachmentIds=List.of(num(attachment.get("Id")));r.images=List.of();
   var changed=(Map<String,Object>)service.save(r).get("item");assertEquals(u.getId(),changed.get("ModifyUser"));assertEquals(saved.get("EntryUser"),changed.get("EntryUser"));assertEquals(saved.get("EntryDate"),changed.get("EntryDate"));assertEquals(21d,((Number)changed.get("PurchasePrice")).doubleValue());assertEquals(22d,((Number)changed.get("WholeSalePrice")).doubleValue());
   assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM ItemImage WHERE ItemId=?",Integer.class,r.Id));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM DMSAttachments WHERE Id=?",Integer.class,attachment.get("Id")));assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemPricingSchedule WHERE ItemId=?",Integer.class,r.Id));
   var rename=new InventoryGeneralItemRequest.Name();rename.id=r.Id;rename.name=r.ItemName+" renamed";rename.other="name edit";service.updateNames(List.of(rename));assertEquals(rename.name,repo.record(u,r.Id).get("ItemName"));
   var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(org.springframework.web.server.ResponseStatusException.class,()->repo.record(other,r.Id));return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM Item WHERE Id=?",Integer.class,r.Id));for(int id:accountIds)assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM ChartofAccount WHERE Id=?",Integer.class,id));
  for(String table:List.of("ItemAllocation","UOMSchedule","ItemTaxSchedule","ItemPricingSchedule","ItemsReorderSchedule","fed.ItemPricingSchedule","ItemImage","ItemWiseCommissionSchedule"))assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM "+table+" WHERE ItemId=?",Integer.class,r.Id));
  Files.writeString(Path.of("migration/inventory/evidence/general-item-"+(groupSchedule?"group-":"")+"rollback.txt"),"Define Item111 original save/update: item ownership, branch, entry/modify audit, automatic accounts/branch allocations, item allocation, base UOM, GST tax schedule, wholesale price type6, gallery insert/remove, DMS insert/remove, bulk name update and company denial verified. All test-created item/account/child rows rolled back. Rights and physical file storage mocked; normal browser rights, actual desktop writes and supplemental forms remain pending.\n");
 }
 private static int num(Object value){return ((Number)value).intValue();}
}



