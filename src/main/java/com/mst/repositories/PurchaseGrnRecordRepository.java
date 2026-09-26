package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import java.sql.Types;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.*;

/** Record scope and original history/deletion contracts for InvFrmGRN and SaleReturnGrn. */
@Repository
public class PurchaseGrnRecordRepository {
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public PurchaseGrnRecordRepository(JdbcTemplate jdbc,CurrentUserContext context) { this.jdbc=jdbc; this.context=context; }

    public Map<String,Object> require(int id,int type) {
        screen(type);
        var rows=jdbc.queryForList("SELECT * FROM dbo.InvGrn WHERE Id=? AND DocumentTypeId=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=?",
                id,type,context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
        if(rows.isEmpty()) throw new ResponseStatusException(NOT_FOUND,"GRN not found in your current company, branch and financial year");
        requireRight(type,"View");
        var row=rows.get(0);
        if(number(row.get("EntryUser"))!=context.currentUserId() && !canViewAll(type))
            throw new ResponseStatusException(NOT_FOUND,"GRN not found in your accessible records");
        return row;
    }

    public boolean canViewAll(int type) {
        return hasRight(type,"CanView AllRecord");
    }

    public void requireRight(int type,String right) {
        if(!hasRight(type,right))throw new ResponseStatusException(FORBIDDEN,"You do not have "+right+" permission for this GRN screen");
    }

    public boolean hasRight(int type,String right) {
        String role=Objects.toString(context.currentRoleName(),"");
        boolean admin="Admin".equalsIgnoreCase(role)||"Administrator".equalsIgnoreCase(role);
        if(admin && !"Delete".equals(right) && !"View".equals(right))return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                context.currentUserId(),screen(type),role,context.currentCompanyId()).stream()
                .anyMatch(r->right.equalsIgnoreCase(Objects.toString(r.get("RightName"),"").trim())
                        && (Boolean.TRUE.equals(r.get("Value"))||"1".equals(Objects.toString(r.get("Value"),""))));
    }

    public List<Map<String,Object>> history(int type,String fromDate,String toDate,Integer supplier,Integer fromNumber,Integer toNumber,String dateType) {
        requireRight(type,"View");
        String mode=Objects.toString(dateType,"").toLowerCase(Locale.ROOT);
        String from="entrydate".equals(mode)?"EntryFromDate":"modifydate".equals(mode)?"ModifyFromDate":"approveddate".equals(mode)?"ApprovedFromDate":"fromDate";
        String to="entrydate".equals(mode)?"EntryToDate":"modifydate".equals(mode)?"ModifyToDate":"approveddate".equals(mode)?"ApprovedToDate":"toDate";
        boolean viewAll=canViewAll(type);
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=?,@CanViewAllRecord=?,@EntryUser=?,@"+from+"=?,@"+to+"=?,@GrnNoF=?,@GrnNoT=?,@SupplierCustomerId=?,@Activity='GRNFormHistory' WITH RECOMPILE",
                context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId(),type,
                viewAll,context.currentUserId(),date(fromDate),date(toDate),positive(fromNumber),positive(toNumber),positive(supplier));
        List<Map<String,Object>> result=new ArrayList<>();
        for(var row:rows) {
            Map<String,Object> mapped=new LinkedHashMap<>(row);
            for(var entry:row.entrySet()) mapped.put(Character.toLowerCase(entry.getKey().charAt(0))+entry.getKey().substring(1),entry.getValue());
            mapped.put("remarks",row.get("RemarksHeader"));
            for(String field:List.of("docDate","entryDate")) if(mapped.get(field)!=null)mapped.put(field,mapped.get(field).toString().substring(0,10));
            result.add(mapped);
        }
        return result;
    }

    public Map<String,Object> load(int id,int type) {
        require(id,type);
        var headers=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadById'",id);
        if(headers.isEmpty())throw new ResponseStatusException(NOT_FOUND,"GRN not found");
        Map<String,Object> result=new LinkedHashMap<>(headers.get(0));
        result.put("details",jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity='ReadByInvGrnID'",id));
        result.put("emptyBags",jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity='ReadByInvGrnIdEmptyBagsDetail'",id));
        result.put("purchaseBreakups",type==46?jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadByHeaderId_PurchaseBreakup'",id):List.of());
        if(type==46)result.putAll(breakupContext(number(result.get("InwardGatePassId")),id));
        return result;
    }

    public Map<String,Object> breakupContext(int gatePassId,int editingId) {
        if(gatePassId<=0)return Map.of("breakupLocked",false,"referenceType",0,"orderCategoryId",0);
        var gates=jdbc.queryForList("SELECT gp.RefDocumentTypeId,po.OrderCatagoryId FROM dbo.GatePassInward gp LEFT JOIN dbo.PurchaseOrder po ON po.Id=gp.PurchaseOrderId AND po.OrganizationId=gp.OrganizationId AND po.CompanyId=gp.CompanyId WHERE gp.Id=? AND gp.OrganizationId=? AND gp.CompanyId=? AND gp.BranchesId=? AND gp.FinancialYearId=? AND gp.DocumentTypeId=51",
                gatePassId,context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
        if(gates.isEmpty())throw new ResponseStatusException(NOT_FOUND,"Gate pass not found in the current company, branch and financial year");
        var count=jdbc.queryForMap("EXEC dbo.Sp_InvGrn_GetAllMethod @OrganizationId=?,@CompanyId=?,@GatePassInwardId=?,@Activity='GetGrnCountAgainstGatepass'",
                context.currentOrganizationId(),context.currentCompanyId(),gatePassId);
        return Map.of("breakupLocked",number(count.get("GRNCount"))>(editingId>0?1:0),
                "referenceType",number(gates.get(0).get("RefDocumentTypeId")),"orderCategoryId",number(gates.get(0).get("OrderCatagoryId")));
    }

    public void delete(int id,int type) {
        require(id,type);
        requireRight(type,"Delete");
        // The original procedure checks invoice references and archives/removes linked stock data.
        ProcExec.run(jdbc,"EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=?,@UserId=?",
                context.currentOrganizationId(),context.currentCompanyId(),id,type,context.currentUserId());
        if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn WHERE Id=?",Integer.class,id)!=0)
            throw new IllegalStateException("The desktop procedure did not delete the GRN");
    }

    private static String screen(int type) {
        if(type==46)return "InvFrmGRN";
        if(type==143)return "SaleReturnGrn";
        throw new ResponseStatusException(BAD_REQUEST,"This route supports GRN or Sale Return GRN only");
    }
    private static int number(Object v) { return v instanceof Number?((Number)v).intValue():0; }
    private static SqlParameterValue positive(Integer v) { return new SqlParameterValue(Types.INTEGER,v!=null && v>0?v:null); }
    private static SqlParameterValue date(String v) { return new SqlParameterValue(Types.DATE,v==null||v.isBlank()?null:java.sql.Date.valueOf(v)); }
}
