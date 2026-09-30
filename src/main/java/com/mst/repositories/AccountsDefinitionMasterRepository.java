package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * DAL of the six ported screens of AppModules 45 "Accounts Definition" (App 19 "Master Data Definition"):
 *
 *   427 PDC Bank            PdcBank.cs               BLL 0075 PdcBankName            Sp_PdcBankName_GetAllMethod / _Insert|_Update (4)
 *   429 Document Group      DocumentGroup.cs         BLL 0478 ExImShipmentDocGroup   Sp_ExImShipmentDocGroup_GetAllMethod / _Insert|_Update (5)
 *   430 Shipment Documents  ExImShipmentDocuments.cs BLL 0480 ExImShipmentDocuments  Sp_ExImShipmentDocuments_GetAllMethod / _Insert|_Update (3)
 *   428 Tax Lookup          Lookups/TaxLookup.cs     BLL 0605 TaxLookUps, 0606 TaxLookUptypes, InvLookUp.GenerateCode
 *                                                     Sp_TaxLookUps_GetAllMethod / _Insert|_Update (7), Sp_TaxLookUptypes_GetAllMethod,
 *                                                     Proc_InvLookUpType_GenerateCodeByLookUpType
 *   414 BsPl Settings Form  BsPlSettingForm.cs       BLL 0650 PLBSSetting, 0624 AccountNotes
 *                                                     Sp_PLBSSetting_GetAllMethod GetAllForBsPlSetting, Sp_PLBSSetting_Insert (11, one per row),
 *                                                     SP_AccountNotes_ReadAllMethodBySPType (ReadAllMethodByPLNote / ByBSNote / ReadAllNotes / UpdateTitleById)
 *   413 Bank                AcfrmDefineBank.cs       BLL 0057 Bank                    Sp_Bank_GetAllMethod ReadAllBankDT / ReadById, Sp_Bank_Insert|Update (27)
 *                                                     combos: Country GetAll, City GetAll, SpStaticColumnNames 'ChequeTempletes',
 *                                                     Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds' @AccountTypeIds='15'
 *
 * Every parameter list is the desktop model, every property sent (GenericProvider.SetProc), checked against
 * procdure.utf8.sql / procdure_index.csv on 2026-09-30. DesktopProc prefixes '@' itself, so names are bare and
 * null values are not sent (ADO.NET). No table, column or procedure is created or changed.
 */
@Repository
public class AccountsDefinitionMasterRepository {

    private final JdbcTemplate jdbc;

    public AccountsDefinitionMasterRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ 427 PDC Bank

