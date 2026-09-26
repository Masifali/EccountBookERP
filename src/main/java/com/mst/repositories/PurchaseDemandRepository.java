package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 340 "Purchase Demand" (frmPurchaseDemand.cs, DocumentTypeId 141) — every procedure call
 * the form makes, with the parameter lists exactly as the recovered BLL builds them:
 *
 *   BLL 0557  Architecture.BLL.Inventory.InvPurchaseDemandHeader
 *             (GenerateCode, GetByID, FormHistory, GetlastPurchaseQtyRateandDate,
 *              GetOutstandingDemandAndStockQty, InvPurchaseDemondSlip, Save)
 *   DAL 0410  Architecture.DAL.Inventory.InvPurchaseDemandHeader       (SetData, GetAll)
 *   BLL 0067  Department.GetAll                                         (Sp_Department_GetAllMethod 'ReadAll')
 *   BLL 0223  FixedAssetsRegister.GetAll                                (Sp_FixedAssetsRegister_GetAllMethod 'ReadAll')
 *   BLL 0019  JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId (USP_GetJobLotsAllocatedToBranch)
 *   BLL 0379  GlobalServicesMethods.AllItemsWithModal / getAllUomsByCompanyId (the "AllItems" and
 *             "UomSchedule" globals, loaded by DatatableHelper.GlobalServicesDbCall:366/:382)
 *   BLL 0583  Item.GetItemIdByBarcodeNo                                 (Sp_Item_GetAllMethod)
 *
 * Every parameter was checked against /root/ddl/procs.json. A parameter the BLL adds only under a
 * condition ("if (x != 0)") is only added here under the same condition; a null is never bound
 * (DesktopProc omits it, as ADO.NET does with a CLR null).
 */
@Repository
public class PurchaseDemandRepository {

    public static final String P_GETALL = "Sp_InvPurchaseDemand_GetAllMethod";

    private final JdbcTemplate jdbc;

    public PurchaseDemandRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /** BLL 0557 GenerateCode (:209) — @BranchesId only when non-zero; reads DocNo. */
    public int nextDocNo(UserAccount u, int financialYearId, int branchesId, int documentTypeId) {
        Map<String, Object> p = params(
                "Activity", "GenerateCode",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId,
                "DocumentTypeId", documentTypeId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================== combos

    /** BLL 0379 AllItemsWithModal(org, comp, 0, 0, "") — PageSize/PageNumber/Keyword not sent. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId()));
    }

    /** BLL 0379 getAllUomsByCompanyId(org, comp, 0, 1) — @ItemId not sent, @Active = 1. */
    public List<Map<String, Object>> uomSchedule(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Active", 1));
    }

    /** BLL 0067 Department.GetAll. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll"));
    }

    /** BLL 0223 FixedAssetsRegister.GetAll. */
    public List<Map<String, Object>> fixedAssets(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll"));
    }

    /** BLL 0019 GetJobLotsAllocatedToBranchByBranchId — @BranchId only when non-zero. */
    public List<Map<String, Object>> jobLots(UserAccount u, int branchesId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
        if (branchesId != 0) p.put("BranchId", branchesId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
    }

    /** BLL 0583 Item.GetItemIdByBarcodeNo — the Id of the first row, 0 when none. */
    public int itemIdByBarcode(UserAccount u, String barcodeNo) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BarcodeNo", barcodeNo,
                "Activity", "GetItemIdByBarcodeNo"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ============================================================================ entry bar

    /** BLL 0557 GetlastPurchaseQtyRateandDate (:646) — @BranchesId only when non-zero. */
    public List<Map<String, Object>> lastPurchase(UserAccount u, int branchesId, int documentTypeId, int itemId) {
        Map<String, Object> p = params(
                "Activity", "GetlastPurchaseQtyRateandDate",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "ItemId", itemId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    /** BLL 0557 GetOutstandingDemandAndStockQty (:693) — @ItemConditionId / @RecId only when non-zero. */
    public List<Map<String, Object>> outstandingDemandAndStock(UserAccount u, int itemId, Timestamp docDate,
                                                               int itemConditionId, int recId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate);
        if (itemConditionId != 0) p.put("ItemConditionId", itemConditionId);
        if (recId != 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "USP_InvPurchaseDemand_GetOutstandingDemandAndStockQty", p);
    }

    // ============================================================================== history

    /**
     * BLL 0557 FormHistory (:56). Every guard is the BLL's. The one exception is the doc-no pair,
     * see {@link com.mst.services.PurchaseDemandService} deviation D1: the BLL names them
     * {@code @FromDocNo}/{@code @ToDocNo}, which the procedure does not declare (it declares
     * {@code @DocNoFrom}/{@code @DocNoTo}); they are sent under the procedure's names.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, int branchesId, int parentCategoryId,
                                                 boolean canViewAll, int entryUser,
                                                 Timestamp fromDate, Timestamp toDate,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo,
                                                 int fromDocNo, int toDocNo) {
        Map<String, Object> p = params(
                "Activity", "FormHistory",
                "organizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
        if (financialYearId != 0)  p.put("FinancialYearId", financialYearId);
        if (branchesId != 0)       p.put("BranchesId", branchesId);
        if (parentCategoryId != 0) p.put("ParentCategoryId", parentCategoryId);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll)           p.put("EntryUser", entryUser);
        if (fromDate != null)      p.put("FromDate", fromDate);
        if (toDate != null)        p.put("ToDate", toDate);
        if (entryFrom != null)     p.put("EntryFromDate", entryFrom);
        if (entryTo != null)       p.put("EntryToDate", entryTo);
        if (modifyFrom != null)    p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null)      p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null)  p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null)    p.put("ApprovedToDate", approvedTo);
        if (fromDocNo != 0)        p.put("DocNoFrom", fromDocNo);   // BLL: @FromDocNo (D1)
        if (toDocNo != 0)          p.put("DocNoTo", toDocNo);       // BLL: @ToDocNo   (D1)
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ================================================================================= read

    /** BLL 0557 GetByID → DAL 0410 GetAll, header part ('ReadById' is SELECT *). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0410 GetAll, detail part — 'InvPurchaseDemandDetailReadById'. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvPurchaseDemandDetailReadById"));
    }

    /** BLL 0557 InvPurchaseDemondSlip → Sp_InvPurchaseDemand_Rpt. */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseDemand_Rpt", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", id));
    }

    // ================================================================================ write

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()), 0 when no row. */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0410 SetData — USP_InvPurchaseDemand_DeleteDetailRows, ExecuteNonQuery. */
    public void deleteDetailRows(int id, String detailIdsToDelete) {
        DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseDemand_DeleteDetailRows]", params(
                "Id", id,
                "DetailIdsToDelete", detailIdsToDelete));
    }
}
