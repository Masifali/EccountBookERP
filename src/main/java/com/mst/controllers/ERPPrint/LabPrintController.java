package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.LabPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import com.mst.repositories.ICompanyRepository;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import javax.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Lab print actions. Generated from the verified seeder contracts. */
@Controller
public class LabPrintController extends ReportPrintSupport {
    @Autowired private ICompanyRepository companies;

    /** Desktop Print_Click sends DS_Table00 and DS_Table01 from the displayed report together. */
    @PostMapping("/reports/lab/purchase-analysis-by-vehicle")
    public void printPurchaseAnalysisByVehicle(HttpServletResponse response,
            @RequestBody VehicleAnalysisPrintRequest request) throws Exception {
        currentUserContext.currentUserId();
        if (request.summary() == null || request.summary().isEmpty()) {
            response.setStatus(404);
            response.setContentType("text/plain;charset=UTF-8");
            response.getWriter().write("No Record Found For Display");
            return;
        }
        var company = companies.findById(currentUserContext.currentCompanyId()).orElse(null);
        Map<String, Object> parameters = new LinkedHashMap<>();
        parameters.put("CompanyName", company == null ? "" : Objects.toString(company.getCompName(), ""));
        parameters.put("CompanyAddress", company == null ? "" : Objects.toString(company.getCompAddress(), ""));
        printReportData(response, "665-LabDataVehicleWiseByParentIdRegister.rpt", Map.of(
                "rows", request.detail() == null ? List.of() : request.detail(),
                "reportParameters", parameters,
                "subReports", List.of(Map.of("template", "LabDataVehicleWiseSummary_Report.rpt", "rows", request.summary()))));
    }

