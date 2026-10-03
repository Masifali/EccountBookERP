package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Date;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Export Reports - group O (report forms without a ScreenDefinition row). Every call is the desktop's own procedure
 * with the desktop's own parameters, traced form -> CommonServices / BLL -> GenericProvider and checked against
 * procdure_index.csv / procdure.utf8.sql (03-Oct-2026):
 *
 *  shared
 *   Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId @CompanyId           clsGlobalVariables.ActiveYr.Start_Period
 *   Sp_SupplierCustomer_GetAllMethod  @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyIdForExport'
 *                                     SupplierCustomer.ReadByOrganizationCompanyIdForExport (BLL 0600) - Id, CompanyName
 *   Sp_ExImLcOrder_GetAllMethod       @OrganizationId @CompanyId @Activity='GetLcOrderNo'  [@FinancialYearId never: 0]
 *                                     CommonServices.GetLcOrderNo -> ExImLcOrder.GetLcOrderNo (BLL 0469) - Id, LcOrderNo
 *   SpStaticColumnNames               @Activity ('ExportSummary' | 'ExportDetailHistory')        GeneralReprots.StaticColumnNames (BLL 0136)
 *   Sp_Item_GetAllMethod              @OrganizationId @CompanyId @Activity='GetExportItemsByOrganizationCompanyId'   Item (BLL 0583)
 *   Sp_SeaPorts_GetAllMethod          @OrganizationId @CompanyId @Activity='ReadByCompanyNOrganizationId'            SeaPorts.Getall
 *
 *  ExImShipmentScheduleHistory (Shipment Schedule History)
 *   SpExImLcOrder_ShipmentSchedule_Rpt @OrganizationId @CompanyId [@FromDate] [@ToDate] [@LcOrderId] [@SupplierCustomerId]
 *                                     ExImLcOrderShipmentScheduleHeader.ExportShipmentsHistory (BLL 0468)
 *
 *  ExportReportSummaries (Export Summary)
 *   SpExImInvoice_ExportsSummery_Reports @OrganizationId @CompanyId [@SupplierCustomerId] [@DestinationPortId] [@ItemId]
 *                                     [@ItemTypeId] [@ItemCategoryId] [@ExImLcorderId] [@FromDate] [@ToDate] [@ReportType]
 *                                     [@TradeTypeId] [@FarmingTypeId]      ExportReports.ExportSummariesReport (BLL 0138)
 *
 *  ExImCommercialInvoiceHistory (Invoices History / Export History Report)
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId        ExImLcOrder.GetDataForDropDownFromExportInvoice (0469)
 *   Sp_ExImInvoice_ReceivedAndOutstandingHistory_Rpt @OrganizationId @CompanyId [@FromDate] [@ToDate] [@InvoiceNo]
 *                                     [@SupplierCustomerId] [@ActionId] [@TradeTypeId] [@FarmingTypeId]   ExImLcOrder.EximInvoiceRegister (0469)
 *   SpExImInvoice_ExportHistoryDetail_Report @OrganizationId @CompanyId @Activity [@ItemTypeId] [@ItemCategoryId] [@ItemId]
 *                                     [@FromDate] [@ToDate] [@LcContractId] [@SupplierCustomerId] [@DestinationPortId]
 *                                     [@TradeTypeId] [@FarmingTypeId]      ExportReports.ExportInvoiceHistoryDetailReport (0138)
 *   SpExImInvoice_ExportsSummery_Reports (as above)                         ExportReports.ExportSummariesReport (0138)
 *
 *  frmExportInvoiceSummaryByMonth (Summary By Month)
 *   SpExImInvoiceSummaryByMonthly_Report @OrganizationId @CompanyId [@FromDate] [@ToDate]   ExportReports.SpExImInvoiceSummaryByMonth (0138)
 *
 *  PerformaInvoiceRegister (Performa Invoice Register)
 *   Sp_Item_GetAllMethod              @OrganizationId @CompanyId @Activity='ReadAllForComboTwoColumns'   CommonServices.ItemGetForComboServiceBind -> Item.GetAllbyCombobind
 *   Sp_MultiCurrency_GetAllMethod     @OrganizationId @CompanyId @Activity='ReadAll'                     MultiCurrency.GetAll (BLL 0076)
 *   Sp_SupplierCustomer_GetAllMethod  @OrganizationId @CompanyId @CustomerGroupIds='7' @Activity='GetSupplierustomerByCustomerGroupId'
 *                                     CommonServices.GetSupplierustomerByCustomerGroupId("7") -> SupplierCustomer (BLL 0600)
 *   USP_GetProformaDataForInvoices    @OrganizationId @CompanyId @BranchesId @FinancialYearId @DocumentTypeIds='151'
 *                                     [@SupplierCustomerId] [@FcurrencyId] [@ItemId] [@SkipZero]       ExImInvoice.GetProformaDataForInvoices (0467)
 *   Sp_DMSAttachments_GetAllMethod    @RefDocumentTypeId=151 @Id @Activity='ReadAttachmentsbyRefDocumentTypeId'
 *                                     CommonServices.GetNoofAttachmentsByRefDocumentTypeID -> DMSAttachments (BLL 0069)
 *
 *  frmStockReservedRegister (Stock Reserved Against third Party Analysis)
 *   dbo.[Usp_AllComboAgainstInventoryStockReserved] @OrganizationId @CompanyId
 *                                     InventoryStockReserved.AllComboAgainstInventoryStockReserved (BLL 0509; @Activity is a CLR null, never bound)
 *   [dbo].[USP_InventoryStockReserved_SlipAndRegister] @OrganizationId @CompanyId @BranchesId @FinancialYearId [@RefDocId]
 *                                     [@RefRefDocumentTypeId] [@FromDate] [@ToDate] [@DocNoFrom] [@DocNoTo] [@SupplierCustomerId]
 *                                     [@WareHouseId] [@ItemId] [@JobLotId] [@ActionId] [@SamplingStatusId] [@SkipZero]
 *                                     [@ClassGroupId] [@ParentCategories]  InventoryStockReserved.InventoryStockReserved_SlipAndRegister (0509)
 *
 * A bracketed parameter is one the BLL wraps in a guard and is left out when unset. A null value is never bound
 * (DesktopProc omits it). Nothing here creates or changes a table, column or procedure.
 */
