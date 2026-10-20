package com.mst.repositories;

import java.time.LocalDateTime;
import java.math.BigDecimal;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvGrnDetail;
import com.mst.projections.InvGrnDetailRow;
import com.mst.projections.InvGrnDetailStoreRow;
import com.mst.projections.InvGrnDetailTradingRow;

/**
 * Repository for dbo.InvGrnDetail.
 *
 * Covers Sp_InvGrnDetail_GetAllMethod (ReadByInvGrnID, ReadByInvGrnID_Engr, ReadByInvGrnIDTrading, ReadByInvGrnIDStore,
 * ReadByInvGrnIdDirect; ReadByInvGrnIdEmptyBagsDetail lives in InvGrnDetailEmptyBagsRepository) and
 * Sp_InvGrnDetail_Insert.
 *
 * Sp_InvGrnDetail_Insert pre-insert logic (in plain words; see {@link #spInvGrnDetailInsert} which keeps it all):
 * - reads header data from InvGrn (@InvGrnId): OrganizationId, CompanyId, DocumentTypeId, SupplierCustomerId, DocDate,
 *   InwardGatePassId, PartyWeight, FactoryWeight, FinancialYearId, EntryUser, GrnTypeId, DeliveryTerm;
 * - reads GpDate / RefDocumentTypeId from GatePassInward and DocNo / Id from InvLabAnalysisPurchaseHeader
 *   (LabReportRef defaults to the lab DocNo when empty);
 * - config 36 (order validate), 165 (tolerance), 412, 217, 103..106 via dbo.GetConfigurationByConfigId;
 * - validations: DocDate not in future, GpDate <= DocDate, PO DocDate <= DocDate, ItemQty / NetBillWeight /
 *   StockWeight > 0, ItemId / WarehouseId required, empty-bag min/max weight per InvPackingType, EB total vs
 *   gross weight, DocumentType 46/169 mandatory fields, RefDocumentType 41 PO checks, PO balance weight / qty checks
 *   (dbo.GetPurchaseOrderBalanceWeightafterGrn, GetPurchaseOrderandGatepassQtyBalance, GetPurchaseOrderAndGrnQtyBalance,
 *   GetPurchaseOrderBalanceQty), challan / demand balance (Fun_DeliveryChallan_GetBalanceQty,
 *   Fun_PurchaseDemand_GetBalanceQty), weigh-bridge vs gross weight (WbTransactions), GDN purchase-return existence,
 *   WbTicket already used (InvGrnDetail / InvGdnDetail / InvStockTransferHeaderPartyProcessing),
 *   returnable GDN balance (GetReturnableGdnBalanceQty);
 * - side effects: UPDATE PurchaseOrder SET OrderStatus='Complete' when the order is fully received,
 *   UPDATE GatePassInward SET VarietyName/ItemId for market/gate purchase, UPDATE WbTransactions ItemId/ItemDescription;
 * - Id = ISNULL(MAX(Id),0)+1 (see {@link #maxId()}), then INSERT, then SELECT @Id.
 */
@Repository
public interface InvGrnDetailRepository extends JpaRepository<InvGrnDetail, Integer> {

    /** Sp_InvGrnDetail_Insert: Id = ISNULL(MAX(CONVERT(INT,Id)),0)+1 (app-assigned id). */
    @Query("select coalesce(max(e.id),0) from InvGrnDetail e")
    Integer maxId();

    /** Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnID' (@Id = InvGrnId). Native: V_UomScheduleAndUom etc. */
    @Query(nativeQuery = true, value =
            "SELECT d.Id, d.ItemId, Item.ItemName AS Item, d.ItemUomId, d.GatePassInwarDetailId, "
            + " d.ItemQty, d.WarehouseId, d.JobLotId, JobLot.JobLotDescription AS JobLot, "
            + " JobLot.JobLotDescription, InvWareHouse.WareHouseCode, d.PackingTypeId, "
            + " PT.PackTypeDesc AS PackingType, d.LabReportRef, d.AdLsWeight, "
            + " Item.ItemName, Item.ItemCodeNew ItemCode, UOM.Equivalent AS UOM, uom.UOMCode, "
            + " d.InvGrnId, "
            + " d.PurchaseOrderId, PurchaseOrder.DocNo AS PurchaseOrder, "
            + " d.CropYear, d.GrossWeight, d.EBWPerUnit, d.EBWTotal, d.NetBillWeight, d.StockWeight, d.AreaCity, d.CommentsDetail, d.WtCut, d.WtCutTotal, "
            + " d.SupplySchedulId, InvWareHouse.WareHouseCode AS Expr1, Gp.GpSrNo AS GpNo, sh.DocNo AS SupplyScheduleNo, PurchaseOrder.DeliveryTerm, "
            + " InvWareHouse.WareHouseName, "
            + " d.InvPurchasedemondId, "
            + " d.PurchaseOrderDetailId, PurchaseOrder.DocNo as PurchaserOrderNo, "
            + " d.ContractorId, C.CompanyName AS Contractor, d.ScaleKart, '' PurchaseOrderDate, "
            + " d.PurchaseDemondDetailId, pd.DocNo PurchaseDemandNo, "
            + " d.EBCutApprovedUserId, EBCutDate, EBCutApprovedUserRemarks, "
            + " d.CityId, d.GdnId, d.GdnDetailId, d.ConditionId, "
            + " d.AssetId, ast.AssetName, lp.LookUpName as ItemCondition, "
            + " d.WareHouseFromId, warto.WareHouseName as WareHouseFromCode, D.EbPurAgainstWeight, D.WbTicketId, wb.TicketNo, "
            + " d.RefDocumentTypeId, d.RefDocNoId, d.RefDocSubIdNo, d.LineId, D.ScaleShortWeight, "
            + " d.SupplierShortWeight, d.WeightCutOnId, d.FreightAmount, d.LabId, LabP.DocNo LabNo, d.QtyForWtCut, "
            + " case when d.WeightCutOnId = 1 then 'Qty' when d.WeightCutOnId = 2 then '40kg' when d.WeightCutOnId = 3 then '100kg' else 'Qty' End WeightCutOn, LabP.WeightCutUom, "
            + " d.PackingDate, d.ExpiryDate, "
            + " d.CropYearId, d.GdnDocumentTypeId, Gd.DocNo GdnNo, d.SupplierQty, d.StockEbUnit, d.StockEbTotal, "
            + " d.ItemConditionId, VIC.ConditionStatus, "
            + " d.RackId, R.rackName, "
            + " d.GainLossWeightDetail, "
            + " CAST('' as date) as PurchaseDemandDocDate, "
            + " CAST(0 as Float) as PurchaseDemandQty, "
            + " CAST(0 as Float) as UsedDemandQtyInPreBill, "
            + " CAST(0 as Float) as UsedDemandQtyInGrn, "
            + " CAST(0 as Float) as TotalUsedDemandQty, "
            + " CAST(0 as Float) as BalDemandQty, "
            + " CAST(0 as Float) as PurchaseOrderQty, "
            + " CAST(0 as Float) as UsedOrderQtyInGrn, "
            + " CAST(0 as Float) as UsedOrderQtyInPreBill, "
            + " CAST(0 as Float) as TotalUsedOrderQty, "
            + " CAST(0 as Float) as BalOrderQty, "
            + " CAST(0 as int) as PurchasePreBillId, "
            + " CAST(0 as int) as PurchasePreBillDetailId, "
            + " CAST(0 as int) as PurchasePreBillNo, "
            + " CAST(0 as Float) as PurchasePreBillQty, "
            + " d.DeliveryChallanId, "
            + " d.DeliveryChallanDetailId, "
            + " CAST(0 as int) as DeliveryChallanNo, "
            + " CAST('' as date) as DeliveryChallanDate, "
            + " CAST(0 as Float) as DeliveryChallanQty, "
            + " CAST(0 as Float) as UsedDeliveryChallanQtyGrn, "
            + " CAST(0 as Float) as BalDeliveryChallanQty "
            + "FROM InvGrnDetail d "
            + "INNER JOIN InvGrn AS g ON d.InvGrnId = g.Id "
            + "INNER JOIN Item ON Item.Id = d.ItemId "
            + "INNER JOIN V_UomScheduleAndUom AS UOM ON d.ItemUomId = UOM.Id "
            + "INNER JOIN InvWareHouse ON d.WarehouseId = InvWareHouse.Id "
            + "LEFT JOIN JobLot ON d.JobLotId = JobLot.Id "
            + "LEFT JOIN InvPackingType AS PT ON d.PackingTypeId = PT.Id "
            + "LEFT JOIN PurchaseOrder ON d.PurchaseOrderId = PurchaseOrder.Id "
            + "LEFT JOIN SupplyScheduleDetail AS shd ON d.SupplySchedulId = shd.Id "
            + "LEFT JOIN SupplySchedule AS sh ON shd.SupplyScheduleId = sh.Id "
            + "LEFT JOIN PurchaseOrderDetail ON PurchaseOrder.Id = PurchaseOrderDetail.PurchaseOrderId and d.PurchaseOrderDetailId = PurchaseOrderDetail.Id "
            + "LEFT JOIN GatePassInwardDetail AS Gpd ON d.GatePassInwarDetailId = Gpd.Id "
            + "LEFT JOIN GatePassInward AS Gp ON g.InwardGatePassId = Gp.Id "
            + "LEFT JOIN SupplierCustomer C ON d.ContractorId = C.Id "
            + "LEFT JOIN FixedAssetsRegister ast on d.AssetId = ast.Id "
            + "LEFT JOIN LookUpsForERP lp on d.Id = lp.Id "
            + "Left join InvPurchaseDemandHeader pd on pd.Id = d.InvPurchasedemondId "
            + "LEFT JOIN InvWareHouse warto ON warto.Id = d.WareHouseFromId "
            + "LEFT JOIN WbTransactions wb on d.WbTicketId = wb.Id "
            + "Left join InvLabAnalysisPurchaseHeader labp on labp.Id = d.LabId "
            + "LEft Join InvGdn Gd on gd.Id = d.GdnId "
            + "LEFT Join V_ItemCondition VIC on VIC.Id = d.ItemConditionId "
            + "LEFT JOIN invWarehouseRack R ON d.RackId = R.Id "
            + "WHERE d.[InvGrnId] = :id "
    )
    List<InvGrnDetailRow> readByInvGrnId(@Param("id") Integer id);

