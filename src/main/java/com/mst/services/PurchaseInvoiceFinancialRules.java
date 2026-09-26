package com.mst.services;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.*;

/** PurchaseInvoiceFinancial 0549 and GeneralFinancialMethods 0614, document types 56/57.
 * Data is supplied by the original scoped account/configuration queries. No database writes here.
 */
public final class PurchaseInvoiceFinancialRules {
    public record Accounts(Map<Integer,Map<String,Object>> items,Map<Integer,Map<String,Object>> parties,
                           Map<Integer,Integer> lots,int stockAgainst,boolean autoRemarks) {}
    public record Voucher(Map<String,Object> header,List<Map<String,Object>> details) {}
    public static Map<String,Object> copy(Map<String,Object> source) {var m=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER);if(source!=null)m.putAll(source);return m;}
    public static int i(Map<String,Object> m,String key){return (int)n(m,key);}
    public static double n(Map<String,Object> m,String key){Object v=m.get(key);if(v==null||v.toString().isBlank())return 0;double x=v instanceof Number?((Number)v).doubleValue():Double.parseDouble(v.toString());if(!Double.isFinite(x))throw new IllegalArgumentException(key+" must be a finite number");return x;}
    public static String s(Map<String,Object> m,String key){Object v=m.get(key);if(v instanceof Number)return num(((Number)v).doubleValue());return Objects.toString(v,"");}
    private static String num(double n){return BigDecimal.valueOf(n).stripTrailingZeros().toPlainString();}
    private static String round(double n,int scale){return BigDecimal.valueOf(n).setScale(scale,RoundingMode.HALF_EVEN).stripTrailingZeros().toPlainString();}
    private static void fields(Map<String,Object> to,Map<String,Object> from,String... keys){for(String k:keys)to.put(k,from.get(k));}
    private static Map<String,Object> entry(int ac,int against,double debit,double credit,String comments){var m=copy(null);m.put("AccountId",ac);m.put("AgainstAccountId",against);m.put("DebitAmount",debit);m.put("CreditAmount",credit);m.put("Comments",comments);return m;}
    private static void subsidiary(Map<String,Object> row,int party){row.put("SupplierCustomerId",party);row.put("SubsidiaryAccountId",party);row.put("SubsidiaryTypeId",1);}
    private static void againstSubsidiary(Map<String,Object> row,int party){row.put("SubsidiaryAgainstAccountId",party);row.put("SubsidiaryAgainstTypeId",1);}
    private static Map<String,Object> item(Accounts a,int id){var item=a.items().get(id);if(item==null||i(item,"PurchaseGLAC")<=0)throw new IllegalArgumentException("Item GL account not found for item "+id);return item;}
    private static Map<String,Object> party(Accounts a,int id){var p=a.parties().get(id);if(p==null||i(p,"GlAccountId")<=0)throw new IllegalArgumentException("Supplier/customer GL account not found for account "+id);return p;}
    private static int account(Accounts a,Map<String,Object> line){int lot=a.lots().getOrDefault(i(line,"JobLotId"),0);return lot>0?lot:i(item(a,i(line,"ItemId")),"PurchaseGLAC");}
    private static void lineIds(Map<String,Object> row,Map<String,Object> line){fields(row,line,"ItemId","JobLotId");row.put("BranchesId",line.get("BranchId"));}
    private static void mainFields(Map<String,Object> row,Map<String,Object> d){
        fields(row,d,"ItemId","ItemRate","RateCut","RateCutAmount","ItemAmount","JobLotId","GpNo","VehicleNo");
        for(var x:Map.of("QtyIn","ItemQty","WeightIn","NetBillWeight","Freight","FreightAmount","Expenses","ExpenseAmount","Journal","Brokery","Commission","CommissionAmount","OrderNo","PurchaseOrder","DMultiCurrencyId","CurrencyId","DExchangeCurrencyRate","ExchangeRate","DCurrencyAmount","FcyAmount").entrySet())row.put(x.getKey(),d.get(x.getValue()));
        row.put("BranchesId",d.get("BranchId"));
    }
    public static Voucher calculate(Map<String,Object> h,List<Map<String,Object>> details,List<Map<String,Object>> freight,List<Map<String,Object>> journal,List<Map<String,Object>> bags,Accounts accounts){
        int type=i(h,"DocumentTypeId");if(type!=56&&type!=57)throw new IllegalArgumentException("Weighted purchase voucher conversion supports types 56 and 57");
        if(details.isEmpty())throw new IllegalArgumentException("Detail list not found");
        if(accounts.stockAgainst()<=0)throw new IllegalArgumentException("Stock AgainstAc configuration not found");
        int supplier=i(h,"SupplierCustomerId"),supplierGl=i(party(accounts,supplier),"GlAccountId"),commissionGl=0,brokerGl=0;
        if(n(h,"CommAmount")>0)commissionGl=i(party(accounts,i(h,"CommissionAgentId")),"GlAccountId");
        if(n(h,"BrokeryAmount")!=0)brokerGl=i(party(accounts,i(h,"BrokerAgentId")),"GlAccountId");
        String supplierName=s(party(accounts,supplier),"CompanyName");boolean auto=accounts.autoRemarks();
        var out=new ArrayList<Map<String,Object>>();var groups=new LinkedHashMap<List<Object>,Map<String,Object>>();
        String[] totals={"ItemAmount","FreightAmount","ExpenseAmount","CommissionAmount","Brokery","ItemQty","NetBillWeight","WeightCut","RateCut","EBTotalWt","GrossWeight"};
        for(var d:details){
            List<Object> key=List.of(i(d,"ItemId"),n(d,"ItemRate"),i(d,"WarehouseId"));
            if(!groups.containsKey(key))groups.put(key,copy(d));else{var g=groups.get(key);for(String k:totals)g.put(k,n(g,k)+n(d,k));}
            d.put("PurchaseGLAC",i(item(accounts,i(d,"ItemId")),"PurchaseGLAC"));
        }
        String lastText="",headerText="",vehicle="";int firstOrder=0,previousItem=0,index=0;double commissionSum=0,freightSum=0;
        for(var d:groups.values()){
            var it=item(accounts,i(d,"ItemId"));int ac=account(accounts,d);d.put("ItemName",it.get("ItemName"));
            if(index==0){firstOrder=i(d,"PurchaseOrder");vehicle=s(d,"VehicleNo");previousItem=i(d,"ItemId");}
            String summary=s(d,"ItemQty")+" Bags  ( "+s(d,"GrossWeight")+" , "+s(d,"NetBillWeight")+")  ";
            String weightRate=round(n(d,"NetBillWeight")/40,2)+" Mond @ Rs. "+num(n(d,"ItemRate")-n(d,"RateCut"))+"   = Rs. "+s(d,"ItemAmount");
            String named=summary+"of "+s(it,"ItemName")+"  "+weightRate;
            if(index==0)headerText=(s(h,"RemarksHeader").isEmpty()?"":s(h,"RemarksHeader")+" ")+"Purchase "+named;
            else headerText+=previousItem==i(d,"ItemId")?" , "+summary+weightRate:"Purchase "+named;
            String text;
            if(auto){
                text="Purchase "+named;
                if(n(d,"CommissionAmount")>0)text+=" Commission paid @ "+s(h,"CommRate")+"% = Rs. "+round(n(d,"CommissionAmount"),0);
                if(n(h,"BrokeryAmount")>0)text+=" Commission deduct @ "+s(h,"BrokeryRate")+"% = Rs. "+round(n(h,"BrokeryAmount"),0);
                if(n(d,"FreightAmount")>0)text+=" Freight Inward = Rs. "+round(n(d,"FreightAmount"),0)+" Vehicle # "+s(d,"VehicleNo");
                if(i(d,"PurchaseOrder")>0)text+=" Under contract # "+s(d,"PurchaseOrder");
                if(!s(h,"ManualBillNo").isEmpty())text+=" Manual # "+s(h,"ManualBillNo");
                if(!s(d,"WareHouseName").isEmpty())text+=" Warehouse "+s(d,"WareHouseName");text+=" from "+supplierName;
            }else{
                text=s(h,"RemarksHeader")+"   NoOfBags: "+s(d,"ItemQty")+"   "+s(it,"ItemName")+"  GrossWeight: "+s(d,"GrossWeight")+"  Net Weight: "+s(d,"NetBillWeight")+"  Rate:"+s(d,"ItemRate");
                if(n(d,"RateCut")!=0)text+="   RateCut:"+s(d,"RateCut")+" NetRate "+num(n(d,"ItemRate")-Math.abs(n(d,"RateCut")));
                if(n(d,"EBTotalWt")>0)text+="   BagsDeduction: "+s(d,"EBTotalWt");
                if(n(d,"WeightCutTotal")>0)text+="   WeightCut: "+s(d,"WeightCutTotal");
                if(n(d,"AdLsWeight")!=0)text+="   AdLsWeight: "+s(d,"AdLsWeight");
                for(String k:List.of("Expense","Commission","Freight"))if(n(d,k+"Amount")>0)text+="    "+k+" Amount: "+round(n(d,k+"Amount"),0);
                if(n(d,"Brokery")>0)text+="    Brokery Amount: "+round(n(d,"Brokery"),0);
                if(!s(d,"WareHouseName").isEmpty())text+=" Warehouse "+s(d,"WareHouseName");
            }
            lastText=text;if(n(d,"CommissionAmount")>0)commissionSum+=n(d,"CommissionAmount");if(n(d,"FreightAmount")>0)freightSum+=n(d,"FreightAmount");
            var dr=entry(ac,supplierGl,n(d,"ItemAmount"),0,text);mainFields(dr,d);againstSubsidiary(dr,supplier);out.add(dr);
            var cr=entry(supplierGl,ac,0,n(d,"ItemAmount"),text);mainFields(cr,d);subsidiary(cr,supplier);out.add(cr);
            if(n(d,"ExpenseAmount")>0){
                dr=entry(ac,supplierGl,n(d,"ExpenseAmount"),0,auto?text:"Other Expenses");lineIds(dr,d);out.add(dr);
                cr=entry(supplierGl,ac,0,n(d,"ExpenseAmount"),auto?text:"Other Expenses");lineIds(cr,d);subsidiary(cr,supplier);out.add(cr);
            }
            if(n(d,"CommissionAmount")>0){
                if(commissionGl<=0)throw new IllegalArgumentException("CommissionAgent account not found");
                String remark=auto?text:s(h,"CommissionRemarks").isEmpty()?"ItemName "+s(it,"ItemName")+" ItemQty "+s(d,"ItemQty")+" Amount "+s(d,"CommissionAmount"):s(h,"CommissionRemarks");
                dr=entry(ac,commissionGl,n(d,"CommissionAmount"),0,remark);lineIds(dr,d);againstSubsidiary(dr,i(h,"CommissionAgentId"));out.add(dr);
                cr=entry(commissionGl,ac,0,n(d,"CommissionAmount"),remark);lineIds(cr,d);subsidiary(cr,i(h,"CommissionAgentId"));out.add(cr);
            }
            previousItem=i(d,"ItemId");index++;
        }
        if(commissionSum>0)headerText+=" Commission paid @ "+s(h,"CommRate")+"% = Rs. "+round(commissionSum,0);
        if(n(h,"BrokeryAmount")>0)headerText+=" Brokery deduct @ "+s(h,"BrokeryRate")+"% = Rs. "+round(n(h,"BrokeryAmount"),0);
        if(freightSum>0)headerText+=" Freight Inward = Rs. "+round(freightSum,0)+" Vehicle # "+vehicle;
        if(firstOrder>0)headerText+=" Under contract # "+firstOrder;
        if(!s(h,"ManualBillNo").isEmpty())headerText+=" Manual # "+s(h,"ManualBillNo");h.put("RemarksHeader",headerText);
        double totalWeight=groups.values().stream().mapToDouble(d->n(d,"NetBillWeight")).sum(),freightTotal=freight.stream().mapToDouble(f->n(f,"FreightAmount")).sum();
        var transporters=new HashSet<Integer>();for(var f:freight)transporters.add(i(f,"TansporterId"));int transporter=transporters.size()==1?transporters.iterator().next():0;
        for(var d:groups.values()){
            int ac=account(accounts,d),purchase=i(item(accounts,i(d,"ItemId")),"PurchaseGLAC");
            if(n(d,"FreightAmount")>0){var dr=entry(ac,i(h,"TransportAccountId")>0?i(h,"TransportAccountId"):transporter>0?transporter:ac,n(d,"FreightAmount"),0,auto?lastText:"ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : "+num(freightTotal)+" / "+num(totalWeight)+" * "+s(d,"NetBillWeight")+" = "+round(n(d,"FreightAmount"),0));lineIds(dr,d);out.add(dr);}
            if(type==56&&n(h,"FreightAmount")>0&&i(h,"TransportAccountId")>0){
                if(n(d,"FreightDeduction")==0)throw new IllegalArgumentException("The proportionate freight deduction amount is zero. Please check.");
                var dr=entry(i(h,"TransportAccountId"),purchase,n(d,"FreightDeduction"),0,"Freight Deduction "+lastText);fields(dr,d,"ItemId","JobLotId");out.add(dr);
                var cr=entry(purchase,i(h,"TransportAccountId"),0,n(d,"FreightDeduction"),"Freight Deduction "+lastText);fields(cr,d,"ItemId","JobLotId");out.add(cr);
            }
            if(n(d,"Brokery")>0&&n(h,"BrokeryAmount")>0){var dr=entry(ac,ac,n(d,"Brokery"),0,auto?lastText:"ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : "+s(h,"BrokeryAmount")+" / "+num(totalWeight)+" * "+s(d,"NetBillWeight")+" = "+round(n(d,"Brokery"),0));lineIds(dr,d);out.add(dr);}
        }
        var purchaseAccounts=new HashSet<Integer>();for(var d:details)purchaseAccounts.add(i(d,"PurchaseGLAC"));int single=purchaseAccounts.size()==1?purchaseAccounts.iterator().next():0;
        for(var f:freight)if(i(f,"TansporterId")>0)for(String field:List.of("Debit","FreightAmount"))if(n(f,field)>0){
            boolean debit=field.equals("Debit");String comment=auto?lastText:!s(f,"Remarks").isEmpty()?s(f,"Remarks"):(debit?"PartyName :  ":"SupplierName :  ")+supplierName+"  Freight : "+s(f,"FreightAmount");
            var e=entry(i(f,"TansporterId"),single>0?single:i(f,"TansporterId"),debit?n(f,field):0,debit?0:n(f,field),comment);if(i(f,"SupplierCustomerId")>0)subsidiary(e,i(f,"SupplierCustomerId"));e.put("BranchesId",h.get("BranchesId"));out.add(e);
        }
        for(var j:journal)if(i(j,"ChartofAccountId")>0&&(n(j,"JvDebit")>0||n(j,"JvCredit")>0)){
            String comment=auto?lastText:s(j,"JvRemarks");var dr=entry(i(j,"ChartofAccountId"),supplierGl,n(j,"JvDebit"),n(j,"JvCredit"),comment);if(i(j,"SupplierCustomerId")>0)subsidiary(dr,i(j,"SupplierCustomerId"));dr.put("BranchesId",h.get("BranchesId"));out.add(dr);
            var cr=entry(supplierGl,i(j,"ChartofAccountId"),n(j,"JvCredit"),n(j,"JvDebit"),comment);subsidiary(cr,supplier);cr.put("BranchesId",h.get("BranchesId"));out.add(cr);
        }
        if(i(h,"BrokerAgentId")>0&&n(h,"BrokeryAmount")>0){
            String end="  Brokery Rate"+s(h,"BrokeryRate")+" BrokeryType"+s(h,"BrokeryType")+"  BrokeryAmount"+s(h,"BrokeryAmount");
            var dr=entry(supplierGl,brokerGl,n(h,"BrokeryAmount"),0,auto?lastText:"BrokerAgent: "+s(party(accounts,i(h,"BrokerAgentId")),"CompanyName")+end);subsidiary(dr,supplier);dr.put("BranchesId",h.get("BranchesId"));out.add(dr);
            var cr=entry(brokerGl,supplierGl,0,n(h,"BrokeryAmount"),auto?lastText:"SupplierName: "+supplierName+end);againstSubsidiary(cr,i(h,"BrokerAgentId"));cr.put("BranchesId",h.get("BranchesId"));out.add(cr);
        }
        boolean explicit=bags.stream().anyMatch(b->(i(b,"TypeId")==2||i(b,"TypeId")==3)&&i(b,"CreditAccountId")>0);double bagTotal=0;
        for(var b:bags){int bt=i(b,"TypeId");if(bt<1||bt>3||i(b,"ItemId")<=0||n(b,"PurchaseQty")<=0)continue;
            var it=item(accounts,i(b,"ItemId"));int ac=i(it,"PurchaseGLAC");double amount=n(b,"PurchaseQty")*n(b,"Rate");if(amount==0)throw new IllegalArgumentException("Packing Material Amount Field Required");
            if(bt!=1&&explicit&&i(b,"CreditAccountId")==0)throw new IllegalArgumentException("Credit account not found against packing material");
            String remark=s(b,"Remarks").isEmpty()?"ItemName: "+s(it,"ItemName")+" Rate: "+s(b,"Rate")+" Amount: "+num(amount):s(b,"Remarks");int against=bt==1?supplierGl:explicit?i(b,"CreditAccountId"):ac;
            var dr=entry(ac,against,amount,0,remark);dr.put("ItemId",b.get("ItemId"));dr.put("BranchesId",h.get("BranchesId"));out.add(dr);
            if(bt==1||explicit){var cr=entry(against,ac,0,amount,remark);cr.put("ItemId",b.get("ItemId"));cr.put("BranchesId",h.get("BranchesId"));if(bt==1)subsidiary(cr,supplier);out.add(cr);}if(bt!=1)bagTotal+=amount;
        }
        if(bagTotal>0&&!explicit){var allocated=new LinkedHashMap<Integer,Double>();for(var d:details)allocated.merge(i(d,"ItemId"),n(d,"EbPurAgainstWeightAmount"),Double::sum);
            for(var x:allocated.entrySet())if(x.getValue()>0){var it=item(accounts,x.getKey());int ac=i(it,"PurchaseGLAC");var cr=entry(ac,ac,0,x.getValue(),"Empty Bags Purchase against "+s(it,"ItemName")+" or Free of Cost Empty Bags");cr.put("ItemId",x.getKey());out.add(cr);}}
        var vh=copy(null);fields(vh,h,"DocumentTypeId","BillAmount","ManualBillNo","DueDate","DueDays","OrganizationId","CompanyId","FinancialYearId","EntryUser","ModifyUser");
        vh.put("DocumentTypeSrNo",h.get("Id"));vh.put("RefDocNoId",h.get("Id"));vh.put("VoucherCode",h.get("DocNo"));vh.put("VoucherDate",h.get("DocDate"));vh.put("VoucherAmount",h.get("BillAmount"));vh.put("MultiCurrencyId",h.get("CurrencyId"));vh.put("ExchangeCurrencyRate",h.get("ExchangeRate"));vh.put("FcAmount",h.get("FcyAmount"));vh.put("Remarks",headerText);vh.put("RemarksOtherLingo","");vh.put("RefAccountId",supplierGl);vh.put("AgainstAccountId",commissionGl);vh.put("BranchId",h.get("BranchesId"));vh.put("ProjectId",h.get("ProjectsId"));vh.put("ChequeDate",java.sql.Date.valueOf(java.time.LocalDate.now()));vh.put("EntryDate",new java.sql.Timestamp(System.currentTimeMillis()));vh.put("ModifyDate",new java.sql.Timestamp(System.currentTimeMillis()));
        return new Voucher(vh,out);
    }
    private PurchaseInvoiceFinancialRules() {}
}
