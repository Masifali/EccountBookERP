package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.Brand;
import com.mst.projections.BrandFormHistoryRow;

/**
 * USP_Brand_InsertAndUpdate (procedural: @Id = 0 -> insert, else update; duplicate BrandCode / BrandName checks with
 * RAISERROR) -> {@link #spBrandInsertAndUpdate} (KEEP-EXEC). For simple cases: {@code save()} with Id = {@link #maxId()}+1
 * and entryDate = now (proc inserts EntryDate = GETDATE(); ModifyDate/ModifyUserId not inserted); duplicate checks via
 * {@link #countByCodeOrName}; update via {@link #updateById} (ModifyDate = GETDATE()). Proc returns @Id in both paths.
 * <p>
 * USP_Brand_GetAllMethod @Activity='DeleteById' (procedural: RAISERROR 'Cannot delete: Brand is referenced in Export Contract.'
 * when ExImLcOrderPackingDetail references the brand) -> {@link #spBrandDeleteById} (KEEP-EXEC), or
 * {@link #countExportContractReferences} + {@link #deleteBrandById}.
 */
@Repository
public interface BrandRepository extends JpaRepository<Brand, Integer> {

    /** USP_Brand_InsertAndUpdate (insert path): next app-assigned Id = maxId()+1 */
    @Query("select coalesce(max(b.id),0) from Brand b")
    Integer maxId();

    /**
     * USP_Brand_InsertAndUpdate duplicate checks: count of OTHER brands in the org/company with the same BrandCode
     * (pass brandName = null) or BrandName (pass brandCode = null). Pass excludeId = 0 for the insert path (Id &lt;&gt; 0 matches all).
     */
    @Query("SELECT COUNT(b) FROM Brand b WHERE b.organizationId = :organizationId AND b.companyId = :companyId "
         + "AND b.id <> :excludeId AND ((:brandCode IS NOT NULL AND b.brandCode = :brandCode) OR (:brandName IS NOT NULL AND b.brandName = :brandName))")
    Long countByCodeOrName(@Param("organizationId") Integer organizationId,
                           @Param("companyId") Integer companyId,
                           @Param("excludeId") Integer excludeId,
                           @Param("brandCode") String brandCode,
                           @Param("brandName") String brandName);

    /** USP_Brand_InsertAndUpdate (update path, @Id &lt;&gt; 0): the UPDATE statement; ModifyDate = GETDATE() */
    @Modifying
    @Transactional
    @Query("UPDATE Brand b SET b.brandCode = :brandCode, b.brandName = :brandName, b.isActive = :isActive, "
         + "b.modifyDate = CURRENT_TIMESTAMP, b.modifyUserId = :modifyUserId, b.organizationId = :organizationId, "
         + "b.companyId = :companyId WHERE b.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("brandCode") String brandCode,
                   @Param("brandName") String brandName,
                   @Param("isActive") Boolean isActive,
                   @Param("modifyUserId") Integer modifyUserId,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId);

    /**
     * USP_Brand_InsertAndUpdate (single branch, whole procedure). // KEEP-EXEC: insert-or-update with duplicate-code/name
     * RAISERROR checks. Defaults: all params NULL except @isActive (required). @EntryDate/@ModifyDate are ignored by the proc (GETDATE()).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_Brand_InsertAndUpdate @Id = :id, @BrandName = :brandName, @BrandCode = :brandCode, @isActive = :isActive, "
         + "@EntryDate = :entryDate, @EntryUserId = :entryUserId, @ModifyDate = :modifyDate, @ModifyUserId = :modifyUserId, "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId", nativeQuery = true)
    void spBrandInsertAndUpdate(@Param("id") Integer id,
                                @Param("brandName") String brandName,
                                @Param("brandCode") String brandCode,
                                @Param("isActive") Boolean isActive,
                                @Param("entryDate") java.time.LocalDateTime entryDate,
                                @Param("entryUserId") Integer entryUserId,
                                @Param("modifyDate") java.time.LocalDateTime modifyDate,
                                @Param("modifyUserId") Integer modifyUserId,
                                @Param("organizationId") Integer organizationId,
                                @Param("companyId") Integer companyId);

    /** USP_Brand_GetAllMethod @Activity='FormHistory' (native: UserAccount.UserTitle is not mapped on the entity). All filters optional (NULL = any). */
    @Query(value = "SELECT h.Id, h.BrandCode, h.BrandName, h.isActive AS isActive, h.EntryDate, h.EntryUserId, UE.UserTitle EntryUserName, "
         + "h.ModifyDate, h.ModifyUserId, UM.UserTitle ModifyUserName, h.OrganizationId, h.CompanyId "
         + "FROM dbo.Brand h Inner Join UserAccount UE on UE.ID = h.EntryUserId LEft Join UserAccount UM on UM.ID = h.ModifyUserId "
         + "WHERE (:organizationId IS NULL OR h.OrganizationId = :organizationId) "
         + "AND (:companyId IS NULL OR h.CompanyId = :companyId) "
         + "AND (:id IS NULL OR h.Id = :id)", nativeQuery = true)
    List<BrandFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId,
                                          @Param("id") Long id);

    /** USP_Brand_GetAllMethod @Activity='DeleteById': the guard (IF EXISTS ... ExImLcOrderPackingDetail, no entity -> native); > 0 means the proc raises and does not delete */
    @Query(value = "SELECT COUNT(1) FROM ExImLcOrderPackingDetail WHERE ItemBrandId = :id AND ISNULL(ActionTypeId, 0) <> 3", nativeQuery = true)
    Integer countExportContractReferences(@Param("id") Long id);

    /** USP_Brand_GetAllMethod @Activity='DeleteById': the DELETE statement */
    @Modifying
    @Transactional
    @Query("DELETE FROM Brand b WHERE b.id = :id")
    int deleteBrandById(@Param("id") Integer id);

    /** USP_Brand_GetAllMethod @Activity='DeleteById' (whole branch). // KEEP-EXEC: guard RAISERROR + DELETE in one call. @OrganizationId/@CompanyId unused by this branch. */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_Brand_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @Activity = 'DeleteById', @Id = :id", nativeQuery = true)
    void spBrandDeleteById(@Param("organizationId") Integer organizationId,
                           @Param("companyId") Integer companyId,
                           @Param("id") Long id);
}
