package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryCategoryRequest;
import com.mst.repositories.DesktopInventoryCategoryRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryCategoryService {
 private final DesktopInventoryCategoryRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryCategoryService(DesktopInventoryCategoryRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,112,"View");return u;}
 public Map<String,Object> lookups(){return repo.lookups(user());}public List<Map<String,Object>> history(){return repo.history(user());}public Map<String,Object> record(int id){return repo.record(user(),id);}public String code(int parent){return repo.code(user(),parent);}
 private UserAccount translationUser(){var u=user();if(!repo.feature(u,7))throw new org.springframework.security.access.AccessDeniedException("Multi-language feature is not allocated to this company");return u;}
 public Map<String,Object> translations(){return repo.translations(translationUser());}
 @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> saveTranslation(InventoryCategoryRequest.Translation r){var u=translationUser();var choices=repo.translations(u);selected(choices,"categories",r.ItemCategoryId);selected(choices,"languages",r.MultiLanguagesId);text(r.CategoryDescription,1000,"Other Language Title");repo.saveTranslation(r);return repo.translations(u);}
 @Transactional(isolation=Isolation.SERIALIZABLE) public Map<String,Object> save(InventoryCategoryRequest r){var u=user();if(r.Id<0)throw new IllegalArgumentException("Invalid category ID");text(r.CategoryCode,20,"Code");text(r.CategoryDescription,100,"Description");if(r.SerialFrom<=0||r.SerialTo<r.SerialFrom)throw new IllegalArgumentException("Enter a nonzero serial range with From no greater than To");var choices=repo.lookups(u);selected(choices,"parents",r.InventoryParentCategoriesId);if(r.ItemClassGroupId!=0)selected(choices,"classes",r.ItemClassGroupId);selected(choices,"stages",r.ItemProductionStageId);selected(choices,"varieties",r.ItemVarietyNatureId);var inventory=selected(choices,"inventory",r.InventoryAccountId);var revenue=selected(choices,"revenue",r.RevenueAccountId);selected(choices,"cgs",r.CGSAccountId);if(r.InventoryAccountId!=r.RevenueAccountId&&((Boolean.TRUE.equals(choices.get("autoAccounts"))&&Boolean.TRUE.equals(choices.get("sameAccounts")))||(Objects.equals(inventory.get("AccountTypeId"),4)&&Objects.equals(revenue.get("AccountTypeId"),4))))throw new IllegalArgumentException("Revenue Account Should be Same as Inventory Account or Select an Account Of Type Sale In Revenue Account");return repo.save(u,r);}
 private static void text(String value,int max,String field){if(value==null||value.isBlank()||value.trim().equals("0")||value.length()>max)throw new IllegalArgumentException("Please Insert "+field+" (maximum "+max+" characters)");}
 @SuppressWarnings("unchecked") private static Map<String,Object> selected(Map<String,Object> choices,String key,int id){return ((List<Map<String,Object>>)choices.get(key)).stream().filter(r->r.get("Id") instanceof Number n&&n.intValue()==id).findFirst().orElseThrow(()->new IllegalArgumentException("Select "+key+" from the available records"));}
}
