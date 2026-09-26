package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;

/** InvFrmGRN/SaleReturnGrn bindings and their GlobalServicesMethods queries. */
@Repository
public class PurchaseGrnLookupRepository {
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final PurchaseGrnRecordRepository records;
    public PurchaseGrnLookupRepository(JdbcTemplate jdbc,CurrentUserContext context,PurchaseGrnRecordRepository records) { this.jdbc=jdbc;this.context=context;this.records=records; }
    public Map<String,Object> all(int type) {
        records.requireRight(type,"View");
        int org=context.currentOrganizationId(),company=context.currentCompanyId(),branch=context.currentBranchId();
        Map<String,Object> result=new LinkedHashMap<>();
        Map<String,Boolean> rights=new LinkedHashMap<>();
        for(String right:List.of("View","Save","Update","Delete","Print"))rights.put(right,records.hasRight(type,right));
        result.put("rights",rights);
        boolean subsidiary=jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",org,company).stream().anyMatch(row->number(row.get("Id"))==4);
        List<Map<String,Object>> suppliers;
        if(type==143) suppliers=jdbc.queryForList("EXEC dbo.USP_GetPartiesFromSaleInvoiceWithGlAccount @OrganizationId=?,@CompanyId=?,@DocumentTypeIds='95,99,186'",org,company);
        else {
            suppliers=jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?,@CompanyId=?",org,company);
            var configs=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription='ShowBothVendorAndCustomerOnSalesPurchase',@Activity='GetConfigurationByOrgCompandConfigDescription'",org,company);
            boolean showBoth=!configs.isEmpty() && "1".equals(Objects.toString(configs.get(0).get("ConfigKey")));
            suppliers.removeIf(row->Set.of(7,9,10).contains(number(row.get("CustomerGroupId"))) || (showBoth && Set.of(8,13).contains(number(row.get("CustomerGroupId")))) || (subsidiary && number(row.get("PartyTypeId"))!=1));
        }
        result.put("suppliers",options(suppliers,"Id","CompanyName"));
        // These are desktop workflow choices (DeliveryTermBind), rather than master records.
        result.put("deliveryTerms",List.of("Load","Ponch","Load & PartyWeight","Load & FactoryWeight","Ponch & PartyWeight","Ponch & FactoryWeight"));
        result.put("items",List.of()); // Desktop fills these after selecting a GP (or return customer).
        result.put("warehouses",options(jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",org,company,branch),"Id","WarehouseName"));
        result.put("cropYears",options(jdbc.queryForList("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",org,company),"Id","CropYear"));
        result.put("jobLots",options(jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?,@CompanyId=?,@Activity='GetJobLotGlIdsandName'",org,company),"Id","JobLotDescription"));
        result.put("cities",options(jdbc.queryForList("EXEC dbo.USP_City_GetAllWithCountryAndTehsil @OrganizationId=?,@CompanyId=?",org,company),"Id","CityName"));
        result.put("vehicleTypes",options(jdbc.queryForList("EXEC dbo.Sp_VehicleType_GetAllMethod"),"Id","VehicleDescription"));
        var packing=jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'");
        if(type==46)packing.removeIf(row->!Set.of(1,2,5).contains(number(row.get("Id"))));
        result.put("packingTypes",options(packing,"Id","PackTypeDesc"));
        result.put("uoms",options(jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?,@CompanyId=?,@Active=1",org,company),"Id","UOMCode"));
        result.put("emptyBagTypes",options(jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='PurchaseOrderEmptyBagsType'"),"Id","type"));
        result.put("emptyBagItems",options(jdbc.queryForList("EXEC dbo.USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo @OrganizationId=?,@CompanyId=?,@TransactionFlowId=1",org,company),"ItemId","ItemName"));
        result.put("bagConditions",options(jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition WHERE Id<>4"),"ID","ConditionStatus"));
        if(subsidiary)result.put("transporters",options(jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?,@CompanyId=?",org,company),"GlAccountId","CompanyName"));
        else {
            var accounts=jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?,@CompanyId=? WITH RECOMPILE",org,company);
            Set<Integer> seen=new HashSet<>();
            accounts.removeIf(row->Set.of(2,4,10,11,12,13,14,15,20,21,22).contains(number(row.get("AccountTypeId"))) || !seen.add(number(row.get("ChartOfAccountId"))));
            result.put("transporters",options(accounts,"ChartOfAccountId","AccountTitle"));
        }
        var pendingGps=jdbc.queryForList("EXEC dbo."+(type==46?"USP_GatePassInward_PendingForGrn":"USP_PendingInwardGatePassForSaleReturnGrn")+" @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=51",org,company,branch,context.currentFinancialYearId());
        // GatepassGridFill :2490 — FVStatus, LastLabId, PoAccessWeight and ItemName (VarietyName) are grid columns.
        if(type==46)pendingGps.replaceAll(PurchaseGrnFormRepository::pendingShape);
        result.put("gatePasses",options(pendingGps,"Id","GpSrNo"));
        return result;
    }
    public List<Map<String,Object>> returnItems(int party) {
        records.requireRight(143,"View");
        return options(jdbc.queryForList("EXEC dbo.USP_GetItemsFromSaleInvoiceAgainstPartyId @OrganizationId=?,@CompanyId=?,@SupplierCustomerId=?,@DocumentTypeIds='95,99'",context.currentOrganizationId(),context.currentCompanyId(),party),"Id","ItemName");
    }
    public Map<String,Object> gatePass(int id,int type) {
        records.requireRight(type,"View");
        int org=context.currentOrganizationId(),company=context.currentCompanyId();
        if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? AND DocumentTypeId=51",Integer.class,id,org,company,context.currentBranchId(),context.currentFinancialYearId())!=1)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Gate pass not found in the current company, branch and financial year");
        var gp=jdbc.queryForMap("EXEC dbo.Sp_GatePassInward_GetAllMethod @Id=?,@Activity='ReadById'",id);
        var result=new LinkedHashMap<String,Object>(gp);
        result.put("items",options(jdbc.queryForList("EXEC dbo.usp_getItemsFromLabOrPurchaseOrderOrAllByGpId @OrganizationId=?,@CompanyId=?,@Id=?",org,company,id),"ItemId","ItemName"));
        result.put("weights",jdbc.queryForList("EXEC dbo.Sp_WbTransation_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@RefDocumentTypeId=51,@Activity='GetNetWeightFromWbTransactions'",org,company,id));
        if(type==46) {
            var breakups=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadByGPId_InvGrnPurchaseBreakup'",id);
            result.put("breakupLocked",!breakups.isEmpty());
            if(breakups.isEmpty()) {
                breakups=jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @Id=?,@Activity='ReadByHeaderId_PurchaseBrekup'",id);
                for(var row:breakups) { row.put("InwardBreakupId",row.get("Id"));row.put("Id",0); }
            }
            result.put("purchaseBreakups",breakups);
        }
        return result;
    }
    private static List<Map<String,Object>> options(List<Map<String,Object>> rows,String id,String caption) {
        List<Map<String,Object>> result=new ArrayList<>();
        for(var row:rows) { var mapped=new LinkedHashMap<String,Object>(row); mapped.put("id",row.get(id)); mapped.put("name",row.get(caption)); result.add(mapped); }
        return result;
    }
}
