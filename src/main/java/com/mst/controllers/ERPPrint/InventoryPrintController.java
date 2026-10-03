package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.InventoryPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import javax.servlet.http.HttpServletResponse;
import java.util.List;
import java.util.Map;
import com.mst.services.InventoryUomGridOutputService;
import org.springframework.http.*;

/** Inventory print actions. Existing print URLs are preserved. */
@Controller
public class InventoryPrintController extends ReportPrintSupport {
    @Autowired private InventoryUomGridOutputService uomGridService;

    @PostMapping("/api/inventory/uom-schedules/grid-print")
    public ResponseEntity<?> printItemUom(@RequestBody InventoryUomGridOutputService.Request request) {
        try {
            return ResponseEntity.ok().contentType(MediaType.APPLICATION_PDF)
                    .header("Content-Disposition", "inline; filename=Item-UOM-Schedule.pdf")
                    .body(uomGridService.print(request));
        } catch (AccessDeniedException exception) {
            return ResponseEntity.status(403).body(Map.of("message", exception.getMessage()));
        } catch (IllegalArgumentException exception) {
            return ResponseEntity.badRequest().body(Map.of("message", exception.getMessage()));
        } catch (IllegalStateException | org.springframework.dao.DataAccessException exception) {
            return ResponseEntity.status(409).body(Map.of("message", "History output could not be generated. Reload and retry."));
        }
    }


    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 326-GetItemsFromMinAndMaxRateSchedule.rpt
     * Procedure: usp_GetItemsFromMinAndMaxRateSchedule
     * Desktop: ItemMinMaxRateSchedule.GetItemsFromMinAndMaxRateSchedule
     */
    @RequestMapping(value = "/reports/print/326-get-items-from-min-and-max-rate-schedule", method = RequestMethod.POST)
    public void print326GetItemsFromMinAndMaxRateSchedule(HttpServletResponse response, @RequestBody(required = false) Rpt326GetItemsFromMinAndMaxRateScheduleRequest request) throws Exception {
        if (request == null) request = new Rpt326GetItemsFromMinAndMaxRateScheduleRequest();
        printReport(response, "326-GetItemsFromMinAndMaxRateSchedule.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/326-get-items-from-min-and-max-rate-schedule", method = RequestMethod.GET)
    public void print326GetItemsFromMinAndMaxRateScheduleGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print326GetItemsFromMinAndMaxRateSchedule(response, objectMapper.convertValue(query, Rpt326GetItemsFromMinAndMaxRateScheduleRequest.class));
    }

    /**
     * Template: 326_01_ItemsFromMinAndMaxRateSchedule_FormHistory.rpt
     * Procedure: usp_GetLastItemMinAndMaxRateByItemIdAndUomId
     * Desktop: ItemMinMaxRateSchedule.GetLastItemMinAndMaxRateByItemIdAndUomId
     */
    @RequestMapping(value = "/reports/print/326-01-items-from-min-and-max-rate-schedule-form-history", method = RequestMethod.POST)
    public void print32601ItemsFromMinAndMaxRateScheduleFormHistory(HttpServletResponse response, @RequestBody(required = false) Rpt32601ItemsFromMinAndMaxRateScheduleFormHistoryRequest request) throws Exception {
        if (request == null) request = new Rpt32601ItemsFromMinAndMaxRateScheduleFormHistoryRequest();
        printReport(response, "326_01_ItemsFromMinAndMaxRateSchedule_FormHistory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/326-01-items-from-min-and-max-rate-schedule-form-history", method = RequestMethod.GET)
    public void print32601ItemsFromMinAndMaxRateScheduleFormHistoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print32601ItemsFromMinAndMaxRateScheduleFormHistory(response, objectMapper.convertValue(query, Rpt32601ItemsFromMinAndMaxRateScheduleFormHistoryRequest.class));
    }

    /**
     * Template: 417-InventoryStockTransactionsReport.rpt
     * Procedure: [pcc].[USP-EvaluationStockTransactionsReport]
     * Desktop: InventoryStockEvalautionDetail.EvaluationStockTransactionsReport
     */
    @RequestMapping(value = "/reports/print/417-inventory-stock-transactions-report", method = RequestMethod.POST)
    public void print417InventoryStockTransactionsReport(HttpServletResponse response, @RequestBody(required = false) Rpt417InventoryStockTransactionsReportRequest request) throws Exception {
        if (request == null) request = new Rpt417InventoryStockTransactionsReportRequest();
        printReport(response, "417-InventoryStockTransactionsReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/417-inventory-stock-transactions-report", method = RequestMethod.GET)
    public void print417InventoryStockTransactionsReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print417InventoryStockTransactionsReport(response, objectMapper.convertValue(query, Rpt417InventoryStockTransactionsReportRequest.class));
    }

