package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Request body of OverTimeRequest.btnsave_Click (screen 665): the header controls, the grid rows
 * (grdDetails / dtdetail) and lstRemoveDetailRecord (rows deleted with the grid's X button).
 * Public camelCase fields - Jackson binds the JSON keys to them exactly.
 */
public class OvertimeRequestDto {
    public int id;                 // RecId (0 = new)
    public String requestDate;     // txtRequestDate
    public String overTimeDate;    // txtOverTimeDate
    public int departmentId;       // cmbDepartmnet
    public int sectionId;          // cmbSection
    public boolean isOffDuty;      // IsOffDuty
    public String reason;          // txtReason
    public int requestById;        // cmbRequestedBy
    public List<OvertimeRequestRowDto> details;
    public List<OvertimeRequestRowDto> removed;
}
