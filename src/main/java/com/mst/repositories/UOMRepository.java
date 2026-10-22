package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.UOM;
import com.mst.projections.ParentUomRow;
import com.mst.projections.UomRow;

/**
 * Sp_UOM_Insert -> {@code save()}. Pre-insert logic in the proc: Id = max(Id)+1 over the whole table ({@link #maxId()});
 * ModifyDate / ModifyUser are NOT inserted; proc returns @Id.
 */
@Repository
public interface UOMRepository extends JpaRepository<UOM, Integer> {

    /** Sp_UOM_Insert: next app-assigned Id = maxId()+1 */
    @Query("select coalesce(max(u.id),0) from UOM u")
    Integer maxId();

    /** Sp_UOM_GetAllMethod @Activity='ReadById' (native: ParentUOM has no entity) */
    @Query(value = "SELECT u.[Id], u.[UOMCode], u.[UOMDescription], u.[UOMSymbol], u.[UOMStatus], u.[EntryDate], u.[EntryUser], "
         + "u.[ModifyDate], u.[ModifyUser], u.[OrganizationId], u.[CompanyId], u.Equivalent, u.QtyEquivalent, "
         + "u.ParentUOMId, p.Description ParentUomCode "
         + "FROM [dbo].[UOM] u Left join ParentUOM p on p.Id = u.ParentUOMId where u.Id = :id", nativeQuery = true)
    List<UomRow> readById(@Param("id") Integer id);

    /** Sp_UOM_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native: ParentUOM has no entity; @CompanyId filter commented out in the proc) */
    @Query(value = "SELECT u.[Id], u.[UOMCode], u.[UOMDescription], u.[UOMSymbol], u.[UOMStatus], u.[EntryDate], u.[EntryUser], "
         + "u.[ModifyDate], u.[ModifyUser], u.[OrganizationId], u.[CompanyId], Equivalent, QtyEquivalent, "
         + "u.ParentUOMId, p.Description ParentUomCode "
         + "FROM [dbo].[UOM] u Left join ParentUOM p on p.Id = u.ParentUOMId where u.OrganizationId = :organizationId", nativeQuery = true)
    List<UomRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId);

    /** Sp_UOM_GetAllMethod @Activity='GetAllParentUOM' (native: ParentUOM has no entity) */
    @Query(value = "SELECT p.Id, p.Description, p.Remarks FROM ParentUOM p", nativeQuery = true)
    List<ParentUomRow> getAllParentUOM();

    /** Sp_UOM_Update (single branch): every SET column; EntryDate/EntryUser not updated (@EntryDate/@EntryUser unused). */
    @Modifying
    @Transactional
    @Query("UPDATE UOM u SET u.uomCode = :uomCode, u.uomDescription = :uomDescription, u.uomSymbol = :uomSymbol, "
         + "u.uomStatus = :uomStatus, u.modifyDate = :modifyDate, u.modifyUser = :modifyUser, "
         + "u.organizationId = :organizationId, u.companyId = :companyId, u.equivalent = :equivalent, "
         + "u.parentUOMId = :parentUOMId, u.qtyEquivalent = :qtyEquivalent "
         + "WHERE u.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("uomCode") String uomCode,
                   @Param("uomDescription") String uomDescription,
                   @Param("uomSymbol") String uomSymbol,
                   @Param("uomStatus") Boolean uomStatus,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("equivalent") Double equivalent,
                   @Param("parentUOMId") Integer parentUOMId,
                   @Param("qtyEquivalent") Double qtyEquivalent);
}
