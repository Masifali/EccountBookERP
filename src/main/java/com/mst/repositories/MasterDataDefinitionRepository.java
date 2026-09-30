package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * DAL of the nine screens of dbo.App 19 "Master Data Definition", AppModules 2039 "System_Level"
 * (all in Architecture.WinApp):
 *
 *   749 Country     DefineCountry.cs        BLL 0064 country          DAL 0058   SP_Country_ReadMethod / Sp_Country_Insert|Update (12 params)
 *   753 Province    DefineProvince.cs       BLL 0083 StateProvince    DAL 0075   Proc_StateProvince_GetAllMethod / Sp_StateProvince_Insert|Update (13)
 *   756 District    DefineDistrict.cs       BLL 0051 District         DAL 0052   Sp_District_GetAllMethod / Sp_District_Insert|Update (13)
 *   755 Tehsil      DefineTehsil.cs         BLL 0050 Tehsil           DAL 0051   Sp_Tehsil_GetAllMethod / Sp_Tehsil_Insert|Update (13)
 *   750 City        DefineCity.cs           BLL 0060 City             DAL 0053   SP_City_GetAllMethod / Sp_City_Insert|Update (15)
 *   751 Currency    DefineMultiCurrency.cs  BLL 0076 MultiCurrency    DAL 0069   Sp_MultiCurrency_GetAllMethod / Sp_MultiCurrency_Insert|Update (16)
 *   752 Sea Ports   SeaPortsDefine.cs       BLL 0597 SeaPorts         DAL 0450   Sp_SeaPorts_GetAllMethod / Sp_SeaPorts_Insert|Update (13)
 *   754 Date Lock   DateLock.cs             BLL 0066 DateLock         DAL 0060   Sp_DateLock_GetAllMethod / Sp_DateLock_Insert|Update (14)
 *   742 Other Items InvOtherItems.cs        BLL 0573 InventoryItemsOther DAL 0427 Sp_InventoryItemsOther_GetAllMethod / _Insert|_Update (10)
 *
 * Every parameter list below is the desktop model, every property sent (GenericProvider.SetProc), and
 * was checked against procdure.utf8.sql / procdure_index.csv on 2026-09-30. Every Insert procedure
 * ends with SELECT SCOPE_IDENTITY(); every Update returns no result set, so Convert.ToInt32 gives 0 -
 * and the DALs of Country, Province and Other Items copy that back into obj.Id, the others do not.
 * Combos: Sp_Branches_GetAllMethod 'GetAll' (BrancheServiceBind), Sp_Projects_GetAllMethod 'GetAll'
 * (ProjectServiceBind), Sp_COAAllocation_GetAllMethod 'COAForCombobindig' + @UserId
 * (CoaAllocationGetForComboServiceBind). No table, column or procedure is created or changed.
 */
@Repository
public class MasterDataDefinitionRepository {

    private final JdbcTemplate jdbc;

    public MasterDataDefinitionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------------ Country (749)

    /** country.GetAll(new Country()) - the form passes a NEW model, so Organization/Company are 0; the GetAll branch ignores them. */
    public List<Map<String, Object>> countries() {
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod", params("OrganizationId", 0, "CompanyId", 0, "MethodType", "GetAll"));
    }

