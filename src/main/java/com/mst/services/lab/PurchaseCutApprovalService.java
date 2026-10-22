package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.lab.PurchaseAnalysisRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/** Dashboard 301: LabPurchaseAnalysisForApproval.WeightApprovalPurchaseOrder. */
@Service
public class PurchaseCutApprovalService {
    public record Cut(int id, double deductionRate, double deductionWeight, int weightCutOnId,
                      double weightCutUom, double qtyForWtCut) { }
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final PurchaseAnalysisRepository analysis;
    public PurchaseCutApprovalService(JdbcTemplate jdbc, CurrentUserContext context, PurchaseAnalysisRepository analysis){
        this.jdbc=jdbc;this.context=context;this.analysis=analysis;
    }
    private UserAccount user(){
        var u=context.requireAccountingUser();
        Integer count=jdbc.queryForObject("SELECT COUNT(*) FROM dbo.tblUserRights r "
            + "JOIN dbo.ScreenDefinition s ON s.Id=r.ScreenId JOIN dbo.ScreenRights sr ON sr.Id=r.RightId "
            + "WHERE r.CompanyId=? AND r.UserId=? AND s.ScreenName='PendingVouchersForApproval' "
            + "AND sr.RightName='LabPurchaseAnalysis' AND r.Value=1",Integer.class,u.getCompanyId(),u.getId());
        if(count==null||count==0)throw new AccessDeniedException("You do not have Purchase Analysis approval rights");
        return u;
    }
    private List<Map<String,Object>> pending(UserAccount u){
        var p=DesktopProc.params("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"ActionId",2);
        if(context.currentFinancialYearId()>0)p.put("FinancialYearId",context.currentFinancialYearId());
        if(u.getBranchesId()!=null&&u.getBranchesId()>0)p.put("BranchesId",u.getBranchesId());
        var rows=DesktopProc.rows(jdbc,"Usp_LabPurchasenAalysisDataForApproval",p);
        rows.forEach(r->r.entrySet().removeIf(e->e.getValue() instanceof byte[]));
        return rows;
    }
    private void member(UserAccount u,int id){
        if(pending(u).stream().noneMatch(r->number(r.get("Id"))==id))
            throw new AccessDeniedException("Record is no longer pending in your company and branch; refresh the list");
    }
    public Map<String,Object> list(){var u=user();return Map.of("rows",pending(u),"weightCutOn",analysis.weightCutOn());}
    public Map<String,Object> record(int id){var u=user();member(u,id);var h=analysis.header(id);
        if(h==null)throw new IllegalArgumentException("Analysis not found");
        var out=new LinkedHashMap<String,Object>();
        for(String k:List.of("Id","DocNo","DeductionRate","DeductionWeight","WeightCutOnId","WeightCutUom","QtyForWtCut","IsWeightCutCompulsory","IsRateCutCompulsory"))out.put(k,h.get(k));
        return out;
    }
    @Transactional
    public Map<String,Object> approve(Cut cut){
        if(cut==null||cut.id()<=0)throw new IllegalArgumentException("Select an analysis first");
        for(double value:new double[]{cut.deductionRate(),cut.deductionWeight(),cut.weightCutUom(),cut.qtyForWtCut()})
            if(!Double.isFinite(value))throw new IllegalArgumentException("Enter valid numeric cuts");
        var u=user();member(u,cut.id());
        var locked=jdbc.queryForList("SELECT h.Id,h.WarehouseId,h.IsRateCutCompulsory,h.IsWeightCutCompulsory "
            + "FROM dbo.InvLabAnalysisPurchaseHeader h WITH (UPDLOCK,HOLDLOCK) "
            + "JOIN dbo.GatePassInward gp ON gp.Id=h.GatePassInwardId "
            + "WHERE h.Id=? AND h.OrganizationId=? AND h.CompanyId=? AND h.DocumentTypeId=303 "
            + "AND h.IsApproved=1 AND ISNULL(h.WeightApplied,0)=0 AND gp.RefDocumentTypeId=41 "
            + "AND (?=0 OR h.BranchesId=?) AND (?=0 OR h.FinancialYearId=?)",
            cut.id(),u.getOrganizationId(),u.getCompanyId(),Objects.requireNonNullElse(u.getBranchesId(),0),
            Objects.requireNonNullElse(u.getBranchesId(),0),context.currentFinancialYearId(),context.currentFinancialYearId());
        if(locked.size()!=1)throw new AccessDeniedException("Record changed; refresh the pending analyses");
        var h=locked.get(0);
        if(number(h.get("WarehouseId"))<=0)throw new IllegalArgumentException("Warehouse field required; update the analysis first");
        if(cut.deductionWeight()>0&&analysis.weightCutOn().stream().noneMatch(r->number(r.get("Id"))==cut.weightCutOnId()))
            throw new IllegalArgumentException("Weight Cut On field required");
        boolean weightLater=cut.deductionWeight()==0&&truth(h.get("IsWeightCutCompulsory"));
        boolean rateLater=cut.deductionRate()==0&&truth(h.get("IsRateCutCompulsory"));
        if(weightLater||rateLater)throw new IllegalArgumentException("Enter the required deferred rate/weight cut before approval");
        DesktopProc.scalar(jdbc,"USP_InvLabAnalysisPurchaseHeader_UpdateDuringApproval",DesktopProc.params(
            "Id",cut.id(),"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),
            "BranchesId",u.getBranchesId(),"FinancialYearId",context.currentFinancialYearId(),"DocumentTypeId",303,
            "ModifyUser",u.getId(),"ApprovedUserId",u.getId(),"ActionId",4,
            "DeductionRate",cut.deductionRate(),"DeductionWeight",cut.deductionWeight(),
            "WeightCutOnId",cut.weightCutOnId(),"WeightCutUom",cut.weightCutUom(),"QtyForWtCut",cut.qtyForWtCut(),
            "IsWeightCutCompulsory",false,"IsRateCutCompulsory",false,
            "WeightApplied",true,"WeightAppliedFromId",4,"WeightAppliedUserId",u.getId()));
        return Map.of("message","Rate/Weight Cut Approved Successfully");
    }
    private static int number(Object v){return v instanceof Number?((Number)v).intValue():0;}
    private static boolean truth(Object v){return Boolean.TRUE.equals(v)||"1".equals(String.valueOf(v));}
}
