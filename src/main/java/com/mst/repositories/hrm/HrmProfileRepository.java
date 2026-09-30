package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.profile.GenDepartment;
import com.mst.models.hrm.profile.GenDesignation;
import com.mst.models.hrm.profile.GenEmployeeCategory;
import com.mst.models.hrm.profile.GenEmployeeGroup;
import com.mst.models.hrm.profile.GenLocation;
import com.mst.models.hrm.profile.GenProfile;
import com.mst.models.hrm.profile.GenProfileType;
import com.mst.models.hrm.profile.GenSection;
import com.mst.models.hrm.profile.HrmBenefit;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Profile Management" screens (AppModules 2018), Architecture.DAL.HRM.ProfileManagement.*
 * and Architecture.DAL.HRM.PolicyManagment.genProfile / genProfileType.
 * Every call is the desktop DAL's own procedure with the parameters its BLL sends
 * (recovered_source/projects/architecture.bll|dal, procdure.utf8.sql).
 *
 *   634 Employee Group     genEmployeeGroup.cs    BLL 0169 / DAL 0133  Sp_genEmployeeGroup_GetAllMethod ReadAll|ReadById, Sp_genEmployeeGroup_Insert|Update (11)
 *   635 Employee Category  frmEmployeeCategory.cs BLL 0168 / DAL 0132  Sp_genEmployeeCategory_GetAllMethod ReadAll|ReadById, Sp_genEmployeeCategory_Insert|Update (11)
 *   636 Benefit            BenifitDefine.cs       BLL 0172 / DAL 0136  Sp_hrmBenefit_GetAllMethod ReadAll|ReadById, Sp_hrmBenefit_Insert|Update (15)
 *   637 Designation        genDesignation.cs      BLL 0167 / DAL 0131  Sp_genDesignation_GetAllMethod ReadAll|ReadById, Sp_genDesignation_Insert|Update (10)
 *   638 Department         genDepartment.cs       BLL 0166 / DAL 0130  Sp_genDepartment_GetAllMethod ReadAll|ReadById, Sp_genDepartment_Insert|Update (17),
 *                                                                       Sp_COAAllocation_GetAllMethod GetAccountTitleByAccountTypeIds
 *   639 Section            DefineSection.cs       BLL 0171 / DAL 0135  Sp_genSection_GetAllMethod ReadAll|ReadById, Sp_genSection_Insert|Update (14)
 *   640 Location           frmgenLocation.cs      BLL 0170 / DAL 0134  Sp_genLocation_GetAllMethod ReadAll|ReadById, Sp_genLocation_Insert|Update (25)
 *   641 Profile Type       ProfileTypes.cs        BLL 0174 / DAL 0140  Sp_genProfileType_GetAllMethod ReadAll|ReadById, Sp_genProfileType_Insert|Update (6)
 *   642 Define Profile     ProfileDefine.cs       BLL 0173 / DAL 0139  Sp_genProfile_GetAllMethod ReadAll|ReadById|ReadByProfileTypeId, Sp_genProfile_Insert|Update (16)
 *
 * Every DAL SetData is the same: Convert.ToInt32(SetProc) inside one SqlTransaction; a positive
 * result is the new id, otherwise the model's own id is returned (an Update returns no row).
 */
@Repository
public class HrmProfileRepository {

    private final HrmProcRepository db;

    public HrmProfileRepository(HrmProcRepository db) { this.db = db; }

    private static int idOr(int n, int ownId) { return n > 0 ? n : ownId; }

    // ------------------------------------------------------------------ 637 Designation

