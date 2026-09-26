package com.mst.controllers;

import com.mst.models.dto.StoreReportsADto;
import com.mst.services.StoreReportsAService;
import javax.servlet.http.HttpServletRequest;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Management Reports (module 46), group A — one page per report:
 * <ul>
 *   <li>333 "Store Purchase Register", ScreenName StorePurchaseRegister,
 *       TargetUrl Architecture.WinApp.Inventory_Reports.StorePurchaseRegister →
 *       /store/reports/store-purchase-register</li>
 *   <li>452 "Store Purchase Demand Report", ScreenName StorePurchaseDemandRegister →
 *       /store/reports/store-purchase-demand-register</li>
 *   <li>453 "Store Issuance Return Report", ScreenName StoreIssuanceReturnRegister →
 *       /store/reports/store-issuance-return-register</li>
 * </ul>
 * APIs live under /api/store/reports/…. No parameter carries an organization, company, user or
 * financial year — those come from the signed-in user. Behaviour notes and deviations are on
 * {@link StoreReportsAService}.
 */
@Controller
public class StoreReportsAController {

    private static final String SPR = "/api/store/reports/store-purchase-register";
    private static final String PDR = "/api/store/reports/store-purchase-demand-register";
    private static final String SRR = "/api/store/reports/store-issuance-return-register";

    private final StoreReportsAService service;

    public StoreReportsAController(StoreReportsAService service) { this.service = service; }

    // ------------------------------------------------------------ 333 Store Purchase Register

