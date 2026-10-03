package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.SaltPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Salt print actions. Generated from the verified seeder contracts. */
@Controller
public class SaltPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1818-ProductionSalt-Slip.rpt
     * Procedure: USP_ProductionSaltSlipAndRegister
     * Desktop: ProductionHeader.ProductionSaltSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1818-production-salt-slip", method = RequestMethod.POST)
    public void print1818ProductionSaltSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1818ProductionSaltSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1818ProductionSaltSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1818-ProductionSalt-Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1818-production-salt-slip", method = RequestMethod.GET)
    public void print1818ProductionSaltSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1818ProductionSaltSlip(response, objectMapper.convertValue(query, Rpt1818ProductionSaltSlipRequest.class));
    }

    /**
     * Template: 1818-ProductionSalt_Register.rpt
     * Procedure: USP_ProductionSaltSlipAndRegister
     * Desktop: ProductionHeader.ProductionSaltSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1818-production-salt-register", method = RequestMethod.POST)
    public void print1818ProductionSaltRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1818ProductionSaltRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1818ProductionSaltRegisterRequest();
        printReport(response, "1818-ProductionSalt_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1818-production-salt-register", method = RequestMethod.GET)
    public void print1818ProductionSaltRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1818ProductionSaltRegister(response, objectMapper.convertValue(query, Rpt1818ProductionSaltRegisterRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
