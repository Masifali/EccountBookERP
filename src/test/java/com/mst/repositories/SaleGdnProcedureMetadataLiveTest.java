package com.mst.repositories;

import static org.junit.jupiter.api.Assertions.assertFalse;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleGdnProcedureMetadataLiveTest {
    @Test void captureDesktopProcedureContracts() throws Exception {
        var p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}
        var j=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));
        var names=List.of("Sp_InvGdn_Insert","Sp_InvGdn_Update","Sp_InvGdnDetail_Insert","Sp_InvGdnExpense_Insert","Sp_InventoryTransactions_GetALLMethod","USP_InventoryValidation","usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding","Sp_InventoryStockEvalautionDetail_Update");
        var out=new StringBuilder("GoldenAcedb procedure contracts used by desktop InvGdn.SetData\n");
        for(String name:names){var rows=j.queryForList("SELECT p.parameter_id,p.name,TYPE_NAME(p.user_type_id) type_name,p.max_length,p.is_output FROM sys.parameters p WHERE p.object_id=OBJECT_ID(?) ORDER BY p.parameter_id",name);assertFalse(rows.isEmpty(),name+" not found");out.append("\n[").append(name).append("]\n");for(var r:rows)out.append(r).append('\n');}
        Files.createDirectories(Path.of("migration/sale/evidence"));Files.writeString(Path.of("migration/sale/evidence/gdn-procedure-contracts.txt"),out.toString());
    }
}
