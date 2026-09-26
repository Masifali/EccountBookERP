package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryWarehouseRequest;
import com.mst.repositories.DesktopInventoryWarehouseRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

@Service
public class DesktopInventoryWarehouseService {
    private final DesktopInventoryWarehouseRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public DesktopInventoryWarehouseService(DesktopInventoryWarehouseRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,116,"View");return u;}
    public Map<String,Object> lookups(){var u=user();return Map.of("types",repo.types(),"branches",repo.branches(u));}
    public List<Map<String,Object>> history(){return repo.history(user());}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    public Map<String,Object> allocations(int branch){return repo.allocations(user(),branch);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryWarehouseRequest r){
        var u=user();
        if(r.id<0)throw new IllegalArgumentException("Invalid warehouse ID");
        text(r.code,50,"Warehouse Code");text(r.name,150,"Warehouse Name");
        if(repo.types().stream().noneMatch(row->((Number)row.get("Id")).intValue()==r.warehouseType))throw new IllegalArgumentException("Department Type Field Is Required");
        return repo.save(u,r);
    }
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> allocate(InventoryWarehouseRequest.Allocation r){
        var u=user();
        if(r.warehouseIds==null||r.warehouseIds.isEmpty()||r.warehouseIds.stream().anyMatch(id->id==null||id<=0))throw new IllegalArgumentException("Check the warehouse rows first");
        repo.allocate(u,r);return repo.allocations(u,r.branchId);
    }
    public Map<String,Object> rackLookups(){var u=user();return Map.of("warehouses",repo.rackWarehouses(u),"racks",repo.racks(u));}
    public List<Map<String,Object>> rackHistory(){return repo.rackHistory(user());}
    public Map<String,Object> rack(int id){return repo.rack(user(),id);}
    public Map<String,Object> rackItems(int rack){return repo.rackItems(user(),rack);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> saveRack(InventoryWarehouseRequest.Rack r){
        var u=user();text(r.name,150,"Rack Name");
        if(r.id<0||r.sortNo<0)throw new IllegalArgumentException("Enter a valid rack ID and whole-number Sort No");
        return repo.saveRack(u,r);
    }
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> saveRackItems(InventoryWarehouseRequest.RackItems r){
        var u=user();
        if(r.itemIds==null||r.itemIds.isEmpty()||r.itemIds.stream().anyMatch(id->id==null||id<=0))throw new IllegalArgumentException("Check the item rows first");
        repo.saveRackItems(u,r);return repo.rackItems(u,r.rackId);
    }
    private static void text(String value,int max,String label){if(value==null||value.isBlank()||value.length()>max)throw new IllegalArgumentException("Enter "+label+" (maximum "+max+" characters)");}
}
