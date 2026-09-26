package com.mst.services;

import java.util.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

class PurchaseDirectInvoiceCalculationsTest {
    private Map<String,Object> line(){var r=copy(null);r.putAll(Map.of("ItemQty",10,"GrossWeight",600,"EBTotalWt",10,"WeightCutTotal",5,"AdLsWeight",2,"ItemRate",2000,"RateCut",10));return r;}
    @Test void weightedRateIncludesEmptyBagsWeightCutAndAddLess(){var r=PurchaseDirectInvoiceCalculations.line(line(),"ItemRate",true,false,60,40,2);assertEquals(587,n(r,"NetBillWeight"));assertEquals(587,n(r,"NetStockWeight"));assertEquals(146.75,n(r,"RateCutAmount"));assertEquals(29203.25,n(r,"ItemAmount"));}
    @Test void bagsAgainstWeightChangeBillButNotStock(){var r=PurchaseDirectInvoiceCalculations.line(line(),"GrossWeight",true,true,60,40,2);assertEquals(597,n(r,"NetBillWeight"));assertEquals(587,n(r,"NetStockWeight"));}
    @Test void quantityChangesGrossOnlyForNewInvoice(){var r=line();r.put("ItemQty",12);assertEquals(720,n(PurchaseDirectInvoiceCalculations.line(r,"ItemQty",true,false,60,40,2),"GrossWeight"));assertEquals(600,n(PurchaseDirectInvoiceCalculations.line(r,"ItemQty",false,false,60,40,2),"GrossWeight"));}
    @Test void focusedUnitFieldsDriveTotalsAndTotalFieldsDriveUnits(){var r=line();r.put("EBWeight",1.25);var result=PurchaseDirectInvoiceCalculations.line(r,"EBWeight",true,false,60,40,2);assertEquals(12.5,n(result,"EBTotalWt"));r.put("WeightCut",2);result=PurchaseDirectInvoiceCalculations.line(r,"WeightCut",true,false,60,40,2);assertEquals(20,n(result,"WeightCutTotal"));result=PurchaseDirectInvoiceCalculations.line(r,"EBTotalWt",true,false,60,40,2);assertEquals(1,n(result,"EBWeight"));}
    @Test void amountHalfIsAwayFromZeroAndZeroInputsStayFinite(){var r=line();r.put("ItemQty",0);r.put("GrossWeight",1);r.put("EBTotalWt",0);r.put("WeightCutTotal",0);r.put("AdLsWeight",0);r.put("ItemRate",2.5);r.put("RateCut",0);var result=PurchaseDirectInvoiceCalculations.line(r,"ItemRate",true,false,1,1,0);assertEquals(3,n(result,"ItemAmount"));assertEquals(0,n(result,"EBWeight"));assertEquals(0,n(result,"WeightCut"));}
    @Test void billDistributesExpensesByQuantityAndFreightByWeight(){
        var h=copy(Map.of("SupplierCustomerId",5,"CommissionAgentId",5,"CommissionType","Percent","CommRate",10,"BrokerAgentId",6,"BrokeryType","Flat","BrokeryRate",20));
        var a=copy(Map.of("ItemQty",1,"NetBillWeight",40,"ItemAmount",100));var b=copy(Map.of("ItemQty",3,"NetBillWeight",60,"ItemAmount",300));
        PurchaseDirectInvoiceCalculations.bill(h,List.of(a,b),List.of(Map.of("Amount",40)),List.of(Map.of("TansporterId",7,"FreightAmount",50)),List.of(Map.of("ChartofAccountId",8,"JvDebit",12,"JvCredit",2)),List.of(Map.of("Amount",10)),7,false,2);
        assertEquals(530,n(h,"BillAmount"));assertEquals(40,n(h,"CommAmount"));assertEquals(20,n(h,"BrokeryAmount"));assertEquals(10,n(a,"ExpenseAmount"));assertEquals(30,n(b,"ExpenseAmount"));assertEquals(20,n(a,"FreightAmount"));assertEquals(30,n(b,"FreightAmount"));assertEquals(10,n(a,"CommissionAmount"));assertEquals(30,n(b,"CommissionAmount"));
    }
    @Test void supplierJournalEntryIsRefused(){assertThrows(IllegalArgumentException.class,()->PurchaseDirectInvoiceCalculations.bill(copy(null),List.of(),List.of(),List.of(),List.of(Map.of("ChartofAccountId",7)),List.of(),7,false,2));}
}
