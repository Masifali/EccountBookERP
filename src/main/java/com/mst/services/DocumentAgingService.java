package com.mst.services;

import com.mst.repositories.DocumentAgingRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class DocumentAgingService {
    private final DocumentAgingRepository repository;
    private final CurrentUserContext context;
    public DocumentAgingService(DocumentAgingRepository repository,CurrentUserContext context) { this.repository=repository; this.context=context; }
    public Map<String,Object> supplierLookups() { return repository.supplierLookups(context.requireAccountingUser()); }
    public Map<String,Object> customerLookups() { return repository.customerLookups(context.requireAccountingUser()); }
    public List<Map<String,Object>> customer(LocalDate date,int days,int account,int group,int custom) {
        if(date==null || days<0 || account<0 || group<0 || custom<0) throw new IllegalArgumentException("Invalid aging filters");
        return repository.customer(context.requireAccountingUser(),date,days,account,group,custom);
    }
    public List<Map<String,Object>> supplier(LocalDate date,int days,int account,int group,int custom) {
        if(date==null || days<0 || account<0 || group<0 || custom<0) throw new IllegalArgumentException("Invalid aging filters");
        return repository.supplier(context.requireAccountingUser(),date,days,account,group,custom);
    }
}
