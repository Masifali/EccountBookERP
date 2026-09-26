package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemWarehouseAllocationRequest;
import com.mst.repositories.DesktopInventoryItemWarehouseAllocationRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryItemWarehouseAllocationService {
 private final DesktopInventoryItemWarehouseAllocationRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryItemWarehouseAllocationService(DesktopInventoryItemWarehouseAllocationRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
 public Map<String,Object> lookups(){return repo.lookups(user());}
 public Map<String,Object> rows(int category,int type){if(category<0||type<0)throw new IllegalArgumentException("Invalid item filter");return repo.rows(user(),category,type);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public Map<String,Object> save(InventoryItemWarehouseAllocationRequest r){var u=user();if(r==null||r.warehouseIds==null||r.warehouseIds.isEmpty())throw new IllegalArgumentException("Please select Warehouse row first");if(r.itemIds==null||r.itemIds.isEmpty())throw new IllegalArgumentException("Please select Item row first");return Map.of("inserted",repo.save(u,r));}
}
