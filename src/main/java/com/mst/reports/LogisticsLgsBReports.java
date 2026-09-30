package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the LgsB logistics screens, traced from the desktop BLL (the page opens them with
 * CrystalPrint.open(key, {id})):
 *
 * lgsb-1303    frmLogisticPurchaseOrder.btnPrint / history Print / Preview -> CrystalReportPrint_Helper.PurchaseOrderHeader_Slip_1303
 *              -> PurchaseOrderHeader.PurchaseOrderHeader_Slip: [lgstcm].[USP_PurchaseOrderHeader_Slip] @OrganizationId, @CompanyId,
 *              @BranchesId, @FinancialYearId, @Id (all always sent).
 * lgsb-1304    frmLogisticPurchaseServicesBill.btnPrint / history Print / Preview 1304 -> ServicesBillHeader_Slip_1304 ->
 *              ServicesBillHeader.ServicesBillHeader_Slip: [lgstcm].[USP_ServicesBillHeader_Slip] (same five parameters).
 * lgsb-1305    frmFreightVoucherExport.btnPrint / history Print / Preview 1305 -> FreightVoucherOutward_Slip_1305 ->
 *              FreightVoucherOutward.FreightVoucherOutward_Slip: [lgstcm].[USP_FreightVoucherOutward_Slip] @OrganizationId,
 *              @CompanyId, @BranchesId, @FinancialYearId, @Id when > 0.
 * lgsb-1305-01 frmFreightVoucherExportMultiVehicles.btnPrint / history Print -> FreightVoucherOutward_Slip_1305_01 -> the same
 *              procedure with the master document id in @MasterDocId (HeaderId when > 0).
 *
 * The voucher prints reuse the traced contracts already registered: hrm-102 (ANewAcRptPaymentReceiptsVoucherSlip_102, @Id or
 * @Ids) and acc-118 (VoucherReport_118, @Id, @DocumentTypeId). The 1305 / 1305_01 .rpt file names are not visible in the
 * desktop source (the helper that names them is not decompiled); the names below follow the 1303 / 1304 pattern.
 */
@Component
public class LogisticsLgsBReports {

    public LogisticsLgsBReports(ReportRegistry registry) {
        registry.register(def("lgsb-1303", "1303_PurchaseOrderHeader_Slip.rpt", "[lgstcm].[USP_PurchaseOrderHeader_Slip]",
                "frmLogisticPurchaseOrder -> CrystalReportPrint_Helper.PurchaseOrderHeader_Slip_1303",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"), P("@Id", "arg:id"))));
        registry.register(def("lgsb-1304", "1304_ServicesBillHeader_Slip.rpt", "[lgstcm].[USP_ServicesBillHeader_Slip]",
                "frmLogisticPurchaseServicesBill -> CrystalReportPrint_Helper.ServicesBillHeader_Slip_1304",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"), P("@Id", "arg:id"))));
        registry.register(def("lgsb-1305", "1305_FreightVoucherOutward_Slip.rpt", "[lgstcm].[USP_FreightVoucherOutward_Slip]",
                "frmFreightVoucherExport -> CrystalReportPrint_Helper.FreightVoucherOutward_Slip_1305",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"), G("@Id", "arg:id"))));
        registry.register(def("lgsb-1305-01", "1305_01_FreightVoucherOutward_Slip.rpt", "[lgstcm].[USP_FreightVoucherOutward_Slip]",
                "frmFreightVoucherExportMultiVehicles -> CrystalReportPrint_Helper.FreightVoucherOutward_Slip_1305_01",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"), G("@MasterDocId", "arg:id"))));
    }
}
