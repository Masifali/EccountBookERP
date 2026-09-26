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
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 320 "Stock Adjustment" (frmStockAdjustment.cs, DocumentTypeId 70) — every procedure the
 * form, its BLL/DAL and its loader dialog call. Names, activities and the "only when non-zero"
 * conditions were read from:
 *
 *   BLL 0546 Architecture.BLL.Inventory.InvStockAdjustment   (GenerateCode, FormHistory, GetByID,
 *                                                            StockAdjustmentSlipAndRegister409,
 *                                                            GetDataForDropDownFromStockAdjustment)
 *   DAL 0401 Architecture.DAL.Inventory.InvStockAdjustment   (SetData, GetDate)
 *   DAL 0205 CommonServices                                  (FIFOImplemention, GetEqvilentByItemIdAndUomScheduleId,
 *                                                            GetERPFeaturesByCompanyId, GetVoucherHeadId)
 *   BLL 0056 GetAvgRatesAndStockInHand                       (AvgRateOnlyForCGS, GetStockByFifoMethod,
 *                                                            GetStockInHandFromInventoryTrasactions)
 *   BLL 0025 / 0019 Warehouses / JobLots AllocationToBranch, BLL 0571 InvCropYear, BLL 0579 InvPackingType,
 *   BLL 0583 Item.ReadAllItems, BLL 0610 UOMSchedule.SearchByObject, BLL 0648 COAAllocation,
 *   BLL 0136 GeneralReprots.StaticColumnNames, BLL 0141 VoucherReports.VoucherValidationReport,
 *   BLL 0125 StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists,
 *   BLL 0574 InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuance, BLL 0084 tblUserRights.
 *
 * A null is never bound (DesktopProc omits it), which is what ADO.NET does with a CLR null.
 * Shared helpers (config, item GL list, voucher head id, setProc) are StoreIssuanceRepository's.
 */
@Repository
public class StockAdjustmentRepository {

    public static final String P_GETALL = "Sp_InvStockAdjustment_GetAllMethod";
    public static final int DOCUMENT_TYPE_ID = 70;

    private final JdbcTemplate jdbc;
    public StockAdjustmentRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================ numbering

