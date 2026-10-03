package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 951 "Custom / Bank Invoice" - Architecture.WinApp.Export.frmCustomInvoice (DocumentTypeId 214,
 * rights name "EximInvoice"). Data layer: Architecture.BLL.Export.ExImInvoice (BLL 0467 / DAL 0519)
 * plus the combo BLLs. Procedures, read from procdure.utf8.sql on 30-Sep-2026:
 *
 *   Sp_Branches_GetAllMethod 'GetAll'                @OrganizationId @CompanyId @Activity                    CommonServices.BrancheServiceBind
 *   Sp_Projects_GetAllMethod 'GetAll'                @OrganizationId @CompanyId @MethodType                  CommonServices.ProjectServiceBind
 *   Sp_ExImInvoice_GetAllMethod 'GenerateDocNo'      @OrganizationId @CompanyId @DocumentTypeId=214 @FinancialYearId @BranchesId @Activity
 *   usp_getLastCustomInvoiceNumber                   @CompanyId @DocumentTypeId=214                          txtLastInvoiceNumber
 *   USP_ExImInvoices_GetForCustomInvoice             @OrganizationId @CompanyId @FinancialYearId [@RecId != 0]  CmbInvoiceNoCI
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport'  @OrganizationId @CompanyId @Activity   customer / notify parties
 *   Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerByCustomerGroupId'   @OrganizationId @CompanyId [@ParentId] [@CustomerGroupIds] @Activity   consignee (ParentId) / sales person ("9")
 *   usp_getFarmingNTrade                             (no parameters)                                          CmbFarmingNTrade
 *   usp_getExportCompaniesByCompanyId                @OrganizationId @CompanyId                              CmbExportCompany (ERP feature 9)
 *   Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll'       @Activity                                               cmbdeliverytermnew (Id, Code, Description)
 *   Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll'      @Activity @OrganizationId @CompanyId                    payment terms (Id, lcOrderTerm, lcOrderTermDesc)
 *   Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId' @OrganizationId @CompanyId @Activity            ports (PortType Loading / Destination)
 *   Sp_Bank_GetAllMethod 'ReadAll'                   @OrganizationId @CompanyId @Activity                    exporter (Home) / importer (Foreign) banks
 *   Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds' @OrganizationId @CompanyId @AppId @AccountTypeIds="3,6,8,10" [@UserId] @Activity   credit account
 *   USP_Item_AllItemsWithModal                       @OrganizationId @CompanyId                              clsGlobalVariables.getGlobalAllItems (ItemTypeOfTypeId not 14 / 17)
 *   Sp_UOMSchedule_GetAllMethod 'ReadByItemID'       @OrganizationId @CompanyId @ItemId @Activity            pack / rate UOM (Id, UOMCode, Equivalent)
 *   USP_CommodityDetailItemCustomerWise_GetAllMethod 'GetRemarks' @OrganizationId @CompanyId @ItemId [@SupplierCustomerId] @Activity   HS Code
 *   usp_GetFINoForCustomInvoice                      @OrganizationId @CompanyId [@RecId > 0]                 dtFiList (Id, FINo, DocumentTypeId, FIBalance)
 *   Sp_Item_GetAllMethod 'GetItemByItemTypeId'       @OrganizationId @CompanyId @LookupTypeIds="14" @Activity  other items
 *   Sp_MultiCurrency_GetAllMethod 'ReadAll'          @OrganizationId @CompanyId @Activity                    cmbfcycode
 *   Sp_InvCropYear_GetAllMethod 'ReadAll'            @OrganizationId @CompanyId @Activity                    CmbCropYear
 *   SP_JobLot_ReadMethod 'GetAll'                    @OrganizationId @CompanyId @Activity                    CmbJobLot
 *   Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll'   @Activity @OrganizationId @CompanyId                    combpcktype
 *   USP_ExportCharges_GetAllMethod 'ComboBind'       @OrganizationId @CompanyId @Activity                    CmbChargesNameChargeDetail
 *   USP_GetERPFeaturesByCompanyId                    @OrganizationId @CompanyId                              features 9 / 15
 *   USP_GetDataForDropDownFromExportInvoice          @OrganizationId @CompanyId                              history combos (ActivityType Customer / Invoice)
 *   USP_ExImInvoice_FormHistory                      @CompanyId @OrganizationId @FinancialYearId @DocumentTypeIds="214" [dates] [@SupplierCustomerId] [@Id] [@DocNoFrom] [@DocNoTo] [@ActionId]
 *   Sp_ExImInvoice_GetAllMethod 'ReadById' + the child activities                                             GetByID
 *   Sp_ExImInvoice_Insert / Sp_ExImInvoice_Update + the five detail inserts                                   Save (DAL SetData; no voucher for 214, no attachments)
 */
