package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * DAL of Architecture.WinApp.Templates.DefineOrgnization (ScreenDefinition 402).
 *
 *   BLL 0072 Architecture.BLL.Organization (Save / GetByID / GetAll), DAL 0066 Architecture.DAL.Organization.SetData
 *   BLL 0098 Architecture.BLL.Templates.OrganizationTemplates.GetAllInTemplateRights
 *
 * GenericProvider.SetProc sends every non-virtual property of Architecture.Model.Organization as "@" + name and
 * ADO.NET drops the null ones (OrgLogo and OrgBaseCurr are never set by this form, so they are not sent and
 * Sp_Organization_Update stores NULL in them, exactly as the desktop does).
 */
@Repository
public class AdmOrganizationRepository {

    private final JdbcTemplate jdbc;

    public AdmOrganizationRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** OrganizationTemplates.GetAllInTemplateRights - Sp_OrganizationTemplates_GetAll @Activity='GetAllInTemplateRights'. */
    public List<Map<String, Object>> templates() {
        return DesktopProc.rows(jdbc, "Sp_OrganizationTemplates_GetAll", params("Activity", "GetAllInTemplateRights"));
    }

    /** Organization.GetAll - Sp_Organization_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> organizations() {
        return DesktopProc.rows(jdbc, "Sp_Organization_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** Organization.GetByID - Sp_Organization_GetAllMethod @Id @Activity='ReadById'; null when there is no row. */
    public Map<String, Object> organization(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Organization_GetAllMethod",
                params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * Organization.Save -> DAL SetData: one SqlTransaction; Id == 0 -> Sp_Organization_Insert, else Sp_Organization_Update.
     * The map holds the model's non-null properties in declaration order.
     */
    @Transactional
    public int save(Map<String, Object> model, int id) {
        return DesktopProc.setProc(jdbc, id == 0 ? "Sp_Organization_Insert" : "Sp_Organization_Update", model);
    }
}
