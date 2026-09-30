package com.mst.services;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * "Special Rights" toolbar button of the voucher forms (PaymentVoucherNew / ReceiptsVoucherNew btnSpecialRights_Click)
 * = Architecture.WinApp.Special_Rights.Configurations.SpecialSreenRightsUserWise, opened with the form's ScreenId
 * (screen combo disabled) and the logged-in user (user combo disabled unless RoleName == "Admin").
 *
 * BLL Architecture.BLL.SreenRightsUserWise / ScreenDefinition, procedures unchanged:
 *   ScreenNameFill  -> Sp_ScreenDefinition_GetAllMethod @Activity='GetSceensWithSpecialRights'
 *   UserNameFill    -> Sp_UserAccount_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll'
 *   RightNameFill   -> USP_ScreenRightsUserWise_GetAllMethod @ScreenName (the screen row's ScreenNameA), 'GetAllSpecialRights'
 *   FormHistory     -> USP_ScreenRightsUserWise_GetAllMethod 'FormHistory' @OrganizationId @CompanyId @BranchesId [@UserId] [@ScreenId]
 *   GetByID         -> USP_ScreenRightsUserWise_GetAllMethod 'ReadById' @Id
 *   Save            -> USP_ScreenRightsUserWise_Insert (Id 0) / USP_ScreenRightsUserWise_Update, one transaction
 *   SpecialRightsByScreenIdAndUserId (the form's SpecialRightsImplement) -> 'SpecialRightsByScreenIdAndUserId'
 */
@Service
public class SpecialScreenRightsService {

    private static final String PROC = "USP_ScreenRightsUserWise_GetAllMethod";
    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public SpecialScreenRightsService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    public boolean isAdmin() {
        return "Admin".equals(ctx.currentRoleName());
    }

    public Map<String, Object> lookups(int screenId) {
        Map<String, Object> m = new LinkedHashMap<>();
        List<Map<String, Object>> screens = DesktopProc.rows(jdbc, "Sp_ScreenDefinition_GetAllMethod",
                DesktopProc.params("Activity", "GetSceensWithSpecialRights"));
        m.put("screens", screens);
        m.put("users", DesktopProc.rows(jdbc, "Sp_UserAccount_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAll")));
        m.put("rights", rights(screenId));
        m.put("history", history(screenId));
        m.put("userId", ctx.currentUserId());
        m.put("screenId", screenId);
        m.put("isAdmin", isAdmin());
        return m;
    }

    /** RightNameFill: filters by the selected screen row's Cells[2] = ScreenNameA (the ScreenName column). */
    public List<Map<String, Object>> rights(int screenId) {
        if (screenId <= 0) return new ArrayList<>();
        String screenName = null;
        for (Map<String, Object> s : DesktopProc.rows(jdbc, "Sp_ScreenDefinition_GetAllMethod",
                DesktopProc.params("Activity", "GetSceensWithSpecialRights"))) {
            if (toInt(s.get("Id")) == screenId) { screenName = str(s.get("ScreenNameA")); break; }
        }
        if (screenName == null || screenName.isEmpty()) return new ArrayList<>();
        return DesktopProc.rows(jdbc, PROC, DesktopProc.params("ScreenName", screenName, "Activity", "GetAllSpecialRights"));
    }

    /** FormHistory: only ScreenId is set on the desktop (UserId stays 0 -> not sent). */
    public List<Map<String, Object>> history(int screenId) {
        return DesktopProc.rows(jdbc, PROC, DesktopProc.params("Activity", "FormHistory",
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "BranchesId", ctx.currentBranchId(), "ScreenId", screenId != 0 ? screenId : null));
    }

    public Map<String, Object> byId(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, PROC, DesktopProc.params("Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> row = r.get(0);
        if (toInt(row.get("OrganizationId")) != ctx.currentOrganizationId() || toInt(row.get("CompanyId")) != ctx.currentCompanyId())
            throw new IllegalArgumentException("Record not found");
        return row;
    }

    /** SpecialRightsImplement: the logged-in user's rights on that screen (RightId / IsActive). */
    public List<Map<String, Object>> mine(int screenId) {
        return DesktopProc.rows(jdbc, PROC, DesktopProc.params("Activity", "SpecialRightsByScreenIdAndUserId",
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "BranchesId", ctx.currentBranchId(), "UserId", ctx.currentUserId(), "ScreenId", screenId));
    }

    /** Insert(): FormValidation messages, then Save (Insert when Id 0, else Update). */
    @Transactional
    public String save(int id, int screenId, int rightId, int userId, boolean isActive) {
        if (screenId <= 0) throw new IllegalArgumentException("Screen Name Field is Required");
        if (rightId <= 0) throw new IllegalArgumentException("Right Name Field is Required");
        if (userId <= 0) throw new IllegalArgumentException("User Name Field is Required");
        /* The desktop disables the user combo for non-Admin users, so they can only write their own rows. */
        if (!isAdmin() && userId != ctx.currentUserId())
            throw new IllegalArgumentException("Only an Admin user can set special rights for another user.");
        if (id > 0) byId(id); // tenancy check on update
        DesktopProc.setProc(jdbc, id > 0 ? "USP_ScreenRightsUserWise_Update" : "USP_ScreenRightsUserWise_Insert",
                DesktopProc.params("Id", id, "UserId", userId, "ScreenId", screenId, "RightId", rightId, "IsActive", isActive,
                        "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                        "BranchesId", ctx.currentBranchId()));
        return id > 0 ? "Record Update Successfully" : "Record Save Successfully";
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0; }
    }

    private static String str(Object o) { return o == null ? null : o.toString(); }
}
