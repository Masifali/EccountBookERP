package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Request body of frmEmployeeRegistration's Save / Update (btnsave_Click): every control the form
 * reads into genEmployee, its genEmployeeHistory row and the ten child grids. Public camelCase fields
 * (Jackson binds the JSON keys to them exactly); dates are "yyyy-MM-dd"; combo values are the
 * selected Value (0 = nothing selected, as Conversion.ToInt(null) is 0).
 *
 * The picture / signature paths are NOT taken from the page: the server keeps what the record already
 * holds (see HrmEmployeeService.saveEmployee).
 */
public class EmployeeRegistrationDto {
    /** RecId: 0 = new (Save), otherwise the EmployeeId being updated. */
    public int id;

    // ---- panel4 (employee information header)
    public String employeeNo;          // txtEmployeeNo
    public int employeeTypeId;         // cmbEmployeeType (Employee Information tab)
    public int employeeCategoryId;     // cmbEmployeeCatagory
    public int titleId;                // cmbTitle
    public String firstName;           // txtFirstName
    public String middleName;          // txtMiddleName
    public String lastName;            // txtLastName
    public int relationTitleId;        // cmbRelationTitle -> SubTitleId
    public String relationName;        // txtRelationName
    public String relationMobile;      // txtRelationMobileNo
    public int genderId;               // cmbGender
    public String dob;                 // dtpDOB
    public int nationalityId;          // cmbNationality
    public String email;               // txtEmail
    public String mobile1;             // txtMobile1 (mask 0000 0000000)
    public String mobile2;             // txtMobile2
    public String cnic;                // txtCNIC (mask 00000-0000000-0)
    public String cnicIssueDate;       // dtpCNICIssueDate
    public String cnicExpiryDate;      // dtpCNICExpiryDate
    public String passportNo;          // txtPassportNo
    public String licenseNo;           // txtLicenseNo
    public int bloodGroupId;           // cmbBloodGroup
    public int branchesId;             // combranches (hidden, first row active)
    public int projectId;              // comproject (hidden, first row active)

    // ---- tabPage5 Employee Information (genEmployeeHistory)
    public String startDate;           // txtStartDate
    public String endDate;             // txtEndDate
    public String probationEndDate;    // txtProbationEndingDate
    public int designationId;          // cmbDesignation
    public int shiftId;                // cmbShift
    public int sectionId;              // cmbSection
    public int departmentId;           // cmbDepartment
    public int payableAcId;            // CmbSalaryPayableAc
    public int expenseAccountId;       // CmbSalariesExpensesAc
    public int loanAcId;               // CmbSalaryLoanAc
    public int employeeGroupId;        // cmbEmployeeGroup
    public int locationId;             // cmbLocation
    public int reportedBranchId;       // cmbReportedBranch -> PartyLocationId
    public int reportedPersonId;       // cmbReportedPerson -> ReportToEmployeeId
    public int storeId;                // cmbStore (hidden)
    public String totalSalary;         // txtSalary
    public boolean active;             // IsActiveEmployeeInformation

    // ---- grids
    public List<EmployeeSalaryRowDto> salaries;            // grdEmployeeSalary
    public List<EmployeeBenefitRowDto> directBenefits;     // grdEmployeeDirectBenefits
    public List<EmployeeBenefitRowDto> assetBenefits;      // grdEmployeeAssetsBenefit
    public List<EmployeeWeekDayRowDto> weekDays;           // grdWeekDays
    public List<EmployeeAddressRowDto> addresses;          // grdAddressDetail
    public List<EmployeeExperienceRowDto> experiences;     // grdEmployeeExperience
    public List<EmployeeEducationRowDto> educations;       // grdEmployeeEducation
    public List<EmployeeFamilyRowDto> families;            // grdFamilyInfo
    public List<EmployeeReferenceRowDto> references;       // grdReference
    public List<EmployeeBankRowDto> banks;                 // grdBankAccount
}
