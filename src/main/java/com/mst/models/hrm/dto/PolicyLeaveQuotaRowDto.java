package com.mst.models.hrm.dto;

/** One leave-grid row: Id (LeaveTypeProfileId) and QuotaValue (double column, null when the cell was cleared). */
public class PolicyLeaveQuotaRowDto {
    public int id;
    public Double quotaValue;
}
