package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management — the issuance family (screens 322 and 321) and the reads Store Return shares.
 *
 * Every procedure name, @Activity and parameter below was read from the recovered desktop source:
 *
 *   BLL 0252  Architecture.BLL.PurchaseTrading.InvGsStoreIssuanceHeader
 *   DAL 0221  Architecture.DAL.PurchaseTrading.InvGsStoreIssuanceHeader  (SetData / GetAll)
 *   BLL 0056  GetAvgRatesAndStockInHand                                    (stock and average rate)
 *   BLL 0558  InvDeliveryOrder.GetPendingDeliveryOrderForIssuance          (DO loader)
 *   BLL 0379  GlobalServicesMethods.GetRacksWithWarehouseByItemId          (racks)
 *   BLL 0010  PackingMaterialItemsAllocateToTransactionFlow.ItemCondition  (V_ItemCondition)
 *   BLL 0066  DateLock.GetByOrganizationCompanyIdAndDate
 *   DAL 0205  CommonServices.GetVoucherHeadId / GetConfigurationFromAllocation
 *   DAL 0434  InvPurchaseInvoice.AccountandInventoryRemoveById             (452 delete)
 *
 * A parameter the desktop only adds conditionally ("if (x != 0)") is only added here under the
 * same condition. A null value is never bound: DesktopProc leaves it out, which is what ADO.NET
 * does with a CLR null.
 */
@Repository
public class StoreIssuanceRepository {

    public static final String P_HEADER_GETALL = "Sp_InvGsStoreIssuanceHeader_GetAllMethod";

    private final JdbcTemplate jdbc;
    public StoreIssuanceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /** BLL 0252 GenerateCode — reads DocNo. */
    public int nextDocNo(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_HEADER_GETALL, params(
                "Activity", "GenerateCode",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "FinancialYearId", financialYearId));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** BLL 0252 GenerateBranchSrNo — reads BranchSrNo. */
    public int nextBranchSrNo(UserAccount u, int branchesId, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_HEADER_GETALL, params(
                "Activity", "GenerateBranchSrNo",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("BranchSrNo"));
    }

    // ============================================================================== globals

    /**
     * clsGlobalVariables.globalItemConditions — BLL 0010 ItemCondition() is a plain
     * {@code SELECT * FROM dbo.V_ItemCondition}; the forms then drop Id 4 in memory.
     */
    public List<Map<String, Object>> itemConditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            int id = toInt(ci(r, "Id"));
            if (id == 4) continue;                                  // .Where(r => r.Id != 4)
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("Description", ci(r, "ConditionStatus"));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.racksWithWarehouseAndItems — BLL 0379 GetRacksWithWarehouseByItemId
     * ({@code usp_getRackswithWarehouseByItemId}), mapped exactly as the BLL maps it:
     * the warehouse id arrives in the column {@code invWarehouseId}.
     */
    public List<Map<String, Object>> racksWithWarehouseAndItem(UserAccount u, int branchId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getRackswithWarehouseByItemId", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchId", branchId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("RackName", str(r.get("RackName")));
            o.put("WarehouseId", toInt(r.get("invWarehouseId")));
            o.put("WareHouseName", str(r.get("WareHouseName")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("BaseRackId", toInt(r.get("BaseRackId")));
            out.add(o);
        }
        return out;
    }

    /** DAL 0205 GetConfigurationFromAllocation — the ConfigKey string, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // ============================================================================ stock/rate

    /**
     * CommonServices.GetAvgRateQtyAndStockInHand → BLL 0056. The desktop calls it twice per row:
     * once WITH warehouse/rack for QtyInHand, once WITHOUT for AvgRate.
     */
    public Map<String, Object> avgRateAndStock(UserAccount u, int itemId, Timestamp docDate,
                                               int itemConditionId, int recId, int documentTypeId,
                                               int warehouseId, int rackId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (recId != 0)          p.put("RecId", recId);
        if (itemConditionId != 0) p.put("ItemConditionId", itemConditionId);
        if (warehouseId != 0)    p.put("WarehouseId", warehouseId);
        if (rackId != 0)         p.put("RackId", rackId);
        p.put("Activity", "GetAvgRateQtyAndStockInHand");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? null : r.get(0);
    }

    /** BLL 0252 GetLastThreeRatesFromIssuance — shown as-is in a list beside the item. */
    public List<Map<String, Object>> lastThreeRates(UserAccount u, int itemId, int itemConditionId) {
        return DesktopProc.rows(jdbc, "USP_GetLastThreeRatesFromIssuance", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "ItemConditionId", itemConditionId));
    }

