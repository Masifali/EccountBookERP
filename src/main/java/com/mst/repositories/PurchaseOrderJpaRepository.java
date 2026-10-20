package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.PurchaseOrder;
import com.mst.projections.AdvancePaymentAdjustmentRow;
import com.mst.projections.MarketPurchaseOrderFormHistoryRow;
import com.mst.projections.PurchaseOrderBySupplierRow;
import com.mst.projections.PurchaseOrderDetailByHeaderRow;
import com.mst.projections.PurchaseOrderEmptyBagsByOrderRow;
import com.mst.projections.PurchaseOrderEmptyBagsRow;
import com.mst.projections.PurchaseOrderExpensesChargeToProductRow;
import com.mst.projections.PurchaseOrderForPurchaseInvoiceRow;
import com.mst.projections.PurchaseOrderForSupplyScheduleRow;
import com.mst.projections.PurchaseOrderFormHistoryRow;
import com.mst.projections.PurchaseOrderIdAndNoRow;
import com.mst.projections.PurchaseOrderIdDocNoRow;
import com.mst.projections.PurchaseOrderInfoForGatepassInwardRow;
import com.mst.projections.PurchaseOrderLabDeductionRow;
import com.mst.projections.PurchaseOrderModernHistoryDetailRow;
import com.mst.projections.PurchaseOrderModernHistoryEmptyBagsRow;
import com.mst.projections.PurchaseOrderModernHistoryExpenseRow;
import com.mst.projections.PurchaseOrderModernHistoryHeaderDataRow;
import com.mst.projections.PurchaseOrderModernHistoryRow;
import com.mst.projections.PurchaseOrderPaymentTermDetailRow;
import com.mst.projections.PurchaseOrderReadByIdRow;
import com.mst.projections.PurchaseOrderSupplierDispatchDetailRow;
import com.mst.projections.PurchaseOrderSupplierExpenseRow;

/**
 * Batch E_purchase_order repository for dbo.PurchaseOrder (the project already has PurchaseOrderRepository, hence the
 * "Jpa" suffix). Covers every @Activity branch of Sp_PurchaseOrder_GetAllMethod and Sp_PurchaseOrder_Update.
 *
 * Parameter defaults: every parameter of Sp_PurchaseOrder_GetAllMethod defaults to NULL; callers pass null for unused
 * optional filters (the {@code (:p IS NULL OR col = :p)} pattern of the proc is kept).
 */
@Repository
public interface PurchaseOrderJpaRepository extends JpaRepository<PurchaseOrder, Integer> {