    /** Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnID_Engr' (@Id = InvGrnId; ItemCondition joined on ConditionId). */
    @Query(nativeQuery = true, value =
            "SELECT InvGrnDetail.Id, InvGrnDetail.ItemId, Item.ItemName AS Item, InvGrnDetail.ItemUomId, InvGrnDetail.GatePassInwarDetailId, InvGrnDetail.ItemQty, InvGrnDetail.WarehouseId, InvGrnDetail.JobLotId, "
            + " JobLot.JobLotDescription AS JobLot, InvGrnDetail.PackingTypeId, PT.PackTypeDesc AS PackingType, InvGrnDetail.LabReportRef, InvGrnDetail.AdLsWeight, Item.ItemName, UOM.Equivalent AS UOM, uom.UOMCode, "
            + " JobLot.JobLotDescription, InvWareHouse.WareHouseCode, InvGrnDetail.InvGrnId, InvGrnDetail.PurchaseOrderId, PurchaseOrder.DocNo AS PurchaseOrder, InvGrnDetail.CropYear, InvGrnDetail.GrossWeight, "
            + " InvGrnDetail.EBWPerUnit, InvGrnDetail.EBWTotal, InvGrnDetail.NetBillWeight, InvGrnDetail.StockWeight, InvGrnDetail.AreaCity, InvGrnDetail.CommentsDetail, InvGrnDetail.WtCut, InvGrnDetail.WtCutTotal, "
            + " InvGrnDetail.SupplySchedulId, InvWareHouse.WareHouseCode AS Expr1, Gp.GpSrNo AS GpNo, sh.DocNo AS SupplyScheduleNo, PurchaseOrder.DeliveryTerm, "
            + " InvWareHouse.WareHouseName, InvGrnDetail.InvPurchasedemondId, InvGrnDetail.PurchaseOrderDetailId, PurchaseOrder.DocNo as PurchaserOrderNo, "
            + " InvGrnDetail.ContractorId, C.CompanyName AS Contractor, InvGrnDetail.ScaleKart, Item.ItemCodeNew ItemCode, '' PurchaseOrderDate, InvGrnDetail.PurchaseDemondDetailId, pd.DocNo PurchaseDemandNo, "
            + " InvGrnDetail.EBCutApprovedUserId, EBCutDate, EBCutApprovedUserRemarks, InvGrnDetail.CityId, "
            + " InvGrnDetail.GdnId, InvGrnDetail.GdnDetailId, InvGrnDetail.ConditionId, InvGrnDetail.AssetId, "
            + " ast.AssetName, lp.LookUpName as ItemCondition, InvGrnDetail.WareHouseFromId, warto.WareHouseName as WareHouseFromCode, "
            + " InvGrnDetail.EbPurAgainstWeight, InvGrnDetail.WbTicketId, wb.TicketNo, "
            + " InvGrnDetail.RefDocumentTypeId, InvGrnDetail.RefDocNoId, InvGrnDetail.RefDocSubIdNo, InvGrnDetail.LineId, "
            + " InvGrnDetail.ScaleShortWeight, InvGrnDetail.SupplierShortWeight, "
            + " InvGrnDetail.WeightCutOnId, InvGrnDetail.FreightAmount, InvGrnDetail.LabId, LabP.DocNo LabNo, InvGrnDetail.QtyForWtCut, "
            + " case when InvGrnDetail.WeightCutOnId = 1 then 'Qty' when InvGrnDetail.WeightCutOnId = 2 then '40kg' when InvGrnDetail.WeightCutOnId = 3 then '100kg' else 'Qty' End WeightCutOn, LabP.WeightCutUom, "
            + " [InvGrnDetail].PackingDate, [InvGrnDetail].ExpiryDate, "
            + " [InvGrnDetail].CropYearId, InvGrnDetail.GdnDocumentTypeId, Gd.DocNo GdnNo, InvGrnDetail.SupplierQty, InvGrnDetail.StockEbUnit, InvGrnDetail.StockEbTotal, "
            + " [InvGrnDetail].ItemConditionId, VIC.ConditionStatus, "
            + " InvGrnDetail.RackId, R.rackName, "
            + " InvGrnDetail.GainLossWeightDetail, "
            + " CAST('' as date) as PurchaseDemandDocDate, "
            + " CAST(0 as Float) as PurchaseDemandQty, "
            + " CAST(0 as Float) as UsedDemandQtyInPreBill, "
            + " CAST(0 as Float) as UsedDemandQtyInGrn, "
            + " CAST(0 as Float) as TotalUsedDemandQty, "
            + " CAST(0 as Float) as BalDemandQty, "
            + " CAST(0 as Float) as PurchaseOrderQty, "
            + " CAST(0 as Float) as UsedOrderQtyInGrn, "
            + " CAST(0 as Float) as UsedOrderQtyInPreBill, "
            + " CAST(0 as Float) as TotalUsedOrderQty, "
            + " CAST(0 as Float) as BalOrderQty, "
            + " CAST(0 as int) as PurchasePreBillId, "
            + " CAST(0 as int) as PurchasePreBillDetailId, "
            + " CAST(0 as int) as PurchasePreBillNo, "
            + " CAST(0 as Float) as PurchasePreBillQty, "
            + " InvGrnDetail.DeliveryChallanId, "
            + " InvGrnDetail.DeliveryChallanDetailId, "
            + " CAST(0 as int) as DeliveryChallanNo, "
            + " CAST('' as date) as DeliveryChallanDate, "
            + " CAST(0 as Float) as DeliveryChallanQty, "
            + " CAST(0 as Float) as UsedDeliveryChallanQtyGrn, "
            + " CAST(0 as Float) as BalDeliveryChallanQty "
            + "FROM InvGrnDetail "
            + "INNER JOIN InvGrn AS g ON InvGrnDetail.InvGrnId = g.Id "
            + "INNER JOIN Item ON Item.Id = InvGrnDetail.ItemId "
            + "INNER JOIN V_UomScheduleAndUom AS UOM ON InvGrnDetail.ItemUomId = UOM.Id "
            + "INNER JOIN InvWareHouse ON InvGrnDetail.WarehouseId = InvWareHouse.Id "
            + "LEFT JOIN JobLot ON InvGrnDetail.JobLotId = JobLot.Id "
            + "LEFT JOIN InvPackingType AS PT ON InvGrnDetail.PackingTypeId = PT.Id "
            + "LEFT JOIN PurchaseOrder ON InvGrnDetail.PurchaseOrderId = PurchaseOrder.Id "
            + "LEFT JOIN SupplyScheduleDetail AS shd ON InvGrnDetail.SupplySchedulId = shd.Id "
            + "LEFT JOIN SupplySchedule AS sh ON shd.SupplyScheduleId = sh.Id "
            + "LEFT JOIN PurchaseOrderDetail ON PurchaseOrder.Id = PurchaseOrderDetail.PurchaseOrderId and InvGrnDetail.PurchaseOrderDetailId = PurchaseOrderDetail.Id "
            + "LEFT JOIN GatePassInwardDetail AS Gpd ON InvGrnDetail.GatePassInwarDetailId = Gpd.Id "
            + "LEFT JOIN GatePassInward AS Gp ON g.InwardGatePassId = Gp.Id "
            + "LEFT JOIN SupplierCustomer C ON InvGrnDetail.ContractorId = C.Id "
            + "LEFT JOIN FixedAssetsRegister ast on InvGrnDetail.AssetId = ast.Id "
            + "LEFT JOIN WbTransactions wb on InvGrnDetail.WbTicketId = wb.Id "
            + "LEFT JOIN LookUpsForERP lp on InvGrnDetail.ConditionId = lp.Id "
            + "LEFT JOIN InvPurchaseDemandHeader pd on pd.Id = InvGrnDetail.InvPurchasedemondId "
            + "LEFT JOIN InvWareHouse warto ON warto.Id = InvGrnDetail.WareHouseFromId "
            + "Left join InvLabAnalysisPurchaseHeader labp on labp.Id = InvGrnDetail.LabId "
            + "LEft Join InvGdn Gd on gd.Id = InvGrnDetail.GdnId "
            + "LEFT Join V_ItemCondition VIC on VIC.Id = [InvGrnDetail].ItemConditionId "
            + "LEFT JOIN invWarehouseRack R ON InvGrnDetail.RackId = R.Id "
            + "WHERE [dbo].[InvGrnDetail].[InvGrnId] = :id "
    )
    List<InvGrnDetailRow> readByInvGrnIdEngr(@Param("id") Integer id);

