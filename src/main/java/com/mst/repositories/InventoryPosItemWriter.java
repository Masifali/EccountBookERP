package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import java.sql.*;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

/** POS subset of Item.SetData. Caller must own a SERIALIZABLE transaction. */
@Repository
public class InventoryPosItemWriter {
    private final JdbcTemplate jdbc;
    private final Map<String,List<String>> parameters=new ConcurrentHashMap<>();
    public InventoryPosItemWriter(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public int save(UserAccount u,int year,InventoryPosItemRequest request,Map<String,Object> old){
        return save(u,year,request,old,null);
    }
    public int save(UserAccount u,int year,InventoryPosItemRequest request,Map<String,Object> old,String productImage){
        return save(u,year,request,old,productImage,null);
    }
    public int save(UserAccount u,int year,InventoryPosItemRequest request,Map<String,Object> old,String productImage,String barcodeImage){
        boolean insert=request.Id==0;var p=InventoryPosDefaults.item();
        if(old!=null)p.putAll(old);p.putAll(request.values());
        if(productImage!=null)p.put("Pic1",productImage);
        if(barcodeImage!=null)p.put("BarcodeImage",barcodeImage);
        p.put("OrganizationId",u.getOrganizationId());p.put("CompanyId",u.getCompanyId());
        p.put("BuyerPartNo",request.ManufacturePartNo); // One manufacturer-code control in POS.
        p.put("ItemName",Objects.toString(request.ItemName,"").trim());p.put("ItemCode",Objects.toString(request.ItemCode,"").trim());
        if(insert){p.put("EntryUser",u.getId());p.put("EntryDate",now());}
        else{p.put("ModifyUser",u.getId());p.put("ModifyDate",now());}
        // POS never assigns Item.BranchesId; its CLR default is 0. Allocation uses user branch.
        if(insert&&parent(u,request.ItemCategoryId)!=9&&configuration(u,"AutoCoaDefineByItemNameOnInsert")){
            var stock=accountParent(request.ItemCategoryId,"Stock");
            var sale=configuration(u,"SameAccountForStockAndRevenue")?stock:accountParent(request.ItemCategoryId,"Sale");
            int stockId=account(u,year,request,stock,"Stock A/c");
            int saleId=Objects.equals(stock.get("ParentCodeId"),sale.get("ParentCodeId"))?stockId:account(u,year,request,sale,"Sale A/c");
            p.put("PurchaseGLAC",stockId);p.put("SaleGLAC",saleId);
        }
        var images=insert?List.<Map<String,Object>>of():jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",request.Id,"ItemImagesByItemId");
        int id=execute("dbo.Sp_Item_"+(insert?"Insert":"Update"),p,request.Id);
        if(id<=0)throw new IllegalStateException("Item save returned no record ID");
        if(!insert){
            // Sp_Item_Update deletes ALL images. POS does not own that gallery. Restore
            // the exact rows/IDs inside the transaction; the image upsert cannot reinsert IDs.
            for(var image:images)jdbc.update("INSERT INTO dbo.ItemImage (Id,ItemId,ImagePath,EffetedDate,IsActive,FileName,IsProfileImage,SortNo) VALUES (?,?,?,?,?,?,?,?)",image.get("Id"),id,image.get("ImagePath"),image.get("EffetedDate"),image.get("IsActive"),image.get("FileName"),image.get("IsProfileImage"),image.get("SortNo"));
        }else{
            Map<String,Object> unit=new LinkedHashMap<>();
            unit.putAll(Map.of("Id",0,"ItemId",id,"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"EntryUser",u.getId(),"ModifyUser",0,"EntryDate",now(),"ModifyDate",now()));
            unit.putAll(Map.of("ScheduleUnitId",request.BaseUnitId,"Equivalent",0d,"QtyEquivalent",1d,"BaseRateUom",false,"BasePackUom",false,"BaseSecondaryUom",false,"Active",true));
            execute("dbo.Sp_UOMSchedule_Insert",unit,0);
        }
        for(int company:new LinkedHashSet<>(request.companies)){
            execute("dbo.Sp_ItemAllocation_Insert",Map.of("Id",0,"ItemId",id,"OrganizationId",u.getOrganizationId(),"CompanyId",company,"BranchId",u.getBranchesId(),"IsActive",request.ItemStatus),0);
            if(!insert)continue; // Desktop update does not create new schedules.
            String[] priceFields={"CostPrice","PurchasePrice","WholeSalePrice","RetailPrice"};
            for(int n=0;n<priceFields.length;n++){
                var price=(Number)p.get(priceFields[n]);if(price.doubleValue()<=0)continue;
                var values=InventoryPosDefaults.price();scope(values,u,company,id);
                values.put("ItemPrice",price);values.put("PriceTypeId",n+4);execute("dbo.Sp_ItemPricingSchedule_Insert",values,0);
            }
            if(request.ReorderLevel.signum()>0){
                var values=InventoryPosDefaults.reorder();scope(values,u,company,id);
                values.putAll(Map.of("MinQty",request.MinStockLevel,"MaxQty",request.MaxStockLevel,"OptimalQty",request.OptimalStockLevel,"ReOrderPoint",request.ReorderLevel));
                execute("dbo.Sp_ItemsReorderSchedule_Insert",values,0);
            }
            var values=InventoryPosDefaults.feedPrice();values.putAll(Map.of("ItemId",id,"OrganizationId",u.getOrganizationId(),"CompanyId",company,"EntryUserId",u.getId(),"ItemRate",1d,"PricingCustomGroupId",1,"IsActive",true,"DocumentTypeId",1,"EffectiveDate",new Timestamp(System.currentTimeMillis()-7L*24*60*60*1000)));
            execute("fed.usp_ItemPricingSchedule_Insert",values,0);
        }
        return id;
    }
    private void scope(Map<String,Object> p,UserAccount u,int company,int item){p.putAll(Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",company,"ItemId",item,"EntryUser",u.getId()));}
    private int parent(UserAccount u,int category){var rows=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),category,"GetParentCategoryIdByItemCategoryId");return rows.isEmpty()?0:InventoryPosItemRepository.number(rows.get(0),"InventoryParentCategoriesId");}
    private boolean configuration(UserAccount u,String name){var rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),name,"GetConfigurationByOrgCompandConfigDescription");return !rows.isEmpty()&&Set.of("true","1").contains(Objects.toString(rows.get(0).get("ConfigKey"),"").toLowerCase(Locale.ROOT));}
    private Map<String,Object> accountParent(int category,String activity){var rows=jdbc.queryForList("EXEC dbo.usp_getAccountsInfoByItemCategoryId @ItemCategoryId=?, @Activity=?",category,activity);if(rows.isEmpty())throw new IllegalArgumentException(activity+" account parent not found");return rows.get(0);}
    private int account(UserAccount u,int year,InventoryPosItemRequest request,Map<String,Object> parent,String suffix){
        var p=InventoryPosDefaults.account();String title=request.ItemName.trim()+" "+suffix;
        for(String key:List.of("AccountClass","ParentCodeId","AccountTypeId","PLNoteId","BSNoteId"))p.put(key,parent.get(key));
        String code=Objects.toString(parent.get("ParentAccountCode"),"");p.put("ParentAccountCode",code.isEmpty()?"0":code);p.put("AccountCode",code);
        p.putAll(Map.of("Account_Level",4,"AccountTitle",title,"AccountGroup","Detail","OtherErpCode",request.ItemCode,"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"EntryUser",u.getId(),"IsActive",true,"FinancialYearId",year,"ActionId",1));
        int id=execute("dbo.Proc_ChartofAccount_Insert",p,0);if(id<=0)throw new IllegalStateException("Account creation returned no record ID");
        for(int company:new LinkedHashSet<>(request.companies)){
            var allocation=InventoryPosDefaults.accountAllocation();allocation.putAll(Map.of("CompanyId",company,"IsActive",true,"ChartofAccountId",id));execute("dbo.Sp_COAAllocation_Insert",allocation,0);
            var opening=InventoryPosDefaults.accountOpening();opening.putAll(Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",company,"EntryUser",u.getId(),"ChartOfAccountId",id,"ChartOfAccountTitle",title,"FinancialYearId",year));execute("dbo.Sp_AccountsOpeningBalances_Insert",opening,0);
        }
        return id;
    }
    private static Timestamp now(){return new Timestamp(System.currentTimeMillis());}
    private int execute(String procedure,Map<String,Object> values,int fallback){
        var names=parameters.computeIfAbsent(procedure,p->jdbc.queryForList("SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",String.class,p));
        if(names.isEmpty())throw new IllegalStateException("Procedure contract unavailable: "+procedure);
        var caseInsensitive=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER);caseInsensitive.putAll(values);
        var selected=names.stream().filter(caseInsensitive::containsKey).toList();
        return jdbc.execute("EXEC "+procedure+" "+String.join(", ",selected.stream().map(k->"@"+k+"=?").toList()),(PreparedStatementCallback<Integer>) s->{
            int n=1;for(String name:selected)s.setObject(n++,caseInsensitive.get(name));boolean result=s.execute();Integer first=null;
            while(true){if(result){try(var rs=s.getResultSet()){if(first==null&&rs.next()&&rs.getObject(1) instanceof Number number)first=number.intValue();}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}
            return first!=null&&first>0?first:fallback;
        });
    }
}
