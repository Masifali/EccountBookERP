package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 346 "Purchase Invoice Return Store" ({@code PurchaseInvoiceReturn_Store.cs}, DocumentTypeId 145)
 * — every read and write the form and its BLL/DAL make, one method per desktop call.
 *
 * Procedures, activities and parameters were read from the recovered desktop source:
 *
 *   BLL 0580 / DAL 0433  InvSaleInvoice            GenerateCode, ReadById (+ the five child reads),
 *                                                  FormHistory, Usp_AllComboAgainstSaleInvoice, SetData
 *   BLL 0581             InvPurchaseInvoice        USP_GetPartiesAndItemFromPurchaseInvoiceStoreOrPM,
 *                                                  USP_PendingPurchaseInvoiceStoreForReturnInvoice,
 *                                                  USP_GetBranchsAllocatedToUserFromPurchaseInvoice
 *   BLL 0612 / 0613      SaleInvoiceFinancial      (voucher build — the reads it makes are here)
 *   BLL 0267             CommonServies             configuration, ERP features, item / party GL lists,
 *                                                  GetAvgRateFromAvaiblableLoaderStock
 *   BLL 0594             jobLot.GetByID            SP_JobLot_ReadMethod 'GetById'
 *   BLL 0006             Reasons                   USP_Reasons_GetAllMethod / _Insert / _Update
 *   BLL 0070             DocumentType.FormHistory  USP_DocumentType_GetAllMethod
 *   BLL 0076             MultiCurrency.GetAll      Sp_MultiCurrency_GetAllMethod 'ReadAll'
 *   BLL 0592             ItemTaxSchedule           Sp_ItemTaxSchedule_GetAllMethod 'GetAllTaxSchedule'
 *   BLL 0573             InventoryItemsOther       Sp_InventoryItemsOther_GetAllMethod 'ReadAll'
 *   BLL 0574             GetCurrentStockByItemId   Sp_SaleOrder_GetAllMethod 'GetCurrentStockByItemId'
 *   BLL 0610             UOMSchedule.SearchByObject Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   BLL 0600             GetVendorsAndCustomers    USP_GetVendorsAndCustomers
 *   BLL 0648             COAAllocation             Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'
 *   BLL 0654             VoucherHead               Sp_Vouchers_GetMethods 'GetMultiCurrencyAndLastRate'
 *   BLL 0379             GlobalServicesMethods     payment terms, cities, UOM schedule, item conditions,
 *                                                  accounts with custom group, all items
 *   BLL 0128             InvSaleInvoiceReports     USP_PurchaseInvoiceReturn_StoreSip (+ its sub-report)
 *   BLL 0141             VoucherReports            Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt
 *
 * A parameter the desktop only adds conditionally is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET's AddWithValue(null) does).
 */
@Repository
public class PurchaseInvoiceReturnStoreRepository {

    public static final String P_GETALL = "[Sp_InvSaleInvoice_GetAllMethod]";

    private final JdbcTemplate jdbc;
    public PurchaseInvoiceReturnStoreRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================ globals

