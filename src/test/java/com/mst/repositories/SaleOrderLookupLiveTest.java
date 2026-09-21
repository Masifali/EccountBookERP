package com.mst.repositories;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;
import java.util.Properties;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named = "sale.live", matches = "true")
class SaleOrderLookupLiveTest {
    private JdbcTemplate jdbc() throws Exception {
        Properties p = new Properties();
        try (var in = Files.newInputStream(Path.of("src/main/resources/application.properties"))) { p.load(in); }
        JdbcTemplate jdbc = new JdbcTemplate(new DriverManagerDataSource(
                p.getProperty("spring.datasource.url"), p.getProperty("spring.datasource.username"),
                p.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        return jdbc;
    }

    @Test
    void desktopOtherItemsUomAndHistoryProceduresReturnTheirRealSchemas() throws Exception {
        JdbcTemplate jdbc = jdbc();
        Map<String, Object> user = jdbc.queryForMap(
                "SELECT ID,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName=?", "numan");
        int org = ((Number) user.get("OrganizationId")).intValue();
        int company = ((Number) user.get("CompanyId")).intValue();
        int branch = ((Number) user.get("BranchesId")).intValue();
        int userId = ((Number) user.get("ID")).intValue();

        List<Map<String, Object>> otherItems = jdbc.queryForList(
                "EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity=?,@organizationId=?,@CompanyId=?",
                "ReadAll", org, company);
        assertFalse(otherItems.isEmpty(), "Desktop Customer Expense Other Item lookup must not be empty");
        assertTrue(otherItems.get(0).containsKey("OtherItemName"));

        Map<String, Object> firstItem = jdbc.queryForList(
                "EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?,@CompanyId=?", org, company).get(0);
        List<Map<String, Object>> uoms = jdbc.queryForList(
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@ItemId=?,@Activity=?",
                org, firstItem.get("Id"), "ReadByItemID");
        assertFalse(uoms.isEmpty());
        assertTrue(uoms.get(0).containsKey("QtyEquivalent"));

        List<Map<String, Object>> history = jdbc.queryForList(
                "EXEC dbo.Sp_SaleOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?," +
                        "@FinancialYearId=?,@CanViewAllRecord=?,@EntryUser=?,@BranchesIds=?,@Activity=?",
                org, company, 81, 58, true, userId, "," + branch, "SaleOrderFormHistory");
        if (!history.isEmpty()) {
            assertTrue(history.get(0).containsKey("DocNo"));
            assertTrue(history.get(0).containsKey("CustomerName"));
            assertTrue(history.get(0).containsKey("IsAproved"));
        }

        Files.createDirectories(Path.of("migration/sale/evidence"));
        Files.writeString(Path.of("migration/sale/evidence/sale-order-lookups-history.txt"),
                "Sale Order document 81. GoldenAcedb desktop Other Item, per-item UOM schedule and SaleOrderFormHistory procedures executed for Numan tenant " +
                        org + "/" + company + " branch " + branch + ". Other Items=" + otherItems.size() +
                        ", first item UOM rows=" + uoms.size() + ", history rows=" + history.size() + ".\n");
    }
}
