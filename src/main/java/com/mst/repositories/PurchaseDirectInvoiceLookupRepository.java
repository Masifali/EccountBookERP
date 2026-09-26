package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** InvfrmPurchasedirectInvoice: SupplierDtFillFromGlobal through PackUomFromGlobalBind. */
@Repository
public class PurchaseDirectInvoiceLookupRepository {
    private final JdbcTemplate jdbc; private final CurrentUserContext context;
    private final PurchaseInvoiceRecordRepository records; private final PurchaseInvoiceWriteRepository writes;
    public PurchaseDirectInvoiceLookupRepository(JdbcTemplate jdbc,CurrentUserContext context,PurchaseInvoiceRecordRepository records,PurchaseInvoiceWriteRepository writes){this.jdbc=jdbc;this.context=context;this.records=records;this.writes=writes;}
    public String configuration(String name){return writes.configuration(context.currentOrganizationId(),context.currentCompanyId(),name);}
    public int amountDigits(){String value=configuration("Default NoofDecimal Points For Amount");int digits=value==null||value.isBlank()?0:Integer.parseInt(value);if(digits<0||digits>15)throw new IllegalStateException("Invalid amount decimal configuration");return digits;}
    public double equivalent(int id,int item){
        if(id==0)return 0;
        var rows=jdbc.queryForList("SELECT Equivalent FROM dbo.UOMSchedule WHERE Id=? AND ItemId=? AND OrganizationId=? AND CompanyId=? AND Active=1",id,item,context.currentOrganizationId(),context.currentCompanyId());
        if(rows.size()!=1)throw new IllegalArgumentException("Select an active UOM belonging to this item and company");
        double value=n(copy(rows.get(0)),"Equivalent");if(value<=0)throw new IllegalArgumentException("The selected UOM has no positive equivalent");return value;
    }
    public Map<String,Object> all(){
        records.requireRight(57,"View");int org=context.currentOrganizationId(),comp=context.currentCompanyId(),branch=context.currentBranchId();
        var result=new LinkedHashMap<String,Object>();var rights=new LinkedHashMap<String,Boolean>();for(String right:List.of("View","Save","Update","Delete","Print"))rights.put(right,records.hasRight(57,right));result.put("rights",rights);
        var features=jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",org,comp);
        boolean subsidiary=features.stream().anyMatch(row->i(copy(row),"Id")==4);
        boolean branchOnly=features.stream().anyMatch(row->i(copy(row),"Id")==11)&&Boolean.parseBoolean(configuration("PurchaseInvoiceDirectBranchWise"));
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
        // Literal workflow choices in DeliveryTerm(), not invented master data.
        result.put("deliveryTerms",List.of("Load","Ponch","Load & PartyWeight","Load & FactoryWeight","Ponch & PartyWeight","Ponch & FactoryWeight"));
        var defaults=new LinkedHashMap<String,Object>();for(String name:List.of("Warehouse","Default Crop Year","Job/Lot","Paking Type"))defaults.put(name,configuration(name));result.put("defaults",defaults);
        result.put("amountDigits",amountDigits());result.put("subsidiary",subsidiary);return result;
    }
}
