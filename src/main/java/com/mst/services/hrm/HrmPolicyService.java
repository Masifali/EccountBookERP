package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.PolicyEobiDto;
import com.mst.models.hrm.dto.PolicyEobiRowDto;
import com.mst.models.hrm.dto.PolicyGeneralDto;
import com.mst.models.hrm.dto.PolicyLeaveQuotaDto;
import com.mst.models.hrm.dto.PolicyLeaveQuotaRowDto;
import com.mst.models.hrm.dto.PolicySalaryBreakupDto;
import com.mst.models.hrm.dto.PolicySalaryBreakupRowDto;
import com.mst.models.hrm.dto.PolicySocialSecurityDto;
import com.mst.models.hrm.dto.PolicySocialSecurityRowDto;
import com.mst.models.hrm.policy.HrmEOBIPolicy;
import com.mst.models.hrm.policy.HrmEOBIPolicyEmployee;
import com.mst.models.hrm.policy.HrmLeaveQuota;
import com.mst.models.hrm.policy.HrmLeaveQuotaDetail;
import com.mst.models.hrm.policy.HrmPolicyDetail;
import com.mst.models.hrm.policy.HrmPolicyHeader;
import com.mst.models.hrm.policy.HrmSalaryBreakupPolicy;
import com.mst.models.hrm.policy.HrmSocialPolicy;
import com.mst.models.hrm.policy.HrmSocialPolicySlab;
import com.mst.repositories.hrm.HrmPolicyRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Policy Management" screens (AppModules 2019). Each method names the desktop form
 * method it reproduces; validation wording/order and the messages are the form's.
 *
 * The forms convert text with Conversion.ToInt (Convert.ToInt32: a string must be a whole number, else 0;
 * a double is rounded half-to-even) - {@link #cInt} reproduces that, because the value the DB receives
 * depends on it (e.g. "10.00" typed in an int box is saved as 0 on the desktop).
 */
@Service
public class HrmPolicyService {

    public static final int SCREEN_SOCIAL_SECURITY = 643;
    public static final int SCREEN_GENERAL_POLICY = 644;
    public static final int SCREEN_SALARY_BREAKUP = 645;
    public static final int SCREEN_LEAVE_QUOTA = 646;
    public static final int SCREEN_EOBI = 647;

    /** genProfileType ids the forms pass to genProfile.GetByProfileTypeId. */
    private static final int PT_SALARY_TYPE = 2, PT_CASUAL_LEAVE = 3, PT_SPECIAL_LEAVE = 4, PT_BENEFIT = 5;

    private static final DateTimeFormatter DD_MMM_YYYY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);
    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd");
    /** DateTime.ToString() / ToShortDateString() in the desktop's en-US culture. */
    private static final DateTimeFormatter NET_GENERAL = DateTimeFormatter.ofPattern("M/d/yyyy h:mm:ss a", Locale.ENGLISH);
    private static final DateTimeFormatter NET_SHORT = DateTimeFormatter.ofPattern("M/d/yyyy", Locale.ENGLISH);
    private static final LocalDateTime NET_NULL_DATE = LocalDate.of(1900, 1, 1).atStartOfDay();

    @Autowired private HrmPolicyRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== 643 Social Security (SocialSecurity.cs)

    /** SocialSecurity_Load: rights (btnsave / btnupdate Enabled) + ProfileType() (salary factors, ProfileTypeId 5). */
    public Map<String, Object> socialSetup() {
        UserAccount u = hrm.user(SCREEN_SOCIAL_SECURITY);
        return map("rights", hrm.rights(u, SCREEN_SOCIAL_SECURITY), "factors", profiles(u, PT_BENEFIT));
    }

    /** ProfileType() - btnRefresh_Click. */
    public List<Map<String, Object>> socialFactors() {
        return profiles(hrm.user(SCREEN_SOCIAL_SECURITY), PT_BENEFIT);
    }

    /** HistoryFill(): { SocialPolicyId, ProfileName, FromDate / ToDate "dd-MMM-yyyy", MinYearLimit, PolicyDescription }. */
    public List<Map<String, Object>> socialHistory() {
        UserAccount u = hrm.user(SCREEN_SOCIAL_SECURITY);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.socialPolicies(u))
            out.add(map("SocialPolicyId", r.get("SocialPolicyId"), "ProfileName", r.get("ProfileName"),
                    "FromDate", fmt(r.get("FromDate"), DD_MMM_YYYY), "ToDate", fmt(r.get("ToDate"), DD_MMM_YYYY),
                    "MinYearLimit", r.get("MinYearLimit"), "PolicyDescription", r.get("PolicyDescription")));
        return out;
    }

    /** GetById / HistoryDetailBind -> hrmSocialPolicy.GetByID (header ReadById + its slabs); only an id of this company's list. */
    public Map<String, Object> socialPolicy(int id) {
        UserAccount u = hrm.user(SCREEN_SOCIAL_SECURITY);
        if (!owns(repo.socialPolicies(u), "SocialPolicyId", id)) throw invalid("Record not found.");
        Map<String, Object> h = one(repo.socialPolicy(id));
        List<Map<String, Object>> slabs = new ArrayList<>();
        for (Map<String, Object> s : repo.socialSlabs(toInt(h.get("SocialPolicyId"))))
            slabs.add(map("FromSalary", toDouble(s.get("FromSalary")), "ToSalary", toDouble(s.get("ToSalary")),
                    "SecurityPercent", toDouble(s.get("SecurityPercent")), "SecurityAmount", toDouble(s.get("SecurityAmount"))));
        return map("SocialPolicyId", h.get("SocialPolicyId"), "SalaryFactorProfileId", h.get("SalaryFactorProfileId"),
                "ProfileName", h.get("ProfileName"),
                "FromDate", fmt(h.get("FromDate"), ISO), "ToDate", fmt(h.get("ToDate"), ISO),
                "MinYearLimit", toInt(h.get("MinYearLimit")), "PolicyDescription", str(h.get("PolicyDescription")),
                "details", slabs);
    }

    /**
     * Insert(): formvalidation, the date check, the "detail rows" check, the model exactly as the form fills it,
     * the Percent% total check (int.Parse of the grid total, as the form does), then hrmSocialPolicy.Save.
     */
    public Map<String, Object> saveSocial(PolicySocialSecurityDto b) {
        UserAccount u = hrm.user(SCREEN_SOCIAL_SECURITY);
        int id = Math.max(0, b.id);
        hrm.require(u, SCREEN_SOCIAL_SECURITY, id > 0 ? "Update" : "Save");
        // formvalidation()
        if (trim(b.salaryFactorText).isEmpty()) throw invalid("Please Select Salary Factor");
        if (trim(b.policyDescription).isEmpty()) throw invalid("Please Insert Policy Descripton");
        LocalDateTime from = toDay(b.fromDate), to = toDay(b.toDate);
        if (from == null || to == null) throw invalid("Please select a valid date.");
        if (from.isAfter(to)) throw invalid("DateFrom Is Greater Than DateTo please Check: Thank You");
        List<PolicySocialSecurityRowDto> rows = b.details == null ? new ArrayList<>() : b.details;
        if (rows.isEmpty()) throw invalid("Please Insert Record In Detail ThankYou");
        if (id > 0 && !owns(repo.socialPolicies(u), "SocialPolicyId", id)) throw invalid("Record not found.");
        int factor = b.salaryFactorProfileId;
        if (factor != 0 && !owns(profileRows(u, PT_BENEFIT), "ProfileId", factor)) throw invalid("Record not found.");

        HrmSocialPolicy m = new HrmSocialPolicy();
        LocalDateTime today = LocalDate.now().atStartOfDay();
        m.SocialPolicyId = id;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.CreatedOn = today;
        m.CreatedById = u.getId();
        m.AlteredById = u.getId();
        m.AlteredOn = today;
        m.FromDate = from;
        m.ToDate = to;
        m.PolicyDescription = trim(b.policyDescription);
        m.SalaryFactorProfileId = factor;
        m.MinYearLimit = cInt(b.minYearLimit);
        // int.Parse(grdfrm.GetTotal(Percent%, Sum).ToString()) > 100
        double total = 0;
        for (PolicySocialSecurityRowDto r : rows) if (r.percent != null) total += r.percent;
        if (total != Math.rint(total) || Math.abs(total) > Integer.MAX_VALUE) throw invalid("Input string was not in a correct format.");
        if ((int) total > 100) throw invalid("Your Total Percent Is Greater Than 100 Please Check It");
        m.SocialSecurityDetail = new ArrayList<>();
        for (PolicySocialSecurityRowDto r : rows) {
            HrmSocialPolicySlab s = new HrmSocialPolicySlab();
            s.FromSalary = cInt(r.fromSalary);            // string column -> Conversion.ToInt
            s.ToSalary = cInt(r.toSalary);
            s.SecurityPercent = cInt(r.percent);          // double column -> Convert.ToInt32(double)
            s.SecurityAmount = cInt(r.amount);
            m.SocialSecurityDetail.add(s);
        }
        int n = repo.saveSocialPolicy(m);
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 644 General Policy (GeneralPolicy.cs)

    /** GeneralPolicy_Load -> GetHeaderId(): cmbid bound to PolicyHeaderId / DateFrom. */
    public Map<String, Object> generalSetup() {
        UserAccount u = hrm.user(SCREEN_GENERAL_POLICY);
        return map("rights", hrm.rights(u, SCREEN_GENERAL_POLICY), "headers", headerRows(u));
    }

    /** GetHeaderId() - btnRefresh_Click, and the rebind every Leave / Save does. */
    public List<Map<String, Object>> generalHeaders() { return headerRows(hrm.user(SCREEN_GENERAL_POLICY)); }

    private List<Map<String, Object>> headerRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.policyHeaders(u))
            out.add(map("PolicyHeaderId", r.get("PolicyHeaderId"),
                    // BindDDLNew copies DateFrom into a string column: DateTime.ToString()
                    "DateFrom", fmt(r.get("DateFrom"), NET_GENERAL),
                    "DateFromText", fmt(r.get("DateFrom"), DD_MMM_YYYY), "DateToText", fmt(r.get("DateTo"), DD_MMM_YYYY)));
        return out;
    }

    /**
     * cmbbeniftprofile_Leave: GetHeaderId() then hrmPolicyDetail.GetByID(RecId) (first row) - each value as the
     * DataRow's ToString() puts it into the text box. {headers, row} with row null = "Record No Found".
     */
    public Map<String, Object> generalDetail(int id) {
        UserAccount u = hrm.user(SCREEN_GENERAL_POLICY);
        List<Map<String, Object>> headers = headerRows(u);
        Map<String, Object> row = null;
        if (id == 0 || owns(headers, "PolicyHeaderId", id)) {
            List<Map<String, Object>> rows = repo.policyDetail(id);
            if (!rows.isEmpty()) {
                Map<String, Object> r = rows.get(0);
                row = map("PayrollCutOffDay", net(r.get("PayrollCutOffDay")), "IsMonthEndPayroll", toBool(r.get("IsMonthEndPayroll")),
                        "AdvanceAfterDay", net(r.get("AdvanceAfterDay")), "AdvanceLimitPercent", net(r.get("AdvanceLimitPercent")),
                        "OTFactorRate", net(r.get("OTFactorRate")), "IsSeparteOTPayroll", toBool(r.get("IsSeparteOTPayroll")),
                        "ShortNoOfHours", net(r.get("ShortNoOfHours")), "NoOfShortLeaveInMonth", net(r.get("NoOfShortLeaveInMonth")),
                        "NoOfShortHoursInDay", net(r.get("NoOfShortHoursInDay")), "LateNoOfDay", net(r.get("LateNoOfDay")),
                        "NoOfShortLeave", net(r.get("NoOfShortLeave")), "NoOfHalfDay", net(r.get("NoOfHalfDay")),
                        "CPLApplyMinHours", net(r.get("CPLApplyMinHours")), "OTApplyAfterMin", net(r.get("OTApplyAfterMin")),
                        "AllowMultiShiftToEmployeeOnDay", toBool(r.get("AllowMultiShiftToEmployeeOnDay")),
                        "LateArrivalWithGraceTime", toBool(r.get("LateArrivalWithGraceTime")));
            }
        }
        return map("headers", headers, "row", row);
    }

    /**
     * btnsave_Click: header (PolicyHeaderId = RecId only when RecId &gt; 0 and updatemode; ActionType 1 always) with one
     * hrmPolicyDetail, hrmPolicyHeader.Save; "Save Successfully" / "Update Successfully" only when the id came back &gt; 0.
     */
    public Map<String, Object> saveGeneral(PolicyGeneralDto b) {
        UserAccount u = hrm.user(SCREEN_GENERAL_POLICY);
        int recId = Math.max(0, b.id);
        boolean update = recId > 0 && b.updateMode;
        if (update && !owns(repo.policyHeaders(u), "PolicyHeaderId", recId)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        HrmPolicyHeader h = new HrmPolicyHeader();
        if (update) h.PolicyHeaderId = recId;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.DateFrom = toDay(b.dateFrom) == null ? now : toDay(b.dateFrom);
        h.DateTo = toDay(b.dateTo) == null ? now : toDay(b.dateTo);
        h.AlteredOn = now;
        h.AlteredBy = u.getId();
        h.CreatedOn = now;
        h.CreatedBy = u.getId();
        h.ActionType = 1;
        HrmPolicyDetail d = new HrmPolicyDetail();
        d.PayrollCutOffDay = cInt(b.payrollCutOffDay);
        d.IsMonthEndPayroll = b.isMonthEndPayroll;
        d.AdvanceAfterDay = cInt(b.advanceAfterDay);
        d.AdvanceLimitPercent = BigDecimal.valueOf(cInt(b.advanceLimitPercent));
        d.OTFactorRate = BigDecimal.valueOf(cInt(b.otFactorRate));
        d.IsSeparteOTPayroll = b.isSeparteOTPayroll;
        d.ShortNoOfHours = cInt(b.monthlyShortHoursLimit);
        d.NoOfShortLeaveInMonth = cInt(b.noOfShortLeaveInMonth);
        d.NoOfShortHoursInDay = cInt(b.noOfShortHoursInDay);
        d.LateNoOfDay = cInt(b.deductionLeaveNoOfLate);
        d.NoOfShortLeave = cInt(b.deductionLeaveNoOfShortLeave);
        d.NoOfHalfDay = cInt(b.deductionLeaveNoOfHalfDay);
        d.CPLApplyMinHours = cInt(b.cplApplyMinutesHours);
        d.EmployeeDutyHour = cInt(b.cplCalculateHours);
        d.OTApplyAfterMin = cInt(b.overtimeApplyAfterMin);
        d.AllowMultiShiftToEmployeeOnDay = b.isAllowMultiShiftInOneDay;
        d.LateArrivalWithGraceTime = b.showLateArrivalWithGraceTime;
        h.HrmPolicyDetailList = new ArrayList<>();
        h.HrmPolicyDetailList.add(d);
        int n = repo.savePolicy(h);
        String msg = null;
        if (recId == 0 && !b.updateMode && n > 0) msg = "Save Successfully";
        if (recId > 0 && b.updateMode && n > 0) msg = "Update Successfully";
        Map<String, Object> r = saved(n, msg);
        r.put("headers", headerRows(u));
        return r;
    }

    // ================================================================== 645 Salary Breakup Policy (SalaryBreakupPolicy.cs)

    /** ProfileDefine_Load: LocationFill() + gridfill(). */
    public Map<String, Object> breakupSetup() {
        UserAccount u = hrm.user(SCREEN_SALARY_BREAKUP);
        return map("rights", hrm.rights(u, SCREEN_SALARY_BREAKUP), "locations", locationRows(u), "rows", breakupGrid(u));
    }

    /** LocationFill() - btnRefresh_Click. */
    public List<Map<String, Object>> breakupLocations() { return locationRows(hrm.user(SCREEN_SALARY_BREAKUP)); }

    /** gridfill(): every salary type (ProfileTypeId 2) as { Id 0, SalaryId, SalaryType, Percentage 0 }. */
    public List<Map<String, Object>> breakupGridFill() { return breakupGrid(hrm.user(SCREEN_SALARY_BREAKUP)); }

    private List<Map<String, Object>> breakupGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : profileRows(u, PT_SALARY_TYPE))
            out.add(map("Id", 0, "SalaryId", r.get("ProfileId"), "SalaryType", r.get("ProfileName"), "Percentage", 0d));
        return out;
    }

    private List<Map<String, Object>> locationRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.locations(u)) out.add(map("LocationId", r.get("LocationId"), "LocationName", r.get("LocationName")));
        return out;
    }

    /**
     * cmbLocation_ValueChanged -> ExistRecordGridFillByLocationId: hrmSalaryBreakupPolicy.GetByLocationId; rows found =
     * { exists true, RecId = first row's id, rows }, none = gridfill() rows (exists false).
     */
    public Map<String, Object> breakupByLocation(int locationId) {
        UserAccount u = hrm.user(SCREEN_SALARY_BREAKUP);
        if (locationId != 0 && !owns(repo.locations(u), "LocationId", locationId)) throw invalid("Record not found.");
        List<Map<String, Object>> lst = repo.breakupByLocation(locationId);
        if (!lst.isEmpty()) {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : lst)
                rows.add(map("Id", r.get("SalaryBreakupPolicyId"), "SalaryId", r.get("SalaryTypeProfileId"),
                        "SalaryType", r.get("SalaryType"), "Percentage", toDouble(r.get("SalaryTypePercent"))));
            return map("exists", true, "recId", toInt(lst.get(0).get("SalaryBreakupPolicyId")), "rows", rows);
        }
        return map("exists", false, "recId", 0, "rows", breakupGrid(u));
    }

    /** History (web): hrmSalaryBreakupPolicy.Getall rows - { SalaryBreakupPolicyId, LocationId, SalaryType, SalaryTypePercent }. */
    public List<Map<String, Object>> breakupHistory() {
        UserAccount u = hrm.user(SCREEN_SALARY_BREAKUP);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.breakups(u))
            out.add(map("SalaryBreakupPolicyId", r.get("SalaryBreakupPolicyId"), "LocationId", r.get("LocationId"),
                    "SalaryType", r.get("SalaryType"), "SalaryTypePercent", toDouble(r.get("SalaryTypePercent"))));
        return out;
    }

    /**
     * Insert(): "Location Required", "Percent Total should be Equal to 100", then one model per grid row exactly as the
     * form fills it (AlteredById / CreatedById = UserAccount.CompanyId and ActionTypeId 0, as the desktop sends them),
     * and hrmSalaryBreakupPolicy.Save (Insert for every row when RecId == 0, else Update for every row).
     */
    public Map<String, Object> saveBreakup(PolicySalaryBreakupDto b) {
        UserAccount u = hrm.user(SCREEN_SALARY_BREAKUP);
        int recId = Math.max(0, b.id);
        if (b.locationId == 0 || !owns(repo.locations(u), "LocationId", b.locationId)) throw invalid("Location Required");
        List<PolicySalaryBreakupRowDto> rows = b.details == null ? new ArrayList<>() : b.details;
        double total = 0;
        for (PolicySalaryBreakupRowDto r : rows) if (r.percentage != null) total += r.percentage;
        if (total != 100.0) throw invalid("Percent Total should be Equal to 100");
        // tenancy: a row id must be a row of this location; a salary type must be one of the company's salary types
        List<Map<String, Object>> existing = repo.breakupByLocation(b.locationId);
        if (recId > 0 && !owns(existing, "SalaryBreakupPolicyId", recId)) throw invalid("Record not found.");
        List<Map<String, Object>> types = profileRows(u, PT_SALARY_TYPE);
        LocalDateTime now = LocalDateTime.now();
        List<HrmSalaryBreakupPolicy> list = new ArrayList<>();
        for (PolicySalaryBreakupRowDto r : rows) {
            if (r.id != 0 && !owns(existing, "SalaryBreakupPolicyId", r.id)) throw invalid("Record not found.");
            if (!owns(types, "ProfileId", r.salaryId) && !owns(existing, "SalaryTypeProfileId", r.salaryId)) throw invalid("Record not found.");
            HrmSalaryBreakupPolicy c = new HrmSalaryBreakupPolicy();
            c.OrganizationId = u.getOrganizationId();
            c.CompanyId = u.getCompanyId();
            c.AlteredById = u.getCompanyId();       // desktop: cat.AlteredById = UserAccount.CompanyId
            c.AlteredOn = now;
            c.CreatedOn = now;
            c.CreatedById = u.getCompanyId();       // desktop: cat.CreatedById = UserAccount.CompanyId
            c.ActionTypeId = 0;
            c.UserLogId = u.getId();
            c.SalaryBreakupPolicyId = r.id;
            c.SalaryTypeProfileId = r.salaryId;
            c.SalaryTypePercent = r.percentage == null ? 0d : r.percentage;
            c.LocationId = b.locationId;
            list.add(c);
        }
        int n = repo.saveBreakup(recId, list);
        return saved(n, recId > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 646 Leave Quota Policy (LeaveQuotaPolicy.cs)

    /** LeaveQuotaPolicy_Load: rights + gridCasualLeaveFill() (ProfileTypeId 3) + gridSpecialLeaveFill() (ProfileTypeId 4). */
    public Map<String, Object> leaveQuotaSetup() {
        UserAccount u = hrm.user(SCREEN_LEAVE_QUOTA);
        return map("rights", hrm.rights(u, SCREEN_LEAVE_QUOTA), "casual", leaveGrid(u, PT_CASUAL_LEAVE), "special", leaveGrid(u, PT_SPECIAL_LEAVE));
    }

    /** Reset(): both grids refilled. */
    public Map<String, Object> leaveQuotaGrids() {
        UserAccount u = hrm.user(SCREEN_LEAVE_QUOTA);
        return map("casual", leaveGrid(u, PT_CASUAL_LEAVE), "special", leaveGrid(u, PT_SPECIAL_LEAVE));
    }

    private List<Map<String, Object>> leaveGrid(UserAccount u, int type) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : profileRows(u, type))
            out.add(map("Id", r.get("ProfileId"), "LeaveType", r.get("ProfileName"), "QuotaValue", 0d));
        return out;
    }

    /** HistoryGridFill(): { LeaveQuotaId, FromDate / ToDate "dd-MMM-yyyy", Description }. */
    public List<Map<String, Object>> leaveQuotaHistory() {
        UserAccount u = hrm.user(SCREEN_LEAVE_QUOTA);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.leaveQuotas(u))
            out.add(map("LeaveQuotaId", r.get("LeaveQuotaId"), "FromDate", fmt(r.get("FromDate"), DD_MMM_YYYY),
                    "ToDate", fmt(r.get("ToDate"), DD_MMM_YYYY), "Description", r.get("Description")));
        return out;
    }

    /** GetById / HistoryDetailBind -> hrmLeaveQuota.GetByID (header + HrmLeaveQuotaDetailList with ProfileTypeName). */
    public Map<String, Object> leaveQuota(int id) {
        UserAccount u = hrm.user(SCREEN_LEAVE_QUOTA);
        if (!owns(repo.leaveQuotas(u), "LeaveQuotaId", id)) throw invalid("Record not found.");
        Map<String, Object> h = one(repo.leaveQuota(id));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> d : repo.leaveQuotaDetails(toInt(h.get("LeaveQuotaId"))))
            details.add(map("LeaveTypeProfileId", d.get("LeaveTypeProfileId"), "ProfileName", d.get("ProfileName"),
                    "ProfileTypeName", d.get("ProfileTypeName"), "QuotaValue", toInt(d.get("QuotaValue"))));
        return map("LeaveQuotaId", h.get("LeaveQuotaId"), "FromDate", fmt(h.get("FromDate"), ISO), "ToDate", fmt(h.get("ToDate"), ISO),
                "Description", str(h.get("Description")), "details", details);
    }

    /**
     * Insert() after the page's "Are you sure to Save? / Update?": the model as the form fills it, the "at least one row"
     * check exactly as the form codes it, both grids' rows as details, and hrmLeaveQuota.Save.
     */
    public Map<String, Object> saveLeaveQuota(PolicyLeaveQuotaDto b) {
        UserAccount u = hrm.user(SCREEN_LEAVE_QUOTA);
        int id = Math.max(0, b.id);
        hrm.require(u, SCREEN_LEAVE_QUOTA, id > 0 ? "Update" : "Save");
        if (id > 0 && !owns(repo.leaveQuotas(u), "LeaveQuotaId", id)) throw invalid("Record not found.");
        LocalDateTime from = toDay(b.fromDate), to = toDay(b.toDate);
        if (from == null || to == null) throw invalid("Please select a valid date.");
        List<PolicyLeaveQuotaRowDto> casual = b.casual == null ? new ArrayList<>() : b.casual;
        List<PolicyLeaveQuotaRowDto> special = b.special == null ? new ArrayList<>() : b.special;

        HrmLeaveQuota m = new HrmLeaveQuota();
        LocalDateTime today = LocalDate.now().atStartOfDay();
        m.LeaveQuotaId = id;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.CreatedOn = today;
        m.CreatedById = u.getId();
        m.AlteredById = u.getId();
        m.AlteredOn = today;
        m.FromDate = from;
        m.ToDate = to;
        m.LocationId = 0;
        m.IsActive = true;
        m.Description = trim(b.description);
        boolean met = false;
        for (PolicyLeaveQuotaRowDto r : casual) if (r.quotaValue != null && cInt(r.quotaValue) != 0) { met = true; break; }
        // desktop: Conversion.ToInt((object)grdspecialleave.GetRow(j).Cells["QuotaValue"]) - the CELL, not its Value:
        // Convert.ToInt32(GridEXCell) always fails -> 0, so a Special Leave quota alone never satisfies the check.
        if (!met) throw invalid("At least one row needs to be filled in one of the grids.");

        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> r : profileRows(u, PT_CASUAL_LEAVE)) allowed.add(toInt(r.get("ProfileId")));
        for (Map<String, Object> r : profileRows(u, PT_SPECIAL_LEAVE)) allowed.add(toInt(r.get("ProfileId")));
        if (id > 0) for (Map<String, Object> d : repo.leaveQuotaDetails(id)) allowed.add(toInt(d.get("LeaveTypeProfileId")));
        m.HrmLeaveQuotaDetailList = new ArrayList<>();
        List<PolicyLeaveQuotaRowDto> all = new ArrayList<>(casual);
        all.addAll(special);
        for (PolicyLeaveQuotaRowDto r : all) {
            if (!allowed.contains(r.id)) throw invalid("Record not found.");
            HrmLeaveQuotaDetail d = new HrmLeaveQuotaDetail();
            d.LeaveTypeProfileId = r.id;
            d.QuotaValue = cInt(r.quotaValue);
            m.HrmLeaveQuotaDetailList.add(d);
        }
        m.ActionTypeId = id == 0 ? 1 : 2;               // BLL hrmLeaveQuota.Save
        int n = repo.saveLeaveQuota(m);
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 647 E.O.B.I Policy (EOBIPolicy.cs)

    /** EOBIPolicy_Load: rights + DetailGridFill(). */
    public Map<String, Object> eobiSetup() {
        UserAccount u = hrm.user(SCREEN_EOBI);
        return map("rights", hrm.rights(u, SCREEN_EOBI), "employees", eobiGrid(u));
    }

    /** DetailGridFill() - clear(). */
    public List<Map<String, Object>> eobiEmployees() { return eobiGrid(hrm.user(SCREEN_EOBI)); }

    private List<Map<String, Object>> eobiGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.eobiEmployees(u)) {
            LocalDateTime dob = toDate(r.get("DOB"));
            out.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"),
                    "DOB", (dob == null ? NET_NULL_DATE : dob).format(DD_MMM_YYYY),   // Conversion.ToDateTime(DBNull) = 01-Jan-1900
                    "TotalSalary", toDouble(r.get("TotalSalary")), "EmployeeShare", 0d, "CompanyShare", 0d, "TotalShare", 0d));
        }
        return out;
    }

    /** gridfill() (History tab): { Id, PolicyDescription, FromDate / ToDate ToShortDateString, AgeLimit, CompanyShare, EmployeeShare }. */
    public List<Map<String, Object>> eobiHistory() {
        UserAccount u = hrm.user(SCREEN_EOBI);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.eobiPolicies(u))
            out.add(map("Id", r.get("EOBIPolicyId"), "PolicyDescription", r.get("PolicyDescription"),
                    "FromDate", fmt(r.get("FromDate"), NET_SHORT), "ToDate", fmt(r.get("ToDate"), NET_SHORT),
                    "AgeLimit", r.get("AgeLimit"), "CompanyShare", net(r.get("CompanyShare")), "EmployeeShare", net(r.get("EmployeeShare"))));
        return out;
    }

    /** DataHistoryGrrid_DoubleClick -> hrmEOBIPolicy.GetByID (DAL GetData also reads the policy's employees). */
    public Map<String, Object> eobiPolicy(int id) {
        UserAccount u = hrm.user(SCREEN_EOBI);
        if (!owns(repo.eobiPolicies(u), "EOBIPolicyId", id)) throw invalid("Record not found.");
        Map<String, Object> h = one(repo.eobiPolicy(id));
        List<Map<String, Object>> emps = repo.eobiPolicyEmployees(toInt(h.get("EOBIPolicyId")));
        return map("EOBIPolicyId", h.get("EOBIPolicyId"), "PolicyDescription", str(h.get("PolicyDescription")),
                "FromDate", fmt(h.get("FromDate"), ISO), "ToDate", fmt(h.get("ToDate"), ISO),
                "AgeLimit", toInt(h.get("AgeLimit")), "EmployeeShare", toDouble(h.get("EmployeeShare")),
                "CompanyShare", toDouble(h.get("CompanyShare")), "employees", emps.size());
    }

    /**
     * Insert(): formvalidation, "Date From Is Greater Than Date To Please Check", (the page asked "Are you sure ...?"),
     * the model as the form fills it, only the grid rows with EmployeeShare &gt; 0, and hrmEOBIPolicy.Save.
     */
    public Map<String, Object> saveEobi(PolicyEobiDto b) {
        UserAccount u = hrm.user(SCREEN_EOBI);
        int id = Math.max(0, b.id);
        hrm.require(u, SCREEN_EOBI, id > 0 ? "Update" : "Save");
        if (trim(b.policyDescription).isEmpty()) throw invalid("Please Insert Policy Description");
        if (trim(b.ageLimit).isEmpty()) throw invalid("Please Insert Age Limit");
        if (trim(b.employeeShare).isEmpty()) throw invalid("Please Insert Employee Share");
        if (trim(b.companyShare).isEmpty()) throw invalid("Please Insert Company Share");
        LocalDateTime from = toDay(b.fromDate), to = toDay(b.toDate);
        if (from == null || to == null) throw invalid("Please select a valid date.");
        if (from.isAfter(to)) throw invalid("Date From Is Greater Than Date To Please Check");
        if (id > 0 && !owns(repo.eobiPolicies(u), "EOBIPolicyId", id)) throw invalid("Record not found.");

        HrmEOBIPolicy m = new HrmEOBIPolicy();
        LocalDateTime today = LocalDate.now().atStartOfDay();
        m.EOBIPolicyId = id;
        m.PolicyDescription = str(b.policyDescription);   // not trimmed on the desktop
        m.FromDate = from;
        m.ToDate = to;
        m.AgeLimit = cInt(trim(b.ageLimit));
        m.EmployeeShare = toDouble(trim(b.employeeShare));
        m.CompanyShare = toDouble(trim(b.companyShare));
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.AlteredOn = today;
        m.AlteredById = u.getId();
        m.CreatedOn = today;
        m.CreatedById = u.getId();
        m.ActionTypeId = 1;
        m.UserLogId = u.getId();
        m.EOBIPolicyEmployeesList = new ArrayList<>();
        List<Map<String, Object>> emps = null;
        if (b.details != null) for (PolicyEobiRowDto r : b.details) {
            double es = r.employeeShare == null ? 0 : r.employeeShare;
            if (!(es > 0.0)) continue;
            if (emps == null) emps = repo.eobiEmployees(u);
            if (!owns(emps, "EmployeeId", (int) r.employeeId)) throw invalid("Record not found.");
            HrmEOBIPolicyEmployee e = new HrmEOBIPolicyEmployee();
            e.EmployeeId = r.employeeId;
            e.CompanyShare = toDec(r.companyShare);
            e.EmployeeShare = toDec(r.employeeShare);
            m.EOBIPolicyEmployeesList.add(e);
        }
        int n = repo.saveEobiPolicy(m);
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== helpers

    private List<Map<String, Object>> profileRows(UserAccount u, int type) { return repo.profilesByType(u, type); }

    /** BindDDLNew(dt, cmb, "ProfileId", "ProfileName", ...) rows. */
    private List<Map<String, Object>> profiles(UserAccount u, int type) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : profileRows(u, type)) out.add(map("ProfileId", r.get("ProfileId"), "ProfileName", r.get("ProfileName")));
        return out;
    }

    /**
     * Conversion.ToInt = Convert.ToInt32 inside try/catch: null / "" -> 0; a string must be a whole number
     * (int.Parse, NumberStyles.Integer: surrounding blanks and a sign allowed) else 0; a double / decimal is
     * rounded to the nearest integer, halves to even; out of range -> 0.
     */
    static int cInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v instanceof Integer || v instanceof Short || v instanceof Byte) return ((Number) v).intValue();
        if (v instanceof Number) {
            BigDecimal d;
            try { d = v instanceof BigDecimal ? (BigDecimal) v : new BigDecimal(String.valueOf(((Number) v).doubleValue())); }
            catch (NumberFormatException e) { return 0; }
            d = d.setScale(0, RoundingMode.HALF_EVEN);
            if (d.compareTo(BigDecimal.valueOf(Integer.MAX_VALUE)) > 0 || d.compareTo(BigDecimal.valueOf(Integer.MIN_VALUE)) < 0) return 0;
            return d.intValue();
        }
        String s = String.valueOf(v).trim();
        if (!s.matches("[+-]?\\d+")) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); } catch (NumberFormatException e) { return 0; }
    }

    /** DataRow cell .ToString() as the desktop puts it into a text box (decimal keeps its scale: 10.00). */
    private static String net(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) {
            double d = ((Number) v).doubleValue();
            if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
            return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
        }
        if (v instanceof Boolean) return ((Boolean) v) ? "True" : "False";
        return String.valueOf(v);
    }

    private static String fmt(Object v, DateTimeFormatter f) {
        LocalDateTime d = toDate(v);
        return d == null ? "" : d.format(f);
    }
}
