package com.mst.services;

import com.mst.models.*;
import com.mst.repositories.SaleOutwardReportRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class SaleOutwardReportService {
    private final SaleOutwardReportRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public SaleOutwardReportService(SaleOutwardReportRepository repo, CurrentUserContext context, DesktopReportRights rights) {
        this.repo=repo; this.context=context; this.rights=rights;
    }
    private UserAccount user() { var u=context.requireAccountingUser(); rights.require(u,repo.screenId(),"View"); return u; }
    public Map<String,Object> initial() {
        var u=user(); var branches=repo.branches(u);
        boolean current=branches.stream().anyMatch(r->Objects.equals(((Number)r.get("BranchId")).intValue(),u.getBranchesId()));
        return Map.of("branches",branches,"branchId",current?u.getBranchesId():0,
                "yearStart",repo.yearStart(u,context.currentFinancialYearId()),"columns",SaleOutwardReportColumns.names());
    }
    public List<Map<String,Object>> lookups(List<Integer> ids) { var u=user(); return repo.lookups(u,branchFilter(u,ids)); }
    public List<Map<String,Object>> rows(SaleOutwardReportFilter f) {
        var u=user();
        if(f==null||f.fromDate()==null||f.toDate()==null)throw new IllegalArgumentException("From Date and To Date are required");
        if(f.status()!=null&&!Set.of("","Open","Accepted","Rejected").contains(f.status()))throw new IllegalArgumentException("Select a valid gate pass status");
        return repo.rows(u,f,branchFilter(u,f.branchIds())).stream().map(SaleOutwardReportColumns::project).toList();
    }
    private String branchFilter(UserAccount u,List<Integer> ids) {
        if(ids==null||ids.isEmpty())throw new IllegalArgumentException("Select Branch First");
        Set<Integer> allowed=repo.branches(u).stream().map(r->((Number)r.get("BranchId")).intValue()).collect(Collectors.toSet());
        if(ids.stream().anyMatch(id->id==null||!allowed.contains(id)))throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(",",",",""));
    }
}
