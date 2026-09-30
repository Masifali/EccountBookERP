package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Screen 507 "Sale Invoice Packing Material" (InvfrmInvSaleInvoiceDirectPackingMaterial, ScreenName
 * FrmSaleInvoiceDirectPackingMaterial, DocumentTypeId 126, BaseDocumentTypeId 2) - the reads the form makes
 * that the 95/171 ports do not already have. Writes go through SaleInvoiceService.persist (BLL 0580 / DAL 0433).
 */
@Repository
public class SaleInvoiceDirectPmRepository {
    public static final int DOC = 126;
    public static final String SCREEN = "FrmSaleInvoiceDirectPackingMaterial";

    private final JdbcTemplate jdbc;
    private final SaleInvoiceRepository si;

    public SaleInvoiceDirectPmRepository(JdbcTemplate jdbc, SaleInvoiceRepository si) { this.jdbc = jdbc; this.si = si; }

    /** CommonServices.InvSaleGenerateSalesTaxNo(true) - BLL 0580 GenerateSalesTaxNo ('GenerateSalesTaxNo', @IsTaxable). */
    public int salesTaxNo(UserAccount u) {
        var rows = si.proc("[Sp_InvSaleInvoice_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "IsTaxable", true, "Activity", "GenerateSalesTaxNo"));
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "SalesTaxNo"));
    }

    /** HistoryComboDbCall (:3256) - AllComboBindAgainstSaleInvoice(DocumentTypeIds "126"), Activity not set; the 'Supplier' rows. */
    public List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[Usp_AllComboAgainstSaleInvoice]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", n(u.getAppId()), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(DOC)))) {
            if (!"Supplier".equals(s(col(r, "Activity")))) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("Name", s(col(r, "ReferenceName")));
            out.add(x);
        }
        return out;
    }

    /** HistoryGridFill (:3351) - InvSaleInvoice.FormHistory with @ScreenName for 126. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, String dateMode,
                                             LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo, int customerId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "CanViewAllRecord", canViewAll);
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
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("ScreenName", SCREEN);
        p.put("Activity", "FormHistory");
        return si.proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /** frmLoadGdnStoreAndPm.BranchesFill - GetBranchesAllocatedToUserFromGdn(org, company, user, 0, ""): neither type sent. */
    public List<Map<String, Object>> gdnBranches(UserAccount u, boolean branchImplemented) {
        if (branchImplemented) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("BranchId", n(u.getBranchesId())); x.put("BranchName", si.branchName(u));
            return new ArrayList<>(List.of(x));
        }
        return si.proc("[dbo].[USP_GetBranchsAllocatedToUserFromGdn]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "UserId", u.getId()));
    }

    /** InvSaleInvoice.GetGdnDataForSaleInvoiceStoreAndPm (BLL 0580:1280) - usp_getGdndataForSaleInvoice_Store. */
    public List<Map<String, Object>> gdnData(UserAccount u, int financialYearId, LocalDateTime from, LocalDateTime to, String branchIds, int orderCategoryId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (orderCategoryId != 0) p.put("OrderCategoryId", orderCategoryId);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        return si.proc("usp_getGdndataForSaleInvoice_Store", p);
    }

    /** AvailableStockGetByItem (:3182) - GetAvgRateQtyAndStockInHand; @WarehouseId / @RackId (StoreRackId) only when non-zero. */
    public double stockInHand(UserAccount u, int itemId, LocalDateTime docDate, int conditionId, int warehouseId, int rackId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "DocDate", docDate == null ? null : Timestamp.valueOf(docDate));
        if (conditionId != 0) p.put("ItemConditionId", conditionId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (rackId != 0) p.put("RackId", rackId);
        p.put("Activity", "GetAvgRateQtyAndStockInHand");
        var rows = si.proc("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "QtyInHand"));
    }

    /** CommonServices.GetTaxTypeIdAndPercentByItemId - ItemTaxSchedule.GetTaxScheduleForItemId. */
    public List<Map<String, Object>> taxForItem(UserAccount u, int itemId, LocalDateTime date) {
        return jdbc.queryForList("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @EffectedDate=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, Timestamp.valueOf(date), "GetItemTaxScheduleForItemId");
    }

    /** DocDate_Leave (:3702) - CommonServices.GetTaxScheduleDetailbyItemIds. */
    public List<Map<String, Object>> taxByItems(UserAccount u, String itemIds, LocalDateTime date) {
        return jdbc.queryForList("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemIds=?, @EffectedDate=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemIds, Timestamp.valueOf(date), "GetTaxScheduleDetailbyItemIds");
    }

    /** DatatableHelper.GetTaxAccountsFromGlobal - AllAccountsWithCustomGroupId of AccountTypeId 3,6,8,16,17,18,19. */
    public List<Map<String, Object>> taxAccounts(UserAccount u) {
        Set<Integer> types = Set.of(3, 6, 8, 16, 17, 18, 19);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (!types.contains(i(col(r, "AccountTypeId")))) continue;
            int id = i(col(r, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id); m.put("AccountTitle", s(col(r, "AccountTitle"))); m.put("AccountCode", s(col(r, "AccountCode")));
            out.add(m);
        }
        return out;
    }

    /** getGlobalAllItems with ItemTypeOfTypeId 14 (ItemDtsFillFromGlobal :814, BaseDocumentTypeId 2). */
    public List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (i(col(r, "ItemTypeOfTypeId")) != 14) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("ItemName", s(col(r, "ItemName"))); m.put("ItemCode", s(col(r, "ItemCode")));
            m.put("ItemCategoryId", i(col(r, "ItemCategoryId"))); m.put("ItemCategory", s(col(r, "ItemCategory")));
            out.add(m);
        }
        return out;
    }

    /** globalUomSchedule with the three base flags (CommonBindings.ItemUomFromGlobalBind). */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("usp_getAllUomsByCompanyId", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("ItemId", i(col(r, "ItemId"))); m.put("UOMCode", s(col(r, "UOMCode")));
            m.put("Equivalent", d(col(r, "Equivalent")));
            m.put("BasePackUom", b(col(r, "BasePackUom"))); m.put("BaseRateUom", b(col(r, "BaseRateUom")));
            m.put("BaseSecondaryUom", b(col(r, "BaseSecondaryUom")));
            out.add(m);
        }
        return out;
    }

    /** racksWithWarehouseAndItems (the user's branch). */
    public List<Map<String, Object>> racks(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.usp_getRackswithWarehouseByItemId @OrganizationId=?, @CompanyId=?, @BranchId=?",
                u.getOrganizationId(), u.getCompanyId(), n(u.getBranchesId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("RackName", s(col(r, "RackName")));
            m.put("WarehouseId", i(col(r, "invWarehouseId"))); m.put("WareHouseName", s(col(r, "WareHouseName")));
            m.put("ItemId", i(col(r, "ItemId")));
            out.add(m);
        }
        return out;
    }

    /** clsGlobalVariables.globalJobLot. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_GetJobLotsAllocatedToBranch] @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("Description", s(col(r, "JobLotDescription")));
            out.add(m);
        }
        return out;
    }

    /** clsGlobalVariables.globalItemConditions. */
    public List<Map<String, Object>> conditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("Description", s(col(r, "ConditionStatus")));
            out.add(m);
        }
        return out;
    }
}
