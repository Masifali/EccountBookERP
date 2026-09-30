package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.policy.HrmEOBIPolicy;
import com.mst.models.hrm.policy.HrmEOBIPolicyEmployee;
import com.mst.models.hrm.policy.HrmLeaveQuota;
import com.mst.models.hrm.policy.HrmLeaveQuotaDetail;
import com.mst.models.hrm.policy.HrmPolicyDetail;
import com.mst.models.hrm.policy.HrmPolicyHeader;
import com.mst.models.hrm.policy.HrmSalaryBreakupPolicy;
import com.mst.models.hrm.policy.HrmSocialPolicy;
import com.mst.models.hrm.policy.HrmSocialPolicySlab;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Policy Management" screens (AppModules 2019), Architecture.DAL.HRM.PolicyManagment.*.
 * Every call is the desktop BLL/DAL's own procedure with the parameters the BLL sends
 * (recovered_source/projects/architecture.bll|dal, procedures of 23-Sep-2026).
 *
 *   643 Social Security        BLL 0182 / DAL 0149   Sp_hrmSocialPolicy_* + Sp_hrmSocialPolicySlab_*
 *   644 General Policy         BLL 0180,0179 / DAL 0147   Sp_hrmPolicyHeader_* + Sp_hrmPolicyDetail_*
 *   645 Salary Breakup Policy  BLL 0181 / DAL 0148   Sp_hrmSalaryBreakupPolicy_*
 *   646 Leave Quota Policy     BLL 0177 / DAL 0144   sp_hrmLeaveQuota_Insert, Sp_hrmLeaveQuota_*, Sp_hrmLeaveQuotaDetail_*
 *   647 E.O.B.I Policy         BLL 0175 / DAL 0142   Sp_hrmEOBIPolicy_*, Sp_hrmEOBIPolicyEmployee_Insert
 *   shared lookups             genProfile.GetByProfileTypeId (BLL 0173), genLocation.Getall (BLL 0170),
 *                              genEmployeeHistory.GetAllEmployeesActiveFroEIOBPolicy (BLL 0200)
 */
@Repository
public class HrmPolicyRepository {

    private final HrmProcRepository db;

    public HrmPolicyRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ lookups

    /**
     * genProfile.GetByProfileTypeId: @OrganizationId, @CompanyId, @ProfileTypeId (only when != 0),
     * @Activity 'ReadByProfileTypeId' on Sp_genProfile_GetAllMethod. The BLL never sends @IsActive
     * (the forms set IsActive = true on the model, but the BLL does not add it).
     */
    public List<Map<String, Object>> profilesByType(UserAccount u, int profileTypeId) {
        if (profileTypeId == 0)
            return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByProfileTypeId");
        return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ProfileTypeId", profileTypeId, "Activity", "ReadByProfileTypeId");
    }

