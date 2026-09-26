package com.mst.repositories;
import com.mst.models.dto.*;
import com.mst.security.*;
import com.mst.services.*;
import java.math.BigDecimal;
import java.nio.charset.StandardCharsets;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.files",matches="true")
class InventoryOpeningAttachmentsLiveTest {
 @Test void saveDownloadRemoveAndRecordUpdateRollbackTogether()throws Exception{
  var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryOpeningRepository(jdbc);var writer=new InventoryOpeningWriter(jdbc);var store=new DesktopAttachmentStore(jdbc,new LegacyVpsCredentials("../recovered_source/bin/Debug/ECCOUNTBOOKERP.exe.config"));var files=new InventoryOpeningAttachmentService(store,jdbc,repo);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);var service=new InventoryOpeningService(repo,writer,context,mock(DesktopReportRights.class),files);
  int year=((Number)repo.years(u).get(0).get("Id")).intValue();var source=repo.history(u,year,new InventoryOpeningRequest.History()).stream().filter(row->((Number)row.get("ProjectsId")).intValue()>0&&((Number)row.get("JobLotId")).intValue()>0).findFirst().orElseThrow();var r=new InventoryOpeningRequest();
  for(String key:List.of("projectsId","warehouseId","itemId","itemUomSch","jobLotId","packingTypeId","stockCrGLAcId","rateUomSch"))InventoryOpeningRequest.class.getField(key).setInt(r,((Number)source.get(Character.toUpperCase(key.charAt(0))+key.substring(1))).intValue());
  for(String key:List.of("qty","weightKgs","itemRate","itemAmount"))InventoryOpeningRequest.class.getField(key).set(r,new BigDecimal(source.get(Character.toUpperCase(key.charAt(0))+key.substring(1)).toString()));
  r.cropYear=Objects.toString(source.get("CropYear"));r.transactionType="Stocks";r.remarks="INVENTORY_ATTACHMENT_ROLLBACK_"+UUID.randomUUID();byte[] bytes="Opening Stock generated attachment verification".getBytes(StandardCharsets.UTF_8);var upload=new InventoryPosItemRequest.Upload();upload.name="opening-stock-verification.txt";upload.base64=Base64.getEncoder().encodeToString(bytes);r.files=List.of(upload);
  var before=jdbc.queryForList("SELECT * FROM DMSAttachments ORDER BY Id");String[] generated={null};int[] created={0};var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(120);
  tx.execute(status->{try{var record=service.save(r);created[0]=((Number)record.get("Id")).intValue();r.id=created[0];var attachments=service.attachments(r.id);assertEquals(1,attachments.size());var attachment=attachments.get(0);int attachmentId=((Number)attachment.get("Id")).intValue();generated[0]=attachment.get("UploadedFileCustomName").toString();assertArrayEquals(bytes,service.download(r.id,attachmentId).bytes());var stored=jdbc.queryForMap("SELECT * FROM DMSAttachments WHERE Id=?",attachmentId);assertEquals(40,stored.get("RefDocumentTypeId"));assertEquals(r.itemId,stored.get("RefAccountId"));assertEquals(u.getCompanyId(),stored.get("CompanyId"));assertEquals(u.getId(),stored.get("EntryUser"));var header=jdbc.queryForMap("SELECT AttachmentsValues,CustomAttachmentsValues FROM InvStockOpeningBalanceHeader WHERE Id=?",r.id);assertEquals(upload.name,header.get("AttachmentsValues"));assertEquals(generated[0],header.get("CustomAttachmentsValues"));
   r.files=List.of();r.removeAttachmentIds=List.of(attachmentId);service.save(r);assertTrue(service.attachments(r.id).isEmpty());var cleared=jdbc.queryForMap("SELECT AttachmentsValues,CustomAttachmentsValues FROM InvStockOpeningBalanceHeader WHERE Id=?",r.id);assertEquals("",Objects.toString(cleared.get("AttachmentsValues"),""));assertEquals("",Objects.toString(cleared.get("CustomAttachmentsValues"),""));assertThrows(org.springframework.web.server.ResponseStatusException.class,()->service.download(r.id,attachmentId));assertTrue(store.containsGenerated(u,generated[0]));return null;}finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList("SELECT * FROM DMSAttachments ORDER BY Id"));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM InvStockOpeningBalanceHeader WHERE Id=?",Integer.class,created[0]));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM VoucherHead WHERE DocumentTypeId=40 AND DocumentTypeSrNo=?",Integer.class,created[0]));assertFalse(store.containsGenerated(u,generated[0]));
  Files.writeString(Path.of("migration/inventory/evidence/opening-attachments-live.txt"),"Opening Stock92: service Save created only a transaction-scoped test opening/voucher and uploaded generated non-business text to configured desktop storage. Download matched bytes, DMS original procedure metadata and header filenames matched, Update removed the association and cleared header names, removed download rejected, complete DMS/original rows unchanged after rollback; test opening/voucher and generated file confirmed absent. Existing physical objects are retained when unlinked. Rights mocked. Browser/native attachment interaction pending.\n");
 }
}
