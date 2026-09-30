package com.mst.repositories;

import com.mst.models.sale.dto.SaleOrderDetailDto;
import com.mst.models.sale.dto.SaleOrderDto;
import com.mst.models.sale.dto.SaleOrderExpenseDto;
import com.mst.models.sale.dto.SaleOrderPaymentScheduleDto;
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
class SaleOrderGridLiveTest {
    private JdbcTemplate jdbc() throws Exception {var p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}var j=new JdbcTemplate(new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password")));j.setQueryTimeout(90);return j;}
    private static int n(Object value){return ((Number)value).intValue();}
    private static BigDecimal d(Object value){return new BigDecimal(String.valueOf(value));}

    @Test void insertLoadUpdateDetailDeleteAndRollback() throws Exception {
        JdbcTemplate jdbc=jdbc();Map<String,Object> user=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName='numan'");
        int uid=n(user.get("ID")),org=n(user.get("OrganizationId")),company=n(user.get("CompanyId")),branch=n(user.get("BranchesId")),year=58;
        CurrentUserContext context=mock(CurrentUserContext.class);when(context.currentUserId()).thenReturn(uid);when(context.currentOrganizationId()).thenReturn(org);when(context.currentCompanyId()).thenReturn(company);when(context.currentBranchId()).thenReturn(branch);when(context.currentFinancialYearId()).thenReturn(year);
        SaleOrderService target=new SaleOrderService();ReflectionTestUtils.setField(target,"jdbcTemplate",jdbc);ReflectionTestUtils.setField(target,"currentUserContext",context);
        var manager=new DataSourceTransactionManager(jdbc.getDataSource());var proxy=new ProxyFactory(target);proxy.addAdvice(new TransactionInterceptor(manager,new AnnotationTransactionAttributeSource()));SaleOrderService service=(SaleOrderService)proxy.getProxy();
        // Reuse valid database keys from an existing order; do not reload every dropdown for this persistence test.
        Map<String,Object> seed=jdbc.queryForMap("SELECT TOP 1 h.OrderSupCustId AS SupplierCustomerId,h.OrderCatagoryId,h.PaymentTermsId,d.OrderItemId,d.OrderItemUOMId,d.OrderItemRateUOMId,d.Crop,d.JobLotId,d.PackingTypeID,d.WarehouseId,d.NetWeight/NULLIF(d.OrderItemQty,0) AS Equivalent FROM dbo.SaleOrder h JOIN dbo.SaleOrderDetail d ON d.SaleOrderId=h.Id WHERE h.OrganizationId=? AND h.CompanyId=? AND h.BranchesId=? AND h.ActionId<>3 AND d.OrderItemQty>0 AND d.NetWeight>0 AND d.JobLotId>0 AND d.PackingTypeID>0 AND d.WarehouseId>0 AND d.OrderItemUOMId>0 AND d.OrderItemRateUOMId>0 ORDER BY h.Id DESC,d.Id",org,company,branch);
        BigDecimal equivalent=d(seed.get("Equivalent"));
        Map<String,Object> expenseItem=jdbc.queryForMap("SELECT TOP 1 Id,OtherItemName FROM dbo.InventoryItemsOther WHERE OrganizationId=? AND CompanyId=? ORDER BY Id",org,company);
        String marker="JAVA-SO-GRID-ROLLBACK-"+System.nanoTime();
        var tx=new TransactionTemplate(manager);
        tx.execute(status->{
            SaleOrderDto order=new SaleOrderDto();order.setVoucherCode(service.generateNextSaleOrderDocNo());order.setOrderDate(LocalDate.now().toString());order.setDueDate(LocalDate.now().plusDays(7).toString());order.setDeliveryStartDate(LocalDate.now().toString());order.setCustomerId(n(seed.get("SupplierCustomerId")));order.setBranchId(branch);order.setOrderCategoryId(n(seed.get("OrderCatagoryId")));order.setPaymentTermId(n(seed.get("PaymentTermsId")));order.setRemarks(marker);
            SaleOrderDetailDto line=new SaleOrderDetailDto();line.setItemId(n(seed.get("OrderItemId")));line.setCropYear(Objects.toString(seed.get("Crop"),""));line.setJobLotId(n(seed.get("JobLotId")));line.setPackingTypeId(n(seed.get("PackingTypeID")));line.setPackUomId(n(seed.get("OrderItemUOMId")));line.setRateUomId(n(seed.get("OrderItemRateUOMId")));line.setQuantity(BigDecimal.ONE);line.setWeight(equivalent);line.setRate(new BigDecimal("100"));line.setAmount(new BigDecimal("100"));line.setWarehouseId(n(seed.get("WarehouseId")));line.setWeightCut(new BigDecimal("0.25"));line.setActionTypeId(1);order.setLineItems(new ArrayList<>(List.of(line)));
            SaleOrderExpenseDto expense=new SaleOrderExpenseDto();expense.setItemId(n(expenseItem.get("Id")));expense.setItemName(Objects.toString(expenseItem.get("OtherItemName")));expense.setQuantity(new BigDecimal("2"));expense.setRate(new BigDecimal("5"));expense.setAmount(new BigDecimal("10"));order.setExpenseItems(new ArrayList<>(List.of(expense)));
            SaleOrderPaymentScheduleDto first=payment(n(seed.get("PaymentTermsId")),"25","25","first"), second=payment(n(seed.get("PaymentTermsId")),"75","75","second");order.setPaymentSchedules(new ArrayList<>(List.of(first,second)));
            Map<String,Object> inserted=service.saveSaleOrder(order);assertEquals(Boolean.TRUE,inserted.get("success"),Objects.toString(inserted.get("message"),""));int id=n(inserted.get("id"));Map<String,Object> loaded=service.getSaleOrderById(id);assertEquals(marker,loaded.get("RemarksHeader"));List<Map<String,Object>> details=(List<Map<String,Object>>)loaded.get("lineItems");assertEquals(1,details.size());assertEquals(0,new BigDecimal(String.valueOf(details.get(0).get("BagWeight"))).compareTo(new BigDecimal("0.25")));
            var expenses=(List<Map<String,Object>>)loaded.get("customerExpenses");assertEquals(1,expenses.size());assertEquals(0,d(expenses.get(0).get("Amount")).compareTo(new BigDecimal("10")));assertEquals("OtherItemName : "+expense.getItemName().trim()+"  2  @5",expenses.get(0).get("Remarks"));
            var payments=(List<Map<String,Object>>)loaded.get("paymentSchedules");assertEquals(2,payments.size());assertEquals(0,d(payments.get(0).get("Amount")).compareTo(new BigDecimal("25")));assertEquals("first",payments.get(0).get("PaymentRemarks"));assertEquals(0,d(payments.get(1).get("Amount")).compareTo(new BigDecimal("75")));
            first.setAmount(new BigDecimal("100"));first.setPercentOfTotal(new BigDecimal("100"));first.setRemarks("replacement");order.setPaymentSchedules(new ArrayList<>(List.of(first)));expense.setAmount(new BigDecimal("20"));expense.setRemarks("updated expense");
            line.setId(n(details.get(0).get("Id")));line.setActionTypeId(2);line.setRemarks("updated detail");order.setId(id);order.setRemarks(marker+"-UPDATED");Map<String,Object> updated=service.saveSaleOrder(order);assertEquals(Boolean.TRUE,updated.get("success"),Objects.toString(updated.get("message"),""));assertEquals(marker+"-UPDATED",service.getSaleOrderById(id).get("RemarksHeader"));
            Map<String,Object> reloaded=service.getSaleOrderById(id);
            expenses=(List<Map<String,Object>>)reloaded.get("customerExpenses");payments=(List<Map<String,Object>>)reloaded.get("paymentSchedules");
            assertEquals(1,expenses.size());assertEquals("updated expense",expenses.get(0).get("Remarks"));assertEquals(0,d(expenses.get(0).get("Amount")).compareTo(new BigDecimal("20")));
            assertEquals(1,payments.size());assertEquals("replacement",payments.get(0).get("PaymentRemarks"));assertEquals(0,d(payments.get(0).get("Amount")).compareTo(new BigDecimal("100")));
            expense.setAmount(BigDecimal.ZERO);assertEquals(Boolean.TRUE,service.saveSaleOrder(order).get("success"));assertTrue(((List<?>)service.getSaleOrderById(id).get("customerExpenses")).isEmpty());
            status.setRollbackOnly();return null;
        });
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM dbo.SaleOrder WHERE RemarksHeader LIKE ?",Integer.class,marker+"%"));
        Files.createDirectories(Path.of("migration/sale/evidence"));Files.writeString(Path.of("migration/sale/evidence/sale-order-grids-rollback.txt"),"GoldenAcedb Sale Order: expense insert/load/update/zero removal; payment 25/75 insert/load and replacement with one 100% row passed through original procedures. Automatic expense remarks verified. Entire transaction rolled back; no test header remained.\n");
    }
    private static SaleOrderPaymentScheduleDto payment(int term,String percent,String amount,String remarks){
        var p=new SaleOrderPaymentScheduleDto();p.setPaymentTermId(term);p.setPercentOfTotal(new BigDecimal(percent));p.setAmount(new BigDecimal(amount));p.setRemarks(remarks);p.setDueDays(7);p.setDueDate(LocalDate.now().plusDays(7).toString());return p;
    }
}
