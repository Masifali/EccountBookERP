package com.mst.repositories;

import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.security.*;
import com.mst.services.InventoryPosItemService;
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
class InventoryPosItemWriteLiveTest {
    @SuppressWarnings("unchecked") private static int first(Map<String,Object> choices,String name,String key){return ((Number)((List<Map<String,Object>>)choices.get(name)).get(0).get(key)).intValue();}
    @Test void insertRelatedRecordsAndUpdateAlwaysRollback() throws Exception {
        var jdbc=InventoryOpeningLiveTest.jdbc();var user=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryPosItemRepository(jdbc);var choices=repo.lookups(user);
        var fixture=jdbc.queryForMap("SELECT TOP 1 c.Id CategoryId,i.ItemTypeId,i.ItemClassId,i.BaseUnitId,i.PurchaseGLAC,i.SaleGLAC,i.COGSGLAC FROM ItemCategory c JOIN ChartofAccount s ON s.Id=c.InventoryAccountId AND s.Account_Level=3 AND s.AccountGroup='Group' JOIN ChartofAccount a ON a.Id=c.RevenueAccountId AND a.Account_Level=3 AND a.AccountGroup='Group' JOIN Item i ON i.ItemCategoryId=c.Id AND i.CompanyId=c.CompanyId JOIN ChartofAccount g ON g.Id=i.COGSGLAC AND g.AccountGroup='Detail' WHERE c.OrganizationId=? AND c.CompanyId=? AND c.InventoryParentCategoriesId<>9 ORDER BY i.Id",user.getOrganizationId(),user.getCompanyId());
        var r=new InventoryPosItemRequest();r.companies=List.of(user.getCompanyId());r.ItemCategoryId=((Number)fixture.get("CategoryId")).intValue();r.ItemTypeId=((Number)fixture.get("ItemTypeId")).intValue();r.ItemClassId=((Number)fixture.get("ItemClassId")).intValue();r.BaseUnitId=((Number)fixture.get("BaseUnitId")).intValue();
        r.PurchaseGLAC=((Number)fixture.get("PurchaseGLAC")).intValue();r.SaleGLAC=((Number)fixture.get("SaleGLAC")).intValue();r.COGSGLAC=((Number)fixture.get("COGSGLAC")).intValue();
        r.UomLookUpId=first(choices,"uomTypes","Id");r.PurchaseTypeLookUpId=first(choices,"purchaseTypes","Id");r.ItemOriginId=first(choices,"countries","Id");
        r.ItemName="CODEX_ROLLBACK_POS_"+UUID.randomUUID().toString().substring(0,8);r.ItemCode="1";r.BarcodeNo=r.ItemName;r.ItemStatus=true;
        r.PackQty=BigDecimal.ONE;r.PurchasePrice=new BigDecimal("11");r.CostPrice=new BigDecimal("12");r.WholeSalePrice=new BigDecimal("13");r.RetailPrice=new BigDecimal("14");r.MinStockLevel=BigDecimal.ONE;r.MaxStockLevel=new BigDecimal("10");r.OptimalStockLevel=new BigDecimal("5");r.ReorderLevel=new BigDecimal("2");
        var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(user);
        var storage=mock(com.mst.services.DesktopAttachmentStore.class);String storedName="Inv_"+UUID.randomUUID()+".txt";when(storage.store(any(),anyString(),any(byte[].class))).thenReturn(storedName);
        var upload=new InventoryPosItemRequest.Upload();upload.name="migration-fixture.txt";upload.base64=Base64.getEncoder().encodeToString("fixture".getBytes(java.nio.charset.StandardCharsets.UTF_8));r.files=List.of(upload);var png=new java.io.ByteArrayOutputStream();javax.imageio.ImageIO.write(new java.awt.image.BufferedImage(2,2,java.awt.image.BufferedImage.TYPE_INT_RGB),"png",png);var picture=new InventoryPosItemRequest.Upload();picture.name="fixture.png";picture.base64=Base64.getEncoder().encodeToString(png.toByteArray());r.productImage=picture;r.barcodeImage=picture;
        var service=new InventoryPosItemService(repo,new InventoryPosItemWriter(jdbc),new InventoryOpeningRepository(jdbc),context,mock(DesktopReportRights.class),new com.mst.services.InventoryPosFileService(storage,jdbc,repo));
        var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(90);
        int[] created={0};List<Integer> accounts=new ArrayList<>();
        tx.execute(status->{try{
            @SuppressWarnings("unchecked") var saved=(Map<String,Object>)service.save(r).get("item");created[0]=((Number)saved.get("Id")).intValue();r.Id=created[0];
            assertEquals(storedName,saved.get("Pic1"));assertEquals(storedName,saved.get("BarcodeImage"));assertEquals(user.getId(),saved.get("EntryUser"));assertEquals(user.getCompanyId(),saved.get("CompanyId"));assertEquals(0,saved.get("BranchesId"));
            for(String key:List.of("PurchaseGLAC","SaleGLAC")){int account=((Number)saved.get(key)).intValue();accounts.add(account);assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ChartofAccount WHERE Id=? AND AccountTitle LIKE ? AND OrganizationId=?",Integer.class,account,r.ItemName+"%",user.getOrganizationId()));}
            assertTrue(jdbc.queryForObject("SELECT COUNT(*) FROM UOMSchedule WHERE ItemId=? AND CompanyId=?",Integer.class,r.Id,user.getCompanyId())>0);
            assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemAllocation WHERE ItemId=? AND CompanyId=? AND BranchId=?",Integer.class,r.Id,user.getCompanyId(),user.getBranchesId()));
            assertEquals(4,jdbc.queryForObject("SELECT COUNT(*) FROM ItemPricingSchedule WHERE ItemId=? AND CompanyId=?",Integer.class,r.Id,user.getCompanyId()));
            assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM ItemsReorderSchedule WHERE ItemId=? AND CompanyId=?",Integer.class,r.Id,user.getCompanyId()));
            assertTrue(jdbc.queryForObject("SELECT COUNT(*) FROM fed.ItemPricingSchedule WHERE ItemId=? AND CompanyId=?",Integer.class,r.Id,user.getCompanyId())>0);
            var attachment=jdbc.queryForMap("SELECT * FROM DMSAttachments WHERE ScreenName=? AND RefDocumentNo=?", "InvAddItemsPOS",r.Id);assertEquals(storedName,attachment.get("UploadedFileCustomName"));assertEquals(user.getId(),attachment.get("EntryUser"));assertEquals(r.ItemTypeId,attachment.get("RefAccountId"));assertEquals(0,attachment.get("RefDocumentTypeId"));
            // Existing data owned by other item forms must survive the POS edit.
            jdbc.update("UPDATE Item SET ItemNameOtherLingo=?, MinRate=?, MaxRate=? WHERE Id=?","rollback preservation",3,7,r.Id);
            jdbc.update("EXEC dbo.USP_ItemImage_Insert @ItemId=?, @ImagePath=?, @EffetedDate=?, @IsActive=?, @FileName=?, @IsProfileImage=?, @SortNo=?",r.Id,"rollback-only",new java.sql.Timestamp(System.currentTimeMillis()),true,"rollback-only",false,1);
            var images=jdbc.queryForList("SELECT * FROM ItemImage WHERE ItemId=? ORDER BY Id",r.Id);
            r.PurchaseGLAC=((Number)saved.get("PurchaseGLAC")).intValue();r.SaleGLAC=((Number)saved.get("SaleGLAC")).intValue();r.ItemAliasName="updated";
            r.files=List.of();r.productImage=null;r.barcodeImage=null;r.removeAttachmentIds=List.of(((Number)attachment.get("Id")).intValue());
            service.save(r);assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM DMSAttachments WHERE Id=?",Integer.class,attachment.get("Id")));var changed=repo.record(user,r.Id);assertEquals(storedName,changed.get("Pic1"));assertEquals(storedName,changed.get("BarcodeImage"));assertEquals("updated",changed.get("ItemAliasName"));assertEquals(user.getId(),changed.get("ModifyUser"));assertEquals("rollback preservation",changed.get("ItemNameOtherLingo"));assertEquals(3d,((Number)changed.get("MinRate")).doubleValue());
            assertEquals(images,jdbc.queryForList("SELECT * FROM ItemImage WHERE ItemId=? ORDER BY Id",r.Id));assertEquals(4,jdbc.queryForObject("SELECT COUNT(*) FROM ItemPricingSchedule WHERE ItemId=?",Integer.class,r.Id));return null;
        }finally{status.setRollbackOnly();}});
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM Item WHERE ItemName=?",Integer.class,r.ItemName));
        for(int id:accounts)assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM ChartofAccount WHERE Id=?",Integer.class,id));
        for(String table:List.of("ItemAllocation","UOMSchedule","ItemPricingSchedule","ItemsReorderSchedule","fed.ItemPricingSchedule","ItemImage"))assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM "+table+" WHERE ItemId=?",Integer.class,created[0]));
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM DMSAttachments WHERE UploadedFileCustomName=?",Integer.class,storedName));
        Files.writeString(Path.of("migration/inventory/evidence/pos-item-rollback.txt"),"POS106 GoldenAcedb always-rollback test passed. Insert created item, stock/sale accounts, allocations, UOM, four POS price types, reorder schedule and feed pricing. Load/update kept original gallery IDs, other-language name and min/max rates. Entry/modify users, organization/company and desktop branch semantics checked. No new item, generated accounts or related rows remained after rollback. DMS attachment insert, ownership/audit values and removal verified inside rollback. Rights mocked for this test; browser/native verification pending. Physical VPS roundtrip tested separately.\n");
    }
}
