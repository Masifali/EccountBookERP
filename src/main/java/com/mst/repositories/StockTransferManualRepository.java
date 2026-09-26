package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 332 "Stock Transfer Manual" (frmStockTransferManual.cs, DocumentTypeId 806) — only the reads and
 * writes that differ from screen 339. Everything the two forms share (GenerateCode, FormHistory,
 * ReadById/ReadByHeaderId/ReadExpenseDetailByHeaderId, AllComboAgainstStockTransfer, item / packing /
 * crop-year / UOM / COA combos, stock and avg-rate reads, the loader dialog's two procedures, FIFO and
 * posting helpers) is called on {@link StoreStockTransferRepository}, unchanged.
 *
 *   BLL 0025 WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId → [dbo].[USP_GetWarehousesAllocatedToBranch]
 *   BLL 0019 JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId      → [dbo].[USP_GetJobLotsAllocatedToBranch]
 *   BLL 0559 InvStockTransferHeader.StockTransferSlipandRegister (CommonServices.StockTransferSlip407, DocumentTypeId 806)
 *   BLL 0559 InvStockTransferHeader.GetRecordsById (Save:185, 'GetRecordId')
 *   DAL 0412 SetData:333 [dbo].[USP_InvStockTransferDetailRowsDeleteByIds] (DocumentTypeId 806 only)
 */
@Repository
public class StockTransferManualRepository {

    private final JdbcTemplate jdbc;
    public StockTransferManualRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** WareHouseFill:1224 — BLL 0025: @BranchId only when BranchesId != 0. Returns Id, WareHouseName, … */
    public List<Map<String, Object>> warehousesAllocatedToBranch(UserAccount u, int branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchesId != 0) p.put("BranchId", branchesId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetWarehousesAllocatedToBranch]", p);
    }

    /** GetJobLot:1400 — BLL 0019: @BranchId only when BranchesId != 0. Returns Id, JobLotDescription, … */
    public List<Map<String, Object>> jobLotsAllocatedToBranch(UserAccount u, int branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchesId != 0) p.put("BranchId", branchesId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
    }

    /** CommonServices.StockTransferSlip407:7755 → BLL 0559 StockTransferSlipandRegister (DocumentTypeId 806, Id). */
    public List<Map<String, Object>> slip407(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("Id", id);
        return DesktopProc.rows(jdbc, "Sp_InvStockTransfer_SlipandRegister", p);
    }

    /**
     * BLL 0559 GetRecordsById — Save:185 reads it into the virtual PreviousDate before an update; the
     * unguarded Rows[0][0] throws when the row is gone.
     */
    public Object recordDate(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, StoreStockTransferRepository.P_GETALL,
                params("Id", id, "Activity", "GetRecordId"));
        if (r.isEmpty()) throw new IllegalArgumentException("There is no row at position 0.");
        return r.get(0).values().iterator().next();
    }

    /** DAL 0412 SetData:333 — only for DocumentTypeId 806 with a non-empty DetailRowsRemoveIds (ExecuteScalar). */
    public void detailRowsDelete(UserAccount u, int documentTypeId, int id, String detailIds) {
        DesktopProc.scalar(jdbc, "[dbo].[USP_InvStockTransferDetailRowsDeleteByIds]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "Id", id,
                "DetailIds", detailIds));
    }

    /**
     * Web-only guard (Deviation 6): a loader reference must be a stock-evaluation row of the user's own
     * organization / company — the table SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance
     * lists the loader rows from.
     */
    public boolean loaderReferenceExists(UserAccount u, int refDocumentTypeId, int refDocIdNo, int refDocSubIdNo) {
        Integer n = jdbc.queryForObject(
                "SELECT COUNT(1) FROM dbo.InventoryStockEvalautionDetail WITH (NOLOCK) "
                        + "WHERE OrganizationId = ? AND CompanyId = ? AND RefDocumentTypeId = ? AND RefDocIdNo = ? AND RefDocSubIdNo = ?",
                Integer.class, u.getOrganizationId(), u.getCompanyId(), refDocumentTypeId, refDocIdNo, refDocSubIdNo);
        return n != null && n > 0;
    }
}
