package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.TaxPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Tax print actions. Generated from the verified seeder contracts. */
@Controller
public class TaxPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 389-GetPreInvoicesAndForwardingDateForCommercialInvoices.rpt
     * Procedure: USP_GetPreInvoicesAndForwardingDateForCommercialInvoices
     * Desktop: ExImInvoice.GetPreInvoicesAndForwardingDateForCommercialInvoices
     */
    @RequestMapping(value = "/reports/print/389-get-pre-invoices-and-forwarding-date-for-commercial-invoices", method = RequestMethod.POST)
    public void print389GetPreInvoicesAndForwardingDateForCommercialInvoices(HttpServletResponse response, @RequestBody(required = false) Rpt389GetPreInvoicesAndForwardingDateForCommercialInvoicesRequest request) throws Exception {
        if (request == null) request = new Rpt389GetPreInvoicesAndForwardingDateForCommercialInvoicesRequest();
        printReport(response, "389-GetPreInvoicesAndForwardingDateForCommercialInvoices.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/389-get-pre-invoices-and-forwarding-date-for-commercial-invoices", method = RequestMethod.GET)
    public void print389GetPreInvoicesAndForwardingDateForCommercialInvoicesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print389GetPreInvoicesAndForwardingDateForCommercialInvoices(response, objectMapper.convertValue(query, Rpt389GetPreInvoicesAndForwardingDateForCommercialInvoicesRequest.class));
    }

    /**
     * Template: 546-GetProformaDataForInvoices.rpt
     * Procedure: USP_GetProformaDataForInvoices
     * Desktop: ExImInvoice.GetProformaDataForInvoices
     */
    @RequestMapping(value = "/reports/print/546-get-proforma-data-for-invoices", method = RequestMethod.POST)
    public void print546GetProformaDataForInvoices(HttpServletResponse response, @RequestBody(required = false) Rpt546GetProformaDataForInvoicesRequest request) throws Exception {
        if (request == null) request = new Rpt546GetProformaDataForInvoicesRequest();
        printReport(response, "546-GetProformaDataForInvoices.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/546-get-proforma-data-for-invoices", method = RequestMethod.GET)
    public void print546GetProformaDataForInvoicesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print546GetProformaDataForInvoices(response, objectMapper.convertValue(query, Rpt546GetProformaDataForInvoicesRequest.class));
    }

    /**
     * Template: 568-CommissionAgentFcyLedger.rpt
     * Procedure: usp_CommissionAgentFcyLedger
     * Desktop: ExImInvoice.CommissionAgentFcyLedger
     */
    @RequestMapping(value = "/reports/print/568-commission-agent-fcy-ledger", method = RequestMethod.POST)
    public void print568CommissionAgentFcyLedger(HttpServletResponse response, @RequestBody(required = false) Rpt568CommissionAgentFcyLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt568CommissionAgentFcyLedgerRequest();
        printReport(response, "568-CommissionAgentFcyLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/568-commission-agent-fcy-ledger", method = RequestMethod.GET)
    public void print568CommissionAgentFcyLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print568CommissionAgentFcyLedger(response, objectMapper.convertValue(query, Rpt568CommissionAgentFcyLedgerRequest.class));
    }

    /**
     * Template: 569-CommercialInvoice_Shipments.rpt
     * Procedure: usp_CommercialInvoice_Shipments
     * Desktop: ExImInvoice.CommercialInvoiceShipments
     */
    @RequestMapping(value = "/reports/print/569-commercial-invoice-shipments", method = RequestMethod.POST)
    public void print569CommercialInvoiceShipments(HttpServletResponse response, @RequestBody(required = false) Rpt569CommercialInvoiceShipmentsRequest request) throws Exception {
        if (request == null) request = new Rpt569CommercialInvoiceShipmentsRequest();
        printReport(response, "569-CommercialInvoice_Shipments.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/569-commercial-invoice-shipments", method = RequestMethod.GET)
    public void print569CommercialInvoiceShipmentsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print569CommercialInvoiceShipments(response, objectMapper.convertValue(query, Rpt569CommercialInvoiceShipmentsRequest.class));
    }

    /**
     * Template: 570-ShipmentdataForBrokeryTax.rpt
     * Procedure: usp_ShipmentdataForBrokeryTax
     * Desktop: ExImInvoice.ShipmentdataForBrokeryTax
     */
    @RequestMapping(value = "/reports/print/570-shipmentdata-for-brokery-tax", method = RequestMethod.POST)
    public void print570ShipmentdataForBrokeryTax(HttpServletResponse response, @RequestBody(required = false) Rpt570ShipmentdataForBrokeryTaxRequest request) throws Exception {
        if (request == null) request = new Rpt570ShipmentdataForBrokeryTaxRequest();
        printReport(response, "570-ShipmentdataForBrokeryTax.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/570-shipmentdata-for-brokery-tax", method = RequestMethod.GET)
    public void print570ShipmentdataForBrokeryTaxGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print570ShipmentdataForBrokeryTax(response, objectMapper.convertValue(query, Rpt570ShipmentdataForBrokeryTaxRequest.class));
    }

    /**
     * Template: 618_01_DoWeightWbWeightDiff.rpt
     * Procedure: SpEximInvoice_DoWeightWbWeightDiff_Rpt
     * Desktop: ExImInvoice.ExportSalesAuditByWeight
     */
    @RequestMapping(value = "/reports/print/618-01-do-weight-wb-weight-diff", method = RequestMethod.POST)
    public void print61801DoWeightWbWeightDiff(HttpServletResponse response, @RequestBody(required = false) Rpt61801DoWeightWbWeightDiffRequest request) throws Exception {
        if (request == null) request = new Rpt61801DoWeightWbWeightDiffRequest();
        printReport(response, "618_01_DoWeightWbWeightDiff.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/618-01-do-weight-wb-weight-diff", method = RequestMethod.GET)
    public void print61801DoWeightWbWeightDiffGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print61801DoWeightWbWeightDiff(response, objectMapper.convertValue(query, Rpt61801DoWeightWbWeightDiffRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
