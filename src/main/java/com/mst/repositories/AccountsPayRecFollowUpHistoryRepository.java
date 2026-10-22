package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.AccountsPayRecFollowUpHistory;
import com.mst.projections.AccountsPayRecFollowUpHistoryRow;

/**
 * dbo.AccountsPayRecFollowUpHistory - Sp_AccountsPayRecFollowUpHistory_GetAllMethod / _Insert / _Update.
 * Insert proc: single-table INSERT -> save(); pre-insert logic: Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 ({@link #maxId()} + 1);
 * the proc returns the new Id (SELECT @Id).
 */
@Repository
public interface AccountsPayRecFollowUpHistoryRepository extends JpaRepository<AccountsPayRecFollowUpHistory, Integer> {

    /** Sp_AccountsPayRecFollowUpHistory_GetAllMethod @Activity='ReadAll' */
    @Query("SELECT rc.id AS id, rc.documentTypeIdRef AS documentTypeIdRef, rc.chartOfAccountId AS chartOfAccountId, rc.commentsDetail AS commentsDetail, "
         + "rc.commentsDate AS commentsDate, rc.followupDate AS followupDate, rc.nextFollowupDays AS nextFollowupDays, rc.promiseDate AS promiseDate, "
         + "rc.entryDate AS entryDate, rc.entryUser AS entryUser, rc.organizationId AS organizationId, rc.companyId AS companyId, c.accountTitle AS accountTitle "
         + "FROM AccountsPayRecFollowUpHistory rc LEFT JOIN ChartofAccount c ON rc.chartOfAccountId = c.id "
         + "WHERE rc.organizationId = :organizationId AND rc.companyId = :companyId")
    List<AccountsPayRecFollowUpHistoryRow> readAll(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_AccountsPayRecFollowUpHistory_Insert: SELECT ISNULL(MAX(CONVERT(int,Id)),0) (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from AccountsPayRecFollowUpHistory e")
    Integer maxId();

    /** Sp_AccountsPayRecFollowUpHistory_Update (single branch; EntryDate is NOT updated by the proc). */
    @Modifying
    @Transactional
    @Query("UPDATE AccountsPayRecFollowUpHistory t SET t.documentTypeIdRef = :documentTypeIdRef, t.chartOfAccountId = :chartOfAccountId, "
         + "t.commentsDetail = :commentsDetail, t.commentsDate = :commentsDate, t.followupDate = :followupDate, t.nextFollowupDays = :nextFollowupDays, "
         + "t.promiseDate = :promiseDate, t.entryUser = :entryUser, t.organizationId = :organizationId, t.companyId = :companyId "
         + "WHERE t.id = :id")
    int updateById(@Param("id") Integer id, @Param("documentTypeIdRef") Integer documentTypeIdRef, @Param("chartOfAccountId") Integer chartOfAccountId,
            @Param("commentsDetail") String commentsDetail, @Param("commentsDate") LocalDateTime commentsDate, @Param("followupDate") LocalDateTime followupDate,
            @Param("nextFollowupDays") Integer nextFollowupDays, @Param("promiseDate") LocalDateTime promiseDate, @Param("entryUser") Integer entryUser,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);
}
