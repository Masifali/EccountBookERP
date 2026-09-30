package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.DesktopModel;
import com.mst.models.hrm.payroll.EmployeeAllowance;
import com.mst.models.hrm.payroll.EmployeeLateAdjustment;
import com.mst.models.hrm.payroll.EmployeeLoanDeduction;
import com.mst.models.hrm.payroll.EmployeeShortAdjustment;
import com.mst.models.hrm.payroll.Payroll;
import com.mst.models.hrm.payroll.PayrollDetail;
import com.mst.models.hrm.payroll.VoucherDetail;
import com.mst.models.hrm.payroll.VoucherHead;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * DAL of the HRM "Payroll" screens (AppModules 2027), Architecture.DAL/BLL.HRM.PayrollManagement.*.
 * Every call is the desktop's own procedure with the parameters its BLL / DAL sends; no inline SQL.
 *
 *   670 Payroll Posting            BLL 0187 Payroll / 0188 PayRollReports, DAL 0154
 *   671 Employee Allowance         BLL 0183 / DAL 0150
 *   672 Employee Late Adjustment   BLL 0184 / DAL 0151
 *   673 Employee Loan Deduction    BLL 0185 / DAL 0152
 *   674 Employee Short Adjustment  BLL 0186 / DAL 0153
 * plus the shared lookups those forms bind (genDepartment / genEmployeeCategory / genEmployeeHistory /
 * hrmEmployeeBenefit Getall, CommonServices configuration and voucher-head lookups).
 */
@Repository
public class HrmPayrollRepository {

    private final HrmProcRepository db;

    public HrmPayrollRepository(HrmProcRepository db) { this.db = db; }

    public <T> T tx(Supplier<T> work) { return db.tx(work); }

    // ================================================================== shared lookups

    /** genDepartment.Getall (BLL 0166): Sp_genDepartment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployeeCategory.Getall (BLL 0168): Sp_genEmployeeCategory_GetAllMethod @OrganizationId (OrginizationId), @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> categories(UserAccount u) {
        return db.rows("Sp_genEmployeeCategory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * genEmployeeHistory.GetAllEmployeesActive (BLL 0200 :119): Sp_genEmployeeHistory_GetAllMethod
     * @OrganizationId, @CompanyId, @DepartmentId (only when != 0), @Activity 'ReadAllActiveEmployee'.
     * The other guarded filters (DepartmentIds, Designation, Section, Group, Store, Location, Shift) are
     * never set by the payroll forms, so they are omitted.
     */
    public List<Map<String, Object>> activeEmployees(UserAccount u, int departmentId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        p.put("Activity", "ReadAllActiveEmployee");
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", p);
    }

