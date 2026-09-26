package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 505 "Stock Transfer Store" (frmStockTransferStore.cs, DocumentTypeId 807) - the reads only this form
 * makes. The shared stock-transfer reads and writes (BLL 0559 / DAL 0412) are on {@link StoreStockTransferRepository}.
 */
@Repository
public class StockTransferStoreRepository {

    private final JdbcTemplate jdbc;

    public StockTransferStoreRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** BLL 0559 GenerateCode - this form sets BranchesId, so @BranchesId goes when non-zero. */
    public int generateCode(UserAccount u, int documentTypeId, int financialYearId, int branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        p.put("Activity", "GenrateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, StoreStockTransferRepository.P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** globalBranchesAllocateToUser - GetGlobalBranchsAllocatedToUser. */
    public List<Map<String, Object>> branchesAllocatedToUser(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUser]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "BranchId")));
            m.put("Description", str(ci(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    /** racksWithWarehouseAndItems / GetRacksWithWarehouseByItemId(org, comp, branch). */
    public List<Map<String, Object>> racks(UserAccount u, int branchId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "usp_getRackswithWarehouseByItemId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branchId))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("RackName", str(ci(r, "RackName")));
            m.put("WarehouseId", toInt(ci(r, "invWarehouseId")));
            m.put("WareHouseName", str(ci(r, "WareHouseName")));
            m.put("ItemId", toInt(ci(r, "ItemId")));
            m.put("BaseRackId", toInt(ci(r, "BaseRackId")));
            out.add(m);
        }
        return out;
    }

    /** ItemDetailFill:492 - getGlobalAllItems with ItemTypeOfTypeId 14 or 17. */
    public List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            int t = toInt(ci(r, "ItemTypeOfTypeId"));
            if (t != 14 && t != 17) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("ItemName", str(ci(r, "ItemName")));
            m.put("ItemCode", str(ci(r, "ItemCode")));
            m.put("ParentCategoryId", toInt(ci(r, "InventoryParentCategoriesId")));
            m.put("ItemTypeOfTypeId", t);
            out.add(m);
        }
        return out;
    }

    /** CommonServices.GetAvgRateQtyAndStockInHand - zero-valued ids are not sent. Row 0 or null. */
    public Map<String, Object> avgRateQtyAndStock(UserAccount u, int itemId, Timestamp docDate, int conditionId, int recId,
                                                  int documentTypeId, int warehouseId, int rackId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDate", docDate);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("RecId", recId);
        if (conditionId != 0) p.put("ItemConditionId", conditionId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (rackId != 0) p.put("RackId", rackId);
        p.put("Activity", "GetAvgRateQtyAndStockInHand");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? null : r.get(0);
    }

    /** CmbRefDocumentType_Leave:684 - GetContractScheduleAndInvoiceDataForPM with DocumentTypeId 245 (sic). */
    public List<Map<String, Object>> refDocs(UserAccount u, int refDocumentTypeId, int recId) {
        if (refDocumentTypeId <= 0) return List.of();
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", 245, "RefDocumentTypeId", refDocumentTypeId);
        if (recId != 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetContractScheduleAndInvoiceDataForPM]", p);
    }

    public List<Map<String, Object>> refDocumentTypes() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "RefDocumentTypeForPackingMaterial"));
    }

    public List<Map<String, Object>> conditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("Description", str(ci(r, "ConditionStatus")));
            out.add(m);
        }
        return out;
    }

    /** globalUomSchedule filtered by item - Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (itemId != 0 && toInt(ci(r, "ItemId")) != itemId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("UOMCode", str(ci(r, "UOMCode")));
            m.put("Equivalent", toDouble(ci(r, "Equivalent")));
            m.put("ItemId", toInt(ci(r, "ItemId")));
            out.add(m);
        }
        return out;
    }
}
