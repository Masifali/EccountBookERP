package com.mst.repositories.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 160 "Purchase Analysis" — Architecture.WinApp.Lab/InvLabPurchaseAnalysis.cs (DocumentTypeId 303)
 * and its loader Architecture.WinApp.Lab/PendingGPForPurchaseLab.cs.
 *
 * Every procedure, @Activity and parameter below was read from the decompiled desktop source and checked
 * against procdure.utf8.sql (line numbers "sql:n" are that file's):
 *
 *   BLL 0403 / DAL 0359  Lab.InvLabAnalysisPurchaseHeader
 *        GenerateCode, GetGpDetailByGatepassNo, GetMoistureFromOrderByItemIdAndOrder, GetAll (ReadAll),
 *        ReadById (+ ReadDetailByHeaderId, ReadSubParamsDetailByHeaderId)
 *                                          -> Sp_InvLabAnalysisPurchaseHeader_GetAllMethod        (sql:165033)
 *        Save (SetData)                    -> Sp_InvLabAnalysisPurchaseHeader_Insert              (sql:165654)
 *                                             Sp_InvLabAnalysisPurchaseHeader_Update              (sql:166102)
 *                                             Sp_InvLabAnalysisPurchaseDetail_Insert              (sql:164982)
 *                                             Sp_InvLabAnalysisPurchaseSubParamsDetail_Insert     (sql:166568)
 *        GetDataForDropDownFromLabPurchaseAnaylsis -> [dbo].[USP_GetDataForDropDownFromLabPurchaseAnaylsis] (sql:341893)
 *        GetPestResult / GetPestStatus     -> usp_PestResult (sql:423229) / usp_PestStatus (sql:423242)
 *        UpdatePurchaseAnalysisParameterResultValue -> USP_UpdatePurchaseAnalysisParameterResultValue (sql:468643)
 *        LabAnalysisPurchase_CutsUpdatationById     -> [dbo].[USP_LabAnalysisPurchase_CutsUpdatationById] (sql:403547)
 *   BLL 0567  Inventory.GatePassInward.GetGpSrNoForPurchaseAnalysis -> Sp_GatePassInward_GetAllMethod (sql:93054)
 *             Inventory.GatePassInward.GetDataForDropDownFromGPI    -> [dbo].[USP_GetDataForDropDownFromGPI] (sql:339525)
 *   BLL 0400  Lab.InvLabAnalysisGroup.GetAllOrById                  -> Sp_InvLabAnalysisGroup_GetAllMethod 'ReadAll' (sql:163936)
 *   BLL 0406  Lab.InvLabGroupAnalysisStandards.GetParametersFromGroupStandards
 *                                          -> Sp_InvLabGroupAnalysisStandards_GetAllMethod (sql:167537)
 *   BLL 0407 / DAL 0362  Lab.InvLabSampleAnalysisHeader.GetByID
 *                                          -> Sp_InvLabSampleAnalysisHeader_GetAllMethod 'ReadById',
 *                                             'ReadDetailByHeaderId', 'ReadLabSampleSubParamsDetailByHeaderId' (sql:168884)
 *   BLL 0583  Inventory.Item.GetAll(obj, "1,2,4")                   -> Sp_Item_GetAllMethod 'ReadByOrganizationCompanyId' (sql:198626)
 *   BLL 0052  ItemPricingScheduleForRice.GetItemsFromPricingSchedule / GetRateUomFromItemPricingSchedule /
 *             GetItemRateByItemIdAndUomId  -> Sp_ItemPricingScheduleForRice_GetAllMethod (sql:204011 / 203944 / 203934)
 *   BLL 0025  WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId
 *                                          -> [dbo].[USP_GetWarehousesAllocatedToBranch] (sql:369000)
 *   Globals   Sp_InvCropYear_GetAllMethod, Sp_InvPackingType_GetAllMethod, SP_JobLot_ReadMethod
 *             'GetJobLotGlIdsandName', usp_getAllUomsByCompanyId, SpStaticColumnNames 'LabWeightCutOn',
 *             Sp_ConfigrationsAllocation_GetAllMethod
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a null is
 * never bound (DesktopProc omits it, as ADO.NET omits a CLR null). Nothing here catches: a failing procedure
 * is a failing request.
 */
@Repository
public class PurchaseAnalysisRepository {

    public static final String P_GETALL = "Sp_InvLabAnalysisPurchaseHeader_GetAllMethod";

    private final JdbcTemplate jdbc;

    public PurchaseAnalysisRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ===================================================================== configuration / numbering

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(description) — ConfigKey or "". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    /**
     * BLL 0403 GenerateCode (bll:131): org, comp, DocumentTypeId, FinancialYearId, 'GenerateDocNo', and
     * \@BranchesId only when non-zero. First row's DocNo.
     */
    public int generateCode(UserAccount u, int financialYearId, int documentTypeId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "Activity", "GenerateDocNo");
        if (branch(u) != 0) p.put("BranchesId", branch(u));
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ================================================================================ gate passes

    /**
     * BLL 0567 GetGpSrNoForPurchaseAnalysis: org, comp, FinancialYearId always; @SupplierCustomerId,
     * \@OrderTypeId (the model's RefDocumentTypeId) and @BranchesId only when non-zero.
     * GpNoFill (InvLabPurchaseAnalysis.cs:753) sends no supplier / order type; the loader's FillGrid
     * (PendingGPForPurchaseLab.cs:186) sends its two combos.
     */
    public List<Map<String, Object>> gatePassesForAnalysis(UserAccount u, int financialYearId, int supplierId, int orderTypeId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (orderTypeId != 0) p.put("OrderTypeId", orderTypeId);
        if (branch(u) != 0) p.put("BranchesId", branch(u));
        p.put("Activity", "GetGpSrNoForPurchaseAnalysis");
        return DesktopProc.rows(jdbc, "Sp_GatePassInward_GetAllMethod", p);
    }

    /** BLL 0403 GetGpDetailByGatepassNo (bll:583): org, comp, @InwardGatePassId, 'GetGpDetailByGatepassNo'. */
    public List<Map<String, Object>> gatePassDetail(UserAccount u, int gatePassId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InwardGatePassId", gatePassId, "Activity", "GetGpDetailByGatepassNo"));
    }

    /**
     * PendingGPForPurchaseLab.CombosFill (:97) — GatePassInward.GetDataForDropDownFromGPI(OrganizationId,
     * CompanyId, null, BranchesId). The BLL method's signature is (int CompanyId, int OrganizationId, ...)
     * (bll 0567), so the form's first argument lands in the BLL's "CompanyId" and is sent as @CompanyId and
     * the second as @OrganizationId: the two ids reach the procedure SWAPPED. Reproduced as the desktop
     * sends it (report: quirk D9). @Activity is left out (null); @BranchesIds is the user's branch as text.
     */
    public List<Map<String, Object>> loaderCombos(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getCompanyId(), "CompanyId", u.getOrganizationId());
        p.put("BranchesIds", String.valueOf(branch(u)));
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromGPI]", p);
    }

    /** CommonServices.GetMoistureByItemIdAndPurchaseOrderId (CommonServices.cs:1254) -> bll:599. */
    public double moisture(UserAccount u, int itemId, int purchaseOrderId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "OrderItemId", itemId, "PurchaseOrderId", purchaseOrderId,
                "Activity", "GetMoistureFromOrderByItemIdAndOrder"));
        if (r.isEmpty()) return 0d;
        Object v = r.get(0).values().isEmpty() ? null : ci(r.get(0), "Moisture");
        return toDouble(v);
    }

    // ===================================================================================== lists

    /** BLL 0400 GetAllOrById: @Id / @ParentCategoryId only when non-zero; org, comp, 'ReadAll'. */
    public List<Map<String, Object>> analysisGroups(UserAccount u, int parentCategoryId) {
        Map<String, Object> p = params();
        if (parentCategoryId != 0) p.put("ParentCategoryId", parentCategoryId);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", p);
    }

    /** CommonServices.ItemGetAllServiceBind("1,2,4") (:1488) -> Item.GetAll: org, comp, @ParentIds, 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> itemsByParentCategories(UserAccount u, String parentIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (parentIds != null && !parentIds.isEmpty()) p.put("ParentIds", parentIds);
        p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** BLL 0052 GetItemsFromPricingSchedule: org, comp (DocumentTypeId / RateUomId are 0 -> not sent). */
    public List<Map<String, Object>> itemsFromPricingSchedule(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemPricingScheduleForRice_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetItemsFromPricingSchedule"));
    }

    /** BLL 0052 GetRateUomFromItemPricingSchedule: org, comp, @ItemId. Columns RecordNo, RateUomId, RateUom. */
    public List<Map<String, Object>> rateUomFromPricingSchedule(UserAccount u, int itemId) {
        return DesktopProc.rows(jdbc, "Sp_ItemPricingScheduleForRice_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "GetRateUomFromItemPricingSchedule"));
    }

    /**
     * BLL 0052 GetItemRateByItemIdAndUomId: org, comp, @ItemId, @RateUomId. The form also sets
     * EffectiveDate (:1397) but the BLL never sends it. Columns Id, ItemRate.
     */
    public List<Map<String, Object>> itemRateByItemAndUom(UserAccount u, int itemId, int rateUomId) {
        return DesktopProc.rows(jdbc, "Sp_ItemPricingScheduleForRice_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "RateUomId", rateUomId, "Activity", "GetItemRateByItemIdAndUomId"));
    }

    /** clsGlobalVariables.globalUomSchedule — usp_getAllUomsByCompanyId (org, comp, Active 1). */
    public List<Map<String, Object>> uomSchedule(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1));
    }

    /** BLL 0406 GetParametersFromGroupStandards: @Id (always, even 0), org, comp. */
    public List<Map<String, Object>> parametersFromGroupStandards(UserAccount u, int groupId) {
        return DesktopProc.rows(jdbc, "Sp_InvLabGroupAnalysisStandards_GetAllMethod", params(
                "Id", groupId, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetParametersFromGroupStandards"));
    }

    /** BLL 0025: org, comp, @BranchId only when the user's branch is non-zero. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branch(u) != 0) p.put("BranchId", branch(u));
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetWarehousesAllocatedToBranch]", p);
    }

    /** CommonServices.StaticColumnsService("LabWeightCutOn") — Id / type: 1 Qty, 2 Weight. */
    public List<Map<String, Object>> weightCutOn() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "LabWeightCutOn"));
    }

    public List<Map<String, Object>> pestResults() { return DesktopProc.rows(jdbc, "usp_PestResult", params()); }

    public List<Map<String, Object>> pestStatuses() { return DesktopProc.rows(jdbc, "usp_PestStatus", params()); }

    /** clsGlobalVariables.globalCropYear — Sp_InvCropYear_GetAllMethod org, comp, 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[Sp_InvCropYear_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** clsGlobalVariables.globalInvPackingType — Sp_InvPackingType_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> packingTypes() {
        return DesktopProc.rows(jdbc, "[dbo].[Sp_InvPackingType_GetAllMethod]", params("Activity", "ReadAll"));
    }

    /** clsGlobalVariables.globalJobLot — CommonServices.GetJobLotGlIdsandName(org, comp). */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetJobLotGlIdsandName"));
    }

    /**
     * HistoryComboFill (:617): Activity "" and DocumentTypeIds "" — the BLL (bll:325) sends neither, so
     * only org and comp go. Rows Id / ReferenceName / Activity.
     */
    public List<Map<String, Object>> historyCombos(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromLabPurchaseAnaylsis]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    // ============================================================================ sample analysis

    /** BLL 0407 GetByID — 'ReadById'. */
    public Map<String, Object> sampleHeader(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_InvLabSampleAnalysisHeader_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0362 GetData — 'ReadDetailByHeaderId'. */
    public List<Map<String, Object>> sampleDetails(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabSampleAnalysisHeader_GetAllMethod", params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL 0362 GetData — 'ReadLabSampleSubParamsDetailByHeaderId'. */
    public List<Map<String, Object>> sampleSubParams(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabSampleAnalysisHeader_GetAllMethod", params("Id", id, "Activity", "ReadLabSampleSubParamsDetailByHeaderId"));
    }

    // ==================================================================================== record

    /** BLL 0403 ReadById (bll:241) — the header row; the procedure filters by Id alone. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0359 GetData — 'ReadDetailByHeaderId'. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL 0359 GetData — 'ReadSubParamsDetailByHeaderId'. */
    public List<Map<String, Object>> subParams(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadSubParamsDetailByHeaderId"));
    }

    /**
     * BLL 0403 GetAll (bll:159) — historygridfill (:2380). Always: org, comp, DocumentTypeId,
     * FinancialYearId, CanViewAllRecord, 'ReadAll'. Conditional: FromDate / ToDate (when the picker is
     * ticked), FromDocNo / ToDocNo / InwardGatePassId / SupplierCustomerId / ItemId (non-zero),
     * AnaylstStatus / ApprovedStatus / VehicleNo (non-empty), EntryUser (when not CanViewAllRecord),
     * BranchesId (non-zero). The form never sets NoOfRecords, PestResultId or PestStatusId, so they are
     * never sent. The BLL names the to-date parameter "@ToDate " (trailing space, bll:176); it is sent
     * here as @ToDate, the procedure's parameter.
     */
    public List<Map<String, Object>> readAll(UserAccount u, int financialYearId, int documentTypeId, boolean canViewAll,
                                             Timestamp fromDate, Timestamp toDate, int fromDocNo, int toDocNo,
                                             int gpId, int supplierId, int itemId,
                                             String analystStatus, String approvedStatus, String vehicleNo) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "CanViewAllRecord", canViewAll);
        p.put("FromDate", fromDate);
        p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (gpId != 0) p.put("InwardGatePassId", gpId);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (analystStatus != null && !analystStatus.isEmpty()) p.put("AnaylstStatus", analystStatus);
        if (approvedStatus != null && !approvedStatus.isEmpty()) p.put("ApprovedStatus", approvedStatus);
        if (vehicleNo != null && !vehicleNo.isEmpty()) p.put("VehicleNo", vehicleNo);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (branch(u) != 0) p.put("BranchesId", branch(u));
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ====================================================================================== save

    /** GenericProvider.SetProc (DAL 0207:283) — Convert.ToInt32(ExecuteScalar()); every non-virtual property in declaration order. */
    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    /** BLL 0403 LabAnalysisPurchase_CutsUpdatationById (bll:1087): @Id, @ModifyUserId, @DeductedRate, @DeductedWeight. */
    public void cutsUpdate(int id, int modifyUserId, double deductedRate, double deductedWeight) {
        DesktopProc.scalar(jdbc, "[dbo].[USP_LabAnalysisPurchase_CutsUpdatationById]", params(
                "Id", id, "ModifyUserId", modifyUserId, "DeductedRate", deductedRate, "DeductedWeight", deductedWeight));
    }

    /**
     * BLL 0403 UpdatePurchaseAnalysisParameterResultValue (bll:1052): per row @Id, @ResultValue, @Remarks.
     * AddWithValue with a null Remarks sends nothing — DesktopProc omits a null the same way.
     */
    public void updateParameterResult(int detailId, double resultValue, String remarks) {
        DesktopProc.scalar(jdbc, "USP_UpdatePurchaseAnalysisParameterResultValue", params(
                "Id", detailId, "ResultValue", resultValue, "Remarks", remarks));
    }

    // =================================================================================== helpers

    public static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt on a database value — anything that is not a number is 0. */
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Math.rint(Double.parseDouble(String.valueOf(v).trim())); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble on a database value — anything that is not a number is 0. */
    public static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) { double d = ((Number) v).doubleValue(); return Double.isFinite(d) ? d : 0d; }
        if (v instanceof Boolean) return ((Boolean) v) ? 1d : 0d;
        try { double d = Double.parseDouble(String.valueOf(v).trim().replace(",", "")); return Double.isFinite(d) ? d : 0d; }
        catch (NumberFormatException e) { return 0d; }
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }
}
