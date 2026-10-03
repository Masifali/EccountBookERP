package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of the two DocumentTypeId 204 commercial-invoice forms of the Export application:
 *
 *   211  Architecture.WinApp.Export.ExImCommercialInvoice   "Export Commercial Invoice"   (/export/commercial-invoice)
 *   880  Architecture.WinApp.Export.frmCommercialInvoiceIII "Commercial Invoice III"     (/export/commercial-invoice-iii)
 *
 * Both save through Architecture.BLL.Export.ExImInvoice.Save (BLL 0467:352) -> DAL ExImInvoice.SetData
 * (DAL 0519:19). Every procedure and parameter below was read from procdure.utf8.sql / procdure_index.csv
 * (01-Oct-2026); GuardED parameters (BLL `if (x != 0)`) are only added when set, and a null value is never
 * bound (ADO.NET AddWithValue(null) leaves the parameter out - DesktopProc does the same).
 *
 *  Reads
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   configuration keys
 *   USP_GetERPFeaturesByCompanyId                    @OrganizationId @CompanyId                    features 9 / 15
 *   Sp_ExImInvoice_GetAllMethod 'GenerateDocNo'      @OrganizationId @CompanyId @DocumentTypeId [@FinancialYearId] [@BranchesId]
 *   Sp_ExImInvoice_GetAllMethod 'GenerateInvoiceNo'  @OrganizationId @CompanyId @DocumentTypeId [@FinancialYearId]   (211, config CommercialInvoicePrefix)
 *   USP_ExportInvoiceNos_GetAllMethod 'GetFinalInvoiceNosForCommercialInvoice' @OrganizationId @CompanyId [@InvoiceNo]  (880 Invoice No combo)
 *   Sp_Projects_GetAllMethod 'GetAll' / Sp_Branches_GetAllMethod 'GetAll'                           211 hidden branch / project combos
 *   [dbo].[USP_GetDataForDropDownFromExportContract] @OrganizationId @CompanyId [@Activity]          211 customer ('Customer'), loader filters (no activity)
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport' / 'GetSupplierustomerByCustomerGroupId' [@ParentId] [@CustomerGroupIds]
 *   USP_GetVendorsAndCustomersWithCityName           @OrganizationId @CompanyId                    880 clsGlobalVariables.globalAllSupplierCustomer
 *   Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll', Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll', Sp_SeaPorts_GetAllMethod
 *     'ReadByCompanyNOrganizationId', Sp_Bank_GetAllMethod 'ReadAll', Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds',
 *     Sp_Item_GetAllMethod 'GetItemByItemTypeId' @LookupTypeIds='14', Sp_MultiCurrency_GetAllMethod 'ReadAll',
 *     [dbo].[usp_getMultiCurrencywithExchangeRate] @CompanyId (880), Sp_InvCropYear_GetAllMethod 'ReadAll',
 *     SP_JobLot_ReadMethod 'GetAll' (211) / 'GetJobLotGlIdsandName' (880 global), Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll',
 *     USP_ExportCharges_GetAllMethod 'ComboBind', usp_getFarmingNTrade, usp_getExportCompaniesByCompanyId,
 *     USP_Continent_GetAllMethod 'ComboBind', SP_Country_ReadMethod 'GetAll', [dbo].[USP_City_GetAllWithCountryAndTehsil],
 *     [dbo].[USP_Item_AllItemsWithModal], [dbo].[usp_getBrands], usp_getAllUomsByCompanyId, Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   [dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod] 'GetRemarks' @ItemId [@SupplierCustomerId]   HS code (211) / commodity + pm detail (880)
 *   usp_GetFINoForInvoice                            @OrganizationId @CompanyId [@SupplierCustomerId]  FI combo (Id, EFormNo, PaymenttermId, DocumentTypeId)
 *   Sp_ExImEFormRegistration_GetAllMethod 'GetFinancialInstrumentsBalance' @DocumentTypeId @Id      FI balance
 *   USP_GetContractScheduleByContractId              @OrganizationId @CompanyId @ContractId [@InvoiceId]   211 detail edit
 *   [dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId] @OrganizationId @CompanyId @InvoiceId   880
 *   [dbo].[USP_InvLabPreProductionExportLotInspectionHeader_GetByItemId] @OrganizationId @CompanyId @FinancialYearId @ItemId   880 F1
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice]  @OrganizationId @CompanyId                    history combos
 *   USP_ExImInvoice_FormHistory                      (BLL FormHistory, @DocumentTypeIds='204')      history grid
 *   Sp_ExImInvoice_GetAllMethod 'ReadById' + 10 child activities                                     GetByID (DAL GetData)
 *   USP_GetProformaDataForInvoices                   LoadSalesContractForInvoice (schedule wise)
 *   USP_GetOtherItemsFromLcorder @OrderIds, Sp_ExImLcOrder_GetAllMethod 'LcOrderPaymentTermsDetailByLcOrderIds' /
 *     'ExImLcOrderOtherChargesDetailByLcOrderIds' @Ids                                              BindLoaderData
 *   SELECT Start_Period FROM FinancialYear                                                          clsGlobalVariables.ActiveYr.Start_Period
 *
 *  Save (DAL SetData, one transaction, rolled back on any error)
 *   Sp_ExImInvoice_Insert | Sp_ExImInvoice_Update, Sp_ExImInvoicePackingDetail_Insert (LineId 1..n),
 *   Sp_ExImInvoicePaymentTermsDetail_Insert, USP_ExImInvoiceOtherChargesDetail_Insert, Sp_ExImInvoiceOtherItems_Insert,
 *   USP_ExImInvoicePackingDetailCustom_Insert, USP_ExImInvoicePaymentTermsDetailCustom_Insert,
 *   USP_ExImInvoiceOtherChargesDetailCustom_Insert, USP_exImInvoiceSaleManCommission_Insert,
 *   sdt.usp_ShipmentDocumentSchedule_UpdateByScheduleId (DocumentTypeId 204, once per distinct schedule),
 *   then the voucher (BLL MakeVoucherForExImInvoice, built only when FinancialisActiveonCommercialInvoice is on for 204):
 *   Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', Sp_VoucherHead_Insert | _Update,
 *   Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert.
 *   Attachment procedures are not called (attachments are not part of this web port).
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportCommercialInvoiceRepository {

    private static final String INV = "Sp_ExImInvoice_GetAllMethod";
    private final JdbcTemplate jdbc;

    public ExportCommercialInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }
    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    private List<Map<String, Object>> rowsA(UserAccount u, String proc, String activity) { Map<String, Object> p = tenant(u); p.put("Activity", activity); return rows(proc, p); }

    // ================================================================== configuration / features

    public String config(UserAccount u, String description) {
        Map<String, Object> p = tenant(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", tenant(u))) {
            Object id = ci(r, "Id");
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (LoadSalesContractForInvoice From Date). */
    public Object financialYearStart(int financialYearId) {
        if (financialYearId == 0) return null;
        List<Map<String, Object>> r = jdbc.queryForList("SELECT Start_Period FROM FinancialYear WHERE Id = ?", financialYearId);
        return r.isEmpty() ? null : ci(r.get(0), "Start_Period");
    }

    // ================================================================== numbering

    /** ExImInvoice.GenerateCode -> DocNo. */
    public int generateDocNo(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        int b = u.getBranchesId() == null ? 0 : u.getBranchesId();
        if (b != 0) p.put("BranchesId", b);
        p.put("Activity", "GenerateDocNo");
        List<Map<String, Object>> r = rows(INV, p);
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "DocNo"));
    }

    /** ExImInvoice.GenerateInvoiceNo -> InvoiceNo. */
    public String generateInvoiceNo(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateInvoiceNo");
        List<Map<String, Object>> r = rows(INV, p);
        Object v = r.isEmpty() ? null : ci(r.get(0), "InvoiceNo");
        return v == null ? "" : String.valueOf(v);
    }

    /** GenerateExportInvoiceNos.GetFinalInvoiceNosForCommercialInvoice(FinalInvoiceNo) - Id, FinalInvoiceNo. */
    public List<Map<String, Object>> finalInvoiceNos(UserAccount u, String invoiceNo) {
        Map<String, Object> p = tenant(u);
        if (invoiceNo != null && !invoiceNo.isEmpty()) p.put("InvoiceNo", invoiceNo);
        p.put("Activity", "GetFinalInvoiceNosForCommercialInvoice");
        return rows("[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
    }

    // ================================================================== combos

    public List<Map<String, Object>> branches(UserAccount u) { return rowsA(u, "Sp_Branches_GetAllMethod", "GetAll"); }
    public List<Map<String, Object>> projects(UserAccount u) { Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll"); return rows("Sp_Projects_GetAllMethod", p); }

    /** ExImLcOrder.GetDataForDropDownFromExportContract - @Activity only when given. */
    public List<Map<String, Object>> exportContractDropDown(UserAccount u, String activity) {
        Map<String, Object> p = tenant(u);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return rows("[dbo].[USP_GetDataForDropDownFromExportContract]", p);
    }

    public List<Map<String, Object>> exportCustomers(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "ReadByOrganizationCompanyIdForExport"); }

    /** CommonServices.GetSupplierustomerByCustomerGroupId(Ids, ParentId). */
    public List<Map<String, Object>> supplierCustomersByGroup(UserAccount u, String groupIds, int parentId) {
        Map<String, Object> p = tenant(u);
        if (parentId != 0) p.put("ParentId", parentId);
        if (groupIds != null && !groupIds.isEmpty()) p.put("CustomerGroupIds", groupIds);
        p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** GlobalServicesMethods.getGlobalSupplierCustomer (no paging) - clsGlobalVariables.globalAllSupplierCustomer. */
    public List<Map<String, Object>> globalSupplierCustomers(UserAccount u) { return rows("USP_GetVendorsAndCustomersWithCityName", tenant(u)); }

    public List<Map<String, Object>> deliveryTerms() { return rows("Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll")); }
    public List<Map<String, Object>> paymentTerms(UserAccount u) { return rows("Sp_ExImLcPaymentTerm_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> seaPorts(UserAccount u) { return rowsA(u, "Sp_SeaPorts_GetAllMethod", "ReadByCompanyNOrganizationId"); }
    public List<Map<String, Object>> banks(UserAccount u) { return rowsA(u, "Sp_Bank_GetAllMethod", "ReadAll"); }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,10"). */
    public List<Map<String, Object>> creditAccounts(UserAccount u) {
        Map<String, Object> p = tenant(u);
        p.put("AppId", u.getAppId() == null ? 0 : u.getAppId());
        p.put("AccountTypeIds", "3,6,8,10");
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    public List<Map<String, Object>> otherItems(UserAccount u) { Map<String, Object> p = tenant(u); p.put("LookupTypeIds", "14"); p.put("Activity", "GetItemByItemTypeId"); return rows("Sp_Item_GetAllMethod", p); }
    public List<Map<String, Object>> currencies(UserAccount u) { return rowsA(u, "Sp_MultiCurrency_GetAllMethod", "ReadAll"); }
    public List<Map<String, Object>> currenciesWithRate(UserAccount u) { return rows("[dbo].[usp_getMultiCurrencywithExchangeRate]", params("CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> cropYears(UserAccount u) { return rowsA(u, "Sp_InvCropYear_GetAllMethod", "ReadAll"); }
    public List<Map<String, Object>> jobLots(UserAccount u) { return rowsA(u, "SP_JobLot_ReadMethod", "GetAll"); }
    public List<Map<String, Object>> jobLotsGlobal(UserAccount u) { return rowsA(u, "[dbo].[SP_JobLot_ReadMethod]", "GetJobLotGlIdsandName"); }
    public List<Map<String, Object>> packTypes(UserAccount u) { return rows("Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> exportCharges(UserAccount u) { return rowsA(u, "USP_ExportCharges_GetAllMethod", "ComboBind"); }
    public List<Map<String, Object>> farmingNTrade() { return rows("usp_getFarmingNTrade", params()); }
    public List<Map<String, Object>> exportCompanies(UserAccount u) { return rows("usp_getExportCompaniesByCompanyId", tenant(u)); }
    public List<Map<String, Object>> continents() { return rows("USP_Continent_GetAllMethod", params("Activity", "ComboBind")); }
    public List<Map<String, Object>> countries(UserAccount u) { Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll"); return rows("SP_Country_ReadMethod", p); }
    public List<Map<String, Object>> cities(UserAccount u) { return rows("[dbo].[USP_City_GetAllWithCountryAndTehsil]", tenant(u)); }
    public List<Map<String, Object>> allItems(UserAccount u) { return rows("[dbo].[USP_Item_AllItemsWithModal]", tenant(u)); }
    public List<Map<String, Object>> brands(UserAccount u) { return rows("[dbo].[usp_getBrands]", tenant(u)); }

    /** GlobalServicesMethods.getAllUomsByCompanyId (the global UOM schedule; the form filters it by ItemId). */
    public List<Map<String, Object>> allUoms(UserAccount u) { return rows("usp_getAllUomsByCompanyId", tenant(u)); }

    /** CommonServices.GetUomScheduleByItemId - Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return rows("Sp_UOMSchedule_GetAllMethod", p);
    }

    /** CommodityDetailItemCustomerWise.GetRemarks(ItemId, SupplierCustomerId) - @SupplierCustomerId only when non-zero. */
    public List<Map<String, Object>> commodityRemarks(UserAccount u, int itemId, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        p.put("ItemId", itemId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "GetRemarks");
        return rows("[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
    }

    /** ExImEFormRegistration.GetFINoForInvoice(SupplierCustomerId). */
    public List<Map<String, Object>> fiNosForInvoice(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return rows("usp_GetFINoForInvoice", p);
    }

    /** ExImEFormRegistration.GetFinancialInstrumentsBalance -> first cell of the first row. */
    public Object fiBalance(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Id", id);
        p.put("Activity", "GetFinancialInstrumentsBalance");
        List<Map<String, Object>> r = rows("Sp_ExImEFormRegistration_GetAllMethod", p);
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        return r.get(0).values().iterator().next();
    }

    /** ExImLcOrder.GetContractScheduleByContractId(ContractId, ExImInvoiceId = RecId). */
    public List<Map<String, Object>> contractSchedules(UserAccount u, int contractId, int invoiceId) {
        Map<String, Object> p = tenant(u);
        p.put("ContractId", contractId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        return rows("USP_GetContractScheduleByContractId", p);
    }

    /** ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(InvoiceId, 0). */
    public List<Map<String, Object>> containers(UserAccount u, int invoiceId) {
        Map<String, Object> p = tenant(u);
        p.put("InvoiceId", invoiceId);
        return rows("[dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId]", p);
    }

    /** InvLabPreProductionExportLotInspectionHeader.LabPreProductionExportLotInspectionHeader_GetByItemId(.., FY, ItemId, 0). */
    public List<Map<String, Object>> thirdPartyInspections(UserAccount u, int financialYearId, int itemId) {
        Map<String, Object> p = tenant(u);
        p.put("FinancialYearId", financialYearId);
        p.put("ItemId", itemId);
        return rows("[dbo].[USP_InvLabPreProductionExportLotInspectionHeader_GetByItemId]", p);
    }

    // ================================================================== history

    public List<Map<String, Object>> historyDropDowns(UserAccount u) { return rows("[dbo].[USP_GetDataForDropDownFromExportInvoice]", tenant(u)); }

    /** ExImInvoice.FormHistory(ReportsParameters) - every parameter guarded as the BLL guards it; ApprovedFilter "All" (no @IsApproved). */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, String documentTypeIds, String dateKind,
                                                 java.sql.Date from, java.sql.Date to, int supplierCustomerId, int invoiceId,
                                                 int fromDocNo, int toDocNo, int actionId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId);
        if ("doc".equals(dateKind)) { if (from != null) p.put("FromDate", from); if (to != null) p.put("ToDate", to); }
        else if ("entry".equals(dateKind)) { if (from != null) p.put("EntryFromDate", from); if (to != null) p.put("EntryToDate", to); }
        else if ("modify".equals(dateKind)) { if (from != null) p.put("ModifyFromDate", from); if (to != null) p.put("ModifyToDate", to); }
        else if ("approved".equals(dateKind)) { if (from != null) p.put("ApprovedFromDate", from); if (to != null) p.put("ApprovedToDate", to); }
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("DocumentTypeIds", documentTypeIds);
        if (invoiceId != 0) p.put("Id", invoiceId);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (actionId != 0) p.put("ActionId", actionId);
        return rows("USP_ExImInvoice_FormHistory", p);
    }

    // ================================================================== GetByID (DAL ExImInvoice.GetData)

    public List<Map<String, Object>> headerById(int id) { return rows(INV, params("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> child(int id, String activity) { return rows(INV, params("Id", id, "Activity", activity)); }

    // ================================================================== loader (LoadSalesContractForInvoice)

    /** ExImInvoice.GetProformaDataForInvoices - schedule wise (radScheduleWise is the checked, only visible option). */
    public List<Map<String, Object>> proformaDataForInvoices(UserAccount u, int financialYearId, int documentTypeId,
                                                             Timestamp from, Timestamp to, int supplierCustomerId, int fcyId,
                                                             int itemId, String scheduleIds) {
        Map<String, Object> p = tenant(u);
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", String.valueOf(documentTypeId));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (fcyId != 0) p.put("FcurrencyId", fcyId);
        if (itemId != 0) p.put("ItemId", itemId);
        p.put("SkipZero", 1);                                   // ZeroBalanceType = 1
        if (scheduleIds != null && !scheduleIds.isEmpty()) p.put("ScheduleIds", scheduleIds);
        return rows("USP_GetProformaDataForInvoices", p);
    }

    public List<Map<String, Object>> otherItemsFromLcOrder(UserAccount u, String ids) {
        Map<String, Object> p = tenant(u); p.put("OrderIds", ids);
        return rows("USP_GetOtherItemsFromLcorder", p);
    }
    public List<Map<String, Object>> lcOrderPaymentTerms(String ids) { return rows("Sp_ExImLcOrder_GetAllMethod", params("Ids", ids, "Activity", "LcOrderPaymentTermsDetailByLcOrderIds")); }
    public List<Map<String, Object>> lcOrderOtherCharges(String ids) { return rows("Sp_ExImLcOrder_GetAllMethod", params("Ids", ids, "Activity", "ExImLcOrderOtherChargesDetailByLcOrderIds")); }

    // ================================================================== voucher look-ups (BLL MakeVoucherForExImInvoice)

    /** CommonServies.GetSupplierCustomerListForFinancialEffects -> Id, GlAccountId. */
    public List<Map<String, Object>> supplierCustomerGl(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "GetGlAccountIdandCompanyNameBySupplierCustomerId"); }
    /** CommonServies.GetItemListForFinancialEffects -> Id, SaleGLAC. */
    public List<Map<String, Object>> itemGl(UserAccount u) { return rowsA(u, "Sp_Item_GetAllMethod", "GetItemGlIdsandItemName"); }

    public int voucherHeadId(int org, int company, int documentTypeId, int id) {
        List<Map<String, Object>> r = rows("Sp_Vouchers_GetMethods", params("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org, "CompanyId", company, "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id));
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "Id"));
    }

    // ================================================================== save (DAL ExImInvoice.SetData)

    /** Every child list of the invoice, in the order DAL SetData writes them. */
    public static class SaveSet {
        public List<Map<String, Object>> packing = new ArrayList<>();
        public List<Map<String, Object>> paymentTerms = new ArrayList<>();
        public List<Map<String, Object>> otherCharges = new ArrayList<>();
        public List<Map<String, Object>> otherItems = new ArrayList<>();
        public List<Map<String, Object>> packingCustom = new ArrayList<>();
        public List<Map<String, Object>> paymentTermsCustom = new ArrayList<>();
        public List<Map<String, Object>> otherChargesCustom = new ArrayList<>();
        public List<Map<String, Object>> commissions = new ArrayList<>();
        public SaleInvoiceModels.VoucherHead voucherHead;
        public List<SaleInvoiceModels.VoucherDetail> voucherDetails = new ArrayList<>();
    }

    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, SaveSet s) {
        int org = asInt(header.get("OrganizationId")), company = asInt(header.get("CompanyId"));
        int documentTypeId = asInt(header.get("DocumentTypeId"));
        boolean insert = asInt(header.get("Id")) == 0;
        Integer v = DesktopProc.scalar(jdbc, insert ? "Sp_ExImInvoice_Insert" : "Sp_ExImInvoice_Update", header);
        int num3 = v == null ? 0 : v;
        int id;
        if (num3 > 0) id = num3; else { id = asInt(header.get("Id")); num3 = id; }

        int line = 0;
        for (Map<String, Object> d : s.packing) { d.put("LineId", ++line); d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "Sp_ExImInvoicePackingDetail_Insert", d); }
        for (Map<String, Object> d : s.paymentTerms) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "Sp_ExImInvoicePaymentTermsDetail_Insert", d); }
        for (Map<String, Object> d : s.otherCharges) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "USP_ExImInvoiceOtherChargesDetail_Insert", d); }
        for (Map<String, Object> d : s.otherItems) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "Sp_ExImInvoiceOtherItems_Insert", d); }
        line = 0;
        for (Map<String, Object> d : s.packingCustom) { d.put("LineId", ++line); d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "USP_ExImInvoicePackingDetailCustom_Insert", d); }
        for (Map<String, Object> d : s.paymentTermsCustom) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "USP_ExImInvoicePaymentTermsDetailCustom_Insert", d); }
        for (Map<String, Object> d : s.otherChargesCustom) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "USP_ExImInvoiceOtherChargesDetailCustom_Insert", d); }
        for (Map<String, Object> d : s.commissions) { d.put("exImInvoiceId", id); DesktopProc.scalar(jdbc, "USP_exImInvoiceSaleManCommission_Insert", d); }

        /* DAL 0519:160 - DocumentTypeId 204: shipment document schedule update per distinct ContractScheduleId of
           the rows that are not removed (ActionTypeId != 3). */
        if (documentTypeId == 204) {
            Set<Integer> schedules = new LinkedHashSet<>();
            for (Map<String, Object> d : s.packing) if (asInt(d.get("ActionTypeId")) != 3) schedules.add(asInt(d.get("ContractScheduleId")));
            for (Integer sid : schedules) {
                ProcExec.run(jdbc, "EXEC sdt.usp_ShipmentDocumentSchedule_UpdateByScheduleId @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @ScheduleId=?",
                        org, company, documentTypeId, id, sid);
            }
        }

        /* DAL 0519:183 - the voucher, when MakeVoucherForExImInvoice produced lines. */
        SaleInvoiceModels.VoucherHead vh = s.voucherHead;
        if (vh != null && !s.voucherDetails.isEmpty()) {
            int existing = voucherHeadId(org, company, documentTypeId, id);
            if (existing > 0) vh.Id = existing;
            vh.DocumentTypeSrNo = id;
            int num = setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
            if (num > 0) vh.Id = num; else num = vh.Id;
            for (SaleInvoiceModels.VoucherDetail d : s.voucherDetails) { d.VoucherHeadId = vh.Id; setProc("Sp_VoucherDetail_Insert", d); }
            if (vh.Id > 0) ProcExec.run(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
            int documentTypeIdRef = setProc("Sp_VoucherHead_H_Insert", vh);
            for (SaleInvoiceModels.VoucherDetail d : s.voucherDetails) { d.VoucherHeadId = vh.Id; d.DocumentTypeIdRef = documentTypeIdRef; setProc("Sp_VoucherDetail_H_Insert", d); }
        }
        return num3;
    }

    /**
     * GenericProvider.SetProc over a model object: every public non-static field not marked @NotParam becomes @Name;
     * a null is not sent (a non-nullable DateTime left unset fails by name, as "SqlDateTime overflow" does on the desktop).
     */
    private int setProc(String proc, Object model) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.contains(".") ? proc : "dbo." + proc).append(' ');
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Field f : model.getClass().getFields()) {
            if (Modifier.isStatic(f.getModifiers())) continue;
            if (f.isAnnotationPresent(SaleInvoiceModels.NotParam.class)) continue;
            Object val;
            try { val = f.get(model); } catch (IllegalAccessException e) { continue; }
            if (val == null) {
                if (f.getType() == LocalDateTime.class && !f.isAnnotationPresent(SaleInvoiceModels.Nullable.class))
                    throw new IllegalStateException("SqlDateTime overflow: " + model.getClass().getSimpleName() + "." + f.getName() + " was not set");
                continue;
            }
            Object bound;
            if (val instanceof LocalDateTime) bound = new SqlParameterValue(Types.TIMESTAMP, Timestamp.valueOf((LocalDateTime) val));
            else if (val instanceof BigDecimal) bound = new SqlParameterValue(Types.DECIMAL, val);
            else if (val instanceof Double) bound = new SqlParameterValue(Types.DOUBLE, val);
            else if (val instanceof Boolean) bound = new SqlParameterValue(Types.BIT, val);
            else if (val instanceof Integer) bound = new SqlParameterValue(Types.INTEGER, val);
            else bound = new SqlParameterValue(Types.NVARCHAR, String.valueOf(val));
            sql.append(first ? "" : ", ").append('@').append(f.getName()).append("=?");
            args.add(bound);
            first = false;
        }
        Integer r = ProcExec.call(jdbc, sql.toString(), args.toArray());
        return r == null ? 0 : r;
    }

    // ================================================================== plumbing

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
