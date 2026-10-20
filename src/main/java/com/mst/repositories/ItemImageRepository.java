package com.mst.repositories;

import java.time.LocalDateTime;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemImage;

/**
 * USP_ItemImage_Insert. Insert branch (ISNULL(@Id,0)=0): Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}), INSERT all 8 columns, SELECT @Id —
 * callers use save(). Update branch (@Id &gt; 0): {@link #updateById}. @IsActive is mandatory; all other params default NULL.
 */
@Repository
public interface ItemImageRepository extends JpaRepository<ItemImage, Integer> {

    /** USP_ItemImage_Insert: ISNULL(MAX(CONVERT(INT,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemImage e")
    Integer maxId();

    /** USP_ItemImage_Insert update branch (@Id &gt; 0) — every SET column of the proc. */
    @Modifying
    @Transactional
    @Query("UPDATE ItemImage e SET e.itemId = :itemId, e.imagePath = :imagePath, e.effetedDate = :effetedDate, e.isActive = :isActive, "
         + "e.fileName = :fileName, e.isProfileImage = :isProfileImage, e.sortNo = :sortNo WHERE e.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("itemId") Integer itemId,
                   @Param("imagePath") String imagePath,
                   @Param("effetedDate") LocalDateTime effetedDate,
                   @Param("isActive") Boolean isActive,
                   @Param("fileName") String fileName,
                   @Param("isProfileImage") Boolean isProfileImage,
                   @Param("sortNo") Integer sortNo);
}
