package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InvGrnPurchaseBreakUp;
import com.mst.projections.GrnBranchAllocatedRow;
import com.mst.projections.GrnLoadForPurchaseInvoiceRow;
import com.mst.projections.GrnLoaderDataForPurchaseInvoiceRow;
import com.mst.projections.GrnSpecialApprovalDataRow;
import com.mst.projections.GrnTransporterFreightRow;
import com.mst.projections.GrnWagesAmountRow;

/**
 * Query-only repository for the GRN support / lookup procedures of batch F2_grn_support:
 * USP_GetBranchsAllocatedToUserFromGrn, USP_GetWagesAmountByRefDocumentTypeIdandRefIds,
 * USP_InvGrn_DataForSpecialApproval, USP_InvGrn_GetTransporterAndFreight,
 * usp_GrnLoadForPurchaseInvoice, usp_getGrnLoaderDataForPurchaseInvoice.
 * Bound to InvGrnPurchaseBreakUp only because Spring Data needs a managed entity.
 */
@Repository
public interface GrnSupportQueryRepository extends org.springframework.data.repository.Repository<InvGrnPurchaseBreakUp, Integer> {

    /**
     * USP_GetBranchsAllocatedToUserFromGrn (single branch). @DocumentTypeId default NULL.
     * Native: BranchesAllocationToUser has no entity. WHERE pasted verbatim — note the proc's
     * un-parenthesised "AND ... IS NULL OR ..." precedence is preserved on purpose.
     */
    @Query(value = "SELECT Distinct c.BranchId, i.BranchName "
         + "FROM dbo.BranchesAllocationToUser AS c "
         + "Inner JOIN dbo.Branches AS i ON i.Id = c.BranchId AND i.CompanyId = c.CompanyId "
         + "Inner JOIN dbo.UserAccount AS g ON c.UserId = g.Id AND g.CompanyId = c.CompanyId "
         + "Inner JOIN dbo.InvGrn as p On p.BranchesId = c.BranchId "
         + "Where c.OrganizationId = :organizationId "
         + "and c.CompanyId = :companyId "
         + "and c.UserId = :userId "
         + "and :documentTypeId Is Null Or p.DocumentTypeId = :documentTypeId", nativeQuery = true)
    List<GrnBranchAllocatedRow> getBranchsAllocatedToUserFromGrn(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("userId") Integer userId,
                                                                 @Param("documentTypeId") Integer documentTypeId);

    /**
     * USP_GetWagesAmountByRefDocumentTypeIdandRefIds (single branch). Always one aggregate row.
     * @RefIds is a comma separated id list (fnSplitString). Native: InvContractorWagesBill* have no entity.
     */
    @Query(value = "SELECT ISNULL(SUM(Qty),0) AS TotalQty, "
         + "ISNULL(SUM(Weight),0) AS TotalWeight, "
         + "ISNULL(SUM(WagesAmount),0) AS WagesAmount "
         + "FROM InvContractorWagesBillHeader h WITH(NOLOCK) "
         + "INNER JOIN InvContractorWagesBillDetail d WITH(NOLOCK) on h.Id = d.InvContractorWagesBillHeaderId "
         + "WHERE h.OrganizationId = :organizationId "
         + "AND h.CompanyId = :companyId "
         + "AND h.RefDocumentTypeId = :refDocumentTypeId "
         + "AND (h.RefDocNoId IN (SELECT DATA FROM dbo.fnSplitString(:refIds,','))) "
         + "and (ISNULL(h.RefDocNoId,0) <> 0)", nativeQuery = true)
    GrnWagesAmountRow getWagesAmountByRefDocumentTypeIdAndRefIds(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                                 @Param("refIds") String refIds);

