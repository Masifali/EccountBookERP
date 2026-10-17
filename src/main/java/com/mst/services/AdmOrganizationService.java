package com.mst.services;

import com.mst.repositories.AdmOrganizationRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * BLL of Architecture.WinApp.Templates.DefineOrgnization (ScreenDefinition 402, opened from DefineCompany's
 * "Define Organization" button, BtnTemplateDefine_Click 1106). Desktop map (DefineOrgnization.cs):
 *   Load 151, TemplateNameFill 170, Reset 200, btnnew 228, btnRefresh 240, FormValidation 252, Insert 341,
 *   ReadById 397, btnupdate 434, btnsave 450, HistoryGridFill 476, GridSetting 521, grd_DoubleClick 542,
 *   grd_ColumnButtonClick 560, KeyDown 582, MakeShortCutKeys 658, txtCompanyCount_KeyPress 687.
 *
 * Same gate as the other Admin Panel pages: RoleName must be "Admin" (AdmPanelService.admin()).
 * Organisation / user come from the session; this form has no organisation field of its own (it defines the
 * organisation rows themselves), so nothing is taken from the session into the model.
 */
@Service
public class AdmOrganizationService {

    @Autowired private AdmOrganizationRepository repo;
    @Autowired private AdmPanelService gate;

    private static final String[] HISTORY = {
            "Id", "OrgCode", "OrgName", "OrgContactPerson", "OrgEmailA", "OrgEmailB", "OrgTel", "OrgMobileA", "OrgMobileB",
            "OrgMobileC", "OrgAddress", "OrgReportingTitle", "OrgCountry", "OrgState", "OrgBaseCurr", "AllowedLocationsCount",
            "ReportingRemarks" };

    /** FormValidation: field key in the request, message, in the desktop order. */
    private static final String[][] REQUIRED = {
            {"code", "Organization Code Field Required"},
            {"name", "Organization Name Field Required"},
            {"reportingTitle", "Reporting Title Field Required"},
            {"contactPerson", "Contact Person Field Required"},
            {"email", "Email Field Required"},
            {"email02", "Email II Field Required"},
            {"address", "Address Field Required"},
            {"phone", "Phone Number Field Required"},
            {"mobile01", "Mobile 01 Field Required"},
            {"mobile02", "Mobile 02 Field Required"},
            {"mobile03", "Mobile 03 Field Required"},
            {"country", "Country Field Required"},
            {"state", "State Field Required"},
            {"remarks", "Reporting Remarks Field Required"} };

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    private static int asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** TemplateNameFill: BindDDLNew(dt, CmbTemplate, "Id", "TemplateName", "Template Name", true); nothing bound when empty. */
    public List<Map<String, Object>> templates() {
        gate.admin();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> t : repo.templates()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", t.get("Id"));
            m.put("TemplateName", t.get("TemplateName"));
            out.add(m);
        }
        return out;
    }

    /** HistoryGridFill: the 17 columns, in the desktop order (OrgLogo is not shown). */
    public List<Map<String, Object>> history() {
        gate.admin();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.organizations()) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : HISTORY) m.put(k, r.get(k));
            out.add(m);
        }
        return out;
    }

    /** ReadById: Organization.GetByID throws (index out of range) when there is no row; the form shows ex.Message. */
    public Map<String, Object> read(int id) {
        gate.admin();
        Map<String, Object> r = repo.organization(id);
        if (r == null) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("id", asInt(r.get("Id")));
        m.put("code", str(r.get("OrgCode")));
        m.put("name", str(r.get("OrgName")));
        m.put("reportingTitle", str(r.get("OrgReportingTitle")));
        m.put("contactPerson", str(r.get("OrgContactPerson")));
        m.put("email", str(r.get("OrgEmailA")));
        m.put("email02", str(r.get("OrgEmailB")));
        m.put("address", str(r.get("OrgAddress")));
        m.put("remarks", str(r.get("ReportingRemarks")));
        m.put("phone", str(r.get("OrgTel")));
        m.put("mobile01", str(r.get("OrgMobileA")));
        m.put("mobile02", str(r.get("OrgMobileB")));
        m.put("mobile03", str(r.get("OrgMobileC")));
        m.put("country", str(r.get("OrgCountry")));
        m.put("state", str(r.get("OrgState")));
        m.put("companyCount", str(r.get("AllowedLocationsCount")));
        m.put("templateId", asInt(r.get("OrganizaionTemplateId")));
        return m;
    }

    /**
     * Insert (341): FormValidation, then Organization.Save. id == 0 is btnsave (RecId = 0 -> Sp_Organization_Insert),
     * id &gt; 0 is btnupdate (Sp_Organization_Update). The confirmation questions are asked by the page before the call.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        gate.admin();
        int id = asInt(body.get("id"));
        if (id < 0) id = 0;
        if (id > 0 && repo.organization(id) == null) throw new IllegalStateException("RecId Not Found...");
        for (String[] f : REQUIRED)
            if (str(body.get(f[0])).trim().isEmpty()) throw new IllegalArgumentException(f[1]);
        int templateId = asInt(body.get("templateId"));
        if (templateId != 0) {
            boolean found = false;
            for (Map<String, Object> t : repo.templates()) if (asInt(t.get("Id")) == templateId) { found = true; break; }
            if (!found) throw new IllegalArgumentException("Template Name not found.");
        }
        int count = asInt(body.get("companyCount"));
        Map<String, Object> model = params(                      // Architecture.Model.Organization declaration order
                "AllowedLocationsCount", count,
                "Id", id,
                "OrgAddress", str(body.get("address")),
                "OrgCode", str(body.get("code")),
                "OrgContactPerson", str(body.get("contactPerson")),
                "OrgCountry", str(body.get("country")),
                "OrgEmailA", str(body.get("email")),
                "OrgEmailB", str(body.get("email02")),
                "OrgMobileA", str(body.get("mobile01")),
                "OrgMobileB", str(body.get("mobile02")),
                "OrgMobileC", str(body.get("mobile03")),
                "OrgName", str(body.get("name")),
                "OrgReportingTitle", str(body.get("reportingTitle")),
                "OrgState", str(body.get("state")),
                "OrgTel", str(body.get("phone")),
                "OrganizaionTemplateId", templateId,
                "ReportingRemarks", str(body.get("remarks")));
        repo.save(model, id);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", id > 0 ? "Update Successfully" : "Save Successfully");
        return r;
    }
}
