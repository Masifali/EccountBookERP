package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Wages Dashboard Report.
 *
 * Ported from Architecture.WinApp.Inventory_Reports\frmWagesDashboardReport.cs (2,101 lines).
 * Its Load handler is still called frmEvaulationDetailSalesReports_Load - the form was copied
 * from that one and never renamed.
 *
 * Report contract, read from
 * BLL.ContractorWages.InvContractorWagesBillHeader.ContractorWagesDashboardsandReport
 * (0485_Architecture.BLL.ContractorWages.InvContractorWagesBillHeader.cs):
 *
 *   USP_ContractorWagesDashboardsandReport
 *       @OrganizationId, @CompanyId     always
 *       @DateFrom  / @DateTo            only when the date is set
 *       @ContratorId                    only when non-zero   (the source's own spelling)
 *       @WagesAccountId                 only when non-zero   (from cmbWagesAccount)
 *       @RefDocumentTypeId              only when non-zero
 *
 * FIVE RESULT SETS (:368-508), read with a ConnectionCallback because queryForList returns only
 * the first:
 *
 *   Tables[0]  one set split three ways by ActivityDescription:
 *                "Summary By Document Type"   -> Summary By Document Type grid
 *                "Summary By Contractor"      -> Summary By Contractor grid
 *                "Summary By Wages Account"   -> Summary By Wages Account grid
 *   Tables[1]  Activity and Document Type detail
 *   Tables[2]  Document Type and Activity detail
 *   Tables[3]  Detail Summary By Contractor
 *   Tables[4]  Detail By Contractor
 *
 * SOURCE QUIRK, reproduced (:381): the "Summary By Contractor" branch fills the grid's
 * ContractorNameId and ContractorName columns from the procedure's RefDocumentTypeId and
 * RefDocumentType. The column is named after the contractor but carries the document type. It is
 * kept because both apps must show the same thing for the same input, and the ported screen
 * labels it the way the desktop grid labels it.
 *
 * SECOND QUIRK (:542-553): GridSummaryByContractorSetting passes NO hidden-column list, so that
 * one grid shows ActivityDiscription and ContractorNameId while the other two hide their
 * equivalents. Reproduced.
 *
 * The field names ActivityDiscription, "Documnet Type && Activity" and "Labour / WAges Activity"
 * are the source's own spellings and are left exactly as they are.
 *
 * Read-only: no save, update or delete, and no History button on the desktop form. btnPrint's
 * handler is an empty stub there (:909-912).
 */
@Service
public class WagesDashboardReportService {

    private static final Logger LOG = LoggerFactory.getLogger(WagesDashboardReportService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ setup

    /** frmEvaulationDetailSalesReports_Load, :191-271 - the three dropdowns and the dates. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();

        /* :263 - From is a week back; To keeps the designer's today. */
        LocalDate today = LocalDate.now();
        out.put("fromDate", today.minusDays(7).toString());
        out.put("toDate", today.toString());

        out.put("decimalsAmount", decimalPoints("Default NoofDecimal Points For Amount"));
        out.put("decimalsRate",   decimalPoints("Default NoofDecimal Points For Rate"));

        out.put("documentTypes", readOrError(out, "documentTypeError",
                () -> lookup("GetDocumentTypesFromContractorWages", "Id", "DocumentTypeDescription")));
        out.put("wagesAccounts", readOrError(out, "wagesAccountError",
                () -> lookup("GetWagesAccounts", "AccountId", "WagesAccountName")));
        out.put("contractors", readOrError(out, "contractorError",
                () -> lookup("GetWagesContractors", "ContractorId", "ContractorName")));
        return out;
    }

    private interface Reader { List<Map<String, Object>> read(); }

    private List<Map<String, Object>> readOrError(Map<String, Object> out, String key, Reader r) {
        try {
            return r.read();
        } catch (Exception e) {
            LOG.error("Dropdown read failed for {}", key, e);
            out.put(key, e.getMessage());
            return new ArrayList<>();
        }
    }

    /**
     * ReferenceDocumentType :273-295, WagesAccountFill :297-319, WagesContractorFill :321-343.
     * All three go through the same procedure with a different @Activity:
     *   Sp_InvContractorWagesBillHeader_GetAllMethod @OrganizationId, @CompanyId, @Activity
     */
    private List<Map<String, Object>> lookup(String activity, String idColumn, String nameColumn) {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC Sp_InvContractorWagesBillHeader_GetAllMethod @OrganizationId=?, "
                        + "@CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                activity);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, idColumn)));
            m.put("name", str(col(r, nameColumn)));
            out.add(m);
        }
        return out;
    }

    // ----------------------------------------------------------------- report

    /** AllGridFill(), :345-523 - one call, five result sets, seven grids. */
    public Map<String, Object> report(String fromDate, String toDate,
                                      int contractorId, int wagesAccountId, int refDocumentTypeId) {
        Map<String, Object> out = new LinkedHashMap<>();

        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        Object from = date(fromDate);
        if (from != null) { names.add("@DateFrom"); args.add(from); }
        Object to = date(toDate);
        if (to != null)   { names.add("@DateTo");   args.add(to); }
        /* @ContratorId is misspelled at source; sent exactly as the procedure declares it. */
        if (contractorId != 0)      { names.add("@ContratorId");       args.add(contractorId); }
        if (wagesAccountId != 0)    { names.add("@WagesAccountId");    args.add(wagesAccountId); }
        if (refDocumentTypeId != 0) { names.add("@RefDocumentTypeId"); args.add(refDocumentTypeId); }

        List<List<Map<String, Object>>> sets;
        try {
            sets = callMultiSet(exec("USP_ContractorWagesDashboardsandReport", names), args);
        } catch (Exception e) {
            LOG.error("USP_ContractorWagesDashboardsandReport failed", e);
            out.put("error", e.getMessage());
            emptyGrids(out);
            return out;
        }

        List<Map<String, Object>> t0 = set(sets, 0);
        List<Map<String, Object>> byDocType = new ArrayList<>();
        List<Map<String, Object>> byContractor = new ArrayList<>();
        List<Map<String, Object>> byWagesAc = new ArrayList<>();

        for (Map<String, Object> r : t0) {
            String activity = str(col(r, "ActivityDescription"));
            if ("Summary By Document Type".equals(activity)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ActivityDiscription", activity);          // the grid's own spelling
                m.put("RefDocumentTypeId", col(r, "RefDocumentTypeId"));
                m.put("RefDocumentType",   col(r, "RefDocumentType"));
                m.put("Weight", col(r, "WagesWeight"));
                m.put("Qty",    col(r, "WagesQty"));
                m.put("Amount", col(r, "WagesAmount"));
                m.put("AvgRate/40Kg", col(r, "AvgRatePer40Kg"));
                byDocType.add(m);
            }
            if ("Summary By Contractor".equals(activity)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ActivityDiscription", activity);
                /* :381 - the source fills the contractor columns from the DOCUMENT TYPE fields.
                   Kept exactly. */
                m.put("ContractorNameId", col(r, "RefDocumentTypeId"));
                m.put("ContractorName",   col(r, "RefDocumentType"));
                m.put("Weight", col(r, "WagesWeight"));
                m.put("Qty",    col(r, "WagesQty"));
                m.put("Amount", col(r, "WagesAmount"));
                byContractor.add(m);
            }
            if ("Summary By Wages Account".equals(activity)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ActivityDiscription", activity);
                /* :385 - and here the wages-account columns are filled from the document type
                   fields too. Same source pattern, same treatment. */
                m.put("WagesAccountId", col(r, "RefDocumentTypeId"));
                m.put("WagesAccount",   col(r, "RefDocumentType"));
                m.put("Weight", col(r, "WagesWeight"));
                m.put("Qty",    col(r, "WagesQty"));
                m.put("Amount", col(r, "WagesAmount"));
                m.put("AvgRate/40Kg", col(r, "AvgRatePer40Kg"));
                byWagesAc.add(m);
            }
        }
        out.put("summaryByDocType",    byDocType);
        out.put("summaryByContractor", byContractor);
        out.put("summaryByWagesAc",    byWagesAc);

        /* Tables[1] and Tables[2] carry identical columns; only the grouping differs (:433, :459). */
        out.put("activityDocumentType", detailWithRate(set(sets, 1)));
        out.put("documentTypeActivity", detailWithRate(set(sets, 2)));

        /* Tables[3] - Detail Summary By Contractor (:485-491). */
        List<Map<String, Object>> t3 = new ArrayList<>();
        for (Map<String, Object> r : set(sets, 3)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RefDocumentTypeId", col(r, "RefDocumentTypeId"));
            m.put("RefDocumentType",   col(r, "RefDocumentType"));
            m.put("WagesAccountId",    col(r, "InvConractorWagesAccountsId"));  // source spelling
            m.put("WagesAccount",      col(r, "WagesAccountName"));
            m.put("ContractorNameId",  col(r, "ContractorId"));
            m.put("ContractorName",    col(r, "ContractorName"));
            m.put("Weight", col(r, "WagesWeight"));
            m.put("Qty",    col(r, "WagesQty"));
            m.put("Amount", col(r, "WagesAmount"));
            t3.add(m);
        }
        out.put("detailSummaryByContractor", t3);

        /* Tables[4] - Detail By Contractor, which adds PackSize and WageRate (:508-514). */
        List<Map<String, Object>> t4 = new ArrayList<>();
        for (Map<String, Object> r : set(sets, 4)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RefDocumentTypeId", col(r, "RefDocumentTypeId"));
            m.put("RefDocumentType",   col(r, "RefDocumentType"));
            m.put("WagesAccountId",    col(r, "InvConractorWagesAccountsId"));
            m.put("WagesAccount",      col(r, "WagesAccountName"));
            m.put("ContractorNameId",  col(r, "ContractorId"));
            m.put("ContractorName",    col(r, "ContractorName"));
            m.put("PackSize",  col(r, "PackSize"));
            m.put("WageRate",  col(r, "WageRate"));
            m.put("Weight", col(r, "WagesWeight"));
            m.put("Qty",    col(r, "WagesQty"));
            m.put("Amount", col(r, "WagesAmount"));
            t4.add(m);
        }
        out.put("detailByContractor", t4);
        return out;
    }

    private List<Map<String, Object>> detailWithRate(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RefDocumentTypeId", col(r, "RefDocumentTypeId"));
            m.put("RefDocumentType",   col(r, "RefDocumentType"));
            m.put("WagesAccountId",    col(r, "InvConractorWagesAccountsId"));
            m.put("WagesAccount",      col(r, "WagesAccountName"));
            m.put("RateEffectedFrom",  col(r, "RateEffectedFrom"));
            m.put("PackSize",          col(r, "PackSize"));
            m.put("WageRate",          col(r, "WageRate"));
            m.put("Weight", col(r, "WagesWeight"));
            m.put("Qty",    col(r, "WagesQty"));
            m.put("Amount", col(r, "WagesAmount"));
            m.put("AvgRate/40Kg", col(r, "AvgRatePer40Kg"));
            out.add(m);
        }
        return out;
    }

    private static void emptyGrids(Map<String, Object> out) {
        for (String k : new String[] { "summaryByDocType", "summaryByContractor", "summaryByWagesAc",
                                       "activityDocumentType", "documentTypeActivity",
                                       "detailSummaryByContractor", "detailByContractor" }) {
            out.put(k, new ArrayList<Map<String, Object>>());
        }
    }

    private static List<Map<String, Object>> set(List<List<Map<String, Object>>> sets, int i) {
        return (sets != null && sets.size() > i) ? sets.get(i) : new ArrayList<>();
    }

    // ---------------------------------------------------------------- helpers

    private List<List<Map<String, Object>>> callMultiSet(final String sql, final List<Object> args) {
        return jdbcTemplate.execute((ConnectionCallback<List<List<Map<String, Object>>>>) (Connection con) -> {
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            try (PreparedStatement ps = con.prepareStatement(sql)) {
                for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
                boolean hasResults = ps.execute();
                while (true) {
                    if (hasResults) {
                        try (ResultSet rs = ps.getResultSet()) {
                            sets.add(readRows(rs));
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    hasResults = ps.getMoreResults();
                    if (!hasResults && ps.getUpdateCount() == -1) break;
                }
            }
            return sets;
        });
    }

    private static List<Map<String, Object>> readRows(ResultSet rs) throws java.sql.SQLException {
        List<Map<String, Object>> rows = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 1; i <= n; i++) {
                String label = md.getColumnLabel(i);
                if (label == null || label.isEmpty()) label = md.getColumnName(i);
                m.put(label, rs.getObject(i));
            }
            rows.add(m);
        }
        return rows;
    }

    private int decimalPoints(String configDescription) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                            + "@ConfigDescription=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    configDescription,
                    "GetConfigurationByOrgCompandConfigDescription");
            if (!rows.isEmpty()) {
                int n = asInt(col(rows.get(0), "ConfigKey"));
                if (n >= 1 && n <= 4) return n;
            }
        } catch (Exception e) {
            LOG.error("Decimal-points configuration read failed for {}", configDescription, e);
        }
        return 2;   // clsGlobalVariables falls back to 2 when the configuration is missing
    }

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    private static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    /** Plain yyyy-MM-dd only - nothing shifts a day at UTC+5. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
