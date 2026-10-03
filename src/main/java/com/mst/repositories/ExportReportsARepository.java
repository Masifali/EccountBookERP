package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Module 17 "Export Reports" - group A (report screens 235, 236, 238, 239, 249, 250, 251).
 *
 * Every call below is one of the desktop's own procedures with the desktop's own parameters, traced
 * form -> CommonServices/BLL -> GenericProvider (read from procdure.utf8.sql on 30-Sep-2026):
 *
 *  235 ExportReceivableByDueDateRegisterA
 *   Sp_ExImInvoice_GetAllMethod            @OrganizationId @CompanyId @Activity='InvoiceNo'                      CommonServices.GetExportInvoiceNo -> ExImInvoice.GetInvoiceNo (BLL 0467)
 *   Sp_ExImLcOrder_GetAllMethod            @OrganizationId @CompanyId @Activity='GetLcOrderNo'                   CommonServices.GetLcOrderNo -> ExImLcOrder.GetLcOrderNo (BLL 0469)
 *   USP_GetExportCustomerFromVouchers      @OrganizationId @CompanyId                                            SupplierCustomer.GetExportCustomerFromVouchers (BLL 0600)
 *   Sp_SeaPorts_GetAllMethod               @OrganizationId @CompanyId @Activity='ReadByCompanyNOrganizationId'  CommonServices.GetSeaPortForComboServiceBind -> SeaPorts.Getall (BLL 0597)
 *   Sp_ExImLcPaymentTerm_GetAllMethod      @Activity='ReadAll' @OrganizationId @CompanyId                        ExImLcPaymentTerm.Getall (BLL 0470)
 *   Sp_ExImDeliveryTerm_GetAllMethod       @Activity='ReadAll'                                                   ExImDeliveryTerm.Getall (BLL 0461)
 *   USP_ExportReceivableByDueDateRegisterA @OrganizationId @CompanyId [@CustomerId] [@ContractId] [@InvoiceId] [@DueFrom] [@DueTo]
 *                                          [@PaymentTermId] [@DeliveryTermId] [@DestinationPortId] [@SkipZero] [@SortNo]   ExportReports.GetExportReceivableRegister (BLL 0138) - 6 result sets
 *  236 frmExportContractSchedulePeriodicB
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod @OrganizationId @CompanyId @Activity='GetLatestAttentiveLoadDate'  ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate (BLL 0457)
 *   Usp_ExportContractSchedulePeriodicB    @OrganizationId @CompanyId @BranchId [@FromDate] [@ToDate] [@PerFclTon] [@DaysInterval] [@SortNo] [@ReferredStatusId]
 *                                                                                                                 ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicB - 4 result sets
 *  238 ExImShipmentCosting
 *   USP_GetExportInvoicesFromVouchers      @OrganizationId @CompanyId                                            ExImInvoice.GetExportInvoicesFromVouchers (BLL 0467)
 *   SpExport_ShipmentCosting_Report        @OrganizationId @CompanyId [@ExImInvoiceId]                           ExportReports.EximShipmentCostingReport (BLL 0138)
 *   Sp_ConfigrationsAllocation_GetAllMethod ... @Activity='GetConfigurationByOrgCompandConfigDescription'        CommonServices.GetDecimalConfiguration (the desktop's number formats)
 *  239 frmExportPendingWorksRegister / 251 ExImSaleContractRegister
 *   USP_GetDataForDropDownFromExportContract @OrganizationId @CompanyId                                          ExImLcOrder.GetDataForDropDownFromExportContract (BLL 0469)
 *   USP_PendingWorkExportRegister          @OrganizationId @CompanyId [@ContractId] [@FromDate] [@ToDate] [@SupplierCustomerId] [@InvoiceStatus]   ExImLcOrder.PendingWorkExportRegister
 *   usp_ExportInvoice_StatusUpdate         @ExImInvoiceId @UserId @InvoiceStatus                                 ExImInvoice.ExportInvoice_StatusUpdate (BLL 0467)
 *   Sp_ExImLcOrder_ExportRegister_Rpt      @OrganizationId @CompanyId [@LcOrderId] [@FromDate] [@ToDate] [@Status] [@SupplierCustomerId] [@ItemId] [@TradeTypeId] [@FarmingTypeId] [@SkipZero] [@SalePersonId]
 *   Sp_ExImLcOrder_ExportRegisterItemWise_Rpt  same + @IsApproved (always, see below)
 *   Usp_ExportContractSummaryTotalForDashboardsandCardandReports @OrganizationId @CompanyId [@FromDate] [@ToDate] [@FromDateSummary] [@ToDateSummary] [@DateType]
 *   usp_ExportContractStatusUpdate         @OrganizationId @CompanyId @DocumentTypeId @Id @ModifyUserId @ReqType @Remarks
 *   usp_ExportContractDetailById           @OrganizationId @CompanyId @LcContractId                              ExImLcOrder.ExportContractDetailById (frmContractDetailByContractId popup) - 4 result sets
 *  249 ExportDetailHistoryReport
 *   SpStaticColumnNames                    @Activity='ExportDetailHistory'                                       GeneralReprots.StaticColumnNames (BLL 0136)
 *   Sp_SupplierCustomer_GetAllMethod       @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyIdForExport'
 *   Sp_Item_GetAllMethod                   @OrganizationId @CompanyId @Activity='GetExportItemsByOrganizationCompanyId'
 *   Sp_ItemCategory_GetAllMethod           @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'
 *   Sp_ItemType_GetAllMethod               @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'
 *   SP_JobLot_ReadMethod                   @OrganizationId @CompanyId @Activity='GetAll'
 *   SpExImInvoice_ExportHistoryDetail_Report @OrganizationId @CompanyId [@ItemTypeId] [@ItemCategoryId] [@ItemId] [@FromDate] [@ToDate] [@LcContractId] [@SupplierCustomerId] [@DestinationPortId]
 *  250 DeliveryOrderHistory (Inventory_Reports)
 *   USP_GetDataForDropDownFromDeliveryOrder @OrganizationId @CompanyId                                           InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (BLL 0558)
 *   Sp_InvDeliveryOrderForApproval_Rpt     @OrganizationId @CompanyId [@DocumentTypeId] [@SupplierCustomerId] [@ItemId] [@ExImInvoiceId] [@WbWeightStatus] [@DoReferedStatus]
 *                                          [@FromDate] [@ToDate] [@IsApproved] [@DeliveryOrderType]              DeliverySchedule.GetDeliveryOrderForApproval (BLL 0566) - 2 result sets
 *  shared
 *   Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId @CompanyId                            clsGlobalVariables.ActiveYr.Start_Period
 *
 * A bracketed parameter is one the BLL wraps in a guard (!= 0, not null) and is left out when unset. A null
 * value is never bound (DesktopProc omits it). Nothing here creates or changes a table, column or procedure.
 */
@Repository
public class ExportReportsARepository {

    private final JdbcTemplate jdbc;

    public ExportReportsARepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================ shared

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(name) - the allocation row's ConfigKey, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the active year row with the session's FinancialYearId (else the first). */
    public Object financialYearStart(UserAccount u, int financialYearId) {
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) { row = r; break; }
        }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row == null ? null : ci(row, "Start_Period");
    }

    /** CommonServices.GetLcOrderNo() -> ExImLcOrder.GetLcOrderNo: Sp_ExImLcOrder_GetAllMethod 'GetLcOrderNo' (Id, LcOrderNo, SupCustId, PartyName). */
    public List<Map<String, Object>> lcOrderNos(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetLcOrderNo"));
    }

    /** CommonServices.GetSeaPortForComboServiceBind() -> SeaPorts.Getall (Id, PortName). */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    /** ExImLcOrder.ExportContractDetailById(ContractId) - usp_ExportContractDetailById, 4 tables (header, items, invoices, schedules). */
    public List<List<Map<String, Object>>> exportContractDetailById(UserAccount u, int contractId) {
        return sets("usp_ExportContractDetailById",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "LcContractId", contractId));
    }

    // ================================================================================ 235

    /** CommonServices.GetExportInvoiceNo() -> ExImInvoice.GetInvoiceNo: Sp_ExImInvoice_GetAllMethod 'InvoiceNo' (Id, InvoiceNo). */
    public List<Map<String, Object>> exportInvoiceNos(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "InvoiceNo"));
    }

    /** SupplierCustomer.GetExportCustomerFromVouchers (Id, CustomerName). */
    public List<Map<String, Object>> exportCustomersFromVouchers(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetExportCustomerFromVouchers]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImLcPaymentTerm.Getall (Id, LcOrderTerm). */
    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod",
                params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImDeliveryTerm.Getall (Id, Code) - the BLL sends only @Activity. */
    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll"));
    }

    /**
     * ExportReports.GetExportReceivableRegister - USP_ExportReceivableByDueDateRegisterA, six tables:
     * [0] register, [1] bank, [2] customer, [3] sale man, [4] fcy, [5] ETA / aging buckets.
     */
    public List<List<Map<String, Object>>> receivableRegister(UserAccount u, int customerId, int contractId, int invoiceId,
                                                              java.sql.Timestamp dueFrom, java.sql.Timestamp dueTo, int paymentTermId,
                                                              int deliveryTermId, int destinationPortId, int skipZero, int sortNo) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (customerId != 0) p.put("CustomerId", customerId);
        if (contractId != 0) p.put("ContractId", contractId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (dueFrom != null) p.put("DueFrom", dueFrom);
        if (dueTo != null) p.put("DueTo", dueTo);
        if (paymentTermId != 0) p.put("PaymentTermId", paymentTermId);
        if (deliveryTermId != 0) p.put("DeliveryTermId", deliveryTermId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        if (sortNo != 0) p.put("SortNo", sortNo);
        return sets("USP_ExportReceivableByDueDateRegisterA", p);
    }

    // ================================================================================ 236

    /** ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate - first cell of the first row, null when none. */
    public Object latestAttentiveLoadDate(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "USp_ExImLcOrderShipmentSchedule_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetLatestAttentiveLoadDate"));
        if (r.isEmpty()) return null;
        return r.get(0).values().iterator().next();
    }

    /**
     * ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicB - Usp_ExportContractSchedulePeriodicB:
     * [0] company logo, [1] loading date + item, [2] loading date + item + customer, [3] periodic buckets.
     * @BranchId is always sent (UserAccount.BranchesId), @PerFclTon only when != 0, @DaysInterval / @SortNo /
     * @ReferredStatusId only when != 0.
     */
    public List<List<Map<String, Object>>> contractSchedulePeriodicB(UserAccount u, java.sql.Timestamp from, java.sql.Timestamp to,
                                                                     double perFclTon, int intervalDays, int sortNo, int referredStatusId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (perFclTon != 0.0) p.put("PerFclTon", perFclTon);
        if (intervalDays != 0) p.put("DaysInterval", intervalDays);
        if (sortNo != 0) p.put("SortNo", sortNo);
        if (referredStatusId != 0) p.put("ReferredStatusId", referredStatusId);
        return sets("Usp_ExportContractSchedulePeriodicB", p);
    }

    // ================================================================================ 238

    /** ExImInvoice.GetExportInvoicesFromVouchers (Id, InvoiceNo). */
    public List<Map<String, Object>> exportInvoicesFromVouchers(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetExportInvoicesFromVouchers]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExportReports.EximShipmentCostingReport - SpExport_ShipmentCosting_Report with @ExImInvoiceId. */
    public List<Map<String, Object>> shipmentCosting(UserAccount u, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (invoiceId != 0) p.put("ExImInvoiceId", invoiceId);
        return DesktopProc.rows(jdbc, "SpExport_ShipmentCosting_Report", p);
    }

    // ================================================================================ 239 / 251

    /** ExImLcOrder.GetDataForDropDownFromExportContract (ActivityType, Id, name). */
    public List<Map<String, Object>> dropDownFromExportContract(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportContract]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImLcOrder.PendingWorkExportRegister. @ToDate is always set by the form (never null). */
    public List<Map<String, Object>> pendingWorkExportRegister(UserAccount u, int contractId, java.sql.Timestamp from, java.sql.Timestamp to,
                                                               int supplierCustomerId, int actionId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (contractId != 0) p.put("ContractId", contractId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (actionId != 0) p.put("InvoiceStatus", actionId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_PendingWorkExportRegister]", p);
    }

    /** ExImInvoice.ExportInvoice_StatusUpdate(InvoiceId, UserId, "Complete"|"Open"). */
    public void exportInvoiceStatusUpdate(int invoiceId, int userId, String status) {
        DesktopProc.rows(jdbc, "usp_ExportInvoice_StatusUpdate",
                params("ExImInvoiceId", invoiceId, "UserId", userId, "InvoiceStatus", status));
    }

    /**
     * ExImLcOrder.EximLcOrder_Register - Sp_ExImLcOrder_ExportRegister_Rpt. The BLL spells the skip-zero
     * parameter "@SkipZero " (trailing space); the procedure declares @SkipZero and that is what is sent here.
     */
    public List<Map<String, Object>> eximLcOrderRegister(UserAccount u, int lcOrderId, int supplierCustomerId, int itemId, int tradeTypeId,
                                                         int farmingTypeId, java.sql.Timestamp from, java.sql.Timestamp to, int skipZero,
                                                         String status, int salePersonId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (lcOrderId != 0) p.put("LcOrderId", lcOrderId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (status != null && !status.isEmpty()) p.put("Status", status);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (tradeTypeId != 0) p.put("TradeTypeId", tradeTypeId);
        if (farmingTypeId != 0) p.put("FarmingTypeId", farmingTypeId);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        if (salePersonId != 0) p.put("SalePersonId", salePersonId);
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_ExportRegister_Rpt", p);
    }

    /**
     * ExImLcOrder.ExImLcOrder_ExportRegisterItemWise - Sp_ExImLcOrder_ExportRegisterItemWise_Rpt. The form never
     * sets ApprovedFilter, so the BLL's (ApprovedFilter != "All") branch always adds @IsApproved = false.
     */
    public List<Map<String, Object>> eximLcOrderRegisterItemWise(UserAccount u, int lcOrderId, int supplierCustomerId, int itemId, int tradeTypeId,
                                                                 int farmingTypeId, java.sql.Timestamp from, java.sql.Timestamp to, int skipZero,
                                                                 String status, int salePersonId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (lcOrderId != 0) p.put("LcOrderId", lcOrderId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (itemId != 0) p.put("ItemId", itemId);
        p.put("IsApproved", Boolean.FALSE);
        if (status != null && !status.isEmpty()) p.put("Status", status);
        if (tradeTypeId != 0) p.put("TradeTypeId", tradeTypeId);
        if (farmingTypeId != 0) p.put("FarmingTypeId", farmingTypeId);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        if (salePersonId != 0) p.put("SalePersonId", salePersonId);
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_ExportRegisterItemWise_Rpt", p);
    }

    /** ExImLcOrder.GetDataForExportInfo - the "card" grid; From/To go both as @FromDate/@ToDate and @FromDateSummary/@ToDateSummary. */
    public List<Map<String, Object>> exportInfoCard(UserAccount u, java.sql.Timestamp from, java.sql.Timestamp to, String dateType) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (from != null) p.put("FromDateSummary", from);
        if (to != null) p.put("ToDateSummary", to);
        if (dateType != null && !dateType.isEmpty()) p.put("DateType", dateType);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_ExportContractSummaryTotalForDashboardsandCardandReports]", p);
    }

    /** ExImLcOrder.ExportContractStatusUpdate(org, comp, ModifyUserId, DocumentTypeId 202, Id, ReqType, Remarks). */
    public void exportContractStatusUpdate(UserAccount u, int userId, int documentTypeId, int id, String reqType, String remarks) {
        DesktopProc.rows(jdbc, "usp_ExportContractStatusUpdate", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Id", id, "ModifyUserId", userId, "ReqType", reqType, "Remarks", remarks));
    }

    // ================================================================================ 249

    /** GeneralReprots.StaticColumnNames(Activity) (ReportType, ReportName). */
    public List<Map<String, Object>> staticColumnNames(String activity) {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", activity));
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForExport (Id, CompanyName). */
    public List<Map<String, Object>> exportCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyIdForExport"));
    }

    /** Item.GetExportItemsByOrganizationCompanyId (Id, ItemName). */
    public List<Map<String, Object>> exportItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetExportItemsByOrganizationCompanyId"));
    }

    /** ItemCategory.Getall (Id, CategoryDescription) - CategoryCode "" so @Ids is not sent. */
    public List<Map<String, Object>> itemCategories(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemCategory_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** ItemType.Getall (Id, TypeDescription) - Type 0 and no parent categories. */
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemType_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** jobLot.GetAll (Id, JobLotDescription). */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /**
     * ExportReports.ExportInvoiceHistoryDetailReport - SpExImInvoice_ExportHistoryDetail_Report. The BLL's @Activity
     * carries obj.Activity, which this form never sets (a CLR null -> not sent); ReportType and JobLotId are set by
     * the form but not forwarded by the BLL, so they never reach the procedure.
     */
    public List<Map<String, Object>> exportHistoryDetail(UserAccount u, java.sql.Timestamp from, java.sql.Timestamp to, int itemId,
                                                         int supplierCustomerId, int lcOrderId, int itemTypeId, int itemCategoryId, int destinationPortId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (itemTypeId != 0) p.put("ItemTypeId", itemTypeId);
        if (itemCategoryId != 0) p.put("ItemCategoryId", itemCategoryId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (lcOrderId != 0) p.put("LcContractId", lcOrderId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        return DesktopProc.rows(jdbc, "SpExImInvoice_ExportHistoryDetail_Report", p);
    }

    // ================================================================================ 250

    /** InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (Activity, Id, ReferenceName). */
    public List<Map<String, Object>> dropDownFromDeliveryOrder(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromDeliveryOrder",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /**
     * DeliverySchedule.GetDeliveryOrderForApproval - Sp_InvDeliveryOrderForApproval_Rpt, [0] rows, [1] company logo.
     * isApproved == null means the "All" filter (parameter left out).
     */
    public List<List<Map<String, Object>>> deliveryOrderForApproval(UserAccount u, int documentTypeId, int supplierCustomerId, int itemId,
                                                                    int exImInvoiceId, int wbWeightStatus, int doReferedStatus,
                                                                    java.sql.Timestamp from, java.sql.Timestamp to, Boolean isApproved, String deliveryOrderType) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (exImInvoiceId != 0) p.put("ExImInvoiceId", exImInvoiceId);
        if (wbWeightStatus != 0) p.put("WbWeightStatus", wbWeightStatus);
        if (doReferedStatus != 0) p.put("DoReferedStatus", doReferedStatus);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (isApproved != null) p.put("IsApproved", isApproved);
        if (deliveryOrderType != null && !deliveryOrderType.isEmpty()) p.put("DeliveryOrderType", deliveryOrderType);
        return sets("Sp_InvDeliveryOrderForApproval_Rpt", p);
    }

    // ================================================================================ plumbing

    /**
     * GenericProvider.GetDataSetProc: every result set of the procedure, in order, as case-insensitive rows
     * (DesktopProc.rows keeps only the first). A null value is omitted from the EXEC, never bound as NULL.
     */
    public List<List<Map<String, Object>>> sets(String proc, Map<String, Object> p) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC ");
        sb.append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (e.getValue() == null) continue;
            sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.execute(sb.toString(), (PreparedStatementCallback<List<List<Map<String, Object>>>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<List<Map<String, Object>>> out = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        List<Map<String, Object>> t = new ArrayList<>();
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "Column" + c;
                                if (!row.containsKey(label)) row.put(label, rs.getObject(c));
                            }
                            t.add(row);
                        }
                        out.add(t);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return out;
        });
    }

    /** Case-insensitive column read. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** Tables[i] of a data set, an empty list when the procedure returned fewer sets. */
    public static List<Map<String, Object>> table(List<List<Map<String, Object>>> sets, int i) {
        return sets != null && sets.size() > i ? sets.get(i) : new ArrayList<>();
    }
}
