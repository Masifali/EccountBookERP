package com.mst.controllers;

import com.mst.services.InvoiceHistoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.Map;

/**
 * Form History for the eight Sale / Purchase invoice screens.
 *
 * GET /api/invoice-history/{screen}
 *   screen = purchase-invoice | purchase-invoice-direct | purchase-invoice-grn-direct
 *          | purchase-invoice-return | sale-invoice | sale-invoice-direct
 *          | sale-invoice-gdn-no-wb | sale-invoice-return
 *
 * The screen key is what picks the DocumentTypeId and the procedure, so a caller cannot ask one
 * screen for another screen's documents. There is deliberately NO documentTypeId parameter, NO
 * userId parameter and NO "view all" flag: the desktop derives all three from the signed-in user
 * and the screen's grant grid, and so does the service.
 */
@RestController
@RequestMapping("/api/invoice-history")
public class InvoiceHistoryRestController {

    @Autowired
    private InvoiceHistoryService service;

    /**
     * @param dateMode doc | entry | modify | approved - the desktop's four mutually exclusive
     *                 radios. A blank from or to is OMITTED, never defaulted to today.
     * @param branchesIds CSV of BranchId. The desktop only runs Purchase history when a branch is
     *                    chosen (the call sits inside `if (cmbBranchName.Text != string.Empty)`),
     *                    so the screen should send one.
     */
    @GetMapping("/{screen}")
    public ResponseEntity<Map<String, Object>> history(
            @PathVariable String screen,
            @RequestParam(defaultValue = "doc") String dateMode,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "0") int fromDocNo,
            @RequestParam(defaultValue = "0") int toDocNo,
            @RequestParam(defaultValue = "0") int supplierCustomerId,
            @RequestParam(defaultValue = "0") int paymentTermId,
            @RequestParam(required = false) String branchesIds,
            @RequestParam(defaultValue = "0") int noOfRecords) {

        InvoiceHistoryService.Filter f = new InvoiceHistoryService.Filter();
        f.dateMode = dateMode;
        f.fromDate = fromDate;
        f.toDate = toDate;
        f.fromDocNo = fromDocNo;
        f.toDocNo = toDocNo;
        f.supplierCustomerId = supplierCustomerId;
        f.paymentTermId = paymentTermId;
        f.branchesIds = branchesIds;
        f.noOfRecords = noOfRecords;

        Map<String, Object> res = service.history(InvoiceHistoryService.Screen.of(screen), f);
        return Boolean.TRUE.equals(res.get("success"))
                ? ResponseEntity.ok(res)
                : ResponseEntity.status(400).body(res);
    }
}
