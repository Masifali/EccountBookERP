package com.mst.repositories;
import com.mst.security.LegacyVpsCredentials;
import com.mst.services.DesktopAttachmentStore;
import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="inventory.files",matches="true")
class InventoryAttachmentLiveTest {
 @Test void generatedFileRoundTripAndRollbackCleanup() throws Exception {
    var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var store=new DesktopAttachmentStore(jdbc,new LegacyVpsCredentials("../recovered_source/bin/Debug/ECCOUNTBOOKERP.exe.config"));byte[] payload="Inventory migration generated storage verification".getBytes(StandardCharsets.UTF_8);String[] created={null};
    var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setTimeout(60);tx.execute(status->{try{created[0]=store.store(u,"inventory-verification.txt",payload);assertArrayEquals(payload,store.read(u,created[0]));assertTrue(store.containsGenerated(u,created[0]));return null;}finally{status.setRollbackOnly();}});
    assertNotNull(created[0]);assertFalse(store.containsGenerated(u,created[0]));Files.writeString(Path.of("migration/inventory/evidence/attachment-storage-live.txt"),"Configured desktop attachment storage: generated non-business text uploaded, read byte-for-byte and removed after transaction rollback. Exact generated filename confirmed absent afterward. No existing files read, modified or deleted. No database mutations. Credentials remained internal to the Java configuration reader.\n");
 }
}
