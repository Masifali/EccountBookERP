package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.Arrays;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Crystal prints of the W3C Production screens (887 Production Costing, 906 Production FOH). The pages print with
 * CrystalPrint.open(key, args) after the screen's own gate (rights + "no rows" refusal).
 *
 * TRACED (the .rpt name and the call are in the decompiled form / BLL):
 *   w3c-921 / w3c-921_01  frmCreateProductionCostingReport.OpenPrint:1538 - ProductionManualCosting_SlipAndRegister
 *                         (@OrganizationId, @CompanyId, @JobOrderId, @DocumentTypeId=921) with the two sub reports
 *                         ProductionManualCosting_InputDetailSubRepirt.rpt / ProductionManualCosting_ByProductDetailSubReport.rpt
 *                         (@OrganizationId, @CompanyId, @JobOrderId). The report names offered are the two 921 templates
 *                         that exist in the report folder; the desktop reads its list from the database
 *                         (CrystalReportPrint_Helper.LoadReportsToDropdown("ProductionManualCosting")).
 *   w3c-921h              frmProductionCostingReport.BtnPrint_Click - 921_I_01_ProductionCostingFormHistoryReport.rpt over
 *                         usp_ProductionCosting_FormHistory (@CompanyId, @FromDate, @ToDate), the rows of the last Show.
 *   w3c-922               CommonServices.ProdcutionFohAllocateToJobOrderPrint922(Id):12987 - 922_ProdcutionFohAllocateToJobOrderSlip.rpt
 *                         over ProdcutionFohAllocateToJobOrder_SlipAndRegister (@OrganizationId, @CompanyId, @FinancialYearId,
 *                         @DocumentTypeId=922, @BranchesId, @Id).
 *   w3c-666_01            frmProductionFohReportCreate.btnPrint_Click:480 - 666_01_MonthlyPlantConsumedHours_Report.rpt over the
 *                         rows DailyPlantConsumedHours.FormHistoryAndReport(@MonthDate) returned.
 */
@Component
public class W3CProductionReports {

    public W3CProductionReports(ReportRegistry registry) {
        registry.register(def("w3c-921", "921-ProductionCostingReport.rpt", "[dbo].[USP_ProductionManualCosting_SlipAndRegister]",
                "frmCreateProductionCostingReport.OpenPrint (921-ProductionCostingReport)", slipParams(), subs()));
        registry.register(def("w3c-921_01", "921-01ProductionCostingReport.rpt", "[dbo].[USP_ProductionManualCosting_SlipAndRegister]",
                "frmCreateProductionCostingReport.OpenPrint (921-01ProductionCostingReport)", slipParams(), subs()));

        registry.register(def("w3c-921h", "921_I_01_ProductionCostingFormHistoryReport.rpt", "[dbo].[usp_ProductionCosting_FormHistory]",
                "frmProductionCostingReport.BtnPrint_Click",
                ps(P("@CompanyId", "session:companyId"), G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   P("rpt:CompanyAddress", "same:@CompanyAddress"), P("rpt:CompanyName", "same:@CompanyName"))));

        registry.register(def("w3c-922", "922_ProdcutionFohAllocateToJobOrderSlip.rpt", "[dbo].[USP_ProdcutionFohAllocateToJobOrder_SlipAndRegister]",
                "CommonServices.ProdcutionFohAllocateToJobOrderPrint922",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FinancialYearId", "session:financialYearId"), P("@DocumentTypeId", "const:922"),
                   G("@BranchesId", "session:branchId"), G("@Id", "arg:id"), P("@IsApproved", "const:0"),
                   P("rpt:CompanyAddress", "same:@CompanyAddress"), P("rpt:CompanyName", "same:@CompanyName"))));

        registry.register(def("w3c-666_01", "666_01_MonthlyPlantConsumedHours_Report.rpt", "[dbo].[USP_DailyPlantConsumedHours_FormHistoryReport]",
                "frmProductionFohReportCreate.btnPrint_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   G("@MonthDate", "arg:monthDate"),
                   P("rpt:CompanyAddress", "same:@CompanyAddress"), P("rpt:CompanyName", "same:@CompanyName"))));
    }

    private static java.util.List<ReportDefinition.Param> slipParams() {
        return ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                G("@JobOrderId", "arg:jobOrderId"), P("@DocumentTypeId", "const:921"),
                P("rpt:CompanyAddress", "same:@CompanyAddress"), P("rpt:CompanyName", "same:@CompanyName"));
    }

    private static java.util.List<ReportDefinition.SubReport> subs() {
        return Arrays.asList(
                sub("ProductionManualCosting_InputDetailSubRepirt.rpt", "[dbo].[USP_ProductionManualCosting_InputDetailSubReport]",
                        ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), G("@JobOrderId", "arg:jobOrderId"))),
                sub("ProductionManualCosting_ByProductDetailSubReport.rpt", "[dbo].[USP_ProductionManualCosting_ByProductDetailSubReport]",
                        ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), G("@JobOrderId", "arg:jobOrderId"))));
    }
}
