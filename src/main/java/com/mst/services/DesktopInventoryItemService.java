package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryGeneralItemRequest;
import com.mst.repositories.*;
import com.mst.security.*;
import java.math.BigDecimal;
import java.util.*;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

/** Screen 111: the non-POS desktop item form. */
@Service
public class DesktopInventoryItemService {
    private final DesktopInventoryItemRepository repo;
    private final DesktopInventoryItemWriter writer;
    private final DesktopInventoryItemFileService files;
    private final InventoryOpeningRepository shared;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public DesktopInventoryItemService(DesktopInventoryItemRepository repo,DesktopInventoryItemWriter writer,DesktopInventoryItemFileService files,InventoryOpeningRepository shared,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.writer=writer;this.files=files;this.shared=shared;this.context=context;this.rights=rights;}
    private UserAccount user(String action){var u=context.requireAccountingUser();rights.require(u,111,action);return u;}
    private boolean allowed(UserAccount u,String action){try{rights.require(u,111,action);return true;}catch(AccessDeniedException ex){return false;}}
    public Map<String,Object> lookups(){var u=user("View");var result=repo.lookups(u);var permissions=new LinkedHashMap<String,Boolean>();for(String action:List.of("Save","Update","Print","CanView AllRecord"))permissions.put(action,allowed(u,action));result.put("permissions",permissions);return result;}
    public List<Map<String,Object>> history(int count,int category,int type,int parent){if(count<0||category<0||type<0||parent<0)throw new IllegalArgumentException("Invalid history filter");return repo.history(user("View"),allowed(context.requireAccountingUser(),"CanView AllRecord"),count,category,type,parent);}
    public Map<String,Object> record(int id){return repo.details(user("View"),id);}
    public String editRoute(int id){
        var u=context.requireAccountingUser();
        boolean general=repo.usesGeneralForm(u,id);
        rights.require(u,general?111:106,"View");
        return (general?"/inventory/items":"/inventory/pos-define-item")+"?id="+id;
    }
    public Map<String,Object> byCode(String code){length(code,50,"Item Code");return repo.byCode(user("View"),Objects.toString(code,"").trim());}
    public Map<String,Object> defaults(int category,int type){var u=user("View");if(type>0)selected(repo.lookups(u),"types","Id",type);return repo.defaults(u,category,type);}
    public DesktopInventoryItemFileService.Download attachment(int item,int attachment){return files.download(user("View"),item,attachment);}
    public DesktopInventoryItemFileService.Download image(int item,int image){return files.image(user("View"),item,image);}

    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryGeneralItemRequest r){
        if(r==null||r.Id<0)throw new IllegalArgumentException("Invalid item");
        var u=user(r.Id==0?"Save":"Update");var old=r.Id>0?repo.record(u,r.Id):null;var choices=repo.lookups(u);
        text(r.ItemName,100,"Item Name");text(r.ItemCode,50,"Item Code");text(r.ItemCodeNew,Integer.MAX_VALUE,"Item Code");
        
        length(r.HSCode,50,"HS Code");length(r.ManufacturePartNo,50,"Manufacturer Part No");length(r.BuyerPartNo,50,"Buyer Part No");length(r.ProductNo,50,"Product No");length(r.ModelName,300,"Model Name");
        selected(choices,"categories","Id",r.ItemCategoryId);selected(choices,"types","Id",r.ItemTypeId);
        var itemClass=selected(choices,"classes","ClassId",r.ItemClassId);var unit=selected(choices,"units","Id",r.BaseUnitId);
        selected(choices,"mothers","Id",r.MotherItemId);
        if(old!=null&&number(old.get("ItemCategoryId"))!=r.ItemCategoryId)throw new IllegalArgumentException("Item Category cannot change after the item is saved");
        if("Brand".equals(itemClass.get("ClassDescription"))&&(r.BaseCropYear==null||r.BaseCropYear.isBlank()))throw new IllegalArgumentException("Crop Year Field is Required");
        if(r.BaseCropYear!=null&&!r.BaseCropYear.isBlank()&&rows(choices,"seasons").stream().noneMatch(row->r.BaseCropYear.equals(Objects.toString(row.get("CropYear"),""))))throw new IllegalArgumentException("Select a Crop Year from this company");
        if(Boolean.TRUE.equals(choices.get("partyProcessing"))){if(!r.IsCompany&&!r.IsThirdParty)throw new IllegalArgumentException("Please check Is Company or Is Third Party");}else{r.IsCompany=true;r.IsThirdParty=false;}
        boolean auto=r.Id==0&&repo.configuration(u,"AutoCoaDefineByItemNameOnInsert");
        Map<String,Object> purchase=null,sale=null;
        if(!auto||r.PurchaseGLAC>0)purchase=selected(choices,"purchaseAccounts","Id",r.PurchaseGLAC);
        if(!auto||r.SaleGLAC>0)sale=selected(choices,"saleAccounts","Id",r.SaleGLAC);
        selected(choices,"cgsAccounts","Id",r.COGSGLAC);
        if(purchase!=null&&sale!=null&&number(purchase.get("AccountTypeId"))==4&&number(sale.get("AccountTypeId"))==4&&r.PurchaseGLAC!=r.SaleGLAC)throw new IllegalArgumentException("Sale Account Should be Same as Purchase Account or Select an Account Of Type Sale In Sale Account");
        for(int id:List.of(r.ManufactureId,r.BuyerSupplierId))if(id>0)selected(choices,"suppliers","Id",id);
        if(r.ItemOriginId>0)selected(choices,"countries","Id",r.ItemOriginId);
        if(r.Id==0&&r.ItemGroupId>0)selected(choices,"groups","GroupId",r.ItemGroupId);
        if(r.Id==0&&r.ApplyGST&&r.TaxTypeId>0)selected(choices,"taxes","Id",r.TaxTypeId);
        if(r.Id==0){
            if(r.companies==null)throw new IllegalArgumentException("Select companies for item allocation");
            var companies=rows(choices,"companies");if(companies.size()==1)r.companies=new ArrayList<>(List.of(number(companies.get(0).get("Id"))));
            for(Integer id:r.companies){if(id==null)throw new IllegalArgumentException("Invalid company allocation");selected(choices,"companies","Id",id);}
        }
        integer(r.PackQty,"Pack Quantity");
        if(r.Id>0){integer(r.MaxStockLevel,"Maximum Stock Level");integer(r.ReorderLevel,"Reorder Level");integer(r.CostPrice,"Cost Rate");integer(r.PurchasePrice,"Purchase Rate");integer(r.WholeSalePrice,"Sale Rate");}
        else if(r.WholeSalePrice==null||!Double.isFinite(r.WholeSalePrice.doubleValue()))throw new IllegalArgumentException("Enter a valid Purchase Rate");
        var years=shared.years(u);if(years.isEmpty())throw new IllegalArgumentException("No active financial year allocated to this company");
        var images=files.prepareImages(u,r);var equivalent=new BigDecimal(unit.get("Equivalent").toString());
        int id=writer.save(u,years.get(0),r,old,images,equivalent);files.persist(u,id,r.ItemTypeId,r);return repo.details(u,id);
    }
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> updateNames(List<InventoryGeneralItemRequest.Name> changes){
        var u=user("Update");if(changes==null||changes.isEmpty())throw new IllegalArgumentException("Select at least one item");
        var ids=new HashSet<Integer>();for(var change:changes){if(change==null||change.id<=0||!ids.add(change.id))throw new IllegalArgumentException("Select each item once");text(change.name,100,"Item Name");repo.record(u,change.id);}
        for(var change:changes)repo.updateName(u,change.id,change.name.trim(),Objects.toString(change.other,"").trim());return Map.of("updated",changes.size());
    }
    @SuppressWarnings("unchecked") private static List<Map<String,Object>> rows(Map<String,Object> choices,String key){return (List<Map<String,Object>>)choices.get(key);}
    private static Map<String,Object> selected(Map<String,Object> choices,String key,String column,int id){return rows(choices,key).stream().filter(row->number(row.get(column))==id&&id>0).findFirst().orElseThrow(()->new IllegalArgumentException("Select a valid "+key+" record from this company"));}
    private static int number(Object value){return value==null?0:((Number)value).intValue();}
    private static void text(String value,int max,String name){if(value==null||value.isBlank())throw new IllegalArgumentException(name+" Field is Required");length(value,max,name);}
    private static void length(String value,int max,String name){if(value!=null&&value.length()>max)throw new IllegalArgumentException(name+" cannot exceed "+max+" characters");}
    private static void integer(BigDecimal value,String name){try{if(value==null)throw new ArithmeticException();value.intValueExact();}catch(ArithmeticException ex){throw new IllegalArgumentException(name+" requires a whole number");}}
}


