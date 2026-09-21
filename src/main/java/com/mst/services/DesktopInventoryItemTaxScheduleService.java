package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemTaxScheduleRequest;
import com.mst.repositories.DesktopInventoryItemTaxScheduleRepository;
import com.mst.security.*;
import java.time.LocalDate;
import java.util.*;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

@Service
public class DesktopInventoryItemTaxScheduleService {
    private final DesktopInventoryItemTaxScheduleRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public DesktopInventoryItemTaxScheduleService(DesktopInventoryItemTaxScheduleRepository repo, CurrentUserContext context, DesktopReportRights rights) { this.repo=repo; this.context=context; this.rights=rights; }
    private UserAccount user(String action) { var u=context.requireAccountingUser(); rights.require(u,176,action); return u; }
    private boolean allowed(UserAccount u,String action) { try {rights.require(u,176,action);return true;} catch(AccessDeniedException ex){return false;} }
    public Map<String,Object> lookups() { var u=user("View");var result=repo.lookups(u);result.put("permissions",Map.of("Save",allowed(u,"Save"),"Update",allowed(u,"Update")));return result; }
    public Map<String,Object> record(int id) { return repo.record(user("View"),id); }
    public List<Map<String,Object>> history(int category,int type,int item,int tax,LocalDate date,Boolean active) {
        if(category<0||type<0||item<0||tax<0)throw new IllegalArgumentException("Invalid history filter");
        return repo.history(user("View"),category,type,item,tax,date,active);
    }
    private void validate(InventoryItemTaxScheduleRequest r,Set<Integer> items,Set<Integer> taxes) {
        if(r==null||r.id<0)throw new IllegalArgumentException("Invalid tax schedule");
        if(!items.contains(r.itemId))throw new IllegalArgumentException("Please select an Item from this company");
        if(!taxes.contains(r.taxTypeId))throw new IllegalArgumentException("Please select a Tax Type from this company");
        if(r.effectedDate==null||r.effectedDate.getYear()<1753)throw new IllegalArgumentException("Please select a valid effected date");
    }
    private Set<Integer> ids(List<Map<String,Object>> rows) { var ids=new HashSet<Integer>();rows.forEach(row->ids.add(((Number)row.get("Id")).intValue()));return ids; }
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryItemTaxScheduleRequest r) {
        if(r==null)throw new IllegalArgumentException("Invalid tax schedule");
        var u=user(r.id==0?"Save":"Update");validate(r,ids(repo.items(u)),ids(repo.taxes(u)));return repo.save(u,r);
    }
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> allocate(List<InventoryItemTaxScheduleRequest> rows) {
        var u=user("Save");if(rows==null||rows.isEmpty())throw new IllegalArgumentException("Please select rows you want to insert");
        var items=ids(repo.items(u));var taxes=ids(repo.taxes(u));
        for(var row:rows){validate(row,items,taxes);if(row.id!=0||row.sourceId<=0||!Objects.equals(repo.record(u,row.sourceId).get("ItemId"),row.itemId))throw new IllegalArgumentException("Select an original history row from this company");}
        var saved=new ArrayList<Integer>();for(var row:rows)saved.add(((Number)repo.save(u,row).get("Id")).intValue());
        return Map.of("inserted",saved.size(),"ids",saved);
    }
}
