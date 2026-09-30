package com.mst.services;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 9 "Define Accounts" - desktop form Architecture.WinApp.AcfrmDefCoa, BLL
 * Architecture.BLL.Accounts.ChartofAccount, DAL Architecture.DAL.Accounts.ChartofAccount.
 * Every list and every write below is the desktop's own procedure call with its own parameter set;
 * the earlier JPA port (client-side code generation, max(id)+1, JPA allocation/opening rows, a
 * delete-by-code the desktop does not have) is no longer used by the page.
 *
 * GenericProvider.SetProc sends EVERY non-virtual property of the model (a CLR null is left out,
 * so the procedure default applies); the insert maps below list exactly those properties.
 */
@Service
public class ChartOfAccountDesktopService {

    public static final String SCREEN_NAME = "AcfrmDefCoa";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public ChartOfAccountDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
    }

    // ------------------------------------------------------------------ load (AcfrmDefCoa_Load)

    public Map<String, Object> lookups() {
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        int user = ctx.currentUserId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        /* CommonServices.GetERPFeatureById(6) - MultiCurrencyFeature */
        out.put("multiCurrency", erpFeature(org, company, 6));
        /* Conversion.ToBool(GetConfigurationByOrgCompandConfigDescription("CustomGroupCompulsoryOnChartofAccount")) */
        out.put("customGroupCompulsory", toBool(configValue(org, company, "CustomGroupCompulsoryOnChartofAccount")));
        /* AccountTitleFill: CoaAllocationGetForComboServiceBind -> COAAllocation.GetForComboBind */
        out.put("accountTitles", DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "UserId", user != 0 ? user : null,
                "Activity", "COAForCombobindig")));
        /* CurrencyFill: MultiCurrency.GetAll */
        out.put("currencies", DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "Activity", "ReadAll")));
        /* CustomGroupNameFill: AcLookUps.GetByProfileTypeId(AcLookUpTypesId = 1) */
        out.put("customGroups", DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "Activity", "ReadByAcLookUpId", "AcLookUpTypesId", 1)));
        /* cmbparentacfill: ChartofAccount.ReadAllAccountgroup (no level filter) */
        out.put("parentAccounts", parentAccounts());
        /* cmbbsnotefill: AccountNotes.ReadByBSNote(0) - ClassId 0 is not sent */
        out.put("bsNotes", DesktopProc.rows(jdbc, "SP_AccountNotes_ReadAllMethodBySPType", DesktopProc.params(
                "AccountNotesType", "ReadAllMethodByBSNote")));
        /* cmbplnotefill: AccountNotes.ReadByPLNote() */
        out.put("plNotes", DesktopProc.rows(jdbc, "SP_AccountNotes_ReadAllMethodBySPType", DesktopProc.params(
                "AccountNotesType", "ReadAllMethodByPLNote")));
        /* CmbAccountType: AccountTypes.GetAll(org, company) - binds cmbactype and CmbAccountTypeFilter */
        out.put("accountTypes", DesktopProc.rows(jdbc, "Proc_AccountTypes_ReadAll", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company)));
        /* cmbsupgroupsfill: CustomerGroup.GetAll (the 3 / 22 filter is applied on the client, as on the desktop) */
        out.put("customerGroups", DesktopProc.rows(jdbc, "Sp_CustomerGroup_GetAllMethod", DesktopProc.params(
                "Activity", "ReadAll", "OrganizationId", org, "CompanyId", company)));
        /* cmbCityFill: City.GetAll */
        out.put("cities", DesktopProc.rows(jdbc, "SP_City_GetAllMethod", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "MethodType", "GetAll")));
        /* BranchesBindInGrid: CommonServices.CompanyServiceBind -> Company.GetAlldt(OrgCompanyTypeId = org) */
        out.put("locations", DesktopProc.rows(jdbc, "Sp_Company_GetAllMethod", DesktopProc.params(
                "OrgCompanyTypeId", org, "Activity", "ReadByOrganizationId")));
        return out;
    }

    /** cmbparentacfill(): ReadAllAccountgroup with OrganizationId, CompanyId, FinancialYearId. */
    public List<Map<String, Object>> parentAccounts() {
        return DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "CoaType", "ReadAllAccountGroup"));
    }

    /** HistoryGridFill() / ReadAccountLevel: the level grids and the Child Of Selected Parent grid. */
    public List<Map<String, Object>> accountLevels() {
        return DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "LanguageId", 0,
                "CoaType", "ReadByFinancialYearIdNOrganizationIdNCompanyIdNParentAccountCode"));
    }

    /** cmbparentac_Leave: ReadNewCodebyParentCode (@CoaType='NewCodeByParentCode'). */
    public Map<String, Object> newCodeByParent(String parentCode) {
        List<Map<String, Object>> r = newCodeRows(parentCode);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return r.get(0);
    }

    private List<Map<String, Object>> newCodeRows(String parentCode) {
        return DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "CoaType", "NewCodeByParentCode",
                "AccountCode", parentCode,
                "FinancialYearId", ctx.currentFinancialYearId()));
    }

    /** CmbAccountTitle_Leave: COAAllocation.GetParentCodeByChartofAccountId (FinancialYearId is 0 -> not sent). */
    public int parentCodeByAccountId(int chartOfAccountId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Id", chartOfAccountId,
                "Activity", "GetParentCodeByChartofAccountId"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("ParentAccountCode"));
    }

    /** CmbAccountTypeFilter_Leave: COAAllocation.Get3rdLevelGroupAccounts (@TypeId always sent). */
    public List<Map<String, Object>> thirdLevelByType(int accountTypeId) {
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "TypeId", accountTypeId,
                "Activity", "Get3rdLevelGroupAccounts"));
    }

    /** ChartofAccount.GetByID -> SP_ChartofAccount_ReadByID @Id. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "SP_ChartofAccount_ReadByID", DesktopProc.params("Id", id));
        return r.isEmpty() ? null : r.get(0);
    }

    // ------------------------------------------------------------------ save (btnsave_Click)

    /**
     * btnsave_Click -> BLL ChartofAccount.Save (Id == 0) -> DAL SetData("Proc_ChartofAccount_Insert").
     * Returns the desktop message: txtactitle.Text + " Save Successfully".
     */
    @Transactional
    public String save(Map<String, Object> f) {
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        int user = ctx.currentUserId();
        int branch = ctx.currentBranchId();
        int finYear = ctx.currentFinancialYearId();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("save"))) throw new IllegalArgumentException("You do not have the Save right for this screen.");

        boolean multiCurrency = erpFeature(org, company, 6);
        boolean customGroupCompulsory = toBool(configValue(org, company, "CustomGroupCompulsoryOnChartofAccount"));

        String titleText = str(f.get("accountTitle"));
        String parentText = str(f.get("parentAccountText"));
        String parentCode = str(f.get("parentAccountCode"));
        String levelText = str(f.get("accountLevel"));
        String classText = str(f.get("accountClass"));
        String groupText = str(f.get("accountGroup"));
        int accountTypeId = toInt(f.get("accountTypeId"));
        String accountTypeText = str(f.get("accountTypeText"));
        int plNoteId = toInt(f.get("plNoteId"));
        String plNoteText = str(f.get("plNoteText"));
        int bsNoteId = toInt(f.get("bsNoteId"));
        String bsNoteText = str(f.get("bsNoteText"));
        int customerGroupId = toInt(f.get("customerGroupId"));
        String customerGroupText = str(f.get("customerGroupText"));
        int customGroupId = toInt(f.get("customGroupId"));
        int cityId = toInt(f.get("cityId"));
        int currencyId = toInt(f.get("currencyId"));
        String phone = str(f.get("phoneNo"));
        String otherCode = str(f.get("otherCode"));
        String opening = str(f.get("openingBalance"));

        /* Control states are derived exactly as cmbparentac_Leave / cmbactype_Leave set them. */
        Map<String, Object> parentRow = newCodeRows(parentCode).stream().findFirst().orElse(null);
        int classCode = parentRow == null ? 0 : toInt(parentRow.get("AccountClass"));
        int level = toInt(levelText);
        boolean plEnabled = level == 3 && (classCode == 4 || classCode == 5);
        boolean bsEnabled = level == 3 && (classCode == 1 || classCode == 2 || classCode == 3);
        boolean customerGroupReadOnly = !(accountTypeId == 3 || accountTypeId == 22);

        /* formvalidation() */
        if (titleText.trim().isEmpty()) throw new IllegalArgumentException("Please Enter Account Title");
        if (parentText.trim().isEmpty()) throw new IllegalArgumentException("Please Enter Parent Account");
        if ("3".equals(levelText) && accountTypeText.trim().isEmpty()) throw new IllegalArgumentException("Please Enter Account Type");
        if (plEnabled && "3".equals(levelText) && plNoteText.trim().isEmpty()) throw new IllegalArgumentException("Please Enter PL Notes");
        if (bsEnabled && "3".equals(levelText) && bsNoteText.trim().isEmpty()) throw new IllegalArgumentException("Please Enter BS Notes");
        if (!customerGroupReadOnly && "4".equals(levelText) && customerGroupText.trim().isEmpty()) throw new IllegalArgumentException("Please Select Customer Group");
        if (accountTypeId != 0 && accountTypeId == 3 && customGroupCompulsory && customGroupId == 0) throw new IllegalArgumentException("Please Select Custom Group");
        if (multiCurrency && "Detail".equals(groupText) && currencyId == 0) throw new IllegalArgumentException("Please Select Currency");

        /* btnsave_Click: the model */
        int accountClass = 0;
        switch (classText) {
            case "Capital": accountClass = 1; break;
            case "Assets": accountClass = 2; break;
            case "Liabilities": accountClass = 3; break;
            case "Expenses": accountClass = 4; break;
            case "Revenue": accountClass = 5; break;
            default: break;
        }
        int parentCodeId = toInt(f.get("parentCodeId"));
        String accountCode = str(f.get("accountCode")).trim();
        double ob = toDouble(opening.trim());
        double yearObDebit = ob > 0.0 ? ob : 0.0;
        double yearObCredit = ob < 0.0 ? ob : 0.0;
        int modelCustomerGroupId = (accountTypeId == 3 || accountTypeId == 22) ? customerGroupId : 0;
        String contactNo = null;
        int modelCurrencyId = 0;
        List<Integer> allocationCompanies = new ArrayList<>();
        if ("Detail".equals(groupText)) {
            contactNo = phone.trim();
            modelCurrencyId = currencyId;
            List<Map<String, Object>> rows = listOfMaps(f.get("allocations"));
            int count = rows.size();
            if (count == 0) throw new IllegalArgumentException("Grid Record Not Found");
            int countFalse = 0;
            for (Map<String, Object> row : rows) {
                if (count == 1) allocationCompanies.add(toInt(row.get("id")));
                else if (toBool(row.get("value"))) allocationCompanies.add(toInt(row.get("id")));
                else countFalse++;
            }
            if (count > 1 && count == countFalse) throw new IllegalArgumentException("Please select Branch first");
        }

        /* BLL Save, Id == 0 */
        LocalDateTime now = LocalDateTime.now();
        String parentAccountCode = parentCode;
        if (!parentAccountCode.isEmpty()) accountCode = parentAccountCode;
        int accountLevel = level;
        String accountGroup = groupText;
        List<Map<String, Object>> code = DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "CoaType", "NewCodeByParentCode",
                "AccountCode", accountCode, "FinancialYearId", finYear));
        if (!code.isEmpty()) {
            accountCode = str(code.get(0).get("AccountCode"));
            accountLevel = toInt(code.get(0).get("Account_Level"));
            if (accountGroup.isEmpty()) accountGroup = "Group";
        }
        if (parentAccountCode.isEmpty()) parentAccountCode = "0";

        /* DAL SetData: features read first (their own connection on the desktop) */
        boolean feature4 = erpFeature(org, company, 4);
        boolean feature17 = erpFeature(org, company, 17);
        Map<String, Object> p = DesktopProc.params(
                "IsActive", true, "PostState", false, "EntryDate", now, "ModifyDate", now, "PostDate", now,
                "Account_Level", accountLevel, "AccountClass", accountClass, "AccountTypeId", accountTypeId,
                "BSNoteId", bsNoteId, "CompanyId", company, "EntryUser", user, "FinancialYearId", finYear,
                "Id", 0, "CurrencyId", modelCurrencyId, "ModifyUser", 0, "OrganizationId", org,
                "PLNoteId", plNoteId, "PostUser", 0, "ParentCodeId", parentCodeId, "BranchId", branch,
                "CustomGroupId", customGroupId, "AccountCode", accountCode, "AccountGroup", accountGroup,
                "AccountTitle", titleText.trim(), "AccountTitleOtherLingo", null, "OtherErpCode", otherCode.trim(),
                "ParentAccountCode", parentAccountCode, "ContactNo", contactNo, "QrCode", null,
                "CityId", cityId, "CustomerGroupId", modelCustomerGroupId);
        int num = DesktopProc.setProc(jdbc, "Proc_ChartofAccount_Insert", p);
        if (num > 0) {
            for (Integer allocCompany : allocationCompanies) {
                DesktopProc.setProc(jdbc, "Sp_COAAllocation_Insert", DesktopProc.params(
                        "IsActive", true, "ChartofAccountId", num, "CompanyId", allocCompany, "Id", 0,
                        "BranchId", branch, "GLPageNo", null));
                if (!feature4 && (accountTypeId == 3 || (accountTypeId == 22 && "Detail".equals(accountGroup)))) {
                    if (modelCustomerGroupId == 0) throw new IllegalArgumentException("Supplier/Customer group field required");
                    DesktopProc.setProc(jdbc, "Sp_SupplierCustomer_Insert", supplierCustomerFromAccount(
                            org, allocCompany, user, now, num, phone.trim(), titleText.trim(), modelCustomerGroupId, cityId));
                }
                if ("Detail".equals(accountGroup)) {
                    DesktopProc.setProc(jdbc, "Sp_AccountsOpeningBalances_Insert", DesktopProc.params(
                            "PostState", false, "EntryDate", now, "ModifyDate", now, "PostDate", now,
                            "YearObCredit", feature17 ? 0.0 : yearObCredit, "YearObDebit", feature17 ? 0.0 : yearObDebit,
                            "ChartOfAccountId", num, "CompanyId", allocCompany, "BranchesId", 0, "EntryUser", user,
                            "FinancialYearId", finYear, "Id", 0, "ModifyUser", 0, "OrganizationId", org, "PostUser", 0,
                            "ChartOfAccountTitle", titleText.trim()));
                }
            }
        }
        return titleText + " Save Successfully";
    }

    /** DAL ChartofAccount.SetData's SupplierCustomer: every non-virtual property of the model. */
    private static Map<String, Object> supplierCustomerFromAccount(int org, int company, int user, LocalDateTime now,
            int glAccountId, String phone, String title, int customerGroupId, int cityId) {
        return DesktopProc.params(
                "IsDebitCredit", false, "IsSubSupCust", false, "PostState", false, "Status", true,
                "CNIC_EXPIRY_DATE", now, "EntryDate", now, "ModifyDate", now, "PostDate", now,
                "CreditLimit", 0.0, "DebitCreditAmount", 0.0, "ActionId", 0, "AdvanceGlAcId", glAccountId,
                "BranchId", 0, "CityId", cityId, "CompanyId", company, "CountryId", 0, "CustomerGroupId", customerGroupId,
                "EntryUser", user, "GlAccountId", glAccountId, "Id", 0, "ModifyUser", 0, "OrganizationId", org,
                "ParentsSupCustId", 0, "DiscountPolicyId", 0, "PostUser", 0, "ProfileGroupId", 0, "ProjectId", 0,
                "StateProvinceId", 0, "CustomerTypeId", 0, "PartyTypeId", 0, "BusinessTypeId", 0,
                "CompanyName", title, "MobilePersonal", phone, "ReportingTitle", title, "IsTaxable", false);
    }

    // ------------------------------------------------------------------ update (btnUpdate_Click)

    /** btnUpdate_Click -> DELETEANDUPDATECHARTOFACCOUNTBYID (empty list -> the single-row branch). */
    public String update(Map<String, Object> f) {
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("update"))) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        String title = str(f.get("accountTitle"));
        if (title.isEmpty() || "0".equals(title)) throw new IllegalArgumentException("AccountTitle Field Required");
        int currencyId = toInt(f.get("currencyId"));
        if (erpFeature(org, company, 6) && "Detail".equals(str(f.get("accountGroup"))) && currencyId == 0)
            throw new IllegalArgumentException("Please Select Currency");
        deleteAndUpdate(org, company, toInt(f.get("id")), title, str(f.get("otherCode")).trim(), currencyId,
                str(f.get("phoneNo")), true);
        return "Recored Update Successfully...;";
    }

    /**
     * DAL DELETEANDUPDATEBYACCOUNTID: OrganizationId, CompanyId, ReqType, ChartOfAccountId, AccountTitle,
     * OtherErpCode, CurrencyId, IsActive, EntryUser always; ContactNo only when non-empty. Run through
     * DesktopProc.rows so the audit procedure's result sets are drained, not rejected by an update call.
     */
    private void deleteAndUpdate(int org, int company, int chartOfAccountId, String title, String otherCode,
                                 int currencyId, String contactNo, boolean isActive) {
        DesktopProc.rows(jdbc, "SP_ChartOfAccount_DeleteAndUpdatebyAccountId", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "ReqType", "Update", "ChartOfAccountId", chartOfAccountId,
                "AccountTitle", title, "OtherErpCode", otherCode, "CurrencyId", currencyId, "IsActive", isActive,
                "EntryUser", ctx.currentUserId(),
                "ContactNo", (contactNo != null && !contactNo.isEmpty()) ? contactNo : null));
    }

    // ------------------------------------------------------------------ history tab

    /**
     * FormhistoryBind: VoucherReports.ChartofAccount. CmbAccount and CmbCustomGroupHistory are never
     * bound on the desktop (always empty), and ApprovedFilter is "All" or null - both skip @IsActive -
     * so only @OrganizationId, @CompanyId, @AccountLevels reach Sp_ChartOfAccounts_Rpt.
     */
    public List<Map<String, Object>> history(String accountLevels) {
        if (accountLevels == null || accountLevels.isEmpty()) throw new IllegalArgumentException("Please Check the Ac Levels First");
        return DesktopProc.rows(jdbc, "Sp_ChartOfAccounts_Rpt", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AccountLevels", accountLevels));
    }

    /** grdFormHistory_ColumnButtonClick "Edit" and btnUpdateHistory_Click: one call per changed row. */
    public void historyUpdate(List<Map<String, Object>> rows, boolean multiCurrencyColumn) {
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("update"))) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        for (Map<String, Object> row : rows) {
            int currency = multiCurrencyColumn ? toInt(row.get("currencyId")) : 0;
            deleteAndUpdate(org, company, toInt(row.get("id")),
                    row.get("accountTitle") == null ? "" : String.valueOf(row.get("accountTitle")),
                    row.get("otherErpCode") == null ? "" : String.valueOf(row.get("otherErpCode")),
                    currency, null, toBool(row.get("isActive")));
        }
    }

    /** toolStripButton7_Click (114-Print): VoucherReports.ChartofAccount with only org/company reaching the proc. */
    public List<Map<String, Object>> printRows() {
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("print"))) throw new IllegalArgumentException("You do not have the Print right for this screen.");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ChartOfAccounts_Rpt", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()));
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");
        return rows;
    }

    // ------------------------------------------------------------------ helpers

    public boolean erpFeatureForPage(int org, int company, int featureId) {
        return erpFeature(org, company, featureId);
    }

    boolean erpFeature(int org, int company, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                DesktopProc.params("OrganizationId", org, "CompanyId", company))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    String configValue(int org, int company, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> listOfMaps(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object e : (List<Object>) o) if (e instanceof Map) out.add((Map<String, Object>) e);
        return out;
    }

    static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    static double toDouble(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0.0;
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0.0; }
    }

    /** Conversion.ToBool: "1" / "true" (any case) are true. */
    static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = o == null ? "" : String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
