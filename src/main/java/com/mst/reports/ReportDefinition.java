package com.mst.reports;

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * One desktop print button, described exactly as the desktop performs it.
 *
 * A definition is DATA, traced from the C#: the report template, the procedure that fills it,
 * which parameters that procedure always receives and which the BLL guards, and any sub-reports
 * with their own procedures.
 *
 * Nothing here is inferred from a filename. The number in a .rpt name is a convention, not a
 * contract — 203, 203A, 203_01 and 203_02 all run the SAME main procedure and differ only in
 * sub-reports and layout.
 */
public final class ReportDefinition {

    /** A parameter the desktop sends. GUARDED ones are omitted when unset, never sent as NULL. */
    public enum Mode { ALWAYS, GUARDED }

    public static final class Param {
        public final String name;      // @OrganizationId
        public final String source;    // session:organizationId | arg:orderId | const:41
        public final Mode mode;
        public Param(String name, String source, Mode mode) {
            this.name = name; this.source = source; this.mode = mode;
        }
    }

    public static final class SubReport {
        public final String template;
        public final String procedure;
        public final List<Param> params;
        public SubReport(String template, String procedure, List<Param> params) {
            this.template = template; this.procedure = procedure;
            this.params = Collections.unmodifiableList(params);
        }
    }

    public final String key;            // stable id used by the endpoint
    public final String template;       // the .rpt filename, exactly as the desktop names it
    public final String procedure;      // main data source
    public final String desktopCaller;  // where this was traced from
    public final List<Param> params;
    public final List<SubReport> subReports;
    /** Report-level parameters the desktop pushes (RptPerameter), not procedure parameters. */
    public final Map<String, String> reportParams;

    public ReportDefinition(String key, String template, String procedure, String desktopCaller,
                            List<Param> params, List<SubReport> subReports) {
        this.key = key; this.template = template; this.procedure = procedure;
        this.desktopCaller = desktopCaller;
        this.params = Collections.unmodifiableList(params);
        this.subReports = Collections.unmodifiableList(subReports);
        Map<String, String> rp = new LinkedHashMap<>();
        /* Every traced caller pushes these two. */
        rp.put("@CompanyName", "session:companyName");
        rp.put("@CompanyAddress", "session:companyAddress");
        this.reportParams = Collections.unmodifiableMap(rp);
    }
}
