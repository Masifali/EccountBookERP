package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportDefinesRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportQSupport.*;

/**
 * BLL side of four Export definition forms (Architecture.WinApp.Export):
 *
 *   207  ExImfrmDefineDocuments        "Define Documents"            /export/define-documents
 *   -    DefineExportCharges           "Define Export Charges"       /export/define-export-charges
 *   -    DefineThirdPartyType          "Define Third Party Type"     /export/define-third-party-type
 *   -    frmGenerateExportContractNos  "Generate Export Contract Nos" (+ DefineExportPrefixType popup)  /export/generate-contract-nos
 *
 * None of these forms carries rights code on the desktop (whoever can open the form can save). 207 is
 * checked against its own ScreenDefinition row; the three popups (no ScreenDefinition row) against the
 * parent screens that open them (any one granting the action is enough). The desktop never gates Save,
 * so every action here requires only "View" - documented so the coordinator can tighten it.
 * Organisation, company, user and financial year come from the session, never from the request.
 */
@Service
public class ExportDefinesService {

    public static final int SCREEN_DEFINE_DOCUMENTS = 207;
    /** DefineExportCharges opens from 951 Custom Invoice, 209 Sales Contract, 879 Contract III, 211/880 Commercial Invoice. */
    public static final int[] PARENTS_EXPORT_CHARGES = {951, 209, 880, 879, 211};
    /** DefineThirdPartyType opens from 857 Third Party Inspection / 858 Lab Against Third Party Inspection. */
    public static final int[] PARENTS_THIRD_PARTY_TYPE = {857, 858};
    /** frmGenerateExportContractNos opens from 879 Contract III (and the 209 Sales Contract family). */
    public static final int[] PARENTS_CONTRACT_NOS = {879, 209};
    /** frmGenerateExportContractNos.DocumentTypeId. */
    public static final int CONTRACT_NOS_DOCUMENT_TYPE_ID = 223;

