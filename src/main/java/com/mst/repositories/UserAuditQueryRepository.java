package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

/**
 * USP_UserAudit_Insert (dbo.UserAudit - entity owned by batch F2_grn_support, so this is a query-only repository bound to
 * LoginUserAndAppInfo). The proc is called by Proc_ChartofAccount_Insert, USP_DriverBiodata_InsertIfNotExists and
 * USP_driverBiodata_GetAllMethod 'DeleteById' in this batch.
 * Logic: when @RefDocumentTypeId > 0 and @ScreenName is empty, ScreenName = DocumentType.ScreenName of that id
 * ({@link #screenNameOfDocumentType}); Id = ISNULL(MAX(Id),0)+1 ({@link #maxUserAuditId()}); then INSERT.
 */
@Repository
public interface UserAuditQueryRepository extends org.springframework.data.repository.Repository<com.mst.models.LoginUserAndAppInfo, Integer> {

    /** USP_UserAudit_Insert (single branch) - KEEP-EXEC. @ReferenceId, @OrganizationId, @CompanyId, @ActionId are required; the rest default to NULL. */
    // KEEP-EXEC: DocumentType screen-name lookup + MAX(Id)+1 + INSERT into UserAudit (no owned entity)
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_UserAudit_Insert @Id = :id, @ActivityDate = :activityDate, @UserId = :userId, @ClientIP = :clientIP, "
         + "@ActivityDescription = :activityDescription, @RefDocumentTypeId = :refDocumentTypeId, @ReferenceId = :referenceId, "
         + "@ScreenName = :screenName, @OrganizationId = :organizationId, @CompanyId = :companyId, @ActionId = :actionId", nativeQuery = true)
    void uspUserAuditInsert(@Param("id") Long id, @Param("activityDate") LocalDateTime activityDate, @Param("userId") Integer userId,
            @Param("clientIP") String clientIP, @Param("activityDescription") String activityDescription,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("referenceId") Long referenceId, @Param("screenName") String screenName,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("actionId") Integer actionId);

    /** USP_UserAudit_Insert step 1: SELECT ScreenName FROM DocumentType WHERE Id = @RefDocumentTypeId. */
    @Query("SELECT d.screenName FROM DocumentType d WHERE d.id = :refDocumentTypeId")
    List<String> screenNameOfDocumentType(@Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** USP_UserAudit_Insert step 2: SELECT ISNULL(MAX(CONVERT(BIGINT,Id)),0) FROM UserAudit (caller adds 1). */
    @Query(value = "SELECT ISNULL(MAX(CONVERT(BIGINT, Id)), 0) FROM UserAudit", nativeQuery = true)
    Long maxUserAuditId();
}
