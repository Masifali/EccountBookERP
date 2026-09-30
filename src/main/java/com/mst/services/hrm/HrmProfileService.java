package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.HrmNameDto;
import com.mst.models.hrm.dto.ProfileBenefitDto;
import com.mst.models.hrm.dto.ProfileCodeNameDto;
import com.mst.models.hrm.dto.ProfileDefineDto;
import com.mst.models.hrm.dto.ProfileDepartmentDto;
import com.mst.models.hrm.dto.ProfileLocationDto;
import com.mst.models.hrm.dto.ProfileSectionDto;
import com.mst.models.hrm.profile.GenDepartment;
import com.mst.models.hrm.profile.GenDesignation;
import com.mst.models.hrm.profile.GenEmployeeCategory;
import com.mst.models.hrm.profile.GenEmployeeGroup;
import com.mst.models.hrm.profile.GenLocation;
import com.mst.models.hrm.profile.GenProfile;
import com.mst.models.hrm.profile.GenProfileType;
import com.mst.models.hrm.profile.GenSection;
import com.mst.models.hrm.profile.HrmBenefit;
import com.mst.repositories.hrm.HrmProfileRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Profile Management" screens (AppModules 2018). Each method names the desktop form
 * method it reproduces; validation wording/order and the messages are the form's.
 */
@Service
public class HrmProfileService {

    public static final int SCREEN_EMPLOYEE_GROUP = 634;
    public static final int SCREEN_EMPLOYEE_CATEGORY = 635;
    public static final int SCREEN_BENEFIT = 636;
    public static final int SCREEN_DESIGNATION = 637;
    public static final int SCREEN_DEPARTMENT = 638;
    public static final int SCREEN_SECTION = 639;
    public static final int SCREEN_LOCATION = 640;
    public static final int SCREEN_PROFILE_TYPE = 641;
    public static final int SCREEN_DEFINE_PROFILE = 642;
    /** dbo.ScreenDefinition 9 "AcfrmDefCoa" - genDepartment.btnCoa1_Click checks its View right. */
    public static final int SCREEN_DEFINE_ACCOUNTS = 9;

    /** genProfileType ids the forms hard-code: BenifitDefine 1, frmgenLocation 61, genDepartment 65. */
    public static final int PROFILE_TYPE_BENEFIT = 1;
    public static final int PROFILE_TYPE_LOCATION = 61;
    public static final int PROFILE_TYPE_DEPARTMENT = 65;
    /** genDepartment.AccountBinds account type lists. */
    public static final String ACC_TYPES_EXPENSE = "11,12,20,21";
    public static final String ACC_TYPES_PAYABLE = "5,8";

