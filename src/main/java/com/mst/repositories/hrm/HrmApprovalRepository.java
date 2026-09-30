package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.approval.EmployeeLoanInstallment;
import com.mst.models.hrm.approval.VoucherDetail;
import com.mst.models.hrm.approval.VoucherHead;
import com.mst.models.hrm.loan.EmployeeLoan;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.map;
import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * DAL of the HRM "Approval Management" screens (AppModules 2026). Every call is the desktop BLL/DAL's own
 * procedure with the parameters it sends. Schema-qualified procedures are written [hrm].[Name] (DesktopProc
 * prefixes "dbo." to any other name).
 *
 *   667 Loan Approval     LoanApproval.cs     BLL EmployeeLoan (0192) / DAL 0158: hrm.Sp_EmployeeLoan_GetAllMethod 'FormHistory',
 *                                             hrm.Sp_EmployeeLoan_Update, hrm.Sp_EmployeeLoanInstallment_Insert, MakeVoucher:
 *                                             Sp_genEmployeeHistory_GetAllMethod 'GetGlAccountsByEmployeeId', Sp_Vouchers_GetMethods,
 *                                             Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck,
 *                                             Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert
 *   668 Leave Approval    LeaveApproval.cs + PendingLeaveDetailForApproval.cs   BLL genEmployeeLeave (0194): Sp_genEmployeeLeave_ReadAll
 *                                             'GetDataForLeaveApproval' | 'GetEmployeeLeaveDetailIdByHeaderId' |
 *                                             'GetLeaveDetailByEmployeeLeaveId' | 'LeaveApproved'
 *   669 Advance Approval  AdvanceApproval.cs  BLL EmployeeAdvance (0191): hrm.Sp_EmployeeAdvance_GetAllMethod 'FormHistoryForApproval' |
 *                                             'UpdateForEmployeeAdvanceApproval'
 *   combos                genEmployeeHistory.GetAllEmployeesActive, genDepartment.Getall, genDesignation.Getall
 */
@Repository
public class HrmApprovalRepository {

    private final HrmProcRepository db;

    public HrmApprovalRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ combos

    /**
     * genEmployeeHistory.GetAllEmployeesActive(ReportsParameters): @OrganizationId, @CompanyId, @DepartmentId only when != 0,
     * @DesignationId only when != 0 (the other guarded filters are never set by these forms), @Activity 'ReadAllActiveEmployee'.
     */
    public List<Map<String, Object>> activeEmployees(UserAccount u, int departmentId, int designationId) {
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (designationId != 0) p.put("DesignationId", designationId);
        p.put("Activity", "ReadAllActiveEmployee");
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", p);
    }

    /** genDepartment.Getall: Sp_genDepartment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genDesignation.Getall: Sp_genDesignation_GetAllMethod @Activity 'ReadAll', @OrganizationId, @CompanyId. */
    public List<Map<String, Object>> designations(UserAccount u) {
        return db.rows("Sp_genDesignation_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ 667 Loan Approval

    /**
     * LoanApproval.PendingLoanForApproval -> EmployeeLoan.FormHistory(IsApproved = false, CanViewAllRecord = true,
     * ApprovedFilter null -> "!= All" so @IsApproved IS sent): @OrganizationId, @CompanyId, @CanViewAllRecord, @IsApproved, @Activity.
     */
    public List<Map<String, Object>> pendingLoans(UserAccount u) {
        return db.rows("[hrm].[Sp_EmployeeLoan_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CanViewAllRecord", Boolean.TRUE, "IsApproved", Boolean.FALSE, "Activity", "FormHistory");
    }

    /**
     * CommonServies.GetGlAccountsByEmployeeId(Org, Company): Sp_genEmployeeHistory_GetAllMethod @OrganizationId, @CompanyId,
     * @Activity 'GetGlAccountsByEmployeeId' (EmployeeId, PayableAcId, ExpenseAccountId, LoanAcId).
     */
    public List<Map<String, Object>> glAccountsByEmployee(int organizationId, int companyId) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", organizationId, "CompanyId", companyId,
                "Activity", "GetGlAccountsByEmployeeId");
    }

