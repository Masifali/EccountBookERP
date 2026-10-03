package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.ContractorWagesPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** ContractorWages print actions. Generated from the verified seeder contracts. */
@Controller
public class ContractorWagesPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 002_01_ContractorWagesPartyProcessingSlip.rpt
     * Procedure: Sp_InvContractorWagesBillHeader_SlipandRegister
     * Desktop: InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/002-01-contractor-wages-party-processing-slip", method = RequestMethod.POST)
    public void print00201ContractorWagesPartyProcessingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt00201ContractorWagesPartyProcessingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt00201ContractorWagesPartyProcessingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "002_01_ContractorWagesPartyProcessingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/002-01-contractor-wages-party-processing-slip", method = RequestMethod.GET)
    public void print00201ContractorWagesPartyProcessingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print00201ContractorWagesPartyProcessingSlip(response, objectMapper.convertValue(query, Rpt00201ContractorWagesPartyProcessingSlipRequest.class));
    }

    /**
     * Template: 003-tSummaryWagesByRefDocumentsAndActivities.rpt
     * Procedure: USP_GetSummaryWagesByRefDocumentsAndActivities
     * Desktop: InvContractorWagesBillHeader.GetSummaryWagesByRefDocumentsAndActivities
     */
    @RequestMapping(value = "/reports/print/003-t-summary-wages-by-ref-documents-and-activities", method = RequestMethod.POST)
    public void print003TSummaryWagesByRefDocumentsAndActivities(HttpServletResponse response, @RequestBody(required = false) Rpt003TSummaryWagesByRefDocumentsAndActivitiesRequest request) throws Exception {
        if (request == null) request = new Rpt003TSummaryWagesByRefDocumentsAndActivitiesRequest();
        printReport(response, "003-tSummaryWagesByRefDocumentsAndActivities.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/003-t-summary-wages-by-ref-documents-and-activities", method = RequestMethod.GET)
    public void print003TSummaryWagesByRefDocumentsAndActivitiesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print003TSummaryWagesByRefDocumentsAndActivities(response, objectMapper.convertValue(query, Rpt003TSummaryWagesByRefDocumentsAndActivitiesRequest.class));
    }

    /**
     * Template: 003_A_WagesReportByContractor.rpt
     * Procedure: USP_GetSummaryWagesByRefDocumentsAndActivities
     * Desktop: InvContractorWagesBillHeader.GetSummaryWagesByRefDocumentsAndActivities
     */
    @RequestMapping(value = "/reports/print/003-a-wages-report-by-contractor", method = RequestMethod.POST)
    public void print003AWagesReportByContractor(HttpServletResponse response, @RequestBody(required = false) Rpt003AWagesReportByContractorRequest request) throws Exception {
        if (request == null) request = new Rpt003AWagesReportByContractorRequest();
        printReport(response, "003_A_WagesReportByContractor.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/003-a-wages-report-by-contractor", method = RequestMethod.GET)
    public void print003AWagesReportByContractorGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print003AWagesReportByContractor(response, objectMapper.convertValue(query, Rpt003AWagesReportByContractorRequest.class));
    }

    /**
     * Template: InvContractorWagesSubReport.rpt
     * Procedure: Sp_InvContractorWagesBillHeader_SlipandRegister
     * Desktop: InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/inv-contractor-wages-sub-report", method = RequestMethod.POST)
    public void printInvContractorWagesSubReport(HttpServletResponse response, @RequestBody(required = false) RptInvContractorWagesSubReportRequest request) throws Exception {
        if (request == null) request = new RptInvContractorWagesSubReportRequest();
        printReport(response, "InvContractorWagesSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-contractor-wages-sub-report", method = RequestMethod.GET)
    public void printInvContractorWagesSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvContractorWagesSubReport(response, objectMapper.convertValue(query, RptInvContractorWagesSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
