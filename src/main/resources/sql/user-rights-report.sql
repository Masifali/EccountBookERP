-- Same aggregate and joins as usp_getScreenRightsByModuleAndUserId.
-- Push final user/application filters before aggregation and bound query memory; no database objects changed.
SELECT
	R.ModuleId,
    R.ModuleDescription,
    R.ScreenAlias,
    R.ScreenId,
    UR.UserId,
	UR.AppId,
    U.FirstName + ' ' + U.LastName AS UserName,
    CAST(MAX(CASE WHEN SR.RightName = 'View' THEN UR.RightId ELSE 0 END) AS Int) AS [ViewId],
    CAST(MAX(CASE WHEN SR.RightName = 'View' THEN UR.Value ELSE 0 END) AS BIT) AS [View],
    CAST(MAX(CASE WHEN SR.RightName = 'Save' THEN UR.RightId ELSE 0 END) AS Int) AS [SaveId],
    CAST(MAX(CASE WHEN SR.RightName = 'Save' THEN UR.Value ELSE 0 END) AS BIT) AS [Save],
    CAST(MAX(CASE WHEN SR.RightName = 'Update' THEN UR.RightId ELSE 0 END) AS Int) AS [UpdateId],
    CAST(MAX(CASE WHEN SR.RightName = 'Update' THEN UR.Value ELSE 0 END) AS BIT) AS [Update],
    CAST(MAX(CASE WHEN SR.RightName = 'Delete' THEN UR.RightId ELSE 0 END) AS Int) AS [DeleteId],
    CAST(MAX(CASE WHEN SR.RightName = 'Delete' THEN UR.Value ELSE 0 END) AS BIT) AS [Delete],
    CAST(MAX(CASE WHEN SR.RightName = 'Print' THEN UR.RightId ELSE 0 END) AS Int) AS [PrintId],
    CAST(MAX(CASE WHEN SR.RightName = 'Print' THEN UR.Value ELSE 0 END) AS BIT) AS [Print],
    CAST(MAX(CASE WHEN SR.RightName = 'CanView AllRecord' THEN UR.RightId ELSE 0 END) AS Int) AS [CanViewAllRecordId],
    CAST(MAX(CASE WHEN SR.RightName = 'CanView AllRecord' THEN UR.Value ELSE 0 END) AS BIT) AS [CanViewAllRecord],
    CAST(MAX(CASE WHEN SR.RightName = 'Field Chooser' THEN UR.RightId ELSE 0 END) AS Int) AS [FieldChooserId],
    CAST(MAX(CASE WHEN SR.RightName = 'Field Chooser' THEN UR.Value ELSE 0 END) AS BIT) AS [FieldChooser],
    CAST(MAX(CASE WHEN SR.RightName = 'Save Layout' THEN UR.RightId ELSE 0 END) AS Int) AS [SaveLayoutId],
    CAST(MAX(CASE WHEN SR.RightName = 'Save Layout' THEN UR.Value ELSE 0 END) AS BIT) AS [SaveLayout],
    CAST(MAX(CASE WHEN SR.RightName = 'Grid Print' THEN UR.RightId ELSE 0 END) AS Int) AS [GridPrintId],
    CAST(MAX(CASE WHEN SR.RightName = 'Grid Print' THEN UR.Value ELSE 0 END) AS BIT) AS [GridPrint],
    CAST(MAX(CASE WHEN SR.RightName = 'Grid Export' THEN UR.RightId ELSE 0 END) AS Int) AS [GridExportId],
    CAST(MAX(CASE WHEN SR.RightName = 'Grid Export' THEN UR.Value ELSE 0 END) AS BIT) AS [GridExport],
    CAST(MAX(CASE WHEN SR.RightName = 'Group Collapse' THEN UR.RightId ELSE 0 END) AS Int) AS [GroupCollapseId],
    CAST(MAX(CASE WHEN SR.RightName = 'Group Collapse' THEN UR.Value ELSE 0 END) AS BIT) AS [GroupCollapse],
    CAST(MAX(CASE WHEN SR.RightName = 'Group Expand' THEN UR.RightId ELSE 0 END) AS Int) AS [GroupExpandId],
    CAST(MAX(CASE WHEN SR.RightName = 'Group Expand' THEN UR.Value ELSE 0 END) AS BIT) AS [GroupExpand]

FROM (SELECT S.ModuleId,A.ModuleDescription,S.Id AS ScreenId,S.ScreenAlias
FROM AppModules A JOIN ScreenDefinition S ON A.Id=S.ModuleId AND S.IsActive=1
JOIN CompanyRights CR ON CR.ScreenId=S.Id AND CR.CompanyId=:company AND CR.IsActive=1
WHERE (:module IS NULL OR S.ModuleId=:module) AND (:screen IS NULL OR S.Id=:screen)) R
		INNER JOIN tblUserRights UR ON R.ScreenId = UR.ScreenId 
		INNER JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID
		INNER JOIN UserAccount U ON UR.UserId = U.ID
WHERE UR.CompanyId = :company AND (:user IS NULL OR UR.UserId=:user) AND (:app IS NULL OR UR.AppId=:app)
GROUP BY
    R.ModuleId,
	R.ModuleDescription,
    R.ScreenAlias,
    R.ScreenId,
    UR.UserId,
	UR.AppId,
    U.FirstName,
	U.LastName
ORDER BY UR.UserId,R.ModuleId
OPTION (MAXDOP 1, MAX_GRANT_PERCENT = 1, RECOMPILE);