    @Autowired private ExportDefinesRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int[] screens) {
        UserAccount u = currentUserContext.requireAccountingUser();
        requireAny(rights, u, screens, "View");
        return u;
    }

    private static final int[] DOCS = {SCREEN_DEFINE_DOCUMENTS};

    // ================================================================================================
    // 207 ExImfrmDefineDocuments
    // ================================================================================================

    /** frmList_Load: GroupCodeBind, DocNameBind, DocListFill, GroupListFill, AssigndoctogrpFill. */
    public Map<String, Object> documentsSetup() {
        UserAccount u = user(DOCS);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "documents", () -> docRows(repo.shipmentDocuments()));
        put(out, "groups", () -> groupRows(repo.docGroups(u)));
        assignRows(out, u);
        return out;
    }

    /** DocListFill: Id / DocCode (exImDocCode) / DocumentName (exImDocName). Also the cmbDocName source. */
    private static List<Map<String, Object>> docRows(List<Map<String, Object>> lst) {
        return project(lst, "Id", "Id", "DocCode", "exImDocCode", "DocumentName", "exImDocName");
    }

    /** GroupListFill: Id / GroupCode (ExImDocGroupCode) / GroupName (ExImDocGroupName). Also the cmbGroupCode source. */
    private static List<Map<String, Object>> groupRows(List<Map<String, Object>> lst) {
        return project(lst, "Id", "Id", "GroupCode", "ExImDocGroupCode", "GroupName", "ExImDocGroupName");
    }

    /**
     * AssigndoctogrpFill: the grid reads ExImDocGroupCode and exImDocName from
     * Sp_ExImShipmentDocGroupSchedule_GetAllMethod 'ReadByCompanyOrganizationId', which returns neither
     * (only Id, Original, Copies, ExImShipmentDocuments, ExImShipmentDocGroupId, CompanyId, OrganizationId).
     * On the desktop DataRow["ExImDocGroupCode"] throws as soon as a row exists and the catch shows the
     * message; the grid stays empty. Reproduced: "assignError" carries that message whenever a column is
     * missing (so the grid fills by itself once the procedure returns the columns).
     */
    private void assignRows(Map<String, Object> out, UserAccount u) {
        List<Map<String, Object>> lst;
        try { lst = repo.docGroupSchedules(u); }
        catch (Exception e) { out.put("assign", new ArrayList<>()); out.put("assignError", msg(e)); return; }
        List<Map<String, Object>> rows = new ArrayList<>();
        if (!lst.isEmpty()) {
            for (String c : new String[] {"Id", "ExImDocGroupCode", "exImDocName", "Copies", "Original"}) {
                if (!has(lst.get(0), c)) {
                    out.put("assign", rows);
                    out.put("assignError", "Column '" + c + "' does not belong to table .");
                    return;
                }
            }
            rows = project(lst, "Id", "Id", "GroupCode", "ExImDocGroupCode", "DocumentName", "exImDocName",
                    "Copies", "Copies", "Orignial", "Original");
        }
        out.put("assign", rows);
    }

    public Map<String, Object> documentsList() {
        user(DOCS);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "documents", () -> docRows(repo.shipmentDocuments()));
        return out;
    }

    public Map<String, Object> groupsList() {
        UserAccount u = user(DOCS);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "groups", () -> groupRows(repo.docGroups(u)));
        return out;
    }

    public Map<String, Object> assignList() {
        UserAccount u = user(DOCS);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "groups", () -> groupRows(repo.docGroups(u)));
        put(out, "documents", () -> docRows(repo.shipmentDocuments()));
        assignRows(out, u);
        return out;
    }

    /** GetByID(..)[0] - List indexer on an empty list. */
    private static Map<String, Object> first(List<Map<String, Object>> r) {
        if (r.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        return r.get(0);
    }

    /** grdDocList_DoubleClick -> ReadById: ExImShipmentDocuments.GetByID. */
    public Map<String, Object> readDocument(int id) {
        user(DOCS);
        Map<String, Object> r = first(repo.shipmentDocumentById(id));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("exImDocCode", raw(ci(r, "exImDocCode")));
        m.put("exImDocName", raw(ci(r, "exImDocName")));
        return m;
    }

    /**
     * btnAddinGrid_Click: no validation on the desktop; Id = RecIdDoc only when RecIdDoc > 0 and the update
     * mode is on; "Save SuccessFully" / "Update SuccessFully" only when Save returned > 0.
     */
    public Map<String, Object> saveDocument(Map<String, Object> body) {
        user(DOCS);
        int recId = asInt(body.get("recId"));
        boolean update = asBool(body.get("updateMode"));
        Map<String, Object> model = new LinkedHashMap<>();
        model.put("Id", recId > 0 && update ? recId : 0);
        model.put("exImDocCode", raw(body.get("code")));
        model.put("exImDocName", raw(body.get("name")));
        int success = repo.saveShipmentDocument(model);
        return saved(success, success > 0 ? (update ? "Update SuccessFully" : "Save SuccessFully") : "");
    }

    /** grdDocGroup_DoubleClick -> ReadByGrpId: ExImShipmentDocGroup.GetByID. */
    public Map<String, Object> readGroup(int id) {
        user(DOCS);
        Map<String, Object> r = first(repo.docGroupById(id));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("ExImDocGroupCode", raw(ci(r, "ExImDocGroupCode")));
        m.put("ExImDocGroupName", raw(ci(r, "ExImDocGroupName")));
        return m;
    }

    /**
     * btnSaveGroup_Click. QUIRK (reproduced): the form sets only the code and the name, so the model's
     * CompanyId and OrganizationId go to the procedure as 0 - a saved group is stored under company 0 and
     * does not come back in this company's list (ReadByCompanyOrganizationId); an update moves it to 0 too.
     */
    public Map<String, Object> saveGroup(Map<String, Object> body) {
        user(DOCS);
        int recId = asInt(body.get("recId"));
        boolean update = asBool(body.get("updateMode"));
        Map<String, Object> model = new LinkedHashMap<>();
        model.put("CompanyId", 0);
        model.put("Id", recId > 0 && update ? recId : 0);
        model.put("OrganizationId", 0);
        model.put("ExImDocGroupCode", raw(body.get("code")));
        model.put("ExImDocGroupName", raw(body.get("name")));
        int success = repo.saveDocGroup(model);
        return saved(success, success > 0 ? (update ? "Update SuccessFully" : "Save SuccessFully") : "");
    }

    /** grdAssignDocToGrp_DoubleClick -> ReadByDocGrpId. */
    public Map<String, Object> readSchedule(int id) {
        user(DOCS);
        Map<String, Object> r = first(repo.docGroupScheduleById(id));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("ExImShipmentDocuments", asInt(ci(r, "ExImShipmentDocuments")));
        m.put("ExImShipmentDocGroupId", asInt(ci(r, "ExImShipmentDocGroupId")));
        m.put("Copies", asInt(ci(r, "Copies")));
        m.put("Original", asInt(ci(r, "Original")));
        return m;
    }

    /**
     * btnSavedoctoGroup_Click: group / document from the combos, Copies / Original Conversion.ToInt of the
     * boxes; no validation. Same CompanyId / OrganizationId = 0 quirk as the group save.
     * Messages "Save Successfully" / "Update SuccessFully" (the desktop's spelling).
     */
    public Map<String, Object> saveSchedule(Map<String, Object> body) {
        user(DOCS);
        int recId = asInt(body.get("recId"));
        boolean update = asBool(body.get("updateMode"));
        Map<String, Object> model = new LinkedHashMap<>();
        model.put("CompanyId", 0);
        model.put("Copies", asInt(body.get("copies")));
        model.put("ExImShipmentDocGroupId", asInt(body.get("groupId")));
        model.put("ExImShipmentDocuments", asInt(body.get("documentId")));
        model.put("Id", recId > 0 && update ? recId : 0);
        model.put("OrganizationId", 0);
        model.put("Original", asInt(body.get("original")));
        int success = repo.saveDocGroupSchedule(model);
        return saved(success, success > 0 ? (update ? "Update SuccessFully" : "Save Successfully") : "");
    }

    // ================================================================================================
    // DefineExportCharges
    // ================================================================================================

    /** InitializeComponentMethod: FormHistory -> BindGrid, then ExpensesAcBind. */
    public Map<String, Object> chargesSetup() {
        UserAccount u = user(PARENTS_EXPORT_CHARGES);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "history", () -> chargesRows(repo.exportChargesHistory(u)));
        put(out, "accounts", () -> expenseAccounts(u));
        return out;
    }

    /** BindGrid: Id, ChargesCode, ChargesName, AccountId, AccountTitle, EntryUser, EntryDate, ModifyUser, ModifyDate. */
    private static List<Map<String, Object>> chargesRows(List<Map<String, Object>> dt) {
        return project(dt, "Id", "ExportChargesID", "ChargesCode", "ExportChargesCode", "ChargesName", "ExportChargesName",
                "AccountId", "AccountId", "AccountTitle", "AccountTitle", "EntryUser", "EntryUserName", "EntryDate", "EntryDate",
                "ModifyUser", "ModifyUserName", "ModifyDate", "ModifyDate");
    }

    /**
     * ExpensesAcBind: DatatableHelper.GetAccountsFromGlobalByTypeIds(withTypeIds {21, 10}, withPlNoteIds {3}) over
     * the AllAccountsWithCustomGroupId global list (USP_GETAllAccountsFromCustomGroups) - AccountTypeId in (21, 10)
     * and PLNoteId = 3, first row per ChartOfAccountId; columns Id, AccountTitle, AccountCode, ParentAccountTitle,
     * AccountClass (AccountClassName), all shown.
     */
    private List<Map<String, Object>> expenseAccounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : repo.accountsWithCustomGroup(u)) {
            int type = asInt(ci(a, "AccountTypeId"));
            if (type != 21 && type != 10) continue;
            if (asInt(ci(a, "PLNoteId")) != 3) continue;
            int id = asInt(ci(a, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", text(ci(a, "AccountTitle")));
            m.put("AccountCode", text(ci(a, "AccountCode")));
            m.put("ParentAccountTitle", text(ci(a, "ParentAccountTitle")));
            m.put("AccountClass", text(ci(a, "AccountClassName")));
            out.add(m);
        }
        return out;
    }

    /** ResetForm: GlobalServicesDbCall("AccountsWithCustomGroupId") then ExpensesAcBind. */
    public Map<String, Object> chargesAccounts() {
        UserAccount u = user(PARENTS_EXPORT_CHARGES);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "accounts", () -> expenseAccounts(u));
        return out;
    }

    public Map<String, Object> chargesHistory() {
        UserAccount u = user(PARENTS_EXPORT_CHARGES);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "history", () -> chargesRows(repo.exportChargesHistory(u)));
        return out;
    }

    /** IsAnyCellValueHaveSameValueExcept over the history grid: exact (case-sensitive) cell equality, row Id != RecId. */
    private static boolean duplicate(List<Map<String, Object>> grid, String col, String value, int recId) {
        for (Map<String, Object> r : grid) {
            if (asInt(r.get("Id")) == recId) continue;
            Object c = r.get(col);
            if (c != null && String.valueOf(c).equals(value)) return true;
        }
        return false;
    }

    /**
     * Insert(): ValidateInputs (texts and order as the desktop), then ExportCharges.Save with the 10 model
     * properties, then "Record saved successfully." / "Record updated successfully.". The toolbar Save sends
     * recId 0 (btnSave_Click sets RecId = 0 - always an insert); Update and the Save/Update button send the
     * loaded RecId. The confirmation is asked on the page before the request.
     */
    public Map<String, Object> saveCharges(Map<String, Object> body) {
        UserAccount u = user(PARENTS_EXPORT_CHARGES);
        int recId = asInt(body.get("recId"));
        String code = raw(body.get("code")), name = raw(body.get("name"));
        int accountId = asInt(body.get("accountId"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("Charges Code is required");
        if (name.trim().isEmpty()) throw new IllegalArgumentException("Charges Name is required");
        if (accountId == 0) throw new IllegalArgumentException("Account Title is required");
        List<Map<String, Object>> grid = chargesRows(repo.exportChargesHistory(u));
        if (duplicate(grid, "ChargesCode", code.trim(), recId)) throw new IllegalArgumentException("This Charges Code already exists.");
        if (duplicate(grid, "ChargesName", name.trim(), recId)) throw new IllegalArgumentException("This Charges Name already exists.");
        Timestamp now = now();
        Map<String, Object> model = new LinkedHashMap<>();
        model.put("EntryDate", now);
        model.put("ModifyDate", now);
        model.put("AccountId", accountId);
        model.put("CompanyId", u.getCompanyId());
        model.put("EntryUserId", u.getId());
        model.put("ExportChargesID", recId);
        model.put("ModifyUserId", u.getId());
        model.put("OrganizationId", u.getOrganizationId());
        model.put("ExportChargesCode", code.trim());
        model.put("ExportChargesName", name.trim());
        int id = repo.saveExportCharges(model);
        return saved(id, recId > 0 ? "Record updated successfully." : "Record saved successfully.");
    }

    // ================================================================================================
    // DefineThirdPartyType
    // ================================================================================================

    public Map<String, Object> thirdPartyTypeSetup() {
        UserAccount u = user(PARENTS_THIRD_PARTY_TYPE);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "history", () -> tptRows(repo.thirdPartyTypeHistory(u)));
        return out;
    }

    /** BindGrid: Id, PartyTypeCode, PartyTypeName, EntryUser, EntryDate, ModifyUser, ModifyDate, Active. */
    private static List<Map<String, Object>> tptRows(List<Map<String, Object>> dt) {
        List<Map<String, Object>> out = project(dt, "Id", "ThirdPartyTypeID", "PartyTypeCode", "PartyTypeCode", "PartyTypeName", "PartyTypeName",
                "EntryUser", "EntryUserName", "EntryDate", "EntryDate", "ModifyUser", "ModifyUserName", "ModifyDate", "ModifyDate",
                "Active", "IsActive");
        for (Map<String, Object> m : out) m.put("Active", asBool(m.get("Active")));
        return out;
    }

    /** Insert(): ValidateInputs then ThirdPartyType.Save (10 model properties, IsActive = chkIsActive.Checked). */
    public Map<String, Object> saveThirdPartyType(Map<String, Object> body) {
        UserAccount u = user(PARENTS_THIRD_PARTY_TYPE);
        int recId = asInt(body.get("recId"));
        String code = raw(body.get("code")), name = raw(body.get("name"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("PartyType Code is required");
        if (name.trim().isEmpty()) throw new IllegalArgumentException("PartyType Name is required");
        List<Map<String, Object>> grid = tptRows(repo.thirdPartyTypeHistory(u));
        if (duplicate(grid, "PartyTypeCode", code.trim(), recId)) throw new IllegalArgumentException("This PartyType Code already exists.");
        if (duplicate(grid, "PartyTypeName", name.trim(), recId)) throw new IllegalArgumentException("This PartyType Name already exists.");
        Timestamp now = now();
        Map<String, Object> model = new LinkedHashMap<>();
        model.put("EntryDate", now);
        model.put("ModifyDate", now);
        model.put("IsActive", asBool(body.get("isActive")));
        model.put("CompanyId", u.getCompanyId());
        model.put("EntryUserId", u.getId());
        model.put("ModifyUserId", u.getId());
        model.put("OrganizationId", u.getOrganizationId());
        model.put("ThirdPartyTypeID", recId);
        model.put("PartyTypeCode", code.trim());
        model.put("PartyTypeName", name.trim());
        int id = repo.saveThirdPartyType(model);
        return saved(id, recId > 0 ? "Record updated successfully." : "Record saved successfully.");
    }

    // ================================================================================================
    // frmGenerateExportContractNos (+ DefineExportPrefixType)
    // ================================================================================================

    /** frmGenerateExportContractNos_Load: BindPrefixType + BindHistory. */
    public Map<String, Object> contractNosSetup() {
        UserAccount u = user(PARENTS_CONTRACT_NOS);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "prefixTypes", () -> project(repo.exportPrefixTypes(), "Id", "Id", "PrefixDescription", "PrefixDescription"));
        historyInto(out, u);
        return out;
    }

    public Map<String, Object> contractNosHistory() {
        UserAccount u = user(PARENTS_CONTRACT_NOS);
        Map<String, Object> out = new LinkedHashMap<>();
        historyInto(out, u);
        return out;
    }

    /**
     * BindHistory: GenerateExportInvoiceNos.GetAll(DocumentTypeId 223); the history grid keeps the FIRST row of
     * each MainId (Id, MainId, PrefixTypeId, PrefixType, Prefix, SerialFrom, SerialTo, ContractNo, EntryDate as
     * ToShortDateString, EntryUser); "rows" keeps every row for Grd_SelectionChanged's detail grid (Id, MainId,
     * Prefix, SerialFrom, SerialTo, ContractNo).
     */
    private void historyInto(Map<String, Object> out, UserAccount u) {
        List<Map<String, Object>> dt;
        try { dt = repo.exportInvoiceNosHistory(u, CONTRACT_NOS_DOCUMENT_TYPE_ID); }
        catch (Exception e) { out.put("history", new ArrayList<>()); out.put("rows", new ArrayList<>()); out.put("historyError", msg(e)); return; }
        List<Map<String, Object>> hist = new ArrayList<>(), rows = new ArrayList<>();
        Set<Integer> mains = new HashSet<>();
        for (Map<String, Object> r : dt) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", asInt(ci(r, "Id")));
            d.put("MainId", asInt(ci(r, "MainId")));
            d.put("Prefix", text(ci(r, "Prefix")));
            d.put("SerialFrom", asInt(ci(r, "SerialFrom")));
            d.put("SerialTo", asInt(ci(r, "SerialTo")));
            d.put("ContractNo", text(ci(r, "FinalInvoiceNo")));
            rows.add(d);
            if (mains.add(asInt(ci(r, "MainId")))) {
                Map<String, Object> h = new LinkedHashMap<>();
                h.put("Id", asInt(ci(r, "Id")));
                h.put("MainId", asInt(ci(r, "MainId")));
                h.put("PrefixTypeId", asInt(ci(r, "PrefixTypeId")));
                h.put("PrefixType", text(ci(r, "PrefixDescription")));
                h.put("Prefix", text(ci(r, "Prefix")));
                h.put("SerialFrom", asInt(ci(r, "SerialFrom")));
                h.put("SerialTo", asInt(ci(r, "SerialTo")));
                h.put("ContractNo", text(ci(r, "FinalInvoiceNo")));
                h.put("EntryDate", iso(ci(r, "EntryDate")));
                h.put("EntryUser", text(ci(r, "EntryUser")));
                hist.add(h);
            }
        }
        out.put("history", hist);
        out.put("rows", rows);
    }

    /**
     * FormValiadation, in the desktop's order and wording. The two "required" checks fire only for an EMPTY box
     * (Text == "" && ToInt == 0) - "0" or text passes and is then caught by the comparison or yields 0.
     */
    private static void contractNosValidation(String prefix, String from, String to) {
        if (prefix.isEmpty()) throw new IllegalArgumentException("PrefixField Is Required");
        if (from.isEmpty() && asInt(from) == 0) throw new IllegalArgumentException("SerialFrom Field Is Required");
        if (to.isEmpty() && asInt(to) == 0) throw new IllegalArgumentException("Serial To Field Is Required");
        if (asInt(to) < asInt(from)) throw new IllegalArgumentException("Serial To Can not Be Less than Serial From");
    }

    /**
     * btnsave_Click -> Insert() (RecId = 0) or btnUpdate_Click -> DeleteAndInsert() ("update": true).
     * One GenerateExportInvoiceNos detail per serial From..To: DocumentTypeId 223, IsActive true, PrefixTypeId =
     * the combo (not validated), PreFix, SerialFrom/To, FinalInvoiceNo = PreFix + "-" + i, MainId =
     * GetMainId() on save (the desktop calls it once per serial; nothing is written between the calls, so one
     * call gives the same value) or the loaded MainId on update. The header MainId the DAL validates/deletes
     * with is 0 on save (never set by Insert()) and the loaded MainId on update.
     */
    public Map<String, Object> saveContractNos(Map<String, Object> body) {
        UserAccount u = user(PARENTS_CONTRACT_NOS);
        boolean update = asBool(body.get("update"));
        String prefix = raw(body.get("prefix")), from = raw(body.get("serialFrom")), to = raw(body.get("serialTo"));
        contractNosValidation(prefix, from, to);
        int prefixTypeId = asInt(body.get("prefixTypeId"));
        int loadedMainId = asInt(body.get("mainId"));
        int detailMainId = update ? loadedMainId : repo.mainId(u);
        int headerMainId = update ? loadedMainId : 0;
        Timestamp now = now();
        List<Map<String, Object>> details = new ArrayList<>();
        int f = asInt(from), t = asInt(to);
        for (int i = f; i <= t; i++) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("MainId", detailMainId);
            d.put("Id", 0);
            d.put("PreFix", prefix);
            d.put("SerialFrom", f);
            d.put("SerialTo", t);
            d.put("FinalInvoiceNo", prefix + "-" + i);
            d.put("IsActive", true);
            d.put("OrganizationId", u.getOrganizationId());
            d.put("CompanyId", u.getCompanyId());
            d.put("DocumentTypeId", CONTRACT_NOS_DOCUMENT_TYPE_ID);
            d.put("PrefixTypeId", prefixTypeId);
            d.put("EntryUserId", u.getId());
            d.put("EntryDate", now);
            d.put("ModifyUserId", u.getId());
            d.put("ModifyDate", now);
            details.add(d);
        }
        int id = repo.saveExportInvoiceNos(u.getOrganizationId(), u.getCompanyId(), headerMainId, details);
        String message = update ? "Update Successfully" : (id > 0 ? "Save Successfully" : "");
        return saved(id, message);
    }

    /** DefineExportPrefixType.GridFill: Id / Description. */
    public List<Map<String, Object>> prefixTypes() {
        user(PARENTS_CONTRACT_NOS);
        return project(repo.exportPrefixTypes(), "Id", "Id", "Description", "PrefixDescription");
    }

    /**
     * DefineExportPrefixType.Insert: "Description Field Required" when blank; Update (btnUpdate_Click) needs a
     * RecId ("RecId not Found"); usp_ExportInvoiceNosPrefix_Insert @Id @PrefixDescription (the untrimmed text).
     */
    public Map<String, Object> savePrefixType(Map<String, Object> body) {
        user(PARENTS_CONTRACT_NOS);
        boolean update = asBool(body.get("update"));
        int recId = update ? asInt(body.get("recId")) : 0;
        if (update && recId == 0) throw new IllegalArgumentException("RecId not Found");
        String desc = raw(body.get("description"));
        if (desc.trim().isEmpty()) throw new IllegalArgumentException("Description Field Required");
        int id = repo.saveExportPrefixType(recId, desc);
        return saved(id, recId > 0 ? "Record Update Successfully" : "Record Save Successfully");
    }
}
