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
    public Map<String,Object> supplierLookups() { return repository.supplierLookups(context.requireAccountingUser(),context.currentFinancialYearId()); }
    /** Screen 873 btnRefresh_Click: the Supplier list only. */
    public List<Map<String,Object>> supplierAccounts() { return repository.supplierAccounts(context.requireAccountingUser()); }
    /** Screen 872: the session year (not "exactly one active year", which failed the whole lookup and left every combo empty). */
    public Map<String,Object> customerLookups() { return repository.customerLookups(context.requireAccountingUser(),context.currentFinancialYearId()); }
    /** Screen 872 btnRefresh_Click: the Customer list only. */
    public List<Map<String,Object>> customerAccounts() { return repository.customerAccounts(context.requireAccountingUser()); }
    public List<Map<String,Object>> customer(LocalDate date,int days,int account,int group,int custom) {
        if(date==null || days<0 || account<0 || group<0 || custom<0) throw new IllegalArgumentException("Invalid aging filters");
        return repository.customer(context.requireAccountingUser(),date,days,account,group,custom);
    }
    public List<Map<String,Object>> supplier(LocalDate date,int days,int account,int group,int custom) {
        if(date==null || days<0 || account<0 || group<0 || custom<0) throw new IllegalArgumentException("Invalid aging filters");
        return repository.supplier(context.requireAccountingUser(),date,days,account,group,custom);
    }
}
