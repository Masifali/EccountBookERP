package com.mst.models.hrm.dto;

import java.util.List;

/**
 * frmEmployeeLeaveRequest (661) Save / Update body. details = datagrid rows, deletes = Deletelst
 * (rows with an EmployeeLeaveDetailId removed with the X button). LocationId is not taken from the page:
 * the service reads it from the employee's active history, as GetEmployeeHistory fills cmbLocation.
 */
public class LeaveRequestSaveDto {
    public int id;
    public int employeeId;
    public int leaveTypeProfileId;
    public int leaveQuotaId;
    public String reason;
    public String remarks;
    public List<LeaveDateRowDto> details;
    public List<LeaveDateRowDto> deletes;
}
