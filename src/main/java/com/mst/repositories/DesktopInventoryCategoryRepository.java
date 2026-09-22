package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryCategoryRequest;
import java.util.*;
import java.sql.Timestamp;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

/** Inventory screen112. Fixed-asset and feed-category screens are separate entry points. */
@Repository
public class DesktopInventoryCategoryRepository {
 private final JdbcTemplate jdbc;
 private static final Set<Integer> PARENTS=Set.of(1,2,3,4,6,10,11);
 public DesktopInventoryCategoryRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @ParentCategoryIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"1,2,3,4,6,10,11","FormHistory");}
 public Map<String,Object> record(UserAccount u,int id){var rows=jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @Id=?, @Activity=?",id,"ReadById");if(rows.size()!=1||!Objects.equals(rows.get(0).get("OrganizationId"),u.getOrganizationId())||!Objects.equals(rows.get(0).get("CompanyId"),u.getCompanyId())||!PARENTS.contains(number(rows.get(0).get("InventoryParentCategoriesId"))))throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Category not found in this company");return rows.get(0);}
 public String code(UserAccount u,int parent){if(parent!=0&&!PARENTS.contains(parent))throw new IllegalArgumentException("Select an Inventory parent category");return jdbc.queryForObject("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @InventoryParentCategoriesId=?, @Activity=?",String.class,u.getOrganizationId(),u.getCompanyId(),parent,"GenerateCode");}
 public boolean feature(UserAccount u,int id){return jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()).stream().anyMatch(r->number(r.get("Id"))==id);}
 private boolean config(UserAccount u,String name){var rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),name,"GetConfigurationByOrgCompandConfigDescription");return !rows.isEmpty()&&Boolean.parseBoolean(Objects.toString(rows.get(0).get("ConfigKey"),"false"));}
 public Map<String,Object> lookups(UserAccount u){
  var data=new LinkedHashMap<String,Object>();boolean auto=config(u,"AutoCoaDefineByItemNameOnInsert"),all=config(u,"All Account Allow on Item and Item Category"),same=config(u,"SameAccountForStockAndRevenue");
  data.put("autoAccounts",auto);data.put("sameAccounts",same);data.put("autoCode",config(u,"ItemCodingEnable"));data.put("multiLanguage",feature(u,7));
  data.put("parents",jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity=?","InventoryParentCategories").stream().filter(r->PARENTS.contains(number(r.get("Id")))).toList());
  data.put("classes",jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @Activity=?","GetItemClassGroup").stream().filter(r->Set.of(1,2,3).contains(number(r.get("Id")))).toList());
  data.put("stages",jdbc.queryForList("EXEC dbo.usp_getItemProductionStage"));data.put("varieties",jdbc.queryForList("EXEC dbo.usp_getItemVarietyNature"));
  var details=jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"COAAllocationSearch");
  List<Map<String,Object>> accounts=details;
  if(auto){var years=new InventoryOpeningRepository(jdbc).years(u);if(years.isEmpty())throw new IllegalArgumentException("No active financial year");accounts=jdbc.queryForList("EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Account_Level=?, @CoaType=?",u.getOrganizationId(),u.getCompanyId(),years.get(0).get("Id"),3,"ReadAllAccountGroup");}
  data.put("inventory",filter(accounts,auto?Set.of(4):all?Set.of(4,10):Set.of(4)));
  data.put("revenue",filter(accounts,auto?(same?Set.of(4):all?Set.of(4,10):Set.of(10)):(all?Set.of(4,10):Set.of(10))));
  data.put("cgs",filter(details,Set.of(12)));return data;
 }
 private static List<Map<String,Object>> filter(List<Map<String,Object>> rows,Set<Integer> types){return rows.stream().filter(r->types.contains(number(r.get("AccountTypeId")))).toList();}
 public Map<String,Object> translations(UserAccount u){
  var owned=new HashSet<>(jdbc.queryForList("SELECT Id FROM ItemCategory WHERE OrganizationId=? AND CompanyId=?",Integer.class,u.getOrganizationId(),u.getCompanyId()));
  return Map.of("languages",jdbc.queryForList("EXEC dbo.Sp_MultiLanguages_GetAll @MethodType=?, @OrganizationId=?, @CompanyId=?","ReadAll",u.getOrganizationId(),u.getCompanyId()),"categories",jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId"),"history",jdbc.queryForList("EXEC dbo.usp_ItemCategoryMultiLanguage_History").stream().filter(r->owned.contains(number(r.get("ItemCategoryId")))).toList());
 }
 public void saveTranslation(InventoryCategoryRequest.Translation r){jdbc.execute("EXEC dbo.Sp_ItemCategoryMultilingo_Insert @Id=?, @ItemCategoryId=?, @MultiLanguagesId=?, @CategoryDescription=?",(PreparedStatementCallback<Void>) s->{s.setInt(1,0);s.setInt(2,r.ItemCategoryId);s.setInt(3,r.MultiLanguagesId);s.setNString(4,r.CategoryDescription);boolean result=s.execute();while(true){if(result){try(var rs=s.getResultSet()){while(rs.next()){/* consume original delete/insert result */}}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}return null;});}
 static int number(Object value){return value==null?0:Integer.parseInt(value.toString());}
 public Map<String,Object> save(UserAccount u,InventoryCategoryRequest r){
  var old=r.Id>0?record(u,r.Id):null;var values=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER);
  for(String name:List.of("accumulatedDepreciationAcId","DepreciationExpenseAcId","CapitalWipAcId","ExpenseMaintenanceAccountId","usefullLifeInMonths","depriciationrate","AssetCategoryId","depreciationMethodScheduleId"))values.put(name,old==null?0:old.get(name));
  // AssetGlAcId is absent from the recovered CLR model; preserve its procedure default.
  for(var field:InventoryCategoryRequest.class.getFields())try{values.put(field.getName(),field.get(r));}catch(IllegalAccessException ex){throw new IllegalStateException(ex);}
  values.put("OrganizationId",u.getOrganizationId());values.put("CompanyId",u.getCompanyId());values.put("EntryUser",u.getId());values.put("ModifyUser",u.getId());values.put("EntryDate",new Timestamp(System.currentTimeMillis()));values.put("ModifyDate",new Timestamp(System.currentTimeMillis()));
  String proc="dbo.Sp_ItemCategory_"+(r.Id==0?"Insert":"Update");var names=jdbc.queryForList("SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",String.class,proc).stream().filter(values::containsKey).toList();
  int id=jdbc.execute("EXEC "+proc+" "+String.join(",",names.stream().map(k->"@"+k+"=?").toList()),(PreparedStatementCallback<Integer>) s->{int n=1;for(String name:names)s.setObject(n++,values.get(name));int saved=r.Id;boolean result=s.execute();while(true){if(result){try(var rs=s.getResultSet()){if(rs.next()&&rs.getObject(1) instanceof Number value&&value.intValue()>0)saved=value.intValue();}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}if(saved<=0)throw new IllegalStateException("Category save returned no ID");return saved;});
  // The form's AttributesForItemFeature field is never assigned: no category allocations
  // are submitted. DAL still propagates existing attributes when company feature13 exists.
  if(feature(u,13))ProcExec.call(jdbc, "EXEC item.USP_ItemAttribute_InsertAndUpdateByCategory @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @ItemId=?",u.getOrganizationId(),u.getCompanyId(),id,null);
  return record(u,id);
 }
}
