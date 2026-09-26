package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import com.mst.services.PurchaseGrnPersistenceService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.aop.framework.ProxyFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.*;
import org.springframework.transaction.annotation.AnnotationTransactionAttributeSource;
import org.springframework.transaction.interceptor.TransactionInterceptor;
import org.springframework.transaction.support.TransactionTemplate;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="purchase.live",matches="true")
class PurchaseGrnParityLiveTest {
    JdbcTemplate jdbc; CurrentUserContext context; PurchaseGrnRecordRepository records;
    PurchaseGrnWriteRepository writes; PurchaseGrnPersistenceService service;
    TransactionTemplate transaction; Map<String,Object> source;
    @BeforeEach void setup() throws Exception {
        var properties=new Properties();
        try(var input=Files.newInputStream(Path.of("src/main/resources/application.properties"))) { properties.load(input); }
        jdbc=new JdbcTemplate(new DriverManagerDataSource(properties.getProperty("spring.datasource.url"),properties.getProperty("spring.datasource.username"),properties.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        var user=jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        source=jdbc.queryForMap("SELECT TOP 1 * FROM dbo.InvGrn h WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? AND DocumentTypeId=46 AND EXISTS(SELECT 1 FROM dbo.InvGrnDetail d WHERE d.InvGrnId=h.Id AND d.ItemId>0 AND d.WarehouseId>0 AND EXISTS(SELECT 1 FROM dbo.V_UomScheduleAndUom u WHERE u.Id=d.ItemUomId)) ORDER BY Id DESC",user.get("OrganizationId"),user.get("CompanyId"),user.get("BranchesId"));
        context=mock(CurrentUserContext.class);
        when(context.currentOrganizationId()).thenReturn(number(source.get("OrganizationId"))); when(context.currentCompanyId()).thenReturn(number(source.get("CompanyId")));
        when(context.currentBranchId()).thenReturn(number(source.get("BranchesId"))); when(context.currentFinancialYearId()).thenReturn(number(source.get("FinancialYearId")));
        when(context.currentUserId()).thenReturn(number(user.get("Id"))); when(context.currentRoleName()).thenReturn("");
        records=new PurchaseGrnRecordRepository(jdbc,context); writes=spy(new PurchaseGrnWriteRepository(jdbc));
        var manager=new DataSourceTransactionManager(jdbc.getDataSource()); transaction=new TransactionTemplate(manager);
        var form=new com.mst.repositories.PurchaseGrnFormRepository(jdbc,context,records);
        var saveRules=new com.mst.services.PurchaseGrnSaveRules(form,jdbc,context);
        var supplements=new com.mst.services.PurchaseGrnSupplementService(records,writes,jdbc,context);
        var proxy=new ProxyFactory(new PurchaseGrnPersistenceService(records,writes,context,jdbc,supplements,saveRules));
        proxy.addAdvice(new TransactionInterceptor(manager,new AnnotationTransactionAttributeSource())); service=(PurchaseGrnPersistenceService)proxy.getProxy();
    }
    @Test void historyAndRecordScopeMatchDesktop() throws Exception {
        int id=number(source.get("Id")),no=number(source.get("DocNo"));
        var original=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=46,@CanViewAllRecord=?,@EntryUser=?,@GrnNoF=?,@GrnNoT=?,@Activity='GRNFormHistory' WITH RECOMPILE",context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId(),records.canViewAll(46),context.currentUserId(),no,no);
        var history=records.history(46,null,null,null,no,no,"docdate");
        assertEquals(original.size(),history.size());
        for(int i=0;i<original.size();i++)for(var entry:original.get(i).entrySet())assertEquals(entry.getValue(),history.get(i).get(entry.getKey()),entry.getKey());
        if(records.canViewAll(46) || number(source.get("EntryUser"))==context.currentUserId()) {
            var loaded=records.load(id,46);
            assertEquals(writes.details(id).size(),((List<?>)loaded.get("details")).size());
        } else assertThrows(org.springframework.web.server.ResponseStatusException.class,()->records.load(id,46));
        assertThrows(org.springframework.web.server.ResponseStatusException.class,()->records.require(id,143));
        int company=context.currentCompanyId(),branch=context.currentBranchId(),year=context.currentFinancialYearId();
        when(context.currentCompanyId()).thenReturn(-1); assertThrows(org.springframework.web.server.ResponseStatusException.class,()->records.load(id,46));
        when(context.currentCompanyId()).thenReturn(company); when(context.currentBranchId()).thenReturn(-1); assertThrows(org.springframework.web.server.ResponseStatusException.class,()->records.require(id,46));
        when(context.currentBranchId()).thenReturn(branch); when(context.currentFinancialYearId()).thenReturn(-1); assertThrows(org.springframework.web.server.ResponseStatusException.class,()->records.require(id,46));
        when(context.currentFinancialYearId()).thenReturn(year);
    }
    private Map<String,Object> payload(String marker) {
        var payload=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER); payload.putAll(source);
        payload.put("Id",0); payload.put("DocNo",0); payload.put("DocDate",java.sql.Date.valueOf(java.time.LocalDate.now()));
        payload.put("FactoryWeight",0d); payload.put("PartyWeight",0d); payload.put("InwardGatePassId",0); payload.put("GpNo",0); payload.put("SupplierDispatchId",0); payload.put("RemarksHeader",marker);
        var line=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER); line.putAll(writes.details(number(source.get("Id"))).get(0));
        for(String key:List.of("Id","PurchaseOrderId","PurchaseOrderDetailId","PurchaserOrderNo","RefDocumentTypeId","RefDocNoId","RefDocSubIdNo","GatePassInwarDetailId","GdnId","GdnDetailId","GdnDocumentTypeId","WbTicketId","LabId"))line.put(key,0);
        line.put("ItemQty",2d); line.put("GrossWeight",120d); line.put("NetBillWeight",120d); line.put("StockWeight",120d);
        line.put("EBWPerUnit",0d); line.put("EBWTotal",0d); line.put("WtCut",0d); line.put("WtCutTotal",0d); line.put("AdLsWeight",0d);
        payload.put("details",List.of(line)); payload.put("emptyBags",List.of());
        payload.put("purchaseBreakups",List.of(Map.of("Qty",2,"UOM",60,"GrossWeight",120,"NetWeight",120,"BillWeight",120)));
        return payload;
    }
    @Test void bothTypesInsertLoadUpdateStockAndDesktopDeleteRollBack() throws Exception {
        String marker="JGRN-"+UUID.randomUUID().toString().substring(0,12);
        for(int type:List.of(46,143)) transaction.execute(status->{
            status.setRollbackOnly();
            var payload=payload(marker+"-"+type); payload.put("CompanyId",-1); payload.put("EntryUser",-1);
            var saved=service.save(payload,type); int id=number(saved.get("id")); assertTrue(id>0);
            var actual=records.require(id,type); assertEquals(context.currentCompanyId(),number(actual.get("CompanyId"))); assertEquals(context.currentUserId(),number(actual.get("EntryUser")));
            assertEquals(saved.get("docNo"),actual.get("DocNo"));
            assertEquals(1,((List<?>)records.load(id,type).get("details")).size());
            assertEquals(2d,jdbc.queryForObject("SELECT ItemQty FROM dbo.InvGrnDetail WHERE InvGrnId=?",Double.class,id));
            assertTrue(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InventoryTransactions WHERE RefDocumentTypeId=? AND RefDocIdNo=?",Integer.class,type,id)>0,"Desktop inventory posting ran");
            var before=writes.details(id).get(0);
            var updated=service.save(Map.of("id",id,"remarksHeader",marker+"-UPDATE"),type);
            assertEquals(saved.get("docNo"),updated.get("docNo"));
            var after=writes.details(id).get(0);
            for(String key:List.of("ItemQty","GrossWeight","NetBillWeight","StockWeight","ItemUomId","WarehouseId","PackingTypeId","CropYear","CityId"))assertEquals(before.get(key),after.get(key),key);
            assertEquals(type==46?0:1,writes.breakups(id).size());
            assertEquals(context.currentUserId(),jdbc.queryForObject("SELECT ModifyUser FROM dbo.InvGrn WHERE Id=?",Integer.class,id));
            // Numan has Save/Update/View but no Delete. Check the application refuses it,
            // then exercise the desktop SQL deletion contract solely inside this rollback fixture.
            if(!records.hasRight(type,"Delete")) {
                assertEquals(403,assertThrows(org.springframework.web.server.ResponseStatusException.class,()->records.delete(id,type)).getStatus().value());
                assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn WHERE Id=?",Integer.class,id));
                com.mst.repositories.support.ProcExec.run(jdbc,"EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=?,@UserId=?",context.currentOrganizationId(),context.currentCompanyId(),id,type,context.currentUserId());
            } else records.delete(id,type);
            assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn WHERE Id=?",Integer.class,id));
            assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrnDetail WHERE InvGrnId=?",Integer.class,id));
            assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InventoryTransactions WHERE RefDocumentTypeId=? AND RefDocIdNo=?",Integer.class,type,id));
            assertTrue(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.H_InvGrn WHERE Id=?",Integer.class,id)>0,"Desktop deletion archive exists inside rollback");
            return null;
        });
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn WHERE RemarksHeader LIKE ?",Integer.class,marker+"%"));
        Files.writeString(Path.of("migration/purchase/evidence/grn-crud-rollback.txt"),"GoldenAcedb: GRN types 46 and 143 insert/load/update through original full-field header/detail procedures. Session scope, user and action rights enforced. Original validations and inventory posting ran. Omitted loaded detail/breakup collections preserved. Numan's missing Delete permission refused deletion without mutation. The original SQL deletion was exercised directly inside the rollback fixture: it archived headers and removed details and inventory rows. All writes rolled back. Full UI, attachments and feature-specific scenarios remain unverified.\n");
    }
    @Test void stockFailureRollsBackHeaderAndChildren() {
        String marker="JAVA-GRN-FAILURE-"+UUID.randomUUID();
        doThrow(new IllegalStateException("Injected stock failure")).when(writes).validateAndPost(anyInt(),anyInt(),anyInt(),anyInt(),anyInt(),anyInt());
        var failure=assertThrows(IllegalStateException.class,()->service.save(payload(marker),46));
        assertEquals("Injected stock failure",failure.getMessage());
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn WHERE RemarksHeader=?",Integer.class,marker));
    }
    @Test void emptyBagInsertUpdateAndClearUseDesktopProceduresAndRollBack() {
        String marker="JEB-"+UUID.randomUUID().toString().substring(0,8);
        transaction.execute(status->{
            status.setRollbackOnly();
            var payload=payload(marker);
            var detail=((List<Map<String,Object>>)payload.get("details")).get(0);detail.put("PackingTypeId",2);
            int item=number(jdbc.queryForList("EXEC dbo.USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo @OrganizationId=?,@CompanyId=?,@TransactionFlowId=1",context.currentOrganizationId(),context.currentCompanyId()).get(0).get("ItemId"));
            var bag=Map.<String,Object>of("ItemId",item,"TypeId",1,"BagsCondition",1,"ReceivedQty",2d,"PurchaseQty",0d,"Remarks",marker);
            payload.put("emptyBags",List.of(bag));
            int id=number(service.save(payload,46).get("id"));
            assertEquals(1,writes.emptyBags(id).size());assertEquals(2d,((Number)writes.emptyBags(id).get(0).get("ReceivedQty")).doubleValue());
            var edit=new HashMap<String,Object>(bag);edit.put("ReceivedQty",1d);edit.put("PurchaseQty",1d);
            service.save(Map.of("id",id,"emptyBags",List.of(edit)),46);
            var actual=writes.emptyBags(id).get(0);assertEquals(1d,((Number)actual.get("ReceivedQty")).doubleValue());assertEquals(1d,((Number)actual.get("PurchaseQty")).doubleValue());
            var invalid=new HashMap<String,Object>(edit);invalid.put("TypeId",2);
            assertThrows(IllegalArgumentException.class,()->service.save(Map.of("id",id,"emptyBags",List.of(invalid)),46));
            assertEquals(1d,((Number)writes.emptyBags(id).get(0).get("ReceivedQty")).doubleValue());
            service.save(Map.of("id",id,"emptyBags",List.of()),46);assertTrue(writes.emptyBags(id).isEmpty());
            return null;
        });
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn WHERE RemarksHeader=?",Integer.class,marker));
    }
    @Test void lookupIdentifiersAndDependentItemsMatchDesktop() {
        var lookups=new PurchaseGrnLookupRepository(jdbc,context,records);
        for(int type:List.of(46,143)) {
            var lists=lookups.all(type);
            for(String key:List.of("suppliers","warehouses","cropYears","jobLots","cities","vehicleTypes","packingTypes","uoms","transporters","emptyBagTypes","emptyBagItems","bagConditions")) {
                var options=(List<Map<String,Object>>)lists.get(key);
                assertFalse(options.isEmpty(),key+" for type "+type);
                assertTrue(options.stream().allMatch(row->row.get("id")!=null && row.get("name")!=null),key+" identifier/caption");
            }
            assertTrue(((List<?>)lists.get("items")).isEmpty(),"Items await the desktop parent selection");
        }
        int gp=number(source.get("InwardGatePassId"));
        if(gp>0) {
            var loaded=lookups.gatePass(gp,46);
            var original=jdbc.queryForList("EXEC dbo.usp_getItemsFromLabOrPurchaseOrderOrAllByGpId @OrganizationId=?,@CompanyId=?,@Id=?",context.currentOrganizationId(),context.currentCompanyId(),gp);
            assertEquals(original.size(),((List<?>)loaded.get("items")).size());
            assertNotNull(records.breakupContext(gp,number(source.get("Id"))).get("referenceType"));
        }
    }
}
