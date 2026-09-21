package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryStoreLedgerRequest;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
@Repository
public class InventoryStoreLedgerRepository {
 private final JdbcTemplate jdbc;
 public InventoryStoreLedgerRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> load(UserAccount u,InventoryStoreLedgerRequest r){
  var p=new LinkedHashMap<String,Object>();p.put("OrganizationId",u.getOrganizationId());p.put("CompanyId",u.getCompanyId());p.put("DateFrom",Date.valueOf(r.fromDate));p.put("DateTo",Date.valueOf(r.toDate));
  add(p,"ItemTypeId",r.typeId);add(p,"ItemCategoryId",r.categoryId);add(p,"WarehouseId",r.warehouseId);add(p,"ItemId",r.itemId);add(p,"ItemStockAc",r.accountId);add(p,"DocumentTypeId",r.documentTypeId);add(p,"ItemConditionId",r.conditionId);add(p,"RackId",r.rackId);
  if(r.saleValue)p.put("SaleValue",1);
  p.put("BranchesIds",csv(r.branchIds));if(!r.parentIds.isEmpty())p.put("Ids",csv(r.parentIds));
  if(r.parentIds.size()==1)p.put("InventoryParentCategoriesId",r.parentIds.get(0));
  var sql=new StringJoiner(", ","EXEC dbo.USP_InventoryEvaluationLedgerStore ","");p.keySet().forEach(k->sql.add("@"+k+"=?"));
  return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),p.values().toArray()));
 }
 private static void add(Map<String,Object> p,String key,int value){if(value>0)p.put(key,value);}
 private static String csv(List<Integer> ids){return String.join(",",ids.stream().map(String::valueOf).toList());}
}
