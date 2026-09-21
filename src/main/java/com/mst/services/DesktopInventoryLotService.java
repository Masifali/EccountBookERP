package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryLotRequest;
import com.mst.repositories.DesktopInventoryLotRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

@Service
public class DesktopInventoryLotService {
    private final DesktopInventoryLotRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public DesktopInventoryLotService(DesktopInventoryLotRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,110,"View");return u;}
    public Map<String,Object> lookups(){return repo.lookups(user());}
    public List<Map<String,Object>> history(){return repo.history(user());}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    public List<Map<String,Object>> references(int type,int record){return repo.references(user(),type,record);}
    public Map<String,Object> allocations(int branch){return repo.allocations(user(),branch);}

    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryLotRequest r){
        var u=user();
        if(r.id<0||r.accountId<0||r.refDocumentTypeId<0||r.refDocNoId<0)throw new IllegalArgumentException("Invalid job lot or reference ID");
        text(r.code,50,"Code");text(r.description,500,"Description");
        // These are workflow states declared by AcfrmDefineLots.StatusFill, not lookup records.
        if(!Set.of("Complete","InComplete","Close").contains(Objects.toString(r.status,"")))throw new IllegalArgumentException("Select Job Status");
        if(r.startDate==null||r.endDate==null)throw new IllegalArgumentException("Enter Start Date and End Date");
        if(r.startDate.toLocalDate().isAfter(r.endDate.toLocalDate()))throw new IllegalArgumentException("StartDate cannot be greater than EndDate Please Check!");
        var choices=repo.lookups(u);
        selected(choices,"jobTypes",r.jobTypeId);
        if(Boolean.TRUE.equals(choices.get("partyProcessing"))){if(!r.company&&!r.thirdParty)throw new IllegalArgumentException("Select Company or Third Party");}
        else{r.company=true;r.thirdParty=false;}
        if(r.accountId>0)selected(choices,"accounts",r.accountId);
        if(r.refDocumentTypeId>0)selected(choices,"documentTypes",r.refDocumentTypeId);
        if(r.refDocNoId>0 && (r.refDocumentTypeId==0 || repo.references(u,r.refDocumentTypeId,r.id).stream().noneMatch(row->number(row.get("Id"))==r.refDocNoId)))throw new IllegalArgumentException("Select a reference document from this company");
        return repo.save(u,r);
    }

    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> allocate(InventoryLotRequest.Allocation r){
        var u=user();
        if(r.lotIds==null||r.lotIds.isEmpty()||r.lotIds.stream().anyMatch(id->id==null||id<=0))throw new IllegalArgumentException("Check the job lot rows first");
        repo.allocate(u,r);return repo.allocations(u,r.branchId);
    }

    private static void text(String value,int length,String label){if(value==null||value.isBlank()||value.length()>length)throw new IllegalArgumentException("Enter "+label+" (maximum "+length+" characters)");}
    @SuppressWarnings("unchecked") private static void selected(Map<String,Object> choices,String key,int id){if(((List<Map<String,Object>>)choices.get(key)).stream().noneMatch(r->number(r.get("Id"))==id))throw new IllegalArgumentException("Select "+key+" from the available records");}
    private static int number(Object n){return n==null?0:Integer.parseInt(n.toString());}
}
