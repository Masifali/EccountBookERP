package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemTypeMultiLingo;
import com.mst.projections.ItemTypeMultiLanguageHistoryRow;

/**
 * Sp_ItemTypeMultiLingo_Insert -> {@link #deleteByItemTypeIdAndMultiLanguagesId} then {@code save()}.
 * Pre-insert logic in the proc: deletes the existing row for (ItemTypeId, MultiLanguagesId) first;
 * Id is IDENTITY (proc returns SCOPE_IDENTITY()); @Id param is unused.
 */
@Repository
public interface ItemTypeMultiLingoRepository extends JpaRepository<ItemTypeMultiLingo, Integer> {

    /** Sp_ItemTypeMultiLingo_Insert: the DELETE that precedes the INSERT */
    @Modifying
    @Transactional
    @Query("DELETE FROM ItemTypeMultiLingo m WHERE m.itemTypeId = :itemTypeId AND m.multiLanguagesId = :multiLanguagesId")
    int deleteByItemTypeIdAndMultiLanguagesId(@Param("itemTypeId") Integer itemTypeId,
                                              @Param("multiLanguagesId") Integer multiLanguagesId);

    /** usp_ItemTypeMultiLanguage_History (single branch); @LanguageId default null = all languages */
    @Query("SELECT ic.id AS id, ic.itemTypeId AS itemTypeId, c.typeDescription AS typeDescription, "
         + "ic.typeDescription AS typeMultiLingo, ic.multiLanguagesId AS multiLanguagesId, ml.languageName AS languageName "
         + "FROM ItemTypeMultiLingo ic JOIN MultiLanguages ml ON ic.multiLanguagesId = ml.id "
         + "JOIN ItemType c ON ic.itemTypeId = c.id "
         + "WHERE (:languageId IS NULL OR ic.multiLanguagesId = :languageId)")
    List<ItemTypeMultiLanguageHistoryRow> history(@Param("languageId") Integer languageId);
}
