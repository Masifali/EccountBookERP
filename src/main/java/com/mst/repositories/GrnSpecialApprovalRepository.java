package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.UserAudit;
import com.mst.projections.GrnWeightValidationRow;

/**
 * GRN special-approval / weight-validation procedures of batch F2_grn_support:
 * USP_GrnBillWeightAndStockWeightValidations, USP_InvGrn_GenerateSpecialApprovalLog, USP_InvGrn_UpdateSpecialApproval.
 * Bound to UserAudit (owned) only because Spring Data needs a managed entity.
 */
@Repository
public interface GrnSpecialApprovalRepository extends org.springframework.data.repository.Repository<UserAudit, Long> {

    // ---------------------------------------------------------------------------------------------
    // USP_GrnBillWeightAndStockWeightValidations
    // ---------------------------------------------------------------------------------------------

    /**
     * USP_GrnBillWeightAndStockWeightValidations (single branch).
     * // KEEP-EXEC: reads configs 110/202/206/249 via dbo.GetConfigurationByConfigId, conditionally UPDATEs
     * InvGrn.BillWeightExceedsStockWeight and EXECs DAW.USp_DocumentApprovalDetail_Insert (46,@Id,1) when
     * EBWTotal = 0 or the bill/stock weight difference exceeds the tolerance. (The RAISERROR blocks are commented out in the proc.)
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_GrnBillWeightAndStockWeightValidations @OrganizationId = :organizationId, @CompanyId = :companyId, @Id = :id",
           nativeQuery = true)
    void grnBillWeightAndStockWeightValidations(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId,
                                                @Param("id") Integer id);

    /**
     * USP_GrnBillWeightAndStockWeightValidations — the aggregate the proc computes before deciding
     * (DiffWeight = SUM(NetBillWeight) - (SUM(StockWeight) + SUM(EbPurAgainstWeight)), EbWtTotal = SUM(EBWTotal)).
     * JPQL on InvGrnDetail / InvGrn (F1_grn_core entities). Empty list when the GRN has no detail rows.
     */
    @Query("SELECT COALESCE(SUM(d.netBillWeight),0) - (COALESCE(SUM(d.stockWeight),0) + COALESCE(SUM(d.ebPurAgainstWeight),0)) AS diffWeight, "
         + "COALESCE(SUM(d.ebwTotal),0) AS ebWtTotal, h.deliveryTerm AS deliveryTerm, h.inwardGatePassId AS inwardGatePassId, "
         + "h.freightDeduction AS freightDeduction "
         + "FROM InvGrnDetail d JOIN InvGrn h ON d.invGrnId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND d.invGrnId = :id "
         + "GROUP BY h.deliveryTerm, h.inwardGatePassId, h.freightDeduction")
    List<GrnWeightValidationRow> grnWeightValidationSummary(@Param("organizationId") Integer organizationId,
                                                            @Param("companyId") Integer companyId,
                                                            @Param("id") Integer id);

    /** USP_GrnBillWeightAndStockWeightValidations: SELECT RefDocumentTypeId FROM GatePassInward WHERE Id = @getGpId */
    @Query("SELECT gp.refDocumentTypeId FROM InwardGatePass gp WHERE gp.id = :gatePassId")
    Integer gatePassRefDocumentTypeId(@Param("gatePassId") Integer gatePassId);

    /** USP_GrnBillWeightAndStockWeightValidations: UPDATE InvGrn SET BillWeightExceedsStockWeight = 1 WHERE Id = @Id */
    @Modifying
    @Transactional
    @Query("UPDATE InvGrn g SET g.billWeightExceedsStockWeight = 1 WHERE g.id = :id")
    int markBillWeightExceedsStockWeight(@Param("id") Integer id);

    /** USP_GrnBillWeightAndStockWeightValidations: the config values the proc reads (110, 202, 206, 249) — returns the raw config string. */
    @Query(value = "SELECT dbo.GetConfigurationByConfigId(:organizationId, :companyId, :configId)", nativeQuery = true)
    String configurationByConfigId(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("configId") Integer configId);

