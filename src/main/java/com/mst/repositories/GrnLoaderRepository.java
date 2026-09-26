package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.sql.Types;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** frmLoadGRN, InvPurchaseInvoice BLL 0581:975-1103 and InvGrn BLL 0576:2684. */
@Repository
public class GrnLoaderRepository {
    private final JdbcTemplate jdbc;private final CurrentUserContext context;private final PurchaseInvoiceRecordRepository rights;private final PurchaseInvoiceWriteRepository config;
    public GrnLoaderRepository(JdbcTemplate jdbc,CurrentUserContext context,PurchaseInvoiceRecordRepository rights,PurchaseInvoiceWriteRepository config){this.jdbc=jdbc;this.context=context;this.rights=rights;this.config=config;}
    private void access(int type){if(type==46)rights.requireRight(56,"View");else if(type==137)rights.requireRight(138,"View");}
    private SqlParameterValue date(String text){try{return new SqlParameterValue(Types.DATE,text==null||text.isBlank()?null:java.sql.Date.valueOf(text));}catch(IllegalArgumentException invalid){throw new IllegalArgumentException("Use a valid GRN filter date");}}
    private SqlParameterValue positive(Integer id){return new SqlParameterValue(Types.INTEGER,id!=null&&id>0?id:null);}
    public List<Map<String,Object>> getUserBranches(int ignoredOrg,int ignoredCompany,int ignoredUser,int type){
        access(type);int org=context.currentOrganizationId(),company=context.currentCompanyId(),user=context.currentUserId();
        if(Boolean.parseBoolean(config.configuration(org,company,"PurchaseInvoiceBranchWise")))return jdbc.queryForList("SELECT Id AS BranchId,BranchName FROM dbo.Branches WHERE Id=? AND OrganizationId=? AND CompanyId=?",context.currentBranchId(),org,company);
        // Original spelling is Branchs. A broad raw Branch fallback previously hid the wrong name.
        var rows=jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromGrn @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=?",org,company,user,positive(type));
        // The existing procedure's unparenthesized OR can return other tenants' allocations.
        // Leave the database object unchanged and enforce this user's allocation here.
        var allocated=new HashSet<>(jdbc.queryForList("SELECT a.BranchId FROM dbo.BranchesAllocationToUser a JOIN dbo.Branches b ON b.Id=a.BranchId AND b.CompanyId=a.CompanyId WHERE a.OrganizationId=? AND a.CompanyId=? AND a.UserId=?",Integer.class,org,company,user));
        return rows.stream().filter(r->allocated.contains(i(copy(r),"BranchId"))).toList();
    }
    private Set<Integer> branches(int type){return getUserBranches(0,0,0,type).stream().map(r->i(copy(r),"BranchId")).collect(Collectors.toCollection(LinkedHashSet::new));}
    private Set<Integer> selectedBranches(String input,int type){
        var allowed=branches(type);if(input==null||input.isBlank())return allowed;
        var selected=new LinkedHashSet<Integer>();for(String token:input.split(",")){if(token.isBlank())continue;int id;try{id=Integer.parseInt(token.trim());}catch(NumberFormatException invalid){throw new IllegalArgumentException("Select valid GRN branches");}if(!allowed.contains(id))throw new ResponseStatusException(FORBIDDEN,"GRN branch is not allocated to this user");selected.add(id);}return selected;
    }
    public List<Map<String,Object>> getPendingGrns(int ignoredOrg,int ignoredCompany,int type,int ignoredYear,String from,String to,String branchIds,Integer supplier,Integer order){
        access(type);var selected=selectedBranches(branchIds,type);var start=date(from);var end=date(to);if(selected.isEmpty())return List.of();
        var rows=jdbc.queryForList("EXEC dbo.usp_getGrnLoaderDataForPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@SupplierCustomerId=?,@FromDate=?,@ToDate=?,@BranchesIds=? WITH RECOMPILE",context.currentOrganizationId(),context.currentCompanyId(),type,context.currentFinancialYearId(),positive(supplier),start,end,selected.stream().map(String::valueOf).collect(Collectors.joining(",")));
        // GoldenAcedb's current procedure has no @OrderId parameter; its result includes this key.
        return rows.stream().filter(r->order==null||order<=0||i(copy(r),"PurchaseOrderId")==order).toList();
    }
    public List<Map<String,Object>> getPendingMarketGrns(int ignoredOrg,int ignoredCompany,int type,String from,String to,Integer supplier){
        access(type);
        // frmLoadGRN.InitializeComponent:765 hides rdMarketGrn. GoldenAcedb also has no
        // GetPendingMarketGrnForPurchaseInvoice activity; do not manufacture an empty result.
        throw new ResponseStatusException(METHOD_NOT_ALLOWED,"The separate Market GRN loader is hidden in the desktop application. Use the regular GRN list.");
    }
    public Map<String,Object> getGrnHeader(int id){
        var rows=jdbc.queryForList("SELECT Id,DocumentTypeId,BranchesId FROM dbo.InvGrn WHERE Id=? AND OrganizationId=? AND CompanyId=? AND FinancialYearId=?",id,context.currentOrganizationId(),context.currentCompanyId(),context.currentFinancialYearId());
        if(rows.isEmpty())throw new ResponseStatusException(NOT_FOUND,"GRN not found in this company and financial year");var scoped=copy(rows.get(0));int type=i(scoped,"DocumentTypeId");access(type);if(!branches(type).contains(i(scoped,"BranchesId")))throw new ResponseStatusException(NOT_FOUND,"GRN not found in your allocated branches");
        return jdbc.queryForMap("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadById'",id);
    }
    public List<Map<String,Object>> getGrnDetails(int id){
        var h=copy(getGrnHeader(id));String activity=switch(i(h,"DocumentTypeId")){case 46,165,167,169,143->"ReadByInvGrnID";case 47->"ReadByInvGrnIDTrading";case 48,701->"ReadByInvGrnIDStore";case 137,217->"ReadByInvGrnIdDirect";case 1602,1618,1858->"ReadByInvGrnID_Engr";default->throw new IllegalArgumentException("Unsupported GRN detail type");};
        return jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity=?",id,activity);
    }
    public List<Map<String,Object>> selectionRows(List<Map<String,Object>> supplied,int type){
        var pending=getPendingGrns(0,0,type,0,null,null,null,null,null);var indexed=new HashMap<Integer,Map<String,Object>>();for(var row:pending)indexed.put(i(copy(row),"Id"),row);
        var result=new ArrayList<Map<String,Object>>();var seen=new HashSet<Integer>();
        for(var raw:supplied){int id=i(copy(raw),"Id");if(!seen.add(id)||!indexed.containsKey(id))throw new IllegalArgumentException("A selected GRN is no longer pending or is outside your allocated branches");result.add(indexed.get(id));}return result;
    }
}
