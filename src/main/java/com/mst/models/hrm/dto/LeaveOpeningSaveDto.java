package com.mst.models.hrm.dto;

import java.math.BigDecimal;
import java.util.List;

/**
 * frmEmployeeLeaveOpening (660) Save / Update body: RecId (always 0 - nothing loads a record), the two
 * combos, and grd.GetRows() - the rows the grid shows (a filtered grid sends only its visible rows, as
 * GridEX.GetRows() does).
 */
public class LeaveOpeningSaveDto {
    public int id;
    public int leaveQuotaId;
    public int leaveTypeProfileId;
    public List<Row> details;

    /** One grid row: EmployeeId, LeaveQuotaDetailId, Assign, Availed. */
    public static class Row {
        public long employeeId;
        public int leaveQuotaDetailId;
        public BigDecimal assign;
        public BigDecimal availed;
    }
}
