package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Leave Approval (668): btnApprove sends the EmployeeLeaveId of every checked row (GetCheckedRows);
 * the popup's Approve / Reject cell sends one detail row.
 */
public class HrmLeaveApprovalDto {
    public List<Integer> employeeLeaveIds;   // btnApprove
    public int employeeId;                   // popup: EmployeeId of the form
    public int leaveTypeProfileId;           // popup: LeaveTypeProfileId of the form
    public int employeeLeaveDetailId;        // popup row
    public String reason;                    // popup row Reason cell
}
