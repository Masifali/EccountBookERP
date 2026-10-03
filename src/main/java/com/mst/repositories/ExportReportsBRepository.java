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
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Module 17 "Export Reports", group B - the DAL side of eleven desktop report forms. Every call is one of
 * the desktop's own procedures with the desktop's own parameters, read from procdure.utf8.sql / procdure_index.csv
 * on 30-Sep-2026 (GetDataTableProc sends the listed SqlParameters; a CLR null is never bound, so a
 * parameter the BLL guards with "if (x != 0)" is simply left out here as well):
 *
 *   254 ExportPendingShipmentsFollowupReport
 *     Sp_ExImLookUps_GetAllMethod            @Activity='ReadByOrganizationCompanyIdNExImLookUpTypeId' @OrganizationId @CompanyId [@ExImLookUptypesId]   (types 5 Document Status, 6 Current Status)
 *     USP_GetDataForDropDownFromExportInvoice @OrganizationId @CompanyId                                (ActivityType Customer / DestinationPort)
 *     ExportPendingShipmentsFollowup          @OrganizationId @CompanyId @BranchId [@SupplierCustomerId] [@DestinationPort] [@DocumentStatusIds] [@CurrentStatusIds] [@ActivityId] [@StatusId] [@SortNo]  -> 2 result sets (grid, ETA buckets)
 *     Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt  @OrganizationId @CompanyId [@ExIminvoiceId] [@Id] [@SupplierCustomerId] [@DestinationPortId]  (266 grid + 560 CRO slip check)
 *   261 frmAuditByWeightReport
 *     USP_GetDataForDropDownFromDeliveryOrder @OrganizationId @CompanyId                                (Activity InvoiceNo / ContractNo)
 *     SpEximInvoice_DoWeightWbWeightDiff_Rpt  @OrganizationId @CompanyId [@InvoiceId] [@ContractId] [@GpDateFrom] [@gpDateTo] [@InvoiceStatus]
 *   262 ExportLoadingSheet
 *     USP_GetDataForDropDownFromGoodsForwarding @OrganizationId @CompanyId [@Activity]               (Activity Invoice / Customer; 269 / 913 per activity)
 *     [dbo].[USP_ExportLoadSheet]             @OrganizationId @CompanyId [@InvoiceId] [@SupplierCustomerId] [@FromDate] [@ToDate]
 *   266 frmExportShipingLineBookingRpt
 *     USP_GetDataForDropDownFromShippingBookingInfo @OrganizationId @CompanyId                       (Activity Customer / InvoiceNo / DestinationPort)
 *   269 ExImForwardingHistory / 913 ExImForwardingCostingReport
 *     Sp_ExImInvoice_GetAllMethod             @OrganizationId @CompanyId [@SupplierCustomerId] @Activity='InvoiceNo'  (CommonServices.GetExportInvoiceNo)
 *     Sp_ExImForwarding_SlipAndRegister_Rpt   @OrganizationId @CompanyId [@EximInvoiceId] [@FromDate] [@ToDate] [@GpDateFrom] [@GpDateTo] [@GpSrNoFrom] [@GpSrNoTo] [@SupplierCustomerId] [@ItemId] [@ContractId] [@WarehouseId] [@ToWarehouseId] [@Activity]  (first table)
 *   270 ExportSalesReport
 *     [dbo].[USP_GetExportInvoicesFromVouchers] / [dbo].[USP_GetExportCustomerFromVouchers] / [dbo].[USP_GetExportItemsFromVouchers] / [dbo].[USP_GetExportJobLotsFromVouchers]   @OrganizationId @CompanyId [@FinancialYearId]
 *     USP_GetExportSales                      @OrganizationId @CompanyId @FinancialYearId [@SupplierCustomerId] [@InvoiceId] [@FromDate] [@ToDate] [@ItemId] [@JobLotId] [@ActionId]
 *   759 ExportCustomerWiseComparisonsSummeryReport / 919 frmExImShipmentCostingSummary
 *     SpStaticColumnNames                     @Activity='ExportComparisonsSummeryNew'
 *     [dbo].[USP_GetDataForDropDownFromExportInvoiceVoucher] @OrganizationId @CompanyId
 *     usp_ExportSaleDetail_report             @OrganizationId @CompanyId [@FromDate] [@ToDate] [@SupplierCustomerId] [@DetinationPortId] [@ItemId] [@ItemTypeId] [@ItemCategoryId] [@LcContractId] [@ExImInvoiceId] [@SalePersonId] [@ItemStockAccountId]
 *     SpExImInvoice_ExportComparisonsSummery_Report  the same + @ReportType
 *     USP_ExportShipmentCosting_SummaryReport @OrganizationId @CompanyId [@FromDate] [@ToDate] [@ExImInvoiceId] [@SupplierCustomerId] [@DestinationPortId]
 *   912 frmServiceBillHistory
 *     USP_GetDataForDropDownFromClearingAgentBill @OrganizationId @CompanyId                          (@Activity null on the desktop -> not sent)
 *     Sp_ExImClearingAgentBill_Rpt            @OrganizationId @CompanyId [@Id] [@InvoiceId] [@SupplierCustomerId] [@ItemId] [@FromDate] [@ToDate] [@LoadingPortId] [@DestinationPortId]
 *   935 frmFinancialInstrumentAdvanceBalanceSummary
 *     Sp_SupplierCustomer_GetAllMethod        @OrganizationId @CompanyId @Activity='GetCustomerIdByLcOrderAndFcReceipt'
 *     USP_GetDataForDropDownFromFcyBankReceipts @OrganizationId @CompanyId @Activity='ReceiverAccount'
 *     usp_FinancialInstrumentAdvanceBalanceSummary @OrganizationId @CompanyId [@SupplierCustomerId] [@BankId] [@FromDate] [@ToDate] [@SkipZero] [@ActionId]
 *     usp_getFIUtilizeInfoByFIId              @FIId @FIDocumentTypeId                              (frmFinancialInstrumentUtilizedDetailByFI popup)
 *   Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId  @OrganizationId @CompanyId             (clsGlobalVariables.ActiveYr.Start_Period)
 *
 * No table, column or procedure is created or changed; every call is read-only.
 */
