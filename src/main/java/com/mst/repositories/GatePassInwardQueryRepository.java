package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InwardGatePass;
import com.mst.projections.GatePassForStockTransferRow;
import com.mst.projections.GatePassGeneralForWeighBridgeRow;
import com.mst.projections.GatePassInwardAccessWeightRow;
import com.mst.projections.GatePassInwardDetailWithSupplyRow;
import com.mst.projections.GatePassInwardForGrnRow;
import com.mst.projections.GatePassInwardForGrnWithLabRow;
import com.mst.projections.GatePassInwardForPurchaseAnalysisRow;
import com.mst.projections.GatePassInwardFreightVoucherRow;
import com.mst.projections.GatePassInwardHeaderDetailRow;
import com.mst.projections.GatePassInwardHistoryGhallaMandiRow;
import com.mst.projections.GatePassInwardHistoryRefDocTypeRow;
import com.mst.projections.GatePassInwardHistoryRow;
import com.mst.projections.GatePassInwardIdSrNoRow;
import com.mst.projections.GatePassInwardImportGrnRow;
import com.mst.projections.GatePassInwardImportShipmentRow;
import com.mst.projections.GatePassInwardLabDataGrnGateRow;
import com.mst.projections.GatePassInwardLabDataRow;
import com.mst.projections.GatePassInwardOpenByDateRow;
import com.mst.projections.GatePassInwardOpenForGhallaMandiRow;
import com.mst.projections.GatePassInwardOpenForGpDetailRow;
import com.mst.projections.GatePassInwardOrderTypeRow;
import com.mst.projections.GatePassInwardPendingForGrnRow;
import com.mst.projections.GatePassInwardPendingGatePurchaseRow;
import com.mst.projections.GatePassInwardPendingGhallaMandiRow;
import com.mst.projections.GatePassInwardPoSoPendingRow;
import com.mst.projections.GatePassInwardPurchaseBreakUpRow;
import com.mst.projections.GatePassInwardPurchaseOrderNoRow;
import com.mst.projections.GatePassInwardReadByGpNoRow;
import com.mst.projections.GatePassInwardReadByIdRow;
import com.mst.projections.GatePassInwardSoPendingRow;
import com.mst.projections.GatePassInwardTotalReceivedQtyRow;
import com.mst.projections.GatePassOutwardForWeighBridgeRow;
import com.mst.projections.GatePassOutwardManualWbRow;

/**
 * Query-only repository for Sp_GatePassInward_GetAllMethod (one method per @Activity branch, 38 branches).
 * Proc parameter defaults: every parameter is NULL except @WeightComparedToPoWt float = 0.
 * Bound to InwardGatePass (owned entity).
 */
@Repository
public interface GatePassInwardQueryRepository extends org.springframework.data.repository.Repository<InwardGatePass, Integer> {

