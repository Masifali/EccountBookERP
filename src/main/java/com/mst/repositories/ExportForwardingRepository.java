package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of the two Export forwarding screens:
 *   212 "Export Forwarding"        Architecture.WinApp.Export.EximForwarding     (DocumentTypeId 205)
 *   793 "Export Forwarding (New)"  Architecture.WinApp.Export.frmForwardingNew   (DocumentTypeId 210)
 *
 * Every call is one of the desktop's own procedures with the BLL's parameter names and the BLL's
 * "only when non-zero / not empty" conditions (read from procdure.utf8.sql / procdure_index.csv):
 *
 *   BLL 0465 / DAL 0517 ExImForwarding
 *     GenerateCode                Sp_ExImForwarding_GetAllMethod @OrganizationId @CompanyId [@FinancialYearId] [@DocumentTypeId] @Activity='GenerateDocNoCompanyIdOrganizationId'
 *     GetByID                     Sp_ExImForwarding_GetAllMethod @Id @Activity='ReadById' (+ 'ReadByForwardingHeaderId', 'ReadOtherItemsByHeaderId')
 *     FormHistory                 [dbo].[USP_ExImForwarding_FormHistory] @OrganizationId @CompanyId @BranchesId [@FinancialYearId] @CanViewAllRecord [@EntryUser] [dates] [@SupplierCustomerId] [@DocNoFrom] [@DocNoTo] [@GpId] [@InvoiceId]
 *     GetDataForDropDownFromGoodsForwarding  USP_GetDataForDropDownFromGoodsForwarding @OrganizationId @CompanyId [@BranchesId]
 *     GetLastWareHouseFromForwarding         Sp_ExImForwarding_GetAllMethod @OrganizationId @CompanyId @DocumentTypeId @Activity='LasdtWareHouseFromForwarding'
 *     GetForwardingIdsForAutoUpdation        [dbo].[USP_GetForwardingIdsForAutoUpdation] @OrganizationId @CompanyId @DocumentTypeId
 *     GetContainersForForwarding             usp_getContainersForForwarding @GpId @InvoiceId
 *     Save / DAL SetDate                     Sp_ExImForwarding_Insert | _Update, Sp_ExImForwardingPackingDetail_Insert, Sp_ExImForwardingOtherItems_Insert,
 *                                            USP_GetStockByFifoMethod / USP_InventoryQtyReverseAndDeleteByReferenceId / USP_InventoryStockEvalautionDetail_Insert (ERP feature 5),
 *                                            usp_StockEvaluationUpdateForExportForwarding (no feature 5), usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding,
 *                                            Sp_InventoryTransactions_GetALLMethod, USP_InventoryValidation, usp_getBalNoOfContainersByContractId
 *   BLL 0467 ExImInvoice           GetInvoiceNoForExportForwarding, GetData, GetOtherItemByExImInvoiceId, GetByID (ReadExImInvoiceOtherItemsByHeaderId)
 *   BLL 0568 GatePassOutward       GetPendingGatePassForExportForwarding, GatePassDetailForForwardingByGpId, GatePassOutwardDataForExportForwarding
 *   BLL 0558 InvDeliveryOrder      DeliveryOrderDetailForExportGdnForwarding
 *   BLL 0463 ExImExportShipingLineBooking.ReadByInvoiceId
 *   BLL 0581 / DAL 0434 InvPurchaseInvoice.RemoveByID   Sp_InvoicesVouchersandStocksDelete
 *   BLL 0056 GetAvgRatesAndStockInHand.GetStockByFifoMethod / GetAvailableStock (usp_getAvailableStock)
 *   Common lookups: Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerByCustomerGroupId', Sp_SeaPorts_GetAllMethod,
 *   Sp_Item_GetAllMethod 'GetExportItemsByOrganizationCompanyId', SP_JobLot_ReadMethod 'GetAll', usp_JobLot_GetWithJobOrderAndItem,
 *   Sp_InvCropYear_GetAllMethod, Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse', Sp_InvPackingType_GetAllMethod,
 *   usp_getAllUomsByCompanyId (clsGlobalVariables.globalUomSchedule), Sp_UOMSchedule_GetAllMethod 'ReadByItemID',
 *   Sp_ConfigrationsAllocation_GetAllMethod, USP_GetERPFeaturesByCompanyId, USP_GetRefDocumentsForWages.
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportForwardingRepository {

    public static final String P_GETALL = "Sp_ExImForwarding_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportForwardingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public JdbcTemplate jdbc() { return jdbc; }

    // ================================================================== shared / global lists

    /** GlobalVariables_Helper.GetConfigValueFromGlobal / CommonServices.GetConfigurationByOrgCompandConfigDescription. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** CommonServices.GetERPFeaturesByCompanyId / GetERPFeatureById. */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(ci(r, "Id")) == featureId) return true;
        }
        return false;
    }

    /** clsGlobalVariables.WagesRefDocumentsStatusList.Find(RefDocumentTypeId == id).IsActive. */
    public boolean wagesActive(int refDocumentTypeId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetRefDocumentsForWages]", params())) {
            if (toInt(ci(r, "RefDocumentTypeId")) == refDocumentTypeId) return toBool(ci(r, "IsActive"));
        }
        return false;
    }

    /** CommonServices.GetSupplierustomerByCustomerGroupId("10") - transporters / shipping lines / agents. */
    public List<Map<String, Object>> suppliersByGroup(UserAccount u, String groupIds) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "CustomerGroupIds", groupIds,
                "Activity", "GetSupplierustomerByCustomerGroupId"));
    }

    /** SeaPorts.Getall / CommonServices.GetSeaPortForComboServiceBind. */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    /** Item.GetExportItemsByOrganizationCompanyId. */
    public List<Map<String, Object>> exportItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetExportItemsByOrganizationCompanyId"));
    }

    /** CommonServices.JobLotGetAllService - jobLot.GetAll: SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** jobLot.JobLot_GetWithJobOrderAndItem(org, comp, 0) - @ItemId only when non-zero. */
    public List<Map<String, Object>> jobLotsWithJobOrderAndItem(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[usp_JobLot_GetWithJobOrderAndItem]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.CropYearGetAllService - InvCropYear.Getall 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** InvCropYear ReadById (DAL 0517 reads the crop year text of a row's CropYearId for the FIFO rows). */
    public String cropYearById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : str(ci(r.get(0), "CropYear"));
    }

    /** CommonServices.getActiveWareHouse - InvWareHouse.GetActiveWareHouse. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetActiveWareHouse"));
    }

    /** InvPackingType.Getall - @Activity 'ReadAll' only. */
    public List<Map<String, Object>> packingTypes() {
        return DesktopProc.rows(jdbc, "Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    /**
     * CommonServices.dtUomFromGloablUomScheduleByItemId(itemId) - clsGlobalVariables.globalUomSchedule
     * (usp_getAllUomsByCompanyId @Active 1, loaded by "UomSchedule" in btnRefresh) filtered by ItemId.
     */
    public List<Map<String, Object>> globalUomByItem(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            if (toInt(ci(r, "ItemId")) != itemId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("UOMCode", str(ci(r, "UOMCode")));
            m.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId - UOMSchedule.SearchByObject: Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uomScheduleByItem(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "Activity", "ReadByItemID"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("UOMCode", str(ci(r, "UOMCode")));
            m.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    // ================================================================== gate pass / invoice / DO

    /** GatePassOutward.GetPendingGatePassForExportForwarding (Id, GpSrNo). */
    public List<Map<String, Object>> pendingGatePasses(UserAccount u, int financialYearId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_GetAllMethod", params(
                "Activity", "GetPendingGatePassForExportForwarding",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId,
                "BranchesId", u.getBranchesId()));
    }

    /** GatePassOutward.GatePassDetailForForwardingByGpId. */
    public List<Map<String, Object>> gatePassDetail(UserAccount u, int gpId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_GetAllMethod", params(
                "Activity", "GpNoPandingforInvGdnForExport",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", gpId));
    }

    /** GatePassOutward.GatePassOutwardDataForExportForwarding - the "Gate Pass information" grid. */
    public List<Map<String, Object>> gatePassGrid(UserAccount u, int financialYearId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GatePassOutwardDataForExportForwarding]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId,
                "BranchesId", u.getBranchesId()));
    }

    /** ExImInvoice.GetInvoiceNoForExportForwarding - @GpId always, @FinancialYearId when non-zero. */
    public List<Map<String, Object>> invoicesForGatePass(UserAccount u, int gpId, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "GpId", gpId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetPendingInvoicesForExportForwarding");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImInvoice.GetData(ReportsParameters) - @DocumentTypeIds / @FinancialYearId / @Id when set. */
    public List<Map<String, Object>> invoiceData(UserAccount u, String documentTypeIds, int financialYearId, int id) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (id != 0) p.put("Id", id);
        p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImInvoice.GetOtherItemByExImInvoiceId. */
    public List<Map<String, Object>> invoiceOtherItemCombo(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", invoiceId,
                "Activity", "GetOtherItemByExImInvoiceId"));
    }

    /** ExImInvoice.GetByID(id).ExImInvoiceOtherItemslist - DAL 0519 'ReadExImInvoiceOtherItemsByHeaderId'. */
    public List<Map<String, Object>> invoiceOtherItems(int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", invoiceId, "Activity", "ReadExImInvoiceOtherItemsByHeaderId"));
    }

    /** InvDeliveryOrder.DeliveryOrderDetailForExportGdnForwarding - @Id = invoice, @GpId = gate pass. */
    public List<Map<String, Object>> deliveryOrderDetail(UserAccount u, int invoiceId, int gpId) {
        return DesktopProc.rows(jdbc, "Sp_InvDeliveryOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", invoiceId, "GpId", gpId, "Activity", "DeliveryOrderDetailForExportGdnForwarding"));
    }

    /** ExImForwarding.GetContainersForForwarding. */
    public List<Map<String, Object>> containers(int gpId, int invoiceId) {
        return DesktopProc.rows(jdbc, "usp_getContainersForForwarding", params("GpId", gpId, "InvoiceId", invoiceId));
    }

    /** ExImExportShipingLineBooking.ReadByInvoiceId. */
    public List<Map<String, Object>> shippingBookingByInvoice(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImExportShipingLineBooking_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ExImInvoiceId", invoiceId, "Activity", "ReadByInvoiceId"));
    }

    // ================================================================== forwarding header

    /** ExImForwarding.GenerateCode. */
    public int generateCode(UserAccount u, int financialYearId, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateDocNoCompanyIdOrganizationId");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return toInt(ci(r.get(0), "DocNo"));
    }

    /** ExImForwarding.GetByID - header (or null), then DAL 0517 GetDate's two detail reads. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByForwardingHeaderId"));
    }

    public List<Map<String, Object>> otherItems(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadOtherItemsByHeaderId"));
    }

    /** ExImForwarding.GetLastWareHouseFromForwarding - the form reads column [2]. */
    public List<Map<String, Object>> lastWarehouse(UserAccount u, int documentTypeId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Activity", "LasdtWareHouseFromForwarding"));
    }

    /** ExImForwarding.GetDataForDropDownFromGoodsForwarding - @BranchesId when non-zero, no @Activity. */
    public List<Map<String, Object>> historyCombos(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (toInt(u.getBranchesId()) != 0) p.put("BranchesId", u.getBranchesId());
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromGoodsForwarding", p);
    }

    /**
     * ExImForwarding.FormHistory. The BLL sends @EntryUser when CanViewAllRecord is false; the procedure
     * declares @EntryUserId instead, so on the desktop that call fails with the procedure's error - the same
     * error comes back here (desktop quirk, kept).
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll,
                                                 Timestamp from, Timestamp to, Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo, Timestamp approvedFrom, Timestamp approvedTo,
                                                 int supplierCustomerId, int docNoFrom, int docNoTo, int gpId, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (entryFrom != null) p.put("EntryFromDate", entryFrom);
        if (entryTo != null) p.put("EntryToDate", entryTo);
        if (modifyFrom != null) p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null) p.put("ApprovedToDate", approvedTo);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        if (gpId != 0) p.put("GpId", gpId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImForwarding_FormHistory]", p);
    }

    /** ExImForwarding.GetForwardingIdsForAutoUpdation. */
    public List<Map<String, Object>> autoUpdateIds(UserAccount u, int documentTypeId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetForwardingIdsForAutoUpdation]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId));
    }

    /** InvPurchaseInvoice.RemoveByID -> DAL 0434 AccountandInventoryRemoveById -> Sp_InvoicesVouchersandStocksDelete. */
    public void removeById(UserAccount u, int documentTypeId, int id) {
        DesktopProc.rows(jdbc, "Sp_InvoicesVouchersandStocksDelete", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "DocumentTypeId", documentTypeId, "UserId", u.getId()));
    }

    // ================================================================== stock (Generate Stock)

    /** GetAvgRatesAndStockInHand.GetStockByFifoMethod (the "Generate Stock" FIFO branch; no document id). */
    public List<Map<String, Object>> stockByFifo(UserAccount u, int itemId, Timestamp docDate, int warehouseId, String cropYear,
                                                 int jobLotId, int packingTypeId, int stockUom) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId);
        if (stockUom != 0) p.put("PackUomId", stockUom);
        p.put("DocDate", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("PackingTypeId", packingTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        return DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p);
    }

    /**
     * GetAvgRatesAndStockInHand.GetAvailableStock. The BLL sends @PackUomId from ReportsParameters.ItemUomId,
     * which the form never sets (it sets stockUOM) - so @PackUomId never reaches usp_getAvailableStock (quirk kept).
     */
    public List<Map<String, Object>> availableStock(UserAccount u, int itemId, Timestamp docDate, int warehouseId, int jobLotId,
                                                    String cropYear, int packingTypeId, int actionId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "DateTo", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        if (packingTypeId != 0) p.put("PackingTypeId", packingTypeId);
        if (actionId != 0) p.put("ActionId", actionId);
        return DesktopProc.rows(jdbc, "usp_getAvailableStock", p);
    }

    // ================================================================== save (DAL 0517 SetDate) helpers

    /** GenericProvider.SetProc - Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    /** ExecuteNonQuery of a procedure with exactly the given parameters. */
    public void exec(String proc, Map<String, Object> p) { DesktopProc.rows(jdbc, proc, p); }

    /** CommonServices.GetItemGlIdsandItemName - Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'. */
    public Map<Integer, String> itemGlIdsAndNames(UserAccount u) {
        Map<Integer, String> out = new LinkedHashMap<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetItemGlIdsandItemName"))) {
            out.putIfAbsent(toInt(ci(r, "Id")), str(ci(r, "ItemName")));
        }
        return out;
    }

    /** CommonServices.GetEqvilentByItemIdAndUomScheduleId. */
    public double equivalent(UserAccount u, int itemId, int scheduleId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId"));
        return r.isEmpty() ? 0d : toDouble(ci(r.get(0), "Equivalent"));
    }

    /**
     * CommonServices.FIFOImplemention - USP_GetStockByFifoMethod with the rows already planned in this save as
     * @FIFOXML (FIFOStockEvaluation: RefDocumentTypeId, RefDocIdNo, RefDocSubIdNo, ReserveQty, ReserveWeight),
     * @DocumentTypeId / @Id only when set (the DAL sets them on an update: ModifyUser &gt; 0).
     */
    public List<Map<String, Object>> fifoStock(UserAccount u, int itemId, Timestamp docDate, int stockUom, int warehouseId,
                                               int cropYearId, int jobLotId, int packingTypeId, String cropYear,
                                               int documentTypeId, int id, List<Map<String, Object>> reserved) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "DocDate", docDate);
        if (stockUom != 0) p.put("PackUomId", stockUom);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("PackingTypeId", packingTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("Id", id);
        if (reserved != null && !reserved.isEmpty()) {
            StringBuilder xml = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-16\"?><ArrayOfFIFOStockEvaluation>");
            for (Map<String, Object> a : reserved) {
                xml.append("<FIFOStockEvaluation>")
                   .append("<RefDocumentTypeId>").append(toInt(a.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
                   .append("<RefDocIdNo>").append(toInt(a.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
                   .append("<RefDocSubIdNo>").append(toInt(a.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
                   .append("<ReserveQty>").append(clr(toDouble(a.get("QtyOut")))).append("</ReserveQty>")
                   .append("<ReserveWeight>").append(clr(toDouble(a.get("StockWeightOut")))).append("</ReserveWeight>")
                   .append("</FIFOStockEvaluation>");
            }
            p.put("FIFOXML", xml.append("</ArrayOfFIFOStockEvaluation>").toString());
        }
        return DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p);
    }


    // ================================================================== DAL 0517 ExImForwarding.SetDate

    /** Strips the page-only keys ("_CropYear", "_ItemName") a detail map carries for the FIFO step. */
    private static Map<String, Object> model(Map<String, Object> m) {
        Map<String, Object> out = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : m.entrySet()) if (!e.getKey().startsWith("_")) out.put(e.getKey(), e.getValue());
        return out;
    }

    /**
     * DAL 0517 ExImForwarding.SetDate(obj, ProcName), one transaction, rolled back on any error:
     *  1. header through ProcName (Sp_ExImForwarding_Insert | _Update): a positive scalar is the new Id, else Id stays.
     *  2. each detail (removed rows first, ActionTypeId 3) - LineId 1..n, ExImForwardingId - through
     *     Sp_ExImForwardingPackingDetail_Insert; the detail's Id becomes Convert.ToInt32(scalar) (0 for an update row).
     *  3. each other item through Sp_ExImForwardingOtherItems_Insert.
     *  4. ERP feature 5 (FIFO) and DocumentTypeId 210: per detail (not ActionTypeId 3, item in GetItemGlIdsandItemName)
     *     FIFOImplemention on NetWeight; every allocation gets an "In" twin in WarehouseToId; the earlier rows of this
     *     save are the @FIFOXML reserve; reverse (update only) and insert all with Ref* = this document.
     *     ERP feature 5 and DocumentTypeId 205 / 206: FIFO on StockWeight; CropBatch = the row's crop year text,
     *     InvoiceId / InvoiceDetailId; reverse (update only) and insert with Other* = this document.
     *     Otherwise usp_StockEvaluationUpdateForExportForwarding @ActionId 1.
     *     Feature 5 with 205/206/210 and no allocation -> "InventoryStockEvalautionDetailslist not Fill".
     *  5. usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding, Sp_InventoryTransactions_GetALLMethod.
     *  6. USP_InventoryValidation per detail (not ActionTypeId 3) - NetWeight for 210, StockWeight otherwise.
     *  7. DocumentTypeId != 210: usp_getBalNoOfContainersByContractId once per ExImLcOrderId (first row of each).
     * Attachments (DMS) are not part of the web port (see the controller notes).
     */
    @org.springframework.transaction.annotation.Transactional(rollbackFor = Exception.class)
    public int saveForwarding(UserAccount u, String procName, Map<String, Object> obj,
                              List<Map<String, Object>> details, List<Map<String, Object>> others) {
        int docType = toInt(obj.get("DocumentTypeId"));
        int num = setProc(procName, obj);
        int id;
        if (num > 0) { id = num; obj.put("Id", id); } else { id = toInt(obj.get("Id")); num = id; }

        int line = 0;
        for (Map<String, Object> d : details) {
            d.put("LineId", ++line);
            d.put("ExImForwardingId", id);
            int detailId = setProc("Sp_ExImForwardingPackingDetail_Insert", model(d));
            d.put("Id", detailId);
        }
        for (Map<String, Object> o : others) {
            o.put("ExImForwardingId", id);
            setProc("Sp_ExImForwardingOtherItems_Insert", o);
        }

        boolean fifo = erpFeature(u, 5);
        boolean autoUpdate = toInt(obj.get("AutoUpdateId")) != 0;
        boolean update = toInt(obj.get("ModifyUser")) > 0;
        Timestamp docDate = (Timestamp) obj.get("DocDate");
        Timestamp stockDate = autoUpdate ? new Timestamp(System.currentTimeMillis()) : docDate;
        List<Map<String, Object>> stock = new ArrayList<>();
        if (fifo && (docType == 210 || docType == 205 || docType == 206)) {
            Map<Integer, String> items = itemGlIdsAndNames(u);
            for (Map<String, Object> d : details) {
                if (toInt(d.get("ActionTypeId")) == 3) continue;
                int itemId = toInt(d.get("ItemId"));
                if (!items.containsKey(itemId)) continue;
                String itemName = items.get(itemId);
                double weight = docType == 210 ? toDouble(d.get("NetWeight")) : toDouble(d.get("StockWeight"));
                List<Map<String, Object>> rows = fifoRows(u, d, itemName, stockDate, update ? docType : 0, update ? id : 0, weight, stock);
                for (Map<String, Object> r : rows) {
                    if (docType == 210) {
                        Map<String, Object> in = stockModel();
                        in.put("LineId", r.get("LineId"));
                        in.put("ItemId", r.get("ItemId"));
                        in.put("WarehouseId", toInt(d.get("WarehouseToId")));
                        in.put("RateUom", r.get("RateUom"));
                        in.put("JobLotId", r.get("JobLotId"));
                        in.put("InvPackingTypeId", r.get("InvPackingTypeId"));
                        in.put("ItemUom", r.get("ItemUom"));
                        in.put("CropBatch", r.get("CropBatch"));
                        in.put("RefRefDocumentTypeId", 0);
                        in.put("RefRefDocIdNo", 0);
                        in.put("RefRefDocSubIdNo", 0);
                        in.put("QtyIn", r.get("QtyOut"));
                        in.put("BillWeightIn", r.get("BillWeightOut"));
                        in.put("StockWeightIn", r.get("StockWeightOut"));
                        in.put("ItemRate", r.get("CgsRate"));
                        in.put("AmountIn", r.get("CgsAmount"));
                        stock.add(in);
                        stock.add(r);
                    } else {
                        int cropYearId = toInt(d.get("CropYearId"));
                        if (cropYearId > 0) {
                            String crop = cropYearById(cropYearId);
                            if (crop != null) r.put("CropBatch", crop);
                        }
                        r.put("InvoiceId", toInt(obj.get("ExImInvoiceId")));
                        r.put("InvoiceDetailId", toInt(d.get("InvoiceDetailId")));
                        stock.add(r);
                    }
                }
            }
            if (!stock.isEmpty()) {
                if (update) exec("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", params(
                        "OrganizationId", obj.get("OrganizationId"), "CompanyId", obj.get("CompanyId"),
                        "RefDocumentTypeId", docType, "RefDocIdNo", id));
                for (Map<String, Object> s : stock) {
                    s.put("OrganizationId", obj.get("OrganizationId"));
                    s.put("CompanyId", obj.get("CompanyId"));
                    s.put("DocDate", docDate);
                    s.put("DocCodeNo", toInt(obj.get("DocNo")));
                    s.put("SupplierCustomerId", toInt(obj.get("SupplierCustomerId")));
                    s.put("BranchesId", toInt(obj.get("BranchesId")));
                    s.put("EntryUser", toInt(obj.get("EntryUser")));
                    s.put("ModifyUser", toInt(obj.get("ModifyUser")));
                    s.put("CalcType", "Weight");
                    Map<String, Object> det = null;
                    int ln = toInt(s.get("LineId"));
                    for (Map<String, Object> d : details) if (ln > 0 && toInt(d.get("LineId")) == ln) { det = d; break; }
                    if (docType == 210) {
                        s.put("RefDocumentTypeId", docType);
                        s.put("ItemRate", s.get("CgsRate"));       // the "In" twins carry CgsRate 0: ItemRate becomes 0 (desktop quirk)
                        s.put("AmountOut", s.get("CgsAmount"));
                        if (det != null) { s.put("RefDocIdNo", det.get("ExImForwardingId")); s.put("RefDocSubIdNo", det.get("Id")); }
                    } else {
                        s.put("OtherDocumentTypeId", docType);
                        if (det != null) { s.put("OtherDocNoId", det.get("ExImForwardingId")); s.put("OtherSubDocNoId", det.get("Id")); }
                    }
                    setProc("USP_InventoryStockEvalautionDetail_Insert", s);
                }
            }
        } else {
            exec("usp_StockEvaluationUpdateForExportForwarding", params(
                    "OrganizationId", obj.get("OrganizationId"), "CompanyId", obj.get("CompanyId"),
                    "RefDocumentTypeId", docType, "RefDocIdNo", id, "ActionId", 1));
        }
        if (fifo && (docType == 210 || docType == 205 || docType == 206) && stock.isEmpty())
            throw new IllegalStateException("InventoryStockEvalautionDetailslist not Fill");

        exec("usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding", params(
                "OrganizationId", obj.get("OrganizationId"), "CompanyId", obj.get("CompanyId"),
                "DocumentTypeId", docType, "Id", id));

        Map<String, Object> it = new LinkedHashMap<>();
        it.put("IsApproved", false); it.put("DocDate", null); it.put("AmountIn", 0d); it.put("AmountOut", 0d);
        it.put("BillWeightIn", 0d); it.put("BillWeightOut", 0d); it.put("ExpenseAmountIn", 0d); it.put("ItemRate", 0d);
        it.put("QtyIn", 0d); it.put("QtyOut", 0d); it.put("StockWeightIn", 0d); it.put("StockWeightOut", 0d);
        it.put("BranchesId", 0); it.put("CompanyId", obj.get("CompanyId")); it.put("DocCodeNo", 0); it.put("InvPackingTypeId", 0);
        it.put("ItemId", 0); it.put("ItemUom", 0); it.put("JobLotId", 0); it.put("OrganizationId", obj.get("OrganizationId"));
        it.put("ProjectsId", 0); it.put("RateUom", 0); it.put("RefDocIdNo", num); it.put("RefDocSubIdNo", 0);
        it.put("RefDocumentTypeId", docType); it.put("SupplierCustomerId", 0); it.put("WarehouseId", 0); it.put("Id", 0L);
        it.put("CalcType", null); it.put("CropBatch", null); it.put("TranRemarks", null);
        DesktopProc.scalar(jdbc, "Sp_InventoryTransactions_GetALLMethod", it);

        for (Map<String, Object> d : details) {
            if (toInt(d.get("ActionTypeId")) == 3) continue;
            exec("USP_InventoryValidation", params(
                    "OrganizationId", obj.get("OrganizationId"),
                    "CompanyId", obj.get("CompanyId"),
                    "DocumentTypeId", docType,
                    "DocDate", stockDate,
                    "ItemId", toInt(d.get("ItemId")),
                    "WarehouseId", toInt(d.get("WarehouseId")),
                    "JobLotId", toInt(d.get("LotJobId")),
                    "CropYearId", toInt(d.get("CropYearId")),
                    "InvPackingTypeId", toInt(d.get("PackingMaterialId")),
                    "PackUomId", toInt(d.get("UOMScheduleIdOuter")),
                    "NetWeight", docType == 210 ? toDouble(d.get("NetWeight")) : toDouble(d.get("StockWeight"))));
        }
        if (docType != 210) {
            java.util.Set<Integer> seen = new java.util.LinkedHashSet<>();
            for (Map<String, Object> d : details) {
                int contract = toInt(d.get("ExImLcOrderId"));
                if (!seen.add(contract)) continue;
                exec("usp_getBalNoOfContainersByContractId", params("ContractId", contract));
            }
        }
        return num;
    }

    /** Model 1023 InventoryStockEvalautionDetail - every non-virtual property at its default. */
    private static Map<String, Object> stockModel() {
        Map<String, Object> s = new LinkedHashMap<>();
        s.put("IsApproved", false); s.put("DocDate", null);
        for (String k : new String[]{"AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate", "ExpenseAmountIn", "ItemRate",
                "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut", "CgsAmount"}) s.put(k, 0d);
        for (String k : new String[]{"BranchesId", "CompanyId", "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId", "ItemId", "ItemUom",
                "JobLotId", "OrderNo", "OrganizationId", "PrdJobOrderNo", "ProjectsId", "RateUom", "RefDocIdNo", "RefDocSubIdNo",
                "RefDocumentTypeId", "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId", "SupplierCustomerId", "WarehouseId",
                "RefWarehouseId", "CityId", "LineId", "RefRefDocumentTypeId", "RefRefDocIdNo", "RefRefDocSubIdNo", "EntryUser",
                "ModifyUser", "InvoiceId", "InvoiceDetailId", "VarientId", "ItemConditionId"}) s.put(k, 0);
        for (String k : new String[]{"CalcType", "CropBatch", "TranRemarks", "VehicleNo", "BiltyNo"}) s.put(k, null);
        return s;
    }

    /** CommonServices.FIFOImplemention for one detail row - the allocation rows, with the desktop's messages. */
    private List<Map<String, Object>> fifoRows(UserAccount u, Map<String, Object> d, String itemName, Timestamp docDate,
                                               int documentTypeId, int id, double netWeight, List<Map<String, Object>> reserve) {
        int itemId = toInt(d.get("ItemId"));
        int stockUom = toInt(d.get("UOMScheduleIdOuter"));
        int warehouseId = toInt(d.get("WarehouseId"));
        int jobLotId = toInt(d.get("LotJobId"));
        int packingTypeId = toInt(d.get("PackingMaterialId"));
        int cropYearId = toInt(d.get("CropYearId"));
        String cropYear = d.get("_CropYear") == null ? null : String.valueOf(d.get("_CropYear"));
        double itemQty = toDouble(d.get("OuterQty"));
        List<Map<String, Object>> stocks = fifoStock(u, itemId, docDate, stockUom, warehouseId, cropYearId, jobLotId, packingTypeId,
                cropYear, documentTypeId, id, reserve);
        if (stocks.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0;
        for (Map<String, Object> s : stocks) available += toDouble(ci(s, "NetBalWeight"));
        double rounded = Math.rint(available * 100d) / 100d;                   // Math.Round(value, 2) - banker's rounding
        if (!(netWeight <= rounded))
            throw new IllegalStateException("Weight available is " + available + " and row Weight is " + netWeight + " this item " + itemName + " against FIFO....");
        List<Map<String, Object>> out = new ArrayList<>();
        double usedQty = 0, usedWeight = 0;
        for (Map<String, Object> s : stocks) {
            double avgRate = toDouble(ci(s, "AvgRate"));
            int rateUom = toInt(ci(s, "RateUomId"));
            if (avgRate <= 0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
            if (rateUom == 0) throw new IllegalStateException("RateUomId not found  this " + itemName + " against FIFO Method");
            double bw = toDouble(ci(s, "NetBalWeight"));
            double bq = toDouble(ci(s, "NetBalQty"));
            double eq = equivalent(u, itemId, rateUom);
            if (eq == 0) throw new IllegalStateException("RateUom Not Found");
            Map<String, Object> r = stockModel();
            r.put("Id", toInt(ci(s, "Id")));
            r.put("LineId", toInt(d.get("LineId")));
            r.put("ItemId", itemId);
            r.put("WarehouseId", warehouseId);
            r.put("RateUom", rateUom);
            r.put("JobLotId", jobLotId);
            r.put("InvPackingTypeId", packingTypeId);
            r.put("ItemUom", stockUom);
            r.put("CropBatch", cropYear == null ? "" : cropYear);
            r.put("RefRefDocumentTypeId", toInt(ci(s, "RefDocumentTypeId")));
            r.put("RefRefDocIdNo", toInt(ci(s, "RefDocIdNo")));
            r.put("RefRefDocSubIdNo", toInt(ci(s, "RefDocSubIdNo")));
            double q, w;
            if (bw <= netWeight - usedWeight) { q = bq; w = bw; }
            else if (bw >= netWeight - usedWeight) { q = itemQty - usedQty; w = netWeight - usedWeight; }
            else continue;
            usedQty += q; usedWeight += w;
            double cgsRate = avgRate * eq;
            r.put("QtyOut", q); r.put("BillWeightOut", w); r.put("StockWeightOut", w);
            r.put("CgsRate", cgsRate); r.put("CgsAmount", w / eq * cgsRate);
            out.add(r);
            if (netWeight == usedWeight) break;
        }
        return out;
    }

    // ================================================================== static helpers (shared with the 208 classes)

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** Conversion.ToInt - text with separators parses, anything else 0. */
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return (Boolean) v ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(s); } catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble. */
    public static double toDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    public static boolean toBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** A double as invariant text without exponent (XmlSerializer writes 'R' round-trip text). */
    public static String clr(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return java.math.BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }
}
