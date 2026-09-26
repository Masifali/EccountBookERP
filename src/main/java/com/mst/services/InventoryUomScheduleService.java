package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryUomScheduleRequest;
import com.mst.repositories.InventoryUomScheduleRepository;
import com.mst.security.*;
import java.math.BigDecimal;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

@Service
public class InventoryUomScheduleService {
    private final InventoryUomScheduleRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public InventoryUomScheduleService(InventoryUomScheduleRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    // Screen 115 defines only View. Its desktop form has no per-action rights check:
    // opening the form authorizes Insert/Update. Do not require nonexistent grants.
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,115,"View");return u;}
    public Map<String,Object> lookups(){var u=user();return Map.of("items",repo.items(u),"units",repo.units(u),"permissions",Map.of("Save",true,"Update",true));}
    public List<Map<String,Object>> history(Integer item){return repo.history(user(),item);}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryUomScheduleRequest r){
        var u=user();validate(r);
        if(r.id>0)repo.record(u,r.id);
        if(repo.items(u).stream().noneMatch(x->((Number)x.get("Id")).intValue()==r.itemId))throw new IllegalArgumentException("Please select an item available to this company");
        if(repo.units(u).stream().noneMatch(x->((Number)x.get("Id")).intValue()==r.scheduleUnitId))throw new IllegalArgumentException("Please select a schedule unit available to this organization");
        return repo.record(u,repo.save(u,r));
    }
    public static void validate(InventoryUomScheduleRequest r){
        if(r.id<0)throw new IllegalArgumentException("Invalid record ID");
        if(r.itemId<=0)throw new IllegalArgumentException("Please Select Item");
        if(r.scheduleUnitId<=0)throw new IllegalArgumentException("Please Select Schedule Unit");
        nonzero(r.equivalent,"Please Insert Equivalent");nonzero(r.qtyEquivalent,"Please Insert QtyEquivalent");
    }
    private static void nonzero(BigDecimal n,String message){if(n==null||n.signum()==0||!Double.isFinite(n.doubleValue()))throw new IllegalArgumentException(message);}
}
