package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.PackingMaterialPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import javax.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import java.util.Map;
import org.springframework.http.*;

/** PackingMaterial print actions. Existing print URLs are preserved. */
@Controller
public class PackingMaterialPrintController extends ReportPrintSupport {
    @Autowired private com.mst.services.PackingMaterialReportsService packingReportsService;

    @PostMapping("/api/packing-material/reports/{report}/print") @ResponseBody
    public ResponseEntity<?> printPackingMaterial(@PathVariable("report") String report,@RequestBody com.mst.models.PackingReportFilter filter,@RequestParam(name="format",defaultValue="main") String format) {
        try {
            return ResponseEntity.ok().header("Content-Type","application/pdf").header("Content-Disposition","inline; filename="+report+".pdf").body(packingReportsService.print(report,filter,format));
        } catch (AccessDeniedException exception) {
            return ResponseEntity.status(403).body(Map.of("message", exception.getMessage()));
        } catch (IllegalArgumentException exception) {
            return ResponseEntity.badRequest().body(Map.of("message", exception.getMessage()));
        } catch (Exception exception) {
            return ResponseEntity.status(500).body(Map.of("message", exception.getMessage() == null ? "The report request failed" : exception.getMessage()));
        }
    }


    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 214-GRNPackingMaterialSlip.rpt
     * Procedure: Sp_InvGrn_StoreSlip_Rpt
     * Desktop: InvGrnandGdnReports.GrnSlipStore
     */
    @RequestMapping(value = "/reports/print/214-grn-packing-material-slip", method = RequestMethod.POST)
    public void print214GRNPackingMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt214GRNPackingMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt214GRNPackingMaterialSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "214-GRNPackingMaterialSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/214-grn-packing-material-slip", method = RequestMethod.GET)
    public void print214GRNPackingMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print214GRNPackingMaterialSlip(response, objectMapper.convertValue(query, Rpt214GRNPackingMaterialSlipRequest.class));
    }

    /**
     * Template: 214_01_GrnPackingMaterialSlip.rpt
     * Procedure: Sp_InvGrn_StoreSlip_Rpt
     * Desktop: InvGrnandGdnReports.GrnSlipStore
     */
    @RequestMapping(value = "/reports/print/214-01-grn-packing-material-slip", method = RequestMethod.POST)
    public void print21401GrnPackingMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt21401GrnPackingMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt21401GrnPackingMaterialSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "214_01_GrnPackingMaterialSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/214-01-grn-packing-material-slip", method = RequestMethod.GET)
    public void print21401GrnPackingMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print21401GrnPackingMaterialSlip(response, objectMapper.convertValue(query, Rpt21401GrnPackingMaterialSlipRequest.class));
    }

    /**
     * Template: 231-InvRptPurchaseBillPackingMaterialSlip.rpt
     * Procedure: SP_CommisionAgentBillOtherExpense_SubRpt
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegisterSupReprt
     */
    @RequestMapping(value = "/reports/print/231-purchase-bill-packing-material-slip", method = RequestMethod.POST)
    public void print231PurchaseBillPackingMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt231PurchaseBillPackingMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt231PurchaseBillPackingMaterialSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "231-InvRptPurchaseBillPackingMaterialSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/231-purchase-bill-packing-material-slip", method = RequestMethod.GET)
    public void print231PurchaseBillPackingMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print231PurchaseBillPackingMaterialSlip(response, objectMapper.convertValue(query, Rpt231PurchaseBillPackingMaterialSlipRequest.class));
    }

    /**
     * Template: 231_01_PurchaseBillPmSlipWithGrnDetail.rpt
     * Procedure: USp_InvPurchaseInvoice_PackingMaterialBill_Rpt
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoice_PM231
     */
    @RequestMapping(value = "/reports/print/231-01-purchase-bill-pm-slip-with-grn-detail", method = RequestMethod.POST)
    public void print23101PurchaseBillPmSlipWithGrnDetail(HttpServletResponse response, @RequestBody(required = false) Rpt23101PurchaseBillPmSlipWithGrnDetailRequest request) throws Exception {
        if (request == null) request = new Rpt23101PurchaseBillPmSlipWithGrnDetailRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "231_01_PurchaseBillPmSlipWithGrnDetail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/231-01-purchase-bill-pm-slip-with-grn-detail", method = RequestMethod.GET)
    public void print23101PurchaseBillPmSlipWithGrnDetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print23101PurchaseBillPmSlipWithGrnDetail(response, objectMapper.convertValue(query, Rpt23101PurchaseBillPmSlipWithGrnDetailRequest.class));
    }

    /**
     * Template: 294-InvRptSaleBillDirectWithoutSO.rpt
     * Procedure: SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep
     * Desktop: InvSaleInvoiceReports.SalesCustomerBillSubReport
     */
    @RequestMapping(value = "/reports/print/294-sale-bill-direct-without-so", method = RequestMethod.POST)
    public void print294SaleBillDirectWithoutSO(HttpServletResponse response, @RequestBody(required = false) Rpt294SaleBillDirectWithoutSORequest request) throws Exception {
        if (request == null) request = new Rpt294SaleBillDirectWithoutSORequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "294-InvRptSaleBillDirectWithoutSO.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/294-sale-bill-direct-without-so", method = RequestMethod.GET)
    public void print294SaleBillDirectWithoutSOGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print294SaleBillDirectWithoutSO(response, objectMapper.convertValue(query, Rpt294SaleBillDirectWithoutSORequest.class));
    }

    /**
     * Template: 215-ExportReturnGrnSlip.rpt
     * Procedure: [dbo].[USp_ExportReturnGrn_SlipAndRegister]
     * Desktop: ExportReturnGrn.ExportReturnGrnSlipandRegister
     */
    @RequestMapping(value = "/reports/print/215-export-return-grn-slip", method = RequestMethod.POST)
    public void print215ExportReturnGrnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt215ExportReturnGrnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt215ExportReturnGrnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "215-ExportReturnGrnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/215-export-return-grn-slip", method = RequestMethod.GET)
    public void print215ExportReturnGrnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print215ExportReturnGrnSlip(response, objectMapper.convertValue(query, Rpt215ExportReturnGrnSlipRequest.class));
    }

    /**
     * Template: 341-GrnPackingMaterialRegister.rpt
     * Procedure: Sp_InvGrn_StoreSlip_Rpt
     * Desktop: InvGrnandGdnReports.GrnSlipStore
     */
    @RequestMapping(value = "/reports/print/341-grn-packing-material-register", method = RequestMethod.POST)
    public void print341GrnPackingMaterialRegister(HttpServletResponse response, @RequestBody(required = false) Rpt341GrnPackingMaterialRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt341GrnPackingMaterialRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "341-GrnPackingMaterialRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/341-grn-packing-material-register", method = RequestMethod.GET)
    public void print341GrnPackingMaterialRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print341GrnPackingMaterialRegister(response, objectMapper.convertValue(query, Rpt341GrnPackingMaterialRegisterRequest.class));
    }

    /**
     * Template: 402-InvStockRptInventoryTransactionsA.rpt
     * Procedure: Sp_InventoryTransactions_GenerateTransactionsLedgerStocks
     * Desktop: InventoryStockEvalautionDetail.InventoryTransactions_GenerateTransactionsLedgerStocks
     */
    @RequestMapping(value = "/reports/print/402-inv-stock-inventory-transactions-a", method = RequestMethod.POST)
    public void print402InvStockInventoryTransactionsA(HttpServletResponse response, @RequestBody(required = false) Rpt402InvStockInventoryTransactionsARequest request) throws Exception {
        if (request == null) request = new Rpt402InvStockInventoryTransactionsARequest();
        printReport(response, "402-InvStockRptInventoryTransactionsA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/402-inv-stock-inventory-transactions-a", method = RequestMethod.GET)
    public void print402InvStockInventoryTransactionsAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print402InvStockInventoryTransactionsA(response, objectMapper.convertValue(query, Rpt402InvStockInventoryTransactionsARequest.class));
    }

    /**
     * Template: 357-PurchaseOrderRegister_PM.rpt
     * Procedure: Sp_PurchaseOrder_PackingMaterial_Rpt
     * Desktop: PurchaseOrder.PurchaseOrderRegister_PM
     */
    @RequestMapping(value = "/reports/print/357-purchase-order-register-pm", method = RequestMethod.POST)
    public void print357PurchaseOrderRegisterPM(HttpServletResponse response, @RequestBody(required = false) Rpt357PurchaseOrderRegisterPMRequest request) throws Exception {
        if (request == null) request = new Rpt357PurchaseOrderRegisterPMRequest();
        printReport(response, "357-PurchaseOrderRegister_PM.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/357-purchase-order-register-pm", method = RequestMethod.GET)
    public void print357PurchaseOrderRegisterPMGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print357PurchaseOrderRegisterPM(response, objectMapper.convertValue(query, Rpt357PurchaseOrderRegisterPMRequest.class));
    }

    /**
     * Template: 357_01-PurchaseOrderRegister_PM.rpt
     * Procedure: Sp_PurchaseOrder_PackingMaterial_Rpt
     * Desktop: PurchaseOrder.PurchaseOrderRegister_PM
     */
    @RequestMapping(value = "/reports/print/357-01-purchase-order-register-pm", method = RequestMethod.POST)
    public void print35701PurchaseOrderRegisterPM(HttpServletResponse response, @RequestBody(required = false) Rpt35701PurchaseOrderRegisterPMRequest request) throws Exception {
        if (request == null) request = new Rpt35701PurchaseOrderRegisterPMRequest();
        printReport(response, "357_01-PurchaseOrderRegister_PM.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/357-01-purchase-order-register-pm", method = RequestMethod.GET)
    public void print35701PurchaseOrderRegisterPMGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print35701PurchaseOrderRegisterPM(response, objectMapper.convertValue(query, Rpt35701PurchaseOrderRegisterPMRequest.class));
    }

    /**
     * Template: 215-PurchaseOrderPackingMaterialSlip.rpt
     * Procedure: Sp_PurchaseOrder_GeneralOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport201
     */
    @RequestMapping(value = "/reports/print/215-purchase-order-packing-material-slip", method = RequestMethod.POST)
    public void print215PurchaseOrderPackingMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt215PurchaseOrderPackingMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt215PurchaseOrderPackingMaterialSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "215-PurchaseOrderPackingMaterialSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/215-purchase-order-packing-material-slip", method = RequestMethod.GET)
    public void print215PurchaseOrderPackingMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print215PurchaseOrderPackingMaterialSlip(response, objectMapper.convertValue(query, Rpt215PurchaseOrderPackingMaterialSlipRequest.class));
    }

    /**
     * Template: 215_StockAdjustmentPMSlip.rpt
     * Procedure: Sp_StockAdjustmentSlipAndRegister
     * Desktop: InvStockAdjustment.StockAdjustmentSlipAndRegister409
     */
    @RequestMapping(value = "/reports/print/215-stock-adjustment-pm-slip", method = RequestMethod.POST)
    public void print215StockAdjustmentPMSlip(HttpServletResponse response, @RequestBody(required = false) Rpt215StockAdjustmentPMSlipRequest request) throws Exception {
        if (request == null) request = new Rpt215StockAdjustmentPMSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "215_StockAdjustmentPMSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/215-stock-adjustment-pm-slip", method = RequestMethod.GET)
    public void print215StockAdjustmentPMSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print215StockAdjustmentPMSlip(response, objectMapper.convertValue(query, Rpt215StockAdjustmentPMSlipRequest.class));
    }

    /**
     * Template: 415-InvStockTransferPackingMaterialAndStore_SlipandRegister.rpt
     * Procedure: Sp_InvStockTransferPackingMaterialAndStore_SlipandRegister
     * Desktop: InvStockTransferHeader.StockTransferPackingMaterialAndStore_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/415-inv-stock-transfer-packing-material-and-store-slipand-register", method = RequestMethod.POST)
    public void print415InvStockTransferPackingMaterialAndStoreSlipandRegister(HttpServletResponse response, @RequestBody(required = false) Rpt415InvStockTransferPackingMaterialAndStoreSlipandRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt415InvStockTransferPackingMaterialAndStoreSlipandRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "415-InvStockTransferPackingMaterialAndStore_SlipandRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/415-inv-stock-transfer-packing-material-and-store-slipand-register", method = RequestMethod.GET)
    public void print415InvStockTransferPackingMaterialAndStoreSlipandRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print415InvStockTransferPackingMaterialAndStoreSlipandRegister(response, objectMapper.convertValue(query, Rpt415InvStockTransferPackingMaterialAndStoreSlipandRegisterRequest.class));
    }

    /**
     * Template: 419-PartyToPartyPackingMaterialSlip.rpt
     * Procedure: USP_PmStockWithPartiesTransfer_SlipAndRegister
     * Desktop: InvPmStockWithPartiesTransferHeader.P2PPM419Slip
     */
    @RequestMapping(value = "/reports/print/419-party-to-party-packing-material-slip", method = RequestMethod.POST)
    public void print419PartyToPartyPackingMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt419PartyToPartyPackingMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt419PartyToPartyPackingMaterialSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "419-PartyToPartyPackingMaterialSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/419-party-to-party-packing-material-slip", method = RequestMethod.GET)
    public void print419PartyToPartyPackingMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print419PartyToPartyPackingMaterialSlip(response, objectMapper.convertValue(query, Rpt419PartyToPartyPackingMaterialSlipRequest.class));
    }

    /**
     * Template: 665-InvStockConversionPackingMaterial-Summery.rpt
     * Procedure: USP-InvStockConversionPackingMaterial_Summery_Rpt
     * Desktop: InvFoodProductionReports.StockConversionPackingMaterial_Summery_Rpt
     */
    @RequestMapping(value = "/reports/print/665-inv-stock-conversion-packing-material-summery", method = RequestMethod.POST)
    public void print665InvStockConversionPackingMaterialSummery(HttpServletResponse response, @RequestBody(required = false) Rpt665InvStockConversionPackingMaterialSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt665InvStockConversionPackingMaterialSummeryRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "665-InvStockConversionPackingMaterial-Summery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/665-inv-stock-conversion-packing-material-summery", method = RequestMethod.GET)
    public void print665InvStockConversionPackingMaterialSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print665InvStockConversionPackingMaterialSummery(response, objectMapper.convertValue(query, Rpt665InvStockConversionPackingMaterialSummeryRequest.class));
    }

    /**
     * Template: 671_01_PackingMaterialRequirementPlanning_Detail.rpt
     * Procedure: usp_PackingMaterialRequirementPlanning
     * Desktop: StocksReport.PackingMaterialRequirementPlanning
     */
    @RequestMapping(value = "/reports/print/671-01-packing-material-requirement-planning-detail", method = RequestMethod.POST)
    public void print67101PackingMaterialRequirementPlanningDetail(HttpServletResponse response, @RequestBody(required = false) Rpt67101PackingMaterialRequirementPlanningDetailRequest request) throws Exception {
        if (request == null) request = new Rpt67101PackingMaterialRequirementPlanningDetailRequest();
        printReport(response, "671_01_PackingMaterialRequirementPlanning_Detail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/671-01-packing-material-requirement-planning-detail", method = RequestMethod.GET)
    public void print67101PackingMaterialRequirementPlanningDetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print67101PackingMaterialRequirementPlanningDetail(response, objectMapper.convertValue(query, Rpt67101PackingMaterialRequirementPlanningDetailRequest.class));
    }

    /**
     * Template: 671_PackingMaterialRequirementPlanning.rpt
     * Procedure: usp_PackingMaterialRequirementPlanning
     * Desktop: StocksReport.PackingMaterialRequirementPlanning
     */
    @RequestMapping(value = "/reports/print/671-packing-material-requirement-planning", method = RequestMethod.POST)
    public void print671PackingMaterialRequirementPlanning(HttpServletResponse response, @RequestBody(required = false) Rpt671PackingMaterialRequirementPlanningRequest request) throws Exception {
        if (request == null) request = new Rpt671PackingMaterialRequirementPlanningRequest();
        printReport(response, "671_PackingMaterialRequirementPlanning.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/671-packing-material-requirement-planning", method = RequestMethod.GET)
    public void print671PackingMaterialRequirementPlanningGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print671PackingMaterialRequirementPlanning(response, objectMapper.convertValue(query, Rpt671PackingMaterialRequirementPlanningRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
