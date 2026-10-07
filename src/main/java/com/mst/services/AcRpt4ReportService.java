package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import java.math.BigDecimal;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Service;

/**
 * R4 reports (new, additive - nothing existing is touched). Tenancy always comes from CurrentUserContext; every stored
 * procedure and parameter set is the desktop form's own call. Result sets keep the procedure's COLUMN ORDER (a
 * GridEX.RetrieveStructure() grid shows the columns in the order the table has them).
 *
 *   56  AuditDashboard            Dashboard.ReadUnBalancedVoucher            Sp_Accounts_AuditDashboard_Rpt (@ActivityId = 2)
 *   76  PdcInventoryReport        GeneralReprots.PdcReceiptsRegister         Sp_PdcInventory_SlipAndRegister_Rpt, Sp_COAAllocation_GetAllMethod 'COAForCombobindig'
 *   87  frmPostDatedChequeReports VoucherReports.PostDatedChecqueReport      USP_PostDatedChecque_Report, Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds' (15 / 3,6,8)
 *  932  frmAuditByWeightReport    ExImInvoice.ExportSalesAuditByWeight       SpEximInvoice_DoWeightWbWeightDiff_Rpt, USP_GetDataForDropDownFromDeliveryOrder
 *  933  frmGrnAudit_History       InvGrn.GetGrnDataForAudit                  usp_getDataForGrnAudit, USP_GetDataForDropDownFromGrn
 *  (934 LabDataVehicleWiseByParent is already ported by the Lab purchase report page - registered only.)
 *
 * The desktop forms check no screen right of their own (the menu hides what a user may not open); the web pages are reachable by
 * URL, so each endpoint here asks for the screen's View right (Admin always passes).
 */