    /** Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnIDTrading' (@Id = InvGrnId; packing type from ExImPackMaterilaType). */
    @Query(nativeQuery = true, value =
            "SELECT InvGrnDetail.Id, InvGrnDetail.ItemId, Item.ItemName AS Item, InvGrnDetail.ItemUomId, InvGrnDetail.ItemQty, InvGrnDetail.WarehouseId, InvGrnDetail.JobLotId, JobLot.JobLotDescription AS JobLot, InvGrnDetail.PackingTypeId, "
            + " PT.Description AS PackingType, InvGrnDetail.LabReportRef, InvGrnDetail.AdLsWeight, Item.ItemName, UOM.Equivalent AS UOM, uom.UOMCode, JobLot.JobLotDescription, InvWareHouse.WareHouseCode, "
            + " InvGrnDetail.InvGrnId, InvGrnDetail.PurchaseOrderId, PurchaseOrder.DocNo AS PurchaseOrder, InvGrnDetail.CropYear, InvGrnDetail.GrossWeight, InvGrnDetail.EBWPerUnit, InvGrnDetail.EBWTotal, InvGrnDetail.NetBillWeight, "
            + " InvGrnDetail.StockWeight, InvGrnDetail.AreaCity, InvGrnDetail.CommentsDetail, InvGrnDetail.WtCut, InvGrnDetail.WtCutTotal, InvGrnDetail.SupplySchedulId, sh.DocNo AS SupplyScheduleNo, "
            + " InvGrnDetail.GatePassInwarDetailId, InvGrnDetail.InvPurchasedemondId, gp.GpSrNo AS GpNo, InvGrnDetail.PurchaseOrderDetailId, InvGrnDetail.PurchaserOrderNo, PurchaseOrder.DeliveryTerm, "
            + " InvGrnDetail.ContractorId, C.CompanyName AS Contractor, InvGrnDetail.ScaleKart, Item.ItemCodeNew ItemCode, '' PurchaseOrderDate, InvGrnDetail.PurchaseDemondDetailId, pd.DocNo PurchaseDemandNo, "
            + " InvGrnDetail.EBCutApprovedUserId, EBCutDate, EBCutApprovedUserRemarks, InvGrnDetail.CityId, "
            + " InvGrnDetail.GdnId, InvGrnDetail.GdnDetailId, InvGrnDetail.ConditionId, InvGrnDetail.AssetId, "
            + " ast.AssetName, lp.LookUpName as ItemCondition, InvGrnDetail.WareHouseFromId, warto.WareHouseName as WareHouseFromCode, "
            + " InvGrnDetail.EbPurAgainstWeight, InvGrnDetail.WbTicketId, wb.TicketNo, "
            + " InvGrnDetail.RefDocumentTypeId, InvGrnDetail.RefDocNoId, InvGrnDetail.RefDocSubIdNo, InvGrnDetail.LineId, "
            + " InvGrnDetail.ScaleShortWeight, InvGrnDetail.SupplierShortWeight, InvGrnDetail.WeightCutOnId, InvGrnDetail.FreightAmount, "
            + " InvGrnDetail.LabId, LabP.DocNo LabNo, InvGrnDetail.QtyForWtCut, "
            + " case when InvGrnDetail.WeightCutOnId = 1 then 'Qty' when InvGrnDetail.WeightCutOnId = 2 then '40kg' when InvGrnDetail.WeightCutOnId = 3 then '100kg' else 'Qty' End WeightCutOn, LabP.WeightCutUom, "
            + " [InvGrnDetail].PackingDate, [InvGrnDetail].ExpiryDate, "
            + " [InvGrnDetail].CropYearId, InvGrnDetail.GdnDocumentTypeId, Gd.DocNo GdnNo, InvGrnDetail.SupplierQty, InvGrnDetail.StockEbUnit, InvGrnDetail.StockEbTotal, "
            + " [InvGrnDetail].ItemConditionId, VIC.ConditionStatus, "
            + " InvGrnDetail.RackId, R.rackName, "
            + " InvGrnDetail.GainLossWeightDetail, "
            + " CAST('' as date) as PurchaseDemandDocDate, "
            + " CAST(0 as Float) as PurchaseDemandQty, "
            + " CAST(0 as Float) as UsedDemandQtyInPreBill, "
            + " CAST(0 as Float) as UsedDemandQtyInGrn, "
            + " CAST(0 as Float) as TotalUsedDemandQty, "
            + " CAST(0 as Float) as BalDemandQty, "
            + " CAST(0 as Float) as PurchaseOrderQty, "
            + " CAST(0 as Float) as UsedOrderQtyInGrn, "
            + " CAST(0 as Float) as UsedOrderQtyInPreBill, "
            + " CAST(0 as Float) as TotalUsedOrderQty, "
            + " CAST(0 as Float) as BalOrderQty, "
            + " CAST(0 as int) as PurchasePreBillId, "
            + " CAST(0 as int) as PurchasePreBillDetailId, "
            + " CAST(0 as int) as PurchasePreBillNo, "
            + " CAST(0 as Float) as PurchasePreBillQty, "
            + " InvGrnDetail.DeliveryChallanId, "
            + " InvGrnDetail.DeliveryChallanDetailId, "
            + " CAST(0 as int) as DeliveryChallanNo, "
            + " CAST('' as date) as DeliveryChallanDate, "
            + " CAST(0 as Float) as DeliveryChallanQty, "
            + " CAST(0 as Float) as UsedDeliveryChallanQtyGrn, "
            + " CAST(0 as Float) as BalDeliveryChallanQty "
            + "FROM PurchaseOrder "
            + " INNER JOIN dbo.PurchaseOrderDetail pod ON dbo.PurchaseOrder.Id = pod.PurchaseOrderId RIGHT OUTER JOIN "
            + " InvGrnDetail INNER JOIN "
            + " SupplyScheduleDetail INNER JOIN "
            + " SupplySchedule AS sh ON SupplyScheduleDetail.SupplyScheduleId = sh.Id ON InvGrnDetail.SupplySchedulId = SupplyScheduleDetail.Id "
            + " LEFT OUTER JOIN Item ON Item.Id = InvGrnDetail.ItemId "
            + " INNER JOIN dbo.V_UomScheduleAndUom UOM ON UOM.Id = InvGrnDetail.ItemUomId "
            + " LEFT OUTER JOIN JobLot ON JobLot.Id = InvGrnDetail.JobLotId "
            + " LEFT OUTER JOIN InvWareHouse ON InvWareHouse.Id = InvGrnDetail.WarehouseId "
            + " LEFT OUTER JOIN ExImPackMaterilaType AS PT ON PT.Id = InvGrnDetail.PackingTypeId ON pod.Id = InvGrnDetail.PurchaseOrderId "
            + " Left outer join GatePassInwardDetail gpd on InvGrnDetail.GatePassInwarDetailId = gpd.Id "
            + " lEFT OUTER join GatePassInward Gp on Gp.Id = gpd.GatePassInwardId "
            + " LEFT JOIN SupplierCustomer C ON InvGrnDetail.ContractorId = C.Id "
            + " LEFT JOIN FixedAssetsRegister ast on InvGrnDetail.AssetId = ast.Id "
            + " LEFT JOIN LookUpsForERP lp on InvGrnDetail.Id = lp.Id "
            + " Left join InvPurchaseDemandHeader pd on pd.Id = InvGrnDetail.InvPurchasedemondId "
            + " LEFT JOIN InvWareHouse warto ON warto.Id = InvGrnDetail.WareHouseFromId "
            + " LEFT JOIN WbTransactions wb on InvGrnDetail.WbTicketId = wb.Id "
            + " Left join InvLabAnalysisPurchaseHeader labp on labp.Id = InvGrnDetail.LabId "
            + " LEft Join InvGdn Gd on gd.Id = InvGrnDetail.GdnId "
            + " LEFT Join V_ItemCondition VIC on VIC.Id = [InvGrnDetail].ItemConditionId "
            + " LEFT JOIN invWarehouseRack R ON InvGrnDetail.RackId = R.Id "
            + "WHERE [dbo].[InvGrnDetail].[InvGrnId] = :id "
    )
    List<InvGrnDetailTradingRow> readByInvGrnIdTrading(@Param("id") Integer id);

