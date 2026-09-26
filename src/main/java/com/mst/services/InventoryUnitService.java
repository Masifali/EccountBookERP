package com.mst.services;

import com.mst.models.dto.InventoryUnitRequest;
import com.mst.repositories.InventoryUnitRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

/** POS base-unit child dialog: desktop opens directly from InvAddItemsPOS. */
@Service
public class InventoryUnitService {
    private final InventoryUnitRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
    public InventoryUnitService(InventoryUnitRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private com.mst.models.UserAccount user(){var u=context.requireAccountingUser();rights.require(u,106,"View");return u;}
    public Map<String,Object> history(){var u=user();return Map.of("rows",repo.history(u),"parents",repo.parents());}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryUnitRequest r){var u=user();validate(r);if(r.parentUomId>0&&repo.parents().stream().noneMatch(p->((Number)p.get("Id")).intValue()==r.parentUomId))throw new IllegalArgumentException("Select a parent UOM from the list");return repo.save(u,r);}
    public static void validate(InventoryUnitRequest r){if(r.id<0||r.parentUomId<0)throw new IllegalArgumentException("Invalid UOM ID");if(r.code==null||r.code.isBlank())throw new IllegalArgumentException("UomCode field is required");if(r.code.trim().length()>20)throw new IllegalArgumentException("UomCode cannot exceed 20 characters");if(r.equivalent==null||r.equivalent.signum()==0||!Double.isFinite(r.equivalent.doubleValue()))throw new IllegalArgumentException("Equivalent field is required");if(r.qtyEquivalent==null||r.qtyEquivalent.signum()==0||!Double.isFinite(r.qtyEquivalent.doubleValue()))throw new IllegalArgumentException("QtyEquivalent field is required");}
}
