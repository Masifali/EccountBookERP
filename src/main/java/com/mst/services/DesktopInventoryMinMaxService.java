package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryMinMaxRequest;
import com.mst.repositories.DesktopInventoryMinMaxRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
import org.springframework.security.access.AccessDeniedException;

@Service
public class DesktopInventoryMinMaxService {
    private final DesktopInventoryMinMaxRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
    public DesktopInventoryMinMaxService(DesktopInventoryMinMaxRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,104,"View");return u;}
    public Map<String,Object> lookups(){var u=user();var result=new LinkedHashMap<>(repo.lookups(u));var permissions=new LinkedHashMap<String,Boolean>();for(String action:List.of("Save","Update","Delete","Print")){try{rights.require(u,104,action);permissions.put(action,true);}catch(AccessDeniedException e){permissions.put(action,false);}}result.put("permissions",permissions);result.put("now",java.time.LocalDateTime.now().withNano(0).toString());return result;}
    public List<Map<String,Object>> items(int parent){return repo.items(user(),parent);}
    public List<Map<String,Object>> uoms(int item){return repo.uoms(user(),item);}
    public List<Map<String,Object>> last(int item,int unit){return repo.last(user(),item,unit);}
    public List<Map<String,Object>> query(InventoryMinMaxRequest.Filter f,boolean history){if(f.parent<0||f.category<0||f.type<0||f.item<0)throw new IllegalArgumentException("Invalid filter");return repo.query(user(),f,history);}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> save(InventoryMinMaxRequest r){
        var u=user();rights.require(u,104,"Save");
        if(r.effectedDate==null||r.rows==null||r.rows.isEmpty())throw new IllegalArgumentException("Enter an effective date and grid rows");
        var seen=new HashSet<Integer>();boolean hasRates=false;
        for(var row:r.rows){if(row==null||row.itemId<=0||!seen.add(row.itemId))throw new IllegalArgumentException("You cannot add the same item in the grid");repo.requireItem(u,row.itemId);if(row.minRate==null||row.maxRate==null)throw new IllegalArgumentException("Enter numeric rates");if(row.minRate.signum()>0&&row.maxRate.signum()>0){/* desktop skips rows without both rates (ItemMinMaxRateSchedule.cs:709-712); only saved rows need a Rate UOM */if(row.rateUom==null||row.rateUom.signum()<=0)throw new IllegalArgumentException("Enter a positive Rate UOM");hasRates=true;if(!Double.isFinite(row.minRate.doubleValue())||!Double.isFinite(row.maxRate.doubleValue()))throw new IllegalArgumentException("Rate is too large");}}
        if(!hasRates)throw new IllegalArgumentException("Please Write New MinRate And New MaxRate For at least one Item");
        return Map.of("ids",repo.save(u,context.currentFinancialYearId(),r));
    }
    @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> delete(InventoryMinMaxRequest.Delete r){var u=user();rights.require(u,104,"Delete");if(r.ids==null||r.ids.isEmpty()||r.ids.stream().anyMatch(i->i==null||i<=0))throw new IllegalArgumentException("Please check any row first");for(int id:new LinkedHashSet<>(r.ids))repo.delete(u,id);return Map.of("deleted",new HashSet<>(r.ids).size());}
}
