package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.projections.UserRightsViewRow;

/**
 * USP_GetUserRightsForViewbyUserId (menu-rights-procedure.sql) - the "View" rights menu for a user. Query-only repository
 * bound to LoginUserAndAppInfo (the one table the proc writes; see LoginUserAndAppInfoRepository for that upsert).
 *
 * Branch logic of the proc (callers that do not use {@link #uspGetUserRightsForViewbyUserId} reproduce it):
 *  1. @AppId NULL/0 -> 1; upsert LoginUserAndAppInfo (AppId, EntryDate = now) for (CompanyId, UserId).
 *  2. If the user is an active SuperUsers row for the company ({@link #superUserId}) -> {@link #superUserRights}.
 *  3. Else if @AppId = 1 (desktop): if Company.CompanyId of @CompanyId is NULL/0 ({@link #companyIdOfCompany}) -> {@link #desktopRightsWithoutCompanyTemplate},
 *     else -> {@link #desktopRightsWithCompanyTemplate}.
 *  4. Else if @AppId IN (4,5,6) (portal apps) -> {@link #portalRights}.
 *  5. Else -> {@link #otherAppRights}.
 * All SELECT branches read ScreenDefinition / AppModules / App / ScreenRights / tblUserRights / CompanyRights / FavoriteScreens /
 * ScreensAllocateToCompany / ModuleAllocateToOrganizationTemplates / InActiveScreenOfTemplatesModule / ApplicationsAllocateToScreen,
 * most without entity -> native.
 */
@Repository
public interface UserRightsQueryRepository extends org.springframework.data.repository.Repository<com.mst.models.LoginUserAndAppInfo, Integer> {

    /** USP_GetUserRightsForViewbyUserId - KEEP-EXEC (upsert of LoginUserAndAppInfo + the branch selection above). @AppId / @AppModuleId default NULL. */
    // KEEP-EXEC: writes LoginUserAndAppInfo then returns one of five result shapes chosen by SuperUsers / @AppId / Company.CompanyId
    @Transactional
    @Query(value = "EXEC dbo.USP_GetUserRightsForViewbyUserId @UserId = :userId, @CompanyId = :companyId, @AppId = :appId, @AppModuleId = :appModuleId", nativeQuery = true)
    List<UserRightsViewRow> uspGetUserRightsForViewbyUserId(@Param("userId") Integer userId, @Param("companyId") Integer companyId,
            @Param("appId") Integer appId, @Param("appModuleId") Integer appModuleId);

    /** Step 2 helper: SELECT Id FROM SuperUsers WHERE UserId=@UserId AND CompanyId=@CompanyId AND ISNULL(IsActive,0)=1. */
    @Query(value = "SELECT Id FROM SuperUsers WHERE UserId = :userId AND CompanyId = :companyId AND ISNULL(IsActive, 0) = 1", nativeQuery = true)
    List<Integer> superUserId(@Param("userId") Integer userId, @Param("companyId") Integer companyId);

    /** Step 3 helper: SELECT CompanyId FROM Company WHERE Id = @CompanyId. */
    @Query("SELECT c.companyId FROM Company c WHERE c.id = :companyId")
    List<Integer> companyIdOfCompany(@Param("companyId") Integer companyId);

    /** USP_GetUserRightsForViewbyUserId, super-user branch: every active screen with a TargetUrl, Value = 1, RightsID = 0. */
    @Query(value = "SELECT 0 AS Id, M.Id AS ModuleID, M.ModuleDescription, sd.Id AS ScreenID, sd.ScreenName, ScreenAlias, :userId AS UserId, 0 AS RightsID, "
         + "'View' AS RightName, CAST(1 AS BIT) AS Value, sd.MenuControllName, M.ModelMenuControllName, sd.TargetUrl, M.IconUrl, m.ModuleTypeId, m.AppId, a.App, "
         + "CAST(CASE WHEN ISNULL(f.Id, 0) = 0 THEN 0 ELSE 1 END AS BIT) IsFavorite, '' AS TblUserRightslst, :companyId AS CompanyId "
         + "FROM ScreenDefinition sd "
         + "INNER JOIN AppModules M ON sd.ModuleId = M.Id "
         + "INNER JOIN App a ON m.AppId = a.Id "
         + "INNER JOIN Company C ON C.Id = :companyId "
         + "LEFT JOIN FavoriteScreens f ON f.ScreenName = sd.ScreenName AND f.UserId = :userId "
         + "WHERE (ISNULL(SD.TargetUrl, '') <> '') AND (ISNULL(sd.IsActive, 0) = 1) "
         + "ORDER BY a.SortNo, M.SortNo, sd.SortNo", nativeQuery = true)
    List<UserRightsViewRow> superUserRights(@Param("userId") Integer userId, @Param("companyId") Integer companyId);

