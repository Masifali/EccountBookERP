package com.mst.repositories;

import java.time.LocalDate;
import java.util.Date;
import java.util.List;

import javax.persistence.Tuple;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoice;
import com.mst.projections.PurchaseInvoiceDetailReadRow;
import com.mst.projections.PurchaseInvoiceDirectDetailRow;
import com.mst.projections.PurchaseInvoiceDirectGrnLoadRow;
import com.mst.projections.PurchaseInvoiceDirectStoreDetailRow;
import com.mst.projections.PurchaseInvoiceDropdownRow;
import com.mst.projections.PurchaseInvoiceEmptyBagsFromGrnRow;
import com.mst.projections.PurchaseInvoiceEmptyBagsRow;
import com.mst.projections.PurchaseInvoiceExpenseRow;
import com.mst.projections.PurchaseInvoiceFormHistoryRow;
import com.mst.projections.PurchaseInvoiceFormHistoryTradeProRow;
import com.mst.projections.PurchaseInvoiceFreightRow;
import com.mst.projections.PurchaseInvoiceGhallaMandiGrnLoadRow;
import com.mst.projections.PurchaseInvoiceGrnLoadTradingRow;
import com.mst.projections.PurchaseInvoiceItemQtyRateFromPoRow;
import com.mst.projections.PurchaseInvoiceJournalRow;
import com.mst.projections.PurchaseInvoiceLastTransportBrokerRow;
import com.mst.projections.PurchaseInvoicePackingMaterialDetailRow;
import com.mst.projections.PurchaseInvoicePaymentTermRow;
import com.mst.projections.PurchaseInvoicePdfAccountAmountRow;
import com.mst.projections.PurchaseInvoicePdfDetailRow;
import com.mst.projections.PurchaseInvoicePdfEmptyBagsRow;
import com.mst.projections.PurchaseInvoicePdfExpenseRow;
import com.mst.projections.PurchaseInvoicePdfHeaderDataRow;
import com.mst.projections.PurchaseInvoicePdfHeaderRow;
import com.mst.projections.PurchaseInvoicePdfJournalRow;
import com.mst.projections.PurchaseInvoicePendingGhallaMandiGrnRow;
import com.mst.projections.PurchaseInvoicePendingGrnRow;
import com.mst.projections.PurchaseInvoicePendingGrnTradingRow;
import com.mst.projections.PurchaseInvoiceProductionIssuanceRow;
import com.mst.projections.PurchaseInvoicePurchaseOrderLoadRow;
import com.mst.projections.PurchaseInvoiceReadByIdRow;
import com.mst.projections.PurchaseInvoiceReturnForApprovalRow;
import com.mst.projections.PurchaseInvoiceStoreGrnLoadRow;
import com.mst.projections.PurchaseInvoiceStoreManagementDetailRow;
import com.mst.projections.PurchaseInvoiceTradingDetailRow;
import com.mst.projections.PurchaseInvoiceTradingPurchaseDetailRow;
import com.mst.projections.PurchaseInvoiceTransporterFreightRow;
import com.mst.projections.PurchaseOrderPaymentTermRow;
import com.mst.projections.PurchaseOrderSupplierExpenseRow;

/**
 * Query-only repository for Sp_InvPurchaseInvoice_GetAllMethod (one method per @Activity branch).
 * This batch owns no table: InvPurchaseInvoice / InvPurchaseInvoiceDetail entities belong to G2_purchase_invoice_write and are
 * only referenced read-only (JPQL by registry column names, otherwise native SQL).
 * Proc parameter defaults: every parameter is NULL unless noted on the method; callers pass null for unused optional filters.
 */
