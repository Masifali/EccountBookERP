package com.mst.reports;

import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

/**
 * Where the Crystal bridge and the templates live.
 *
 * Paths are configured by the application. Registered report names resolve only to
 * existing .rpt files inside templateRoot; no database index or schema change is required.
 *
 * application.properties:
 *   reports.crystal.enabled=true
 *   reports.crystal.bridge-exe=D:/CShapEccorErp/EccountingERP/migration/six-reports/CrystalReportBridge.exe
 *   reports.crystal.template-root=D:/CShapEccorErp/EccountingERP/GoldenAceRice
 *   reports.crystal.timeout-seconds=120
 */
@Component
@ConfigurationProperties(prefix = "reports.crystal")
public class CrystalPrintProperties {

    /** Off by default: the bridge only runs on Windows with the Crystal runtime installed. */
    private boolean enabled = false;
    private String bridgeExe = "";
    private String templateRoot = "";
    private int timeoutSeconds = 120;
    /** Scratch directory for the request/response files. Defaults to the JVM temp dir. */
    private String workDir = "";

    public boolean isEnabled() { return enabled; }
    public void setEnabled(boolean enabled) { this.enabled = enabled; }

    public String getBridgeExe() { return bridgeExe; }
    public void setBridgeExe(String bridgeExe) { this.bridgeExe = bridgeExe; }

    public String getTemplateRoot() { return templateRoot; }
    public void setTemplateRoot(String templateRoot) { this.templateRoot = templateRoot; }

    public int getTimeoutSeconds() { return timeoutSeconds; }
    public void setTimeoutSeconds(int timeoutSeconds) { this.timeoutSeconds = timeoutSeconds; }

    public String getWorkDir() { return workDir; }
    public void setWorkDir(String workDir) { this.workDir = workDir; }
}
