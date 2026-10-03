package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.PosPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;
import org.springframework.beans.factory.annotation.Autowired;
import java.util.List;

/** Pos print actions. Generated from the verified seeder contracts. */
@Controller
public class PosPrintController extends ReportPrintSupport {
    @Autowired private com.mst.services.InventoryPosItemService posItemService;

    /** Uses the same authorized Code-128 bars as the POS item preview. */
    @PostMapping("/reports/print/item-barcode")
    public void printItemBarcode(HttpServletResponse response, @RequestBody Map<String, String> request) throws Exception {
        Map<String, Object> barcode = posItemService.barcode(request.get("code"));
        List<?> bars = (List<?>) barcode.get("bars");
        float width = 20;
        for (Object bar : bars) width += ((Number) bar).floatValue();
        java.io.ByteArrayOutputStream output = new java.io.ByteArrayOutputStream();
        com.itextpdf.text.Document document = new com.itextpdf.text.Document(new com.itextpdf.text.Rectangle(width, 70), 0, 0, 0, 0);
        com.itextpdf.text.pdf.PdfWriter writer = com.itextpdf.text.pdf.PdfWriter.getInstance(document, output);
        document.open();
        try {
            com.itextpdf.text.pdf.PdfContentByte canvas = writer.getDirectContent();
            float x = 10;
            for (int index = 0; index < bars.size(); index++) {
                float barWidth = ((Number) bars.get(index)).floatValue();
                if (index % 2 == 0) canvas.rectangle(x, 10, barWidth, 50);
                x += barWidth;
            }
            canvas.fill();
        } finally { document.close(); }
        byte[] pdf = output.toByteArray();
        response.setContentType("application/pdf");
        response.setHeader("Content-Disposition", "inline; filename=\"Item-Barcode.pdf\"");
        response.setContentLength(pdf.length);
        response.getOutputStream().write(pdf);
    }

    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 324-SalesItemPricingList.rpt
     * Procedure: USP_GetFinalItemSalePriceList
     * Desktop: ItemPricingSchedule.FinalItemSalePriceListSlip
     */
    @RequestMapping(value = "/reports/print/324-sales-item-pricing-list", method = RequestMethod.POST)
    public void print324SalesItemPricingList(HttpServletResponse response, @RequestBody(required = false) Rpt324SalesItemPricingListRequest request) throws Exception {
        if (request == null) request = new Rpt324SalesItemPricingListRequest();
        printReport(response, "324-SalesItemPricingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/324-sales-item-pricing-list", method = RequestMethod.GET)
    public void print324SalesItemPricingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print324SalesItemPricingList(response, objectMapper.convertValue(query, Rpt324SalesItemPricingListRequest.class));
    }

    /**
     * Template: 324A_SalesPricingList.rpt
     * Procedure: Sp_ItemPricingSchedule_GetAllMethod
     * Desktop: ItemPricingSchedule.GetItemPricingFormulaAgainstPriceTypeId
     */
    @RequestMapping(value = "/reports/print/324a-sales-pricing-list", method = RequestMethod.POST)
    public void print324ASalesPricingList(HttpServletResponse response, @RequestBody(required = false) Rpt324ASalesPricingListRequest request) throws Exception {
        if (request == null) request = new Rpt324ASalesPricingListRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "324A_SalesPricingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/324a-sales-pricing-list", method = RequestMethod.GET)
    public void print324ASalesPricingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print324ASalesPricingList(response, objectMapper.convertValue(query, Rpt324ASalesPricingListRequest.class));
    }

    /**
     * Template: 325-ItemPricingList.rpt
     * Procedure: USP_GetItemSchedulePricingSlip
     * Desktop: ItemPricingSchedule.GetItemSchedulePricingSlip
     */
    @RequestMapping(value = "/reports/print/325-item-pricing-list", method = RequestMethod.POST)
    public void print325ItemPricingList(HttpServletResponse response, @RequestBody(required = false) Rpt325ItemPricingListRequest request) throws Exception {
        if (request == null) request = new Rpt325ItemPricingListRequest();
        printReport(response, "325-ItemPricingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/325-item-pricing-list", method = RequestMethod.GET)
    public void print325ItemPricingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print325ItemPricingList(response, objectMapper.convertValue(query, Rpt325ItemPricingListRequest.class));
    }

    /**
     * Template: 325_01-ItemPricingList.rpt
     * Procedure: USP_GetItemPricingList
     * Desktop: ItemPricingSchedule.GetItemPricingList
     */
    @RequestMapping(value = "/reports/print/325-01-item-pricing-list", method = RequestMethod.POST)
    public void print32501ItemPricingList(HttpServletResponse response, @RequestBody(required = false) Rpt32501ItemPricingListRequest request) throws Exception {
        if (request == null) request = new Rpt32501ItemPricingListRequest();
        printReport(response, "325_01-ItemPricingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/325-01-item-pricing-list", method = RequestMethod.GET)
    public void print32501ItemPricingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print32501ItemPricingList(response, objectMapper.convertValue(query, Rpt32501ItemPricingListRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
