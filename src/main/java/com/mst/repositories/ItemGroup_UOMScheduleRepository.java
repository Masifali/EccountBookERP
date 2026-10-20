package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemGroup_UOMSchedule;
import com.mst.projections.ItemGroupUomScheduleReadAllRow;
import com.mst.projections.ItemGroupUomScheduleRow;

/**
 * Sp_ItemGroup_UOMSchedule_Insert -> {@code save()}. Pre-insert logic in the proc: Id = max(Id)+1 over the whole
 * table ({@link #maxId()}); then RAISERROR 'Record Not Insert beacuase Record has already Exist Against this Uom'
 * when a row with the same (OrganizationId, CompanyId, ItemGroupId, UOMId) exists ({@link #countSchedule} &gt; 0); proc returns @Id.
 * <p>
 * Sp_ItemGroup_UOMSchedule_Update -> {@link #updateById}. Pre-update logic in the proc: reads the previous UOMId
 * ({@link #findUomIdById}); if it differs from @UOMId and a row with (OrganizationId, CompanyId, ItemGroupId, previous UOMId)
 * exists, RAISERROR 'Record Not Update beacuase Record has already Exist' (proc checks @PreUomId, verbatim).
 */
@Repository
public interface ItemGroup_UOMScheduleRepository extends JpaRepository<ItemGroup_UOMSchedule, Integer> {

    /** Sp_ItemGroup_UOMSchedule_Insert: next app-assigned Id = maxId()+1 */
    @Query("select coalesce(max(s.id),0) from ItemGroup_UOMSchedule s")
    Integer maxId();

    /** Sp_ItemGroup_UOMSchedule_Insert / _Update: the duplicate-existence check (IF EXISTS ... -> count > 0) */
    @Query("SELECT COUNT(s) FROM ItemGroup_UOMSchedule s WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND s.itemGroupId = :itemGroupId AND s.uomId = :uomId")
    Long countSchedule(@Param("organizationId") Integer organizationId,
                           @Param("companyId") Integer companyId,
                           @Param("itemGroupId") Integer itemGroupId,
                           @Param("uomId") Integer uomId);

    /** Sp_ItemGroup_UOMSchedule_Update: SELECT @PreUomId = UOMId ... WHERE Id = @Id */
    @Query("SELECT s.uomId FROM ItemGroup_UOMSchedule s WHERE s.id = :id")
    Integer findUomIdById(@Param("id") Integer id);

    /** Sp_ItemGroup_UOMSchedule_GetAllMethod @Activity='ReadAll' */
    @Query("SELECT s.id AS id, s.itemGroupId AS itemGroupId, s.uomId AS uomId, s.equivalent AS equivalent, "
         + "g.itemGroupName AS itemGroupName, u.uomCode AS scheduleUnit "
         + "FROM ItemGroup_UOMSchedule s JOIN ItemGroup g ON s.itemGroupId = g.id JOIN UOM u ON s.uomId = u.id "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId")
    List<ItemGroupUomScheduleReadAllRow> readAll(@Param("organizationId") Integer organizationId,
                                                 @Param("companyId") Integer companyId);

    /** Sp_ItemGroup_UOMSchedule_GetAllMethod @Activity='ReadById' */
    @Query("SELECT s.id AS id, s.itemGroupId AS itemGroupId, s.uomId AS uomId, s.equivalent AS equivalent, "
         + "s.organizationId AS organizationId, s.companyId AS companyId "
         + "FROM ItemGroup_UOMSchedule s WHERE s.id = :id")
    List<ItemGroupUomScheduleRow> readById(@Param("id") Integer id);

    /** Sp_ItemGroup_UOMSchedule_Update (single branch): the UPDATE statement with every SET column */
    @Modifying
    @Transactional
    @Query("UPDATE ItemGroup_UOMSchedule s SET s.itemGroupId = :itemGroupId, s.uomId = :uomId, s.equivalent = :equivalent, "
         + "s.organizationId = :organizationId, s.companyId = :companyId WHERE s.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("itemGroupId") Integer itemGroupId,
                   @Param("uomId") Integer uomId,
                   @Param("equivalent") String equivalent,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId);
}
