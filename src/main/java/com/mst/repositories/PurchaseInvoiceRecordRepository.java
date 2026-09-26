package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.sql.Types;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.*;

/** InvPurchaseInvoice BLL 0581 FormHistory/GetByID and DAL 0434 GetDate. */
@Repository
public class PurchaseInvoiceRecordRepository {
    private static final Map<Integer,String> SCREENS=Map.of(56,"InvfrmPurchaseInvoice",57,"InvfrmPurchasedirectInvoice",59,"InvfrmInvPurchaseInvoiceReturn",61,"frmPurchaseInvoiceDirectStore",138,"frmPurchaseInvoiceAgaintGrnDirect");
    private static final Map<Integer,String> ROUTES=Map.of(56,"/purchase/purchase-invoice",57,"/purchase/purchase-invoice-direct",59,"/purchase/purchase-invoice-return",61,"/purchase/purchase-invoice-store-management",138,"/purchase/purchase-invoice-again-grn-direct");
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public PurchaseInvoiceRecordRepository(JdbcTemplate jdbc,CurrentUserContext context) {this.jdbc=jdbc;this.context=context;}
    private String screen(int type) {if(!SCREENS.containsKey(type))throw new ResponseStatusException(BAD_REQUEST,"Unsupported purchase invoice type");return SCREENS.get(type);}
    public boolean hasRight(int type,String right) {
        String name=screen(type),role=Objects.toString(context.currentRoleName(),"");
        if(("Admin".equalsIgnoreCase(role)||"Administrator".equalsIgnoreCase(role))&&!"Delete".equals(right)&&!"View".equals(right))return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,@RightName=?,@CompanyId=?,@Activity='GetByUserId'",context.currentUserId(),name,role,context.currentCompanyId()).stream()
                .anyMatch(row->right.equalsIgnoreCase(Objects.toString(row.get("RightName"),"").trim())&&(Boolean.TRUE.equals(row.get("Value"))||"1".equals(Objects.toString(row.get("Value")))));
    }
    public void requireRight(int type,String right) {if(!hasRight(type,right))throw new ResponseStatusException(FORBIDDEN,"You do not have "+right+" permission for this purchase invoice screen");}
    public void scopeWrite(Map<String,Object> payload,int type) {
        int id=number(payload.get("id"));requireRight(type,id>0?"Update":"Save");
        if(id>0)require(id,type);
        payload.put("documentTypeId",type);payload.put("organizationId",context.currentOrganizationId());payload.put("companyId",context.currentCompanyId());
        payload.put("branchesId",context.currentBranchId());payload.put("financialYearId",context.currentFinancialYearId());payload.put("entryUser",context.currentUserId());
    }
    public Map<String,Object> require(int id,int type) {
        screen(type);
        var rows=jdbc.queryForList("SELECT * FROM dbo.InvPurchaseInvoice WHERE Id=? AND DocumentTypeId=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=?",id,type,context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
        if(rows.isEmpty())throw new ResponseStatusException(NOT_FOUND,"Invoice not found in your current company, branch and financial year");
        requireRight(type,"View");var row=rows.get(0);
        if(number(row.get("EntryUser"))!=context.currentUserId()&&!hasRight(type,"CanView AllRecord"))throw new ResponseStatusException(NOT_FOUND,"Invoice not found in your accessible records");
        return row;
    }
    public List<Map<String,Object>> history(int type,String fromDate,String toDate,Integer supplier,Integer fromNumber,Integer toNumber,String dateType) {
        requireRight(type,"View");String mode=Objects.toString(dateType,"").toLowerCase(Locale.ROOT);
        String prefix="entrydate".equals(mode)?"Entry":"modifydate".equals(mode)?"Modify":"approveddate".equals(mode)?"Approved":"";
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesIds=?,@CanViewAllRecord=?,@EntryUser=?,@"+prefix+"FromDate=?,@"+prefix+"ToDate=?,@SupplierCustomerId=?,@FromDocNo=?,@ToDocNo=?,@Activity='FormHistory' WITH RECOMPILE",
                context.currentOrganizationId(),context.currentCompanyId(),type,context.currentFinancialYearId(),Integer.toString(context.currentBranchId()),hasRight(type,"CanView AllRecord"),context.currentUserId(),date(fromDate),date(toDate),positive(supplier),positive(fromNumber),positive(toNumber));
        List<Map<String,Object>> mapped=new ArrayList<>();
        for(var row:rows) {
            var result=aliases(row);result.put("remarks",row.get("RemarksHeader"));
            result.put("recordUrl",ROUTES.get(type)+"?id="+number(row.get("Id")));mapped.add(result);
        }
        return mapped;
    }
    public Map<String,Object> load(int id,int type) {
        require(id,type);
        var result=aliases(jdbc.queryForMap("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById' WITH RECOMPILE",id));
        result.put("supplierName",result.get("SupplierCustomerName"));
        if(type==61)result.put("details",jdbc.queryForList("EXEC dbo.USP_InvPurchaseInvoiceDetail_ReadById @Id=?",id));
        else result.put("details",read(id,type==56?"PurchaseDetailReadByInvPurchaseInvoiceId":"DirectPurchaseDetailReadByInvPurchaseInvoiceId"));
        result.put("expenses",read(id,"InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
        result.put("freight",read(id,"InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
        result.put("journal",read(id,"InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
        result.put("emptyBags",read(id,"InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID"));
        result.put("paymentTerms",read(id,"InvPurchaseInvoicePaymentTerm_ReadByPurchaseInvoiceID"));
        result.put("paymentDues",jdbc.queryForList("EXEC dbo.Sp_PaymentDueSchedule_ReadAll @DocumentTypeId=?,@RefDocNoId=?",type,id));
        result.put("recordUrl",ROUTES.get(type)+"?id="+id);
        return result;
    }
    @org.springframework.transaction.annotation.Transactional
    public void delete(int id,int type) {
        if(type==59)throw new ResponseStatusException(METHOD_NOT_ALLOWED,"The desktop Purchase Invoice Return form has no header Delete action");
        require(id,type);requireRight(type,"Delete");
        com.mst.repositories.support.ProcExec.run(jdbc,"EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=?,@UserId=?",context.currentOrganizationId(),context.currentCompanyId(),id,type,context.currentUserId());
        if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvPurchaseInvoice WHERE Id=?",Integer.class,id)!=0)throw new IllegalStateException("The desktop procedure did not delete the invoice");
    }
    private List<Map<String,Object>> read(int id,String activity) {return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=? WITH RECOMPILE",id,activity);}
    private Map<String,Object> aliases(Map<String,Object> row) {
        Map<String,Object> result=new LinkedHashMap<>(row);
        for(var entry:row.entrySet())result.put(Character.toLowerCase(entry.getKey().charAt(0))+entry.getKey().substring(1),entry.getValue());
        for(String key:List.of("docDate","entryDate","dueDate"))if(result.get(key)!=null)result.put(key,result.get(key).toString().substring(0,10));
        return result;
    }
    private static int number(Object value) {return value instanceof Number?((Number)value).intValue():0;}
    private static SqlParameterValue positive(Integer value) {return new SqlParameterValue(Types.INTEGER,value!=null&&value>0?value:null);}
    private static SqlParameterValue date(String value) {try{return new SqlParameterValue(Types.DATE,value==null||value.isBlank()?null:java.sql.Date.valueOf(value));}catch(IllegalArgumentException invalid){throw new ResponseStatusException(BAD_REQUEST,"Use a valid invoice filter date",invalid);}}
}
