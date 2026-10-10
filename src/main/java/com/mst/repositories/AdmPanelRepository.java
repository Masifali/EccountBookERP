package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * DAL of the Admin Panel (module 22) screens that were not yet on the web:
 *
 *   312 Templates.DefineCompany                        BLL 0062 Company / Organization / OrganizationTemplates, DAL 0056
 *   314 Templates.AllocationOrganizationTemplateRights BLL 0097 OrganizationTemplateRights, DAL 0089
 *   315 Templates.frmModuleAllocateToOrganizationTemplates  BLL 0095 / 0094, DAL 0087 / 0086
 *   316 Templates.frmScreensAllocateToCompany          BLL 0093, DAL 0085
 *   318/397 UserRightsManagement.UserRightsEditing     BLL 0084 tblUserRights, DAL tblUserRights.SetData
 *   319 Configurations.frmUserModuleAdmin              BLL 0096 UserModuleAdmin, DAL 0088
 *   791 frmScreenDefinetion                            BLL 0080 ScreenDefinition / 0081 ScreenRights / 0007 Applications,
 *                                                      DAL 0072; tabs DocumentType (BLL 0070) and ReportsMethod (BLL 0026)
 *
 * GenericProvider.SetProc / SetNew send EVERY non-virtual property of the model as "@"+name and ADO.NET drops the
 * null ones (DesktopProc does the same). Every write below therefore sends exactly the model's non-virtual
 * properties, in the models' declaration order. Each @Transactional method is the desktop DAL's single
 * SqlTransaction (any exception rolls everything back). No table, column or procedure is created or changed.
 */
@Repository
public class AdmPanelRepository {

    private final JdbcTemplate jdbc;

    public AdmPanelRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    // ================================================================== shared lookups

    /** OrganizationTemplates.FormHistory - Sp_OrganizationTemplates_GetAll @Activity='GetAll'. */
    public List<Map<String, Object>> templates() {
        return rows("Sp_OrganizationTemplates_GetAll", params("Activity", "GetAll"));
    }

    /** ScreenDefinition.GetAllModules - Sp_AppModules_ReadAll. */
    public List<Map<String, Object>> appModules() {
        return rows("Sp_AppModules_ReadAll", params());
    }

    // ================================================================== 318 UserRightsEditing

    /** UserAccount.ReadAll - Sp_UserAccount_GetAllMethod @OrganizationId @CompanyId @Activity='ReadAll'. */
    public List<Map<String, Object>> allUsers(int organizationId, int companyId) {
        return rows("Sp_UserAccount_GetAllMethod",
                params("OrganizationId", organizationId, "CompanyId", companyId, "Activity", "ReadAll"));
    }

    /** UserAccountAllocation.GetAllCompaniesByUserId - IsActive=1 is sent because the form sets it. */
    public List<Map<String, Object>> companiesOfUser(int organizationId, int userId) {
        return rows("sp_UserAccountAllocation_GetAllMethod",
                params("OrganizationId", organizationId, "UserAccountId", userId, "IsActive", 1,
                        "Activity", "GetCompaniesByUserId"));
    }

    /** tblUserRights.GetbyId - Sp_tblUserRights_GetAllMethod @UserId @CompanyId @Activity='GetbyId'. */
    public List<Map<String, Object>> userRights(int userId, int companyId) {
        return rows("Sp_tblUserRights_GetAllMethod",
                params("UserId", userId, "CompanyId", companyId, "Activity", "GetbyId"));
    }

