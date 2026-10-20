package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.LoginUserAndAppInfo;

/**
 * dbo.LoginUserAndAppInfo - the upsert at the top of USP_GetUserRightsForViewbyUserId (menu-rights-procedure.sql):
 * when @AppId > 0 (NULL/0 is first replaced by 1) and a row for (CompanyId, UserId) exists it is updated
 * ({@link #updateAppId}), else a new row is inserted with Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()} + 1) and EntryDate = GETDATE()
 * -> save(). The SELECT branches of that proc are in UserRightsQueryRepository.
 */
@Repository
public interface LoginUserAndAppInfoRepository extends JpaRepository<LoginUserAndAppInfo, Integer> {

    /** USP_GetUserRightsForViewbyUserId: SELECT ISNULL(MAX(Id),0) FROM LoginUserAndAppInfo (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from LoginUserAndAppInfo e")
    Integer maxId();

    /** USP_GetUserRightsForViewbyUserId: existence check for (CompanyId, UserId). */
    @Query("SELECT COUNT(r) FROM LoginUserAndAppInfo r WHERE r.companyId = :companyId AND r.userId = :userId")
    Long countByCompanyIdAndUserId(@Param("companyId") Integer companyId, @Param("userId") Integer userId);

    /** USP_GetUserRightsForViewbyUserId: UPDATE LoginUserAndAppInfo SET AppId=@AppId, EntryDate=GETDATE() WHERE CompanyId=@CompanyId AND UserId=@UserId. */
    @Modifying
    @Transactional
    @Query("UPDATE LoginUserAndAppInfo r SET r.appId = :appId, r.entryDate = CURRENT_TIMESTAMP WHERE r.companyId = :companyId AND r.userId = :userId")
    int updateAppId(@Param("companyId") Integer companyId, @Param("userId") Integer userId, @Param("appId") Integer appId);
}
