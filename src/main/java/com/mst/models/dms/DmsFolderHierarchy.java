package com.mst.models.dms;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.DMS.FolderHierarchy (model 0321) - every property (none is virtual), in declaration
 * order. GenericProvider.SetProc sends them to DMS.Sp_FolderHierarchy_Insert / _Update (13 parameters each).
 */
public class DmsFolderHierarchy extends DesktopModel {
    public LocalDateTime CreatedDate;
    public LocalDateTime ModifyDate;
    public int CompanyId;
    public int FolderLevel;
    public int OrganizationId;
    public long CreatedById;
    public String DriveName;
    public long FolderHierarchyId;
    public long FolderOwnerId;
    public long ModifyById;
    public long ParentFolderHierarchyId;
    public String FolderName;
    public String FolderPath;
}
