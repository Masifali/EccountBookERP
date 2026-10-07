package com.mst.services;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 786 "Party Custom Group" - desktop form Architecture.WinApp.Account_Definition.frmPartyCustomGroup,
 * reproduced call for call (BLL/DAL read from recovered_source/projects).
 *
 * <ul>
 * <li>GridBind (grdLookUp): AcLookUps.GetAll(ReportsParameters{TypeId=2}) -> Sp_AcLookUps_GetAllMethod
 *     @OrganizationId, @CompanyId, @AcLookUpTypesId=2 (TypeId != 0), @Activity='ReadAll'.</li>
 * <li>LookUpBind (cmbGroupName): AcLookUps.GetByProfileTypeId -> Sp_AcLookUps_GetAllMethod
 *     @OrganizationId, @CompanyId, @Activity='ReadByAcLookUpId', @AcLookUpTypesId=2.</li>
 * <li>AccountTypeCombo (cmbAccountTypes): CustomerGroup.GetAll -> Sp_CustomerGroup_GetAllMethod
 *     @Activity='ReadAll', @OrganizationId, @CompanyId.</li>
 * <li>grdLookUp_DoubleClick: AcLookUps.GetById -> Sp_AcLookUps_GetAllMethod @Id, @Activity='ReadById'.</li>
 * <li>InsertLookUp / UpdateLookUp: AcLookUps.Save -> GenericProvider.SetProc sends every model property
 *     (AcLookUpTypesId, Id, OrganizationId, CompanyId, AcLookUpsDescription) to Sp_AcLookUps_Insert when Id == 0,
 *     else Sp_AcLookUps_Update. The desktop's UpdateLookUp sends AcLookUpTypesId = 1 (not 2): the proc then
 *     rewrites the row's type, which moves the group out of this screen's list. Kept as on the desktop.</li>
 * <li>BtnShow: PartyCustomGroups_AllocatedData (-> right grid grdUnAllocated) then PartyCustomGroups_UnAllocatedData
 *     (-> left grid grdAllocated): USP_PartyCustomGroups_AllocatedData / _UnAllocatedData @OrganizationId, @CompanyId,
 *     @CustomGroupId, and @CustomerGroupId only when it is not 0.</li>
 * <li>SaveAccountCustomGroup (Allocate): PartyCustomGroups.Save -> DAL.SetData: ONE transaction, one
 *     USP_PartyCustomGroups_InsertAndUpdate per checked row with every model property (Id 0, SupplierCustomerId,
 *     CustomGroupId, SortNo 0, companyId, EntryUserId, ModifyUserId, organizationId); result = last row's.</li>
 * <li>btnDelete (Un-Allocate): PartyCustomGroups.DeletePartyCustomGroup: per row, each call on its own
 *     connection (no transaction): USP_PartyCustomGroups_DeleteById @OrganizationId, @CompanyId, @CustomGroupId,
 *     @SupplierCustomerId.</li>
 * </ul>
 * The form checks no screen rights.
 */
@Service
public class PartyCustomGroupDesktopService {

