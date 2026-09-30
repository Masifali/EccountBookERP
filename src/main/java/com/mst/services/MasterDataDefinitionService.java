package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.MasterDataDefinitionRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * BLL of the nine "Master Data Definition" screens (App 19, module 2039 System_Level). Each save
 * repeats the form's FormValidation with the desktop's wording and order, builds the model exactly
 * as the form and its BLL build it, and sends every property to the procedure. Tenancy and audit
 * (Organization, Company, user id, dates) come from the session, never from the request.
 *
 * Rights: only SeaPortsDefine checks rights on the desktop (SetRightsValueInRightsObject
 * "frmExImSeaPorts" -> Save/Update buttons). The other forms have no rights code; here every page
 * still needs View on its own dbo.ScreenDefinition row, as the menu that opens it does.
 *
 * Return values mirror what the MessageBox shows: the Insert's SCOPE_IDENTITY, or 0 for an Update
 * (no result set) - so "Record Update Successfully...[n]" is shown only where the form does not test
 * success >= 1 (City), and not at all where it does (Country, Province, Currency).
 */
@Service
public class MasterDataDefinitionService {

    public static final int SCREEN_COUNTRY = 749, SCREEN_CITY = 750, SCREEN_CURRENCY = 751, SCREEN_SEA_PORTS = 752,
            SCREEN_PROVINCE = 753, SCREEN_DATE_LOCK = 754, SCREEN_TEHSIL = 755, SCREEN_DISTRICT = 756, SCREEN_OTHER_ITEMS = 742;

