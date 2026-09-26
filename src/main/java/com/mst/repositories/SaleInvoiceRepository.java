package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

/**
 * Sale Invoice (InvfrmSaleInvoice, DocumentTypeId 95) - every database call the desktop form, its BLL
 * (0580 InvSaleInvoice, 0612/0613 SaleInvoiceFinancial) and its DAL (0433 InvSaleInvoice.SetData) make,
 * with the same procedures, the same parameters and the same "send only when" guards.
 *
 * Nothing here creates or changes a procedure, table or row outside what the desktop itself writes.
 * Tenancy (organization, company, branch, user, financial year) always comes from the signed-in
 * session - never from the request.
 */
@Repository
public class SaleInvoiceRepository {
    public static final int DOCUMENT_TYPE_ID = 95;
    /** base.Name of the form = the rights screen name and the ScreenName written on the record. */
    public static final String SCREEN_NAME = "InvfrmSaleInvoice";

    private final JdbcTemplate jdbc;
    public SaleInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================== plumbing

    public List<Map<String, Object>> q(String sql, Object... args) { return jdbc.queryForList(sql, args); }

    /** EXEC proc with only the named parameters present in the map (the desktop's "send only when" lists). */
    public List<Map<String, Object>> proc(String proc, LinkedHashMap<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") || proc.contains(".") ? proc : "dbo." + proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue() instanceof LocalDateTime ? Timestamp.valueOf((LocalDateTime) e.getValue()) : e.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    public static LinkedHashMap<String, Object> params(Object... kv) {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i < kv.length; i += 2) m.put((String) kv[i], kv[i + 1]);
        return m;
    }

