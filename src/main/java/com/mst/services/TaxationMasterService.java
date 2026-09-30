package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.TaxationMasterRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The BLL side of the four Taxation master-data forms (Architecture.WinApp.Taxation):
 *
 *   178 InvfrmAddTaxType                     "Add Tax Type"
 *   177 InvfrmAddTaxSchedule                 "Add Tax Schedule"
 *   172 frmTaxNotesAndGLMaping               "Tax Notes And GL Maping"
 *   173 frmSupplierCustomerExemptionSchedule "Supplier Customer Exemption Schedule"
 *
 * Rights come from the real chain through DesktopReportRights (CompanyRights + tblUserRights /
 * ScreenRights by the screen's own dbo.ScreenDefinition.Id): View to load, Save to insert, Update to edit.
 * Each page runs the desktop's FormValidation with the desktop's messages and order; the same checks are
 * repeated here so a procedure is never reached with what the form would have refused. Organisation,
 * company and the audit user come from the session, never from the request (the BLLs read
 * clsGlobalVariables.UserAccount; the forms read their own UserAccount - same thing).
 *
 * Save answers with what the desktop's MessageBox shows: the Insert's new Id, or the record's own Id for
 * an Update.
 */
@Service
public class TaxationMasterService {

    public static final int SCREEN_ADD_TAX_TYPE = 178;
    public static final int SCREEN_ADD_TAX_SCHEDULE = 177;
    public static final int SCREEN_TAX_NOTES_GL_MAPING = 172;
    public static final int SCREEN_SUPPLIER_CUSTOMER_EXEMPTION = 173;

    @Autowired private TaxationMasterRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screen, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen, action);
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private Map<String, Object> permissions(UserAccount u, int screen) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Save", allowed(u, screen, "Save"));
        p.put("Update", allowed(u, screen, "Update"));
        return p;
    }

    // ================================================================= 178 Add Tax Type

    /**
     * AcfrmAddTaxType_Load: CmbTaxTypeFill (a fixed two-row table: 1 WithHoldingTax, 2 SalesTax, first row
     * active), CmbAccountFill (COAAllocation.GetLst minus AccountTypeId 2, 11, 15), DataGridAddTaxTypeFill.
     */
    public Map<String, Object> taxTypeSetup() {
        UserAccount u = user(SCREEN_ADD_TAX_TYPE, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> types = new ArrayList<>();
        types.add(pair("Id", 1, "TaxType", "WithHoldingTax"));
        types.add(pair("Id", 2, "TaxType", "SalesTax"));
        out.put("types", types);
        try { out.put("accounts", taxTypeAccounts(u)); }
        catch (Exception e) { out.put("accountsError", msg(e)); }
        try { out.put("taxTypes", repo.taxTypes(u)); }
        catch (Exception e) { out.put("taxTypesError", msg(e)); }
        out.put("permissions", permissions(u, SCREEN_ADD_TAX_TYPE));
        return out;
    }

    /** CmbAccountFill :  every COAAllocationSearch row whose AccountTypeId is not 2, 11 or 15. */
    private List<Map<String, Object>> taxTypeAccounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.coaAllocationSearch(u)) {
            int t = asInt(ci(r, "AccountTypeId"));
            if (t != 2 && t != 11 && t != 15) out.add(pair("Id", ci(r, "Id"), "AccountTitle", ci(r, "AccountTitle")));
        }
        return out;
    }

    /** DataGridAddTaxTypeFill - TaxesTypes.Getall. */
    public List<Map<String, Object>> taxTypes() {
        return repo.taxTypes(user(SCREEN_ADD_TAX_TYPE, "View"));
    }

    /** GridAddTaxType_DoubleClick - TaxesTypes.GetByID, one of this company's own rows. */
    public List<Map<String, Object>> taxType(int id) {
        UserAccount u = user(SCREEN_ADD_TAX_TYPE, "View");
        if (id <= 0 || !owns(repo.taxTypes(u), id)) throw new IllegalArgumentException("Record not found.");
        return repo.taxTypeById(id);
    }

    /**
     * Insert() : FormValidation ("Name Field Required", "Type Field Required", "Account Field Required"),
     * then TaxesTypes.Save { TaxGLAccountId, Type, TaxName = TaxDescription = txtName.Text.Trim() } with the
     * BLL's audit stamp (EntryDate/User, ModifyDate/User, OrganizationId, CompanyId).
     */
    public Map<String, Object> saveTaxType(Map<String, Object> body) {
        int id = asInt(body.get("id"));
        UserAccount u = user(SCREEN_ADD_TAX_TYPE, id > 0 ? "Update" : "Save");
        String name = text(body.get("name"));
        int type = asInt(body.get("type"));
        int account = asInt(body.get("accountId"));
        if (name.isEmpty()) throw new IllegalArgumentException("Name Field Required");
        if (type == 0) throw new IllegalArgumentException("Type Field Required");
        if (account == 0) throw new IllegalArgumentException("Account Field Required");
        if (type != 1 && type != 2) throw new IllegalArgumentException("Type Field Required");
        if (!owns(taxTypeAccounts(u), account)) throw new IllegalArgumentException("Account Field Required");
        if (id > 0 && !owns(repo.taxTypes(u), id)) throw new IllegalArgumentException("Record not found.");
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@CompanyId", u.getCompanyId());
        m.put("@EntryDate", now);
        m.put("@EntryUser", u.getId());
        m.put("@Id", id);
        m.put("@ModifyDate", now);
        m.put("@ModifyUser", u.getId());
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@TaxDescription", name);
        m.put("@TaxGLAccountId", account);
        m.put("@TaxName", name);
        m.put("@Type", type);
        return result(repo.saveTaxType(m));
    }

    // ================================================================= 177 Add Tax Schedule

    /** InvfrmAddTaxSchedule_Load: CmbTaxTypeFill (TaxesTypes.Getall -> Id, TaxDescription) and HistoryGridFill. */
    public Map<String, Object> taxScheduleSetup() {
        UserAccount u = user(SCREEN_ADD_TAX_SCHEDULE, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("taxTypes", repo.taxTypes(u)); }
        catch (Exception e) { out.put("taxTypesError", msg(e)); }
        try { out.put("schedules", repo.taxSchedules(u)); }
        catch (Exception e) { out.put("schedulesError", msg(e)); }
        out.put("permissions", permissions(u, SCREEN_ADD_TAX_SCHEDULE));
        return out;
    }

    /** HistoryGridFill - TaxScheduleMain.GetAll. */
    public List<Map<String, Object>> taxSchedules() {
        return repo.taxSchedules(user(SCREEN_ADD_TAX_SCHEDULE, "View"));
    }

    /** HistoryGrd_DoubleClick_1 - TaxScheduleMain.GetByID, one of this company's own rows. */
    public List<Map<String, Object>> taxSchedule(int id) {
        UserAccount u = user(SCREEN_ADD_TAX_SCHEDULE, "View");
        if (id <= 0 || !owns(repo.taxSchedules(u), id)) throw new IllegalArgumentException("Record not found.");
        return repo.taxScheduleById(id);
    }

    /**
     * BtnSave_Click / BtnEdit_Click : FormValidation ("Tax Type Field Required", "Tax Percent cannot be greater
     * than 100", "Tax Percent Field Required" - in that order), then TaxScheduleMain.Save with the form's own
     * stamps: Save sets EntryDate/EntryUser, Update sets ModifyDate/ModifyUser; both set PostDate/PostUser now
     * and PostState from the Is Active box.
     *
     * The desktop's Update path leaves EntryDate/EntryUser at the model's defaults (null / 0) and the Update
     * procedure writes them, so every desktop edit blanks the row's EntryDate. This port sends the row's own
     * EntryDate/EntryUser back on an update - nothing shown changes, the original entry stamp survives.
     */
    public Map<String, Object> saveTaxSchedule(Map<String, Object> body) {
        int id = asInt(body.get("id"));
        UserAccount u = user(SCREEN_ADD_TAX_SCHEDULE, id > 0 ? "Update" : "Save");
        int taxNameId = asInt(body.get("taxNameId"));
        String percentText = text(body.get("taxPercent"));
        double percent = asDouble(percentText);
        LocalDate effected = asDate(body.get("effectedDate"));
        boolean active = asBool(body.get("isActive"));
        if (taxNameId == 0) throw new IllegalArgumentException("Tax Type Field Required");
        if (percent > 100.0) throw new IllegalArgumentException("Tax Percent cannot be greater than 100");
        if (percentText.isEmpty() || "0".equals(percentText)) throw new IllegalArgumentException("Tax Percent Field Required");
        if (effected == null) throw new IllegalArgumentException("Effected Date Field Required");
        if (!owns(repo.taxTypes(u), taxNameId)) throw new IllegalArgumentException("Tax Type Field Required");
        Object entryDate; Object entryUser;
        if (id > 0) {
            if (!owns(repo.taxSchedules(u), id)) throw new IllegalArgumentException("Record not found.");
            List<Map<String, Object>> cur = repo.taxScheduleById(id);
            entryDate = cur.isEmpty() ? null : asTimestamp(ci(cur.get(0), "EntryDate"));
            entryUser = cur.isEmpty() ? 0 : asInt(ci(cur.get(0), "EntryUser"));
        } else {
            entryDate = new Timestamp(System.currentTimeMillis());
            entryUser = u.getId();
        }
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@TaxNameId", taxNameId);
        m.put("@EffectedDate", java.sql.Date.valueOf(effected));
        m.put("@EntryDate", entryDate);
        m.put("@EntryUser", entryUser);
        m.put("@ModifyDate", id > 0 ? now : null);
        m.put("@ModifyUser", id > 0 ? u.getId() : 0);
        m.put("@PostDate", now);
        m.put("@PostUser", u.getId());
        m.put("@PostState", active);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@TaxPercent", percent);
        return result(repo.saveTaxSchedule(m));
    }

    // ================================================================= 172 Tax Notes and GL Maping

    /**
     * frmTaxNotesAndGLMaping_Load: GridBind, SaleTaxFill (TaxLookUps type 1), InputOutputFill (type 2),
     * GLAccountFill (CoaAllocationGetForComboServiceBind).
     */
    public Map<String, Object> glMapingSetup() {
        UserAccount u = user(SCREEN_TAX_NOTES_GL_MAPING, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("mapings", repo.glMapings(u)); }
        catch (Exception e) { out.put("mapingsError", msg(e)); }
        try { out.put("saleTaxNotes", repo.taxLookUps(u, 1)); }
        catch (Exception e) { out.put("saleTaxNotesError", msg(e)); }
        try { out.put("inputOutput", repo.taxLookUps(u, 2)); }
        catch (Exception e) { out.put("inputOutputError", msg(e)); }
        try { out.put("glAccounts", repo.coaForCombo(u)); }
        catch (Exception e) { out.put("glAccountsError", msg(e)); }
        out.put("permissions", permissions(u, SCREEN_TAX_NOTES_GL_MAPING));
        return out;
    }

    /** GridBind - TaxSalesNotesGLMaping.Getall. */
    public List<Map<String, Object>> glMapings() {
        return repo.glMapings(user(SCREEN_TAX_NOTES_GL_MAPING, "View"));
    }

    /**
     * RetrivedData - TaxSalesNotesGLMaping.GetByID. The procedure's ReadById is "where Id = Id" (every row),
     * so the desktop loads the table's first row whichever was double-clicked. The row with the requested
     * Id is taken from this company's own list instead.
     */
    public List<Map<String, Object>> glMaping(int id) {
        UserAccount u = user(SCREEN_TAX_NOTES_GL_MAPING, "View");
        if (id <= 0) throw new IllegalArgumentException("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.glMapings(u)) if (asInt(ci(r, "Id")) == id) { out.add(r); break; }
        if (out.isEmpty()) throw new IllegalArgumentException("Record not found.");
        return out;
    }

    /**
     * Insert() : FormValidation ("Sale Tax Notes Required", "Input / Output Required", "GL Account Required"),
     * then TaxSalesNotesGLMaping.Save { Id, TaxLookUpsId, LookUpsTaxInOutId, GLAccountId } - the BLL stamps
     * OrganizationId, CompanyId, EntryUserId, ModifyUserId, EntryDate, ModifyDate; BranchId, ProjectId and
     * SortNo stay 0 (the form never sets them; the Insert procedure computes SortNo itself).
     */
    public Map<String, Object> saveGlMaping(Map<String, Object> body) {
        int id = asInt(body.get("id"));
        UserAccount u = user(SCREEN_TAX_NOTES_GL_MAPING, id > 0 ? "Update" : "Save");
        int notes = asInt(body.get("taxLookUpsId"));
        int inOut = asInt(body.get("lookUpsTaxInOutId"));
        int account = asInt(body.get("glAccountId"));
        if (notes == 0) throw new IllegalArgumentException("Sale Tax Notes Required");
        if (inOut == 0) throw new IllegalArgumentException("Input / Output Required");
        if (account == 0) throw new IllegalArgumentException("GL Account Required");
        if (!owns(repo.taxLookUps(u, 1), notes)) throw new IllegalArgumentException("Sale Tax Notes Required");
        if (!owns(repo.taxLookUps(u, 2), inOut)) throw new IllegalArgumentException("Input / Output Required");
        if (!owns(repo.coaForCombo(u), account)) throw new IllegalArgumentException("GL Account Required");
        if (id > 0 && !owns(repo.glMapings(u), id)) throw new IllegalArgumentException("Record not found.");
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@BranchId", 0);
        m.put("@CompanyId", u.getCompanyId());
        m.put("@EntryDate", now);
        m.put("@EntryUserId", u.getId());
        m.put("@GLAccountId", account);
        m.put("@Id", id);
        m.put("@LookUpsTaxInOutId", inOut);
        m.put("@ModifyDate", now);
        m.put("@ModifyUserId", u.getId());
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@ProjectId", 0);
        m.put("@SortNo", 0);
        m.put("@TaxLookUpsId", notes);
        return result(repo.saveGlMaping(m));
    }

    // ================================================================= 173 Supplier Customer Tax Exemption

    /** frmSupplierCustomerExemptionSchedule_Load: SupplierCustomerFill, TaxTypeFill, GridBind. */
    public Map<String, Object> exemptionSetup() {
        UserAccount u = user(SCREEN_SUPPLIER_CUSTOMER_EXEMPTION, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<Map<String, Object>> list = new ArrayList<>();
            for (Map<String, Object> r : repo.supplierCustomers(u)) list.add(pair("Id", ci(r, "Id"), "CustomerName", ci(r, "CompanyName")));
            out.put("supplierCustomers", list);
        } catch (Exception e) { out.put("supplierCustomersError", msg(e)); }
        try { out.put("taxTypes", repo.taxTypes(u)); }
        catch (Exception e) { out.put("taxTypesError", msg(e)); }
        try { out.put("schedules", repo.exemptions(u)); }
        catch (Exception e) { out.put("schedulesError", msg(e)); }
        out.put("permissions", permissions(u, SCREEN_SUPPLIER_CUSTOMER_EXEMPTION));
        return out;
    }

    /** GridBind - SupplierCustomerExemptionSchedule.Getall. */
    public List<Map<String, Object>> exemptions() {
        return repo.exemptions(user(SCREEN_SUPPLIER_CUSTOMER_EXEMPTION, "View"));
    }

    /** RetrivedData - SupplierCustomerExemptionSchedule.GetByID, one of this company's own rows. */
    public List<Map<String, Object>> exemption(int id) {
        UserAccount u = user(SCREEN_SUPPLIER_CUSTOMER_EXEMPTION, "View");
        if (id <= 0 || !owns(repo.exemptions(u), id)) throw new IllegalArgumentException("Record not found.");
        return repo.exemptionById(id);
    }

    /**
     * Insert() : FormValidation ("Supplier Customer Required", "Tax Type Required", "Tax Percent must be less
     * than 100" - the desktop converts the TextBox object itself, which is always 0, so that third check never
     * fires there; the typed value is checked here, and the procedure refuses anything outside 0..100 anyway).
     * TaxPrcnt is Conversion.ToInt of the text - a whole number, as the model's int property. FromDate/ToDate
     * from the two pickers. Organisation, company, IsActive = 1 and the audit stamp go with it (see the
     * repository note on the newer procedures).
     */
    public Map<String, Object> saveExemption(Map<String, Object> body) {
        int id = asInt(body.get("id"));
        UserAccount u = user(SCREEN_SUPPLIER_CUSTOMER_EXEMPTION, id > 0 ? "Update" : "Save");
        int party = asInt(body.get("supplierCustomerId"));
        int taxType = asInt(body.get("taxTypeId"));
        String percentText = text(body.get("taxPrcnt"));
        LocalDate from = asDate(body.get("fromDate"));
        LocalDate to = asDate(body.get("toDate"));
        if (party == 0) throw new IllegalArgumentException("Supplier Customer Required");
        if (taxType == 0) throw new IllegalArgumentException("Tax Type Required");
        if (asDouble(percentText) > 100.0) throw new IllegalArgumentException("Tax Percent must be less than 100");
        if (from == null || to == null) throw new IllegalArgumentException("From Date and To Date are required.");
        if (!owns(repo.supplierCustomers(u), party)) throw new IllegalArgumentException("Supplier Customer Required");
        if (!owns(repo.taxTypes(u), taxType)) throw new IllegalArgumentException("Tax Type Required");
        if (id > 0 && !owns(repo.exemptions(u), id)) throw new IllegalArgumentException("Record not found.");
        int percent = (int) asDouble(percentText);   // Conversion.ToInt(TaxPrcnt.Text)
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@FromDate", java.sql.Date.valueOf(from));
        m.put("@ToDate", java.sql.Date.valueOf(to));
        m.put("@SupplierCustomerId", party);
        m.put("@TaxTypeId", taxType);
        m.put("@TaxPrcnt", (double) percent);
        m.put("@IsActive", true);
        m.put("@EntryDate", now);
        m.put("@EntryUserId", u.getId());
        m.put("@ModifyDate", now);
        m.put("@ModifyUserId", u.getId());
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        return result(repo.saveExemption(m));
    }

    // ================================================================= helpers

    private static boolean owns(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (asInt(ci(r, "Id")) == id) return true;
        return false;
    }

    private static Map<String, Object> pair(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k1, v1);
        m.put(k2, v2);
        return m;
    }

    private static Map<String, Object> result(int n) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", n);
        return out;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim()); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble - unparseable text is 0. */
    private static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    /** yyyy-MM-dd from the page's date input (or a yyyy-MM-ddTHH:mm prefix). */
    private static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) { return null; }
    }

    private static Timestamp asTimestamp(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return (Timestamp) v;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); }
            catch (IllegalArgumentException e2) { return null; }
        }
    }
}