    // ================================================================================ loaders

    /** PendingDoPmForIssuance → BLL 0558 GetPendingDeliveryOrderForIssuance. */
    public List<Map<String, Object>> pendingDeliveryOrdersPm(UserAccount u, int financialYearId, int branchesId,
                                                             Timestamp from, Timestamp to,
                                                             int docNoFrom, int docNoTo) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        if (from != null)    p.put("FromDate", from);
        if (to != null)      p.put("ToDate", to);
        if (docNoFrom != 0)  p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0)    p.put("DocNoTo", docNoTo);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetPendingDeliveryOrderPmForIssuance]", p);
    }

    /**
     * LoadDepRequestToConsumableStore.AllCombobind → BLL 0252 GetDataForDropDownFromDepartmentRequest.
     * The BLL's guard {@code (obj.Activity != "" || obj.Activity != null)} is always true, and the
     * form never sets Activity, so AddWithValue receives null and @Activity is NOT sent.
     */
    public List<Map<String, Object>> departmentRequestDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromDepartmentRequest]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId()));
    }

    /** LoadDepRequestToConsumableStore.PendingDepRequestLoad → BLL 0252 GetPendingDepartmentRequestForIssuanceNew. */
    public List<Map<String, Object>> pendingDepartmentRequests(UserAccount u, int financialYearId, int documentTypeId,
                                                               Timestamp from, Timestamp to, int docNoFrom, int docNoTo,
                                                               int itemId, int departmentFromId, int departmentToId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (from != null)         p.put("FromDate", from);
        if (to != null)           p.put("ToDate", to);
        if (docNoFrom != 0)       p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0)         p.put("DocNoTo", docNoTo);
        if (itemId != 0)          p.put("ItemId", itemId);
        if (departmentFromId != 0) p.put("DepartmentFromId", departmentFromId);
        if (departmentToId != 0)   p.put("DepartmentToId", departmentToId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetPendingDepartmentRequestForIssuance]", p);
    }

    // ================================================================================ history

    /** BLL 0252 FormHistory. Parameter names and conditions exactly as the BLL builds them. */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, int financialYearId,
                                                 int baseDocumentTypeId, int branchesId, boolean canViewAll,
                                                 int entryUser, Timestamp from, Timestamp to,
                                                 int fromDocNo, int toDocNo) {
        Map<String, Object> p = params(
                "organizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "CanViewAllRecord", canViewAll);
        if (financialYearId != 0)    p.put("FinancialYearId", financialYearId);
        if (baseDocumentTypeId != 0) p.put("BaseDocumentTypeId", baseDocumentTypeId);
        if (branchesId != 0)         p.put("BranchesId", branchesId);
        if (!canViewAll)             p.put("EntryUser", entryUser);
        if (from != null)            p.put("FromDate", from);
        if (to != null)              p.put("ToDate", to);
        if (fromDocNo != 0)          p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)            p.put("ToDocNo", toDocNo);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, P_HEADER_GETALL, p);
    }

    // ================================================================================ read

    /** BLL 0252 GetByID → DAL 0221 GetAll: the header row (index 0), or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_HEADER_GETALL, params(
                "Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * DAL 0221 GetAll — which detail activity is read is decided by the header, not by the
     * screen. The three ifs are independent on the desktop, so a later one overwrites an
     * earlier one; the same order is kept.
     */
    public List<Map<String, Object>> details(Map<String, Object> header) {
        int id = toInt(header.get("Id"));
        int refDocType = toInt(header.get("RefDocumentTypeId"));
        int docType = toInt(header.get("DocumentTypeId"));
        List<Map<String, Object>> d = new ArrayList<>();
        if (refDocType == 450 || docType == 1616) {
            d = DesktopProc.rows(jdbc, P_HEADER_GETALL, params("Id", id, "Activity", "InvGsStoreIssuanceDetailReadById"));
        }
        if (refDocType == 85) {
            d = DesktopProc.rows(jdbc, P_HEADER_GETALL, params("Id", id, "Activity", "InvGsStoreIssuanceDetailReadByIdForDelivery"));
        }
        if (docType == 452 || docType == 213) {
            d = DesktopProc.rows(jdbc, P_HEADER_GETALL, params("Id", id, "Activity", "InvGsStoreIssuanceDetailReadByIdForIssuanceDirect"));
        }
        return d;
    }

    /** CommonServices.VoucherHeadIdGet → BLL 0654 / DAL 0205 GetVoucherHeadId. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int documentTypeSrNo) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "DocumentTypeSrNo", documentTypeSrNo));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ================================================================================ delete

    /** BLL 0252 DeleteByID — screen 322. */
    public void deleteIssuance(int entryUser, int id) {
        DesktopProc.rows(jdbc, "[dbo].[" + P_HEADER_GETALL + "]", params(
                "EntryUser", entryUser, "Id", id, "Activity", "DeleteById"));
    }

    /** BLL 0581 RemoveByID → DAL 0434 AccountandInventoryRemoveById — screen 321 (and Store Return). */
    public void removeInvoiceVoucherAndStock(UserAccount u, int documentTypeId, int id, int userId) {
        DesktopProc.scalar(jdbc, "Sp_InvoicesVouchersandStocksDelete", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", id,
                "DocumentTypeId", documentTypeId,
                "UserId", userId));
    }

    // ================================================================================ write

    /**
     * BLL 0066 — the lock date, or null when none is set.
     *
     * Sp_DateLock_GetAllMethod 'ReadByOrganizationCompanyIdandDate' is {@code SELECT MAX([Date])},
     * so it always returns exactly one row, NULL when the company has no active lock. The desktop's
     * unguarded Rows[0] therefore never fails; NULL simply means no lock, as here. (Confirmed
     * against procdure.sql, 2026-09-23.)
     */
    public Timestamp dateLock(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_DateLock_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyIdandDate"));
        if (r.isEmpty()) return null;
        Object d = r.get(0).get("Date");
        if (d instanceof Timestamp) return (Timestamp) d;
        if (d instanceof java.util.Date) return new Timestamp(((java.util.Date) d).getTime());
        if (d instanceof java.time.LocalDateTime) return Timestamp.valueOf((java.time.LocalDateTime) d);
        return null;
    }

    /** CommonServies.GetItemListForFinancialEffects (BLL 0267) — Sp_Item_GetAllMethod. */
    public List<Map<String, Object>> itemGlAccounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GetItemGlIdsandItemName"));
    }

    /** GenericProvider.SetProc — returns ExecuteScalar's value (0 when none). */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0221 :154-170 — one USP_InventoryValidation per detail, all of them. */
    public void inventoryValidation(UserAccount u, int documentTypeId, Timestamp docDate,
                                    int itemId, int itemConditionId, int warehouseId, double netWeight) {
        DesktopProc.scalar(jdbc, "USP_InventoryValidation", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "DocDate", docDate,
                "ItemId", itemId,
                "ItemConditionId", itemConditionId,
                "WarehouseId", warehouseId,
                "NetWeight", netWeight));
    }

    /** The two stock recalculations every store document ends with (DAL 0221 / 0219). */
    public void postStock(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", documentTypeId);
        t.put("RefDocIdNo", id);
        setProc("Sp_InventoryTransactions_GetALLMethod", t);

        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", documentTypeId);
        e.put("RefDocIdNo", id);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    // ============================================================================= helpers

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt — anything unparseable is 0. */
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        try { return (int) Math.round(Double.parseDouble(String.valueOf(v).trim())); }
        catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble — anything unparseable is 0. */
    public static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0d; }
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /**
     * .NET Framework {@code double.ToString()} — 15 significant digits, no trailing zeros, no
     * ".0" on whole numbers. The voucher Comments text is built by string concatenation on the
     * desktop, so the numbers inside it must print the way the CLR prints them.
     */
    public static String clr(double d) {
        if (d == 0d) return "0";
        if (Double.isNaN(d) || Double.isInfinite(d)) return String.valueOf(d);
        BigDecimal b = new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros();
        return b.toPlainString();
    }
}
