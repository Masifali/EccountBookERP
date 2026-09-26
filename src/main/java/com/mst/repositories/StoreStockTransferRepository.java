package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 339 "Stock Transfer" (frmStockTransfer.cs, DocumentTypeId 68) — every read and write the
 * form, its BLL and its DAL make, with the BLL's parameter names and conditions.
 *
 *   BLL 0559 Architecture.BLL.Inventory.InvStockTransferHeader   (GenerateCode, FormHistory, GetByID,
 *            PendingRecordsForStockTransfer, GetItemIdFromDeliveryOrderForStockTransfer,
 *            GetBranchDataFromStockTransfer(4 args), AllComboAgainstStockTransfer,
 *            StockTransferSlipandRegister, Save/MakeVoucher, GetRecordsById)
 *   DAL 0412 Architecture.DAL.Inventory.InvStockTransferHeader   (SetData, GetData)
 *   BLL 0550 GatePassGeneral (GetGatePassNoForStockTransfer, GetTicketNoForStockTransfer,
 *            GetTicketNoMoveOrderBaseForStockTransfer, GetNetWeightByTicketId)
 *   BLL 0582 InvWareHouse.GetActiveWareHouse, 0594 jobLot.GetAll, 0571 InvCropYear.Getall,
 *            0579 InvPackingType.Getall, 0583 Item.ReadAllItems, 0610 UOMSchedule.SearchByObject,
 *            0648 COAAllocation.GetAll, 0580 InvSaleInvoice.GetWeightCurrStockByItem,
 *            0056 GetAvgRatesAndStockInHand.AvgRateOnlyForCGS
 *   BLL 0125 StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists and
 *   BLL 0574 InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuance
 *            (the LoadavailableTransactionsForIssuance dialog)
 *   DAL 0205 CommonServices: FIFOImplemention (USP_GetStockByFifoMethod), GetEqvilentByItemIdAndUomScheduleId,
 *            GetUomScheduleIdByItemIdAndEquivalent, GetERPFeaturesByCompanyId, GetItemGlIdsandItemName
 */
@Repository
public class StoreStockTransferRepository {

    public static final String P_GETALL = "Sp_InvStockTransferHeader_GetAllMethod";

    private final JdbcTemplate jdbc;
    public StoreStockTransferRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /** BLL 0559 GenerateCode — Activity 'GenrateCode' (sic); the form never sets BranchesId. */
    public int generateCode(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenrateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================ combos

    /** CommonServices.getActiveWareHouse → BLL 0582 GetActiveWareHouse. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetActiveWareHouse"));
    }

    /** BLL 0579 InvPackingType.Getall — @Activity only. */
    public List<Map<String, Object>> packingTypes() {
        return DesktopProc.rows(jdbc, "Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** CommonServices.CropYearGetAllService → BLL 0571 InvCropYear.Getall. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** CommonServices.JobLotGetAllService → BLL 0594 jobLot.GetAll. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** CommonServices.ReadAllItems → BLL 0583 Item.ReadAllItems. */
    public List<Map<String, Object>> readAllItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllItems"));
    }

    /** CommonServices.CoaAllocationGetAllServiceBind → BLL 0648 COAAllocation.GetAll. */
    public List<Map<String, Object>> coaAllocation(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "COAAllocationSearch");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /** bindRateUomAndItemPackUom → BLL 0610 UOMSchedule.SearchByObject. */
    public List<Map<String, Object>> uomsByItem(UserAccount u, int itemId) {
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "Activity", "ReadByItemID"));
    }

