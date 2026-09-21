package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryStoreStockRequest;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
@Repository
public class InventoryStoreStockRepository {
 private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(InventoryStoreStockRepository.class);
 private final JdbcTemplate jdbc;
 public InventoryStoreStockRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> branches(UserAccount u){
  /* Three fallbacks, and every one of them used to swallow its exception, so "the procedure
     failed" and "this company has no branches" both arrived at the screen as an empty list and
     select2 said "No results found" either way. Each step now says what happened. Behaviour is
     unchanged - the same list is returned - but a failure is no longer invisible.

     Note the last fallback reads Branches with NO OrganizationId/CompanyId filter at all, i.e.
     every tenant's branches. Left in place because removing a fallback changes which branches the
     screen offers, but it is flagged: see the note in the project docs. */
  List<Map<String,Object>> list = new ArrayList<>();
  try {
      list = jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?, @CompanyId=?, @UserId=?",u.getOrganizationId(),u.getCompanyId(),u.getId());
      if (list.isEmpty()) LOG.info("USP_GetBranchsAllocatedToUser returned no branches for user {} (org {}, company {})",u.getId(),u.getOrganizationId(),u.getCompanyId());
  } catch (Exception e) { LOG.warn("USP_GetBranchsAllocatedToUser failed for user {}",u.getId(),e); }
  if (list.isEmpty()) {
      try { list = jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()); }
      catch (Exception e) { LOG.warn("USP_GetBranchesFromVouchersByAccountId fallback failed",e); }
  }
  if (list.isEmpty()) {
      try { list = jdbc.queryForList("SELECT ID as Id, BranchName FROM Branches"); }
      catch (Exception e) { LOG.warn("Branches table fallback failed",e); }
      if (list.isEmpty()) LOG.warn("No branches from ANY of the three sources - the Branch Name picker will show 'No results found'");
  }
  for (Map<String, Object> map : list) {
      Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : (map.get("BranchId") != null ? map.get("BranchId") : map.get("branchId")));
      Object name = map.get("BranchName") != null ? map.get("BranchName") : (map.get("branchName") != null ? map.get("branchName") : (map.get("Name") != null ? map.get("Name") : map.get("name")));
      map.put("Id", id); map.put("id", id); map.put("ID", id); map.put("BranchId", id); map.put("branchId", id);
      map.put("BranchName", name); map.put("branchName", name); map.put("Name", name); map.put("name", name);
  }
  return list;
 }
 public List<Map<String,Object>> years(UserAccount u){return jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());}
 public boolean manual(UserAccount u){var r=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription='ManualRateAndAmountForItemStockSummary', @Activity='GetConfigurationByOrgCompandConfigDescription'",u.getOrganizationId(),u.getCompanyId());return !r.isEmpty()&&Set.of("1","true").contains(Objects.toString(r.get(0).get("ConfigKey"),"").toLowerCase(Locale.ROOT));}
 public List<Map<String,Object>> choices(UserAccount u,List<Integer> branches){return jdbc.queryForList("EXEC dbo.USP_Evalaution_DropDown_ByParentCategories @OrganizationId=?, @CompanyId=?, @BranchesIds=?, @InventoryParentCategory='7,8'",u.getOrganizationId(),u.getCompanyId(),csv(branches));}
 public List<Map<String,Object>> load(UserAccount u,InventoryStoreStockRequest r){
  var p=new LinkedHashMap<String,Object>();p.put("OrganizationId",u.getOrganizationId());p.put("CompanyId",u.getCompanyId());p.put("DateFrom",Date.valueOf(r.fromDate));p.put("DateTo",Date.valueOf(r.toDate));
  add(p,"ItemTypeId",r.typeId);add(p,"ItemCategoryId",r.categoryId);add(p,"WarehouseId",r.warehouseId);add(p,"ItemId",r.itemId);if(!r.itemWise){p.put("IsPackSizeOn",1);p.put("IsPackTypeOn",1);}add(p,"ItemStockAc",r.accountId);p.put("InventoryParentCategoriesIds",csv(r.parentIds));if(r.saleValue)p.put("SaleValue",1);if(r.skipZero)p.put("SkipZero",1);add(p,"ItemConditionId",r.conditionId);p.put("BranchesIds",csv(r.branchIds));add(p,"RackId",r.rackId);p.put("Activity",r.reportType.equals("WarehouseAndItem")||r.reportType.equals("ItemAndWarehouse")?"ItemandWarehouseStockSummary":r.reportType);
  var sql=new StringJoiner(", ","EXEC dbo.Sp_ItemStockReportWithValues_Store ","");p.keySet().forEach(k->sql.add("@"+k+"=?"));return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),p.values().toArray()));
 }
 public void saveRate(UserAccount u,int year,InventoryStoreStockRequest request,Map<String,Object> row,java.math.BigDecimal rate){
  int item=integer(row.get("ItemId")),unit=integer(row.get("PackUomId")),pack=integer(row.get("PackingTypeId"));
  // The original procedure matches a date/item/UOM/packing key without tenant columns.
  if(jdbc.queryForObject("SELECT COUNT(*) FROM StockSummaryByItem WITH (UPDLOCK,HOLDLOCK) WHERE ItemId=? AND ItemUOMId=? AND PackingTypeId=? AND CONVERT(date,DocDate)=? AND (OrganizationId<>? OR CompanyId<>? OR OrganizationId IS NULL OR CompanyId IS NULL)",Integer.class,item,unit,pack,Date.valueOf(request.toDate),u.getOrganizationId(),u.getCompanyId())>0)throw new IllegalArgumentException("A matching stock rate belongs to another company; it cannot be updated here");
  java.math.BigDecimal weight=decimal(row.get("BalWeight")),amount=weight.multiply(rate);Timestamp now=Timestamp.valueOf(LocalDateTime.now());
  Object[] args={item,unit,pack,decimal(row.get("BalQty")),rate,weight,amount,u.getOrganizationId(),u.getCompanyId(),year,now,now,Date.valueOf(request.toDate),u.getId(),u.getId()};
  jdbc.execute("EXEC dbo.USP_StockSummaryByItem_InsertAndUpdate @Id=0, @ItemId=?, @ItemUOMId=?, @PackingTypeId=?, @ItemQty=?, @ItemRate=?, @NetWeight=?, @RateUOM=1, @Amount=?, @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @ActionId=0, @EntryDate=?, @ModifyDate=?, @DocDate=?, @EntryUserId=?, @ModifyUserId=?",(org.springframework.jdbc.core.PreparedStatementCallback<Void>) statement->{for(int i=0;i<args.length;i++)statement.setObject(i+1,args[i]);boolean result=statement.execute();while(true){if(result){try(var rs=statement.getResultSet()){while(rs.next()){/* Original insert returns Id; update returns none. */}}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return null;});
 }
 private static void add(Map<String,Object> p,String key,int value){if(value>0)p.put(key,value);}
 private static String csv(List<Integer> ids){return String.join(",",ids.stream().map(String::valueOf).toList());}
 public static int integer(Object value){return value==null?0:((Number)value).intValue();}
 public static java.math.BigDecimal decimal(Object value){return value==null?java.math.BigDecimal.ZERO:new java.math.BigDecimal(value.toString());}
}