@Repository
public class ExportCustomInvoiceRepository {

    private static final String INV = "Sp_ExImInvoice_GetAllMethod";
    private final JdbcTemplate jdbc;

    public ExportCustomInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private Map<String, Object> tenant(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    public String config(UserAccount u, String description) {
        Map<String, Object> p = tenant(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** CommonServices.GetERPFeatureById(id). */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", tenant(u))) {
            Object id = r.get("Id");
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    public List<Map<String, Object>> branches(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "GetAll"); return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", p); }
    public List<Map<String, Object>> projects(UserAccount u) { Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll"); return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", p); }

    /** ExImInvoice.GenerateCode(org, comp, BranchesId, 214, ActiveYr.Id). */
    public List<Map<String, Object>> generateCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", 214);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (u.getBranchesId() != null && u.getBranchesId() != 0) p.put("BranchesId", u.getBranchesId());
        p.put("Activity", "GenerateDocNo");
        return DesktopProc.rows(jdbc, INV, p);
    }

    public List<Map<String, Object>> lastCustomInvoiceNumber(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getLastCustomInvoiceNumber", params("CompanyId", u.getCompanyId(), "DocumentTypeId", 214));
    }

    public List<Map<String, Object>> invoicesForCustomInvoice(UserAccount u, int financialYearId, int recId) {
        Map<String, Object> p = tenant(u);
        p.put("FinancialYearId", financialYearId);
        if (recId != 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoices_GetForCustomInvoice]", p);
    }

    public List<Map<String, Object>> exportCustomers(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "ReadByOrganizationCompanyIdForExport"); return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p); }

    /** SupplierCustomer.GetSupplierustomerByCustomerGroupId(Ids, ParentId). */
    public List<Map<String, Object>> supplierCustomersByGroup(UserAccount u, String groupIds, int parentId) {
        Map<String, Object> p = tenant(u);
        if (parentId != 0) p.put("ParentId", parentId);
        if (groupIds != null && !groupIds.isEmpty()) p.put("CustomerGroupIds", groupIds);
        p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    public List<Map<String, Object>> farmingNTrade() { return DesktopProc.rows(jdbc, "usp_getFarmingNTrade", params()); }
    public List<Map<String, Object>> exportCompanies(UserAccount u) { return DesktopProc.rows(jdbc, "usp_getExportCompaniesByCompanyId", tenant(u)); }
    public List<Map<String, Object>> deliveryTerms() { return DesktopProc.rows(jdbc, "Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll")); }
    public List<Map<String, Object>> paymentTerms(UserAccount u) { return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> seaPorts(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "ReadByCompanyNOrganizationId"); return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", p); }
    public List<Map<String, Object>> banks(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll"); return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod", p); }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,10") - AppId and UserId from the session. */
    public List<Map<String, Object>> creditAccounts(UserAccount u) {
        Map<String, Object> p = tenant(u);
        p.put("AppId", u.getAppId() == null ? 0 : u.getAppId());
        p.put("AccountTypeIds", "3,6,8,10");
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    public List<Map<String, Object>> allItems(UserAccount u) { return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", tenant(u)); }

    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    public List<Map<String, Object>> hsCodes(UserAccount u, int itemId, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        p.put("ItemId", itemId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "GetRemarks");
        return DesktopProc.rows(jdbc, "[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
    }

    public List<Map<String, Object>> fiNos(UserAccount u, int recId) {
        Map<String, Object> p = tenant(u);
        if (recId > 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "usp_GetFINoForCustomInvoice", p);
    }

    public List<Map<String, Object>> otherItems(UserAccount u) { Map<String, Object> p = tenant(u); p.put("LookupTypeIds", "14"); p.put("Activity", "GetItemByItemTypeId"); return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p); }
    public List<Map<String, Object>> currencies(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll"); return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p); }
    public List<Map<String, Object>> cropYears(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll"); return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", p); }
    public List<Map<String, Object>> jobLots(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "GetAll"); return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", p); }
    public List<Map<String, Object>> packTypes(UserAccount u) { return DesktopProc.rows(jdbc, "Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> exportCharges(UserAccount u) { Map<String, Object> p = tenant(u); p.put("Activity", "ComboBind"); return DesktopProc.rows(jdbc, "USP_ExportCharges_GetAllMethod", p); }
    public List<Map<String, Object>> historyDropDowns(UserAccount u) { return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]", tenant(u)); }

    /** ExImInvoice.FormHistory(ReportsParameters) with Ids="214", ApprovedFilter="All" (no @IsApproved). */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, String dateKind, java.sql.Date from, java.sql.Date to,
                                                 int supplierCustomerId, int invoiceId, int fromDocNo, int toDocNo, int actionId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId);
        if ("doc".equals(dateKind)) { if (from != null) p.put("FromDate", from); if (to != null) p.put("ToDate", to); }
        else if ("entry".equals(dateKind)) { if (from != null) p.put("EntryFromDate", from); if (to != null) p.put("EntryToDate", to); }
        else if ("modify".equals(dateKind)) { if (from != null) p.put("ModifyFromDate", from); if (to != null) p.put("ModifyToDate", to); }
        else if ("approved".equals(dateKind)) { if (from != null) p.put("ApprovedFromDate", from); if (to != null) p.put("ApprovedToDate", to); }
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("DocumentTypeIds", "214");
        if (invoiceId != 0) p.put("Id", invoiceId);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (actionId != 0) p.put("ActionId", actionId);
        return DesktopProc.rows(jdbc, "USP_ExImInvoice_FormHistory", p);
    }

    // ------------------------------------------------------------------ ReadById (DAL GetData)
    public List<Map<String, Object>> headerById(int id) { return DesktopProc.rows(jdbc, INV, params("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> packingDetail(int id) { return DesktopProc.rows(jdbc, INV, params("Id", id, "Activity", "ReadExImInvoicePackingDetailByHeaderId")); }
    public List<Map<String, Object>> paymentTermsDetail(int id) { return DesktopProc.rows(jdbc, INV, params("Id", id, "Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId")); }
    public List<Map<String, Object>> otherItemsDetail(int id) { return DesktopProc.rows(jdbc, INV, params("Id", id, "Activity", "ReadExImInvoiceOtherItemsByHeaderId")); }
    public List<Map<String, Object>> otherChargesDetail(int id) { return DesktopProc.rows(jdbc, INV, params("Id", id, "Activity", "ExImInvoiceOtherChargesDetail_ReadByHeaderId")); }
    public List<Map<String, Object>> commissionDetail(int id) { return DesktopProc.rows(jdbc, INV, params("Id", id, "Activity", "EximInvoiceCommissionInfoByReadByHeaderId")); }

    // ------------------------------------------------------------------ save (DAL SetData for DocumentTypeId 214)

    /**
     * Header SetProc (Insert when Id == 0 / Update), then ExImInvoicePackingDetail (LineId 1..n),
     * ExImInvoicePaymentTermsDetail, ExImInvoiceOtherChargesDetail, ExImInvoiceOtherItems and
     * exImInvoiceSaleManCommission, each with ExImInvoiceId = the header id; one transaction.
     * No voucher (Save() builds one only for 204 / 211 / 212) and no attachment rows.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> packing, List<Map<String, Object>> paymentTerms,
                    List<Map<String, Object>> otherCharges, List<Map<String, Object>> otherItems, List<Map<String, Object>> commissions) {
        boolean insert = ((Number) header.get("Id")).intValue() == 0;
        Integer v = DesktopProc.scalar(jdbc, insert ? "Sp_ExImInvoice_Insert" : "Sp_ExImInvoice_Update", header);
        int num = v == null ? 0 : v;
        if (num <= 0) num = ((Number) header.get("Id")).intValue();
        int line = 0;
        for (Map<String, Object> d : packing) { d.put("LineId", ++line); d.put("ExImInvoiceId", num); DesktopProc.scalar(jdbc, "Sp_ExImInvoicePackingDetail_Insert", d); }
        for (Map<String, Object> d : paymentTerms) { d.put("ExImInvoiceId", num); DesktopProc.scalar(jdbc, "Sp_ExImInvoicePaymentTermsDetail_Insert", d); }
        for (Map<String, Object> d : otherCharges) { d.put("ExImInvoiceId", num); DesktopProc.scalar(jdbc, "USP_ExImInvoiceOtherChargesDetail_Insert", d); }
        for (Map<String, Object> d : otherItems) { d.put("ExImInvoiceId", num); DesktopProc.scalar(jdbc, "Sp_ExImInvoiceOtherItems_Insert", d); }
        for (Map<String, Object> d : commissions) { d.put("exImInvoiceId", num); DesktopProc.scalar(jdbc, "USP_exImInvoiceSaleManCommission_Insert", d); }
        return num;
    }
}
