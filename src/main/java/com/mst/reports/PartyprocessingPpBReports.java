package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the Party Processing screens of group PpB (traced from the desktop).
 *
 * ppb-333   PartyProGrnAndGdn (GRN, DocumentTypeId 49) printToolStripButton / history Print / after save when "Print" is ticked
 *           -> CrystalReportPrint_Helper.GrnSlipPartyProcessing333(Id) -> "333-InvRptGoodsReceiptsNotesRiceSlip.rpt" with the rows of
 *           Sp_InvGrnPartyProcessing_RiceSlip_Rpt (@OrganizationId @CompanyId @FinancialYearId @DocumentTypeId 49 @Id).
 *           The page only calls it after /api/party-processing/grn-gdn/slip has checked the Print right and the record.
 */
@Component
public class PartyprocessingPpBReports {

    public PartyprocessingPpBReports(ReportRegistry registry) {
        registry.register(def("ppb-333", "333-InvRptGoodsReceiptsNotesRiceSlip.rpt", "[dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]",
                "PartyProGrnAndGdn.printToolStripButton_Click -> CrystalReportPrint_Helper.GrnSlipPartyProcessing333",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FinancialYearId", "session:financialYearId"), P("@DocumentTypeId", "const:49"),
                   G("@Id", "arg:id"))));
    }
}
