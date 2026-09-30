package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Body of Insert() on frmDutyRoasterNew (656) and frmEmployeeRoaster (657): the form's RecId, the
 * header controls, the filter the grids were last loaded with (to verify the rows), the on-duty
 * detail rows and every grdGroup row.
 */
public class RosterSaveDto {
    public long recId;
    public String fromDate;
    public String toDate;
    public String description;
    public int employeeCategoryId;
    public int sectionId;
    public int locationId;
    public RosterLoadDto load;
    public List<RosterDetailRowDto> details;
    public List<RosterGroupRowDto> groups;
}