    /**
     * USP_InvGrn_DataForSpecialApproval (single branch).
     * // KEEP-EXEC: 5 temp tables (#TmpResult/#TmpRecivings/#tmpFinalWeights/#tmpDms/#tmpDmsCloud) with
     * multi-step UPDATEs, fnSplitString, vEmptyBagTypes view, DAW schema join — not expressible as one statement.
     * Defaults: every parameter after @BranchesIds defaults to NULL (@ReasonId, @GrnNoF, @GrnNoT, @GpSrNoF, @GpSrNoT,
     * @SupplierCustomerId, @GrnDateF, @GrnDateT, @WarehouseId, @CityId, @VehicleNo, @JobLotId, @ItemId,
     * @InventoryParentCategorId, @TransporterId, @CropYear, @ItemCategoryId, @ItemTypeId, @Id, @CustomeGroupId,
     * @IsApproved, @UserId). @CropYear/@ItemCategoryId/@ItemTypeId/@UserId are accepted but unused by the proc body.
     */
    @Query(value = "EXEC dbo.USP_InvGrn_DataForSpecialApproval "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FinancialYearId = :financialYearId, "
         + "@DocumentTypeId = :documentTypeId, @BranchesIds = :branchesIds, @ReasonId = :reasonId, "
         + "@GrnNoF = :grnNoF, @GrnNoT = :grnNoT, @GpSrNoF = :gpSrNoF, @GpSrNoT = :gpSrNoT, "
         + "@SupplierCustomerId = :supplierCustomerId, @GrnDateF = :grnDateF, @GrnDateT = :grnDateT, "
         + "@WarehouseId = :warehouseId, @CityId = :cityId, @VehicleNo = :vehicleNo, @JobLotId = :jobLotId, "
         + "@ItemId = :itemId, @InventoryParentCategorId = :inventoryParentCategorId, @TransporterId = :transporterId, "
         + "@CropYear = :cropYear, @ItemCategoryId = :itemCategoryId, @ItemTypeId = :itemTypeId, @Id = :id, "
         + "@CustomeGroupId = :customeGroupId, @IsApproved = :isApproved, @UserId = :userId", nativeQuery = true)
    List<GrnSpecialApprovalDataRow> dataForSpecialApproval(@Param("organizationId") Integer organizationId,
                                                           @Param("companyId") Integer companyId,
                                                           @Param("financialYearId") Integer financialYearId,
                                                           @Param("documentTypeId") Integer documentTypeId,
                                                           @Param("branchesIds") String branchesIds,
                                                           @Param("reasonId") Integer reasonId,
                                                           @Param("grnNoF") Integer grnNoF,
                                                           @Param("grnNoT") Integer grnNoT,
                                                           @Param("gpSrNoF") Integer gpSrNoF,
                                                           @Param("gpSrNoT") Integer gpSrNoT,
                                                           @Param("supplierCustomerId") Integer supplierCustomerId,
                                                           @Param("grnDateF") LocalDate grnDateF,
                                                           @Param("grnDateT") LocalDate grnDateT,
                                                           @Param("warehouseId") Integer warehouseId,
                                                           @Param("cityId") Integer cityId,
                                                           @Param("vehicleNo") String vehicleNo,
                                                           @Param("jobLotId") Integer jobLotId,
                                                           @Param("itemId") Integer itemId,
                                                           @Param("inventoryParentCategorId") Integer inventoryParentCategorId,
                                                           @Param("transporterId") Integer transporterId,
                                                           @Param("cropYear") String cropYear,
                                                           @Param("itemCategoryId") Integer itemCategoryId,
                                                           @Param("itemTypeId") Integer itemTypeId,
                                                           @Param("id") Integer id,
                                                           @Param("customeGroupId") Integer customeGroupId,
                                                           @Param("isApproved") Boolean isApproved,
                                                           @Param("userId") Integer userId);

