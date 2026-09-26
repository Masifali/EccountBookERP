package com.mst.services;

import java.util.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static com.mst.services.PurchaseGrnGridRules.*;

class PurchaseGrnGridRulesTest {
    @Test void breakupApportionsBothShortagesAndUsesAbsoluteBagWeight() {
        var original=List.<Map<String,Object>>of(Map.of("Id",31,"Qty",10,"UOM",50,"EbWeight",0.2),Map.of("Id",32,"Qty",5,"UOM",60,"EbWeight",-0.4));
        var rows=calculateBreakups(original,780,770,true,true);
        assertEquals(31,rows.get(0).get("Id"));assertFalse(original.get(0).containsKey("GrossWeight"));
        assertEquals(800,rows.stream().mapToDouble(r->value(r,"GrossWeight")).sum());
        assertEquals(4,rows.stream().mapToDouble(r->value(r,"EBTotal")).sum());
        assertEquals(20,rows.stream().mapToDouble(r->value(r,"SupplierShortWeight")).sum(),0.000001);
        assertEquals(10,rows.stream().mapToDouble(r->value(r,"ScaleShortage")).sum(),0.000001);
        assertEquals(766,rows.stream().mapToDouble(r->value(r,"BillWeight")).sum(),0.000001);
        assertEquals(59.6,value(rows.get(1),"NetPackSize"),0.000001);
        var uncut=calculateBreakups(original,780,770,false,false);
        assertEquals(796,uncut.stream().mapToDouble(r->value(r,"BillWeight")).sum(),0.000001);
    }
    @Test void excessAndZeroQuantityDoNotCreateShortagesOrDivideByZero() {
        var rows=calculateBreakups(List.of(Map.of("Qty",0,"UOM",50,"EbWeight",1)),200,220,true,true);
        assertEquals(0,value(rows.get(0),"BillWeight"));assertEquals(0,value(rows.get(0),"ScaleShortage"));
        assertThrows(IllegalArgumentException.class,()->calculateBreakups(List.of(Map.of("Qty",Double.NaN)),0,0,false,false));
    }
    @Test void bagTypesAndCombinedQuantityFollowDesktopRules() {
        var details=List.<Map<String,Object>>of(Map.of("PackingTypeId",2,"ItemQty",12),Map.of("PackingTypeId",5,"ItemQty",100));
        var bags=List.<Map<String,Object>>of(Map.of("TypeId",1,"ItemId",627,"BagsCondition",1,"ReceivedQty",7,"PurchaseQty",5));
        assertDoesNotThrow(()->validateEmptyBags(bags,details));
        for(var invalid:List.of(Map.of("TypeId",2,"ItemId",627,"BagsCondition",1,"ReceivedQty",1,"PurchaseQty",11),Map.of("TypeId",4,"ItemId",627,"BagsCondition",1,"PurchaseQty",12),Map.of("TypeId",1,"ItemId",627,"BagsCondition",1,"ReceivedQty",11)))
            assertThrows(IllegalArgumentException.class,()->validateEmptyBags(List.of(new HashMap<>(invalid)),details));
    }
    @Test void purchasedBagWeightAllocatesInRowOrderAndClearsRemovedAllocation() {
        var details=copy(List.of(Map.of("ItemQty",3,"GrossWeight",150,"EBWPerUnit",0.5,"EBWTotal",1.5),Map.of("ItemQty",5,"GrossWeight",250,"EBWPerUnit",1,"EBWTotal",5)));
        applyPurchasedBagWeight(details,List.of(Map.of("TypeId",2,"PurchaseQty",4)),41,false);
        assertEquals(150,value(details.get(0),"NetBillWeight"));assertEquals(246,value(details.get(1),"NetBillWeight"));
        applyPurchasedBagWeight(details,List.of(),41,false);
        assertEquals(0,value(details.get(0),"EbPurAgainstWeight"));assertEquals(148.5,value(details.get(0),"NetBillWeight"));
        applyPurchasedBagWeight(details,List.of(),41,true);
        assertEquals(150,value(details.get(0),"NetBillWeight"));
    }
}
