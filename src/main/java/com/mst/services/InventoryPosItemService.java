package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.repositories.*;
import com.mst.security.*;
import java.math.BigDecimal;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
import org.springframework.security.access.AccessDeniedException;

@Service
public class InventoryPosItemService {
    private final InventoryPosItemRepository repo;private final InventoryPosItemWriter writer;
    private final InventoryOpeningRepository shared;private final CurrentUserContext context;private final DesktopReportRights rights;
    private final InventoryPosFileService files;
    public InventoryPosItemService(InventoryPosItemRepository repo,InventoryPosItemWriter writer,InventoryOpeningRepository shared,CurrentUserContext context,DesktopReportRights rights,InventoryPosFileService files){this.repo=repo;this.writer=writer;this.shared=shared;this.context=context;this.rights=rights;this.files=files;}
    private UserAccount user(String action){var u=context.requireAccountingUser();rights.require(u,106,action);return u;}
    private boolean allowed(UserAccount u,String action){try{rights.require(u,106,action);return true;}catch(AccessDeniedException ex){return false;}}
    public Map<String,Object> lookups(){var u=user("View");var data=repo.lookups(u);var permissions=new LinkedHashMap<String,Boolean>();for(String action:List.of("Save","Update","Print","CanView AllRecord"))permissions.put(action,allowed(u,action));data.put("permissions",permissions);data.put("fileTransfers",true);return data;}
    public List<Map<String,Object>> history(boolean all,Integer category){var u=user("View");return repo.history(u,allowed(u,"CanView AllRecord"),all,category);}
    public Map<String,Object> record(int id){return repo.details(user("View"),id);}
    public Map<String,Object> category(int id){return repo.categoryDefaults(user("View"),id);}
    public InventoryPosFileService.Download attachment(int item,int attachment){return files.download(user("View"),item,attachment);}
    public InventoryPosFileService.Download product(int item){return files.product(user("View"),item);}
    public Map<String,Object> barcode(String code){user("View");text(code,50,"Barcode");if(code.chars().anyMatch(c->c<32||c>126))throw new IllegalArgumentException("Code 128 requires printable ASCII characters");byte[] bars=com.itextpdf.text.pdf.Barcode128.getBarsCode128Raw(com.itextpdf.text.pdf.Barcode128.getRawText(code,false));List<Integer> widths=new ArrayList<>();for(byte b:bars)widths.add((int)b);return Map.of("code",code,"bars",widths);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryPosItemRequest r){
        var u=user(r.Id==0?"Save":"Update");validate(r);
        var old=r.Id>0?repo.record(u,r.Id):null;var choices=repo.lookups(u);
        selected(choices,"categories","Id",r.ItemCategoryId);selected(choices,"types","Id",r.ItemTypeId);selected(choices,"classes","ClassId",r.ItemClassId);
        selected(choices,"units","Id",r.BaseUnitId);selected(choices,"uomTypes","Id",r.UomLookUpId);selected(choices,"purchaseTypes","Id",r.PurchaseTypeLookUpId);selected(choices,"countries","Id",r.ItemOriginId);
        for(int id:List.of(r.PurchaseGLAC,r.SaleGLAC,r.COGSGLAC))selected(choices,"accounts","Id",id);
        for(Integer company:r.companies){if(company==null)throw new IllegalArgumentException("Select a valid company");selected(choices,"companies","Id",company);}
        for(int id:List.of(r.ManufactureId,r.BuyerSupplierId))if(id>0)selected(choices,"suppliers","Id",id);
        if(r.MasterItemId>0)selected(choices,"masterItems","Id",r.MasterItemId);
        if(r.BaseCropYear!=null&&!r.BaseCropYear.isBlank()&&rows(choices,"seasons").stream().noneMatch(x->r.BaseCropYear.equals(Objects.toString(x.get("CropYear"),""))))throw new IllegalArgumentException("Select a season from the company list");
        var years=shared.years(u);if(years.isEmpty())throw new IllegalArgumentException("No active financial year allocated to this company");
        String picture=files.prepareProduct(u,r);
        String barcodePicture=files.prepareBarcode(u,r);
        int id=writer.save(u,((Number)years.get(0).get("Id")).intValue(),r,old,picture,barcodePicture);files.persist(u,id,r.ItemTypeId,r);return repo.details(u,id);
    }
    @SuppressWarnings("unchecked") private static List<Map<String,Object>> rows(Map<String,Object> choices,String key){return (List<Map<String,Object>>)choices.get(key);}
    private static void selected(Map<String,Object> choices,String key,String idKey,int id){if(rows(choices,key).stream().noneMatch(x->x.get(idKey) instanceof Number n&&n.intValue()==id))throw new IllegalArgumentException("Selected "+key+" record is not available to this company/user");}
    public static void validate(InventoryPosItemRequest r){
        if(r.Id<0)throw new IllegalArgumentException("Invalid item ID");
        if(r.companies==null||r.companies.isEmpty())throw new IllegalArgumentException("Please Check At least One Company For Allocate Item");
        required(r.ItemCategoryId,"Item Category");required(r.ItemTypeId,"Item Type");required(r.ItemClassId,"Item Class");
        text(r.ItemCode,50,"Item Code");if("0".equals(r.ItemCode))throw new IllegalArgumentException("Item Code Field is Required");text(r.ItemName,100,"Item Name");
        required(r.BaseUnitId,"Unit");required(r.UomLookUpId,"Unit Type");nonzero(r.PackQty,"Pack Qty");required(r.ItemOriginId,"Item Reigion");required(r.PurchaseTypeLookUpId,"Purchase Type");required(r.PurchaseGLAC,"Purchase GL A/C");required(r.SaleGLAC,"Sale GL A/C");required(r.COGSGLAC,"COGS GL A/C");
        nonzero(r.PurchasePrice,"Purchase Rate");nonzero(r.CostPrice,"Cost Rate");nonzero(r.RetailPrice,"Retail Rate");nonzero(r.WholeSalePrice,"Whole Sale Rate");nonzero(r.ReorderLevel,"ReOrder Level");nonzero(r.OptimalStockLevel,"Economical Stock Level");nonzero(r.MinStockLevel,"Min Stock Level");nonzero(r.MaxStockLevel,"Max Stock Level");
        length(r.ItemAliasName,50,"Alias");length(r.BarcodeNo,50,"Barcode");length(r.HSCode,50,"HS Code");length(r.ManufacturePartNo,50,"Manufacturer Code");length(r.ItemSpecification,500,"Specification");
        if(r.SaleTaxPurPercent==null||r.SaleTaxSalesPercent==null||!Double.isFinite(r.SaleTaxPurPercent.doubleValue())||!Double.isFinite(r.SaleTaxSalesPercent.doubleValue()))throw new IllegalArgumentException("Enter valid tax percentages");
    }
    private static void required(int id,String name){if(id<=0)throw new IllegalArgumentException(name+" Field is Required");}
    private static void text(String value,int max,String name){if(value==null||value.isBlank())throw new IllegalArgumentException(name+" Field is Required");length(value,max,name);}
    private static void length(String value,int max,String name){if(value!=null&&value.length()>max)throw new IllegalArgumentException(name+" cannot exceed "+max+" characters");}
    private static void nonzero(BigDecimal value,String name){if(value==null||value.signum()==0||!Double.isFinite(value.doubleValue()))throw new IllegalArgumentException(name+" must be nonzero");}
}
