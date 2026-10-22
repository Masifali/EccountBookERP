package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemType;
import com.mst.projections.ItemTypeByOrganizationRow;
import com.mst.projections.ItemTypeRow;
import com.mst.projections.MotherItemRow;

/**
 * Sp_ItemType_Insert -> {@code save()}. Pre-insert logic in the proc:
 * 1) if dbo.GetConfigurationByConfigId(@OrganizationId, @CompanyId, 410) = 1 ({@link #itemCodingEnabled}) then
 *    TypeCode = max(TRY_CAST(TypeCode AS INT))+1 within OrganizationId/CompanyId as varchar ({@link #generateCode});
 * 2) Id = max(Id)+1 over the whole table ({@link #maxId()});
 * 3) ModifyDate / ModifyUser are NOT inserted; proc returns @Id.
 */
@Repository
public interface ItemTypeRepository extends JpaRepository<ItemType, Integer> {

    /** Sp_ItemType_Insert: next app-assigned Id = maxId()+1 */
    @Query("select coalesce(max(t.id),0) from ItemType t")
    Integer maxId();

    /** Sp_ItemType_Insert: DECLARE @ItemCodingEnable = ISNULL(dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,410),0) (1 = auto code) */
    @Query(value = "SELECT CAST(ISNULL(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 410), 0) AS int)", nativeQuery = true)
    Integer itemCodingEnabled(@Param("organizationId") Integer organizationId,
                              @Param("companyId") Integer companyId);

    /** Sp_ItemType_GetAllMethod @Activity='GenerateCode' (also the code step of Sp_ItemType_Insert); returns a single TypeCode string */
    @Query(value = "SELECT CAST(ISNULL(MAX(TRY_CAST(TypeCode AS INT)), 0) + 1 AS VARCHAR(10)) AS TypeCode "
         + "FROM dbo.ItemType WHERE OrganizationId = :organizationId AND CompanyId = :companyId", nativeQuery = true)
    String generateCode(@Param("organizationId") Integer organizationId,
                        @Param("companyId") Integer companyId);

    /** Sp_ItemType_GetAllMethod @Activity='ReadById' */
    @Query("SELECT t.id AS id, t.typeCode AS typeCode, t.typeDescription AS typeDescription, t.type AS type, "
         + "t.entryDate AS entryDate, t.entryUser AS entryUser, t.modifyDate AS modifyDate, t.modifyUser AS modifyUser, "
         + "t.postDate AS postDate, t.postUser AS postUser, t.postState AS postState, t.organizationId AS organizationId, "
         + "t.companyId AS companyId, t.parentCategoryId AS parentCategoryId, t.isMother AS isMother "
         + "FROM ItemType t WHERE t.id = :id")
    List<ItemTypeRow> readById(@Param("id") Integer id);

    /**
     * Sp_ItemType_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native: InventoryParentCategories has no entity,
     * dbo.fnSplitString). @Type default null = all; @ParentCategoryIds default null = all (comma-separated list otherwise).
     */
    @Query(value = "SELECT distinct T.[Id], T.[TypeCode], T.[TypeDescription], T.[Type], T.[EntryDate], T.[EntryUser], "
         + "T.[ModifyDate], T.[ModifyUser], T.[PostDate], T.[PostUser], T.[PostState], T.[OrganizationId], T.[CompanyId], "
         + "0 as ItemCategoryId, T.ParentCategoryId, P.InvParentCateDescription, l.LookupName, T.IsMother, "
         + "CASE WHEN T.IsMother = 1 THEN 'Mother' ELSE 'Not Mother' END AS MotherStatus "
         + "FROM [dbo].[ItemType] T "
         + "Left join Item i on T.Id = i.ItemTypeId "
         + "Left join InvLookUp l on T.Type = l.Id "
         + "Left Join InventoryParentCategories P on P.Id = T.ParentCategoryId "
         + "where T.OrganizationId = :organizationId and T.CompanyId = :companyId "
         + "and (:type is null or T.Type = :type) "
         + "and (:parentCategoryIds IS NULL OR EXISTS (select 1 from dbo.fnSplitString (:parentCategoryIds, ',') Where T.ParentCategoryId in (data)))",
         nativeQuery = true)
    List<ItemTypeByOrganizationRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId,
                                                                @Param("companyId") Integer companyId,
                                                                @Param("type") Integer type,
                                                                @Param("parentCategoryIds") String parentCategoryIds);

    /** usp_getMotherItems (single branch): item types flagged IsMother = 1 */
    @Query("SELECT t.id AS id, t.typeDescription AS motherItem FROM ItemType t "
         + "WHERE t.organizationId = :organizationId AND t.companyId = :companyId AND t.isMother = true")
    List<MotherItemRow> getMotherItems(@Param("organizationId") Integer organizationId,
                                       @Param("companyId") Integer companyId);

    /** Sp_ItemType_Update (single branch): every SET column; EntryDate/EntryUser are not updated (@EntryDate/@EntryUser unused). */
    @Modifying
    @Transactional
    @Query("UPDATE ItemType t SET t.typeCode = :typeCode, t.typeDescription = :typeDescription, t.type = :type, "
         + "t.modifyDate = :modifyDate, t.modifyUser = :modifyUser, t.postDate = :postDate, t.postUser = :postUser, "
         + "t.postState = :postState, t.organizationId = :organizationId, t.companyId = :companyId, "
         + "t.parentCategoryId = :parentCategoryId, t.isMother = :isMother "
         + "WHERE t.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("typeCode") String typeCode,
                   @Param("typeDescription") String typeDescription,
                   @Param("type") Integer type,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("postDate") LocalDateTime postDate,
                   @Param("postUser") Integer postUser,
                   @Param("postState") Boolean postState,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("parentCategoryId") Integer parentCategoryId,
                   @Param("isMother") Boolean isMother);
}
