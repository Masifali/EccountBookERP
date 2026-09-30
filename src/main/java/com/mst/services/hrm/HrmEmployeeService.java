package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.EmployeeAddressRowDto;
import com.mst.models.hrm.dto.EmployeeBankRowDto;
import com.mst.models.hrm.dto.EmployeeBenefitRowDto;
import com.mst.models.hrm.dto.EmployeeEducationRowDto;
import com.mst.models.hrm.dto.EmployeeExperienceRowDto;
import com.mst.models.hrm.dto.EmployeeFamilyRowDto;
import com.mst.models.hrm.dto.EmployeeReferenceRowDto;
import com.mst.models.hrm.dto.EmployeeRegistrationDto;
import com.mst.models.hrm.dto.EmployeeSalaryRowDto;
import com.mst.models.hrm.dto.EmployeeWeekDayRowDto;
import com.mst.models.hrm.employee.EmployeeSalary;
import com.mst.models.hrm.employee.GenEmployee;
import com.mst.models.hrm.employee.GenEmployeeEducation;
import com.mst.models.hrm.employee.GenEmployeeHistory;
import com.mst.models.hrm.employee.GenEmployeeWeekDay;
import com.mst.models.hrm.employee.HrmEmployeeBankAccount;
import com.mst.models.hrm.employee.HrmEmployeeBenefit;
import com.mst.models.hrm.employee.HrmEmployeeExperience;
import com.mst.models.hrm.employee.HrmEmployeeFamilyInfo;
import com.mst.models.hrm.employee.HrmEmployeeReference;
import com.mst.models.hrm.employee.TblEmployeeAddressDetail;
import com.mst.repositories.hrm.HrmEmployeeRepository;
import com.mst.repositories.hrm.HrmEmployeeRepository.EmployeeSave;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of screen 648 Employee Registration (Architecture.WinApp.HRM.EmployeeManagement.frmEmployeeRegistration).
 * Each method names the desktop form method it reproduces; validation wording, order and the MessageBox
 * texts are the form's. Tenancy and audit come from the session; every id the page sends is checked
 * against the company's own lists before it is written.
 */
@Service
public class HrmEmployeeService {

    public static final int SCREEN_EMPLOYEE_REGISTRATION = 648;
    /** frmDutyRoasterNew / frmGenShiftTiming / AcfrmDefCoa rights the toolbar buttons check (SetRightsValueInRightsObject). */
    public static final int SCREEN_DUTY_ROASTER = 656;
    public static final int SCREEN_SHIFT_TIMING = 654;
    public static final int SCREEN_DEFINE_COA = 9;
    /** UserAccount.AppId of an HRM login (dbo.App 12) - what COAAllocation.GetAccountTitleByAccountTypeIds receives as @AppId. */
    public static final int APP_HRM = 12;

    /** genProfile.ProfileTypeId values the form filters dtAllLookUp by. */
    private static final int PT_NATIONALITY = 15, PT_EMPLOYEE_TYPE = 20, PT_BLOOD_GROUP = 22, PT_TITLE = 24,
            PT_RELATION = 25, PT_GENDER = 26, PT_REPORTED_BRANCH = 28, PT_STORE = 31, PT_REPORTED_PERSON = 34,
            PT_RELATIONSHIP = 47, PT_DEGREE = 57, PT_ADDRESS_TYPE = 59, PT_WEEK_DAYS = 60;

    @Autowired private HrmEmployeeRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== frmEmployeeRegistration_Load

