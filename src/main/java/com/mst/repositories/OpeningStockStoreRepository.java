package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 341 "Opening Stock Store" ({@code frmStoreOpeningStockBalancing.cs}, DocumentTypeId 39) —
 * every read and write the form, its BLL and its DAL make, one method per desktop call.
 *
 * Procedures, activities and parameters were read from the recovered desktop source:
 *
 *   BLL 0253 / DAL 0222  InvStockOpeningBalanceHeader   GenerateCode, GetAllOrById, FormHistory,
 *                                                       GetDataForDropDownFromInvStockOpeningBalanceHeader,
 *                                                       Save → SetData (Sp_InvStockOpeningBalanceHeader_Insert|_Update)
 *   BLL 0583             Item.GetByID                   Sp_Item_GetAllMethod 'ReadById'
 *   BLL 0594             jobLot.GetByID                 SP_JobLot_ReadMethod 'GetById'
 *   BLL 0379             GlobalServicesMethods          AllItemsWithModal, getAllUomsByCompanyId(…,0,1),
 *                                                       GetGlobalAllAccountsWithCustomGroup, GetItemCondtions,
 *                                                       GetRacksWithWarehouseByItemId (via StoreIssuanceRepository)
 *   DAL 0205             CommonServices                 GetJobLotGlIdsandName
 *   BLL 0136             GeneralReprots                 StockopeningBalanceRegister (the 416 print)
 *
 * The two stock-posting procedures and the voucher-id lookup are the shared ones in
 * {@link StoreIssuanceRepository} (postStock uses {@link InventoryOpeningDefaults}); the voucher
 * models start from {@link InventoryOpeningDefaults#voucher()} / {@link InventoryOpeningDefaults#detail()}.
 *
 * A parameter the desktop only adds conditionally is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET's AddWithValue(null) does).
 */
@Repository
public class OpeningStockStoreRepository {

    public static final String P_GETALL = "Sp_InvStockOpeningBalanceHeader_GetAllMethod";

    private final JdbcTemplate jdbc;
    private final StoreIssuanceRepository shared;

    public OpeningStockStoreRepository(JdbcTemplate jdbc, StoreIssuanceRepository shared) {
        this.jdbc = jdbc;
        this.shared = shared;
    }

    // ============================================================================ numbering

    /** BLL 0253 GenerateCode — DocNo (BranchesId is set on the object but never sent). */
    public int generateCode(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================ read / history

    /**
     * BLL 0253 GetAllOrById as ReadById calls it (frmStoreOpeningStockBalancing.cs:719): only
     * OrganizationId, CompanyId, DocumentTypeId and Id are set, so FinancialYearId,
     * BaseDocumentTypeId and DocNo (all 0) are not sent.
     */
    public List<Map<String, Object>> getById(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = params(
                "DocumentTypeId", documentTypeId,
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
        if (id != 0) p.put("Id", id);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    /**
     * BLL 0253 FormHistory (GridHistoryBind:861). The form sets BranchesId, CanViewAllRecord and
     * EntryUser on the ReportsParameters, but the BLL never sends them; BaseDocumentTypeId, Id and
     * DocNo are never set by the form, so they stay 0 and are not sent either.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, int financialYearId,
                                                 Timestamp from, Timestamp to, int fromDocNo, int toDocNo,
                                                 int accountId, int itemId, int itemStockAccountId) {
        Map<String, Object> p = params(
                "DocumentTypeId", documentTypeId,
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll");
        if (financialYearId != 0)    p.put("FinancialYearId", financialYearId);
        if (from != null)            p.put("FromDate", from);
        if (to != null)              p.put("ToDate", to);
        if (fromDocNo != 0)          p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)            p.put("ToDocNo", toDocNo);
        if (accountId != 0)          p.put("AccountId", accountId);
        if (itemId != 0)             p.put("ItemId", itemId);
        if (itemStockAccountId != 0) p.put("ItemStockAccountId", itemStockAccountId);
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    /**
     * BLL 0253 GetDataForDropDownFromInvStockOpeningBalanceHeader (HistoryComboDbCall:1004).
     * The form never sets Activity, so it is not sent (every activity comes back).
     */
    public List<Map<String, Object>> historyDropDowns(UserAccount u, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromInvStockOpeningBalanceHeader]", p);
    }

    /** CommonServices.StockOpeningBalanceSlipandRegister416 → BLL 0136 StockopeningBalanceRegister. */
    public List<Map<String, Object>> register(UserAccount u, String documentTypeIds, int id) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (id != 0) p.put("Id", id);
        return DesktopProc.rows(jdbc, "Sp_InvStockOpeningBalanceHeader_RegisterRpt", p);
    }

    // ============================================================================== globals

    /**
     * clsGlobalVariables.getGlobalAllItems (BLL 0379 AllItemsWithModal(org, comp, 0, 0, "") —
     * PageSize/PageNumber 0 and Keyword "" are not sent), filtered as ItemDtsFillFromGlobal:375
     * filters it: the given ItemTypeOfTypeIds only.
     */
    public List<Map<String, Object>> items(UserAccount u, Set<Integer> allowedTypeOfTypes) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (!allowedTypeOfTypes.contains(toInt(r.get("ItemTypeOfTypeId")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalUomSchedule — getAllUomsByCompanyId(org, comp, 0, 1): ItemId 0 is
     * not sent, Active = 1. Mapped as dtUomFromGloablUomScheduleByItemId builds its table.
     */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            o.put("BaseRateUom", bool(r.get("BaseRateUom")));
            o.put("BasePackUom", bool(r.get("BasePackUom")));
            o.put("BaseSecondaryUom", bool(r.get("BaseSecondaryUom")));
            o.put("ItemId", toInt(r.get("ItemId")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.racksWithWarehouseAndItems — shared read, BranchId = the user's branch. */
    public List<Map<String, Object>> racks(UserAccount u, int branchId) {
        return shared.racksWithWarehouseAndItem(u, branchId);
    }

    /**
     * clsGlobalVariables.globalJobLot — DAL 0205 GetJobLotGlIdsandName, bound by
     * JobLotDtFillFromGlobalAndBind:538 as (Id, Description = JobLotDescription).
     */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[SP_JobLot_ReadMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetJobLotGlIdsandName"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Description", str(r.get("JobLotDescription")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalItemConditions — BLL 0379 GetItemCondtions (every V_ItemCondition
     * row). This form binds the whole list (ItemConditionBindFromGlobal:551, no Id 4 filter —
     * Insert:604 even branches on condition 4).
     */
    public List<Map<String, Object>> itemConditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(StoreIssuanceRepository.ci(r, "Id")));
            o.put("Description", str(StoreIssuanceRepository.ci(r, "ConditionStatus")));
            out.add(o);
        }
        return out;
    }

    /**
     * StockCreditAccountBindFromGlobal:566 — DatatableHelper.GetAccountsFromGlobalByTypeIds(null,
     * {2, 11, 15, 22}): the second argument is withoutTypeIds, so these types are EXCLUDED.
     * Source clsGlobalVariables.AllAccountsWithCustomGroupId (USP_GETAllAccountsFromCustomGroups);
     * distinct by ChartOfAccountId, first row wins.
     */
    public List<Map<String, Object>> stockCreditAccounts(UserAccount u, Set<Integer> withoutTypeIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (withoutTypeIds.contains(toInt(a.get("AccountTypeId")))) continue;
            int id = toInt(a.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(a.get("AccountTitle")));
            o.put("AccountCode", str(a.get("AccountCode")));
            o.put("ParentAccountTitle", str(a.get("ParentAccountTitle")));
            o.put("AccountClass", str(a.get("AccountClassName")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================ voucher reads

    /** BLL 0583 Item.GetByID — {@code GetData(...)[0]}: no row is an index error on the desktop. */
    public Map<String, Object> itemById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        return r.get(0);
    }

    /** BLL 0594 jobLot.GetByID — {@code GetData(...)[0]}. */
    public Map<String, Object> jobLotById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params("Id", id, "Activity", "GetById"));
        if (r.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        return r.get(0);
    }

    /** DAL 0222 :111 — Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int documentTypeSrNo) {
        return shared.voucherHeadId(u, documentTypeId, documentTypeSrNo);
    }

    /** A fresh VoucherHead / VoucherDetail model — the CLR defaults of every non-nullable property. */
    public static Map<String, Object> voucherHeadModel() { return InventoryOpeningDefaults.voucher(); }
    public static Map<String, Object> voucherDetailModel() { return InventoryOpeningDefaults.detail(); }

    // ================================================================================ write

    /** GenericProvider.SetProc — {@code Convert.ToInt32(ExecuteScalar())}. */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0222 :176-187 — Sp_InventoryTransactions_GetALLMethod then Sp_InventoryStockEvalautionDetail_Update. */
    public void postStock(UserAccount u, int documentTypeId, int id) {
        shared.postStock(u, documentTypeId, id);
    }

    // ============================================================= attachments (DAL 0222 :30-80)

    /** FormHelper.LoadAttachmentsForObject → DMSAttachments.GetByID(recId, screenName) — 'ReadById'. */
    public List<Map<String, Object>> attachments(String screenName, int id) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params("ScreenName", screenName, "Id", id, "Activity", "ReadById"));
    }

    /** DAL 0222 :50 — Sp_DMSAttachments_GetAllMethod 'DeleteById' (ScreenName, Id). */
    public void deleteAttachments(String screenName, int id) {
        DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params("ScreenName", screenName, "Id", id, "Activity", "DeleteById"));
    }

    /** DAL 0222 :66-78 — one Proc_DMSAttachments_Insert per attachment. */
    public void insertAttachment(UserAccount u, String screenName, int itemId, int documentTypeId, int id,
                                 String attachment, String customName, double sizeMb) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        DesktopProc.scalar(jdbc, "Proc_DMSAttachments_Insert", params(
                "Id", 0, "RefAccountId", itemId, "DMSFoldersLabelsId", 0, "RefDocumentTypeId", documentTypeId,
                "RefDocumentNo", id, "Attachment", attachment, "EntryDate", now, "EntryUser", u.getId(),
                "ModifyDate", now, "ModifyUser", u.getId(), "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchId", 0, "ScreenName", screenName, "DetailWiseAttachment", 0,
                "UploadedFileCustomName", customName, "UploadedFileSizeMb", sizeMb, "LineId", 0));
    }

    // ============================== Upload Opening (frmItemDefineExcelSheetUpload, ItemDefinitionType 2)

    /** BLL 0584 ItemCategory.Getall — 'ReadByOrganizationCompanyId' (no parent / code filter). */
    public List<Map<String, Object>> itemCategories(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemCategory_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** BLL 0593 ItemType.Getall(ReportsParameters) — 'ReadByOrganizationCompanyId' (TypeId 0 / ItemIds empty: not sent). */
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemType_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** BLL 0609 UOM.Getall — Sp_UOM_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> uomMaster(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_UOM_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** BLL 0379 GetWarehouseRacks → BLL 0507 invWarehouseRack.GetRackswithWarehouse (usp_getRackswithWarehouse). */
    public List<Map<String, Object>> warehouseRacks(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getRackswithWarehouse", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GetGlobalAllAccountsWithCustomGroup — the raw rows (AccountCode → ChartOfAccountId). */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GetConfigValueFromGlobal / GetConfigurationFromAllocation — ConfigKey text ("" when absent). */
    public String config(UserAccount u, String description) { return shared.config(u, description); }

    /** CommonServices.GetERPFeatureById — the id is listed by USP_GetERPFeaturesByCompanyId. */
    public boolean feature(UserAccount u, int id) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == id) return true;
        }
        return false;
    }

    /** BLL 0583 Item.GenerateCode — 'GenerateItemCodeByCategoryId'. */
    public List<Map<String, Object>> generateItemCode(UserAccount u, int itemCategoryId, int itemTypeId) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemCategoryId", itemCategoryId, "ItemTypeId", itemTypeId, "Activity", "GenerateItemCodeByCategoryId"));
    }

    /** Model 1044 Item — every non-virtual property at its CLR default. */
    public static Map<String, Object> itemModel() { return InventoryPosDefaults.item(); }
    /** Model 1054 ItemsReorderSchedule — defaults. */
    public static Map<String, Object> reorderModel() { return InventoryPosDefaults.reorder(); }

    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && "true".equalsIgnoreCase(String.valueOf(v).trim());
    }
}
