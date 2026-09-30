package com.mst.services.fixedassets;

import com.mst.models.UserAccount;
import com.mst.models.fixedassets.FaAssetRegister;
import com.mst.repositories.StoreDefineAssetsExtraRepository;
import com.mst.repositories.fixedassets.FaRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Fixed Assets - 353 "Assets Register", Architecture.WinApp.AssetSchema.frmAssetsRegister (base.Name
 * "frmAssetsRegister", ClientSize 1084 x 539). BLL/DAL/procedures: {@link FaRepository}; the account lists
 * (DatatableHelper.GetAccountsFromGlobalByTypeIds) and the depreciation methods
 * (DepreciationMethodSchedule.GetActiveDepreciationMethodSchedule) are the calls StoreDefineAssetsExtraRepository
 * already makes for the same form family - reused, not duplicated.
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 *  1. Load: SetRightsValueInRightsObject - btnSave = Save right, btnUpdate = Update right; combos bound with no
 *     default row and nothing selected.
 *  2. Asset Category Leave (:443): only while Save is visible and enabled the Serial No is regenerated
 *     (GenerateSerailNoByCategoryId); the item list is re-read for the category; Depreciation Method, Rate and
 *     Useable Life come from the category row; the four GL accounts come from the category row when no item is
 *     chosen. Asset Item Leave (:491): the four GL accounts come from the item row. GL combos, method, rate,
 *     useable life, expiry date and serial no are read-only (Enabled=false) on the desktop.
 *  3. Expiry Date = Purchase Date + Useable Life months, recomputed when either changes (CalculateExpiryDate).
 *  4. Validation (FormHelper.ValidateControls, list order and texts): combo -> "X field is required",
 *     String text box -> "X field is required", INT / Double text box -> "X must be a non-zero number".
 *  5. Save / Update confirm "Are you sure to Save?" / "Are you sure to Update?" (page); messages
 *     "Data Save Successfully....  <name>" / "Data Update Successfully....  <name>"; Update with no record:
 *     "Record Not Update because RecId Not Found". assetCode = serialNo = the Serial No box; assetsTypeId "";
 *     branchesId = projectsId = the user's branch; isApproved false; approved / entry / modify = user, now;
 *     brand / manufacturer / vendor ids 0, isDepreciable false; glAccountId = capitalWIPAccountId = Asset GL.
 *  6. Depreciation Method and Rate are VIRTUAL model properties: the desktop never saves them (the procedure
 *     has no such parameters) - reproduced; UomId is not a model property, so an update writes UomId = NULL.
 *  7. History (tab): Show -> FormHistory with the Entry-date or Modify-date bounds (only the checked pickers);
 *     the "Asset Category" filter is read by the form but never sent by the BLL, so it filters nothing -
 *     reproduced. New resets the dates (-3 days / today) and clears the grid; Refresh re-binds the category combos.
 *     Double click / Ctrl+Enter -> ReadById (Asset Category is then disabled).
 *
 * DEVIATIONS (web)
 *  D1. Tenancy from the session; an asset of another organization / company is "Record Not Found".
 *  D2. Combo values are checked against the lists the page was given (LimitToList); a value outside its
 *      list counts as not selected and gets the desktop's "field is required" message. The Asset Item must
 *      be in the chosen category's item list (or be the stored item on an update).
 *  D3. The Serial No box is read-only: an insert takes the serial GenerateSerailNoByCategoryId returns at save
 *      time (the same call the category Leave makes), an update keeps the stored serial - never the request's.
 *  D4. Pictures (Asset Images, Browse): the desktop File.Copy()s the picked file to configuration "Attachment
 *      Folder Path" on the client's share and stores that Windows path. Not available in the browser: insert
 *      sends pic1Path = pic2Path = "" (what the desktop sends when nothing is browsed), update keeps the stored
 *      paths (the desktop keeps them when the file exists).
 *  D5. On ReadById the item list is loaded for the record's category so the stored item is visible (the
 *      desktop only fills the item list on a category Leave).
 */
@Service
public class FaAssetsRegisterService {

