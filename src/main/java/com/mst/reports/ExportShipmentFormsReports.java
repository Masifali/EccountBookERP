package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.Collections;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Crystal prints of the five Export shipment forms (agent P2), registered from this component so ReportRegistry.java is not
 * edited (the ExportReportSupport / HrmReportSupport pattern). Pages print with CrystalPrint.open(key, args).
 * Procedures and parameters checked against procdure_index.csv / procdure.utf8.sql.
 *
 *  exp-466   ExImGoodsReceiptsAtPort.PrintRegister_Click - the desktop prints its history DataTable (Sp_ExImGoodsReceiptsAtPort_GetAllMetohd
 *            'FormHistory'); the contract runs the BLL's own register procedure USP_ExImGoodsReceiptsAtPort_Register for the same
 *            organisation / company / branch (same rows; it names the warehouse column "Warehouse" and has no EntryUser / ModifyUser
 *            names). "@CompanyName" / "@CompanyAddress" with the @ (default pair).
 *  exp-475   CommonServices.StoreIssuanceHeader_Slip475(Id) -> InvGsStoreIssuanceHeader.StoreIssuanceHistory (@Id only).
 *  exp-248   frmDhlTracking.btnRegister_Click - the desktop prints dtHistory (USP_DHLTracking_GetAllMethod 'FormHistory', table 0);
 *            the contract re-runs it with the filters the page last showed (args), CanViewAllRecord 1, DocumentTypeId 248.
 *  exp-319 / exp-319a  CommonServices.SaleInvoiceDirectSlip_294("319-/319A-ExportSaleInvoiceSlip.rpt", Id) ->
 *            InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice (sp_InvSaleInvoiceDirectSlip @OrganizationId @CompanyId [@Id]) with the
 *            sub report "InvRptPurchaseBillSupplierOthers" (SalesCustomerBillSubReport -> "SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep"
 *            @PihId); company pair pushed WITHOUT the @.
 *  103 voucher (acc-103) and 118 voucher (acc-118) are existing ReportRegistry keys.
 */
@Component
public class ExportShipmentFormsReports {

    private static final ReportDefinition.Param ORG = P("@OrganizationId", "session:organizationId");
    private static final ReportDefinition.Param CO = P("@CompanyId", "session:companyId");

    public ExportShipmentFormsReports(ReportRegistry registry) {
        registry.register(def("exp-466", "466-ExportGoodsReceiptsAtPortRegister.rpt", "USP_ExImGoodsReceiptsAtPort_Register",
                "ExImGoodsReceiptsAtPort.PrintRegister_Click (BLL ExImGoodsReceiptsAtPort_Register)",
                ps(ORG, CO, P("@BranchesId", "session:branchId"), G("@Id", "arg:id"))));

        registry.register(def("exp-475", "475-RptInvGsStoreIssuanceHeader_Slip.rpt", "Sp_InvGsStoreIssuanceHeader_SlipandRegister",
                "CommonServices.StoreIssuanceHeader_Slip475 (:8061) -> InvGsStoreIssuanceHeader.StoreIssuanceHistory",
                ps(ORG, CO, G("@Id", "arg:id"))));

        registry.register(def("exp-248", "248-DHLTrackingRegister.rpt", "USP_DHLTracking_GetAllMethod",
                "frmDhlTracking.btnRegister_Click (dtHistory = DHLTracking.FormHistory)",
                ps(ORG, CO, P("@DocumentTypeId", "const:248"), P("@CanViewAllRecord", "const:1"),
                        G("@DispatchedFromDate", "arg:dispatchedFrom"), G("@DispatchedToDate", "arg:dispatchedTo"),
                        G("@FinalETAFromDate", "arg:etaFrom"), G("@FinalETAToDate", "arg:etaTo"),
                        G("@AWBBillNumber", "arg:awbBillNo"), G("@CurrentStatusId", "arg:statusId"),
                        P("@Activity", "const:FormHistory"))));

        for (String[] k : new String[][] { {"exp-319", "319-ExportSaleInvoiceSlip.rpt"}, {"exp-319a", "319A-ExportSaleInvoiceSlip.rpt"} }) {
            registry.register(def(k[0], k[1], "sp_InvSaleInvoiceDirectSlip",
                    "CommonServices.SaleInvoiceDirectSlip_294 (:6304) -> InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice",
                    ps(ORG, CO, G("@Id", "arg:id"),
                            P("rpt:CompanyAddress", "same:@CompanyAddress"), P("rpt:CompanyName", "same:@CompanyName")),
                    Collections.singletonList(sub("InvRptPurchaseBillSupplierOthers",
                            "[SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep]", ps(P("@PihId", "arg:id"))))));
        }
    }
}