@Service
public class AcRpt4ReportService {
    private static final Logger LOG = LoggerFactory.getLogger(AcRpt4ReportService.class);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public AcRpt4ReportService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
    }

    // ===================================================================================== shared helpers

    private UserAccount user(String screenName) {
        UserAccount u = ctx.requireAccountingUser();
        if (!"Admin".equals(ctx.currentRoleName()) && !rights.has(screenName, "view")) {
            throw new SecurityException("You do not have the View right for this screen.");
        }
        return u;
    }

    private static LocalDate date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        return LocalDate.parse(s.trim().substring(0, 10));
    }

    private static java.sql.Date sqlDate(LocalDate d) { return d == null ? null : java.sql.Date.valueOf(d); }

    private static Object cell(Object v) {
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Timestamp) {
            java.time.LocalDateTime l = ((Timestamp) v).toLocalDateTime();
            return l.toLocalTime().toSecondOfDay() == 0 && l.getNano() == 0 ? l.toLocalDate().toString() : l.toString().replace('T', ' ');
        }
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof byte[]) return null;                       // CompLogoImage etc. - binary is not sent to the browser
        return v;
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Math.round(Double.parseDouble(o.toString().trim())); } catch (NumberFormatException e) { return 0; }
    }

    /** One result set: the column names in the procedure's order and the rows. */
    private static final class RS {
        final List<String> cols = new ArrayList<>();
        final List<String> types = new ArrayList<>();      // int | num | date | bool | text (from the JDBC column type)
        final List<Map<String, Object>> rows = new ArrayList<>();
    }

    /** EXEC dbo.<proc> with the non-null parameters (ADO.NET does not send a null), every result set, column order kept. */
    private List<RS> exec(String proc, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sql.append(first ? " @" : ", @").append(e.getKey().trim()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.execute(sql.toString(), (PreparedStatementCallback<List<RS>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<RS> all = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet r = ps.getResultSet()) {
                        ResultSetMetaData md = r.getMetaData();
                        int n = md.getColumnCount();
                        RS set = new RS();
                        for (int c = 1; c <= n; c++) {
                            String label = md.getColumnLabel(c);
                            if (label == null || label.isEmpty()) label = "Column" + c;
                            set.cols.add(label);
                            set.types.add(kind(md.getColumnType(c)));
                        }
                        while (r.next()) {
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) {
                                String label = set.cols.get(c - 1);
                                if (!row.containsKey(label)) row.put(label, cell(r.getObject(c)));
                            }
                            set.rows.add(row);
                        }
                        all.add(set);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return all;
        });
    }

    private static String kind(int jdbcType) {
        switch (jdbcType) {
            case java.sql.Types.TINYINT: case java.sql.Types.SMALLINT: case java.sql.Types.INTEGER: case java.sql.Types.BIGINT: return "int";
            case java.sql.Types.DECIMAL: case java.sql.Types.NUMERIC: case java.sql.Types.FLOAT: case java.sql.Types.DOUBLE:
            case java.sql.Types.REAL: return "num";
            case java.sql.Types.DATE: case java.sql.Types.TIMESTAMP: case java.sql.Types.TIMESTAMP_WITH_TIMEZONE: return "date";
            case java.sql.Types.BIT: case java.sql.Types.BOOLEAN: return "bool";
            default: return "text";
        }
    }

    private static Map<String, Object> table(RS rs) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("columns", rs == null ? new ArrayList<String>() : rs.cols);
        o.put("types", rs == null ? new ArrayList<String>() : rs.types);
        o.put("rows", rs == null ? new ArrayList<Object>() : rs.rows);
        return o;
    }

    private static RS first(List<RS> sets) { return sets == null || sets.isEmpty() ? null : sets.get(0); }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription(description) -> ConfigKey, or null. */
    private String config(UserAccount u, String description) {
        try {
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod",
                    DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                            "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
            if (rows.isEmpty() || rows.get(0).get("ConfigKey") == null) return null;
            return String.valueOf(rows.get(0).get("ConfigKey")).trim();
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", description, e);
            return null;
        }
    }

    /** GetDecimalConfiguration(): 1..4 -> that many zeros in the format, anything else -> none. */
    private int decimalPlaces(UserAccount u, String description) {
        int p = toInt(config(u, description));
        return p >= 1 && p <= 4 ? p : 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period = the session's financial year. */
    private String yearStart() {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList("SELECT Start_Period FROM FinancialYear WHERE Id = ?", ctx.currentFinancialYearId());
            if (!rows.isEmpty()) {
                Object v = cell(rows.get(0).get("Start_Period"));
                return v == null ? null : String.valueOf(v).substring(0, 10);
            }
        } catch (Exception e) {
            LOG.warn("Financial year start could not be read", e);
        }
        return null;
    }

    // ===================================================================================== 56 AuditDashboard

    public Map<String, Object> auditInit() {
        user("AuditDashboard");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("yearStart", yearStart());
        return out;
    }

    /**
     * Dashboard.ReadUnBalancedVoucher: OrganizationId, CompanyId, FinancialYearId (ActiveYr.Id), ActivityId = 2, FromDate, ToDate
     * (the procedure ignores the dates and the year for activity 2).
     */
    public Map<String, Object> auditData(String from, String to) {
        UserAccount u = user("AuditDashboard");
        return table(first(exec("Sp_Accounts_AuditDashboard_Rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "ActivityId", 2,
                "FromDate", sqlDate(date(from)), "ToDate", sqlDate(date(to))))));
    }

    // ===================================================================================== 76 PdcInventoryReport

    /** AccountTitleFill: Sp_COAAllocation_GetAllMethod, OrganizationId, CompanyId, UserId (when not 0), @Activity = 'COAForCombobindig'. */
    public Map<String, Object> pdcInit() {
        UserAccount u = user("PdcInventoryReport");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("accounts", table(first(exec("Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "UserId", u.getId() == null || u.getId() == 0 ? null : u.getId(), "Activity", "COAForCombobindig")))));
        return out;
    }

    /**
     * GeneralReprots.PdcReceiptsRegister: OrganizationId, CompanyId, GLCrAccountId (when not 0), DateFrom / DateTo / DateFromCheq /
     * DateToCheq (only when the matching check box is ticked - the page sends the date only then), CheqStatus = the status combo's Text
     * trimmed (sent unless it is "All"; an empty text is sent as an empty string).
     */
    public Map<String, Object> pdcData(int accountId, String dateFrom, String dateTo, String chqFrom, String chqTo, String status) {
        UserAccount u = user("PdcInventoryReport");
        String st = status == null ? "" : status.trim();
        return table(first(exec("Sp_PdcInventory_SlipAndRegister_Rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "GLCrAccountId", accountId == 0 ? null : accountId,
                "DateFrom", sqlDate(date(dateFrom)), "DateTo", sqlDate(date(dateTo)),
                "DateFromCheq", sqlDate(date(chqFrom)), "DateToCheq", sqlDate(date(chqTo)),
                "CheqStatus", "All".equals(st) ? null : st))));
    }

    // ===================================================================================== 87 frmPostDatedChequeReports

    /**
     * GlAccountFill / ChequeAccountFill: CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15") and ("3,6,8") =
     * Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountTypeIds' with OrganizationId, CompanyId, AppId
     * (UserAccount.AppId as is), UserId (when not 0), AccountTypeIds.
     */
    public Map<String, Object> pchqInit() {
        UserAccount u = user("frmPostDatedChequeReports");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bankAccounts", accountTitles(u, "15"));
        out.put("chequeAccounts", accountTitles(u, "3,6,8"));
        out.put("amountDecimals", decimalPlaces(u, "Default NoofDecimal Points For Amount"));
        return out;
    }

    private List<Map<String, Object>> accountTitles(UserAccount u, String typeIds) {
        RS rs = first(exec("Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", u.getAppId() == null ? 0 : u.getAppId(),
                "AccountTypeIds", typeIds,
                "UserId", u.getId() == null || u.getId() == 0 ? null : u.getId(),
                "Activity", "GetAccountTitleByAccountTypeIds")));
        return rs == null ? new ArrayList<>() : rs.rows;
    }

    /**
     * VoucherReports.PostDatedChecqueReport (USP_PostDatedChecque_Report): OrganizationId, CompanyId, AccountId (when not 0),
     * CheqDateFrom (always - the From picker has no check box), CheqDateTo (only when its box is ticked), AccountIdBank (when not 0),
     * CheqNoFrom / CheqNoTo (when not 0, as a double like ReportsParameters.FromDocNo), CheqStatus = IsApproved (a bool, always sent:
     * Unclear = true, Clear = false; the Cancel button is hidden so the desktop default false stays).
     */
    public Map<String, Object> pchqData(String from, String to, int bankId, int accountId, int chqNoFrom, int chqNoTo, boolean approved) {
        UserAccount u = user("frmPostDatedChequeReports");
        return table(first(exec("USP_PostDatedChecque_Report", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AccountId", accountId == 0 ? null : accountId,
                "CheqDateFrom", sqlDate(date(from)), "CheqDateTo", sqlDate(date(to)),
                "AccountIdBank", bankId == 0 ? null : bankId,
                "CheqNoFrom", chqNoFrom == 0 ? null : Double.valueOf(chqNoFrom),
                "CheqNoTo", chqNoTo == 0 ? null : Double.valueOf(chqNoTo),
                "CheqStatus", approved))));
    }

    // ===================================================================================== 932 frmAuditByWeightReport

    /** ComboFill: InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder = USP_GetDataForDropDownFromDeliveryOrder (OrganizationId, CompanyId). */
    public Map<String, Object> weightInit() {
        UserAccount u = user("frmAuditByWeightReport");
        Map<String, Object> out = new LinkedHashMap<>();
        RS rs = first(exec("USP_GetDataForDropDownFromDeliveryOrder", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())));
        out.put("items", rs == null ? new ArrayList<Object>() : rs.rows);
        out.put("yearStart", yearStart());
        return out;
    }

    /**
     * ExImInvoice.ExportSalesAuditByWeight: OrganizationId, CompanyId, InvoiceId / ContractId (when not 0), GpDateFrom, gpDateTo,
     * InvoiceStatus ("In Process" / "Completed", null for All). With only an invoice id (the InvoiceNo link) no dates are sent.
     */
    public Map<String, Object> weightData(int invoiceId, int contractId, String from, String to, String status) {
        UserAccount u = user("frmAuditByWeightReport");
        String st = status == null ? "" : status.trim();
        return table(first(exec("SpEximInvoice_DoWeightWbWeightDiff_Rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InvoiceId", invoiceId == 0 ? null : invoiceId, "ContractId", contractId == 0 ? null : contractId,
                "GpDateFrom", sqlDate(date(from)), "gpDateTo", sqlDate(date(to)),
                "InvoiceStatus", st.isEmpty() ? null : st))));
    }

    // ===================================================================================== 933 frmGrnAudit_History

    /**
     * AllDropDownBind: InvGrn.GetDataForDropDownFromGrn = [dbo].[USP_GetDataForDropDownFromGrn] with OrganizationId, CompanyId,
     * FinancialYearId, DocumentTypeIds = "46", BranchesIds = UserAccount.BranchesId (as text). Rows carry Activity / Id / ReferenceName.
     */
    public Map<String, Object> grnInit() {
        UserAccount u = user("frmGrnAudit_History");
        Map<String, Object> out = new LinkedHashMap<>();
        RS rs = first(exec("[dbo].[USP_GetDataForDropDownFromGrn]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId() == 0 ? null : ctx.currentFinancialYearId(), "DocumentTypeIds", "46",
                "BranchesIds", u.getBranchesId() == null ? "0" : String.valueOf(u.getBranchesId()))));
        out.put("items", rs == null ? new ArrayList<Object>() : rs.rows);
        out.put("amountDecimals", decimalPlaces(u, "Default NoofDecimal Points For Amount"));
        out.put("yearStart", yearStart());
        return out;
    }

    /**
     * InvGrn.GetGrnDataForAudit (usp_getDataForGrnAudit): OrganizationId, CompanyId, FinancialYearId, BranchesId, FromDate, ToDate;
     * ParentCategoryId / PurchaseTypeId / PurchaseOrderId / SupplierCustomerId / CityId when not 0; FreightDiscountAmount = 1.0 when
     * "Show Only Freight-Discounted Rows" is ticked; ActionId = 1 when "Freight Audit by City" is ticked.
     */
    public Map<String, Object> grnData(String from, String to, int parentCategoryId, int purchaseTypeId, int orderId, int supplierId,
                                       int cityId, boolean onlyDiscounted, boolean freightByCity) {
        UserAccount u = user("frmGrnAudit_History");
        return table(first(exec("usp_getDataForGrnAudit", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId(),
                "FromDate", sqlDate(date(from)), "ToDate", sqlDate(date(to)),
                "ParentCategoryId", parentCategoryId == 0 ? null : parentCategoryId,
                "PurchaseTypeId", purchaseTypeId == 0 ? null : purchaseTypeId,
                "PurchaseOrderId", orderId == 0 ? null : orderId,
                "SupplierCustomerId", supplierId == 0 ? null : supplierId,
                "CityId", cityId == 0 ? null : cityId,
                "FreightDiscountAmount", onlyDiscounted ? Double.valueOf(1.0) : null,
                "ActionId", freightByCity ? Integer.valueOf(1) : null))));
    }

    /**
     * The "FactoryWeight" link: CommonServices.WbTransactionSlipByGpId(GpId, 51) = WbTransactionsReports.WbTransactionSlip280
     * (Sp_WbTransactionsSlip_rpt: OrganizationId, CompanyId, ReferenceDocTypeId = 51, ReferenceDocNoId = GpId); the rows go to
     * 280-InvRptWeighBridgeSlip.rpt (CompanyName / CompanyAddress).
     */
    public Map<String, Object> grnWbSlip(int gpId) {
        UserAccount u = user("frmGrnAudit_History");
        if (gpId == 0) throw new IllegalArgumentException("No Record Found For Display");
        return table(first(exec("Sp_WbTransactionsSlip_rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ReferenceDocTypeId", 51, "ReferenceDocNoId", gpId))));
    }
}