@Repository
public class ExportReportsORepository {

    private final JdbcTemplate jdbc;

    public ExportReportsORepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenancy(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    private static void guard(Map<String, Object> p, String name, int v) { if (v != 0) p.put(name, v); }
    private static void guard(Map<String, Object> p, String name, double v) { if (v != 0.0) p.put(name, v); }
    private static void guard(Map<String, Object> p, String name, String v) { if (v != null && !v.isEmpty()) p.put(name, v); }
    private static void guard(Map<String, Object> p, String name, Date v) { if (v != null) p.put(name, v); }

    // ================================================================================ shared

    /** clsGlobalVariables.ActiveYr - the active year row with the session's FinancialYearId (else the first); null when none. */
    public Map<String, Object> activeYear(UserAccount u, int financialYearId) {
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", tenancy(u));
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) { row = r; break; }
        }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row;
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForExport (Id, CompanyName). */
    public List<Map<String, Object>> customersForExport(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", "ReadByOrganizationCompanyIdForExport");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServices.GetLcOrderNo() -> ExImLcOrder.GetLcOrderNo (Id, LcOrderNo); FinancialYearId 0 so not sent. */
    public List<Map<String, Object>> lcOrderNos(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", "GetLcOrderNo");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /** GeneralReprots.StaticColumnNames - the BLL sends only @Activity. */
    public List<Map<String, Object>> staticColumnNames(String activity) {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", activity));
    }

    /** Item.GetExportItemsByOrganizationCompanyId (Id, ItemName). */
    public List<Map<String, Object>> exportItems(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", "GetExportItemsByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** CommonServices.GetSeaPortForComboServiceBind -> SeaPorts.Getall (Id, PortName). */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", "ReadByCompanyNOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", p);
    }

    // ================================================================================ Shipment Schedule History

    /**
     * ExImLcOrderShipmentScheduleHeader.ExportShipmentsHistory: the form fills OrderId (-> @LcOrderId), SupplierCustomerId
     * and both picker dates; BranchesId / ProjectsId / Status / ItemId stay at their defaults and are not sent.
     */
    public List<Map<String, Object>> shipmentsHistory(UserAccount u, int orderId, int customerId, Date from, Date to) {
        Map<String, Object> p = tenancy(u);
        guard(p, "FromDate", from);
        guard(p, "ToDate", to);
        guard(p, "LcOrderId", orderId);
        guard(p, "SupplierCustomerId", customerId);
        return DesktopProc.rows(jdbc, "SpExImLcOrder_ShipmentSchedule_Rpt", p);
    }

    // ================================================================================ Export summaries (two forms)

    /** ExportReports.ExportSummariesReport - every filter guarded. */
    public List<Map<String, Object>> exportSummaries(UserAccount u, int customerId, int portId, int itemId, int itemTypeId,
                                                     int itemCategoryId, int lcOrderId, Date from, Date to, String reportType,
                                                     int tradeTypeId, int farmingTypeId) {
        Map<String, Object> p = tenancy(u);
        guard(p, "SupplierCustomerId", customerId);
        guard(p, "DestinationPortId", portId);
        guard(p, "ItemId", itemId);
        guard(p, "ItemTypeId", itemTypeId);
        guard(p, "ItemCategoryId", itemCategoryId);
        guard(p, "ExImLcorderId", lcOrderId);
        guard(p, "FromDate", from);
        guard(p, "ToDate", to);
        guard(p, "ReportType", reportType);
        guard(p, "TradeTypeId", tradeTypeId);
        guard(p, "FarmingTypeId", farmingTypeId);
        return DesktopProc.rows(jdbc, "SpExImInvoice_ExportsSummery_Reports", p);
    }

    // ================================================================================ Commercial Invoice History

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice with no @Activity / @DocumentTypeIds - (Id, name, ActivityType). */
    public List<Map<String, Object>> dropDownFromExportInvoice(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]", tenancy(u));
    }

    /** ExImLcOrder.EximInvoiceRegister - @InvoiceNo is the combo TEXT, sent when not empty. */
    public List<Map<String, Object>> invoiceRegister(UserAccount u, Date from, Date to, String invoiceNo, int customerId,
                                                     int actionId, int tradeTypeId, int farmingTypeId) {
        Map<String, Object> p = tenancy(u);
        guard(p, "FromDate", from);
        guard(p, "ToDate", to);
        guard(p, "InvoiceNo", invoiceNo);
        guard(p, "SupplierCustomerId", customerId);
        guard(p, "ActionId", actionId);
        guard(p, "TradeTypeId", tradeTypeId);
        guard(p, "FarmingTypeId", farmingTypeId);
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_ReceivedAndOutstandingHistory_Rpt", p);
    }

    /** ExportReports.ExportInvoiceHistoryDetailReport - @Activity always sent (the report-type text, "" included). */
    public List<Map<String, Object>> exportHistoryDetail(UserAccount u, String activity, int itemTypeId, int itemCategoryId,
                                                         int itemId, Date from, Date to, int lcOrderId, int customerId,
                                                         int portId, int tradeTypeId, int farmingTypeId) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", activity == null ? "" : activity);
        guard(p, "ItemTypeId", itemTypeId);
        guard(p, "ItemCategoryId", itemCategoryId);
        guard(p, "ItemId", itemId);
        guard(p, "FromDate", from);
        guard(p, "ToDate", to);
        guard(p, "LcContractId", lcOrderId);
        guard(p, "SupplierCustomerId", customerId);
        guard(p, "DestinationPortId", portId);
        guard(p, "TradeTypeId", tradeTypeId);
        guard(p, "FarmingTypeId", farmingTypeId);
        return DesktopProc.rows(jdbc, "SpExImInvoice_ExportHistoryDetail_Report", p);
    }