    /**
     * Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnIDStore' (@Id = InvGrnId).
     * The #PurchasePreBillDetail temp table and its 9 UPDATE steps (order / demand / challan / pre-bill numbers and the
     * already-used quantities) are rewritten as CTEs joined to the base rows; defaults (0, CAST('' AS date)) are kept
     * for unmatched rows exactly like the procedure.
     */
    @Query(nativeQuery = true, value =
            "WITH grd AS ( "
            + " SELECT grd.Id, grd.ItemId, grd.ItemQty, grd.ItemUomId, grd.WarehouseId, grd.JobLotId, grd.PackingTypeId, "
            + "  grd.LabReportRef, grd.AdLsWeight, grd.InvGrnId, grd.PurchaseOrderId, grd.CropYear, grd.GrossWeight, "
            + "  grd.EBWPerUnit, grd.EBWTotal, grd.NetBillWeight, grd.StockWeight, grd.AreaCity, grd.CommentsDetail, "
            + "  grd.WtCut, grd.WtCutTotal, grd.SupplySchedulId, grd.GatePassInwarDetailId, grd.InvPurchasedemondId, "
            + "  grd.PurchaseOrderDetailId, grd.PurchaserOrderNo, grd.ContractorId, grd.ScaleKart, grd.CityId, grd.PurchaseDemondDetailId, "
            + "  grd.EBCutApprovedUserId, grd.EBCutDate, grd.EBCutApprovedUserRemarks, grd.GdnId, grd.GdnDetailId, grd.ConditionId, grd.AssetId, "
            + "  grd.WareHouseFromId, grd.EbPurAgainstWeight, grd.WbTicketId, grd.RefDocumentTypeId, grd.RefDocNoId, grd.RefDocSubIdNo, grd.LineId, "
            + "  grd.ScaleShortWeight, grd.SupplierShortWeight, grd.WeightCutOnId, grd.FreightAmount, grd.LabId, grd.QtyForWtCut, "
            + "  grd.PackingDate, grd.ExpiryDate, grd.CropYearId, grd.GdnDocumentTypeId, grd.SupplierQty, grd.StockEbUnit, grd.StockEbTotal, "
            + "  grd.ItemConditionId, grd.RackId, grd.GainLossWeightDetail, grd.DeliveryChallanId, grd.DeliveryChallanDetailId "
            + " FROM InvGrnDetail grd WHERE grd.InvGrnId = :id "
            + "), TmpOrder AS ( "
            + " select r.PurchaseOrderId, r.PurchaseOrderDetailId, h.DocNo, h.DocDate, ISNULL(sum(d.OrderItemQty),0) as OrderQty "
            + " from grd r "
            + " inner join dbo.PurchaseOrderDetail d on r.PurchaseOrderId = d.PurchaseOrderId and r.PurchaseOrderDetailId = d.Id "
            + " inner join dbo.PurchaseOrder h on h.Id = d.PurchaseOrderId "
            + " Where ISNULL(h.ActionId,0) <> 3 and ISNULL(d.ActionTypeId,0) <> 3 "
            + " GROUP BY r.PurchaseOrderId, r.PurchaseOrderDetailId, h.DocNo, h.DocDate "
            + "), TmpDemand AS ( "
            + " select r.InvPurchasedemondId, r.PurchaseDemondDetailId, h.DocNo, h.DocDate, ISNULL(sum(d.ApprovedQty),0) as DemandQty "
            + " from grd r "
            + " inner join dbo.InvPurchaseDemandDetail d on r.InvPurchasedemondId = d.InvPurchaseDemandHeaderId and r.PurchaseDemondDetailId = d.Id "
            + " inner join dbo.InvPurchaseDemandHeader h on h.Id = d.InvPurchaseDemandHeaderId "
            + " GROUP BY r.InvPurchasedemondId, r.PurchaseDemondDetailId, h.DocNo, h.DocDate "
            + "), TmpChallan AS ( "
            + " select r.DeliveryChallanId, r.DeliveryChallanDetailId, h.DocNo, h.DocDate, ISNULL(sum(d.Qty),0) as ChallanQty "
            + " from grd r "
            + " inner join dbo.DeliveryChallanDetail d on r.DeliveryChallanId = d.DeliveryChallanHeaderId and r.DeliveryChallanDetailId = d.DeliveryChallanDetailId "
            + " inner join dbo.DeliveryChallanHeader h on h.DeliveryChallanHeaderId = d.DeliveryChallanHeaderId "
            + " GROUP BY r.DeliveryChallanId, r.DeliveryChallanDetailId, h.DocNo, h.DocDate "
            + "), TmpPreBill AS ( "
            + " select r.DeliveryChallanId, r.DeliveryChallanDetailId, pd.PurchasePreBillHeaderId, pd.PurchasePreBillDetailId, ph.DocNo, ph.DocDate, ISNULL(sum(pd.Qty),0) as PurchasePreBillQty "
            + " from grd r "
            + " inner join dbo.DeliveryChallanDetail d on r.DeliveryChallanId = d.DeliveryChallanHeaderId and r.DeliveryChallanDetailId = d.DeliveryChallanDetailId "
            + " inner join dbo.DeliveryChallanHeader h on h.DeliveryChallanHeaderId = d.DeliveryChallanHeaderId "
            + " inner join dbo.PurchasePreBillDetail pd on pd.PurchasePreBillDetailId = d.PurchasePreBillDetailId and pd.PurchasePreBillHeaderId = d.PurchasePreBillHeaderId "
            + " inner join dbo.PurchasePreBillHeader ph on ph.PurchasePreBillHeaderId = pd.PurchasePreBillHeaderId "
            + " GROUP BY r.DeliveryChallanId, r.DeliveryChallanDetailId, pd.PurchasePreBillHeaderId, pd.PurchasePreBillDetailId, ph.DocNo, ph.DocDate "
            + "), TmpAlreadyUsedDemandQtyInPreBill AS ( "
            + " select r.InvPurchasedemondId, r.PurchaseDemondDetailId, ISNULL(sum(d.Qty),0) as PreBillQty "
            + " from grd r "
            + " inner join dbo.PurchasePreBillDetail d on r.InvPurchasedemondId = d.PurchaseDemandHeaderId and r.PurchaseDemondDetailId = d.PurchaseDemandDetailId "
            + " inner join dbo.PurchasePreBillHeader h on h.PurchasePreBillHeaderId = d.PurchasePreBillHeaderId AND ISNULL(h.ActionId,0) <> 3 and ISNULL(d.ActionTypeId,0) <> 3 "
            + " GROUP BY r.InvPurchasedemondId, r.PurchaseDemondDetailId "
            + "), TmpAlreadyUsedDemandQtyInGrn AS ( "
            + " select r.InvPurchasedemondId, r.PurchaseDemondDetailId, ISNULL(sum(d.ItemQty),0) as GrnQty "
            + " from grd r "
            + " inner join dbo.InvGrnDetail d on r.InvPurchasedemondId = d.InvPurchasedemondId and r.PurchaseDemondDetailId = d.PurchaseDemondDetailId "
            + " inner join dbo.InvGrn h on h.ID = d.InvGrnId "
            + " Where ISNULL(d.DeliveryChallanId,0) = 0 AND ISNULL(d.DeliveryChallanDetailId,0) = 0 AND d.Id <> r.Id "
            + " GROUP BY r.InvPurchasedemondId, r.PurchaseDemondDetailId "
            + "), TmpAlreadyUsedOrderQtyInPreBill AS ( "
            + " select r.PurchaseOrderId, r.PurchaseOrderDetailId, ISNULL(sum(d.Qty),0) as PreBillQty "
            + " from grd r "
            + " inner join dbo.PurchasePreBillDetail d on r.PurchaseOrderId = d.PurchaseOrderHeaderId and r.PurchaseOrderDetailId = d.PurchaseOrderDetailId "
            + " inner join dbo.PurchasePreBillHeader h on h.PurchasePreBillHeaderId = d.PurchasePreBillHeaderId AND ISNULL(h.ActionId,0) <> 3 and ISNULL(d.ActionTypeId,0) <> 3 "
            + " GROUP BY r.PurchaseOrderId, r.PurchaseOrderDetailId "
            + "), TmpAlreadyUsedOrderQtyInGrn AS ( "
            + " select r.PurchaseOrderId, r.PurchaseOrderDetailId, ISNULL(sum(d.ItemQty),0) as GrnQty "
            + " from grd r "
            + " inner join dbo.InvGrnDetail d on r.PurchaseOrderId = d.PurchaseOrderId and r.PurchaseOrderDetailId = d.PurchaseOrderDetailId "
            + " inner join dbo.InvGrn h on h.ID = d.InvGrnId "
            + " Where ISNULL(d.DeliveryChallanId,0) = 0 AND ISNULL(d.DeliveryChallanDetailId,0) = 0 AND d.Id <> r.Id "
            + " GROUP BY r.PurchaseOrderId, r.PurchaseOrderDetailId "
            + "), TmpAlreadyUsedChallanQtyInGrn AS ( "
            + " select r.DeliveryChallanId, r.DeliveryChallanDetailId, ISNULL(sum(d.ItemQty),0) as GrnQty "
            + " from grd r "
            + " inner join dbo.InvGrnDetail d on r.DeliveryChallanId = d.DeliveryChallanId and r.DeliveryChallanDetailId = d.DeliveryChallanDetailId "
            + " inner join dbo.InvGrn h on h.ID = d.InvGrnId "
            + " Where ISNULL(d.DeliveryChallanId,0) > 0 AND ISNULL(d.DeliveryChallanDetailId,0) > 0 AND d.Id <> r.Id "
            + " GROUP BY r.DeliveryChallanId, r.DeliveryChallanDetailId "
            + "), Calc AS ( "
            + " SELECT grd.*, "
            + "  CASE WHEN o.PurchaseOrderId IS NULL THEN CAST(0 as int) ELSE ISNULL(o.DocNo,0) END AS PurchaseOrder, "
            + "  CASE WHEN o.PurchaseOrderId IS NULL THEN CAST('' as Date) ELSE o.DocDate END AS PurchaseOrderDate, "
            + "  CASE WHEN o.PurchaseOrderId IS NULL THEN CAST(0 as float) ELSE ISNULL(o.OrderQty,0) END AS PurchaseOrderQty, "
            + "  CASE WHEN dm.InvPurchasedemondId IS NULL THEN CAST(0 as int) ELSE ISNULL(dm.DocNo,0) END AS PurchaseDemandNo, "
            + "  CASE WHEN dm.InvPurchasedemondId IS NULL THEN CAST('' as Date) ELSE dm.DocDate END AS PurchaseDemandDocDate, "
            + "  CASE WHEN dm.InvPurchasedemondId IS NULL THEN CAST(0 as float) ELSE ISNULL(dm.DemandQty,0) END AS PurchaseDemandQty, "
            + "  CASE WHEN ch.DeliveryChallanId IS NULL THEN CAST(0 as int) ELSE ISNULL(ch.DocNo,0) END AS DeliveryChallanNo, "
            + "  CASE WHEN ch.DeliveryChallanId IS NULL THEN CAST('' as Date) ELSE ch.DocDate END AS DeliveryChallanDate, "
            + "  CASE WHEN ch.DeliveryChallanId IS NULL THEN CAST(0 as float) ELSE ISNULL(ch.ChallanQty,0) END AS DeliveryChallanQty, "
            + "  CASE WHEN pb.DeliveryChallanId IS NULL THEN CAST(0 as int) ELSE ISNULL(pb.PurchasePreBillHeaderId,0) END AS PurchasePreBillId, "
            + "  CASE WHEN pb.DeliveryChallanId IS NULL THEN CAST(0 as int) ELSE ISNULL(pb.PurchasePreBillDetailId,0) END AS PurchasePreBillDetailId, "
            + "  CASE WHEN pb.DeliveryChallanId IS NULL THEN CAST(0 as int) ELSE ISNULL(pb.DocNo,0) END AS PurchasePreBillNo, "
            + "  CASE WHEN pb.DeliveryChallanId IS NULL THEN CAST(0 as Float) ELSE ISNULL(pb.PurchasePreBillQty,0) END AS PurchasePreBillQty, "
            + "  CAST(ISNULL(udp.PreBillQty,0) AS Float) AS UsedDemandQtyInPreBill, "
            + "  CAST(ISNULL(udg.GrnQty,0) AS Float) AS UsedDemandQtyInGrn, "
            + "  CAST(ISNULL(uop.PreBillQty,0) AS Float) AS UsedOrderQtyInPreBill, "
            + "  CAST(ISNULL(uog.GrnQty,0) AS Float) AS UsedOrderQtyInGrn, "
            + "  CAST(ISNULL(ucg.GrnQty,0) AS Float) AS UsedDeliveryChallanQtyGrn "
            + " FROM grd "
            + " LEFT JOIN TmpOrder o ON grd.PurchaseOrderId = o.PurchaseOrderId and grd.PurchaseOrderDetailId = o.PurchaseOrderDetailId "
            + " LEFT JOIN TmpDemand dm ON grd.InvPurchasedemondId = dm.InvPurchasedemondId and grd.PurchaseDemondDetailId = dm.PurchaseDemondDetailId "
            + " LEFT JOIN TmpChallan ch ON grd.DeliveryChallanId = ch.DeliveryChallanId and grd.DeliveryChallanDetailId = ch.DeliveryChallanDetailId "
            + " LEFT JOIN TmpPreBill pb ON grd.DeliveryChallanId = pb.DeliveryChallanId and grd.DeliveryChallanDetailId = pb.DeliveryChallanDetailId "
            + " LEFT JOIN TmpAlreadyUsedDemandQtyInPreBill udp ON grd.InvPurchasedemondId = udp.InvPurchasedemondId and grd.PurchaseDemondDetailId = udp.PurchaseDemondDetailId "
            + " LEFT JOIN TmpAlreadyUsedDemandQtyInGrn udg ON grd.InvPurchasedemondId = udg.InvPurchasedemondId and grd.PurchaseDemondDetailId = udg.PurchaseDemondDetailId "
            + " LEFT JOIN TmpAlreadyUsedOrderQtyInPreBill uop ON grd.PurchaseOrderId = uop.PurchaseOrderId and grd.PurchaseOrderDetailId = uop.PurchaseOrderDetailId "
            + " LEFT JOIN TmpAlreadyUsedOrderQtyInGrn uog ON grd.PurchaseOrderId = uog.PurchaseOrderId and grd.PurchaseOrderDetailId = uog.PurchaseOrderDetailId "
            + " LEFT JOIN TmpAlreadyUsedChallanQtyInGrn ucg ON grd.DeliveryChallanId = ucg.DeliveryChallanId and grd.DeliveryChallanDetailId = ucg.DeliveryChallanDetailId "
            + ") "
            + "SELECT grd.Id, grd.ItemId, grd.ItemQty, i.ItemName as Item, i.ItemCodeNew ItemCode, "
            + " grd.ItemUomId, UOM.UOMCode, UOM.Equivalent AS UOM, "
            + " grd.WarehouseId, w.WareHouseCode, grd.JobLotId, '' AS JobLot, grd.PackingTypeId, '' AS PackingType, "
            + " grd.LabReportRef, grd.AdLsWeight, '' as JobLotDescription, "
            + " 0 as GpNo, "
            + " grd.InvGrnId, grd.PurchaseOrderId, grd.PurchaseOrderDetailId, "
            + " grd.CropYear, grd.GrossWeight, grd.EBWPerUnit, grd.EBWTotal, grd.NetBillWeight, "
            + " grd.StockWeight, grd.AreaCity, grd.CommentsDetail, grd.WtCut, grd.WtCutTotal, grd.SupplySchedulId, "
            + " grd.PurchaseOrder SupplyScheduleNo, "
            + " grd.GatePassInwarDetailId, grd.InvPurchasedemondId, grd.PurchaserOrderNo, '' DeliveryTerm, "
            + " grd.ContractorId, C.CompanyName AS Contractor, grd.ScaleKart, grd.CityId, "
            + " grd.PurchaseDemondDetailId, grd.EBCutApprovedUserId, grd.EBCutDate, grd.EBCutApprovedUserRemarks, "
            + " grd.GdnId, grd.GdnDetailId, grd.ConditionId, grd.AssetId, ast.AssetName, lp.LookUpName as ItemCondition, "
            + " grd.WareHouseFromId, warto.WareHouseName as WareHouseFromCode, "
            + " grd.EbPurAgainstWeight, grd.WbTicketId, wb.TicketNo, "
            + " grd.RefDocumentTypeId, grd.RefDocNoId, grd.RefDocSubIdNo, grd.LineId, "
            + " grd.ScaleShortWeight, grd.SupplierShortWeight, grd.WeightCutOnId, grd.FreightAmount, grd.LabId, LabP.DocNo LabNo, grd.QtyForWtCut, "
            + " case when grd.WeightCutOnId = 1 then 'Qty' when grd.WeightCutOnId = 2 then '40kg' when grd.WeightCutOnId = 3 then '100kg' else 'Qty' End WeightCutOn, LabP.WeightCutUom, "
            + " grd.PackingDate, grd.ExpiryDate, "
            + " grd.CropYearId, grd.GdnDocumentTypeId, Gd.DocNo GdnNo, grd.SupplierQty, grd.StockEbUnit, grd.StockEbTotal, "
            + " grd.PurchaseDemandNo, grd.PurchaseDemandDocDate, grd.PurchaseDemandQty, "
            + " grd.UsedDemandQtyInPreBill, grd.UsedDemandQtyInGrn, "
            + " grd.UsedDemandQtyInPreBill + grd.UsedDemandQtyInGrn as TotalUsedDemandQty, "
            + " (grd.PurchaseDemandQty - (grd.UsedDemandQtyInPreBill + grd.UsedDemandQtyInGrn)) as BalDemandQty, "
            + " grd.PurchaseOrder, grd.PurchaseOrderDate, grd.PurchaseOrderQty, "
            + " grd.UsedOrderQtyInGrn, grd.UsedOrderQtyInPreBill, "
            + " grd.UsedOrderQtyInGrn + grd.UsedOrderQtyInPreBill as TotalUsedOrderQty, "
            + " (grd.PurchaseOrderQty - (grd.UsedOrderQtyInGrn + grd.UsedOrderQtyInPreBill)) as BalOrderQty, "
            + " grd.PurchasePreBillId, grd.PurchasePreBillDetailId, grd.PurchasePreBillNo, grd.PurchasePreBillQty, "
            + " grd.DeliveryChallanId, grd.DeliveryChallanDetailId, grd.DeliveryChallanNo, grd.DeliveryChallanDate, grd.DeliveryChallanQty, "
            + " grd.UsedDeliveryChallanQtyGrn, "
            + " (grd.DeliveryChallanQty - grd.UsedDeliveryChallanQtyGrn) as BalDeliveryChallanQty, "
            + " grd.ItemConditionId, VIC.ConditionStatus, "
            + " grd.RackId, R.rackName, "
            + " grd.GainLossWeightDetail "
            + "From Calc grd "
            + "inner join Item i on grd.ItemId = i.Id "
            + "inner join V_UomScheduleAndUom UOM on grd.ItemUomId = UOM.Id "
            + "inner join InvWareHouse w on grd.WarehouseId = w.Id "
            + "LEFT JOIN SupplierCustomer C ON GRD.ContractorId = C.Id "
            + "LEFT JOIN FixedAssetsRegister ast on grd.AssetId = ast.Id "
            + "LEFT JOIN LookUpsForERP lp on grd.Id = lp.Id "
            + "LEFT JOIN InvWareHouse warto ON warto.Id = grd.WareHouseFromId "
            + "LEFT JOIN WbTransactions wb on grd.WbTicketId = wb.Id "
            + "Left join InvLabAnalysisPurchaseHeader labp on labp.Id = grd.LabId "
            + "LEft Join InvGdn Gd on gd.Id = grd.GdnId "
            + "LEFT Join V_ItemCondition VIC on VIC.Id = grd.ItemConditionId "
            + "LEFT JOIN invWarehouseRack R ON grd.RackId = R.Id "
            + "WHERE grd.InvGrnId = :id "
    )
    List<InvGrnDetailStoreRow> readByInvGrnIdStore(@Param("id") Integer id);

