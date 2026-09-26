package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryWarehouseRequest;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

@Repository
public class DesktopInventoryWarehouseRepository {
    private final JdbcTemplate jdbc;
    public DesktopInventoryWarehouseRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }
    public List<Map<String,Object>> history(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId");
    }
    public Map<String,Object> record(UserAccount u,int id) {
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @Id=?, @Activity=?",id,"ReadById");
        if(rows.size()!=1 || !Objects.equals(rows.get(0).get("OrganizationId"),u.getOrganizationId()) || !Objects.equals(rows.get(0).get("CompanyId"),u.getCompanyId()))
            throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Warehouse not found in this company");
        return rows.get(0);
    }
    public List<Map<String,Object>> types() {
        return jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @Activity=?","GetWareHouseType");
    }
    public List<Map<String,Object>> branches(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"GetAll");
    }
    public Map<String,Object> save(UserAccount u,InventoryWarehouseRequest r) {
        // The C# model has no BranchesId property. Preserve an existing value rather
        // than clearing an ownership attribute that this form cannot edit.
        Object branch = r.id>0 ? record(u,r.id).get("BranchesId") : null;
        var now=Timestamp.valueOf(LocalDateTime.now());
        int id=execute("EXEC dbo.Sp_InvWareHouse_"+(r.id>0?"Update":"Insert")+" @CompanyId=?, @EntryDate=?, @EntryUser=?, @Id=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @WareHouseCode=?, @WareHouseName=?, @IsActive=?, @BranchesId=?, @WarehouseType=?",
            new Object[]{u.getCompanyId(),now,u.getId(),r.id,now,u.getId(),u.getOrganizationId(),r.code.trim(),r.name.trim(),r.active,branch,String.valueOf(r.warehouseType)},r.id);
        if(id<=0)throw new IllegalStateException("Warehouse save returned no ID");
        if(r.id==0) {
            // Restrict the legacy procedure's unscoped auto-allocation, only for this new record.
            jdbc.update("DELETE a FROM dbo.WarehousesAllocationToBranch a WHERE a.WarehouseId=? AND a.OrganizationId=? AND a.CompanyId=? AND NOT EXISTS (SELECT 1 FROM dbo.Branches b WHERE b.Id=a.BranchId AND b.OrganizationId=a.OrganizationId AND b.CompanyId=a.CompanyId)",id,u.getOrganizationId(),u.getCompanyId());
        }
        return record(u,id);
    }
    public Map<String,Object> allocations(UserAccount u,int branch) {
        requireBranch(u,branch);
        return Map.of("unallocated",allocationRows(u,branch,1),"allocated",allocationRows(u,branch,2));
    }
    private List<Map<String,Object>> allocationRows(UserAccount u,int branch,int action) {
        return jdbc.queryForList("EXEC dbo.USP_WarehousesAllocatedOrUnAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?, @ActionId=?",u.getOrganizationId(),u.getCompanyId(),branch,action);
    }
    public void allocate(UserAccount u,InventoryWarehouseRequest.Allocation r) {
        requireBranch(u,r.branchId);
        var available=new HashSet<Integer>();
        allocationRows(u,r.branchId,r.allocate?1:2).forEach(row->available.add(number(row.get("WarehouseId"))));
        for(int id:r.warehouseIds) {
            record(u,id);
            if(!available.contains(id))throw new IllegalArgumentException("The selected allocation changed. Refresh the branch and select the rows again.");
        }
        if(r.allocate) {
            var now=Timestamp.valueOf(LocalDateTime.now());
            for(int id:new LinkedHashSet<>(r.warehouseIds))execute("EXEC dbo.USP_WarehousesAllocationToBranch_Insert @Id=?, @WarehouseId=?, @BranchId=?, @EntryDate=?, @EntryUserId=?, @ModifyDate=?, @ModifyUserId=?, @OrganizationId=?, @CompanyId=?, @FinancialYearId=?",new Object[]{0,id,r.branchId,now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),0},0);
        } else {
            for(int id:r.warehouseIds)if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.WarehousesAllocationToBranch WHERE BranchId=? AND WarehouseId=? AND (OrganizationId<>? OR CompanyId<>?)",Integer.class,r.branchId,id,u.getOrganizationId(),u.getCompanyId())>0)
                throw new IllegalArgumentException("This allocation contains inconsistent company ownership");
            execute("EXEC dbo.USP_WarehousesAllocationToBranchDeleteById @BranchId=?, @WarehouseIds=?",new Object[]{r.branchId,String.join(",",r.warehouseIds.stream().distinct().map(String::valueOf).toList())},0);
        }
    }
    public List<Map<String,Object>> rackWarehouses(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()).stream().filter(r->Boolean.TRUE.equals(r.get("IsActive"))).toList();
    }
    public List<Map<String,Object>> rackHistory(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.usp_invWarehouseRack_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadAll");
    }
    public List<Map<String,Object>> racks(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.usp_getRackswithWarehouse @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
    }
    public Map<String,Object> rack(UserAccount u,int id) {
        return rackHistory(u).stream().filter(r->number(r.get("Id"))==id).findFirst().orElseThrow(()->new ResponseStatusException(HttpStatus.NOT_FOUND,"Rack not found in this company"));
    }
    public Map<String,Object> saveRack(UserAccount u,InventoryWarehouseRequest.Rack r) {
        if(r.id>0)rack(u,r.id);
        record(u,r.warehouseId);
        if(rackWarehouses(u).stream().noneMatch(row->number(row.get("Id"))==r.warehouseId))throw new IllegalArgumentException("Select an active warehouse allocated to this branch");
        if(rackHistory(u).stream().anyMatch(row->number(row.get("Id"))!=r.id && number(row.get("invWarehouseId"))==r.warehouseId && Objects.toString(row.get("rackName"),"").trim().equalsIgnoreCase(r.name.trim())))throw new IllegalArgumentException("Rack already exists in selected warehouse");
        var now=Timestamp.valueOf(LocalDateTime.now());
        int id=execute("EXEC dbo.USP_invWarehouseRack_InsertAndUpdate @Id=?, @invWarehouseId=?, @rackName=?, @sortNo=?, @isActive=?, @EntryDate=?, @EntryUserId=?, @ModifyDate=?, @ModifyUserId=?",new Object[]{r.id,r.warehouseId,r.name,r.sortNo,r.active,now,u.getId(),now,u.getId()},r.id);
        return rack(u,id);
    }
    public Map<String,Object> rackItems(UserAccount u,int rackId) {
        requireActiveRack(u,rackId);
        var codes=new LinkedHashMap<Integer,Object>();
        jdbc.queryForList("SELECT Id,ItemCodeNew FROM dbo.Item WHERE OrganizationId=? AND CompanyId=?",u.getOrganizationId(),u.getCompanyId()).forEach(row->codes.put(number(row.get("Id")),row.get("ItemCodeNew")));
        return Map.of("unallocated",rackItemRows(u,rackId,1),"allocated",rackItemRows(u,rackId,2),"codes",codes);
    }
    private List<Map<String,Object>> rackItemRows(UserAccount u,int rackId,int action) {
        return jdbc.queryForList("EXEC dbo.usp_GetItemsByRackAllocation @OrganizationId=?, @CompanyId=?, @ActionId=?, @StoreRackId=?",u.getOrganizationId(),u.getCompanyId(),action,rackId);
    }
    private void requireActiveRack(UserAccount u,int rackId) {
        if(racks(u).stream().noneMatch(row->number(row.get("Id"))==rackId))throw new IllegalArgumentException("Select an active rack from this company");
    }
    public void saveRackItems(UserAccount u,InventoryWarehouseRequest.RackItems r) {
        requireActiveRack(u,r.rackId);
        var allocated=new HashSet<Integer>();rackItemRows(u,r.rackId,2).forEach(row->allocated.add(number(row.get("Id"))));
        var available=new HashSet<Integer>();rackItemRows(u,r.rackId,1).forEach(row->available.add(number(row.get("Id"))));
        for(int item:r.itemIds)if(!allocated.contains(item) && (!r.active || !available.contains(item)))throw new IllegalArgumentException("Select an item from the current rack allocation grid");
        var now=Timestamp.valueOf(LocalDateTime.now());
        for(int item:new LinkedHashSet<>(r.itemIds))execute("EXEC dbo.USP_ItemAllocationToStoreRack_InsertAndUpdate @Id=?, @itemId=?, @storeRackId=?, @isActive=?, @EntryUserId=?, @EntryDate=?, @ModifyUserId=?, @ModifyDate=?",new Object[]{0,item,r.rackId,r.active,u.getId(),now,u.getId(),now},0);
    }
    private void requireBranch(UserAccount u,int id) {
        if(branches(u).stream().noneMatch(r->number(r.get("Id"))==id))throw new IllegalArgumentException("Select a branch from this company");
    }
    private int execute(String sql,Object[] values,int fallback) {
        return jdbc.execute(sql,(PreparedStatementCallback<Integer>)statement->{
            for(int i=0;i<values.length;i++)statement.setObject(i+1,values[i]);
            int saved=fallback;boolean result=statement.execute();
            while(true){if(result){try(var rs=statement.getResultSet()){while(rs.next())if(rs.getObject(1) instanceof Number n && n.intValue()>0)saved=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}
            return saved;
        });
    }
    private static int number(Object n){return n==null?0:Integer.parseInt(n.toString());}
}
