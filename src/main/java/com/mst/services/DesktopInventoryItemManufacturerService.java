package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemManufacturerRequest;
import com.mst.repositories.DesktopInventoryItemManufacturerRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryItemManufacturerService {
 private final DesktopInventoryItemManufacturerRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryItemManufacturerService(DesktopInventoryItemManufacturerRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
 public Map<String,Object> lookups(){return repo.lookups(user());}
 public List<Map<String,Object>> history(){return repo.history(user());}
 public Map<String,Object> record(int id){return repo.record(user(),id);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public Map<String,Object> save(InventoryItemManufacturerRequest r){var u=user();if(r==null||r.id<0)throw new IllegalArgumentException("Invalid manufacturer allocation");if(r.itemId<=0)throw new IllegalArgumentException("Item Field is Required");if(r.manufacturerId<=0)throw new IllegalArgumentException("Manufacturer Field is Required");return repo.save(u,r);}
}