    public static final int SCREEN_ID = 353;
    static final int[] ASSET_ACCOUNT_TYPES = {1};
    static final int[] EXPENSE_ACCOUNT_TYPES = {11, 12, 14, 20, 21, 23};

    @Autowired private FaRepository repo;
    @Autowired private StoreDefineAssetsExtraRepository accounts;
    @Autowired private HrmSupport hrm;

    // ================================================================================ lookups

    /** frmAssetsRegister_Load:281 (also BtnRefresh_Click:834). */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> m = lookups(u);
        m.put("rights", hrm.rights(u, SCREEN_ID));
        return m;
    }

    private Map<String, Object> lookups(UserAccount u) {
        return map("categories", categories(u),
                "assetAccounts", accounts.accountsByTypes(u, ASSET_ACCOUNT_TYPES),
                "expenseAccounts", accounts.accountsByTypes(u, EXPENSE_ACCOUNT_TYPES),
                "statuses", statuses(),
                "departments", departments(u),
                "conditions", conditions(),
                "methods", accounts.depreciationMethods(u));
    }

    private List<Map<String, Object>> categories(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.categories(u)) {
            out.add(map("Id", toInt(r.get("Id")), "categoryDescription", str(r.get("categoryDescription")),
                    "categoryCode", str(r.get("categoryCode")), "GLAccountId", toInt(r.get("GLAccountId")),
                    "accumulatedAccountId", toInt(r.get("accumulatedAccountId")),
                    "depriciationAccountId", toInt(r.get("depriciationAccountId")),
                    "expenseAccountId", toInt(r.get("expenseAccountId")),
                    "depreciationMethodScheduleId", toInt(r.get("depreciationMethodScheduleId")),
                    "usefullLifeInMonths", str(r.get("usefullLifeInMonths")),
                    "depriciationrate", decText(r.get("depriciationrate"))));
        }
        return out;
    }

    private List<Map<String, Object>> departments(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.departments(u)) out.add(map("Id", toInt(r.get("Id")), "DepartmentName", str(r.get("DepartmentName"))));
        return out;
    }

    private List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.statuses()) out.add(map("Id", toInt(r.get("Id")), "StatusName", str(r.get("StatusName"))));
        return out;
    }

    private List<Map<String, Object>> conditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.conditions()) out.add(map("Id", toInt(r.get("Id")), "ConditionName", str(r.get("ConditionName"))));
        return out;
    }

    /** AssetItemFill(categoryId):333 - Id, ItemName, ItemCode, GLAccountId (CapitalWipAcId), the three other GL ids. */
    public List<Map<String, Object>> items(int categoryId) {
        return items(hrm.user(SCREEN_ID), categoryId);
    }

    private List<Map<String, Object>> items(UserAccount u, int categoryId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.assetItems(u, categoryId)) {
            out.add(map("Id", toInt(r.get("Id")), "ItemName", str(r.get("ItemName")), "ItemCode", str(r.get("ItemCode")),
                    "GLAccountId", toInt(r.get("GLAccountId")), "accumulatedDepreciationAcId", toInt(r.get("accumulatedDepreciationAcId")),
                    "DepreciationExpenseAcId", toInt(r.get("DepreciationExpenseAcId")),
                    "ExpenseMaintenanceAccountId", toInt(r.get("ExpenseMaintenanceAccountId"))));
        }
        return out;
    }

    /** CmbAssetCategory_Leave:461 - only called by the page while Save is visible and enabled. */
    public Map<String, Object> serialNo(int categoryId) {
        UserAccount u = hrm.user(SCREEN_ID);
        if (!owns(repo.categories(u), "Id", categoryId)) return map("SerialNo", null);
        return map("SerialNo", repo.generateSerialNo(u, categoryId));
    }

    // ================================================================================ history

    /**
     * Gridbind():907. dateType "entry" (rdentrydate) or "modify" (rdmodifydate); a bound is sent only when its
     * picker is checked (the page sends it then). Columns as the form's dt.
     */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate) {
        UserAccount u = hrm.user(SCREEN_ID);
        LocalDateTime f = toDate(fromDate), t = toDate(toDate);
        boolean modify = "modify".equals(dateType);
        List<Map<String, Object>> raw = modify ? repo.history(u, null, null, ts(f), ts(t)) : repo.history(u, ts(f), ts(t), null, null);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> i : raw) {
            out.add(map("Id", toInt(i.get("assetRegisterId")), "CategoryId", toInt(i.get("categoryId")),
                    "CategoryDescription", str(i.get("categoryDescription")), "CategoryCode", str(i.get("categoryCode")),
                    "SerialNo", str(i.get("serialNo")), "AssetName", str(i.get("assetName")), "ItemId", toInt(i.get("itemId")),
                    "ItemName", str(i.get("ItemName")), "ItemCode", str(i.get("ItemCode")), "MakeDesc", str(i.get("makeDesc")),
                    "ModelDesc", str(i.get("modelDesc")), "PurchaseDate", i.get("purchaseDate"), "PurchasePrice", i.get("purchasePrice"),
                    "UseableLifeMonth", toInt(i.get("useableLifeMonth")), "ExpiryDate", i.get("expiryDate"),
                    "CurrentValue", i.get("currentValue"), "AssetStatus", str(i.get("assetStatus")),
                    "DepartmentName", str(i.get("DepartmentName")), "AssetCondition", str(i.get("AssetCondition")),
                    "AccumulatedDepreciationAcId", toInt(i.get("accumulateddepriciationAcId")),
                    "AccumulatedDepreciationAccount", str(i.get("AccumulatedDepreciationAccount")),
                    "DepreciationExpenseAcId", toInt(i.get("depriciationExpenseAcId")),
                    "DepreciationExpenseAccount", str(i.get("DepreciationExpenseAccount")),
                    "CapitalWIPAccountId", toInt(i.get("capitalWIPAccountId")), "GlAccountAccount", str(i.get("GlAccountAccount")),
                    "ExpenseMaintenanceAccount", str(i.get("ExpenseMaintenanceAccount")), "EntryDate", i.get("entryDate"),
                    "EntryUserName", str(i.get("EntryUserName")), "LastModifyDate", i.get("LastModifyDate"),
                    "LastModifyUserName", str(i.get("LastModifyUserName"))));
        }
        return out;
    }

    // =============================================================================== read

    /** ReadById(Id):685 - AssetRegister.GetById. */
    public Map<String, Object> asset(long id) {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> r = owned(u, id);
        int categoryId = toInt(r.get("categoryId"));
        return map("assetRegisterId", toLong(r.get("assetRegisterId")), "categoryId", categoryId, "itemId", toLong(r.get("itemId")),
                "brandName", str(r.get("brandName")), "assetName", str(r.get("assetName")), "modelDesc", str(r.get("modelDesc")),
                "conditionId", toInt(r.get("conditionId")), "serialNo", str(r.get("serialNo")), "departmentId", toInt(r.get("departmentId")),
                "manufacturerName", str(r.get("manufacturerName")), "makeDesc", str(r.get("makeDesc")),
                "locationBranch", str(r.get("locationBranch")), "vendorName", str(r.get("vendorName")),
                "purchaseDate", r.get("purchaseDate"), "purchasePrice", netDouble(r.get("purchasePrice")),
                "useableLifeMonth", String.valueOf(toInt(r.get("useableLifeMonth"))), "expiryDate", r.get("expiryDate"),
                "currentValue", netDouble(r.get("currentValue")), "depreciationMethodScheduleId", toInt(r.get("depreciationMethodScheduleId")),
                "depriciationrate", decText(r.get("depriciationrate")), "assetStatus", str(r.get("assetStatus")),
                "accumulateddepriciationAcId", toInt(r.get("accumulateddepriciationAcId")),
                "depriciationExpenseAcId", toInt(r.get("depriciationExpenseAcId")), "glAccountId", toInt(r.get("glAccountId")),
                "ExpenseMaintenanceAccountId", toInt(r.get("ExpenseMaintenanceAccountId")),
                "pic1Path", str(r.get("pic1Path")), "pic2Path", str(r.get("pic2Path")),
                "items", items(u, categoryId));
    }

    private Map<String, Object> owned(UserAccount u, long id) {
        List<Map<String, Object>> rows = repo.assetById(id);
        if (rows.isEmpty()) throw invalid("Record Not Found");
        Map<String, Object> r = rows.get(0);
        if (toInt(r.get("organizationId")) != nz(u.getOrganizationId()) || toInt(r.get("companyId")) != nz(u.getCompanyId())) {
            throw invalid("Record Not Found");
        }
        return r;
    }

    // =============================================================================== save

    /** btnSave_Click:656 (id 0) / btnUpdate_Click:669 -> Insert():559 -> AssetRegister.Save. */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = hrm.user(SCREEN_ID);
        long recId = toLong(b.get("id"));
        boolean update = "update".equals(str(b.get("mode")));
        if (update) {
            if (recId == 0) throw invalid("Record Not Update because RecId Not Found");
            hrm.require(u, SCREEN_ID, "Update");
        } else {
            recId = 0;
            hrm.require(u, SCREEN_ID, "Save");
        }
        Map<String, Object> stored = recId > 0 ? owned(u, recId) : null;

        List<Map<String, Object>> cats = repo.categories(u);
        List<Map<String, Object>> assetAcc = accounts.accountsByTypes(u, ASSET_ACCOUNT_TYPES);
        List<Map<String, Object>> expenseAcc = accounts.accountsByTypes(u, EXPENSE_ACCOUNT_TYPES);
        int category = inList(cats, "Id", toInt(b.get("categoryId")));
        if (stored != null) category = toInt(stored.get("categoryId"));            // the combo is disabled on update
        String assetName = str(b.get("assetName"));
        int condition = inList(repo.conditions(), "Id", toInt(b.get("conditionId")));
        int department = inList(repo.departments(u), "Id", toInt(b.get("departmentId")));
        String purchasePrice = str(b.get("purchasePrice"));
        String useableLife = str(b.get("useableLifeMonth"));
        String currentValue = str(b.get("currentValue"));
        int method = inList(accounts.depreciationMethods(u), "depreciationMethodScheduleId", toInt(b.get("depreciationMethodScheduleId")));
        String rate = str(b.get("depriciationrate"));
        String status = str(b.get("assetStatus"));
        boolean statusOk = false;
        for (Map<String, Object> s : repo.statuses()) if (str(s.get("StatusName")).equals(status) && !status.isEmpty()) statusOk = true;
        int glAcc = inList(assetAcc, "Id", toInt(b.get("glAccountId")));
        int deprAcc = inList(expenseAcc, "Id", toInt(b.get("depriciationExpenseAcId")));
        int accumAcc = inList(assetAcc, "Id", toInt(b.get("accumulateddepriciationAcId")));
        int expMaint = inList(expenseAcc, "Id", toInt(b.get("ExpenseMaintenanceAccountId")));

        /* Serial No (D3) */
        String serial = stored != null ? str(stored.get("serialNo")) : (category > 0 ? repo.generateSerialNo(u, category) : null);

        /* FormValidation():537 - FormHelper.ValidateControls, same order, same texts. */
        if (category == 0) throw invalid("Asset Category field is required");
        if (assetName.trim().isEmpty()) throw invalid("Asset Name field is required");
        if (condition == 0) throw invalid("Asset Condition field is required");
        if (serial == null || serial.trim().isEmpty()) throw invalid("Asset Serial No field is required");
        if (department == 0) throw invalid("Asset Department field is required");
        if (!nonZeroDouble(purchasePrice)) throw invalid("Purchase Price must be a non-zero number");
        if (!nonZeroInt(useableLife)) throw invalid("Useable Life Month must be a non-zero number");
        if (!nonZeroDouble(currentValue)) throw invalid("Current Value must be a non-zero number");
        if (method == 0) throw invalid("Depreciation Method field is required");
        if (!nonZeroDouble(rate)) throw invalid("Depreciation Rate must be a non-zero number");
        if (!statusOk) throw invalid("Asset Status field is required");
        if (glAcc == 0) throw invalid("Asset GL A/c field is required");
        if (deprAcc == 0) throw invalid("Depreciation_Expense A/c field is required");
        if (accumAcc == 0) throw invalid("Accumulated_Depreciation A/c field is required");
        if (expMaint == 0) throw invalid("Expense Maintenance A/c field is required");

        /* Asset Item (optional on the desktop; D2) */
        long itemId = toLong(b.get("itemId"));
        if (itemId != 0) {
            boolean ok = owns(items(u, category), "Id", (int) itemId) || (stored != null && toLong(stored.get("itemId")) == itemId);
            if (!ok) throw invalid("Asset Item not found in the selected Asset Category");
        }

        LocalDateTime now = LocalDateTime.now();
        FaAssetRegister m = new FaAssetRegister();
        if (recId > 0) m.assetRegisterId = recId;
        m.companyId = nz(u.getCompanyId());
        m.organizationId = nz(u.getOrganizationId());
        m.branchesId = nz(u.getBranchesId());
        m.projectsId = nz(u.getBranchesId());
        m.entryUserId = nz(u.getId());
        m.entryDate = now;
        m.LastModifyById = nz(u.getId());
        m.LastModifyDate = now;
        m.approvedDate = now;
        m.approvedUserId = nz(u.getId());
        m.isApproved = false;
        m.categoryId = category;
        m.itemId = itemId;
        m.brandName = str(b.get("brandName"));
        m.assetName = assetName;
        m.conditionId = condition;
        m.modelDesc = str(b.get("modelDesc"));
        m.assetCode = serial;
        m.serialNo = serial;
        m.departmentId = department;
        m.manufacturerName = str(b.get("manufacturerName"));
        m.makeDesc = str(b.get("makeDesc"));
        m.locationBranch = str(b.get("locationBranch"));
        m.vendorName = str(b.get("vendorName"));
        m.purchaseDate = withTime(toDay(b.get("purchaseDate")), now);
        m.purchasePrice = toDouble(purchasePrice);
        m.useableLifeMonth = toInt(useableLife);
        m.expiryDate = withTime(toDay(b.get("expiryDate")), now);
        m.currentValue = toDouble(currentValue);
        m.assetStatus = status;
        m.assetsTypeId = "";
        m.glAccountId = glAcc;
        m.capitalWIPAccountId = glAcc;
        m.depriciationExpenseAcId = deprAcc;
        m.accumulateddepriciationAcId = accumAcc;
        m.ExpenseMaintenanceAccountId = expMaint;
        m.pic1Path = stored == null ? "" : str(stored.get("pic1Path"));                // D4
        m.pic2Path = stored == null ? "" : str(stored.get("pic2Path"));
        if (m.purchaseDate == null || m.expiryDate == null) throw invalid("Purchase Date is not valid");
        int id = repo.saveAsset(m);
        return saved(id, (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + assetName);
    }

    // ============================================================================== helpers

    private static int inList(List<Map<String, Object>> rows, String key, int id) {
        if (id == 0) return 0;
        return owns(rows, key, id) ? id : 0;
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }

    static long toLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return Long.parseLong(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0L; }
    }

    /** double.TryParse (Float | AllowThousands) and != 0. */
    static boolean nonZeroDouble(String s) {
        if (s == null) return false;
        String t = s.trim().replace(",", "");
        if (t.isEmpty()) return false;
        try { return Double.parseDouble(t) != 0d; } catch (NumberFormatException e) { return false; }
    }

    /** int.TryParse and != 0. */
    static boolean nonZeroInt(String s) {
        if (s == null) return false;
        try { return Integer.parseInt(s.trim()) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** DateTimePicker.Value carries the time of day; the page sends the date. */
    private static LocalDateTime withTime(LocalDateTime day, LocalDateTime now) {
        return day == null ? null : day.toLocalDate().atTime(now.toLocalTime());
    }

    /** decimal.ToString(): the stored text with its scale ("10.00"). */
    private static String decText(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        return String.valueOf(v);
    }

    /** double.ToString(): shortest text ("1500" not "1500.0"). */
    private static String netDouble(Object v) {
        double d = toDouble(v);
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }
}