    // ================================================================================ Summary By Month

    /** ExportReports.SpExImInvoiceSummaryByMonth. */
    public List<Map<String, Object>> summaryByMonth(UserAccount u, Date from, Date to) {
        Map<String, Object> p = tenancy(u);
        guard(p, "FromDate", from);
        guard(p, "ToDate", to);
        return DesktopProc.rows(jdbc, "SpExImInvoiceSummaryByMonthly_Report", p);
    }

    // ================================================================================ Performa Invoice Register

    /** CommonServices.ItemGetForComboServiceBind() -> Item.GetAllbyCombobind (ParentCategoryId 0 so not sent). */
    public List<Map<String, Object>> itemsForCombo(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", "ReadAllForComboTwoColumns");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** MultiCurrency.GetAll (Id, CurrencyName). */
    public List<Map<String, Object>> currencies(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p);
    }

    /** SupplierCustomer.GetSupplierustomerByCustomerGroupId(Ids = "7", ParentId 0 -> not sent). */
    public List<Map<String, Object>> customersByGroup(UserAccount u, String groupIds) {
        Map<String, Object> p = tenancy(u);
        guard(p, "CustomerGroupIds", groupIds);
        p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /**
     * ExImInvoice.GetProformaDataForInvoices: @BranchesId / @FinancialYearId / @DocumentTypeIds always; the form never
     * sets FromDate / ToDate / Ids / ContractScheduleIds, so those are not sent (its date pickers are not used).
     */
    public List<Map<String, Object>> proformaData(UserAccount u, int branchesId, int financialYearId, int customerId,
                                                  int fcyId, int itemId, int zeroBalanceType) {
        Map<String, Object> p = tenancy(u);
        p.put("BranchesId", branchesId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", "151");
        guard(p, "SupplierCustomerId", customerId);
        guard(p, "FcurrencyId", fcyId);
        guard(p, "ItemId", itemId);
        guard(p, "SkipZero", zeroBalanceType);
        return DesktopProc.rows(jdbc, "USP_GetProformaDataForInvoices", p);
    }

    /** DMSAttachments.ReadAttachmentsbyRefDocumentTypeId(Id, RefDocumentTypeId). */
    public List<Map<String, Object>> attachmentsByRefDocumentType(int id, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod",
                params("RefDocumentTypeId", refDocumentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    // ================================================================================ Stock Reserved Register

    /** InventoryStockReserved.AllComboAgainstInventoryStockReserved (ActivityType, Id, name). */
    public List<Map<String, Object>> stockReservedCombos(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstInventoryStockReserved]", tenancy(u));
    }

    /**
     * InventoryStockReserved.InventoryStockReserved_SlipAndRegister. The form fills obj.CropYear (a string) and never
     * obj.CropYearId, so @CropYearId is never sent; PackingTypeId is never set either.
     */
    public List<Map<String, Object>> stockReservedRegister(UserAccount u, int branchesId, int financialYearId, int refDocId,
                                                           int refRefDocumentTypeId, Date from, Date to, double docNoFrom,
                                                           double docNoTo, int customerId, int warehouseId, int itemId,
                                                           int jobLotId, int actionId, int statusId, int skipZero,
                                                           int classGroupId, String parentCategories) {
        Map<String, Object> p = tenancy(u);
        p.put("BranchesId", branchesId);
        p.put("FinancialYearId", financialYearId);
        guard(p, "RefDocId", refDocId);
        guard(p, "RefRefDocumentTypeId", refRefDocumentTypeId);
        guard(p, "FromDate", from);
        guard(p, "ToDate", to);
        guard(p, "DocNoFrom", docNoFrom);
        guard(p, "DocNoTo", docNoTo);
        guard(p, "SupplierCustomerId", customerId);
        guard(p, "WareHouseId", warehouseId);
        guard(p, "ItemId", itemId);
        guard(p, "JobLotId", jobLotId);
        guard(p, "ActionId", actionId);
        guard(p, "SamplingStatusId", statusId);
        guard(p, "SkipZero", skipZero);
        guard(p, "ClassGroupId", classGroupId);
        guard(p, "ParentCategories", parentCategories);
        return DesktopProc.rows(jdbc, "[dbo].[USP_InventoryStockReserved_SlipAndRegister]", p);
    }

    // ================================================================================ plumbing

    /** Case-insensitive column read. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }
}
