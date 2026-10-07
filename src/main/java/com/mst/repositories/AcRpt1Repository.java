package com.mst.repositories;

import com.mst.models.UserAccount;
import java.math.BigDecimal;
import java.sql.Date;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.ColumnMapRowMapper;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/**
 * Group R1 (Account Reports): the four payables / receivables report screens, each on the desktop's own
 * procedure and parameter set (VoucherReports.cs).
 *   48 Payables              Sp_Accounts_Payables_Rpt             (Payables.btnshow_Click)
 *   63 PayablesByDueDate     Sp_DueByDatePayablesAndReceivables   (PayablesByDueDate.btnshow_Click)
 *   70 ReceiveablesByDueDate USP_DueByDateReceivables             (ReceiveablesByDueDate.btnshow_Click)
 *   68 ReceivablesByDueDatesNew Usp_ReceivablesByDueDates         (ReceivablesByDueDatesNew.FillGridData)
 * Every guarded parameter (the desktop's "if (obj.X != 0)") is OMITTED when unset, never sent as NULL.
 * The result is the FIRST result set, as SqlDataAdapter.Fill(DataTable) takes it.
 */
@Repository
public class AcRpt1Repository {
    private final JdbcTemplate jdbc;

    public AcRpt1Repository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ shared lookups
    /** CommonServices.CustomeGroupsDefine(1) = AcLookUps.GetAll(TypeId 1): Sp_AcLookUps_GetAllMethod ReadAll. */
    private List<Map<String, Object>> customGroups(UserAccount u) {
        return norm(jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'",
                u.getOrganizationId(), u.getCompanyId()));
    }

    /** ChartofAccount.ReadAllAccountgroup: Sp_ChartofAccount_GetAllMethodFromCOA ReadAllAccountGroup (Account_Level 3, AccountTypeId 3). */
    private List<Map<String, Object>> accountGroups(UserAccount u, int yearId, String classIds, int classId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Account_Level=3, @AccountTypeId=3");
        List<Object> a = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId(), yearId));
        if (classId != 0) { sql.append(", @AccountClassId=?"); a.add(classId); }
        if (classIds != null && !classIds.trim().isEmpty()) { sql.append(", @AccountClassIds=?"); a.add(classIds); }
        sql.append(", @CoaType='ReadAllAccountGroup'");
        return norm(jdbc.queryForList(sql.toString(), a.toArray()));
    }

    /** CustomerGroup.GetAll: Sp_CustomerGroup_GetAllMethod ReadAll, minus Ids 7, 9, 10, 12, 13, 14, 15 (CustomerGroupFill). */
    private List<Map<String, Object>> customerGroupsFiltered(UserAccount u) {
        List<Map<String, Object>> all = norm(jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",
                u.getOrganizationId(), u.getCompanyId()));
        Set<Integer> skip = new HashSet<>(Arrays.asList(7, 9, 10, 12, 13, 14, 15));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) {
            Object id = ci(r, "Id");
            int n = 0;
            try { n = id == null ? 0 : (int) Math.round(Double.parseDouble(String.valueOf(id))); } catch (Exception ignored) { }
            if (!skip.contains(n)) out.add(r);
        }
        return out;
    }

    /** CustomerGroup.GetSupplierCustomerGroupFromInventoryStockEvaluation. */
    private List<Map<String, Object>> inventoryGroups(UserAccount u) {
        return norm(jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetSupplierCustomerGroupFromInventoryStockEvaluation'",
                u.getOrganizationId(), u.getCompanyId()));
    }

    /** CommonServices.CityGetAllService: City.GetAll = SP_City_GetAllMethod 'GetAll'. */
    private List<Map<String, Object>> cities(UserAccount u) {
        return norm(jdbc.queryForList("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType='GetAll'", u.getOrganizationId(), u.getCompanyId()));
    }

    /** CommonServices.MultiLanguagesGetAll: MultiLanguages.GetAll = Sp_MultiLanguages_GetAll 'ReadAll'. */
    private List<Map<String, Object>> languages(UserAccount u) {
        return norm(jdbc.queryForList("EXEC dbo.Sp_MultiLanguages_GetAll @MethodType='ReadAll', @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId()));
    }

    /** VoucherHead.GetBranchesFromVouchersByAccountId(OrganizationId, CompanyId, "", 0). */
    private List<Map<String, Object>> branches(UserAccount u) {
        return norm(jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId()));
    }

    /** clsGlobalVariables.ErpFeaturesList = USP_GetERPFeaturesByCompanyId; GetERPFeatureById(id) = the feature is in the list. */
    private Set<Integer> features(UserAccount u) {
        Set<Integer> out = new HashSet<>();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            Object id = ci(r, "Id");
            if (id instanceof Number) out.add(((Number) id).intValue());
        }
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the session's financial year, as a local yyyy-MM-dd (null when unknown). */
    private String yearStart(UserAccount u, int yearId) {
        List<Map<String, Object>> years = jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
        Map<String, Object> pick = null;
        for (Map<String, Object> y : years) {
            Object id = ci(y, "Id");
            if (id instanceof Number && ((Number) id).intValue() == yearId) { pick = y; break; }
        }
        if (pick == null && !years.isEmpty() && yearId <= 0) pick = years.get(0);
        if (pick == null) return null;
        Object start = ci(pick, "Start_Period");
        if (start instanceof Timestamp) return ((Timestamp) start).toLocalDateTime().toLocalDate().toString();
        if (start instanceof java.util.Date) return new Date(((java.util.Date) start).getTime()).toLocalDate().toString();
        return start == null ? null : String.valueOf(start).substring(0, Math.min(10, String.valueOf(start).length()));
    }

    private int amountDecimals(UserAccount u) {
        return ReportValueSupport.amountDecimals(jdbc, u);
    }

    // ------------------------------------------------------------------ 48 Payables
    /** Payables_Load: BranchFeature(17) / consolidated(18), BranchesFill, CustomeGroupsDefine, AccountFill3rdLevel, CityNameFill, CustomerGroupBind. */
    public Map<String, Object> payablesLookups(UserAccount u, int yearId) {
        Map<String, Object> d = new LinkedHashMap<>();
        Set<Integer> f = features(u);
        boolean branchFeature = f.contains(17), consolidated = f.contains(18);
        d.put("amountDecimals", amountDecimals(u));
        d.put("branchFeature", branchFeature);
        d.put("branchConsolidated", consolidated);
        d.put("branches", branchFeature ? branches(u) : new ArrayList<>());
        d.put("userBranchId", u.getBranchesId());
        d.put("customGroups", customGroups(u));
        d.put("accounts", accountGroups(u, yearId, "2,3", 0));
        d.put("cities", cities(u));
        d.put("inventoryGroups", inventoryGroups(u));
        d.put("yearStart", yearStart(u, yearId));
        return d;
    }

    public List<Map<String, Object>> payables(UserAccount u, int yearId, LocalDate from, LocalDate to, int actionId, int cityId, String branchesIds,
            String controlAccountIds, int customGroupId, String customerGroupIds, double balanceFrom, double balanceTo, int showAssetLiability) {
        Sql s = new Sql("Sp_Accounts_Payables_Rpt");
        s.p("FinancialYearId", yearId); s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("UserId", u.getId());
        if (from != null) s.p("FromDate", Date.valueOf(from));
        if (to != null) s.p("ToDate", Date.valueOf(to));
        s.opt("ShowOnlyTrade", actionId);
        s.opt("CityId", cityId);
        s.optS("BranchesIds", branchesIds);
        s.optS("ControlAccountIds", controlAccountIds);
        s.opt("CustomGroupId", customGroupId);
        s.optS("CustomerGroupIds", customerGroupIds);
        if (balanceFrom != 0.0) s.p("BalanceFrom", balanceFrom);
        if (balanceTo != 0.0) s.p("BalanceTo", balanceTo);
        s.opt("ShowAssetLiability", showAssetLiability);
        return norm(first(s));
    }

    // ------------------------------------------------------------------ 63 PayablesByDueDate
    /** PayablesByDueDate_Load: Datetypefill (client), CustomerGroupFill, LanguageDropdownBind. */
    public Map<String, Object> payablesDueLookups(UserAccount u, int yearId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals(u));
        d.put("customerGroups", customerGroupsFiltered(u));
        d.put("languages", languages(u));
        d.put("yearStart", yearStart(u, yearId));
        return d;
    }

    /** btnRefresh_Click: CustomerGroupFill + LanguageDropdownBind. */
    public Map<String, Object> payablesDueRefresh(UserAccount u) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("customerGroups", customerGroupsFiltered(u));
        d.put("languages", languages(u));
        return d;
    }

    public List<Map<String, Object>> payablesDue(UserAccount u, LocalDate dueTo, String ids, int fromDocNo, int toDocNo, int languageId) {
        Sql s = new Sql("Sp_DueByDatePayablesAndReceivables");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("UserId", u.getId());
        s.optS("CustomerGroupIds", ids);
        if (dueTo != null) s.p("DueDateTo", Date.valueOf(dueTo));
        s.opt("BalanceFrom", fromDocNo);
        s.opt("BalanceTo", toDocNo);
        s.opt("LanguageId", languageId);
        return norm(first(s));
    }

    // ------------------------------------------------------------------ 70 ReceiveablesByDueDate
    /** ReceiveablesByDueDate_Load: CustomerGroupFill, LanguageDropdownBind, AccountFill3rdLevel (AccouuntClassId 2), CustomeGroupsDefine. */
    public Map<String, Object> receivablesDueLookups(UserAccount u, int yearId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals(u));
        d.put("customerGroups", customerGroupsFiltered(u));
        d.put("languages", languages(u));
        d.put("accounts", accountGroups(u, yearId, null, 2));
        d.put("customGroups", customGroups(u));
        d.put("yearStart", yearStart(u, yearId));
        return d;
    }

    public List<Map<String, Object>> receivablesDue(UserAccount u, LocalDate dueTo, int fromDocNo, int toDocNo, int languageId, int customGroupId,
            String ids, String parentAccountCode) {
        Sql s = new Sql("USP_DueByDateReceivables");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("UserId", u.getId());
        if (dueTo != null) s.p("DueDateTo", Date.valueOf(dueTo));
        s.opt("BalanceFrom", fromDocNo);
        s.opt("BalanceTo", toDocNo);
        s.opt("LanguageId", languageId);
        s.optS("CustomerGroupIds", ids);
        s.optS("ParentAccountCode", parentAccountCode);
        s.opt("CustomGroupId", customGroupId);
        return norm(first(s));
    }

    // ------------------------------------------------------------------ 68 ReceivablesByDueDatesNew
    /** AccountFill3rdLevel (ReadAll3rdLevelAccountsForPayablesandReceeivablesAging, TypeNo 2) + CustomeGroupsDefine(1). */
    public Map<String, Object> receivablesNewLookups(UserAccount u) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals(u));
        List<Map<String, Object>> all = norm(jdbc.queryForList("EXEC dbo.Sp_AccountsOpeningBalances_GetMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll3rdLevelAccountsForPayablesandReceeivablesAging'",
                u.getOrganizationId(), u.getCompanyId()));
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : all) {
            Object t = ci(r, "TypeNo");
            if (t instanceof Number && ((Number) t).intValue() == 2) accounts.add(r);
        }
        d.put("accounts", accounts);
        d.put("customGroups", customGroups(u));
        return d;
    }

    public List<Map<String, Object>> receivablesNew(UserAccount u, LocalDate from, LocalDate to, int customGroupId, int parentId) {
        Sql s = new Sql("Usp_ReceivablesByDueDates");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId());
        if (from != null) s.p("FromDate", Date.valueOf(from));
        s.p("ToDate", Date.valueOf(to));
        s.p("AccouuntClassId", 2);
        s.p("UserId", u.getId());
        s.p("ActionId", 1);
        s.opt("ParentAccountId", parentId);
        s.opt("CustomGroupId", customGroupId);
        return norm(first(s));
    }

    // ------------------------------------------------------------------ plumbing
    /** EXEC dbo.Proc @a=?, @b=? builder; the order of the calls is the order sent. */
    private static final class Sql {
        final StringBuilder text;
        final List<Object> args = new ArrayList<>();
        boolean any;
        Sql(String proc) { text = new StringBuilder("EXEC dbo.").append(proc).append(' '); }
        void p(String name, Object v) { text.append(any ? ", " : "").append('@').append(name).append("=?"); any = true; args.add(v); }
        void opt(String name, int v) { if (v != 0) p(name, v); }
        void optS(String name, String v) { if (v != null && !v.isEmpty()) p(name, v); }
    }

    /** The first result set of the procedure (SqlDataAdapter.Fill), skipping update counts. */
    private List<Map<String, Object>> first(Sql s) {
        return jdbc.execute(s.text.toString(), (PreparedStatementCallback<List<Map<String, Object>>>) st -> {
            for (int i = 0; i < s.args.size(); i++) st.setObject(i + 1, s.args.get(i));
            boolean result = st.execute();
            while (true) {
                if (result) {
                    try (ResultSet rs = st.getResultSet()) {
                        List<Map<String, Object>> rows = new ArrayList<>();
                        ColumnMapRowMapper mapper = new ColumnMapRowMapper();
                        while (rs.next()) rows.add(mapper.mapRow(rs, rows.size()));
                        return rows;
                    }
                } else if (st.getUpdateCount() == -1) return new ArrayList<>();
                result = st.getMoreResults();
            }
        });
    }

    /** Numbers as doubles (Conversion.ToDouble), dates as local ISO text (a Timestamp would serialize as a UTC instant and can shift a day). */
    private static List<Map<String, Object>> norm(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                else if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof Date) v = ((Date) v).toLocalDate().toString();
                else if (v instanceof java.util.Date) v = new Date(((java.util.Date) v).getTime()).toLocalDate().toString();
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private static Object ci(Map<String, Object> row, String key) {
        if (row == null) return null;
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
}
