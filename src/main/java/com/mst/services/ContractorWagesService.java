package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

/**
 * Contractor Wages - Wages Account screen.
 *
 * Ported from Architecture.WinApp.Contractor_Wages\frmContractorWagesAccount.cs. Every statement
 * below is the stored procedure the desktop calls for the same purpose; no table is written to
 * directly and no procedure was added.
 *
 *   Grid / list   Sp_InvConractorWagesAccounts_GetAllMethod  @Activity='ReadAll'
 *                 (InvConractorWagesAccounts.GetAll, BLL :29-62; FillGrid(), form :270)
 *   Read one      Sp_InvConractorWagesAccounts_GetAllMethod  @Activity='ReadById'   (BLL :64-85)
 *   Insert        Sp_InvConractorWagesAccounts_Insert        (BLL :15-19, when Id == 0)
 *   Update        Sp_InvConractorWagesAccounts_Update        (BLL :20, when Id != 0)
 *   GL Account    Sp_COAAllocation_GetAllMethod  @Activity='GetAccountTitleByAccountTypeIds'
 *                 (GLAcoountBind(), form :126-141)
 *   Wages Type    Sp_InvLookup_GetAllMethod      @Activity='ReadByInvlookTypeId', type 10
 *                 (GetLookupsByTypeIdDt(), form :142-157)
 *   Activity      USP_DropDownActivityWages      @Activity='ReadAll'
 *                 (GetActivityNature(), form :158-181)
 *
 * The desktop grid sets AllowEdit and AllowDelete to False (GridSetting(), form :436-437), so this
 * screen has no delete operation. None is offered here either.
 */
@Service
public class ContractorWagesService {

    private static final org.slf4j.Logger log =
            org.slf4j.LoggerFactory.getLogger(ContractorWagesService.class);

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /* ------------------------------------------------------------------ statements */

    /** GLAcoountBind() passes AccountTypeIds "11,12,13,20,21" (form :133). @UserId must be non-zero
     *  or the procedure raises an error, and @AppId is passed as 1 — the same values
     *  PurchaseOrderFullService already uses successfully against this procedure.
     *  Columns returned: Id, AccountTitle, AccountCode. */
    private static final String SQL_GL_ACCOUNTS =
            "EXEC Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?, "
          + "@AccountTypeIds=?, @Activity=?";

    private static final String GL_ACCOUNT_TYPE_IDS = "11,12,13,20,21";

    /** CommonServices.GetLookupsByTypeIdDt(10) -> InvLookUp.GetLookupsByTypeIdDt, BLL :130-154.
     *  Columns returned include Id and LookupName. */
    private static final String SQL_WAGES_TYPES =
            "EXEC Sp_InvLookup_GetAllMethod @OrganizationId=?, @CompanyId=?, @InvLookupTypeId=?, @Activity=?";

    private static final int WAGES_TYPE_LOOKUP_TYPE_ID = 10;

    /** InvContractorWagesBillHeader.GetActivityNature, BLL :940-960.
     *  Columns returned include Id and ActivityNature. */
    private static final String SQL_ACTIVITY_NATURES =
            "EXEC USP_DropDownActivityWages @OrganizationId=?, @CompanyId=?, @Activity=?";

    private static final String SQL_WAGES_ACCOUNTS =
            "EXEC Sp_InvConractorWagesAccounts_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    private static final String SQL_WAGES_ACCOUNT_BY_ID =
            "EXEC Sp_InvConractorWagesAccounts_GetAllMethod @Id=?, @Activity=?";

    /* GenericProvider.SetDate binds one @PropertyName per non-virtual property of
     * Architecture.Model.ContractorWages.InvConractorWagesAccounts, which has exactly these twelve.
     * The parameter list is therefore fixed by the model, not chosen here. */
    private static final String SQL_WAGES_ACCOUNT_INSERT =
            "EXEC Sp_InvConractorWagesAccounts_Insert @Id=?, @OrganizationId=?, @CompanyId=?, "
          + "@WagesAccountName=?, @GlAccountId=?, @WagesLookupId=?, @WagesActivityId=?, "
          + "@EntryUserId=?, @EntryDate=?, @ModifyUserId=?, @ModifyDate=?, @IsActive=?";

