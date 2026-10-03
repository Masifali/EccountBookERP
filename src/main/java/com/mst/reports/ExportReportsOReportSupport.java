package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.List;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the Export Reports group O pages (ExportReportsOController), registered from this component so
 * ReportRegistry.java is not edited (same pattern as HrmReportSupport / ExportReportSupport). The pages print with
 * CrystalPrint.open(key, args) -> POST /api/reports/{key}/print.pdf; arg:<name> are the JSON keys the pages send.
 *
 * Every one of these desktop buttons prints the DataTable the form already holds (ShowReportWithDataTable(dt, ...)).
 * The contract re-runs the SAME procedure with the SAME parameters the form used for that DataTable (the page sends
 * the filters of its last Show), so the rows are identical. GUARDED = the BLL's "if (x != 0)" / CheckDateTimeNull omits it.
 * Report parameters: default "@CompanyName" / "@CompanyAddress"; rpt:CompanyName / rpt:CompanyAddress (no @) where the
 * form pushes them without the @ (522, 527, 527 customer wise). All procedures/parameters checked in procdure_index.csv.
 */
@Component
public class ExportReportsOReportSupport {

    private static final ReportDefinition.Param RPT_ADDR = P("rpt:CompanyAddress", "same:@CompanyAddress");
    private static final ReportDefinition.Param RPT_NAME = P("rpt:CompanyName", "same:@CompanyName");
    private static final ReportDefinition.Param ORG = P("@OrganizationId", "session:organizationId");
    private static final ReportDefinition.Param CO = P("@CompanyId", "session:companyId");

    /** ExportReports.ExportSummariesReport - SpExImInvoice_ExportsSummery_Reports, every filter guarded. */
    private static List<ReportDefinition.Param> summaries() {
        return ps(ORG, CO,
                G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@DestinationPortId", "arg:destinationPortId"),
                G("@ItemId", "arg:itemId"), G("@ItemTypeId", "arg:itemTypeId"), G("@ItemCategoryId", "arg:itemCategoryId"),
                G("@ExImLcorderId", "arg:lcOrderId"), G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                G("@ReportType", "arg:reportType"), G("@TradeTypeId", "arg:tradeTypeId"), G("@FarmingTypeId", "arg:farmingTypeId"));
    }

