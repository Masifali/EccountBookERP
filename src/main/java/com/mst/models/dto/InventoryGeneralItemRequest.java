package com.mst.models.dto;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.*;
/** InvDefrmAddItem assignments, generated from recovered save/update methods. */
public class InventoryGeneralItemRequest {
 public int Id;
 public String ItemCode="";
 public String ItemCodeNew="";
 public String ItemName="";
 public String ItemNameOtherLingo="";
 public int BaseUnitId;
 public int ItemCategoryId;
 public int ItemTypeId;
 public int ItemClassId;
 public String HSCode="";
 public BigDecimal PackQty=BigDecimal.ZERO;
 public boolean ItemStatus;
 public boolean IsTaxable;
 public int PurchaseGLAC;
 public int SaleGLAC;
 public int COGSGLAC;
 public int ManufactureId;
 public String ManufacturePartNo="";
 public int BuyerSupplierId;
 public String BuyerPartNo="";
 public String ProductNo="";
 public String ModelName="";
 public int ItemGroupId;
 public BigDecimal WholeSalePrice=BigDecimal.ZERO;
 public int MotherItemId;
 public String BaseCropYear="";
 public boolean ApplyGST;
 public int TaxTypeId;
 public boolean ItemWithVarient;
 public boolean IsThirdParty;
 public boolean IsCompany;
 public boolean CommOnSale;
 public boolean CommOnPurchase;
 public BigDecimal MaxStockLevel=BigDecimal.ZERO;
 public BigDecimal ReorderLevel=BigDecimal.ZERO;
 public int ItemOriginId;
 public BigDecimal CostPrice=BigDecimal.ZERO;
 public int UOMScheduleIdCostRate;
 public BigDecimal PurchasePrice=BigDecimal.ZERO;
 public int UOMScheduleIdPurRate;
 public int UOMScheduleIdWhsRate;
 public String AmountCalcType="";
 public boolean ApplyVAT;
 public boolean ApplyExciese;
 public boolean OtherTax;
 public List<Integer> companies=new ArrayList<>(); public List<InventoryPosItemRequest.Upload> files=new ArrayList<>(); public List<Integer> removeAttachmentIds=new ArrayList<>(); public List<Image> images; public LocalDateTime imageDate; public static class Image {public int id,sortNo; public InventoryPosItemRequest.Upload upload;}
 private static final Set<String> INSERT=Set.of("ItemCode","ItemCodeNew","ItemName","ItemNameOtherLingo","BaseUnitId","ItemCategoryId","ItemTypeId","ItemClassId","HSCode","PackQty","ItemStatus","IsTaxable","PurchaseGLAC","SaleGLAC","COGSGLAC","ManufactureId","ManufacturePartNo","BuyerSupplierId","BuyerPartNo","ProductNo","ModelName","ItemGroupId","WholeSalePrice","MotherItemId","BaseCropYear","ApplyGST","TaxTypeId","ItemWithVarient","IsThirdParty","IsCompany","CommOnSale","CommOnPurchase");
 private static final Set<String> UPDATE=Set.of("ItemCode","ItemCodeNew","ItemName","ItemNameOtherLingo","BaseUnitId","ItemCategoryId","ItemTypeId","ItemClassId","HSCode","PackQty","ItemStatus","IsTaxable","PurchaseGLAC","SaleGLAC","COGSGLAC","ManufactureId","ManufacturePartNo","BuyerSupplierId","BuyerPartNo","ProductNo","ModelName","MaxStockLevel","ReorderLevel","ItemOriginId","CostPrice","UOMScheduleIdCostRate","PurchasePrice","UOMScheduleIdPurRate","WholeSalePrice","UOMScheduleIdWhsRate","BaseCropYear","AmountCalcType","ApplyGST","ApplyVAT","ApplyExciese","OtherTax","MotherItemId","ItemWithVarient","IsThirdParty","IsCompany");
 public Map<String,Object> values(boolean insert){var values=new LinkedHashMap<String,Object>();for(String key:insert?INSERT:UPDATE){try{values.put(key,InventoryGeneralItemRequest.class.getField(key).get(this));}catch(ReflectiveOperationException ex){throw new IllegalStateException(ex);}}values.put("Id",Id);return values;}
 public static class Name {public int id;public String name,other;}
}
