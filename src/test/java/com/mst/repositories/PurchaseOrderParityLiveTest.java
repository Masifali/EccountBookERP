package com.mst.repositories;

import com.fasterxml.jackson.databind.*;
import com.mst.models.dto.*;
import com.mst.security.CurrentUserContext;
import com.mst.services.*;
import java.nio.file.*;
import java.time.LocalDate;
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

@EnabledIfSystemProperty(named="purchase.live", matches="true")
class PurchaseOrderParityLiveTest {
    private static int n(Object v) { return v instanceof Number ? ((Number)v).intValue() : 0; }
    private JdbcTemplate jdbc() throws Exception {
        var p = new Properties();
        try (var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))) { p.load(in); }
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"), p.getProperty("spring.datasource.username"), p.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60); return jdbc;
    }

    @Test void invoiceNumbersMatchAllFiveDesktopDocumentTypes() throws Exception {
        var jdbc=jdbc();
        var user=jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int org=n(user.get("OrganizationId")), company=n(user.get("CompanyId")), branch=n(user.get("BranchesId"));
        int year=jdbc.queryForObject("SELECT TOP 1 FinancialYearId FROM dbo.PurchaseOrder WHERE OrganizationId=? AND CompanyId=? AND DocumentTypeId=41 ORDER BY Id DESC",Integer.class,org,company);
        var repository=new PurchaseInvoiceNumberingRepository(jdbc);
        var evidence=new StringBuilder("GoldenAcedb desktop Sp_InvPurchaseInvoice_GetAllMethod numbering parity. Organization/Company/FinancialYear/Branch passed explicitly.\n");
        for (int type : new int[]{56,57,59,61,138}) {
            var number=jdbc.queryForMap("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity=?",org,company,type,year,"GenerateCode");
            assertEquals(n(number.get("DocNo")),repository.next(org,company,year,type));
            var branchNumber=jdbc.queryForMap("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity=?",org,company,type,year,branch,"GenerateBranchSrNo");
            assertEquals(n(branchNumber.get("BranchSrNo")),repository.nextBranch(org,company,year,branch,type));
            evidence.append("Type ").append(type).append(": document and branch numbering match desktop procedure.\n");
        }
        Files.createDirectories(Path.of("migration/purchase/evidence")); Files.writeString(Path.of("migration/purchase/evidence/invoice-numbering.txt"),evidence);
    }

    @Test void grnNumbersMatchBothDesktopDocumentTypes() throws Exception {
        var jdbc=jdbc();
        var user=jdbc.queryForMap("SELECT OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int org=n(user.get("OrganizationId")),company=n(user.get("CompanyId")),branch=n(user.get("BranchesId"));
        int year=jdbc.queryForObject("SELECT TOP 1 FinancialYearId FROM dbo.InvGrn WHERE OrganizationId=? AND CompanyId=? AND BranchesId=? ORDER BY Id DESC",Integer.class,org,company,branch);
        var numbers=new GrnNumberingRepository(jdbc);
        for(int type:List.of(46,143)) {
            var row=jdbc.queryForMap("EXEC dbo.Sp_InvGrn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GenerateInvGrnCode'",org,company,type,year,branch);
            assertEquals(n(row.get("DocNo")),numbers.next(org,company,branch,year,type));
            int expected=jdbc.queryForObject("SELECT ISNULL(MAX(DocNo),0)+1 FROM dbo.InvGrn WHERE OrganizationId=? AND CompanyId=? AND DocumentTypeId=? AND FinancialYearId=? AND BranchesId=?",Integer.class,org,company,type,year,branch);
            assertEquals(expected,numbers.next(org,company,branch,year,type));
        }
        Files.writeString(Path.of("migration/purchase/evidence/grn-numbering.txt"),"GoldenAcedb: types 46 (GRN) and 143 (Sale Return GRN) numbering matches the original GenerateInvGrnCode procedure and rows for the active organization, company, branch and financial year. No writes.\n");
    }

    @Test void desktopReadNumberingInsertUpdateAttachmentsAndRollback() throws Exception {
        var jdbc=jdbc();
        var user=jdbc.queryForMap("SELECT Id,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int org=n(user.get("OrganizationId")), company=n(user.get("CompanyId")), uid=n(user.get("Id"));
        var source=jdbc.queryForMap("SELECT TOP 1 p.* FROM dbo.PurchaseOrder p WHERE p.DocumentTypeId=41 AND p.OrganizationId=? AND p.CompanyId=? AND p.EntryUser=? AND EXISTS(SELECT 1 FROM dbo.PurchaseOrderDetail d WHERE d.PurchaseOrderId=p.Id) AND EXISTS(SELECT 1 FROM dbo.PurchaseOrderEmptyBags b WHERE b.PurchaseOrderId=p.Id) ORDER BY p.Id DESC", org, company, uid);
        var context=mock(CurrentUserContext.class);
        when(context.currentOrganizationId()).thenReturn(org); when(context.currentCompanyId()).thenReturn(company);
        when(context.currentUserId()).thenReturn(uid); when(context.currentBranchId()).thenReturn(n(source.get("BranchesId")));
        when(context.currentFinancialYearId()).thenReturn(n(source.get("FinancialYearId"))); when(context.currentRoleName()).thenReturn("");
        var records=new PurchaseOrderRecordRepository(jdbc,context);
        var store=mock(DesktopAttachmentStore.class);
        when(store.store(any(),anyString(),any())).thenAnswer(inv -> "Inv_"+UUID.randomUUID()+".txt");
        var attachments=new PurchaseOrderAttachmentService(jdbc,context,store,records);
        var headers=new PurchaseOrderHeaderRepository(jdbc);
        var target=new PurchaseOrderFullService();
        ReflectionTestUtils.setField(target,"jdbcTemplate",jdbc); ReflectionTestUtils.setField(target,"currentUserContext",context);
        ReflectionTestUtils.setField(target,"purchaseOrderHeaderRepository",headers); ReflectionTestUtils.setField(target,"purchaseOrderRecords",records);
        ReflectionTestUtils.setField(target,"purchaseOrderAttachments",attachments);
        ReflectionTestUtils.setField(target,"purchaseOrderDetails",new PurchaseOrderDetailRepository(jdbc));
        ReflectionTestUtils.setField(target,"purchaseOrderSupplements",new PurchaseOrderSupplementRepository(jdbc));
        var manager=new DataSourceTransactionManager(jdbc.getDataSource());
        var proxy=new ProxyFactory(target); proxy.addAdvice(new TransactionInterceptor(manager,new AnnotationTransactionAttributeSource()));
        var service=(PurchaseOrderFullService)proxy.getProxy();
        int next=service.generateNextDocNo(41);
        assertEquals(headers.nextDocNo(org,company,41,context.currentFinancialYearId()),next);
        var loaded=service.getPurchaseOrderById(n(source.get("Id")));
        assertEquals(n(source.get("DocNo")),loaded.get("docNo"));
        when(context.currentCompanyId()).thenReturn(-1);
        assertThrows(org.springframework.web.server.ResponseStatusException.class,()->service.getPurchaseOrderById(n(source.get("Id"))));
        when(context.currentCompanyId()).thenReturn(company);
        var mapper=new ObjectMapper().configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES,false).configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES,true);
        var dto=mapper.convertValue(loaded,PurchaseOrderFullDto.class);
        dto.setPurchaseOrderMasterId(0); dto.setDocNo(next); dto.setDocDate(LocalDate.now().toString());
        dto.setBranchNo(service.generateNextBranchSrNo(41)); dto.setPaymentTermId(n(source.get("PaymentTermsId")));
        dto.setDueDays(n(source.get("OrderDueDays"))); dto.setPaymentDueDate(LocalDate.now().plusDays(n(source.get("OrderDueDays"))).toString());
        dto.setDeliveryStartDate(LocalDate.now().toString()); dto.setDeliveryTermName(Objects.toString(source.get("DeliveryTerm"),""));
        dto.setCommissionTypeName(Objects.toString(source.get("CommissionType"),"")); dto.setCommUomId(n(source.get("UomScheduleIdCmRate")));
        dto.setBrokeryTypeName(Objects.toString(source.get("BrokeryType"),"")); dto.setBrokeryRateUomId(n(source.get("BrokeryUom")));
        dto.getLineItems().forEach(line -> line.setPurchaseOrderDetailId(0));
        String marker="JAVA-PO-ROLLBACK-"+UUID.randomUUID(); dto.setRemarksHeader(marker);
        var change=new PurchaseOrderAttachmentsDto(); var upload=new InventoryPosItemRequest.Upload();
        upload.name="purchase-parity-test.txt"; upload.base64=Base64.getEncoder().encodeToString(marker.getBytes(java.nio.charset.StandardCharsets.UTF_8));
        change.files.add(upload); dto.setAttachments(change);
        var transaction=new TransactionTemplate(manager);
        int labParameter=jdbc.queryForObject("SELECT TOP 1 Id FROM dbo.InvLabAnalysisItems ORDER BY Id",Integer.class);
        transaction.execute(status -> {
            status.setRollbackOnly();
            var saved=service.savePurchaseOrder(dto,null);
            assertEquals(true,saved.get("success"),Objects.toString(saved.get("message")));
            int id=n(saved.get("id"));
            assertEquals(saved.get("docNo"),jdbc.queryForObject("SELECT DocNo FROM dbo.PurchaseOrder WHERE Id=?",Integer.class,id));
            var actual=service.getPurchaseOrderById(id);
            assertEquals(marker,actual.get("remarksHeader"));
            var files=attachments.list(id); assertEquals(1,files.size());
            assertEquals(upload.name,jdbc.queryForObject("SELECT AttachmentsValues FROM dbo.PurchaseOrder WHERE Id=?",String.class,id));
            var detail=(List<Map<String,Object>>)actual.get("lineItems");
            assertEquals(dto.getLineItems().size(),detail.size());
            // Seed only this disposable order's lab grid, then prove a header update retains it.
            com.mst.repositories.support.ProcExec.call(jdbc,"EXEC dbo.Sp_PurchaseOrderLabDeduction_Insert @Id=?,@PurchaseOrderId=?,@ItemId=?,@AnalysisParameterId=?,@RangeFrom=?,@RangeTo=?,@DeductFrom=?,@StandardValue=?,@DeductionValue=?,@WeightKgs=?,@InvLabAnalysisStandardDeductionPolicyHeaderId=?,@PurchaseOrderDetailId=?",
                    0,id,dto.getLineItems().get(0).getItemId(),labParameter,0,1,"Weight",0,0,0,0,0);
            for (int i=0;i<detail.size();i++) dto.getLineItems().get(i).setPurchaseOrderDetailId(n(detail.get(i).get("purchaseOrderDetailId")));
            dto.setPurchaseOrderMasterId(id); dto.setRemarksHeader(marker+"-UPDATE"); dto.setDocNo(n(saved.get("docNo")));
            double previousTotal=dto.getLineItems().stream().mapToDouble(r->r.getItemAmount()).sum();
            var payment=new PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto();
            payment.setPaymentTermId(dto.getPaymentTermId());payment.setDueDays(Math.max(2,dto.getDueDays()));
            payment.setDueDate(LocalDate.now().plusDays(payment.getDueDays()).toString());payment.setPrcntOfTotal(100d);payment.setAmount(previousTotal);
            dto.setPaymentTermsDetail(List.of(payment));
            var changedLine=dto.getLineItems().get(0);changedLine.setItemAmount(changedLine.getItemAmount()+2117.5);
            change.files.clear(); change.removeAttachmentIds.add(n(files.get(0).get("Id")));
            var updated=service.savePurchaseOrder(dto,null); assertEquals(true,updated.get("success"),Objects.toString(updated.get("message")));
            assertEquals(previousTotal+2117.5,jdbc.queryForObject("SELECT SUM(Amount) FROM dbo.PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId=?",Double.class,id),0.0001);
            assertEquals(marker+"-UPDATE",service.getPurchaseOrderById(id).get("remarksHeader")); assertTrue(attachments.list(id).isEmpty());
            assertEquals(1,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.PurchaseOrderLabDeduction WHERE PurchaseOrderId=? AND DeductFrom='Weight'",Integer.class,id));
            assertEquals("",jdbc.queryForObject("SELECT AttachmentsValues FROM dbo.PurchaseOrder WHERE Id=?",String.class,id));
            // The desktop's explicit row-removal procedure must remove an unreferenced test row.
            int removedId=n(detail.get(0).get("purchaseOrderDetailId"));
            com.mst.repositories.support.ProcExec.call(jdbc,"EXEC dbo.USP_DeletePurchaseOrderDetailIfNotExistInGrn @OrganizationId=?,@CompanyId=?,@OrderId=?,@UserId=?,@OrderDetailIds=?",
                    org,company,id,uid,String.valueOf(removedId));
            assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.PurchaseOrderDetail WHERE Id=? AND PurchaseOrderId=?",Integer.class,removedId,id));
            assertThrows(org.springframework.web.server.ResponseStatusException.class,() -> service.deletePurchaseOrder(id));
            return null;
        });
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.PurchaseOrder WHERE RemarksHeader LIKE ?",Integer.class,marker+"%"));
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.DMSAttachments WHERE Attachment=?",Integer.class,upload.name));
        Files.createDirectories(Path.of("migration/purchase/evidence"));
        Files.writeString(Path.of("migration/purchase/evidence/purchase-order-rollback.txt"),
                "GoldenAcedb: Purchase Order 41; Numan org="+org+", company="+company+", branch="+context.currentBranchId()+", year="+context.currentFinancialYearId()+". Desktop procedure numbering/read, insert, update, child IDs, attachment insert/removal and header attachment fields passed. All test rows rolled back. File storage mocked; physical upload/download and browser checks are separate.\n");
    }
}
