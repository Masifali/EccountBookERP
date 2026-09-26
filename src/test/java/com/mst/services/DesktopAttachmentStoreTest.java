package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.security.LegacyVpsCredentials;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.transaction.support.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class DesktopAttachmentStoreTest {
 @TempDir Path directory;
 @Test void newLocalFileIsCleanedOnRollbackAndNeverReplacesExistingFile() throws Exception {
    var jdbc=mock(JdbcTemplate.class);var user=new UserAccount();user.setOrganizationId(78);user.setCompanyId(78);
    String sql="EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?";
    when(jdbc.queryForList(sql,78,78,"IsVpsAttachmentsServiceOn","GetConfigurationByOrgCompandConfigDescription")).thenReturn(List.of(Map.of("ConfigKey","False")));
    when(jdbc.queryForList(sql,78,78,"Attachment Folder Path","GetConfigurationByOrgCompandConfigDescription")).thenReturn(List.of(Map.of("ConfigKey",directory.toString())));
    var store=new DesktopAttachmentStore(jdbc,mock(LegacyVpsCredentials.class));byte[] original={1,2,3};Files.write(directory.resolve("document.txt"),original);
    TransactionSynchronizationManager.initSynchronization();try{String name=store.store(user,"document.txt",new byte[]{4,5,6});assertNotEquals("document.txt",name);assertArrayEquals(new byte[]{4,5,6},store.read(user,name));for(var sync:TransactionSynchronizationManager.getSynchronizations())sync.afterCompletion(TransactionSynchronization.STATUS_ROLLED_BACK);assertFalse(Files.exists(directory.resolve(name)));assertArrayEquals(original,Files.readAllBytes(directory.resolve("document.txt")));}finally{TransactionSynchronizationManager.clearSynchronization();}
 }
 @Test void rejectsTraversalOversizedPayloadsAndDisguisedImages(){for(String name:List.of("../file","C:secret","a\\b","a\r\nb",".."))assertThrows(IllegalArgumentException.class,()->DesktopAttachmentStore.validateName(name));var file=new InventoryPosItemRequest.Upload();file.name="photo.png";file.base64=Base64.getEncoder().encodeToString("not an image".getBytes(java.nio.charset.StandardCharsets.UTF_8));assertThrows(IllegalArgumentException.class,()->InventoryPosFileService.imageType(InventoryPosFileService.decode(file)));file.base64="!";assertThrows(IllegalArgumentException.class,()->InventoryPosFileService.decode(file));}
}
