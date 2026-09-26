package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.AccountFollowUpRequest;
import com.mst.repositories.AccountFollowUpRepository;
import com.mst.security.*;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class AccountFollowUpService {
    private final AccountFollowUpRepository repository;private final CurrentUserContext context;private final DesktopReportRights rights;
    private final Map<String,Integer> saved=new LinkedHashMap<>();
    public AccountFollowUpService(AccountFollowUpRepository repository,CurrentUserContext context,DesktopReportRights rights){this.repository=repository;this.context=context;this.rights=rights;}
    private int screen(int accountClass){if(accountClass!=2&&accountClass!=3)throw new IllegalArgumentException("Invalid report");return accountClass==3?66:65;}
    private int screen(int accountClass,int requested){int trade=screen(accountClass);if(requested==0||requested==trade)return trade;if(requested==(accountClass==3?48:80))return requested;throw new IllegalArgumentException("Invalid follow-up screen");}
    public Map<String,Object> history(int accountClass){return history(accountClass,0);}
    public Map<String,Object> history(int accountClass,int reportScreenId){UserAccount u=context.requireAccountingUser();rights.require(u,screen(accountClass,reportScreenId),"View");return Map.of("rows",repository.history(u),"accounts",repository.accounts(u));}
    public synchronized int save(AccountFollowUpRequest r){
        UserAccount u=context.requireAccountingUser();rights.require(u,screen(r.getAccountClass(),r.getReportScreenId()),r.getId()==0?"Save":"Update");
        String token=u.getOrganizationId()+":"+u.getCompanyId()+":"+u.getId()+":"+r.getRequestId();
        if(saved.containsKey(token))return saved.get(token);
        int id=repository.save(u,r);saved.put(token,id);
        if(saved.size()>1000)saved.remove(saved.keySet().iterator().next());return id;
    }
}
