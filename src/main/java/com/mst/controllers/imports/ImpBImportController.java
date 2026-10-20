package com.mst.controllers.imports;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.imports.ImpBImportInvoiceService;
import com.mst.services.imports.ImpBPackingDetailService;
import com.mst.services.imports.ImpBProformaService;
import com.mst.services.imports.ImpBSerialService;
import com.mst.services.imports.ImpBShipmentBookingService;
import com.mst.services.imports.ImpBSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.nio.charset.StandardCharsets;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * Import application - four screens (routes wired in DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID):
 *
 *   782 /import/proforma-invoice        frmProformaInvoice        API /api/import/proforma-invoice/...
 *   783 /import/import-invoice          frmImportInvoice          API /api/import/import-invoice/...
 *   784 /import/invoice-packing-detail  frmInvoicePackingDetail   API /api/import/invoice-packing-detail/...
 *   785 /import/shipment-booking        frmShipmentBooking        API /api/import/shipment-booking/...
 *
 * Page GETs only render the template; every API call checks View on its ScreenDefinition row in the service
 * (and Save / Update / Print where the desktop form checks them). No parameter carries tenancy or a user id.
 */
@Controller
public class ImpBImportController {


    @Autowired private ImpBShipmentBookingService booking;
    @Autowired private ImpBPackingDetailService packing;
    @Autowired private ImpBProformaService proforma;
    @Autowired private ImpBImportInvoiceService invoice;
    @Autowired private ImpBSerialService serial;

    // ------------------------------------------------------------------ pages

    @GetMapping("/import/shipment-booking")
    public String bookingPage(Model model) { model.addAttribute("activeMenu", "import"); return "imports/shipment-booking"; }

    @GetMapping("/import/invoice-packing-detail")
    public String packingPage(Model model) { model.addAttribute("activeMenu", "import"); return "imports/invoice-packing-detail"; }

    @GetMapping("/import/proforma-invoice")
    public String proformaPage(Model model) { model.addAttribute("activeMenu", "import"); return "imports/proforma-invoice"; }

    @GetMapping("/import/import-invoice")
    public String invoicePage(Model model) { model.addAttribute("activeMenu", "import"); return "imports/import-invoice"; }

