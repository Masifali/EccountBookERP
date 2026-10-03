package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportForwardingRepository.ci;
import static com.mst.repositories.ExportForwardingRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * 208 "Export Delivery Order" - Architecture.WinApp.Export.ExportDeliveryOrderB (ScreenName "ExportDeliveryOrderB",
 * DocumentTypeId 84, DeliveryOrderType "Export"). Every call is the desktop's own procedure with the BLL's
 * parameters and conditions:
 *
 *   BLL 0558 / DAL 0411 InvDeliveryOrder
 *     GenerateCode (CommonServices.DeliveryOrderGenerateCode(84))  Sp_InvDeliveryOrder_GetAllMethod 'GenerateCode' [@FinancialYearId] [@BranchesId]
 *     GetByID                         Sp_InvDeliveryOrder_GetAllMethod 'ReadById' -> 'ReadByIdDetailIdExport', 'ReadInvDeliveryOrderExpensesByHeaderIdForExport'
 *     GetDataForDropDownFromDeliveryOrder  USP_GetDataForDropDownFromDeliveryOrder [@FinancialYearId] @DocumentTypeIds='84' @DeliveryOrderType='Export'
 *     ExportFormHistoryNew            [dbo].[USP_DeliveryOrderExport_FormHistory]
 *     Save -> SetData                 Sp_InvDeliveryOrder_Insert | _Update, Sp_InvDeliveryOrderDetail_Insert, Sp_InvDeliveryOrderExpense_Insert,
 *                                     [DAW].[USp_DocumentApprovalDetail_Insert] (LimitAmount 0)
 *   BLL 0467 ExImInvoice              usp_getOutstandingExportInvoicesForDeliveryOrder, USP_GetInvoiceDetailForDetailId,
 *                                     USP_GetExportInvoiceHeaderDataByInvoiceId, USP_GetContractsByInvoiceId,
 *                                     Sp_InvDeliveryOrder_GetAllMethod 'GetInvoicewiseDoWeightandBalanceWeight',
 *                                     [dbo].[USP_ExImInvoice_ExportInvoiceDetailByHeaderId],
 *                                     [dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId],
 *                                     Sp_ExImInvoice_GetAllMethod 'ExImInvoiceOtherItemsByInvoiceIds',
 *                                     [dbo].[usp_getExportInvoiceDataForDeliveryOrder] (Load Invoices popup)
 *   BLL 0469 ExImLcOrder.GetDataForDropDownFromExportInvoice  [dbo].[USP_GetDataForDropDownFromExportInvoice] (popup combos)
 *   BLL 0472 InvLabPreProductionExportLotInspectionHeader  [dbo].[USP_InvLabPreProductionExportLotInspectionHeader_GetByItemId],
 *                                     [dbo].[USP_ThirdPartyInspectionSubLot_GetByLotId]
 *   BLL 0463 ExImExportShipingLineBooking.GetBookingCROIdByInvoiceId  usp_getBookingCROIdByInvoiceId
 *   Lookups: Sp_Branches_GetAllMethod 'GetAll', Sp_Item_GetAllMethod 'GetItemByItemTypeId' (@LookupTypeIds '14'),
 *   Sp_SeaPorts_GetAllMethod (PortTypeId 2), Sp_SupplierCustomer_GetAllMethod (group 10), Sp_InvWareHouse_GetAllMethod,
 *   SP_JobLot_ReadMethod, Sp_InvPackingType_GetAllMethod, Sp_UOMSchedule_GetAllMethod (shared with ExportForwardingRepository).
 */
@Repository
public class ExportDeliveryOrderRepository {

