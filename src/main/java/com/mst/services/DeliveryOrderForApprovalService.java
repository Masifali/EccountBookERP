package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/** DeliveryOrderForApproval: local sales DO (dashboard type 37, document 84). */
@Service
public class DeliveryOrderForApprovalService {
    public record Filter(LocalDate fromDate, LocalDate toDate, String status) { }
    public record Approval(Filter filter, List<Integer> ids) { }
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public DeliveryOrderForApprovalService(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc=jdbc; this.context=context;
    }
    private UserAccount user() {
        var u=context.requireAccountingUser();
        Integer count=jdbc.queryForObject("SELECT COUNT(*) FROM dbo.tblUserRights r "
            + "JOIN dbo.ScreenDefinition s ON s.Id=r.ScreenId JOIN dbo.ScreenRights sr ON sr.Id=r.RightId "
            + "WHERE r.CompanyId=? AND r.UserId=? AND s.ScreenName='PendingVouchersForApproval' "
            + "AND sr.RightName='DeliveryOrder' AND r.Value=1",Integer.class,u.getCompanyId(),u.getId());
        if(count==null||count==0)throw new AccessDeniedException("You do not have Delivery Order approval rights");
        return u;
    }
    private List<Map<String,Object>> rows(UserAccount u, Filter f) {
        if(f==null||!Set.of("Not Approved","Approved","All").contains(Objects.toString(f.status(),"")))
            throw new IllegalArgumentException("Select an approval status");
        if(f.fromDate()!=null&&f.toDate()!=null&&f.fromDate().isAfter(f.toDate()))
            throw new IllegalArgumentException("From Date must not be after To Date");
        var p=DesktopProc.params("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),
                "DocumentTypeId",84,"DeliveryOrderType","Local");
        if(u.getBranchesId()!=null&&u.getBranchesId()>0)p.put("BranchesId",u.getBranchesId());
        if(f.fromDate()!=null)p.put("FromDate",Date.valueOf(f.fromDate()));
        if(f.toDate()!=null)p.put("ToDate",Date.valueOf(f.toDate()));
        if(!"All".equals(f.status()))p.put("IsApproved","Approved".equals(f.status()));
        var result=DesktopProc.rows(jdbc,"Sp_InvDeliveryOrderForApproval_Rpt",p);
        // Report results include binary company logos; they are not grid cells.
        result.forEach(r->r.entrySet().removeIf(e->e.getValue() instanceof byte[]));
        return result;
    }
    public Map<String,Object> history(Filter f) {return Map.of("rows",rows(user(),f));}
    @Transactional
    public Map<String,Object> approve(Approval request) {
        var u=user();
        if(request==null||request.filter()==null||!"Not Approved".equals(request.filter().status()))
            throw new IllegalArgumentException("Only Not Approved records can be approved");
        if(request.ids()==null||request.ids().isEmpty())throw new IllegalArgumentException("Select a delivery order first");
        var ids=new LinkedHashSet<>(request.ids());
        if(ids.size()!=request.ids().size()||ids.stream().anyMatch(id->id==null||id<=0))
            throw new IllegalArgumentException("Invalid delivery order selection");
        Set<Integer> available=new HashSet<>();
        for(var row:rows(u,request.filter()))available.add(((Number)row.get("Id")).intValue());
        if(!available.containsAll(ids))throw new AccessDeniedException("Selection changed; refresh the pending orders");
        for(int id:ids){
            var locked=jdbc.queryForList("SELECT Id,IsApproved,BranchesId FROM dbo.InvDeliveryOrder WITH (UPDLOCK,HOLDLOCK) "
                + "WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=84 AND DeliveryOrderType='Local'",
                id,u.getOrganizationId(),u.getCompanyId());
            if(locked.size()!=1)throw new AccessDeniedException("Delivery order is unavailable");
            var row=locked.get(0);
            if(Boolean.TRUE.equals(row.get("IsApproved"))||"1".equals(String.valueOf(row.get("IsApproved"))))
                throw new IllegalArgumentException("This delivery order has already been approved; refresh the list");
            if(u.getBranchesId()!=null&&u.getBranchesId()>0&&!Objects.equals(u.getBranchesId(),row.get("BranchesId")))
                throw new AccessDeniedException("Delivery order belongs to another branch");
        }
        for(int id:ids)DesktopProc.rows(jdbc,"Sp_InvDeliveryOrder_GetAllMethod",DesktopProc.params(
            "OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"EntryUser",u.getId(),
            "Id",id,"ReqType","Approve","Activity","ApprovedRecordByDeliveryOrderId"));
        return Map.of("message","Record Approved Successfully","count",ids.size());
    }
}