    /** BLL 0546 GenerateCode — Rows[0]["DocNo"]. */
    public int nextDocNo(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "FinancialYearId", financialYearId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // =================================================================================== combos

    /** EntryTypeBind — CommonServices.StaticColumnsService("StockAdjustmentType") → SpStaticColumnNames. */
    public List<Map<String, Object>> entryTypes() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "StockAdjustmentType"));
    }

    /** WareHouseFill — BLL 0025 GetWarehousesAllocatedToBranchByBranchId (@BranchId only when != 0). */
    public List<Map<String, Object>> warehouses(UserAccount u, int branchId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("BranchId", branchId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetWarehousesAllocatedToBranch]", p);
    }

    /** JobLotFill — BLL 0019 GetJobLotsAllocatedToBranchByBranchId (@BranchId only when != 0). */
    public List<Map<String, Object>> jobLots(UserAccount u, int branchId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("BranchId", branchId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
    }

    /** CropYearFill — CommonServices.CropYearGetAllService → BLL 0571 InvCropYear.Getall ('ReadAll'). */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** ItemBind — CommonServices.ReadAllItems → BLL 0583 Item.ReadAllItems. */
    public List<Map<String, Object>> items(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllItems"));
    }

    /** CmbPackingTypeFill — BLL 0579 InvPackingType.Getall() — @Activity only, no tenancy parameter. */
    public List<Map<String, Object>> packingTypes() {
        return DesktopProc.rows(jdbc, "Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** bindRateUomAndItemPackUom — CommonServices.GetUomScheduleByItemId → BLL 0610 SearchByObject ('ReadByItemID'). */
    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /**
     * CmbEntryType_Leave — CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids) → BLL 0648
     * GetAccountTitleByAccountTypeIds. The wrapper fills AppId and UserId from the signed-in user;
     * CostCenterId / NotReferred / RecId are 0 and the class-id strings "" — all left out.
     */
    public List<Map<String, Object>> accountsByTypeIds(UserAccount u, String accountTypeIds) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "AppId", u.getAppId() == null ? 0 : u.getAppId());
        if (accountTypeIds != null && !accountTypeIds.isEmpty()) p.put("AccountTypeIds", accountTypeIds);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /**
     * HistoryCombosFill — BLL 0546 GetDataForDropDownFromStockAdjustment(org, comp, "StockAdjustmentType").
     * The BLL's guard {@code (Activity != "" || Activity != null)} is always true.
     */
    public List<Map<String, Object>> historyAdjustmentTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromStockAdjustment]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "StockAdjustmentType"));
    }

    // ============================================================================ features / rights

    /** CommonServices.GetERPFeaturesByCompanyId / GetERPFeatureById — the ids in USP_GetERPFeaturesByCompanyId. */
    public Set<Integer> erpFeatures(UserAccount u) {
        Set<Integer> out = new HashSet<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            out.add(toInt(r.get("Id")));
        }
        return out;
    }

    /** BLL 0084 tblUserRights.GetByUserId — the raw grant rows of one screen. */
    public List<Map<String, Object>> userRights(UserAccount u, String screenName, String roleName) {
        return DesktopProc.rows(jdbc, "Sp_tblUserRights_GetAllMethod", params(
                "UserId", u.getId(),
                "ScreenName", screenName,
                "RightName", roleName,
                "CompanyId", u.getCompanyId(),
                "Activity", "GetByUserId"));
    }

    /** Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId — clsGlobalVariables.ActiveYr.Start_Period. */
    public String financialYearStart(UserAccount u, int yearId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == yearId) {
                Object v = r.get("Start_Period");
                return v == null ? null : String.valueOf(v).substring(0, 10);
            }
        }
        return null;
    }

    // ================================================================================ stock / rate

    /** BLL 0056 GetStockInHandFromInventoryTrasactions — Rows[0][0], 0 when no row. */
    public double stockInHand(UserAccount u, int itemId, Timestamp docDate, int jobLotId, int warehouseId, String cropYear) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        p.put("Activity", "GetStockInHandFromInventoryTrasactions");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        if (r.isEmpty()) return 0d;
        Object first = r.get(0).values().isEmpty() ? null : r.get(0).get("CurrStockByItem");
        return StoreIssuanceRepository.toDouble(first);
    }

    /** BLL 0056 AvgRateOnlyForCGS — Rows[0]["AvgRate"]. CropYear is always null from this form. */
    public double avgRateOnlyForCgs(UserAccount u, int itemId, Timestamp docDate, int documentTypeId, int recId,
                                    int jobLotId, int cropYearId, int warehouseId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("RecId", recId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        p.put("Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? 0d : StoreIssuanceRepository.toDouble(r.get(0).get("AvgRate"));
    }

    /**
     * USP_GetStockByFifoMethod — the parameters of BLL 0056 GetStockByFifoMethod and of DAL 0205
     * FIFOImplemention (same names, same "only when non-zero" rules). {@code fifoXml} is sent only by
     * FIFOImplemention, and only when earlier lines of the same save reserved stock.
     */
    public List<Map<String, Object>> stockByFifo(UserAccount u, int itemId, Timestamp docDate, int packUomId,
                                                 int warehouseId, int jobLotId, int packingTypeId, int cropYearId,
                                                 String cropYear, int documentTypeId, int id, String fifoXml) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate);
        if (packUomId != 0) p.put("PackUomId", packUomId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("PackingTypeId", packingTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("Id", id);
        if (fifoXml != null) p.put("FIFOXML", fifoXml);
        return DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p);
    }

    /** DAL 0205 GetEqvilentByItemIdAndUomScheduleId — Rows[0]["Equivalent"], 0 when none. */
    public double equivalent(UserAccount u, int itemId, int scheduleId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "ScheduleId", scheduleId,
                "Activity", "GetEqvilentByItemIdAndUomScheduleId"));
        return r.isEmpty() ? 0d : StoreIssuanceRepository.toDouble(r.get(0).get("Equivalent"));
    }

    // ================================================================================== history

    /** BLL 0546 FormHistory — parameters and conditions exactly as the BLL adds them. */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll, int entryUser,
                                                 Timestamp from, Timestamp to, int fromDocNo, int toDocNo,
                                                 int adjustmentTypeId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "FinancialYearId", financialYearId,
                "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", entryUser);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (adjustmentTypeId != 0) p.put("AdjustmentTypeId", adjustmentTypeId);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ===================================================================================== read

    /** BLL 0546 GetByID → DAL GetDate: the header ('ReadById'), or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL GetDate — 'ReadDetailByHeaderId' for the header's Id. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** CommonServices.StockAdjustmentSlipAndRegister409 → BLL 0546 (org, company, @Id). */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (id != 0) p.put("Id", id);
        return DesktopProc.rows(jdbc, "Sp_StockAdjustmentSlipAndRegister", p);
    }

    /**
     * CommonServices.VoucherReport_118(VoucherHeadId) → BLL 0141 VoucherValidationReport with
     * UserId and Id set, DocumentTypeId 0 and ApprovedFilter "All" (both therefore left out).
     */
    public List<Map<String, Object>> voucherSlip(UserAccount u, int voucherHeadId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        if (voucherHeadId != 0) p.put("Id", voucherHeadId);
        return DesktopProc.rows(jdbc, "Sp_Accounts_VouchersValidation_Rpt", p);
    }

    // =================================================================================== loader

    /** LoadavailableTransactionsForIssuance.StockComboFill — BranchesIds = the user's branch as text. */
    public List<Map<String, Object>> loaderDropDowns(UserAccount u, int branchId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        String b = String.valueOf(branchId);
        if (!b.isEmpty()) p.put("BranchesIds", b);
        return DesktopProc.rows(jdbc, "USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    /** BLL 0574 GetAvailableTransactionsForIssuance — each filter only when set, as the BLL adds it. */
    public List<Map<String, Object>> availableTransactions(UserAccount u, int branchId, Timestamp from, Timestamp to,
                                                           int supplierCustomerId, int warehouseId, int itemId,
                                                           int refDocumentTypeId, int jobLotId, int parentCategoryId,
                                                           int itemCategoryId, int itemTypeId, String cropYear) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("BranchesId", branchId);
        if (from != null) p.put("DateFrom", from);
        if (to != null) p.put("DateTo", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (parentCategoryId != 0) p.put("InventoryParentCategories", parentCategoryId);
        if (itemCategoryId != 0) p.put("ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("ItemTypeId", itemTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        return DesktopProc.rows(jdbc, "SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance", p);
    }

    // ==================================================================================== write

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0401 :190 — ExecuteNonQuery, all four parameters always sent. */
    public void reverseInventoryQty(UserAccount u, int id) {
        DesktopProc.scalar(jdbc, "[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "RefDocumentTypeId", DOCUMENT_TYPE_ID,
                "RefDocIdNo", id));
    }

    /** DAL 0401 :238-256 — one USP_InventoryValidation per detail, every parameter sent (AddWithValue of ints). */
    public void inventoryValidation(UserAccount u, Timestamp docDate, Map<String, Object> d) {
        DesktopProc.scalar(jdbc, "USP_InventoryValidation", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "DocDate", docDate,
                "ItemId", d.get("ItemId"),
                "WarehouseId", d.get("WarehouseId"),
                "JobLotId", d.get("JobLotId"),
                "CropYearId", d.get("CropYearId"),
                "InvPackingTypeId", d.get("PackingTypeId"),
                "PackUomId", d.get("PackUomId"),
                "NetWeight", d.get("NetWeight"),
                "RefDocumentTypeId", d.get("RefDocumentTypeId"),
                "RefDocNoId", d.get("RefDocNoId"),
                "RefDocSubIdNo", d.get("RefDocSubIdNo"),
                "ItemConditionId", d.get("ItemConditionId")));
    }

    /** The evaluation-update / transactions-recalc pair, each as DAL 0401 builds it (defaults of the model). */
    public void evaluationUpdate(UserAccount u, int id) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
        e.put("RefDocIdNo", id);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    public void transactionsRecalc(UserAccount u, int id) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
        t.put("RefDocIdNo", id);
        setProc("Sp_InventoryTransactions_GetALLMethod", t);
    }

    /** InventoryStockEvalautionDetail (model 1023) non-virtual properties with the CLR defaults. */
    public static Map<String, Object> evaluationModel() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("DocDate", null);
        for (String k : new String[] { "AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate",
                "ExpenseAmountIn", "ItemRate", "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut", "CgsAmount" }) {
            m.put(k, 0d);
        }
        for (String k : new String[] { "BranchesId", "CompanyId", "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId",
                "ItemId", "ItemUom", "JobLotId", "OrderNo", "OrganizationId", "PrdJobOrderNo", "ProjectsId", "RateUom",
                "RefDocIdNo", "RefDocSubIdNo", "RefDocumentTypeId", "OtherDocumentTypeId", "OtherDocNoId",
                "OtherSubDocNoId", "SupplierCustomerId", "WarehouseId", "RefWarehouseId", "CityId", "LineId",
                "RefRefDocumentTypeId", "RefRefDocIdNo", "RefRefDocSubIdNo", "EntryUser", "ModifyUser", "InvoiceId",
                "InvoiceDetailId", "VarientId", "ItemConditionId" }) {
            m.put(k, 0);
        }
        m.put("CalcType", null);
        m.put("CropBatch", null);
        m.put("TranRemarks", null);
        m.put("VehicleNo", null);
        m.put("BiltyNo", null);
        return m;
    }

    /** Projects rows to {Id, Name} — the value/display pair a combo binds. */
    public static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get(id)));
            o.put("Name", str(r.get(name)));
            out.add(o);
        }
        return out;
    }
}