    /**
     * USP_InvGrn_GetTransporterAndFreight (single branch). @GdnIds = comma separated InvGrn ids.
     * Native: the proc's #Data / #updateInvFreightVoucher temp-table pipeline rewritten as ONE statement:
     * CTE Data = first SELECT; CTE FV = the freight-voucher aggregate; OUTER APPLY (TOP 1) reproduces the
     * UPDATE-from-join (one matching row per gate pass); the UPDATEd columns become CASE expressions.
     * InvFreightVoucher/InvFreightVoucherDetail have no entity.
     */
    @Query(value = "WITH Data AS ("
         + "SELECT h.Id AS MainId, h.DocNo, h.DocDate, h.InwardGatePassId, isnull(h.GpNo,0) GpNo, "
         + "h.TransporterSupCustId, h.TransporterId as Transporter, h.CarriageAmount, h.BiltyNo, h.VehicleNo, "
         + "case when gp.Freight > 0 and gp.FreightOn = 1 then 1 else 0 end as FreightOnCash, "
         + "H.AdvanceByPartyFreight, h.AdvanceByFactoryFreight, h.FreightDeduction, gp.RefDocumentTypeId "
         + "FROM InvGrn h "
         + "Left Join Gatepassinward gp on gp.id = h.InwardGatePassId "
         + "WHERE (h.Id in (select data from dbo.fnSplitString(:gdnIds,','))) "
         + "AND coalesce(h.ActionId,0) <> 3"
         + "), FV AS ("
         + "Select h.GatePassId, d.FreightVoucherId, "
         + "Sum(d.DeductedAmount) DeductionAmount, Sum(d.TotalPayableAmount) PaidAmount, Sum(d.chargetoParty) chargetoParty, "
         + "Sum(d.AdvanceByFactory) AdvanceByFactory, Sum(d.AdvanceByParty) AdvanceByParty "
         + "From Data R "
         + "Inner Join InvFreightVoucher h on R.InwardGatePassId = h.GatePassId "
         + "Inner Join InvFreightVoucherDetail d on d.FreightVoucherId = h.id "
         + "Group By h.GatePassId, d.FreightVoucherId"
         + "), Resolved AS ("
         + "SELECT R.MainId, R.DocNo, R.DocDate, R.GpNo, R.TransporterSupCustId, R.Transporter, R.CarriageAmount, R.BiltyNo, R.VehicleNo, "
         + "R.FreightOnCash, R.RefDocumentTypeId, "
         + "CASE WHEN d.GatePassId IS NOT NULL THEN d.FreightVoucherId ELSE 0 END AS FreightId, "
         + "CASE WHEN d.GatePassId IS NOT NULL THEN d.PaidAmount ELSE R.CarriageAmount END AS TotalPaidAmount, "
         + "CASE WHEN d.GatePassId IS NOT NULL THEN d.chargetoParty ELSE Cast(0 as float) END AS TotalChargeToPartyAmount, "
         + "CASE WHEN d.GatePassId IS NOT NULL THEN d.DeductionAmount ELSE R.FreightDeduction END AS TotalDeductionAmount, "
         + "CASE WHEN d.GatePassId IS NOT NULL THEN d.AdvanceByParty ELSE R.AdvanceByPartyFreight END AS TotalAdanceByPartyAmount, "
         + "CASE WHEN d.GatePassId IS NOT NULL THEN d.AdvanceByFactory ELSE R.AdvanceByFactoryFreight END AS TotalAdvanceByFactoryAmount "
         + "FROM Data R "
         + "OUTER APPLY (SELECT TOP 1 f.* FROM FV f WHERE f.GatePassId = R.InwardGatePassId) d"
         + ") "
         + "Select h.MainId, h.DocNo, h.DocDate, h.GpNo, h.TransporterSupCustId, h.Transporter, "
         + "CASe when ISNULL(h.TransporterSupCustId,0) > 0 then sp.CompanyName when ISNULL(h.Transporter,0) > 0 then c.AccountTitle else '' end as AccountTitle, "
         + "Case when h.FreightId > 0 AND ISNULL(h.RefDocumentTypeId,0) <> 105 then h.TotalPaidAmount ELSE h.CarriageAmount END as CarriageAmount, "
         + "h.BiltyNo, h.VehicleNo, h.FreightOnCash, h.FreightId, h.TotalPaidAmount, h.TotalChargeToPartyAmount, "
         + "h.TotalDeductionAmount, h.TotalAdanceByPartyAmount, h.TotalAdvanceByFactoryAmount "
         + "From Resolved h "
         + "LEFT JOIN ChartofAccount c ON h.Transporter = c.Id "
         + "LEFT JOIN SupplierCustomer sp on sp.Id = h.TransporterSupCustId", nativeQuery = true)
    List<GrnTransporterFreightRow> getTransporterAndFreight(@Param("gdnIds") String gdnIds);

