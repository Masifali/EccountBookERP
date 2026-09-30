package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.approval.EmployeeLoanInstallment;
import com.mst.models.hrm.approval.VoucherDetail;
import com.mst.models.hrm.approval.VoucherHead;
import com.mst.models.hrm.dto.HrmAdvanceApprovalDto;
import com.mst.models.hrm.dto.HrmLeaveApprovalDto;
import com.mst.models.hrm.dto.HrmLoanApprovalDto;
import com.mst.models.hrm.dto.HrmLoanInstallmentRowDto;
import com.mst.models.hrm.loan.EmployeeLoan;
import com.mst.repositories.hrm.HrmApprovalRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Approval Management" screens (AppModules 2026). Each method names the desktop form method it
 * reproduces; validation wording/order and the messages are the form's. The desktop forms check no Save right
 * (they call no SetRightsValueInRightsObject); the page needs View on its own screen row, as the menu tile does.
 *
 *   667 Loan Approval     ApprovalManagement/LoanApproval.cs
 *   668 Leave Approval    ApprovalManagement/LeaveApproval.cs (+ PendingLeaveDetailForApproval.cs, the "Detail" popup)
 *   669 Advance Approval  ApprovalManagement/AdvanceApproval.cs
 */
@Service
public class HrmApprovalService {

    public static final int SCREEN_LOAN_APPROVAL = 667;
    public static final int SCREEN_LEAVE_APPROVAL = 668;
    public static final int SCREEN_ADVANCE_APPROVAL = 669;
    /** LoanApproval.Insert: obj.DocumentTypeId = 1001. */
    public static final int DOC_TYPE_EMPLOYEE_LOAN = 1001;
    /** What the desktop shows when the combo's selected row has no third cell (SelectedRow.Cells[2]). */
    static final String INDEX_OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index";

    @Autowired private HrmApprovalRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== combos

