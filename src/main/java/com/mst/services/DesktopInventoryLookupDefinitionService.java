package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryLookupDefinitionRequest;
import com.mst.repositories.DesktopInventoryLookupDefinitionRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryLookupDefinitionService {
 private final DesktopInventoryLookupDefinitionRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryLookupDefinitionService(DesktopInventoryLookupDefinitionRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
 public List<Map<String,Object>> types(){return repo.types(user());}
 public List<Map<String,Object>> history(){return repo.history(user());}
 public Map<String,Object> record(int id){return repo.record(user(),id);}
 public Map<String,Object> code(int type){return Map.of("code",repo.code(user(),type));}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public Map<String,Object> save(InventoryLookupDefinitionRequest r){var u=user();if(r==null||r.id<0)throw new IllegalArgumentException("Invalid lookup");if(r.typeId<=0)throw new IllegalArgumentException("Profile Name Field Required");if(r.code==null)throw new IllegalArgumentException("Code Field Required");if(r.name==null||r.name.isBlank()||r.name.length()>50)throw new IllegalArgumentException("Lookup Name is required and must not exceed 50 characters");return repo.save(u,r);}
}