    public static final int PARTY_GROUP_TYPE_ID = 2;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public PartyCustomGroupDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    /** GridBind(): left grid. */
    public List<Map<String, Object>> gridGroups() {
        return DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AcLookUpTypesId", PARTY_GROUP_TYPE_ID,
                "Activity", "ReadAll"));
    }

    /** LookUpBind(): cmbGroupName. */
    public List<Map<String, Object>> comboGroups() {
        return DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadByAcLookUpId",
                "AcLookUpTypesId", PARTY_GROUP_TYPE_ID));
    }

    /** AccountTypeCombo(): cmbAccountTypes (CustomerGroup.GetAll). */
    public List<Map<String, Object>> partyGroups() {
        return DesktopProc.rows(jdbc, "Sp_CustomerGroup_GetAllMethod", DesktopProc.params(
                "Activity", "ReadAll",
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId()));
    }

    /** grdLookUp_DoubleClick: AcLookUps.GetById, first row of the result. */
    public Map<String, Object> groupById(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod",
                DesktopProc.params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        return rows.get(0);
    }

    /**
     * InsertLookUp / UpdateLookUp. recId == 0 is the insert (btnAdd.Text "Add"); otherwise the update.
     * Returns the desktop's message, or null when Success was 0 (the desktop then shows nothing).
     */
    @Transactional
    public String saveGroup(int recId, boolean update, String groupName) {
        String text = groupName == null ? "" : groupName;
        if (text.trim().isEmpty()) throw new IllegalArgumentException("Group Name Required");
        int id = update ? recId : 0;
        String proc = id == 0 ? "Sp_AcLookUps_Insert" : "Sp_AcLookUps_Update";
        int typeId = update ? 1 : PARTY_GROUP_TYPE_ID;      // frmPartyCustomGroup.UpdateLookUp: AcLookUpTypesId = 1 (as on the desktop)
        int num = DesktopProc.setProc(jdbc, proc, DesktopProc.params(
                "AcLookUpTypesId", typeId,
                "Id", id,
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AcLookUpsDescription", text));
        if (num <= 0) num = id;                              // DAL: num > 0 ? num : obj.Id
        if (num <= 0) return null;
        return update ? "Record Update Successfully" : "Record Saved Successfully";
    }

    /** BtnShow_Click: [0] = grdAllocated (left; USP_..._UnAllocatedData), [1] = grdUnAllocated (right; USP_..._AllocatedData). */
    public Map<String, Object> show(int customGroupId, int customerGroupId) {
        Map<String, Object> out = new LinkedHashMap<>();
        /* UnAllocatedData(...) runs first: it calls PartyCustomGroups_AllocatedData and fills grdUnAllocated */
        out.put("right", DesktopProc.rows(jdbc, "[dbo].[USP_PartyCustomGroups_AllocatedData]", showParams(customGroupId, customerGroupId)));
        /* AllocatedData(...) second: it calls PartyCustomGroups_UnAllocatedData and fills grdAllocated */
        out.put("left", DesktopProc.rows(jdbc, "[dbo].[USP_PartyCustomGroups_UnAllocatedData]", showParams(customGroupId, customerGroupId)));
        return out;
    }

    private Map<String, Object> showParams(int customGroupId, int customerGroupId) {
        return DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "CustomGroupId", customGroupId,
                "CustomerGroupId", customerGroupId != 0 ? customerGroupId : null);
    }

    /** SaveAccountCustomGroup(): true when the last insert returned > 0 ("Saved Successfully"). */
    @Transactional
    public boolean allocate(int customGroupId, List<Integer> supplierCustomerIds) {
        if (supplierCustomerIds == null || supplierCustomerIds.isEmpty()) throw new IllegalArgumentException("Checked row first");
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        int user = ctx.currentUserId();
        int result = 0;
        for (Integer sc : supplierCustomerIds) {
            result = DesktopProc.setProc(jdbc, "USP_PartyCustomGroups_InsertAndUpdate", DesktopProc.params(
                    "Id", 0,
                    "SupplierCustomerId", sc == null ? 0 : sc,
                    "CustomGroupId", customGroupId,
                    "SortNo", 0,
                    "companyId", company,
                    "EntryUserId", user,
                    "ModifyUserId", user,
                    "organizationId", org));
        }
        return result > 0;
    }

    /** btnDelete_Click -> DeletePartyCustomGroup: each row on its own connection, no transaction. */
    public void unAllocate(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Checked row first");
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        for (Map<String, Object> r : rows) {
            DesktopProc.rows(jdbc, "[dbo].[USP_PartyCustomGroups_DeleteById]", DesktopProc.params(
                    "OrganizationId", org,
                    "CompanyId", company,
                    "CustomGroupId", toInt(r.get("customGroupId")),
                    "SupplierCustomerId", toInt(r.get("supplierCustomerId"))));
        }
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
