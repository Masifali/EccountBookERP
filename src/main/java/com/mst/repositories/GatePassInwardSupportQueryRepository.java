package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.projections.ItemsByGpIdRow;
import com.mst.projections.PurchaseOrderInfoForGatepassInwardRow;
import com.mst.projections.RateCutAccessWeightRow;

/**
 * Query-only repository for the gate-pass-inward support procedures of batch D2:
 * USP_GetDataRateCutInCaseOfAccessWeightReceived, USP_Validation_MarketGRNGrossWeightCompareToScaleWeight,
 * usp_GetPurchaseOrderInformationForGatepassInward, usp_getItemsFromLabOrPurchaseOrderOrAllByGpId.
 * Bound to {@link com.mst.models.InwardGatePass} (existing entity of the same domain) only to satisfy Spring Data.
 */
@Repository
public interface GatePassInwardSupportQueryRepository
        extends org.springframework.data.repository.Repository<com.mst.models.InwardGatePass, Integer> {

    /**
     * USP_GetDataRateCutInCaseOfAccessWeightReceived (single branch). {@code gdnIds} is the comma-separated list of
     * InvGrn ids (split with dbo.fnSplitString). The proc's variable
     * {@code @AdjustmentAcForRateCutAmountInCaseOfAccessWeightReceived} (config 5071) is inlined as
     * {@code CONVERT(int, dbo.GetConfigurationByConfigId(...))} both in the select list (GlAccountId) and in the
     * ChartofAccount join.
     */
    @Query(value = "SELECT H.Id AS MainId, "
         + "       H.DocNo, "
         + "       H.DocDate, "
         + "       isnull(H.GpNo,0) AS GpNo, "
         + "       CONVERT(int, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 5071)) AS GlAccountId, "
         + "       ChartofAccount.AccountTitle, "
         + "       CASE WHEN H.AccessWeight > 0 THEN H.AccessWeight ELSE gp.AccessWeight END AS Qty, "
         + "       CASE WHEN ISNULL(H.RateCut,0) > 0 THEN H.RateCut ELSE gp.RateCut END AS RateCut, "
         + "       CASE "
         + "           WHEN ISNULL(H.AccessWeight,0) > 0 And H.RateCut > 0 THEN H.AccessWeight * H.RateCut "
         + "           WHEN ISNULL(gp.AccessWeight,0) > 0 And gp.RateCut > 0 THEN gp.AccessWeight * gp.RateCut "
         + "           ELSE 0 "
         + "       END AS RateCutAmount, "
         + "       h.BiltyNo, "
         + "       h.VehicleNo "
         + "FROM InvGrn H "
         + "INNER JOIN Gatepassinward gp on gp.id = H.InwardGatePassId "
         + "LEFT JOIN ChartofAccount ON ChartofAccount.Id = CONVERT(int, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 5071)) "
         + "WHERE (H.Id in (select data from dbo.fnSplitString(:gdnIds, ','))) "
         + "And (H.AccessWeight > 0 OR gp.AccessWeight > 0)", nativeQuery = true)
    List<RateCutAccessWeightRow> getDataRateCutInCaseOfAccessWeightReceived(@Param("organizationId") Integer organizationId,
                                                                            @Param("companyId") Integer companyId,
                                                                            @Param("gdnIds") String gdnIds);

    /**
     * USP_Validation_MarketGRNGrossWeightCompareToScaleWeight (single branch).
     * // KEEP-EXEC: pure validation procedure — reads GatePassInward.RefDocumentTypeId (only acts when 105 = market
     * purchase), checks InvGrnPurchaseBreakUp exists for the GRN, computes FactoryWeight / BreakUp GrossWeight *
     * TotalBillWeight (+ short weights depending on config 249) and RAISERRORs (severity 16) when the bill weight exceeds
     * it. Returns no result set; the RAISERROR surfaces as a SQLException / DataAccessException to the caller.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_Validation_MarketGRNGrossWeightCompareToScaleWeight @GrnId = :grnId, @GpId = :gpId",
           nativeQuery = true)
    void validateMarketGrnGrossWeightCompareToScaleWeight(@Param("grnId") Integer grnId,
                                                          @Param("gpId") Integer gpId);

    /**
     * usp_GetPurchaseOrderInformationForGatepassInward (single branch). Defaults: financialYearId = null (unused by the
     * proc — its filter is commented out), branchesId = null, orderSupCustId = null, all date / doc-no filters null,
     * documentTypeId = null and expiryDays = null (both unused by the proc), ids = comma-separated DocumentTypeIds (split
     * with fnSplitString).
     * <p>
     * Temp-table pipeline ({@code #TmpResult} + {@code #TmpGRN} / {@code #TmpGatePassData} UPDATEs + {@code #Data})
     * rewritten as CTEs: the two UPDATE...FROM steps become LEFT JOINs with ISNULL(...,0) computed columns (received /
     * balance by gate pass and by GRN). Configs 122 (RejectedGatePassWeightStatus) and 273 (PurchaseOrderBranchWise) are
     * inlined in CTE {@code Cfg}. Final filter {@code BalWeight > 0} and output columns identical to the proc.
     */
    @Query(value = "WITH Cfg AS ( "
         + "  SELECT CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 122)) AS RejectedGatePassWeightStatus, "
         + "         CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 273)) AS PurchaseOrderBranchWise "
         + "), TmpResult AS ( "
         + "  SELECT D.PurchaseOrderId, "
         + "         h.DocumentTypeId, "
         + "         dt.DocumentTypeDescription, "
         + "         h.DocNo AS OrderNo, "
         + "         sp.CompanyName AS SupplierName, "
         + "         d.OrderItemId AS ItemId, "
         + "         i.ItemName, "
         + "         Uom.UOMCode AS PackUom, "
         + "         d.OrderItemQty AS OrderQty, "
         + "         d.NetWeight AS OrderWeight, "
         + "         h.RemarksHeader, "
         + "         CASE WHEN h.IsAproved = 1 THEN 'Approved' ELSE 'Not Approved' END AS ApprovedStatus, "
         + "         bb.BranchName, "
         + "         h.OrderStatus, "
         + "         h.OrderExpiryDate "
         + "  FROM PurchaseOrderDetail d "
         + "  INNER JOIN PurchaseOrder h ON d.PurchaseOrderId = h.Id "
         + "  INNER JOIN SupplierCustomer sp ON h.OrderSupCustId = sp.Id "
         + "  INNER JOIN Item i ON d.OrderItemId = i.Id "
         + "  INNER JOIN DocumentType dt ON dt.Id = h.DocumentTypeId "
         + "  INNER JOIN V_UomScheduleAndUom Uom ON d.OrderItemUOMId = Uom.Id "
         + "  LEFT JOIN Branches bb on bb.Id = h.BranchesId "
         + "  CROSS JOIN Cfg cfg "
         + "  WHERE h.OrganizationId = :organizationId "
         + "  AND h.CompanyId = :companyId "
         + "  AND ( "
         + "        (ISNULL(cfg.PurchaseOrderBranchWise,0) = 1 and h.BranchesId = :branchesId) "
         + "        OR "
         + "        (ISNULL(cfg.PurchaseOrderBranchWise,0) = 0) "
         + "      ) "
         + "  AND h.OrderStatus = 'Open' "
         + "  AND H.IsAproved = 1 "
         + "  AND h.DocumentTypeId IN (SELECT data FROM fnSplitString(:ids, ',')) "
         + "  AND (:orderSupCustId IS NULL or h.OrderSupCustId = :orderSupCustId) "
         + "  AND (:docDateFrom IS NULL OR h.DocDate >= :docDateFrom) "
         + "  AND (:docDateTo IS NULL OR h.DocDate < DATEADD(DAY, 1, :docDateTo)) "
         + "  and (:entryFromDate is null or h.EntryDate >= :entryFromDate) "
         + "  and (:entryToDate is null or h.EntryDate < DATEADD(DAY, 1, :entryToDate)) "
         + "  and (:modifyFromDate is null or h.ModifyDate >= :modifyFromDate) "
         + "  and (:modifyToDate is null or h.ModifyDate < DATEADD(DAY, 1, :modifyToDate)) "
         + "  and (:approvedFromDate is null or h.PostDate >= :approvedFromDate) "
         + "  and (:approvedToDate is null or h.PostDate < DATEADD(DAY, 1, :approvedToDate)) "
         + "  AND (:fromDocNo IS NULL OR h.DocNo >= :fromDocNo) "
         + "  and (:toDocNo is null or h.DocNo <= :toDocNo) "
         + "), TmpGRN AS ( "
         + "  SELECT GP.PurchaseOrderId, "
         + "         R.DocumentTypeId AS RefDocumentTypeId, "
         + "         GP.ItemId, "
         + "         SUM(GP.ItemQty) AS Qty, "
         + "         SUM(GP.NetBillWeight) AS NetBillWeight "
         + "  FROM InvGrnDetail GP "
         + "  INNER JOIN TmpResult R ON GP.PurchaseOrderId = R.PurchaseOrderId "
         + "  WHERE R.DocumentTypeId in (41,700) "
         + "  GROUP BY GP.PurchaseOrderId, R.DocumentTypeId, GP.ItemId "
         + "), TmpGatePassData AS ( "
         + "  SELECT GP.PurchaseOrderId, "
         + "         GP.RefDocumentTypeId, "
         + "         SUM(NoOfPackages) AS GpQty, "
         + "         SUM(WeightComparedToPoWt) AS FactoryWeight "
         + "  FROM GatePassInward GP "
         + "  INNER JOIN TmpResult R ON GP.PurchaseOrderId = R.PurchaseOrderId "
         + "  CROSS JOIN Cfg cfg "
         + "  WHERE ( "
         + "          (ISNULL(cfg.RejectedGatePassWeightStatus, 0) = CAST(0 AS BIT) AND Status <> 'Rejected') "
         + "          OR "
         + "          (ISNULL(cfg.RejectedGatePassWeightStatus, 0) = CAST(1 AS BIT)) "
         + "        ) "
         + "  AND gp.RefDocumentTypeId in (41,700) "
         + "  GROUP BY GP.PurchaseOrderId, RefDocumentTypeId "
         + "), Data AS ( "
         + "  SELECT T.DocumentTypeId, "
         + "         T.DocumentTypeDescription, "
         + "         T.OrderNo, "
         + "         T.SupplierName, "
         + "         T.ItemId, "
         + "         T.ItemName, "
         + "         T.PackUom, "
         + "         T.OrderQty, "
         + "         T.OrderWeight, "
         + "         ISNULL(G.GpQty, 0) AS ReceivedQty, "
         + "         ISNULL(G.FactoryWeight, 0) AS ReceivedWeight, "
         + "         T.OrderQty - ISNULL(G.GpQty, 0) AS BalQty, "
         + "         T.OrderWeight - ISNULL(G.FactoryWeight, 0) AS BalWeight, "
         + "         ISNULL(N.Qty, 0) AS GrnReceivedQty, "
         + "         ISNULL(N.NetBillWeight, 0) AS GrnReceivedWeight, "
         + "         T.OrderQty - ISNULL(N.Qty, 0) AS BalQtyByGrn, "
         + "         T.OrderWeight - ISNULL(N.NetBillWeight, 0) AS BalWeightByGrn, "
         + "         T.RemarksHeader, "
         + "         T.ApprovedStatus, "
         + "         T.BranchName, "
         + "         T.OrderStatus, "
         + "         T.OrderExpiryDate "
         + "  FROM TmpResult T "
         + "  LEFT JOIN TmpGRN N ON N.PurchaseOrderId = T.PurchaseOrderId AND N.ItemId = T.ItemId "
         + "  LEFT JOIN TmpGatePassData G ON G.PurchaseOrderId = T.PurchaseOrderId "
         + ") "
         + "SELECT R.DocumentTypeId, "
         + "       R.DocumentTypeDescription, "
         + "       R.OrderNo, "
         + "       R.SupplierName, "
         + "       R.ItemId, "
         + "       R.ItemName, "
         + "       R.PackUom, "
         + "       R.OrderQty, "
         + "       R.OrderWeight, "
         + "       R.ReceivedQty, "
         + "       R.ReceivedWeight, "
         + "       R.BalQty, "
         + "       R.BalWeight, "
         + "       R.GrnReceivedQty, "
         + "       R.GrnReceivedWeight, "
         + "       R.BalQtyByGrn, "
         + "       R.BalWeightByGrn, "
         + "       R.RemarksHeader, "
         + "       R.ApprovedStatus, "
         + "       R.BranchName, "
         + "       R.OrderStatus, "
         + "       R.OrderExpiryDate "
         + "FROM Data R "
         + "WHERE R.BalWeight > 0", nativeQuery = true)
    List<PurchaseOrderInfoForGatepassInwardRow> getPurchaseOrderInformationForGatepassInward(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
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
            @Param("toDocNo") Integer toDocNo,
            @Param("ids") String ids);

    /**
     * usp_getItemsFromLabOrPurchaseOrderOrAllByGpId (single branch). Default: actionId = null (proc treats null as 0;
     * actionId = 1 excludes the lab source).
     * // KEEP-EXEC: procedural IF / ELSE IF chain driven by scalar look-ups (GatePassInward.PurchaseOrderId /
     * RefDocumentTypeId / DocumentTypeId and InvLabAnalysisPurchaseHeader.Id) selecting ONE of three sources
     * (approved/accepted lab items, purchase-order items when RefDocumentTypeId in (41,700), or all non-service items when
     * RefDocumentTypeId in (105,106)) into #TmpResult, followed by two UPDATEs (ItemWbWeight from WbTransactions when a
     * lab exists; Moisture from PurchaseOrderDetail when 0). Output columns: ItemId, ItemName, ItemCode, PoDetailId,
     * ItemWbWeight, Moisture.
     */
    @Query(value = "EXEC dbo.usp_getItemsFromLabOrPurchaseOrderOrAllByGpId @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @Id = :id, @ActionId = :actionId", nativeQuery = true)
    List<ItemsByGpIdRow> getItemsFromLabOrPurchaseOrderOrAllByGpId(@Param("organizationId") Integer organizationId,
                                                                   @Param("companyId") Integer companyId,
                                                                   @Param("id") Integer id,
                                                                   @Param("actionId") Integer actionId);
}
