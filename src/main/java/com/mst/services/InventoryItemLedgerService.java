package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLedgerRequest;
import com.mst.repositories.InventoryItemLedgerRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

@Service
public class InventoryItemLedgerService {
    private final InventoryItemLedgerRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public InventoryItemLedgerService(InventoryItemLedgerRepository repository,CurrentUserContext context,DesktopReportRights rights) { this.repository=repository;this.context=context;this.rights=rights; }
    private UserAccount user(){UserAccount user=context.requireAccountingUser();rights.require(user,287,"View");return user;}
    public Map<String,Object> authorizeOutput(String action) {
        UserAccount u=user();
        if(!List.of("Grid Print","Grid Export").contains(action))throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Unknown output action");
        rights.require(u,287,action);
        return Map.of("authorized",true);
    }
    public Map<String,Object> lookups(){
        UserAccount u=user();Map<String,Object> result=new LinkedHashMap<>();
        result.put("items",repository.items(u));result.put("financialYears",repository.financialYears(u));
        result.put("context",Map.of("organizationId",u.getOrganizationId(),"companyId",u.getCompanyId(),"userId",u.getId(),"branchId",u.getBranchesId()==null?0:u.getBranchesId()));
        Map<String,Boolean> permissions=new LinkedHashMap<>();
        for(String action:List.of("Grid Print","Grid Export")) {
            try {rights.require(u,287,action);permissions.put(action,true);}
            catch(org.springframework.security.access.AccessDeniedException ex){permissions.put(action,false);}
        }
        result.put("permissions",permissions);return result;
    }
    public Map<String,Object> load(InventoryItemLedgerRequest r,String action) {
        UserAccount u=user();
        if(!"View".equals(action))rights.require(u,287,action);
        if(r.getItemId()==null||r.getItemId()<=0)throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Item Field Required...");
        if(r.getFromDate()==null||r.getToDate()==null)throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"From and To Date are required");
        if(r.getFromDate().isAfter(r.getToDate()))throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"From Date must be on or before To Date");
        boolean allowed=repository.items(u).stream().anyMatch(item->r.getItemId().toString().equals(String.valueOf(item.get("Id"))));
        if(!allowed)throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Select an item from the current company's list");
        return project(repository.load(u,r));
    }
    /** Keep SQL's running balances and row order. Opening/closing are endpoints, never sums. */
    public static Map<String,Object> project(List<Map<String,Object>> raw) {
        List<Map<String,Object>> rows=new ArrayList<>();
        for(Map<String,Object> source:raw) {
            Map<String,Object> row=new LinkedHashMap<>(source);
            row.put("TranDate",source.get("TrancDate"));row.put("RateUomIn",source.get("RateUom"));row.put("RateUomOut",source.get("RateUom"));row.put("PackingType",source.get("PackTypeDesc"));rows.add(row);
        }
        Map<String,Object> result=new LinkedHashMap<>();result.put("rows",rows);
        result.put("opening",summary(raw.isEmpty()?Map.of():raw.get(0)));
        result.put("closing",summary(raw.isEmpty()?Map.of():raw.get(raw.size()-1)));
        return result;
    }
    private static Map<String,Object> summary(Map<String,Object> row) {
        Map<String,Object> result=new LinkedHashMap<>();
        for(String key:List.of("BalQty","BalWeight","BalAmount","AvgRate"))result.put(key,row.get(key)==null?"0":row.get(key));
        return result;
    }
}
