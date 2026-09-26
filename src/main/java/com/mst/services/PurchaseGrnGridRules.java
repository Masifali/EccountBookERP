package com.mst.services;

import java.util.*;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;

/** InvFrmGRN.cs:1642-1800, 2384-2428, 4864-4904 and 5153-5321. */
public final class PurchaseGrnGridRules {
    private PurchaseGrnGridRules() {}

    public static List<Map<String,Object>> calculateBreakups(List<Map<String,Object>> input,
            double partyWeight,double factoryWeight,boolean supplierCut,boolean scaleCut) {
        var rows=copy(input);
        double qtyTotal=0,grossTotal=0;
        for(var row:rows) {
            double qty=value(row,"Qty"),uom=value(row,"UOM"),eb=value(row,"EbWeight");
            double gross=qty*uom,empty=Math.abs(qty*eb);
            row.put("GrossWeight",gross);row.put("EBTotal",empty);
            row.put("NetPackSize",uom-Math.abs(eb));row.put("NetWeight",gross-empty);
            qtyTotal+=qty;grossTotal+=gross;
        }
        for(var row:rows) {
            double fraction=qtyTotal>0?value(row,"Qty")/qtyTotal:0;
            double supplierShort=Math.max(0,grossTotal-partyWeight)*fraction;
            double scaleShort=Math.max(0,partyWeight-factoryWeight)*fraction;
            double received=value(row,"NetWeight")-(supplierCut?supplierShort:0);
            row.put("SupplierShortWeight",supplierShort);row.put("ScaleShortage",scaleShort);
            row.put("SupplierRcvdWeight",received);row.put("BillWeight",received-(scaleCut?scaleShort:0));
        }
        return rows;
    }

    public static void validateEmptyBags(List<Map<String,Object>> bags,List<Map<String,Object>> details) {
        if(bags.isEmpty())return;
        double received=0,purchased=0,grnQty=0;
        for(var detail:details)if(number(detail.get("PackingTypeId"))!=5)grnQty+=value(detail,"ItemQty");
        for(var bag:bags) {
            int type=number(bag.get("TypeId"));
            if(number(bag.get("ItemId"))<=0||type<=0||number(bag.get("BagsCondition"))<=0)
                throw new IllegalArgumentException("Select the item, type and condition in every empty-bag row");
            double r=value(bag,"ReceivedQty"),p=value(bag,"PurchaseQty");
            if(r<0||p<0)throw new IllegalArgumentException("Empty-bag quantities cannot be less than zero");
            if((type==4||type==5)&&p!=0)throw new IllegalArgumentException("Retained or returned bags cannot be purchased");
            if(type==2&&r!=0)throw new IllegalArgumentException("Purchase Against Weight cannot have received quantity");
            received+=r;purchased+=p;
        }
        for(var bag:bags) {
            int type=number(bag.get("TypeId"));
            if(type==1&&received+purchased==0)throw new IllegalArgumentException("Received or purchase quantity is required for empty bags");
            if((type==2||type==3)&&purchased==0)throw new IllegalArgumentException("Purchase quantity is required for empty bags");
            if((type==4||type==5)&&received==0)throw new IllegalArgumentException("Received quantity is required for empty bags");
        }
        if(Math.abs(grnQty-received-purchased)>0.000001)
            throw new IllegalArgumentException("Empty bags quantity must equal GRN quantity without Open Bulk");
    }

    public static void applyPurchasedBagWeight(List<Map<String,Object>> details,List<Map<String,Object>> bags,int referenceType,boolean governmentOrder) {
        double remaining=bags.stream().filter(row->number(row.get("TypeId"))==2).mapToDouble(row->value(row,"PurchaseQty")).sum();
        for(var row:details) {
            // Clear any previously allocated weight too when a row changes type/quantity.
            double allocated=Math.min(remaining,value(row,"ItemQty"))*value(row,"EBWPerUnit");
            remaining=Math.max(0,remaining-value(row,"ItemQty"));
            row.put("EbPurAgainstWeight",allocated);
            double gross=value(row,"GrossWeight");
            row.put("NetBillWeight",referenceType==105?gross-Math.abs(value(row,"AdLsWeight")):
                    governmentOrder?gross:gross-value(row,"EBWTotal")-value(row,"WtCutTotal")-value(row,"AdLsWeight")+allocated);
        }
    }

    public static List<Map<String,Object>> copy(List<Map<String,Object>> input) {
        List<Map<String,Object>> rows=new ArrayList<>();
        if(input!=null)for(var row:input) { Map<String,Object> copied=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);copied.putAll(row);rows.add(copied); }
        return rows;
    }
    public static double value(Map<String,Object> row,String key) {
        Object raw=row.get(key);double result=raw==null||raw.toString().isBlank()?0:Double.parseDouble(raw.toString());
        if(!Double.isFinite(result))throw new IllegalArgumentException(key+" must be a finite number");
        return result;
    }
}
