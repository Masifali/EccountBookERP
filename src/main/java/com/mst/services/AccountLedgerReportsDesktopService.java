package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Account Reports (module 3), group R1 2026-09-30 - the two ledger screens that were still on raw SQL:
 *
 *  - 49 Party Ledger (CustomerLedger.cs): CustomerLedgerFill -> InventoryStockEvalautionDetail
 *    .GeneralLedgerSupplierCustomerQuantative(ReportsParameters) -> Sp_GeneralLedger_SupplierCustomerQuantative.
 *    BLL (0574_Architecture.BLL.Inventory.InventoryStockEvalautionDetail.cs:1107): @OrganizationId, @CompanyId always;
 *    @DateFrom / @DateTo unless CheckDateTimeNull; @SupplierCustomerId when != 0.
 *
 *  - General Ledger Statement (GeneralLedgerStatment.cs): btnshow_Click -> VoucherReports.GeneralLedgerStatement
 *    (0141_...VoucherReports.cs:1237) -> Sp_GeneralLedgerStatement_Rpt: @FinancialYearId (ActiveYr.Id), @OrganizationId,
 *    @CompanyId always; @VoucherDateF / @VoucherDateT unless CheckDateTimeNull; @AccountId always; @ProjectsId and
 *    @BranchesId when != 0; @ReportType = 'Account Statment By Date'.
 *    Its combos: CompanyServiceBind -> Company.GetAlldt -> Sp_Company_GetAllMethod (@OrgCompanyTypeId, @Activity=
 *    'ReadByOrganizationId'); BrancheServiceBind -> Branches.GetAll -> Sp_Branches_GetAllMethod (@OrganizationId,
 *    @CompanyId, @Activity='GetAll') - bound to BOTH cmbbranch and cmbproject; CoaAllocationGetForComboServiceBind ->
 *    COAAllocation.GetForComboBind -> Sp_COAAllocation_GetAllMethod (@OrganizationId, @CompanyId, @UserId when != 0,
 *    @Activity='COAForCombobindig').
 *
 * Tenancy always comes from CurrentUserContext; DesktopProc omits a null exactly as ADO.NET leaves an unset parameter
 * out. No raw SQL and no fallbacks: a failing procedure surfaces its own message, as the desktop MessageBox does.
 * Date values in the returned rows are written as yyyy-MM-dd strings so the page never depends on Jackson's date setting.
 */
@Service
public class AccountLedgerReportsDesktopService {

    @Autowired
    private JdbcTemplate jdbc;

    @Autowired
    private CurrentUserContext ctx;

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) {
        return normalize(DesktopProc.rows(jdbc, proc, p));
    }

    private static Map<String, Object> p(Object... kv) {
        return DesktopProc.params(kv);
    }

    private static Integer nz(Integer v) {
        return v == null || v == 0 ? null : v;
    }

    private static List<Map<String, Object>> normalize(List<Map<String, Object>> in) {
        List<Map<String, Object>> out = new ArrayList<>(in.size());
        SimpleDateFormat f = new SimpleDateFormat("yyyy-MM-dd");
        for (Map<String, Object> r : in) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof java.util.Date) v = f.format((java.util.Date) v);
                else if (v instanceof byte[]) continue;      // logos / images are not grid data
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> keep(List<Map<String, Object>> in, String... keys) {
        List<Map<String, Object>> out = new ArrayList<>(in.size());
        for (Map<String, Object> r : in) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : keys) if (r.containsKey(k)) m.put(k, r.get(k));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------ 49 Party Ledger

    public List<Map<String, Object>> partyLedger(Integer supplierCustomerId, String fromDate, String toDate) {
        if (supplierCustomerId == null || supplierCustomerId <= 0)
            throw new IllegalArgumentException("Party Name required. Please Check!");
        return rows("Sp_GeneralLedger_SupplierCustomerQuantative", p("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "DateFrom", AccountReportsDesktopService.date(fromDate),
                "DateTo", AccountReportsDesktopService.date(toDate), "SupplierCustomerId", nz(supplierCustomerId)));
    }

    // ------------------------------------------------------------------ General Ledger Statement

    public List<Map<String, Object>> companies() {
        return keep(rows("Sp_Company_GetAllMethod", p("OrgCompanyTypeId", ctx.currentOrganizationId(),
                "Activity", "ReadByOrganizationId")), "Id", "CompName");
    }

    public List<Map<String, Object>> branchesAll() {
        return keep(rows("Sp_Branches_GetAllMethod", p("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "Activity", "GetAll")), "Id", "BranchCode", "BranchName", "ProjectName");
    }

    public List<Map<String, Object>> coaForCombo() {
        return rows("Sp_COAAllocation_GetAllMethod", p("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "UserId", nz(ctx.currentUserId()), "Activity", "COAForCombobindig"));
    }

    public List<Map<String, Object>> generalLedgerStatement(Integer accountId, String fromDate, String toDate,
                                                            Integer branchId, Integer projectId) {
        if (accountId == null || accountId == 0)
            throw new IllegalArgumentException("Account Title Field Required............");
        return rows("Sp_GeneralLedgerStatement_Rpt", p("FinancialYearId", ctx.currentFinancialYearId(),
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "VoucherDateF", AccountReportsDesktopService.date(fromDate), "VoucherDateT", AccountReportsDesktopService.date(toDate),
                "AccountId", accountId, "ProjectsId", nz(projectId), "BranchesId", nz(branchId),
                "ReportType", "Account Statment By Date"));
    }
}
