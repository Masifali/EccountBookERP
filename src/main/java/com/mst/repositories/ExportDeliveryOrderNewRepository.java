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

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of 201 "Export Delivery Order (New)" - Architecture.WinApp.Export.ExportDeliveryOrderNew (ScreenName of the
 * rights "ExportDeliveryOrder" = ScreenDefinition 201, form Name "ExportDeliveryOrderNew", DocumentTypeId 84,
 * DeliveryOrderType "Export"; tables InvDeliveryOrder / InvDeliveryOrderDetail). One method per desktop BLL call, each
 * with the desktop's procedure and parameters (verified in procdure_index.csv / procdure.utf8.sql, 03-Oct-2026):
 *
 *   BLL 0558 / DAL 0411 InvDeliveryOrder
 *     GenerateCode (CommonServices.DeliveryOrderGenerateCode(84))  Sp_InvDeliveryOrder_GetAllMethod @OrganizationId @CompanyId @DocumentTypeId
 *                                                                  [@FinancialYearId] [@BranchesId] @Activity='GenerateCode'
 *     GetByID      Sp_InvDeliveryOrder_GetAllMethod @Id @Activity='ReadById'; for DocumentTypeId 84 + "Export": 'ReadByIdDetailIdExport'
 *                  and 'ReadInvDeliveryOrderExpensesByHeaderIdForExport' (DAL GetData)
 *     GetDataForDropDownFromDeliveryOrder  USP_GetDataForDropDownFromDeliveryOrder @OrganizationId @CompanyId [@FinancialYearId]
 *                  @DocumentTypeIds='84' @DeliveryOrderType='Export'
 *     ExportFormHistoryNew  [dbo].[USP_DeliveryOrderExport_FormHistory] (BranchesId is set on the object but never sent)
 *     Save -> SetData  Sp_InvDeliveryOrder_Insert | Sp_InvDeliveryOrder_Update (every non-virtual model property),
 *                  Sp_InvDeliveryOrderDetail_Insert per row (removed rows first, ActionTypeId 3), no expenses on this form,
 *                  [DAW].[USp_DocumentApprovalDetail_Insert] (LimitAmount 0) - one transaction; "Detail List not found" when empty.
 *   BLL 0467 ExImInvoice
 *     getOutstandingExportInvoicesForDeliveryOrder  usp_getOutstandingExportInvoicesForDeliveryOrder @OrganizationId @CompanyId [@DoId]
 *     GetContractsByInvoiceId              USP_GetContractsByInvoiceId @OrganizationId @CompanyId @Id
 *     GetExportInvoiceHeaderDataByInvoiceId USP_GetExportInvoiceHeaderDataByInvoiceId @OrganizationId @CompanyId @Id
 *     GetInvoicewiseDoWeightandBalanceWeight Sp_InvDeliveryOrder_GetAllMethod @OrganizationId @CompanyId @ExImInvoiceId
 *                                          @Activity='GetInvoicewiseDoWeightandBalanceWeight'
 *     ExportInvoiceDetailByHeaderId        Sp_ExImInvoice_GetAllMethod @OrganizationId @CompanyId @Id @ContractId
 *                                          @Activity='ExportInvoiceDetailByHeaderId'
 *     GetInvoiceDetailForDetailId          USP_GetInvoiceDetailForDetailId @Id
 *     GetContainerNoFromShipingLineBookingDetailByInvoiceId  [dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId]
 *                                          @OrganizationId @CompanyId @InvoiceId [@DoId]
 *   Lookups: Sp_Branches_GetAllMethod 'GetAll' (BrancheServiceBind), Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'
 *     (getActiveWareHouse), SP_JobLot_ReadMethod 'GetAll' (JobLotGetAllService), Sp_InvPackingType_GetAllMethod 'ReadAll'
 *     (InvPackingType.Getall), Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'
 *     (clsGlobalVariables.configrationsAllocation).
 */
@Repository
public class ExportDeliveryOrderNewRepository {