@Repository
public class ExportReportsBRepository {

    private final JdbcTemplate jdbc;

    public ExportReportsBRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Object nz(int v) { return v != 0 ? v : null; }
    private static Object nz(String v) { return v == null || v.trim().isEmpty() ? null : v; }

    // ------------------------------------------------------------------ shared

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

    /** ExImLookUps.GetByLookTypeId (BLL 0473) - Sp_ExImLookUps_GetAllMethod. */
    public List<Map<String, Object>> lookUpsByType(UserAccount u, int lookUpTypeId) {
        Map<String, Object> p = params("Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (lookUpTypeId != 0) p.put("ExImLookUptypesId", lookUpTypeId);
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", p);
    }

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice (BLL 0469) - no Activity on the desktop call. */
    public List<Map<String, Object>> dropDownFromExportInvoice(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImForwarding.GetDataForDropDownFromGoodsForwarding (BLL 0465) - @Activity only when given. */
    public List<Map<String, Object>> dropDownFromGoodsForwarding(UserAccount u, String activity) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromGoodsForwarding", p);
    }

    /** ExImExportShipingLineBooking.GetDataForDropDownFromShippingBookingInfo (BLL 0463). */
    public List<Map<String, Object>> dropDownFromShippingBookingInfo(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromShippingBookingInfo",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (BLL 0558) - only the two tenancy parameters on this form. */
    public List<Map<String, Object>> dropDownFromDeliveryOrder(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromDeliveryOrder",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExportVoucher.GetDataForDropDownFromExportInvoiceVoucher (BLL 0456). */
    public List<Map<String, Object>> dropDownFromExportInvoiceVoucher(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoiceVoucher]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImClearingAgentBill.GetDataForDropDownFromClearingAgentBill(org, comp, null) (BLL 0114) - the null Activity is not sent. */
    public List<Map<String, Object>> dropDownFromClearingAgentBill(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromClearingAgentBill",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.GetExportInvoiceNo(SupplierCustomerId) -> ExImInvoice.GetInvoiceNo: Sp_ExImInvoice_GetAllMethod 'InvoiceNo'. */
    public List<Map<String, Object>> exportInvoiceNos(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "InvoiceNo");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImExportShipingLineBooking.PrintSlipandRegister (BLL 0463 :395). */
    public List<Map<String, Object>> bookingSlipAndRegister(UserAccount u, int exImInvoiceId, int id, int supplierCustomerId, int destinationPortId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (exImInvoiceId != 0) p.put("ExIminvoiceId", exImInvoiceId);
        if (id != 0) p.put("Id", id);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        return DesktopProc.rows(jdbc, "Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt", p);
    }

    // ------------------------------------------------------------------ 254

    /**
     * ExImShippedConsignmentFollowUps.ExportPendingShipmentsFollowup (BLL 0471 :188) - GetDataSetProc: both
     * result sets come back ([0] the follow-up rows, [1] the ETA-destination buckets).
     */
    public List<List<Map<String, Object>>> pendingShipmentsFollowup(UserAccount u, int supplierCustomerId, int destinationPortId,
                                                                    String documentStatusIds, String currentStatusIds,
                                                                    int activityId, int statusId, int sortNo) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (destinationPortId != 0) p.put("DestinationPort", destinationPortId);
        if (nz(documentStatusIds) != null) p.put("DocumentStatusIds", documentStatusIds);
        if (nz(currentStatusIds) != null) p.put("CurrentStatusIds", currentStatusIds);
        if (activityId != 0) p.put("ActivityId", activityId);
        if (statusId != 0) p.put("StatusId", statusId);
        if (sortNo != 0) p.put("SortNo", sortNo);
        return resultSets(jdbc, "ExportPendingShipmentsFollowup", p, 2);
    }

    // ------------------------------------------------------------------ 261

    /** ExImInvoice.ExportSalesAuditByWeight (BLL 0467 :1575) - PageNumber / PageSize are never set by this form. */
    public List<Map<String, Object>> salesAuditByWeight(UserAccount u, int invoiceId, int contractId, java.sql.Date from, java.sql.Date to, String status) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (contractId != 0) p.put("ContractId", contractId);
        if (from != null) p.put("GpDateFrom", from);
        if (to != null) p.put("gpDateTo", to);
        if (nz(status) != null) p.put("InvoiceStatus", status);
        return DesktopProc.rows(jdbc, "SpEximInvoice_DoWeightWbWeightDiff_Rpt", p);
    }

    // ------------------------------------------------------------------ 262

    /** ExportReports.ExportLoadingSheet (BLL 0138 :1190). */
    public List<Map<String, Object>> exportLoadSheet(UserAccount u, int invoiceId, int supplierCustomerId, java.sql.Date from, java.sql.Date to) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExportLoadSheet]", p);
    }

