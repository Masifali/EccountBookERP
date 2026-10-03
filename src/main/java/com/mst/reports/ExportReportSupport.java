package com.mst.reports;

import org.springframework.stereotype.Component;

import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Crystal prints of the ported Export screens, traced from the desktop and registered from this component so
 * ReportRegistry.java is not edited (same pattern as HrmReportSupport / ImportsImpAReports). The pages print with
 * CrystalPrint.open(key, args) -> POST /api/reports/{key}/print.pdf; arg:<name> are the JSON keys the pages send.
 *
 * Report parameters: the default pair is "@CompanyName" / "@CompanyAddress" (ReportDefinition). Where the desktop
 * pushes them WITHOUT the @ the entry carries rpt:CompanyName / rpt:CompanyAddress (same:@...), exactly as the
 * launch site does.
 *
 * Every procedure and parameter below was checked against procdure_index.csv / procdure.utf8.sql; the desktop
 * caller is named on each entry (CommonServices.cs line, or the form). GUARDED = the BLL's "if (x != 0)" omits it.
 *
 * Seeded keys REPLACED here (register() wins over a seeded row):
 *  - 523-exprptsalescontractwiseinvoiceregister: the seed lacks @LcContractId (ExportPdfReport.ExpRptSalesContractExportRegister_523
 *    sends it when ExImLcOrderId != 0; the contract-register page sends exImLcOrderId).
 *  - 551-02-... / 551-03-...: the desktop prints Tables[1] / Tables[2] of Usp_ExportContractSchedulePeriodicB; the procedure
 *    returns only that set when @FlagForReport = 2 / 3, so the flag is sent as a constant.
 *
 * Not reproducible through a procedure contract (see UNRESOLVED): 551's checked-row filter.
 */
@Component
public class ExportReportSupport {

    /** Export print gaps, kept visible next to the contracts (ReportRegistry.UNAVAILABLE style). */
    public static final Map<String, String> UNRESOLVED;
    static {
        Map<String, String> m = new LinkedHashMap<>();
        m.put("exp-551", "PARTIAL - frmSaleContractSchedule.BtnHistoryPrint_Click prints the TICKED rows of dtHistory in RecordNo order "
                + "(LINQ join on the grid). The contract re-runs USP_ContractSchedule_FormHistory with the same history filters, so all "
                + "rows of the filter print; the page's 'ids' argument has no procedure parameter to carry it.");
        m.put("841 / 841A (Document Tracking 625)", "NOT A PRINT - hidden buttons with empty handlers on the desktop; no .rpt, no key.");
        m.put("511 / 512 (Consignment Follow Up 203)", "NOT CALLED - the page keeps the hidden buttons inert; seeded keys "
                + "511-eximshippedconsignmentfollowups-slip / 512-...-register exist but take @OrganizationId/@CompanyId from arg:, not session.");
        UNRESOLVED = Collections.unmodifiableMap(m);
    }

    private static final ReportDefinition.Param RPT_ADDR = P("rpt:CompanyAddress", "same:@CompanyAddress");
    private static final ReportDefinition.Param RPT_NAME = P("rpt:CompanyName", "same:@CompanyName");
    private static final ReportDefinition.Param ORG = P("@OrganizationId", "session:organizationId");
    private static final ReportDefinition.Param CO = P("@CompanyId", "session:companyId");

    private static List<ReportDefinition.Param> with(List<ReportDefinition.Param> a, ReportDefinition.Param... more) {
        ReportDefinition.Param[] all = new ReportDefinition.Param[a.size() + more.length];
        for (int i = 0; i < a.size(); i++) all[i] = a.get(i);
        System.arraycopy(more, 0, all, a.size(), more.length);
        return Arrays.asList(all);
    }

    /* ---------------------------------------------------------------- Sale contract 501 family (SaleContractReports501) */

    /** SaleContractReports.SaleContractReports501 with Id only, ApprovedFilter "All" (Status null, so @Status/@IsApproved not sent). */
    private static List<ReportDefinition.Param> contract501() {
        return ps(ORG, CO, G("@LcOrderId", "arg:id"), RPT_ADDR, RPT_NAME);
    }

