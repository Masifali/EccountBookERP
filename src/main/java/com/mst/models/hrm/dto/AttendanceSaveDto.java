package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Save (658 btnsave_Click) / Update (659 btnupdate_Click) request: the grid rows the form walks
 * (grd.GetRows() - the rows in the current view) and the filter the grid was loaded with, which the
 * service uses to re-read the company's own attendance rows as the tenancy guard.
 */
public class AttendanceSaveDto {
    public AttendanceFilterDto loaded;
    public List<AttendanceRowDto> details;
}
