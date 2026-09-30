package com.mst.models.hrm.dto;

/** 653 frmShiftLocation btnsave / btnupdate: RecId (0 = new), Shift, Location, the hidden Branch / Project, Is Active. */
public class ShiftLocationSaveDto {
    public int id;
    public int shiftId;
    public int locationId;
    public int branchId;
    public int projectId;
    public boolean isActive;
}
