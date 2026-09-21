package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryVehicleTransactionsRequest;
import com.mst.repositories.InventoryVehicleTransactionsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

@Service
public class InventoryVehicleTransactionsService {
    private final InventoryVehicleTransactionsRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public InventoryVehicleTransactionsService(InventoryVehicleTransactionsRepository repository,CurrentUserContext context,DesktopReportRights rights){this.repository=repository;this.context=context;this.rights=rights;}
    private UserAccount user(){UserAccount user=context.requireAccountingUser();rights.require(user,291,"View");return user;}
    public Map<String,Object> lookups(){
        UserAccount u=user();Map<String,Object> result=repository.lookups(u),permissions=new LinkedHashMap<>();
        for(String action:List.of("Grid Print","Grid Export")){
            try{rights.require(u,291,action);permissions.put(action,true);}catch(org.springframework.security.access.AccessDeniedException ex){permissions.put(action,false);}
        }
        result.put("permissions",permissions);return result;
    }
    public List<Map<String,Object>> uoms(int itemId){return repository.uoms(user(),itemId);}
    public Map<String,Object> authorizeOutput(String action){rights.require(user(),291,action);return Map.of("authorized",true);}
    public Map<String,Object> load(InventoryVehicleTransactionsRequest r){
        UserAccount u=user();
        if(r.getFromDate()==null||r.getToDate()==null||r.getFromDate().isAfter(r.getToDate()))throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Choose a valid From Date and To Date");
        if(r.getDocumentTypeIds()!=null&&!r.getDocumentTypeIds().isBlank()&&!r.getDocumentTypeIds().matches("[1-9][0-9]*(,[1-9][0-9]*)*"))throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Choose valid reference documents");
        Map<String,Object> result=new LinkedHashMap<>();result.put("rows",project(repository.load(u,r)));
        result.put("documentFilterApplied",r.getDocumentTypeIds()!=null&&InventoryVehicleTransactionsRepository.desktopDocumentType(r.getDocumentTypeIds())!=null);
        return result;
    }
    public static List<Map<String,Object>> project(List<Map<String,Object>> source){
        List<Map<String,Object>> result=new ArrayList<>();
        for(Map<String,Object> row:source){Map<String,Object> mapped=new LinkedHashMap<>(row);mapped.put("Warehouse",row.get("WareHouseCode"));mapped.put("JobLot",row.get("JobLotCode"));mapped.put("AvgRate",row.get("AVgRate"));result.add(mapped);}
        return result;
    }
}
