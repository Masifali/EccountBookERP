package com.mst.models.hrm.overtime;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.OverTimeManagement.OverTimeRequestDetail (model 0694) - the 16 non-virtual
 * properties SetProc sends to [hrm].[Sp_OverTimeRequestDetail_Insert] (16 parameters, checked:
 * overtimerequestdetailid overtimerequestid employeeid fromdatetime todatetime othours otrate
 * isallowmeal isapproved createdbyid createdon alteredbyid alteredon actiontypeid userlogid requeststatus).
 * The virtual EmployeeName is not a parameter (transient).
 *
 * The desktop leaves CreatedOn / AlteredOn at DateTime.MinValue on the rows it marks for delete
 * (ActionTypeId 3), which ADO.NET rejects with "SqlDateTime overflow"; here they stay null and are
 * omitted, so the proc's delete branch actually runs (see the report).
 */
public class OverTimeRequestDetail extends DesktopModel {
    public int ActionTypeId;
    public boolean IsAllowMeal;
    public boolean IsApproved;
    public long OTHours;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDateTime;
    public LocalDateTime ToDateTime;
    public BigDecimal OTRate = BigDecimal.ZERO;
    public long AlteredById;
    public long CreatedById;
    public long EmployeeId;
    public long OverTimeRequestDetailId;
    public long OverTimeRequestId;
    public long UserLogId;
    public boolean RequestStatus;
    public transient String EmployeeName;
}
