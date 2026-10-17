package com.mst.repositories;

import java.util.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Procedure calls shared by the Paddy Purchase screens (module 48): Architecture.WinApp.Paddy_Purchase
 * frmGatePassInwardForGate / frmGatePassInwardForGhallaMandi / frmGrnForGate / frmGrnForGhallaMandi /
 * frmPurchaseInvoiceForGate / frmPurchaseInvoiceForGhallaMandi. Every method is one BLL/DAL call of the desktop
 * (the BLL method is named in the comment); organization, company, branch and year are always passed in by the caller.
 */
@Repository
public class PaddyPurchaseRepository {
    @Autowired private JdbcTemplate jdbc;

    /** CommonServices.GetSupplierustomerByCustomerGroupId(ids): BLL SupplierCustomer.GetSupplierustomerByCustomerGroupId. ParentId is sent only when non-zero, the ids only when non-empty. */
    public List<Map<String,Object>> suppliersByGroupIds(int org,int company,String ids,int parentId) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?");
        List<Object> args=new ArrayList<>(List.of(org,company));
        if (parentId!=0) { sql.append(",@ParentId=?"); args.add(parentId); }
        if (ids!=null && !ids.isEmpty()) { sql.append(",@CustomerGroupIds=?"); args.add(ids); }
        sql.append(",@Activity='GetSupplierustomerByCustomerGroupId'");
        return jdbc.queryForList(sql.toString(),args.toArray());
    }

    /** CommonServices.SupplierCustomerGetAllServiceBind(): SupplierCustomer.Getall -> 'ReadByOrganizationCompanyId' (dtSupplier: Id, CNIC, MobilePersonal ...). */
    public List<Map<String,Object>> supplierCustomerAll(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationCompanyId'",org,company);
    }

    /** CommonServices.ReadAllItems(): Item.ReadAllItems -> Sp_Item_GetAllMethod 'ReadAllItems'. */
    public List<Map<String,Object>> readAllItems(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAllItems'",org,company);
    }

    /** VehicleType.GetAll(): Sp_VehicleType_GetAllMethod (no parameters). */
    public List<Map<String,Object>> vehicleTypes() {
        return jdbc.queryForList("EXEC dbo.Sp_VehicleType_GetAllMethod");
    }

    /** City.GetAll(new City{OrganizationId, CompanyId}): SP_City_GetAllMethod 'GetAll'. */
    public List<Map<String,Object>> cities(int org,int company) {
        return jdbc.queryForList("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'",org,company);
    }

    /** City.GetDistrictAndTehsilByCityId: SP_City_GetAllMethod 'GetDistrictAndTehsilByCityId' (DistrictId, District, TehsilId, Tehsil). */
    public List<Map<String,Object>> districtAndTehsil(int org,int company,int cityId) {
        return jdbc.queryForList("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@MethodType='GetDistrictAndTehsilByCityId'",org,company,cityId);
    }

    /**
     * CommonServices.GatepassHistoryForRefdocumentTypeWise(DocumentTypeId, canViewAll, NoOfRecords, RefDocumentTypeId):
     * BLL GatePassInward.GatepassHistoryForRefdocumentTypeWise. BranchesId stays 0 in the desktop object, so it is not sent;
     * NoOfRecords is sent only when above 0; EntryUser only when the user cannot view all records.
     */
    public List<Map<String,Object>> gatePassHistoryByRefType(int org,int company,int year,int userId,int documentTypeId,boolean canViewAll,int noOfRecords,int refDocumentTypeId) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_GatePassInward_GetAllMethod @organizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@CanViewAllRecord=?");
        List<Object> args=new ArrayList<>(List.of(org,company,documentTypeId,year,canViewAll));
        if (refDocumentTypeId!=0) { sql.append(",@RefDocumentTypeId=?"); args.add(refDocumentTypeId); }
        if (noOfRecords>0) { sql.append(",@NoOfRecords=?"); args.add(noOfRecords); }
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(userId); }
        sql.append(",@Activity='GatepassHistoryForRefdocumentTypeWise'");
        return jdbc.queryForList(sql.toString(),args.toArray());
    }

    /** CommonServices.GetGhallaMandi -> Ghalla_Mandi.GetAll: USp_Ghalla_Mandi_GetAllMethod 'GetAll'. */
    public List<Map<String,Object>> ghallaMandi(int org,int company) {
        return jdbc.queryForList("EXEC dbo.USp_Ghalla_Mandi_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",org,company);
    }

    /**
     * GatePassInward.GatepassHistoryForGhallaMandi(ReportsParameters): Sp_GatePassInward_GetAllMethod 'GatepassHistoryForGhallaMandi';
     * NoOfRecords only when non-zero, EntryUser only when the user cannot view all records, BranchesId (0 in the form) not sent.
     */
    public List<Map<String,Object>> gatePassHistoryGhalla(int org,int company,int year,int userId,int documentTypeId,boolean canViewAll,int noOfRecords,int refDocumentTypeId) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_GatePassInward_GetAllMethod @organizationId=?,@CompanyId=?,@DocumentTypeId=?,@RefDocumentTypeId=?,@FinancialYearId=?");
        List<Object> args=new ArrayList<>(List.of(org,company,documentTypeId,refDocumentTypeId,year));
        if (noOfRecords!=0) { sql.append(",@NoOfRecords=?"); args.add(noOfRecords); }
        sql.append(",@CanViewAllRecord=?"); args.add(canViewAll);
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(userId); }
        sql.append(",@Activity='GatepassHistoryForGhallaMandi'");
        return jdbc.queryForList(sql.toString(),args.toArray());
    }

    /** GatePassInward.GetOpenGatePassForGpDetail: zero FinancialYearId / RefDocumentTypeId / BranchesId are not sent. */
    public List<Map<String,Object>> openGatePassForGpDetail(int org,int company,int documentTypeId,int year,int refDocumentTypeId,int branch,String activity) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?");
        List<Object> args=new ArrayList<>(List.of(org,company,documentTypeId));
        if (year!=0) { sql.append(",@FinancialYearId=?"); args.add(year); }
        if (refDocumentTypeId!=0) { sql.append(",@RefDocumentTypeId=?"); args.add(refDocumentTypeId); }
        if (branch!=0) { sql.append(",@BranchesId=?"); args.add(branch); }
        sql.append(",@Activity=?"); args.add(activity);
        return jdbc.queryForList(sql.toString(),args.toArray());
    }

    /** GatePassInward.ReadByHeaderId(Id): Sp_GatePassInward_GetAllMethod 'ReadByHeaderId' (history detail grid). */
    public List<Map<String,Object>> gatePassReadByHeaderId(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @Id=?,@Activity='ReadByHeaderId'",id);
    }

    /** GatePassInward.GetByID header (Sp_GatePassInward_GetAllMethod 'ReadById'). */
    public Map<String,Object> gatePassHeader(int id) {
        List<Map<String,Object>> rows=jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @Id=?,@Activity='ReadById'",id);
        return rows.isEmpty()?null:rows.get(0);
    }

    /** GatePassInward.GetByID details for DocumentTypeId other than 53: Sp_GatePassInwardDetail_GetAllMethod 'ReadById'. */
    public List<Map<String,Object>> gatePassDetails(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_GatePassInwardDetail_GetAllMethod @Id=?,@Activity='ReadById'",id);
    }

    /** DMSAttachments.GetByID(Id, ScreenName): Sp_DMSAttachments_GetAllMethod 'ReadById'. */
    public List<Map<String,Object>> attachmentsByScreen(int id,String screenName) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity='ReadById'",screenName,id);
    }
}
