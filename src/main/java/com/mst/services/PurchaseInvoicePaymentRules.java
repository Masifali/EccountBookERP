package com.mst.services;

import java.math.*;
import java.time.LocalDate;
import java.util.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** InvfrmPurchaseInvoice payment grid: 2575-3078, final validation:3686-3807. */
public final class PurchaseInvoicePaymentRules {
    private PurchaseInvoicePaymentRules(){}
    private static double round(double v,int digits){return PurchaseDirectInvoiceCalculations.round(v,digits,RoundingMode.HALF_UP);}
    private static LocalDate date(Object v){return v==null?LocalDate.now():LocalDate.parse(v.toString().substring(0,10));}
    public static List<Map<String,Object>> calculate(Map<String,Object> h,List<Map<String,Object>> details,List<Map<String,Object>> existing,boolean byPercent,boolean validate){
        var groups=new LinkedHashMap<Integer,List<Map<String,Object>>>();for(var raw:details){var d=copy(raw);groups.computeIfAbsent(i(d,"PurchaseOrderId"),key->new ArrayList<>()).add(d);}
        double total=details.stream().mapToDouble(d->n(d,"ItemAmount")).sum(),expenses=details.stream().mapToDouble(d->n(d,"ExpenseAmount")).sum();
        double other=n(h,"BillAmount")-total-expenses;var result=new ArrayList<Map<String,Object>>();
        for(var entry:groups.entrySet()){
            int order=entry.getKey();var items=entry.getValue();double itemTotal=items.stream().mapToDouble(d->n(d,"ItemAmount")).sum(),ratio=total>0?itemTotal/total:0;
            double expense=items.stream().mapToDouble(d->n(d,"ExpenseAmount")).sum();double bill=itemTotal+(expense>0?expense:expenses*ratio)+other*ratio;
            var rows=new ArrayList<>(existing.stream().map(PurchaseInvoiceFinancialRules::copy).filter(r->i(r,"PurchaseOrderId")==order).toList());
            if(rows.isEmpty()){var row=copy(null);row.put("PurchaseOrderId",order);row.put("PurchaseOrderNo",items.get(0).get("PurchaseOrder"));row.put("PaymentTermId",h.get("PaymentTermsId"));row.put("DueDays",h.get("DueDays"));rows.add(row);}
            boolean automatic=rows.stream().allMatch(r->n(r,"Amount")<=0&&n(r,"PrcntOfTotal")<=0);
            if(automatic){var row=copy(rows.get(0));row.put("PrcntOfTotal",100);row.put("Amount",bill);row.put("DueDays",Math.max(0,i(h,"DueDays")));row.put("DueDate",date(h.get("DocDate")).plusDays(i(row,"DueDays")).toString());row.put("SystemGeneratedRow",true);rows=new ArrayList<>(List.of(row));}
            for(var row:rows){
                row.put("NetBillAmount",bill);row.put("ItemAmount",itemTotal);row.put("Expense",expense);
                if(!row.containsKey("PaymentTermId"))row.put("PaymentTermId",row.getOrDefault("PaymentTermsId",h.get("PaymentTermsId")));
                if(byPercent||rows.size()==1)row.put("Amount",round(Math.max(0,n(row,"PrcntOfTotal"))*bill/100,4));
                if(row.get("DueDate")==null||s(row,"DueDate").isBlank())row.put("DueDate",date(h.get("DocDate")).plusDays(i(row,"DueDays")).toString());
                if(validate){if(n(row,"Amount")<=0||n(row,"PrcntOfTotal")<=0)throw new IllegalArgumentException("Every payment row requires both percentage and amount");if(i(row,"PaymentTermId")<=0)throw new IllegalArgumentException("Payment Term is required");if(!automatic&&i(row,"PaymentTermId")==2&&i(row,"DueDays")<=0)throw new IllegalArgumentException("Due Days are required for Credit payments");}
            }
            if(validate&&Math.abs(rows.stream().mapToDouble(r->n(r,"PrcntOfTotal")).sum()-100)>0.05)throw new IllegalArgumentException("Payment percentage for order "+order+" must total 100");
            result.addAll(rows);
        }
        if(validate&&Math.abs(result.stream().mapToDouble(r->n(r,"Amount")).sum()-n(h,"BillAmount"))>0.99)throw new IllegalArgumentException("Payment Detail Amount is not equal to Bill Amount");
        return result;
    }
    public static Map<String,Object> edit(List<Map<String,Object>> rows,int index,String changed,LocalDate docDate){
        if(index<0||index>=rows.size())throw new IllegalArgumentException("Select a payment row");var row=copy(rows.get(index));double otherPercent=0,otherAmount=0;
        for(int j=0;j<rows.size();j++){var other=copy(rows.get(j));if(j!=index&&(i(row,"PurchaseOrderId")==0||i(row,"PurchaseOrderId")==i(other,"PurchaseOrderId"))){otherPercent+=n(other,"PrcntOfTotal");otherAmount+=n(other,"Amount");}}
        double net=n(row,"NetBillAmount");
        if("PrcntOfTotal".equals(changed)){if(otherPercent+n(row,"PrcntOfTotal")>100.01)throw new IllegalArgumentException("Total percentage cannot exceed 100");row.put("Amount",PurchaseDirectInvoiceCalculations.round(net*n(row,"PrcntOfTotal")/100,4,RoundingMode.HALF_EVEN));}
        if("Amount".equals(changed)){double value=Math.min(n(row,"Amount"),Math.max(0,net-otherAmount));row.put("Amount",value);row.put("PrcntOfTotal",PurchaseDirectInvoiceCalculations.round(value*100/(net==0?1:net),8,RoundingMode.HALF_EVEN));}
        if("DueDays".equals(changed))row.put("DueDate",docDate.plusDays(i(row,"DueDays")).toString());
        if("DueDate".equals(changed)){LocalDate due=date(row.get("DueDate"));if(due.isBefore(docDate))throw new IllegalArgumentException("Due Date cannot be before Doc Date");row.put("DueDays",java.time.temporal.ChronoUnit.DAYS.between(docDate,due));}
        return row;
    }
}
