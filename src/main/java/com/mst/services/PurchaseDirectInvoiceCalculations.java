package com.mst.services;

import java.math.*;
import java.util.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/**
 * InvfrmPurchasedirectInvoice (DocumentTypeId 57) calculations, lines 3922-4452 and 4234-4392.
 * Math.Round(x, n) is .NET ToEven; Math.Round(x, n, AwayFromZero) and the ToString("#,##0.###") display formats round
 * half away from zero (HALF_UP here).
 */
public final class PurchaseDirectInvoiceCalculations {
    private PurchaseDirectInvoiceCalculations(){}
    private static double sum(List<Map<String,Object>> rows,String field){return rows.stream().mapToDouble(row->n(copy(row),field)).sum();}
    private static double amount(double value,int digits){return round(value,digits,RoundingMode.HALF_UP);}
    /**
     * TotalCommissionAmount :4139 / TotalBrokeryAmount :4394. "Comm Weight" divides the bill weight by the UOM text; with no
     * UOM the division gives Infinity (or NaN), which Conversion.ToDouble reads back as 0 (Conversion :151) - so 0 here.
     */
    private static double commission(String type,double rate,double uom,double total,double weight,int digits,boolean percentageAlias){
        double value=switch(type){case "Flat"->rate;case "Percent"->total*rate/100;case "Percentage"->percentageAlias?total*rate/100:0;case "Comm Weight"->uom==0?0:weight/uom*rate;default->0;};
        return Double.isFinite(value)?amount(value,digits):0;
    }
    /** txtcommamount after TotalCommissionAmount :4139 ("Percent" or "Percentage"). */
    public static double commissionAmount(Map<String,Object> h,List<Map<String,Object>> details,int digits){
        if(s(h,"CommRate").isBlank())return 0;
        return commission(s(h,"CommissionType"),n(h,"CommRate"),n(h,"UomScheduleIdCmRate"),sum(details,"ItemAmount"),sum(details,"NetBillWeight"),digits,true);
    }
    /** The "AccountId" cell of a Supplier AddLess row (party id with subsidiary accounts, else chart account id). */
    private static int journalAccount(Map<String,Object> j){return i(j,"SupplierCustomerId")>0?i(j,"SupplierCustomerId"):i(j,"ChartofAccountId");}
    /** ExpProportion :3922, FreightProportion :3955, BillProportion :4001, CommissionProportion :4023, BillAmount :4068. */
    public static void bill(Map<String,Object> h,List<Map<String,Object>> details,List<Map<String,Object>> expenses,List<Map<String,Object>> freight,List<Map<String,Object>> journal,List<Map<String,Object>> bags,int supplierGl,boolean freightToExpense,int digits){
        double total=sum(details,"ItemAmount"),qty=sum(details,"ItemQty"),weight=sum(details,"NetBillWeight"),expense=sum(expenses,"Amount"),freightTotal=amount(sum(freight,"FreightAmount"),digits);
        double comm=commissionAmount(h,details,digits);
        // TotalBrokeryAmount only knows "Percent"; BillAmount keeps the brokery only with a Brokery Agent row (:4115-4129).
        double broker=i(h,"BrokerAgentId")>0&&!s(h,"BrokeryRate").isBlank()?commission(s(h,"BrokeryType"),n(h,"BrokeryRate"),n(h,"BrokeryUom"),total,weight,digits,false):0;
        if(broker<=0)broker=0;
        h.put("CommAmount",comm);h.put("BrokeryAmount",broker);
        double debit=0,credit=0,freightForSupplier=0;
        // :4082-4088 - the AccountId cell against txtSupplierGLId.
        for(var raw:journal){var j=copy(raw);int account=journalAccount(j);if(account>0){if(account==supplierGl)throw new IllegalArgumentException("You cannot select supplier Account");debit+=n(j,"JvDebit");credit+=n(j,"JvCredit");}}
        // :4094-4101 - the Transporter cell (SupplierCustomerId) against txtSupplierGLId.
        for(var raw:freight){var f=copy(raw);if(i(f,"SupplierCustomerId")>0&&i(f,"SupplierCustomerId")==supplierGl)freightForSupplier+=n(f,"FreightAmount");}
        double bill=amount(total,digits)+amount(expense,digits)+amount(sum(bags,"Amount"),digits)+amount(debit,digits)-amount(credit,digits)+amount(freightForSupplier,digits)-broker;
        if(i(h,"SupplierCustomerId")==i(h,"CommissionAgentId"))bill+=comm;
        h.put("BillAmount",amount(bill,digits));
        for(var d:details){
            d.put("ExpenseAmount",expense>0&&qty>0?amount(expense/qty*n(d,"ItemQty"),digits):0);
            d.put("FreightAmount",!freightToExpense&&freightTotal>0&&weight>0?freightTotal/weight*n(d,"NetBillWeight"):0);
            d.put("CommissionAmount",comm<=0?0:"Percent".equals(s(h,"CommissionType"))?amount(n(d,"ItemAmount")*n(h,"CommRate")/100,digits):weight>0?amount(comm/weight*n(d,"NetBillWeight"),digits):0);
            // BillProportion :4001 - "Item Net Amount".
            d.put("BillAmount",amount(amount(n(d,"ItemAmount"),digits)+amount(n(d,"ExpenseAmount"),digits)+amount(n(d,"FreightAmount"),digits)+amount(n(d,"CommissionAmount"),digits),digits));
        }
    }
    public static double round(double value,int digits,RoundingMode mode){if(!Double.isFinite(value))throw new IllegalArgumentException("Enter a finite numeric value");return BigDecimal.valueOf(value).setScale(digits,mode).doubleValue();}
    /**
     * TotalWeight :4319 (new record with the Save right only), Total :4234 and AmountCaluculation :4349 for the Detail editor.
     * {@code changed} is the field the user edited ("EBWeight" = PMW/Unit, "EBTotalWt" = PMW Total, "WeightCut", "WeightCutTotal", ...).
     */
    public static Map<String,Object> line(Map<String,Object> input,String changed,boolean newInvoice,boolean bagsAgainstWeight,double packEquivalent,double rateEquivalent,int digits){
        var r=copy(input);double qty=n(r,"ItemQty"),gross=n(r,"GrossWeight"),empty=n(r,"EBTotalWt"),cut=n(r,"WeightCutTotal");
        // txtGrossWeight.Text = (PackUOM * Qty).ToString("#,##0.##")
        if(newInvoice&&Set.of("ItemQty","ItemUOMId").contains(changed))gross=round(qty*packEquivalent,2,RoundingMode.HALF_UP);
        // Tag PmUnit: total = unit * qty ("#,##0.###"); otherwise unit = total / qty ("#,##0.####").
        if("EBWeight".equals(changed))empty=round(qty*n(r,"EBWeight"),3,RoundingMode.HALF_UP);
        else r.put("EBWeight",qty==0?0:round(empty/qty,4,RoundingMode.HALF_UP));
        if("WeightCut".equals(changed))cut=round(qty*n(r,"WeightCut"),3,RoundingMode.HALF_UP);
        else r.put("WeightCut",qty==0?0:round(cut/qty,4,RoundingMode.HALF_UP));
        // Weight: an EmptyBags row of Type 2 (Purchase Against Weight) keeps the bag weight in the bill weight (:4305).
        double stock=round(gross,2,RoundingMode.HALF_EVEN)-empty-round(cut,2,RoundingMode.HALF_EVEN)+n(r,"AdLsWeight");
        double bill=round(stock+(bagsAgainstWeight?empty:0),2,RoundingMode.HALF_EVEN);
        r.put("GrossWeight",gross);r.put("EBTotalWt",empty);r.put("WeightCutTotal",cut);r.put("NetBillWeight",bill);
        if(gross>0)r.put("NetStockWeight",round(stock,3,RoundingMode.HALF_UP));
        // AmountCaluculation: without weight, rate UOM or rate the amount is 0 and Rate Cut Total keeps its old value.
        if(bill>0&&rateEquivalent>0&&n(r,"ItemRate")>0){
            double itemAmount=round(bill/rateEquivalent*n(r,"ItemRate"),digits,RoundingMode.HALF_UP),rateCut=round(bill/rateEquivalent*n(r,"RateCut"),digits,RoundingMode.HALF_UP);
            r.put("RateCutAmount",rateCut);r.put("ItemAmount",round(itemAmount-rateCut,digits,RoundingMode.HALF_UP));
        }else r.put("ItemAmount",0);
        return r;
    }
}
