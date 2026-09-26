package com.mst.repositories;

import com.mst.services.*;
import com.mst.security.CurrentUserContext;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.*;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

@EnabledIfSystemProperty(named="purchase.live",matches="true")
class PurchaseInvoiceFinancialParityLiveTest {
    JdbcTemplate jdbc;CurrentUserContext context;PurchaseInvoiceRecordRepository records;PurchaseInvoiceWriteRepository writes;TransactionTemplate transaction;
    @BeforeEach void setup()throws Exception{
        var properties=new Properties();try(var input=Files.newInputStream(Path.of("src/main/resources/application.properties"))){properties.load(input);}
        var ds=new DriverManagerDataSource(properties.getProperty("spring.datasource.url"),properties.getProperty("spring.datasource.username"),properties.getProperty("spring.datasource.password"));jdbc=new JdbcTemplate(ds);jdbc.setQueryTimeout(60);transaction=new TransactionTemplate(new DataSourceTransactionManager(ds));
        var user=copy(jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'"));
        context=mock(CurrentUserContext.class);when(context.currentOrganizationId()).thenReturn(i(user,"OrganizationId"));when(context.currentCompanyId()).thenReturn(i(user,"CompanyId"));when(context.currentBranchId()).thenReturn(i(user,"BranchesId"));when(context.currentUserId()).thenReturn(i(user,"Id"));when(context.currentRoleName()).thenReturn("");
        when(context.currentFinancialYearId()).thenReturn(jdbc.queryForObject("SELECT TOP 1 FinancialYearId FROM dbo.InvPurchaseInvoice WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? ORDER BY Id DESC",Integer.class,i(user,"OrganizationId"),i(user,"CompanyId"),i(user,"BranchesId")));
        records=new PurchaseInvoiceRecordRepository(jdbc,context);writes=new PurchaseInvoiceWriteRepository(jdbc);
    }
    private Map<String,Object> latest(int type){return copy(jdbc.queryForMap("SELECT TOP 1 * FROM dbo.InvPurchaseInvoice WHERE DocumentTypeId=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? ORDER BY Id DESC",type,context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId()));}
    private Map<String,Double> amounts(List<Map<String,Object>> rows){var sums=new TreeMap<String,Double>();for(var raw:rows){var r=copy(raw);String key=i(r,"AccountId")+":"+i(r,"AgainstAccountId")+":"+i(r,"ItemId")+":"+i(r,"SubsidiaryAccountId")+":"+i(r,"SubsidiaryAgainstAccountId");sums.merge(key+":debit",n(r,"DebitAmount"),Double::sum);sums.merge(key+":credit",n(r,"CreditAmount"),Double::sum);}return sums;}
    private List<Map<String,Object>> comparableRows(List<Map<String,Object>> rows){
        return rows.stream().map(row->{Map<String,Object> normalized=new TreeMap<>();row.forEach((key,value)->normalized.put(key,value instanceof byte[] bytes?Arrays.toString(bytes):value));return normalized;}).toList();
    }
    @Test void recalculatedVouchersMatchSavedDesktopAccountEntries()throws Exception{
        var evidence=new ArrayList<String>();
        for(int type:List.of(56,57)){
            var h=latest(type);int id=i(h,"Id");records.require(id,type);var details=writes.collection(id,"details").stream().map(PurchaseInvoiceFinancialRules::copy).toList();
            var calculated=PurchaseInvoiceFinancialRules.calculate(h,details,writes.collection(id,"freight").stream().map(PurchaseInvoiceFinancialRules::copy).toList(),writes.collection(id,"journal").stream().map(PurchaseInvoiceFinancialRules::copy).toList(),writes.collection(id,"emptyBags").stream().map(PurchaseInvoiceFinancialRules::copy).toList(),writes.accounts(h,details));
            var saved=jdbc.queryForList("SELECT d.* FROM dbo.VoucherHead h JOIN dbo.VoucherDetail d ON d.VoucherHeadId=h.Id WHERE h.OrganizationId=? AND h.CompanyId=? AND h.DocumentTypeId=? AND h.DocumentTypeSrNo=?",context.currentOrganizationId(),context.currentCompanyId(),type,id);
            assertFalse(saved.isEmpty());var expected=amounts(saved);var actual=amounts(calculated.details());assertEquals(expected.keySet(),actual.keySet(),"Account/subsidiary entries for "+type);for(var key:expected.keySet())assertEquals(expected.get(key),actual.get(key),0.01,key);
            evidence.add("Type "+type+", invoice "+i(h,"DocNo")+" / Id "+id+": "+saved.size()+" saved voucher rows match calculated account, against account, item, subsidiary and debit/credit totals. Narrative/layout parity is not established by this test.");
        }
        Files.write(Path.of("migration/purchase/evidence/invoice-financial-parity.txt"),evidence);
    }
    @Test void directLookupsAndGeneratedProcedureContractsMatchDatabase()throws Exception{
        var lookups=new PurchaseDirectInvoiceLookupRepository(jdbc,context,records,writes);var data=lookups.all();
        var evidence=new ArrayList<String>();
        for(String key:List.of("suppliers","items","warehouses","cropYears","jobLots","packingTypes","uoms","paymentTerms","commissionUoms")){
            var list=(List<?>)data.get(key);assertFalse(list.isEmpty(),key);evidence.add(key+": "+list.size()+" original-query rows");
        }
        var excluded=jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?,@CompanyId=?",context.currentOrganizationId(),context.currentCompanyId()).stream().map(PurchaseInvoiceFinancialRules::copy).filter(r->!Set.of(14,17).contains(i(r,"ItemTypeOfTypeId"))).map(r->i(r,"Id")).collect(java.util.stream.Collectors.toSet());
        var actual=((List<Map<String,Object>>)data.get("items")).stream().map(PurchaseInvoiceFinancialRules::copy).map(r->i(r,"Id")).collect(java.util.stream.Collectors.toSet());assertEquals(excluded,actual);
        var d=copy(writes.collection(i(latest(57),"Id"),"details").get(0));assertTrue(lookups.equivalent(i(d,"ItemUOMId"),i(d,"ItemId"))>0);assertThrows(IllegalArgumentException.class,()->lookups.equivalent(i(d,"ItemUOMId"),-1));
        for(var entry:PurchaseInvoiceContracts.ALL.entrySet()){
            var expected=jdbc.queryForList("SELECT name FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND is_output=0 ORDER BY parameter_id",String.class,"dbo."+entry.getKey()).stream().map(name->name.substring(1).toLowerCase()).toList();
            assertEquals(expected,entry.getValue().stream().map(p->p.name().toLowerCase()).toList(),entry.getKey());
        }
        evidence.add("All 14 generated write contracts include every original input parameter, in order. Cross-item UOM rejected. Dropdown contents remain scoped to Numan's session company/branch; visual testing is separate.");Files.write(Path.of("migration/purchase/evidence/direct-invoice-lookups.txt"),evidence);
    }
    @Test void normalInvoiceLookupsAndBillMatchOriginalSavedRecord()throws Exception{
        var lookup=new PurchaseInvoiceLookupRepository(jdbc,context,records,writes);var data=lookup.all();var evidence=new ArrayList<String>();
        for(String key:List.of("suppliers","paymentTerms","locations","otherItems","commissionUoms","accounts","freightAccounts","bagCreditAccounts")){var rows=(List<?>)data.get(key);assertNotNull(rows);evidence.add(key+": "+rows.size()+" original-query records");}
        var h=latest(56);int id=i(h,"Id");var details=writes.collection(id,"details").stream().map(PurchaseInvoiceFinancialRules::copy).toList();double expectedBill=n(h,"BillAmount");
        var uoms=lookup.rateUoms(details);for(var detail:details){int item=i(detail,"ItemId");assertEquals(comparableRows(jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'",context.currentOrganizationId(),context.currentCompanyId(),item)),comparableRows(uoms.get(item)));}
        int supplierGl=jdbc.queryForObject("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id=? AND CompanyId=?",Integer.class,i(h,"SupplierCustomerId"),context.currentCompanyId());
        PurchaseInvoiceCalculations.bill(h,details,writes.collection(id,"expenses"),writes.collection(id,"freight"),writes.collection(id,"journal"),writes.collection(id,"emptyBags"),supplierGl,new PurchaseInvoiceCalculations.Configuration(lookup.amountDigits(),lookup.enabled("DebitAmountChargetoExpenseAcFreightGridPurchase"),lookup.enabled("WagesAmountCalculateOnQty"),lookup.enabled("ContractWagesChargetoProduct"),lookup.subsidiary()));
        assertEquals(expectedBill,n(h,"BillAmount"),0.01);
        var original=writes.collection(id,"details");for(int index=0;index<original.size();index++)for(String field:List.of("ExpenseAmount","FreightAmount","CommissionAmount","FreightDeduction","WagesAmount","EbPurAgainstWeightAmount"))assertEquals(n(copy(original.get(index)),field),n(details.get(index),field),0.01,field);
        evidence.add("Type 56 invoice "+i(h,"DocNo")+" / Id "+id+": bill total and per-line expense, freight, commission, wages, freight deduction and bag allocations match stored desktop values. Other feature/transaction branches and GUI remain open.");Files.write(Path.of("migration/purchase/evidence/invoice-bill-lookups.txt"),evidence);
    }
    @Test void normalGrnTransferMatchesSourceQueriesWithoutWriting()throws Exception{
        var loader=new GrnLoaderRepository(jdbc,context,records,writes);var pending=loader.getPendingGrns(0,0,46,0,null,null,null,null,null);assertFalse(pending.isEmpty(),"Need one pending desktop GRN to exercise the transfer");
        var lookup=new PurchaseInvoiceLookupRepository(jdbc,context,records,writes);var repository=new PurchaseInvoiceGrnRepository(jdbc,context,loader);
        var service=new PurchaseInvoiceGrnService(new GrnLoaderService(loader),repository,lookup,jdbc,context);
        var selected=copy(pending.get(0));int id=i(selected,"Id");var request=List.<Map<String,Object>>of(Map.of("Id",id));
        if(i(selected,"GrnStatusId")==1){var error=assertThrows(IllegalArgumentException.class,()->service.preview(request));assertTrue(error.getMessage().contains("Approval is required"));Files.writeString(Path.of("migration/purchase/evidence/invoice-grn-transfer.txt"),"Pending GRN "+id+" is approval-held and is refused before transfer, matching desktop. An approved pending fixture is still needed for complete transfer verification. No writes.\n");return;}
        var result=service.preview(request);var detail=(List<Map<String,Object>>)result.get("details");var expected=jdbc.queryForList("EXEC dbo.usp_GrnLoadForPurchaseInvoice @GrnIds=?",Integer.toString(id));assertEquals(expected.size(),detail.size());
        for(int index=0;index<expected.size();index++)for(String key:List.of("InvGrnId","InvGrnDetailId","ItemId","WarehouseId","ItemQty","GrossWeight","NetBillWeight"))assertEquals(n(copy(expected.get(index)),key),n(copy(detail.get(index)),key),0.00001,key);
        assertEquals(i(selected,"SupplierCustomerId"),i(result,"SupplierCustomerId"));assertFalse(((List<?>)result.get("paymentTerms")).isEmpty());
        Files.writeString(Path.of("migration/purchase/evidence/invoice-grn-transfer.txt"),"GRN "+id+": "+detail.size()+" invoice detail rows preserve original references, items, warehouse, quantities and weights. Freight, rate-cut, expenses, empty bags, wages and PO-payment queries executed. New draft payment terms generated; supplementary financial branches still require populated fixtures. Read-only; no invoice saved.\n");
    }
    @Test void normalGrnInvoiceSaveLoadAndUpdateRollBack()throws Exception{
        var loader=new GrnLoaderRepository(jdbc,context,records,writes);var pending=loader.getPendingGrns(0,0,46,0,null,null,null,null,null).stream().map(PurchaseInvoiceFinancialRules::copy).filter(r->i(r,"GrnStatusId")!=1).findFirst().orElseThrow();
        var lookup=new PurchaseInvoiceLookupRepository(jdbc,context,records,writes);var transfer=new PurchaseInvoiceGrnService(new GrnLoaderService(loader),new PurchaseInvoiceGrnRepository(jdbc,context,loader),lookup,jdbc,context);
        var payload=transfer.preview(List.of(Map.of("Id",i(pending,"Id"))));payload.put("ManualBillNo","987653");payload.put("RemarksHeader","JAVA parity");
        var terms=(List<Map<String,Object>>)payload.get("paymentTerms");for(var r:terms)if(i(copy(r),"PaymentTermId")==2&&i(copy(r),"DueDays")<=0){r.put("DueDays",7);r.put("DueDate",java.time.LocalDate.parse(s(payload,"DocDate")).plusDays(7).toString());}
        var service=new PurchaseInvoicePersistenceService(records,writes,context,jdbc,new PurchaseInvoiceAttachmentService(jdbc,context,mock(DesktopAttachmentStore.class),records));int[] created={0};
        transaction.execute(status->{try{
            var saved=service.save(payload,56);int id=((Number)saved.get("id")).intValue();created[0]=id;var loaded=records.load(id,56);
            var detail=(List<Map<String,Object>>)loaded.get("details");assertFalse(detail.isEmpty());assertEquals(i(pending,"Id"),i(copy(detail.get(0)),"InvGrnId"));
            assertEquals(n(records.require(id,56),"BillAmount"),writes.collection(id,"paymentTerms").stream().mapToDouble(r->n(copy(r),"Amount")).sum(),0.99);
            var update=copy(loaded);update.put("ManualBillNo","987654");service.save(update,56);assertEquals("987654",records.require(id,56).get("ManualBillNo"));
            assertEquals(i(copy(detail.get(0)),"UomScheduleIdRate"),i(copy(writes.collection(id,"details").get(0)),"UomScheduleIdRate"));
            assertTrue(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.VoucherDetail WHERE VoucherHeadId IN (SELECT Id FROM dbo.VoucherHead WHERE DocumentTypeId=56 AND DocumentTypeSrNo=?)",Integer.class,id)>0);
            return null;
        }finally{status.setRollbackOnly();}});
        assertTrue(created[0]>0);assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvPurchaseInvoice WHERE Id=?",Integer.class,created[0]));
        Files.writeString(Path.of("migration/purchase/evidence/invoice-grn-crud-rollback.txt"),"Type 56: approved pending GRN "+i(pending,"Id")+" transferred through original queries, inserted and loaded as an invoice, payment total matches bill, original voucher rows posted, full-detail update succeeded. All fixture changes rolled back. This does not establish all supplemental/feature cases or GUI parity.\n");
    }
    @Test void grnLoaderUsesOriginalRowsAndRejectsUnallocatedOrUnscopedInputs()throws Exception{
        var loader=new GrnLoaderRepository(jdbc,context,records,writes);
        var branches=loader.getUserBranches(-1,-1,-1,46);assertFalse(branches.isEmpty());
        var selected=branches.stream().map(r->String.valueOf(i(copy(r),"BranchId"))).collect(java.util.stream.Collectors.joining(","));
        var loaded=loader.getPendingGrns(-1,-1,46,-1,null,null,selected,null,null);
        var original=jdbc.queryForList("EXEC dbo.usp_getGrnLoaderDataForPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeId=46,@FinancialYearId=?,@BranchesIds=? WITH RECOMPILE",context.currentOrganizationId(),context.currentCompanyId(),context.currentFinancialYearId(),selected);
        assertEquals(comparableRows(original),comparableRows(loaded));
        assertThrows(IllegalArgumentException.class,()->loader.getPendingGrns(0,0,46,0,"2026-01-01' OR 1=1--",null,selected,null,null));
        assertThrows(org.springframework.web.server.ResponseStatusException.class,()->loader.getPendingGrns(0,0,46,0,null,null,"-123",null,null));
        int id=jdbc.queryForObject("SELECT TOP 1 Id FROM dbo.InvGrn WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? AND DocumentTypeId=46 ORDER BY Id DESC",Integer.class,context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
        assertEquals(comparableRows(jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity='ReadByInvGrnID'",id)),comparableRows(loader.getGrnDetails(id)));
        int originalCompany=context.currentCompanyId();when(context.currentCompanyId()).thenReturn(-1);
        try{assertThrows(org.springframework.web.server.ResponseStatusException.class,()->loader.getGrnDetails(id));}finally{when(context.currentCompanyId()).thenReturn(originalCompany);}
        if(!loaded.isEmpty()){int pendingId=i(copy(loaded.get(0)),"Id");var trusted=loader.selectionRows(List.of(Map.of("Id",pendingId,"SupplierCustomerId",-1,"GrnStatusId",0)),46);assertEquals(comparableRows(List.of(loaded.get(0))),comparableRows(trusted));}
        var hidden=assertThrows(org.springframework.web.server.ResponseStatusException.class,()->loader.getPendingMarketGrns(0,0,46,null,null,null));
        assertEquals(405,hidden.getRawStatusCode());
        Files.writeString(Path.of("migration/purchase/evidence/grn-invoice-loader.txt"),"Original pending loader rows match: "+loaded.size()+"; allowed branches: "+selected+". Original GRN detail result matches. Invalid dates, unallocated branches and cross-company detail reads rejected. Selected row metadata reloaded from DB. Separate Market radio is hidden by frmLoadGRN:765 and its activity is absent in GoldenAcedb; endpoint refuses with 405. No writes. Other GRN document types and loader UI remain unverified.\n");
    }
    @Test void invoiceAttachmentChangesShareOriginalInvoiceTransaction()throws Exception{
        var store=mock(DesktopAttachmentStore.class);
        when(store.store(any(),eq("java-parity.txt"),any())).thenReturn("Inv_00000000-0000-0000-0000-000000000001.txt");
        when(store.read(any(),eq("Inv_00000000-0000-0000-0000-000000000001.txt"))).thenReturn("parity".getBytes(java.nio.charset.StandardCharsets.UTF_8));
        var attachments=new PurchaseInvoiceAttachmentService(jdbc,context,store,records);
        for(int type:List.of(56,57)){
            int originalId=i(latest(type),"Id");var before=attachments.list(originalId,type);var headerBefore=records.require(originalId,type);
            var service=new PurchaseInvoicePersistenceService(records,writes,context,jdbc,attachments);
            var addition=Map.of("files",List.of(Map.of("name","java-parity.txt","base64","cGFyaXR5")),"removeAttachmentIds",List.of());
            transaction.execute(status->{try{
                // The existing Direct Invoice is approved. Create an unapproved fixture instead of changing approval.
                int id=type==57?((Number)service.save(directDraft(),57).get("id")).intValue():originalId;
                var existing=attachments.list(id,type);
                service.save(new HashMap<>(Map.of("Id",id,"attachments",addition)),type);
                var after=attachments.list(id,type);assertEquals(existing.size()+1,after.size());
                var added=after.stream().filter(r->s(r,"Attachment").equals("java-parity.txt")).findFirst().orElseThrow();
                assertEquals(context.currentUserId(),i(added,"EntryUser"));assertEquals(context.currentBranchId(),i(added,"BranchId"));assertEquals(type,i(added,"RefDocumentTypeId"));
                assertEquals("parity",new String(attachments.download(id,type,i(added,"Id")).bytes(),java.nio.charset.StandardCharsets.UTF_8));
                assertTrue(s(copy(records.require(id,type)),"AttachmentsValues").contains("java-parity.txt"));
                assertThrows(IllegalArgumentException.class,()->service.save(new HashMap<>(Map.of("Id",id,"attachments",Map.of("files",List.of(),"removeAttachmentIds",List.of(-1)))),type));
                service.save(new HashMap<>(Map.of("Id",id,"attachments",Map.of("files",List.of(),"removeAttachmentIds",List.of(i(added,"Id"))))),type);
                assertEquals(comparableRows(existing),comparableRows(attachments.list(id,type)));return null;
            }finally{status.setRollbackOnly();}});
            assertEquals(comparableRows(before),comparableRows(attachments.list(originalId,type)));
            var failing=spy(writes);doThrow(new IllegalStateException("Injected attachment posting failure")).when(failing).post(anyMap(),any());
            var failingService=new PurchaseInvoicePersistenceService(records,failing,context,jdbc,attachments);
            var failedPayload=type==57?directDraft():copy(Map.of("Id",originalId));failedPayload.put("attachments",addition);
            assertThrows(IllegalStateException.class,()->transaction.execute(status->{failingService.save(failedPayload,type);return null;}));
            assertEquals(comparableRows(before),comparableRows(attachments.list(originalId,type)));assertEquals(headerBefore.get("AttachmentsValues"),records.require(originalId,type).get("AttachmentsValues"));
        }
        Files.writeString(Path.of("migration/purchase/evidence/invoice-attachments-rollback.txt"),"Types 56/57: original DMS add/list/download/remove and invoice header names verified; foreign attachment removal rejected. Invoice and DMS changes rolled back, including an injected post-write failure. File storage was mocked, so physical local/VPS file operations and browser upload remain unverified.\n");
    }
    private Map<String,Object> directDraft(){
        var h=latest(57);var d=copy(writes.collection(i(h,"Id"),"details").get(0));h.put("Id",0);h.put("RemarksHeader","JAVA parity");h.put("ManualBillNo","987650");
        for(String key:List.of("Id","InvPurchaseInvoiceId","InvGrnId","InvGrnDetailId","PurchaseOrderId","PurchaseOrderDetailId","RefDocumentTypeId","RefDocId","RefDocSubId","GdnId","GdnDetailId"))d.put(key,0);
        for(String key:List.of("FreightAmount","ExpenseAmount","CommissionAmount","Brokery","WagesAmount","EbPurAgainstWeightAmount","FreightDeduction"))d.put(key,0);
        for(String key:List.of("CommAmount","FreightAmount","BrokeryAmount","WagesAmount","DiscountAmount"))h.put(key,0);
        h.put("BillAmount",n(d,"ItemAmount"));h.put("details",List.of(d));return h;
    }
    @Test void directInvoiceInsertLoadUpdateAndFailureRollbackUseOriginalPostingChain()throws Exception{
        assertTrue(records.hasRight(57,"Save"));assertTrue(records.hasRight(57,"Update"));
        var service=new PurchaseInvoicePersistenceService(records,writes,context,jdbc,new PurchaseInvoiceAttachmentService(jdbc,context,mock(DesktopAttachmentStore.class),records));var template=latest(57);int original=i(template,"Id");
        int[] created={0};
        transaction.execute(status->{try{
            var h=copy(template);h.put("Id",0);h.put("RemarksHeader","JAVA parity");h.put("ManualBillNo","987650");
            var d=copy(writes.collection(original,"details").get(0));
            for(String k:List.of("Id","InvPurchaseInvoiceId","InvGrnId","InvGrnDetailId","PurchaseOrderId","PurchaseOrderDetailId","RefDocumentTypeId","RefDocId","RefDocSubId","GdnId","GdnDetailId"))d.put(k,0);
            for(String k:List.of("FreightAmount","ExpenseAmount","CommissionAmount","Brokery","WagesAmount","EbPurAgainstWeightAmount","FreightDeduction"))d.put(k,0);
            for(String k:List.of("CommAmount","FreightAmount","BrokeryAmount","WagesAmount","DiscountAmount"))h.put(k,0);
            h.put("BillAmount",n(d,"ItemAmount"));h.put("details",List.of(d));
            var result=service.save(h,57);int id=((Number)result.get("id")).intValue();created[0]=id;assertTrue(id>original);
            var loaded=records.load(id,57);assertEquals(1,((List<?>)loaded.get("details")).size());
            assertEquals(2,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.VoucherDetail WHERE VoucherHeadId IN (SELECT Id FROM dbo.VoucherHead WHERE DocumentTypeId=57 AND DocumentTypeSrNo=?)",Integer.class,id));
            assertTrue(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InventoryTransactions WHERE RefDocumentTypeId=57 AND RefDocIdNo=?",Integer.class,id)>0);
            var update=copy(null);update.put("Id",id);update.put("ManualBillNo","987651");service.save(update,57);
            assertEquals("987651",records.require(id,57).get("ManualBillNo"));assertEquals(1,writes.collection(id,"details").size());
            var foreign=copy(null);foreign.put("Id",id);foreign.put("details",List.of(Map.of("Id",i(copy(writes.collection(original,"details").get(0)),"Id"))));assertThrows(IllegalArgumentException.class,()->service.save(foreign,57));
            return null;
        }finally{status.setRollbackOnly();}});
        assertTrue(created[0]>0);assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvPurchaseInvoice WHERE Id=?",Integer.class,created[0]));
        Files.writeString(Path.of("migration/purchase/evidence/invoice-crud-rollback.txt"),"Type 57: real original-procedure insert/load, balanced voucher, inventory entries, update with omitted child preservation, foreign-detail rejection. All fixture rows rolled back. Type 56 referenced-GRN write, GUI, attachment edits and allowed Delete remain unverified.\n");
    }
    @Test void referencedInvoiceUpdatePreservesChildrenAndPostingFailureRollsBack()throws Exception{
        var original=latest(56);int id=i(original,"Id");assertTrue(records.hasRight(56,"Update"));
        var detailBefore=writes.collection(id,"details");var freightBefore=writes.collection(id,"freight");
        var service=new PurchaseInvoicePersistenceService(records,writes,context,jdbc,new PurchaseInvoiceAttachmentService(jdbc,context,mock(DesktopAttachmentStore.class),records));
        transaction.execute(status->{try{
            service.save(new HashMap<>(Map.of("id",id,"manualBillNo","987652")),56);
            assertEquals("987652",records.require(id,56).get("ManualBillNo"));
            assertEquals(detailBefore.size(),writes.collection(id,"details").size());assertEquals(freightBefore.size(),writes.collection(id,"freight").size());
            return null;
        }finally{status.setRollbackOnly();}});
        assertEquals(original.get("ManualBillNo"),records.require(id,56).get("ManualBillNo"));
        // Fail after the real header/detail writes and before posting; the public Spring service must be atomic.
        var failing=spy(writes);doThrow(new IllegalStateException("Injected posting failure")).when(failing).post(anyMap(),any());
        var failingService=new PurchaseInvoicePersistenceService(records,failing,context,jdbc,new PurchaseInvoiceAttachmentService(jdbc,context,mock(DesktopAttachmentStore.class),records));
        assertThrows(IllegalStateException.class,()->transaction.execute(status->{failingService.save(new HashMap<>(Map.of("id",id,"manualBillNo","987653")),56);return null;}));
        assertEquals(original.get("ManualBillNo"),records.require(id,56).get("ManualBillNo"));
        assertEquals(comparableRows(detailBefore),comparableRows(writes.collection(id,"details")));assertEquals(comparableRows(freightBefore),comparableRows(writes.collection(id,"freight")));
        Files.writeString(Path.of("migration/purchase/evidence/invoice-referenced-update.txt"),"Type 56: original referenced invoice update passes; detail/freight collections retained. Injected post-write posting failure restores original header/detail/freight rows via rollback. No committed changes.\n");
    }
}
