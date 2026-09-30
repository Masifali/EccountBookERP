package com.mst.models.hrm.dto;

/**
 * The ReportsParameters filter MannualDataSetInDutyRoaster hands genDutyRoaster.GetDutyDatesForDutyRoaster
 * (SP_GetDutyDatesForDutyRoaster): the From / To pickers, the Employee / Section / Category / Location
 * combos, the form's RecId and the department ids string built from cmbDepartment.Text (",3,7").
 */
public class RosterLoadDto {
    public String fromDate;
    public String toDate;
    public int employeeId;
    public int sectionId;
    public int employeeCategoryId;
    public int locationId;
    public long dutyRoasterId;
    public String departmentIds;
}
