package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLanguageRequest;
import com.mst.repositories.DesktopInventoryItemLanguageRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryItemLanguageService {
 private final DesktopInventoryItemLanguageRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryItemLanguageService(DesktopInventoryItemLanguageRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
 public Map<String,Object> lookups(){return repo.lookups(user());}
 public List<Map<String,Object>> history(){return repo.history(user());}
 public Map<String,Object> record(int id){return repo.record(user(),id);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public Map<String,Object> save(InventoryItemLanguageRequest r){var u=user();if(r==null||r.id<0)throw new IllegalArgumentException("Invalid translation");if(r.itemId<=0)throw new IllegalArgumentException("Item Name Field is Required");if(r.languageId<=0)throw new IllegalArgumentException("Language Field is Required");if(r.title==null||r.title.isBlank())throw new IllegalArgumentException("Other Language Title Field is Required");return repo.save(u,r);}
}
