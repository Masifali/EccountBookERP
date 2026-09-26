package com.mst.reports;

import com.mst.repositories.ReportContractRepository;
import org.springframework.beans.factory.ObjectProvider;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import javax.annotation.PostConstruct;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.reports.ReportDefinition.Mode.ALWAYS;
import static com.mst.reports.ReportDefinition.Mode.GUARDED;

/**
 * The traced print contracts.
 *
 * Each entry was read from the desktop C#: form button -> CommonServices/helper method -> BLL
 * -> GenericProvider.GetDataTableProc("<procedure>"). The `desktopCaller` field records where.
 *
 * GUARDED parameters are the ones the BLL wraps in a test (if (obj.X != 0), if
 * (!Conversion.CheckDateTimeNull(...))). They are OMITTED when unset — a guarded parameter sent
 * as NULL is a different call and can return a different result set.
 *
 * Reports NOT registered here, and why, are listed in ReportRegistry.UNAVAILABLE.
 */
@Component
public class ReportRegistry {

    private static ReportDefinition.Param P(String n, String src)  { return new ReportDefinition.Param(n, src, ALWAYS); }
    private static ReportDefinition.Param G(String n, String src)  { return new ReportDefinition.Param(n, src, GUARDED); }
    private static List<ReportDefinition.Param> ps(ReportDefinition.Param... a) { return Arrays.asList(a); }
    private static List<ReportDefinition.SubReport> subs(ReportDefinition.SubReport... a) { return Arrays.asList(a); }

    private final Map<String, ReportDefinition> byKey = new LinkedHashMap<>();

    /** Keys written by hand below, from a traced desktop caller. Seeded rows never replace them. */
    private final java.util.Set<String> handTraced = new java.util.LinkedHashSet<>();

    private int seededCount = 0;
    private int seededSkipped = 0;

    @Autowired(required = false)
    private ObjectProvider<ReportContractRepository> contractRepository;

    private void add(ReportDefinition d) { byKey.put(d.key, d); handTraced.add(d.key); }

    /**
     * Merge the contracts seeded from the C# trace (migration/report-contracts).
     *
     * Hand-traced entries win outright: a key already present is counted and skipped, never
     * overwritten. If the seed tables are absent the registry is exactly what the constructor
     * built, so this is safe to run before the seeder has ever been executed.
     */
    @PostConstruct
    void loadSeededContracts() {
        if (contractRepository == null) return;
        ReportContractRepository repo = contractRepository.getIfAvailable();
        if (repo == null) return;
        for (ReportDefinition d : repo.loadAll()) {
            if (handTraced.contains(d.key)) { seededSkipped++; continue; }
            byKey.put(d.key, d);
            seededCount++;
        }
        LOG.info("Report registry: {} hand-traced, {} seeded from the C# trace, {} seeded rows "
                 + "skipped because a hand-traced contract already covers them",
                 handTraced.size(), seededCount, seededSkipped);
    }

    private static final org.slf4j.Logger LOG =
            org.slf4j.LoggerFactory.getLogger(ReportRegistry.class);

    /** How the registry was assembled - surfaced so the split is visible, not assumed. */
    public Map<String, Integer> composition() {
        Map<String, Integer> m = new LinkedHashMap<>();
        m.put("handTraced", handTraced.size());
        m.put("seeded", seededCount);
        m.put("seededSkippedAsAlreadyTraced", seededSkipped);
        m.put("total", byKey.size());
        return m;
    }

    public boolean isHandTraced(String key) { return handTraced.contains(key); }

    public ReportRegistry() {

        /* ---------------------------------------------------------------- Purchase Order (41) */

        // CommonServices.PurchaseOrderSlipReport203
        add(new ReportDefinition("po-203", "203-InvRptPurchaseOrderRiceSlip.rpt",
                "Sp_PurchaseOrderSlip_Rpt",
                "CommonServices.PurchaseOrderSlipReport203",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@DocumentTypeId","const:41"),
                   P("@OrderId","arg:id"),
                   P("@ApprovedFilter","const:All"),
                   G("@StartOrderDate","arg:fromDate"), G("@EndOrderDate","arg:toDate"),
                   G("@PoSrFrom","arg:fromDocNo"),      G("@PoSrTo","arg:toDocNo")),
                subs(new ReportDefinition.SubReport("PurchaseOrderSubReport.rpt",
                            "USP-PurchaseOrderSubReport", ps(P("@OrderId","arg:id"))),
                     new ReportDefinition.SubReport("PurchaseOrderSupplierExpenseSubReport.rpt",
                            "[dbo].[USP_PurchaseOrderSupplierExpense_SubReport]", ps(P("@OrderId","arg:id"))))));

        // CommonServices.PurchaseOrderSlipReport203_01 - same main procedure, same sub-reports
        add(new ReportDefinition("po-203-01", "203_01_PurchaseOrderRiceSlip.rpt",
                "Sp_PurchaseOrderSlip_Rpt",
                "CommonServices.PurchaseOrderSlipReport203_01",
                byKey.get("po-203").params,
                byKey.get("po-203").subReports));

        // CommonServices.GenerateReport (General Order Slip)
        add(new ReportDefinition("po-201", "201-InvRptPurchaseOrderGeneralSlip.rpt",
                "Sp_PurchaseOrder_GeneralOrderSlip_Rpt",
                "CommonServices.GenerateReport",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@OrderId","arg:id")),
                Collections.emptyList()));

        // toolStripButton2_Click / toolStripButton1_Click - the register pair share one procedure
        add(new ReportDefinition("po-205", "205-InvRptPurchaseOrderDetailRegisterRice.rpt",
                "Sp_PurchaseOrder_RegisterDetail_Rpt", "PurchaseOrderHistory.toolStripButton2_Click",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate")),
                Collections.emptyList()));

        add(new ReportDefinition("po-206", "206-RptPurchaseOrderRegisterRice.rpt",
                "Sp_PurchaseOrder_RegisterDetail_Rpt", "PurchaseOrderHistory.toolStripButton1_Click",
                byKey.get("po-205").params, Collections.emptyList()));

