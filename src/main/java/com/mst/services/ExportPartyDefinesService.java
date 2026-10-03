package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportPartyDefinesRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportFormSupport.*;

/**
 * BLL side of 206 ExportPartyDefines "Export Parties Define" (Architecture.WinApp.Export).
 *
 * tabControl1: "Define Party" (panel6 entry + grdfrm history) | "Define Consignee/Notify" (groupBox1 "Entry Feilds"
 * + grdExConsignee history). Both tabs save Architecture.Model.Inventory.SupplierCustomer rows with
 * CustomerGroupId 7 through SupplierCustomer.Save; a consignee is a sub-party (ParentsSupCustId = the parent
 * export party, IsSubSupCust = true, GlAccountId = the parent's GL account).
 *
 * Rights: ScreenDefinition 206 - View to load, Save (btnsave / btnSaveConsignee), Update (btnupdate,
 * btnUpdateConsignee, the grid Update buttons and the multi-row Update buttons, the Select column), Print
 * (292-Print, 292_01-Print, the grid Print buttons). Organisation, company, user come from the session.
 *
 * Desktop quirks kept (do not "fix"):
 *  - Status is Conversion.ToBool(checkbox.Text) - the caption, not Checked. chkactivestatus.Text is " " and
 *    ChkConsigneeStatus.Text is "Active", so a NEW row is always saved with Status = 0; after ReadById the caption
 *    becomes "True"/"False" and that is what an Update saves. The page sends that caption as statusText.
 *  - ReadByIdConsignee(ID) sets RecIdConsignee = ID but loads SupplierCustomer.GetByID(RecId) - the PARTY tab's
 *    current record (0 when none, which makes GetByID throw "Index was outside the bounds of the array.").
 *  - cmbglac_Leave looks the GL account up in SupplierCustomer.Getall, whose procedure excludes customer group 7,
 *    so an export party is never found that way; a party of another group on the same GL account IS loaded.
 *  - Insert() builds a NEW SupplierCustomer on Update, so every column the form does not carry goes as 0 / omitted.
 */
@Service
public class ExportPartyDefinesService {

    public static final int SCREEN_ID = 206;
    public static final int CUSTOMER_GROUP_ID = 7;

    @Autowired private ExportPartyDefinesRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    // ================================================================= load

