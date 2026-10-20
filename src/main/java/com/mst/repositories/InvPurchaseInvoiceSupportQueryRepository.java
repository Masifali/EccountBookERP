package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InvPurchaseInvoice;
import com.mst.projections.PendingPurchaseInvoiceForApprovalRow;
import com.mst.projections.PurchaseInvoiceComboRow;
import com.mst.projections.PurchaseInvoiceDetailReadByIdRow;

/**
 * Query-only repository for USP_InvPurchaseInvoiceDetail_ReadById, USp_PendingInvPurchaseInvoiceForApproval and
 * Usp_AllComboAgainstPurchaseInvoice. All three are multi-temp-table pipelines and are kept as EXEC calls (KEEP-EXEC).
 * Bound to the existing InvPurchaseInvoice entity (owned by G2_purchase_invoice_write) only as the Spring Data anchor.
 */
@Repository
public interface InvPurchaseInvoiceSupportQueryRepository extends org.springframework.data.repository.Repository<InvPurchaseInvoice, Integer> {

    /**
     * USP_InvPurchaseInvoiceDetail_ReadById (single branch)
     * KEEP-EXEC: #AttachData + #WO_With_Attachments temp tables with 12 UPDATE passes (lab, GRN, order, demand, challan, pre-bill, 5 attachment counts, ExIm/sale refs)
     * before the final 15-join select — not expressible as one statement. Projection = the proc's final SELECT (OrderDate/GrnDate are DATE columns).
     */
    @Query(value = "EXEC dbo.USP_InvPurchaseInvoiceDetail_ReadById @Id = :id", nativeQuery = true)
    List<PurchaseInvoiceDetailReadByIdRow> uspInvPurchaseInvoiceDetailReadById(@Param("id") Integer id);

    /**
     * USp_PendingInvPurchaseInvoiceForApproval (single branch)
     * KEEP-EXEC: builds @ReportParamCSV, 8 temp tables (#tmpResult, #tmpDms, #tmpAprovalLimit, #TmpIsFinalUsers, #TmpIsMandatoryUsers, #TmpUnqIds, #TmpFilterids, #Data)
     * with STRING_AGG / OFFSET-FETCH pagination / window totals; DAW.DocumentApprovalDetail has no entity. Defaults: FromDate/ToDate/SupplierCustomerId/ItemId/IsApproved/UserId/BranchesIds/PageSize/PageNumber = NULL
     * (NULL PageSize+PageNumber → all rows). Projection = ReportCriteria + #Data.* + TotalOfBillAmount, TotalOfCommAmount, TotalHeaderRecord, BranchName.
     */
    @Query(value = "EXEC dbo.USp_PendingInvPurchaseInvoiceForApproval @OrganizationId = :organizationId, @CompanyId = :companyId, @DocumentTypeId = :documentTypeId, "
         + "@FromDate = :fromDate, @Todate = :toDate, @SupplierCustomerId = :supplierCustomerId, @ItemId = :itemId, @IsApproved = :isApproved, "
         + "@UserId = :userId, @BranchesIds = :branchesIds, @PageSize = :pageSize, @PageNumber = :pageNumber", nativeQuery = true)
    List<PendingPurchaseInvoiceForApprovalRow> uspPendingInvPurchaseInvoiceForApproval(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeId") Integer documentTypeId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate, @Param("supplierCustomerId") Integer supplierCustomerId, @Param("itemId") Integer itemId, @Param("isApproved") Boolean isApproved, @Param("userId") Integer userId, @Param("branchesIds") String branchesIds, @Param("pageSize") Integer pageSize, @Param("pageNumber") Integer pageNumber);

    /**
     * Usp_AllComboAgainstPurchaseInvoice (single branch)
     * KEEP-EXEC: #Data + #tmpResult temp tables filled by 16 INSERT...SELECT lookups (ReferenceParties, InvDueTerms, InventoryParentCategories, ItemClassGroup have no entity).
     * Defaults: all parameters NULL (Activity NULL → every lookup type). Output columns Activity, Id, ReferenceName.
     */
    @Query(value = "EXEC dbo.Usp_AllComboAgainstPurchaseInvoice @OrganizationId = :organizationId, @CompanyId = :companyId, @DocumentTypeIds = :documentTypeIds, "
         + "@Activity = :activity, @BranchesIds = :branchesIds", nativeQuery = true)
    List<PurchaseInvoiceComboRow> uspAllComboAgainstPurchaseInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("documentTypeIds") String documentTypeIds, @Param("activity") String activity, @Param("branchesIds") String branchesIds);
}
