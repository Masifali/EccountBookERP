package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.UserDefineRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.LegacyUserPasswordEncoder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.regex.Pattern;

/**
 * BLL of the Admin Panel item "User Rights" = Architecture.WinApp.Configurations.frmUserRights, which despite its
 * caption is the USER MASTER (bottom tabs "User Define" / "User History"; top tabs "Define User" and
 * "ApplicationsAllocateToUser" = BookingOffice.frmApplicationsAllocateToUser hosted in the panel), plus
 * Lookups.frmBranchesAllocationToUser that its "Branches Allocation To User" button opens.
 *
 * GATE. None of these forms reads screen rights: they are reached only from DashboardNew's gear menu item
 * "Admin Panel", which is drawn only when UserAccount.RoleName == "Admin" (DashboardNew.cs:1191-1196). The same
 * RoleName test is made here for every read and write - see GearMenuController.
 *
 * Every save repeats the form's validation with its wording and order, then builds the desktop model exactly as
 * frmUserRights.Insert() / the allocation forms do. Organization, company, branch and entry user come from the
 * session; every id the browser sends (user, company, allocation row, application row, branch, role) is checked
 * against the list the desktop itself would have shown before it is written.
 */
@Service
public class UserDefineService {

    /** frmUserRights.ValidatePhoneNumber / ValidateCNIC / ValidateEmail - the exact regular expressions. */
    private static final Pattern PHONE = Pattern.compile("^\\d{4}-\\d{3}-\\d{4}$");
    private static final Pattern CNIC = Pattern.compile("^\\d{5}-\\d{7}-\\d{1}$");
    private static final Pattern EMAIL = Pattern.compile("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");

    /** GetERPFeatureById(16) - "DeviceDependency" (frmUserRights_Load :271). */
    private static final int FEATURE_DEVICE_DEPENDENCY = 16;

    @Autowired private UserDefineRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private LegacyUserPasswordEncoder passwords;
    @Autowired private DesktopAttachmentStore attachments;

    // ================================================================= gate

