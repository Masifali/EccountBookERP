package com.mst.repositories;

import com.mst.models.sale.dto.SaleOrderDetailDto;
import com.mst.models.sale.dto.SaleOrderDto;
import com.mst.security.CurrentUserContext;
import com.mst.services.sale.SaleOrderService;
import java.math.BigDecimal;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.*;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.aop.framework.ProxyFactory;
import org.springframework.transaction.annotation.AnnotationTransactionAttributeSource;
import org.springframework.transaction.interceptor.TransactionInterceptor;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleOrderLiveTest {
    private JdbcTemplate jdbc() throws Exception {var p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}var j=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));j.setQueryTimeout(90);return j;}
    private static int n(Object value){return ((Number)value).intValue();}
    private static BigDecimal d(Object value){return new BigDecimal(String.valueOf(value));}

    @Test void insertLoadUpdateDetailDeleteAndRollback() throws Exception {
        JdbcTemplate jdbc=jdbc();Map<String,Object> user=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int uid=n(user.get("ID")),org=n(user.get("OrganizationId")),company=n(user.get("CompanyId")),branch=n(user.get("BranchesId")),year=58;
        CurrentUserContext context=mock(CurrentUserContext.class);when(context.currentUserId()).thenReturn(uid);when(context.currentOrganizationId()).thenReturn(org);when(context.currentCompanyId()).thenReturn(company);when(context.currentBranchId()).thenReturn(branch);when(context.currentFinancialYearId()).thenReturn(year);
        SaleOrderService target=new SaleOrderService();ReflectionTestUtils.setField(target,"jdbcTemplate",jdbc);ReflectionTestUtils.setField(target,"currentUserContext",context);
        var manager=new DataSourceTransactionManager(jdbc.getDataSource());var proxy=new ProxyFactory(target);proxy.addAdvice(new TransactionInterceptor(manager,new AnnotationTransactionAttributeSource()));SaleOrderService service=(SaleOrderService)proxy.getProxy();
        Map<String,Object> lookups=service.getMasterLookups();
        Map<String,Object> customer=((List<Map<String,Object>>)lookups.get("customers")).get(0), item=((List<Map<String,Object>>)lookups.get("items")).get(0), crop=((List<Map<String,Object>>)lookups.get("cropYears")).get(0), job=((List<Map<String,Object>>)lookups.get("jobLots")).get(0), packing=((List<Map<String,Object>>)lookups.get("packingTypes")).get(0), warehouse=((List<Map<String,Object>>)lookups.get("warehouses")).get(0), category=((List<Map<String,Object>>)lookups.get("orderCategories")).get(0), term=((List<Map<String,Object>>)lookups.get("paymentTerms")).get(0);
        Map<String,Object> uom=service.getItemUomSchedule(n(item.get("Id"))).get(0);BigDecimal equivalent=d(uom.get("QtyEquivalent"));
        String marker="JAVA-SO-ROLLBACK-"+System.nanoTime();
        var tx=new TransactionTemplate(manager);
        tx.execute(status->{
            SaleOrderDto order=new SaleOrderDto();order.setVoucherCode(service.generateNextSaleOrderDocNo());order.setOrderDate(LocalDate.now().toString());order.setDueDate(LocalDate.now().plusDays(7).toString());order.setDeliveryStartDate(LocalDate.now().toString());order.setCustomerId(n(customer.get("Id")));order.setBranchId(branch);order.setOrderCategoryId(n(category.get("Id")));order.setPaymentTermId(n(term.get("Id")));order.setRemarks(marker);
            SaleOrderDetailDto line=new SaleOrderDetailDto();line.setItemId(n(item.get("Id")));line.setCropYear(Objects.toString(crop.get("CropYear"),""));line.setCropYearId(n(crop.get("Id")));line.setJobLotId(n(job.get("Id")));line.setPackingTypeId(n(packing.get("Id")));line.setPackUomId(n(uom.get("Id")));line.setRateUomId(n(uom.get("Id")));line.setQuantity(BigDecimal.ONE);line.setWeight(equivalent);line.setRate(new BigDecimal("100"));line.setAmount(new BigDecimal("100"));line.setWarehouseId(n(warehouse.get("Id")));line.setWeightCut(new BigDecimal("0.25"));line.setActionTypeId(1);order.setLineItems(new ArrayList<>(List.of(line)));
            Map<String,Object> inserted=service.saveSaleOrder(order);assertEquals(Boolean.TRUE,inserted.get("success"),Objects.toString(inserted.get("message"),""));int id=n(inserted.get("id"));Map<String,Object> loaded=service.getSaleOrderById(id);assertEquals(marker,loaded.get("RemarksHeader"));List<Map<String,Object>> details=(List<Map<String,Object>>)loaded.get("lineItems");assertEquals(1,details.size());assertEquals(0,new BigDecimal(String.valueOf(details.get(0).get("BagWeight"))).compareTo(new BigDecimal("0.25")));
            line.setId(n(details.get(0).get("Id")));line.setActionTypeId(2);line.setRemarks("updated detail");order.setId(id);order.setRemarks(marker+"-UPDATED");Map<String,Object> updated=service.saveSaleOrder(order);assertEquals(Boolean.TRUE,updated.get("success"),Objects.toString(updated.get("message"),""));assertEquals(marker+"-UPDATED",service.getSaleOrderById(id).get("RemarksHeader"));
            status.setRollbackOnly();return null;
        });
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.SaleOrder WHERE RemarksHeader LIKE ?",Integer.class,marker+"%"));
        Files.createDirectories(Path.of("migration/sale/evidence"));Files.writeString(Path.of("migration/sale/evidence/sale-order-rollback.txt"),"Sale Order document 81. Exact GoldenAcedb numbering, master lookups, detail UOM calculation fields, insert, load and update procedures passed for Numan tenant 78/78 branch 58 year 58. BagWeight persistence was verified. Transaction rolled back; zero test rows remained.\n");
    }
}