    /** CommonServices.GetERPFeatureById / BLL 0267 GetERPFeatureById — USP_GetERPFeaturesByCompanyId. */
    public Set<Integer> features(UserAccount u) {
        Set<Integer> out = new HashSet<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            out.add(toInt(r.get("Id")));
        }
        return out;
    }

    /**
     * GetConfigurationFromAllocation / GetConfigValueFromGlobal — the ConfigKey text, or null when
     * the company has no row for that description (GridItemNetAmountCalculation:3848 tells the two apart).
     */
    public String configRaw(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return null;
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    /** BLL 0580 GenerateInvSaleInvoiceCode — DocNo (BranchesId is set on the object but never sent). */
    public int generateCode(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "FinancialYearId", financialYearId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** BLL 0581 GetPartiesAndItemFromPurchaseInvoiceStoreOrPM (:769 passes "58,61,63,64,131,1604,1603"). */
    public List<Map<String, Object>> partiesAndItems(UserAccount u, String documentTypeIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetPartiesAndItemFromPurchaseInvoiceStoreOrPM]", p);
    }

    /** clsGlobalVariables.globalPaymentTerm — BLL 0379 getPaymentTermlist. */
    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvDueTerms_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** clsGlobalVariables.AllAccountsWithCustomGroupId — BLL 0379 (PageSize/PageNumber 0 and Keyword "" are not sent). */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** BLL 0592 GetAllTaxSchedule(org, comp, EffectedDate, TaxTypeId). */
    public List<Map<String, Object>> taxSchedule(UserAccount u, Timestamp effectedDate, int taxTypeId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "EffectedDate", effectedDate);
        if (taxTypeId != 0) p.put("TaxTypeId", taxTypeId);
        p.put("Activity", "GetAllTaxSchedule");
        return DesktopProc.rows(jdbc, "Sp_ItemTaxSchedule_GetAllMethod", p);
    }

    /** clsGlobalVariables.globalItemConditions — BLL 0379 GetItemCondtions: every V_ItemCondition row, no filter. */
    public List<Map<String, Object>> itemConditions() {
        return jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition");
    }

    /** clsGlobalVariables.globalJobLot — DAL 0205 GetJobLotGlIdsandName. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[SP_JobLot_ReadMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetJobLotGlIdsandName"));
    }

    /** BLL 0594 jobLot.GetByID — {@code GetData(...)[0]}: no row is an index error on the desktop. */
    public Map<String, Object> jobLotById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params("Id", id, "Activity", "GetById"));
        if (r.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        return r.get(0);
    }

    /** clsGlobalVariables.globalAllCities — BLL 0379 getGlobalAllCity. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_City_GetAllWithCountryAndTehsil]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.globalUomSchedule — getAllUomsByCompanyId(org, comp, 0, 1). */
    public List<Map<String, Object>> uoms(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1));
    }

    /** clsGlobalVariables.getGlobalAllItems — BLL 0379 AllItemsWithModal(org, comp, 0, 0, ""). */
    public Map<Integer, Integer> itemParentCategories(UserAccount u) {
        Map<Integer, Integer> out = new HashMap<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            out.putIfAbsent(toInt(r.get("Id")), toInt(r.get("InventoryParentCategoriesId")));
        }
        return out;
    }

    /** BLL 0076 MultiCurrency.GetAll. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** BLL 0573 InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params(
                "Activity", "ReadAll", "organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** BLL 0600 SupplierCustomer.GetVendorsAndCustomers(org, comp, PartyTypeId). */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u, int partyTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (partyTypeId != 0) p.put("PartyTypeId", partyTypeId);
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomers", p);
    }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds → BLL 0648 GetAccountTitleByAccountTypeIds. */
    public List<Map<String, Object>> accountsByTypeIds(UserAccount u, int appId, String typeIds, String typeIdsNot) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "AppId", appId);
        if (typeIds != null && !typeIds.isEmpty()) p.put("AccountTypeIds", typeIds);
        if (typeIdsNot != null && !typeIdsNot.isEmpty()) p.put("AccountTypeIdsNot", typeIdsNot);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /** BLL 0006 Reasons.GetByRefDocumentType. */
    public List<Map<String, Object>> reasonsByRefDocumentType(int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Reasons_GetAllMethod]", params(
                "RefDocumentTypeId", refDocumentTypeId, "Activity", "GetByRefDocumentType"));
    }

    /** BLL 0006 Reasons.FormHistory. */
    public List<Map<String, Object>> reasonsHistory() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Reasons_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** BLL 0006 Reasons.GetByID — null when no row. */
    public Map<String, Object> reasonById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[USP_Reasons_GetAllMethod]", params(
                "ReasonId", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** BLL 0070 DocumentType.FormHistory. */
    public List<Map<String, Object>> documentTypes() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DocumentType_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** BLL 0580 AllComboBindAgainstSaleInvoice (DocumentTypeIds "145"; Activity / CostCenterId not set). */
    public List<Map<String, Object>> historyCombos(UserAccount u, int appId, int userId, String documentTypeIds) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "AppId", appId,
                "UserId", userId);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstSaleInvoice]", p);
    }

    // ================================================================================ entry bar

    /**
     * BLL 0574 GetCurrentStockByItemId — the StoreRackId the form sets is never sent; CropYear is a
     * null string (omitted); PackingTypeId is 0 and IS sent.
     */
    public double currentStock(UserAccount u, int itemId, Timestamp docDate, int warehouseId, int jobLotId, int uomId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_SaleOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDateTo", docDate,
                "WareHouseId", warehouseId,
                "JobLotId", jobLotId,
                "InvPackingTypeId", 0,
                "ItemUomId", uomId,
                "Activity", "GetCurrentStockByItemId"));
        return r.isEmpty() ? 0d : StoreIssuanceRepository.toDouble(r.get(0).get("AvailableStock"));
    }

    /** BLL 0654 GetLastExchangeRateAndCurrencyOfVoucher (cmbCurrency_Leave:1282). */
    public List<Map<String, Object>> lastExchangeRate(UserAccount u, String documentTypeIds, String currencyIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (currencyIds != null && !currencyIds.isEmpty()) p.put("DMultiCurrencyIds", currencyIds);
        p.put("Activity", "GetMultiCurrencyAndLastRate");
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", p);
    }

    /** CommonServices.GetUomScheduleByItemId → BLL 0610 SearchByObject. */
    public List<Map<String, Object>> uomScheduleByItem(UserAccount u, int itemId) {
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    // ================================================================================ loader

    /** frmPendingPurchaseInvoiceForReturn.BranchesFill — BLL 0581 (DocumentTypeId 0 is not sent). */
    public List<Map<String, Object>> loaderBranches(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()));
    }

    /** frmPendingPurchaseInvoiceForReturn.PendingGdnLoad → BLL 0581 PendingPurchaseInvoiceStoreForReturnInvoice. */
    public List<Map<String, Object>> pendingInvoices(UserAccount u, int financialYearId, Timestamp from, Timestamp to,
                                                     String branchesIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "USP_PendingPurchaseInvoiceStoreForReturnInvoice", p);
    }

    // ================================================================================ reference checks (web)

    /**
     * The purchase-invoice line a return row points at — scoped to the session organization and company.
     * Not a desktop call: the web re-reads what the desktop trusted from its loader (service D7).
     */
    public Map<String, Object> purchaseInvoiceLine(UserAccount u, int invoiceId, int detailId) {
        List<Map<String, Object>> r = jdbc.queryForList(
                "SELECT h.Id, h.DocumentTypeId, h.SupplierCustomerId, d.Id AS DetailId, d.ItemId, d.ItemQty "
              + "FROM dbo.InvPurchaseInvoice h INNER JOIN dbo.InvPurchaseInvoiceDetail d ON d.InvPurchaseInvoiceId = h.Id "
              + "WHERE h.Id = ? AND d.Id = ? AND h.OrganizationId = ? AND h.CompanyId = ?",
                invoiceId, detailId, u.getOrganizationId(), u.getCompanyId());
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * Quantity already returned against that line by OTHER documents — the same InvSaleInvoiceDetail sum
     * USP_PendingPurchaseInvoiceStoreForReturnInvoice subtracts, with this document excluded (update add-back).
     */
    public double returnedQtyElsewhere(int documentTypeId, int invoiceId, int detailId, int excludeInvSaleInvoiceId) {
        Object v = jdbc.queryForObject(
                "SELECT ISNULL(SUM(ItemQty), 0) FROM dbo.InvSaleInvoiceDetail "
              + "WHERE RefRefDocumentTypeId = ? AND RefRefDocIdNo = ? AND RefDocSubId = ? AND InvSaleInvoiceId <> ?",
                Object.class, documentTypeId, invoiceId, detailId, excludeInvSaleInvoiceId);
        return StoreIssuanceRepository.toDouble(v);
    }

    // ================================================================================ read

    /** BLL 0580 GetByID → DAL 0433 GetData: the header row, or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0433 GetData — details: DocumentTypeId 96 reads the trading activity, every other type this one. */
    public List<Map<String, Object>> details(int id, int documentTypeId) {
        String act = documentTypeId == 96 ? "SaleDetailTradingReadByInvSaleInvoiceId" : "SaleDetailReadByInvSaleInvoiceId";
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", act));
    }

    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
    }

    public List<Map<String, Object>> freights(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
    }

    public List<Map<String, Object>> journals(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
    }

    // ================================================================================ history

    /** BLL 0580 FormHistory — parameters and conditions as the BLL builds them. */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, boolean canViewAll, int financialYearId,
                                                 Map<String, Timestamp> dates, int fromDocNo, int toDocNo,
                                                 int supplierCustomerId, int entryUser, String screenName) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        for (String k : new String[] { "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate",
                "ModifyToDate", "ApprovedFromDate", "ApprovedToDate" }) {
            Timestamp t = dates.get(k);
            if (t != null) p.put(k, t);
        }
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (!canViewAll) p.put("EntryUser", entryUser);
        if (screenName != null && !screenName.isEmpty()) p.put("ScreenName", screenName);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ================================================================================ print

    /** BLL 0128 PurchaseInvoiceReturn_StoreSip — both slips (145 and 145A) read these rows. */
    public List<Map<String, Object>> slip(UserAccount u, int branchesId, int financialYearId, int id) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId);
        if (id != 0) p.put("Id", id);
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchaseInvoiceReturn_StoreSip]", p);
    }

    /** BLL 0128 SalesCustomerBillSubReport — the slips' "InvRptPurchaseBillSupplierOthers" sub-report. */
    public List<Map<String, Object>> slipSubReport(int id) {
        return DesktopProc.rows(jdbc, "[SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep]", params("PihId", id));
    }

    /** CommonServices.AcRptPurchaseSalesVoucherSlip_103 → BLL 0141 (ApprovedFilter "All": IsApproved not sent). */
    public List<Map<String, Object>> voucherSlip(UserAccount u, int voucherHeadId, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (voucherHeadId != 0) p.put("Id", voucherHeadId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt", p);
    }

    // ================================================================================ voucher reads

    /** BLL 0267 GetSupplierCustomerListForFinancialEffects. */
    public List<Map<String, Object>> supplierGlAccounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId"));
    }

    /** BLL 0267 GetAvgRateFromAvaiblableLoaderStock. */
    public List<Map<String, Object>> avgRateFromLoaderStock(UserAccount u, int documentTypeId, int refDocIdNo, int refDocSubIdNo) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "RefDocIdNo", refDocIdNo,
                "RefDocSubIdNo", refDocSubIdNo,
                "Activity", "GetAvgRateFromAvaiblableLoaderStock"));
    }

    // ================================================================================ writes

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0433 UpdateInventoryReference — a whole InventoryStockEvalautionDetail model with four fields set. */
    public void updateInventoryReference(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", documentTypeId);
        e.put("RefDocIdNo", id);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    /** DAL 0433 — usp_StockInTransit_VoucherDelete_ByGdnId @Id (ExecuteNonQuery). */
    public void stockInTransitVoucherDelete(int id) {
        DesktopProc.scalar(jdbc, "usp_StockInTransit_VoucherDelete_ByGdnId", params("Id", id));
    }

    /** DAL 0433 — one USP_InventoryValidation per detail, with the fifteen parameters the DAL adds. */
    public void inventoryValidation(Map<String, Object> p) {
        DesktopProc.scalar(jdbc, "USP_InventoryValidation", p);
    }

    /** DAL 0433 — USP_VoucherBalanceCheck (ExecuteScalar). */
    public void voucherBalanceCheck(UserAccount u, int voucherHeadId) {
        DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", voucherHeadId));
    }

    /** DAL 0013 Reasons.SetData — USP_Reasons_Insert / _Update with the model's seven properties. */
    public int saveReason(boolean insert, Map<String, Object> model) {
        return setProc(insert ? "USP_Reasons_Insert" : "USP_Reasons_Update", model);
    }

    // ================================================================================ helpers

    /** Projects rows to the named columns (case-insensitive source keys), in the given order. */
    public static List<Map<String, Object>> project(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String c : cols) o.put(c, StoreIssuanceRepository.ci(r, c));
            out.add(o);
        }
        return out;
    }
}
