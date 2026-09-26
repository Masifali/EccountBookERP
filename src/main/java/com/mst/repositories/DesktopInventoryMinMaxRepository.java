package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryMinMaxRequest;
import java.sql.Timestamp;
import java.math.BigDecimal;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

@Repository
public class DesktopInventoryMinMaxRepository {
    private final JdbcTemplate jdbc;
    public DesktopInventoryMinMaxRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public Map<String,Object> lookups(UserAccount u){
        return Map.of("parents",jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity=?","InventoryParentCategories").stream().filter(r->Set.of(1,2,3,4,7,8).contains(number(r.get("Id")))).toList(),
            "categories",jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId"),
            "types",jdbc.queryForList("EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId"),
            "historyChoices",jdbc.queryForList("EXEC dbo.Usp_DropDownFillFromItemMinAndMaxRateSchedule @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()));
    }
    public List<Map<String,Object>> items(UserAccount u,int parent){
        if(parent==0)return List.of();
        if(!Set.of(1,2,3,4,7,8).contains(parent))throw new IllegalArgumentException("Select an available parent category");
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @InventoryParentCategoriesId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),parent,"ReadAllItemsByParentId");
    }
    public void requireItem(UserAccount u,int id){
        if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.Item WHERE Id=? AND OrganizationId=? AND CompanyId=?",Integer.class,id,u.getOrganizationId(),u.getCompanyId())!=1)throw new IllegalArgumentException("Item does not belong to this company");
    }
    public List<Map<String,Object>> uoms(UserAccount u,int item){requireItem(u,item);return new InventoryOpeningRepository(jdbc).uoms(u,item);}
    public List<Map<String,Object>> last(UserAccount u,int item,int uom){
        requireItem(u,item);if(uom>0 && uoms(u,item).stream().noneMatch(r->number(r.get("Id"))==uom))throw new IllegalArgumentException("Select a UOM defined for this item");
        return jdbc.queryForList("EXEC dbo.usp_GetLastItemMinAndMaxRateByItemIdAndUomId @OrganizationId=?, @CompanyId=?, @ItemId=?, @RateUomId=?",u.getOrganizationId(),u.getCompanyId(),item,uom==0?null:uom);
    }
    public List<Map<String,Object>> query(UserAccount u,InventoryMinMaxRequest.Filter f,boolean history){
        StringBuilder sql=new StringBuilder("EXEC dbo."+(history?"USP_GetItemsFromMinAndMaxRateSchedule_FormHistory":"usp_GetItemsFromMinAndMaxRateSchedule")+" @OrganizationId=?, @CompanyId=?");
        var args=new ArrayList<Object>(List.of(u.getOrganizationId(),u.getCompanyId()));
        add(sql,args,"InventoryParentCategoriesId",optional(f.parent));add(sql,args,"ItemCategoryId",optional(f.category));add(sql,args,"ItemTypeId",optional(f.type));
        if(history)add(sql,args,"ItemId",optional(f.item));
        add(sql,args,"EffectedDate",f.date==null?null:java.sql.Date.valueOf(f.date));
        return jdbc.queryForList(sql.toString(),args.toArray());
    }
    public Map<String,Object> record(UserAccount u,int id){
        var rows=jdbc.queryForList("SELECT * FROM dbo.ItemMinMaxRateSchedule WHERE Id=? AND OrganizationId=? AND CompanyId=?",id,u.getOrganizationId(),u.getCompanyId());
        if(rows.size()!=1)throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Rate schedule not found in this company");return rows.get(0);
    }
    public List<Integer> save(UserAccount u,int year,InventoryMinMaxRequest r){
        var ids=new ArrayList<Integer>();var now=new Timestamp(System.currentTimeMillis());
        for(var row:r.rows){
            if(row.minRate==null||row.maxRate==null||row.minRate.signum()<=0||row.maxRate.signum()<=0)continue;
            var uoms=uoms(u,row.itemId);
            // Desktop resolves the numeric rate equivalent against the item's UOM schedule before writing.
            int equivalent=row.rateUom.intValue();
            int unit=uoms.stream().filter(v->new BigDecimal(v.get("Equivalent").toString()).compareTo(BigDecimal.valueOf(equivalent))==0).mapToInt(v->number(v.get("Id"))).findFirst().orElseThrow(()->new IllegalArgumentException(row.rateUom+" RateUom not defined against item "+row.itemId));
            Object[] args={0,row.itemId,Timestamp.valueOf(r.effectedDate),row.minRate.doubleValue(),row.maxRate.doubleValue(),u.getId(),now,u.getId(),now,u.getOrganizationId(),u.getCompanyId(),u.getBranchesId(),year,unit};
            int id=jdbc.execute("EXEC dbo.USP_ItemMinMaxRateSchedule_Insert @Id=?, @ItemId=?, @EffectedDate=?, @MinRate=?, @MaxRate=?, @EntryUser=?, @EntryDate=?, @ModifyUser=?, @ModifyDate=?, @OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, @RateUomId=?",(PreparedStatementCallback<Integer>) s->{for(int i=0;i<args.length;i++)s.setObject(i+1,args[i]);int saved=0;boolean result=s.execute();while(true){if(result){try(var rs=s.getResultSet()){while(rs.next())if(rs.getObject(1) instanceof Number n)saved=n.intValue();}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}return saved;});
            if(id<=0)throw new IllegalStateException("Rate save returned no ID");ids.add(id);
        }
        return ids;
    }
    public void delete(UserAccount u,int id){record(u,id);ProcExec.call(jdbc, "EXEC dbo.usp_MinMaxDeletionById @Id=?",id);}
    public int year(UserAccount u){var rows=new InventoryOpeningRepository(jdbc).years(u);if(rows.isEmpty())throw new IllegalArgumentException("No active financial year");return number(rows.get(0).get("Id"));}
    private static Object optional(int id){return id==0?null:id;}
    private static void add(StringBuilder sql,List<Object> args,String name,Object value){if(value!=null){sql.append(", @").append(name).append("=?");args.add(value);}}
    private static int number(Object value){return value==null?0:Integer.parseInt(value.toString());}
}
