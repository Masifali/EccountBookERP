package com.mst.repositories;
import com.mst.models.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import java.time.LocalDate;
import java.util.*;
import java.util.regex.Pattern;
import static org.junit.jupiter.api.Assertions.*;

class SaleActivitiesReportRepositoryTest {
    static class Capture extends JdbcTemplate {
        String sql;Map<String,Object> parameters;
        @Override public List<Map<String,Object>> queryForList(String query,Object...args){sql=query;parameters=new LinkedHashMap<>();var matcher=Pattern.compile("(?m)^DECLARE @(\\w+) .+? = \\?;$").matcher(query);int i=0;while(matcher.find())parameters.put(matcher.group(1),args[i++]);assertEquals(args.length,i);return List.of();}
    }
    UserAccount user(){var user=new UserAccount();user.setId(78);user.setCompanyId(58);user.setOrganizationId(58);user.setAppId(3);return user;}
    @Test void reportBindsAllDesktopFiltersAndDoesNotWritePersistentReportRows(){
        var db=new Capture();var filter=new SaleActivitiesReportFilter(LocalDate.of(2026,9,1),LocalDate.of(2026,10,4),10,20,1,2,3,4,50,"2025-26",5,6,7,8,9,10,11,12,13,14,15,"Sales Register",List.of(58),List.of(16,17));
        new SaleActivitiesReportRepository(db).rows(user(),filter,",58",",16,17");
        assertEquals(36,db.parameters.size());assertEquals(78,db.parameters.get("UserId"));assertEquals(58,db.parameters.get("CompanyId"));assertEquals(",58",db.parameters.get("BranchesIds"));assertEquals(",16,17",db.parameters.get("CustomGroupIds"));
        for(var expected:Map.of("DocNoFrom",10,"DocNoTo",20,"SupplierCustomerId",3,"ItemId",4,"PackUom",50,"RefPartyId",15,"CGSAccountId",14,"ItemClassGroupId",9).entrySet())assertEquals(expected.getValue(),db.parameters.get(expected.getKey()),expected.getKey());
        assertEquals("2025-26",db.parameters.get("CropYear"));assertNull(db.parameters.get("ActionId"));
        assertFalse(Pattern.compile("(?i)(DELETE\\s+FROM|INSERT\\s+INTO|UPDATE)\\s+(?:dbo\\.)?InvSalesRegisterTemp\\b").matcher(db.sql).find());assertTrue(db.sql.contains("INTO #InvSalesRegisterTemp FROM dbo.InvSalesRegisterTemp"));
    }
    @Test void dropdownRowsKeepAnIntegerIdAndTheSelectedTenant(){var db=new Capture();new SaleActivitiesReportRepository(db).lookups(user(),",58");assertEquals(11,db.parameters.size());assertEquals("Sale",db.parameters.get("DocType"));assertEquals(78,db.parameters.get("UserId"));assertEquals(58,db.parameters.get("OrganizationId"));assertEquals(",58",db.parameters.get("BranchesIds"));assertTrue(db.sql.contains("select Distinct d.BookingPersonId, sp.ReferencePartyName, 'PurchaseBookingPerson'"));}
}
