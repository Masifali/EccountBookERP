package com.mst.models.dto;
import java.math.BigDecimal;
import java.util.*;
/** Field names match the desktop Item model and API record projection. */
public class InventoryPosItemRequest {
 public int Id;
 public List<Integer> companies=new ArrayList<>();
 public List<Upload> files=new ArrayList<>();
 public List<Integer> removeAttachmentIds=new ArrayList<>();
 public Upload productImage;
 public Upload barcodeImage;
 public static class Upload {public String name;public String base64;}
 public int ItemCategoryId;
 public int ItemTypeId;
 public int ItemClassId;
 public String ItemCode="";
 public String BarcodeNo="";
 public String ItemName="";
 public String ItemAliasName="";
 public int BaseUnitId;
 public int UomLookUpId;
 public int PurchaseTypeLookUpId;
 public BigDecimal PackQty=BigDecimal.ZERO;
 public int MasterItemId;
 public int ItemOriginId;
 public String BaseCropYear="";
 public String HSCode="";
 public int ManufactureId;
 public String ManufacturePartNo="";
 public String BuyerPartNo="";
 public int BuyerSupplierId;
 public int ShelfLife;
 public String ItemSpecification="";
 public boolean IsDiscountable;
 public boolean IsExpirable;
 public boolean ItemStatus;
 public int PurchaseGLAC;
 public int SaleGLAC;
 public int COGSGLAC;
 public BigDecimal PurchasePrice=BigDecimal.ZERO;
 public BigDecimal WholeSalePrice=BigDecimal.ZERO;
 public BigDecimal CostPrice=BigDecimal.ZERO;
 public BigDecimal RetailPrice=BigDecimal.ZERO;
 public BigDecimal ReorderLevel=BigDecimal.ZERO;
 public BigDecimal MaxStockLevel=BigDecimal.ZERO;
 public BigDecimal MinStockLevel=BigDecimal.ZERO;
 public BigDecimal OptimalStockLevel=BigDecimal.ZERO;
 public boolean IsTaxable;
 public BigDecimal SaleTaxPurPercent=BigDecimal.ZERO;
 public boolean AtRetailPriceTax;
 public boolean AtPurcahsePriceTax;
 public BigDecimal SaleTaxSalesPercent=BigDecimal.ZERO;
 public Map<String,Object> values(){Map<String,Object> p=new LinkedHashMap<>();for(var field:InventoryPosItemRequest.class.getFields()){if(List.class.isAssignableFrom(field.getType())||field.getType()==Upload.class)continue;try{p.put(field.getName(),field.get(this));}catch(IllegalAccessException ex){throw new IllegalStateException(ex);}}return p;}
}
