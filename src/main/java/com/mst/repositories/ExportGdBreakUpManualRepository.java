package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 198 "GD Break Up Manual" - Architecture.WinApp.Export.GdBreakUpManual. Data layer:
 * Architecture.BLL.Export.GDBreakUpManual (BLL 0452) / DAL 0504 and Bank.GetAll (BLL 0057).
 * Procedures, read from procdure.utf8.sql on 30-Sep-2026:
 *
 *   Sp_Bank_GetAllMethod 'ReadAll'   @OrganizationId @CompanyId @Activity        BankBind (the form keeps IsHomeland == 'Home Country')
 *   usp_GDBreakUpManual_History      @OrganizationId @CompanyId                  HistoryBind (grdGdBreakUp)
 *   usp_GDBreakUpManual_ReadById     @Id                                         grdGdBreakUp_DoubleClick -> GetByID
 *   usp_GDBreakUpManual_Insert       the 25 non-virtual model properties         Save (ActionTypeId 1 insert / 2 update; RAISERROR texts surface)
 *
 * DAL SetData: one SetProc in a transaction; Convert.ToInt32(ExecuteScalar()) is the new Id on
 * an insert and 0 on an update (the DAL then returns obj.Id).
 */
@Repository
public class ExportGdBreakUpManualRepository {

    private final JdbcTemplate jdbc;

    public ExportGdBreakUpManualRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** Bank.GetAll - Sp_Bank_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** GDBreakUpManual.FormHistory(org, comp). */
    public List<Map<String, Object>> history(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_GDBreakUpManual_History",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GDBreakUpManual.GetByID(Id) - [0] of the list; the BLL throws when nothing comes back. */
    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "usp_GDBreakUpManual_ReadById", params("Id", id));
    }

    /** DAL GDBreakUpManual.SetData(obj, "usp_GDBreakUpManual_Insert"). */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> model) {
        Integer v = DesktopProc.scalar(jdbc, "usp_GDBreakUpManual_Insert", model);
        int num = v == null ? 0 : v;
        if (num > 0) return num;
        Object id = model.get("Id");
        return id instanceof Number ? ((Number) id).intValue() : 0;
    }
}
