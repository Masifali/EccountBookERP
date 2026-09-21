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
        m.put("customers",q("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationIdCompanyIdForBinding'",u.getOrganizationId(),u.getCompanyId()));
        m.put("gatePasses",q("EXEC dbo.Sp_GatePassOutward_GetAllMethod @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GpNoPandingforInvGdn'",u.getOrganizationId(),u.getCompanyId(),year,u.getBranchesId()));
        m.put("warehouses",q("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()));
        m.put("items",items(u,0));
        m.put("brands",q("EXEC dbo.USP_Brand_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='FormHistory',@Id=0",u.getOrganizationId(),u.getCompanyId()));
        m.put("crops",q("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId()));
        m.put("jobLots",q("EXEC dbo.USP_GetJobLotsAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()));
        m.put("packingTypes",q("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"));
        m.put("otherItems",q("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll',@organizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()));
        m.put("cities",q("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'",u.getOrganizationId(),u.getCompanyId()));
        m.put("transporters",q("EXEC dbo.USP_Accounts_GetAccountTitleByAccountTypeIds @OrganizationId=?,@CompanyId=?,@AppId=?,@UserId=?,@AccountTypeIds=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),u.getId(),"6,8"));
        m.put("feature5",q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()).stream().anyMatch(r->number(r.get("Id"))==5));
        return m;
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
