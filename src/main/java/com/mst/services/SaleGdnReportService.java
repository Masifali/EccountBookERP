package com.mst.services;

import com.mst.models.*;
import com.mst.repositories.SaleGdnReportRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class SaleGdnReportService {
    private final SaleGdnReportRepository repo;private final CurrentUserContext context;private final DesktopReportRights rights;
    public SaleGdnReportService(SaleGdnReportRepository repo,CurrentUserContext context,DesktopReportRights rights){this.repo=repo;this.context=context;this.rights=rights;}
    private UserAccount user(){var u=context.requireAccountingUser();rights.require(u,483,"View");return u;}
    public Map<String,Object> initial(){
        var u=user();return Map.of("branches",repo.branches(u),"branchId",u.getBranchesId(),"lookups",repo.lookups(u,""),"yearStart",repo.yearStart(u,context.currentFinancialYearId()));
    }
    public List<Map<String,Object>> lookups(List<Integer> ids){var u=user();return repo.lookups(u,branchFilter(u,ids));}
    public List<Map<String,Object>> history(SaleGdnReportFilter filter){
        var u=user();
        if(filter==null||filter.fromDate()==null||filter.toDate()==null)throw new IllegalArgumentException("From Date and To Date are required");
        return repo.history(u,filter,branchFilter(u,filter.branchIds()));
    }
    private String branchFilter(UserAccount u,List<Integer> ids){
        if(ids==null||ids.isEmpty())throw new IllegalArgumentException("Select Branch First");
        Set<Integer> allowed=repo.branches(u).stream().map(r->((Number)r.get("BranchId")).intValue()).collect(Collectors.toSet());
        if(ids.stream().anyMatch(id->id==null||!allowed.contains(id)))throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(",",",",""));
    }
}