    /** CombGatePassNoFill:470 → BLL 0550 GetGatePassNoForStockTransfer (every parameter unconditional). */
    public List<Map<String, Object>> gatePasses(UserAccount u, int financialYearId, int branchesId, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassInward_GetAllMethod", params(
                "Activity", "GetGatePassNoForStockTransfer",
                "DocumentTypeId", documentTypeId,
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId,
                "BranchesId", branchesId));
    }

    /** combTicketNo:519 → BLL 0550 GetTicketNoForStockTransfer (BranchesId never set by the form → omitted). */
    public List<Map<String, Object>> tickets(UserAccount u, int financialYearId, int gatePassId, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassGeneral_GetAllMethod", params(
                "Activity", "GetTicketNoForStockTransfer",
                "DocumentTypeId", refDocumentTypeId,
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", gatePassId,
                "FinancialYearId", financialYearId));
    }

    /** GetSlipNoForMoveOrder:895 → BLL 0550 GetTicketNoMoveOrderBaseForStockTransfer. */
    public List<Map<String, Object>> moveOrderTickets(UserAccount u, int financialYearId, int branchesId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassGeneral_GetAllMethod", params(
                "Activity", "GetTicketNoMoveOrderBaseForStockTransfer",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId,
                "BranchesId", branchesId));
    }

    /** cmbTicketNo_Leave:1038 → BLL 0550 GetNetWeightByTicketId (FinancialYearId not set → omitted). */
    public Map<String, Object> netWeightByTicket(UserAccount u, int ticketId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GatePassGeneral_GetAllMethod", params(
                "Activity", "GetNetWeightByTicketId",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", ticketId));
        return r.isEmpty() ? null : r.get(0);
    }

    /** GetItemIdFromDeliveryOrderForStockTransfer:571 → BLL 0559 (the gate pass id travels as @Id). */
    public List<Map<String, Object>> itemsFromDeliveryOrder(UserAccount u, int gatePassId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", gatePassId,
                "Activity", "GetItemIdFromDeliveryOrderForStockTransfer"));
    }

    /** PendingRecordForStockTransfer:1403 → BLL 0559 PendingRecordsForStockTransfer (model overload). */
    public List<Map<String, Object>> pending(UserAccount u, int financialYearId, int branchesId) {
        return DesktopProc.rows(jdbc, "Sp_InvStockTransferHeader_PendingRecordsForStockTransfer", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "BranchesId", branchesId));
    }

    /** BindDetailFromLoader:3482 → BLL 0559 GetBranchDataFromStockTransfer(org, comp, branch, gp). */
    public List<Map<String, Object>> branchData(UserAccount u, int branchesId, int gatePassId) {
        return DesktopProc.rows(jdbc, "USP_GetBranchDataFromStockTransfer", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId, "GpInwardId", gatePassId));
    }

    /** AvailableStockGetByItem:3220 → CommonServices.GetWeightCurrStockByItem → BLL 0580. */
    public List<Map<String, Object>> weightCurrStock(UserAccount u, int itemId, Timestamp toDate,
                                                     int warehouseId, int jobLotId, String cropYear) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "ToDate", toDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        String c = cropYear == null ? "" : cropYear;                 // Conversion.ToString
        if (!c.isEmpty()) p.put("CropYear", c);
        p.put("Activity", "GetWeightCurrStockByItem");
        return DesktopProc.rows(jdbc, "[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /** CommonServices.AvgRateOnlyForCGS → BLL 0056 AvgRateOnlyForCGS (BranchesId default 0 → omitted). */
    public double avgRateOnlyForCgs(UserAccount u, int itemId, Timestamp docDate, int documentTypeId, int recId,
                                    int jobLotId, int cropYearId, String cropYear, int warehouseId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDate", docDate);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("RecId", recId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        p.put("Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? 0d : toDouble(r.get(0).get("AvgRate"));
    }

    /** HistoryComboFill:2669 → BLL 0559 AllComboAgainstStockTransfer, DocumentTypeIds "68". */
    public List<Map<String, Object>> allCombo(UserAccount u, String documentTypeIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        return DesktopProc.rows(jdbc, "Usp_AllComboAgainstStockTransfer", p);
    }

    /** BLL 0559 FormHistory — parameter names and conditions as the BLL builds them. */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, boolean canViewAll, int financialYearId,
                                                 int branchesId, int entryUser,
                                                 Timestamp docFrom, Timestamp docTo, Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo, Timestamp apprFrom, Timestamp apprTo,
                                                 int fromDocNo, int toDocNo, String transferType) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        if (!canViewAll) p.put("EntryUser", entryUser);
        if (docFrom != null) p.put("DocDateFrom", docFrom);
        if (docTo != null) p.put("DocDateTo", docTo);
        if (entryFrom != null) p.put("EntryFromDate", entryFrom);
        if (entryTo != null) p.put("EntryToDate", entryTo);
        if (modifyFrom != null) p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("ModifyToDate", modifyTo);
        if (apprFrom != null) p.put("ApprovedFromDate", apprFrom);
        if (apprTo != null) p.put("ApprovedToDate", apprTo);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (transferType != null && !transferType.isEmpty()) p.put("TransferType", transferType);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ============================================================================ read (DAL 0412 GetData)

    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> details(int headerId) {
        return DesktopProc.rows(jdbc, P_GETALL, params("HeaderId", headerId, "Activity", "ReadByHeaderId"));
    }

    public List<Map<String, Object>> expenses(int headerId) {
        return DesktopProc.rows(jdbc, P_GETALL, params("HeaderId", headerId, "Activity", "ReadExpenseDetailByHeaderId"));
    }

    /** CommonServices.StockTransferSlip406 → BLL 0559 StockTransferSlipandRegister (@Id only). */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvStockTransfer_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    // ============================================================================ loader dialog

    /** LoadavailableTransactionsForIssuance.StockComboFill:197 → BLL 0125. */
    public List<Map<String, Object>> loaderDropDowns(UserAccount u, String branchesIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    /** PendingInventoryTransactionsForIssuanceLoad:319 → BLL 0574 GetAvailableTransactionsForIssuance. */
    public List<Map<String, Object>> loaderAvailable(UserAccount u, int branchesId, Timestamp from, Timestamp to,
                                                     int supplierCustomerId, int warehouseId, int itemId,
                                                     int refDocumentTypeId, int jobLotId, int parentCategory,
                                                     int itemCategoryId, int itemTypeId, String cropYear) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchesId != 0) p.put("BranchesId", branchesId);
        if (from != null) p.put("DateFrom", from);
        if (to != null) p.put("DateTo", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (parentCategory != 0) p.put("InventoryParentCategories", parentCategory);
        if (itemCategoryId != 0) p.put("ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("ItemTypeId", itemTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        return DesktopProc.rows(jdbc, "SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance", p);
    }

    // ============================================================================ save support

    /** DAL 0205 GetERPFeaturesByCompanyId — read per call, not cached. */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    /** GenericProvider.SetProc. */
    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    /** A procedure executed with ExecuteScalar/ExecuteNonQuery whose result is not used. */
    public void exec(String proc, Map<String, Object> p) { DesktopProc.scalar(jdbc, proc, p); }

    /** DAL 0205 FIFOImplemention's read. */
    public List<Map<String, Object>> stockByFifo(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p);
    }

    /** DAL 0205 GetEqvilentByItemIdAndUomScheduleId. */
    public double equivalentByItemAndSchedule(UserAccount u, int itemId, int scheduleId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId"));
        return r.isEmpty() ? 0d : toDouble(r.get(0).get("Equivalent"));
    }

    /** DAL 0205 GetUomScheduleIdByItemIdAndEquivalent. */
    public int uomScheduleIdByEquivalent(UserAccount u, int itemId, double equivalent) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "USP_GetUomScheduleIdByItemIdAndEquivalent", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "Equivalent", equivalent));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    /** DAL 0412:… Sp_InventoryStockEvalautionDetail_Update with the model's non-null defaults. */
    public void evaluationUpdate(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", documentTypeId);
        e.put("RefDocIdNo", id);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    /** DAL 0412:… InventoryTransactions model → Sp_InventoryTransactions_GetALLMethod. */
    public void inventoryTransactions(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", documentTypeId);
        t.put("RefDocIdNo", id);
        setProc("Sp_InventoryTransactions_GetALLMethod", t);
    }

    /** Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId → ActiveYr.Start_Period (yyyy-MM-dd). */
    public String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(StoreIssuanceRepository.ci(r, "Id")) == yearId) {
                    Object v = StoreIssuanceRepository.ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(v).substring(0, 10);
                }
            }
        } catch (Exception ignored) { }
        return null;
    }

}
