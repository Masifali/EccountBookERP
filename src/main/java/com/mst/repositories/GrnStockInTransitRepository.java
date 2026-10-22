package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvGrnPurchaseBreakUp;

/**
 * Stock-in-transit evaluation / voucher posting procedures of batch F2_grn_support:
 * usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn,
 * usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId.
 * Bound to InvGrnPurchaseBreakUp (owned) only because Spring Data needs a managed entity.
 */
@Repository
public interface GrnStockInTransitRepository extends org.springframework.data.repository.Repository<InvGrnPurchaseBreakUp, Integer> {

    // ---------------------------------------------------------------------------------------------
    // usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn
    // ---------------------------------------------------------------------------------------------

    /**
     * usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn (single branch).
     * // KEEP-EXEC: deletes previous InventoryStockEvalautionDetail rows + VoucherHead/VoucherDetail of the GRN, then
     * (only for DocumentTypeId IN 46,137,143,701,48) reads config 403 (Inventory Control Account — RAISERROR if 0),
     * builds the per-item latest rate (#ItemLatestRate: 4-priority UNION ALL + ROW_NUMBER), inserts the stock
     * evaluation rows (freight apportioned by StockWeight for Load delivery terms) and posts a VoucherHead with
     * Dr Inventory / Cr Control Account VoucherDetail lines. Not expressible as one statement.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, @DocumentTypeId = :documentTypeId, @GrnId = :grnId",
           nativeQuery = true)
    void stockInTransitUpdateStockEvaluationAndVoucherInsertFromGrn(@Param("organizationId") Integer organizationId,
                                                                    @Param("companyId") Integer companyId,
                                                                    @Param("documentTypeId") Integer documentTypeId,
                                                                    @Param("grnId") Integer grnId);

    /**
     * usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn: DELETE FROM InventoryStockEvalautionDetail
     * WHERE OrganizationId=@OrganizationId AND CompanyId=@CompanyId AND OtherDocumentTypeid=@DocumentTypeId AND OtherDocNoId=@GrnId
     */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryStockEvalautionDetail t WHERE t.organizationId = :organizationId AND t.companyId = :companyId "
         + "AND t.otherDocumentTypeId = :documentTypeId AND t.otherDocNoId = :grnId")
    int deleteStockEvaluationByGrn(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("documentTypeId") Integer documentTypeId,
                                   @Param("grnId") Integer grnId);

    /**
     * usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn: SELECT Id FROM VoucherHead WHERE DocumentTypeId=@DocumentTypeId
     * AND DocumentTypeSrNo=@GrnId AND OrganizationId=@OrganizationId AND CompanyId=@CompanyId (proc takes the single value; take first).
     */
    @Query("SELECT vh.id FROM VoucherHead vh WHERE vh.documentType.id = :documentTypeId AND vh.documentTypeSrNo = :grnId "
         + "AND vh.organizationId = :organizationId AND vh.companyId = :companyId")
    List<Integer> findVoucherHeadIdsByGrn(@Param("documentTypeId") Integer documentTypeId,
                                          @Param("grnId") Integer grnId,
                                          @Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId);

    /** usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn: DELETE FROM VoucherDetail WHERE VoucherHeadId=@VHId */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail vd WHERE vd.voucherHead.id = :voucherHeadId")
    int deleteVoucherDetailByVoucherHeadId(@Param("voucherHeadId") Integer voucherHeadId);

    /** usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn: DELETE FROM VoucherHead WHERE Id=@VHId */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherHead vh WHERE vh.id = :voucherHeadId")
    int deleteVoucherHeadById(@Param("voucherHeadId") Integer voucherHeadId);

    /** usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn: SELECT FinancialYearId FROM InvGrn WHERE ID = @GrnId */
    @Query("SELECT g.financialYearId FROM InvGrn g WHERE g.id = :grnId")
    Integer findGrnFinancialYearId(@Param("grnId") Integer grnId);

    /** usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn: config 403 Inventory Control Account (proc RAISERRORs when NULL/0). */
    @Query(value = "SELECT CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 403) AS INT)", nativeQuery = true)
    Integer inventoryControlAccount(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    // ---------------------------------------------------------------------------------------------
    // usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId
    // ---------------------------------------------------------------------------------------------

    /**
     * usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId (single branch).
     * // KEEP-EXEC: TRY/CATCH + explicit transaction (XACT_ABORT) deleting, for every GRN on the purchase invoice,
     * the InventoryStockEvalautionDetail rows and the GRN's VoucherDetail then VoucherHead. The three deletes are also
     * available as the JPQL methods below (call them in that order inside one transaction).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId @Id = :id", nativeQuery = true)
    void stockInTransitEvaluationAndVoucherDeleteByPurchaseInvoiceId(@Param("id") Integer id);

    /**
     * usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId STEP 2: DELETE InventoryStockEvalautionDetail T
     * JOIN InvGrn GH ON T.OtherDocNoId = GH.Id AND T.OtherDocumentTypeId = GH.DocumentTypeId for GRNs of the invoice.
     */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryStockEvalautionDetail t WHERE EXISTS ("
         + "SELECT 1 FROM InvGrn gh WHERE gh.id = t.otherDocNoId AND gh.documentTypeId = t.otherDocumentTypeId "
         + "AND gh.id IN (SELECT d.invGrnId FROM InvPurchaseInvoiceDetail d WHERE d.invPurchaseInvoiceId = :id))")
    int deleteStockEvaluationByPurchaseInvoiceId(@Param("id") Integer id);

    /** usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId STEP 3: the VoucherHead ids of the invoice's GRNs (#VH). */
    @Query("SELECT vh.id FROM VoucherHead vh JOIN InvGrn gh ON vh.documentTypeSrNo = gh.id AND vh.documentType.id = gh.documentTypeId "
         + "WHERE gh.id IN (SELECT d.invGrnId FROM InvPurchaseInvoiceDetail d WHERE d.invPurchaseInvoiceId = :id)")
    List<Integer> findVoucherHeadIdsByPurchaseInvoiceId(@Param("id") Integer id);

    /** usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId STEP 4: DELETE VoucherDetail of those VoucherHeads. */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail vd WHERE vd.voucherHead.id IN ("
         + "SELECT vh.id FROM VoucherHead vh JOIN InvGrn gh ON vh.documentTypeSrNo = gh.id AND vh.documentType.id = gh.documentTypeId "
         + "WHERE gh.id IN (SELECT d.invGrnId FROM InvPurchaseInvoiceDetail d WHERE d.invPurchaseInvoiceId = :id))")
    int deleteVoucherDetailByPurchaseInvoiceId(@Param("id") Integer id);

    /** usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId STEP 5: DELETE those VoucherHeads (after the details). */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherHead vh WHERE vh.id IN ("
         + "SELECT vh2.id FROM VoucherHead vh2 JOIN InvGrn gh ON vh2.documentTypeSrNo = gh.id AND vh2.documentType.id = gh.documentTypeId "
         + "WHERE gh.id IN (SELECT d.invGrnId FROM InvPurchaseInvoiceDetail d WHERE d.invPurchaseInvoiceId = :id))")
    int deleteVoucherHeadByPurchaseInvoiceId(@Param("id") Integer id);
}
