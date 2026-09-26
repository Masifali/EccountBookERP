package com.mst.services;

import java.util.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

class PurchaseInvoiceCalculationsTest {
    private Map<String,Object> row(Object... pairs){var r=copy(null);for(int k=0;k<pairs.length;k+=2)r.put(pairs[k].toString(),pairs[k+1]);return r;}
    private PurchaseInvoiceCalculations.Configuration config(boolean freight,boolean wagesQty,boolean wagesProduct,boolean subsidiary){return new PurchaseInvoiceCalculations.Configuration(0,freight,wagesQty,wagesProduct,subsidiary);}
    @Test void normalRateCutAndAddLessUseRecoveredWeightSigns(){
        var r=row("GrossWeight",1000,"EBTotalWt",20,"EbPurAgainstWeight",5,"WeightCutTotal",10,"AdLsWeight",15,"EquivalentPoRate",40,"ItemRate",100,"RateCut",3);
        var plus=PurchaseInvoiceCalculations.line(r,1);assertEquals(960,n(plus,"NetBillWeight"));assertEquals(72,n(plus,"RateCutAmount"));assertEquals(2328,n(plus,"ItemAmount"));
        r.put("AdLsWeight",-15);assertEquals(990,n(PurchaseInvoiceCalculations.line(r,1),"NetBillWeight"));
    }
    @Test void marketWeightAlwaysSubtractsAbsoluteAddLessAndRoundsNetAmountOnce(){
        var r=row("GrossWeight",1000,"EBTotalWt",100,"AdLsWeight",-15,"EquivalentPoRate",40,"ItemRate",100,"RateCut",1);
        var result=PurchaseInvoiceCalculations.line(r,2);assertEquals(985,n(result,"NetBillWeight"));assertEquals(2438,n(result,"ItemAmount"));
        r.put("EquivalentPoRate",0);assertThrows(IllegalArgumentException.class,()->PurchaseInvoiceCalculations.line(r,2));
    }
    @Test void freightStaysWithItsGrnAndCommonChargeIsSpreadByWeight(){
        var a=row("InvGrnId",10,"NetBillWeight",100,"ItemQty",10,"ItemAmount",1000);var b=row("InvGrnId",20,"NetBillWeight",300,"ItemQty",30,"ItemAmount",3000);
        var h=row("SupplierCustomerId",5,"WagesAmount",80,"FreightAmount",40);
        PurchaseInvoiceCalculations.bill(h,List.of(a,b),List.of(row("Amount",100)),List.of(row("InvGrnId",10,"FreightAmount",100),row("InvGrnId",20,"FreightAmount",90),row("InvGrnId",0,"FreightAmount",40)),List.of(),List.of(),55,config(false,true,true,false));
        assertEquals(110,n(a,"FreightAmount"));assertEquals(120,n(b,"FreightAmount"));assertEquals(25,n(a,"ExpenseAmount"));assertEquals(20,n(a,"WagesAmount"));assertEquals(10,n(a,"FreightDeduction"));assertEquals(1145,n(a,"BillAmount"));assertEquals(4100,n(h,"BillAmount"));
    }
    @Test void flatCommissionAllocatesByAmountRatherThanWeightAndBagCreditUsesRemainingQty(){
        var a=row("InvGrnId",10,"NetBillWeight",100,"ItemQty",10,"ItemAmount",3000);var b=row("InvGrnId",20,"NetBillWeight",300,"ItemQty",30,"ItemAmount",1000);
        var h=row("SupplierCustomerId",5,"CommissionAgentId",5,"CommissionType","Flat","CommRate",80);
        PurchaseInvoiceCalculations.bill(h,List.of(a,b),List.of(),List.of(),List.of(),List.of(row("TypeId",2,"PurchaseQty",15,"Amount",30)),55,config(false,false,false,false));
        assertEquals(60,n(a,"CommissionAmount"));assertEquals(20,n(b,"CommissionAmount"));assertEquals(20,n(a,"EbPurAgainstWeightAmount"));assertEquals(10,n(b,"EbPurAgainstWeightAmount"));assertEquals(4080,n(h,"BillAmount"));
    }
    @Test void subsidiarySupplierFreightAndDeductionUsePartyId(){
        var a=row("InvGrnId",10,"NetBillWeight",100,"ItemQty",10,"ItemAmount",1000);
        var h=row("SupplierCustomerId",5,"TransporterCreditPartyId",5,"FreightAmount",40,"BrokerAgentId",6,"BrokeryType","Comm Weight","BrokeryRate",1,"BrokeryUom",0);
        PurchaseInvoiceCalculations.bill(h,List.of(a),List.of(),List.of(row("SupplierCustomerId",5,"TansporterId",55,"FreightAmount",80,"Debit",10)),List.of(row("ChartofAccountId",77,"JvDebit",30,"JvCredit",5)),List.of(row("TypeId",1,"Amount",20)),55,config(true,false,false,true));
        assertEquals(0,n(a,"FreightAmount"));assertEquals(100,n(h,"BrokeryAmount"));assertEquals(975,n(h,"BillAmount"));
    }
    @Test void externalBagCreditClearsEarlierLineAllocation(){
        var a=row("ItemQty",10,"NetBillWeight",100,"ItemAmount",1000,"EbPurAgainstWeightAmount",50);
        PurchaseInvoiceCalculations.bill(row(),List.of(a),List.of(),List.of(),List.of(),List.of(row("TypeId",2,"Amount",50,"PurchaseQty",10,"CreditAccountId",88)),55,config(false,false,false,false));
        assertEquals(0,n(a,"EbPurAgainstWeightAmount"));
    }
    @Test void expenseAndBagGridEditsUseCompanyRoundingAndClearManualAmountInputs(){
        var r=PurchaseInvoiceCalculations.supplement("expenses",row("Qty",3,"Rate",2.5),"Rate",0,0,false);
        assertEquals(8,n(r,"Amount"));r.put("Amount",6.45);r=PurchaseInvoiceCalculations.supplement("expenses",r,"Amount",0,1,false);
        assertEquals(6.5,n(r,"Amount"));assertEquals(0,n(r,"Qty"));assertEquals(0,n(r,"Rate"));
        assertEquals(8,n(PurchaseInvoiceCalculations.supplement("emptyBags",row("PurchaseQty",3,"Rate",2.5),"Rate",0,0,false),"Amount"));
    }
    @Test void journalPercentSelectsDebitOrCreditAndRejectsLockedGrnChanges(){
        var r=PurchaseInvoiceCalculations.supplement("journal",row("JvPrcnt",-10,"JvQty",4,"JvRate",3,"JvCredit",77),"JvPrcnt",125,0,false);
        assertEquals(13,n(r,"JvDebit"));assertEquals(0,n(r,"JvCredit"));assertEquals(0,n(r,"JvQty"));assertEquals(0,n(r,"JvRate"));
        assertThrows(IllegalArgumentException.class,()->PurchaseInvoiceCalculations.supplement("journal",row("InvGrnId",8,"JvPrcnt",5),"JvPrcnt",1000,0,false));
        assertThrows(IllegalArgumentException.class,()->PurchaseInvoiceCalculations.supplement("journal",row("JvPrcnt",101),"JvPrcnt",1000,0,false));
        assertThrows(IllegalArgumentException.class,()->PurchaseInvoiceCalculations.supplement("journal",row("JvDebit",5,"JvCredit",10),"JvDebit",1000,0,false));
        assertEquals(12,n(PurchaseInvoiceCalculations.supplement("journal",row("InvGrnId",8,"RowType",1,"JvQty",4,"JvRate",3),"JvRate",0,0,false),"JvCredit"));
    }
    @Test void freightDebitRulesDependOnCompanyConfiguration(){
        var r=row("Debit",8,"FreightAmount",10);
        assertEquals(8,n(PurchaseInvoiceCalculations.supplement("freight",r,"Debit",0,0,false),"Debit"));
        assertThrows(IllegalArgumentException.class,()->PurchaseInvoiceCalculations.supplement("freight",r,"Debit",0,0,true));
        assertThrows(IllegalArgumentException.class,()->PurchaseInvoiceCalculations.supplement("freight",row("InvGrnId",1,"FreightId",2,"FreightAmount",3),"FreightAmount",0,0,false));
    }
    @Test void importedBagAmountUsesRemainingQuantityInSourceOrder(){
        var a=row("ItemQty",10,"EBTotalWt",20,"EquivalentPoRate",40,"ItemRate",100);
        var b=row("ItemQty",20,"EBTotalWt",20,"EquivalentPoRate",40,"ItemRate",200);
        var bag=row("TypeId",2,"PurchaseQty",15);PurchaseInvoiceCalculations.initialBagAmounts(List.of(a,b),List.of(bag));
        assertEquals(75,n(bag,"Amount"));assertEquals(5,n(bag,"Rate"));
    }
}
