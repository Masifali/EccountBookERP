package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemManufacturerRequest;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
@Repository
public class DesktopInventoryItemManufacturerRepository {
 private final JdbcTemplate jdbc;
 public DesktopInventoryItemManufacturerRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> items(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAllItems'",u.getOrganizationId(),u.getCompanyId());}
 public List<Map<String,Object>> manufacturers(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_InvLookup_GetAllMethod @OrganizationId=?, @CompanyId=?, @InvLookupTypeId=14, @Activity='ReadByInvlookTypeId'",u.getOrganizationId(),u.getCompanyId());}
 public Map<String,Object> lookups(UserAccount u){return Map.of("items",items(u),"manufacturers",manufacturers(u));}
 public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_ItemAllocateToManufactures_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId());}
 public Map<String,Object> record(UserAccount u,int id){return history(u).stream().filter(r->num(r.get("Id"))==id).findFirst().orElseThrow(()->new ResponseStatusException(HttpStatus.NOT_FOUND,"Manufacturer allocation not found in this company"));}
 public Map<String,Object> save(UserAccount u,InventoryItemManufacturerRequest r){if(r.id>0)record(u,r.id);if(items(u).stream().noneMatch(row->num(row.get("Id"))==r.itemId))throw new IllegalArgumentException("Select an Item from this company");if(manufacturers(u).stream().noneMatch(row->num(row.get("Id"))==r.manufacturerId))throw new IllegalArgumentException("Select an available Manufacturer");
  // Allocation has no tenant columns. Its history procedure scopes through the item's origin.
  if(jdbc.queryForObject("SELECT COUNT(*) FROM Item WHERE Id=? AND OrganizationId=? AND CompanyId=?",Integer.class,r.itemId,u.getOrganizationId(),u.getCompanyId())!=1)throw new IllegalArgumentException("Manufacturer allocation requires an item owned by this company");
  int id=jdbc.execute("EXEC dbo.usp_ItemAllocateToManufactures_Insert @Id=?, @ItemId=?, @ManufactureId=?, @IsActive=?",(PreparedStatementCallback<Integer>) statement->{statement.setInt(1,r.id);statement.setInt(2,r.itemId);statement.setInt(3,r.manufacturerId);statement.setBoolean(4,r.active);boolean result=statement.execute();int saved=r.id;while(true){if(result){try(var rows=statement.getResultSet()){while(rows.next())if(rows.getObject(1) instanceof Number n)saved=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return saved;});return record(u,id);
 }
 private static int num(Object value){return value==null?0:((Number)value).intValue();}
}