    /**
     * GenericProvider.SetProc (DAL 0207:283): every non-virtual public property becomes @Name and the
     * first column of the first row is returned (ExecuteScalar). A null reference is "not supplied",
     * exactly as SqlParameter with Value == null behaves, so the procedure default applies. A
     * non-nullable DateTime left unset would be DateTime.MinValue on the desktop and fail with
     * "SqlDateTime overflow" - it fails here too, by name, instead of writing a date.
     */
    public int setProc(String proc, Object model) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.contains(".") ? proc : "dbo." + proc).append(' ');
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Field f : model.getClass().getFields()) {
            if (Modifier.isStatic(f.getModifiers())) continue;
            if (f.isAnnotationPresent(SaleInvoiceModels.NotParam.class)) continue;
            Object v;
            try { v = f.get(model); } catch (IllegalAccessException e) { continue; }
            if (v == null) {
                if (f.getType() == LocalDateTime.class && !f.isAnnotationPresent(SaleInvoiceModels.Nullable.class))
                    throw new IllegalStateException("SqlDateTime overflow: " + model.getClass().getSimpleName() + "." + f.getName() + " was not set");
                continue;
            }
            Object bound;
            if (v instanceof LocalDateTime) bound = new SqlParameterValue(Types.TIMESTAMP, Timestamp.valueOf((LocalDateTime) v));
            else if (v instanceof BigDecimal) bound = new SqlParameterValue(Types.DECIMAL, v);
            else if (v instanceof Double) bound = new SqlParameterValue(Types.DOUBLE, v);
            else if (v instanceof Boolean) bound = new SqlParameterValue(Types.BIT, v);
            else if (v instanceof Integer) bound = new SqlParameterValue(Types.INTEGER, v);
            else bound = new SqlParameterValue(Types.NVARCHAR, String.valueOf(v));
            sql.append(first ? "" : ", ").append('@').append(f.getName()).append("=?");
            args.add(bound);
            first = false;
        }
        Integer id = ProcExec.call(jdbc, sql.toString(), args.toArray());
        return id == null ? 0 : id;
    }

    public void run(String sql, Object... args) { ProcExec.run(jdbc, sql, args); }

    // ============================================================================== session facts

    /** CommonServices.GetERPFeatureById(id) - USP_GetERPFeaturesByCompanyId contains the Id. */
    public boolean feature(UserAccount u, int id) {
        return q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())
                .stream().anyMatch(r -> i(r.get("Id")) == id);
    }

    /** GetConfigurationFromAllocation / GetConfigValueFromGlobal - the ConfigKey text, "" when absent. */
    public String config(UserAccount u, String description) {
        return config(u.getOrganizationId(), u.getCompanyId(), description);
    }
    public String config(int org, int company, String description) {
        var rows = q("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                org, company, description, "GetConfigurationByOrgCompandConfigDescription");
        Object v = rows.isEmpty() ? null : col(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }
    /** Conversion.ToBool on a config value. */
    public static boolean truthy(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    /**
     * CommonServices.SetRightsValueInRightsObject("InvfrmSaleInvoice") - tblUserRights.GetByUserId, the
     * role name compared to "Admin" exactly as the desktop does (case-sensitive), Delete never forced.
     */
    public Map<String, Boolean> rights(UserAccount u, String role) {
        boolean admin = "Admin".equals(role);
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("Save", admin); r.put("Update", admin); r.put("Delete", admin); r.put("Print", admin);
        r.put("CanViewAllRecord", admin); r.put("View", false);
        for (var row : q("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                u.getId(), SCREEN_NAME, role, u.getCompanyId(), "GetByUserId")) {
            String name = s(col(row, "RightName")).trim();
            boolean value = b(col(row, "Value"));
            switch (name) {
                case "View": r.put("View", value); break;
                case "Save": r.put("Save", admin || value); break;
                case "Update": r.put("Update", admin || value); break;
                case "Print": r.put("Print", admin || value); break;
                case "CanView AllRecord": r.put("CanViewAllRecord", admin || value); break;
                case "Delete": r.put("Delete", value); break;
                default: break;
            }
        }
        return r;
    }

    // ============================================================================== form open

    /** GenerateDocumentNo - Sp_InvSaleInvoice_GetAllMethod 'GenerateCode', reading DocNo. */
    public int nextDocNo(UserAccount u, int financialYearId) {
        var rows = proc("[Sp_InvSaleInvoice_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "DocNo"));
    }

    /** GenerateBranchSrNo - 'GenerateBranchCode' with the user's branch, reading BranchSrNo. */
    public int nextBranchSrNo(UserAccount u, int financialYearId) {
        var rows = proc("[Sp_InvSaleInvoice_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", n(u.getBranchesId()), "DocumentTypeId", DOCUMENT_TYPE_ID, "FinancialYearId", financialYearId,
                "Activity", "GenerateBranchCode"));
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "BranchSrNo"));
    }

    /** VoucherHead.GetLocationType - usp_getLocationType, no parameters. */
    public List<Map<String, Object>> locationTypes() { return q("EXEC dbo.usp_getLocationType"); }

    /** CurrencyDbCall - MultiCurrency.GetAll. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        return q("EXEC dbo.Sp_MultiCurrency_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", u.getOrganizationId(), u.getCompanyId(), "ReadAll");
    }

    /**
     * SupplierDtFillFromGlobal (:929) over clsGlobalVariables.globalSupplierCustomer, which
     * DatatableHelper.GlobalSupplierCustomerListsFillDbCall fills from USP_GetVendorsAndCustomersWithCityName
     * dropping CustomerGroupId 7, 9, 10 (and 8, 13 when ShowBothVendorAndCustomerOnSalesPurchase = 1).
     * The form drops group 7 again and, with ERP feature 4, keeps PartyTypeId 2 only.
     */
    public List<Map<String, Object>> customers(UserAccount u, boolean subsidiary) {
        int showBoth = i(config(u, "ShowBothVendorAndCustomerOnSalesPurchase").trim());
        Set<Integer> always = Set.of(7, 9, 10), withShowBoth = Set.of(7, 8, 9, 10, 13);
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : q("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            int group = i(col(r, "CustomerGroupId"));
            if (always.contains(group) || (showBoth == 1 && withShowBoth.contains(group))) continue;
            if (subsidiary && i(col(r, "PartyTypeId")) != 2) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("CompanyName", s(col(r, "CompanyName"))); x.put("PartyCode", s(col(r, "PartyCode")));
            x.put("GlAccountId", i(col(r, "GlAccountId"))); x.put("CityId", i(col(r, "CityId"))); x.put("CityName", s(col(r, "CityName")));
            x.put("MobileNo", s(col(r, "MobilePersonal")));
            out.add(x);
        }
        return out;
    }

    /**
     * AccountlstDtFillFromGlobal (:1057) - dtAccountlst {Id, SupplierCustomerId, AccountTitle}.
     * Feature 4: getGlobalVendorsAndCustomersForTransporters as (GlAccountId, Id, CompanyName).
     * Otherwise AllAccountsWithCustomGroupId (USP_GETAllAccountsFromCustomGroups) without AccountTypeId
     * {2,15,22} when DebitAmountChargetoExpenseAcFreightGrid is on, else without {2,11,12,13,14,15,20,21,22};
     * first row per ChartOfAccountId, SupplierCustomerId 0.
     */
    public List<Map<String, Object>> accounts(UserAccount u, boolean subsidiary, boolean freightDebitToExpenses) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (var r : q("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", i(col(r, "GlAccountId"))); x.put("SupplierCustomerId", i(col(r, "Id"))); x.put("AccountTitle", s(col(r, "CompanyName")));
                out.add(x);
            }
            return out;
        }
        Set<Integer> excluded = freightDebitToExpenses ? Set.of(2, 15, 22) : Set.of(2, 11, 12, 13, 14, 15, 20, 21, 22);
        Set<Integer> seen = new HashSet<>();
        for (var r : q("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (excluded.contains(i(col(r, "AccountTypeId")))) continue;
            int id = i(col(r, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", id); x.put("SupplierCustomerId", 0); x.put("AccountTitle", s(col(r, "AccountTitle")));
            out.add(x);
        }
        return out;
    }

    /**
     * CommissionDebitAccountBind (:2344) - only when DebitAmountChargetoExpenseAcOfCommission is on:
     * CoaAllocationAccountTitleByAccountTypeIds("3,11,12,13,14,20,21") (feature 4 uses dtAccountlst instead).
     */
    public List<Map<String, Object>> commissionDebitAccounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : q("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, @AccountTypeIds=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), n(u.getAppId()), "3,11,12,13,14,20,21", u.getId(), "GetAccountTitleByAccountTypeIds")) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("AccountTitle", s(col(r, "AccountTitle"))); x.put("AccountCode", s(col(r, "AccountCode")));
            out.add(x);
        }
        return out;
    }

    /** OtherItemsdtFillDbCall - InventoryItemsOther.GetAll: Sp_InventoryItemsOther_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> otherItems(UserAccount u) { return otherItems(u.getOrganizationId(), u.getCompanyId()); }
    public List<Map<String, Object>> otherItems(int org, int company) {
        return q("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?", "ReadAll", org, company);
    }

    /** CommissionUOMFill - CommonServices.StaticColumnsService("GetCommissionUom"). */
    public List<Map<String, Object>> commissionUoms() { return q("EXEC dbo.SpStaticColumnNames @Activity=?", "GetCommissionUom"); }

    /** clsGlobalVariables.globalPaymentTerm - Sp_InvDueTerms_GetAllMethod 'GetAll'. */
    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return q("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", u.getOrganizationId(), u.getCompanyId(), "GetAll");
    }

    /** cmbCurrency_Leave - VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher for DocumentTypeId 95. */
    public Double lastExchangeRate(UserAccount u, int currencyId) {
        var rows = q("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?, @DMultiCurrencyIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID), String.valueOf(currencyId), "GetMultiCurrencyAndLastRate");
        return rows.isEmpty() ? null : d(col(rows.get(0), "LastExchRate"));
    }

    /**
     * cmbsuppliername_ValueChanged (:6446): without feature 4, GetAccountsBalancesOpeningCurrentAndClosing
     * (GL, ActiveYr.Start_Period .. DocDate) reading ClosingBalance; with feature 4,
     * USP_Accounts_GetSubsidiaryLedgerBalance reading Balance.
     */
    public double ledgerBalance(UserAccount u, boolean subsidiary, int customerId, int glAccountId, LocalDateTime yearStart, LocalDateTime docDate) {
        if (!subsidiary) {
            var rows = q("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @AccountId=?, @FromDate=?, @ToDate=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), glAccountId, Timestamp.valueOf(yearStart), Timestamp.valueOf(docDate),
                    "GetAccountsBalancesOpeningCurrentAndClosing");
            return rows.isEmpty() ? 0d : d(col(rows.get(0), "ClosingBalance"));
        }
        var rows = q("EXEC dbo.USP_Accounts_GetSubsidiaryLedgerBalance @OrganizationId=?, @CompanyId=?, @SupplierCustomerId=?, @ToDate=?",
                u.getOrganizationId(), u.getCompanyId(), customerId, Timestamp.valueOf(docDate));
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "Balance"));
    }

    /**
     * clsGlobalVariables.ActiveYr.Start_Period - the row of Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId
     * whose Id is the session's financial year.
     */
    public LocalDateTime financialYearStart(UserAccount u, int financialYearId) {
        for (var r : q("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId()))
            if (i(col(r, "Id")) == financialYearId) return dt(col(r, "Start_Period"));
        return null;
    }

    // ============================================================================== history

    /**
     * HistoryCombosBranchDbCall (:686): SaleInvoiceBranchWise -> only the user's branch; otherwise
     * USP_GetBranchsAllocatedToUserFromSaleInvoice @UserId, @DocumentTypeId=95 (BranchId, BranchName).
     */
    public List<Map<String, Object>> historyBranches(UserAccount u, boolean branchImplemented) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchImplemented) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", n(u.getBranchesId())); x.put("BranchName", branchName(u));
            out.add(x);
            return out;
        }
        for (var r : q("EXEC dbo.USP_GetBranchsAllocatedToUserFromSaleInvoice @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), DOCUMENT_TYPE_ID)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "BranchId"))); x.put("BranchName", s(col(r, "BranchName")));
            out.add(x);
        }
        return out;
    }

    /** UserAccount.BranchName - the user's branch row of Sp_Branches_GetAllMethod 'GetAll'. */
    public String branchName(UserAccount u) {
        for (var r : q("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", u.getOrganizationId(), u.getCompanyId(), "GetAll"))
            if (i(col(r, "Id")) == n(u.getBranchesId())) return s(col(r, "BranchName"));
        return "";
    }

    /**
     * HistoryComboDbCall (:752) - InvSaleInvoice.AllComboBindAgainstSaleInvoice: Usp_AllComboAgainstSaleInvoice
     * @OrganizationId, @CompanyId, @AppId, @UserId, @DocumentTypeIds='95', @Activity='Supplier'. The BLL
     * does not forward BranchesIds even though the form computes it, so neither does this.
     */
    public List<Map<String, Object>> historyCustomers(UserAccount u) {
        return q("EXEC dbo.Usp_AllComboAgainstSaleInvoice @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), n(u.getAppId()), u.getId(), String.valueOf(DOCUMENT_TYPE_ID), "Supplier");
    }

    /**
     * FillHistoryGrid (:4854) -> InvSaleInvoice.FormHistory (BLL 0580:555). The form never assigns
     * PI.EntryUser, so when the user lacks "CanView AllRecord" the BLL sends @EntryUser = 0, and it never
     * assigns ScreenName or PaymenetTermId, so neither is sent. Reproduced as-is.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, String dateMode,
                                             LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo,
                                             int customerId, String branchIds) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        String fromKey, toKey;
        switch (dateMode == null ? "" : dateMode) {
            case "entry": fromKey = "EntryFromDate"; toKey = "EntryToDate"; break;
            case "modify": fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; break;
            case "approved": fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; break;
            default: fromKey = "FromDate"; toKey = "ToDate"; break;
        }
        if (from != null) p.put(fromKey, from);
        if (to != null) p.put(toKey, to);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (!canViewAll) p.put("EntryUser", 0);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        p.put("Activity", "FormHistory");
        return proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    // ============================================================================== Load GDN (frmLoadGDN)

    /**
     * frmLoadGDN.BranchesFill (:119): SaleInvoiceBranchWise -> the user's branch; otherwise
     * InvGdn.GetBranchesAllocatedToUserFromGdn(@UserId, @DocumentTypeId=86).
     */
    public List<Map<String, Object>> gdnBranches(UserAccount u, boolean branchImplemented) {
        if (branchImplemented) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("BranchId", n(u.getBranchesId())); x.put("BranchName", branchName(u));
            return new ArrayList<>(List.of(x));
        }
        return q("EXEC dbo.USP_GetBranchsAllocatedToUserFromGdn @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), 86);
    }

    /** PendingGdnLoad (:200) - InvSaleInvoice.PendingGdnforLoadingInvoiceRice, DocumentTypeId 86. */
    public List<Map<String, Object>> pendingGdn(UserAccount u, int financialYearId, LocalDateTime from, LocalDateTime to, String branchIds) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", 86);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        p.put("Activity", "GetPendingGdnForSaleInvoiceRice");
        return proc("Sp_InvSaleInvoice_GetAllMethod", p);
    }

    /** frmLoadGDN.grd_SelectionChanged (:325) - InvGdn.GetByID, detail activity chosen as DAL InvGdn.GetData does. */
    public List<Map<String, Object>> gdnDetail(UserAccount u, int gdnId) {
        var h = q("EXEC dbo.Sp_InvGdn_GetAllMethod @Id=?, @Activity=?", gdnId, "GetById");
        if (h.isEmpty()) return List.of();
        var head = h.get(0);
        if (i(col(head, "OrganizationId")) != u.getOrganizationId() || i(col(head, "CompanyId")) != u.getCompanyId()) return List.of();
        String activity = i(col(head, "DocumentTypeId")) == 86 && "Export".equals(s(col(head, "TransporterDocRef")))
                ? "GetExportGDNDetailByGdnId" : "GetGDNDetailByGdnId";
        return q("EXEC dbo.Sp_InvGdn_GetAllMethod @InvGdnMainId=?, @Activity=?", gdnId, activity);
    }

    /** LoadDataDetailGridAgainstGP - InvSaleInvoice.GdnLoadForSaleInvoice: [usp_getGdnDataForSaleInvoiceByIds] @GdnIds. */
    public List<Map<String, Object>> gdnLoad(String gdnIds) { return q("EXEC [dbo].[usp_getGdnDataForSaleInvoiceByIds] @GdnIds=?", gdnIds); }

    /** LoadFreightData - 'GetTransporterAndFreightFromGdn' @GdnIds. */
    public List<Map<String, Object>> freightFromGdn(String gdnIds) {
        return q("EXEC [dbo].[Sp_InvSaleInvoice_GetAllMethod] @GdnIds=?, @Activity=?", gdnIds, "GetTransporterAndFreightFromGdn");
    }

    /** LoadExpensesFromGdn - [dbo].[USP_InvGdnExpensesByInvGdnIdIds] @GdnIds. */
    public List<Map<String, Object>> expensesFromGdn(String gdnIds) { return q("EXEC [dbo].[USP_InvGdnExpensesByInvGdnIdIds] @GdnIds=?", gdnIds); }

    /** LoadExpData - 'GetItemQtyAndItemRateFromSO' @GdnIds. */
    public List<Map<String, Object>> itemQtyAndRateFromSo(String gdnIds) {
        return q("EXEC [dbo].[Sp_InvSaleInvoice_GetAllMethod] @GdnIds=?, @Activity=?", gdnIds, "GetItemQtyAndItemRateFromSO");
    }

    /** LoadPaymentDetail - InvSaleInvoice.SaleOrderPaymentTermDetailBySoIds: Sp_SaleOrder_GetAllMethod @OrderIds. */
    public List<Map<String, Object>> saleOrderPaymentTerms(String orderIds) {
        return q("EXEC [dbo].[Sp_SaleOrder_GetAllMethod] @OrderIds=?, @Activity=?", orderIds, "SaleOrderPaymentTermDetailBySoIds");
    }

    /** CommissionAmountCalculateInCaseofPolicy - USP_GetCommissionRateByCustomerAndItemIdFromCommPolicy. */
    public List<Map<String, Object>> commissionPolicy(UserAccount u, int customerId, LocalDateTime effectiveDate, String itemIds, int policyTypeId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CustomerId", customerId, "ItemIds", itemIds, "EffectiveDate", effectiveDate);
        if (policyTypeId != 0) p.put("PolicyTypeId", policyTypeId);
        return proc("[dbo].[USP_GetCommissionRateByCustomerAndItemIdFromCommPolicy]", p);
    }

    /** CommonServices.GetGlAccountIdBySupplierCustomerId - first column of 'GetGlAccountIdBySupplierCustomerId'. */
    public int glAccountIdOfParty(UserAccount u, int supplierCustomerId) {
        var rows = q("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @SupplierCustomerId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), supplierCustomerId, "GetGlAccountIdBySupplierCustomerId");
        if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        return i(rows.get(0).values().iterator().next());
    }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'). */
    public List<Map<String, Object>> uomScheduleByItem(int org, int company, int itemId) {
        return q("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?", org, company, itemId, "ReadByItemID");
    }

    // ============================================================================== read / delete

    /** InvSaleInvoice.GetByID - DAL 0433 GetData: 'ReadById' then the six child reads. */
    public Map<String, Object> readById(int id) {
        var head = q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
        if (head.isEmpty()) return null;
        Map<String, Object> out = new LinkedHashMap<>(head.get(0));
        int type = i(col(head.get(0), "DocumentTypeId"));
        out.put("details", q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id,
                type == 96 ? "SaleDetailTradingReadByInvSaleInvoiceId" : "SaleDetailReadByInvSaleInvoiceId"));
        out.put("expenses", q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
        out.put("freights", q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
        out.put("journals", q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
        out.put("commissions", q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvSaleInvoiceCommission_ReadBySaleInvoiceID"));
        out.put("paymentTerms", q("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvSaleInvoicePaymentTerm_ReadBySaleInvoiceID"));
        return out;
    }

    /** CommonServices.VoucherHeadIdGet -> 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    public int voucherHeadId(int org, int company, int documentTypeId, int id) {
        var rows = q("EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId", org, company, documentTypeId, id);
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "Id"));
    }

    /** btnDelete_Click (:4727) - InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete. */
    public void delete(UserAccount u, int id) {
        run("EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?, @CompanyId=?, @Id=?, @DocumentTypeId=?, @UserId=?",
                u.getOrganizationId(), u.getCompanyId(), id, DOCUMENT_TYPE_ID, u.getId());
    }

    // ============================================================================== docType-parameterised reads (171 and later)

    /** SetRightsValueInRightsObject(screenName) for another form of the family. */
    public Map<String, Boolean> rights(UserAccount u, String role, String screenName) {
        boolean admin = "Admin".equals(role);
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("Save", admin); r.put("Update", admin); r.put("Delete", admin); r.put("Print", admin);
        r.put("CanViewAllRecord", admin); r.put("View", false);
        for (var row : q("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                u.getId(), screenName, role, u.getCompanyId(), "GetByUserId")) {
            String name = s(col(row, "RightName")).trim();
            boolean value = b(col(row, "Value"));
            switch (name) {
                case "View": r.put("View", value); break;
                case "Save": r.put("Save", admin || value); break;
                case "Update": r.put("Update", admin || value); break;
                case "Print": r.put("Print", admin || value); break;
                case "CanView AllRecord": r.put("CanViewAllRecord", admin || value); break;
                case "Delete": r.put("Delete", value); break;
                default: break;
            }
        }
        return r;
    }

    /** InvSaleInvoice.GenerateInvSaleInvoiceCode for the given DocumentTypeId. */
    public int nextDocNo(UserAccount u, int financialYearId, int documentTypeId) {
        var rows = proc("[Sp_InvSaleInvoice_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "DocNo"));
    }

    /** VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher with DocumentTypeIds = the form's type. */
    public Double lastExchangeRate(UserAccount u, int currencyId, int documentTypeId) {
        var rows = q("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?, @DMultiCurrencyIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(documentTypeId), String.valueOf(currencyId), "GetMultiCurrencyAndLastRate");
        return rows.isEmpty() ? null : d(col(rows.get(0), "LastExchRate"));
    }

    /** InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete for the given DocumentTypeId. */
    public void delete(UserAccount u, int id, int documentTypeId) {
        run("EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?, @CompanyId=?, @Id=?, @DocumentTypeId=?, @UserId=?",
                u.getOrganizationId(), u.getCompanyId(), id, documentTypeId, u.getId());
    }

    /**
     * InvSaleInvoice.FormHistory (BLL 0580:555) as frmSaleInvoiceAgainstGdnWithoutWb.GetAll (:2701) fills it:
     * no branches, and PI.EntryUser = the user when "CanView AllRecord" is off.
     */
    public List<Map<String, Object>> historyByUser(UserAccount u, int documentTypeId, int financialYearId, boolean canViewAll, String dateMode,
                                                   LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo, int customerId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        String fromKey, toKey;
        switch (dateMode == null ? "" : dateMode) {
            case "entry": fromKey = "EntryFromDate"; toKey = "EntryToDate"; break;
            case "modify": fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; break;
            case "approved": fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; break;
            default: fromKey = "FromDate"; toKey = "ToDate"; break;
        }
        if (from != null) p.put(fromKey, from);
        if (to != null) p.put(toKey, to);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("Activity", "FormHistory");
        return proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /** InvGdn.GetDataForDropDownFromGdn(org, company) - USP_GetDataForDropDownFromGdn, the 'Supplier' rows as {Id, Customer}. */
    public List<Map<String, Object>> customersFromGdn(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : q("EXEC dbo.USP_GetDataForDropDownFromGdn @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (!"Supplier".equals(s(col(r, "Activity")))) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("Customer", s(col(r, "ReferenceName")));
            out.add(x);
        }
        return out;
    }

    /** frmLoadGDN.BranchesFill with DocumentTypeIds set: GetBranchesAllocatedToUserFromGdn(@UserId, @DocumentTypeIds). */
    public List<Map<String, Object>> gdnBranches(UserAccount u, boolean branchImplemented, String documentTypeIds) {
        if (branchImplemented) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("BranchId", n(u.getBranchesId())); x.put("BranchName", branchName(u));
            return new ArrayList<>(List.of(x));
        }
        return q("EXEC dbo.USP_GetBranchsAllocatedToUserFromGdn @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), documentTypeIds);
    }

    /** frmLoadGDN.PendingGdnLoad with DocumentTypeIds set (DocumentTypeId 0 is not sent by the BLL). */
    public List<Map<String, Object>> pendingGdn(UserAccount u, int financialYearId, LocalDateTime from, LocalDateTime to, String branchIds, String documentTypeIds) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", documentTypeIds);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        p.put("Activity", "GetPendingGdnForSaleInvoiceRice");
        return proc("Sp_InvSaleInvoice_GetAllMethod", p);
    }

    /** InvSaleInvoice.GdnLoadForSaleInvoiceWithoutWeighBridge - [usp_getGdnDataForSaleInvoiceWithoutWeightBridgeByIds] @GdnIds. */
    public List<Map<String, Object>> gdnLoadWithoutWb(String gdnIds) {
        return q("EXEC [dbo].[usp_getGdnDataForSaleInvoiceWithoutWeightBridgeByIds] @GdnIds=?", gdnIds);
    }

    /** CommonServices.GetVendorsAndCustomers(PartyTypeId) - USP_GetVendorsAndCustomers (@PartyTypeId only when not 0). */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u, int partyTypeId) {
        if (partyTypeId != 0)
            return q("EXEC dbo.USP_GetVendorsAndCustomers @OrganizationId=?, @CompanyId=?, @PartyTypeId=?", u.getOrganizationId(), u.getCompanyId(), partyTypeId);
        return q("EXEC dbo.USP_GetVendorsAndCustomers @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId());
    }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(AccountTypeIds, AccountTypeIdsNot) - COAAllocation 'GetAccountTitleByAccountTypeIds'. */
    public List<Map<String, Object>> accountTitlesByTypes(UserAccount u, String accountTypeIds, String accountTypeIdsNot) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", n(u.getAppId()));
        if (accountTypeIds != null && !accountTypeIds.isEmpty()) p.put("AccountTypeIds", accountTypeIds);
        if (accountTypeIdsNot != null && !accountTypeIdsNot.isEmpty()) p.put("AccountTypeIdsNot", accountTypeIdsNot);
        if (u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return proc("Sp_COAAllocation_GetAllMethod", p);
    }

    /** SupplierCustomer.GetSupplierCustomerIdbyGlAccountId - Sp_SupplierCustomer_GetAllMethod 'GetIdByGlAccountId', first column. */
    public int supplierCustomerIdByGl(UserAccount u, int glAccountId) {
        var rows = q("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @GlAccountId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), glAccountId, "GetIdByGlAccountId");
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "Id"));
    }

    /** USP_InventoryValidation (DAL 0433 SetData :316) for one invoice line. */
    public void inventoryValidation(int org, int company, int documentTypeId, LocalDateTime docDate, int itemId, int warehouseId, int jobLotId,
                                    String cropYear, int packingTypeId, int packUomId, double netWeight, int refDocumentTypeId, int refDocNoId,
                                    int refDocSubIdNo, int itemConditionId) {
        run("EXEC dbo.USP_InventoryValidation @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocDate=?, @ItemId=?, @WarehouseId=?, @JobLotId=?, "
                        + "@CropYear=?, @InvPackingTypeId=?, @PackUomId=?, @NetWeight=?, @RefDocumentTypeId=?, @RefDocNoId=?, @RefDocSubIdNo=?, @ItemConditionId=?",
                org, company, documentTypeId, docDate == null ? null : java.sql.Timestamp.valueOf(docDate), itemId, warehouseId, jobLotId,
                cropYear, packingTypeId, packUomId, netWeight, refDocumentTypeId, refDocNoId, refDocSubIdNo, itemConditionId);
    }

    // ============================================================================== financial lookups (BLL/DAL CommonServices)

    /** CommonServies.GetItemListForFinancialEffects / DAL GetItemGlIdsandItemName - Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'. */
    public Map<Integer, Map<String, Object>> itemGl(int org, int company) {
        Map<Integer, Map<String, Object>> out = new LinkedHashMap<>();
        for (var r : q("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", org, company, "GetItemGlIdsandItemName"))
            out.putIfAbsent(i(col(r, "Id")), r);
        return out;
    }

    /** GetSupplierCustomerListForFinancialEffects - 'GetGlAccountIdandCompanyNameBySupplierCustomerId'. */
    public Map<Integer, Map<String, Object>> partyGl(int org, int company) {
        Map<Integer, Map<String, Object>> out = new LinkedHashMap<>();
        for (var r : q("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", org, company,
                "GetGlAccountIdandCompanyNameBySupplierCustomerId"))
            out.putIfAbsent(i(col(r, "Id")), r);
        return out;
    }

    /** DAL CommonServices.GetJobLotGlIdsandName - SP_JobLot_ReadMethod 'GetJobLotGlIdsandName' (Id -> AccountId). */
    public Map<Integer, Integer> jobLotAccounts(int org, int company) {
        Map<Integer, Integer> out = new LinkedHashMap<>();
        for (var r : q("EXEC [dbo].[SP_JobLot_ReadMethod] @OrganizationId=?, @CompanyId=?, @Activity=?", org, company, "GetJobLotGlIdsandName"))
            out.putIfAbsent(i(col(r, "Id")), i(col(r, "AccountId")));
        return out;
    }

    /** jobLot.GetByID - SP_JobLot_ReadMethod @Id, 'GetById', reading AccountId (0 when the lot is not found). */
    public int jobLotAccountById(int jobLotId) {
        var rows = q("EXEC dbo.SP_JobLot_ReadMethod @Id=?, @Activity=?", jobLotId, "GetById");
        return rows.isEmpty() ? 0 : i(col(rows.get(0), "AccountId"));
    }

    /** GetEqvilentByItemIdAndUomScheduleId - Sp_Item_GetAllMethod @ScheduleId, reading Equivalent. */
    public double equivalent(int org, int company, int itemId, int scheduleId) {
        var rows = q("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @ScheduleId=?, @Activity=?",
                org, company, itemId, scheduleId, "GetEqvilentByItemIdAndUomScheduleId");
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "Equivalent"));
    }

    /** CommonServies.GetAvgRateFromAvaiblableLoaderStock - [Sp_InvSaleInvoice_GetAllMethod]. */
    public Map<String, Object> avgRateFromLoaderStock(int org, int company, int refDocumentTypeId, int refDocIdNo, int refDocSubId) {
        var rows = q("EXEC [dbo].[Sp_InvSaleInvoice_GetAllMethod] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @RefDocIdNo=?, @RefDocSubIdNo=?, @Activity=?",
                org, company, refDocumentTypeId, refDocIdNo, refDocSubId, "GetAvgRateFromAvaiblableLoaderStock");
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** CommonServies.getAvgRateByJobOrder - usp_getAvgRateByJobOrder, @DocumentTypeId/@RecId only when non-zero. */
    public double avgRateByJobOrder(int org, int company, LocalDateTime docDate, int jobLotId, int itemId, int documentTypeId, int recId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", org, "CompanyId", company, "DocDate", docDate, "JobLotId", jobLotId, "ItemId", itemId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("RecId", recId);
        var rows = proc("usp_getAvgRateByJobOrder", p);
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "AvgRate"));
    }

    /** GetAvgRatesAndStockInHand.AvgRateOnlyForCGS - 'GetOnlyAvgRateForCGS' with its non-zero guards. */
    public double avgRateOnlyForCgs(int org, int company, int itemId, LocalDateTime docDate, int documentTypeId, int id,
                                    int jobLotId, String cropYear, int warehouseId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", org, "CompanyId", company, "ItemId", itemId, "DocDate", docDate);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("RecId", id);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        p.put("Activity", "GetOnlyAvgRateForCGS");
        var rows = proc("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "AvgRate"));
    }

    /** DAL CommonServices.GetInventoryStockEvalautionDetailByOtherIds. */
    public List<Map<String, Object>> stockByOtherIds(int org, int company, int otherDocumentTypeId, int otherDocNoId, int otherSubDocNoId) {
        return q("EXEC dbo.USP_GetInventoryStockEvalautionDetailByOtherIds @OrganizationId=?, @CompanyId=?, @OtherDocumentTypeId=?, @OtherDocNoId=?, @OtherSubDocNoId=?",
                org, company, otherDocumentTypeId, otherDocNoId, otherSubDocNoId);
    }

    // ============================================================================== helpers

    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }
    public static int n(Integer v) { return v == null ? 0 : v; }
    /**
     * Architecture.Common.Conversion.ToInt = Convert.ToInt32(value), 0 on any exception: a number rounds half
     * to even, a bool is 1/0, a string must be a whole number (Int32.Parse - "12.5" or "1,200" give 0).
     */
    public static int i(Object o) {
        if (o == null) return 0;
        if (o instanceof Integer || o instanceof Short || o instanceof Byte) return ((Number) o).intValue();
        if (o instanceof Long) { long l = (Long) o; return l > Integer.MAX_VALUE || l < Integer.MIN_VALUE ? 0 : (int) l; }
        if (o instanceof BigDecimal) {
            BigDecimal r = ((BigDecimal) o).setScale(0, java.math.RoundingMode.HALF_EVEN);
            try { return r.intValueExact(); } catch (ArithmeticException e) { return 0; }
        }
        if (o instanceof Number) {
            double v = ((Number) o).doubleValue();
            if (Double.isNaN(v) || Double.isInfinite(v)) return 0;
            double r = Math.rint(v);
            return r > Integer.MAX_VALUE || r < Integer.MIN_VALUE ? 0 : (int) r;
        }
        if (o instanceof Boolean) return (Boolean) o ? 1 : 0;
        String t = String.valueOf(o).trim();
        if (t.isEmpty()) return 0;
        try { return Integer.parseInt(t.startsWith("+") ? t.substring(1) : t); } catch (NumberFormatException e) { return 0; }
    }
    public static double d(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0d;
        String t = String.valueOf(o).trim();
        if (t.isEmpty()) return 0d;
        try { return Double.parseDouble(t.replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }
    public static BigDecimal dec(Object o) {
        if (o instanceof BigDecimal) return (BigDecimal) o;
        if (o instanceof Number) return BigDecimal.valueOf(((Number) o).doubleValue());
        if (o == null) return BigDecimal.ZERO;
        String t = String.valueOf(o).trim();
        if (t.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(t.replace(",", "")); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }
    public static boolean b(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        return o != null && truthy(String.valueOf(o));
    }
    public static String s(Object o) { return o == null ? "" : String.valueOf(o); }
    public static LocalDateTime dt(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().atStartOfDay();
        if (o instanceof LocalDateTime) return (LocalDateTime) o;
        if (o instanceof LocalDate) return ((LocalDate) o).atStartOfDay();
        if (o == null) return null;
        String t = String.valueOf(o).trim();
        if (t.isEmpty()) return null;
        t = t.replace('T', ' ');
        if (t.length() == 10) return LocalDate.parse(t).atStartOfDay();
        try { return Timestamp.valueOf(t.length() == 16 ? t + ":00" : t).toLocalDateTime(); }
        catch (IllegalArgumentException e) { return LocalDate.parse(t.substring(0, 10)).atStartOfDay(); }
    }
}
