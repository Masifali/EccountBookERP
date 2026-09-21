package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryGeneralItemRequest;
import java.sql.*;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

/** InvDefrmAddItem implementation of Item.SetData. Caller must own a SERIALIZABLE transaction. */
@Repository
public class DesktopInventoryItemWriter {
    private final JdbcTemplate jdbc;
    private final Map<String,List<String>> parameters=new ConcurrentHashMap<>();
    public DesktopInventoryItemWriter(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public int save(UserAccount u,Map<String,Object> year,InventoryGeneralItemRequest request,Map<String,Object> old,List<Map<String,Object>> images,java.math.BigDecimal equivalent){
        boolean insert=request.Id==0;var p=InventoryPosDefaults.item();
        p.putAll(request.values(insert));
        p.replaceAll((key,value)->value instanceof String text?text.trim():value);
        p.put("OrganizationId",u.getOrganizationId());p.put("CompanyId",u.getCompanyId());p.put("BranchesId",u.getBranchesId());
        p.put("IsTaxable",false);p.put("Equivalent",equivalent);p.put("ItemName",request.ItemName.trim());p.put("ItemCode",request.ItemCode.trim());p.put("ItemCodeNew",request.ItemCodeNew.trim());
        p.put("EntryDate",now());p.put("ModifyDate",now());p.put("PostDate",now());p.put("PostState",false);
        p.put("EntryUser",insert?u.getId():0);p.put("ModifyUser",insert?0:u.getId());
        if(!insert){p.put("CommOnPurchase",false);p.put("CommOnSale",false);}
        for(int slot=1;slot<=2;slot++){final int selected=slot;var picture=images.stream().filter(r->((Number)r.get("SortNo")).intValue()==selected).findFirst();p.put("Pic"+slot,picture.map(r->r.get("FileName")).orElse(""));}
        if(insert&&parent(u,request.ItemCategoryId)!=9&&configuration(u,"AutoCoaDefineByItemNameOnInsert")){
            var stock=accountParent(request.ItemCategoryId,"Stock");var sale=configuration(u,"SameAccountForStockAndRevenue")?stock:accountParent(request.ItemCategoryId,"Sale");
            int financialYear=((Number)year.get("Id")).intValue();int stockId=account(u,financialYear,request,stock,"Stock A/c");
            int saleId=Objects.equals(stock.get("ParentCodeId"),sale.get("ParentCodeId"))?stockId:account(u,financialYear,request,sale,"Sale A/c");p.put("PurchaseGLAC",stockId);p.put("SaleGLAC",saleId);
        }
        int id=execute("dbo.Sp_Item_"+(insert?"Insert":"Update"),p,request.Id);
        if(id<=0)throw new IllegalStateException("Item save returned no record ID");
        // The desktop update procedure returns zero: DAL skips insert-only child schedules.
        if(insert){
            if(request.ApplyGST&&request.TaxTypeId>0){var tax=new LinkedHashMap<String,Object>();tax.putAll(Map.of("Id",0,"ItemId",id,"IsActive",true,"EffectedDate",now(),"EntryDate",now(),"ModifyDate",now(),"EntryUser",u.getId(),"ModifyUser",0,"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId()));tax.put("TaxTypeId",request.TaxTypeId);tax.put("TaxName",null);execute("dbo.Sp_ItemTaxSchedule_Insert",tax,0);}
            if(request.ItemGroupId>0){
                for(var row:jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @ItemGroupId=?, @Activity=?",request.ItemGroupId,"ReadByItemGroupId"))unit(u,id,request.ItemClassId,((Number)row.get("UOMId")).intValue(),row.get("Equivalent"),row.get("QtyEquivalent"));
            }else unit(u,id,request.ItemClassId,request.BaseUnitId,equivalent,1d);
            for(int company:new LinkedHashSet<>(request.companies)){
                execute("dbo.Sp_ItemAllocation_Insert",Map.of("Id",0,"ItemId",id,"OrganizationId",u.getOrganizationId(),"CompanyId",company,"BranchId",u.getBranchesId(),"IsActive",true),0);
                String[] fields={"CostPrice","PurchasePrice","WholeSalePrice","RetailPrice"};
                for(int n=0;n<fields.length;n++){var price=(Number)p.get(fields[n]);if(price.doubleValue()<=0)continue;var values=InventoryPosDefaults.price();scope(values,u,company,id);values.putAll(Map.of("BranchesId",u.getBranchesId(),"ItemPrice",price,"PriceTypeId",n+4));execute("dbo.Sp_ItemPricingSchedule_Insert",values,0);}
                if(((Number)p.get("ReorderLevel")).doubleValue()>0){var values=InventoryPosDefaults.reorder();scope(values,u,company,id);values.putAll(Map.of("BranchesId",u.getBranchesId(),"MinQty",p.get("MinStockLevel"),"MaxQty",p.get("MaxStockLevel"),"OptimalQty",p.get("OptimalStockLevel"),"ReOrderPoint",p.get("ReorderLevel")));execute("dbo.Sp_ItemsReorderSchedule_Insert",values,0);}
                if(request.CommOnSale||request.CommOnPurchase){var commission=new LinkedHashMap<String,Object>();commission.putAll(Map.of("Id",0,"CompanyId",company,"OrgnizationId",u.getOrganizationId(),"BranchesId",u.getBranchesId(),"EntryUserId",u.getId(),"ModifyUserId",0,"EntryDate",now(),"ModifyDate",now(),"ItemId",id));commission.putAll(Map.of("OnSale",request.CommOnSale,"OnPurchase",request.CommOnPurchase,"EffectedFromDate",year.get("Start_Period")));execute("dbo.USP_ItemWiseCommission_InsertOrUpdate",commission,0);}
                var values=InventoryPosDefaults.feedPrice();values.putAll(Map.of("ItemId",id,"OrganizationId",u.getOrganizationId(),"CompanyId",company,"EntryUserId",u.getId(),"ItemRate",1d,"PricingCustomGroupId",1,"IsActive",true,"DocumentTypeId",1,"EffectiveDate",new Timestamp(System.currentTimeMillis()-7L*24*60*60*1000)));execute("fed.usp_ItemPricingSchedule_Insert",values,0);
            }
        }
        for(var image:images){var values=new LinkedHashMap<>(image);values.put("Id",0);values.put("ItemId",id);execute("dbo.USP_ItemImage_Insert",values,0);}
        return id;
    }
    private void unit(UserAccount u,int id,int itemClass,int unitId,Object equivalent,Object quantity){
        var row=new LinkedHashMap<String,Object>();row.putAll(Map.of("Id",0,"ItemId",id,"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"EntryUser",u.getId(),"ModifyUser",0,"EntryDate",now(),"ModifyDate",now()));
        double value=legacyNumber(equivalent);row.putAll(Map.of("ScheduleUnitId",unitId,"Equivalent",value,"QtyEquivalent",legacyNumber(quantity),"BaseRateUom",itemClass==8&&value==1000d||value==40d,"BasePackUom",false,"BaseSecondaryUom",false,"Active",true));execute("dbo.Sp_UOMSchedule_Insert",row,0);
    }
    private static double legacyNumber(Object value){try{return value==null?0d:Double.parseDouble(value.toString());}catch(NumberFormatException ex){return 0d;}}
    private void scope(Map<String,Object> p,UserAccount u,int company,int item){p.putAll(Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",company,"ItemId",item,"EntryUser",u.getId()));}
    private int parent(UserAccount u,int category){var rows=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),category,"GetParentCategoryIdByItemCategoryId");return rows.isEmpty()?0:InventoryPosItemRepository.number(rows.get(0),"InventoryParentCategoriesId");}
    private boolean configuration(UserAccount u,String name){var rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),name,"GetConfigurationByOrgCompandConfigDescription");return !rows.isEmpty()&&Set.of("true","1").contains(Objects.toString(rows.get(0).get("ConfigKey"),"").toLowerCase(Locale.ROOT));}
    private Map<String,Object> accountParent(int category,String activity){var rows=jdbc.queryForList("EXEC dbo.usp_getAccountsInfoByItemCategoryId @ItemCategoryId=?, @Activity=?",category,activity);if(rows.isEmpty())throw new IllegalArgumentException(activity+" account parent not found");return rows.get(0);}
    private int account(UserAccount u,int year,InventoryGeneralItemRequest request,Map<String,Object> parent,String suffix){
        var p=InventoryPosDefaults.account();String title=request.ItemName.trim()+" "+suffix;
        for(String key:List.of("AccountClass","ParentCodeId","AccountTypeId","PLNoteId","BSNoteId"))p.put(key,parent.get(key));
        String code=Objects.toString(parent.get("ParentAccountCode"),"");p.put("ParentAccountCode",code.isEmpty()?"0":code);p.put("AccountCode",code);
        p.putAll(Map.of("Account_Level",4,"AccountTitle",title,"AccountGroup","Detail","OtherErpCode",request.ItemCode,"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"EntryUser",u.getId(),"IsActive",true,"FinancialYearId",year,"ActionId",1));
        p.put("BranchId",u.getBranchesId());
        int id=execute("dbo.Proc_ChartofAccount_Insert",p,0);if(id<=0)throw new IllegalStateException("Account creation returned no record ID");
        for(int company:new LinkedHashSet<>(request.companies)){
            var allocation=InventoryPosDefaults.accountAllocation();allocation.putAll(Map.of("CompanyId",company,"IsActive",true,"ChartofAccountId",id,"BranchId",u.getBranchesId()));execute("dbo.Sp_COAAllocation_Insert",allocation,0);
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
