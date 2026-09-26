package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryConsumptionRequest;
import java.util.*;
import java.sql.Timestamp;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

@Repository
public class DesktopInventoryConsumptionRepository {
    private final JdbcTemplate jdbc;
    public DesktopInventoryConsumptionRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    /**
     * Both combos, with the desktop's exact parameter lists.
     *
     * The control labelled "Item Category" really is populated with Item.GetAll - that is the
     * desktop's own quirk (ConsumptionItems.cs:161-165, BindDDLNew on "ItemName" with the caption
     * "Item Category"), not a mistake here.
     *
     * WHY THE PARAMETER LISTS GREW. These calls sent three parameters each; the desktop's BLLs
     * always send more, and a stored procedure whose parameters have no defaults rejects the call
     * outright:
     *
     *   CommonServices.ItemGetAllServiceBind -> Item.GetAll (BLL 0583)
     *       @OrganizationId, @CompanyId, @ParentIds, @Activity
     *       ParentCategoryIds is null when the caller passes nothing, which this screen does.
     *
     *   CommonServices.GetItemTypeForComboServiceBind -> ItemType.Getall (BLL 0593)
     *       @OrganizationId, @CompanyId, @Type, @ParentCategoryIds, @Activity
     *       the helper defaults are Type = 0 and ParentCategories = "" (CommonServices.cs:1929-1945).
     *
     * The values below are exactly what the desktop sends from this form - nothing invented.
     */
    public Map<String,Object> lookups(UserAccount u){
        return Map.of(
            "items", jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ParentIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), null, "ReadByOrganizationCompanyId"),
            "types", jdbc.queryForList(
                // BLL ItemType.Getall sends @Type only when non-zero and @ParentCategoryIds only when non-empty (BLL.Inventory IL 83263);
                // @Type=0 / '' would match no rows in Sp_ItemType_GetAllMethod.
                "EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId"));
    }
    public List<Map<String,Object>> available(UserAccount u,int category,int type){
        StringBuilder sql=new StringBuilder("EXEC dbo.USP_GetItemsAllocationForConsumption @OrganizationId=?, @CompanyId=?");var args=new ArrayList<Object>(List.of(u.getOrganizationId(),u.getCompanyId()));
        if(category>0){sql.append(", @ItemCategoryId=?");args.add(category);}if(type>0){sql.append(", @ItemTypeId=?");args.add(type);}return jdbc.queryForList(sql.toString(),args.toArray());
    }
    public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.USP_ConsumptionItems_FormHistory @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());}
    public Map<Integer,String> codes(UserAccount u){var result=new LinkedHashMap<Integer,String>();jdbc.queryForList("SELECT Id,ItemCodeNew FROM dbo.Item WHERE OrganizationId=? AND CompanyId=?",u.getOrganizationId(),u.getCompanyId()).forEach(r->result.put(((Number)r.get("Id")).intValue(),Objects.toString(r.get("ItemCodeNew"),"")));return result;}
    public Map<String,Object> record(UserAccount u,int id){var rows=jdbc.queryForList("SELECT * FROM dbo.ConsumptionItems WHERE Id=? AND OrganizationId=? AND CompanyId=?",id,u.getOrganizationId(),u.getCompanyId());if(rows.size()!=1)throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Consumption allocation not found in this company");return rows.get(0);}
    public List<Integer> write(UserAccount u,InventoryConsumptionRequest r,boolean update){
        var available=new HashSet<Integer>();available(u,0,0).forEach(row->available.add(((Number)row.get("Id")).intValue()));var ids=new ArrayList<Integer>();var now=new Timestamp(System.currentTimeMillis());
        for(var row:r.rows){
            if(update){var old=record(u,row.id);if(!Objects.equals(old.get("ItemId"),row.itemId))throw new IllegalArgumentException("An allocation cannot be reassigned to another item");}
            else if(!available.remove(row.itemId))throw new IllegalArgumentException("This item is already allocated or belongs to another company. Refresh the list.");
            new DesktopInventoryMinMaxRepository(jdbc).requireItem(u,row.itemId);
            Object[] values={row.id,row.itemId,update?row.active:true,row.remarks,u.getOrganizationId(),u.getCompanyId(),u.getBranchesId(),now,u.getId(),now,u.getId()};
            int id=jdbc.execute("EXEC dbo.USP_ConsumptionItems_Insert @Id=?, @ItemId=?, @IsActive=?, @Remarks=?, @OrganizationId=?, @CompanyId=?, @BranchesId=?, @EntryDate=?, @EntryUserId=?, @ModifyDate=?, @ModifyUserId=?",(PreparedStatementCallback<Integer>) s->{for(int i=0;i<values.length;i++)s.setObject(i+1,values[i]);int saved=row.id;boolean result=s.execute();while(true){if(result){try(var rs=s.getResultSet()){while(rs.next())if(rs.getObject(1) instanceof Number n)saved=n.intValue();}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}return saved;});if(id<=0)throw new IllegalStateException("Consumption allocation returned no ID");ids.add(id);
        }
        return ids;
    }
}