    /** USP_GetUserRightsForViewbyUserId, @AppId=1 and Company.CompanyId NULL/0: granted 'View' rights joined with CompanyRights (@AppModuleId optional). */
    @Query(value = "SELECT 0 AS Id, M.Id AS ModuleID, M.ModuleDescription, sd.Id AS ScreenID, sd.ScreenName, ScreenAlias, UR.UserId, sr.Id AS RightsID, sr.RightName, "
         + "ISNULL(UR.Value, 0) AS Value, sd.MenuControllName, M.ModelMenuControllName, sd.TargetUrl, M.IconUrl, m.ModuleTypeId, m.AppId, a.App, "
         + "CAST(CASE WHEN ISNULL(f.Id, 0) = 0 THEN 0 ELSE 1 END AS BIT) IsFavorite, '' AS TblUserRightslst, :companyId AS CompanyId "
         + "FROM ScreenDefinition sd "
         + "INNER JOIN AppModules M ON sd.ModuleId = M.Id "
         + "INNER JOIN App a ON m.AppId = a.Id "
         + "INNER JOIN ScreenRights sr ON sd.Id = sr.ScreenID "
         + "INNER JOIN (SELECT DISTINCT ScreenId, RightId, Value, UserId FROM tblUserRights WHERE UserId = :userId AND CompanyId = :companyId) UR "
         + "ON UR.ScreenId = sd.Id AND UR.RightId = sr.Id AND Value = 1 "
         + "LEFT JOIN FavoriteScreens f ON f.ScreenName = sd.ScreenName AND f.UserId = :userId "
         + "INNER JOIN CompanyRights CRT ON CRT.ScreenId = sr.ScreenID AND CRT.CompanyId = :companyId "
         + "WHERE sr.RightName = 'View' AND (ISNULL(SD.TargetUrl, '') <> '') AND (ISNULL(sd.IsActive, 0) = 1) AND (ISNULL(CRT.IsActive, 0) = 1) "
         + "AND (:appModuleId IS NULL OR m.Id = :appModuleId) "
         + "ORDER BY a.SortNo, M.SortNo, sd.SortNo", nativeQuery = true)
    List<UserRightsViewRow> desktopRightsWithoutCompanyTemplate(@Param("userId") Integer userId, @Param("companyId") Integer companyId,
            @Param("appModuleId") Integer appModuleId);

    /** USP_GetUserRightsForViewbyUserId, @AppId=1 and Company.CompanyId set: rights restricted by ScreensAllocateToCompany and the company template's modules (@AppModuleId optional). */
    @Query(value = "SELECT 0 AS Id, M.Id AS ModuleID, M.ModuleDescription, sd.Id AS ScreenID, sd.ScreenName, sd.ScreenAlias, UR.UserId, sr.Id AS RightsID, sr.RightName, "
         + "ISNULL(UR.Value, 0) AS Value, sd.MenuControllName, M.ModelMenuControllName, sd.TargetUrl, M.IconUrl, m.ModuleTypeId, m.AppId, a.App, "
         + "CAST(CASE WHEN COALESCE(f.Id, 0) > 0 THEN 1 ELSE 0 END AS BIT) AS IsFavorite, '' AS TblUserRightslst, :companyId AS CompanyId "
         + "FROM ScreenDefinition sd "
         + "INNER JOIN AppModules M ON sd.ModuleId = M.Id "
         + "INNER JOIN App a ON M.AppId = a.Id "
         + "INNER JOIN ScreenRights sr ON sd.Id = sr.ScreenID "
         + "INNER JOIN Company C ON C.Id = :companyId "
         + "INNER JOIN (SELECT ScreenId, RightId, Value, UserId FROM tblUserRights WHERE UserId = :userId AND CompanyId = :companyId AND Value = 1 "
         + "AND (:appId IS NULL OR AppId = :appId)) UR ON UR.ScreenId = sd.Id AND UR.RightId = sr.Id "
         + "INNER JOIN (SELECT DISTINCT ScreenName FROM ScreensAllocateToCompany WHERE CompanyId = :companyId AND ISNULL(IsActive, 0) = 1) SAL ON SD.ScreenName = SAL.ScreenName "
         + "INNER JOIN ModuleAllocateToOrganizationTemplates T ON C.CompanyTemplateId = T.OrganizationTemplateId AND T.ModuleId = SD.ModuleId AND T.IsActive = 1 "
         + "LEFT JOIN InActiveScreenOfTemplatesModule MAC ON T.OrganizationTemplateId = MAC.OrganizationTemplateId AND SD.ScreenName = MAC.ScreenName AND MAC.IsActive = 1 "
         + "LEFT JOIN FavoriteScreens f ON f.ScreenName = sd.ScreenName AND f.UserId = :userId "
         + "WHERE sr.RightName = 'View' AND (sd.TargetUrl IS NOT NULL AND sd.TargetUrl <> '') AND sd.IsActive = 1 "
         + "AND EXISTS (SELECT 1 FROM CompanyRights CRT WHERE CRT.ScreenId = sr.ScreenID AND CRT.CompanyId = :companyId AND CRT.IsActive = 1) "
         + "AND (:appModuleId IS NULL OR M.Id = :appModuleId) "
         + "ORDER BY a.SortNo, M.SortNo, sd.SortNo", nativeQuery = true)
    List<UserRightsViewRow> desktopRightsWithCompanyTemplate(@Param("userId") Integer userId, @Param("companyId") Integer companyId,
            @Param("appId") Integer appId, @Param("appModuleId") Integer appModuleId);

