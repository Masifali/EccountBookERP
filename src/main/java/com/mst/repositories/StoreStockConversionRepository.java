package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 502 "Store Stock Conversion" (Production/StoreStockConversion.cs, DocTypeId 808) and its
 * loader LoadavailableTransactionsForConversionStore. The calls whose DocTypeId
 * {@link StockConversionRepository} fixes at 66 are taken here with 808. They run through that class's
 * executor, which omits null parameters the way ADO.NET does.
 */
@Repository
public class StoreStockConversionRepository {

    public static final int DOC = 808;

    private final StockConversionRepository sc;
    public StoreStockConversionRepository(StockConversionRepository sc) { this.sc = sc; }

    private static Map<String, Object> p() { return new LinkedHashMap<>(); }

    /** GenerateDocNumber:1032 - BLL 0337 GetGenerateCode; the form sets no BranchedId, so it is not sent. */
    public int nextCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = p();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocTypeId", DOC);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> r = sc.rows("Sp_InvStockConversion_GetAllMethod", p);
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        Object v = r.get(0).values().iterator().next();
        return StoreIssuanceRepository.toInt(v);
    }

    /** BindGridHeaderistory:397 - BLL 0337 FormHistoryNew with DocumentTypeId 808. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, int entryUser,
                                             String from, String to, String entryFrom, String entryTo,
                                             String modifyFrom, String modifyTo, int docNoFrom, int docNoTo) {
        Map<String, Object> p = p();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchedId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("@DocumentTypeId", DOC);
        p.put("@FinancialYearId", financialYearId);
        p.put("@CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("@EntryUser", entryUser);
        if (from != null) p.put("@FromDate", from);
        if (to != null) p.put("@ToDate", to);
        if (entryFrom != null) p.put("@EntryFromDate", entryFrom);
        if (entryTo != null) p.put("@EntryToDate", entryTo);
        if (modifyFrom != null) p.put("@ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("@ModifyToDate", modifyTo);
        if (docNoFrom != 0) p.put("@DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("@DocNoTo", docNoTo);
        return sc.rows("USP_InvStockConversion_FormHistory", p);
    }

    /** ParentCategoryFill:548 - Item.InventoryParentCategories (no @Ids), rows 7 and 8 kept by the form. */
    public List<Map<String, Object>> parentCategories() {
        Map<String, Object> p = p();
        p.put("@Activity", "InventoryParentCategories");
        return sc.rows("Sp_InventoryItemsOther_GetAllMethod", p);
    }

    /** Loader ParentCategoryComboFill:116 - StocksReport.Evalaution_DropDown_ByParentCategories, Ids "7,8". */
    public List<Map<String, Object>> loaderDropDowns(UserAccount u) {
        Map<String, Object> p = p();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@InventoryParentCategory", "7,8");
        return sc.rows("USP_Evalaution_DropDown_ByParentCategories", p);
    }

    /** Loader PendingInventoryTransactions:265 - GetAvailableStockForStoreConsumption; each filter only when set. */
    public List<Map<String, Object>> availableStock(UserAccount u, String from, String to, int parentCategoryId,
                                                    int itemCategoryId, int itemTypeId, int warehouseId, int itemId,
                                                    int itemConditionId) {
        Map<String, Object> p = p();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (u.getBranchesId() != null && u.getBranchesId() != 0) p.put("@BranchesId", u.getBranchesId());
        if (from != null) p.put("@DateFrom", from);
        if (to != null) p.put("@DateTo", to);
        if (parentCategoryId != 0) p.put("@InventoryParentCategories", parentCategoryId);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (itemConditionId != 0) p.put("@ItemConditionId", itemConditionId);
        return sc.rows("[dbo].[SpInventoryStockEvaluation_GetAvailableStockForConsumption]", p);
    }
}