    @Autowired private HrmProfileRepository repo;
    @Autowired private HrmSupport hrm;

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... pairs) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Object[] kv = new Object[pairs.length];
            for (int i = 0; i + 1 < pairs.length; i += 2) { kv[i] = pairs[i]; kv[i + 1] = r.get(pairs[i + 1]); }
            out.add(map(kv));
        }
        return out;
    }

    // ================================================================== 637 Designation (genDesignation.cs)

    /** genDesignation_Load / GridFill: DataTable { Id = DesignationId, Name = DesignationName }. */
    public Map<String, Object> designationSetup() {
        UserAccount u = hrm.user(SCREEN_DESIGNATION);
        return map("rights", hrm.rights(u, SCREEN_DESIGNATION), "rows", designationGrid(u));
    }

    public List<Map<String, Object>> designations() { return designationGrid(hrm.user(SCREEN_DESIGNATION)); }

    private List<Map<String, Object>> designationGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.designations(u)) out.add(map("Id", r.get("DesignationId"), "Name", r.get("DesignationName")));
        return out;
    }

    /** grd_DoubleClick -> RetrivedData -> genDesignation.GetByID (only an id of this company's list). */
    public Map<String, Object> designation(int id) {
        UserAccount u = hrm.user(SCREEN_DESIGNATION);
        if (!owns(repo.designations(u), "DesignationId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.designation(id));
        return map("Id", r.get("DesignationId"), "DesignationName", r.get("DesignationName"));
    }

    /**
     * Insert(): formvalidation ("Please Insert Designation Name"), then the model exactly as the form
     * fills it - ActionTypeId 1/2, AlteredById = CreatedById = UserLogId = user, AlteredOn = CreatedOn = now,
     * Company/Organization from the session - and genDesignation.Save (Insert when DesignationId == 0).
     */
    public Map<String, Object> saveDesignation(HrmNameDto b) {
        UserAccount u = hrm.user(SCREEN_DESIGNATION);
        String name = trim(b.name);
        if (name.isEmpty()) throw invalid("Please Insert Designation Name");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.designations(u), "DesignationId", id)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        GenDesignation m = new GenDesignation();
        m.DesignationId = id;
        m.DesignationName = name;
        m.ActionTypeId = id > 0 ? 2 : 1;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.UserLogId = u.getId();
        int n = repo.saveDesignation(m);
        if (n <= 0) throw invalid("Record not saved.");
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 634 Employee Group (genEmployeeGroup.cs)

    /** genDepartment_Load (the form's Load handler name) -> GridFill: { Id, ShortName, Name }. */
    public Map<String, Object> employeeGroupSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_GROUP);
        return map("rights", hrm.rights(u, SCREEN_EMPLOYEE_GROUP), "rows", employeeGroupGrid(u));
    }

    public List<Map<String, Object>> employeeGroups() { return employeeGroupGrid(hrm.user(SCREEN_EMPLOYEE_GROUP)); }

    private List<Map<String, Object>> employeeGroupGrid(UserAccount u) {
        return pick(repo.employeeGroups(u), "Id", "EmployeeGroupId", "ShortName", "EmployeeGroupPrefix", "Name", "EmployeeGroupName");
    }

    /** grd_DoubleClick -> RetrivedData -> genEmployeeGroup.GetByID. */
    public Map<String, Object> employeeGroup(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_GROUP);
        if (!owns(repo.employeeGroups(u), "EmployeeGroupId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.employeeGroup(id));
        return map("Id", r.get("EmployeeGroupId"), "EmployeeGroupPrefix", r.get("EmployeeGroupPrefix"), "EmployeeGroupName", r.get("EmployeeGroupName"));
    }

    /**
     * Insert(): formvalidation ("Please Insert Pre Name", "Please Insert Group Name"); for a new record the
     * grid check - a row whose Name equals txtGroupName.Text -> "This Already already Exist"; then the model
     * as the form fills it and genEmployeeGroup.Save.
     */
    public Map<String, Object> saveEmployeeGroup(ProfileCodeNameDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_GROUP);
        if (trim(b.shortName).isEmpty()) throw invalid("Please Insert Pre Name");
        if (trim(b.name).isEmpty()) throw invalid("Please Insert Group Name");
        int id = Math.max(0, b.id);
        List<Map<String, Object>> list = repo.employeeGroups(u);
        if (id > 0) {
            if (!owns(list, "EmployeeGroupId", id)) throw invalid("Record not found.");
        } else {
            for (Map<String, Object> r : list) if (str(r.get("EmployeeGroupName")).equals(str(b.name))) throw invalid("This Already already Exist");
        }
        LocalDateTime now = LocalDateTime.now();
        GenEmployeeGroup m = new GenEmployeeGroup();
        m.EmployeeGroupId = id;
        m.EmployeeGroupPrefix = trim(b.shortName);
        m.EmployeeGroupName = trim(b.name);
        m.ActionTypeId = id > 0 ? 2 : 1;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.OrginizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.UserLogId = u.getId();
        int n = repo.saveEmployeeGroup(m);
        return saved(n, n > 0 ? (id > 0 ? "Update Successfully" : "Save Successfully") : null);
    }

    // ================================================================== 635 Employee Category (frmEmployeeCategory.cs)

    /** genDepartment_Load -> GridFill: { Id, CategoryName, ShortName }. */
    public Map<String, Object> employeeCategorySetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_CATEGORY);
        return map("rights", hrm.rights(u, SCREEN_EMPLOYEE_CATEGORY), "rows", employeeCategoryGrid(u));
    }

    public List<Map<String, Object>> employeeCategories() { return employeeCategoryGrid(hrm.user(SCREEN_EMPLOYEE_CATEGORY)); }

    private List<Map<String, Object>> employeeCategoryGrid(UserAccount u) {
        return pick(repo.employeeCategories(u), "Id", "EmployeeCategoryId", "CategoryName", "EmployeeCategoryName", "ShortName", "EmployeeCategoryPrefix");
    }

    /** grd_DoubleClick -> RetrivedData -> genEmployeeCategory.GetByID. */
    public Map<String, Object> employeeCategory(int id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_CATEGORY);
        if (!owns(repo.employeeCategories(u), "EmployeeCategoryId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.employeeCategory(id));
        return map("Id", r.get("EmployeeCategoryId"), "EmployeeCategoryPrefix", r.get("EmployeeCategoryPrefix"), "EmployeeCategoryName", r.get("EmployeeCategoryName"));
    }

    /** Insert(): formvalidation ("Please Insert Short Name", "Please Insert Category Name"), model, genEmployeeCategory.Save. */
    public Map<String, Object> saveEmployeeCategory(ProfileCodeNameDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_CATEGORY);
        if (trim(b.shortName).isEmpty()) throw invalid("Please Insert Short Name");
        if (trim(b.name).isEmpty()) throw invalid("Please Insert Category Name");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.employeeCategories(u), "EmployeeCategoryId", id)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        GenEmployeeCategory m = new GenEmployeeCategory();
        m.EmployeeCategoryId = id;
        m.EmployeeCategoryPrefix = trim(b.shortName);
        m.EmployeeCategoryName = trim(b.name);
        m.ActionTypeId = id > 0 ? 2 : 1;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.OrginizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.UserLogId = u.getId();
        int n = repo.saveEmployeeCategory(m);
        return saved(n, n > 0 ? (id > 0 ? "Update Successfully" : "Save Successfully") : null);
    }

    // ================================================================== 636 Benefit (BenifitDefine.cs)

    /** BenifitDefine_Load: gridfill() + ProfileType(). */
    public Map<String, Object> benefitSetup() {
        UserAccount u = hrm.user(SCREEN_BENEFIT);
        return map("rights", hrm.rights(u, SCREEN_BENEFIT), "rows", benefitGrid(u), "profiles", benefitProfiles(u));
    }

    public List<Map<String, Object>> benefits() { return benefitGrid(hrm.user(SCREEN_BENEFIT)); }

    /** btnRefresh_Click -> ProfileType(). */
    public Map<String, Object> benefitCombos() { return map("profiles", benefitProfiles(hrm.user(SCREEN_BENEFIT))); }

    /**
     * gridfill(): { Id, BenefitProfile, BenefitName, Prefix }. The desktop fills BenefitProfile with
     * BenefitName (copy slip in table.Rows.Add); the proc returns the profile's ProfileName, which is shown.
     */
    private List<Map<String, Object>> benefitGrid(UserAccount u) {
        return pick(repo.benefits(u), "Id", "BenefitId", "BenefitProfile", "ProfileName", "BenefitName", "BenefitName", "Prefix", "BenefitPrefix");
    }

    /** ProfileType(): genProfile.GetByProfileTypeId(ProfileTypeId 1) -> { Id = ProfileId, BenefitProfile = ProfileName }. */
    private List<Map<String, Object>> benefitProfiles(UserAccount u) {
        return pick(repo.profilesByType(u, PROFILE_TYPE_BENEFIT), "Id", "ProfileId", "BenefitProfile", "ProfileName");
    }

    /** grdfrm_DoubleClick -> hrmBenefit.GetByID. */
    public Map<String, Object> benefit(int id) {
        UserAccount u = hrm.user(SCREEN_BENEFIT);
        if (!owns(repo.benefits(u), "BenefitId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.benefit(id));
        return map("Id", r.get("BenefitId"), "BenefitTypeProfileId", r.get("BenefitTypeProfileId"), "BenefitName", r.get("BenefitName"), "BenefitPrefix", r.get("BenefitPrefix"));
    }

    /**
     * btnsave_Click / btnupdate_Click: formvalidation ("Please Select Benefit Profile", "Please Insert Benefit
     * Name", "Please Insert Prefix"), then hrmBenefit.Save with ActionTypeId 1 (save) / 2 (update, BenefitId = RecId).
     * The desktop shows "Save Successfully" / "Update Successfully" whatever Save returns.
     */
    public Map<String, Object> saveBenefit(ProfileBenefitDto b) {
        UserAccount u = hrm.user(SCREEN_BENEFIT);
        if (b.profileId <= 0 || !owns(repo.profilesByType(u, PROFILE_TYPE_BENEFIT), "ProfileId", b.profileId)) throw invalid("Please Select Benefit Profile");
        if (trim(b.name).isEmpty()) throw invalid("Please Insert Benefit Name");
        if (trim(b.prefix).isEmpty()) throw invalid("Please Insert Prefix");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.benefits(u), "BenefitId", id)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        HrmBenefit m = new HrmBenefit();
        m.BenefitId = id;
        m.BenefitTypeProfileId = b.profileId;
        m.BenefitName = trim(b.name);
        m.BenefitPrefix = trim(b.prefix);
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.AlteredOn = now;
        m.AlteredById = u.getId();
        m.CreatedOn = now;
        m.CreatedById = u.getId();
        m.ActionTypeId = id > 0 ? 2 : 1;
        m.UserLogId = u.getId();
        int n = repo.saveBenefit(m);
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 638 Department (genDepartment.cs)

    /** genDepartment_Load: AccountBinds(), GridFill(), DepartmentTypeFill(); plus the AcfrmDefCoa View right of btnCoa1_Click. */
    public Map<String, Object> departmentSetup() {
        UserAccount u = hrm.user(SCREEN_DEPARTMENT);
        Map<String, Object> d = departmentCombos(u);
        d.put("rights", hrm.rights(u, SCREEN_DEPARTMENT));
        d.put("rows", departmentGrid(u));
        d.put("canCoa", hrm.can(u, SCREEN_DEFINE_ACCOUNTS, "View"));
        return d;
    }

    public List<Map<String, Object>> departments() { return departmentGrid(hrm.user(SCREEN_DEPARTMENT)); }

    /** btnrefresh_Click: AccountBinds() + DepartmentTypeFill() (the page reloads the grid with /list). */
    public Map<String, Object> departmentCombos() { return departmentCombos(hrm.user(SCREEN_DEPARTMENT)); }

    private Map<String, Object> departmentCombos(UserAccount u) {
        return map(
                "types", pick(repo.profilesByType(u, PROFILE_TYPE_DEPARTMENT), "ProfileId", "ProfileId", "ProfileName", "ProfileName"),
                "expense", pick(repo.accountsByTypes(u, ACC_TYPES_EXPENSE), "Id", "Id", "AccountTitle", "AccountTitle"),
                "payable", pick(repo.accountsByTypes(u, ACC_TYPES_PAYABLE), "Id", "Id", "AccountTitle", "AccountTitle"));
    }

    /** GridFill(): { Id, Type, ShortName, Name, Payable Account, Expense Account, Loan Account }. */
    private List<Map<String, Object>> departmentGrid(UserAccount u) {
        return pick(repo.departments(u), "Id", "DepartmentId", "Type", "DepartmentType", "ShortName", "ShortName", "Name", "DepartmentName",
                "Payable Account", "PayableAccount", "Expense Account", "ExpensesAccount", "Loan Account", "LoanAccount");
    }

    /** grd_DoubleClick -> RetrivedData -> genDepartment.GetByID. */
    public Map<String, Object> department(int id) {
        UserAccount u = hrm.user(SCREEN_DEPARTMENT);
        if (!owns(repo.departments(u), "DepartmentId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.department(id));
        return map("Id", r.get("DepartmentId"), "DepartmentTypeProfileId", r.get("DepartmentTypeProfileId"), "ShortName", r.get("ShortName"),
                "DepartmentName", r.get("DepartmentName"), "LoanAcId", r.get("LoanAcId"), "PayableAcId", r.get("PayableAcId"), "ExpenseAccountId", r.get("ExpenseAccountId"));
    }

    /**
     * Insert(): formvalidation ("Please Select Department Type", "Please Insert Short Name", "Please Insert
     * Department Name"); the page asks "Are you sure to Save?" / "Are you sure to Update?" first. Model as the
     * form fills it: DepartmentCode = ShortName = txtShortName, BranchId 0, ActionTypeId 1/2; genDepartment.Save.
     * An account id must be one of its combo's list (0 = "...Select Any Value..." is sent as 0, as on the desktop).
     */
    public Map<String, Object> saveDepartment(ProfileDepartmentDto b) {
        UserAccount u = hrm.user(SCREEN_DEPARTMENT);
        if (b.typeId <= 0 || !owns(repo.profilesByType(u, PROFILE_TYPE_DEPARTMENT), "ProfileId", b.typeId)) throw invalid("Please Select Department Type");
        if (trim(b.shortName).isEmpty()) throw invalid("Please Insert Short Name");
        if (trim(b.name).isEmpty()) throw invalid("Please Insert Department Name");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.departments(u), "DepartmentId", id)) throw invalid("Record not found.");
        if (b.expenseAccountId != 0 && !owns(repo.accountsByTypes(u, ACC_TYPES_EXPENSE), "Id", b.expenseAccountId)) throw invalid("Record not found.");
        if (b.payableAcId != 0 || b.loanAcId != 0) {
            List<Map<String, Object>> pay = repo.accountsByTypes(u, ACC_TYPES_PAYABLE);
            if (b.payableAcId != 0 && !owns(pay, "Id", b.payableAcId)) throw invalid("Record not found.");
            if (b.loanAcId != 0 && !owns(pay, "Id", b.loanAcId)) throw invalid("Record not found.");
        }
        LocalDateTime now = LocalDateTime.now();
        GenDepartment m = new GenDepartment();
        m.DepartmentId = id;
        m.DepartmentTypeProfileId = b.typeId;
        m.ExpenseAccountId = b.expenseAccountId;
        m.PayableAcId = b.payableAcId;
        m.LoanAcId = b.loanAcId;
        m.ShortName = trim(b.shortName);
        m.DepartmentCode = trim(b.shortName);
        m.DepartmentName = trim(b.name);
        m.ActionTypeId = id > 0 ? 2 : 1;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.BranchId = 0;
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.UserLogId = u.getId();
        int n = repo.saveDepartment(m);
        return saved(n, n > 0 ? (id > 0 ? "Update Successfully" : "Save Successfully") : null);
    }

    // ================================================================== 639 Section (DefineSection.cs)

    /** BenifitDefine_Load (the form's Load handler name): gridfill() + DepartmentFill(). */
    public Map<String, Object> sectionSetup() {
        UserAccount u = hrm.user(SCREEN_SECTION);
        return map("rights", hrm.rights(u, SCREEN_SECTION), "rows", sectionGrid(u),
                "departments", pick(repo.departments(u), "Id", "DepartmentId", "DepartmentName", "DepartmentName"));
    }

    public List<Map<String, Object>> sections() { return sectionGrid(hrm.user(SCREEN_SECTION)); }

    /** gridfill(): { Id, DepartmentName, ShortName, SectionName, Code }. */
    private List<Map<String, Object>> sectionGrid(UserAccount u) {
        return pick(repo.sections(u), "Id", "SectionId", "DepartmentName", "DepartmentName", "ShortName", "ShortName", "SectionName", "SectionName", "Code", "SectionCode");
    }

    /** grdfrm_DoubleClick -> genSection.GetByID. */
    public Map<String, Object> section(int id) {
        UserAccount u = hrm.user(SCREEN_SECTION);
        if (!owns(repo.sections(u), "SectionId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.section(id));
        return map("Id", r.get("SectionId"), "DepartmentId", r.get("DepartmentId"), "ShortName", r.get("ShortName"), "SectionName", r.get("SectionName"), "SectionCode", r.get("SectionCode"));
    }

    /**
     * btnsave_Click / btnupdate_Click: formvalidation - the desktop's own (copied) texts "Please Select Benefit
     * Profile", "Please Insert Benefit Name", "Please Insert Prefix" - then genSection.Save with only the fields
     * the form sets (no dates, ActionTypeId 0, SectionSeqNo 0). The message shows when Save returns > 0.
     */
    public Map<String, Object> saveSection(ProfileSectionDto b) {
        UserAccount u = hrm.user(SCREEN_SECTION);
        if (b.departmentId <= 0 || !owns(repo.departments(u), "DepartmentId", b.departmentId)) throw invalid("Please Select Benefit Profile");
        if (trim(b.shortName).isEmpty()) throw invalid("Please Insert Benefit Name");
        if (trim(b.name).isEmpty()) throw invalid("Please Insert Prefix");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.sections(u), "SectionId", id)) throw invalid("Record not found.");
        GenSection m = new GenSection();
        m.SectionId = id;
        m.DepartmentId = b.departmentId;
        m.ShortName = trim(b.shortName);
        m.SectionName = trim(b.name);
        m.SectionCode = trim(b.code);
        m.AlteredById = u.getId();
        m.CreatedById = u.getId();
        m.UserLogId = u.getId();
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        int n = repo.saveSection(m);
        return saved(n, n > 0 ? (id > 0 ? "Update Successfully" : "Save Successfully") : null);
    }

    // ================================================================== 640 Location (frmgenLocation.cs)

    /** LeaveQuotaPolicy_Load (the form's Load handler name): LocationTypeFill() + gridFill(). */
    public Map<String, Object> locationSetup() {
        UserAccount u = hrm.user(SCREEN_LOCATION);
        return map("rights", hrm.rights(u, SCREEN_LOCATION), "rows", locationGrid(u), "types", locationTypes(u));
    }

    public List<Map<String, Object>> locations() { return locationGrid(hrm.user(SCREEN_LOCATION)); }

    /** btnrefresh_Click -> LocationTypeFill(). */
    public Map<String, Object> locationCombos() { return map("types", locationTypes(hrm.user(SCREEN_LOCATION))); }

    /** LocationTypeFill(): genProfile.GetByProfileTypeId(ProfileTypeId 61) -> ProfileId / ProfileName. */
    private List<Map<String, Object>> locationTypes(UserAccount u) {
        return pick(repo.profilesByType(u, PROFILE_TYPE_LOCATION), "ProfileId", "ProfileId", "ProfileName", "ProfileName");
    }

    /** gridFill(): { Id, LocationType, LocationName, ShortName, SubName, Mobile1, Mobile2, Email, PortalURL, AddressDetail, LicenseNo }. */
    private List<Map<String, Object>> locationGrid(UserAccount u) {
        return pick(repo.locations(u), "Id", "LocationId", "LocationType", "LocationType", "LocationName", "LocationName", "ShortName", "LocationShortName",
                "SubName", "SubLocationName", "Mobile1", "Mobile1", "Mobile2", "Mobile2", "Email", "Email", "PortalURL", "PortalURL",
                "AddressDetail", "HeaderDetail", "LicenseNo", "LicenseNo");
    }

    /** grd_DoubleClick -> RetrivedData -> genLocation.GetByID. */
    public Map<String, Object> location(int id) {
        UserAccount u = hrm.user(SCREEN_LOCATION);
        if (!owns(repo.locations(u), "LocationId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.location(id));
        return map("Id", r.get("LocationId"), "LocationName", r.get("LocationName"), "LocationShortName", r.get("LocationShortName"),
                "SubLocationName", r.get("SubLocationName"), "Email", r.get("Email"), "LocationTypeProfileId", r.get("LocationTypeProfileId"),
                "Mobile1", r.get("Mobile1"), "Mobile2", r.get("Mobile2"), "PortalURL", r.get("PortalURL"), "LicenseNo", r.get("LicenseNo"),
                "HeaderDetail", r.get("HeaderDetail"));
    }

    /**
     * btnsave_Click / btnupdate_Click: formValidation ("Location Name Required" when the text is "" or "0",
     * untrimmed; "Location Type"), the page's "Are you sure to Save?/Update?" and genLocation.Save with the fields
     * the form sets: CreatedOn = now, AlteredOn = Today (date only), everything else left at its default.
     */
    public Map<String, Object> saveLocation(ProfileLocationDto b) {
        UserAccount u = hrm.user(SCREEN_LOCATION);
        String raw = str(b.name);
        if (raw.isEmpty() || raw.equals("0")) throw invalid("Location Name Required");
        if (b.typeId <= 0 || !owns(repo.profilesByType(u, PROFILE_TYPE_LOCATION), "ProfileId", b.typeId)) throw invalid("Location Type");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.locations(u), "LocationId", id)) throw invalid("Record not found.");
        GenLocation m = new GenLocation();
        m.LocationId = id;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.CreatedOn = LocalDateTime.now();
        m.CreatedById = u.getId();
        m.AlteredById = u.getId();
        m.AlteredOn = LocalDate.now().atStartOfDay();
        m.LocationName = trim(b.name);
        m.LocationShortName = trim(b.shortName);
        m.SubLocationName = trim(b.subName);
        m.Email = trim(b.email);
        m.Mobile1 = trim(b.mobile1);
        m.Mobile2 = trim(b.mobile2);
        m.PortalURL = trim(b.portalUrl);
        m.LicenseNo = trim(b.licenseNo);
        m.HeaderDetail = trim(b.addressDetail);
        m.LocationTypeProfileId = b.typeId;
        int n = repo.saveLocation(m);
        return saved(n, n > 0 ? (id > 0 ? "Update Successfully" : "Save Successfully") : null);
    }

    // ================================================================== 641 Profile Type (ProfileTypes.cs)

    /** ProfileTypes_Load -> gridfill(): { Id, ProfileType, Prefix }. */
    public Map<String, Object> profileTypeSetup() {
        UserAccount u = hrm.user(SCREEN_PROFILE_TYPE);
        return map("rights", hrm.rights(u, SCREEN_PROFILE_TYPE), "rows", profileTypeGrid(u));
    }

    public List<Map<String, Object>> profileTypes() { return profileTypeGrid(hrm.user(SCREEN_PROFILE_TYPE)); }

    private List<Map<String, Object>> profileTypeGrid(UserAccount u) {
        return pick(repo.profileTypes(u), "Id", "ProfileTypeId", "ProfileType", "ProfileTypeName", "Prefix", "Prefix");
    }

    /** grdfrm_DoubleClick -> genProfileType.GetByID. */
    public Map<String, Object> profileType(int id) {
        UserAccount u = hrm.user(SCREEN_PROFILE_TYPE);
        if (!owns(repo.profileTypes(u), "ProfileTypeId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.profileType(id));
        return map("Id", r.get("ProfileTypeId"), "Prefix", r.get("Prefix"), "ProfileTypeName", r.get("ProfileTypeName"));
    }

    /**
     * btnsave_Click / btnupdate_Click: formvalidation ("Please Insert ProfileName", "Please Insert Prefix"), then
     * genProfileType.Save with Prefix, ProfileTypeName, Organization, Company (ProfileTypeId = RecId on update).
     * NOTE: both procs RAISERROR in the 23-Sep-2026 DB; the message reaches the page as the desktop shows it.
     */
    public Map<String, Object> saveProfileType(ProfileCodeNameDto b) {
        UserAccount u = hrm.user(SCREEN_PROFILE_TYPE);
        if (trim(b.name).isEmpty()) throw invalid("Please Insert ProfileName");
        if (trim(b.shortName).isEmpty()) throw invalid("Please Insert Prefix");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.profileTypes(u), "ProfileTypeId", id)) throw invalid("Record not found.");
        GenProfileType m = new GenProfileType();
        m.ProfileTypeId = id;
        m.Prefix = trim(b.shortName);
        m.ProfileTypeName = trim(b.name);
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        int n = repo.saveProfileType(m);
        return saved(n, n > 0 ? (id > 0 ? "Update Successfully" : "Save Successfully") : null);
    }

    // ================================================================== 642 Define Profile (ProfileDefine.cs)

    /** ProfileDefine_Load: ProfileType() + gridfill(). */
    public Map<String, Object> profileSetup() {
        UserAccount u = hrm.user(SCREEN_DEFINE_PROFILE);
        return map("rights", hrm.rights(u, SCREEN_DEFINE_PROFILE), "rows", profileGrid(u),
                "types", pick(repo.profileTypes(u), "ProfileTypeId", "ProfileTypeId", "ProfileTypeName", "ProfileTypeName"));
    }

    public List<Map<String, Object>> profiles() { return profileGrid(hrm.user(SCREEN_DEFINE_PROFILE)); }

    /** gridfill(): dtPrfile -> { Id, ProfileType, Profile, Prefix, IsActive }. */
    private List<Map<String, Object>> profileGrid(UserAccount u) {
        return pick(repo.profiles(u), "Id", "ProfileId", "ProfileType", "ProfileTypeName", "Profile", "ProfileName", "Prefix", "ProfilePrefix", "IsActive", "IsActive");
    }

    /** grdfrm_DoubleClick -> genProfile.GetByID. */
    public Map<String, Object> profile(int id) {
        UserAccount u = hrm.user(SCREEN_DEFINE_PROFILE);
        if (!owns(repo.profiles(u), "ProfileId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.profile(id));
        return map("Id", r.get("ProfileId"), "ProfileTypeId", r.get("ProfileTypeId"), "ProfilePrefix", r.get("ProfilePrefix"),
                "ProfileName", r.get("ProfileName"), "IsActive", toBool(r.get("IsActive")));
    }

    /**
     * btnsave_Click / btnupdate_Click: formvalidation ("Please Select Profil Type", "Please Insert Profile Name",
     * "Please Insert Prefix"), then genProfile.Save with the model the form builds (ProfileCode "", ProfileUrduName "",
     * ProfileSeqNo 0, ActionTypeId 0, IsActive = chkactive). The desktop shows the message whatever Save returns.
     * NOTE: both procs RAISERROR in the 23-Sep-2026 DB.
     */
    public Map<String, Object> saveProfile(ProfileDefineDto b) {
        UserAccount u = hrm.user(SCREEN_DEFINE_PROFILE);
        if (b.profileTypeId <= 0 || !owns(repo.profileTypes(u), "ProfileTypeId", b.profileTypeId)) throw invalid("Please Select Profil Type");
        if (trim(b.name).isEmpty()) throw invalid("Please Insert Profile Name");
        if (trim(b.prefix).isEmpty()) throw invalid("Please Insert Prefix");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.profiles(u), "ProfileId", id)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        GenProfile m = new GenProfile();
        m.ProfileId = id;
        m.ProfileTypeId = b.profileTypeId;
        m.ProfileName = trim(b.name);
        m.ProfilePrefix = trim(b.prefix);
        m.IsActive = b.isActive;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.ModifyDate = now;
        m.ModifyUser = u.getId();
        m.EntryDate = now;
        m.EntryUser = u.getId();
        m.ProfileCode = "";
        m.ProfileUrduName = "";
        m.ProfileSeqNo = 0;
        m.ActionTypeId = 0;
        m.UserLogId = u.getId();
        int n = repo.saveProfile(m);
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }
}