    private static ResponseEntity<?> file(Callable<ImpBSupport.DesktopAttachmentStoreFile> work) {
        try {
            ImpBSupport.DesktopAttachmentStoreFile f = work.call();
            return ResponseEntity.ok()
                    .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(f.name, StandardCharsets.UTF_8).build().toString())
                    .contentType(MediaType.APPLICATION_OCTET_STREAM).body(f.bytes);
        } catch (Exception e) {
            return HrmApi.run(() -> { throw e; });
        }
    }

    // ------------------------------------------------------------------ 785 Shipment Booking

    @GetMapping("/api/import/shipment-booking/setup") @ResponseBody
    public ResponseEntity<?> sbSetup() { return HrmApi.run(() -> booking.setup()); }

    @GetMapping("/api/import/shipment-booking/refresh") @ResponseBody
    public ResponseEntity<?> sbRefresh(@RequestParam(value = "recId", defaultValue = "0") long recId) { return HrmApi.run(() -> booking.refresh(recId)); }

    @GetMapping("/api/import/shipment-booking/invoices") @ResponseBody
    public ResponseEntity<?> sbInvoices(@RequestParam(value = "recId", defaultValue = "0") long recId) { return HrmApi.run(() -> booking.invoiceList(recId)); }

    @GetMapping("/api/import/shipment-booking/by-id") @ResponseBody
    public ResponseEntity<?> sbById(@RequestParam("id") long id) { return HrmApi.run(() -> booking.byId(id)); }

    @PostMapping("/api/import/shipment-booking/history") @ResponseBody
    public ResponseEntity<?> sbHistory(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> booking.history(f)); }

    @PostMapping("/api/import/shipment-booking/save") @ResponseBody
    public ResponseEntity<?> sbSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> booking.save(b)); }

    @GetMapping("/api/import/shipment-booking/print") @ResponseBody
    public ResponseEntity<?> sbPrint(@RequestParam(value = "id", defaultValue = "0") long id) { return HrmApi.run(() -> booking.print(id)); }

    @GetMapping("/api/import/shipment-booking/attachments") @ResponseBody
    public ResponseEntity<?> sbAttachments(@RequestParam("id") long id) { return HrmApi.run(() -> booking.attachments(id)); }

    @GetMapping("/api/import/shipment-booking/attachment-file")
    public ResponseEntity<?> sbAttachmentFile(@RequestParam("id") long id, @RequestParam("attachmentId") int a) { return file(() -> booking.attachmentFile(id, a)); }

    // ------------------------------------------------------------------ 784 Invoice Packing Detail

    @GetMapping("/api/import/invoice-packing-detail/setup") @ResponseBody
    public ResponseEntity<?> pdSetup() { return HrmApi.run(() -> packing.setup()); }

    @GetMapping("/api/import/invoice-packing-detail/refresh") @ResponseBody
    public ResponseEntity<?> pdRefresh(@RequestParam(value = "recId", defaultValue = "0") long recId) { return HrmApi.run(() -> packing.refresh(recId)); }

    @GetMapping("/api/import/invoice-packing-detail/history-combos") @ResponseBody
    public ResponseEntity<?> pdHistoryCombos() { return HrmApi.run(() -> packing.historyCombos()); }

    @GetMapping("/api/import/invoice-packing-detail/uoms") @ResponseBody
    public ResponseEntity<?> pdUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> packing.uoms(itemId)); }

    @GetMapping("/api/import/invoice-packing-detail/by-invoice") @ResponseBody
    public ResponseEntity<?> pdByInvoice(@RequestParam("id") long id) { return HrmApi.run(() -> packing.byInvoice(id)); }

    @PostMapping("/api/import/invoice-packing-detail/history") @ResponseBody
    public ResponseEntity<?> pdHistory(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> packing.history(f)); }

    @PostMapping("/api/import/invoice-packing-detail/save") @ResponseBody
    public ResponseEntity<?> pdSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> packing.save(b)); }

    // ------------------------------------------------------------------ 782 Proforma Invoice

    @GetMapping("/api/import/proforma-invoice/setup") @ResponseBody
    public ResponseEntity<?> piSetup() { return HrmApi.run(() -> proforma.setup()); }

    @GetMapping("/api/import/proforma-invoice/refresh") @ResponseBody
    public ResponseEntity<?> piRefresh(@RequestParam(value = "recId", defaultValue = "0") long recId) { return HrmApi.run(() -> proforma.refresh(recId)); }

    @GetMapping("/api/import/proforma-invoice/reset") @ResponseBody
    public ResponseEntity<?> piReset() { return HrmApi.run(() -> proforma.reset()); }

    @GetMapping("/api/import/proforma-invoice/uoms") @ResponseBody
    public ResponseEntity<?> piUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> proforma.uoms(itemId)); }

    @GetMapping("/api/import/proforma-invoice/by-id") @ResponseBody
    public ResponseEntity<?> piById(@RequestParam("id") long id) { return HrmApi.run(() -> proforma.byId(id)); }

    @GetMapping("/api/import/proforma-invoice/history-combos") @ResponseBody
    public ResponseEntity<?> piHistoryCombos() { return HrmApi.run(() -> proforma.historyCombos()); }

    @PostMapping("/api/import/proforma-invoice/history") @ResponseBody
    public ResponseEntity<?> piHistory(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> proforma.history(f)); }

    @GetMapping("/api/import/proforma-invoice/history-detail") @ResponseBody
    public ResponseEntity<?> piHistoryDetail(@RequestParam("id") long id) { return HrmApi.run(() -> proforma.historyDetail(id)); }

    @PostMapping("/api/import/proforma-invoice/save") @ResponseBody
    public ResponseEntity<?> piSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> proforma.save(b)); }

    @GetMapping("/api/import/proforma-invoice/print") @ResponseBody
    public ResponseEntity<?> piPrint(@RequestParam(value = "id", defaultValue = "0") long id) { return HrmApi.run(() -> proforma.print(id)); }

    @GetMapping("/api/import/proforma-invoice/attachments") @ResponseBody
    public ResponseEntity<?> piAttachments(@RequestParam("id") long id) { return HrmApi.run(() -> proforma.attachments(id)); }

    @GetMapping("/api/import/proforma-invoice/attachment-file")
    public ResponseEntity<?> piAttachmentFile(@RequestParam("id") long id, @RequestParam("attachmentId") int a) { return file(() -> proforma.attachmentFile(id, a)); }

    @GetMapping("/api/import/proforma-invoice/proforma-serials") @ResponseBody
    public ResponseEntity<?> piSerials() { return HrmApi.run(() -> serial.proformaSerials(ImpBProformaService.SCREEN)); }

    @PostMapping("/api/import/proforma-invoice/proforma-serials/save") @ResponseBody
    public ResponseEntity<?> piSerialSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> serial.saveProformaSerial(ImpBProformaService.SCREEN, b)); }

    // ------------------------------------------------------------------ 783 Import Invoice

    @GetMapping("/api/import/import-invoice/setup") @ResponseBody
    public ResponseEntity<?> iiSetup() { return HrmApi.run(() -> invoice.setup()); }

    @GetMapping("/api/import/import-invoice/refresh") @ResponseBody
    public ResponseEntity<?> iiRefresh(@RequestParam(value = "recId", defaultValue = "0") long recId) { return HrmApi.run(() -> invoice.refresh(recId)); }

    @GetMapping("/api/import/import-invoice/reset") @ResponseBody
    public ResponseEntity<?> iiReset() { return HrmApi.run(() -> invoice.reset()); }

    @GetMapping("/api/import/import-invoice/uoms") @ResponseBody
    public ResponseEntity<?> iiUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> invoice.uoms(itemId)); }

    @GetMapping("/api/import/import-invoice/by-exporter") @ResponseBody
    public ResponseEntity<?> iiByExporter(@RequestParam("exporterId") int exporterId) { return HrmApi.run(() -> invoice.byExporter(exporterId)); }

    @GetMapping("/api/import/import-invoice/fi-balance") @ResponseBody
    public ResponseEntity<?> iiFiBalance(@RequestParam("documentTypeId") int documentTypeId, @RequestParam("id") int id) {
        return HrmApi.run(() -> invoice.fiBalance(documentTypeId, id));
    }

    @GetMapping("/api/import/import-invoice/by-id") @ResponseBody
    public ResponseEntity<?> iiById(@RequestParam("id") long id) { return HrmApi.run(() -> invoice.byId(id)); }

    @GetMapping("/api/import/import-invoice/history-combos") @ResponseBody
    public ResponseEntity<?> iiHistoryCombos() { return HrmApi.run(() -> invoice.historyCombos()); }

    @PostMapping("/api/import/import-invoice/history") @ResponseBody
    public ResponseEntity<?> iiHistory(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> invoice.history(f)); }

    @GetMapping("/api/import/import-invoice/history-detail") @ResponseBody
    public ResponseEntity<?> iiHistoryDetail(@RequestParam("id") long id) { return HrmApi.run(() -> invoice.historyDetail(id)); }

    @PostMapping("/api/import/import-invoice/save") @ResponseBody
    public ResponseEntity<?> iiSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> invoice.save(b)); }

    @GetMapping("/api/import/import-invoice/print") @ResponseBody
    public ResponseEntity<?> iiPrint(@RequestParam(value = "id", defaultValue = "0") long id) { return HrmApi.run(() -> invoice.print(id)); }

    @GetMapping("/api/import/import-invoice/attachments") @ResponseBody
    public ResponseEntity<?> iiAttachments(@RequestParam("id") long id) { return HrmApi.run(() -> invoice.attachments(id)); }

    @GetMapping("/api/import/import-invoice/attachment-file")
    public ResponseEntity<?> iiAttachmentFile(@RequestParam("id") long id, @RequestParam("attachmentId") int a) { return file(() -> invoice.attachmentFile(id, a)); }

    @GetMapping("/api/import/import-invoice/proforma-serials") @ResponseBody
    public ResponseEntity<?> iiProformaSerials() { return HrmApi.run(() -> serial.proformaSerials(ImpBImportInvoiceService.SCREEN)); }

    @PostMapping("/api/import/import-invoice/proforma-serials/save") @ResponseBody
    public ResponseEntity<?> iiProformaSerialSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> serial.saveProformaSerial(ImpBImportInvoiceService.SCREEN, b)); }

    @GetMapping("/api/import/import-invoice/master-serials") @ResponseBody
    public ResponseEntity<?> iiMasterSerials() { return HrmApi.run(() -> serial.masterSerials(ImpBImportInvoiceService.SCREEN)); }

    @PostMapping("/api/import/import-invoice/master-serials/save") @ResponseBody
    public ResponseEntity<?> iiMasterSerialSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> serial.saveMasterSerial(ImpBImportInvoiceService.SCREEN, b)); }
}