    /**
     * genEmployeeHistory.GetAllActiveEmployee (BLL 0200 :85): the same procedure and activity with
     * @OrganizationId, @CompanyId (ShiftId 0 -> omitted). Late / Short adjustment bind it.
     */
    public List<Map<String, Object>> allActiveEmployees(UserAccount u) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllActiveEmployee");
    }

    /** genEmployeeHistory.GetAccountsFromEmployee(org, company, "Expense"): USP_GetAccountsFromEmployee. */
    public List<Map<String, Object>> accountsFromEmployee(UserAccount u, String activity) {
        return db.rows("USP_GetAccountsFromEmployee", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", activity);
    }

    /** hrmEmployeeBenefit.ReadByEmployeeId: Sp_hrmEmployeeBenefit_GetAllMethod @Activity 'ReadByEmployeeId', @OrganizationId, @CompanyId, @EmployeeId. */
    public List<Map<String, Object>> employeeBenefits(UserAccount u, int employeeId) {
        return db.rows("Sp_hrmEmployeeBenefit_GetAllMethod", "Activity", "ReadByEmployeeId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "EmployeeId", employeeId);
    }

    /** CommonServices.GetYears reads clsGlobalVariables.ActiveYr: Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId (the login's year list). */
    public List<Map<String, Object>> financialYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /**
     * CommonServices.GetConfigurationFromAllocation / GetConfigurationByOrgCompandConfigDescription:
     * Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId, @CompanyId, @ConfigDescription,
     * @Activity 'GetConfigurationByOrgCompandConfigDescription' -> Rows[0]["ConfigKey"] ("" when none).
     */
    public String config(int org, int comp, String description) {
        List<Map<String, Object>> r = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", org, "CompanyId", comp,
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    /**
     * CommonServices.GetVoucherHeadId (DAL 0205 :58) and VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId
     * (BLL 0654 :572, used by CommonServices.VoucherHeadIdGet): Sp_Vouchers_GetMethods @Activity
     * 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', @OrganizationId, @CompanyId, @DocumentTypeId,
     * @DocumentTypeSrNo -> Rows[0]["Id"] (0 when none).
     */
    public int voucherHeadId(int org, int comp, int documentTypeId, int srNo) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("OrganizationId", org);
        p.put("CompanyId", comp);
        p.put("DocumentTypeId", documentTypeId);
        p.put("DocumentTypeSrNo", srNo);
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ================================================================== 670 Payroll Posting

    /** Payroll.GenerateCode: hrm.Sp_Payroll_GetAllMethod @OrganizationId, @CompanyId, @DocMovementId, @FinancialYearId, @BranchesId, @Activity 'GenerateCode' -> Rows[0][0]. */
    public int payrollCode(UserAccount u, int financialYearId, int docMovementId) {
        List<Map<String, Object>> r = db.rows("hrm.Sp_Payroll_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocMovementId", docMovementId, "FinancialYearId", financialYearId, "BranchesId", u.getBranchesId(), "Activity", "GenerateCode");
        if (r.isEmpty()) return 0;
        Object first = r.get(0).values().stream().findFirst().orElse(null);
        return toInt(first);
    }

    /**
     * Payroll.ReadAll (HistoryFill): @OrganizationId, @CompanyId, @DocMovementId (DocumentTypeId 1000),
     * @FinancialYearId, @BranchesId, @IsApproved only when ApprovedFilter != "All" (never, from this
     * form's menu route), @Activity 'ReadAll'.
     */
    public List<Map<String, Object>> payrolls(UserAccount u, int financialYearId, int docMovementId) {
        return db.rows("hrm.Sp_Payroll_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocMovementId", docMovementId, "FinancialYearId", financialYearId, "BranchesId", u.getBranchesId(), "Activity", "ReadAll");
    }

    /** Payroll.GetByID -> DAL GetData: @Activity 'ReadById', @PayrollId. */
    public List<Map<String, Object>> payroll(int id) {
        return db.rows("hrm.Sp_Payroll_GetAllMethod", "Activity", "ReadById", "PayrollId", id);
    }

    /** DAL Payroll.GetData detail list: @PayrollId, @Activity 'ReadDetailByHeaderId'. */
    public List<Map<String, Object>> payrollDetails(int id) {
        return db.rows("hrm.Sp_Payroll_GetAllMethod", "PayrollId", id, "Activity", "ReadDetailByHeaderId");
    }

    /**
     * PayRollReports.GetEmployeePayrollSalary (btnShowDetail_Click): Sp_GetEmployeePayrollSalary with
     * @EmployeeId / @EmployeeCategoryId / @DepartmentId / @DebitAccountId only when != 0 (SectionId is never
     * set by the form), then @Month (the month's text), @Year, @MonthValue, @OrganizationId, @CompanyId.
     * GetDataSetProc - the grid reads Tables[0]; Tables[1] only feeds the company logo.
     */
    public List<Map<String, Object>> employeePayrollSalary(UserAccount u, int employeeId, int categoryId, int departmentId,
                                                            int debitAccountId, String month, int year, int monthValue) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        if (categoryId != 0) p.put("EmployeeCategoryId", categoryId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (debitAccountId != 0) p.put("DebitAccountId", debitAccountId);
        p.put("Month", month);
        p.put("Year", year);
        p.put("MonthValue", monthValue);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        return db.rows("Sp_GetEmployeePayrollSalary", p);
    }

    /**
     * PayRollReports.GetEmployeePostedSalary (tsDropDownPrint_DropDownItemClicked): Sp_GetEmployeePostedSalary
     * @PayrollId only when RecId != 0, @Month (text), @Year, @MonthValue, @OrganizationId, @CompanyId.
     */
    public List<Map<String, Object>> employeePostedSalary(UserAccount u, int payrollId, String month, int year, int monthValue) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (payrollId != 0) p.put("PayrollId", payrollId);
        p.put("Month", month);
        p.put("Year", year);
        p.put("MonthValue", monthValue);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        return db.rows("Sp_GetEmployeePostedSalary", p);
    }

    /**
     * CommonServies.GetGlAccountsByEmployeeId (BLL 0285 :507): Sp_genEmployeeHistory_GetAllMethod
     * @OrganizationId, @CompanyId, @Activity 'GetGlAccountsByEmployeeId' (EmployeeId, PayableAcId, ExpenseAccountId, LoanAcId).
     */
    public List<Map<String, Object>> glAccountsByEmployee(int org, int comp) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", org, "CompanyId", comp, "Activity", "GetGlAccountsByEmployeeId");
    }

    /** GenericProvider.SetProc of one model (inside the caller's transaction). */
    public int set(String proc, DesktopModel m) { return db.set(proc, m); }

    /**
     * DAL Payroll.SetData, run inside {@link #tx}: header Insert/Update, every detail through
     * hrm.Sp_PayrollDetail_Insert, then - when BLL Save built a voucher - Sp_VoucherHead_Insert/Update,
     * Sp_VoucherDetail_Insert per line, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert and
     * Sp_VoucherDetail_H_Insert with DocumentTypeIdRef. Returns the payroll id (num3).
     */
    public long savePayroll(Payroll obj, String procName, boolean withVoucher) {
        long num3 = db.set(procName, obj);
        if (num3 > 0) obj.PayrollId = (int) num3;
        else num3 = obj.PayrollId;
        for (PayrollDetail d : obj.PayrollDetailList) {
            d.PayrollId = obj.PayrollId;
            db.set("hrm.Sp_PayrollDetail_Insert", d);
        }
        if (withVoucher) {
            VoucherHead vh = obj.VoucherHeadInvoices;
            int num4 = (int) obj.PayrollId, num5 = (int) obj.OrganizationId, num6 = (int) obj.CompanyId;
            int num = voucherHeadId(num5, num6, obj.DocMovementId, num4);
            if (num != 0) vh.Id = num;
            vh.DocumentTypeSrNo = num4;
            vh.RefDocNoId = num4;
            int num2 = db.set(num == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
            if (num2 > 0) vh.Id = num2;
            if (vh.voucherDetailList == null || vh.voucherDetailList.isEmpty()) throw new IllegalStateException("VoucherDetail List Not Found");
            for (VoucherDetail vd : vh.voucherDetailList) {
                vd.VoucherHeadId = vh.Id;
                db.set("Sp_VoucherDetail_Insert", vd);
            }
            Map<String, Object> bc = new LinkedHashMap<>();
            bc.put("OrganizationId", num5);
            bc.put("CompanyId", num6);
            bc.put("Id", vh.Id);
            db.scalar("USP_VoucherBalanceCheck", bc);
            int documentTypeIdRef = db.set("Sp_VoucherHead_H_Insert", vh);
            for (VoucherDetail vd : vh.voucherDetailList) {
                vd.VoucherHeadId = vh.Id;
                vd.DocumentTypeIdRef = documentTypeIdRef;
                db.set("Sp_VoucherDetail_H_Insert", vd);
            }
        }
        return num3;
    }

    // ================================================================== 671 Employee Allowance

    /** EmployeeAllowance.ReadByEmployeeId: hrm.Sp_EmployeeAllowance_GetAllMethod @Activity 'ReadByEmployeeId', @OrganizationId, @CompanyId, @EmployeeId. */
    public List<Map<String, Object>> allowancesByEmployee(UserAccount u, int employeeId) {
        return db.rows("hrm.Sp_EmployeeAllowance_GetAllMethod", "Activity", "ReadByEmployeeId", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "EmployeeId", employeeId);
    }

    /** DAL EmployeeAllowance.SetData: every list row through hrm.Sp_EmployeeAllowance_Insert in one transaction (the proc branches on @ActionTypeId). */
    public void saveAllowances(List<EmployeeAllowance> list) {
        db.tx(() -> {
            for (EmployeeAllowance a : list) db.set("hrm.Sp_EmployeeAllowance_Insert", a);
            return null;
        });
    }

    // ================================================================== 672 Employee Late Adjustment

    /** EmployeeLateAdjustment.FormHistory: hrm.Sp_EmployeeLateAdjustment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> lateAdjustments(UserAccount u) {
        return db.rows("hrm.Sp_EmployeeLateAdjustment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** EmployeeLateAdjustment.GetByID: @EmployeeLateAdjustmentId, @Activity 'ReadById'. */
    public List<Map<String, Object>> lateAdjustment(int id) {
        return db.rows("hrm.Sp_EmployeeLateAdjustment_GetAllMethod", "EmployeeLateAdjustmentId", id, "Activity", "ReadById");
    }

    /** BLL EmployeeLateAdjustment.Save -> DAL SetData: _Insert (ActionTypeId 1) when the id is 0, else _Update (2); returns the id. */
    public long saveLateAdjustment(EmployeeLateAdjustment m) {
        String proc;
        if (m.EmployeeLateAdjustmentId == 0) { m.ActionTypeId = 1; proc = "hrm.Sp_EmployeeLateAdjustment_Insert"; }
        else { m.ActionTypeId = 2; proc = "hrm.Sp_EmployeeLateAdjustment_Update"; }
        long n = db.set(proc, m);
        return n > 0 ? n : m.EmployeeLateAdjustmentId;
    }

    // ================================================================== 673 Employee Loan Deduction

    /** EmployeeLoanDeduction.GetLoanInstallmentsForLoanDeductions: [hrm].[Sp_EmployeeLoanDeduction_GetAllMethod] @OrganizationId, @CompanyId, @Activity 'GetLoanInstallmentsForLoanDeductions'. */
    public List<Map<String, Object>> loanInstallments(UserAccount u) {
        return db.rows("[hrm].[Sp_EmployeeLoanDeduction_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetLoanInstallmentsForLoanDeductions");
    }

    /** EmployeeLoanDeduction.FormHistory: hrm.Sp_EmployeeLoanDeduction_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> loanDeductions(UserAccount u) {
        return db.rows("hrm.Sp_EmployeeLoanDeduction_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** DAL EmployeeLoanDeduction.SetData: ActionTypeId 1 / 2 by id, every row through hrm.Sp_EmployeeLoanDeduction_Insert, one transaction. */
    public long saveLoanDeductions(List<EmployeeLoanDeduction> list) {
        return db.tx(() -> {
            long result = 0;
            for (EmployeeLoanDeduction d : list) {
                d.ActionTypeId = d.EmployeeLoanDeductionId == 0 ? 1 : 2;
                result = db.set("hrm.Sp_EmployeeLoanDeduction_Insert", d);
            }
            return result;
        });
    }

    // ================================================================== 674 Employee Short Adjustment

    /** EmployeeShortAdjustment.FormHistory: hrm.Sp_EmployeeShortAdjustment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> shortAdjustments(UserAccount u) {
        return db.rows("hrm.Sp_EmployeeShortAdjustment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** EmployeeShortAdjustment.GetByID: @EmployeeShortAdjustmentId, @Activity 'ReadById'. */
    public List<Map<String, Object>> shortAdjustment(int id) {
        return db.rows("hrm.Sp_EmployeeShortAdjustment_GetAllMethod", "EmployeeShortAdjustmentId", id, "Activity", "ReadById");
    }

    /** BLL EmployeeShortAdjustment.Save -> DAL SetData: _Insert (ActionTypeId 1) when the id is 0, else _Update (2); returns the id. */
    public long saveShortAdjustment(EmployeeShortAdjustment m) {
        String proc;
        if (m.EmployeeShortAdjustmentId == 0) { m.ActionTypeId = 1; proc = "hrm.Sp_EmployeeShortAdjustment_Insert"; }
        else { m.ActionTypeId = 2; proc = "hrm.Sp_EmployeeShortAdjustment_Update"; }
        long n = db.set(proc, m);
        return n > 0 ? n : m.EmployeeShortAdjustmentId;
    }
}
