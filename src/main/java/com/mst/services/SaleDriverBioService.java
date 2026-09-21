package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDriverBioRequest;
import com.mst.repositories.SaleDriverBioRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SaleDriverBioService {
    private final SaleDriverBioRepository repository;
    private final CurrentUserContext context;
    public SaleDriverBioService(SaleDriverBioRepository repository,CurrentUserContext context){this.repository=repository;this.context=context;}
    public Map<String,Object> initial(){UserAccount u=context.requireAccountingUser();return Map.of("documentTypeId",93,"referenceDocumentTypeId",91,"pendingGatePasses",repository.pending(u,context.currentFinancialYearId()),"knownDrivers",repository.knownDrivers(u));}
    public List<Map<String,Object>> history(){return repository.history(context.requireAccountingUser(),context.currentFinancialYearId(),true);}
    public Map<String,Object> record(int id){return repository.record(context.requireAccountingUser(),id);}
    @Transactional public Map<String,Object> save(SaleDriverBioRequest request){return repository.save(context.requireAccountingUser(),request);}
}
