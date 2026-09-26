package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemGroupScheduleRequest;
import com.mst.repositories.DesktopInventoryItemGroupScheduleRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryItemGroupScheduleService {
    private final DesktopInventoryItemGroupScheduleRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
    public DesktopInventoryItemGroupScheduleService(DesktopInventoryItemGroupScheduleRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
    public Map<String,Object> lookups(){return repo.lookups(user());}
    public List<Map<String,Object>> history(){return repo.history(user());}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryItemGroupScheduleRequest r){var u=user();if(r==null||r.id<0||r.groupId<=0)throw new IllegalArgumentException("Please Select Item Group");if(r.equivalent==null||r.equivalent.isBlank())throw new IllegalArgumentException("Please Insert Equivalent");if(r.equivalent.length()>50||!r.equivalent.matches("[0-9]+(?:\\.[0-9]*)?|\\.[0-9]+")||!Double.isFinite(Double.parseDouble(r.equivalent)))throw new IllegalArgumentException("Enter a valid Equivalent");return repo.save(u,r);}
    public Map<String,Object> assignmentLookups(){return repo.assignmentLookups(user());}
    public List<Map<String,Object>> assignmentRows(int parent,int category,int type,int item){return repo.assignmentRows(user(),parent,category,type,item);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> assign(InventoryItemGroupScheduleRequest.Assignment r){var u=user();if(r==null||r.groupId<=0)throw new IllegalArgumentException("Please select a valid Item Group");if(r.itemIds==null||r.itemIds.isEmpty())throw new IllegalArgumentException("Please select row first");return Map.of("assigned",repo.assign(u,r));}
}
