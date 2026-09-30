package com.mst.repositories.dms;

import com.mst.models.UserAccount;
import com.mst.models.dms.DmsFolderHierarchy;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * Procedure calls of Architecture.WinApp.DMS_FolderHierarchy.CreateFolderStructure (ScreenDefinition 473),
 * traced through BLL 0258 DMS.FolderHierarchy / DAL 0226, BLL 0257 DMS.DocFilePath, BLL UserAccount.
 * Every procedure and parameter was checked in procdure.utf8.sql:
 *
 *   DMS.Sp_FolderHierarchy_GetAllMethod  @OrganizationId @CompanyId @FolderHierarchyId @UserId @DocFilePathId @Activity
 *   DMS.Sp_FolderHierarchy_Insert        13 params = model 0321 (SetProc)
 *   [DMS].[Sp_DocFilePath_GetAllMethod]  @OrganizationId @CompanyId @DocFilePathId @Activity
 */
@Repository
public class DmsFolderRepository {

    private static final String FH = "DMS.Sp_FolderHierarchy_GetAllMethod";

    private final HrmProcRepository db;

    public DmsFolderRepository(HrmProcRepository db) { this.db = db; }

    /** FolderHierarchy.GetAll(org, company): @OrganizationId @CompanyId @Activity='ReadAll'. */
    public List<Map<String, Object>> readAll(UserAccount u) {
        return db.rows(FH, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** FolderHierarchy.ReadFoldersBySubUserId(org, company, userId): @UserId, @Activity='ReadFoldersBySubUserId'. */
    public List<Map<String, Object>> readFoldersBySubUserId(UserAccount u) {
        return db.rows(FH, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId(),
                "Activity", "ReadFoldersBySubUserId");
    }

    /** FolderHierarchy.GetChildDataByParentId(id): @FolderHierarchyId, @Activity='GetChildDataByParentId'. */
    public List<Map<String, Object>> children(long parentId) {
        return db.rows(FH, "FolderHierarchyId", parentId, "Activity", "GetChildDataByParentId");
    }

    /** FolderHierarchy.GetDocFilePathbyFolderId(org, company, folderId, userId): Activity 'GetDocFilePathbyFolderId'. */
    public List<Map<String, Object>> documents(UserAccount u, long folderId, int userId) {
        return db.rows(FH, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FolderHierarchyId", folderId, "UserId", userId, "Activity", "GetDocFilePathbyFolderId");
    }

    /** FolderHierarchy.Save: FolderHierarchyId == 0 -> DMS.Sp_FolderHierarchy_Insert (SetProc of the whole model). */
    public int insert(DmsFolderHierarchy m) {
        return db.set("DMS.Sp_FolderHierarchy_Insert", m);
    }

    /**
     * FolderHierarchy.DeleteFolderById(id): @FolderHierarchyId, @Activity='DeleteFolderById' (GetDataTableProc).
     * The procedure RAISERRORs "You cannot delete this folder because this folder has subfolders." /
     * "... has files." and otherwise deletes the row.
     */
    public void deleteFolder(long folderId) {
        db.rows(FH, "FolderHierarchyId", folderId, "Activity", "DeleteFolderById");
    }

    /** DocFilePath.DeleteFile(id): [DMS].[Sp_DocFilePath_GetAllMethod] @DocFilePathId, @Activity='DeleteById'. */
    public void deleteFile(int docFilePathId) {
        db.rows("[DMS].[Sp_DocFilePath_GetAllMethod]", "DocFilePathId", docFilePathId, "Activity", "DeleteById");
    }
}
