package com.mst.models.dto;
import java.math.BigDecimal;
public class InventoryUnitRequest {
    public int id,parentUomId;
    public String code="";
    public BigDecimal equivalent=BigDecimal.ZERO,qtyEquivalent=BigDecimal.ZERO;
    public boolean active=true;
}