    public record VehicleAnalysisPrintRequest(List<Map<String, Object>> detail, List<Map<String, Object>> summary) { }

    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 256-GatePassInward_WithDetailSlip.rpt
     * Procedure: Sp_GatePassInward_SlipAndRegister_WithDetail_Rpt
     * Desktop: GatePassInwardReports.GatePassInwardSlipandRegister_WithDetail_256
     */
    @RequestMapping(value = "/reports/print/256-gate-pass-inward-with-detail-slip", method = RequestMethod.POST)
    public void print256GatePassInwardWithDetailSlip(HttpServletResponse response, @RequestBody(required = false) Rpt256GatePassInwardWithDetailSlipRequest request) throws Exception {
        if (request == null) request = new Rpt256GatePassInwardWithDetailSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "256-GatePassInward_WithDetailSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/256-gate-pass-inward-with-detail-slip", method = RequestMethod.GET)
    public void print256GatePassInwardWithDetailSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print256GatePassInwardWithDetailSlip(response, objectMapper.convertValue(query, Rpt256GatePassInwardWithDetailSlipRequest.class));
    }

    /**
     * Template: 291_01_GatePassVehicleEntryAndExitTimeAnalysisReport.rpt
     * Procedure: USP_GatePass_VehicleEntryAndExitTime_AnalysisReport
     * Desktop: GatePassOutwardReports.GatePass_VehicleEntryAndExitTime_AnalysisReport
     */
    @RequestMapping(value = "/reports/print/291-01-gate-pass-vehicle-entry-and-exit-time-analysis-report", method = RequestMethod.POST)
    public void print29101GatePassVehicleEntryAndExitTimeAnalysisReport(HttpServletResponse response, @RequestBody(required = false) Rpt29101GatePassVehicleEntryAndExitTimeAnalysisReportRequest request) throws Exception {
        if (request == null) request = new Rpt29101GatePassVehicleEntryAndExitTimeAnalysisReportRequest();
        printReport(response, "291_01_GatePassVehicleEntryAndExitTimeAnalysisReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/291-01-gate-pass-vehicle-entry-and-exit-time-analysis-report", method = RequestMethod.GET)
    public void print29101GatePassVehicleEntryAndExitTimeAnalysisReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print29101GatePassVehicleEntryAndExitTimeAnalysisReport(response, objectMapper.convertValue(query, Rpt29101GatePassVehicleEntryAndExitTimeAnalysisReportRequest.class));
    }

    /**
     * Template: 657-RptInvLabSampleAnalysisSlipA.rpt
     * Procedure: Sp_InvLabSampleAnalysisHeader_RiceSlipAndRegister_Rpt
     * Desktop: InvLabSampleAnalysisHeader.GetPrintSlipAndReport
     */
    @RequestMapping(value = "/reports/print/657-inv-lab-sample-analysis-slip-a", method = RequestMethod.POST)
    public void print657InvLabSampleAnalysisSlipA(HttpServletResponse response, @RequestBody(required = false) Rpt657InvLabSampleAnalysisSlipARequest request) throws Exception {
        if (request == null) request = new Rpt657InvLabSampleAnalysisSlipARequest();
        printReport(response, "657-RptInvLabSampleAnalysisSlipA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/657-inv-lab-sample-analysis-slip-a", method = RequestMethod.GET)
    public void print657InvLabSampleAnalysisSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print657InvLabSampleAnalysisSlipA(response, objectMapper.convertValue(query, Rpt657InvLabSampleAnalysisSlipARequest.class));
    }

    /**
     * Template: 658-RptInvLabSaleAnalysisSlip.rpt
     * Procedure: SP_InvLabAnalysisSale_Slip_Rpt
     * Desktop: InvLabAnalysisSaleHeader.SaleAnalysisPrint
     */
    @RequestMapping(value = "/reports/print/658-inv-lab-sale-analysis-slip", method = RequestMethod.POST)
    public void print658InvLabSaleAnalysisSlip(HttpServletResponse response, @RequestBody(required = false) Rpt658InvLabSaleAnalysisSlipRequest request) throws Exception {
        if (request == null) request = new Rpt658InvLabSaleAnalysisSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "658-RptInvLabSaleAnalysisSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/658-inv-lab-sale-analysis-slip", method = RequestMethod.GET)
    public void print658InvLabSaleAnalysisSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print658InvLabSaleAnalysisSlip(response, objectMapper.convertValue(query, Rpt658InvLabSaleAnalysisSlipRequest.class));
    }

    /**
     * Template: 659-RptInvLabInProcessAnalysisSlip.rpt
     * Procedure: USp_InvLabAnalysisInProcessHeader_Slip
     * Desktop: InvLabAnalysisPurchaseHeader.InvLabAnalysisInProcessHeader_Slip659
     */
    @RequestMapping(value = "/reports/print/659-inv-lab-in-process-analysis-slip", method = RequestMethod.POST)
    public void print659InvLabInProcessAnalysisSlip(HttpServletResponse response, @RequestBody(required = false) Rpt659InvLabInProcessAnalysisSlipRequest request) throws Exception {
        if (request == null) request = new Rpt659InvLabInProcessAnalysisSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "659-RptInvLabInProcessAnalysisSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/659-inv-lab-in-process-analysis-slip", method = RequestMethod.GET)
    public void print659InvLabInProcessAnalysisSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print659InvLabInProcessAnalysisSlip(response, objectMapper.convertValue(query, Rpt659InvLabInProcessAnalysisSlipRequest.class));
    }

    /**
     * Template: 660-LabSInProcessAnalysisRegister.rpt
     * Procedure: USP_InProcessAnalysisRegister
     * Desktop: InvLabAnalysisInProcessHeader.InProcessStepAnalysisRegister
     */
    @RequestMapping(value = "/reports/print/660-lab-s-in-process-analysis-register", method = RequestMethod.POST)
    public void print660LabSInProcessAnalysisRegister(HttpServletResponse response, @RequestBody(required = false) Rpt660LabSInProcessAnalysisRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt660LabSInProcessAnalysisRegisterRequest();
        printReport(response, "660-LabSInProcessAnalysisRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/660-lab-s-in-process-analysis-register", method = RequestMethod.GET)
    public void print660LabSInProcessAnalysisRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print660LabSInProcessAnalysisRegister(response, objectMapper.convertValue(query, Rpt660LabSInProcessAnalysisRegisterRequest.class));
    }

    /**
     * Template: 661-LabPurchaseAnalysisRegitser.rpt
     * Procedure: USP_LabPurchaseAnalysis_Register
     * Desktop: InvLabAnalysisPurchaseHeader.LabPurchaseAnalysisRegister
     */
    @RequestMapping(value = "/reports/print/661-lab-purchase-analysis-regitser", method = RequestMethod.POST)
    public void print661LabPurchaseAnalysisRegitser(HttpServletResponse response, @RequestBody(required = false) Rpt661LabPurchaseAnalysisRegitserRequest request) throws Exception {
        if (request == null) request = new Rpt661LabPurchaseAnalysisRegitserRequest();
        printReport(response, "661-LabPurchaseAnalysisRegitser.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/661-lab-purchase-analysis-regitser", method = RequestMethod.GET)
    public void print661LabPurchaseAnalysisRegitserGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print661LabPurchaseAnalysisRegitser(response, objectMapper.convertValue(query, Rpt661LabPurchaseAnalysisRegitserRequest.class));
    }

    /**
     * Template: 662-LabSaleAnalysisRegitser.rpt
     * Procedure: [USP_LabSaleAnalysis_Register]
     * Desktop: InvLabAnalysisSaleHeader.LabSaleAnalysisRegister
     */
    @RequestMapping(value = "/reports/print/662-lab-sale-analysis-regitser", method = RequestMethod.POST)
    public void print662LabSaleAnalysisRegitser(HttpServletResponse response, @RequestBody(required = false) Rpt662LabSaleAnalysisRegitserRequest request) throws Exception {
        if (request == null) request = new Rpt662LabSaleAnalysisRegitserRequest();
        printReport(response, "662-LabSaleAnalysisRegitser.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/662-lab-sale-analysis-regitser", method = RequestMethod.GET)
    public void print662LabSaleAnalysisRegitserGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print662LabSaleAnalysisRegitser(response, objectMapper.convertValue(query, Rpt662LabSaleAnalysisRegitserRequest.class));
    }

    /**
     * Template: 664-LabSampleAnalysisRegister.rpt
     * Procedure: [USP_LabSampleAnalysis_Register]
     * Desktop: InvLabSampleAnalysisHeader.LabSampleAnalysisRegister
     */
    @RequestMapping(value = "/reports/print/664-lab-sample-analysis-register", method = RequestMethod.POST)
    public void print664LabSampleAnalysisRegister(HttpServletResponse response, @RequestBody(required = false) Rpt664LabSampleAnalysisRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt664LabSampleAnalysisRegisterRequest();
        printReport(response, "664-LabSampleAnalysisRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/664-lab-sample-analysis-register", method = RequestMethod.GET)
    public void print664LabSampleAnalysisRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print664LabSampleAnalysisRegister(response, objectMapper.convertValue(query, Rpt664LabSampleAnalysisRegisterRequest.class));
    }

    /**
     * Template: 665-LabDataVehicleWiseByParentIdRegister.rpt
     * Procedure: usp_getLabDataVehicleWiseByParentId
     * Desktop: InvLabAnalysisPurchaseHeader.LabDataVehicleWiseByParent
     */
    @RequestMapping(value = "/reports/print/665-lab-data-vehicle-wise-by-parent-id-register", method = RequestMethod.POST)
    public void print665LabDataVehicleWiseByParentIdRegister(HttpServletResponse response, @RequestBody(required = false) Rpt665LabDataVehicleWiseByParentIdRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt665LabDataVehicleWiseByParentIdRegisterRequest();
        printReport(response, "665-LabDataVehicleWiseByParentIdRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/665-lab-data-vehicle-wise-by-parent-id-register", method = RequestMethod.GET)
    public void print665LabDataVehicleWiseByParentIdRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print665LabDataVehicleWiseByParentIdRegister(response, objectMapper.convertValue(query, Rpt665LabDataVehicleWiseByParentIdRegisterRequest.class));
    }

    /**
     * Template: InvLabInProcessGroupAnalysisSub.rpt
     * Procedure: USp_InvLabAnalysisGroup_SubReport
     * Desktop: InvLabAnalysisPurchaseHeader.GetDataForLabInProcessGroupAnalysisSubReport
     */
    @RequestMapping(value = "/reports/print/inv-lab-in-process-group-analysis-sub", method = RequestMethod.POST)
    public void printInvLabInProcessGroupAnalysisSub(HttpServletResponse response, @RequestBody(required = false) RptInvLabInProcessGroupAnalysisSubRequest request) throws Exception {
        if (request == null) request = new RptInvLabInProcessGroupAnalysisSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvLabInProcessGroupAnalysisSub.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-lab-in-process-group-analysis-sub", method = RequestMethod.GET)
    public void printInvLabInProcessGroupAnalysisSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvLabInProcessGroupAnalysisSub(response, objectMapper.convertValue(query, RptInvLabInProcessGroupAnalysisSubRequest.class));
    }

    /**
     * Template: InvLabInProcessStepAnalysisSub.rpt
     * Procedure: USp_InvLabAnalysisStep_SubReport
     * Desktop: InvLabAnalysisPurchaseHeader.GetDataForLabAnalysisStep_SubReport
     */
    @RequestMapping(value = "/reports/print/inv-lab-in-process-step-analysis-sub", method = RequestMethod.POST)
    public void printInvLabInProcessStepAnalysisSub(HttpServletResponse response, @RequestBody(required = false) RptInvLabInProcessStepAnalysisSubRequest request) throws Exception {
        if (request == null) request = new RptInvLabInProcessStepAnalysisSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvLabInProcessStepAnalysisSub.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-lab-in-process-step-analysis-sub", method = RequestMethod.GET)
    public void printInvLabInProcessStepAnalysisSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvLabInProcessStepAnalysisSub(response, objectMapper.convertValue(query, RptInvLabInProcessStepAnalysisSubRequest.class));
    }

    /**
     * Template: InvLabPurchaseAnalysisSubForGP.rpt
     * Procedure: Sp_InvLabAnalysisPurchaseSlip_Rpt
     * Desktop: InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653
     */
    @RequestMapping(value = "/reports/print/inv-lab-purchase-analysis-sub-for-gp", method = RequestMethod.POST)
    public void printInvLabPurchaseAnalysisSubForGP(HttpServletResponse response, @RequestBody(required = false) RptInvLabPurchaseAnalysisSubForGPRequest request) throws Exception {
        if (request == null) request = new RptInvLabPurchaseAnalysisSubForGPRequest();
        printReport(response, "InvLabPurchaseAnalysisSubForGP.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-lab-purchase-analysis-sub-for-gp", method = RequestMethod.GET)
    public void printInvLabPurchaseAnalysisSubForGPGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvLabPurchaseAnalysisSubForGP(response, objectMapper.convertValue(query, RptInvLabPurchaseAnalysisSubForGPRequest.class));
    }

    /**
     * Template: LabAnalysisPurchaseByGPSubReport.rpt
     * Procedure: USP_InvLabAnalysisPurchaseByGP_SubReport
     * Desktop: InvLabAnalysisPurchaseHeader.LabAnalysisPurchaseByGP_SubReport
     */
    @RequestMapping(value = "/reports/print/lab-analysis-purchase-by-gp-sub-report", method = RequestMethod.POST)
    public void printLabAnalysisPurchaseByGPSubReport(HttpServletResponse response, @RequestBody(required = false) RptLabAnalysisPurchaseByGPSubReportRequest request) throws Exception {
        if (request == null) request = new RptLabAnalysisPurchaseByGPSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "LabAnalysisPurchaseByGPSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lab-analysis-purchase-by-gp-sub-report", method = RequestMethod.GET)
    public void printLabAnalysisPurchaseByGPSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLabAnalysisPurchaseByGPSubReport(response, objectMapper.convertValue(query, RptLabAnalysisPurchaseByGPSubReportRequest.class));
    }

    /**
     * Template: LabDataVehicleWiseSummary_Report.rpt
     * Procedure: usp_getLabDataVehicleWiseByParentId
     * Desktop: InvLabAnalysisPurchaseHeader.LabDataVehicleWiseByParent
     */
    @RequestMapping(value = "/reports/print/lab-data-vehicle-wise-summary-report", method = RequestMethod.POST)
    public void printLabDataVehicleWiseSummaryReport(HttpServletResponse response, @RequestBody(required = false) RptLabDataVehicleWiseSummaryReportRequest request) throws Exception {
        if (request == null) request = new RptLabDataVehicleWiseSummaryReportRequest();
        printReport(response, "LabDataVehicleWiseSummary_Report.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lab-data-vehicle-wise-summary-report", method = RequestMethod.GET)
    public void printLabDataVehicleWiseSummaryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLabDataVehicleWiseSummaryReport(response, objectMapper.convertValue(query, RptLabDataVehicleWiseSummaryReportRequest.class));
    }

    /**
     * Template: LabPurchaseAnalysisSubParameterReport.rpt
     * Procedure: Sp_InvLabAnalysisPurchaseHeader_GetAllMethod
     * Desktop: InvLabAnalysisPurchaseHeader.GetDataForLabPurchaseAnalysisSubReport
     */
    @RequestMapping(value = "/reports/print/lab-purchase-analysis-sub-parameter-report", method = RequestMethod.POST)
    public void printLabPurchaseAnalysisSubParameterReport(HttpServletResponse response, @RequestBody(required = false) RptLabPurchaseAnalysisSubParameterReportRequest request) throws Exception {
        if (request == null) request = new RptLabPurchaseAnalysisSubParameterReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "LabPurchaseAnalysisSubParameterReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lab-purchase-analysis-sub-parameter-report", method = RequestMethod.GET)
    public void printLabPurchaseAnalysisSubParameterReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLabPurchaseAnalysisSubParameterReport(response, objectMapper.convertValue(query, RptLabPurchaseAnalysisSubParameterReportRequest.class));
    }

    /**
     * Template: LabSampleAnalysisSubParameterReport.rpt
     * Procedure: Sp_InvLabSampleAnalysisHeader_GetAllMethod
     * Desktop: InvLabSampleAnalysisHeader.GetDataForLabSampleAnalysisSubReport
     */
    @RequestMapping(value = "/reports/print/lab-sample-analysis-sub-parameter-report", method = RequestMethod.POST)
    public void printLabSampleAnalysisSubParameterReport(HttpServletResponse response, @RequestBody(required = false) RptLabSampleAnalysisSubParameterReportRequest request) throws Exception {
        if (request == null) request = new RptLabSampleAnalysisSubParameterReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "LabSampleAnalysisSubParameterReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lab-sample-analysis-sub-parameter-report", method = RequestMethod.GET)
    public void printLabSampleAnalysisSubParameterReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLabSampleAnalysisSubParameterReport(response, objectMapper.convertValue(query, RptLabSampleAnalysisSubParameterReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
