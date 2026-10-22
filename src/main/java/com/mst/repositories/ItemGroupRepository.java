package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemGroup;
import com.mst.projections.ItemGroupReadByIdRow;
import com.mst.projections.ItemGroupRow;

/**
 * Sp_ItemGroup_Insert -> {@code save()}. Pre-insert logic in the proc: GroupId = max(GroupId)+1 over the whole
 * table (use {@link #maxId()}); proc returns the new @GroupId.
 * Note: the existing entity maps column GroupId onto field {@code id}.
 */
@Repository
public interface ItemGroupRepository extends JpaRepository<ItemGroup, Integer> {

    /** Sp_ItemGroup_Insert: next app-assigned GroupId = maxId()+1 */
    @Query("select coalesce(max(g.id),0) from ItemGroup g")
    Integer maxId();

    /** Sp_ItemGroup_GetAllMethod @Activity='ReadAll' */
    @Query("SELECT g.id AS groupId, g.itemGroupName AS itemGroupName FROM ItemGroup g "
         + "WHERE g.organizationId = :organizationId AND g.companyId = :companyId")
    List<ItemGroupRow> readAll(@Param("organizationId") Integer organizationId,
                               @Param("companyId") Integer companyId);

    /** Sp_ItemGroup_GetAllMethod @Activity='ReadById' */
    @Query("SELECT g.id AS groupId, g.itemGroupName AS itemGroupName, g.organizationId AS organizationId, g.companyId AS companyId "
         + "FROM ItemGroup g WHERE g.id = :groupId")
    List<ItemGroupReadByIdRow> readById(@Param("groupId") Integer groupId);

    /** Sp_ItemGroup_Update (single branch) */
    @Modifying
    @Transactional
    @Query("UPDATE ItemGroup g SET g.itemGroupName = :itemGroupName, g.organizationId = :organizationId, g.companyId = :companyId "
         + "WHERE g.id = :groupId")
    int updateByGroupId(@Param("groupId") Integer groupId,
                        @Param("itemGroupName") String itemGroupName,
                        @Param("organizationId") Integer organizationId,
                        @Param("companyId") Integer companyId);
}
