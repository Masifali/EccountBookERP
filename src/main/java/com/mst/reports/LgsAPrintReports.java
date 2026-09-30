package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.Collections;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Prints of the logistics screens 233 / 937 / 941 / 939, keyed lgsa-*, opened by the pages with
 * CrystalPrint.open(key, {id}) after the page's own right / ownership check.
 *
 * The desktop launchers (CommonServices.ServicesBillSlip540, CrystalReportPrint_Helper.LogisticRateNegotiationSlip_1300 /
 * LogisticRateNegotiationTransporterSlip_1302 / AgreementHeader_Slip_1301) are not in the recovered source; the data
 * contracts below are the BLL methods those prints read:
 *
 * lgsa-540   ServicesReports.ServicesBill_SlipAndRegister (BLL 0117): Sp_ExImClearingAgentBill_Rpt @OrganizationId, @CompanyId,
 *            @Id when != 0 (the other filters are guarded and not set by a slip).
 * lgsa-1300  logisticRateNegotiationHeader.logisticRateNegotiationHeader_Slip (BLL 0391): [lgstcm].[USP_logisticRateNegotiationHeader_Slip]
 *            @OrganizationId, @CompanyId, @BranchesId, @FinancialYearId, @Id; sub-report data
 *            logisticRateNegotiation_SourceDocumentSubReport: [lgstcm].[USP_logisticRateNegotiation_SourceDocumentSubReport] @logisticRateNegotiationHeaderId.
 * lgsa-1302  the same slip procedure for the transporter document (DocumentTypeId 1302).
 * lgsa-1301  AgreementHeader.AgreementHeader_Slip (BLL 0390): [lgstcm].[USP_AgreementHeader_Slip] @OrganizationId, @CompanyId, @BranchesId,
 *            @FinancialYearId, @Id.
 *
 * The .rpt names follow the helper names (13xx_&lt;Name&gt;_Slip, as 1303_PurchaseOrderHeader_Slip / 1304_ServicesBillHeader_Slip);
 * when no converted template carries the name, ReportPrintController lays the procedure's rows out itself.
 */
@Component
public class LgsAPrintReports {

    public LgsAPrintReports(ReportRegistry registry) {
        registry.register(def("lgsa-540", "540-ServicesBillSlip.rpt", "Sp_ExImClearingAgentBill_Rpt",
                "ExImClearingAgentBillDirect.Slip420 -> CommonServices.ServicesBillSlip540 -> ServicesReports.ServicesBill_SlipAndRegister",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), G("@Id", "arg:id"))));
        registry.register(def("lgsa-1300", "1300_LogisticRateNegotiationSlip.rpt", "[lgstcm].[USP_logisticRateNegotiationHeader_Slip]",
                "frmLogisticRateNegotiation.LogisticRateNegotiationSlip -> CrystalReportPrint_Helper.LogisticRateNegotiationSlip_1300",
                slipParams(),
                Collections.singletonList(sub("LogisticRateNegotiationSourceDocumentSubReport.rpt",
                        "[lgstcm].[USP_logisticRateNegotiation_SourceDocumentSubReport]",
                        ps(P("@logisticRateNegotiationHeaderId", "arg:id"))))));
        registry.register(def("lgsa-1302", "1302_LogisticRateNegotiationTransporterSlip.rpt", "[lgstcm].[USP_logisticRateNegotiationHeader_Slip]",
                "frmLogisticRateNegotiationTransporter.LogisticRateNegotiationSlip -> CrystalReportPrint_Helper.LogisticRateNegotiationTransporterSlip_1302",
                slipParams()));
        registry.register(def("lgsa-1301", "1301_AgreementHeader_Slip.rpt", "[lgstcm].[USP_AgreementHeader_Slip]",
                "frmLogisticAgreement.AgreementHeader_Slip -> CrystalReportPrint_Helper.AgreementHeader_Slip_1301",
                slipParams()));
    }

    private static java.util.List<ReportDefinition.Param> slipParams() {
        return ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"), P("@Id", "arg:id"));
    }
}