    /**
     * usp_GrnLoadForPurchaseInvoice (single branch). @GrnIds = comma separated InvGrn ids.
     * // KEEP-EXEC: the proc first reads config 326 (GrnApprovalNotCompulsory) and THROWs 50001
     * "GRN cannot be loaded because approval is pending: ..." when SpecialApprovalLog has pending rows; the
     * EXEC keeps that validation (surfaces as a SQLException). Pure result set: {@link #grnLoadForPurchaseInvoiceRows}.
     */
    @Query(value = "EXEC dbo.usp_GrnLoadForPurchaseInvoice @GrnIds = :grnIds", nativeQuery = true)
    List<GrnLoadForPurchaseInvoiceRow> grnLoadForPurchaseInvoice(@Param("grnIds") String grnIds);

    /**
     * usp_GrnLoadForPurchaseInvoice — the pending-approval guard: STRING_AGG of SpecialApprovalReason for the given
     * GRNs (RefDocumentTypeId 46/217, not approved, ActionId &lt;&gt; 3). NULL/empty = nothing pending. The proc only
     * applies this guard when config 326 (GrnApprovalNotCompulsory) is 0 — see {@link #grnApprovalNotCompulsory}.
     */
    @Query(value = "SELECT STRING_AGG(SR.SpecialApprovalReason, ', ') "
         + "FROM dbo.SpecialApprovalLog SAL "
         + "INNER JOIN dbo.SpecialApprovalReason SR ON SR.Id = SAL.ReasonId "
         + "INNER JOIN dbo.fnSplitString(:grnIds, ',') S ON SAL.RefDocId = S.data "
         + "WHERE SAL.RefDocumentTypeId IN (46, 217) "
         + "AND ISNULL(SAL.IsApproved, 0) = 0 "
         + "AND SAL.ActionId <> 3", nativeQuery = true)
    String pendingApprovalReasonsForGrns(@Param("grnIds") String grnIds);

