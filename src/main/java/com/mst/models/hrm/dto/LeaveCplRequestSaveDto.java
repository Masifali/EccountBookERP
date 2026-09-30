package com.mst.models.hrm.dto;

import java.util.List;

/**
 * frmEmployeeCPLRequest (462) Save / Update body: RecId, employee, the two date pickers, reason,
 * the leave-date grid (datagrid + Deletelst) and the ticked CPL rows of grd (CPLAttendanceId, Active).
 * CPL quotas are re-read from the database by the service; the page's numbers are not trusted.
 */
public class LeaveCplRequestSaveDto {
    public int id;
    public int employeeId;
    public String fromDate;
    public String toDate;
    public String reason;
    public List<LeaveDateRowDto> details;
    public List<LeaveDateRowDto> deletes;
    public List<Cpl> cpl;

    public static class Cpl {
        public long cplAttendanceId;
        public boolean active;
    }
}
