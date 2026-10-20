package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.SaleOrder;
import com.mst.projections.SaleOrderRiceAndPaddyRegisterRptRow;

/**
 * Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt (single branch, report).
 * The proc is a temp-table pipeline (#tmpResult, #TmpDispatches, #tmpDispatchWt, #tmpDms, #tmpAprovalLimit, #tmpOrderTotalAmount,
 * #TmpDistinctIds, #TmpIsFinalUsers, #TmpIsMandatoryDistinctIds, #TmpIsMandatoryUsers, #TmpUnqIds, #TmpFilterids, #Data) with
 * STRING_AGG ... WITHIN GROUP, OFFSET/FETCH pagination, several UPDATE passes and a procedurally built @ReportParamCSV string,
 * so it is kept as a procedure call (KEEP-EXEC). It returns TWO result sets: the report rows (riceAndPaddyRegisterRpt) and
 * Company.CompLogoImage (companyLogoImage).
 */
@Repository
public interface SalesSaleOrderRiceAndPaddyRegisterRptQueryRepository extends org.springframework.data.repository.Repository<SaleOrder, Integer> {

    /**
     * Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt - first result set.
     * // KEEP-EXEC: temp-table pipeline with STRING_AGG, OFFSET/FETCH paging, multi-pass UPDATEs and procedural ReportCriteria string.
     * All parameters default to NULL in the proc. pageSize/pageNumber NULL = all rows (SkipRows 0, PageSize 1000000);
     * appId in (4,6) with customerUserId overrides supplierCustomerId from UserAccount.SupplierCustomerId;
     * actionId = 1 adds DO (not yet in GDN) dispatch quantities; userId > 0 switches to approval-policy filtering.
     */
    @Query(value = "EXEC dbo.Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId, "
         + "@DocumentTypeId = :documentTypeId, @FromId = :fromId, @ToId = :toId, @FromDate = :fromDate, @Todate = :toDate, "
         + "@FromDocNo = :fromDocNo, @ToDocNo = :toDocNo, @InventoryParentCategoriesId = :inventoryParentCategoriesId, "
         + "@ItemCategoryId = :itemCategoryId, @ItemTypeId = :itemTypeId, @ItemId = :itemId, @SupplierCustomerId = :supplierCustomerId, "
         + "@IsApproved = :isApproved, @ActionId = :actionId, @CityId = :cityId, @Status = :status, @ReferencePartyId = :referencePartyId, "
         + "@ExpiryDays = :expiryDays, @PageSize = :pageSize, @PageNumber = :pageNumber, @BranchesIds = :branchesIds, @AppId = :appId, "
         + "@UserId = :userId, @CustomerUserId = :customerUserId, @CostCenterId = :costCenterId, @BookingPersonId = :bookingPersonId",
           nativeQuery = true)
    List<SaleOrderRiceAndPaddyRegisterRptRow> riceAndPaddyRegisterRpt(@Param("organizationId") Integer organizationId,
                                                                      @Param("companyId") Integer companyId,
                                                                      @Param("branchesId") Integer branchesId,
                                                                      @Param("projectsId") Integer projectsId,
                                                                      @Param("documentTypeId") Integer documentTypeId,
                                                                      @Param("fromId") Integer fromId,
                                                                      @Param("toId") Integer toId,
                                                                      @Param("fromDate") LocalDate fromDate,
                                                                      @Param("toDate") LocalDate toDate,
                                                                      @Param("fromDocNo") Integer fromDocNo,
                                                                      @Param("toDocNo") Integer toDocNo,
                                                                      @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
                                                                      @Param("itemCategoryId") Integer itemCategoryId,
                                                                      @Param("itemTypeId") Integer itemTypeId,
                                                                      @Param("itemId") Integer itemId,
                                                                      @Param("supplierCustomerId") Integer supplierCustomerId,
                                                                      @Param("isApproved") Boolean isApproved,
                                                                      @Param("actionId") Integer actionId,
                                                                      @Param("cityId") Integer cityId,
                                                                      @Param("status") String status,
                                                                      @Param("referencePartyId") Integer referencePartyId,
                                                                      @Param("expiryDays") Integer expiryDays,
                                                                      @Param("pageSize") Integer pageSize,
                                                                      @Param("pageNumber") Integer pageNumber,
                                                                      @Param("branchesIds") String branchesIds,
                                                                      @Param("appId") Integer appId,
                                                                      @Param("userId") Integer userId,
                                                                      @Param("customerUserId") Integer customerUserId,
                                                                      @Param("costCenterId") Integer costCenterId,
                                                                      @Param("bookingPersonId") Integer bookingPersonId);

    /** Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt - second result set (SELECT C.CompLogoImage FROM Company C WHERE C.Id = @CompanyId). */
    @Query(value = "SELECT C.CompLogoImage FROM Company C WHERE C.Id = :companyId", nativeQuery = true)
    byte[] companyLogoImage(@Param("companyId") Integer companyId);
}
