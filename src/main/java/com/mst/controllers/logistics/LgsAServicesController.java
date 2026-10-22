package com.mst.controllers.logistics;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.logistics.LgsAServiceBillService;
import com.mst.services.logistics.LgsAServicesItemService;
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

/**
 * Logistics (Service) pages and API:
 *
 *   234 /logistics/define-services-item  Architecture.WinApp.Service/EximServicesDefine.cs
 *       API /api/logistics/define-services-item/{setup|refresh|allocation|category|history|by-id|save}
 *   233 /logistics/service-bill-direct   Architecture.WinApp.Service/ExImClearingAgentBillDirect.cs
 *       API /api/logistics/service-bill-direct/{setup|refresh|doc-no|uoms|invoice|history|history-detail|by-id|save|delete|slip-check|voucher-id}
 *
 * Page GETs only render the template; every API call checks the screen's View right (and Save / Update / Delete /
 * Print where the desktop checks them) in the service.
 */
@Controller
public class LgsAServicesController {


    @Autowired private LgsAServicesItemService items;
    @Autowired private LgsAServiceBillService bills;

    // ------------------------------------------------------------------ 234 Define Services Item's

    @GetMapping("/logistics/define-services-item")
    public String servicesItemPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/define-services-item"; }

    @GetMapping("/api/logistics/define-services-item/setup") @ResponseBody
    public ResponseEntity<?> siSetup() { return HrmApi.run(() -> items.setup()); }

    @GetMapping("/api/logistics/define-services-item/refresh") @ResponseBody
    public ResponseEntity<?> siRefresh() { return HrmApi.run(() -> items.refresh()); }

    @GetMapping("/api/logistics/define-services-item/allocation") @ResponseBody
    public ResponseEntity<?> siAllocation() { return HrmApi.run(() -> items.allocation()); }

    @GetMapping("/api/logistics/define-services-item/category") @ResponseBody
    public ResponseEntity<?> siCategory(@RequestParam("id") int id) { return HrmApi.run(() -> items.categoryLeave(id)); }

    @GetMapping("/api/logistics/define-services-item/history") @ResponseBody
    public ResponseEntity<?> siHistory() { return HrmApi.run(() -> items.history()); }

    @GetMapping("/api/logistics/define-services-item/by-id") @ResponseBody
    public ResponseEntity<?> siById(@RequestParam("id") int id) { return HrmApi.run(() -> items.read(id)); }

    @PostMapping("/api/logistics/define-services-item/save") @ResponseBody
    public ResponseEntity<?> siSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> items.save(body)); }

    // ------------------------------------------------------------------ 233 Service Bill Direct

    @GetMapping("/logistics/service-bill-direct")
    public String serviceBillPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/service-bill-direct"; }

    @GetMapping("/api/logistics/service-bill-direct/setup") @ResponseBody
    public ResponseEntity<?> sbSetup() { return HrmApi.run(() -> bills.setup()); }

    @GetMapping("/api/logistics/service-bill-direct/refresh") @ResponseBody
    public ResponseEntity<?> sbRefresh() { return HrmApi.run(() -> bills.refresh()); }

    @GetMapping("/api/logistics/service-bill-direct/doc-no") @ResponseBody
    public ResponseEntity<?> sbDocNo() { return HrmApi.run(() -> bills.docNo()); }

    @GetMapping("/api/logistics/service-bill-direct/uoms") @ResponseBody
    public ResponseEntity<?> sbUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> bills.uoms(itemId)); }

    @GetMapping("/api/logistics/service-bill-direct/invoice") @ResponseBody
    public ResponseEntity<?> sbInvoice(@RequestParam("id") int id) { return HrmApi.run(() -> bills.invoiceData(id)); }

    @GetMapping("/api/logistics/service-bill-direct/history") @ResponseBody
    public ResponseEntity<?> sbHistory(@RequestParam(value = "dateKind", required = false) String dateKind,
                                       @RequestParam(value = "fromDate", required = false) String fromDate,
                                       @RequestParam(value = "toDate", required = false) String toDate,
                                       @RequestParam(value = "fromDocNo", required = false) String fromDocNo,
                                       @RequestParam(value = "toDocNo", required = false) String toDocNo) {
        return HrmApi.run(() -> bills.history(dateKind, fromDate, toDate, fromDocNo, toDocNo));
    }

    @GetMapping("/api/logistics/service-bill-direct/history-detail") @ResponseBody
    public ResponseEntity<?> sbHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> bills.historyDetail(id)); }

    @GetMapping("/api/logistics/service-bill-direct/by-id") @ResponseBody
    public ResponseEntity<?> sbById(@RequestParam("id") int id) { return HrmApi.run(() -> bills.read(id)); }

    @PostMapping("/api/logistics/service-bill-direct/save") @ResponseBody
    public ResponseEntity<?> sbSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> bills.save(body)); }

    @PostMapping("/api/logistics/service-bill-direct/delete") @ResponseBody
    public ResponseEntity<?> sbDelete(@RequestParam("id") int id) { return HrmApi.run(() -> bills.delete(id)); }

    @GetMapping("/api/logistics/service-bill-direct/slip-check") @ResponseBody
    public ResponseEntity<?> sbSlipCheck(@RequestParam("id") int id) { return HrmApi.run(() -> bills.slipCheck(id)); }

    @GetMapping("/api/logistics/service-bill-direct/voucher-id") @ResponseBody
    public ResponseEntity<?> sbVoucherId(@RequestParam("id") int id) { return HrmApi.run(() -> bills.voucherId(id)); }
}
