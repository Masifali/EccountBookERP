package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** R4 reports - the five new desktop-faithful pages (screens 56, 76, 87, 932, 933). */
@Controller
public class AcRpt4PageController {
    /** Screen 56 AuditDashboard - "Un_Balanced Voucher Report". */
    @GetMapping("/accounts/reports/un-balanced-voucher-report")
    public String audit() { return "accounts/reports/acrpt4_un_balanced_voucher"; }

    /** Screen 76 PdcInventoryReport - "Pdc Inventory Report". */
    @GetMapping("/accounts/reports/pdc-inventory-report")
    public String pdc() { return "accounts/reports/acrpt4_pdc_inventory"; }

    /** Screen 87 frmPostDatedChequeReports - "Post Dated Cheque Reports". */
    @GetMapping("/accounts/reports/post-dated-cheque-reports")
    public String postDatedCheque() { return "accounts/reports/acrpt4_post_dated_cheque"; }

    /** Screen 932 frmAuditByWeightReport - "5016 Export Shipment Weight Audit" (Audit_Dashboard). */
    @GetMapping("/audit/reports/export-shipment-weight-audit")
    public String weightAudit() { return "accounts/reports/acrpt4_export_weight_audit"; }

    /** Screen 933 frmGrnAudit_History - "Goods Receipts Audit" (Inventory_Reports). */
    @GetMapping("/inventory/reports/grn-audit-history")
    public String grnAudit() { return "accounts/reports/acrpt4_grn_audit_history"; }
}
