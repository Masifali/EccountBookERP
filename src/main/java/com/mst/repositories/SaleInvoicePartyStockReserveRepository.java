package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Reads of screen 143 "Sale Invoice Party Stock Reserve" - Architecture.WinApp.Sale.InvfrmInvSaleInvoiceDirect with FlagForm = 1
 * (ScreenName "SaleInvoicepartyStockReserve", DocumentTypeId 99, header IsReserve = true).
 * Every method is one call the desktop form makes (the desktop line is given); the shared pieces of the sale-invoice family
 * (customers, currencies, other items, commission uoms, config, features, ledger, delete) are used from {@link SaleInvoiceRepository}.
 */
@Repository
public class SaleInvoicePartyStockReserveRepository {
    public static final int DOC = 99;
    public static final String SCREEN = "SaleInvoicepartyStockReserve";

    private final SaleInvoiceRepository si;

    public SaleInvoicePartyStockReserveRepository(SaleInvoiceRepository si) { this.si = si; }

    private LinkedHashMap<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** bindWareHouse :885 - WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId (USP_GetWarehousesAllocatedToBranch). */
    public List<Map<String, Object>> warehouses(UserAccount u, int branchId) {
        LinkedHashMap<String, Object> p = tenant(u);
        if (branchId != 0) p.put("BranchId", branchId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[USP_GetWarehousesAllocatedToBranch]", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("WareHouseName", s(col(r, "WareHouseName")));
            m.put("BranchId", i(col(r, "BranchId"))); m.put("BranchName", s(col(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    /** JobLotDtFillFromGlobalAndBind :1371 - JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId (USP_GetJobLotsAllocatedToBranch). */
    public List<Map<String, Object>> jobLots(UserAccount u, int branchId) {
        LinkedHashMap<String, Object> p = tenant(u);
        if (branchId != 0) p.put("BranchId", branchId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[USP_GetJobLotsAllocatedToBranch]", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("JobLotDescription", s(col(r, "JobLotDescription")));
            m.put("BranchId", i(col(r, "BranchId"))); m.put("BranchName", s(col(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    /** Load :622 - jobLot.JobLot_GetWithJobOrderAndItem(org, company, 0) (usp_JobLot_GetWithJobOrderAndItem) when SaleCostingJobOrderWise. */
    public List<Map<String, Object>> jobLotsWithItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[usp_JobLot_GetWithJobOrderAndItem]", tenant(u))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("JobLotDescription", s(col(r, "JobLotDescription")));
            m.put("BranchId", i(col(r, "BranchId"))); m.put("BranchName", s(col(r, "BranchName")));
            m.put("ItemId", i(col(r, "ItemId")));
            out.add(m);
        }
        return out;
    }

    /** BinCity :952 - CommonServices.CityGetAllService (SP_City_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        LinkedHashMap<String, Object> p = tenant(u); p.put("MethodType", "GetAll");
        for (var r : si.proc("SP_City_GetAllMethod", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id")));
            Object name = col(r, "CityName");
            m.put("CityName", s(name != null ? name : col(r, "Description")));
            out.add(m);
        }
        return out;
    }

    /** cropyear :1490 - CommonServices.CropYearGetAllService (Sp_InvCropYear_GetAllMethod 'ReadAll'). */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("Sp_InvCropYear_GetAllMethod", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("CropYear", s(col(r, "CropYear")));
            out.add(m);
        }
        return out;
    }

    /** Project :1224 - CommonServices.ProjectServiceBind (Sp_Projects_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> projects(UserAccount u) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("MethodType", "GetAll");
        return si.proc("Sp_Projects_GetAllMethod", p);
    }

    /** braches :1241 - CommonServices.BrancheServiceBind (Sp_Branches_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> branches(UserAccount u) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("Activity", "GetAll");
        return si.proc("Sp_Branches_GetAllMethod", p);
    }

    /** PaymentTerms :1258 - CommonServices.GetDueTermServiceBind (Sp_InvDueTerms_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> dueTerms(UserAccount u) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("Activity", "GetAll");
        return si.proc("Sp_InvDueTerms_GetAllMethod", p);
    }

    /** PackingType :1342 - InvPackingType.Getall (Sp_InvPackingType_GetAllMethod 'ReadAll'). */
    public List<Map<String, Object>> packingTypes() {
        return si.proc("Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** OrderCategory_II :1660 - CommonServices.GetLookupsByTypeIdDt(19) (Sp_InvLookup_GetAllMethod 'ReadByInvlookTypeId'). */
    public List<Map<String, Object>> otherCategories(UserAccount u) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("InvLookupTypeId", 19); p.put("Activity", "ReadByInvlookTypeId");
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("Sp_InvLookup_GetAllMethod", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("LookupName", s(col(r, "LookupName")));
            out.add(m);
        }
        return out;
    }

    /** ItemCategoryOrTypeBind/ItemdtFillFromAll :1090/:1148 - clsGlobalVariables.getGlobalAllItems (USP_Item_AllItemsWithModal) without ItemTypeOfTypeId 14 and 17. */
    public List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[USP_Item_AllItemsWithModal]", tenant(u))) {
            int tt = i(col(r, "ItemTypeOfTypeId"));
            if (tt == 14 || tt == 17) continue;
            out.add(itemRow(r, "ItemCode", "ItemProductionStageId"));
        }
        return out;
    }

    /** ItemdtFillFromAll :1148 with GetItemagainstwarehouseId - CommonServices.GetItemsFromWarehouseAllocationByWareHouseId (Sp_ItemAllocateToWareHouse_GetAllMethod). */
    public List<Map<String, Object>> itemsByWarehouse(UserAccount u, int warehouseId) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("WarehouseId", warehouseId); p.put("Activity", "GetItemsFromWarehouseAllocationByWareHouseId");
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("Sp_ItemAllocateToWareHouse_GetAllMethod", p)) out.add(itemRow(r, "ItemCodeNew", "ItemProductionStageId"));
        return out;
    }

    private static Map<String, Object> itemRow(Map<String, Object> r, String codeCol, String stageCol) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", i(col(r, "Id"))); m.put("ItemName", s(col(r, "ItemName"))); m.put("ItemCode", s(col(r, codeCol)));
        m.put("InventoryParentCategoriesId", i(col(r, "InventoryParentCategoriesId")));
        m.put("ItemCategoryId", i(col(r, "ItemCategoryId"))); m.put("ItemCategory", s(col(r, "ItemCategory")));
        m.put("ItemTypeId", i(col(r, "ItemTypeId"))); m.put("ItemType", s(col(r, "ItemType")));
        return m;
    }

    /** BrandDtFillFromGlobal :1683 - clsGlobalVariables.getGlobalAllBrands (usp_getBrands): Id, ItemName = BrandName, ItemCode = BrandCode. */
    public List<Map<String, Object>> brands(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[usp_getBrands]", tenant(u))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("ItemName", s(col(r, "BrandName"))); m.put("ItemCode", s(col(r, "BrandCode")));
            out.add(m);
        }
        return out;
    }

    /** PackUOM :1442 - CommonServices.GetUomScheduleByItemId (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'): Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        LinkedHashMap<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("Sp_UOMSchedule_GetAllMethod", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("UOMCode", s(col(r, "UOMCode"))); m.put("Equivalent", d(col(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    /** AvailableStockGetByItem :1993 - InventoryStockEvalautionDetail.GetCurrentStockByItemId (Sp_SaleOrder_GetAllMethod 'GetCurrentStockByItemId'); every parameter is sent. */
    public double currentStock(UserAccount u, int itemId, LocalDateTime docDate, int warehouseId, int jobLotId, String cropYear, int packingTypeId, int stockUom) {
        LinkedHashMap<String, Object> p = tenant(u);
        p.put("ItemId", itemId); p.put("DocDateTo", docDate); p.put("WareHouseId", warehouseId); p.put("JobLotId", jobLotId);
        p.put("CropYear", cropYear == null ? "" : cropYear); p.put("InvPackingTypeId", packingTypeId); p.put("ItemUomId", stockUom);
        p.put("Activity", "GetCurrentStockByItemId");
        var rows = si.proc("Sp_SaleOrder_GetAllMethod", p);
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "AvailableStock"));
    }

