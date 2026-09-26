package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLanguageRequest;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
@Repository
public class DesktopInventoryItemLanguageRepository {
 private final JdbcTemplate jdbc;
 public DesktopInventoryItemLanguageRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> items(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAllItemsIncludedPM'",u.getOrganizationId(),u.getCompanyId());}
 public List<Map<String,Object>> languages(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_MultiLanguages_GetAll @MethodType='ReadAll', @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());}
 public Map<String,Object> lookups(UserAccount u){return Map.of("items",items(u),"languages",languages(u));}
 public List<Map<String,Object>> history(UserAccount u){var codes=new HashMap<Integer,Object>();jdbc.queryForList("SELECT i.Id,i.ItemCodeNew FROM Item i WHERE i.OrganizationId=? AND EXISTS(SELECT 1 FROM ItemAllocation a WHERE a.ItemId=i.Id AND a.OrganizationId=? AND a.CompanyId=?)",u.getOrganizationId(),u.getOrganizationId(),u.getCompanyId()).forEach(r->codes.put(num(r.get("Id")),r.get("ItemCodeNew")));var rows=jdbc.queryForList("EXEC dbo.USP_ItemMultiLingo_ReadAll").stream().filter(r->codes.containsKey(num(r.get("ItemId")))).toList();rows.forEach(r->r.put("ItemCodeNew",codes.get(num(r.get("ItemId")))));return rows;}
 public Map<String,Object> record(UserAccount u,int id){return history(u).stream().filter(r->num(r.get("Id"))==id).findFirst().orElseThrow(()->new ResponseStatusException(HttpStatus.NOT_FOUND,"Item translation not found in this company"));}
 public Map<String,Object> save(UserAccount u,InventoryItemLanguageRequest r){if(r.id>0)record(u,r.id);if(items(u).stream().noneMatch(row->num(row.get("Id"))==r.itemId))throw new IllegalArgumentException("Select an Item from this company");if(languages(u).stream().noneMatch(row->num(row.get("Id"))==r.languageId))throw new IllegalArgumentException("Select an available Language");int id=jdbc.execute("EXEC dbo.Sp_ItemMultiLingo_Insert @Id=?, @ItemId=?, @MultiLanguagesId=?, @ItemName=?",(PreparedStatementCallback<Integer>) statement->{statement.setInt(1,r.id);statement.setInt(2,r.itemId);statement.setInt(3,r.languageId);statement.setNString(4,r.title);boolean result=statement.execute();int saved=0;while(true){if(result){try(var rows=statement.getResultSet()){while(rows.next())if(rows.getObject(1) instanceof Number n)saved=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return saved;});return record(u,id);}
 private static int num(Object value){return value==null?0:((Number)value).intValue();}
}
