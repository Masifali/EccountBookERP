package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvGrn;
import com.mst.projections.InvGrnDefaultConfigRow;
import com.mst.projections.InvGrnDetailForContractorWagesRow;
import com.mst.projections.InvGrnFormHistoryRow;
import com.mst.projections.InvGrnIdDocNoRow;
import com.mst.projections.InvGrnIdRefDocTypeRow;
import com.mst.projections.InvGrnLoadForStoreInvoiceDemandRow;
import com.mst.projections.InvGrnLoadForStoreInvoiceOrderRow;
import com.mst.projections.InvGrnMarketHistoryRow;
import com.mst.projections.InvGrnPendingForContractorWagesRow;
import com.mst.projections.InvGrnPendingForPurchaseInvoiceRow;
import com.mst.projections.InvGrnPreviousGpDataRow;
import com.mst.projections.InvGrnPurchaseBreakUpRow;
import com.mst.projections.InvGrnReadByIdRow;
import com.mst.projections.InvGrnReceivedBalanceWeightRow;
import com.mst.projections.InvGrnStoreRegisterRow;

/**
 * Repository for dbo.InvGrn (GRN header).
 *
 * Covers Sp_InvGRN_GetAllMethod (all 20 @Activity branches), Sp_InvGrn_Insert, Sp_InvGrn_Update and
 * USp_InvGrnStore_Register.
 *
 * Sp_InvGrn_Insert pre-insert logic (kept verbatim in {@link #spInvGrnInsert}):
 * - dbo.FunCheckValidFinancialYear(@OrganizationId,@CompanyId,@FinancialYearId,@DocDate) must be 1;
 * - DocNo = ISNULL(MAX(DocNo),0)+1 within OrganizationId / CompanyId / DocumentTypeId / FinancialYearId / BranchesId
 *   (see {@link #maxDocNo}); the @DocNo parameter is ignored;
 * - from GatePassInward (@InwardGatePassId): RefDocumentTypeId, TransType = OtherSupCust, bilty Freight,
 *   ActionIdForSpecialApproval; factory weight = SUM(WbTransactions.NetWbWeight) of the gate pass;
 * - validations: special-approval pending (RefDocumentType 41), lab rate / weight applied (config 331),
 *   duplicate GRN per gate pass / supplier (except DocumentType 1602/1858, RefDocumentType 105, BaseDocumentType 2),
 *   DeliveryTerm required and weigh-bridge weight = FactoryWeight (except DocumentType 48/137/169/701/217/1602),
 *   SupplierCustomerId required, Status required for RefDocumentType 110/111 + DocumentType 165/167,
 *   GRN date = GP date (config 5616), freight voucher required / approved (config 327), SupplierDispatch reuse;
 * - side effects: UPDATE GatePassInward / WbTransactions SupplierCustomerId (RefDocumentType 105/106), UPDATE GatePassInward
 *   Status/FactoryWeight/VehicleNo/... (RefDocumentType 110/111), UPDATE GatePassInward SupplierDispatchId / VehicleNo;
 * - Status = 'Accepted' unless DocumentType 165/167; Id = ISNULL(MAX(Id),0)+1 (see {@link #maxId()});
 *   UserLogId defaults to UserAccount.LastUserLogId of @EntryUser; EXEC USP_UserAudit_Insert (ActionId 1); SELECT @Id.
 *
 * Sp_InvGrn_Update logic (kept verbatim in {@link #spInvGrnUpdate}): financial-year validation, no change of financial
 * year, approved GRN cannot be updated (config 305), weigh-bridge weight check (DocumentType 46/143), supplier / gate
 * pass duplicate, GatePassInward updates as in insert, GRN referred in a purchase invoice cannot be updated, GP date
 * check (config 5616), freight voucher checks, SupplierDispatch reuse, Status = 'Accepted' unless 165/167, UPDATE InvGrn
 * (see {@link #updateById}), DELETE InvGrnDetail, DELETE InvGrnDetailEmptyBags / InvGrnPurchaseBreakUp
 * (DocumentType 46/143/165/167/169), DELETE InvGrnDetailEmptyBags (137/217), EXEC USP_UserAudit_Insert (ActionId 2).
 */
@Repository
public interface InvGrnRepository extends JpaRepository<InvGrn, Integer> {

    /** Sp_InvGrn_Insert: Id = ISNULL(MAX(CONVERT(INT,Id)),0)+1 (app-assigned id). */
    @Query("select coalesce(max(e.id),0) from InvGrn e")
    Integer maxId();

