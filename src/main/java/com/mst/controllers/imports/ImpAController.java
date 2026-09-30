package com.mst.controllers.imports;

import com.mst.controllers.hrm.HrmApi;
import com.mst.models.imports.ImpADtos;
import com.mst.services.imports.ImpACommercialInvoiceService;
import com.mst.services.imports.ImpALcOrderScheduleService;
import com.mst.services.imports.ImpALcOrderService;
import com.mst.services.imports.ImpARegistersService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Import pages and API (screens owned by the ImpA* classes):
 *
 *   525 /import/lc-order                    ImLcOrder.cs               API /api/import/lc-order/...
 *   526 /import/lc-order-schedule           ImLcOrderSchedule.cs       API /api/import/lc-order-schedule/...
 *   221 /import/commercial-invoice[?mode=237] ImCommercialInvoice.cs   API /api/import/commercial-invoice/...
 *   392 /import/reports/invoice-register    ImportInvoiceRegister.cs   API /api/import/reports/invoice/...
 *   393 /import/reports/contract-register   ImportContractRegister.cs  API /api/import/reports/contract/...
 *   394 /import/reports/grn-register        ImportGrnRegister.cs       API /api/import/reports/grn/...
 *   395 /import/reports/purchase-register   ImportPurchaseRegister.cs  API /api/import/reports/purchase/...
 *
 * Page GETs only render; every API call checks View on its screen (and Save / Update / Delete / Print where the form does).
 */
@Controller
public class ImpAController {

    private static final String LC = "/api/import/lc-order";
    private static final String SC = "/api/import/lc-order-schedule";
    private static final String CI = "/api/import/commercial-invoice";
    private static final String RG = "/api/import/reports";

    @Autowired private ImpALcOrderService lc;
    @Autowired private ImpALcOrderScheduleService sc;
    @Autowired private ImpACommercialInvoiceService ci;
    @Autowired private ImpARegistersService rg;

    // ------------------------------------------------------------------ pages

    @GetMapping("/import/lc-order")
    public String lcOrderPage(Model m) { m.addAttribute("activeMenu", "apps"); return "imports/impa-lc-order"; }

    @GetMapping("/import/lc-order-schedule")
    public String schedulePage(Model m) { m.addAttribute("activeMenu", "apps"); return "imports/impa-lc-order-schedule"; }

    @GetMapping("/import/commercial-invoice")
    public String invoicePage(Model m, @RequestParam(value = "mode", required = false) Integer mode) {
        m.addAttribute("activeMenu", "apps");
        m.addAttribute("impaMode", ImpACommercialInvoiceService.docType(mode));
        return "imports/impa-commercial-invoice";
    }

    @GetMapping("/import/reports/invoice-register")
    public String invoiceRegisterPage(Model m) { return register(m, "invoice", 392, "Import Invoice Register", "807-Print", "807-ImInvoiceRegister_WithAvgRates.rpt", "impa-807"); }

    @GetMapping("/import/reports/contract-register")
    public String contractRegisterPage(Model m) { return register(m, "contract", 393, "Import Contract Register", "808-ImportContractRegister", "808-ImportContractRegister.rpt", "impa-808"); }

    @GetMapping("/import/reports/grn-register")
    public String grnRegisterPage(Model m) { return register(m, "grn", 394, "Import Grn Register", "811-ImportGrnRegister", "811-ImportGrnRegister.rpt", "impa-811"); }

    @GetMapping("/import/reports/purchase-register")
    public String purchaseRegisterPage(Model m) { return register(m, "purchase", 395, "Import Purchase Order Register", "810-ImportPurchaseOrderRegister", "810-ImportPurchaseOrder.rpt", "impa-810"); }

    private static String register(Model m, String kind, int screen, String title, String printText, String rpt, String key) {
        m.addAttribute("activeMenu", "apps");
        m.addAttribute("impaKind", kind);
        m.addAttribute("impaScreen", screen);
        m.addAttribute("impaTitle", title);
        m.addAttribute("impaPrintText", printText);
        m.addAttribute("impaRpt", rpt);
        m.addAttribute("impaKey", key);
        return "imports/impa-register";
    }

    // ------------------------------------------------------------------ 525 Lc Order

    @GetMapping(LC + "/setup") @ResponseBody
    public ResponseEntity<?> lcSetup() { return HrmApi.run(() -> lc.setup()); }

    @GetMapping(LC + "/refresh") @ResponseBody
    public ResponseEntity<?> lcRefresh() { return HrmApi.run(() -> lc.refresh()); }

    @GetMapping(LC + "/doc-no") @ResponseBody
    public ResponseEntity<?> lcDocNo() { return HrmApi.run(() -> lc.newDocNo()); }