    /** Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnIdDirect' (@Id = InvGrnId). */
    @Query(nativeQuery = true, value =
            "SELECT InvGrnDetail.Id, InvGrnDetail.ItemId, Item.ItemName AS Item, InvGrnDetail.ItemUomId, InvGrnDetail.GatePassInwarDetailId, InvGrnDetail.ItemQty, InvGrnDetail.WarehouseId, InvGrnDetail.JobLotId, "
            + " JobLot.JobLotDescription AS JobLot, InvGrnDetail.PackingTypeId, PT.PackTypeDesc AS PackingType, InvGrnDetail.LabReportRef, InvGrnDetail.AdLsWeight, Item.ItemName, UOM.Equivalent AS UOM, UOM.UOMCode, "
            + " JobLot.JobLotDescription, InvWareHouse.WareHouseCode, InvGrnDetail.InvGrnId, InvGrnDetail.PurchaseOrderId, PurchaseOrder.DocNo AS PurchaseOrder, InvGrnDetail.CropYear, InvGrnDetail.GrossWeight, "
            + " InvGrnDetail.EBWPerUnit, InvGrnDetail.EBWTotal, InvGrnDetail.NetBillWeight, InvGrnDetail.StockWeight, InvGrnDetail.AreaCity, InvGrnDetail.CommentsDetail, InvGrnDetail.WtCut, InvGrnDetail.WtCutTotal, "
            + " InvGrnDetail.SupplySchedulId, InvWareHouse.WareHouseCode AS Expr1, Gp.GpSrNo AS GpNo, sh.DocNo AS SupplyScheduleNo, PurchaseOrder.DeliveryTerm, "
            + " InvWareHouse.WareHouseName, InvGrnDetail.InvPurchasedemondId, InvGrnDetail.PurchaseOrderDetailId, PurchaseOrder.DocNo PurchaserOrderNo, "
            + " InvGrnDetail.ContractorId, C.CompanyName AS Contractor, InvGrnDetail.ScaleKart, Item.ItemCodeNew ItemCode, '' PurchaseOrderDate, InvGrnDetail.PurchaseDemondDetailId, pd.DocNo PurchaseDemandNo, "
            + " InvGrnDetail.EBCutApprovedUserId, EBCutDate, EBCutApprovedUserRemarks, InvGrnDetail.CityId, "
            + " InvGrnDetail.GdnId, InvGrnDetail.GdnDetailId, InvGrnDetail.ConditionId, InvGrnDetail.AssetId, "
            + " ast.AssetName, lp.LookUpName as ItemCondition, InvGrnDetail.WareHouseFromId, warto.WareHouseName as WareHouseFromCode, "
            + " InvGrnDetail.EbPurAgainstWeight, InvGrnDetail.WbTicketId, wb.TicketNo, "
            + " InvGrnDetail.RefDocumentTypeId, InvGrnDetail.RefDocNoId, InvGrnDetail.RefDocSubIdNo, InvGrnDetail.LineId, "
            + " InvGrnDetail.ScaleShortWeight, InvGrnDetail.SupplierShortWeight, InvGrnDetail.WeightCutOnId, "
            + " InvGrnDetail.FreightAmount, InvGrnDetail.LabId, LabP.DocNo LabNo, InvGrnDetail.QtyForWtCut, "
            + " case when InvGrnDetail.WeightCutOnId = 1 then 'Qty' when InvGrnDetail.WeightCutOnId = 2 then '40kg' when InvGrnDetail.WeightCutOnId = 3 then '100kg' else 'Qty' End WeightCutOn, LabP.WeightCutUom, "
            + " [InvGrnDetail].PackingDate, [InvGrnDetail].ExpiryDate, "
            + " [InvGrnDetail].CropYearId, InvGrnDetail.GdnDocumentTypeId, Gd.DocNo GdnNo, InvGrnDetail.SupplierQty, InvGrnDetail.StockEbUnit, InvGrnDetail.StockEbTotal, "
            + " [InvGrnDetail].ItemConditionId, VIC.ConditionStatus, "
            + " InvGrnDetail.RackId, r.rackName, "
            + " InvGrnDetail.GainLossWeightDetail, "
            + " CAST('' as date) as PurchaseDemandDocDate, "
            + " CAST(0 as Float) as PurchaseDemandQty, "
            + " CAST(0 as Float) as UsedDemandQtyInPreBill, "
            + " CAST(0 as Float) as UsedDemandQtyInGrn, "
            + " CAST(0 as Float) as TotalUsedDemandQty, "
            + " CAST(0 as Float) as BalDemandQty, "
            + " CAST(0 as Float) as PurchaseOrderQty, "
            + " CAST(0 as Float) as UsedOrderQtyInGrn, "
            + " CAST(0 as Float) as UsedOrderQtyInPreBill, "
            + " CAST(0 as Float) as TotalUsedOrderQty, "
            + " CAST(0 as Float) as BalOrderQty, "
            + " CAST(0 as int) as PurchasePreBillId, "
            + " CAST(0 as int) as PurchasePreBillDetailId, "
            + " CAST(0 as int) as PurchasePreBillNo, "
            + " CAST(0 as Float) as PurchasePreBillQty, "
            + " InvGrnDetail.DeliveryChallanId, "
            + " InvGrnDetail.DeliveryChallanDetailId, "
            + " CAST(0 as int) as DeliveryChallanNo, "
            + " CAST('' as date) as DeliveryChallanDate, "
            + " CAST(0 as Float) as DeliveryChallanQty, "
            + " CAST(0 as Float) as UsedDeliveryChallanQtyGrn, "
            + " CAST(0 as Float) as BalDeliveryChallanQty "
            + "FROM InvGrnDetail "
            + " INNER JOIN InvGrn AS g ON InvGrnDetail.InvGrnId = g.Id "
            + " INNER JOIN Item ON Item.Id = InvGrnDetail.ItemId "
            + " INNER JOIN V_UomScheduleAndUom AS UOM ON InvGrnDetail.ItemUomId = UOM.Id "
            + " LEFT OUTER JOIN InvWareHouse ON InvGrnDetail.WarehouseId = InvWareHouse.Id "
            + " LEFT OUTER JOIN JobLot ON InvGrnDetail.JobLotId = JobLot.Id "
            + " LEFT OUTER JOIN InvPackingType AS PT ON InvGrnDetail.PackingTypeId = PT.Id "
            + " LEFT OUTER JOIN PurchaseOrder ON InvGrnDetail.PurchaseOrderId = PurchaseOrder.Id "
            + " LEFT OUTER JOIN SupplyScheduleDetail AS shd ON InvGrnDetail.SupplySchedulId = shd.Id "
            + " LEFT OUTER JOIN SupplySchedule AS sh ON shd.SupplyScheduleId = sh.Id "
            + " LEFT OUTER JOIN GatePassInwardDetail AS Gpd ON InvGrnDetail.GatePassInwarDetailId = Gpd.Id "
            + " LEFT OUTER JOIN GatePassInward AS Gp ON g.InwardGatePassId = Gp.Id "
            + " LEFT JOIN SupplierCustomer C ON InvGrnDetail.ContractorId = C.Id "
            + " LEFT JOIN FixedAssetsRegister ast on InvGrnDetail.AssetId = ast.Id "
            + " LEFT JOIN LookUpsForERP lp on InvGrnDetail.Id = lp.Id "
            + " Left join InvPurchaseDemandHeader pd on pd.Id = InvGrnDetail.InvPurchasedemondId "
            + " LEFT JOIN InvWareHouse warto ON warto.Id = InvGrnDetail.WareHouseFromId "
            + " LEFT JOIN WbTransactions wb on InvGrnDetail.WbTicketId = wb.Id "
            + " Left join InvLabAnalysisPurchaseHeader labp on labp.Id = InvGrnDetail.LabId "
            + " LEFT Join V_ItemCondition VIC on VIC.Id = InvGrnDetail.ItemConditionId "
            + " LEft Join InvGdn Gd on gd.Id = InvGrnDetail.GdnId "
            + " LEFT JOIN invWarehouseRack R ON InvGrnDetail.RackId = R.Id "
            + "WHERE [dbo].[InvGrnDetail].[InvGrnId] = :id "
    )
    List<InvGrnDetailRow> readByInvGrnIdDirect(@Param("id") Integer id);

