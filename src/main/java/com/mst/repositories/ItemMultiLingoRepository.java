package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemMultiLingo;
import com.mst.projections.ItemMultiLingoReadAllRow;

/**
 * Sp_ItemMultiLingo_Insert -> {@link #deleteByItemIdAndMultiLanguagesId} then {@code save()}.
 * Pre-insert logic in the proc: deletes the existing row for (ItemId, MultiLanguagesId) first;
 * Id is IDENTITY (proc returns SCOPE_IDENTITY()); @Id param is unused.
 */
@Repository
public interface ItemMultiLingoRepository extends JpaRepository<ItemMultiLingo, Integer> {

    /** Sp_ItemMultiLingo_Insert: the DELETE that precedes the INSERT */
    @Modifying
    @Transactional
    @Query("DELETE FROM ItemMultiLingo m WHERE m.itemId = :itemId AND m.multiLanguagesId = :multiLanguagesId")
    int deleteByItemIdAndMultiLanguagesId(@Param("itemId") Integer itemId,
                                          @Param("multiLanguagesId") Integer multiLanguagesId);

    /** USP_ItemMultiLingo_ReadAll (single branch); @MultiLanguagesId default null = all languages */
    @Query("SELECT i.id AS id, i.itemId AS itemId, i.multiLanguagesId AS multiLanguagesId, m.languageDescription AS languageName, "
         + "it.itemName AS itemName, i.itemName AS otherLanguageTitle "
         + "FROM ItemMultiLingo i JOIN MultiLanguages m ON i.multiLanguagesId = m.id "
         + "JOIN Item it ON i.itemId = it.id "
         + "WHERE (:multiLanguagesId IS NULL OR i.multiLanguagesId = :multiLanguagesId)")
    List<ItemMultiLingoReadAllRow> readAll(@Param("multiLanguagesId") Integer multiLanguagesId);
}
