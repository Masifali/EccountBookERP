package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the Export Reports group N pages (237 / 248 / 259 / 264 / 268 / 271 / 272), traced from the desktop and
 * registered from this component so ReportRegistry.java is not edited (same pattern as HrmReportSupport / ExportReportSupport).
 * The pages print with CrystalPrint.open(key, args) -> POST /api/reports/{key}/print.pdf; arg:<name> are the JSON keys sent.
 *
 * Every desktop launch site below prints the DataTable its own grid query returned (Reporting.ShowReportWithDataTable), so each
 * contract re-runs that query with the same filters. Keys that already exist as seeded contracts (05_seed_report_contracts_bll.sql)
 * are registered again here with the procedure's real parameter list (the seed carries @BranchesId / @ProjectsId / @GroupBy, which
 * these procedures do not declare; they are GUARDED and never sent by these pages, so the result is identical).
 *
 *   exp-289-bankgd   289-BankGdsSummaryRegister.rpt   BankGdSummary.btnPrint_Click (:237)            - the seed 289-bankgdssummaryregister
 *                    lacks @FromToBalance (the BLL names it "@FromToBalance " with a trailing space)
 *   506-eximbilloflading-register                     frmBillofLadingSlipandRegister.ShowRegister (:343)
 *   543-usp-exportdetailbycontract-reportwithcontainers / 544-...withoutcontainers   PreInvoiceRegister (:470 / :493)
 *   558-exportinvoiceagainstforwarding-register       CommercialInvoiceAgainstForwardingPreInvoices (:487)
 *   (523 row print of 271 uses the existing exp-523; 536 of 248 uses the existing exp-536.)
 */
@Component
public class ExportReportsNPrints {

    private static final ReportDefinition.Param ORG = P("@OrganizationId", "session:organizationId");
    private static final ReportDefinition.Param CO = P("@CompanyId", "session:companyId");

    public ExportReportsNPrints(ReportRegistry registry) {

        registry.register(def("exp-289-bankgd", "289-BankGdsSummaryRegister.rpt", "USP_BankGdsSummary",
                "BankGdSummary.btnPrint_Click (:237) -> ExportReports.BankGdSummary (BLL 0138 :1247)",
                ps(ORG, CO, P("@BranchId", "session:branchId"), G("@FromToBalance", "arg:fromToBalance"))));

        registry.register(def("506-eximbilloflading-register", "506-ExImBillOfLading_register.rpt", "Sp_ExImBillOfLading_SlipAndRegister_Rpt",
                "frmBillofLadingSlipandRegister.ShowRegister (:343) -> ExImBillOfLading.ExImBillOfLading_SlipAndRegister",
                ps(ORG, CO, G("@ExImInvoiceId", "arg:eximInvoiceId"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                   G("@ShippingLineId", "arg:shippingLineId"))));

        for (String[] k : new String[][] {
                { "543-usp-exportdetailbycontract-reportwithcontainers", "543-USP_ExportDetailByContract_ReportWithContainers.rpt", "PreInvoiceRegister.btnRegisterPrint543_Click (:470)" },
                { "544-usp-exportdetailbycontract-reportwithoutcontainers", "544-USP_ExportDetailByContract_ReportWithoutContainers.rpt", "PreInvoiceRegister.btnRegisterPrint544_Click (:493)" } }) {
            registry.register(def(k[0], k[1], "USP_ExportDetailByContract_Report",
                    k[2] + " -> ExImLcOrder.ExportShipmentDetailReportByContract (BLL 0469 :1417)",
                    ps(ORG, CO, G("@ContractDateFrom", "arg:fromDate"), G("@ContractDateTo", "arg:toDate"),
                       G("@CustomerId", "arg:supplierCustomerId"), G("@ExImLcOrderId", "arg:exImLcOrderId"),
                       G("@ItemTypeId", "arg:itemTypeId"), G("@ItemId", "arg:itemId"), G("@ActionId", "arg:zeroBalanceType"))));
        }

        registry.register(def("558-exportinvoiceagainstforwarding-register", "558-ExportInvoiceAgainstForwarding_Register.rpt",
                "USP_ExportInvoiceAgainstForwarding_Register",
                "CommercialInvoiceAgainstForwardingPreInvoices.btnRegisterPrint543_Click (:487) -> ExImLcOrder.ExportInvoiceAgainstForwarding_Register (BLL 0469 :1537)",
                ps(ORG, CO, G("@ContractDateFrom", "arg:fromDate"), G("@ContractDateTo", "arg:toDate"),
                   G("@CustomerId", "arg:supplierCustomerId"), G("@ExImLcOrderId", "arg:exImLcOrderId"),
                   G("@ItemTypeId", "arg:itemTypeId"), G("@ItemId", "arg:itemId"))));
    }
}
