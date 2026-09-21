package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryVehicleTransactionsRequest;
import com.mst.services.InventoryVehicleTransactionsService;
import java.nio.file.*;
import java.util.*;
import java.math.BigDecimal;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

/** Opt-in, read-only. No application context, seeders or business writes. */
@EnabledIfSystemProperty(named="inventory.live",matches="true")
class InventoryVehicleTransactionsLiveTest {
    private static Map<String,Object> normalize(Map<String,Object> row){Map<String,Object> out=new TreeMap<>();row.forEach((k,v)->out.put(k,v instanceof byte[]?Base64.getEncoder().encodeToString((byte[])v):v instanceof BigDecimal?((BigDecimal)v).toPlainString():v));return out;}
    private static Map<Map<String,Object>,Integer> multiset(List<Map<String,Object>> rows){Map<Map<String,Object>,Integer> result=new HashMap<>();rows.forEach(row->result.merge(normalize(row),1,Integer::sum));return result;}
    @Test void desktopProceduresAndJavaReturnIdenticalLookupsAndTransactions() throws Exception {
        Properties props=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){props.load(in);}
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(props.getProperty("spring.datasource.url"),props.getProperty("spring.datasource.username"),props.getProperty("spring.datasource.password")));jdbc.setQueryTimeout(60);
        var account=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId FROM dbo.UserAccount WHERE UserName=?","numan");var u=new UserAccount();u.setId(((Number)account.get("ID")).intValue());u.setOrganizationId(((Number)account.get("OrganizationId")).intValue());u.setCompanyId(((Number)account.get("CompanyId")).intValue());
        var repository=new InventoryVehicleTransactionsRepository(jdbc);var lookup=repository.lookups(u);
        assertEquals(multiset(jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId())),multiset((List<Map<String,Object>>)lookup.get("stock")));
        assertEquals(multiset(jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?","ReadAll")),multiset((List<Map<String,Object>>)lookup.get("packingTypes")));
        assertEquals(multiset(jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"GetDocumentTypeFromInventoryStocksEvaluations")),multiset((List<Map<String,Object>>)lookup.get("documentTypes")));
        int item=3;assertEquals(multiset(jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),item,"ReadByItemID")),multiset(repository.uoms(u,item)));
        var latest=jdbc.queryForObject("SELECT MAX(CAST(DocDate AS date)) FROM dbo.InventoryStockEvalautionDetail WHERE OrganizationId=? AND CompanyId=? AND ItemId=?",java.sql.Date.class,u.getOrganizationId(),u.getCompanyId(),item);assertNotNull(latest);
        var r=new InventoryVehicleTransactionsRequest();r.setItemId(item);r.setToDate(latest.toLocalDate());r.setFromDate(r.getToDate().minusDays(7));
        var desktop=jdbc.queryForList("EXEC dbo.USP_GetStockEvalautionDetailByRefRefIds @OrganizationId=?, @CompanyId=?, @DateFrom=?, @DateTo=?, @ItemId=?",u.getOrganizationId(),u.getCompanyId(),java.sql.Date.valueOf(r.getFromDate()),latest,item);var javaRows=repository.load(u,r);
        assertFalse(desktop.isEmpty());assertEquals(multiset(desktop),multiset(javaRows));assertEquals(javaRows.size(),InventoryVehicleTransactionsService.project(javaRows).size());
        Files.createDirectories(Path.of("migration/inventory/evidence"));Files.writeString(Path.of("migration/inventory/evidence/vehicle-transactions-live.txt"),"Read-only live comparison\nUser="+u.getId()+" Organization="+u.getOrganizationId()+" Company="+u.getCompanyId()+"\nItem="+item+" From="+r.getFromDate()+" To="+r.getToDate()+"\nTransaction rows="+javaRows.size()+"\nStock, packing, document and item UOM lookups match desktop procedures. Every report cell and duplicate-row count matched.\nNative UI and Crystal print not verified. No database writes.\n");
    }
}
