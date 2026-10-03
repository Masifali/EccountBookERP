package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.ProductionPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Production print actions. Generated from the verified seeder contracts. */
@Controller
public class ProductionPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 666-DailyPlantConsumedHours_FormHistoryReport.rpt
     * Procedure: [dbo].[USP_DailyPlantConsumedHours_FormHistoryReport]
     * Desktop: DailyPlantConsumedHours.FormHistoryAndReport
     */
    @RequestMapping(value = "/reports/print/666-daily-plant-consumed-hours-form-history-report", method = RequestMethod.POST)
    public void print666DailyPlantConsumedHoursFormHistoryReport(HttpServletResponse response, @RequestBody(required = false) Rpt666DailyPlantConsumedHoursFormHistoryReportRequest request) throws Exception {
        if (request == null) request = new Rpt666DailyPlantConsumedHoursFormHistoryReportRequest();
        printReport(response, "666-DailyPlantConsumedHours_FormHistoryReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/666-daily-plant-consumed-hours-form-history-report", method = RequestMethod.GET)
    public void print666DailyPlantConsumedHoursFormHistoryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print666DailyPlantConsumedHoursFormHistoryReport(response, objectMapper.convertValue(query, Rpt666DailyPlantConsumedHoursFormHistoryReportRequest.class));
    }

    /**
     * Template: 627-ProductionOutputAllocationWithExportInvoice.rpt
     * Procedure: [dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]
     * Desktop: ProductionOutputAllocationWithExportInvoice.ProductionOutputAllocationWithExportInvoice_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/627-production-output-allocation-with-export-invoice", method = RequestMethod.POST)
    public void print627ProductionOutputAllocationWithExportInvoice(HttpServletResponse response, @RequestBody(required = false) Rpt627ProductionOutputAllocationWithExportInvoiceRequest request) throws Exception {
        if (request == null) request = new Rpt627ProductionOutputAllocationWithExportInvoiceRequest();
        printReport(response, "627-ProductionOutputAllocationWithExportInvoice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/627-production-output-allocation-with-export-invoice", method = RequestMethod.GET)
    public void print627ProductionOutputAllocationWithExportInvoiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print627ProductionOutputAllocationWithExportInvoice(response, objectMapper.convertValue(query, Rpt627ProductionOutputAllocationWithExportInvoiceRequest.class));
    }

    /**
     * Template: 601-InvFoodProductionSlip.rpt
     * Procedure: Sp_InvFoodProduction_Rpt
     * Desktop: InvFoodProductionReports.InvFoodProductionSlip
     */
    @RequestMapping(value = "/reports/print/601-inv-food-production-slip", method = RequestMethod.POST)
    public void print601InvFoodProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt601InvFoodProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt601InvFoodProductionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "601-InvFoodProductionSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/601-inv-food-production-slip", method = RequestMethod.GET)
    public void print601InvFoodProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print601InvFoodProductionSlip(response, objectMapper.convertValue(query, Rpt601InvFoodProductionSlipRequest.class));
    }

    /**
     * Template: 606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt
     * Procedure: Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder
     * Desktop: InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder
     */
    @RequestMapping(value = "/reports/print/606-inv-food-production-packing-and-over-head-report-by-job-order", method = RequestMethod.POST)
    public void print606InvFoodProductionPackingAndOverHeadReportByJobOrder(HttpServletResponse response, @RequestBody(required = false) Rpt606InvFoodProductionPackingAndOverHeadReportByJobOrderRequest request) throws Exception {
        if (request == null) request = new Rpt606InvFoodProductionPackingAndOverHeadReportByJobOrderRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/606-inv-food-production-packing-and-over-head-report-by-job-order", method = RequestMethod.GET)
    public void print606InvFoodProductionPackingAndOverHeadReportByJobOrderGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print606InvFoodProductionPackingAndOverHeadReportByJobOrder(response, objectMapper.convertValue(query, Rpt606InvFoodProductionPackingAndOverHeadReportByJobOrderRequest.class));
    }

    /**
     * Template: 602A-InvRptProductionSummeryWithValues.rpt
     * Procedure: Sp_InvFoodProduction_Summery_Rpt
     * Desktop: InvFoodProductionReports.InvFoodProductionRecoverySummeryReport
     */
    @RequestMapping(value = "/reports/print/602a-production-summery-with-values", method = RequestMethod.POST)
    public void print602AProductionSummeryWithValues(HttpServletResponse response, @RequestBody(required = false) Rpt602AProductionSummeryWithValuesRequest request) throws Exception {
        if (request == null) request = new Rpt602AProductionSummeryWithValuesRequest();
        printReport(response, "602A-InvRptProductionSummeryWithValues.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/602a-production-summery-with-values", method = RequestMethod.GET)
    public void print602AProductionSummeryWithValuesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print602AProductionSummeryWithValues(response, objectMapper.convertValue(query, Rpt602AProductionSummeryWithValuesRequest.class));
    }

    /**
     * Template: 613-InvRptProductionSummeryWithExpValues.rpt
     * Procedure: Sp_InvFoodProduction_Summery2_Rpt
     * Desktop: InvFoodProductionReports.InvFoodProduction_Summery2_Rpt
     */
    @RequestMapping(value = "/reports/print/613-production-summery-with-exp-values", method = RequestMethod.POST)
    public void print613ProductionSummeryWithExpValues(HttpServletResponse response, @RequestBody(required = false) Rpt613ProductionSummeryWithExpValuesRequest request) throws Exception {
        if (request == null) request = new Rpt613ProductionSummeryWithExpValuesRequest();
        printReport(response, "613-InvRptProductionSummeryWithExpValues.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/613-production-summery-with-exp-values", method = RequestMethod.GET)
    public void print613ProductionSummeryWithExpValuesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print613ProductionSummeryWithExpValues(response, objectMapper.convertValue(query, Rpt613ProductionSummeryWithExpValuesRequest.class));
    }

    /**
     * Template: 615-InvRptProductionSummery.rpt
     * Procedure: Sp_InvFoodProduction_Summery3_Rpt
     * Desktop: InvFoodProductionReports.InvFoodProduction_Summery3_Rpt
     */
    @RequestMapping(value = "/reports/print/615-production-summery", method = RequestMethod.POST)
    public void print615ProductionSummery(HttpServletResponse response, @RequestBody(required = false) Rpt615ProductionSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt615ProductionSummeryRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "615-InvRptProductionSummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/615-production-summery", method = RequestMethod.GET)
    public void print615ProductionSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print615ProductionSummery(response, objectMapper.convertValue(query, Rpt615ProductionSummeryRequest.class));
    }

    /**
     * Template: 623-InvFoodProduction_ConsumptionReport.rpt
     * Procedure: [dbo].[USP_InvFoodProduction_ConsumptionReport]
     * Desktop: InvFoodProductionReports.FoodProduction_ConsumptionReport
     */
    @RequestMapping(value = "/reports/print/623-inv-food-production-consumption-report", method = RequestMethod.POST)
    public void print623InvFoodProductionConsumptionReport(HttpServletResponse response, @RequestBody(required = false) Rpt623InvFoodProductionConsumptionReportRequest request) throws Exception {
        if (request == null) request = new Rpt623InvFoodProductionConsumptionReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "623-InvFoodProduction_ConsumptionReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/623-inv-food-production-consumption-report", method = RequestMethod.GET)
    public void print623InvFoodProductionConsumptionReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print623InvFoodProductionConsumptionReport(response, objectMapper.convertValue(query, Rpt623InvFoodProductionConsumptionReportRequest.class));
    }

    /**
     * Template: 620-ProductionJobOrderSlip.rpt
     * Procedure: Sp_InvProductionJobOrder_Slip_Rpt
     * Desktop: InvProductionJobOrder.GetPrintSlipAndReport
     */
    @RequestMapping(value = "/reports/print/620-production-job-order-slip", method = RequestMethod.POST)
    public void print620ProductionJobOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt620ProductionJobOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt620ProductionJobOrderSlipRequest();
        printReport(response, "620-ProductionJobOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/620-production-job-order-slip", method = RequestMethod.GET)
    public void print620ProductionJobOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print620ProductionJobOrderSlip(response, objectMapper.convertValue(query, Rpt620ProductionJobOrderSlipRequest.class));
    }

    /**
     * Template: 605-StockConversionSummaryNewRpt.rpt
     * Procedure: SpInvStockConversion_Summery_Rpt
     * Desktop: InvFoodProductionReports.StockConversionSummeryReport
     */
    @RequestMapping(value = "/reports/print/605-stock-conversion-summary-new", method = RequestMethod.POST)
    public void print605StockConversionSummaryNew(HttpServletResponse response, @RequestBody(required = false) Rpt605StockConversionSummaryNewRequest request) throws Exception {
        if (request == null) request = new Rpt605StockConversionSummaryNewRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "605-StockConversionSummaryNewRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/605-stock-conversion-summary-new", method = RequestMethod.GET)
    public void print605StockConversionSummaryNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print605StockConversionSummaryNew(response, objectMapper.convertValue(query, Rpt605StockConversionSummaryNewRequest.class));
    }

    /**
     * Template: 812-ProductionKamPackMaterialConsumption.rpt
     * Procedure: Sp_InvProductionCumPackMaterialConsumption_Register
     * Desktop: InvFoodProductionReports.ProductionCumPackMaterialConsumption_Register_812
     */
    @RequestMapping(value = "/reports/print/812-production-kam-pack-material-consumption", method = RequestMethod.POST)
    public void print812ProductionKamPackMaterialConsumption(HttpServletResponse response, @RequestBody(required = false) Rpt812ProductionKamPackMaterialConsumptionRequest request) throws Exception {
        if (request == null) request = new Rpt812ProductionKamPackMaterialConsumptionRequest();
        printReport(response, "812-ProductionKamPackMaterialConsumption.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/812-production-kam-pack-material-consumption", method = RequestMethod.GET)
    public void print812ProductionKamPackMaterialConsumptionGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print812ProductionKamPackMaterialConsumption(response, objectMapper.convertValue(query, Rpt812ProductionKamPackMaterialConsumptionRequest.class));
    }

    /**
     * Template: 604-FoodProductionComparisonRpt.rpt
     * Procedure: SpInvFoodProductionComparisons_Rpt
     * Desktop: InvFoodProduction.FoodProductionComparisons_Rpt
     */
    @RequestMapping(value = "/reports/print/604-food-production-comparison", method = RequestMethod.POST)
    public void print604FoodProductionComparison(HttpServletResponse response, @RequestBody(required = false) Rpt604FoodProductionComparisonRequest request) throws Exception {
        if (request == null) request = new Rpt604FoodProductionComparisonRequest();
        printReport(response, "604-FoodProductionComparisonRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/604-food-production-comparison", method = RequestMethod.GET)
    public void print604FoodProductionComparisonGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print604FoodProductionComparison(response, objectMapper.convertValue(query, Rpt604FoodProductionComparisonRequest.class));
    }

    /**
     * Template: 625_FoodProduction_DocWiseSummeryReport.rpt
     * Procedure: [dbo].[USP_FoodProduction_DocWiseSummeryReport]
     * Desktop: InvFoodProductionReports.FoodProduction_DocWiseSummeryReport
     */
    @RequestMapping(value = "/reports/print/625-food-production-doc-wise-summery-report", method = RequestMethod.POST)
    public void print625FoodProductionDocWiseSummeryReport(HttpServletResponse response, @RequestBody(required = false) Rpt625FoodProductionDocWiseSummeryReportRequest request) throws Exception {
        if (request == null) request = new Rpt625FoodProductionDocWiseSummeryReportRequest();
        printReport(response, "625_FoodProduction_DocWiseSummeryReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/625-food-production-doc-wise-summery-report", method = RequestMethod.GET)
    public void print625FoodProductionDocWiseSummeryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print625FoodProductionDocWiseSummeryReport(response, objectMapper.convertValue(query, Rpt625FoodProductionDocWiseSummeryReportRequest.class));
    }

    /**
     * Template: 164-WagesSummaryByContractorAndJobOrder.rpt
     * Procedure: USp_WagesRegister
     * Desktop: InventoryStockEvalautionDetail.WagesRegister
     */
    @RequestMapping(value = "/reports/print/164-wages-summary-by-contractor-and-job-order", method = RequestMethod.POST)
    public void print164WagesSummaryByContractorAndJobOrder(HttpServletResponse response, @RequestBody(required = false) Rpt164WagesSummaryByContractorAndJobOrderRequest request) throws Exception {
        if (request == null) request = new Rpt164WagesSummaryByContractorAndJobOrderRequest();
        printReport(response, "164-WagesSummaryByContractorAndJobOrder.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/164-wages-summary-by-contractor-and-job-order", method = RequestMethod.GET)
    public void print164WagesSummaryByContractorAndJobOrderGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print164WagesSummaryByContractorAndJobOrder(response, objectMapper.convertValue(query, Rpt164WagesSummaryByContractorAndJobOrderRequest.class));
    }

    /**
     * Template: 461-BoilerConsumption_Slip.rpt
     * Procedure: Sp_BoilerConsumption_Slip&Register
     * Desktop: BoilerConsumption.BoilerConsumptionHistory
     */
    @RequestMapping(value = "/reports/print/461-boiler-consumption-slip", method = RequestMethod.POST)
    public void print461BoilerConsumptionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt461BoilerConsumptionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt461BoilerConsumptionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "461-BoilerConsumption_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/461-boiler-consumption-slip", method = RequestMethod.GET)
    public void print461BoilerConsumptionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print461BoilerConsumptionSlip(response, objectMapper.convertValue(query, Rpt461BoilerConsumptionSlipRequest.class));
    }

    /**
     * Template: 553-ProductionRegisterWithActivity.rpt
     * Procedure: USP_ProductionRegisterWithActivity
     * Desktop: InvFoodProduction.ProductionRegister
     */
    @RequestMapping(value = "/reports/print/553-production-register-with-activity", method = RequestMethod.POST)
    public void print553ProductionRegisterWithActivity(HttpServletResponse response, @RequestBody(required = false) Rpt553ProductionRegisterWithActivityRequest request) throws Exception {
        if (request == null) request = new Rpt553ProductionRegisterWithActivityRequest();
        printReport(response, "553-ProductionRegisterWithActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/553-production-register-with-activity", method = RequestMethod.GET)
    public void print553ProductionRegisterWithActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print553ProductionRegisterWithActivity(response, objectMapper.convertValue(query, Rpt553ProductionRegisterWithActivityRequest.class));
    }

    /**
     * Template: 554-ProductionRegisterWithActivity(OutPut By Packing Material).rpt
     * Procedure: USP_ProductionRegisterWithActivity
     * Desktop: InvFoodProduction.ProductionRegister
     */
    @RequestMapping(value = "/reports/print/554-production-register-with-activity-out-put-by-packing-material", method = RequestMethod.POST)
    public void print554ProductionRegisterWithActivityOutPutByPackingMaterial(HttpServletResponse response, @RequestBody(required = false) Rpt554ProductionRegisterWithActivityOutPutByPackingMaterialRequest request) throws Exception {
        if (request == null) request = new Rpt554ProductionRegisterWithActivityOutPutByPackingMaterialRequest();
        printReport(response, "554-ProductionRegisterWithActivity(OutPut By Packing Material).rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/554-production-register-with-activity-out-put-by-packing-material", method = RequestMethod.GET)
    public void print554ProductionRegisterWithActivityOutPutByPackingMaterialGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print554ProductionRegisterWithActivityOutPutByPackingMaterial(response, objectMapper.convertValue(query, Rpt554ProductionRegisterWithActivityOutPutByPackingMaterialRequest.class));
    }

    /**
     * Template: 562-ProductionRegisterWithActivity.rpt
     * Procedure: USP_ProductionRegisterWithActivity
     * Desktop: InvFoodProduction.ProductionRegister
     */
    @RequestMapping(value = "/reports/print/562-production-register-with-activity", method = RequestMethod.POST)
    public void print562ProductionRegisterWithActivity(HttpServletResponse response, @RequestBody(required = false) Rpt562ProductionRegisterWithActivityRequest request) throws Exception {
        if (request == null) request = new Rpt562ProductionRegisterWithActivityRequest();
        printReport(response, "562-ProductionRegisterWithActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/562-production-register-with-activity", method = RequestMethod.GET)
    public void print562ProductionRegisterWithActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print562ProductionRegisterWithActivity(response, objectMapper.convertValue(query, Rpt562ProductionRegisterWithActivityRequest.class));
    }

    /**
     * Template: 602-InvRptProductionSummery.rpt
     * Procedure: Sp_InvFoodProduction_Summery_Rpt
     * Desktop: InvFoodProductionReports.InvFoodProductionRecoverySummeryReport
     */
    @RequestMapping(value = "/reports/print/602-production-summery", method = RequestMethod.POST)
    public void print602ProductionSummery(HttpServletResponse response, @RequestBody(required = false) Rpt602ProductionSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt602ProductionSummeryRequest();
        printReport(response, "602-InvRptProductionSummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/602-production-summery", method = RequestMethod.GET)
    public void print602ProductionSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print602ProductionSummery(response, objectMapper.convertValue(query, Rpt602ProductionSummeryRequest.class));
    }

    /**
     * Template: 602_01_ProductionSummeryWithLab.rpt
     * Procedure: Sp_InvFoodProduction_Summery_WithOutValue_Rpt
     * Desktop: InvFoodProductionReports.InvFoodProduction_Summery_WithOutValue_Rpt
     */
    @RequestMapping(value = "/reports/print/602-01-production-summery-with-lab", method = RequestMethod.POST)
    public void print60201ProductionSummeryWithLab(HttpServletResponse response, @RequestBody(required = false) Rpt60201ProductionSummeryWithLabRequest request) throws Exception {
        if (request == null) request = new Rpt60201ProductionSummeryWithLabRequest();
        printReport(response, "602_01_ProductionSummeryWithLab.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/602-01-production-summery-with-lab", method = RequestMethod.GET)
    public void print60201ProductionSummeryWithLabGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print60201ProductionSummeryWithLab(response, objectMapper.convertValue(query, Rpt60201ProductionSummeryWithLabRequest.class));
    }

    /**
     * Template: 603-ProductionPendingInvoice.rpt
     * Procedure: Sp_InvPurchaseInvoice_PendingInvoiceForProduction_rpt
     * Desktop: InvFoodProductionReports.PendingInvoicesForProduction
     */
    @RequestMapping(value = "/reports/print/603-production-pending-invoice", method = RequestMethod.POST)
    public void print603ProductionPendingInvoice(HttpServletResponse response, @RequestBody(required = false) Rpt603ProductionPendingInvoiceRequest request) throws Exception {
        if (request == null) request = new Rpt603ProductionPendingInvoiceRequest();
        printReport(response, "603-ProductionPendingInvoice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/603-production-pending-invoice", method = RequestMethod.GET)
    public void print603ProductionPendingInvoiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print603ProductionPendingInvoice(response, objectMapper.convertValue(query, Rpt603ProductionPendingInvoiceRequest.class));
    }

    /**
     * Template: 607-FoodProductionIssuanceGrnWiseByJobOrderId.rpt
     * Procedure: Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt
     * Desktop: InvFoodProductionReports.FoodProductionIssuanceGrnWiseByJobOrderId_607
     */
    @RequestMapping(value = "/reports/print/607-food-production-issuance-grn-wise-by-job-order-id", method = RequestMethod.POST)
    public void print607FoodProductionIssuanceGrnWiseByJobOrderId(HttpServletResponse response, @RequestBody(required = false) Rpt607FoodProductionIssuanceGrnWiseByJobOrderIdRequest request) throws Exception {
        if (request == null) request = new Rpt607FoodProductionIssuanceGrnWiseByJobOrderIdRequest();
        printReport(response, "607-FoodProductionIssuanceGrnWiseByJobOrderId.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/607-food-production-issuance-grn-wise-by-job-order-id", method = RequestMethod.GET)
    public void print607FoodProductionIssuanceGrnWiseByJobOrderIdGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print607FoodProductionIssuanceGrnWiseByJobOrderId(response, objectMapper.convertValue(query, Rpt607FoodProductionIssuanceGrnWiseByJobOrderIdRequest.class));
    }

    /**
     * Template: 613_01-ProductionBeforSettlement_Summery2.rpt
     * Procedure: usp_ProductionBeforSettlement_Summery2_Rpt
     * Desktop: InvFoodProductionReports.ProductionBeforSettlement_Summery2
     */
    @RequestMapping(value = "/reports/print/613-01-production-befor-settlement-summery-2", method = RequestMethod.POST)
    public void print61301ProductionBeforSettlementSummery2(HttpServletResponse response, @RequestBody(required = false) Rpt61301ProductionBeforSettlementSummery2Request request) throws Exception {
        if (request == null) request = new Rpt61301ProductionBeforSettlementSummery2Request();
        printReport(response, "613_01-ProductionBeforSettlement_Summery2.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/613-01-production-befor-settlement-summery-2", method = RequestMethod.GET)
    public void print61301ProductionBeforSettlementSummery2Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print61301ProductionBeforSettlementSummery2(response, objectMapper.convertValue(query, Rpt61301ProductionBeforSettlementSummery2Request.class));
    }

    /**
     * Template: 613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt
     * Procedure: [dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]
     * Desktop: InvFoodProductionReports.ProductionSettlement_WithReferenceDocumentDetailReport
     */
    @RequestMapping(value = "/reports/print/613-02-production-settlement-with-reference-document-detail-report", method = RequestMethod.POST)
    public void print61302ProductionSettlementWithReferenceDocumentDetailReport(HttpServletResponse response, @RequestBody(required = false) Rpt61302ProductionSettlementWithReferenceDocumentDetailReportRequest request) throws Exception {
        if (request == null) request = new Rpt61302ProductionSettlementWithReferenceDocumentDetailReportRequest();
        printReport(response, "613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/613-02-production-settlement-with-reference-document-detail-report", method = RequestMethod.GET)
    public void print61302ProductionSettlementWithReferenceDocumentDetailReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print61302ProductionSettlementWithReferenceDocumentDetailReport(response, objectMapper.convertValue(query, Rpt61302ProductionSettlementWithReferenceDocumentDetailReportRequest.class));
    }

    /**
     * Template: 615_01-ProductionBeforSettlement_Summery3.rpt
     * Procedure: usp_ProductionBeforSettlement_Summery3_Rpt
     * Desktop: InvFoodProductionReports.ProductionBeforSettlement_Summery3
     */
    @RequestMapping(value = "/reports/print/615-01-production-befor-settlement-summery-3", method = RequestMethod.POST)
    public void print61501ProductionBeforSettlementSummery3(HttpServletResponse response, @RequestBody(required = false) Rpt61501ProductionBeforSettlementSummery3Request request) throws Exception {
        if (request == null) request = new Rpt61501ProductionBeforSettlementSummery3Request();
        printReport(response, "615_01-ProductionBeforSettlement_Summery3.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/615-01-production-befor-settlement-summery-3", method = RequestMethod.GET)
    public void print61501ProductionBeforSettlementSummery3Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print61501ProductionBeforSettlementSummery3(response, objectMapper.convertValue(query, Rpt61501ProductionBeforSettlementSummery3Request.class));
    }

    /**
     * Template: 663-InvRptPurchaseOrderDetailRegisterRice.rpt
     * Procedure: USP_GetGrnsPendingOrUsedinProduction
     * Desktop: InvGrn.GetGrnsPendingOrUsedinProduction
     */
    @RequestMapping(value = "/reports/print/663-purchase-order-detail-register-rice", method = RequestMethod.POST)
    public void print663PurchaseOrderDetailRegisterRice(HttpServletResponse response, @RequestBody(required = false) Rpt663PurchaseOrderDetailRegisterRiceRequest request) throws Exception {
        if (request == null) request = new Rpt663PurchaseOrderDetailRegisterRiceRequest();
        printReport(response, "663-InvRptPurchaseOrderDetailRegisterRice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/663-purchase-order-detail-register-rice", method = RequestMethod.GET)
    public void print663PurchaseOrderDetailRegisterRiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print663PurchaseOrderDetailRegisterRice(response, objectMapper.convertValue(query, Rpt663PurchaseOrderDetailRegisterRiceRequest.class));
    }

    /**
     * Template: 666_01_MonthlyPlantConsumedHours_Report.rpt
     * Procedure: [dbo].[USP_DailyPlantConsumedHours_FormHistoryReport]
     * Desktop: DailyPlantConsumedHours.FormHistoryAndReport
     */
    @RequestMapping(value = "/reports/print/666-01-monthly-plant-consumed-hours-report", method = RequestMethod.POST)
    public void print66601MonthlyPlantConsumedHoursReport(HttpServletResponse response, @RequestBody(required = false) Rpt66601MonthlyPlantConsumedHoursReportRequest request) throws Exception {
        if (request == null) request = new Rpt66601MonthlyPlantConsumedHoursReportRequest();
        printReport(response, "666_01_MonthlyPlantConsumedHours_Report.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/666-01-monthly-plant-consumed-hours-report", method = RequestMethod.GET)
    public void print66601MonthlyPlantConsumedHoursReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print66601MonthlyPlantConsumedHoursReport(response, objectMapper.convertValue(query, Rpt66601MonthlyPlantConsumedHoursReportRequest.class));
    }

    /**
     * Template: 672_JobOrderSummaryReport.rpt
     * Procedure: [dbo].[usp_JobOrderSummary_Report]
     * Desktop: InvFoodProduction.JobOrderSummary
     */
    @RequestMapping(value = "/reports/print/672-job-order-summary-report", method = RequestMethod.POST)
    public void print672JobOrderSummaryReport(HttpServletResponse response, @RequestBody(required = false) Rpt672JobOrderSummaryReportRequest request) throws Exception {
        if (request == null) request = new Rpt672JobOrderSummaryReportRequest();
        printReport(response, "672_JobOrderSummaryReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/672-job-order-summary-report", method = RequestMethod.GET)
    public void print672JobOrderSummaryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print672JobOrderSummaryReport(response, objectMapper.convertValue(query, Rpt672JobOrderSummaryReportRequest.class));
    }

    /**
     * Template: 921_I_01_ProductionCostingFormHistoryReport.rpt
     * Procedure: [dbo].[usp_ProductionCosting_FormHistory]
     * Desktop: ProductionManualCosting.ProductionCosting_FormHistoryReport
     */
    @RequestMapping(value = "/reports/print/921-i-01-production-costing-form-history-report", method = RequestMethod.POST)
    public void print921I01ProductionCostingFormHistoryReport(HttpServletResponse response, @RequestBody(required = false) Rpt921I01ProductionCostingFormHistoryReportRequest request) throws Exception {
        if (request == null) request = new Rpt921I01ProductionCostingFormHistoryReportRequest();
        printReport(response, "921_I_01_ProductionCostingFormHistoryReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/921-i-01-production-costing-form-history-report", method = RequestMethod.GET)
    public void print921I01ProductionCostingFormHistoryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print921I01ProductionCostingFormHistoryReport(response, objectMapper.convertValue(query, Rpt921I01ProductionCostingFormHistoryReportRequest.class));
    }

    /**
     * Template: 922_ProdcutionFohAllocateToJobOrderSlip.rpt
     * Procedure: [dbo].[USP_ProdcutionFohAllocateToJobOrder_SlipAndRegister]
     * Desktop: ProdcutionFohAllocateToJobOrder.ProdcutionFohAllocateToJobOrder_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/922-prodcution-foh-allocate-to-job-order-slip", method = RequestMethod.POST)
    public void print922ProdcutionFohAllocateToJobOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt922ProdcutionFohAllocateToJobOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt922ProdcutionFohAllocateToJobOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "922_ProdcutionFohAllocateToJobOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/922-prodcution-foh-allocate-to-job-order-slip", method = RequestMethod.GET)
    public void print922ProdcutionFohAllocateToJobOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print922ProdcutionFohAllocateToJobOrderSlip(response, objectMapper.convertValue(query, Rpt922ProdcutionFohAllocateToJobOrderSlipRequest.class));
    }

    /**
     * Template: ProductionJobOrder_SubReport.rpt
     * Procedure: [dbo].[USP_ProductionJobOrder_SubReport]
     * Desktop: InvProductionJobOrder.JobOrderPlantAndScheduleSubReport
     */
    @RequestMapping(value = "/reports/print/production-job-order-sub-report", method = RequestMethod.POST)
    public void printProductionJobOrderSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionJobOrderSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionJobOrderSubReportRequest();
        printReport(response, "ProductionJobOrder_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-job-order-sub-report", method = RequestMethod.GET)
    public void printProductionJobOrderSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionJobOrderSubReport(response, objectMapper.convertValue(query, RptProductionJobOrderSubReportRequest.class));
    }

    /**
     * Template: ProductionManualCosting_ByProductDetailSubReport.rpt
     * Procedure: [dbo].[USP_ProductionManualCosting_ByProductDetailSubReport]
     * Desktop: ProductionManualCosting.ProductionManualCosting_ByProductDetailSubReport
     */
    @RequestMapping(value = "/reports/print/production-manual-costing-by-product-detail-sub-report", method = RequestMethod.POST)
    public void printProductionManualCostingByProductDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionManualCostingByProductDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionManualCostingByProductDetailSubReportRequest();
        printReport(response, "ProductionManualCosting_ByProductDetailSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-manual-costing-by-product-detail-sub-report", method = RequestMethod.GET)
    public void printProductionManualCostingByProductDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionManualCostingByProductDetailSubReport(response, objectMapper.convertValue(query, RptProductionManualCostingByProductDetailSubReportRequest.class));
    }

    /**
     * Template: ProductionManualCosting_InputDetailSubRepirt.rpt
     * Procedure: [dbo].[USP_ProductionManualCosting_InputDetailSubReport]
     * Desktop: ProductionManualCosting.ProductionManualCosting_InputDetailSubReport
     */
    @RequestMapping(value = "/reports/print/production-manual-costing-input-detail-sub-repirt", method = RequestMethod.POST)
    public void printProductionManualCostingInputDetailSubRepirt(HttpServletResponse response, @RequestBody(required = false) RptProductionManualCostingInputDetailSubRepirtRequest request) throws Exception {
        if (request == null) request = new RptProductionManualCostingInputDetailSubRepirtRequest();
        printReport(response, "ProductionManualCosting_InputDetailSubRepirt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-manual-costing-input-detail-sub-repirt", method = RequestMethod.GET)
    public void printProductionManualCostingInputDetailSubRepirtGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionManualCostingInputDetailSubRepirt(response, objectMapper.convertValue(query, RptProductionManualCostingInputDetailSubRepirtRequest.class));
    }

    /**
     * Template: RptInvProductionJobOrderSlipA.rpt
     * Procedure: Sp_InvProductionJobOrder_GetAllMethod
     * Desktop: InvProductionJobOrder.GetAll
     */
    @RequestMapping(value = "/reports/print/inv-production-job-order-slip-a", method = RequestMethod.POST)
    public void printInvProductionJobOrderSlipA(HttpServletResponse response, @RequestBody(required = false) RptInvProductionJobOrderSlipARequest request) throws Exception {
        if (request == null) request = new RptInvProductionJobOrderSlipARequest();
        printReport(response, "RptInvProductionJobOrderSlipA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-production-job-order-slip-a", method = RequestMethod.GET)
    public void printInvProductionJobOrderSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvProductionJobOrderSlipA(response, objectMapper.convertValue(query, RptInvProductionJobOrderSlipARequest.class));
    }

    /**
     * Template: usp_InvProductionJobOrderLabStandardDetail_SubReport.rpt
     * Procedure: usp_InvProductionJobOrderLabStandardDetail_SubReport
     * Desktop: InvFoodProductionReports.InvProductionJobOrderLabStandardDetail_SubReport
     */
    @RequestMapping(value = "/reports/print/usp-inv-production-job-order-lab-standard-detail-sub-report", method = RequestMethod.POST)
    public void printUspInvProductionJobOrderLabStandardDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptUspInvProductionJobOrderLabStandardDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptUspInvProductionJobOrderLabStandardDetailSubReportRequest();
        printReport(response, "usp_InvProductionJobOrderLabStandardDetail_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/usp-inv-production-job-order-lab-standard-detail-sub-report", method = RequestMethod.GET)
    public void printUspInvProductionJobOrderLabStandardDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printUspInvProductionJobOrderLabStandardDetailSubReport(response, objectMapper.convertValue(query, RptUspInvProductionJobOrderLabStandardDetailSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
