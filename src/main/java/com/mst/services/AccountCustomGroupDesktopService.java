package com.mst.services;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 1 "Account Custom Group" - desktop form
 * Architecture.WinApp.Account_Definition.frmAccountCustomGroup, reproduced call for call.
 *
 * <ul>
 * <li>GridBind (left grid grdLookUp): AcLookUps.GetAll -> Sp_AcLookUps_GetAllMethod
 *     @OrganizationId, @CompanyId, @AcLookUpTypesId=1 (TypeId != 0), @Activity='ReadAll'.</li>
 * <li>LookUpBind (cmbGroupName): AcLookUps.GetByProfileTypeId -> Sp_AcLookUps_GetAllMethod
 *     @OrganizationId, @CompanyId, @Activity='ReadByAcLookUpId', @AcLookUpTypesId=1.</li>
 * <li>AccountTypeCombo (cmbAccountTypes): AccountTypes.GetAll(new AccountTypes()) ->
 *     Proc_AccountTypes_ReadAll @OrganizationId=0, @CompanyId=0 (a fresh model: both ints are 0
 *     and both are always added).</li>
 * <li>ThirdLevelAccountsDbCall (CmbParentAccount): ChartofAccount.ReadAllAccountgroup ->
 *     Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId, @CompanyId, @FinancialYearId,
 *     @Account_Level=3, @CoaType='ReadAllAccountGroup'; the AccountTypeId filter is applied to
 *     the cached rows on the client (BindThirdLevelAccounts), not sent to SQL.</li>
 * <li>Insert/UpdateLookUp: AcLookUps.Save -> GenericProvider.SetProc sends every model property:
 *     @AcLookUpTypesId=1, @Id (0 on insert), @OrganizationId, @CompanyId, @AcLookUpsDescription
 *     (txtGroupName.Text as typed - Validation() trims only for the empty check) to
 *     Sp_AcLookUps_Insert when Id == 0, else Sp_AcLookUps_Update.</li>
 * <li>BtnShow: USP_AccountCustomGroup_UnAllocatedData then USP_AccountCustomGroup_AllocatedData,
 *     @OrganizationId, @CompanyId, @CustomGroupId always; @AccountTypeId / @ParentAccountId only
 *     when non-zero.</li>
 * <li>SaveAccountCustomGroup (Allocate): one transaction, Sp_AccountsCustomGroups_Insert per
 *     checked row with @AcLookUpsId, @ChartOfAccountId, @SortNo=0, @companyId, @EntryUserId,
 *     @ModifyUserId, @organizationId.</li>
 * <li>btnDelete (Un-Allocate): ChartofAccount.DeleteAccountsFromCustomGroup - per row, each on
 *     its own connection (no transaction): Sp_ChartofAccount_GetAllMethodFromCOA @AcLookUpsId
 *     (the row's own AcLookUpsId), @Id (ChartOfAccountId), @CoaType='DeleteAccountsFromCustomGroup'.</li>
 * </ul>
 * The form checks no rights and has no delete for a group itself.
 */
@Service
public class AccountCustomGroupDesktopService {

    public static final int CUSTOM_GROUP_TYPE_ID = 1;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public AccountCustomGroupDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    /** GridBind(): the left grid. */
    public List<Map<String, Object>> gridGroups() {
        return DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AcLookUpTypesId", CUSTOM_GROUP_TYPE_ID,
                "Activity", "ReadAll"));
    }

    /** LookUpBind(): cmbGroupName. */
    public List<Map<String, Object>> comboGroups() {
        return DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadByAcLookUpId",
                "AcLookUpTypesId", CUSTOM_GROUP_TYPE_ID));
    }

    /** AccountTypeCombo(): cmbAccountTypes. */
    public List<Map<String, Object>> accountTypes() {
        return DesktopProc.rows(jdbc, "Proc_AccountTypes_ReadAll", DesktopProc.params(
                "OrganizationId", 0,
                "CompanyId", 0));
    }

    /** ThirdLevelAccountsDbCall(): all 3rd-level group accounts (filtered by type on the client). */
    public List<Map<String, Object>> thirdLevelAccounts() {
        return DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "Account_Level", 3,
                "CoaType", "ReadAllAccountGroup"));
    }

    /** grdLookUp_DoubleClick: AcLookUps.GetById -> @Id, @Activity='ReadById', first row. */
    public Map<String, Object> groupById(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod",
                DesktopProc.params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        return rows.get(0);
    }

    /**
     * InsertLookUp / UpdateLookUp. Returns the desktop's message, or null when Success was 0
     * (the desktop then shows nothing).
     */
    @Transactional
    public String saveGroup(int recId, String groupName) {
        String text = groupName == null ? "" : groupName;
        if (text.trim().isEmpty()) throw new IllegalArgumentException("Group Name Required");
        String proc = recId == 0 ? "Sp_AcLookUps_Insert" : "Sp_AcLookUps_Update";
        int num = DesktopProc.setProc(jdbc, proc, DesktopProc.params(
                "AcLookUpTypesId", CUSTOM_GROUP_TYPE_ID,
                "Id", recId,
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AcLookUpsDescription", text));
        if (num <= 0) num = recId;           // DAL: num > 0 ? num : obj.Id
        if (num <= 0) return null;
        return recId == 0 ? "Record Saved Successfully" : "Record Update Successfully";
    }

    /** BtnShow_Click: [0] = UnAllocatedData (left grid), [1] = AllocatedData (right grid). */
    public List<List<Map<String, Object>>> show(int customGroupId, int accountTypeId, int parentAccountId) {
        List<List<Map<String, Object>>> out = new ArrayList<>();
        out.add(DesktopProc.rows(jdbc, "[dbo].[USP_AccountCustomGroup_UnAllocatedData]",
                showParams(customGroupId, accountTypeId, parentAccountId)));
        out.add(DesktopProc.rows(jdbc, "[dbo].[USP_AccountCustomGroup_AllocatedData]",
                showParams(customGroupId, accountTypeId, parentAccountId)));
        return out;
    }

    private Map<String, Object> showParams(int customGroupId, int accountTypeId, int parentAccountId) {
        return DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "CustomGroupId", customGroupId,
                "AccountTypeId", accountTypeId != 0 ? accountTypeId : null,
                "ParentAccountId", parentAccountId != 0 ? parentAccountId : null);
    }

    /** SaveAccountCustomGroup(): true when the last insert returned > 0 ("Saved Successfully"). */
    @Transactional
    public boolean allocate(int customGroupId, List<Integer> chartOfAccountIds) {
        if (chartOfAccountIds == null || chartOfAccountIds.isEmpty()) throw new IllegalArgumentException("Checked row first");
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        int user = ctx.currentUserId();
        int result = 0;
        for (Integer coaId : chartOfAccountIds) {
            result = DesktopProc.setProc(jdbc, "Sp_AccountsCustomGroups_Insert", DesktopProc.params(
                    "AcLookUpsId", customGroupId,
                    "ChartOfAccountId", coaId == null ? 0 : coaId,
                    "SortNo", 0,
                    "companyId", company,
                    "EntryUserId", user,
                    "ModifyUserId", user,
                    "organizationId", org));
        }
        return result > 0;
    }

    /** btnDelete_Click: each row carries its own AcLookUpsId (the grid's hidden "Id" cell). */
    public void unAllocate(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Checked row first");
        for (Map<String, Object> r : rows) {
            DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                    "AcLookUpsId", toInt(r.get("acLookUpsId")),
                    "Id", toInt(r.get("chartOfAccountId")),
                    "CoaType", "DeleteAccountsFromCustomGroup"));
        }
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
