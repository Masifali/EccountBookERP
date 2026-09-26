package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLookupRequest;
import com.mst.repositories.DesktopInventoryItemLookupRepository;
import com.mst.repositories.DesktopInventoryItemLookupRepository.Kind;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryItemLookupService {
    private final DesktopInventoryItemLookupRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
    public DesktopInventoryItemLookupService(DesktopInventoryItemLookupRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    // Neither recovered child has additional Save/Update rights checks.
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
    public List<Map<String,Object>> history(Kind kind){return repo.history(user(),kind);}
    public Map<String,Object> record(Kind kind,int id){return repo.record(user(),kind,id);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(Kind kind,InventoryItemLookupRequest r){var u=user();if(r==null||r.id<0)throw new IllegalArgumentException("Invalid record");if(r.name==null||r.name.isBlank())throw new IllegalArgumentException((kind==Kind.CROP_YEAR?"Crop Year":"Item Group")+" Field Required");if(r.name.length()>50)throw new IllegalArgumentException("Name cannot exceed 50 characters");return repo.save(u,kind,r);}
}
