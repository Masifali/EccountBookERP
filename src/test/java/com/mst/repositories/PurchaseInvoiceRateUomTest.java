package com.mst.repositories;

import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

class PurchaseInvoiceRateUomTest {
    @Test void changedEquivalentResolvesWithinCurrentItemAndCompany(){
        var jdbc=mock(JdbcTemplate.class);var writes=new PurchaseInvoiceWriteRepository(jdbc);
        String sql="EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'";
        when(jdbc.queryForList(sql,78,78,656)).thenReturn(List.of(Map.of("Id",6384,"Equivalent",40),Map.of("Id",6385,"Equivalent",60)));
        var header=copy(Map.of("OrganizationId",78,"CompanyId",78,"InvoiceTypeId",2));
        var line=copy(Map.of("ItemId",656,"ItemName","Rice","EquivalentPoRate",60,"UomScheduleIdRate",6384));
        writes.resolveRateUoms(header,List.of(line));assertEquals(6385,i(line,"UomScheduleIdRate"));
        line.put("EquivalentPoRate",999);assertThrows(IllegalArgumentException.class,()->writes.resolveRateUoms(header,List.of(line)));
        line.remove("EquivalentPoRate");writes.resolveRateUoms(header,List.of(line));assertEquals(6385,i(line,"UomScheduleIdRate"));
        header.put("InvoiceTypeId",1);line.put("EquivalentPoRate",40);writes.resolveRateUoms(header,List.of(line));assertEquals(6385,i(line,"UomScheduleIdRate"));
        verify(jdbc,times(2)).queryForList(sql,78,78,656);
    }
}
