package com.mst.security;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;
import java.io.Serializable;

/**
 * What LoginNew.cs writes into {@code clsGlobalVariables} once the operator has chosen - company,
 * branch, financial year and application - held for this browser session instead.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THE SESSION AND NOT THE USER ROW
 * ---------------------------------------------------------------------------------------------
 * dbo.UserAccount carries one CompanyId and one BranchesId, but a user can be allocated to
 * several of each - that is the whole reason the desktop asks. Writing the choice back to the row
 * would change it for every other session that user has open, and for the desktop client too.
 * So the choice lives here, for one browser session, and the row is left alone.
 *
 * ---------------------------------------------------------------------------------------------
 * THE USER NAME IS PART OF THE KEY, DELIBERATELY
 * ---------------------------------------------------------------------------------------------
 * A session that is re-authenticated as somebody else must not inherit the previous operator's
 * company. {@link #isFor} is checked on every read, so a context whose owner does not match the
 * signed-in principal is ignored rather than trusted.
 */
public class LoginContext implements Serializable {

    private static final long serialVersionUID = 1L;

    /** The session attribute. One per browser session. */
    public static final String ATTRIBUTE = "com.mst.security.LoginContext";

    private final String userName;
    private int companyId;
    private String companyName;
    private boolean headOffice;
    private int branchId;
    private String branchName;
    private int financialYearId;
    private String financialYearCode;
    private int appId;
    private String appName;
    /** False until every step the desktop asks for has been answered. */
    private boolean complete;

    public LoginContext(String userName) { this.userName = userName; }

    public boolean isFor(String name) {
        return userName != null && userName.equalsIgnoreCase(name);
    }

    /** The context for this request, or null - never another user's. */
    public static LoginContext of(HttpServletRequest request, String userName) {
        if (request == null) return null;
        HttpSession session = request.getSession(false);
        if (session == null) return null;
        Object value = session.getAttribute(ATTRIBUTE);
        if (!(value instanceof LoginContext)) return null;
        LoginContext context = (LoginContext) value;
        return context.isFor(userName) ? context : null;
    }

    public static void store(HttpServletRequest request, LoginContext context) {
        request.getSession(true).setAttribute(ATTRIBUTE, context);
    }

    public String getUserName()          { return userName; }
    public int getCompanyId()            { return companyId; }
    public String getCompanyName()       { return companyName; }
    public boolean isHeadOffice()        { return headOffice; }
    public int getBranchId()             { return branchId; }
    public String getBranchName()        { return branchName; }
    public int getFinancialYearId()      { return financialYearId; }
    public String getFinancialYearCode() { return financialYearCode; }
    public int getAppId()                { return appId; }
    public String getAppName()           { return appName; }
    public boolean isComplete()          { return complete; }

    public void setCompany(int id, String name, boolean headOffice) {
        this.companyId = id; this.companyName = name; this.headOffice = headOffice;
    }
    public void setBranch(int id, String name)         { this.branchId = id; this.branchName = name; }
    public void setFinancialYear(int id, String code)  { this.financialYearId = id; this.financialYearCode = code; }
    public void setApplication(int id, String name)    { this.appId = id; this.appName = name; }
    public void setComplete(boolean complete)          { this.complete = complete; }
}
