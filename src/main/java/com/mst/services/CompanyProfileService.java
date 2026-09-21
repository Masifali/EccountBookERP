package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.CompanyProfileRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Base64;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Company Profile" - Admin Panel item 6, desktop form
 * {@code Architecture.WinApp.Configurations.frmCompanyProfile}.
 *
 * ---------------------------------------------------------------------------------------------
 * THE GATE
 * ---------------------------------------------------------------------------------------------
 * The form itself checks no rights at all - it has no ScreenDefinition row, so there are none to
 * check. Its only gate is that the menu item carrying it is drawn only for
 * {@code UserAccount.RoleName == "Admin"} (DashboardNew.cs:1193).
 *
 * On the desktop, not drawing the item is enough. On the web a URL is reachable whether or not a
 * link to it was rendered, so the same comparison is made here, server-side, on every call. That
 * is the desktop's gate enforced at the only place the web can enforce it - not an extra rule.
 *
 * ---------------------------------------------------------------------------------------------
 * TENANCY
 * ---------------------------------------------------------------------------------------------
 * OrganizationFill() binds exactly one row: Organization.GetByID(UserAccount.OrganizationId).
 * The organization is therefore never chosen by the operator, and OrgCompanyTypeId on a save is
 * always the signed-in user's own organization. Nothing the client posts is used for it, and a
 * record whose Id does not belong to that organization cannot be loaded or updated.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THE DESKTOP'S OWN UPDATE DOES TO COLUMNS THIS SCREEN CANNOT SHOW
 * ---------------------------------------------------------------------------------------------
 * Insert() fills thirteen fields and leaves the rest of the model at its C# defaults. Sent
 * through Proc_Company_Update, whose SET list assigns every one of them, that blanks ten columns
 * the screen has no input for - CompBaseCurr, CompLogo, CompEmailB, CompTel, CompMobileB,
 * CompMobileC, PictureURL, CityName, CompanyWebsite, CompanyFaxNo - and writes 0 into EntryUser
 * and ModifyUser and "now" into EntryDate.
 *
 * That is what the desktop does to live rows today. It is reproduced here rather than quietly
 * corrected, because the instruction is that both sides behave the same; the behaviour is
 * reported separately so it can be decided on deliberately.
 */
@Service
public class CompanyProfileService {

    /** frmCompanyProfile.browse_Click: "File Size Limit Exceeded" above this many bytes. */
    private static final long LOGO_MAX_BYTES = 5_000_000L;

    private final CurrentUserContext context;
    private final CompanyProfileRepository repo;

    public CompanyProfileService(CurrentUserContext context, CompanyProfileRepository repo) {
        this.context = context;
        this.repo = repo;
    }

    /** DashboardNew.cs:1193 - the one condition under which this screen is reachable. */
    public boolean canOpen() {
        try {
            return "Admin".equals(context.currentRoleName());
        } catch (RuntimeException e) {
            return false;
        }
    }

    private UserAccount admin() {
        UserAccount u = context.requireAccountingUser();
        if (!"Admin".equals(context.currentRoleName())) {
            throw new AccessDeniedException(
                    "Company Profile is on the desktop's Admin Panel, which is shown only to the Admin role.");
        }
        return u;
    }

    // ================================================================================== read

    /** OrganizationFill() + LocationsBind() - what the form has on screen after it loads. */
    public Map<String, Object> load() {
        UserAccount u = admin();
        int orgId = u.getOrganizationId() == null ? 0 : u.getOrganizationId();

        Map<String, Object> org = repo.organization(orgId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("organizationId", orgId);
        out.put("organizationName", org == null ? null : org.get("OrgName"));
        out.put("rows", grid(repo.companies(orgId)));
        return out;
    }

    /**
     * LocationsBind() builds its own DataTable with the desktop's column names, and hides Id,
     * CompanyId, OrgId and LogoImage. The same shape is returned here so the web grid shows the
     * same eleven columns in the same order.
     */
    private List<Map<String, Object>> grid(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("Id",             r.get("Id"));                             // hidden
            g.put("CompanyId",      r.get("CompanyId"));                      // hidden
            g.put("OrgId",          r.get("OrgCompanyTypeId"));               // hidden
            g.put("CompanyCode",    r.get("CompCode"));
            g.put("CompanyName",    r.get("CompName"));
            g.put("OtherName",      r.get("CompanyNameOtherLanguage"));
            g.put("ReportingTitle", r.get("CompReportingTitle"));
            g.put("Email",          r.get("CompEmailA"));
            g.put("Phone",          r.get("CompMobileA"));
            g.put("Country",        r.get("CompCountry"));
            g.put("State",          r.get("CompState"));
            g.put("Address",        r.get("CompAddress"));
            g.put("OtherAddress",   r.get("CompanyAddressOtherLanguage"));
            g.put("ContactPerson",  r.get("CompContactPerson"));
            g.put("LogoImage",      dataUri(r.get("CompLogoImage")));         // hidden; shown in the picture box
            out.add(g);
        }
        return out;
    }

    private static String dataUri(Object v) {
        if (!(v instanceof byte[])) return null;
        byte[] b = (byte[]) v;
        if (b.length == 0) return null;
        return "data:image;base64," + Base64.getEncoder().encodeToString(b);
    }

    // ================================================================================== write

