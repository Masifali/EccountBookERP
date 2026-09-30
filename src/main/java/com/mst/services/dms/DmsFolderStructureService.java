package com.mst.services.dms;

import com.mst.models.UserAccount;
import com.mst.models.dms.DmsFolderHierarchy;
import com.mst.repositories.dms.DmsFolderRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Upload documents - 473 "Create Folder Structure", Architecture.WinApp.DMS_FolderHierarchy.CreateFolderStructure.
 *
 * WHAT THE DESKTOP FORM ACTUALLY SHOWS (designer + CreateFolderStructure_Load:245)
 *   - the "Folders" tree (treeView) with a right-click menu: "Add Child New Folder", "Upload File",
 *     "Delete Folder";
 *   - on a node click (treeView_AfterSelect:466) the "Allocatd Documents" panel with a Card View and a
 *     Grid View of the folder's files (only when the folder has files), each with Delete and open.
 *   Hidden and unreachable on the desktop, therefore not ported: the root-folder box (label1, FolderName,
 *   btnCreateFolder - all Visible=false), the "User Name" combo (CmbUserName Visible=false, so its value is
 *   always 0 and the allocated / un-allocated branch of AfterSelect never runs), the Customer Allocation panel
 *   (AllocationPanel.Hide() at Load), the Un-Allocated Documents panel and the allocate / de-allocate buttons
 *   (they need CmbUserName > 0), and the "Folders" tab (removed at Load).
 *
 * FILE-SYSTEM ACTIONS (the desktop works on the Windows share under configuration
 * "RootPathForShareFolderStructure"; the web server cannot reach the client's share):
 *   - Add Child New Folder: SubFolderPopUp checks Directory.Exists(parent path + "\" + name) ("Folder Name X
 *     Already Exist") and Directory.CreateDirectory()s it before the row is inserted. The web inserts the
 *     DMS.FolderHierarchy row only; the page says the directory is NOT created on the share.
 *   - Delete Folder: the row is deleted by the procedure (which refuses a folder with subfolders or files);
 *     Directory.Delete(path) is NOT done by the web.
 *   - Delete (file, card or grid): the desktop deletes the file with File.Delete and then the DocFilePath row;
 *     it does nothing (card) or says "File Not Exist" (grid) when the file is missing. The web cannot see the
 *     file: it deletes the DocFilePath row (and its allocations, inside the procedure) only, after a
 *     confirmation that says the file stays on the share.
 *   - Open a file (card click / FileName link: Process.Start(path)), the card thumbnails, the file size and
 *     creation date (FileInfo), and "Upload File" (DMSAttachmentsUpload copies local files to the share):
 *     not available in the browser - the page shows the stored path instead.
 *
 * DESKTOP DEFECT FIXED: after "Delete Folder" the desktop does not rebuild the tree, so the deleted node
 * stays clickable and "Add Child New Folder" on it would insert a child of a folder that no longer exists
 * (an orphan row). The web reloads the tree after a delete.
 *
 * Rights: the form reads none; every call needs View on ScreenDefinition 473 (the menu tile).
 */
@Service
public class DmsFolderStructureService {

    public static final int SCREEN_ID = 473;

    @Autowired private DmsFolderRepository repo;
    @Autowired private HrmSupport hrm;

    // ============================================================================== tree

    /** TreeViewFill():267 + PopulateTreeViewFromDb():287. */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN_ID);
        return map("tree", tree(u));
    }

    public List<Map<String, Object>> tree() { return tree(hrm.user(SCREEN_ID)); }

    /**
     * SupplierCustomerId > 0 (a customer-portal user) -> ReadFoldersBySubUserId, else GetAll. Roots are the rows
     * with ParentFolderHierarchyId 0, in the procedure's order; children of every node come from
     * GetChildDataByParentId, recursively, as PopulateTreeViewFromDb reads them. (The desktop also calls
     * GetChildDataByParentId for the non-root rows of the first list, whose nodes it never attaches; those
     * calls change nothing on screen and are not repeated.)
     */
    private List<Map<String, Object>> tree(UserAccount u) {
        int sc = u.getSupplierCustomerId() == null ? 0 : u.getSupplierCustomerId();
        List<Map<String, Object>> list = sc <= 0 ? repo.readAll(u) : repo.readFoldersBySubUserId(u);
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Long> seen = new HashSet<>();
        for (Map<String, Object> r : list) {
            if (toLong(r.get("ParentFolderHierarchyId")) != 0L) continue;
            Map<String, Object> n = node(r);
            if (!seen.add(toLong(n.get("FolderHierarchyId")))) continue;
            n.put("children", children(toLong(n.get("FolderHierarchyId")), seen, 1));
            out.add(n);
        }
        return out;
    }

    private List<Map<String, Object>> children(long parentId, Set<Long> seen, int depth) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (depth > 64) return out;                                              // a cycle in the data cannot hang the page
        for (Map<String, Object> r : repo.children(parentId)) {
            Map<String, Object> n = node(r);
            long id = toLong(n.get("FolderHierarchyId"));
            if (!seen.add(id)) continue;
            n.put("children", children(id, seen, depth + 1));
            out.add(n);
        }
        return out;
    }

    private static Map<String, Object> node(Map<String, Object> r) {
        Map<String, Object> n = new LinkedHashMap<>();
        n.put("FolderHierarchyId", toLong(r.get("FolderHierarchyId")));
        n.put("ParentFolderHierarchyId", toLong(r.get("ParentFolderHierarchyId")));
        n.put("FolderLevel", toInt(r.get("FolderLevel")));
        n.put("FolderName", str(r.get("FolderName")));
        n.put("FolderPath", str(r.get("FolderPath")));
        n.put("DriveName", r.get("DriveName") == null ? null : str(r.get("DriveName")));
        n.put("OrganizationId", toInt(r.get("OrganizationId")));
        n.put("CompanyId", toInt(r.get("CompanyId")));
        return n;
    }

    /** A folder the signed-in user's tree shows (tenancy for every id the page sends). */
    private Map<String, Object> ownedFolder(UserAccount u, long id) {
        Map<String, Object> hit = find(tree(u), id);
        if (hit == null) throw invalid("No tree node selected. Please Select!");
        return hit;
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> find(List<Map<String, Object>> nodes, long id) {
        for (Map<String, Object> n : nodes) {
            if (toLong(n.get("FolderHierarchyId")) == id) return n;
            Map<String, Object> c = find((List<Map<String, Object>>) n.get("children"), id);
            if (c != null) return c;
        }
        return null;
    }

    // ========================================================================= documents

    /**
     * treeView_AfterSelect():466 with CmbUserName = 0 (the combo is hidden): GetDocFilePathbyFolderId(org,
     * company, folderId, UserAccount.ID). Grid columns as GridFillForAllocatedAndUnAllocatedDocuments:619
     * (DocFilePathId / FilePath hidden, CreatedDate "Upload Date", UserName "Upload User").
     */
    public Map<String, Object> documents(long folderId) {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> f = ownedFolder(u, folderId);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.documents(u, folderId, u.getId())) {
            rows.add(map("FolderName", str(r.get("FolderName")), "DocFilePathId", toInt(r.get("DocFilePathId")),
                    "FilePath", str(r.get("FilePath")), "FileName", str(r.get("FileName")),
                    "FileExtension", str(r.get("FileExtension")), "CreatedDate", r.get("CreatedDate"),
                    "UserName", str(r.get("UserName"))));
        }
        return map("folderPath", f.get("FolderPath"), "rows", rows);
    }

    // ========================================================================= folders

    /**
     * AddNewFolder_Click:417 -> SubFolderPopUp.btnOk_Click ("Folder Name Required!" when the text is empty,
     * untrimmed) -> InsertSubFolder(parent, name):349. RecId is never set on this form, so it is always an
     * insert: DriveName = parent's, Parent = parent id, FolderLevel = parent level + 1, FolderName = the text
     * as typed, FolderPath = parent path + "\" + name, owner / created / modified by = user, dates = now.
     * (Sp_FolderHierarchy_Insert itself takes FolderOwnerId from the parent row when there is a parent.)
     */
    public Map<String, Object> addSubFolder(long parentId, String name) {
        UserAccount u = hrm.user(SCREEN_ID);
        String sub = name == null ? "" : name;
        if (sub.isEmpty()) throw invalid("Folder Name Required!");
        Map<String, Object> p = ownedFolder(u, parentId);
        LocalDateTime now = LocalDateTime.now();
        DmsFolderHierarchy m = new DmsFolderHierarchy();
        m.FolderHierarchyId = 0L;
        m.DriveName = (String) p.get("DriveName");
        m.ParentFolderHierarchyId = toLong(p.get("FolderHierarchyId"));
        m.FolderLevel = toInt(p.get("FolderLevel")) + 1;
        m.FolderName = sub;
        m.FolderPath = str(p.get("FolderPath")) + "\\" + sub;
        m.FolderOwnerId = u.getId();
        m.CreatedById = u.getId();
        m.CreatedDate = now;
        m.ModifyDate = now;
        m.ModifyById = u.getId();
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        int id = repo.insert(m);
        Map<String, Object> r = saved(id, "Folder Created Successfully...");
        r.put("tree", tree(u));
        return r;
    }

    /** BtnDeleteFolder_Click:1045 -> FolderHierarchy.DeleteFolderById; "Fodler Deleted Successfully" (sic). */
    public Map<String, Object> deleteFolder(long folderId) {
        UserAccount u = hrm.user(SCREEN_ID);
        ownedFolder(u, folderId);
        repo.deleteFolder(folderId);
        Map<String, Object> r = saved((int) folderId, "Fodler Deleted Successfully");
        r.put("tree", tree(u));
        return r;
    }

    /**
     * UserControl_Click (card "Delete") / GrdDocuments_ColumnButtonClick ("Delete") -> DocFilePath.DeleteFile(id);
     * "File Deleted Successfully". Only a file listed in that folder for this user is accepted.
     */
    public Map<String, Object> deleteFile(long folderId, int docFilePathId) {
        UserAccount u = hrm.user(SCREEN_ID);
        ownedFolder(u, folderId);
        boolean listed = false;
        for (Map<String, Object> r : repo.documents(u, folderId, u.getId())) {
            if (toInt(r.get("DocFilePathId")) == docFilePathId) { listed = true; break; }
        }
        if (!listed) throw invalid("File Not Exist");
        repo.deleteFile(docFilePathId);
        return saved(docFilePathId, "File Deleted Successfully");
    }

    static long toLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return Long.parseLong(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0L; }
    }
}
