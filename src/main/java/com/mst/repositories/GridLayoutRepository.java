package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.GridLayout;

/**
 * dbo.GridLayout - Sp_GridLayout_Insert, USP_GridLayout_ReadById, USP_GridLayout_DeleteById.
 * Sp_GridLayout_Insert is "replace": it first DELETEs the row(s) with the same (Layout, OrganizationId, CompanyId)
 * ({@link #deleteByLayout} - identical to USP_GridLayout_DeleteById), then Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()} + 1),
 * EntryDate = GETDATE() (the @EntryDate parameter is ignored), then INSERT -> save(); the proc returns the new Id (SELECT @Id).
 * The read/delete procs match on Layout (nvarchar(150)), not on Id, despite their names.
 */
@Repository
public interface GridLayoutRepository extends JpaRepository<GridLayout, Integer> {

    /** Sp_GridLayout_Insert: SELECT ISNULL(MAX(convert(int,Id)),0) FROM GridLayout (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from GridLayout e")
    Integer maxId();

    /** USP_GridLayout_DeleteById (single branch) and the first statement of Sp_GridLayout_Insert. */
    @Modifying
    @Transactional
    @Query("DELETE FROM GridLayout g WHERE g.layout = :layout AND g.organizationId = :organizationId AND g.companyId = :companyId")
    int deleteByLayout(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("layout") String layout);

    /** USP_GridLayout_ReadById (single branch; all entity columns). */
    @Query("SELECT g FROM GridLayout g WHERE g.layout = :layout AND g.organizationId = :organizationId AND g.companyId = :companyId")
    List<GridLayout> readById(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("layout") String layout);
}