    /** genLocation.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll' on Sp_genLocation_GetAllMethod. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * genEmployeeHistory.GetAllEmployeesActiveFroEIOBPolicy: @OrganizationId, @CompanyId (the filter ids are 0 on
     * the EOBI form, so they are not sent), @Activity 'ReadAllActiveEmployeeForEIOBPolicy' on Sp_genEmployeeHistory_GetAllMethod.
     */
    public List<Map<String, Object>> eobiEmployees(UserAccount u) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadAllActiveEmployeeForEIOBPolicy");
    }

    // ------------------------------------------------------------------ 643 Social Security

    /** hrmSocialPolicy.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> socialPolicies(UserAccount u) {
        return db.rows("Sp_hrmSocialPolicy_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** hrmSocialPolicy.GetByID -> DAL GetAll: @SocialPolicyId, @Activity 'ReadById'. */
    public List<Map<String, Object>> socialPolicy(int id) {
        return db.rows("Sp_hrmSocialPolicy_GetAllMethod", "SocialPolicyId", id, "Activity", "ReadById");
    }

    /** DAL GetAll, per header: @SocialPolicySlabId = SocialPolicyId, @Activity 'ReadById' on Sp_hrmSocialPolicySlab_GetAllMethod. */
    public List<Map<String, Object>> socialSlabs(int socialPolicyId) {
        return db.rows("Sp_hrmSocialPolicySlab_GetAllMethod", "SocialPolicySlabId", socialPolicyId, "Activity", "ReadById");
    }

    /**
     * hrmSocialPolicy.Save -> DAL SetData in one SqlTransaction: Sp_hrmSocialPolicy_Insert (SocialPolicyId == 0) or
     * Sp_hrmSocialPolicy_Update (which itself deletes the old hrmSocialPolicySlab rows), then
     * Sp_hrmSocialPolicySlab_Insert for every slab with SocialPolicyId = the header id.
     */
    public int saveSocialPolicy(HrmSocialPolicy m) {
        return db.tx(() -> {
            int n = db.set(m.SocialPolicyId == 0 ? "Sp_hrmSocialPolicy_Insert" : "Sp_hrmSocialPolicy_Update", m);
            if (n > 0) m.SocialPolicyId = n; else n = m.SocialPolicyId;
            for (HrmSocialPolicySlab s : m.SocialSecurityDetail) {
                s.SocialPolicyId = m.SocialPolicyId;
                db.set("Sp_hrmSocialPolicySlab_Insert", s);
            }
            return n;
        });
    }

    // ------------------------------------------------------------------ 644 General Policy

    /** hrmPolicyHeader.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll' on Sp_hrmPolicyHeader_GetAllMethod. */
    public List<Map<String, Object>> policyHeaders(UserAccount u) {
        return db.rows("Sp_hrmPolicyHeader_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** hrmPolicyDetail.GetByID: @PolicyId (the header id - the proc filters HeaderId = @PolicyId), @Activity 'ReadById'. */
    public List<Map<String, Object>> policyDetail(int headerId) {
        return db.rows("Sp_hrmPolicyDetail_GetAllMethod", "PolicyId", headerId, "Activity", "ReadById");
    }

    /**
     * hrmPolicyHeader.Save -> DAL SetData in one SqlTransaction: Sp_hrmPolicyHeader_Insert (PolicyHeaderId == 0) or
     * Sp_hrmPolicyHeader_Update, then Sp_hrmPolicyDetail_Insert for the one detail with HeaderId = the header id
     * (the update procedure deletes nothing, so every update adds one more detail row - as on the desktop).
     */
    public int savePolicy(HrmPolicyHeader h) {
        return db.tx(() -> {
            int n = db.set(h.PolicyHeaderId == 0 ? "Sp_hrmPolicyHeader_Insert" : "Sp_hrmPolicyHeader_Update", h);
            if (n > 0) h.PolicyHeaderId = n; else n = h.PolicyHeaderId;
            for (HrmPolicyDetail d : h.HrmPolicyDetailList) {
                d.HeaderId = h.PolicyHeaderId;
                db.set("Sp_hrmPolicyDetail_Insert", d);
            }
            return n;
        });
    }

    // ------------------------------------------------------------------ 645 Salary Breakup Policy

    /** hrmSalaryBreakupPolicy.GetByLocationId: @LocationId, @Activity 'ReadByLocationId'. */
    public List<Map<String, Object>> breakupByLocation(int locationId) {
        return db.rows("Sp_hrmSalaryBreakupPolicy_GetAllMethod", "LocationId", locationId, "Activity", "ReadByLocationId");
    }

    /** hrmSalaryBreakupPolicy.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll' (the web History list). */
    public List<Map<String, Object>> breakups(UserAccount u) {
        return db.rows("Sp_hrmSalaryBreakupPolicy_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * hrmSalaryBreakupPolicy.Save -> DAL SetData in one SqlTransaction: the SAME procedure for every row of the list
     * (Sp_hrmSalaryBreakupPolicy_Insert when the form's RecId == 0, else Sp_hrmSalaryBreakupPolicy_Update), the last
     * row's return value kept.
     */
    public int saveBreakup(int recId, List<HrmSalaryBreakupPolicy> rows) {
        String proc = recId == 0 ? "Sp_hrmSalaryBreakupPolicy_Insert" : "Sp_hrmSalaryBreakupPolicy_Update";
        return db.tx(() -> {
            int n = 0;
            for (HrmSalaryBreakupPolicy r : rows) n = db.set(proc, r);
            return n > 0 ? n : recId;
        });
    }

    // ------------------------------------------------------------------ 646 Leave Quota Policy

    /** hrmLeaveQuota.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> leaveQuotas(UserAccount u) {
        return db.rows("Sp_hrmLeaveQuota_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** hrmLeaveQuota.GetByID -> DAL GetAll: @leaveQuotaId, @Activity 'ReadById'. */
    public List<Map<String, Object>> leaveQuota(int id) {
        return db.rows("Sp_hrmLeaveQuota_GetAllMethod", "leaveQuotaId", id, "Activity", "ReadById");
    }

    /** DAL GetAll, per header: @LeaveQuotaDetailId = LeaveQuotaId, @Activity 'ReadById' on Sp_hrmLeaveQuotaDetail_GetAllMethod. */
    public List<Map<String, Object>> leaveQuotaDetails(int leaveQuotaId) {
        return db.rows("Sp_hrmLeaveQuotaDetail_GetAllMethod", "LeaveQuotaDetailId", leaveQuotaId, "Activity", "ReadById");
    }

    /**
     * hrmLeaveQuota.Save (ActionTypeId 1 / 2) -> DAL SetData in one SqlTransaction: sp_hrmLeaveQuota_Insert or
     * Sp_hrmLeaveQuota_Update (which deletes the old details), then Sp_hrmLeaveQuotaDetail_Insert for every detail.
     */
    public int saveLeaveQuota(HrmLeaveQuota m) {
        return db.tx(() -> {
            int n = db.set(m.LeaveQuotaId == 0 ? "sp_hrmLeaveQuota_Insert" : "Sp_hrmLeaveQuota_Update", m);
            if (n > 0) m.LeaveQuotaId = n; else n = m.LeaveQuotaId;
            for (HrmLeaveQuotaDetail d : m.HrmLeaveQuotaDetailList) {
                d.LeaveQuotaId = m.LeaveQuotaId;
                db.set("Sp_hrmLeaveQuotaDetail_Insert", d);
            }
            return n;
        });
    }

    // ------------------------------------------------------------------ 647 E.O.B.I Policy

    /** hrmEOBIPolicy.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> eobiPolicies(UserAccount u) {
        return db.rows("Sp_hrmEOBIPolicy_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** hrmEOBIPolicy.GetByID -> DAL GetData: @EOBIPolicyId, @Activity 'ReadById'. */
    public List<Map<String, Object>> eobiPolicy(int id) {
        return db.rows("Sp_hrmEOBIPolicy_GetAllMethod", "EOBIPolicyId", id, "Activity", "ReadById");
    }

    /** DAL GetData, per header: @EOBIPolicyId, @Activity 'ReadhrmEOBIPolicyEmployeeByHeaderId'. */
    public List<Map<String, Object>> eobiPolicyEmployees(int id) {
        return db.rows("Sp_hrmEOBIPolicy_GetAllMethod", "EOBIPolicyId", id, "Activity", "ReadhrmEOBIPolicyEmployeeByHeaderId");
    }

    /**
     * hrmEOBIPolicy.Save -> DAL SetData in one SqlTransaction: Sp_hrmEOBIPolicy_Insert or Sp_hrmEOBIPolicy_Update
     * (which deletes the old employees), then Sp_hrmEOBIPolicyEmployee_Insert for every employee with hrmEOBIPolicyId = the header id.
     */
    public int saveEobiPolicy(HrmEOBIPolicy m) {
        return db.tx(() -> {
            int n = db.set(m.EOBIPolicyId == 0 ? "Sp_hrmEOBIPolicy_Insert" : "Sp_hrmEOBIPolicy_Update", m);
            if (n > 0) m.EOBIPolicyId = n; else n = m.EOBIPolicyId;
            for (HrmEOBIPolicyEmployee e : m.EOBIPolicyEmployeesList) {
                e.hrmEOBIPolicyId = m.EOBIPolicyId;
                db.set("Sp_hrmEOBIPolicyEmployee_Insert", e);
            }
            return n;
        });
    }
}
