package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.HrmAdvanceDto;
import com.mst.models.hrm.dto.HrmLoanDto;
import com.mst.models.hrm.loan.EmployeeAdvance;
import com.mst.models.hrm.loan.EmployeeLoan;
import com.mst.repositories.hrm.HrmLoanRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Loan Management" screens (AppModules 2024). Each method names the desktop form method
 * it reproduces; validation wording/order and the messages are the form's.
 *
 *   663 Employee Loan      LoanManagement/frmEmployeeLoan.cs
 *   664 Employee Advance   LoanManagement/frmEmployeeAdvance.cs
 */
@Service
public class HrmLoanService {

    public static final int SCREEN_EMPLOYEE_LOAN = 663;
    public static final int SCREEN_EMPLOYEE_ADVANCE = 664;
    /** frmEmployeeRegistration (btnEmployeeRegister opens it after its own View right). */
    public static final int SCREEN_EMPLOYEE_REGISTRATION = 648;
    /** The loan's DocumentTypeId used by the 102 print (CommonServices.VoucherHeadIdGet(RecId, 1001)). */
    public static final int DOC_TYPE_EMPLOYEE_LOAN = 1001;

    @Autowired private HrmLoanRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== shared

    /**
     * EmployeeName(): GetAllActiveEmployee -> BindDDLNew(dt, cmbEmployeeName, "EmployeeId", "EmployeeName", ..., false).
     * The whole DataTable (dtemployee) is kept by the form for cmbEmployeeName_Leave, so the page gets the
     * columns that handler reads (EmployeeNo, DesignationName, DepartmentName, TotalSalary, SectionName);
     * TotalSalary as Conversion.ToString of the decimal (its text, scale kept). DISTINCT rows in the proc can
     * still repeat an employee; the combo shows each row as the UltraCombo does.
     */
    private List<Map<String, Object>> employees(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.activeEmployees(u)) {
            out.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"), "EmployeeNo", str(r.get("EmployeeNo")),
                    "DesignationName", str(r.get("DesignationName")), "DepartmentName", str(r.get("DepartmentName")),
                    "TotalSalary", text(r.get("TotalSalary")), "SectionName", str(r.get("SectionName"))));
        }
        return out;
    }

    /** Conversion.ToString of a decimal cell: its plain text with the scale SQL returned ("25000.00"). */
    static String text(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        return String.valueOf(v);
    }

    private boolean ownsEmployee(UserAccount u, int employeeId) {
        return owns(repo.activeEmployees(u), "EmployeeId", employeeId);
    }

    // ================================================================== 663 Employee Loan (frmEmployeeLoan.cs)

    /**
     * EmployeeFamilyInfo_Load: SetRightsValueInRightsObject("EmployeeLoan") (btnsave = Save, btnupdate = Update),
     * EmployeeName(), GridBind(). canRegister = the View right frmEmployeeRegistration checks in btnEmployeeRegister_Click.
     */
    public Map<String, Object> loanSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LOAN);
        return map("rights", hrm.rights(u, SCREEN_EMPLOYEE_LOAN), "employees", employees(u), "rows", loanGrid(u),
                "canRegister", hrm.can(u, SCREEN_EMPLOYEE_REGISTRATION, "View"));
    }

    /** btnRefresh_Click -> EmployeeName(). */
    public List<Map<String, Object>> loanEmployees() { return employees(hrm.user(SCREEN_EMPLOYEE_LOAN)); }

    /** GridBind(): FormHistory(CanViewAllRecord true, ApprovedFilter "All"). */
    public List<Map<String, Object>> loanHistory() { return loanGrid(hrm.user(SCREEN_EMPLOYEE_LOAN)); }

    /**
     * GridBind(): dtEmp built row by row from FormHistory - LoanId, DocumentTypeId, DocNo, EmployeeId, EmployeeName,
     * Department, Designation, AppliedDate, LoanAmount, ApprovedAmount, NoOfInstallments, Reason, EntryUser, EntryDate,
     * ModifyUser, ApprovedUser, Approve_Status, ApprovedDate (the dates as .ToShortDateString() - the page shows the date part).
     */
    private List<Map<String, Object>> loanGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.loanHistory(u, null)) {
            out.add(map("LoanId", r.get("EmployeeLoanId"), "DocumentTypeId", r.get("DocumentTypeId"), "DocNo", r.get("DocNo"),
                    "EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"), "Department", r.get("DepartmentName"),
                    "Designation", r.get("DesignationName"), "AppliedDate", r.get("AppliedOn"), "LoanAmount", r.get("LoanAmount"),
                    "ApprovedAmount", r.get("ApprovedAmount"), "NoOfInstallments", r.get("NoOfInstallment"), "Reason", r.get("Reason"),
                    "EntryUser", r.get("CreatedBy"), "EntryDate", r.get("CreatedOn"), "ModifyUser", r.get("AlteredBy"),
                    "ApprovedUser", r.get("ApprovedBy"), "Approve_Status", str(r.get("IsApproved")), "ApprovedDate", r.get("ApprovedOn")));
        }
        return out;
    }

    /** ReadById(Id): EmployeeLoan.GetByID (only an id of this company's history). */
    public Map<String, Object> loan(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LOAN);
        if (!owns(repo.loanHistory(u, null), "EmployeeLoanId", id)) throw invalid("Record Not Found");
        Map<String, Object> r = one(repo.loan(id));
        return map("EmployeeLoanId", r.get("EmployeeLoanId"), "AppliedOn", r.get("AppliedOn"), "EmployeeId", r.get("EmployeeId"),
                "Reason", str(r.get("Reason")), "LoanAmount", text(r.get("LoanAmount")), "NoOfInstallment", str(r.get("NoOfInstallment")));
    }

    /**
     * Insert(): Validation() in the form's order and wording, then the model exactly as the form fills it
     * (FinancialYearId = active year, Branch from the session, Created/Altered/Approved by = user, *On = now,
     * IsApproved false, DocumentTypeId 0, empty installment list) and EmployeeLoan.Save.
     */
    public Map<String, Object> saveLoan(HrmLoanDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LOAN);
        int id = Math.max(0, b.id);
        hrm.require(u, SCREEN_EMPLOYEE_LOAN, id > 0 ? "Update" : "Save");
        if (b.employeeId <= 0 || !ownsEmployee(u, b.employeeId)) throw invalid("Employee Name Required!");
        if (toDouble(trim(b.loanAmount)) == 0.0) throw invalid("LoanAmount Field Required!");
        if (toDouble(b.noOfInstallments) == 0.0) throw invalid("No Of Installments Field Required!");
        if (toDouble(b.noOfInstallments) > 60.0) throw invalid("No Of Installments Can not greater than 60...!");
        if (id > 0 && !owns(repo.loanHistory(u, null), "EmployeeLoanId", id)) throw invalid("Record Not Found");
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime applied = toDate(b.appliedOn);
        EmployeeLoan m = new EmployeeLoan();
        m.EmployeeLoanId = id;
        m.EmployeeId = b.employeeId;
        m.LoanAmount = toDec(trim(b.loanAmount));
        m.AppliedOn = applied == null ? now : applied;
        m.FinancialYearId = hrm.financialYearId();
        m.Reason = trim(b.reason);
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchId = toInt(u.getBranchesId());
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.ApprovedOn = now;
        m.ApprovedById = u.getId();
        m.NoOfInstallment = toInt(trim(b.noOfInstallments));
        int n = repo.saveLoan(m);
        return saved(n, id > 0 ? "Update Successfully" : "Saved Successfully");
    }

    /**
     * btnPrint_Click / grd_LinkClicked(DocNo): CommonServices.VoucherHeadIdGet(RecId, 1001) - the page then prints
     * hrm-102 (ANewAcRptPaymentReceiptsVoucherSlip_102) with that id. 0 -> the page shows "VoucherId Not Found".
     */
    public Map<String, Object> loanVoucherId(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LOAN);
        if (id == 0) throw invalid("Record Not Found For Display");
        if (!owns(repo.loanHistory(u, null), "EmployeeLoanId", id)) throw invalid("Record Not Found For Display");
        return map("voucherHeadId", repo.voucherHeadId(u, DOC_TYPE_EMPLOYEE_LOAN, id));
    }

    // ================================================================== 664 Employee Advance (frmEmployeeAdvance.cs)

    /** EmployeeFamilyInfo_Load: rights("EmployeeAdvance"), GenerateCode(), EmployeeName(), GridBind(). */
    public Map<String, Object> advanceSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ADVANCE);
        return map("rights", hrm.rights(u, SCREEN_EMPLOYEE_ADVANCE), "docNo", repo.advanceCode(u), "employees", employees(u),
                "rows", advanceGrid(u), "canRegister", hrm.can(u, SCREEN_EMPLOYEE_REGISTRATION, "View"));
    }

    /** GenerateCode() (Reset calls it after GridBind). */
    public Map<String, Object> advanceCode() { return map("docNo", repo.advanceCode(hrm.user(SCREEN_EMPLOYEE_ADVANCE))); }

    public List<Map<String, Object>> advanceEmployees() { return employees(hrm.user(SCREEN_EMPLOYEE_ADVANCE)); }

    public List<Map<String, Object>> advanceHistory() { return advanceGrid(hrm.user(SCREEN_EMPLOYEE_ADVANCE)); }

    /**
     * GridBind(): dtEmp - RequestDate, RequestNo, AdvanceId, EmployeeName, Department, Designation, AppliedDate,
     * AdvanceAmount, ApprovedAmount, Reason, EntryUser, EntryDate, ModifyUser, ApprovedUser, IsApproved (bool), ApprovedDate.
     */
    private List<Map<String, Object>> advanceGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.advanceHistory(u)) {
            out.add(map("RequestDate", r.get("RequestDate"), "RequestNo", r.get("RequestNo"), "AdvanceId", r.get("EmployeeAdvanceId"),
                    "EmployeeName", r.get("EmployeeName"), "Department", r.get("DepartmentName"), "Designation", r.get("DesignationName"),
                    "AppliedDate", r.get("AppliedOn"), "AdvanceAmount", r.get("AdvanceAmount"), "ApprovedAmount", r.get("ApprovedAmount"),
                    "Reason", r.get("Reason"), "EntryUser", r.get("CreatedBy"), "EntryDate", r.get("CreatedOn"), "ModifyUser", r.get("AlteredBy"),
                    "ApprovedUser", r.get("ApprovedBy"), "IsApproved", toBool(r.get("IsApproved")), "ApprovedDate", r.get("ApprovedOn")));
        }
        return out;
    }

    /** RetrivedData(Id): EmployeeAdvance.GetByID (only an id of this company's history). */
    public Map<String, Object> advance(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ADVANCE);
        if (!owns(repo.advanceHistory(u), "EmployeeAdvanceId", id)) throw invalid("Record Not Found");
        Map<String, Object> r = one(repo.advance(id));
        return map("EmployeeAdvanceId", r.get("EmployeeAdvanceId"), "DocNo", str(r.get("DocNo")), "DocDate", r.get("DocDate"),
                "AppliedOn", r.get("AppliedOn"), "EmployeeId", r.get("EmployeeId"), "Reason", str(r.get("Reason")),
                "AdvanceAmount", text(r.get("AdvanceAmount")), "PreviousAdvanceAmount", text(r.get("PreviousAdvanceAmount")),
                "PreviousApprovedAmount", text(r.get("PreviousApprovedAmount")));
    }

    /**
     * PreviousAdvanceAmount(): GetPreviousAdvanceAmount(EmployeeId = cmbEmployeeName.Value, Month / Year of datAppliedOn);
     * rows[0] AdvanceAmount / ApprovedAmount as doubles, or 0 / 0 when there is no row.
     */
    public Map<String, Object> previousAdvance(int employeeId, String appliedOn) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ADVANCE);
        LocalDateTime d = toDate(appliedOn);
        if (d == null) d = LocalDateTime.now();
        List<Map<String, Object>> r = repo.previousAdvance(u, employeeId, d.getMonthValue(), d.getYear());
        if (r.isEmpty()) return map("found", false, "AdvanceAmount", 0, "ApprovedAmount", 0);
        return map("found", true, "AdvanceAmount", toDouble(r.get(0).get("AdvanceAmount")), "ApprovedAmount", toDouble(r.get(0).get("ApprovedAmount")));
    }

    /**
     * Insert(): Validation() ("Employee Name Required!", "Advance Amount Field Required!"), then the model as the form
     * fills it (PayrollMonth / PayrollYear from AppliedOn, DocNo from txtDocNo, IsApproved false) and EmployeeAdvance.Save.
     * The desktop asks no confirmation here.
     */
    public Map<String, Object> saveAdvance(HrmAdvanceDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ADVANCE);
        int id = Math.max(0, b.id);
        hrm.require(u, SCREEN_EMPLOYEE_ADVANCE, id > 0 ? "Update" : "Save");
        if (b.employeeId <= 0 || !ownsEmployee(u, b.employeeId)) throw invalid("Employee Name Required!");
        if (toDouble(b.advanceAmount) == 0.0) throw invalid("Advance Amount Field Required!");
        if (id > 0 && !owns(repo.advanceHistory(u), "EmployeeAdvanceId", id)) throw invalid("Record Not Found");
        LocalDateTime now = LocalDateTime.now();
        EmployeeAdvance m = new EmployeeAdvance();
        m.EmployeeAdvanceId = id;
        m.EmployeeId = b.employeeId;
        m.DocNo = toInt(b.docNo);
        LocalDateTime docDate = toDate(b.requestDate), applied = toDate(b.appliedOn);
        m.DocDate = docDate == null ? now : docDate;
        m.AdvanceAmount = toDec(trim(b.advanceAmount));
        m.PreviousAdvanceAmount = toDec(trim(b.previousAdvance));
        m.PreviousApprovedAmount = toDec(trim(b.previousApprovedAmount));
        m.AppliedOn = applied == null ? now : applied;
        m.Reason = trim(b.reason);
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchId = toInt(u.getBranchesId());
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.ApprovedOn = now;
        m.ApprovedById = u.getId();
        m.PayrollMonth = m.AppliedOn.getMonthValue();
        m.PayrollYear = m.AppliedOn.getYear();
        int n = repo.saveAdvance(m);
        return saved(n, id > 0 ? "Update Successfully" : "Saved Successfully");
    }

    /**
     * GenerateSlip(PrintId): EmployeeAdvance.EmployeeAdvanceSlip(PrintId) - no row -> "Not Record Found For Display";
     * otherwise the page prints hrm-1008. Only an id of this company's history is accepted.
     */
    public Map<String, Object> advanceSlipCheck(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ADVANCE);
        if (id == 0 || !owns(repo.advanceHistory(u), "EmployeeAdvanceId", id)) throw invalid("Not Record Found For Display");
        if (repo.advanceSlip(id).isEmpty()) throw invalid("Not Record Found For Display");
        return map("id", id);
    }
}