    // ------------------------------------------------------------------ 269 / 913

    /** ExImForwarding.ExImForwardingSlipandRegister (BLL 0465 :418) - the first table (the second is the company logo). */
    public List<Map<String, Object>> forwardingSlipAndRegister(UserAccount u, int eximInvoiceId, java.sql.Date from, java.sql.Date to,
                                                               java.sql.Date gpFrom, java.sql.Date gpTo, int gpSrNoFrom, int gpSrNoTo,
                                                               int supplierCustomerId, int itemId, int contractId, int warehouseId,
                                                               int toWarehouseId, String activity) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (eximInvoiceId != 0) p.put("EximInvoiceId", eximInvoiceId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (gpFrom != null) p.put("GpDateFrom", gpFrom);
        if (gpTo != null) p.put("GpDateTo", gpTo);
        if (gpSrNoFrom != 0) p.put("GpSrNoFrom", gpSrNoFrom);
        if (gpSrNoTo != 0) p.put("GpSrNoTo", gpSrNoTo);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (contractId != 0) p.put("ContractId", contractId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (toWarehouseId != 0) p.put("ToWarehouseId", toWarehouseId);
        if (nz(activity) != null) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_SlipAndRegister_Rpt", p);
    }

    // ------------------------------------------------------------------ 270

    /** The four "...FromVouchers" combo procedures (BLL 0467 :2925, 0600 :1492, 0583 :2465, 0594 :122). */
    public List<Map<String, Object>> fromVouchers(UserAccount u, String proc, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        return DesktopProc.rows(jdbc, "[dbo].[" + proc + "]", p);
    }

    /** ExportReports.GetExportSalesReport (BLL 0138 :1005) - USP_GetExportSales. */
    public List<Map<String, Object>> exportSales(UserAccount u, int financialYearId, int supplierCustomerId, int invoiceId,
                                                 java.sql.Date from, java.sql.Date to, int itemId, int jobLotId, int actionId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (itemId != 0) p.put("ItemId", itemId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (actionId != 0) p.put("ActionId", actionId);
        return DesktopProc.rows(jdbc, "USP_GetExportSales", p);
    }

    // ------------------------------------------------------------------ 759

    /** GeneralReprots.StaticColumnNames (BLL 0136 :178). */
    public List<Map<String, Object>> staticColumnNames(String activity) {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", activity));
    }

    /** Filters shared by the two 759 procedures (BLL 0138 :1350 / :592), in the BLL's own parameter order. */
    public static final class ComparisonFilter {
        public java.sql.Date from, to;
        public int supplierCustomerId, destinationPortId, itemId, itemTypeId, itemCategoryId, lcContractId, exImInvoiceId, salePersonId, itemStockAccountId;
        public String reportType;
    }

    /** ExportReports.ExportSaleDetailReport - usp_ExportSaleDetail_report, first table. */
    public List<Map<String, Object>> exportSaleDetail(UserAccount u, ComparisonFilter f) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        comparisonParams(p, f);
        return DesktopProc.rows(jdbc, "usp_ExportSaleDetail_report", p);
    }

    /** ExportReports.ExImInvoice_ExportComparisonsSummery_Report - @ReportType always sent, right after the tenancy pair. */
    public List<Map<String, Object>> comparisonsSummary(UserAccount u, ComparisonFilter f) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ReportType", f.reportType);
        comparisonParams(p, f);
        return DesktopProc.rows(jdbc, "SpExImInvoice_ExportComparisonsSummery_Report", p);
    }

