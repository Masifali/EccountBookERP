package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.Arrays;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Crystal prints of the Party Processing screens of group PpC (676, 677, 675, 602, 603, 604, 614). The pages print with
 * CrystalPrint.open(key, args) after the screen's own print gate (/api/party-processing/{page}/print). The history
 * "Voucher" buttons use the existing "acc-118" (118-AcRptVoucherSlip.rpt) with the voucher head id.
 *
 * TRACED (the .rpt name and the call are in the decompiled form / BLL):
 *   ppc-601_02  frmProductionPartyProcessing GenerateReportPM -> 601_02-InvFoodPackingMaterialSlip.rpt,
 *               USP_FoodProductionPartyProcessingPackingMaterial_SlipAndRegister (active year, DocumentTypeId 0, Id).
 *   ppc-608     btnSummeryReport_Click -> 608-InvRptProductionSummeryPartyProcessing.rpt, Sp_InvFoodProductionPartyProcessing_Summery_Rpt.
 *   ppc-609     btn609ProductionIssuance_Click -> 609-FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId.rpt.
 *   ppc-611     ProductionPartyProcessingBillReport_611(JobOrderId, Id): the three procedures are BLL 0302
 *               InvProductionProcessingBill_PartyProcessingModule_Report_611 / _SubReport_611 / ByProduct_SubReport_611
 *               (all by @JobOrderId); the helper body is not in the decompiled source.
 *
 * INFERRED (CrystalReportPrint_Helper / CommonServices bodies are not in the decompiled source; the procedure is the
 * party-processing slip procedure of the document, called with the values the form has; the template names are NOT
 * verified - a generated layout prints when no converted template has that name):
 *   ppc-626     CommonServices.ProductionJobOrderPartyProcessing_626(Id) -> USP_ProductionJobOrderPartyProcessing_SlipandRegister.
 *   ppc-601_01  CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(Id) -> Sp_InvFoodProductionPartyProcessing_Rpt.
 *   ppc-600     CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600(Id) -> SpInvStockConversionPartyProcessing_Summery_Rpt.
 *   ppc-220     CommonServices.StockTransferPartyProcessingSlip(Id) -> Sp_InvStockTransferPartyProcessing_SlipandRegister (220).
 *   ppc-158     CommonServices.StockAdjustmentPartyProcessingSlip(Id) -> Sp_StockAdjustmentPartyProcessingSlipAndRegister.
 *   ppc-219     CommonServices.ContractorWagesPartyProcessingSlip(Id) -> Sp_InvContractorWagesBillHeader_SlipandRegister (219).
 */
@Component
public class PartyprocessingPpCReports {

    public PartyprocessingPpCReports(ReportRegistry registry) {
        registry.register(def("ppc-601_02", "601_02-InvFoodPackingMaterialSlip.rpt",
                "[dbo].[USP_FoodProductionPartyProcessingPackingMaterial_SlipAndRegister]",
                "frmProductionPartyProcessing.GenerateReportPM",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FinancialYearId", "session:financialYearId"), P("@DocumentTypeId", "const:0"), G("@Id", "arg:id"))));
        registry.register(def("ppc-608", "608-InvRptProductionSummeryPartyProcessing.rpt", "Sp_InvFoodProductionPartyProcessing_Summery_Rpt",
                "frmProductionPartyProcessing.btnSummeryReport_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@JobOrderId", "arg:jobOrderId"), G("@LanguageId", "arg:languageId"))));
        registry.register(def("ppc-609", "609-FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId.rpt",
                "Sp_InvFoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_rpt",
                "frmProductionPartyProcessing.btn609ProductionIssuance_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@InvJobOrderId", "arg:jobOrderId"))));
        registry.register(def("ppc-611", "611-ProductionPartyProcessingBill.rpt", "spInvProductionProcessingBill_PartyProcessingModule_Report",
                "ProductionPartyProcessingBill.Print_Click -> CrystalReportPrint_Helper.ProductionPartyProcessingBillReport_611",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), P("@JobOrderId", "arg:jobOrderId")),
                Arrays.asList(
                        sub("611-ProductionPartyProcessingBill_SubReport.rpt", "spInvProductionProcessingBill_PartyProcessingModule_SubReport",
                                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), P("@JobOrderId", "arg:jobOrderId"))),
                        sub("611-ProductionPartyProcessingBillByProduct_SubReport.rpt", "Usp_ProductionProcessingBillOutPut_SubReport",
                                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), P("@JobOrderId", "arg:jobOrderId"))))));

        registry.register(def("ppc-626", "626-ProductionJobOrderPartyProcessing.rpt", "USP_ProductionJobOrderPartyProcessing_SlipandRegister",
                "ProductionJobOrderPartyProcessing.GenerateReport -> CommonServices.ProductionJobOrderPartyProcessing_626 (inferred)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@DocumentTypeId", "const:120"), G("@Id", "arg:id"))));
        registry.register(def("ppc-601_01", "601_01-ProductionPartyProcessingSlip.rpt", "Sp_InvFoodProductionPartyProcessing_Rpt",
                "frmProductionPartyProcessing -> CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01 (inferred)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FinancialYearId", "session:financialYearId"), G("@Id", "arg:id"))));
        registry.register(def("ppc-600", "600-StockConversionPartyProcessingSummery.rpt", "SpInvStockConversionPartyProcessing_Summery_Rpt",
                "invfrmStockConversionPartyProcessing -> CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600 (inferred)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), G("@Id", "arg:id"))));
        registry.register(def("ppc-220", "StockTransferPartyProcessingSlip.rpt", "Sp_InvStockTransferPartyProcessing_SlipandRegister",
                "StockTransfer (Party Processing) -> CommonServices.StockTransferPartyProcessingSlip (inferred)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@DocumentTypeId", "const:220"), G("@Id", "arg:id"))));
        registry.register(def("ppc-158", "StockAdjustmentPartyProcessingSlip.rpt", "Sp_StockAdjustmentPartyProcessingSlipAndRegister",
                "StockAdjustment (Party Processing) -> CommonServices.StockAdjustmentPartyProcessingSlip (inferred)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"), G("@Id", "arg:id"))));
        registry.register(def("ppc-219", "ContractorWagesPartyProcessingSlip.rpt", "Sp_InvContractorWagesBillHeader_SlipandRegister",
                "frmWagesBillPartyProcessing.SlipPrint_002 -> CommonServices.ContractorWagesPartyProcessingSlip (inferred)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@DocumentTypeId", "const:219"), G("@Id", "arg:id"))));
    }
}