        // CrystalReportPrint_Helper.PurchaseOrderSlip_1052 - CMAGT purchase order
        add(new ReportDefinition("po-1052", "1052_PurchaseOrderSlip.rpt",
                "[cmagt].[USP_purchaseOrderMaster_Slip]",
                "CrystalReportPrint_Helper.PurchaseOrderSlip_1052",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        // CrystalReportPrint_Helper.PurchaseOrderHeader_Slip_1303 - logistics schema
        add(new ReportDefinition("po-1303", "1303_PurchaseOrderHeader_Slip.rpt",
                "[lgstcm].[USP_PurchaseOrderHeader_Slip]",
                "CrystalReportPrint_Helper.PurchaseOrderHeader_Slip_1303",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        // CommonServices.PurchaseOrderSlip1100
        add(new ReportDefinition("po-1100", "1100_PurchaseOrderSlip.rpt",
                "[fed].[USP_PurchaseOrderSubReport]",
                "CommonServices.PurchaseOrderSlip1100",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@OrderId","arg:id")),
                subs(new ReportDefinition.SubReport("PurchaseOrderSupplierExpenseSubReport.rpt",
                            "[fed].[USP_PurchaseOrderSupplierExpense_SubReport]", ps(P("@OrderId","arg:id"))))));

        // CommonServices.PurchaseOrderSlipConcrete - pcc schema
        add(new ReportDefinition("po-1863-slip", "1863-InvRptPurchaseOrderSlip.rpt",
                "[pcc].[USP_PurchaseOrder_SlipAndRegister]",
                "CommonServices.PurchaseOrderSlipConcrete",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* ------------------------------------------------------------ Purchase Invoice / Bill */

        add(new ReportDefinition("pi-1859", "1859_InvPurchaseInvoice_DirectSlip.rpt",
                "[pcc].[USP_InvPurchaseInvoice_DirectSlip]",
                "CommonServices.PurchaseInvoiceAndRetunFinishGoodsPrint1859",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                subs(new ReportDefinition.SubReport("InvPurchaseInvoice_ItemExpense_SubReport.rpt",
                            "[pcc].[USP_InvPurchaseInvoice_ItemExpense_SubReport]", ps(P("@Id","arg:id"))))));

        /* -------------------------------------------------------------------------- GRN / GDN */

        add(new ReportDefinition("grn-211", "211-InvRptGoodsReceiptsNotesRiceSlip.rpt",
                "Sp_InvGrn_RiceSlip_Rpt", "CommonServices.GrnSlipReport211",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                subs(new ReportDefinition.SubReport("InvGrnDetailEmptyBagsSubReport.rpt",
                            "Sp_InvGrnDetailEmptyBagsSubReport", ps(P("@Id","arg:id"))))));

        /* InvGrnandGdnReports.GrnSlipStore (BLL 0131 :326) always sends @DocumentTypeId, and the
           procedure filters on Grh.DocumentTypeId = @DocumentTypeId - omitting it returns no rows.
           GrnSlipReport212 sets 48, GrnPMSlipReport214 / GrnPmSlipWithSubReport214_01 set 701. */
        add(new ReportDefinition("grn-212", "212-InvRptGoodsReceiptsNotesStoreSlip.rpt",
                "Sp_InvGrn_StoreSlip_Rpt", "CommonServices.GrnSlipReport212",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@DocumentTypeId","const:48"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        add(new ReportDefinition("grn-213", "213-GoodsReceiptsNotesAgainstOrderSlip.rpt",
                "Sp_InvGrn_RiceSlip_Rpt", "CommonServices.GrnSlipReport213",
                byKey.get("grn-211").params, byKey.get("grn-211").subReports));

        /* PurchsaeOrderPmNew.GenerateReport :2815 -> CommonServices.PurchaseOrderPackingMaterialSlip215
           :8542 -> PurchaseOrderReports.PurchaseOrderSlipReport201 (BLL 0134 :14). The form sets
           DocumentTypeId 700 and ApprovedFilter "All", but the BLL never sends DocumentTypeId, and
           ApprovedFilter "All" suppresses @IsApproved - so only these three go. */
        add(new ReportDefinition("po-pm-215", "215-PurchaseOrderPackingMaterialSlip.rpt",
                "Sp_PurchaseOrder_GeneralOrderSlip_Rpt", "CommonServices.PurchaseOrderPackingMaterialSlip215",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@OrderId","arg:id")),
                Collections.emptyList()));

        add(new ReportDefinition("grn-214", "214-GRNPackingMaterialSlip.rpt",
                "Sp_InvGrn_StoreSlip_Rpt", "CommonServices.GrnPMSlipReport214",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@DocumentTypeId","const:701"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* PurchaseInvoicePMSlip_231 / _231_01 (CommonServices) - InvPurchaseInvoiceReports.PurchaseInvoice_PM231
           plus the supplier add/less sub-report. 231_01 adds PurchaseOrder_WithGrnDetail over the DISTINCT
           values of the main rows' "Id" column, joined with "," - that is what the desktop passes as
           @PurchaseOrderIds (ported as written). */
        add(new ReportDefinition("pi-pm-231", "231-InvRptPurchaseBillPackingMaterialSlip.rpt",
                "USp_InvPurchaseInvoice_PackingMaterialBill_Rpt", "CommonServices.PurchaseInvoicePMSlip_231",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers",
                            "SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt", ps(P("@PihId","arg:id"))))));

        add(new ReportDefinition("pi-pm-231-01", "231_01_PurchaseBillPmSlipWithGrnDetail.rpt",
                "USp_InvPurchaseInvoice_PackingMaterialBill_Rpt", "CommonServices.PurchaseInvoicePMSlip_231_01",
                byKey.get("pi-pm-231").params,
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers.rpt",
                            "SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt", ps(P("@PihId","arg:id"))),
                     new ReportDefinition.SubReport("PurchaseOrder_WithGrnDetail_SubReport.rpt",
                            "[dbo].[USP_PurchaseOrder_WithGrnDetail_Report]",
                            ps(P("@OrganizationId","session:organizationId"),
                               P("@CompanyId","session:companyId"),
                               P("@PurchaseOrderIds","rows:Id"))))));

        /* InvDeliveryOrderSlip(Id, 85) (CommonServices :9381) - BLL 0131 InvGrnandGdnReports.InvDeliveryOrderSlip. */
        add(new ReportDefinition("dopm-262", "262-DeliveryOrderSlip.rpt",
                "Sp_InvDeliveryOrder_Slip", "CommonServices.InvDeliveryOrderSlip",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@DocumentTypeId","const:85"),
                   P("@Id","arg:id"),
                   G("@UserId","session:userId"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                Collections.emptyList()));

        /* InvStockConversionPackingMaterial665 (CommonServices :16587) - InvFoodProductionReports
           .StockConversionPackingMaterial_Summery_Rpt (org, company, @Id); rpt CompanyAddress / CompanyName. */
        add(new ReportDefinition("ssc-665", "665-InvStockConversionPackingMaterial-Summery.rpt",
                "[dbo].[USP-InvStockConversionPackingMaterial_Summery_Rpt]", "CommonServices.InvStockConversionPackingMaterial665",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* StockAdjutmentPackingMaterial215 (CommonServices :716) - BLL 0546 StockAdjustmentSlipAndRegister409
           (org, company, @Id); rpt parameters CompanyName / CompanyAddress / PrintedBy. */
        add(new ReportDefinition("sa-215", "215_StockAdjustmentPMSlip.rpt",
                "Sp_StockAdjustmentSlipAndRegister", "CommonServices.StockAdjutmentPackingMaterial215",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id")),
                Collections.emptyList()));

        /* StockTransferPackingMaterialAndStore_Slip415 (CommonServices) - BLL 0559
           StockTransferPackingMaterialAndStore_SlipandRegister: only @Id is set, the filters stay 0/null. */
        add(new ReportDefinition("st-415", "415-InvStockTransferPackingMaterialAndStore_SlipandRegister.rpt",
                "Sp_InvStockTransferPackingMaterialAndStore_SlipandRegister", "CommonServices.StockTransferPackingMaterialAndStore_Slip415",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id")),
                Collections.emptyList()));

        /* StoreSendReceipt_Slip465 (CommonServices :8018) - InvStoreSendReceipt.StoreSendReceipt_Slip. */
        add(new ReportDefinition("ssr-465", "465-StoreSendReceipt_Slip.rpt",
                "USP_StoreSendReceipt_Slip", "CommonServices.StoreSendReceipt_Slip465",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@BranchesId","session:branchId"),
                   P("@FinancialYearId","session:financialYearId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* PurchaseInvoiceDirectPM_PartySlip_245 (CommonServices :12086) - InvPurchaseInvoiceReports
           .InvPurchaseInvoiceDirect_PartySlip_Engr; BranchesId and EntryUser are never set by the caller,
           so @BranchesId and @LoginUserId go as 0 (the BLL sends both unconditionally). */
        add(new ReportDefinition("pi-dpm-245", "245_PurchaseInvoiceDirectPM_PartySlip.rpt",
                "USP_InvPurchaseInvoiceDirect_PartySlip_Engr", "CommonServices.PurchaseInvoiceDirectPM_PartySlip_245",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@BranchesId","const:0"),
                   P("@FinancialYearId","session:financialYearId"),
                   P("@DocumentTypeIds","const:245"),
                   G("@Id","arg:id"),
                   P("@LoginUserId","const:0")),
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers.rpt",
                            "SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt", ps(P("@PihId","arg:id"))))));

        /* GrnPmSlipWithSubReport214_01 (CommonServices :14348): the same slip, plus
           InvGrnandGdnReports.PurchaseOrder_WithGrnDetail_Report over the DISTINCT PurchaseOrderId
           values of the slip's own rows, joined with ",". */
        add(new ReportDefinition("grn-214-01", "214_01_GrnPackingMaterialSlip.rpt",
                "Sp_InvGrn_StoreSlip_Rpt", "CommonServices.GrnPmSlipWithSubReport214_01",
                byKey.get("grn-214").params,
                subs(new ReportDefinition.SubReport("PurchaseOrder_WithGrnDetail_SubReport.rpt",
                            "[dbo].[USP_PurchaseOrder_WithGrnDetail_Report]",
                            ps(P("@OrganizationId","session:organizationId"),
                               P("@CompanyId","session:companyId"),
                               P("@PurchaseOrderIds","rows:PurchaseOrderId"))))));

        add(new ReportDefinition("grn-1864", "1864_GoodsRecieptNotesFinish.rpt",
                "[pcc].[USP_InvGrn_SlipAndRegister]", "CommonServices.GrnConcreteSlip",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        add(new ReportDefinition("gdn-1866", "1866-InvGdnDirect_Slip.rpt",
                "[pcc].[USP_InvGdn_Slip]", "CommonServices.GenerateCustomerSlip",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                subs(new ReportDefinition.SubReport("InvGdnWagesDetail_SubReport.rpt",
                            "[pcc].[USP_InvGdnWagesDetail_SubReport]", ps(P("@Id","arg:id"))))));

        add(new ReportDefinition("gdn-703", "703_Gdn_PurchaseReturnSlip.rpt",
                "[dbo].[USP_InvGdn_PurchaseReturnReport]",
                "CommonServices.Gdn_PurchaseReturnSlip703",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* --------------------------------------------------------------------------- Export */

        add(new ReportDefinition("exp-216", "216-ExportReturnInvoiceSlip.rpt",
                "[dbo].[USp_ExportReturnInvoice_SlipAndRegister]",
                "CommonServices.ExportReturnInvoiceSlip242",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* ------------------------------------------------------------------ Inventory / Stock */

        /* btnPrint_Click prints the grid the SCREEN already loaded, so the data source is the
           screen's own load call, not something inside the print handler.
           frmStockReportWithValues:870  dtStockSum = StocksReport.stockReportWithValues(obj) */
        List<ReportDefinition.Param> stockValues = ps(
                P("@OrganizationId","session:organizationId"),
                P("@CompanyId","session:companyId"),
                G("@DateFrom","arg:fromDate"),       G("@DateTo","arg:toDate"),
                G("@ItemTypeId","arg:itemTypeId"),   G("@ItemCategoryId","arg:itemCategoryId"),
                G("@CropYear","arg:cropYear"),       G("@JobLotId","arg:jobLotId"),
                G("@WarehouseId","arg:warehouseId"), G("@ItemId","arg:itemId"),
                G("@IsPackSizeOn","arg:isPackSizeOn"), G("@ItemStockAc","arg:itemStockAc"),
                G("@AccountGroupId","arg:accountGroupId"), G("@PackingTypeId","arg:packingTypeId"),
                G("@IsPackTypeOn","arg:isPackTypeOn"), G("@ItemClassGroupId","arg:itemClassGroupId"));

        for (String[] r : new String[][]{
                {"stock-188", "188-ItemStockSummary.rpt"},
                {"stock-189", "189-ItemandWarehouseStockSummary.rpt"},
                {"stock-191", "191-ItemandCropYearStockSummary.rpt"},
                {"stock-193", "193-JobLotandItemStockSummary.rpt"}}) {
            add(new ReportDefinition(r[0], r[1], "Sp_ItemStockReportWithValues_Rpt",
                    "frmStockReportWithValues.btnPrint_Click (grid loaded at :870)",
                    stockValues, Collections.emptyList()));
        }

        // frmStockReport:786  dtStockSum = StocksReport.stockGeneralSummaryByWeight(obj)
        add(new ReportDefinition("stock-187", "187-ItemandPackSizeandPackingTypeStockSummary.rpt",
                "Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt",
                "frmStockReport.btnPrint_Click (grid loaded at :786)",
                stockValues, Collections.emptyList()));

        // frmStockReportWithValuesForStore:703  StocksReport.ItemStockReportWithValues_Store(obj)
        add(new ReportDefinition("stock-202", "202_ItemStockSummary.rpt",
                "Sp_ItemStockReportWithValues_Store",
                "frmStockReportWithValuesForStore.btnPrint_Click (grid loaded at :703)",
                stockValues, Collections.emptyList()));

        add(new ReportDefinition("item-200", "200-InvRptItemsList.rpt",
                "SP_Item_List_Rpt", "CommonServices.ShowReport",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId")),
                Collections.emptyList()));

        /* -------------------------------------------------------------------------- Finance */

        /* VoucherReport_118 (CommonServices :5647) - VoucherReports.VoucherValidationReport. */
        add(new ReportDefinition("acc-118", "118-AcRptVoucherSlip.rpt",
                "Sp_Accounts_VouchersValidation_Rpt", "CommonServices.VoucherReport_118",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@UserId","session:userId"),
                   G("@Id","arg:id"),
                   G("@DocumentTypeId","arg:documentTypeId"),
                   /* VoucherReport_118:5196 pushes the company pair WITHOUT the @. */
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                Collections.emptyList()));

        /* CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId) ->
           VoucherReports.VoucherSlipForInventoryReport: @Id and @DocumentTypeId only when non-zero,
           ApprovedFilter "All" so @IsApproved is never sent. */
        add(new ReportDefinition("acc-103", "103-AcRptPurchaseSalesVoucherSlip.rpt",
                "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt",
                "CommonServices.AcRptPurchaseSalesVoucherSlip_103",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id"),
                   G("@DocumentTypeId","arg:documentTypeId")),
                Collections.emptyList()));

        /* ------------------------------------------------------ Sale Invoice (InvfrmSaleInvoice, 95)
           CommonServices.SaleInvoicetSlip_301 / _303 / SaleInvoiceItemSlip_303A (WinApp Common) ->
           InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt (BLL 0128:1185): @Id and @PbmId
           together when the id is non-zero, @DocumentTypeId when non-zero. The template parameters
           are pushed WITHOUT the @ ("CompanyAddress", "CompanyName"). */
        add(new ReportDefinition("si-301", "301-InvRepSaleBillCustomer.rpt",
                "Sp_InvSaleInvoice_CustomerBillRice_Rpt",
                "CommonServices.SaleInvoicetSlip_301",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id"),
                   G("@PbmId","arg:id"),
                   G("@DocumentTypeId","const:95"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers",
                            "[dbo].[SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep]", ps(P("@PihId","arg:id"))),
                     new ReportDefinition.SubReport("InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt",
                            "[dbo].[Sp_InvSaleInvoice_CustomerBillRice_JournalExp_SubRep]", ps(P("@JouranId","arg:id"))),
                     /* InvPurchaseInvoiceReports.GetDataForLedgerSubReport (BLL 0132): the slip's
                        ReportsParameters carries no branch, so @BranchesId is always sent as 0. */
                     new ReportDefinition.SubReport("getLedgerforPrint.rpt",
                            "[dbo].[usp_getLedgerforPrint]",
                            ps(P("@OrganizationId","session:organizationId"),
                               P("@CompanyId","session:companyId"),
                               P("@BranchesId","const:0"),
                               G("@FromDate","arg:fromDate"),
                               G("@ToDate","arg:toDate"),
                               G("@SupplierCustomerId","arg:supplierCustomerId"),
                               G("@AccountId","arg:accountId"))))));

        add(new ReportDefinition("si-303", "303-InvRepSaleBillCustomer-Format-II.rpt",
                "Sp_InvSaleInvoice_CustomerBillRice_Rpt",
                "CommonServices.SaleInvoicetSlip_303",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id"),
                   G("@PbmId","arg:id"),
                   G("@DocumentTypeId","arg:documentTypeId"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers.rpt",
                            "[dbo].[SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep]", ps(P("@PihId","arg:id"))))));

        /* CommonServices.SaleInvoicetSlip_318 / _318A (171): InvSaleInvoice_CustomerBillRice_Rpt with the
           form's DocumentTypeId and the SalesCustomerBillSubReport sub-report. */
        add(new ReportDefinition("si-318", "318-InvRepSaleBillCustomer.rpt",
                "Sp_InvSaleInvoice_CustomerBillRice_Rpt",
                "CommonServices.SaleInvoicetSlip_318",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id"),
                   G("@PbmId","arg:id"),
                   G("@DocumentTypeId","arg:documentTypeId"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers.rpt",
                            "[dbo].[SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep]", ps(P("@PihId","arg:id"))))));

        add(new ReportDefinition("si-318a", "318A-InvRepSaleBillCustomer.rpt",
                "Sp_InvSaleInvoice_CustomerBillRice_Rpt",
                "CommonServices.SaleInvoicetSlip_318A",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id"),
                   G("@PbmId","arg:id"),
                   G("@DocumentTypeId","arg:documentTypeId"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                subs(new ReportDefinition.SubReport("InvRptPurchaseBillSupplierOthers.rpt",
                            "[dbo].[SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep]", ps(P("@PihId","arg:id"))))));

        add(new ReportDefinition("si-303a", "303A_SaleInvoiceItemSlip.rpt",
                "Sp_InvSaleInvoice_CustomerBillRice_Rpt",
                "CommonServices.SaleInvoiceItemSlip_303A",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@Id","arg:id"),
                   G("@PbmId","arg:id"),
                   G("@DocumentTypeId","const:95"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"),
                   P("rpt:CompanyName","same:@CompanyName")),
                subs(new ReportDefinition.SubReport("SaleInvoiceItemExpense_SubReport.rpt",
                            "[dbo].[USP_InvSaleInvoiceItemExpense_SubReport]", ps(P("@InvSaleInvoiceId","arg:id"))))));

        /* ------------------------------------------------------ Sale Order (the 1852 family)

           NOTE ON NAMES. A list of "1054_*" and "1864_*" Sale Order reports does not exist in
           this system: 1054 is the GRN Supplier Loading family and 1864 is GrnRegister / stock
           summaries. The real Sale Order reports are 1053_* and 1852_*, verified by searching
           the whole GoldenAceRice tree for "*SaleOrder*". Registered under their real names. */

        // CommonServices.SaleOrderSlipConcrete
        add(new ReportDefinition("so-1852-slip", "1852-InvRptSaleOrderSlip.rpt",
                "[pcc].[USP_SaleOrderSlipAndRegister]",
                "CommonServices.SaleOrderSlipConcrete (CommonServices.cs:17970)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        // CommonServices.SaleOrderReports273_01 - Rice slip with two sub-reports
        add(new ReportDefinition("so-273-01", "273_01_SaleOrderSlip.rpt",
                "Sp_SaleOrder_RiceSlip_Rpt",
                "CommonServices.SaleOrderReports273_01 (CommonServices.cs:9091)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                subs(new ReportDefinition.SubReport("SaleOrderCustomerExpense_SubReport.rpt",
                            "[dbo].[USP_SaleOrderCustomerExpense_SubReport]", ps(P("@Id","arg:id"))))));

        // CommonServices.SaleOrderReport_1110
        add(new ReportDefinition("so-1110", "1110-SaleOrderSlip.rpt",
                "[dbo].[USP_SaleOrderCustomerExpense_SubReport]",
                "CommonServices.SaleOrderReport_1110 (CommonServices.cs:11790)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                subs(new ReportDefinition.SubReport("SaleOrderPaymentTermsDetail_SubReport.rpt",
                            "[fed].[usp_SaleOrderPaymentTermsDetail_SubReport]", ps(P("@Id","arg:id"))))));

        /* --------------------------------------------- Purchase Order Engr (the 1600 report)

           This one IS on disk. It is spelled 1600-PurchaseOrder_Slip_Engr.rpt with a HYPHEN;
           an underscore lookup misses it. */
        add(new ReportDefinition("po-1600", "1600-PurchaseOrder_Slip_Engr.rpt",
                "USP_PurchaseOrder_Slip_Engr",
                "CommonServices.PurchaseOrderSlip_Engr1600 (CommonServices.cs:12281)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* ------------------------------------------------------------- Services Bill (1304)

           1304 is the SERVICES bill header, not a Sale Order header. */
        add(new ReportDefinition("svc-1304", "1304_ServicesBillHeader_Slip.rpt",
                "[lgstcm].[USP_ServicesBillHeader_Slip]",
                "CrystalReportPrint_Helper.ServicesBillHeader_Slip_1304 (:538)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        add(new ReportDefinition("frt-241", "241-FreightVoucherSlip.rpt",
                "SP_FreightVoucherSlipAndRegister",
                "CommonServices.FreightVoucherSlip241",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* ------------------------------------------------ Production Job Order (screen 281, 403)

           CommonServices.PrintJobOrder620:14005, called from frmProductionJobOrderMain's toolbar
           "620-Print", the history grid's Print column, and after a save when Preview is ticked.
           BLL InvProductionJobOrder.GetPrintSlipAndReport sends @hId only when Id != 0 (guarded),
           then @OrganizationId and @CompanyId; the sub-report is
           JobOrderPlantAndScheduleSubReport(Id) -> @ProductionJobOrderId. @CompanyName and
           @CompanyAddress are the two RptPerameter calls ReportDataService already adds. */
        add(new ReportDefinition("jo-620", "620-ProductionJobOrderSlip.rpt",
                "Sp_InvProductionJobOrder_Slip_Rpt",
                "CommonServices.PrintJobOrder620 (:14005)",
                ps(G("@hId","arg:id"),
                   P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId")),
                subs(new ReportDefinition.SubReport("ProductionJobOrder_SubReport.rpt",
                        "[dbo].[USP_ProductionJobOrder_SubReport]",
                        ps(P("@ProductionJobOrderId","arg:id"))))));

        /* 975 frmProductionJobOrderSummaryRpt.BtnPrint_Click - prints the RAW result of
           InvFoodProduction.JobOrderSummary (dtGridFromDb), same guards as Show:
           @IsApproved only when not "All", @FromDate only when ticked, @ToDate always. */
        add(new ReportDefinition("jos-672", "672_JobOrderSummaryReport.rpt",
                "[dbo].[usp_JobOrderSummary_Report]",
                "frmProductionJobOrderSummaryRpt.BtnPrint_Click (:311)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"),
                   P("@ToDate","arg:toDate"),
                   G("@IsApproved","arg:isApproved")),
                Collections.emptyList()));

        /* 975 DataGridHistory_LinkClicked:210 - the JobOrderNo link. actionId is fixed at 1 by
           the form; the sub-report sends @JobOrderId only when > 0 (BLL guard). */
        add(new ReportDefinition("jo-613-02", "613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt",
                "[dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]",
                "frmProductionJobOrderSummaryRpt.DataGridHistory_LinkClicked (:210)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@JobOrderId","arg:id"),
                   P("@ActionId","const:1")),
                subs(new ReportDefinition.SubReport("612-InvRptProductionDetailWithExpValues.rpt",
                        "[dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]",
                        ps(G("@JobOrderId","arg:id"))))));

        /* ------------------------------------------- 309 ProductionSummaryReport (and 308, 280) */

        /* CommonServices.ProductionRecoveryReport602:8287 - InvFoodProduction_Summery_WithOutValue_Rpt:
           @PlantId when != 0; @FromDate/@ToDate only when BOTH are set (useDateFilter && both);
           @BranchesIds null when blank. Sub-report DailyPlantConsumedHoursByJobOrderId_SubReport
           (@PlantId when != 0). The page sends the dates only as a pair. */
        List<ReportDefinition.Param> p602 = ps(P("@OrganizationId","session:organizationId"),
                P("@CompanyId","session:companyId"),
                P("@JobOrderId","arg:id"),
                G("@PlantId","arg:plantId"),
                G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"),
                G("@BranchesIds","arg:branchesIds"));
        ReportDefinition.SubReport dailyHours = new ReportDefinition.SubReport(
                "DailyPlantConsumedHoursByJobOrderId_SubReport",
                "usp_DailyPlantConsumedHoursByJobOrderId_SubReport",
                ps(P("@JobOrderId","arg:id"), G("@PlantId","arg:plantId")));
        add(new ReportDefinition("ps-602", "602-InvRptProductionSummery.rpt",
                "Sp_InvFoodProduction_Summery_WithOutValue_Rpt",
                "CommonServices.ProductionRecoveryReport602 (:8287)", p602, subs(dailyHours)));

        /* CommonServices.ProductionRecoveryReportWithLab_602_01:8325 - same data, plus the lab sub-report. */
        add(new ReportDefinition("ps-602-01", "602_01_ProductionSummeryWithLab.rpt",
                "Sp_InvFoodProduction_Summery_WithOutValue_Rpt",
                "CommonServices.ProductionRecoveryReportWithLab_602_01 (:8325)", p602,
                subs(dailyHours, new ReportDefinition.SubReport(
                        "usp_InvProductionJobOrderLabStandardDetail_SubReport.rpt",
                        "usp_InvProductionJobOrderLabStandardDetail_SubReport",
                        ps(P("@JobOrderId","arg:id"))))));

        /* ProductionSummaryReport.btnPrintForDetailGridData_Click:909 - the raw doc-wise result
           (USP_FoodProduction_DocWiseSummeryReport, the Show parameters: @PlantId when != 0,
           each date by its own tick, @BranchesIds always). */
        add(new ReportDefinition("ps-625", "625_FoodProduction_DocWiseSummeryReport.rpt",
                "[dbo].[USP_FoodProduction_DocWiseSummeryReport]",
                "ProductionSummaryReport.btnPrintForDetailGridData_Click (:909)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@JobOrderId","arg:id"),
                   G("@PlantId","arg:plantId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"),
                   P("@BranchesIds","arg:branchesIds")),
                Collections.emptyList()));

        /* InvFoodProductionReports.FoodProductionIssuanceGrnWiseByJobOrderId_607 - every parameter
           but tenancy guarded. 309's "Input Detail" drop-down (no plant/item/uom, with branches)
           and 308's "607-InPutDetail_Register" (plant/item/pack uom, no branches) both land here. */
        add(new ReportDefinition("prod-607", "607-FoodProductionIssuanceGrnWiseByJobOrderId.rpt",
                "Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt",
                "ProductionSummaryReport.tsDropDowngrnWiseInput (:320) / FoodProductionComparisonRpt.btnPrint607Register (:1181)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@InvJobOrderId","arg:id"),
                   G("@PlantId","arg:plantId"),
                   G("@ItemId","arg:itemId"),
                   G("@PackUomId","arg:packUomId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"),
                   G("@BranchesIds","arg:branchesIds")),
                Collections.emptyList()));

        /* -------------------------------------------------------- 308 FoodProductionComparisonRpt */

        /* toolStripButton1_Click:1162 prints dtReg (SpInvFoodProductionComparisons_Rpt). This
           form pushes "CompanyName"/"CompanyAddress" WITHOUT the @. */
        add(new ReportDefinition("pc-604", "604-FoodProductionComparisonRpt.rpt",
                "SpInvFoodProductionComparisons_Rpt",
                "FoodProductionComparisonRpt.toolStripButton1_Click (:1162)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@ProductionType","arg:productionType"),
                   G("@DocNoFrom","arg:docNoFrom"), G("@DocNoTo","arg:docNoTo"),
                   G("@PlantId","arg:plantId"), G("@JobOrderId","arg:jobOrderId"),
                   P("rpt:CompanyName","same:@CompanyName"),
                   P("rpt:CompanyAddress","same:@CompanyAddress")),
                Collections.emptyList()));

        /* DataGridHistory_ColumnButtonClick:730 - the row Print column: InvFoodProductionRecoverySummeryReport
           (Sp_InvFoodProduction_Summery_Rpt, @JobOrderId only) into the 602 template, plus the
           daily-hours sub-report (added only when it has rows on the desktop). */
        add(new ReportDefinition("pc-602-row", "602-InvRptProductionSummery.rpt",
                "Sp_InvFoodProduction_Summery_Rpt",
                "FoodProductionComparisonRpt.DataGridHistory_ColumnButtonClick (:730)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@JobOrderId","arg:id")),
                subs(new ReportDefinition.SubReport("DailyPlantConsumedHoursByJobOrderId_SubReport",
                        "usp_DailyPlantConsumedHoursByJobOrderId_SubReport",
                        ps(P("@JobOrderId","arg:id"))))));

        /* ----------------------------------------------------------- 310 ProductionRegister */

        /* btnPrintCurrent_Click:1021 prints dt1 (the Show result) into 553 / 554 / 562 by the
           Activity (PrintButtonManage:991). Same guarded parameter set as the Show. */
        String[][] registerPrints = {
                { "pr-553", "553-ProductionRegisterWithActivity.rpt", "Production Register" },
                { "pr-554", "554-ProductionRegisterWithActivity(OutPut By Packing Material).rpt", "OutPut By Packing Material" },
                { "pr-562", "562-ProductionRegisterWithActivity.rpt", "Production_Summary" } };
        for (String[] r : registerPrints) {
            add(new ReportDefinition(r[0], r[1], "USP_ProductionRegisterWithActivity",
                    "ProductionRegister.btnPrintCurrent_Click (:1021)",
                    ps(P("@OrganizationId","session:organizationId"),
                       P("@CompanyId","session:companyId"),
                       G("@InvJobOrderId","arg:jobOrderId"),
                       G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"),
                       G("@EntryFromDate","arg:entryFromDate"), G("@EntryToDate","arg:entryToDate"),
                       G("@PlantId","arg:plantId"), G("@ItemId","arg:itemId"),
                       G("@ItemStockAccountId","arg:stockAccountId"),
                       G("@WarehouseId","arg:warehouseId"),
                       G("@ParentCategoriesId","arg:parentCategoryId"),
                       G("@WipAccountId","arg:wipAccountId"),
                       G("@EntryTypeDetail","arg:entryTypeDetail"),
                       G("@BranchesIds","arg:branchesIds"),
                       P("@Activity","const:" + r[2])),
                    Collections.emptyList()));
        }

        /* ------------------------------------ 306 frmProductionPackingMaterialConsumptionRegister */

        /* PrintRegister:321 - the Show result into 812, with @PrintedBy = UserAccount.UserName. */
        add(new ReportDefinition("pm-812", "812-ProductionKamPackMaterialConsumption.rpt",
                "Sp_InvProductionCumPackMaterialConsumption_Register",
                "frmProductionPackingMaterialConsumptionRegister.PrintRegister (:321)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"),
                   G("@PmItemId","arg:pmItemId"), G("@ItemId","arg:itemId"),
                   G("@BranchesIds","arg:branchesIds"),
                   P("rpt:@PrintedBy","session:userName")),
                Collections.emptyList()));

        /* ------------------------------------------------ Stock Conversion (DocTypeId 66) */

        /* CommonServices.StockConversionSummary_605:7661 - SpInvStockConversion_Summery_Rpt @Id. */
        add(new ReportDefinition("sc-605", "605-StockConversionSummaryNewRpt.rpt",
                "SpInvStockConversion_Summery_Rpt",
                "CommonServices.StockConversionSummary_605 (:7661)",
                ps(P("@OrganizationId","session:organizationId"),
                   P("@CompanyId","session:companyId"),
                   P("@Id","arg:id")),
                Collections.emptyList()));

        /* ---------------- screen 280 hosted tabs (Input / Output / PM / Overhead / Consumption / Settlement) */
        /* frmProductionInput.Print_Click / frmProductionOutput.btnPrintOutPut_Click -> CommonServices.ProductionInPutAndOutPut601
           ReqType "" -> @EntryType omitted; BranchesId 0 -> omitted. */
        add(new ReportDefinition("prod-601", "601-InvFoodProductionSlip.rpt", "Sp_InvFoodProduction_Rpt",
                "CommonServices.ProductionInPutAndOutPut601",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), G("@Id","arg:id")), Collections.emptyList()));
        /* History "WagesPrint" -> CommonServices.ContractorWagesBill_SlipandRegister_002. ApprovedFilter is null,
           so (null != "All") holds and IsApproved=false is always sent as @FreeOfCost. */
        add(new ReportDefinition("wages-002", "002-ContractorWagesSlip.rpt", "Sp_InvContractorWagesBillHeader_SlipandRegister",
                "CommonServices.ContractorWagesBill_SlipandRegister_002",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@DocumentTypeId","const:101"), G("@Id","arg:id"), P("@FreeOfCost","const:0"),
                   P("rpt:@CompanyName","same:@CompanyName"), P("rpt:@CompanyAddress","same:@CompanyAddress")),
                Collections.emptyList()));
        /* FoodProductionWithValues consumption print -> CommonServices.ProductionConsumptionReport_623 */
        add(new ReportDefinition("prod-623", "623-InvFoodProduction_ConsumptionReport.rpt",
                "[dbo].[USP_InvFoodProduction_ConsumptionReport]", "CommonServices.ProductionConsumptionReport_623",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), G("@Id","arg:id")), Collections.emptyList()));
        /* frmProductionPackingMaterial.btnPackingMaterialSlip_Click */
        add(new ReportDefinition("prod-606-pm", "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt",
                "Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder", "frmProductionPackingMaterial.btnPackingMaterialSlip_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@ReportType","const:PackingMaterial"), G("@JobOrderId","arg:jobOrderId"),
                   P("rpt:@CompanyAddress","same:@CompanyAddress"), P("rpt:@CompanyName","same:@CompanyName")),
                Collections.emptyList()));
        /* frmProductionOverhead.btnPrintOverHead_Click */
        add(new ReportDefinition("pr-606", "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt",
                "Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder", "frmProductionOverhead.btnPrintOverHead_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@ReportType","const:OverHead"), G("@JobOrderId","arg:jobOrderId"),
                   P("rpt:@CompanyAddress","same:@CompanyAddress"), P("rpt:@CompanyName","same:@CompanyName")),
                Collections.emptyList()));
        /* frmProductionSettlement prints: @JobOrderId always, @ActionId guarded */
        add(new ReportDefinition("p280s-613", "613-InvRptProductionSummeryWithExpValues.rpt", "Sp_InvFoodProduction_Summery2_Rpt", "frmProductionSettlement.btn613SummaryReport_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@JobOrderId","arg:id"), G("@ActionId","arg:actionId")),
                Collections.emptyList()));
        add(new ReportDefinition("p280s-613A", "613_01-ProductionBeforSettlement_Summery2.rpt", "usp_ProductionBeforSettlement_Summery2_Rpt", "frmProductionSettlement.BtnPrint613A_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@JobOrderId","arg:id"), G("@ActionId","arg:actionId")),
                Collections.emptyList()));
        add(new ReportDefinition("p280s-615", "615-InvRptProductionSummery.rpt", "Sp_InvFoodProduction_Summery3_Rpt", "frmProductionSettlement.btn615Report_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@JobOrderId","arg:id"), G("@ActionId","arg:actionId")),
                Collections.emptyList()));
        add(new ReportDefinition("p280s-615A", "615_01-ProductionBeforSettlement_Summery3.rpt", "usp_ProductionBeforSettlement_Summery3_Rpt", "frmProductionSettlement.BtnPrint615A_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@JobOrderId","arg:id"), G("@ActionId","arg:actionId")),
                Collections.emptyList()));
        add(new ReportDefinition("p280s-602A", "602A-InvRptProductionSummeryWithValues.rpt", "Sp_InvFoodProduction_Summery_Rpt", "frmProductionSettlement.btnRecoveryReport_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@JobOrderId","arg:id"), G("@ActionId","arg:actionId")),
                Collections.emptyList()));
        add(new ReportDefinition("p280s-613B", "613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt", "[dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]", "frmProductionSettlement.BtnPrint613B_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), P("@JobOrderId","arg:id"), G("@ActionId","arg:actionId")),
                subs(new ReportDefinition.SubReport("612-InvRptProductionDetailWithExpValues.rpt", "[dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]", ps(G("@JobOrderId","arg:id"))))));
        /* frmDailyPlantConsumedHours.btnPrint_Click (screen 877) - company pair pushed without the @ */
        add(new ReportDefinition("dpch-666", "666-DailyPlantConsumedHours_FormHistoryReport.rpt",
                "[dbo].[USP_DailyPlantConsumedHours_FormHistoryReport]", "frmDailyPlantConsumedHours.btnPrint_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"),
                   G("@EntryFromDate","arg:entryFromDate"), G("@EntryToDate","arg:entryToDate"),
                   G("@ModifyFromDate","arg:modifyFromDate"), G("@ModifyToDate","arg:modifyToDate"),
                   G("@JobOrderId","arg:jobOrderId"), G("@ReasonId","arg:reasonId"),
                   P("rpt:CompanyAddress","same:@CompanyAddress"), P("rpt:CompanyName","same:@CompanyName")),
                Collections.emptyList()));
        /* ProductionOutputAllocationWithExportInvoice.Slip */
        add(new ReportDefinition("poa-627", "627-ProductionOutputAllocationWithExportInvoice.rpt",
                "[dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]",
                "ProductionOutputAllocationWithExportInvoice.Slip",
                ps(G("@JobOrderId","arg:jobOrderId")), Collections.emptyList()));
        /* frmEvaulationDetailWagesReports.btnPrint_Click - one procedure, five templates */
        add(new ReportDefinition("wr-160", "160-WagesRegister.rpt", "dbo.USp_WagesRegister", "frmEvaulationDetailWagesReports.btnPrint_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"), G("@WagesAccountId","arg:wagesAccountId"),
                   G("@ContractorId","arg:contractorId"), G("@ReferenceDocumentTypeId","arg:documentTypeId"),
                   G("@DebitAccountId","arg:debitAccountId"), G("@BranchesIds","arg:branchesIds"), G("@ActionId","arg:actionId"),
                   G("@StockPartyId","arg:stockPartyId"), G("@JobOrderId","arg:jobOrderId"),
                   G("@ReferenceDocumentTypeIds","arg:referenceDocumentTypeIds"), G("@FreeOfCost","arg:freeOfCost"),
                   G("@ActivityName","arg:activityName"), G("@BranchWise","arg:branchWise"),
                   P("rpt:@PrintedBy","session:userName")), Collections.emptyList()));
        add(new ReportDefinition("wr-161", "161-WagesbyContractor&DocumentType.rpt", "dbo.USp_WagesRegister", "frmEvaulationDetailWagesReports.btnPrint_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"), G("@WagesAccountId","arg:wagesAccountId"),
                   G("@ContractorId","arg:contractorId"), G("@ReferenceDocumentTypeId","arg:documentTypeId"),
                   G("@DebitAccountId","arg:debitAccountId"), G("@BranchesIds","arg:branchesIds"), G("@ActionId","arg:actionId"),
                   G("@StockPartyId","arg:stockPartyId"), G("@JobOrderId","arg:jobOrderId"),
                   G("@ReferenceDocumentTypeIds","arg:referenceDocumentTypeIds"), G("@FreeOfCost","arg:freeOfCost"),
                   G("@ActivityName","arg:activityName"), G("@BranchWise","arg:branchWise"),
                   P("rpt:@PrintedBy","session:userName")), Collections.emptyList()));
        add(new ReportDefinition("wr-162", "162-WagesByContractor.rpt", "dbo.USp_WagesRegister", "frmEvaulationDetailWagesReports.btnPrint_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"), G("@WagesAccountId","arg:wagesAccountId"),
                   G("@ContractorId","arg:contractorId"), G("@ReferenceDocumentTypeId","arg:documentTypeId"),
                   G("@DebitAccountId","arg:debitAccountId"), G("@BranchesIds","arg:branchesIds"), G("@ActionId","arg:actionId"),
                   G("@StockPartyId","arg:stockPartyId"), G("@JobOrderId","arg:jobOrderId"),
                   G("@ReferenceDocumentTypeIds","arg:referenceDocumentTypeIds"), G("@FreeOfCost","arg:freeOfCost"),
                   G("@ActivityName","arg:activityName"), G("@BranchWise","arg:branchWise"),
                   P("rpt:@PrintedBy","session:userName")), Collections.emptyList()));
        add(new ReportDefinition("wr-163", "163-WagesByDocumentType.rpt", "dbo.USp_WagesRegister", "frmEvaulationDetailWagesReports.btnPrint_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"), G("@WagesAccountId","arg:wagesAccountId"),
                   G("@ContractorId","arg:contractorId"), G("@ReferenceDocumentTypeId","arg:documentTypeId"),
                   G("@DebitAccountId","arg:debitAccountId"), G("@BranchesIds","arg:branchesIds"), G("@ActionId","arg:actionId"),
                   G("@StockPartyId","arg:stockPartyId"), G("@JobOrderId","arg:jobOrderId"),
                   G("@ReferenceDocumentTypeIds","arg:referenceDocumentTypeIds"), G("@FreeOfCost","arg:freeOfCost"),
                   G("@ActivityName","arg:activityName"), G("@BranchWise","arg:branchWise"),
                   P("rpt:@PrintedBy","session:userName")), Collections.emptyList()));
        add(new ReportDefinition("wr-164", "164-WagesSummaryByContractorAndJobOrder.rpt", "dbo.USp_WagesRegister", "frmEvaulationDetailWagesReports.btnPrint_Click",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"),
                   G("@FromDate","arg:fromDate"), G("@ToDate","arg:toDate"), G("@WagesAccountId","arg:wagesAccountId"),
                   G("@ContractorId","arg:contractorId"), G("@ReferenceDocumentTypeId","arg:documentTypeId"),
                   G("@DebitAccountId","arg:debitAccountId"), G("@BranchesIds","arg:branchesIds"), G("@ActionId","arg:actionId"),
                   G("@StockPartyId","arg:stockPartyId"), G("@JobOrderId","arg:jobOrderId"),
                   G("@ReferenceDocumentTypeIds","arg:referenceDocumentTypeIds"), G("@FreeOfCost","arg:freeOfCost"),
                   G("@ActivityName","arg:activityName"), G("@BranchWise","arg:branchWise"),
                   P("rpt:@PrintedBy","session:userName")), Collections.emptyList()));
        /* frmProductionSettlement.btnSettlementVoucher_Click -> CommonServices.AcRptPurchaseSalesVoucherSlip_103 */
        add(new ReportDefinition("p280s-103", "103-AcRptPurchaseSalesVoucherSlip.rpt", "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt",
                "CommonServices.AcRptPurchaseSalesVoucherSlip_103",
                ps(P("@OrganizationId","session:organizationId"), P("@CompanyId","session:companyId"), G("@Id","arg:id"), G("@DocumentTypeId","arg:documentTypeId")), Collections.emptyList()));
    }

    /**
     * Requested reports that are NOT registered, with the reason. Exposed by the controller so
     * the gap is visible rather than silently missing.
     *
     * "no such file" entries were checked against the whole GoldenAceRice tree AND against every
     * "*.rpt" literal in the desktop source. Absent from both means the name does not exist in
     * this system - building an endpoint for it would be fabrication.
     */
    public static final Map<String, String> UNAVAILABLE;
    static {
        Map<String, String> m = new LinkedHashMap<>();
        /* RENAMED, NOT MISSING - these names resolve to real files under different numbers.
           1054 is the GRN Supplier Loading family and 1864 is GrnRegister / stock summaries;
           the Sale Order family is 1053 and 1852. Each is registered under its real name. */
        m.put("1600_PurchaseOrder_Slip_Engr.rpt", "RENAMED -> 1600-PurchaseOrder_Slip_Engr.rpt (hyphen). Registered as po-1600.");
        m.put("1054_01_SaleOrderReport.rpt",      "RENAMED -> 1053_01_SaleOrderDetailReport.rpt family. 1054_* is GRN Supplier Loading.");
        m.put("1054_SaleOrderSlip.rpt",           "RENAMED -> 273_01_SaleOrderSlip.rpt / 1110-SaleOrderSlip.rpt. Registered as so-273-01 and so-1110.");
        m.put("1304_SaleOrderHeader_Slip.rpt",    "RENAMED -> 1304_ServicesBillHeader_Slip.rpt (a SERVICES bill, not a sale order). Registered as svc-1304.");
        m.put("1864-InvRptSaleOrderSlip.rpt",     "RENAMED -> 1852-InvRptSaleOrderSlip.rpt. Registered as so-1852-slip.");
        m.put("1864-SaleOrderRegister.rpt",       "RENAMED -> 1852-SaleOrderRegister.rpt. BLOCKED - procedure IS now known: [pcc].[USP_SaleOrderSlipAndRegister]. Still blocked: grid-backed.");
        m.put("1864A-SaleOrder_Register.rpt",     "RENAMED -> 1852A-SaleOrder_Register.rpt. BLOCKED - procedure IS now known: [pcc].[USP_SaleOrderSlipAndRegister]. Still blocked: grid-backed.");
        m.put("1053_01_SaleOrderDetailReport.rpt","BLOCKED - on disk but no caller found in the desktop source; its parameters are unknown.");
        m.put("1052_01_PurchaseOrderReport.rpt",  "BLOCKED - on disk but no caller found in the desktop source; its parameters are unknown");
        m.put("220-InvRptPurchaseBillSupplierRiceSlip.rpt", "BLOCKED - main procedure IS now known: Sp_InvPurchaseInvoice_SupplierBill_Rpt (InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220). Still blocked: 15 launch sites disagree on the sub-reports; pick one screen and trace it.");
        m.put("1863-PurchaseOrderRegister.rpt",   "BLOCKED - procedure IS now known: [fed].[USP_PurchaseOrderSummaryRegister] (PurchaseOrderRegister.printRegister_Click). Still blocked: prints the screen's grid, so the filter values come from the form, not a ReportsParameters initializer.");
        m.put("224-InvRptPurchaseBillRegister.rpt", "BLOCKED - procedure IS now known: Sp_InvPurchaseInvoice_Rpt (frmPurchaseInvoiceHistory.btnSlip_Click via the PurchaseInvoiceRegisterDetail(String) helper). Still blocked: grid-backed, filters come from the form.");
        m.put("234-PurchaseRegisterSummary.rpt",  "BLOCKED - procedure IS now known: Usp_InvPurchaseInvoice_PurchasingReports (PurchaseRegisterPopUp.GridFill). Still blocked: prints a hand-built DataTable assembled from the grid, not the procedure's own rows.");
        m.put("1528-PurchaseRegisterSummary.rpt", "BLOCKED - procedure IS now known: [ST].[USP-PurchaseInvoiceRegisterWithActivities]. Still blocked: grid-backed.");
        m.put("1804_01_PurchaseRegister.rpt",     "BLOCKED - procedure IS now known: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]. Still blocked: grid-backed.");
        m.put("1880-SalesRegisterSummary.rpt",    "BLOCKED - procedure IS now known: [pcc].[USP_Sales_EvaulationDetailReports]. Still blocked: grid-backed.");
        m.put("1886_01_StockReportWithDocumentWiseIncludeOrders.rpt", "BLOCKED - procedure IS now known: pcc.USP_StockReportWithDocumentWiseIncludeOrders. Still blocked: grid-backed.");
        m.put("240-FreightVoucherRegister.rpt",   "BLOCKED - procedure IS now known: SP_FreightVoucherSlipAndRegister - the SAME procedure as 241; it returns a DataSet whose Tables[0] is the rows and Tables[1] the company logo. Still blocked: grid-backed, filters come from the form.");
        UNAVAILABLE = Collections.unmodifiableMap(m);
    }

    public ReportDefinition get(String key) { return byKey.get(key); }
    public List<ReportDefinition> all() { return new ArrayList<>(byKey.values()); }
}
