package com.mst.services;

import com.mst.repositories.support.ProcExec;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Contractor Wages - Wages Rate Schedule.
 * Ported from Architecture.WinApp.Contractor_Wages\frmContractWagesSchedule.cs (2,248 lines) and
 * its BLL, Architecture.BLL.ContractorWages.InvContractorWagesSchedule.
 *
 * Data contract, read out of the BLL rather than guessed:
 *
 *   Insert        Sp_InvContractorWagesSchedule_Insert        (BLL Save(), Id == 0, ActionId = 1)
 *   Update        Sp_InvContractorWagesSchedule_Update        (BLL Save(), Id != 0, ActionId = 2)
 *   Grid / list   Sp_InvContractorWagesSchedule_GetAllMethod  @Activity='ReadAll'
 *   Read one      Sp_InvContractorWagesSchedule_GetAllMethod  @Activity='ReadById'
 *   Approve       Sp_InvContractorWagesSchedule_GetAllMethod  @Activity='ApproveUnApprove'
 *   Overlap check SpInvContractorWagesSchedule_Validations
 *   Account combo Sp_InvConractorWagesAccounts_GetAllMethod   @Activity='ReadAll', @ActionId=1
 *                 (accountName(), form :229-268)
 *
 * The Insert/Update parameter list is NOT a guess: GenericProvider.SetProc binds exactly one
 * @PropertyName per non-virtual property of Architecture.Model.ContractorWages.InvContractorWagesSchedule,
 * which declares these seventeen and no others.
 */
@Service
public class ContractorWagesScheduleService {

    private static final Logger LOG = LoggerFactory.getLogger(ContractorWagesScheduleService.class);

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** accountName(), form :239-244 - the account combo is filtered with @ActionId = 1. */
    private static final String SQL_WAGES_ACCOUNTS =
            "EXEC Sp_InvConractorWagesAccounts_GetAllMethod @OrganizationId=?, @CompanyId=?, @ActionId=?, @Activity=?";

    /** BindgrdWagesSchedule(), form :654-660 - the form grid is read with @ActionId = 2. */
    private static final String SQL_SCHEDULES_BY_ACCOUNT =
            "EXEC Sp_InvContractorWagesSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, "
          + "@InvConractorWagesAccountsId=?, @ActionId=?, @Activity=?";

    private static final String SQL_SCHEDULE_BY_ID =
            "EXEC Sp_InvContractorWagesSchedule_GetAllMethod @Id=?, @Activity=?";

    /* Seventeen properties on the model = seventeen parameters. Order follows the model. */
    private static final String SQL_SCHEDULE_INSERT =
            "EXEC Sp_InvContractorWagesSchedule_Insert @EffectedDate=?, @EffectedDateTo=?, @RateUom=?, "
          + "@WageRate=?, @PackUomFrom=?, @PackUomTo=?, @CompanyRate=?, @CompanyId=?, @ContractorId=?, "
          + "@Id=?, @InvConractorWagesAccountsId=?, @OrganizationId=?, @ActionId=?, @EntryUserId=?, "
          + "@EntryDate=?, @ModifyUserId=?, @ModifyDate=?";

    private static final String SQL_SCHEDULE_UPDATE =
            "EXEC Sp_InvContractorWagesSchedule_Update @EffectedDate=?, @EffectedDateTo=?, @RateUom=?, "
          + "@WageRate=?, @PackUomFrom=?, @PackUomTo=?, @CompanyRate=?, @CompanyId=?, @ContractorId=?, "
          + "@Id=?, @InvConractorWagesAccountsId=?, @OrganizationId=?, @ActionId=?, @EntryUserId=?, "
          + "@EntryDate=?, @ModifyUserId=?, @ModifyDate=?";

    /** ApproveUnApprove, BLL :312-336. */
    private static final String SQL_APPROVE =
            "EXEC Sp_InvContractorWagesSchedule_GetAllMethod @Id=?, @ReqType=?, @CompanyId=?, "
          + "@EntryUserId=?, @OrganizationId=?, @Activity=?";

