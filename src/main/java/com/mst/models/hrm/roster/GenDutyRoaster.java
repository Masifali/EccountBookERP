package com.mst.models.hrm.roster;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.HRM.AttendanceManagement.genDutyRoaster (model 0729) - every non-virtual property,
 * as GenericProvider.SetProc sends it to Sp_genDutyRoaster_Insert / Sp_genDutyRoaster_Update
 * (18 parameters, all declared by both procedures - checked with proc.py).
 * The two virtual lists are the DAL's child rows and are not parameters.
 */
public class GenDutyRoaster extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDate;
    public LocalDateTime ToDate;
    public int DepartmentId;
    public int OrganizationId;
    public int CompanyId;
    public int EmployeeCategoryId;
    public int SectionId;
    public int ShiftId;
    public long AlteredById;
    public long CreatedById;
    public long DutyRoasterId;
    public long LocationId;
    public long PartyLocationId;
    public long UserLogId;
    public String Description;

    /** virtual List&lt;genDutyRoasterDetail&gt; GenDutyRoasterDetailsList - saved row by row by the DAL. */
    public transient List<GenDutyRoasterDetail> GenDutyRoasterDetailsList = new ArrayList<>();
    /** virtual List&lt;DutyRoasterGroups&gt; DutyRoasterGroupsList - saved row by row by the DAL. */
    public transient List<DutyRoasterGroups> DutyRoasterGroupsList = new ArrayList<>();
}