    /** Sp_GatePassInward_GetAllMethod @Activity='ReadById' (temp table #ReadbyId folded into one SELECT; RefferedInWbOrLab is always 0). */
    @Query(value = "SELECT gp.[Id], gp.[DocumentTypeId], gp.[GpSrNo], gp.[GpDate], gp.[GatepassType], gp.[GpTypeSrNo], gp.[SupplierCustomerId], "
         + "gp.[OtherSupCust], gp.[VehicleType], gp.[VehicleNo], gp.[BiltyNo], gp.[NoOfPackages], gp.[OtherRemarks], gp.[RefDocumentTypeId], "
         + "gp.[RefDocumentEntryNo], gp.[InDateTimeStamp], gp.[OutDateTimeStamp], gp.[DocAttachment], gp.[IsApproved], gp.[EntryDate], "
         + "gp.[EntryUser], gp.[ModifyDate], gp.[ModifyUser], gp.[OrganizationId], gp.[CompanyId], gp.[PostUser], gp.[PostState], gp.[PostDate], "
         + "gp.[CityId], gp.[Freight], gp.AdvanceByFactory, gp.AdvanceByParty, gp.[SupplierContractCode], gp.[Status], "
         + "case when fr.Id > 0 then fr.TotalPaidAmount else gp.[NetPaid] end as NetPaid, gp.SupplierFirstWeight, gp.SupplierSecondWeight, "
         + "gp.[SupplierWeight], gp.[FactoryWeight], gp.[DifferenceWeight], gp.[WeighBridgeId], gp.[WarehouseId], gp.ItemId, gp.[VarietyName], "
         + "gp.[PurchaseOrderId], gp.[Container], gp.[Container1], gp.[PackUnit], gp.[WeightComparedToPoWt], gp.[FinancialYearId], gp.[GpQRCode], "
         + "gp.[DestrictId], gp.[TehsilId], gp.[SupplierCNICNO], gp.[SupplierMobileNo], gp.[TransporterName], gp.WeightDiffComments, "
         + "gp.SystemErrorMessage, gp.FreightOn, gp.GhallaMandiId, gp.LoadingContractorId, gp.CarriageContractorId, gp.WorkingReportNo, "
         + "gp.BiltyDate, gp.BranchesId, gp.RateCut, gp.RateCutRemarks, gp.AccessWeight, gp.SupplierDispatchId, gp.PackingTypeId, "
         + "gp.ActionIdForSpecialApproval, gp.SpecialApprovalUserId, gp.SpecialApprovalDate, City.Description AS CityName, "
         + "InvWareHouse.WareHouseName, SupplierCustomer.CompanyName SupplierName, fr.Id as freightVoucherId, gp.driverBioDataId, "
         + "'' AttachmentsList, '' DeleteAttachmentsList, '' gatePassInwardDetails, '' GatePassInwardPurchaseBreakUpList, "
         + "driver.DriverName, driver.CnicNo As DriverCNICNO, driver.cellNo As DriverMobileNo, driver.whatsappNo, driver.AlternateCellNo, "
         + "driver.FatherName, driver.fatherCnicNo, CAST(0 as bit) as RefferedInWbOrLab "
         + "FROM GatePassInward gp with(nolock) "
         + "LEFT JOIN InvWareHouse with(nolock) ON gp.WarehouseId = InvWareHouse.Id "
         + "LEFT JOIN SupplierCustomer with(nolock) ON gp.SupplierCustomerId = SupplierCustomer.Id "
         + "LEFT JOIN City with(nolock) ON gp.CityId = City.Id "
         + "LEFT JOIN InvFreightVoucher fr on gp.Id = fr.GatePassId "
         + "Left Join driverBiodata driver on driver.driverBiodataId = gp.driverBiodataId "
         + "WHERE gp.Id = :id", nativeQuery = true)
    List<GatePassInwardReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='ReadByHeaderId' (kept verbatim incl. the proc's "po on p.Id = gpd.PurchaseOrderId" join). */
    @Query(value = "select i.ItemName, p.PackTypecode, gpd.PackUnit, gpd.ItemQty, Gpd.Weight, uom.Equivalent, uom.UOMCode, "
         + "0 as DocNo, C.Description CityName, po.DocNo OrderNo, gpd.RemarksDetail, gpd.RefDocumentTypeId "
         + "from GatePassInwardDetail gpd "
         + "Left join InvPackingType p on p.id = gpd.packingTypeid "
         + "Left join V_UomScheduleAndUom uom on uom.Id = gpd.ItemUOMId "
         + "Inner join item i on i.id = gpd.itemid "
         + "Left join PurchaseOrder po on p.Id = gpd.PurchaseOrderId "
         + "Left join City C on C.id = gpd.CityId "
         + "where gpd.gatepassinwardid = :id", nativeQuery = true)
    List<GatePassInwardHeaderDetailRow> readByHeaderId(@Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='ReadByHeaderId_PurchaseBrekup' */
    @Query("SELECT gg.id AS id, gg.inwardGatePassId AS inwardGatePassId, gg.qty AS qty, gg.uom AS uom, gg.grossWeight AS grossWeight, "
         + "gg.ebWeight AS ebWeight, gg.ebTotal AS ebTotal, gg.netWeight AS netWeight "
         + "FROM InwardGatePassPurchaseBreakUp gg WHERE gg.inwardGatePassId = :id")
    List<GatePassInwardPurchaseBreakUpRow> readByHeaderIdPurchaseBrekup(@Param("id") Integer id);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='ReadByGpNo'.
     * // KEEP-EXEC: raises 'Approved Required Because Of Access Weight!' (config 5070, DocumentTypeId 51, RefDocumentTypeId 41) before selecting.
     */
    @Query(value = "EXEC dbo.Sp_GatePassInward_GetAllMethod @Activity = 'ReadByGpNo', @Id = :id, @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @BranchesId = :branchesId, @DocumentTypeId = :documentTypeId, @FinancialYearId = :financialYearId",
         nativeQuery = true)
    List<GatePassInwardReadByGpNoRow> readByGpNo(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId);

    /** Sp_GatePassInward_GetAllMethod @Activity='PurchaseOrderNoByGpId' (#tmp folded into NOT EXISTS; Organization/Company taken from the GP row as in the proc). */
    @Query(value = "SELECT Distinct d.PurchaseOrderId, po.DocNo PurchaseOrderNo "
         + "FROM GatePassInward h "
         + "INNER JOIN GatePassInwardDetail d on h.Id = d.GatePassInwardId "
         + "INNER JOIN PurchaseOrder po on po.Id = d.PurchaseOrderId "
         + "Where d.PurchaseOrderId > 0 and h.Id = :id "
         + "AND NOT EXISTS (Select 1 from InvGrn g Inner join InvGrnDetail gd on gd.InvGrnId = g.Id "
         + "WHERE g.OrganizationId = h.OrganizationId AND g.CompanyId = h.CompanyId AND (:branchesId is null or g.BranchesId = :branchesId) "
         + "AND g.InwardGatePassId = h.Id AND (:recId IS NULL OR g.Id <> :recId) AND gd.PurchaseOrderId = d.PurchaseOrderId)", nativeQuery = true)
    List<GatePassInwardPurchaseOrderNoRow> purchaseOrderNoByGpId(@Param("id") Integer id, @Param("branchesId") Integer branchesId,
            @Param("recId") Integer recId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GenerategpCode' → GpSrNo */
    @Query("select coalesce(max(gp.gpSrNo),0) + 1 from InwardGatePass gp where gp.companyId = :companyId and gp.organizationId = :organizationId "
         + "and gp.documentTypeId = :documentTypeId and gp.financialYearId = :financialYearId and (:branchesId is null or gp.branchesId = :branchesId)")
    Integer generateGpCode(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GenerateGPTypeCode' → GpTypeSrNo */
    @Query("select coalesce(max(gp.gpTypeSrNo),0) + 1 from InwardGatePass gp where gp.companyId = :companyId and gp.organizationId = :organizationId "
         + "and gp.gatepassType = :gatepassType and gp.financialYearId = :financialYearId and (:branchesId is null or gp.branchesId = :branchesId) "
         + "and (:documentTypeId is null or gp.documentTypeId = :documentTypeId)")
    Integer generateGpTypeCode(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
            @Param("gatepassType") String gatepassType, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId, @Param("documentTypeId") Integer documentTypeId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GenerateCodeByRefDocumentTypeId' → GpSrNo (next RefDocumentEntryNo) */
    @Query("select coalesce(max(gp.refDocumentEntryNo),0) + 1 from InwardGatePass gp where gp.companyId = :companyId and gp.organizationId = :organizationId "
         + "and gp.documentTypeId = :documentTypeId and gp.financialYearId = :financialYearId and gp.refDocumentTypeId = :refDocumentTypeId "
         + "and (:branchesId is null or gp.branchesId = :branchesId)")
    Integer generateCodeByRefDocumentTypeId(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='TotalReceivedQtyByPurchaseOrderId' (@NoOfPackages variable folded into a CROSS JOIN sub-select). */
    @Query(value = "SELECT distinct pod.PurchaseOrderId, pod.OrderItemId, isnull(COUNT(gp.VehicleNo),0) AS VehicleNo, "
         + "ISNULL(pod.OrderItemQty, 0) AS OrderItemQty, np.NoOfPackages as NoOfPackages, "
         + "ISNULL(pod.OrderItemQty, 0) - isnull(np.NoOfPackages,0) AS BalQty, MAX(gp.GpDate) AS GpDate "
         + "FROM dbo.PurchaseOrderDetail AS pod INNER JOIN dbo.PurchaseOrder AS po ON pod.PurchaseOrderId = po.Id "
         + "LEFT OUTER JOIN dbo.GatePassInward AS gp ON po.Id = gp.PurchaseOrderId "
         + "CROSS JOIN (select isnull(sum(g2.NoOfPackages),0) as NoOfPackages from GatePassInward g2 where g2.OrganizationId = :organizationId "
         + "AND g2.CompanyId = :companyId AND g2.PurchaseOrderId = :purchaseOrderId and g2.Status != 'Rejected') np "
         + "WHERE po.OrganizationId = :organizationId AND po.CompanyId = :companyId AND po.Id = :purchaseOrderId "
         + "GROUP BY pod.PurchaseOrderId, pod.OrderItemId, pod.OrderItemQty, np.NoOfPackages "
         + "ORDER BY pod.PurchaseOrderId", nativeQuery = true)
    List<GatePassInwardTotalReceivedQtyRow> totalReceivedQtyByPurchaseOrderId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("purchaseOrderId") Integer purchaseOrderId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GatepassHistory' (#History folded into a CTE). */
    @Query(value = "WITH History AS (SELECT ROW_NUMBER() Over (Order By Gp.GpDate Desc) RecordNo, GP.Id, GP.GpSrNo, GP.GpDate, "
         + "Case when GP.RefDocumentTypeId = 41 then 'Purchase Order' when GP.RefDocumentTypeId = 104 then 'Supply Order' "
         + "when GP.RefDocumentTypeId = 105 then 'Market Purchase' when GP.RefDocumentTypeId = 232 then 'Import PO' "
         + "when GP.RefDocumentTypeId = 106 then 'Gate Purchase' when GP.RefDocumentTypeId = 1500 then 'Purchase_Order' "
         + "when GP.RefDocumentTypeId = 700 then 'Purchase_Order_Pm' WHEN GP.RefDocumentTypeId IN (98) THEN 'Sale Return' "
         + "WHEN GP.RefDocumentTypeId IN (52) THEN 'General' when GP.RefDocumentTypeId = 204 then 'Export Return' "
         + "when GP.RefDocumentTypeId = 241 then 'Export Dispatch Return' when GP.RefDocumentTypeId = 175 then 'Stock Received' "
         + "else 'Other' END OrderType, Sp.CompanyName, GP.VehicleType, GP.VehicleNo, gp.BiltyDate, GP.BiltyNo, GP.NoOfPackages as ItemQty, "
         + "GP.OtherRemarks, GP.InDateTimeStamp as InTime, GP.OutDateTimeStamp as OutTime, EUser.FirstName + ' ' + EUser.LastName as UserName, "
         + "GP.EntryDate, MUser.FirstName + ' ' + MUser.LastName as ModifyUserName, GP.ModifyDate, Cty.Description, GP.Freight, "
         + "GP.AdvanceByParty, GP.AdvanceByFactory, GP.Freight - ISNULL(GP.AdvanceByFactory,0) - ISNULL(GP.AdvanceByParty,0) AS TotalPayableFreight, "
         + "GP.NetPaid, gp.RateCut, gp.RateCutRemarks, gp.AccessWeight, "
         + "Case when ISNULL(gp.IsApproved,0) = 1 then 'Approved' else 'Not Approved' end as ApprovalStatus, "
         + "CASE WHEN GP.RefDocumentTypeId in (41,1500,700) THEN GP.SupplierContractCode ELSE 0 END as OrderNo, "
         + "GP.GatepassType, GP.Status, GP.SupplierFirstWeight, GP.SupplierSecondWeight, GP.SupplierWeight, GP.FactoryWeight, GP.DifferenceWeight, "
         + "GP.VarietyName, gp.WeightComparedToPoWt, gp.PackUnit, "
         + "(select isnull(count(Attachment),0) from DMSAttachments dms where dms.RefDocumentTypeId = :documentTypeId and dms.RefDocumentNo = gp.Id) as NoOfAttachments, "
         + "GP.PurchaseOrderId, gp.PackingTypeId, PP.PackTypeDesc PackingType, Gp.driverBioDataId, driver.DriverName, driver.CnicNo AS DriverCNICNO, "
         + "driver.cellNo As DriverMobileNo, driver.whatsappNo, driver.AlternateCellNo, driver.FatherName, driver.fatherCnicNo "
         + "FROM dbo.GatePassInward AS GP "
         + "Left JOIN dbo.UserAccount AS MUser ON GP.ModifyUser = MUser.ID "
         + "Left JOIN dbo.UserAccount AS EUser ON GP.EntryUser = EUser.ID "
         + "LEFT JOIN dbo.InvWareHouse AS Wh ON GP.WarehouseId = Wh.Id "
         + "LEFT JOIN dbo.City AS Cty ON GP.CityId = Cty.Id "
         + "left join SupplierCustomer sp on gp.SupplierCustomerId = sp.Id "
         + "Left Join InvPackingType PP on PP.Id = gp.PackingTypeId "
         + "Left Join driverBiodata driver on driver.driverBiodataId = Gp.driverBioDataId "
         + "WHERE (GP.OrganizationId = :organizationId) AND (GP.CompanyId = :companyId) and (gp.DocumentTypeId = :documentTypeId) "
         + "AND (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and GP.EntryUser = :entryUser)) "
         + "and (:financialYearId is null or Gp.FinancialYearId = :financialYearId) "
         + "and (:dateFrom is null or CAST(gP.GpDate AS DATE) >= CAST(:dateFrom AS DATE)) "
         + "and (:dateTo is null or CAST(GP.GpDate AS DATE) <= CAST(:dateTo AS DATE)) "
         + "and (:entryFromDate is null or CAST(GP.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + "and (:entryToDate is null or CAST(GP.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + "and (:modifyFromDate is null or CAST(GP.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + "and (:modifyToDate is null or CAST(GP.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + "and (:docNoFrom is null or GP.GpSrNo >= :docNoFrom) and (:docNoTo is null or GP.GpSrNo <= :docNoTo) "
         + "and (:supplierCustomerId is null or Gp.SupplierCustomerId = :supplierCustomerId)) "
         + "SELECT * FROM History Where (:noOfRecords is null or RecordNo <= :noOfRecords) order by GpSrNo desc", nativeQuery = true)
    List<GatePassInwardHistoryRow> gatepassHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId,
            @Param("canViewAllRecord") Boolean canViewAllRecord, @Param("entryUser") Integer entryUser,
            @Param("financialYearId") Integer financialYearId, @Param("dateFrom") LocalDateTime dateFrom, @Param("dateTo") LocalDateTime dateTo,
            @Param("entryFromDate") LocalDate entryFromDate, @Param("entryToDate") LocalDate entryToDate,
            @Param("modifyFromDate") LocalDate modifyFromDate, @Param("modifyToDate") LocalDate modifyToDate,
            @Param("docNoFrom") Integer docNoFrom, @Param("docNoTo") Integer docNoTo,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("noOfRecords") Integer noOfRecords);

    /** Sp_GatePassInward_GetAllMethod @Activity='GatepassHistoryForRefdocumentTypeWise' (#HistoryRef folded into a CTE). */
    @Query(value = "WITH HistoryRef AS (SELECT ROW_NUMBER() Over (Order By Gp.GpDate Desc) RecordNo, GP.Id, GP.GpDate, GP.GpSrNo, "
         + "Sp.CompanyName, GP.VehicleType, GP.VehicleNo, GP.BiltyNo, GP.NoOfPackages as ItemQty, GP.OtherRemarks, GP.InDateTimeStamp as InTime, "
         + "GP.OutDateTimeStamp as OutTime, EUser.FirstName + ' ' + EUser.LastName as UserName, Cty.Description, GP.Freight, "
         + "Gp.SupplierFirstWeight, Gp.SupplierSecondWeight, GP.SupplierWeight, GP.VarietyName, "
         + "(select isnull(count(Attachment),0) from DMSAttachments dms where dms.RefDocumentTypeId = :documentTypeId and dms.RefDocumentNo = gp.Id) as NoOfAttachments "
         + "FROM dbo.GatePassInward AS GP "
         + "INNER JOIN dbo.UserAccount AS EUser ON GP.EntryUser = EUser.ID "
         + "LEFT JOIN dbo.InvWareHouse AS Wh ON GP.WarehouseId = Wh.Id "
         + "LEFT JOIN dbo.City AS Cty ON GP.CityId = Cty.Id "
         + "LEFT JOIN SupplierCustomer sp on gp.SupplierCustomerId = sp.Id "
         + "WHERE (GP.OrganizationId = :organizationId) AND (GP.CompanyId = :companyId) and (gp.DocumentTypeId = :documentTypeId) "
         + "AND (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and GP.EntryUser = :entryUser)) "
         + "and (:financialYearId is null or Gp.FinancialYearId = :financialYearId) "
         + "and (:refDocumentTypeId is null or Gp.RefDocumentTypeId = :refDocumentTypeId)) "
         + "SELECT * FROM HistoryRef Where (:noOfRecords is null or RecordNo <= :noOfRecords) order by GpSrNo desc", nativeQuery = true)
    List<GatePassInwardHistoryRefDocTypeRow> gatepassHistoryForRefdocumentTypeWise(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId,
            @Param("canViewAllRecord") Boolean canViewAllRecord, @Param("entryUser") Integer entryUser,
            @Param("financialYearId") Integer financialYearId, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("noOfRecords") Integer noOfRecords);

    /** Sp_GatePassInward_GetAllMethod @Activity='GatepassHistoryForGhallaMandi' (#TmpHisGM folded into a CTE). */
    @Query(value = "WITH TmpHisGM AS (SELECT ROW_NUMBER() Over (Order By Gp.GpDate Desc) RecordNo, GP.Id, GP.GpDate, GP.GpSrNo, gp.WorkingReportNo, "
         + "GM.GhallaMandi, Sp.CompanyName as LoadingContractor, GP.VehicleType, GP.VehicleNo, GP.BiltyNo, GP.NoOfPackages as ItemQty, GP.OtherRemarks, "
         + "GP.InDateTimeStamp as InTime, GP.OutDateTimeStamp as OutTime, EUser.FirstName + ' ' + EUser.LastName as UserName, "
         + "cc.CompanyName as CarriageContractro, GP.Freight, "
         + "(select isnull(count(Attachment),0) from DMSAttachments dms where dms.RefDocumentTypeId = :documentTypeId and dms.RefDocumentNo = gp.Id) as NoOfAttachments "
         + "FROM dbo.GatePassInward AS GP "
         + "INNER JOIN dbo.UserAccount AS EUser ON GP.EntryUser = EUser.ID "
         + "INNER JOIN Ghalla_Mandi AS GM ON GP.GhallaMandiId = GM.Id "
         + "INNER JOIN SupplierCustomer sp on gp.LoadingContractorId = sp.Id "
         + "LEFT JOIN SupplierCustomer AS cc ON GP.CarriageContractorId = cc.Id "
         + "WHERE (GP.OrganizationId = :organizationId) AND (GP.CompanyId = :companyId) and (gp.DocumentTypeId = :documentTypeId) "
         + "AND (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and GP.EntryUser = :entryUser)) "
         + "and (:financialYearId is null or Gp.FinancialYearId = :financialYearId) "
         + "and (:refDocumentTypeId is null or Gp.RefDocumentTypeId = :refDocumentTypeId)) "
         + "SELECT * FROM TmpHisGM Where (:noOfRecords is null or RecordNo <= :noOfRecords) order by GpSrNo desc", nativeQuery = true)
    List<GatePassInwardHistoryGhallaMandiRow> gatepassHistoryForGhallaMandi(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId,
            @Param("canViewAllRecord") Boolean canViewAllRecord, @Param("entryUser") Integer entryUser,
            @Param("financialYearId") Integer financialYearId, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("noOfRecords") Integer noOfRecords);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='ReadByGpNoForGRN' — four sub-branches chosen by the GP's own RefDocumentTypeId
     * (41 Purchase Order / 1500 Steel PO (ST schema) / 105 Market Purchase / 106,110 Gate Purchase with ScaleKart lookup).
     * // KEEP-EXEC: sub-branch selected from DB state, STRING_AGG temp table, ScaleKart range lookup, @PayableFreightPaid variable.
     */
    @Query(value = "EXEC dbo.Sp_GatePassInward_GetAllMethod @Activity = 'ReadByGpNoForGRN', @Id = :id, @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @BranchesId = :branchesId, @DocumentTypeId = :documentTypeId, @FinancialYearId = :financialYearId",
         nativeQuery = true)
    List<GatePassInwardForGrnRow> readByGpNoForGRN(@Param("id") Integer id, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId);

    /** Sp_GatePassInward_GetAllMethod @Activity='ReadByGpNoForGRNWithLabAttach' */
    @Query(value = "SELECT g.SupplierWeight, wb.NetWbWeight, g.Id, g.GpSrNo, g.GpDate, g.VehicleType, g.VehicleNo, g.BiltyNo, g.OtherRemarks, "
         + "g.NetPaid, g.NoOfPackages, g.SupplierFirstWeight, g.SupplierSecondWeight, g.FactoryWeight, g.DifferenceWeight, "
         + "g.SupplierContractCode, wh.WareHouseName, sp.CompanyName, g.PurchaseOrderId, g.Status, g.SupplierCustomerId, "
         + "lab.AnalystName, lab.DeductionWeight, g.RefDocumentTypeId "
         + "FROM WbTransactions wb "
         + "INNER JOIN GatePassInward g on g.Id = wb.ReferenceDocNoId and g.DocumentTypeId = wb.ReferenceDocTypeId "
         + "left join InvWareHouse wh on g.WarehouseId = wh.Id "
         + "inner join SupplierCustomer sp on g.SupplierCustomerId = sp.Id "
         + "inner join InvLabAnalysisPurchaseHeader lab on g.Id = lab.GatePassInwardId "
         + "where g.DocumentTypeId = :documentTypeId and g.OrganizationId = :organizationId and g.CompanyId = :companyId "
         + "AND (:branchesId is null or g.BranchesId = :branchesId) "
         + "and (:financialYearId is null or g.FinancialYearId = :financialYearId) "
         + "and g.Id = :id and wb.NetWbWeight > 0 "
         + "and not EXISTS (select 1 from InvGrn WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND (:branchesId is null or InvGrn.BranchesId = :branchesId) AND InwardGatePassId = g.Id)", nativeQuery = true)
    List<GatePassInwardForGrnWithLabRow> readByGpNoForGRNWithLabAttach(@Param("documentTypeId") Integer documentTypeId,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId, @Param("financialYearId") Integer financialYearId, @Param("id") Integer id);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='GpNoPandingforInvGrn' — #tmpGrnRecord / #GpNoPandingforInvGrn / #LabInfo pipeline folded into
     * CTEs; the "Update R Set LabId/LabNo from #LabInfo" step becomes OUTER APPLY (TOP 1 lab per GP, 0 when none).
     */
    @Query(value = "WITH tmpGrnRecord AS (select h.InwardGatePassId, ISNULL(SUM(d.GrossWeight),0) as ReceivedWeight, "
         + "(ISNULL(SUM(d.StockWeight),0) + ISNULL(SUM(d.EBWTotal),0) + ISNULL(SUM(d.WtCutTotal),0)) as StockWeight "
         + "from InvGrn h inner join InvGrnDetail d on d.InvGrnId = h.id "
         + "where h.OrganizationId = :organizationId and h.CompanyId = :companyId and (:branchesId is null or h.BranchesId = :branchesId) "
         + "GROUP BY h.InwardGatePassId), "
         + "R AS (SELECT gp.Id, gp.GpSrNo, gp.GpDate, SP.CompanyName AS SupplierName, gp.VehicleNo, gp.BiltyNo, gp.VarietyName, "
         + "gp.NoOfPackages as Qty, gp.SupplierWeight, gp.SupplierFirstWeight, gp.SupplierSecondWeight, GP.FactoryWeight, "
         + "0 AS ReceivedWeight, 0 as StockWeight, gp.Status, gp.OtherSupCust as OrderType, gp.VehicleType, gp.NetPaid, gp.RefDocumentTypeId, "
         + "gp.EntryDate, Concat(Usr.FirstName,'-',Usr.LastName) as EntryUser, gp.ModifyDate, Concat(MUsr.FirstName,'-',MUsr.LastName) as ModifyUser, "
         + "(select count(Id) from GatePassInwardDetail where GatePassInwardId = gp.Id) as DetailIdsCount, "
         + "case when gp.Freight > 0 and gp.FreightOn = 1 then 1 else 0 end as FreightOnCash, fr.Id as FreightId, "
         + "Gp.AccessWeight, Gp.SupplierContractCode "
         + "FROM GatePassInward gp with(nolock) "
         + "INNER JOIN UserAccount Usr on gp.EntryUser = Usr.ID "
         + "LEFT JOIN SupplierCustomer SP with(nolock) ON gp.SupplierCustomerId = SP.Id "
         + "LEFT JOIN InvFreightVoucher fr on gp.Id = fr.GatePassId "
         + "LEFT JOIN UserAccount MUsr on gp.ModifyUser = MUsr.ID "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and (:financialYearId is null or gp.FinancialYearId = :financialYearId) and (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and gp.RefDocumentTypeId in (41,106) and gp.Status <> 'Rejected' "
         + "and NOT EXISTS (select 1 from InvGrn WHERE OrganizationId = gp.OrganizationId AND CompanyId = gp.CompanyId AND InwardGatePassId = gp.Id) "
         + "UNION ALL "
         + "SELECT gp.Id, gp.GpSrNo, gp.GpDate, SP.CompanyName AS SupplierName, gp.VehicleNo, gp.BiltyNo, gp.VarietyName, gp.NoOfPackages as Qty, "
         + "gp.SupplierWeight, gp.SupplierFirstWeight, gp.SupplierSecondWeight, gp.FactoryWeight, isnull(t.ReceivedWeight,0), isnull(t.StockWeight,0), "
         + "gp.Status, gp.OtherSupCust as OrderType, gp.VehicleType, gp.NetPaid, gp.RefDocumentTypeId, gp.EntryDate, "
         + "Concat(Usr.FirstName,'-',Usr.LastName) as EntryUser, gp.ModifyDate, Concat(MUsr.FirstName,'-',MUsr.LastName) as ModifyUser, "
         + "(select count(Id) from GatePassInwardDetail where GatePassInwardId = gp.Id) as DetailIdsCount, "
         + "case when gp.Freight > 0 and gp.FreightOn = 1 then 1 else 0 end as FreightOnCash, fr.Id as FreightId, Gp.AccessWeight, gp.SupplierContractCode "
         + "FROM GatePassInward gp "
         + "INNER JOIN UserAccount Usr on gp.EntryUser = Usr.ID "
         + "LEFT JOIN SupplierCustomer SP ON gp.SupplierCustomerId = SP.Id "
         + "LEFT JOIN InvGrn h on h.InwardGatePassId = gp.Id AND gp.RefDocumentTypeId = 105 "
         + "LEFT JOIN tmpGrnRecord t on gp.Id = t.InwardGatePassId "
         + "LEFT JOIN InvFreightVoucher fr on gp.Id = fr.GatePassId "
         + "LEFT JOIN UserAccount MUsr on gp.ModifyUser = MUsr.ID "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and (:financialYearId is null or gp.FinancialYearId = :financialYearId) and (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and gp.RefDocumentTypeId = 105 "
         + "and ((gp.Status = 'Accepted' and ISNULL(gp.FactoryWeight,0) - Round(ISNULL(t.StockWeight,0),0) > 0) OR (gp.Status = 'Open')) "
         + "group by gp.Id, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.BiltyNo, gp.VarietyName, gp.NoOfPackages, gp.SupplierWeight, "
         + "gp.SupplierFirstWeight, gp.SupplierSecondWeight, gp.FactoryWeight, gp.Status, gp.OtherSupCust, gp.VehicleType, gp.NetPaid, "
         + "gp.RefDocumentTypeId, SP.CompanyName, t.ReceivedWeight, t.StockWeight, gp.Freight, gp.FreightOn, fr.Id, gp.EntryDate, gp.ModifyDate, "
         + "Usr.FirstName, Usr.LastName, MUsr.FirstName, MUsr.LastName, Gp.AccessWeight, gp.SupplierContractCode) "
         + "Select R.Id, R.GpSrNo, R.GpDate, R.SupplierName, R.VehicleNo, R.BiltyNo, R.VarietyName, R.Qty, R.SupplierFirstWeight, "
         + "R.SupplierSecondWeight, R.SupplierWeight, R.FactoryWeight, R.ReceivedWeight, R.StockWeight, R.Status, R.OrderType, R.VehicleType, "
         + "R.NetPaid, R.RefDocumentTypeId, R.EntryDate, R.EntryUser, R.ModifyDate, R.ModifyUser, R.DetailIdsCount, R.FreightOnCash, R.FreightId, "
         + "ISNULL(lab.LabId, 0) AS LabId, ISNULL(lab.LabNo, 0) AS LabNo, R.AccessWeight, R.SupplierContractCode "
         + "From R OUTER APPLY (SELECT TOP 1 P.Id LabId, P.DocNo LabNo FROM InvLabAnalysisPurchaseHeader P WHERE P.GatePassInwardId = R.Id) lab",
         nativeQuery = true)
    List<GatePassInwardPendingForGrnRow> gpNoPandingforInvGrn(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='ReadByGPDate' — #TmpResultOpenGP / #tmpDms / #TmpWbWeight pipeline folded into CTEs
     * (NoOfAttachments and FactoryWeight updates become joined computed columns).
     */
    @Query(value = "WITH R AS (SELECT gp.Id, GP.DocumentTypeId, gp.GatepassType, gp.GpDate, gp.GpSrNo, "
         + "Case when GP.RefDocumentTypeId = 41 then 'Purchase Order' when GP.RefDocumentTypeId = 104 then 'Supply Order' "
         + "when GP.RefDocumentTypeId = 105 then 'Market Purchase' when GP.RefDocumentTypeId = 232 then 'Import PO' "
         + "when GP.RefDocumentTypeId = 106 then 'Gate Purchase' when GP.RefDocumentTypeId = 1500 then 'Purchase_Order' "
         + "when GP.RefDocumentTypeId = 700 then 'Purchase_Order_Pm' WHEN GP.RefDocumentTypeId IN (98) THEN 'Sale Return' "
         + "WHEN GP.RefDocumentTypeId IN (52) THEN 'General' when GP.RefDocumentTypeId = 204 then 'Export Return' "
         + "when GP.RefDocumentTypeId = 241 then 'Export Dispatch Return' when gp.RefDocumentTypeId = 175 then 'Stock Received' "
         + "else 'Other' END OrderType, gp.VehicleType, gp.VehicleNo, gp.BiltyNo, gp.Freight, gp.AdvanceByParty, gp.AdvanceByFactory, "
         + "gp.Freight - ISNULL(gp.AdvanceByFactory,0) - ISNULL(gp.AdvanceByParty,0) AS TotalPayableFreight, gp.NetPaid, gp.VarietyName, "
         + "GP.WarehouseId, gp.NoOfPackages as ItemQty, GP.SupplierCustomerId, gp.OtherRemarks, gp.InDateTimeStamp as InTime, "
         + "CASE WHEN gp.RefDocumentTypeId in (41,700) then gp.SupplierContractCode ELSE 0 END as OrderNo, "
         + "gp.SupplierFirstWeight, gp.SupplierSecondWeight, gp.SupplierWeight, gp.FactoryWeight, gp.DifferenceWeight, GP.CityId, gp.Status, "
         + "gp.RefDocumentTypeId, gp.RateCut, gp.RateCutRemarks, gp.AccessWeight, "
         + "Case when ISNULL(gp.IsApproved,0) = 1 then 'Approved' else 'Not Approved' end as ApprovalStatus, "
         + "gp.WeightComparedToPoWt, gp.PackUnit, CAST(0 as bit) as RefferedInWbOrLab "
         + "FROM GatePassInward gp "
         + "WHERE (gp.OrganizationId = :organizationId) AND (gp.CompanyId = :companyId) AND (gp.DocumentTypeId = :documentTypeId) "
         + "AND (:financialYearId IS NULL OR gp.FinancialYearId = :financialYearId) AND (:branchesId IS NULL OR gp.BranchesId = :branchesId) "
         + "and (gp.Status = 'Open')), "
         + "tmpDms AS (SELECT COUNT(d.Id) as NoOfAttachments, d.RefDocumentTypeId, d.RefDocumentNo FROM dbo.DMSAttachments d "
         + "INNER JOIN R on d.RefDocumentTypeId = R.DocumentTypeId and d.RefDocumentNo = R.Id "
         + "WHERE D.OrganizationId = :organizationId AND D.CompanyId = :companyId GROUP BY d.RefDocumentTypeId, d.RefDocumentNo), "
         + "TmpWbWeight AS (SELECT R.DocumentTypeId, R.Id, SUM(T.NetWbWeight) AS WBWeight FROM R "
         + "INNER JOIN WbTransactions T ON R.DocumentTypeId = T.ReferenceDocTypeId AND R.Id = T.ReferenceDocNoId GROUP BY R.DocumentTypeId, R.Id) "
         + "SELECT R.Id, R.DocumentTypeId, R.GatepassType, R.GpDate, R.GpSrNo, R.OrderType, R.VehicleType, R.VehicleNo, R.BiltyNo, R.Freight, "
         + "R.AdvanceByFactory, R.AdvanceByParty, R.TotalPayableFreight, R.NetPaid, R.VarietyName, W.WareHouseName, R.ItemQty, "
         + "SP.CompanyName AS SupplierName, R.OtherRemarks, R.InTime, R.OrderNo, R.SupplierFirstWeight, R.SupplierSecondWeight, R.SupplierWeight, "
         + "CASE WHEN WB.Id IS NOT NULL THEN WB.WBWeight ELSE R.FactoryWeight END AS FactoryWeight, "
         + "R.SupplierWeight - (CASE WHEN WB.Id IS NOT NULL THEN WB.WBWeight ELSE R.FactoryWeight END) AS DifferenceWeight, "
         + "C.Description AS CityName, R.Status, R.RefDocumentTypeId, ISNULL(dm.NoOfAttachments, 0) AS NoOfAttachments, "
         + "R.RateCut, R.RateCutRemarks, R.WeightComparedToPoWt, R.PackUnit, R.AccessWeight, R.ApprovalStatus, R.RefferedInWbOrLab "
         + "FROM R "
         + "LEFT JOIN InvWareHouse W ON R.WarehouseId = W.Id "
         + "LEFT JOIN SupplierCustomer SP ON R.SupplierCustomerId = SP.Id "
         + "LEFT JOIN City C ON R.CityId = C.Id "
         + "LEFT JOIN tmpDms dm ON dm.RefDocumentTypeId = R.DocumentTypeId AND dm.RefDocumentNo = R.Id "
         + "LEFT JOIN TmpWbWeight WB ON WB.DocumentTypeId = R.DocumentTypeId AND WB.Id = R.Id "
         + "ORDER BY R.GpSrNo DESC", nativeQuery = true)
    List<GatePassInwardOpenByDateRow> readByGPDate(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetOpenGatePassForGpDetail' */
    @Query(value = "SELECT gp.Id, gp.GatepassType, gp.RefDocumentTypeId, gp.GpDate, gp.RefDocumentEntryNo as GpSrNo, gp.VehicleType, gp.VehicleNo, "
         + "gp.BiltyNo, gp.NoOfPackages as ItemQty, SupplierCustomer.CompanyName as SupplierName, gp.SupplierCNICNO, gp.SupplierMobileNo, "
         + "gp.DriverName, gp.DriverCNICNO, gp.DriverMobileNo, ds.Description District, t.Description as Tehsil, City.Description AS CityName, "
         + "gp.Status, gp.DocAttachment as WbStatus, gp.SupplierFirstWeight, gp.SupplierSecondWeight, gp.SupplierWeight, WB.WbWeight AS FactoryWeight, "
         + "gp.OtherRemarks, gp.InDateTimeStamp as InTime, gp.EntryDate, CONCAT(Usr.FirstName,'-',Usr.LastName) as EntryUser, "
         + "gp.ModifyDate, CONCAT(Musr.FirstName,'-',Musr.LastName) as ModifyUser "
         + "FROM GatePassInward gp "
         + "INNER JOIN (SELECT GatePassInwardId, sum(ItemQty) as ItemQty FROM GatePassInwardDetail group by GatePassInwardId) d on gp.Id = d.GatePassInwardId "
         + "INNER JOIN SupplierCustomer ON gp.SupplierCustomerId = SupplierCustomer.Id "
         + "INNER JOIN City ON gp.CityId = City.Id "
         + "INNER JOIN District ds on gp.DestrictId = ds.Id "
         + "INNER JOIN Tehsil t on gp.TehsilId = t.Id "
         + "INNER JOIN UserAccount Usr on gp.EntryUser = Usr.ID "
         + "LEFT JOIN UserAccount Musr on gp.ModifyUser = Musr.ID "
         + "LEFT JOIN (SELECT ReferenceDocTypeId, ReferenceDocNoId, ISNULL(Sum(NetWbWeight),0) AS WbWeight FROM WbTransactions "
         + "GROUP BY ReferenceDocTypeId, ReferenceDocNoId) wb ON GP.DocumentTypeId = WB.ReferenceDocTypeId AND GP.Id = WB.ReferenceDocNoId "
         + "WHERE (gp.OrganizationId = :organizationId) AND (gp.CompanyId = :companyId) AND (gp.DocumentTypeId = :documentTypeId) "
         + "AND (:branchesId is null or gp.BranchesId = :branchesId) AND (:financialYearId IS NULL OR gp.FinancialYearId = :financialYearId) "
         + "AND (:refDocumentTypeId IS NULL OR gp.RefDocumentTypeId = :refDocumentTypeId) and (gp.Status = 'Open') "
         + "order by GpSrNo desc", nativeQuery = true)
    List<GatePassInwardOpenForGpDetailRow> getOpenGatePassForGpDetail(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("branchesId") Integer branchesId,
            @Param("financialYearId") Integer financialYearId, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetOpenGatePassForGhallaMandiGpDetail' (no @BranchesId filter in this branch). */
    @Query(value = "SELECT gp.Id, gp.GatepassType, gp.RefDocumentTypeId, gp.GpDate, gp.RefDocumentEntryNo as GpSrNo, gp.VehicleType, gp.VehicleNo, "
         + "gp.BiltyNo, gp.NoOfPackages as ItemQty, SupplierCustomer.CompanyName as LoadingContractor, gp.DriverName, gp.DriverMobileNo, "
         + "gm.GhallaMandi, ts.CompanyName as CarriageContractor, gp.Status, gp.DocAttachment as WbStatus, gp.SupplierFirstWeight, "
         + "gp.SupplierSecondWeight, gp.SupplierWeight, WB.WbWeight AS FactoryWeight, gp.OtherRemarks, gp.InDateTimeStamp as InTime, "
         + "gp.EntryDate, CONCAT(Usr.FirstName,'-',Usr.LastName) as EntryUser, gp.ModifyDate, CONCAT(Musr.FirstName,'-',Musr.LastName) as ModifyUser "
         + "FROM GatePassInward gp "
         + "INNER JOIN (SELECT GatePassInwardId, sum(ItemQty) as ItemQty FROM GatePassInwardDetail group by GatePassInwardId) d on gp.Id = d.GatePassInwardId "
         + "INNER JOIN SupplierCustomer ON gp.LoadingContractorId = SupplierCustomer.Id "
         + "INNER JOIN Ghalla_Mandi gm ON gp.GhallaMandiId = gm.Id "
         + "LEFT JOIN SupplierCustomer ts on gp.CarriageContractorId = ts.Id "
         + "INNER JOIN UserAccount Usr on gp.EntryUser = Usr.ID "
         + "LEFT JOIN UserAccount Musr on gp.ModifyUser = Musr.ID "
         + "LEFT JOIN (SELECT ReferenceDocTypeId, ReferenceDocNoId, ISNULL(Sum(NetWbWeight),0) AS WbWeight FROM WbTransactions "
         + "GROUP BY ReferenceDocTypeId, ReferenceDocNoId) wb ON GP.DocumentTypeId = WB.ReferenceDocTypeId AND GP.Id = WB.ReferenceDocNoId "
         + "WHERE (gp.OrganizationId = :organizationId) AND (gp.CompanyId = :companyId) AND (gp.DocumentTypeId = :documentTypeId) "
         + "AND (:financialYearId IS NULL OR gp.FinancialYearId = :financialYearId) "
         + "AND (:refDocumentTypeId IS NULL OR gp.RefDocumentTypeId = :refDocumentTypeId) and (gp.Status = 'Open') "
         + "order by GpSrNo desc", nativeQuery = true)
    List<GatePassInwardOpenForGhallaMandiRow> getOpenGatePassForGhallaMandiGpDetail(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("financialYearId") Integer financialYearId, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetOrderTypeForGpInward' (config variables 113/112/111/273/280/281 folded into a cfg CTE; @getBranchesCount is unused by the proc). */
    @Query(value = "WITH cfg AS (SELECT CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 113)) AS POFENABLE, "
         + "CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 112)) AS MRKFENABLE, "
         + "CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 111)) AS GPENABLE, "
         + "CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 273)) AS PurchaseOrderBranchWise, "
         + "CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 280)) AS SaleInvoiceReturnBranchWise, "
         + "CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 281)) AS StockTransferMultiLocationsAllowed) "
         + "Select 41 AS Id, 'Purchase Order' OrderType FROM cfg where (select count(Id) from PurchaseOrder where OrganizationId = :organizationId "
         + "and CompanyId = :companyId AND ((ISNULL(cfg.PurchaseOrderBranchWise,0) = 1 and BranchesId = :branchesId) OR (ISNULL(cfg.PurchaseOrderBranchWise,0) = 0))) > 0 "
         + "and ISNULL(cfg.POFENABLE,0) = 1 "
         + "Union All Select 236 AS Id, 'Import Order' AS OrderType where (select count(id) from ExImLcOrderPurchaseOrder where OrganizationId = :organizationId and CompanyId = :companyId) > 0 "
         + "UNION ALL SELECT 105 AS Id, 'Market Purchase' as OrderType FROM cfg WHERE ISNULL(cfg.MRKFENABLE,0) = 1 "
         + "UNION ALL SELECT 106 AS Id, 'Gate Purchase' as OrderType FROM cfg WHERE ISNULL(cfg.GPENABLE,0) = 1 "
         + "UNION ALL SELECT 175 AS Id, 'Stock Received' as OrderType FROM cfg where (select Count(Id) from InvDeliveryOrder where OrganizationId = :organizationId "
         + "and CompanyId = :companyId AND (:branchesId is null or ToBranchId = :branchesId) and DeliveryOrderType = 'StockTransfer' "
         + "AND (cfg.StockTransferMultiLocationsAllowed = 1 or ToBranchId > 0)) > 0 "
         + "Union All Select 1500 AS Id, 'Purchase_Order' OrderType FROM cfg where (select count(Id) from ST.PurchaseOrder where OrganizationId = :organizationId "
         + "and CompanyId = :companyId) > 0 and ISNULL(cfg.POFENABLE,0) = 1 "
         + "Union All Select 700 AS Id, 'Purchase_Order_PM' OrderType FROM cfg where (select count(Id) from dbo.PurchaseOrder where OrganizationId = :organizationId "
         + "and CompanyId = :companyId AND ((ISNULL(cfg.PurchaseOrderBranchWise,0) = 1 and BranchesId = :branchesId) OR (ISNULL(cfg.PurchaseOrderBranchWise,0) = 0))) > 0 "
         + "and ISNULL(cfg.POFENABLE,0) = 1 "
         + "Union All Select 98 AS Id, 'Sale Return' OrderType FROM cfg where (select count(Id) from dbo.InvSaleInvoice where OrganizationId = :organizationId "
         + "and CompanyId = :companyId and DocumentTypeId in (95,99) AND ((ISNULL(cfg.SaleInvoiceReturnBranchWise,0) = 1 and BranchesId = :branchesId) "
         + "OR (ISNULL(cfg.SaleInvoiceReturnBranchWise,0) = 0))) > 0 "
         + "Union All Select 52 AS Id, 'General' OrderType "
         + "UNION ALL SELECT 204 AS Id, 'Export Return' as OrderType where (select count(Id) from ExportVoucher where OrganizationId = :organizationId "
         + "and CompanyId = :companyId and DocumentTypeId = 27) > 0 "
         + "UNION ALL SELECT 241 AS Id, 'Export Dispatch Return' as OrderType where (select count(Id) from ExImForwarding where OrganizationId = :organizationId "
         + "and CompanyId = :companyId) > 0", nativeQuery = true)
    List<GatePassInwardOrderTypeRow> getOrderTypeForGpInward(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("branchesId") Integer branchesId);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='GetRefDocNoidByDocumentTypeId', sub-branch @DocumentTypeId=102 AND @RefDocumentTypeId=91
     * AND ISNULL(@CheckConfig,0)=1 (outward gate numbers for weigh bridge; @WeighBridgeForOutwardPartyAndItemWise config 5090 inlined).
     * Gate conditions kept in the WHERE so other parameter combinations return nothing, like the proc.
     */
    @Query(value = "SELECT Distinct gp.Id, gp.DocumentTypeId, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, gp.OtherRemarks, "
         + "gp.SupplierContractCode, wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.GatepassType "
         + "FROM dbo.GatePassOutward AS gp with(nolock) "
         + "LEFT JOIN dbo.WbTransactions AS wbt with(nolock) ON gp.DocumentTypeId = wbt.ReferenceDocTypeId AND gp.Id = wbt.ReferenceDocNoId "
         + "Left Join InvDeliveryOrderDetail dd on dd.InvDeliveryOrderId = gp.SaleOrderId "
         + "Where :documentTypeId = 102 and :refDocumentTypeId = 91 "
         + "and gp.OrganizationId = :organizationId and gp.CompanyId = :companyId "
         + "and (:financialYearId is null or gp.FinancialYearId = :financialYearId) and (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and gp.Status = 'Open' "
         + "AND (ISNULL((Select ISNULL(ConfigKey,0) From ConfigrationsAllocation where OrganizationId = :organizationId and CompanyId = :companyId "
         + "and ConfigrationsDefinitionId = 5090),0) = 0 OR Not Exists (Select 1 From WbTransactions wb Where Wb.ReferenceDocTypeId = Gp.DocumentTypeId "
         + "And Wb.ReferenceDocNoId = Gp.Id AND Wb.ItemId = dd.ItemId AND wb.SupplierCustomerId = dd.SupplierCustomerId)) "
         + "and NOT EXISTS (SELECT 1 FROM InvGdn GH WHERE GH.OutwardGatePassId = GP.Id) "
         + "GROUP BY gp.Id, gp.DocumentTypeId, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, gp.OtherRemarks, gp.SupplierContractCode, "
         + "wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.GatepassType", nativeQuery = true)
    List<GatePassOutwardForWeighBridgeRow> getRefDocNoidByDocumentTypeIdWithCheckConfig(@Param("documentTypeId") Integer documentTypeId,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='GetRefDocNoidByDocumentTypeId', sub-branch @DocumentTypeId=102 AND @RefDocumentTypeId=91
     * AND ISNULL(@CheckConfig,0)=0 (outward gate numbers not yet referred in weigh bridge).
     */
    @Query(value = "SELECT gp.Id, gp.DocumentTypeId, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, gp.OtherRemarks, gp.SupplierContractCode, "
         + "wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.GatepassType "
         + "FROM dbo.GatePassOutward AS gp with(nolock) "
         + "LEFT JOIN dbo.WbTransactions AS wbt with(nolock) ON gp.DocumentTypeId = wbt.ReferenceDocTypeId AND gp.Id = wbt.ReferenceDocNoId "
         + "Where :documentTypeId = 102 and :refDocumentTypeId = 91 "
         + "and gp.OrganizationId = :organizationId and gp.CompanyId = :companyId "
         + "and (:financialYearId is null or gp.FinancialYearId = :financialYearId) and (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and NOT EXISTS (SELECT 1 FROM InvGdn GH WHERE GH.OutwardGatePassId = GP.Id) "
         + "GROUP BY gp.Id, gp.DocumentTypeId, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, gp.OtherRemarks, gp.SupplierContractCode, "
         + "wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.GatepassType "
         + "HAVING (wbt.ReferenceDocNoId IS NULL)", nativeQuery = true)
    List<GatePassOutwardForWeighBridgeRow> getRefDocNoidByDocumentTypeIdWithoutCheckConfig(@Param("documentTypeId") Integer documentTypeId,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='ReadDetailByGatePassInward' (d.* + lookups, verbatim incl. duplicated RemarksDetail/RefDocumentTypeId columns). */
    @Query(value = "SELECT d.*, pt.PackTypeDesc as PackingType, s.DocNo, i.ItemName, JL.JobLotDescription, uomsch.Equivalent, uomsch.UOMCode, "
         + "wh.WareHouseName, C.Description CityName, p.DocNo OrderNo, d.RemarksDetail, d.RefDocumentTypeId "
         + "FROM GatePassInwardDetail AS d "
         + "inner JOIN SupplyScheduleDetail AS ss ON d.SupplyScheduleId = ss.Id "
         + "inner join SupplySchedule s on s.Id = ss.SupplyScheduleId "
         + "Inner join JobLot AS JL ON d.JobLotId = JL.Id "
         + "left join InvPackingType pt on d.PackingTypeId = pt.Id "
         + "Left JOIN Item AS i ON d.ItemId = i.Id "
         + "Left JOIN V_UomScheduleAndUom AS uomsch ON d.ItemUOMId = uomsch.Id "
         + "Left JOIN InvWareHouse AS wh ON d.WareHouseId = wh.Id "
         + "Left join PurchaseOrder p on p.Id = d.PurchaseOrderId "
         + "Left join City C on C.id = d.CityId "
         + "where d.GatePassInwardId = :id", nativeQuery = true)
    List<GatePassInwardDetailWithSupplyRow> readDetailByGatePassInward(@Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetForWeighBridgeGatePassGeneral' (dbo.GatePassGeneral). */
    @Query(value = "SELECT Distinct gp.Id, gp.GpSrNo, gp.VehicleNo "
         + "FROM dbo.GatePassGeneral AS gp with(nolock) "
         + "LEFT OUTER JOIN dbo.WbTransactions AS wbt with(nolock) ON gp.DocumentTypeId = wbt.ReferenceDocTypeId AND gp.Id = wbt.ReferenceDocNoId "
         + "Where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and gp.FinancialYearId = :financialYearId AND (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and gp.Status = 'Weighable' and gp.DocAttachment = 'Open' "
         + "and gp.Id not in (select wb.ReferenceDocNoId from WbTransactions wb where wb.OrganizationId = :organizationId "
         + "and wb.CompanyId = :companyId and wb.ReferenceDocTypeId = :documentTypeId)", nativeQuery = true)
    List<GatePassGeneralForWeighBridgeRow> getForWeighBridgeGatePassGeneral(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetGatePassNoForStockTransfer', sub-branch @DocumentTypeId = 51 (inward, RefDocumentTypeId 175). */
    @Query(value = "SELECT gp.Id, gp.GpSrNo, gp.PurchaseOrderId as RefDocId "
         + "FROM GatePassInward AS gp with(nolock) "
         + "INNER JOIN dbo.WbTransactions AS wbt with(nolock) ON gp.DocumentTypeId = wbt.ReferenceDocTypeId AND gp.Id = wbt.ReferenceDocNoId "
         + "Where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.FinancialYearId = :financialYearId "
         + "and gp.BranchesId = :branchesId and gp.DocumentTypeId = 51 and gp.RefDocumentTypeId = 175 and gp.Status = 'Accepted' "
         + "and wbt.NetWbWeight > 0 "
         + "and CONCAT(gp.Id, wbt.Id) not in (select CONCAT(GatePassId, WbTicketId) from InvStockTransferHeader where OrganizationId = :organizationId "
         + "and CompanyId = :companyId AND TransferType = 'Inward')", nativeQuery = true)
    List<GatePassForStockTransferRow> getGatePassNoForStockTransferInward(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetGatePassNoForStockTransfer', sub-branch @DocumentTypeId = 91 (outward; pass documentTypeId = 91). */
    @Query(value = "SELECT gp.Id, gp.GpSrNo, gp.SaleOrderId as RefDocId "
         + "FROM GatePassOutward AS gp with(nolock) "
         + "INNER JOIN InvDeliveryOrder do on gp.SaleOrderId = do.Id and gp.OtherSupCust = 'DeliverOrder' and do.ActionId <> 3 "
         + "INNER JOIN dbo.WbTransactions AS wbt with(nolock) ON gp.DocumentTypeId = wbt.ReferenceDocTypeId AND gp.Id = wbt.ReferenceDocNoId "
         + "Where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and gp.FinancialYearId = :financialYearId and gp.BranchesId = :branchesId and do.DeliveryOrderType = 'StockTransfer' "
         + "and gp.Status = 'Accepted' and wbt.NetWbWeight > 0 "
         + "and CONCAT(gp.Id, wbt.Id) not in (select CONCAT(GatePassId, WbTicketId) from InvStockTransferHeader where OrganizationId = :organizationId "
         + "and CompanyId = :companyId AND TransferType = 'Outward')", nativeQuery = true)
    List<GatePassForStockTransferRow> getGatePassNoForStockTransferOutward(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetGatePassOutwardForMannualWeighBridge' (config 5090 inlined). */
    @Query(value = "SELECT Distinct gp.Id, gp.GpSrNo, gp.VehicleNo, gp.GatepassType "
         + "FROM dbo.GatePassOutward AS gp with(nolock) "
         + "Left Join InvDeliveryOrderDetail dd on dd.InvDeliveryOrderId = gp.SaleOrderId "
         + "Where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and gp.FinancialYearId = :financialYearId and (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and gp.Status = 'Open' and gp.DocAttachment <> 'Auto' "
         + "AND NOT Exists (SELECT ReferenceDocNoId FROM WbTransactions WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND ReferenceDocTypeId = :documentTypeId AND WbTransactions.BranchId = :branchesId and ISNULL(FirstWeight,0) > 0 "
         + "AND ReferenceDocNoId = Gp.Id and ISNULL(:checkConfig,0) = 0) "
         + "AND (ISNULL((Select ISNULL(ConfigKey,0) From ConfigrationsAllocation where OrganizationId = :organizationId and CompanyId = :companyId "
         + "and ConfigrationsDefinitionId = 5090),0) = 0 OR Not Exists (Select 1 From WbTransactions wb Where Wb.ReferenceDocTypeId = Gp.DocumentTypeId "
         + "And Wb.ReferenceDocNoId = Gp.Id AND Wb.ItemId = dd.ItemId AND wb.SupplierCustomerId = dd.SupplierCustomerId AND (:recId is null or WB.Id <> :recId))) "
         + "and NOT EXISTS (SELECT 1 FROM InvGdn GH WHERE GH.OutwardGatePassId = GP.Id)", nativeQuery = true)
    List<GatePassOutwardManualWbRow> getGatePassOutwardForMannualWeighBridge(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId,
            @Param("checkConfig") Boolean checkConfig, @Param("recId") Integer recId);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='GetGpSrNoForPurchaseAnalysis' — #TmpGpData + LatestLab UPDATE folded into CTEs
     * (@LabCompAfterFirstWeight = config 99 inlined; LabType ordinal computed by LEFT JOIN on the latest lab, default '1st Lab').
     */
    @Query(value = "WITH cfg AS (SELECT CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 99)) AS LabCompAfterFirstWeight), "
         + "R AS (SELECT DISTINCT g.Id, g.GpSrNo, g.GpDate, g.VehicleType, g.VehicleNo, g.RefDocumentTypeId, S.CompanyName AS SupplierName, "
         + "g.BiltyNo, g.BiltyDate, c.Description AS City, g.InDateTimeStamp, EntryUser.FirstName + ' ' + EntryUser.LastName AS EntryUserName, "
         + "g.EntryUser, g.EntryDate, "
         + "CASE WHEN g.RefDocumentTypeId = 41 THEN 'Regular Purchase (PO)' WHEN g.RefDocumentTypeId IN (105, 111) THEN 'Market Purchase' "
         + "WHEN g.RefDocumentTypeId IN (106, 110) THEN 'Gate Purchase' WHEN g.RefDocumentTypeId = 98 THEN 'Sale Return' "
         + "WHEN g.RefDocumentTypeId = 700 THEN 'Order_PM' WHEN g.RefDocumentTypeId = 52 THEN 'General' "
         + "WHEN g.RefDocumentTypeId = 175 THEN 'Stock Received' END AS OrderType "
         + "FROM GatePassInward g CROSS JOIN cfg "
         + "LEFT JOIN SupplierCustomer S ON S.Id = g.SupplierCustomerId "
         + "LEFT JOIN dbo.UserAccount EntryUser ON EntryUser.ID = g.EntryUser "
         + "LEFT JOIN City c ON c.Id = g.CityId "
         + "LEFT JOIN WbTransactions wb ON g.DocumentTypeId = wb.ReferenceDocTypeId AND g.Id = wb.ReferenceDocNoId "
         + "WHERE g.OrganizationId = :organizationId AND g.CompanyId = :companyId AND g.FinancialYearId = :financialYearId "
         + "AND (:branchesId IS NULL OR g.BranchesId = :branchesId) AND (:supplierCustomerId IS NULL OR g.SupplierCustomerId = :supplierCustomerId) "
         + "AND (:orderTypeId IS NULL OR g.RefDocumentTypeId = :orderTypeId) AND g.Status = 'Open' "
         + "AND (cfg.LabCompAfterFirstWeight = 0 OR (g.RefDocumentTypeId = 700 OR (cfg.LabCompAfterFirstWeight = 1 AND wb.FirstWeight > 0)))), "
         + "LatestLab AS (SELECT GatePassInwardId, LabSortNo, ROW_NUMBER() OVER (PARTITION BY GatePassInwardId ORDER BY Id DESC) AS rn "
         + "FROM InvLabAnalysisPurchaseHeader) "
         + "SELECT R.Id, R.GpSrNo, R.GpDate, R.VehicleType, R.VehicleNo, R.RefDocumentTypeId, R.SupplierName, R.BiltyNo, R.BiltyDate, R.City, "
         + "R.InDateTimeStamp, R.EntryUserName, R.EntryUser, R.EntryDate, R.OrderType, "
         + "ISNULL(CASE WHEN (L.LabSortNo + 1) % 100 IN (11, 12, 13) THEN CAST(L.LabSortNo + 1 AS VARCHAR) + 'th Lab' "
         + "WHEN (L.LabSortNo + 1) % 10 = 1 THEN CAST(L.LabSortNo + 1 AS VARCHAR) + 'st Lab' "
         + "WHEN (L.LabSortNo + 1) % 10 = 2 THEN CAST(L.LabSortNo + 1 AS VARCHAR) + 'nd Lab' "
         + "WHEN (L.LabSortNo + 1) % 10 = 3 THEN CAST(L.LabSortNo + 1 AS VARCHAR) + 'rd Lab' "
         + "ELSE CAST(L.LabSortNo + 1 AS VARCHAR) + 'th Lab' END, '1st Lab') AS LabType "
         + "FROM R LEFT JOIN LatestLab L ON R.Id = L.GatePassInwardId AND L.rn = 1", nativeQuery = true)
    List<GatePassInwardForPurchaseAnalysisRow> getGpSrNoForPurchaseAnalysis(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("orderTypeId") Integer orderTypeId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetDataByGpId' (@LabCompGatePur is never set in this branch → GatePurchaseLabConfigStatus = 0). */
    @Query(value = "select distinct h.Id, h.DocDate, h.DocNo, h.ItemId, i.ItemName, h.DeductionWeight, h.DeductionRate, h.ItemRate, h.AnalystName, "
         + "h.RemarksHeader, h.Crop, Pod.Id AS PoDetailId, "
         + "CASE WHEN h.IsAcceptedApproved = 1 AND ISNULL(h.IsApproved,0) = 1 then 'Accepted' "
         + "WHEN ISNULL(h.IsAcceptedApproved,0) = 0 AND ISNULL(h.IsAccepted,0) = 1 AND ISNULL(h.IsApproved,0) = 0 then 'Accepted' "
         + "ELSE 'Rejected' END AS LabStatus, CAST(0 AS bit) AS GatePurchaseLabConfigStatus, EntryUser.UserTitle AS EntryUserName, "
         + "h.LabSortNo, i.ItemCodeNew AS ItemCode, WB.FactoryWeightItemWise "
         + "from InvLabAnalysisPurchaseHeader h "
         + "INNER JOIN GatePassInward gp on h.GatePassInwardId = gp.Id "
         + "INNER JOIN dbo.UserAccount EntryUser on EntryUser.ID = h.EntryUser "
         + "INNER JOIN Item i on h.ItemId = i.Id "
         + "LEFT JOIN PurchaseOrderdetail Pod on Pod.PurchaseOrderId = gp.PurchaseOrderId and Pod.OrderItemId = h.ItemId "
         + "LEFT JOIN (SELECT ReferenceDocTypeId, ReferenceDocNoId, ItemId, SUM(NetWbWeight) AS FactoryWeightItemWise FROM WbTransactions "
         + "GROUP BY ReferenceDocTypeId, ReferenceDocNoId, ItemId) WB ON gp.DocumentTypeId = WB.ReferenceDocTypeId AND gp.Id = WB.ReferenceDocNoId AND h.ItemId = WB.ItemId "
         + "where h.OrganizationId = :organizationId and h.CompanyId = :companyId and h.GatePassInwardId = :id "
         + "ORDER BY h.LabSortNo DESC, H.DocNo DESC", nativeQuery = true)
    List<GatePassInwardLabDataRow> getDataByGpId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='GpNoByPurchaserOrderAndSupplyOrderPendingForGrn' (UNION ALL Supply_Order / Purchase_Order). */
    @Query(value = "Select 'Supply_Order' RefDocTypeDesc, gp.Id GpId, gp.GpDate, gp.GpSrNo, gp.VehicleType, gp.VehicleNo, gp.BiltyNo, "
         + "gp.SupplierFirstWeight, gp.SupplierSecondWeight, gp.SupplierWeight, wb.WbNetWeight, gp.RefDocumentTypeId, gp.RefDocumentEntryNo, "
         + "sph.Id PoSpohId, spd.Id PoSpodId, sph.DocNo PoSpoDocNo, po.Id PurchaseOrderId, po.DocNo PurchaserOrderNo, pod.id purchaserOrderDetailId, "
         + "spd.SupplierCustomerId, sp.CompanyName SupplierName, spd.ItemId, i.ItemName, spd.CropYear, spd.PackSizeId, iu.UOMCode, spd.Qty, "
         + "round((iu.Equivalent * spd.Qty),0) NetWeight "
         + "From GatePassInward gp "
         + "inner JOIN InvSupplyOrder sph on gp.PurchaseOrderId = sph.Id and gp.RefDocumentTypeId = 104 "
         + "inner JOIN InvSupplyOrderDetail spd on sph.Id = spd.InvSupplyOrderId "
         + "inner join PurchaseOrder po on spd.PoHeaderId = po.Id "
         + "INNER JOIN PurchaseOrderDetail pod on po.Id = pod.PurchaseOrderId and spd.PoDetailId = pod.Id "
         + "inner join SupplierCustomer sp on spd.SupplierCustomerId = sp.Id "
         + "INNER JOIN item i on spd.ItemId = i.Id "
         + "INNER JOIN V_UomScheduleAndUom iu on spd.PackSizeId = iu.Id "
         + "inner join (select WbTransactions.ReferenceDocTypeId, WbTransactions.ReferenceDocNoId, sum(NetWbWeight) WbNetWeight from WbTransactions "
         + "Where OrganizationId = :organizationId and CompanyId = :companyId Group by WbTransactions.ReferenceDocTypeId, WbTransactions.ReferenceDocNoId) wb "
         + "on gp.DocumentTypeId = wb.ReferenceDocTypeId and gp.Id = wb.ReferenceDocNoId "
         + "LEFT JOIN InvGrn gh on gh.InwardGatePassId = gp.Id and gh.SupplierCustomerId = spd.SupplierCustomerId "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId "
         + "and (gh.InwardGatePassId is null) and (gh.SupplierCustomerId is null) and (gp.Status = 'Accepted') "
         + "Union All "
         + "Select 'Purchase_Order', gp.Id, gp.GpDate, gp.GpSrNo, gp.VehicleType, gp.VehicleNo, gp.BiltyNo, gp.SupplierFirstWeight, "
         + "gp.SupplierSecondWeight, gp.SupplierWeight, wb.WbNetWeight, gp.RefDocumentTypeId, gp.RefDocumentEntryNo, po.Id PoSpohId, pod.Id PoSpodId, "
         + "po.DocNo, po.Id PurchaseOrderId, po.DocNo PurchaserOrderNo, pod.id purchaserOrderDetailId, po.OrderSupCustId, sp.CompanyName SupplierName, "
         + "pod.OrderItemId, i.ItemName, pod.Crop, pod.OrderItemUOMId, iu.UOMCode, pod.OrderItemQty as Qty, pod.NetWeight as NetWeight "
         + "From GatePassInward gp "
         + "inner JOIN PurchaseOrder po on gp.PurchaseOrderId = po.Id and (gp.RefDocumentTypeId = 41 or gp.RefDocumentTypeId = 0) "
         + "inner JOIN PurchaseOrderDetail pod on po.Id = pod.PurchaseOrderId "
         + "inner join SupplierCustomer sp on po.OrderSupCustId = sp.Id "
         + "INNER JOIN item i on pod.OrderItemId = i.Id "
         + "INNER JOIN V_UomScheduleAndUom iu on pod.OrderItemUOMId = iu.Id "
         + "inner join (SELECT WbTransactions.ReferenceDocTypeId, WbTransactions.ReferenceDocNoId, sum(NetWbWeight) WbNetWeight FROM WbTransactions "
         + "Where OrganizationId = :organizationId and CompanyId = :companyId GROUP BY WbTransactions.ReferenceDocTypeId, WbTransactions.ReferenceDocNoId) wb "
         + "on gp.DocumentTypeId = wb.ReferenceDocTypeId and gp.Id = wb.ReferenceDocNoId "
         + "LEFT JOIN InvGrn gh on gh.InwardGatePassId = gp.Id and gh.SupplierCustomerId = po.OrderSupCustId "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId "
         + "and (gh.InwardGatePassId is null) and (gh.SupplierCustomerId is null) and (gp.Status = 'Accepted')", nativeQuery = true)
    List<GatePassInwardPoSoPendingRow> gpNoByPurchaserOrderAndSupplyOrderPendingForGrn(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GpNoBySupplyOrderPendingForGrn' (@Warehouse / @PackingType config 12 / 11 lookups inlined as a cfg CTE). */
    @Query(value = "WITH cfg AS (SELECT (select Distinct Isnull(ConfigKey,0) from ConfigrationsAllocation where OrganizationId = :organizationId "
         + "and CompanyId = :companyId and ConfigrationsDefinitionId = 12) AS Warehouse, "
         + "(select Distinct Isnull(ConfigKey,0) from ConfigrationsAllocation where OrganizationId = :organizationId and CompanyId = :companyId "
         + "and ConfigrationsDefinitionId = 11) AS PackingType) "
         + "Select pod.id PoDetailId, po.Id OrderId, po.DocNo OrderNo, spd.ItemId, i.ItemName, spd.CropYear, pod.JobLotId JobId, "
         + "cfg.PackingType as PackingTypeId, iu.UOMCode UOM, spd.PackSizeId UOMId, spd.Qty, spd.GrossWeight, spd.EbWtUnit EbUnit, spd.EbWtTotal EbTotal, "
         + "0 as WtCut, 0 as WtCutTotal, 0 as AddLesswt, round((iu.Equivalent * spd.Qty),0) NetWeight, round((iu.Equivalent * spd.Qty),0) StockWeight, "
         + "cfg.Warehouse as WareHouse, po.CityId as City, gp.Id GpId, gp.GpSrNo, gp.VehicleNo, gp.BiltyNo, gp.SupplierFirstWeight, gp.SupplierSecondWeight, "
         + "gp.SupplierWeight, gp.FactoryWeight, gp.NetPaid, spd.SupplierCustomerId, sp.CompanyName SupplierName, po.DeliveryTerm "
         + "From GatePassInward gp CROSS JOIN cfg "
         + "inner JOIN InvSupplyOrder sph on gp.PurchaseOrderId = sph.Id and gp.RefDocumentTypeId = 104 "
         + "inner JOIN InvSupplyOrderDetail spd on sph.Id = spd.InvSupplyOrderId "
         + "inner join PurchaseOrder po on spd.PoHeaderId = po.Id "
         + "INNER JOIN PurchaseOrderDetail pod on po.Id = pod.PurchaseOrderId and spd.PoDetailId = pod.Id "
         + "inner join SupplierCustomer sp on spd.SupplierCustomerId = sp.Id "
         + "INNER JOIN item i on spd.ItemId = i.Id "
         + "INNER JOIN V_UomScheduleAndUom iu on spd.PackSizeId = iu.Id "
         + "inner join (select WbTransactions.ReferenceDocTypeId, WbTransactions.ReferenceDocNoId, sum(NetWbWeight) WbNetWeight from WbTransactions "
         + "Where OrganizationId = :organizationId and CompanyId = :companyId Group by WbTransactions.ReferenceDocTypeId, WbTransactions.ReferenceDocNoId) wb "
         + "on gp.DocumentTypeId = wb.ReferenceDocTypeId and gp.Id = wb.ReferenceDocNoId "
         + "LEFT JOIN InvGrn gh on gh.InwardGatePassId = gp.Id and gh.SupplierCustomerId = spd.SupplierCustomerId "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and (:id is null or gp.Id = :id) "
         + "and (gh.InwardGatePassId is null) and (gh.SupplierCustomerId is null) and (gp.Status = 'Accepted')", nativeQuery = true)
    List<GatePassInwardSoPendingRow> gpNoBySupplyOrderPendingForGrn(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetGatePassForImportGrn' */
    @Query(value = "SELECT DISTINCT gp.Id, gp.GpSrNo FROM dbo.GatePassInward gp "
         + "INNER JOIN dbo.ExImLcOrderPurchaseOrder Sh ON gp.PurchaseOrderId = sh.Id AND sh.DocumentTypeId = 232 "
         + "WHERE gp.OrganizationId = :organizationId AND gp.CompanyId = :companyId AND gp.Status = 'Accepted' and gp.GatePassType = 'Import' "
         + "AND (gp.Id NOT IN (SELECT f.GatePassOutwardId FROM dbo.ImForwarding f WHERE f.OrganizationId = :organizationId AND f.CompanyId = :companyId))",
         nativeQuery = true)
    List<GatePassInwardIdSrNoRow> getGatePassForImportGrn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetShipmentDataByGpNoForImport' */
    @Query(value = "SELECT Distinct gpi.Id, po.ExImLcOrderId, lc.LcOrderNo, po.Id ShipmentId, "
         + "dbo.FunGetShipmentNoByImportOrderId(:organizationId, :companyId, gpi.PurchaseOrderId) AS ShipmentNo, "
         + "gpi.OtherSupCust AS OrderType, gpi.PurchaseOrderId, gpi.GpDate, gpi.GpSrNo, gpi.VehicleNo, gpi.BiltyNo, gpi.VarietyName, gpi.NoOfPackages, "
         + "gpi.FactoryWeight, gpi.DifferenceWeight, gpi.GatepassType, gpi.SupplierContractCode, po.SupplierCustomerId, sp.CompanyName, gpi.VehicleType, "
         + "gpi.Freight, gpi.Container, gpi.Container1, gpi.FactoryWeight as WbGrossWeight "
         + "FROM dbo.GatePassInward gpi "
         + "INNER JOIN dbo.ExImLcOrderPurchaseOrder Po ON Po.Id = gpi.PurchaseOrderId AND po.DocumentTypeId = 232 "
         + "INNER JOIN dbo.ImLcOrder lc ON po.ExImLcOrderId = lc.Id "
         + "INNER JOIN dbo.SupplierCustomer AS sp ON po.SupplierCustomerId = sp.Id "
         + "where gpi.OrganizationId = :organizationId and gpi.CompanyId = :companyId and gpi.Id = :id "
         + "and gpi.Status = 'Accepted' and gpi.GatePassType = 'Import'", nativeQuery = true)
    List<GatePassInwardImportShipmentRow> getShipmentDataByGpNoForImport(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='GpNoAllforInvGrnForImport' */
    @Query(value = "SELECT Distinct gpi.Id, lc.LcOrderNo, imGdn.GatePassOutwardId, gpi.OtherSupCust AS OrderType, gpi.PurchaseOrderId, gpi.GpDate, "
         + "gpi.GpSrNo, gpi.VehicleNo, gpi.BiltyNo, gpi.VarietyName, gpi.NoOfPackages, gpi.FactoryWeight, gpi.DifferenceWeight, gpi.GatepassType, "
         + "gpi.SupplierContractCode, sp.CompanyName, gpi.VehicleType, gpi.Freight, gpi.Container, gpi.Container1, po.ExImLcOrderId, "
         + "wb.NetWbWeight as WbGrossWeight "
         + "FROM ExImLcOrderPurchaseOrder Po "
         + "INNER JOIN GatePassInward gpi ON Po.Id = gpi.PurchaseOrderId "
         + "INNER JOIN ImLcOrder lc ON po.ExImLcOrderId = lc.Id "
         + "LEFT OUTER JOIN ImForwarding AS imGdn ON gpi.Id = imGdn.GatePassOutwardId "
         + "LEFT OUTER JOIN SupplierCustomer AS sp ON po.SupplierCustomerId = sp.Id "
         + "inner join (Select ReferenceDocNoId, ReferenceDocTypeId, isnull(sum(NetWbWeight),0) NetWbWeight from WbTransactions "
         + "Group By ReferenceDocNoId, ReferenceDocTypeId) wb on gpi.Id = wb.ReferenceDocNoId and wb.ReferenceDocTypeId = 51 "
         + "where gpi.OrganizationId = :organizationId and gpi.CompanyId = :companyId and Po.Id = :orderId "
         + "and gpi.Status = 'Accepted' and gpi.GatePassType = 'Import'", nativeQuery = true)
    List<GatePassInwardImportGrnRow> gpNoAllforInvGrnForImport(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("orderId") Integer orderId);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='VehicleAndSupplierWeightCompare'.
     * The proc only assigns @FlagVehicle = 1 (SELECT into a variable) and returns NO result set; this method returns the number of
     * matching gate passes (> 0 is equivalent to @FlagVehicle = 1).
     */
    @Query(value = "SELECT COUNT(1) FROM GatePassInward WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND (:branchesId is null or BranchesId = :branchesId) and CAST(GpDate AS DATE) = CAST(:docDate AS DATE) "
         + "AND ((Upper(RTRIM(LTRIM(VehicleNo))) = Upper(RTRIM(LTRIM(:vehicleNo)))) OR (:supplierWeight > 0 AND SupplierWeight = :supplierWeight))",
         nativeQuery = true)
    Long vehicleAndSupplierWeightCompare(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId, @Param("docDate") LocalDate docDate, @Param("vehicleNo") String vehicleNo,
            @Param("supplierWeight") Double supplierWeight);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetGpDataForFreightVoucher' (configs 110 / 49 inlined as a cfg CTE). */
    @Query(value = "WITH cfg AS (SELECT CONVERT(float, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 110)) AS PartyAndFactoryWeightTolerance, "
         + "ISNULL(CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 49) AS INT),0) AS FreightAcId) "
         + "SELECT GP.Id, GP.GpDate, gp.GpSrNo, gp.VehicleNo, gp.BiltyNo, gp.Freight as BiltyFreight, wb.WbCharges, gp.SupplierFirstWeight, "
         + "gp.SupplierSecondWeight, gp.SupplierWeight, wb.NetWbWeight, ISNULL(cfg.PartyAndFactoryWeightTolerance,0) as ToleranceWeight, "
         + "CASE WHEN GP.SupplierWeight > (wb.NetWbWeight + ISNULL(cfg.PartyAndFactoryWeightTolerance,0)) THEN GP.SupplierWeight - wb.NetWbWeight ELSE 0 END AS ShortWeight, "
         + "CASE WHEN D.OrderRate > 0 AND GP.SupplierWeight > (wb.NetWbWeight + ISNULL(cfg.PartyAndFactoryWeightTolerance,0)) "
         + "THEN (GP.SupplierWeight - wb.NetWbWeight) / 40 * D.OrderRate ELSE 0 END ShortageAmount, "
         + "gp.Status, gp.OtherRemarks as Remarks, cfg.FreightAcId as DebitAccountId, c.AccountTitle, gp.CityId, City.Description as CityName, "
         + "sp.CompanyName as SupplierName, gp.VarietyName, gp.NoOfPackages as ItemQty "
         + "FROM GatePassInward gp CROSS JOIN cfg "
         + "inner join (SELECT ReferenceDocNoId, ReferenceDocTypeId, ISNULL(SUM(NetWbWeight),0) AS NetWbWeight, SUM(WbCharges) AS WbCharges "
         + "FROM WbTransactions WHERE FirstWeight > 0 AND SecondWeight > 0 GROUP BY ReferenceDocNoId, ReferenceDocTypeId) wb "
         + "on gp.Id = wb.ReferenceDocNoId and gp.DocumentTypeId = wb.ReferenceDocTypeId "
         + "inner join SupplierCustomer sp on gp.SupplierCustomerId = sp.Id "
         + "left join PurchaseOrder poh on gp.PurchaseOrderId = poh.Id AND RefDocumentTypeId = 41 "
         + "left join (select PurchaseOrderId, avg(OrderItemRate) as OrderRate from PurchaseOrderDetail Group By PurchaseOrderId) d on poh.Id = d.PurchaseOrderId "
         + "left join ChartofAccount c on cfg.FreightAcId = c.Id "
         + "left join City on gp.CityId = City.Id "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId AND (ISNULL(GP.Freight,0) > 0) and (ISNULL(gp.FreightOn,0) = 1) "
         + "AND (gp.Id not in (select GatePassId from InvFreightVoucher where OrganizationId = :organizationId and CompanyId = :companyId and GatePassId = gp.Id))",
         nativeQuery = true)
    List<GatePassInwardFreightVoucherRow> getGpDataForFreightVoucher(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetPandingGatePassforInvGrnForGatePurchase' (RefDocumentTypeId 110). */
    @Query(value = "SELECT DISTINCT gp.Id, gp.GpSrNo, gp.GpDate, SP.CompanyName AS SupplierName, gp.VehicleType, gp.VehicleNo, gp.BiltyNo, i.ItemName, "
         + "lab.Crop, gp.NoOfPackages as Qty, gp.SupplierFirstWeight, gp.SupplierSecondWeight, gp.SupplierWeight, wb.NetWeight as FactoryWeight, "
         + "gp.Status, gp.NetPaid "
         + "FROM GatePassInward gp with(nolock) "
         + "INNER JOIN GatePassInwardDetail d on gp.Id = d.GatePassInwardId "
         + "INNER JOIN SupplierCustomer SP ON gp.SupplierCustomerId = SP.Id "
         + "INNER JOIN InvLabAnalysisPurchaseHeader lab on gp.Id = lab.GatePassInwardId "
         + "INNER JOIN Item I ON lab.ItemId = I.Id "
         + "INNER JOIN (SELECT ReferenceDocNoId, sum(NetWbWeight) as NetWeight FROM WbTransactions where FirstWeight > 0 AND SecondWeight > 0 "
         + "AND ReferenceDocTypeId = 51 GROUP BY ReferenceDocNoId) wb on gp.Id = WB.ReferenceDocNoId "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and gp.FinancialYearId = :financialYearId AND (:branchesId is null or gp.BranchesId = :branchesId) "
         + "and gp.RefDocumentTypeId = 110 and (gp.Status <> 'Rejected') "
         + "and NOT EXISTS (select 1 from InvGrn WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND (:branchesId is null or BranchesId = :branchesId) AND InwardGatePassId = gp.Id)", nativeQuery = true)
    List<GatePassInwardPendingGatePurchaseRow> getPandingGatePassforInvGrnForGatePurchase(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("financialYearId") Integer financialYearId, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetPandingGatePassforInvGrnForGhallaMandiPurchase' (RefDocumentTypeId 111). */
    @Query(value = "SELECT DISTINCT gp.Id, gp.GpSrNo, gp.GpDate, gp.LoadingContractorId, SP.CompanyName AS LoadingContractor, gp.GhallaMandiId, gm.GhallaMandi, "
         + "gp.VehicleType, gp.VehicleNo, gp.BiltyNo, gp.OtherRemarks, gp.NoOfPackages, i.ItemName, d.ItemQty, wb.NetWeight as FactoryWeight, gp.NetPaid, "
         + "gp.CarriageContractorId, cc.CompanyName as CarriageContractor "
         + "FROM GatePassInward gp with(nolock) "
         + "INNER JOIN GatePassInwardDetail d on gp.Id = d.GatePassInwardId "
         + "INNER JOIN SupplierCustomer SP ON gp.LoadingContractorId = SP.Id "
         + "INNER JOIN Ghalla_Mandi gm on gp.GhallaMandiId = gm.Id "
         + "INNER JOIN Item I ON d.ItemId = I.Id "
         + "INNER JOIN (SELECT ReferenceDocNoId, sum(NetWbWeight) as NetWeight FROM WbTransactions where FirstWeight > 0 AND SecondWeight > 0 "
         + "AND ReferenceDocTypeId = 51 GROUP BY ReferenceDocNoId) wb on gp.Id = WB.ReferenceDocNoId "
         + "LEFT JOIN SupplierCustomer cc on gp.CarriageContractorId = cc.Id "
         + "where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and gp.DocumentTypeId = :documentTypeId "
         + "and gp.FinancialYearId = :financialYearId and gp.RefDocumentTypeId = 111 and (gp.Status <> 'Rejected') "
         + "and (gp.Id not in (select InwardGatePassId from InvGrn WHERE OrganizationId = :organizationId AND CompanyId = :companyId))", nativeQuery = true)
    List<GatePassInwardPendingGhallaMandiRow> getPandingGatePassforInvGrnForGhallaMandiPurchase(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId,
            @Param("financialYearId") Integer financialYearId);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetLabDataByGpIdForGrnGate' (returns rows only when config 101 = 1, like the proc's IF). */
    @Query(value = "select distinct h.Id, h.DocDate, h.DocNo, h.ItemId, i.ItemName, h.DeductionWeight, h.DeductionRate, h.ItemRate, h.AnalystName, "
         + "h.RemarksHeader, h.Crop, h.PoDetailId, "
         + "Case when ISNULL(h.IsApproved,0) = 1 and ISNULL(h.IsAcceptedApproved,0) = 0 then 'Rejected' else 'Accepted' end as LabStatus, "
         + "ISNULL(CONVERT(bit, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 101)),0) AS GatePurchaseLabConfigStatus, "
         + "EntryUser.UserTitle AS EntryUserName, wb.NetWeight "
         + "from InvLabAnalysisPurchaseHeader h "
         + "INNER JOIN GatePassInward gp on h.GatePassInwardId = gp.Id "
         + "INNER JOIN GatePassInwardDetail gpd on gp.Id = gpd.GatePassInwardId "
         + "INNER JOIN dbo.UserAccount EntryUser on EntryUser.ID = h.EntryUser "
         + "INNER JOIN Item i on h.ItemId = i.Id "
         + "INNER JOIN (SELECT ReferenceDocNoId, ItemId, ISNULL(sum(NetWbWeight),0) as NetWeight FROM WbTransactions where ReferenceDocTypeId = 51 "
         + "GROUP BY ReferenceDocNoId, ItemId) wb on gp.Id = WB.ReferenceDocNoId and h.ItemId = wb.ItemId "
         + "where h.OrganizationId = :organizationId and h.CompanyId = :companyId and h.GatePassInwardId = :id "
         + "and ISNULL(CONVERT(bit, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 101)),0) = 1", nativeQuery = true)
    List<GatePassInwardLabDataGrnGateRow> getLabDataByGpIdForGrnGate(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_GatePassInward_GetAllMethod @Activity='GetLastestUsedOrderType' (TOP 1 RefDocumentTypeId ORDER BY GpSrNo DESC; null when none). */
    @Query(value = "SELECT TOP 1 RefDocumentTypeId FROM GatePassInward WHERE CompanyId = :companyId And OrganizationId = :organizationId "
         + "AND (:branchesId is null or GatePassInward.BranchesId = :branchesId) And OtherSupCust IS NOT NULL ORDER BY GpSrNo DESC", nativeQuery = true)
    Integer getLastestUsedOrderType(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
            @Param("branchesId") Integer branchesId);

    /**
     * Sp_GatePassInward_GetAllMethod @Activity='GetAccessWeightByWeightComparedToPoWtAndGpId' (@WeightComparedToPoWt default 0).
     * Returns one row only when @RefDocumentTypeId = 41 and @GatepassType <> 'Import' (otherwise empty, like the proc);
     * AccessWeight = WeightComparedToPoWt - BalQty when the weight exceeds the PO balance, else 0.
     */
    @Query(value = "SELECT CASE WHEN :weightComparedToPoWt > b.BalQty THEN :weightComparedToPoWt - b.BalQty ELSE 0 END AS AccessWeight, b.BalQty AS BalQtyOfPo "
         + "FROM (SELECT [dbo].[GetPurchaseOrderandGatepassQtyBalance](:organizationId, :companyId, :purchaseOrderId, null, 1, :weightComparedToPoWt) AS BalQty) b "
         + "WHERE :refDocumentTypeId = 41 and :gatepassType <> 'Import'", nativeQuery = true)
    List<GatePassInwardAccessWeightRow> getAccessWeightByWeightComparedToPoWtAndGpId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("purchaseOrderId") Integer purchaseOrderId,
            @Param("weightComparedToPoWt") Double weightComparedToPoWt, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("gatepassType") String gatepassType);
}