    /**
     * frmEmployeeRegistration_Load: rights (btnsave / btnupdate), every combo (the fills below), the generated
     * Employee No, the benefit and week-day grids, and txtEndDate = clsGlobalVariables.ActiveYr.End_Period.
     */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", hrm.rights(u, SCREEN_EMPLOYEE_REGISTRATION));
        out.put("links", map("dutyRoaster", hrm.can(u, SCREEN_DUTY_ROASTER, "View"),
                "shiftTiming", hrm.can(u, SCREEN_SHIFT_TIMING, "View"),
                "coa", hrm.can(u, SCREEN_DEFINE_COA, "View")));
        out.put("lookups", lookups(u));
        out.putAll(resetData(u));
        out.put("endDate", activeYearEnd());
        return out;
    }

    /** btnRefresh_Click: every combo filled again (the page then re-runs GridBindEmployeeSalary for the location). */
    public Map<String, Object> refresh() {
        return lookups(hrm.user(SCREEN_EMPLOYEE_REGISTRATION));
    }

    /**
     * btnnew_Click's database parts: EmlpoyeeNoGenerate (Reset), grdEmployeeDirectBenefitFill /
     * grdEmployeeAssetsBenefitFill (ResetEmployeeBenefits) and grdWeekDaysFill (ResetWeekDays).
     */
    public Map<String, Object> reset() {
        return resetData(hrm.user(SCREEN_EMPLOYEE_REGISTRATION));
    }

    private Map<String, Object> resetData(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("employeeNo", generateEmployeeNo(u));
        List<Map<String, Object>> direct = new ArrayList<>(), assets = new ArrayList<>();
        for (Map<String, Object> r : repo.benefits(u)) {
            int type = toInt(r.get("BenefitTypeProfileId"));
            // dtEmployee*Benefits.Rows.Add(BenefitId, ProfileName, BenefitName): column "BenefitName" holds the benefit
            // TYPE's profile name and "BenefitValue" the benefit's own name - as the form adds them.
            Map<String, Object> row = map("BenefitId", r.get("BenefitId"), "BenefitName", r.get("ProfileName"),
                    "BenefitValue", r.get("BenefitName"), "ApprovalPersonId", null, "Select", false);
            if (type == 1) direct.add(row);
            if (type == 2) assets.add(row);
        }
        out.put("directBenefits", direct);
        out.put("assetBenefits", assets);
        List<Map<String, Object>> days = new ArrayList<>();
        for (Map<String, Object> r : repo.profiles(u, PT_WEEK_DAYS))       // dtWeekDays.Rows.Add(ProfileId, ProfileName, 1)
            days.add(map("WeekDaysProfileId", r.get("ProfileId"), "WeekDays", r.get("ProfileName"), "Select", true));
        out.put("weekDays", days);
        return out;
    }

    /** EmlpoyeeNoGenerate: genEmployee.GenerateEmployeeNo; txtEmployeeNo is set only when the code is > 0 ("" otherwise). */
    private String generateEmployeeNo(UserAccount u) {
        List<Map<String, Object>> r = repo.generateEmployeeNo(u);
        int code = r.isEmpty() ? 0 : toInt(r.get(0).get("EmployeeNo"));
        return code > 0 ? String.valueOf(code) : "";
    }

    private String activeYearEnd() {
        int fy = hrm.financialYearId();
        if (fy <= 0) return null;
        List<Map<String, Object>> r = repo.financialYear(fy);
        if (r.isEmpty()) return null;
        LocalDateTime d = toDate(r.get(0).get("End_Period"));
        return d == null ? null : d.toLocalDate().toString();
    }

    /** Every combo of the form, in the columns its DDL.BindDDL / BindDDLNew call binds. */
    private Map<String, Object> lookups(UserAccount u) {
        Map<String, Object> l = new LinkedHashMap<>();
        // AllLookUpFill: genProfile 'All' -> the form splits it by ProfileTypeId (EmployeeTypeComboFill, NationalityComboFill ...)
        List<Map<String, Object>> all = repo.profiles(u, 0);
        l.put("employeeTypes", profileRows(all, PT_EMPLOYEE_TYPE));
        l.put("nationalities", profileRows(all, PT_NATIONALITY));
        l.put("bloodGroups", profileRows(all, PT_BLOOD_GROUP));
        l.put("titles", profileRows(all, PT_TITLE));
        l.put("relations", profileRows(all, PT_RELATION));
        l.put("genders", profileRows(all, PT_GENDER));
        l.put("reportedBranches", profileRows(all, PT_REPORTED_BRANCH));
        l.put("stores", profileRows(all, PT_STORE));
        l.put("reportedPersons", profileRows(all, PT_REPORTED_PERSON));   // also dtReportedPersone (benefit Approval Person)
        l.put("addressTypes", profileRows(all, PT_ADDRESS_TYPE));
        l.put("degrees", profileRows(all, PT_DEGREE));
        l.put("relationships", profileRows(all, PT_RELATIONSHIP));
        l.put("employeeGroups", cols(repo.employeeGroups(u), "EmployeeGroupId", "EmployeeGroupName"));
        List<Map<String, Object>> cats = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeCategories(u)) cats.add(map("Id", r.get("EmployeeCategoryId"), "EmployeeCategory", r.get("EmployeeCategoryName")));
        l.put("employeeCategories", cats);
        l.put("departments", cols(repo.departments(u), "DepartmentId", "DepartmentName"));
        l.put("locations", cols(repo.locations(u), "LocationId", "LocationName"));
        l.put("sections", cols(repo.sections(u), "SectionId", "SectionName"));
        l.put("designations", cols(repo.designations(u), "DesignationId", "DesignationName"));
        l.put("shifts", cols(repo.shifts(u), "ShiftId", "ShiftName"));
        l.put("countries", cols(repo.countries(), "Id", "Description"));
        l.put("cities", cols(repo.cities(u), "Id", "CityName"));
        l.put("provinces", cols(repo.provinces(u), "Id", "Description"));
        l.put("banks", cols(repo.banks(u), "Id", "BranchName"));
        l.put("projects", cols(repo.projects(u), "Id", "ProjectName"));
        l.put("branches", cols(repo.branches(u), "Id", "BranchName"));
        l.put("expenseAccounts", cols(repo.accountsByType(u, APP_HRM, "11,12,20,21"), "Id", "AccountTitle"));
        l.put("payableAccounts", cols(repo.accountsByType(u, APP_HRM, "5,8"), "Id", "AccountTitle"));   // CmbSalaryPayableAc and CmbSalaryLoanAc
        return l;
    }

    private static List<Map<String, Object>> profileRows(List<Map<String, Object>> all, int type) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) if (toInt(r.get("ProfileTypeId")) == type) out.add(map("Id", r.get("ProfileId"), "Name", r.get("ProfileName")));
        return out;
    }

    private static List<Map<String, Object>> cols(List<Map<String, Object>> rows, String v, String t) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map(v, r.get(v), t, r.get(t)));
        return out;
    }

    // ================================================================== events that read the DB

    /** cmbLocation_ValueChanged -> GridBindEmployeeSalary(LocationId): the location's breakup, Amount 0. */
    public List<Map<String, Object>> salaryBreakup(int locationId) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        if (locationId <= 0) return Collections.emptyList();
        if (!owns(repo.locations(u), "LocationId", locationId)) throw invalid("Record not found.");
        return breakupRows(locationId);
    }

    private List<Map<String, Object>> breakupRows(int locationId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.salaryBreakupByLocation(locationId))
            out.add(map("SalaryTypeId", r.get("SalaryTypeProfileId"), "SalaryType", r.get("SalaryType"), "TypePercent", r.get("SalaryTypePercent"), "Amount", 0));
        return out;
    }

    /** cmbDepartment_Leave: GetCaoByDepartmentId -> the department's Payable / Expense / Loan accounts (empty = first rows). */
    public Map<String, Object> departmentAccounts(int departmentId) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        if (!owns(repo.departments(u), "DepartmentId", departmentId)) return map("found", false);
        List<Map<String, Object>> r = repo.accountsByDepartment(u, departmentId);
        if (r.isEmpty()) return map("found", false);
        Map<String, Object> x = r.get(0);
        return map("found", true, "PayableAcId", toInt(x.get("PayableAcId")), "ExpenseAccountId", toInt(x.get("ExpenseAccountId")), "LoanAcId", toInt(x.get("LoanAcId")));
    }

    /** txtEmployeeNoForUpdate_Leave: GetEmployeeIdByEmployeeNo; 0 -> "Record Not Found For this EmployeeNo". */
    public Map<String, Object> idByEmployeeNo(String employeeNo) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        List<Map<String, Object>> r = repo.employeeIdByNo(u, trim(employeeNo));
        int id = r.isEmpty() ? 0 : toInt(r.get(0).get("EmployeeId"));
        if (id <= 0) throw invalid("Record Not Found For this EmployeeNo");
        return map("id", id);
    }

    /** tabPage2 History: GridBindEmployeeRegistration - genEmployee.Getall, the columns the form builds. */
    public List<Map<String, Object>> history() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employees(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("EmployeeId", r.get("EmployeeId"));
            m.put("EmployeeNo", r.get("EmployeeNo"));
            m.put("EmployeeType", r.get("EmployeeType"));
            m.put("EmployeeCatagory", r.get("EmployeeCategory"));
            m.put("Title", r.get("TitleName"));
            m.put("FirstName", r.get("FistName"));
            m.put("MiddleName", r.get("MiddleName"));
            m.put("LastName", r.get("LastName"));
            m.put("RelationTitle", r.get("RelationTitleName"));
            m.put("RelationName", r.get("RelationName"));
            m.put("RelationMobileNo", r.get("RelationMobile"));
            m.put("Gender", r.get("Gender"));
            m.put("DOB", day(r.get("DOB")));
            m.put("Nationality", r.get("Nationality"));
            m.put("Email", r.get("Email"));
            m.put("Mobile1", r.get("Mobile1"));
            m.put("Mobile2", r.get("Mobile2"));
            m.put("CNIC", r.get("CNIC"));
            m.put("CNIC Issue Date", day(r.get("CNICIssueDate")));
            m.put("CNIC Expiry Date", day(r.get("CNICExpiryDate")));
            m.put("PassportNo", r.get("PassportNo"));
            m.put("LicenseNo", r.get("LicenseNo"));
            m.put("BloodGroup", r.get("BloodGroup"));
            m.put("FlatSharePercent", r.get("FlatSharePercent"));
            m.put("IsAllowShare", r.get("IsAllowShare"));
            m.put("NeedLogin", r.get("NeedLogin"));
            m.put("SignPictureFilePath", r.get("SignPictureFilePath"));
            m.put("PictureFilePath", r.get("PictureFilePath"));
            m.put("Status", r.get("ActiveStatus"));
            out.add(m);
        }
        return out;
    }

    private static String day(Object v) { LocalDateTime d = toDate(v); return d == null ? null : d.toLocalDate().toString(); }

    /** btnPrint / grid Print / btnPrintRegister: EmployeeRegistrationSlipAndRegister row count ("Not Record Found For Display" when 0). */
    public Map<String, Object> printCheck(int employeeId) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        return map("rows", repo.slipAndRegister(u, Math.max(0, employeeId)).size());
    }

    // ================================================================== RetrivedDataEmployeeRegistration

    /**
     * RetrivedDataEmployeeRegistration(Id): genEmployee.GetByID (header + the DAL's ten child reads), mapped to the
     * controls / grid columns exactly as the form fills them. Only an employee of the signed-in company.
     * Setting cmbLocation fires cmbLocation_ValueChanged (GridBindEmployeeSalary) before the saved salaries replace
     * the grid, so the location's breakup is returned as well.
     */
    public Map<String, Object> employee(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        Map<String, Object> e = mine(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("EmployeeId", e.get("EmployeeId"));
        for (String k : new String[] { "EmployeeNo", "EmployeeTypeProfileId", "EmployeeCategoryId", "TitleId", "FistName", "MiddleName", "LastName",
                "SubTitleId", "RelationName", "RelationMobile", "GenderProfileId", "NationalityProfileId", "Email", "Mobile1", "Mobile2", "CNIC",
                "PassportNo", "LicenseNo", "BloodGroupProfileId", "BranchesId", "ProjectId", "PictureFilePath", "SignPictureFilePath" })
            h.put(k, e.get(k));
        h.put("DOB", day(e.get("DOB")));
        h.put("CNICIssueDate", day(e.get("CNICIssueDate")));
        h.put("CNICExpiryDate", day(e.get("CNICExpiryDate")));
        out.put("employee", h);

        List<Map<String, Object>> hist = repo.employeeChild(id, "ReadEmployeeHistoryByEmployeeId");
        if (!hist.isEmpty()) {
            Map<String, Object> x = hist.get(0);                         // EmployeeHistorylist[0]
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("FromDate", day(x.get("FromDate")));
            m.put("ToDate", day(x.get("ToDate")));
            m.put("ProbationEnding", day(x.get("ProbationEnding")));
            for (String k : new String[] { "DesignationId", "ShiftId", "DepartmentId", "PayableAcId", "ExpenseAccountId", "LoanAcId", "LocationId",
                    "SectionId", "EmployeeTypeProfileId", "PartyLocationId", "ReportToEmployeeId", "StoreId", "EmployeeGroupId" })
                m.put(k, toInt(x.get(k)));
            // txtSalary.Text = Conversion.ToInt(TotalSalary).ToString(): Convert.ToInt32(decimal) rounds half to even
            m.put("TotalSalary", String.valueOf(toDec(x.get("TotalSalary")).setScale(0, RoundingMode.HALF_EVEN).intValue()));
            m.put("Active", toBool(x.get("Active")));
            out.put("history", m);
            int loc = toInt(x.get("LocationId"));
            out.put("locationBreakup", loc > 0 ? breakupRows(loc) : Collections.emptyList());
        } else {
            out.put("history", null);
            out.put("locationBreakup", Collections.emptyList());
        }

        List<Map<String, Object>> sal = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeSalaryByEmployeeId"))
            sal.add(map("SalaryTypeId", r.get("SalaryTypeProfileId"), "SalaryType", r.get("SalaryType"), "TypePercent", r.get("SalaryTypePercent"), "Amount", r.get("Amount")));
        out.put("salaries", sal);

        List<Map<String, Object>> ben = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeBenfitsByEmployeeId"))
            ben.add(map("BenefitId", r.get("BenefitId"), "ProfileName", r.get("ProfileName"), "Description", r.get("Description"), "ApprovalEmployeeId", r.get("ApprovalEmployeeId")));
        out.put("benefits", ben);

        List<Map<String, Object>> wd = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadWeekDaysByEmployeeId"))
            wd.add(map("WeekDayProfileId", r.get("WeekDayProfileId"), "WeekDayProfileName", r.get("WeekDayProfileName")));
        out.put("weekDays", wd);

        List<Map<String, Object>> ad = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeAddressByEmployeeId"))
            ad.add(map("AddressTypeId", r.get("AddressTypeId"), "AddressType", r.get("AddressType"), "CountryId", r.get("CountryId"), "Country", r.get("CountryName"),
                    "ProvinceId", r.get("ProvinceId"), "Province", r.get("StateProvince"), "CityId", r.get("CityId"), "City", r.get("CityName"), "AddressDetail", r.get("AddressDetail")));
        out.put("addresses", ad);

        List<Map<String, Object>> ex = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeExperenceByEmployeeId"))
            ex.add(map("DesignationId", r.get("DesignationId"), "Designation", r.get("Designation"), "Organization", r.get("OrginizationName"), "CityId", r.get("CityId"),
                    "City", r.get("CityName"), "FromDate", day(r.get("FromDate")), "ToDate", day(r.get("ToDate")), "Description", r.get("Description")));
        out.put("experiences", ex);

        List<Map<String, Object>> ed = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeEducationByEmployeeId"))
            ed.add(map("DegreeId", r.get("DegreeId"), "Degree", r.get("DegreeName"), "DegreeTitle", r.get("DegreeTitle"), "Institute", r.get("Institute"), "CityId", r.get("CityId"),
                    "City", r.get("CityName"), "StartYear", day(r.get("StartYear")), "PassingYear", day(r.get("PassingYear")), "CGPA", r.get("CGPA")));
        out.put("educations", ed);

        List<Map<String, Object>> fa = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeFamilyInfoByEmployeeId"))
            fa.add(map("RelationShipId", r.get("RelationshipProfileId"), "RelationShip", r.get("RelationshipName"), "RelationName", r.get("Name"), "Description", r.get("Description")));
        out.put("families", fa);

        List<Map<String, Object>> re = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeReferebceByEmployeeId"))
            re.add(map("ReferenceName", r.get("Name"), "OrganizationName", r.get("OrginizationName"), "DesignationId", r.get("DesignationId"), "Designation", r.get("DesignationName"),
                    "Mobile", r.get("Mobile"), "Email", r.get("Email"), "Description", r.get("Description")));
        out.put("references", re);

        List<Map<String, Object>> bk = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeChild(id, "ReadEmployeeBankByEmployeeId"))
            // The form puts BankProfileId (never written, always 0) into the BankId column, so an Update re-inserted every
            // account with BankAccountId 0 and the next read's INNER JOIN Bank dropped it. BankAccountId - the id the form
            // saved and the procedure joins on - is used instead (see report).
            bk.add(map("BankId", r.get("BankAccountId"), "Bank", r.get("BankName"), "AccountTitle", r.get("AccountTitle"), "AccountNo", r.get("AccountNo"),
                    "IsPayRoll", toBool(r.get("IsPayrollAccount"))));
        out.put("banks", bk);
        return out;
    }

    /** GetByID's row of this company (the desktop reads any id; the web only this company's). */
    private Map<String, Object> mine(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.employee(id);
        if (r.isEmpty()) throw invalid("Record not found.");
        Map<String, Object> e = r.get(0);
        if (toInt(e.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(e.get("CompanyId")) != toInt(u.getCompanyId()))
            throw invalid("Record not found.");
        return e;
    }

    // ================================================================== btnsave_Click / btnupdate_Click

    /**
     * btnsave_Click (btnupdate_Click calls it): ValidationEmployeeRegistration, the CNIC ten-year check,
     * EmployeeInformationValidation, "Employee Salary Breakup Required", the benefit row checks - all in the form's
     * order and wording (the page shows the Yes/No confirmation before calling) - then genEmployee.Save with the model
     * and the ten child lists filled exactly as the form fills them. "Information Saved" / "Information Update".
     */
    public Map<String, Object> saveEmployee(EmployeeRegistrationDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTRATION);
        int recId = Math.max(0, b.id);
        hrm.require(u, SCREEN_EMPLOYEE_REGISTRATION, recId == 0 ? "Save" : "Update");

        // ---- ValidationEmployeeRegistration
        if (toInt(b.employeeNo) == 0) throw invalid("EmployeeNo Required!");
        if (b.employeeTypeId == 0) throw invalid("Employee Type Required!");
        if (b.employeeCategoryId == 0) throw invalid("Employee Catagory Required!");
        if (b.titleId == 0) throw invalid("Employee Title Required!");
        if (trim(b.firstName).isEmpty()) throw invalid("FirstName Required!");
        if (trim(b.lastName).isEmpty()) throw invalid("LastName Required!");
        if (b.genderId == 0) throw invalid("Gender Required!");
        if (b.nationalityId == 0) throw invalid("Nationality Required!");
        if (trim(b.cnic).isEmpty()) throw invalid("CNIC Required!");
        if (trim(b.mobile1).isEmpty()) throw invalid("Mobile1 Required!");
        if (b.departmentId == 0) throw invalid("Department Required!");                // ActiveRow == null
        if (b.payableAcId == 0) throw invalid("Payables Account Field Required");
        if (b.expenseAccountId == 0) throw invalid("Expense Account Field Required");
        if (b.loanAcId == 0) throw invalid("Loan Account Field Required");

        LocalDateTime today = LocalDateTime.now().toLocalDate().atStartOfDay();
        LocalDateTime cnicIssue = dateOr(b.cnicIssueDate, today), cnicExpiry = dateOr(b.cnicExpiryDate, today);
        if (cnicIssue.plusYears(10).isBefore(cnicExpiry)) throw invalid("CNIC Expired!");

        // ---- EmployeeInformationValidation
        if (b.designationId == 0) throw invalid("Designation Required");
        if (b.shiftId == 0) throw invalid("Shift Required");
        if (b.departmentId == 0) throw invalid("Department Required");
        if (b.locationId == 0) throw invalid("Location Required");
        if (b.sectionId == 0) throw invalid("Section Required");
        if (b.employeeTypeId == 0) throw invalid("Employee Type Required");
        if (toDouble(b.totalSalary) == 0.0) throw invalid("Salary Required");

        if (b.salaries == null || b.salaries.isEmpty()) throw invalid("Employee Salary Breakup Required");
        checkBenefits(b.directBenefits);
        checkBenefits(b.assetBenefits);

        // ---- tenancy: every id picked on the page belongs to this company's lists
        String oldPicture = "", oldSign = "";
        if (recId > 0) {
            Map<String, Object> e = mine(u, recId);
            oldPicture = str(e.get("PictureFilePath"));
            oldSign = str(e.get("SignPictureFilePath"));
        }
        checkIds(u, b);

        LocalDateTime now = LocalDateTime.now();
        int userId = toInt(u.getId());
        int org = toInt(u.getOrganizationId()), comp = toInt(u.getCompanyId());

        GenEmployee m = new GenEmployee();
        m.EmployeeId = recId;
        m.ActionTypeId = 0;
        m.AddressDetail = "";
        m.AddressId = 0;
        m.AlteredById = userId;
        m.AlteredOn = now;
        m.BillCategoryProfileId = 45;
        m.BloodGroupProfileId = b.bloodGroupId;
        m.CNIC = trim(b.cnic);
        m.CNICExpiryDate = cnicExpiry;
        m.CNICIssueDate = cnicIssue;
        m.CompanyId = comp;
        m.BranchesId = b.branchesId;
        m.ProjectId = b.projectId;
        m.ConsultantTypeProfileId = 46;
        m.ControlTypeProfileId = 0;
        m.CreatedById = userId;
        m.CreatedOn = now;
        m.DOB = dateOr(b.dob, today);
        m.Email = str(b.email);
        m.EmployeeCategoryId = b.employeeCategoryId;
        m.EmployeeHistoryId = 0;
        m.EmployeeNo = str(b.employeeNo);
        m.EmployeePictureString = "";
        m.EmployeeTypeProfileId = b.employeeTypeId;
        m.FistName = trim(b.firstName);
        m.GenderProfileId = b.genderId;
        m.IsApproved = false;
        m.LastName = trim(b.lastName);
        m.LicenseNo = trim(b.licenseNo);
        m.MiddleName = trim(b.middleName);
        m.Mobile1 = trim(b.mobile1);
        m.Mobile2 = trim(b.mobile2);
        m.NationalityProfileId = b.nationalityId;
        m.OrganizationId = org;
        m.PartyLocationId = 0;
        m.PassportNo = str(b.passportNo);
        m.PersonId = 0;
        // fileSavePath / fileSavePathcookingpic: the path the record already holds (a browser cannot copy a local file to
        // the desktop's "Attachment Folder Path" share), "" for a new employee as the form has before a file is chosen.
        m.PictureFilePath = oldPicture;
        m.SignPictureFilePath = oldSign;
        m.RelationMobile = str(b.relationMobile);
        m.RelationName = trim(b.relationName);
        m.Signature = "";
        m.SignPictureString = "";
        m.SubTitleId = b.relationTitleId;
        m.TitleId = b.titleId;
        m.UserLogId = userId;
        m.Active = b.active;

        EmployeeSave s = new EmployeeSave();
        s.employee = m;

        GenEmployeeHistory h = new GenEmployeeHistory();
        h.FromDate = dateOr(b.startDate, today);
        h.ToDate = dateOr(b.endDate, today);
        h.ProbationEnding = dateOr(b.probationEndDate, today);
        h.DesignationId = b.designationId;
        h.ShiftId = b.shiftId;
        h.DepartmentId = b.departmentId;
        h.PayableAcId = b.payableAcId;
        h.ExpenseAccountId = b.expenseAccountId;
        h.LoanAcId = b.loanAcId;
        h.LocationId = b.locationId;
        h.SectionId = b.sectionId;
        h.EmployeeTypeProfileId = b.employeeTypeId;
        h.PartyLocationId = b.reportedBranchId;
        h.ReportToEmployeeId = b.reportedPersonId;
        h.StoreId = b.storeId;
        h.TotalSalary = toDec(b.totalSalary);
        h.Active = b.active;
        h.ActionTypeId = 1;
        h.AlteredById = userId;
        h.AlteredOn = now;
        h.CompanyId = comp;
        h.OrganizationId = org;
        h.BranchId = toInt(u.getBranchesId());
        h.CreatedById = userId;
        h.CreatedOn = now;
        h.EmployeeHistoryLineId = 1;
        h.WithEffectFrom = now;
        h.EmployeeGroupId = b.employeeGroupId;
        s.histories = new ArrayList<>();
        s.histories.add(h);

        s.salaries = new ArrayList<>();
        for (EmployeeSalaryRowDto r : b.salaries) {
            EmployeeSalary x = new EmployeeSalary();
            x.SalaryTypeProfileId = r.salaryTypeId;
            x.SalaryTypePercent = toDec(r.typePercent);
            x.Amount = toDec(r.amount);
            x.ActionTypeId = 1;
            x.AlteredById = userId;
            x.AlteredOn = now;
            x.CompanyId = comp;
            x.CreatedById = userId;
            x.CreatedOn = now;
            x.OrganizationId = org;
            x.UserLogId = userId;
            s.salaries.add(x);
        }

        s.benefits = new ArrayList<>();
        for (List<EmployeeBenefitRowDto> list : java.util.Arrays.asList(b.directBenefits, b.assetBenefits)) {
            if (list == null) continue;
            for (EmployeeBenefitRowDto r : list) {
                if (!r.select) continue;
                HrmEmployeeBenefit x = new HrmEmployeeBenefit();
                x.BenefitId = r.benefitId;
                x.Description = trim(r.benefitValue);
                x.ApprovalEmployeeId = r.approvalPersonId;
                x.ActionTypeId = 1;
                x.AlteredById = userId;
                x.AlteredOn = now;
                x.CompanyId = comp;
                x.OrganizationId = org;
                x.CreatedById = userId;
                x.CreatedOn = now;
                x.UserLogId = userId;
                s.benefits.add(x);
            }
        }

        s.weekDays = new ArrayList<>();
        if (b.weekDays != null) for (EmployeeWeekDayRowDto r : b.weekDays) {
            if (!r.select) continue;
            GenEmployeeWeekDay x = new GenEmployeeWeekDay();
            x.WeekDayProfileId = r.weekDaysProfileId;
            x.WeekDayName = str(r.weekDays);
            x.ActionTypeId = 1;
            x.AlteredById = userId;
            x.AlteredOn = now;
            x.OrganizationId = org;
            x.CompanyId = comp;
            x.CreatedById = userId;
            x.CreatedOn = now;
            x.LineId = r.rowIndex;                      // GridEXRow.RowIndex
            s.weekDays.add(x);
        }

        s.addresses = new ArrayList<>();
        if (b.addresses != null) for (EmployeeAddressRowDto r : b.addresses) {
            TblEmployeeAddressDetail x = new TblEmployeeAddressDetail();
            x.AddressTypeId = r.addressTypeId;
            x.CountryId = r.countryId;
            x.ProvinceId = r.provinceId;
            x.CityId = r.cityId;
            x.AddressDetail = str(r.addressDetail);
            x.CompanyId = comp;
            x.OrganizationId = org;
            s.addresses.add(x);
        }

        s.experiences = new ArrayList<>();
        if (b.experiences != null) for (EmployeeExperienceRowDto r : b.experiences) {
            HrmEmployeeExperience x = new HrmEmployeeExperience();
            x.DesignationId = r.designationId;
            x.OrginizationName = str(r.organization);
            x.CityId = r.cityId;
            x.FromDate = dateOr(r.fromDate, LocalDateTime.of(1900, 1, 1, 0, 0));     // Conversion.ToDateTime("") = 1900-01-01
            x.ToDate = dateOr(r.toDate, LocalDateTime.of(1900, 1, 1, 0, 0));
            x.Description = str(r.description);
            x.ActionTypeId = 1;
            x.AlteredById = userId;
            x.AlteredOn = now;
            x.CompanyId = comp;
            x.CreatedById = userId;
            x.CreatedOn = now;
            x.EmployeeExperienceLineId = r.rowIndex;
            x.OrganizationId = org;
            s.experiences.add(x);
        }

        s.educations = new ArrayList<>();
        if (b.educations != null) for (EmployeeEducationRowDto r : b.educations) {
            GenEmployeeEducation x = new GenEmployeeEducation();
            x.DegreeId = r.degreeId;
            x.DegreeTitle = str(r.degreeTitle);
            x.Institute = str(r.institute);
            x.CGPA = toDouble(r.cgpa);
            x.CityId = r.cityId;
            x.StartYear = dateOr(r.startYear, LocalDateTime.of(1900, 1, 1, 0, 0));
            x.PassingYear = dateOr(r.passingYear, LocalDateTime.of(1900, 1, 1, 0, 0));
            x.EntryDate = now;
            x.PostDate = now;
            x.ModifyDate = now;
            x.PostState = false;
            x.PostUser = userId;
            x.EntryUser = userId;
            x.ModifyUser = userId;
            x.CompanyId = comp;
            x.OrganizationId = org;
            s.educations.add(x);
        }

        s.families = new ArrayList<>();
        if (b.families != null) for (EmployeeFamilyRowDto r : b.families) {
            HrmEmployeeFamilyInfo x = new HrmEmployeeFamilyInfo();
            x.RelationshipProfileId = r.relationShipId;
            x.Name = str(r.relationName);
            x.Description = str(r.description);
            x.AlteredOn = now;
            x.AlteredById = userId;
            x.ActionTypeId = 1;
            x.CompanyId = comp;
            x.OrganizationId = org;
            x.CreatedById = userId;
            x.CreatedOn = now;
            x.EmployeeFamilyInfoLineId = r.rowIndex;
            x.UserLogId = userId;
            x.BranchId = 0;
            x.ProjectId = 0;
            s.families.add(x);
        }

        s.references = new ArrayList<>();
        if (b.references != null) for (EmployeeReferenceRowDto r : b.references) {
            HrmEmployeeReference x = new HrmEmployeeReference();
            x.Name = str(r.referenceName);
            x.OrginizationName = str(r.organizationName);
            x.DesignationId = r.designationId;
            x.Mobile = str(r.mobile);
            x.Email = str(r.email);
            x.Description = str(r.description);
            x.ActionTypeId = 1;
            x.AlteredById = userId;
            x.AlteredOn = now;
            x.CompanyId = comp;
            x.OrganizationId = org;
            x.CreatedById = userId;
            x.CreatedOn = now;
            x.UserLogId = userId;
            s.references.add(x);
        }

        s.banks = new ArrayList<>();
        if (b.banks != null) for (EmployeeBankRowDto r : b.banks) {
            HrmEmployeeBankAccount x = new HrmEmployeeBankAccount();
            x.BankAccountId = r.bankId;
            x.AccountTitle = str(r.accountTitle);
            x.AccountNo = str(r.accountNo);
            x.IsPayrollAccount = r.isPayRoll;
            x.ActionTypeId = 1;
            x.AlteredById = userId;
            x.AlteredOn = now;
            x.CompanyId = comp;
            x.OrganizationId = org;
            x.CreatedById = userId;
            x.CreatedOn = now;
            x.UserLogId = userId;
            s.banks.add(x);
        }

        int n = repo.saveEmployee(s);
        return saved(n, recId == 0 ? "Information Saved" : "Information Update");
    }

    /** The two benefit loops of btnsave_Click: only ticked rows, "PLease Check Benefit" / "Please Check Benefit Value" / "Please Check Approval Person". */
    private static void checkBenefits(List<EmployeeBenefitRowDto> rows) {
        if (rows == null) return;
        for (EmployeeBenefitRowDto r : rows) {
            if (!r.select) continue;
            if (r.benefitId == 0) throw invalid("PLease Check Benefit");
            if (trim(r.benefitValue).isEmpty()) throw invalid("Please Check Benefit Value");
            if (r.approvalPersonId == 0) throw invalid("Please Check Approval Person");
        }
    }

    /** Conversion.ToDateTime of a DateTimePicker value: the page's date, or today when the page sent none. */
    private static LocalDateTime dateOr(String v, LocalDateTime fallback) {
        LocalDateTime d = toDay(v);
        return d == null ? fallback : d;
    }

    /** Tenancy guard: every non-zero id the page picked must be a row of this company's own list. */
    private void checkIds(UserAccount u, EmployeeRegistrationDto b) {
        Set<Integer> profiles = ids(repo.profiles(u, 0), "ProfileId");
        need(profiles, b.employeeTypeId, "Employee Type");
        need(profiles, b.titleId, "Employee Title");
        need(profiles, b.relationTitleId, "Relation Title");
        need(profiles, b.genderId, "Gender");
        need(profiles, b.nationalityId, "Nationality");
        need(profiles, b.bloodGroupId, "Blood Group");
        need(profiles, b.reportedBranchId, "Reported Branch");
        need(profiles, b.reportedPersonId, "Reported Person");
        need(profiles, b.storeId, "Store");
        need(ids(repo.employeeCategories(u), "EmployeeCategoryId"), b.employeeCategoryId, "Employee Catagory");
        need(ids(repo.employeeGroups(u), "EmployeeGroupId"), b.employeeGroupId, "Employee Group");
        need(ids(repo.departments(u), "DepartmentId"), b.departmentId, "Department");
        need(ids(repo.locations(u), "LocationId"), b.locationId, "Location");
        need(ids(repo.sections(u), "SectionId"), b.sectionId, "Section");
        Set<Integer> designations = ids(repo.designations(u), "DesignationId");
        need(designations, b.designationId, "Designation");
        need(ids(repo.shifts(u), "ShiftId"), b.shiftId, "Shift");
        need(ids(repo.projects(u), "Id"), b.projectId, "Project");
        need(ids(repo.branches(u), "Id"), b.branchesId, "Branch");
        need(ids(repo.accountsByType(u, APP_HRM, "11,12,20,21"), "Id"), b.expenseAccountId, "Expense Account");
        Set<Integer> payables = ids(repo.accountsByType(u, APP_HRM, "5,8"), "Id");
        need(payables, b.payableAcId, "Payable Account");
        need(payables, b.loanAcId, "Loan Account");
        for (EmployeeSalaryRowDto r : b.salaries) need(profiles, r.salaryTypeId, "Salary Type");
        for (List<EmployeeBenefitRowDto> list : java.util.Arrays.asList(b.directBenefits, b.assetBenefits)) {
            if (list == null) continue;
            Set<Integer> ben = ids(repo.benefits(u), "BenefitId");
            for (EmployeeBenefitRowDto r : list) if (r.select) { need(ben, r.benefitId, "Benefit"); need(profiles, r.approvalPersonId, "Approval Person"); }
        }
        if (b.weekDays != null) for (EmployeeWeekDayRowDto r : b.weekDays) if (r.select) need(profiles, r.weekDaysProfileId, "Week Day");
        Set<Integer> cities = null;
        if (b.addresses != null && !b.addresses.isEmpty()) {
            Set<Integer> countries = ids(repo.countries(), "Id"), provinces = ids(repo.provinces(u), "Id");
            cities = ids(repo.cities(u), "Id");
            for (EmployeeAddressRowDto r : b.addresses) {
                need(profiles, r.addressTypeId, "Address Type");
                need(countries, r.countryId, "Country");
                need(provinces, r.provinceId, "Province");
                need(cities, r.cityId, "City");
            }
        }
        if ((b.experiences != null && !b.experiences.isEmpty()) || (b.educations != null && !b.educations.isEmpty())) {
            if (cities == null) cities = ids(repo.cities(u), "Id");
            if (b.experiences != null) for (EmployeeExperienceRowDto r : b.experiences) { need(designations, r.designationId, "Designation"); need(cities, r.cityId, "City"); }
            if (b.educations != null) for (EmployeeEducationRowDto r : b.educations) { need(profiles, r.degreeId, "Degree"); need(cities, r.cityId, "City"); }
        }
        if (b.families != null) for (EmployeeFamilyRowDto r : b.families) need(profiles, r.relationShipId, "Relationship");
        if (b.references != null) for (EmployeeReferenceRowDto r : b.references) need(designations, r.designationId, "Designation");
        if (b.banks != null && !b.banks.isEmpty()) {
            Set<Integer> banks = ids(repo.banks(u), "Id");
            for (EmployeeBankRowDto r : b.banks) need(banks, r.bankId, "Bank");
        }
    }

    private static Set<Integer> ids(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(r.get(key)));
        return s;
    }

    private static void need(Set<Integer> ids, int id, String what) {
        if (id != 0 && !ids.contains(id)) throw invalid(what + " is not in this company's list.");
    }
}
