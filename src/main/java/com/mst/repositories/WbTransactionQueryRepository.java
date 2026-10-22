package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.projections.DefineVehicleWeightRow;
import com.mst.projections.WbPartyRow;
import com.mst.projections.WbPendingTicketsForContractorWagesRow;
import com.mst.projections.WbRejectedTicketRow;
import com.mst.projections.WbNetWeightRow;
import com.mst.projections.WbTicketByRefDocRow;
import com.mst.projections.WbTransactionByGatePassRow;
import com.mst.projections.WbTransactionByGatePassStRow;
import com.mst.projections.WbTransactionFormHistoryRow;
import com.mst.projections.WbTransactionReadByIdRow;
import com.mst.projections.WbTransactionReadHistoryRow;

/**
 * Sp_WbTransation_GetAllMethod — one method per {@code @Activity} branch. dbo.WbTransactions has no entity in the
 * project, so every branch is a native query (the SELECTs are pasted verbatim; temp tables became CTEs).
 * Bound to {@link com.mst.models.InwardGatePass} only to satisfy Spring Data (query-only repository).
 */
@Repository
public interface WbTransactionQueryRepository
        extends org.springframework.data.repository.Repository<com.mst.models.InwardGatePass, Integer> {

    /** Sp_WbTransation_GetAllMethod @Activity='GenerateDocNo' — next TicketNo (ISNULL(MAX)+1). */
    @Query(value = "select ISNULL(Max(convert(int,TicketNo)),0) + 1 As TicketNo "
         + "from WbTransactions with(nolock) "
         + "Where OrganizationId = :organizationId "
         + "And CompanyId = :companyId "
         + "and FinancialYearId = :financialYearId "
         + "and BranchId = :branchId", nativeQuery = true)
    Integer generateDocNo(@Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("financialYearId") Integer financialYearId,
                          @Param("branchId") Integer branchId);

    /**
     * Sp_WbTransation_GetAllMethod @Activity='FormHistory'. Temp tables {@code #TempGP} / {@code #tmHis} rewritten as CTEs;
     * the proc's {@code @MultiWeighBridgeaginstSingleBranch} (config 162, forced to 0 when documentTypeId is 113 or null)
     * is inlined as CTE {@code Cfg}. Note the ROW_NUMBER() restarts in each UNION ALL leg exactly as in the proc.
     * All filter parameters default to null; canViewAllRecord is the proc's bit parameter.
     */
    @Query(value = "WITH TempGP AS ( "
         + "  select a.DocumentTypeId, a.Id, a.GpSrNo, a.NoOfPackages, a.SupplierCustomerId "
         + "  from ( "
         + "    Select DocumentTypeId, Id, GpSrNo, NoOfPackages, SupplierCustomerId from GatePassInward group by DocumentTypeId, Id, GpSrNo, NoOfPackages, SupplierCustomerId "
         + "    Union All "
         + "    select DocumentTypeId, id, GpSrNo, NoOfPackages, SupplierCustomerId from GatePassOutward group by DocumentTypeId, Id, GpSrNo, NoOfPackages, SupplierCustomerId "
         + "    Union All "
         + "    Select DocumentTypeId, Id, GpSrNo, NoOfPackages, SupplierCustomerId from GatePassGeneral group by DocumentTypeId, Id, GpSrNo, NoOfPackages, SupplierCustomerId "
         + "  ) A Group By a.DocumentTypeId, a.Id, a.GpSrNo, a.NoOfPackages, a.SupplierCustomerId "
         + "), Cfg AS ( "
         + "  SELECT CASE WHEN :documentTypeId = 113 OR :documentTypeId IS NULL THEN CAST(0 AS BIT) "
         + "              ELSE CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 162) AS BIT) END AS MultiWeighBridgeaginstSingleBranch "
         + "), tmHis AS ( "
         + "    SELECT ROW_NUMBER() OVER (ORDER BY wb.DocDate DESC) RecordNo, wb.Id, "
         + "           wb.DocDate, dct.DocumentTypeDescription, "
         + "           wb.WeighBridgeType, wb.ReferenceDocNoId, Gp.GpSrNo, wb.TicketNo, wb.VehicleNo, "
         + "           wb.BiltyNo, wb.ItemDescription, wb.ReferenceDocTypeId, "
         + "           CASE WHEN ISNULL(wb.ItemQty, 0) > 0 THEN ISNULL(wb.ItemQty, 0) ELSE ISNULL(GP.NoOfPackages, 0) END AS ItemQty, "
         + "           wb.WbCharges, wb.WorkingReportNo, "
         + "           wb.WbRemarks, wb.EntryDate, wb.PackingType, Eusr.FirstName AS UserName, "
         + "           wb.FirstWeight, Case when wb.FirstWtWbStatus = 1 then 'A' when wb.FirstWtWbStatus = 2 then 'M' else '' End FirstWtWbStatus, "
         + "           wb.SecondWeight, Case when wb.SecondWtWbStatus = 1 then 'A' when wb.SecondWtWbStatus = 2 then 'M' else '' End SecondWtWbStatus, "
         + "           wb.NetWbWeight, "
         + "           CASE WHEN sp.CompanyName IS NULL THEN wb.PartyName ELSE sp.CompanyName END AS PartyName, "
         + "           wb.PackingUnitWeight, "
         + "           wb.PackingTotalWeight, "
         + "           ABS(wb.WeightAfterPackingWeight) AS WeightAfterPackingWeight, "
         + "           wb.LoadWeight, "
         + "           wb.TearWeight, "
         + "           wb.SupplierWbNetWeight "
         + "    FROM WbTransactions wb "
         + "    INNER JOIN dbo.DocumentType AS dct ON wb.ReferenceDocTypeId = dct.Id "
         + "    INNER JOIN dbo.UserAccount AS Eusr ON wb.EntryUser = Eusr.ID "
         + "    LEFT JOIN TempGP AS Gp ON wb.ReferenceDocTypeId = Gp.DocumentTypeId AND wb.ReferenceDocNoId = Gp.Id "
         + "    LEFT JOIN dbo.SupplierCustomer AS sp ON Gp.SupplierCustomerId = sp.Id "
         + "    CROSS JOIN Cfg cfg "
         + "    WHERE wb.OrganizationId = :organizationId "
         + "      AND wb.CompanyId = :companyId "
         + "      AND Wb.ReferenceDocTypeId <> 1601 "
         + "      AND (:documentTypeId is null or WB.DocTypeId = :documentTypeId) "
         + "      AND wb.FinancialYearId = :financialYearId "
         + "      AND wb.BranchId = :branchId "
         + "      AND (:canViewAllRecord = 1 OR (:canViewAllRecord = 0 AND wb.EntryUser = :entryUser)) "
         + "      AND ( "
         + "            (ISNULL(cfg.MultiWeighBridgeaginstSingleBranch, 0) = 1 AND wb.FirstWeighBridgeId = :firstWeighBridgeId) "
         + "            OR "
         + "            (ISNULL(cfg.MultiWeighBridgeaginstSingleBranch, 0) = 0) "
         + "          ) "
         + "      and (:dateFrom is null or CAST(wb.DocDate as date) >= cast(:dateFrom as date)) "
         + "      and (:dateTo is null or CAST(wb.DocDate as date) <= cast(:dateTo as date)) "
         + "      and (:entryFromDate is null or CAST(wb.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + "      and (:entryToDate is null or CAST(wb.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + "      and (:modifyFromDate is null or CAST(wb.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + "      and (:modifyToDate is null or CAST(wb.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + "      and (:approvedFromDate is null or CAST(wb.ApprovedDateTime AS DATE) >= CAST(:approvedFromDate AS DATE)) "
         + "      and (:approvedToDate is null or CAST(wb.ApprovedDateTime AS DATE) <= CAST(:approvedToDate AS DATE)) "
         + "      and (:ticketNoFrom is null or wb.TicketNo >= :ticketNoFrom) "
         + "      and (:ticketNoTo is null or wb.TicketNo <= :ticketNoTo) "
         + "      and (:gpNoFrom is null or gp.GpSrNo >= :gpNoFrom) "
         + "      and (:gpNoTo is null or gp.GpSrNo <= :gpNoTo) "
         + "      and (:vehicleNo is null or wb.VehicleNo = :vehicleNo) "
         + "      and (:financialYearId is null or wb.FinancialYearId = :financialYearId) "
         + "      and (:refDocumentTypeId is null or wb.ReferenceDocTypeId = :refDocumentTypeId) "
         + "      and (:weighBridgeType is null or wb.WeighBridgeType = :weighBridgeType) "
         + "    UNION ALL "
         + "    SELECT ROW_NUMBER() OVER (ORDER BY wb.DocDate DESC) RecordNo, wb.Id, wb.DocDate, dct.DocumentTypeDescription, wb.WeighBridgeType, wb.ReferenceDocNoId, Gp.GpSrNo, wb.TicketNo, wb.VehicleNo, "
         + "           wb.BiltyNo, wb.ItemDescription, wb.ReferenceDocTypeId, "
         + "           CASE WHEN ISNULL(wb.ItemQty, 0) > 0 THEN ISNULL(wb.ItemQty, 0) ELSE ISNULL(GP.NoOfPackages, 0) END AS ItemQty, "
         + "           wb.WbCharges, wb.WorkingReportNo, "
         + "           wb.WbRemarks, wb.EntryDate, wb.PackingType, Eusr.FirstName AS UserName, "
         + "           wb.FirstWeight, Case when wb.FirstWtWbStatus = 1 then 'A' when wb.FirstWtWbStatus = 2 then 'M' else '' End FirstWtWbStatus, "
         + "           wb.SecondWeight, Case when wb.SecondWtWbStatus = 1 then 'A' when wb.SecondWtWbStatus = 2 then 'M' else '' End SecondWtWbStatus, "
         + "           wb.NetWbWeight, "
         + "           CASE WHEN sp.CompanyName IS NULL THEN wb.PartyName ELSE sp.CompanyName END AS PartyName, "
         + "           wb.PackingUnitWeight, "
         + "           wb.PackingTotalWeight, "
         + "           ABS(wb.WeightAfterPackingWeight) AS WeightAfterPackingWeight, "
         + "           wb.LoadWeight, "
         + "           wb.TearWeight, "
         + "           wb.SupplierWbNetWeight "
         + "    FROM WbTransactions wb "
         + "    INNER JOIN dbo.DocumentType AS dct ON wb.ReferenceDocTypeId = dct.Id "
         + "    INNER JOIN dbo.UserAccount AS Eusr ON wb.EntryUser = Eusr.ID "
         + "    LEFT JOIN TempGP AS Gp ON wb.ReferenceDocTypeId = Gp.DocumentTypeId AND wb.ReferenceDocNoId = Gp.Id "
         + "    LEFT JOIN dbo.SupplierCustomer AS sp ON Gp.SupplierCustomerId = sp.Id "
         + "    CROSS JOIN Cfg cfg "
         + "    WHERE wb.OrganizationId = :organizationId "
         + "      AND wb.CompanyId = :companyId "
         + "      AND Wb.ReferenceDocTypeId = 1601 "
         + "      AND wb.FinancialYearId = :financialYearId "
         + "      AND wb.BranchId = :branchId "
         + "      AND (:canViewAllRecord = 1 OR (:canViewAllRecord = 0 AND wb.EntryUser = :entryUser)) "
         + "      AND ( "
         + "            (ISNULL(cfg.MultiWeighBridgeaginstSingleBranch, 0) = 1 AND wb.FirstWeighBridgeId = :firstWeighBridgeId) "
         + "            OR "
         + "            (ISNULL(cfg.MultiWeighBridgeaginstSingleBranch, 0) = 0) "
         + "          ) "
         + "      and (:dateFrom is null or CAST(wb.DocDate as date) >= cast(:dateFrom as date)) "
         + "      and (:dateTo is null or CAST(wb.DocDate as date) <= cast(:dateTo as date)) "
         + "      and (:entryFromDate is null or CAST(wb.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + "      and (:entryToDate is null or CAST(wb.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + "      and (:modifyFromDate is null or CAST(wb.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + "      and (:modifyToDate is null or CAST(wb.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + "      and (:approvedFromDate is null or CAST(wb.ApprovedDateTime AS DATE) >= CAST(:approvedFromDate AS DATE)) "
         + "      and (:approvedToDate is null or CAST(wb.ApprovedDateTime AS DATE) <= CAST(:approvedToDate AS DATE)) "
         + "      and (:ticketNoFrom is null or wb.TicketNo >= :ticketNoFrom) "
         + "      and (:ticketNoTo is null or wb.TicketNo <= :ticketNoTo) "
         + "      and (:gpNoFrom is null or gp.GpSrNo >= :gpNoFrom) "
         + "      and (:gpNoTo is null or gp.GpSrNo <= :gpNoTo) "
         + "      and (:vehicleNo is null or wb.VehicleNo = :vehicleNo) "
         + "      and (:financialYearId is null or wb.FinancialYearId = :financialYearId) "
         + "      and (:refDocumentTypeId is null or wb.ReferenceDocTypeId = :refDocumentTypeId) "
         + "      and (:weighBridgeType is null or wb.WeighBridgeType = :weighBridgeType) "
         + ") "
         + "select distinct * "
         + "from tmHis "
         + "Where (:noOfRecords is null or RecordNo <= :noOfRecords) "
         + "order by TicketNo DESC", nativeQuery = true)
    List<WbTransactionFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId,
                                                  @Param("companyId") Integer companyId,
                                                  @Param("documentTypeId") Integer documentTypeId,
                                                  @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                  @Param("financialYearId") Integer financialYearId,
                                                  @Param("branchId") Integer branchId,
                                                  @Param("canViewAllRecord") Boolean canViewAllRecord,
                                                  @Param("entryUser") Integer entryUser,
                                                  @Param("firstWeighBridgeId") Integer firstWeighBridgeId,
                                                  @Param("dateFrom") LocalDate dateFrom,
                                                  @Param("dateTo") LocalDate dateTo,
                                                  @Param("entryFromDate") LocalDate entryFromDate,
                                                  @Param("entryToDate") LocalDate entryToDate,
                                                  @Param("modifyFromDate") LocalDate modifyFromDate,
                                                  @Param("modifyToDate") LocalDate modifyToDate,
                                                  @Param("approvedFromDate") LocalDate approvedFromDate,
                                                  @Param("approvedToDate") LocalDate approvedToDate,
                                                  @Param("ticketNoFrom") Integer ticketNoFrom,
                                                  @Param("ticketNoTo") Integer ticketNoTo,
                                                  @Param("gpNoFrom") Integer gpNoFrom,
                                                  @Param("gpNoTo") Integer gpNoTo,
                                                  @Param("vehicleNo") String vehicleNo,
                                                  @Param("weighBridgeType") String weighBridgeType,
                                                  @Param("noOfRecords") Integer noOfRecords);

    /**
     * Sp_WbTransation_GetAllMethod @Activity='ReadById'. Temp table {@code #TempDocs} (union of GatePassInward /
     * GatePassOutward / GatePassGeneral / GatePassPartyProcessing) rewritten as a CTE.
     */
    @Query(value = "WITH TempDocs AS ( "
         + "  select distinct a.DocumentTypeId, a.Id, a.GpSrNo, a.RefDocumentTypeId, a.GatepassType, A.OtherSupCust, a.OrderId "
         + "  from ( "
         + "    Select distinct DocumentTypeId, Id, GpSrNo, RefDocumentTypeId, GatepassType, OtherSupCust, PurchaseOrderId as OrderId from GatePassInward "
         + "    Union All "
         + "    select distinct DocumentTypeId, id, GpSrNo, RefDocumentTypeId, GatepassType, OtherSupCust, SaleOrderId from GatePassOutward "
         + "    Union All "
         + "    select distinct DocumentTypeId, id, GpSrNo, RefDocumentTypeId, GatepassType, OtherSupCust, PurchaseOrderId from GatePassGeneral "
         + "    Union All "
         + "    select distinct DocumentTypeId, id, GpSrNo, 0, GatepassType, '', 0 from dbo.GatePassPartyProcessing "
         + "  ) A "
         + ") "
         + "SELECT wb.Id, wb.EntryNo, wb.ReferenceDocTypeId, wb.ReferenceDocNoId, "
         + "       wb.WeighBridgeType, wb.TicketNo, wb.VehicleNo, wb.VehicleType, "
         + "       wb.BiltyNo, wb.SupplierCustomerId, wb.PartyName, wb.ItemDescription, "
         + "       wb.ItemQty, wb.WbCharges, wb.WbRemarks, wb.EntryDate, wb.EntryUser, "
         + "       wb.ModifyDate, wb.ModifyUser, wb.IsApproved, wb.ApprovedUserId, "
         + "       wb.ApprovedDateTime, wb.OrganizationId, wb.CompanyId, "
         + "       DocumentType.DocumentTypeDescription, wb.DocTypeId, wb.DocDate, "
         + "       wb.FirstWeight, wb.FirstDateTime, wb.FirstWtUpperPic, wb.FirstWtPicReading, "
         + "       wb.FirstWtFrontPic, wb.SecondWeight, wb.SecondDateTime, wb.SecondPicReading, "
         + "       wb.SecondWtUpperPic, wb.SecondWtFrontPic, wb.ContainerNo, wb.ActionId, "
         + "       wb.WorkingReportNo, wb.WeightDiffComments, wb.NetWbWeight, wb.ScaleKart, "
         + "       wb.ItemId, i.ItemName, TempDocs.GpSrNo, TempDocs.RefDocumentTypeId, "
         + "       wb.PackingType, wb.FirstWtWbStatus, wb.SecondWtWbStatus, "
         + "       WB.InvoiceId, "
         + "       wb.PackingUnitWeight, "
         + "       wb.PackingTotalWeight, "
         + "       wb.WeightAfterPackingWeight, "
         + "       wb.LoadWeight, "
         + "       wb.TearWeight, "
         + "       wb.SupplierWbNetWeight, "
         + "       TempDocs.GatepassType, "
         + "       TempDocs.OtherSupCust, "
         + "       TempDocs.OrderId "
         + "FROM WbTransactions wb with(nolock) "
         + "INNER JOIN DocumentType with(nolock) ON wb.ReferenceDocTypeId = DocumentType.Id "
         + "LEFT JOIN TempDocs on wb.ReferenceDocTypeId = TempDocs.DocumentTypeId and wb.ReferenceDocNoId = TempDocs.Id "
         + "LEFT JOIN Item i on wb.ItemId = i.Id "
         + "WHERE wb.Id = :id", nativeQuery = true)
    List<WbTransactionReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_WbTransation_GetAllMethod @Activity='GetIdbyTicketNo'. Defaults: financialYearId = null, branchId = null. */
    @Query(value = "select Id "
         + "from WbTransactions "
         + "where TicketNo = :ticketNo "
         + "and OrganizationId = :organizationId "
         + "and CompanyId = :companyId "
         + "and (:financialYearId is null or FinancialYearId = :financialYearId) "
         + "and (:branchId IS NULL OR BranchId = :branchId)", nativeQuery = true)
    List<Integer> getIdByTicketNo(@Param("ticketNo") Integer ticketNo,
                                  @Param("organizationId") Integer organizationId,
                                  @Param("companyId") Integer companyId,
                                  @Param("financialYearId") Integer financialYearId,
                                  @Param("branchId") Integer branchId);

    /**
     * Sp_WbTransation_GetAllMethod @Activity='GetVehicleWeight'. Kept verbatim: the proc compares
     * {@code vw.CompanyId = @OrganizationId} (not @CompanyId). Default: financialYearId = null.
     */
    @Query(value = "select vw.Id, vw.VehicleType, "
         + "       vw.VehicleNo, vw.VehicleNetWeight "
         + "from DefineVehicleWeight vw "
         + "where vw.OrganizationId = :organizationId "
         + "and vw.CompanyId = :organizationId "
         + "and (:financialYearId is null or FinancialYearId = :financialYearId)", nativeQuery = true)
    List<DefineVehicleWeightRow> getVehicleWeight(@Param("organizationId") Integer organizationId,
                                                  @Param("financialYearId") Integer financialYearId);

    /** Sp_WbTransation_GetAllMethod @Activity='GetTicketNoByRefDocId', @DocumentTypeId = 51 (Gate Pass Inward). Default: branchId = null. */
    @Query(value = "SELECT wb.Id, wb.TicketNo, cbh.RefDocNoId "
         + "FROM dbo.WbTransactions AS wb "
         + "INNER JOIN dbo.GatePassInward AS gp ON wb.ReferenceDocTypeId = gp.DocumentTypeId AND wb.ReferenceDocNoId = gp.Id "
         + "LEFT JOIN dbo.InvContractorWagesBillHeader AS cbh ON wb.ReferenceDocTypeId = cbh.RefDocumentTypeId AND wb.ReferenceDocNoId = cbh.RefDocNoId "
         + "where wb.FirstWeight > 0 "
         + "and wb.SecondWeight > 0 "
         + "and wb.ReferenceDocNoId = :referenceDocNoId "
         + "and wb.ReferenceDocTypeId = :documentTypeId "
         + "and (:branchId IS NULL OR wb.BranchId = :branchId) "
         + "GROUP BY wb.Id, wb.TicketNo, cbh.RefDocNoId "
         + "HAVING (cbh.RefDocNoId IS NULL)", nativeQuery = true)
    List<WbTicketByRefDocRow> getTicketNoByRefDocIdGpIn(@Param("referenceDocNoId") Integer referenceDocNoId,
                                                        @Param("documentTypeId") Integer documentTypeId,
                                                        @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='GetTicketNoByRefDocId', @DocumentTypeId = 91 (Gate Pass Outward). Default: branchId = null. */
    @Query(value = "SELECT wb.Id, wb.TicketNo, cbh.RefDocNoId "
         + "FROM dbo.WbTransactions AS wb "
         + "INNER JOIN dbo.GatePassOutward AS gp ON wb.ReferenceDocTypeId = gp.DocumentTypeId AND wb.ReferenceDocNoId = gp.Id "
         + "LEFT JOIN dbo.InvContractorWagesBillHeader AS cbh ON wb.ReferenceDocTypeId = cbh.RefDocumentTypeId AND wb.ReferenceDocNoId = cbh.RefDocNoId "
         + "where wb.FirstWeight > 0 and wb.SecondWeight > 0 "
         + "and wb.ReferenceDocNoId = :referenceDocNoId "
         + "and wb.ReferenceDocTypeId = :documentTypeId "
         + "and (:branchId IS NULL OR wb.BranchId = :branchId) "
         + "GROUP BY wb.Id, wb.TicketNo, cbh.RefDocNoId "
         + "HAVING (cbh.RefDocNoId IS NULL)", nativeQuery = true)
    List<WbTicketByRefDocRow> getTicketNoByRefDocIdGpOut(@Param("referenceDocNoId") Integer referenceDocNoId,
                                                         @Param("documentTypeId") Integer documentTypeId,
                                                         @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='GetTicketNoByRefDocId', @DocumentTypeId = 74 (Stock Transfer, no gate pass join). Default: branchId = null. */
    @Query(value = "SELECT wb.Id, wb.TicketNo, cbh.RefDocNoId "
         + "FROM dbo.WbTransactions AS wb "
         + "LEFT JOIN dbo.InvContractorWagesBillHeader AS cbh ON wb.ReferenceDocTypeId = cbh.RefDocumentTypeId "
         + "     AND wb.ReferenceDocNoId = cbh.RefDocNoId "
         + "where wb.FirstWeight > 0 and wb.SecondWeight > 0 "
         + "and wb.ReferenceDocNoId = :referenceDocNoId "
         + "and wb.ReferenceDocTypeId = :documentTypeId "
         + "and (:branchId IS NULL OR wb.BranchId = :branchId) "
         + "GROUP BY wb.Id, wb.TicketNo, cbh.RefDocNoId "
         + "HAVING (cbh.RefDocNoId IS NULL)", nativeQuery = true)
    List<WbTicketByRefDocRow> getTicketNoByRefDocIdSt(@Param("referenceDocNoId") Integer referenceDocNoId,
                                                      @Param("documentTypeId") Integer documentTypeId,
                                                      @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='GetWbTransactionByGatePassId', @RefDocumentTypeId = 51 (Gate Pass Inward). Default: branchId = null. */
    @Query(value = "SELECT wbt.Id, wbt.TicketNo, gp.GpSrNo, wbt.FirstWeight, "
         + "       wbt.SecondWeight, wbt.EntryDate, usr.FirstName, "
         + "       gp.VehicleNo, gp.OtherRemarks, wbt.ItemQty "
         + "FROM WbTransactions AS wbt "
         + "INNER JOIN GatePassInward AS gp ON wbt.ReferenceDocTypeId = gp.DocumentTypeId AND wbt.ReferenceDocNoId = gp.Id "
         + "LEFT JOIN UserAccount AS usr ON wbt.EntryUser = usr.ID "
         + "where wbt.ReferenceDocNoId = :referenceDocNoId "
         + "and wbt.ReferenceDocTypeId = :refDocumentTypeId "
         + "and (:branchId IS NULL OR wbt.BranchId = :branchId) "
         + "GROUP BY wbt.TicketNo, gp.GpSrNo, wbt.FirstWeight, wbt.SecondWeight, wbt.EntryDate, usr.FirstName, wbt.Id, gp.VehicleNo, gp.OtherRemarks, wbt.ItemQty",
           nativeQuery = true)
    List<WbTransactionByGatePassRow> getWbTransactionByGatePassIdGpIn(@Param("referenceDocNoId") Integer referenceDocNoId,
                                                                      @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                                      @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='GetWbTransactionByGatePassId', @RefDocumentTypeId = 91 (Gate Pass Outward; no BranchId filter in the proc). */
    @Query(value = "SELECT wbt.Id, wbt.TicketNo, gp.GpSrNo, wbt.FirstWeight, "
         + "       wbt.SecondWeight, wbt.EntryDate, usr.FirstName, "
         + "       gp.VehicleNo, gp.OtherRemarks, wbt.ItemQty "
         + "FROM dbo.WbTransactions AS wbt "
         + "INNER JOIN dbo.GatePassOutward AS gp ON wbt.ReferenceDocTypeId = gp.DocumentTypeId AND wbt.ReferenceDocNoId = gp.Id "
         + "LEFT JOIN dbo.UserAccount AS usr ON wbt.EntryUser = usr.ID "
         + "where wbt.ReferenceDocNoId = :referenceDocNoId "
         + "and wbt.ReferenceDocTypeId = :refDocumentTypeId "
         + "GROUP BY wbt.TicketNo, gp.GpSrNo, wbt.FirstWeight, wbt.SecondWeight, "
         + "         wbt.EntryDate, usr.FirstName, wbt.Id, gp.VehicleNo, "
         + "         gp.OtherRemarks, wbt.ItemQty", nativeQuery = true)
    List<WbTransactionByGatePassRow> getWbTransactionByGatePassIdGpOut(@Param("referenceDocNoId") Integer referenceDocNoId,
                                                                       @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /**
     * Sp_WbTransation_GetAllMethod @Activity='GetWbTransactionByGatePassId', @RefDocumentTypeId = 74 (Stock Transfer).
     * Kept verbatim: the proc selects {@code wbt.TicketNo} twice and returns WbRemarks instead of OtherRemarks.
     * Default: branchId = null.
     */
    @Query(value = "SELECT wbt.Id, wbt.TicketNo, wbt.TicketNo, wbt.FirstWeight, wbt.SecondWeight, "
         + "       wbt.EntryDate, usr.FirstName, wbt.VehicleNo, wbt.WbRemarks, wbt.ItemQty "
         + "FROM dbo.WbTransactions AS wbt "
         + "LEFT JOIN dbo.UserAccount AS usr ON wbt.EntryUser = usr.ID "
         + "where wbt.ReferenceDocNoId = :referenceDocNoId "
         + "and wbt.ReferenceDocTypeId = :refDocumentTypeId "
         + "and (:branchId IS NULL OR wbt.BranchId = :branchId) "
         + "GROUP BY wbt.TicketNo, wbt.WbRemarks, wbt.FirstWeight, "
         + "         wbt.SecondWeight, wbt.EntryDate, usr.FirstName, "
         + "         wbt.Id, wbt.VehicleNo, wbt.WbRemarks, wbt.ItemQty", nativeQuery = true)
    List<WbTransactionByGatePassStRow> getWbTransactionByGatePassIdSt(@Param("referenceDocNoId") Integer referenceDocNoId,
                                                                      @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                                      @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='PendingTicketsforContratorWages' (UNION of GP-In / GP-Out / Stock Transfer). Default: financialYearId = null. */
    @Query(value = "SELECT A.TranType, A.ReferenceDocTypeId, A.ReferenceDocNoId, A.Id, A.GpSrNo, A.GpDate, A.VehicleNo, A.NoOfPackages, A.FirstWeight, A.SecondWeight, A.TicketNo, A.ScaleSlipNo, A.SupplierCustomerId, A.CompanyName, A.OtherRemarks, A.VarietyName From ( "
         + "SELECT 'Gate Pass Inward' As TranType, wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.Id, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, wbt.FirstWeight, wbt.SecondWeight, wbt.TicketNo, cbh.WbTransactionsIdDt as ScaleSlipNo, gp.SupplierCustomerId, sp.CompanyName, gp.OtherRemarks, gp.VarietyName "
         + "FROM dbo.GatePassInward AS gp INNER JOIN dbo.WbTransactions AS wbt ON gp.Id = wbt.ReferenceDocNoId AND gp.DocumentTypeId = wbt.ReferenceDocTypeId LEFT OUTER JOIN dbo.SupplierCustomer AS sp ON gp.SupplierCustomerId = sp.Id LEFT OUTER JOIN dbo.InvContractorWagesBillDetail AS cbh ON wbt.Id = cbh.WbTransactionsIdDt "
         + "Where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and wbt.FirstWeight > 0 and wbt.SecondWeight > 0 "
         + "      and (wbt.ReferenceDocTypeId = :refDocumentTypeIdGpIn) and (:financialYearId is null or wbt.FinancialYearId = :financialYearId) "
         + "GROUP BY wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.Id, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, wbt.FirstWeight, wbt.SecondWeight, wbt.TicketNo, cbh.WbTransactionsIdDt, gp.SupplierCustomerId, sp.CompanyName, gp.OtherRemarks, gp.VarietyName "
         + "HAVING (cbh.WbTransactionsIdDt IS NULL) "
         + "Union All "
         + "SELECT 'Gate Pass Outware' As TranType, wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.Id, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, wbt.FirstWeight, wbt.SecondWeight, wbt.TicketNo, cbh.WbTransactionsIdDt as ScaleSlipNo, gp.SupplierCustomerId, sp.CompanyName, gp.OtherRemarks, gp.VarietyName "
         + "FROM dbo.GatePassOutward AS gp INNER JOIN dbo.WbTransactions AS wbt ON gp.Id = wbt.ReferenceDocNoId AND gp.DocumentTypeId = wbt.ReferenceDocTypeId LEFT OUTER JOIN dbo.SupplierCustomer AS sp ON gp.SupplierCustomerId = sp.Id LEFT OUTER JOIN dbo.InvContractorWagesBillDetail AS cbh ON wbt.Id = cbh.WbTransactionsIdDt "
         + "Where gp.OrganizationId = :organizationId and gp.CompanyId = :companyId and wbt.FirstWeight > 0 and wbt.SecondWeight > 0 "
         + "      and (wbt.ReferenceDocTypeId = :refDocumentTypeIdGpOut) and (:financialYearId is null or wbt.FinancialYearId = :financialYearId) "
         + "GROUP BY wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, gp.Id, gp.GpSrNo, gp.GpDate, gp.VehicleNo, gp.NoOfPackages, wbt.FirstWeight, wbt.SecondWeight, wbt.TicketNo, cbh.WbTransactionsIdDt, gp.SupplierCustomerId, sp.CompanyName, gp.OtherRemarks, gp.VarietyName "
         + "HAVING (cbh.WbTransactionsIdDt IS NULL) "
         + "Union All "
         + "SELECT 'Stock Transfer', wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, wbt.id, wbt.TicketNo, wbt.DocDate, wbt.VehicleNo, wbt.ItemQty, wbt.FirstWeight, wbt.SecondWeight, wbt.TicketNo, cbh.WbTransactionsIdDt as ScaleSlipNo, 0, 'Stock Transfer', wbt.WbRemarks, wbt.ItemDescription "
         + "FROM dbo.WbTransactions AS wbt LEFT OUTER JOIN dbo.InvContractorWagesBillDetail AS cbh ON wbt.Id = cbh.WbTransactionsIdDt "
         + "Where wbt.OrganizationId = :organizationId and wbt.CompanyId = :companyId and wbt.FirstWeight > 0 and wbt.SecondWeight > 0 "
         + "      and (wbt.ReferenceDocTypeId = :refDocumentTypeIdSt) and (:financialYearId is null or wbt.FinancialYearId = :financialYearId) "
         + "GROUP BY wbt.ReferenceDocTypeId, wbt.ReferenceDocNoId, wbt.FirstWeight, wbt.SecondWeight, cbh.WbTransactionsIdDt, wbt.id, wbt.TicketNo, wbt.DocDate, wbt.VehicleNo, wbt.ItemQty, wbt.WbRemarks, wbt.ItemDescription "
         + "HAVING (cbh.WbTransactionsIdDt IS NULL) "
         + ") A Group By A.TranType, A.ReferenceDocTypeId, A.ReferenceDocNoId, A.Id, A.GpSrNo, A.GpDate, A.VehicleNo, A.NoOfPackages, A.FirstWeight, A.SecondWeight, A.TicketNo, A.ScaleSlipNo, A.SupplierCustomerId, A.CompanyName, A.OtherRemarks, A.VarietyName "
         + "ORDER BY A.ReferenceDocTypeId", nativeQuery = true)
    List<WbPendingTicketsForContractorWagesRow> pendingTicketsForContractorWages(@Param("organizationId") Integer organizationId,
                                                                                @Param("companyId") Integer companyId,
                                                                                @Param("refDocumentTypeIdGpIn") Integer refDocumentTypeIdGpIn,
                                                                                @Param("refDocumentTypeIdGpOut") Integer refDocumentTypeIdGpOut,
                                                                                @Param("refDocumentTypeIdSt") Integer refDocumentTypeIdSt,
                                                                                @Param("financialYearId") Integer financialYearId);

    /**
     * Sp_WbTransation_GetAllMethod @Activity='ReadHistory'. Temp table {@code #MyDocs} rewritten as a CTE;
     * {@code dbo.fnSplitString} kept for the comma-separated refDocumentTypeIds. All filters default to null.
     */
    @Query(value = "WITH MyDocs AS ( "
         + "  select a.DocumentTypeId, a.Id, a.GpSrNo, a.SupplierWeight, a.SupplierContractCode, a.SupplierCustomerId, a.NoOfPackages "
         + "  from ( "
         + "    Select DocumentTypeId, Id, GpSrNo, SupplierWeight, SupplierContractCode, SupplierCustomerId, NoOfPackages from GatePassInward group by DocumentTypeId, Id, GpSrNo, SupplierWeight, SupplierContractCode, SupplierCustomerId, NoOfPackages "
         + "    Union All "
         + "    select DocumentTypeId, id, GpSrNo, SupplierWeight, SupplierContractCode, SupplierCustomerId, NoOfPackages from GatePassOutward group by DocumentTypeId, Id, GpSrNo, SupplierWeight, SupplierContractCode, SupplierCustomerId, NoOfPackages "
         + "    Union All "
         + "    select wb.ReferenceDocTypeId, Id, ReferenceDocNoId, 0, '', 0 SupplierCustomerId, 0 from WbTransactions wb where wb.ReferenceDocTypeId in (74,75) Group By wb.ReferenceDocTypeId, Id, ReferenceDocNoId "
         + "  ) A Group By a.DocumentTypeId, a.Id, a.GpSrNo, a.SupplierWeight, a.SupplierContractCode, a.SupplierCustomerId, a.NoOfPackages "
         + ") "
         + "SELECT wbt.Id, wbt.DocTypeId, wbt.EntryNo, wbt.WeighBridgeType, wbt.DocDate, wbt.TicketNo, wbt.ReferenceDocTypeId, dct.DocumentTypeCode, dct.DocumentTypeDescription, wbt.ReferenceDocNoId, "
         + "       wbt.VehicleNo, wbt.BiltyNo, wbt.SupplierCustomerId, "
         + "       case when sp.CompanyName is null then wbt.PartyName else sp.CompanyName end PartyName, "
         + "       sp.CompanyName, "
         + "       sp.SupCustCode, wbt.ItemDescription, "
         + "       CASE WHEN ISNULL(wbt.ItemQty,0) > 0 THEN ISNULL(wbt.ItemQty,0) ELSE ISNULL(GP.NoOfPackages,0) END AS ItemQty, "
         + "       wbt.WbCharges, wbt.FirstWeight, wbt.PackingType, wbt.FirstDateTime, wbt.FirstWtPicReading, "
         + "       wbt.FirstWtUpperPic, wbt.FirstWtFrontPic, wbt.SecondWeight, "
         + "       wbt.SecondDateTime, wbt.SecondPicReading, wbt.SecondWtUpperPic, wbt.SecondWtFrontPic, wbt.WbRemarks, "
         + "       wbt.EntryDate, wbt.EntryUser, wbt.ModifyDate, wbt.ModifyUser, "
         + "       wbt.IsApproved, wbt.ApprovedUserId, "
         + "       wbt.ApprovedDateTime, wbt.OrganizationId, wbt.NetWbWeight, "
         + "       wbt.CompanyId, doc.DocumentTypeDescription DocType, "
         + "       wbt.LoadWeight, wbt.TearWeight, Wbt.SupplierWbNetWeight, "
         + "       Eusr.FirstName + ' ' + Eusr.LastName AS FirstNameEusr, Ausr.FirstName AS FirstNameAusr, wbt.ContainerNo, Gp.GpSrNo, gp.SupplierWeight, gp.SupplierContractCode OrderNo "
         + "FROM dbo.WbTransactions AS wbt "
         + "INNER JOIN dbo.DocumentType AS dct ON wbt.ReferenceDocTypeId = dct.Id "
         + "INNER JOIN DocumentType doc on wbt.DocTypeId = doc.Id "
         + "LEFT JOIN MyDocs AS Gp ON wbt.ReferenceDocTypeId = Gp.DocumentTypeId AND wbt.ReferenceDocNoId = Gp.Id "
         + "LEFT JOIN dbo.UserAccount AS Ausr ON wbt.ApprovedUserId = Ausr.ID "
         + "LEFT JOIN dbo.UserAccount AS Eusr ON wbt.EntryUser = Eusr.ID "
         + "LEFT JOIN dbo.SupplierCustomer AS sp ON Gp.SupplierCustomerId = sp.Id "
         + "where wbt.OrganizationId = :organizationId "
         + "and wbt.CompanyId = :companyId "
         + "and (:mId is null or wbt.Id = :mId) "
         + "and (:dateFrom is null or CAST(wbt.DocDate as date) >= cast(:dateFrom as date)) "
         + "and (:dateTo is null or CAST(wbt.DocDate as date) <= cast(:dateTo as date)) "
         + "and (:ticketNoFrom is null or wbt.TicketNo >= :ticketNoFrom) "
         + "and (:ticketNoTo is null or wbt.TicketNo <= :ticketNoTo) "
         + "and (:orderNoFrom is null or gp.SupplierContractCode >= :orderNoFrom) "
         + "and (:orderNoTo is null or gp.SupplierContractCode <= :orderNoTo) "
         + "and (:documentTypeId is null or wbt.DocTypeId = :documentTypeId) "
         + "and (:gpNoFrom is null or gp.GpSrNo >= :gpNoFrom) "
         + "and (:gpNoTo is null or gp.GpSrNo <= :gpNoTo) "
         + "and (:vehicleNo is null or wbt.VehicleNo = :vehicleNo) "
         + "and (:financialYearId is null or wbt.FinancialYearId = :financialYearId) "
         + "and (:refDocumentTypeIds is null or wbt.ReferenceDocTypeId in (select data from dbo.fnSplitString(:refDocumentTypeIds, ','))) "
         + "and (:branchId is null or wbt.BranchId = :branchId) "
         + "and (:weighBridgeType is null or wbt.WeighBridgeType = :weighBridgeType) "
         + "and (:supplierCustomerId is null or wbt.SupplierCustomerId = :supplierCustomerId) "
         + "and (:wbPartyName is null or (case when sp.CompanyName is null then wbt.PartyName else sp.CompanyName end) = :wbPartyName) "
         + "order by DocDate, TicketNo", nativeQuery = true)
    List<WbTransactionReadHistoryRow> readHistory(@Param("organizationId") Integer organizationId,
                                                  @Param("companyId") Integer companyId,
                                                  @Param("mId") Integer mId,
                                                  @Param("dateFrom") LocalDate dateFrom,
                                                  @Param("dateTo") LocalDate dateTo,
                                                  @Param("ticketNoFrom") Integer ticketNoFrom,
                                                  @Param("ticketNoTo") Integer ticketNoTo,
                                                  @Param("orderNoFrom") Integer orderNoFrom,
                                                  @Param("orderNoTo") Integer orderNoTo,
                                                  @Param("documentTypeId") Integer documentTypeId,
                                                  @Param("gpNoFrom") Integer gpNoFrom,
                                                  @Param("gpNoTo") Integer gpNoTo,
                                                  @Param("vehicleNo") String vehicleNo,
                                                  @Param("financialYearId") Integer financialYearId,
                                                  @Param("refDocumentTypeIds") String refDocumentTypeIds,
                                                  @Param("branchId") Integer branchId,
                                                  @Param("weighBridgeType") String weighBridgeType,
                                                  @Param("supplierCustomerId") Integer supplierCustomerId,
                                                  @Param("wbPartyName") String wbPartyName);

    /** Sp_WbTransation_GetAllMethod @Activity='GetNetWeightFromWbTransactions' (@Id = ReferenceDocNoId). Default: branchId = null. */
    @Query(value = "SELECT WbTransactions.Id, "
         + "       WbTransactions.EntryNo, "
         + "       WbTransactions.TicketNo, "
         + "       WbTransactions.ReferenceDocTypeId, "
         + "       WbTransactions.ReferenceDocNoId, DocumentType.DocumentTypeDescription, "
         + "       WbTransactions.FirstWeight, "
         + "       WbTransactions.SecondWeight, "
         + "       WbTransactions.DocDate, "
         + "       WbTransactions.WbRemarks, "
         + "       WbTransactions.ContainerNo, "
         + "       WbTransactions.NetWbWeight, "
         + "       WbTransactions.LoadWeight, "
         + "       WbTransactions.TearWeight, "
         + "       WbTransactions.SupplierWbNetWeight "
         + "FROM WbTransactions "
         + "LEFT JOIN DocumentType ON WbTransactions.ReferenceDocTypeId = DocumentType.Id "
         + "WHERE (WbTransactions.OrganizationId = :organizationId) "
         + "AND (WbTransactions.CompanyId = :companyId) "
         + "AND (WbTransactions.FirstWeight > 0) "
         + "and (WbTransactions.SecondWeight > 0) "
         + "and WbTransactions.ReferenceDocNoId = :id "
         + "and WbTransactions.ReferenceDocTypeId = :refDocumentTypeId "
         + "and (:branchId IS NULL OR WbTransactions.BranchId = :branchId)", nativeQuery = true)
    List<WbNetWeightRow> getNetWeightFromWbTransactions(@Param("organizationId") Integer organizationId,
                                                        @Param("companyId") Integer companyId,
                                                        @Param("id") Integer id,
                                                        @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                        @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='GetWeighBridgeDataForRejectedTicketNos'. Default: branchId = null. */
    @Query(value = "SELECT WB.Id, WB.DocDate, WB.TicketNo, dt.DocumentTypeDescription, "
         + "       WB.ItemQty, WB.FirstWeight, WB.SecondWeight, WB.NetWbWeight, "
         + "       WB.VehicleNo, WB.BiltyNo, WB.ItemDescription, WB.WbRemarks, WB.WbCharges "
         + "FROM WbTransactions WB "
         + "LEFT JOIN DocumentType dt ON WB.ReferenceDocTypeId = dt.Id "
         + "WHERE (WB.OrganizationId = :organizationId) "
         + "AND (WB.CompanyId = :companyId) "
         + "AND (WB.NetWbWeight > 0) "
         + "and (ISNULL(WB.WbStatus,0) = 0) "
         + "and WB.ReferenceDocTypeId = :refDocumentTypeId "
         + "and (:branchId IS NULL OR wb.BranchId = :branchId) "
         + "and Not EXISTS (select WbTicketId from InvStockTransferHeader "
         + "                where OrganizationId = :organizationId "
         + "                AND CompanyId = :companyId "
         + "                AND WbTicketId = WB.Id)", nativeQuery = true)
    List<WbRejectedTicketRow> getWeighBridgeDataForRejectedTicketNos(@Param("organizationId") Integer organizationId,
                                                                     @Param("companyId") Integer companyId,
                                                                     @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                                     @Param("branchId") Integer branchId);

    /** Sp_WbTransation_GetAllMethod @Activity='WeighBridgeRejectedStatusUpdate' — sets WbStatus = 1, RejectedUser = @EntryUser, RejectedDate = @RejectedDate. */
    @Modifying
    @Transactional
    @Query(value = "UPDATE WbTransactions SET WbStatus = 1, RejectedUser = :entryUser, RejectedDate = :rejectedDate "
         + "WHERE ReferenceDocTypeId = :refDocumentTypeId "
         + "AND Id = :id "
         + "AND OrganizationId = :organizationId "
         + "AND CompanyId = :companyId", nativeQuery = true)
    int weighBridgeRejectedStatusUpdate(@Param("refDocumentTypeId") Integer refDocumentTypeId,
                                        @Param("id") Integer id,
                                        @Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId,
                                        @Param("entryUser") Integer entryUser,
                                        @Param("rejectedDate") LocalDateTime rejectedDate);

    /** Sp_WbTransation_GetAllMethod @Activity='GetPartiesFromWbTransactions'. */
    @Query(value = "SELECT distinct SupplierCustomerId as Id, sp.CompanyName "
         + "FROM WbTransactions wb "
         + "INNER JOIN SupplierCustomer sp on wb.SupplierCustomerId = sp.Id "
         + "WHERE WB.OrganizationId = :organizationId "
         + "AND WB.CompanyId = :companyId", nativeQuery = true)
    List<WbPartyRow> getPartiesFromWbTransactions(@Param("organizationId") Integer organizationId,
                                                  @Param("companyId") Integer companyId);
}
