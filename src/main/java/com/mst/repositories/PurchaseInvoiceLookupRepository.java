package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** InvfrmPurchaseInvoice:629-1124. These lists belong to type 56, not Direct Invoice. */
@Repository
public class PurchaseInvoiceLookupRepository {
    private final JdbcTemplate jdbc; private final CurrentUserContext context;
    private final PurchaseInvoiceRecordRepository records; private final PurchaseInvoiceWriteRepository writes;
    public PurchaseInvoiceLookupRepository(JdbcTemplate jdbc,CurrentUserContext context,PurchaseInvoiceRecordRepository records,PurchaseInvoiceWriteRepository writes){this.jdbc=jdbc;this.context=context;this.records=records;this.writes=writes;}
    public String configuration(String name){return writes.configuration(context.currentOrganizationId(),context.currentCompanyId(),name);}
    public boolean enabled(String name){return Boolean.parseBoolean(configuration(name));}
    public int amountDigits(){String v=configuration("Default NoofDecimal Points For Amount");int digits=v.isBlank()?0:Integer.parseInt(v);if(digits<0||digits>15)throw new IllegalStateException("Invalid amount decimal configuration");return digits;}
    public boolean subsidiary(){return feature(4);}
    /** CommonServices.GetERPFeatureById (DAL 0243:218-236). 4 = subsidiary accounts, 11 = branch feature. */
    public boolean feature(int id){return jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",context.currentOrganizationId(),context.currentCompanyId()).stream().anyMatch(r->i(copy(r),"Id")==id);}
    /** InvfrmPurchaseInvoice.HistoryCombosBranchDbCall:803-826 - own branch when PurchaseInvoiceBranchWise, else BLL 0581:3121. */
    public List<Map<String,Object>> historyBranches(){
        int org=context.currentOrganizationId(),company=context.currentCompanyId();
        if(enabled("PurchaseInvoiceBranchWise"))return jdbc.queryForList("SELECT Id AS BranchId,BranchName FROM dbo.Branches WHERE Id=? AND OrganizationId=? AND CompanyId=?",context.currentBranchId(),org,company);
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromPurchaseInvoice @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=?",org,company,context.currentUserId(),56);
    }
    /** HistoryComboDbCall:855-904 -> BLL 0581:2172 Usp_AllComboAgainstPurchaseInvoice, Activity "Supplier" (Id, ReferenceName). */
    public List<Map<String,Object>> historySuppliers(String branchIds){
        return jdbc.queryForList("EXEC dbo.Usp_AllComboAgainstPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?,@Activity=?,@BranchesIds=?",context.currentOrganizationId(),context.currentCompanyId(),"56","Supplier",branchIds);
    }
    /** Conversion.ToInt: a blank or non-numeric configuration value reads as 0. */
    private static int toInt(String v){try{return v==null||v.isBlank()?0:(int)Math.rint(Double.parseDouble(v.trim()));}catch(NumberFormatException notNumeric){return 0;}}
    private String financialYearStart(){
        for(var r:jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?,@CompanyId=?",context.currentOrganizationId(),context.currentCompanyId())){var c=copy(r);if(i(c,"Id")==context.currentFinancialYearId()&&c.get("Start_Period")!=null)return c.get("Start_Period").toString().substring(0,10);}
        return "";
    }
    public Map<Integer,List<Map<String,Object>>> rateUoms(List<Map<String,Object>> details){
        records.requireRight(56,"View");var result=new LinkedHashMap<Integer,List<Map<String,Object>>>();
        for(var raw:details){int item=i(copy(raw),"ItemId");result.computeIfAbsent(item,key->jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'",context.currentOrganizationId(),context.currentCompanyId(),key));}
        return result;
    }
    public Map<String,Object> all(){
        records.requireRight(56,"View");int org=context.currentOrganizationId(),company=context.currentCompanyId();boolean subsidiary=subsidiary();
        var result=new LinkedHashMap<String,Object>();var rights=new LinkedHashMap<String,Boolean>();for(String right:List.of("View","Save","Update","Delete","Print"))rights.put(right,records.hasRight(56,right));result.put("rights",rights);
        var suppliers=jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?,@CompanyId=?",org,company);
        suppliers.removeIf(raw->{var r=copy(raw);return i(r,"CustomerGroupId")==7||(subsidiary&&i(r,"PartyTypeId")!=1);});result.put("suppliers",suppliers);
        result.put("paymentTerms",jdbc.queryForList("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",org,company));
        result.put("locations",jdbc.queryForList("EXEC dbo.usp_getLocationType"));
        result.put("otherItems",jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",org,company));
        result.put("commissionUoms",jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='GetCommissionUom'"));
        var accounts=jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?,@CompanyId=?",org,company);
        var unique=new LinkedHashMap<Integer,Map<String,Object>>();for(var r:accounts)unique.putIfAbsent(i(copy(r),"ChartOfAccountId"),r);
        result.put("bagCreditAccounts",unique.values().stream().filter(r->Set.of(8,10).contains(i(copy(r),"AccountTypeId"))).toList());
        if(subsidiary){
            var transporters=jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?,@CompanyId=?",org,company);
            result.put("accounts",transporters);result.put("freightAccounts",transporters);
        }else{
            var excluded=enabled("DebitAmountChargetoExpenseAcFreightGridPurchase")?Set.of(2,15,22):Set.of(2,11,12,13,14,15,20,21,22);
            result.put("accounts",unique.values().stream().filter(r->!excluded.contains(i(copy(r),"AccountTypeId"))).toList());
            result.put("freightAccounts",unique.values().stream().filter(r->!Set.of(2,15,22).contains(i(copy(r),"AccountTypeId"))).toList());
        }
        var flags=new LinkedHashMap<String,Boolean>();for(String name:List.of("ContractWagesChargetoProduct","WagesAmountCalculateOnQty","DebitAmountChargetoExpenseAcFreightGridPurchase","RateEditableOnPurchaseInvoice_InGatePurchase","WeightAddLessOnPurchaseInvoice","PurchaseInvoiceBranchWise","ValidateGrnAndInvoiceDateWithGpDate"))flags.put(name,enabled(name));
        result.put("configuration",flags);result.put("subsidiary",subsidiary);result.put("amountDigits",amountDigits());
        // ImplementConfiguration:947-958, frmLoadGRN_Load:107, InitializeComponentMethod:743 (history From date).
        result.put("branchFeature",feature(11));result.put("currentBranchId",context.currentBranchId());result.put("financialYearStart",financialYearStart());
        result.put("historyFromDays",toInt(configuration("DefaultDaysToLessFromHistoryFromDate")));
        var branches=historyBranches();result.put("historyBranches",branches);
        // At load the branch combo is still empty, so HistoryComboDbCall(ValidateBranch:false) sends the user's branch (:890).
        result.put("historySuppliers",historySuppliers(String.valueOf(context.currentBranchId())));
        return result;
    }
}