    public UserAccount admin() {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!"Admin".equals(currentUserContext.currentRoleName()))
            throw new AccessDeniedException("Admin Panel - User Rights is shown on the desktop only when RoleName is \"Admin\".");
        return u;
    }

    // ================================================================= frmUserRights_Load

    /**
     * frmUserRights_Load :264 - DeviceDependencyFeature (button + check visible), TwoWayAuthentication configuration
     * (check + grid column visible), bindUser, UserRole, LocationsBind, and which extra tabs AddTabPage would add:
     * ApplicationsAllocateToUser always; the three cost-centre tabs when App 5 (Supplier Portal) is allocated and
     * active for the company; CustomerPortalRegistration when App 4 or 6 is.
     */
    public Map<String, Object> setup() {
        UserAccount u = admin();
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("deviceFeature", deviceFeature(org, comp));
        r.put("twoWayAuthentication", truthy(attachments.configuration(u, "TwoWayAuthentication")));
        r.put("users", userGrid(org, comp));
        r.put("roles", repo.roles(org, comp));
        r.put("locations", locations(org));
        Boolean supplierPortal = null, customerPortal = null;
        for (Map<String, Object> a : repo.applicationsAllocatedToCompanies()) {
            if (asInt(ci(a, "CompanyId")) != comp) continue;
            int app = asInt(ci(a, "AppId"));
            boolean active = truthy(ci(a, "IsActive"));
            /* List.Find returns the FIRST matching row and only then tests IsActive. */
            if (app == 5 && supplierPortal == null) supplierPortal = active;
            if ((app == 4 || app == 6) && customerPortal == null) customerPortal = active;
        }
        if (supplierPortal == null) supplierPortal = false;
        if (customerPortal == null) customerPortal = false;
        r.put("supplierPortal", supplierPortal);
        r.put("customerPortal", customerPortal);
        return r;
    }

    /** bindUser :394 - the grid built from ReadAll with the password DECRYPTED, as the desktop grid shows it. */
    public List<Map<String, Object>> users() {
        UserAccount u = admin();
        return userGrid(u.getOrganizationId(), u.getCompanyId());
    }

    private List<Map<String, Object>> userGrid(int org, int comp) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : repo.readAllUsers(org, comp)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ID", asInt(ci(row, "ID")));
            m.put("UserName", str(ci(row, "UserName")));
            String plain = passwords.decode(str(ci(row, "Password")));
            m.put("Password", plain == null ? "" : plain);
            m.put("UserGroupName", str(ci(row, "UserGroupName")));
            m.put("FirstName", str(ci(row, "FirstName")));
            m.put("LastName", str(ci(row, "LastName")));
            m.put("FatherName", str(ci(row, "FatherName")));
            m.put("Title", str(ci(row, "Title")));
            m.put("WhatsApp", str(ci(row, "WhatsApp")));
            m.put("PhoneNumber", str(ci(row, "PhoneNumber")));
            m.put("CNIC", str(ci(row, "CNICNumber")));
            m.put("Email", str(ci(row, "Email")));
            m.put("IsActive", truthy(ci(row, "IsActive")));
            m.put("AppId", asInt(ci(row, "AppId")));
            m.put("AuthenticationEnabled", asInt(ci(row, "AuthenticationEnabled")) == 1
                    ? " Two Way Authentication Enabled" : "Authentication not Enabled");
            Object dd = ci(row, "DeviceDependency");
            m.put("DeviceDependency", dd == null ? "" : (truthy(dd) ? "True" : "False"));
            out.add(m);
        }
        return out;
    }

    /** LocationsBind :533 - (Id 0, CompanyId, Location, Active false, UserStatus false) per company of the organization. */
    private List<Map<String, Object>> locations(int org) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> c : repo.companiesOfOrganization(org)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("CompanyId", asInt(ci(c, "Id")));
            m.put("Location", str(ci(c, "CompName")));
            m.put("Active", false);
            m.put("UserStatus", false);
            out.add(m);
        }
        return out;
    }

    // ================================================================= grduserdefine_DoubleClick

    /** UserAccount.GetByID + the DAL's allocation and profile reads; the record must be one bindUser listed. */
    public Map<String, Object> user(int id) {
        UserAccount u = admin();
        Map<String, Object> row = listedUser(u, id);
        if (row == null) throw new IllegalArgumentException("Record not found.");
        List<Map<String, Object>> byId = repo.readUserById(id);
        if (byId.isEmpty()) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> ua = byId.get(0);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("ID", id);
        r.put("FirstName", str(ci(ua, "FirstName")));
        r.put("LastName", str(ci(ua, "LastName")));
        r.put("UserName", str(ci(ua, "UserName")));
        String plain = passwords.decode(str(ci(ua, "Password")));
        r.put("Password", plain == null ? "" : plain);
        r.put("UserRoleId", asInt(ci(ua, "UserRoleId")));
        r.put("IsActive", truthy(ci(ua, "IsActive")));
        r.put("AuthenticationEnabledForUser", asInt(ci(ua, "AuthenticationEnabledForUser")) != 0);
        r.put("DeviceDependency", truthy(ci(ua, "DeviceDependency")));
        List<Map<String, Object>> prof = repo.profileOfUser(id);
        if (!prof.isEmpty()) {
            Map<String, Object> p = prof.get(0);
            r.put("hasProfile", true);
            r.put("FatherName", str(ci(p, "FatherName")));
            r.put("WhatsApp", str(ci(p, "WhatsApp")));
            r.put("CellNo", str(ci(p, "CellNo")));
            r.put("CNICNumber", str(ci(p, "CNICNumber")));
            r.put("Email", str(ci(p, "Email")));
            r.put("ProfileImageFileName", str(ci(p, "ProfileImageFileName")));
        } else {
            r.put("hasProfile", false);
        }
        List<Map<String, Object>> allocs = new ArrayList<>();
        for (Map<String, Object> a : repo.allocationsOfUser(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(a, "Id")));
            m.put("CompanyId", asInt(ci(a, "CompanyId")));
            m.put("CompName", str(ci(a, "CompName")));
            m.put("IsActive", asInt(ci(a, "IsActive")));
            allocs.add(m);
        }
        r.put("allocations", allocs);
        return r;
    }

    /** The profile picture the double-click shows (Attachment Folder Path, or the VPS when IsVpsAttachmentsServiceOn). */
    public byte[] profileImage(int id) {
        UserAccount u = admin();
        if (listedUser(u, id) == null) throw new IllegalArgumentException("Record not found.");
        List<Map<String, Object>> prof = repo.profileOfUser(id);
        String name = prof.isEmpty() ? "" : str(ci(prof.get(0), "ProfileImageFileName"));
        if (name.trim().isEmpty()) throw new IllegalArgumentException("No picture.");
        return attachments.read(u, baseName(name));
    }

    // ================================================================= btnsave_Click / btnUpdate_Click -> Insert()

    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = admin();
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        int recId = asInt(b.get("recId"));
        boolean update = truthy(b.get("update"));

        /* btnUpdate_Click :747 */
        if (update && recId == 0) throw new IllegalArgumentException("Id not found");
        if (!update) recId = 0;   /* btnsave_Click :734 RecId = 0 */

        /* FormValidation :759 - order and wording */
        String userName = raw(b.get("userName")), password = raw(b.get("password"));
        String firstName = raw(b.get("firstName")), lastName = raw(b.get("lastName")), fatherName = raw(b.get("fatherName"));
        if (blankOrZero(userName)) throw new IllegalArgumentException("UserName Field Required");
        if (blankOrZero(password)) throw new IllegalArgumentException("Password Field Required");
        if (blankOrZero(firstName)) throw new IllegalArgumentException("First Name Field Required");
        if (blankOrZero(lastName)) throw new IllegalArgumentException("Last Name Field Required");
        if (blankOrZero(fatherName)) throw new IllegalArgumentException("Father Name Field Required");
        int roleId = asInt(b.get("roleId"));
        if (!idIn(repo.roles(org, comp), "Id", roleId)) throw new IllegalArgumentException("Role Name Field Required");

        if (recId > 0 && listedUser(u, recId) == null) throw new IllegalArgumentException("Record not found.");

        /* :617-641 - one allocation per Location row; BranchId = the signed-in user's branch */
        List<Map<String, Object>> rows = list(b.get("locations"));
        Set<Integer> companies = new HashSet<>();
        for (Map<String, Object> c : repo.companiesOfOrganization(org)) companies.add(asInt(ci(c, "Id")));
        Set<Integer> ownAllocationIds = new HashSet<>();
        if (recId > 0) for (Map<String, Object> a : repo.allocationsOfUser(recId)) ownAllocationIds.add(asInt(ci(a, "Id")));
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        List<Map<String, Object>> allocations = new ArrayList<>();
        for (Map<String, Object> row : rows) {
            int companyId = asInt(row.get("companyId"));
            int allocationId = asInt(row.get("id"));
            if (!companies.contains(companyId)) throw new IllegalArgumentException("Location not found.");
            if (allocationId != 0 && !ownAllocationIds.contains(allocationId))
                throw new IllegalArgumentException("Location allocation does not belong to this user. Press New and open the user again.");
            Map<String, Object> a = new LinkedHashMap<>();         /* Model 0071, non-virtual properties */
            a.put("EntryDate", now);
            a.put("ModifyDate", now);
            a.put("BranchId", nz(u.getBranchesId()));
            a.put("CompanyId", companyId);
            a.put("EntryUserId", 0);
            a.put("Id", allocationId);
            a.put("IsActive", truthy(row.get("active")) ? 1 : 0);
            a.put("ModifyUserId", 0);
            a.put("OrganizationId", org);
            a.put("UserAccountId", 0);
            allocations.add(a);
        }
        if (allocations.isEmpty()) throw new IllegalArgumentException("Please select Location first");

        /* :667-686 - checked after the confirmation, in this order */
        String whatsApp = raw(b.get("whatsApp")), cellNo = raw(b.get("cellNo")), cnic = raw(b.get("cnic")), email = raw(b.get("email"));
        if (!PHONE.matcher(whatsApp).matches()) throw new IllegalArgumentException("WhatsApp No is not Valid");
        if (!PHONE.matcher(cellNo).matches()) throw new IllegalArgumentException("Cell No is not Valid");
        if (!CNIC.matcher(cnic).matches()) throw new IllegalArgumentException("CNIC is not Valid");
        if (!EMAIL.matcher(email).matches()) throw new IllegalArgumentException("Email is not Valid");

        boolean deviceFeature = deviceFeature(org, comp);

        /* Model 0099 UserAccount, non-virtual properties (CellNo is never set by the form -> null -> not sent) */
        Map<String, Object> account = new LinkedHashMap<>();
        account.put("IsActive", truthy(b.get("isActive")));
        account.put("DeviceDependency", deviceFeature && truthy(b.get("deviceDependency")));
        account.put("CompanyId", comp);
        account.put("Id", recId);
        account.put("OrganizationId", org);
        account.put("UserGroupId", 0);           /* CmbUserName (hidden) - UserFromRefresh sets 0, nothing sets it again */
        account.put("UserRoleId", roleId);
        account.put("BranchesId", 0);            /* grdBranches.CurrentRow "Id" - 0 for every row of a new user; Update ignores it */
        account.put("SupplierCustomerId", 0);
        account.put("EntryUserId", nz(u.getId()));
        account.put("ModifyUserId", 0);
        account.put("AuthenticationCode", 0);
        account.put("FirstName", firstName);
        account.put("LastName", lastName);
        account.put("Password", passwords.encode(password));
        account.put("UserName", userName);
        account.put("EntryDate", now);
        account.put("ModifyDate", now);
        account.put("AuthenticationEnabledForUser", truthy(b.get("enableAuthentication")) ? 1 : 0);
        account.put("UserTypeId", 0);
        account.put("AppId", 0);
        account.put("EmployeeId", 0);

        /* UserProfileList - always present on the desktop (new UserProfile()); Id = the profile GetByID returned */
        int profileId = 0;
        String storedImage = "";
        if (recId > 0) {
            List<Map<String, Object>> prof = repo.profileOfUser(recId);
            if (!prof.isEmpty()) {
                profileId = asInt(ci(prof.get(0), "Id"));
                storedImage = str(ci(prof.get(0), "ProfileImageFileName"));
            }
        }
        Map<String, Object> profile = new LinkedHashMap<>();   /* Model 0104, non-virtual properties */
        profile.put("Age", 0);
        profile.put("Id", profileId);
        profile.put("UserId", 0);
        profile.put("CellNo", cellNo);
        profile.put("Email", email);
        profile.put("FatherName", fatherName);
        profile.put("WhatsApp", whatsApp);
        profile.put("CNICNumber", cnic);
        profile.put("JoiningDate", now);

        /* ImageNameOrPath: "keep" = the name GetByID loaded, "clear" = null (btnClearImage), "new" = a Browse'd file */
        String imageMode = raw(b.get("imageMode"));
        UserDefineRepository.ProfileImageWriter writer = null;
        if ("new".equals(imageMode)) {
            String fileName = raw(b.get("imageName"));
            String lower = fileName.toLowerCase();
            if (!(lower.endsWith(".jpg") || lower.endsWith(".jpeg") || lower.endsWith(".png")))
                throw new IllegalArgumentException("Only *.jpg, *.jpeg and *.png pictures can be chosen.");
            byte[] bytes;
            try { bytes = Base64.getDecoder().decode(raw(b.get("imageBase64"))); }
            catch (IllegalArgumentException e) { throw new IllegalArgumentException("The picture could not be read."); }
            if (bytes.length > DesktopAttachmentStore.MAX_BYTES) throw new IllegalArgumentException("File Size Exceeds 5MB Of File: ");
            final byte[] data = bytes;
            writer = () -> attachments.store(u, fileName, data);
        } else if ("keep".equals(imageMode)) {
            profile.put("ProfileImageFileName", storedImage);
        } else {
            /* "clear" or nothing loaded: ImageNameOrPath null/"" -> "" for a new user, null after Clear */
            if (!"clear".equals(imageMode)) profile.put("ProfileImageFileName", "");
        }

        int saved = repo.saveUser(account, profile, allocations, 0, writer);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("id", saved);
        r.put("message", recId == 0 ? "Save Successfully" : "Update Successfully");
        return r;
    }

    // ================================================================= frmApplicationsAllocateToUser

    /** UserNameFill - UserAccount.ReadAll (Id, UserName). */
    public List<Map<String, Object>> appUsers() {
        UserAccount u = admin();
        return idName(repo.readAllUsers(u.getOrganizationId(), u.getCompanyId()), "ID", "UserName");
    }

    /** BtnShow_Click -> CompanyGridFill: GetAllCompaniesByUserId with IsActive = 1 (Id, CompanyName). */
    public List<Map<String, Object>> appCompanies(int userId) {
        UserAccount u = admin();
        if (listedUser(u, userId) == null) throw new IllegalArgumentException("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> c : repo.companiesOfUser(u.getOrganizationId(), userId, 1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(c, "CompanyId")));
            m.put("CompanyName", str(ci(c, "CompName")));
            out.add(m);
        }
        return out;
    }

    /** grdCompany_SelectionChanged -> UnAllocatedGridFill + AllocatedGridFill. */
    public Map<String, Object> appGrids(int userId, int companyId) {
        UserAccount u = admin();
        checkUserCompany(u, userId, companyId);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("unallocated", appRows(repo.appsUnAllocated(companyId, userId), userId, companyId));
        r.put("allocated", appRows(repo.appsAllocated(companyId, userId), userId, companyId));
        return r;
    }

    /** Insert(grd, "Allocate"|"Un-Allocate", Active) - one InsertAndUpdate per checked row. */
    public Map<String, Object> saveApps(Map<String, Object> b) {
        UserAccount u = admin();
        boolean allocate = truthy(b.get("allocate"));
        String word = allocate ? "Allocate" : "Un-Allocate";
        int userId = asInt(b.get("userId")), companyId = asInt(b.get("companyId"));
        List<Map<String, Object>> checked = list(b.get("rows"));
        if (checked.isEmpty()) throw new IllegalArgumentException("Check Row's Which You want To " + word);
        checkUserCompany(u, userId, companyId);
        List<Map<String, Object>> source = appRows(allocate ? repo.appsUnAllocated(companyId, userId)
                                                            : repo.appsAllocated(companyId, userId), userId, companyId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> c : checked) {
            long id = asLong(c.get("id"));
            int appId = asInt(c.get("appId"));
            boolean found = false;
            for (Map<String, Object> s : source)
                if (asLong(s.get("Id")) == id && asInt(s.get("AppId")) == appId) { found = true; break; }
            if (!found) throw new IllegalArgumentException("The application list has changed. Press Show again.");
            Map<String, Object> m = new LinkedHashMap<>();      /* Model 0003, non-virtual properties */
            m.put("ApplicationsAllocateToUserId", id);
            m.put("UserId", userId);
            m.put("CompanyId", companyId);
            m.put("AppId", appId);
            m.put("IsActive", allocate);
            m.put("EntryUserId", nz(u.getId()));
            m.put("ModifyUserId", nz(u.getId()));
            m.put("EntryDate", now);
            m.put("ModifyDate", now);
            rows.add(m);
        }
        repo.saveAppAllocations(rows);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", "Row's " + word + " Successfully");
        return r;
    }

    private List<Map<String, Object>> appRows(List<Map<String, Object>> data, int userId, int companyId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : data) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asLong(ci(d, "ApplicationsAllocateToUserId")));
            m.put("UserId", userId);
            m.put("UserName", str(ci(d, "UserName")));
            m.put("CompanyId", companyId);
            m.put("CompanyName", str(ci(d, "CompanyName")));
            m.put("AppId", asInt(ci(d, "AppId")));
            m.put("AppName", str(ci(d, "App")));
            m.put("IsActive", ci(d, "IsActive"));
            out.add(m);
        }
        return out;
    }

    private void checkUserCompany(UserAccount u, int userId, int companyId) {
        if (listedUser(u, userId) == null) throw new IllegalArgumentException("Record not found.");
        if (!idIn(repo.companiesOfUser(u.getOrganizationId(), userId, 1), "CompanyId", companyId))
            throw new IllegalArgumentException("Company not found for this user.");
    }

    // ================================================================= frmBranchesAllocationToUser

    public List<Map<String, Object>> branchUsers() { return appUsers(); }

    /** ShowData - "Select Branch Name First..." when no user; ActionId 1 (un-allocated) and 2 (allocated). */
    public Map<String, Object> branchGrids(int userId) {
        UserAccount u = admin();
        if (userId <= 0) throw new IllegalArgumentException("Select Branch Name First...");
        if (listedUser(u, userId) == null) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("unallocated", idName(repo.branchesOfUser(u.getOrganizationId(), u.getCompanyId(), userId, 1), "BranchId", "BranchName"));
        r.put("allocated", idName(repo.branchesOfUser(u.getOrganizationId(), u.getCompanyId(), userId, 2), "BranchId", "BranchName"));
        return r;
    }

    /** BtnAllocateBranchs_Click - one USP_BranchesAllocationToUser_Insert per checked row (FinancialYearId 0: never set). */
    public Map<String, Object> allocateBranches(Map<String, Object> b) {
        UserAccount u = admin();
        int userId = asInt(b.get("userId"));
        if (userId == 0) throw new IllegalArgumentException("Select Branch First");
        List<Object> ids = rawList(b.get("branchIds"));
        if (ids.isEmpty()) throw new IllegalArgumentException("Checked Row's first To Allocate Branch");
        if (listedUser(u, userId) == null) throw new IllegalArgumentException("Record not found.");
        Set<Integer> open = idSet(repo.branchesOfUser(u.getOrganizationId(), u.getCompanyId(), userId, 1), "BranchId");
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Object o : ids) {
            int branchId = asInt(o);
            if (!open.contains(branchId)) throw new IllegalArgumentException("The branch list has changed. Press Show again.");
            Map<String, Object> m = new LinkedHashMap<>();      /* Model 0016, non-virtual properties */
            m.put("EntryDate", now);
            m.put("ModifyDate", now);
            m.put("CompanyId", u.getCompanyId());
            m.put("EntryUserId", nz(u.getId()));
            m.put("FinancialYearId", 0);
            m.put("Id", 0);
            m.put("BranchId", branchId);
            m.put("UserId", userId);
            m.put("ModifyUserId", nz(u.getId()));
            m.put("OrganizationId", u.getOrganizationId());
            rows.add(m);
        }
        repo.saveBranchAllocations(rows);
        return ok("Branch Allocated Successfully");
    }

    /** btnDeAllocate_Click - DeleteById(UserId, "id,id,"). */
    public Map<String, Object> deallocateBranches(Map<String, Object> b) {
        UserAccount u = admin();
        int userId = asInt(b.get("userId"));
        if (userId == 0) throw new IllegalArgumentException("Select Branch First");
        List<Object> ids = rawList(b.get("branchIds"));
        if (ids.isEmpty()) throw new IllegalArgumentException("Checked Row's first To Un-Allocate Branch");
        if (listedUser(u, userId) == null) throw new IllegalArgumentException("Record not found.");
        Set<Integer> allocated = idSet(repo.branchesOfUser(u.getOrganizationId(), u.getCompanyId(), userId, 2), "BranchId");
        StringBuilder csv = new StringBuilder();
        for (Object o : ids) {
            int branchId = asInt(o);
            if (!allocated.contains(branchId)) throw new IllegalArgumentException("The branch list has changed. Press Show again.");
            csv.append(branchId).append(',');
        }
        repo.deleteBranchAllocations(userId, csv.toString());
        return ok("Branch UnAllocated Successfully");
    }

    // ================================================================= helpers

    private Map<String, Object> listedUser(UserAccount u, int id) {
        if (id <= 0) return null;
        for (Map<String, Object> row : repo.readAllUsers(u.getOrganizationId(), u.getCompanyId()))
            if (asInt(ci(row, "ID")) == id) return row;
        return null;
    }

    private boolean deviceFeature(int org, int comp) {
        return idIn(repo.erpFeatures(org, comp), "Id", FEATURE_DEVICE_DEPENDENCY);
    }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idKey, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idKey)));
            m.put("Name", str(ci(r, nameKey)));
            out.add(m);
        }
        return out;
    }

    private static Set<Integer> idSet(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(asInt(ci(r, key)));
        return s;
    }

    private static boolean idIn(List<Map<String, Object>> rows, String key, int id) {
        if (id == 0) return false;
        for (Map<String, Object> r : rows) if (asInt(ci(r, key)) == id) return true;
        return false;
    }

    private static Map<String, Object> ok(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", message);
        return r;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    private static List<Object> rawList(Object v) {
        return v instanceof List ? new ArrayList<>((List<Object>) v) : new ArrayList<>();
    }

    private static String baseName(String name) {
        String s = name.replace('\\', '/');
        int i = s.lastIndexOf('/');
        return i >= 0 ? s.substring(i + 1) : s;
    }

    private static boolean blankOrZero(String s) { String t = s.trim(); return t.isEmpty() || t.equals("0"); }

    private static int nz(Integer v) { return v == null ? 0 : v; }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

    private static boolean truthy(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim().toLowerCase();
        return s.equals("true") || s.equals("1") || s.equals("yes");
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static long asLong(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return Long.parseLong(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
