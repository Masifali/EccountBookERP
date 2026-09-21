package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryConsumptionRequest;
import com.mst.repositories.DesktopInventoryConsumptionRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
import org.springframework.security.access.AccessDeniedException;
@Service
public class DesktopInventoryConsumptionService {
    private final DesktopInventoryConsumptionRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
    public DesktopInventoryConsumptionService(DesktopInventoryConsumptionRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,105,"View");return u;}
    public Map<String,Object> lookups(){var u=user();var data=new LinkedHashMap<>(repo.lookups(u));var permissions=new LinkedHashMap<String,Boolean>();for(String action:List.of("Save","Update")){try{rights.require(u,105,action);permissions.put(action,true);}catch(AccessDeniedException e){permissions.put(action,false);}}data.put("permissions",permissions);return data;}
    public Map<String,Object> show(int category,int type){if(category<0||type<0)throw new IllegalArgumentException("Invalid filter");var u=user();return Map.of("available",repo.available(u,category,type),"history",repo.history(u),"codes",repo.codes(u));}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> save(InventoryConsumptionRequest r,boolean update){var u=user();rights.require(u,105,update?"Update":"Save");if(r.rows==null||r.rows.isEmpty())throw new IllegalArgumentException(update?"No items to update":"Check the items to allocate first");var ids=new HashSet<Integer>();var items=new HashSet<Integer>();for(var row:r.rows){if(row==null||row.itemId<=0||!items.add(row.itemId)||row.id<0||(update?(row.id==0||!ids.add(row.id)):row.id!=0))throw new IllegalArgumentException("Invalid or duplicate allocation row");if(row.remarks==null)row.remarks="";if(row.remarks.length()>250)throw new IllegalArgumentException("Remarks must be at most 250 characters");}return Map.of("ids",repo.write(u,r,update));}
}
