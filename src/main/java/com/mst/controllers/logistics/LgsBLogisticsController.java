package com.mst.controllers.logistics;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.logistics.LgsBFreightVoucherService;
import com.mst.services.logistics.LgsBPurchaseOrderService;
import com.mst.services.logistics.LgsBServicesBillService;
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
 * Logistics transaction screens (LgsB group) - pages and API.
 *
 *   949 /logistics/purchase-order                frmLogisticPurchaseOrder              API /api/logistics/purchase-order/...
 *   955 /logistics/services-bill                 frmLogisticPurchaseServicesBill       API /api/logistics/services-bill/...
 *   965 /logistics/freight-voucher-export        frmFreightVoucherExport               API /api/logistics/freight-voucher-export/...
 *   967 /logistics/freight-voucher-export-multi  frmFreightVoucherExportMultiVehicles  API /api/logistics/freight-voucher-export-multi/...
 *
 * The page GETs only render the template; every API call checks View on the screen (and Save / Update / Delete / Print
 * where the form does) in the service. Each page also opens a record from ?id=.
 */
@Controller
public class LgsBLogisticsController {


    @Autowired private LgsBPurchaseOrderService po;
    @Autowired private LgsBServicesBillService sb;
    @Autowired private LgsBFreightVoucherService fv;

    // ================================================================== pages

    @GetMapping("/logistics/purchase-order")
    public String purchaseOrderPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/purchase-order"; }

    @GetMapping("/logistics/services-bill")
    public String servicesBillPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/services-bill"; }

    @GetMapping("/logistics/freight-voucher-export")
    public String freightVoucherPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/freight-voucher-export"; }

    @GetMapping("/logistics/freight-voucher-export-multi")
    public String freightVoucherMultiPage(Model model) { model.addAttribute("activeMenu", "apps"); return "logistics/freight-voucher-export-multi"; }

    // ================================================================== 949 Purchase Order