    /** USP_GetUserRightsForViewbyUserId, @AppId IN (4,5,6) (customer-portal / booking-office apps): every active screen allocated to the app, Value = 1. */
    @Query(value = "SELECT 0 AS Id, M.Id AS ModuleID, M.ModuleDescription, sd.Id AS ScreenID, sd.ScreenName, ScreenAlias, :userId AS UserId, sr.Id AS RightsID, sr.RightName, "
         + "CAST(1 AS BIT) AS Value, sd.MenuControllName, M.ModelMenuControllName, sd.TargetUrl, M.IconUrl, m.ModuleTypeId, m.AppId, a.App, "
         + "CAST(CASE WHEN ISNULL(f.Id, 0) = 0 THEN 0 ELSE 1 END AS BIT) IsFavorite, '' AS TblUserRightslst, :companyId AS CompanyId "
         + "FROM ScreenDefinition sd "
         + "INNER JOIN AppModules M ON sd.ModuleId = M.Id "
         + "INNER JOIN App a ON m.AppId = a.Id "
         + "INNER JOIN ScreenRights sr ON sd.Id = sr.ScreenID "
         + "INNER JOIN ApplicationsAllocateToScreen APS ON SD.ScreenName = APS.ScreenName AND aps.AppId = :appId "
         + "LEFT JOIN FavoriteScreens f ON f.ScreenName = sd.ScreenName AND f.UserId = :userId "
         + "WHERE sr.RightName = 'View' AND (ISNULL(sd.IsActive, 0) = 1) "
         + "ORDER BY a.SortNo, M.SortNo, sd.SortNo", nativeQuery = true)
    List<UserRightsViewRow> portalRights(@Param("userId") Integer userId, @Param("companyId") Integer companyId, @Param("appId") Integer appId);

    /** USP_GetUserRightsForViewbyUserId, any other @AppId: granted rights of the app joined with ApplicationsAllocateToScreen and CompanyRights, Value = 1. */
    @Query(value = "SELECT 0 AS Id, M.Id AS ModuleID, M.ModuleDescription, sd.Id AS ScreenID, sd.ScreenName, ScreenAlias, UR.UserId, sr.Id AS RightsID, sr.RightName, "
         + "CAST(1 AS BIT) AS Value, sd.MenuControllName, M.ModelMenuControllName, sd.TargetUrl, M.IconUrl, m.ModuleTypeId, m.AppId, a.App, "
         + "CAST(CASE WHEN ISNULL(f.Id, 0) = 0 THEN 0 ELSE 1 END AS BIT) IsFavorite, '' AS TblUserRightslst, :companyId AS CompanyId "
         + "FROM ScreenDefinition sd "
         + "INNER JOIN AppModules M ON sd.ModuleId = M.Id "
         + "INNER JOIN App a ON m.AppId = a.Id "
         + "INNER JOIN ScreenRights sr ON sd.Id = sr.ScreenID "
         + "INNER JOIN (SELECT ScreenId, RightId, Value, UserId FROM tblUserRights WHERE UserId = :userId AND CompanyId = :companyId "
         + "AND (:appId IS NULL OR AppId = :appId)) UR ON UR.ScreenId = sd.Id AND UR.RightId = sr.Id "
         + "INNER JOIN ApplicationsAllocateToScreen APS ON SD.Id = APS.ScreenId AND aps.AppId = :appId "
         + "LEFT JOIN FavoriteScreens f ON f.ScreenName = sd.ScreenName AND f.UserId = :userId "
         + "INNER JOIN CompanyRights CRT ON CRT.ScreenId = sr.ScreenID AND CRT.CompanyId = :companyId "
         + "WHERE sr.RightName = 'View' AND (ISNULL(SD.TargetUrl, '') <> '') AND (ISNULL(sd.IsActive, 0) = 1) AND (ISNULL(CRT.IsActive, 0) = 1) "
         + "ORDER BY a.SortNo, M.SortNo, sd.SortNo", nativeQuery = true)
    List<UserRightsViewRow> otherAppRights(@Param("userId") Integer userId, @Param("companyId") Integer companyId, @Param("appId") Integer appId);
}