    private static final String SQL_WAGES_ACCOUNT_UPDATE =
            "EXEC Sp_InvConractorWagesAccounts_Update @Id=?, @OrganizationId=?, @CompanyId=?, "
          + "@WagesAccountName=?, @GlAccountId=?, @WagesLookupId=?, @WagesActivityId=?, "
          + "@EntryUserId=?, @EntryDate=?, @ModifyUserId=?, @ModifyDate=?, @IsActive=?";

    /* ------------------------------------------------------------------ helpers */

    /** Result-set keys are read case-insensitively. Procedure column casing is not something to
     *  assume: ADO.NET's DataTable indexer is case-insensitive where a Java Map is not, and that
     *  mismatch is what silently blanked columns on earlier screens in this port. */
    private static Object col(Map<String, Object> row, String name) {
        Object v = row.get(name);
        if (v != null) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Map<String, Object> row, String name) {
        Object v = col(row, name);
        return v == null ? "" : String.valueOf(v).trim();
    }

    private static int num(Map<String, Object> row, String name) {
        Object v = col(row, name);
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (Exception e) { return 0; }
    }

    private static boolean flag(Map<String, Object> row, String name) {
        Object v = col(row, name);
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "True".equals(s);
    }

    /** Maps {id, text} for a dropdown, reading the value column case-insensitively. An empty list
     *  is returned rather than a placeholder, so the screen can say the list is empty instead of
     *  offering an invented option. */
    private List<Map<String, Object>> asOptions(List<Map<String, Object>> rows,
                                                String idCol, String textCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", num(r, idCol));
            m.put("text", str(r, textCol));
            out.add(m);
        }
        return out;
    }

    /* ------------------------------------------------------------------ dropdowns */

    public List<Map<String, Object>> getGlAccounts() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_GL_ACCOUNTS,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId(),
                    1,
                    GL_ACCOUNT_TYPE_IDS,
                    "GetAccountTitleByAccountTypeIds");
            return asOptions(rows, "Id", "AccountTitle");
        } catch (Exception e) {
            log.error("Wages Account: GL Account dropdown failed (Sp_COAAllocation_GetAllMethod, "
                    + "AccountTypeIds={})", GL_ACCOUNT_TYPE_IDS, e);
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getWagesTypes() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_WAGES_TYPES,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    WAGES_TYPE_LOOKUP_TYPE_ID,
                    "ReadByInvlookTypeId");
            return asOptions(rows, "Id", "LookupName");
        } catch (Exception e) {
            log.error("Wages Account: Wages Type dropdown failed (Sp_InvLookup_GetAllMethod, "
                    + "InvLookupTypeId={})", WAGES_TYPE_LOOKUP_TYPE_ID, e);
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getActivityNatures() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_ACTIVITY_NATURES,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    "ReadAll");
            return asOptions(rows, "Id", "ActivityNature");
        } catch (Exception e) {
            log.error("Wages Account: Activity Nature dropdown failed (USP_DropDownActivityWages)", e);
            return Collections.emptyList();
        }
    }

    /* ------------------------------------------------------------------ grid */

    /** FillGrid(), form :270-302. The desktop projects the procedure's rows into the grid columns
     *  Id, AccountName, ActivityType, ActivityNature, GlAccount, GlAccountId, WagesLookupId,
     *  IsActive — reading WagesAccountName, WagesType, ActivityNature, AccountTitle from the
     *  result set. The same projection is done here so the web grid shows the same columns.
     *
     *  wagesActivityId is carried additionally: the desktop's row-load assigns the activity NAME
     *  to a value-bound combo (form :380), which only works because an UltraCombo also matches on
     *  text. Carrying the id lets the web select it properly when the procedure supplies one, and
     *  the name is kept as the fallback for when it does not. */
    public List<Map<String, Object>> getWagesAccounts() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_WAGES_ACCOUNTS,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    "ReadAll");
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : rows) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", num(r, "Id"));
                m.put("accountName", str(r, "WagesAccountName"));
                m.put("activityType", str(r, "WagesType"));
                m.put("activityNature", str(r, "ActivityNature"));
                m.put("glAccount", str(r, "AccountTitle"));
                m.put("glAccountId", num(r, "GlAccountId"));
                m.put("wagesLookupId", num(r, "WagesLookupId"));
                m.put("wagesActivityId", num(r, "WagesActivityId"));
                m.put("isActive", flag(r, "IsActive"));
                out.add(m);
            }
            return out;
        } catch (Exception e) {
            log.error("Wages Account: list failed (Sp_InvConractorWagesAccounts_GetAllMethod "
                    + "@Activity='ReadAll')", e);
            return Collections.emptyList();
        }
    }

    public Map<String, Object> getWagesAccountById(int id) {
        try {
            List<Map<String, Object>> rows =
                    jdbcTemplate.queryForList(SQL_WAGES_ACCOUNT_BY_ID, id, "ReadById");
            if (rows.isEmpty()) return Collections.emptyMap();
            Map<String, Object> r = rows.get(0);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", num(r, "Id"));
            m.put("accountName", str(r, "WagesAccountName"));
            m.put("glAccountId", num(r, "GlAccountId"));
            m.put("wagesLookupId", num(r, "WagesLookupId"));
            m.put("wagesActivityId", num(r, "WagesActivityId"));
            m.put("activityNature", str(r, "ActivityNature"));
            m.put("isActive", flag(r, "IsActive"));
            return m;
        } catch (Exception e) {
            log.error("Wages Account: read by id {} failed", id, e);
            return Collections.emptyMap();
        }
    }

    /* ------------------------------------------------------------------ save */

    /**
     * btnsave_Click (form :205-245) and UpdateWagesAccount (form :304-346) build the identical
     * payload; the only difference is that Update sets Id = recId. InvConractorWagesAccounts.Save
     * then picks Insert or Update on Id == 0 (BLL :15-21), which is reproduced here.
     *
     * EntryDate and ModifyDate are both DateTime.Now in the desktop, on insert and on update alike.
     */
    public Map<String, Object> saveWagesAccount(Map<String, Object> body) {
        Map<String, Object> res = new LinkedHashMap<>();

        int id = toInt(body.get("id"));
        String accountName = body.get("accountName") == null ? "" : String.valueOf(body.get("accountName")).trim();
        int glAccountId = toInt(body.get("glAccountId"));
        int wagesLookupId = toInt(body.get("wagesLookupId"));
        int wagesActivityId = toInt(body.get("wagesActivityId"));
        boolean isActive = Boolean.TRUE.equals(body.get("isActive"))
                || "true".equalsIgnoreCase(String.valueOf(body.get("isActive")));

        /* FormValidation(), form :182-204 — same three checks, same order, same wording.
         * Activity Nature is deliberately NOT validated: the desktop does not require it. */
        String error = null;
        if (accountName.isEmpty())      error = "Account Name Field Required";
        else if (glAccountId <= 0)      error = "GL Account Field is Required";
        else if (wagesLookupId <= 0)    error = "WagesType Field is Required";

        if (error != null) {
            res.put("success", false);
            res.put("message", error);
            return res;
        }

        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();
            java.sql.Timestamp now = new java.sql.Timestamp(System.currentTimeMillis());

            boolean isUpdate = id > 0;
            jdbcTemplate.update(isUpdate ? SQL_WAGES_ACCOUNT_UPDATE : SQL_WAGES_ACCOUNT_INSERT,
                    id, orgId, compId, accountName, glAccountId, wagesLookupId, wagesActivityId,
                    userId, now, userId, now, isActive);

            res.put("success", true);
            /* The desktop's own wording, form :238 and :339. */
            res.put("message", isUpdate ? "Record Update Successfully" : "Record Saved Successfully");
            return res;
        } catch (Exception e) {
            log.error("Wages Account: save failed (id={}, name={})", id, accountName, e);
            res.put("success", false);
            res.put("message", e.getMessage() == null ? "The server rejected the save." : e.getMessage());
            return res;
        }
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (Exception e) { return 0; }
    }
}
