package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the Party Processing screens of group PpA, traced from the desktop. Every desktop caller
 * pushes @CompanyName / @CompanyAddress (ReportDefinition adds them). The pages print with
 * CrystalPrint.open(key, args) after the desktop's own row check ("Record Not Found For Display").
 *
 * pp-293      frmPartyProcessingDefineSupplier.grdfrm_ColumnButtonClick(Print) -> GeneralReprots.SupplierCustomerRegister
 *             (SupplierCustomerId = row Id; ApprovedFilter unset -> @IsTaxable = false is sent) -> Sp_SupplierCustomerHistory_rpt.
 * pp-292_01   frmPartyProcessingDefineSupplier.btnPrint_Click -> same BLL with CityId, CountryId (when != 0), GroupAccountId 12 (@GroupId).
 * pp-416_01   StockOpeningBalancePartyProcessing.btnPrint_Click / history Print / after save when "Print" is ticked ->
 *             CommonServices.StockOpeningBalancePartyProcSlipandRegister416_01(Id). That CommonServices body is not in the
 *             decompiled source: the contract below is BLL InvStockOpeningBalancePartyProcessing_SlipandRegister with the
 *             values the form itself uses for this document (DocumentTypeId 121, active year, session branch, Id), and the
 *             template name is not verified (a generated layout prints when no converted template has that name).
 * pp-332/337/338/338gdn  frmGRNGDNPartyProcessing register buttons -> GrnGdnReports.GrnAndGdnSlipAndRegister (the rows of the
 *             last Show): Sp_InvGrnPartyProcessing_RiceSlip_Rpt.
 * pp-333      frmPartyProcessingGrnInfo.GenerateReport(Id) -> GrnAndGdnSlipAndRegister(DocumentTypeId 49, active year, Id).
 * pp-169      frmPartyProcessingGrnInfo.btn334Register_Click -> GrnGdnReports.PartyProcessingGrnInfo: Sp_InvPartyProcessingGrn_Info.
 * pp-300      frmGatePassPartyProcessing.toolStripButton1_Click -> PartyProcessingGatePassSlipandRegister:
 *             Sp_GatePassPartyProcessing_SlipAndRegister_Rpt.
 * pp-466_01..06  frmStockReportPartyProcessing.Register_Click -> StocksReport.PartyProcessingstockGeneralSummaryByWeight:
 *             Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt (one procedure, six templates).
 */
@Component
public class PartyprocessingPpAReports {