    @GetMapping("/api/logistics/purchase-order/setup") @ResponseBody
    public ResponseEntity<?> poSetup(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> po.setup(recId)); }

    @GetMapping("/api/logistics/purchase-order/refresh") @ResponseBody
    public ResponseEntity<?> poRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> po.refresh(recId)); }

    @GetMapping("/api/logistics/purchase-order/ports") @ResponseBody
    public ResponseEntity<?> poPorts() { return HrmApi.run(() -> po.ports()); }

    @GetMapping("/api/logistics/purchase-order/tax") @ResponseBody
    public ResponseEntity<?> poTax(@RequestParam("itemId") int itemId, @RequestParam(value = "docDate", required = false) String docDate) {
        return HrmApi.run(() -> po.tax(itemId, docDate));
    }

    @GetMapping("/api/logistics/purchase-order/tax-by-items") @ResponseBody
    public ResponseEntity<?> poTaxByItems(@RequestParam("itemIds") String itemIds, @RequestParam(value = "docDate", required = false) String docDate) {
        return HrmApi.run(() -> po.taxByItems(itemIds, docDate));
    }

    @GetMapping("/api/logistics/purchase-order/invoice-info") @ResponseBody
    public ResponseEntity<?> poInvoiceInfo(@RequestParam("id") int id) { return HrmApi.run(() -> po.invoiceInfo(id)); }

    @GetMapping("/api/logistics/purchase-order/by-id") @ResponseBody
    public ResponseEntity<?> poById(@RequestParam("id") int id) { return HrmApi.run(() -> po.read(id)); }

    @PostMapping("/api/logistics/purchase-order/save") @ResponseBody
    public ResponseEntity<?> poSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> po.save(body)); }

    @PostMapping("/api/logistics/purchase-order/delete") @ResponseBody
    public ResponseEntity<?> poDelete(@RequestParam("id") int id) { return HrmApi.run(() -> po.delete(id)); }

    @GetMapping("/api/logistics/purchase-order/history-combos") @ResponseBody
    public ResponseEntity<?> poHistoryCombos() { return HrmApi.run(() -> po.historyCombos()); }

    @PostMapping("/api/logistics/purchase-order/history") @ResponseBody
    public ResponseEntity<?> poHistory(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> po.history(body)); }

    @GetMapping("/api/logistics/purchase-order/history-detail") @ResponseBody
    public ResponseEntity<?> poHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> po.historyDetail(id)); }

    @GetMapping("/api/logistics/purchase-order/print-check") @ResponseBody
    public ResponseEntity<?> poPrintCheck(@RequestParam("id") int id) { return HrmApi.run(() -> po.printCheck(id)); }

    @GetMapping("/api/logistics/purchase-order/agreement-combos") @ResponseBody
    public ResponseEntity<?> poAgreementCombos() { return HrmApi.run(() -> po.agreementCombos()); }

    @PostMapping("/api/logistics/purchase-order/agreements") @ResponseBody
    public ResponseEntity<?> poAgreements(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> po.agreements(body)); }

    // ================================================================== 955 Services Bill

    @GetMapping("/api/logistics/services-bill/setup") @ResponseBody
    public ResponseEntity<?> sbSetup() { return HrmApi.run(() -> sb.setup()); }

    @GetMapping("/api/logistics/services-bill/refresh") @ResponseBody
    public ResponseEntity<?> sbRefresh() { return HrmApi.run(() -> sb.refresh()); }

    @GetMapping("/api/logistics/services-bill/ports") @ResponseBody
    public ResponseEntity<?> sbPorts() { return HrmApi.run(() -> sb.ports()); }

    @GetMapping("/api/logistics/services-bill/tax") @ResponseBody
    public ResponseEntity<?> sbTax(@RequestParam("itemId") int itemId, @RequestParam(value = "docDate", required = false) String docDate) {
        return HrmApi.run(() -> sb.tax(itemId, docDate));
    }

    @GetMapping("/api/logistics/services-bill/tax-by-items") @ResponseBody
    public ResponseEntity<?> sbTaxByItems(@RequestParam("itemIds") String itemIds, @RequestParam(value = "docDate", required = false) String docDate) {
        return HrmApi.run(() -> sb.taxByItems(itemIds, docDate));
    }

    @GetMapping("/api/logistics/services-bill/invoice-info") @ResponseBody
    public ResponseEntity<?> sbInvoiceInfo(@RequestParam("id") int id) { return HrmApi.run(() -> sb.invoiceInfo(id)); }

    @GetMapping("/api/logistics/services-bill/by-id") @ResponseBody
    public ResponseEntity<?> sbById(@RequestParam("id") int id) { return HrmApi.run(() -> sb.read(id)); }

    @PostMapping("/api/logistics/services-bill/save") @ResponseBody
    public ResponseEntity<?> sbSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> sb.save(body)); }

    @PostMapping("/api/logistics/services-bill/delete") @ResponseBody
    public ResponseEntity<?> sbDelete(@RequestParam("id") int id) { return HrmApi.run(() -> sb.delete(id)); }

    @GetMapping("/api/logistics/services-bill/history-combos") @ResponseBody
    public ResponseEntity<?> sbHistoryCombos() { return HrmApi.run(() -> sb.historyCombos()); }

    @PostMapping("/api/logistics/services-bill/history") @ResponseBody
    public ResponseEntity<?> sbHistory(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> sb.history(body)); }

    @GetMapping("/api/logistics/services-bill/history-detail") @ResponseBody
    public ResponseEntity<?> sbHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> sb.historyDetail(id)); }

    @GetMapping("/api/logistics/services-bill/print-check") @ResponseBody
    public ResponseEntity<?> sbPrintCheck(@RequestParam("id") int id) { return HrmApi.run(() -> sb.printCheck(id)); }

    @GetMapping("/api/logistics/services-bill/voucher-id") @ResponseBody
    public ResponseEntity<?> sbVoucherId(@RequestParam("id") int id) { return HrmApi.run(() -> sb.voucherId(id)); }

    @GetMapping("/api/logistics/services-bill/po-combos") @ResponseBody
    public ResponseEntity<?> sbPoCombos() { return HrmApi.run(() -> sb.poCombos()); }

    @PostMapping("/api/logistics/services-bill/pending-po") @ResponseBody
    public ResponseEntity<?> sbPendingPo(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> sb.pendingPo(body)); }

    @GetMapping("/api/logistics/services-bill/fv-combos") @ResponseBody
    public ResponseEntity<?> sbFvCombos() { return HrmApi.run(() -> sb.fvCombos()); }

    @PostMapping("/api/logistics/services-bill/pending-freight") @ResponseBody
    public ResponseEntity<?> sbPendingFreight(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> sb.pendingFreight(body)); }

    @GetMapping("/api/logistics/services-bill/freight-by-ids") @ResponseBody
    public ResponseEntity<?> sbFreightByIds(@RequestParam("invoiceId") int invoiceId, @RequestParam("ids") String ids) {
        return HrmApi.run(() -> sb.freightByIds(invoiceId, ids));
    }

    // ================================================================== 965 Freight Voucher (Export)

    @GetMapping("/api/logistics/freight-voucher-export/setup") @ResponseBody
    public ResponseEntity<?> fvSetup(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> fv.setup(false, recId)); }

    @GetMapping("/api/logistics/freight-voucher-export/refresh") @ResponseBody
    public ResponseEntity<?> fvRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> fv.refresh(false, recId)); }

    @GetMapping("/api/logistics/freight-voucher-export/pending-po") @ResponseBody
    public ResponseEntity<?> fvPendingPo(@RequestParam("partyId") int partyId, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return HrmApi.run(() -> fv.pendingPo(false, partyId, recId));
    }

    @GetMapping("/api/logistics/freight-voucher-export/pending-gp") @ResponseBody
    public ResponseEntity<?> fvPendingGp(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> fv.pendingGp(recId)); }

    @GetMapping("/api/logistics/freight-voucher-export/balance") @ResponseBody
    public ResponseEntity<?> fvBalance(@RequestParam("accountId") int accountId) { return HrmApi.run(() -> fv.balance(accountId)); }

    @GetMapping("/api/logistics/freight-voucher-export/cheques") @ResponseBody
    public ResponseEntity<?> fvCheques(@RequestParam("bankId") int bankId, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return HrmApi.run(() -> fv.cheques(bankId, recId));
    }

    @GetMapping("/api/logistics/freight-voucher-export/other-charges") @ResponseBody
    public ResponseEntity<?> fvOtherCharges() { return HrmApi.run(() -> fv.otherCharges()); }

    @GetMapping("/api/logistics/freight-voucher-export/gp-loader-combos") @ResponseBody
    public ResponseEntity<?> fvGpLoaderCombos() { return HrmApi.run(() -> fv.gpLoaderCombos()); }

    @PostMapping("/api/logistics/freight-voucher-export/gp-loader") @ResponseBody
    public ResponseEntity<?> fvGpLoader(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> fv.gpLoader(body)); }

    @GetMapping("/api/logistics/freight-voucher-export/by-id") @ResponseBody
    public ResponseEntity<?> fvById(@RequestParam("id") int id) { return HrmApi.run(() -> fv.read(id)); }

    @PostMapping("/api/logistics/freight-voucher-export/save") @ResponseBody
    public ResponseEntity<?> fvSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> fv.save(body)); }

    @PostMapping("/api/logistics/freight-voucher-export/delete") @ResponseBody
    public ResponseEntity<?> fvDelete(@RequestParam("id") int id) { return HrmApi.run(() -> fv.delete(false, id)); }

    @PostMapping("/api/logistics/freight-voucher-export/history") @ResponseBody
    public ResponseEntity<?> fvHistory(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> fv.history(false, body)); }

    @GetMapping("/api/logistics/freight-voucher-export/history-detail") @ResponseBody
    public ResponseEntity<?> fvHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> fv.historyDetail(false, id)); }

    @GetMapping("/api/logistics/freight-voucher-export/print-check") @ResponseBody
    public ResponseEntity<?> fvPrintCheck(@RequestParam("id") int id) { return HrmApi.run(() -> fv.printCheck(false, id)); }

    @GetMapping("/api/logistics/freight-voucher-export/voucher-id") @ResponseBody
    public ResponseEntity<?> fvVoucherId(@RequestParam("id") int id) { return HrmApi.run(() -> fv.voucherId(id)); }

    // ================================================================== 967 Freight Voucher Export (Multi Vehicles)

    @GetMapping("/api/logistics/freight-voucher-export-multi/setup") @ResponseBody
    public ResponseEntity<?> fmSetup(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> fv.setup(true, recId)); }

    @GetMapping("/api/logistics/freight-voucher-export-multi/refresh") @ResponseBody
    public ResponseEntity<?> fmRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return HrmApi.run(() -> fv.refresh(true, recId)); }

    @GetMapping("/api/logistics/freight-voucher-export-multi/pending-po") @ResponseBody
    public ResponseEntity<?> fmPendingPo(@RequestParam("partyId") int partyId, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return HrmApi.run(() -> fv.pendingPo(true, partyId, recId));
    }

    @PostMapping("/api/logistics/freight-voucher-export-multi/loader") @ResponseBody
    public ResponseEntity<?> fmLoader(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> fv.multiLoader(body)); }

    @GetMapping("/api/logistics/freight-voucher-export-multi/by-id") @ResponseBody
    public ResponseEntity<?> fmById(@RequestParam("id") int id) { return HrmApi.run(() -> fv.readMulti(id)); }

    @PostMapping("/api/logistics/freight-voucher-export-multi/save") @ResponseBody
    public ResponseEntity<?> fmSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> fv.saveMulti(body)); }

    @PostMapping("/api/logistics/freight-voucher-export-multi/delete") @ResponseBody
    public ResponseEntity<?> fmDelete(@RequestParam("id") int id) { return HrmApi.run(() -> fv.delete(true, id)); }

    @PostMapping("/api/logistics/freight-voucher-export-multi/history") @ResponseBody
    public ResponseEntity<?> fmHistory(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> fv.history(true, body)); }

    @GetMapping("/api/logistics/freight-voucher-export-multi/history-detail") @ResponseBody
    public ResponseEntity<?> fmHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> fv.historyDetail(true, id)); }

    @GetMapping("/api/logistics/freight-voucher-export-multi/print-check") @ResponseBody
    public ResponseEntity<?> fmPrintCheck(@RequestParam("id") int id) { return HrmApi.run(() -> fv.printCheck(true, id)); }

    @GetMapping("/api/logistics/freight-voucher-export-multi/voucher-ids") @ResponseBody
    public ResponseEntity<?> fmVoucherIds(@RequestParam("id") int id) { return HrmApi.run(() -> fv.voucherIds(id)); }
}
