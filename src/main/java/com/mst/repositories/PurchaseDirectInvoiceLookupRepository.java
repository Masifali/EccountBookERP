package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/**
 * InvfrmPurchasedirectInvoice (screen 117, DocumentTypeId 57) lookups: InitializeComponentMethod :714-813 and the
 * binders it calls (SupplierDtFillFromGlobal :1050 ... PackingTypeDtFillFromGlobalAndBind :1383), the grid value lists
 * (AccountsDtFillFromGlobal :1180, OtherItemdtDbCall :1415, EBComboBind :1765), GetConfigurationsFromGlobal :992 and the
 * History combos (HistoryBranchdtFillDBCall :869, HistoryComboDbCall :923). Only the Direct form uses this class.
 */
@Repository
public class PurchaseDirectInvoiceLookupRepository {
    private static final int TYPE = 57;
    private final JdbcTemplate jdbc; private final CurrentUserContext context;
    private final PurchaseInvoiceRecordRepository records; private final PurchaseInvoiceWriteRepository writes;
    public PurchaseDirectInvoiceLookupRepository(JdbcTemplate jdbc,CurrentUserContext context,PurchaseInvoiceRecordRepository records,PurchaseInvoiceWriteRepository writes){this.jdbc=jdbc;this.context=context;this.records=records;this.writes=writes;}
    public String configuration(String name){return writes.configuration(context.currentOrganizationId(),context.currentCompanyId(),name);}
    /** Conversion.ToBool of a configuration value. */
    public boolean flag(String name){return Boolean.parseBoolean(Objects.toString(configuration(name),"").trim())||"1".equals(Objects.toString(configuration(name),"").trim());}
    public int amountDigits(){String value=configuration("Default NoofDecimal Points For Amount");int digits=value==null||value.isBlank()?0:Integer.parseInt(value.trim());if(digits<0||digits>15)throw new IllegalStateException("Invalid amount decimal configuration");return digits;}
    /** CommonServices.GetERPFeatureById: 4 = subsidiary accounts on vouchers, 11 = branch feature. */
    public boolean feature(int id){return jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",context.currentOrganizationId(),context.currentCompanyId()).stream().anyMatch(row->i(copy(row),"Id")==id);}
    public boolean subsidiary(){return feature(4);}
    public double equivalent(int id,int item){
        if(id==0)return 0;
        var rows=jdbc.queryForList("SELECT Equivalent FROM dbo.UOMSchedule WHERE Id=? AND ItemId=? AND OrganizationId=? AND CompanyId=? AND Active=1",id,item,context.currentOrganizationId(),context.currentCompanyId());
        if(rows.size()!=1)throw new IllegalArgumentException("Select an active UOM belonging to this item and company");
        double value=n(copy(rows.get(0)),"Equivalent");if(value<=0)throw new IllegalArgumentException("The selected UOM has no positive equivalent");return value;
    }
    /** Conversion.ToInt of a configuration value (blank or non-numeric reads 0). */
    private static int toInt(String v){try{return v==null||v.isBlank()?0:(int)Math.rint(Double.parseDouble(v.trim()));}catch(NumberFormatException notNumeric){return 0;}}

    /** AccountsDtFillFromGlobal :1180 - dtAccountlst (Supplier Add/Less value list). Subsidiary: transporter parties (GlAccountId, Id, CompanyName); else accounts without types 2,11,12,13,14,15,20,21,22. */
    public List<Map<String,Object>> accounts(boolean subsidiary){
        int org=context.currentOrganizationId(),comp=context.currentCompanyId();
        if(subsidiary)return jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?,@CompanyId=?",org,comp);
        return customAccounts(Set.of(2,11,12,13,14,15,20,21,22));
    }
    /** dtAccountlstForFreight: the same list, except without subsidiary and with FreightDebitToExpenses only types 2,15,22 are removed. */
    public List<Map<String,Object>> freightAccounts(boolean subsidiary){
        if(subsidiary||!flag("DebitAmountChargetoExpenseAcFreightGridPurchase"))return accounts(subsidiary);
        return customAccounts(Set.of(2,15,22));
    }
    private List<Map<String,Object>> customAccounts(Set<Integer> excluded){
        var unique=new LinkedHashMap<Integer,Map<String,Object>>();
        for(var r:jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?,@CompanyId=?",context.currentOrganizationId(),context.currentCompanyId())){
            var c=copy(r);if(excluded.contains(i(c,"AccountTypeId")))continue;unique.putIfAbsent(i(c,"ChartOfAccountId"),r);
        }
        return new ArrayList<>(unique.values());
    }
    /** EBComboBind :1769 - DatatableHelper.GetAccountsFromGlobalByTypeIds(null, {10}). */
    public List<Map<String,Object>> bagCreditAccounts(){
        var unique=new LinkedHashMap<Integer,Map<String,Object>>();
        for(var r:jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?,@CompanyId=?",context.currentOrganizationId(),context.currentCompanyId())){
            var c=copy(r);if(i(c,"AccountTypeId")==10)unique.putIfAbsent(i(c,"ChartOfAccountId"),r);
        }
        return new ArrayList<>(unique.values());
    }
    /** OtherItemdtDbCall :1415 - InventoryItemsOther.GetAll (Id, OtherItemName). */
    public List<Map<String,Object>> otherItems(){return jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",context.currentOrganizationId(),context.currentCompanyId());}
    /** ItemDtsFillFromGlobal :1311 dtPmItems = CommonServices.GetPackingMaterialItemsAllocateToFlow() (TransactionFlowId 1). */
    public List<Map<String,Object>> packingMaterialItems(){return jdbc.queryForList("EXEC dbo.USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo @OrganizationId=?,@CompanyId=?,@TransactionFlowId=?",context.currentOrganizationId(),context.currentCompanyId(),1);}
    /** EBComboBind :1771 - globalItemConditions without Id 4 (Id, ConditionStatus). */
    public List<Map<String,Object>> itemConditions(){return jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition WHERE Id<>4");}
    /** GlobalVariables_Helper.GetWagesRefDocumentsStatusById(57): the USP_GetRefDocumentsForWages row of this document type. */
    public boolean wagesActive(){
        return jdbc.queryForList("EXEC dbo.USP_GetRefDocumentsForWages").stream().map(r->copy(r)).anyMatch(r->i(r,"RefDocumentTypeId")==TYPE&&(Boolean.TRUE.equals(r.get("IsActive"))||"1".equals(Objects.toString(r.get("IsActive"),""))||"true".equalsIgnoreCase(Objects.toString(r.get("IsActive"),""))));
    }
    /**
     * HistoryBranchdtFillDBCall :869 - the user's own branch when BranchImplemented (PurchaseInvoiceDirectBranchWise), else
     * InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(org, company, user, 57). At form load the call runs before
     * GetConfigurationsFromGlobal has read BranchImplemented (:726 vs :780), so the load list is always the allocated one
     * ({@code applyBranchWise=false}); History Refresh (:5224) applies the configuration.
     */
    public List<Map<String,Object>> historyBranches(boolean applyBranchWise){
        int org=context.currentOrganizationId(),company=context.currentCompanyId();
        if(applyBranchWise&&flag("PurchaseInvoiceDirectBranchWise"))return jdbc.queryForList("SELECT Id AS BranchId,BranchName FROM dbo.Branches WHERE Id=? AND OrganizationId=? AND CompanyId=?",context.currentBranchId(),org,company);
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromPurchaseInvoice @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=?",org,company,context.currentUserId(),TYPE);
    }
    /** HistoryComboDbCall :923 -> AllComboBindAgainstPurchaseInvoice (DocumentTypeIds "57", Activity "Supplier"): Id, ReferenceName. */
    public List<Map<String,Object>> historySuppliers(String branchIds){
        return jdbc.queryForList("EXEC dbo.Usp_AllComboAgainstPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?,@Activity=?,@BranchesIds=?",context.currentOrganizationId(),context.currentCompanyId(),"57","Supplier",branchIds);
    }
    public Map<String,Object> all(){
        records.requireRight(TYPE,"View");int org=context.currentOrganizationId(),comp=context.currentCompanyId(),branch=context.currentBranchId();
        var result=new LinkedHashMap<String,Object>();var rights=new LinkedHashMap<String,Boolean>();for(String right:List.of("View","Save","Update","Delete","Print"))rights.put(right,records.hasRight(TYPE,right));result.put("rights",rights);
        var features=jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",org,comp);
        boolean subsidiary=features.stream().anyMatch(row->i(copy(row),"Id")==4);
        boolean branchFeature=features.stream().anyMatch(row->i(copy(row),"Id")==11);
        boolean branchOnly=branchFeature&&flag("PurchaseInvoiceDirectBranchWise");
        var suppliers=jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?,@CompanyId=?",org,comp);
        suppliers.removeIf(row->{var r=copy(row);return i(r,"CustomerGroupId")==7||(subsidiary&&i(r,"PartyTypeId")!=1);});result.put("suppliers",suppliers);
        var items=jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?,@CompanyId=?",org,comp);
        items.removeIf(row->Set.of(14,17).contains(i(copy(row),"ItemTypeOfTypeId")));result.put("items",items);
        var warehouses=jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",org,comp,branch);
        var lots=jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?,@CompanyId=?,@Activity='GetJobLotGlIdsandName'",org,comp);
        if(branchOnly){warehouses.removeIf(row->i(copy(row),"BranchId")!=branch);lots.removeIf(row->i(copy(row),"BranchId")!=branch);}
        result.put("warehouses",warehouses);result.put("jobLots",lots);
        result.put("cropYears",jdbc.queryForList("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",org,comp));
        var packing=jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'");packing.removeIf(row->!Set.of(1,2,5,10).contains(i(copy(row),"Id")));result.put("packingTypes",packing);
        result.put("uoms",jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?,@CompanyId=?,@Active=1",org,comp));
        result.put("paymentTerms",jdbc.queryForList("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",org,comp));
        result.put("commissionUoms",jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='GetCommissionUom'"));
        // DeliveryTerm() :1119 - literal Id/DeliveryTerm rows 1..6 bound with DDL.BindDDL.
        result.put("deliveryTerms",List.of("Load","Ponch","Load & PartyWeight","Load & FactoryWeight","Ponch & PartyWeight","Ponch & FactoryWeight"));
        // Grid value lists.
        result.put("accounts",accounts(subsidiary));result.put("freightAccounts",freightAccounts(subsidiary));
        result.put("otherItems",otherItems());result.put("pmItems",packingMaterialItems());
        result.put("emptyBagTypes",jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='PurchaseOrderEmptyBagsType'"));
        result.put("bagCreditAccounts",bagCreditAccounts());result.put("itemConditions",itemConditions());
        // GetConfigurationsFromGlobalAndBindValuesInColumns :1009 defaults and GetConfigurationsFromGlobal :992 flags.
        var defaults=new LinkedHashMap<String,Object>();for(String name:List.of("Warehouse","Default Crop Year","Job/Lot","Paking Type"))defaults.put(name,configuration(name));result.put("defaults",defaults);
        var flags=new LinkedHashMap<String,Boolean>();for(String name:List.of("WagesCompulsoryOnPurchaseInvoiceDirect","DebitAmountChargetoExpenseAcFreightGridPurchase","PurchaseInvoiceDirectBranchWise","ItemSearchByCode"))flags.put(name,flag(name));result.put("configuration",flags);
        result.put("wagesActive",wagesActive());
        result.put("historyFromDays",toInt(configuration("DefaultDaysToLessFromHistoryFromDate")));
        result.put("amountDigits",amountDigits());result.put("subsidiary",subsidiary);result.put("branchFeature",branchFeature);result.put("currentBranchId",branch);
        result.put("historyBranches",historyBranches(false));
        // HistoryComboDbCall(ValidateBranch:false) at load: the branch combo is still empty, so the user's branch is sent (:950).
        result.put("historySuppliers",historySuppliers(String.valueOf(branch)));
        return result;
    }
}
