package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvCropYear;
import com.mst.projections.InvCropYearReadAllRow;

/**
 * Sp_InvCropYear_Insert -> {@code save()} (Id is IDENTITY, proc returns SCOPE_IDENTITY()).
 * Pre-insert logic in the proc: EntryDate = GETDATE(); ModifyUserId / ModifyDate are NOT inserted
 * (caller must set entryDate = LocalDateTime.now() before save()).
 */
@Repository
public interface InvCropYearRepository extends JpaRepository<InvCropYear, Integer> {

    /** Sp_InvCropYear_GetAllMethod @Activity='ReadById' (SELECT *) */
    @Query("SELECT c FROM InvCropYear c WHERE c.id = :id")
    List<InvCropYear> readById(@Param("id") Integer id);

    /** Sp_InvCropYear_GetAllMethod @Activity='ReadAll' */
    @Query("SELECT h.id AS id, h.cropYear AS cropYear, h.defaultKey AS isActive, "
         + "CONCAT(usr.firstName, ' ', usr.lastName) AS entryUserName, h.entryDate AS entryDate, "
         + "CONCAT(usm.firstName, ' ', usm.lastName) AS modifyUserName, h.modifyDate AS modifyDate "
         + "FROM InvCropYear h LEFT JOIN UserAccount usr ON h.entryUserId = usr.id "
         + "LEFT JOIN UserAccount usm ON h.modifyUserId = usm.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId")
    List<InvCropYearReadAllRow> readAll(@Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId);

    /** Sp_InvCropYear_GetAllMethod @Activity='GetAll' (SELECT *; "New Services For FIFO") */
    @Query("SELECT c FROM InvCropYear c WHERE c.organizationId = :organizationId AND c.companyId = :companyId")
    List<InvCropYear> getAll(@Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId);

    /** Sp_InvCropYear_Update (single branch): ModifyDate = GETDATE() in the proc; @EntryUserId/@EntryDate/@ModifyDate params are unused. */
    @Modifying
    @Transactional
    @Query("UPDATE InvCropYear c SET c.cropYear = :cropYear, c.defaultKey = :defaultKey, c.organizationId = :organizationId, "
         + "c.companyId = :companyId, c.modifyUserId = :modifyUserId, c.modifyDate = CURRENT_TIMESTAMP "
         + "WHERE c.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("cropYear") String cropYear,
                   @Param("defaultKey") Boolean defaultKey,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("modifyUserId") Integer modifyUserId);
}
