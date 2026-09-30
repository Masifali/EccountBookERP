package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * The read side shared by three plain (non-tax) voucher screens of the desktop's
 * {@code Architecture.WinApp.Account_Definition} namespace:
 *
 * <pre>
 *   screen 19  VoucherEntry     "Jounal Voucher"   Account_Definition/VoucherEntry.cs   DocumentTypeId 5
 *   screen 22  ContraVoucher    "Contra Voucher"   Account_Definition/ContraVoucher.cs  DocumentTypeId 10
 *   screen 46  ExpenseVoucher   "Expense Voucher"  Account_Definition/ExpenseVoucher.cs DocumentTypeId 26
 * </pre>
 *
 * (ScreenDefinition.TargetUrl of 19/22/46 names exactly these three classes; the
 * VouchersWithTax twins are separate screens: 860 JournalVoucher, 855 ContraVoucherTax,
 * 862 ExpenseVoucherNew. CommonServices.OpenDocument routes DocumentTypeId 5/10/26 with
 * BaseDocumentTypeId 0 to these plain forms.)
 *
 * Every lookup below is the desktop's own call: the procedure, and the parameter set exactly as
 * the BLL builds it (a parameter the BLL adds only when non-zero / non-empty is omitted here in
 * the same case - DesktopProc drops a null). Tenancy always comes from {@link CurrentUserContext}.
 */
