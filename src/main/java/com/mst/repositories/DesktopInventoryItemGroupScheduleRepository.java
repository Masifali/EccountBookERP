package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemGroupScheduleRequest;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
@Repository
public class DesktopInventoryItemGroupScheduleRepository {
    private final JdbcTemplate jdbc;private final DesktopInventoryItemLookupRepository groups;
    public DesktopInventoryItemGroupScheduleRepository(JdbcTemplate jdbc,DesktopInventoryItemLookupRepository groups){this.jdbc=jdbc;this.groups=groups;}
    public Map<String,Object> lookups(UserAccount u){return Map.of("groups",groups.history(u,DesktopInventoryItemLookupRepository.Kind.UOM_GROUP),"units",jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadByOrganizationCompanyId'",u.getOrganizationId(),u.getCompanyId()));}
    public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_ItemGroup_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId());}
    public Map<String,Object> record(UserAccount u,int id){var rows=jdbc.queryForList("EXEC dbo.Sp_ItemGroup_UOMSchedule_GetAllMethod @Id=?, @Activity='ReadById'",id);if(rows.size()!=1||number(rows.get(0).get("OrganizationId"))!=u.getOrganizationId()||number(rows.get(0).get("CompanyId"))!=u.getCompanyId())throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Group schedule not found in this company");return rows.get(0);}
    public Map<String,Object> save(UserAccount u,InventoryItemGroupScheduleRequest r){
        if(r.id>0)record(u,r.id);groups.record(u,DesktopInventoryItemLookupRepository.Kind.UOM_GROUP,r.groupId);
        if(jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadByOrganizationCompanyId'",u.getOrganizationId(),u.getCompanyId()).stream().noneMatch(row->number(row.get("Id"))==r.unitId))throw new IllegalArgumentException("Select a unit from this company");
        int id=execute("EXEC dbo.Sp_ItemGroup_UOMSchedule_"+(r.id==0?"Insert":"Update")+" @Id=?, @ItemGroupId=?, @UOMId=?, @Equivalent=?, @OrganizationId=?, @CompanyId=?",new Object[]{r.id,r.groupId,r.unitId,r.equivalent,u.getOrganizationId(),u.getCompanyId()},r.id);return record(u,id);
    }
    public List<Map<String,Object>> allItems(UserAccount u){return jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());}
    public Map<String,Object> assignmentLookups(UserAccount u){return Map.of("groups",groups.history(u,DesktopInventoryItemLookupRepository.Kind.UOM_GROUP),"items",allItems(u));}
    public List<Map<String,Object>> assignmentRows(UserAccount u,int parent,int category,int type,int item){return allItems(u).stream().filter(row->(parent<=0||number(row.get("InventoryParentCategoriesId"))==parent)&&(category<=0||number(row.get("ItemCategoryId"))==category)&&(type<=0||number(row.get("ItemTypeId"))==type)&&(item<=0||number(row.get("Id"))==item)).toList();}
    public int assign(UserAccount u,InventoryItemGroupScheduleRequest.Assignment r){
        groups.record(u,DesktopInventoryItemLookupRepository.Kind.UOM_GROUP,r.groupId);var allowed=new HashSet<Integer>();allItems(u).forEach(row->allowed.add(number(row.get("Id"))));
        for(Integer item:r.itemIds)if(item==null||!allowed.contains(item))throw new IllegalArgumentException("Select items from this company's grid");
        int count=0;for(int item:new LinkedHashSet<>(r.itemIds)){
            var schedules=jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @ItemGroupId=?, @Activity='GetUomForAssigntoItem'",u.getOrganizationId(),u.getCompanyId(),item,r.groupId);
            for(var row:schedules){double equivalent=decimal(row.get("Equivalent")),quantity=decimal(row.get("QtyEquivalent"));var now=new Timestamp(System.currentTimeMillis());
                execute("EXEC dbo.Sp_UOMSchedule_Insert @CompanyId=?, @EntryDate=?, @EntryUser=?, @Equivalent=?, @Id=?, @ItemId=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @ScheduleUnitId=?, @QtyEquivalent=?, @BaseRateUom=?, @BasePackUom=?, @BaseSecondaryUom=?, @Active=?",new Object[]{u.getCompanyId(),now,u.getId(),equivalent,0,item,now,0,u.getOrganizationId(),number(row.get("UOMId")),quantity,equivalent==1000d||equivalent==40d,false,false,true},0);count++;
            }
        }return count;
    }
    private static int number(Object value){return value==null?0:((Number)value).intValue();}
    private static double decimal(Object value){return value==null?0d:Double.parseDouble(value.toString());}
    private int execute(String sql,Object[] values,int fallback){return jdbc.execute(sql,(PreparedStatementCallback<Integer>) statement->{for(int i=0;i<values.length;i++)statement.setObject(i+1,values[i]);boolean result=statement.execute();int saved=fallback;while(true){if(result){try(var rows=statement.getResultSet()){while(rows.next())if(rows.getObject(1) instanceof Number n&&n.intValue()>0)saved=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return saved;});}
}