    /**
     * DAL tblUserRights.SetData(obj, "Sp_tblUserRights_Update") in one transaction:
     * ScreenRightstoUser{UserId, NewUserid=0, CompanyId, Activity="DeleteByUserIdCompanyId"} through
     * Sp_tblUserRights_GetAllMethod, then one Sp_tblUserRights_Update per grid row
     * (Value, Id=0, RightsID, ScreenID, UserId, CompanyId, AppId=0).
     * The desktop deletes with the LOGGED-IN company and writes with the SELECTED company - kept as is.
     *
     * @param rows each {screenId, rightsId, value(0/1)}
     */
    @Transactional
    public int saveUserRights(int userId, int deleteCompanyId, int rowCompanyId, List<int[]> rows) {
        DesktopProc.setProc(jdbc, "Sp_tblUserRights_GetAllMethod",
                params("UserId", userId, "NewUserid", 0, "CompanyId", deleteCompanyId,
                        "Activity", "DeleteByUserIdCompanyId"));
        int result = 0;
        for (int[] r : rows) {
            result = DesktopProc.setProc(jdbc, "Sp_tblUserRights_Update",
                    params("Value", r[2] != 0, "Id", 0, "RightsID", r[1], "ScreenID", r[0],
                            "UserId", userId, "CompanyId", rowCompanyId, "AppId", 0));
        }
        return result;
    }

    // ================================================================== 319 frmUserModuleAdmin

    /** UserAccount.ReadUserByCompanyId(CompanyId, 0) - @CompanyId @Activity='ReadAllUser'. */
    public List<Map<String, Object>> usersOfCompany(int companyId) {
        return rows("Sp_UserAccount_GetAllMethod", params("CompanyId", companyId, "Activity", "ReadAllUser"));
    }

    public List<Map<String, Object>> userModulesUnAllocated(int organizationId, int companyId, int userId) {
        return rows("USP_UserModuleAdmin_UnAllocatedData",
                params("OrganizationId", organizationId, "CompanyId", companyId, "UserId", userId));
    }

    public List<Map<String, Object>> userModulesAllocated(int organizationId, int companyId, int userId) {
        return rows("USP_UserModuleAdmin_AllocatedData",
                params("OrganizationId", organizationId, "CompanyId", companyId, "UserId", userId));
    }

    /**
     * UserModuleAdmin.Save -> DAL 0088 SetData: USP_UserModuleAdmin_InsertAndUpdate once per row, one transaction.
     * Model order: IsActive, EntryDate, ModifyDate, CompanyId, EntryUserId, UserModuleAdminId, UserId,
     * ModifyUserId, OrganizationId, ModuleId.
     */
    @Transactional
    public long saveUserModules(List<Map<String, Object>> items) {
        long result = 0;
        for (Map<String, Object> it : items) {
            result = DesktopProc.setProc(jdbc, "USP_UserModuleAdmin_InsertAndUpdate", it);
        }
        return result;
    }

    // ================================================================== 315 frmModuleAllocateToOrganizationTemplates

    public List<Map<String, Object>> templateModulesUnAllocated(int templateId) {
        return rows("USP_ModuleAllocateToOrganizationTemplates_UnAllocatedData", params("TemplateId", templateId));
    }

    public List<Map<String, Object>> templateModulesAllocated(int templateId) {
        return rows("USP_ModuleAllocateToOrganizationTemplates_AllocatedData", params("TemplateId", templateId));
    }

