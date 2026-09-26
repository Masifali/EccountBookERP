package com.mst.models.dto;

import java.util.*;

/** Keeps the original header and all three desktop child collections together. */
public final class PurchaseGrnDto {
    public final Map<String,Object> header;
    public final List<Map<String,Object>> details;
    public final List<Map<String,Object>> emptyBags;
    public final List<Map<String,Object>> purchaseBreakups;

    public PurchaseGrnDto(Map<String,Object> payload) {
        header=new TreeMap<>(String.CASE_INSENSITIVE_ORDER); header.putAll(payload);
        details=rows(payload,"details"); emptyBags=rows(payload,"emptyBags"); purchaseBreakups=rows(payload,"purchaseBreakups");
        header.remove("details"); header.remove("emptyBags"); header.remove("purchaseBreakups");
    }
    private static List<Map<String,Object>> rows(Map<String,Object> payload,String name) {
        Object value=payload.get(name); if(value==null)return null;
        if(!(value instanceof List))throw new IllegalArgumentException(name+" must be a list");
        List<Map<String,Object>> result=new ArrayList<>();
        for(Object row:(List<?>)value) {
            if(!(row instanceof Map))throw new IllegalArgumentException(name+" contains an invalid row");
            Map<String,Object> copy=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
            ((Map<?,?>)row).forEach((k,v)->copy.put(Objects.toString(k),v)); result.add(copy);
        }
        return result;
    }
}