    /** country.GetById: @MethodType 'GetById', @Id. */
    public List<Map<String, Object>> country(int id) {
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod", params("MethodType", "GetById", "Id", id));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveCountry(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_Country_Insert" : "Sp_Country_Update", bare(model));
    }

    // ------------------------------------------------------------------------ Province (753)

    /** StateProvince.GetAll: @Activity 'ReadAll' first, then the session's Organization/Company. */
    public List<Map<String, Object>> provinces(UserAccount u) {
        return DesktopProc.rows(jdbc, "Proc_StateProvince_GetAllMethod",
                params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** StateProvince.GetByID: @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> province(int id) {
        return DesktopProc.rows(jdbc, "Proc_StateProvince_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveProvince(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_StateProvince_Insert" : "Sp_StateProvince_Update", bare(model));
    }

    // ------------------------------------------------------------------------ District (756)

    /** District.GetAll: @OrganizationId, @CompanyId, @Activity 'ReadAll' (columns Id, District, StateProvinceId, StateProvince, EntryDate, EntryUser, ModifyDate, ModifyUser). */
    public List<Map<String, Object>> districts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_District_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveDistrict(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_District_Insert" : "Sp_District_Update", bare(model));
    }

    // ------------------------------------------------------------------------ Tehsil (755)

    /** Tehsil.GetAll: @OrganizationId, @CompanyId, @Activity 'ReadAll' (Id, Tehsil, DistrictId, District, EntryDate, EntryUser, ModifyDate, ModifyUser). */
    public List<Map<String, Object>> tehsils(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Tehsil_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveTehsil(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_Tehsil_Insert" : "Sp_Tehsil_Update", bare(model));
    }

    // ------------------------------------------------------------------------ City (750)

    /** City.GetAll: @OrganizationId, @CompanyId, @MethodType 'GetAll' (the procedure filters on Organization only). */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_City_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveCity(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_City_Insert" : "Sp_City_Update", bare(model));
    }

    // ------------------------------------------------------------------------ Currency (751)

    /** MultiCurrency.GetAll: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** MultiCurrency.GetById: @Activity 'ReadById', @Id. */
    public List<Map<String, Object>> currency(int id) {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", params("Activity", "ReadById", "Id", id));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveCurrency(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_MultiCurrency_Insert" : "Sp_MultiCurrency_Update", bare(model));
    }

    // ------------------------------------------------------------------------ Sea Ports (752)

    /** SeaPorts.Getall: @OrganizationId, @CompanyId, @Activity 'ReadByCompanyNOrganizationId' (Id, PortName, PortType, ...). */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    /** SeaPorts.GetByID: @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> seaPort(int id) {
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveSeaPort(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_SeaPorts_Insert" : "Sp_SeaPorts_Update", bare(model));
    }

    // ------------------------------------------------------------------------ Date Lock (754)

    /** DateLock.GetAll: @OrganizationId, @CompanyId, @Activity 'ReadAll' (Status already 'Lock' / 'Unlock'). */
    public List<Map<String, Object>> dateLocks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_DateLock_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** DateLock.GetbyId: @Id, @Activity 'ReadById' (Status as the bit). */
    public List<Map<String, Object>> dateLock(int id) {
        return DesktopProc.rows(jdbc, "Sp_DateLock_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveDateLock(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_DateLock_Insert" : "Sp_DateLock_Update", bare(model));
    }

    // ------------------------------------------------------------------------ Other Items (742)

    /** InventoryItemsOther.GetAll: @Activity 'ReadAll', @organizationId, @CompanyId. */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod",
                params("Activity", "ReadAll", "organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** InventoryItemsOther.GetById: @Activity 'ReadById', @Id. */
    public List<Map<String, Object>> otherItem(int id) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params("Activity", "ReadById", "Id", id));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveOtherItem(Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("Id")) == 0 ? "Sp_InventoryItemsOther_Insert" : "Sp_InventoryItemsOther_Update", bare(model));
    }

    // ------------------------------------------------------------------------ shared combos

    /** CommonServices.BrancheServiceBind -> Sp_Branches_GetAllMethod 'GetAll' (Id, BranchName). */
    public List<Map<String, Object>> branches(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** CommonServices.ProjectServiceBind -> Projects.GetAlldt: Sp_Projects_GetAllMethod 'GetAll' (Id, ProjectName). */
    public List<Map<String, Object>> projects(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** CommonServices.CoaAllocationGetForComboServiceBind -> COAAllocation.GetForComboBind: 'COAForCombobindig', @UserId when != 0 (Id, AccountTitle). */
    public List<Map<String, Object>> coaForCombo(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "COAForCombobindig");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /**
     * DesktopProc prefixes every key with '@' itself, so parameter names are passed bare. The service
     * names the model properties as the desktop does ("@Id", ...); this strips that prefix.
     */
    private static Map<String, Object> bare(Map<String, Object> model) {
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            String k = e.getKey();
            out.put(k.startsWith("@") ? k.substring(1) : k, e.getValue());
        }
        return out;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
