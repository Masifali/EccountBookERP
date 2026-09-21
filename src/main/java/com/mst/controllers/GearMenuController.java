package com.mst.controllers;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The gear menu at the bottom-left of the desktop shell - "Admin Panel" and "System Utilities".
 *
 * ---------------------------------------------------------------------------------------------
 * THIS IS NOT THE RIGHTS TREE, AND THAT IS WHY THE EARLIER ROUTE FOUND NOTHING
 * ---------------------------------------------------------------------------------------------
 * Everything else in the port comes from App -> AppModules -> ScreenDefinition, resolved through
 * the user's rights rows. These two do not. They are {@code contextMenuStrip3} on DashboardNew,
 * a ContextMenuStrip written into the form's own InitializeComponent:
 *
 *   DashboardNew.cs:5568
 *     this.contextMenuStrip3.Items.AddRange(new ToolStripItem[2] { this.btnadminpanel, this.btnUtility });
 *     this.btnadminpanel.Text = "Admin Panel";      // :5578
 *     this.btnUtility.Text    = "System Utilities"; // :5641
 *
 * So the item list is fixed in code, the captions are literals, and each item opens a form
 * directly with {@code new Form(UserAccount)} - there is no TargetUrl and no ScreenDefinition row
 * behind any of them. "System Utilities" does not appear in the App table because it was never an
 * application; my earlier {@code /system-utilities} route looked for one and could not find it.
 *
 * ---------------------------------------------------------------------------------------------
 * VISIBILITY - COPIED, NOT INVENTED
 * ---------------------------------------------------------------------------------------------
 * Admin Panel is hidden at design time and shown in exactly one place:
 *
 *   DashboardNew_Load, DashboardNew.cs:1191
 *     btnadminpanel.Visible = false;
 *     if (UserAccount.RoleName == "Admin") { btnadminpanel.Visible = true; ... }
 *
 * That is the whole gate - the RoleName string, not a screen right, because these items have no
 * screen rows to carry rights. {@link CurrentUserContext#currentRoleName()} holds the same value
 * from Sp_UserAccount_Login, so the same comparison is made here.
 *
 * System Utilities carries no Visible=false of its own, so it shows for everyone, exactly as on
 * the desktop.
 *
 * Items marked {@code Visible = false} in the designer and never turned back on are absent below,
 * because they are absent on the desktop screen the user is looking at. Both lists declare
 * thirteen items; Admin Panel shows eleven (Screen Defination :5595 and Stock_Reconcilation :5604
 * are hidden) and System Utilities shows two (the other eleven are hidden at :5654, :5662, :5676,
 * :5692, :5696, :5700, :5704, :5708, :5712, :5716, :5720, and nothing in the file sets any of
 * them true - InitializeMenu at :2325-2332 only re-asserts false).
 *
 * ---------------------------------------------------------------------------------------------
 * AN ITEM WITHOUT A WEB SCREEN SAYS SO
 * ---------------------------------------------------------------------------------------------
 * Most of these forms are not ported yet. The menu still lists them, in the desktop's order and
 * wording, marked as not built and not clickable - rather than quietly dropping them, which would
 * make the web menu look complete when it is not.
 */
@Controller
public class GearMenuController {

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * btnadminpanel - DashboardNew.cs:5571-5575 order, hidden items removed.
     *
     * Columns: caption exactly as the desktop draws it, the form its Click handler constructs,
     * the web route if one exists, and - where there is none - why.
     */
    private static List<Map<String, Object>> adminPanelItems() {
        List<Map<String, Object>> items = new ArrayList<>();
        /* btnadminpanelsysconfig_Click -> Architecture.WinApp.Configurations.Configuration */
        items.add(item("System Configuration", "Configurations.Configuration",
                "/configurations", null));
        /* btnadminpaneluserright_Click -> frmUserRights. Despite the caption this form is the
           user MASTER (tabs "User Define" and "User History", frmUserRights.cs:1521/1602) - it
           defines users, it does not allocate rights. Not ported. */
        items.add(item("User Rights", "frmUserRights", null,
                "User master (User Define / User History) - not ported yet."));
        /* btnScreenRights_Click -> UserRightsByCompany, which IS the ported rights screen: its
           first tab is captioned "Screens Allocate To User". */
        items.add(item("Screen Allocate To User", "UserRightsByCompany",
                "/user-management/rights", null));
        /* DefineReports_Click -> frmReportConfig */
        items.add(item("Define Reports", "frmReportConfig",
                "/configurations/define-reports", null));
        /* btnPlBSNotes_Click -> BsPlSettingForm */
        items.add(item("PL & BS Notes", "BsPlSettingForm", null, "Not ported yet."));
        /* companyProfileToolStripMenuItem_Click -> frmCompanyProfile */
        items.add(item("Company Profile", "frmCompanyProfile",
                "/configurations/company-profile", null));
        /* btnAuditLogReport_Click -> AuditLogReport */
        items.add(item("Audit Log Report", "AuditLogReport", null, "Not ported yet."));
        /* sQLQueryExecuterToolStripMenuItem_Click -> new SqlLogin().ShowDialog(), which opens
           SqlQueryExecuter: a free-text SQL window run against the live database with credentials
           typed into the dialog. Deliberately not ported - see the note rendered to the user. */
        items.add(item("SQL Query Executer", "SqlLogin -> SqlQueryExecuter", null,
                "Runs free-text SQL against the live database. Not ported - ask before I build it."));
        /* btnWeighBridge_WeightUpdate_Click -> WeighBridge_WeightUpdate */
        items.add(item("WeighBridge_WeightsUpdate", "WeighBridge_WeightUpdate", null,
                "Not ported yet."));
        /* BtnThemeForm_Click -> ThemeSelectionForm */
        items.add(item("Theme", "ThemeSelectionForm", null, "Not ported yet."));
        /* btnAccountMovement_Click -> AccountToAccountTransfer */
        items.add(item("Account Movement", "AccountToAccountTransfer", null, "Not ported yet."));
        return items;
    }

    /**
     * btnUtility - DashboardNew.cs:5634-5638 order, hidden items removed. Two items remain.
     */
    private static List<Map<String, Object>> systemUtilitiesItems() {
        List<Map<String, Object>> items = new ArrayList<>();
        /* btnDatabaseBackup_Click_1 -> BackUpDatabase */
        items.add(item("DataBase Backup", "BackUpDatabase", null,
                "Backs up the live database from the server. Not ported - ask before I build it."));
        /* licenseKeyToolStripMenuItem_Click -> LicenseKey */
        items.add(item("License Key", "LicenseKey", "/utilities/license-key", null));
        return items;
    }

    @GetMapping({"/admin-panel", "/admin"})
    public String adminPanel(Model model) {
        boolean admin = isDesktopAdmin();
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("menuTitle", "Admin Panel");
        model.addAttribute("menuSource", "DashboardNew.contextMenuStrip3 -> btnadminpanel");
        /* DashboardNew.cs:1191-1196 - the item is simply not drawn for a non-Admin RoleName. */
        model.addAttribute("denied", !admin);
        model.addAttribute("deniedReason",
                "The desktop shows Admin Panel only when RoleName is \"Admin\" (DashboardNew_Load). "
              + "Your RoleName is " + describeRole() + ".");
        model.addAttribute("items", admin ? adminPanelItems() : new ArrayList<>());
        model.addAttribute("hiddenNote",
                "Two further items exist in the designer but are Visible = false on the desktop and "
              + "are therefore not shown here either: Screen Defination, Stock_Reconcilation.");
        return "gear_menu";
    }

    @GetMapping({"/system-utilities", "/utilities"})
    public String systemUtilities(Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("menuTitle", "System Utilities");
        model.addAttribute("menuSource", "DashboardNew.contextMenuStrip3 -> btnUtility");
        model.addAttribute("denied", false);
        model.addAttribute("items", systemUtilitiesItems());
        model.addAttribute("hiddenNote",
                "Eleven further items exist in the designer but are Visible = false on the desktop and "
              + "are therefore not shown here either: Accounts Lookups, SupplierDefine, Define Inventory, "
              + "Define Country, DateLock, SMS Contacts, Screen Message Receivers, SMS Confiquration, "
              + "SMS History, InvLookUps, Scale Kart Define.");
        return "gear_menu";
    }

    /* ------------------------------------------------------------------------------- helpers */

    private boolean isDesktopAdmin() {
        String role = safeRole();
        return "Admin".equals(role);
    }

    private String describeRole() {
        String role = safeRole();
        return (role == null || role.trim().isEmpty()) ? "not set" : "\"" + role + "\"";
    }

    private String safeRole() {
        try {
            return currentUserContext.currentRoleName();
        } catch (RuntimeException e) {
            return null;
        }
    }

    private static Map<String, Object> item(String text, String form, String route, String note) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("text", text);
        m.put("form", form);
        m.put("route", route);
        m.put("note", note);
        m.put("built", route != null);
        return m;
    }
}