    private static void comparisonParams(Map<String, Object> p, ComparisonFilter f) {
        if (f.from != null) p.put("FromDate", f.from);
        if (f.to != null) p.put("ToDate", f.to);
        if (f.supplierCustomerId != 0) p.put("SupplierCustomerId", f.supplierCustomerId);
        if (f.destinationPortId != 0) p.put("DetinationPortId", f.destinationPortId);
        if (f.itemId != 0) p.put("ItemId", f.itemId);
        if (f.itemTypeId != 0) p.put("ItemTypeId", f.itemTypeId);
        if (f.itemCategoryId != 0) p.put("ItemCategoryId", f.itemCategoryId);
        if (f.lcContractId != 0) p.put("LcContractId", f.lcContractId);
        if (f.exImInvoiceId != 0) p.put("ExImInvoiceId", f.exImInvoiceId);
        if (f.salePersonId != 0) p.put("SalePersonId", f.salePersonId);
        if (f.itemStockAccountId != 0) p.put("ItemStockAccountId", f.itemStockAccountId);
    }

    // ------------------------------------------------------------------ 912

    /** ServicesReports.ServicesBill_SlipAndRegister (BLL 0117 :12) - Sp_ExImClearingAgentBill_Rpt. */
    public List<Map<String, Object>> servicesBillSlipAndRegister(UserAccount u, int id, int invoiceId, int supplierCustomerId, int itemId,
                                                                 java.sql.Date from, java.sql.Date to, int loadingPortId, int destinationPortId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (id != 0) p.put("Id", id);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (loadingPortId != 0) p.put("LoadingPortId", loadingPortId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        return DesktopProc.rows(jdbc, "Sp_ExImClearingAgentBill_Rpt", p);
    }

    // ------------------------------------------------------------------ 919

    /** ExportReports.ExportShipmentCosting_SummaryReport (BLL 0138 :855) - first table. */
    public List<Map<String, Object>> shipmentCostingSummary(UserAccount u, java.sql.Date from, java.sql.Date to, int exImInvoiceId,
                                                            int supplierCustomerId, int destinationPortId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (exImInvoiceId != 0) p.put("ExImInvoiceId", exImInvoiceId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        return DesktopProc.rows(jdbc, "USP_ExportShipmentCosting_SummaryReport", p);
    }

    // ------------------------------------------------------------------ 935

    /** SupplierCustomer.GetCustomerIdByLcOrderAndFcReceipt (BLL 0600 :668). */
    public List<Map<String, Object>> customersByLcOrderAndFcReceipt(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetCustomerIdByLcOrderAndFcReceipt"));
    }

    /** ExImFCBankReceipts.GetDataForDropDownFromFcyBankReceipts(.., "ReceiverAccount"). */
    public List<Map<String, Object>> dropDownFromFcyBankReceipts(UserAccount u, String activity) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (nz(activity) != null) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromFcyBankReceipts", p);
    }

    /** ExImEFormRegistration.FIAdvanceBalanceSummary (BLL 0462 :163) - note the BLL's "@FromDate " (trailing blank) is the same @FromDate. */
    public List<Map<String, Object>> fiAdvanceBalanceSummary(UserAccount u, int supplierCustomerId, int bankId, java.sql.Date from, java.sql.Date to,
                                                             int skipZero, int actionId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (bankId != 0) p.put("BankId", bankId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        if (actionId != 0) p.put("ActionId", actionId);
        return DesktopProc.rows(jdbc, "usp_FinancialInstrumentAdvanceBalanceSummary", p);
    }

    /** ExportReports.getFIUtilizeInfoByFIId (BLL 0138 :312). */
    public List<Map<String, Object>> fiUtilizeInfoByFIId(int fiId, int fiDocumentTypeId) {
        return DesktopProc.rows(jdbc, "usp_getFIUtilizeInfoByFIId", params("FIId", fiId, "FIDocumentTypeId", fiDocumentTypeId));
    }

    // ------------------------------------------------------------------ plumbing

    /** Case-insensitive column read. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /**
     * GenericProvider.GetDataSetProc: the first {@code count} result sets as lists (a missing one is an
     * empty list), with the desktop's null semantics of DesktopProc (a null value is not sent).
     */
    private static List<List<Map<String, Object>>> resultSets(JdbcTemplate jdbc, String proc, Map<String, Object> params, int count) {
        StringBuilder sb = new StringBuilder("EXEC dbo.").append(proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        String sql = sb.toString();
        return jdbc.execute(sql, (PreparedStatementCallback<List<List<Map<String, Object>>>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<List<Map<String, Object>>> out = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        List<Map<String, Object>> rows = new ArrayList<>();
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "Column" + c;
                                if (!row.containsKey(label)) row.put(label, rs.getObject(c));
                            }
                            rows.add(row);
                        }
                        if (out.size() < count) out.add(rows);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            while (out.size() < count) out.add(new ArrayList<>());
            return out;
        });
    }

    private static void bind(PreparedStatement ps, List<Object> args) throws java.sql.SQLException {
        for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
    }

    public static Map<String, Object> newRow() { return new LinkedHashMap<>(); }
}
