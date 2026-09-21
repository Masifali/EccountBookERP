package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemCompanyAllocationRequest;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
@Repository
public class DesktopInventoryItemCompanyAllocationRepository {
 private final JdbcTemplate jdbc;
 public DesktopInventoryItemCompanyAllocationRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> companies(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity='ReadByOrganizationId'",u.getOrganizationId());}
 public boolean companyLocked(UserAccount u,List<Map<String,Object>> companies){return companies.stream().anyMatch(r->num(r.get("Id"))==u.getCompanyId()&&"Branch".equals(r.get("CompType")));}
 public Map<String,Object> lookups(UserAccount u){var companies=companies(u);return Map.of("companies",companies,"locked",companyLocked(u,companies),"currentCompany",u.getCompanyId());}
 public void requireCompany(UserAccount u,int company){var rows=companies(u);if(rows.stream().noneMatch(r->num(r.get("Id"))==company)||companyLocked(u,rows)&&company!=u.getCompanyId())throw new IllegalArgumentException("Select an available company in this organization");}
 public List<Map<String,Object>> allocations(UserAccount u,int company,boolean allocated){return jdbc.queryForList("EXEC dbo.Sp_ItemAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ActionId=?, @Activity='ItemsAllocatedOrUnAllocatedToCompany'",u.getOrganizationId(),company,allocated?2:1);}
 public List<Map<String,Object>> statusRows(UserAccount u,int company,boolean active){
  // Installed Sp_Item_GetAllMethod lacks the desktop BLL's @Status/activity entirely.
  return jdbc.queryForList("SELECT i.Id,i.ItemName,i.ItemCodeNew FROM Item i WHERE i.OrganizationId=? AND EXISTS(SELECT 1 FROM ItemAllocation a WHERE a.ItemId=i.Id AND a.OrganizationId=? AND a.CompanyId=? AND ISNULL(a.IsActive,0)=?)",u.getOrganizationId(),u.getOrganizationId(),company,active);
 }
 public Map<String,Object> rows(UserAccount u,int company,boolean status){requireCompany(u,company);var left=status?statusRows(u,company,false):allocations(u,company,false);var right=status?statusRows(u,company,true):allocations(u,company,true);if(!status){var codes=new HashMap<Integer,Object>();jdbc.queryForList("SELECT Id,ItemCodeNew FROM Item WHERE OrganizationId=?",u.getOrganizationId()).forEach(r->codes.put(num(r.get("Id")),r.get("ItemCodeNew")));for(var list:List.of(left,right))for(var r:list)r.put("ItemCodeNew",codes.get(num(r.get("Id"))));}return Map.of("left",left,"right",right);}
 public int change(UserAccount u,InventoryItemCompanyAllocationRequest r,String action){requireCompany(u,r.companyId);var selected=new LinkedHashSet<>(r.itemIds);var eligible=switch(action){case "allocate"->allocations(u,r.companyId,false);case "deallocate"->allocations(u,r.companyId,true);case "activate"->statusRows(u,r.companyId,false);case "deactivate"->statusRows(u,r.companyId,true);default->throw new IllegalArgumentException("Unknown allocation action");};var ids=new HashSet<Integer>();eligible.forEach(row->ids.add(num(row.get("Id"))));if(selected.stream().anyMatch(id->id==null||!ids.contains(id)))throw new IllegalArgumentException("Select current rows from the chosen company's grid");
  if(action.equals("allocate")){for(int item:selected)execute("EXEC dbo.Sp_ItemAllocation_Insert @Id=0, @ItemId=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @IsActive=1",item,u.getOrganizationId(),r.companyId,u.getBranchesId());}
  else {String values=selected.stream().map(String::valueOf).collect(Collectors.joining(","))+",";if(action.equals("deallocate"))execute("EXEC dbo.Sp_ItemAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemIds=?, @Activity='DeleteAllocatedItemsCompanyWise'",u.getOrganizationId(),r.companyId,values);
   else {boolean active=action.equals("activate");execute("EXEC dbo.Sp_ItemAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemIds=?, @Status=?, @Activity='UpdateItemStatusInAllocation'",u.getOrganizationId(),r.companyId,values,active);
    // The original procedure joins the item's origin company to the allocation company.
    // Complete the same selected operation for cross-company allocations within this org.
    for(int item:selected)jdbc.update("UPDATE a SET IsActive=? FROM ItemAllocation a JOIN Item i ON i.Id=a.ItemId WHERE a.OrganizationId=? AND i.OrganizationId=? AND a.CompanyId=? AND a.ItemId=? AND ISNULL(a.IsActive,0)<>?",active,u.getOrganizationId(),u.getOrganizationId(),r.companyId,item,active);
   }
  }
  return selected.size();
 }
 private void execute(String sql,Object... values){jdbc.execute(sql,(PreparedStatementCallback<Void>) statement->{for(int i=0;i<values.length;i++)statement.setObject(i+1,values[i]);boolean result=statement.execute();while(true){if(result){try(var rows=statement.getResultSet()){while(rows.next()){} }}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return null;});}
 private static int num(Object value){return value==null?0:((Number)value).intValue();}
}
