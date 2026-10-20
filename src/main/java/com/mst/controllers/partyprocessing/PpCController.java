package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpCJobOrderService;
import com.mst.services.partyprocessing.PpCLoaderService;
import com.mst.services.partyprocessing.PpCProcessingBillService;
import com.mst.services.partyprocessing.PpCProductionService;
import com.mst.services.partyprocessing.PpCStockAdjustmentService;
import com.mst.services.partyprocessing.PpCStockConversionService;
import com.mst.services.partyprocessing.PpCStockTransferService;
import com.mst.services.partyprocessing.PpCSupport;
import com.mst.services.partyprocessing.PpCWagesBillService;
import com.mst.models.UserAccount;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;
import java.util.function.IntConsumer;

/**
 * Party Processing group PpC: job order (676), production (677), processing bill (675), stock conversion (602),
 * stock transfer (603), stock adjustment (604), wages bill (614). Pages only render; every API call checks the
 * screen's View right in the service (support.user(screen)) and Save / Update / Delete / Print where the desktop does.
 * API root: /api/party-processing/{page}/...
 */
@Controller
public class PpCController {

    private static final String A = "/api/party-processing/";

    @Autowired private PpCJobOrderService jobOrder;
    @Autowired private PpCProductionService production;
    @Autowired private PpCProcessingBillService bill;
    @Autowired private PpCStockConversionService conversion;
    @Autowired private PpCStockTransferService transfer;
    @Autowired private PpCStockAdjustmentService adjustment;
    @Autowired private PpCWagesBillService wages;
    @Autowired private PpCLoaderService loader;
    @Autowired private PpCSupport pp;