    @GetMapping("/store/reports/store-purchase-register")
    public String storePurchaseRegisterPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/store_purchase_register";
    }

    @GetMapping(SPR + "/lookups")
    @ResponseBody
    public ResponseEntity<?> sprLookups() { return run(service::purchaseRegisterLookups, "Could not load the filters."); }

    /** Refresh button — branches and combos again. */
    @GetMapping(SPR + "/refresh")
    @ResponseBody
    public ResponseEntity<?> sprRefresh() { return run(service::purchaseRegisterRefresh, "Could not refresh."); }

    /** cmbBranchName_Leave with a branch ticked. */
    @GetMapping(SPR + "/combos")
    @ResponseBody
    public ResponseEntity<?> sprCombos() { return run(service::purchaseRegisterCombos, "Could not load the filters."); }

    @GetMapping(SPR)
    @ResponseBody
    public ResponseEntity<?> sprRegister(@RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate,
                                         @RequestParam(defaultValue = "0") int parentCategoryId,
                                         @RequestParam(defaultValue = "0") int itemCategoryId,
                                         @RequestParam(defaultValue = "0") int itemTypeId,
                                         @RequestParam(defaultValue = "0") int itemId,
                                         @RequestParam(defaultValue = "0") int grnNoFrom,
                                         @RequestParam(defaultValue = "0") int grnNoTo,
                                         @RequestParam(defaultValue = "0") int gpNoFrom,
                                         @RequestParam(defaultValue = "0") int gpNoTo,
                                         @RequestParam(defaultValue = "0") int poNoFrom,
                                         @RequestParam(defaultValue = "0") int poNoTo,
                                         @RequestParam(defaultValue = "0") int supplierId,
                                         @RequestParam(defaultValue = "0") int warehouseId,
                                         @RequestParam(required = false) String branchIds) {
        return run(() -> service.purchaseRegister(fromDate, toDate, parentCategoryId, itemCategoryId, itemTypeId,
                itemId, grnNoFrom, grnNoTo, gpNoFrom, gpNoTo, poNoFrom, poNoTo, supplierId, warehouseId,
                ids(branchIds)), "Could not run the report.");
    }

    /** kind: print | printWithTax | grn | order | invoice. */
    @GetMapping(SPR + "/slip/{kind}/{id}")
    @ResponseBody
    public ResponseEntity<?> sprSlip(@PathVariable String kind, @PathVariable int id,
                                     @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.purchaseRegisterSlip(kind, id, documentTypeId), "Could not print.");
    }

    // ------------------------------------------------------------ 452 Store Purchase Demand Report

    @GetMapping("/store/reports/store-purchase-demand-register")
    public String storePurchaseDemandRegisterPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/store_purchase_demand_register";
    }

    @GetMapping(PDR + "/lookups")
    @ResponseBody
    public ResponseEntity<?> pdrLookups() { return run(service::demandRegisterLookups, "Could not load the filters."); }

    @GetMapping(PDR + "/combos")
    @ResponseBody
    public ResponseEntity<?> pdrCombos() { return run(service::demandRegisterCombos, "Could not load the filters."); }

    @GetMapping(PDR)
    @ResponseBody
    public ResponseEntity<?> pdrRegister(@RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate,
                                         @RequestParam(defaultValue = "0") int docNoFrom,
                                         @RequestParam(defaultValue = "0") int docNoTo,
                                         @RequestParam(defaultValue = "0") int parentCategoryId,
                                         @RequestParam(defaultValue = "0") int itemCategoryId,
                                         @RequestParam(defaultValue = "0") int itemTypeId,
                                         @RequestParam(defaultValue = "0") int itemId,
                                         @RequestParam(defaultValue = "0") int departmentId,
                                         @RequestParam(defaultValue = "0") int assetId,
                                         @RequestParam(required = false) String status,
                                         @RequestParam(required = false) String approved) {
        return run(() -> service.demandRegister(fromDate, toDate, docNoFrom, docNoTo, parentCategoryId,
                itemCategoryId, itemTypeId, itemId, departmentId, assetId, status, approved),
                "Could not run the report.");
    }

    @GetMapping(PDR + "/slip/{id}")
    @ResponseBody
    public ResponseEntity<?> pdrSlip(@PathVariable int id) {
        return run(() -> service.demandSlip(id), "Could not print.");
    }

    @PostMapping(PDR + "/status")
    @ResponseBody
    public ResponseEntity<?> pdrStatus(@RequestBody StoreReportsADto.DemandStatus body) {
        return run(() -> service.demandStatus(body), "Could not update the status.");
    }

    // ------------------------------------------------------------ 453 Store Issuance Return Report

    @GetMapping("/store/reports/store-issuance-return-register")
    public String storeIssuanceReturnRegisterPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/store_issuance_return_register";
    }

    @GetMapping(SRR + "/lookups")
    @ResponseBody
    public ResponseEntity<?> srrLookups() { return run(service::returnRegisterLookups, "Could not load the filters."); }

    @GetMapping(SRR + "/combos")
    @ResponseBody
    public ResponseEntity<?> srrCombos() { return run(service::returnRegisterCombos, "Could not load the filters."); }

    @GetMapping(SRR)
    @ResponseBody
    public ResponseEntity<?> srrRegister(@RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate,
                                         @RequestParam(defaultValue = "0") int departmentId,
                                         @RequestParam(defaultValue = "0") int itemId,
                                         @RequestParam(defaultValue = "0") int assetId,
                                         @RequestParam(defaultValue = "0") int warehouseId,
                                         @RequestParam(defaultValue = "0") int accountId,
                                         @RequestParam(defaultValue = "0") int fromDocNo,
                                         @RequestParam(defaultValue = "0") int toDocNo) {
        return run(() -> service.returnRegister(fromDate, toDate, departmentId, itemId, assetId, warehouseId,
                accountId, fromDocNo, toDocNo), "Could not run the report.");
    }

    @GetMapping(SRR + "/slip/{id}")
    @ResponseBody
    public ResponseEntity<?> srrSlip(@PathVariable int id) {
        return run(() -> service.returnSlip(id), "Could not print.");
    }

    @GetMapping(SRR + "/voucher")
    @ResponseBody
    public ResponseEntity<?> srrVoucher(@RequestParam(defaultValue = "0") int voucherHeadId,
                                        @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.returnVoucher(voucherHeadId, documentTypeId), "Could not print.");
    }

    // ------------------------------------------------------------ NoOfAttachments links (all three, D8)

    @GetMapping({SPR + "/attachments", PDR + "/attachments", SRR + "/attachments"})
    @ResponseBody
    public ResponseEntity<?> attachments(HttpServletRequest request,
                                         @RequestParam(defaultValue = "0") int id,
                                         @RequestParam(defaultValue = "0") int documentTypeId) {
        String screen = screenOf(request);
        return run(() -> service.attachments(screen, id, documentTypeId), "Could not load the attachments.");
    }

    @GetMapping({SPR + "/attachments/{attachmentId}", PDR + "/attachments/{attachmentId}", SRR + "/attachments/{attachmentId}"})
    @ResponseBody
    public ResponseEntity<?> attachmentDownload(HttpServletRequest request, @PathVariable int attachmentId,
                                                @RequestParam(defaultValue = "0") int id,
                                                @RequestParam(defaultValue = "0") int documentTypeId) {
        try {
            var file = service.attachmentDownload(screenOf(request), id, documentTypeId, attachmentId);
            return ResponseEntity.ok()
                    .header("Content-Disposition", ContentDisposition.attachment()
                            .filename(file.name(), StandardCharsets.UTF_8).build().toString())
                    .header("X-Content-Type-Options", "nosniff")
                    .contentType(MediaType.APPLICATION_OCTET_STREAM)
                    .body(file.bytes());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, "Download failed.")));
        }
    }

    /** Which of the three screens the request came in on (the path segment after /api/store/reports/). */
    private static String screenOf(HttpServletRequest request) {
        String uri = request.getRequestURI();
        if (uri.contains(SPR + "/")) return "store-purchase-register";
        if (uri.contains(PDR + "/")) return "store-purchase-demand-register";
        if (uri.contains(SRR + "/")) return "store-issuance-return-register";
        return "";
    }

    // ------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));

        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static List<Integer> ids(String csv) {
        List<Integer> out = new ArrayList<>();
        if (csv == null) return out;
        for (String s : csv.split(",")) {
            String t = s.trim();
            if (t.isEmpty()) continue;
            try { out.add(Integer.parseInt(t)); } catch (NumberFormatException ignored) { }
        }
        return out;
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