    /**
     * Sp_InvGrnDetail_Insert — full procedure (validations, PO auto-complete, gate pass / weigh-bridge side effects,
     * max(Id)+1 and INSERT). Returns the new Id (SELECT @Id). All parameters default to NULL in the proc.
     * KEEP-EXEC: procedural validation against ~10 tables and 7 scalar functions, cannot be expressed as one statement.
     * Not @Modifying on purpose: the proc ends with SELECT @Id (SET NOCOUNT ON), so it is executed as a result-set
     * query inside a transaction and the new Id is returned.
     */
    @Transactional
    @Query(nativeQuery = true, value = "EXEC dbo.Sp_InvGrnDetail_Insert @Id = :id, @InvGrnId = :invGrnId, @PurchaseOrderId = :purchaseOrderId, "
            + "@ItemId = :itemId, @PackingTypeId = :packingTypeId, @CropYear = :cropYear, @JobLotId = :jobLotId, @ItemQty = :itemQty, "
            + "@ItemUomId = :itemUomId, @GrossWeight = :grossWeight, @EBWPerUnit = :ebwPerUnit, @EBWTotal = :ebwTotal, @WtCut = :wtCut, "
            + "@WtCutTotal = :wtCutTotal, @AdLsWeight = :adLsWeight, @NetBillWeight = :netBillWeight, @StockWeight = :stockWeight, "
            + "@WarehouseId = :warehouseId, @LabReportRef = :labReportRef, @AreaCity = :areaCity, @CommentsDetail = :commentsDetail, "
            + "@SupplySchedulId = :supplySchedulId, @GatePassInwarDetailId = :gatePassInwarDetailId, @InvPurchasedemondId = :invPurchasedemondId, "
            + "@PurchaseOrderDetailId = :purchaseOrderDetailId, @PurchaserOrderNo = :purchaserOrderNo, @ContractorId = :contractorId, "
            + "@PurchaseDemondDetailId = :purchaseDemondDetailId, @ScaleKart = :scaleKart, @EBCutApprovedUserId = :ebCutApprovedUserId, "
            + "@EBCutDate = :ebCutDate, @EBCutApprovedUserRemarks = :ebCutApprovedUserRemarks, @CityId = :cityId, @GdnId = :gdnId, "
            + "@GdnDetailId = :gdnDetailId, @AssetId = :assetId, @ConditionId = :conditionId, @WareHouseFromId = :wareHouseFromId, "
            + "@EbPurAgainstWeight = :ebPurAgainstWeight, @WbTicketId = :wbTicketId, @RefDocumentTypeId = :refDocumentTypeId, "
            + "@RefDocNoId = :refDocNoId, @RefDocSubIdNo = :refDocSubIdNo, @LineId = :lineId, @ScaleShortWeight = :scaleShortWeight, "
            + "@SupplierShortWeight = :supplierShortWeight, @WeightCutOnId = :weightCutOnId, @FreightAmount = :freightAmount, @LabId = :labId, "
            + "@QtyForWtCut = :qtyForWtCut, @PackingDate = :packingDate, @ExpiryDate = :expiryDate, @CropYearId = :cropYearId, "
            + "@GdnDocumentTypeId = :gdnDocumentTypeId, @SupplierQty = :supplierQty, @StockEbUnit = :stockEbUnit, @StockEbTotal = :stockEbTotal, "
            + "@DeliveryChallanId = :deliveryChallanId, @DeliveryChallanDetailId = :deliveryChallanDetailId, @ItemConditionId = :itemConditionId, "
            + "@RackId = :rackId, @GainLossWeightDetail = :gainLossWeightDetail")
    Integer spInvGrnDetailInsert(@Param("id") Integer id, @Param("invGrnId") Integer invGrnId, @Param("purchaseOrderId") Integer purchaseOrderId,
            @Param("itemId") Integer itemId, @Param("packingTypeId") Integer packingTypeId, @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId, @Param("itemQty") Double itemQty, @Param("itemUomId") Integer itemUomId,
            @Param("grossWeight") Double grossWeight, @Param("ebwPerUnit") Double ebwPerUnit, @Param("ebwTotal") Double ebwTotal,
            @Param("wtCut") Double wtCut, @Param("wtCutTotal") Double wtCutTotal, @Param("adLsWeight") Double adLsWeight,
            @Param("netBillWeight") Double netBillWeight, @Param("stockWeight") Double stockWeight, @Param("warehouseId") Integer warehouseId,
            @Param("labReportRef") String labReportRef, @Param("areaCity") String areaCity, @Param("commentsDetail") String commentsDetail,
            @Param("supplySchedulId") Integer supplySchedulId, @Param("gatePassInwarDetailId") Integer gatePassInwarDetailId,
            @Param("invPurchasedemondId") Integer invPurchasedemondId, @Param("purchaseOrderDetailId") Integer purchaseOrderDetailId,
            @Param("purchaserOrderNo") Integer purchaserOrderNo, @Param("contractorId") Integer contractorId,
            @Param("purchaseDemondDetailId") Integer purchaseDemondDetailId, @Param("scaleKart") Double scaleKart,
            @Param("ebCutApprovedUserId") Integer ebCutApprovedUserId, @Param("ebCutDate") LocalDateTime ebCutDate,
            @Param("ebCutApprovedUserRemarks") String ebCutApprovedUserRemarks, @Param("cityId") Integer cityId, @Param("gdnId") Integer gdnId,
            @Param("gdnDetailId") Integer gdnDetailId, @Param("assetId") Integer assetId, @Param("conditionId") Integer conditionId,
            @Param("wareHouseFromId") Integer wareHouseFromId, @Param("ebPurAgainstWeight") Double ebPurAgainstWeight,
            @Param("wbTicketId") Integer wbTicketId, @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("refDocNoId") Integer refDocNoId,
            @Param("refDocSubIdNo") Integer refDocSubIdNo, @Param("lineId") Integer lineId, @Param("scaleShortWeight") Double scaleShortWeight,
            @Param("supplierShortWeight") Double supplierShortWeight, @Param("weightCutOnId") Integer weightCutOnId,
            @Param("freightAmount") Double freightAmount, @Param("labId") Integer labId, @Param("qtyForWtCut") Double qtyForWtCut,
            @Param("packingDate") LocalDateTime packingDate, @Param("expiryDate") LocalDateTime expiryDate, @Param("cropYearId") Integer cropYearId,
            @Param("gdnDocumentTypeId") Integer gdnDocumentTypeId, @Param("supplierQty") Double supplierQty, @Param("stockEbUnit") Double stockEbUnit,
            @Param("stockEbTotal") Double stockEbTotal, @Param("deliveryChallanId") Integer deliveryChallanId,
            @Param("deliveryChallanDetailId") Integer deliveryChallanDetailId, @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId, @Param("gainLossWeightDetail") BigDecimal gainLossWeightDetail);

