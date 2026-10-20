package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.SaleOrder;
import com.mst.projections.SaleOrderCustomerExpenseByOrderIdsRow;
import com.mst.projections.SaleOrderCustomerExpenseRow;
import com.mst.projections.SaleOrderDeliveryTermGatepassRow;
import com.mst.projections.SaleOrderDetailByHeaderIdRow;
import com.mst.projections.SaleOrderDetailByOrderIdRow;
import com.mst.projections.SaleOrderDetailForBookingInvoiceRow;
import com.mst.projections.SaleOrderDetailForDeliveryScheduleRow;
import com.mst.projections.SaleOrderDetailForTraderProLoaderRow;
import com.mst.projections.SaleOrderDetailRecordRow;
import com.mst.projections.SaleOrderExtraItemDetailRow;
import com.mst.projections.SaleOrderForGoodsDispatchNotesRow;
import com.mst.projections.SaleOrderFormHistoryRow;
import com.mst.projections.SaleOrderIdDocNoRow;
import com.mst.projections.SaleOrderIdNoPaymentTermRow;
import com.mst.projections.SaleOrderIdNoRow;
import com.mst.projections.SaleOrderItemRow;
import com.mst.projections.SaleOrderModernHistoryDetailRow;
import com.mst.projections.SaleOrderModernHistoryHeaderDataRow;
import com.mst.projections.SaleOrderModernHistoryRow;
import com.mst.projections.SaleOrderPaymentTermDetailBySoIdsRow;
import com.mst.projections.SaleOrderPaymentTermDetailRow;
import com.mst.projections.SaleOrderReadAllRow;
import com.mst.projections.SaleOrderReadByIdRow;
import com.mst.projections.SaleOrderSupplierRow;
import com.mst.projections.SaleOrderTradingFormHistoryRow;

/**
 * Sp_SaleOrder_GetAllMethod - one method per @Activity branch (31 branches).
 * Most branches join SaleOrderDetail / InvDueTerms / V_UomScheduleAndUom / vItemAllocation which have no entity, so they are native.
 * The proc also evaluates dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,277) into @SaleOrderBranchWise on every call,
 * but that value is never used by any branch, so it is not reproduced here.
 */
@Repository
public interface SaleOrderRepository extends JpaRepository<SaleOrder, Integer> {