    @Autowired private MasterDataDefinitionRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screen) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; } catch (AccessDeniedException ex) { return false; }
    }

    // ================================================================= 749 Country (DefineCountry.cs)

    /** CounytryDefineGridFill - country.GetAll(new Country()). */
    public List<Map<String, Object>> countries() { user(SCREEN_COUNTRY); return repo.countries(); }

    /** grdcountrydefine_DoubleClick - country.GetById(RecId). */
    public Map<String, Object> country(int id) { user(SCREEN_COUNTRY); return one(repo.country(id)); }

    /**
     * saveCountry :45 / btnUpdate_Click :120. The insert sets EntryUser and leaves ModifyUser 0; the
     * update sets ModifyUser and leaves EntryUser 0 - each is what the form's initializer writes.
     */
    public Map<String, Object> saveCountry(Map<String, Object> b) {
        UserAccount u = user(SCREEN_COUNTRY);
        int id = asInt(b.get("id"));
        String code = raw(b.get("code")), description = raw(b.get("description"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("Country Code Field Required");
        if (description.trim().isEmpty()) throw new IllegalArgumentException("Country Name Field Required");
        if (id > 0 && repo.country(id).isEmpty()) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Code", code);
        m.put("@Description", description);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", id == 0 ? u.getId() : 0);
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", id == 0 ? 0 : u.getId());
        m.put("@PostDate", ts(now));
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        return result(repo.saveCountry(m));
    }

    // ================================================================= 753 Province (DefineProvince.cs)

    /** DefineProvince_Load - CountryComboFill (country.GetAll) and ProvienceDefineGridFill (StateProvince.GetAll). */
    public Map<String, Object> provinceSetup() {
        UserAccount u = user(SCREEN_PROVINCE);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("countries", repo.countries()); } catch (Exception e) { out.put("countriesError", msg(e)); }
        out.put("provinces", repo.provinces(u));
        return out;
    }

    public List<Map<String, Object>> provinces() { return repo.provinces(user(SCREEN_PROVINCE)); }

    /** grdProviencedefine_DoubleClick - StateProvince.GetByID(RecId). */
    public Map<String, Object> province(int id) {
        UserAccount u = user(SCREEN_PROVINCE);
        if (!owns(repo.provinces(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.province(id));
    }

    /**
     * btnsave_Click :121 / btnUpdate_Click :90 -> StateProvince.Save, which overwrites EntryDate,
     * EntryUser, ModifyDate, ModifyUser, OrganizationId and CompanyId from the signed-in user
     * (BLL 0083 :14-19) before choosing Insert or Update.
     */
    public Map<String, Object> saveProvince(Map<String, Object> b) {
        UserAccount u = user(SCREEN_PROVINCE);
        int id = asInt(b.get("id"));
        String code = raw(b.get("code")), description = raw(b.get("description"));
        int countryId = asInt(b.get("countryId"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("Provience Code Field Required");
        if (description.trim().isEmpty()) throw new IllegalArgumentException("Provience Name Field Required");
        if (countryId <= 0 || !owns(repo.countries(), countryId)) throw new IllegalArgumentException("Country Field Required");
        if (id > 0 && !owns(repo.provinces(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Code", code);
        m.put("@Description", description);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", u.getId());
        m.put("@PostDate", ts(now));
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@CountryId", countryId);
        return result(repo.saveProvince(m));
    }

    // ================================================================= 756 District (DefineDistrict.cs)

    /** DefineCountry_Load - StateProvinceBind (StateProvince.GetAll) and FormHistory (District.GetAll). */
    public Map<String, Object> districtSetup() {
        UserAccount u = user(SCREEN_DISTRICT);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("provinces", repo.provinces(u)); } catch (Exception e) { out.put("provincesError", msg(e)); }
        try { out.put("districts", repo.districts(u)); } catch (Exception e) { out.put("districtsError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> districts() { return repo.districts(user(SCREEN_DISTRICT)); }

    /** Insert() :55 - Code = Description = text; EntryUser set, ModifyUser left 0 (the form never sets it). */
    public Map<String, Object> saveDistrict(Map<String, Object> b) {
        UserAccount u = user(SCREEN_DISTRICT);
        int id = asInt(b.get("id"));
        String name = raw(b.get("name"));
        int provinceId = asInt(b.get("stateProvinceId"));
        if (provinceId <= 0 || !owns(repo.provinces(u), provinceId)) throw new IllegalArgumentException("StateProvince Field is Required");
        if (name.trim().isEmpty()) throw new IllegalArgumentException("District Name Field is Required");
        if (id > 0 && !owns(repo.districts(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Code", name);
        m.put("@Description", name);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", 0);
        m.put("@PostDate", ts(now));
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@StateProvinceId", provinceId);
        return result(repo.saveDistrict(m));
    }

    // ================================================================= 755 Tehsil (DefineTehsil.cs)

    /** DefineCountry_Load - DistrictBind (District.GetAll) and FormHistory (Tehsil.GetAll). */
    public Map<String, Object> tehsilSetup() {
        UserAccount u = user(SCREEN_TEHSIL);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("districts", repo.districts(u)); } catch (Exception e) { out.put("districtsError", msg(e)); }
        try { out.put("tehsils", repo.tehsils(u)); } catch (Exception e) { out.put("tehsilsError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> tehsils() { return repo.tehsils(user(SCREEN_TEHSIL)); }

    /** btnRefresh_Click - DistrictBind. */
    public List<Map<String, Object>> districtsForTehsil() { return repo.districts(user(SCREEN_TEHSIL)); }

    /** Insert() :55 - Code = Description = text; EntryUser set, ModifyUser left 0. */
    public Map<String, Object> saveTehsil(Map<String, Object> b) {
        UserAccount u = user(SCREEN_TEHSIL);
        int id = asInt(b.get("id"));
        String name = raw(b.get("name"));
        int districtId = asInt(b.get("districtId"));
        if (districtId <= 0 || !owns(repo.districts(u), districtId)) throw new IllegalArgumentException("District Field is Required");
        if (name.trim().isEmpty()) throw new IllegalArgumentException("Tehsil Name Field is Required");
        if (id > 0 && !owns(repo.tehsils(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Code", name);
        m.put("@Description", name);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", 0);
        m.put("@PostDate", ts(now));
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@DistrictId", districtId);
        return result(repo.saveTehsil(m));
    }

    // ================================================================= 750 City (DefineCity.cs)

    /** DefineCity_Load - TehsilBind (Tehsil.GetAll) and CityDefineGridFill (City.GetAll). */
    public Map<String, Object> citySetup() {
        UserAccount u = user(SCREEN_CITY);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("tehsils", repo.tehsils(u)); } catch (Exception e) { out.put("tehsilsError", msg(e)); }
        try { out.put("cities", repo.cities(u)); } catch (Exception e) { out.put("citiesError", msg(e)); }
        return out;
    }

    /** btnRefresh_Click - TehsilBind. */
    public List<Map<String, Object>> tehsilsForCity() { return repo.tehsils(user(SCREEN_CITY)); }

    /**
     * Insert() :100 - the 15 City properties: Code = Description = text, CityNameOtherLingo null,
     * CountryId 0, PostDate null (DateTime?), PostUser 0, PostState false, TehsilId = combo value
     * (0 when "...Select Any Value..." - the form does not validate the Tehsil).
     */
    public Map<String, Object> saveCity(Map<String, Object> b) {
        UserAccount u = user(SCREEN_CITY);
        int id = asInt(b.get("id"));
        String name = raw(b.get("name"));
        int tehsilId = asInt(b.get("tehsilId"));
        if (name.trim().isEmpty()) throw new IllegalArgumentException("CityName Field is Required");
        if (tehsilId != 0 && !owns(repo.tehsils(u), tehsilId)) throw new IllegalArgumentException("Tehsil not found.");
        if (id > 0 && !owns(repo.cities(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Code", name);
        m.put("@Description", name);
        m.put("@CityNameOtherLingo", null);
        m.put("@CountryId", 0);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", u.getId());
        m.put("@PostDate", null);
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@TehsilId", tehsilId);
        return result(repo.saveCity(m));
    }

    // ================================================================= 751 Currency (DefineMultiCurrency.cs)

    public List<Map<String, Object>> currencies() { return repo.currencies(user(SCREEN_CURRENCY)); }

    /** grdcurrency_DoubleClick - MultiCurrency.GetById(RecId). */
    public Map<String, Object> currency(int id) {
        UserAccount u = user(SCREEN_CURRENCY);
        if (!owns(repo.currencies(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.currency(id));
    }

    /** btnsave_Click :39 / btnupdate_Click :70 - the 16 properties; CurrencyRate = Conversion.ToSingle (0 when not numeric); CurrencyStatus true. */
    public Map<String, Object> saveCurrency(Map<String, Object> b) {
        UserAccount u = user(SCREEN_CURRENCY);
        int id = asInt(b.get("id"));
        String code = raw(b.get("code")), name = raw(b.get("name")), rate = raw(b.get("rate")),
               symbol = raw(b.get("symbol")), url = raw(b.get("url"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("Currency Code Field Required");
        if (name.trim().isEmpty()) throw new IllegalArgumentException("Currency Name Field Required");
        if (rate.trim().isEmpty()) throw new IllegalArgumentException("Currency Rate Field Required");
        if (id > 0 && !owns(repo.currencies(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@CurrencyCode", code);
        m.put("@CurrencyName", name);
        m.put("@CurrencyRate", toSingle(rate));
        m.put("@CurrencySymbol", symbol);
        m.put("@Url", url);
        m.put("@CurrencyStatus", true);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", u.getId());
        m.put("@PostDate", ts(now));
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        return result(repo.saveCurrency(m));
    }

    // ================================================================= 752 Sea Ports (SeaPortsDefine.cs)

    /** DateLock_Load :54 - the rights of frmExImSeaPorts (Save -> btnSave/btnAdd, Update -> btnupdate), TypeFill, BindGrid. */
    public Map<String, Object> seaPortSetup() {
        UserAccount u = user(SCREEN_SEA_PORTS);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", allowed(u, SCREEN_SEA_PORTS, "Save"));
        out.put("canUpdate", allowed(u, SCREEN_SEA_PORTS, "Update"));
        try { out.put("ports", repo.seaPorts(u)); } catch (Exception e) { out.put("portsError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> seaPorts() { return repo.seaPorts(user(SCREEN_SEA_PORTS)); }

    /** ReadById :137 - SeaPorts.GetByID. */
    public Map<String, Object> seaPort(int id) {
        UserAccount u = user(SCREEN_SEA_PORTS);
        if (!owns(repo.seaPorts(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.seaPort(id));
    }

    /**
     * Insert() :87 - PortName = text.Trim(), PortTypeId 1 Destination / 2 Loading, CountryId 0; the BLL
     * sets EntryDate, ModifyDate, PostDate = now and PostState false; EntryUser = ModifyUser = user.
     * The desktop enables the buttons from the Save / Update rights; the same rights are required here.
     */
    public Map<String, Object> saveSeaPort(Map<String, Object> b) {
        UserAccount u = user(SCREEN_SEA_PORTS);
        int id = asInt(b.get("id"));
        String description = raw(b.get("description"));
        int typeId = asInt(b.get("portTypeId"));
        if (description.isEmpty()) throw new IllegalArgumentException("Description Field is Required");
        if (typeId == 0) throw new IllegalArgumentException("Type Field is Required");
        if (typeId != 1 && typeId != 2) throw new IllegalArgumentException("Type Field is Required");
        rights.require(u, SCREEN_SEA_PORTS, id == 0 ? "Save" : "Update");
        if (id > 0 && !owns(repo.seaPorts(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@PortName", description.trim());
        m.put("@CountryId", 0);
        m.put("@EntryDate", ts(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@ModifyUser", u.getId());
        m.put("@PostDate", ts(now));
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@PortTypeId", typeId);
        return result(repo.saveSeaPort(m));
    }

    // ================================================================= 754 Date Lock (DateLock.cs)

    /** DateLock_Load - BindGrid, ProjectComboFill, BranchComboFill (each first row activated on the page). */
    public Map<String, Object> dateLockSetup() {
        UserAccount u = user(SCREEN_DATE_LOCK);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("locks", repo.dateLocks(u)); } catch (Exception e) { out.put("locksError", msg(e)); }
        try { out.put("projects", repo.projects(u)); } catch (Exception e) { out.put("projectsError", msg(e)); }
        try { out.put("branches", repo.branches(u)); } catch (Exception e) { out.put("branchesError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> dateLocks() { return repo.dateLocks(user(SCREEN_DATE_LOCK)); }

    /** RetrivedData :150 - DateLock.GetbyId(RecId). */
    public Map<String, Object> dateLock(int id) {
        UserAccount u = user(SCREEN_DATE_LOCK);
        if (!owns(repo.dateLocks(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.dateLock(id));
    }

    /**
     * btnSave_Click :70 - no validation on the desktop. Date = picker, Status = (text == "Lock"),
     * Branches / Projects = combos, IsApproved false, ApprovedUserId 0, ApprovedDate now.
     */
    public Map<String, Object> saveDateLock(Map<String, Object> b) {
        UserAccount u = user(SCREEN_DATE_LOCK);
        int id = asInt(b.get("id"));
        int branchId = asInt(b.get("branchesId")), projectId = asInt(b.get("projectsId"));
        String status = raw(b.get("status"));
        LocalDate date = date(b.get("date"));
        if (branchId != 0 && !owns(repo.branches(u), branchId)) throw new IllegalArgumentException("Branch not found.");
        if (projectId != 0 && !owns(repo.projects(u), projectId)) throw new IllegalArgumentException("Project not found.");
        if (id > 0 && !owns(repo.dateLocks(u), id)) throw new IllegalArgumentException("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Date", ts(date.atTime(now.toLocalTime())));
        m.put("@Status", "Lock".equals(status));
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@BranchesId", branchId);
        m.put("@ProjectsId", projectId);
        m.put("@EntryUserId", u.getId());
        m.put("@EntryDate", ts(now));
        m.put("@ModifyUserId", u.getId());
        m.put("@ModifyDate", ts(now));
        m.put("@IsApproved", false);
        m.put("@ApprovedUserId", 0);
        m.put("@ApprovedDate", ts(now));
        return result(repo.saveDateLock(m));
    }

    // ================================================================= 742 Other Items (InvOtherItems.cs)

    /** InvOtherItems_Load - PurchaseGLAcFill (three combos from one list), BindGrid, BranchComboFill, ProjectComboFill. */
    public Map<String, Object> otherItemSetup() {
        UserAccount u = user(SCREEN_OTHER_ITEMS);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("accounts", repo.coaForCombo(u)); } catch (Exception e) { out.put("accountsError", msg(e)); }
        try { out.put("items", repo.otherItems(u)); } catch (Exception e) { out.put("itemsError", msg(e)); }
        try { out.put("branches", repo.branches(u)); } catch (Exception e) { out.put("branchesError", msg(e)); }
        try { out.put("projects", repo.projects(u)); } catch (Exception e) { out.put("projectsError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> otherItems() { return repo.otherItems(user(SCREEN_OTHER_ITEMS)); }

    /** RetrivedData :171 - InventoryItemsOther.GetById(RecId). */
    public Map<String, Object> otherItem(int id) {
        UserAccount u = user(SCREEN_OTHER_ITEMS);
        if (!owns(repo.otherItems(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.otherItem(id));
    }

    /** btnsave_Click :96 / btnupdate_Click :139 - the 10 properties. The form tests text == "" or "0". */
    public Map<String, Object> saveOtherItem(Map<String, Object> b) {
        UserAccount u = user(SCREEN_OTHER_ITEMS);
        int id = asInt(b.get("id"));
        String code = raw(b.get("code")), name = raw(b.get("name"));
        int stockGl = asInt(b.get("stockGLAcId")), saleGl = asInt(b.get("saleGLAcId")), cogsGl = asInt(b.get("cogsGLAcId"));
        int branchId = asInt(b.get("branchesId")), projectId = asInt(b.get("projectsId"));
        if (code.isEmpty() || "0".equals(code)) throw new IllegalArgumentException("Code field required");
        if (name.isEmpty() || "0".equals(name)) throw new IllegalArgumentException("Item Name field required");
        List<Map<String, Object>> accounts = repo.coaForCombo(u);
        if (stockGl == 0 || !owns(accounts, stockGl)) throw new IllegalArgumentException("Purchase GL field required");
        if (saleGl == 0 || !owns(accounts, saleGl)) throw new IllegalArgumentException("Sale GL field required");
        if (cogsGl == 0 || !owns(accounts, cogsGl)) throw new IllegalArgumentException("CGS GL field required");
        if (branchId != 0 && !owns(repo.branches(u), branchId)) throw new IllegalArgumentException("Branch not found.");
        if (projectId != 0 && !owns(repo.projects(u), projectId)) throw new IllegalArgumentException("Project not found.");
        if (id > 0 && !owns(repo.otherItems(u), id)) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@OtherItemName", name);
        m.put("@OtherItemType", code);
        m.put("@StockGLAcId", stockGl);
        m.put("@SaleGLAcId", saleGl);
        m.put("@CogsGLAcId", cogsGl);
        m.put("@CompanyId", u.getCompanyId());
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@BranchesId", branchId);
        m.put("@ProjectsId", projectId);
        return result(repo.saveOtherItem(m));
    }

    // ================================================================= helpers

    private static Map<String, Object> result(int n) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", n);
        return out;
    }

    private static Map<String, Object> one(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Record not found.");   // [0] on an empty list
        return rows.get(0);
    }

    private static boolean owns(List<Map<String, Object>> rows, int id) {
        if (id <= 0) return false;
        for (Map<String, Object> r : rows) if (asInt(ci(r, "Id")) == id) return true;
        return false;
    }

    private static Timestamp ts(LocalDateTime t) { return Timestamp.valueOf(t); }

    private static LocalDate date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return LocalDate.now();
        try { return LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid date."); }
    }

    /** Conversion.ToSingle - a float, 0 when the text is not a number. */
    private static double toSingle(String s) {
        try { return (double) Float.parseFloat(s.trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
