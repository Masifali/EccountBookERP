package com.mst.services;

import java.util.*;
import java.time.LocalDate;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

class PurchaseInvoicePaymentRulesTest {
    private Map<String,Object> row(Object... pairs){var r=copy(null);for(int k=0;k<pairs.length;k+=2)r.put(pairs[k].toString(),pairs[k+1]);return r;}
    private Map<String,Object> header(){return row("DocDate","2026-09-23","DueDays",5,"PaymentTermsId",1,"BillAmount",1200);}
    @Test void automaticScheduleAllocatesByOrderAndPreservesExpenseBasis(){
        var details=List.of(row("PurchaseOrderId",11,"PurchaseOrder",100,"ItemAmount",200,"ExpenseAmount",40),row("PurchaseOrderId",12,"PurchaseOrder",101,"ItemAmount",800,"ExpenseAmount",60));
        var result=PurchaseInvoicePaymentRules.calculate(header(),details,List.of(),true,true);assertEquals(2,result.size());assertEquals(260,n(result.get(0),"Amount"));assertEquals(940,n(result.get(1),"Amount"));assertEquals("2026-09-28",result.get(0).get("DueDate"));assertEquals(true,result.get(0).get("SystemGeneratedRow"));
    }
    @Test void manualPercentagesRecalculateAndIncompleteScheduleIsRefused(){
        var details=List.of(row("ItemAmount",1200));var terms=List.of(row("PaymentTermId",1,"PrcntOfTotal",25,"Amount",1),row("PaymentTermId",2,"DueDays",30,"PrcntOfTotal",75,"Amount",1));
        var result=PurchaseInvoicePaymentRules.calculate(header(),details,terms,true,true);assertEquals(300,n(result.get(0),"Amount"));assertEquals(900,n(result.get(1),"Amount"));
        terms.get(1).put("PrcntOfTotal",70);assertThrows(IllegalArgumentException.class,()->PurchaseInvoicePaymentRules.calculate(header(),details,terms,true,true));
    }
    @Test void paymentEditCapsAmountAtRemainderAndComputesDate(){
        var rows=List.of(row("PurchaseOrderId",11,"NetBillAmount",1000,"Amount",600,"PrcntOfTotal",60),row("PurchaseOrderId",11,"NetBillAmount",1000,"Amount",600,"PrcntOfTotal",40,"DueDays",7));
        var result=PurchaseInvoicePaymentRules.edit(rows,1,"Amount",LocalDate.of(2026,9,23));assertEquals(400,n(result,"Amount"));assertEquals(40,n(result,"PrcntOfTotal"));
        result=PurchaseInvoicePaymentRules.edit(rows,1,"DueDays",LocalDate.of(2026,9,23));assertEquals("2026-09-30",result.get("DueDate"));rows.get(1).put("DueDate","2026-09-22");assertThrows(IllegalArgumentException.class,()->PurchaseInvoicePaymentRules.edit(rows,1,"DueDate",LocalDate.of(2026,9,23)));
    }
}