    /** BindDDLNew(dt, cmb, "EmployeeId", "EmployeeName", "Employee", false). */
    private List<Map<String, Object>> employees(UserAccount u, int departmentId, int designationId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.activeEmployees(u, departmentId, designationId))
            out.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName")));
        return out;
    }

    private List<Map<String, Object>> departments(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.departments(u)) out.add(map("DepartmentId", r.get("DepartmentId"), "DepartmentName", r.get("DepartmentName")));
        return out;
    }

    private List<Map<String, Object>> designations(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.designations(u)) out.add(map("DesignationId", r.get("DesignationId"), "DesignationName", r.get("DesignationName")));
        return out;
    }

    /** cmbDepartment_Leave / cmbDesignation_Leave -> EmployeeFill() with the two filters. */
    public List<Map<String, Object>> filteredEmployees(int screen, int departmentId, int designationId) {
        if (screen != SCREEN_LEAVE_APPROVAL && screen != SCREEN_ADVANCE_APPROVAL) throw invalid("Record not found.");
        return employees(hrm.user(screen), departmentId, designationId);
    }

    // ================================================================== 667 Loan Approval (LoanApproval.cs)

    /** EmployeeFamilyInfo_Load: EmployeeFill(), PendingLoanForApproval(). */
    public Map<String, Object> loanSetup() {
        UserAccount u = hrm.user(SCREEN_LOAN_APPROVAL);
        return map("employees", employees(u, 0, 0), "rows", repo.pendingLoans(u));
    }

    /** PendingLoanForApproval(): the FormHistory rows as the grid binds them (RetrieveStructure). */
    public List<Map<String, Object>> pendingLoans() { return repo.pendingLoans(hrm.user(SCREEN_LOAN_APPROVAL)); }

    /**
     * Insert(): Validation() ("Employee Name Required!", "LoanAmount Field Required!", "No Of Installments Field Required!"),
     * then the model exactly as the form fills it - DocumentTypeId 1001, IsApproved true, EmployeeLoanId = the loaded row's
     * LoanId, LoanAmount = txtLoanAmount (Conversion.ToInt of the row's LoanAmount), ApprovedAmount, AppliedOn = the row's
     * AppliedOn, NoOfInstallment = grd.GetRows().Count(), Reason / Branch / Location / Project NOT set (the update writes
     * NULL / 0 there, as the desktop does) - and the installment rows of the grid. BLL Save then builds the voucher (MakeVoucher).
     */
    public Map<String, Object> saveLoanApproval(HrmLoanApprovalDto b) {
        UserAccount u = hrm.user(SCREEN_LOAN_APPROVAL);
        if (b.employeeId <= 0) throw invalid("Employee Name Required!");
        if (toDouble(b.approvedAmount) == 0.0) throw invalid("LoanAmount Field Required!");
        if (toDouble(b.noOfInstallments) == 0.0) throw invalid("No Of Installments Field Required!");
        if (b.loanId <= 0) throw invalid(INDEX_OUT_OF_RANGE);
        Map<String, Object> row = null;
        for (Map<String, Object> r : repo.pendingLoans(u))
            if (toInt(r.get("EmployeeLoanId")) == b.loanId && toInt(r.get("EmployeeId")) == b.employeeId) { row = r; break; }
        if (row == null) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        EmployeeLoan obj = new EmployeeLoan();
        obj.OrganizationId = u.getOrganizationId();
        obj.CompanyId = u.getCompanyId();
        obj.FinancialYearId = hrm.financialYearId();
        obj.DocumentTypeId = DOC_TYPE_EMPLOYEE_LOAN;
        obj.IsApproved = true;
        obj.EmployeeId = b.employeeId;
        obj.EmployeeLoanId = b.loanId;
        // txtLoanAmount.Text = Conversion.ToInt(item.Cells["LoanAmount"].Value) - Convert.ToInt32(decimal) rounds half to even
        obj.LoanAmount = toDec(row.get("LoanAmount")).setScale(0, RoundingMode.HALF_EVEN);
        obj.ApprovedAmount = toDec(trim(b.approvedAmount));
        LocalDateTime applied = toDate(row.get("AppliedOn"));
        obj.AppliedOn = applied == null ? now : applied;
        obj.AlteredById = u.getId();
        obj.ApprovedById = u.getId();
        obj.AlteredOn = now;
        obj.ApprovedOn = now;
        obj.CreatedOn = now;
        List<HrmLoanInstallmentRowDto> rows = b.details == null ? new ArrayList<>() : b.details;
        obj.NoOfInstallment = rows.size();
        for (HrmLoanInstallmentRowDto r : rows) {
            EmployeeLoanInstallment d = new EmployeeLoanInstallment();
            d.EmployeeLoanInstallmentId = r.employeeLoanInstallmentId;
            d.EmployeeLoanId = b.employeeId;             // the form sets cmbEmployeeName.Value; the DAL overwrites it with the loan id
            d.InstallmentNo = r.installments;
            d.Amount = BigDecimal.valueOf(r.amount);
            d.Month = str(r.month);
            d.Year = r.year;
            d.OrganizationId = u.getOrganizationId();
            d.CompanyId = u.getCompanyId();
            d.CreatedById = u.getId();
            d.CreatedOn = now;
            d.AlteredById = u.getId();
            d.AlteredOn = now;
            d.IsPaid = false;
            d.PayrollId = 0L;
            d.UserLogId = u.getId();
            d.ActionTypeId = 1;
            obj.EmployeeLoanInstallmentslist.add(d);
        }
        obj.VoucherHeadInvoices = makeVoucher(obj);             // BLL Save: IsApproved -> MakeVoucher(obj)
        repo.saveLoanApproval(obj);
        return saved(obj.EmployeeLoanId, "Saved Successfully"); // RecId is always 0 on this form
    }

    /** BLL EmployeeLoan.MakeVoucher(obj) - line by line. */
    VoucherHead makeVoucher(EmployeeLoan obj) {
        int entryUser = (int) obj.ApprovedById;
        int modifyUser = (int) obj.AlteredById;
        VoucherHead vh = new VoucherHead();
        vh.DocumentTypeId = obj.DocumentTypeId;
        vh.DocumentTypeSrNo = obj.EmployeeLoanId;
        vh.RefDocNoId = obj.EmployeeLoanId;
        vh.VoucherCode = obj.EmployeeLoanId;
        vh.VoucherDate = obj.AppliedOn;
        vh.Remarks = str(obj.Reason);
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = obj.BranchId;
        vh.ProjectId = 0;
        vh.ManualBillNo = "";
        vh.DueDate = LocalDateTime.now();
        vh.OrganizationId = obj.OrganizationId;
        vh.CompanyId = obj.CompanyId;
        vh.FinancialYearId = obj.FinancialYearId;
        vh.EntryUser = entryUser;
        vh.EntryDate = obj.CreatedOn;
        vh.ModifyDate = obj.AlteredOn;
        vh.ModifyUser = modifyUser;
        vh.VoucherAmount = obj.ApprovedAmount.doubleValue();
        vh.BillAmount = obj.ApprovedAmount.doubleValue();
        List<Map<String, Object>> list = repo.glAccountsByEmployee(obj.OrganizationId, obj.CompanyId);
        if (list.isEmpty()) throw invalid("Employee Accounts List Not found");
        Map<String, Object> acc = null;
        for (Map<String, Object> r : list) if (toInt(r.get("EmployeeId")) == (int) obj.EmployeeId) { acc = r; break; }
        if (acc == null) throw invalid("Employee Accounts List Not found");
        double value = obj.ApprovedAmount.doubleValue() / (double) obj.NoOfInstallment;
        String text = str(obj.Reason) + " Loan Approved " + obj.ApprovedAmount.toPlainString() + " Noof Installments " + obj.NoOfInstallment
                + " Monthly Installment " + netDouble(roundEven(value, 2));
        VoucherDetail d1 = new VoucherDetail();
        d1.AccountId = toInt(acc.get("LoanAcId"));
        d1.AgainstAccountId = toInt(acc.get("PayableAcId"));
        d1.Comments = text;
        d1.DebitAmount = obj.ApprovedAmount.doubleValue();
        d1.EmployeeId = (int) obj.EmployeeId;
        d1.SubsidiaryTypeId = 2;
        d1.SubsidiaryAccountId = (int) obj.EmployeeId;
        vh.voucherDetailList.add(d1);
        VoucherDetail d2 = new VoucherDetail();
        d2.AccountId = toInt(acc.get("PayableAcId"));
        d2.AgainstAccountId = toInt(acc.get("LoanAcId"));
        d2.Comments = text;
        d2.CreditAmount = obj.ApprovedAmount.doubleValue();
        d2.EmployeeId = (int) obj.EmployeeId;
        d2.SubsidiaryTypeId = 2;
        d2.SubsidiaryAccountId = (int) obj.EmployeeId;
        vh.voucherDetailList.add(d2);
        return vh;
    }

    /** Math.Round(double, 2) - MidpointRounding.ToEven; non-finite values pass through. */
    static double roundEven(double v, int places) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        return BigDecimal.valueOf(v).setScale(places, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** double.ToString() of .NET Framework: shortest round-trip text, no trailing ".0", "Infinity" / "NaN". */
    static String netDouble(double v) {
        if (Double.isNaN(v)) return "NaN";
        if (Double.isInfinite(v)) return v > 0 ? "Infinity" : "-Infinity";
        BigDecimal b = BigDecimal.valueOf(v).stripTrailingZeros();
        return b.scale() < 0 ? b.setScale(0).toPlainString() : b.toPlainString();
    }

    // ================================================================== 668 Leave Approval (LeaveApproval.cs)

    /** EmployeeFamilyInfo_Load: EmployeeFill(), DepartmentFill(), DesignationFill(), HistoryForApproval(). */
    public Map<String, Object> leaveSetup() {
        UserAccount u = hrm.user(SCREEN_LEAVE_APPROVAL);
        return map("employees", employees(u, 0, 0), "departments", departments(u), "designations", designations(u),
                "rows", repo.leavesForApproval(u, 0, 0, 0));
    }

    /** btnSearch_Click / Reset -> HistoryForApproval(): GetDataForLeaveApproval with the three combos. */
    public List<Map<String, Object>> leaves(int employeeId, int departmentId, int designationId) {
        return repo.leavesForApproval(hrm.user(SCREEN_LEAVE_APPROVAL), employeeId, departmentId, designationId);
    }

    /**
     * btnApprove_Click (after "Are you sure to Approve?"): for every checked row, GetEmployeeLeaveDetailIdByHeaderId and a
     * LeaveApproval item per detail id (LeaveQuotaId, EmployeeId, LeaveTypeProfileId of the row, EntryUser = user, ActionId
     * left 0), then LeaveApprovedlist - one call per item, no transaction. The row values come from this company's pending
     * list, not from the page. No checked row -> nothing is sent and the form still says "Approve Successfully".
     */
    public Map<String, Object> approveLeaves(HrmLeaveApprovalDto b) {
        UserAccount u = hrm.user(SCREEN_LEAVE_APPROVAL);
        List<Map<String, Object>> pending = repo.leavesForApproval(u, 0, 0, 0);
        List<int[]> items = new ArrayList<>();
        for (Integer id : b.employeeLeaveIds == null ? new ArrayList<Integer>() : b.employeeLeaveIds) {
            Map<String, Object> row = null;
            for (Map<String, Object> r : pending) if (toInt(r.get("EmployeeLeaveId")) == toInt(id)) { row = r; break; }
            if (row == null) throw invalid("Record not found.");
            for (Map<String, Object> d : repo.leaveDetailIds(toInt(id))) {
                items.add(new int[] { toInt(row.get("EmployeeLeaveQuotaId")), toInt(row.get("EmployeeId")), toInt(row.get("LeaveTypeProfileId")),
                        toInt(d.get("EmployeeLeaveDetailId")) });
            }
        }
        for (int[] it : items)
            repo.leaveApproved(u.getOrganizationId(), u.getCompanyId(), it[0], it[2], it[1], u.getId(), it[3], 0);
        return map("success", true, "message", "Approve Successfully", "count", items.size());
    }

    /** PendingLeaveDetailForApproval.HistoryForApproval(EmployeeId): GetLeaveDetailByEmployeeLeaveId. */
    public List<Map<String, Object>> leaveDetails(int employeeId, int leaveTypeProfileId) {
        return repo.leaveDetails(hrm.user(SCREEN_LEAVE_APPROVAL), employeeId, leaveTypeProfileId);
    }

    /**
     * PendingLeaveDetailForApproval.grdHistory_ColumnButtonClick: "Reason Field Required" when the row's Reason is empty;
     * Reject -> LeaveApproved(ActionId 1), Approve -> LeaveApproved(ActionId 2), with the row's EmployeeLeaveQuotaId /
     * EmployeeId / LeaveTypeProfileId / EmployeeLeaveDetailId (read again from this company's pending detail list).
     */
    public Map<String, Object> leaveDetailAction(HrmLeaveApprovalDto b, boolean approve) {
        UserAccount u = hrm.user(SCREEN_LEAVE_APPROVAL);
        if (str(b.reason).isEmpty()) throw invalid("Reason Field Required");
        Map<String, Object> row = null;
        for (Map<String, Object> r : repo.leaveDetails(u, b.employeeId, b.leaveTypeProfileId))
            if (toInt(r.get("EmployeeLeaveDetailId")) == b.employeeLeaveDetailId) { row = r; break; }
        if (row == null) throw invalid("Record not found.");
        repo.leaveApproved(u.getOrganizationId(), u.getCompanyId(), toInt(row.get("EmployeeLeaveQuotaId")), toInt(row.get("LeaveTypeProfileId")),
                toInt(row.get("EmployeeId")), u.getId(), b.employeeLeaveDetailId, approve ? 2 : 1);
        return map("success", true);
    }

    // ================================================================== 669 Advance Approval (AdvanceApproval.cs)

    /** EmployeeFamilyInfo_Load: EmployeeFill(), DepartmentFill(), DesignationFill(), HistoryForApproval(). */
    public Map<String, Object> advanceSetup() {
        UserAccount u = hrm.user(SCREEN_ADVANCE_APPROVAL);
        return map("employees", employees(u, 0, 0), "departments", departments(u), "designations", designations(u),
                "rows", repo.advancesForApproval(u, 0, 0, 0));
    }

    /** btnSearch_Click / Reset -> HistoryForApproval(). */
    public List<Map<String, Object>> advances(int employeeId, int departmentId, int designationId) {
        return repo.advancesForApproval(hrm.user(SCREEN_ADVANCE_APPROVAL), employeeId, departmentId, designationId);
    }

    /**
     * grdPendingForLoan_ColumnButtonClick (Approve cell, after "Are you sure to Approve?"): "Approved Amount Field Required",
     * "Approved Amount Can not Greater than Advance Amount", then "Approve Amount Required" when Conversion.ToInt(ApprovedAmount)
     * is 0; UpdateEmployeeAdvanceForApproval; "Approve Successfully". AdvanceAmount is the row's value from this company's list.
     */
    public Map<String, Object> approveAdvance(HrmAdvanceApprovalDto b) {
        UserAccount u = hrm.user(SCREEN_ADVANCE_APPROVAL);
        Map<String, Object> row = null;
        for (Map<String, Object> r : repo.advancesForApproval(u, 0, 0, 0))
            if (toInt(r.get("EmployeeAdvanceId")) == b.employeeAdvanceId) { row = r; break; }
        if (row == null) throw invalid("Record not found.");
        BigDecimal approved = toDec(trim(b.approvedAmount));
        if (approved.compareTo(BigDecimal.ZERO) == 0) throw invalid("Approved Amount Field Required");
        if (approved.compareTo(toDec(row.get("AdvanceAmount"))) > 0) throw invalid("Approved Amount Can not Greater than Advance Amount");
        if (approved.setScale(0, RoundingMode.HALF_EVEN).intValue() == 0) throw invalid("Approve Amount Required");
        repo.approveAdvance(b.employeeAdvanceId, u.getId(), approved);
        return map("success", true, "message", "Approve Successfully");
    }
}