    /** Sp_InvGrnDetail_Insert standalone step: UPDATE PurchaseOrder SET OrderStatus = 'Complete' (order fully received). */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrder p SET p.orderStatus = 'Complete' WHERE p.organizationId = :organizationId AND p.companyId = :companyId AND p.id = :purchaseOrderId")
    int markPurchaseOrderComplete(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("purchaseOrderId") Integer purchaseOrderId);

    /** Sp_InvGrnDetail_Insert standalone step (market / gate purchase): UPDATE GatePassInward SET VarietyName, ItemId. */
    @Modifying
    @Transactional
    @Query("UPDATE InwardGatePass g SET g.varietyName = :itemName, g.itemId = :itemId WHERE g.id = :gatePassId")
    int updateGatePassVariety(@Param("itemName") String itemName, @Param("itemId") Integer itemId, @Param("gatePassId") Integer gatePassId);

    /** Sp_InvGrnDetail_Insert standalone step: UPDATE WbTransactions SET ItemId, ItemDescription for the gate pass (no entity → native). */
    @Modifying
    @Transactional
    @Query(nativeQuery = true, value = "UPDATE WbTransactions SET ItemId = :itemId, ItemDescription = :itemName WHERE ReferenceDocTypeId = 51 AND ReferenceDocNoId = :gatePassId")
    int updateWbTransactionsItem(@Param("itemId") Integer itemId, @Param("itemName") String itemName, @Param("gatePassId") Integer gatePassId);

    /** Sp_InvGrn_Update standalone step: delete InvGrnDetail where InvGrnId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvGrnDetail d WHERE d.invGrnId = :invGrnId")
    int deleteByInvGrnId(@Param("invGrnId") Integer invGrnId);

    /** Sp_InvGrnDetail_Insert helper: COUNT(*) of detail rows of the GRN (row number used in the empty-bags message). */
    @Query("select count(d) from InvGrnDetail d where d.invGrnId = :invGrnId")
    Long countByInvGrnId(@Param("invGrnId") Integer invGrnId);
}