    /** Helper for app-assigned ids (no Insert proc in this batch). */
    @Query("select coalesce(max(e.id),0) from SaleOrder e")
    Integer maxId();

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSalesInfoByDate' (proc just returns SELECT 1). */
    @Query(value = "SELECT 1", nativeQuery = true)
    Integer getSalesInfoByDate();

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadBySaleOrderIDNOrderItemId' (native: SaleOrderDetail, UOMSchedule, InvDueTerms). */
    @Query(value = "SELECT saleOrderDetail.Id, saleOrderDetail.SaleOrderId, saleOrderDetail.OrderItemId, saleOrderDetail.OrderItemUOMId, Item.ItemName, item.ItemCodeNew, "
         + " UOMSchedule.Equivalent, saleOrderDetail.Crop, JobLot.JobLotDescription, saleOrderDetail.OrderItemQty, "
         + " saleOrderDetail.NetWeight, saleOrderDetail.OrderItemRate, UomRate.Equivalent as RateUOM, saleOrderDetail.OrderItemRateUOMId, saleOrderDetail.Amount, saleOrderDetail.JobLotId, "
         + " saleOrderDetail.CityArea, saleOrderDetail.LabSampleNo, saleOrderDetail.AmountCalcType, saleOrderDetail.TaxableStatus, saleOrderDetail.OrderRemarks, "
         + " saleOrderDetail.BagWeight, saleOrderDetail.BagPrice, s.DocumentTypeId, "
         + " saleOrderDetail.DiscountTypeId, "
         + " Case when saleOrderDetail.DiscountTypeId = 2 then 'Percent' when saleOrderDetail.DiscountTypeId = 1 then 'Flat' else '' End As DiscountType, "
         + " IDT.TermsDescription PaymentTerm, s.DocNo "
         + "FROM saleOrderDetail "
         + " INNER JOIN SaleOrder s on SaleOrderDetail.SaleOrderId = s.Id "
         + " INNER JOIN UOMSchedule UOMSchedule ON saleOrderDetail.OrderItemUOMId = UOMSchedule.Id "
         + " INNER JOIN Item ON saleOrderDetail.OrderItemId = Item.Id "
         + " INNER JOIN UOMSchedule UomRate on SaleOrderDetail.OrderItemRateUOMId = UomRate.Id "
         + " LEFT  JOIN JobLot ON JobLot.Id = saleOrderDetail.JobLotId "
         + " LEFT  JOIN InvDueTerms IDT on IDT.Id = s.PaymentTermsId "
         + "WHERE saleOrderDetail.SaleOrderId = :saleOrderId "
         + " and SaleOrderDetail.ActionTypeId <> 3",
           nativeQuery = true)
    List<SaleOrderDetailByOrderIdRow> readBySaleOrderIDNOrderItemId(@Param("saleOrderId") Integer saleOrderId);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderCustomerExpensesByHeaderId' (native: SaleOrderCustomerExpenses). */
    @Query(value = "SELECT R.[Id], R.[SaleOrderId], R.[InvRevExpItemId], R.[Qty], R.[Rate], R.[Amount], R.[Remarks], OT.OtherItemName "
         + "FROM [dbo].[SaleOrderCustomerExpenses] R "
         + " INNER JOIN InventoryItemsOther OT ON R.InvRevExpItemId = OT.Id "
         + "WHERE R.[SaleOrderId] = :id",
           nativeQuery = true)
    List<SaleOrderCustomerExpenseRow> saleOrderCustomerExpensesByHeaderId(@Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderCustomerExpensesByOrderIds' (native: fnSplitString; orderIds = comma separated SaleOrderDetail ids). */
    @Query(value = "SELECT R.[Id], R.[SaleOrderId], s.DocNo SaleOrder, R.[InvRevExpItemId], R.[Qty], R.[Rate], R.[Amount], R.[Remarks], OT.OtherItemName "
         + "FROM [dbo].[SaleOrderCustomerExpenses] R "
         + " INNER JOIN InventoryItemsOther OT ON R.InvRevExpItemId = OT.Id "
         + " left  join SaleOrder s on r.SaleOrderId = s.Id "
         + "WHERE (R.[SaleOrderId] in (Select SaleOrderId From SaleOrderDetail where Id in (select data from dbo.fnSplitString (:orderIds, ','))))",
           nativeQuery = true)
    List<SaleOrderCustomerExpenseByOrderIdsRow> saleOrderCustomerExpensesByOrderIds(@Param("orderIds") String orderIds);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSupplierBySaleOrderNo' */
    @Query("SELECT DISTINCT sp.id AS id, sp.companyName AS companyName, sp.supCustCode AS supCustCode, so.id AS soHId, "
         + "so.docNo AS docNo, so.docDate AS soHDocDate, so.documentTypeId AS documentTypeId "
         + "FROM SupplierCustomer sp INNER JOIN SaleOrder so ON sp.id = so.orderSupCustId "
         + "WHERE so.organizationId = :organizationId AND so.companyId = :companyId AND so.id = :id "
         + "AND so.documentTypeId = :documentTypeId AND so.isAproved = true")
    List<SaleOrderSupplierRow> getSupplierBySaleOrderNo(@Param("organizationId") Integer organizationId,
                                                        @Param("companyId") Integer companyId,
                                                        @Param("id") Integer id,
                                                        @Param("documentTypeId") Integer documentTypeId);

    /** Sp_SaleOrder_GetAllMethod @Activity='GenerateSaleOrderCodeByDocId' (native: config 321 UseSaleOrderBalanceInNextFinYear). */
    @Query(value = "SELECT ISNULL(Max(Convert(int,DocNo)),0)+1 DocNo "
         + "FROM dbo.SaleOrder "
         + "WHERE DocumentTypeId = :documentTypeId "
         + "AND OrganizationId = :organizationId "
         + "AND CompanyId = :companyId "
         + "AND ( (ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 321)),0) = 0 AND FinancialYearId = :financialYearId) "
         + "   OR (ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 321)),0) = 1) )",
           nativeQuery = true)
    Integer generateSaleOrderCodeByDocId(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("documentTypeId") Integer documentTypeId,
                                         @Param("financialYearId") Integer financialYearId);

    /** Sp_SaleOrder_GetAllMethod @Activity='GenerateSaleOrderBranchCodeByDocId' (native: config 321 UseSaleOrderBalanceInNextFinYear). */
    @Query(value = "SELECT ISNULL(Max(Convert(int,BranchSrNo)),0)+1 BranchSrNo "
         + "FROM dbo.SaleOrder "
         + "WHERE DocumentTypeId = :documentTypeId "
         + "AND OrganizationId = :organizationId "
         + "AND CompanyId = :companyId "
         + "and BranchesId = :branchesId "
         + "AND ( (ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 321)),0) = 0 AND FinancialYearId = :financialYearId) "
         + "   OR (ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 321)),0) = 1) )",
           nativeQuery = true)
    Integer generateSaleOrderBranchCodeByDocId(@Param("organizationId") Integer organizationId,
                                               @Param("companyId") Integer companyId,
                                               @Param("documentTypeId") Integer documentTypeId,
                                               @Param("branchesId") Integer branchesId,
                                               @Param("financialYearId") Integer financialYearId);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSaleIdByDocNo' */
    @Query("SELECT s.id FROM SaleOrder s WHERE s.docNo = :docNo AND s.documentTypeId = :documentTypeId "
         + "AND s.organizationId = :organizationId AND s.companyId = :companyId")
    List<Integer> getSaleIdByDocNo(@Param("docNo") Integer docNo,
                                   @Param("documentTypeId") Integer documentTypeId,
                                   @Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId);

    /**
     * Sp_SaleOrder_GetAllMethod @Activity='SaleOrderFormHistory'
     * (native: #TmpSaleOrderHistory / #tmpDms temp tables rewritten as CTEs; the NoOfAttachments UPDATE becomes ISNULL(dms.NoOfAttachments,0);
     * ROW_NUMBER, fnSplitString). Optional filters are NULL = no filter; canViewAllRecord=0 restricts to so.EntryUser = entryUser.
     */
    @Query(value = "WITH TmpSaleOrderHistory AS ( "
         + " SELECT ROW_NUMBER() over (order by so.DocDate desc) AS RecordNo, so.Id, so.DocNo, so.BranchSrNo, so.BranchesId, so.DocDate, so.DocumentTypeId, "
         + "  so.OrderSupCustId, so.CommissionType, so.CommRate, so.CommAmount, so.ItemAmountHeader, so.DiscountAmountHeader, so.TaxAmountHeader, "
         + "  so.OrderAmount, so.SupplierRefNo, so.OrderQty, so.OrderWeight, so.CommissionRemarks, so.OrderDueDays as DueDays, so.OrderDueDate as DueDate, "
         + "  so.OrderExpiryDate, so.IsAproved, so.DeliveryTerm, so.DeliveryDays, so.RemarksHeader, so.OrderStatus, so.PaymentTermsId, "
         + "  so.EntryUser as EntryUserId, so.EntryDate, so.ModifyUser, so.ModifyDate, so.PostUser, so.PostDate, "
         + "  so.OtherCommissionAgentId, so.OtherCommissionType, so.OtherCommissionUom, so.OtherCommissionRate, so.OtherCommissionAmount, so.OtherCommissionRemarks, "
         + "  so.OrderTypeId, so.OtherCategoryId, so.BookingPersonId "
         + " FROM SaleOrder so "
         + " where so.OrganizationId = :organizationId "
         + " and so.CompanyId = :companyId "
         + " and so.DocumentTypeId = :documentTypeId "
         + " and (:docDateFrom is null or CAST(so.DocDate AS DATE) >= CAST(:docDateFrom AS DATE)) "
         + " and (:docDateTo is null or CAST(so.DocDate AS DATE) <= CAST(:docDateTo AS DATE)) "
         + " and (:entryFromDate is null or CAST(so.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + " and (:entryToDate is null or CAST(so.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + " and (:modifyFromDate is null or CAST(so.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + " and (:modifyToDate is null or CAST(so.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + " and (:approvedFromDate is null or CAST(so.PostDate AS DATE) >= CAST(:approvedFromDate AS DATE)) "
         + " and (:approvedToDate is null or CAST(so.PostDate AS DATE) <= CAST(:approvedToDate AS DATE)) "
         + " and (:poSrFrom is null or so.DocNo >= :poSrFrom) "
         + " and (:poSrTo is null or so.DocNo <= :poSrTo) "
         + " And (:supplierCustomerId is null or so.OrderSupCustId = :supplierCustomerId) "
         + " AND (:isApproved is null or so.IsAproved = :isApproved) "
         + " and (:status is null or so.OrderStatus = :status) "
         + " and (:financialYearId is null or so.FinancialYearId = :financialYearId) "
         + " and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and so.EntryUser = :entryUser)) "
         + " AND (:bookingPersonId IS NULL or so.BookingPersonId = :bookingPersonId) "
         + " and ( :branchesIds is null OR ( EXISTS (select 1 from dbo.fnSplitString (:branchesIds, ',') Where so.BranchesId in (data)) ) ) "
         + "), TmpDms AS ( "
         + " SELECT COUNT(d.Id) as NoOfAttachments, d.RefDocumentTypeId, d.RefDocumentNo "
         + " FROM dbo.DMSAttachments d "
         + "  INNER JOIN TmpSaleOrderHistory r on d.RefDocumentTypeId = r.DocumentTypeId and d.RefDocumentNo = r.Id "
         + " WHERE D.OrganizationId = :organizationId AND D.CompanyId = :companyId "
         + " GROUP BY d.RefDocumentTypeId, d.RefDocumentNo "
         + ") "
         + "SELECT so.RecordNo, so.Id, so.DocNo, so.BranchSrNo, br.BranchName, so.DocDate, supcust.CompanyName AS CustomerName, "
         + " so.CommissionType, so.CommRate, so.CommAmount, so.ItemAmountHeader, so.DiscountAmountHeader, so.TaxAmountHeader, so.OrderAmount, "
         + " so.OrderQty, so.OrderWeight, so.CommissionRemarks, so.DueDays, so.DueDate, so.OrderExpiryDate, so.IsAproved, so.DeliveryTerm, "
         + " so.DeliveryDays, so.RemarksHeader, so.OrderStatus, so.SupplierRefNo, Dut.TermsDescription, "
         + " Usr.FirstName + ' ' + Usr.LastName as UserName, so.EntryDate, "
         + " UsM.FirstName + ' ' + UsM.LastName as ModifyUserName, so.ModifyDate, "
         + " UsA.FirstName + ' ' + UsA.LastName as ApprovedUser, so.PostDate, "
         + " so.OtherCommissionAgentId, OtherComm.CompanyName OtherCommissionAgent, so.OtherCommissionType, so.OtherCommissionUom, "
         + " so.OtherCommissionRate, so.OtherCommissionAmount, so.OtherCommissionRemarks, "
         + " ISNULL(dms.NoOfAttachments,0) AS NoOfAttachments, "
         + " OT.LookupName AS OrderType, OTHC.LookupName AS OtherCategory, bp.ReferencePartyName as BookingPerson "
         + "FROM TmpSaleOrderHistory so "
         + " INNER JOIN SupplierCustomer AS supcust ON so.OrderSupCustId = supcust.Id "
         + " INNER JOIN UserAccount Usr on so.EntryUserId = Usr.ID "
         + " LEFT  JOIN InvDueTerms Dut on so.PaymentTermsId = Dut.Id "
         + " LEFT  JOIN UserAccount UsM on so.ModifyUser = UsM.ID "
         + " LEFT  JOIN UserAccount UsA on so.PostUser = UsA.ID "
         + " INNER JOIN Branches br on so.BranchesId = br.Id "
         + " LEFT  JOIN SupplierCustomer AS OtherComm ON so.OtherCommissionAgentId = OtherComm.Id "
         + " LEFT  JOIN InvLookUp OT ON so.OrderTypeId = OT.Id "
         + " LEFT  JOIN InvLookUp OTHC ON so.OtherCategoryId = OTHC.Id "
         + " LEFT  JOIN referenceparties bp ON so.BookingPersonId = bp.Id "
         + " LEFT  JOIN TmpDms dms ON dms.RefDocumentTypeId = so.DocumentTypeId and dms.RefDocumentNo = so.Id "
         + "WHERE (:noOfRecords is null or so.RecordNo <= :noOfRecords) "
         + "ORDER BY so.DocDate, so.DocNo DESC",
           nativeQuery = true)
    List<SaleOrderFormHistoryRow> saleOrderFormHistory(@Param("organizationId") Integer organizationId,
                                                       @Param("companyId") Integer companyId,
                                                       @Param("documentTypeId") Integer documentTypeId,
                                                       @Param("docDateFrom") LocalDate docDateFrom,
                                                       @Param("docDateTo") LocalDate docDateTo,
                                                       @Param("entryFromDate") LocalDate entryFromDate,
                                                       @Param("entryToDate") LocalDate entryToDate,
                                                       @Param("modifyFromDate") LocalDate modifyFromDate,
                                                       @Param("modifyToDate") LocalDate modifyToDate,
                                                       @Param("approvedFromDate") LocalDate approvedFromDate,
                                                       @Param("approvedToDate") LocalDate approvedToDate,
                                                       @Param("poSrFrom") Integer poSrFrom,
                                                       @Param("poSrTo") Integer poSrTo,
                                                       @Param("supplierCustomerId") Integer supplierCustomerId,
                                                       @Param("isApproved") Boolean isApproved,
                                                       @Param("status") String status,
                                                       @Param("financialYearId") Integer financialYearId,
                                                       @Param("canViewAllRecord") Boolean canViewAllRecord,
                                                       @Param("entryUser") Integer entryUser,
                                                       @Param("bookingPersonId") Integer bookingPersonId,
                                                       @Param("branchesIds") String branchesIds,
                                                       @Param("noOfRecords") Integer noOfRecords);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderTradingFormHistory' (native: Dense_Rank, vItemAllocation, V_UomScheduleAndUom, InvSaleInvoiceDetail). */
    @Query(value = "select * from ( "
         + " SELECT distinct Dense_Rank () Over (Order By h.Id Desc) RecordNo, h.Id, h.DocNo, h.DocDate "
         + "  , customer.CompanyName CustomerName, h.OrderSupCustId SupplierCustomerId, pt.TermsDescription PymentTerms "
         + "  , h.OrderDueDate, h.OrderDueDays, h.DeliveryTerm, h.IsAproved, h.OrderStatus, h.RemarksHeader "
         + "  , h.ItemAmountHeader, h.DiscountAmountHeader, h.TaxAmountHeader, h.OrderAmount, h.OrderQty, h.OrderWeight "
         + "  , eu.UserName EntryUser, mu.UserName ModifyUser, au.UserName PostUser "
         + "  , h.BrokerAgentSupCustId CommissionAgentId, comissionAgent.CompanyName ComissionAgent "
         + "  , h.CommissionType, h.CommRate, comissionRateUom.UOMCode ComissionRateUom, h.CommAmount "
         + "  , i.ItemName, packSize.UOMCode, d.OrderItemQty ItemQty, d.OrderItemRate ItemRate, d.Amount ItemAmount "
         + "  , case when (select distinct 1 from InvSaleInvoiceDetail d where d.SaleOrderId = h.Id) = 1 then 'Refered' else 'NotRefered' End as IsRefered "
         + " FROM SaleOrderDetail d "
         + "  INNER JOIN SaleOrder h on h.Id = d.SaleOrderId and d.ActionTypeId <> 3 "
         + "  INNER JOIN SupplierCustomer customer on customer.Id = h.OrderSupCustId "
         + "  INNER JOIN vItemAllocation i on i.Id = d.OrderItemId and i.OrganizationId = h.OrganizationId and i.CompanyId = h.CompanyId "
         + "  INNER JOIN UserAccount eu on eu.ID = h.EntryUser "
         + "  LEFT  JOIN UserAccount mu on mu.ID = h.ModifyUser "
         + "  LEFT  JOIN UserAccount au on au.ID = h.PostUser "
         + "  LEFT  JOIN InvDueTerms pt on pt.Id = h.PaymentTermsId "
         + "  LEFT  JOIN SupplierCustomer comissionAgent on comissionAgent.Id = h.BrokerAgentSupCustId "
         + "  LEFT  JOIN V_UomScheduleAndUom packSize on packSize.Id = d.OrderItemUOMId "
         + "  LEFT  JOIN V_UomScheduleAndUom comissionRateUom on comissionRateUom.Id = h.UomScheduleIdCmRate "
         + " WHERE h.OrganizationId = :organizationId "
         + "  and h.CompanyId = :companyId "
         + "  and h.DocumentTypeId = :documentTypeId "
         + "  and (:financialYearId is null or h.FinancialYearId = :financialYearId) "
         + "  and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and h.EntryUser = :entryUser)) "
         + ") a Where (:noOfRecords is null or a.RecordNo <= :noOfRecords) "
         + "order by a.DocNo desc",
           nativeQuery = true)
    List<SaleOrderTradingFormHistoryRow> saleOrderTradingFormHistory(@Param("organizationId") Integer organizationId,
                                                                     @Param("companyId") Integer companyId,
                                                                     @Param("documentTypeId") Integer documentTypeId,
                                                                     @Param("financialYearId") Integer financialYearId,
                                                                     @Param("canViewAllRecord") Boolean canViewAllRecord,
                                                                     @Param("entryUser") Integer entryUser,
                                                                     @Param("noOfRecords") Integer noOfRecords);

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadBySaleOrderHeaderId' (native: SaleOrderDetail, views, EXISTS subqueries). */
    @Query(value = "SELECT SOD.Id, SOD.SaleOrderId, SOD.OrderItemId, SOD.OrderItemUOMId, SOD.Amount, SOD.OrderItemQty, SOD.OrderItemRate, SOD.OrderRemarks, "
         + " SOD.Crop, CY.Id AS CropYearId, SOD.JobLotId, SOD.NetWeight, SOD.OrderItemRateUOMId, I.ItemName, I.ItemCodeNew, U1.UOMCode, "
         + " U1.Equivalent AS UOMDescription, U1.ScheduleUnitId AS PackScheduleUnitId, JL.JobLotDescription, SOD.CityArea, SOD.LabSampleNo, "
         + " SOD.AmountCalcType, SOD.TaxableStatus, SOD.BagWeight, SOD.BagPrice, SOD.CurrencyId, SOD.FcyAmount, SOD.ExchangeRate, "
         + " U2.Equivalent AS EquivalentRate, U2.UOMCode AS RateUom, SOD.TaxAmount, SOD.RetailRate, SOD.TaxNameId, SOD.TaxPercent, TT.TaxName, "
         + " SOD.CityId, SOD.ItemPrice, SOD.RateDiscount, SOD.PriceScheduleId, SOD.ActionTypeId, SOD.ItemDiscount, SOD.ItemDiscountAmount, "
         + " SOD.PackingAddLessOnRate, SOD.TotalAmount, SOD.PackingTypeID, PT.PackTypeDesc AS PackingType, SOD.CommOnSale, SOD.ReferencePartyId, "
         + " RP.ReferencePartyName, SOD.ItemDiscription, SOD.RefDocId, SOD.RefDocDetailId, PB.DocNo AS RefDocNo, SOD.DiscountTypeId, "
         + " CASE WHEN SOD.DiscountTypeId = 2 THEN 'Percent' WHEN SOD.DiscountTypeId = 1 THEN 'Flat' ELSE '' END AS DiscountType, "
         + " SOD.WarehouseId, WH.WarehouseName, SOD.ItemVariantId, AV.VarientDescription AS VariantDescription, SOD.CastingTypeId, VC.CastingType, "
         + " SOD.CostCenterId, SOD.LocationTypeId, I.ItemCategoryId, I.WeightFinishGoods, "
         + " CAST(CASE WHEN EXISTS (SELECT 1 FROM InvGdnDetail D WHERE D.ActionTypeId <> 3 AND D.SaleOrderDetailId = SOD.Id) THEN 1 ELSE 0 END AS bit) AS ReferredInGDN, "
         + " CAST(CASE WHEN EXISTS (SELECT 1 FROM InvSaleInvoiceDetail D WHERE D.SaleOrderDetailId = SOD.Id) THEN 1 ELSE 0 END AS BIT) AS ReferredInInvoice, "
         + " sod.ItemConditionId, ic.ConditionStatus As ItemCondition, "
         + " sod.SecondaryUomId, SecUom.UomCode as SecondaryUomCode, SecUom.Equivalent SecondaryUomEquivalent, sod.SecondaryUomQty, "
         + " CASE when sod.SecondaryUomQty > 0 And sod.OrderItemQty > 0 then CAST(sod.SecondaryUomQty as float) / sod.OrderItemQty ELSE 0 END as PerItemSecondaryUomQty, "
         + " sod.SecondaryUomItemRate "
         + "FROM SaleOrderDetail AS SOD "
         + "INNER JOIN SaleOrder SOH ON SOD.SaleOrderId = SOH.Id AND SOH.ActionId <> 3 "
         + "LEFT JOIN InvCropYear CY ON CY.CropYear = SOD.Crop AND CY.CompanyId = SOH.CompanyId "
         + "LEFT JOIN TaxesTypes TT ON SOD.TaxNameId = TT.Id "
         + "LEFT JOIN V_UomScheduleAndUom U2 ON SOD.OrderItemRateUOMId = U2.Id "
         + "LEFT JOIN Item I ON I.Id = SOD.OrderItemId "
         + "LEFT JOIN InvPackingType PT ON PT.Id = SOD.PackingTypeID "
         + "LEFT JOIN V_UomScheduleAndUom U1 ON U1.Id = SOD.OrderItemUOMId "
         + "LEFT JOIN JobLot JL ON JL.Id = SOD.JobLotId "
         + "LEFT JOIN ReferenceParties RP ON SOD.ReferencePartyId = RP.Id "
         + "LEFT JOIN PreBookingOrder PB ON PB.Id = SOD.RefDocId "
         + "LEFT JOIN InvWareHouse WH ON WH.Id = SOD.WarehouseId "
         + "LEFT JOIN ItemAttributeVarient AV ON AV.Id = SOD.ItemVariantId "
         + "LEFT JOIN mfg.vCastingType VC ON VC.CastingTypeId = SOD.CastingTypeId "
         + "LEFT JOIN V_UomScheduleAndUom SecUom on sod.SecondaryUomId = SecUom.Id AND sod.OrderItemId = SecUom.ItemId "
         + "LEFT JOIN V_ItemCondition ic on sod.ItemConditionId = ic.ID "
         + "WHERE SOD.SaleOrderId = :id AND SOD.ActionTypeId <> 3",
           nativeQuery = true)
    List<SaleOrderDetailByHeaderIdRow> readBySaleOrderHeaderId(@Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadBySaleOrderId_ExtraItemDetail' (native: SaleOrderExtraItemsDetail, V_UomScheduleAndUom). */
    @Query(value = "SELECT d.Id, d.SaleOrderId, "
         + " d.MasterItemId, MasterItem.ItemName MasterItemName, MasterItem.ItemCodeNew MasterItemCode, "
         + " d.MasterPackUomId, MasterUom.UOMCode MasterPackUomCode, MasterUom.Equivalent MasterPackUomEquivalent, "
         + " d.ItemId, Item.ItemName ItemName, Item.ItemCodeNew ItemCode, "
         + " d.PackUomId, Uom.UOMCode PackUomCode, Uom.Equivalent PackUomEquivalent, d.ItemDiscription, d.Remarks, "
         + " d.JobLotId, JobLot.JobLotDescription, d.PackingTypeId, p.PackTypeCode PackingType, d.ItemQty, d.NetWeight "
         + "FROM SaleOrderExtraItemsDetail AS d "
         + " Inner JOIN Item ON Item.Id = d.ItemId "
         + " Inner JOIN V_UomScheduleAndUom Uom ON d.PackUomId = Uom.Id "
         + " LEFT JOIN Item MasterItem ON MasterItem.Id = d.MasterItemId "
         + " LEFT JOIN V_UomScheduleAndUom MasterUom ON d.MasterPackUomId = MasterUom.Id "
         + " LEFT JOIN InvPackingType p ON p.Id = d.PackingTypeId "
         + " LEFT JOIN JobLot ON JobLot.Id = d.JobLotId "
         + "where d.SaleOrderId = :id",
           nativeQuery = true)
    List<SaleOrderExtraItemDetailRow> readBySaleOrderIdExtraItemDetail(@Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderPaymentTermDetailByHeaderId' (native: SaleOrderPaymentTermsDetail, InvDueTerms). */
    @Query(value = "SELECT d.Id, d.SaleOrderId, d.PaymentTermId, d.PaymentTermId As PaymentTermsId, pt.TermsDescription, "
         + " d.PrcntOfTotal, d.DueDays, d.PaymentRemarks, d.Amount, d.SortNo, d.DueDate "
         + "FROM SaleOrderPaymentTermsDetail d "
         + " INNER JOIN InvDueTerms pt on d.PaymentTermId = pt.Id "
         + "WHERE D.SaleOrderId = :id",
           nativeQuery = true)
    List<SaleOrderPaymentTermDetailRow> saleOrderPaymentTermDetailByHeaderId(@Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderPaymentTermDetailBySoIds' (native: fnSplitString; orderIds = comma separated SaleOrder ids). */
    @Query(value = "SELECT d.Id, d.SaleOrderId, So.DocNo As SaleOrderNo, d.PaymentTermId, d.PaymentTermId As PaymentTermsId, pt.TermsDescription, "
         + " d.PrcntOfTotal, d.DueDays, d.PaymentRemarks, d.Amount, d.SortNo, d.DueDate "
         + "FROM SaleOrderPaymentTermsDetail d "
         + " INNER JOIN InvDueTerms pt on d.PaymentTermId = pt.Id "
         + " INNER JOIN SaleOrder So ON So.Id = d.SaleOrderId "
         + "WHERE D.SaleOrderId in (Select data from fnSplitString(:orderIds, ','))",
           nativeQuery = true)
    List<SaleOrderPaymentTermDetailBySoIdsRow> saleOrderPaymentTermDetailBySoIds(@Param("orderIds") String orderIds);

    /**
     * Sp_SaleOrder_GetAllMethod @Activity='ReadById' (native: InvDueTerms, V_UomScheduleAndUom, DMSAttachments count).
     * The proc selects OrderAmount/OrderQty/OrderWeight twice; the projection exposes each alias once.
     */
    @Query(value = "SELECT PoH.Id, PoH.DocumentTypeId, PoH.OrderType, Dtp.DocumentTypeCode, "
         + " Dtp.DocumentTypeDescription, PoH.DocNo, PoH.BranchSrNo, PoH.DocDate, PoH.OrderCatagoryId, PoH.CatagorySrNo, "
         + " PoH.OrderSupCustId, Sup.CompanyName AS SupplierName, "
         + " PoH.SupplierRefNo, PoH.RefrenenceParty, PoH.SupplierCustomerIdStockParty, "
         + " PoH.BrokerAgentSupCustId, Agent.CompanyName AS Expr1, PoH.CommissionType, PoH.CommRate, PoH.UomScheduleIdCmRate, "
         + " V_UomScheduleAndUom.UOMCode, V_UomScheduleAndUom.UOMDescription, PoH.CommAmount, "
         + " PoH.CommissionRemarks, PoH.PaymentTermsId, pTerm.TermsCode, pTerm.TermsDescription, PoH.OrderDueDays, "
         + " PoH.OrderDueDate, PoH.OrderExpiryDate, PoH.DeliveryTerm, PoH.DeliveryStartDate, "
         + " PoH.DeliveryDays, PoH.DeliveryRemarks, PoH.OrderTaxable, PoH.RemarksHeader, "
         + " PoH.OrderStatus, PoH.IsAproved, PoH.EntryDate, "
         + " PoH.EntryUser, PoH.ModifyDate, PoH.ModifyUser, PoH.PostDate, PoH.PostUser, "
         + " PoH.PostState, PoH.OrganizationId, PoH.CompanyId, PoH.BranchesId, PoH.ProjectsId, "
         + " PoH.ExportInvoiceNo, PoH.ExportOrderNo, PoH.OrderQty, poh.OrderWeight, "
         + " poh.OrderAmount, poh.ExchangeRate, poh.CurrencyId, poh.FcyAmount, "
         + " PoH.NoOfCntnr, PoH.ExportLotRef, PoH.PfiScId, PoH.ActionId, PoH.FinancialYearId, Poh.IsValidate, poh.BillCalculateTypeId, "
         + " POH.OrderStatusRemarks, Poh.ItemAmountHeader, Poh.DiscountAmountHeader, Poh.TaxAmountHeader, "
         + " Poh.OrderAmount, Poh.OrderQty, Poh.OrderWeight, Poh.BookingPersonId, "
         + " (SELECT COUNT(Attachment) AS Expr1 FROM DMSAttachments WHERE (RefDocumentTypeId = PoH.DocumentTypeId) AND (RefDocumentNo = PoH.Id)) AS NoOfAttachments, "
         + " PoH.AttachmentsValues, poh.CustomAttachmentsValues, Poh.OtherCommissionAgentId, OtherComm.CompanyName OtherCommissionAgent, "
         + " Poh.OtherCommissionType, Poh.OtherCommissionUom, Poh.OtherCommissionRate, Poh.OtherCommissionAmount, Poh.OtherCommissionRemarks, "
         + " POH.OrderTypeId, POH.OtherCategoryId "
         + "FROM SaleOrder AS PoH "
         + " INNER JOIN SupplierCustomer AS Sup ON PoH.OrderSupCustId = Sup.Id "
         + " LEFT  JOIN InvDueTerms AS pTerm ON PoH.PaymentTermsId = pTerm.Id "
         + " LEFT  JOIN V_UomScheduleAndUom ON PoH.UomScheduleIdCmRate = V_UomScheduleAndUom.Id "
         + " LEFT  JOIN SupplierCustomer AS Agent ON PoH.BrokerAgentSupCustId = Agent.Id "
         + " LEFT  JOIN SupplierCustomer AS SupStk ON PoH.SupplierCustomerIdStockParty = SupStk.Id "
         + " LEFT  JOIN DocumentType AS Dtp ON PoH.DocumentTypeId = Dtp.Id "
         + " LEFT  JOIN SupplierCustomer AS OtherComm ON PoH.OtherCommissionAgentId = OtherComm.Id "
         + "WHERE [PoH].[Id] = :id and [PoH].[ActionId] <> 3",
           nativeQuery = true)
    List<SaleOrderReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadAll' (native: InvOrderCategory, InvDueTerms, DMSAttachments count). */
    @Query(value = "SELECT s.Id, s.DocumentTypeId, s.DocNo, s.BranchSrNo, s.DocDate, s.OrderCatagoryId, s.CatagorySrNo, s.OrderSupCustId, "
         + " s.SupplierRefNo, s.RefrenenceParty, s.SupplierCustomerIdStockParty, s.BrokerAgentSupCustId, s.CommissionType, "
         + " s.CommRate, s.UomScheduleIdCmRate, s.CommAmount, s.CommissionRemarks, s.PaymentTermsId, s.OrderDueDays, s.OrderDueDate, s.OrderExpiryDate, "
         + " s.DeliveryTerm, s.DeliveryStartDate, s.DeliveryDays, s.DeliveryRemarks, s.OrderTaxable, s.OrderType, s.RemarksHeader, s.OrderStatus, s.IsAproved, "
         + " s.EntryDate, s.EntryUser, s.ModifyDate, s.ModifyUser, s.PostDate, s.PostUser, s.PostState, s.OrganizationId, s.CompanyId, "
         + " s.BranchesId, s.ProjectsId, s.ExportInvoiceNo, s.ExportOrderNo, s.NoOfCntnr, s.ExportLotRef, s.PfiScId, s.ActionId, "
         + " InvOrderCategory.OrderCategoryName, SupplierCustomer.CompanyName, InvDueTerms.TermsDescription, "
         + " s.ItemAmountHeader, s.DiscountAmountHeader, s.TaxAmountHeader, s.OrderAmount, s.OrderQty, s.OrderWeight, "
         + " (SELECT COUNT(Attachment) FROM dbo.DMSAttachments WHERE RefDocumentTypeId = s.DocumentTypeId AND RefDocumentNo = s.Id) AS NoOfAttachments, "
         + " s.OtherCommissionAgentId, OtherComm.CompanyName OtherCommissionAgent, s.OtherCommissionType, s.OtherCommissionUom, "
         + " s.OtherCommissionRate, s.OtherCommissionAmount, s.OtherCommissionRemarks "
         + "FROM SaleOrder AS s "
         + " INNER JOIN InvOrderCategory ON s.OrderCatagoryId = InvOrderCategory.Id "
         + " INNER JOIN SupplierCustomer ON s.SupplierCustomerIdStockParty = SupplierCustomer.Id "
         + " INNER JOIN InvDueTerms ON s.PaymentTermsId = InvDueTerms.Id "
         + " LEFT  JOIN SupplierCustomer AS OtherComm ON s.OtherCommissionAgentId = OtherComm.Id "
         + "where s.OrganizationId = :organizationId "
         + " and s.CompanyId = :companyId "
         + " and s.DocumentTypeId = :documentTypeId "
         + " and (:financialYearId is null or s.FinancialYearId = :financialYearId) "
         + " and s.ActionId <> 3",
           nativeQuery = true)
    List<SaleOrderReadAllRow> readAll(@Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("documentTypeId") Integer documentTypeId,
                                      @Param("financialYearId") Integer financialYearId);

    // ------------------------------------------------------------------ UpdateStatusandIsApprovedbyOrderId

    /**
     * Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' (all @ReqType sub-branches:
     * Status, Approve, Cancel, Open, UpdateExpiryDate, ApprovedAndCancel) followed by EXEC USP_UserAudit_Insert (ActionId 2).
     * // KEEP-EXEC: multi-branch update + audit procedure call; behaviour kept identical.
     * docDate is only used by @ReqType='UpdateExpiryDate'; orderStatusRemarks is ignored by 'Approve'.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_SaleOrder_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @Id = :id, "
         + "@Activity = 'UpdateStatusandIsApprovedbyOrderId', @ReqType = :reqType, @OrderStatusRemarks = :orderStatusRemarks, "
         + "@PostUser = :postUser, @DocDate = :docDate",
           nativeQuery = true)
    void spUpdateStatusAndIsApprovedByOrderId(@Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("id") Integer id,
                                              @Param("reqType") String reqType,
                                              @Param("orderStatusRemarks") String orderStatusRemarks,
                                              @Param("postUser") Integer postUser,
                                              @Param("docDate") LocalDate docDate);

    /** Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' @ReqType='Status' (OrderStatus='Complete'; no audit). */
    @Modifying
    @Transactional
    @Query("UPDATE SaleOrder s SET s.orderStatus = 'Complete', s.orderStatusRemarks = :orderStatusRemarks, s.postState = true, "
         + "s.postDate = CURRENT_TIMESTAMP, s.postUser = :postUser "
         + "WHERE s.id = :id AND s.organizationId = :organizationId AND s.companyId = :companyId")
    int updateStatusComplete(@Param("id") Integer id,
                             @Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId,
                             @Param("orderStatusRemarks") String orderStatusRemarks,
                             @Param("postUser") Integer postUser);

    /** Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' @ReqType='Approve' (no audit). */
    @Modifying
    @Transactional
    @Query("UPDATE SaleOrder s SET s.isAproved = true, s.postState = true, s.postDate = CURRENT_TIMESTAMP, s.postUser = :postUser "
         + "WHERE s.id = :id AND s.organizationId = :organizationId AND s.companyId = :companyId")
    int updateApprove(@Param("id") Integer id,
                      @Param("organizationId") Integer organizationId,
                      @Param("companyId") Integer companyId,
                      @Param("postUser") Integer postUser);

    /** Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' @ReqType='Cancel' (no audit). */
    @Modifying
    @Transactional
    @Query("UPDATE SaleOrder s SET s.orderStatus = 'Cancel', s.orderStatusRemarks = :orderStatusRemarks, s.postState = true, "
         + "s.postDate = CURRENT_TIMESTAMP, s.postUser = :postUser "
         + "WHERE s.id = :id AND s.organizationId = :organizationId AND s.companyId = :companyId")
    int updateStatusCancel(@Param("id") Integer id,
                           @Param("organizationId") Integer organizationId,
                           @Param("companyId") Integer companyId,
                           @Param("orderStatusRemarks") String orderStatusRemarks,
                           @Param("postUser") Integer postUser);

    /** Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' @ReqType='Open' (no audit). */
    @Modifying
    @Transactional
    @Query("UPDATE SaleOrder s SET s.orderStatus = 'Open', s.orderStatusRemarks = :orderStatusRemarks, s.postState = true, "
         + "s.postDate = CURRENT_TIMESTAMP, s.postUser = :postUser "
         + "WHERE s.id = :id AND s.organizationId = :organizationId AND s.companyId = :companyId")
    int updateStatusOpen(@Param("id") Integer id,
                         @Param("organizationId") Integer organizationId,
                         @Param("companyId") Integer companyId,
                         @Param("orderStatusRemarks") String orderStatusRemarks,
                         @Param("postUser") Integer postUser);

    /** Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' @ReqType='UpdateExpiryDate' (OrderExpiryDate = @DocDate; no audit). */
    @Modifying
    @Transactional
    @Query("UPDATE SaleOrder s SET s.postUser = :postUser, s.orderStatusRemarks = :orderStatusRemarks, s.orderExpiryDate = :docDate "
         + "WHERE s.id = :id AND s.organizationId = :organizationId AND s.companyId = :companyId")
    int updateExpiryDate(@Param("id") Integer id,
                         @Param("organizationId") Integer organizationId,
                         @Param("companyId") Integer companyId,
                         @Param("orderStatusRemarks") String orderStatusRemarks,
                         @Param("postUser") Integer postUser,
                         @Param("docDate") java.time.LocalDateTime docDate);

    /** Sp_SaleOrder_GetAllMethod @Activity='UpdateStatusandIsApprovedbyOrderId' @ReqType='ApprovedAndCancel' (no audit). */
    @Modifying
    @Transactional
    @Query("UPDATE SaleOrder s SET s.isAproved = true, s.postState = true, s.postDate = CURRENT_TIMESTAMP, s.postUser = :postUser, "
         + "s.orderStatus = 'Cancel', s.orderStatusRemarks = :orderStatusRemarks "
         + "WHERE s.id = :id AND s.organizationId = :organizationId AND s.companyId = :companyId")
    int updateApprovedAndCancel(@Param("id") Integer id,
                                @Param("organizationId") Integer organizationId,
                                @Param("companyId") Integer companyId,
                                @Param("orderStatusRemarks") String orderStatusRemarks,
                                @Param("postUser") Integer postUser);

    // ------------------------------------------------------------------

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSaleOrderNotInInvoice' (native: InvSaleInvoiceDetail / InvSaleInvoice). */
    @Query(value = "select Id, DocNo from SaleOrder "
         + "where Id not in (select distinct(isnull(SaleOrderId,0)) from InvSaleInvoiceDetail "
         + " inner join InvSaleInvoice on InvSaleInvoice.Id = InvSaleInvoiceId "
         + " where OrganizationId = :organizationId and CompanyId = :companyId) "
         + "and DocumentTypeId = :documentTypeId",
           nativeQuery = true)
    List<SaleOrderIdDocNoRow> getSaleOrderNotInInvoice(@Param("organizationId") Integer organizationId,
                                                       @Param("companyId") Integer companyId,
                                                       @Param("documentTypeId") Integer documentTypeId);

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadFromSaleOrderBySaleOrderId' (native: GatePassOutward). */
    @Query(value = "SELECT PoH.Id, PoH.DeliveryTerm, GatePassOutward.GatepassType "
         + "FROM SaleOrder AS PoH Left Outer JOIn GatePassOutward ON PoH.Id = GatePassOutward.SaleOrderId "
         + "WHERE [PoH].[Id] = :id",
           nativeQuery = true)
    List<SaleOrderDeliveryTermGatepassRow> readFromSaleOrderBySaleOrderId(@Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSaleOrderdetailforDeliverySchedule' (native: SaleOrderDetail, V_UomScheduleAndUom). */
    @Query(value = "SELECT PiD.Id, PiH.DocDate, PiH.DocNo, PiH.OrderSupCustId, Sp.CompanyName, PiH.SupplierRefNo "
         + " , PiD.OrderItemId AS ItemId, PiD.OrderItemUOMId, Item.ProductNo, Item.ManufacturePartNo, "
         + " dbo.Item.ItemName, item.ItemCodeNew, ISNULL(PiD.OrderItemQty,0) AS OrderItemQty, dbo.Item.ItemCode, pid.CommOnSale, UomRate.UOMCode "
         + "FROM dbo.SaleOrder AS PiH "
         + " INNER JOIN dbo.SaleOrderDetail AS PiD ON PiH.Id = PiD.SaleOrderId and pid.ActionTypeId <> 3 and ISNULL(pid.isCanceled,0) = 0 "
         + " INNER JOIN dbo.SupplierCustomer AS Sp ON PiH.OrderSupCustId = Sp.Id "
         + " INNER JOIN dbo.Item ON PiD.OrderItemId = dbo.Item.Id "
         + " inner join dbo.V_UomScheduleAndUom AS UomRate ON PiD.OrderItemUOMId = UomRate.Id "
         + "WHERE PiH.OrganizationId = :organizationId "
         + "AND (:companyId IS NULL or PiH.CompanyId = :companyId) "
         + "AND (:orderSupCustId IS NULL or pih.OrderSupCustId = :orderSupCustId) "
         + "AND (:docDateFrom IS NULL OR PiH.DocDate >= :docDateFrom) "
         + "AND (:docDateTo IS NULL OR PiH.DocDate <= :docDateTo) "
         + "AND (:docNo IS NULL OR PiH.DocNo = :docNo) "
         + "AND (:id IS NULL OR PiD.Id = :id) "
         + "AND (PiH.DocumentTypeId = :documentTypeId)",
           nativeQuery = true)
    List<SaleOrderDetailForDeliveryScheduleRow> getSaleOrderdetailforDeliverySchedule(@Param("organizationId") Integer organizationId,
                                                                                      @Param("companyId") Integer companyId,
                                                                                      @Param("orderSupCustId") Integer orderSupCustId,
                                                                                      @Param("docDateFrom") LocalDate docDateFrom,
                                                                                      @Param("docDateTo") LocalDate docDateTo,
                                                                                      @Param("docNo") Integer docNo,
                                                                                      @Param("id") Integer id,
                                                                                      @Param("documentTypeId") Integer documentTypeId);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderIdandNoGetForSupplierCustomerId' (native: InvDueTerms; DocumentTypeId in (81,127,1605,1656)). */
    @Query(value = "select h.Id as SaleOrderId, h.DocNo as SaleOrderNo, h.PaymentTermsId, d.TermsDescription PaymentTerm "
         + "from SaleOrder h "
         + " Left Join InvdueTerms d on d.Id = h.PaymentTermsId "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.OrderSupCustId = :orderSupCustId "
         + "and h.DocumentTypeId in (81,127,1605,1656) "
         + "and h.IsAproved = 1 "
         + "and h.OrderStatus = 'Open'",
           nativeQuery = true)
    List<SaleOrderIdNoPaymentTermRow> saleOrderIdandNoGetForSupplierCustomerId(@Param("organizationId") Integer organizationId,
                                                                               @Param("companyId") Integer companyId,
                                                                               @Param("orderSupCustId") Integer orderSupCustId);

    /** Sp_SaleOrder_GetAllMethod @Activity='OrderIdandNoForIssuanceGdnBySupplierCustomerId' (native: InvGdn / InvGdnDetail; DocumentTypeId 1605). */
    @Query(value = "select h.Id as SaleOrderId, h.DocNo as SaleOrderNo from SaleOrder h "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.OrderSupCustId = :orderSupCustId "
         + "and h.DocumentTypeId in (1605) "
         + "and h.IsAproved = 1 "
         + "and h.OrderStatus = 'Open' "
         + "and (h.Id not in ( Select d.SaleOrderId from InvGdn gh "
         + "   inner join InvGdnDetail d on gh.Id = d.InvGdnId "
         + "   where gh.OrganizationId = :organizationId "
         + "   and gh.CompanyId = :companyId "
         + "   and gh.DocumentTypeId = :documentTypeId "
         + "   and (:recId is null or gh.Id <> :recId) ) )",
           nativeQuery = true)
    List<SaleOrderIdNoRow> orderIdandNoForIssuanceGdnBySupplierCustomerId(@Param("organizationId") Integer organizationId,
                                                                          @Param("companyId") Integer companyId,
                                                                          @Param("orderSupCustId") Integer orderSupCustId,
                                                                          @Param("documentTypeId") Integer documentTypeId,
                                                                          @Param("recId") Integer recId);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetCurrentStockByItemId' (native scalar function dbo.FunGetWeightCurrStockByItem). */
    @Query(value = "select dbo.FunGetWeightCurrStockByItem(:organizationId, :companyId, :itemId, :docDateTo, :wareHouseId, :jobLotId, :cropYear, :invPackingTypeId, :itemUomId) AvailableStock",
           nativeQuery = true)
    Double getCurrentStockByItemId(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("itemId") Integer itemId,
                                   @Param("docDateTo") LocalDate docDateTo,
                                   @Param("wareHouseId") Integer wareHouseId,
                                   @Param("jobLotId") Integer jobLotId,
                                   @Param("cropYear") String cropYear,
                                   @Param("invPackingTypeId") Integer invPackingTypeId,
                                   @Param("itemUomId") Integer itemUomId);

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadSaleOrderNo' (native: SaleOrderDetail). */
    @Query(value = "select Distinct h.Id, h.DocNo "
         + "from SaleOrder h "
         + " Inner Join SaleOrderDetail d on d.SaleOrderId = h.Id "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and (:orderSupCustId is null or h.OrderSupCustId = :orderSupCustId) "
         + "and (:itemId is null or d.OrderItemId = :itemId) "
         + "and (h.IsAproved = 1) "
         + "and (h.OrderStatus = 'Open')",
           nativeQuery = true)
    List<SaleOrderIdDocNoRow> readSaleOrderNo(@Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("orderSupCustId") Integer orderSupCustId,
                                              @Param("itemId") Integer itemId);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetOrderDetailRecordByOrderId' (native: SaleOrderDetail, GetSaleOrderandGatepassQtyBalance, V_UomScheduleAndUom). */
    @Query(value = "select d.Id, d.SaleOrderId, d.OrderItemId, d.OrderItemUOMId, i.ItemName, ItemUom.Equivalent, d.Crop, d.JobLotId, j.JobLotCode, "
         + " [dbo].[GetSaleOrderandGatepassQtyBalance](:organizationId, :companyId, d.SaleOrderId, d.OrderItemId) OrderItemQty, "
         + " d.OrderItemRate, d.OrderItemRateUOMId, RateUom.Equivalent RateUOM "
         + "from SaleOrderDetail d "
         + " inner join SaleOrder h on h.Id = d.SaleOrderId and d.ActionTypeId <> 3 "
         + " inner join Item i on d.OrderItemId = i.Id "
         + " inner join JobLot j on d.JobLotId = j.Id "
         + " inner join V_UomScheduleAndUom ItemUom on d.OrderItemUOMId = ItemUom.Id "
         + " inner join V_UomScheduleAndUom RateUom on d.OrderItemRateUOMId = RateUom.Id "
         + "where d.SaleOrderId = :saleOrderId "
         + "and h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and ([dbo].[GetSaleOrderandGatepassQtyBalance](:organizationId, :companyId, d.SaleOrderId, d.OrderItemId) > 0)",
           nativeQuery = true)
    List<SaleOrderDetailRecordRow> getOrderDetailRecordByOrderId(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("saleOrderId") Integer saleOrderId);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSaleOrderDetailForTraderProLoader' (native: SaleOrderDetail, vItemAllocation, V_UomScheduleAndUom). */
    @Query(value = "select distinct h.Id as SaleOrderId, h.DocDate, h.DocNo as SaleOrderNo, h.RemarksHeader, h.DeliveryTerm, h.DeliveryDays, h.BrokerAgentSupCustId, "
         + " H.OrderSupCustId, SP.CompanyName CustomerName, h.OrderStatus, "
         + " h.CommissionType, h.CommRate, h.UomScheduleIdCmRate, h.CommAmount, h.CommissionRemarks, h.OrderDueDays, h.IsAproved, "
         + " entryUser.UserName as EntryUser, approvedUser.UserName as ApprovedUser, "
         + " d.Id as SaleOrderDetailId, d.OrderItemId as ItemId, i.ItemName, "
         + " d.JobLotId, J.JobLotDescription, d.OrderItemQty as ItemQty, d.OrderItemUOMId as ItemUOMId, ItemUom.Equivalent PackUom, ItemUom.UOMCode, d.NetWeight "
         + " , d.OrderItemRate as ItemRate, d.CommOnSale, d.OrderItemRateUOMId as UomScheduleIdRate, RateUom.Equivalent RateUom, RateUom.UOMCode RateUomCode, "
         + " d.Amount as ItemAmount, d.OrderRemarks DetailRemarks "
         + "from SaleOrderDetail d "
         + " INNER JOIN SaleOrder h on h.Id = d.SaleOrderId and d.ActionTypeId <> 3 and ISNULL(d.isCanceled,0) = 0 "
         + " INNER JOIN vItemAllocation i on i.Id = d.OrderItemId "
         + " INNER JOIN SupplierCustomer SP ON H.OrderSupCustId = SP.Id "
         + " INNER JOIN UserAccount entryUser on entryUser.Id = h.EntryUser "
         + " LEFT  JOIN UserAccount approvedUser on approvedUser.Id = h.PostUser "
         + " LEFT JOIN V_UomScheduleAndUom ItemUom on d.OrderItemUOMId = ItemUom.Id "
         + " Left JOIN V_UomScheduleAndUom RateUom on d.OrderItemRateUOMId = RateUom.Id "
         + " LEFT JOIN JobLot j on j.Id = d.JobLotId "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.DocumentTypeId = :documentTypeId "
         + "and (:orderSupCustId is null or h.OrderSupCustId = :orderSupCustId) "
         + "and (:itemId is null or d.OrderItemId = :itemId) "
         + "and (:jobLotId is null or d.JobLotId = :jobLotId) "
         + "and (:docDateFrom is null or h.DocDate >= :docDateFrom) "
         + "and (:docDateTo is null or h.DocDate <= :docDateTo) "
         + "AND (h.OrderStatus = 'Open') "
         + "AND (h.IsAproved = 1)",
           nativeQuery = true)
    List<SaleOrderDetailForTraderProLoaderRow> getSaleOrderDetailForTraderProLoader(@Param("organizationId") Integer organizationId,
                                                                                    @Param("companyId") Integer companyId,
                                                                                    @Param("documentTypeId") Integer documentTypeId,
                                                                                    @Param("orderSupCustId") Integer orderSupCustId,
                                                                                    @Param("itemId") Integer itemId,
                                                                                    @Param("jobLotId") Integer jobLotId,
                                                                                    @Param("docDateFrom") LocalDate docDateFrom,
                                                                                    @Param("docDateTo") LocalDate docDateTo);

    /** Sp_SaleOrder_GetAllMethod @Activity='GetSaleOrderDetailForBookingInvoice' (native: SaleOrderDetail, vItemAllocation, InvSaleInvoiceDetail). */
    @Query(value = "select distinct h.Id as SaleOrderId, h.DocDate, h.DocNo as SaleOrderNo, h.RemarksHeader, h.DeliveryTerm, h.DeliveryDays, h.BrokerAgentSupCustId, "
         + " H.OrderSupCustId, SP.CompanyName CustomerName, h.OrderStatus, "
         + " h.CommissionType, h.CommRate, h.UomScheduleIdCmRate, h.CommAmount, h.CommissionRemarks, h.OrderDueDays, h.IsAproved, "
         + " entryUser.UserName as EntryUser, approvedUser.UserName as ApprovedUser, "
         + " d.Id as SaleOrderDetailId, d.OrderItemId as ItemId, i.ItemName, "
         + " d.JobLotId, J.JobLotDescription, d.OrderItemQty as ItemQty, d.OrderItemUOMId as ItemUOMId, ItemUom.Equivalent PackUom, ItemUom.UOMCode, d.NetWeight "
         + " , d.OrderItemRate as ItemRate, d.CommOnSale, d.OrderItemRateUOMId as UomScheduleIdRate, RateUom.Equivalent RateUom, d.Amount as ItemAmount, d.OrderRemarks DetailRemarks, "
         + " d.DiscountTypeId, Case when d.DiscountTypeId = 2 then 'Percent' when d.DiscountTypeId = 1 then 'Flat' else '' End As DiscountType "
         + "from SaleOrderDetail d "
         + " INNER JOIN SaleOrder h on h.Id = d.SaleOrderId and d.ActionTypeId <> 3 and ISNULL(d.isCanceled,0) = 0 "
         + " INNER JOIN SupplierCustomer SP ON H.OrderSupCustId = SP.Id "
         + " INNER JOIN UserAccount entryUser on entryUser.Id = h.EntryUser "
         + " LEFT  JOIN UserAccount approvedUser on approvedUser.Id = h.PostUser "
         + " LEFT JOIN V_UomScheduleAndUom ItemUom on d.OrderItemUOMId = ItemUom.Id "
         + " Left JOIN V_UomScheduleAndUom RateUom on d.OrderItemRateUOMId = RateUom.Id "
         + " INNER JOIN vItemAllocation i on i.Id = d.OrderItemId "
         + " LEFT JOIN JobLot j on j.Id = d.JobLotId "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and h.DocumentTypeId = :documentTypeId "
         + "and (:orderSupCustId is null or h.OrderSupCustId = :orderSupCustId) "
         + "and (:itemId is null or d.OrderItemId = :itemId) "
         + "and (:jobLotId is null or d.JobLotId = :jobLotId) "
         + "and (:docDateFrom is null or h.DocDate >= :docDateFrom) "
         + "and (:docDateTo is null or h.DocDate <= :docDateTo) "
         + "AND (h.OrderStatus = 'Open') "
         + "and (h.IsAproved = 1) "
         + "and (h.Id not in (select d.SaleOrderId from InvSaleInvoiceDetail d inner join InvSaleInvoice h on d.InvSaleInvoiceId = h.Id "
         + "   where h.OrganizationId = :organizationId and h.CompanyId = :companyId))",
           nativeQuery = true)
    List<SaleOrderDetailForBookingInvoiceRow> getSaleOrderDetailForBookingInvoice(@Param("organizationId") Integer organizationId,
                                                                                  @Param("companyId") Integer companyId,
                                                                                  @Param("documentTypeId") Integer documentTypeId,
                                                                                  @Param("orderSupCustId") Integer orderSupCustId,
                                                                                  @Param("itemId") Integer itemId,
                                                                                  @Param("jobLotId") Integer jobLotId,
                                                                                  @Param("docDateFrom") LocalDate docDateFrom,
                                                                                  @Param("docDateTo") LocalDate docDateTo);

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderModernHistory_HeaderData' (native: ROW_NUMBER, GETDATE, SaleOrderDetail aggregate). */
    @Query(value = "SELECT * from( "
         + " SELECT ROW_NUMBER () Over (Order By h.DocDate Desc) RecordNo, h.Id OrderId, h.DocNo, h.DocDate, "
         + "  Supplier.CompanyName Supplier_Customer_Name, "
         + "  h.IsAproved, h.OrderStatus, CASE WHEN CONVERT(DATE,h.DocDate) < CONVERT(DATE,GETDATE()) THEN 'EXPIRED' ELSE 'NOT-EXPIRED' END ExpiryStatus, "
         + "  SUM(d.Amount) OrderAmount "
         + " FROM dbo.SaleOrder h "
         + " INNER JOIN SupplierCustomer Supplier on Supplier.Id = h.OrderSupCustId "
         + " INNER JOIN dbo.SaleOrderDetail d on d.SaleOrderId = h.Id and d.ActionTypeId <> 3 "
         + " WHERE h.OrganizationId = :organizationId "
         + " AND h.CompanyId = :companyId "
         + " AND h.FinancialYearId = :financialYearId "
         + " AND h.DocumentTypeId = :documentTypeId "
         + " AND (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and h.EntryUser = :entryUser)) "
         + " AND (:isApproved IS NULL OR h.IsAproved = :isApproved) "
         + " GROUP BY h.Id, h.DocNo, h.DocDate, Supplier.CompanyName, h.IsAproved, h.OrderStatus "
         + ") h Where (:noOfRecords is null or h.RecordNo <= :noOfRecords)",
           nativeQuery = true)
    List<SaleOrderModernHistoryHeaderDataRow> saleOrderModernHistoryHeaderData(@Param("organizationId") Integer organizationId,
                                                                               @Param("companyId") Integer companyId,
                                                                               @Param("financialYearId") Integer financialYearId,
                                                                               @Param("documentTypeId") Integer documentTypeId,
                                                                               @Param("canViewAllRecord") Boolean canViewAllRecord,
                                                                               @Param("entryUser") Integer entryUser,
                                                                               @Param("isApproved") Boolean isApproved,
                                                                               @Param("noOfRecords") Integer noOfRecords);

    /**
     * Sp_SaleOrder_GetAllMethod @Activity='SaleOrderModernHistory' - FIRST result set (#SaleOrder header).
     * Native: config 91 (attachment URL) inlined via dbo.GetConfigurationByConfigId, UserProfile, AttachmentsHistory, ROW_NUMBER.
     * The proc returns two result sets; the second is saleOrderModernHistoryDetail().
     */
    @Query(value = "SELECT * from( "
         + " SELECT ROW_NUMBER () Over (Order By h.DocDate Desc) RecordNo, h.Id OrderId, h.DocNo, h.DocDate, OrderCategory.OrderCategoryName, h.CatagorySrNo, "
         + "  h.SupplierRefNo, h.RefrenenceParty, PaymentTerms.TermsDescription PaymentTerm, h.OrderDueDays, h.OrderDueDate, "
         + "  Supplier.CompanyName Supplier_Customer_Name, Supplier.MobilePersonal Supplier_Customer_Mobile, Supplier.Email Supplier_Customer_Email, "
         + "  Supplier.Address1 Supplier_Customer_Address, "
         + "  h.OrderExpiryDate, h.DeliveryTerm, h.DeliveryStartDate, h.DeliveryDays, h.DeliveryRemarks, h.RemarksHeader, "
         + "  h.IsAproved, h.OrderStatus, CASE WHEN CONVERT(DATE,h.DocDate) < CONVERT(DATE,GETDATE()) THEN 'EXPIRED' ELSE 'NOT-EXPIRED' END ExpiryStatus, "
         + "  (SELECT COUNT(Id) FROM dbo.AttachmentsHistory WHERE ReferenceDocumentTypeId = h.DocumentTypeId AND ReferenceDocumentId = h.Id) AS AttachmentsCount, "
         + "  h.BrokerAgentSupCustId CommissionAgentId, CommissionAgent.CompanyName CommissionAgentName, h.CommissionType, h.CommRate, h.UomScheduleIdCmRate, h.CommAmount, "
         + "  EntryUser.FirstName + ' ' + EntryUser.LastName EntryUserName, h.EntryUser, h.EntryDate, "
         + "  ModifyUser.FirstName + ' ' + ModifyUser.LastName ModifyUserName, h.ModifyUser, h.ModifyDate, "
         + "  ApprovalUser.FirstName + ' ' + ApprovalUser.LastName ApprovalUserName, h.PostUser, h.PostDate, "
         + "  CASE WHEN ISNULL(EntryUserProfile.ProfileImageFileName,'') = '' THEN CONVERT(nvarchar(max), dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91)) + 'DummyProfilePicture.jpg' "
         + "       ELSE CONVERT(nvarchar(max), dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91)) + EntryUserProfile.ProfileImageFileName END EntryUserProfileImage, "
         + "  CASE WHEN ISNULL(ModifyUserProfile.ProfileImageFileName,'') = '' THEN CONVERT(nvarchar(max), dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91)) + 'DummyProfilePicture.jpg' "
         + "       ELSE CONVERT(nvarchar(max), dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91)) + ModifyUserProfile.ProfileImageFileName END ModifyUserProfileImage, "
         + "  CASE WHEN ISNULL(ApprovalUserProfile.ProfileImageFileName,'') = '' THEN CONVERT(nvarchar(max), dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91)) + 'DummyProfilePicture.jpg' "
         + "       ELSE CONVERT(nvarchar(max), dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91)) + ApprovalUserProfile.ProfileImageFileName END ApprovalUserProfileImage, "
         + "  h.OrganizationId, h.CompanyId, h.FinancialYearId "
         + " FROM dbo.SaleOrder h "
         + " INNER JOIN dbo.InvOrderCategory OrderCategory on OrderCategory.Id = h.OrderCatagoryId "
         + " INNER JOIN SupplierCustomer Supplier on Supplier.Id = h.OrderSupCustId "
         + " INNER JOIN InvDueTerms PaymentTerms on PaymentTerms.Id = h.PaymentTermsId "
         + " INNER JOIN UserAccount EntryUser on EntryUser.ID = h.EntryUser "
         + " LEFT  JOIN SupplierCustomer CommissionAgent on CommissionAgent.Id = h.BrokerAgentSupCustId "
         + " LEFT  JOIN UserProfile EntryUserProfile on EntryUserProfile.UserId = EntryUser.ID "
         + " LEFT  JOIN UserAccount ModifyUser on ModifyUser.ID = h.ModifyUser "
         + " LEFT  JOIN UserProfile ModifyUserProfile on ModifyUserProfile.UserId = ModifyUser.ID "
         + " LEFT  JOIN UserAccount ApprovalUser on ApprovalUser.ID = h.PostUser "
         + " LEFT  JOIN UserProfile ApprovalUserProfile on ApprovalUserProfile.UserId = ApprovalUser.ID "
         + " WHERE h.Id = :id "
         + ") h",
           nativeQuery = true)
    List<SaleOrderModernHistoryRow> saleOrderModernHistory(@Param("organizationId") Integer organizationId,
                                                           @Param("companyId") Integer companyId,
                                                           @Param("id") Integer id);

    /**
     * Sp_SaleOrder_GetAllMethod @Activity='SaleOrderModernHistory' - SECOND result set (detail lines of #SaleOrder;
     * the temp table is replaced by SaleOrder h WHERE h.Id = :id).
     */
    @Query(value = "SELECT d.Id, h.Id AS OrderId, i.ItemName, PackUom.UOMCode PackUom, d.OrderItemQty, d.NetWeight, d.OrderItemRate, "
         + " RateUom.UOMCode RateUom, d.Amount, Job_Lot.JobLotDescription, d.CityArea, d.Crop, d.BagWeight, d.BagPrice, d.OrderRemarks "
         + "FROM dbo.SaleOrderDetail d "
         + " INNER JOIN dbo.SaleOrder h on h.Id = d.SaleOrderId and d.ActionTypeId <> 3 "
         + " INNER JOIN vItemAllocation i on i.Id = d.OrderItemId AND i.CompanyId = h.CompanyId "
         + " INNER JOIN V_UomScheduleAndUom PackUom on d.OrderItemUOMId = PackUom.Id "
         + " INNER JOIN V_UomScheduleAndUom RateUom on d.OrderItemRateUOMId = RateUom.Id "
         + " INNER JOIN JobLot Job_Lot on Job_Lot.Id = d.JobLotId "
         + "WHERE h.Id = :id",
           nativeQuery = true)
    List<SaleOrderModernHistoryDetailRow> saleOrderModernHistoryDetail(@Param("id") Integer id);

    /**
     * Sp_SaleOrder_GetAllMethod @Activity='LoadSaleOrderForGoodsDispatchNotes'
     * (native: @PackingTypeId = (select distinct ISNULL(ConfigKey,0) from ConfigrationsAllocation ... ConfigrationsDefinitionId = 11)
     * inlined as a single-row CTE; SaleOrderDetail, V_UomScheduleAndUom).
     */
    @Query(value = "WITH cfg AS ( SELECT (select Distinct Isnull(ConfigKey,0) from ConfigrationsAllocation "
         + "   where OrganizationId = :organizationId and CompanyId = :companyId and ConfigrationsDefinitionId = 11) AS PackingTypeId ) "
         + "select s.OrderSupCustId as SupplierCustomerId, sp.CompanyName as SupplierCustomer, "
         + " 0 as RefPartyId, s.Id as OrderId, s.DocNo as OrderNo, d.Id as SaleOrderDetailId, "
         + " d.OrderItemId as ItemId, i.ItemName as Item, Uom.Equivalent as ItemUOM, "
         + " d.OrderItemUOMId as ItemUOMId, cfg.PackingTypeId as PackingTypeId, "
         + " pt.PackTypeDesc, OrderItemQty as QTY, NetWeight as Weight, "
         + " OrderItemQty as LoadQty, NetWeight as LoadWeight, "
         + " case when Isnull(d.BagWeight,0) > 0 then Isnull(d.OrderItemQty,0) * Isnull(d.BagWeight,0) else 0 end as PackingWeight, "
         + " case when Isnull(d.BagWeight,0) > 0 then Isnull(d.NetWeight,0) + Isnull(d.OrderItemQty,0) * Isnull(d.BagWeight,0) else d.NetWeight end as GrossWeight, "
         + " d.JobLotId, j.JobLotDescription, d.OrderRemarks as Remarks, d.Crop as CropYear, s.DeliveryTerm, d.CityId, d.CityArea, i.ItemCodeNew ItemCode "
         + "from SaleOrder s "
         + " CROSS JOIN cfg "
         + " inner join SaleOrderDetail d on s.Id = d.SaleOrderId and d.ActionTypeId <> 3 and ISNULL(d.isCanceled,0) = 0 "
         + " inner join SupplierCustomer sp on s.OrderSupCustId = sp.Id "
         + " inner join Item i on d.OrderItemId = i.Id "
         + " inner join V_UomScheduleAndUom Uom on d.OrderItemUOMId = Uom.Id "
         + " inner join JobLot j on d.JobLotId = j.Id "
         + " left  join InvPackingType pt on cfg.PackingTypeId = pt.Id "
         + "where s.Id = :id "
         + "and s.OrganizationId = :organizationId "
         + "and s.CompanyId = :companyId",
           nativeQuery = true)
    List<SaleOrderForGoodsDispatchNotesRow> loadSaleOrderForGoodsDispatchNotes(@Param("organizationId") Integer organizationId,
                                                                               @Param("companyId") Integer companyId,
                                                                               @Param("id") Integer id);

    /** Sp_SaleOrder_GetAllMethod @Activity='ItemsBySaleOrderId' (native: SaleOrderDetail). */
    @Query(value = "select Distinct i.Id, i.ItemName, Coalesce(i.ItemCodeNew,'') ItemCode "
         + "from SaleOrder h "
         + " Inner join SaleOrderDetail d on d.SaleOrderId = h.Id and Coalesce(d.ActionTypeId,0) <> 3 "
         + " Inner Join Item i on i.Id = d.OrderItemId "
         + "where h.OrganizationId = :organizationId "
         + "and h.CompanyId = :companyId "
         + "and Coalesce(h.ActionId,0) <> 3 "
         + "and h.Id = :id",
           nativeQuery = true)
    List<SaleOrderItemRow> itemsBySaleOrderId(@Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("id") Integer id);
}
