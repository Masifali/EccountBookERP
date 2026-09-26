package com.mst.repositories;

import com.fasterxml.jackson.databind.*;
import com.mst.models.*;
import com.mst.security.CurrentUserContext;
import com.mst.services.InwardGatePassService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.aop.framework.ProxyFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.*;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.transaction.annotation.AnnotationTransactionAttributeSource;
import org.springframework.transaction.interceptor.TransactionInterceptor;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="purchase.live",matches="true")
class InwardGatePassParityLiveTest {
    private static int n(Object v) { return v instanceof Number?((Number)v).intValue():0; }
    @Test void desktopLookupsHistoryAndTransactionalInsertUpdate() throws Exception {
        var properties=new Properties();
        try(var input=Files.newInputStream(Path.of("src/main/resources/application.properties"))) { properties.load(input); }
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(properties.getProperty("spring.datasource.url"),properties.getProperty("spring.datasource.username"),properties.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        var user=jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int org=n(user.get("OrganizationId")),company=n(user.get("CompanyId")),branch=n(user.get("BranchesId")),uid=n(user.get("Id"));
        var source=jdbc.queryForMap("SELECT TOP 1 * FROM dbo.GatePassInward WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? AND DocumentTypeId=51 AND EntryUser=? ORDER BY Id DESC",org,company,branch,uid);
        var context=mock(CurrentUserContext.class);
        when(context.currentOrganizationId()).thenReturn(org); when(context.currentCompanyId()).thenReturn(company);
        when(context.currentBranchId()).thenReturn(branch); when(context.currentUserId()).thenReturn(uid);
        when(context.currentFinancialYearId()).thenReturn(n(source.get("FinancialYearId"))); when(context.currentRoleName()).thenReturn("");
        var repository=spy(new InwardGatePassRepository()); ReflectionTestUtils.setField(repository,"jdbcTemplate",jdbc);
        var access=new InwardGatePassRecordRepository(jdbc,context);
        var target=new InwardGatePassService(); ReflectionTestUtils.setField(target,"repository",repository);
        ReflectionTestUtils.setField(target,"records",access); ReflectionTestUtils.setField(target,"context",context);
        var manager=new DataSourceTransactionManager(jdbc.getDataSource());
        var proxy=new ProxyFactory(target); proxy.addAdvice(new TransactionInterceptor(manager,new AnnotationTransactionAttributeSource()));
        var service=(InwardGatePassService)proxy.getProxy();
        assertEquals(jdbc.queryForList("EXEC dbo.Sp_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'",org,company).size(),repository.getCities(org,company).size());
        assertEquals(jdbc.queryForList("EXEC dbo.Sp_VehicleType_GetAllMethod").size(),repository.getVehicleTypes(org,company).size());
        assertEquals(jdbc.queryForList("EXEC dbo.Sp_GatePassType_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='Inward'",org,company).size(),repository.getGatePassTypes(org,company).size());
        assertTrue(repository.getVehicleTypes(org,company).stream().allMatch(row->row.get("name")!=null));
        var dropdowns=service.getDropdowns(org,company);
        for(String key:List.of("suppliers","cities","items","vehicleTypes","gatePassTypes","packingTypes","orderTypes","statuses")) {
            var options=(List<Map<String,Object>>)dropdowns.get(key);
            assertFalse(options.isEmpty(),key+" is populated from the desktop source");
            assertTrue(options.stream().allMatch(row->row.get("id")!=null && row.get("name")!=null),key+" maps identifiers and captions");
        }
        int sourceId=n(source.get("Id")),supplier=n(source.get("SupplierCustomerId"));
        var transit=repository.getTransitVehicles(org,company,supplier,0,sourceId);
        var desktopTransit=jdbc.queryForList("EXEC dbo.USP_SupplierDispatch_GetNoForGpandGrn @OrganizationId=?,@CompanyId=?,@SupplierId=?,@GpRecId=?",org,company,supplier,sourceId);
        assertEquals(desktopTransit.size(),transit.size());
        for(int i=0;i<transit.size();i++) { assertEquals(desktopTransit.get(i).get("SupplierDispatchId"),transit.get(i).get("id")); assertEquals(desktopTransit.get(i).get("SupplierDispatchNo"),transit.get(i).get("name")); }
        assertEquals(jdbc.queryForList("EXEC dbo.Sp_WbTransation_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@RefDocumentTypeId=51,@Activity='GetNetWeightFromWbTransactions'",org,company,sourceId),repository.getWeighBridgeWeights(org,company,sourceId));
        double docNo=((Number)source.get("GpSrNo")).doubleValue();
        var history=service.getHistory(org,company,branch,context.currentFinancialYearId(),51,null,null,docNo,docNo,null);
        assertTrue(history.stream().anyMatch(row->n(row.get("Id"))==n(source.get("Id"))),"Desktop-visible source is present in history");
        var original=jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=51,@CanViewAllRecord=?,@EntryUser=?,@DocNoFrom=?,@DocNoTo=?,@Activity='GatepassHistory'",org,company,branch,context.currentFinancialYearId(),access.canViewAll(),uid,(int)docNo,(int)docNo);
        assertEquals(original,history);
        when(context.currentCompanyId()).thenReturn(-1);
        assertThrows(org.springframework.web.server.ResponseStatusException.class,()->service.getById(n(source.get("Id"))));
        when(context.currentCompanyId()).thenReturn(company);
        var mapper=new ObjectMapper().configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES,true).configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES,false);
        var dto=mapper.convertValue(source,InwardGatePass.class);
        dto.setId(0); dto.setGpDate(new Date()); dto.setBiltyDate(new Date()); dto.setGpSrNo(0); dto.setGpTypeSrNo(0);
        dto.setStatus("Open"); dto.setOtherSupCust("Market Purchase"); dto.setRefDocumentTypeId(105); dto.setPurchaseOrderId(0);
        dto.setNoOfPackages(1); dto.setPackUnit(1d); dto.setWeightComparedToPoWt(1d); dto.setFactoryWeight(0d); dto.setSupplierWeight(1d);
        dto.setAdvanceByFactory(0d); dto.setAdvanceByParty(0d); dto.setFreight(0d); dto.setNetPaid(0d);
        String marker="JAVA-IGP-ROLLBACK-"+UUID.randomUUID(); dto.setOtherRemarks(marker);
        // The desktop main form stores its item on the header; it does not invent a detail row.
        dto.setGatePassInwardDetails(new ArrayList<>());
        var breakup=new InwardGatePassPurchaseBreakUp(); breakup.setQty(2d); breakup.setUom(60d); breakup.setGrossWeight(120d); breakup.setEbWeight(1d); breakup.setEbTotal(2d); breakup.setNetWeight(122d);
        dto.setGatePassInwardPurchaseBreakUpList(new ArrayList<>(List.of(breakup)));
        var transaction=new TransactionTemplate(manager);
        transaction.execute(status->{
            status.setRollbackOnly();
            var saved=service.saveRecord(dto); assertEquals(true,saved.get("success"),Objects.toString(saved.get("message")));
            int id=n(saved.get("igpId")); assertTrue(id>0);
            assertEquals(saved.get("gpSrNo"),jdbc.queryForObject("SELECT GpSrNo FROM dbo.GatePassInward WHERE Id=?",Integer.class,id));
            assertEquals(0,((List<?>)service.getById(id).get("details")).size());
            var storedBreakup=jdbc.queryForMap("SELECT Qty,UOM,GrossWeight,EbWeight,EBTotal,NetWeight FROM dbo.GatePassInwardPurchaseBreakUp WHERE InwardGatePassId=?",id);
            assertEquals(122d,((Number)storedBreakup.get("NetWeight")).doubleValue());
            dto.setOtherRemarks(marker+"-UPDATE"); dto.setNoOfPackages(2);
            var updated=service.saveRecord(dto); assertEquals(true,updated.get("success"),Objects.toString(updated.get("message")));
            assertEquals(marker+"-UPDATE",jdbc.queryForObject("SELECT OtherRemarks FROM dbo.GatePassInward WHERE Id=?",String.class,id));
            assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInwardDetail WHERE GatePassInwardId=?",Integer.class,id));
            assertEquals(2,jdbc.queryForObject("SELECT NoOfPackages FROM dbo.GatePassInward WHERE Id=?",Integer.class,id));
            assertEquals(uid,jdbc.queryForObject("SELECT ModifyUser FROM dbo.GatePassInward WHERE Id=?",Integer.class,id));
            assertEquals(storedBreakup,jdbc.queryForMap("SELECT Qty,UOM,GrossWeight,EbWeight,EBTotal,NetWeight FROM dbo.GatePassInwardPurchaseBreakUp WHERE InwardGatePassId=?",id));
            assertEquals(1d,dto.getDifferenceWeight());
            // The original SQL resets factory/difference weights while a vehicle is Open.
            assertEquals(0d,jdbc.queryForObject("SELECT DifferenceWeight FROM dbo.GatePassInward WHERE Id=?",Double.class,id));
            assertEquals(405,assertThrows(org.springframework.web.server.ResponseStatusException.class,()->service.deleteRecord(id,org,company)).getStatus().value());
            return null;
        });
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInward WHERE OtherRemarks LIKE ?",Integer.class,marker+"%"));
        // Prove the service's own error handling rolls back a header when a child write fails.
        dto.setId(0); dto.setOtherRemarks(marker+"-FAILURE");
        dto.setGatePassInwardPurchaseBreakUpList(new ArrayList<>(List.of(new InwardGatePassPurchaseBreakUp())));
        doThrow(new IllegalStateException("Injected child write failure")).when(repository).savePurchaseBreakUp(any());
        var failed=service.saveRecord(dto); assertEquals(false,failed.get("success"));
        assertTrue(Objects.toString(failed.get("message")).contains("Injected child write failure"));
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInward WHERE OtherRemarks LIKE ?",Integer.class,marker+"%"));
        Files.writeString(Path.of("migration/purchase/evidence/inward-gate-pass-rollback.txt"),"GoldenAcedb: all master lookups populated with id/name aliases. Original city, vehicle, inward gate-pass type, supplier dispatch and weighbridge lookups match. Filtered history matches desktop procedure with user/organization/company/branch/year filters. Wrong-company read refused. Insert/load/update/ModifyUser and breakup persistence/rebuild passed without fabricating main-item detail rows. Absolute weight difference passed. Injected child failure rolled back its header through the service's transaction. Header delete unavailable as desktop. All test writes rolled back. Physical attachments and full UI events are not verified.\n");
    }
}