    private static String page(Model model, int screen, String view) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("ppScreen", screen);
        return "partyprocessing/" + view;
    }

    @GetMapping("/party-processing/production-job-order")
    public String jobOrderPage(Model model) { return page(model, PpCJobOrderService.SCREEN, "production-job-order"); }

    @GetMapping("/party-processing/production")
    public String productionPage(Model model) { return page(model, PpCProductionService.SCREEN, "production"); }

    @GetMapping("/party-processing/processing-bill")
    public String billPage(Model model) { return page(model, PpCProcessingBillService.SCREEN, "processing-bill"); }

    @GetMapping("/party-processing/stock-conversion")
    public String conversionPage(Model model) { return page(model, PpCStockConversionService.SCREEN, "stock-conversion"); }

    @GetMapping("/party-processing/stock-transfer")
    public String transferPage(Model model) { return page(model, PpCStockTransferService.SCREEN, "stock-transfer"); }

    @GetMapping("/party-processing/stock-adjustment")
    public String adjustmentPage(Model model) { return page(model, PpCStockAdjustmentService.SCREEN, "stock-adjustment"); }

    @GetMapping("/party-processing/wages-bill")
    public String wagesPage(Model model) { return page(model, PpCWagesBillService.SCREEN, "wages-bill"); }

    /** Print gate: View on the screen, Print right for the tool-strip button (history row prints are not gated on the desktop), own record. */
    private Map<String, Object> printGate(int screen, int id, boolean fromHistory, IntConsumer read) {
        UserAccount u = pp.user(screen);
        if (!fromHistory) pp.require(u, screen, "Print");
        read.accept(id);
        return PpCSupport.m("id", id, "financialYearId", pp.year());
    }

    // ================================================================ issuance loader (604 / 603 / 602 / 677)

    private static int loaderScreen(String page) {
        switch (page) {
            case "stock-adjustment": return PpCStockAdjustmentService.SCREEN;
            case "stock-transfer": return PpCStockTransferService.SCREEN;
            case "stock-conversion": return PpCStockConversionService.SCREEN;
            case "production": return PpCProductionService.SCREEN;
            default: throw new IllegalArgumentException("Screen not found");
        }
    }

    @GetMapping("/api/party-processing/loader/combos") @ResponseBody
    public ResponseEntity<?> loaderCombos(@RequestParam("page") String page) { return HrmApi.run(() -> loader.combos(loaderScreen(page))); }

    @PostMapping("/api/party-processing/loader/list") @ResponseBody
    public ResponseEntity<?> loaderList(@RequestParam("page") String page, @RequestBody Map<String, Object> b) {
        return HrmApi.run(() -> loader.list(loaderScreen(page), b));
    }

    // ================================================================ 676 job order


    @GetMapping("/api/party-processing/production-job-order/setup") @ResponseBody public ResponseEntity<?> joSetup() { return HrmApi.run(jobOrder::setup); }
    @GetMapping("/api/party-processing/production-job-order/refresh") @ResponseBody public ResponseEntity<?> joRefresh() { return HrmApi.run(jobOrder::refresh); }
    @GetMapping("/api/party-processing/production-job-order/code") @ResponseBody public ResponseEntity<?> joCode() { return HrmApi.run(jobOrder::code); }
    @GetMapping("/api/party-processing/production-job-order/history-combos") @ResponseBody public ResponseEntity<?> joHistoryCombos() { return HrmApi.run(jobOrder::historyCombosApi); }
    @GetMapping("/api/party-processing/production-job-order/read") @ResponseBody public ResponseEntity<?> joRead(@RequestParam("id") int id) { return HrmApi.run(() -> jobOrder.read(id)); }
    @PostMapping("/api/party-processing/production-job-order/save") @ResponseBody public ResponseEntity<?> joSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> jobOrder.save(b)); }
    @PostMapping("/api/party-processing/production-job-order/delete") @ResponseBody public ResponseEntity<?> joDelete(@RequestParam("id") int id) { return HrmApi.run(() -> jobOrder.delete(id)); }
    @PostMapping("/api/party-processing/production-job-order/history") @ResponseBody public ResponseEntity<?> joHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> jobOrder.history(b)); }
    @GetMapping("/api/party-processing/production-job-order/print") @ResponseBody
    public ResponseEntity<?> joPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> printGate(PpCJobOrderService.SCREEN, id, h, jobOrder::read));
    }

    // ================================================================ 677 production


    @GetMapping("/api/party-processing/production/setup") @ResponseBody public ResponseEntity<?> prSetup() { return HrmApi.run(production::setup); }
    @GetMapping("/api/party-processing/production/refresh") @ResponseBody public ResponseEntity<?> prRefresh() { return HrmApi.run(production::refresh); }
    @GetMapping("/api/party-processing/production/history-combos") @ResponseBody public ResponseEntity<?> prHistoryCombos() { return HrmApi.run(production::historyCombos); }
    @GetMapping("/api/party-processing/production/codes") @ResponseBody public ResponseEntity<?> prCodes() { return HrmApi.run(production::codes); }
    @GetMapping("/api/party-processing/production/job-order") @ResponseBody public ResponseEntity<?> prJobOrder(@RequestParam("id") int id) { return HrmApi.run(() -> production.jobOrderGl(id)); }
    @GetMapping("/api/party-processing/production/balance") @ResponseBody
    public ResponseEntity<?> prBalance(@RequestParam("itemId") int itemId, @RequestParam("stockPartyId") int sp, @RequestParam("warehouseId") int wh) {
        return HrmApi.run(() -> production.balance(itemId, sp, wh));
    }
    @GetMapping("/api/party-processing/production/uoms") @ResponseBody public ResponseEntity<?> prUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> production.uoms(itemId)); }
    @GetMapping("/api/party-processing/production/totals") @ResponseBody public ResponseEntity<?> prTotals(@RequestParam("jobOrderId") int id) { return HrmApi.run(() -> production.totals(id)); }
    @GetMapping("/api/party-processing/production/read") @ResponseBody
    public ResponseEntity<?> prRead(@RequestParam("id") int id, @RequestParam(value = "expect", required = false) String expect) {
        return HrmApi.run(() -> production.read(id, expect));
    }
    @GetMapping("/api/party-processing/production/history-detail") @ResponseBody public ResponseEntity<?> prHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> production.historyDetail(id)); }
    @PostMapping("/api/party-processing/production/save-input") @ResponseBody public ResponseEntity<?> prSaveInput(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> production.saveInput(b)); }
    @PostMapping("/api/party-processing/production/save-output") @ResponseBody public ResponseEntity<?> prSaveOutput(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> production.saveOutput(b)); }
    @PostMapping("/api/party-processing/production/delete") @ResponseBody
    public ResponseEntity<?> prDelete(@RequestParam("id") int id, @RequestParam(value = "expect", required = false) String expect) {
        return HrmApi.run(() -> production.delete(id, expect));
    }
    @PostMapping("/api/party-processing/production/history") @ResponseBody
    public ResponseEntity<?> prHistory(@RequestParam(value = "docType", defaultValue = "0") int docType, @RequestBody Map<String, Object> b) {
        return HrmApi.run(() -> production.history(b, docType));
    }
    @PostMapping("/api/party-processing/production/save-pm") @ResponseBody public ResponseEntity<?> prSavePm(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> production.savePm(b)); }
    @GetMapping("/api/party-processing/production/read-pm") @ResponseBody public ResponseEntity<?> prReadPm(@RequestParam("id") int id) { return HrmApi.run(() -> production.readPm(id)); }
    @PostMapping("/api/party-processing/production/delete-pm") @ResponseBody public ResponseEntity<?> prDeletePm(@RequestParam("id") int id) { return HrmApi.run(() -> production.deletePm(id)); }
    @PostMapping("/api/party-processing/production/pm-history") @ResponseBody public ResponseEntity<?> prPmHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> production.pmHistory(b)); }
    @GetMapping("/api/party-processing/production/pm-print") @ResponseBody public ResponseEntity<?> prPmPrint(@RequestParam("id") int id) { return HrmApi.run(() -> production.pmPrintCheck(id)); }
    @GetMapping("/api/party-processing/production/report-check") @ResponseBody
    public ResponseEntity<?> prReportCheck(@RequestParam("which") String which, @RequestParam("jobOrderId") int jobOrderId,
                                           @RequestParam(value = "languageId", defaultValue = "0") int languageId) {
        return HrmApi.run(() -> production.reportCheck(which, jobOrderId, languageId));
    }
    @GetMapping("/api/party-processing/production/print") @ResponseBody
    public ResponseEntity<?> prPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> printGate(PpCProductionService.SCREEN, id, h, x -> production.read(x, null)));
    }

    // ================================================================ 675 processing bill


    @GetMapping("/api/party-processing/processing-bill/setup") @ResponseBody public ResponseEntity<?> pbSetup() { return HrmApi.run(bill::setup); }
    @GetMapping("/api/party-processing/processing-bill/refresh") @ResponseBody public ResponseEntity<?> pbRefresh() { return HrmApi.run(bill::refresh); }
    @GetMapping("/api/party-processing/processing-bill/code") @ResponseBody public ResponseEntity<?> pbCode() { return HrmApi.run(bill::code); }
    @GetMapping("/api/party-processing/processing-bill/history-combos") @ResponseBody public ResponseEntity<?> pbHistoryCombos() { return HrmApi.run(bill::historyCombos); }
    @GetMapping("/api/party-processing/processing-bill/job-order") @ResponseBody public ResponseEntity<?> pbJobOrder(@RequestParam("id") int id) { return HrmApi.run(() -> bill.jobOrder(id)); }
    @GetMapping("/api/party-processing/processing-bill/read") @ResponseBody public ResponseEntity<?> pbRead(@RequestParam("id") int id) { return HrmApi.run(() -> bill.read(id)); }
    @GetMapping("/api/party-processing/processing-bill/history-detail") @ResponseBody
    public ResponseEntity<?> pbHistoryDetail(@RequestParam("id") int id, @RequestParam("jobOrderId") int jobOrderId) {
        return HrmApi.run(() -> bill.historyDetail(id, jobOrderId));
    }
    @PostMapping("/api/party-processing/processing-bill/save") @ResponseBody public ResponseEntity<?> pbSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> bill.save(b)); }
    @PostMapping("/api/party-processing/processing-bill/delete") @ResponseBody public ResponseEntity<?> pbDelete(@RequestParam("id") int id) { return HrmApi.run(() -> bill.delete(id)); }
    @PostMapping("/api/party-processing/processing-bill/history") @ResponseBody public ResponseEntity<?> pbHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> bill.history(b)); }
    @GetMapping("/api/party-processing/processing-bill/voucher") @ResponseBody public ResponseEntity<?> pbVoucher(@RequestParam("id") int id) { return HrmApi.run(() -> bill.voucher(id)); }
    @GetMapping("/api/party-processing/processing-bill/print") @ResponseBody
    public ResponseEntity<?> pbPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> bill.printCheck(id, h));
    }

    // ================================================================ 602 stock conversion


    @GetMapping("/api/party-processing/stock-conversion/setup") @ResponseBody public ResponseEntity<?> scSetup() { return HrmApi.run(conversion::setup); }
    @GetMapping("/api/party-processing/stock-conversion/refresh") @ResponseBody public ResponseEntity<?> scRefresh() { return HrmApi.run(conversion::refresh); }
    @GetMapping("/api/party-processing/stock-conversion/code") @ResponseBody public ResponseEntity<?> scCode() { return HrmApi.run(conversion::code); }
    @GetMapping("/api/party-processing/stock-conversion/party") @ResponseBody public ResponseEntity<?> scParty(@RequestParam("stockPartyId") int id) { return HrmApi.run(() -> conversion.party(id)); }
    @GetMapping("/api/party-processing/stock-conversion/uoms") @ResponseBody public ResponseEntity<?> scUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> conversion.uoms(itemId)); }
    @GetMapping("/api/party-processing/stock-conversion/balance") @ResponseBody
    public ResponseEntity<?> scBalance(@RequestParam("itemId") int itemId, @RequestParam("stockPartyId") int sp, @RequestParam("warehouseId") int wh) {
        return HrmApi.run(() -> conversion.balance(itemId, sp, wh));
    }
    @GetMapping("/api/party-processing/stock-conversion/read") @ResponseBody public ResponseEntity<?> scRead(@RequestParam("id") int id) { return HrmApi.run(() -> conversion.read(id)); }
    @GetMapping("/api/party-processing/stock-conversion/history-detail") @ResponseBody public ResponseEntity<?> scHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> conversion.historyDetail(id)); }
    @PostMapping("/api/party-processing/stock-conversion/save") @ResponseBody public ResponseEntity<?> scSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> conversion.save(b)); }
    @PostMapping("/api/party-processing/stock-conversion/delete") @ResponseBody public ResponseEntity<?> scDelete(@RequestParam("id") int id) { return HrmApi.run(() -> conversion.delete(id)); }
    @PostMapping("/api/party-processing/stock-conversion/history") @ResponseBody public ResponseEntity<?> scHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> conversion.history(b)); }
    @GetMapping("/api/party-processing/stock-conversion/print") @ResponseBody
    public ResponseEntity<?> scPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> printGate(PpCStockConversionService.SCREEN, id, h, conversion::read));
    }

    // ================================================================ 603 stock transfer


    @GetMapping("/api/party-processing/stock-transfer/setup") @ResponseBody public ResponseEntity<?> stSetup() { return HrmApi.run(transfer::setup); }
    @GetMapping("/api/party-processing/stock-transfer/refresh") @ResponseBody public ResponseEntity<?> stRefresh() { return HrmApi.run(transfer::refresh); }
    @GetMapping("/api/party-processing/stock-transfer/code") @ResponseBody public ResponseEntity<?> stCode(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> transfer.code(recId)); }
    @GetMapping("/api/party-processing/stock-transfer/uoms") @ResponseBody public ResponseEntity<?> stUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> transfer.uoms(itemId)); }
    @GetMapping("/api/party-processing/stock-transfer/stock") @ResponseBody
    public ResponseEntity<?> stStock(@RequestParam("warehouseId") int wh, @RequestParam("itemId") int itemId, @RequestParam(value = "jobLotId", defaultValue = "0") int jl,
                                     @RequestParam(value = "cropYear", required = false) String crop, @RequestParam(value = "docDate", required = false) String d) {
        return HrmApi.run(() -> transfer.stock(wh, itemId, jl, crop, d));
    }
    @GetMapping("/api/party-processing/stock-transfer/read") @ResponseBody public ResponseEntity<?> stRead(@RequestParam("id") int id) { return HrmApi.run(() -> transfer.read(id)); }
    @GetMapping("/api/party-processing/stock-transfer/history-detail") @ResponseBody public ResponseEntity<?> stHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> transfer.historyDetail(id)); }
    @PostMapping("/api/party-processing/stock-transfer/save") @ResponseBody public ResponseEntity<?> stSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> transfer.save(b)); }
    @PostMapping("/api/party-processing/stock-transfer/history") @ResponseBody public ResponseEntity<?> stHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> transfer.history(b)); }
    @GetMapping("/api/party-processing/stock-transfer/print") @ResponseBody
    public ResponseEntity<?> stPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> printGate(PpCStockTransferService.SCREEN, id, h, transfer::read));
    }

    // ================================================================ 604 stock adjustment


    @GetMapping("/api/party-processing/stock-adjustment/setup") @ResponseBody public ResponseEntity<?> saSetup() { return HrmApi.run(adjustment::setup); }
    @GetMapping("/api/party-processing/stock-adjustment/refresh") @ResponseBody public ResponseEntity<?> saRefresh() { return HrmApi.run(adjustment::refresh); }
    @GetMapping("/api/party-processing/stock-adjustment/code") @ResponseBody public ResponseEntity<?> saCode() { return HrmApi.run(adjustment::code); }
    @GetMapping("/api/party-processing/stock-adjustment/history-combos") @ResponseBody public ResponseEntity<?> saHistoryCombos() { return HrmApi.run(adjustment::historyCombosApi); }
    @GetMapping("/api/party-processing/stock-adjustment/uoms") @ResponseBody public ResponseEntity<?> saUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> adjustment.uoms(itemId)); }
    @GetMapping("/api/party-processing/stock-adjustment/stock") @ResponseBody
    public ResponseEntity<?> saStock(@RequestParam("itemId") int itemId, @RequestParam(value = "docDate", required = false) String d,
                                     @RequestParam(value = "jobLotId", defaultValue = "0") int jl, @RequestParam("warehouseId") int wh,
                                     @RequestParam(value = "cropYear", required = false) String crop) {
        return HrmApi.run(() -> adjustment.stock(itemId, d, jl, wh, crop));
    }
    @GetMapping("/api/party-processing/stock-adjustment/read") @ResponseBody public ResponseEntity<?> saRead(@RequestParam("id") int id) { return HrmApi.run(() -> adjustment.read(id)); }
    @PostMapping("/api/party-processing/stock-adjustment/save") @ResponseBody public ResponseEntity<?> saSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> adjustment.save(b)); }
    @PostMapping("/api/party-processing/stock-adjustment/delete") @ResponseBody public ResponseEntity<?> saDelete(@RequestParam("id") int id) { return HrmApi.run(() -> adjustment.delete(id)); }
    @PostMapping("/api/party-processing/stock-adjustment/history") @ResponseBody public ResponseEntity<?> saHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> adjustment.history(b)); }
    @GetMapping("/api/party-processing/stock-adjustment/print") @ResponseBody
    public ResponseEntity<?> saPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> printGate(PpCStockAdjustmentService.SCREEN, id, h, adjustment::read));
    }

    // ================================================================ 614 wages bill


    @GetMapping("/api/party-processing/wages-bill/setup") @ResponseBody
    public ResponseEntity<?> wbSetup(@RequestParam(value = "refDocTypeId", defaultValue = "0") int t, @RequestParam(value = "refDocId", defaultValue = "0") int id) {
        return HrmApi.run(() -> wages.setup(t, id));
    }
    @GetMapping("/api/party-processing/wages-bill/reset") @ResponseBody
    public ResponseEntity<?> wbReset(@RequestParam(value = "refDocTypeId", defaultValue = "0") int t, @RequestParam(value = "refDocId", defaultValue = "0") int id) {
        return HrmApi.run(() -> wages.reset(t, id));
    }
    @GetMapping("/api/party-processing/wages-bill/refresh") @ResponseBody
    public ResponseEntity<?> wbRefresh(@RequestParam(value = "refDocTypeId", defaultValue = "0") int t) { return HrmApi.run(() -> wages.refresh(t)); }
    @GetMapping("/api/party-processing/wages-bill/doc-types") @ResponseBody public ResponseEntity<?> wbDocTypes() { return HrmApi.run(wages::docTypes); }
    @PostMapping("/api/party-processing/wages-bill/load") @ResponseBody public ResponseEntity<?> wbLoad(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> wages.load(b)); }
    @GetMapping("/api/party-processing/wages-bill/read") @ResponseBody public ResponseEntity<?> wbRead(@RequestParam("id") int id) { return HrmApi.run(() -> wages.read(id)); }
    @GetMapping("/api/party-processing/wages-bill/rate") @ResponseBody
    public ResponseEntity<?> wbRate(@RequestParam("date") String date, @RequestParam("packSize") double packSize, @RequestParam("wagesId") int wagesId,
                                    @RequestParam("contractorId") int contractorId) {
        return HrmApi.run(() -> wages.rate(date, packSize, wagesId, contractorId));
    }
    @GetMapping("/api/party-processing/wages-bill/free-of-cost") @ResponseBody
    public ResponseEntity<?> wbFree(@RequestParam("date") String date, @RequestParam("refDocTypeId") int t, @RequestParam("itemId") int itemId,
                                    @RequestParam("wagesId") int wagesId) {
        return HrmApi.run(() -> wages.freeOfCost(date, t, itemId, wagesId));
    }
    @PostMapping("/api/party-processing/wages-bill/delete-by-reference") @ResponseBody
    public ResponseEntity<?> wbDeleteByRef(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> wages.deleteByReference(b)); }
    @PostMapping("/api/party-processing/wages-bill/save") @ResponseBody public ResponseEntity<?> wbSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> wages.save(b)); }
    @PostMapping("/api/party-processing/wages-bill/history") @ResponseBody public ResponseEntity<?> wbHistory(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> wages.history(b)); }
    @GetMapping("/api/party-processing/wages-bill/history-detail") @ResponseBody public ResponseEntity<?> wbHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> wages.historyDetail(id)); }
    @GetMapping("/api/party-processing/wages-bill/voucher") @ResponseBody public ResponseEntity<?> wbVoucher(@RequestParam("id") int id) { return HrmApi.run(() -> wages.voucher(id)); }
    @GetMapping("/api/party-processing/wages-bill/print") @ResponseBody
    public ResponseEntity<?> wbPrint(@RequestParam("id") int id, @RequestParam(value = "history", defaultValue = "false") boolean h) {
        return HrmApi.run(() -> wages.printCheck(id, h));
    }
}
