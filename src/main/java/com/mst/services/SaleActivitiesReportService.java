package com.mst.services;

import com.mst.models.*;
import com.mst.repositories.SaleActivitiesReportRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class SaleActivitiesReportService {
    private final SaleActivitiesReportRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public SaleActivitiesReportService(SaleActivitiesReportRepository repo,CurrentUserContext context,DesktopReportRights rights) {
        this.repo=repo;this.context=context;this.rights=rights;
    }
    private UserAccount user() { var u=context.requireAccountingUser();rights.require(u,repo.screenId(),"View");return u; }
    public Map<String,Object> initial() {
        var u=user();var branches=repo.branches(u);
        int selected=branches.stream().anyMatch(r->Objects.equals(((Number)r.get("BranchId")).intValue(),u.getBranchesId()))
                ?u.getBranchesId():branches.isEmpty()?0:((Number)branches.get(0).get("BranchId")).intValue();
        return Map.of("branches",branches,"branchId",selected,"yearStart",repo.yearStart(u,context.currentFinancialYearId()),
                "activities",SaleActivitiesReportColumns.ACTIVITIES,"packUoms",List.of(1,5,10,20,25,40,50,60,65,80,100),
                "costingByJobOrder",repo.costingByJobOrder(u));
    }
    public List<Map<String,Object>> lookups(List<Integer> ids) { var u=user();return repo.lookups(u,branchFilter(u,ids)); }
    public Map<String,Object> rows(SaleActivitiesReportFilter f) {
        var u=user();
        if(f==null||f.fromDate()==null||f.toDate()==null)throw new IllegalArgumentException("From Date and To Date are required");
        String spec=SaleActivitiesReportColumns.spec(f.activity()),branches=branchFilter(u,f.branchIds());
        String groups="";
        if(f.customGroupIds()!=null&&!f.customGroupIds().isEmpty()) {
            Set<Integer> allowed=repo.lookups(u,branches).stream().filter(r->"GetCustomGroups".equals(r.get("Activity")))
                    .map(r->((Number)r.get("Id")).intValue()).collect(Collectors.toSet());
            if(f.customGroupIds().stream().anyMatch(id->id==null||!allowed.contains(id)))throw new IllegalArgumentException("Select a valid custom group");
            groups=f.customGroupIds().stream().distinct().map(String::valueOf).collect(Collectors.joining(",",",",""));
        }
        var rows=repo.rows(u,f,branches,groups);
        return Map.of("columns",SaleActivitiesReportColumns.names(spec),"rows",rows.stream().map(r->SaleActivitiesReportColumns.project(r,spec)).toList());
    }
    private String branchFilter(UserAccount u,List<Integer> ids) {
        if(ids==null||ids.isEmpty())throw new IllegalArgumentException("Select Branch First");
        Set<Integer> allowed=repo.branches(u).stream().map(r->((Number)r.get("BranchId")).intValue()).collect(Collectors.toSet());
        if(ids.stream().anyMatch(id->id==null||!allowed.contains(id)))throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(",",",",""));
    }
}
