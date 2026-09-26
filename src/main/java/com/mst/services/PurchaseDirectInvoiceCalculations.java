package com.mst.services;

import java.math.*;
import java.util.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** Direct form lines 4157-4310. Weight rounding uses .NET ToEven; amounts use AwayFromZero. */
public final class PurchaseDirectInvoiceCalculations {
    private PurchaseDirectInvoiceCalculations(){}
    private static double sum(List<Map<String,Object>> rows,String field){return rows.stream().mapToDouble(row->n(copy(row),field)).sum();}
    private static double amount(double value,int digits){return round(value,digits,RoundingMode.HALF_UP);}
    private static double commission(String type,double rate,double equivalent,double total,double weight,int digits){
        return amount(switch(type){case "Flat"->rate;case "Percent","Percentage"->total*rate/100;case "Comm Weight"->{if(equivalent<=0&&rate!=0)throw new IllegalArgumentException("Commission weight UOM is required");yield equivalent>0?weight/equivalent*rate:0;}default->0;},digits);
    }
    /** Direct form ExpProportion/FreightProportion/CommissionProportion/BillAmount, 3844-4118. */
    public static void bill(Map<String,Object> h,List<Map<String,Object>> details,List<Map<String,Object>> expenses,List<Map<String,Object>> freight,List<Map<String,Object>> journal,List<Map<String,Object>> bags,int supplierGl,boolean freightToExpense,int digits){
        double total=sum(details,"ItemAmount"),qty=sum(details,"ItemQty"),weight=sum(details,"NetBillWeight"),expense=sum(expenses,"Amount"),freightTotal=amount(sum(freight,"FreightAmount"),digits);
        double comm=commission(s(h,"CommissionType"),n(h,"CommRate"),n(h,"UomScheduleIdCmRate"),total,weight,digits);
        double broker=i(h,"BrokerAgentId")>0?commission(s(h,"BrokeryType"),n(h,"BrokeryRate"),n(h,"BrokeryUom"),total,weight,digits):0;
        h.put("CommAmount",comm);h.put("BrokeryAmount",broker);
        double debit=0,credit=0,freightForSupplier=0;
        for(var raw:journal){var j=copy(raw);if(i(j,"ChartofAccountId")>0){if(i(j,"ChartofAccountId")==supplierGl)throw new IllegalArgumentException("You cannot select the supplier account in the journal grid");debit+=n(j,"JvDebit");credit+=n(j,"JvCredit");}}
        for(var raw:freight){var f=copy(raw);if(i(f,"TansporterId")==supplierGl)freightForSupplier+=n(f,"FreightAmount");}
        double bill=amount(total,digits)+amount(expense,digits)+amount(sum(bags,"Amount"),digits)+amount(debit,digits)-amount(credit,digits)+amount(freightForSupplier,digits)-broker;
        if(i(h,"SupplierCustomerId")==i(h,"CommissionAgentId"))bill+=comm;
        h.put("BillAmount",amount(bill,digits));
        for(var d:details){
            d.put("ExpenseAmount",expense>0&&qty>0?amount(expense/qty*n(d,"ItemQty"),digits):0);
            d.put("FreightAmount",!freightToExpense&&freightTotal>0&&weight>0?freightTotal/weight*n(d,"NetBillWeight"):0);
            d.put("CommissionAmount",comm<=0?0:"Percent".equals(s(h,"CommissionType"))?amount(n(d,"ItemAmount")*n(h,"CommRate")/100,digits):weight>0?amount(comm/weight*n(d,"NetBillWeight"),digits):0);
        }
    }
    public static double round(double value,int digits,RoundingMode mode){if(!Double.isFinite(value))throw new IllegalArgumentException("Enter a finite numeric value");return BigDecimal.valueOf(value).setScale(digits,mode).doubleValue();}
    public static Map<String,Object> line(Map<String,Object> input,String changed,boolean newInvoice,boolean bagsAgainstWeight,double packEquivalent,double rateEquivalent,int digits){
        var r=copy(input);double qty=n(r,"ItemQty"),gross=n(r,"GrossWeight"),empty=n(r,"EBTotalWt"),cut=n(r,"WeightCutTotal");
        if(newInvoice&&Set.of("ItemQty","ItemUOMId").contains(changed))gross=round(qty*packEquivalent,2,RoundingMode.HALF_EVEN);
        if("EBWeight".equals(changed))empty=round(qty*n(r,"EBWeight"),3,RoundingMode.HALF_EVEN);
        else r.put("EBWeight",qty==0?0:round(empty/qty,4,RoundingMode.HALF_EVEN));
        if("WeightCut".equals(changed))cut=round(qty*n(r,"WeightCut"),3,RoundingMode.HALF_EVEN);
        else r.put("WeightCut",qty==0?0:round(cut/qty,4,RoundingMode.HALF_EVEN));
        double stock=round(gross,2,RoundingMode.HALF_EVEN)-empty-round(cut,2,RoundingMode.HALF_EVEN)+n(r,"AdLsWeight");
        double bill=round(stock+(bagsAgainstWeight?empty:0),2,RoundingMode.HALF_EVEN);
        r.put("GrossWeight",gross);r.put("EBTotalWt",empty);r.put("WeightCutTotal",cut);r.put("NetBillWeight",bill);
        if(gross>0)r.put("NetStockWeight",round(stock,3,RoundingMode.HALF_EVEN));
        double amount=0,rateCut=0;
        if(bill>0&&rateEquivalent>0&&n(r,"ItemRate")>0){amount=round(bill/rateEquivalent*n(r,"ItemRate"),digits,RoundingMode.HALF_UP);rateCut=round(bill/rateEquivalent*n(r,"RateCut"),digits,RoundingMode.HALF_UP);}
        r.put("RateCutAmount",rateCut);r.put("ItemAmount",round(amount-rateCut,digits,RoundingMode.HALF_UP));return r;
    }
}