    /** usp_GrnLoadForPurchaseInvoice / usp_getGrnLoaderDataForPurchaseInvoice: config 326 GrnApprovalNotCompulsory (NULL → treat as 0). */
    @Query(value = "SELECT CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 326))", nativeQuery = true)
    Boolean grnApprovalNotCompulsory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * usp_GrnLoadForPurchaseInvoice — the result SELECT pasted verbatim (without the THROW guard).
     * Native: V_UomScheduleAndUom view, InvLabAnalysisPurchaseHeader (no entity), ROW_NUMBER, fnSplitString,
     * nested RIGHT JOIN ... ON chains.
     */
    @Query(value = "SELECT distinct Grd.Id as InvGrnDetailId, Grd.InvGrnId, GR.DocDate, GR.DocNo, GR.SupplierCustomerId, SC.CompanyName AS SupplierCustomer, "
         + "RP.CompanyName AS ReferenceParty, SP.CompanyName AS StockParty, ISNULL(GR.CarriageAmount, 0) As CarriageAmount, "
         + "Grd.ItemId, Item.ItemCode, Item.ItemName, Grd.CropYear, Grd.ItemUomId as ItemUOMId, UomItm.UOMCode AS UOMCodeItem, "
         + "Grd.ItemQty, Grd.WarehouseId, wr.WareHouseName, Grd.PackingTypeId, pp.PackTypeDesc, "
         + "CASE WHEN ISNULL(Grd.LabReportRef,'') = '' THEN CAST(LP.DocNo AS NVARCHAR(50)) ELSE GRD.LabReportRef END AS LabAnalisysNo, "
         + "Grd.PurchaseOrderId, "
         + "PurchaseOrder.DocNo AS PurchaseOrder, Grd.GrossWeight, Grd.EBWPerUnit, Grd.EBWTotal as EBTotalWt, grd.EbPurAgainstWeight, Grd.WtCut, "
         + "Grd.WtCutTotal as WeightCutTotal, ISNULL(Grd.AdLsWeight,0) AdLsWeight, Grd.ScaleKart, ISNULL(Grd.NetBillWeight, 0) AS NetBillWeight, "
         + "Grd.StockWeight as NetStockWeight, Grd.AreaCity, "
         + "gp.AccessWeight As ExcessWeigt, gp.RateCut As RateCutForExcessWeight, "
         + "CASE "
         + "WHEN (gp.RefDocumentTypeId = 106 OR gp.RefDocumentTypeId = 110) AND isnull(lp.ItemRate,0) > 0 "
         + "THEN (isnull(lp.ItemRate,0) + isnull(lp.Premium,0) - isnull(lp.DeductionRate,0)) "
         + "WHEN gp.RefDocumentTypeId = 41 OR GR.DocumentTypeId = 169 OR grd.PurchaseOrderId > 0 "
         + "THEN ISNULL(PurchaseOrderDetail.OrderItemRate,0) "
         + "ELSE 0 END ItemRate, "
         + "CASE "
         + "WHEN gp.RefDocumentTypeId = 41 OR GR.DocumentTypeId = 169 OR grd.PurchaseOrderId > 0 THEN PurchaseOrderDetail.OrderItemRateUOMId "
         + "ELSE lp.RateUomId END UomScheduleIdRate, "
         + "CASE "
         + "WHEN gp.RefDocumentTypeId = 41 OR GR.DocumentTypeId = 169 OR grd.PurchaseOrderId > 0 THEN ISNULL(UomPoRate.Equivalent, 0) "
         + "ELSE UomRateLab.Equivalent END EquivalentPoRate, "
         + "CASE "
         + "WHEN gp.RefDocumentTypeId = 41 OR GR.DocumentTypeId = 169 OR grd.PurchaseOrderId > 0 THEN ISNULL(UomPoRate.UOMCode, 0) "
         + "ELSE UomRateLab.UOMCode END RateUom, "
         + "CASE "
         + "WHEN gp.RefDocumentTypeId = 41 OR GR.DocumentTypeId = 169 OR grd.PurchaseOrderId > 0 "
         + "THEN (CASE WHEN Isnull(Grd.NetBillWeight, 0) > 0 AND Isnull(UomPoRate.Equivalent, 0) > 0 "
         + "THEN Grd.NetBillWeight / UomPoRate.Equivalent * dbo.PurchaseOrderDetail.OrderItemRate ELSE 0 END) "
         + "WHEN gp.RefDocumentTypeId = 106 OR GP.RefDocumentTypeId = 110 "
         + "THEN (CASE WHEN Isnull(Grd.NetBillWeight, 0) > 0 AND Isnull(UomRateLab.Equivalent, 0) > 0 "
         + "THEN Grd.NetBillWeight / UomRateLab.Equivalent * ((isnull(lp.ItemRate,0) + isnull(lp.Premium,0) - isnull(lp.DeductionRate,0))) ELSE 0 END) "
         + "ELSE 0 END AS ItemAmount, "
         + "Grd.JobLotId, j.JobLotDescription, Grd.CommentsDetail, PurchaseOrder.BrokerAgentSupCustId, "
         + "SupplierCustomer.CompanyName AS CommissionAgent, PurchaseOrder.CommissionType, "
         + "PurchaseOrder.CommRate, PurchaseOrder.UomScheduleIdCmRate, "
         + "PurchaseOrder.CommAmount, GR.DeliveryTerm, PurchaseOrder.OrderDueDate, "
         + "PurchaseOrder.OrderDueDays, PurchaseOrder.PaymentTermsId, "
         + "PurchaseOrder.CommissionRemarks, GR.ReferencePartyId, GR.StockPartyId, "
         + "Item.PurchaseGLAC, SC.GlAccountId, PurchaseOrder.RemarksHeader, "
         + "TR.AccountTitle, gp.GpDate, GR.GpNo, GR.VehicleNo, GR.InwardGatePassId, lp.DeductionWeight, "
         + "case when isnull(lp.ItemRate,0) = 0 then lp.DeductionRate else 0 end as DeductionRate, "
         + "PurchaseOrder.BrokerAgentId, PurchaseOrder.BrokeryType, PurchaseOrder.BrokeryUom, "
         + "PurchaseOrder.BrokeryRate, PurchaseOrder.BrokeryAmount, JAB.BranchId, bb.BranchName, UomItm.Equivalent ItemUomEquivalent, "
         + "GR.FreightDeduction, GR.TransporterId "
         + "FROM dbo.SupplierCustomer "
         + "RIGHT JOIN dbo.V_UomScheduleAndUom AS UomPoRate "
         + "INNER JOIN dbo.PurchaseOrderDetail "
         + "LEFT JOIN dbo.PurchaseOrder ON dbo.PurchaseOrderDetail.PurchaseOrderId = dbo.PurchaseOrder.Id "
         + "ON UomPoRate.Id = dbo.PurchaseOrderDetail.OrderItemRateUOMId "
         + "ON dbo.SupplierCustomer.Id = dbo.PurchaseOrder.BrokerAgentSupCustId "
         + "RIGHT JOIN dbo.ChartofAccount AS TR "
         + "RIGHT JOIN dbo.V_UomScheduleAndUom AS UomItm "
         + "RIGHT JOIN dbo.InvGrnDetail Grd "
         + "INNER JOIN dbo.InvGrn AS GR ON GR.Id = Grd.InvGrnId "
         + "INNER JOIN dbo.Item ON Grd.ItemId = dbo.Item.Id ON UomItm.Id = Grd.ItemUomId ON TR.Id = GR.TransporterId "
         + "ON dbo.PurchaseOrderDetail.OrderItemId = Grd.ItemId "
         + "AND dbo.PurchaseOrder.Id = Grd.PurchaseOrderId "
         + "AND Grd.PurchaseOrderDetailId = PurchaseOrderDetail.Id "
         + "INNER JOIN InvWareHouse wr on Grd.WarehouseId = wr.Id "
         + "LEFT JOIN GatePassInward gp on GR.InwardGatePassId = gp.Id "
         + "LEFT join JobLot j on j.Id = grd.JobLotId "
         + "LEFT join InvPackingType pp on pp.Id = grd.PackingTypeId "
         + "LEFT JOIN dbo.SupplierCustomer AS SC ON GR.SupplierCustomerId = SC.Id "
         + "LEFT JOIN dbo.SupplierCustomer AS RP ON GR.ReferencePartyId = RP.Id "
         + "LEFT JOIN dbo.SupplierCustomer AS SP ON GR.StockPartyId = SP.Id "
         + "LEFT JOIN ("
         + "SELECT ROW_NUMBER() OVER (PARTITION BY GatePassInwardId, ItemId ORDER BY GatePassInwardId DESC) AS RowNum, "
         + "GatePassInwardId, ItemId, RateUomId, DeductionRate, ItemRate, Premium, DocNo, DeductionWeight "
         + "FROM InvLabAnalysisPurchaseHeader"
         + ") lp ON gr.InwardGatePassId = lp.GatePassInwardId AND Grd.ItemId = lp.ItemId AND lp.RowNum = 1 "
         + "LEFT JOIN V_UomScheduleAndUom UomRateLab on lp.RateUomId = UomRateLab.Id "
         + "Left Join JobLotsAllocationToBranch JAB on JAB.JobLotId = Grd.JobLotId "
         + "Left Join Branches bb on bb.Id = JAB.BranchId "
         + "WHERE (GR.Id in (select data from dbo.fnSplitString(:grnIds,',')))", nativeQuery = true)
    List<GrnLoadForPurchaseInvoiceRow> grnLoadForPurchaseInvoiceRows(@Param("grnIds") String grnIds);