    // =============================================================================================
    // Sp_PurchaseOrder_GetAllMethod
    // =============================================================================================

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='GenerateDocNoByDocumentTypeId'.
     * Native because of dbo.GetConfigurationByConfigId(.., 320) ("use purchase order balance in next financial year").
     */
    @Query(value = "SELECT ISNULL(MAX(CONVERT(int, DocNo)), 0) + 1 DocNo FROM dbo.PurchaseOrder "
         + "WHERE DocumentTypeId = :documentTypeId AND OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND ((ISNULL(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 320), 0) = 0 AND FinancialYearId = :financialYearId) "
         + "  OR (ISNULL(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 320), 0) = 1))",
         nativeQuery = true)
    Integer generateDocNoByDocumentTypeId(@Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId,
                                          @Param("documentTypeId") Integer documentTypeId,
                                          @Param("financialYearId") Integer financialYearId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GeneratePurchaseOrderBranchCodeByDocId' */
    @Query("SELECT COALESCE(MAX(p.branchSrNo), 0) + 1 FROM PurchaseOrder p "
         + "WHERE p.documentTypeId = :documentTypeId AND p.organizationId = :organizationId AND p.companyId = :companyId "
         + "AND p.financialYearId = :financialYearId AND p.branchesId = :branchesId")
    Integer generatePurchaseOrderBranchCodeByDocId(@Param("organizationId") Integer organizationId,
                                                   @Param("companyId") Integer companyId,
                                                   @Param("documentTypeId") Integer documentTypeId,
                                                   @Param("financialYearId") Integer financialYearId,
                                                   @Param("branchesId") Integer branchesId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetPurhcaseOrderIdByDocNo' */
    @Query("SELECT p.id FROM PurchaseOrder p WHERE p.docNo = :docNo AND p.documentTypeId = :documentTypeId "
         + "AND p.organizationId = :organizationId AND p.companyId = :companyId AND p.financialYearId = :financialYearId")
    List<Integer> getPurhcaseOrderIdByDocNo(@Param("docNo") Integer docNo,
                                            @Param("documentTypeId") Integer documentTypeId,
                                            @Param("organizationId") Integer organizationId,
                                            @Param("companyId") Integer companyId,
                                            @Param("financialYearId") Integer financialYearId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadById' */
    @Query("SELECT p.id AS id, p.documentTypeId AS documentTypeId, p.docNo AS docNo, p.branchSrNo AS branchSrNo, p.docDate AS docDate, "
         + "p.orderCatagoryId AS orderCatagoryId, p.catagorySrNo AS catagorySrNo, p.orderSupCustId AS orderSupCustId, "
         + "p.supplierRefNo AS supplierRefNo, p.refrenenceParty AS refrenenceParty, "
         + "p.supplierCustomerIdStockParty AS supplierCustomerIdStockParty, p.brokerAgentSupCustId AS brokerAgentSupCustId, "
         + "p.commissionType AS commissionType, p.commRate AS commRate, p.uomScheduleIdCmRate AS uomScheduleIdCmRate, "
         + "p.commAmount AS commAmount, p.commissionRemarks AS commissionRemarks, p.paymentTermsId AS paymentTermsId, "
         + "p.orderDueDays AS orderDueDays, p.orderDueDate AS orderDueDate, p.orderExpiryDate AS orderExpiryDate, "
         + "p.deliveryTerm AS deliveryTerm, p.deliveryStartDate AS deliveryStartDate, p.deliveryDays AS deliveryDays, "
         + "p.deliveryRemarks AS deliveryRemarks, p.orderTaxable AS orderTaxable, p.remarksHeader AS remarksHeader, "
         + "p.orderStatus AS orderStatus, p.isAproved AS isAproved, p.entryDate AS entryDate, p.entryUser AS entryUser, "
         + "p.modifyDate AS modifyDate, p.modifyUser AS modifyUser, p.postDate AS postDate, p.postUser AS postUser, "
         + "p.postState AS postState, p.organizationId AS organizationId, p.companyId AS companyId, p.branchesId AS branchesId, "
         + "p.projectsId AS projectsId, p.orderType AS orderType, p.financialYearId AS financialYearId, p.cityId AS cityId, "
         + "p.brokerAgentId AS brokerAgentId, p.brokeryType AS brokeryType, p.brokeryRate AS brokeryRate, p.brokeryUom AS brokeryUom, "
         + "p.brokeryAmount AS brokeryAmount, p.cashFreight AS cashFreight, p.creditFreight AS creditFreight, "
         + "p.billCalculateTypeId AS billCalculateTypeId, p.orderQty AS orderQty, p.orderWeight AS orderWeight, "
         + "p.orderAmount AS orderAmount, p.currencyId AS currencyId, p.exchangeRate AS exchangeRate, p.fcyAmount AS fcyAmount, "
         + "p.orderStatusRemarks AS orderStatusRemarks, p.baseDocumentTypeId AS baseDocumentTypeId, "
         + "p.attachmentsValues AS attachmentsValues, p.customAttachmentsValues AS customAttachmentsValues, "
         + "p.deliveryTermId AS deliveryTermId, p.locationTypeId AS locationTypeId, p.bookingPersonId AS bookingPersonId, "
         + "p.permitNo AS permitNo, p.permitIssueDate AS permitIssueDate, p.permitValidityUpto AS permitValidityUpto "
         + "FROM PurchaseOrder p WHERE p.id = :id")
    List<PurchaseOrderReadByIdRow> readById(@Param("id") Integer id);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='ReadByPurchaseOrderHeaderId'.
     * Native: V_UomScheduleAndUom view and the ExIm.., PurchasePreBill.., InvPurchaseDemand.. tables without entities.
     * The #DetailDataRecords temp table + 3 UPDATEs are rewritten as one statement: RefDocNo / RefDocInvoiceNo via
     * OUTER APPLY (TOP 1, same "first match wins" semantics as UPDATE ... FROM) and the three qty aggregates as LEFT JOINs.
     */
    @Query(value = "SELECT R.Id, R.PurchaseOrderId, R.OrderItemId, R.OrderItemUOMId, R.Amount, R.OrderItemQty, R.OrderItemRate, "
         + "R.OrderRemarks, R.Crop, R.JobLotId, R.NetWeight, R.OrderItemRateUOMId, item.ItemCodeNew as ItemCode, Item.ItemName, "
         + "ItemUom.UOMCode, ItemUom.Equivalent AS UOMDescription, JobLot.JobLotDescription, R.CityArea, R.LabSampleNo, "
         + "R.AmountCalcType, R.TaxableStatus, R.BagWeight, R.BagPrice, RateUom.UOMCode as RateUom, RateUom.Equivalent AS EquivalentRate, "
         + "R.Moisture, R.TaxAmount, R.LineId, R.RetailRate, R.TaxNameId, R.TaxPercent, R.TotalAmount, TaxesTypes.TaxName, "
         + "R.InvLabSampleAnalysisHeaderId, R.WeightPerQty, R.CurrencyId, R.ExchangeRate, R.FcyAmount, R.CityId, R.PackingDate, "
         + "R.ExpiryDate, R.CropYearId, R.LeadTime, R.WeightCapacity, R.RefDocumentTypeId, DD.DocumentTypeDescription RefDocumentType, "
         + "R.RefDocId, "
         + "CASE WHEN X244.Matched = 1 THEN X244.ScheduleCode WHEN X141.Matched = 1 THEN CAST(X141.DocNo AS nvarchar(50)) ELSE CAST('' as nvarchar(50)) END AS RefDocNo, "
         + "R.RefDocInvoiceId, "
         + "CASE WHEN X244.Matched = 1 THEN X244.InvoiceNo ELSE CAST('' as nvarchar(100)) END AS RefDocInvoiceNo, "
         + "R.LabAnalysisStandardScheduleId, "
         + "D.ApprovedQty - (ISNULL(po.OrderQty, 0) + ISNULL(grn.GrnQty, 0) + ISNULL(pb.PreBillQty, 0)) AS DemandBalanceQty "
         + "FROM PurchaseOrderDetail R "
         + "INNER JOIN Item ON Item.Id = R.OrderItemId "
         + "LEFT JOIN V_UomScheduleAndUom RateUom ON R.OrderItemRateUOMId = RateUom.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS ItemUom ON ItemUom.Id = R.OrderItemUOMId "
         + "LEFT JOIN TaxesTypes ON R.TaxNameId = TaxesTypes.Id "
         + "LEFT JOIN JobLot ON JobLot.Id = R.JobLotId "
         + "LEFT JOIN DocumentType DD ON DD.Id = R.RefDocumentTypeId "
         + "LEFT JOIN InvPurchaseDemandDetail D ON R.RefDocId = D.InvPurchaseDemandHeaderId AND D.Id = R.RefDocInvoiceId "
         + "OUTER APPLY (SELECT TOP 1 1 AS Matched, h.ScheduleCode, Eh.InvoiceNo FROM ExImLcOrderShipmentSchedule h "
         + "   LEFT JOIN ExImInvoicePackingDetail Ed ON Ed.ContractId = h.ExImLcOrderId AND Ed.ContractScheduleId = h.Id "
         + "   LEFT JOIN ExImInvoice Eh ON Eh.Id = Ed.ExImInvoiceId "
         + "   WHERE h.Id = R.RefDocId AND R.RefDocumentTypeId = 244) X244 "
         + "OUTER APPLY (SELECT TOP 1 1 AS Matched, h.DocNo FROM InvPurchaseDemandHeader h "
         + "   WHERE h.Id = R.RefDocId AND R.RefDocumentTypeId = 141) X141 "
         + "LEFT JOIN (SELECT d.RefDocId, d.RefDocInvoiceId, d.RefDocumentTypeId, SUM(d.OrderItemQty) AS OrderQty "
         + "   FROM dbo.PurchaseOrderDetail d INNER JOIN dbo.PurchaseOrder h ON h.Id = d.PurchaseOrderId "
         + "   WHERE (h.Id <> :id) GROUP BY d.RefDocId, d.RefDocInvoiceId, d.RefDocumentTypeId) po "
         + "   ON R.RefDocId = po.RefDocId AND R.RefDocInvoiceId = po.RefDocInvoiceId AND R.RefDocumentTypeId = po.RefDocumentTypeId "
         + "LEFT JOIN (SELECT d.InvPurchasedemondId, d.PurchaseDemondDetailId, SUM(d.ItemQty) AS GrnQty "
         + "   FROM dbo.InvGrnDetail d INNER JOIN dbo.InvGrn h ON h.Id = d.InvGrnId "
         + "   WHERE ISNULL(d.DeliveryChallanId, 0) = 0 AND ISNULL(d.DeliveryChallanDetailId, 0) = 0 "
         + "   GROUP BY d.InvPurchasedemondId, d.PurchaseDemondDetailId) grn "
         + "   ON R.RefDocId = grn.InvPurchasedemondId AND R.RefDocInvoiceId = grn.PurchaseDemondDetailId AND R.RefDocumentTypeId = 141 "
         + "LEFT JOIN (SELECT d.PurchaseDemandHeaderId, d.PurchaseDemandDetailId, SUM(d.Qty) AS PreBillQty "
         + "   FROM dbo.PurchasePreBillDetail d INNER JOIN dbo.PurchasePreBillHeader h ON h.PurchasePreBillHeaderId = d.PurchasePreBillHeaderId "
         + "   WHERE ISNULL(h.ActionId, 0) <> 3 AND ISNULL(d.ActionTypeId, 0) <> 3 "
         + "   GROUP BY d.PurchaseDemandHeaderId, d.PurchaseDemandDetailId) pb "
         + "   ON R.RefDocId = pb.PurchaseDemandHeaderId AND R.RefDocInvoiceId = pb.PurchaseDemandDetailId AND R.RefDocumentTypeId = 141 "
         + "WHERE (R.PurchaseOrderId = :id)",
         nativeQuery = true)
    List<PurchaseOrderDetailByHeaderRow> readByPurchaseOrderHeaderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderLabDeductionByHeaderId' (native: InvLabAnalysisItems has no entity). */
    @Query(value = "SELECT h.Id, h.InvLabAnalysisStandardDeductionPolicyHeaderId, h.PurchaseOrderId, h.PurchaseOrderDetailId, h.ItemId, "
         + "h.AnalysisParameterId, item.ItemName, h.RangeFrom, h.RangeTo, h.DeductFrom, h.StandardValue, h.DeductionValue, "
         + "h.WeightKgs, InvLabAnalysisItems.AnalysisParameterDescription AS AnalysisItem "
         + "FROM PurchaseOrderLabDeduction AS h "
         + "INNER JOIN InvLabAnalysisItems ON h.AnalysisParameterId = InvLabAnalysisItems.Id "
         + "INNER JOIN Item ON h.ItemId = item.Id "
         + "WHERE h.PurchaseOrderId = :id", nativeQuery = true)
    List<PurchaseOrderLabDeductionRow> readPurchaseOrderLabDeductionByHeaderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderSupplierExpenseByHeaderId' (native: PurchaseOrderSupplierExpense has no entity). */
    @Query(value = "SELECT R.[Id], R.[PurchaseOrderId], R.[InvRevExpItemId], R.[Qty], R.[Rate], R.[Amount], R.[Remarks], OIT.OtherItemName "
         + "FROM [dbo].[PurchaseOrderSupplierExpense] R "
         + "INNER JOIN InventoryItemsOther oit ON R.InvRevExpItemId = OIT.Id "
         + "WHERE PurchaseOrderId = :id", nativeQuery = true)
    List<PurchaseOrderSupplierExpenseRow> readPurchaseOrderSupplierExpenseByHeaderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderEmptyBagsDetailByHeaderId' (native: PurchaseOrderEmptyBags has no entity). */
    @Query(value = "SELECT h.Id, h.ItemId, i.ItemName, h.PackingTypeId, pt.PackTypeDesc, h.PurchaseOrderId, h.Rate, h.WeightCut, h.Type, "
         + "CASE h.Type WHEN 1 THEN 'Normal' WHEN 2 THEN 'Purchase Against Weight' WHEN 3 THEN 'Free of Cost' "
         + "WHEN 4 THEN 'Retained' WHEN 5 THEN 'Returned' ELSE '' END EmptyBagsType "
         + "FROM PurchaseOrderEmptyBags h "
         + "LEFT JOIN Item i ON h.ItemId = i.Id "
         + "LEFT JOIN InvPackingType pt ON h.PackingTypeId = pt.Id "
         + "WHERE h.PurchaseOrderId = :id", nativeQuery = true)
    List<PurchaseOrderEmptyBagsRow> readPurchaseOrderEmptyBagsDetailByHeaderId(@Param("id") Integer id);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId'
     * (native: PurchaseOrderExpensesChargeToProduct has no entity). The proc selects h.Percentage twice; the duplicate
     * column is collapsed to a single Percentage (identical value) so the native result set has unique column names.
     */
    @Query(value = "SELECT h.Id, h.AccountId, c.AccountTitle, h.Percentage, h.PurchaseOrderId, h.Qty, h.Rate, h.SupplierCustomerId, "
         + "sp.CompanyName SupplierCustomerName, h.Amount, h.Remarks "
         + "FROM PurchaseOrderExpensesChargeToProduct h "
         + "LEFT JOIN ChartofAccount c ON h.AccountId = c.Id "
         + "LEFT JOIN SupplierCustomer sp ON sp.Id = h.SupplierCustomerId "
         + "WHERE h.PurchaseOrderId = :id", nativeQuery = true)
    List<PurchaseOrderExpensesChargeToProductRow> readPurchaseOrderExpensesChargeToProductDetailByHeaderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadAdvancePaymentAdjustmentByHeaderId' (native: AdvancePaymentAdjustment has no entity). */
    @Query(value = "SELECT h.Id, h.PurchaseOrderId, h.VoucherId, h.AccountId, c.AccountTitle, h.SupplierCustomerId, "
         + "sp.CompanyName SupplierCustomerName, h.AdjustmentAmount, h.DocumentStatusId "
         + "FROM AdvancePaymentAdjustment h "
         + "LEFT JOIN ChartofAccount c ON h.AccountId = c.Id "
         + "LEFT JOIN SupplierCustomer sp ON sp.Id = h.SupplierCustomerId "
         + "WHERE h.PurchaseOrderId = :id", nativeQuery = true)
    List<AdvancePaymentAdjustmentRow> readAdvancePaymentAdjustmentByHeaderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderPaymentTermDetailByHeaderId' (native: PurchaseOrderPaymentTermsDetail / InvDueTerms have no entity). */
    @Query(value = "SELECT d.Id, d.PurchaseOrderId, d.PaymentTermId, pt.TermsDescription As PaymentTerm, d.PrcntOfTotal, d.DueDays, "
         + "d.PaymentRemarks, d.Amount, d.SortNo, d.DueDate "
         + "FROM PurchaseOrderPaymentTermsDetail d "
         + "INNER JOIN InvDueTerms pt ON d.PaymentTermId = pt.Id "
         + "WHERE D.PurchaseOrderId = :id", nativeQuery = true)
    List<PurchaseOrderPaymentTermDetailRow> purchaseOrderPaymentTermDetailByHeaderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadByHeaderId_PurchaseOrderSupplierDispatchDetail' (native: V_UomScheduleAndUom view). */
    @Query(value = "SELECT d.Id, d.PurchaseOrderId, d.SupplierDispatchId, SD.DocNo SupplierDispatchNo, D.PackingTypeId, pp.PackTypeDesc PackingType, "
         + "D.LoadingDetailId, d.ItemId, Item.ItemName, item.ItemCodeNew ItemCode, d.CropYearId, cr.CropYear, "
         + "d.ItemUOMId, ItemUom.UOMCode ItemUomCode, ItemUom.Equivalent ItemUomEquivalent, "
         + "d.ItemQty, d.NetWeight, d.ItemRate, d.RateUOMId, RateUom.UOMCode RateUomCode, RateUom.Equivalent RateUomEquivalent, d.Amount "
         + "FROM PurchaseOrderSupplierDispatchDetail d "
         + "INNER JOIN Item ON Item.Id = d.ItemId "
         + "LEFT JOIN V_UomScheduleAndUom RateUom ON d.RateUOMId = RateUom.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS ItemUom ON ItemUom.Id = d.ItemUOMId "
         + "LEFT JOIN InvCropYear Cr ON Cr.Id = d.CropYearId "
         + "LEFT JOIN SupplierDispatch SD ON SD.Id = d.SupplierDispatchId "
         + "LEFT JOIN InvPackingType pp ON pp.Id = d.PackingTypeId "
         + "WHERE (d.PurchaseOrderId = :id)", nativeQuery = true)
    List<PurchaseOrderSupplierDispatchDetailRow> readByHeaderId_PurchaseOrderSupplierDispatchDetail(@Param("id") Integer id);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderFormHistory'.
     * Native: ROW_NUMBER, DATEADD, STRING_AGG, fnSplitString, InvDueTerms/InvOrderCategory/LocationType/ReferenceParties/
     * AttachmentsHistory without entities. The #TmpOrderHistory pipeline (base select + 4 UPDATEs) is rewritten as one
     * statement with the aggregates as LEFT JOINs (ISNULL keeps the temp-table defaults 0 / '').
     * canViewAllRecord: bit (true = all records, false = only EntryUser = entryUser).
     */
    @Query(value = "WITH Base AS ( "
         + "SELECT ROW_NUMBER() OVER (ORDER BY poh.DocDate, POH.DocNo) AS RecordNo, Poh.Id, Poh.DocumentTypeId, Poh.DocNo, Poh.BranchSrNo, "
         + "Poh.BranchesId, Poh.DocDate, poh.OrderSupCustId, Poh.BrokerAgentSupCustId, Poh.CommissionType, Poh.CommRate, Poh.CommAmount, "
         + "Poh.CommissionRemarks, Poh.BrokerAgentId, Poh.BrokeryType, Poh.BrokeryRate, Poh.BrokeryAmount, Poh.BrokeryUom, "
         + "Poh.OrderDueDays as DueDays, Poh.OrderDueDate as DueDate, Poh.OrderExpiryDate, Poh.DeliveryTerm, Poh.DeliveryDays, "
         + "Poh.DeliveryStartDate, Poh.RemarksHeader, Poh.OrderStatus, poh.EntryDate, poh.EntryUser, poh.ModifyDate, poh.ModifyUser, "
         + "POH.PostDate, poh.PostUser, Poh.IsAproved, Poh.PaymentTermsId, poh.RefrenenceParty, poh.BookingPersonId, poh.OrderCatagoryId, "
         + "poh.DeliveryTermId, poh.LocationTypeId, CASE WHEN Poh.BillCalculateTypeId = 1 THEN 'On Weight' ELSE 'On Qty' END AS BillType "
         + "FROM PurchaseOrder Poh "
         + "WHERE Poh.OrganizationId = :organizationId AND Poh.CompanyId = :companyId AND Poh.DocumentTypeId = :documentTypeId "
         + "AND (:orderSupCustId IS NULL OR Poh.OrderSupCustId = :orderSupCustId) "
         + "AND (:referencePartyId IS NULL OR Poh.RefrenenceParty = :referencePartyId) "
         + "AND (:bookingPersonId IS NULL OR Poh.BookingPersonId = :bookingPersonId) "
         + "AND (:docDateFrom IS NULL OR Poh.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR poh.DocDate < DATEADD(DAY, 1, :docDateTo)) "
         + "AND (:entryFromDate IS NULL OR Poh.EntryDate >= :entryFromDate) "
         + "AND (:entryToDate IS NULL OR Poh.EntryDate < DATEADD(DAY, 1, :entryToDate)) "
         + "AND (:modifyFromDate IS NULL OR Poh.ModifyDate >= :modifyFromDate) "
         + "AND (:modifyToDate IS NULL OR Poh.ModifyDate < DATEADD(DAY, 1, :modifyToDate)) "
         + "AND (:approvedFromDate IS NULL OR Poh.PostDate >= :approvedFromDate) "
         + "AND (:approvedToDate IS NULL OR Poh.PostDate < DATEADD(DAY, 1, :approvedToDate)) "
         + "AND (:fromDocNo IS NULL OR Poh.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR Poh.DocNo <= :toDocNo) "
         + "AND Poh.OrderCatagoryId <> 7 "
         + "AND (:financialYearId IS NULL OR Poh.FinancialYearId = :financialYearId) "
         + "AND (:canViewAllRecord = 1 OR (:canViewAllRecord = 0 AND Poh.EntryUser = :entryUser)) "
         + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:branchesIds, ',') b WHERE b.data = Poh.BranchesId)) "
         + ") "
         + "SELECT Poh.RecordNo, Poh.Id, Poh.DocNo, Poh.DocDate, Poh.BranchSrNo, br.BranchName, Poh.BranchesId, "
         + "ISNULL(OQ.OrderQty, CAST(0 AS FLOAT)) AS OrderQty, Poh.IsAproved, supcust.CompanyName AS SupplierName, "
         + "SupCom.CompanyName AS CommissionAgentName, Poh.CommissionType, Poh.CommRate, Poh.CommAmount, Poh.CommissionRemarks, "
         + "poh.EntryUser, poh.EntryDate, poh.ModifyUser, poh.ModifyDate, SupBr.CompanyName as BrokerAgentName, Poh.BrokeryType, "
         + "Poh.BrokeryRate, Poh.BrokeryAmount, Poh.BrokeryUom, Poh.DueDays, Poh.DueDate, Poh.OrderExpiryDate, poh.DeliveryTermId, "
         + "DDT.Description DeliveryTermDB, Poh.DeliveryTerm, Poh.DeliveryDays, Poh.DeliveryStartDate, Poh.RemarksHeader, Poh.OrderStatus, "
         + "Dut.TermsDescription, Msr.FirstName + ' ' + Msr.LastName as ModifyUserName, Usr.FirstName + ' ' + Usr.LastName as UserName, "
         + "POH.PostDate, Asr.FirstName + ' ' + Asr.LastName as ApprovedUserName, o.OrderCategoryDescription as OrderCategoryDescription, "
         + "POH.BillType, ISNULL(PB.PreBillNos, CAST('' AS NVARCHAR(MAX))) AS PreBillNos, "
         + "ISNULL(PB.PreBillVehicleNos, CAST('' AS NVARCHAR(MAX))) AS PreBillVehicleNos, ISNULL(DA.NoOfAttachments, 0) AS NoOfAttachments, "
         + "poh.RefrenenceParty, RefP.ReferencePartyName, poh.BookingPersonId, RefBp.ReferencePartyName As BookingPerson, "
         + "ISNULL(CA.NoOfAttachments, 0) AS AttachmentsCount, poh.LocationTypeId, L.Location LocationType "
         + "FROM Base Poh "
         + "INNER JOIN SupplierCustomer AS supcust ON Poh.OrderSupCustId = supcust.Id "
         + "INNER JOIN InvDueTerms Dut ON Poh.PaymentTermsId = Dut.Id "
         + "INNER JOIN UserAccount Usr ON Poh.EntryUser = Usr.ID "
         + "LEFT JOIN UserAccount Msr ON Poh.ModifyUser = Msr.ID "
         + "LEFT JOIN UserAccount Asr ON Poh.PostUser = Asr.ID "
         + "LEFT JOIN InvOrderCategory o ON Poh.OrderCatagoryId = o.Id "
         + "LEFT JOIN SupplierCustomer AS SupBr ON Poh.BrokerAgentId = SupBr.Id "
         + "LEFT JOIN SupplierCustomer AS SupCom ON Poh.BrokerAgentSupCustId = SupCom.Id "
         + "INNER JOIN Branches br ON Poh.BranchesId = br.Id "
         + "LEFT JOIN DeliveryTerm DDT ON DDT.Id = Poh.DeliveryTermId "
         + "LEFT JOIN LocationType L ON L.Id = poh.LocationTypeId "
         + "LEFT JOIN ReferenceParties RefP ON RefP.Id = poh.RefrenenceParty "
         + "LEFT JOIN ReferenceParties RefBp ON RefBp.Id = poh.BookingPersonId "
         + "LEFT JOIN (SELECT PurchaseOrderId, Sum(OrderItemQty) OrderQty FROM PurchaseOrderDetail GROUP BY PurchaseOrderId) OQ ON Poh.Id = OQ.PurchaseOrderId "
         + "LEFT JOIN (SELECT d.RefDocumentTypeId, d.RefDocumentNo, COUNT(d.Id) AS NoOfAttachments FROM DMSAttachments d "
         + "   GROUP BY d.RefDocumentTypeId, d.RefDocumentNo) DA ON DA.RefDocumentTypeId = Poh.DocumentTypeId AND DA.RefDocumentNo = Poh.Id "
         + "LEFT JOIN (SELECT d.ReferenceDocumentTypeId, d.ReferenceDocumentId, COUNT(d.Id) AS NoOfAttachments FROM AttachmentsHistory d "
         + "   WHERE d.ActionTypeId <> 3 GROUP BY d.ReferenceDocumentTypeId, d.ReferenceDocumentId) CA "
         + "   ON CA.ReferenceDocumentTypeId = Poh.DocumentTypeId AND CA.ReferenceDocumentId = Poh.Id "
         + "LEFT JOIN (SELECT X.PurchaseOrderId AS Id, STRING_AGG(S.DocNo, ',') AS PreBillNos, STRING_AGG(S.VehicleNo, ',') AS PreBillVehicleNos "
         + "   FROM (SELECT DISTINCT D.PurchaseOrderId, D.SupplierDispatchId FROM PurchaseOrderSupplierDispatchDetail D) X "
         + "   INNER JOIN SupplierDispatch S ON X.SupplierDispatchId = S.Id GROUP BY X.PurchaseOrderId) PB ON Poh.Id = PB.Id "
         + "WHERE (:noOfRecords IS NULL OR Poh.RecordNo <= :noOfRecords) "
         + "ORDER BY Poh.RecordNo DESC",
         nativeQuery = true)
    List<PurchaseOrderFormHistoryRow> purchaseOrderFormHistory(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId,
                                                               @Param("documentTypeId") Integer documentTypeId,
                                                               @Param("orderSupCustId") Integer orderSupCustId,
                                                               @Param("referencePartyId") Integer referencePartyId,
                                                               @Param("bookingPersonId") Integer bookingPersonId,
                                                               @Param("docDateFrom") LocalDate docDateFrom,
                                                               @Param("docDateTo") LocalDate docDateTo,
                                                               @Param("entryFromDate") LocalDate entryFromDate,
                                                               @Param("entryToDate") LocalDate entryToDate,
                                                               @Param("modifyFromDate") LocalDate modifyFromDate,
                                                               @Param("modifyToDate") LocalDate modifyToDate,
                                                               @Param("approvedFromDate") LocalDate approvedFromDate,
                                                               @Param("approvedToDate") LocalDate approvedToDate,
                                                               @Param("fromDocNo") Integer fromDocNo,
                                                               @Param("toDocNo") Integer toDocNo,
                                                               @Param("financialYearId") Integer financialYearId,
                                                               @Param("canViewAllRecord") Boolean canViewAllRecord,
                                                               @Param("entryUser") Integer entryUser,
                                                               @Param("branchesIds") String branchesIds,
                                                               @Param("noOfRecords") Long noOfRecords);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchasrOrderNotInInvoice' */
    @Query("SELECT p.id AS id, p.docNo AS docNo FROM PurchaseOrder p "
         + "WHERE p.id NOT IN (SELECT DISTINCT COALESCE(d.purchaseOrderId, 0) FROM InvPurchaseInvoiceDetail d "
         + "   JOIN InvPurchaseInvoice i ON i.id = d.invPurchaseInvoiceId "
         + "   WHERE i.organizationId = :organizationId AND i.companyId = :companyId) "
         + "AND p.documentTypeId = :documentTypeId")
    List<PurchaseOrderIdDocNoRow> getPurchasrOrderNotInInvoice(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId,
                                                               @Param("documentTypeId") Integer documentTypeId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadForInvGrn' */
    @Query("SELECT p.id AS id, p.docNo AS docNo FROM PurchaseOrder p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId "
         + "AND p.documentTypeId = :documentTypeId AND p.orderSupCustId = :orderSupCustId")
    List<PurchaseOrderIdDocNoRow> readForInvGrn(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId,
                                                @Param("documentTypeId") Integer documentTypeId,
                                                @Param("orderSupCustId") Integer orderSupCustId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderdetailforSupplySchedule' (native: V_UomScheduleAndUom view, fnSplitString). ids = comma separated PurchaseOrder ids. */
    @Query(value = "SELECT PiD.Id, PiH.DocDate, PiH.DocNo, PiH.OrderSupCustId, Sp.CompanyName, PiH.SupplierRefNo, "
         + "PiD.OrderItemId AS ItemId, PiD.OrderItemUOMId, dbo.Item.ItemName, ISNULL(PiD.OrderItemQty, 0) AS OrderItemQty, dbo.Item.ItemCode, UomRate.UOMCode "
         + "FROM dbo.PurchaseOrder AS PiH "
         + "INNER JOIN dbo.PurchaseOrderDetail AS PiD ON PiH.Id = PiD.PurchaseOrderId "
         + "INNER JOIN dbo.SupplierCustomer AS Sp ON PiH.OrderSupCustId = Sp.Id "
         + "INNER JOIN dbo.Item ON PiD.OrderItemId = dbo.Item.Id "
         + "INNER JOIN dbo.V_UomScheduleAndUom AS UomRate ON PiD.OrderItemUOMId = UomRate.Id "
         + "WHERE PiH.OrganizationId = :organizationId AND PiH.CompanyId = :companyId "
         + "AND (:orderSupCustId IS NULL OR pih.OrderSupCustId = :orderSupCustId) "
         + "AND (:docDateFrom IS NULL OR PiH.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR PiH.DocDate <= :docDateTo) "
         + "AND (:docNo IS NULL OR PiH.DocNo = :docNo) "
         + "AND (:id IS NULL OR PiH.Id = :id) "
         + "AND (:documentTypeId IS NULL OR PiH.DocumentTypeId = :documentTypeId) "
         + "AND (:ids IS NULL OR PiH.Id IN (SELECT data FROM dbo.fnSplitString(:ids, ',')))",
         nativeQuery = true)
    List<PurchaseOrderForSupplyScheduleRow> getPurchaseOrderdetailforSupplySchedule(@Param("organizationId") Integer organizationId,
                                                                                    @Param("companyId") Integer companyId,
                                                                                    @Param("orderSupCustId") Integer orderSupCustId,
                                                                                    @Param("docDateFrom") LocalDate docDateFrom,
                                                                                    @Param("docDateTo") LocalDate docDateTo,
                                                                                    @Param("docNo") Integer docNo,
                                                                                    @Param("id") Integer id,
                                                                                    @Param("documentTypeId") Integer documentTypeId,
                                                                                    @Param("ids") String ids);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderForPurchaseInvoice' (native: dbo.GetPurchaseOrderandGatepassQtyBalance, V_UomScheduleAndUom, fnSplitString). ids = comma separated PurchaseOrder ids to EXCLUDE. */
    @Query(value = "SELECT PiH.Id, PiH.DocDate, PiH.DocNo, PiH.OrderSupCustId SupplierCustomerId, Sp.CompanyName as SupplierName, Item.ItemName, "
         + "ROUND([dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId, :companyId, PiH.Id, PiD.OrderItemId, NULL, NULL) / PackUom.Equivalent, 0) AS ItemQty, "
         + "[dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId, :companyId, PiH.Id, PiD.OrderItemId, NULL, NULL) ItemWeight, "
         + "Item.ItemCode, UomRate.UOMCode, Job.JobLotDescription, UomRate.Equivalent, Pid.CropYearId, pid.Crop, pid.OrderItemUOMId PackUomId, "
         + "PackUom.UOMCode PackUom, PackUom.Equivalent PackEquivalent, pid.OrderItemRateUOMId RateUomId, pid.OrderItemRate, "
         + "Pih.DeliveryTerm, Pih.BrokerAgentId, Pih.BrokeryType, Pih.BrokeryRate, Pih.BrokeryUom, Pih.BrokeryAmount, "
         + "Pih.BrokerAgentSupCustId, Pih.CommissionType, Pih.CommRate, Pih.UomScheduleIdCmRate, Pih.CommAmount, Pih.RemarksHeader, pid.Id PODetailId, "
         + "Round(Isnull(PiD.BagWeight, 0), 3) EmptyBags, Round(Isnull(PiD.BagWeight, 0), 3) * isnull(PiD.OrderItemQty, 0) EmptyBagsTotal, "
         + "CASE WHEN PackUom.Equivalent > 0 THEN [dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId, :companyId, PiH.Id, PiD.OrderItemId, NULL, NULL) ELSE 0 END GrossWeight, "
         + "PiD.OrderItemId "
         + "FROM dbo.PurchaseOrder AS PiH "
         + "INNER JOIN dbo.PurchaseOrderDetail AS PiD ON PiH.Id = PiD.PurchaseOrderId "
         + "INNER JOIN dbo.SupplierCustomer AS Sp ON PiH.OrderSupCustId = Sp.Id "
         + "INNER JOIN dbo.Item ON PiD.OrderItemId = dbo.Item.Id "
         + "INNER JOIN dbo.V_UomScheduleAndUom AS UomRate ON PiD.OrderItemUOMId = UomRate.Id "
         + "INNER JOIN V_UomScheduleAndUom PackUom ON PiD.OrderItemUOMId = PackUom.Id "
         + "LEFT JOIN JobLot Job ON PiD.JobLotId = Job.Id "
         + "WHERE PiH.OrganizationId = :organizationId AND (PiH.CompanyId = :companyId) "
         + "AND (:orderSupCustId IS NULL OR pih.OrderSupCustId = :orderSupCustId) "
         + "AND (:docDateFrom IS NULL OR PiH.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR PiH.DocDate <= :docDateTo) "
         + "AND (:fromDocNo IS NULL OR PiH.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR Pih.DocNo <= :toDocNo) "
         + "AND (:id IS NULL OR PiH.Id = :id) "
         + "AND (PiH.OrderStatus = 'Open') "
         + "AND (:documentTypeId IS NULL OR PiH.DocumentTypeId = :documentTypeId) "
         + "AND (:ids IS NULL OR PiH.Id NOT IN (SELECT data FROM dbo.fnSplitString(:ids, ','))) "
         + "AND ([dbo].[GetPurchaseOrderandGatepassQtyBalance](PIH.OrganizationId, PIH.CompanyId, PiH.Id, PiD.OrderItemId, NULL, NULL) > 0) "
         + "AND (pih.IsAproved = 1) "
         + "ORDER BY pih.DocNo",
         nativeQuery = true)
    List<PurchaseOrderForPurchaseInvoiceRow> getPurchaseOrderForPurchaseInvoice(@Param("organizationId") Integer organizationId,
                                                                                @Param("companyId") Integer companyId,
                                                                                @Param("orderSupCustId") Integer orderSupCustId,
                                                                                @Param("docDateFrom") LocalDate docDateFrom,
                                                                                @Param("docDateTo") LocalDate docDateTo,
                                                                                @Param("fromDocNo") Integer fromDocNo,
                                                                                @Param("toDocNo") Integer toDocNo,
                                                                                @Param("id") Integer id,
                                                                                @Param("documentTypeId") Integer documentTypeId,
                                                                                @Param("ids") String ids);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' (all @ReqType sub-branches:
     * 'Status', 'Cancel', 'ApproveAndCancel', 'Approve', 'PendingForView', 'Open', 'UpdateExpiryDate').
     * // KEEP-EXEC: after the UPDATE the proc reads DocNo/DocumentType and calls USP_UserAudit_Insert (ActionId = 2).
     * The individual UPDATE statements are also exposed below as JPQL (without the audit row).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id = :id, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@ReqType = :reqType, @OrderStatusRemarks = :orderStatusRemarks, @PostUser = :postUser, "
         + "@OrderExpiryDate = :orderExpiryDate, @Activity = 'UpdateStatusandIsapprovedByOrderId'",
         nativeQuery = true)
    void updateStatusandIsapprovedByOrderId(@Param("id") Integer id,
                                            @Param("organizationId") Integer organizationId,
                                            @Param("companyId") Integer companyId,
                                            @Param("reqType") String reqType,
                                            @Param("orderStatusRemarks") String orderStatusRemarks,
                                            @Param("postUser") Integer postUser,
                                            @Param("orderExpiryDate") LocalDate orderExpiryDate);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='Status' (OrderStatus = 'Complete'). */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.orderStatus = 'Complete', p.orderStatusRemarks = :orderStatusRemarks, "
         + "p.statusChangedDate = CURRENT_TIMESTAMP, p.statusChangedById = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateStatusComplete(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId, @Param("orderStatusRemarks") String orderStatusRemarks,
                             @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='Cancel'. */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.orderStatus = 'Cancel', p.isAproved = true, p.orderStatusRemarks = :orderStatusRemarks, "
         + "p.statusChangedDate = CURRENT_TIMESTAMP, p.statusChangedById = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateStatusCancel(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                           @Param("companyId") Integer companyId, @Param("orderStatusRemarks") String orderStatusRemarks,
                           @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='ApproveAndCancel'. */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.isAproved = true, p.postState = false, p.postDate = CURRENT_TIMESTAMP, p.postUser = :postUser, "
         + "p.orderStatus = 'Cancel', p.orderStatusRemarks = :orderStatusRemarks, "
         + "p.statusChangedDate = CURRENT_TIMESTAMP, p.statusChangedById = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateStatusApproveAndCancel(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                                     @Param("companyId") Integer companyId, @Param("orderStatusRemarks") String orderStatusRemarks,
                                     @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='Approve'. */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.isAproved = true, p.postState = false, p.postDate = CURRENT_TIMESTAMP, p.postUser = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateStatusApprove(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId, @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='PendingForView'. */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.isAproved = false, p.postState = true, p.postDate = CURRENT_TIMESTAMP, p.postUser = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateStatusPendingForView(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId, @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='Open'. */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.orderStatus = 'Open', p.orderStatusRemarks = :orderStatusRemarks, "
         + "p.statusChangedDate = CURRENT_TIMESTAMP, p.statusChangedById = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateStatusOpen(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                         @Param("companyId") Integer companyId, @Param("orderStatusRemarks") String orderStatusRemarks,
                         @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId' @ReqType='UpdateExpiryDate'. */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.orderExpiryDate = :orderExpiryDate, p.orderStatusRemarks = :orderStatusRemarks, "
         + "p.modifyDate = CURRENT_TIMESTAMP, p.modifyUser = :postUser "
         + "WHERE p.id = :id AND p.organizationId = :organizationId AND p.companyId = :companyId")
    int updateExpiryDate(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
                         @Param("companyId") Integer companyId, @Param("orderExpiryDate") LocalDate orderExpiryDate,
                         @Param("orderStatusRemarks") String orderStatusRemarks, @Param("postUser") Integer postUser);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PONoFill' */
    @Query("SELECT p.id AS id, p.docNo AS docNo FROM PurchaseOrder p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId")
    List<PurchaseOrderIdDocNoRow> poNoFill(@Param("organizationId") Integer organizationId,
                                           @Param("companyId") Integer companyId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderIdandNoGetForSupplierCustomerId' */
    @Query("SELECT h.id AS purchaseOrderId, h.docNo AS purchaseOrderNo FROM PurchaseOrder h "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.orderSupCustId = :orderSupCustId "
         + "AND h.documentTypeId = :documentTypeId AND h.isAproved = true "
         + "AND h.id NOT IN (SELECT d.purchaseOrderId FROM InvGrn gh JOIN InvGrnDetail d ON gh.id = d.invGrnId "
         + "   WHERE gh.organizationId = :organizationId AND gh.companyId = :companyId AND gh.documentTypeId = :documentTypeId)")
    List<PurchaseOrderIdAndNoRow> purchaseOrderIdandNoGetForSupplierCustomerId(@Param("organizationId") Integer organizationId,
                                                                               @Param("companyId") Integer companyId,
                                                                               @Param("orderSupCustId") Integer orderSupCustId,
                                                                               @Param("documentTypeId") Integer documentTypeId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='MarketPurchaseOrderFormHistory' (native: ROW_NUMBER, InvDueTerms/InvOrderCategory without entity). */
    @Query(value = "SELECT * FROM ( "
         + "SELECT ROW_NUMBER() OVER (ORDER BY PurchaseOrder.DocDate DESC) RecordNo, PurchaseOrder.Id, PurchaseOrder.DocNo, PurchaseOrder.DocDate, "
         + "supcust.CompanyName AS SupplierName, PurchaseOrder.CommissionType, PurchaseOrder.CommRate, PurchaseOrder.CommAmount, PurchaseOrder.IsAproved, "
         + "PurchaseOrder.CommissionRemarks, PurchaseOrder.OrderDueDays as DueDays, PurchaseOrder.OrderDueDate as DueDate, PurchaseOrder.OrderExpiryDate, "
         + "PurchaseOrder.DeliveryTerm, PurchaseOrder.DeliveryDays, PurchaseOrder.DeliveryStartDate, "
         + "PurchaseOrder.RemarksHeader, PurchaseOrder.OrderStatus, pod.ItemCount, pod.ItemQuantity, pod.ItemWeight, "
         + "Dut.TermsDescription, Usr.FirstName + ' ' + Usr.LastName as UserName, PurchaseOrder.CityId, c.Description as CityName, "
         + "PurchaseOrder.OrderCatagoryId, o.OrderCategoryDescription as OrderCategoryDescription, PurchaseOrder.BrokerAgentSupCustId, sc.CompanyName as PurchaserName, "
         + "(SELECT COUNT(Attachment) FROM dbo.DMSAttachments WHERE RefDocumentTypeId = PurchaseOrder.DocumentTypeId AND RefDocumentNo = dbo.PurchaseOrder.Id) AS NoOfAttachments "
         + "FROM PurchaseOrder "
         + "INNER JOIN SupplierCustomer AS supcust ON PurchaseOrder.OrderSupCustId = supcust.Id "
         + "LEFT OUTER JOIN SupplierCustomer AS refparty ON PurchaseOrder.RefrenenceParty = refparty.Id "
         + "LEFT OUTER JOIN InvDueTerms Dut ON PurchaseOrder.PaymentTermsId = Dut.Id "
         + "INNER JOIN UserAccount Usr ON PurchaseOrder.EntryUser = Usr.ID "
         + "LEFT JOIN City c ON PurchaseOrder.CityId = c.Id "
         + "LEFT JOIN SupplierCustomer sc ON PurchaseOrder.BrokerAgentSupCustId = sc.Id "
         + "LEFT JOIN InvOrderCategory o ON PurchaseOrder.OrderCatagoryId = o.Id "
         + "INNER JOIN (SELECT PurchaseOrderId, COUNT(OrderItemId) ItemCount, Isnull(SUM(OrderItemQty), 0) ItemQuantity, Isnull(SUM(NetWeight), 0) ItemWeight "
         + "   FROM PurchaseOrderDetail GROUP BY PurchaseOrderId) pod ON PurchaseOrder.Id = pod.PurchaseOrderId "
         + "WHERE PurchaseOrder.OrganizationId = :organizationId AND PurchaseOrder.CompanyId = :companyId "
         + "AND PurchaseOrder.DocumentTypeId = :documentTypeId "
         + "AND (:orderSupCustId IS NULL OR PurchaseOrder.OrderSupCustId = :orderSupCustId) "
         + "AND (:docDateFrom IS NULL OR PurchaseOrder.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR PurchaseOrder.DocDate <= :docDateTo) "
         + "AND (:fromDocNo IS NULL OR PurchaseOrder.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR PurchaseOrder.DocNo <= :toDocNo) "
         + "AND PurchaseOrder.OrderCatagoryId = 7 "
         + "AND (:orderCategoryId IS NULL OR PurchaseOrder.OrderCatagoryId = :orderCategoryId) "
         + ") a WHERE (:noOfRecords IS NULL OR a.RecordNo <= :noOfRecords) "
         + "ORDER BY a.DocNo DESC",
         nativeQuery = true)
    List<MarketPurchaseOrderFormHistoryRow> marketPurchaseOrderFormHistory(@Param("organizationId") Integer organizationId,
                                                                           @Param("companyId") Integer companyId,
                                                                           @Param("documentTypeId") Integer documentTypeId,
                                                                           @Param("orderSupCustId") Integer orderSupCustId,
                                                                           @Param("docDateFrom") LocalDate docDateFrom,
                                                                           @Param("docDateTo") LocalDate docDateTo,
                                                                           @Param("fromDocNo") Integer fromDocNo,
                                                                           @Param("toDocNo") Integer toDocNo,
                                                                           @Param("orderCategoryId") Integer orderCategoryId,
                                                                           @Param("noOfRecords") Long noOfRecords);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetSupplierRefNo' (single column SupplierRefNo). */
    @Query("SELECT COALESCE(p.supplierRefNo, '') FROM PurchaseOrder p "
         + "WHERE p.organizationId = :organizationId AND p.companyId = :companyId AND p.orderCatagoryId = :orderCategoryId "
         + "AND p.documentTypeId = :documentTypeId AND p.supplierRefNo <> ''")
    List<String> getSupplierRefNo(@Param("organizationId") Integer organizationId,
                                  @Param("companyId") Integer companyId,
                                  @Param("orderCategoryId") Integer orderCategoryId,
                                  @Param("documentTypeId") Integer documentTypeId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderEmptyBagsDetailByOrderId' (native: PurchaseOrderEmptyBags has no entity; fnSplitString). ids = comma separated PurchaseOrder ids. */
    @Query(value = "SELECT *, CASE h.Type WHEN 1 THEN 'Normal' WHEN 2 THEN 'Purchase Against Weight' WHEN 3 THEN 'Free of Cost' "
         + "WHEN 4 THEN 'Retained' WHEN 5 THEN 'Returned' ELSE '' END EmptyBagsType "
         + "FROM PurchaseOrderEmptyBags h "
         + "WHERE (:id IS NULL OR h.PurchaseOrderId = :id) "
         + "AND (:ids IS NULL OR h.PurchaseOrderId IN (SELECT data FROM fnSplitString(:ids, ',')))",
         nativeQuery = true)
    List<PurchaseOrderEmptyBagsByOrderRow> getPurchaseOrderEmptyBagsDetailByOrderId(@Param("id") Integer id,
                                                                                    @Param("ids") String ids);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderInformationForGatepassInward'.
     * Native: config 122 (dbo.GetConfigurationByConfigId), ST.PurchaseOrder* (steel) tables, vItemAllocation, V_UomScheduleAndUom,
     * fnSplitString. #TmpResult/#TmpGatePassData/#Data pipeline rewritten as UNION ALL of the two sources; the gate-pass
     * aggregate is applied with OUTER APPLY TOP 1 (same "first match" semantics as the proc's UPDATE ... FROM) and rows keep
     * the temp-table defaults (0) when no gate pass matched; only rows with BalWeight > 0 are returned (as in the proc).
     * ids = comma separated DocumentTypeIds.
     */
    @Query(value = "SELECT R.DocumentTypeId, R.DocumentTypeDescription, R.OrderNo, R.SupplierName, R.ItemName, R.PackUom, "
         + "CAST(R.OrderQty AS DECIMAL(18, 2)) AS OrderQty, CAST(R.OrderWeight AS DECIMAL(18, 2)) AS OrderWeight, "
         + "CAST(R.ReceivedQty AS DECIMAL(18, 2)) AS ReceivedQty, CAST(R.ReceivedWeight AS DECIMAL(18, 2)) AS ReceivedWeight, "
         + "CAST(R.BalQty AS DECIMAL(18, 2)) AS BalQty, CAST(R.BalWeight AS DECIMAL(18, 2)) AS BalWeight, R.RemarksHeader, R.ApprovedStatus "
         + "FROM ( "
         + "SELECT h.DocumentTypeId, dt.DocumentTypeDescription, h.DocNo AS OrderNo, sp.CompanyName AS SupplierName, i.ItemName, Uom.UOMCode AS PackUom, "
         + "d.OrderItemQty AS OrderQty, d.NetWeight AS OrderWeight, "
         + "CASE WHEN G.Matched = 1 THEN G.GpQty ELSE 0 END AS ReceivedQty, "
         + "CASE WHEN G.Matched = 1 THEN G.FactoryWeight ELSE 0 END AS ReceivedWeight, "
         + "CASE WHEN G.Matched = 1 THEN d.OrderItemQty - G.GpQty ELSE 0 END AS BalQty, "
         + "CASE WHEN G.Matched = 1 THEN d.NetWeight - G.FactoryWeight ELSE 0 END AS BalWeight, "
         + "h.RemarksHeader, CASE WHEN h.IsAproved = 1 THEN 'Approved' ELSE 'Not Approved' END AS ApprovedStatus "
         + "FROM PurchaseOrderDetail d "
         + "INNER JOIN PurchaseOrder h ON d.PurchaseOrderId = h.Id "
         + "INNER JOIN SupplierCustomer sp ON h.OrderSupCustId = sp.Id "
         + "INNER JOIN Item i ON d.OrderItemId = i.Id "
         + "INNER JOIN DocumentType dt ON dt.Id = h.DocumentTypeId "
         + "INNER JOIN V_UomScheduleAndUom Uom ON d.OrderItemUOMId = Uom.Id "
         + "OUTER APPLY (SELECT TOP 1 1 AS Matched, SUM(GP.NoOfPackages) AS GpQty, SUM(GP.WeightComparedToPoWt) AS FactoryWeight "
         + "   FROM GatePassInward GP WHERE GP.PurchaseOrderId = h.Id "
         + "   AND ((ISNULL(CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 122) AS BIT), 0) = CAST(0 AS BIT) AND GP.Status <> 'Rejected') "
         + "     OR (ISNULL(CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 122) AS BIT), 0) = CAST(1 AS BIT))) "
         + "   AND GP.RefDocumentTypeId IN (41, 700) GROUP BY GP.RefDocumentTypeId) G "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
         + "AND (:financialYearId IS NULL OR h.FinancialYearId = :financialYearId) "
         + "AND h.OrderStatus = 'Open' AND h.IsAproved = 1 "
         + "AND h.DocumentTypeId IN (SELECT data FROM fnSplitString(:ids, ',')) "
         + "AND (:orderSupCustId IS NULL OR h.OrderSupCustId = :orderSupCustId) "
         + "AND (:docDateFrom IS NULL OR h.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR h.DocDate <= :docDateTo) "
         + "AND (:entryFromDate IS NULL OR CAST(h.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + "AND (:entryToDate IS NULL OR CAST(h.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + "AND (:modifyFromDate IS NULL OR CAST(h.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + "AND (:modifyToDate IS NULL OR CAST(h.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + "AND (:approvedFromDate IS NULL OR CAST(h.PostDate AS DATE) >= CAST(:approvedFromDate AS DATE)) "
         + "AND (:approvedToDate IS NULL OR CAST(h.PostDate AS DATE) <= CAST(:approvedToDate AS DATE)) "
         + "AND (:fromDocNo IS NULL OR h.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR h.DocNo <= :toDocNo) "
         + "UNION ALL "
         + "SELECT h.DocumentTypeId, dt.DocumentTypeDescription, h.DocNo AS OrderNo, sp.CompanyName AS SupplierName, i.ItemName, Uom.UOMCode AS PackUom, "
         + "d.OrderItemQty AS OrderQty, d.NetWeight AS OrderWeight, "
         + "CASE WHEN G.Matched = 1 THEN G.GpQty ELSE 0 END AS ReceivedQty, "
         + "CASE WHEN G.Matched = 1 THEN G.FactoryWeight ELSE 0 END AS ReceivedWeight, "
         + "CASE WHEN G.Matched = 1 THEN d.OrderItemQty - G.GpQty ELSE 0 END AS BalQty, "
         + "CASE WHEN G.Matched = 1 THEN d.NetWeight - G.FactoryWeight ELSE 0 END AS BalWeight, "
         + "h.RemarksHeader, CASE WHEN h.IsApproved = 1 THEN 'Approved' ELSE 'Not Approved' END AS ApprovedStatus "
         + "FROM ST.PurchaseOrderDetail d "
         + "INNER JOIN ST.PurchaseOrder h ON d.PurchaseOrderId = h.Id "
         + "INNER JOIN SupplierCustomer sp ON h.OrderSupplierId = sp.Id "
         + "INNER JOIN vItemAllocation i ON d.OrderItemId = i.Id "
         + "INNER JOIN DocumentType dt ON dt.Id = h.DocumentTypeId "
         + "INNER JOIN V_UomScheduleAndUom Uom ON d.OrderItemUOMId = Uom.Id "
         + "OUTER APPLY (SELECT TOP 1 1 AS Matched, SUM(GP.NoOfPackages) AS GpQty, SUM(GP.WeightComparedToPoWt) AS FactoryWeight "
         + "   FROM GatePassInward GP WHERE GP.PurchaseOrderId = h.Id "
         + "   AND ((ISNULL(CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 122) AS BIT), 0) = CAST(0 AS BIT) AND GP.Status <> 'Rejected') "
         + "     OR (ISNULL(CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 122) AS BIT), 0) = CAST(1 AS BIT))) "
         + "   AND GP.RefDocumentTypeId = 1500 GROUP BY GP.RefDocumentTypeId) G "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
         + "AND (:financialYearId IS NULL OR h.FinancialYearId = :financialYearId) "
         + "AND h.OrderStatus = 'Open' AND h.IsApproved = 1 "
         + "AND h.DocumentTypeId IN (SELECT data FROM fnSplitString(:ids, ',')) "
         + "AND (:orderSupCustId IS NULL OR h.OrderSupplierId = :orderSupCustId) "
         + "AND (:docDateFrom IS NULL OR h.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR h.DocDate <= :docDateTo) "
         + "AND (:entryFromDate IS NULL OR CAST(h.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + "AND (:entryToDate IS NULL OR CAST(h.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + "AND (:modifyFromDate IS NULL OR CAST(h.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + "AND (:modifyToDate IS NULL OR CAST(h.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + "AND (:approvedFromDate IS NULL OR CAST(h.ApprovedDate AS DATE) >= CAST(:approvedFromDate AS DATE)) "
         + "AND (:approvedToDate IS NULL OR CAST(h.ApprovedDate AS DATE) <= CAST(:approvedToDate AS DATE)) "
         + "AND (:fromDocNo IS NULL OR h.DocNo >= :fromDocNo) "
         + "AND (:toDocNo IS NULL OR h.DocNo <= :toDocNo) "
         + ") R WHERE R.BalWeight > 0",
         nativeQuery = true)
    List<PurchaseOrderInfoForGatepassInwardRow> getPurchaseOrderInformationForGatepassInward(@Param("organizationId") Integer organizationId,
                                                                                             @Param("companyId") Integer companyId,
                                                                                             @Param("financialYearId") Integer financialYearId,
                                                                                             @Param("ids") String ids,
                                                                                             @Param("orderSupCustId") Integer orderSupCustId,
                                                                                             @Param("docDateFrom") LocalDate docDateFrom,
                                                                                             @Param("docDateTo") LocalDate docDateTo,
                                                                                             @Param("entryFromDate") LocalDate entryFromDate,
                                                                                             @Param("entryToDate") LocalDate entryToDate,
                                                                                             @Param("modifyFromDate") LocalDate modifyFromDate,
                                                                                             @Param("modifyToDate") LocalDate modifyToDate,
                                                                                             @Param("approvedFromDate") LocalDate approvedFromDate,
                                                                                             @Param("approvedToDate") LocalDate approvedToDate,
                                                                                             @Param("fromDocNo") Integer fromDocNo,
                                                                                             @Param("toDocNo") Integer toDocNo);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='GetOrdersBySupplierId' */
    @Query("SELECT h.id AS id, h.docNo AS docNo, h.docDate AS docDate, h.orderSupCustId AS orderSupCustId FROM PurchaseOrder h "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.orderSupCustId = :orderSupCustId "
         + "AND h.documentTypeId = :documentTypeId "
         + "AND (:orderStatus IS NULL OR h.orderStatus = :orderStatus) "
         + "AND (:isApproved IS NULL OR h.isAproved = :isApproved) "
         + "AND (:financialYearId IS NULL OR h.financialYearId = :financialYearId)")
    List<PurchaseOrderBySupplierRow> getOrdersBySupplierId(@Param("organizationId") Integer organizationId,
                                                           @Param("companyId") Integer companyId,
                                                           @Param("orderSupCustId") Integer orderSupCustId,
                                                           @Param("documentTypeId") Integer documentTypeId,
                                                           @Param("orderStatus") String orderStatus,
                                                           @Param("isApproved") Boolean isApproved,
                                                           @Param("financialYearId") Integer financialYearId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory_HeaderData' (native: ROW_NUMBER, GETDATE). canViewAllRecord: bit. */
    @Query(value = "SELECT * FROM ( "
         + "SELECT ROW_NUMBER() OVER (ORDER BY h.DocDate DESC) RecordNo, h.Id OrderId, h.DocNo, h.DocDate, "
         + "Supplier.CompanyName Supplier_Customer_Name, h.IsAproved, h.OrderStatus, "
         + "CASE WHEN CONVERT(DATE, h.DocDate) < CONVERT(DATE, GETDATE()) THEN 'EXPIRED' ELSE 'NOT-EXPIRED' END ExpiryStatus, "
         + "SUM(d.Amount) OrderAmount "
         + "FROM dbo.PurchaseOrder h "
         + "INNER JOIN dbo.SupplierCustomer Supplier ON Supplier.Id = h.OrderSupCustId "
         + "INNER JOIN dbo.PurchaseOrderDetail d ON d.PurchaseOrderId = h.Id "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND h.FinancialYearId = :financialYearId "
         + "AND h.DocumentTypeId = :documentTypeId "
         + "AND (:canViewAllRecord = 1 OR (:canViewAllRecord = 0 AND h.EntryUser = :entryUser)) "
         + "AND (:isApproved IS NULL OR ISNULL(h.IsAproved, 0) = :isApproved) "
         + "GROUP BY h.Id, h.DocNo, h.DocDate, Supplier.CompanyName, h.IsAproved, h.OrderStatus "
         + ") h WHERE (:noOfRecords IS NULL OR h.RecordNo <= :noOfRecords)",
         nativeQuery = true)
    List<PurchaseOrderModernHistoryHeaderDataRow> purchaseOrderModernHistory_HeaderData(@Param("organizationId") Integer organizationId,
                                                                                        @Param("companyId") Integer companyId,
                                                                                        @Param("financialYearId") Integer financialYearId,
                                                                                        @Param("documentTypeId") Integer documentTypeId,
                                                                                        @Param("canViewAllRecord") Boolean canViewAllRecord,
                                                                                        @Param("entryUser") Integer entryUser,
                                                                                        @Param("isApproved") Boolean isApproved,
                                                                                        @Param("noOfRecords") Long noOfRecords);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' — result set 1 (header, #PurchaseOrder).
     * Native: config 91 (attachment url), UserProfile / InvOrderCategory / InvDueTerms / AttachmentsHistory without entity.
     * The proc returns 4 result sets; see purchaseOrderModernHistoryDetail / ...EmptyBags / ...Expenses for sets 2-4.
     */
    @Query(value = "SELECT * FROM ( "
         + "SELECT ROW_NUMBER() OVER (ORDER BY h.DocDate DESC) RecordNo, h.Id OrderId, h.DocNo, h.DocDate, OrderCategory.OrderCategoryName, h.CatagorySrNo, "
         + "h.SupplierRefNo, h.RefrenenceParty, PaymentTerms.TermsDescription PaymentTerm, h.OrderDueDays, h.OrderDueDate, "
         + "Supplier.CompanyName Supplier_Customer_Name, Supplier.MobilePersonal Supplier_Customer_Mobile, Supplier.Email Supplier_Customer_Email, "
         + "Supplier.Address1 Supplier_Customer_Address, "
         + "h.OrderExpiryDate, h.DeliveryTerm, h.DeliveryStartDate, h.DeliveryDays, h.DeliveryRemarks, h.RemarksHeader, "
         + "h.IsAproved, h.OrderStatus, CASE WHEN CONVERT(DATE, h.DocDate) < CONVERT(DATE, GETDATE()) THEN 'EXPIRED' ELSE 'NOT-EXPIRED' END ExpiryStatus, "
         + "(SELECT COUNT(Id) FROM dbo.AttachmentsHistory WHERE ReferenceDocumentTypeId = h.DocumentTypeId AND ReferenceDocumentId = h.Id) AS AttachmentsCount, "
         + "h.BrokerAgentSupCustId CommissionAgentId, CommissionAgent.CompanyName CommissionAgentName, h.CommissionType, h.CommRate, h.UomScheduleIdCmRate, h.CommAmount, "
         + "h.BrokerAgentId, BrokerAgent.CompanyName BrokerAgentName, h.BrokeryType, h.BrokeryRate, h.BrokeryUom, h.BrokeryAmount, "
         + "EntryUser.FirstName + ' ' + EntryUser.LastName EntryUserName, h.EntryUser, h.EntryDate, "
         + "ModifyUser.FirstName + ' ' + ModifyUser.LastName ModifyUserName, h.ModifyUser, h.ModifyDate, "
         + "ApprovalUser.FirstName + ' ' + ApprovalUser.LastName ApprovalUserName, h.PostUser, h.PostDate, "
         + "CASE WHEN ISNULL(EntryUserProfile.ProfileImageFileName, '') = '' THEN CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) AS nvarchar(max)) + 'DummyProfilePicture.jpg' "
         + "   ELSE CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) AS nvarchar(max)) + EntryUserProfile.ProfileImageFileName END EntryUserProfileImage, "
         + "CASE WHEN ISNULL(ModifyUserProfile.ProfileImageFileName, '') = '' THEN CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) AS nvarchar(max)) + 'DummyProfilePicture.jpg' "
         + "   ELSE CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) AS nvarchar(max)) + ModifyUserProfile.ProfileImageFileName END ModifyUserProfileImage, "
         + "CASE WHEN ISNULL(ApprovalUserProfile.ProfileImageFileName, '') = '' THEN CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) AS nvarchar(max)) + 'DummyProfilePicture.jpg' "
         + "   ELSE CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) AS nvarchar(max)) + ApprovalUserProfile.ProfileImageFileName END ApprovalUserProfileImage, "
         + "h.OrganizationId, h.CompanyId, h.FinancialYearId "
         + "FROM dbo.PurchaseOrder h "
         + "INNER JOIN dbo.InvOrderCategory OrderCategory ON OrderCategory.Id = h.OrderCatagoryId "
         + "INNER JOIN SupplierCustomer Supplier ON Supplier.Id = h.OrderSupCustId "
         + "INNER JOIN InvDueTerms PaymentTerms ON PaymentTerms.Id = h.PaymentTermsId "
         + "INNER JOIN UserAccount EntryUser ON EntryUser.ID = h.EntryUser "
         + "LEFT JOIN SupplierCustomer CommissionAgent ON CommissionAgent.Id = h.BrokerAgentSupCustId "
         + "LEFT JOIN UserProfile EntryUserProfile ON EntryUserProfile.UserId = EntryUser.ID "
         + "LEFT JOIN UserAccount ModifyUser ON ModifyUser.ID = h.ModifyUser "
         + "LEFT JOIN UserProfile ModifyUserProfile ON ModifyUserProfile.UserId = ModifyUser.ID "
         + "LEFT JOIN UserAccount ApprovalUser ON ApprovalUser.ID = h.PostUser "
         + "LEFT JOIN UserProfile ApprovalUserProfile ON ApprovalUserProfile.UserId = ApprovalUser.ID "
         + "LEFT JOIN SupplierCustomer BrokerAgent ON BrokerAgent.Id = h.BrokerAgentId "
         + "WHERE h.Id = :id "
         + ") h",
         nativeQuery = true)
    List<PurchaseOrderModernHistoryRow> purchaseOrderModernHistory(@Param("id") Integer id,
                                                                   @Param("organizationId") Integer organizationId,
                                                                   @Param("companyId") Integer companyId);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' — result set 2 (details). Native: vItemAllocation, V_UomScheduleAndUom, InvLabSampleAnalysisHeader. */
    @Query(value = "SELECT d.Id, h.Id OrderId, i.ItemName, PackUom.UOMCode PackUom, d.OrderItemQty, d.NetWeight, d.OrderItemRate, "
         + "RateUom.UOMCode RateUom, d.Amount, Job_Lot.JobLotDescription, d.CityArea, d.Crop, d.BagWeight, d.BagPrice, "
         + "d.Moisture, Analysis.Id LabSampleId, Analysis.DocNo SampleAnalysisNo, d.OrderRemarks "
         + "FROM dbo.PurchaseOrderDetail d "
         + "INNER JOIN dbo.PurchaseOrder h ON h.Id = d.PurchaseOrderId AND h.Id = :id "
         + "INNER JOIN vItemAllocation i ON i.Id = d.OrderItemId AND i.CompanyId = h.CompanyId "
         + "INNER JOIN V_UomScheduleAndUom PackUom ON PackUom.Id = d.OrderItemUOMId "
         + "INNER JOIN V_UomScheduleAndUom RateUom ON RateUom.Id = d.OrderItemUOMId "
         + "INNER JOIN JobLot Job_Lot ON Job_Lot.Id = d.JobLotId "
         + "LEFT JOIN InvLabSampleAnalysisHeader Analysis ON Analysis.Id = d.InvLabSampleAnalysisHeaderId",
         nativeQuery = true)
    List<PurchaseOrderModernHistoryDetailRow> purchaseOrderModernHistoryDetail(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' — result set 3 (empty bags). Native: PurchaseOrderEmptyBags has no entity. */
    @Query(value = "SELECT e.Id, h.Id OrderId, "
         + "CASE e.Type WHEN 1 THEN 'Normal' WHEN 2 THEN 'Purchase Against Weight' WHEN 3 THEN 'Free of Cost' "
         + "WHEN 4 THEN 'Retained' WHEN 5 THEN 'Returned' ELSE '' END EmptyBagsType, "
         + "i.OtherItemName, p.PackTypeDesc, e.Rate, e.WeightCut "
         + "FROM dbo.PurchaseOrderEmptyBags e "
         + "INNER JOIN dbo.PurchaseOrder h ON e.PurchaseOrderId = h.Id AND h.Id = :id "
         + "LEFT JOIN InventoryItemsOther i ON i.Id = e.ItemId "
         + "LEFT JOIN InvPackingType p ON p.Id = e.PackingTypeId",
         nativeQuery = true)
    List<PurchaseOrderModernHistoryEmptyBagsRow> purchaseOrderModernHistoryEmptyBags(@Param("id") Integer id);

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' — result set 4 (expenses charge to product). Native: PurchaseOrderExpensesChargeToProduct has no entity. */
    @Query(value = "SELECT c.Id, h.Id OrderId, Account.AccountTitle, c.Percentage, c.Qty, c.Rate, c.SupplierCustomerId, "
         + "sp.CompanyName SupplierCustomerName, c.Amount, c.Remarks "
         + "FROM PurchaseOrderExpensesChargeToProduct c "
         + "INNER JOIN dbo.PurchaseOrder h ON c.PurchaseOrderId = h.Id AND h.Id = :id "
         + "LEFT JOIN SupplierCustomer sp ON sp.Id = c.SupplierCustomerId "
         + "LEFT JOIN ChartofAccount Account ON Account.Id = c.AccountId",
         nativeQuery = true)
    List<PurchaseOrderModernHistoryExpenseRow> purchaseOrderModernHistoryExpenses(@Param("id") Integer id);

    /**
     * Sp_PurchaseOrder_GetAllMethod @Activity='PoWeightAndGpWeightValidation'.
     * // KEEP-EXEC: validation only (no result set) — raises 'Record cannot be Updated beacuase OrderWeight cannot be lesser
     * then GPI Received Weight...' when config 36 is on and GP weight > order weight, and the lab purchase analysis item
     * mismatch error. The scalar checks are also exposed below (isPoWeightValidationEnabled, sumGatePassWeightForOrder,
     * sumOrderNetWeight, PurchaseOrderDetailJpaRepository.countLabAnalysisItemMismatch).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id = :id, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@Activity = 'PoWeightAndGpWeightValidation'", nativeQuery = true)
    void poWeightAndGpWeightValidation(@Param("id") Integer id,
                                       @Param("organizationId") Integer organizationId,
                                       @Param("companyId") Integer companyId);

    /** PoWeightAndGpWeightValidation: SELECT CAST(ISNULL(ConfigKey,0) AS BIT) FROM ConfigrationsAllocation WHERE ConfigrationsDefinitionId = 36 AND CompanyId = @CompanyId (take first). */
    @Query(value = "SELECT CAST(ISNULL(ConfigKey, 0) AS BIT) FROM ConfigrationsAllocation "
         + "WHERE ConfigrationsDefinitionId = 36 AND CompanyId = :companyId", nativeQuery = true)
    List<Boolean> isPoWeightValidationEnabled(@Param("companyId") Integer companyId);

    /** PoWeightAndGpWeightValidation: ISNULL(SUM(WeightComparedToPoWt),0) of GatePassInward with RefDocumentTypeId = 41 for the order. */
    @Query("SELECT COALESCE(SUM(g.weightComparedToPoWt), 0) FROM InwardGatePass g "
         + "WHERE g.organizationId = :organizationId AND g.companyId = :companyId AND g.refDocumentTypeId = 41 AND g.purchaseOrderId = :id")
    Double sumGatePassWeightForOrder(@Param("organizationId") Integer organizationId,
                                     @Param("companyId") Integer companyId,
                                     @Param("id") Integer id);

    /** PoWeightAndGpWeightValidation: ISNULL(SUM(NetWeight),0) of PurchaseOrderDetail for the order. */
    @Query("SELECT COALESCE(SUM(d.netWeight), 0) FROM PurchaseOrderDetail d JOIN PurchaseOrder h ON d.purchaseOrderId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND d.purchaseOrderId = :id")
    Double sumOrderNetWeight(@Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId,
                             @Param("id") Integer id);

    // =============================================================================================
    // Sp_PurchaseOrder_Update
    // =============================================================================================

    /**
     * Sp_PurchaseOrder_Update (single branch).
     * // KEEP-EXEC: financial-year validation (dbo.FunCheckValidFinancialYear, FinancialYear table), approved-record
     * config 57, commission/brokery 1.5 percent checks, GRN / invoice reference checks, DocDate vs DeliveryStartDate,
     * OrderExpiryDate = DATEADD(DAY,@DeliveryDays,@DeliveryStartDate), UserLogId fallback from UserAccount.LastUserLogId,
     * deletes of 7 child tables and USP_UserAudit_Insert.
     * Defaults: every parameter NULL; LocationTypeId null/0 → 1; OrderCatagoryId = 8 → DeliveryTerm 'Load', DeliveryTermId 1.
     * EntryDate/EntryUser/PostDate/PostUser/PostState/BranchSrNo are accepted but not written by the proc.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_PurchaseOrder_Update @BranchesId = :branchesId, @BrokerAgentSupCustId = :brokerAgentSupCustId, "
         + "@CatagorySrNo = :catagorySrNo, @CommAmount = :commAmount, @CommissionRemarks = :commissionRemarks, "
         + "@CommissionType = :commissionType, @CommRate = :commRate, @CompanyId = :companyId, @DeliveryDays = :deliveryDays, "
         + "@DeliveryRemarks = :deliveryRemarks, @DeliveryStartDate = :deliveryStartDate, @DeliveryTerm = :deliveryTerm, "
         + "@DocDate = :docDate, @DocNo = :docNo, @BranchSrNo = :branchSrNo, @DocumentTypeId = :documentTypeId, "
         + "@EntryDate = :entryDate, @EntryUser = :entryUser, @Id = :id, @IsAproved = :isAproved, @ModifyDate = :modifyDate, "
         + "@ModifyUser = :modifyUser, @OrderCatagoryId = :orderCatagoryId, @OrderDueDate = :orderDueDate, "
         + "@OrderDueDays = :orderDueDays, @OrderExpiryDate = :orderExpiryDate, @OrderStatus = :orderStatus, "
         + "@OrderSupCustId = :orderSupCustId, @OrderTaxable = :orderTaxable, @OrderType = :orderType, "
         + "@OrganizationId = :organizationId, @PaymentTermsId = :paymentTermsId, @PostDate = :postDate, @PostState = :postState, "
         + "@PostUser = :postUser, @ProjectsId = :projectsId, @RefrenenceParty = :refrenenceParty, @RemarksHeader = :remarksHeader, "
         + "@SupplierCustomerIdStockParty = :supplierCustomerIdStockParty, @SupplierRefNo = :supplierRefNo, "
         + "@UomScheduleIdCmRate = :uomScheduleIdCmRate, @FinancialYearId = :financialYearId, @CityId = :cityId, "
         + "@BrokerAgentId = :brokerAgentId, @BrokeryType = :brokeryType, @BrokeryRate = :brokeryRate, @BrokeryUom = :brokeryUom, "
         + "@BrokeryAmount = :brokeryAmount, @CashFreight = :cashFreight, @CreditFreight = :creditFreight, "
         + "@BillCalculateTypeId = :billCalculateTypeId, @OrderQty = :orderQty, @OrderWeight = :orderWeight, "
         + "@OrderAmount = :orderAmount, @CurrencyId = :currencyId, @ExchangeRate = :exchangeRate, @FcyAmount = :fcyAmount, "
         + "@OrderStatusRemarks = :orderStatusRemarks, @BaseDocumentTypeId = :baseDocumentTypeId, "
         + "@AttachmentsValues = :attachmentsValues, @CustomAttachmentsValues = :customAttachmentsValues, "
         + "@DeliveryTermId = :deliveryTermId, @LocationTypeId = :locationTypeId, @BookingPersonId = :bookingPersonId, "
         + "@PermitNo = :permitNo, @PermitIssueDate = :permitIssueDate, @PermitValidityUpto = :permitValidityUpto, "
         + "@UserLogId = :userLogId",
         nativeQuery = true)
    void spPurchaseOrderUpdate(@Param("branchesId") Integer branchesId,
                               @Param("brokerAgentSupCustId") Integer brokerAgentSupCustId,
                               @Param("catagorySrNo") Integer catagorySrNo,
                               @Param("commAmount") Double commAmount,
                               @Param("commissionRemarks") String commissionRemarks,
                               @Param("commissionType") String commissionType,
                               @Param("commRate") Double commRate,
                               @Param("companyId") Integer companyId,
                               @Param("deliveryDays") Integer deliveryDays,
                               @Param("deliveryRemarks") String deliveryRemarks,
                               @Param("deliveryStartDate") LocalDateTime deliveryStartDate,
                               @Param("deliveryTerm") String deliveryTerm,
                               @Param("docDate") LocalDateTime docDate,
                               @Param("docNo") Integer docNo,
                               @Param("branchSrNo") Integer branchSrNo,
                               @Param("documentTypeId") Integer documentTypeId,
                               @Param("entryDate") LocalDateTime entryDate,
                               @Param("entryUser") Integer entryUser,
                               @Param("id") Integer id,
                               @Param("isAproved") Boolean isAproved,
                               @Param("modifyDate") LocalDateTime modifyDate,
                               @Param("modifyUser") Integer modifyUser,
                               @Param("orderCatagoryId") Integer orderCatagoryId,
                               @Param("orderDueDate") LocalDateTime orderDueDate,
                               @Param("orderDueDays") Integer orderDueDays,
                               @Param("orderExpiryDate") LocalDateTime orderExpiryDate,
                               @Param("orderStatus") String orderStatus,
                               @Param("orderSupCustId") Integer orderSupCustId,
                               @Param("orderTaxable") Boolean orderTaxable,
                               @Param("orderType") String orderType,
                               @Param("organizationId") Integer organizationId,
                               @Param("paymentTermsId") Integer paymentTermsId,
                               @Param("postDate") LocalDateTime postDate,
                               @Param("postState") Boolean postState,
                               @Param("postUser") Integer postUser,
                               @Param("projectsId") Integer projectsId,
                               @Param("refrenenceParty") Integer refrenenceParty,
                               @Param("remarksHeader") String remarksHeader,
                               @Param("supplierCustomerIdStockParty") Integer supplierCustomerIdStockParty,
                               @Param("supplierRefNo") String supplierRefNo,
                               @Param("uomScheduleIdCmRate") Integer uomScheduleIdCmRate,
                               @Param("financialYearId") Integer financialYearId,
                               @Param("cityId") Integer cityId,
                               @Param("brokerAgentId") Integer brokerAgentId,
                               @Param("brokeryType") String brokeryType,
                               @Param("brokeryRate") Double brokeryRate,
                               @Param("brokeryUom") Double brokeryUom,
                               @Param("brokeryAmount") Double brokeryAmount,
                               @Param("cashFreight") Boolean cashFreight,
                               @Param("creditFreight") Boolean creditFreight,
                               @Param("billCalculateTypeId") Integer billCalculateTypeId,
                               @Param("orderQty") Double orderQty,
                               @Param("orderWeight") Double orderWeight,
                               @Param("orderAmount") Double orderAmount,
                               @Param("currencyId") Integer currencyId,
                               @Param("exchangeRate") Double exchangeRate,
                               @Param("fcyAmount") Double fcyAmount,
                               @Param("orderStatusRemarks") String orderStatusRemarks,
                               @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                               @Param("attachmentsValues") String attachmentsValues,
                               @Param("customAttachmentsValues") String customAttachmentsValues,
                               @Param("deliveryTermId") Integer deliveryTermId,
                               @Param("locationTypeId") Integer locationTypeId,
                               @Param("bookingPersonId") Integer bookingPersonId,
                               @Param("permitNo") String permitNo,
                               @Param("permitIssueDate") LocalDateTime permitIssueDate,
                               @Param("permitValidityUpto") LocalDateTime permitValidityUpto,
                               @Param("userLogId") Long userLogId);

    /**
     * Sp_PurchaseOrder_Update: the standalone UPDATE PurchaseOrder SET ... WHERE Id = @Id (every SET column of the proc;
     * no validations, no child deletes, no audit). Caller supplies the proc-derived values: orderExpiryDate =
     * deliveryStartDate + deliveryDays, locationTypeId default 1, DeliveryTerm/DeliveryTermId for OrderCatagoryId 8,
     * userLogId fallback (see findLastUserLogIdByUserId).
     */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.documentTypeId = :documentTypeId, p.docNo = :docNo, p.docDate = :docDate, "
         + "p.orderCatagoryId = :orderCatagoryId, p.catagorySrNo = :catagorySrNo, p.orderSupCustId = :orderSupCustId, "
         + "p.supplierRefNo = :supplierRefNo, p.refrenenceParty = :refrenenceParty, "
         + "p.supplierCustomerIdStockParty = :supplierCustomerIdStockParty, p.brokerAgentSupCustId = :brokerAgentSupCustId, "
         + "p.commissionType = :commissionType, p.commRate = :commRate, p.uomScheduleIdCmRate = :uomScheduleIdCmRate, "
         + "p.commAmount = :commAmount, p.commissionRemarks = :commissionRemarks, p.paymentTermsId = :paymentTermsId, "
         + "p.orderDueDays = :orderDueDays, p.orderDueDate = :orderDueDate, p.orderExpiryDate = :orderExpiryDate, "
         + "p.deliveryTerm = :deliveryTerm, p.deliveryStartDate = :deliveryStartDate, p.deliveryDays = :deliveryDays, "
         + "p.deliveryRemarks = :deliveryRemarks, p.orderTaxable = :orderTaxable, p.remarksHeader = :remarksHeader, "
         + "p.orderStatus = :orderStatus, p.isAproved = :isAproved, p.modifyDate = :modifyDate, p.modifyUser = :modifyUser, "
         + "p.organizationId = :organizationId, p.companyId = :companyId, p.branchesId = :branchesId, p.projectsId = :projectsId, "
         + "p.orderType = :orderType, p.financialYearId = :financialYearId, p.cityId = :cityId, p.brokerAgentId = :brokerAgentId, "
         + "p.brokeryType = :brokeryType, p.brokeryRate = :brokeryRate, p.brokeryUom = :brokeryUom, p.brokeryAmount = :brokeryAmount, "
         + "p.cashFreight = :cashFreight, p.creditFreight = :creditFreight, p.billCalculateTypeId = :billCalculateTypeId, "
         + "p.orderQty = :orderQty, p.orderWeight = :orderWeight, p.orderAmount = :orderAmount, p.currencyId = :currencyId, "
         + "p.exchangeRate = :exchangeRate, p.fcyAmount = :fcyAmount, p.orderStatusRemarks = :orderStatusRemarks, "
         + "p.baseDocumentTypeId = :baseDocumentTypeId, p.attachmentsValues = :attachmentsValues, "
         + "p.customAttachmentsValues = :customAttachmentsValues, p.deliveryTermId = :deliveryTermId, "
         + "p.locationTypeId = :locationTypeId, p.bookingPersonId = :bookingPersonId, p.permitNo = :permitNo, "
         + "p.permitIssueDate = :permitIssueDate, p.permitValidityUpto = :permitValidityUpto, p.userLogId = :userLogId "
         + "WHERE p.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("documentTypeId") Integer documentTypeId,
                   @Param("docNo") Integer docNo,
                   @Param("docDate") Date docDate,
                   @Param("orderCatagoryId") Integer orderCatagoryId,
                   @Param("catagorySrNo") Integer catagorySrNo,
                   @Param("orderSupCustId") Integer orderSupCustId,
                   @Param("supplierRefNo") String supplierRefNo,
                   @Param("refrenenceParty") Integer refrenenceParty,
                   @Param("supplierCustomerIdStockParty") Integer supplierCustomerIdStockParty,
                   @Param("brokerAgentSupCustId") Integer brokerAgentSupCustId,
                   @Param("commissionType") String commissionType,
                   @Param("commRate") Double commRate,
                   @Param("uomScheduleIdCmRate") Integer uomScheduleIdCmRate,
                   @Param("commAmount") Double commAmount,
                   @Param("commissionRemarks") String commissionRemarks,
                   @Param("paymentTermsId") Integer paymentTermsId,
                   @Param("orderDueDays") Integer orderDueDays,
                   @Param("orderDueDate") LocalDateTime orderDueDate,
                   @Param("orderExpiryDate") LocalDate orderExpiryDate,
                   @Param("deliveryTerm") String deliveryTerm,
                   @Param("deliveryStartDate") LocalDateTime deliveryStartDate,
                   @Param("deliveryDays") Integer deliveryDays,
                   @Param("deliveryRemarks") String deliveryRemarks,
                   @Param("orderTaxable") Boolean orderTaxable,
                   @Param("remarksHeader") String remarksHeader,
                   @Param("orderStatus") String orderStatus,
                   @Param("isAproved") Boolean isAproved,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("branchesId") Integer branchesId,
                   @Param("projectsId") Integer projectsId,
                   @Param("orderType") String orderType,
                   @Param("financialYearId") Integer financialYearId,
                   @Param("cityId") Integer cityId,
                   @Param("brokerAgentId") Integer brokerAgentId,
                   @Param("brokeryType") String brokeryType,
                   @Param("brokeryRate") Double brokeryRate,
                   @Param("brokeryUom") Double brokeryUom,
                   @Param("brokeryAmount") Double brokeryAmount,
                   @Param("cashFreight") Boolean cashFreight,
                   @Param("creditFreight") Boolean creditFreight,
                   @Param("billCalculateTypeId") Integer billCalculateTypeId,
                   @Param("orderQty") Double orderQty,
                   @Param("orderWeight") Double orderWeight,
                   @Param("orderAmount") Double orderAmount,
                   @Param("currencyId") Integer currencyId,
                   @Param("exchangeRate") Double exchangeRate,
                   @Param("fcyAmount") Double fcyAmount,
                   @Param("orderStatusRemarks") String orderStatusRemarks,
                   @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                   @Param("attachmentsValues") String attachmentsValues,
                   @Param("customAttachmentsValues") String customAttachmentsValues,
                   @Param("deliveryTermId") Integer deliveryTermId,
                   @Param("locationTypeId") Integer locationTypeId,
                   @Param("bookingPersonId") Integer bookingPersonId,
                   @Param("permitNo") String permitNo,
                   @Param("permitIssueDate") LocalDateTime permitIssueDate,
                   @Param("permitValidityUpto") LocalDateTime permitValidityUpto,
                   @Param("userLogId") Long userLogId);

    /** Sp_PurchaseOrder_Update: SELECT LastUserLogId FROM UserAccount WHERE ID = @ModifyUser (UserLogId fallback when null/0; take first). */
    @Query(value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :userId", nativeQuery = true)
    List<Long> findLastUserLogIdByUserId(@Param("userId") Integer userId);

    /** Sp_PurchaseOrder_Update: GRN reference check (count > 0 → 'Record cannot be updated beacuase record has been refered in Grn'). */
    @Query("SELECT COUNT(grd.id) FROM InvGrn grh JOIN InvGrnDetail grd ON grh.id = grd.invGrnId "
         + "WHERE grh.organizationId = :organizationId AND grh.companyId = :companyId AND grd.purchaseOrderId = :id")
    Long countGrnReferences(@Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId,
                            @Param("id") Integer id);

    /** Sp_PurchaseOrder_Update: Purchase Invoice reference check (count > 0 → 'Record cannot be updated beacuase record has been refered in Invoice'). */
    @Query("SELECT COUNT(pid.id) FROM InvPurchaseInvoice pih JOIN InvPurchaseInvoiceDetail pid ON pih.id = pid.invPurchaseInvoiceId "
         + "WHERE pih.organizationId = :organizationId AND pih.companyId = :companyId AND pid.purchaseOrderId = :id")
    Long countInvoiceReferences(@Param("organizationId") Integer organizationId,
                                @Param("companyId") Integer companyId,
                                @Param("id") Integer id);

    /** Sp_PurchaseOrder_Update: DELETE PurchaseOrderEmptyBags WHERE PurchaseOrderId = @Id (no entity → native). */
    @Modifying
    @Transactional
    @Query(value = "DELETE PurchaseOrderEmptyBags WHERE PurchaseOrderId = :id", nativeQuery = true)
    int deleteEmptyBagsByPurchaseOrderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_Update: DELETE PurchaseOrderExpensesChargeToProduct WHERE PurchaseOrderId = @Id (no entity → native). */
    @Modifying
    @Transactional
    @Query(value = "DELETE PurchaseOrderExpensesChargeToProduct WHERE PurchaseOrderId = :id", nativeQuery = true)
    int deleteExpensesChargeToProductByPurchaseOrderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_Update: DELETE PurchaseOrderSupplierExpense WHERE PurchaseOrderId = @Id (no entity → native). */
    @Modifying
    @Transactional
    @Query(value = "DELETE PurchaseOrderSupplierExpense WHERE PurchaseOrderId = :id", nativeQuery = true)
    int deleteSupplierExpenseByPurchaseOrderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_Update: DELETE PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId = @Id (no entity → native). */
    @Modifying
    @Transactional
    @Query(value = "DELETE PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId = :id", nativeQuery = true)
    int deletePaymentTermsDetailByPurchaseOrderId(@Param("id") Integer id);

    /** Sp_PurchaseOrder_Update: DELETE AdvancePaymentAdjustment WHERE PurchaseOrderId = @Id (no entity → native). */
    @Modifying
    @Transactional
    @Query(value = "DELETE AdvancePaymentAdjustment WHERE PurchaseOrderId = :id", nativeQuery = true)
    int deleteAdvancePaymentAdjustmentByPurchaseOrderId(@Param("id") Integer id);
}
