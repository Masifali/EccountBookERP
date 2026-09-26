package com.mst.services;

import java.math.RoundingMode;
import java.util.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** Type-56 desktop grid events 1490-1542 and allocation/bill events 2347-2407, 4099-4680. */
public final class PurchaseInvoiceCalculations {
    private PurchaseInvoiceCalculations(){}
    public record Configuration(int digits,boolean freightToExpense,boolean wagesOnQty,boolean wagesToProduct,boolean subsidiary){}
    private static double sum(List<Map<String,Object>> rows,String key){return rows.stream().mapToDouble(r->n(copy(r),key)).sum();}
    private static double amount(double value,int digits){return PurchaseDirectInvoiceCalculations.round(value,digits,RoundingMode.HALF_UP);}
    public static Map<String,Object> line(Map<String,Object> row,int invoiceType){
        var r=copy(row);double equivalent=n(r,"EquivalentPoRate");if(equivalent<=0)throw new IllegalArgumentException("Rate UOM must have a positive equivalent");
        double addLess=n(r,"AdLsWeight"),weight=invoiceType==2?n(r,"GrossWeight")-Math.abs(addLess):n(r,"GrossWeight")-n(r,"EBTotalWt")+n(r,"EbPurAgainstWeight")-n(r,"WeightCutTotal")-addLess;
        double cut=weight/equivalent*n(r,"RateCut");r.put("NetBillWeight",weight);r.put("RateCutAmount",cut);r.put("ItemAmount",amount(weight/equivalent*n(r,"ItemRate")-cut,0));return r;
    }
    /** TotalAmtForEmptyBagGrid runs once when GRNs are loaded. */
    public static void initialBagAmounts(List<Map<String,Object>> details,List<Map<String,Object>> bags){
        var included=bags.stream().filter(r->Set.of(2,3).contains(i(r,"TypeId"))).toList();if(included.stream().anyMatch(r->i(r,"CreditAccountId")>0))return;
        double remaining=included.stream().mapToDouble(r->n(r,"PurchaseQty")).sum(),total=0;if(remaining<=0)return;
        for(var d:details){double qty=n(d,"ItemQty"),equivalent=n(d,"EquivalentPoRate"),allocated=Math.min(qty,remaining);total+=qty>0&&equivalent>0?n(d,"EBTotalWt")/qty*allocated/equivalent*n(d,"ItemRate"):0;remaining-=allocated;if(remaining<=0)break;}
        if(total>0)for(var b:included){b.put("Amount",total);b.put("Rate",n(b,"PurchaseQty")>0?total/n(b,"PurchaseQty"):0);}
    }
    /** CellUpdated events for invoice expense, freight, supplier adjustment and bag grids. */
    public static Map<String,Object> supplement(String grid,Map<String,Object> input,String changed,double itemTotal,int digits,boolean freightToExpense){
        var r=copy(input);
        switch(grid){
            case "expenses"->{if(Set.of("Qty","Rate").contains(changed))r.put("Amount",amount(n(r,"Qty")*n(r,"Rate"),digits));if("Amount".equals(changed)){r.put("Qty",0);r.put("Rate",0);r.put("Amount",amount(n(r,"Amount"),digits));}}
            case "emptyBags"->{if("Rate".equals(changed))r.put("Amount",amount(n(r,"PurchaseQty")*n(r,"Rate"),digits));}
            case "freight","journal"->{
                boolean journal=grid.equals("journal"),grn=i(r,"InvGrnId")>0;String qty=journal?"JvQty":"FrQty",rate=journal?"JvRate":"FrRate",percentage=journal?"JvPrcnt":"Percentage",credit=journal?"JvCredit":"FreightAmount",debit=journal?"JvDebit":"Debit";
                if(changed.equals(percentage)){if(Math.abs(n(r,percentage))>100)throw new IllegalArgumentException("Percentage must not exceed 100");if(grn)throw new IllegalArgumentException("You cannot change credit amount against a GRN");double result=itemTotal/100*n(r,percentage);if(journal){r.put(credit,result>0?amount(result,digits):0);r.put(debit,result<=0?amount(Math.abs(result),digits):0);}else if(result>0)r.put(credit,amount(result,digits));r.put(qty,0);r.put(rate,0);}
                if(changed.equals(qty)||changed.equals(rate)){if(grn&&(!journal||i(r,"RowType")!=1))throw new IllegalArgumentException("You cannot change credit amount against a GRN");r.put(credit,amount(n(r,qty)*n(r,rate),digits));r.put(percentage,0);if(journal)r.put(debit,0);}
                if(changed.equals(credit)){if(grn&&(journal||i(r,"FreightId")>0))throw new IllegalArgumentException("You cannot change credit amount against a GRN");r.put(credit,amount(n(r,credit),digits));if((journal||freightToExpense)&&n(r,debit)>0)throw new IllegalArgumentException("Debit side is already added");}
                if(changed.equals(debit)){r.put(debit,amount(n(r,debit),digits));if((journal||freightToExpense)&&n(r,credit)>0)throw new IllegalArgumentException("Credit side is already added");}
            }
            default->throw new IllegalArgumentException("Unsupported invoice grid");
        }
        return r;
    }
    private static double charge(String type,double rate,double equivalent,double total,double weight,int digits,boolean broker){
        double value=switch(type){case "Flat"->rate;case "Percent"->total*rate/100;case "Comm Weight"->{if(broker&&equivalent==0)equivalent=1;if(equivalent<=0&&rate!=0&&weight!=0)throw new IllegalArgumentException("Commission UOM is required");yield equivalent>0?weight/equivalent*rate:0;}default->0;};return amount(value,digits);
    }
    public static void bill(Map<String,Object> h,List<Map<String,Object>> details,List<Map<String,Object>> expenses,List<Map<String,Object>> freight,List<Map<String,Object>> journal,List<Map<String,Object>> bags,int supplierGl,Configuration config){
        double qty=sum(details,"ItemQty"),weight=sum(details,"NetBillWeight"),total=sum(details,"ItemAmount"),expense=sum(expenses,"Amount"),deduction=amount(n(h,"FreightAmount"),config.digits()),wages=n(h,"WagesAmount");
        double commission=charge(s(h,"CommissionType"),n(h,"CommRate"),n(h,"UomScheduleIdCmRate"),total,weight,config.digits(),false);
        double brokery=i(h,"BrokerAgentId")>0?charge(s(h,"BrokeryType"),n(h,"BrokeryRate"),n(h,"BrokeryUom"),total,weight,config.digits(),true):0;
        h.put("CommAmount",commission);h.put("BrokeryAmount",brokery);
        Map<Integer,Double> grnWeight=new HashMap<>(),grnFreight=new HashMap<>();
        for(var d:details)grnWeight.merge(i(d,"InvGrnId"),n(d,"NetBillWeight"),Double::sum);
        double bill=total+expense;
        for(var raw:freight){var f=copy(raw);grnFreight.merge(i(f,"InvGrnId"),n(f,"FreightAmount"),Double::sum);
            boolean same=config.subsidiary()?i(f,"SupplierCustomerId")==i(h,"SupplierCustomerId"):i(f,"TansporterId")==supplierGl;
            if(same)bill+=n(f,"FreightAmount")-n(f,"Debit");
        }
        grnFreight.replaceAll((key,value)->amount(value,config.digits()));
        for(var raw:journal){var j=copy(raw);if(i(h,"SupplierCustomerId")>0&&(i(j,"ChartofAccountId")>0||i(j,"SupplierCustomerId")>0))bill+=n(j,"JvDebit")-n(j,"JvCredit");}
        for(var raw:bags){var b=copy(raw);if(i(b,"TypeId")==1&&n(b,"Amount")>0)bill+=n(b,"Amount");}
        if(i(h,"SupplierCustomerId")==i(h,"CommissionAgentId"))bill+=commission;bill-=brokery;
        if(config.subsidiary()?i(h,"TransporterCreditPartyId")==i(h,"SupplierCustomerId"):i(h,"TransportAccountId")==supplierGl)bill-=n(h,"FreightAmount");
        h.put("BillAmount",amount(bill,config.digits()));
        double bagAmount=0,bagQty=0;boolean externalBagCredit=false;
        for(var raw:bags){var b=copy(raw);if(Set.of(2,3).contains(i(b,"TypeId"))){bagAmount+=n(b,"Amount");bagQty+=n(b,"PurchaseQty");externalBagCredit|=i(b,"CreditAccountId")>0;}}
        double remainingQty=externalBagCredit?0:bagQty,bagRate=bagQty>0&&bagAmount>0?bagAmount/bagQty:0;
        for(var d:details){
            double w=n(d,"NetBillWeight"),q=n(d,"ItemQty"),groupWeight=grnWeight.getOrDefault(i(d,"InvGrnId"),0d),groupFreight=grnFreight.getOrDefault(i(d,"InvGrnId"),0d),globalFreight=grnFreight.getOrDefault(0,0d);
            d.put("ExpenseAmount",expense>0&&qty>0?expense/qty*q:0);
            d.put("FreightAmount",config.freightToExpense()?0:(groupFreight>0&&groupWeight>0?groupFreight/groupWeight*w:0)+(i(d,"InvGrnId")!=0&&globalFreight>0&&weight>0?globalFreight/weight*w:0));
            d.put("FreightDeduction",deduction>0&&weight>0?deduction/weight*w:0);
            d.put("WagesAmount",wages<=0?0:config.wagesOnQty()?(qty>0?wages/qty*q:0):(weight>0?wages/weight*w:0));
            d.put("CommissionAmount",commission<=0?0:Set.of("Flat","Percent").contains(s(h,"CommissionType"))?(total!=0?commission/total*n(d,"ItemAmount"):0):(weight!=0?commission/weight*w:0));
            double allocated=Math.min(q,remainingQty);d.put("EbPurAgainstWeightAmount",allocated*bagRate);remainingQty-=allocated;
            d.put("BillAmount",amount(n(d,"ItemAmount")+n(d,"ExpenseAmount")+n(d,"FreightAmount")-n(d,"FreightDeduction")+n(d,"JournalAmount")+n(d,"CommissionAmount")+(config.wagesToProduct()?n(d,"WagesAmount"):0)-n(d,"EbPurAgainstWeightAmount"),config.digits()));
        }
    }
}
