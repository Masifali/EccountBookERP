package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Contractor Wages - "Wages Report (With Activities)".
 * Ported from Architecture.WinApp.Pcc.Reports\WagesReportWithActivities.cs (116 KB) and
 * Architecture.BLL.pcc.ContractorWagesActivity.
 *
 * This replaces a generic page that posted to /accounts/api/reports/wages-report - the PLAIN wages
 * report's endpoint - and therefore could never return this report's data. The real source is:
 *
 *   Report      [pcc].[USP_WagesReportWithActivities]         (BLL WagesActivity(), :147-258)
 *   Item /      [pcc].[USP_GetDataForDropDownFromWagesSchedule]  one call, rows split on the
 *   Activity                                                   Activity column into "GroupName"
 *                                                              (Item Name) and "Service Activity"
 *                                                              (form :299-330)
 *   Plant       Sp_InvProductionPlant_GetAllMethod @Activity='GetALL'   (form :371-378)
 *   Contractor  Sp_SupplierCustomer_GetAllMethod
 *               @Activity='ReadByOrganizationCompanyIdForContractorWages'  (form :410-417)
 *
 * The twelve report types are NOT twelve different queries - the desktop sends the chosen type as
 * @ActivityName and the procedure shapes its own result set (form :510-511, BLL :250-254). So the
 * rows are returned as-is and the page renders whatever columns come back, rather than a
 * hand-written column list per type that could drift from the procedure.
 */
@Service
public class ContractorWagesActivityReportService {

    private static final Logger LOG = LoggerFactory.getLogger(ContractorWagesActivityReportService.class);

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** ReportTypeFill(), form :338-353 - a fixed list built in code, in this order. */
    private static final List<String> REPORT_TYPES = Collections.unmodifiableList(Arrays.asList(
            "Wages Detail",
            "Wages Detail Without Item",
            "Summary By Contractor",
            "Summary By Item",
            "Summary By Item and Contractor",
            "Summary By Item and Service Activity",
            "Summary By Contractor and Service Activity",
            "Summary By Item Contractor and Service Activity",
            "Summary By Wages Group",
            "Summary By Wages Group and Service Activity",
            "Summary By Wages Group and Contractor",
            "Summary By Wages Group Contractor and Service Activity"));

    /** CommonServices.DateType(), CommonServices.cs :15850-15863 - also a fixed in-code list. */
    private static final List<String> DATE_TYPES = Collections.unmodifiableList(Arrays.asList(
            "This Day", "This Week", "This Month", "This Year", "Financial Year"));

    private static final String SQL_DROPDOWNS =
            "EXEC [pcc].[USP_GetDataForDropDownFromWagesSchedule] @OrganizationId=?, @CompanyId=?, @Activity=?";

    private static final String SQL_PLANTS =
            "EXEC Sp_InvProductionPlant_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    private static final String SQL_CONTRACTORS =
            "EXEC Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";


    // ---------------------------------------------------------------- lookups

