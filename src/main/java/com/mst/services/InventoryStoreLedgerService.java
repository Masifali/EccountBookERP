package com.mst.services;
import com.mst.models.dto.InventoryStoreLedgerRequest;
import com.mst.repositories.InventoryStoreLedgerRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
@Service
public class InventoryStoreLedgerService {
 private final InventoryStoreLedgerRepository repo;private final InventoryStoreStockService stock;private final CurrentUserContext context;private final DesktopReportRights rights;
 public InventoryStoreLedgerService(InventoryStoreLedgerRepository repo,InventoryStoreStockService stock,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.stock=stock;this.context=context;this.rights=rights;}
 public List<Map<String,Object>> load(InventoryStoreLedgerRequest r){
  var u=context.requireAccountingUser();rights.require(u,288,"View");
  if(r==null||r.fromDate==null||r.toDate==null||r.fromDate.isAfter(r.toDate))throw new IllegalArgumentException("Select a valid From Date and To Date");
  stock.choices(r.branchIds); // Uses the same user-allocated branch boundary as the parent form.
  if(r.parentIds==null||!Set.of(7,8).containsAll(r.parentIds))throw new IllegalArgumentException("Select store Parent Categories");
  for(int id:List.of(r.categoryId,r.typeId,r.warehouseId,r.itemId,r.accountId,r.conditionId,r.rackId,r.documentTypeId))if(id<0)throw new IllegalArgumentException("Invalid ledger filter");
  return repo.load(u,r);
 }
}
