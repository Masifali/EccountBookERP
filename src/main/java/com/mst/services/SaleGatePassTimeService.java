package com.mst.services;

import com.mst.models.SaleGatePassTimeFilter;
import com.mst.models.UserAccount;
import com.mst.repositories.SaleGatePassTimeRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class SaleGatePassTimeService {
    private final SaleGatePassTimeRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public SaleGatePassTimeService(SaleGatePassTimeRepository repo,CurrentUserContext context,DesktopReportRights rights){
        this.repo=repo;this.context=context;this.rights=rights;
    }
    private UserAccount user(){
        var u=context.requireAccountingUser();rights.require(u,repo.screenId(),"View");return u;
    }
    public Map<String,Object> initial(){
        var u=user();var branches=repo.branches(u);
        boolean currentAllowed=branches.stream().anyMatch(r->SaleInvoiceRepository.i(r.get("BranchId"))==u.getBranchesId());
        return Map.of("branches",branches,"branchId",currentAllowed?u.getBranchesId():0,
                "yearStart",repo.yearStart(u,context.currentFinancialYearId()));
    }
    public List<Map<String,Object>> lookups(List<Integer> branchIds){
        var u=user();return repo.lookups(u,branchFilter(u,branchIds));
    }
    public Map<String,Object> rows(SaleGatePassTimeFilter filter){
        var u=user();
        if(filter==null||filter.fromDate()==null||filter.toDate()==null)throw new IllegalArgumentException("From Date and To Date are required");
        if(filter.status()!=null&&!Set.of("","Open","Accepted,Rejected").contains(filter.status()))
            throw new IllegalArgumentException("Select a valid gate pass status");
        String branches=branchFilter(u,filter.branchIds());int year=context.currentFinancialYearId();
        var detail=repo.analysis(u,year,filter,branches);
        var summary=repo.summary(u,year,filter,branches);
        return Map.of("rows",detail,"inward",summary.stream().filter(SaleGatePassTimeService::inward).toList(),
                "outward",summary.stream().filter(r->!inward(r)).toList());
    }
    private String branchFilter(UserAccount u,List<Integer> selected){
        if(selected==null||selected.isEmpty())throw new IllegalArgumentException("Select Branch First");
        Set<Integer> allowed=repo.branches(u).stream().map(r->SaleInvoiceRepository.i(r.get("BranchId"))).collect(Collectors.toSet());
        if(selected.stream().anyMatch(id->id==null||!allowed.contains(id)))
            throw new AccessDeniedException("The selected branch is not allocated to this user");
        return selected.stream().distinct().map(String::valueOf).collect(Collectors.joining(",",",",""));
    }
    private static boolean inward(Map<String,Object> row){
        int type=SaleInvoiceRepository.i(row.get("DocumentTypeId"));return type==51||type==52;
    }
}
