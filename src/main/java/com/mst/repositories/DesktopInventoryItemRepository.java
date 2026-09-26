package com.mst.repositories;

import com.mst.repositories.support.ProcExec;

import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** InvDefrmAddItem's own lookup/history contract; distinct from InvAddItemsPOS. */
@Repository
public class DesktopInventoryItemRepository {
    public static final String SCREEN="InvDefrmAddItem", PARENTS="1,2,3,4,6,10,11";
    private final JdbcTemplate jdbc;
    private final InventoryPosItemRepository items;
    public DesktopInventoryItemRepository(JdbcTemplate jdbc,InventoryPosItemRepository items){this.jdbc=jdbc;this.items=items;}
    public Map<String,Object> lookups(UserAccount u){
        var result=new LinkedHashMap<String,Object>();
        result.put("categories",jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Ids=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),PARENTS,"ReadByOrganizationCompanyId"));
        result.put("types",jdbc.queryForList("EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @ParentCategoryIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),PARENTS,"ReadByOrganizationCompanyId"));
        result.put("parents",jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity=?","InventoryParentCategories").stream().filter(r->Set.of(1,2,3,4,6,10,11).contains(number(r.get("Id")))).toList());
        result.put("mothers",jdbc.queryForList("EXEC dbo.usp_getMotherItems @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()));
        result.put("classes",jdbc.queryForList("EXEC dbo.Sp_ItemClass_GetAllMethod @Activity=?","ReadAll").stream().filter(r->Set.of(1,6,7,8).contains(number(r.get("ClassId")))).toList());
        result.put("units",scoped(u,"Sp_UOM_GetAllMethod","ReadByOrganizationCompanyId"));
        result.put("groups",scoped(u,"Sp_ItemGroup_GetAllMethod","ReadAll"));
        result.put("taxes",scoped(u,"Sp_TaxesTypes_GetAllMethod","ReadByOrganizationCompanyId").stream().filter(r->number(r.get("Type"))!=1).toList());
        result.put("suppliers",scoped(u,"Sp_SupplierCustomer_GetAllMethod","ReadByOrganizationCompanyId"));
        result.put("countries",jdbc.queryForList("EXEC dbo.SP_Country_ReadMethod @OrganizationId=?, @CompanyId=?, @MethodType=?",u.getOrganizationId(),u.getCompanyId(),"GetAll"));
        result.put("seasons",scoped(u,"Sp_InvCropYear_GetAllMethod","ReadAll"));
        result.put("companies",jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity=?",u.getOrganizationId(),"ReadByOrganizationId"));
        var configs=new LinkedHashMap<String,Boolean>();
        for(String key:List.of("AutoCoaDefineByItemNameOnInsert","SameAccountForStockAndRevenue","ItemCodingEnable","All Account Allow on Item and Item Category"))configs.put(key,configuration(u,key));
        result.put("configuration",configs);result.put("partyProcessing",new DesktopInventoryCategoryRepository(jdbc).feature(u,8));
        var accounts=scoped(u,"Sp_COAAllocation_GetAllMethod","COAAllocationSearch");
        boolean same=configs.get("AutoCoaDefineByItemNameOnInsert")&&configs.get("SameAccountForStockAndRevenue"),all=configs.get("All Account Allow on Item and Item Category");
        result.put("purchaseAccounts",accounts.stream().filter(r->number(r.get("AccountTypeId"))==4 || all&&!same&&number(r.get("AccountTypeId"))==10).toList());
        result.put("saleAccounts",accounts.stream().filter(r->same?number(r.get("AccountTypeId"))==4:(number(r.get("AccountTypeId"))==10 || all&&number(r.get("AccountTypeId"))==4)).toList());
        result.put("cgsAccounts",accounts.stream().filter(r->number(r.get("AccountTypeId"))==12).toList());
        return result;
    }
    public List<Map<String,Object>> history(UserAccount u,boolean allRecords,int count,int category,int type,int parent){
        // RPC mirrors SqlCommand(CommandType.StoredProcedure) used by the desktop.
        // The first 25 parameters are from the installed procedure signature;
        // omitted optional filters have its original NULL defaults.
        return jdbc.execute("{call dbo.Sp_Item_GetAllMethod(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}",(org.springframework.jdbc.core.CallableStatementCallback<List<Map<String,Object>>>) statement->{
            for(int index=1;index<=25;index++)statement.setNull(index,switch(index){case 3,10,11,12,24,25->java.sql.Types.NVARCHAR;case 7->java.sql.Types.BIT;case 20->java.sql.Types.DATE;default->java.sql.Types.INTEGER;});
            statement.setInt(1,u.getOrganizationId());statement.setInt(2,u.getCompanyId());statement.setNString(3,"FormHistory");
            if(category>0)statement.setInt(4,category);if(count>0)statement.setInt(6,count);
            statement.setBoolean(7,allRecords);if(!allRecords)statement.setInt(8,u.getId());
            if(type>0)statement.setInt(9,type);statement.setNString(10,SCREEN);
            if(parent>0)statement.setInt(19,parent);else statement.setNString(25,PARENTS);
            statement.setInt(23,0);
            try(var result=statement.executeQuery()){return new org.springframework.jdbc.core.RowMapperResultSetExtractor<Map<String,Object>>(new org.springframework.jdbc.core.ColumnMapRowMapper()).extractData(result);}
        });
    }
    public Map<String,Object> record(UserAccount u,int id){return items.record(u,id);}
    public boolean usesGeneralForm(UserAccount u,int id){
        var item=record(u,id);
        Integer parent=jdbc.queryForObject("SELECT InventoryParentCategoriesId FROM ItemCategory WHERE Id=?",Integer.class,item.get("ItemCategoryId"));
        return parent!=null&&Set.of(1,2,3,4,6,10,11).contains(parent)&&Set.of(1,6,7,8).contains(number(item.get("ItemClassId")));
    }
    public Map<String,Object> details(UserAccount u,int id){
        var r=new LinkedHashMap<String,Object>();r.put("item",record(u,id));
        r.put("images",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",id,"ItemImagesByItemId"));
        r.put("attachments",attachments(u,id));
        r.put("allocations",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",id,"ReadItemAllocationByItemId"));
        r.put("rateUnits",scoped(u,"Sp_UOMSchedule_GetAllMethod","ReadByOrganizationCompanyId").stream().filter(row->number(row.get("ItemId"))==id).toList());
        return r;
    }
    public List<Map<String,Object>> attachments(UserAccount u,int id){
        record(u,id);
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?",SCREEN,id,"ReadById").stream().filter(r->number(r.get("OrganizationId"))==u.getOrganizationId()&&number(r.get("CompanyId"))==u.getCompanyId()).toList();
    }
    public Map<String,Object> byCode(UserAccount u,String code){
        var rows=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Code=?, @Activity=?",code,"ReadByItemCode").stream().filter(r->number(r.get("OrganizationId"))==u.getOrganizationId()&&number(r.get("CompanyId"))==u.getCompanyId()).toList();
        return rows.isEmpty()?Map.of():details(u,number(rows.get(0).get("Id")));
    }
    public Map<String,Object> defaults(UserAccount u,int category,int type){
        var rows=jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Ids=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),PARENTS,"ReadByOrganizationCompanyId");
        if(rows.stream().noneMatch(r->number(r.get("Id"))==category))throw new IllegalArgumentException("Select an Item Category from this company");
        return Map.of("code",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @ItemTypeId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),category,type,"GenerateItemCodeByCategoryId"),
            "accounts",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),category,"GetGLAccountbyItemCategoryId"));
    }
    public void updateName(UserAccount u,int id,String name,String other){
        var old=record(u,id);
        ProcExec.call(jdbc, "EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?, @ItemName=?, @ItemNameOtherLingo=?, @Code=?",id,"Update",name,other,old.get("ItemCode"));
    }
    public boolean configuration(UserAccount u,String key){var rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),key,"GetConfigurationByOrgCompandConfigDescription");return !rows.isEmpty()&&Set.of("true","1").contains(Objects.toString(rows.get(0).get("ConfigKey"),"").toLowerCase(Locale.ROOT));}
    private List<Map<String,Object>> scoped(UserAccount u,String procedure,String activity){return jdbc.queryForList("EXEC dbo."+procedure+" @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),activity);}
    static int number(Object value){return value==null?0:Integer.parseInt(value.toString());}
}