@Repository
public interface InvPurchaseInvoiceReadQueryRepository extends org.springframework.data.repository.Repository<InvPurchaseInvoice, Integer> {

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='ReadById'
     * Native: joins view V_UomScheduleAndUom (no entity). Single row expected (take first).
     */
    @Query(value = "SELECT h.Id,h.DocumentTypeId,h.DocumentTypeSrNo,h.DocNo,h.DocDate, "
         + "h.SupplierCustomerId,h.SalesTaxNo,h.SupplierReferenceNo, "
         + "h.SupplierInvoiceNo,h.SupplierInvoiceDate,h.ManualBillNo, "
         + "h.StockPartyId,h.ReferencePartyId,h.CommissionAgentId, "
         + "h.CommissionType,h.CommRate,h.UomScheduleIdCmRate, "
         + "h.CommAmount,h.CommissionRemarks,h.CashReceived,h.RemarksHeader, "
         + "h.OtherRemarks,h.IsAttachments,h.IsTaxable,h.IsApproved, "
         + "h.EntryDate,h.EntryUser,h.ModifyDate,h.ModifyUser,h.PostDate, "
         + "h.PostUser,h.BranchesId,h.ProjectsId, D1.DocumentTypeCode, "
         + "SC1.CompanyName AS SupplierCustomerName, "
         + "SC2.CompanyName AS StockPartyName, "
         + "Sc3.CompanyName AS ReferenceParty, "
         + "CS4.CompanyName AS CommissionAgentName, "
         + "V_UomScheduleAndUom.Equivalent AS EquivalentRate, "
         + "V_UomScheduleAndUom.UOMCode AS UOMCodeRate, "
         + "h.CompanyId, h.OrganizationId,h.BillAmount, SC1.GlAccountId, "
         + "h.DeliveryTerm,h.ReferencePartyName, "
         + "h.FinancialYearId,h.PaymentTermsId,h.DueDate,h.DueDays "
         + ",h.BrokerAgentId,h.BrokeryType,h.BrokeryUom, "
         + "h.BrokeryRate,h.BrokeryAmount "
         + ",h.InvoiceTypeId,h.TransportAccountId, "
         + "h.FreightAmount,h.ActionId, "
         + "h.GhallaMandiId,h.DeliveryDays,h.DeliveryStartDate, "
         + "TaxAccountId,h.BillCalculateTypeId, "
         + "h.InvoiceQty, h.InvoiceWeight, "
         + "h.CurrencyId, h.ExchangeRate, h.FcyAmount, h.ScreenName, "
         + "h.WagesAmount,h.BranchSrNo,h.CommissionCreditAccountId,h.AttachmentsValues, "
         + "h.CustomAttachmentsValues, "
         + "h.DiscountAmount "
         + ",h.DiscountAccountId "
         + ",h.TransporterCreditPartyId "
         + ",h.TransporterDebitGLId "
         + ",h.TransporterDebitPartyId "
         + ",h.FreightRemark "
         + ",h.CustomAccounts "
         + ",h.IsUploaded "
         + ",h.UploadedById "
         + ",h.UploadedDate "
         + ",'' DeleteAttachmentsList "
         + "FROM InvPurchaseInvoice h "
         + "INNER JOIN SupplierCustomer AS SC1 ON h.SupplierCustomerId = SC1.Id "
         + "LEFT JOIN dbo.VoucherHead VH ON VH.DocumentTypeId = h.DocumentTypeId AND VH.DocumentTypeSrNo = h.Id "
         + "LEFT JOIN SupplierCustomer AS Sc3 ON h.ReferencePartyId = Sc3.Id "
         + "LEFT JOIN SupplierCustomer AS CS4 ON h.CommissionAgentId = CS4.Id "
         + "LEFT JOIN V_UomScheduleAndUom ON h.UomScheduleIdCmRate = V_UomScheduleAndUom.Id "
         + "LEFT JOIN SupplierCustomer AS SC2 ON h.StockPartyId = SC2.Id "
         + "LEFT JOIN DocumentType AS D1 ON h.DocumentTypeId = D1.Id "
         + "WHERE h.[Id] = :id", nativeQuery = true)
    List<PurchaseInvoiceReadByIdRow> readById(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GenerateCode'
     * Native: scalar UDF dbo.GenerateDocumentNumberDynamicly; returns the generated DocNo.
     */
    @Query(value = "SELECT dbo.GenerateDocumentNumberDynamicly(:organizationId, :companyId, :financialYearId, :documentTypeId, GETDATE(), NULL, NULL) as DocNo", nativeQuery = true)
    Integer generateCode(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("financialYearId") Integer financialYearId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GenerateBranchSrNo'
     * Output column BranchSrNo = ISNULL(MAX(BranchSrNo),0)+1.
     */
    @Query(value = "SELECT COALESCE(MAX(p.branchSrNo), 0) + 1 FROM InvPurchaseInvoice p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId "
         + "AND p.financialYearId = :financialYearId AND p.documentTypeId = :documentTypeId "
         + "AND p.branchesId = :branchesId")
    Integer generateBranchSrNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("financialYearId") Integer financialYearId, @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GenerateSalesTaxNo'
     * Proc: IF @IsTaxable = 1 → this query (SalesTaxNo = ISNULL(MAX(SalesTaxNo),0)+1); ELSE the proc returns 0 — caller must short-circuit to 0 when isTaxable != true.
     */
    @Query(value = "SELECT COALESCE(MAX(p.salesTaxNo), 0) + 1 FROM InvPurchaseInvoice p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId AND p.isTaxable = :isTaxable")
    Integer generateSalesTaxNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("isTaxable") Boolean isTaxable);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetInvoicesDetailForProductionIssence'
     * Native: view V_UomScheduleAndUom, table InvFoodProductionDetail (no entity), ROUND.
     */
    @Query(value = "SELECT PiH.Id, PiH.DocDate, PiH.DocNo ,PiH.SupplierCustomerId, "
         + "sp.CompanyName as SupplierName, PiH.ManualBillNo, "
         + "PiD.ItemId,PiD.ItemUOMId,Uom.Equivalent AS UOMCodeItem, "
         + "PiD.CropYear,PiD.JobLotId,j.JobLotCode,PiD.PackingTypeId, "
         + "p.PackTypeCode,w.WareHouseName,PiD.WarehouseId, "
         + "Item.ItemName,PID.UomScheduleIdRate as RateUOMId, "
         + "UomRate.Equivalent as RateUOM, "
         + "SUM(ISNULL(PiD.ItemQty,0)) - SUM(ISNULL(FPD.Qty,0)) AS ItemQty , "
         + "SUM(ISNULL(PiD.NetBillWeight,0)) - SUM(ISNULL(FPD.Weight,0)) AS NetBillWeight, "
         + "CASE "
         + "WHEN SUM(ISNULL(PiD.NetBillWeight,0)) > 0 AND Isnull(UomRate.Equivalent, 0) > 0 "
         + "THEN ROUND(SUM(ISNULL(pid.BillAmount,0)) / SUM(ISNULL(PiD.NetBillWeight,0)) * UomRate.Equivalent,2) "
         + "ELSE 0 END AS ItemRate, "
         + "ROUND(SUM(ISNULL(pid.BillAmount,0)) - SUM(ISNULL(FPD.Amount,0)),2) AS ItemAmount, "
         + "PID.CurrencyId, PID.ExchangeRate, PID.FcyAmount "
         + "FROM dbo.InvPurchaseInvoice AS PiH "
         + "INNER JOIN dbo.InvPurchaseInvoiceDetail AS PiD ON PiH.Id = PiD.InvPurchaseInvoiceId "
         + "Inner Join SupplierCustomer sp on PiH.SupplierCustomerId = sp.Id "
         + "Inner JOIN dbo.Item ON PiD.ItemId = dbo.Item.Id "
         + "Inner JOIN dbo.V_UomScheduleAndUom AS UomRate ON PiD.UomScheduleIdRate = UomRate.Id "
         + "LEFT JOIN dbo.UOMSchedule AS Uom ON PiD.ItemUOMId = Uom.Id "
         + "LEFT JOIN dbo.JobLot j ON PiD.JobLotId = j.Id "
         + "LEFT JOIN dbo.InvPackingType p ON PiD.PackingTypeId = p.Id "
         + "LEFT JOIN dbo.InvWareHouse w ON PiD.WarehouseId = w.Id "
         + "left join InvFoodProductionDetail FPD on PiH.Id = FPD.RefDocNoId "
         + "WHERE PiH.OrganizationId = :organizationId "
         + "AND (:companyId IS NULL or PiH.CompanyId = :companyId) "
         + "AND (:supplierCustomerId IS NULL or pih.SupplierCustomerId = :supplierCustomerId) "
         + "AND (:fromDate IS NULL OR PiH.DocDate >= :fromDate) "
         + "AND (:toDate IS NULL OR PiH.DocDate <= :toDate) "
         + "AND (:itemId IS NULL OR PiD.ItemId = :itemId) "
         + "AND (:documentTypeId is null or PiH.DocumentTypeId = :documentTypeId) "
         + "AND (:jobLotId IS NULL OR PiD.JobLotId = :jobLotId) "
         + "and (ISNULL(pid.BillAmount,0) - ISNULL(FPD.Amount,0) > 0) "
         + "AND (ISNULL(pid.NetBillWeight,0) - ISNULL(FPD.Weight,0) > 0) "
         + "GROUP BY PiH.Id,PiH.DocDate, PiH.DocNo, PiH.ManualBillNo,PiD.ItemId, "
         + "Item.ItemName,PID.ItemRate,PiD.ItemUOMId,Uom.Equivalent, "
         + "PiD.CropYear,j.JobLotCode,p.PackTypeCode,PiD.JobLotId, "
         + "PiD.PackingTypeId,w.WareHouseName,PiD.WarehouseId, "
         + "PID.UomScheduleIdRate,UomRate.Equivalent,PID.CurrencyId, "
         + "PID.ExchangeRate, PID.FcyAmount ,PiH.SupplierCustomerId, "
         + "sp.CompanyName", nativeQuery = true)
    List<PurchaseInvoiceProductionIssuanceRow> getInvoicesDetailForProductionIssence(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("itemId") Integer itemId, @Param("documentTypeId") Integer documentTypeId, @Param("jobLotId") Integer jobLotId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='DirectGrnLoadForPurchaseInvoiceDirect'
     * Native: fnSplitString + view V_UomScheduleAndUom. gdnIds = comma-separated InvGrn ids (@GdnIds).
     */
    @Query(value = "SELECT DISTINCT "
         + "d.Id as InvGrnDetailId, d.InvGrnId, GR.DocDate, GR.DocNo,GR.SupplierCustomerId, "
         + "SP.CompanyName AS SupplierCustomer,GR.TransporterId,TR.AccountTitle, "
         + "ISNULL(GR.CarriageAmount, 0) As CarriageAmount,GR.DeliveryTerm,GR.GpNo,GR.VehicleNo, "
         + "d.WarehouseId,wr.WareHouseName,d.ItemId,Item.ItemCodeNew ItemCode,Item.ItemName, "
         + "d.CropYear,d.JobLotId,j.JobLotDescription,d.PackingTypeId,pp.PackTypeDesc, "
         + "d.ItemUomId as ItemUOMId, UomItm.UOMCode AS UOMCodeItem,UomItm.Equivalent, "
         + "d.ItemQty,d.GrossWeight,d.EBWPerUnit,d.EBWTotal as EBTotalWt,d.LabReportRef as LabAnalisysNo, "
         + "d.WtCut,d.WtCutTotal as WeightCutTotal,ISNULL(d.AdLsWeight, 0) AdLsWeight, "
         + "ISNULL(d.NetBillWeight, 0) AS NetBillWeight, d.StockWeight as NetStockWeight, "
         + "d.CityId,d.AreaCity, d.CommentsDetail,Item.PurchaseGLAC,d.ScaleKart,bb.BranchName , "
         + "d.PurchaseOrderId,d.PurchaseOrderDetailId,Poh.DocNo OrderNo,Poh.DocDate OrderDate, "
         + "pod.OrderItemRate,pod.OrderItemRateUOMId,Ruom.Equivalent OrderRateEquivalent "
         + "FROM InvGrnDetail d "
         + "INNER JOIN InvGrn GR ON d.InvGrnId = GR.Id "
         + "INNER JOIN SupplierCustomer SP ON GR.SupplierCustomerId = SP.Id "
         + "INNER JOIN dbo.Item ON d.ItemId = dbo.Item.Id "
         + "INNER JOIN dbo.V_UomScheduleAndUom AS UomItm ON d.ItemUomId = UomItm.Id "
         + "LEFT JOIN dbo.ChartofAccount AS TR ON GR.TransporterId = TR.Id "
         + "INNER JOIN InvWareHouse wr on d.WarehouseId = wr.Id "
         + "LEFT join JobLot j on j.Id=d.JobLotId "
         + "LEFT join InvPackingType pp on pp.Id=d.PackingTypeId "
         + "Left Join Branches bb on bb.Id=GR.BranchesId "
         + "LEft Join PurchaseOrderDetail Pod on Pod.PurchaseOrderId=d.PurchaseOrderId and Pod.Id=d.PurchaseOrderDetailId "
         + "Left Join PurchaseOrder Poh on Poh.Id=d.PurchaseOrderId "
         + "LEFT JOIN dbo.V_UomScheduleAndUom AS Ruom ON Pod.OrderItemRateUOMId = Ruom.Id "
         + "WHERE (GR.Id in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseInvoiceDirectGrnLoadRow> directGrnLoadForPurchaseInvoiceDirect(@Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPendingGrnForPurchaseInvoiceTrading'
     * Native: view V_UomScheduleAndUom, CONVERT(date,...,102).
     */
    @Query(value = "SELECT gh.Id, gh.DocDate, gh.DocNo, gh.GpNo, gh.VehicleNo, gh.BiltyNo, "
         + "SupplierCustomer.CompanyName AS SupplierCustomer, gd.PurchaseOrderId, gh.SupplierCustomerId, "
         + "gd.ItemQty, Item.ItemName, pod.OrderItemRate, PoH.DocNo AS PurchaseOrder, Item.ItemCode, U.UOMCode, gd.WarehouseId "
         + "FROM SupplierCustomer INNER JOIN "
         + "InvGrn gh ON SupplierCustomer.Id = gh.SupplierCustomerId INNER JOIN "
         + "InvGrnDetail gd ON gh.Id = gd.InvGrnId INNER JOIN "
         + "Item ON gd.ItemId = Item.Id INNER JOIN "
         + "V_UomScheduleAndUom AS U ON U.Id = gd.ItemUomId LEFT OUTER JOIN "
         + "PurchaseOrderDetail AS pod on gd.PurchaseOrderId = pod.Id "
         + "INNER JOIN PurchaseOrder AS PoH ON pod.PurchaseOrderId = PoH.Id "
         + "where (gh.OrganizationId = :organizationId) "
         + "and (gh.CompanyId = :companyId) "
         + "AND (gh.SupplierCustomerId = :supplierCustomerId) "
         + "and (:fromDate is null or CAST(gh.DocDate AS DATE) >= (CONVERT(date,:fromDate,102))) "
         + "and (:toDate is null or CAST(gh.DocDate AS DATE) <= (CONVERT(date,:toDate,102))) "
         + "and (gh.DocumentTypeId = :documentTypeId) "
         + "and (gh.Id NOT IN(SELECT InvGrnId FROM InvPurchaseInvoiceDetail "
         + "inner join InvPurchaseInvoice on InvPurchaseInvoice.Id = InvPurchaseInvoiceId "
         + "where OrganizationId = :organizationId and CompanyId = :companyId))", nativeQuery = true)
    List<PurchaseInvoicePendingGrnTradingRow> getPendingGrnForPurchaseInvoiceTrading(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetTransporterAndFreightFromGrn'
     * Native: fnSplitString, InvFreightVoucher (no entity).
     */
    @Query(value = "SELECT InvGrn.Id AS MainId, InvGrn.DocNo, InvGrn.DocDate, isnull(InvGrn.GpNo,0) GpNo,InvGrn.TransporterSupCustId, InvGrn.TransporterId as Transporter, "
         + "CASE WHEN ISNULL(FR.TotalPaidAmount,0) > 0 THEN FR.TotalPaidAmount ELSE InvGrn.CarriageAmount END AS CarriageAmount "
         + ", CASe when ISNULL(InvGrn.TransporterSupCustId,0) > 0 "
         + "then sp.CompanyName "
         + "when ISNULL(InvGrn.TransporterId,0) > 0 "
         + "then ChartofAccount.AccountTitle "
         + "else '' end as AccountTitle, "
         + "InvGrn.BiltyNo,InvGrn.VehicleNo "
         + ",case when gp.Freight > 0 and gp.FreightOn = 1 then 1 else 0 end as FreightOnCash,fr.Id as FreightId "
         + "FROM InvGrn "
         + "Left Join Gatepassinward gp on gp.id = invGrn.InwardGatePassId "
         + "LEFT JOIN ChartofAccount ON InvGrn.TransporterId = ChartofAccount.Id "
         + "LEFT JOIN SupplierCustomer sp on sp.Id=InvGrn.TransporterSupCustId "
         + "LEFT JOIN InvFreightVoucher fr on gp.Id = fr.GatePassId and fr.GatePassId=invGrn.InwardGatePassId "
         + "WHERE (InvGrn.Id in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseInvoiceTransporterFreightRow> getTransporterAndFreightFromGrn(@Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetItemQtyAndItemRateFromPO'
     * Native: fnSplitString.
     */
    @Query(value = "SELECT PurchaseOrderDetail.BagPrice, SUM(ISNULL(InvGrnDetail.ItemQty, 0)) AS ItemQty, InvGrnDetail.InvGrnId AS Id "
         + "FROM PurchaseOrder "
         + "INNER JOIN PurchaseOrderDetail ON PurchaseOrder.Id = PurchaseOrderDetail.PurchaseOrderId "
         + "INNER JOIN InvGrnDetail ON PurchaseOrderDetail.PurchaseOrderId = InvGrnDetail.PurchaseOrderId AND InvGrnDetail.ItemId = PurchaseOrderDetail.OrderItemId "
         + "WHERE (InvGrnDetail.InvGrnId in (select data from dbo.fnSplitString (:gdnIds,','))) "
         + "GROUP BY PurchaseOrderDetail.BagPrice, InvGrnDetail.InvGrnId", nativeQuery = true)
    List<PurchaseInvoiceItemQtyRateFromPoRow> getItemQtyAndItemRateFromPO(@Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='ReadPurchaseOrderSupplierExpenseByGdnIds'
     * Native: PurchaseOrderSupplierExpense (no entity), fnSplitString.
     */
    @Query(value = "SELECT R.[Id], "
         + "R.[PurchaseOrderId], "
         + "R.[InvRevExpItemId], "
         + "R.[Qty], "
         + "R.[Rate], "
         + "R.[Amount], "
         + "R.[Remarks], "
         + "OIT.OtherItemName "
         + "FROM [dbo].[PurchaseOrderSupplierExpense] R "
         + "INNER JOIN InventoryItemsOther oit on R.InvRevExpItemId = OIT.Id "
         + "INNER JOIN PurchaseOrder h on h.Id=R.PurchaseOrderId "
         + "INNER JOIN PurchaseOrderDetail d ON h.Id = d.PurchaseOrderId "
         + "INNER JOIN InvGrnDetail ON R.PurchaseOrderId = InvGrnDetail.PurchaseOrderId AND InvGrnDetail.ItemId = d.OrderItemId "
         + "WHERE (InvGrnDetail.InvGrnId in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseOrderSupplierExpenseRow> readPurchaseOrderSupplierExpenseByGdnIds(@Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseOrderSupplierExpenseByOrderIds'
     * Native: PurchaseOrderSupplierExpense (no entity), fnSplitString. Same columns as ReadPurchaseOrderSupplierExpenseByGdnIds.
     */
    @Query(value = "SELECT R.[Id], "
         + "R.[PurchaseOrderId], "
         + "R.[InvRevExpItemId], "
         + "R.[Qty], "
         + "R.[Rate], "
         + "R.[Amount], "
         + "R.[Remarks], "
         + "OIT.OtherItemName "
         + "FROM [dbo].[PurchaseOrderSupplierExpense] R "
         + "INNER JOIN InventoryItemsOther oit on R.InvRevExpItemId = OIT.Id "
         + "INNER JOIN PurchaseOrder h on h.Id=R.PurchaseOrderId "
         + "WHERE (R.PurchaseOrderId in (select data from dbo.fnSplitString (:orderIds,',')))", nativeQuery = true)
    List<PurchaseOrderSupplierExpenseRow> purchaseOrderSupplierExpenseByOrderIds(@Param("orderIds") String orderIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetEmptyBagsFromGrn'
     * Native: PurchaseOrderEmptyBags, view V_ItemCondition, fnSplitString. orderId = @OrderId substituted when EB.PurchaseOrderId = 0.
     */
    @Query(value = "SELECT Distinct CASE WHEN EB.PurchaseOrderId = 0 THEN :orderId ELSE EB.PurchaseOrderId END AS PurchaseOrderId,EB.InvGrnId,EB.TypeId,EB.ItemId,i.ItemName,EB.PurchaseQty "
         + ",CASE WHEN POEB.Rate IS NULL THEN POB.Rate ELSE POEB.Rate END AS Rate "
         + ",EB.Remarks "
         + ",case EB.TypeId "
         + "when 1 then 'Normal' "
         + "When 2 then 'Purchase Against Weight' "
         + "When 3 then 'Free of Cost' "
         + "When 4 then 'Retained' "
         + "End EmptyBagsType ,EB.BagsCondition as ItemConditionId,VC.ConditionStatus as ItemCondition "
         + "FROM InvGrnDetailEmptyBags EB "
         + "INNER JOIN Item i on EB.ItemId = i.Id "
         + "LEFT JOIN PurchaseOrderEmptyBags POEB ON EB.PurchaseOrderId = POEB.PurchaseOrderId and EB.ItemId = POEB.ItemId and POEB.PackingTypeId=1 "
         + "LEFT JOIN PurchaseOrderEmptyBags POB ON EB.ItemId = POB.ItemId AND POB.PurchaseOrderId = EB.PurchaseOrderId and POB.PackingTypeId=2 "
         + "LEft Join V_ItemCondition VC on VC.Id = EB.BagsCondition "
         + "WHERE EB.PurchaseQty > 0 "
         + "and (EB.InvGrnId in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseInvoiceEmptyBagsFromGrnRow> getEmptyBagsFromGrn(@Param("orderId") Integer orderId, @Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='FormHistory'
     * Native: #TmpInvoiceHistory + 3 UPDATEs rewritten as one CTE: VoucherHeadId = MAX(VoucherHead.Id) match (proc: arbitrary match, 0 if none),
     * NoOfAttachments = COUNT(DMSAttachments), VehicleNos = STRING_AGG of distinct detail VehicleNo; #TmpBranchIds → fnSplitString EXISTS.
     * Same output columns and ORDER BY RecordNo DESC as the proc.
     */
    @Query(value = "WITH TmpInvoiceHistory AS ( "
         + "SELECT ROW_NUMBER () Over (Order By h.DocDate,H.DocNo) as RecordNo, "
         + "h.Id, "
         + "ISNULL((SELECT MAX(VH.Id) FROM VoucherHead VH WHERE VH.DocumentTypeId = h.DocumentTypeId AND VH.DocumentTypeSrNo = h.Id), 0) AS VoucherHeadId, "
         + "h.DocumentTypeId, "
         + "h.DocNo, "
         + "h.DocDate, "
         + "CASE "
         + "WHEN H.InvoiceTypeId = 1 THEN 'PurchaseOrder' "
         + "WHEN H.InvoiceTypeId = 2 THEN 'Market Purchase' "
         + "WHEN H.InvoiceTypeId = 3 THEN 'Gate Purchase' "
         + "WHEN H.InvoiceTypeId = 4 THEN 'Purchase From Party Processing' "
         + "ELSE 'Other' END AS PurchaseAgainst, "
         + "h.DueDays, "
         + "h.DueDate, "
         + "h.ManualBillNo, "
         + "H.SupplierCustomerId, "
         + "h.ReferencePartyName, "
         + "H.CommissionAgentId, "
         + "h.CommissionType, "
         + "h.CommRate, "
         + "h.CommAmount, "
         + "h.CommissionRemarks, "
         + "h.RemarksHeader, "
         + "h.BillAmount, "
         + "h.CurrencyId, "
         + "h.ExchangeRate, "
         + "h.FcyAmount, "
         + "h.EntryDate, "
         + "H.EntryUser AS EntryUserId, "
         + "h.ModifyDate, "
         + "h.ModifyUser AS ModifyUserId, "
         + "h.PostDate, "
         + "h.PostUser, h.ScreenName, "
         + "CASE WHEN h.BillCalculateTypeId = 2 THEN 'On Qty' ELSE 'On Weight' END AS BillType, "
         + "CASE WHEN ISNULL(H.IsApproved,0) = 0 THEN 'Not Approved' ELSE 'APPROVED' END ApprovedStatus, "
         + "ISNULL((SELECT COUNT(d.Id) FROM DMSAttachments d WHERE d.RefDocumentTypeId = h.DocumentTypeId AND d.RefDocumentNo = h.Id), 0) AS NoOfAttachments, "
         + "h.BranchesId ,h.BranchSrNo,h.CommissionCreditAccountId, "
         + "ISNULL((SELECT STRING_AGG(x.VehicleNo, ',') "
         + "FROM (SELECT DISTINCT InvPurchaseInvoiceId, VehicleNo FROM InvPurchaseInvoiceDetail WHERE ActionTypeID <> 3) AS x "
         + "WHERE x.InvPurchaseInvoiceId = h.Id), '') as VehicleNos "
         + "FROM InvPurchaseInvoice h "
         + "WHERE h.[OrganizationId] = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.DocumentTypeId = :documentTypeId "
         + "and (:financialYearId is null or h.FinancialYearId = :financialYearId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and h.EntryUser = :entryUser)) "
         + "AND (:supplierCustomerId IS NULL or h.SupplierCustomerId = :supplierCustomerId) "
         + "AND (:fromDate IS NULL OR h.DocDate >= :fromDate) "
         + "AND (:toDate IS NULL OR h.DocDate < DATEADD(DAY,1,:toDate)) "
         + "AND (:entryFromDate IS NULL OR h.EntryDate >= :entryFromDate) "
         + "AND (:entryToDate IS NULL OR h.EntryDate < DATEADD(DAY,1,:entryToDate)) "
         + "AND (:modifyFromDate IS NULL OR h.ModifyDate >= :modifyFromDate) "
         + "AND (:modifyToDate IS NULL OR h.ModifyDate < DATEADD(DAY,1,:modifyToDate)) "
         + "AND (:approvedFromDate IS NULL OR h.PostDate >= :approvedFromDate) "
         + "AND (:approvedToDate IS NULL OR h.PostDate < DATEADD(DAY,1,:approvedToDate)) "
         + "AND (:fromDocNo IS NULL OR h.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR h.DocNo <= :toDocNo) "
         + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:branchesIds, ',') WHERE data = h.BranchesId)) "
         + ") "
         + "SELECT H.RecordNo, "
         + "h.Id, "
         + "h.VoucherHeadId, "
         + "h.DocumentTypeId, "
         + "h.DocNo, "
         + "POH.DocNo AS OrderNo, "
         + "h.DocDate, "
         + "H.PurchaseAgainst, "
         + "h.DueDays, "
         + "h.DueDate, "
         + "h.ManualBillNo,H.SupplierCustomerId, "
         + "SC1.CompanyName AS SupplierName, "
         + "h.ReferencePartyName, "
         + "CS4.CompanyName AS CommissionAgent, "
         + "h.CommissionType, "
         + "h.CommRate, "
         + "h.CommAmount, "
         + "h.CommissionRemarks, "
         + "h.RemarksHeader, "
         + "h.BillAmount, "
         + "h.CurrencyId, "
         + "mc.CurrencyCode, "
         + "mc.CurrencyName, "
         + "h.ExchangeRate, "
         + "h.FcyAmount, "
         + "h.EntryDate, "
         + "UserAccount.UserTitle AS EntryUser, "
         + "h.ModifyDate, "
         + "Musr.UserTitle AS ModifyUser, "
         + "h.PostDate, "
         + "Ausr.UserTitle AS ApprovedUser, "
         + "H.BillType, h.ScreenName, "
         + "H.ApprovedStatus, "
         + "H.NoOfAttachments ,bb.BranchName,h.BranchSrNo ,h.CommissionCreditAccountId, "
         + "H.VehicleNos "
         + "FROM TmpInvoiceHistory h "
         + "INNER JOIN SupplierCustomer AS SC1 ON h.SupplierCustomerId = SC1.Id "
         + "INNER JOIN UserAccount ON h.EntryUserId = UserAccount.ID "
         + "INNER JOIN (SELECT InvPurchaseInvoiceId,PurchaseOrderId "
         + "FROM InvPurchaseInvoiceDetail "
         + "GROUP BY InvPurchaseInvoiceId,PurchaseOrderId "
         + ") d on h.Id = d.InvPurchaseInvoiceId "
         + "LEFT JOIN PurchaseOrder POH ON D.PurchaseOrderId = POH.Id "
         + "LEFT JOIN UserAccount Musr ON h.ModifyUserId = Musr.ID "
         + "LEFT JOIN UserAccount Ausr ON h.PostUser = Ausr.ID "
         + "LEFT JOIN MultiCurrency mc ON mc.Id = h.CurrencyId "
         + "LEFT JOIN SupplierCustomer AS CS4 ON h.CommissionAgentId = CS4.Id "
         + "LEFT JOIn Branches bb on bb.Id=h.BranchesId "
         + "WHERE (:noOfRecords IS NULL OR H.RecordNo <= :noOfRecords) "
         + "ORDER BY H.RecordNo DESC", nativeQuery = true)
    List<PurchaseInvoiceFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId, @Param("canViewAllRecord") Boolean canViewAllRecord, @Param("entryUser") Integer entryUser, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("entryFromDate") LocalDate entryFromDate, @Param("entryToDate") LocalDate entryToDate, @Param("modifyFromDate") LocalDate modifyFromDate, @Param("modifyToDate") LocalDate modifyToDate, @Param("approvedFromDate") LocalDate approvedFromDate, @Param("approvedToDate") LocalDate approvedToDate, @Param("fromDocNo") Integer fromDocNo, @Param("toDocNo") Integer toDocNo, @Param("branchesIds") String branchesIds, @Param("noOfRecords") Integer noOfRecords);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='FormHistoryTradePro'
     * Native: ROW_NUMBER, derived table, view V_UomScheduleAndUom.
     */
    @Query(value = "SELECT distinct * FROM ( "
         + "SELECT ROW_NUMBER () Over (Order By p.DocDate Desc) RecordNo, p.Id,h.Id as VoucherHeadId, "
         + "0 as GrnId,p.DocNo, p.DocDate,p.ManualBillNo,SC1.CompanyName AS SupplierName, "
         + "CS4.CompanyName AS CommissionAgent,0 GrnNo,p.CommissionType, p.CommRate,p.CommAmount,p.CommissionRemarks, "
         + "p.RemarksHeader,SC2.CompanyName AS StockPartyName,Sc3.CompanyName AS RefPartyName,p.BillAmount, "
         + "UserAccount.FirstName + ' ' + UserAccount.LastName as UserName,i.ItemName,ItemUOM.Equivalent,d.ItemRate, "
         + "d.ItemAmount,d.ItemQty,d.RemarksDetail, p.ScreenName, "
         + "(SELECT COUNT(Attachment) FROM dbo.DMSAttachments WHERE RefDocumentTypeId = p.DocumentTypeId AND RefDocumentNo = p.Id) AS NoOfAttachments "
         + "FROM InvPurchaseInvoice p "
         + "INNER JOIN InvPurchaseInvoiceDetail d on p.Id = d.InvPurchaseInvoiceId "
         + "INNER JOIN Item i on d.ItemId = i.Id "
         + "INNER JOIN SupplierCustomer AS SC1 ON p.SupplierCustomerId = SC1.Id "
         + "LEFT JOIN V_UomScheduleAndUom ItemUOM on d.ItemUOMId = ItemUOM.Id "
         + "LEFT JOIN UserAccount ON p.EntryUser = UserAccount.ID "
         + "LEFT JOIN SupplierCustomer AS CS4 ON p.CommissionAgentId = CS4.Id "
         + "LEFT JOIN SupplierCustomer AS Sc3 ON p.ReferencePartyId = Sc3.Id "
         + "LEFT JOIN SupplierCustomer AS SC2 ON p.StockPartyId = SC2.Id "
         + "LEFT join PurchaseOrderDetail Pod on d.PurchaseOrderId = pod.PurchaseOrderId and Pod.OrderItemId = d.ItemId "
         + "INNER JOIN VoucherHead h on p.Id = h.DocumentTypeSrNo and p.Documenttypeid = h.documenttypeid "
         + "WHERE p.[OrganizationId] = :organizationId "
         + "and p.CompanyId = :companyId "
         + "and p.DocumentTypeId = :documentTypeId "
         + "and (:financialYearId is null or p.FinancialYearId = :financialYearId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and p.EntryUser = :entryUser)) "
         + "AND (:supplierCustomerId IS NULL or p.SupplierCustomerId = :supplierCustomerId) "
         + "AND (:fromDate IS NULL OR Cast(p.DocDate as Date) >= :fromDate) "
         + "AND (:toDate IS NULL OR CAST(p.DocDate as Date) <= :toDate) "
         + "AND (:fromDocNo IS NULL OR p.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR p.DocNo <= :toDocNo) "
         + ")a Where (:noOfRecords is null or a.RecordNo <= :noOfRecords) "
         + "order by a.DocNo desc", nativeQuery = true)
    List<PurchaseInvoiceFormHistoryTradeProRow> formHistoryTradePro(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId, @Param("canViewAllRecord") Boolean canViewAllRecord, @Param("entryUser") Integer entryUser, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("fromDocNo") Integer fromDocNo, @Param("toDocNo") Integer toDocNo, @Param("noOfRecords") Integer noOfRecords);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseDetailReadByInvPurchaseInvoiceId'
     * KEEP-EXEC: #DetailDataRecords temp table + 3 UPDATEs (lab OUTER APPLY TOP 1, ExIm ref 244, sale-invoice ref 99/95) before the final 20-join select;
     * updates join on non-unique keys so they are not expressible as deterministic computed columns. Projection = the proc's final SELECT.
     */
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id = :id, @Activity = 'PurchaseDetailReadByInvPurchaseInvoiceId'", nativeQuery = true)
    List<PurchaseInvoiceDetailReadRow> purchaseDetailReadByInvPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseTradingDetailReadByInvPurchaseInvoiceId'
     * Native verbatim (views V_UomScheduleAndUom / V_ItemCondition / mfg.*). The proc selects pid.Brokery twice; the duplicate column was dropped (same value).
     */
    @Query(value = "SELECT distinct Pid.Id, Pid.InvPurchaseInvoiceId, Pid.InvGrnId, Pid.InvGrnDetailId, "
         + "Pid.PurchaseOrderId,pid.PurchaseOrderDetailId, "
         + "Pid.ItemId, Pid.PackingTypeId, Pid.CropYear, Pid.JobLotId, "
         + "Pid.ItemQty, Pid.ItemUOMId, Pid.GrossWeight, Pid.EBWeight, "
         + "Pid.EBTotalWt,Pid.AdLsWeight, Pid.NetBillWeight, Pid.NetStockWeight, "
         + "Pid.ItemRate, Pid.UomScheduleIdRate, Pid.ItemAmount, Pid.WarehouseId, "
         + "Pid.LabAnalisysNo, Pid.GpDate, Pid.IsTaxable,Txt.TaxName TaxDescriptions, Pid.TaxAmount, "
         + "Pid.ItemCgsRate, Pid.AvgCgsRate, Pid.RemarksDetail, Pid.RateCut, "
         + "Pid.RateCutAmount, Pid.BillAmount, Pid.FreightAmount, Pid.ExpenseAmount, "
         + "Pid.JournalAmount, Pid.CommissionAmount, Pid.WeightCut, Pid.WeightCutTotal, "
         + "Pid.GpNo, Pid.VehicleNo, Pid.TaxNameId,Pid.Brokery, i.ItemName, "
         + "job.JobLotDescription, Pid.TaxPercent, i.PurchaseGLAC, i.ItemCode, "
         + "Uom.UOMCode AS UOMCodeItem, UOMRate.Equivalent AS EquivalentPoRate, "
         + "POH.DocNo AS PurchaseOrder, Wh.WareHouseName,Isnull(PT.PackTypeDesc,'') as PackTypeDesc , "
         + "grh.DocDate as GrnDate,grh.DocNo as GrnNo, "
         + "pid.ScaleKart,uomRate.uomCode RateUom,pid.LineId, "
         + "UOM.Equivalent AS ItemUOMEquivalent, pid.CurrencyId , "
         + "pid.ExchangeRate, pid.FcyAmount,pid.WagesAmount, Pid.AttributeVariantId,vt.ComputedName AttributeVariant, "
         + "pid.EbPurAgainstWeight,pid.BranchId,bb.BranchName,pid.PackingDate,pid.ExpiryDate,pid.CropYearId, "
         + "Pid.RefDocumentTypeId, "
         + "CAST('' as nvarchar(max)) RefDocumentType, "
         + "Pid.RefDocId,pid.RefDocSubId, "
         + "CAST('' as nvarchar(max)) RefDocNo, "
         + "Pid.RefDocInvoiceId, "
         + "CAST('' as nvarchar(max)) RefDocInvoiceNo, "
         + "CAST('' as float) RefInvoiceQty, "
         + "CAST('' as float) RefInvoiceWeight "
         + ",pid.DiscountTypeId "
         + ",pid.DiscountPercent "
         + ",pid.DiscountAmount "
         + ",pid.CityId "
         + ",pid.CastingTypeId "
         + ",pid.ProductionStageId, "
         + "i.WeightFinishGoods, "
         + "Poh.DocDate OrderDate, "
         + "vt.VarientDescription VariantDescription, "
         + "ct.Description CityName, "
         + "Vc.CastingType, "
         + "PPS.ProductionProcessStage, "
         + "Case When pid.DiscountTypeId=1 then 'Flat' "
         + "when pid.DiscountTypeId=2 then 'Percent' "
         + "else '' end as DiscountType, "
         + "pid.GdnDocumentTypeId, "
         + "pid.GdnId, "
         + "pid.GdnDetailId,Gdh.DocNo GdnNo,Gdh.DocDate GdnDate, "
         + "pid.LocationTypeId, "
         + "pid.EbPurAgainstWeightAmount, "
         + "pid.ActionTypeId, "
         + "pid.ExcessWeight, "
         + "pid.RateCutForExcessWeight, "
         + "pid.RateCutAmountForExcessWeight, "
         + "pid.FreightDeduction, "
         + "pid.ItemConditionId,VIC.ConditionStatus as ItemCondition, "
         + "Pid.RackId,R.rackName,pid.ItemAmountWithoutDiscount, "
         + "Cast(0 as int ) as PurchaseOrderDocumentTypeId, "
         + "Cast(0 as float ) as PurchaseOrderQty, "
         + "Cast(0 as int ) as PurchaseDemandDocumentTypeId, "
         + "Cast(0 as int ) as PurchaseDemandId, "
         + "Cast(0 as int ) as PurchaseDemandDetailId, "
         + "Cast(0 as int ) as PurchaseDemandNo, "
         + "Cast(0 as float ) as PurchaseDemandQty, "
         + "Cast(0 as int ) as PreBillDocumentTypeId, "
         + "Cast(0 as int ) as PreBillId, "
         + "Cast(0 as int ) as PreBillDetailId, "
         + "Cast(0 as int ) as PreBillNo, "
         + "Cast(0 as float ) as PreBillQty, "
         + "Cast(0 as int ) as DeliveryChallanDocumentTypeId, "
         + "Cast(0 as int ) as DeliveryChallanId, "
         + "Cast(0 as int ) as DeliveryChallanDetailId, "
         + "Cast(0 as int ) as DeliveryChallanNo, "
         + "Cast(0 as float ) as DeliveryChallanQty, "
         + "Cast(0 as int ) as GrnDocumentTypeId, "
         + "Cast(0 as float ) as GrnQty, "
         + "Cast(0 as int ) as NoOfAttachmentsPO, "
         + "Cast(0 as int ) as NoOfAttachmentsPD, "
         + "Cast(0 as int ) as NoOfAttachmentsPreBill, "
         + "Cast(0 as int ) as NoOfAttachmentsDC, "
         + "Cast(0 as int ) as NoOfAttachmentsGrn "
         + "FROM InvPurchaseInvoiceDetail AS Pid "
         + "INNER JOIN Item AS i ON Pid.ItemId = i.Id "
         + "INNER JOIN V_UomScheduleAndUom AS UOMRate ON Pid.UomScheduleIdRate = UOMRate.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS Uom ON Pid.ItemUOMId = Uom.Id "
         + "LEFT JOIN TaxesTypes Txt on Pid.TaxNameId = Txt.Id "
         + "LEFT JOIN JobLot AS job ON Pid.JobLotId = job.Id "
         + "LEFT JOIN PurchaseOrder AS POH ON Pid.PurchaseOrderId = POH.Id "
         + "LEFT JOIN InvWareHouse AS Wh ON Pid.WarehouseId = Wh.Id "
         + "LEFT JOIN InvPackingType PT ON Pid.PackingTypeId = PT.Id "
         + "LEFT JOIN InvGrn grh on pid.InvGrnId = grh.Id "
         + "Left join ItemAttributeVarient vt on vt.Id=Pid.AttributeVariantId "
         + "Left Join Branches bb on bb.Id=pid.BranchId "
         + "Left Join mfg.VCastingType Vc on Vc.CastingTypeid=pid.CastingTypeId "
         + "Left Join Mfg.vProductionProcessStage PPS on PPS.ProductionProcessStageId=pid.ProductionStageId "
         + "Left Join InvGdn Gdh on Gdh.Id=Pid.GdnId and Gdh.DocumentTypeId = pid.GdnDocumentTypeId "
         + "Left Join City ct on ct.Id=pid.CityId "
         + "LEFT JOIN V_ItemCondition VIC on VIC.ID = pid.ItemConditionId "
         + "LEFT JOIN invWarehouseRack R ON Pid.RackId = R.Id "
         + "WHERE (Pid.InvPurchaseInvoiceId = :id) "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3", nativeQuery = true)
    List<PurchaseInvoiceTradingDetailRow> purchaseTradingDetailReadByInvPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='DirectPurchaseDetailReadByInvPurchaseInvoiceId'
     * KEEP-EXEC: #DirectDetailDataRecords temp table + 2 ref UPDATEs (ExIm 244, sale-invoice 99/95) before the final select.
     */
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id = :id, @Activity = 'DirectPurchaseDetailReadByInvPurchaseInvoiceId'", nativeQuery = true)
    List<PurchaseInvoiceDirectDetailRow> directPurchaseDetailReadByInvPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='TradingPurchaseDetailReadByInvPurchaseInvoiceId'
     * Native verbatim (views). The proc selects pid.Brokery twice; the duplicate column was dropped (same value).
     */
    @Query(value = "SELECT distinct Pid.Id, Pid.InvPurchaseInvoiceId, Pid.InvGrnId, Pid.InvGrnDetailId, Pid.PurchaseOrderId,pid.PurchaseOrderDetailId, Pid.ItemId, "
         + "Pid.PackingTypeId, Pid.CropYear, Pid.JobLotId, Pid.ItemQty, Pid.ItemUOMId, Pid.GrossWeight, Pid.EBWeight, Pid.EBTotalWt, "
         + "Pid.AdLsWeight, Pid.NetBillWeight, Pid.NetStockWeight, Pid.ItemRate, Pid.UomScheduleIdRate, Pid.ItemAmount, Pid.WarehouseId, Pid.LabAnalisysNo, Pid.GpDate, Pid.IsTaxable, Pid.TaxDescriptions, Pid.TaxAmount, "
         + "Pid.ItemCgsRate, Pid.AvgCgsRate, Pid.RemarksDetail, Pid.RateCut, Pid.RateCutAmount, Pid.BillAmount, Pid.FreightAmount, Pid.ExpenseAmount, Pid.JournalAmount, Pid.CommissionAmount, Pid.WeightCut, Pid.WeightCutTotal, "
         + "Pid.GpNo, Pid.VehicleNo, Pid.TaxNameId,Pid.Brokery, i.ItemName, job.JobLotDescription, Pid.TaxPercent, i.PurchaseGLAC, i.ItemCode, Uom.UOMDescription AS UOMCodeItem, UOMRate.Equivalent AS EquivalentPoRate, "
         + "POH.DocNo AS PurchaseOrder, Wh.WareHouseName,Isnull(PT.PackTypeDesc,'') as PackTypeDesc ,uomRate.uomCode RateUom "
         + ",grn.DocDate as GrnDate,grn.DocNo as GrnNo ,pid.ScaleKart,pid.LineId "
         + ", UOM.Equivalent AS ItemUOMEquivalent,pid.CurrencyId , pid.ExchangeRate, pid.FcyAmount ,pid.WagesAmount , "
         + "Pid.AttributeVariantId,vt.ComputedName AttributeVariant, "
         + "pid.EbPurAgainstWeight,pid.BranchId,bb.BranchName,pid.PackingDate,pid.ExpiryDate,pid.CropYearId, "
         + "Pid.RefDocumentTypeId, "
         + "CAST('' as nvarchar(max)) RefDocumentType, "
         + "Pid.RefDocId,pid.RefDocSubId, "
         + "CAST('' as nvarchar(max)) RefDocNo, "
         + "Pid.RefDocInvoiceId, "
         + "CAST('' as nvarchar(max)) RefDocInvoiceNo, "
         + "CAST('' as float) RefInvoiceQty, "
         + "CAST('' as float) RefInvoiceWeight "
         + ",pid.DiscountTypeId "
         + ",pid.DiscountPercent "
         + ",pid.DiscountAmount "
         + ",pid.CityId "
         + ",pid.CastingTypeId "
         + ",pid.ProductionStageId "
         + ",Pid.ExcessWeight "
         + ",pid.RateCutForExcessWeight "
         + ",pid.RateCutAmountForExcessWeight "
         + ",i.WeightFinishGoods, "
         + "Poh.DocDate OrderDate, "
         + "vt.VarientDescription VariantDescription, "
         + "ct.Description CityName, "
         + "Vc.CastingType, "
         + "PPS.ProductionProcessStage, "
         + "Case When pid.DiscountTypeId=1 then 'Flat' "
         + "when pid.DiscountTypeId=2 then 'Percent' "
         + "else '' end as DiscountType, "
         + "pid.GdnDocumentTypeId, "
         + "pid.GdnId , "
         + "pid.GdnDetailId,Gdh.DocNo GdnNo,Gdh.DocDate GdnDate, "
         + "pid.LocationTypeId ,pid.EbPurAgainstWeightAmount,pid.ActionTypeId,pid.FreightDeduction, "
         + "pid.ItemConditionId,VIC.ConditionStatus as ItemCondition, "
         + "Pid.RackId,R.rackName,pid.ItemAmountWithoutDiscount, "
         + "Cast(0 as int ) as PurchaseOrderDocumentTypeId, "
         + "Cast(0 as float ) as PurchaseOrderQty, "
         + "Cast(0 as int ) as PurchaseDemandDocumentTypeId, "
         + "Cast(0 as int ) as PurchaseDemandId, "
         + "Cast(0 as int ) as PurchaseDemandDetailId, "
         + "Cast(0 as int ) as PurchaseDemandNo, "
         + "Cast(0 as float ) as PurchaseDemandQty, "
         + "Cast(0 as int ) as PreBillDocumentTypeId, "
         + "Cast(0 as int ) as PreBillId, "
         + "Cast(0 as int ) as PreBillDetailId, "
         + "Cast(0 as int ) as PreBillNo, "
         + "Cast(0 as float ) as PreBillQty, "
         + "Cast(0 as int ) as DeliveryChallanDocumentTypeId, "
         + "Cast(0 as int ) as DeliveryChallanId, "
         + "Cast(0 as int ) as DeliveryChallanDetailId, "
         + "Cast(0 as int ) as DeliveryChallanNo, "
         + "Cast(0 as float ) as DeliveryChallanQty, "
         + "Cast(0 as int ) as GrnDocumentTypeId, "
         + "Cast(0 as float ) as GrnQty, "
         + "Cast(0 as int ) as NoOfAttachmentsPO, "
         + "Cast(0 as int ) as NoOfAttachmentsPD, "
         + "Cast(0 as int ) as NoOfAttachmentsPreBill, "
         + "Cast(0 as int ) as NoOfAttachmentsDC, "
         + "Cast(0 as int ) as NoOfAttachmentsGrn "
         + "FROM InvPurchaseInvoiceDetail AS Pid "
         + "INNER JOIN Item AS i ON Pid.ItemId = i.Id "
         + "Inner JOIN JobLot AS job ON Pid.JobLotId = job.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS Uom ON Pid.ItemUOMId = Uom.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS UOMRate ON Pid.UomScheduleIdRate = UOMRate.Id "
         + "LEFT JOIN PurchaseOrder AS POH ON Pid.PurchaseOrderId = POH.Id "
         + "LEFT JOIN InvWareHouse AS Wh ON Pid.WarehouseId = Wh.Id "
         + "LEFT JOIN InvPackingType PT ON Pid.PackingTypeId = PT.Id "
         + "left join InvGrnDetail gd on pid.InvGrnDetailId = gd.Id and pid.InvGrnId = gd.InvGrnId "
         + "LEFT join InvGrn grn on gd.InvGrnId = grn.Id "
         + "Left join ItemAttributeVarient vt on vt.Id=Pid.AttributeVariantId "
         + "Left Join Branches bb on bb.Id=pid.BranchId "
         + "Left Join mfg.VCastingType Vc on Vc.CastingTypeid=pid.CastingTypeId "
         + "Left Join Mfg.vProductionProcessStage PPS on PPS.ProductionProcessStageId=pid.ProductionStageId "
         + "Left Join InvGdn Gdh on Gdh.Id=Pid.GdnId and Gdh.DocumentTypeId = pid.GdnDocumentTypeId "
         + "Left Join City ct on ct.Id=pid.CityId "
         + "LEFT JOIN V_ItemCondition VIC on VIC.ID = pid.ItemConditionId "
         + "LEFT JOIN invWarehouseRack R ON Pid.RackId = R.Id "
         + "WHERE (Pid.InvPurchaseInvoiceId = :id) "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3", nativeQuery = true)
    List<PurchaseInvoiceTradingPurchaseDetailRow> tradingPurchaseDetailReadByInvPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseDetailReadByInvPurchaseInvoiceIdDirectStoreManagement'
     * KEEP-EXEC: #StoreData temp table + 2 ref UPDATEs (ExIm 244, sale-invoice 99/95) before the final select.
     * GrnDate is '' (string) and OrderDate is CAST('' as Date) in this branch; pid.Brokery is selected twice by the proc (projected once).
     */
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id = :id, @Activity = 'PurchaseDetailReadByInvPurchaseInvoiceIdDirectStoreManagement'", nativeQuery = true)
    List<PurchaseInvoiceDirectStoreDetailRow> purchaseDetailReadByInvPurchaseInvoiceIdDirectStoreManagement(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID'
     * Native: InvGdn has no entity.
     */
    @Query(value = "SELECT Ex.Id, "
         + "Ex.GdnId, "
         + "Ex.GdnExpId,gd.DocNo GdnNo, "
         + "Ex.PurchaseOrderId, "
         + "Ex.PurchaseOrderSupplierExpId, "
         + "Ex.InvPurchaseInvoiceId, "
         + "Ex.InvRevExpItemId, "
         + "I.OtherItemName, "
         + "Ex.Qty, "
         + "Ex.Rate, "
         + "Ex.Amount, "
         + "Ex.Remarks, "
         + "I.StockGLAcId, sCoa.AccountTitle AS AccountTitleStock, "
         + "I.SaleGLAcId, rCoa.AccountTitle AS AccountTitleSales, "
         + "I.CogsGLAcId, cCoa.AccountTitle AS AccountTitleCgs , "
         + "ex.CustomRemarks , "
         + "Ex.GrnId,gr.DocNo GrnNo, "
         + "Ex.SupplierDispatchId, "
         + "Ex.SupplierDispatchExpenseId "
         + "FROM dbo.InvPurchaseInvoiceExpense Ex "
         + "INNER JOIN dbo.InventoryItemsOther I ON Ex.InvRevExpItemId = I.Id "
         + "LEFT JOIN dbo.ChartofAccount AS cCoa ON I.CogsGLAcId = cCoa.Id "
         + "LEFT JOIN dbo.ChartofAccount AS rCoa ON I.SaleGLAcId = rCoa.Id "
         + "LEFT JOIN dbo.ChartofAccount AS sCoa ON I.StockGLAcId = sCoa.Id "
         + "Left join InvGrn gr on gr.Id = Ex.GrnId "
         + "Left join InvGdn gd on gd.Id = Ex.GdnId "
         + "where Ex.InvPurchaseInvoiceId = :id", nativeQuery = true)
    List<PurchaseInvoiceExpenseRow> invPurchaseInvoiceExpenseReadByPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID'
     * JPQL over InvPurchaseInvoiceFreight (entity owned by G2_purchase_invoice_write), ChartofAccount, SupplierCustomer.
     */
    @Query(value = "SELECT f.id AS id, f.supplierCustomerId AS supplierCustomerId, sp.companyName AS supplierName, f.invPurchaseInvoiceId AS invPurchaseInvoiceId, "
         + "f.invGrnId AS invGrnId, f.frQty AS frQty, f.frWeight AS frWeight, f.frRate AS frRate, f.freightAmount AS freightAmount, f.debit AS debit, "
         + "f.percentage AS percentage, f.freightId AS freightId, f.purchaseOrderId AS purchaseOrderId, f.remarks AS remarks, "
         + "f.tansporterId AS tansporterId, c.accountTitle AS accountTitle "
         + "FROM InvPurchaseInvoiceFreight f "
         + "LEFT JOIN ChartofAccount c ON f.tansporterId = c.id "
         + "LEFT JOIN SupplierCustomer sp ON sp.id = f.supplierCustomerId "
         + "WHERE f.invPurchaseInvoiceId = :id")
    List<PurchaseInvoiceFreightRow> invPurchaseInvoiceFreightReadByPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID'
     * JPQL over InvPurchaseInvoiceJournal (entity owned by G2_purchase_invoice_write), ChartofAccount, SupplierCustomer.
     */
    @Query(value = "SELECT j.id AS id, j.supplierCustomerId AS supplierCustomerId, sp.companyName AS supplierName, j.invPurchaseInvoiceId AS invPurchaseInvoiceId, "
         + "j.chartofAccountId AS chartofAccountId, c.accountTitle AS accountTitle, j.jvRemarks AS jvRemarks, j.jvDebit AS jvDebit, j.jvCredit AS jvCredit, "
         + "j.freightId AS freightId, j.jvPrcnt AS jvPrcnt, j.jvQty AS jvQty, j.jvRate AS jvRate, j.invGrnId AS invGrnId, "
         + "j.transporterSupCustId AS transporterSupCustId, j.rowType AS rowType "
         + "FROM InvPurchaseInvoiceJournal j "
         + "LEFT JOIN ChartofAccount c ON j.chartofAccountId = c.id "
         + "LEFT JOIN SupplierCustomer sp ON sp.id = j.supplierCustomerId "
         + "WHERE j.invPurchaseInvoiceId = :id")
    List<PurchaseInvoiceJournalRow> invPurchaseInvoiceJournalReadByPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID'
     * Native: view V_ItemCondition.
     */
    @Query(value = "SELECT Eb.Id,Eb.TypeId,Eb.PurchaseOrderId,Eb.InvGrnId,Eb.InvPurchaseInvoiceId,Eb.ItemId,i.ItemName,Eb.PurchaseQty,Eb.Rate,Eb.Amount,Eb.Remarks,Eb.ReceivedQty "
         + ",case EB.TypeId when 1 then 'Normal' When 2 then 'Purchase Against Weight' When 3 then 'Free of Cost' End EmptyBagsType "
         + ",eb.CreditAccountId,c.AccountTitle,eb.WarehouseId,eb.RateUomId,eb.CustomRemarks,EB.ItemConditionId,VC.ConditionStatus as ItemCondition "
         + ",EB.RackId,R.rackName "
         + "FROM InvPurchaseInvoiceEmptyBags Eb "
         + "INNER JOIN Item i ON Eb.ItemId = i.Id "
         + "LEFT JOIN ChartofAccount C ON eb.CreditAccountId = c.Id "
         + "LEFT Join V_ItemCondition VC on Vc.id=Eb.ItemConditionId "
         + "LEFT JOIN invWarehouseRack R ON Eb.RackId = R.Id "
         + "WHERE Eb.InvPurchaseInvoiceId = :id", nativeQuery = true)
    List<PurchaseInvoiceEmptyBagsRow> invPurchaseInvoiceEmptyBagsReadByPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoicePaymentTerm_ReadByPurchaseInvoiceID'
     * Native: InvDueTerms has no entity.
     */
    @Query(value = "SELECT d.Id, "
         + "d.InvPurchaseInvoiceId, "
         + "d.PaymentTermId, "
         + "pt.TermsDescription, "
         + "d.PrcntOfTotal, "
         + "d.Amount, "
         + "d.PaymentRemarks, "
         + "d.DueDays, "
         + "D.DueDate, "
         + "d.SortNo, "
         + "d.PurchaseOrderId, "
         + "Po.DocNo As PurchaseOrderNo, "
         + "d.SystemGeneratedRow "
         + "FROM InvPurchaseInvoicePaymentTermsDetail d "
         + "INNER JOIN InvDueTerms pt ON d.PaymentTermId = pt.Id "
         + "LEFT JOIN PurchaseOrder Po ON Po.Id = d.PurchaseOrderId "
         + "where d.InvPurchaseInvoiceId = :id", nativeQuery = true)
    List<PurchaseInvoicePaymentTermRow> invPurchaseInvoicePaymentTermReadByPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPendingGrnForPurchaseInvoice'
     * Native: CTEs, STRING_AGG, config UDF (@GrnApprovalNotCompulsory inlined as dbo.GetConfigurationByConfigId(...,326)); #BranchesIds → fnSplitString EXISTS.
     */
    @Query(value = "WITH PendingApproval AS "
         + "( "
         + "SELECT SAL.RefDocId AS GrnId, "
         + "STRING_AGG(SR.SpecialApprovalReason, ', ') AS ApprovalReasons "
         + "FROM dbo.SpecialApprovalLog SAL "
         + "INNER JOIN dbo.SpecialApprovalReason SR ON SR.Id = SAL.ReasonId "
         + "WHERE SAL.RefDocumentTypeId IN (46,217) "
         + "AND SAL.IsApproved = 0 "
         + "AND SAL.ActionId <> 3 "
         + "GROUP BY SAL.RefDocId "
         + "), "
         + "GrnDetailAgg AS "
         + "( "
         + "SELECT InvGrnId, PurchaseOrderId, SUM(ItemQty) AS ItemQty "
         + "FROM dbo.InvGrnDetail "
         + "GROUP BY InvGrnId, PurchaseOrderId "
         + ") "
         + "SELECT "
         + "gh.Id, "
         + "gh.DocDate, "
         + "gh.DocNo, "
         + "gh.GpNo, "
         + "gp.GpDate, "
         + "gh.VehicleNo, "
         + "gh.BiltyNo, "
         + "gh.DeliveryTerm, "
         + "CASE WHEN ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 326)), 0) = 0 "
         + "AND PA.GrnId IS NOT NULL "
         + "THEN 1 ELSE 0 END AS GrnStatusId, "
         + "CASE WHEN ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 326)), 0) = 0 "
         + "AND PA.GrnId IS NOT NULL "
         + "THEN PA.ApprovalReasons ELSE 'Ready For Invoice' END AS GrnStatus, "
         + "sc.CompanyName AS SupplierCustomer, "
         + "gd.PurchaseOrderId, "
         + "gh.SupplierCustomerId, "
         + "gd.ItemQty, "
         + "poh.DocNo AS PurchaseOrder, "
         + "CASE WHEN gh.DocumentTypeId = 137 THEN 110 "
         + "WHEN gh.DocumentTypeId = 217 THEN 217 "
         + "ELSE gp.RefDocumentTypeId END AS RefDocumentTypeId, "
         + "CASE WHEN gp.RefDocumentTypeId = 41 THEN 'PurchaseOrder' "
         + "WHEN gp.RefDocumentTypeId = 104 THEN 'Supply Order' "
         + "WHEN gp.RefDocumentTypeId = 105 THEN 'Market Purchase' "
         + "WHEN gp.RefDocumentTypeId IN (106,110) OR gh.DocumentTypeId = 137 THEN 'Gate Purchase' "
         + "WHEN gh.DocumentTypeId = 217 THEN 'Purchase From Party Processing' "
         + "WHEN gp.RefDocumentTypeId = 98 THEN 'Sales Return' "
         + "ELSE 'Other' END AS PurchaseAgainst, "
         + "gh.BranchesId, "
         + "bb.BranchName "
         + "FROM dbo.InvGrn gh "
         + "INNER JOIN dbo.SupplierCustomer sc ON sc.Id = gh.SupplierCustomerId "
         + "INNER JOIN GrnDetailAgg gd ON gh.Id = gd.InvGrnId "
         + "LEFT JOIN dbo.GatePassInward gp ON gh.InwardGatePassId = gp.Id "
         + "LEFT JOIN dbo.PurchaseOrder poh ON gd.PurchaseOrderId = poh.Id "
         + "LEFT JOIN dbo.Branches bb ON bb.Id = gh.BranchesId "
         + "LEFT JOIN PendingApproval PA ON PA.GrnId = gh.Id "
         + "WHERE gh.OrganizationId = :organizationId "
         + "AND gh.CompanyId = :companyId "
         + "AND (:supplierCustomerId IS NULL OR gh.SupplierCustomerId = :supplierCustomerId) "
         + "AND (:financialYearId IS NULL OR gh.FinancialYearId = :financialYearId) "
         + "AND (:fromDate IS NULL OR gh.DocDate >= :fromDate) "
         + "AND (:toDate IS NULL OR gh.DocDate < DATEADD(DAY,1,:toDate)) "
         + "AND ((:documentTypeId = 46 AND gh.DocumentTypeId IN (46,217)) OR gh.DocumentTypeId = :documentTypeId) "
         + "AND NOT EXISTS ( "
         + "SELECT 1 "
         + "FROM dbo.InvPurchaseInvoiceDetail d "
         + "INNER JOIN dbo.InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE d.InvGrnId = gh.Id "
         + "AND d.ActionTypeId <> 3 "
         + "AND h.ActionId <> 3 "
         + "AND h.OrganizationId = :organizationId "
         + "AND h.CompanyId = :companyId "
         + ") "
         + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:branchesIds, ',') WHERE data = gh.BranchesId)) "
         + "ORDER BY gh.DocNo", nativeQuery = true)
    List<PurchaseInvoicePendingGrnRow> getPendingGrnForPurchaseInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("financialYearId") Integer financialYearId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("documentTypeId") Integer documentTypeId, @Param("branchesIds") String branchesIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetInvPurhcaseInvoiceIdByDocNo'
     * Output column Id.
     */
    @Query(value = "SELECT p.id FROM InvPurchaseInvoice p "
         + "WHERE p.docNo = :docNo AND p.documentTypeId = :documentTypeId AND p.organizationId = :organizationId AND p.companyId = :companyId")
    List<Integer> getInvPurhcaseInvoiceIdByDocNo(@Param("docNo") Integer docNo, @Param("documentTypeId") Integer documentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GrnLoadForPurchaseInvoiceTrading'
     * Native verbatim (nested RIGHT/LEFT OUTER JOIN chain, views V_UomScheduleAndUom / V_TaxScheduleForTax, fnSplitString).
     */
    @Query(value = "SELECT InvGrnDetail.Id as Id, InvGrnDetail.InvGrnId, GR.DocDate, GR.DocNo, GR.SupplierCustomerId, SC.CompanyName AS SupplierCustomer, GR.CarriageAmount, InvGrnDetail.ItemId, Item.ItemCode, Item.ItemName, InvGrnDetail.CropYear, "
         + "InvGrnDetail.ItemUomId , UomItm.UOMCode AS UOMCodeItem, InvGrnDetail.ItemQty, InvGrnDetail.WarehouseId, InvGrnDetail.PackingTypeId, InvGrnDetail.LabReportRef as LabAnalisysNo, InvGrnDetail.PurchaseOrderId, "
         + "PurchaseOrder.DocNo AS PurchaseOrder, InvGrnDetail.GrossWeight, InvGrnDetail.EBWPerUnit as EBWeight, InvGrnDetail.EBWTotal as EBTotalWt, InvGrnDetail.WtCut, InvGrnDetail.WtCutTotal as WeightCutTotal, ISNULL(InvGrnDetail.NetBillWeight, 0) AS NetBillWeight, "
         + "InvGrnDetail.StockWeight as NetStockWeight, InvGrnDetail.AreaCity, ISNULL(PurchaseOrderDetail.OrderItemRate, 0) AS OrderItemRate, "
         + "PurchaseOrderDetail.OrderItemRateUOMId, ISNULL(UomPoRate.Equivalent, 0) AS EquivalentPoRate, "
         + "CASE WHEN Isnull(dbo.InvGrnDetail.NetBillWeight, 0) > 0 AND Isnull(UomPoRate.Equivalent, 0) "
         + "> 0 THEN dbo.InvGrnDetail.NetBillWeight / UomPoRate.Equivalent * dbo.PurchaseOrderDetail.OrderItemRate ELSE 0 END AS ItemAmount, "
         + "CASE WHEN Isnull(dbo.InvGrnDetail.NetBillWeight, 0) > 0 "
         + "THEN dbo.InvGrnDetail.NetBillWeight / UomPoRate.Equivalent * dbo.PurchaseOrderDetail.OrderItemRate * Tax.TaxPercent / 100 "
         + "ELSE 0 END AS TaxAmount, "
         + "PurchaseOrderDetail.JobLotId,Tax.ItemTaxSchId, Tax.TaxPercent, PurchaseOrder.DeliveryTerm, Item.PurchaseGLAC, SC.GlAccountId, "
         + "PurchaseOrder.RemarksHeader, TR.AccountTitle, GR.GpNo, GR.VehicleNo, GR.InwardGatePassId "
         + "FROM ChartofAccount AS TR RIGHT OUTER JOIN "
         + "V_UomScheduleAndUom AS UomItm RIGHT OUTER JOIN "
         + "SupplierCustomer AS SC INNER JOIN "
         + "InvGrnDetail INNER JOIN "
         + "InvGrn AS GR ON GR.Id = InvGrnDetail.InvGrnId INNER JOIN "
         + "Item ON InvGrnDetail.ItemId = Item.Id ON SC.Id = GR.SupplierCustomerId ON UomItm.Id = InvGrnDetail.ItemUomId ON TR.Id = GR.TransporterId LEFT OUTER JOIN "
         + "V_UomScheduleAndUom AS UomPoRate INNER JOIN "
         + "PurchaseOrderDetail LEFT OUTER JOIN "
         + "PurchaseOrder ON PurchaseOrderDetail.PurchaseOrderId = PurchaseOrder.Id ON UomPoRate.Id = PurchaseOrderDetail.OrderItemRateUOMId ON InvGrnDetail.ItemId = PurchaseOrderDetail.OrderItemId AND "
         + "InvGrnDetail.PurchaseOrderId = PurchaseOrderDetail.Id LEFT OUTER JOIN "
         + "V_TaxScheduleForTax AS Tax ON Item.Id = Tax.ItemId "
         + "WHERE (Gr.Id in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseInvoiceGrnLoadTradingRow> grnLoadForPurchaseInvoiceTrading(@Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseOrderLoadForPurchaseInvoice'
     * Native: UDF GetPurchaseOrderandGatepassQtyBalance, views, fnSplitString. UOMCodeItem/RateUOM are numeric Equivalent values in this branch.
     */
    @Query(value = "SELECT PoH.Id,Pod.Id as PODetailId, PoH.DocNo AS OrderNo, PoH.DocDate ,poh.OrderSupCustId as SupplierCustomerId, SP.CompanyName AS SupplierName,PoH.OrderDueDate,PoH.OrderDueDays, "
         + "Pod.OrderItemId as ItemId,Item.ItemCode, Item.ItemName,Pod.Crop,Pod.OrderItemUOMId, UomItm.Equivalent AS UOMCodeItem,UomItm.UOMCode AS PackUom, "
         + "[dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId,:companyId,PoH.Id,Pod.OrderItemId,NULL,NULL) / UomItm.Equivalent AS ItemQty, "
         + "[dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId,:companyId,PoH.Id,Pod.OrderItemId,NULL,NULL) AS NetWeight, "
         + "pod.LabSampleNo, ISNULL(Pod.OrderItemRate, 0) AS ItemRate, Pod.OrderItemRateUOMId, "
         + "ISNULL(UomPoRate.Equivalent, 0) AS RateUOM,UomPoRate.UOMCode as RateUomCode,Pod.Amount "
         + ",Round(Isnull(Pod.BagWeight,0),3) EmptyBags,Round(Isnull(Pod.BagWeight,0),3) * isnull(Pod.OrderItemQty,0) EmptyBagsTotal, "
         + "case when UomItm.Equivalent > 0 "
         + "then [dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId,:companyId,PoH.Id,Pod.OrderItemId,NULL,NULL) "
         + "else 0 end GrossWeight, "
         + "Pod.TaxPercent,Pod.TaxAmount ,Cmgt.CompanyName AS BrokerAgentName, "
         + "PoH.CommissionType, PoH.CommRate, PoH.UomScheduleIdCmRate, PoH.CommAmount, PoH.DeliveryTerm,Poh.PaymentTermsId, PoH.CommissionRemarks, "
         + "PoH.BrokerAgentSupCustId, Item.PurchaseGLAC, PoH.RemarksHeader,Pod.JobLotId,Job.JobLotDescription "
         + ",PoH.BrokerAgentId,PoH.BrokeryType,PoH.BrokeryRate,PoH.BrokeryUom,PoH.BrokeryAmount,pod.CityArea,pod.OrderRemarks "
         + ",JAB.BranchId,bb.BranchName "
         + "FROM PurchaseOrder PoH "
         + "INNER JOIN PurchaseOrderDetail Pod on PoH.Id = Pod.PurchaseOrderId "
         + "INNER JOIN SupplierCustomer SP ON PoH.OrderSupCustId = SP.Id "
         + "INNER JOIN Item ON Pod.OrderItemId = ITEM.Id "
         + "INNER JOIN JobLot Job on Pod.JobLotId = Job.Id "
         + "LEFT JOIN V_UomScheduleAndUom UomPoRate on Pod.OrderItemRateUOMId = UomPoRate.Id "
         + "LEFT JOIN V_UomScheduleAndUom UomItm on Pod.OrderItemUOMId = UomItm.Id "
         + "LEFT JOIN SupplierCustomer Cmgt on PoH.BrokerAgentSupCustId = Cmgt.Id "
         + "LEFt JOIN JobLotsAllocationToBranch JAB on JAB.JobLotId=Pod.JobLotId "
         + "LEFT JOIN Branches bb on bb.Id =JAB.BranchId "
         + "WHERE PoH.OrganizationId = :organizationId "
         + "AND (PoH.CompanyId = :companyId) "
         + "AND (:supplierCustomerId IS NULL or PoH.OrderSupCustId = :supplierCustomerId) "
         + "AND (:fromDate IS NULL OR CAST(PoH.DocDate AS DATE) >= CAST(:fromDate AS DATE)) "
         + "AND (:toDate IS NULL OR CAST(PoH.DocDate AS DATE) <= CAST(:toDate AS DATE)) "
         + "AND (:docNo IS NULL OR PoH.DocNo = :docNo) "
         + "and (:documentTypeId is null or PoH.DocumentTypeId = :documentTypeId) "
         + "and (:poIds is null or PoH.Id in (select data from dbo.fnSplitString (:poIds,','))) "
         + "and ([dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId,:companyId,PoH.Id,Pod.OrderItemId,NULL,null) > 0)", nativeQuery = true)
    List<PurchaseInvoicePurchaseOrderLoadRow> purchaseOrderLoadForPurchaseInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("docNo") Integer docNo, @Param("documentTypeId") Integer documentTypeId, @Param("poIds") String poIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceandVoucherApprove'
     * KEEP-EXEC: @ReqType 'AP' approves / 'UP' un-approves VoucherHead + InvPurchaseInvoice + InventoryStockEvalautionDetail,
     * UP first checks settled production job orders and RAISERRORs; then EXEC USP_UserAudit_Insert (ActionId 2). Multi-table procedural logic kept in the proc.
     * Standalone single-table statements of this branch are also exposed below (approve/unapprove helpers).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @Id = :id, "
         + "@DocumentTypeId = :documentTypeId, @EntryUser = :entryUser, @ReqType = :reqType, @ScreenName = :screenName, "
         + "@Activity = 'InvPurchaseInvoiceandVoucherApprove'", nativeQuery = true)
    void invPurchaseInvoiceAndVoucherApprove(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("entryUser") Integer entryUser, @Param("reqType") String reqType, @Param("screenName") String screenName);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceandVoucherApprove'
     * @ReqType='AP' statement: UPDATE InvPurchaseInvoice SET IsApproved=1, PostDate=GETDATE(), PostUser=@EntryUser.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE InvPurchaseInvoice p SET p.isApproved = true, p.postDate = CURRENT_TIMESTAMP, p.postUser = :entryUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId AND p.documentTypeId = :documentTypeId")
    int approveInvoiceHeader(@Param("entryUser") Integer entryUser, @Param("id") Integer id, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceandVoucherApprove'
     * @ReqType='UP' statement (native: UnApproveDate is not a registry column of InvPurchaseInvoice).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE InvPurchaseInvoice SET IsApproved = 0, PostDate = NULL, PostUser = 0, UnApproveDate = GETDATE(), UnApproveUserId = :entryUser "
         + "WHERE Id = :id AND OrganizationId = :organizationId AND CompanyId = :companyId AND DocumentTypeId = :documentTypeId", nativeQuery = true)
    int unapproveInvoiceHeader(@Param("entryUser") Integer entryUser, @Param("id") Integer id, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceandVoucherApprove'
     * @ReqType='AP' statement on VoucherHead (native: PostState/PostDate not registry columns of VoucherHead).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE VoucherHead SET IsApproved = 1, PostState = 1, PostDate = GETDATE(), PostUser = :entryUser "
         + "WHERE DocumentTypeSrNo = :id AND OrganizationId = :organizationId AND CompanyId = :companyId AND DocumentTypeId = :documentTypeId", nativeQuery = true)
    int approveVoucherHead(@Param("entryUser") Integer entryUser, @Param("id") Integer id, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceandVoucherApprove'
     * @ReqType='UP' statement on VoucherHead.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE VoucherHead SET IsApproved = 0, PostState = 0, PostDate = NULL, PostUser = 0, UnApproveDate = GETDATE(), UnApproveUserId = :entryUser "
         + "WHERE DocumentTypeSrNo = :id AND OrganizationId = :organizationId AND CompanyId = :companyId AND DocumentTypeId = :documentTypeId", nativeQuery = true)
    int unapproveVoucherHead(@Param("entryUser") Integer entryUser, @Param("id") Integer id, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceandVoucherApprove'
     * AP → isApproved=true, UP → isApproved=false: UPDATE InventoryStockEvalautionDetail SET IsApproved=... WHERE RefDocIdNo=@Id AND RefDocumentTypeId=@DocumentTypeId.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE InventoryStockEvalautionDetail e SET e.isApproved = :isApproved "
         + "WHERE e.organizationId = :organizationId AND e.companyId = :companyId AND e.refDocIdNo = :id AND e.refDocumentTypeId = :documentTypeId")
    int setStockEvaluationApproved(@Param("isApproved") Boolean isApproved, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='LoadGrnForStorePurchaseInvoiceByGrnIds'
     * Native: UDF GetItemTaxScheduleDetailByItemId, views, fnSplitString. Proc default `IF @EffectedDate IS NULL SET @EffectedDate = GETDATE()` inlined as COALESCE(:effectedDate, GETDATE()).
     * TaxNameId/TaxPercent come from the same scalar UDF (string mode selector) so they are declared String.
     */
    @Query(value = "select g.Id as InvGrnId,g.SupplierCustomerId,g.RemarksHeader, d.Id,d.ItemId,i.ItemName,d.ItemQty, "
         + "d.WarehouseId,d.ItemUomId,Uom.UOMCode ,w.WareHouseName,isnull(pod.OrderItemRate,0) as ItemRate, "
         + "case when d.InvPurchasedemondId > 0 then d.InvPurchasedemondId "
         + "when d.PurchaseOrderId > 0 then d.PurchaseOrderId end InvPurchasedemondId, "
         + "case when d.InvPurchasedemondId > 0 then p.DocNo "
         + "when d.PurchaseOrderId > 0 then poh.DocNo end RefDocNo, "
         + "CASE WHEN Isnull(d.ItemQty, 0) > 0 AND Isnull(UomPoRate.Equivalent, 0) > 0 "
         + "THEN d.ItemQty / UomPoRate.Equivalent * pod.OrderItemRate "
         + "ELSE 0 "
         + "END AS ItemAmount, "
         + "pod.JobLotId "
         + ", poh.DeliveryTerm "
         + ", i.PurchaseGLAC "
         + ", sp.GlAccountId "
         + ",dbo.GetItemTaxScheduleDetailByItemId(g.OrganizationId,G.CompanyId,d.ItemId,COALESCE(:effectedDate, GETDATE()),'NameId') as TaxNameId "
         + ",dbo.GetItemTaxScheduleDetailByItemId(g.OrganizationId,g.CompanyId,d.ItemId,COALESCE(:effectedDate, GETDATE()),'Name') as TaxName "
         + ",dbo.GetItemTaxScheduleDetailByItemId(g.OrganizationId,g.CompanyId,d.ItemId,COALESCE(:effectedDate, GETDATE()),'Percent') as TaxPercent, "
         + "CASE WHEN poh.OrderTaxable = 1 "
         + "THEN (d.ItemQty / UomPoRate.Equivalent * pod.OrderItemRate) "
         + "* dbo.GetItemTaxScheduleDetailByItemId(g.OrganizationId,g.CompanyId,d.ItemId,COALESCE(:effectedDate, GETDATE()),'Percent') / 100 "
         + "END AS TaxAmount "
         + "from InvGrn g "
         + "inner join InvGrnDetail d on g.Id = d.InvGrnId "
         + "inner join Item i on d.ItemId = i.Id "
         + "inner join SupplierCustomer sp on g.SupplierCustomerId = sp.Id "
         + "inner join InvWareHouse w on d.WarehouseId = w.Id "
         + "inner join V_UomScheduleAndUom Uom on d.ItemUomId = Uom.Id "
         + "left join PurchaseOrder poh on d.PurchaseOrderId = poh.Id "
         + "left join PurchaseOrderDetail pod on poh.Id = pod.PurchaseOrderId and d.ItemId = pod.OrderItemId "
         + "left join InvPurchaseDemandHeader p on d.InvPurchasedemondId = p.Id "
         + "left join V_UomScheduleAndUom UomPoRate on pod.OrderItemRateUOMId = UomPoRate.Id "
         + "WHERE (g.Id in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseInvoiceStoreGrnLoadRow> loadGrnForStorePurchaseInvoiceByGrnIds(@Param("gdnIds") String gdnIds, @Param("effectedDate") LocalDate effectedDate);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseDetailStoreManaementReadByInvPurchaseInvoiceId'
     * Native verbatim (views, InvPurchaseDemandHeader). GrnDate is '' (string) in this branch.
     */
    @Query(value = "SELECT Pid.Id, Pid.InvPurchaseInvoiceId, Pid.InvGrnId, Pid.InvGrnDetailId, Pid.PurchaseOrderId,pid.PurchaseOrderDetailId, Pid.ItemId, Pid.PackingTypeId, Pid.CropYear, Pid.JobLotId, Pid.ItemQty, Pid.ItemUOMId, Pid.GrossWeight, Pid.EBWeight, Pid.EBTotalWt, "
         + "Pid.AdLsWeight, Pid.NetBillWeight, Pid.NetStockWeight, Pid.ItemRate, Pid.UomScheduleIdRate, Pid.ItemAmount, Pid.WarehouseId, Pid.LabAnalisysNo, Pid.GpDate, Pid.IsTaxable, Pid.TaxDescriptions, Pid.TaxAmount, "
         + "Pid.ItemCgsRate, Pid.AvgCgsRate, Pid.RemarksDetail, Pid.RateCut, Pid.RateCutAmount, Pid.BillAmount, Pid.FreightAmount, Pid.ExpenseAmount, Pid.JournalAmount, Pid.CommissionAmount, Pid.WeightCut, Pid.WeightCutTotal, "
         + "Pid.GpNo, Pid.VehicleNo, Pid.TaxNameId, i.ItemName, job.JobLotDescription, Pid.TaxPercent, i.PurchaseGLAC, i.ItemCode, Uom.UOMDescription AS UOMCodeItem,UOM.Equivalent AS ItemUOMEquivalent, UOMRate.Equivalent AS EquivalentPoRate,UOMRate.UOMCode as RateUom, "
         + "Wh.WareHouseName,Isnull(PT.PackTypeDesc,'') as PackTypeDesc, pid.CurrencyId,pid.ExchangeRate,pid.FcyAmount, "
         + "case when POH.Id > 0 then POH.DocNo "
         + "when PO.Id > 0 then PO.DocNo end PurchaseOrder "
         + ",'' as GrnDate,0 as GrnNo ,pid.Brokery,pid.ScaleKart,pid.LineId ,pid.WagesAmount , "
         + "Pid.AttributeVariantId,vt.ComputedName AttributeVariant,pid.EbPurAgainstWeight, "
         + "pid.BranchId,bb.BranchName,pid.PackingDate,pid.ExpiryDate,pid.CropYearId, "
         + "Pid.RefDocumentTypeId,DD.DocumentTypeDescription RefDocumentType, "
         + "Pid.RefDocId, "
         + "Pid.RefDocInvoiceId, "
         + "pid.RefDocSubId, "
         + "CAST('' as nvarchar(max)) RefDocNo, "
         + "CAST('' as nvarchar(max)) RefDocInvoiceNo, "
         + "CAST('' as float) RefInvoiceQty, "
         + "CAST('' as float) RefInvoiceWeight "
         + ",pid.DiscountTypeId "
         + ",pid.DiscountPercent "
         + ",pid.DiscountAmount "
         + ",pid.CityId "
         + ",pid.CastingTypeId "
         + ",pid.ProductionStageId, "
         + "i.WeightFinishGoods, "
         + "Po.DocDate OrderDate, "
         + "vt.VarientDescription VariantDescription, "
         + "ct.Description CityName, "
         + "Vc.CastingType, "
         + "PPS.ProductionProcessStage, "
         + "Case When pid.DiscountTypeId=1 then 'Flat' "
         + "when pid.DiscountTypeId=2 then 'Percent' "
         + "else '' end as DiscountType, "
         + "pid.GdnDocumentTypeId, "
         + "pid.GdnId , "
         + "pid.GdnDetailId, "
         + "Gdh.DocNo GdnNo, "
         + "Gdh.DocDate GdnDate, "
         + "pid.LocationTypeId, "
         + "pid.EbPurAgainstWeightAmount, "
         + "pid.ActionTypeId, "
         + "pid.ExcessWeight, "
         + "pid.RateCutForExcessWeight, "
         + "pid.RateCutAmountForExcessWeight, "
         + "pid.FreightDeduction, "
         + "pid.ItemConditionId,VIC.ConditionStatus as ItemCondition, "
         + "Pid.RackId,R.rackName,pid.ItemAmountWithoutDiscount, "
         + "Cast(0 as int ) as PurchaseOrderDocumentTypeId, "
         + "Cast(0 as float ) as PurchaseOrderQty, "
         + "Cast(0 as int ) as PurchaseDemandDocumentTypeId, "
         + "Cast(0 as int ) as PurchaseDemandId, "
         + "Cast(0 as int ) as PurchaseDemandDetailId, "
         + "Cast(0 as int ) as PurchaseDemandNo, "
         + "Cast(0 as float ) as PurchaseDemandQty, "
         + "Cast(0 as int ) as PreBillDocumentTypeId, "
         + "Cast(0 as int ) as PreBillId, "
         + "Cast(0 as int ) as PreBillDetailId, "
         + "Cast(0 as int ) as PreBillNo, "
         + "Cast(0 as float ) as PreBillQty, "
         + "Cast(0 as int ) as DeliveryChallanDocumentTypeId, "
         + "Cast(0 as int ) as DeliveryChallanId, "
         + "Cast(0 as int ) as DeliveryChallanDetailId, "
         + "Cast(0 as int ) as DeliveryChallanNo, "
         + "Cast(0 as float ) as DeliveryChallanQty, "
         + "Cast(0 as int ) as GrnDocumentTypeId, "
         + "Cast(0 as float ) as GrnQty, "
         + "Cast(0 as int ) as NoOfAttachmentsPO, "
         + "Cast(0 as int ) as NoOfAttachmentsPD, "
         + "Cast(0 as int ) as NoOfAttachmentsPreBill, "
         + "Cast(0 as int ) as NoOfAttachmentsDC, "
         + "Cast(0 as int ) as NoOfAttachmentsGrn "
         + "FROM InvPurchaseInvoiceDetail AS Pid "
         + "INNER JOIN Item AS i ON Pid.ItemId = i.Id "
         + "INNER JOIN V_UomScheduleAndUom AS Uom ON Pid.ItemUOMId = Uom.Id "
         + "INNER JOIN InvWareHouse AS Wh ON Pid.WarehouseId = Wh.Id "
         + "LEFT JOIN JobLot AS job ON Pid.JobLotId = job.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS UOMRate ON Pid.UomScheduleIdRate = UOMRate.Id "
         + "LEFT JOIN InvPurchaseDemandHeader AS POH ON Pid.PurchaseOrderId = POH.Id "
         + "LEFT JOIN InvPackingType PT ON Pid.PackingTypeId = PT.Id "
         + "LEFT JOIN PurchaseOrder PO ON Pid.PurchaseOrderId = PO.Id "
         + "LEFT JOIN PurchaseOrderDetail POD ON PO.Id = POD.PurchaseOrderId AND Pid.ItemId = POD.OrderItemId "
         + "LEFT JOIN ItemAttributeVarient vt on vt.Id=Pid.AttributeVariantId "
         + "LEFT JOIN Branches bb on bb.Id=pid.BranchId "
         + "LEFT JOIN DocumentType DD on DD.Id=pid.RefDocumentTypeId "
         + "LEFT JOIN mfg.VCastingType Vc on Vc.CastingTypeid=pid.CastingTypeId "
         + "LEFT JOIN Mfg.vProductionProcessStage PPS on PPS.ProductionProcessStageId=pid.ProductionStageId "
         + "LEFT JOIN InvGdn Gdh on Gdh.Id=Pid.GdnId and Gdh.DocumentTypeId = pid.GdnDocumentTypeId "
         + "LEFT JOIN City ct on ct.Id=pid.CityId "
         + "LEFT JOIN V_ItemCondition VIC on VIC.ID = pid.ItemConditionId "
         + "LEFT JOIN invWarehouseRack R ON Pid.RackId = R.Id "
         + "WHERE (Pid.InvPurchaseInvoiceId = :id) "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3", nativeQuery = true)
    List<PurchaseInvoiceStoreManagementDetailRow> purchaseDetailStoreManaementReadByInvPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseDetailPackingMaterialReadByHeaderId'
     * Native verbatim (views). GrnDate is '' (string) in this branch.
     */
    @Query(value = "SELECT Pid.Id, Pid.InvPurchaseInvoiceId, Pid.InvGrnId, Pid.InvGrnDetailId, Pid.PurchaseOrderId, Pid.ItemId, Pid.PackingTypeId, Pid.CropYear, Pid.JobLotId, Pid.ItemQty, Pid.ItemUOMId, Pid.GrossWeight, Pid.EBWeight, Pid.EBTotalWt, "
         + "Pid.AdLsWeight, Pid.NetBillWeight, Pid.NetStockWeight, Pid.ItemRate, Pid.UomScheduleIdRate, Pid.ItemAmount, Pid.WarehouseId, Pid.LabAnalisysNo, Pid.GpDate, Pid.IsTaxable,tx.TaxName as TaxDescriptions, Pid.TaxAmount, "
         + "Pid.ItemCgsRate, Pid.AvgCgsRate, Pid.RemarksDetail, Pid.RateCut, Pid.RateCutAmount, Pid.BillAmount, Pid.FreightAmount, Pid.ExpenseAmount, Pid.JournalAmount, Pid.CommissionAmount, Pid.WeightCut, Pid.WeightCutTotal, "
         + "Pid.GpNo, Pid.VehicleNo, Pid.TaxNameId, i.ItemName, job.JobLotDescription, Pid.TaxPercent, i.PurchaseGLAC, i.ItemCode, Uom.UOMCode AS UOMCodeItem,UOMRate.UOMCode as RateUom,UOMRate.Equivalent AS EquivalentPoRate, "
         + "Wh.WareHouseName,PO.DocNo AS PurchaseOrder,pid.Brokery,pid.ScaleKart,pid.PurchaseOrderDetailId,pid.LineId ,UOM.Equivalent AS ItemUOMEquivalent "
         + ",'' as GrnDate,0 as GrnNo,'' as PackTypeDesc , pid.CurrencyId,pid.ExchangeRate,pid.FcyAmount,pid.WagesAmount, "
         + "Pid.AttributeVariantId,vt.ComputedName AttributeVariant,pid.EbPurAgainstWeight,pid.BranchId,bb.BranchName,pid.PackingDate,pid.ExpiryDate,pid.CropYearId "
         + ",Pid.RefDocumentTypeId, "
         + "CAST('' as nvarchar(max)) RefDocumentType, "
         + "Pid.RefDocId,pid.RefDocSubId, "
         + "CAST('' as nvarchar(max)) RefDocNo, "
         + "Pid.RefDocInvoiceId, "
         + "CAST('' as nvarchar(max)) RefDocInvoiceNo, "
         + "CAST('' as float) RefInvoiceQty, "
         + "CAST('' as float) RefInvoiceWeight "
         + ",pid.DiscountTypeId "
         + ",pid.DiscountPercent "
         + ",pid.DiscountAmount "
         + ",pid.CityId "
         + ",pid.CastingTypeId "
         + ",pid.ProductionStageId, "
         + "i.WeightFinishGoods, "
         + "Po.DocDate OrderDate, "
         + "vt.VarientDescription VariantDescription, "
         + "ct.Description CityName, "
         + "Vc.CastingType, "
         + "PPS.ProductionProcessStage, "
         + "Case When pid.DiscountTypeId=1 then 'Flat' "
         + "when pid.DiscountTypeId=2 then 'Percent' "
         + "else '' end as DiscountType, "
         + "pid.GdnDocumentTypeId, "
         + "pid.GdnId , "
         + "pid.GdnDetailId, "
         + "Gdh.DocNo GdnNo, "
         + "Gdh.DocDate GdnDate, "
         + "pid.LocationTypeId, "
         + "pid.EbPurAgainstWeightAmount, "
         + "pid.ActionTypeId, "
         + "Pid.ExcessWeight, "
         + "pid.RateCutForExcessWeight, "
         + "pid.RateCutAmountForExcessWeight, "
         + "pid.FreightDeduction, "
         + "pid.ItemConditionId,VIC.ConditionStatus as ItemCondition, "
         + "Pid.RackId,R.rackName,pid.ItemAmountWithoutDiscount, "
         + "Cast(0 as int ) as PurchaseOrderDocumentTypeId, "
         + "Cast(0 as float ) as PurchaseOrderQty, "
         + "Cast(0 as int ) as PurchaseDemandDocumentTypeId, "
         + "Cast(0 as int ) as PurchaseDemandId, "
         + "Cast(0 as int ) as PurchaseDemandDetailId, "
         + "Cast(0 as int ) as PurchaseDemandNo, "
         + "Cast(0 as float ) as PurchaseDemandQty, "
         + "Cast(0 as int ) as PreBillDocumentTypeId, "
         + "Cast(0 as int ) as PreBillId, "
         + "Cast(0 as int ) as PreBillDetailId, "
         + "Cast(0 as int ) as PreBillNo, "
         + "Cast(0 as float ) as PreBillQty, "
         + "Cast(0 as int ) as DeliveryChallanDocumentTypeId, "
         + "Cast(0 as int ) as DeliveryChallanId, "
         + "Cast(0 as int ) as DeliveryChallanDetailId, "
         + "Cast(0 as int ) as DeliveryChallanNo, "
         + "Cast(0 as float ) as DeliveryChallanQty, "
         + "Cast(0 as int ) as GrnDocumentTypeId, "
         + "Cast(0 as float ) as GrnQty, "
         + "Cast(0 as int ) as NoOfAttachmentsPO, "
         + "Cast(0 as int ) as NoOfAttachmentsPD, "
         + "Cast(0 as int ) as NoOfAttachmentsPreBill, "
         + "Cast(0 as int ) as NoOfAttachmentsDC, "
         + "Cast(0 as int ) as NoOfAttachmentsGrn "
         + "FROM InvPurchaseInvoiceDetail AS Pid "
         + "INNER JOIN InvGrnDetail d on pid.InvGrnId = d.InvGrnId and Pid.InvGrnDetailId = d.Id "
         + "INNER JOIN Item AS i ON Pid.ItemId = i.Id "
         + "INNER JOIN V_UomScheduleAndUom AS Uom ON Pid.ItemUOMId = Uom.Id "
         + "INNER JOIN InvWareHouse AS Wh ON Pid.WarehouseId = Wh.Id "
         + "INNER JOIN V_UomScheduleAndUom AS UOMRate ON Pid.UomScheduleIdRate = UOMRate.Id "
         + "INNER JOIN PurchaseOrder PO ON Pid.PurchaseOrderId = PO.Id "
         + "INNER JOIN PurchaseOrderDetail POD ON PO.Id = POD.PurchaseOrderId AND Pid.ItemId = POD.OrderItemId and d.PurchaseOrderDetailId = POD.Id "
         + "LEFT JOIN JobLot AS job ON Pid.JobLotId = job.Id "
         + "LEFT JOIN TaxesTypes tx on pid.TaxNameId = tx.Id "
         + "Left join ItemAttributeVarient vt on vt.Id=Pid.AttributeVariantId "
         + "Left Join Branches bb on bb.Id=pid.BranchId "
         + "Left Join mfg.VCastingType Vc on Vc.CastingTypeid=pid.CastingTypeId "
         + "Left Join Mfg.vProductionProcessStage PPS on PPS.ProductionProcessStageId=pid.ProductionStageId "
         + "Left Join InvGdn Gdh on Gdh.Id=Pid.GdnId and Gdh.DocumentTypeId = pid.GdnDocumentTypeId "
         + "Left Join City ct on ct.Id=pid.CityId "
         + "LEFT JOIN V_ItemCondition VIC on VIC.ID = pid.ItemConditionId "
         + "LEFT JOIN invWarehouseRack R ON Pid.RackId = R.Id "
         + "WHERE (Pid.InvPurchaseInvoiceId = :id) "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3", nativeQuery = true)
    List<PurchaseInvoicePackingMaterialDetailRow> purchaseDetailPackingMaterialReadByHeaderId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetDataForPurchaseInvoiceRegisterDropdownBind'
     * Native: UNION ALL of 9 lookups (InventoryParentCategories has no entity).
     */
    @Query(value = "select Distinct 'Supplier' as ActivityType,pih.SupplierCustomerId as Id ,sp.CompanyName as Name "
         + "from InvPurchaseInvoice pih "
         + "inner join SupplierCustomer sp on pih.SupplierCustomerId = sp.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'CommissionAgent' as ActivityType,pih.CommissionAgentId as Id,sp.CompanyName as Name from InvPurchaseInvoice pih "
         + "inner join SupplierCustomer sp on pih.CommissionAgentId = sp.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'ItemName' as ActivityType,Pid.ItemId as Id,i.ItemName as Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'ItemCategory' as ActivityType,i.ItemCategoryId as Id,ic.CategoryDescription as Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'ItemType' as ActivityType,i.ItemTypeId as Id,it.TypeDescription as Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "inner join ItemType it on i.ItemTypeId = it.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'JobLot' as ActivityType,Pid.JobLotId as Id,j.JobLotDescription as Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "inner join JobLot j on Pid.JobLotId = j.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'WareHouse' as ActivityType,Pid.WarehouseId as Id,w.WareHouseName as Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "inner join InvWareHouse w on Pid.WarehouseId = w.Id "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'ItemParentCategory' as ActivityType,ic.InventoryParentCategoriesId as Id,p.InvParentCateDescription as Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Inner join InventoryParentCategories p on p.Id=ic.InventoryParentCategoriesId "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId "
         + "Union All "
         + "select Distinct 'CropYear' as ActivityType,w.Id,Pid.CropYear Name from InvPurchaseInvoice pih "
         + "inner join InvPurchaseInvoiceDetail Pid on Pih.Id = Pid.InvPurchaseInvoiceId "
         + "AND ISNULL(pid.ActionTypeId,0) <> 3 "
         + "inner join Item i on Pid.ItemId = i.Id "
         + "inner join InvCropYear w on Pid.CropYear = w.CropYear "
         + "where pih.OrganizationId = :organizationId "
         + "and pih.CompanyId = :companyId", nativeQuery = true)
    List<PurchaseInvoiceDropdownRow> getDataForPurchaseInvoiceRegisterDropdownBind(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPurchaseOrderExpensesChargToProdut'
     * Native: SELECT * from PurchaseOrderExpensesChargeToProduct (table has no entity and its column list is not in the proc) → javax.persistence.Tuple rows.
     * Proc guard `IF (@Id > 0 OR @POIds IS NOT NULL)` — caller must skip the call (empty result) when neither is given.
     */
    @Query(value = "select * from PurchaseOrderExpensesChargeToProduct "
         + "where (:id is null or PurchaseOrderId = :id) "
         + "and (:poIds is null or PurchaseOrderId in (select data from dbo.fnSplitString (:poIds,',')))", nativeQuery = true)
    List<Tuple> getPurchaseOrderExpensesChargToProdut(@Param("id") Integer id, @Param("poIds") String poIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPurchaseInvoiceReturnForApproval'
     * Native: InvSaleInvoice / InvSaleInvoiceDetail have no entity.
     */
    @Query(value = "select h.Id,h.DocDate,h.DocNo,sp.CompanyName CustomerName,h.RemarksHeader,i.ItemName,d.ItemQty,d.ItemRate,d.ItemAmount,w.WareHouseName "
         + "from InvSaleInvoice h "
         + "inner join InvSaleInvoiceDetail d on h.Id = d.InvSaleInvoiceId "
         + "inner join SupplierCustomer sp on h.SupplierCustomerId = sp.Id "
         + "inner join Item i on d.ItemId = i.Id "
         + "inner join InvWareHouse w on d.WarehouseId = w.Id "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.DocumentTypeId = :documentTypeId "
         + "and (Isnull(h.IsApproved,0) <> 1)", nativeQuery = true)
    List<PurchaseInvoiceReturnForApprovalRow> getPurchaseInvoiceReturnForApproval(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetLastInvoiceTransportAndBrokerAgentId'
     * Proc uses TOP 1 ORDER BY DocNo DESC → take first element.
     */
    @Query(value = "SELECT p.transportAccountId AS transportAccountId, p.brokerAgentId AS brokerAgentId "
         + "FROM InvPurchaseInvoice p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId AND p.documentTypeId = :documentTypeId "
         + "ORDER BY p.docNo DESC")
    List<PurchaseInvoiceLastTransportBrokerRow> getLastInvoiceTransportAndBrokerAgentId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetLastPurchaseTaxAcId'
     * Output column PurchaseTaxAcId (= TaxAccountId); proc uses TOP 1 ORDER BY Id DESC → take first element.
     */
    @Query(value = "SELECT p.taxAccountId FROM InvPurchaseInvoice p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId "
         + "ORDER BY p.id DESC")
    List<Integer> getLastPurchaseTaxAcId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetLastDiscountAcId'
     * Output column DiscountAccountId; proc uses TOP 1 ORDER BY Id DESC → take first element.
     */
    @Query(value = "SELECT p.discountAccountId FROM InvPurchaseInvoice p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId "
         + "ORDER BY p.id DESC")
    List<Integer> getLastDiscountAcId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetRecordId'
     * Output column DocDate (existing entity field InvPurchaseInvoice.docDate is java.util.Date).
     */
    @Query(value = "SELECT p.docDate FROM InvPurchaseInvoice p WHERE p.id = :id")
    List<Date> getRecordId(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport_HeaderData'
     * Native: ROW_NUMBER, derived table.
     */
    @Query(value = "Select * from ( "
         + "select distinct ROW_NUMBER () Over (Order By h.DocDate Desc) RecordNo, h.Id InvoiceId, h.DocNo InvoiceNumber,h.DocDate InvoiceDate "
         + ",POH.DocNo as Sale_Purcahse_OrderNumber,POH.Id Sale_Purchase_OrderId,h.InvoiceTypeId,h.SupplierCustomerId,Customer.CompanyName Customer_Supplier_Name "
         + ",h.IsApproved,h.BillAmount GrandBillAmount "
         + "FROM InvPurchaseInvoice h "
         + "Inner Join SupplierCustomer Customer on Customer.Id=h.SupplierCustomerId "
         + "LEFT JOIN (SELECT InvPurchaseInvoiceId,PurchaseOrderId "
         + "FROM InvPurchaseInvoiceDetail GROUP BY InvPurchaseInvoiceId,PurchaseOrderId) d on h.Id = d.InvPurchaseInvoiceId "
         + "LEFT JOIN PurchaseOrder POH ON D.PurchaseOrderId = POH.Id "
         + "WHERE h.[OrganizationId] = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.DocumentTypeId = :documentTypeId "
         + "and (:financialYearId is null or h.FinancialYearId = :financialYearId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and h.EntryUser = :entryUser)) "
         + "and (:isApproved is null or h.IsApproved = :isApproved) "
         + ")a Where (:noOfRecords is null or a.RecordNo <= :noOfRecords) "
         + "ORDER BY A.InvoiceNumber DESC", nativeQuery = true)
    List<PurchaseInvoicePdfHeaderDataRow> getHistoryForPdfReportHeaderData(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId, @Param("canViewAllRecord") Boolean canViewAllRecord, @Param("entryUser") Integer entryUser, @Param("isApproved") Boolean isApproved, @Param("noOfRecords") Integer noOfRecords);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport'
     * Result set 1 of 6 (#PurchaseInvoiceHeader). Native: ROW_NUMBER, AttachmentsHistory (no entity).
     */
    @Query(value = "Select * from ( "
         + "select distinct ROW_NUMBER () Over (Order By h.DocDate Desc) RecordNo, h.Id InvoiceId, h.DocNo InvoiceNumber,h.DocDate InvoiceDate, "
         + "h.SupplierReferenceNo Reference,h.ManualBillNo ManualBillNumber,h.DueDays,h.DueDate,h.RemarksHeader InvoiceHeaderRemarks "
         + ",POH.DocNo as Sale_Purcahse_OrderNumber,POH.Id Sale_Purchase_OrderId,h.InvoiceTypeId "
         + ",h.SupplierCustomerId Customer_Supplier_Id,Customer.CompanyName Customer_Supplier_Name "
         + ",Customer.MobilePersonal Customer_Supplier_MobileNumber,Customer.Email Customer_Supplier_Email "
         + ",Customer.Address1 Customer_Supplier_Address,Customer.GlAccountId Customer_Supplier_GlAccountId "
         + ",h.CommissionAgentId ComissionAgentId,comissionAgent.GlAccountId as ComissionAgentGlId,comissionAgent.CompanyName ComissionAgentName,h.CommissionType ComissionType "
         + ",h.CommRate ComissioniRate,h.UomScheduleIdCmRate ComissionRateUom,h.CommAmount ComissionAmount,h.CommissionRemarks ComissionRemarks "
         + ",h.BrokerAgentId BrokerAgentId,brokerAgent.GlAccountId as BrokerGlId,brokerAgent.CompanyName BrokerAgentName,h.BrokeryType,h.BrokeryRate,h.BrokeryUom BrokeryRateUom "
         + ",h.BrokeryAmount,h.OtherRemarks BrokeryRemarks "
         + ",h.IsApproved "
         + ",Entry_User.UserName EntryUser,h.EntryDate,Modify_User.UserName ModifyUser,h.ModifyDate "
         + ",Approved_User.UserName ApprovedUser,h.PostDate ApprovedDate,h.OrganizationId,h.CompanyId "
         + ",h.BillAmount GrandBillAmount "
         + ",(SELECT COUNT(Id) FROM dbo.AttachmentsHistory WHERE ReferenceDocumentTypeId=h.DocumentTypeId AND ReferenceDocumentId=h.Id) AttachmentsCount "
         + ",(SELECT COUNT(Id) FROM dbo.DMSAttachments WHERE RefDocumentTypeId = h.DocumentTypeId AND RefDocumentNo=h.Id) as NoOfAttachments "
         + "FROM InvPurchaseInvoice h "
         + "Inner Join SupplierCustomer Customer on Customer.Id=h.SupplierCustomerId "
         + "Inner Join UserAccount Entry_User on Entry_User.ID=h.EntryUser "
         + "LEFT JOIN (SELECT InvPurchaseInvoiceId,PurchaseOrderId "
         + "FROM InvPurchaseInvoiceDetail GROUP BY InvPurchaseInvoiceId,PurchaseOrderId) d on h.Id = d.InvPurchaseInvoiceId "
         + "LEFT JOIN PurchaseOrder POH ON D.PurchaseOrderId = POH.Id "
         + "Left Join SupplierCustomer comissionAgent on comissionAgent.Id=h.CommissionAgentId "
         + "Left Join SupplierCustomer brokerAgent on brokerAgent.Id=h.BrokerAgentId "
         + "Left Join UserAccount Modify_User on Modify_User.ID=h.ModifyUser "
         + "Left Join UserAccount Approved_User on Approved_User.ID=h.PostUser "
         + "WHERE h.Id=:id "
         + ")a", nativeQuery = true)
    List<PurchaseInvoicePdfHeaderRow> getHistoryForPdfReportHeader(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport'
     * Result set 2 of 6 (detail). #PurchaseInvoiceHeader join replaced by InvPurchaseInvoice h WHERE h.Id = :id (same InvoiceId/OrganizationId/CompanyId).
     */
    @Query(value = "select distinct d.Id,d.InvPurchaseInvoiceId InvoiceId,grh.Id Gdn_Grn_Id,grh.DocNo as Gdn_Grn_Number,gp.Id GatePassId,gp.GpSrNo as GatePassNumber,d.WarehouseId,warehouse.WareHouseName WarehouseName,d.JobLotId Job_LotId,job.JobLotDescription JobLotName "
         + ",d.CropYear,d.ItemId,i.ItemName,packing.PackTypeDesc PackingType,d.ItemQty,packUom.UOMCode PackUom "
         + ",d.GrossWeight,d.EBTotalWt BagWeight,d.WeightCutTotal WeightCut,d.NetBillWeight NetWeight,d.NetStockWeight StockWeight "
         + ",d.ItemRate Rate,rateUom.UOMCode RateUom,d.ItemAmount,d.RateCutAmount,d.FreightAmount,d.ExpenseAmount,d.CommissionAmount "
         + ",d.BillAmount "
         + ",d.ExcessWeight "
         + ",d.RateCutForExcessWeight,d.RateCutAmountForExcessWeight,i.PurchaseGLAC,i.SaleGLAC "
         + "FROM InvPurchaseInvoiceDetail d "
         + "inner join InvPurchaseInvoice h on h.Id=d.InvPurchaseInvoiceId and h.Id = :id "
         + "inner join InvWareHouse warehouse on warehouse.Id=d.WarehouseId "
         + "inner join Item i on i.Id=d.ItemId and i.OrganizationId=h.OrganizationId and i.CompanyId=h.CompanyId "
         + "left join InvGrnDetail grd on d.InvGrnId = grd.InvGrnId and d.InvGrnDetailId = grd.Id "
         + "left join InvGrn grh on grd.InvGrnId = grh.Id "
         + "left join GatePassInward gp on grh.InwardGatePassId = gp.Id "
         + "left join JobLot job on job.Id=d.JobLotId "
         + "left join InvPackingType packing on packing.Id=d.PackingTypeId "
         + "left join V_UomScheduleAndUom packUom on packUom.Id=d.ItemUOMId and packUom.CompanyId=h.CompanyId "
         + "left join V_UomScheduleAndUom rateUom on rateUom.Id=d.UomScheduleIdRate and rateUom.CompanyId=h.CompanyId", nativeQuery = true)
    List<PurchaseInvoicePdfDetailRow> getHistoryForPdfReportDetail(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport'
     * Result set 3 of 6 (ChargeToProduct / freight).
     */
    @Query(value = "select h.Id InvoiceId, f.TansporterId AccountId,transporter.AccountTitle AccountName,f.FreightAmount Amount,f.Remarks "
         + "FROM InvPurchaseInvoiceFreight f "
         + "inner join InvPurchaseInvoice h on h.Id=f.InvPurchaseInvoiceId and h.Id = :id "
         + "inner join ChartofAccount transporter on transporter.Id=f.TansporterId", nativeQuery = true)
    List<PurchaseInvoicePdfAccountAmountRow> getHistoryForPdfReportChargeToProduct(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport'
     * Result set 4 of 6 (SupplierAdd/Less journal).
     */
    @Query(value = "select h.Id InvoiceId,j.ChartofAccountId AccountId,account.AccountTitle AccountName,j.JvDebit DebitAmount,j.JvCredit CreditAmount "
         + ",j.JvRemarks Remarks "
         + "FROM InvPurchaseInvoiceJournal j "
         + "inner join InvPurchaseInvoice h on h.Id=j.InvPurchaseInvoiceId and h.Id = :id "
         + "inner join ChartofAccount account on account.Id=j.ChartofAccountId", nativeQuery = true)
    List<PurchaseInvoicePdfJournalRow> getHistoryForPdfReportSupplierAddLess(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport'
     * Result set 5 of 6 (OtherExpenses).
     */
    @Query(value = "select h.Id InvoiceId,e.InvRevExpItemId ItemId,i.OtherItemName ItemName,e.Amount,e.Remarks "
         + "FROM InvPurchaseInvoiceExpense e "
         + "inner join InvPurchaseInvoice h on h.Id=e.InvPurchaseInvoiceId and h.Id = :id "
         + "inner join InventoryItemsOther i on i.Id=e.InvRevExpItemId", nativeQuery = true)
    List<PurchaseInvoicePdfExpenseRow> getHistoryForPdfReportOtherExpenses(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport'
     * Result set 6 of 6 (EmptyBags, view vItemAllocation).
     */
    @Query(value = "select h.Id InvoiceId,b.ItemId,b.TypeId,i.ItemName,b.Amount,b.Remarks "
         + "FROM InvPurchaseInvoiceEmptyBags b "
         + "inner join InvPurchaseInvoice h on h.Id=b.InvPurchaseInvoiceId and h.Id = :id "
         + "inner join vItemAllocation i on i.Id=b.ItemId and i.OrganizationId=h.OrganizationId and i.CompanyId=h.CompanyId", nativeQuery = true)
    List<PurchaseInvoicePdfEmptyBagsRow> getHistoryForPdfReportEmptyBags(@Param("id") Integer id);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPendingGhallaMandiGrnForInvoice'
     * Native: Ghalla_Mandi (no entity), view V_UomScheduleAndUom, CONVERT(date,...,102).
     */
    @Query(value = "SELECT gh.Id, gh.DocDate,gh.DocNo,gh.GhallaMandiId,gm.GhallaMandi, gh.GpNo, gh.VehicleNo, gh.BiltyNo, "
         + "gh.SupplierCustomerId,SupplierCustomer.CompanyName AS LoadingContractor, "
         + "gd.ItemQty,Item.ItemCode,Item.ItemName,U.UOMCode as PackUom "
         + "FROM SupplierCustomer "
         + "INNER JOIN InvGrn gh ON SupplierCustomer.Id = gh.SupplierCustomerId "
         + "INNER JOIN InvGrnDetail gd ON gh.Id = gd.InvGrnId "
         + "INNER JOIN GatePassInward gp on gh.InwardGatePassId = gp.Id "
         + "INNER JOIN Item ON gd.ItemId = Item.Id "
         + "INNER JOIN V_UomScheduleAndUom AS U ON U.Id = gd.ItemUomId "
         + "INNER JOIN Ghalla_Mandi AS gm on gh.GhallaMandiId = gm.Id "
         + "where (gh.OrganizationId = :organizationId) "
         + "and (gh.CompanyId = :companyId) "
         + "and (gh.DocumentTypeId = :documentTypeId) "
         + "AND (gh.FinancialYearId = :financialYearId) "
         + "and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId) "
         + "and (:ghallaMandiId is null or gh.GhallaMandiId = :ghallaMandiId) "
         + "and (:fromDate is null or CAST(gh.DocDate AS DATE) >= (CONVERT(date,:fromDate,102))) "
         + "and (:toDate is null or CAST(gh.DocDate AS DATE) <= (CONVERT(date,:toDate,102))) "
         + "and (gh.Id NOT IN(SELECT InvGrnId FROM InvPurchaseInvoiceDetail "
         + "inner join InvPurchaseInvoice on InvPurchaseInvoice.Id = InvPurchaseInvoiceId "
         + "where OrganizationId = :organizationId and CompanyId = :companyId)) "
         + "Order by gh.DocNo", nativeQuery = true)
    List<PurchaseInvoicePendingGhallaMandiGrnRow> getPendingGhallaMandiGrnForInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("ghallaMandiId") Integer ghallaMandiId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='GrnGhallaMandiLoadForInvoice'
     * Native: Ghalla_Mandi, InvLabAnalysisPurchaseHeader (no entity), fnSplitString.
     */
    @Query(value = "SELECT distinct GD.Id as InvGrnDetailId, GD.InvGrnId, GR.DocDate, GR.DocNo,gr.GhallaMandiId,gm.GhallaMandi,GR.SupplierCustomerId,SP.CompanyName AS LoadingContractor "
         + ",TR.AccountTitle,ISNULL(GR.CarriageAmount, 0) As CarriageAmount,gr.RemarksHeader , "
         + "GD.ItemId, Item.ItemCode, Item.ItemName, GD.CropYear, GD.ItemUomId as ItemUOMId, UomItm.UOMCode AS UOMCodeItem, GD.ItemQty, GD.WarehouseId,wr.WareHouseName,GD.PackingTypeId, "
         + "GD.LabReportRef as LabAnalisysNo,GD.GrossWeight,GD.EBWPerUnit,GD.EBWTotal as EBTotalWt, "
         + "GD.WtCut,GD.WtCutTotal as WeightCutTotal,ISNULL(GD.AdLsWeight, 0) AdLsWeight,ISNULL(GD.ScaleKart, 0) ScaleKart,ISNULL(GD.NetBillWeight, 0) AS NetBillWeight, GD.StockWeight as NetStockWeight, "
         + "GD.AreaCity "
         + ",0 as ItemRate,0 as UomScheduleIdRate,40 as EquivalentPoRate,0 AS ItemAmount, "
         + "GD.JobLotId, GD.CommentsDetail, "
         + "GR.DeliveryTerm,Item.PurchaseGLAC,GR.GpNo,GR.VehicleNo,GR.InwardGatePassId,lp.DeductionWeight "
         + ",case when isnull(lp.ItemRate,0) = 0 then lp.DeductionRate else 0 end as DeductionRate "
         + "FROM dbo.SupplierCustomer sp "
         + "INNER JOIN dbo.InvGrn AS GR ON GR.SupplierCustomerId = sp.Id "
         + "INNER JOIN InvGrnDetail gd on gr.Id = gd.InvGrnId "
         + "INNER JOIN GatePassInward gp on GR.InwardGatePassId = gp.Id "
         + "INNER JOIN dbo.Item ON gd.ItemId = dbo.Item.Id "
         + "INNER JOIN dbo.V_UomScheduleAndUom AS UomItm ON GD.ItemUomId = UomItm.Id "
         + "INNER JOIN InvWareHouse wr on GD.WarehouseId = wr.Id "
         + "INNER JOIN Ghalla_Mandi gm ON GR.GhallaMandiId = GM.Id "
         + "LEFT JOIN dbo.ChartofAccount AS TR ON GR.TransporterId = TR.Id "
         + "LEFT JOIN InvLabAnalysisPurchaseHeader lp on gd.LabId = Lp.Id and gr.InwardGatePassId = lp.GatePassInwardId and GD.ItemId = lp.ItemId "
         + "WHERE (GR.Id in (select data from dbo.fnSplitString (:gdnIds,',')))", nativeQuery = true)
    List<PurchaseInvoiceGhallaMandiGrnLoadRow> grnGhallaMandiLoadForInvoice(@Param("gdnIds") String gdnIds);

    /**
     * Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseOrderPaymentTermDetailByPoIds'
     * Native: PurchaseOrderPaymentTermsDetail / InvDueTerms have no entity; fnSplitString.
     */
    @Query(value = "SELECT d.Id, "
         + "d.PurchaseOrderId, "
         + "Po.DocNo As PurchaseOrderNo, "
         + "d.PaymentTermId, "
         + "d.PaymentTermId as PaymentTermsId, "
         + "pt.TermsDescription, "
         + "d.PrcntOfTotal, "
         + "d.DueDays, "
         + "d.PaymentRemarks, "
         + "d.Amount, "
         + "d.SortNo, "
         + "d.DueDate "
         + "FROM PurchaseOrderPaymentTermsDetail d "
         + "INNER JOIN InvDueTerms pt on d.PaymentTermId = pt.Id "
         + "INNER JOIN PurchaseOrder Po ON Po.Id = d.PurchaseOrderId "
         + "WHERE D.PurchaseOrderId in (Select data from fnSplitString(:orderIds,','))", nativeQuery = true)
    List<PurchaseOrderPaymentTermRow> purchaseOrderPaymentTermDetailByPoIds(@Param("orderIds") String orderIds);
}
