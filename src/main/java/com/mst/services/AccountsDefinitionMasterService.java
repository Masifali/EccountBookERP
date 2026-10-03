package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.AccountsDefinitionMasterRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * BLL of the six ported screens of AppModules 45 "Accounts Definition" (App 19): 427 PDC Bank, 429 Document Group,
 * 430 Shipment Documents, 428 Tax Lookup, 414 BsPl Settings Form, 413 Bank. (412 Account Custom Group is the same
 * desktop class as screen 1 and opens that page.)
 *
 * Each save repeats the form's own validation with its wording and order and builds the model as the form and its
 * BLL do. Tenancy and audit come from the session. Rights: only AcfrmDefineBank checks rights on the desktop
 * (SetRightsValueInRightsObject(base.Name) -> Save / Update buttons); every page here still needs View on its
 * ScreenDefinition row, as the menu that opens it does.
 */
@Service
public class AccountsDefinitionMasterService {

    public static final int SCREEN_BANK = 413, SCREEN_BSPL = 414, SCREEN_PDC_BANK = 427, SCREEN_TAX_LOOKUP = 428,
            SCREEN_DOC_GROUP = 429, SCREEN_SHIPMENT_DOCS = 430;

    @Autowired private AccountsDefinitionMasterRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screen) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; } catch (AccessDeniedException ex) { return false; }
    }

    // ================================================================= 427 PDC Bank (PdcBank.cs)

    public List<Map<String, Object>> pdcBanks() { return repo.pdcBanks(user(SCREEN_PDC_BANK)); }

    public Map<String, Object> pdcBank(int id) {
        UserAccount u = user(SCREEN_PDC_BANK);
        if (!owns(repo.pdcBanks(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.pdcBank(id));
    }

    /** Inset() :29 - "Bank Name Field Required"; the four model properties. */
    public Map<String, Object> savePdcBank(Map<String, Object> b) {
        UserAccount u = user(SCREEN_PDC_BANK);
        int id = asInt(b.get("id"));
        String name = raw(b.get("bankName"));
        if (name.trim().isEmpty()) throw new IllegalArgumentException("Bank Name Field Required");
        if (id > 0 && !owns(repo.pdcBanks(u), id)) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@BankName", name);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        return result(repo.savePdcBank(m));
    }

    // ================================================================= 429 Document Group (DocumentGroup.cs)

    public List<Map<String, Object>> docGroups() { return repo.docGroups(user(SCREEN_DOC_GROUP)); }

    public Map<String, Object> docGroup(int id) {
        UserAccount u = user(SCREEN_DOC_GROUP);
        if (!owns(repo.docGroups(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.docGroup(id));
    }

    /**
     * btnSave_Click :27 - no validation. The form sets ONLY code and name on the model, so the desktop inserts
     * OrganizationId = CompanyId = 0 and the grid (which filters by the session's company) never shows the new row.
     * Reproduced as written; recorded in the project doc.
     */
    public Map<String, Object> saveDocGroup(Map<String, Object> b) {
        UserAccount u = user(SCREEN_DOC_GROUP);
        int id = asInt(b.get("id"));
        if (id > 0 && !owns(repo.docGroups(u), id)) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@ExImDocGroupCode", raw(b.get("code")).trim());
        m.put("@ExImDocGroupName", raw(b.get("description")).trim());
        m.put("@Id", id);
        m.put("@CompanyId", 0);
        m.put("@OrganizationId", 0);
        return result(repo.saveDocGroup(m));
    }

    // ================================================================= 430 Shipment Documents (ExImShipmentDocuments.cs)

    public List<Map<String, Object>> shipmentDocs() { user(SCREEN_SHIPMENT_DOCS); return repo.shipmentDocs(); }

    public Map<String, Object> shipmentDoc(int id) {
        user(SCREEN_SHIPMENT_DOCS);
        if (!owns(repo.shipmentDocs(), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.shipmentDoc(id));
    }

    /** btnSave_Click :27 - no validation; the three model properties (the table has no tenancy). */
    public Map<String, Object> saveShipmentDoc(Map<String, Object> b) {
        user(SCREEN_SHIPMENT_DOCS);
        int id = asInt(b.get("id"));
        if (id > 0 && !owns(repo.shipmentDocs(), id)) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@exImDocCode", raw(b.get("code")).trim());
        m.put("@exImDocName", raw(b.get("description")).trim());
        return result(repo.saveShipmentDoc(m));
    }

    // ================================================================= 428 Tax Lookup (TaxLookup.cs)

    /** frmEduLookups_Load - LookupTypeBind, GridBind. */
    public Map<String, Object> taxLookupSetup() {
        UserAccount u = user(SCREEN_TAX_LOOKUP);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("types", repo.taxLookupTypes()); } catch (Exception e) { out.put("typesError", msg(e)); }
        try { out.put("lookups", repo.taxLookups(u)); } catch (Exception e) { out.put("lookupsError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> taxLookups() { return repo.taxLookups(user(SCREEN_TAX_LOOKUP)); }

    public Map<String, Object> taxLookup(int id) {
        UserAccount u = user(SCREEN_TAX_LOOKUP);
        if (!owns(repo.taxLookups(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.taxLookup(id));
    }

    /** cmbProfileName_Leave - the next code for the chosen type (0 = leave the box as it is). */
    public Map<String, Object> taxLookupCode(int typeId) {
        UserAccount u = user(SCREEN_TAX_LOOKUP);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("code", repo.generateTaxLookupCode(u, typeId));
        return out;
    }

    /** btnsave_Click :33 / btnUpdate_Click :46 - the BLL sets Org/Company; ActionId 0; Code = Conversion.ToInt(text). */
    public Map<String, Object> saveTaxLookup(Map<String, Object> b) {
        UserAccount u = user(SCREEN_TAX_LOOKUP);
        int id = asInt(b.get("id"));
        String code = raw(b.get("code")), name = raw(b.get("lookUpName"));
        int typeId = asInt(b.get("taxLookUptypesId"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("Code Field Required");
        if (name.trim().isEmpty()) throw new IllegalArgumentException("LookUpName Field Required");
        if (typeId <= 0 || !owns(repo.taxLookupTypes(), typeId)) throw new IllegalArgumentException("ProfileName Field Required");
        if (id > 0 && !owns(repo.taxLookups(u), id)) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@ActionId", 0);
        m.put("@CompanyId", u.getCompanyId());
        m.put("@Id", id);
        m.put("@LookUpName", name);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@TaxLookUptypesId", typeId);
        m.put("@Code", asInt(code.trim()));
        return result(repo.saveTaxLookup(m));
    }

    // ================================================================= 414 BS & PL Setting (BsPlSettingForm.cs)

    /**
     * GridLoad :150 - "PL" or "BS"; for BS the class must be chosen (2 Assets / 3 Liabilities), else
     * "Please select either the 'Assets' or 'Liabilities' button...". Also returns the value list the grid's
     * NoteTitle column offers (GridSettings: PL notes, or the BS notes of the chosen class).
     */
    public Map<String, Object> bsPlLoad(String req, int accountClassId) {
        UserAccount u = user(SCREEN_BSPL);
        boolean bs = "BS".equals(req);
        if (bs && accountClassId != 2 && accountClassId != 3)
            throw new IllegalArgumentException("Please select either the 'Assets' or 'Liabilities' button...");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", repo.bsPlAccounts(u, bs ? "BS" : "PL", bs ? accountClassId : 0));
        out.put("notes", bs ? repo.bsNotes(accountClassId) : repo.plNotes());
        return out;
    }

    /**
     * btnUpdate_Click :213 - one Sp_PLBSSetting_Insert per changed row (the BLL never updates), all in one transaction:
     * ChartOfAccountId, BSNoteId / PLNoteId by the radio, FinancialYearId = the active year, EntryUserId /
     * ModifyUserId = the USER ROW's EntryUserId / ModifyUserId (as the form reads them), Id 0.
     * The desktop sends rows whose chosen note id is not 0 and differs from Conversion.ToInt(the original title
     * text) - which is always 0 - so every row with a chosen note is sent.
     */
    public Map<String, Object> bsPlUpdate(Map<String, Object> b) {
        UserAccount u = user(SCREEN_BSPL);
        boolean bs = "BS".equals(raw(b.get("reqType")));
        int fy = currentUserContext.currentFinancialYearId();
        LocalDateTime now = LocalDateTime.now();
        List<Map<String, Object>> items = new ArrayList<>();
        Object rowsObj = b.get("rows");
        if (rowsObj instanceof List) {
            for (Object o : (List<?>) rowsObj) {
                if (!(o instanceof Map)) continue;
                Map<?, ?> r = (Map<?, ?>) o;
                int noteId = asInt(r.get("noteId")), coaId = asInt(r.get("chartOfAccountId"));
                if (noteId == 0 || coaId == 0) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("@Id", 0);
                m.put("@ChartOfAccountId", coaId);
                m.put("@BSNoteId", bs ? noteId : 0);
                m.put("@PLNoteId", bs ? 0 : noteId);
                m.put("@OrganizationId", u.getOrganizationId());
                m.put("@CompanyId", u.getCompanyId());
                m.put("@FinancialYearId", fy);
                m.put("@EntryUserId", u.getEntryUserId() == null ? 0 : u.getEntryUserId());
                m.put("@EntryDate", Timestamp.valueOf(now));
                m.put("@ModifyUserId", u.getModifyUserId() == null ? 0 : u.getModifyUserId());
                m.put("@ModifyDate", Timestamp.valueOf(now));
                items.add(m);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", items.size());
        if (!items.isEmpty()) repo.savePlBsSettings(items);
        return out;
    }

    /** GridNoteTitleFill :302 - AccountNotes.ReadAllNotes("" / "PL" / "BS"). */
    public List<Map<String, Object>> bsPlNotes(String note) {
        user(SCREEN_BSPL);
        return repo.allNotes(note);
    }

    /** btnUpdateNote_Click :334 - UpdateTitleById for the rows whose title changed (id != 0). */
    public Map<String, Object> bsPlUpdateNotes(Map<String, Object> b) {
        user(SCREEN_BSPL);
        List<Map<String, Object>> items = new ArrayList<>();
        Object rowsObj = b.get("rows");
        if (rowsObj instanceof List) {
            for (Object o : (List<?>) rowsObj) {
                if (!(o instanceof Map)) continue;
                Map<?, ?> r = (Map<?, ?>) o;
                int id = asInt(r.get("id"));
                if (id == 0) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", id);
                m.put("noteTitle", raw(r.get("noteTitle")));
                items.add(m);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", items.size());
        if (!items.isEmpty()) out.put("result", repo.updateNoteTitles(items));
        return out;
    }

    // ================================================================= 413 Bank (AcfrmDefineBank.cs)

    /** AcfrmDefineBank_Load :35 - rights, CountryTypeFill (fixed), ChequeTempleteFill, CountryComboFill, CityFill, grid, GL accounts. */
    public Map<String, Object> bankSetup() {
        UserAccount u = user(SCREEN_BANK);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", allowed(u, SCREEN_BANK, "Save"));
        out.put("canUpdate", allowed(u, SCREEN_BANK, "Update"));
        out.put("canGridPrint", allowed(u, SCREEN_BANK, "Grid Print"));
        out.put("canGridExport", allowed(u, SCREEN_BANK, "Grid Export"));
        try { out.put("chequeTemplates", repo.chequeTemplates()); } catch (Exception e) { out.put("chequeTemplatesError", msg(e)); }
        try { out.put("countries", repo.countries(u)); } catch (Exception e) { out.put("countriesError", msg(e)); }
        try { out.put("cities", repo.cities(u)); } catch (Exception e) { out.put("citiesError", msg(e)); }
        try { out.put("banks", repo.banks(u)); } catch (Exception e) { out.put("banksError", msg(e)); }
        try { out.put("glAccounts", repo.bankGlAccounts(u)); } catch (Exception e) { out.put("glAccountsError", msg(e)); }
        return out;
    }

    /** btnRefresh_Click - the four combos again. */
    public Map<String, Object> bankCombos() {
        UserAccount u = user(SCREEN_BANK);
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("chequeTemplates", repo.chequeTemplates()); } catch (Exception e) { out.put("chequeTemplatesError", msg(e)); }
        try { out.put("countries", repo.countries(u)); } catch (Exception e) { out.put("countriesError", msg(e)); }
        try { out.put("cities", repo.cities(u)); } catch (Exception e) { out.put("citiesError", msg(e)); }
        try { out.put("glAccounts", repo.bankGlAccounts(u)); } catch (Exception e) { out.put("glAccountsError", msg(e)); }
        return out;
    }

    public List<Map<String, Object>> banks() { return repo.banks(user(SCREEN_BANK)); }

    public Map<String, Object> bank(int id) {
        UserAccount u = user(SCREEN_BANK);
        if (!owns(repo.banks(u), id)) throw new IllegalArgumentException("Record not found.");
        return one(repo.bank(id));
    }

    /**
     * Insert() :150 - FormValidation :95 in the desktop's order, then the 27 Bank properties: IsHomeland and
     * ChequeTemplete are the combos' TEXT, BranchCity / CountryId / ChartOfAccountId their values, EntryDate =
     * ModifyDate = now (BLL), EntryUser = ModifyUser = user, PostDate null (not sent), PostUser 0, PostState false.
     * Save / Update need the screen's own Save / Update right, as the desktop buttons do.
     */
    public Map<String, Object> saveBank(Map<String, Object> b) {
        UserAccount u = user(SCREEN_BANK);
        int id = asInt(b.get("id"));
        String homeland = raw(b.get("isHomelandText")).trim();
        int homelandId = asInt(b.get("isHomelandId"));
        String bankName = raw(b.get("bankName")), branchCode = raw(b.get("branchCode")), accountNo = raw(b.get("bankAccountNo")),
               accountTitle = raw(b.get("bankAccountTitle")), iban = raw(b.get("bankIbanNo")), chequeTemplate = raw(b.get("chequeTemplate"));
        int glId = asInt(b.get("chartOfAccountId")), countryId = asInt(b.get("countryId")), cityId = asInt(b.get("branchCity"));
        if (homeland.isEmpty() || homelandId == 0) throw new IllegalArgumentException("Home Land Name Field Required");
        if (bankName.trim().isEmpty()) throw new IllegalArgumentException("Bank Name Field Required");
        if (branchCode.trim().isEmpty()) throw new IllegalArgumentException("Branch Code Field Required");
        if (accountNo.trim().isEmpty()) throw new IllegalArgumentException("Bank Account No Field Required");
        if (iban.trim().isEmpty()) throw new IllegalArgumentException("BankIBANNo Field Required");
        if (homelandId == 2 && chequeTemplate.isEmpty()) throw new IllegalArgumentException("Cheque Template Field Required");
        if (homelandId == 2 && glId == 0) throw new IllegalArgumentException("GL Account Field Required");
        rights.require(u, SCREEN_BANK, id == 0 ? "Save" : "Update");
        if (id > 0 && !owns(repo.banks(u), id)) throw new IllegalArgumentException("Record not found.");
        if (glId != 0 && !owns(repo.bankGlAccounts(u), glId)) throw new IllegalArgumentException("GL Account Field Required");
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@BranchCode", branchCode.trim());
        m.put("@BranchName", bankName.trim());
        m.put("@BranchAddress", raw(b.get("branchAddress")).trim());
        m.put("@BranchCity", cityId);
        m.put("@CountryId", countryId);
        m.put("@Contact1Tel", raw(b.get("contact1Tel")).trim());
        m.put("@Contact2Tel", raw(b.get("contact2Tel")).trim());
        m.put("@Contact3Mobile", raw(b.get("contact3Mobile")).trim());
        m.put("@emailPrimery", raw(b.get("emailPrimery")).trim());
        m.put("@emailAlternate", raw(b.get("emailAlternate")).trim());
        m.put("@IsHomeland", homeland);
        m.put("@OtherInfo", raw(b.get("otherInfo")).trim());
        m.put("@BankAccountNo", accountNo);
        m.put("@BankIBANNo", iban);
        m.put("@ChartOfAccountId", glId);
        m.put("@EntryDate", Timestamp.valueOf(now));
        m.put("@EntryUser", u.getId());
        m.put("@ModifyDate", Timestamp.valueOf(now));
        m.put("@ModifyUser", u.getId());
        m.put("@PostDate", null);
        m.put("@PostUser", 0);
        m.put("@PostState", false);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@ChequeTemplete", chequeTemplate);
        m.put("@BankAccountTitle", accountTitle);
        return result(repo.saveBank(m));
    }

    // ================================================================= helpers

    private static Map<String, Object> result(int n) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", n);
        return out;
    }

    private static Map<String, Object> one(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Record not found.");
        return rows.get(0);
    }

    private static boolean owns(List<Map<String, Object>> rows, int id) {
        if (id <= 0) return false;
        for (Map<String, Object> r : rows) if (asInt(ci(r, "Id")) == id) return true;
        return false;
    }

    private static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

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
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
