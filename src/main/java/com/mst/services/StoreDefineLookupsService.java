package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StoreDefineLookupsDto;
import com.mst.repositories.StoreDefineLookupsRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * The two lookup forms the Store screens open with their green "+" buttons, as modal dialogs:
 *
 *   (a) Architecture.WinApp.Lookups.Define_Department   (base.Name "Define_Department", Define_Department.cs:416)
 *       opened by {@code new Define_Department(UserAccount).Show()} from StoreIssuanceDirect.cs:2335,
 *       frmDepartmentRequest.cs:1551, frmPurchaseDemand.cs:1950, DepartmentRequestToConsumableStore.cs:1382
 *       and frmLookUpDefineAsset.cs:479.  API /api/store/define/department.
 *   (b) Architecture.WinApp.Lookups.frmLookUpDefineAsset (base.Name "frmLookUpDefineAsset", frmLookUpDefineAsset.cs:1470)
 *       opened by {@code new frmLookUpDefineAsset(UserAccount).Show()} from StoreIssuanceDirect.cs:2347,
 *       frmPurchaseDemand.cs:1962, DepartmentRequestToConsumableStore.cs:1394.  API /api/store/define/asset.
 *
 * No DocumentTypeId (master data). No page route — the dialogs are built by
 * static/build/js/countx_store_define_lookups.js (window.StoreDefine.openDepartment / openAsset).
 * BLL/DAL/Model: see {@link StoreDefineLookupsRepository}.
 *
 * RIGHTS. Neither form calls SetRightsValueInRightsObject or reads any right: the desktop lets
 * whoever has the host form open define departments / assets. The DB dump GoldenAceDb(0509)t.sql has
 * NO dbo.ScreenDefinition row for "Define_Department" nor for "frmLookUpDefineAsset" (the only
 * near names are 356 "frmDefineAssets" and 638 "Department" = HRM genDepartment, different forms).
 * Server side (StoreScreenRights, never cached), an action is allowed when ANY of:
 *   - the role is Admin (CommonServices.cs:17581 short-circuit);
 *   - the user has "View" on the HOST screen that opened the dialog (whitelisted ScreenNames below) —
 *     the desktop's only gate is being able to open that host form;
 *   - the user has the matching right on the dialog's own ScreenName (view for reads, save / update
 *     for writes) — honoured in case a ScreenDefinition row is added later.
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *   1. Define_Department: no delete, no duplicate-name check; the only validation is
 *      "Department Name Field Required" on Trim() == "" (FormValidation:83); the name is saved untrimmed
 *      (txtDepartmentName is Multiline — Enter inserts a line break into the name).
 *   2. Define_Department update message is "Record Update Successfully...[0]": Sp_Department_Update
 *      returns no row, so SetProc's Convert.ToInt32(ExecuteScalar()) is 0 (btnUpdate_Click:111).
 *   3. Sp_Department_Insert / _Update receive EntryDate = ModifyDate = PostDate = now, EntryUser = user,
 *      ModifyUser = 0 (never set), PostUser = 0, PostState = false; the update proc writes
 *      ModifyUser = 0 (the modifying user is not recorded).
 *   4. Sp_Department_Insert computes Id = MAX(Id)+1 over the WHOLE table (all companies).
 *   5. frmLookUpDefineAsset: only "Asset Name Field Required" (FormValidation:245); Category, Item and
 *      Department may be empty (sent as 0). Confirm "Are you sure to Save?" / "Are you sure to Update?"
 *      before saving (client side). Messages "Data Save Successfully....  <AssetName>" /
 *      "Data Update Successfully....  <AssetName>".
 *   6. Asset save sends BranchesId = ProjectsId = user's branch, EntryUserId = ApprovedUserId = user,
 *      EntryDate = ApprovedDate = PurchaseDate = ExpiryDate = now, IsApproved = false, AssetsType = "",
 *      and 0 for AssetGLAccountId, AssetDepriciationAcId, AssetAcmltvAcId, ExpenseMaintenanceAccountId,
 *      UseableLifeMonth, PurchasePrice, CurrentValue; Vendor, Manufacturer, AssetSerialNo, AssetStatus,
 *      MakeDesc, Pic1Path, Pic2Path are null → not sent → NULL. On UPDATE this OVERWRITES whatever the
 *      full Fixed Asset screen stored in those columns (GL accounts, price, serial, pictures, entry
 *      user/date) — reproduced as the desktop does it.
 *   7. Asset update with RecId 0 → "Record Not Update because RecId Not Found" (btnUpdate_Click:328).
 *   8. AssetLocationId is NCHAR(10): longer text is cut by the parameter; read back it is space padded.
 *   9. Asset history: Entry Date radio → @EntryFromDate/@EntryToDate, Modify Date radio →
 *      @ModifyFromDate/@ModifyToDate (the proc compares these with ApprovedDate), each only when its
 *      picker is checked; @CategoryId only when != 0 (Gridbind:512).
 *  10. Asset ReadById keeps the previous combo values when the record's Category / Item / Department is 0
 *      (ReadById:394 "if (SC.X > 0)") — client side.
 *
 * DEVIATIONS (web only, with reason)
 *   D1. Tenancy: Sp_Department_Update / Sp_FixedAssetsRegister_Update and both ReadById branches filter
 *       by Id only. Here open / update refuse a row that is not the session's organization + company
 *       ("Record Not Found"). Org / company / branch / user always come from the session.
 *   D2. Rights are checked server side as described above (the desktop checks none).
 *   D3. Validation is repeated server side with the same messages.
 */
