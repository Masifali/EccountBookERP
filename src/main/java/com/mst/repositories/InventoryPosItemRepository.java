package com.mst.repositories;

import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

/** InvAddItemsPOS and Architecture.BLL.Inventory.Item read contracts. */
@Repository
public class InventoryPosItemRepository {
    private final JdbcTemplate jdbc;
    public InventoryPosItemRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public Map<String,Object> lookups(UserAccount u){
        Map<String,Object> result=new LinkedHashMap<>();
        result.put("companies",jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity=?",u.getOrganizationId(),"ReadByOrganizationId"));
        result.put("countries",jdbc.queryForList("EXEC dbo.SP_Country_ReadMethod @OrganizationId=?, @CompanyId=?, @MethodType=?",u.getOrganizationId(),u.getCompanyId(),"GetAll"));
        result.put("uomTypes",lookup(u,7));result.put("purchaseTypes",lookup(u,8));
        result.put("categories",scoped(u,"Sp_ItemCategory_GetAllMethod","ReadByOrganizationCompanyId"));
        result.put("types",scoped(u,"Sp_ItemType_GetAllMethod","ReadByOrganizationCompanyId"));
        result.put("classes",jdbc.queryForList("EXEC dbo.Sp_ItemClass_GetAllMethod @Activity=?","ReadAll"));
        result.put("units",scoped(u,"Sp_UOM_GetAllMethod","ReadByOrganizationCompanyId"));
        result.put("seasons",scoped(u,"Sp_InvCropYear_GetAllMethod","ReadAll"));
        result.put("suppliers",scoped(u,"Sp_SupplierCustomer_GetAllMethod","ReadByOrganizationIdCompanyIdForBinding"));
        result.put("accounts",jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),u.getId(),"COAForCombobindig"));
        result.put("masterItems",jdbc.execute("{call dbo.Sp_Item_GetAllMethod(?,?,?)}",(CallableStatementCallback<List<Map<String,Object>>>) s->{
            s.setInt(1,u.getOrganizationId());s.setInt(2,u.getCompanyId());s.setNString(3,"ReadAllForComboTwoColumns");
            try(var rs=s.executeQuery()){return new RowMapperResultSetExtractor<Map<String,Object>>(new ColumnMapRowMapper()).extractData(rs);}
        }));
        return result;
    }
    private List<Map<String,Object>> lookup(UserAccount u,int type){return jdbc.queryForList("EXEC dbo.Sp_InvLookup_GetAllMethod @OrganizationId=?, @CompanyId=?, @InvLookupTypeId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),type,"ReadByInvlookTypeId");}
    private List<Map<String,Object>> scoped(UserAccount u,String procedure,String activity){return jdbc.queryForList("EXEC dbo."+procedure+" @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),activity);}
    public List<Map<String,Object>> history(UserAccount u,boolean allRecords,boolean loadAll,Integer category){
        String sql="EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?, @CanViewAllRecord=?, @IsTaxable=?";
        List<Object> values=new ArrayList<>(List.of(u.getOrganizationId(),u.getCompanyId(),"FormHistory",allRecords,false));
        // Use the authenticated user for restricted history; never trust a browser UserId.
        if(!allRecords){sql+=", @EntryUser=?";values.add(u.getId());}
        if(!loadAll){sql+=", @NoOfRecords=?";values.add(50);}
        // The C# form accidentally assigns ReportsParameters.Id, which BLL ignores.
        // Honor the visible category control using the BLL's actual parameter.
        if(category!=null&&category>0){sql+=", @ItemCategoryId=?";values.add(category);}
        return jdbc.queryForList(sql,values.toArray());
    }
    public Map<String,Object> record(UserAccount u,int id){
        var rows=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",id,"ReadById");
        if(rows.size()!=1||number(rows.get(0),"OrganizationId")!=u.getOrganizationId()||number(rows.get(0),"CompanyId")!=u.getCompanyId())throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Item not found in this company");
        return rows.get(0);
    }
    public Map<String,Object> details(UserAccount u,int id){
        var result=new LinkedHashMap<String,Object>();result.put("item",record(u,id));
        result.put("allocations",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",id,"ReadItemAllocationByItemId"));
        result.put("images",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",id,"ItemImagesByItemId"));
        result.put("attachments",jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?","InvAddItemsPOS",id,"ReadById").stream().filter(r->number(r,"OrganizationId")==u.getOrganizationId()&&number(r,"CompanyId")==u.getCompanyId()).toList());return result;
    }
    public Map<String,Object> categoryDefaults(UserAccount u,int category){
        var allowed=scoped(u,"Sp_ItemCategory_GetAllMethod","ReadByOrganizationCompanyId");
        if(allowed.stream().noneMatch(r->number(r,"Id")==category))throw new IllegalArgumentException("Please select a category available to this company");
        var code=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @ItemTypeId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),category,0,"GenerateItemCodeByCategoryId");
        var accounts=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),category,"GetGLAccountbyItemCategoryId");
        return Map.of("code",code,"accounts",accounts);
    }
    static int number(Map<String,Object> row,String key){return row.get(key) instanceof Number n?n.intValue():0;}
}
