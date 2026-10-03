package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Admin Panel -> "Audit Log Report" - desktop Architecture.WinApp.Audit_Dashboard.AuditLogReport
 * (DashboardNew.cs:2666 btnAuditLogReport_Click -> new AuditLogReport(UserAccount), shown inside panel2).
 *
 * Every call is the form's own BLL call with the BLL's own parameter list:
 *
 *   AllDropDownBind  UserAccount.ReadUserByCompanyId(CompanyId, 0)
 *                      -> Sp_UserAccount_GetAllMethod @CompanyId, @Activity='ReadAllUser'
 *                         (@ActionId only when != 0 - the form passes 0, so it is not sent)
 *                      DropDownBind.BindDDLNew copies ONLY Id + UserName into the combo, so only
 *                      those two columns leave the server (the proc also returns Password).
 *   Load             tblUserRights.GetScreensForAuditReport() -> [dbo].[USP_GetScreensForAuditReport] (no params)
 *   gridHisory       Dashboard.AuditLogReport(ReportsParameters)
 *                      -> USP_UserAudit_Report @OrganizationId, @CompanyId,
 *                         @FromDate / @ToDate (unless CheckDateTimeNull), @UserId (only when != 0)
 *   Slip column      ReportsMethod.FormHistory() -> [dbo].[USP_ReportsMethod_GetAllMethod] @Activity='FormHistory'
 *
 * Tenancy (organization, company) always comes from CurrentUserContext, never from the request.
 * The form itself checks no screen right; the only gate on the desktop is the Admin Panel menu,
 * which DashboardNew_Load shows only when RoleName == "Admin" (GearMenuController) - the same gate
 * is applied to every call here.
 */
@Service
public class AuditLogReportService {

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private AccountReportsDesktopService accountReports;
    @Autowired private DashboardModuleService dashboardModules;

    /** Screen names the form's View handler resolves by hand when USP_GetScreensForAuditReport gives no TargetUrl
        (AuditLogReport.cs DataGridHistory_ColumnButtonClick) - the class each one constructs. */
    private static final String[][] HAND_TARGETS = {
            {"DeliveryOrder", "Architecture.WinApp.Sale.DeliveryOrder"},
            {"ExportDeliveryOrderB", "Architecture.WinApp.Export.ExportDeliveryOrderB"},
            {"ExportDeliveryOrderNew", "Architecture.WinApp.Export.ExportDeliveryOrderNew"},
            {"frmPurchaseInvoiceDirectStoreWithVariant", "Architecture.WinApp.StoreManagement.frmPurchaseInvoiceDirectStoreWithVariant"},
            {"frmPurchaseInvoiceDirectStore", "Architecture.WinApp.StoreManagement.frmPurchaseInvoiceDirectStore"},
            {"FoodProductionWithValues", "Architecture.WinApp.Production.FoodProductionWithValues"}
    };

    /** DashboardNew_Load: btnadminpanel.Visible only when UserAccount.RoleName == "Admin". */
    public void admin() {
        String role;
        try { role = ctx.currentRoleName(); } catch (RuntimeException e) { role = null; }
        if (!"Admin".equals(role)) {
            throw new AccessDeniedException("The desktop shows Admin Panel (and so the Audit Log Report) only when RoleName is \"Admin\".");
        }
    }

    /** frmGRNHistory_Load: ParameterFill + AllDropDownBind + GetScreensForAuditReport (+ ActiveYr for Date Type 5). */
    public Map<String, Object> load() {
        admin();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("dateTypes", dateTypes());
        out.put("users", users());
        out.put("screens", screens());
        out.put("handTargets", handTargets());
        out.put("activeYear", accountReports.activeYear());
        return out;
    }

    /** btnRefresh_Click -> AllDropDownBind() only. */
    public Map<String, Object> refresh() {
        admin();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("users", users());
        return out;
    }

    /** CommonServices.DateType() - a fixed DataTable built in code on the desktop (CommonServices.cs:15850). */
    private static List<Map<String, Object>> dateTypes() {
        String[] names = {"This Day", "This Week", "This Month", "This Year", "Financial Year"};
        List<Map<String, Object>> list = new ArrayList<>();
        for (int i = 0; i < names.length; i++) {
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("Id", i + 1);
            r.put("Parameters", names[i]);
            list.add(r);
        }
        return list;
    }

    /** AllDropDownBind: UserAccount.ReadUserByCompanyId(UserAccount.CompanyId, 0); BindDDLNew(dt, "Id", "UserName"). */
    private List<Map<String, Object>> users() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_UserAccount_GetAllMethod",
                DesktopProc.params("CompanyId", ctx.currentCompanyId(), "Activity", "ReadAllUser"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("ID"));
            m.put("UserName", r.get("UserName"));
            out.add(m);
        }
        return out;
    }

    /** dtScreens = tblUserRights.GetScreensForAuditReport(), plus the web page each TargetUrl is ported to. */
    private List<Map<String, Object>> screens() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "[dbo].[USP_GetScreensForAuditReport]", new LinkedHashMap<>());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            Integer screenId = asInt(r.get("ScreenID"));
            String screenName = str(r.get("ScreenName"));
            String targetUrl = str(r.get("TargetUrl"));
            m.put("ScreenID", screenId);
            m.put("ScreenName", screenName);
            m.put("ScreenAlias", str(r.get("ScreenAlias")));
            m.put("TargetUrl", targetUrl);
            m.put("webRoute", dashboardModules.webRouteFor(screenId, screenName, targetUrl));
            out.add(m);
        }
        return out;
    }

    /** The hand-coded TargetUrls of the View handler, with the web page each class is ported to. */
    private List<Map<String, Object>> handTargets() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] t : HAND_TARGETS) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ScreenName", t[0]);
            m.put("TargetUrl", t[1]);
            m.put("webRoute", dashboardModules.webRouteFor(null, t[0], t[1]));
            out.add(m);
        }
        return out;
    }

    /**
     * btnshow_Click -> gridHisory(): Dashboard.AuditLogReport(obj) with obj.FromDate/ToDate = the two
     * DateTimePickers and obj.UserId = cmbusername.Value. The rows come back whole (dt is what the
     * "Audit Log Report" toolbar print hands to AuditActiviyReport.rpt); the page builds dtGrn from them.
     */
    public Map<String, Object> show(String fromDate, String toDate, Integer userId) {
        admin();
        java.sql.Date from = AccountReportsDesktopService.date(fromDate);
        java.sql.Date to = AccountReportsDesktopService.date(toDate);
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FromDate", from,
                "ToDate", to,
                "UserId", userId == null || userId == 0 ? null : userId);
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_UserAudit_Report", p);
        SimpleDateFormat dt = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;                 // CompLogoImage: CAST('' AS varbinary(MAX))
                if (v instanceof java.util.Date) v = dt.format((java.util.Date) v);
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("rows", out);
        return res;
    }

    /** DataGridHistory_ColumnButtonClick "Slip": ReportsMethod.FormHistory() (read at every click, as the desktop does). */
    public Map<String, Object> reportMethods() {
        admin();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "[dbo].[USP_ReportsMethod_GetAllMethod]",
                DesktopProc.params("Activity", "FormHistory"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ScreenName", r.get("ScreenName"));
            m.put("RefDocumentTypeId", r.get("RefDocumentTypeId"));
            m.put("ReportMethod", r.get("ReportMethod"));
            out.add(m);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("rows", out);
        return res;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static Integer asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return null;
        try { return Integer.valueOf(String.valueOf(o).trim()); } catch (NumberFormatException e) { return null; }
    }
}
