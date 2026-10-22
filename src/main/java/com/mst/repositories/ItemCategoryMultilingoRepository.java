package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemCategoryMultilingo;
import com.mst.projections.ItemCategoryMultiLanguageHistoryRow;

/**
 * Sp_ItemCategoryMultilingo_Insert -> {@link #deleteByItemCategoryIdAndMultiLanguagesId} then {@code save()}.
 * Pre-insert logic in the proc: deletes the existing row for (ItemCategoryId, MultiLanguagesId) first;
 * Id is IDENTITY (proc returns SCOPE_IDENTITY()); @Id param is unused.
 */
@Repository
public interface ItemCategoryMultilingoRepository extends JpaRepository<ItemCategoryMultilingo, Integer> {

    /** Sp_ItemCategoryMultilingo_Insert: the DELETE that precedes the INSERT */
    @Modifying
    @Transactional
    @Query("DELETE FROM ItemCategoryMultilingo m WHERE m.itemCategoryId = :itemCategoryId AND m.multiLanguagesId = :multiLanguagesId")
    int deleteByItemCategoryIdAndMultiLanguagesId(@Param("itemCategoryId") Integer itemCategoryId,
                                                  @Param("multiLanguagesId") Integer multiLanguagesId);

    /** usp_ItemCategoryMultiLanguage_History (single branch); @LanguageId default null = all languages */
    @Query("SELECT ic.id AS id, ic.itemCategoryId AS itemCategoryId, c.categoryDescription AS categoryDescription, "
         + "ic.categoryDescription AS categoryMultiLingo, ic.multiLanguagesId AS multiLanguagesId, ml.languageName AS languageName "
         + "FROM ItemCategoryMultilingo ic JOIN MultiLanguages ml ON ic.multiLanguagesId = ml.id "
         + "JOIN ItemCategory c ON ic.itemCategoryId = c.id "
         + "WHERE (:languageId IS NULL OR ic.multiLanguagesId = :languageId)")
    List<ItemCategoryMultiLanguageHistoryRow> history(@Param("languageId") Integer languageId);
}