    /**
     * Insert() - validation first, in the desktop's order and with its exact messages, then the
     * model, then Company.Save.
     *
     * The desktop asks "Are you sure to Save?" / "Are you sure to Update?" between the two. That
     * confirmation is the operator's, so it is raised by the page before it posts, not invented
     * server-side.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> form) {
        UserAccount u = admin();
        int orgId = u.getOrganizationId() == null ? 0 : u.getOrganizationId();

        String code          = text(form.get("code"));
        String name          = text(form.get("name"));
        String otherName     = text(form.get("otherName"));
        String reporting     = text(form.get("reportingTitle"));
        String contact       = text(form.get("contactPerson"));
        String address       = text(form.get("address"));
        String otherAddress  = text(form.get("otherAddress"));
        String email         = text(form.get("email"));
        String phone         = text(form.get("phone"));
        String country       = text(form.get("country"));
        String state         = text(form.get("state"));

        /* FormValidation(), in order. "0" fails the same way an empty box does, as on the desktop.
           The organization check is first there; here the organization is the signed-in user's, so
           it can only fail if the account has none. */
        if (orgId == 0)                       throw new IllegalArgumentException("CompanyName Field is Required");
        if (blank(code))                      throw new IllegalArgumentException("Code Field is Required");
        if (blank(name))                      throw new IllegalArgumentException("Name Field is Required");
        if (blank(reporting))                 throw new IllegalArgumentException("Reporting Title Field is Required");
        if (blank(contact))                   throw new IllegalArgumentException("Contact Person Field is Required");
        if (blank(address))                   throw new IllegalArgumentException("Address Field is Required");
        if (blank(email))                     throw new IllegalArgumentException("Email Field is Required");
        if (blank(phone))                     throw new IllegalArgumentException("Phone Number Field is Required");

        /* RecId is the grid row the operator double-clicked. It is re-read here instead of being
           trusted: an Id from another organization must not be updatable through this screen, and
           CompanyId has to come from the stored row rather than from the post, because
           Proc_Company_Update writes CompanyId back. */
        int recId = CompanyProfileRepository.intOf(form.get("id"));
        int companyId = 0;
        if (recId > 0) {
            Map<String, Object> existing = repo.company(orgId, recId);
            if (existing == null) {
                throw new AccessDeniedException("That company is not in your organization.");
            }
            companyId = CompanyProfileRepository.intOf(existing.get("CompanyId"));
        }

        byte[] logo = logo(form.get("logo"), form.get("logoUnchanged"), orgId, recId);

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = CompanyProfileRepository.blankModel();
        m.put("Id",                          recId);
        m.put("OrgCompanyTypeId",            orgId);
        m.put("CompAddress",                 address);
        m.put("CompCode",                    code);
        m.put("CompContactPerson",           contact);
        m.put("CompCountry",                 country);
        m.put("CompEmailA",                  email);
        m.put("CompMobileA",                 phone);
        m.put("CompName",                    name);
        m.put("CompReportingTitle",          reporting);
        m.put("CompState",                   state);
        m.put("CompanyId",                   companyId);
        m.put("CompanyNameOtherLanguage",    otherName);
        m.put("CompanyNameOtherLing",        otherName);   // Insert():369-370 sets both
        m.put("CompanyAddressOtherLanguage", otherAddress);
        m.put("CompLogoImage",               logo);
        m.put("EntryDate",                   now);
        m.put("ModifyDate",                  now);
        /* Left at the C# defaults the desktop sends, so the two write identical rows:
           EntryUser 0, ModifyUser 0, CompanyTemplateId 0, AllowedUserCount 0, and null for
           CompBaseCurr, CompLogo, CompEmailB, CompMobileB, CompMobileC, CompTel, CompType,
           PictureURL, CityName, CompanyWebsite, CompanyFaxNo. */
        m.put("EntryUser",         0);
        m.put("ModifyUser",        0);
        m.put("CompanyTemplateId", 0);
        m.put("AllowedUserCount",  0);

        int id = repo.save(m);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        /* Insert():409 - the desktop's own two messages. */
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        out.put("rows", grid(repo.companies(orgId)));       // Insert() ends with LocationsBind()
        return out;
    }

    /**
     * browse_Click keeps the picture in the form until Save, and Insert() writes whatever is in
     * the picture box - so leaving the logo alone on an update must send the STORED bytes back,
     * not null, or the update would clear it.
     */
    private byte[] logo(Object posted, Object unchanged, int orgId, int recId) {
        boolean keep = Boolean.TRUE.equals(unchanged) || "true".equals(String.valueOf(unchanged));
        if (keep && recId > 0) {
            Map<String, Object> existing = repo.company(orgId, recId);
            Object v = existing == null ? null : existing.get("CompLogoImage");
            return (v instanceof byte[] && ((byte[]) v).length > 0) ? (byte[]) v : null;
        }
        String s = posted == null ? null : String.valueOf(posted);
        if (s == null || s.isEmpty() || "null".equals(s)) return null;
        int comma = s.indexOf(',');
        if (s.startsWith("data:") && comma > 0) s = s.substring(comma + 1);
        byte[] bytes;
        try {
            bytes = Base64.getDecoder().decode(s);
        } catch (IllegalArgumentException e) {
            throw new IllegalArgumentException("The logo could not be read as an image.");
        }
        if (bytes.length > LOGO_MAX_BYTES) throw new IllegalArgumentException("File Size Limit Exceeded");
        return bytes.length == 0 ? null : bytes;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** FormValidation treats "0" as empty on every field it checks. */
    private static boolean blank(String s) { return s.isEmpty() || "0".equals(s); }
}