    /**
     * Template: 200-InvRptItemsList.rpt
     * Procedure: SP_Item_List_Rpt
     * Desktop: Item.RptItemList
     */
    @RequestMapping(value = "/reports/print/200-items-list", method = RequestMethod.POST)
    public void print200ItemsList(HttpServletResponse response, @RequestBody(required = false) Rpt200ItemsListRequest request) throws Exception {
        if (request == null) request = new Rpt200ItemsListRequest();
        printReport(response, "200-InvRptItemsList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/200-items-list", method = RequestMethod.GET)
    public void print200ItemsListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print200ItemsList(response, objectMapper.convertValue(query, Rpt200ItemsListRequest.class));
    }

    /**
     * Template: 255-GatePassGeneralRpt.rpt
     * Procedure: Sp_GatePassGeneral_Register
     * Desktop: GatePassGeneralReports.GatePassGeneralRegister
     */
    @RequestMapping(value = "/reports/print/255-gate-pass-general", method = RequestMethod.POST)
    public void print255GatePassGeneral(HttpServletResponse response, @RequestBody(required = false) Rpt255GatePassGeneralRequest request) throws Exception {
        if (request == null) request = new Rpt255GatePassGeneralRequest();
        printReport(response, "255-GatePassGeneralRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/255-gate-pass-general", method = RequestMethod.GET)
    public void print255GatePassGeneralGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print255GatePassGeneral(response, objectMapper.convertValue(query, Rpt255GatePassGeneralRequest.class));
    }

    /**
     * Template: 274_1-PreBookinRegister.rpt
     * Procedure: USP_PreBookingOrderSlipAndRegister
     * Desktop: PreBookingOrder.PreBookingOrderSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/274-1-pre-bookin-register", method = RequestMethod.POST)
    public void print2741PreBookinRegister(HttpServletResponse response, @RequestBody(required = false) Rpt2741PreBookinRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt2741PreBookinRegisterRequest();
        printReport(response, "274_1-PreBookinRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/274-1-pre-bookin-register", method = RequestMethod.GET)
    public void print2741PreBookinRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print2741PreBookinRegister(response, objectMapper.convertValue(query, Rpt2741PreBookinRegisterRequest.class));
    }

    /**
     * Template: 292-InvRptSupplierRegister.rpt
     * Procedure: Sp_SupplierCustomerHistory_rpt
     * Desktop: GeneralReprots.SupplierCustomerRegister
     */
    @RequestMapping(value = "/reports/print/292-supplier-register", method = RequestMethod.POST)
    public void print292SupplierRegister(HttpServletResponse response, @RequestBody(required = false) Rpt292SupplierRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt292SupplierRegisterRequest();
        printReport(response, "292-InvRptSupplierRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/292-supplier-register", method = RequestMethod.GET)
    public void print292SupplierRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print292SupplierRegister(response, objectMapper.convertValue(query, Rpt292SupplierRegisterRequest.class));
    }

    /**
     * Template: 292_01-InvRptSupplierRegister.rpt
     * Procedure: Sp_SupplierCustomerHistory_rpt
     * Desktop: GeneralReprots.SupplierCustomerRegister
     */
    @RequestMapping(value = "/reports/print/292-01-supplier-register", method = RequestMethod.POST)
    public void print29201SupplierRegister(HttpServletResponse response, @RequestBody(required = false) Rpt29201SupplierRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt29201SupplierRegisterRequest();
        printReport(response, "292_01-InvRptSupplierRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/292-01-supplier-register", method = RequestMethod.GET)
    public void print29201SupplierRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print29201SupplierRegister(response, objectMapper.convertValue(query, Rpt29201SupplierRegisterRequest.class));
    }

    /**
     * Template: 293-InvRptSupplierSlip.rpt
     * Procedure: Sp_SupplierCustomerHistory_rpt
     * Desktop: GeneralReprots.SupplierCustomerRegister
     */
    @RequestMapping(value = "/reports/print/293-supplier-slip", method = RequestMethod.POST)
    public void print293SupplierSlip(HttpServletResponse response, @RequestBody(required = false) Rpt293SupplierSlipRequest request) throws Exception {
        if (request == null) request = new Rpt293SupplierSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "293-InvRptSupplierSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/293-supplier-slip", method = RequestMethod.GET)
    public void print293SupplierSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print293SupplierSlip(response, objectMapper.convertValue(query, Rpt293SupplierSlipRequest.class));
    }

    /**
     * Template: BarCodeReport.rpt
     * Procedure: Sp_Item_GetAllMethod
     * Desktop: Item.GetItemByItemTypeId
     */
    @RequestMapping(value = "/reports/print/bar-code-report", method = RequestMethod.POST)
    public void printBarCodeReport(HttpServletResponse response, @RequestBody(required = false) RptBarCodeReportRequest request) throws Exception {
        if (request == null) request = new RptBarCodeReportRequest();
        printReport(response, "BarCodeReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/bar-code-report", method = RequestMethod.GET)
    public void printBarCodeReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printBarCodeReport(response, objectMapper.convertValue(query, RptBarCodeReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
