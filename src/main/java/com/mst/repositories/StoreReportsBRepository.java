package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Date;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Procedure calls for the Store Management report group B (module 46): 454 Stock Adjustment
 * Report, 455 Department Request History, 456 Stock Transfer Report.
 *
 * Every call reproduces its BLL method's parameter list and guards: a guarded parameter that is
 * unset is passed as null here, which {@link DesktopProc} OMITS (ADO.NET AddWithValue(null)
 * semantics) - the BLLs simply do not add it. Every parameter sent was checked against the
 * procedure header in procs.json.
 *
 * BLL files: 0546 InvStockAdjustment, 0068 DepartmentRequest, 0559 InvStockTransferHeader,
 * 0571 InvCropYear, 0582 InvWareHouse, 0594 jobLot, 0583 Item, 0136 GeneralReprots.
 */
@Repository
public class StoreReportsBRepository {

    private final JdbcTemplate jdbc;
    public StoreReportsBRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ======================================================================= shared lookups

    /** CommonServices.CropYearGetAllService -> BLL 0571 InvCropYear.Getall -> Sp_InvCropYear_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll"));
    }

    /** CommonServices.WareHouseGetAllService -> BLL 0582 InvWareHouse.Getall -> Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"));
    }

    /** CommonServices.JobLotGetAllService -> BLL 0594 jobLot.GetAll -> SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GetAll"));
    }

    /** CommonServices.StaticColumnsService(activity) -> BLL 0136 GeneralReprots.StaticColumnNames -> SpStaticColumnNames. */
    public List<Map<String, Object>> staticColumns(String activity) {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", activity));
    }

    /** CommonServices.ReadAllItems -> BLL 0583 Item.ReadAllItems -> Sp_Item_GetAllMethod 'ReadAllItems'. */
    public List<Map<String, Object>> readAllItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAllItems"));
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the active year row whose Id is the session's year. */
    public String financialYearStart(UserAccount u, int financialYearId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(ci(r, "Id")) == financialYearId) {
                Object v = ci(r, "Start_Period");
                return v == null ? null : String.valueOf(v).substring(0, 10);
            }
        }
        return null;
    }

    // ======================================================================= 454 Stock Adjustment Report

    /**
     * BLL 0546 InvStockAdjustment.StockAdjustmentSlipAndRegister409 -> Sp_StockAdjustmentSlipAndRegister.
     * Guards (BLL :360-440): dates by CheckDateTimeNull, every id by != 0. The BLL's ActionId goes
     * to @AdjustmentTypeId.
     */
    public List<Map<String, Object>> stockAdjustmentRegister(UserAccount u, Date fromDate, Date toDate, int id,
                                                             int itemId, int jobLotId, int warehouseId,
                                                             int actionId, int cropYearId) {
        return DesktopProc.rows(jdbc, "Sp_StockAdjustmentSlipAndRegister", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FromDate", fromDate,
                "ToDate", toDate,
                "Id", nz(id),
                "ItemId", nz(itemId),
                "JobLotId", nz(jobLotId),
                "WarehouseId", nz(warehouseId),
                "AdjustmentTypeId", nz(actionId),
                "CropYearId", nz(cropYearId)));
    }

    // ======================================================================= 455 Department Request History

    /** BLL 0068 GetDataForDropDownFromDepartmentRequest -> [dbo].[USP_GetDataForDropDownFromDepartmentRequest]; Activity unset -> omitted. */
    public List<Map<String, Object>> departmentRequestDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromDepartmentRequest]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId()));
    }

    /**
     * BLL 0068 DepartmentRequestHistory -> Sp_DepartmentRequest_History. Parameter order and guards
     * as BLL :341-455. @FinancialYearId / @PageSize / @PageNumber are never set by this form (0 ->
     * not sent; they are not declared by the procedure either). isApproved null = ApprovedFilter
     * "All" (BLL :420 omits @IsApproved).
     */
    public List<Map<String, Object>> departmentRequestHistory(UserAccount u, int documentTypeId, int id,
                                                              int departmentId, int itemId, int assetId,
                                                              Date fromDate, Date toDate, Boolean isApproved,
                                                              int itemConditionId) {
        return DesktopProc.rows(jdbc, "Sp_DepartmentRequest_History", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", nz(documentTypeId),
                "Id", nz(id),
                "DepartmentId", nz(departmentId),
                "ItemId", nz(itemId),
                "AssetId", nz(assetId),
                "FromDate", fromDate,
                "ToDate", toDate,
                "IsApproved", isApproved,
                "ItemConditionId", nz(itemConditionId)));
    }

    // ======================================================================= 456 Stock Transfer Report

    /** BLL 0559 GetBranchesAllocatedToUserFromStockTransfer(org, company, user, 68) -> [dbo].[USP_GetBranchsAllocatedToUserFromSTockTransfer]. */
    public List<Map<String, Object>> stockTransferBranches(UserAccount u, int documentTypeId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromSTockTransfer]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "UserId", u.getId(),
                "DocumentTypeId", nz(documentTypeId)));
    }

    /**
     * BLL 0559 AllComboAgainstStockTransfer -> Usp_AllComboAgainstStockTransfer. The form sets
     * obj.DocumentTypeId = 608, but the BLL only reads obj.DocumentTypeIds (never set) - so no
     * document-type parameter is sent. Activity unset -> omitted. BranchesIds sent when non-empty.
     */
    public List<Map<String, Object>> stockTransferCombos(UserAccount u, String branchesIds) {
        return DesktopProc.rows(jdbc, "Usp_AllComboAgainstStockTransfer", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesIds", blankToNull(branchesIds)));
    }

    /** BLL 0559 StockTransferSlipandRegister (:426-521) -> Sp_InvStockTransfer_SlipandRegister. */
    public List<Map<String, Object>> stockTransferRegister(UserAccount u, int documentTypeId, int id, int itemId,
                                                           Date fromDate, Date toDate, int fromWarehouseId,
                                                           int toWarehouseId, int jobLotId, int toJobLotId,
                                                           String branchesIds) {
        return DesktopProc.rows(jdbc, "Sp_InvStockTransfer_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", nz(documentTypeId),
                "Id", nz(id),
                "ItemId", nz(itemId),
                "FromDate", fromDate,
                "ToDate", toDate,
                "FromWarehouseId", nz(fromWarehouseId),
                "ToWarehouseId", nz(toWarehouseId),
                "JobLotId", nz(jobLotId),
                "ToJobLotId", nz(toJobLotId),
                "BranchesIds", blankToNull(branchesIds)));
    }

    // ======================================================================= helpers

    /** BLL guard {@code if (x != 0)}: 0 is not sent. */
    private static Integer nz(int v) { return v == 0 ? null : v; }

    /** BLL guard {@code if (s != null && s != string.Empty)}. */
    private static String blankToNull(String s) { return s == null || s.isEmpty() ? null : s; }
}