    /** supfrmDefineSupplier1_Load: rights, cmbglacfill, ParentAccountfill, cmbcountryfill, Provincefill, CityFill, datagridviewform, ConsigneeHistory. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.putAll(refresh());
        return out;
    }

    /** btnRefresh_Click / btnRefreshConsignee_Click: every combo and both grids re-read. */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "glAccounts", () -> glRows(repo.glAccounts(u, 0)));
        put(out, "parentAccounts", () -> parentRows(repo.globalAllSupplierCustomer(u)));
        put(out, "countries", () -> pick(repo.countries(u), "Id", "Description"));
        put(out, "provinces", () -> pick(repo.provinces(u), "Id", "Description"));
        put(out, "cities", () -> pick(repo.cities(u), "Id", "CityName"));
        List<Map<String, Object>> hist;
        try { hist = repo.formHistory(u); }
        catch (Exception e) { hist = new ArrayList<>(); out.put("historyError", msg(e)); }
        final List<Map<String, Object>> h = hist;
        put(out, "history", () -> partyRows(h));
        put(out, "consigneeHistory", () -> consigneeRows(h));
        return out;
    }

    /** cmbglacfill(GlrecId) - re-read with @RecId so the account of the loaded party is offered even when referred. */
    public List<Map<String, Object>> glAccounts(int recId) { return glRows(repo.glAccounts(user("View"), recId)); }

    private static List<Map<String, Object>> glRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("AccountTitle", text(ci(r, "AccountTitle")));
            m.put("AccountCode", text(ci(r, "AccountCode")));
            out.add(m);
        }
        return out;
    }

    /** ParentAccountfill: globalAllSupplierCustomer where CustomerGroupId == 7 -> Id, CompanyName, PartyCode, GlAccountId. */
    private static List<Map<String, Object>> parentRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(ci(r, "CustomerGroupId")) != CUSTOMER_GROUP_ID) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("CompanyName", text(ci(r, "CompanyName")));
            m.put("PartyCode", text(ci(r, "PartyCode")));
            m.put("GlAccountId", asInt(ci(r, "GlAccountId")));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("Name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    /** datagridviewform's dt - the grdfrm columns in the desktop's order (Id, ParentsSupCustId, CustomerGroupId, GlAccountId, PictureName hidden). */
    public static List<Map<String, Object>> partyRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ParentsSupCustId", asInt(ci(r, "ParentsSupCustId")));
            m.put("CustomerGroupId", asInt(ci(r, "CustomerGroupId")));
            m.put("GlAccountId", asInt(ci(r, "GlAccountId")));
            m.put("GlAccount", text(ci(r, "AccountTitle")));
            m.put("CompanyName", text(ci(r, "CompanyName")));
            m.put("PersonName", text(ci(r, "FirstName")));
            m.put("Mobile", text(ci(r, "MobilePersonal")));
            m.put("Country", asInt(ci(r, "CountryId")));
            m.put("Province", asInt(ci(r, "StateProvinceId")));
            m.put("City", asInt(ci(r, "CityId")));
            m.put("Email", text(ci(r, "Email")));
            m.put("EORI", text(ci(r, "STRN_No")));
            m.put("VATNo", text(ci(r, "NTN_No")));
            m.put("ZipCode", text(ci(r, "ZipCode")));
            m.put("WebPage", text(ci(r, "WebPage")));
            m.put("Address", text(ci(r, "Address1")));
            m.put("ParentAccountTitle", text(ci(r, "ParentAccountTitle")));
            m.put("Status", statusText(ci(r, "Status")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("PictureName", text(ci(r, "PictureName")));
            out.add(m);
        }
        return out;
    }

    /** ConsigneeHistory: FormHistory rows with ParentsSupCustId > 0 and Id != ParentsSupCustId, in the grdExConsignee column order. */
    public static List<Map<String, Object>> consigneeRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int parent = asInt(ci(r, "ParentsSupCustId")), id = asInt(ci(r, "Id"));
            if (!(parent > 0 && id != parent)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("ParentsSupCustId", parent);
            m.put("ParentAccountTitle", text(ci(r, "ParentAccountTitle")));
            m.put("GlAccountId", asInt(ci(r, "GlAccountId")));
            m.put("GlAccount", text(ci(r, "AccountTitle")));
            m.put("CompanyName", text(ci(r, "CompanyName")));
            m.put("Country", asInt(ci(r, "CountryId")));
            m.put("City", asInt(ci(r, "CityId")));
            m.put("EORI", text(ci(r, "STRN_No")));
            m.put("VATNo", text(ci(r, "NTN_No")));
            m.put("Email", text(ci(r, "Email")));
            m.put("WebPage", text(ci(r, "WebPage")));
            m.put("ZipCode", text(ci(r, "ZipCode")));
            m.put("Address", text(ci(r, "Address1")));
            m.put("Status", statusText(ci(r, "Status")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            out.add(m);
        }
        return out;
    }

    /** The desktop dt column "Status" is typeof(string): a bit becomes "True" / "False". */
    private static String statusText(Object v) { return asBool(v) ? "True" : "False"; }

    // ================================================================= read

    /** ReadById(ID): SupplierCustomer.GetByID into the party fields (+ cmbglacfill(GlAccountId)); the picture file is not reachable from the web. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> rows = repo.byId(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was outside the bounds of the array.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> out = new LinkedHashMap<>(r);
        out.put("record", record(r));
        out.put("glAccounts", glRows(repo.glAccounts(u, asInt(ci(r, "GlAccountId")))));
        out.put("history", partyRows(repo.formHistory(u)));
        return out;
    }

    /** The SupplierCustomer properties the two ReadById methods copy into controls. */
    private static Map<String, Object> record(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("CompanyName", text(ci(r, "CompanyName")));
        m.put("GlAccountId", asInt(ci(r, "GlAccountId")));
        m.put("NTN_No", text(ci(r, "NTN_No")));
        m.put("STRN_No", text(ci(r, "STRN_No")));
        m.put("Title", text(ci(r, "Title")));
        m.put("FirstName", text(ci(r, "FirstName")));
        m.put("LastName", text(ci(r, "LastName")));
        m.put("CNIC", text(ci(r, "CNIC")));
        m.put("CNIC_EXPIRY_DATE", iso(ci(r, "CNIC_EXPIRY_DATE")));
        m.put("Address1", text(ci(r, "Address1")));
        m.put("Status", asBool(ci(r, "Status")));
        m.put("StatusText", statusText(ci(r, "Status")));
        m.put("CountryId", asInt(ci(r, "CountryId")));
        m.put("CityId", asInt(ci(r, "CityId")));
        m.put("Town", text(ci(r, "Town")));
        m.put("ZipCode", text(ci(r, "ZipCode")));
        m.put("Phone", text(ci(r, "Phone")));
        m.put("MobileOffice", text(ci(r, "MobileOffice")));
        m.put("MobilePersonal", text(ci(r, "MobilePersonal")));
        m.put("Email", text(ci(r, "Email")));
        m.put("WebPage", text(ci(r, "WebPage")));
        m.put("AdvanceGlAcId", asInt(ci(r, "AdvanceGlAcId")));
        m.put("StateProvinceId", asInt(ci(r, "StateProvinceId")));
        m.put("ParentsSupCustId", asInt(ci(r, "ParentsSupCustId")));
        m.put("PictureURL", text(ci(r, "PictureURL")));
        return m;
    }

    /**
     * cmbglac_Leave: SupplierCustomerGetAllServiceBind rows whose GlAccountId == the chosen account -> ReadById of
     * each match (the last one wins). Returns the id to load, 0 when nothing matched. A failure is "Id Not Found".
     */
    public Map<String, Object> glAccountLeave(int glAccountId) {
        UserAccount u = user("View");
        int found = 0;
        try {
            for (Map<String, Object> r : repo.getAll(u)) {
                if (asInt(ci(r, "GlAccountId")) == glAccountId) found = asInt(ci(r, "Id"));
            }
        } catch (Exception e) { throw new IllegalStateException("Id Not Found"); }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", found);
        return out;
    }

    /**
     * ReadByIdConsignee(ID): RecIdConsignee = ID, but the fields come from GetByID(RecId) - the party tab's record
     * (desktop bug, kept). mainRecId is that RecId; 0 raises the desktop's index error.
     */
    public Map<String, Object> readConsigneeById(int id, int mainRecId) {
        user("View");
        List<Map<String, Object>> rows = repo.byId(mainRecId);
        if (rows.isEmpty()) throw new IllegalStateException("Index was outside the bounds of the array.");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("recIdConsignee", id);
        out.put("record", record(rows.get(0)));
        return out;
    }

    // ================================================================= save (party tab)

    /** Insert(): formvalidation() then SupplierCustomer.Save with the fields the form sets. */
    public Map<String, Object> saveParty(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recId")));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        if (asInt(b.get("glAccountId")) == 0 && text(b.get("glAccountText")).isEmpty()) throw new IllegalArgumentException("GL Account Field is Required!!!");
        if (text(b.get("companyName")).isEmpty()) throw new IllegalArgumentException("Company Name Field is Required!!!");
        if (text(b.get("firstName")).isEmpty()) throw new IllegalArgumentException("Person Name Field is Required!!!");
        if (!flag(b.get("activeChecked")) && recId == 0) throw new IllegalArgumentException("Please check the Active Status!!!");

        Map<String, Object> m = blank(u);
        m.put("Id", recId);
        m.put("CompanyName", text(b.get("companyName")));
        m.put("ReportingTitle", text(b.get("companyName")));
        m.put("GlAccountId", asInt(b.get("glAccountId")));
        m.put("CustomerGroupId", CUSTOMER_GROUP_ID);
        m.put("NTN_No", text(b.get("ntnNo")));
        m.put("STRN_No", text(b.get("strnNo")));
        m.put("Title", text(b.get("title")));
        m.put("FirstName", text(b.get("firstName")));
        m.put("LastName", text(b.get("lastName")));
        m.put("CNIC", text(b.get("cnic")));
        m.put("CNIC_EXPIRY_DATE", ts(b.get("cnicExpiry")));
        m.put("Address1", text(b.get("address1")));
        m.put("Status", asBool(b.get("statusText")));          // Conversion.ToBool(chkactivestatus.Text.Trim())
        m.put("CountryId", asInt(b.get("countryId")));
        m.put("CityId", asInt(b.get("cityId")));
        m.put("Town", text(b.get("town")));
        m.put("ZipCode", text(b.get("zipCode")));
        m.put("Phone", text(b.get("phone")));
        m.put("MobileOffice", text(b.get("mobileOffice")));
        m.put("MobilePersonal", text(b.get("mobilePersonal")));
        m.put("Email", text(b.get("email")));
        m.put("WebPage", text(b.get("webPage")));
        m.put("AdvanceGlAcId", asInt(b.get("advanceGlAcId")));
        m.put("StateProvinceId", asInt(b.get("stateProvinceId")));
        m.put("ParentsSupCustId", 0);
        m.put("IsSubSupCust", false);
        m.put("PictureURL", fileName(text(b.get("pictureUrl"))));   // Path.GetFileName(fileSavePath)
        int n = repo.save(m);
        return saved(n, recId > 0 ? "Data Update Successfully.... " : "Data Save Successfully.... ");
    }

    /** grdfrm "Update" button / Ctrl+Space on it - RecordUpdate(item) with the row's (edited) cells. */
    public Map<String, Object> updatePartyRow(Map<String, Object> r) {
        UserAccount u = user("Update");
        repo.save(partyRowModel(u, r, false));
        return saved(asInt(r.get("Id")), "Record Updated Successfully!");
    }

    /** btnUpdateRecordsFromGrid_Click - every checked row through SupplierCustomer.Save (ParentsSupCustId kept, IsSubSupCust when it has a parent, PictureURL kept). */
    public Map<String, Object> updatePartyRows(Map<String, Object> body) {
        UserAccount u = user("Update");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Please Select Rows first to update multi rows");
        for (Map<String, Object> r : rows) repo.save(partyRowModel(u, r, true));
        return saved(rows.size(), "Record's Updated Successfully!");
    }

    private Map<String, Object> partyRowModel(UserAccount u, Map<String, Object> r, boolean multi) {
        Map<String, Object> m = blank(u);
        m.put("Id", asInt(r.get("Id")));
        m.put("GlAccountId", asInt(r.get("GlAccountId")));
        if (multi) {
            int parent = asInt(r.get("ParentsSupCustId"));
            m.put("ParentsSupCustId", parent);
            if (parent > 0) m.put("IsSubSupCust", true);
            m.put("PictureURL", fileName(text(r.get("PictureName"))));
        }
        m.put("CompanyName", text(r.get("CompanyName")));
        m.put("ReportingTitle", text(r.get("CompanyName")));
        m.put("CustomerGroupId", CUSTOMER_GROUP_ID);
        m.put("FirstName", text(r.get("PersonName")));
        m.put("MobilePersonal", text(r.get("Mobile")));
        m.put("CountryId", asInt(r.get("Country")));
        m.put("StateProvinceId", asInt(r.get("Province")));
        m.put("CityId", asInt(r.get("City")));
        m.put("Email", text(r.get("Email")));
        m.put("STRN_No", text(r.get("EORI")));
        m.put("NTN_No", text(r.get("VATNo")));
        m.put("ZipCode", text(r.get("ZipCode")));
        m.put("WebPage", text(r.get("WebPage")));
        m.put("Address1", text(r.get("Address")));
        m.put("Status", asBool(r.get("Status")));
        return m;
    }

    // ================================================================= save (consignee tab)

    /** InsertConsignee(): formValidationConsignee() then SupplierCustomer.Save as a sub-party of the chosen parent. */
    public Map<String, Object> saveConsignee(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recIdConsignee")));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        int parentId = asInt(b.get("parentId"));
        if (parentId == 0 && text(b.get("parentText")).isEmpty()) throw new IllegalArgumentException("Parent Account Field is Required!!!");
        if (text(b.get("consigneeName")).isEmpty()) throw new IllegalArgumentException("Consignee Name Field is Required!!!");
        if (!flag(b.get("activeChecked")) && recId == 0) throw new IllegalArgumentException("Please check the Active Status!!!");
        /* CmbParentAcConsignee.SelectedRow.Cells["GlAccountId"] - from the parent list, never from the request. */
        int glAccountId = 0;
        for (Map<String, Object> p : parentRows(repo.globalAllSupplierCustomer(u))) {
            if (asInt(p.get("Id")) == parentId) glAccountId = asInt(p.get("GlAccountId"));
        }
        if (parentId != 0 && glAccountId == 0) throw new IllegalStateException("Object reference not set to an instance of an object.");
        Map<String, Object> m = blank(u);
        m.put("Id", recId);
        m.put("ParentsSupCustId", parentId);
        m.put("CompanyName", text(b.get("consigneeName")));
        m.put("GlAccountId", glAccountId);
        m.put("ReportingTitle", text(b.get("consigneeName")));
        m.put("CustomerGroupId", CUSTOMER_GROUP_ID);
        m.put("CountryId", asInt(b.get("countryId")));
        m.put("CityId", asInt(b.get("cityId")));
        m.put("STRN_No", text(b.get("strnNo")));
        m.put("NTN_No", text(b.get("ntnNo")));
        m.put("Email", text(b.get("email")));
        m.put("WebPage", text(b.get("webPage")));
        m.put("ZipCode", text(b.get("zipCode")));
        m.put("Address1", text(b.get("address1")));
        m.put("Status", asBool(b.get("statusText")));          // Conversion.ToBool(ChkConsigneeStatus.Text.Trim()) - "Active" -> false
        m.put("IsSubSupCust", true);
        int n = repo.save(m);
        return saved(n, recId > 0 ? "Data Update Successfully.... " : "Data Save Successfully.... ");
    }

    /** grdExConsignee "Update" button - RecordUpdateConsignee(item) (IsSubSupCust is NOT set here, as on the desktop). */
    public Map<String, Object> updateConsigneeRow(Map<String, Object> r) {
        UserAccount u = user("Update");
        repo.save(consigneeRowModel(u, r, false));
        return saved(asInt(r.get("Id")), "Record Updated Successfully!");
    }

    /** btnMultiRecordsUpdateConsignee_Click - checked rows, IsSubSupCust = true. */
    public Map<String, Object> updateConsigneeRows(Map<String, Object> body) {
        UserAccount u = user("Update");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Please Select Rows first to update multi rows");
        for (Map<String, Object> r : rows) repo.save(consigneeRowModel(u, r, true));
        return saved(rows.size(), "Record's Updated Successfully!");
    }

    private Map<String, Object> consigneeRowModel(UserAccount u, Map<String, Object> r, boolean multi) {
        Map<String, Object> m = blank(u);
        m.put("Id", asInt(r.get("Id")));
        m.put("GlAccountId", asInt(r.get("GlAccountId")));
        m.put("ParentsSupCustId", asInt(r.get("ParentsSupCustId")));
        m.put("CompanyName", text(r.get("CompanyName")));
        m.put("ReportingTitle", text(r.get("CompanyName")));
        m.put("CustomerGroupId", CUSTOMER_GROUP_ID);
        m.put("CountryId", asInt(r.get("Country")));
        m.put("CityId", asInt(r.get("City")));
        m.put("Email", text(r.get("Email")));
        m.put("STRN_No", text(r.get("EORI")));
        m.put("NTN_No", text(r.get("VATNo")));
        m.put("ZipCode", text(r.get("ZipCode")));
        m.put("WebPage", text(r.get("WebPage")));
        m.put("Address1", text(r.get("Address")));
        m.put("Status", asBool(r.get("Status")));
        if (multi) m.put("IsSubSupCust", true);
        return m;
    }

    // ================================================================= model

    /**
     * A new Architecture.Model.Inventory.SupplierCustomer as GenericProvider.SetProc sends it: the 58 non-virtual
     * properties in declaration order, CLR defaults (0 / false / null -> omitted), plus what every save sets:
     * EntryUser / ModifyUser = the user, Organization / Company from the session, EntryDate / ModifyDate /
     * CNIC_EXPIRY_DATE = now (BLL Save). AccountTitle / AccountTitleAdvance / CustomerType are virtual.
     */
    private static Map<String, Object> blank(UserAccount u) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsDebitCredit", false);
        m.put("IsSubSupCust", false);
        m.put("PostState", false);
        m.put("Status", false);
        m.put("CNIC_EXPIRY_DATE", now);
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("PostDate", null);
        m.put("CreditLimit", BigDecimal.ZERO);
        m.put("DebitCreditAmount", BigDecimal.ZERO);
        m.put("ActionId", 0);
        m.put("AdvanceGlAcId", 0);
        m.put("BranchId", 0);
        m.put("CityId", 0);
        m.put("CompanyId", u.getCompanyId());
        m.put("CountryId", 0);
        m.put("CustomerGroupId", 0);
        m.put("EntryUser", u.getId());
        m.put("GlAccountId", 0);
        m.put("Id", 0);
        m.put("ModifyUser", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("ParentsSupCustId", 0);
        m.put("DiscountPolicyId", 0);
        m.put("PostUser", 0);
        m.put("ProfileGroupId", 0);
        m.put("ProjectId", 0);
        m.put("StateProvinceId", 0);
        m.put("CustomerTypeId", 0);
        m.put("PartyTypeId", 0);
        m.put("BusinessTypeId", 0);
        m.put("PartyTypePrefix", null);
        m.put("Address1", null);
        m.put("Address2", null);
        m.put("CNIC", null);
        m.put("CompanyName", null);
        m.put("Email", null);
        m.put("FirstName", null);
        m.put("FTN_No", null);
        m.put("LastName", null);
        m.put("MobileOffice", null);
        m.put("MobilePersonal", null);
        m.put("NTN_No", null);
        m.put("Phone", null);
        m.put("PictureURL", null);
        m.put("NickName", null);
        m.put("ReportingTitle", null);
        m.put("STRN_No", null);
        m.put("SupCustCode", null);
        m.put("Title", null);
        m.put("Town", null);
        m.put("WebPage", null);
        m.put("ZipCode", null);
        m.put("ManualPartyCode", null);
        m.put("WhatsAppNo", null);
        m.put("CompanyIndividuals", null);
        m.put("IsTaxable", false);
        return m;
    }

    /** Path.GetFileName. */
    private static String fileName(String p) {
        if (p == null) return "";
        int i = Math.max(p.lastIndexOf('/'), p.lastIndexOf('\\'));
        return i >= 0 ? p.substring(i + 1) : p;
    }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
