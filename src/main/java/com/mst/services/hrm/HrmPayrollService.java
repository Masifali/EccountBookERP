package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.PayrollAdjustmentDto;
import com.mst.models.hrm.dto.PayrollAllowanceDto;
import com.mst.models.hrm.dto.PayrollAllowanceRowDto;
import com.mst.models.hrm.dto.PayrollLoanDeductionDto;
import com.mst.models.hrm.dto.PayrollPostingDto;
import com.mst.models.hrm.dto.PayrollRowDto;
import com.mst.models.hrm.payroll.EmployeeAllowance;
import com.mst.models.hrm.payroll.EmployeeLateAdjustment;
import com.mst.models.hrm.payroll.EmployeeLoanDeduction;
import com.mst.models.hrm.payroll.EmployeeShortAdjustment;
import com.mst.models.hrm.payroll.Payroll;
import com.mst.models.hrm.payroll.PayrollDetail;
import com.mst.models.hrm.payroll.VoucherDetail;
import com.mst.models.hrm.payroll.VoucherHead;
import com.mst.repositories.hrm.HrmPayrollRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.time.DateTimeException;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.TextStyle;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Payroll" screens (AppModules 2027). Each method names the desktop form / BLL method
 * it reproduces; validation wording and order and the MessageBox texts are the form's.
 *
 *   670 frmPayrollPosting          671 frmEmployeeAllowance      672 frmEmployeeLateAdjustment
 *   673 EmployeeLoanDeduction      674 EmployeeShortAdjustment
 *
 * Value conversion follows Architecture.Common.Conversion exactly: ToInt is Convert.ToInt32 (a text
 * that is not a whole number is 0; a grid double is rounded to even), ToDecimal is Convert.ToDecimal,
 * ToBool is Convert.ToBoolean then Convert.ToInt32 - see {@link #cint}, {@link #cdec}, {@link #cbool}.
 */
@Service
public class HrmPayrollService {

    public static final int SCREEN_POSTING = 670;
    public static final int SCREEN_ALLOWANCE = 671;
    public static final int SCREEN_LATE = 672;
    public static final int SCREEN_LOAN = 673;
    public static final int SCREEN_SHORT = 674;

    /** frmPayrollPosting: DocMovementId / DocumentTypeId 1000. */
    public static final int DOC_PAYROLL = 1000;

    /**
     * tsDropDownPrint items: CommonServices.DynamicReportsLoad("SalarySheet") lists the .rpt files of the
     * Reports\SalarySheet folder. The web server cannot enumerate that folder; the two salary-sheet
     * templates that exist on this installation (rpt_list.txt) and that no other desktop code names are
     * offered (see HrmPayrollReports).
     */
    public static final String[] SALARY_SHEETS = {"1100-EmployeeSalarySheet", "1115-EmployeeSalarySheet"};

    @Autowired private HrmPayrollRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== shared

    private List<Map<String, Object>> departmentsCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.departments(u)) out.add(map("Id", r.get("DepartmentId"), "Name", r.get("DepartmentName")));
        return out;
    }

    private List<Map<String, Object>> categoriesCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.categories(u)) out.add(map("Id", r.get("EmployeeCategoryId"), "Name", r.get("EmployeeCategoryName")));
        return out;
    }

    private static List<Map<String, Object>> employeeCombo(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("EmployeeId"));
            if (!seen.add(id)) continue;   // DISTINCT rows can repeat an employee through several shift timings
            out.add(map("EmployeeId", id, "EmployeeName", r.get("EmployeeName"), "EmployeeNo", r.get("EmployeeNo"),
                    "DesignationName", r.get("DesignationName"), "DepartmentName", r.get("DepartmentName"),
                    "TotalSalary", r.get("TotalSalary"), "SectionName", r.get("SectionName"), "LocationId", r.get("LocationId")));
        }
        return out;
    }

    /** CommonServices.GetYears: ActiveYr.Start_Period.Year .. DateTime.Now.Year as { Id, Year }. */
    private List<Map<String, Object>> yearsFromActiveYear(UserAccount u) {
        int yearId = hrm.financialYearId();
        int start = LocalDate.now().getYear();
        try {
            List<Map<String, Object>> years = repo.financialYears(u);
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) if (toInt(r.get("Id")) == yearId) { row = r; break; }
            if (row == null && !years.isEmpty()) row = years.get(0);
            LocalDateTime s = row == null ? null : toDate(row.get("Start_Period"));
            if (s != null) start = s.getYear();
        } catch (RuntimeException e) { /* keep the current year only */ }
        List<Map<String, Object>> out = new ArrayList<>();
        for (int i = start; i <= LocalDate.now().getYear(); i++) out.add(map("Id", i, "Year", i));
        return out;
    }

    /** Default NoofDecimal Points For Amount (CommonServices.GetDecimalConfiguration). */
    private int amountDecimals(UserAccount u) {
        return cint(repo.config(u.getOrganizationId(), u.getCompanyId(), "Default NoofDecimal Points For Amount"));
    }

    private static int[] ids(List<Map<String, Object>> rows, String key) {
        int[] a = new int[rows.size()];
        for (int i = 0; i < a.length; i++) a[i] = toInt(rows.get(i).get(key));
        return a;
    }

    private static boolean in(int[] ids, int id) { for (int x : ids) if (x == id) return true; return false; }

    private static boolean sameTenant(Map<String, Object> row, UserAccount u) {
        return toInt(row.get("OrganizationId")) == u.getOrganizationId() && toInt(row.get("CompanyId")) == u.getCompanyId();
    }

    // ================================================================== 670 Payroll Posting (frmPayrollPosting.cs)

    /**
     * FrmExportSalesContract_Load: rights (PayrollPosting), EmployeeDepartmentFill, EmployeeCategoryFill,
     * YearFill, MonthFill (fixed list, in the page), GenerateCode, GetAccountsFromEmployee ("Expense"),
     * EmployeeFill (department 0 at load), ReportsLoad.
     */
    public Map<String, Object> postingSetup() {
        UserAccount u = hrm.user(SCREEN_POSTING);
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : repo.accountsFromEmployee(u, "Expense")) accounts.add(map("Id", r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        return map("rights", hrm.rights(u, SCREEN_POSTING),
                "departments", departmentsCombo(u), "categories", categoriesCombo(u), "years", yearsFromActiveYear(u),
                "docNo", repo.payrollCode(u, hrm.financialYearId(), DOC_PAYROLL),
                "debitAccounts", accounts, "employees", employeeCombo(repo.activeEmployees(u, 0)),
                "reports", SALARY_SHEETS, "amountDecimals", amountDecimals(u));
    }

    /** toolStripButton1_Click (Refresh): EmployeeDepartmentFill, EmployeeCategoryFill, YearFill, MonthFill. */
    public Map<String, Object> postingRefresh() {
        UserAccount u = hrm.user(SCREEN_POSTING);
        return map("departments", departmentsCombo(u), "categories", categoriesCombo(u), "years", yearsFromActiveYear(u));
    }

    /** GenerateCode (Reset). */
    public Map<String, Object> postingCode() {
        UserAccount u = hrm.user(SCREEN_POSTING);
        return map("docNo", repo.payrollCode(u, hrm.financialYearId(), DOC_PAYROLL));
    }

    /**
     * btnShowDetail_Click: PayRollReports.GetEmployeePayrollSalary with the form's filters. The page
     * already checked "Month feild is required..." / "Year feild is required..."; the ids picked from
     * the combos must be this company's.
     */
    public List<Map<String, Object>> postingShow(int departmentId, int categoryId, String month, int year, int monthValue,
                                                 int debitAccountId, int employeeId) {
        UserAccount u = hrm.user(SCREEN_POSTING);
        if (monthValue < 1 || monthValue > 12) throw invalid("Month feild is required...");
        if (year == 0) throw invalid("Year feild is required...");
        if (departmentId != 0 && !owns(repo.departments(u), "DepartmentId", departmentId)) throw invalid("Record not found.");
        if (categoryId != 0 && !owns(repo.categories(u), "EmployeeCategoryId", categoryId)) throw invalid("Record not found.");
        if (debitAccountId != 0 && !owns(repo.accountsFromEmployee(u, "Expense"), "Id", debitAccountId)) throw invalid("Record not found.");
        if (employeeId != 0 && !owns(repo.activeEmployees(u, 0), "EmployeeId", employeeId)) throw invalid("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employeePayrollSalary(u, employeeId, categoryId, departmentId, debitAccountId, month, year, monthValue)) {
            /* dtdetail.Rows.Add(0, AccountId, AccountCode, EmployeeNo, EmployeeId, EmployeeName, DesignationName,
               DepartmentName, SectionName, EmployeeHistoryId, GrossSalary, EmployeeDays, EmployeePerDaySalary,
               EmployeePayrollSalary, LateDeductionAmount, IncomeTaxAmount, LeaveAmount, AdvanceAmount, LoanAmount,
               NoOfInstallments, PFAmount, EOBIAmount, MiscLessAmount, TotalLessAmount, OTAmount, TravellingAmount,
               MedicalAmount, MobileAmount, FuelAmount, FoodAmount, MiscAddAmount, ArearAmount, NetSalary (-> Salary),
               0 (AddLessAmount), NetSalary) */
            out.add(detailRow(0, r.get("AccountId"), r.get("AccountCode"), r.get("EmployeeNo"), r.get("EmployeeId"), r.get("EmployeeName"),
                    r.get("DesignationName"), r.get("DepartmentName"), r.get("SectionName"), r.get("EmployeeHistoryId"), r.get("GrossSalary"),
                    r.get("EmployeeDays"), r.get("EmployeePerDaySalary"), r.get("EmployeePayrollSalary"), r.get("LateDeductionAmount"),
                    r.get("IncomeTaxAmount"), r.get("LeaveAmount"), r.get("AdvanceAmount"), r.get("LoanAmount"), r.get("NoOfInstallments"),
                    r.get("PFAmount"), r.get("EOBIAmount"), r.get("MiscLessAmount"), r.get("TotalLessAmount"), r.get("OTAmount"),
                    r.get("TravellingAmount"), r.get("MedicalAmount"), r.get("MobileAmount"), r.get("FuelAmount"), r.get("FoodAmount"),
                    r.get("MiscAddAmount"), r.get("ArearAmount"), r.get("NetSalary"), 0, r.get("NetSalary")));
        }
        return out;
    }

    private static Map<String, Object> detailRow(Object... v) {
        String[] k = {"Id", "AccountId", "AccountCode", "EmployeeNo", "EmployeeId", "EmployeeName", "DesignationName", "DepartmentName",
                "SectionName", "EmployeeHistoryId", "GrossSalary", "EmployeeDays", "EmployeePerDaySalary", "EmployeePayrollSalary",
                "LateDeductionAmount", "IncomeTaxAmount", "LeaveAmount", "AdvanceAmount", "LoanAmount", "LoanNOS", "PFAmount", "EOBIAmount",
                "MiscLessAmount", "TotalLessAmount", "OTAmount", "TravellingAmount", "MedicalAmount", "MobileAmount", "FuelAmount",
                "FoodAmount", "MiscAddAmount", "ArearAmount", "Salary", "AddLessAmount", "NetSalary"};
        Map<String, Object> m = map();
        for (int i = 0; i < k.length; i++) m.put(k[i], v[i]);
        return m;
    }

    /** Payroll.GetByID, only for a payroll of the signed-in company. */
    private Map<String, Object> ownedPayroll(UserAccount u, int id) {
        List<Map<String, Object>> rows = repo.payroll(id);
        if (rows.isEmpty() || !sameTenant(rows.get(0), u)) throw invalid("Record not found.");
        return rows.get(0);
    }

    /**
     * ReadById (Edit button / history double-click / ?id= from a voucher drill-down) and
     * DataGridHistory_SelectionChanged: the header and dtdetail rows as ReadById adds them
     * (Designation / Department / Section blank).
     */
    public Map<String, Object> posting(int id) {
        UserAccount u = hrm.user(SCREEN_POSTING);
        Map<String, Object> h = ownedPayroll(u, id);
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> d : repo.payrollDetails(id)) {
            details.add(detailRow(d.get("PayrollDetailId"), d.get("AccountId"), d.get("AccountCode"), d.get("EmployeeNo"), d.get("EmployeeId"),
                    d.get("EmployeeName"), "", "", "", d.get("EmployeeHistoryId"), d.get("GrossSalary"), d.get("EmployeeDays"),
                    d.get("EmployeePerDaySalary"), d.get("EmployeePayrollSalary"), d.get("LateDeductionAmount"), d.get("IncomeTaxAmount"),
                    d.get("LeaveAmount"), d.get("AdvanceAmount"), d.get("LoanAmount"), d.get("LoanNOS"), d.get("PFAmount"), d.get("EOBIAmount"),
                    d.get("MiscLessAmount"), d.get("TotalAmount"), d.get("OTAmount"), d.get("TravellingAmount"), d.get("MedicalAmount"),
                    d.get("MobileAmount"), d.get("FuelAmount"), d.get("FoodAmount"), d.get("MiscAddAmount"), d.get("ArrearAmount"),
                    d.get("SalaryWithoutAddLess"), d.get("AddLessAmount"), d.get("NetSalary")));
        }
        return map("PayrollId", h.get("PayrollId"), "PayrollDate", h.get("PayrollDate"), "PayrollMonth", h.get("PayrollMonth"),
                "PayrollYear", h.get("PayrollYear"), "DocNo", h.get("DocNo"), "IsApproved", h.get("IsApproved"), "details", details);
    }

    /** HistoryFill: Payroll.ReadAll -> { PayrollId, PayrollDocNo, PayrollDate, PayrollMonth, PayrollYear, EntryUserName }. */
    public List<Map<String, Object>> postingHistory() {
        UserAccount u = hrm.user(SCREEN_POSTING);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.payrolls(u, hrm.financialYearId(), DOC_PAYROLL)) {
            out.add(map("PayrollId", r.get("PayrollId"), "PayrollDocNo", r.get("DocNo"), "PayrollDate", r.get("PayrollDate"),
                    "PayrollMonth", r.get("PayrollMonth"), "PayrollYear", r.get("PayrollYear"), "EntryUserName", r.get("EntryUserName")));
        }
        return out;
    }

    /** DataGridHistory_ColumnButtonClick "Voucher": CommonServices.VoucherHeadIdGet(PayrollId, 1000). */
    public Map<String, Object> postingVoucherHeadId(int id) {
        UserAccount u = hrm.user(SCREEN_POSTING);
        ownedPayroll(u, id);
        return map("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOC_PAYROLL, id));
    }

    /** tsDropDownPrint_DropDownItemClicked: PayRollReports.GetEmployeePostedSalary row count ("Not Record Found For Display" when 0). */
    public Map<String, Object> postingPrintCheck(int payrollId, String month, int year, int monthValue) {
        UserAccount u = hrm.user(SCREEN_POSTING);            // the desktop's Print drop-down checks no Print right
        if (payrollId != 0) ownedPayroll(u, payrollId);
        int n = repo.employeePostedSalary(u, payrollId, month, year, monthValue).size();
        if (n <= 0) throw invalid("Not Record Found For Display");
        return map("rows", n);
    }

    /**
     * btnsave_Click / btnUpdate_Click (the "Are you sure to Save/Update?" confirmation ran in the page):
     * the Payroll header as the form fills it, every grid row with the per-row checks in the form's order,
     * the lstDelete rows, then BLL Payroll.Save - MakeVoucher when IsApproved or the
     * "PayRollPostingWithoutApproval" configuration is on - and DAL SetData in one transaction.
     */
    public Map<String, Object> savePosting(PayrollPostingDto b) {
        UserAccount u = hrm.user(SCREEN_POSTING);
        int recId = Math.max(0, b.id);
        hrm.require(u, SCREEN_POSTING, recId > 0 ? "Update" : "Save");
        Set<Integer> knownDetailIds = new HashSet<>();
        Set<Integer> employees = new HashSet<>();
        for (int e : ids(repo.activeEmployees(u, 0), "EmployeeId")) employees.add(e);
        if (recId > 0) {
            ownedPayroll(u, recId);
            for (Map<String, Object> d : repo.payrollDetails(recId)) {
                knownDetailIds.add(toInt(d.get("PayrollDetailId")));
                employees.add(toInt(d.get("EmployeeId")));
            }
        }
        int decimals = amountDecimals(u);
        LocalDateTime now = LocalDateTime.now();
        Payroll obj = new Payroll();
        if (recId > 0) obj.PayrollId = recId;
        obj.LocationId = 0;
        obj.DocMovementId = DOC_PAYROLL;
        LocalDateTime day = toDay(b.docDate);
        obj.PayrollDate = day == null ? now : day.toLocalDate().atTime(LocalTime.now());   // DateTimePicker.Value carries the time of day
        obj.PayrollMonth = cint(b.month);
        obj.PayrollYear = cint(b.year);
        obj.DocNo = cint(b.docNo);
        obj.PostedById = u.getId();
        obj.CreatedById = u.getId();
        obj.CreatedOn = now;
        obj.AlteredById = u.getId();
        obj.AlteredOn = now;
        obj.ActionTypeId = recId > 0 ? 2 : 1;
        obj.UserLogId = u.getId();
        obj.OrganizationId = u.getOrganizationId();
        obj.CompanyId = u.getCompanyId();
        obj.FinancialYearId = hrm.financialYearId();
        obj.BranchesId = u.getBranchesId();
        obj.IsApproved = false;                       // ApprovedStatus: false from the menu route
        obj.PayrollDetailList = new ArrayList<>();
        List<PayrollRowDto> rows = b.details == null ? new ArrayList<>() : b.details;
        if (rows.isEmpty()) throw invalid("Grid record not found");
        final boolean[] showChecked = {false};
        for (PayrollRowDto r : rows) {
            PayrollDetail vd = new PayrollDetail();
            vd.PayrollDetailId = cint(r.id);
            if (cint(r.employeeId) == 0) throw invalid("Employee Name Required");
            vd.EmployeeId = cint(r.employeeId);
            if (cint(r.employeeHistoryId) == 0) throw invalid("Employee History Required");
            vd.EmployeeHistoryId = cint(r.employeeHistoryId);
            if (cdec(r.grossSalary).signum() == 0) throw invalid("Gross Salary Required");
            vd.GrossSalary = cdec(r.grossSalary);
            if (cint(r.employeeDays) == 0) throw invalid("Employee Days Required");
            vd.EmployeeDays = cint(r.employeeDays);
            if (vd.PayrollDetailId != 0 && !knownDetailIds.contains((int) vd.PayrollDetailId)) throw invalid("Record not found.");
            if (!employees.contains((int) vd.EmployeeId)) {
                /* not in the active list: the rows came from Sp_GetEmployeePayrollSalary (Show Detail), so that
                   company-scoped list for the same month / year is the reference, read once */
                int mv = cint(b.month), yr = cint(b.year);
                if (!showChecked[0] && mv >= 1 && mv <= 12 && yr > 0) {
                    showChecked[0] = true;
                    for (Map<String, Object> e : repo.employeePayrollSalary(u, 0, 0, 0, 0, java.time.Month.of(mv).getDisplayName(TextStyle.FULL, Locale.ENGLISH), yr, mv))
                        employees.add(toInt(e.get("EmployeeId")));
                }
                if (!employees.contains((int) vd.EmployeeId)) throw invalid("Record not found.");
            }
            fillAmounts(vd, r);
            vd.AddLessAmount = cdec(r.addLessAmount);
            vd.SalaryWithoutAddLess = cdec(r.salary).setScale(decimals, RoundingMode.HALF_UP);   // Math.Round(.., AwayFromZero)
            vd.NetSalary = cdec(r.netSalary).setScale(decimals, RoundingMode.HALF_UP);
            vd.InwardId = 0;
            vd.CreatedById = u.getId();
            vd.CreatedOn = now;
            vd.AlteredById = u.getId();
            vd.AlteredOn = now;
            vd.ActionTypeId = vd.PayrollDetailId == 0 ? 1 : 2;
            vd.UserLogId = u.getId();
            obj.PayrollDetailList.add(vd);
        }
        /* grdDetails_ColumnButtonClick "Delete": rows that had an Id join the list with ActionTypeId 3
           (AddLessAmount / SalaryWithoutAddLess are not copied there, and NetSalary is not rounded). */
        if (b.deleted != null) {
            for (PayrollRowDto r : b.deleted) {
                int did = cint(r.id);
                if (did <= 0) continue;
                if (!knownDetailIds.contains(did)) throw invalid("Record not found.");
                PayrollDetail vd = new PayrollDetail();
                vd.ActionTypeId = 3;
                vd.PayrollDetailId = did;
                vd.EmployeeId = cint(r.employeeId);
                vd.EmployeeHistoryId = cint(r.employeeHistoryId);
                vd.GrossSalary = cdec(r.grossSalary);
                vd.EmployeeDays = cint(r.employeeDays);
                fillAmounts(vd, r);
                vd.NetSalary = cdec(r.netSalary);
                vd.InwardId = 0;
                vd.CreatedById = u.getId();
                vd.CreatedOn = now;
                vd.AlteredById = u.getId();
                vd.AlteredOn = now;
                vd.UserLogId = u.getId();
                obj.PayrollDetailList.add(vd);
            }
        }
        /* BLL Payroll.Save */
        boolean flag = cbool(repo.config((int) obj.OrganizationId, (int) obj.CompanyId, "PayRollPostingWithoutApproval"));
        boolean withVoucher = obj.IsApproved || flag;
        if (withVoucher) obj.VoucherHeadInvoices = makeVoucher(obj);
        String proc = obj.PayrollId == 0 ? "hrm.Sp_Payroll_Insert" : "hrm.Sp_Payroll_Update";
        long id = repo.tx(() -> repo.savePayroll(obj, proc, withVoucher));
        return saved((int) id, recId == 0 ? "Record Save Successfully" : "Record Update Successfully");
    }

    private static void fillAmounts(PayrollDetail vd, PayrollRowDto r) {
        vd.EmployeePerDaySalary = cdec(r.employeePerDaySalary);
        vd.EmployeePayrollSalary = cdec(r.employeePayrollSalary);
        vd.LateDeductionAmount = cdec(r.lateDeductionAmount);
        vd.IncomeTaxAmount = cdec(r.incomeTaxAmount);
        vd.LeaveAmount = cdec(r.leaveAmount);
        vd.AdvanceAmount = cdec(r.advanceAmount);
        vd.LoanAmount = cdec(r.loanAmount);
        vd.LoanNOS = cint(r.loanNos);
        vd.PFAmount = cdec(r.pfAmount);
        vd.EOBIAmount = cdec(r.eobiAmount);
        vd.FoodAmount = cdec(r.foodAmount);
        vd.MiscLessAmount = cdec(r.miscLessAmount);
        vd.TotalAmount = cdec(r.totalLessAmount);
        vd.OTAmount = cdec(r.otAmount);
        vd.TravellingAmount = cdec(r.travellingAmount);
        vd.MedicalAmount = cdec(r.medicalAmount);
        vd.MobileAmount = cdec(r.mobileAmount);
        vd.FuelAmount = cdec(r.fuelAmount);
        vd.MiscAddAmount = cdec(r.miscAddAmount);
        vd.ArrearAmount = cdec(r.arearAmount);
    }

    /** BLL Payroll.MakeVoucher (0187): one Expense Dr / Payable Cr pair per employee, plus a Payable Dr / Loan Cr pair when LoanAmount > 0. */
    private VoucherHead makeVoucher(Payroll obj) {
        VoucherHead vh = new VoucherHead();
        int num = (int) obj.PayrollId;
        vh.DocumentTypeId = obj.DocMovementId;
        vh.DocumentTypeSrNo = num;
        vh.RefDocNoId = num;
        vh.VoucherCode = obj.DocNo;
        vh.VoucherDate = obj.PayrollDate;
        vh.Remarks = obj.Remarks == null ? "" : obj.Remarks;
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = obj.BranchesId;
        vh.ProjectId = obj.ProjectsId;
        vh.ManualBillNo = String.valueOf(obj.DocNo);
        vh.DueDate = LocalDateTime.now();
        vh.OrganizationId = (int) obj.OrganizationId;
        vh.CompanyId = (int) obj.CompanyId;
        vh.FinancialYearId = obj.FinancialYearId;
        vh.EntryUser = (int) obj.CreatedById;
        vh.EntryDate = obj.CreatedOn;
        vh.ModifyDate = obj.AlteredOn;
        vh.ModifyUser = (int) obj.AlteredById;
        List<Map<String, Object>> list = repo.glAccountsByEmployee(vh.OrganizationId, vh.CompanyId);
        if (obj.PayrollDetailList.isEmpty()) return vh;
        double num2 = 0.0;
        String text;
        try {
            text = LocalDate.of(obj.PayrollYear, obj.PayrollMonth, 1).getMonth().getDisplayName(TextStyle.SHORT, Locale.ENGLISH);
        } catch (DateTimeException e) {
            throw invalid("Year, Month, and Day parameters describe an un-representable DateTime.");
        }
        for (PayrollDetail item : obj.PayrollDetailList) {
            if (item.ActionTypeId == 3) continue;
            if (list.isEmpty()) throw invalid("Employee Accounts List Not found");
            Map<String, Object> acc = null;
            for (Map<String, Object> a : list) if (toInt(a.get("EmployeeId")) == (int) item.EmployeeId) { acc = a; break; }
            if (acc == null) throw invalid("Employee Accounts List Not found");
            int expense = toInt(acc.get("ExpenseAccountId")), payable = toInt(acc.get("PayableAcId")), loan = toInt(acc.get("LoanAcId"));
            String empty = buildSalaryRemarks(item, text, obj.PayrollYear);
            double num3 = item.NetSalary.doubleValue() + item.AdvanceAmount.doubleValue() + item.LoanAmount.doubleValue();
            VoucherDetail d1 = new VoucherDetail();
            d1.AccountId = expense;
            d1.AgainstAccountId = payable;
            d1.Comments = empty;
            d1.DebitAmount = num3;
            vh.voucherDetailList.add(d1);
            VoucherDetail d2 = new VoucherDetail();
            d2.AccountId = payable;
            d2.AgainstAccountId = expense;
            d2.Comments = empty;
            d2.CreditAmount = num3;
            d2.EmployeeId = (int) item.EmployeeId;
            d2.SubsidiaryTypeId = 2;
            d2.SubsidiaryAccountId = (int) item.EmployeeId;
            vh.voucherDetailList.add(d2);
            num2 += d2.CreditAmount;
            if (item.LoanAmount.signum() > 0) {
                empty = "Salary M/O " + text + " - " + obj.PayrollYear;
                empty = empty + "  Loan No of Instalments " + item.LoanNOS;
                empty = empty + "  and Loan Amount is " + roundAway0(item.LoanAmount);
                VoucherDetail d3 = new VoucherDetail();
                d3.AccountId = payable;
                d3.AgainstAccountId = loan;
                d3.Comments = empty;
                d3.DebitAmount = item.LoanAmount.doubleValue();
                d3.EmployeeId = (int) item.EmployeeId;
                d3.SubsidiaryTypeId = 2;
                d3.SubsidiaryAccountId = (int) item.EmployeeId;
                vh.voucherDetailList.add(d3);
                VoucherDetail d4 = new VoucherDetail();
                d4.AccountId = loan;
                d4.AgainstAccountId = payable;
                d4.Comments = empty;
                d4.CreditAmount = item.LoanAmount.doubleValue();
                d4.EmployeeId = (int) item.EmployeeId;
                d4.SubsidiaryTypeId = 2;
                d4.SubsidiaryAccountId = (int) item.EmployeeId;
                vh.voucherDetailList.add(d4);
                num2 += d4.CreditAmount;
            }
        }
        vh.VoucherAmount = num2;
        vh.BillAmount = num2;
        return vh;
    }

    /** BLL Payroll.BuildSalaryRemarks: every non-zero amount, rounded away from zero to 0 places except "Extra AddLess". */
    private static String buildSalaryRemarks(PayrollDetail item, String monthName, int payrollYear) {
        StringBuilder text = new StringBuilder("Salary M/O " + monthName + " - " + payrollYear + " Gross Salary " + roundAway0(item.GrossSalary));
        Object[][] array = {
                {"Late Deduction", item.LateDeductionAmount, true}, {"IncomeTax", item.IncomeTaxAmount, true},
                {"LeaveAmount", item.LeaveAmount, true}, {"Advance", item.AdvanceAmount, true}, {"Loan", item.LoanAmount, true},
                {"PFAmount", item.PFAmount, true}, {"EOBIA", item.EOBIAmount, true}, {"MiscLessAmount", item.MiscLessAmount, true},
                {"Total Less", item.TotalAmount, true}, {"OTAmount", item.OTAmount, true}, {"Travelling", item.TravellingAmount, true},
                {"Medical", item.MedicalAmount, true}, {"MobileAmount", item.MobileAmount, true}, {"FuelAmount", item.FuelAmount, true},
                {"FoodAmount", item.FoodAmount, true}, {"MiscAddAmount", item.MiscAddAmount, true},
                {"Extra AddLess", item.AddLessAmount, false}, {"NetSalary", item.NetSalary, true}};
        for (Object[] t : array) {
            BigDecimal v = (BigDecimal) t[1];
            if (v.signum() == 0) continue;
            String num = (Boolean) t[2] ? roundAway0(v) : plain(v);
            text.append("  ").append(t[0]).append(' ').append(num);
        }
        return text.toString();
    }

    private static String roundAway0(BigDecimal v) { return v.setScale(0, RoundingMode.HALF_UP).toPlainString(); }

    /** decimal.ToString() of a value the grid held as a double (Convert.ToDecimal keeps no trailing zeros). */
    private static String plain(BigDecimal v) {
        BigDecimal s = v.stripTrailingZeros();
        return (s.scale() < 0 ? s.setScale(0) : s).toPlainString();
    }

    // ================================================================== 671 Employee Allowance (frmEmployeeAllowance.cs)

    /** FrmExportSalesContract_Load: rights (EmployeeAllowance), EmployeeFill (GetAllEmployeesActive). */
    public Map<String, Object> allowanceSetup() {
        UserAccount u = hrm.user(SCREEN_ALLOWANCE);
        return map("rights", hrm.rights(u, SCREEN_ALLOWANCE), "employees", employeeCombo(repo.activeEmployees(u, 0)));
    }

    /** btnRefresh_Click_1: EmployeeFill (EmployeeBenefitFill follows in the page for the current employee). */
    public List<Map<String, Object>> allowanceEmployees() {
        UserAccount u = hrm.user(SCREEN_ALLOWANCE);
        return employeeCombo(repo.activeEmployees(u, 0));
    }

    /** EmployeeBenefitFill: hrmEmployeeBenefit.ReadByEmployeeId -> { BenefitId, Description }. */
    public List<Map<String, Object>> allowanceBenefits(int employeeId) {
        UserAccount u = hrm.user(SCREEN_ALLOWANCE);
        if (!owns(repo.activeEmployees(u, 0), "EmployeeId", employeeId)) return new ArrayList<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeBenefits(u, employeeId)) out.add(map("BenefitId", r.get("BenefitId"), "Description", r.get("Description")));
        return out;
    }

    /** GetByEmployeeId: EmployeeAllowance.ReadByEmployeeId -> dthead rows. */
    public List<Map<String, Object>> allowancesOf(int employeeId) {
        UserAccount u = hrm.user(SCREEN_ALLOWANCE);
        List<Map<String, Object>> out = new ArrayList<>();
        if (!owns(repo.activeEmployees(u, 0), "EmployeeId", employeeId)) return out;
        for (Map<String, Object> r : repo.allowancesByEmployee(u, employeeId)) {
            out.add(map("Id", r.get("EmployeeAllowanceId"), "EmployeeId", r.get("EmployeeId"), "Employee", r.get("EmployeeName"),
                    "EmployeeBenefitId", r.get("EmployeeBenefitId"), "EmployeeBenefit", r.get("EmployeeBenefit"), "AppliedOn", r.get("AppliedOn"),
                    "TotalAmount", r.get("TotalAmount"), "IsForPayroll", r.get("IsForPayroll"), "Remarks", r.get("Remarks")));
        }
        return out;
    }

    /**
     * btnsave_Click (confirmation in the page): one EmployeeAllowance per grid row (ActionTypeId 1 / 2 by
     * Id, IsApproved false, Approved* = user / now) plus the lstDelete rows that had an Id (ActionTypeId 3),
     * then EmployeeAllowance.Save -> DAL SetData (hrm.Sp_EmployeeAllowance_Insert per row, one transaction).
     */
    public Map<String, Object> saveAllowance(PayrollAllowanceDto b) {
        UserAccount u = hrm.user(SCREEN_ALLOWANCE);
        hrm.require(u, SCREEN_ALLOWANCE, "Save");
        List<PayrollAllowanceRowDto> rows = b.rows == null ? new ArrayList<>() : b.rows;
        if (rows.isEmpty()) throw invalid("Grid record not found");
        int[] employees = ids(repo.activeEmployees(u, 0), "EmployeeId");
        LocalDateTime now = LocalDateTime.now();
        List<EmployeeAllowance> list = new ArrayList<>();
        for (PayrollAllowanceRowDto r : rows) {
            EmployeeAllowance vd = allowanceModel(u, r, employees, now, true);
            vd.ActionTypeId = vd.EmployeeAllowanceId == 0 ? 1 : 2;
            list.add(vd);
        }
        if (b.deleted != null) {
            for (PayrollAllowanceRowDto r : b.deleted) {
                if (cint(r.id) == 0) continue;
                EmployeeAllowance vd = allowanceModel(u, r, employees, now, false);
                vd.ActionTypeId = 3;
                list.add(vd);
            }
        }
        repo.saveAllowances(list);
        return saved(0, "Receod Save Successfully");
    }

    private EmployeeAllowance allowanceModel(UserAccount u, PayrollAllowanceRowDto r, int[] employees, LocalDateTime now, boolean checkBenefit) {
        EmployeeAllowance vd = new EmployeeAllowance();
        vd.EmployeeAllowanceId = cint(r.id);
        vd.EmployeeId = cint(r.employeeId);
        vd.EmployeeBenefitId = cint(r.employeeBenefitId);
        if (!in(employees, (int) vd.EmployeeId)) throw invalid("Employee Required");
        int savedBenefit = -1;
        if (vd.EmployeeAllowanceId != 0) {
            Map<String, Object> saved = null;
            for (Map<String, Object> a : repo.allowancesByEmployee(u, (int) vd.EmployeeId))
                if (toInt(a.get("EmployeeAllowanceId")) == (int) vd.EmployeeAllowanceId) { saved = a; break; }
            if (saved == null) throw invalid("Record not found.");
            savedBenefit = toInt(saved.get("EmployeeBenefitId"));
        }
        /* a benefit picked in cmbEmployeeBenefit must be one of the employee's own (EmployeeBenefitFill) */
        if (checkBenefit && savedBenefit != (int) vd.EmployeeBenefitId
                && !owns(repo.employeeBenefits(u, (int) vd.EmployeeId), "BenefitId", (int) vd.EmployeeBenefitId))
            throw invalid("Employee Benefit Required");
        LocalDateTime applied = toDay(r.appliedOn);
        vd.AppliedOn = applied == null ? LocalDateTime.of(1900, 1, 1, 0, 0) : applied;   // Conversion.ToDateTime fallback
        vd.TotalAmount = cdec(r.totalAmount);
        vd.IsForPayroll = cbool(r.isForPayroll);
        vd.Remarks = r.remarks == null ? "" : r.remarks;
        vd.OrganizationId = u.getOrganizationId();
        vd.CompanyId = u.getCompanyId();
        vd.CreatedOn = now;
        vd.AlteredOn = now;
        vd.CreatedById = u.getId();
        vd.AlteredById = u.getId();
        vd.ApprovedOn = now;
        vd.ApprovedById = u.getId();
        vd.IsApproved = false;
        vd.UserLogId = u.getId();
        return vd;
    }

    // ================================================================== 672 Late / 674 Short adjustment

    /** EmployeeFamilyInfo_Load (672): rights, MonthFill (page), YearFill (GetYears), EmployeeName (GetAllActiveEmployee), GridBind. */
    public Map<String, Object> lateSetup() {
        UserAccount u = hrm.user(SCREEN_LATE);
        return map("rights", hrm.rights(u, SCREEN_LATE), "years", yearsFromActiveYear(u),
                "employees", employeeCombo(repo.allActiveEmployees(u)), "rows", lateGrid(u));
    }

    public List<Map<String, Object>> lateList() { return lateGrid(hrm.user(SCREEN_LATE)); }

    public List<Map<String, Object>> lateEmployees() { return employeeCombo(repo.allActiveEmployees(hrm.user(SCREEN_LATE))); }

    /** GridBind: FormHistory -> the 12 columns the form copies. */
    private List<Map<String, Object>> lateGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.lateAdjustments(u)) {
            out.add(map("EmployeeLateAdjustmentId", r.get("EmployeeLateAdjustmentId"), "EmployeeId", r.get("EmployeeId"),
                    "EmployeeName", r.get("EmployeeName"), "LocationId", r.get("LocationId"), "LAMonth", r.get("LAMonth"), "LAYear", r.get("LAYear"),
                    "TotalLates", r.get("TotalLates"), "TotalLateDays", r.get("TotalLateDays"), "DeductedDays", r.get("DeductedDays"),
                    "DeductionRate", r.get("DeductionRate"), "IsDeduction", r.get("IsDeduction"), "IsLeaveAdjust", r.get("IsLeaveAdjust")));
        }
        return out;
    }

    /** RetrivedData: EmployeeLateAdjustment.GetByID (a record of the signed-in company only). */
    public Map<String, Object> late(int id) {
        UserAccount u = hrm.user(SCREEN_LATE);
        List<Map<String, Object>> rows = repo.lateAdjustment(id);
        if (rows.isEmpty() || !sameTenant(rows.get(0), u)) throw invalid("Record Not Found");
        Map<String, Object> r = rows.get(0);
        return map("Id", r.get("EmployeeLateAdjustmentId"), "EmployeeId", r.get("EmployeeId"), "LAMonth", r.get("LAMonth"), "LAYear", r.get("LAYear"),
                "TotalLates", cint(r.get("TotalLates")), "TotalLateDays", cint(r.get("TotalLateDays")), "DeductedDays", cint(r.get("DeductedDays")),
                "DeductionRate", cint(r.get("DeductionRate")), "IsDeduction", cbool(r.get("IsDeduction")), "IsLeaveAdjust", cbool(r.get("IsLeaveAdjust")));
    }

    /**
     * Insert() (672): Validation() in the form's order, the model as the form fills it (LocationId from
     * the employee's row, text boxes through Conversion.ToInt), BLL Save (_Insert / _Update by id).
     */
    public Map<String, Object> saveLate(PayrollAdjustmentDto b) {
        UserAccount u = hrm.user(SCREEN_LATE);
        int recId = Math.max(0, b.id);
        hrm.require(u, SCREEN_LATE, recId > 0 ? "Update" : "Save");
        Map<String, Object> emp = employeeRow(repo.allActiveEmployees(u), cint(b.employeeId));
        if (emp == null) throw invalid("Employee Name Required!");
        if (toDouble(b.totalLates) == 0.0) throw invalid("Total Lates Field Required!");
        if (toDouble(b.totalLateDays) == 0.0) throw invalid("Total Late Days Field Required!");
        if (toDouble(b.deductedDays) == 0.0) throw invalid("Total Deducted Days Field Required!");
        if (toDouble(b.deductionRate) == 0.0) throw invalid("Total Deduction Rate Field Required!");
        if (recId > 0) {
            List<Map<String, Object>> rows = repo.lateAdjustment(recId);
            if (rows.isEmpty() || !sameTenant(rows.get(0), u)) throw invalid("Record Not Found");
        }
        LocalDateTime now = LocalDateTime.now();
        EmployeeLateAdjustment obj = new EmployeeLateAdjustment();
        if (recId > 0) obj.EmployeeLateAdjustmentId = recId;
        obj.EmployeeId = cint(b.employeeId);
        obj.LocationId = toInt(emp.get("LocationId"));
        obj.LAMonth = cint(b.month);
        obj.LAYear = cint(b.year);
        obj.TotalLates = cint(b.totalLates);
        obj.TotalLateDays = BigDecimal.valueOf(cint(b.totalLateDays));
        obj.DeductedDays = BigDecimal.valueOf(cint(b.deductedDays));
        obj.DeductionRate = BigDecimal.valueOf(cint(b.deductionRate));
        obj.IsDeduction = b.isDeduction;
        obj.IsLeaveAdjust = b.isLeaveAdjust;
        obj.OrganizationId = u.getOrganizationId();
        obj.CompanyId = u.getCompanyId();
        obj.CreatedById = u.getId();
        obj.CreatedOn = now;
        obj.AlteredById = u.getId();
        obj.AlteredOn = now;
        obj.ApprovedOn = now;
        obj.ApprovedById = u.getId();
        long id = repo.saveLateAdjustment(obj);
        return saved((int) id, recId > 0 ? "Update Successfully" : "Saved Successfully");
    }

    /** EmployeeFamilyInfo_Load (674): rights, EmployeeName, YearFill (1900..now, in the page), MonthFill, GridBind. */
    public Map<String, Object> shortSetup() {
        UserAccount u = hrm.user(SCREEN_SHORT);
        return map("rights", hrm.rights(u, SCREEN_SHORT), "employees", employeeCombo(repo.allActiveEmployees(u)), "rows", shortGrid(u));
    }

    public List<Map<String, Object>> shortList() { return shortGrid(hrm.user(SCREEN_SHORT)); }

    public List<Map<String, Object>> shortEmployees() { return employeeCombo(repo.allActiveEmployees(hrm.user(SCREEN_SHORT))); }

    private List<Map<String, Object>> shortGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.shortAdjustments(u)) {
            out.add(map("EmployeeShortAdjustmentId", r.get("EmployeeShortAdjustmentId"), "EmployeeId", r.get("EmployeeId"),
                    "EmployeeName", r.get("EmployeeName"), "LocationId", r.get("LocationId"), "SAMonth", r.get("SAMonth"), "SAYear", r.get("SAYear"),
                    "TotalShortHours", r.get("TotalShortHours"), "TotalShortMinutes", r.get("TotalShortMinutes"),
                    "DeductedShortHours", r.get("DeductedShortHours"), "DeductionRate", r.get("DeductionRate"),
                    "IsDeduction", r.get("IsDeduction"), "IsLeaveAdjust", r.get("IsLeaveAdjust")));
        }
        return out;
    }

    /** RetrivedData (674): EmployeeShortAdjustment.GetByID. */
    public Map<String, Object> shortAdjustment(int id) {
        UserAccount u = hrm.user(SCREEN_SHORT);
        List<Map<String, Object>> rows = repo.shortAdjustment(id);
        if (rows.isEmpty() || !sameTenant(rows.get(0), u)) throw invalid("Record Not Found");
        Map<String, Object> r = rows.get(0);
        return map("Id", r.get("EmployeeShortAdjustmentId"), "EmployeeId", r.get("EmployeeId"), "SAMonth", r.get("SAMonth"), "SAYear", r.get("SAYear"),
                "TotalShortHours", cint(r.get("TotalShortHours")), "TotalShortMinutes", cint(r.get("TotalShortMinutes")),
                "DeductedShortHours", cint(r.get("DeductedShortHours")), "DeductionRate", cint(r.get("DeductionRate")),
                "IsDeduction", cbool(r.get("IsDeduction")), "IsLeaveAdjust", cbool(r.get("IsLeaveAdjust")));
    }

    /** Insert() (674). */
    public Map<String, Object> saveShort(PayrollAdjustmentDto b) {
        UserAccount u = hrm.user(SCREEN_SHORT);
        int recId = Math.max(0, b.id);
        hrm.require(u, SCREEN_SHORT, recId > 0 ? "Update" : "Save");
        Map<String, Object> emp = employeeRow(repo.allActiveEmployees(u), cint(b.employeeId));
        if (emp == null) throw invalid("Employee Name Required!");
        if (toDouble(b.totalHours) == 0.0) throw invalid("Total Hours Field Required!");
        if (toDouble(b.deductedShortHours) == 0.0) throw invalid("Total Deducted Hours Field Required!");
        if (toDouble(b.deductionRate) == 0.0) throw invalid("Total Deduction Rate Field Required!");
        if (recId > 0) {
            List<Map<String, Object>> rows = repo.shortAdjustment(recId);
            if (rows.isEmpty() || !sameTenant(rows.get(0), u)) throw invalid("Record Not Found");
        }
        LocalDateTime now = LocalDateTime.now();
        EmployeeShortAdjustment obj = new EmployeeShortAdjustment();
        if (recId > 0) obj.EmployeeShortAdjustmentId = recId;
        obj.EmployeeId = cint(b.employeeId);
        obj.LocationId = toInt(emp.get("LocationId"));
        obj.SAMonth = cint(b.month);
        obj.SAYear = cint(b.year);
        obj.TotalShortHours = cint(b.totalHours);
        obj.TotalShortMinutes = cint(b.totalMinutes);
        obj.DeductedShortHours = cint(b.deductedShortHours);
        obj.DeductionRate = BigDecimal.valueOf(cint(b.deductionRate));
        obj.IsDeduction = b.isDeduction;
        obj.IsLeaveAdjust = b.isLeaveAdjust;
        obj.OrganizationId = u.getOrganizationId();
        obj.CompanyId = u.getCompanyId();
        obj.CreatedById = u.getId();
        obj.CreatedOn = now;
        obj.AlteredById = u.getId();
        obj.AlteredOn = now;
        obj.ApprovedOn = now;
        obj.ApprovedById = u.getId();
        long id = repo.saveShortAdjustment(obj);
        return saved((int) id, recId > 0 ? "Update Successfully" : "Saved Successfully");
    }

    private static Map<String, Object> employeeRow(List<Map<String, Object>> rows, int employeeId) {
        if (employeeId == 0) return null;
        for (Map<String, Object> r : rows) if (toInt(r.get("EmployeeId")) == employeeId) return r;
        return null;
    }

    // ================================================================== 673 Employee Loan Deduction (EmployeeLoanDeduction.cs)

    /** EmployeeFamilyInfo_Load: rights (EmployeeLoanDeduction); YearFill / MonthFill are fixed lists in the page. */
    public Map<String, Object> loanSetup() {
        UserAccount u = hrm.user(SCREEN_LOAN);
        return map("rights", hrm.rights(u, SCREEN_LOAN));
    }

    /** PendingForDeductionFill (btnLoad_Click): GetLoanInstallmentsForLoanDeductions -> dtPending (NoOfInstallment 1, Amount 0). */
    public List<Map<String, Object>> loanPending() {
        UserAccount u = hrm.user(SCREEN_LOAN);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.loanInstallments(u)) {
            out.add(map("Id", r.get("EmployeeLoanDeductionId"), "EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"),
                    "RemaningNoOfInstallment", r.get("RemaningNoOfInstallment"), "InstallmentAmount", r.get("InstallmentAmount"),
                    "NoOfInstallment", 1, "Amount", 0));
        }
        return out;
    }

    /**
     * History tab: EmployeeLoanDeduction.FormHistory (ReadAll). The desktop's tabControl1_SelectedIndexChanged
     * is an empty handler, so its History grid stays empty; the BLL's own FormHistory is what fills it here.
     */
    public List<Map<String, Object>> loanHistory() {
        UserAccount u = hrm.user(SCREEN_LOAN);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.loanDeductions(u)) {
            out.add(map("EmployeeLoanDeductionId", r.get("EmployeeLoanDeductionId"), "LocationName", r.get("LocationName"),
                    "PayrollMonth", r.get("PayrollMonth"), "PayrollYear", r.get("PayrollYear"), "EmployeeName", r.get("EmployeeName"),
                    "NoOfInstallment", r.get("NoOfInstallment"), "InstallmentAmount", r.get("InstallmentAmount"),
                    "TotalLoanAmount", r.get("TotalLoanAmount"), "CreatedByName", r.get("CreatedByName"), "CreatedOn", r.get("CreatedOn"),
                    "AlteredByName", r.get("AlteredByName"), "AlteredOn", r.get("AlteredOn")));
        }
        return out;
    }

    /** Insert(): "Month Required" / "Year Required", one EmployeeLoanDeduction per grid row, BLL Save -> DAL SetData. */
    public Map<String, Object> saveLoan(PayrollLoanDeductionDto b) {
        UserAccount u = hrm.user(SCREEN_LOAN);
        hrm.require(u, SCREEN_LOAN, "Save");
        int month = cint(b.month), year = cint(b.year);
        if (month < 1 || month > 12) throw invalid("Month Required");
        if (year == 0) throw invalid("Year Required");
        int[] pendingEmployees = ids(repo.loanInstallments(u), "EmployeeId");
        int[] savedIds = null;
        LocalDateTime now = LocalDateTime.now();
        List<EmployeeLoanDeduction> list = new ArrayList<>();
        if (b.rows != null) {
            for (PayrollLoanDeductionDto.Row r : b.rows) {
                EmployeeLoanDeduction obj2 = new EmployeeLoanDeduction();
                obj2.EmployeeLoanDeductionId = cint(r.id);
                obj2.EmployeeId = cint(r.employeeId);
                if (!in(pendingEmployees, (int) obj2.EmployeeId)) throw invalid("Record not found.");
                if (obj2.EmployeeLoanDeductionId != 0) {
                    if (savedIds == null) savedIds = ids(repo.loanDeductions(u), "EmployeeLoanDeductionId");
                    if (!in(savedIds, (int) obj2.EmployeeLoanDeductionId)) throw invalid("Record not found.");
                }
                obj2.InstallmentAmount = cdec(r.installmentAmount);
                obj2.NoOfInstallment = cint(r.noOfInstallment);
                obj2.TotalLoanAmount = BigDecimal.valueOf(cint(r.amount));
                obj2.PayrollMonth = month;
                obj2.PayrollYear = year;
                obj2.OrganizationId = u.getOrganizationId();
                obj2.CompanyId = u.getCompanyId();
                obj2.CreatedById = u.getId();
                obj2.CreatedOn = now;
                obj2.AlteredById = u.getId();
                obj2.AlteredOn = now;
                list.add(obj2);
            }
        }
        long id = repo.saveLoanDeductions(list);
        return saved((int) id, "Saved Successfully");
    }

    // ================================================================== Conversion.* (exact .NET semantics)

    /**
     * Conversion.ToInt = Convert.ToInt32(value) inside try/catch: a whole-number text parses, any other
     * text is 0; a double / decimal is rounded to the nearest even integer (Convert.ToInt32(double)).
     */
    static int cint(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v instanceof Integer || v instanceof Long || v instanceof Short || v instanceof Byte) {
            long l = ((Number) v).longValue();
            return l > Integer.MAX_VALUE || l < Integer.MIN_VALUE ? 0 : (int) l;
        }
        if (v instanceof Number) {
            BigDecimal d = v instanceof BigDecimal ? (BigDecimal) v : new BigDecimal(String.valueOf(((Number) v).doubleValue()));
            try { return d.setScale(0, RoundingMode.HALF_EVEN).intValueExact(); } catch (ArithmeticException e) { return 0; }
        }
        String s = String.valueOf(v).trim();
        if (!s.matches("[+-]?\\d+")) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDecimal = Convert.ToDecimal: numbers as they are (a double to 15 significant digits), text parsed, else 0. */
    static BigDecimal cdec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Double || v instanceof Float) {
            double d = ((Number) v).doubleValue();
            if (Double.isNaN(d) || Double.isInfinite(d)) return BigDecimal.ZERO;
            return new BigDecimal(String.valueOf(d)).round(new MathContext(15, RoundingMode.HALF_EVEN));
        }
        if (v instanceof Number) return BigDecimal.valueOf(((Number) v).longValue());
        return toDec(v);
    }

    /** Conversion.ToBool: Convert.ToBoolean ("true" / "false"), else Convert.ToBoolean(Convert.ToInt32(value)), else false. */
    static boolean cbool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).doubleValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.equalsIgnoreCase("true")) return true;
        if (s.equalsIgnoreCase("false")) return false;
        return cint(s) != 0;
    }
}
