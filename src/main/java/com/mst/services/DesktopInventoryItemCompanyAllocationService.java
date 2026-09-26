package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemCompanyAllocationRequest;
import com.mst.repositories.DesktopInventoryItemCompanyAllocationRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopInventoryItemCompanyAllocationService {
 private final DesktopInventoryItemCompanyAllocationRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
 public DesktopInventoryItemCompanyAllocationService(DesktopInventoryItemCompanyAllocationRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
 private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,111,"View");return u;}
 public Map<String,Object> lookups(){return repo.lookups(user());}
 public Map<String,Object> rows(int company,boolean status){return repo.rows(user(),company,status);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public Map<String,Object> change(String action,InventoryItemCompanyAllocationRequest r){var u=user();if(r==null||r.companyId<=0)throw new IllegalArgumentException("Select Company First");if(r.itemIds==null||r.itemIds.isEmpty())throw new IllegalArgumentException("Select rows first");return Map.of("changed",repo.change(u,r,action));}
}