    public static final int DOCUMENT_TYPE_ID = 84;
    public static final String P_GETALL = "Sp_InvDeliveryOrder_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportDeliveryOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public int generateCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOCUMENT_TYPE_ID);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (toInt(u.getBranchesId()) != 0) p.put("BranchesId", u.getBranchesId());
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    public List<Map<String, Object>> branches(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** CommonServices.GetItemByItemTypeId("14") - Item.GetItemByItemTypeId: @LookupTypeIds always. */
    public List<Map<String, Object>> itemsByType(UserAccount u, String ids) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "LookupTypeIds", ids, "Activity", "GetItemByItemTypeId"));
    }

    /** ExImInvoice.getOutstandingExportInvoicesForDeliveryOrder - @DoId when non-zero. */
    public List<Map<String, Object>> outstandingInvoices(UserAccount u, int doId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (doId != 0) p.put("DoId", doId);
        return DesktopProc.rows(jdbc, "usp_getOutstandingExportInvoicesForDeliveryOrder", p);
    }

    public List<Map<String, Object>> invoiceDetailForDetailId(int id) {
        return DesktopProc.rows(jdbc, "USP_GetInvoiceDetailForDetailId", params("Id", id));
    }

    public List<Map<String, Object>> invoiceHeader(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, "USP_GetExportInvoiceHeaderDataByInvoiceId", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", invoiceId));
    }

    public List<Map<String, Object>> contractsByInvoice(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, "USP_GetContractsByInvoiceId", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", invoiceId));
    }

    public List<Map<String, Object>> invoiceWeights(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, P_GETALL, params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ExImInvoiceId", invoiceId, "Activity", "GetInvoicewiseDoWeightandBalanceWeight"));
    }

    public List<Map<String, Object>> invoiceDetailByHeader(UserAccount u, int contractId, int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoice_ExportInvoiceDetailByHeaderId]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ContractId", contractId, "ExImInvoiceId", invoiceId));
    }

    /** ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(org, comp, invoice, RecId) - @DoId when non-zero. */
    public List<Map<String, Object>> containers(UserAccount u, int invoiceId, int doId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "InvoiceId", invoiceId);
        if (doId != 0) p.put("DoId", doId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId]", p);
    }

    public List<Map<String, Object>> otherItemsByInvoiceIds(String ids) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Ids", ids, "Activity", "ExImInvoiceOtherItemsByInvoiceIds"));
    }

    public List<Map<String, Object>> inspections(UserAccount u, int financialYearId, int itemId, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId, "ItemId", itemId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvLabPreProductionExportLotInspectionHeader_GetByItemId]", p);
    }

    public List<Map<String, Object>> subLots(UserAccount u, int analysisId, int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ThirdPartyInspectionSubLot_GetByLotId]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", analysisId, "InvoiceId", invoiceId));
    }

    public int croIdByInvoice(UserAccount u, int invoiceId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "usp_getBookingCROIdByInvoiceId", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", invoiceId));
        if (r.isEmpty()) return 0;
        return toInt(r.get(0).values().iterator().next());
    }

    // ---------------------------------------------------------------- Load Invoices popup (LoadCommercialInvoiceForDO)

    public List<Map<String, Object>> loaderCombos(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImInvoice.LoadExportInvoiceDataForDeliveryOrder - every filter only when set (BranchesId / FinancialYearId never sent). */
    public List<Map<String, Object>> loaderData(UserAccount u, Timestamp from, Timestamp to, int invoiceId, int contractId,
                                                int customerId, int itemId, int jobLotId, int cropYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("Todate", to);
        if (invoiceId != 0) p.put("ExImInvoiceId", invoiceId);
        if (contractId != 0) p.put("ContractId", contractId);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        return DesktopProc.rows(jdbc, "[dbo].[usp_getExportInvoiceDataForDeliveryOrder]", p);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (LoadCommercialInvoiceForDO.InitializeComponentCustom FromDate). */
    public Object financialYearStart(int financialYearId) {
        List<Map<String, Object>> r = jdbc.queryForList("SELECT Start_Period FROM FinancialYear WHERE Id = ?", financialYearId);
        return r.isEmpty() ? null : ci(r.get(0), "Start_Period");
    }

    // ---------------------------------------------------------------- history / read

    public List<Map<String, Object>> historyCombos(UserAccount u, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", "84");
        p.put("DeliveryOrderType", "Export");
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromDeliveryOrder", p);
    }

    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll,
                                                 Timestamp from, Timestamp to, Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo, Timestamp approvedFrom, Timestamp approvedTo,
                                                 int docNoFrom, int docNoTo, int customerId, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (entryFrom != null) p.put("EntryFromDate", entryFrom);
        if (entryTo != null) p.put("EntryToDate", entryTo);
        if (modifyFrom != null) p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null) p.put("ApprovedToDate", approvedTo);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryOrderExport_FormHistory]", p);
    }

    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0411 GetData for DocumentTypeId 84 and DeliveryOrderType "Export". */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByIdDetailIdExport"));
    }

    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadInvDeliveryOrderExpensesByHeaderIdForExport"));
    }

    // ---------------------------------------------------------------- save (DAL 0411 SetData)

    /**
     * DAL 0411 SetData: "Detail List not found" when empty; header through ProcName (positive scalar = new Id);
     * each detail (removed rows first) through Sp_InvDeliveryOrderDetail_Insert; each expense through
     * Sp_InvDeliveryOrderExpense_Insert; [DAW].[USp_DocumentApprovalDetail_Insert] with LimitAmount 0. One transaction.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(String procName, Map<String, Object> po, List<Map<String, Object>> details, List<Map<String, Object>> expenses) {
        if (details.isEmpty()) throw new IllegalStateException("Detail List not found");
        int num = DesktopProc.setProc(jdbc, procName, po);
        int id;
        if (num > 0) id = num; else { id = toInt(po.get("Id")); num = id; }
        for (Map<String, Object> d : details) {
            d.put("InvDeliveryOrderId", id);
            DesktopProc.setProc(jdbc, "Sp_InvDeliveryOrderDetail_Insert", d);
        }
        for (Map<String, Object> e : expenses) {
            e.put("InvDeliveryOrderId", id);
            DesktopProc.setProc(jdbc, "Sp_InvDeliveryOrderExpense_Insert", e);
        }
        DesktopProc.setProc(jdbc, "[DAW].[USp_DocumentApprovalDetail_Insert]", params(
                "OrganizationId", po.get("OrganizationId"), "CompanyId", po.get("CompanyId"),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "Id", id, "LimitAmount", BigDecimal.ZERO));
        return num;
    }
}
