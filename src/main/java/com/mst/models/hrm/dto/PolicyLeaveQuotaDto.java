package com.mst.models.hrm.dto;

import java.util.List;

/** Request body of Leave Quota Policy (646, LeaveQuotaPolicy.cs Insert()): RecId, dates, description and both grids. */
public class PolicyLeaveQuotaDto {
    public int id;
    public String fromDate;
    public String toDate;
    public String description;
    /** grdcasualleave rows (Offical Leave). */
    public List<PolicyLeaveQuotaRowDto> casual;
    /** grdspecialleave rows (Special Leave). */
    public List<PolicyLeaveQuotaRowDto> special;
}
