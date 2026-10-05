package com.mst.repositories;

import com.mst.models.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.*;
import java.sql.PreparedStatement;
import java.time.LocalDate;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SaleOrderReportRepositoryTest {
    static class Capture extends JdbcTemplate {
        String sql;Map<String,Object> parameters;
        @Override public List<Map<String,Object>> queryForList(String query,Object... args){
            sql=query;parameters=new LinkedHashMap<>();var matcher=java.util.regex.Pattern.compile("(?m)^DECLARE @(\\w+) .+? = \\?;$").matcher(query);int index=0;
            while(matcher.find())parameters.put(matcher.group(1),args[index++]);assertEquals(args.length,index);return List.of();
        }
        @Override public <T> T execute(String query,PreparedStatementCallback<T> callback){
            try{sql=query;var values=new TreeMap<Integer,Object>();var statement=mock(PreparedStatement.class);
                doAnswer(call->{values.put(call.getArgument(0),call.getArgument(1));return null;}).when(statement).setObject(anyInt(),any());when(statement.getUpdateCount()).thenReturn(-1);
                T result=callback.doInPreparedStatement(statement);parameters=new LinkedHashMap<>();var matcher=java.util.regex.Pattern.compile("@(\\w+)=\\?").matcher(query);int index=1;while(matcher.find())parameters.put(matcher.group(1),values.get(index++));return result;
            }catch(Exception ex){throw new RuntimeException(ex);}
        }
    }
    UserAccount user(){var u=new UserAccount();u.setId(78);u.setOrganizationId(58);u.setCompanyId(58);u.setAppId(3);return u;}
    SaleOrderReportFilter filter(String approval,boolean include){return new SaleOrderReportFilter(LocalDate.of(2026,9,1),LocalDate.of(2026,10,4),137,140,12,13,14,15,16,17,18,19,20,"Open",approval,include,List.of(58),40,"2025-26",21,22,23,"Order Summary By Item",true);}
    @Test void detailUsesDesktopFilterNamesAndCustomerUserNotApprovalQueueUser(){var db=new Capture();var repo=new SaleOrderReportRepository(db);repo.detail(user(),filter("Approve",true),",58");
        assertTrue(db.sql.startsWith("EXEC dbo.Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt "));assertEquals(78,db.parameters.get("CustomerUserId"));assertFalse(db.parameters.containsKey("UserId"));assertEquals(15,db.parameters.get("ItemCategoryId"));assertEquals(17,db.parameters.get("BookingPersonId"));assertEquals(19,db.parameters.get("ReferencePartyId"));assertEquals(137,db.parameters.get("FromDocNo"));assertEquals(true,db.parameters.get("IsApproved"));assertEquals(1,db.parameters.get("ActionId"));assertEquals(",58",db.parameters.get("BranchesIds"));
        repo.detail(user(),filter("All",false),",58");assertFalse(db.parameters.containsKey("IsApproved"));assertFalse(db.parameters.containsKey("ActionId"));
    }
    @Test void summaryUsesNativeActivityAndPackingFiltersWithoutExecutingSharedScratchWrites(){var db=new Capture();new SaleOrderReportRepository(db).summary(user(),filter("UnApprove",false),",58");
        assertTrue(db.sql.contains("SELECT TOP (0) * INTO #SaleOrderRegisterTemp FROM dbo.SaleOrderRegisterTemp"));
        assertFalse(java.util.regex.Pattern.compile("(?i)(DELETE\\s+FROM|INSERT\\s+INTO|UPDATE)\\s+(?:dbo\\.)?SaleOrderRegisterTemp\\b").matcher(db.sql).find());
        assertEquals(34,db.parameters.size());assertEquals(78,db.parameters.get("UserId"));assertEquals(58,db.parameters.get("CompanyId"));assertEquals(",58",db.parameters.get("BranchesIds"));
        assertEquals(137,db.parameters.get("DocNoFrom"));assertEquals(140,db.parameters.get("DocNoTo"));assertEquals(12,db.parameters.get("OrderSupCustId"));assertEquals(13,db.parameters.get("OrderItemId"));assertEquals(14,db.parameters.get("InventoryParentCategories"));assertEquals(40,db.parameters.get("PackUom"));assertEquals("2025-26",db.parameters.get("CropYear"));assertEquals("Order Summary By Item",db.parameters.get("ActivityName"));assertEquals(false,db.parameters.get("IsApproved"));assertEquals(1,db.parameters.get("SkipZero"));assertNull(db.parameters.get("ActionId"));}
    @Test void dropdownsUseNativeSaleOrderDocumentAndSessionScope(){var db=new Capture();new SaleOrderReportRepository(db).lookups(user(),",58",20);assertTrue(db.sql.startsWith("EXEC dbo.USP_GetDataForDropDownFromSaleOrder "));assertEquals(Map.of("OrganizationId",58,"CompanyId",58,"AppId",3,"UserId",78,"DocumentTypeIds","81","BranchesIds",",58","CostCenterId",20),db.parameters);}
}