    /**
     * Everything the filter bar needs, in one call.
     * Report types and date types are the desktop's own in-code lists, so they are constants here
     * rather than a fabricated lookup table. Date Type defaults to index 2 - "This Month" - because
     * DateTypeFill() activates Rows[2] (form :432-433).
     */
    public Map<String, Object> getFilterLookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        List<Map<String, Object>> items = new ArrayList<>();
        List<Map<String, Object>> activities = new ArrayList<>();
        try {
            /* The form passes a ReportsParameters carrying only Organization/Company, so
               @Activity goes in as NULL (form :296-299). */
            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_DROPDOWNS, orgId, compId, null)) {
                String activity = str(col(r, "Activity"));
                Map<String, Object> row = new LinkedHashMap<>();
                row.put("Id", col(r, "Id"));
                row.put("Name", col(r, "ReferenceName"));
                if ("GroupName".equals(activity)) items.add(row);
                else if ("Service Activity".equals(activity)) activities.add(row);
            }
        } catch (Exception e) {
            LOG.error("Wages activity report dropdowns failed", e);
        }

        List<Map<String, Object>> plants = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_PLANTS, orgId, compId, "GetALL")) {
                Map<String, Object> row = new LinkedHashMap<>();
                row.put("Id", col(r, "Id"));
                row.put("Name", col(r, "Description"));
                plants.add(row);
            }
        } catch (Exception e) {
            LOG.error("Plant list failed", e);
        }

        List<Map<String, Object>> contractors = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_CONTRACTORS, orgId, compId,
                    "ReadByOrganizationCompanyIdForContractorWages")) {
                Map<String, Object> row = new LinkedHashMap<>();
                row.put("Id", col(r, "Id"));
                row.put("Name", col(r, "CompanyName"));
                contractors.add(row);
            }
        } catch (Exception e) {
            LOG.error("Contractor list failed", e);
        }

        out.put("items", items);
        out.put("serviceActivities", activities);
        out.put("plants", plants);
        out.put("contractors", contractors);
        out.put("reportTypes", REPORT_TYPES);
        out.put("dateTypes", DATE_TYPES);
        out.put("defaultDateTypeIndex", 2);          // DateTypeFill() Rows[2].Activate()
        return out;
    }

    // ---------------------------------------------------------------- report

    /**
     * GridBind(), form :490-520. actionId is 2 for the Sales radio and 1 for Production (:513-519);
     * it is only sent when one of the two is chosen, matching the BLL's "omit when zero" rule.
     */
    public Map<String, Object> runReport(String reportType, String fromDate, String toDate,
                                         Integer fromDocNo, Integer toDocNo, Integer plantId,
                                         Integer itemId, Integer actionId, Integer contractorId,
                                         Integer serviceActivityId) {
        Map<String, Object> out = new LinkedHashMap<>();
        if (reportType == null || reportType.trim().isEmpty()) {
            out.put("success", false);
            out.put("message", "Report Type is required.");
            out.put("columns", Collections.emptyList());
            out.put("rows", Collections.emptyList());
            return out;
        }
        try {
            /* The BLL (ContractorWagesActivity.WagesActivity, :147-258) OMITS every optional
             * parameter whose value is null or zero - it never sends a NULL. Sending them anyway
             * is what produced "Operand type clash: date is incompatible with int" against the real
             * procedure, because a typed NULL bound to the wrong slot. The list is therefore built
             * the same way here: a parameter appears only when the desktop would have added it,
             * and in the desktop's order.
             */
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();

            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());

            Object from = date(fromDate);
            if (from != null) { names.add("@FromDate"); args.add(from); }
            Object to = date(toDate);
            if (to != null)   { names.add("@ToDate");   args.add(to); }

            if (nz(fromDocNo))          { names.add("@FromDocNo");                 args.add(fromDocNo); }
            if (nz(toDocNo))            { names.add("@ToDocNo");                   args.add(toDocNo); }
            if (nz(plantId))            { names.add("@PlantId");                   args.add(plantId); }
            if (nz(itemId))             { names.add("@ItemId");                    args.add(itemId); }
            if (nz(actionId))           { names.add("@ActionId");                  args.add(actionId); }
            if (nz(contractorId))       { names.add("@ContractorId");              args.add(contractorId); }
            if (nz(serviceActivityId))  { names.add("@ContractorWagesActivityId"); args.add(serviceActivityId); }

            names.add("@ActivityName"); args.add(reportType.trim());

            StringBuilder sql = new StringBuilder("EXEC [pcc].[USP_WagesReportWithActivities] ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            List<Map<String, Object>> rows = jdbcTemplate.queryForList(sql.toString(), args.toArray());

            /* Column order is whatever the procedure returned, taken from the first row. */
            List<String> columns = rows.isEmpty()
                    ? Collections.emptyList()
                    : new ArrayList<>(rows.get(0).keySet());

            out.put("success", true);
            out.put("columns", columns);
            out.put("rows", rows);
        } catch (Exception e) {
            LOG.error("Wages report with activities failed for type '{}'", reportType, e);
            out.put("success", false);
            out.put("message", e.getMessage());
            out.put("columns", Collections.emptyList());
            out.put("rows", Collections.emptyList());
        }
        return out;
    }

    private static boolean nz(Integer v) { return v != null && v != 0; }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