    /**
     * DAL CommonServices.GetVoucherHeadId(Org, Company, DocumentTypeId, Id): Sp_Vouchers_GetMethods @Activity
     * 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', @OrganizationId, @CompanyId, @DocumentTypeId, @DocumentTypeSrNo.
     */
    public int voucherHeadId(int organizationId, int companyId, int documentTypeId, int id) {
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", organizationId, "CompanyId", companyId, "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    /**
     * BLL EmployeeLoan.Save (EmployeeLoanId != 0 -> ActionTypeId 2, hrm.Sp_EmployeeLoan_Update) -> DAL EmployeeLoan.SetData,
     * in ONE transaction exactly as the DAL's SqlTransaction:
     *   SetProc(header); every EmployeeLoanInstallment (EmployeeLoanId = header id) -> hrm.Sp_EmployeeLoanInstallment_Insert;
     *   when IsApproved and ActionTypeId 2: GetVoucherHeadId -> Sp_VoucherHead_Insert (none yet) | Sp_VoucherHead_Update
     *   (Id = the existing one), DocumentTypeSrNo = RefDocNoId = VoucherCode = EmployeeLoanId; no detail -> "VoucherDetail List
     *   Not Found"; each VoucherDetail (VoucherHeadId) -> Sp_VoucherDetail_Insert; USP_VoucherBalanceCheck @OrganizationId,
     *   @CompanyId, @Id (ExecuteScalar); Sp_VoucherHead_H_Insert -> DocumentTypeIdRef of each Sp_VoucherDetail_H_Insert.
     * Returns what the DAL returns (num3).
     */
    public int saveLoanApproval(EmployeeLoan obj) {
        return db.tx(() -> {
            obj.ActionTypeId = 2;
            int num3 = db.set("[hrm].[Sp_EmployeeLoan_Update]", obj);
            if (num3 > 0) obj.EmployeeLoanId = num3; else num3 = obj.EmployeeLoanId;
            for (EmployeeLoanInstallment item : obj.EmployeeLoanInstallmentslist) {
                item.EmployeeLoanId = obj.EmployeeLoanId;
                num3 = db.set("[hrm].[Sp_EmployeeLoanInstallment_Insert]", item);
            }
            if (obj.IsApproved && obj.ActionTypeId == 2) {
                VoucherHead vh = obj.VoucherHeadInvoices;
                int num = voucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.EmployeeLoanId);
                if (num != 0) vh.Id = num;
                vh.DocumentTypeSrNo = obj.EmployeeLoanId;
                vh.RefDocNoId = obj.EmployeeLoanId;
                vh.VoucherCode = obj.EmployeeLoanId;
                int num2 = db.set(num == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
                if (num2 > 0) vh.Id = num2;
                if (vh.voucherDetailList == null || vh.voucherDetailList.isEmpty()) throw new IllegalStateException("VoucherDetail List Not Found");
                for (VoucherDetail d : vh.voucherDetailList) {
                    d.VoucherHeadId = vh.Id;
                    db.set("Sp_VoucherDetail_Insert", d);
                }
                db.scalar("USP_VoucherBalanceCheck", map("OrganizationId", obj.OrganizationId, "CompanyId", obj.CompanyId, "Id", vh.Id));
                int documentTypeIdRef = db.set("Sp_VoucherHead_H_Insert", vh);
                for (VoucherDetail d : vh.voucherDetailList) {
                    d.VoucherHeadId = vh.Id;
                    d.DocumentTypeIdRef = documentTypeIdRef;
                    db.set("Sp_VoucherDetail_H_Insert", d);
                }
            }
            return num3;
        });
    }

    // ------------------------------------------------------------------ 668 Leave Approval

    /**
     * genEmployeeLeave.GetDataForLeaveApproval: @OrganizationId, @CompanyId, @EmployeeId / @DepartmentId / @DesignationId only
     * when != 0, @Activity 'GetDataForLeaveApproval'.
     */
    public List<Map<String, Object>> leavesForApproval(UserAccount u, int employeeId, int departmentId, int designationId) {
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (designationId != 0) p.put("DesignationId", designationId);
        p.put("Activity", "GetDataForLeaveApproval");
        return db.rows("Sp_genEmployeeLeave_ReadAll", p);
    }

    /** genEmployeeLeave.GetEmployeeLeaveDetailIdByHeaderId(EmployeeLeaveId): @EmployeeLeaveId, @Activity. */
    public List<Map<String, Object>> leaveDetailIds(int employeeLeaveId) {
        return db.rows("Sp_genEmployeeLeave_ReadAll", "EmployeeLeaveId", employeeLeaveId, "Activity", "GetEmployeeLeaveDetailIdByHeaderId");
    }

    /**
     * genEmployeeLeave.GetLeaveDetailByEmployeeLeaveId (PendingLeaveDetailForApproval.HistoryForApproval): @OrganizationId,
     * @CompanyId, @EmployeeId, @LeaveTypeProfileId, @Activity 'GetLeaveDetailByEmployeeLeaveId'.
     */
    public List<Map<String, Object>> leaveDetails(UserAccount u, int employeeId, int leaveTypeProfileId) {
        return db.rows("Sp_genEmployeeLeave_ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EmployeeId", employeeId, "LeaveTypeProfileId", leaveTypeProfileId, "Activity", "GetLeaveDetailByEmployeeLeaveId");
    }

    /**
     * genEmployeeLeave.LeaveApproved / LeaveApprovedlist (one call per item, each on its own connection - no transaction):
     * @OrganizationId, @CompanyId, @EmployeeLeaveQuotaId, @LeaveTypeProfileId, @EmployeeId, @EntryUser, @EmployeeLeaveDetailId,
     * @ActionId (1 = reject, 2 = approve; the list sends the model default 0 = approve), @Activity 'LeaveApproved'.
     */
    public void leaveApproved(int organizationId, int companyId, int leaveQuotaId, int leaveTypeProfileId, int employeeId,
                              int entryUser, int employeeLeaveDetailId, int actionId) {
        db.rows("Sp_genEmployeeLeave_ReadAll", "OrganizationId", organizationId, "CompanyId", companyId, "EmployeeLeaveQuotaId", leaveQuotaId,
                "LeaveTypeProfileId", leaveTypeProfileId, "EmployeeId", employeeId, "EntryUser", entryUser,
                "EmployeeLeaveDetailId", employeeLeaveDetailId, "ActionId", actionId, "Activity", "LeaveApproved");
    }

    // ------------------------------------------------------------------ 669 Advance Approval

    /**
     * EmployeeAdvance.FormHistoryForApproval(Status "Approved", IsApproved false): @OrganizationId, @CompanyId, @EmployeeId /
     * @DepartmentId / @DesignationId (= PartyLocationId = cmbDesignation) only when != 0, @IsApproved false, @Activity.
     */
    public List<Map<String, Object>> advancesForApproval(UserAccount u, int employeeId, int departmentId, int designationId) {
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (designationId != 0) p.put("DesignationId", designationId);
        p.put("IsApproved", Boolean.FALSE);
        p.put("Activity", "FormHistoryForApproval");
        return db.rows("[hrm].[Sp_EmployeeAdvance_GetAllMethod]", p);
    }

    /**
     * EmployeeAdvance.UpdateEmployeeAdvanceForApproval: @EmployeeAdvanceId, @ApprovedUserId, @ApprovedAmount,
     * @Activity 'UpdateForEmployeeAdvanceApproval' (@ApprovalRemarks is not sent - the procedure then sets it to NULL).
     */
    public void approveAdvance(int employeeAdvanceId, int approvedUserId, java.math.BigDecimal approvedAmount) {
        db.rows("[hrm].[Sp_EmployeeAdvance_GetAllMethod]", "EmployeeAdvanceId", employeeAdvanceId, "ApprovedUserId", approvedUserId,
                "ApprovedAmount", approvedAmount, "Activity", "UpdateForEmployeeAdvanceApproval");
    }
}