@Service
public class StoreDefineLookupsService {

    public static final String SCREEN_DEPARTMENT = "Define_Department";
    public static final String SCREEN_ASSET = "frmLookUpDefineAsset";

    /** ScreenNames of the store forms that open these dialogs on the desktop. */
    public static final Set<String> HOSTS = Collections.unmodifiableSet(new LinkedHashSet<>(Arrays.asList(
            "StoreIssuanceDirect", "frmDepartmentRequest", "frmPurchaseDemand", "DepartmentRequestToConsumableStore")));

    private final StoreDefineLookupsRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StoreDefineLookupsService(StoreDefineLookupsRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================== rights

    private void require(String ownScreen, String host, String right) {
        if ("Admin".equals(ctx.currentRoleName())) return;
        if (host != null && HOSTS.contains(host) && rights.has(host, "view")) return;
        if (rights.has(ownScreen, right)) return;
        throw new IllegalStateException("You do not have the rights to use this screen.");
    }

    // ============================================================================== Department

    /** Define_Department_Load:59 / Reset:64 → DepartmentDefineGridFill:93 — Id (hidden), DepartmentName. */
    public List<Map<String, Object>> departmentList(String host) {
        require(SCREEN_DEPARTMENT, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.departments(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("DepartmentName", ci(r, "DepartmentName"));
            out.add(o);
        }
        return out;
    }

    /** grdDefineCity_DoubleClick:196 → Department.GetByID(RecId) → txtDepartmentName.Text. */
    public Map<String, Object> department(int id, String host) {
        require(SCREEN_DEPARTMENT, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> r = repo.department(id);
        if (r == null || !ownRow(r, u)) return null;                         // D1
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(ci(r, "Id")));
        o.put("DepartmentName", ci(r, "DepartmentName"));
        return o;
    }

    /** btnsave_Click:147 (insert, Id 0) / btnUpdate_Click:111 (update, Id = RecId). */
    @Transactional
    public Map<String, Object> saveDepartment(StoreDefineLookupsDto.Department d, boolean update) {
        String host = d == null ? null : d.host;
        require(SCREEN_DEPARTMENT, host, update ? "update" : "save");
        UserAccount u = ctx.requireAccountingUser();
        String name = d.DepartmentName == null ? "" : d.DepartmentName;
        if (name.trim().isEmpty()) throw new IllegalStateException("Department Name Field Required");   // FormValidation:83
        int id = 0;
        if (update) {
            id = d.Id == null ? 0 : d.Id;
            Map<String, Object> cur = repo.department(id);
            if (cur == null || !ownRow(cur, u)) throw new IllegalStateException("Record Not Found");   // D1
        }
        Timestamp now = new Timestamp(System.currentTimeMillis());
        int result = repo.saveDepartment(!update, id, name, now, toInt(u.getId()), u);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("result", result);
        o.put("message", (update ? "Record Update Successfully...[" : "Record Save Successfully...[") + result + "]");
        return o;
    }

    // =================================================================================== Asset

    /** Define_Department_Load:141 (the asset form reuses that handler name): ItemCategory(), ItemFill(), Department(). */
    public Map<String, Object> assetLookups(String host) {
        require(SCREEN_ASSET, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cats = new ArrayList<>();                     // ItemCategory:155 → "Id","Category"
        for (Map<String, Object> r : repo.assetCategories(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Category", str(ci(r, "CategoryDescription")));
            cats.add(o);
        }
        out.put("categories", cats);
        List<Map<String, Object>> items = new ArrayList<>();                    // ItemFill:181 → "Id","ItemName"
        for (Map<String, Object> r : repo.assetItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            items.add(o);
        }
        out.put("items", items);
        out.put("departments", assetDepartments(u));                            // Department():194 → "Id","Department"
        return out;
    }

    /** Department():194 alone — used by the web to re-fill cmbDepartment after the nested Define Department closes. */
    public List<Map<String, Object>> assetDepartments(String host) {
        require(SCREEN_ASSET, host, "view");
        return assetDepartments(ctx.requireAccountingUser());
    }

    private List<Map<String, Object>> assetDepartments(UserAccount u) {
        List<Map<String, Object>> deps = new ArrayList<>();
        for (Map<String, Object> r : repo.departments(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Department", str(ci(r, "DepartmentName")));
            deps.add(o);
        }
        return deps;
    }

    /** btnShow_Click:497 → Gridbind:505 → FixedAssetsRegister.GetHistory. dateType "entry" | "modify". */
    public List<Map<String, Object>> assetHistory(String host, String dateType, String fromDate, String toDate, int categoryId) {
        require(SCREEN_ASSET, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        Timestamp from = day(fromDate), to = day(toDate);
        boolean modify = "modify".equalsIgnoreCase(dateType);
        List<Map<String, Object>> rows = repo.assetHistory(u,
                modify ? null : from, modify ? null : to,
                modify ? from : null, modify ? to : null, categoryId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> i : rows) {                                    // Gridbind:546 dt.Rows.Add(...)
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(i, "Id"));
            o.put("Category", ci(i, "AssetsCategory"));
            o.put("AssetItemName", ci(i, "ItemName"));
            o.put("AssetName", ci(i, "AssetName"));
            o.put("Model", ci(i, "ModelDesc"));
            o.put("Brand", ci(i, "Brand"));
            o.put("Department", ci(i, "DepartmentName"));
            o.put("AssetCondition", ci(i, "AssetCondition"));
            o.put("EntryDate", iso(ci(i, "EntryDate")));
            o.put("EntryUser", ci(i, "EntryUser"));
            o.put("ApprovedDate", iso(ci(i, "ApprovedDate")));
            o.put("ApprovedUser", ci(i, "ModifyUser"));
            out.add(o);
        }
        return out;
    }

    /** grdDefineCity_DoubleClick:374 → ReadById:386 → FixedAssetsRegister.GetById. */
    public Map<String, Object> asset(int id, String host) {
        require(SCREEN_ASSET, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> r = repo.asset(id);
        if (r == null || !ownRow(r, u)) return null;                         // D1
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(ci(r, "Id")));
        o.put("FixedAssetsCategoryId", toInt(ci(r, "FixedAssetsCategoryId")));
        o.put("ItemId", toInt(ci(r, "ItemId")));
        o.put("Brand", ci(r, "Brand"));
        o.put("AssetName", ci(r, "AssetName"));
        o.put("ModelDesc", ci(r, "ModelDesc"));
        o.put("AssetCondition", ci(r, "AssetCondition"));
        o.put("AssetsDepartmentId", toInt(ci(r, "AssetsDepartmentId")));
        o.put("AssetLocationId", ci(r, "AssetLocationId"));
        return o;
    }

    /** Insert():255 — btnsave_Click:340 (RecId = 0) / btnUpdate_Click:324 (RecId must be > 0). */
    @Transactional
    public Map<String, Object> saveAsset(StoreDefineLookupsDto.Asset a, boolean update) {
        String host = a == null ? null : a.host;
        require(SCREEN_ASSET, host, update ? "update" : "save");
        UserAccount u = ctx.requireAccountingUser();
        int id = update ? (a.Id == null ? 0 : a.Id) : 0;
        if (update && id == 0) throw new IllegalStateException("Record Not Update because RecId Not Found");   // :328
        String name = a.AssetName == null ? "" : a.AssetName;
        if (name.trim().isEmpty()) throw new IllegalStateException("Asset Name Field Required");               // FormValidation:245
        if (update) {
            Map<String, Object> cur = repo.asset(id);
            if (cur == null || !ownRow(cur, u)) throw new IllegalStateException("Record Not Found");         // D1
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("CompanyId", u.getCompanyId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("BranchesId", u.getBranchesId());
        m.put("UserId", u.getId());
        m.put("Now", new Timestamp(System.currentTimeMillis()));
        m.put("FixedAssetsCategoryId", a.FixedAssetsCategoryId == null ? 0 : a.FixedAssetsCategoryId);
        m.put("ItemId", a.ItemId == null ? 0 : a.ItemId);
        m.put("Brand", nz(a.Brand));
        m.put("AssetName", name);
        m.put("ModelDesc", nz(a.ModelDesc));
        m.put("AssetCondition", nz(a.AssetCondition));
        m.put("AssetsDepartmentId", a.AssetsDepartmentId == null ? 0 : a.AssetsDepartmentId);
        m.put("AssetLocationId", nz(a.AssetLocationId));
        int result = repo.saveAsset(!update, m);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("result", result);
        o.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + name);
        return o;
    }

    // ================================================================================= helpers

    private static boolean ownRow(Map<String, Object> r, UserAccount u) {
        return toInt(ci(r, "OrganizationId")) == toInt(u.getOrganizationId())
                && toInt(ci(r, "CompanyId")) == toInt(u.getCompanyId());
    }

    /** TextBox.Text is never null; Conversion.ToString("") is "". */
    private static String nz(String s) { return s == null ? "" : s; }

    private static Timestamp day(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { throw new IllegalStateException("Invalid date: " + s); }
    }

    private static String iso(Object v) {
        if (v instanceof java.util.Date) return new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format((java.util.Date) v);
        if (v instanceof java.time.LocalDateTime) return v.toString();
        return v == null ? null : String.valueOf(v);
    }
}
