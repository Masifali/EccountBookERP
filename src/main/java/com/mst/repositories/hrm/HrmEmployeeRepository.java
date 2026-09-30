package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
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
import com.mst.models.hrm.employee.USERINFO;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Employee Management" screen 648 Employee Registration (frmEmployeeRegistration.cs).
 * Every call is the desktop BLL/DAL's own procedure with the parameters its BLL sends
 * (recovered_source/projects/architecture.bll|dal, procdure.utf8.sql 23-Sep-2026):
 *
 *   BLL 0198 / DAL 0166 genEmployee          Sp_genEmployee_GetAllMethod (GenerateEmployeeNo | ReadAll | ReadById + 10 child
 *                                            activities | GetEmployeeIdByEmployeeNo | GetCaoByDepartmentId),
 *                                            Sp_genEmployee_Insert | Sp_genEmployee_Update, Sp_UserInfo_GetAllMethod,
 *                                            Sp_USERINFO_Insert, Sp_genEmployeeHistory_Insert, hrm.Sp_EmployeeSalary_Insert,
 *                                            Sp_hrmEmployeeBenefit_Insert, Sp_genEmployeeWeekDay_Insert,
 *                                            Sp_tblEmployeeAddressDetail_Insert, Sp_hrmEmployeeExperience_Insert,
 *                                            Sp_GenEmployeeEducation_Insert, Sp_hrmEmployeeFamilyInfo_Insert,
 *                                            Sp_hrmEmployeeReference_Insert, Sp_hrmEmployeeBankAccount_Insert
 *   the form's lookups                       Sp_genProfile_GetAllMethod ReadByProfileTypeId, Sp_genEmployeeGroup / Category /
 *                                            Department / Location / Section / Designation / Shift _GetAllMethod ReadAll,
 *                                            SP_Country_ReadMethod, SP_City_GetAllMethod, Proc_StateProvince_GetAllMethod,
 *                                            Sp_Bank_GetAllMethod ReadAllBankDT, Sp_hrmBenefit_GetAllMethod ReadAll,
 *                                            Sp_hrmSalaryBreakupPolicy_GetAllMethod ReadByLocationId,
 *                                            Sp_Projects_GetAllMethod, Sp_Branches_GetAllMethod,
 *                                            Sp_COAAllocation_GetAllMethod GetAccountTitleByAccountTypeIds,
 *                                            USP_FinancialYear_ReadById (clsGlobalVariables.ActiveYr)
 *   HRM_Reports 0210                         Sp_genEmployee_SlipandRegister (1112 / 1111 prints)
 */
@Repository
public class HrmEmployeeRepository {

    private static final String EMP = "Sp_genEmployee_GetAllMethod";

    private final HrmProcRepository db;

    public HrmEmployeeRepository(HrmProcRepository db) { this.db = db; }

    // ================================================================== lookups (frmEmployeeRegistration_Load)

    /**
     * AllLookUpFill / grdWeekDaysFill: genProfile.GetByProfileTypeId - @OrganizationId, @CompanyId,
     * @ProfileTypeId only when != 0 (AllLookUpFill leaves it 0 -> every profile of the company), @Activity 'ReadByProfileTypeId'.
     */
    public List<Map<String, Object>> profiles(UserAccount u, int profileTypeId) {
        return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ProfileTypeId", profileTypeId != 0 ? profileTypeId : null, "Activity", "ReadByProfileTypeId");
    }