    // ---------------------------------------------------------------------------------------------
    // USP_InvGrn_GenerateSpecialApprovalLog
    // ---------------------------------------------------------------------------------------------

    /**
     * USP_InvGrn_GenerateSpecialApprovalLog (single branch).
     * // KEEP-EXEC: detects reason ids 1..5 (STRING_AGG) and calls dbo.USP_SpecialApprovalLog_InsertAndUpdate
     * with @ReasonIds ('' when none) — behaviour must stay the same.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_InvGrn_GenerateSpecialApprovalLog @CompanyId = :companyId, @OrganizationId = :organizationId, "
         + "@GrnDocumentTypeId = :grnDocumentTypeId, @GrnDocId = :grnDocId, @UserId = :userId", nativeQuery = true)
    void generateSpecialApprovalLog(@Param("companyId") Integer companyId,
                                    @Param("organizationId") Integer organizationId,
                                    @Param("grnDocumentTypeId") Integer grnDocumentTypeId,
                                    @Param("grnDocId") Integer grnDocId,
                                    @Param("userId") Integer userId);

    /**
     * USP_InvGrn_GenerateSpecialApprovalLog — the ReasonCTE pasted verbatim: the applicable special-approval
     * reason ids (1 WeightDiffComments &amp; AccessWeight&gt;0, 2 BillWeightExceedsStockWeight, 3 EBWTotal=0,
     * 4 WeightDiffComments &amp; AccessWeight=0, 5 RateCut&lt;&gt;0 &amp; AccessWeight&gt;0), each only while the GRN
     * has no purchase invoice. Native (UNION ALL, FROM-less SELECT ... WHERE EXISTS).
     */
    @Query(value = "SELECT 1 AS ReasonId WHERE EXISTS (SELECT 1 FROM dbo.InvGrn pih LEFT JOIN dbo.GatePassInward gp ON gp.Id = pih.InwardGatePassId "
         + "WHERE pih.Id = :grnDocId AND ISNULL(gp.WeightDiffComments,'') <> '' AND ISNULL(pih.AccessWeight,0) > 0 "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND d.InvGrnId = pih.Id)) "
         + "UNION ALL "
         + "SELECT 2 WHERE EXISTS (SELECT 1 FROM dbo.InvGrn pih "
         + "WHERE pih.Id = :grnDocId AND ISNULL(pih.BillWeightExceedsStockWeight,0) = 1 "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND d.InvGrnId = pih.Id)) "
         + "UNION ALL "
         + "SELECT 3 WHERE EXISTS (SELECT 1 FROM dbo.InvGrn pih INNER JOIN dbo.InvGrnDetail pid ON pih.Id = pid.InvGrnId "
         + "WHERE pih.Id = :grnDocId AND ISNULL(pid.EBWTotal,0) = 0 "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND d.InvGrnId = pih.Id)) "
         + "UNION ALL "
         + "SELECT 4 WHERE EXISTS (SELECT 1 FROM dbo.InvGrn pih LEFT JOIN dbo.GatePassInward gp ON gp.Id = pih.InwardGatePassId "
         + "WHERE pih.Id = :grnDocId AND ISNULL(gp.WeightDiffComments,'') <> '' AND ISNULL(pih.AccessWeight,0) = 0 "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND d.InvGrnId = pih.Id)) "
         + "UNION ALL "
         + "SELECT 5 WHERE EXISTS (SELECT 1 FROM dbo.InvGrn pih LEFT JOIN dbo.GatePassInward gp ON gp.Id = pih.InwardGatePassId "
         + "WHERE pih.Id = :grnDocId AND ISNULL(gp.RateCut,0) <> 0 AND ISNULL(pih.AccessWeight,0) > 0 "
         + "AND NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail d INNER JOIN InvPurchaseInvoice h ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND d.InvGrnId = pih.Id))", nativeQuery = true)
    List<Integer> detectSpecialApprovalReasonIds(@Param("companyId") Integer companyId,
                                                 @Param("organizationId") Integer organizationId,
                                                 @Param("grnDocId") Integer grnDocId);

    // ---------------------------------------------------------------------------------------------
    // USP_InvGrn_UpdateSpecialApproval  (TVP parameter dbo.TVP_SpecialApprovalListType)
    // ---------------------------------------------------------------------------------------------
    // The proc takes a table-valued parameter which JPA/JDBC cannot bind, so it is decomposed per TVP row:
    // the caller loops over the rows, obtains one `now` timestamp (the proc's @Now = GETDATE()) and, inside ONE
    // transaction, calls updateSpecialApprovalLog → updateInvGrnRateCutForReason5 → insertUserAuditForSpecialApproval
    // for each row. Every statement is the proc's SQL with "@ApprovalData A" replaced by a one-row derived table
    // of the same columns (Id, ReasonId, OrganizationId, CompanyId, FinancialYearId, DocumentTypeId, ReqType,
    // ApprovedUserId, ApprovalRemarks, RateCut).

    /**
     * USP_InvGrn_UpdateSpecialApproval step 1 — per TVP row: UPDATE SpecialApprovalLog (IsApproved = ReqType='AP',
     * ApprovedDate = @Now, ApprovedUserId, ApprovalRemarks) joined to InvGrn, ActionId &lt;&gt; 3. Native (no entity).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE WO SET "
         + "WO.IsApproved = CASE WHEN A.ReqType = 'AP' THEN 1 ELSE 0 END, "
         + "WO.ApprovedDate = :now, "
         + "WO.ApprovedUserId = A.ApprovedUserId, "
         + "WO.ApprovalRemarks = A.ApprovalRemarks "
         + "FROM dbo.SpecialApprovalLog WO "
         + "INNER JOIN dbo.InvGrn G ON G.Id = WO.RefDocId AND G.DocumentTypeId = WO.RefDocumentTypeId "
         + "INNER JOIN (SELECT :id AS Id, :reasonId AS ReasonId, :organizationId AS OrganizationId, :companyId AS CompanyId, "
         + ":financialYearId AS FinancialYearId, :documentTypeId AS DocumentTypeId, :reqType AS ReqType, "
         + ":approvedUserId AS ApprovedUserId, :approvalRemarks AS ApprovalRemarks) A "
         + "ON A.Id = G.Id AND A.ReasonId = WO.ReasonId AND A.OrganizationId = G.OrganizationId AND A.CompanyId = G.CompanyId "
         + "AND A.FinancialYearId = G.FinancialYearId AND A.DocumentTypeId = G.DocumentTypeId "
         + "WHERE WO.ActionId <> 3", nativeQuery = true)
    int updateSpecialApprovalLog(@Param("id") Integer id,
                                 @Param("reasonId") Integer reasonId,
                                 @Param("organizationId") Integer organizationId,
                                 @Param("companyId") Integer companyId,
                                 @Param("financialYearId") Integer financialYearId,
                                 @Param("documentTypeId") Integer documentTypeId,
                                 @Param("reqType") String reqType,
                                 @Param("approvedUserId") Integer approvedUserId,
                                 @Param("approvalRemarks") String approvalRemarks,
                                 @Param("now") LocalDateTime now);

    /**
     * USP_InvGrn_UpdateSpecialApproval step 2 — per TVP row: UPDATE InvGrn SET RateCut, RateCutRemarks = ApprovalRemarks
     * only when the row's ReasonId = 5 and a matching non-deleted SpecialApprovalLog exists. Native (SpecialApprovalLog join).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE G SET "
         + "G.RateCut = A.RateCut, "
         + "G.RateCutRemarks = A.ApprovalRemarks "
         + "FROM dbo.InvGrn G "
         + "INNER JOIN dbo.SpecialApprovalLog WO ON G.Id = WO.RefDocId AND G.DocumentTypeId = WO.RefDocumentTypeId AND WO.ActionId <> 3 "
         + "INNER JOIN (SELECT :id AS Id, :reasonId AS ReasonId, :organizationId AS OrganizationId, :companyId AS CompanyId, "
         + ":financialYearId AS FinancialYearId, :documentTypeId AS DocumentTypeId, :rateCut AS RateCut, "
         + ":approvalRemarks AS ApprovalRemarks) A "
         + "ON A.Id = G.Id AND A.ReasonId = WO.ReasonId AND A.ReasonId = 5 AND A.OrganizationId = G.OrganizationId "
         + "AND A.CompanyId = G.CompanyId AND A.FinancialYearId = G.FinancialYearId AND A.DocumentTypeId = G.DocumentTypeId",
           nativeQuery = true)
    int updateInvGrnRateCutForReason5(@Param("id") Integer id,
                                      @Param("reasonId") Integer reasonId,
                                      @Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("financialYearId") Integer financialYearId,
                                      @Param("documentTypeId") Integer documentTypeId,
                                      @Param("rateCut") BigDecimal rateCut,
                                      @Param("approvalRemarks") String approvalRemarks);

    /**
     * USP_InvGrn_UpdateSpecialApproval step 3 — per TVP row: INSERT dbo.UserAudit (Id = ISNULL(MAX(Id),0)+ROW_NUMBER(),
     * ActivityDate = @Now, UserId = ApprovedUserId, ClientIP NULL, ActivityDescription 'Approve/UnApprove "Grn (<reason>)"
     * With doc No <DocNo>', RefDocumentTypeId = DocumentTypeId, ReferenceId = Id, ScreenName = DocumentType.ScreenName,
     * ActionId = 2) — one audit row per non-deleted SpecialApprovalLog of the GRN (the proc does not filter by ReasonId here).
     * Native INSERT ... SELECT verbatim; alternatively build {@link com.mst.models.UserAudit} and save() with UserAuditRepository.maxId()+n.
     */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO dbo.UserAudit (Id, ActivityDate, UserId, ClientIP, ActivityDescription, RefDocumentTypeId, ReferenceId, "
         + "ScreenName, OrganizationId, CompanyId, ActionId) "
         + "SELECT (SELECT ISNULL(MAX(Id), 0) FROM dbo.UserAudit) + ROW_NUMBER() OVER (ORDER BY A.Id), "
         + ":now, A.ApprovedUserId, NULL, "
         + "CASE WHEN A.ReqType = 'AP' "
         + "THEN 'Approve \"Grn (' + SR.SpecialApprovalReason + ')\" With doc No ' + CAST(G.DocNo AS NVARCHAR(50)) "
         + "ELSE 'UnApprove \"Grn (' + SR.SpecialApprovalReason + ')\" With doc No ' + CAST(G.DocNo AS NVARCHAR(50)) END, "
         + "A.DocumentTypeId, A.Id, DT.ScreenName, A.OrganizationId, A.CompanyId, 2 "
         + "FROM (SELECT :id AS Id, :organizationId AS OrganizationId, :companyId AS CompanyId, :documentTypeId AS DocumentTypeId, "
         + ":reqType AS ReqType, :approvedUserId AS ApprovedUserId) A "
         + "INNER JOIN dbo.InvGrn G ON A.Id = G.Id AND A.DocumentTypeId = G.DocumentTypeId "
         + "INNER JOIN dbo.SpecialApprovalLog WO ON G.Id = WO.RefDocId AND G.DocumentTypeId = WO.RefDocumentTypeId AND WO.ActionId <> 3 "
         + "INNER JOIN dbo.SpecialApprovalReason SR ON SR.Id = WO.ReasonId "
         + "LEFT JOIN dbo.DocumentType DT ON DT.Id = A.DocumentTypeId", nativeQuery = true)
    int insertUserAuditForSpecialApproval(@Param("id") Integer id,
                                          @Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId,
                                          @Param("documentTypeId") Integer documentTypeId,
                                          @Param("reqType") String reqType,
                                          @Param("approvedUserId") Integer approvedUserId,
                                          @Param("now") LocalDateTime now);
}
