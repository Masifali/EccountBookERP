package com.mst.services;

import com.mst.repositories.AdmPanelRepository;
import com.mst.security.CurrentUserContext;
import com.mst.models.UserAccount;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import javax.crypto.Cipher;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * BLL of the Admin Panel (module 22) screens: 312 DefineCompany, 314 AllocationOrganizationTemplateRights,
 * 315 frmModuleAllocateToOrganizationTemplates, 316 frmScreensAllocateToCompany, 318/397 UserRightsEditing,
 * 319 frmUserModuleAdmin and 791 frmScreenDefinetion. See {@link AdmPanelRepository} for the DAL mapping.
 *
 * GATE. These forms are reachable only from DashboardNew's gear item "Admin Panel", drawn only when
 * UserAccount.RoleName == "Admin" - the same test is made here for every read and write.
 *
 * They change rights and security data, so every id the browser sends is checked against the list the desktop
 * form itself would have shown (the same procedure, re-run on the server) and the model is built from THAT row,
 * never from the browser's values. Organisation, company, user and dates come from the session / the server clock.
 */
@Service
public class AdmPanelService {

    @Autowired private AdmPanelRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ScreenRouteIndex screenRouteIndex;

    // ------------------------------------------------------------------ gate + helpers

    public UserAccount admin() {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!"Admin".equals(currentUserContext.currentRoleName()))
            throw new AccessDeniedException("Admin Panel is shown on the desktop only when RoleName is \"Admin\".");
        return u;
    }

    private int org() { return currentUserContext.currentOrganizationId(); }
    private int comp() { return currentUserContext.currentCompanyId(); }
    private int uid() { return currentUserContext.currentUserId(); }
    private static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    private static int asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        String s = String.valueOf(o).trim();
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }
    private static boolean truthy(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o).trim().toLowerCase(Locale.ROOT);
        return s.equals("true") || s.equals("1");
    }
    private static Map<String, Object> pick(Map<String, Object> row, String... keys) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : keys) m.put(k, row.get(k));
        return m;
    }
    private static List<Map<String, Object>> pickAll(List<Map<String, Object>> rows, String... keys) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(pick(r, keys));
        return out;
    }
    @SuppressWarnings("unchecked")
    private static List<Object> list(Object o) { return o instanceof List ? (List<Object>) o : new ArrayList<>(); }
    @SuppressWarnings("unchecked")
    private static Map<String, Object> map(Object o) { return o instanceof Map ? (Map<String, Object>) o : new LinkedHashMap<>(); }

    private static Map<String, Object> ok(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", message);
        return r;
    }

    /** ScreenAlias link column = CommonServices.OpenDynamicallyScreen(TargetUrl, ...): a link only when a web route exists. */
    private List<Map<String, Object>> withLinks(List<Map<String, Object>> rows, String... keys) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = pick(r, keys);
            String url = null;
            try { url = screenRouteIndex.routeFor(str(r.get("ScreenName")), str(r.get("TargetUrl"))); } catch (RuntimeException e) { /* no link */ }
            m.put("url", url);
            out.add(m);
        }
        return out;
    }

    private boolean templateExists(int templateId) {
        for (Map<String, Object> t : repo.templates()) if (asInt(t.get("Id")) == templateId) return true;
        return false;
    }

    /** OrganizationTemplates.FormHistory - the grid / combo source shared by 314, 315, 316 and 312. */
    public List<Map<String, Object>> templates() {
        admin();
        return pickAll(repo.templates(), "Id", "TemplateName", "TemplateDescription");
    }

    // ================================================================== 318 UserRightsEditing

    private List<Map<String, Object>> urUsers() {
        return repo.allUsers(org(), comp());
    }

    /** UserNameFill - BindDDLNew(dt, CmbUserName, "Id", "UserName", "UserName", false). */
    public Map<String, Object> urSetup() {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("users", pickAll(urUsers(), "Id", "UserName"));
        return r;
    }

    /** CompanyNameFill - BindDDLNew(dt, CmbCompanyName, "CompanyId", "CompName", "Company", false). */
    public List<Map<String, Object>> urCompanies(int userId) {
        admin();
        if (!urUserKnown(userId)) return new ArrayList<>();
        return pickAll(repo.companiesOfUser(org(), userId), "CompanyId", "CompName");
    }

    private boolean urUserKnown(int userId) {
        for (Map<String, Object> u : urUsers()) if (asInt(u.get("Id")) == userId) return true;
        return false;
    }
    private boolean urCompanyKnown(int userId, int companyId) {
        for (Map<String, Object> c : repo.companiesOfUser(org(), userId)) if (asInt(c.get("CompanyId")) == companyId) return true;
        return false;
    }

    /** BtnFetchRights_Click - tblUserRights.GetbyId(user, company); GridFill column set. */
    public List<Map<String, Object>> urRights(int userId, int companyId) {
        admin();
        if (!urUserKnown(userId) || !urCompanyKnown(userId, companyId)) return new ArrayList<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.userRights(userId, companyId)) {
            Map<String, Object> m = pick(r, "ModuleID", "RightName", "ModuleDescription", "ScreenName", "ScreenAlias", "RightsID", "ScreenID");
            m.put("Value", truthy(r.get("Value")));
            out.add(m);
        }
        return out;
    }

    /** btnSave_Click - see {@link AdmPanelRepository#saveUserRights}. */
    public Map<String, Object> urSave(Map<String, Object> body) {
        admin();
        int userId = asInt(body.get("userId")), companyId = asInt(body.get("companyId"));
        String userName = null;
        for (Map<String, Object> u : urUsers()) if (asInt(u.get("Id")) == userId) userName = str(u.get("UserName"));
        if (userName == null) throw new IllegalArgumentException("User Name Field is Required");
        if (!urCompanyKnown(userId, companyId)) throw new IllegalArgumentException("Company Field is Required");
        Set<String> allowed = new HashSet<>();
        for (Map<String, Object> r : repo.userRights(userId, companyId))
            allowed.add(asInt(r.get("ScreenID")) + ":" + asInt(r.get("RightsID")));
        List<int[]> rows = new ArrayList<>();
        Set<String> seen = new HashSet<>();
        for (Object o : list(body.get("rights"))) {
            Map<String, Object> m = map(o);
            int screen = asInt(m.get("screenId")), right = asInt(m.get("rightsId"));
            String key = screen + ":" + right;
            if (!allowed.contains(key) || !seen.add(key)) continue;
            rows.add(new int[] { screen, right, truthy(m.get("value")) ? 1 : 0 });
        }
        repo.saveUserRights(userId, comp(), companyId, rows);
        return ok("User Rights for " + userName + " has been updated Successfully!");
    }

    // ================================================================== 319 frmUserModuleAdmin

    private List<Map<String, Object>> umaUsers() {
        return repo.usersOfCompany(comp());
    }

    /** UsersGridFill - columns UserId (hidden) and UserName. */
    public List<Map<String, Object>> umaUsersGrid() {
        admin();
        return pickAll(umaUsers(), "UserId", "UserName");
    }

    private String umaUserName(int userId) {
        for (Map<String, Object> u : umaUsers()) if (asInt(u.get("UserId")) == userId) return str(u.get("UserName"));
        return null;
    }

    /** grdItemName_SelectionChanged - the unallocated rows take UserId / UserName from the selected user. */
    public Map<String, Object> umaGrids(int userId) {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        String name = umaUserName(userId);
        List<Map<String, Object>> un = new ArrayList<>(), al = new ArrayList<>();
        if (name != null) {
            for (Map<String, Object> row : repo.userModulesUnAllocated(org(), comp(), userId)) {
                Map<String, Object> m = pick(row, "UserModuleAdminId", "ModuleId", "ModuleName", "IsActive");
                m.put("UserId", userId);
                m.put("UserName", name);
                un.add(m);
            }
            al = pickAll(repo.userModulesAllocated(org(), comp(), userId), "UserModuleAdminId", "UserId", "UserName", "ModuleId", "ModuleName", "IsActive");
        }
        r.put("userName", name == null ? "" : name);
        r.put("unallocated", un);
        r.put("allocated", al);
        return r;
    }

    /** InsertTemplateII(grid, "Allocate" | "Update"). */
    public Map<String, Object> umaSave(Map<String, Object> body) {
        admin();
        int userId = asInt(body.get("userId"));
        String message = "Update".equals(str(body.get("mode"))) ? "Update" : "Allocate";
        Set<Integer> picked = new LinkedHashSet<>();
        for (Object o : list(body.get("moduleIds"))) picked.add(asInt(o));
        if (picked.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + message);
        if (umaUserName(userId) == null) throw new IllegalArgumentException("Check Row's Which You want To " + message);
        List<Map<String, Object>> source = message.equals("Update")
                ? repo.userModulesAllocated(org(), comp(), userId)
                : repo.userModulesUnAllocated(org(), comp(), userId);
        List<Map<String, Object>> items = new ArrayList<>();
        Timestamp t = now();
        for (Map<String, Object> r : source) {
            int moduleId = asInt(r.get("ModuleId"));
            if (!picked.remove(moduleId)) continue;
            items.add(params("IsActive", !message.equals("Update"), "EntryDate", t, "ModifyDate", t,
                    "CompanyId", comp(), "EntryUserId", uid(), "UserModuleAdminId", asLong(r.get("UserModuleAdminId")),
                    "UserId", userId, "ModifyUserId", uid(), "OrganizationId", org(), "ModuleId", moduleId));
        }
        if (items.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + message);
        repo.saveUserModules(items);
        return ok("Row's " + message + " Successfully");
    }

    private static long asLong(Object o) {
        if (o instanceof Number) return ((Number) o).longValue();
        try { return Long.parseLong(str(o).trim()); } catch (NumberFormatException e) { return 0L; }
    }

    // ================================================================== 315 frmModuleAllocateToOrganizationTemplates

    /** UnAllocatedModulesGridFill / AllocatedModulesGridFill - TemplateId of the unallocated rows is the selection. */
    public Map<String, Object> matModules(int templateId) {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        List<Map<String, Object>> un = new ArrayList<>();
        for (Map<String, Object> row : repo.templateModulesUnAllocated(templateId)) {
            Map<String, Object> m = pick(row, "Id", "TemplateName", "ModuleId", "ModuleName", "IsActive");
            m.put("TemplateId", templateId);
            un.add(m);
        }
        r.put("unallocated", un);
        r.put("allocated", pickAll(repo.templateModulesAllocated(templateId), "Id", "TemplateId", "TemplateName", "ModuleId", "ModuleName", "IsActive"));
        return r;
    }

    /** UnAllocatedScreensGridFill / AllocatedScreensGridFill (ScreensGridFill). */
    public Map<String, Object> matScreens(int templateId, int moduleId) {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        String[] keys = { "Id", "ModuleId", "ModuleName", "TemplateName", "ScreenId", "ScreenName", "ScreenAlias", "IsActive", "TargetUrl" };
        r.put("active", withLinks(repo.templateScreensUnAllocated(templateId, moduleId), keys));
        r.put("inactive", withLinks(repo.templateScreensAllocated(templateId, moduleId), keys));
        return r;
    }

    /** InsertTemplate(grid, SaveRecord). */
    public Map<String, Object> matSaveModules(Map<String, Object> body) {
        admin();
        int templateId = asInt(body.get("templateId"));
        boolean saveRecord = truthy(body.get("allocate"));
        String msg = saveRecord ? "Allocate" : "UnAllocate";
        Set<Integer> picked = new LinkedHashSet<>();
        for (Object o : list(body.get("moduleIds"))) picked.add(asInt(o));
        if (picked.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        if (!templateExists(templateId)) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        List<Map<String, Object>> source = saveRecord ? repo.templateModulesUnAllocated(templateId) : repo.templateModulesAllocated(templateId);
        List<Map<String, Object>> items = new ArrayList<>();
        Timestamp t = now();
        for (Map<String, Object> r : source) {
            int moduleId = asInt(r.get("ModuleId"));
            if (!picked.remove(moduleId)) continue;
            items.add(params("IsActive", saveRecord, "EntryDate", t, "ModifyDate", t, "EntryUserId", uid(),
                    "Id", asInt(r.get("Id")), "ModifyUserId", uid(), "ModuleId", moduleId, "OrganizationTemplateId", templateId));
        }
        if (items.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        repo.saveTemplateModules(items);
        return ok("Row's " + msg + " Successfully");
    }

    /** InsertScreen(grid, SaveRecord) - the desktop message still says "Modules". */
    public Map<String, Object> matSaveScreens(Map<String, Object> body) {
        admin();
        int templateId = asInt(body.get("templateId")), moduleId = asInt(body.get("moduleId"));
        boolean saveRecord = truthy(body.get("allocate"));
        String msg = saveRecord ? "Allocate" : "UnAllocate";
        Set<String> picked = new LinkedHashSet<>();
        for (Object o : list(body.get("screens"))) {
            Map<String, Object> m = map(o);
            picked.add(asInt(m.get("moduleId")) + "|" + str(m.get("screenName")));
        }
        if (picked.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        if (!templateExists(templateId)) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        List<Map<String, Object>> source = saveRecord
                ? repo.templateScreensUnAllocated(templateId, moduleId) : repo.templateScreensAllocated(templateId, moduleId);
        List<Map<String, Object>> items = new ArrayList<>();
        Timestamp t = now();
        for (Map<String, Object> r : source) {
            String key = asInt(r.get("ModuleId")) + "|" + str(r.get("ScreenName"));
            if (!picked.remove(key)) continue;
            items.add(params("IsActive", saveRecord, "EntryDate", t, "ModifyDate", t, "EntryUserId", uid(),
                    "Id", asInt(r.get("Id")), "ModifyUserId", uid(), "ModuleId", asInt(r.get("ModuleId")),
                    "OrganizationTemplateId", templateId, "ScreenName", str(r.get("ScreenName"))));
        }
        if (items.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        repo.saveTemplateScreens(items);
        return ok("Row's " + msg + " Successfully");
    }

    // ================================================================== 316 frmScreensAllocateToCompany

    /** BtnShow_Click -> CompanyGridFill(TemplateId): Company.GetCompaniesByTemplateId, columns Id and CompanyName. */
    public List<Map<String, Object>> sacCompanies(int templateId) {
        admin();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.companiesByTemplate(templateId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(r.get("Id")));
            m.put("CompanyName", r.get("CompName"));
            out.add(m);
        }
        return out;
    }

    /** AllocatedModulesGridFill - ModuleAllocateToOrganizationTemplates_AllocatedData(TemplateId). */
    public List<Map<String, Object>> sacModules(int templateId) {
        admin();
        return pickAll(repo.templateModulesAllocated(templateId), "Id", "TemplateId", "TemplateName", "ModuleId", "ModuleName", "IsActive");
    }

    /** grdModules_SelectionChanged - the company id is the selected company on both grids. */
    public Map<String, Object> sacScreens(int templateId, int companyId, int moduleId) {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        String[] keys = { "Id", "CompanyName", "ScreenId", "ScreenName", "ScreenAlias", "IsActive", "TargetUrl" };
        boolean known = sacCompanyKnown(templateId, companyId);
        List<Map<String, Object>> un = new ArrayList<>(), al = new ArrayList<>();
        if (known) {
            un = withLinks(repo.companyScreensUnAllocated(companyId, moduleId), keys);
            al = withLinks(repo.companyScreensAllocated(companyId, moduleId), keys);
        }
        r.put("unallocated", un);
        r.put("allocated", al);
        return r;
    }

    private boolean sacCompanyKnown(int templateId, int companyId) {
        for (Map<String, Object> c : repo.companiesByTemplate(templateId)) if (asInt(c.get("Id")) == companyId) return true;
        return false;
    }

    /** Insert(grid, SaveRecord) of frmScreensAllocateToCompany. */
    public Map<String, Object> sacSave(Map<String, Object> body) {
        admin();
        int templateId = asInt(body.get("templateId")), companyId = asInt(body.get("companyId")), moduleId = asInt(body.get("moduleId"));
        boolean saveRecord = truthy(body.get("allocate"));
        String msg = saveRecord ? "Allocate" : "UnAllocate";
        Set<String> picked = new LinkedHashSet<>();
        for (Object o : list(body.get("screenNames"))) picked.add(str(o));
        if (picked.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        if (!sacCompanyKnown(templateId, companyId)) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        List<Map<String, Object>> source = saveRecord
                ? repo.companyScreensUnAllocated(companyId, moduleId) : repo.companyScreensAllocated(companyId, moduleId);
        List<Map<String, Object>> items = new ArrayList<>();
        Timestamp t = now();
        for (Map<String, Object> r : source) {
            String name = str(r.get("ScreenName"));
            if (!picked.remove(name)) continue;
            items.add(params("EntryUserId", uid(), "ModifyUserId", uid(), "EntryDate", t, "ModifyDate", t,
                    "CompanyId", companyId, "Id", asInt(r.get("Id")), "IsActive", saveRecord, "ScreenName", name));
        }
        if (items.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + msg);
        repo.saveCompanyScreens(items);
        return ok("Row's " + msg + " Successfully");
    }

    // ================================================================== 314 AllocationOrganizationTemplateRights

    /** Load: TemplateNameFill + AppModuleFill. */
    public Map<String, Object> trSetup() {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("templates", pickAll(repo.templates(), "Id", "TemplateName"));
        r.put("modules", pickAll(repo.appModules(), "Id", "ModuleDescription"));
        return r;
    }

    /** ShowData - UnAllocatedItemsGridFill (ActionId 1) and AllocatedItemsGridFill (ActionId 2). */
    public Map<String, Object> trScreens(int templateId, int moduleId) {
        admin();
        if (templateId <= 0) throw new IllegalArgumentException("Select Template Name First");
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("unallocated", screenIdName(repo.templateRightsScreens(templateId, 1, moduleId)));
        r.put("allocated", screenIdName(repo.templateRightsScreens(templateId, 2, moduleId)));
        return r;
    }

    private static List<Map<String, Object>> screenIdName(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ScreenId", asInt(r.get("ScreenId")));
            m.put("ScreenName", r.get("ScreenAlias"));
            out.add(m);
        }
        return out;
    }

    private List<Integer> trPicked(Map<String, Object> body, int templateId, int moduleId, int actionId, String emptyMessage) {
        Set<Integer> picked = new LinkedHashSet<>();
        for (Object o : list(body.get("screenIds"))) picked.add(asInt(o));
        if (picked.isEmpty()) throw new IllegalArgumentException(emptyMessage);
        List<Integer> ids = new ArrayList<>();
        for (Map<String, Object> r : repo.templateRightsScreens(templateId, actionId, moduleId)) {
            int id = asInt(r.get("ScreenId"));
            if (picked.remove(id)) ids.add(id);
        }
        if (ids.isEmpty()) throw new IllegalArgumentException(emptyMessage);
        return ids;
    }

    /** BtnAllocateItems_Click. */
    public Map<String, Object> trAllocate(Map<String, Object> body) {
        admin();
        int templateId = asInt(body.get("templateId")), moduleId = asInt(body.get("moduleId"));
        if (templateId == 0) throw new IllegalArgumentException("Select Template Name First");
        List<Integer> ids = trPicked(body, templateId, moduleId, 1, "Checked Row's first To Allocate Items");
        repo.saveTemplateRights(templateId, ids);
        return ok("Screen's Allocated Successfully");
    }

    /** btnDeAllocate_Click - the confirmation "Are you sure to UnAllocate Item's?" is asked by the page. */
    public Map<String, Object> trDeallocate(Map<String, Object> body) {
        admin();
        int templateId = asInt(body.get("templateId")), moduleId = asInt(body.get("moduleId"));
        if (templateId == 0) throw new IllegalArgumentException("Select Product Type First");
        List<Integer> ids = trPicked(body, templateId, moduleId, 2, "Checked Row's first To Un-Allocate Items");
        StringBuilder sb = new StringBuilder();
        for (Integer id : ids) sb.append(id).append(',');
        repo.deleteTemplateRights(templateId, sb.toString());
        return ok("Screen's UnAllocated Successfully");
    }

    // ================================================================== 312 DefineCompany

    private static final String[] HISTORY_COLUMNS = {
            "Id", "OrgCompanyTypeId", "OrganizationName", "CompCode", "CompName", "CompContactPerson", "FirstName", "LastName",
            "CompEmailA", "CompEmailB", "CompTel", "CompMobileA", "CompMobileB", "CompMobileC", "CompAddress",
            "CompReportingTitle", "CompLogo", "CompCountry", "CompState", "CompBaseCurr", "CompType", "EntryUser", "EntryDate",
            "ModifyUser", "ModifyDate", "AllowedUserCount", "CompanyTypeId", "IsHeadOffice", "CompanyNameOtherLanguage",
            "CompanyAddressOtherLanguage", "CompanyWebsite", "CompanyFaxNo", "CompanyTemplateId", "CityName", "CompanyId" };

    /** HistoryGridFill - Company.FormHistory(0); OrgName is shown as OrganizationName. */
    public List<Map<String, Object>> dcHistory() {
        admin();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.companyHistory()) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : HISTORY_COLUMNS) m.put(c, c.equals("OrganizationName") ? r.get("OrgName") : r.get(c));
            out.add(m);
        }
        return out;
    }

    /** Load: OrganizationFill + TemplateNameFill + HistoryGridFill. */
    public Map<String, Object> dcSetup() {
        admin();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("organizations", pickAll(repo.organizations(), "Id", "OrgName"));
        r.put("templates", pickAll(repo.templates(), "Id", "TemplateName"));
        r.put("history", dcHistory());
        return r;
    }

    /** CmbOrganizationName_Leave - Organization.GetByID. */
    public Map<String, Object> dcOrganization(int id) {
        admin();
        Map<String, Object> o = id > 0 ? repo.organization(id) : null;
        if (o == null) return new LinkedHashMap<>();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("OrgReportingTitle", str(o.get("OrgReportingTitle")));
        m.put("OrgState", str(o.get("OrgState")));
        m.put("OrgCountry", str(o.get("OrgCountry")));
        m.put("OrgContactPerson", str(o.get("OrgContactPerson")));
        m.put("OrgEmailA", str(o.get("OrgEmailA")));
        m.put("OrgTel", str(o.get("OrgTel")));
        m.put("OrganizaionTemplateId", asInt(o.get("OrganizaionTemplateId")));
        return m;
    }

    /** ReadById(Id) - Company.GetByID; the logo is served by {@link #dcLogo(int)}. */
    public Map<String, Object> dcCompany(int id) {
        admin();
        Map<String, Object> c = repo.company(id);
        if (c == null) return new LinkedHashMap<>();
        Map<String, Object> m = pick(c, "Id", "OrgCompanyTypeId", "CompCode", "CompName", "CompanyNameOtherLanguage",
                "CompReportingTitle", "CompContactPerson", "FirstName", "LastName", "CompanyWebsite", "CompAddress",
                "CompanyAddressOtherLanguage", "CompEmailA", "CompEmailB", "CompTel", "CompMobileA", "CompMobileB",
                "CompMobileC", "CompanyFaxNo", "CompBaseCurr", "AllowedUserCount", "CompanyId", "CompCountry", "CompState",
                "CityName", "CompanyTemplateId");
        Object logo = c.get("CompLogoImage");
        m.put("hasLogo", logo instanceof byte[] && ((byte[]) logo).length > 0);
        return m;
    }

    public byte[] dcLogo(int id) {
        admin();
        Map<String, Object> c = repo.company(id);
        Object logo = c == null ? null : c.get("CompLogoImage");
        if (!(logo instanceof byte[]) || ((byte[]) logo).length == 0) throw new IllegalArgumentException("No picture.");
        return (byte[]) logo;
    }

    private String need(Map<String, Object> b, String key, String message) {
        String v = str(b.get(key));
        if (v.trim().isEmpty()) throw new IllegalArgumentException(message);
        return v;
    }

    /** FormValidation - the same order and wording. */
    private void dcValidate(Map<String, Object> b) {
        int orgId = asInt(b.get("organizationId"));
        boolean orgOk = false;
        if (orgId != 0) for (Map<String, Object> o : repo.organizations()) if (asInt(o.get("Id")) == orgId) orgOk = true;
        if (!orgOk) throw new IllegalArgumentException("Organization Field Required");
        need(b, "code", "Company Code Field Required");
        need(b, "name", "Company Name Field Required");
        need(b, "reportingTitle", "Reporting Title Field Required");
        need(b, "email", "Email Field Required");
        need(b, "phone", "Phone Number Field Required");
        need(b, "mobile01", "Mobile 01 Field Required");
        need(b, "address", "Address Field Required");
        need(b, "country", "Country Field Required");
        need(b, "state", "State Field Required");
        need(b, "city", "City Field Required");
        need(b, "baseCurrency", "Base Currency Field Required");
        need(b, "contactPerson", "Contact Person Field Required");
        String cnt = str(b.get("allowedUsers")).trim();
        if (cnt.isEmpty() || asInt(cnt) == 0) throw new IllegalArgumentException("Allowed User Count Field Required");
    }

    private byte[] dcLogoBytes(Map<String, Object> b, int recId) {
        String data = str(b.get("logo"));
        if (!data.isEmpty()) {
            int comma = data.indexOf(',');
            byte[] bytes;
            try { bytes = Base64.getDecoder().decode(comma >= 0 ? data.substring(comma + 1) : data); }
            catch (IllegalArgumentException e) { throw new IllegalArgumentException("Parameter is not valid."); }
            if (bytes.length > 5000000) throw new IllegalArgumentException("File Size Limit Exceeded");
            boolean png = bytes.length > 4 && (bytes[0] & 0xFF) == 0x89 && bytes[1] == 'P';
            boolean jpg = bytes.length > 3 && (bytes[0] & 0xFF) == 0xFF && (bytes[1] & 0xFF) == 0xD8;
            boolean gif = bytes.length > 3 && bytes[0] == 'G' && bytes[1] == 'I' && bytes[2] == 'F';
            boolean bmp = bytes.length > 2 && bytes[0] == 'B' && bytes[1] == 'M';
            if (!(png || jpg || gif || bmp)) throw new IllegalArgumentException("Parameter is not valid.");
            return bytes;
        }
        if (truthy(b.get("keepLogo")) && recId > 0) {
            Map<String, Object> c = repo.company(recId);
            Object logo = c == null ? null : c.get("CompLogoImage");
            if (logo instanceof byte[] && ((byte[]) logo).length > 0) return (byte[]) logo;
        }
        return null;
    }

    /** InsertOnlyForSave. */
    public Map<String, Object> dcSave(Map<String, Object> b) {
        admin();
        dcValidate(b);
        need(b, "firstName", "First Name Field Required");
        need(b, "lastName", "Last Name Field Required");
        Timestamp t = now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", 0);
        m.put("CompanyTemplateId", asInt(b.get("templateId")));
        m.put("CompanyId", asInt(b.get("companyId")));
        m.put("FirstName", str(b.get("firstName")));
        m.put("LastName", str(b.get("lastName")));
        m.put("CityName", str(b.get("city")));
        m.put("IsHeadOffice", false);
        m.put("EntryDate", t);
        m.put("ModifyDate", t);
        m.put("AllowedUserCount", asInt(b.get("allowedUsers")));
        m.put("CompanyTypeId", 0);
        m.put("EntryUser", uid());
        m.put("ModifyUser", uid());
        m.put("OrgCompanyTypeId", asInt(b.get("organizationId")));
        m.put("CompAddress", str(b.get("address")));
        m.put("CompanyAddressOtherLanguage", str(b.get("addressOtherLang")));
        m.put("CompanyFaxNo", str(b.get("fax")));
        m.put("CompanyNameOtherLanguage", str(b.get("nameOtherLang")));
        m.put("CompanyNameOtherLing", str(b.get("nameOtherLang")));
        m.put("CompanyWebsite", str(b.get("website")));
        m.put("CompBaseCurr", str(b.get("baseCurrency")));
        m.put("CompCode", str(b.get("code")));
        m.put("CompContactPerson", str(b.get("contactPerson")));
        m.put("CompCountry", str(b.get("country")));
        m.put("CompEmailA", str(b.get("email")));
        m.put("CompEmailB", str(b.get("email02")));
        m.put("CompMobileA", str(b.get("mobile01")));
        m.put("CompMobileB", str(b.get("mobile02")));
        m.put("CompMobileC", str(b.get("mobile03")));
        m.put("CompName", str(b.get("name")));
        m.put("CompReportingTitle", str(b.get("reportingTitle")));
        m.put("CompState", str(b.get("state")));
        m.put("CompTel", str(b.get("phone")));
        byte[] logo = dcLogoBytes(b, 0);
        if (logo != null) m.put("CompLogoImage", logo);
        repo.registerCompany(m, (name, to) -> getKey(name, to));
        return ok("Save Successfully");
    }

    /** Insert() with RecId &gt; 0. */
    public Map<String, Object> dcUpdate(Map<String, Object> b) {
        admin();
        int recId = asInt(b.get("recId"));
        if (recId <= 0) throw new IllegalArgumentException("RecId Not Found...");
        dcValidate(b);
        Timestamp t = now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", recId);
        m.put("OrgCompanyTypeId", asInt(b.get("organizationId")));
        m.put("CompAddress", str(b.get("address")));
        m.put("CompBaseCurr", str(b.get("baseCurrency")));
        m.put("CompCode", str(b.get("code")));
        m.put("CompContactPerson", str(b.get("contactPerson")));
        m.put("CompCountry", str(b.get("country")));
        m.put("CompEmailA", str(b.get("email")));
        m.put("CompEmailB", str(b.get("email02")));
        m.put("CompMobileA", str(b.get("mobile01")));
        m.put("CompMobileB", str(b.get("mobile02")));
        m.put("CompMobileC", str(b.get("mobile03")));
        m.put("CompName", str(b.get("name")));
        m.put("CompReportingTitle", str(b.get("reportingTitle")));
        m.put("CompState", str(b.get("state")));
        m.put("CompTel", str(b.get("phone")));
        m.put("EntryUser", uid());
        m.put("ModifyUser", uid());
        byte[] logo = dcLogoBytes(b, recId);
        if (logo != null) m.put("CompLogoImage", logo);
        m.put("EntryDate", t);
        m.put("ModifyDate", t);
        m.put("CityName", str(b.get("city")));
        m.put("CompanyTemplateId", asInt(b.get("templateId")));
        m.put("CompanyId", asInt(b.get("companyId")));
        m.put("CompanyNameOtherLanguage", str(b.get("nameOtherLang")));
        m.put("CompanyNameOtherLing", str(b.get("nameOtherLang")));
        m.put("CompanyAddressOtherLanguage", str(b.get("addressOtherLang")));
        m.put("CompanyWebsite", str(b.get("website")));
        m.put("AllowedUserCount", asInt(b.get("allowedUsers")));
        m.put("CompanyFaxNo", str(b.get("fax")));
        repo.updateCompany(m);
        return ok("Update Successfully");
    }

    /**
     * DAL 0056 GetKey(CompanyName, ToDate): AES over UTF-16LE(CompanyName + "(" + dd-MMM-yyyy), key and IV from one
     * Rfc2898DeriveBytes stream (GetBytes(32) then GetBytes(16)) - the exact inverse of DesktopLicenseKey.returnKey.
     */
    static String getKey(String companyName, LocalDate toDate) {
        try {
            byte[] salt = { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118 };
            byte[] derived = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1")
                    .generateSecret(new PBEKeySpec("AMIR2SPSCS44602".toCharArray(), salt, 1000, 48 * 8)).getEncoded();
            Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
            cipher.init(Cipher.ENCRYPT_MODE, new SecretKeySpec(Arrays.copyOfRange(derived, 0, 32), "AES"),
                    new IvParameterSpec(Arrays.copyOfRange(derived, 32, 48)));
            String text = companyName + "(" + toDate.format(DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH));
            return Base64.getEncoder().encodeToString(cipher.doFinal(text.getBytes(StandardCharsets.UTF_16LE)));
        } catch (Exception e) {
            throw new IllegalStateException("License key could not be created.", e);
        }
    }

    // ================================================================== 791 frmScreenDefinetion

    private static final String TYPES_RESOURCE = "/adm/desktop_types.txt";
    private volatile Set<String> desktopTypes;

    /** Type.GetType(TargetUrl) != null - emulated with the class list recovered from the desktop assembly. */
    private boolean typeConfigured(String targetUrl) {
        Set<String> t = desktopTypes;
        if (t == null) {
            Set<String> s = new HashSet<>();
            try (InputStream in = AdmPanelService.class.getResourceAsStream(TYPES_RESOURCE)) {
                if (in != null) {
                    for (String line : new String(in.readAllBytes(), StandardCharsets.UTF_8).split("\\R")) {
                        String x = line.trim();
                        if (!x.isEmpty()) s.add(x);
                    }
                }
            } catch (Exception e) { /* empty list: every URL fails the check, as on a machine without the assembly */ }
            desktopTypes = t = s;
        }
        return t.contains(targetUrl);
    }

    /** Load: ScreenDefinationGrd, ScreenRightsGrd, AppModuleFill, GrdDocTypeFill, DocumentTypeFill, CompanyNameComboFill, Applications. */
    public Map<String, Object> sdSetup() {
        UserAccount u = admin();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("screens", sdScreens());
        r.put("modules", pickAll(repo.appModules(), "Id", "ModuleDescription"));
        r.put("companies", pickAll(repo.allCompanies(), "Id", "CompName"));
        r.put("loginCompanyId", comp());
        r.put("rights", pickAll(repo.rightNames(), "Id", "RightsName"));
        r.put("applications", pickAll(repo.applications(), "Id", "AppName", "MasterApp", "MasterAppId"));
        r.put("documentTypes", sdDocumentTypes());
        r.put("methods", sdMethods());
        return r;
    }

    public List<Map<String, Object>> sdScreens() {
        admin();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.screenDefinitions()) {
            Map<String, Object> m = pick(r, "Id", "ScreenName", "ScreenAlias", "ModuleId", "TargetUrl", "SortNo", "AppId");
            m.put("Active", truthy(r.get("IsActive")));
            m.put("url", null);
            try { m.put("url", screenRouteIndex.routeFor(str(r.get("ScreenName")), str(r.get("TargetUrl")))); } catch (RuntimeException e) { /* no link */ }
            out.add(m);
        }
        return out;
    }

    /** RetrivedData(Id). */
    public Map<String, Object> sdById(int id) {
        admin();
        Map<String, Object> s = repo.screenDefinition(id);
        if (s == null) return new LinkedHashMap<>();
        Map<String, Object> m = pick(s, "Id", "ScreenName", "ScreenAlias", "ModuleId", "TargetUrl", "SortNo", "AppId");
        m.put("IsActive", truthy(s.get("IsActive")));
        List<Object> rights = new ArrayList<>();
        for (Map<String, Object> r : repo.screenRightsOf(id)) rights.add(r.get("RightName"));
        List<Object> apps = new ArrayList<>();
        for (Map<String, Object> a : repo.applicationsOfScreen(id)) apps.add(asInt(a.get("AppId")));
        m.put("rights", rights);
        m.put("applications", apps);
        return m;
    }

    /** Insert() of the Screen Definition tab. */
    public Map<String, Object> sdSave(Map<String, Object> b) {
        admin();
        int recId = asInt(b.get("recId"));
        int moduleId = asInt(b.get("moduleId"));
        boolean moduleOk = false;
        if (moduleId != 0) for (Map<String, Object> mo : repo.appModules()) if (asInt(mo.get("Id")) == moduleId) moduleOk = true;
        if (!moduleOk) throw new IllegalArgumentException("Module Required!");
        String screenName = str(b.get("screenName")), alias = str(b.get("screenAlias")), target = str(b.get("targetUrl"));
        if (screenName.isEmpty()) throw new IllegalArgumentException("Screen Name Required!");
        if (alias.isEmpty()) throw new IllegalArgumentException("Screen Alias Required!");
        int appId = asInt(b.get("appId"));
        if (!(truthy(b.get("notValidateUrl")) && truthy(b.get("notValidateVisible"))) && !typeConfigured(target) && appId != 3)
            throw new IllegalArgumentException("Target URL not Configured properly... ");
        boolean haveView = false;
        Set<String> knownRights = new LinkedHashSet<>();
        for (Map<String, Object> r : repo.rightNames()) {
            knownRights.add(str(r.get("RightsName")));
            if ("View".equals(str(r.get("RightsName")))) haveView = true;
        }
        if (!haveView) throw new IllegalArgumentException("View Right For Screen Required....");

        Map<Integer, String> companyNames = new LinkedHashMap<>();
        for (Map<String, Object> c : repo.allCompanies()) companyNames.put(asInt(c.get("Id")), str(c.get("CompName")));
        StringBuilder companyIds = new StringBuilder();
        int companyCount = 0;
        for (Object o : list(b.get("companyIds"))) {
            int cid = asInt(o);
            if (companyNames.containsKey(cid)) { companyIds.append(',').append(cid); companyCount++; }
        }
        if (companyCount == 0) throw new IllegalArgumentException("Select Company first");

        List<String> rights = new ArrayList<>();
        for (Object o : list(b.get("rights"))) {
            String name = str(o);
            if (knownRights.contains(name) && !rights.contains(name)) rights.add(name);
        }
        Set<Integer> knownApps = new HashSet<>();
        for (Map<String, Object> a : repo.applications()) knownApps.add(asInt(a.get("Id")));
        List<Integer> apps = new ArrayList<>();
        for (Object o : list(b.get("applications"))) {
            int a = asInt(o);
            if (knownApps.contains(a) && !apps.contains(a)) apps.add(a);
        }
        if (rights.isEmpty()) throw new IllegalArgumentException("Please Check the Screen Rights");
        if (apps.isEmpty()) throw new IllegalArgumentException("Please Check the Applications");

        if (recId > 0 && repo.screenDefinition(recId) == null) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> model = new LinkedHashMap<>();
        model.put("IsCreateSMSTemplate", false);
        model.put("Id", recId > 0 ? recId : 0);
        model.put("ModuleId", moduleId);
        model.put("ScreenAlias", alias);
        model.put("ScreenName", screenName);
        model.put("TargetUrl", target);
        model.put("IsActive", truthy(b.get("isActive")));
        model.put("SortNo", asInt(b.get("sortNo")));
        model.put("AppId", appId);
        model.put("CompanyIds", companyIds.toString());
        repo.saveScreenDefinition(model, rights, apps);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully");
    }

    /** toolStripButton1_Click - "Update_Sorting & Screen Alias": every grid row, one call each. */
    public Map<String, Object> sdSorting(Map<String, Object> b) {
        admin();
        Map<Integer, Map<String, Object>> existing = new LinkedHashMap<>();
        for (Map<String, Object> r : repo.screenDefinitions()) existing.put(asInt(r.get("Id")), r);
        List<Map<String, Object>> items = new ArrayList<>();
        for (Object o : list(b.get("rows"))) {
            Map<String, Object> m = map(o);
            int id = asInt(m.get("Id"));
            if (!existing.containsKey(id)) continue;
            Map<String, Object> it = new LinkedHashMap<>();
            it.put("ScreenAlias", str(m.get("ScreenAlias")));
            it.put("SortNo", asInt(m.get("SortNo")));
            it.put("Id", id);
            it.put("IsActive", truthy(m.get("Active")));
            items.add(it);
        }
        repo.screenSortingAndAliasUpdate(items);
        return ok("Record Update Successfully");
    }

    // ---- Document Types tab

    public List<Map<String, Object>> sdDocumentTypes() {
        admin();
        return pickAll(repo.documentTypes(), "Id", "DocumentTypeCode", "DocumentTypeDescription",
                "DocumentTypeDescriptionOtherLing", "ScreenName", "TargetUrl", "Remarks", "EntryDate", "EntryUserName",
                "ModifyDate", "ModifyUserName", "ApprovedDate", "ApprovedUserName");
    }

    /** InsertDocType. */
    public Map<String, Object> sdSaveDocType(Map<String, Object> b) {
        admin();
        int recId = asInt(b.get("recId"));
        String idText = str(b.get("id"));
        if (idText.isEmpty() || asInt(idText) == 0) throw new IllegalArgumentException("DocTypeId Required!");
        if (str(b.get("code")).isEmpty()) throw new IllegalArgumentException("DocType Code Required!");
        if (str(b.get("screenName")).isEmpty()) throw new IllegalArgumentException("Screen Name Required!");
        if (str(b.get("targetUrl")).isEmpty()) throw new IllegalArgumentException("Target Url Required!");
        Timestamp t = now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("PostState", false);
        m.put("Status", false);
        m.put("EntryDate", t);
        m.put("ModifyDate", t);
        m.put("PostDate", t);
        m.put("CompanyId", comp());
        m.put("ControlAccountId", 0);
        m.put("EntryUser", uid());
        m.put("Id", asInt(idText));
        m.put("ModifyUser", uid());
        m.put("OrganizationId", org());
        m.put("PostUser", 0);
        m.put("SortNo", 0);
        m.put("DocumentTypeCode", str(b.get("code")));
        m.put("DocumentTypeDescription", str(b.get("description")));
        m.put("DocumentTypeDescriptionOtherLing", str(b.get("otherLangDescription")));
        m.put("Remarks", str(b.get("remarks")));
        m.put("ScreenName", str(b.get("screenName")));
        m.put("TargetUrl", str(b.get("targetUrl")));
        repo.saveDocumentType(m);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully");
    }

    // ---- Define Methods tab

    public List<Map<String, Object>> sdMethods() {
        admin();
        return pickAll(repo.reportsMethods(), "Id", "RefDocumentTypeId", "DocumentTypeCode", "DocumentTypeDescription",
                "ScreenName", "ReportTypeId", "ReportMethod", "Remarks");
    }

    /** InsertMethodDefine. */
    public Map<String, Object> sdSaveMethod(Map<String, Object> b) {
        admin();
        int recId = asInt(b.get("recId"));
        int docType = asInt(b.get("documentTypeId"));
        boolean known = false;
        if (docType != 0) for (Map<String, Object> d : repo.documentTypes()) if (asInt(d.get("Id")) == docType) known = true;
        if (!known) throw new IllegalArgumentException("Document Type Required!");
        if (str(b.get("screenName")).isEmpty()) throw new IllegalArgumentException("Screen Name Required!");
        if (str(b.get("reportMethod")).isEmpty()) throw new IllegalArgumentException("Report Method Required!");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", recId > 0 ? recId : 0);
        m.put("RefDocumentTypeId", docType);
        m.put("ReportTypeId", 1);
        m.put("Remarks", str(b.get("remarks")));
        m.put("ReportMethod", str(b.get("reportMethod")));
        m.put("ScreenName", str(b.get("screenName")));
        repo.saveReportsMethod(m);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully");
    }
}
