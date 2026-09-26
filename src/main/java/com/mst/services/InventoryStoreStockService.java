package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryStoreStockRequest;
import com.mst.repositories.InventoryStoreStockRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
import static com.mst.repositories.InventoryStoreStockRepository.*;
@Service
public class InventoryStoreStockService {
 private final InventoryStoreStockRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public InventoryStoreStockService(InventoryStoreStockRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,288,"View");return u;}
 public Map<String,Object> lookups(){var u=user();return Map.of("branches",repo.branches(u),"financialYears",repo.years(u),"branchId",u.getBranchesId(),"manual",repo.manual(u));}
 public List<Map<String,Object>> choices(List<Integer> branches){var u=user();branches(u,branches);return repo.choices(u,branches);}
 private void branches(UserAccount u,List<Integer> ids){if(ids==null||ids.isEmpty())throw new IllegalArgumentException("Select Branch First");Set<Integer> allowed=new HashSet<>();repo.branches(u).forEach(row->allowed.add(integer(row.get("BranchId"))));if(ids.size()!=new HashSet<>(ids).size()||!allowed.containsAll(ids))throw new IllegalArgumentException("Select branches allocated to the current user");}
 private void validate(UserAccount u,InventoryStoreStockRequest r){if(r==null||r.fromDate==null||r.toDate==null||r.fromDate.isAfter(r.toDate))throw new IllegalArgumentException("Select a valid From Date and To Date");branches(u,r.branchIds);if(r.parentIds==null||r.parentIds.isEmpty()||!Set.of(7,8).containsAll(r.parentIds))throw new IllegalArgumentException("Select a store Parent Category");if(!Set.of("ItemStockSummary","WarehouseAndItem","ItemAndWarehouse","ItemandPackSizeStockSummary").contains(r.reportType))throw new IllegalArgumentException("Select Report Type");for(int id:List.of(r.categoryId,r.typeId,r.warehouseId,r.itemId,r.accountId,r.conditionId,r.rackId))if(id<0)throw new IllegalArgumentException("Invalid report filter");}
 private static int desktopInt(java.math.BigDecimal value){try{return value.setScale(0,java.math.RoundingMode.HALF_EVEN).intValueExact();}catch(ArithmeticException ex){return 0;}}
 public Map<String,Object> load(InventoryStoreStockRequest r){var u=user();validate(u,r);return result(u,r);}
 private Map<String,Object> result(UserAccount u,InventoryStoreStockRequest r){return Map.of("rows",repo.load(u,r),"manual",repo.manual(u)&&r.reportType.equals("ItemStockSummary")&&!r.itemWise);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public Map<String,Object> update(InventoryStoreStockRequest r){
  var u=user();validate(u,r);if(!repo.manual(u)||!r.reportType.equals("ItemStockSummary")||r.itemWise)throw new IllegalArgumentException("Manual rate updates are not enabled for this report");if(r.rates==null||r.rates.isEmpty())throw new IllegalArgumentException("No Record Found For Update...");var years=repo.years(u);if(years.size()!=1)throw new IllegalArgumentException("Select one active Financial Year before updating rates");int year=integer(years.get(0).get("Id"));var rows=repo.load(u,r);var changed=new HashSet<String>();int count=0;
  for(var edit:r.rates){if(edit==null||edit.rate==null||desktopInt(edit.rate)<=0)throw new IllegalArgumentException("Enter a positive manual rate");String key=edit.itemId+":"+edit.packUomId+":"+edit.packingTypeId;if(!changed.add(key))throw new IllegalArgumentException("The same item/UOM/packing rate appears more than once");var candidates=rows.stream().filter(row->integer(row.get("ItemId"))==edit.itemId&&integer(row.get("PackUomId"))==edit.packUomId&&integer(row.get("PackingTypeId"))==edit.packingTypeId&&integer(row.get("ItemConditionId"))==edit.conditionId).toList();if(candidates.size()!=1)throw new IllegalArgumentException("Reload the report before updating a manual rate");var row=candidates.get(0);if(decimal(row.get("BalWeight")).signum()<=0)throw new IllegalArgumentException("Manual amount requires a positive balance quantity");if(desktopInt(decimal(row.get("ManualRate")))==desktopInt(edit.rate))continue;repo.saveRate(u,year,r,row,edit.rate);count++;}
  if(count==0)throw new IllegalArgumentException("No Record Found For Update...");return result(u,r);
 }
}