    public ExportReportsOReportSupport(ReportRegistry registry) {

        /* ---------------------------------------------------- ExImCommercialInvoiceHistory (Invoices History / Export History Report) */

        /* toolStripButton2 "522-Print Register" -> ShowRegister527(): dtHistory of ExImLcOrder.EximInvoiceRegister,
           RptPerameter("CompanyAddress") / ("CompanyName") WITHOUT the @. */
        registry.register(def("exp-o-522", "522-ExImInvoiceRegister.rpt", "Sp_ExImInvoice_ReceivedAndOutstandingHistory_Rpt",
                "ExImCommercialInvoiceHistory.ShowRegister527 -> ExImLcOrder.EximInvoiceRegister (BLL 0469)",
                ps(ORG, CO, G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@InvoiceNo", "arg:invoiceNo"),
                        G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@ActionId", "arg:actionId"),
                        G("@TradeTypeId", "arg:tradeTypeId"), G("@FarmingTypeId", "arg:farmingTypeId"), RPT_ADDR, RPT_NAME)));

        /* btnReport537 "537-ExportDetail": dtdetail of ExportReports.ExportInvoiceHistoryDetailReport (@Activity always). */
        registry.register(def("exp-o-537", "537-ExportDetailHistoryReport.rpt", "SpExImInvoice_ExportHistoryDetail_Report",
                "ExImCommercialInvoiceHistory.btnReport537_Click -> ExportReports.ExportInvoiceHistoryDetailReport (BLL 0138)",
                ps(ORG, CO, P("@Activity", "arg:activity"),
                        G("@ItemTypeId", "arg:itemTypeId"), G("@ItemCategoryId", "arg:itemCategoryId"), G("@ItemId", "arg:itemId"),
                        G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@LcContractId", "arg:lcContractId"),
                        G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@DestinationPortId", "arg:destinationPortId"),
                        G("@TradeTypeId", "arg:tradeTypeId"), G("@FarmingTypeId", "arg:farmingTypeId"))));

        /* btnSummaryReport538 (text set by PrintButtonManage): dtSummaries of ExportReports.ExportSummariesReport. */
        registry.register(def("exp-o-538-customer", "538-ExportSummaryByCustomer.rpt", "SpExImInvoice_ExportsSummery_Reports",
                "ExImCommercialInvoiceHistory.btnSummaryReport538_Click (538-ExportSummaryByCustomer)", summaries()));
        registry.register(def("exp-o-538-01-item", "538_01-ExportSummaryByItem.rpt", "SpExImInvoice_ExportsSummery_Reports",
                "ExImCommercialInvoiceHistory.btnSummaryReport538_Click (538_01-ExportSummaryByItem)", summaries()));
        registry.register(def("exp-o-538-02-port", "538_02-ExportSummaryByPort.rpt", "SpExImInvoice_ExportsSummery_Reports",
                "ExImCommercialInvoiceHistory.btnSummaryReport538_Click (538_02-ExportSummaryByPort)", summaries()));

        /* ---------------------------------------------------- ExportReportSummaries */

        /* btnReport "538-Summary": dtdetail of ExportReports.ExportSummariesReport (@CompanyAddress / @CompanyName). */
        registry.register(def("exp-o-538", "538-ExportSummariesReport.rpt", "SpExImInvoice_ExportsSummery_Reports",
                "ExportReportSummaries.btnReport_Click -> ExportReports.ExportSummariesReport (BLL 0138)", summaries()));

        /* ---------------------------------------------------- ExImShipmentScheduleHistory */

        /* toolStripButton2 "527-Print Register" -> ShowRegister527 -> ExportPdfReport.ExpRptSalesContractExportRegister_518.
           The form fills OrderId (the BLL reads LcOrderId -> never sent), the BLL's date guards are inverted
           (CheckDateTimeNull(...) without "!" -> never sent for a picker value), Status is a CLR null (not bound),
           IsApproved false, ItemId 0: only @SupplierCustomerId can follow the filters. */
        registry.register(def("exp-o-527", "527-ExImLcOrderShipmentScheduleDetailRegister.rpt", "Sp_ExImLcOrderNo_ExportSlip_Rpt",
                "ExImShipmentScheduleHistory.ShowRegister527 -> ExportPdfReport.ExpRptSalesContractExportRegister_518 (BLL 0137)",
                ps(ORG, CO, G("@SupplierCustomerId", "arg:supplierCustomerId"), RPT_ADDR, RPT_NAME)));

        /* btnregister523 "527-Register Customer Wise" -> ExpRptSalesContractExportRegister_523: dates + customer; the form sets
           OrderId, not ExImLcOrderId, so @LcContractId is never sent; ActionId 0. */
        registry.register(def("exp-o-527-cw", "527-ExImLcOrderShipmentScheduleDetailRegisterCustomerWise.rpt",
                "SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt",
                "ExImShipmentScheduleHistory.ShowRegister527CustomerWise -> ExportPdfReport.ExpRptSalesContractExportRegister_523 (BLL 0137)",
                ps(ORG, CO, G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                        RPT_ADDR, RPT_NAME)));

        /* ---------------------------------------------------- PerformaInvoiceRegister */

        /* btnPrint "Print-546": dtloadExIO of ExImInvoice.GetProformaDataForInvoices. */
        registry.register(def("exp-o-546", "546-GetProformaDataForInvoices.rpt", "USP_GetProformaDataForInvoices",
                "PerformaInvoiceRegister.btnPrint_Click -> ExImInvoice.GetProformaDataForInvoices (BLL 0467)",
                ps(ORG, CO, P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"),
                        P("@DocumentTypeIds", "const:151"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                        G("@FcurrencyId", "arg:fcyId"), G("@ItemId", "arg:itemId"), G("@SkipZero", "arg:skipZero"))));

        /* ---------------------------------------------------- frmExportInvoiceSummaryByMonth */

        /* btnPrint "531-Print Register": dtHistory of ExportReports.SpExImInvoiceSummaryByMonth. */
        registry.register(def("exp-o-531", "531-ExImInvoiceSummeryByMonth.rpt", "SpExImInvoiceSummaryByMonthly_Report",
                "frmExportInvoiceSummaryByMonth.btnPrint_Click -> ExportReports.SpExImInvoiceSummaryByMonth (BLL 0138)",
                ps(ORG, CO, G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"))));

        /* ---------------------------------------------------- frmStockReservedRegister */

        /* btnRegister "225_Register": dtFromDb of InventoryStockReserved.InventoryStockReserved_SlipAndRegister,
           @CompanyName / @CompanyAddress / @PrintedBy (UserAccount.UserName). */
        registry.register(def("exp-o-225", "225_StockReservedRegister.rpt", "[dbo].[USP_InventoryStockReserved_SlipAndRegister]",
                "frmStockReservedRegister.btnRegister_Click -> InventoryStockReserved.InventoryStockReserved_SlipAndRegister (BLL 0509)",
                ps(ORG, CO, P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"),
                        G("@RefDocId", "arg:refDocId"), G("@RefRefDocumentTypeId", "arg:refRefDocumentTypeId"),
                        G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@DocNoFrom", "arg:docNoFrom"), G("@DocNoTo", "arg:docNoTo"),
                        G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@WareHouseId", "arg:warehouseId"), G("@ItemId", "arg:itemId"),
                        G("@JobLotId", "arg:jobLotId"), G("@ActionId", "arg:actionId"), G("@SamplingStatusId", "arg:statusId"),
                        G("@SkipZero", "arg:skipZero"), G("@ClassGroupId", "arg:classGroupId"), G("@ParentCategories", "arg:parentCategoryIds"),
                        P("rpt:@PrintedBy", "session:userName"))));
    }
}
