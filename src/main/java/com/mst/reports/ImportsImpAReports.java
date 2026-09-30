package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.Collections;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Crystal prints of the Import screens 525 / 221 and registers 392-395, traced from the desktop. The pages check the
 * record / rights through their own API first ("Record Not Found For Display") and then print with
 * CrystalPrint.open(key, args). @CompanyName / @CompanyAddress are supplied by ReportDataService.
 *
 * impa-231  ImLcOrder.GenerateSlip(PrintId) -> ImLcOrder.ImLcOrder_SlipAndRegister: USP_ImLcOrder_Register @OrganizationId,
 *           @CompanyId, @BranchesId, @FinancialYearId, @LcOrderId (only when != 0; dates / doc nos / status / supplier / item are
 *           guarded and never set by this caller); sub-report ImLcOrderPaymentDetail_SubReport.rpt <-
 *           USP_ImLcOrderPaymentDetail_SubReport @ImLcOrderId.
 * impa-806  ImCommercialInvoice.btnPrint_Click / history Print -> ImportReports.ImInvoiceSlip_806: Sp_ImInvoice_SlipandRegister
 *           @OrganizationId, @CompanyId, @Id (only when != 0); ApprovedFilter "All" -> @IsApproved not sent.
 * impa-807  ImportInvoiceRegister.print_Click (dtgrid of btnshow): SP_ImInvoiceRegister_WithAvgRates @OrganizationId, @CompanyId,
 *           @FromDate, @ToDate, @SupplierCustomerId / @ItemId only when != 0.
 * impa-808  ImportContractRegister.print_Click: Sp_ImLcOrderNo_ImportSlip_Rpt @OrganizationId, @CompanyId, @FromDate, @ToDate,
 *           @SupplierCustomerId / @ItemId only when != 0 (the BLL's @IsApproved is not a parameter of the procedure - left out).
 * impa-811  ImportGrnRegister.print_Click: Sp_ImGRN_SlipAndRegister @OrganizationId, @CompanyId, @DocumentTypeId 234, @FromDate,
 *           @ToDate, @SupplierCustomerId / @ItemId only when != 0.
 * impa-810  ImportPurchaseRegister.print_Click: Sp_ImportPurchaseOrder_Slip_rpt @OrganizationId, @CompanyId, @DocumentTypeId 236,
 *           @FromDate, @ToDate, @SupplierCustomerId / @ItemId only when != 0, @IsApproved false (ApprovedFilter unset).
 * (526's 239-ImLcOrderSchedule_FormHistory.rpt prints the checked history rows - a grid print, POST /reports/print/grid.)
 */
@Component
public class ImportsImpAReports {

    public ImportsImpAReports(ReportRegistry registry) {
        registry.register(def("impa-231", "231-ImLcOrder-Slip.rpt", "[dbo].[USP_ImLcOrder_Register]",
                "ImLcOrder.GenerateSlip -> ImLcOrder.ImLcOrder_SlipAndRegister",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@BranchesId", "session:branchId"),
                   P("@FinancialYearId", "session:financialYearId"),
                   G("@LcOrderId", "arg:id")),
                Collections.singletonList(sub("ImLcOrderPaymentDetail_SubReport.rpt", "[dbo].[USP_ImLcOrderPaymentDetail_SubReport]",
                        ps(P("@ImLcOrderId", "arg:id"))))));
        registry.register(def("impa-806", "806-ImInvoiceSlip.rpt", "Sp_ImInvoice_SlipandRegister",
                "ImCommercialInvoice.btnPrint_Click -> ImportReports.ImInvoiceSlip_806",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   G("@Id", "arg:id"))));
        registry.register(def("impa-807", "807-ImInvoiceRegister_WithAvgRates.rpt", "SP_ImInvoiceRegister_WithAvgRates",
                "ImportInvoiceRegister.print_Click -> ImportReports.ImInvoiceRegister_WithAvgRates",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@FromDate", "arg:fromDate"),
                   P("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:supplierId"),
                   G("@ItemId", "arg:itemId"))));
        registry.register(def("impa-808", "808-ImportContractRegister.rpt", "Sp_ImLcOrderNo_ImportSlip_Rpt",
                "ImportContractRegister.print_Click -> ImportReports.ImportContractSlipAndRegister",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@FromDate", "arg:fromDate"),
                   P("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:supplierId"),
                   G("@ItemId", "arg:itemId"))));
        registry.register(def("impa-811", "811-ImportGrnRegister.rpt", "Sp_ImGRN_SlipAndRegister",
                "ImportGrnRegister.print_Click -> ImportReports.ImportGrnSlipAndRegister",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@DocumentTypeId", "const:234"),
                   P("@FromDate", "arg:fromDate"),
                   P("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:supplierId"),
                   G("@ItemId", "arg:itemId"))));
        registry.register(def("impa-810", "810-ImportPurchaseOrder.rpt", "Sp_ImportPurchaseOrder_Slip_rpt",
                "ImportPurchaseRegister.print_Click -> ImportReports.ImportPurchaseOrderSlip",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@DocumentTypeId", "const:236"),
                   P("@FromDate", "arg:fromDate"),
                   P("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:supplierId"),
                   G("@ItemId", "arg:itemId"),
                   P("@IsApproved", "const:0"))));
    }
}
