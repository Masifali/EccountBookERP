package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.List;

/**
 * Architecture.Model.HRM.PayrollManagement.Payroll (model 0690) - every non-virtual property, as
 * GenericProvider.SetProc sends it to hrm.Sp_Payroll_Insert / hrm.Sp_Payroll_Update (checked against
 * the procedure: all 21 declared; @PostedOn is declared by the procedure but not a model property, so
 * it is not sent, as on the desktop). The two virtual properties are transient.
 */
public class Payroll extends DesktopModel {
    public boolean IsApproved;
    public int ActionTypeId;               // byte
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime PayrollDate;
    public int DocMovementId;
    public int PayrollMonth;
    public int PayrollYear;
    public int DocNo;
    public int FinancialYearId;
    public int BranchesId;
    public int ProjectsId;
    public String Remarks;
    public long AlteredById;
    public long CompanyId;
    public long CreatedById;
    public long LocationId;
    public long OrganizationId;
    public long PayrollId;
    public long PostedById;
    public long UserLogId;

    /** virtual VoucherHead VoucherHeadInvoices - built by BLL Payroll.MakeVoucher. */
    public transient VoucherHead VoucherHeadInvoices;
    /** virtual List&lt;PayrollDetail&gt; PayrollDetailList. */
    public transient List<PayrollDetail> PayrollDetailList;
}
