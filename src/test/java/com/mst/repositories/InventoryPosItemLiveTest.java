package com.mst.repositories;

import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.web.server.ResponseStatusException;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="inventory.live",matches="true")
class InventoryPosItemLiveTest {
    @Test void desktopLookupAndHistoryContracts() throws Exception {
        var jdbc=InventoryOpeningLiveTest.jdbc();var u=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryPosItemRepository(jdbc);
        var lookups=repo.lookups(u);var rows=repo.history(u,true,false,null);
        var direct=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?, @CanViewAllRecord=?, @IsTaxable=?, @NoOfRecords=?",u.getOrganizationId(),u.getCompanyId(),"FormHistory",true,false,50);
        assertEquals(direct,rows);
        var evidence=new StringBuilder("POS106 read-only GoldenAcedb verification\nUser="+u.getId()+" Organization="+u.getOrganizationId()+" Company="+u.getCompanyId()+" Branch="+u.getBranchesId()+"\n");
        for(var e:lookups.entrySet())evidence.append(e.getKey()).append(" rows=").append(((List<?>)e.getValue()).size()).append('\n');
        assertFalse(((List<?>)lookups.get("categories")).isEmpty());
        if(!rows.isEmpty()){
            int id=((Number)rows.get(0).get("Id")).intValue();var record=repo.record(u,id);
            assertEquals(id,record.get("Id"));assertNotNull(repo.details(u,id).get("allocations"));
            int category=((Number)record.get("ItemCategoryId")).intValue();assertFalse(((List<?>)repo.categoryDefaults(u,category).get("code")).isEmpty());
            for(var r:repo.history(u,true,true,category))assertEquals(category,((Number)r.get("ItemCategoryId")).intValue());
            var other=InventoryOpeningLiveTest.user(jdbc);other.setCompanyId(-1);assertThrows(ResponseStatusException.class,()->repo.record(other,id));
        }
        evidence.append("History first50 matches every returned procedure column: ").append(rows.size()).append(" rows.\nCategory defaults, history filter, original-record load and cross-company denial checked.\nNo writes. POS UI, events, write transaction and native parity remain pending.\n");
        Files.writeString(Path.of("migration/inventory/evidence/pos-item-read.txt"),evidence);
    }
}