    /**
     * usp_getGrnLoaderDataForPurchaseInvoice (single branch). All parameters default NULL; @BranchesIds is a comma
     * separated list (NULL = no branch filter).
     * Native, one statement: CTE Cfg replaces the @GrnApprovalNotCompulsory variable (config 326), CTE PendingApproval
     * and GrnDetailAgg are the proc's CTEs, the #BranchesIds temp table is inlined as fnSplitString; STRING_AGG,
     * DATEADD, SpecialApprovalLog/SpecialApprovalReason have no entity.
     */
    @Query(value = "WITH Cfg AS ("
         + "SELECT ISNULL(CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 326)), 0) AS GrnApprovalNotCompulsory"
         + "), PendingApproval AS ("
         + "SELECT SAL.RefDocId AS GrnId, STRING_AGG(SR.SpecialApprovalReason, ', ') AS ApprovalReasons "
         + "FROM dbo.SpecialApprovalLog SAL "
         + "INNER JOIN dbo.SpecialApprovalReason SR ON SR.Id = SAL.ReasonId "
         + "WHERE SAL.RefDocumentTypeId IN (46,217) AND SAL.IsApproved = 0 AND SAL.ActionId <> 3 "
         + "GROUP BY SAL.RefDocId"
         + "), GrnDetailAgg AS ("
         + "SELECT InvGrnId, PurchaseOrderId, SUM(ItemQty) AS ItemQty FROM dbo.InvGrnDetail GROUP BY InvGrnId, PurchaseOrderId"
         + ") "
         + "SELECT gh.Id, gh.DocDate, gh.DocNo, gh.GpNo, gp.GpDate, gh.VehicleNo, gh.BiltyNo, gh.DeliveryTerm, "
         + "CASE WHEN cfg.GrnApprovalNotCompulsory = 0 AND PA.GrnId IS NOT NULL THEN 1 ELSE 0 END AS GrnStatusId, "
         + "CASE WHEN cfg.GrnApprovalNotCompulsory = 0 AND PA.GrnId IS NOT NULL THEN PA.ApprovalReasons ELSE 'Ready For Invoice' END AS GrnStatus, "
         + "sc.CompanyName AS SupplierCustomer, gd.PurchaseOrderId, gh.SupplierCustomerId, gd.ItemQty, poh.DocNo AS PurchaseOrder, "
         + "CASE WHEN gh.DocumentTypeId = 137 THEN 110 WHEN gh.DocumentTypeId = 217 THEN 217 ELSE gp.RefDocumentTypeId END AS RefDocumentTypeId, "
         + "CASE "
         + "WHEN gp.RefDocumentTypeId = 41 AND POH.OrderCatagoryId = 8 THEN 'Govt.Purchase' "
         + "WHEN gp.RefDocumentTypeId = 41 THEN 'PurchaseOrder' "
         + "WHEN gp.RefDocumentTypeId = 104 THEN 'Supply Order' "
         + "WHEN gp.RefDocumentTypeId = 105 THEN 'Market Purchase' "
         + "WHEN gp.RefDocumentTypeId IN (106,110) OR gh.DocumentTypeId = 137 THEN 'Gate Purchase' "
         + "WHEN gh.DocumentTypeId = 217 THEN 'Purchase From Party Processing' "
         + "WHEN gp.RefDocumentTypeId = 98 THEN 'Sales Return' "
         + "ELSE 'Other' END AS PurchaseAgainst, "
         + "gh.BranchesId, bb.BranchName "
         + "FROM dbo.InvGrn gh "
         + "CROSS JOIN Cfg cfg "
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
         + "AND NOT EXISTS (SELECT 1 FROM dbo.InvPurchaseInvoiceDetail d "
         + "INNER JOIN dbo.InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE d.InvGrnId = gh.Id AND d.ActionTypeId <> 3 AND h.ActionId <> 3 "
         + "AND h.OrganizationId = :organizationId AND h.CompanyId = :companyId) "
         + "AND (:branchesIds IS NULL OR EXISTS(SELECT * FROM dbo.fnSplitString(:branchesIds,',') WHERE data = gh.BranchesId)) "
         + "ORDER BY gh.DocNo", nativeQuery = true)
    List<GrnLoaderDataForPurchaseInvoiceRow> getGrnLoaderDataForPurchaseInvoice(@Param("organizationId") Integer organizationId,
                                                                                @Param("companyId") Integer companyId,
                                                                                @Param("supplierCustomerId") Integer supplierCustomerId,
                                                                                @Param("fromDate") LocalDate fromDate,
                                                                                @Param("toDate") LocalDate toDate,
                                                                                @Param("documentTypeId") Integer documentTypeId,
                                                                                @Param("financialYearId") Integer financialYearId,
                                                                                @Param("branchesIds") String branchesIds);
}
