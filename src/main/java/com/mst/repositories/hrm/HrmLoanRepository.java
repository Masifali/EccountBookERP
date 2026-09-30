package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.loan.EmployeeAdvance;
import com.mst.models.hrm.loan.EmployeeLoan;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * DAL of the HRM "Loan Management" screens (AppModules 2024), Architecture.DAL.HRM.LoanManagement.*.
 * Every call is the desktop BLL/DAL's own procedure with the parameters it sends.
 * Schema-qualified procedures are written [hrm].[Name]: DesktopProc prefixes "dbo." to any name not starting with "[".
 *
 *   663 Employee Loan     frmEmployeeLoan.cs     BLL 0192 / DAL 0158   hrm.Sp_EmployeeLoan_GetAllMethod ReadById|FormHistory,
 *                                                                     hrm.Sp_EmployeeLoan_Insert|Update (23), Sp_Vouchers_GetMethods (print)
 *   664 Employee Advance  frmEmployeeAdvance.cs  BLL 0191 / DAL 0157   hrm.Sp_EmployeeAdvance_GetAllMethod ReadById|GenerateCode|FormHistory,
 *                                                                     hrm.Sp_EmployeeAdvance_Insert|Update (26), dbo.USP_GetPreviousAdvanceAmount,
 *                                                                     dbo.USP_EmployeeAdvanceSlip (print)
 *   both                  genEmployeeHistory.GetAllActiveEmployee   Sp_genEmployeeHistory_GetAllMethod 'ReadAllActiveEmployee'
 */
@Repository
public class HrmLoanRepository {

    private final HrmProcRepository db;

    public HrmLoanRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ shared

    /** genEmployeeHistory.GetAllActiveEmployee(obj): @OrganizationId, @CompanyId, (@ShiftId only when != 0 - never set here), @Activity 'ReadAllActiveEmployee'. */
    public List<Map<String, Object>> activeEmployees(UserAccount u) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadAllActiveEmployee");
    }

    /**
     * CommonServices.VoucherHeadIdGet(Id, DocumentTypeId) -> VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId:
     * Sp_Vouchers_GetMethods @Activity 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', @OrganizationId, @CompanyId,
     * @DocumentTypeId, @DocumentTypeSrNo; rows[0]["Id"] or 0.
     */
    public int voucherHeadId(UserAccount u, int documentTypeId, int id) {
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ------------------------------------------------------------------ 663 Employee Loan

    /**
     * EmployeeLoan.FormHistory(ReportsParameters): @OrganizationId, @CompanyId, @CanViewAllRecord (true: @EntryUser is
     * then not sent), @NoOfRecords only when != 0 (never), @IsApproved only when ApprovedFilter != "All", @Activity 'FormHistory'.
     * frmEmployeeLoan.GridBind passes ApprovedFilter "All" -> isApproved null (not sent).
     */
    public List<Map<String, Object>> loanHistory(UserAccount u, Boolean isApproved) {
        Map<String, Object> p = com.mst.services.hrm.HrmSupport.map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CanViewAllRecord", Boolean.TRUE);
        if (isApproved != null) p.put("IsApproved", isApproved);
        p.put("Activity", "FormHistory");
        return db.rows("[hrm].[Sp_EmployeeLoan_GetAllMethod]", p);
    }

    /** EmployeeLoan.GetByID(Id): @EmployeeLoanId, @Activity 'ReadById' (select * from hrm.EmployeeLoan). */
    public List<Map<String, Object>> loan(int id) {
        return db.rows("[hrm].[Sp_EmployeeLoan_GetAllMethod]", "EmployeeLoanId", id, "Activity", "ReadById");
    }

    /**
     * BLL EmployeeLoan.Save -> DAL SetData(obj, proc) for the Employee Loan form: EmployeeLoanId == 0 -> ActionTypeId 1,
     * hrm.Sp_EmployeeLoan_Insert; else ActionTypeId 2, hrm.Sp_EmployeeLoan_Update (IsApproved is always false from this
     * form, so no voucher and the installment list is empty). One transaction; returns the new id, or the id on an update.
     */
    public int saveLoan(EmployeeLoan m) {
        return db.tx(() -> {
            String proc;
            if (m.EmployeeLoanId == 0) { m.ActionTypeId = 1; proc = "[hrm].[Sp_EmployeeLoan_Insert]"; }
            else { m.ActionTypeId = 2; proc = "[hrm].[Sp_EmployeeLoan_Update]"; }
            int n = db.set(proc, m);
            if (n > 0) m.EmployeeLoanId = n; else n = m.EmployeeLoanId;
            return n;
        });
    }

    // ------------------------------------------------------------------ 664 Employee Advance

    /** EmployeeAdvance.GenerateCode: @OrganizationId, @CompanyId, @Activity 'GenerateCode'; rows[0]["DocNo"] or 0. */
    public int advanceCode(UserAccount u) {
        List<Map<String, Object>> r = db.rows("[hrm].[Sp_EmployeeAdvance_GetAllMethod]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /**
     * EmployeeAdvance.FormHistory(ReportsParameters) from frmEmployeeAdvance.GridBind: @OrganizationId, @CompanyId,
     * (@EmployeeId / @DepartmentId / @DesignationId only when != 0 - never here), @IsApproved = false (Status is null, and
     * null != string.Empty), @Activity 'FormHistory'.
     */
    public List<Map<String, Object>> advanceHistory(UserAccount u) {
        return db.rows("[hrm].[Sp_EmployeeAdvance_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "IsApproved", Boolean.FALSE, "Activity", "FormHistory");
    }

    /** EmployeeAdvance.GetByID(Id): @EmployeeAdvanceId, @Activity 'ReadById'. */
    public List<Map<String, Object>> advance(int id) {
        return db.rows("[hrm].[Sp_EmployeeAdvance_GetAllMethod]", "EmployeeAdvanceId", id, "Activity", "ReadById");
    }

    /** EmployeeAdvance.GetPreviousAdvanceAmount: dbo.USP_GetPreviousAdvanceAmount @OrganizationId, @CompanyId, @EmployeeId, @Month, @Year. */
    public List<Map<String, Object>> previousAdvance(UserAccount u, int employeeId, int month, int year) {
        return db.rows("USP_GetPreviousAdvanceAmount", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EmployeeId", employeeId, "Month", month, "Year", year);
    }

    /** EmployeeAdvance.EmployeeAdvanceSlip(Id): dbo.USP_EmployeeAdvanceSlip @Id (the 1008 print's data). */
    public List<Map<String, Object>> advanceSlip(int id) {
        return db.rows("USP_EmployeeAdvanceSlip", "Id", id);
    }

    /**
     * BLL EmployeeAdvance.Save -> DAL SetData: EmployeeAdvanceId == 0 -> ActionTypeId 1, hrm.Sp_EmployeeAdvance_Insert;
     * else ActionTypeId 2, hrm.Sp_EmployeeAdvance_Update. One transaction; the new id or the id on an update.
     */
    public int saveAdvance(EmployeeAdvance m) {
        return db.tx(() -> {
            String proc;
            if (m.EmployeeAdvanceId == 0) { m.ActionTypeId = 1; proc = "[hrm].[Sp_EmployeeAdvance_Insert]"; }
            else { m.ActionTypeId = 2; proc = "[hrm].[Sp_EmployeeAdvance_Update]"; }
            int n = db.set(proc, m);
            if (n > 0) m.EmployeeAdvanceId = n; else n = m.EmployeeAdvanceId;
            return n;
        });
    }
}
