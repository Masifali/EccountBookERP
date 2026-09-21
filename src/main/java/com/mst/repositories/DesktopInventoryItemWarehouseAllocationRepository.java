package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemWarehouseAllocationRequest;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
@Repository
public class DesktopInventoryItemWarehouseAllocationRepository {
 private final JdbcTemplate jdbc;
 public DesktopInventoryItemWarehouseAllocationRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public Map<String,Object> lookups(UserAccount u){return Map.of("categories",jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadByOrganizationCompanyId'",u.getOrganizationId(),u.getCompanyId()),"types",jdbc.queryForList("EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadByOrganizationCompanyId'",u.getOrganizationId(),u.getCompanyId()));}
 public List<Map<String,Object>> warehouses(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetActiveWareHouse'",u.getOrganizationId(),u.getCompanyId());}
 public List<Map<String,Object>> items(UserAccount u,int category,int type){
  // Match the desktop RPC call. Typed nulls avoid an unintended prepared EXEC plan.
  return jdbc.execute("{call dbo.Sp_Item_GetAllMethod(?,?,?,?,?,?,?,?,?)}",(CallableStatementCallback<List<Map<String,Object>>>) statement->{
   for(int i=1;i<=9;i++)statement.setNull(i,i==3?java.sql.Types.NVARCHAR:i==7?java.sql.Types.BIT:java.sql.Types.INTEGER);
   statement.setInt(1,u.getOrganizationId());statement.setInt(2,u.getCompanyId());statement.setNString(3,"GetItemsForAllocatetoWarehouse");if(category>0)statement.setInt(4,category);if(type>0)statement.setInt(9,type);
   boolean result=statement.execute();while(!result&&statement.getUpdateCount()!=-1)result=statement.getMoreResults();if(!result)return List.of();try(var rs=statement.getResultSet()){return new ColumnMapRowMapperExtractor().extract(rs);}
  });
 }
 public Map<String,Object> rows(UserAccount u,int category,int type){var rows=items(u,category,type);var codes=new HashMap<Integer,Object>();jdbc.queryForList("SELECT Id,ItemCodeNew FROM Item WHERE OrganizationId=?",u.getOrganizationId()).forEach(r->codes.put(((Number)r.get("Id")).intValue(),r.get("ItemCodeNew")));for(var r:rows)r.put("ItemCodeNew",codes.get(((Number)r.get("Id")).intValue()));return Map.of("items",rows,"warehouses",warehouses(u));}
 public int save(UserAccount u,InventoryItemWarehouseAllocationRequest r){var itemIds=ids(items(u,0,0));var warehouseIds=ids(warehouses(u));if(r.itemIds.stream().anyMatch(id->id==null||!itemIds.contains(id)))throw new IllegalArgumentException("Select items from this company's available grid");if(r.warehouseIds.stream().anyMatch(id->id==null||!warehouseIds.contains(id)))throw new IllegalArgumentException("Select active warehouses from this company");int inserted=0;for(int item:new LinkedHashSet<>(r.itemIds))for(int warehouse:new LinkedHashSet<>(r.warehouseIds)){var now=new Timestamp(System.currentTimeMillis());Object[] values={0,item,warehouse,now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),0,0,1};inserted+=jdbc.execute("EXEC dbo.Sp_ItemAllocateToWareHouse_Insert @Id=?, @ItemId=?, @WarehouseId=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BrancheId=?, @ProjectsId=?, @ActionId=?",(PreparedStatementCallback<Integer>) statement->{for(int i=0;i<values.length;i++)statement.setObject(i+1,values[i]);boolean result=statement.execute();int count=0;while(true){if(result){try(var rs=statement.getResultSet()){while(rs.next())if(rs.getObject(1) instanceof Number n&&n.intValue()>0)count++;}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return count;});}return inserted;}
 private static Set<Integer> ids(List<Map<String,Object>> rows){var result=new HashSet<Integer>();rows.forEach(r->result.add(((Number)r.get("Id")).intValue()));return result;}
 private static class ColumnMapRowMapperExtractor {List<Map<String,Object>> extract(java.sql.ResultSet rs)throws java.sql.SQLException{var result=new ArrayList<Map<String,Object>>();var mapper=new ColumnMapRowMapper();while(rs.next())result.add(mapper.mapRow(rs,result.size()));return result;}}
}