    /** Sp_InvGrn_Insert: DocNo = ISNULL(MAX(DocNo),0)+1 within organization / company / document type / financial year / branch. */
    @Query("select coalesce(max(e.docNo),0) from InvGrn e WHERE e.companyId = :companyId AND e.organizationId = :organizationId "
         + "AND e.documentTypeId = :documentTypeId AND e.financialYearId = :financialYearId AND (:branchesId IS NULL OR e.branchesId = :branchesId)")
    Integer maxDocNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId);

    // ---------------------------------------------------------------- Sp_InvGRN_GetAllMethod

    /** Sp_InvGRN_GetAllMethod @Activity='GrnPrevoiusDataOfSelectedGP' (@RecId default null). */
    @Query(nativeQuery = true, value =
            "SELECT d.ItemUomId, U.UOMCode PackUom, u.Equivalent PackUomEquivalent, Sum(d.ItemQty) ItemQty "
            + "FROM InvGrn h "
            + "Inner Join InvGrnDetail d ON h.Id = d.InvGrnId "
            + "INNER JOIN Item ON d.ItemId = Item.Id "
            + "INNER JOIN V_UomScheduleAndUom AS U ON U.Id = d.ItemUomId "
            + "where (h.OrganizationId = :organizationId) "
            + "and (h.CompanyId = :companyId) "
            + "AND (h.InwardGatePassId = :gatePassInwardId) "
            + "ANd (:recId is null Or h.Id <> :recId) "
            + "Group By d.ItemUomId, U.UOMCode, u.Equivalent "
    )
    List<InvGrnPreviousGpDataRow> grnPrevoiusDataOfSelectedGP(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("gatePassInwardId") Integer gatePassInwardId, @Param("recId") Integer recId);

    /** Sp_InvGRN_GetAllMethod @Activity='GetPendingGrnForPurchaseInvoice' (@Id, @SupplierCustomerId, @BranchesId default null). */
    @Query(nativeQuery = true, value =
            "SELECT InvGrn.Id, InvGrn.DocDate, InvGrn.DocNo, InvGrn.GpNo, InvGrn.VehicleNo, "
            + " InvGrn.BiltyNo, SupplierCustomer.CompanyName AS SupplierCustomer, "
            + " InvGrnDetail.PurchaseOrderId, InvGrn.SupplierCustomerId, "
            + " InvGrnDetail.ItemQty, Item.ItemName, pod.OrderItemRate, "
            + " PoH.DocNo AS PurchaseOrder, Item.ItemCode, U.UOMCode, "
            + " InvGrnDetail.WarehouseId "
            + "FROM SupplierCustomer "
            + "INNER JOIN InvGrn ON SupplierCustomer.Id = InvGrn.SupplierCustomerId "
            + "INNER JOIN InvGrnDetail ON InvGrn.Id = InvGrnDetail.InvGrnId "
            + "INNER JOIN Item ON InvGrnDetail.ItemId = Item.Id "
            + "INNER JOIN V_UomScheduleAndUom AS U ON U.Id = InvGrnDetail.ItemUomId "
            + "LEFT JOIN PurchaseOrderDetail AS pod "
            + "INNER JOIN PurchaseOrder AS PoH ON pod.PurchaseOrderId = PoH.Id ON InvGrnDetail.PurchaseOrderId = pod.Id "
            + "LEFT JOIN InvPurchaseInvoiceDetail ON InvGrn.Id = InvPurchaseInvoiceDetail.InvGrnId "
            + "where (InvGrn.OrganizationId = :organizationId) "
            + "and (InvGrn.CompanyId = :companyId) "
            + "AND (:id IS NULL or dbo.InvGrn.Id = :id) "
            + "AND (:supplierCustomerId IS NULL OR dbo.InvGrn.SupplierCustomerId = :supplierCustomerId) "
            + "AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
            + "and not EXISTS (select 1 "
            + " from dbo.InvPurchaseInvoiceDetail d "
            + " Inner join InvPurchaseInvoice h on h.Id = d.PurchaseOrderId and ISNULL(d.ActionTypeId,0) <> 3 "
            + " Where h.OrganizationId = :organizationId "
            + " and h.CompanyId = :companyId "
            + " AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
            + " and d.InvGrnId = InvGrn.Id) "
    )
    List<InvGrnPendingForPurchaseInvoiceRow> getPendingGrnForPurchaseInvoice(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("id") Integer id, @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("branchesId") Integer branchesId);

    /**
     * Sp_InvGRN_GetAllMethod @Activity='ReadByID'. Take first.
     * The #ReadById / #updateInvFreightVoucher / #WbUpdateData temp-table pipeline is rewritten as one statement:
     * invoice no, freight voucher sums, weigh-bridge sums (STRING_AGG ticket nos) and order category come from
     * OUTER APPLY (TOP 1) sub-selects, the defaults (0 / '' / CarriageAmount) are kept for unmatched rows.
     */
    @Query(nativeQuery = true, value =
            "SELECT InvGrn.Id, InvGrn.TransporterSupCustId, InvGrn.DocumentTypeId, InvGrn.DocDate, InvGrn.DocNo, InvGrn.SupplierCustomerId, InvGrn.GpNo, "
            + " InvGrn.ReferencePartyId, InvGrn.StockPartyId, InvGrn.SupplierReference, InvGrn.ReferenceDocNo, InvGrn.PartyWeight, InvGrn.FactoryWeight, "
            + " InvGrn.VehicleType, InvGrn.VehicleNo, InvGrn.TransporterDocRef, InvGrn.BillCalculateTypeId, InvGrn.TransporterId, InvGrn.OtherCharges, "
            + " InvGrn.RemarksHeader, InvGrn.ScreenName, InvGrn.IsApproved, InvGrn.EntryDate, InvGrn.EntryUser, InvGrn.ModifyDate, InvGrn.ModifyUser, "
            + " InvGrn.PostDate, InvGrn.PostUser, InvGrn.OrganizationId, InvGrn.CompanyId, InvGrn.BranchesId, InvGrn.ProjectsId, InvGrn.BiltyNo, "
            + " InvGrn.InwardGatePassId, InvGrn.ActionId, InvGrn.FinancialYearId, InvGrn.DeliveryTerm, InvGrn.Status, InvGrn.ScaleKart, InvGrn.GhallaMandiId, "
            + " InvGrn.BillWeightExceedsStockWeight, InvGrn.GrnTypeId, InvGrn.ReturnableDate, InvGrn.ReqestedById, InvGrn.TransType, "
            + " 0 AS LabWeightCutOnId, "
            + " InvGrn.AddWages, InvGrn.ScaleShortWeightApply, InvGrn.SupplierShortWeightApply, InvGrn.RateCut, InvGrn.RateCutRemarks, InvGrn.AccessWeight, "
            + " InvGrn.AttachmentsValues, InvGrn.CustomAttachmentsValues, InvGrn.DeductionPolicyForGrnId, InvGrn.SupplierDispatchId, "
            + " InvGrn.FreightDeduction, InvGrn.BiltyFreight, InvGrn.AdvanceByFactoryFreight, InvGrn.AdvanceByPartyFreight, "
            + " CASE WHEN FV.GatePassId IS NOT NULL AND ISNULL(FV.PaidAmount,0) > 0 THEN FV.PaidAmount ELSE InvGrn.CarriageAmount END AS CarriageAmount, "
            + " CASE WHEN WB.ReferenceDocNoId IS NOT NULL AND ISNULL(FV.FreightVoucherId,0) > 0 THEN CAST(WB.WbCharges AS FLOAT) ELSE CAST(0 AS FLOAT) END AS ScaleCharges, "
            + " CASE WHEN FV.GatePassId IS NOT NULL THEN CAST(FV.ShortageAmount AS FLOAT) ELSE CAST(0 AS FLOAT) END AS ShortageAmountFV, "
            + " CASE WHEN FV.GatePassId IS NOT NULL THEN CAST(FV.DiscountAmount AS FLOAT) ELSE CAST(0 AS FLOAT) END AS DiscountAmountFV, "
            + " CASE WHEN FV.GatePassId IS NOT NULL THEN CAST(FV.ChargeToPartyAmount AS FLOAT) ELSE CAST(0 AS FLOAT) END AS ChargeToPartyAmountFV, "
            + " CASE WHEN FV.GatePassId IS NOT NULL THEN CAST(FV.OtherDeduction AS FLOAT) ELSE CAST(0 AS FLOAT) END AS OtherFreightDeduction, "
            + " InvGrn.GainLossWeight, "
            + " CASE WHEN WB.ReferenceDocNoId IS NOT NULL THEN WB.TicketNos ELSE '' END AS TicketNos, "
            + " CASE WHEN FV.GatePassId IS NOT NULL THEN FV.FreightVoucherId ELSE 0 END AS FreightId, "
            + " CASE WHEN INV.InvGrnId IS NOT NULL THEN INV.InvPurchaseInvoiceId ELSE 0 END AS InvoiceId, "
            + " CASE WHEN INV.InvGrnId IS NOT NULL THEN INV.DocNo ELSE 0 END AS InvoiceNo, "
            + " D1.DocumentTypeCode, "
            + " C1.CompanyName, "
            + " CT.Id AS TransporterIdAlt, "
            + " GP.RefDocumentTypeId, "
            + " GP.OtherSupCust AS OrderType, "
            + " Gty.LookUpName AS GrnType, "
            + " Lp.LookupName AS RequestedBy, "
            + " SDH.DocNo AS PreBillNo, "
            + " '' AS DeleteAttachmentsList, "
            + " '' AS invGrnDetail, "
            + " '' AS InvgrnDetailEmptyBagslist, "
            + " '' AS AttachmentsList, "
            + " '' AS PreviousDate, "
            + " '' AS InvGrnPurchaseBreakUplist, "
            + " '' AS InventoryTransactionsPartyProcessingForFIFOlist, "
            + " InvGrn.BaseDocumentTypeId, "
            + " CASE WHEN OC.OrderCatagoryId IS NOT NULL THEN OC.OrderCatagoryId ELSE 0 END AS OrderCatagoryId "
            + "FROM InvGrn "
            + "JOIN SupplierCustomer C1 ON InvGrn.SupplierCustomerId = C1.Id "
            + "LEFT JOIN GatePassInward GP ON InvGrn.InwardGatePassId = GP.Id "
            + "LEFT JOIN SupplierCustomer CT ON InvGrn.TransporterId = CT.GlAccountId "
            + "LEFT JOIN SupplierCustomer C2 ON InvGrn.StockPartyId = C2.Id "
            + "LEFT JOIN SupplierCustomer Crp ON InvGrn.ReferencePartyId = Crp.Id "
            + "LEFT JOIN DocumentType D1 ON InvGrn.DocumentTypeId = D1.Id "
            + "LEFT JOIN InvLookUp Lp ON InvGrn.ReqestedById = Lp.Id "
            + "LEFT JOIN LookUpsForERP Gty ON InvGrn.GrnTypeId = Gty.Id "
            + "LEFT JOIN SupplierDispatch SDH ON SDH.Id = InvGrn.SupplierDispatchId "
            + "OUTER APPLY (SELECT TOP 1 D.InvGrnId, D.InvPurchaseInvoiceId, H.DocNo "
            + " FROM InvPurchaseInvoiceDetail D JOIN InvPurchaseInvoice H ON H.Id = D.InvPurchaseInvoiceId "
            + " WHERE D.InvGrnId = InvGrn.Id) INV "
            + "OUTER APPLY (SELECT TOP 1 H.GatePassId, D.FreightVoucherId, "
            + " SUM(D.ShortageAmount) AS ShortageAmount, SUM(D.AddLessAmount) AS DiscountAmount, SUM(D.ChargeToParty) AS ChargeToPartyAmount, "
            + " SUM(D.OtherDeduction) AS OtherDeduction, SUM(D.DeductedAmount) AS DeductionAmount, SUM(D.TotalPayableAmount) AS PaidAmount "
            + " FROM InvFreightVoucher H JOIN InvFreightVoucherDetail D ON D.FreightVoucherId = H.Id "
            + " WHERE H.GatePassId = InvGrn.InwardGatePassId "
            + " GROUP BY H.GatePassId, D.FreightVoucherId) FV "
            + "OUTER APPLY (SELECT TOP 1 Wb.ReferenceDocNoId, Wb.ReferenceDocTypeId, SUM(Wb.NetWbWeight) AS NetWbWeight, SUM(Wb.WbCharges) AS WbCharges, STRING_AGG(Wb.TicketNo, ',') AS TicketNos "
            + " FROM WbTransactions Wb "
            + " WHERE Wb.ReferenceDocTypeId = InvGrn.DocumentTypeId AND Wb.ReferenceDocNoId = InvGrn.Id AND Wb.FirstWeight > 0 AND Wb.SecondWeight > 0 "
            + " GROUP BY Wb.ReferenceDocNoId, Wb.ReferenceDocTypeId) WB "
            + "OUTER APPLY (SELECT TOP 1 POH.OrderCatagoryId "
            + " FROM GatePassInward GP0 JOIN PurchaseOrder POH ON GP0.PurchaseOrderId = POH.Id AND GP0.RefDocumentTypeId = POH.DocumentTypeId "
            + " WHERE GP0.Id = InvGrn.InwardGatePassId) OC "
            + "WHERE InvGrn.Id = :id "
    )
    List<InvGrnReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_InvGRN_GetAllMethod @Activity='ReadByHeaderId_PurchaseBreakup' (@Id = InvGrnId). JPQL on InvGrnPurchaseBreakUp (batch F2). */
    @Query("SELECT b.id AS id, b.invGrnId AS invGrnId, b.inwardGatePassId AS inwardGatePassId, b.inwardBreakupId AS inwardBreakupId, "
         + "b.qty AS qty, b.uom AS UOM, b.grossWeight AS grossWeight, b.ebWeight AS ebWeight, b.ebTotal AS EBTotal, "
         + "b.supplierShortWeight AS supplierShortWeight, b.netWeight AS netWeight, b.netPackSize AS netPackSize, "
         + "b.supplierRcvdWeight AS supplierRcvdWeight, b.scaleShortage AS scaleShortage, b.billWeight AS billWeight "
         + "FROM InvGrnPurchaseBreakUp b WHERE b.invGrnId = :id")
    List<InvGrnPurchaseBreakUpRow> readByHeaderIdPurchaseBreakup(@Param("id") Integer id);

    /** Sp_InvGRN_GetAllMethod @Activity='ReadByGPId_InvGrnPurchaseBreakup' (@Id = InwardGatePassId; only DENSE_RANK = 1 rows). */
    @Query(nativeQuery = true, value =
            "WITH Data AS ( "
            + " SELECT DENSE_RANK() OVER (ORDER BY InwardGatePassId, InvGrnId) as RowNo, "
            + " [Id], [InvGrnId], [InwardGatePassId], [InwardBreakupId], [Qty], [UOM], [GrossWeight], [EbWeight], [EBTotal], [NetWeight], SupplierShortWeight, "
            + " NetPackSize, SupplierRcvdWeight, ScaleShortage, BillWeight "
            + " FROM [dbo].[InvGrnPurchaseBreakUp] gg "
            + " where gg.InwardGatePassId = :id "
            + ") "
            + "Select [Id], [InvGrnId], [InwardGatePassId], [InwardBreakupId], [Qty], [UOM], [GrossWeight], [EbWeight], [EBTotal], [NetWeight], SupplierShortWeight, "
            + " NetPackSize, SupplierRcvdWeight, ScaleShortage, BillWeight "
            + "from Data "
            + "where RowNo = 1 "
    )
    List<InvGrnPurchaseBreakUpRow> readByGPIdInvGrnPurchaseBreakup(@Param("id") Integer id);

    /** Sp_InvGRN_GetAllMethod @Activity='GenerateInvGrnCode' — returns ISNULL(MAX(DocNo),0)+1 (output column DocNo). */
    @Query("SELECT COALESCE(MAX(g.docNo),0)+1 FROM InvGrn g WHERE g.organizationId = :organizationId AND g.companyId = :companyId "
         + "AND g.documentTypeId = :documentTypeId AND g.financialYearId = :financialYearId AND (:branchesId IS NULL OR g.branchesId = :branchesId)")
    Integer generateInvGrnCode(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId);

    /** Sp_InvGRN_GetAllMethod @Activity='GetGRNIdByDocNo' (@FinancialYearId, @BranchesId default null). */
    @Query("SELECT h.id AS id, gp.refDocumentTypeId AS refDocumentTypeId FROM InvGrn h LEFT JOIN InwardGatePass gp ON h.inwardGatePassId = gp.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.docNo = :docNo AND h.documentTypeId = :documentTypeId "
         + "AND (:financialYearId IS NULL OR h.financialYearId = :financialYearId) AND (:branchesId IS NULL OR h.branchesId = :branchesId) "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoice hh JOIN InvPurchaseInvoiceDetail dd ON hh.id = dd.invPurchaseInvoiceId "
         + "WHERE hh.organizationId = :organizationId AND hh.companyId = :companyId AND (:branchesId IS NULL OR hh.branchesId = :branchesId) AND dd.invGrnId = h.id)")
    List<InvGrnIdRefDocTypeRow> getGRNIdByDocNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("docNo") Integer docNo, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId);

    /**
     * Sp_InvGRN_GetAllMethod @Activity='GRNFormHistory' (all filters default null; @ActionId 1 = referred in invoice,
     * 2 = not referred, null = all). The #TmpBaseDocTypes / #GrnHis temp tables and their UPDATE steps are rewritten as a
     * CTE plus OUTER APPLY / scalar sub-selects. Note: the procedure stores TicketNo into an int column, so TicketNo is int.
     */
    @Query(nativeQuery = true, value =
            "WITH GrnHis AS ( "
            + " SELECT ROW_NUMBER () Over (Order By GrH.DocDate, GrH.DocNo) AS RecordNo, "
            + "  GrH.Id, poh.Id OrderId, GP.OtherSupCust as OrderType, "
            + "  poh.DocNo OrderNo, GrH.DocDate, GrH.InwardGatePassId, GrH.ReferencePartyId, GrH.ScreenName, "
            + "  GrH.DocNo, GrH.DocumentTypeId, GrH.DeliveryTerm, Sp.CompanyName as SupplierName, "
            + "  GrH.GpNo, GrH.VehicleNo, GrH.BiltyNo, Grh.AddWages, "
            + "  GrH.FactoryWeight, GrH.PartyWeight, "
            + "  Coa.AccountTitle AS Transporter, "
            + "  GrH.CarriageAmount, GrH.RemarksHeader, "
            + "  Grh.ScaleShortWeightApply, Grh.SupplierShortWeightApply, "
            + "  Grh.RateCut, Grh.RateCutRemarks, Grh.AccessWeight, "
            + "  grh.EntryDate, "
            + "  Usr.FirstName + ' ' + Usr.LastName as EntryUser, "
            + "  grh.ModifyDate, "
            + "  Musr.FirstName + ' ' + Musr.LastName as ModifyUser, "
            + "  grh.PostDate, "
            + "  Ausr.FirstName + ' ' + Ausr.LastName as ApprovedUser, "
            + "  rq.LookUpName as RequestedBy, "
            + "  (select count(Id) from GatePassInwardDetail where GatePassInwardId = grh.InwardGatePassId) as DetailIdsCount, "
            + "  Gp.RefDocumentTypeId, grh.BaseDocumentTypeId "
            + " FROM InvGrn AS GrH with(nolock) "
            + "  inner join SupplierCustomer AS Sp with(nolock) ON GrH.SupplierCustomerId = Sp.Id "
            + "  inner join UserAccount Usr on GrH.EntryUser = Usr.ID "
            + "  left join UserAccount Musr on GrH.ModifyUser = Musr.ID "
            + "  left join UserAccount Ausr on GrH.PostUser = Ausr.ID "
            + "  left join GatePassInward GP on GrH.InwardGatePassId = GP.Id "
            + "  Left join ChartofAccount Coa on GrH.TransporterId = Coa.Id "
            + "  left join purchaseorder poh on gp.purchaseorderid = poh.id and GP.RefDocumentTypeId in (41,1600) "
            + "  left join LookUpsForERP Rq on grh.ReqestedById = rq.Id "
            + " Where grh.OrganizationId = :organizationId "
            + "  and (:companyId is null or Grh.CompanyId = :companyId) "
            + "  and (:documentTypeId is null or GrH.DocumentTypeId = :documentTypeId) "
            + "  and (:canViewAllRecord is null or :canViewAllRecord = 1 or (:canViewAllRecord = 0 and GrH.EntryUser = :entryUser)) "
            + "  and (:financialYearId is null or grh.FinancialYearId = :financialYearId) "
            + "  and (:branchesId is null or grh.BranchesId = :branchesId) "
            + "  AND (:fromDate IS NULL OR GrH.DocDate >= :fromDate) "
            + "  AND (:toDate IS NULL OR GrH.DocDate < DATEADD(DAY,1,:toDate)) "
            + "  AND (:entryFromDate IS NULL OR GrH.EntryDate >= :entryFromDate) "
            + "  AND (:entryToDate IS NULL OR GrH.EntryDate < DATEADD(DAY,1,:entryToDate)) "
            + "  AND (:modifyFromDate IS NULL OR GrH.ModifyDate >= :modifyFromDate) "
            + "  AND (:modifyToDate IS NULL OR GrH.ModifyDate < DATEADD(DAY,1,:modifyToDate)) "
            + "  AND (:approvedFromDate IS NULL OR GrH.PostDate >= :approvedFromDate) "
            + "  AND (:approvedToDate IS NULL OR GrH.PostDate < DATEADD(DAY,1,:approvedToDate)) "
            + "  and (:supplierCustomerId is null or grh.SupplierCustomerId = :supplierCustomerId) "
            + "  and (:grnNoF is null or grh.DocNo >= :grnNoF) "
            + "  and (:grnNoT is null or grh.DocNo <= :grnNoT) "
            + "  and (:reqestedById is null or grh.ReqestedById = :reqestedById) "
            + "  AND (:baseDocumentTypeId IS NULL OR grh.BaseDocumentTypeId = :baseDocumentTypeId) "
            + "  and (:baseDocumentTypeIds IS NULL OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:baseDocumentTypeIds, ',') WHERE data = grh.BaseDocumentTypeId)) "
            + ") "
            + "SELECT R.RecordNo, R.Id, R.OrderId, R.OrderType, R.OrderNo, R.DocDate, R.InwardGatePassId, R.ReferencePartyId, R.ScreenName, "
            + " R.DocNo, R.DocumentTypeId, R.DeliveryTerm, R.SupplierName, R.GpNo, R.VehicleNo, R.BiltyNo, "
            + " CASE WHEN WG.Id IS NULL THEN 0 ELSE WG.Id END AS WagesId, "
            + " CASE WHEN WG.Id IS NULL THEN 0 ELSE WG.DocNo END AS WagesNo, "
            + " R.AddWages, R.FactoryWeight, R.PartyWeight, R.Transporter, R.CarriageAmount, R.RemarksHeader, "
            + " R.EntryDate, R.EntryUser, R.ModifyDate, R.ModifyUser, R.PostDate, R.ApprovedUser, R.RequestedBy, "
            + " CASE WHEN WB.Id IS NULL THEN 0 ELSE WB.Id END AS WbTicketId, "
            + " CASE WHEN WB.Id IS NULL THEN 0 ELSE CAST(WB.TicketNo AS int) END AS TicketNo, "
            + " CASE WHEN INV.InvGrnId IS NULL THEN 0 ELSE INV.DocNo END AS InvoiceNo, "
            + " (SELECT COUNT(da.Id) FROM DMSAttachments da WHERE da.RefDocumentTypeId = R.DocumentTypeId AND da.RefDocumentNo = R.Id) AS NoOfAttachments, "
            + " (SELECT COUNT(ah.Id) FROM AttachmentsHistory ah WHERE ah.ActionTypeId <> 3 AND ah.ReferenceDocumentTypeId = R.DocumentTypeId AND ah.ReferenceDocumentId = R.Id) AS AttachmentsCount, "
            + " R.DetailIdsCount, R.ScaleShortWeightApply, R.SupplierShortWeightApply, R.RateCut, R.RateCutRemarks, R.AccessWeight, R.RefDocumentTypeId, "
            + " R.BaseDocumentTypeId, BD.Description BaseDocumentType "
            + "FROM GrnHis R "
            + "LEft Join BaseDocumentType BD on BD.Id = R.BaseDocumentTypeId and BD.DocumentTypeId = R.DocumentTypeId "
            + "OUTER APPLY (SELECT TOP 1 WB0.Id, WB0.TicketNo FROM InvGrnDetail D INNER JOIN WbTransactions WB0 ON D.WbTicketId = WB0.Id WHERE R.DocumentTypeId = 217 AND D.InvGrnId = R.Id) WB "
            + "OUTER APPLY (SELECT TOP 1 d.InvGrnId, H.DocNo FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice H ON d.InvPurchaseInvoiceId = H.Id WHERE d.InvGrnId = R.Id) INV "
            + "OUTER APPLY (SELECT TOP 1 wbh.Id, wbh.DocNo FROM InvContractorWagesBillHeader wbh WHERE wbh.RefDocNoId = R.Id AND wbh.RefDocumentTypeId = R.DocumentTypeId) WG "
            + "Where (:noOfRecords is null or R.RecordNo <= :noOfRecords) "
            + "AND ((:actionId = 1 AND ISNULL(CASE WHEN INV.InvGrnId IS NULL THEN 0 ELSE INV.DocNo END,0) > 0) "
            + " OR (:actionId = 2 AND ISNULL(CASE WHEN INV.InvGrnId IS NULL THEN 0 ELSE INV.DocNo END,0) = 0) "
            + " OR (:actionId IS NULL)) "
            + "Order BY R.RecordNo DESC "
    )
    List<InvGrnFormHistoryRow> grnFormHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("canViewAllRecord") Boolean canViewAllRecord,
            @Param("entryUser") Integer entryUser, @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId,
            @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate,
            @Param("entryFromDate") LocalDate entryFromDate, @Param("entryToDate") LocalDate entryToDate,
            @Param("modifyFromDate") LocalDate modifyFromDate, @Param("modifyToDate") LocalDate modifyToDate,
            @Param("approvedFromDate") LocalDate approvedFromDate, @Param("approvedToDate") LocalDate approvedToDate,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("grnNoF") Integer grnNoF, @Param("grnNoT") Integer grnNoT,
            @Param("reqestedById") Integer reqestedById, @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
            @Param("baseDocumentTypeIds") String baseDocumentTypeIds, @Param("noOfRecords") Integer noOfRecords,
            @Param("actionId") Integer actionId);

    /** Sp_InvGRN_GetAllMethod @Activity='HistoryForMarketGrn' (@CanViewAllRecord is compared to 1/0; others default null). */
    @Query(nativeQuery = true, value =
            "Select * from ( "
            + " SELECT DISTINCT "
            + "  ROW_NUMBER () Over (Order By GrH.DocDate Desc) RecordNo, "
            + "  GrH.Id, poh.Id OrderNo, GrH.DocDate, GrH.DocNo, GrH.DocumentTypeId, "
            + "  GrH.ScreenName, Sp.CompanyName as SupplierName, GrH.GpNo, GrH.VehicleNo, "
            + "  GrH.BiltyNo, GrH.FactoryWeight, GrH.PartyWeight, Coa.AccountTitle, "
            + "  gp.Freight, GP.NetPaid, GrH.RemarksHeader, "
            + "  Usr.FirstName + ' ' + Usr.LastName as UserName, "
            + "  (select count(Attachment) from DMSAttachments where RefDocumentTypeId = GrH.DocumentTypeId and RefDocumentNo = GrH.Id) as NoOfAttachments "
            + " FROM InvGrn AS GrH with(nolock) "
            + " INNER JOIN SupplierCustomer AS Sp with(nolock) ON GrH.SupplierCustomerId = Sp.Id "
            + " inner join UserAccount Usr on GrH.EntryUser = Usr.ID "
            + " left join GatePassInward GP on GrH.InwardGatePassId = GP.Id "
            + " Left join ChartofAccount Coa on GrH.TransporterId = Coa.Id "
            + " inner join purchaseorder poh on gp.purchaseorderid = poh.id and GP.RefDocumentTypeId = 104 "
            + " Where grh.OrganizationId = :organizationId "
            + "  and (:companyId is null or Grh.CompanyId = :companyId) "
            + "  and (:documentTypeId is null or GrH.DocumentTypeId = :documentTypeId) "
            + "  and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and GrH.EntryUser = :entryUser)) "
            + "  and (:financialYearId is null or GrH.FinancialYearId = :financialYearId) "
            + "  AND (:branchesId IS NULL OR grh.BranchesId = :branchesId) "
            + ") a Where (:noOfRecords is null or a.RecordNo <= :noOfRecords) "
    )
    List<InvGrnMarketHistoryRow> historyForMarketGrn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("canViewAllRecord") Boolean canViewAllRecord,
            @Param("entryUser") Integer entryUser, @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId,
            @Param("noOfRecords") Integer noOfRecords);

    /** Sp_InvGRN_GetAllMethod @Activity='GetGrnNoNotInInvoice' (@BranchesId default null). */
    @Query("SELECT g.id AS id, g.docNo AS docNo FROM InvGrn g WHERE g.organizationId = :organizationId AND g.companyId = :companyId "
         + "AND g.documentTypeId = :documentTypeId AND (:branchesId IS NULL OR g.branchesId = :branchesId) "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail d JOIN InvPurchaseInvoice h ON h.id = d.invPurchaseInvoiceId "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND (:branchesId IS NULL OR h.branchesId = :branchesId) "
         + "AND COALESCE(d.invGrnId,0) = g.id)")
    List<InvGrnIdDocNoRow> getGrnNoNotInInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId);

    /** Sp_InvGRN_GetAllMethod @Activity='GrnLoadForStoreInvoiceDemondBase' (@SupplierCustomerId, @fromDate, @toDate, @BranchesId default null). */
    @Query(nativeQuery = true, value =
            "SELECT distinct "
            + " InvGrn.Id, InvGrn.DocDate, InvGrn.DocNo, "
            + " InvGrn.GpNo, InvGrn.VehicleNo, InvGrn.BiltyNo, "
            + " SupplierCustomer.CompanyName AS SupplierName, "
            + " dh.DocNo as DemandNo, InvGrn.SupplierCustomerId, "
            + " InvGrnDetail.ItemQty, Item.ItemName, Item.ItemCode, "
            + " U.UOMCode, w.WareHouseName, bb.BranchName "
            + "FROM SupplierCustomer "
            + "INNER JOIN InvGrn ON SupplierCustomer.Id = InvGrn.SupplierCustomerId "
            + "INNER JOIN InvGrnDetail ON InvGrn.Id = InvGrnDetail.InvGrnId "
            + "INNER JOIN Item ON InvGrnDetail.ItemId = Item.Id "
            + "INNER JOIN V_UomScheduleAndUom AS U ON U.Id = InvGrnDetail.ItemUomId "
            + "INNER JOIN InvPurchaseDemandHeader dh on InvGrnDetail.InvPurchasedemondId = dh.Id "
            + "INNER JOIN InvPurchaseDemandDetail dd on dh.Id = dd.InvPurchaseDemandHeaderId and InvGrnDetail.ItemId = dd.ItemId "
            + "INNER JOIN InvWareHouse w on InvGrnDetail.WarehouseId = w.Id "
            + "Left Join Branches bb on bb.Id = InvGrn.BranchesId "
            + "where InvGrn.OrganizationId = :organizationId "
            + "and InvGrn.CompanyId = :companyId "
            + "and InvGrn.DocumentTypeId = :documentTypeId "
            + "and (:supplierCustomerId is null or InvGrn.SupplierCustomerId = :supplierCustomerId) "
            + "and (:fromDate is null or InvGrn.DocDate >= :fromDate) "
            + "and (:toDate is null or InvGrn.DocDate <= :toDate) "
            + "AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
            + "and not EXISTS (select d.InvGrnId "
            + " from InvPurchaseInvoice h "
            + " inner join InvPurchaseInvoiceDetail d on h.Id = d.InvPurchaseInvoiceId "
            + " where h.OrganizationId = :organizationId "
            + " and h.CompanyId = :companyId "
            + " AND (:branchesId IS NULL OR h.BranchesId = :branchesId) "
            + " AND d.InvGrnId = invgrn.Id) "
    )
    List<InvGrnLoadForStoreInvoiceDemandRow> grnLoadForStoreInvoiceDemondBase(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate,
            @Param("branchesId") Integer branchesId);

    /** Sp_InvGRN_GetAllMethod @Activity='GrnLoadForStoreInvoicePurchaseOrderBase' (@OrderTaxable etc. default null). */
    @Query(nativeQuery = true, value =
            "SELECT InvGrn.Id, InvGrn.DocDate, InvGrn.DocNo, InvGrn.GpNo, gp.RefDocumentTypeId, "
            + " Case "
            + "  when GP.RefDocumentTypeId = 41 then 'Purchase Order' "
            + "  when GP.RefDocumentTypeId = 104 then 'Supply Order' "
            + "  when GP.RefDocumentTypeId = 105 then 'Market Purchase' "
            + "  when GP.RefDocumentTypeId = 232 then 'Import PO' "
            + "  when GP.RefDocumentTypeId = 106 then 'Gate Purchase' "
            + "  when InvGrn.DocumentTypeId IN (217) then 'Purchase From Party Processing' "
            + "  when GP.RefDocumentTypeId = 1500 then 'Purchase_Order' "
            + "  when GP.RefDocumentTypeId = 700 then 'Purchase_Order_Pm' "
            + "  WHEN GP.RefDocumentTypeId IN (98) THEN 'Sale Return' "
            + "  WHEN GP.RefDocumentTypeId IN (52) THEN 'General' "
            + "  else 'Other' END OrderType, "
            + " InvGrn.VehicleNo, InvGrn.BiltyNo, SupplierCustomer.CompanyName AS SupplierName, "
            + " poh.DocNo as OrderNo, InvGrn.SupplierCustomerId, "
            + " InvGrnDetail.ItemQty, Item.ItemName, Item.ItemCode, U.UOMCode, w.WareHouseName, "
            + " InvGrnDetail.PurchaseOrderId, InvGrn.BillCalculateTypeId, "
            + " CASE WHEN InvGrn.BillCalculateTypeId = 1 THEN 'On Weight' ELSE 'On Qty' END AS BillCalculateType, bb.BranchName, "
            + " InvGrnDetail.ItemConditionId, VIC.ConditionStatus "
            + "FROM SupplierCustomer "
            + " INNER JOIN InvGrn ON SupplierCustomer.Id = InvGrn.SupplierCustomerId "
            + " INNER JOIN InvGrnDetail ON InvGrn.Id = InvGrnDetail.InvGrnId "
            + " INNER JOIN Item ON InvGrnDetail.ItemId = Item.Id "
            + " INNER JOIN V_UomScheduleAndUom AS U ON U.Id = InvGrnDetail.ItemUomId "
            + " INNER JOIN PurchaseOrder poh on InvGrnDetail.PurchaseOrderId = poh.Id "
            + " INNER JOIN PurchaseOrderDetail pod on poh.Id = pod.PurchaseOrderId and InvGrnDetail.PurchaseOrderDetailId = Pod.Id and InvGrnDetail.ItemId = pod.OrderItemId "
            + " INNER JOIN InvWareHouse w on InvGrnDetail.WarehouseId = w.Id "
            + " Left join GatePassInward gp on gp.Id = InvGrn.InwardGatePassId "
            + " Left Join Branches bb on bb.Id = InvGrn.BranchesId "
            + " LEFT Join V_ItemCondition VIC on VIC.Id = InvGrnDetail.ItemConditionId "
            + "where InvGrn.OrganizationId = :organizationId "
            + "and InvGrn.CompanyId = :companyId "
            + "and InvGrn.DocumentTypeId = :documentTypeId "
            + "and (:supplierCustomerId is null or InvGrn.SupplierCustomerId = :supplierCustomerId) "
            + "and (:fromDate is null or CAST(InvGrn.DocDate AS DATE) >= CAST(:fromDate AS DATE)) "
            + "and (:toDate is null or CAST(InvGrn.DocDate AS DATE) <= CAST(:toDate AS DATE)) "
            + "AND (:orderTaxable is null or ISNULL(poh.OrderTaxable,0) = :orderTaxable) "
            + "AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
            + "and NOT EXISTS (select 1 "
            + " from InvPurchaseInvoice h "
            + " inner join InvPurchaseInvoiceDetail d on h.Id = d.InvPurchaseInvoiceId "
            + " where h.OrganizationId = dbo.InvGrn.OrganizationId "
            + " and h.CompanyId = dbo.InvGrn.CompanyId "
            + " AND (:branchesId IS NULL OR h.BranchesId = :branchesId) "
            + " and d.InvGrnId = dbo.InvGrn.Id) "
    )
    List<InvGrnLoadForStoreInvoiceOrderRow> grnLoadForStoreInvoicePurchaseOrderBase(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate,
            @Param("orderTaxable") Boolean orderTaxable, @Param("branchesId") Integer branchesId);

    /** Sp_InvGRN_GetAllMethod @Activity='PendingGrnForContractorWages'. */
    @Query(nativeQuery = true, value =
            "Select gh.Id, gh.DocumentTypeId, dt.DocumentTypeDescription, gh.DocDate, gh.DocNo, sp.CompanyName SupplierCustomer, "
            + " gp.GpSrNo GpNo, gh.VehicleNo, gh.BiltyNo, gd.GrossWeight "
            + "from InvGrn gh INNER JOIN (select InvGrnId, Sum(GrossWeight) as GrossWeight from InvGrnDetail group by InvGrnId) gd on gh.Id = gd.InvGrnId "
            + " INNER JOIN SupplierCustomer sp ON gh.SupplierCustomerId = sp.Id "
            + " INNER JOIN DocumentType dt ON gh.DocumentTypeId = dt.Id "
            + " INNER JOIN GatePassInward gp ON gh.InwardGatePassId = gp.Id "
            + "where gh.OrganizationId = :organizationId "
            + " and gh.CompanyId = :companyId "
            + " and NOT EXISTS (select 1 "
            + "  from InvContractorWagesBillHeader g "
            + "  where g.OrganizationId = gh.OrganizationId "
            + "  and g.CompanyId = gh.CompanyId "
            + "  and g.RefDocumentTypeId = gh.DocumentTypeId "
            + "  and g.RefDocNoId = gh.Id) "
    )
    List<InvGrnPendingForContractorWagesRow> pendingGrnForContractorWages(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_InvGRN_GetAllMethod @Activity='GetGrnDetialForContractorWages' (@Id = InvGrnId). */
    @Query(nativeQuery = true, value =
            "Select gh.Id, gh.DocDate, gd.ItemId, i.ItemName, gd.CropYear, j.Id JlId, j.JobLotDescription, gd.PackingTypeId, pt.PackTypeCode, iu.Equivalent, gd.GrossWeight, gd.NetBillWeight, "
            + " gd.WarehouseId WhFId, wh.WareHouseCode WarehouseFrom, gd.WarehouseId WhTId, wh.WareHouseCode WareHouseTo "
            + "from InvGrn gh INNER JOIN InvGrnDetail gd on gh.Id = gd.InvGrnId "
            + " INNER JOIN SupplierCustomer sp ON gh.SupplierCustomerId = sp.Id "
            + " INNER JOIN item i on gd.ItemId = i.Id "
            + " INNER JOIN DocumentType dt ON gh.DocumentTypeId = dt.Id "
            + " INNER JOIN GatePassInward gp ON gh.InwardGatePassId = gp.Id "
            + " LEFT JOIN JobLot j on gd.JobLotId = j.Id "
            + " LEFT JOIN InvPackingType pt ON gd.PackingTypeId = pt.Id "
            + " INNER JOIN V_UomScheduleAndUom iu on gd.ItemUomId = iu.Id "
            + " INNER JOIN InvWareHouse wh ON gd.WarehouseId = wh.Id "
            + "where gh.OrganizationId = :organizationId "
            + " and gh.CompanyId = :companyId "
            + " and gh.Id = :id "
    )
    List<InvGrnDetailForContractorWagesRow> getGrnDetialForContractorWages(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_InvGRN_GetAllMethod @Activity='CountPendingGrn' — COUNT(DocNo) of GRNs not yet in a purchase invoice (output column DocNo). */
    @Query("SELECT COUNT(gh.docNo) FROM SupplierCustomer sc JOIN InvGrn gh ON sc.id = gh.supplierCustomerId "
         + "WHERE gh.organizationId = :organizationId AND gh.companyId = :companyId AND gh.documentTypeId = :documentTypeId "
         + "AND (:branchesId IS NULL OR gh.branchesId = :branchesId) "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail dd JOIN InvPurchaseInvoice hh ON hh.id = dd.invPurchaseInvoiceId "
         + "WHERE hh.organizationId = :organizationId AND hh.companyId = :companyId AND (:branchesId IS NULL OR hh.branchesId = :branchesId) AND gh.id = dd.invGrnId)")
    Long countPendingGrn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId);

    /** Sp_InvGRN_GetAllMethod @Activity='GetByDefaultCropJobPackingTypeWarehouseCity' (configuration definitions 9..13). */
    @Query(nativeQuery = true, value =
            "select convert(nvarchar(50), ConfigKey, 0) Id, c.ConfigDescription "
            + "from ConfigrationsAllocation coa "
            + "inner join ConfigrationsDefinition c on c.Id = coa.ConfigrationsDefinitionId "
            + "where OrganizationId = :organizationId "
            + "and CompanyId = :companyId "
            + "and ConfigrationsDefinitionId in (9,10,11,12,13) "
    )
    List<InvGrnDefaultConfigRow> getByDefaultCropJobPackingTypeWarehouseCity(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_InvGRN_GetAllMethod @Activity='GetReceivedWeightAndBalanceWeight' (@Id = GatePassInward.Id, market purchase only). */
    @Query(nativeQuery = true, value =
            "SELECT "
            + " CASE WHEN ISNULL(h.PartyWeight,0) > 0 THEN h.PartyWeight ELSE gp.SupplierWeight END AS SupplierWeight, "
            + " gp.FactoryWeight, isnull(sum(d.ReceivedWeight),0) as ReceivedWeight, "
            + " gp.SupplierWeight - isnull(sum(d.ReceivedWeight),0) AS BalanceWeight, "
            + " 'Load' as DeliverTerm "
            + "FROM GatePassInward gp "
            + "left join InvGrn h on h.InwardGatePassId = gp.Id "
            + "LEFT JOIN (SELECT InvGrnId, ISNULL(SUM(GrossWeight),0) as ReceivedWeight FROM InvGrnDetail group by InvGrnId) d on d.InvGrnId = h.Id "
            + "where gp.OrganizationId = :organizationId "
            + "and gp.CompanyId = :companyId "
            + "and gp.DocumentTypeId = 51 "
            + "and gp.RefDocumentTypeId = 105 "
            + "and gp.Id = :id "
            + "and gp.Status <> 'Rejected' "
            + "group by gp.SupplierWeight, gp.FactoryWeight, h.PartyWeight "
    )
    List<InvGrnReceivedBalanceWeightRow> getReceivedWeightAndBalanceWeight(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_InvGRN_GetAllMethod @Activity='GetRecordId' — SELECT DocDate FROM InvGrn WHERE Id = @Id. */
    @Query("SELECT g.docDate FROM InvGrn g WHERE g.id = :id")
    LocalDateTime getRecordId(@Param("id") Integer id);

    /** Sp_InvGRN_GetAllMethod @Activity='GetGrnCountAgainstGatepass' (output column GRNCount). */
    @Query("SELECT COUNT(gp.id) FROM InvGrn gp WHERE gp.organizationId = :organizationId AND gp.companyId = :companyId AND gp.inwardGatePassId = :gatePassInwardId")
    Long getGrnCountAgainstGatepass(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("gatePassInwardId") Integer gatePassInwardId);

    /**
     * Sp_InvGRN_GetAllMethod @Activity='InvGrnApprove' — @ReqType 'AP' approves (IsApproved=1, PostDate=GETDATE(), PostUser,
     * ApprovalRemarks, RateCut, RateCutRemarks + InventoryTransactions.IsApproved=1), 'UP' un-approves (IsApproved=0,
     * PostDate NULL, PostUser 0, UnApprovedDate/UnApprovedUserId, RateCut 0 + InventoryTransactions.IsApproved=0); then
     * EXEC USP_UserAudit_Insert (ActionId 2). @Remarks, @RateCut, @ScreenName, @DocumentTypeId default null.
     * KEEP-EXEC: branches on @ReqType, reads DocumentTypeId / DocNo / DocumentTypeDescription and calls USP_UserAudit_Insert.
     */
    @Modifying
    @Transactional
    @Query(nativeQuery = true, value = "EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @Id = :id, "
            + "@ApprovedUserId = :approvedUserId, @ReqType = :reqType, @Remarks = :remarks, @DocumentTypeId = :documentTypeId, "
            + "@RateCut = :rateCut, @ScreenName = :screenName, @Activity = 'InvGrnApprove'")
    void invGrnApprove(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("id") Integer id,
            @Param("approvedUserId") Integer approvedUserId, @Param("reqType") String reqType, @Param("remarks") String remarks,
            @Param("documentTypeId") Integer documentTypeId, @Param("rateCut") Double rateCut, @Param("screenName") String screenName);

    /** InvGrnApprove @ReqType='AP' standalone step: approve the GRN header. */
    @Modifying
    @Transactional
    @Query("UPDATE InvGrn g SET g.isApproved = true, g.postDate = CURRENT_TIMESTAMP, g.postUser = :approvedUserId, g.approvalRemarks = :remarks, "
         + "g.rateCut = COALESCE(:rateCut, 0), g.rateCutRemarks = :remarks WHERE g.id = :id")
    int approveById(@Param("id") Integer id, @Param("approvedUserId") Integer approvedUserId, @Param("remarks") String remarks,
            @Param("rateCut") Double rateCut);

    /** InvGrnApprove @ReqType='UP' standalone step: un-approve the GRN header. */
    @Modifying
    @Transactional
    @Query("UPDATE InvGrn g SET g.isApproved = false, g.postDate = NULL, g.postUser = 0, g.unApprovedDate = CURRENT_TIMESTAMP, "
         + "g.unApprovedUserId = :approvedUserId, g.approvalRemarks = :remarks, g.rateCut = 0 "
         + "WHERE g.id = :id AND g.organizationId = :organizationId AND g.companyId = :companyId AND g.documentTypeId = :documentTypeId")
    int unApproveById(@Param("id") Integer id, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("approvedUserId") Integer approvedUserId, @Param("remarks") String remarks);

    /** InvGrnApprove standalone step: UPDATE InventoryTransactions SET IsApproved for the GRN (entity InventoryTransactions, batch J2). */
    @Modifying
    @Transactional
    @Query("UPDATE InventoryTransactions t SET t.isApproved = :isApproved WHERE t.organizationId = :organizationId AND t.companyId = :companyId "
         + "AND t.refDocIdNo = :id AND t.refDocumentTypeId = :documentTypeId")
    int setInventoryTransactionsApproved(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("isApproved") Boolean isApproved);

    // ---------------------------------------------------------------- Sp_InvGrn_Insert

    /**
     * Sp_InvGrn_Insert — full procedure (see class Javadoc). Returns the new Id (SELECT @Id). @DocumentTypeId, @OrganizationId,
     * @CompanyId and @FinancialYearId are mandatory in the proc, everything else defaults to NULL; @DocNo and @Id are
     * recomputed by the proc. KEEP-EXEC: multi-table validations + GatePassInward / WbTransactions updates + USP_UserAudit_Insert.
     * Not @Modifying on purpose: the proc ends with SELECT @Id (SET NOCOUNT ON) so it is run as a result-set query in a transaction.
     */
    @Transactional
    @Query(nativeQuery = true, value = "EXEC dbo.Sp_InvGrn_Insert @Id = :id, @DocumentTypeId = :documentTypeId, @DocDate = :docDate, @DocNo = :docNo, "
            + "@SupplierCustomerId = :supplierCustomerId, @GpNo = :gpNo, @ReferencePartyId = :referencePartyId, @StockPartyId = :stockPartyId, "
            + "@SupplierReference = :supplierReference, @ReferenceDocNo = :referenceDocNo, @PartyWeight = :partyWeight, @FactoryWeight = :factoryWeight, "
            + "@VehicleType = :vehicleType, @BiltyNo = :biltyNo, @VehicleNo = :vehicleNo, @TransporterDocRef = :transporterDocRef, "
            + "@TransporterId = :transporterId, @CarriageAmount = :carriageAmount, @OtherCharges = :otherCharges, @RemarksHeader = :remarksHeader, "
            + "@IsApproved = :isApproved, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
            + "@PostDate = :postDate, @PostUser = :postUser, @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, "
            + "@ProjectsId = :projectsId, @InwardGatePassId = :inwardGatePassId, @ActionId = :actionId, @FinancialYearId = :financialYearId, "
            + "@DeliveryTerm = :deliveryTerm, @Status = :status, @ScaleKart = :scaleKart, @GhallaMandiId = :ghallaMandiId, "
            + "@TransporterSupCustId = :transporterSupCustId, @BillCalculateTypeId = :billCalculateTypeId, "
            + "@BillWeightExceedsStockWeight = :billWeightExceedsStockWeight, @GrnTypeId = :grnTypeId, @ReqestedById = :reqestedById, "
            + "@ReturnableDate = :returnableDate, @TransType = :transType, @ScreenName = :screenName, @AddWages = :addWages, "
            + "@ScaleShortWeightApply = :scaleShortWeightApply, @SupplierShortWeightApply = :supplierShortWeightApply, @RateCut = :rateCut, "
            + "@RateCutRemarks = :rateCutRemarks, @AccessWeight = :accessWeight, @AttachmentsValues = :attachmentsValues, "
            + "@CustomAttachmentsValues = :customAttachmentsValues, @DeductionPolicyForGrnId = :deductionPolicyForGrnId, "
            + "@SupplierDispatchId = :supplierDispatchId, @FreightDeduction = :freightDeduction, @BiltyFreight = :biltyFreight, "
            + "@AdvanceByFactoryFreight = :advanceByFactoryFreight, @AdvanceByPartyFreight = :advanceByPartyFreight, "
            + "@BaseDocumentTypeId = :baseDocumentTypeId, @GainLossWeight = :gainLossWeight, @UserLogId = :userLogId")
    Integer spInvGrnInsert(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("docDate") LocalDate docDate,
            @Param("docNo") Integer docNo, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("gpNo") Integer gpNo,
            @Param("referencePartyId") Integer referencePartyId, @Param("stockPartyId") Integer stockPartyId,
            @Param("supplierReference") String supplierReference, @Param("referenceDocNo") String referenceDocNo,
            @Param("partyWeight") Double partyWeight, @Param("factoryWeight") Double factoryWeight, @Param("vehicleType") String vehicleType,
            @Param("biltyNo") String biltyNo, @Param("vehicleNo") String vehicleNo, @Param("transporterDocRef") String transporterDocRef,
            @Param("transporterId") Integer transporterId, @Param("carriageAmount") Double carriageAmount, @Param("otherCharges") Double otherCharges,
            @Param("remarksHeader") String remarksHeader, @Param("isApproved") Boolean isApproved, @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser, @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate, @Param("postUser") Integer postUser, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId, @Param("projectsId") Integer projectsId,
            @Param("inwardGatePassId") Integer inwardGatePassId, @Param("actionId") Integer actionId, @Param("financialYearId") Integer financialYearId,
            @Param("deliveryTerm") String deliveryTerm, @Param("status") String status, @Param("scaleKart") Double scaleKart,
            @Param("ghallaMandiId") Integer ghallaMandiId, @Param("transporterSupCustId") Integer transporterSupCustId,
            @Param("billCalculateTypeId") Integer billCalculateTypeId, @Param("billWeightExceedsStockWeight") Integer billWeightExceedsStockWeight,
            @Param("grnTypeId") Integer grnTypeId, @Param("reqestedById") Integer reqestedById, @Param("returnableDate") LocalDate returnableDate,
            @Param("transType") String transType, @Param("screenName") String screenName, @Param("addWages") Boolean addWages,
            @Param("scaleShortWeightApply") Boolean scaleShortWeightApply, @Param("supplierShortWeightApply") Boolean supplierShortWeightApply,
            @Param("rateCut") Double rateCut, @Param("rateCutRemarks") String rateCutRemarks, @Param("accessWeight") Double accessWeight,
            @Param("attachmentsValues") String attachmentsValues, @Param("customAttachmentsValues") String customAttachmentsValues,
            @Param("deductionPolicyForGrnId") Integer deductionPolicyForGrnId, @Param("supplierDispatchId") Integer supplierDispatchId,
            @Param("freightDeduction") Double freightDeduction, @Param("biltyFreight") Double biltyFreight,
            @Param("advanceByFactoryFreight") Double advanceByFactoryFreight, @Param("advanceByPartyFreight") Double advanceByPartyFreight,
            @Param("baseDocumentTypeId") Integer baseDocumentTypeId, @Param("gainLossWeight") BigDecimal gainLossWeight,
            @Param("userLogId") Long userLogId);

    /** Sp_InvGrn_Insert / Sp_InvGrn_Update standalone step (RefDocumentType 105/106): UPDATE GatePassInward SET SupplierCustomerId. */
    @Modifying
    @Transactional
    @Query("UPDATE InwardGatePass g SET g.supplierCustomerId = :supplierCustomerId WHERE g.id = :inwardGatePassId")
    int updateGatePassSupplier(@Param("supplierCustomerId") Integer supplierCustomerId, @Param("inwardGatePassId") Integer inwardGatePassId);

    /** Sp_InvGrn_Insert / Sp_InvGrn_Update standalone step (RefDocumentType 105/106): UPDATE WbTransactions SET SupplierCustomerId (no entity → native). */
    @Modifying
    @Transactional
    @Query(nativeQuery = true, value = "UPDATE WbTransactions SET SupplierCustomerId = :supplierCustomerId WHERE OrganizationId = :organizationId "
            + "AND CompanyId = :companyId AND ReferenceDocTypeId = 51 AND ReferenceDocNoId = :inwardGatePassId AND FirstWeight > 0 AND SecondWeight > 0")
    int updateWbTransactionsSupplier(@Param("supplierCustomerId") Integer supplierCustomerId, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("inwardGatePassId") Integer inwardGatePassId);

    /** Sp_InvGrn_Insert / Sp_InvGrn_Update standalone step (RefDocumentType 110/111): UPDATE GatePassInward status / weights / vehicle / freight. */
    @Modifying
    @Transactional
    @Query("UPDATE InwardGatePass g SET g.status = :status, g.factoryWeight = :factoryWeight, g.modifyDate = :entryDate, g.modifyUser = :entryUser, "
         + "g.vehicleNo = :vehicleNo, g.vehicleType = :vehicleType, g.biltyNo = :biltyNo, g.freight = :carriageAmount, g.netPaid = :carriageAmount "
         + "WHERE g.id = :inwardGatePassId")
    int updateGatePassStatusFromGrn(@Param("status") String status, @Param("factoryWeight") Double factoryWeight,
            @Param("entryDate") java.util.Date entryDate, @Param("entryUser") Integer entryUser, @Param("vehicleNo") String vehicleNo,
            @Param("vehicleType") String vehicleType, @Param("biltyNo") String biltyNo, @Param("carriageAmount") Double carriageAmount,
            @Param("inwardGatePassId") Integer inwardGatePassId);

    /** Sp_InvGrn_Insert / Sp_InvGrn_Update standalone step: UPDATE GatePassInward SET SupplierDispatchId, VehicleNo. */
    @Modifying
    @Transactional
    @Query("UPDATE InwardGatePass g SET g.supplierDispatchId = :supplierDispatchId, g.vehicleNo = :vehicleNo WHERE g.id = :inwardGatePassId")
    int updateGatePassSupplierDispatch(@Param("supplierDispatchId") Integer supplierDispatchId, @Param("vehicleNo") String vehicleNo,
            @Param("inwardGatePassId") Integer inwardGatePassId);

    /** Sp_InvGrn_Insert helper: UserLogId defaults to UserAccount.LastUserLogId of the entry user (no entity column → native). */
    @Query(nativeQuery = true, value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :userId")
    Long lastUserLogId(@Param("userId") Integer userId);

    /** Sp_InvGrn_Insert / Sp_InvGrn_Update helper: SUM(WbTransactions.NetWbWeight) of the gate pass (factory weight check). */
    @Query(nativeQuery = true, value = "SELECT ISNULL(SUM(NetWbWeight),0) FROM WbTransactions WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
            + "AND ReferenceDocTypeId = 51 AND ReferenceDocNoId = :inwardGatePassId AND FirstWeight > 0 AND SecondWeight > 0")
    Double weighBridgeNetWeightOfGatePass(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("inwardGatePassId") Integer inwardGatePassId);

    // ---------------------------------------------------------------- Sp_InvGrn_Update

    /**
     * Sp_InvGrn_Update — full procedure (see class Javadoc). @FinancialYearId is mandatory, everything else defaults to NULL.
     * KEEP-EXEC: validations against FinancialYear / InvPurchaseInvoice / InvFreightVoucher / GatePassInward, child deletes
     * and USP_UserAudit_Insert.
     */
    @Modifying
    @Transactional
    @Query(nativeQuery = true, value = "EXEC dbo.Sp_InvGrn_Update @Id = :id, @DocumentTypeId = :documentTypeId, @DocDate = :docDate, @DocNo = :docNo, "
            + "@SupplierCustomerId = :supplierCustomerId, @GpNo = :gpNo, @ReferencePartyId = :referencePartyId, @StockPartyId = :stockPartyId, "
            + "@SupplierReference = :supplierReference, @ReferenceDocNo = :referenceDocNo, @PartyWeight = :partyWeight, @FactoryWeight = :factoryWeight, "
            + "@VehicleType = :vehicleType, @VehicleNo = :vehicleNo, @BiltyNo = :biltyNo, @TransporterDocRef = :transporterDocRef, "
            + "@TransporterId = :transporterId, @CarriageAmount = :carriageAmount, @OtherCharges = :otherCharges, @RemarksHeader = :remarksHeader, "
            + "@IsApproved = :isApproved, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
            + "@PostDate = :postDate, @PostUser = :postUser, @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, "
            + "@ProjectsId = :projectsId, @InwardGatePassId = :inwardGatePassId, @ActionId = :actionId, @FinancialYearId = :financialYearId, "
            + "@DeliveryTerm = :deliveryTerm, @Status = :status, @ScaleKart = :scaleKart, @GhallaMandiId = :ghallaMandiId, "
            + "@TransporterSupCustId = :transporterSupCustId, @BillCalculateTypeId = :billCalculateTypeId, "
            + "@BillWeightExceedsStockWeight = :billWeightExceedsStockWeight, @GrnTypeId = :grnTypeId, @ReqestedById = :reqestedById, "
            + "@ReturnableDate = :returnableDate, @TransType = :transType, @ScreenName = :screenName, @AddWages = :addWages, "
            + "@ScaleShortWeightApply = :scaleShortWeightApply, @SupplierShortWeightApply = :supplierShortWeightApply, @RateCut = :rateCut, "
            + "@RateCutRemarks = :rateCutRemarks, @AccessWeight = :accessWeight, @AttachmentsValues = :attachmentsValues, "
            + "@CustomAttachmentsValues = :customAttachmentsValues, @DeductionPolicyForGrnId = :deductionPolicyForGrnId, "
            + "@SupplierDispatchId = :supplierDispatchId, @FreightDeduction = :freightDeduction, @BiltyFreight = :biltyFreight, "
            + "@AdvanceByFactoryFreight = :advanceByFactoryFreight, @AdvanceByPartyFreight = :advanceByPartyFreight, "
            + "@BaseDocumentTypeId = :baseDocumentTypeId, @GainLossWeight = :gainLossWeight, @UserLogId = :userLogId")
    void spInvGrnUpdate(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("docDate") LocalDateTime docDate,
            @Param("docNo") Integer docNo, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("gpNo") Integer gpNo,
            @Param("referencePartyId") Integer referencePartyId, @Param("stockPartyId") Integer stockPartyId,
            @Param("supplierReference") String supplierReference, @Param("referenceDocNo") String referenceDocNo,
            @Param("partyWeight") Double partyWeight, @Param("factoryWeight") Double factoryWeight, @Param("vehicleType") String vehicleType,
            @Param("vehicleNo") String vehicleNo, @Param("biltyNo") String biltyNo, @Param("transporterDocRef") String transporterDocRef,
            @Param("transporterId") Integer transporterId, @Param("carriageAmount") Double carriageAmount, @Param("otherCharges") Double otherCharges,
            @Param("remarksHeader") String remarksHeader, @Param("isApproved") Boolean isApproved, @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser, @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate, @Param("postUser") Integer postUser, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId, @Param("projectsId") Integer projectsId,
            @Param("inwardGatePassId") Integer inwardGatePassId, @Param("actionId") Integer actionId, @Param("financialYearId") Integer financialYearId,
            @Param("deliveryTerm") String deliveryTerm, @Param("status") String status, @Param("scaleKart") Double scaleKart,
            @Param("ghallaMandiId") Integer ghallaMandiId, @Param("transporterSupCustId") Integer transporterSupCustId,
            @Param("billCalculateTypeId") Integer billCalculateTypeId, @Param("billWeightExceedsStockWeight") Integer billWeightExceedsStockWeight,
            @Param("grnTypeId") Integer grnTypeId, @Param("reqestedById") Integer reqestedById, @Param("returnableDate") LocalDate returnableDate,
            @Param("transType") String transType, @Param("screenName") String screenName, @Param("addWages") Boolean addWages,
            @Param("scaleShortWeightApply") Boolean scaleShortWeightApply, @Param("supplierShortWeightApply") Boolean supplierShortWeightApply,
            @Param("rateCut") Double rateCut, @Param("rateCutRemarks") String rateCutRemarks, @Param("accessWeight") Double accessWeight,
            @Param("attachmentsValues") String attachmentsValues, @Param("customAttachmentsValues") String customAttachmentsValues,
            @Param("deductionPolicyForGrnId") Integer deductionPolicyForGrnId, @Param("supplierDispatchId") Integer supplierDispatchId,
            @Param("freightDeduction") Double freightDeduction, @Param("biltyFreight") Double biltyFreight,
            @Param("advanceByFactoryFreight") Double advanceByFactoryFreight, @Param("advanceByPartyFreight") Double advanceByPartyFreight,
            @Param("baseDocumentTypeId") Integer baseDocumentTypeId, @Param("gainLossWeight") BigDecimal gainLossWeight,
            @Param("userLogId") Long userLogId);

    /**
     * Sp_InvGrn_Update main statement: UPDATE InvGrn SET ... WHERE Id = @Id (every SET column of the proc; EntryDate / EntryUser /
     * PostDate / PostUser / RateCut / RateCutRemarks are commented out in the proc and therefore not updated here).
     * Callers must apply the proc rules first: Status = 'Accepted' unless DocumentTypeId in (165,167); UserLogId defaults to
     * UserAccount.LastUserLogId of @ModifyUser (see {@link #lastUserLogId}).
     */
    @Modifying
    @Transactional
    @Query("UPDATE InvGrn g SET g.documentTypeId = :documentTypeId, g.docDate = :docDate, g.docNo = :docNo, g.supplierCustomerId = :supplierCustomerId, "
         + "g.gpNo = :gpNo, g.referencePartyId = :referencePartyId, g.stockPartyId = :stockPartyId, g.supplierReference = :supplierReference, "
         + "g.referenceDocNo = :referenceDocNo, g.partyWeight = :partyWeight, g.factoryWeight = :factoryWeight, g.vehicleType = :vehicleType, "
         + "g.vehicleNo = :vehicleNo, g.biltyNo = :biltyNo, g.transporterDocRef = :transporterDocRef, g.transporterId = :transporterId, "
         + "g.carriageAmount = :carriageAmount, g.otherCharges = :otherCharges, g.remarksHeader = :remarksHeader, g.isApproved = :isApproved, "
         + "g.modifyDate = :modifyDate, g.modifyUser = :modifyUser, g.organizationId = :organizationId, g.companyId = :companyId, "
         + "g.branchesId = :branchesId, g.projectsId = :projectsId, g.inwardGatePassId = :inwardGatePassId, g.actionId = :actionId, "
         + "g.financialYearId = :financialYearId, g.deliveryTerm = :deliveryTerm, g.status = :status, g.scaleKart = :scaleKart, "
         + "g.ghallaMandiId = :ghallaMandiId, g.billCalculateTypeId = :billCalculateTypeId, g.transporterSupCustId = :transporterSupCustId, "
         + "g.billWeightExceedsStockWeight = :billWeightExceedsStockWeight, g.grnTypeId = :grnTypeId, g.reqestedById = :reqestedById, "
         + "g.returnableDate = :returnableDate, g.transType = :transType, g.screenName = :screenName, g.addWages = :addWages, "
         + "g.scaleShortWeightApply = :scaleShortWeightApply, g.supplierShortWeightApply = :supplierShortWeightApply, "
         + "g.accessWeight = :accessWeight, g.attachmentsValues = :attachmentsValues, g.customAttachmentsValues = :customAttachmentsValues, "
         + "g.deductionPolicyForGrnId = :deductionPolicyForGrnId, g.supplierDispatchId = :supplierDispatchId, g.freightDeduction = :freightDeduction, "
         + "g.biltyFreight = :biltyFreight, g.advanceByFactoryFreight = :advanceByFactoryFreight, g.advanceByPartyFreight = :advanceByPartyFreight, "
         + "g.gainLossWeight = :gainLossWeight, g.baseDocumentTypeId = :baseDocumentTypeId, g.userLogId = :userLogId "
         + "WHERE g.id = :id")
    int updateById(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("docDate") LocalDateTime docDate,
            @Param("docNo") Integer docNo, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("gpNo") Integer gpNo,
            @Param("referencePartyId") Integer referencePartyId, @Param("stockPartyId") Integer stockPartyId,
            @Param("supplierReference") String supplierReference, @Param("referenceDocNo") String referenceDocNo,
            @Param("partyWeight") Double partyWeight, @Param("factoryWeight") Double factoryWeight, @Param("vehicleType") String vehicleType,
            @Param("vehicleNo") String vehicleNo, @Param("biltyNo") String biltyNo, @Param("transporterDocRef") String transporterDocRef,
            @Param("transporterId") Integer transporterId, @Param("carriageAmount") Double carriageAmount, @Param("otherCharges") Double otherCharges,
            @Param("remarksHeader") String remarksHeader, @Param("isApproved") Boolean isApproved,
            @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId, @Param("projectsId") Integer projectsId,
            @Param("inwardGatePassId") Integer inwardGatePassId, @Param("actionId") Integer actionId, @Param("financialYearId") Integer financialYearId,
            @Param("deliveryTerm") String deliveryTerm, @Param("status") String status, @Param("scaleKart") Double scaleKart,
            @Param("ghallaMandiId") Integer ghallaMandiId, @Param("billCalculateTypeId") Integer billCalculateTypeId,
            @Param("transporterSupCustId") Integer transporterSupCustId, @Param("billWeightExceedsStockWeight") Integer billWeightExceedsStockWeight,
            @Param("grnTypeId") Integer grnTypeId, @Param("reqestedById") Integer reqestedById, @Param("returnableDate") LocalDate returnableDate,
            @Param("transType") String transType, @Param("screenName") String screenName, @Param("addWages") Boolean addWages,
            @Param("scaleShortWeightApply") Boolean scaleShortWeightApply, @Param("supplierShortWeightApply") Boolean supplierShortWeightApply,
            @Param("accessWeight") Double accessWeight, @Param("attachmentsValues") String attachmentsValues,
            @Param("customAttachmentsValues") String customAttachmentsValues, @Param("deductionPolicyForGrnId") Integer deductionPolicyForGrnId,
            @Param("supplierDispatchId") Integer supplierDispatchId, @Param("freightDeduction") Double freightDeduction,
            @Param("biltyFreight") Double biltyFreight, @Param("advanceByFactoryFreight") Double advanceByFactoryFreight,
            @Param("advanceByPartyFreight") Double advanceByPartyFreight, @Param("gainLossWeight") BigDecimal gainLossWeight,
            @Param("baseDocumentTypeId") Integer baseDocumentTypeId, @Param("userLogId") Long userLogId);

    /** Sp_InvGrn_Update standalone step (DocumentType 46/143/165/167/169): delete InvGrnPurchaseBreakUp where InvGrnId = @Id (entity of batch F2). */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvGrnPurchaseBreakUp b WHERE b.invGrnId = :invGrnId")
    int deletePurchaseBreakUpByInvGrnId(@Param("invGrnId") Integer invGrnId);

    // ---------------------------------------------------------------- USp_InvGrnStore_Register

    /**
     * USp_InvGrnStore_Register (GRN store register report). All parameters except @OrganizationId default to NULL;
     * @DocumentTypeId is a comma separated list and is mandatory in practice (fnSplitString). The #TmpResult / #tmpDms / #Data
     * temp-table pipeline is rewritten as a CTE with OUTER APPLY / scalar sub-selects (invoice no, wages, attachment count).
     * Note: the procedure selects BaseDocumentTypeId twice, the duplicate is dropped; DetailId is r.Id as in the proc's #Data step.
     */
    @Query(nativeQuery = true, value =
            "WITH TmpResult AS ( "
            + " SELECT GrH.Id, grd.Id as DetailId, GrH.DocumentTypeId, GrH.DocDate, GrH.DocNo, "
            + "  grh.SupplierCustomerId AS SuppliercustomerId, GrH.SupplierReference, GrH.GpNo, GrH.VehicleNo, GrH.BiltyNo, grh.VehicleType, "
            + "  GrH.InwardGatePassId, GrH.ReferenceDocNo, grh.RemarksHeader, "
            + "  GRD.WarehouseId, GrD.ItemId, GrD.CropYear, GRD.PackingTypeId, GRD.JobLotId, GrD.ItemQty, "
            + "  GRD.ItemUomId, GrD.GrossWeight, GrD.EBWPerUnit, GrD.EBWTotal, GrD.WtCut, GrD.WtCutTotal, "
            + "  GrD.AdLsWeight, GrD.NetBillWeight, grd.PurchaseOrderId, grd.PurchaseOrderDetailId, grh.DeliveryTerm, "
            + "  grd.StockWeight, grh.FactoryWeight, grh.PartyWeight, GrD.LabReportRef, grd.AreaCity, GrD.CityId, "
            + "  GrH.CarriageAmount FreightAmount, grh.TransporterId, GrH.RemarksHeader AS CommentsDetail, "
            + "  GrH.EntryDate, grh.EntryUser, GrH.ModifyDate, grh.ModifyUser, grh.BranchesId, grh.BaseDocumentTypeId "
            + " FROM dbo.InvGrn AS GrH "
            + "  INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId "
            + " Where grh.OrganizationId = :organizationId "
            + "  and (:companyId is null or Grh.CompanyId = :companyId) "
            + "  and (grh.DocumentTypeId in (Select Data from fnSplitString(:documentTypeId, ','))) "
            + "  and (:grnId is null or grh.Id = :grnId) "
            + "  and (:grnDateF is null or CAST(grh.DocDate as date) >= CAST(:grnDateF as date)) "
            + "  and (:grnDateT is null or CAST(grh.DocDate as date) <= CAST(:grnDateT as date)) "
            + "  and (:supplierCustomerId is null or Grh.SupplierCustomerId = :supplierCustomerId) "
            + "  and (:grnNoF is null or GrH.DocNo >= :grnNoF) "
            + "  and (:grnNoT is null or GrH.DocNo <= :grnNoT) "
            + "  AND (:warehouseId IS NULL OR grd.WarehouseId = :warehouseId) "
            + "  AND (:jobLotId IS NULL OR grd.JobLotId = :jobLotId) "
            + "  AND (:itemId IS NULL OR grd.ItemId = :itemId) "
            + "  AND (:transporterId IS NULL OR grh.TransporterId = :transporterId) "
            + "  AND (:areaCity IS NULL OR GrD.AreaCity = :areaCity) "
            + "  AND (:gpSrNoF IS NULL OR grh.InwardGatePassId >= :gpSrNoF) "
            + "  AND (:gpSrNoT IS NULL OR GRH.InwardGatePassId <= :gpSrNoT) "
            + "  and (:branchesIds IS NULL OR EXISTS (select 1 from dbo.fnSplitString (:branchesIds, ',') Where grh.BranchesId in (data))) "
            + "  and (:baseDocumentTypeIds IS NULL OR EXISTS (select 1 from dbo.fnSplitString (:baseDocumentTypeIds, ',') Where grh.BaseDocumentTypeId in (data))) "
            + "  AND (:baseDocumentTypeId IS NULL OR grh.BaseDocumentTypeId = :baseDocumentTypeId) "
            + "  and (:onlyPending is null "
            + "   or Not Exists (Select 1 "
            + "    From InvPurchaseInvoice dc "
            + "    Inner Join InvPurchaseInvoiceDetail dd on dd.InvPurchaseInvoiceId = dc.Id and dd.ActionTypeId <> 3 "
            + "    Where dd.InvGrnId = grh.id and dc.ActionId <> 3)) "
            + ") "
            + "Select r.Id, r.Id as DetailId, r.DocumentTypeId, r.DocDate, r.DocNo, "
            + " CASE WHEN INV.InvGrnId IS NULL THEN 0 ELSE INV.DocNo END AS InvoiceNo, "
            + " r.SuppliercustomerId, Sp.CompanyName, r.SupplierReference, dbo.GatePassInward.GpDate, r.GpNo, "
            + " r.RemarksHeader, r.VehicleNo, r.VehicleType, r.BiltyNo, r.InwardGatePassId, r.ReferenceDocNo, "
            + " CASE WHEN WG.Id IS NULL THEN 0 ELSE WG.Id END AS WagesId, "
            + " CASE WHEN WG.Id IS NULL THEN 0 ELSE WG.DocNo END AS WagesNo, "
            + " PoH.Id as OrderId, poh.DocDate as PoDate, poh.DocNo as PoNo, "
            + " dbo.InvWareHouse.WareHouseName, r.ItemId, i.ItemName, r.CropYear, "
            + " PckT.PackTypeDesc, JL.JobLotDescription, r.ItemQty, "
            + " UomItm.UOMCode, r.GrossWeight, r.EBWPerUnit, r.EBWTotal, r.WtCut, r.WtCutTotal, r.AdLsWeight, r.NetBillWeight, "
            + " r.DeliveryTerm, r.StockWeight, r.FactoryWeight, r.PartyWeight, r.LabReportRef, r.AreaCity, r.FreightAmount, "
            + " r.TransporterId, c.AccountTitle TransporterName, r.CommentsDetail, r.EntryDate, "
            + " Concat(Eusr.FirstName, '-', Eusr.LastName) as UserNameEusr, "
            + " r.ModifyDate, "
            + " Concat(Musr.FirstName, '-', Musr.LastName) as UserNameMusr, "
            + " (SELECT COUNT(dms.Id) FROM dbo.DMSAttachments dms WHERE dms.RefDocumentTypeId = r.DocumentTypeId AND dms.RefDocumentNo = r.Id AND dms.OrganizationId = :organizationId AND dms.CompanyId = :companyId) AS NoOfAttachments, "
            + " r.BaseDocumentTypeId, bb.BranchName, dt.DocumentTypeDescription, "
            + " bd.Description BaseDocumentType, "
            + " co.CompCountry, co.CompContactPerson, co.CompMobileA, co.CompMobileB, "
            + " co.CompMobileC, co.CompEmailA, co.CompEmailB, co.CompLogoImage, "
            + " co.CompanyWebsite, co.CompanyFaxNo, org.ReportingRemarks OrgReportingRemarks "
            + "FROM TmpResult R "
            + " INNER JOIN SupplierCustomer sp on r.SuppliercustomerId = sp.Id "
            + " INNER JOIN Item i on r.ItemId = i.Id "
            + " INNER JOIN dbo.InvWareHouse ON r.WarehouseId = dbo.InvWareHouse.Id "
            + " INNER JOIN dbo.UserAccount AS Eusr ON R.EntryUser = Eusr.ID "
            + " INNER JOIN Company cmp on cmp.Id = :companyId "
            + " LEFT JOIN dbo.GatePassInward ON r.InwardGatePassId = dbo.GatePassInward.Id "
            + " LEFT JOIN dbo.V_UomScheduleAndUom AS UomItm ON r.ItemUomId = UomItm.Id "
            + " LEFT JOIN dbo.JobLot AS JL ON r.JobLotId = JL.Id "
            + " LEFT JOIN dbo.InvPackingType AS PckT ON r.PackingTypeId = PckT.Id "
            + " LEFT JOIN dbo.UserAccount AS Musr ON R.ModifyUser = Musr.ID "
            + " LEFT JOIN ChartofAccount c on R.TransporterId = c.Id "
            + " LEFT JOIN dbo.PurchaseOrderDetail AS PoD on R.PurchaseOrderId = pod.PurchaseOrderId and R.PurchaseOrderDetailId = pod.Id and ISNULL(pod.ActionTypeId,0) <> 3 "
            + " LEFT JOIN dbo.PurchaseOrder AS PoH ON PoD.PurchaseOrderId = PoH.Id "
            + " Left Join bRanches bb on bb.Id = r.BranchesId "
            + " Inner join Company co on co.Id = :companyId "
            + " Inner join Organization org on org.Id = co.OrgCompanyTypeId "
            + " Inner join DocumentType dt on dt.Id = r.DocumentTypeId "
            + " LEFT join BaseDocumentType bd on bd.Id = r.BaseDocumentTypeId and bd.DocumentTypeId = r.DocumentTypeId "
            + " OUTER APPLY (SELECT TOP 1 d.InvGrnId, h.DocNo FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice h on d.InvPurchaseInvoiceId = h.Id and d.ActionTypeId <> 3 WHERE d.InvGrnId = r.Id and d.InvGrnDetailId = r.DetailId) INV "
            + " OUTER APPLY (SELECT TOP 1 w.Id, w.DocNo FROM InvContractorWagesBillHeader w WHERE w.RefDocNoId = r.Id and w.RefDocumentTypeId = r.DocumentTypeId) WG "
            + "WHERE (:poNoFrom is null or Poh.DocNo >= :poNoFrom) "
            + " and (:poNoTo is null or Poh.DocNo <= :poNoTo) "
            + "ORDER BY R.DocDate, R.DocNo "
    )
    List<InvGrnStoreRegisterRow> invGrnStoreRegister(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("grnId") Integer grnId, @Param("grnNoF") Integer grnNoF, @Param("grnNoT") Integer grnNoT,
            @Param("gpSrNoF") Integer gpSrNoF, @Param("gpSrNoT") Integer gpSrNoT, @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("grnDateF") LocalDate grnDateF, @Param("grnDateT") LocalDate grnDateT, @Param("warehouseId") Integer warehouseId,
            @Param("areaCity") String areaCity, @Param("jobLotId") Integer jobLotId, @Param("itemId") Integer itemId,
            @Param("poNoFrom") Integer poNoFrom, @Param("poNoTo") Integer poNoTo, @Param("documentTypeId") String documentTypeId,
            @Param("transporterId") Integer transporterId, @Param("branchesIds") String branchesIds,
            @Param("baseDocumentTypeId") Integer baseDocumentTypeId, @Param("baseDocumentTypeIds") String baseDocumentTypeIds,
            @Param("onlyPending") Boolean onlyPending);
}