    private static List<ReportDefinition.SubReport> contractSubs3() {
        return Arrays.asList(
                sub("LcorderOtherItem_SubRpt.rpt", "USP_LcorderOtherItem_SubRpt", ps(P("@Id", "arg:id"))),
                sub("ExImLcOrderPaymentTermsDetail_SubRpt.rpt", "USP_ExImLcOrderPaymentTermsDetail_SubRpt", ps(P("@Id", "arg:id"))),
                sub("ExImLcContractPackingMaterialDetail_SubRpt.rpt", "USP_ExImLcContractPackingMaterialDetail_SubRpt", ps(P("@Id", "arg:id"))));
    }

    /** ExpRptSalesContractExportRegister_523: org, company, then guarded dates / customer / @LcContractId / @ActionId. */
    private static List<ReportDefinition.Param> register523(String contractArg) {
        return ps(ORG, CO,
                G("@FromDate", "arg:fromDate"),
                G("@ToDate", "arg:toDate"),
                G("@SupplierCustomerId", "arg:supplierCustomerId"),
                G("@LcContractId", "arg:" + contractArg),
                G("@ActionId", "arg:actionId"));
    }

    /** ExportPdfReport.InvoiceSlip521 (note the procedure's own spelling @OrginzationId). */
    private static List<ReportDefinition.Param> invoice521(String invoiceArg) {
        return ps(P("@OrginzationId", "session:organizationId"), CO, P("@EximInvoiceId", "arg:" + invoiceArg), RPT_ADDR, RPT_NAME);
    }

    private static List<ReportDefinition.Param> deliveryOrderSlip(String docTypeSource) {
        return ps(ORG, CO, P("@DocumentTypeId", docTypeSource), P("@Id", "arg:id"), G("@UserId", "session:userId"), RPT_ADDR, RPT_NAME);
    }

    private static List<ReportDefinition.Param> forwarding508() {
        return ps(ORG, CO,
                G("@EximInvoiceId", "arg:eximInvoiceId"),
                G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                G("@GpDateFrom", "arg:gpDateFrom"), G("@GpDateTo", "arg:gpDateTo"),
                G("@GpSrNoFrom", "arg:gpSrNoFrom"), G("@GpSrNoTo", "arg:gpSrNoTo"),
                G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@ItemId", "arg:itemId"),
                G("@ContractId", "arg:contractId"), G("@WarehouseId", "arg:warehouseId"), G("@ToWarehouseId", "arg:toWarehouseId"),
                G("@Activity", "arg:activity"));
    }

    private static List<ReportDefinition.Param> weightDiff618() {
        return ps(ORG, CO,
                G("@InvoiceId", "arg:eximInvoiceId"), G("@ContractId", "arg:exImLcOrderId"),
                G("@GpDateFrom", "arg:fromDate"), G("@gpDateTo", "arg:toDate"), G("@InvoiceStatus", "arg:status"));
    }

    private static List<ReportDefinition.Param> comparisonFilters() {
        return ps(ORG, CO,
                G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@DetinationPortId", "arg:destinationPortId"),
                G("@ItemId", "arg:itemId"), G("@ItemTypeId", "arg:itemTypeId"), G("@ItemCategoryId", "arg:itemCategoryId"),
                G("@LcContractId", "arg:lcContractId"), G("@ExImInvoiceId", "arg:exImInvoiceId"),
                G("@SalePersonId", "arg:salePersonId"), G("@ItemStockAccountId", "arg:itemStockAccountId"));
    }

    private static List<ReportDefinition.Param> periodicB(String flag) {
        return ps(ORG, CO,
                P("@BranchId", "arg:branchesId"),
                G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                G("@PerFclTon", "arg:netWeight"), G("@DaysInterval", "arg:intervalDays"), G("@SortNo", "arg:sortNo"),
                P("@FlagForReport", "const:" + flag),
                G("@ReferredStatusId", "arg:statusId"));
    }