    /** genDesignation.Getall: @Activity 'ReadAll', @OrganizationId, @CompanyId. */
    public List<Map<String, Object>> designations(UserAccount u) {
        return db.rows("Sp_genDesignation_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** genDesignation.GetByID: @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> designation(int id) {
        return db.rows("Sp_genDesignation_GetAllMethod", "Id", id, "Activity", "ReadById");
    }

    /**
     * DAL genDesignation.SetData: Convert.ToInt32(SetProc) - the Insert's SCOPE_IDENTITY; the Update
     * returns no row (0), and the DAL then returns obj.DesignationId, so the form sees the id either way.
     */
    public int saveDesignation(GenDesignation m) {
        int n = db.set(m.DesignationId == 0 ? "Sp_genDesignation_Insert" : "Sp_genDesignation_Update", m);
        return n > 0 ? n : m.DesignationId;
    }

    // ------------------------------------------------------------------ 634 Employee Group

    /** genEmployeeGroup.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeGroups(UserAccount u) {
        return db.rows("Sp_genEmployeeGroup_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployeeGroup.GetByID: @EmployeeGroupId, @Activity 'ReadById'. */
    public List<Map<String, Object>> employeeGroup(int id) {
        return db.rows("Sp_genEmployeeGroup_GetAllMethod", "EmployeeGroupId", id, "Activity", "ReadById");
    }

    /** genEmployeeGroup.Save: Sp_genEmployeeGroup_Insert when EmployeeGroupId == 0, else _Update (DAL 0133 SetData). */
    public int saveEmployeeGroup(GenEmployeeGroup m) {
        return idOr(db.set(m.EmployeeGroupId == 0 ? "Sp_genEmployeeGroup_Insert" : "Sp_genEmployeeGroup_Update", m), m.EmployeeGroupId);
    }

    // ------------------------------------------------------------------ 635 Employee Category

    /** genEmployeeCategory.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeCategories(UserAccount u) {
        return db.rows("Sp_genEmployeeCategory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployeeCategory.GetByID: @EmployeeCategoryId, @Activity 'ReadById'. */
    public List<Map<String, Object>> employeeCategory(int id) {
        return db.rows("Sp_genEmployeeCategory_GetAllMethod", "EmployeeCategoryId", id, "Activity", "ReadById");
    }

    /** genEmployeeCategory.Save: Sp_genEmployeeCategory_Insert when EmployeeCategoryId == 0, else _Update. */
    public int saveEmployeeCategory(GenEmployeeCategory m) {
        return idOr(db.set(m.EmployeeCategoryId == 0 ? "Sp_genEmployeeCategory_Insert" : "Sp_genEmployeeCategory_Update", m), m.EmployeeCategoryId);
    }

    // ------------------------------------------------------------------ 636 Benefit

    /** hrmBenefit.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> benefits(UserAccount u) {
        return db.rows("Sp_hrmBenefit_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** hrmBenefit.GetByID: @BenefitId, @Activity 'ReadById'. */
    public List<Map<String, Object>> benefit(int id) {
        return db.rows("Sp_hrmBenefit_GetAllMethod", "BenefitId", id, "Activity", "ReadById");
    }

    /**
     * hrmBenefit.Save: Sp_hrmBenefit_Insert when BenefitId == 0, else _Update. NOTE: both procedures
     * of the 23-Sep-2026 DB start with RAISERROR('Record Can Not Insert or Update Please Contact
     * EccountbookERP Support Team...') - the desktop shows that message too.
     */
    public int saveBenefit(HrmBenefit m) {
        return idOr(db.set(m.BenefitId == 0 ? "Sp_hrmBenefit_Insert" : "Sp_hrmBenefit_Update", m), m.BenefitId);
    }

    // ------------------------------------------------------------------ 642 genProfile (used by 636 / 638 / 640 / 642)

    /** genProfile.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> profiles(UserAccount u) {
        return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genProfile.GetByID: @ProfileId, @Activity 'ReadById'. */
    public List<Map<String, Object>> profile(int id) {
        return db.rows("Sp_genProfile_GetAllMethod", "ProfileId", id, "Activity", "ReadById");
    }

    /**
     * genProfile.GetByProfileTypeId: @OrganizationId, @CompanyId, @ProfileTypeId (only when != 0),
     * @Activity 'ReadByProfileTypeId'. The BLL never sends @IsActive (the forms set IsActive = true on the
     * model, but the parameter list does not carry it), so the proc returns active and inactive rows.
     */
    public List<Map<String, Object>> profilesByType(UserAccount u, int profileTypeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (profileTypeId != 0) p.put("ProfileTypeId", profileTypeId);
        p.put("Activity", "ReadByProfileTypeId");
        return db.rows("Sp_genProfile_GetAllMethod", p);
    }

    /**
     * genProfile.Save: Sp_genProfile_Insert when ProfileId == 0, else _Update. NOTE: both procedures
     * RAISERROR('Record Can Not Insert or Update ...') before their body in the 23-Sep-2026 DB.
     */
    public int saveProfile(GenProfile m) {
        return idOr(db.set(m.ProfileId == 0 ? "Sp_genProfile_Insert" : "Sp_genProfile_Update", m), m.ProfileId);
    }

    // ------------------------------------------------------------------ 641 Profile Type

    /** genProfileType.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll' (the proc's WHERE is commented out: every type). */
    public List<Map<String, Object>> profileTypes(UserAccount u) {
        return db.rows("Sp_genProfileType_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genProfileType.GetByID: @ProfileTypeId, @Activity 'ReadById'. */
    public List<Map<String, Object>> profileType(int id) {
        return db.rows("Sp_genProfileType_GetAllMethod", "ProfileTypeId", id, "Activity", "ReadById");
    }

    /**
     * genProfileType.Save: Sp_genProfileType_Insert when ProfileTypeId == 0, else _Update. NOTE: both
     * procedures RAISERROR('Record Can Not Insert or Update ...') before their body in the 23-Sep-2026 DB.
     */
    public int saveProfileType(GenProfileType m) {
        return idOr(db.set(m.ProfileTypeId == 0 ? "Sp_genProfileType_Insert" : "Sp_genProfileType_Update", m), m.ProfileTypeId);
    }

    // ------------------------------------------------------------------ 638 Department

    /** genDepartment.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll' (also DefineSection.DepartmentFill). */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genDepartment.GetByID: @DepartmentId, @Activity 'ReadById'. */
    public List<Map<String, Object>> department(int id) {
        return db.rows("Sp_genDepartment_GetAllMethod", "DepartmentId", id, "Activity", "ReadById");
    }

    /** genDepartment.Save: Sp_genDepartment_Insert when DepartmentId == 0, else _Update. */
    public int saveDepartment(GenDepartment m) {
        return idOr(db.set(m.DepartmentId == 0 ? "Sp_genDepartment_Insert" : "Sp_genDepartment_Update", m), m.DepartmentId);
    }

    /**
     * CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids) -> COAAllocation.GetAccountTitleByAccountTypeIds:
     * @OrganizationId, @CompanyId, @AppId, @AccountTypeIds, @UserId (when != 0), @Activity
     * 'GetAccountTitleByAccountTypeIds' on Sp_COAAllocation_GetAllMethod. CostCenterId / NotReferred / RecId are
     * 0 and the class filters empty in this call, so the BLL does not send them.
     */
    public List<Map<String, Object>> accountsByTypes(UserAccount u, String accountTypeIds) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("AppId", u.getAppId());
        p.put("AccountTypeIds", accountTypeIds);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return db.rows("Sp_COAAllocation_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ 639 Section

    /** genSection.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> sections(UserAccount u) {
        return db.rows("Sp_genSection_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genSection.GetByID: @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> section(int id) {
        return db.rows("Sp_genSection_GetAllMethod", "Id", id, "Activity", "ReadById");
    }

    /** genSection.Save: Sp_genSection_Insert when SectionId == 0, else _Update. */
    public int saveSection(GenSection m) {
        return idOr(db.set(m.SectionId == 0 ? "Sp_genSection_Insert" : "Sp_genSection_Update", m), m.SectionId);
    }

    // ------------------------------------------------------------------ 640 Location

    /** genLocation.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genLocation.GetByID: @LocationId, @Activity 'ReadById'. */
    public List<Map<String, Object>> location(int id) {
        return db.rows("Sp_genLocation_GetAllMethod", "LocationId", id, "Activity", "ReadById");
    }

    /** genLocation.Save: Sp_genLocation_Insert when LocationId == 0, else _Update. */
    public int saveLocation(GenLocation m) {
        return idOr(db.set(m.LocationId == 0 ? "Sp_genLocation_Insert" : "Sp_genLocation_Update", m), m.LocationId);
    }
}