    public PartyprocessingPpAReports(ReportRegistry registry) {
        registry.register(def("pp-293", "293-InvRptSupplierSlip.rpt", "Sp_SupplierCustomerHistory_rpt",
                "frmPartyProcessingDefineSupplier.grdfrm_ColumnButtonClick(Print) -> GeneralReprots.SupplierCustomerRegister",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   G("@SupplierCustomerId", "arg:supplierCustomerId"), P("@IsTaxable", "const:0"))));
        registry.register(def("pp-292_01", "292_01-InvRptSupplierRegister.rpt", "Sp_SupplierCustomerHistory_rpt",
                "frmPartyProcessingDefineSupplier.btnPrint_Click -> GeneralReprots.SupplierCustomerRegister",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@GroupId", "const:12"), G("@CountryId", "arg:countryId"), G("@CityId", "arg:cityId"),
                   P("@IsTaxable", "const:0"))));
        registry.register(def("pp-416_01", "416_01-StockOpeningBalancePartyProcessing.rpt",
                "[dbo].[Sp_InvStockOpeningBalancePartyProcessing_SlipandRegister]",
                "StockOpeningBalancePartyProcessing.btnPrint_Click -> CommonServices.StockOpeningBalancePartyProcSlipandRegister416_01",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"),
                   P("@DocumentTypeId", "const:121"), G("@Id", "arg:id"))));

        String[][] grn = {
                { "pp-332", "332-Sp_InvGrnPartyProcessing_RiceRegister.rpt", "frmGRNGDNPartyProcessing.toolStripButton1_Click" },
                { "pp-337", "337-GrnPartyProcessing_RiceRegister-Format-2.rpt", "frmGRNGDNPartyProcessing.btnGrnRegister337_Click" },
                { "pp-338", "338-GrnPartyProcessing_RiceRegister-Format-3.rpt", "frmGRNGDNPartyProcessing.btnGrnRegister338_Click" },
                { "pp-338gdn", "338-GdnPartyProcessing_RiceRegister-Format-2.rpt", "frmGRNGDNPartyProcessing.btnGdnRegister338_Click" } };
        for (String[] g : grn) {
            registry.register(def(g[0], g[1], "[dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]", g[2] + " -> GrnGdnReports.GrnAndGdnSlipAndRegister",
                    ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                       P("@FinancialYearId", "session:financialYearId"), P("@DocumentTypeId", "arg:documentTypeId"),
                       G("@BranchesId", "session:branchId"), G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                       G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@StockPartyId", "arg:stockPartyId"),
                       G("@DocNoFrom", "arg:docNoFrom"), G("@DocNoTo", "arg:docNoTo"), G("@ItemId", "arg:itemId"),
                       P("@IsApproved", "const:0"))));
        }
        registry.register(def("pp-333", "333-InvRptGoodsReceiptsNotesRiceSlip.rpt", "[dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]",
                "frmPartyProcessingGrnInfo.GenerateReport -> GrnGdnReports.GrnAndGdnSlipAndRegister",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FinancialYearId", "session:financialYearId"), P("@DocumentTypeId", "const:49"),
                   G("@Id", "arg:id"), P("@IsApproved", "const:0"))));
        registry.register(def("pp-169", "169-InvPartyProcessingGrnInfo.rpt", "Sp_InvPartyProcessingGrn_Info",
                "frmPartyProcessingGrnInfo.btn334Register_Click -> GrnGdnReports.PartyProcessingGrnInfo",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@DocumentTypeId", "const:49"), G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:stockPartyId"), G("@ReferencePartyId", "arg:refPartyId"),
                   G("@WarHouseId", "arg:warehouseId"), G("@JobLotId", "arg:jobLotId"),
                   G("@DocNoFrom", "arg:docNoFrom"), G("@DocNoTo", "arg:docNoTo"),
                   G("@GPNoFrom", "arg:gpNoFrom"), G("@GPNoTo", "arg:gpNoTo"),
                   G("@GrnTypeId", "arg:grnTypeId"), G("@ItemId", "arg:itemId"))));
        registry.register(def("pp-300", "300-RptPartyProcessingGatePassRegister.rpt", "[dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]",
                "frmGatePassPartyProcessing.toolStripButton1_Click -> PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FinancialYearId", "session:financialYearId"), G("@DocumentTypeId", "arg:documentTypeId"),
                   G("@StockPartyId", "arg:stockPartyId"), G("@ReferencePartyId", "arg:refPartyId"),
                   G("@DocNoFrom", "arg:docNoFrom"), G("@DocNoTo", "arg:docNoTo"),
                   G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   G("@Status", "arg:status"), P("@IsApproved", "const:0"), G("@GatePassType", "arg:gatePassType"))));

        String[][] stock = {
                { "pp-466_01", "466_01-ItemStockSummaryPartyProcessing.rpt" },
                { "pp-466_02", "466_02-ItemandWarehouseStockSummaryPartyProcessing.rpt" },
                { "pp-466_03", "466_03-ItemandCropYearStockSummaryPartyProcessing.rpt" },
                { "pp-466_04", "466_04-JobLotandItemStockSummaryPartyProcessing.rpt" },
                { "pp-466_05", "466_05-ItemandCropYearandWarehouseStockSummaryPartyProcessing.rpt" },
                { "pp-466_06", "466_06-WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing.rpt" } };
        for (String[] s : stock) {
            registry.register(def(s[0], s[1], "Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt",
                    "frmStockReportPartyProcessing.Register_Click -> StocksReport.PartyProcessingstockGeneralSummaryByWeight",
                    ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                       G("@DateFrom", "arg:fromDate"), G("@DateTo", "arg:toDate"),
                       G("@ItemTypeId", "arg:itemTypeId"), G("@ItemCategoryId", "arg:itemCategoryId"),
                       G("@CropYear", "arg:cropYear"), G("@JobLotId", "arg:jobLotId"), G("@WarehouseId", "arg:warehouseId"),
                       G("@ItemId", "arg:itemId"), G("@IsPackSizeOn", "arg:isPackSizeOn"), G("@IsPackTypeOn", "arg:isPackTypeOn"),
                       G("@StockPartyId", "arg:stockPartyId"), G("@SkipZero", "arg:skipZero"),
                       P("@Activity", "arg:activity"))));
        }
    }
}
