package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import org.springframework.web.server.ResponseStatusException;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="purchase.live",matches="true")
class PurchaseInvoiceRecordParityLiveTest {
    JdbcTemplate jdbc;CurrentUserContext context;PurchaseInvoiceRecordRepository records;
    @BeforeEach void setup() throws Exception {
        var properties=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){properties.load(in);}
        jdbc=new JdbcTemplate(new DriverManagerDataSource(properties.getProperty("spring.datasource.url"),properties.getProperty("spring.datasource.username"),properties.getProperty("spring.datasource.password")));jdbc.setQueryTimeout(60);
        var user=jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int year=jdbc.queryForObject("SELECT TOP 1 FinancialYearId FROM dbo.InvPurchaseInvoice WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? ORDER BY Id DESC",Integer.class,user.get("OrganizationId"),user.get("CompanyId"),user.get("BranchesId"));
        context=mock(CurrentUserContext.class);when(context.currentOrganizationId()).thenReturn(n(user.get("OrganizationId")));when(context.currentCompanyId()).thenReturn(n(user.get("CompanyId")));when(context.currentBranchId()).thenReturn(n(user.get("BranchesId")));when(context.currentFinancialYearId()).thenReturn(year);when(context.currentUserId()).thenReturn(n(user.get("Id")));when(context.currentRoleName()).thenReturn("");
        records=new PurchaseInvoiceRecordRepository(jdbc,context);
    }
    @Test void historyAndLoadedCollectionsMatchOriginalQueries() throws Exception {
        List<String> evidence=new ArrayList<>();
        for(int type:List.of(56,57,59,61,138)) {
            if(!records.hasRight(type,"View")) {
                assertEquals(403,assertThrows(ResponseStatusException.class,()->records.history(type,null,null,null,null,null,"docdate")).getStatus().value());
                evidence.add("Type "+type+": View denied by Numan's actual rights; no data bypass.");continue;
            }
            var source=jdbc.queryForList("SELECT TOP 1 Id,DocNo FROM dbo.InvPurchaseInvoice WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? AND DocumentTypeId=? AND (?=1 OR EntryUser=?) ORDER BY Id DESC",context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId(),type,records.hasRight(type,"CanView AllRecord"),context.currentUserId());
            Integer no=source.isEmpty()?null:n(source.get(0).get("DocNo"));
            var actual=records.history(type,null,null,null,no,no,"docdate");
            var original=jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesIds=?,@CanViewAllRecord=?,@EntryUser=?,@FromDocNo=?,@ToDocNo=?,@Activity='FormHistory' WITH RECOMPILE",context.currentOrganizationId(),context.currentCompanyId(),type,context.currentFinancialYearId(),Integer.toString(context.currentBranchId()),records.hasRight(type,"CanView AllRecord"),context.currentUserId(),new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.INTEGER,no),new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.INTEGER,no));
            assertEquals(original.size(),actual.size());
            for(int i=0;i<original.size();i++)for(var entry:original.get(i).entrySet())assertEquals(entry.getValue(),actual.get(i).get(entry.getKey()),entry.getKey());
            if(source.isEmpty()){evidence.add("Type "+type+": empty scoped history matches original; no record fixture available.");continue;}
            int id=n(source.get(0).get("Id"));var loaded=records.load(id,type);
            var head=jdbc.queryForMap("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById'",id);
            for(var entry:head.entrySet())assertEquals(entry.getValue(),loaded.get(entry.getKey()),entry.getKey());
            var detail=type==61?jdbc.queryForList("EXEC dbo.USP_InvPurchaseInvoiceDetail_ReadById @Id=?",id):jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=?",id,type==56?"PurchaseDetailReadByInvPurchaseInvoiceId":"DirectPurchaseDetailReadByInvPurchaseInvoiceId");
            assertEquals(detail,loaded.get("details"));
            for(var entry:Map.of("expenses","InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID","freight","InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID","journal","InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID","emptyBags","InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID","paymentTerms","InvPurchaseInvoicePaymentTerm_ReadByPurchaseInvoiceID").entrySet())
                assertEquals(jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=?",id,entry.getValue()),loaded.get(entry.getKey()));
            evidence.add("Type "+type+": document "+no+" (Id "+id+") header, details, expenses, freight, journal, bags and payment terms match original procedure results; history rows "+actual.size()+".");
            assertTrue(loaded.get("recordUrl").toString().endsWith("?id="+id));
            int company=context.currentCompanyId(),branch=context.currentBranchId(),year=context.currentFinancialYearId();
            when(context.currentCompanyId()).thenReturn(-1);assertEquals(404,assertThrows(ResponseStatusException.class,()->records.load(id,type)).getStatus().value());when(context.currentCompanyId()).thenReturn(company);
            when(context.currentBranchId()).thenReturn(-1);assertThrows(ResponseStatusException.class,()->records.load(id,type));when(context.currentBranchId()).thenReturn(branch);
            when(context.currentFinancialYearId()).thenReturn(-1);assertThrows(ResponseStatusException.class,()->records.load(id,type));when(context.currentFinancialYearId()).thenReturn(year);
            assertThrows(ResponseStatusException.class,()->records.load(id,type==56?57:56));
            if(!records.hasRight(type,"Delete")) {assertEquals(403,assertThrows(ResponseStatusException.class,()->records.delete(id,type)).getStatus().value());assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvPurchaseInvoice WHERE Id=?",Integer.class,id));}
        }
        evidence.add("Read-only verification. Invoice Save/Update, permitted Delete, GUI and full financial posting are not verified by this test.");
        Files.write(Path.of("migration/purchase/evidence/invoice-record-parity.txt"),evidence);
    }
    @Test void malformedDatesAndUnrelatedDocumentTypesAreRejected() {
        assertThrows(ResponseStatusException.class,()->records.history(18,null,null,null,null,null,"docdate"));
        assertEquals(405,assertThrows(ResponseStatusException.class,()->records.delete(0,59)).getStatus().value());
        if(records.hasRight(56,"View"))assertEquals(400,assertThrows(ResponseStatusException.class,()->records.history(56,"2026-09-01' OR 1=1--",null,null,null,null,"docdate")).getStatus().value());
    }
    private static int n(Object value){return ((Number)value).intValue();}
}
