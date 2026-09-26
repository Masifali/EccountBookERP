package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTypeRequest;
import com.mst.repositories.DesktopInventoryTypeRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service public class DesktopInventoryTypeService {
 private final DesktopInventoryTypeRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryTypeService(DesktopInventoryTypeRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,113,"View");return u;}
 public Map<String,Object> lookups(){return repo.lookups(user());}public List<Map<String,Object>> history(){return repo.history(user());}public Map<String,Object> record(int id){return repo.record(user(),id);}public String code(){return repo.code(user());}
 @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> save(InventoryTypeRequest r){var u=user();if(r.id<0)throw new IllegalArgumentException("Invalid item type ID");text(r.typeCode,50,"Code");text(r.typeDescription,100,"Description");var choices=repo.lookups(u);selected(choices,"itemTypes",r.type);selected(choices,"parentCategories",r.parentCategoryId);return repo.save(u,r);}
 private static void text(String s,int max,String label){if(s==null||s.isBlank()||s.trim().equals("0")||s.length()>max)throw new IllegalArgumentException("Please Insert "+label+" (maximum "+max+" characters)");}
 @SuppressWarnings("unchecked") private static void selected(Map<String,Object> choices,String key,int id){if(((List<Map<String,Object>>)choices.get(key)).stream().noneMatch(r->r.get("Id") instanceof Number n&&n.intValue()==id))throw new IllegalArgumentException("Select "+key+" from the available records");}
 private UserAccount translationUser(){var u=user();if(!repo.multiLanguage(u))throw new org.springframework.security.access.AccessDeniedException("Multi-language feature is not allocated to this company");return u;}
 public Map<String,Object> translations(){return repo.translations(translationUser());}
 @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> saveTranslation(InventoryTypeRequest.Translation r){var u=translationUser();var choices=repo.translations(u);selected(choices,"types",r.ItemTypeId);selected(choices,"languages",r.MultiLanguagesId);text(r.TypeDescription,32767,"Other Language Title");repo.saveTranslation(r);return repo.translations(u);}
}