    public ExportReportSupport(ReportRegistry registry) {

        /* ------------------------------------------------ Sales Contract 196 / Contract III / Packing Detail by Contract (agent G) */
        registry.register(def("exp-501", "501-ExpRptSalesContractExport.rpt", "Sp_ExImLcOrderNo_ExportSlip_Rpt",
                "CommonServices.ExportSalesContractExport501 (:10141) -> SaleContractReports.SaleContractReports501", contract501()));
        registry.register(def("exp-501-sub", "501-ExpRptSalesContractExport.rpt", "Sp_ExImLcOrderNo_ExportSlip_Rpt",
                "CommonServices.ExportSalesContractExport501WithSubReport (:10230)", contract501(),
                Collections.singletonList(sub("LcorderOtherItem_SubRpt.rpt", "USP_LcorderOtherItem_SubRpt", ps(P("@Id", "arg:id"))))));
        registry.register(def("exp-501new", "501-ExportSalesContractExportNew.rpt", "Sp_ExImLcOrderNo_ExportSlip_Rpt",
                "CommonServices (:9991-10033) 501-ExportSalesContractExportNew with 3 sub-reports", contract501(), contractSubs3()));
        registry.register(def("exp-501a", "501A_ExportSalesContractExportNew.rpt", "Sp_ExImLcOrderNo_ExportSlip_Rpt",
                "CommonServices (:10041-10083) 501A_ExportSalesContractExportNew with 3 sub-reports", contract501(), contractSubs3()));
        registry.register(def("exp-501b", "501B_ExportSalesContractExportNew.rpt", "Sp_ExImLcOrderNo_ExportSlip_Rpt",
                "CommonServices (:10091-10133) 501B_ExportSalesContractExportNew with 3 sub-reports", contract501(), contractSubs3()));
        registry.register(def("exp-523", "523-ExpRptSalesContractWiseInvoiceRegister.rpt",
                "SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt",
                "CommonServices.SalesContractExportSlip523 (:9906) -> ExportPdfReport.ExpRptSalesContractExportRegister_523",
                with(register523("id"), RPT_ADDR, RPT_NAME)));
        /* Seeded row replaced: the seed omitted @LcContractId (contract-register page sends exImLcOrderId). */
        registry.register(def("523-exprptsalescontractwiseinvoiceregister", "523-ExpRptSalesContractWiseInvoiceRegister.rpt",
                "SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt",
                "ExportPdfReport.ExpRptSalesContractExportRegister_523 (contract register print)",
                with(register523("exImLcOrderId"), RPT_ADDR, RPT_NAME)));

        /* ------------------------------------------------ Contract Schedule 394 (agent G) */
        /* LcOrderShipmentScheduleLcOrderWise_Slip(LcOrderId, ContractScheduleId) (:9945): the MAIN table is the 523 procedure
           (@LcContractId = LcOrderId); sub 1 USP_LcOrderShipmentScheduleLcOrderWise_Slip @OrganizationId @CompanyId @LcOrderId
           [@ContractScheduleId]; sub 2 USP_LcOrderShipmentScheduleLcOrderWiseDetail_SubReport @LcOrderId [@ContractScheduleId].
           Report parameters WITH the @ (default pair). */
        registry.register(def("exp-395", "395-LcOrderShipmentScheduleLcOrderWise_Slip.rpt",
                "SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt",
                "CommonServices.LcOrderShipmentScheduleLcOrderWise_Slip (:9945)",
                ps(ORG, CO, G("@LcContractId", "arg:id")),
                Arrays.asList(
                        sub("LcOrderShipmentScheduleLcOrderWise_SubReport1.rpt", "[dbo].[USP_LcOrderShipmentScheduleLcOrderWise_Slip]",
                                ps(ORG, CO, P("@LcOrderId", "arg:id"), G("@ContractScheduleId", "arg:contractScheduleId"))),
                        sub("LcOrderShipmentScheduleLcOrderWiseDetail_SubReport.rpt",
                                "[dbo].[USP_LcOrderShipmentScheduleLcOrderWiseDetail_SubReport]",
                                ps(P("@LcOrderId", "arg:id"), G("@ContractScheduleId", "arg:contractScheduleId"))))));
        /* BtnHistoryPrint_Click (frmSaleContractSchedule:2807) - dtHistory = ContractSchedule_FormHistory with the history filters
           (DataGridHistoryFill :2587). Page body {fromDate,toDate,customerId,portId,itemId,ids}. See UNRESOLVED (ticked rows). */
        registry.register(def("exp-551", "551-ContractSchedule_FormHistory.rpt", "USP_ContractSchedule_FormHistory",
                "frmSaleContractSchedule.BtnHistoryPrint_Click -> ExImLcOrderShipmentSchedule.ContractSchedule_FormHistory",
                ps(ORG, CO, G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:customerId"), G("@DestinationPortId", "arg:portId"), G("@ItemId", "arg:itemId"))));
        /* BtnPrint551_01_Click (frmSaleContractSchedule:3786). The seed lacks @ItemId and uses other arg names. */
        registry.register(def("exp-551-01", "551_01_ContractSchedule_StatusReport.rpt", "USP_ContractSchedule_StatusReport",
                "frmSaleContractSchedule.BtnPrint551_01_Click -> ExImLcOrderShipmentSchedule.ContractSchedule_StatusReport",
                ps(ORG, CO, G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@DestinationPortId", "arg:destinationPortId"),
                   G("@ItemId", "arg:itemId"))));
        /* Contract Schedule Periodic (agent D): seeded rows replaced to add @FlagForReport 2 / 3. */
        registry.register(def("551-02-exportcontractscheduleloadingdateitemwise-slip",
                "551_02_ExportContractScheduleLoadingDateItemWise_Slip.rpt", "Usp_ExportContractSchedulePeriodicB",
                "frmExportContractSchedulePeriodicB.btnPrint02_Click (Tables[1] = @FlagForReport 2)", periodicB("2")));
        registry.register(def("551-03-exportcontractscheduleloadingdatecustomeritemwise-slip",
                "551_03_ExportContractScheduleLoadingDateCustomerItemWise_Slip.rpt", "Usp_ExportContractSchedulePeriodicB",
                "frmExportContractSchedulePeriodicB.btnPrint03_Click (Tables[2] = @FlagForReport 3)", periodicB("3")));

        /* ------------------------------------------------ Commercial / custom invoice (agents B, E) */
        /* CommercialInvoiceSlip521 (:10265): InvoiceSlip521 + ExportInvoiceOtherExpense_SubRpt / ExportInvoiceCustomExpense_SubRpt
           (@PihId = EximInvoiceId); rpt params without the @. Pages send eximInvoiceId (custom invoice sends id + eximInvoiceId). */
        registry.register(def("exp-521", "521-ExportInvoiceSlip.rpt", "Sp_ExImInvoice_SlipAndRegister_Rpt",
                "CommonServices.CommercialInvoiceSlip521 (:10265) -> ExportPdfReport.InvoiceSlip521", invoice521("eximInvoiceId"),
                Arrays.asList(
                        sub("ExportInvoiceOtherExpense_SubRpt.rpt", "[ExportInvoiceOtherExpense_SubRpt]", ps(P("@PihId", "arg:eximInvoiceId"))),
                        sub("ExportInvoiceCustomExpense_SubRpt.rpt", "[ExportInvoiceCustomExpense_SubRpt]", ps(P("@PihId", "arg:eximInvoiceId"))))));
        /* ExportPackingList529 (:10394): InvoiceSlip521 + 529-SubRpt-ContainerListByInvoice (SpExImInvoiceGetContainersList
           @ExImInvoiceId); rpt params without the @. (Not usp_ExImInvoicePackingList_Rpt - that is the 529A/B/C family.) */
        registry.register(def("exp-529", "529-ExportInvoiceSlipPackingList.rpt", "Sp_ExImInvoice_SlipAndRegister_Rpt",
                "CommonServices.ExportPackingList529 (:10394) -> ExportPdfReport.InvoiceSlip521", invoice521("id"),
                Collections.singletonList(sub("529-SubRpt-ContainerListByInvoice.rpt", "SpExImInvoiceGetContainersList",
                        ps(P("@ExImInvoiceId", "arg:id"))))));
        /* CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(Id, 84) -> InvDeliveryOrder.DeliveryOrderExortSlip_ByInvoiceId
           = Sp_InvDeliveryOrder_ExportSlipByInvoice @OrganizationId @CompanyId @DocumentTypeId @Id(=InvoiceId); rpt params WITH @. */
        registry.register(def("exp-do-84", "527-ExportDeliveryOrderByInvoiceIdSlip.rpt", "Sp_InvDeliveryOrder_ExportSlipByInvoice",
                "CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(Id, 84) -> InvDeliveryOrder.DeliveryOrderExortSlip_ByInvoiceId",
                ps(ORG, CO, P("@DocumentTypeId", "const:84"), P("@Id", "arg:invoiceId"))));

        /* ------------------------------------------------ CRO booking / shipping line (agent B) */
        /* ExBookingInfoSlip560 (@Id) / BookingInfoSlip560_AgainstInvoice (@ExIminvoiceId) (:10690-10755) ->
           ExImExportShipingLineBooking.PrintSlipandRegister, all guarded; rpt params WITH @. */
        registry.register(def("exp-560", "560-ExBooking Info(CRO).rpt", "Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt",
                "CommonServices.ExBookingInfoSlip560 / BookingInfoSlip560_AgainstInvoice -> ExImExportShipingLineBooking.PrintSlipandRegister",
                ps(ORG, CO, G("@ExIminvoiceId", "arg:exImInvoiceId"), G("@Id", "arg:id"))));
        registry.register(def("exp-509", "509-ExImExportShipingLineBooking_Register.rpt",
                "Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt",
                "frmExportShipingLineBookingRpt (:343) -> ExImExportShipingLineBooking.PrintSlipandRegister",
                ps(ORG, CO, G("@ExIminvoiceId", "arg:exImInvoiceId"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                   G("@DestinationPortId", "arg:destinationPortId"))));
        /* ExportPendingShipmentsFollowupReport (:904) - first result set of ExportPendingShipmentsFollowup. */
        registry.register(def("exp-552", "552-ExportPendingShippmentFollowUp.rpt", "ExportPendingShipmentsFollowup",
                "ExportPendingShipmentsFollowupReport.print (:904)",
                ps(ORG, CO, P("@BranchId", "session:branchId"),
                   G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@DestinationPort", "arg:destinationPortId"),
                   G("@DocumentStatusIds", "arg:documentStatusIds"), G("@CurrentStatusIds", "arg:currentStatusIds"),
                   G("@ActivityId", "arg:activityId"), G("@StatusId", "arg:statusId"), G("@SortNo", "arg:sortNo"))));

        /* ------------------------------------------------ Weight audit / gate pass / delivery orders (agents B, D, F) */
        registry.register(def("exp-618", "618-DoWeightWbWeightDiff.rpt", "SpEximInvoice_DoWeightWbWeightDiff_Rpt",
                "frmAuditByWeightReport (:504 / :896) -> ExImInvoice.ExportSalesAuditByWeight", weightDiff618()));
        registry.register(def("exp-618-01", "618_01_DoWeightWbWeightDiff.rpt", "SpEximInvoice_DoWeightWbWeightDiff_Rpt",
                "frmAuditByWeightReport (:919) -> ExImInvoice.ExportSalesAuditByWeight", weightDiff618()));
        /* OutwardGatePassWithWb(GatePassId) (:14228): GatePassOutwardSlipandRegister @Id + WbTransationByOutwardGPID_SubReport @GpId;
           rpt params WITH @. exp-258 (weight audit GpNo link) and exp-258-gp (container inspection GpSlip) are the same print. */
        for (String key : new String[] {"exp-258", "exp-258-gp"}) {
            registry.register(def(key, "258-OutwardGatePassWithWbAndLabSlip.rpt", "Sp_GatePassOutward_SlipAndRegister_Rpt",
                    "CommonServices.OutwardGatePassWithWb (:14228) -> GatePassOutwardReports.GatePassOutwardSlipandRegister",
                    ps(ORG, CO, G("@Id", "arg:id")),
                    Collections.singletonList(sub("WbTransationByOutwardGPID_SubReport.rpt", "USP_WbTransationByOutwardGPID_SubReport",
                            ps(P("@GpId", "arg:id"))))));
        }
        /* ExportDeliveryOrderSlip396(Id, DocumentTypeId) (:9546) -> InvGrnandGdnReports.InvDeliveryOrderSlip; rpt params without @. */
        registry.register(def("exp-396", "396-ExportDeliveryOrderSlip.rpt", "Sp_InvDeliveryOrder_Slip",
                "CommonServices.ExportDeliveryOrderSlip396 (:9546)", deliveryOrderSlip("arg:documentTypeId")));
        /* InvDeliveryOrderSlip(Id, 84) (:9381) - as dopm-262 with DocumentTypeId 84. */
        registry.register(def("exp-262-do84", "262-DeliveryOrderSlip.rpt", "Sp_InvDeliveryOrder_Slip",
                "CommonServices.InvDeliveryOrderSlip(Id, 84) (:9381)", deliveryOrderSlip("const:84")));
        /* Delivery Order Report 250 print (agent D trace); the seeded 263-invdeliveryorderforapproval names the wrong procedure. */
        registry.register(def("exp-263-do", "263-InvDeliveryOrderForApproval.rpt", "Sp_InvDeliveryOrderForApproval_Rpt",
                "DeliveryOrderReport.print -> Sp_InvDeliveryOrderForApproval_Rpt",
                ps(ORG, CO,
                   G("@DocumentTypeId", "arg:documentTypeId"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                   G("@ItemId", "arg:itemId"), G("@ExImInvoiceId", "arg:exImInvoiceId"),
                   G("@WbWeightStatus", "arg:wbWeightStatus"), G("@DoReferedStatus", "arg:doReferedStatus"),
                   G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   G("@IsApproved", "arg:isApproved"), G("@DeliveryOrderType", "arg:deliveryOrderType"),
                   RPT_ADDR, RPT_NAME)));
        /* GatePassInspection.btnPrint_Click (:1679) -> ExImVCITransaction_SlipAndRegister @GpId; Tables[0]. The BLL adds no
           tenancy parameters but the procedure declares @OrganizationId / @CompanyId WITHOUT defaults, so they are sent from
           the session (agent F). rpt params WITH @. */
        registry.register(def("exp-244", "244-GatePassInspectionSlip.rpt", "USP_ExImVCITransaction_SlipAndRegister",
                "GatePassInspection.btnPrint_Click -> ExImVCITransaction.ExImVCITransaction_SlipAndRegister",
                ps(ORG, CO, G("@GpId", "arg:gpId"))));

        /* ------------------------------------------------ Forwarding / loading (agent B) */
        registry.register(def("exp-556", "556-ExportLoadSheet.rpt", "[dbo].[USP_ExportLoadSheet]",
                "ExportLoadingSheet (:334) -> ExportReports.ExportLoadingSheet",
                ps(ORG, CO, G("@InvoiceId", "arg:eximInvoiceId"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                   G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"))));
        registry.register(def("exp-508", "508-ExImForwarding_Register.rpt", "Sp_ExImForwarding_SlipAndRegister_Rpt",
                "ExImForwardingHistory (:823) -> Sp_ExImForwarding_SlipAndRegister_Rpt (the seeded 508 names the booking procedure)",
                forwarding508()));
        registry.register(def("exp-508-01", "508_01_Forwarding_Customised_Register.rpt", "Sp_ExImForwarding_SlipAndRegister_Rpt",
                "ExImForwardingHistory (:887) -> Sp_ExImForwarding_SlipAndRegister_Rpt", forwarding508()));
        registry.register(def("exp-508-02", "508_02_ForwardingSummary_Register.rpt", "Sp_ExImForwarding_SlipAndRegister_Rpt",
                "ExImForwardingCostingReport (:792) -> Sp_ExImForwarding_SlipAndRegister_Rpt (@Activity Summary)", forwarding508()));
        registry.register(def("exp-507", "507-ExImForwarding_Slip.rpt", "Sp_ExImForwarding_Rpt",
                "ExImForwardingHistory (:864) -> ExportPdfReport.ExImForwarding_Slip_507", ps(ORG, CO, G("@Id", "arg:id"))));

        /* ------------------------------------------------ Sales / comparisons / costing / FI (agent B) */
        registry.register(def("exp-359", "359-GetExportSales.rpt", "USP_GetExportSales",
                "ExportSalesReport (:447) -> ExportReports.GetExportSalesReport",
                ps(ORG, CO, P("@FinancialYearId", "session:financialYearId"),
                   G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@InvoiceId", "arg:exImInvoiceNoId"),
                   G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@ItemId", "arg:itemId"),
                   G("@JobLotId", "arg:jobLotId"), G("@ActionId", "arg:actionId"))));
        registry.register(def("exp-534", "534-ExportComparisonSummaryByCustomer&ItemReport.rpt",
                "SpExImInvoice_ExportComparisonsSummery_Report", "ExportCustomerWiseComparisonsSummeryReport (:717)",
                with(comparisonFilters(), P("@ReportType", "arg:reportType"))));
        registry.register(def("exp-535", "535-ExportComparisonSummaryByCustomer&ItemReport.rpt",
                "SpExImInvoice_ExportComparisonsSummery_Report", "ExportCustomerWiseComparisonsSummeryReport (:742) + @LoginUserName",
                with(comparisonFilters(), P("@ReportType", "arg:reportType"), P("rpt:@LoginUserName", "session:userName"))));
        registry.register(def("exp-536", "536-ExportCustomerWiseComparisonReport.rpt",
                "SpExImInvoice_ExportComparisonsSummery_Report", "ExportCustomerWiseComparisonsSummeryReport (:765)",
                with(comparisonFilters(), P("@ReportType", "arg:reportType"))));
        registry.register(def("exp-536-01", "536_01_ExportSaleDetailReport.rpt", "usp_ExportSaleDetail_report",
                "ExportCustomerWiseComparisonsSummeryReport (:893)", comparisonFilters()));
        registry.register(def("exp-541", "541-ServiceBillRegister.rpt", "Sp_ExImClearingAgentBill_Rpt",
                "frmServiceBillRegister.print -> ServicesReports.ServicesBill_SlipAndRegister",
                ps(ORG, CO, G("@InvoiceId", "arg:exImInvoiceId"), G("@SupplierCustomerId", "arg:supplierCustomerId"),
                   G("@ItemId", "arg:itemId"), G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"),
                   G("@LoadingPortId", "arg:loadingPortId"), G("@DestinationPortId", "arg:destinationPortId"),
                   RPT_ADDR, RPT_NAME)));
        registry.register(def("exp-542-01", "542_01-ShipmentCostingSummary.rpt", "USP_ExportShipmentCosting_SummaryReport",
                "frmExImShipmentCostingSummary (:536) -> ExportReports.ExportShipmentCosting_SummaryReport",
                ps(ORG, CO, G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@ExImInvoiceId", "arg:exImInvoiceId"),
                   G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@DestinationPortId", "arg:destinationPortId"),
                   RPT_ADDR, RPT_NAME)));
        registry.register(def("exp-300-02", "300_02-FIAdvanceBalanceSummary.rpt", "usp_FinancialInstrumentAdvanceBalanceSummary",
                "frmFinancialInstrumentAdvanceBalanceSummary (:626) -> ExImEFormRegistration.FIAdvanceBalanceSummary",
                ps(ORG, CO, G("@SupplierCustomerId", "arg:supplierCustomerId"), G("@BankId", "arg:bankId"),
                   G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"), G("@SkipZero", "arg:skipZero"),
                   G("@ActionId", "arg:actionId"))));

        /* ------------------------------------------------ Receivables (agent D) - Tables[0] of the 6-set procedure */
        registry.register(def("exp-243", "243-ExportReceivablesDueByDate.rpt", "USP_ExportReceivableByDueDateRegisterA",
                "ExportReceivableByDueDateRegisterA (:809)",
                ps(ORG, CO, G("@CustomerId", "arg:customerId"), G("@ContractId", "arg:contractId"), G("@InvoiceId", "arg:invoiceId"),
                   G("@DueFrom", "arg:dueFrom"), G("@DueTo", "arg:dueTo"), G("@PaymentTermId", "arg:paymentTermId"),
                   G("@DeliveryTermId", "arg:deliveryTermId"), G("@DestinationPortId", "arg:destinationPortId"),
                   G("@SkipZero", "arg:skipZero"), G("@SortNo", "arg:sortNo"))));
            /* ------------------------------------------------ Waves H-L (commercial invoice 211/880, pre invoice 192/194,
           forwarding / DO 212/793/208, third party inspection 857/858, FCY receipts 794). Traces from the porting agents;
           procedures and parameters checked against procdure_index.csv. */
        for (String v : new String[] {"a", "b", "c", "d"}) {
            registry.register(def("521-" + v + "-exportinvoiceslip", "521_" + v.toUpperCase() + "_ExportInvoiceSlip.rpt",
                    "Sp_ExImInvoice_SlipAndRegister_Rpt",
                    "frmCommercialInvoiceIII history Print" + v.toUpperCase() + " -> ExportPdfReport.InvoiceSlip521",
                    invoice521("eximInvoiceId"),
                    Arrays.asList(
                            sub("ExportInvoiceOtherExpense_SubRpt.rpt", "[ExportInvoiceOtherExpense_SubRpt]", ps(P("@PihId", "arg:eximInvoiceId"))),
                            sub("ExportInvoiceCustomExpense_SubRpt.rpt", "[ExportInvoiceCustomExpense_SubRpt]", ps(P("@PihId", "arg:eximInvoiceId"))))));
        }
        for (String[] k : new String[][] {{"exp-546", "546-ExportPreInvoiceSlip.rpt"}, {"exp-546-bank", "546-ExportPreInvoiceSlipForBank.rpt"}}) {
            registry.register(def(k[0], k[1], "[dbo].[USP_PreInvoice_SlipAndRegister_Rpt]",
                    "PreCommiercialInvoice Print-546 / Bank Print-546",
                    ps(P("@OrginzationId", "session:organizationId"), CO, P("@Id", "arg:id")),
                    Collections.singletonList(sub("PreCommercialInvoiceExpenseSubReport.rpt", "[ExportInvoiceOtherExpense_SubRpt]",
                            ps(P("@PihId", "arg:id"))))));
        }
        registry.register(def("exp-523-contract", "523-ExpRptSalesContractWiseInvoiceRegister.rpt",
                "SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt", "ProformaInvoice / PreCommiercialInvoice contract register",
                ps(ORG, CO, P("@LcContractId", "arg:id"))));
        registry.register(def("295-gp-by-invoice", "295-InvRptOutwardGatePassSlipWithItems.rpt", "Sp_GatePassOutward_SlipAndRegister_Rpt",
                "ExportDeliveryOrderB GpPrint -> outward gate pass slip by invoice",
                ps(ORG, CO, P("@InvoiceId", "arg:invoiceId"))));
        registry.register(def("exp-514", "514_ThirdPartyInspectionSlip.rpt", "usp_InvLabPreThirdPartyInspection_Slip",
                "frmThirdPartyInspection / frmLabAgainstThirdPartyInspection Print-514",
                ps(ORG, CO, P("@FinancialYearId", "session:financialYearId"), P("@Id", "arg:id"))));
        registry.register(def("exp-514-01", "514_01_InventoryStockReservedSlip.rpt", "USP_InventoryStockReserved_SlipAndRegister",
                "frmThirdPartyInspection Print-514_01",
                ps(ORG, CO, G("@BranchesId", "session:branchId"), P("@FinancialYearId", "session:financialYearId"), P("@RefDocId", "arg:id"))));
        registry.register(def("acc-102", "102-AcRptPaymentReceiptsVoucherSlip.rpt", "Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt",
                "Acfrmfcbankreceipt voucher print -> VoucherReports.PaymentReceiptVoucherSlip",
                ps(ORG, CO, P("@Id", "arg:id"), P("@DocumentTypeId", "arg:documentTypeId"), G("@IsApproved", "arg:isApproved"))));
        registry.register(def("exp-516", "516-ExImRptFCBankReceipts.rpt", "Sp_ExImFcBankReceiptsAdvoice_Rpt",
                "Acfrmfcbankreceipt Print-516", ps(ORG, CO, G("@Id", "arg:id"))));
    }
}