    @GetMapping(LC + "/uoms") @ResponseBody
    public ResponseEntity<?> lcUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> lc.uoms(itemId)); }

    @GetMapping(LC + "/history") @ResponseBody
    public ResponseEntity<?> lcHistory(@RequestParam(value = "all", defaultValue = "false") boolean all) { return HrmApi.run(() -> lc.history(all)); }

    @GetMapping(LC + "/history-detail") @ResponseBody
    public ResponseEntity<?> lcHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> lc.historyDetail(id)); }

    @GetMapping(LC + "/by-id") @ResponseBody
    public ResponseEntity<?> lcById(@RequestParam("id") int id) { return HrmApi.run(() -> lc.byId(id)); }

    @PostMapping(LC + "/save") @ResponseBody
    public ResponseEntity<?> lcSave(@RequestBody ImpADtos.LcOrder body) { return HrmApi.run(() -> lc.save(body)); }

    @PostMapping(LC + "/delete") @ResponseBody
    public ResponseEntity<?> lcDelete(@RequestBody Map<String, Object> body) {
        return HrmApi.run(() -> lc.delete(com.mst.services.hrm.HrmSupport.toInt(body == null ? null : body.get("id"))));
    }

    @GetMapping(LC + "/print-check") @ResponseBody
    public ResponseEntity<?> lcPrintCheck(@RequestParam(value = "id", defaultValue = "0") int id) { return HrmApi.run(() -> lc.printCheck(id)); }

    // ------------------------------------------------------------------ 526 Lc Order Schedule

    @GetMapping(SC + "/setup") @ResponseBody
    public ResponseEntity<?> scSetup() { return HrmApi.run(() -> sc.setup()); }

    @GetMapping(SC + "/contracts") @ResponseBody
    public ResponseEntity<?> scContracts(@RequestParam(value = "all", defaultValue = "false") boolean all) { return HrmApi.run(() -> sc.contracts(all)); }

    @GetMapping(SC + "/load") @ResponseBody
    public ResponseEntity<?> scLoad(@RequestParam("contractId") int contractId) { return HrmApi.run(() -> sc.load(contractId)); }

    @GetMapping(SC + "/schedule-mton") @ResponseBody
    public ResponseEntity<?> scMton(@RequestParam("contractId") int contractId, @RequestParam("scheduleId") int scheduleId) {
        return HrmApi.run(() -> sc.scheduleMTon(contractId, scheduleId));
    }

    @GetMapping(SC + "/uoms") @ResponseBody
    public ResponseEntity<?> scUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> sc.uoms(itemId)); }

    @PostMapping(SC + "/save-main") @ResponseBody
    public ResponseEntity<?> scSaveMain(@RequestBody ImpADtos.ScheduleMain body) { return HrmApi.run(() -> sc.saveMain(body)); }

    @PostMapping(SC + "/save-detail") @ResponseBody
    public ResponseEntity<?> scSaveDetail(@RequestBody ImpADtos.SchedulePacking body) { return HrmApi.run(() -> sc.saveDetail(body)); }

    @GetMapping(SC + "/history") @ResponseBody
    public ResponseEntity<?> scHistory(@RequestParam(value = "from", required = false) String from, @RequestParam(value = "to", required = false) String to,
                                       @RequestParam(value = "portId", defaultValue = "0") int portId,
                                       @RequestParam(value = "supplierId", defaultValue = "0") int supplierId,
                                       @RequestParam(value = "itemId", defaultValue = "0") int itemId,
                                       @RequestParam(value = "all", defaultValue = "false") boolean all) {
        return HrmApi.run(() -> sc.history(from, to, portId, supplierId, itemId, all));
    }

    @GetMapping(SC + "/history-combos") @ResponseBody
    public ResponseEntity<?> scHistoryCombos() { return HrmApi.run(() -> sc.historyCombos()); }

    @GetMapping(SC + "/print-check") @ResponseBody
    public ResponseEntity<?> scPrintCheck() { return HrmApi.run(() -> sc.printCheck()); }

    // ------------------------------------------------------------------ 221 Commercial Invoice

    @GetMapping(CI + "/setup") @ResponseBody
    public ResponseEntity<?> ciSetup(@RequestParam(value = "mode", required = false) Integer mode) { return HrmApi.run(() -> ci.setup(mode)); }

    @GetMapping(CI + "/doc-no") @ResponseBody
    public ResponseEntity<?> ciDocNo() { return HrmApi.run(() -> ci.docNo()); }

    @GetMapping(CI + "/orders") @ResponseBody
    public ResponseEntity<?> ciOrders(@RequestParam(value = "mode", required = false) Integer mode, @RequestParam("supplierId") int supplierId) {
        return HrmApi.run(() -> ci.orders(mode, supplierId));
    }

    @GetMapping(CI + "/order") @ResponseBody
    public ResponseEntity<?> ciOrder(@RequestParam(value = "mode", required = false) Integer mode, @RequestParam("supplierId") int supplierId,
                                     @RequestParam("orderId") int orderId) {
        return HrmApi.run(() -> ci.order(mode, supplierId, orderId));
    }

    @PostMapping(CI + "/save") @ResponseBody
    public ResponseEntity<?> ciSave(@RequestParam(value = "mode", required = false) Integer mode, @RequestBody ImpADtos.Invoice body) {
        return HrmApi.run(() -> ci.save(body, mode));
    }

    @GetMapping(CI + "/history") @ResponseBody
    public ResponseEntity<?> ciHistory() { return HrmApi.run(() -> ci.history()); }

    @GetMapping(CI + "/print-check") @ResponseBody
    public ResponseEntity<?> ciPrintCheck(@RequestParam(value = "id", defaultValue = "0") int id) { return HrmApi.run(() -> ci.printCheck(id)); }

    // ------------------------------------------------------------------ 392-395 registers

    @GetMapping(RG + "/{kind}/setup") @ResponseBody
    public ResponseEntity<?> rgSetup(@PathVariable("kind") String kind) { return HrmApi.run(() -> rg.setup(kind)); }

    @GetMapping(RG + "/{kind}/show") @ResponseBody
    public ResponseEntity<?> rgShow(@PathVariable("kind") String kind, @RequestParam(value = "from", required = false) String from,
                                    @RequestParam(value = "to", required = false) String to,
                                    @RequestParam(value = "supplierId", defaultValue = "0") int supplierId,
                                    @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return HrmApi.run(() -> rg.show(kind, from, to, supplierId, itemId));
    }

    @GetMapping(RG + "/{kind}/print-check") @ResponseBody
    public ResponseEntity<?> rgPrintCheck(@PathVariable("kind") String kind) { return HrmApi.run(() -> rg.printCheck(kind)); }
}
