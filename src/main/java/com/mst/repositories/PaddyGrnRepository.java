package com.mst.repositories;

import java.util.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Procedure calls shared by the Paddy Purchase GRN screens (465 frmGrnForGate, 469 frmGrnForGhallaMandi).
 * Every method is one BLL call of the desktop (named in the comment); organization, company, year and user are passed in by the caller.
 */
@Repository
public class PaddyGrnRepository {
    @Autowired private JdbcTemplate jdbc;

    /** CommonServices.BrancheServiceBind(): Branches.GetAll -> Sp_Branches_GetAllMethod 'GetAll'. */
    public List<Map<String,Object>> branches(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",org,company);
    }
    /** CommonServices.ProjectServiceBind(): Projects.GetAlldt -> Sp_Projects_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String,Object>> projects(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_Projects_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'",org,company);
    }
    /** COAAllocation.GetAccountTitleByAccountTypeIds: UserId / AccountTypeIds are sent only when set. */
    public List<Map<String,Object>> accountsByTypeIds(int org,int company,int appId,int userId,String typeIds) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@AppId=?");
        List<Object> args=new ArrayList<>(List.of(org,company,appId));
        if (typeIds!=null && !typeIds.isEmpty()) { sql.append(",@AccountTypeIds=?"); args.add(typeIds); }
        if (userId!=0) { sql.append(",@UserId=?"); args.add(userId); }
        sql.append(",@Activity='GetAccountTitleByAccountTypeIds'");
        return jdbc.queryForList(sql.toString(),args.toArray());
    }
    /** CommonServices.CropYearGetAllService(): InvCropYear.Getall -> Sp_InvCropYear_GetAllMethod 'ReadAll'. */
    public List<Map<String,Object>> cropYears(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",org,company);
    }
    /** CommonServices.JobLotGetAllService(): jobLot.GetAll -> SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String,Object>> jobLots(int org,int company) {
        return jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",org,company);
    }
    /** InvPackingType.Getall(): Sp_InvPackingType_GetAllMethod 'ReadAll'. */
    public List<Map<String,Object>> packingTypes() {
        return jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'");
    }
    /** CommonServices.getActiveWareHouse(): InvWareHouse.GetActiveWareHouse -> Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'. */
    public List<Map<String,Object>> activeWarehouses(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetActiveWareHouse'",org,company);
    }
    /** CommonServices.GetUomScheduleByItemId(ItemId): UOMSchedule.SearchByObject -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID' (Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom). */
    public List<Map<String,Object>> uomScheduleByItem(int org,int company,int itemId) {
        return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'",org,company,itemId);
    }
    /** CommonServices.StaticColumnsService(activity): GeneralReprots.StaticColumnNames -> SpStaticColumnNames. */
    public List<Map<String,Object>> staticColumns(String activity) {
        return jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity=?",activity);
    }
    /** CommonServices.GetItemByItemTypeId(ids): Item.GetItemByItemTypeId -> Sp_Item_GetAllMethod 'GetItemByItemTypeId'. */
    public List<Map<String,Object>> itemsByItemTypeIds(int org,int company,String ids) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@LookupTypeIds=?,@Activity='GetItemByItemTypeId'",org,company,ids);
    }
    /** CommonServices.GetMultipleConfigurationsByConfigDescriptions(csv): Sp_ConfigrationsAllocation_GetAllMethod. */
    public List<Map<String,Object>> configurations(int org,int company,String csv) {
        return jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetMultipleConfigurationsByConfigDescriptions'",org,company,csv);
    }

    /** InvGrn.GenerateInvGrnCode: FinancialYearId sent when non-zero, BranchesId stays 0 in the desktop object (not sent). Returns DocNo of the first row. */
    public int generateInvGrnCode(int org,int company,int documentTypeId,int year) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?");
        List<Object> args=new ArrayList<>(List.of(org,company,documentTypeId));
        if (year!=0) { sql.append(",@FinancialYearId=?"); args.add(year); }
        sql.append(",@Activity='GenerateInvGrnCode'");
        List<Map<String,Object>> rows=jdbc.queryForList(sql.toString(),args.toArray());
        if (rows.isEmpty()) return 0;
        Object v=rows.get(0).get("DocNo");
        return v instanceof Number?((Number)v).intValue():0;
    }

    /** GatePassInward.GetPandingGatePassforInvGrnForGatePurchase: BranchesId not sent. */
    public List<Map<String,Object>> pendingGatePassesForGate(int org,int company,int documentTypeId,int year) {
        return jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity='GetPandingGatePassforInvGrnForGatePurchase'",org,company,documentTypeId,year);
    }
    /** CommonServices.GetGhallaMandi (GhallaMandiBind): Ghalla_Mandi.GetAll -> USp_Ghalla_Mandi_GetAllMethod 'GetAll'. */
    public List<Map<String,Object>> ghallaMandi(int org,int company) {
        return jdbc.queryForList("EXEC dbo.USp_Ghalla_Mandi_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",org,company);
    }
    /** CommonServices.ItemGetAllServiceBind() (frmGrnForGhallaMandi.ItemsBind): Item.GetAll -> Sp_Item_GetAllMethod 'ReadByOrganizationCompanyId' (no parent categories sent). */
    public List<Map<String,Object>> allItems(int org,int company) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationCompanyId'",org,company);
    }
    /** GatePassInward.GetPandingGatePassforInvGrnForGhallaMandiPurchase: BranchesId not sent. */
    public List<Map<String,Object>> pendingGatePassesForGhalla(int org,int company,int documentTypeId,int year) {
        return jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity='GetPandingGatePassforInvGrnForGhallaMandiPurchase'",org,company,documentTypeId,year);
    }
    /** GatePassInward.GetByGpNoForGRN: FinancialYearId sent when non-zero, BranchesId not sent. */
    public List<Map<String,Object>> gatePassForGrn(int org,int company,int gpId,int documentTypeId,int year) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=?");
        List<Object> args=new ArrayList<>(List.of(org,company,gpId,documentTypeId));
        if (year!=0) { sql.append(",@FinancialYearId=?"); args.add(year); }
        sql.append(",@Activity='ReadByGpNoForGRN'");
        return jdbc.queryForList(sql.toString(),args.toArray());
    }
    /** GatePassInward.GetLabDataByGpIdForGrnGate. */
    public List<Map<String,Object>> labDataForGate(int org,int company,int gpId) {
        return jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@Activity='GetLabDataByGpIdForGrnGate'",org,company,gpId);
    }

    /** InvGrn.GetHisoty(ReportsParameters): BranchesId / FromDate etc. not set by the Gate screens; EntryUser only when the user cannot view all, NoOfRecords only when above 0. */
    public List<Map<String,Object>> grnHistory(int org,int company,int documentTypeId,boolean canViewAll,int year,int userId,int noOfRecords) {
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?");
        List<Object> args=new ArrayList<>(List.of(org,company,documentTypeId,canViewAll));
        if (year!=0) { sql.append(",@FinancialYearId=?"); args.add(year); }
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(userId); }
        if (noOfRecords!=0) { sql.append(",@NoOfRecords=?"); args.add(noOfRecords); }
        sql.append(",@Activity='GRNFormHistory'");
        return jdbc.queryForList(sql.toString(),args.toArray());
    }
    /** InvGrn.GetByID header: Sp_InvGRN_GetAllMethod 'ReadByID'. */
    public Map<String,Object> grnHeader(int id) {
        List<Map<String,Object>> rows=jdbc.queryForList("EXEC dbo.Sp_InvGRN_GetAllMethod @Id=?,@Activity='ReadByID'",id);
        return rows.isEmpty()?null:rows.get(0);
    }
    /** InvGrn DAL GetDate, DocumentTypeId 46/165/167/169/143: Sp_InvGrnDetail_GetAllMethod 'ReadByInvGrnID'. */
    public List<Map<String,Object>> grnDetails(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity='ReadByInvGrnID'",id);
    }
    /** InvGrn DAL GetDate: Sp_InvGrnDetail_GetAllMethod 'ReadByInvGrnIdEmptyBagsDetail'. */
    public List<Map<String,Object>> grnEmptyBags(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity='ReadByInvGrnIdEmptyBagsDetail'",id);
    }
    /** DMSAttachments.ReadAttachmentsbyRefDocumentTypeId(Id, RefDocumentTypeId): the NoOfAttachments link of the history grid. */
    public List<Map<String,Object>> attachmentsByRefDocumentType(int id,int refDocumentTypeId) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @RefDocumentTypeId=?,@Id=?,@Activity='ReadAttachmentsbyRefDocumentTypeId'",refDocumentTypeId,id);
    }
    /** DMSAttachments.GetByID(Id, ScreenName). */
    public List<Map<String,Object>> attachmentsByScreen(int id,String screenName) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity='ReadById'",screenName,id);
    }
}