@Service
public class DesktopVoucherScreenService {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopVoucherScreenService.class);

    /** The three screens, with the per-screen facts each form hard-codes. */
    public enum Screen {
        /** VoucherEntry.cs: base.Name "VoucherEntry" (:6041), GenerateVoucherCode(5) (:444),
         *  HistoryGridFill sets neither AppId nor AccountId (:1985-2053). */
        VOUCHER_ENTRY("voucher-entry", "VoucherEntry", 5, false, false),
        /** ContraVoucher.cs: base.Name "ContraVoucher" (:5436), GenerateVoucherCode(10) (:1093),
         *  HistoryFill sets AppId (:1653) and AccountId (:1706). */
        CONTRA("contra", "ContraVoucher", 10, true, true),
        /** ExpenseVoucher.cs: base.Name "ExpenseVoucher" (:6211), GenerateVoucherCode(26) (:630),
         *  HistoryFill sets AppId (:2697) and AccountId (:2751). */
        EXPENSE("expense", "ExpenseVoucher", 26, true, true),
        /** Screen 855 "Contra Voucher New" - VouchersWithTax/ContraVoucher.cs, ScreenName
         *  ContraVoucherTax (its rights are read by base.Tag, :1100), GenerateVoucherCode(10) (:1432),
         *  HistoryFill sets AppId (:2168) and AccountId (:2221). BaseDocumentTypeId 3. */
        CONTRA_TAX("contra-tax", "ContraVoucherTax", 10, true, true);

        public final String slug;
        public final String screenName;
        public final int documentTypeId;
        public final boolean historySendsAppId;
        public final boolean historyHasAccountFilter;

        Screen(String slug, String screenName, int documentTypeId, boolean historySendsAppId,
               boolean historyHasAccountFilter) {
            this.slug = slug;
            this.screenName = screenName;
            this.documentTypeId = documentTypeId;
            this.historySendsAppId = historySendsAppId;
            this.historyHasAccountFilter = historyHasAccountFilter;
        }

        public static Screen of(String slug) {
            for (Screen s : values()) if (s.slug.equalsIgnoreCase(slug)) return s;
            throw new IllegalArgumentException("Unknown voucher screen: " + slug);
        }
    }

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    // ================================================================== rights / features / config

    /**
     * CommonServices.SetRightsValueInRightsObject(base.Name) (:17565): tblUserRights.GetByUserId
     * -> Sp_tblUserRights_GetAllMethod @UserId, @ScreenName, @RightName = the user's RoleName,
     * @CompanyId, @Activity='GetByUserId'. RoleName "Admin" is granted Save/Update/Delete/Print/
     * CanView AllRecord outright (:17581).
     */
    public Map<String, Boolean> rights(Screen s) {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        r.put("save", admin);
        r.put("update", admin);
        r.put("delete", admin);
        r.put("print", admin);
        r.put("canViewAllRecord", admin);
        try {
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_tblUserRights_GetAllMethod",
                    DesktopProc.params("UserId", ctx.currentUserId(), "ScreenName", s.screenName,
                            "RightName", role == null ? "" : role, "CompanyId", ctx.currentCompanyId(),
                            "Activity", "GetByUserId"));
            for (Map<String, Object> row : rows) {
                String name = str(row.get("RightName")).trim();
                boolean v = toBool(row.get("Value"));
                if (admin) continue;
                switch (name) {
                    case "Save": r.put("save", v); break;
                    case "Update": r.put("update", v); break;
                    case "Delete": r.put("delete", v); break;
                    case "Print": r.put("print", v); break;
                    case "CanView AllRecord": r.put("canViewAllRecord", v); break;
                    default: break;
                }
            }
        } catch (Exception e) {
            LOG.warn("Rights for {} could not be read; denying", s.screenName, e);
        }
        return r;
    }

    public boolean hasRight(Screen s, String key) {
        return Boolean.TRUE.equals(rights(s).get(key));
    }

    /** CommonServices.GetERPFeatureById(id): the id is in USP_GetERPFeaturesByCompanyId's rows. */
    public boolean feature(int featureId) {
        try {
            for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                    DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                            "CompanyId", ctx.currentCompanyId()))) {
                if (toInt(r.get("Id")) == featureId) return true;
            }
        } catch (Exception e) {
            LOG.warn("ERP feature {} could not be read; treating as off", featureId, e);
        }
        return false;
    }

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal / the configrationsAllocation lookups: the
     * ConfigKey of the company's allocation for one ConfigDescription, or null when there is none.
     * Read through Sp_ConfigrationsAllocation_GetAllMethod
     * @Activity='GetConfigurationByOrgCompandConfigDescription' - the same reading ContraVoucherService
     * already uses.
     */
    public String config(String description) {
        try {
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod",
                    DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                            "CompanyId", ctx.currentCompanyId(), "ConfigDescription", description,
                            "Activity", "GetConfigurationByOrgCompandConfigDescription"));
            if (rows.isEmpty()) return null;
            Object v = rows.get(0).get("ConfigKey");
            return v == null ? null : String.valueOf(v);
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", description, e);
            return null;
        }
    }

    public boolean configBool(String description) {
        return toBool(config(description));
    }

    /**
     * IsBookingOffice: clsGlobalVariables.GetApplicationsAllocateToCompanyList
     * (USP_GetApplicationsAllocateToCompany, no parameters) has a row AppId == 5 for this
     * CompanyId whose IsActive is set (VoucherEntry.cs:1482, ContraVoucher.cs:893).
     */
    public boolean isBookingOffice() {
        try {
            int comp = ctx.currentCompanyId();
            for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetApplicationsAllocateToCompany]",
                    new LinkedHashMap<>())) {
                if (toInt(r.get("AppId")) == 5 && toInt(r.get("CompanyId")) == comp) {
                    return toBool(r.get("IsActive"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Applications allocated to company could not be read", e);
        }
        return false;
    }

    /** UserAccount.AppId - the application chosen at login; 0 when it cannot be resolved. */
    public int appId() {
        try {
            return ctx.currentAppId();
        } catch (Exception e) {
            UserAccount u = ctx.requireAccountingUser();
            return u.getAppId() == null ? 0 : u.getAppId();
        }
    }

    /** CommonServices.GenerateVoucherCode(documentTypeId) (:11064) -> BLL :334. */
    public int nextVoucherCode(Screen s) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "DocumentTypeId", s.documentTypeId, "FinancialYearId", ctx.currentFinancialYearId(),
                        "BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId(),
                        "Activity", "GenerateVoucherCodeByDocumentTypeId"));
        return rows.isEmpty() ? 0 : toInt(rows.get(0).get("VoucherCode"));
    }

    /**
     * Everything a form reads in its _Load: rights, the three ERP features, the configuration
     * values each form consults, IsBookingOffice, and the next voucher code.
     */
    public Map<String, Object> context(Screen s) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("screen", s.slug);
        r.put("screenName", s.screenName);
        r.put("documentTypeId", s.documentTypeId);
        r.put("rights", rights(s));
        r.put("multiCurrency", feature(6));      // MultiCurrencyFeature(): GetERPFeatureById(6)
        r.put("branchFeature", feature(17));     // BranchFeature = GetERPFeatureById(17)
        r.put("subsidiaryFeature", feature(4));  // SubsidiaryAccountAllownOnVouchers = GetERPFeatureById(4)
        r.put("isBookingOffice", isBookingOffice());
        r.put("appId", appId());
        r.put("userBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        r.put("nextVoucherCode", nextVoucherCode(s));
        Map<String, Object> cfg = new LinkedHashMap<>();
        for (String k : new String[] { "Job/Lot", "Base Currency", "BaseCurrencyRate",
                "InventoryRelatedAccountsShowInVouchers", "AllowExportPartiesOnJV", "CheqBook Enabled",
                "is Minus balance Allowed", "DisplayWarningforNegativeBalance",
                "DisableBothNegativeBalanceRestrictions", "AutoRemarksForPaymentThroughBank",
                /* CommonServices.GetDecimalConfiguration (:5376) - the grids' number formats. */
                "Default NoofDecimal Points For Amount", "Default NoofDecimal Points For Rate",
                "DefaultNoOfDecimalPointsForFcyAmount" }) {
            cfg.put(k, config(k));
        }
        r.put("config", cfg);
        return r;
    }

    // ============================================================================== dropdowns

    /** CommonServices.ProjectServiceBind (:844) -> Projects.GetAlldt -> Sp_Projects_GetAllMethod. */
    public List<Map<String, Object>> projects() {
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "MethodType", "GetAll"));
    }

    /** Projects.GetSubCostCenters(Org, Comp, UserId, AppId, 0) (BLL 0078:126) -> usp_getCostCenters;
     *  @AppId only when non-zero, @ParentCostCenterId never (0). */
    public List<Map<String, Object>> costCenters() {
        int app = appId();
        return DesktopProc.rows(jdbc, "usp_getCostCenters",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "UserId", ctx.currentUserId(),
                        "AppId", app != 0 ? app : null));
    }

    /** jobLot.GetList / GetAll (BLL 0594) -> SP_JobLot_ReadMethod @Activity='GetAll'. */
    public List<Map<String, Object>> jobLots() {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "Activity", "GetAll"));
    }

    /** BranchesAllocationToUser.GetBranchsAllocatedToUser (BLL 0017:85). */
    public List<Map<String, Object>> branches() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUser]",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "UserId", ctx.currentUserId()));
    }

    /** MultiCurrency.GetAll (BLL 0076:29) -> Sp_MultiCurrency_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> currencies() {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAll"));
    }

    /**
     * DatatableHelper.GetAccountsFromGlobalByTypeIds over clsGlobalVariables.AllAccountsWithCustomGroupId
     * (GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup(org, comp, 0, 0, "") ->
     * [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId @CompanyId). The helper applies the
     * type / PL-note filters in memory and keeps the first row per ChartOfAccountId, returning
     * Id, AccountTitle, AccountCode, ParentAccountTitle, AccountClass (= AccountClassName).
     */
    public List<Map<String, Object>> globalAccounts(int[] withTypeIds, int[] withoutTypeIds, int[] withoutPlNoteIds) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()));
        Set<Integer> with = set(withTypeIds), without = set(withoutTypeIds), withoutPl = set(withoutPlNoteIds);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int type = toInt(r.get("AccountTypeId"));
            if (!with.isEmpty() && !with.contains(type)) continue;
            if (!without.isEmpty() && without.contains(type)) continue;
            if (!withoutPl.isEmpty() && withoutPl.contains(toInt(r.get("PLNoteId")))) continue;
            int id = toInt(r.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", r.get("AccountTitle"));
            m.put("AccountCode", r.get("AccountCode"));
            m.put("ParentAccountTitle", r.get("ParentAccountTitle"));
            m.put("AccountClass", r.get("AccountClassName"));
            out.add(m);
        }
        return out;
    }

    /**
     * VoucherEntry.DebitAccountTitleFill (:496): both the debit and the credit combo bind the same
     * list - withoutTypeIds {4, 12, X} and withoutPlNoteIds {2} when InventoryRelatedAccountsShowInVouchers
     * is off, withoutTypeIds {X} alone when it is on, X = 22 unless multi-currency or
     * AllowExportPartiesOnJV is on (then 0).
     */
    public List<Map<String, Object>> voucherEntryAccounts() {
        boolean inventoryRelated = configBool("InventoryRelatedAccountsShowInVouchers");
        boolean allowExport = configBool("AllowExportPartiesOnJV");
        int x = (!feature(6) && !allowExport) ? 22 : 0;
        int[] without = inventoryRelated ? new int[] { x } : new int[] { 4, 12, x };
        int[] withoutPl = inventoryRelated ? null : new int[] { 2 };
        return globalAccounts(null, without, withoutPl);
    }

    /**
     * ExpenseVoucher.DetailAccountFill (:638): withTypeIds {11,13,14,20,21,23,X}, or with 12 added
     * when InventoryRelatedAccountsShowInVouchers is on; X = 22 unless multi-currency is on (0).
     */
    public List<Map<String, Object>> expenseDetailAccounts() {
        boolean inventoryRelated = configBool("InventoryRelatedAccountsShowInVouchers");
        int x = !feature(6) ? 22 : 0;
        int[] include = inventoryRelated
                ? new int[] { 11, 12, 13, 14, 20, 21, 23, x }
                : new int[] { 11, 13, 14, 20, 21, 23, x };
        return globalAccounts(include, null, null);
    }

    /**
     * ContraVoucher.AccountsComboBind (:1101): CommonServices.Accounts_GetAccountTitleByAccountTypeIds("2,15")
     * -> COAAllocation.Accounts_GetAccountTitleByAccountTypeIds (BLL 0648:520) ->
     * [dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds] @OrganizationId @CompanyId @AppId
     * @AccountTypeIds (@UserId only when non-zero). The detail combo copies the same rows (:1132).
     */
    public List<Map<String, Object>> contraAccounts() {
        int user = ctx.currentUserId();
        return DesktopProc.rows(jdbc, "[dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds]",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "AppId", appId(),
                        "AccountTypeIds", "2,15", "UserId", user != 0 ? user : null));
    }

    /**
     * ExpenseVoucher.AccountTitleFill (:596): CommonServices.CoaAllocationAccountTitleByAccountTypeIds("2,15")
     * (:1027) -> COAAllocation.GetAccountTitleByAccountTypeIds (BLL 0648:423) ->
     * Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountTypeIds' - a DIFFERENT
     * procedure from Contra's, with @AppId always and @UserId only when non-zero.
     */
    public List<Map<String, Object>> expenseCreditAccounts() {
        int user = ctx.currentUserId();
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "AppId", appId(),
                        "AccountTypeIds", "2,15", "UserId", user != 0 ? user : null,
                        "Activity", "GetAccountTitleByAccountTypeIds"));
    }

    /**
     * Screen 855 BindReferenceAccounts (:1523): CommonServices.CoaAllocationGetAllServiceBind (:864) ->
     * COAAllocation.GetAll (BLL 0648:154) -> Sp_COAAllocation_GetAllMethod @Activity='COAAllocationSearch'
     * (@UserId only when non-zero), then FilterDataTableByAccountTypes(dt, {3,5,6,7,8}, include) keeping
     * Id, AccountTitle, AccountCode, ParentAccountTitle, AccountClass.
     */
    public List<Map<String, Object>> referenceAccounts() {
        int user = ctx.currentUserId();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                        "UserId", user != 0 ? user : null, "Activity", "COAAllocationSearch"));
        Set<Integer> types = set(new int[] { 3, 5, 6, 7, 8 });
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!types.contains(toInt(r.get("AccountTypeId")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("AccountTitle", r.get("AccountTitle"));
            m.put("AccountCode", r.get("AccountCode"));
            m.put("ParentAccountTitle", r.get("ParentAccountTitle"));
            m.put("AccountClass", r.get("AccountClass"));
            out.add(m);
        }
        return out;
    }

    /** Screen 855 CurrencyFill (:1364): MultiCurrency.GetMultiCurrencywithExchangeRate(CompanyId) ->
     *  [dbo].[usp_getMultiCurrencywithExchangeRate] @CompanyId. */
    public List<Map<String, Object>> currenciesWithRate() {
        return DesktopProc.rows(jdbc, "[dbo].[usp_getMultiCurrencywithExchangeRate]",
                DesktopProc.params("CompanyId", ctx.currentCompanyId()));
    }

    /** Screen 855 LocationType (:1330): VoucherHead.GetLocationType -> usp_getLocationType (no parameters). */
    public List<Map<String, Object>> locationTypes() {
        return DesktopProc.rows(jdbc, "usp_getLocationType", new LinkedHashMap<>());
    }

    /**
     * VoucherHead.BindSubsidiaryAccount(CompanyId, DetailAccountId, TypeId) (BLL 0654:2323) ->
     * USP_GetSubsidiaryAccountsByParentAccount @CompanyId, @SubsidiaryTypeIds (a CLR null is not
     * sent), @AccountId only when non-zero.
     */
    public List<Map<String, Object>> subsidiaryAccounts(int accountId, String typeIds) {
        return DesktopProc.rows(jdbc, "USP_GetSubsidiaryAccountsByParentAccount",
                DesktopProc.params("CompanyId", ctx.currentCompanyId(),
                        "SubsidiaryTypeIds", (typeIds == null || typeIds.isEmpty()) ? null : typeIds,
                        "AccountId", accountId != 0 ? accountId : null));
    }

    /**
     * The history "Account Title" filter - VoucherReports.LedgerByJobLotDropDownAndLists
     * (BLL 0141) -> Sp_Vouchers_LedgerByJobLot_DropDownAndLists, rows whose ActivityType is
     * "Account Header". ComboBindForHistory (Contra :934, Expense :874) passes
     * CompanyId = UserAccount.OrganizationId - reproduced as the desktop sends it.
     */
    public List<Map<String, Object>> historyAccounts(Screen s) {
        int user = ctx.currentUserId();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_LedgerByJobLot_DropDownAndLists",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentOrganizationId(), "AppId", appId(), "UserId", user,
                        "DocumentTypeIds", String.valueOf(s.documentTypeId)));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!"Account Header".equals(str(r.get("ActivityType")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("AccountTitle", r.get("name"));
            out.add(m);
        }
        return out;
    }

    /**
     * CheqBookHeader.OutstandingCheqNo (BLL 0647:61) -> SP_CheqBookHeader_GetAllMethod
     * @MethodType='OutstandingCheqNo', @RecId only when the form's RecId is non-zero, @Id never
     * (ActionId is not set by the callers).
     */
    public List<Map<String, Object>> outstandingCheques(int bankId, int recId) {
        return DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "BankId", bankId,
                        "RecId", recId != 0 ? recId : null, "MethodType", "OutstandingCheqNo"));
    }

    /** VoucherHead.ReadByCurrentBalanceByDateAndAccountId (BLL 0654:491), column Balance. */
    public double accountBalance(int accountId, String voucherDate) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Activity", "ReadByCurrentBalanceByDateAndAccountId",
                        "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                        "FinancialYearId", ctx.currentFinancialYearId(), "RefAccountId", accountId,
                        "VoucherDate", voucherDate));
        return rows.isEmpty() ? 0d : toDouble(rows.get(0).get("Balance"));
    }

    public boolean accountBalanceHasRow(int accountId, String voucherDate) {
        return !DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Activity", "ReadByCurrentBalanceByDateAndAccountId",
                        "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                        "FinancialYearId", ctx.currentFinancialYearId(), "RefAccountId", accountId,
                        "VoucherDate", voucherDate)).isEmpty();
    }

    /** CommonServices.GetSubsiadiaryBalance (:374) -> VoucherHead.GetGLAndSubsidiaryCurrentBalance
     *  (BLL 0654:534), column BalanceAmount. */
    public double subsidiaryBalance(int glAccountId, int subsidiaryId, String date) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetGLAndSubsidiaryCurrentBalance",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(), "ToDate", date,
                        "GlAccountId", glAccountId, "SubSaidiaryId", subsidiaryId));
        return rows.isEmpty() ? 0d : toDouble(rows.get(0).get("BalanceAmount"));
    }

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (BLL 0654:2679),
     *  @DocumentTypeIds = the form's own type, @DMultiCurrencyIds = the chosen currency. */
    public Map<String, Object> lastExchangeRate(Screen s, int currencyId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                        "CompanyId", ctx.currentCompanyId(),
                        "DocumentTypeIds", String.valueOf(s.documentTypeId),
                        "DMultiCurrencyIds", String.valueOf(currencyId),
                        "Activity", "GetMultiCurrencyAndLastRate"));
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("found", !rows.isEmpty());
        r.put("LastExchRate", rows.isEmpty() ? 0d : toDouble(rows.get(0).get("LastExchRate")));
        return r;
    }

    // =================================================================================== history

    /**
     * VoucherHead.VoucherFormHistory (BLL 0654:617) as each form's HistoryFill builds it.
     * @CanViewAllRecord = the "CanView AllRecord" right; @EntryUser only when that right is off;
     * @FinancialYearId when non-zero; the date pair of the chosen radio, each only when its
     * picker is checked; @DocNoFrom/@DocNoTo when non-zero; @AccountId when non-zero (Contra and
     * Expense only - VoucherEntry has no such filter); @IsApproved unless the status is "All";
     * @AppId when non-zero (Contra and Expense set it; VoucherEntry never does).
     */
    public List<Map<String, Object>> history(Screen s, String dateType, String fromDate, String toDate,
                                             Integer fromDocNo, Integer toDocNo, Integer accountId,
                                             String approvedStatus) {
        boolean canViewAll = hasRight(s, "canViewAllRecord");
        int user = ctx.currentUserId();
        int year = ctx.currentFinancialYearId();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", ctx.currentOrganizationId());
        p.put("CompanyId", ctx.currentCompanyId());
        p.put("UserId", user);
        p.put("DocumentTypeName", String.valueOf(s.documentTypeId));
        p.put("CanViewAllRecord", canViewAll);
        if (year != 0) p.put("FinancialYearId", year);
        String f = blank(fromDate) ? null : fromDate.trim();
        String t = blank(toDate) ? null : toDate.trim();
        if ("entry".equalsIgnoreCase(dateType)) {
            p.put("EntryFromDate", f); p.put("EntryToDate", t);
        } else if ("modify".equalsIgnoreCase(dateType)) {
            p.put("ModifyFromDate", f); p.put("ModifyToDate", t);
        } else if ("approved".equalsIgnoreCase(dateType)) {
            p.put("ApprovedFromDate", f); p.put("ApprovedToDate", t);
        } else {
            p.put("FromDate", f); p.put("ToDate", t);
        }
        if (fromDocNo != null && fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != null && toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (s.historyHasAccountFilter && accountId != null && accountId != 0) p.put("AccountId", accountId);
        if (!canViewAll) p.put("EntryUser", user);
        if (!"all".equalsIgnoreCase(approvedStatus)) {
            p.put("IsApproved", "approved".equalsIgnoreCase(approvedStatus));
        }
        if (s.historySendsAppId) {
            int app = appId();
            if (app != 0) p.put("AppId", app);
        }
        return DesktopProc.rows(jdbc, "USP_VoucherFormHistory", p);
    }

    // ================================================================================ read by id

    /**
     * VoucherHead.GetByID (BLL 0654:377) -> DAL GetData: Sp_Vouchers_GetMethods 'ReadByID', then
     * 'VoucherDetail_ReadByVoucherHeadID' and 'voucherCostCenterDetailByHeaderId' for that id.
     * ReadByID filters on @Id alone, so the row is checked against the signed-in tenancy and the
     * screen's DocumentTypeId before anything is returned.
     */
    public Map<String, Object> read(Screen s, int id) {
        Map<String, Object> head = readHead(id);
        if (head == null) return null;
        if (toInt(head.get("OrganizationId")) != ctx.currentOrganizationId()
                || toInt(head.get("CompanyId")) != ctx.currentCompanyId()
                || toInt(head.get("DocumentTypeId")) != s.documentTypeId) {
            return null;
        }
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("head", head);
        r.put("details", readDetails(id));
        r.put("costCenters", DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "voucherCostCenterDetailByHeaderId")));
        return r;
    }

    public Map<String, Object> readHead(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "ReadByID"));
        return rows.isEmpty() ? null : rows.get(0);
    }

    public List<Map<String, Object>> readDetails(int id) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "VoucherDetail_ReadByVoucherHeadID"));
    }

    /** A voucher being updated must belong to this tenant and this screen's document type. */
    public Map<String, Object> requireOwnHead(Screen s, int id) {
        Map<String, Object> head = readHead(id);
        if (head == null
                || toInt(head.get("OrganizationId")) != ctx.currentOrganizationId()
                || toInt(head.get("CompanyId")) != ctx.currentCompanyId()
                || toInt(head.get("DocumentTypeId")) != s.documentTypeId) {
            throw new IllegalArgumentException("Record Not Update  " + id);
        }
        return head;
    }

    // ================================================================================== helpers

    private static Set<Integer> set(int[] a) {
        Set<Integer> s = new HashSet<>();
        if (a != null) for (int v : a) s.add(v);
        return s;
    }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    public static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (Exception e) { return 0; }
    }

    public static double toDouble(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0d;
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (Exception e) { return 0d; }
    }

    /** Architecture.Common.Conversion.ToBool: "1"/"true" are true; anything else false. */
    public static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
