package com.mst.reports;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Helpers for the HRM print contracts. An HRM group registers its traced Crystal prints from its own
 * component so ReportRegistry itself is not edited per screen:
 *
 *   @Component
 *   public class HrmPayrollReports {
 *       public HrmPayrollReports(ReportRegistry registry) {
 *           registry.register(HrmReportSupport.def("hrm-1100", "1100-EmployeeSalarySheet.rpt", "Sp_...",
 *               "PayRollSalarySheetRpt.btnPrint_Click",
 *               HrmReportSupport.ps(HrmReportSupport.P("@OrganizationId", "session:organizationId"),
 *                                    HrmReportSupport.G("@DepartmentId", "arg:departmentId"))));
 *       }
 *   }
 *
 * and the page prints with CrystalPrint.open('hrm-1100', { departmentId: ... }) (countx_crystal_print.js).
 * Sources: session:organizationId|companyId|userId|branchId, arg:<name> (from the page), const:<value>.
 * GUARDED parameters are omitted when blank, as the desktop BLL's "if (x != 0)" omits them.
 */
public final class HrmReportSupport {
    private HrmReportSupport() { }

    public static ReportDefinition.Param P(String n, String src) { return new ReportDefinition.Param(n, src, ReportDefinition.Mode.ALWAYS); }
    public static ReportDefinition.Param G(String n, String src) { return new ReportDefinition.Param(n, src, ReportDefinition.Mode.GUARDED); }
    public static List<ReportDefinition.Param> ps(ReportDefinition.Param... a) { return Arrays.asList(a); }
    public static ReportDefinition.SubReport sub(String template, String procedure, List<ReportDefinition.Param> params) {
        return new ReportDefinition.SubReport(template, procedure, params);
    }

    public static ReportDefinition def(String key, String rpt, String proc, String caller, List<ReportDefinition.Param> params) {
        return new ReportDefinition(key, rpt, proc, caller, params, Collections.emptyList());
    }

    public static ReportDefinition def(String key, String rpt, String proc, String caller, List<ReportDefinition.Param> params,
                                       List<ReportDefinition.SubReport> subs) {
        return new ReportDefinition(key, rpt, proc, caller, params, subs);
    }
}
