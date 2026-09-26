package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.TradeReportRequest;
import com.mst.repositories.TradeReportRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class TradeReportService {
    private final TradeReportRepository repository;
    private final CurrentUserContext context;
    public TradeReportService(TradeReportRepository repository,CurrentUserContext context) { this.repository=repository; this.context=context; }
    private UserAccount user() {
        UserAccount u=context.requireAccountingUser();
        if(u.getOrganizationId()<=0 || u.getCompanyId()<=0 || u.getId()==null || u.getAppId()==null)
            throw new IllegalStateException("The signed-in user needs a valid accounting company and application");
        return u;
    }
    public Map<String,Object> lookups() { return repository.lookups(user()); }
    public List<Map<String,Object>> accounts(int costCenter) { return repository.accounts(user(),costCenter); }
    public List<Map<String,Object>> load(TradeReportRequest r) {
        if(r.getFromDate()==null || r.getToDate()==null || r.getFromDate().isAfter(r.getToDate())) throw new IllegalArgumentException("Select a valid From and To date");
        if(r.getAccountClass()!=2 && r.getAccountClass()!=3) throw new IllegalArgumentException("Invalid report account class");
        if(!Set.of(0,2,3).contains(r.getBalanceClass())) throw new IllegalArgumentException("Invalid balance classification");
        UserAccount u=user();
        if(u.getAppId()==5 && r.getCostCenterId()==0) throw new IllegalArgumentException("Cost Center Not Found");
        return repository.load(u,r);
    }
}