    /** CommissionDebitAccountFill :1037 - feature 4: GetVendorsAndCustomersForTransporter; else CoaAllocationAccountTitleByAccountTypeIds("11,12,13,14,20,21"). Rows {GlAccountId, AccountTitle, SupplierCustomerId}. */
    public List<Map<String, Object>> commissionDebitAccounts(UserAccount u, boolean subsidiary) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (var r : si.q("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId()))
                out.add(three(i(col(r, "GlAccountId")), s(col(r, "CompanyName")), i(col(r, "Id"))));
        } else {
            for (var r : si.accountTitlesByTypes(u, "11,12,13,14,20,21", null))
                out.add(three(i(col(r, "Id")), s(col(r, "AccountTitle")), 0));
        }
        return out;
    }

    /**
     * TransporterAcFill :1589 - dtAccountlst {Id (gl account), SupplierCustomerId, AccountTitle}.
     * Feature 4: GetVendorsAndCustomersForTransporter (GlAccountId, Id, CompanyName).
     * Otherwise CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22") when FreightDebitToExpenses, else (null, "2,11,12,,13,14,15,20,21,22").
     */
    public List<Map<String, Object>> transporterAccounts(UserAccount u, boolean subsidiary, boolean freightDebitToExpenses) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (var r : si.q("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", i(col(r, "GlAccountId"))); x.put("SupplierCustomerId", i(col(r, "Id"))); x.put("AccountTitle", s(col(r, "CompanyName")));
                out.add(x);
            }
            return out;
        }
        String not = freightDebitToExpenses ? "2,15,22" : "2,11,12,,13,14,15,20,21,22";
        for (var r : si.accountTitlesByTypes(u, null, not)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("SupplierCustomerId", 0); x.put("AccountTitle", s(col(r, "AccountTitle")));
            out.add(x);
        }
        return out;
    }

    private static Map<String, Object> three(int gl, String title, int party) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put("GlAccountId", gl); x.put("AccountTitle", title); x.put("SupplierCustomerId", party);
        return x;
    }

    /** BranchSrNoFill :1547 - InvSaleInvoice.GenerateInvSaleInvoiceCodeBranch ('GenerateBranchCode'), DocumentTypeId 99. */
    public int nextBranchSrNo(UserAccount u, int financialYearId) {
        LinkedHashMap<String, Object> p = tenant(u);
        p.put("BranchesId", n(u.getBranchesId())); p.put("DocumentTypeId", DOC); p.put("FinancialYearId", financialYearId); p.put("Activity", "GenerateBranchCode");
        var rows = si.proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "BranchSrNo"));
    }

    /** HistoryCombosBranchFill :4353 - BranchImplemented: the user's branch; else InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(DocumentTypeId 99). */
    public List<Map<String, Object>> historyBranches(UserAccount u, boolean branchImplemented) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchImplemented) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", n(u.getBranchesId())); x.put("BranchName", si.branchName(u));
            out.add(x);
            return out;
        }
        for (var r : si.q("EXEC dbo.USP_GetBranchsAllocatedToUserFromSaleInvoice @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), DOC)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "BranchId"))); x.put("BranchName", s(col(r, "BranchName")));
            out.add(x);
        }
        return out;
    }

    /** HistoryCombosFill :4410 - InvSaleInvoice.AllComboBindAgainstSaleInvoice (Usp_AllComboAgainstSaleInvoice), the 'Supplier' rows as {Id, Customer}. */
    public List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[Usp_AllComboAgainstSaleInvoice]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", n(u.getAppId()), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(DOC)))) {
            if (!"Supplier".equals(s(col(r, "Activity")))) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("Customer", s(col(r, "ReferenceName")));
            out.add(x);
        }
        return out;
    }

    /**
     * GetAll :4470 - InvSaleInvoice.FormHistory (BLL 0580:555) with CanViewAllRecord = true (so no EntryUser), FinancialYearId, the dates of
     * the picked date type, doc-no range, customer and the picked branches. ScreenName and PaymenetTermId are never assigned, so not sent.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, String dateMode, LocalDateTime from, LocalDateTime to,
                                             int fromDocNo, int toDocNo, int customerId, String branchIds) {
        LinkedHashMap<String, Object> p = tenant(u);
        p.put("DocumentTypeId", DOC); p.put("CanViewAllRecord", true);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        String fromKey, toKey;
        switch (dateMode == null ? "" : dateMode) {
            case "entry": fromKey = "EntryFromDate"; toKey = "EntryToDate"; break;
            case "modify": fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; break;
            case "approved": fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; break;
            default: fromKey = "FromDate"; toKey = "ToDate"; break;
        }
        if (from != null) p.put(fromKey, from);
        if (to != null) p.put(toKey, to);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        p.put("Activity", "FormHistory");
        return si.proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /** btnRecordsUpdate_Click :4310 - InvSaleInvoice.GetSaleInvoiceIdsForAutoUpdation (USP_GetSaleInvoiceIdsForAutoUpdation). */
    public List<Integer> autoUpdateIds(UserAccount u, int financialYearId) {
        LinkedHashMap<String, Object> p = tenant(u);
        p.put("FinancialYearId", financialYearId); p.put("DocumentTypeId", DOC);
        List<Integer> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[USP_GetSaleInvoiceIdsForAutoUpdation]", p)) out.add(i(col(r, "Id")));
        return out;
    }

    /** WagesRefDocumentsStatusList - GlobalVariables_Helper.GetWagesRefDocumentsStatusById: the USP_GetRefDocumentsForWages row of document type 99 is active. */
    public boolean wagesActive() {
        for (var r : si.q("EXEC dbo.USP_GetRefDocumentsForWages"))
            if (i(col(r, "RefDocumentTypeId")) == DOC) return b(col(r, "IsActive"));
        return false;
    }
}
