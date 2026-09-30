package com.mst.services;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 11 "Define Supplier" - desktop form Architecture.WinApp.Supplier_Purchase.supfrmDefineSupplier
 * (ScreenName = the form's Tag, "supfrmDefineSupplier"), BLL/DAL Inventory.SupplierCustomer.
 *
 * <ul>
 * <li>Party Type: SupplierCustomer.PartyTypeGetAll -> USP_GetAllSupplierCustomerType @OrganizationId, @CompanyId.</li>
 * <li>cmbPartyType_Leave: USP_GetAccountsForSupplierCustomerByPartyType (@GlRecId only when != 0) for the
 *     GL and Advance account combos, and USP_GeneratePartyCode for the party code.</li>
 * <li>Party Group: CustomerGroup.GetAll -> Sp_CustomerGroup_GetAllMethod ReadAll.</li>
 * <li>Discount policy: DiscountPolicy.GetDiscountPolicyForCustomers (enabled only for group 3).</li>
 * <li>Country: SP_Country_ReadMethod GetAll; City: the login city list USP_City_GetAllWithCountryAndTehsil;
 *     Parent party: USP_GetVendorsAndCustomersWithCityName; Business type: USP_BusinessType_GetAllMethod ComboBind.</li>
 * <li>Defaults: configuration "City Area" / "DefaultPartyGroup".</li>
 * <li>Save / Update: Sp_SupplierCustomer_Insert / _Update with every non-virtual model property, then
 *     USP_SupplierCustomerTaxSchedule_SyncFromMapping, one transaction.</li>
 * <li>Register tab: USP_GetDataForDropDownFrom_SupplierCustomer for its combos and Sp_SupplierCustomerHistory_rpt.</li>
 * </ul>
 * There is no delete on the desktop form.
 */
@Service
public class SupplierDesktopService {

    public static final String SCREEN_NAME = "supfrmDefineSupplier";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public SupplierDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
    }

    private int org() { return ctx.currentOrganizationId(); }
    private int company() { return ctx.currentCompanyId(); }

    public Map<String, Object> lookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("subsidiaryAccounts", erpFeature(4));
        out.put("attributesForItem", erpFeature(13));
        out.put("partyTypes", DesktopProc.rows(jdbc, "[dbo].[USP_GetAllSupplierCustomerType]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company())));
        out.put("partyGroups", DesktopProc.rows(jdbc, "Sp_CustomerGroup_GetAllMethod", DesktopProc.params(
                "Activity", "ReadAll", "OrganizationId", org(), "CompanyId", company())));
        out.put("discountPolicies", discountPolicies());
        out.put("countries", DesktopProc.rows(jdbc, "SP_Country_ReadMethod", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "MethodType", "GetAll")));
        out.put("cities", DesktopProc.rows(jdbc, "[dbo].[USP_City_GetAllWithCountryAndTehsil]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company())));
        out.put("businessTypes", DesktopProc.rows(jdbc, "USP_BusinessType_GetAllMethod", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "Activity", "ComboBind")));
        out.put("parties", DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company())));
        out.put("defaultCityId", configValue("City Area"));
        out.put("defaultPartyGroupId", configValue("DefaultPartyGroup"));
        out.put("registerCombos", DesktopProc.rows(jdbc, "USP_GetDataForDropDownFrom_SupplierCustomer", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company())));
        return out;
    }

    public List<Map<String, Object>> discountPolicies() {
        return DesktopProc.rows(jdbc, "Sp_DiscountPolicy_GetAllMethod", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "Activity", "GetDiscountPolicyForCustomers"));
    }

    /** cmbPartyType_Leave */
    public Map<String, Object> byPartyType(int partyTypeId, int glRecId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("accounts", partyTypeId > 0
                ? DesktopProc.rows(jdbc, "[dbo].[USP_GetAccountsForSupplierCustomerByPartyType]", DesktopProc.params(
                        "OrganizationId", org(), "CompanyId", company(), "PartyTypeId", partyTypeId,
                        "GlRecId", glRecId != 0 ? glRecId : null))
                : List.of());
        String code = "";
        if (partyTypeId > 0) {
            List<Map<String, Object>> r = DesktopProc.rows(jdbc, "USP_GeneratePartyCode", DesktopProc.params(
                    "OrganizationId", org(), "CompanyId", company(), "PartyTypeId", partyTypeId));
            if (!r.isEmpty()) {
                Object v = r.get(0).values().iterator().next();
                code = v == null ? "" : String.valueOf(v);
            }
        }
        out.put("partyCode", code);
        return out;
    }

    /** ReadById: SupplierCustomer.GetByID -> Sp_SupplierCustomer_GetAllMethod @Id, @Activity='ReadById'. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", DesktopProc.params(
                "Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) return null;
        Map<String, Object> row = r.get(0);
        Object c = row.get("CompanyId");
        if (c instanceof Number && ((Number) c).intValue() != company()) return null;
        return row;
    }

    /**
     * Insert(): recId 0 = btnsave (Save right), otherwise btnupdate (Update right). The field
     * validation, the Active Status check and the confirmation run on the page first; the required
     * fields are re-checked here with the desktop's messages.
     */
    @Transactional
    public String save(Map<String, Object> f) {
        int recId = toInt(f.get("recId"));
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (recId == 0 && !Boolean.TRUE.equals(r.get("save"))) throw new IllegalArgumentException("You do not have the Save right for this screen.");
        if (recId != 0 && !Boolean.TRUE.equals(r.get("update"))) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        requireInt(f, "partyTypeId", "Party Type");
        requireText(f, "partyCode", "Party Code");
        requireText(f, "companyName", "Company / Business Name");
        requireInt(f, "glAccountId", "GL Account");
        if (toBool(f.get("isSubSupCust"))) requireInt(f, "parentsSupCustId", "Parent Account");
        requireInt(f, "customerGroupId", "Party Group");
        requireInt(f, "businessTypeId", "Company / Individual");
        requireText(f, "address1", "Address");
        requireText(f, "mobilePersonal", "Phone Number");
        requireText(f, "whatsAppNo", "WhatsApp Number");
        requireInt(f, "countryId", "Country");
        requireInt(f, "cityId", "City");
        if (toInt(f.get("businessTypeId")) == 1) requireText(f, "ntnNo", "NTN #");
        /* CNIC is a MaskedTextBox on the desktop, which FormHelper.ValidateControls skips. */
        if (recId == 0 && !toBool(f.get("status"))) throw new IllegalArgumentException("Please check the Active Status!!!");

        LocalDateTime now = LocalDateTime.now();
        LocalDateTime cnicExpiry = now;
        String cnicExp = str(f.get("cnicExpiryDate"));
        if (!cnicExp.isEmpty()) {
            try { cnicExpiry = LocalDate.parse(cnicExp).atTime(now.toLocalTime()); } catch (Exception ignored) { cnicExpiry = now; }
        }
        String manual = str(f.get("manualPartyCode"));
        int user = ctx.currentUserId();
        Map<String, Object> p = DesktopProc.params(
                "IsDebitCredit", false,
                "IsSubSupCust", toBool(f.get("isSubSupCust")),
                "PostState", false,
                "Status", toBool(f.get("status")),
                "CNIC_EXPIRY_DATE", cnicExpiry,
                "EntryDate", now, "ModifyDate", now, "PostDate", now,
                "CreditLimit", 0.0, "DebitCreditAmount", 0.0,
                "ActionId", 0,
                "AdvanceGlAcId", toInt(f.get("advanceGlAcId")),
                "BranchId", 0,
                "CityId", toInt(f.get("cityId")),
                "CompanyId", company(),
                "CountryId", toInt(f.get("countryId")),
                "CustomerGroupId", toInt(f.get("customerGroupId")),
                "EntryUser", user,
                "GlAccountId", toInt(f.get("glAccountId")),
                "Id", recId,
                "ModifyUser", user,
                "OrganizationId", org(),
                "ParentsSupCustId", toInt(f.get("parentsSupCustId")),
                "DiscountPolicyId", toInt(f.get("discountPolicyId")),
                "PostUser", 0, "ProfileGroupId", 0, "ProjectId", 0,
                "StateProvinceId", 0,
                "CustomerTypeId", 0,
                "PartyTypeId", toInt(f.get("partyTypeId")),
                "BusinessTypeId", toInt(f.get("businessTypeId")),
                "PartyTypePrefix", recId > 0 ? str(f.get("partyTypePrefix")) : str(f.get("partyCode")),
                "Address1", str(f.get("address1")).trim(),
                "Address2", str(f.get("address2")).trim(),
                "CNIC", str(f.get("cnic")).trim(),
                "CompanyName", str(f.get("companyName")).trim(),
                "Email", str(f.get("email")).trim(),
                "FirstName", str(f.get("firstName")).trim(),
                "FTN_No", "0",
                "LastName", str(f.get("lastName")).trim(),
                "MobileOffice", str(f.get("mobileOffice")).trim(),
                "MobilePersonal", str(f.get("mobilePersonal")).trim(),
                "NTN_No", str(f.get("ntnNo")).trim(),
                "Phone", str(f.get("phone")).trim(),
                "PictureURL", recId > 0 ? str(f.get("pictureUrl")) : "",
                "NickName", str(f.get("nickName")).trim(),
                "ReportingTitle", str(f.get("companyName")).trim(),
                "STRN_No", str(f.get("strnNo")).trim(),
                "SupCustCode", manual.toLowerCase().trim(),
                "Title", str(f.get("title")).trim(),
                "Town", str(f.get("town")).trim(),
                "WebPage", str(f.get("webPage")).trim(),
                "ZipCode", str(f.get("zipCode")).trim(),
                "ManualPartyCode", manual,
                "WhatsAppNo", str(f.get("whatsAppNo")).trim(),
                "CompanyIndividuals", str(f.get("businessTypeText")).trim(),
                "IsTaxable", false);
        if (recId > 0 && readById(recId) == null) throw new IllegalArgumentException("Rec Id not found...");
        DesktopProc.setProc(jdbc, recId == 0 ? "Sp_SupplierCustomer_Insert" : "Sp_SupplierCustomer_Update", p);
        DesktopProc.rows(jdbc, "[dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company()));
        return recId > 0 ? "Update Successfully" : "Data Save Successfully";
    }

    /** RegisterGridBind: GeneralReprots.SupplierCustomerRegister (@IsTaxable = false is always sent). */
    public List<Map<String, Object>> register(int partyTypeId, int glAccountId, int customerGroupId, int cityId) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomerHistory_rpt", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(),
                "CityId", cityId != 0 ? cityId : null,
                "GlAccountId", glAccountId != 0 ? glAccountId : null,
                "PartyTypeId", partyTypeId != 0 ? partyTypeId : null,
                "CustomerGroupId", customerGroupId != 0 ? customerGroupId : null,
                "IsTaxable", false));
    }

    public Map<String, Boolean> rights() {
        return rights.of(SCREEN_NAME);
    }

    private boolean erpFeature(int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                DesktopProc.params("OrganizationId", org(), "CompanyId", company()))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    private String configValue(String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", org(), "CompanyId", company(), "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    private static void requireInt(Map<String, Object> f, String key, String label) {
        if (toInt(f.get(key)) == 0) throw new IllegalArgumentException(label + " field is required");
    }

    private static void requireText(Map<String, Object> f, String key, String label) {
        if (str(f.get(key)).trim().isEmpty()) throw new IllegalArgumentException(label + " field is required");
    }

    static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = o == null ? "" : String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
