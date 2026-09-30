package com.mst.models.hrm.overtime;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.OverTimeManagement.OverTimeRequest (model 0693) - every non-virtual property,
 * as GenericProvider.SetProc sends it to [hrm].[Sp_OverTimeRequest_Insert] / [hrm].[Sp_OverTimeRequest_Update]
 * (20 parameters each, checked with proc.py: overtimerequestid locationid requestdate overtimedate
 * departmentid sectionid isoffduty reason requestbyid isapproved approvedon approvedbyid createdbyid
 * createdon alteredbyid alteredon actiontypeid userlogid organizationid companyid).
 * The virtual OverTimeRequestDetailList / AttachmentsList are not parameters.
 */
public class OverTimeRequest extends DesktopModel {
    public int ActionTypeId;              // byte
    public boolean IsApproved;
    public boolean IsOffDuty;
    public LocalDateTime AlteredOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime OverTimeDate;
    public LocalDateTime RequestDate;
    public int DepartmentId;
    public int SectionId;
    public long AlteredById;
    public long ApprovedById;
    public long CompanyId;
    public long CreatedById;
    public long LocationId;               // never set by the form -> 0
    public long OrganizationId;
    public long OverTimeRequestId;
    public long RequestById;
    public long UserLogId;                // never set by the form -> 0
    public String Reason;
}
