package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 334 "Purchase Invoice Direct Store" ({@code frmPurchaseInvoiceDirectStore}, DocumentTypeId 61)
 * — the reads and writes StoreIssuanceRepository does not already have.
 *
 * Every procedure, @Activity and parameter below was read from the recovered desktop source and
 * checked against /root/ddl/procs.json:
 *
 *   BLL 0581 / DAL 0434  Architecture.*.Inventory.InvPurchaseInvoice  (GenerateCode, GenerateBranchSrNo,
 *                        GetByID → GetDate, FormHistory, AllComboBindAgainstPurchaseInvoice,
 *                        GetBranchsAllocatedToUserFromPurchaseInvoice, StockInReferenceValidationReferredOrNot,
 *                        SetData)
 *   BLL 0379             GlobalServicesMethods (the clsGlobalVariables lists the form binds from)
 *   BLL 0019             JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId
 *   BLL 0594             jobLot.GetByID (the job lot's AccountId, used by the voucher)
 *   BLL 0267             CommonServies.GetSupplierCustomerListForFinancialEffects
 *   BLL 0141             VoucherReports.VoucherValidationReport (118) / VoucherSlipForInventoryReport (103)
 *
 * A parameter the desktop only adds under a condition is only added here under the same
 * condition; a null is never bound (DesktopProc omits it, as ADO.NET does with a CLR null).
 */
@Repository
public class PurchaseInvoiceDirectStoreRepository {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseInvoiceDirectStoreRepository.class);

    public static final String P_GETALL = "Sp_InvPurchaseInvoice_GetAllMethod";

    private final JdbcTemplate jdbc;
    public PurchaseInvoiceDirectStoreRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /** CommonServices.PurchaseInvoiceGenerateCode → BLL 0581 GenerateInvPurchaseInvoiceCode:139. */
    public int nextDocNo(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[" + P_GETALL + "]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "FinancialYearId", financialYearId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    /** BLL 0581 GenerateBranchCode:234. */
    public int nextBranchSrNo(UserAccount u, int financialYearId, int branchesId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[" + P_GETALL + "]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "FinancialYearId", financialYearId,
                "BranchesId", branchesId,
                "Activity", "GenerateBranchSrNo"));
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "BranchSrNo"));
    }

    // ============================================================================== globals

    /** CommonServices.GetERPFeatureById — is the id in USP_GetERPFeaturesByCompanyId. Failure = off. */
    public boolean erpFeature(UserAccount u, int featureId) {
        try {
            for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
                if (toInt(ci(r, "Id")) == featureId) return true;
            }
        } catch (Exception e) {
            LOG.warn("ERP feature {} could not be read; treating as off", featureId, e);
        }
        return false;
    }

    /**
     * clsGlobalVariables.globalSupplierCustomer — DatatableHelper.GlobalSupplierCustomerListsFillDbCall
     * (partyTypeId 0) over BLL 0379 getGlobalSupplierCustomer (USP_GetVendorsAndCustomersWithCityName):
     * customer groups 7, 9, 10 always dropped; with ShowBothVendorAndCustomerOnSalesPurchase = 1 the
     * groups 7, 8, 9, 10, 13 are dropped; otherwise (partyTypeId 0) everything else stays.
     */
    public List<Map<String, Object>> suppliers(UserAccount u, int showBothVendorAndCustomer) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            int g = toInt(ci(r, "CustomerGroupId"));
            if (g == 7 || g == 9 || g == 10) continue;
            boolean excluded = g == 7 || g == 8 || g == 9 || g == 10 || g == 13;
            if (showBothVendorAndCustomer == 1 && excluded) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CompanyName", str(ci(r, "CompanyName")));
            o.put("PartyCode", str(ci(r, "PartyCode")));
            o.put("GlAccountId", toInt(ci(r, "GlAccountId")));
            o.put("CityId", toInt(ci(r, "CityId")));
            o.put("CityName", str(ci(r, "CityName")));
            o.put("MobileNo", str(ci(r, "MobilePersonal")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters — USP_GetVendorsAndCustomersForTransporter. */
    public List<Map<String, Object>> vendorsForTransporter(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetVendorsAndCustomersForTransporter]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.AllAccountsWithCustomGroupId — USP_GETAllAccountsFromCustomGroups. */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.getGlobalAllItems — USP_Item_AllItemsWithModal. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.globalUomSchedule — getAllUomsByCompanyId(org, comp, 0, 1): @ItemId not sent, @Active 1. */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("UOMCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            o.put("BaseRateUom", toBool(ci(r, "BaseRateUom")));
            o.put("BasePackUom", toBool(ci(r, "BasePackUom")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.globalWarehousesWithBranches — USP_GetWarehousesAllocatedToBranch @BranchId = user's branch. */
    public List<Map<String, Object>> warehousesWithBranches(UserAccount u, int branchId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetWarehousesAllocatedToBranch", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branchId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("WareHouseName", str(ci(r, "WarehouseName")));
            o.put("BranchId", toInt(ci(r, "BranchId")));
            o.put("BranchName", str(ci(r, "BranchName")));
            o.put("IsActive", toBool(ci(r, "IsActive")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.racksWithWarehouseAndItems — BLL 0379 GetRacksWithWarehouseByItemId, with the
     * BranchId / BranchName the BLL also maps (btnAdd_Click:2009 reads them off the warehouse row).
     */
    public List<Map<String, Object>> racks(UserAccount u, int branchId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getRackswithWarehouseByItemId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branchId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("RackName", str(ci(r, "RackName")));
            o.put("WarehouseId", toInt(ci(r, "invWarehouseId")));
            o.put("WareHouseName", str(ci(r, "WareHouseName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("BranchId", toInt(ci(r, "BranchId")));
            o.put("BranchName", str(ci(r, "BranchName")));
            out.add(o);
        }
        return out;
    }

    /** BLL 0019 GetJobLotsAllocatedToBranchByBranchId — @BranchId only when non-zero. */
    public List<Map<String, Object>> jobLots(UserAccount u, int branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchesId != 0) p.put("BranchId", branchesId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetJobLotsAllocatedToBranch]", p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("JobLotDescription", str(ci(r, "JobLotDescription")));
            o.put("BranchId", toInt(ci(r, "BranchId")));
            o.put("BranchName", str(ci(r, "BranchName")));
            out.add(o);
        }
        return out;
    }

    /**
     * BLL 0594 jobLot.GetByID → SP_JobLot_ReadMethod 'GetById', the AccountId column. The desktop
     * indexes [0] unguarded, so a missing job lot throws there; the same happens here.
     */
    public int jobLotAccountId(int jobLotId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "Id", jobLotId, "Activity", "GetById"));
        if (r.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        return toInt(ci(r.get(0), "AccountId"));
    }

    /** BLL 0267 GetSupplierCustomerListForFinancialEffects — Sp_SupplierCustomer_GetAllMethod. */
    public List<Map<String, Object>> supplierGlAccounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId"));
    }

    // ================================================================================ read

    /** BLL 0581 GetByID → DAL 0434 GetDate: the header row, or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0434 GetDate:632 — DocumentTypeId 61 / 64 / 245 read their details from USP_InvPurchaseInvoiceDetail_ReadById. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseInvoiceDetail_ReadById]", params("Id", id));
    }

    /** DAL 0434 GetDate:664. */
    public List<Map<String, Object>> freight(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
    }

    /** DAL 0434 GetDate:678. */
    public List<Map<String, Object>> journal(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
    }

    // ============================================================================== history

    /** BLL 0581 GetBranchsAllocatedToUserFromPurchaseInvoice:3121 — @DocumentTypeId only when non-zero. */
    public List<Map<String, Object>> historyBranches(UserAccount u, int documentTypeId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "UserId", u.getId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", p);
    }

    /** BLL 0581 AllComboBindAgainstPurchaseInvoice:2172 — the form never sets Activity, so it is not sent. */
    public List<Map<String, Object>> historyCombos(UserAccount u, String documentTypeIds, String branchesIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstPurchaseInvoice]", p);
    }

    /** BLL 0581 FormHistory:369 — every parameter under the BLL's own condition, in its order. */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, int financialYearId, boolean canViewAll,
                                                 Timestamp from, Timestamp to, Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo,
                                                 int fromDocNo, int toDocNo, int supplierCustomerId, String branchesIds) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (entryFrom != null) p.put("EntryFromDate", entryFrom);
        if (entryTo != null) p.put("EntryToDate", entryTo);
        if (modifyFrom != null) p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null) p.put("ApprovedToDate", approvedTo);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "[" + P_GETALL + "]", p);
    }

    // ============================================================================== detail row delete

    /** BLL 0581 StockInReferenceValidationReferredOrNot:65 — its own transaction on the desktop. */
    public void stockInReferenceValidation(UserAccount u, int documentTypeId, int id, int detailId) {
        DesktopProc.scalar(jdbc, "[dbo].[usp_StockInReferenceValidationReferredOrNot]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "Id", id,
                "DetailId", detailId));
    }

    // ================================================================================ write (DAL 0434 SetData)

    /** GenericProvider.SetProc. */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0434:140 usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations. */
    public void removeDetailsNotIn(UserAccount u, int id, String detailIds) {
        DesktopProc.scalar(jdbc, "usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", id,
                "DetailIds", detailIds));
    }

    /** DAL 0434:152 — DocumentTypeId != 59. */
    public void stockInTransitDelete(int id) {
        DesktopProc.scalar(jdbc, "usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId", params("Id", id));
    }

    /** DAL 0434:469 — a fresh InventoryStockEvalautionDetail with four fields set. */
    public void stockEvaluationUpdate(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", documentTypeId);
        e.put("RefDocIdNo", id);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    /** DAL 0434:526 — a fresh InventoryTransactions with four fields set. */
    public void inventoryTransactions(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", documentTypeId);
        t.put("RefDocIdNo", id);
        setProc("Sp_InventoryTransactions_GetALLMethod", t);
    }

    /** DAL 0434:508 USP_VoucherBalanceCheck — ExecuteScalar; a RAISERROR rolls the save back. */
    public void voucherBalanceCheck(UserAccount u, int voucherHeadId) {
        DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", voucherHeadId));
    }

    // ================================================================================ print

    /** 233-SpInvPurchaseInvoice_StoreBillDirect.rpt — its procedure, with the report's own parameters. */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "SpInvPurchaseInvoice_StoreBillDirect_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    /** CommonServices.VoucherReport_118 → BLL 0141 VoucherValidationReport (ApprovedFilter "All"). */
    public List<Map<String, Object>> voucher118(UserAccount u, int voucherHeadId, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        if (voucherHeadId != 0) p.put("Id", voucherHeadId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "Sp_Accounts_VouchersValidation_Rpt", p);
    }

    /** CommonServices.AcRptPurchaseSalesVoucherSlip_103 → BLL 0141 VoucherSlipForInventoryReport. */
    public List<Map<String, Object>> voucher103(UserAccount u, int voucherHeadId, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (voucherHeadId != 0) p.put("Id", voucherHeadId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt", p);
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }
}