    /** @ModuleId is added only when it is not 0 (BLL 0094). */
    public List<Map<String, Object>> templateScreensUnAllocated(int templateId, int moduleId) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (moduleId != 0) p.put("ModuleId", moduleId);
        p.put("TemplateId", templateId);
        return rows("USP_InActiveScreenOfTemplatesModule_UnAllocatedData", p);
    }

    public List<Map<String, Object>> templateScreensAllocated(int templateId, int moduleId) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (moduleId != 0) p.put("ModuleId", moduleId);
        p.put("TemplateId", templateId);
        return rows("USP_InActiveScreenOfTemplatesModule_AllocatedData", p);
    }

    /**
     * ModuleAllocateToOrganizationTemplates.Save -> DAL 0087: USP_ModuleAllocateToOrganizationTemplates_InsertAndUpdate
     * per row, one transaction. Model: IsActive, EntryDate, ModifyDate, EntryUserId, Id, ModifyUserId, ModuleId,
     * OrganizationTemplateId.
     */
    @Transactional
    public long saveTemplateModules(List<Map<String, Object>> items) {
        long result = 0;
        for (Map<String, Object> it : items) {
            result = DesktopProc.setProc(jdbc, "USP_ModuleAllocateToOrganizationTemplates_InsertAndUpdate", it);
        }
        return result;
    }

    /**
     * InActiveScreenOfTemplatesModule.Save -> DAL 0086: USP_InActiveScreenOfTemplatesModule_InsertAndUpdate per row.
     * Model: IsActive, EntryDate, ModifyDate, EntryUserId, Id, ModifyUserId, ModuleId, OrganizationTemplateId,
     * ScreenName.
     */
    @Transactional
    public long saveTemplateScreens(List<Map<String, Object>> items) {
        long result = 0;
        for (Map<String, Object> it : items) {
            result = DesktopProc.setProc(jdbc, "USP_InActiveScreenOfTemplatesModule_InsertAndUpdate", it);
        }
        return result;
    }

    // ================================================================== 316 frmScreensAllocateToCompany

    /** Company.GetCompaniesByTemplateId - USP_Company_ByTemplateId @TemplateId. */
    public List<Map<String, Object>> companiesByTemplate(int templateId) {
        return rows("USP_Company_ByTemplateId", params("TemplateId", templateId));
    }

    public List<Map<String, Object>> companyScreensUnAllocated(int companyId, int moduleId) {
        return rows("USP_ScreensAllocateToCompany_UnAllocatedData", params("CompanyId", companyId, "ModuleId", moduleId));
    }

    public List<Map<String, Object>> companyScreensAllocated(int companyId, int moduleId) {
        return rows("USP_ScreensAllocateToCompany_AllocatedData", params("CompanyId", companyId, "ModuleId", moduleId));
    }

    /**
     * ScreensAllocateToCompany.Save -> DAL 0085: USP_ScreensAllocateToCompany_Insert per row, one transaction.
     * Model: EntryUserId, ModifyUserId, EntryDate, ModifyDate, CompanyId, Id, IsActive, ScreenName.
     */
    @Transactional
    public int saveCompanyScreens(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> it : items) {
            result = DesktopProc.setProc(jdbc, "USP_ScreensAllocateToCompany_Insert", it);
        }
        return result;
    }

    // ================================================================== 314 AllocationOrganizationTemplateRights

    /** OrganizationTemplateRights.GetAllocatedAndUnAllocatedScreens - @ActionId 1 = unallocated, 2 = allocated. */
    public List<Map<String, Object>> templateRightsScreens(int templateId, int actionId, int moduleId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationTemplateId", templateId);
        p.put("ActionId", actionId);
        if (moduleId != 0) p.put("AppModuleId", moduleId);
        return rows("USP_ScreenAllocatedOrUnAllocatedInOrganizationTemplateRights", p);
    }

    /**
     * DAL 0089 SetData: ExecuteNonQuery Sp_OrganizationTemplateRights_Insert @FIFOXML = ConvertListToXmlSerializer
     * of the {Id, OrganizationTemplateId, ScreenId} items, one transaction.
     */
    @Transactional
    public void saveTemplateRights(int templateId, List<Integer> screenIds) {
        StringBuilder xml = new StringBuilder("<ArrayOfOrganizationTemplateRights>");
        for (Integer s : screenIds) {
            xml.append("<OrganizationTemplateRights><Id>0</Id><OrganizationTemplateId>").append(templateId)
               .append("</OrganizationTemplateId><ScreenId>").append(s).append("</ScreenId></OrganizationTemplateRights>");
        }
        xml.append("</ArrayOfOrganizationTemplateRights>");
        rows("Sp_OrganizationTemplateRights_Insert", params("FIFOXML", screenIds.isEmpty() ? "" : xml.toString()));
    }

    /** OrganizationTemplateRights.DeleteById - Sp_OrganizationTemplateRights_GetAll 'DeleteByOrganizationTemplateId'. */
    public void deleteTemplateRights(int templateId, String screenIds) {
        rows("Sp_OrganizationTemplateRights_GetAll",
                params("OrganizationTemplateId", templateId, "ScreenIds", screenIds,
                        "Activity", "DeleteByOrganizationTemplateId"));
    }

    // ================================================================== 312 DefineCompany

    /** Organization.GetAll - Sp_Organization_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> organizations() {
        return rows("Sp_Organization_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** Organization.GetByID - @Id @Activity='ReadById'. */
    public Map<String, Object> organization(int id) {
        List<Map<String, Object>> r = rows("Sp_Organization_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** Company.FormHistory(0) - Sp_Company_GetAllMethod @Activity='FormHistory' (no organisation filter). */
    public List<Map<String, Object>> companyHistory() {
        return rows("Sp_Company_GetAllMethod", params("Activity", "FormHistory"));
    }

    /** Company.GetByID - Proc_Company_ReadByID @Id. */
    public Map<String, Object> company(int id) {
        List<Map<String, Object>> r = rows("Proc_Company_ReadByID", params("Id", id));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * Company.SaveAll -> DAL 0056 SetDataForAll, one transaction:
     * USP_NewCompanyRegister (CompanyRegistration model, scalar = new company id; &lt;= 0 throws),
     * Sp_Company_GetAllMethod 'GetNewlyGeneratedCompanyData', the CLV licence key, Insert_CLV.
     */
    @Transactional
    public int registerCompany(Map<String, Object> model, LicenseKeyFactory keys) {
        int num = DesktopProc.setProc(jdbc, "USP_NewCompanyRegister", model);
        if (num <= 0) throw new IllegalStateException("Cannot Find Id of Newly Generated Organization Info");
        List<Map<String, Object>> dt = rows("Sp_Company_GetAllMethod",
                params("Activity", "GetNewlyGeneratedCompanyData", "Id", num));
        if (dt.isEmpty()) throw new IllegalStateException("Cannot Find Info Of Newly Generated Organization!");
        Map<String, Object> first = dt.get(0);
        String compName = first.get("CompName") == null ? "" : String.valueOf(first.get("CompName"));
        String licenseKey = keys.create(compName, java.time.LocalDate.now().plusDays(90));
        DesktopProc.setProc(jdbc, "Insert_CLV",
                params("CompanyId", toInt(first.get("CompanyId")), "Id", 0,
                        "OrganizationId", toInt(first.get("OrganizationId")),
                        "CompanyName", compName, "LicenseKey", licenseKey));
        return num;
    }

    /** Company.Save with Id &gt; 0 -> DAL 0056 SetData(obj, "Proc_Company_Update"), one transaction. */
    @Transactional
    public int updateCompany(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, "Proc_Company_Update", model);
    }

    /** Supplies GetKey(CompanyName, ToDate) - kept out of the DAL so the cipher lives in one place. */
    public interface LicenseKeyFactory {
        String create(String companyName, java.time.LocalDate toDate);
    }

    // ================================================================== 791 frmScreenDefinetion

    public List<Map<String, Object>> screenDefinitions() {
        return rows("Sp_ScreenDefinition_GetAllMethod", params("Activity", "FormHistory"));
    }

    public Map<String, Object> screenDefinition(int id) {
        List<Map<String, Object>> r = rows("Sp_ScreenDefinition_GetAllMethod", params("Activity", "ReadById", "Id", id));
        return r.isEmpty() ? null : r.get(0);
    }

    /** Company.GetAllCompanies - Sp_Company_GetAllMethod @Activity='ReadAll' (Id, CompName, PictureURL). */
    public List<Map<String, Object>> allCompanies() {
        return rows("Sp_Company_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** ScreenRights.GetAll - Sp_UrmRightsName_GetAllMethod (Id, RightsName). */
    public List<Map<String, Object>> rightNames() {
        return rows("Sp_UrmRightsName_GetAllMethod", params());
    }

    public List<Map<String, Object>> screenRightsOf(int screenId) {
        return rows("Sp_ScreenRights_GetAllMethod", params("Activity", "ReadByScreenId", "ScreenId", screenId));
    }

    /** Applications.getApplicationsByMasterAppId(0) - usp_Applications_GetAllMethod (MasterAppId not sent for 0). */
    public List<Map<String, Object>> applications() {
        return rows("usp_Applications_GetAllMethod", params("Activity", "getApplicationsByMasterAppId"));
    }

    public List<Map<String, Object>> applicationsOfScreen(int screenId) {
        return rows("Usp_ApplicationsAllocateToScreen", params("ScreenId", screenId));
    }

    /**
     * ScreenDefinition.save -> DAL 0072 SetData, one transaction:
     * Sp_ScreenDefinition_Insert (Id == 0) or _Update; a positive scalar becomes the Id, otherwise the Id stays;
     * Sp_ScreenRights_Insert per ticked right; USP_ApplicationsAllocateToScreen_Insert per ticked application;
     * ExecuteNonQuery USP_CompanyRightsAndtblUserRights_Insert @ScreenId @ModuleId @CompanyIds.
     */
    @Transactional
    public int saveScreenDefinition(Map<String, Object> model, List<String> rightNames, List<Integer> appIds) {
        int id = toInt(model.get("Id"));
        int num = DesktopProc.setProc(jdbc, id == 0 ? "Sp_ScreenDefinition_Insert" : "Sp_ScreenDefinition_Update", model);
        if (num > 0) id = num; else num = id;
        for (String right : rightNames) {
            DesktopProc.setProc(jdbc, "Sp_ScreenRights_Insert", params("Id", 0, "ScreenID", id, "RightName", right));
        }
        String screenName = String.valueOf(model.get("ScreenName"));
        for (Integer app : appIds) {
            DesktopProc.setProc(jdbc, "USP_ApplicationsAllocateToScreen_Insert",
                    params("Id", 0, "ScreenId", id, "AppId", app, "IsActive", true, "ScreenName", screenName));
        }
        rows("USP_CompanyRightsAndtblUserRights_Insert",
                params("ScreenId", id, "ModuleId", model.get("ModuleId"), "CompanyIds", model.get("CompanyIds")));
        return num;
    }

    /**
     * ScreenDefinition.ScreenSortingAndAliasUpdate - NOT transactional on the desktop either: every grid row is its
     * own call of Sp_ScreenDefinition_GetAllMethod 'ScreenSortingAndAliasUpdate'.
     */
    public void screenSortingAndAliasUpdate(List<Map<String, Object>> items) {
        for (Map<String, Object> it : items) {
            rows("Sp_ScreenDefinition_GetAllMethod",
                    params("ScreenAlias", it.get("ScreenAlias"), "SortNo", it.get("SortNo"), "Id", it.get("Id"),
                            "IsActive", it.get("IsActive"), "Activity", "ScreenSortingAndAliasUpdate"));
        }
    }

    // ---- DocumentType tab (BLL 0070) and ReportsMethod tab (BLL 0026)

    public List<Map<String, Object>> documentTypes() {
        return rows("USP_DocumentType_GetAllMethod", params("Activity", "FormHistory"));
    }

    public boolean documentTypeExists(int id) {
        return !rows("USP_DocumentType_GetAllMethod", params("Activity", "CheckIfIdExists", "Id", id)).isEmpty();
    }

    /** SetDataNew(obj, "", "Update"|"Insert") -> schema + "usp_DocumentType_" + ProcName; every non-virtual property. */
    @Transactional
    public int saveDocumentType(Map<String, Object> model) {
        int id = toInt(model.get("Id"));
        boolean exists = documentTypeExists(id);
        Integer r = DesktopProc.scalar(jdbc, exists ? "usp_DocumentType_Update" : "usp_DocumentType_Insert", model);
        return r == null ? 0 : r;
    }

    public List<Map<String, Object>> reportsMethods() {
        return rows("USP_ReportsMethod_GetAllMethod", params("Activity", "FormHistory"));
    }

    /** SetDataNew(obj, "") -> usp_ReportsMethod_Insert with Id, RefDocumentTypeId, ReportTypeId, Remarks, ReportMethod, ScreenName. */
    @Transactional
    public int saveReportsMethod(Map<String, Object> model) {
        Integer r = DesktopProc.scalar(jdbc, "usp_ReportsMethod_Insert", model);
        return r == null ? 0 : r;
    }

    // ------------------------------------------------------------------ helpers

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    static List<Object> none() { return new ArrayList<>(); }

    static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }
}