    /** ComboBindFromWagesSchedule(), form :291-360 - one call returns both lists, split on Activity. */
    private static final String SQL_HISTORY_DROPDOWNS =
            "EXEC [dbo].[USP_GetDataForDropDownFromWagesSchedule] @OrganizationId=?, @CompanyId=?";

    // ---------------------------------------------------------------- lookups

    /** accountName(), form :229-268. Id + WagesAccountName, header "Account Name". */
    public List<Map<String, Object>> getWagesAccounts() {
        try {
            return jdbcTemplate.queryForList(SQL_WAGES_ACCOUNTS,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(), 1, "ReadAll");
        } catch (Exception e) {
            LOG.error("Wages account list failed", e);
            return Collections.emptyList();
        }
    }

    /**
     * History-tab combos. The desktop makes ONE call and splits the rows on the Activity column
     * into "Contractor" and "WagesAccount" (form :328-346), so that split happens here too rather
     * than inventing two endpoints.
     */
    public Map<String, Object> getHistoryDropdowns() {
        Map<String, Object> out = new HashMap<>();
        List<Map<String, Object>> contractors = new ArrayList<>();
        List<Map<String, Object>> wagesAccounts = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_HISTORY_DROPDOWNS,
                    currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId())) {
                String activity = str(col(r, "Activity"));
                Map<String, Object> row = new HashMap<>();
                row.put("Id", col(r, "Id"));
                row.put("name", col(r, "ReferenceName"));
                if ("Contractor".equals(activity)) contractors.add(row);
                else if ("WagesAccount".equals(activity)) wagesAccounts.add(row);
            }
        } catch (Exception e) {
            LOG.error("Wages schedule history dropdowns failed", e);
        }
        out.put("contractors", contractors);
        out.put("wagesAccounts", wagesAccounts);
        return out;
    }

    // ---------------------------------------------------------------- grid

    /** BindgrdWagesSchedule(WagesAccountId), form :643-704. */
    public List<Map<String, Object>> getSchedulesByAccount(int wagesAccountId) {
        try {
            return jdbcTemplate.queryForList(SQL_SCHEDULES_BY_ACCOUNT,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    wagesAccountId, 2, "ReadAll");
        } catch (Exception e) {
            LOG.error("Wages schedule grid failed for account {}", wagesAccountId, e);
            return Collections.emptyList();
        }
    }

    /** ReadbyId(Id), form :585-614. */
    public Map<String, Object> getScheduleById(int id) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_SCHEDULE_BY_ID, id, "ReadById");
            return rows.isEmpty() ? Collections.emptyMap() : rows.get(0);
        } catch (Exception e) {
            LOG.error("Wages schedule read failed for id {}", id, e);
            return Collections.emptyMap();
        }
    }

    // ---------------------------------------------------------------- save

    /**
     * Insert(), form :518-566. One entry point, exactly like the desktop: btnsave_Click zeroes RecId
     * first so Save is always an insert, btnUpdate_Click keeps it so Update is always an update.
     *
     * Two assignments in the desktop look like duplicates and are deliberate - they are reproduced
     * verbatim rather than "corrected":
     *   wagesSchedule.RateUom      = txtPackUOMFrom.Text   (form :541, same box as PackUomFrom)
     *   wagesSchedule.CompanyRate  = txtwagesrate.Text     (form :544, same box as WageRate)
     */
    public Map<String, Object> saveSchedule(Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();

        int id = asInt(body.get("id"));
        int wagesAccountId = asInt(body.get("wagesAccountId"));
        double packUomFrom = asDouble(body.get("packUomFrom"));
        double packUomTo = asDouble(body.get("packUomTo"));
        double wagesRate = asDouble(body.get("wagesRate"));
        String effectedDate = str(body.get("effectedDate"));
        String effectedDateTo = str(body.get("effectedDateTo"));

        // FormValidation(), form :393-420 - messages verbatim.
        List<String> errors = new ArrayList<>();
        if (wagesAccountId == 0)                errors.add("Account Field is Required");
        else if (packUomFrom == 0.0)            errors.add("PackUOMFrom Field Required");
        else if (packUomTo == 0.0)              errors.add("PackUOMTo Field Required");
        else if (wagesRate == 0.0)              errors.add("Wages Rate Field Required");
        if (!errors.isEmpty()) {
            res.put("success", false);
            res.put("message", errors.get(0));
            return res;
        }

        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        Timestamp now = new Timestamp(System.currentTimeMillis());
        boolean isUpdate = id > 0;
        int actionId = isUpdate ? 2 : 1;                       // BLL Save() :20-28

        try {
            Object[] args = new Object[] {
                    date(effectedDate), date(effectedDateTo),
                    packUomFrom,        // @RateUom     - form :541, the PackUomFrom box
                    wagesRate,          // @WageRate
                    packUomFrom,        // @PackUomFrom
                    packUomTo,          // @PackUomTo
                    wagesRate,          // @CompanyRate - form :544, the Wages Rate box
                    compId,
                    0,                  // @ContractorId - this form never sets one; the
                                        // Contractor-Wise schedule form is the one that does
                    id, wagesAccountId, orgId, actionId,
                    userId, now, userId, now
            };
            jdbcTemplate.update(isUpdate ? SQL_SCHEDULE_UPDATE : SQL_SCHEDULE_INSERT, args);
            res.put("success", true);
            res.put("message", isUpdate ? "Data Update Successfully." : "Data Save Successfully.");
        } catch (Exception e) {
            LOG.error("Wages schedule save failed", e);
            res.put("success", false);
            res.put("message", e.getMessage());
        }
        return res;
    }

    /**
     * ApproveUnApprove, form :987 / BLL :312-336. The desktop only ever approves from this grid -
     * grdWagesSchedule_ColumnButtonClick :631 fires only when the row is NOT already approved - so
     * no un-approve path is exposed here.
     */
    public Map<String, Object> approve(int id, int rowEntryUserId) {
        Map<String, Object> res = new HashMap<>();
        try {
            ProcExec.call(jdbcTemplate, SQL_APPROVE, id, "Approve",
                    currentUserContext.currentCompanyId(), rowEntryUserId,
                    currentUserContext.currentOrganizationId(), "ApproveUnApprove");
            res.put("success", true);
        } catch (Exception e) {
            LOG.error("Wages schedule approve failed for id {}", id, e);
            res.put("success", false);
            res.put("message", e.getMessage());
        }
        return res;
    }

    // ================================================================
    // Wages Rate Schedule CONTRACTOR WISE
    // frmContractWiseWagesSchedule.cs - same model and same procedures as above; what differs is
    // that it sets ContractorId, keeps Company Rate in its own box, and reads the grid with
    // @ActionId = 1 instead of 2 (:581-587).
    // ================================================================

    /** ContractorFill(), form :221-256 - Sp_SupplierCustomer_GetAllMethod, contractor-wages activity. */
    private static final String SQL_CONTRACTORS =
            "EXEC Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    private static final String SQL_SCHEDULES_CONTRACTOR_WISE =
            "EXEC Sp_InvContractorWagesSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, "
          + "@InvConractorWagesAccountsId=?, @ContractorId=?, @ActionId=?, @Activity=?";

    /** CommonServices.GetWagesRate -> BLL GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize. */
    private static final String SQL_WAGES_RATE_LOOKUP =
            "EXEC Sp_InvContractorWagesSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, "
          + "@InvConractorWagesAccountsId=?, @ContractorId=?, @EffectedDate=?, @PackUomFrom=?, @Activity=?";

    public List<Map<String, Object>> getContractors() {
        try {
            return jdbcTemplate.queryForList(SQL_CONTRACTORS,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    "ReadByOrganizationCompanyIdForContractorWages");
        } catch (Exception e) {
            LOG.error("Contractor list failed", e);
            return Collections.emptyList();
        }
    }

    /**
     * BindgrdWagesSchedule(WagesAccountId, ContractorId), form :570-632. Either filter may be 0 -
     * the desktop calls this as soon as EITHER combo has a value (:206, :407, :435), and the BLL
     * simply omits a zero parameter.
     */
    public List<Map<String, Object>> getSchedulesContractorWise(int wagesAccountId, int contractorId) {
        try {
            return jdbcTemplate.queryForList(SQL_SCHEDULES_CONTRACTOR_WISE,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    wagesAccountId, contractorId, 1, "ReadAll");
        } catch (Exception e) {
            LOG.error("Contractor-wise schedule grid failed (account {}, contractor {})",
                      wagesAccountId, contractorId, e);
            return Collections.emptyList();
        }
    }

    /**
     * txtPackUOMFrom_TextChanged / Cmbcontractor_Leave, form :379-400 and :421-432: looks up the
     * schedule rate in force and drops it into Company Rate - but ONLY when it is greater than zero
     * (:391). A miss leaves the box exactly as the user left it; nothing is zeroed or defaulted.
     */
    public Map<String, Object> getWagesRate(String effectedDate, double packUomFrom,
                                            int wagesAccountId, int contractorId) {
        Map<String, Object> out = new HashMap<>();
        out.put("wagesRate", null);
        out.put("scheduleId", null);
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_WAGES_RATE_LOOKUP,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    wagesAccountId, contractorId, date(effectedDate), packUomFrom,
                    "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize");
            if (!rows.isEmpty()) {
                out.put("wagesRate", col(rows.get(0), "WageRate"));
                out.put("scheduleId", col(rows.get(0), "Id"));
            }
        } catch (Exception e) {
            LOG.error("Wages rate lookup failed", e);
        }
        return out;
    }

    /**
     * Insert(), frmContractWiseWagesSchedule.cs :748-800. Differs from the plain schedule in three
     * places only:
     *   ContractorId comes from Cmbcontractor         (:777)
     *   CompanyRate  comes from its OWN box           (:783), not from the Wages Rate box
     *   validation starts with the contractor         (:448-453)
     * RateUom is still fed from the Pack UOM From box (:781), exactly as on the other form.
     */
    public Map<String, Object> saveContractorWiseSchedule(Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();

        int id = asInt(body.get("id"));
        int contractorId = asInt(body.get("contractorId"));
        int wagesAccountId = asInt(body.get("wagesAccountId"));
        double packUomFrom = asDouble(body.get("packUomFrom"));
        double packUomTo = asDouble(body.get("packUomTo"));
        double wagesRate = asDouble(body.get("wagesRate"));
        double companyRate = asDouble(body.get("companyRate"));
        String effectedDate = str(body.get("effectedDate"));
        String effectedDateTo = str(body.get("effectedDateTo"));

        // FormValidation(), :446-479 - messages verbatim, including the lower-case "contractor".
        String err = null;
        if (contractorId == 0)        err = "contractor Field is Required";
        else if (wagesAccountId == 0) err = "Account Field is Required";
        else if (packUomFrom == 0.0)  err = "PackUOMFrom Field Required";
        else if (packUomTo == 0.0)    err = "PackUOMTo Field Required";
        else if (wagesRate == 0.0)    err = "Wages Rate Field Required";
        if (err != null) {
            res.put("success", false);
            res.put("message", err);
            return res;
        }

        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        Timestamp now = new Timestamp(System.currentTimeMillis());
        boolean isUpdate = id > 0;

        try {
            Object[] args = new Object[] {
                    date(effectedDate), date(effectedDateTo),
                    packUomFrom,        // @RateUom     - :781, the PackUomFrom box
                    wagesRate,          // @WageRate
                    packUomFrom,        // @PackUomFrom
                    packUomTo,          // @PackUomTo
                    companyRate,        // @CompanyRate - :783, its own box on THIS form
                    compId, contractorId,
                    id, wagesAccountId, orgId, isUpdate ? 2 : 1,
                    userId, now, userId, now
            };
            jdbcTemplate.update(isUpdate ? SQL_SCHEDULE_UPDATE : SQL_SCHEDULE_INSERT, args);
            res.put("success", true);
            res.put("message", isUpdate ? "Data Update Successfully." : "Data Save Successfully.");
        } catch (Exception e) {
            LOG.error("Contractor-wise schedule save failed", e);
            res.put("success", false);
            res.put("message", e.getMessage());
        }
        return res;
    }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double asDouble(Object o) {
        if (o == null) return 0.0;
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0.0; }
    }

    /** Dates arrive as yyyy-MM-dd from the date inputs; null stays null so the proc sees NULL. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