    public static final int DOCUMENT_TYPE_ID = 84;
    private static final String P_GETALL = "Sp_InvDeliveryOrder_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportDeliveryOrderNewRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private Map<String, Object> oc(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return o == null ? 0 : (int) Math.round(Double.parseDouble(String.valueOf(o).trim())); } catch (NumberFormatException e) { return 0; }
    }

    public String config(UserAccount u, String description) {
        Map<String, Object> p = oc(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public int generateCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (u.getBranchesId() != null && u.getBranchesId() != 0) p.put("BranchesId", u.getBranchesId());
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : num(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> branches(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "GetAll");
        return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", p);
    }

    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "GetActiveWareHouse");
        return DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", p);
    }

    public List<Map<String, Object>> jobLots(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "GetAll");
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", p);
    }

    public List<Map<String, Object>> packingTypes() {
        return DesktopProc.rows(jdbc, "Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    public List<Map<String, Object>> outstandingInvoices(UserAccount u, int doId) {
        Map<String, Object> p = oc(u);
        if (doId != 0) p.put("DoId", doId);
        return DesktopProc.rows(jdbc, "usp_getOutstandingExportInvoicesForDeliveryOrder", p);
    }

    public List<Map<String, Object>> contractsByInvoice(UserAccount u, int invoiceId) {
        Map<String, Object> p = oc(u); p.put("Id", invoiceId);
        return DesktopProc.rows(jdbc, "USP_GetContractsByInvoiceId", p);
    }

    public List<Map<String, Object>> invoiceHeader(UserAccount u, int invoiceId) {
        Map<String, Object> p = oc(u); p.put("Id", invoiceId);
        return DesktopProc.rows(jdbc, "USP_GetExportInvoiceHeaderDataByInvoiceId", p);
    }

    public List<Map<String, Object>> invoiceWeights(UserAccount u, int invoiceId) {
        Map<String, Object> p = oc(u);
        p.put("ExImInvoiceId", invoiceId);
        p.put("Activity", "GetInvoicewiseDoWeightandBalanceWeight");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    /** ExImInvoice.ExportInvoiceDetailByHeaderId: @Id = invoice, @ContractId = order. */
    public List<Map<String, Object>> invoiceDetailByHeader(UserAccount u, int invoiceId, int orderId) {
        Map<String, Object> p = oc(u);
        p.put("Id", invoiceId);
        p.put("ContractId", orderId);
        p.put("Activity", "ExportInvoiceDetailByHeaderId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    public List<Map<String, Object>> invoiceDetailForDetailId(int id) {
        return DesktopProc.rows(jdbc, "USP_GetInvoiceDetailForDetailId", params("Id", id));
    }

    public List<Map<String, Object>> containers(UserAccount u, int invoiceId, int doId) {
        Map<String, Object> p = oc(u);
        p.put("InvoiceId", invoiceId);
        if (doId != 0) p.put("DoId", doId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId]", p);
    }

    public List<Map<String, Object>> historyCombos(UserAccount u, int financialYearId) {
        Map<String, Object> p = oc(u);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", "84");
        p.put("DeliveryOrderType", "Export");
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromDeliveryOrder", p);
    }

    /** InvDeliveryOrder.ExportFormHistoryNew: the dates / doc nos / party / invoice only when set. */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll, Map<String, Timestamp> dates,
                                                 int docNoFrom, int docNoTo, int customerId, int invoiceId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (!canViewAll) p.put("EntryUser", u.getId());
        for (String k : new String[] { "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", "ApprovedFromDate", "ApprovedToDate" })
            if (dates.get(k) != null) p.put(k, dates.get(k));
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

    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByIdDetailIdExport"));
    }

    /**
     * DAL 0411 SetData: header (positive scalar = new Id, otherwise the record's Id), each detail with InvDeliveryOrderId,
     * [DAW].[USp_DocumentApprovalDetail_Insert] (OrganizationId, CompanyId, DocumentTypeId, Id, LimitAmount 0). Returns
     * the header id as the desktop's "num".
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(String procName, Map<String, Object> po, List<Map<String, Object>> details) {
        if (details.isEmpty()) throw new IllegalStateException("Detail List not found");
        int n = DesktopProc.setProc(jdbc, procName, po);
        int id;
        if (n > 0) id = n; else { id = num(po.get("Id")); n = id; }
        for (Map<String, Object> d : details) {
            d.put("InvDeliveryOrderId", id);
            DesktopProc.setProc(jdbc, "Sp_InvDeliveryOrderDetail_Insert", d);
        }
        DesktopProc.setProc(jdbc, "[DAW].[USp_DocumentApprovalDetail_Insert]", params(
                "OrganizationId", po.get("OrganizationId"), "CompanyId", po.get("CompanyId"),
                "DocumentTypeId", po.get("DocumentTypeId"), "Id", id, "LimitAmount", BigDecimal.ZERO));
        return n;
    }
}