    /** EmployeeGroupComboFill: genEmployeeGroup.Getall - @OrganizationId (OrginizationId), @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeGroups(UserAccount u) {
        return db.rows("Sp_genEmployeeGroup_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** EmployeeCatagoryComboFill: genEmployeeCategory.Getall - @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeCategories(UserAccount u) {
        return db.rows("Sp_genEmployeeCategory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** DepartmentNameComboFill: genDepartment.Getall - @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** LocationNameComboFill: genLocation.Getall - @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** SectionNameComboFill: genSection.Getall - @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> sections(UserAccount u) {
        return db.rows("Sp_genSection_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** DesignationNameComboFill: genDesignation.Getall - @Activity 'ReadAll', @OrganizationId, @CompanyId. */
    public List<Map<String, Object>> designations(UserAccount u) {
        return db.rows("Sp_genDesignation_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** ShiftNameComboFill: genShift.Getall - @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> shifts(UserAccount u) {
        return db.rows("Sp_genShift_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** CountryFill: country.GetAll(new Country()) - the form passes an EMPTY Country, so @OrganizationId = @CompanyId = 0, @MethodType 'GetAll'. */
    public List<Map<String, Object>> countries() {
        return db.rows("SP_Country_ReadMethod", "OrganizationId", 0, "CompanyId", 0, "MethodType", "GetAll");
    }

    /** CityFill: City.GetAll - @OrganizationId, @CompanyId, @MethodType 'GetAll'. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return db.rows("SP_City_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll");
    }

    /** ProvinceFill: StateProvince.GetAll - @Activity 'ReadAll', @OrganizationId, @CompanyId (clsGlobalVariables.UserAccount). */
    public List<Map<String, Object>> provinces(UserAccount u) {
        return db.rows("Proc_StateProvince_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** BankName: Bank.GetAllDt - @OrganizationId, @CompanyId, @Activity 'ReadAllBankDT'. */
    public List<Map<String, Object>> banks(UserAccount u) {
        return db.rows("Sp_Bank_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllBankDT");
    }

    /** grdEmployeeDirectBenefitFill / grdEmployeeAssetsBenefitFill: hrmBenefit.Getall - @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> benefits(UserAccount u) {
        return db.rows("Sp_hrmBenefit_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** GridBindEmployeeSalary: hrmSalaryBreakupPolicy.GetByLocationId - @LocationId, @Activity 'ReadByLocationId'. */
    public List<Map<String, Object>> salaryBreakupByLocation(int locationId) {
        return db.rows("Sp_hrmSalaryBreakupPolicy_GetAllMethod", "LocationId", locationId, "Activity", "ReadByLocationId");
    }

    /** Project(): CommonServices.ProjectServiceBind -> Projects.GetAlldt - @OrganizationId, @CompanyId, @MethodType 'GetAll'. */
    public List<Map<String, Object>> projects(UserAccount u) {
        return db.rows("Sp_Projects_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll");
    }

    /** braches(): CommonServices.BrancheServiceBind -> Branches.GetAll - @OrganizationId, @CompanyId, @Activity 'GetAll'. */
    public List<Map<String, Object>> branches(UserAccount u) {
        return db.rows("Sp_Branches_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll");
    }

    /**
     * AccountBinds: CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids) -> COAAllocation.GetAccountTitleByAccountTypeIds:
     * @OrganizationId, @CompanyId, @AppId (the HRM login's AppId), @AccountTypeIds, @UserId (!= 0),
     * @Activity 'GetAccountTitleByAccountTypeIds'. CostCenterId / NotReferred / RecId / class ids are left unset by
     * the helper's defaults, so the BLL guards omit them.
     */
    public List<Map<String, Object>> accountsByType(UserAccount u, int appId, String accountTypeIds) {
        return db.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", appId, "AccountTypeIds", accountTypeIds, "UserId", u.getId() != null && u.getId() != 0 ? u.getId() : null,
                "Activity", "GetAccountTitleByAccountTypeIds");
    }

    /** clsGlobalVariables.ActiveYr (End_Period for txtEndDate): FinancialYear.GetById -> USP_FinancialYear_ReadById @Id. */
    public List<Map<String, Object>> financialYear(int id) {
        return db.rows("USP_FinancialYear_ReadById", "Id", id);
    }

    // ================================================================== genEmployee reads

    /** EmlpoyeeNoGenerate: genEmployee.GenerateEmployeeNo - @OrganizationId, @CompanyId, @Activity 'GenerateEmployeeNo'. */
    public List<Map<String, Object>> generateEmployeeNo(UserAccount u) {
        return db.rows(EMP, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GenerateEmployeeNo");
    }

    /** GridBindEmployeeRegistration: genEmployee.Getall(ReportsParameters) - @OrganizationId, @CompanyId, (@DepartmentId when != 0: never here), @Activity 'ReadAll'. */
    public List<Map<String, Object>> employees(UserAccount u) {
        return db.rows(EMP, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployee.GetByID: @EmployeeId, @Activity 'ReadById' (DAL GetAll's header row). */
    public List<Map<String, Object>> employee(int id) {
        return db.rows(EMP, "EmployeeId", id, "Activity", "ReadById");
    }

    /**
     * DAL genEmployee.GetAll's ten child reads, each @EmployeeId + @Activity:
     * ReadEmployeeHistoryByEmployeeId, ReadEmployeeSalaryByEmployeeId, ReadEmployeeBenfitsByEmployeeId,
     * ReadWeekDaysByEmployeeId, ReadEmployeeAddressByEmployeeId, ReadEmployeeExperenceByEmployeeId,
     * ReadEmployeeEducationByEmployeeId, ReadEmployeeReferebceByEmployeeId, ReadEmployeeFamilyInfoByEmployeeId,
     * ReadEmployeeBankByEmployeeId (spelling as in the DAL and the procedure).
     */
    public List<Map<String, Object>> employeeChild(int id, String activity) {
        return db.rows(EMP, "EmployeeId", id, "Activity", activity);
    }

    /** txtEmployeeNoForUpdate_Leave: genEmployee.GetEmployeeIdByEmployeeNo - @OrganizationId, @CompanyId, @EmployeeNo, @Activity 'GetEmployeeIdByEmployeeNo'. */
    public List<Map<String, Object>> employeeIdByNo(UserAccount u, String employeeNo) {
        return db.rows(EMP, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "EmployeeNo", employeeNo, "Activity", "GetEmployeeIdByEmployeeNo");
    }

    /**
     * cmbDepartment_Leave: genEmployee.GetCaoByDepartmentId - @OrganizationId, @CompanyId, @DepartmentId, @Activity 'GetCaoByDepartmentId'.
     * The form fills ReportsParameters.CompanyId with UserAccount.OrganizationId (a desktop slip) - reproduced, so
     * @CompanyId receives the organization id exactly as on the desktop.
     */
    public List<Map<String, Object>> accountsByDepartment(UserAccount u, int departmentId) {
        return db.rows(EMP, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getOrganizationId(), "DepartmentId", departmentId, "Activity", "GetCaoByDepartmentId");
    }

    /** HRM_Reports.EmployeeRegistrationSlipAndRegister: @OrganizationId, @CompanyId, @EmployeeId only when != 0. */
    public List<Map<String, Object>> slipAndRegister(UserAccount u, int employeeId) {
        return db.rows("Sp_genEmployee_SlipandRegister", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EmployeeId", employeeId != 0 ? employeeId : null);
    }

    // ================================================================== DAL genEmployee.SetData

    /** Everything btnsave_Click hands genEmployee.Save: the header and its ten child lists. */
    public static class EmployeeSave {
        public GenEmployee employee;
        public List<GenEmployeeHistory> histories;
        public List<EmployeeSalary> salaries;
        public List<HrmEmployeeBenefit> benefits;
        public List<GenEmployeeWeekDay> weekDays;
        public List<TblEmployeeAddressDetail> addresses;
        public List<HrmEmployeeExperience> experiences;
        public List<GenEmployeeEducation> educations;
        public List<HrmEmployeeFamilyInfo> families;
        public List<HrmEmployeeReference> references;
        public List<HrmEmployeeBankAccount> banks;
    }

    /**
     * BLL genEmployee.Save -> DAL genEmployee.SetData(obj, EmployeeId == 0 ? "Sp_genEmployee_Insert" : "Sp_genEmployee_Update"),
     * in ONE transaction, step by step as the DAL:
     *   num = SetProc(header) (the Insert's SELECT @EmployeeId; the Update returns none -> obj.EmployeeId);
     *   when num > 0: Sp_UserInfo_GetAllMethod (@OrganizationId, @CompanyId, @USERID = EmployeeNo - it deletes the
     *   device user row), Sp_USERINFO_Insert (USERID = BADGENUMBER = ToInt(EmployeeNo), NAME = First+" "+Middle+" "+Last),
     *   every history row (EmployeeId = EmployeeHistoryLineId = num; its SCOPE_IDENTITY is num2), salaries and benefits
     *   (EmployeeId = num, EmployeeHistoryId = num2), week days, addresses, experience, education, family, references,
     *   bank accounts (EmployeeId = num). Sp_genEmployee_Update deletes all ten child tables first, so the lists replace them.
     */
    public int saveEmployee(EmployeeSave s) {
        return db.tx(() -> {
            GenEmployee e = s.employee;
            int num = db.set(e.EmployeeId == 0 ? "Sp_genEmployee_Insert" : "Sp_genEmployee_Update", e);
            if (num > 0) e.EmployeeId = num; else num = e.EmployeeId;
            if (num > 0) {
                e.EmployeeId = num;
                db.rows("Sp_UserInfo_GetAllMethod", "OrganizationId", e.OrganizationId, "CompanyId", e.CompanyId, "USERID", e.EmployeeNo);
                USERINFO ui = new USERINFO();
                ui.USERID = com.mst.services.hrm.HrmSupport.toInt(e.EmployeeNo);
                ui.BADGENUMBER = com.mst.services.hrm.HrmSupport.toInt(e.EmployeeNo);
                ui.NAME = e.FistName + " " + e.MiddleName + " " + e.LastName;
                ui.OrganizationId = e.OrganizationId;
                ui.CompanyId = e.CompanyId;
                db.set("Sp_USERINFO_Insert", ui);
                int num2 = 0;
                for (GenEmployeeHistory h : s.histories) {
                    h.EmployeeId = num;
                    h.EmployeeHistoryLineId = num;
                    num2 = db.set("Sp_genEmployeeHistory_Insert", h);
                }
                for (EmployeeSalary x : s.salaries) { x.EmployeeId = num; x.EmployeeHistoryId = num2; db.set("[hrm].[Sp_EmployeeSalary_Insert]", x); }
                for (HrmEmployeeBenefit x : s.benefits) { x.EmployeeId = num; x.EmployeeHistoryId = num2; db.set("Sp_hrmEmployeeBenefit_Insert", x); }
                for (GenEmployeeWeekDay x : s.weekDays) { x.EmployeeId = num; db.set("Sp_genEmployeeWeekDay_Insert", x); }
                for (TblEmployeeAddressDetail x : s.addresses) { x.EmployeeId = num; db.set("Sp_tblEmployeeAddressDetail_Insert", x); }
                for (HrmEmployeeExperience x : s.experiences) { x.EmployeeId = num; db.set("Sp_hrmEmployeeExperience_Insert", x); }
                for (GenEmployeeEducation x : s.educations) { x.EmployeeId = num; db.set("Sp_GenEmployeeEducation_Insert", x); }
                for (HrmEmployeeFamilyInfo x : s.families) { x.EmployeeId = num; db.set("Sp_hrmEmployeeFamilyInfo_Insert", x); }
                for (HrmEmployeeReference x : s.references) { x.EmployeeId = num; db.set("Sp_hrmEmployeeReference_Insert", x); }
                for (HrmEmployeeBankAccount x : s.banks) { x.EmployeeId = num; db.set("Sp_hrmEmployeeBankAccount_Insert", x); }
            }
            return num;
        });
    }
}