    public List<Map<String, Object>> pdcBanks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_PdcBankName_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> pdcBank(int id) {
        return DesktopProc.rows(jdbc, "Sp_PdcBankName_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int savePdcBank(Map<String, Object> m) { return save("Sp_PdcBankName_Insert", "Sp_PdcBankName_Update", m); }

    // ------------------------------------------------------------------ 429 Document Group

    public List<Map<String, Object>> docGroups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocGroup_GetAllMethod",
                params("Activity", "ReadByCompanyOrganizationId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> docGroup(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocGroup_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveDocGroup(Map<String, Object> m) { return save("Sp_ExImShipmentDocGroup_Insert", "Sp_ExImShipmentDocGroup_Update", m); }

    // ------------------------------------------------------------------ 430 Shipment Documents

    /** ExImShipmentDocuments.Getall() - @Activity 'ReadAll' only: the table is global (no tenancy). */
    public List<Map<String, Object>> shipmentDocs() {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocuments_GetAllMethod", params("Activity", "ReadAll"));
    }

    public List<Map<String, Object>> shipmentDoc(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImShipmentDocuments_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveShipmentDoc(Map<String, Object> m) { return save("Sp_ExImShipmentDocuments_Insert", "Sp_ExImShipmentDocuments_Update", m); }

    // ------------------------------------------------------------------ 428 Tax Lookup

    /** TaxLookUptypes.Getall - 'ReadAll' (Id, LookupTypeName). */
    public List<Map<String, Object>> taxLookupTypes() {
        return DesktopProc.rows(jdbc, "Sp_TaxLookUptypes_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** TaxLookUps.Getall - 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> taxLookups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_TaxLookUps_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    public List<Map<String, Object>> taxLookup(int id) {
        return DesktopProc.rows(jdbc, "Sp_TaxLookUps_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /**
     * cmbProfileName_Leave -> InvLookUp.GenerateCode: Proc_InvLookUpType_GenerateCodeByLookUpType with the TAX lookup
     * type's id as @InvLookupTypeId - the desktop reads the INVENTORY lookup-type table with it (reproduced).
     * Returns Conversion.ToInt(rows[0]["Code"]), 0 when nothing came back.
     */
    public int generateTaxLookupCode(UserAccount u, int lookupTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Proc_InvLookUpType_GenerateCodeByLookUpType",
                params("OrganizationId", u.getOrganizationId(), "CompanyID", u.getCompanyId(), "InvLookupTypeId", lookupTypeId));
        if (r.isEmpty()) return 0;
        Object v = ci(r.get(0), "Code");
        return asInt(v);
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveTaxLookup(Map<String, Object> m) { return save("Sp_TaxLookUps_Insert", "Sp_TaxLookUps_Update", m); }

    // ------------------------------------------------------------------ 414 BS & PL Setting

    /** PLBSSetting.GetAllForBsPlSetting: @OrganizationId @CompanyId @Note ('PL'/'BS') [@AccountClassId when != 0] @Activity. */
    public List<Map<String, Object>> bsPlAccounts(UserAccount u, String note, int accountClassId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Note", note);
        if (accountClassId != 0) p.put("AccountClassId", accountClassId);
        p.put("Activity", "GetAllForBsPlSetting");
        return DesktopProc.rows(jdbc, "Sp_PLBSSetting_GetAllMethod", p);
    }

    /** AccountNotes.ReadByPLNote. */
    public List<Map<String, Object>> plNotes() {
        return DesktopProc.rows(jdbc, "SP_AccountNotes_ReadAllMethodBySPType", params("AccountNotesType", "ReadAllMethodByPLNote"));
    }

    /** AccountNotes.ReadByBSNote(classId) - @AccountClassId only when != 0. */
    public List<Map<String, Object>> bsNotes(int classId) {
        Map<String, Object> p = params("AccountNotesType", "ReadAllMethodByBSNote");
        if (classId != 0) p.put("AccountClassId", classId);
        return DesktopProc.rows(jdbc, "SP_AccountNotes_ReadAllMethodBySPType", p);
    }

    /** AccountNotes.ReadAllNotes(note) - @Note only when not empty. */
    public List<Map<String, Object>> allNotes(String note) {
        Map<String, Object> p = params("AccountNotesType", "ReadAllNotes");
        if (note != null && !note.isEmpty()) p.put("Note", note);
        return DesktopProc.rows(jdbc, "SP_AccountNotes_ReadAllMethodBySPType", p);
    }

    /** PLBSSetting.Save -> DAL SetData: one Sp_PLBSSetting_Insert per list item inside ONE transaction; the result is always 0. */
    @Transactional(rollbackFor = Exception.class)
    public int savePlBsSettings(List<Map<String, Object>> items) {
        for (Map<String, Object> item : items) DesktopProc.setProc(jdbc, "Sp_PLBSSetting_Insert", bare(item));
        return 0;
    }

    /** AccountNotes.UpdateTitleById - per item: @Id @Note @AccountNotesType='UpdateTitleById' (no transaction on the desktop). */
    public int updateNoteTitles(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> item : items) {
            List<Map<String, Object>> r = DesktopProc.rows(jdbc, "SP_AccountNotes_ReadAllMethodBySPType",
                    params("Id", item.get("id"), "Note", item.get("noteTitle"), "AccountNotesType", "UpdateTitleById"));
            if (!r.isEmpty()) result = asInt(r.get(0).values().iterator().next());
        }
        return result;
    }

    // ------------------------------------------------------------------ 413 Bank (AcfrmDefineBank)

    /** Bank.GetAllDt - 'ReadAllBankDT'. */
    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllBankDT"));
    }

    /** Bank.GetByID - @Id, 'ReadById'. */
    public List<Map<String, Object>> bank(int id) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveBank(Map<String, Object> m) { return save("Sp_Bank_Insert", "Sp_Bank_Update", m); }

    /** CountryComboFill - country.GetAll with the session's ids (the GetAll branch ignores them). */
    public List<Map<String, Object>> countries(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** CityFill - City.GetAll 'GetAll' (CityName). */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_City_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** ChequeTempleteFill - CommonServices.StaticColumnsService("ChequeTempletes") -> SpStaticColumnNames (Id, type). */
    public List<Map<String, Object>> chequeTemplates() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "ChequeTempletes"));
    }

    /**
     * CombGlAccountNameFill - CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15") -> COAAllocation
     * .GetAccountTitleByAccountTypeIds: @OrganizationId @CompanyId @AppId (0, the ReportsParameters default)
     * @AccountTypeIds '15' @Activity; @UserId / @CostCenterId / @NotReferred / @RecId / class ids omitted (0 or empty).
     */
    public List<Map<String, Object>> bankGlAccounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", 0,
                       "AccountTypeIds", "15", "Activity", "GetAccountTitleByAccountTypeIds"));
    }

    // ------------------------------------------------------------------ plumbing

    /** BLL Save: Id == 0 -> Insert, else Update; DAL SetData -> Convert.ToInt32(ExecuteScalar), 0 when nothing came back. */
    private int save(String insert, String update, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, asInt(model.get("@Id")) == 0 ? insert : update, bare(model));
    }

    private static Map<String, Object> bare(Map<String, Object> model) {
        Map<String, Object> out = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            String k = e.getKey();
            out.put(k.startsWith("@") ? k.substring(1) : k, e.getValue());
        }
        return out;
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
