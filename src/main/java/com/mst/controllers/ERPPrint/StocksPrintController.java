package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.StocksPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Stocks print actions. Generated from the verified seeder contracts. */
@Controller
public class StocksPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 416-InvStockOpeningBalanceHeader_Register.rpt
     * Procedure: Sp_InvStockOpeningBalanceHeader_RegisterRpt
     * Desktop: GeneralReprots.StockopeningBalanceRegister
     */
    @RequestMapping(value = "/reports/print/416-inv-stock-opening-balance-header-register", method = RequestMethod.POST)
    public void print416InvStockOpeningBalanceHeaderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt416InvStockOpeningBalanceHeaderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt416InvStockOpeningBalanceHeaderRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "416-InvStockOpeningBalanceHeader_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/416-inv-stock-opening-balance-header-register", method = RequestMethod.GET)
    public void print416InvStockOpeningBalanceHeaderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print416InvStockOpeningBalanceHeaderRegister(response, objectMapper.convertValue(query, Rpt416InvStockOpeningBalanceHeaderRegisterRequest.class));
    }

    /**
     * Template: 202_ItemStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Store
     * Desktop: StocksReport.ItemStockReportWithValues_Store
     */
    @RequestMapping(value = "/reports/print/202-item-stock-summary", method = RequestMethod.POST)
    public void print202ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt202ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt202ItemStockSummaryRequest();
        printReport(response, "202_ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/202-item-stock-summary", method = RequestMethod.GET)
    public void print202ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print202ItemStockSummary(response, objectMapper.convertValue(query, Rpt202ItemStockSummaryRequest.class));
    }

    /**
     * Template: 202_02_ItemandWarehouseStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Store
     * Desktop: StocksReport.ItemStockReportWithValues_Store
     */
    @RequestMapping(value = "/reports/print/202-02-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print20202ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt20202ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt20202ItemandWarehouseStockSummaryRequest();
        printReport(response, "202_02_ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/202-02-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print20202ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print20202ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt20202ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 202_03_ItemandPackSizeStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Store
     * Desktop: StocksReport.ItemStockReportWithValues_Store
     */
    @RequestMapping(value = "/reports/print/202-03-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print20203ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt20203ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt20203ItemandPackSizeStockSummaryRequest();
        printReport(response, "202_03_ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/202-03-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print20203ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print20203ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt20203ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 621-StockEvalautionDetailTransactionVehiclesWise.rpt
     * Procedure: USP_GetStockEvalautionDetailByRefRefIds
     * Desktop: InventoryStockEvalautionDetail.GetStockEvalautionDetailByRefIds
     */
    @RequestMapping(value = "/reports/print/621-stock-evalaution-detail-transaction-vehicles-wise", method = RequestMethod.POST)
    public void print621StockEvalautionDetailTransactionVehiclesWise(HttpServletResponse response, @RequestBody(required = false) Rpt621StockEvalautionDetailTransactionVehiclesWiseRequest request) throws Exception {
        if (request == null) request = new Rpt621StockEvalautionDetailTransactionVehiclesWiseRequest();
        printReport(response, "621-StockEvalautionDetailTransactionVehiclesWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/621-stock-evalaution-detail-transaction-vehicles-wise", method = RequestMethod.GET)
    public void print621StockEvalautionDetailTransactionVehiclesWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print621StockEvalautionDetailTransactionVehiclesWise(response, objectMapper.convertValue(query, Rpt621StockEvalautionDetailTransactionVehiclesWiseRequest.class));
    }

    /**
     * Template: 1560-StockRptInventoryTransactionsNew.rpt
     * Procedure: USP_InventoryEvaluationItemLedger_Rpt
     * Desktop: InventoryStockEvalautionDetail.InventoryTransactionReportNew
     */
    @RequestMapping(value = "/reports/print/1560-stock-inventory-transactions-new", method = RequestMethod.POST)
    public void print1560StockInventoryTransactionsNew(HttpServletResponse response, @RequestBody(required = false) Rpt1560StockInventoryTransactionsNewRequest request) throws Exception {
        if (request == null) request = new Rpt1560StockInventoryTransactionsNewRequest();
        printReport(response, "1560-StockRptInventoryTransactionsNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1560-stock-inventory-transactions-new", method = RequestMethod.GET)
    public void print1560StockInventoryTransactionsNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1560StockInventoryTransactionsNew(response, objectMapper.convertValue(query, Rpt1560StockInventoryTransactionsNewRequest.class));
    }

    /**
     * Template: 1561-InvStockRptInventoryTransactionsNew.rpt
     * Procedure: USP_InventoryEvaluationItemLedger_Rpt
     * Desktop: InventoryStockEvalautionDetail.InventoryTransactionReportNew
     */
    @RequestMapping(value = "/reports/print/1561-inv-stock-inventory-transactions-new", method = RequestMethod.POST)
    public void print1561InvStockInventoryTransactionsNew(HttpServletResponse response, @RequestBody(required = false) Rpt1561InvStockInventoryTransactionsNewRequest request) throws Exception {
        if (request == null) request = new Rpt1561InvStockInventoryTransactionsNewRequest();
        printReport(response, "1561-InvStockRptInventoryTransactionsNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1561-inv-stock-inventory-transactions-new", method = RequestMethod.GET)
    public void print1561InvStockInventoryTransactionsNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1561InvStockInventoryTransactionsNew(response, objectMapper.convertValue(query, Rpt1561InvStockInventoryTransactionsNewRequest.class));
    }

    /**
     * Template: 160-WagesRegister.rpt
     * Procedure: USp_WagesRegister
     * Desktop: InventoryStockEvalautionDetail.WagesRegister
     */
    @RequestMapping(value = "/reports/print/160-wages-register", method = RequestMethod.POST)
    public void print160WagesRegister(HttpServletResponse response, @RequestBody(required = false) Rpt160WagesRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt160WagesRegisterRequest();
        printReport(response, "160-WagesRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/160-wages-register", method = RequestMethod.GET)
    public void print160WagesRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160WagesRegister(response, objectMapper.convertValue(query, Rpt160WagesRegisterRequest.class));
    }

    /**
     * Template: 161-WagesbyContractor&DocumentType.rpt
     * Procedure: USp_WagesRegister
     * Desktop: InventoryStockEvalautionDetail.WagesRegister
     */
    @RequestMapping(value = "/reports/print/161-wagesby-contractor-document-type", method = RequestMethod.POST)
    public void print161WagesbyContractorDocumentType(HttpServletResponse response, @RequestBody(required = false) Rpt161WagesbyContractorDocumentTypeRequest request) throws Exception {
        if (request == null) request = new Rpt161WagesbyContractorDocumentTypeRequest();
        printReport(response, "161-WagesbyContractor&DocumentType.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/161-wagesby-contractor-document-type", method = RequestMethod.GET)
    public void print161WagesbyContractorDocumentTypeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print161WagesbyContractorDocumentType(response, objectMapper.convertValue(query, Rpt161WagesbyContractorDocumentTypeRequest.class));
    }

    /**
     * Template: 162-WagesByContractor.rpt
     * Procedure: USp_WagesRegister
     * Desktop: InventoryStockEvalautionDetail.WagesRegister
     */
    @RequestMapping(value = "/reports/print/162-wages-by-contractor", method = RequestMethod.POST)
    public void print162WagesByContractor(HttpServletResponse response, @RequestBody(required = false) Rpt162WagesByContractorRequest request) throws Exception {
        if (request == null) request = new Rpt162WagesByContractorRequest();
        printReport(response, "162-WagesByContractor.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/162-wages-by-contractor", method = RequestMethod.GET)
    public void print162WagesByContractorGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print162WagesByContractor(response, objectMapper.convertValue(query, Rpt162WagesByContractorRequest.class));
    }

    /**
     * Template: 163-WagesByDocumentType.rpt
     * Procedure: USp_WagesRegister
     * Desktop: InventoryStockEvalautionDetail.WagesRegister
     */
    @RequestMapping(value = "/reports/print/163-wages-by-document-type", method = RequestMethod.POST)
    public void print163WagesByDocumentType(HttpServletResponse response, @RequestBody(required = false) Rpt163WagesByDocumentTypeRequest request) throws Exception {
        if (request == null) request = new Rpt163WagesByDocumentTypeRequest();
        printReport(response, "163-WagesByDocumentType.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/163-wages-by-document-type", method = RequestMethod.GET)
    public void print163WagesByDocumentTypeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print163WagesByDocumentType(response, objectMapper.convertValue(query, Rpt163WagesByDocumentTypeRequest.class));
    }

    /**
     * Template: 165-ItemLedgerRetail.rpt
     * Procedure: SpInventoryStockEvalautionDetail_RetailItemLedgerReport
     * Desktop: StocksReport.InventoryStockEvalautionDetail_RetailItemLedgerReport
     */
    @RequestMapping(value = "/reports/print/165-item-ledger-retail", method = RequestMethod.POST)
    public void print165ItemLedgerRetail(HttpServletResponse response, @RequestBody(required = false) Rpt165ItemLedgerRetailRequest request) throws Exception {
        if (request == null) request = new Rpt165ItemLedgerRetailRequest();
        printReport(response, "165-ItemLedgerRetail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/165-item-ledger-retail", method = RequestMethod.GET)
    public void print165ItemLedgerRetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print165ItemLedgerRetail(response, objectMapper.convertValue(query, Rpt165ItemLedgerRetailRequest.class));
    }

    /**
     * Template: 178-ItemStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/178-item-stock-summary", method = RequestMethod.POST)
    public void print178ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt178ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt178ItemStockSummaryRequest();
        printReport(response, "178-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/178-item-stock-summary", method = RequestMethod.GET)
    public void print178ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print178ItemStockSummary(response, objectMapper.convertValue(query, Rpt178ItemStockSummaryRequest.class));
    }

    /**
     * Template: 179-ItemandWarehouseStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/179-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print179ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt179ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt179ItemandWarehouseStockSummaryRequest();
        printReport(response, "179-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/179-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print179ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print179ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt179ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 180-ItemandWarehouseStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/180-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print180ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt180ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt180ItemandWarehouseStockSummaryRequest();
        printReport(response, "180-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/180-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print180ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt180ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 181-ItemandCropYearStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/181-itemand-crop-year-stock-summary", method = RequestMethod.POST)
    public void print181ItemandCropYearStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt181ItemandCropYearStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt181ItemandCropYearStockSummaryRequest();
        printReport(response, "181-ItemandCropYearStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/181-itemand-crop-year-stock-summary", method = RequestMethod.GET)
    public void print181ItemandCropYearStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print181ItemandCropYearStockSummary(response, objectMapper.convertValue(query, Rpt181ItemandCropYearStockSummaryRequest.class));
    }

    /**
     * Template: 182-ItemandCropYearandWarehouseStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/182-itemand-crop-yearand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print182ItemandCropYearandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt182ItemandCropYearandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt182ItemandCropYearandWarehouseStockSummaryRequest();
        printReport(response, "182-ItemandCropYearandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/182-itemand-crop-yearand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print182ItemandCropYearandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print182ItemandCropYearandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt182ItemandCropYearandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 183-JobLotandItemStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/183-job-lotand-item-stock-summary", method = RequestMethod.POST)
    public void print183JobLotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt183JobLotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt183JobLotandItemStockSummaryRequest();
        printReport(response, "183-JobLotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/183-job-lotand-item-stock-summary", method = RequestMethod.GET)
    public void print183JobLotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print183JobLotandItemStockSummary(response, objectMapper.convertValue(query, Rpt183JobLotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 184-WarehouseandJoblotandItemStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/184-warehouseand-joblotand-item-stock-summary", method = RequestMethod.POST)
    public void print184WarehouseandJoblotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt184WarehouseandJoblotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt184WarehouseandJoblotandItemStockSummaryRequest();
        printReport(response, "184-WarehouseandJoblotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/184-warehouseand-joblotand-item-stock-summary", method = RequestMethod.GET)
    public void print184WarehouseandJoblotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print184WarehouseandJoblotandItemStockSummary(response, objectMapper.convertValue(query, Rpt184WarehouseandJoblotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 185-ItemandPackSizeStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/185-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print185ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt185ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt185ItemandPackSizeStockSummaryRequest();
        printReport(response, "185-ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/185-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print185ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt185ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 186-ItemandPackingTypeStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/186-itemand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print186ItemandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt186ItemandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt186ItemandPackingTypeStockSummaryRequest();
        printReport(response, "186-ItemandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/186-itemand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print186ItemandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186ItemandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt186ItemandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 187-ItemandPackSizeandPackingTypeStockSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/187-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print187ItemandPackSizeandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt187ItemandPackSizeandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt187ItemandPackSizeandPackingTypeStockSummaryRequest();
        printReport(response, "187-ItemandPackSizeandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/187-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print187ItemandPackSizeandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print187ItemandPackSizeandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt187ItemandPackSizeandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 188-ItemStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/188-item-stock-summary", method = RequestMethod.POST)
    public void print188ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt188ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt188ItemStockSummaryRequest();
        printReport(response, "188-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/188-item-stock-summary", method = RequestMethod.GET)
    public void print188ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print188ItemStockSummary(response, objectMapper.convertValue(query, Rpt188ItemStockSummaryRequest.class));
    }

    /**
     * Template: 188_01-ItemStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/188-01-item-stock-summary", method = RequestMethod.POST)
    public void print18801ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt18801ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt18801ItemStockSummaryRequest();
        printReport(response, "188_01-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/188-01-item-stock-summary", method = RequestMethod.GET)
    public void print18801ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print18801ItemStockSummary(response, objectMapper.convertValue(query, Rpt18801ItemStockSummaryRequest.class));
    }

    /**
     * Template: 188_02-ItemStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/188-02-item-stock-summary", method = RequestMethod.POST)
    public void print18802ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt18802ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt18802ItemStockSummaryRequest();
        printReport(response, "188_02-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/188-02-item-stock-summary", method = RequestMethod.GET)
    public void print18802ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print18802ItemStockSummary(response, objectMapper.convertValue(query, Rpt18802ItemStockSummaryRequest.class));
    }

    /**
     * Template: 189-ItemandWarehouseStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/189-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print189ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt189ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt189ItemandWarehouseStockSummaryRequest();
        printReport(response, "189-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/189-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print189ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print189ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt189ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 190-ItemandWarehouseStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/190-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print190ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt190ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt190ItemandWarehouseStockSummaryRequest();
        printReport(response, "190-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/190-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print190ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print190ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt190ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 191-ItemandCropYearStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/191-itemand-crop-year-stock-summary", method = RequestMethod.POST)
    public void print191ItemandCropYearStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt191ItemandCropYearStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt191ItemandCropYearStockSummaryRequest();
        printReport(response, "191-ItemandCropYearStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/191-itemand-crop-year-stock-summary", method = RequestMethod.GET)
    public void print191ItemandCropYearStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print191ItemandCropYearStockSummary(response, objectMapper.convertValue(query, Rpt191ItemandCropYearStockSummaryRequest.class));
    }

    /**
     * Template: 192-ItemandCropYearandWarehouseStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/192-itemand-crop-yearand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print192ItemandCropYearandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt192ItemandCropYearandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt192ItemandCropYearandWarehouseStockSummaryRequest();
        printReport(response, "192-ItemandCropYearandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/192-itemand-crop-yearand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print192ItemandCropYearandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print192ItemandCropYearandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt192ItemandCropYearandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 193-JobLotandItemStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/193-job-lotand-item-stock-summary", method = RequestMethod.POST)
    public void print193JobLotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt193JobLotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt193JobLotandItemStockSummaryRequest();
        printReport(response, "193-JobLotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/193-job-lotand-item-stock-summary", method = RequestMethod.GET)
    public void print193JobLotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print193JobLotandItemStockSummary(response, objectMapper.convertValue(query, Rpt193JobLotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 194-WarehouseandJoblotandItemStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/194-warehouseand-joblotand-item-stock-summary", method = RequestMethod.POST)
    public void print194WarehouseandJoblotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt194WarehouseandJoblotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt194WarehouseandJoblotandItemStockSummaryRequest();
        printReport(response, "194-WarehouseandJoblotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/194-warehouseand-joblotand-item-stock-summary", method = RequestMethod.GET)
    public void print194WarehouseandJoblotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print194WarehouseandJoblotandItemStockSummary(response, objectMapper.convertValue(query, Rpt194WarehouseandJoblotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 194A-WarehouseandItemandJoblotStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/194a-warehouseand-itemand-joblot-stock-summary", method = RequestMethod.POST)
    public void print194AWarehouseandItemandJoblotStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt194AWarehouseandItemandJoblotStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt194AWarehouseandItemandJoblotStockSummaryRequest();
        printReport(response, "194A-WarehouseandItemandJoblotStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/194a-warehouseand-itemand-joblot-stock-summary", method = RequestMethod.GET)
    public void print194AWarehouseandItemandJoblotStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print194AWarehouseandItemandJoblotStockSummary(response, objectMapper.convertValue(query, Rpt194AWarehouseandItemandJoblotStockSummaryRequest.class));
    }

    /**
     * Template: 195-ItemandPackSizeStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/195-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print195ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt195ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt195ItemandPackSizeStockSummaryRequest();
        printReport(response, "195-ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/195-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print195ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print195ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt195ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 196-ItemandPackingTypeStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/196-itemand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print196ItemandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt196ItemandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt196ItemandPackingTypeStockSummaryRequest();
        printReport(response, "196-ItemandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/196-itemand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print196ItemandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print196ItemandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt196ItemandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 197-ItemandPackSizeandPackingTypeStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Rpt
     * Desktop: StocksReport.stockReportWithValues
     */
    @RequestMapping(value = "/reports/print/197-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print197ItemandPackSizeandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt197ItemandPackSizeandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt197ItemandPackSizeandPackingTypeStockSummaryRequest();
        printReport(response, "197-ItemandPackSizeandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/197-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print197ItemandPackSizeandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print197ItemandPackSizeandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt197ItemandPackSizeandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 198-WIPStockPlantWiseSummary.rpt
     * Procedure: Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.stockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/198-wip-stock-plant-wise-summary", method = RequestMethod.POST)
    public void print198WIPStockPlantWiseSummary(HttpServletResponse response, @RequestBody(required = false) Rpt198WIPStockPlantWiseSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt198WIPStockPlantWiseSummaryRequest();
        printReport(response, "198-WIPStockPlantWiseSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/198-wip-stock-plant-wise-summary", method = RequestMethod.GET)
    public void print198WIPStockPlantWiseSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print198WIPStockPlantWiseSummary(response, objectMapper.convertValue(query, Rpt198WIPStockPlantWiseSummaryRequest.class));
    }

    /**
     * Template: 225_StockReservedRegister.rpt
     * Procedure: [dbo].[USP_InventoryStockReserved_SlipAndRegister]
     * Desktop: InventoryStockReserved.InventoryStockReserved_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/225-stock-reserved-register", method = RequestMethod.POST)
    public void print225StockReservedRegister(HttpServletResponse response, @RequestBody(required = false) Rpt225StockReservedRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt225StockReservedRegisterRequest();
        printReport(response, "225_StockReservedRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/225-stock-reserved-register", method = RequestMethod.GET)
    public void print225StockReservedRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print225StockReservedRegister(response, objectMapper.convertValue(query, Rpt225StockReservedRegisterRequest.class));
    }

    /**
     * Template: 287-CurrentStockReportWithAvgRates.rpt
     * Procedure: SP_GetCurrentStockReportWithAvgRates
     * Desktop: StocksReport.GetCurrentStockReportWithAvgRates
     */
    @RequestMapping(value = "/reports/print/287-current-stock-report-with-avg-rates", method = RequestMethod.POST)
    public void print287CurrentStockReportWithAvgRates(HttpServletResponse response, @RequestBody(required = false) Rpt287CurrentStockReportWithAvgRatesRequest request) throws Exception {
        if (request == null) request = new Rpt287CurrentStockReportWithAvgRatesRequest();
        printReport(response, "287-CurrentStockReportWithAvgRates.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/287-current-stock-report-with-avg-rates", method = RequestMethod.GET)
    public void print287CurrentStockReportWithAvgRatesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print287CurrentStockReportWithAvgRates(response, objectMapper.convertValue(query, Rpt287CurrentStockReportWithAvgRatesRequest.class));
    }

    /**
     * Template: 300-StockMonthlyClosingSlip.rpt
     * Procedure: usp_MonthlyClosingStock_Slip
     * Desktop: StockMonthlyClosingHeader.MonthlyClosingStock_Slip
     */
    @RequestMapping(value = "/reports/print/300-stock-monthly-closing-slip", method = RequestMethod.POST)
    public void print300StockMonthlyClosingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt300StockMonthlyClosingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt300StockMonthlyClosingSlipRequest();
        printReport(response, "300-StockMonthlyClosingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/300-stock-monthly-closing-slip", method = RequestMethod.GET)
    public void print300StockMonthlyClosingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print300StockMonthlyClosingSlip(response, objectMapper.convertValue(query, Rpt300StockMonthlyClosingSlipRequest.class));
    }

    /**
     * Template: 390_01_GetStockByFifo.rpt
     * Procedure: USP_GetStockByFifo_Report
     * Desktop: StocksReport.StockByFiFo
     */
    @RequestMapping(value = "/reports/print/390-01-get-stock-by-fifo", method = RequestMethod.POST)
    public void print39001GetStockByFifo(HttpServletResponse response, @RequestBody(required = false) Rpt39001GetStockByFifoRequest request) throws Exception {
        if (request == null) request = new Rpt39001GetStockByFifoRequest();
        printReport(response, "390_01_GetStockByFifo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/390-01-get-stock-by-fifo", method = RequestMethod.GET)
    public void print39001GetStockByFifoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print39001GetStockByFifo(response, objectMapper.convertValue(query, Rpt39001GetStockByFifoRequest.class));
    }

    /**
     * Template: 390_GetFIFODataForAudit.rpt
     * Procedure: USP_GetFIFODataForAudit
     * Desktop: InventoryStockEvalautionDetail.GetFIFODataForAudit
     */
    @RequestMapping(value = "/reports/print/390-get-fifo-data-for-audit", method = RequestMethod.POST)
    public void print390GetFIFODataForAudit(HttpServletResponse response, @RequestBody(required = false) Rpt390GetFIFODataForAuditRequest request) throws Exception {
        if (request == null) request = new Rpt390GetFIFODataForAuditRequest();
        printReport(response, "390_GetFIFODataForAudit.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/390-get-fifo-data-for-audit", method = RequestMethod.GET)
    public void print390GetFIFODataForAuditGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print390GetFIFODataForAudit(response, objectMapper.convertValue(query, Rpt390GetFIFODataForAuditRequest.class));
    }

    /**
     * Template: 401-StockEvalautionDetail_GenerateStocks_Register.rpt
     * Procedure: Sp_InventoryStockEvalautionDetail_GenerateStocks
     * Desktop: InventoryStockEvalautionDetail.InventoryStockGenrate
     */
    @RequestMapping(value = "/reports/print/401-stock-evalaution-detail-generate-stocks-register", method = RequestMethod.POST)
    public void print401StockEvalautionDetailGenerateStocksRegister(HttpServletResponse response, @RequestBody(required = false) Rpt401StockEvalautionDetailGenerateStocksRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt401StockEvalautionDetailGenerateStocksRegisterRequest();
        printReport(response, "401-StockEvalautionDetail_GenerateStocks_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/401-stock-evalaution-detail-generate-stocks-register", method = RequestMethod.GET)
    public void print401StockEvalautionDetailGenerateStocksRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print401StockEvalautionDetailGenerateStocksRegister(response, objectMapper.convertValue(query, Rpt401StockEvalautionDetailGenerateStocksRegisterRequest.class));
    }

    /**
     * Template: 403-InvStockRptInventoryTransactionsStocks.rpt
     * Procedure: SpInventoryTransactions_History_Report
     * Desktop: InventoryStockEvalautionDetail.TransactionStockGenrate
     */
    @RequestMapping(value = "/reports/print/403-inv-stock-inventory-transactions-stocks", method = RequestMethod.POST)
    public void print403InvStockInventoryTransactionsStocks(HttpServletResponse response, @RequestBody(required = false) Rpt403InvStockInventoryTransactionsStocksRequest request) throws Exception {
        if (request == null) request = new Rpt403InvStockInventoryTransactionsStocksRequest();
        printReport(response, "403-InvStockRptInventoryTransactionsStocks.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/403-inv-stock-inventory-transactions-stocks", method = RequestMethod.GET)
    public void print403InvStockInventoryTransactionsStocksGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print403InvStockInventoryTransactionsStocks(response, objectMapper.convertValue(query, Rpt403InvStockInventoryTransactionsStocksRequest.class));
    }

    /**
     * Template: 404-RptEBGLBySupplierandItem.rpt
     * Procedure: Sp_InventoryEBGLBySupplierandItem_Rpt
     * Desktop: StocksReport.GetStockWithSupplier
     */
    @RequestMapping(value = "/reports/print/404-ebgl-by-supplierand-item", method = RequestMethod.POST)
    public void print404EBGLBySupplierandItem(HttpServletResponse response, @RequestBody(required = false) Rpt404EBGLBySupplierandItemRequest request) throws Exception {
        if (request == null) request = new Rpt404EBGLBySupplierandItemRequest();
        printReport(response, "404-RptEBGLBySupplierandItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/404-ebgl-by-supplierand-item", method = RequestMethod.GET)
    public void print404EBGLBySupplierandItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print404EBGLBySupplierandItem(response, objectMapper.convertValue(query, Rpt404EBGLBySupplierandItemRequest.class));
    }

    /**
     * Template: 408-StockTransferRegister.rpt
     * Procedure: Sp_InvStockTransfer_SlipandRegister
     * Desktop: InvStockTransferHeader.StockTransferSlipandRegister
     */
    @RequestMapping(value = "/reports/print/408-stock-transfer-register", method = RequestMethod.POST)
    public void print408StockTransferRegister(HttpServletResponse response, @RequestBody(required = false) Rpt408StockTransferRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt408StockTransferRegisterRequest();
        printReport(response, "408-StockTransferRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/408-stock-transfer-register", method = RequestMethod.GET)
    public void print408StockTransferRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print408StockTransferRegister(response, objectMapper.convertValue(query, Rpt408StockTransferRegisterRequest.class));
    }

    /**
     * Template: 410-InvStockAdjustmentRegister.rpt
     * Procedure: Sp_StockAdjustmentSlipAndRegister
     * Desktop: InvStockAdjustment.StockAdjustmentSlipAndRegister409
     */
    @RequestMapping(value = "/reports/print/410-inv-stock-adjustment-register", method = RequestMethod.POST)
    public void print410InvStockAdjustmentRegister(HttpServletResponse response, @RequestBody(required = false) Rpt410InvStockAdjustmentRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt410InvStockAdjustmentRegisterRequest();
        printReport(response, "410-InvStockAdjustmentRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/410-inv-stock-adjustment-register", method = RequestMethod.GET)
    public void print410InvStockAdjustmentRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print410InvStockAdjustmentRegister(response, objectMapper.convertValue(query, Rpt410InvStockAdjustmentRegisterRequest.class));
    }

    /**
     * Template: 411-InvStockRptInventoryTransactionsNew.rpt
     * Procedure: USP_InventoryEvaluationItemLedger_Rpt
     * Desktop: InventoryStockEvalautionDetail.InventoryTransactionReportNew
     */
    @RequestMapping(value = "/reports/print/411-inv-stock-inventory-transactions-new", method = RequestMethod.POST)
    public void print411InvStockInventoryTransactionsNew(HttpServletResponse response, @RequestBody(required = false) Rpt411InvStockInventoryTransactionsNewRequest request) throws Exception {
        if (request == null) request = new Rpt411InvStockInventoryTransactionsNewRequest();
        printReport(response, "411-InvStockRptInventoryTransactionsNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/411-inv-stock-inventory-transactions-new", method = RequestMethod.GET)
    public void print411InvStockInventoryTransactionsNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print411InvStockInventoryTransactionsNew(response, objectMapper.convertValue(query, Rpt411InvStockInventoryTransactionsNewRequest.class));
    }

    /**
     * Template: 412-StockRptInventoryTransactionsNew.rpt
     * Procedure: USP_InventoryEvaluationItemLedger_Rpt
     * Desktop: InventoryStockEvalautionDetail.InventoryTransactionReportNew
     */
    @RequestMapping(value = "/reports/print/412-stock-inventory-transactions-new", method = RequestMethod.POST)
    public void print412StockInventoryTransactionsNew(HttpServletResponse response, @RequestBody(required = false) Rpt412StockInventoryTransactionsNewRequest request) throws Exception {
        if (request == null) request = new Rpt412StockInventoryTransactionsNewRequest();
        printReport(response, "412-StockRptInventoryTransactionsNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/412-stock-inventory-transactions-new", method = RequestMethod.GET)
    public void print412StockInventoryTransactionsNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print412StockInventoryTransactionsNew(response, objectMapper.convertValue(query, Rpt412StockInventoryTransactionsNewRequest.class));
    }

    /**
     * Template: 416_01_StockOpeningBalanceHeader_Register.rpt
     * Procedure: Sp_InvStockOpeningBalanceHeader_RegisterRpt
     * Desktop: GeneralReprots.StockopeningBalanceRegister
     */
    @RequestMapping(value = "/reports/print/416-01-stock-opening-balance-header-register", method = RequestMethod.POST)
    public void print41601StockOpeningBalanceHeaderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt41601StockOpeningBalanceHeaderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt41601StockOpeningBalanceHeaderRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "416_01_StockOpeningBalanceHeader_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/416-01-stock-opening-balance-header-register", method = RequestMethod.GET)
    public void print41601StockOpeningBalanceHeaderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print41601StockOpeningBalanceHeaderRegister(response, objectMapper.convertValue(query, Rpt41601StockOpeningBalanceHeaderRegisterRequest.class));
    }

    /**
     * Template: 421_01_ItemEvaluationLedger.rpt
     * Procedure: usp_ItemLedgerFromStockEvaluations
     * Desktop: InventoryStockEvalautionDetail.ItemEvaluationLedgerNew
     */
    @RequestMapping(value = "/reports/print/421-01-item-evaluation-ledger", method = RequestMethod.POST)
    public void print42101ItemEvaluationLedger(HttpServletResponse response, @RequestBody(required = false) Rpt42101ItemEvaluationLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt42101ItemEvaluationLedgerRequest();
        printReport(response, "421_01_ItemEvaluationLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/421-01-item-evaluation-ledger", method = RequestMethod.GET)
    public void print42101ItemEvaluationLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print42101ItemEvaluationLedger(response, objectMapper.convertValue(query, Rpt42101ItemEvaluationLedgerRequest.class));
    }

    /**
     * Template: 421_02_ItemLedgerWithoutValue.rpt
     * Procedure: usp_ItemLedgerFromStockTransactions
     * Desktop: InventoryStockEvalautionDetail.ItemLedgerFromStockTransactions
     */
    @RequestMapping(value = "/reports/print/421-02-item-ledger-without-value", method = RequestMethod.POST)
    public void print42102ItemLedgerWithoutValue(HttpServletResponse response, @RequestBody(required = false) Rpt42102ItemLedgerWithoutValueRequest request) throws Exception {
        if (request == null) request = new Rpt42102ItemLedgerWithoutValueRequest();
        printReport(response, "421_02_ItemLedgerWithoutValue.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/421-02-item-ledger-without-value", method = RequestMethod.GET)
    public void print42102ItemLedgerWithoutValueGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print42102ItemLedgerWithoutValue(response, objectMapper.convertValue(query, Rpt42102ItemLedgerWithoutValueRequest.class));
    }

    /**
     * Template: 622-StockEvalaution_VehicleWiseTransaction.rpt
     * Procedure: USP_StockEvalaution_VehicleWiseTransaction_Report
     * Desktop: InventoryStockEvalautionDetail.StockEvalaution_VehicleWiseTransaction_Report
     */
    @RequestMapping(value = "/reports/print/622-stock-evalaution-vehicle-wise-transaction", method = RequestMethod.POST)
    public void print622StockEvalautionVehicleWiseTransaction(HttpServletResponse response, @RequestBody(required = false) Rpt622StockEvalautionVehicleWiseTransactionRequest request) throws Exception {
        if (request == null) request = new Rpt622StockEvalautionVehicleWiseTransactionRequest();
        printReport(response, "622-StockEvalaution_VehicleWiseTransaction.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/622-stock-evalaution-vehicle-wise-transaction", method = RequestMethod.GET)
    public void print622StockEvalautionVehicleWiseTransactionGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print622StockEvalautionVehicleWiseTransaction(response, objectMapper.convertValue(query, Rpt622StockEvalautionVehicleWiseTransactionRequest.class));
    }

    /**
     * Template: 622A_StockEvalaution_VehicleWiseTransaction.rpt
     * Procedure: USP_StockEvalaution_VehicleWiseTransaction_Report
     * Desktop: InventoryStockEvalautionDetail.StockEvalaution_VehicleWiseTransaction_Report
     */
    @RequestMapping(value = "/reports/print/622a-stock-evalaution-vehicle-wise-transaction", method = RequestMethod.POST)
    public void print622AStockEvalautionVehicleWiseTransaction(HttpServletResponse response, @RequestBody(required = false) Rpt622AStockEvalautionVehicleWiseTransactionRequest request) throws Exception {
        if (request == null) request = new Rpt622AStockEvalautionVehicleWiseTransactionRequest();
        printReport(response, "622A_StockEvalaution_VehicleWiseTransaction.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/622a-stock-evalaution-vehicle-wise-transaction", method = RequestMethod.GET)
    public void print622AStockEvalautionVehicleWiseTransactionGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print622AStockEvalautionVehicleWiseTransaction(response, objectMapper.convertValue(query, Rpt622AStockEvalautionVehicleWiseTransactionRequest.class));
    }

    /**
     * Template: 842-StockBreakUpInOutSummary.rpt
     * Procedure: usp_TotalStockBreakup_InAndOut_Summary
     * Desktop: InventoryStockEvalautionDetail.TotalStockBreakup_InAndOut_Summary
     */
    @RequestMapping(value = "/reports/print/842-stock-break-up-in-out-summary", method = RequestMethod.POST)
    public void print842StockBreakUpInOutSummary(HttpServletResponse response, @RequestBody(required = false) Rpt842StockBreakUpInOutSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt842StockBreakUpInOutSummaryRequest();
        printReport(response, "842-StockBreakUpInOutSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/842-stock-break-up-in-out-summary", method = RequestMethod.GET)
    public void print842StockBreakUpInOutSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print842StockBreakUpInOutSummary(response, objectMapper.convertValue(query, Rpt842StockBreakUpInOutSummaryRequest.class));
    }

    /**
     * Template: 843-PaddyStockBreakUpItemWise.rpt
     * Procedure: usp_TotalStockBreakup_InAndOut_Summary
     * Desktop: InventoryStockEvalautionDetail.TotalStockBreakup_InAndOut_Summary
     */
    @RequestMapping(value = "/reports/print/843-paddy-stock-break-up-item-wise", method = RequestMethod.POST)
    public void print843PaddyStockBreakUpItemWise(HttpServletResponse response, @RequestBody(required = false) Rpt843PaddyStockBreakUpItemWiseRequest request) throws Exception {
        if (request == null) request = new Rpt843PaddyStockBreakUpItemWiseRequest();
        printReport(response, "843-PaddyStockBreakUpItemWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/843-paddy-stock-break-up-item-wise", method = RequestMethod.GET)
    public void print843PaddyStockBreakUpItemWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print843PaddyStockBreakUpItemWise(response, objectMapper.convertValue(query, Rpt843PaddyStockBreakUpItemWiseRequest.class));
    }

    /**
     * Template: 844-StockClosingAndOpeningData.rpt
     * Procedure: usp_getStockClosingAndOpeningData
     * Desktop: InventoryStockEvalautionDetail.StockClosingAndOpeningData
     */
    @RequestMapping(value = "/reports/print/844-stock-closing-and-opening-data", method = RequestMethod.POST)
    public void print844StockClosingAndOpeningData(HttpServletResponse response, @RequestBody(required = false) Rpt844StockClosingAndOpeningDataRequest request) throws Exception {
        if (request == null) request = new Rpt844StockClosingAndOpeningDataRequest();
        printReport(response, "844-StockClosingAndOpeningData.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/844-stock-closing-and-opening-data", method = RequestMethod.GET)
    public void print844StockClosingAndOpeningDataGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print844StockClosingAndOpeningData(response, objectMapper.convertValue(query, Rpt844StockClosingAndOpeningDataRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
