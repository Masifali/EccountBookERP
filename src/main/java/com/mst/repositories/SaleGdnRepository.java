package com.mst.repositories;

import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Desktop Architecture.WinApp.Sale.InvFrmGDN data access (document type 86). */
@Repository
public class SaleGdnRepository extends SaleGdnPurchaseReturnRepository {
    public static final int DOCUMENT_TYPE_ID=86;
    public SaleGdnRepository(JdbcTemplate jdbc){super(jdbc);}
    @Override protected int documentTypeId(){return DOCUMENT_TYPE_ID;}
    @Override protected String screenName(){return "InvFrmGDN";}
    @Override protected String recordLabel(){return "Goods Dispatch Note";}

    @Override public Map<String,Object> initial(UserAccount u,int year){
        Map<String,Object> m=new LinkedHashMap<>();
        var no=q("EXEC dbo.Sp_InvGdn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GenerateInvGdnCode'",u.getOrganizationId(),u.getCompanyId(),DOCUMENT_TYPE_ID,year,u.getBranchesId());
        m.put("nextNo",no.isEmpty()?1:no.get(0).get("DocNo"));
        boolean subsidiaryAccounts=feature(u,4);   // SubsidiaryAccountAllownOnVouchers = GetERPFeatureById(4), InvFrmGDN:680
        m.put("customers",customers(u,subsidiaryAccounts));
        m.put("gatePasses",q("EXEC dbo.Sp_GatePassOutward_GetAllMethod @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GpNoPandingforInvGdn'",u.getOrganizationId(),u.getCompanyId(),year,u.getBranchesId()));
        m.put("warehouses",q("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()));
        m.put("items",items(u,0));
        m.put("brands",globalBrands(u));        // BrandDtFillFromGlobal, InvFrmGDN:1055 - cached usp_getBrands
        m.put("crops",q("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId()));
        m.put("jobLots",saleJobLots(u));        // JobLotDtFillFromGlobalAndBind, InvFrmGDN:1117 - not branch-allocated
        m.put("packingTypes",q("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"));
        m.put("otherItems",q("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll',@organizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()));
        m.put("cities",globalCities(u));        // CityDtFillFromGlobalAndBind, InvFrmGDN:1182
        m.put("transporters",subsidiaryAccounts?transporterParties(u,true):accountTransporters(u));
        m.put("feature5",feature(u,5));
        return m;
    }

    /** SupplierDtFillFromGlobal (InvFrmGDN:883). The cache is clsGlobalVariables.globalSupplierCustomer, filled by
        DatatableHelper.GlobalSupplierCustomerListsFillDbCall(org, comp, 0) from USP_GetVendorsAndCustomersWithCityName:
        CustomerGroupId 7, 9, 10 always dropped; 8 and 13 also dropped when config ShowBothVendorAndCustomerOnSalesPurchase = 1.
        With ERP feature 4 the form keeps PartyTypeId == 2 only. Row shape = the form's dtSupplier (:578). */
    private List<Map<String,Object>> customers(UserAccount u,boolean subsidiaryAccounts){
        int showBoth;try{showBoth=Integer.parseInt(config(u,"ShowBothVendorAndCustomerOnSalesPurchase"));}catch(NumberFormatException e){showBoth=0;}
        Set<Integer> always=Set.of(7,9,10),withShowBoth=Set.of(7,8,9,10,13);
        List<Map<String,Object>> out=new ArrayList<>();
        for(var r:q("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId())){
            int group=number(r.get("CustomerGroupId"));
            if(always.contains(group)||(showBoth==1&&withShowBoth.contains(group)))continue;
            if(subsidiaryAccounts&&number(r.get("PartyTypeId"))!=2)continue;
            Map<String,Object> x=new LinkedHashMap<>();
            x.put("Id",r.get("Id"));x.put("CompanyName",r.get("CompanyName"));x.put("PartyCode",r.get("PartyCode"));x.put("GlAccountId",r.get("GlAccountId"));
            x.put("CityId",r.get("CityId"));x.put("CityName",r.get("CityName"));x.put("MobileNo",r.get("MobilePersonal"));
            out.add(x);
        }
        return out;
    }

    /** TransporterDtFillFromGlobal without feature 4 (InvFrmGDN:950): clsGlobalVariables.AllAccountsWithCustomGroupId
        = USP_GETAllAccountsFromCustomGroups @OrganizationId,@CompanyId (GlobalServicesMethods:609), dropping
        AccountTypeId 2, 4, 10, 11, 12, 15 and keeping the first row per ChartOfAccountId. The web used to send
        AccountTypeIds '6,8' - that list belongs to the purchase-return form, not to this one. */
    private List<Map<String,Object>> accountTransporters(UserAccount u){
        Set<Integer> excluded=Set.of(2,4,10,11,12,15);
        Map<Integer,Map<String,Object>> byId=new LinkedHashMap<>();
        for(var r:q("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId())){
            if(excluded.contains(number(r.get("AccountTypeId"))))continue;
            int id=number(r.get("ChartOfAccountId"));if(byId.containsKey(id))continue;
            Map<String,Object> x=new LinkedHashMap<>();
            x.put("Id",r.get("ChartOfAccountId"));x.put("AccountTitle",r.get("AccountTitle"));x.put("AccountCode",r.get("AccountCode"));x.put("SupplierCustomerId",0);
            byId.put(id,x);
        }
        return new ArrayList<>(byId.values());
    }

    @Override public List<Map<String,Object>> items(UserAccount u,int ignored){
        return q("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()).stream()
                .filter(r->number(r.get("ItemTypeOfTypeId"))!=14&&number(r.get("ItemTypeOfTypeId"))!=17).toList();
    }
    public List<Map<String,Object>> advanceOrders(UserAccount u,int year,int customerId,int gdnId){
        return q("EXEC dbo.USP_DeliveryOrder_GetAdvanceDo @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@BranchesId=?,@SupplierCustomerId=?,@GdnRecId=?",u.getOrganizationId(),u.getCompanyId(),year,u.getBranchesId(),customerId,gdnId);
    }
    public List<Map<String,Object>> deliveryOrder(UserAccount u,int customerId,int id){
        return q("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=84,@SupplierCustomerId=?,@Id=?,@Activity='DeliveryOrderLoad'",u.getOrganizationId(),u.getCompanyId(),customerId,id);
    }
    public List<Map<String,Object>> gatePassCustomers(UserAccount u,int gatePassId){
        return q("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@Activity='CustomerLoadForDeliveryOrderId'",u.getOrganizationId(),u.getCompanyId(),gatePassId);
    }
    public List<Map<String,Object>> expenses(String deliveryOrderIds){
        return q("EXEC dbo.USP_InvDeliveryOrderExpensesByDoIds @DeliveryOrderIds=?",deliveryOrderIds);
    }
}
