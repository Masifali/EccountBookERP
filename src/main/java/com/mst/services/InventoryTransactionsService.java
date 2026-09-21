package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTransactionsRequest;
import com.mst.repositories.InventoryTransactionsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

@Service
public class InventoryTransactionsService {
    private final InventoryTransactionsRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public InventoryTransactionsService(InventoryTransactionsRepository repository,CurrentUserContext context,DesktopReportRights rights) {
        this.repository=repository; this.context=context; this.rights=rights;
    }
    private UserAccount user() { UserAccount user=context.requireAccountingUser(); rights.require(user,580,"View"); return user; }
    public Map<String,Object> lookups() { return repository.lookups(user()); }
    public List<Map<String,Object>> load(InventoryTransactionsRequest r) {
        UserAccount user=user();
        if(r==null || r.getFromDate()==null || r.getToDate()==null)
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"From Date and To Date are required");
        if(r.getFromDate()!=null && r.getToDate()!=null && r.getFromDate().isAfter(r.getToDate()))
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"From Date must be on or before To Date");
        for(String ids:Arrays.asList(r.getItemTypeIds(),r.getWarehouseIds())) {
            if(ids!=null && !ids.isBlank() && !ids.matches("[1-9][0-9]*(,[1-9][0-9]*)*"))
                throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Choose valid items from the dropdown");
        }
        return repository.load(user,r);
    }
}
